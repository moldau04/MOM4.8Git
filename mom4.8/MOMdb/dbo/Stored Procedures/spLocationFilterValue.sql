CREATE PROCEDURE [dbo].[spLocationFilterValue]
	@DbName varchar(50)
	
AS
BEGIN

Select Distinct Customer from vw_LocationReportDetails where Customer != ''

Select Distinct Location from vw_LocationReportDetails where Location != ''

Select Distinct City from vw_LocationReportDetails where City != ''

Select Distinct State from vw_LocationReportDetails where State != ''

Select Distinct Zip from vw_LocationReportDetails where Zip != ''

Select Distinct Address from vw_LocationReportDetails where Address != ''

Select Distinct LocationSTax from vw_LocationReportDetails where LocationSTax != ''

Select Distinct Type from vw_LocationReportDetails where Type != ''

Select Distinct TaxDesc from vw_LocationReportDetails where TaxDesc != ''

Select Distinct TaxName from vw_LocationReportDetails where TaxName != ''

Select Distinct TaxRate from vw_LocationReportDetails where TaxRate >= 0

Select Distinct SalesPerson from vw_LocationReportDetails where SalesPerson != ''

Select Distinct DefaultWorker from vw_LocationReportDetails where DefaultWorker != ''

Select Distinct Acct# from vw_LocationReportDetails where Acct# != ''

Select Distinct PreferredWorker from vw_LocationReportDetails where PreferredWorker != ''

End