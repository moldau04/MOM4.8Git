CREATE PROCEDURE [dbo].[spGetLeadReportFiltersValue]
	@DbName varchar(50)
	
AS
BEGIN

  select distinct [Prospect#] from vw_LeadReportDetails where [Prospect#] != '' order by [Prospect#]
    
  select distinct [Type] from vw_LeadReportDetails where [Type] != '' order by [Type]

  select distinct [Customer Name] from vw_LeadReportDetails where [Customer Name] != '' order by [Customer Name]

  select distinct Name from vw_LeadReportDetails where Name != '' order by Name

  select distinct City from vw_LeadReportDetails where City != '' order by City

  select distinct State from vw_LeadReportDetails where State != '' order by State

  select distinct Status from vw_LeadReportDetails where Status != '' order by Status 
 
  select distinct [#Days] from vw_LeadReportDetails where [#Days] != '' order by [#Days]
  
  select distinct [#Opps] from vw_LeadReportDetails where [#Opps] >= 0 order by [#Opps]  

  select distinct [Sales Person] from vw_LeadReportDetails where [Sales Person] != '' order by [Sales Person]

END
