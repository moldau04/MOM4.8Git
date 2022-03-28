CREATE PROCEDURE [dbo].[spDeleteDeposit] 
	@id INT,
    @isDeleteReceive BIT
    
AS
BEGIN
	
	SET NOCOUNT ON;
	
	BEGIN TRY
	BEGIN TRANSACTION
	
		DELETE FROM Trans WHERE Type IN (5, 6) AND Ref=@id

		DELETE FROM Trans WHERE ID IN (SELECT TransID FROM OpenAR WHERE Ref = @id and type = 1)

		EXEC spCalChartBalance

		DELETE FROM OpenAR where Ref = @id and Type = 1

		UPDATE r SET Status = 0 
			FROM ReceivedPayment r
			INNER JOIN DepositDetails d ON d.ReceivedPaymentID = r.ID AND d.DepID = @id
			
			IF @isDeleteReceive=1
			BEGIN
				DECLARE @c_ReceiptID INT	
				DECLARE cur_Rec CURSOR FOR 
					SELECT ReceivedPaymentID FROM DepositDetails WHERE DepID = @id
				OPEN cur_Rec  

				FETCH NEXT FROM cur_Rec INTO @c_ReceiptID
			
				WHILE @@FETCH_STATUS = 0  
					BEGIN
						EXEC spDeleteReceivedPayment @c_ReceiptID
						FETCH NEXT FROM cur_Rec INTO @c_ReceiptID
					END	
				CLOSE cur_Rec  
				DEALLOCATE cur_Rec  
            END	
			

		DELETE FROM DepositDetails WHERE DepID = @id

		DELETE FROm Dep WHERE ref = @id

	
	COMMIT 
	END TRY
	BEGIN CATCH
		CLOSE cur_Rec  
		DEALLOCATE cur_Rec  
	SELECT ERROR_MESSAGE()

    IF @@TRANCOUNT>0
        ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
        RETURN

	END CATCH
	
END

