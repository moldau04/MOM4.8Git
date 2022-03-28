CREATE PROCEDURE [dbo].[spDeleteJE]
	@Batch INT
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @Job Int
	DECLARE @Phase Int
	DECLARE @Comm numeric(30,2)
	DECLARE @MatActual numeric(30,2)

	CREATE TABLE #temp
	(	Job int,
		Phase int
	)

	INSERT INTO #temp (Job, Phase)
	SELECT Job, Phase FROM JobI WHERE TransID IN (SELECT ID FROM Trans WHERE Batch = @Batch) AND Type = 1

	DELETE FROM JobI WHERE TransID IN (SELECT ID FROM Trans WHERE Batch = @Batch) AND Type = 1

    DELETE FROM GLA WHERE Batch = @Batch

	DELETE FROM Trans WHERE Batch = @Batch

	DELETE FROM TransDeposits WHERE Batch = @Batch
	
	DELETE FROM TransChecks WHERE Batch = @Batch

	DECLARE db_cursor CURSOR FOR 

		SELECT Job, Phase FROM #temp

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @Job, @Phase

	WHILE @@FETCH_STATUS = 0
	BEGIN  	
		IF @Job > 0
			BEGIN

				EXEC spUpdateJobMatExp @Job

			END


		IF @Phase > 0
			BEGIN
				
					SET @Comm = ISNULL((SELECT sum(isnull(p.Balance,0)) from POItem p 
												INNER JOIN PO on p.po = po.po
												WHERE p.Job = @Job and p.Phase = @phase and po.status in (0,3,4)),0) + 
									ISNULL((SELECT sum(isnull(rp.Amount,0)) from RPOItem rp 
												INNER JOIN ReceivePO r on r.ID = rp.ReceivePO
												LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line
												WHERE p.Job = @Job and p.Phase = @Phase and r.status = 0),0)
				
					SET @MatActual = isnull((SELECT sum(isnull(Amount,0)) FROM JobI
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


	FETCH NEXT FROM db_cursor INTO  @Job, @Phase

	END	

	CLOSE db_cursor  
	DEALLOCATE db_cursor

	EXEC spCalCustomerBalance 

	EXEC spCalChartBalance
	DROP TABLE #temp

END