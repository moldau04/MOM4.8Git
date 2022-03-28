CREATE Procedure [dbo].[spGetEmailFromListRoleID]             
	@lsRole  VARCHAR(max)
AS 
SET @lsRole = REPLACE(@lsRole, '6_','')

 SELECT isnull(r.email,'') AS  email 
FROM tbluser u 
LEFT outer join Emp e  on u.fUser=e.CallSign left outer join Rol r on e.Rol=r.ID 
WHERE  u.id in (select isnull( UserID,0) from tblUserRole where RoleId in (select  convert(int,Item) from dbo.SplitString(@lsRole,';') ))