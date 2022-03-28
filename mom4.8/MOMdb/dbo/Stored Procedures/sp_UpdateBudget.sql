CREATE PROCEDURE [dbo].[Budget_Update]
(
	@BudgetID	    INT = NULL OUTPUT, 
    @Budget			VARCHAR(50),
	@Year			INT = Null
)
AS
BEGIN
	  SET NOCOUNT ON;
	  UPDATE Budget SET Budget = @Budget WHERE BudgetID = @BudgetID
	
END
