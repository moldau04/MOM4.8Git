CREATE PROCEDURE [dbo].[spGetRPOByID]
	@RPOID int
AS
BEGIN
    -- Table[0]
    SELECT p.PO, p.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, isnull(r.Status,0) as Status, p.Due, p.ShipVia,           
            p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,    
            p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, r.Amount as ReceivedAmount, r.fDate as ReceiveDate, 
            ro.Address +', '+ CHAR(13)+ ro.City +', '+ ro.State+', '+ ro.Zip as Address    
            , v.Type VendorType
    FROM ReceivePO as r with (nolock)                            
        INNER JOIN PO as p with (nolock) ON r.PO=p.PO                 
        INNER JOIN Vendor as v with (nolock) ON p.Vendor = v.ID       
        INNER JOIN Rol as ro with (nolock) ON v.Rol = ro.ID           
    WHERE r.ID = @RPOID


    SELECT p.PO,p.Line, p.Quan, p.fDesc, p.Price, p.Job, p.Phase,    
            p.Rquan, p.Billed, p.Ticket, p.Due, p.GL, p.Freight, p.Inv,J.fDesc As JobName,Loc.Tag As LocationName, 
            p.Amount as Ordered,                       
            p.Selected as PrvIn,                       
            p.Balance as Outstanding,                  
            rp.Amount as Received,                     
            p.Quan as OrderedQuan,                     
            p.SelectedQuan as PrvInQuan,               
            p.BalanceQuan as OutstandQuan,             
            isnull(rp.Quan,0) as ReceivedQuan,p.WarehouseID,p.LocationID,                    
            rp.POLine,                                 
            rp.ReceivePO,rp.IsReceiveIssued ,                              
            isNULL((SELECT top 1  1 FROM INV with (nolock) WHERE ID = (p.Inv)and type = 0),0) IsItemsExistsInInventory  ,                 
            (SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW with (nolock) inner join Warehouse AS Wh with (nolock) on Wh.ID = INW.WarehouseID   where  INW.InvID=p.Inv  and    INW.WareHouseID=p.WarehouseID) As WarehouseName  ,             
            (Select top 1 Name from WHLoc WH with (nolock) where WH.WareHouseID = p.WarehouseID and id = p.LocationID) As WarehouseLoc   ,    
            (SELECT  top 1 isnull(bt.Type, '') as Phase FROM POItem as ppp with (nolock)                
                LEFT JOIN JobTItem as jt with (nolock) ON jt.Line = ppp.Phase and isnull(jt.Job,0) = isnull(j.ID, 0)                 
                INNER JOIN BOM as b with (nolock) ON b.JobTItemID = jt.ID                 
                LEFT JOIN Inv as i with (nolock) on i.ID = ppp.Inv and b.matitem = i.id                 
                inner join BOMT bt with (nolock) on bt.ID = b.Type                 
             WHERE ppp.PO = p.PO and ppp.Line = p.Line  ) as IsInventoryCode                 
    FROM ReceivePO AS r   with (nolock)       
        RIGHT JOIN RPOItem AS rp with (nolock) on rp.ReceivePO = r.ID                 
        INNER JOIN POItem AS p with (nolock) ON p.Line = rp.POLine                    
        left outer JOIN Job AS J with (nolock) ON p.Job = J.ID                  
        left outer JOIN LOC AS LOC with (nolock) ON LOC.Loc = J.Loc                  
    WHERE r.ID = @RPOID and p.PO = r.PO                   
END