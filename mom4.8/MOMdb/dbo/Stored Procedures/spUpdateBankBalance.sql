
CREATE PROCEDURE [dbo].[spUpdateBankBalance](
	 @Acct      INT
    ,@Amount  NUMERIC(30,2)
	)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CurrentBalance NUMERIC(30,2)
	DECLARE @TotalAmount NUMERIC(30,2)

	SELECT @CurrentBalance=Balance FROM Bank WHERE Chart=@Acct;


	IF @Amount < 0
	BEGIN
		SET @TotalAmount = @CurrentBalance - (@Amount * -1)
	END
	ELSE
	BEGIN
		SET @TotalAmount = @CurrentBalance + @Amount
	END
	
	UPDATE Bank SET Balance = @TotalAmount WHERE Chart=@Acct
	
END
