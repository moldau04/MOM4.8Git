CREATE PROCEDURE [dbo].[spGetUserSearch]
	@SearchText varchar(150),
	@UserRoleName varchar(255)
AS
declare @WOspacialchars varchar(50) 
declare @text nvarchar(max)
set @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)
BEGIN
	
	SET NOCOUNT ON;

	SET @UserRoleName = (SELECT r.RoleName FROM tblRole r WHERE r.RoleName = @UserRoleName AND ISNULL(r.Status, 0) = 0)

	IF(@UserRoleName is null OR @UserRoleName = '')
	BEGIN
		set @text = 'SELECT t.ID AS value, t.fUser as label, e.fFirst, e.Last as fLast, r.Email, r.Cellular, tblRole.RoleName
					 FROM [dbo].[tblUser] t 
						LEFT JOIN [dbo].[Emp] e ON e.CallSign = t.fUser
						LEFT JOIN [dbo].[Rol] r ON r.ID = e.Rol
						LEFT JOIN tblUserRole ur ON ur.UserId = t.ID
						LEFT JOIN tblRole ON tblRole.Id = ur.RoleId
						WHERE t.Status = 0 '

		
	END
	ELSE
	BEGIN
		set @text = 'SELECT t.ID AS value, t.fUser as label, e.fFirst, e.Last as fLast, r.Email, r.Cellular, tblRole.RoleName
					 FROM [dbo].[tblUser] t 
						LEFT JOIN [dbo].[Emp] e ON e.CallSign = t.fUser
						LEFT JOIN [dbo].[Rol] r ON r.ID = e.Rol
						LEFT JOIN tblUserRole ur ON ur.UserId = t.ID
						LEFT JOIN tblRole ON tblRole.Id = ur.RoleId
						WHERE t.Status = 0 
						AND tblRole.RoleName = '''+@UserRoleName+'''
						'
	END

	if(@SearchText<>'')
	begin
		set @text += ' AND ((dbo.RemoveSpecialChars(fUser) LIKE ''%'+@WOspacialchars+'%'') '
		set @text += ' or (dbo.RemoveSpecialChars(e.ffirst) LIKE ''%'+@WOspacialchars+'%'') '
		set @text += ' or (dbo.RemoveSpecialChars(e.last) LIKE ''%'+@WOspacialchars+'%'')) '
	end
	set @text += ' Order by fUser'
	exec(@text)
END

