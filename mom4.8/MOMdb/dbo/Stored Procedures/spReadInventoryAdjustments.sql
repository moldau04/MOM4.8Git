CREATE PROCEDURE [dbo].[spReadInventoryAdjustments]

			@ID [int]=null,
			@stdateDate datetime=null,
		    @enddateDate datetime=null,
			@EN int =null,
			@UserID int
		

AS
	begin
	
		if(@ID is null and @stdateDate is null and @enddateDate is null)
			SELECT IAdj.ID, IAdj.Quan, CONVERT(VARCHAR(10),IAdj.fDate,101) as fDate, LEFT(IAdj.fDesc,100) AS fDesc, IAdj.Amount, Inv.Name 
			FROM IAdj INNER JOIN Inv ON IAdj.Item=Inv.ID  ORDER BY IAdj.fDate, IAdj.ID
		else if(@ID is null and @stdateDate is not null and @enddateDate is not null)
		BEGIN
				IF(@EN=1)
				Begin
			 SELECT IAdj.ID, IAdj.Quan, CONVERT(VARCHAR(10),IAdj.fDate,101) as fDate, LEFT(IAdj.fDesc,100) AS fDesc, IAdj.Amount, Inv.Name,INV.fDesc Itemsfdesc,IAdj.WarehouseID, LTRIM(RTRIM(B.Name)) As Company,Warehouse.EN ,Warehouse.Name as WHName ,WLoc.Name WHLoc  
			FROM IAdj INNER JOIN Inv ON IAdj.Item=Inv.ID  
			
			left outer join IWarehouseLocAdj IWarehouseLocAdj on IWarehouseLocAdj.InvID=IAdj.Item 
               and isnull(IWarehouseLocAdj.WarehouseID,0) = isnull(IAdj.WarehouseID,0)
                and isnull(IWarehouseLocAdj.LocationID,0) = isnull(IAdj.LocationID,0)
				left outer join Warehouse Warehouse on Warehouse.ID=IWarehouseLocAdj.WarehouseID 
				left outer join WHLoc Wloc on Wloc.ID=IWarehouseLocAdj.LocationID 
				left outer join Branch B on B.ID = Warehouse.EN
				left outer join tblUserCo UC on UC.CompanyID = Warehouse.EN
			
			WHERE IAdj.fDate>=@stdateDate AND IAdj.fDate<=@enddateDate and UC.IsSel=1 and Uc.UserID=@UserID  AND ISNULL(IAdj.Type,0)=0
			ORDER BY IAdj.fDate, IAdj.ID
			end
			else
			Begin
			 SELECT IAdj.ID, IAdj.Quan, CONVERT(VARCHAR(10),IAdj.fDate,101) as fDate, LEFT(IAdj.fDesc,100) AS fDesc, IAdj.Amount, Inv.Name , INV.fDesc Itemsfdesc,IAdj.WarehouseID, LTRIM(RTRIM(B.Name)) As Company,Warehouse.EN  ,Warehouse.Name as WHName ,WLoc.Name WHLoc  
			FROM IAdj INNER JOIN Inv ON IAdj.Item=Inv.ID  
			
			left outer join IWarehouseLocAdj IWarehouseLocAdj on IWarehouseLocAdj.InvID=IAdj.Item 
               and isnull(IWarehouseLocAdj.WarehouseID,0) = isnull(IAdj.WarehouseID,0)
                and isnull(IWarehouseLocAdj.LocationID,0) = isnull(IAdj.LocationID,0)
				left outer join Warehouse Warehouse on Warehouse.ID=IWarehouseLocAdj.WarehouseID 
				left outer join WHLoc Wloc on Wloc.ID=IWarehouseLocAdj.LocationID 
				left outer join Branch B on B.ID = Warehouse.EN

			
			WHERE IAdj.fDate>=@stdateDate AND IAdj.fDate<=@enddateDate AND ISNULL(IAdj.Type,0)=0 ORDER BY IAdj.fDate, IAdj.ID
			end
			END

        else if(@ID is not null) 
			--SELECT IAdj.ID, IAdj.Quan, CONVERT(VARCHAR(10),IAdj.fDate,101) as fDate, LEFT(IAdj.fDesc,100) AS fDesc, IAdj.Amount, Inv.Name 
			--FROM IAdj INNER JOIN Inv ON IAdj.Item=Inv.ID  WHERE IAdj.ID=@ID
			    SELECT IAdj.ID As AdjID,IAdj.Quan As Quantity, CONVERT(VARCHAR(10),IAdj.fDate,101) as fDate, LEFT(IAdj.fDesc,100) AS fDesc, IAdj.Amount,IAdj.TransID, 
				IAdj.Batch,IAdj.Acct,IAdj.WarehouseID,IAdj.LocationID,c.ID As ChartID,c.fDesc As Chart,IWarehouseLocAdj.Hand,
                IWarehouseLocAdj.Balance,Inv.ID As InvID,Inv.Name AS ItemName, Inv.fDesc As InvDesc ,Warehouse.Name As WarehouseName,WHLoc.Name As LocationName,LTRIM(RTRIM(B.Name)) As Company,Warehouse.EN
				FROM IAdj IAdj 
                inner join Chart c on c.ID=IAdj.Acct
               left outer join IWarehouseLocAdj IWarehouseLocAdj on IWarehouseLocAdj.InvID=IAdj.Item 
               and isnull(IWarehouseLocAdj.WarehouseID,0) = isnull(IAdj.WarehouseID,0)
                and isnull(IWarehouseLocAdj.LocationID,0) = isnull(IAdj.LocationID,0)
				left outer join Warehouse Warehouse on Warehouse.ID=IWarehouseLocAdj.WarehouseID 
				left outer join WHLoc WHLoc on WHLoc.ID=IWarehouseLocAdj.LocationID 
				left outer join Branch B on B.ID = Warehouse.EN
                  inner join Inv Inv on Inv.ID=IAdj.Item

                  WHERE IAdj.ID = @ID

	end
