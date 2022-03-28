CREATE PROCEDURE [dbo].[Budget_Delete]
(
	@BudgetID INT
)
AS
SET NOCOUNT ON
	DELETE FROM AccountDetails
	WHERE BudgetID = @BudgetID
	DELETE FROM Budget 
	WHERE BudgetID = @BudgetID
RETURN