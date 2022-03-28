CREATE PROCEDURE [dbo].[spGeneralLedger] 
	@cid int,
	@sdate datetime,
	@edate datetime
AS
BEGIN

SELECT 
	acctTemp.ID,
	acctTemp.AcctNo,
	acctTemp.ChartName,
	acctTemp.ChartType,
	ISNULL(acctTemp.Beginning,0) AS Beginning,
	ISNULL(acctTemp.Activity,0) AS Activity,
	ISNULL(acctTemp.Ending,0) AS Ending,
	tranTemp.TID,
	tranTemp.fDate,
	tranTemp.Batch,
	tranTemp.Ref,
	tranTemp.TypeText,
	tranTemp.fDesc,
	tranTemp.Name,
	ISNULL(tranTemp.Amount,0) AS Amount,
	ISNULL(tranTemp.Balance,0) AS Balance,
	ISNULL(tranTemp.Debit,0) AS Debit,
	ISNULL(tranTemp.Credit,0) AS Credit
FROM (
	SELECT
		t.Acct AS ID, 
		c.Acct AS AcctNo,
		c.fDesc AS ChartName,
		c.Type AS ChartType,
		CASE
			WHEN c.Type = 3 OR c.Type = 4 OR c.Type = 5 THEN 0
			ELSE (SELECT SUM(ISNULL(Amount,0)) AS Beginning FROM Trans WHERE Acct = t.Acct AND fDate < @sdate) 
		END AS Beginning,
		(SELECT SUM(ISNULL(Amount,0)) AS Activity FROM Trans WHERE Acct = t.Acct AND fDate >= @sdate AND fDate <=@edate) AS Activity,
		CASE
			WHEN c.Type = 3 OR c.Type = 4 OR c.Type = 5 THEN (SELECT SUM(ISNULL(Amount,0)) AS Activity FROM Trans WHERE Acct = t.Acct AND fDate >= @sdate AND fDate <=@edate)
			ELSE (SELECT SUM(ISNULL(Amount,0)) AS Ending FROM Trans WHERE Acct = t.Acct AND fDate <= @edate)
		END AS Ending
	FROM Chart c	
		INNER JOIN Trans t ON c.ID = t.Acct
	WHERE (@cid = 0 OR c.ID = @cid)	
	GROUP BY t.Acct, c.Acct, t.Acct, c.fDesc, c.Type
) AS acctTemp

LEFT JOIN (
	SELECT  
		t.Acct AS ID, 
		c.Acct AS AcctNo,
		t.ID AS TID,
		t.fDate, 
		t.Batch, 
		CASE t.Type 
			WHEN 30 THEN CONVERT(VARCHAR(150),ISNULL(g.ref,0)) 
			WHEN 31 THEN CONVERT(VARCHAR(150),ISNULL(g.ref,0))   
			WHEN 40 THEN t.strRef
			WHEN 41 THEN t.strRef
			ELSE CONVERT(VARCHAR(150),ISNULL(t.Ref,0)) END AS Ref, 
		dbo.TransTypeToText(t.Type) AS TypeText,
		ISNULL(t.fDesc,'') AS fDesc, 
		ISNULL(t.Amount,0) AS Amount,
		0 AS Balance,
		CASE WHEN t.Amount > 0  
			THEN t.Amount 
			ELSE 0 END AS Debit, 
		CASE WHEN t.Amount < 0  
			THEN (t.Amount * -1) 
			ELSE 0 END AS Credit,
		(CASE 
			WHEN t.Type = 20 THEN (SELECT Rol.Name FROM Vendor LEFT JOIN Rol ON Vendor.Rol = Rol.ID WHERE Vendor.ID = (SELECT TOP 1 AcctSub FROM Trans WHERE Batch = t.Batch AND Type = 21))
			WHEN t.Type = 21 THEN r.Name
			WHEN t.Type = 30 THEN lj.Tag
			WHEN t.Type = 31 THEN lj.Tag
			WHEN t.Type = 40 THEN r.Name
			WHEN t.Type = 41 THEN (SELECT Rol.Name FROM Vendor LEFT JOIN Rol ON Vendor.Rol = Rol.ID WHERE Vendor.ID = (SELECT TOP 1 AcctSub FROM Trans WHERE Batch = t.Batch AND Type = 40))		
		ELSE li.Tag END) AS Name
	FROM  Chart c 
		INNER JOIN Trans t ON c.ID=t.Acct	
		LEFT JOIN GLA g ON g.Batch = t.Batch
		LEFT JOIN Vendor v ON v.ID = t.AcctSub AND (t.Type = 40 OR t.Type = 21)
		LEFT JOIN Rol AS r ON v.Rol = r.ID   
		LEFT JOIN Invoice i ON i.Ref = t.Ref AND (t.Type = 1 OR t.Type = 2 OR t.Type = 3 OR t.Type = 98 OR t.Type = 99)
		LEFT JOIN Loc li ON li.Loc = i.Loc
		LEFT JOIN Job AS j on j.ID = t.VInt AND (t.Type = 30 OR t.Type = 31)
        LEFT JOIN Loc AS lj on lj.Loc = j.Loc 
	WHERE t.fDate >= @sdate AND t.fdate <= @edate AND (@cid = 0 OR t.Acct = @cid)
) AS tranTemp ON acctTemp.ID = tranTemp.ID

WHERE (acctTemp.Beginning <> 0 OR acctTemp.Activity <> 0 OR acctTemp.Ending <> 0 )
ORDER BY acctTemp.AcctNo

END