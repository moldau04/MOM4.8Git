CREATE PROCEDURE [dbo].[spSearchInventory]	
@Id int= null ,
@EN int= null,
@UserID int,
@SearchField varchar(250),
@SearchValue varchar(250),
@IncludeClose int=1
AS
BEGIN

EXEC CalculateInventory

        DECLARE @QUERY AS VARCHAR(MAX);
        DECLARE @tbl  table (IIID int);
        SET @QUERY= ' select Inv.[ID]  from Inv with (nolock)'	  
	    IF @SearchField = 'ApprovedVendor'
		BEGIN SET @QUERY=@QUERY + ' inner join InvParts on InvParts.ItemID=Inv.ID where Inv.Type=0 AND InvParts.VendorID='+@SearchValue	END
	    ELSE IF @SearchField = 'MPN'
		BEGIN    SET @QUERY=@QUERY + ' inner join InvParts on InvParts.ItemID=Inv.ID where Inv.Type=0 AND InvParts.MPN= '''+ @SearchValue+''''	END
	    ELSE IF @SearchField = 'ApprovedManufacturer'
		BEGIN 	SET @QUERY=@QUERY + ' inner join InvParts on InvParts.ItemID=Inv.ID where Inv.Type=0 AND InvParts.Mfg='''+@SearchValue +''''	END 
	    ELSE BEGIN	SET @QUERY=@QUERY + ' where Inv.Type=0 and '+@SearchField+' like ''%'+@SearchValue+'%'''	   END		 
		INSERT INTO @tbl (iiid)
		EXEC (@QUERY) 

  SET @Id =ISNULL(@Id,0)
		
 SELECT Inv.[ID]
      ,[Name]
      ,[fDesc]
      ,Inv.[Part]
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
      ,Inv.[MPN]
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

	  where   Inv.Type =0 

	  and Inv.ID =  case @Id when 0 then Inv.ID else @Id END  

	  and inv.Status in (0 , @IncludeClose ,2)

	 and Inv.ID in (select * from @tbl)

	  Order by Inv.Name 
	   

 
END