CREATE VIEW [dbo].[CustomerReportDetails]
AS

SELECT     TOP (100) PERCENT r.Name, r.City, r.State, r.Zip, r.Phone, r.Fax, r.Contact, r.Address, r.EMail, r.Country, o.Type, o.Balance, CASE WHEN o.Status = 0 THEN 'Active' ELSE 'Inactive' END AS Status,
                          (SELECT     COUNT(1) AS Expr1
                            FROM          dbo.Loc
                            WHERE      (Owner = o.ID)) AS loc,
                          (SELECT     COUNT(1) AS Expr1
                            FROM          dbo.Elev
                            WHERE      (Owner = o.ID)) AS equip,
                          (SELECT     COUNT(1) AS Expr1
                            FROM          dbo.TicketO
                            WHERE      (Owner = o.ID)) AS opencall, r.Website, r.Cellular
FROM         dbo.Rol AS r INNER JOIN
                      dbo.Owner AS o ON r.ID = o.Rol
ORDER BY r.Name
GO