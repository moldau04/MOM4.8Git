CREATE PROCEDURE [dbo].[spGetTaskReportFiltersValue]
	@DbName varchar(50)
	
AS
BEGIN

  select distinct [Task#] from vw_TaskReportDetails where [Task#] != '' order by [Task#]
    
  select distinct Name from vw_TaskReportDetails where Name != '' order by Name

  select distinct [Due Date] from vw_TaskReportDetails where [Due Date] != '' order by [Due Date]

  select distinct [#Days] from vw_TaskReportDetails where [#Days] != '' order by [#Days]

  select distinct [Subject] from vw_TaskReportDetails where [Subject] != '' order by [Subject]

  select distinct Cast([Desc] as varchar(MAX)) As [Desc] from vw_TaskReportDetails where Cast([Desc] as varchar(MAX)) != '' order by Cast([Desc] as varchar(MAX))

  select distinct Cast(Resolution as varchar(MAX)) As Resolution from vw_TaskReportDetails where Cast(Resolution as varchar(MAX)) != '' order by Cast(Resolution as varchar(MAX)) 
 
  select distinct [Assigned To] from vw_TaskReportDetails where [Assigned To] != '' order by [Assigned To]
  
  select distinct Status from vw_TaskReportDetails where Status != '' order by Status  

END
