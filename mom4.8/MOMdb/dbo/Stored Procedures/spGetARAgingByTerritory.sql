CREATE PROCEDURE [dbo].[spGetARAgingByTerritory]
	@fDate DateTime,
	@Territories Varchar(50),
	@CreditFlag TinyInt = 0
AS
BEGIN
	DECLARE @filterfDate Datetime
	DECLARE @filterTerrType VARCHAR(500)
	SET @filterfDate=@fDate
	DECLARE @acct INT
	SET @acct=(SELECT TOP 1 ID FROM Chart WHERE DefaultNo='D1200' AND Status=0 ORDER BY ID) 

	DECLARE @countdata INT
	DECLARE @countTerrInput INT
	SET @countdata= (SELECT count(*) FROM Terr)
	SET @countTerrInput= (SELECT count(*) FROM [dbo].[fnSplit](@Territories,','))

	IF (@countdata= @countTerrInput)
	BEGIN
		SET @filterTerrType =''
	END
	ELSE
	BEGIN
		SET @filterTerrType =@Territories
	END


	IF OBJECT_ID('tempdb..#tempInvoice') IS NOT NULL DROP TABLE #tempInvoice
	CREATE TABLE #tempInvoice(
		TransID         INT , 	
		Type			INT,
		AcctSub			INT,
		fDate			Datetime,
		Original		NUMERIC (30, 2),
		Total			NUMERIC (30, 2),
		Paid			NUMERIC (30, 2),
		fDesc			varchar(max),
		REF				INT,	
		Status			varchar(200)
		)
	INSERT INTO #tempInvoice(
		TransID,
		Type,
		AcctSub,
		fDate,	
		Original,
		Total,
		Paid,
		fDesc,
		Ref,	
		Status
		)
	SELECT 
		t.ID,
		t.Type,
		t.AcctSub,
		t.fDate,	
		ISNULL(t.Amount, 0) AS Original,
		0 AS Total,
		0 AS Paid,
		t.fDesc,
		t.Ref,	
		t.Status 
	FROM Trans t 	
		LEFT JOIN loc l  WITH (NOLOCK) ON l.loc=t.AcctSub   
		LEFT JOIN (SELECT AcctSub FROM Trans tsub WHERE Acct=@acct AND tsub.AcctSub is not null  AND tsub.fdate<=@filterfDate GROUP BY tsub.AcctSub HAVING sum(tsub.Amount)<>0) t2 ON t2.AcctSub=t.AcctSub
		LEFT JOIN Terr te ON te.ID = l.Terr
	WHERE 
		t.Acct=@acct 
		AND t.AcctSub is not null 
		AND t.fdate<=@filterfDate
		AND t2.AcctSub is not null
		AND (  @filterTerrType = '' or te.ID in( SELECT SplitValue FROM [dbo].[fnSplit](@filterTerrType ,','))) 

	-- Delete Total Service data
	DELETE FROM #tempInvoice
	WHERE Status IN
       (SELECT Status
        FROM #tempInvoice
        WHERE (Status IS NOT NULL AND status <>'')
        GROUP BY Status
        HAVING sum(Original)=0)

	--Update Paid value
	UPDATE #tempInvoice
	SET Paid = ISNULL((SELECT SUM(ISNULL(Original,0)) * -1 FROM #tempInvoice tsub WHERE tsub.Ref = #tempInvoice.Ref AND tsub.Type = 99 AND tsub.AcctSub = #tempInvoice.AcctSub), 0)
	WHERE type=1

	DELETE FROM #tempInvoice
	WHERE type=99 AND REF in(SELECT REF FROM #tempInvoice tsub WHERE tsub.Type=1 AND tsub.AcctSub=#tempInvoice.AcctSub)

	--Update Total value
	UPDATE #tempInvoice
	SET Total = Original -ISNULL(Paid,0)

	DELETE w
	FROM #tempInvoice w
	INNER JOIN ((SELECT tsub.AcctSub FROM #tempInvoice tsub WHERE ISNULL(tsub.Status, '') <> '' GROUP BY tsub.AcctSub HAVING SUM(tsub.Total)=0)) r2 ON r2.AcctSub=w.AcctSub
	WHERE  ISNULL(w.Status, '') <> ''
 
	DELETE  FROM #tempInvoice
	WHERE Original=0

	UPDATE  #tempInvoice
	SET Ref = ISNULL((SELECT top 1  PaymentDetails.InvoiceID FROM PaymentDetails WHERE PaymentDetails.IsInvoice <> 1 AND PaymentDetails.TransID = #tempInvoice.TransID - 1), #tempInvoice.REf)
	WHERE #tempInvoice.Type=99 AND (SELECT count(1) FROM OpenAR WHERE OpenAR.Ref=#tempInvoice.REF AND type = 2) = 0

	DELETE  FROM #tempInvoice
	WHERE Type = 99 AND Ref in (SELECT tm.Ref FROM #tempInvoice tm WHERE tm.type = 99 GROUP BY tm.AcctSub, tm.Ref HAVING SUM(tm.Original) = 0)

	-- Update data for Data Range

	SELECT 
		t.TransID,
		te.Name AS Salesperson,
		te2.Name AS DefaultSalesperson,
		t.Type,
		ISNULL(r.Name,'') AS CID,
		ISNULL(r.Name,'') AS CustomerName , '' AS Custom1, 
		l.Loc,
		l.ID +' - '+ l.Tag AS LocID,
		l.Tag AS LocName,
		l.Type AS LocType,
		t.fDate,
		ISNULL(i.DDate, dbo.GetDueDate(t.fDate, ISNULL(i.Terms, 0))) AS Due
		,Original,t.Total AS Total,Paid,t.fDesc,
		t.Ref,
		CASE
			WHEN ISNULL(DATEDIFF(DAY, ISNULL(i.fDate, dbo.GetDueDate(t.fDate, ISNULL(i.Terms, 0))), @filterfDate), 0) < 0 THEN 0
			ELSE ISNULL(DATEDIFF(DAY, ISNULL(i.fDate, dbo.GetDueDate(t.fDate, ISNULL(i.Terms, 0))), @filterfDate), 0)
		END AS DueIn
		,CASE
			WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) <=1) THEN t.Total
			ELSE 0
		END  AS CurrentDay,
		CASE
			WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) >= 0)
				AND (DATEDIFF(DAY, t.fDate, @filterfDate) <=30) THEN t.Total
			ELSE 0
		END AS ThirtyDay
		,CASE
			WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) > 30)
				AND (DATEDIFF(DAY, t.fDate, @filterfDate) <=60) THEN t.Total
			ELSE 0
		END AS SixtyDay
		,CASE
			WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) >= 61) THEN t.Total
			ELSE 0
		END AS SixtyOneDay
		,CASE
			WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) >60)
				AND (DATEDIFF(DAY, t.fDate, @filterfDate) <=90) THEN t.Total
			ELSE 0
		END AS NintyDay
		,CASE
			WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) >90)
				AND (DATEDIFF(DAY, t.fDate, @filterfDate)<=120) THEN t.Total
			ELSE 0
			END AS OverNintyDay
		,CASE
			WHEN  DATEDIFF(DAY, t.fDate, @filterfDate) >120 THEN t.Total
			ELSE 0
			END AS OverOneTwentyDay
		,CASE
			WHEN (DATEDIFF(DAY, t.fDate, @filterfDate) >90)
				AND (DATEDIFF(DAY, t.fDate, @filterfDate)<=120) THEN t.Total
			ELSE 0
		END AS OnetwentyDay				
		,t.Status 
	FROM #tempInvoice t
		LEFT JOIN loc l WITH (NOLOCK) ON l.loc=t.AcctSub   
		LEFT JOIN owner ow WITH (NOLOCK) ON ow.id = l.owner  
		LEFT JOIN Terr te ON te.ID = l.Terr
		LEFT JOIN rol r WITH (NOLOCK)   ON ow.rol = r.id    
		LEFT JOIN rol lr WITH (NOLOCK)  ON l.rol = lr.id  
		LEFT JOIN Branch B ON B.ID= r.EN
		LEFT JOIN Invoice i ON i.Ref = t.Ref
		LEFT JOIN Loc l2 ON l2.Loc = i.Loc
		LEFT JOIN Terr te2 ON te2.ID = l2.Terr
	WHERE t.total <>0 
		AND (@CreditFlag = 0 OR l.CreditFlag = 1)
	ORDER BY te.Name, l.ID, t.fDate 
	
END