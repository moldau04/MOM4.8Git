CREATE PROCEDURE [dbo].[spUpdateInvWarehouseForAjustments]			
         
			@InvID INT,
			@WarehouseID varchar(5),	
			@locationID INT,
			@OnHand [numeric](30, 2),
			@Balance [numeric](30, 2)= NULL
			
AS
BEGIN
	IF @locationID=0
		BEGIN
			IF not exists(SELECT 1 FROM IWarehouseLocAdj WHERE InvID=@InvID and WareHouseID=@WarehouseID )
				BEGIN
				           IF not exists(SELECT 1 FROM InvWarehouse WHERE InvID=@InvID and WareHouseID=@WarehouseID )
				       BEGIN
								Insert Into InvWarehouse (InvID,WarehouseID)values(@InvID,@WarehouseID)
								Insert Into IWarehouseLocAdj (InvID,WarehouseID,LocationID,Hand,Balance) values(@InvID,@WarehouseID,NULL,@OnHand,@Balance)
				       END
					ELSE
					  BEGIN
							-- INSERT WITH INV ID AND WAREHOUSE ID. LOCATION ID WILL BE NULL
							Insert Into IWarehouseLocAdj (InvID,WarehouseID,LocationID,Hand,Balance) values(@InvID,@WarehouseID,NULL,@OnHand,@Balance)
					  END
				END
			ELSE
				BEGIN
					--- UPDATE WITH INV ID AND WAREHOUSE ID
					Update IWarehouseLocAdj Set Hand=@OnHand,Balance=@Balance where InvID=@InvID and  WareHouseID=@WarehouseID
				END
		END
	ELSE
		BEGIN
			IF not exists(SELECT 1 FROM IWarehouseLocAdj WHERE InvID=@InvID and WareHouseID=@WarehouseID and LocationID= @locationID)
				BEGIN
				  IF not exists(SELECT 1 FROM InvWarehouse WHERE InvID=@InvID and WareHouseID=@WarehouseID )
				       BEGIN
								Insert Into InvWarehouse (InvID,WarehouseID)values(@InvID,@WarehouseID)
								Insert Into IWarehouseLocAdj (InvID,WarehouseID,LocationID,Hand,Balance) values(@InvID,@WarehouseID,@locationID,@OnHand,@Balance)
				       END
				ELSE
					BEGIN
								-- INSERT WITH ALL
								Insert Into IWarehouseLocAdj (InvID,WarehouseID,LocationID,Hand,Balance) values(@InvID,@WarehouseID,@locationID,@OnHand,@Balance)
				    END
			END
			ELSE
				BEGIN
					-- UPDATE WITH ALL 
					Update IWarehouseLocAdj Set Hand=@OnHand,Balance=@Balance where InvID=@InvID and  WareHouseID=@WarehouseID and LocationID= @locationID
				END
		END
END