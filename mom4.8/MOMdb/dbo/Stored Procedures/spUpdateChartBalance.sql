
CREATE PROCEDURE [dbo].[spUpdateChartBalance] (
	 @Acct      INT
    ,@Amount  NUMERIC(30,2)
	)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION

    DECLARE @CurrentBalance NUMERIC(30,2)
	DECLARE @TotalAmount NUMERIC(30,2)
	DECLARE @PrevTimeStamp TimeStamp
	DECLARE @CurrTimeStamp TimeStamp

	SELECT @CurrentBalance=Balance, @PrevTimeStamp=[TimeStamp] FROM Chart WHERE ID=@Acct;


	IF @Amount < 0
	BEGIN
		SET @TotalAmount = @CurrentBalance + (@Amount * -1)
	END
	ELSE
	BEGIN
		SET @TotalAmount = @CurrentBalance - @Amount
	END
		UPDATE Chart SET Balance = @TotalAmount WHERE ID=@Acct

	--SELECT @CurrTimeStamp=Balance FROM Chart WHERE ID=@Acct;

	--IF(@CurrTimeStamp = @PrevTimeStamp)
	--BEGIN
	
	--	UPDATE Chart SET Balance = @TotalAmount WHERE ID=@Acct

	--END
	--ELSE
	--BEGIN

	--	SELECT @CurrentBalance = Balance FROM Chart WHERE ID=@Acct;

	--	IF @Amount < 0
	--	BEGIN
	--		SET @TotalAmount = @CurrentBalance + (@Amount * -1)
	--	END
	--	ELSE
	--	BEGIN
	--		SET @TotalAmount = @CurrentBalance - @Amount
	--	END

	--	UPDATE Chart SET Balance = @TotalAmount WHERE ID=@Acct

	--END

	COMMIT TRANSACTION
	
END
