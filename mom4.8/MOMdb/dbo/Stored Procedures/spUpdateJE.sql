CREATE PROCEDURE [dbo].[spUpdateJE]
	@JEItems tblTypeTrans READONLY,
	@fDate DATETIME,
	@fDesc VARCHAR(8000),
	@Internal VARCHAR(50),
	@Frequency INT = NULL,
	@IsJobSpec BIT,
	@IsRecur BIT,
	@Batch INT = NULL,
	@Ref INT,
	@UpdatedBy varchar(100)
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @IsBankAdj BIT = 0
	DECLARE @AcctID INT
	DECLARE @tDesc VARCHAR(MAX)
	DECLARE @Debit NUMERIC(30,2)
	DECLARE @Credit NUMERIC(30,2)
	DECLARE @Job INT
	DECLARE @Phase INT
	DECLARE @Amount NUMERIC(30,2)
	DECLARE @Loc INT
	DECLARE @Line smallint = 0
	DECLARE @ChartType SMALLINT
	DECLARE @Type SMALLINT
	DECLARE @AcctSub INT
	DECLARE @Bank INT
	DECLARE @TransId INT
	DECLARE @Comm NUMERIC(30,2)
	DECLARE @MatActual NUMERIC(30,2)
	DECLARE @TypeID INT

	DECLARE @CurrfDate DATETIME
	DECLARE @CurrfDesc VARCHAR(8000)
	DECLARE @CurrInternal VARCHAR(50)
	DECLARE @CurrFrequency INT = NULL
	DECLARE @CurrIsJobSpec BIT
	DECLARE @CurrIsRecur BIT
	DECLARE @CurrDebit NUMERIC(30,2)
	DECLARE @CurrCredit NUMERIC(30,2)
	DECLARE @NewCredit NUMERIC(30,2)
	DECLARE @NewDebit NUMERIC(30,2)

	BEGIN TRY
	BEGIN TRANSACTION

	SELECT @NewDebit = Sum(Debit), @NewCredit = Sum(Credit) FROM @JEItems

	IF (@IsRecur = 0)			------------------------ UPDATE JOURNAL ENTRY --------------------------
	BEGIN

		SET @IsBankAdj = isnull((SELECT TOP 1 1 FROM Trans WHERE Batch = @Batch and Type = 30),0)
		
		SELECT @CurrfDate = fDate
			, @CurrfDesc = fDesc
			, @CurrInternal = Internal
			, @CurrDebit = ISNULL((SELECT SUM(Amount) FROM Trans WHERE Batch = GLA.Batch AND Trans.Type in (30,31,50) And Amount > 0),0.00)
			, @CurrCredit = ISNULL((SELECT SUM(Amount * -1) FROM Trans WHERE Batch = GLA.Batch And Trans.Type in (30,31,50) And Amount < 0),0.00)
		FROM GLA WHERE Ref = @Ref

		IF EXISTS (SELECT 1 FROM Trans WHERE VInt > 0 AND (Type = 30 OR Type = 31) AND Ref = @Ref)
			SET @CurrIsJobSpec = 1
		ELSE SET @CurrIsJobSpec = 0

		IF (@IsBankAdj = 1)
		BEGIN


			DECLARE @ID INT
			DECLARE db_cursor CURSOR FOR 

				SELECT ID, Acct, Amount FROM Trans WHERE Batch = @Batch and Type = 30

			OPEN db_cursor  
			FETCH NEXT FROM db_cursor INTO @ID, @AcctID, @Amount

			WHILE @@FETCH_STATUS = 0
			BEGIN
				
				SET @Amount = @Amount * -1

				EXEC spUpdateBankBalance @AcctID, @Amount

			FETCH NEXT FROM db_cursor INTO @ID, @AcctID, @Amount
			END

			CLOSE db_cursor  
			DEALLOCATE db_cursor


		END
		DELETE FROM JobI			WHERE TransID IN (SELECT ID FROM Trans WHERE Batch = @Batch) --Type = 1 AND 
										  
		DELETE FROM TransDeposits	WHERE Batch = @Batch
		DELETE FROM TransChecks		WHERE Batch = @Batch
		DELETE FROM Trans			WHERE Batch = @Batch
		
		UPDATE GLA 
		SET
		Ref = @Ref,
		fDate = @fDate,
		Internal = @Internal,
		fDesc = @fDesc

		WHERE Batch = @Batch 


		SET @IsBankAdj = (SELECT top 1 1 FROM @JEItems as t WHERE (select type from chart where id = t.AcctID) = 6)

		DECLARE db_cursor CURSOR FOR 

		SELECT AcctID, fDesc, Debit, Credit, JobID, PhaseID,TypeID, (select type from chart where id = t.AcctID) as ChartType FROM @JEItems as t

		OPEN db_cursor  
		FETCH NEXT FROM db_cursor INTO @AcctID, @tDesc, @Debit, @Credit, @Job, @Phase,@TypeID, @ChartType

		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @AcctSub = NULL
			SET @Line = @Line + 1

			SET @Type = 50

		
			IF @Debit <> 0 
			BEGIN
				SET @Amount = @Debit
			END
			ELSE
			BEGIN
				SET @Amount = @Credit * -1
			END

			IF @IsBankAdj = 1
			BEGIN
			
				SET @Type = 31
			
				IF @ChartType = 6
				BEGIN

					SET @Type = 30
					SET @AcctSub = (SELECT TOP 1 ID FROM Bank WHERE Chart = @AcctID)

					EXEC spUpdateBankBalance @Bank, @Amount
					EXEC spAddTransBankAdj @Batch, @Bank, 0, @Amount
				END

			END
			
			--IF (@Job IS NOT NULL)
			--BEGIN
			--	SET @AcctSub = (SELECT ISNULL(Loc,0) FROM Job WHERE ID = @Job)

			--END

			EXEC @TransId = AddJournal null, @Batch, @fDate, @Type, @Line, @Ref, @tDesc, @Amount, @AcctID, @AcctSub, null, 0, @Job, @Phase 

			IF (@Job IS NOT NULL)
			BEGIN
					INSERT INTO [dbo].[JobI]
						   ([Job],[Phase],[fDate],[Ref],[fDesc],[Amount],[TransID],[Type],[UseTax],[Billed])
					 VALUES
						   (@Job,@Phase,@fDate,@Ref,@fDesc,(CASE WHEN @TypeID = 1 THEN @Amount ELSE @Amount * -1 END),@TransId,@TypeID,0,1)
					
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

					IF (@AcctID = ISNULL((SELECT ISNULL(ID,0) FROM Chart WHERE DefaultNo='D1200'),0))
					BEGIN
						
						EXEC spUpdateCustomerLocBalance @AcctSub, @Amount
					END
			END

		FETCH NEXT FROM db_cursor INTO @AcctID, @tDesc, @Debit, @Credit, @Job, @Phase,@TypeID, @ChartType
		END

		CLOSE db_cursor  
		DEALLOCATE db_cursor

		EXEC spCalChartBalance

		IF EXISTS (SELECT 1 FROM Trans WHERE VInt > 0 AND (Type = 30 OR Type = 31) AND Ref = @Ref)
			SET @IsJobSpec = 1
		ELSE SET @IsJobSpec = 0
	END
	ELSE						------------------------ UPDATE RECURRING JOURNAL ENTRY --------------------------
	BEGIN
		SELECT @CurrfDate = fDate
			, @CurrfDesc = fDesc
			, @CurrInternal = Internal
			, @CurrFrequency = Frequency
			, @CurrDebit = ISNULL((SELECT SUM(Amount) FROM GLARecurI WHERE Ref = GLARecur.Ref And Amount > 0),0.00)
			, @CurrCredit = ISNULL((SELECT SUM(Amount * -1) FROM GLARecurI WHERE Ref = GLARecur.Ref  And Amount < 0),0.00)
		FROM GLARecur WHERE Ref = @Ref

		IF EXISTS (SELECT 1 FROM GLARecurI WHERE Job > 0 AND Ref = @Ref) 
			SET @CurrIsJobSpec = 1
		ELSE SET @CurrIsJobSpec = 0


		DELETE FROM GLARecurI WHERE Ref = @Ref

		UPDATE GLARecur 
		SET
		
		fDate = @fDate,
		Internal = @Internal,
		fDesc = @fDesc,
		Frequency = @Frequency

		WHERE Ref = @Ref 

		
		DECLARE db_cursor CURSOR FOR 

		SELECT AcctID, fDesc, Debit, Credit, JobID, PhaseID,TypeID FROM @JEItems as t

		OPEN db_cursor  
		FETCH NEXT FROM db_cursor INTO @AcctID, @tDesc, @Debit, @Credit, @Job, @Phase,@TypeID

		WHILE @@FETCH_STATUS = 0
		BEGIN
			
			SET @Line = @Line + 1

			IF @Debit <> 0 
			BEGIN
				SET @Amount = @Debit
			END
			ELSE
			BEGIN
				SET @Amount = @Credit * -1
			END
			
			SET IDENTITY_INSERT GLARecurI ON 

			INSERT INTO GLARecurI (	ID, 
									Ref,
									Line,
									fDesc,
									Amount,
									Acct,
									Job,
									Phase,TypeID) 
			VALUES (		(SELECT ISNULL(MAX(ID),0)+1 FROM GLARecurI),
							@Ref,
							@Line,
							@tDesc,
							@Amount,
							@AcctID,
							@Job,
							@Phase,@TypeID)	

			SET IDENTITY_INSERT GLARecurI OFF 

		FETCH NEXT FROM db_cursor INTO @AcctID, @tDesc, @Debit, @Credit, @Job, @Phase,@TypeID
		END

		CLOSE db_cursor  
		DEALLOCATE db_cursor

		IF EXISTS (SELECT 1 FROM GLARecurI WHERE Job > 0 AND Ref = @Ref) 
			SET @IsJobSpec = 1
		ELSE SET @IsJobSpec = 0

	END

	------------- Logs -----------------
    DECLARE @Screen varchar(100)
	IF(ISNULL(@IsRecur,0) = 0) SET @Screen='Journal Entry';
	ELSE SET @Screen='Recurring Entry';

	Declare @Sdate nvarchar(150)  
	SELECT @Sdate = convert(varchar, @fDate, 101)  
	Declare @SCurrdate nvarchar(150)  
	SELECT @SCurrdate = convert(varchar, @CurrfDate, 101)  
    IF(@Sdate <> @SCurrdate)  
    BEGIN    
	    EXEC log2_insert @UpdatedBy,@Screen,@Ref,'Date',@SCurrdate,@Sdate  
    END  

	IF(ISNULL(@fDesc,'') != ISNULL(@CurrfDesc,''))  
    BEGIN
		EXEC log2_insert @UpdatedBy,@Screen,@Ref,'Description',@CurrfDesc,@fDesc  
    END  

	-- @Frequency
	IF(ISNULL(@IsRecur,0) = 1)
	BEGIN
		IF(ISNULL(@Frequency,-1) != ISNULL(@CurrFrequency,-1))  
		BEGIN
			DECLARE @StrFrequency varchar(50)
			SELECT @StrFrequency = CASE @Frequency WHEN 0 THEN 'Monthly'
				WHEN 1 THEN 'Bi-Monthly'
				WHEN 2 THEN 'Quarterly'
				WHEN 3 THEN '3 Times A Year'
				WHEN 4 THEN 'Semi-Annually'
				WHEN 5 THEN 'Annually'
				WHEN 6 THEN 'Weekly'
				ELSE ''
				END

			DECLARE @StrCurrFrequency varchar(50)
			SELECT @StrCurrFrequency = CASE @CurrFrequency WHEN 0 THEN 'Monthly'
				WHEN 1 THEN 'Bi-Monthly'
				WHEN 2 THEN 'Quarterly'
				WHEN 3 THEN '3 Times A Year'
				WHEN 4 THEN 'Semi-Annually'
				WHEN 5 THEN 'Annually'
				WHEN 6 THEN 'Weekly'
				ELSE ''
				END

			EXEC log2_insert @UpdatedBy,@Screen,@Ref,'Frequency',@StrCurrFrequency,@StrFrequency  
		END
	END

	--@Internal
	IF(ISNULL(@Internal,'') != ISNULL(@CurrInternal,''))  
    BEGIN
		EXEC log2_insert @UpdatedBy,@Screen,@Ref,'Entry Number',@CurrInternal,@Internal  
    END
	--@IsJobSpec
	IF(ISNULL(@IsJobSpec,0) != @CurrIsJobSpec)
	BEGIN
		IF(ISNULL(@IsJobSpec,0) = 0)
		BEGIN
			EXEC log2_insert @UpdatedBy,@Screen,@Ref,'Project Specific','Yes','No'  
		END
		ELSE
		BEGIN
			EXEC log2_insert @UpdatedBy,@Screen,@Ref,'Project Specific','No','Yes'  
		END
	END

	----@IsRecur
	--IF(ISNULL(@IsRecur,0) = 0)
	--BEGIN
	--	EXEC log2_insert @UpdatedBy,@Screen,@Ref,'Is Recurring','','No'  
 --   END
	--ELSE
	--BEGIN
	--	EXEC log2_insert @UpdatedBy,@Screen,@Ref,'Is Recurring','','Yes'  
 --   END

	IF(@CurrCredit <> @NewCredit)  
    BEGIN    
	    EXEC log2_insert @UpdatedBy,@Screen,@Ref,'Credit',@CurrCredit,@NewCredit  
    END
	
	IF(@CurrDebit <> @NewDebit)  
    BEGIN    
	    EXEC log2_insert @UpdatedBy,@Screen,@Ref,'Debit',@CurrDebit,@NewDebit  
    END
	------------- End logs -------------------

	COMMIT 
	END TRY
	BEGIN CATCH

	SELECT ERROR_MESSAGE()

	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

	END CATCH
END