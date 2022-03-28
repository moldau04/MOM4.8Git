CREATE PROCEDURE [dbo].[spUpdateViolationCode]
	@ID INT,
	@Code VARCHAR(200),
	@Desc VARCHAR(500),
	@SectionID int,
	@CategoryID int
AS
DECLARE @IsExisted int = 0

SELECT @IsExisted = Count(*) FROM ViolationCode WHERE ID = @ID
IF @IsExisted > 0
BEGIN
	SET @IsExisted = 0
	SELECT @IsExisted = Count(*) FROM ViolationCode WHERE ID <> @ID AND [Code] = @Code
	IF @IsExisted = 0
	BEGIN
		UPDATE ViolationCode SET
			[Code] =@Code,
			[Description] = @Desc,
			[SectionID]=@SectionID,
			[CategoryID]=@CategoryID
		WHERE ID = @ID
	END
	ELSE
	BEGIN
		RAISERROR ('This violation code already exists in the database. Please check and try again!',16,1)
		RETURN
	END
END
ELSE
BEGIN
	RAISERROR ('Cannot find this violation code in the database. Please check and try again!',16,1)
    RETURN
END
	
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
BEGIN
    RAISERROR ('Error Occured',16,1)
    RETURN
END