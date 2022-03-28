CREATE PROCEDURE [dbo].[spDeleteInventoryWareHouse]
@WareHouseID Varchar(20),
@InvID int
as
  BEGIN 
  IF @InvID = 0
	BEGIN
		IF @WareHouseID <> 'OFC'
		BEGIN
			DECLARE @CheckInvItem AS INT
			SET @CheckInvItem =(SELECT COUNT(InvID) FROM IWarehouseLocAdj WHERE WarehouseID = @WareHouseID AND (Hand > 0 OR fOrder > 0 OR Available > 0))
			IF (@CheckInvItem>0)
			BEGIN
				DECLARE @MSG VARCHAR(MAX)
				SET @MSG = 'This warehouse '+ @WareHouseID+' has transactions and cannot be deleted.'
				RAISERROR (@MSG,16,1)
				RETURN -1
			END
			ELSE 
			BEGIN
				SET @CheckInvItem =(SELECT COUNT(PO) FROM POItem WHERE WarehouseID = @WareHouseID)
				IF (@CheckInvItem>0)
				BEGIN
					SET @MSG = 'This warehouse '+ @WareHouseID+' has transactions and cannot be deleted.'
					RAISERROR (@MSG,16,1)
					RETURN -1
				END
				ELSE 
				BEGIN
					Delete from Warehouse where ID=@WareHouseID
					DELETE FROM InvWarehouse where WareHouseID=@WareHouseID
				END
			END
		END
		ELSE 
		BEGIN
			RAISERROR ('This is a default warehouse OFC and cannot be deleted.',16,1)
			RETURN -1
		END
	END
	---------------------------------------
	IF @InvID <> 0
	BEGIN
		IF @WareHouseID <> 'OFC'
		BEGIN
			DECLARE @CheckInvItem1 AS INT
			SET @CheckInvItem1 =(SELECT COUNT(InvID) FROM IWarehouseLocAdj WHERE WarehouseID = @WareHouseID AND InvID = @InvID AND (Hand > 0 OR fOrder > 0 OR Available > 0))
			PRINT @CheckInvItem1
			IF (@CheckInvItem1>0)
			BEGIN
				DECLARE @MSG1 VARCHAR(MAX)
				SET @MSG1 = 'This warehouse '+ @WareHouseID+' has transactions and cannot be deleted.'
				RAISERROR (@MSG1,16,1)
				RETURN -1
			END
			ELSE 
			BEGIN
				--Delete from Warehouse where ID=@WareHouseID

				SET @CheckInvItem1 =(SELECT COUNT(PO) FROM POItem WHERE WarehouseID = @WareHouseID AND Inv = @InvID )
				PRINT @CheckInvItem1
				IF (@CheckInvItem1>0)
				BEGIN
					SET @MSG1 = 'This warehouse '+ @WareHouseID+' has transactions and cannot be deleted.'
					RAISERROR (@MSG1,16,1)
					RETURN -1
				END
				ELSE 
				BEGIN
					DELETE FROM InvWarehouse where WareHouseID=@WareHouseID AND InvID = @InvID
					Print 'Deleted'
				END
			END
		END
		ELSE 
		BEGIN
			RAISERROR ('This is a default warehouse OFC and cannot be deleted.',16,1)
			RETURN -1
		END
	END
	---------------------------------------
  END
GO