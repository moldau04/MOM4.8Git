CREATE VIEW [dbo].[vw_DeliveryReportDetails]
AS

WITH t AS (
	SELECT j.ID AS [Project #],
		r.Name AS Customer,
		r.Phone AS Phone, 
		l.Tag AS Location, 
		l.City, 
		l.State, 
		l.Zip, 
		CASE WHEN j.Status = 0 THEN 'Active' ELSE 'Inactive' END AS Status,
		cj.Value AS [Delivery Pymt Rcvd],
		(SELECT cj1.Value
			FROM dbo.tblCustomJob AS cj1
				LEFT OUTER JOIN dbo.tblCustomFields AS cf1 ON cf1.ID = cj1.tblCustomFieldsID
			WHERE (cj1.JobID = j.ID) AND (cf1.Label = 'Unit delivered')) AS [Unit Delivered]
	FROM dbo.Job AS j 
		INNER JOIN dbo.tblCustomJob AS cj ON cj.JobID = j.ID
		INNER JOIN dbo.tblCustomFields AS cf ON cf.ID = cj.tblCustomFieldsID AND cf.Label = 'PAYMENT 3 - DELIVERY PYMT RCVD'
		INNER JOIN dbo.Loc AS l ON l.Loc = j.Loc
		LEFT JOIN  dbo.Owner AS o ON o.ID = j.Owner
		LEFT JOIN dbo.Rol AS r ON r.ID = o.Rol AND r.Type = 0
)

SELECT * FROM t
WHERE ([Unit Delivered] <> '') 
	AND ([Delivery Pymt Rcvd] IS NULL OR [Delivery Pymt Rcvd] = '')
