Create PROCEDURE [dbo].[spGetInvoiceHistoryPayment]
@ID int 
AS
BEGIN

Declare @status varchar(100)
Declare @acct int
Declare @loc int
select @status=isnull(status,'') ,@loc=isnull(AcctSub,0),@acct=isnull(Acct,0) from Trans where Ref=@ID and type=1
SELECT * FROM (
SELECT 'Payment' as Type, pd.ReceivedPaymentID as ReceivedPaymentID
	,t.fDate as PaymentDate
	,t.fDesc +' '+ rp.CheckNumber as fDesc
	,t.Amount as Amount
	
	,CONCAT('addreceivepayment.aspx?page=addinvoice&invoiceId=', CONVERT(VARCHAR,@ID) ,'&id=', CONVERT(VARCHAR,pd.ReceivedPaymentID))  as link
	FROM PaymentDetails pd
	inner join ReceivedPayment rp on rp.ID=pd.ReceivedPaymentID
	inner join Trans t on pd.TransID= t.ID
	where pd.InvoiceID=@ID
	Union all
select
	CASE
		When Trans.Type=1 Then 'Invoice'
		When Trans.Type=6 Then 'Payment'
	END as Type
	,Ref as ReceivedPaymentID
	,fDate as PaymentDate
	,fDesc as fDesc
	,Amount as Amount
	,CASE
		When Trans.Type=1 Then ''
		When Trans.Type=6 Then CONCAT('adddeposit.aspx?page=addinvoice&invoiceId=', CONVERT(VARCHAR,@ID) ,'&id=', CONVERT(VARCHAR,Ref))
	END as link
	 from Trans  where Status=@status  and Acct=@acct and AcctSub=@loc and Status<>''
) AS history 
ORDER BY PaymentDate 

END