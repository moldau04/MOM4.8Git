  
 ----------IF Exists (SELECT batch FROM pj where  batch not in (SELECT Batch FROM APBillItem))
 ----------Print ('Please run Script')
 ----------else   Print ('Does not need to run Script')
  
  
  ---//AP Bill Script to Migrate Old-line item to New Table APBillItem.sql
  
  DECLARE  @APtemp table  
 (  
   RowNubmer int IDENTITY(1,1), 
   Batch int
  )

  INSERT INTO @APtemp  

  SELECT batch    FROM pj where   batch not in (SELECT Batch FROM APBillItem)   order by ID asc 
   

   DECLARE @rowNO int=1 
   DECLARE @MAX int=0 
   DECLARE @batch int=0 
   DECLARE @start int=1 
   DECLARE @End int=1 
   
  
   
   SELECT  @MAX =   max(RowNubmer)   FROM @APtemp    
   
   SET @start =@rowNO;    SET @End =@MAX;
  
   WHILE (@rowNO <=  @MAX)

   BEGIN ------1111

    SELECT @batch=batch FROM @APtemp  WHERE RowNubmer=@rowNO
   
    print('#Start----') print( @start) 

    print('#CurrentNO----') print(@rowNO)

	print('#END----') print(@End)

   IF NOT EXISTS ( SELECT 1 FROM APBillItem WHERE batch =@batch )
   BEGIN        
    print('#CurrentBATCHNotExists----') print(@batch)
	 
 
 DECLARE @IsSalesTaxAPBill int=0
 declare @count int =0  
 declare @jcount int=0  

 declare  @temp table  
 (  
  rowid int IDENTITY(1,1),  
  AcctID VARCHAR(1000) NULL,
  amount VARCHAR(1000) NULL,  
  Quan VARCHAR(1000) NULL,
  batch int,  
  id int,  
  line VARCHAR(1000) NULL,
  ref VARCHAR(1000) NULL,
  sel VARCHAR(1000) NULL,
  type VARCHAR(1000) NULL,
  PhaseID VARCHAR(1000) NULL,
  JobId int,  
  strRef VARCHAR(1000) NULL,
  AcctNo VARCHAR(1000) NULL, 
  fDesc VARCHAR(1000) NULL,
  AcctName VARCHAR(1000) NULL,  
  UseTax VARCHAR(1000) NULL, 
  UtaxGL VARCHAR(1000) NULL,
  UName VARCHAR(1000) NULL,  
  jobName VARCHAR(1000) NULL,
  phase VARCHAR(1000) NULL,
  loc VARCHAR(1000) NULL, 
   ItemID VARCHAR(1000) NULL,
  ItemDesc VARCHAR(1000) NULL,
  TypeID VARCHAR(1000) NULL,
  Ticket VARCHAR(1000) NULL,
  OpSq VARCHAR(1000) NULL,
  PrvIn VARCHAR(1000) NULL,
  PrvInQuan VARCHAR(1000) NULL,
  OutstandQuan VARCHAR(1000) NULL,
  OutstandBalance VARCHAR(1000) NULL,
  [STax] VARCHAR(1000) NULL,
	[STaxName] VARCHAR(1000) NULL,
	[STaxRate] VARCHAR(1000) NULL,
	[STaxAmt] VARCHAR(1000) NULL,
	[STaxGL] VARCHAR(1000) NULL,
	[GSTRate] VARCHAR(1000) NULL,
	[GTaxAmt] VARCHAR(1000) NULL,
	[GSTTaxGL] VARCHAR(1000) NULL,
	[STaxType] VARCHAR(1000) NULL,
	[UTaxType]VARCHAR(1000) NULL,
	Warehouse varchar(100) NULL,
	WHLocID VARCHAR(1000) NULL,
	Warehousefdesc VARCHAR(1000) NULL,
	Locationfdesc VARCHAR(500) NULL
 )  

 SELECT @IsSalesTaxAPBill = ISNULL(IsSalesTaxAPBill,1) FROM Control
      IF @IsSalesTaxAPBill = 0 ---------------------------
                             BEGIN

 
 
  
insert into @temp  (AcctID, amount,Quan, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc,ItemID,ItemDesc,TypeID,Ticket,OpSq,PrvIn,PrvInQuan,OutstandQuan,OutstandBalance,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GTaxAmt,GSTTaxGL,STaxType,UTaxType,Warehouse ,WHLocID,Warehousefdesc ,Locationfdesc )  
 SELECT distinct t.Acct As AcctID, (t.Amount - (isnull((SELECT Amount FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),0)     )) as amount,isnull(t.Status,1) As Quan,  
 t.Batch, t.ID, t.Line, t.Ref, isnull(t.Sel,0) as Sel, t.Type, isnull(t.VDoub,0) as PhaseDoub,   
 isnull(t.VInt,0) as JobID, t.strRef,  
 c.Acct+' - '+c.fDesc  AS AcctNo,   
 replace(t.fDesc,'(Amount Before Use Tax - '+CONVERT(varchar, cast(CAST(isnull(t.Amount,0) as decimal) - cast(isnull(p.Amount,0) as decimal) as money), 1)+')','') as fDesc,   
 c.fDesc AS AcctName, 
 
   
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
 
  isnull((SELECT GL FROM Stax WHERE UType = 0 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),'')),0) as STaxGL,
  isnull((SELECT UseTax FROM PJItem WHERE TaxType = 2 AND TRID = t.ID),0) as GSTRate,
  isnull((SELECT Amount FROM PJItem WHERE TaxType = 2 AND TRID = t.ID),0) as GSTTaxAmt,
  isnull((SELECT TOP 1 Acct FROM Trans WHERE BATCH = @batch AND fDesc='GST Payable'),0) AS GSTTaxGL,
  isnull((SELECT Type FROM Stax WHERE UType = 0 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 0 AND TRID = t.ID),'')),0) as STaxType,
  isnull((SELECT Type FROM Stax WHERE UType = 1 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),'')),0) as UTaxType,

 
  isnull((SELECT WarehouseID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID),'') as Warehouse,
  isnull((SELECT LocationID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID),0) as LocationID,
  isnull((SELECT NAME FROM Warehouse WHERE ID IN (SELECT WarehouseID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID)),'') as Warehousefdesc,
  isnull((SELECT Name FROM WHLoc WHERE ID IN (SELECT LocationID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID)),'') as Locationfdesc


 FROM Trans as t   
  INNER JOIN Chart as c on t.Acct = c.ID  
  LEFT JOIN Job as j ON t.VInt = j.ID  
  LEFT JOIN PJItem as p ON t.ID = p.TRID --AND p.TaxType = 0 
  LEFT JOIN (   
  
  SELECT t.ID,   
    (case b.type when 2 then isnull(b.LabItem,0) WHEN 8 THEN isnull(i.ID,'') else isnull(b.MatItem,0) end) as ItemID,   
    CASE b.type WHEN 1 THEN isnull(i.Name,'')  WHEN 8 THEN isnull(i.Name,'') WHEN 2 THEN isnull(pr.fdesc,'')  else isnull(i.Name,'')  END AS Item,   
    isnull(b.Type,0) as TypeID,   
    isnull((select Type from BOMT where ID = b.Type),'') as Phase,jb.APTicket As Ticket,  
    jt.Code  
	
    FROM Trans as t   
    Left JOIN Job as j ON t.VInt = j.ID  
    LEFT JOIN JobTItem as jt ON isnull(jt.Job,0) = isnull(j.ID,0) AND (t.VDoub = jt.Line and isnull(j.ID,0) > 0) AND jt.Type in ( 1,2)  
    INNER JOIN BOM as b ON b.JobTItemID = jt.ID  
    LEFT JOIN Inv as i ON i.ID =t.AcctSub  
    LEFT JOIN PRWage as pr ON pr.ID = t.AcctSub  
	LEFT JOIN PJItem as PJI ON PJI.TRID = t.ID --AND PJI.TaxType = 0
    left join JobI as jb on jb.TransID=t.ID --To get Ticket value against Bill line item   	
     
	WHERE t.Type = 41 AND t.Batch = @batch 
	 
	AND t.fdesc NOT IN ('GST Payable') 
     ) as bom ON bom.ID = t.ID  
 WHERE  t.Type = 41 AND t.Batch = @batch   
 and t.fdesc NOT IN ('GST Payable')  
 order By t.ID   
 select @count = count(*) from @temp  
  
 If (@count = 0)  
 begin  
 insert into @temp (AcctID, amount, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc,ItemID,ItemDesc,TypeID,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GTaxAmt,GSTTaxGL,STaxType,UTaxType,Warehouse ,WHLocID,Warehousefdesc ,Locationfdesc)  
 values(0,'',@batch,0,0,0,0,0,0,0,'','','','','',0,'','','','',0,'',0,0,'',0,0,0,0,0,0,0,0,'',0,'','');  
 end   
 UPDATE @temp SET Quan = 0 WHERE fdesc IN (SELECT sTaxName+' Payable' FROM @temp WHERE Stax = 1) 
 
  
 END
   ------------------------------------------------------------------------------------------------
      IF @IsSalesTaxAPBill = 1------------------------------
      
       BEGIN 
  
insert into @temp  (AcctID, amount,Quan, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc,ItemID,ItemDesc,TypeID,Ticket,OpSq,PrvIn,PrvInQuan,OutstandQuan,OutstandBalance,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GTaxAmt,GSTTaxGL,STaxType,UTaxType,Warehouse ,WHLocID,Warehousefdesc ,Locationfdesc)  
 SELECT distinct t.Acct As AcctID, (t.Amount - (isnull((SELECT Amount FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),0)     )) as amount,t.Status As Quan,  
 t.Batch, t.ID, t.Line, t.Ref, isnull(t.Sel,0) as Sel, t.Type, isnull(t.VDoub,0) as PhaseDoub,   
 isnull(t.VInt,0) as JobID, t.strRef,  
 c.Acct+' - '+c.fDesc  AS AcctNo,   
 replace(t.fDesc,'(Amount Before Use Tax - '+CONVERT(varchar, cast(CAST(isnull(t.Amount,0) as decimal) - cast(isnull(p.Amount,0) as decimal) as money), 1)+')','') as fDesc,   
 c.fDesc AS AcctName,  
  isnull((SELECT UseTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),0) as UseTax,  
  isnull((SELECT GL FROM Stax WHERE UType = 1 AND Name = isnull((SELECT sTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),'')),0) as UtaxGL,
  isnull((SELECT sTax FROM PJItem WHERE TaxType = 1 AND TRID = t.ID),'') as UName,
  isnull(CONVERT(varchar, j.ID)+', '+j.fDesc,'') as JobName, 
 case   when( bom.Phase is null and isnull(t.VInt,0) =0 ) 
 then (select top 1 'Inventory' from inv where id=t.AcctSub and type=0) 
 else bom.Phase end AS  Phase ,   
 isnull((select Tag from Loc where Loc = j.Loc),'') as loc,   
 case   when (bom.ItemID is null and isnull(t.VInt,0) =0 )
 then t.AcctSub else bom.ItemID end AS  ItemID ,   
 case   when( bom.Item is null and isnull(t.VInt,0) =0 ) 
 then (select top 1 name from inv where id=t.AcctSub and type=0) else bom.Item end AS  Item ,   
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
 
  isnull((SELECT WarehouseID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID),'') as Warehouse,
  isnull((SELECT LocationID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID),0) as LocationID,
  isnull((SELECT NAME FROM Warehouse WHERE ID IN (SELECT WarehouseID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID)),'') as Warehousefdesc,
  isnull((SELECT Name FROM WHLoc WHERE ID IN (SELECT LocationID FROM PJItem WHERE TaxType IS NULL AND TRID = t.ID)),'') as Locationfdesc
 FROM Trans as t   
  INNER JOIN Chart as c on t.Acct = c.ID  
  LEFT JOIN Job as j ON t.VInt = j.ID  
  LEFT JOIN PJItem as p ON t.ID = p.TRID --AND p.TaxType = 0 
  LEFT JOIN (   
 
  SELECT t.ID,   
    (case b.type when 2 then isnull(b.LabItem,0) WHEN 8 THEN isnull(i.ID,'') else isnull(b.MatItem,0) end) as ItemID,   
    CASE b.type WHEN 1 THEN isnull(i.Name,'')  WHEN 8 THEN isnull(i.Name,'') WHEN 2 THEN isnull(pr.fdesc,'')  else isnull(i.Name,'')  END AS Item,   
    isnull(b.Type,0) as TypeID,   
    isnull((select Type from BOMT where ID = b.Type),'') as Phase,jb.APTicket As Ticket,  
    jt.Code  
    FROM Trans as t   
    Left JOIN Job as j ON t.VInt = j.ID  
    LEFT JOIN JobTItem as jt ON isnull(jt.Job,0) = isnull(j.ID,0) AND (t.VDoub = jt.Line and isnull(j.ID,0) > 0) AND jt.Type in ( 1,2)  
    INNER JOIN BOM as b ON b.JobTItemID = jt.ID  
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
 
 and t.fdesc NOT IN ('GST Payable') 
 
 order By t.ID  
 
 select @count = count(*) from @temp  
  
 If (@count = 0)  
 begin  
 insert into @temp (AcctID, amount, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc,ItemID,ItemDesc,TypeID,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GTaxAmt,GSTTaxGL,STaxType,UTaxType,Warehouse ,WHLocID,Warehousefdesc ,Locationfdesc)  
 values(0,'',@batch,0,0,0,0,0,0,0,'','','','','',0,'','','','',0,'',0,0,'',0,0,0,0,0,0,0,0,'',0,'','');  
 end   

 --delete from  @temp  where fDesc  = sTaxName+' Payable'  and Stax = 1
 --delete from  @temp  where fDesc  = UName+' Payable' 
 
  
    END 
    -------------------------------------------------------------------
	  INSERT INTO [dbo].[APBillItem]
           ([AcctID],[Amount],[Quan],[Batch],[TRID],[line],[Ref],[Sel],[Type],[PhaseID],[JobId],[strRef],[AcctNo],[fDesc],[AcctName],[UseTax]
           ,[UtaxGL],[UName],[jobName],[phase],[loc],[ItemID],[ItemDesc],[TypeID],[Ticket],[OpSq],[PrvIn],[PrvInQuan],[OutstandQuan],[OutstandBalance],[STax],[STaxName]
           ,[STaxRate],[STaxAmt],[STaxGL],[GSTRate],[GTaxAmt],[GSTTaxGL],[STaxType],[UTaxType],[Warehouse],[WHLocID],[Warehousefdesc],[Locationfdesc]
		   )

		 SELECT [AcctID],[Amount],[Quan],[Batch],[id],[line],[Ref],[Sel],
            [Type],[PhaseID],[JobId],[strRef],[AcctNo],[fDesc],[AcctName],[UseTax]
           ,[UtaxGL],[UName],[jobName],[phase],[loc],[ItemID],[ItemDesc],[TypeID],
		   [Ticket],[OpSq],[PrvIn],[PrvInQuan],[OutstandQuan],[OutstandBalance],[STax],[STaxName]
           ,[STaxRate],[STaxAmt],[STaxGL],[GSTRate],[GTaxAmt],[GSTTaxGL],[STaxType],
		   [UTaxType],[Warehouse],[WHLocID],[Warehousefdesc],[Locationfdesc] FROM (
    SELECT  [AcctID],[Amount],[Quan],[Batch],[id],[line],[Ref],[Sel],
            [Type],[PhaseID],[JobId],[strRef],[AcctNo],[fDesc],[AcctName],[UseTax]
           ,[UtaxGL],[UName],[jobName],[phase],[loc],[ItemID],[ItemDesc],[TypeID],
		   [Ticket],[OpSq],[PrvIn],[PrvInQuan],[OutstandQuan],[OutstandBalance],[STax],[STaxName]
           ,[STaxRate],[STaxAmt],[STaxGL],[GSTRate],[GTaxAmt],[GSTTaxGL],[STaxType],
		   [UTaxType],[Warehouse],[WHLocID],[Warehousefdesc],[Locationfdesc]
		   FROM @temp WHERE     fDesc NOT IN (SELECT sTaxName+' Payable' FROM @temp WHERE Stax = 1 )) AS P WHERE fDesc NOT IN (SELECT UName+' Payable' FROM @temp )
		   
		   delete @temp 
		   
	--------------------------------------
	END
	SET @rowNO  = @rowNO+1;
    END -----1111


 
  
