CREATE VIEW [dbo].[vw_OpenJobReport]            
 AS             

SELECT j.ID AS [Project #],
	r.Name AS Customer,
	r.Phone AS Phone,
	l.tag AS Location,
	l.City,
	l.State,
	l.Zip,
	CASE WHEN j.Status = 0 THEN 'Active' ELSE 'Inactive' END AS Status, 
	(SELECT STUFF((SELECT ', ' + CAST(Type AS VARCHAR(100)) Type
		FROM elev 
		WHERE loc = l.Loc 
		group by type
		FOR XML PATH(''), TYPE)
		.value('.','NVARCHAR(MAX)'),1,2,' ') 
	) AS Type,
	j.Custom1,
	j.Custom2,
	j.Custom3,
	j.Custom4,
	j.Custom5,
	j.Custom6,
	j.Custom7,
	j.Custom8,
	j.Custom9,
	j.Custom10,
	j.Custom11,
	j.Custom12,
	j.Custom13,
	j.Custom14,
	j.Custom15,
	j.Custom16,
	j.Custom17,
	j.Custom18,
	j.Custom19,
	j.Custom20,
	j.TaskCategory AS [Task Category],
	jt.fDesc AS [Template Type],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'New Contract Signed'
	) AS [New Contract Signed],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Approval from DCA / Local Twp.' 
	) AS [Approval from DCA / Local Twp.],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Down Pymt Recd' 
	) AS [Down Pymt Recd],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Door Frame Installed' 
	) AS [Door Frame Installed],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Door Frame Delivered' 
	) AS [Door Frame Delivered],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Final Selections received' 
	) AS [Final Selections received],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Elevator Ordered' 
	) AS [Elevator Ordered],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Elevator Delivered' 
	) AS [Elevator Delivered],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Contract Signed' 
	) AS [Contract Signed],		
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Final Release Rcvd' 
	) AS [Final Release Rcvd],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Submitted to engineering' 
	) AS [Submitted to engineering],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Engineering Date Stamp' 
	) AS [Engineering Date Stamp],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Machine Room Sign Off Received' 
	) AS [Machine Room Sign Off Received],		
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Passed Inspection' 
	) AS [Passed Inspection],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Re-inspection Passed' 
	) AS [Re-inspection Passed],

	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Trim Complete' 
	) AS [Trim Complete],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Unit delivered' 
	) AS [Unit delivered],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Unit Finished' 
	) AS [Unit Finished],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Unit Ordered' 
	) AS [Unit Ordered],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Unit substantially complete' 
	) AS [Unit substantially complete],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Preliminary Order Form Received' 
	) AS [Preliminary Order Form Received],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'PAYMENT 2 - MFG PYMT RCVD' 
	) AS [PAYMENT 2 - MFG PYMT RCVD],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'PAYMENT 3 - DELIVERY PYMT RCVD' 
	) AS [PAYMENT 3 - DELIVERY PYMT RCVD],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'PAYMENT 4 - FINAL PYMT RCVD' 
	) AS [PAYMENT 4 - FINAL PYMT RCVD],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Change Order Issued' 
	) AS [Change Order Issued],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Change Order Accepted (Yes / No)' 
	) AS [Change Order Accepted (Yes / No)],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Change Order Accept / Deny Date' 
	) AS [Change Order Accept / Deny Date],
	(SELECT cj.Value
		FROM tblCustomJob cj 
		LEFT OUTER JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID
		WHERE cj.jobid = j.id AND cf.Label = 'Drawings / Technical Sheet Submitted' 
	) AS [Drawings / Technical Sheet Submitted],
	ISNULL(t.Name,'Unassigned') AS SalesPerson
              
FROM Job j
	INNER JOIN Loc l ON l.Loc = j.Loc
	LEFT JOIN Terr t ON l.Terr = t.ID
	LEFT JOIN Owner o ON o.ID = j.Owner
	LEFT JOIN Rol r ON r.ID = o.Rol
	LEFT JOIN JobT jt ON j.Template = jt.ID
WHERE j.ID IN (SELECT DISTINCT JobID FROM tblCustomJob)
