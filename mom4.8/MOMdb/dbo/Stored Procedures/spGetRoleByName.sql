CREATE PROCEDURE [dbo].[spGetRoleByName]
	@RoleName Varchar(255)
AS
BEGIN
	SELECT * FROM tblRole WHERE RoleName = @RoleName
END
