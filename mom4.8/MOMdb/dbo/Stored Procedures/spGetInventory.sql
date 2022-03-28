CREATE PROCEDURE [dbo].[spGetInventory]
@Id int= null ,
@EN int= null,
@UserID int ,
@IncludeClose int=1
AS
 
EXEC CalculateInventory


create table #tempSearch
(
	Id int identity(1,1),
	DisplayName nvarchar(100),
	MappingColumn nvarchar(100)
)

insert into #tempSearch
select N'Select','Select'
insert into #tempSearch
select N'Part Number','Name'
insert into #tempSearch
select N'Description','fDesc'
--insert into #tempSearch
--select N'Specification','Specification'
insert into #tempSearch
select N'Status','Status'
insert into #tempSearch
select N'Date Created','DateCreated'
insert into #tempSearch
select N'ABC Class','ABCClass'
i--nsert into #tempSearch 
insert into #tempSearch
select N'Shelf Life','ShelfLife'
insert into #tempSearch
select N'Commodity','Commodity'
insert into #tempSearch
select N'MPN','MPN'
insert into #tempSearch
select N'Manufacturer','ApprovedManufacturer'
insert into #tempSearch
select N'Vendor','ApprovedVendor'
 

 set @Id =ISNULL(@Id,0)
		
 select Inv.[ID]
      ,[Name]
      ,[fDesc]
      ,[Part]
      ,case when [Status]=0 then 'Active' else 'Inactive' end as [StrStatus]
	  ,[Status]
      ,[SAcct]
      ,[Measure]
      ,[Tax] 
      ,[Price1]
      ,[Price2]
      ,[Price3]
      ,[Price4]
      ,[Price5]
      ,Inv.[Remarks]
      ,[Cat]
      ,[LVendor]
      ,[LCost]
      ,[AllowZero]
      ,Inv.[Type]
      ,[InUse]
      ,[EN] 
      ,[Aisle] 
      ,[Min]
      ,[Shelf]
      ,[Bin]
      ,[Requ]
      ,[Warehouse]
      ,[Price6] 
      ,[QBInvID]
      ,[LastUpdateDate]
      ,[QBAccountID] 
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
      ,[Eco]
      ,[Drawing]
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
      ,WarrantyPeriod 
      ,CONVERT(VARCHAR(10), PODueDate, 101) AS [PODueDate] 
      ,[DefaultReceivingLocation]
      ,[DefaultInspectionLocation]
      ,[LastSalePrice]
      ,[AnnualSalesQty]
      ,[AnnualSalesAmt]
      ,[QtyAllocatedToSO]
      ,[MaxDiscountPercentage],[Height] 
	  ,[GLSales],[leadTime],[DateLastPurchase] 
	  , isnull(inv.WarehouseCount,0)  As WarehouseCount 
	  , isnull(Inv.Hand,0) As Hand
	  ,isnull(Inv.Balance,0) As Balance
	  ,isnull(Inv.fOrder,0) As fOrder
      ,isnull(Inv.Committed,0) As [Committed] 
      ,isnull(Inv.Available,0) As Available   
	 , cast ( ( isnull(Inv.Balance,0) ) 
	 /  ( case isnull(Inv.Hand,0) when 0 then 1 
	 else isnull(Inv.Hand,1) end  )  as money  )  
	 AS UnitCost  
	, IT.Type As catName  
	  
	  from Inv with (nolock) 
	  
	  left outer join IType  IT with (nolock)  on IT.ID=inv.Cat
	  
	  where   Inv.Type=0 

	  and Inv.ID = (case @Id when 0 then Inv.ID else @Id END) 

	  and inv.Status in (0 , @IncludeClose)

	  Order by Inv.Name
  
      Select * from #tempSearch  

     drop table #tempSearch
