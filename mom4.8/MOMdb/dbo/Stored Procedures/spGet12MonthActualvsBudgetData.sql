CREATE PROCEDURE [dbo].[Get12MonthActualvsBudgetData] (
  @StartDate DateTime = NULL, 
  @EndDate DateTime = NULL, 
  @BudgetName varchar(50) = NULL,
  @Centers varchar(500) = NULL
) AS BEGIN 

SET 
  NOCOUNT ON;

DECLARE @fDate DATETIME = @EndDate 
DECLARE @NDate DATETIME = DATEADD(DAY, 1, DATEADD(YEAR, -1, @fDate))
DECLARE @fYear VARCHAR(4) = CONVERT(VARCHAR(4), YEAR(@fDate))
DECLARE @Count INT = 0 
DECLARE @SqlStr VARCHAR(MAX)
 
DECLARE 
 @StartYear INT,
 @EndYear INT,
 @BudgetYear INT,
 @StartMonth INT,
 @EndMonth INT,
 @StartPeriodAsInt INT,
 @EndperiodAsInt INT

SET @StartYear = YEAR(@StartDate)
SET @EndYear = YEAR(@EndDate)
SET @StartMonth = MONTH(@StartDate)
SET @EndMonth = MONTH(@EndDate)

SET @BudgetYear = (SELECT Year FROM Budget WHERE Budget = @BudgetName)
SET @StartPeriodAsInt = @BudgetYear * 100 + @StartMonth
SET @EndperiodAsInt = @BudgetYear * 100 + @EndMonth

 CREATE TABLE #ACTUALTOTAL
(
  Acct INT, 
  AcctNo VARCHAR(50), 
  AcctName VARCHAR(150), 
  fDesc VARCHAR(150), 
  Type INT, 
  TypeName VARCHAR(50), 
  Sub VARCHAR(50), 
  NTotal NUMERIC(30, 2), 
  NMonth Char(25), 
  OrderID SMALLINT,
  Url varchar(500)
) 

CREATE TABLE #ACTUAL
(
  Acct INT, 
  AcctNo VARCHAR(50), 
  AcctName VARCHAR(150), 
  fDesc VARCHAR(150), 
  Type INT, 
  TypeName VARCHAR(50), 
  Sub VARCHAR(50), 
  NTotal NUMERIC(30, 2), 
  NMonth Char(25), 
  OrderID SMALLINT,
  Url varchar(500)
) 

CREATE TABLE #BUDGETTOTAL
(
  Acct INT, 
  AcctNo VARCHAR(50), 
  AcctName VARCHAR(150), 
  fDesc VARCHAR(150), 
  Type INT, 
  TypeName VARCHAR(50), 
  Sub VARCHAR(50), 
  NBudget NUMERIC(30, 2), 
  Period Char(25), 
  NMonth Char(25), 
  Year INT,
  Url varchar(500)
) 

INSERT INTO #ACTUAL (Acct, AcctNo, AcctName, fDesc, Type, TypeName, Sub, NTotal, NMonth, OrderID, Url)
	SELECT 
		t.Acct, 
		c.Acct AS AcctNo, 
		c.fDesc AS AcctName,
		c.Acct + '  ' + c.fDesc AS fDesc, 
		c.Type, 
		CASE c.Type 
			WHEN 0 THEN 'Asset' 
			WHEN 1 THEN 'Liability' 
			WHEN 2 THEN 'Equity' 
			WHEN 3 THEN 'Revenues' 
			WHEN 4 THEN 'Cost of Sales' 
			WHEN 5 THEN 'Expenses' 
			WHEN 6 THEN 'Bank' 
		END AS TypeName, 
		CASE c.Sub 
			WHEN '' THEN (
				CASE c.Type 
					WHEN 0 THEN 'Asset' 
					WHEN 1 THEN 'Liability' 
					WHEN 2 THEN 'Equity' 
					WHEN 3 THEN 'Revenues' 
					WHEN 4 THEN 'Cost of Sales' 
					WHEN 5 THEN 'Expenses' 
					WHEN 6 THEN 'Bank' 
				END) 
			ELSE c.Sub 
		END AS Sub, 
		CASE c.Type 
			WHEN 3 THEN (ISNULL(t.Amount, 0) * -1) 
			ELSE ISNULL(t.Amount, 0) 
		END AS NTotal, 
		DATENAME(MM, t.fDate) AS NMonth, 
		MONTH(t.fDate) AS OrderID ,
		'' AS Url
	FROM Trans t WITH (NOLOCK)
		INNER JOIN Chart c ON t.Acct = c.ID 
	WHERE c.Type IN (3, 4, 5) 
		AND t.Amount <> 0 
		AND t.fDate >= @StartDate
		AND t.fDate <= @EndDate
		AND (@Centers = '' OR  c.Department IN (SELECT SplitValue FROM [dbo].[fnSplit](@Centers, ',')))

INSERT INTO #ACTUALTOTAL (Acct, AcctNo, AcctName, fDesc, Type, TypeName, Sub, NTotal, NMonth, OrderID, Url)
	SELECT 
		Acct, 
		AcctNo, 
		AcctName, 
		fDesc, 
		Type, 
		TypeName, 
		Sub, 
		SUM(ISNULL(Ntotal, 0)) AS NTotal,
		NMonth, 
		OrderID ,
		Url
	FROM #ACTUAL 
	GROUP BY 
		Acct, 
		fDesc, 
		Type, 
		TypeName, 
		Sub, 
		NMonth, 
		OrderID, 
		AcctNo, 
		AcctName,
		Url

INSERT INTO #BUDGETTOTAL (Acct, AcctNo, AcctName, fDesc, Type, TypeName, Sub, NBudget, Period, NMonth, Year, Url)
	SELECT 
		c.ID AS Acct, 
		Act.Acct AS AcctNo, 
		Act.fDesc AS AcctName, 
		(Act.Acct + '  ' + Act.fDesc) AS fDesc, 
		CASE Act.Type 
			WHEN 'Revenues' THEN 3 
			WHEN 'Cost of Sales' 
			THEN 4 WHEN 'Expenses' THEN 5 
		END AS Type, 
		Act.Type AS TypeName, 
		Act.Type AS Sub, 
		ActD.Amount AS NBudget, 
		Period AS Period, 
		CASE SUBSTRING(CONVERT(varchar(10), Period), 5, 2)
			WHEN '01' THEN 'January' 
			WHEN '02' THEN 'February' 
			WHEN '03' THEN 'March' 
			WHEN '04' THEN 'April' 
			WHEN '05' THEN 'May' 
			WHEN '06' THEN 'June' 
			WHEN '07' THEN 'July'
			WHEN '08' THEN 'August' 
			WHEN '09' THEN 'September' 
			WHEN '10' THEN 'October' 
			WHEN '11' THEN 'November' 
			WHEN '12' THEN 'December' 
		END AS NMonth, 
		B.Year,
		'' AS Url
	FROM Account Act 
		INNER JOIN AccountDetails ActD ON Act.AccountID = ActD.AccountID 
		INNER JOIN Chart c ON Act.Acct = c.Acct
		INNER JOIN Budget B ON B.BudgetID = ActD.BudgetID 
	WHERE B.Budget = @BudgetName 
		AND ActD.Period >= @StartPeriodAsInt AND ActD.Period <= @EndperiodAsInt
		AND ActD.Amount <> 0
		AND (@Centers = '' OR  c.Department IN (SELECT SplitValue FROM [dbo].[fnSplit](@Centers, ',')))

IF @BudgetName <> '' 
  BEGIN
	SELECT 
		ISNULL(A.Acct, B.Acct) AS Acct, 
		ISNULL(A.AcctNo, B.AcctNo) AS AcctNo,
		ISNULL(A.AcctName, B.AcctName) AS AcctName,
		ISNULL(A.fDesc, B.fDesc) AS fDesc,
		ISNULL(A.Type, B.Type) AS Type,
		ISNULL(A.TypeName, B.TypeName) AS TypeName, 
		B.Period,
		ISNULL(A.NMonth, B.NMonth) AS NMonth, 
		'' AS Url,
		ISNULL(A.NTotal, 0) AS NTotal, 
		ISNULL(B.NBudget, 0) AS NBudget, 
		ISNULL(A.NTotal, 0) - ISNULL(B.NBudget, 0) AS Difference, 
		CASE 
			WHEN ISNULL(B.NBudget, 0) = 0 THEN 0 
			ELSE (ISNULL(A.NTotal, 0) - ISNULL(B.NBudget, 0)) / Abs(B.NBudget)
		END AS Variance
	FROM #ACTUALTOTAL A
		FULL OUTER JOIN #BUDGETTOTAL B ON B.AcctNo = A.AcctNo AND A.NMonth = B.NMonth 
	ORDER BY A.fDesc, A.Type, B.Type 
  END
ELSE 
  BEGIN
	SELECT 
		A.Acct, 
		A.AcctNo, 
		A.AcctName, 
		A.fDesc, 
		A.Type, 
		A.TypeName,
		A.Url,
		CASE (
		A.NMonth
		) WHEN 'January' THEN @fYear+'01' WHEN 'February' THEN @fYear+'02' WHEN 'March' THEN @fYear+'03' WHEN 'April' THEN @fYear+'04' WHEN 'May' THEN @fYear+'05' WHEN 'June' THEN @fYear+'06' WHEN 'July' THEN @fYear+'07' WHEN 'August' THEN @fYear+'08' WHEN 'September' THEN @fYear+'09' WHEN 'October' THEN @fYear+'10' WHEN 'November' THEN @fYear+'11' WHEN 'December' THEN @fYear+'12' END AS Period, 
		A.NMonth,
		A.NTotal, 
		A.NTotal AS NBudget, 
		0 AS Difference, 
		0 AS Variance, 
		A.NMonth 
	FROM #ACTUALTOTAL A
  END
END
