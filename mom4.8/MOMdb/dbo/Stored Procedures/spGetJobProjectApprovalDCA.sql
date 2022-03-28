CREATE PROCEDURE [dbo].[spGetJobProjectApprovalDCA]          
AS
BEGIN  

SELECT DISTINCT 
	j.ID, 
	o.ID AS CustomerID, 
	r.Name AS CustomerName, 
	r.Phone, 
	r.Cellular,   
	j.Loc,
	l.Address, 
	l.City, 
	l.State, 
	l.Zip,
	CASE 
		WHEN tf.Label LIKE '%Approval from DCA / Local Twp%' THEN 
			(CASE 
				WHEN cj.TeamMember IS NULL OR cj.TeamMember = '' THEN ''
				ELSE 
					(SELECT STUFF((SELECT '; ' + CAST(t.EMail AS VARCHAR(1000))
					FROM (
						SELECT r.EMail
						FROM tblUser u 
							LEFT OUTER JOIN Emp e ON u.fUser=e.CallSign
							LEFT OUTER JOIN Rol r ON e.Rol=r.ID 
						WHERE (CASE 
							WHEN ISNULL(fWork,'')='' THEN '0_' + CONVERT(VARCHAR(10),u.ID)
							ELSE '1_' +  CONVERT(VARCHAR(10),u.ID) END) IN (SELECT SplitValue FROM [dbo].[fnSplit](cj.TeamMember,';'))
						UNION 
						SELECT ISNULL(r.email,'') as email
						FROM OWNER o 
							LEFT OUTER JOIN Rol r ON o.Rol=r.ID 
						WHERE Internet=1 AND o.Status = 1 AND ('2_' + CONVERT(VARCHAR(10),o.ID)) in (SELECT SplitValue FROM [dbo].[fnSplit](cj.TeamMember,';'))
					) AS t
					FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' '))
			END)
		ELSE ''
	END AS ApprovalDCAEmail,
	CASE 
		WHEN tf.Label LIKE '%Ordered Inspection%' THEN
			(CASE 
				WHEN cj.TeamMember IS NULL OR cj.TeamMember = '' THEN ''
				ELSE
					(SELECT STUFF((SELECT '; ' + CAST(t.EMail AS VARCHAR(1000))
					FROM (
						SELECT r.EMail
						FROM tblUser u 
							LEFT OUTER JOIN Emp e ON u.fUser=e.CallSign
							LEFT OUTER JOIN Rol r ON e.Rol=r.ID 
						WHERE (CASE 
							WHEN ISNULL(fWork,'')='' THEN '0_' + CONVERT(VARCHAR(10),u.ID)
							ELSE '1_' +  CONVERT(VARCHAR(10),u.ID) END) IN (SELECT SplitValue FROM [dbo].[fnSplit](cj.TeamMember,';'))
						UNION 
						SELECT ISNULL(r.email,'') as email
						FROM OWNER o 
							LEFT OUTER JOIN Rol r ON o.Rol=r.ID 
						WHERE Internet=1 AND o.Status = 1 AND ('2_' + CONVERT(VARCHAR(10),o.ID)) in (SELECT SplitValue FROM [dbo].[fnSplit](cj.TeamMember,';'))
					) AS t
					FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' '))
			END)
		ELSE ''
	END AS OrderedInspectionEmail,
	CASE 
		WHEN tf.Label LIKE '%Passed Inspection%' THEN 
			(CASE 
				WHEN cj.TeamMember IS NULL OR cj.TeamMember = '' THEN ''
				ELSE
					(SELECT STUFF((SELECT '; ' + CAST(t.EMail AS VARCHAR(1000))
					FROM (
						SELECT r.EMail
						FROM tblUser u 
							LEFT OUTER JOIN Emp e ON u.fUser=e.CallSign
							LEFT OUTER JOIN Rol r ON e.Rol=r.ID 
						WHERE (CASE 
							WHEN ISNULL(fWork,'')='' THEN '0_' + CONVERT(VARCHAR(10),u.ID)
							ELSE '1_' +  CONVERT(VARCHAR(10),u.ID) END) IN (SELECT SplitValue FROM [dbo].[fnSplit](cj.TeamMember,';'))
						UNION 
						SELECT ISNULL(r.email,'') as email
						FROM OWNER o 
							LEFT OUTER JOIN Rol r ON o.Rol=r.ID 
						WHERE Internet=1 AND o.Status = 1 AND ('2_' + CONVERT(VARCHAR(10),o.ID)) in (SELECT SplitValue FROM [dbo].[fnSplit](cj.TeamMember,';'))
					) AS t
					FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' '))
			END)
		ELSE ''
	END AS PassedInspectionEmail
FROM JobT t 
	INNER JOIN tblCustomJobt jt ON t.ID = jt.JobTID
	INNER JOIN tblCustomFields tf ON jt.tblCustomFieldsID = tf.ID
	INNER JOIN tblCustomJob cj ON tf.ID = cj.tblCustomFieldsID
	INNER JOIN Job j ON j.ID = cj.JobID
	INNER JOIN Loc l ON j.loc = l.Loc
	INNER JOIN Owner o ON j.Owner = o.ID
	INNER JOIN Rol r ON o.Rol = r.ID 
WHERE tf.tblTabID = 1 AND j.Status = 0
	AND cj.Value IS NOT NULL AND cj.Value <> ''
	AND (tf.Label LIKE '%Approval from DCA / Local Twp%' OR tf.Label like'%Ordered Inspection%' OR tf.Label like'%Passed Inspection%')

END