CREATE PROCEDURE  [dbo].[spGetBudgetVSActualData]
(
	@StartDate DateTime,
	@EndDate DateTime,
	@BudgetName varchar(50),
	@IncludeZero bit
 )
AS

DECLARE 
	@StartYear varchar(50),
	@EndYear varchar(50),
	@BudgetYear varchar(50),
	@StartMonth varchar(50),
	@EndMonth varchar(50),
	@StartPeriodAsInt int,
	@EndperiodAsInt int,

	@StartMonthDOM int,
	@EndMonthDOM int,
	@StartMonthDays int,
	@EndMonthDays int

SET @StartYear = year(@StartDate)
SET @EndYear = year(@EndDate)
SET @StartMonth = month(@StartDate)
SET @EndMonth = month(@EndDate)

SET @BudgetYear = (SELECT Year FROM Budget WHERE Budget = @BudgetName)
SET @StartPeriodAsInt = @StartYear * 100 + @StartMonth
SET @EndperiodAsInt = @EndYear * 100 + @EndMonth

SET @StartMonthDOM = DAY(EOMONTH(@StartDate))
SET @EndMonthDOM = DAY(EOMONTH(@EndDate))
SET @StartMonthDays = DAY(EOMONTH(@StartDate)) - DAY(@StartDate) + 1
SET @EndMonthDays = DAY(@EndDate)

CREATE TABLE #ACTUAL
(
	Acct INT,
	AcctNo VARCHAR(50),
	AcctName VARCHAR(150),
	fDesc VARCHAR(150),
	Type INT,
	TypeName VARCHAR(50),
	Sub VARCHAR(50),
	NTotal NUMERIC(30,2),
	NMonth Char(25),
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
	NBudget NUMERIC(30,2),
	NMonth Char(25),
	Url varchar(500)
)

INSERT INTO #ACTUAL (Acct, AcctNo, AcctName, fDesc, Type, TypeName, Sub, NTotal, NMonth, Url)
	SELECT  
		c.ID AS Acct, 
		c.Acct AS AcctNo,
		c.fDesc AS AcctName,
		c.Acct+'  '+c.fDesc	AS fDesc, 
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
			WHEN '' THEN          
				(CASE c.Type WHEN 0 THEN 'Asset'        
					WHEN 1 THEN 'Liability'       
					WHEN 2 THEN 'Equity'          
					WHEN 3 THEN 'Revenues'         
					WHEN 4 THEN 'Cost of Sales'   
					WHEN 5 THEN 'Expenses'         
					WHEN 6 THEN 'Bank'            
				END)            
		ELSE c.Sub END AS Sub, 
		CASE c.Type 
			WHEN 3 THEN (ISNULL(SUM(t.Amount),0) * -1)
			ELSE ISNULL(SUM(t.Amount),0)
		END AS NTotal,
		'Total' AS NMonth,
		'' AS Url
	FROM Chart c 
		LEFT OUTER JOIN Trans t ON t.Acct = c.ID AND t.fDate >= @StartDate AND t.fDate <= @EndDate
	WHERE c.Type IN (3, 4, 5) 
		AND (c.Status = 0 OR t.Amount <> 0) 		
	GROUP BY c.ID, c.Acct, c.fDesc, c.Type, c.Sub
	ORDER BY c.Type

INSERT INTO #BUDGETTOTAL (Acct, AcctNo, AcctName, fDesc, Type, TypeName, Sub, NBudget, NMonth, Url)
	SELECT DISTINCT
		Act.AccountID AS Acct, 
		Act.Acct AS AcctNo,
		Act.fDesc AS AcctName,
		Act.Acct + '  ' + Act.fDesc AS fDesc, 
		CASE Act.Type 
			WHEN 'Revenues' THEN 3 
			WHEN 'Cost of Sales' THEN 4 
			WHEN 'Expenses' THEN 5 
		END AS Type, 
		Act.Type AS TypeName, 
		Act.Type AS Sub, 
		SUM(CASE ActD.Period 
			WHEN @StartPeriodAsInt THEN ((ISNULL(ActD.Amount,0) / @StartMonthDOM) * @StartMonthDays)
			WHEN @EndPeriodAsInt THEN ((ISNULL(ActD.Amount,0) / @EndMonthDOM) * @EndMonthDays)
			ELSE ISNULL(ActD.Amount,0) 
		END) AS NBudget, 
		'Total' AS NMonth,
		'' AS Url
	FROM 
		Account Act 
		INNER JOIN AccountDetails ActD ON Act.AccountID = ActD.AccountID 
		INNER JOIN Budget B ON B.BudgetID = ActD.BudgetID 
	WHERE  B.Budget = @BudgetName 
		AND ActD.Period >= @StartPeriodAsInt AND ActD.Period <= @EndperiodAsInt
	GROUP BY  fDesc, Act.AccountID, Act.Acct, Act.Type, Sub
	ORDER BY Act.Type

SELECT A.Acct, 
	A.AcctNo, 
	A.AcctName, 
	A.fDesc,	
	A.Type, 
	A.TypeName,	
	A.Sub,	
	ISNULL(A.NTotal, 0) AS NTotal, 
	ISNULL(B.NBudget, 0) AS NBudget,  
	ISNULL(A.NTotal, 0) - ISNULL(B.NBudget, 0) AS Difference, 
	(CASE WHEN ISNULL(B.NBudget, 0) = 0 then 0.00 ELSE (A.NTotal - B.NBudget) / Abs(B.NBudget)END) As Variance, 
	'' As Url 
FROM  #ACTUAL A 
	LEFT OUTER JOIN #BUDGETTOTAL B ON B.AcctNo = A.AcctNo
WHERE @IncludeZero = 1 OR A.NTotal <> 0 OR B.NBudget <> 0
ORDER BY A.AcctNo,A.Type, B.Type