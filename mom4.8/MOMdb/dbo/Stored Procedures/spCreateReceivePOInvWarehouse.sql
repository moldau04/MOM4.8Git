CREATE PROCEDURE [dbo].[spCreateReceivePOInvWarehouse]			
         
			@InvID INT,
			@WarehouseID varchar(5),	
			@locationID INT,
			@OnHand [numeric](30, 2),
			@Balance [numeric](30, 2)= NULL,
			@fOrder [numeric](30, 2)= NULL,
			@Committed [numeric](30, 4)= NULL,
			@Available [numeric](30, 4)= NULL
			
AS
BEGIN
	IF @locationID=0
		BEGIN
			IF not exists(SELECT 1 FROM IWarehouseLocAdj WHERE InvID=@InvID and WareHouseID=@WarehouseID )
				BEGIN
				           IF not exists(SELECT 1 FROM InvWarehouse WHERE InvID=@InvID and WareHouseID=@WarehouseID )
				       BEGIN
								Insert Into InvWarehouse (InvID,WarehouseID)values(@InvID,@WarehouseID)
								Insert Into IWarehouseLocAdj (InvID,WarehouseID,LocationID,Hand,Balance,fOrder,[Committed],Available) values(@InvID,@WarehouseID,NULL,@OnHand,@Balance,@fOrder,@Committed,@Available)
				       END
					ELSE
					  BEGIN
							-- INSERT WITH INV ID AND WAREHOUSE ID. LOCATION ID WILL BE NULL
							Insert Into IWarehouseLocAdj (InvID,WarehouseID,LocationID,Hand,Balance,fOrder,[Committed],Available) values(@InvID,@WarehouseID,NULL,@OnHand,@Balance,@fOrder,@Committed,@Available)
					  END
				END
			ELSE
				BEGIN
					--- UPDATE WITH INV ID AND WAREHOUSE ID
					Update IWarehouseLocAdj Set Hand=Hand+@OnHand, Balance=Balance+@Balance,forder=@fOrder,[Committed]=[Committed]+@Committed,Available=@Available where InvID=@InvID and  WareHouseID=@WarehouseID
				END
		END
	ELSE
		BEGIN
			IF not exists(SELECT 1 FROM IWarehouseLocAdj WHERE InvID=@InvID and WareHouseID=@WarehouseID and LocationID= @locationID)
				BEGIN
				  IF not exists(SELECT 1 FROM InvWarehouse WHERE InvID=@InvID and WareHouseID=@WarehouseID )
				       BEGIN
								Insert Into InvWarehouse (InvID,WarehouseID)values(@InvID,@WarehouseID)
								Insert Into IWarehouseLocAdj (InvID,WarehouseID,LocationID,Hand,Balance,fOrder,[Committed],Available) values(@InvID,@WarehouseID,@locationID,@OnHand,@Balance,@fOrder,@Committed,@Available)
				       END
				ELSE
					BEGIN
								-- INSERT WITH ALL
								Insert Into IWarehouseLocAdj (InvID,WarehouseID,LocationID,Hand,Balance,fOrder,[Committed],Available) values(@InvID,@WarehouseID,@locationID,@OnHand,@Balance,@fOrder,@Committed,@Available)
				    END
			END
			ELSE
				BEGIN
					-- UPDATE WITH ALL 
					Update IWarehouseLocAdj Set  Hand=Hand+@OnHand,Balance=Balance+@Balance,forder=@fOrder,[Committed]=[Committed]+@Committed,Available=@Available where InvID=@InvID and  WareHouseID=@WarehouseID and LocationID= @locationID
				END
		END
END


-----------------START $$$  UPDATE INV HAND AND Balance WHEN  PO Recived $$$---------->
	IF EXISTS (SELECT  1 FROM inv WHERE ID = @InvID) -- CHECK IF ITEM  IS EXISTS!
	BEGIN
	UPDATE i SET i.Hand= (isnull(I.Hand,0)+isnull(@OnHand,0)) , I.Balance =isnull(I.Balance,0)+ isnull(@Balance,0)
	FROM   inv I  
	WHERE I.ID=@InvID and i.Type=0
	END
GO

