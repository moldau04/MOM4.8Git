CREATE PROCEDURE [dbo].[spUpdateCustomerLocBalance](
	 @LocID      INT
    ,@Amount  NUMERIC(30,2)
	)
AS
BEGIN
	
	SET NOCOUNT ON;

	BEGIN TRANSACTION

	DECLARE @balance NUMERIC(30,2) = 0;
	
	SELECT @balance = ISNULL([Balance],0) FROM [dbo].[Loc] WHERE [Loc] = @LocID -- Get timestamp before calculation of location

	--SET @balance = @balance + @Amount;
	SET @balance=dbo.GetLocationBalance(@LocID)

	UPDATE [dbo].[Loc] SET [Balance] = @balance WHERE [Loc] = @LocID

	DECLARE @OwnerID INT
	SET @OwnerID=(SELECT Owner FROM Loc WHERE Loc=@LocID)
	
	UPDATE Owner
		SET 
		Owner.Balance = (SELECT Sum(Balance) FROM Loc WHERE Loc IN  (SELECT Loc FROM Loc WHERE Owner = @OwnerID))
		
		FROM Owner 
		WHERE ID = @OwnerID

	COMMIT TRANSACTION
END
