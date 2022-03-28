CREATE PROCEDURE [dbo].[spGetEquipmentShutdownLogs]
	@equipID INT
AS

SELECT l.[id]
	,CASE ISNULL(l.ticket_id, 0) 
		WHEN 0 THEN '' 
		ELSE Convert(varchar(50),l.ticket_id) END ticket_id
    ,CASE l.[status] 
		WHEN 1 THEN 'Yes'
		ELSE 'No' END AS [status]
    ,l.[elev_id]
    ,l.[created_on]
    ,CASE ISNULL(ticket_id, 0) 
		WHEN 0 THEN UPPER(ISNULL(w.fDesc,u.fUser))
		ELSE UPPER(wk.fDesc) END AS worker
    ,l.[reason]
	,l.[longdesc]
FROM [dbo].[ElevShutDownLog] l
	LEFT JOIN tblUser u ON u.ID = l.created_by
	LEFT JOIN tblWork w ON w.fDesc = u.fUser
	LEFT JOIN TicketD t ON t.ID = l.ticket_id
	LEFT JOIN TicketDPDA AS dp ON dp.ID = l.ticket_id
	LEFT OUTER JOIN tblWork wk ON t.fWork = wk.ID OR dp.fWork = wk.ID
WHERE l.elev_id = @equipID 
ORDER BY l.created_on DESC, l.id DESC