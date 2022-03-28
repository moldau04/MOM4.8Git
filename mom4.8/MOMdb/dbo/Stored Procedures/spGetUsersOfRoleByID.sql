CREATE PROCEDURE [dbo].[spGetUsersOfRoleByID]
	@RoleID int
AS
BEGIN
	--SELECT r.Id AS RoleID, r.RoleName, u.* FROM tblRole r 
	--INNER JOIN tblUserRole ur ON ur.RoleId = r.Id
	--INNER JOIN tblUser u ON u.ID = ur.UserId
	--WHERE r.Id = @RoleID
	SELECT  
		u.ID as UserId
		, u.fUser
	FROM tblUserRole uro INNER JOIN tblUser u on u.ID = uro.UserId WHERE uro.RoleId = @RoleID
END
