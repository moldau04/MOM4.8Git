CREATE PROCEDURE [dbo].[spRecurringDetails]
	As

Begin 
SELECT (SELECT TOP 1 NAME
        FROM   rol r
        WHERE  o.rol = r.id)AS Customer,
       l.ID                 AS [Location Id],
       l.Tag                AS [Location],
	   l.Type as [Loc Type],
       j.ctype              AS [Service Type],
       j.fdesc              AS Description,
	   (select name from route where id =  j.Custom20) as [Preferred Worker],
	   CONVERT(VARCHAR(10),c.SStart,101) as [Ticket Start],
	   cast (c.STime as time ) as [Ticket Time],
       c.Hours,
       CASE c.scycle
         WHEN 0 THEN 'Monthly'
         WHEN 1 THEN 'Bi-Monthly'
         WHEN 2 THEN 'Quarterly'
         WHEN 3 THEN 'Semi-Anually'
         WHEN 4 THEN 'Anually'
         WHEN 5 THEN 'Weekly'
         WHEN 6 THEN 'Bi-Weekly'
         WHEN 7 THEN 'Every 13 Weeks'
         WHEN 10 THEN 'Every 2 Years'
         WHEN 8 THEN 'Every 3 Years'
         WHEN 9 THEN 'Every 5 Years'
         WHEN 11 THEN 'Every 7 Years'
         WHEN 12 THEN 'On-Demand'
       END                  [Ticket Freq],
	   CONVERT(VARCHAR(10),c.BStart,101) as [Bill Start],
       c.BAmt               AS [Bill Amount],
       CASE c.bcycle
         WHEN 0 THEN 'Monthly'
         WHEN 1 THEN 'Bi-Monthly'
         WHEN 2 THEN 'Quarterly'
         WHEN 3 THEN '3 Times/Year'
         WHEN 4 THEN 'Semi-Annually'
         WHEN 5 THEN 'Anually'
         WHEN 6 THEN 'Never'
       END                  [Bill Freqency],
       CASE j.Status
         WHEN 0 THEN 'Active'
         WHEN 1 THEN 'Closed'
         WHEN 2 THEN 'Hold'
         WHEN 3 THEN 'Completed'
       END                  Status,
	   Expiration,
	   CONVERT(VARCHAR(10),ExpirationDate,101) As [Expiration Date],
	   (select top 1 (select unit from elev where id = jej.Elev) from tblJoinElevJob jej where Job = j.ID ) as Equipment,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 1
and cj.JobID= j.ID
) [Phone Monitoring],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 2
and cj.JobID= j.ID
) [Contract Type],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 3
and cj.JobID= j.ID
)[Occupancy Discount],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 4
and cj.JobID= j.ID
)Exclusions,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 5
and cj.JobID= j.ID
)[Term of Contract],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 6
and cj.JobID= j.ID
)[Price Adjustment Cap],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 7
and cj.JobID= j.ID
)[Fire Service Testing Included],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 8
and cj.JobID= j.ID
)[Special Rates],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 9
and cj.JobID= j.ID
)[Contract Expiration],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 10
and cj.JobID= j.ID
)[Prorated Items],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 11
and cj.JobID= j.ID
)[Annual Test Included],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 12
and cj.JobID= j.ID
)[Five Year State Test Included],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 13
and cj.JobID= j.ID
)[Fire Service Tested Included],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 14
and cj.JobID= j.ID
)[Cancellation Notification Days],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 15
and cj.JobID= j.ID
)[Price Adjustment Notification Days],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 16
and cj.JobID= j.ID
)[After Hours Calls Included],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 17
and cj.JobID= j.ID
)[OG Service Calls Included],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 18
and cj.JobID= j.ID
)[Contract Hours],
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 19
and cj.JobID= j.ID
)[Contract Format]
FROM   job j
       INNER JOIN Contract c
               ON j.id = c.Job
       LEFT OUTER JOIN Loc l
                    ON l.Loc = c.Loc
       LEFT OUTER JOIN owner o
                    ON o.id = l.owner
       LEFT OUTER JOIN rol r
                    ON o.rol = r.id


End
