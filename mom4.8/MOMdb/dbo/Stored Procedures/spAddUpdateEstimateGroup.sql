CREATE PROCEDURE [dbo].[spAddUpdateEstimateGroup]
	@GroupId int = 0,
	@GroupName varchar(255),
	@RolId INT
AS
DECLARE @NewGroupId int
BEGIN TRY
	BEGIN TRANSACTION
		-- Add new group
		IF @GroupId = 0
		BEGIN
			SET @NewGroupId = (SELECT (MAX(ISNULL(ID,0))+1) FROM tblEstimateGroup)
			IF @NewGroupId IS NULL
			BEGIN
				SET @NewGroupId=1;
			END
			--INSERT INTO tblEstimateGroup (Id, GroupName, RolId) VALUES (@NewGroupId, @GroupName, @RolId)
			IF NOT EXISTS (SELECT 1 FROM tblEstimateGroup WHERE RolId = @RolId AND @GroupName = GroupName)
			BEGIN
				INSERT INTO tblEstimateGroup (Id, GroupName, RolId) VALUES (@NewGroupId, @GroupName, @RolId)
			END
			ELSE
			BEGIN
				ROLLBACK	
				RAISERROR ('This group name already existed',16,1)
				RETURN
			END
		END
		ELSE
		BEGIN
			--UPDATE tblEstimateGroup SET GroupName = @GroupName WHERE Id = @GroupId
			--SET @NewGroupId = @GroupId
			IF NOT EXISTS (SELECT 1 FROM tblEstimateGroup WHERE RolId = @RolId AND @GroupName = GroupName AND Id != @GroupId)
			BEGIN
				UPDATE tblEstimateGroup SET GroupName = @GroupName WHERE Id = @GroupId
					SET @NewGroupId = @GroupId
			END
			ELSE
			BEGIN
				ROLLBACK	
				RAISERROR ('This group name already existed',16,1)
				RETURN
			END
			END

	COMMIT 

	EXEC spGetEstimateGroupNameByRol @RolId

	SELECT @NewGroupId
END TRY
BEGIN CATCH

	--SELECT ERROR_MESSAGE()

	--IF @@TRANCOUNT>0
	--	ROLLBACK	
	--	RAISERROR ('An error has occurred on this page.',16,1)
	--	RETURN
	DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

	IF @@TRANCOUNT>0 ROLLBACK
	RAISERROR (@ErrorMessage, -- Message text.
				@ErrorSeverity, -- Severity.
				@ErrorState -- State.
				);
	RETURN
END CATCH


