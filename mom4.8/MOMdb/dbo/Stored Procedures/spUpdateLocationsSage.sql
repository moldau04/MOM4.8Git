CREATE proc [dbo].[spUpdateLocationsSage]
as
SELECT (SELECT SageID 
        FROM   Owner 
        WHERE  ID = l.Owner)                                            AS sagecustomerid, 
       SageID, 
       Substring(tag, 1, 30)                                            AS tag, 
       Loc                                                              AS ID, 
       substring(l.Remarks,1,500) as remarks, 
       CASE l.Status 
         WHEN 0 THEN 'Active' 
         ELSE 'Inactive' 
       END                                                              AS Status, 
       
         Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(l.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 1), 1, 30) AS Address1, 
       Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(l.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 2), 1, 30) AS Address2, 
                  
                  Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(r.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 1), 1, 30) AS billAddress1, 
       Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(r.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 2), 1, 30) AS billAddress2, 
                  
       --Substring((SELECT items 
       -- FROM   dbo.Splitsentence(Isnull(l.Address, ''), 30) spl 
       -- WHERE  spl.id = 1)  ,1,30)                                            AS Address1, 
       --substring((SELECT items 
       -- FROM   dbo.Splitsentence(Isnull(l.Address, ''), 30) spl 
       -- WHERE  spl.id = 2) , 1,30)                                            AS Address2, 
        
       Substring(l.City, 1, 15)                                         AS City, 
       Substring(l.State, 1, 4)                                         AS State, 
       Substring(l.Zip, 1, 10)                                          AS Zip, 
       Isnull(LastUpdateDate, '01/01/1900')                             AS LastUpdateDate,Substring(l.type, 1, 15) as type ,
       Substring(r.City,1,15) as billcity,
		Substring(r.State, 1, 4)                                         AS BillState,
		Substring(r.Zip, 1, 10)                                          AS billZip,
		--Substring((SELECT items 
  --      FROM   dbo.Splitsentence(Isnull(r.Address, ''), 30) spl 
  --      WHERE  spl.id = 1) ,1,30)                                             AS billAddress1, 
  --     Substring((SELECT items 
  --      FROM   dbo.Splitsentence(Isnull(r.Address, ''), 30) spl 
  --      WHERE  spl.id = 2)    ,1,30)                                          AS billAddress2, 
		Substring(r.Phone,1,15) as phone,
		r.Website,
		r.EMail,
		Substring(r.Cellular,1,15) as Cellular,
		Substring(r.Fax,1,15) as Fax,
		l.Owner,
		Substring(r.Contact,1,30) as contact,
		Substring(Custom14,1,50) as Custom14,
		case substring(l.ID,9, 11) 
		when '-10' then '1-10' 
		when '-20' then '1-20' 
		when '-30' then '1-30' 
		when '-40' then '1-30' 
		when '-15' then '1-10'
		end as costaccountprefix
FROM   Loc l 
       INNER JOIN Rol r 
               ON r.ID = l.Rol 
WHERE  SageID IS NOT NULL 
       AND LastUpdateDate >= (SELECT SageLastSync 
                              FROM   Control) 
                              
                              
                              
