CREATE PROCEDURE [dbo].[spAccountLedger] 
	@cid int,
	@sdate datetime,
	@edate datetime
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE	@runningAmount numeric(30,2) = 0
	DECLARE @cType int
	SET @cType = (SELECT Type FROM Chart WHERE ID = @cid)

	IF (@cType <> 3 AND @cType <> 4 AND @cType <> 5)
		BEGIN
			SET @runningAmount = ISNULL( (SELECT SUM(ISNULL(Amount,0)) FROM Trans WHERE Acct = @cid AND (fDate < @sdate + '00:00:00')),0)
		END

	SELECT t.ID, 
		t.Acct, 
		t.fDate, 
		CONVERT(varchar(30),t.fDate,101) AS fDates,
		t.Batch, 
		(CASE t.type 
			WHEN 30 THEN CONVERT(varchar(50),ISNULL(g.Ref,0))
			WHEN 31 THEN CONVERT(varchar(50), ISNULL(g.Ref,0))
			WHEN 40 THEN t.strRef
			WHEN 41 THEN t.strRef
			WHEN 80 THEN t.strRef
			WHEN 81 THEN t.strRef
			ELSE CONVERT(varchar(50),ISNULL(t.Ref,0))
		END) AS Ref, 
		dbo.TransTypeToText(t.type) AS TypeText,
		t.Type,
		ISNULL(c.fDesc,'') AS ChartName, 
		ISNULL(t.fDesc,'') AS fDesc, 
		ISNULL(t.Amount,0) AS Amount, 
		(SUM (t.Amount) OVER (ORDER BY  t.fDate, t.ID)) + @runningAmount AS Balance,
		(CASE WHEN t.Amount > 0  
			THEN t.Amount 
		ELSE 0 END) AS Debit, 
		(CASE WHEN t.Amount < 0  
			THEN (t.Amount * -1) 
		ELSE 0 END) AS Credit,
		(CASE WHEN (t.Type = 1 OR t.Type = 2 OR t.Type = 3)
			THEN (CASE i.Job WHEN 0 THEN '' ELSE i.Job END) 
			ELSE (CASE t.VInt WHEN 0 THEN '' ELSE t.VInt END) END) AS Job,
		(CASE 
			WHEN t.Type = 20 THEN bk.fDesc
			WHEN t.Type = 21 THEN (SELECT fDesc FROM Bank WHERE ID IN (SELECT TOP 1 AcctSub FROM Trans WHERE Batch = t.Batch AND Type = 20))
			WHEN t.Type = 40 THEN r.Name
			WHEN t.Type = 41 THEN (SELECT Rol.Name FROM Vendor LEFT JOIN Rol ON Vendor.Rol = Rol.ID WHERE Vendor.ID IN (SELECT TOP 1 AcctSub FROM Trans WHERE Batch = t.Batch AND Type = 40))
			WHEN t.Type = 1 OR t.Type = 2 OR t.Type = 3 THEN l.Tag
		ELSE c.fDesc END) AS Name,
		CASE 
			WHEN p.PO IS NOT NULL THEN p.PO
			ELSE p1.PO END AS PO,
		CASE 
			WHEN p.fDesc IS NOT NULL THEN p.fDesc
			ELSE p1.fDesc END AS Comment,
			CASE WHEN ISNULL(g.OriginalJE,0) <> 0 THEN CONVERT(varchar(50),ISNULL( g.OriginalJE,0)) ELSE NULL END AS OriginalJE			
	FROM  Chart c	
		INNER JOIN Trans t ON c.ID = t.Acct 
		LEFT JOIN GLA g ON g.Batch = t.Batch AND g.Ref = t.Ref
		LEFT JOIN Vendor v ON v.ID = t.AcctSub AND t.Type = 40 
		LEFT JOIN Rol AS r ON v.Rol = r.ID   
		LEFT JOIN Bank bk ON bk.ID = t.AcctSub AND t.Type = 20
		LEFT JOIN Invoice i ON i.Ref = t.Ref AND (t.Type = 1 OR t.Type = 2 OR t.Type = 3)
		LEFT JOIN PJ pj ON pj.Batch = t.Batch AND (t.Type = 40 OR t.Type = 41)
		LEFT JOIN PO p ON CAST(p.PO AS varchar(20)) = i.PO
		LEFT JOIN PO p1 ON pj.PO = p1.PO
		LEFT JOIN Loc l ON l.Loc = i.Loc
	WHERE c.ID=@cid AND (t.fDate >= @sdate + '00:00:00') AND ( t.fDate <= @edate + '23:59:59') 
	ORDER BY t.fDate, t.ID

	SELECT * FROM Chart WHERE ID = @cid
END
GO