CREATE PROCEDURE [dbo].[spGetOpportunityReportFiltersValue]
		@DbName varchar(50)	
AS
BEGIN

  select distinct [Opportunity#] from vw_OpportunityReportDetails where [Opportunity#] != ''  order by [Opportunity#]

  select distinct Contact from vw_OpportunityReportDetails where Contact != ''  order by Contact

  select distinct Name from vw_OpportunityReportDetails where Name != ''  order by Name

  select distinct Status from vw_OpportunityReportDetails where Status != ''  order by Status

  select distinct Probability from vw_OpportunityReportDetails where Probability != ''  order by Probability

  select distinct [Close Date] from vw_OpportunityReportDetails where [Close Date] != ''  order by [Close Date]

  select distinct Closed from vw_OpportunityReportDetails where Closed != ''  order by Closed

  select distinct Amount from vw_OpportunityReportDetails where Amount >= 0  order by Amount

  select distinct [Sales Person] from vw_OpportunityReportDetails where [Sales Person] != ''  order by [Sales Person]

  select distinct [Estimate#] from vw_OpportunityReportDetails where [Estimate#] != ''  order by [Estimate#]

  select distinct [Project#] from vw_OpportunityReportDetails where [Project#] != ''  order by [Project#]
  
End
