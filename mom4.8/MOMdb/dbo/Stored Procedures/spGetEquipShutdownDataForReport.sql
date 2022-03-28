CREATE PROCEDURE [dbo].[spGetEquipShutdownDataForReport]
	@EndDate Date
AS

IF @EndDate is null
BEGIN
	SET @EndDate = Convert(Date,GETDATE(), 101);
END

SELECT *, ROW_NUMBER() OVER(ORDER BY temp.Planned, temp.[Date] ASC) AS Row# FROM (
	SELECT edTemp.id, l.Tag as [Location]
		, eq.Unit Equipment
		, CASE ISNULL(ed.ticket_id, 0) WHEN 0 THEN '' ELSE Convert(varchar(50),ed.ticket_id) END Ticket
		, ed.created_on [Date]
		, m.fDesc AS Mechanic
		, CASE ISNull(ed.planned,0) WHEN 0 Then 'No'
			ELSE 'Yes' END AS Planned
		, ed.reason
		, ed.longdesc
		, CASE ed.[status] WHEN 1 THEN 'Down'
			ELSE '' END [Status]
		, m.Super AS Supervisor
		, CASE WHEN DPDA.DescRes IS NOT NULL THEN DPDA.DescRes WHEN D.DescRes IS NOT NULL THEN D.DescRes ELSE '-' END AS WorkCompleted
		,CASE ISNULL(ticket_id, 0) 
			WHEN 0 THEN UPPER(ISNULL(w.fDesc,u.fUser))
			ELSE UPPER(wk.fDesc) END AS Worker
	FROM Elev eq 
		INNER JOIN 
			(SELECT max(id) as id, elev_id FROM ElevShutDownLog
				WHERE Convert(DATE, created_on, 101) <= @EndDate
				GROUP BY elev_id
			) edTemp on eq.ID = edTemp.elev_id
		INNER JOIN ElevShutDownLog ed on ed.id = edTemp.id 
		INNER JOIN Loc l on l.Loc = eq.Loc
		INNER JOIN Route rou ON rou.ID = l.Route
		LEFT JOIN TicketDPDA AS DPDA ON DPDA.ID = ed.ticket_id
		LEFT JOIN TicketD AS D ON D.ID = ed.ticket_id
		LEFT JOIN tblWork m ON m.ID = rou.Mech
		LEFT JOIN tblUser u ON u.ID = ed.created_by
		LEFT JOIN tblWork w ON w.fDesc = u.fUser
		LEFT OUTER JOIN tblWork wk ON D.fWork = wk.ID OR DPDA.fWork = wk.ID
	WHERE ed.[status] = 1 AND eq.Status = 0
) temp