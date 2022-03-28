CREATE PROCEDURE [dbo].[spGetInvoiceReportFiltersValue]
	@DbName varchar(50)
	
AS
BEGIN

  select distinct [Invoice Date] from vw_InvoiceReportDetails where [Invoice Date] != ''  order by [Invoice Date]
  
  select distinct Location  from vw_InvoiceReportDetails where Location != '' order by Location

  select distinct [Location Name] from vw_InvoiceReportDetails where [Location Name] != '' order by [Location Name]

  select Distinct Cast(Description as varchar(MAX)) As Description from vw_InvoiceReportDetails where Cast(Description as Varchar(Max)) != '' order by Cast(Description as Varchar(Max))

  select distinct Cast([Location Remarks] as varchar(MAX)) As [Location Remarks] from vw_InvoiceReportDetails where Cast([Location Remarks] as varchar(MAX)) != '' order by Cast([Location Remarks] as varchar(MAX))

  select distinct Cast([Job Remarks] as varchar(MAX)) As [Job Remarks] from vw_InvoiceReportDetails where Cast([Job Remarks] as varchar(MAX))  != ''  order by Cast([Job Remarks] as varchar(MAX)) 

  select distinct [Pre Tax Amount] from vw_InvoiceReportDetails where [Pre Tax Amount] >=0 order by [Pre Tax Amount]

  select distinct [Sales Tax] from vw_InvoiceReportDetails where [Sales Tax] >=0  order by [Sales Tax]
  
  select distinct Total from vw_InvoiceReportDetails where Total >=0 order by Total

  select distinct [Manual Invoice] from vw_InvoiceReportDetails where [Manual Invoice] != '' order by [Manual Invoice]

  select distinct Status from vw_InvoiceReportDetails where Status != ''  order by Status

  select distinct PO from vw_InvoiceReportDetails where PO != ''  order by PO
  
  select distinct [Customer Name] from vw_InvoiceReportDetails where [Customer Name] != '' order by [Customer Name]

   select distinct [Department Type] from vw_InvoiceReportDetails where [Department Type] != '' order by [Department Type]

   select distinct [Amount Due] from vw_InvoiceReportDetails where [Amount Due] >=0 order by [Amount Due]

   select distinct [Due Date] from vw_InvoiceReportDetails where [Due Date] != ''  order by [Due Date]

   select distinct [Invoice#] from vw_InvoiceReportDetails where [Invoice#] != ''  order by [Invoice#]
End