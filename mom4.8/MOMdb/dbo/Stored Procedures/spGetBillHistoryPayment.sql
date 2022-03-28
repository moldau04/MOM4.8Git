CREATE PROCEDURE [dbo].[spGetBillHistoryPayment]    
@ID int     
AS    
BEGIN    
    
Declare @status varchar(100)    
Declare @acct int    
Declare @loc int    
--select @status=isnull(status,'') ,@loc=isnull(AcctSub,0),@acct=isnull(Acct,0) from Trans where Ref=@ID and type=1    
SELECT * FROM (    
SELECT 'Check Payment' as Type, cd.ID as ReceivedPaymentID    
 ,cd.fDate as PaymentDate    
 ,cd.Memo +' #'+ CONVERT(VARCHAR(MAX),cd.Ref) as fDesc    
 ,cd.Amount as Amount       
 --,CONCAT('editcheck.aspx?id=', cd.ID )  as link    
 ,'editcheck.aspx?id='+CAST(cd.ID AS VARCHAR)+'&frm=MNG1'  as link 
 FROM CD cd  INNER JOIN Paid p ON cd.ID = p.PITR INNER JOIN PJ pj ON p.TRID = pj.TRID where pj.ID = @ID and pj.status in (1,3)    
 Union all    
select    
 'Credit Apply' as Type   
 ,cp.PITR as ReceivedPaymentID    
 ,cp.fDate as PaymentDate   
 ,CASE WHEN cp.FromPJID = @ID AND cp.ToPJID <> 0 THEN CONCAT('Appy Credit To #', cp.Ref ) WHEN cp.ToPJID = @ID THEN CONCAT('Appy Credit From #', (SELECT pj.Ref FROM PJ pj WHERE pj.ID = cp.FromPJID) ) END as fDesc   
 ,CASE WHEN cp.FromPJID = @ID AND cp.ToPJID <> 0 THEN cp.Paid*(-1) WHEN cp.ToPJID = @ID THEN cp.Paid END as Amount     
 ,'' as link    
  from CreditPaid cp where (cp.FromPJID = @ID OR cp.ToPJID =@ID) AND cp.ToPJID <> 0   
) AS history     
ORDER BY PaymentDate     
    
END    
GO