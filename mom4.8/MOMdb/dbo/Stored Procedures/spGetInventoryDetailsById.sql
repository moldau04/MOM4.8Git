CREATE PROCEDURE [dbo].[spGetInventoryDetailsById]
@Id int ,
@EN int= null,
@UserID int

as



	begin
		select [ID]
      ,[Name]
      ,[fDesc]
      ,[Part]
      ,[Status]
      ,[SAcct]
      ,[Measure]
      ,[Tax]
      ,[Balance]
      ,[Price1]
      ,[Price2]
      ,[Price3]
      ,[Price4]
      ,[Price5]
      ,[Remarks]
      ,[Cat]
      ,[LVendor]
      ,[LCost]
      ,[AllowZero]
      ,[Type]
      ,[InUse]
      ,[EN]

		
      ,[Hand]
      ,[Aisle]
     ,[fOrder]
      ,[Min]
      ,[Shelf]
      ,[Bin]
      ,[Requ]
      ,[Warehouse]
      ,[Price6]
     ,[Committed]
      ,[QBInvID]
      ,[LastUpdateDate]
      ,[QBAccountID]
      ,[Available]
      ,[IssuedOpenJobs]
      ,[Description2]
      ,[Description3]
      ,[Description4]
	  ,CONVERT(VARCHAR(10), DateCreated, 101) AS [DateCreated]
      ,[Class]
      ,[Specification]
      ,[Specification2]
      ,[Specification3]
      ,[Specification4]
      ,[Revision]
      ,CONVERT(VARCHAR(10), LastRevisionDate, 101) AS [LastRevisionDate] 
      --,[Eco]
      --,[Drawing]
      ,[Reference]
      ,[Length]
      ,[Width]
      ,[Weight]
      ,[InspectionRequired]
      ,[CoCRequired]
      ,[ShelfLife]
      ,[SerializationRequired]
      ,[GLcogs]
      ,[GLPurchases]
      ,[ABCClass]
      ,[OHValue]
      ,[OOValue]
      ,[OverIssueAllowance]
      ,[UnderIssueAllowance]
      ,[InventoryTurns]
      ,[MOQ]
	   ,[EOQ]
      ,[MinInvQty]
      ,[MaxInvQty]
      ,[Commodity]
      ,CONVERT(VARCHAR(10), LastReceiptDate, 101) AS [LastReceiptDate]
      ,[MPN]
      ,[ApprovedManufacturer]
      ,[ApprovedVendor]
      ,[EAU]
      ,CONVERT(VARCHAR(10), EOLDate, 101) AS [EOLDate]
      ,WarrantyPeriod--CONVERT(VARCHAR(10), WarrantyPeriod, 101) AS [WarrantyPeriod] 
      ,CONVERT(VARCHAR(10), PODueDate, 101) AS [PODueDate] 
      ,[DefaultReceivingLocation]
      ,[DefaultInspectionLocation]
      ,[LastSalePrice]
      ,[AnnualSalesQty]
      ,[AnnualSalesAmt]
      ,[QtyAllocatedToSO]
      ,[MaxDiscountPercentage],[Height],[UnitCost],[GLSales],[leadTime],[DateLastPurchase]  from Inv with (nolock) where Inv.ID=@Id and Inv.Type=0




	  select ID,ItemID,MPN,Part,Supplier,VendorID,Price,Mfg,MfgPrice from InvParts 
		where ItemID =@Id


		 select ID,InvID,Date,Version,Comment,Eco,Drawing from ItemRev 
		where InvID =@Id

		--select Inmerge.ID,Inmerge.InvID,Inmerge.WareHouseID,ww.Name As WarehouseName  from InvWarehouse Inmerge
		--inner join Warehouse ww on ww.ID=Inmerge.WareHouseID
		--where InvID =@Id

		if(@EN=1)
		Begin
	   select Inmerge.ID,Inmerge.InvID,Inmerge.WareHouseID,ww.Name As WarehouseName,
 (case when (select SUM(Hand) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) is null then 0.00 else (select SUM(Hand) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) end) As Hand,
 (case when (select SUM(Balance) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) is null then 0.00 else (select SUM(Balance) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) end) As Balance,
 (case when (select sum(QtyRequired) AS Quantity from BOM where MatItem=Inmerge.InvID group by MatItem) is null then 0.00 else (select sum(QtyRequired) AS Quantity from BOM where MatItem=Inmerge.InvID group by MatItem) end) As [Committed],
 (case when (select sum( POItem.BalanceQuan) as OnOrder from PO left outer join POItem on PO.PO=POItem.PO where PO.Status in (0,4) and POItem.Inv=Inmerge.InvID and POItem.WarehouseID=Inmerge.WareHouseID group by POItem.Inv) 
  is null then 0.00 else (select sum( POItem.BalanceQuan) as OnOrder from PO left outer join POItem on PO.PO=POItem.PO where PO.Status in (0,4) and POItem.Inv=Inmerge.InvID and POItem.WarehouseID=Inmerge.WareHouseID group by POItem.Inv) end) As fOrder,
  
  (case when (select SUM(Available) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) is null then 0.00 else (select SUM(Available) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) end) As Available,
  LTRIM(RTRIM(B.Name)) As Company,ww.EN
  from InvWarehouse Inmerge
	inner join Warehouse ww on ww.ID=Inmerge.WareHouseID
	left outer join Branch B on B.ID = ww.EN
	left outer join tblUserCo UC on UC.CompanyID = ww.EN
	where Inmerge.InvID=@Id and UC.IsSel=1 and UC.UserID=@UserID

	END
	ELSE
	Begin
	--   select Inmerge.ID,Inmerge.InvID,Inmerge.WareHouseID,ww.Name As WarehouseName,
 --(case when (select SUM(Hand) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) is null then 0.00 else (select SUM(Hand) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) end) As Hand,
 --(case when (select SUM(Balance) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) is null then 0.00 else (select SUM(Balance) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) end) As Balance,
 --(case when (select sum(QtyRequired) AS Quantity from BOM where MatItem=Inmerge.InvID group by MatItem) is null then 0.00 else (select sum(QtyRequired) AS Quantity from BOM where MatItem=Inmerge.InvID group by MatItem) end) As [Committed],
 --(case when (select sum( POItem.BalanceQuan) as OnOrder from PO left outer join POItem on PO.PO=POItem.PO where PO.Status in (0,4) and POItem.Inv=Inmerge.InvID and POItem.WarehouseID=Inmerge.WareHouseID group by POItem.Inv) 
 -- is null then 0.00 else (select sum( POItem.BalanceQuan) as OnOrder from PO left outer join POItem on PO.PO=POItem.PO where PO.Status in (0,4) and POItem.Inv=Inmerge.InvID and POItem.WarehouseID=Inmerge.WareHouseID group by POItem.Inv) end) As fOrder,
 
 -- (case when (select SUM(Available) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) is null then 0.00 else (select SUM(Available) from IWarehouseLocAdj WHERE InvID=Inmerge.InvID and WarehouseID=Inmerge.WareHouseID) end) As Available,
 -- LTRIM(RTRIM(B.Name)) As Company,ww.EN
 -- from InvWarehouse Inmerge
	--inner join Warehouse ww on ww.ID=Inmerge.WareHouseID
	--left outer join Branch B on B.ID = ww.EN
	--where Inmerge.InvID=@Id

	------------------------------------------Changes ------------------------------
	SELECT Inmerge.ID,Inmerge.InvID,Inmerge.WareHouseID,ww.Name As WarehouseName,
	(SELECT SUM(ISNULL(Hand,0)) FROM tblInventoryWHTrans WHERE InvID = Inmerge.InvID AND WarehouseID = Inmerge.WareHouseID ) as Hand,
	(SELECT SUM(ISNULL(Balance,0)) FROM tblInventoryWHTrans WHERE InvID = Inmerge.InvID AND WarehouseID = Inmerge.WareHouseID ) as Balance,(SELECT SUM(ISNULL(fOrder,0)) FROM tblInventoryWHTrans WHERE InvID = Inmerge.InvID AND WarehouseID = Inmerge.WareHouseID ) as fOrder,(SELECT SUM(ISNULL(Committed,0)) FROM tblInventoryWHTrans WHERE InvID = Inmerge.InvID AND WarehouseID = Inmerge.WareHouseID ) as Committed,
	(SELECT SUM(ISNULL(Hand,0)) FROM tblInventoryWHTrans WHERE InvID = Inmerge.InvID AND WarehouseID = Inmerge.WareHouseID ) -
	(SELECT SUM(ISNULL(Committed,0)) FROM tblInventoryWHTrans WHERE InvID = Inmerge.InvID AND WarehouseID = Inmerge.WareHouseID )
	 as Available,	LTRIM(RTRIM(B.Name)) As Company,ww.EN
	FROM InvWareHouse  	
	Inmerge
	inner join Warehouse ww on ww.ID=Inmerge.WareHouseID
	left outer join Branch B on B.ID = ww.EN
	where Inmerge.InvID=@Id
	

	END


	end
