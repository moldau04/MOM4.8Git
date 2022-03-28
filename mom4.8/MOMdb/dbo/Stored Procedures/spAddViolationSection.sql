CREATE PROCEDURE [dbo].[spAddViolationSection]
	@Name VARCHAR(200)	
AS
DECLARE @IsExisted int = 0

SELECT @IsExisted = Count(*) FROM ViolationSection WHERE Name = @Name

IF @IsExisted = 0
BEGIN
	INSERT INTO ViolationSection (Name) VALUES (@Name)
END
ELSE
BEGIN
	RAISERROR ('This violation section already exists in the database. Please check and try again!',16,1)
    RETURN
END
	
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
BEGIN
    RAISERROR ('Error Occured',16,1)
    RETURN
END