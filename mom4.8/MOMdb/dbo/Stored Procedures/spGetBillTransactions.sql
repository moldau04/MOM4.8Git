CREATE PROCEDURE [dbo].[spGetBillTransactions]  
 @batch int  
AS  
BEGIN  
  
 SET NOCOUNT ON;  
 --declare @acct int  
 --declare @acctSub int  
 --declare @amount numeric(30,2)  
 --declare @tid int  
 --declare @line smallint  
 --declare @ref int  
 --declare @sel smallint  
 --declare @status varchar(10)  
 --declare @type smallint  
 --declare @phaseDoub numeric(30,2)  
 --declare @jobId int  
 --declare @strRef varchar(50)  
 --declare @cAcct varchar(15)  
 --declare @fDesc varchar(max)  
 --declare @useTax int  
 --declare @UtaxGL int  
 --declare @UtaxName varchar(25)  
 --declare @jobName varchar(75)  
 --declare @phase varchar(255)  
 --declare @loc varchar(100)  
 DECLARE @IsSalesTaxAPBill int=0
 declare @count int =0  
 declare @jcount int=0  

 create table #temp  
 (  
  rowid int IDENTITY(1,1),  
  AcctID int,  
  amount varchar(12),  
  Quan numeric(30,2),  
  batch int,  
  id int,  
  line smallint,  
  ref int,  
  sel smallint,  
  type smallint,  
  PhaseID smallint,  
  JobId int,  
  strRef varchar(50),  
  AcctNo varchar(250),  
  fDesc varchar(max),  
  AcctName varchar(150),  
  UseTax varchar(15),  
  UtaxGL int,  
  UName varchar(25),  
  jobName varchar(150),  
  phase varchar(255),  
  loc varchar(100),  
   ItemID int,  
  ItemDesc varchar(8000),  
  TypeID int,  
  Ticket int,  
  OpSq varchar(150),  
  PrvIn numeric(30,2),   
  PrvInQuan numeric(30,2),   
  OutstandQuan numeric(30,2),  
  OutstandBalance numeric(30,2) ,
  [STax] [smallint] NULL,
	[STaxName] [varchar](50) NULL,
	[STaxRate] [numeric](30, 4) NULL,
	[STaxAmt] [numeric](30, 4) NULL,
	[STaxGL] [int] NULL,
	[GSTRate] [numeric](30, 4) NULL,
	[GTaxAmt] [numeric](30, 4) NULL,
	[GSTTaxGL] [int] NULL,
	[STaxType] [int] NULL,
	[UTaxType] [int] NULL,
	Warehouse varchar(100) NULL,
	WHLocID int,
	Warehousefdesc VARCHAR(500) NULL,
	Locationfdesc VARCHAR(500) NULL
 )  

 SELECT @IsSalesTaxAPBill = ISNULL(IsSalesTaxAPBill,1) FROM Control
  IF @IsSalesTaxAPBill = 0 
 BEGIN

 
 
  
insert into #temp  (AcctID, amount,Quan, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc,ItemID,ItemDesc,TypeID,Ticket,OpSq,PrvIn,PrvInQuan,OutstandQuan,OutstandBalance,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GTaxAmt,GSTTaxGL,STaxType,UTaxType,Warehouse ,WHLocID,Warehousefdesc ,Locationfdesc )  
 --SELECT distinct t.Acct As AcctID, (t.Amount - CONVERT(DECIMAL(10,2), isnull(p.Amount,0))) as amount,t.Status As Quan,  
 --SELECT distinct t.Acct As AcctID, (t.Amount - (isnull((SELECT Amount FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),0)+isnull((SELECT Amount FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),0)+isnull((SELECT Amount FROM PJItem WHERE TaxType = 2 AND TRID = t.ID),0)     )) as amount,t.Status As Quan,
 SELECT distinct t.Acct As AcctID, (t.Amount - (isnull((SELECT Amount FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),0)     )) as amount,t.Status As Quan,  
 t.Batch, t.ID, t.Line, t.Ref, isnull(t.Sel,0) as Sel, t.Type, isnull(t.VDoub,0) as PhaseDoub,   
 isnull(t.VInt,0) as JobID, t.strRef,  
 c.Acct+' - '+c.fDesc  AS AcctNo,   
 replace(t.fDesc,'(Amount Before Use Tax - '+CONVERT(varchar, cast(CAST(isnull(t.Amount,0) as decimal) - cast(isnull(p.Amount,0) as decimal) as money), 1)+')','') as fDesc,   
 c.fDesc AS AcctName, 
 --isnull(p.UseTax,0) as UseTax,   
 --isnull((SELECT GL FROM stax WHERE Name=p.Stax),'') as UtaxGL,  
 --isnull(p.Stax,'') as UName,  
   
  isnull((SELECT UseTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),0) as UseTax,
  --isnull((SELECT TOP 1 Acct FROM Trans WHERE BATCH = @batch AND fDesc='Use Tax Payable'),0) AS UtaxGL,
  isnull((SELECT GL FROM Stax WHERE UType = 1 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),'')),0) as UtaxGL,
  isnull((SELECT sTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),'') as UName,

 isnull(CONVERT(varchar, j.ID)+', '+j.fDesc,'') as JobName,   
 case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) then (select top 1 'Inventory' from inv where id=t.AcctSub and type=0) else bom.Phase end AS  Phase ,   
 isnull((select Tag from Loc where Loc = j.Loc),'') as loc,   
 case   when (bom.ItemID is null and isnull(t.VInt,0) =0 ) then t.AcctSub else bom.ItemID end AS  ItemID ,   
 case   when( bom.Item is null and isnull(t.VInt,0) =0 ) then (select top 1 name from inv where id=t.AcctSub and type=0) else bom.Item end AS  Item ,   
  bom.TypeID,  
  bom.Ticket,bom.Code,  
  '0' as PrvIn,  
  '0' as PrvInQuan,  
  '0' as OutstandQuan,  
  '0' as OutstandBalance,  
  
  CASE WHEN isnull((SELECT TOP 1 UseTax FROM PJItem WHERE TaxType IN(0,2) AND TRID = t.ID),0) =0 THEN 0 ELSE 1 END as STax,
  isnull((SELECT sTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),'') as STaxName,
  
  isnull((SELECT UseTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),0) as STaxRate,
  isnull((SELECT Amount FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),0) as STaxAmt, 
  --isnull((SELECT TOP 1 Acct FROM Trans WHERE BATCH = @batch AND fDesc='Sales Tax Payable'),0) AS STaxGL,
  isnull((SELECT GL FROM Stax WHERE UType = 0 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),'')),0) as STaxGL,
  isnull((SELECT UseTax FROM PJItem WHERE TaxType = 2 AND TRID = t.ID),0) as GSTRate,
  isnull((SELECT Amount FROM PJItem WHERE TaxType = 2 AND TRID = t.ID),0) as GSTTaxAmt,
  isnull((SELECT TOP 1 Acct FROM Trans WHERE BATCH = @batch AND fDesc='GST Payable'),0) AS GSTTaxGL,
  isnull((SELECT Type FROM Stax WHERE UType = 0 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),'')),0) as STaxType,
  isnull((SELECT Type FROM Stax WHERE UType = 1 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),'')),0) as UTaxType,

  --case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) then (SELECT WareHouseID FROM POItem poi INNER JOIN PO po ON poi.PO = po.PO INNER JOIN PJ pj ON po.PO = pj.PO WHERE poi.Inv = t.AcctSub AND pj.Batch=@batch) else '' END as Warehouse, 
  --case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) then (SELECT LocationID FROM POItem poi INNER JOIN PO po ON poi.PO = po.PO INNER JOIN PJ pj ON po.PO = pj.PO WHERE poi.Inv = t.AcctSub AND pj.Batch=@batch ) else 0 END as LocationID,
  --case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) then  (SELECT NAME FROM Warehouse WHERE ID IN (SELECT WareHouseID FROM POItem poi INNER JOIN PO po ON poi.PO = po.PO INNER JOIN PJ pj ON po.PO = pj.PO WHERE poi.Inv = t.AcctSub AND pj.Batch=@batch)) else '' END as Warehousefdesc, 
  --case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) then  (SELECT Tag FROM Loc WHERE Loc IN (SELECT LocationID FROM POItem poi INNER JOIN PO po ON poi.PO = po.PO INNER JOIN PJ pj ON po.PO = pj.PO WHERE poi.Inv = t.AcctSub AND pj.Batch=@batch)) else '' END as Locationfdesc

  isnull((SELECT WarehouseID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID),'') as Warehouse,
  isnull((SELECT LocationID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID),0) as LocationID,
  isnull((SELECT NAME FROM Warehouse WHERE ID IN (SELECT WarehouseID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID)),'') as Warehousefdesc,
  isnull((SELECT Name FROM WHLoc WHERE ID IN (SELECT LocationID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID)),'') as Locationfdesc


 FROM Trans as t   
  INNER JOIN Chart as c on t.Acct = c.ID  
  LEFT JOIN Job as j ON t.VInt = j.ID  
  LEFT JOIN PJItem as p ON t.ID = p.TRID --AND p.TaxType = 0 
  LEFT JOIN (   
  --SELECT t.ID,   
  --  (case b.type when 2 then isnull(b.LabItem,0) else isnull(b.MatItem,0) end) as ItemID,   
  --  CASE b.type WHEN 1 THEN isnull(i.Name,'')  WHEN 8 THEN isnull(i.Name,'') WHEN 2 THEN isnull(pr.fdesc,'') END AS Item,   
  --  isnull(b.Type,0) as TypeID,   
  --  isnull((select Type from BOMT where ID = b.Type),'') as Phase   
  --  FROM Trans as t   
  --  INNER JOIN Job as j ON t.VInt = j.ID  
  --  LEFT JOIN JobTItem as jt ON jt.Job = j.ID AND t.VDoub = jt.Line AND jt.Type = 1  
  --  INNER JOIN BOM as b ON b.JobTItemID = jt.ID  
  --  LEFT JOIN Inv as i ON i.ID = b.MatItem  
  --  LEFT JOIN PRWage as pr ON pr.ID = b.LabItem  
  --  WHERE t.Type = 41 AND t.Batch = @batch AND t.fdesc<>'Use Tax Payable'   
  SELECT t.ID,   
    (case b.type when 2 then isnull(b.LabItem,0) WHEN 8 THEN isnull(i.ID,'') else isnull(b.MatItem,0) end) as ItemID,   
    CASE b.type WHEN 1 THEN isnull(i.Name,'')  WHEN 8 THEN isnull(i.Name,'') WHEN 2 THEN isnull(pr.fdesc,'')  else isnull(i.Name,'')  END AS Item,   
    isnull(b.Type,0) as TypeID,   
    isnull((select Type from BOMT where ID = b.Type),'') as Phase,jb.APTicket As Ticket,  
    jt.Code  
	
    FROM Trans as t   
    Left JOIN Job as j ON t.VInt = j.ID  
    --LEFT JOIN JobTItem as jt ON isnull(jt.Job,0) = isnull(j.ID,0) AND (t.VDoub = jt.Line and isnull(j.ID,0) > 0) AND jt.Type in ( 1,2,0)  
    --INNER JOIN BOM as b ON b.JobTItemID = jt.ID  
	LEFT JOIN JobTItem as jt ON isnull(jt.Job,0) = isnull(j.ID,0) AND (t.VDoub = jt.Line ) AND jt.Type in ( 1,2,0)  
    LEFT JOIN BOM as b ON b.JobTItemID = jt.ID  
    LEFT JOIN Inv as i ON i.ID =t.AcctSub  
    LEFT JOIN PRWage as pr ON pr.ID = t.AcctSub  
	LEFT JOIN PJItem as PJI ON PJI.TRID = t.ID --AND PJI.TaxType = 0
    left join JobI as jb on jb.TransID=t.ID --To get Ticket value against Bill line item   
	
    --WHERE t.Type = 41 AND t.Batch = @batch AND t.fdesc NOT IN ('Use Tax Payable'   ,'Sales Tax Payable','GST Payable')
	WHERE t.Type = 41 AND t.Batch = @batch 
	--AND t.fdesc NOT IN ('Use Tax Payable'   ,'GST Payable') 
	AND t.fdesc NOT IN ('GST Payable') 
	--AND replace(replace(replace(t.fDesc,'Use Tax Payable',''),'GST Payable',''),CASE WHEN PJI.Stax IS NOT NULL THEN PJI.Stax+' Payable' WHEN  PJI.Stax IS NULL THEN '' END,'') <> ''
    ) as bom ON bom.ID = t.ID  
 WHERE  t.Type = 41 AND t.Batch = @batch   
 --and t.fdesc NOT IN ('Use Tax Payable','GST Payable') 
 and t.fdesc NOT IN ('GST Payable') 
 --AND replace(replace(replace(t.fDesc,'Use Tax Payable',''),'GST Payable',''),CASE WHEN p.Stax IS NOT NULL THEN p.Stax+' Payable' WHEN  p.Stax IS NULL THEN '' END,'') <> ''
 
 
 --and t.fdesc NOT IN ('Use Tax Payable'   ,isnull((SELECT sTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),'')+' Payable','GST Payable') 

 
 order By t.ID  
  
 --SET IDENTITY_INSERT #tempTrans ON  
  
 --insert into #tempTrans (rowid,AcctID, amount, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc)  
 --select * from #temp  
  
 --SET IDENTITY_INSERT #tempTrans OFF  
  
 select @count = count(*) from #temp  
  
 If (@count = 0)  
 begin  
 insert into #temp (AcctID, amount, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc,ItemID,ItemDesc,TypeID,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GTaxAmt,GSTTaxGL,STaxType,UTaxType,Warehouse ,WHLocID,Warehousefdesc ,Locationfdesc)  
 values(0,'',0,0,0,0,0,0,0,0,'','','','','',0,'','','','',0,'',0,0,'',0,0,0,0,0,0,0,0,'',0,'','');  
 end  
  
 UPDATE #temp SET Quan = 0 WHERE fdesc IN (SELECT sTaxName+' Payable' FROM #temp WHERE Stax = 1)
 --SELECT * FROM #temp
 --select * INTO #temp1 from #temp WHERE     fDesc NOT IN (SELECT sTaxName+' Payable' FROM #temp WHERE Stax = 1 )
 --select * from #temp1 WHERE     fDesc NOT IN (SELECT UName+' Payable' FROM #temp )
 select * from #temp WHERE     fDesc NOT IN (SELECT UName+' Payable' FROM #temp )
 
 -- ,( SELECT UName+' Payable' FROM #temp WHERE Stax = 0))
 --WHERE ItemDesc IS  NOT NULL  
   
 drop table #temp  
 --drop table #temp1  
 --drop table #tempTrans


 END
 ------------------------------------------------------------------------------------------------
 IF @IsSalesTaxAPBill = 1
 BEGIN
 --declare @count int =0  
 --declare @jcount int=0  
 
  
insert into #temp  (AcctID, amount,Quan, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc,ItemID,ItemDesc,TypeID,Ticket,OpSq,PrvIn,PrvInQuan,OutstandQuan,OutstandBalance,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GTaxAmt,GSTTaxGL,STaxType,UTaxType,Warehouse ,WHLocID,Warehousefdesc ,Locationfdesc)  
 --SELECT distinct t.Acct As AcctID, (t.Amount - CONVERT(DECIMAL(10,2), isnull(p.Amount,0))) as amount,t.Status As Quan,  
 --SELECT distinct t.Acct As AcctID, (t.Amount - (isnull((SELECT Amount FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),0)+isnull((SELECT Amount FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),0)+isnull((SELECT Amount FROM PJItem WHERE TaxType = 2 AND TRID = t.ID),0)     )) as amount,t.Status As Quan,
 SELECT distinct t.Acct As AcctID, (t.Amount - (isnull((SELECT Amount FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),0)     )) as amount,t.Status As Quan,  
 t.Batch, t.ID, t.Line, t.Ref, isnull(t.Sel,0) as Sel, t.Type, isnull(t.VDoub,0) as PhaseDoub,   
 isnull(t.VInt,0) as JobID, t.strRef,  
 c.Acct+' - '+c.fDesc  AS AcctNo,   
 replace(t.fDesc,'(Amount Before Use Tax - '+CONVERT(varchar, cast(CAST(isnull(t.Amount,0) as decimal) - cast(isnull(p.Amount,0) as decimal) as money), 1)+')','') as fDesc,   
 c.fDesc AS AcctName, 
 --isnull(p.UseTax,0) as UseTax,   
 --isnull((SELECT GL FROM stax WHERE Name=p.Stax),'') as UtaxGL,  
 --isnull(p.Stax,'') as UName,  
   
  isnull((SELECT UseTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),0) as UseTax,
  --isnull((SELECT TOP 1 Acct FROM Trans WHERE BATCH = @batch AND fDesc='Use Tax Payable'),0) AS UtaxGL,
  isnull((SELECT GL FROM Stax WHERE UType = 1 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),'')),0) as UtaxGL,
  isnull((SELECT sTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),'') as UName,

 isnull(CONVERT(varchar, j.ID)+', '+j.fDesc,'') as JobName,   
 case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) then (select top 1 'Inventory' from inv where id=t.AcctSub and type=0) else bom.Phase end AS  Phase ,   
 isnull((select Tag from Loc where Loc = j.Loc),'') as loc,   
 case   when (bom.ItemID is null and isnull(t.VInt,0) =0 ) then t.AcctSub else bom.ItemID end AS  ItemID ,   
 case   when( bom.Item is null and isnull(t.VInt,0) =0 ) then (select top 1 name from inv where id=t.AcctSub and type=0) else bom.Item end AS  Item ,   
  bom.TypeID,  
  bom.Ticket,bom.Code,  
  '0' as PrvIn,  
  '0' as PrvInQuan,  
  '0' as OutstandQuan,  
  '0' as OutstandBalance,  
  
  CASE WHEN isnull((SELECT TOP 1 UseTax FROM PJItem WHERE TaxType IN(0,2) AND TRID = t.ID),0) =0 THEN 0 ELSE 1 END as STax,
  isnull((SELECT sTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),'') as STaxName,
  
  isnull((SELECT UseTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),0) as STaxRate,
  isnull((SELECT Amount FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),0) as STaxAmt, 
  --isnull((SELECT TOP 1 Acct FROM Trans WHERE BATCH = @batch AND fDesc='Sales Tax Payable'),0) AS STaxGL,
  isnull((SELECT GL FROM Stax WHERE UType = 0 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),'')),0) as STaxGL,
  isnull((SELECT UseTax FROM PJItem WHERE TaxType = 2 AND TRID = t.ID),0) as GSTRate,
  isnull((SELECT Amount FROM PJItem WHERE TaxType = 2 AND TRID = t.ID),0) as GSTTaxAmt,
  isnull((SELECT TOP 1 Acct FROM Trans WHERE BATCH = @batch AND fDesc='GST Payable'),0) AS GSTTaxGL,
  isnull((SELECT Type FROM Stax WHERE UType = 0 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),'')),0) as STaxType,
  isnull((SELECT Type FROM Stax WHERE UType = 1 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),'')),0) as UTaxType,

  --case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) then (select top 1 'Inventory' from inv where id=t.AcctSub and type=0) else bom.Phase end AS  Phase ,   

  --case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) then (SELECT WareHouseID FROM POItem poi INNER JOIN PO po ON poi.PO = po.PO INNER JOIN PJ pj ON po.PO = pj.PO WHERE poi.Inv = t.AcctSub AND pj.Batch=@batch) else '' END as Warehouse, 
  --case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) then (SELECT LocationID FROM POItem poi INNER JOIN PO po ON poi.PO = po.PO INNER JOIN PJ pj ON po.PO = pj.PO WHERE poi.Inv = t.AcctSub AND pj.Batch=@batch ) else 0 END as LocationID,
  --case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) then  (SELECT NAME FROM Warehouse WHERE ID IN (SELECT WareHouseID FROM POItem poi INNER JOIN PO po ON poi.PO = po.PO INNER JOIN PJ pj ON po.PO = pj.PO WHERE poi.Inv = t.AcctSub AND pj.Batch=@batch)) else '' END as Warehousefdesc, 
  --case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) then  (SELECT Tag FROM Loc WHERE Loc IN (SELECT LocationID FROM POItem poi INNER JOIN PO po ON poi.PO = po.PO INNER JOIN PJ pj ON po.PO = pj.PO WHERE poi.Inv = t.AcctSub AND pj.Batch=@batch)) else '' END as Locationfdesc

  isnull((SELECT WarehouseID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID),'') as Warehouse,
  isnull((SELECT LocationID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID),0) as LocationID,
  isnull((SELECT NAME FROM Warehouse WHERE ID IN (SELECT WarehouseID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID)),'') as Warehousefdesc,
  isnull((SELECT Name FROM WHLoc WHERE ID IN (SELECT LocationID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID)),'') as Locationfdesc
 FROM Trans as t   
  INNER JOIN Chart as c on t.Acct = c.ID  
  LEFT JOIN Job as j ON t.VInt = j.ID  
  LEFT JOIN PJItem as p ON t.ID = p.TRID --AND p.TaxType = 0 
  LEFT JOIN (   
  --SELECT t.ID,   
  --  (case b.type when 2 then isnull(b.LabItem,0) else isnull(b.MatItem,0) end) as ItemID,   
  --  CASE b.type WHEN 1 THEN isnull(i.Name,'')  WHEN 8 THEN isnull(i.Name,'') WHEN 2 THEN isnull(pr.fdesc,'') END AS Item,   
  --  isnull(b.Type,0) as TypeID,   
  --  isnull((select Type from BOMT where ID = b.Type),'') as Phase   
  --  FROM Trans as t   
  --  INNER JOIN Job as j ON t.VInt = j.ID  
  --  LEFT JOIN JobTItem as jt ON jt.Job = j.ID AND t.VDoub = jt.Line AND jt.Type = 1  
  --  INNER JOIN BOM as b ON b.JobTItemID = jt.ID  
  --  LEFT JOIN Inv as i ON i.ID = b.MatItem  
  --  LEFT JOIN PRWage as pr ON pr.ID = b.LabItem  
  --  WHERE t.Type = 41 AND t.Batch = @batch AND t.fdesc<>'Use Tax Payable'   
  SELECT t.ID,   
    (case b.type when 2 then isnull(b.LabItem,0) WHEN 8 THEN isnull(i.ID,'') else isnull(b.MatItem,0) end) as ItemID,   
    CASE b.type WHEN 1 THEN isnull(i.Name,'')  WHEN 8 THEN isnull(i.Name,'') WHEN 2 THEN isnull(pr.fdesc,'')  else isnull(i.Name,'')  END AS Item,   
    isnull(b.Type,0) as TypeID,   
    isnull((select Type from BOMT where ID = b.Type),'') as Phase,jb.APTicket As Ticket,  
    jt.Code  
    FROM Trans as t   
    Left JOIN Job as j ON t.VInt = j.ID  
    --LEFT JOIN JobTItem as jt ON isnull(jt.Job,0) = isnull(j.ID,0) AND (t.VDoub = jt.Line and isnull(j.ID,0) > 0) AND jt.Type in ( 1,2)  
    --INNER JOIN BOM as b ON b.JobTItemID = jt.ID  
	LEFT JOIN JobTItem as jt ON isnull(jt.Job,0) = isnull(j.ID,0) AND (t.VDoub = jt.Line ) AND jt.Type in ( 1,2,0)  
    LEFT JOIN BOM as b ON b.JobTItemID = jt.ID  
    LEFT JOIN Inv as i ON i.ID =t.AcctSub  
    LEFT JOIN PRWage as pr ON pr.ID = t.AcctSub  
	LEFT JOIN PJItem as PJI ON PJI.TRID = t.ID --AND PJI.TaxType = 0
    left join JobI as jb on jb.TransID=t.ID --To get Ticket value against Bill line item   
    --WHERE t.Type = 41 AND t.Batch = @batch AND t.fdesc NOT IN ('Use Tax Payable'   ,'Sales Tax Payable','GST Payable')
	WHERE t.Type = 41 AND t.Batch = @batch 
	--AND t.fdesc NOT IN ('Use Tax Payable'   ,'GST Payable') 
	AND t.fdesc NOT IN ('GST Payable') 
	--AND replace(replace(replace(t.fDesc,'Use Tax Payable',''),'GST Payable',''),CASE WHEN PJI.Stax IS NOT NULL THEN PJI.Stax+' Payable' WHEN  PJI.Stax IS NULL THEN '' END,'') <> ''
    ) as bom ON bom.ID = t.ID  
 WHERE  t.Type = 41 AND t.Batch = @batch   
 --and t.fdesc NOT IN ('Use Tax Payable','GST Payable') 
 and t.fdesc NOT IN ('GST Payable') 
 --AND replace(replace(replace(t.fDesc,'Use Tax Payable',''),'GST Payable',''),CASE WHEN p.Stax IS NOT NULL THEN p.Stax+' Payable' WHEN  p.Stax IS NULL THEN '' END,'') <> ''
 
 
 --and t.fdesc NOT IN ('Use Tax Payable'   ,isnull((SELECT sTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),'')+' Payable','GST Payable') 

 
 order By t.ID  
  
 --SET IDENTITY_INSERT #tempTrans ON  
  
 --insert into #tempTrans (rowid,AcctID, amount, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc)  
 --select * from #temp  
  
 --SET IDENTITY_INSERT #tempTrans OFF  
  
 select @count = count(*) from #temp  
  
 If (@count = 0)  
 begin  
 insert into #temp (AcctID, amount, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc,ItemID,ItemDesc,TypeID,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GTaxAmt,GSTTaxGL,STaxType,UTaxType,Warehouse ,WHLocID,Warehousefdesc ,Locationfdesc)  
 values(0,'',0,0,0,0,0,0,0,0,'','','','','',0,'','','','',0,'',0,0,'',0,0,0,0,0,0,0,0,'',0,'','');  
 end  
  

 select * INTO #temp2 from #temp WHERE     fDesc NOT IN (SELECT sTaxName+' Payable' FROM #temp WHERE Stax = 1 )
 select * from #temp2 WHERE     fDesc NOT IN (SELECT UName+' Payable' FROM #temp )
 
 -- ,( SELECT UName+' Payable' FROM #temp WHERE Stax = 0))
 --WHERE ItemDesc IS  NOT NULL  
   
 drop table #temp  
 drop table #temp2  
 --drop table #tempTrans  
   
  
END
END
