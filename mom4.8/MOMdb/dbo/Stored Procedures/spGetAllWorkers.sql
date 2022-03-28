CREATE PROCEDURE spGetAllWorkers
AS
BEGIN
	SELECT DISTINCT w.fDesc AS MechanicName, j.Type AS Department	 
	FROM TicketO t 
		LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID
		LEFT OUTER JOIN tblWork w ON t.fWork = w.ID
		LEFT OUTER JOIN Emp e ON w.ID = e.fWork
		LEFT OUTER JOIN tblJoinEmpDepartment dep ON dep.Emp = e.ID
		LEFT OUTER JOIN JobType j ON dep.Department = j.ID
	WHERE t.Assigned <> 4 AND t.fWork IS NOT NULL AND w.fDesc IS NOT NULL
	ORDER BY j.Type, w.fDesc
END