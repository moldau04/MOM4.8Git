CREATE PROCEDURE [dbo].[spGetCheckRecurr]  
 @CDID INT
AS  
BEGIN  
 SET NOCOUNT ON;  
   
 --DECLARE @CDID INT = (SELECT ISNULL(MAX(ID),0)+1 FROM CDRecurr)  
  
BEGIN TRY  
BEGIN TRANSACTION  
 
 create table #temp  
 (  
  rowid int IDENTITY(1,1), AcctID int,  amount varchar(12),  Quan numeric(30,2),  batch int,    id int,    line smallint,    ref int,    sel smallint,    type smallint,    PhaseID smallint,  
  JobId int,    strRef varchar(50),    AcctNo varchar(250),    fDesc varchar(max),    AcctName varchar(150),    UseTax varchar(15),    UtaxGL int,    UName varchar(25),    jobName varchar(150),  
  phase varchar(255),    loc varchar(100),     ItemID int,    ItemDesc varchar(8000),    TypeID int,    Ticket int,    OpSq varchar(150),    PrvIn numeric(30,2),     PrvInQuan numeric(30,2),   
  OutstandQuan numeric(30,2),    OutstandBalance numeric(30,2) ,  [STax] [smallint] NULL,	[STaxName] [varchar](50) NULL,	[STaxRate] [numeric](30, 4) NULL,	[STaxAmt] [numeric](30, 4) NULL,
	[STaxGL] [int] NULL,	[GSTRate] [numeric](30, 4) NULL,	[GTaxAmt] [numeric](30, 4) NULL,	[GSTTaxGL] [int] NULL,	[STaxType] [int] NULL,	[UTaxType] [int] NULL,
	Warehouse varchar(100) NULL,	WHLocID int,	Warehousefdesc VARCHAR(500) NULL,	Locationfdesc VARCHAR(500) NULL
 ) 
 insert into #temp  (AcctID, amount,Quan, batch, id,line,ref,sel,type,PhaseID,JobId,strRef,AcctNo,fDesc,AcctName,UseTax,UtaxGL,UName,jobName,phase,loc,ItemID,ItemDesc,TypeID,Ticket,OpSq,PrvIn,PrvInQuan,OutstandQuan,OutstandBalance,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GTaxAmt,GSTTaxGL,STaxType,UTaxType,Warehouse ,WHLocID,Warehousefdesc ,Locationfdesc )  
 SELECT pjri.AcctID,pjri.Amount,pjri.Quan,0,pjri.ID,pjri.Line,0,0,0,pjri.PhaseID,pjri.JobID,pj.Ref,c.Acct+' - '+c.fDesc,pjri.fDesc, c.fDesc,pjri.UseTax,pjri.UTaxGL,pjri.UtaxName,
 isnull(CONVERT(varchar, j.ID)+', '+j.fDesc,'') ,pjri.Phase, isnull((select Tag from Loc where Loc = j.Loc),'') as loc,pjri.ItemID,pjri.ItemDesc,pjri.TypeID,pjri.Ticket,pjri.OpSq,0,0,0,0,
 pjri.STax,pjri.STaxName,pjri.STaxRate,pjri.STaxAmt,pjri.STaxGL,pjri.GSTRate,pjri.GSTTaxAmt,pjri.GSTTaxGL,
 isnull((SELECT Type FROM Stax WHERE UType = 0 AND Name = pjri.STaxName),0) as STaxType,
  isnull((SELECT Type FROM Stax WHERE UType = 1 AND Name = pjri.UtaxName),0) as UTaxType,pjri.Warehouse,pjri.WHLocID,
  isnull((SELECT NAME FROM Warehouse WHERE ID IN (pjri.Warehouse)),'') as Warehousefdesc,
  isnull((SELECT Name FROM WHLoc WHERE ID IN (pjri.WHLocID)),'') as Locationfdesc
 FROM PJRecurr AS pj INNER JOIN  CDRecurr as cd ON pj.ID = cd.PJID INNER JOIN PJRecurrI pjri on pj.ID = pjri.PJID 
INNER JOIN Chart c on c.ID = pjri.AcctID
LEFT JOIN Job as j ON pjri.JobID = j.ID  
WHERE cd.ID=@CDID

 select * from #temp
 drop table #temp 
  
   
COMMIT   
 END TRY  
 BEGIN CATCH  
  
 SELECT ERROR_MESSAGE()  
  
    IF @@TRANCOUNT>0  
        ROLLBACK   
  RAISERROR ('An error has occurred on this page.',16,1)  
        RETURN  
  
 END CATCH  
 RETURN @CDID  
END  