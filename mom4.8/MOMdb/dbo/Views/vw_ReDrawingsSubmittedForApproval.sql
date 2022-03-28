CREATE VIEW [dbo].[vw_ReDrawingsSubmittedForApproval]
AS 
WITH t AS (
	SELECT 
		j.ID AS [Project #],
		r.Name  AS Customer,
		r.Phone AS Phone,
		l.tag AS Location,
		l.City,
		l.State,
		l.Zip,
		CASE 
			WHEN j.Status = 0 THEN 'Active' 
			ELSE 'Inactive' 
		END AS Status, 
		(SELECT  cj.Value
			FROM tblcustomjob cj 
			LEFT OUTER JOIN tblCustomFields cf on  cf.id= cj.tblCustomFieldsID
			WHERE cj.jobid= j.id AND cf.Label='Re-Do Drawings Submitted for approval' 
		) AS [Re-Do Drawings Submitted for approval],
		(SELECT  cj.Value
			FROM tblcustomjob cj 
			LEFT OUTER JOIN tblCustomFields cf on  cf.id= cj.tblCustomFieldsID
			WHERE cj.jobid= j.id AND cf.Label='Re-Do Drawings Approved' 
		) AS [Re-Do Drawings Approved]
	FROM Job j
		INNER JOIN Loc l on l.Loc = j.Loc
		INNER JOIN Owner o on o.ID= j.Owner
		INNER JOIN Rol r on r.ID= o.Rol
	WHERE j.id in (SELECT DISTINCT jobid FROM tblCustomjob)
)

SELECT 
	* 
FROM t
WHERE ([Re-Do Drawings Submitted for approval] IS NOT NULL AND [Re-Do Drawings Submitted for approval] != '') 
	AND ([Re-Do Drawings Approved] IS NULL OR [Re-Do Drawings Approved] = '')