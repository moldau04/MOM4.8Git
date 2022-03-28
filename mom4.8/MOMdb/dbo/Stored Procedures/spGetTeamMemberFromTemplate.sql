CREATE PROCEDURE [dbo].[spGetTeamMemberFromTemplate]
	 @jobt int, @job int--, @customLabelId int
AS

BEGIN
	DECLARE @teamMemberFromTemplate varchar(max);

	SET @teamMemberFromTemplate = '';

	SELECT
        @teamMemberFromTemplate = @teamMemberFromTemplate + ISNULL(tc.TeamMember + ';', '')
    FROM tblCustomJobT tbjobt
    INNER JOIN tblCustomFields tc
        ON tc.ID = tbjobt.tblCustomFieldsID
    INNER JOIN JobT jobt
        ON jobt.ID = tbjobt.JobTID
    WHERE tbjobt.JobTID = @jobt
    AND (tc.IsDeleted IS NULL
    OR tc.IsDeleted = 0)
    AND tbjobt.JobID IS NULL
    AND tc.tblTabID IS NOT NULL
	AND tc.TeamMember IS NOT NULL
	--AND tc.ID = @customLabelId

	SELECT DISTINCT * FROM 
	(
		SELECT  
			CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID)
				ELSE '1_' +  Convert(varchar(10),u.ID) END AS memberkey
			, fUser
			, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
			, isnull(r.email,'') as email
		FROM Split(@teamMemberFromTemplate, ';') team
		INNER JOIN tblUser u on '0_' + Convert(varchar(50), u.ID) = RTRIM(LTRIM(team.items)) OR '1_' + Convert(varchar(50), u.ID) = RTRIM(LTRIM(team.items))
		LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
		LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
		LEFT OUTER JOIN Rol r on e.Rol=r.ID
		UNION
		SELECT	
			'2_' + Convert(varchar(10),o.ID) as memberkey
			, fLogin
			, 'Customer' as usertype
			, isnull(r.email,'') as email
		FROM Split(@teamMemberFromTemplate, ';') team
		INNER JOIN OWNER o on '2_' + Convert(varchar(50), o.ID) = RTRIM(LTRIM(team.items))
		LEFT OUTER JOIN Rol r on o.Rol=r.ID
		WHERE internet=1 
		UNION
		SELECT	
			RTRIM(LTRIM(team.items)) as memberkey
			, (select th.items From (SELECT
				ROW_NUMBER () OVER (ORDER BY (Select null)) AS RowNum,
				*
				FROM Split(RTRIM(LTRIM(team.items)), '|') tt) th
				where RowNum = 3
				) fLogin
			, (select top 1 'Exchange ' + SUBSTRING(isnull(tt.items, ''),3,Len(isnull(tt.items, '')) - 2) from Split(RTRIM(LTRIM(team.items)), '|') tt) as usertype
			, (select th.items From (SELECT
				ROW_NUMBER () OVER (ORDER BY (Select null)) AS RowNum,
				*
				FROM Split(RTRIM(LTRIM(team.items)), '|') tt) th
				where RowNum = 2
				)
				as email
		FROM Split(@teamMemberFromTemplate, ';') team
		WHERE RTRIM(LTRIM(items)) like '3_%'-- or RTRIM(LTRIM(items)) like '4_%'
		UNION
		SELECT	
			RTRIM(LTRIM(team.items)) as memberkey
			, (select th.items From (SELECT
				ROW_NUMBER () OVER (ORDER BY (Select null)) AS RowNum,
				*
				FROM Split(RTRIM(LTRIM(team.items)), '|') tt) th
				where RowNum = 3
				) fLogin
			, (select top 1 'Exchange Group: ' + SUBSTRING(isnull(tt.items, ''),3,Len(isnull(tt.items, '')) - 2) from Split(RTRIM(LTRIM(team.items)), '|') tt) as usertype
			, (select th.items From (SELECT
				ROW_NUMBER () OVER (ORDER BY (Select null)) AS RowNum,
				*
				FROM Split(RTRIM(LTRIM(team.items)), '|') tt) th
				where RowNum = 2
				)
				as email
		FROM Split(@teamMemberFromTemplate, ';') team
		WHERE RTRIM(LTRIM(items)) like '4_%'
		UNION
		-- Need to union with project team
		--SELECT 
		--	CASE WHEN isnull(fWork,'')='' THEN '0_' + Convert(varchar(10),u.ID)
		--				ELSE '1_' +  Convert(varchar(10),u.ID) END AS memberkey
		--	, t.MomUserID
		--	, case when isnull(fWork,'')='' then 'Office' else 'Field'  end as usertype
		--	, t.Email

		--FROM Team t
		--INNER JOIN tblUser u on t.MomUserID = u.fUser
		--LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
		--LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
		SELECT 
			'5_' +  Convert(varchar(10),t.ID) AS memberkey
			, t.MomUserID
			, 'Project Team' as usertype
			, t.Email

		FROM Team t
		where t.JobID = @job
	) AS NewTeamMember
END
