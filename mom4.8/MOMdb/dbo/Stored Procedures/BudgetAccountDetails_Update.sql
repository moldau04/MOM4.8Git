
CREATE PROCEDURE [dbo].[BudgetAccountDetails_Update]
(	
	@AccountDetailID INT = NULL OUTPUT,
	@AccountID		INT	= NULL OUTPUT,
	@BudgetID       INT = NULL OUTPUT,
	@Period			INT = NULL,
	@Total		    DECIMAL(18,10),
	@Jan			DECIMAL(18,10),
	@Feb			DECIMAL(18,10),
	@Mar			DECIMAL(18,10),
	@Apr			DECIMAL(18,10),
	@May			DECIMAL(18,10),
	@Jun			DECIMAL(18,10),
	@Jul			DECIMAL(18,10),
	@Aug			DECIMAL(18,10),
	@Sep			DECIMAL(18,10),
	@Oct			DECIMAL(18,10),
	@Nov			DECIMAL(18,10),
	@Dec			DECIMAL(18,10)
)
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS(SELECT * FROM BudgetAccountDetails WHERE AccountID = @AccountID AND BudgetID = @BudgetID)
	BEGIN
			INSERT INTO BudgetAccountDetails(AccountID,BudgetID,Total,Jan,Feb,Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec) 
			VALUES (@AccountID,@BudgetID,@Total,@Jan,@Feb,@Mar, @Apr, @May, @Jun, @Jul, @Aug, @Sep, @Oct, @Nov, @Dec) 
			SET @AccountDetailID = scope_identity()
	END
	ELSE
	BEGIN
	  SET @AccountDetailID = (SELECT BudgetAccountDetailID from BudgetAccountDetails WHERE AccountID = @AccountID AND BudgetID = @BudgetID )
	  UPDATE BudgetAccountDetails
	  SET Total=@Total, Jan=@Jan, Feb=@Feb, Mar=@Mar, Apr=@Apr, May=@May, Jun=@Jun, Jul=@Jul, Aug=@Aug, Sep=@Sep, Oct=@Oct, Nov=@Nov, Dec=@Dec
	  WHERE BudgetAccountDetailID=@AccountDetailID and AccountID = @AccountID 
	END
END

GO


