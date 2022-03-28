CREATE PROCEDURE [dbo].[spGetUsersOfRoleByName]
	@RoleName varchar(255)
AS
BEGIN
	SELECT r.Id AS RoleID, r.RoleName, u.* FROM tblRole r 
	INNER JOIN tblUserRole ur ON ur.RoleId = r.Id
	INNER JOIN tblUser u ON u.ID = ur.UserId
	WHERE r.RoleName = @RoleName
END
