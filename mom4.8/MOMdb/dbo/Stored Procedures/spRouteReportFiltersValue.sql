CREATE PROCEDURE [dbo].[spRouteReportFiltersValue]
	@DbName varchar(50)
	
AS
BEGIN

  select distinct Name from vw_RouteReportDetails where Name != '' order by Name

  select distinct MechName from vw_RouteReportDetails where MechName != '' order by MechName

  select distinct Hour from vw_RouteReportDetails where Hour >=0  order by Hour
  
  select distinct Amount from vw_RouteReportDetails where Amount >=0  order by Amount

  select distinct Remarks from vw_RouteReportDetails where Remarks != ''  order by Remarks

  End
  