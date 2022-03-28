CREATE PROCEDURE [dbo].[Budget_Add]
(
	@BudgetID	    INT = NULL OUTPUT, 
    @Budget			VARCHAR(50),
	@Year			INT = Null
)
AS
BEGIN
	SET NOCOUNT ON;
	IF @BudgetID is NULL 
	BEGIN
			INSERT INTO Budget (Budget, Year) 
			VALUES (@Budget, @Year)
			SET @BudgetID = scope_identity()
	END
	
END
