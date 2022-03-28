CREATE PROCEDURE [dbo].[spVoidInvoice]
		@Ref int,
		@VoidedDate VARCHAR(15),
		@Username VARCHAR(50)
AS
BEGIN
SET NOCOUNT ON;
BEGIN TRY
BEGIN TRANSACTION

	DECLARE @Batch INT
	DECLARE @TransID INT
	DECLARE @Amount NUMERIC(30,2)
	DECLARE @fDesc VARCHAR(MAX)
	DECLARE @loc INT
	DECLARE @job INT
	
	SELECT @fDesc = fDesc, @Batch = Batch, @TransID = TransID, @loc = Loc, @Amount = total , @job=Job FROM Invoice WHERE Ref=@Ref

	UPDATE Invoice
	SET		Status = 2,
			fDesc = 'Voided on '+@VoidedDate+' by user '+@Username+' - '+@fDesc,
			Amount = 0,
			Stax = 0,
			Total = 0,
			GTax=0
			WHERE Ref = @Ref
	
	UPDATE [dbo].[Trans]
			SET fDesc = 'Void '+@fDesc, Amount = 0, Status = 'Void', Sel = 2
			WHERE ID = @TransID

	SET @Amount = @Amount * -1;
	
		
	DELETE FROM Trans WHERE Batch = @Batch AND Type in (2,3,4) 
	
	DELETE FROM [dbo].[InvoiceI] WHERE Ref = @Ref

	DELETE FROM JobI WHERE Ref = convert(varchar(50),@Ref) AND Type = 0 AND TransID > 0
	
	DELETE FROM OpenAR WHERE Ref = @Ref AND Type = 0 

	---------------$$$$ Inventory  Adjustment $$$ ------------------------>
	DELETE FROM tblInventoryWHTrans WHERE Batch = @Batch AND ScreenID = @Ref AND Screen = 'AR Invoice'
	EXEC CalculateInventory
	-------------------------------END------------------------------------>

		--UPDATE Ticket . Invoice Field 
	
	UPDATE TICKETD  SET Charge=1 , Invoice=null  where Invoice=@Ref

	EXEC spCalChartBalance
	EXEC spUpdateCustomerLocBalance @loc,@Amount;	-- Update Owner, Location balance
	EXEC spUpdateJobRev @job	

	---Update Wip
	DECLARE @WipID INT 
	Declare @JobID int
	SET @WipID=isnull((SELECT ID FROM WIPHeader	 WHERE InvoiceId=convert(varchar(50),@Ref)),0)
	SET @JobID=isnull((SELECT JobId FROM WIPHeader	 WHERE InvoiceId=convert(varchar(50),@Ref)),0)
	IF (@WipID<>0)
	BEGIN
	    DECLARE @c_Line INT
        DECLARE @c_TotalBill numeric(30,2)	
		DECLARE cur_Items CURSOR FOR
			SELECT Line,TotalBilled FROM WIPDetails	WHERE WIPId=@WipID
		OPEN cur_Items  
		FETCH NEXT FROM cur_Items INTO @c_Line,@c_TotalBill
		WHILE @@FETCH_STATUS = 0  
			BEGIN
				Update WIPDetails
				SET
				PreviousBilled=PreviousBilled-@c_TotalBill,
				BalanceToFinsh=BalanceToFinsh+@c_TotalBill,
				TotalBilled= TotalBilled-@c_TotalBill,
				TotalCompletedAndStored=TotalCompletedAndStored-@c_TotalBill
				where WIPId in(select Id from WIPHeader where Id>@WipID and JobId=@JobID) AND Line=@c_Line
			FETCH NEXT FROM cur_Items INTO @c_Line,@c_TotalBill
			END	
		CLOSE cur_Items  
		DEALLOCATE cur_Items  	
		
	UPDATE WIPDetails
	set CompletedThisPeriod=0
	,TotalCompletedAndStored=0
	,BalanceToFinsh=0
	,TotalBilled=0
	,RetainageAmount=0
	,RetainagePer=0
	,PerComplete=0
	where WIPId=@WipID 
	
    END 
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
GO

