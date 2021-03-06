CREATE VIEW [dbo].[VendorReportDetails]
	AS 
	
SELECT DISTINCT v.ID,v.Rol,r.Name,v.Acct,v.Type,CASE WHEN v.Status = 0 THEN 'Active' ELSE 'Inactive' END AS Status,v.Balance,v.CLimit,v.[1099],v.FID,v.DA,v.Acct#,v.Terms,v.Disc,v.Days,
v.InUse,v.Remit,v.OnePer,v.DBank,v.Custom1,v.Custom2,v.Custom3,v.Custom4,v.Custom5,v.Custom6,v.Custom7,v.Custom8,v.Custom9,
v.Custom10,v.ShipVia,v.QBVendorID FROM Vendor v, Rol r where v.Rol=r.ID

Go