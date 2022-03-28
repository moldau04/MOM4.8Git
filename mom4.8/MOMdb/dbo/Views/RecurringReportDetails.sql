CREATE VIEW [dbo].[RecurringReportDetails]
AS
SELECT        j.ID AS ContractNo, j.CType AS ContractServiceType, j.fDesc AS ContractDescription, (CASE j.Status WHEN 0 THEN 'Active' WHEN 1 THEN 'Closed' WHEN 2 THEN 'Hold' WHEN 3 THEN 'Completed' END) 
                         AS ContractStatus, (CASE c.BCycle WHEN 0 THEN 'Monthly' WHEN 1 THEN 'Bi-Monthly' WHEN 2 THEN 'Quarterly' WHEN 3 THEN 'Semi-Anually' WHEN 4 THEN 'Anually' END) AS BillingFrequency, 
                         CAST(ROUND(CASE c.BCycle WHEN 0 THEN c.BAmt WHEN 1 THEN c.BAmt / 2 WHEN 2 THEN c.BAmt / 3 WHEN 3 THEN c.BAmt / 6 WHEN 4 THEN c.BAmt / 12 END, 2) AS Numeric(15, 2)) AS MonthlyBilling, 
                         CAST(ROUND(CASE c.SCycle WHEN 0 THEN c.Hours WHEN 1 THEN c.Hours / 2 WHEN 2 THEN c.Hours / 3 WHEN 3 THEN c.Hours / 6 WHEN 4 THEN c.Hours / 12 WHEN 5 THEN c.Hours * 4.3 / 12 WHEN 6 THEN c.Hours
                          * 2.15 / 12 WHEN 10 THEN c.Hours / 12 * 2 WHEN 8 THEN c.Hours / 12 * 3 WHEN 9 THEN c.Hours / 12 * 5 WHEN 11 THEN c.Hours / 12 * 7 END, 2) AS Numeric(15, 2)) AS MonthlyHours, 
                         (CASE c.SCycle WHEN 0 THEN 'Monthly' WHEN 1 THEN 'Bi-Monthly' WHEN 2 THEN 'Quarterly' WHEN 3 THEN 'Semi-Anually' WHEN 4 THEN 'Anually' WHEN 5 THEN 'Weekly' WHEN 6 THEN 'Bi-Weekly' WHEN 7
                          THEN 'Every 13 Weeks' WHEN 10 THEN 'Every 2 Years' WHEN 8 THEN 'Every 3 Years' WHEN 9 THEN 'Every 5 Years' WHEN 11 THEN 'Every 7 Years' WHEN 12 THEN 'On-Demand' END) AS TicketFrequency, 
                         c.BAmt AS TotalPeriodBilling, c.Hours AS TotalPeriodHours, c.Loc AS ContLocId, c.SStart AS ScheduleDate, r.Name AS PreferredWorker
FROM            dbo.Contract AS c INNER JOIN
                         dbo.Job AS j ON c.Job = j.ID INNER JOIN
                         dbo.Route AS r ON j.Custom20 = r.ID
