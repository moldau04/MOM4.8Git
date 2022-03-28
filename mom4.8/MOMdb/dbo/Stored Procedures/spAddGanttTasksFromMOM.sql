
CREATE PROCEDURE [dbo].[spAddGanttTasksFromMOM]
	@ParentID INT,
	@Name Varchar(100),
	@OrderID INT,
	@ProjectID INT,
	@PlannerID INT,
	@TaskType Varchar(50),
	@Duration float,
	@StartDate DateTime,
	@EndDate DateTime,
	@Summary bit,
	@Notes nvarchar(max),
	@VendorID int,
	@VendorName varchar(75),
	@RootVendorID int,
	@RootVendorName varchar(75),
	@ProjectName varchar(75),
	@ItemRefID int,
	@ActualHours float
AS
BEGIN
	DECLARE @ResourceID int
	DECLARE @TaskID int
	DECLARE @PlannerTaskID int = 0
	SELECT @ResourceID=ID FROM GanttResources WHERE [Name] = @VendorName

	SET @PlannerTaskID = ISNULL((SELECT MAX(PlannerTaskID) FROM GanttTasks WHERE PlannerID = @PlannerID),0) + 1

	--IF(@VendorName is not null AND @VendorName != '' AND (@ResourceID is null OR @ResourceID = 0))
	--BEGIN
	--	INSERT INTO GanttResources (Name) VALUES (@VendorName)
	--	SELECT @ResourceID = (SELECT SCOPE_IDENTITY())
	--END

	IF (@ParentID = 0) SET @ParentID = NULL

	INSERT INTO GanttTasks(parentId
		,OrderID
		,Title
		,[Start]
		,[End]
		,PercentComplete
		,Expanded
		,Summary
		,[Description]
		,ProjectID
		,PlannerID
		,CusTaskType
		,CusDuration
		,VendorID
		,Vendor
		,RootVendorID
		,RootVendorName
		,ProjectName
		,PlannerTaskID
		,ItemRefID
		,CusActualHour
		)
	VALUES(@ParentID
		,@OrderID
		,@Name
		,@StartDate
		,@EndDate
		,0
		,1
		,@Summary
		,@Notes
		,@ProjectID
		,@PlannerID
		,@TaskType
		,@Duration
		,@VendorID
		,@VendorName
		,@RootVendorID
		,@RootVendorName
		,@ProjectName
		,@PlannerTaskID
		,@ItemRefID
		,@ActualHours
		)

	SELECT @TaskID = @@Identity
	SELECT @TaskID

	-- Update GanttTaskID to JobTItem 
	IF @ItemRefID is not null AND @ItemRefID != 0
	BEGIN
		UPDATE JobTItem SET GanttTaskID = @TaskID WHERE Job = @ProjectID AND ID = @ItemRefID
	END

	--IF (@ResourceID is not null AND @ResourceID != 0)
	--BEGIN
	--	INSERT INTO GanttResourceAssignments (ResourceID,TaskID,Units) VALUES (@ResourceID,@TaskID,1)
	--END
END
