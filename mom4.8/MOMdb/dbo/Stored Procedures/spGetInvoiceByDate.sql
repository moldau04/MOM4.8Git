Create PROCEDURE [dbo].[spGetInvoiceByDate]
	@fDate datetime	
AS
Begin

	SELECT i.fDate as InvDate, i.Ref, 	
	ISNULL(i.Custom1,'') + ' ' + Convert(varchar, i.fDesc) as Description,
	ar.Balance, ar.Type as Dep,r.Name  as Customer, Loc.Tag,Loc.Tag +'-' + Loc.ID as Location, Loc.ID, isnull(jt.Type,'') as Type,
	isnull(DATEDIFF(DAY, i.fDate, @fDate), 0) as Age,i.DDate as DueDate,0 AS Retainage,
CASE
	WHEN (isnull(DATEDIFF(DAY, isnull(ar.fDate,ar.Due), @fDate), 0) <=1) THEN ar.Balance
	ELSE 0
END AS CurrentDay,
CASE
	WHEN (
		isnull(DATEDIFF(DAY, isnull(ar.fDate,ar.Due), @fDate), 0) <=30) THEN ar.Balance
	ELSE 0
END AS ThirtyDay,
CASE		
	WHEN (isnull(DATEDIFF(DAY, isnull(ar.fDate,ar.Due), @fDate), 0) > 30)
		AND (isnull(DATEDIFF(DAY, isnull(ar.fDate,ar.Due), @fDate), 0) <=60) THEN ar.Balance
	ELSE 0
END AS SixtyDay,
CASE	
	WHEN (isnull(DATEDIFF(DAY, isnull(ar.fDate,ar.Due), @fDate), 0) >60)
		AND (isnull(DATEDIFF(DAY, isnull(ar.fDate,ar.Due), @fDate), 0) <=90) THEN ar.Balance
	ELSE 0
END AS NintyDay,
CASE		
	WHEN (isnull(DATEDIFF(DAY, isnull(ar.fDate,ar.Due), @fDate), 0) >90) THEN ar.Balance
	ELSE 0
END AS OverNintyDay
 FROM   OpenAR ar 
 INNER JOIN Loc Loc ON ar.Loc=Loc.Loc 
 INNER JOIN owner o  ON o.id = Loc.owner 
 INNER JOIN rol r ON o.rol = r.id 
 INNER JOIN Invoice i ON ar.TransID=i.TransID  
 LEFT OUTER JOIN Job j ON i.Job=j.ID 
 LEFT OUTER JOIN JobType jt ON j.Type=jt.ID
 WHERE  ar.fDate<= @fDate 
 and ar.Balance <>0 
order by r.Name , jt.Type

END
