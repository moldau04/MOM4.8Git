CREATE PROC [dbo].[spGetTasksByUserName]
	@UserName VARCHAR(50)
AS
SELECT td.* FROM todo td
INNER JOIN (SELECT e.Last+', '+e.fFirst as fuser,u.fuser as username FROM tbluser u 
	INNER JOIN emp e ON u.fuser = e.callsign 
	WHERE e.sales = 1 AND e.Status=0 
		AND u.fUser = @UserName) as a
ON a.fuser = td.fUser
GO