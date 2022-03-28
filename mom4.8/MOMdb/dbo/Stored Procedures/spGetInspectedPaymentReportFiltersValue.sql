CREATE PROCEDURE [dbo].[spGetInspectedPaymentReportFiltersValue]
	@DbName varchar(50)
	
AS
BEGIN
  
  select distinct Customer from vw_InspectedPaymentDetails where Customer != '' order by Customer

  select distinct Location from vw_InspectedPaymentDetails where Location != '' order by Location
  
  select distinct City from vw_InspectedPaymentDetails where City != '' order by City

  select distinct State from vw_InspectedPaymentDetails where State != '' order by State

  select distinct Zip from vw_InspectedPaymentDetails where Zip != '' order by Zip

  select distinct Status from vw_InspectedPaymentDetails where Status != '' order by Status

END
