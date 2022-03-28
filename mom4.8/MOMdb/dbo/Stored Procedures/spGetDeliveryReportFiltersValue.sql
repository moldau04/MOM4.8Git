CREATE PROCEDURE [dbo].[spGetDeliveryReportFiltersValue]
	@DbName varchar(50)
	
AS
BEGIN
  
  select distinct Customer from vw_DeliveryReportDetails where Customer != '' order by Customer

  select distinct Location from vw_DeliveryReportDetails where Location != '' order by Location
  
  select distinct City from vw_DeliveryReportDetails where City != '' order by City

  select distinct State from vw_DeliveryReportDetails where State != '' order by State

  select distinct Zip from vw_DeliveryReportDetails where Zip != '' order by Zip

  select distinct Status from vw_DeliveryReportDetails where Status != '' order by Status

END
