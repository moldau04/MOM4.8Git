Create Procedure spGetUserDetailByEmpID
@EmpID int
AS
BEGIN
	SELECT 
		   e.ID AS Empid,
		   u.ID AS userid,      
		   fUser,      
		   r.Phone,
		   r.Address,
		   isnull(EMail,'') as Email,
		   Cellular,
		   Field,
		   isnull(fFirst,'') as FirstName,
		   Middle,
		   isnull(e.Last,'') as LastName,      
		   CallSign,  
		   r.Contact,
		   u.Title,
		   u.ProfileImage,
		   u.CoverImage     
	FROM tblUser u
	LEFT OUTER JOIN Emp e ON u.fUser=e.CallSign
	LEFT OUTER JOIN Rol r ON e.Rol=r.ID
	LEFT OUTER JOIN tblWork w ON w.ID=e.fWork
	WHERE e.ID=@EmpID
END

