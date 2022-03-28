CREATE PROCEDURE [dbo].[spEditShutdownReason]
	@ID int,
	@ShutdownReason VARCHAR(100),
	@Planned bit,
	@Username VARCHAR(100)
AS

DECLARE @IsExisted int = 0

SELECT @IsExisted = Count(*) FROM ElevShutdownReason WHERE ID = @ID
IF @IsExisted > 0
BEGIN
	SET @IsExisted = 0
	SELECT @IsExisted = Count(*) FROM ElevShutdownReason WHERE ID <> @ID AND Reason = @ShutdownReason
	IF @IsExisted = 0
	BEGIN
		UPDATE ElevShutdownReason SET
			Reason = @ShutdownReason,
			Planned = @Planned,
			UpdatedDate = GETDATE(),
			UpdatedBy = @Username
		WHERE ID = @ID
	END
	ELSE
	BEGIN
		RAISERROR ('This reason existed in the database. Please check and try again!',16,1)
		RETURN
	END
END
ELSE
BEGIN
	RAISERROR ('Cannot find this reason in the database. Please check and try again!',16,1)
    RETURN
END
	
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
BEGIN
    RAISERROR ('Error Occured',16,1)
    RETURN
END