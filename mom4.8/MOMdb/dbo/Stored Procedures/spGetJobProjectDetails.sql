CREATE PROCEDURE [dbo].[spGetJobProjectDetails]
AS
Begin
SELECT l.Tag As Location,j.ID As [Project#], j.fdesc As Description, 
CASE j.status WHEN 0 THEN 'Open' WHEN 1 THEN 'Closed' WHEN 2 THEN 'Hold' WHEN 3 THEN 'Completed' END as Status, 
jt.Type,j.fDate As [Date Created],
ISNULL(j.Hour,0) as Hours,
ISNULL(j.Comm,0) as [Total On Order], 
ISNULL(j.Rev,0) as [Total Billed], 
ISNULL(j.Labor,0) as [Labor Expense],
ISNULL(j.Mat,0) - ISNULL((SELECT SUM(Amount) FROM JobI WHERE Type = 1 AND ISNULL(JobI.Labor,0) = 0 AND TransID < 0 AND JobI.Job = j.ID),0) as [Material Expense], 
ISNULL((SELECT SUM(ISNULL(Amount,0)) FROM JobI WHERE Type = 1 AND ISNULL(JobI.Labor,0) = 0 AND TransID < 0 AND JobI.Job = j.ID),0) as Expenses,
ISNULL(j.Cost,0) as [Total Expenses], 
ISNULL(j.Profit,0) as Net, 
ISNULL(j.Ratio,0) as [% in Profit] ,
(Select Name From rol where id =(select rol from Owner where id=j.Owner)) as Customer
FROM Job j	INNER JOIN Loc l ON j.Loc = l.Loc 
INNER JOIN Rol r ON l.Rol =r.ID 
INNER JOIN Owner o ON l.Owner=o.ID 
INNER JOIN JobType jt ON j.Type = jt.ID 
End
