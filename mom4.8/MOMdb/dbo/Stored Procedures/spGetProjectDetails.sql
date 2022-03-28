CREATE PROCEDURE [dbo].[spGetProjectDetails]

@jobtype int


as
	begin

	if @jobtype>=0
	select (SELECT id FROM   estimate WHERE  job = 1) AS estimateid, 
j.id, j.fdesc, 
CASE j.status WHEN 0 THEN 'Active'WHEN 1 THEN 'Inactive'END AS status, 
(SELECT tag FROM   loc WHERE  loc = j.loc)	AS locname,
convert(varchar(10),cast(j.fDate as date),101) as fDate,sum(isnull(po.Amount,0))AS TotalOnOrder,sum(isnull(d.ActualHr,0))AS Hour, 
sum(isnull(i.Amount,0)) AS TotalBilled, sum(isnull(d.LaborExp,0)) as LaborExp,
sum(isnull(d.Expense,0)) as Expenses,sum(isnull(p.Amount,0)) as MaterialExp,
( (sum(isnull(d.LaborExp,0)) + sum(isnull(d.Expense,0))) + sum(isnull(p.Amount,0)) )   AS TotalExp, 
( sum(isnull(i.Amount,0)) - sum(isnull(p.Amount,0)) - sum(isnull(d.LaborExp,0)) -  sum(isnull(d.Expense,0))) AS Net,
case when (sum(isnull(i.Amount,0)) > 0)
then 
((( sum(isnull(i.Amount,0)) - sum(isnull(p.Amount,0)) - sum(isnull(d.LaborExp,0)) -  sum(isnull(d.Expense,0))) / sum(isnull(i.Amount,0))) * 100)
else 0 end as NetPercent,
department.ID  as DepartmentId,
department.Type
FROM   Job j  
inner join JobT on j.Template=JobT.ID
inner join JobType department on JobT.Type=department.ID
LEFT OUTER JOIN
(SELECT t.Job, sum(isnull(t.Expense,0)) as Expense, sum(isnull(LaborExp,0)) as LaborExp, sum(isnull(ActualHr,0)) as ActualHr FROM
(SELECT t.ID,t.Job,t.edate,(isnull(t.zone,0) + isnull(t.toll, 0) + isnull(t.othere,0)) AS Expense,
(CONVERT(NUMERIC(30, 2), ( Isnull(t.Reg, 0) + ( Isnull(t.OT, 0) * 1.5 ) +( (Isnull(t.DT, 0)) * 2 ) + ( (Isnull(t.NT, 0)) * 1.7 ) + 
Isnull(t.TT, 0) ) * Isnull(w.HourlyRate, 0))) AS LaborExp,((isnull(t.Reg,0) + isnull(t.RegTrav,0)) + ((isnull(t.OT,0) + isnull(t.OTTrav,0)) * 1.5) + 
((isnull(t.NT,0) + isnull(t.NTTrav,0)) * 1.7) + ((isnull(t.DT,0) + isnull(t.DTTrav,0)) * 2) + (isnull(t.TT,0))) AS ActualHr from TicketD t LEFT JOIN tblWork w 
ON t.fWork = w.ID WHERE t.Job IS NOT NULL
UNION
SELECT t.ID,t.Job,t.edate,(isnull(t.zone,0) + isnull(t.toll, 0) + isnull(t.othere,0)) AS Expense,
(CONVERT(NUMERIC(30, 2), ( Isnull(t.Reg, 0) + ( Isnull(t.OT, 0) * 1.5 ) +( (Isnull(t.DT, 0)) * 2 ) + ( (Isnull(t.NT, 0)) * 1.7 ) + 
Isnull(t.TT, 0) ) * Isnull(w.HourlyRate, 0))) AS LaborExp,
((isnull(t.Reg,0) + isnull(t.RegTrav,0)) + ((isnull(t.OT,0) + isnull(t.OTTrav,0)) * 1.5) + ((isnull(t.NT,0) + isnull(t.NTTrav,0)) * 1.7) + ((isnull(t.DT,0) + isnull(t.DTTrav,0)) * 2) + (isnull(t.TT,0))) AS ActualHr
FROM TicketDPDA t LEFT JOIN tblWork w ON t.fWork = w.ID LEFT JOIN Loc l ON t.Loc = l.Loc	
WHERE t.Job IS NOT NUll ) as t 
where t.job is not null and t.job <> 0
GROUP BY t.Job)  as d ON d.Job = j.ID   
LEFT JOIN 
(SELECT inv.Job, sum(isnull(inv.Price,0) * isnull(inv.Quan,0)) as Amount FROM InvoiceI inv  LEFT JOIN Invoice i ON inv.Ref = i.Ref WHERE inv.Job is not null GROUP BY inv.Job) as i ON i.Job = j.ID
LEFT JOIN
(SELECT jobi.Job, sum(isnull(jobi.Amount,0)) as Amount FROM 	JobI as jobi LEFT JOIN Trans as t ON Abs(JobI.TransID) = t.ID AND t.Type IN (41,40) WHERE jobi.Type = 1 and t.ID is not null GROUP BY jobi.Job) as p ON p.Job = j.ID
LEFT JOIN 
(SELECT poi.Job, sum(isnull(poi.Amount,0)) as Amount FROM POItem as poi LEFT JOIN PO as p ON poi.PO = p.PO WHERE p.Status <> 5 GROUP BY poi.Job) as po ON po.Job = j.ID
WHERE j.id is not null and department.ID=@jobtype GROUP  BY j.ID, j.fdesc, j.Status, j.Loc, j.fDate, j.Rev, j.PO, j.Mat, j.Labor,department.ID,
department.Type 
	else
	select (SELECT id FROM   estimate WHERE  job = 1) AS estimateid, 
j.id, j.fdesc, 
CASE j.status WHEN 0 THEN 'Active'WHEN 1 THEN 'Inactive'END AS status, 
(SELECT tag FROM   loc WHERE  loc = j.loc)	AS locname,
convert(varchar(10),cast(j.fDate as date),101) as fDate,sum(isnull(po.Amount,0))AS TotalOnOrder,sum(isnull(d.ActualHr,0))AS Hour, 
sum(isnull(i.Amount,0)) AS TotalBilled, sum(isnull(d.LaborExp,0)) as LaborExp,
sum(isnull(d.Expense,0)) as Expenses,sum(isnull(p.Amount,0)) as MaterialExp,
( (sum(isnull(d.LaborExp,0)) + sum(isnull(d.Expense,0))) + sum(isnull(p.Amount,0)) )   AS TotalExp, 
( sum(isnull(i.Amount,0)) - sum(isnull(p.Amount,0)) - sum(isnull(d.LaborExp,0)) -  sum(isnull(d.Expense,0))) AS Net,
case when (sum(isnull(i.Amount,0)) > 0)
then 
((( sum(isnull(i.Amount,0)) - sum(isnull(p.Amount,0)) - sum(isnull(d.LaborExp,0)) -  sum(isnull(d.Expense,0))) / sum(isnull(i.Amount,0))) * 100)
else 0 end as NetPercent,
department.ID,
department.Type
FROM   Job j  
left outer join JobT on j.Template=JobT.ID
left outer join JobType department on JobT.Type=department.ID
LEFT OUTER JOIN
(SELECT t.Job, sum(isnull(t.Expense,0)) as Expense, sum(isnull(LaborExp,0)) as LaborExp, sum(isnull(ActualHr,0)) as ActualHr FROM
(SELECT t.ID,t.Job,t.edate,(isnull(t.zone,0) + isnull(t.toll, 0) + isnull(t.othere,0)) AS Expense,
(CONVERT(NUMERIC(30, 2), ( Isnull(t.Reg, 0) + ( Isnull(t.OT, 0) * 1.5 ) +( (Isnull(t.DT, 0)) * 2 ) + ( (Isnull(t.NT, 0)) * 1.7 ) + 
Isnull(t.TT, 0) ) * Isnull(w.HourlyRate, 0))) AS LaborExp,((isnull(t.Reg,0) + isnull(t.RegTrav,0)) + ((isnull(t.OT,0) + isnull(t.OTTrav,0)) * 1.5) + 
((isnull(t.NT,0) + isnull(t.NTTrav,0)) * 1.7) + ((isnull(t.DT,0) + isnull(t.DTTrav,0)) * 2) + (isnull(t.TT,0))) AS ActualHr from TicketD t LEFT JOIN tblWork w 
ON t.fWork = w.ID WHERE t.Job IS NOT NULL
UNION
SELECT t.ID,t.Job,t.edate,(isnull(t.zone,0) + isnull(t.toll, 0) + isnull(t.othere,0)) AS Expense,
(CONVERT(NUMERIC(30, 2), ( Isnull(t.Reg, 0) + ( Isnull(t.OT, 0) * 1.5 ) +( (Isnull(t.DT, 0)) * 2 ) + ( (Isnull(t.NT, 0)) * 1.7 ) + 
Isnull(t.TT, 0) ) * Isnull(w.HourlyRate, 0))) AS LaborExp,
((isnull(t.Reg,0) + isnull(t.RegTrav,0)) + ((isnull(t.OT,0) + isnull(t.OTTrav,0)) * 1.5) + ((isnull(t.NT,0) + isnull(t.NTTrav,0)) * 1.7) + ((isnull(t.DT,0) + isnull(t.DTTrav,0)) * 2) + (isnull(t.TT,0))) AS ActualHr
FROM TicketDPDA t LEFT JOIN tblWork w ON t.fWork = w.ID LEFT JOIN Loc l ON t.Loc = l.Loc	
WHERE t.Job IS NOT NUll ) as t 
where t.job is not null and t.job <> 0
GROUP BY t.Job)  as d ON d.Job = j.ID   
LEFT JOIN 
(SELECT inv.Job, sum(isnull(inv.Price,0) * isnull(inv.Quan,0)) as Amount FROM InvoiceI inv  LEFT JOIN Invoice i ON inv.Ref = i.Ref WHERE inv.Job is not null GROUP BY inv.Job) as i ON i.Job = j.ID
LEFT JOIN
(SELECT jobi.Job, sum(isnull(jobi.Amount,0)) as Amount FROM 	JobI as jobi LEFT JOIN Trans as t ON Abs(JobI.TransID) = t.ID AND t.Type IN (41,40) WHERE jobi.Type = 1 and t.ID is not null GROUP BY jobi.Job) as p ON p.Job = j.ID
LEFT JOIN 
(SELECT poi.Job, sum(isnull(poi.Amount,0)) as Amount FROM POItem as poi LEFT JOIN PO as p ON poi.PO = p.PO WHERE p.Status <> 5 GROUP BY poi.Job) as po ON po.Job = j.ID
WHERE j.id is not null GROUP  BY j.ID, j.fdesc, j.Status, j.Loc, j.fDate, j.Rev, j.PO, j.Mat, j.Labor,department.ID,
department.Type




	end