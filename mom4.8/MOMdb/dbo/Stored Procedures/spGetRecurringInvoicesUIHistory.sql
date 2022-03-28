CREATE PROCEDURE [dbo].[spGetRecurringInvoicesUIHistory]	
as
BEGIN
	select top 1 isnull(TaxType,'') as TaxType, 
	ISNULL(IsCanadaCompany,0) as IsCanadaCompany,
	ISNULL(Taxable,0) as Taxable,
	PaymentTerms,
	Remarks
	FROM RecurringInvoicesUIHistory 
	
END