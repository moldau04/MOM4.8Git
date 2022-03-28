CREATE PROCEDURE [dbo].[Get12MonthActualvsBudgetGraphData] (
  @StartDate DateTime = NULL, 
  @EndDate DateTime = NULL, 
  @BudgetID INT = NULL,
  @FiscalYear INT = NULL
) AS BEGIN 
SET 
	NOCOUNT ON;
	DECLARE @Count INT = 1;

	CREATE TABLE #ACTUAL
	(
		Acct INT, 
		AcctNo VARCHAR(50), 
		AcctName VARCHAR(150), 
		fDesc VARCHAR(150), 
		Type INT, 
		TypeName VARCHAR(50), 
		NTotal NUMERIC(30, 2), 
		NMonth INT
	) 

	CREATE TABLE #ACTUALTOTAL
	(
		Acct INT, 
		AcctNo VARCHAR(50), 
		AcctName VARCHAR(150), 
		fDesc VARCHAR(150), 
		Type INT, 
		TypeName VARCHAR(50), 
		NTotal NUMERIC(30, 2), 
		NMonth INT
	) 

	CREATE TABLE #BUDGETTOTAL
	(
		Acct INT, 
		AcctNo VARCHAR(50), 
		AcctName VARCHAR(150), 
		fDesc VARCHAR(150), 
		Type INT, 
		TypeName VARCHAR(50),
		Jan NUMERIC(30, 2),
		Feb NUMERIC(30, 2),
		Mar NUMERIC(30, 2),
		Apr NUMERIC(30, 2),
		May NUMERIC(30, 2),
		Jun NUMERIC(30, 2),
		Jul NUMERIC(30, 2),
		Aug NUMERIC(30, 2),
		Sep NUMERIC(30, 2),
		Oct NUMERIC(30, 2),
		Nov NUMERIC(30, 2),
		Dec NUMERIC(30, 2),
	) 
	
	CREATE TABLE #BUDGETACTUAL
	(
		Acct INT, 
		AcctNo VARCHAR(50), 
		AcctName VARCHAR(150), 
		fDesc VARCHAR(150),
		NTotal NUMERIC(30, 2), 
		NMonth INT
	)
	
	CREATE TABLE #RESULT
	(
		NBudget NUMERIC(30, 2), 
		NTotal NUMERIC(30, 2), 
		NMonth VARCHAR(3)
	)  

	INSERT INTO #ACTUAL (Acct, AcctNo, AcctName,  fDesc, Type, TypeName, NTotal, NMonth)
	(
		SELECT 
			t.Acct, 
			c.Acct AS AcctNo, 
			c.fDesc AS AcctName, 
			c.Acct + ' ' + c.fDesc AS fDesc, 
			c.Type, 
			CASE c.Type WHEN 0 THEN 'Asset' WHEN 1 THEN 'Liability' WHEN 2 THEN 'Equity' WHEN 3 THEN 'Revenues' WHEN 4 THEN 'Cost of Sales' WHEN 5 THEN 'Expenses' WHEN 6 THEN 'Bank' END AS TypeName,  
			(
			CASE c.Type WHEN 3 THEN (
				ISNULL(t.Amount, 0) * -1
			) ELSE ISNULL(t.Amount, 0) END
			) AS NTotal, 
			MONTH(t.fDate) AS NMonth 
		FROM 
			Trans t 
			INNER JOIN Chart c ON t.Acct = c.ID
		WHERE 
			c.Type IN (3, 4, 5) 
			AND t.Amount <> 0 
			AND t.fDate >= @StartDate 
			AND t.fDate <= @EndDate
	)

	INSERT INTO #ACTUALTOTAL (Acct, AcctNo, AcctName, fDesc, Type, TypeName, NTotal, NMonth)
	(
		SELECT 
			Acct, 
			AcctNo, 
			AcctName, 
			fDesc, 
			Type, 
			TypeName, 
			(
			CASE Type WHEN 3 THEN (
				SUM(
				ISNULL(Ntotal, 0)
				)
			) ELSE SUM(
				ISNULL(Ntotal, 0)
			) * -1 END
			) AS NTotal,
			NMonth 
		FROM 
			#ACTUAL 
		GROUP BY 
			Acct, 
			AcctNo, 
			AcctName, 
			fDesc, 
			Type, 
			TypeName, 
			NMonth
	) 
  
	INSERT INTO #BUDGETTOTAL (Acct, AcctNo, AcctName, fDesc, Type, TypeName, Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec)
	(
		SELECT 
			act.AccountID AS Acct, 
			act.Acct AS AcctNo, 
			act.fDesc AS AcctName, 
			(act.Acct + ' ' + act.fDesc) AS fDesc, 
			CASE act.Type WHEN 'Revenues' THEN 3 WHEN 'Cost of Sales' THEN 4 WHEN 'Expenses' THEN 5 END AS Type,
			act.Type AS TypeName,
			bud.Jan,
			bud.Feb,
			bud.Mar,
			bud.Apr,
			bud.May,
			bud.Jun,
			bud.Jul,
			bud.Aug,
			bud.Sep,
			bud.Oct,
			bud.Nov,
			bud.Dec
		FROM 
			Account act 
			INNER JOIN BudgetAccountDetails bud ON act.AccountID = bud.AccountID 
			INNER JOIN Budget B ON B.BudgetID = bud.BudgetID 
		WHERE 
			(@BudgetID IS NULL OR B.BudgetID = @BudgetID) AND B.Year = @FiscalYear
	)

	INSERT INTO #BUDGETACTUAL (Acct, AcctNo, AcctName, fDesc, NTotal, NMonth)
	(
		SELECT
			bud.Acct, 
			bud.AcctNo,
			bud.AcctName,
			bud.fDesc,
			act.NTotal,
			act.NMonth
		FROM #BUDGETTOTAL bud
		INNER JOIN #ACTUALTOTAL act ON LTrim(RTrim(bud.fDesc)) = LTrim(RTrim(act.fDesc)) AND act.Type = 3
		WHERE
			bud.Type = 3
	)

	WHILE @Count <= 12 
	BEGIN 
		INSERT INTO #RESULT (NTotal, NBudget, NMonth)
		(
			SELECT 
				ISNULL((SELECT SUM(NTotal) FROM #BUDGETACTUAL WHERE NMonth = @Count), 0) AS NTotal,
				ISNULL(CASE @Count
					WHEN 1 THEN SUM(ISNULL(Jan, 0))
					WHEN 2 THEN SUM(ISNULL(Feb, 0))
					WHEN 3 THEN SUM(ISNULL(Mar, 0))
					WHEN 4 THEN SUM(ISNULL(Apr, 0))
					WHEN 5 THEN SUM(ISNULL(May, 0)) 
					WHEN 6 THEN SUM(ISNULL(Jun, 0))
					WHEN 7 THEN SUM(ISNULL(Jul, 0))
					WHEN 8 THEN SUM(ISNULL(Aug, 0))
					WHEN 9 THEN SUM(ISNULL(Sep, 0))
					WHEN 10 THEN SUM(ISNULL(Oct, 0))
					WHEN 11 THEN SUM(ISNULL(Nov, 0))
					WHEN 12 THEN SUM(ISNULL(Dec, 0))
				END, 0) AS NBudget,
				CASE @Count
					WHEN 1 THEN 'Jan'
					WHEN 2 THEN 'Feb'
					WHEN 3 THEN 'Mar'
					WHEN 4 THEN 'Apr'
					WHEN 5 THEN 'May' 
					WHEN 6 THEN 'Jun'
					WHEN 7 THEN 'Jul'
					WHEN 8 THEN 'Aug'
					WHEN 9 THEN 'Sep'
					WHEN 10 THEN 'Oct'
					WHEN 11 THEN 'Nov'
					WHEN 12 THEN 'Dec'
				END AS NMonth
			FROM #BUDGETTOTAL
		)
		
		SET 
		  @Count = @Count + 1 
	END

	SELECT * FROM #RESULT
END