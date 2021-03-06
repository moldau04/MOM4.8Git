 CREATE PROCEDURE [dbo].[spGetSupervisorByTypeId]
 @TypeId INT
 AS
 BEGIN
	SELECT e.ID,LTRIM(RTRIM(u.fUser)) AS fUser FROM tblUser u
	INNER JOIN tblUserRole ur on ur.UserId = u.ID
	INNER JOIN tblRole r on r.Id = ur.RoleId
	LEFT JOIN Emp e on e.CallSign=u.fUser
	WHERE r.RoleName = 'Supervisor'
		AND e.ID IN (SELECT emp FROM tbljoinempdepartment WHERE Department=@TypeId GROUP BY emp)
 END