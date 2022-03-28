CREATE PROCEDURE [dbo].[Budget_GetBudgetDataByBudgetName] 
@BudgetName VARCHAR(50) 
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
  ActD.Amount AS NTotal, 
  ActD.Period AS NMonth, 
  100 As OrderID 
FROM 
  Account Act 
  INNER JOIN AccountDetails ActD ON Act.AccountID = ActD.AccountID 
  INNER JOIN Budget B ON B.BudgetID = ActD.BudgetID 
  WHERE B.Budget = @BudgetName
  END 
  END

