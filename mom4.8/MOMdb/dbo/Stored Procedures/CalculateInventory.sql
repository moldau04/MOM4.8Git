CREATE PROCEDURE CalculateInventory
as BEGIN
 
UPDATE    tblInventoryWHTrans           set WarehouseID='OFC'where     isnull(WarehouseID,'') = ''

UPDATE tblInventoryWHTrans set LocationID = isnull(LocationID,0 ) where LocationID is null

UPDATE I 
          SET i.hand=(SELECT isnull(sum(isnull(Adj.Hand,0)),0)     FROM tblInventoryWHTrans   adj  WHERE adj.InvID=I.ID) ,  
          I.Balance= (SELECT isnull(sum(isnull(Adj.Balance,0)),0)  FROM tblInventoryWHTrans     adj  WHERE adj.InvID=I.ID) ,  
		  I.Committed= (SELECT isnull(sum(isnull(Adj.Committed,0)),0)  FROM tblInventoryWHTrans   adj  WHERE adj.InvID=I.ID) , 
		  I.fOrder= (SELECT isnull(sum(isnull(Adj.fOrder,0)),0)  FROM tblInventoryWHTrans   adj  WHERE adj.InvID=I.ID) ,
		  I.Available = i.Hand  - i.Committed,
		  I.WarehouseCount =( (SELECT   COUNT(1)     FROM InvWarehouse   adj  WHERE adj.InvID=I.ID)),
		  I.LastUpdateDate=GETDATE()
          FROM  INV I WHERE i.Type=0

TRUNCATE TABLE IWarehouseLocAdj
INSERT INTO IWarehouseLocAdj([InvID] ,[WarehouseID] ,[LocationID] ,[Hand] ,[Balance] ,[fOrder] ,[Committed] ,[Available]) 
SELECT   [InvID] ,[WarehouseID] ,[LocationID] ,sum(isnull([Hand],0)) ,sum(isnull([Balance],0)) ,sum(isnull([fOrder],0)) ,sum(isnull([Committed],0)) ,sum(isnull(Hand,0)) -sum(isnull([Committed],0)) as [Available]	
FROM tblInventoryWHTrans  
GROUP BY [InvID]  ,[WarehouseID] ,[LocationID]  

update Warehouse  set Count=   (SELECT   COUNT(1)     FROM InvWarehouse   adj  WHERE adj.WareHouseID=Warehouse.ID)

 
		 ---- UPDATE LCOST
		update Inv set LCost= CASE WHEN hand > 0 THEN (isnull(Balance,0)/isnull(hand,0)) ELSE 0 END  where type=0
		---

END