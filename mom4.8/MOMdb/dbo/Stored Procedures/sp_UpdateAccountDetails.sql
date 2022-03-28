CREATE PROCEDURE [dbo].[AccountDetails_Update]
(	
	@AccountDetailID INT = NULL OUTPUT,
	@AccountID		INT	= NULL OUTPUT,
	@BudgetID       INT = NULL OUTPUT,
	@Period			INT,
	@Credit			DECIMAL(30,2),
	@Debit			DECIMAL(30,2),
	@Amount			DECIMAL(30,2)
)
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS(SELECT * FROM AccountDetails WHERE AccountID = @AccountID AND BudgetID = @BudgetID AND Period = @Period)
	BEGIN
			INSERT INTO AccountDetails(AccountID,BudgetID,Period,Credit,Debit,Amount) 
			VALUES (@AccountID,@BudgetID,@Period,@Credit,@Debit,@Amount)
			SET @AccountDetailID = scope_identity()
	END
	ELSE
	BEGIN
	  SET @AccountDetailID = (SELECT AccountDetailID from AccountDetails WHERE AccountID = @AccountID AND BudgetID = @BudgetID AND Period = @Period)
	  UPDATE AccountDetails
	  SET Credit=@Credit, Debit=@Debit, Amount = @Amount
	  WHERE AccountDetailID=@AccountDetailID and AccountID = @AccountID and Period = @Period
	END
END
