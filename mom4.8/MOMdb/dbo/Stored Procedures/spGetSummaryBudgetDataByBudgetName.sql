CREATE PROCEDURE [dbo].[spGetSummaryBudgetDataByBudgetName] 
	@StartDate DateTime,
	@EndDate DateTime,
	@BudgetName VARCHAR(50) 
AS 
BEGIN 
SET NOCOUNT ON;
DECLARE 
 @StartPeriod int,
 @EndPeriod int

SET @StartPeriod = YEAR(@StartDate) * 100 + MONTH(@StartDate)
SET @EndPeriod = YEAR(@EndDate) * 100 + MONTH(@EndDate)

BEGIN 
	SELECT 
		ch.ID AS Acct, 
		Act.Acct AS AcctNo,
		Act.Acct + '  ' + ch.fDesc AS fDesc, 
		CASE Act.Type 
			WHEN 'Revenues' THEN 3 
			WHEN 'Cost of Sales' THEN 4 
			WHEN 'Expenses' THEN 5 END
		AS Type, 
		Act.Status As Status,
		Act.Type AS TypeName, 
		Act.Type AS Sub, 
		SUM(ActD.Amount) AS NTotal, 
		100 As OrderID 
	FROM Account Act
		INNER JOIN Chart ch ON ch.Acct = Act.Acct 
		INNER JOIN AccountDetails ActD ON Act.AccountID = ActD.AccountID 
		INNER JOIN Budget B ON B.BudgetID = ActD.BudgetID 
	WHERE B.Budget = @BudgetName
		AND ActD.Period >= @StartPeriod AND ActD.Period <= @EndPeriod
	GROUP BY ch.ID, Act.Acct, ch.fDesc, Act.Type, Act.Status
	ORDER BY Act.Acct
  END 
END