-- =============================================
-- Author:		Nitin
-- Create date: 07-July

-- =============================================
CREATE PROCEDURE [dbo].[spGetCustReportFiltersValue]
	@DbName varchar(50)
	
AS
BEGIN
	


  select distinct Name from CustomerReportDetails where Name != '' order by Name
  
  select distinct Address from CustomerReportDetails where [Address] != '' order by [Address]
  
  select distinct City from CustomerReportDetails where City != '' order by City

  select distinct State from CustomerReportDetails where State != '' order by State

  select distinct [Type] from CustomerReportDetails where [Type] != '' order by [Type]

  select distinct Status from CustomerReportDetails where Status != '' order by Status

END
