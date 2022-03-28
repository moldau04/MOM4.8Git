CREATE PROCEDURE [dbo].[spGetBillRecurrTransactions]
 @batch int
AS
BEGIN

	SET NOCOUNT ON;
	
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
	 OutstandBalance numeric(30,2),
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
	Locationfdesc VARCHAR(500) NULL,
	[GTax] [smallint] NULL,
	[Price] [numeric](30, 4) NULL,
	OrderedQuan numeric(30,2),
		Ordered numeric(30,2)
	)

	

 insert into #temp  (AcctID, amount,Quan, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc,ItemID,ItemDesc,TypeID,Ticket,OpSq,PrvIn,PrvInQuan,OutstandQuan,OutstandBalance,
 STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GTaxAmt,	GSTTaxGL,STaxType,UTaxType,
 Warehouse ,WHLocID ,Warehousefdesc ,Locationfdesc,GTax,Price ,OrderedQuan ,Ordered )
 SELECT distinct t.AcctID, CONVERT(DECIMAL(10,2), isnull(t.Amount,0)) as amount,t.Quan As Quan,p.Frequency as  batch ,
	t.ID ,t.Line,0,0,0,t.PhaseID,t.JobId,p.ref,c.Acct+' - '+c.fDesc  AS AcctNo,t.fDesc,c.fDesc AS AcctName,isnull(t.UseTax,0) as UseTax,ISNULL(t.UTaxGL,0),t.UtaxName,
	isnull(CONVERT(varchar, j.ID)+', '+j.fDesc,'') as JobName, t.Phase,
	isnull((select Tag from Loc where Loc = j.Loc),'') as loc, 
	t.ItemID , t.ItemDesc,t.TypeID,t.ticket,t.opsq,0,0,0,0,isnull(t.STax,0),isnull(t.STaxName,''),isnull(t.STaxRate,0),isnull(t.STaxAmt,0),ISNULL(t.STaxGL,0),ISNULL(t.GSTRate,0),ISNUll(t.GSTTaxAmt,0),ISNULL(t.GSTTaxGL,0),
	isnull((SELECT Type FROM Stax WHERE UType = 0 AND Name = t.STaxName),0) as STaxType,
  isnull((SELECT Type FROM Stax WHERE UType = 1 AND Name = t.UtaxName AND Rate = isnull(t.UseTax,0)),0) as UTaxType,

  isnull(Warehouse,'') as Warehouse,
  isnull(WHLOCID,0) as LocationID,
  isnull((SELECT NAME FROM Warehouse WHERE ID =Warehouse),'') as Warehousefdesc,
  isnull((SELECT Name FROM WHLoc WHERE ISNULL(ID,0)= WHLOCID),'') as Locationfdesc,isnull(t.GTax,0),ISNULL(t.Price,0),
  0.00 AS OrderedQuan,
0.00 AS Ordered

	FROM PJRecurrI as t INNER JOIN PJRecurr as p ON p.ID = t.PJID INNER JOIN Chart as c on t.AcctID = c.ID LEFT JOIN Job as j ON t.JobID = j.ID
	
 WHERE  p.ID = @batch  
 order By t.ID,t.Line

 --SET IDENTITY_INSERT #tempTrans ON

 --insert into #tempTrans (rowid,AcctID, amount, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc)
 --select * from #temp

 --SET IDENTITY_INSERT #tempTrans OFF

 select @count = count(*) from #temp

 If (@count = 0)
 begin
	insert into #temp (AcctID, amount, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc,ItemID,ItemDesc,TypeID,STax,STaxName,
	STaxRate,STaxAmt,STaxGL,GSTRate,GTaxAmt,	GSTTaxGL,STaxType,UTaxType,
	Warehouse ,WHLocID ,Warehousefdesc ,Locationfdesc,GTax,Price,OrderedQuan ,Ordered )
	
	values(0,'',0,0,0,0,0,0,0,0,'','','','','',0,'','','','',0,'',0,0,'',0,0,0,0,0,0,0,0,'',0,'','',0,0,0,0);
 end


 select *,1 as IsPO from #temp
 
 drop table #temp
 --drop table #tempTrans
 

END
GO