 CREATE PROCEDURE [dbo].[spGetAssignedProjectByTypeId]
 @TypeId INT
 AS
 BEGIN
	 SELECT e.ID,LTRIM(RTRIM(u.fUser)) AS fUser
	 FROM tblUser u
	 LEFT JOIN Emp e on e.CallSign=u.fUser
	 WHERE e.ID in(SELECT emp FROM tbljoinempdepartment WHERE Department=@TypeId GROUP BY emp)
	 AND u.IsAssignedProject=1
 END