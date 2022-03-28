CREATE PROCEDURE [dbo].[spAllVendorSchedules]
AS
select p.ID
, p.PID VendorID
, r.Name VendorName
, p.[Desc] 
, p.CreatedDt
, p.CreatedBy
, p.UpdatedDt
, p.UpdatedBy
from Planner p
inner join Vendor v on p.PID = v.id and p.Type = 'vendor'
left join Rol r on r.id = v.Rol
