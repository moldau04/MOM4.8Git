CREATE PROCEDURE [dbo].[spGetProjectPOs]
	@projectID int
AS
SELECT po.PO, rv.Name Vendor, po.fdesc, sum(poi.Amount) Amount, po.fDate
, po.SalesOrderNo
, (CASE ISNULL(po.Status,0)
            WHEN 0 THEN 'Open'
            WHEN 1 THEN 'Closed'
            WHEN 2 THEN 'Void'
            WHEN 3 THEN 'Partial-Quantity'
            WHEN 4 THEN 'Partial-Amount'
            WHEN 5 THEN 'Closed At Received PO'
            ELSE ''
        END) AS StatusName
FROM POItem poi WITH (NOLOCK)
INNER JOIN PO WITH (NOLOCK) on po.PO = poi.PO
LEFT JOIN Vendor v WITH (NOLOCK) on v.ID = po.Vendor
LEFT JOIN Rol rv WITH (NOLOCK) on rv.ID = v.Rol
WHERE poi.Job = @projectId
GROUP BY po.PO, rv.Name, po.fdesc, po.fDate, po.SalesOrderNo, po.Status
ORDER BY po.PO DESC
