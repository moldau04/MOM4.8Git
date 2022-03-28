CREATE PROCEDURE [dbo].[spGetUsersForRole]
	@status int,
	@isCusIncluced bit = 1,
	@RoleID int
AS
BEGIN
	IF @status = 1 OR @status = 0
	BEGIN
		IF @isCusIncluced = 1
		BEGIN
			SELECT	
				u.ID as UserID
				, LTRIM(RTRIM(e.fFirst)) as fFirst
				, LTRIM(RTRIM(e.Last)) as lLast
				, CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID)
					ELSE '1_' +  Convert(varchar(10),u.ID) END AS memberkey
				, fUser
				, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, ISNULL(r.RoleName, '') RoleName
				, ISNULL(u.ApplyUserRolePermission, 0) ApplyUserRolePermission
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
			LEFT OUTER JOIN tblRole r on r.Id = ur.RoleId
			WHERE u.Status = @status 
				-- Remove Admin user on selecting users for role
				AND u.fUser != 'Admin'
				AND (ur.RoleID = @RoleID OR ur.RoleID is null)
				--AND u.ID not in (SELECT UserID from tblUserRole WHERE RoleId != @RoleID)
			UNION 
			SELECT	o.ID as UserID
				, r.Name
				, r.Name
				, '2_' + Convert(varchar(10),o.ID) as memberkey
				, fLogin
				, o.Status
				, 'Customer' as usertype
				, 2 as usertypeid
				, '' RoleName
				, 0 ApplyUserRolePermission
			FROM OWNER o 
			LEFT OUTER JOIN Rol r on o.Rol=r.ID 
			WHERE internet=1 AND o.Status = @status
			Order By usertype, fUser
		END
		ELSE
		BEGIN
			SELECT	
				u.ID as UserID
				, LTRIM(RTRIM(e.fFirst)) as fFirst
				, LTRIM(RTRIM(e.Last)) as lLast
				, CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID)
					ELSE '1_' +  Convert(varchar(10),u.ID) END AS memberkey
				, fUser
				, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, ISNULL(r.RoleName, '') RoleName
				, ISNULL(u.ApplyUserRolePermission, 0) ApplyUserRolePermission
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
			LEFT OUTER JOIN tblRole r on r.Id = ur.RoleId
			WHERE u.Status = @status
				-- Remove Admin user on selecting users for role
				AND u.fUser != 'Admin'
				AND (ur.RoleID = @RoleID OR ur.RoleID is null)
				--AND u.ID not in (SELECT UserID from tblUserRole WHERE RoleId != @RoleID)
			Order By usertype, fUser
			
		END
	END
	ELSE
	BEGIN
		IF @isCusIncluced = 1
		BEGIN
			SELECT	
				u.ID as UserID
				, LTRIM(RTRIM(e.fFirst)) as fFirst
				, LTRIM(RTRIM(e.Last)) as lLast
				, CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID)
					ELSE '1_' +  Convert(varchar(10),u.ID) END AS memberkey
				, fUser
				, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, ISNULL(r.RoleName, '') RoleName
				, ISNULL(u.ApplyUserRolePermission, 0) ApplyUserRolePermission
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
			LEFT OUTER JOIN tblRole r on r.Id = ur.RoleId
			WHERE -- Remove Admin user on selecting users for role
				u.fUser != 'Admin'
				AND (ur.RoleID = @RoleID OR ur.RoleID is null)
				--AND u.ID not in (SELECT UserID from tblUserRole WHERE RoleId != @RoleID)
			UNION 
			SELECT	
				o.ID as UserID
				, r.Name
				, r.Name
				, '2_' + Convert(varchar(10),o.ID) as memberkey
				, fLogin
				, o.Status
				, 'Customer' as usertype
				, 2 as usertypeid
				, '' RoleName
				, 0 ApplyUserRolePermission
			FROM OWNER o 
			LEFT OUTER JOIN Rol r on o.Rol=r.ID
			WHERE internet=1 
			Order By usertype, fUser
		END
		ELSE
		BEGIN
			SELECT	
				u.ID as UserID
				, LTRIM(RTRIM(e.fFirst)) as fFirst
				, LTRIM(RTRIM(e.Last)) as lLast
				, CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID)
					ELSE '1_' +  Convert(varchar(10),u.ID) END AS memberkey
				, fUser
				, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, ISNULL(r.RoleName, '') RoleName
				, ISNULL(u.ApplyUserRolePermission, 0) ApplyUserRolePermission
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
			LEFT OUTER JOIN tblRole r on r.Id = ur.RoleId
			WHERE -- Remove Admin user on selecting users for role
				u.fUser != 'Admin'
				AND (ur.RoleID = @RoleID OR ur.RoleID is null)
				--AND u.ID not in (SELECT UserID from tblUserRole WHERE RoleId != @RoleID)
			Order By usertype, fUser
		END
	END

END