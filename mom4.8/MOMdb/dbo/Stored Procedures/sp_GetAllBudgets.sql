CREATE PROCEDURE [dbo].[Budget_GetAllBudgets]
@Year INT = NULL
AS
	SET NOCOUNT ON;
	SELECT BudgetID, Budget, Year FROM Budget
	WHERE Year = ISNULL(@Year, Year)
	ORDER BY Year
RETURN