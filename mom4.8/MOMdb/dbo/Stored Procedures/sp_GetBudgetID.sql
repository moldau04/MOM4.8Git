CREATE PROCEDURE [dbo].[Budget_GetBudgetID]
(
	@BudgetID	    INT = NULL OUTPUT, 
    @Budget			VARCHAR(50)
)
AS
BEGIN
	SET @BudgetID = (SELECT BudgetID from Budget WHERE Budget = @Budget)
END