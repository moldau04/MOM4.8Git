CREATE proc [dbo].[spWorkerChangeCalculation]
@WorkerData As [dbo].[tblTypeWorkerDetails] Readonly
as

SELECT r.Name, 
       Sum (c.BAmt)        BAmt, 
       Sum(c.Hours)        Hours, 
       Sum(Round (CASE c.BCycle 
                    WHEN 0 THEN c.BAmt 
                    WHEN 1 THEN c.BAmt / 6 
                    WHEN 2 THEN c.BAmt / 4 
                    WHEN 3 THEN c.BAmt / 3 
                    WHEN 4 THEN c.BAmt / 2 
                    WHEN 5 THEN c.BAmt / 12 
                    WHEN 6 THEN 0 
                  END, 2)) AS MonthlyBill, 
       Sum(Round (CASE c.SCycle 
                    WHEN 0 THEN c.Hours 
                    WHEN 1 THEN c.Hours / 6 
                    WHEN 2 THEN c.Hours / 4 
                    WHEN 3 THEN c.Hours / 3 
                    WHEN 4 THEN c.Hours / 2 
                    WHEN 5 THEN c.Hours / 12 
                    WHEN 6 THEN 0 
                  END, 2)) AS MonthlyHours 
FROM   job j 
       INNER JOIN Contract c 
               ON j.id = c.Job 
       INNER JOIN Loc l 
               ON l.Loc = j.Loc 
       inner join @WorkerData w on w.loc=l.Loc
       inner JOIN route r 
                     ON r.ID = w.Route 
GROUP  BY r.Name