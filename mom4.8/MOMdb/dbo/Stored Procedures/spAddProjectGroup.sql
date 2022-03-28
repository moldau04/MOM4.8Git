CREATE PROCEDURE [dbo].[spAddProjectGroup]
	@LocId int = 0,
	@GroupName varchar(255),
	@ProjectId int = 0,
	@GroupId int = 0,
	@EquipItem  tblTypeEquipItem readonly
AS

DECLARE @NewGroupId int

BEGIN TRY
	BEGIN TRANSACTION

	DECLARE @RolId INT

	SELECT @RolId=rol from loc where loc=@LocId
		-- Add new group 
	IF @GroupId = 0
	BEGIN

		SET @NewGroupId = (SELECT (MAX(ISNULL(ID,0))+1) FROM tblEstimateGroup)

		IF @NewGroupId IS NULL SET @NewGroupId=1;
			 
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


		IF(@ProjectId <> 0)
		BEGIN
			-- Adding Group from Estimate to project
			IF NOT EXISTS (SELECT 1 FROM tblProjectGroup WHERE ProjectId = @ProjectId AND GroupId = @NewGroupId)
			BEGIN 
				INSERT INTO tblProjectGroup VALUES (@ProjectId,@NewGroupId)
			END
		END
	END 
	ELSE
	BEGIN
		--UPDATE tblEstimateGroup SET GroupName = @GroupName WHERE Id = @GroupId
		--	SET @NewGroupId = @GroupId
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

	------------------------ BEGIN INSERT EQUIPMENT ITEMS (INSERT ESTIMATE) ----------------------

	-- Delete all the old equipment the group
	DELETE tblEstimateGroupEquipment WHERE GroupId = @NewGroupId
	-- And replace it by the new one
	INSERT INTO tblEstimateGroupEquipment (GroupId, EquipmentID) SELECT @NewGroupId, EquipmentID  FROM @EquipItem

	------------------------ END INSERT EQUIPMENT ITEMS (INSERT ESTIMATE) ------------------------

	COMMIT   
	--Table[0]: get all group of project
	EXEC spGetProjectGroupNames @ProjectId
	--Table[1]: GroupID
	SELECT @NewGroupId
END TRY
BEGIN CATCH
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


