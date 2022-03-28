CREATE PROCEDURE [dbo].[spGetRecReportFiltersValue]
	@DbName varchar(50)
	
AS
BEGIN
	  --Rahil

   select distinct Customer from vw_RecurReportDetails where Customer != '' order by Customer

  select distinct [Location Id] from vw_RecurReportDetails where [Location Id] != '' order by [Location Id]
  
  select distinct Location from vw_RecurReportDetails where Location != '' order by Location

  select distinct [Loc Type] from vw_RecurReportDetails where [Loc Type] != '' order by [Loc Type]

  select distinct [Service Type] from vw_RecurReportDetails where [Service Type] != '' order by [Service Type]

  select distinct Description from vw_RecurReportDetails where Description != '' order by Description

  select distinct [Preferred Worker] from vw_RecurReportDetails where [Preferred Worker] != '' order by  [Preferred Worker]

  select distinct [Ticket Start] from vw_RecurReportDetails where [Ticket Start] != '' order by [Ticket Start]

  select distinct [Ticket Time] from vw_RecurReportDetails where [Ticket Time] != '' order by [Ticket Time]

  select distinct Hours from vw_RecurReportDetails where Hours >= 0 order by Hours
  
  select distinct [Ticket Freq] from vw_RecurReportDetails where [Ticket Freq] != '' order by [Ticket Freq]

  select distinct [Bill Start] from vw_RecurReportDetails where [Bill Start] != '' order by [Bill Start]

  select distinct [Bill Amount] from vw_RecurReportDetails where [Bill Amount] >= 0 order by [Bill Amount]

  select distinct [Bill Freqency] from vw_RecurReportDetails where [Bill Freqency] != '' order by [Bill Freqency]
  
  select distinct Status from vw_RecurReportDetails where Status != '' order by Status

   select distinct Equipment from vw_RecurReportDetails where Equipment != '' order by Equipment

  select distinct Expiration from vw_RecurReportDetails where Expiration != '' order by Expiration

  select distinct [Expiration Date] from vw_RecurReportDetails where [Expiration Date] != '' order by [Expiration Date]

  select distinct [Phone Monitoring] from vw_RecurReportDetails where [Phone Monitoring] != '' order by [Phone Monitoring]

  select distinct [Contract Type] from vw_RecurReportDetails where [Contract Type] != '' order by [Contract Type]

  select distinct [Occupancy Discount] from vw_RecurReportDetails where [Occupancy Discount] != '' order by [Occupancy Discount]

  select distinct Exclusions from vw_RecurReportDetails where Exclusions != '' order by Exclusions

  select distinct [Term Of Contract] from vw_RecurReportDetails where [Term Of Contract] != '' order by [Term Of Contract]

  select distinct [Price Adjustment Cap] from vw_RecurReportDetails where [Price Adjustment Cap] != '' order by [Price Adjustment Cap]

  select distinct [Fire Service Testing Included] from vw_RecurReportDetails where [Fire Service Testing Included] != '' order by [Fire Service Testing Included]

  select distinct [Special Rates] from vw_RecurReportDetails where [Special Rates] != '' order by [Special Rates]

  select distinct [Contract Expiration] from vw_RecurReportDetails where [Contract Expiration] != '' order by [Contract Expiration]

  select distinct [Prorated Items] from vw_RecurReportDetails where [Prorated Items] != '' order by [Prorated Items]

  select distinct [Annual Test Included] from vw_RecurReportDetails where [Annual Test Included] != '' order by [Annual Test Included]

  select distinct [Five Year State Test Included] from vw_RecurReportDetails where [Five Year State Test Included] != '' order by [Five Year State Test Included]

  select distinct [Fire Service Tested Included] from vw_RecurReportDetails where [Fire Service Tested Included] != '' order by [Fire Service Tested Included]

  select distinct [Cancellation Notification Days] from vw_RecurReportDetails where [Cancellation Notification Days] != '' order by [Cancellation Notification Days]

  select distinct [Price Adjustment Notification Days] from vw_RecurReportDetails where [Price Adjustment Notification Days] != '' order by [Price Adjustment Notification Days]

  select distinct [After Hours Calls Included] from vw_RecurReportDetails where [After Hours Calls Included] != '' order by [After Hours Calls Included]

  select distinct [OG Service Calls Included] from vw_RecurReportDetails where [OG Service Calls Included] != '' order by [OG Service Calls Included]

  select distinct [Contract Hours] from vw_RecurReportDetails where [Contract Hours] != '' order by [Contract Hours]

  select distinct [Contract Format] from vw_RecurReportDetails where [Contract Format] != '' order by [Contract Format]

END
