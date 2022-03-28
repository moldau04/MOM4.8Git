
/*--------------------------------------------------------------------
Author: Thurstan
Created On:	 04 Oct 2018	
Description: Get All Project Template Custom  Field
Modified By: Thurstan
Modified On: 09 Oct 2018
Description: Include all project into temp table
Modified By: Thurstan
Modified On: 26 Oct 2018
Description: Change the way to get all custom label field
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spGetAllProjectTemplateCustomField] @Job
AS
[dbo].[tblJob] READONLY AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @Temp TABLE (
		IDTemp INT identity(1, 1)
		,ProjectId INT
		,ID INT
		,tblTabID INT
		,Label NVARCHAR(200)
		,Line INT
		,[Format] INT
		,IsDeleted BIT
		,FieldControl NVARCHAR(50)
		,Value NVARCHAR(50)
		,UpdateDate DATETIME
		,UserName NVARCHAR(50)
		)
	DECLARE @TempJobId TABLE (
		ID INT identity(1, 1)
		,JobId INT
		,JobType INT
		)

	INSERT INTO @TempJobId
	SELECT *
	FROM @Job

	DECLARE @Count INT = 0;
	DECLARE @RowNum INT = 1;
	DECLARE @JobId INT = 0;
	DECLARE @JobType INT = 0;

	SET @Count = (
			SELECT Count(*)
			FROM @TempJobId
			)

	WHILE (@RowNum <= @Count)
	BEGIN
		SET @JobId = (
				SELECT JobId
				FROM @TempJobId
				WHERE ID = @RowNum
				)
		SET @JobType = (
				SELECT JobType
				FROM @TempJobId
				WHERE ID = @RowNum
				)

		INSERT INTO @Temp
		EXEC [spGetProjectTemplateCustomLabelField] @JobType
			,@JobId

		SET @RowNum = @RowNum + 1;
	END

	SELECT *
	FROM @Temp

	Select distinct Label from tblCustomFields
END
GO
