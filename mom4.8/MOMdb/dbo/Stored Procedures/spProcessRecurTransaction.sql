CREATE PROCEDURE [dbo].[spProcessRecurTransaction]
	@ref int
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @Processdt datetime
	DECLARE @GLRef int
	DECLARE @fDate datetime
	DECLARE @Frequency int
	DECLARE @Internal varchar(50)
	DECLARE @fDesc varchar(max)
	DECLARE @batch int
	DECLARE @IsBankAdj bit = 0
	DECLARE @Acct int
	DECLARE @AcctSub int
	DECLARE @Total numeric(30,2)
	DECLARE @CountRecur int
	DECLARE @Type smallint
	DECLARE @Line smallint
	DECLARE @Amount numeric(30,2)
	DECLARE @Job int
	DECLARE @Phase int
	DECLARE @Sel smallint
	DECLARE @TypeID int
	DECLARE @TransId INT
	DECLARE @Comm INT
	DECLARE @MatActual INT

	SELECT  @fDate = fDate, @Internal=Internal, @fDesc=fDesc, @Frequency = Frequency
	FROM GLARecur WHERE Ref = @ref  

	SELECT @Processdt = CASE @Frequency
							 WHEN 0 THEN DATEADD(mm, 1, @fDate)
							 WHEN 1 THEN DATEADD(mm, 2, @fDate)
							 WHEN 2 THEN DATEADD(mm, 3, @fDate)
							 WHEN 3 THEN DATEADD(mm, 4, @fDate)
							 WHEN 4 THEN DATEADD(mm, 6, @fDate)
							 WHEN 5 THEN DATEADD(yy, 1, @fDate)
							 WHEN 6 THEN DATEADD(dd, 7, @fDate)
						END

	UPDATE GLARecur
	SET
		fDate = @Processdt
	WHERE Ref = @ref

	SELECT @GLRef = ISNULL(MAX(Ref),0)+1 FROM GLA
	SELECT @batch = ISNULL(MAX(Batch),0)+1 FROM Trans

	----------------Insert GLA----------------
	INSERT INTO [dbo].[GLA]
		([Ref]
		,[fDate]
		,[fDesc]
		,[Internal]
		,[Batch])
	VALUES
		(@GLRef
		,@fDate
		,@fDesc
		,@Internal
		,@batch)

	----------------Check bank Account----------------
	IF EXISTS(SELECT TOP 1 1 FROM GLARecurI g INNER JOIN Chart c ON c.ID = g.Acct WHERE g.Ref = @ref AND c.Type = 6)
	BEGIN
		SET @IsBankAdj = 1
	END
	
	----------------Get GLA Recurring item----------------
	DECLARE db_cursor CURSOR FOR 
	SELECT  
		@batch, 
		@fDate, 
		CASE @IsBankAdj WHEN 1 THEN (CASE c.Type WHEN 6 THEN 30 ELSE 31 END) ELSE 50 END AS Type, 
		g.Line, 
		@GLRef, 
		g.fDesc, 
		g.Amount, 
		g.Acct, 
		NULL AS AcctSub,
		0 AS Sel, 
		g.Job, 
		g.Phase,
		g.TypeID
	FROM GLARecurI g
		LEFT JOIN Chart c ON g.Acct = c.ID
	WHERE Ref = @ref
	ORDER BY Line

	----------------Insert Trans----------------
	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO  @batch, @fDate, @Type, @Line, @GLRef, @fDesc, @Amount, @Acct, @AcctSub, @Sel, @Job, @Phase,@TypeID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @Type = 30
		BEGIN
			SET @AcctSub = (SELECT TOP 1 ID FROM Bank WHERE Chart = @Acct)
		END

		--INSERT INTO Trans(ID, Batch, fDate, Type, Line, Ref, fDesc, Amount, Acct, AcctSub, Sel, VInt, VDoub)
		--values ((SELECT ISNULL(MAX(ID),0)+1 AS MAXID FROM Trans),@batch, @fDate, @Type, @Line,@GLRef, @fDesc, @Amount, @Acct, @AcctSub, @Sel, @Job, @Phase)

		EXEC @TransId = AddJournal null, @batch, @fDate, @Type, @Line, @GLRef, @fDesc, @Amount, @Acct, @AcctSub, null, 0, @Job, @Phase 

		IF (@Job IS NOT NULL)
			BEGIN
					INSERT INTO [dbo].[JobI]
						   ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],[Billed])
					 VALUES
						   (@Job,@Phase,@fDate,@GLRef,@fDesc,(CASE WHEN @TypeID = 1 THEN @Amount ELSE @Amount * -1 END),@TransId,@TypeID,0,1)

					EXEC spUpdateJobcostByJob @Job

					IF (@Phase > 0)
					BEGIN
						SET @Comm = ISNULL((SELECT sum(isnull(p.Balance,0)) from POItem p 
												INNER JOIN PO on p.po = po.po
												WHERE p.Job = @Job and p.Phase = @phase and po.status in (0,3,4)),0) + 
									ISNULL((SELECT sum(isnull(rp.Amount,0)) from RPOItem rp 
												INNER JOIN ReceivePO r on r.ID = rp.ReceivePO
												LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line
												WHERE p.Job = @Job and p.Phase = @Phase and r.status = 0),0)
				
						SET @MatActual = isnull((SELECT sum(isnull(amount,0)) from jobi 
														WHERE Type = 1 
																AND Job = @Job 
																AND Phase = @Phase
																AND (TransID > 0 or isnull(Labor,0) = 0)),0)

						UPDATE JobTItem 
						SET 
							Actual = @MatActual,
							Comm = @Comm												
						WHERE		Type = 1 
								AND Job = @Job 
								AND Line = @Phase
					END

					IF (@Acct = ISNULL((SELECT ISNULL(ID,0) FROM Chart WHERE DefaultNo='D1200'),0))
					BEGIN
						
						EXEC spUpdateCustomerLocBalance @AcctSub, @Amount
					END
			END



		FETCH NEXT FROM db_cursor INTO @batch, @fDate, @Type, @Line, @GLRef, @fDesc, @Amount, @Acct, @AcctSub, @Sel, @Job, @Phase,@TypeID
	END

	CLOSE db_cursor  
	DEALLOCATE db_cursor

	---------------------------------------------
	IF(@IsBankAdj = 1)
		BEGIN
			SELECT @Acct = Acct, @Total = SUM(Amount) 
			FROM Trans 
			WHERE Batch = @batch AND Type = 30
			GROUP BY Batch, Acct

			IF(@Total < 0) SET @Total = @Total * -1

			EXEC spUpdateBankBalance @Acct, @Total
		END
	 
	EXEC spCalChartBalance

	SELECT @CountRecur = COUNT(*) FROM GLARecur WHERE fDate <= GETDATE()

	RETURN @CountRecur
END