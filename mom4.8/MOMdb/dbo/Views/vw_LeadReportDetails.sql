CREATE VIEW [dbo].[vw_LeadReportDetails]
	AS SELECT p.ID As [Prospect#],
       p.Type,
	   CustomerName As [Customer Name],
       Ltrim(Rtrim(r.Name))  AS Name,
       r.City,
       r.State,
	   Case p.Status When 0 Then 'Active'
	   Else 'InActive' End As Status,
       ( Datediff(day, [ldate], Getdate()) ) AS [#Days],
	   (select count(rol) from Lead where rol = p.Rol) as #Opps,
       (select Name from terr t where t.id=p.terr) as [Sales Person]
	   
FROM   Prospect p
       INNER JOIN Rol r
               ON r.ID = p.Rol 
       where p.id is not null     