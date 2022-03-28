CREATE PROCEDURE [dbo].[spGetEmployeeList]      
 As      
 Begin      
       
 SELECT Emp.ID as  UserID,
case when isnull(fWork,'')='' then 0 else 1  end as usertypeid,      
Emp.ID, Emp.Last, Emp.fFirst, Emp.Name, Emp.Title, Emp.Status,CASE WHEN Emp.Status = 0 THEN 'Active' WHEN Emp.Status = 1 THEN 'InActive' WHEN Emp.Status=2 THEN 'Hold' END AS SSTATUS,      
Emp.SSN, Emp.Field, CASE WHEN Emp.Field=0 THEN 'Yes' WHEN Emp.Field=1 THEN 'No' END AS FField, Emp.Rol, Emp.Ref   
FROM Emp INNER JOIN Rol ON Emp.Rol = Rol.ID       
--INNER JOIN tblUser u ON u.fUser=Emp.CallSign      
LEFT OUTER JOIN tblWork ON       
Emp.fWork = tblWork.ID  ORDER BY Emp.Name      
      
 End 