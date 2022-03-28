CREATE Procedure [dbo].[spUpdateDataPaymentDetail] 

AS
BEGIN
--Invoice
Update PaymentDetails
set IsInvoice = 1
where IsInvoice is null and (select count(*) from OpenAR where Ref=PaymentDetails.InvoiceID and type =0)=1
--Credit
Update PaymentDetails
set IsInvoice = 0
where IsInvoice is null and (select count(*) from OpenAR where Ref=PaymentDetails.InvoiceID and type =2)=1

--Deposit
Update PaymentDetails
set IsInvoice = 2
where IsInvoice is null and (select count(*) from OpenAR where Ref=PaymentDetails.InvoiceID and type =1)=1


--Invoice
Update PaymentDetails
set RefTranID = (select TransID from Invoice where Ref=PaymentDetails.InvoiceID)
where IsInvoice=1 and RefTranID is NULL
--Credit
Update PaymentDetails
set RefTranID = (select TransID from OpenAR where Ref=PaymentDetails.InvoiceID and Type=2)
where IsInvoice=0  and RefTranID is NULL

--Deposit
Update PaymentDetails
set RefTranID = (select TransID from OpenAR where Ref=PaymentDetails.InvoiceID  and type =1)
where IsInvoice=2  and RefTranID is null and (select count(*) from OpenAR where Ref=PaymentDetails.InvoiceID and type =1)=1

END  