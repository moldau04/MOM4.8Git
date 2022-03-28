CREATE PROCEDURE [dbo].[spDeletePO]
	@POID int,
	@POAmount numeric(30,2),
	@PODescription varchar(Max),
	@Status smallint
AS

BEGIN TRY
	BEGIN TRANSACTION
	UPDATE PO SET Amount = @POAmount
		, fDesc = @PODescription
		, Status = @Status WHERE PO = @POID;
	INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
	SELECT POItem.Inv, POItem.WarehouseID , POItem.LocationID ,0,0,ISNULL(POItem.Quan,0) * -1,0,0  ,Screen= 'PO',@POID,'Delete',GETDATE(),'Revert',0,GETDATE() from POItem 
			INNER JOIN BOMT on BOMT.ID=POItem.TypeID --and  BOMT.Type='Inventory'
			where PO =@POID and  BOMT.Type='Inventory' 
	DELETE FROM POItem WHERE PO = @POID;

	UPDATE Chart SET Balance = ISNULL (p.Balance , 0)
    FROM Chart c 
	LEFT JOIN (SELECT Sum(Amount) AS Balance FROM PO) p  
        ON c.DefaultNo = 'D9991' AND Status = 0

	
	EXEC CalculateInventory 
	COMMIT 
END TRY
BEGIN CATCH

	SELECT ERROR_MESSAGE()

	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on store procedure spDeletePO.',16,1)
		RETURN

END CATCH
GO