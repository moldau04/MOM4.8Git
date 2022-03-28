CREATE PROCEDURE [dbo].[spGetEstimateReportFiltersValue]
	@DbName varchar(50)	
AS
BEGIN

  select distinct Id from vw_EstimateReportDetails where Id != ''  order by Id

  select distinct Name from vw_EstimateReportDetails where Name != ''  order by Name

  select distinct Description from vw_EstimateReportDetails where Description != ''  order by Description

  select distinct Remarks from vw_EstimateReportDetails where Remarks != ''  order by Remarks

  select distinct Job from vw_EstimateReportDetails where Job != ''  order by Job

  select distinct Contact from vw_EstimateReportDetails where Contact != ''  order by Contact

  select distinct Type from vw_EstimateReportDetails where Type != ''  order by Type

  select distinct Status from vw_EstimateReportDetails where Status != ''  order by Status

  select distinct [Estimate Price] from vw_EstimateReportDetails where [Estimate Price] >=0  order by [Estimate Price]

  select distinct [Quoted Price] from vw_EstimateReportDetails where [Quoted Price] >=0  order by [Quoted Price]
  
End