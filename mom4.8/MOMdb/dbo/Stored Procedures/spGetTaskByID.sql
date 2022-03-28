CREATE proc [dbo].[spGetTaskByID]
@id int

as

SELECT T.ID,
		T.Rol,
		T.Type,
		T.DateDue,
		T.TimeDue,
		T.Subject,
		T.Remarks AS Remarks,
		T.Keyword,       
		T.fUser,
		--R.Name,
		CASE r.Type 
			WHEN 4 THEN (
				SELECT TOP 1 l.Tag AS Name
				FROM Loc l
					LEFT OUTER JOIN Rol r ON l.Rol = r.ID
				WHERE r.ID = T.Rol)
            WHEN 3 THEN r.Name
        END AS Name,
		r.EN,
		B.Name As Company,
		T.Duration,
		T.CreateDate,
		T.lastupdatedate,
		T.createdby,
		T.LastUpdatedBy,
		CAST(CAST(DateDue AS DATE) AS DATETIME) + CAST(CAST(TimeDue AS TIME) AS DATETIME) as duedate,
		(Datediff(day, DateDue, Getdate())) AS days,
		(CASE r.Type
				WHEN 0 THEN 'Customer'
				WHEN 1 THEN 'Vendor'
				WHEN 2 THEN 'Bank'
				WHEN 3 THEN 'Lead'
				WHEN 4 THEN 'Account'
				WHEN 5 THEN 'Employee'
				ELSE 'Misc'
        END) AS contacttype,
        0 as statusid,
        '' as result,
		T.Contact,
		T.Phone,
		T.Email,
        CASE r.Type 
			WHEN 4 THEN (
				SELECT TOP 1 ro.Name AS CustomerName
				FROM Loc l
						LEFT OUTER JOIN Rol r ON l.Rol = r.ID
						INNER JOIN Owner o ON o.ID = l.owner
						LEFT OUTER JOIN Rol ro ON o.Rol = ro.ID
				WHERE r.ID = T.Rol)
            WHEN 3 THEN (SELECT TOP 1 CustomerName FROM Prospect WHERE Rol = r.ID)
        END AS CustomerName,
		ISNULL(T.IsAlert,0) IsAlert
FROM ToDo T
INNER JOIN Rol r ON T.Rol = R.ID 
left Outer join Branch B on B.ID = R.EN
where T.ID=@id
union all
SELECT T.ID,
		T.Rol,
		T.Type,
		T.Datedone as DateDue,
		T.Timedone as TimeDue,
		T.Subject,
		T.Remarks AS Remarks,
		T.Keyword,       
		T.fUser,
		--R.Name,
		CASE r.Type 
			WHEN 4 THEN (
				SELECT TOP 1 l.Tag AS Name
				FROM Loc l
					LEFT OUTER JOIN Rol r ON l.Rol = r.ID
				WHERE r.ID = T.Rol)
            WHEN 3 THEN r.Name
        END AS Name,
		r.EN,
		B.Name As Company,
		T.Duration,
		T.CreateDate,
		T.lastupdatedate,
		T.createdby,
		T.LastUpdatedBy,
		CAST(CAST(Datedone AS DATE) AS DATETIME) + CAST(CAST(TimeDone AS TIME) AS DATETIME) as duedate,
		( Datediff(day, Datedone, Getdate()) ) AS days,
		(CASE r.Type
                WHEN 0 THEN 'Customer'
                WHEN 1 THEN 'Vendor'
                WHEN 2 THEN 'Bank'
                WHEN 3 THEN 'Prospect'
                WHEN 4 THEN 'Account'
                WHEN 5 THEN 'Employee'
                ELSE 'Misc'
        END) AS contacttype,
        1 as statusid,
        result,
		T.Contact,
		T.Phone,
		T.Email,
		 CASE r.Type 
			WHEN 4 THEN (
				SELECT TOP 1 ro.Name AS CustomerName
				FROM Loc l
						LEFT OUTER JOIN Rol r ON l.Rol = r.ID
						INNER JOIN Owner o ON o.ID = l.owner
						LEFT OUTER JOIN Rol ro ON o.Rol = ro.ID
				WHERE r.ID = T.Rol)
            WHEN 3 THEN (SELECT TOP 1 CustomerName FROM Prospect WHERE Rol = r.ID)
        END AS CustomerName,
		ISNULL(T.IsAlert,0) IsAlert
FROM   done T
INNER JOIN Rol r  ON T.Rol = R.ID 
left Outer join Branch B on B.ID = R.EN
where T.ID=@id
