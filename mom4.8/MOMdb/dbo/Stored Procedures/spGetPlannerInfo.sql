CREATE PROCEDURE [dbo].[spGetPlannerInfo]
	@ProjectID INT
	--, @PlannerID INT
AS
BEGIN
	DECLARE @ID AS INT
	DECLARE @DESC AS VARCHAR(500)
	DECLARE @START_DATE AS DATETIME
	DECLARE @END_DATE AS DATETIME
	DECLARE @NO_OF_TASK AS INT
	DECLARE @NEXT_DUE_TASK AS INT
	DECLARE @PROGRESS_TASK AS INT
	DECLARE @TOTAL_HOURS AS DECIMAL
	DECLARE @TOTAL_DAYS AS DECIMAL

	SELECT Top 1 @ID=pl.ID,@DESC=pl.[Desc] From Planner pl INNER JOIN GanttTasks gt ON pl.ID = gt.PlannerID
	Where PID=@ProjectID AND (pl.Type is null or pl.Type = '' or pl.Type = 'project')
	--SET @START_DATE=(SELECT TOP 1 StartDate FROM Tasks WHERE parentIdRaw IS NOT NULL AND StartDate IS NOT NULL AND ProjectID=@ProjectID  ORDER BY StartDate)
	--SET @END_DATE=(SELECT TOP 1 EndDate FROM Tasks WHERE parentIdRaw IS NOT NULL AND EndDate IS NOT NULL AND ProjectID=@ProjectID  ORDER BY EndDate DESC)
	--SET @NO_OF_TASK=(SELECT COUNT(*) FROM Tasks WHERE parentIdRaw IS NOT NULL AND ProjectID=@ProjectID)
	--SET @NEXT_DUE_TASK=(SELECT COUNT(*) FROM Tasks WHERE (PercentDone IS NULL OR PercentDone=0) AND parentIdRaw IS NOT NULL AND ProjectID=@ProjectID)
	--SET @PROGRESS_TASK=(SELECT COUNT(*) FROM Tasks WHERE PercentDone IS NOT NULL AND PercentDone!=0 AND parentIdRaw IS NOT NULL AND ProjectID=@ProjectID)
	--SET @TOTAL_HOURS=((SELECT SUM(Duration) FROM Tasks WHERE parentIdRaw IS NULL AND ProjectID=@ProjectID)*8)
	--SET @TOTAL_DAYS=(SELECT SUM(Duration) FROM Tasks WHERE parentIdRaw IS NULL AND ProjectID=@ProjectID)

	SELECT @START_DATE=MIN(Start),@END_DATE=MAX([END]) FROM GanttTasks WHERE ProjectID=@ProjectID AND PlannerID=@ID AND ParentID is null
	SELECT @NO_OF_TASK=COUNT(*) FROM GanttTasks WHERE ProjectID=@ProjectID AND PlannerID=@ID and Summary = 0
	SELECT @NEXT_DUE_TASK=COUNT(*) FROM GanttTasks WHERE (PercentComplete IS NULL OR PercentComplete=0) AND PlannerID=@ID AND Summary = 0 AND ProjectID=@ProjectID
	SELECT @PROGRESS_TASK=COUNT(*) FROM GanttTasks WHERE (PercentComplete IS NOT NULL AND PercentComplete!=0) AND PlannerID=@ID AND Summary = 0 AND ProjectID=@ProjectID
	SELECT @TOTAL_HOURS=SUM(CusDuration) FROM GanttTasks WHERE ProjectID=@ProjectID AND PlannerID=@ID and Summary = 0
	SET @TOTAL_DAYS=@TOTAL_HOURS/8

	SELECT @ID AS ID
		, @DESC AS [Desc]
		, @START_DATE AS StartDate
		, @END_DATE AS EndDate
		, @NO_OF_TASK AS NoOfTask
		, @NEXT_DUE_TASK AS NextDueTask
		, @PROGRESS_TASK AS ProgressTask
		, @TOTAL_HOURS AS TotalHours
		, @TOTAL_DAYS AS TotalDays

END
