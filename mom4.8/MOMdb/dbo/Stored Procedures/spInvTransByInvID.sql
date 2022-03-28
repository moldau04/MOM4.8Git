CREATE PROCEDURE [dbo].[spInvTransByInvID] @ID int,
@FromDate datetime=null,
@EndDate datetime=null,
@EN int = NULL,
@UserID int
AS
BEGIN  
 

if(@FromDate is null)   set @FromDate ='01/01/1980' if(@EndDate is null)   set @FromDate =GETDATE();
 
	---------------------------------------------------
	SELECT  

 case	Trans.Type when  41 then PJ.ID  when  80 then ReceivePO.ID  else Trans.Ref  end as ref,
 case Trans.type 
 when 60 then 'AddInventoryAdjustment?id='+ CAST( Trans.Ref AS varchar (100) )+''  
 when 97 then 'AddInventoryAdjustment?id='+ CAST( Trans.Ref AS varchar (100) )+''
 when 81 then 'addreceivepo?id='+ CAST( ReceivePO.ID AS varchar (100) )+'' 
 when 41 then 'addbills?id='+ CAST( PJ.ID AS varchar (100) )+''
 when 70 then 'addticket?id='+ CAST( Trans.Ref AS varchar (100) )+'&comp=1&pop=1'  end AS URLref ,
Trans.fDate, 
 case Trans.type 
 when 60 then 'Item Adjustment'  when 97 then 'Inventory Item Transfer' when 81 then 'RPO'
 when 41 then 'APBill' when 70 then 'PostToProject/InventoryUsed' end AS TType,
Trans.fDesc AS MDesc, 
Trans.AcctSub as INVID,
isnull(Trans.Status,0) as Quan  ,
Trans.Amount,  
isnull((CASE WHEN Trans.Amount >=0 THEN Trans.Amount END) ,0) AS Charges,
isnull((CASE WHEN Trans.Amount < 0 THEN (Trans.Amount) END),0)As Credits,  
SUM(Trans.Amount) OVER(ORDER BY Trans.Id,Trans.fdate) Balance  
FROM Trans 
left join PJ on pj.batch=Trans.Batch
left join ReceivePO on ReceivePO.batch=Trans.Batch
WHERE Trans.Acct in (select ID from Chart where Type=0) 
AND Trans.fDate>=@FromDate 
AND Trans.fDate<=@EndDate
and Trans.AcctSub=@ID
AND Trans.Type   IN ( 60,97,81,41,70)    
order by Trans.Id,Trans.fdate  asc

    
	 

END