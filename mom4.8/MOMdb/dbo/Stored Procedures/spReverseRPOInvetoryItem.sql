CREATE PROCEDURE spReverseRPOInvetoryItem 
@RPOID int,
@UserId varchar(50)
AS 

BEGIN

--***********************************************************************

--SELECT  p.PO, p.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, isnull(r.Status,0) as Status, p.Due, p.ShipVia, 
--p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,  
--p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, r.Amount as ReceivedAmount, r.fDate as ReceiveDate, 
--ro.Address +', '+ CHAR(13)+ ro.City +', '+ ro.State+', '+ ro.Zip as Address INTO #Temp1
--FROM ReceivePO as r with (nolock)                            
--INNER JOIN PO as p with (nolock) ON r.PO=p.PO                
--INNER JOIN Vendor as v with (nolock) ON p.Vendor = v.ID       
--INNER JOIN Rol as ro with (nolock) ON v.Rol = ro.ID          
--WHERE r.ID = @RPOID  
SELECT p.PO,p.Line, p.Quan, p.fDesc, p.Price, p.Job, p.Phase,   
p.Rquan, p.Billed, p.Ticket, p.Due, p.GL, p.Freight, p.Inv,  
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
( SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW with (nolock) inner join Warehouse AS Wh with (nolock) on Wh.ID = INW.WarehouseID   where  INW.InvID=p.Inv  and  
INW.WareHouseID=p.WarehouseID) As WarehouseName  ,           
(Select top 1 Name from WHLoc WH with (nolock) where WH.WareHouseID = p.WarehouseID and id = p.LocationID) As WarehouseLoc   
    INTO #Temp2
FROM ReceivePO AS r   with (nolock)           
RIGHT JOIN RPOItem AS rp with (nolock) on rp.ReceivePO = r.ID     
INNER JOIN POItem AS p with (nolock) ON p.Line = rp.POLine 
INNER JOIN BOMT ON BOMT.ID = p.TypeID
WHERE r.ID = @RPOID and p.PO = r.PO    AND BOMT.Type = 'Inventory'


--***********************************************************************
DECLARE @Batch int
SELECT TOP 1 @Batch =  batch FROM ReceivePO WHERE ID = @RPOID

DECLARE @table table (rowno int identity(1,1),transid int)
INSERT INTO @table(transid)
SELECT ID FROM TRANS WHERE [Batch] =(select Batch from ReceivePO where id=@RPOID AND Batch <> 0)
DECLARE @Rowno int = 1
DECLARE @Rowmax int
SELECT @Rowmax = MAX(transid) FROM @table

WHILE (@Rowno < @Rowmax)
BEGIN
	INSERT INTO Trans ( [Batch]  , [fDate]  , [Type]  , [Line] , [Ref] , [fDesc] , [Amount] , [Acct]  , [AcctSub], [Status] , [Sel] , [VInt] , [VDoub] , [EN] , [strRef]	) 
		SELECT   
			--(SELECT ISNULL(MAX(ID),0)+1 FROM Trans) ID,
			 @Batch, fDate  , [Type]  , [Line]  , [Ref] , 'Deleted Receive PO By '+@UserId , ([Amount] * -1) , [Acct]  , [AcctSub]  
			, cast(  convert(int, (cast( isnull([Status],'0') as numeric(30,2))) * -1) as varchar(10))  
			, [Sel] , [VInt] , [VDoub]  , [EN]   , [strRef] 
	FROM Trans 
	WHERE [Batch] =(select Batch from ReceivePO where id=@RPOID) AND ID = (SELECT transid FROM @table WHERE rowno = @Rowno)
	SET @Rowno +=1;
END
		------------------------------------------- REVERT ENTRY OF PO -----------
	    INSERT INTO tblInventoryWHTrans (InvID,WarehouseID,LocationID,Hand,Balance,fOrder,Committed,Available,Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
		SELECT Inv,WarehouseID,LocationID,0,0,ISNULL(ReceivedQuan,0),0,0,'RPO',@RPOID,'Edit',GETDATE(),'Revert',@Batch,GETDATE() FROM #Temp2 
		WHERE ReceivePO = @RPOID 
		--------------------------------------------
		
		INSERT INTO tblInventoryWHTrans (InvID,WarehouseID,LocationID,Hand,Balance,fOrder,Committed,Available,Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
		SELECT Inv,WarehouseID,LocationID,ISNULL(ReceivedQuan,0)*-1,ISNULL(Received,0)*-1,0,0,0,'RPO',@RPOID,'Edit',GETDATE(),'Revert',@Batch,GETDATE() FROM #Temp2 
		WHERE ReceivePO = @RPOID 

		DROP TABLE #Temp2

		EXEC CalculateInventory
 END