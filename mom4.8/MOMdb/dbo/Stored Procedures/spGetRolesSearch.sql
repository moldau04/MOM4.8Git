CREATE PROCEDURE [dbo].[spGetRolesSearch]
	@SearchBy varchar(50),
	@SearchValue varchar(Max),
	@IsIncInactive bit
AS

DECLARE @strQuery varchar(MAX)

SET @strQuery = 'SELECT * FROM tblRole WHERE 1=1 '

IF(IsNull(@IsIncInactive,0) = 0)
BEGIN
	SET @strQuery += ' AND ISNULL(status,0) = 0 '
END

IF (@SearchBy = 'rolename')
BEGIN
	SET @strQuery += ' AND rolename like ''%' + @SearchValue + '%'''
END

IF (@SearchBy = 'status')
BEGIN
	SET @strQuery += ' AND ISNULL(' + @SearchBy + ',0) = ''' + ISNULL(@SearchValue,0) + ''''
END

SET @strQuery += ' Order By RoleName'
--Print @strQuery
Exec(@strQuery)
