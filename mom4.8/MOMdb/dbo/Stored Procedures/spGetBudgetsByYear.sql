CREATE PROCEDURE [dbo].[Budget_GetBudgetsByDate]
@FromYear INT = NULL,
@ToYear INT = NULL
AS
	SET NOCOUNT ON;
	SELECT BudgetID, Budget, Year FROM Budget
	WHERE Year >= ISNULL(@FromYear, Year) and Year <= ISNULL(@ToYear + 1, Year)
RETURN