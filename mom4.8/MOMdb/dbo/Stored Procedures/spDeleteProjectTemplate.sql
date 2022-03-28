
CREATE PROCEDURE [dbo].[spDeleteProjectTemplate]
	@id int
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @CountInUsed int;

	SET @CountInUsed = ISNULL((SELECT count(*) FROM job WHERE Template = @id),0)

	IF(@CountInUsed != 0)
	BEGIN
		RAISERROR('This template is being used in projects',16,1)
		RETURN
	END

	SET @CountInUsed = ISNULL((SELECT count(*) FROM Estimate WHERE Template = @id),0)
	IF(@CountInUsed != 0)
	BEGIN
		RAISERROR('This template is being used in estimates',16,1)
		RETURN
	END
	 
	IF NOT EXISTS(SELECT 1 FROM tblCustomJobT WHERE JobTID = @id and JobID > 0)
    BEGIN
		BEGIN TRANSACTION
		CREATE table #temp
		( ID int null)

		INSERT INTO #temp
		SELECT field.ID FROM tblCustomFields as field 
			RIGHT JOIN tblCustomJobT as jobt
			ON field.ID = jobt.tblCustomFieldsID
				where jobt.JobTID = @id AND (jobt.JobID is null or jobt.JobID = 0)

		DELETE cus FROM tblCustom as cus 
			INNER JOIN tblCustomFields as field
				ON cus.tblCustomFieldsID = field.ID
			INNER JOIN tblCustomJobT as jobt
				ON jobt.tblCustomFieldsID = field.ID
				WHERE jobt.JobTID = @id AND (jobt.JobID is null or jobt.JobID = 0)
			
		DELETE jobt FROM tblCustomJobT as jobt WHERE jobt.JobTID = @id AND (jobt.JobID is null or jobt.JobID = 0)
			
		DELETE FROM tblCustomFields WHERE ID IN (select ID from #temp)

		DROP TABLE #temp
		
		DELETE b FROM BOM b INNER JOIN JobTItem j ON b.JobTItemID = j.ID 
					WHERE j.JobT = @id and (j.Job is null or j.Job = 0)

	
		DELETE m FROM Milestone m INNER JOIN JobTItem j ON m.JobTItemID = j.ID 
					WHERE j.JobT = @id and (j.Job is null or j.Job = 0)
   
		DELETE FROM [dbo].[JobTItem]
			WHERE JobT=@id and (Job is null or Job = 0)

		DELETE FROM [dbo].[JobT]
			WHERE ID=@id

		IF @@ERROR <> 0
		BEGIN
			RAISERROR ('Deleting error',16,1)
			IF @@TRANCOUNT > 0
				ROLLBACK TRANSACTION
			RETURN
		END
		COMMIT
	END
	ELSE
	BEGIN
		RAISERROR('You can not delete this project template.',16,1)
		RETURN
	END
END
