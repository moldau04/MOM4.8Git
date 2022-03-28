CREATE PROCEDURE [dbo].[spGetPOItemByPO]
	@POID int,
	@EN int,
	@UserID int
AS
BEGIN
	--DECLARE	@POID int=9469,
	--@EN int=0,
	--@UserID int=1

	DECLARE @text1 nvarchar(max)
	SET @text1 = 'select poi.PO
					, poi.Line
					, poi.Quan
					, poi.fDesc
					, poi.Price
					, poi.Amount
					, poi.Job
					, poi.Phase
					, poi.due
					, poi.Inv
					, J.fDesc AS JobName
					, Loc.Tag As LocationName ,   
					poi.Amount as Ordered,         
					isnull(poi.Selected,0.00) as PrvIn,                      
					isnull(poi.Balance,poi.Amount) as Outstanding,                
					0.00 as Received,                   
					poi.Quan as OrderedQuan,            
					isnull(poi.SelectedQuan,0.00) as PrvInQuan,      
					isnull(poi.BalanceQuan,poi.Quan) as OutstandQuan,    
					0.00 as ReceivedQuan,0 As IsReceiveIssued               
					, poi.GL
					, poi.Freight
					, poi.Rquan
					, poi.Billed
					, poi.Ticket
					,poi.WarehouseID
					,poi.LocationID ,   
					--isNULL((SELECT  1 FROM INV with (nolock) WHERE ID = (poi.Inv)and type = 0),0) IsItemsExistsInInventory    
					CASE WHEN INV.ID is null THEN 0 ELSE 1 END IsItemsExistsInInventory ,
					case when  isnull(poi.WarehouseID,'''')  != '''' THEN
						(SELECT TOP 1 Wh.Name FROM InvWarehouse As INW with (nolock) inner join Warehouse AS Wh with (nolock) on Wh.ID = INW.WarehouseID   where  INW.InvID=poi.Inv  and    INW.WareHouseID=poi.WarehouseID)
					else '''' end As WarehouseName   ,                          
					case when isnull(poi.WarehouseID,'''')  != '''' and isnull(poi.LocationID,0) != 0 then
						(Select TOP 1  Name from WHLoc WH with (nolock) where WH.WareHouseID = poi.WarehouseID and id = poi.LocationID)
					else '''' end  As WarehouseLoc ,
					(SELECT TOP 1 isnull(bt.Type, '''')                
						FROM POItem as ppp   with (nolock)              
							LEFT JOIN JobTItem as jt with (nolock) ON jt.Line = ppp.Phase and isnull(jt.Job,0) = isnull(j.ID, 0)                
							INNER JOIN BOM as b with (nolock) ON b.JobTItemID = jt.ID                 
							LEFT JOIN Inv as i with (nolock) on i.ID = ppp.Inv and b.matitem = i.id                 
							inner join BOMT bt with (nolock) on bt.ID = b.Type                  
						WHERE ppp.PO = poi.PO and ppp.Line = poi.Line  ) as IsInventoryCode                 
                FROM POItem as poi with (nolock) LEFT JOIN PO as p with (nolock) ON poi.PO = p.PO   
					left outer JOIN Job as J with (nolock) ON poi.Job = J.ID   
					left outer JOIN Loc as Loc with (nolock) ON Loc.Loc = J.Loc   
                    INNER JOIN Vendor as v with (nolock)   ON p.Vendor = v.ID                      
                    INNER JOIN Rol as r with (nolock) ON v.Rol = r.ID   '
	IF @EN = 1
	BEGIN
		SET @text1 +=   ' left outer join tblUserCo UC with (nolock) on UC.CompanyID = r.EN  '
	END
	SET @text1 += ' left outer join Branch B with (nolock) on B.ID = r.EN  
					left join INV on Inv.ID = poi.Inv and Inv.Type = 0 
					WHERE poi.PO = '''+ convert(nvarchar(50), @POID) +''''
        
    IF @EN = 1
	BEGIN        
		SET @text1 += ' AND UC.IsSel = ' + @EN + ' and UC.UserID=  ' + @UserID
	END
                        
    SET @text1 +=  ' ORDER by poi.line '
	print @text1
	exec(@text1) 
END
