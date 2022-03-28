Create PROCEDURE [dbo].[spGetUserForEstimate]
	
AS

BEGIN
	


	SELECT t.ID AS value, t.fUser as label, e.fFirst, e.Last as fLast, r.Email, r.Cellular
				 FROM [dbo].[tblUser] t 
					LEFT JOIN [dbo].[Emp] e ON e.CallSign = t.fUser
					LEFT JOIN [dbo].[Rol] r ON r.ID = e.Rol
					where e.Sales=1 and e.Status=0
					order by e.Last
	


END
