CREATE PROCEDURE [dbo].[spGetGanttTasksByPlannerID]
	@plannerID int = 0
AS

Select 
g.PlannerTaskID TaskID
, gp.PlannerTaskID ParentID
, g.Title
, g.[Start]
, g.[End]
, g.CusDuration
, ISNULL(g.PercentComplete,0)*100 PercentComplete
, ISNULL(g.CusActualHour,0) CusActualHour
, g.Vendor
, g.VendorID
, g.Description
, gd.PlannerTaskID Dependency
FROM GanttTasks g WITH (NOLOCK)
LEFT JOIN GanttTasks gp WITH (NOLOCK) on gp.id = g.ParentID
LEFT JOIN GanttDependencies d WITH (NOLOCK) on d.SuccessorID = g.ID
LEFT JOIN GanttTasks gd WITH (NOLOCK) on gd.ID = d.PredecessorID
WHERE g.PlannerID = @plannerID