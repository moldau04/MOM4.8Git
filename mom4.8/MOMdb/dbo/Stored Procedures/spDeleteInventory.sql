CREATE PROCEDURE [dbo].[spDeleteInventory]

@ID int 

AS

DECLARE @RET_VAL VARCHAR(200)
SET @RET_VAL='false'

BEGIN

	IF EXISTS(select Inv from POItem WHERE Inv=@ID)
		BEGIN
			SET @RET_VAL='Selected item is used in PO, it cannot be deleted!'
		END
	
	IF EXISTS( select AcctSub from Trans WHERE AcctSub=@ID)
		BEGIN
			SET @RET_VAL='Selected item is used in Transaction, it cannot be deleted!'
		END
		
IF EXISTS(  select MatItem from BOM WHERE MatItem=@ID)
		BEGIN
			SET @RET_VAL='Selected item is used in BOM, it cannot be deleted!'
		END

IF EXISTS( select Item from IAdj WHERE Item=@ID)
		BEGIN
			SET @RET_VAL='Selected item is used in Inventory Adjustment, it cannot be deleted!'
		END

IF EXISTS( select InvID from IWarehouseLocAdj WHERE InvID=@ID)
		BEGIN
			SET @RET_VAL='Selected item is assigned to warehouse, it cannot be deleted!'
		END

IF EXISTS( select GLRev from Job WHERE GLRev=@ID)
		BEGIN
			SET @RET_VAL='Selected item is used in Job, it cannot be deleted!'
		END

IF EXISTS( select InvID from InvWarehouse WHERE InvID=@ID)
		BEGIN
			SET @RET_VAL='Selected item is assigned to warehouse, it cannot be deleted!'
		END
IF EXISTS( select ItemID from InvParts WHERE ItemID=@ID)
		BEGIN
			SET @RET_VAL='Selected item is assigned to Vendor, it cannot be deleted!'
		END


IF @RET_VAL = 'false'
BEGIN
   Delete from Inv where ID=@ID
END


SELECT @RET_VAL

END

