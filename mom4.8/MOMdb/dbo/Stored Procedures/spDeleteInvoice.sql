CREATE PROCEDURE [dbo].[spDeleteInvoice]
	@Ref int,
	@Batch int,
	@Loc int
AS
BEGIN
	
SET NOCOUNT ON;
	
DECLARE @Amount numeric(30,2)
DECLARE @TransID int
DECLARE @job int
DECLARE @Rev numeric(30,2)
DECLARE @Phase int
DECLARE @Period int
DECLARE @OldRetainage numeric(30,2) = 0

BEGIN TRY
BEGIN TRANSACTION
	
	--SELECT @TransID = ID, @Amount = Amount FROM Trans WHERE Batch = @Batch AND Type = 1
	SELECT @TransID = ID, @Amount = Amount ,@Batch =Batch , @loc= AcctSub FROM Trans WHERE Ref = @Ref AND Type = 1
	SELECT @job = Job, @Period = YEAR(fdate) * 100 + MONTH(fdate) FROM Invoice WHERE Ref = @Ref

	CREATE TABLE #tempCode
	( Phase int
	)

	INSERT INTO #tempCode(Phase)
	SELECT Phase FROM JobI WHERE Type = 0 AND TransID >= 0 AND Ref = CONVERT(VARCHAR(50), @Ref) 

	-------------  $$$$ -- REMOVE WIP RETAINAGE ----- $$$$$$$
	SELECT @OldRetainage = SUM(Amount) FROM InvoiceI where Ref = @Ref AND fDesc = 'Retainage'
	IF (@OldRetainage <> 0)
	BEGIN
		UPDATE t1
		SET t1.RetainageBilling = t1.RetainageBilling - @OldRetainage
		FROM ProjectWIPDetail t1
			INNER JOIN ProjectWIP t2 ON t1.WIPID = t2.ID
		WHERE t2.Period = @Period AND t2.IsPost = 0 AND t1.Job = @job
	END

	DELETE FROM Trans WHERE Batch = @Batch
	DELETE FROM InvoiceI WHERE Ref = @Ref 
	DELETE FROM Invoice WHERE Ref = @Ref
	DELETE FROM JobI where Type = 0 and TransID >= 0 and Ref =convert(varchar(50), @Ref)
	DELETE FROM OpenAR WHERE Ref = @Ref AND Type = 0 AND TransID = @TransID

	---------------$$$$ Inventory  Adjustment $$$ --------------------
	DELETE FROM tblInventoryWHTrans WHERE Batch = @Batch AND ScreenID = @Ref AND Screen = 'AR Invoice'
	EXEC CalculateInventory
	------------------------------END------------------------------------

	--UPDATE Ticket . Invoice Field 
	UPDATE TICKETD  SET Charge=1 , Invoice=null  where Invoice=@Ref

	EXEC spUpdateJobRev @job

	DECLARE db_cursor CURSOR FOR 
		SELECT Phase FROM #tempCode

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @Phase

	WHILE @@FETCH_STATUS = 0
	BEGIN  	
		IF @Phase IS NOT NULL
			BEGIN
				
				SET @Rev = isnull((select sum(isnull(amount,0)) from jobi 
												where	type = 0 
													and Job = @Job 
													and Phase = @Phase),0)

				UPDATE JobTItem 
				SET 
					Actual = @Rev
					
				WHERE		Type = 0 
						AND Job = @Job 
						AND Line = @Phase 
			END

		FETCH NEXT FROM db_cursor INTO  @Phase
	END	

	CLOSE db_cursor  
	DEALLOCATE db_cursor

	set @Amount = @Amount * -1
	EXEC spUpdateCustomerLocBalance @Loc, @Amount
	EXEC spCalChartBalance
	DROP TABLE #tempCode

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