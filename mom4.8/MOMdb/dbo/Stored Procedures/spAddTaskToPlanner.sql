CREATE PROCEDURE [dbo].[spAddTaskToPlanner]
	@ParentID INT,
	@Name Varchar(100),
	@idx INT,
	@ProjectID INT,
	@TaskType Varchar(50),
	@Duration Decimal(18,2),
	@DurationUnit nVarchar(5),
	@StartDate DateTime
AS
BEGIN
	IF (@ParentID = 0) SET @ParentID = NULL

	INSERT INTO Tasks(parentId
		,[Name]
		,Duration
		,DurationUnit
		,SchedulingMode
		,[idx]
		,expanded
		,ManuallyScheduled
		,Draggable
		,Resizable
		,[Rollup]
		,ShowInTimeline
		,ProjectID
		,TaskType
		,StartDate
		,BaselinePercentDone)
	VALUES(@ParentID,@Name,@Duration,@DurationUnit,'Normal',@idx,1,0,1,1,0,1,@ProjectID,@TaskType,@StartDate,0)

    --UPDATE Tasks SET parentIdRaw=NULL WHERE parentIdRaw=0
	
	--SET IDENTITY_INSERT bryntum_gantt.dbo.Tasks on
	
	--insert into bryntum_gantt.dbo.Tasks 
	--(
	--	[Id]					,
	--	[parentIdRaw]			,
	--	[Name]					,
	--	[StartDate]				,
	--	[EndDate]				,
	--	[Duration]				,
	--	[DurationUnit]			,
	--	[PercentDone]			,
	--	[SchedulingMode]		,
	--	[BaselineStartDate]		,
	--	[BaselineEndDate]		,
	--	[BaselinePercentDone]	,
	--	[Cls]					,
	--	[index]					,
	--	[CalendarIdRaw]			,
	--	[expanded]				,
	--	[Effort]				,
	--	[EffortUnit]			,
	--	[Note]					,
	--	[ConstraintType]		,
	--	[ConstraintDate]		,
	--	[ManuallyScheduled]		,
	--	[Draggable]				,
	--	[Resizable]				,
	--	[Rollup]				,
	--	[ShowInTimeline]		,
	--	[Color]					,
	--	[PlannerID]			    ,
	--	[ProjectID]				,
	--	[TaskType]				
	--)
	--select 
	--	[Id]					,
	--	[parentId]			,
	--	[Name]					,
	--	[StartDate]				,
	--	[EndDate]				,
	--	[Duration]				,
	--	[DurationUnit]			,
	--	[PercentDone]			,
	--	[SchedulingMode]		,
	--	[BaselineStartDate]		,
	--	[BaselineEndDate]		,
	--	[BaselinePercentDone]	,
	--	[Cls]					,
	--	[idx]					,
	--	[CalendarId]			,
	--	[expanded]				,
	--	[Effort]				,
	--	[EffortUnit]			,
	--	[Note]					,
	--	[ConstraintType]		,
	--	[ConstraintDate]		,
	--	[ManuallyScheduled]		,
	--	[Draggable]				,
	--	[Resizable]				,
	--	[Rollup]				,
	--	[ShowInTimeline]		,
	--	[Color]					,
	--	[PlannerID]			    ,
	--	[ProjectID]				,
	--	[TaskType]				

	--from Tasks where projectID = @ProjectID and id = @@Identity
	--SET IDENTITY_INSERT bryntum_gantt.dbo.Tasks off
	Select @@Identity
END
