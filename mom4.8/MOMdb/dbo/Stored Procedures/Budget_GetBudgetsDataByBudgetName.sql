CREATE PROCEDURE [dbo].[Budget_GetBudgetsDataByBudgetName] 
@BudgetName VARCHAR(50), 
@AccountType VARCHAR(500) 
AS 
BEGIN 
SET NOCOUNT ON;
BEGIN 
	SELECT 
	  Act.AccountID AS Acct, 
	  (Act.Acct + '  ' + Act.fDesc) AS fDesc, 
	  (
		CASE Act.Type WHEN 'Revenues' THEN 3 WHEN 'Cost of Sales' THEN 4 WHEN 'Expenses' THEN 5 END
	  ) AS Type, 
	  Act.Status As Status,
	  Act.Type AS TypeName, 
	  Act.Type AS Sub, 
	  Act.Acct AS AcctNumber,
	  Act.fDesc AS AcctName,
	  ActD.Total AS AnnualTotal,
	  ActD.Jan,
	  ActD.Feb,
	  ActD.Mar,
	  ActD.Apr,
	  ActD.May,
	  ActD.Jun,
	  ActD.Jul,
	  ActD.Aug,
	  ActD.Sep,
	  ActD.Oct,
	  ActD.Nov,
	  ActD.Dec
	FROM 
	  Account Act 
	  INNER JOIN BudgetAccountDetails ActD ON Act.AccountID = ActD.AccountID 
	  INNER JOIN Budget B ON B.BudgetID = ActD.BudgetID 
	WHERE B.Budget = @BudgetName 
	  AND (@AccountType = '' OR Act.Type IN (SELECT SplitValue FROM [dbo].[fnSplit](@AccountType, ',')))
  END 
END
GO


