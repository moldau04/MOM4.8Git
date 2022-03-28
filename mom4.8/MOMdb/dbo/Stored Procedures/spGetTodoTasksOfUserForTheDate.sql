/**
Thomas
To fix bug on json convert datetime, we need to modify year in TimeDue columns to make sure it > 1975
DATEADD(year,76, td.TimeDue) as TimeDue
*/

CREATE PROC [dbo].[spGetTodoTasksOfUserForTheDate]
	@UserID int,
	@Date DATETIME
AS
SELECT td.ID 
	, td.Rol
	, td.DateDue
	, DATEADD(year,76, td.TimeDue) as TimeDue
	, td.Subject
	, td.Remarks
	, td.Contact
	, r.Name 
FROM todo td
INNER JOIN Rol r ON td.Rol = r.ID
INNER JOIN 
	(SELECT --e.Last+', '+e.fFirst as fuser,
		u.fuser FROM tbluser u 
	INNER JOIN emp e ON u.fuser = e.callsign 
	WHERE 
		--e.sales = 1 AND
		e.Status=0 
		--AND u.fUser = @UserName
		AND u.ID = @UserID
) as a
ON a.fuser = td.fUser
WHERE @Date + 7 >= td.DateDue
Order by DateDue asc, TimeDue asc
GO