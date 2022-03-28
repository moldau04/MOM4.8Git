CREATE PROCEDURE [dbo].[spGetUsersEmailByTypeAndUserID]
	@type int,
	@userID int,
	@email VARCHAR	(100) OUTPUT	
AS

DECLARE @strEmail VARCHAR (200)
SET @strEmail=''
IF (@type=2)
	BEGIN
		SET @strEmail=ISNULL((SELECT TOP 1 ISNULL(r.email,'') as email FROM OWNER o 
				LEFT OUTER JOIN Rol r on o.Rol=r.ID 
				WHERE o.ID=@userID),'')
	END	
ELSE
BEGIN
	IF (@type=5)
	BEGIN
		SET @strEmail=ISNULL((SELECT TOP 1 t.Email FROM Team t	where t.ID = @userID),'')
    END 
	ELSE
    BEGIN
		SET @strEmail=ISNULL ((SELECT TOP 1	isnull(r.email,'') as email				
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN Rol r on e.Rol=r.ID 
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
	        LEFT OUTER JOIN tblRole rur on rur.Id = ur.RoleId 
			WHERE u.ID=@userID),'')
    END 
END
SET @email=@strEmail



