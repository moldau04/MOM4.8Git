CREATE PROCEDURE [dbo].[spGetEquipShutdownHistoryForReport]
	@StartDate Date,
	@EndDate Date,
	@epId varchar(Max),
	@filtered bit = 1
AS

IF @StartDate is null
BEGIN
	SET @StartDate = '1975-01-01';
END
IF @EndDate is null
BEGIN
	SET @EndDate = Convert(Date,GETDATE(), 101);
END

IF @epId is null OR @epId = ''
BEGIN
	IF @filtered = 0
	BEGIN
		SELECT DISTINCT eq.ID
			,l.Tag as [Location]
			, eq.Unit Equipment
		FROM Elev eq
		INNER JOIN ElevShutDownLog ed on ed.elev_id = eq.ID
		INNER JOIN Loc l on l.Loc = eq.Loc
		WHERE @StartDate <= Convert(DATE, ed.created_on, 101) AND @EndDate >= Convert(DATE, ed.created_on, 101) AND eq.Status = 0

		SELECT *, ROW_NUMBER() OVER(ORDER BY temp.ID, temp.[Date] desc) AS Row# FROM 
		(SELECT eq.ID, l.Tag as [Location]
			, eq.Unit Equipment
			, CASE ISNULL(ed.ticket_id, 0) WHEN 0 THEN '' ELSE Convert(varchar(50),ed.ticket_id) END Ticket
			, ed.created_on [Date]
			, m.fDesc AS Mechanic
			, CASE ed.[status] WHEN 1 THEN CASE ISNull(ed.planned,0) WHEN 0 THEN 'No' ELSE 'Yes' END
				ELSE '' END AS Planned
			, ed.reason
			, ed.longdesc
			, CASE ed.[status] WHEN 1 THEN 'Down'
				ELSE 'Return' END [Status]
			, m.Super AS Supervisor
			, CASE WHEN DPDA.DescRes IS NOT NULL THEN DPDA.DescRes WHEN D.DescRes IS NOT NULL THEN D.DescRes ELSE '-' END AS WorkCompleted
			,CASE ISNULL(ticket_id, 0) 
				WHEN 0 THEN UPPER(ISNULL(w.fDesc,u.fUser))
				ELSE UPPER(wk.fDesc) END AS Worker
		FROM Elev eq 
			INNER JOIN ElevShutDownLog ed on ed.elev_id = eq.ID
			INNER JOIN Loc l on l.Loc = eq.Loc
			INNER JOIN Route rou ON rou.ID = l.Route
			LEFT JOIN tblWork m ON m.ID = rou.Mech
			LEFT JOIN TicketDPDA AS DPDA ON DPDA.ID = ed.ticket_id
			LEFT JOIN TicketD AS D ON D.ID = ed.ticket_id
			LEFT JOIN tblUser u ON u.ID = ed.created_by
			LEFT JOIN tblWork w ON w.fDesc = u.fUser
			LEFT OUTER JOIN tblWork wk ON D.fWork = wk.ID OR DPDA.fWork = wk.ID
		WHERE @StartDate <= Convert(DATE, ed.created_on, 101) AND @EndDate >= Convert(DATE, ed.created_on, 101) AND eq.Status = 0
		) AS temp
	END
	IF @filtered = 1
	BEGIN
		SELECT DISTINCT eq.ID
			,l.Tag as [Location]
			, eq.Unit Equipment
		FROM Elev eq
		INNER JOIN ElevShutDownLog ed on ed.elev_id = eq.ID
		INNER JOIN Loc l on l.Loc = eq.Loc
		WHERE @StartDate <= Convert(DATE, ed.created_on, 101) 
			AND @EndDate >= Convert(DATE, ed.created_on, 101)
			AND eq.Status = 0

		SELECT eq.ID, l.Tag as [Location]
			, eq.Unit Equipment
			, CASE ISNULL(ed.ticket_id, 0) WHEN 0 THEN '' ELSE Convert(varchar(50),ed.ticket_id) END Ticket
			, ed.created_on [Date]
			, m.fDesc AS Mechanic
			, CASE ed.[status] WHEN 1 THEN CASE ISNull(ed.planned,0) WHEN 0 THEN 'No' ELSE 'Yes' END
				ELSE '' END AS Planned
			, ed.reason
			, ed.longdesc
			, CASE ed.[status] WHEN 1 THEN 'Down'
				ELSE 'Return' END [Status]
			, m.Super AS Supervisor
			, CASE WHEN DPDA.DescRes IS NOT NULL THEN DPDA.DescRes WHEN D.DescRes IS NOT NULL THEN D.DescRes ELSE '-' END AS WorkCompleted
			,CASE ISNULL(ticket_id, 0) 
				WHEN 0 THEN UPPER(ISNULL(w.fDesc,u.fUser))
				ELSE UPPER(wk.fDesc) END AS Worker
		FROM Elev eq 
			INNER JOIN ElevShutDownLog ed on ed.elev_id = eq.ID
			INNER JOIN Loc l on l.Loc = eq.Loc
			INNER JOIN Route rou ON rou.ID = l.Route
			LEFT JOIN tblWork m ON m.ID = rou.Mech
			LEFT JOIN TicketDPDA AS DPDA ON DPDA.ID = ed.ticket_id
			LEFT JOIN TicketD AS D ON D.ID = ed.ticket_id
			LEFT JOIN tblUser u ON u.ID = ed.created_by
			LEFT JOIN tblWork w ON w.fDesc = u.fUser
			LEFT OUTER JOIN tblWork wk ON D.fWork = wk.ID OR DPDA.fWork = wk.ID
		WHERE @StartDate <= Convert(DATE, ed.created_on, 101)
			AND @EndDate >= Convert(DATE, ed.created_on, 101)
			AND eq.Status = 0
		ORDER BY eq.ID, [Date] desc
	END
END
ELSE
BEGIN
	SELECT DISTINCT eq.ID
		,l.Tag as [Location]
		, eq.Unit Equipment
	FROM Elev eq
	INNER JOIN ElevShutDownLog ed on ed.elev_id = eq.ID
	INNER JOIN Loc l on l.Loc = eq.Loc
	WHERE @StartDate <= Convert(DATE, ed.created_on, 101) 
		AND @EndDate >= Convert(DATE, ed.created_on, 101)
		AND eq.ID in (select * from dbo.Split(@epId, ','))

	SELECT *, ROW_NUMBER() OVER(ORDER BY temp.ID, temp.[Date] desc) AS Row# FROM 
		(SELECT eq.ID, l.Tag as [Location]
			, eq.Unit Equipment
			, CASE ISNULL(ed.ticket_id, 0) WHEN 0 THEN '' ELSE Convert(varchar(50),ed.ticket_id) END Ticket
			, ed.created_on [Date]
			, m.fDesc AS Mechanic
			, CASE ed.[status] WHEN 1 THEN CASE ISNull(ed.planned,0) WHEN 0 THEN 'No' ELSE 'Yes' END
				ELSE '' END AS Planned
			, ed.reason
			, ed.longdesc
			, CASE ed.[status] WHEN 1 THEN 'Down'
				ELSE 'Return' END [Status]
			,m.Super AS Supervisor
			, CASE WHEN DPDA.DescRes IS NOT NULL THEN DPDA.DescRes WHEN D.DescRes IS NOT NULL THEN D.DescRes ELSE '-' END AS WorkCompleted
			,CASE ISNULL(ticket_id, 0) 
				WHEN 0 THEN UPPER(ISNULL(w.fDesc,u.fUser))
				ELSE UPPER(wk.fDesc) END AS Worker
		FROM Elev eq 
			INNER JOIN ElevShutDownLog ed on ed.elev_id = eq.ID
			INNER JOIN Loc l on l.Loc = eq.Loc
			INNER JOIN Route rou ON rou.ID = l.Route
			LEFT JOIN tblWork m ON m.ID = rou.Mech
			LEFT JOIN TicketDPDA AS DPDA ON DPDA.ID = ed.ticket_id
			LEFT JOIN TicketD AS D ON D.ID = ed.ticket_id
			LEFT JOIN tblUser u ON u.ID = ed.created_by
			LEFT JOIN tblWork w ON w.fDesc = u.fUser
			LEFT OUTER JOIN tblWork wk ON D.fWork = wk.ID OR DPDA.fWork = wk.ID
		WHERE @StartDate <= Convert(DATE, ed.created_on, 101)
			AND @EndDate >= Convert(DATE, ed.created_on, 101)
			AND eq.ID in (select * from dbo.Split(@epId, ','))
			AND eq.Status = 0
		) as temp
END