CREATE PROCEDURE [dbo].[spGetBillReportFiltersValue]
@DbName varchar(50)
	
AS
BEGIN

  select distinct [Invoice Date] from vw_BillReportDetails where [Invoice Date] != ''  order by [Invoice Date]

  select distinct Ref from vw_BillReportDetails where Ref !=''  order by Ref
  
  select distinct Description from vw_BillReportDetails where Description != '' order by Description

  select distinct Amount from vw_BillReportDetails where Amount >= 0 order by Amount

  select distinct Status from vw_BillReportDetails where Status !=''  order by Status

  select distinct [Use Tax] from vw_BillReportDetails where [Use Tax] >= 0 order by [Use Tax]

  select distinct [Vendor Name] from vw_BillReportDetails where [Vendor Name] != ''  order by [Vendor Name]

  select distinct [Posting Date] from vw_BillReportDetails where [Posting Date] != ''  order by [Posting Date]

END