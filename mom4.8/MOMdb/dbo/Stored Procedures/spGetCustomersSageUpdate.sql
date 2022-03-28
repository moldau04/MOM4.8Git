
CREATE proc [dbo].[spGetCustomersSageUpdate]
as
SELECT o.ID, 
       Isnull(LastUpdateDate, '01/01/1900')   AS LastUpdateDate, 
       Substring(r.NAME, 1, 50)               AS NAME, 
       Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(r.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 1), 1, 30) AS Address1, 
       Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(r.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 2), 1, 30) AS Address2, 
       Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(r.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 3), 1, 30) AS Address3, 
       Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(r.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 4), 1, 30) AS Address4, 
       Substring(r.City, 1, 30)               AS City, 
       Substring(r.Contact, 1, 15)            AS Contact, 
       Substring(r.EMail, 1, 50)              AS EMail, 
       Substring(r.Phone, 1, 15)              AS Phone, 
       Substring(Remarks,1,500) as Remarks, 
       Substring(r.State, 1, 4)               AS State, 
       Substring(r.Zip, 1, 10)                AS Zip, 
       CASE o.Status 
         WHEN 0 THEN 'Active' 
         ELSE 'Inactive' 
       END                                    AS Status, 
       CASE Isnull(o.type, '') 
         WHEN '' THEN 'Standard' 
         ELSE Substring(o.Type, 1, 20) 
       END                                    AS type 
FROM   Owner o 
       LEFT OUTER JOIN Rol r 
                    ON o.Rol = r.ID 
WHERE  SageID IS NOT NULL 
       AND LastUpdateDate >= (SELECT SageLastSync 
                              FROM   Control) 