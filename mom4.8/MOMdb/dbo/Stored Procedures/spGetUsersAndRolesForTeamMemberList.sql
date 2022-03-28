CREATE PROCEDURE [dbo].[spGetUsersAndRolesForTeamMemberList]
	@status int,
	@isCusIncluced bit = 1,
	@job int = 0
AS
IF @job = 0
BEGIN
	IF @status = 1 OR @status = 0
	BEGIN
		IF @isCusIncluced = 1
		BEGIN
			SELECT	
				--LTRIM(RTRIM(e.fFirst)) as fFirst
				--, LTRIM(RTRIM(e.Last)) as fLast
				--,
				CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID) --+ '_0'
					ELSE '1_' +  Convert(varchar(10),u.ID)-- + '_0' 
					END AS memberkey
				, fUser
				--, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				--, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, isnull(r.email,'') as email
				, isnull(rur.RoleName,'') as RoleName
				, isnull(rur.Id,0) as RoleId
				, 0 IsTask
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN Rol r on e.Rol=r.ID 
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
	        LEFT OUTER JOIN tblRole rur on rur.Id = ur.RoleId 
			WHERE u.Status = @status
			UNION 
			SELECT	--r.Name
				--, r.Name
				--,
				'2_' + Convert(varchar(10),o.ID) --+ '_0'
					as memberkey
				, fLogin
				--, o.Status
				, 'Customer' as usertype
				--, 2 as usertypeid
				, isnull(r.email,'') as email
				, '' RoleName
				, 0 RoleId
				, 0 IsTask
			FROM OWNER o 
			LEFT OUTER JOIN Rol r on o.Rol=r.ID 
			WHERE internet=1 AND o.Status = @status
			UNION 
			SELECT	--r.Name
				--, r.Name
				--,
				'6_' + Convert(varchar(10),ID) --+ '_0' 
					as memberkey
				, ''
				--, o.Status
				, 'UserRole' as usertype
				--, 2 as usertypeid
				, '' as email
				, RoleName
				, Id RoleId
				, 0 IsTask
			FROM tblRole ur
			WHERE ISNULL(Status,0) = 0
		END
		ELSE
		BEGIN
			SELECT	
				--LTRIM(RTRIM(e.fFirst)) as fFirst
				--, LTRIM(RTRIM(e.Last)) as fLast
				--, 
				CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID)-- + '_0'
					ELSE '1_' +  Convert(varchar(10),u.ID)-- + '_0'
					END AS memberkey
				, fUser
				--, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				--, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, isnull(r.email,'') as email
				, isnull(rur.RoleName,'') as RoleName
				, isnull(rur.Id,0) as RoleId
				, 0 IsTask
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN Rol r on e.Rol=r.ID 
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
	        LEFT OUTER JOIN tblRole rur on rur.Id = ur.RoleId 
			WHERE u.Status = @status
			UNION 
			SELECT	--r.Name
				--, r.Name
				--,
				'6_' + Convert(varchar(10),ID) --+ '_0'
					as memberkey
				, ''
				--, o.Status
				, 'UserRole' as usertype
				--, 2 as usertypeid
				, '' as email
				, RoleName
				, Id RoleId
				, 0 IsTask
			FROM tblRole ur
			WHERE ISNULL(Status,0) = 0
			
		END
	END
	ELSE
	BEGIN
		IF @isCusIncluced = 1
		BEGIN
			SELECT	
				--LTRIM(RTRIM(e.fFirst)) as fFirst
				--, LTRIM(RTRIM(e.Last)) as fLast
				--, 
				CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID)-- + '_0'
					ELSE '1_' +  Convert(varchar(10),u.ID)-- + '_0' 
					END AS memberkey
				, fUser
				--, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				--, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, isnull(r.email,'') as email
				, isnull(rur.RoleName,'') as RoleName
				, isnull(rur.Id,0) as RoleId
				, 0 IsTask
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN Rol r on e.Rol=r.ID 
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
	        LEFT OUTER JOIN tblRole rur on rur.Id = ur.RoleId 
			UNION 
			SELECT	--r.Name
				--, r.Name
				--, 
				'2_' + Convert(varchar(10),o.ID)-- + '_0'
					as memberkey
				, fLogin
				--, o.Status
				, 'Customer' as usertype
				--, 2 as usertypeid
				, isnull(r.email,'') as email
				, '', 0
				, 0 IsTask
			FROM OWNER o 
			LEFT OUTER JOIN Rol r on o.Rol=r.ID
			WHERE internet=1 
			UNION 
			SELECT	--r.Name
				--, r.Name
				--,
				'6_' + Convert(varchar(10),ID)-- + '_0'
					as memberkey
				, ''
				--, o.Status
				, 'UserRole' as usertype
				--, 2 as usertypeid
				, '' as email
				, RoleName
				, Id RoleId
				, 0 IsTask
			FROM tblRole ur
			WHERE ISNULL(Status,0) = 0
		END
		ELSE
		BEGIN
			SELECT	
				--LTRIM(RTRIM(e.fFirst)) as fFirst
				--, LTRIM(RTRIM(e.Last)) as fLast
				--, 
				CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID)-- + '_0'
					ELSE '1_' +  Convert(varchar(10),u.ID)-- + '_0' 
					END AS memberkey
				, fUser
				--, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				--, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, isnull(r.email,'') as email
				, isnull(rur.RoleName,'') as RoleName
				, isnull(rur.Id,0) as RoleId
				, 0 IsTask
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN Rol r on e.Rol=r.ID 
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
	        LEFT OUTER JOIN tblRole rur on rur.Id = ur.RoleId 
			UNION 
			SELECT	--r.Name
				--, r.Name
				--,
				'6_' + Convert(varchar(10),ID)-- + '_0'
					as memberkey
				, ''
				--, o.Status
				, 'UserRole' as usertype
				--, 2 as usertypeid
				, '' as email
				, RoleName
				, Id RoleId
				, 0 IsTask
			FROM tblRole ur
			WHERE ISNULL(Status,0) = 0
		END
	END
END
ELSE
BEGIN
	IF @status = 1 OR @status = 0
	BEGIN
		IF @isCusIncluced = 1
		BEGIN
			SELECT	
				--LTRIM(RTRIM(e.fFirst)) as fFirst
				--, LTRIM(RTRIM(e.Last)) as fLast
				--,
				CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID) --+ '_0'
					ELSE '1_' +  Convert(varchar(10),u.ID) --+ '_0'
					END AS memberkey
				, fUser
				--, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				--, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, isnull(r.email,'') as email
				, isnull(rur.RoleName,'') as RoleName
				, isnull(rur.Id,0) as RoleId
				, 0 IsTask
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN Rol r on e.Rol=r.ID 
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
	        LEFT OUTER JOIN tblRole rur on rur.Id = ur.RoleId 
			WHERE u.Status = @status
			UNION 
			SELECT	--r.Name
				--, r.Name
				--,
				'2_' + Convert(varchar(10),o.ID) --+ '_0' 
					as memberkey
				, fLogin
				--, o.Status
				, 'Customer' as usertype
				--, 2 as usertypeid
				, isnull(r.email,'') as email
				, '', 0
				, 0 IsTask
			FROM OWNER o 
			LEFT OUTER JOIN Rol r on o.Rol=r.ID 
			WHERE internet=1 AND o.Status = @status
			UNION
			SELECT 
				'5_' +  Convert(varchar(10),t.ID) --+ '_0'
					AS memberkey
				, t.MomUserID
				, 'Project Team' as usertype
				, t.Email
				, t.Title, 0
				, 0 IsTask
			FROM Team t
			where t.JobID = @job
			UNION 
			SELECT	--r.Name
				--, r.Name
				--,
				'6_' + Convert(varchar(10),ID)-- + '_0' 
					as memberkey
				, ''
				--, o.Status
				, 'UserRole' as usertype
				--, 2 as usertypeid
				, '' as email
				, RoleName
				, Id RoleId
				, 0 IsTask
			FROM tblRole ur
			WHERE ISNULL(Status,0) = 0
			UNION 
			SELECT	
				'7_' + t1.Title + '|'
					as memberkey
				, ''
				, 'Project Team Title' as usertype
				, '' as email
				, t1.Title
				, 0 RoleId
				, 0 IsTask
			FROM (SELECT Distinct t.Title FROM Team t Left join tblRole ur ON ur.RoleName = t.Title
					WHERE ur.Id is null AND t.JobID = @job) t1
		END
		ELSE
		BEGIN
			SELECT	
				--LTRIM(RTRIM(e.fFirst)) as fFirst
				--, LTRIM(RTRIM(e.Last)) as fLast
				--, 
				CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID)-- + '_0' 
					ELSE '1_' +  Convert(varchar(10),u.ID) -- + '_0' 
					END AS memberkey
				, fUser
				--, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				--, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, isnull(r.email,'') as email
				, isnull(rur.RoleName,'') as RoleName
				, isnull(rur.Id,0) as RoleId
				, 0 IsTask
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN Rol r on e.Rol=r.ID 
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
	        LEFT OUTER JOIN tblRole rur on rur.Id = ur.RoleId 
			WHERE u.Status = @status
			UNION
			SELECT 
				'5_' +  Convert(varchar(10),t.ID)-- + '_0'
					AS memberkey
				, t.MomUserID
				, 'Project Team' as usertype
				, t.Email
				, t.Title, 0
				, 0 IsTask
			FROM Team t
			where t.JobID = @job
			UNION 
			SELECT	--r.Name
				--, r.Name
				--,
				'6_' + Convert(varchar(10),ID)-- + '_0'
					as memberkey
				, ''
				--, o.Status
				, 'UserRole' as usertype
				--, 2 as usertypeid
				, '' as email
				, RoleName
				, Id RoleId
				, 0 IsTask
			FROM tblRole ur
			WHERE ISNULL(Status,0) = 0
			UNION 
			SELECT	
				'7_' + t1.Title + '|'
					as memberkey
				, ''
				, 'Project Team Title' as usertype
				, '' as email
				, t1.Title
				, 0 RoleId
				, 0 IsTask
			FROM (SELECT Distinct t.Title FROM Team t Left join tblRole ur ON ur.RoleName = t.Title
					WHERE ur.Id is null AND t.JobID = @job) t1
		END
	END
	ELSE
	BEGIN
		IF @isCusIncluced = 1
		BEGIN
			SELECT	
				--LTRIM(RTRIM(e.fFirst)) as fFirst
				--, LTRIM(RTRIM(e.Last)) as fLast
				--, 
				CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID) --+ '_0'
					ELSE '1_' +  Convert(varchar(10),u.ID) --+ '_0'
					END AS memberkey
				, fUser
				--, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				--, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, isnull(r.email,'') as email
				, isnull(rur.RoleName,'') as RoleName
				, isnull(rur.Id,0) as RoleId
				, 0 IsTask
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN Rol r on e.Rol=r.ID 
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
	        LEFT OUTER JOIN tblRole rur on rur.Id = ur.RoleId 
			UNION 
			SELECT	--r.Name
				--, r.Name
				--, 
				'2_' + Convert(varchar(10),o.ID)-- + '_0'
					as memberkey
				, fLogin
				--, o.Status
				, 'Customer' as usertype
				--, 2 as usertypeid
				, isnull(r.email,'') as email
				, '', 0
				, 0 IsTask
			FROM OWNER o 
			LEFT OUTER JOIN Rol r on o.Rol=r.ID
			WHERE internet=1 
			UNION
			SELECT 
				'5_' +  Convert(varchar(10),t.ID)-- + '_0'
					AS memberkey
				, t.MomUserID
				, 'Project Team' as usertype
				, t.Email
				, t.Title, 0
				, 0 IsTask
			FROM Team t
			where t.JobID = @job
			UNION 
			SELECT	--r.Name
				--, r.Name
				--,
				'6_' + Convert(varchar(10),ID) --+ '_0'
					as memberkey
				, ''
				--, o.Status
				, 'UserRole' as usertype
				--, 2 as usertypeid
				, '' as email
				, RoleName
				, Id RoleId
				, 0 IsTask
			FROM tblRole ur
			WHERE ISNULL(Status,0) = 0
			UNION 
			SELECT	
				'7_' + t1.Title + '|'
					as memberkey
				, ''
				, 'Project Team Title' as usertype
				, '' as email
				, t1.Title
				, 0 RoleId
				, 0 IsTask
			FROM (SELECT Distinct t.Title FROM Team t Left join tblRole ur ON ur.RoleName = t.Title
					WHERE ur.Id is null AND t.JobID = @job) t1
		END
		ELSE
		BEGIN
			SELECT	
				--LTRIM(RTRIM(e.fFirst)) as fFirst
				--, LTRIM(RTRIM(e.Last)) as fLast
				--, 
				CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID)-- + '_0'
					ELSE '1_' +  Convert(varchar(10),u.ID)-- + '_0' 
					END AS memberkey
				, fUser
				--, u.Status
				, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
				--, case when isnull(fWork,'')='' then 0 else 1  end as usertypeid
				, isnull(r.email,'') as email
				, isnull(rur.RoleName,'') as RoleName
				, isnull(rur.Id,0) as RoleId
				, 0 IsTask
			FROM tblUser u 
			LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
			LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
			LEFT OUTER JOIN Rol r on e.Rol=r.ID 
			LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
	        LEFT OUTER JOIN tblRole rur on rur.Id = ur.RoleId 
			UNION
			SELECT 
				'5_' +  Convert(varchar(10),t.ID)-- + '_0'
					AS memberkey
				, t.MomUserID
				, 'Project Team' as usertype
				, t.Email
				, t.Title, 0
				, 0 IsTask
			FROM Team t
			where t.JobID = @job
			UNION 
			SELECT	--r.Name
				--, r.Name
				--,
				'6_' + Convert(varchar(10),ID)-- + '_0'
					as memberkey
				, ''
				--, o.Status
				, 'UserRole' as usertype
				--, 2 as usertypeid
				, '' as email
				, RoleName
				, Id RoleId
				, 0 IsTask
			FROM tblRole ur
			WHERE ISNULL(Status,0) = 0
			UNION 
			SELECT	
				'7_' + t1.Title + '|'
					as memberkey
				, ''
				, 'Project Team Title' as usertype
				, '' as email
				, t1.Title
				, 0 RoleId
				, 0 IsTask
			FROM (SELECT Distinct t.Title FROM Team t Left join tblRole ur ON ur.RoleName = t.Title
					WHERE ur.Id is null AND t.JobID = @job) t1
		END
	END
END