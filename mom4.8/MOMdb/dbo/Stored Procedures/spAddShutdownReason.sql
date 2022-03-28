CREATE PROCEDURE [dbo].[spAddShutdownReason]
	@ShutdownReason VARCHAR(100),
	@Planned bit,
	@Username VARCHAR(100)
AS
DECLARE @IsExisted int = 0

SELECT @IsExisted = Count(*) FROM ElevShutdownReason WHERE Reason = @ShutdownReason

IF @IsExisted = 0
BEGIN
	INSERT INTO ElevShutdownReason
		(
		Reason,
		Planned,
		CreatedDate,
		CreatedBy
		)
		VALUES
		(
		@ShutdownReason,
		@Planned,
		GETDATE(),
		@Username
		)
END
ELSE
BEGIN
	RAISERROR ('This reason existed in the database. Please check and try again!',16,1)
    RETURN
END
	
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
BEGIN
    RAISERROR ('Error Occured',16,1)
    RETURN
END