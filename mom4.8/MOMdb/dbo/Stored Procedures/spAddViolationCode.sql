CREATE PROCEDURE [dbo].[spAddViolationCode]
	@Code VARCHAR(200),
	@Desc VARCHAR(500),
	@SectionID int,
	@CategoryID int
AS
DECLARE @IsExisted int = 0

SELECT @IsExisted = Count(*) FROM ViolationCode WHERE Code = @Code

IF @IsExisted = 0
BEGIN
	INSERT INTO ViolationCode
		(
		[Code],
		[Description],
		[SectionID],
		[CategoryID]
		)
		VALUES
		(
		@Code,
		@Desc,
		@SectionID,
		@CategoryID
		)
END
ELSE
BEGIN
	RAISERROR ('This violation code already exists in the database. Please check and try again!',16,1)
    RETURN
END
	
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
BEGIN
    RAISERROR ('Error Occured',16,1)
    RETURN
END