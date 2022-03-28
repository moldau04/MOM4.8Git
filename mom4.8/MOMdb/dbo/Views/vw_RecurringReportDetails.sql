CREATE VIEW [dbo].[vw_RecurringReportDetails]
	SELECT (SELECT TOP 1 NAME
        FROM   rol r
        WHERE  o.rol = r.id)AS Customer,
       l.ID                 AS LocationId,
       l.Tag                AS Location,
	   l.Type as LocType,
       j.ctype              AS ServiceType,
       j.fdesc              AS Description,
	   (select name from route where id =  j.Custom20) as PreferredWorker,
	   c.SStart as TicketStart,
	   cast (c.STime as time ) as TicketTime,
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
       END                  TicketFreq,
	   c.BStart as BillStart,
       c.BAmt               AS BillAmount,
       CASE c.bcycle
         WHEN 0 THEN 'Monthly'
         WHEN 1 THEN 'Bi-Monthly'
         WHEN 2 THEN 'Quarterly'
         WHEN 3 THEN '3 Times/Year'
         WHEN 4 THEN 'Semi-Annually'
         WHEN 5 THEN 'Anually'
         WHEN 6 THEN 'Never'
       END                  BillFreqency,
       CASE j.Status
         WHEN 0 THEN 'Active'
         WHEN 1 THEN 'Closed'
         WHEN 2 THEN 'Hold'
         WHEN 3 THEN 'Completed'
       END                  Status,
	   Expiration,
	   ExpirationDate,
	   (select top 1 (select unit from elev where id = jej.Elev) from tblJoinElevJob jej where Job = j.ID ) as Equipment,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 1
and cj.JobID= j.ID
) PhoneMonitoring,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 2
and cj.JobID= j.ID
) ContractType,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 3
and cj.JobID= j.ID
)OccupancyDiscount,
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
)TermofContract,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 6
and cj.JobID= j.ID
)PriceAdjustmentCap,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 7
and cj.JobID= j.ID
)FireServiceTestingIncluded,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 8
and cj.JobID= j.ID
)SpecialRates,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 9
and cj.JobID= j.ID
)ContractExpiration,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 10
and cj.JobID= j.ID
)ProratedItems,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 11
and cj.JobID= j.ID
)AnnualTestIncluded,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 12
and cj.JobID= j.ID
)FiveYearStateTestIncluded,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 13
and cj.JobID= j.ID
)FireServiceTestedIncluded,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 14
and cj.JobID= j.ID
)CancellationNotificationDays,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 15
and cj.JobID= j.ID
)PriceAdjustmentNotificationDays,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 16
and cj.JobID= j.ID
)AfterHoursCallsIncluded,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 17
and cj.JobID= j.ID
)OGServicecallsIncluded,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 18
and cj.JobID= j.ID
)ContractHours,
(SELECT 
ISNULL(cj.Value,'') AS Value  
FROM tblCustomJobT cj 
INNER JOIN tblCustomFields t ON t.ID = cj.tblCustomFieldsID 
INNER JOIN JobT jt ON jt.ID = cj.JobTID    
WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0) and line = 19
and cj.JobID= j.ID
)ContractFormat
FROM   job j
       INNER JOIN Contract c
               ON j.id = c.Job
       LEFT OUTER JOIN Loc l
                    ON l.Loc = c.Loc
       LEFT OUTER JOIN owner o
                    ON o.id = l.owner
       LEFT OUTER JOIN rol r
                    ON o.rol = r.id 
GO
