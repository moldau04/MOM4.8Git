 
    ----------------------NEED TO RUN ON LIVE SERVER
	 ------  OLD DATA MIGRATION  
   
   --INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)

-- SELECT  Trans.AcctSub as INVID, 'OFC' ,0 , cast ( Trans.Status as numeric(30,2))  ,Trans.Amount , 0,0 ,0 
-- ,case type 
-- when 60 then 'Item Adjustment' 
-- when 97 then 'Inventory Item Transfer'
-- when 81 then 'RPO'
-- when 41 then 'APBill'
-- when 70 then 'PostToProject/InventoryUsed' end ,

-- case type 
-- when 60 then   cast( trans.Ref as varchar (100))
-- when 97 then   cast( trans.Ref as varchar (100))
-- when 81 then   (select top 1 ID from ReceivePO where ref=trans.strRef)
-- when 41 then    (select top 1 ID from pj where Batch=trans.Batch)
-- when 70 then   cast( trans.Ref as varchar (100))
--  end ,
 
--  'Add' , Trans.fDate , 'In' ,Trans.Batch , Trans.fDate  
--FROM Trans 
--WHERE Trans.Acct=3  
--AND Trans.fDate>='13/02/1989' 
--AND Trans.fDate<='12/30/2020' 
--and  Trans.Type   in  (41 , 60 ,  70 , 97) 
--and Trans.AcctSub  is not null
--and trans.acctsub in (select id from inv where type=0)
--order by trans.id 


----RPO if bill not generated

--    update RP set RP.batch=(  select top 1 batch from Trans where fDesc='Inventory Recieve PO' and Type=81  and strRef=RP.Ref) 
--  from  ReceivePO RP  where isnull(RP.batch,0)=0


       
--	SELECT p.PO,p.Line, p.Quan, p.fDesc, p.Price, p.Job, p.Phase,   
--p.Rquan, p.Billed, p.Ticket, p.Due, p.GL, p.Freight, p.Inv,  
--p.Amount as Ordered,  
--p.Selected as PrvIn, 
--p.Balance as Outstanding,  
--rp.Amount as Received,
--p.Quan as OrderedQuan, 
--p.SelectedQuan as PrvInQuan,  
--p.BalanceQuan as OutstandQuan,  
--isnull(rp.Quan,0) as ReceivedQuan,p.WarehouseID,p.LocationID,  
--rp.POLine,                                   
--rp.ReceivePO,rp.IsReceiveIssued ,            
--isNULL((SELECT top 1  1 FROM INV with (nolock) WHERE ID = (p.Inv)and type = 0),0) IsItemsExistsInInventory  , 
--( SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW with (nolock) inner join Warehouse AS Wh with (nolock) on Wh.ID = INW.WarehouseID   where  INW.InvID=p.Inv  and  
--INW.WareHouseID=p.WarehouseID) As WarehouseName  ,           
--(Select top 1 Name from WHLoc WH with (nolock) where WH.WareHouseID = p.WarehouseID and id = p.LocationID) As WarehouseLoc   ,
--r.Batch  
--INTO #Temp2
--FROM ReceivePO AS r   with (nolock)           
--RIGHT JOIN RPOItem AS rp with (nolock) on rp.ReceivePO = r.ID     
--INNER JOIN POItem AS p with (nolock) ON p.Line = rp.POLine 
--INNER JOIN BOMT ON BOMT.ID = p.TypeID
--WHERE  p.PO = r.PO    AND BOMT.Type = 'Inventory'
--and ReceivePO  not in ( select ReceivePO   from PJ where ReceivePO is not null  )
------------------------------- RPO PO ------------ 
--INSERT INTO tblInventoryWHTrans (InvID,WarehouseID,LocationID,Hand,Balance,fOrder,Committed,Available,Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
--  SELECT Inv,WarehouseID,LocationID,ISNULL(ReceivedQuan,0) ,ISNULL(Received,0) ,0,0,0,'RPO',ReceivePO,'Add',GETDATE(),'In',batch,GETDATE() FROM #Temp2 
   
--  drop table #Temp2


 ------ END OLD DATA MIGRATION  
---------------------------$$$$$$$$$$$$$$$$$---------------------------
  
   ------    New Column  inv.New_ID int

   --------    New Column  NOTNEEFNOWIWarehouseLocAdj.New_ID int 
    

 --INV table

  

--UPDATE I SET I.New_ID =   (  

--case when exists 
--(     select 1 from inv where name=i.name and status=0 ) 
--then (Select Top 1 ID from Inv where Name=I.Name and status=0  ) 
--else (Select Top 1 ID from Inv where Name=I.Name  order by ID desc   )
--end
--)

--FROM INV I

--$$PO / RPO
 
--update POItem set  POItem.Inv =Inv.New_ID
--from POItem inner join Inv on Inv.ID=POItem.Inv   and Inv.type=0
--            inner join BOMT on BOMT.ID=POItem.TypeID and BOMT.Type='Inventory'

 

--update Trans set Trans.AcctSub = Inv.New_ID from Trans 
--inner join Inv on Inv.ID=Trans.AcctSub  and Inv.type=0
--where Trans.Type=81 and Trans.fDesc='Inventory Recieve PO' 

--- $$Inventory Adjusment
  --delete from IAdj where ISNULL(iadj.type,0)=1
   

--update  IAdj set IAdj.Item=Inv.New_ID
--from IAdj 
--inner join Inv on Inv.ID=IAdj.Item  and Inv.type=0
 

-- update  InvParts set InvParts.ItemID=Inv.New_ID
--from InvParts 
--inner join Inv on Inv.ID=InvParts.ItemID  and Inv.type=0
 
    
 ---- AP BILL 
   

 --update   [APBillItem] set Warehouse='OFC' where phase='inventory'
  

 ----UPDATE   [APBillItem]  set [APBillItem].ItemID=Inv.New_ID from [APBillItem]
 ----inner join Inv on Inv.ID = [APBillItem].ItemID and  Inv.Type=0 and [APBillItem].phase='inventory'
  

 --insert into PJItem (TRID , WarehouseID ,LocationID)
 --select TRID , Warehouse ,WHLocID from [APBillItem] where phase='inventory' and TRID not in ( select TRID from PJItem )


 ---$$$$$$$$$$$$$Ticket 

 

--update TicketI set TicketI.Item=Inv.New_ID  from TicketI
--inner join Inv on Inv.ID=TicketI.Item  and Inv.type=0

 

--update TicketIPDA set TicketIPDA.Item=Inv.New_ID  from TicketIPDA
--inner join Inv on Inv.ID=TicketIPDA.Item  and Inv.type=0
 
  

--- $$$$$$$$$$$$$ AR INVOICE

--select   * from InvoiceI    
--inner join Inv on Inv.ID=InvoiceI.Acct  and Inv.type=0

--update InvoiceI set InvoiceI.Acct=Inv.New_ID from InvoiceI  
--inner join Inv on Inv.ID=InvoiceI.Acct  and Inv.type=0

--update Trans set AcctSub=2619 where  ID=195626 and Type=4
 

 --update POItem set TypeID=0 where TypeID=8 and GL not in (select ID from Chart where type=0) 
  
   --update [APBillItem] set phase='' 
   --where phase='inventory'  
   --and AcctID not in ( (select ID from Chart where type=0) )

    
-- update t2  set    t2.InvID =Inv.New_ID
--  from tblInventoryWHTrans t2 inner join  Inv on Inv.ID=t2.InvID   and Inv.type=0

--update Trans set trans.acctsub = (select New_ID from inv where type=0 and ID= trans.acctsub)
--FROM Trans 
--WHERE Trans.Acct in (3,270)  
--and  Trans.Type   in  (41 , 60 ,  70 , 97) 
--and Trans.AcctSub  is not null
--and trans.acctsub in (select id from inv where type=0)
 

 -----
 -- UPDATE   BOM  set BOM.MatItem=Inv.New_ID from BOM
 --inner join Inv on Inv.ID = BOM.MatItem and  Inv.Type=0 and  BOM.MatItem<>Inv.New_ID

 -- UPDATE   [APBillItem]  set [APBillItem].ItemID=Inv.New_ID from [APBillItem]
 --inner join Inv on Inv.ID = [APBillItem].ItemID and  Inv.Type=0 
 --and [APBillItem].phase<>'inventory'
 --and [APBillItem].ItemID<>Inv.New_ID

 -----------------------Delete dubplicate Inventory Items------------------------>


		  --delete from Inv where   ID not   in (select New_ID from Inv )  

--	 truncate table InvWarehouse 
--insert into InvWarehouse (InvID , WareHouseID)
--select ID, 'OFC' from Inv where Type=0



 
   






 
 
