CREATE PROCEDURE [dbo].[spInsertInInvWHTrans] 
	-- Add the parameters for the stored procedure here
	    @InvID int,
        @WarehouseID varchar(50),
        @LocationID int,
        @Hand numeric(30,2),
        @Balance numeric(30,2),
        @fOrder numeric(30,2), 
        @Committed numeric(30,2),
        @Available numeric(30,2),
        @Screen nvarchar(50),
        @ScreenID int,
        @Mode nvarchar(50),
        @TransType nvarchar(50),
		@Batch int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here

	--If(@Mode == 'Add')
	--BEGIN

	IF @Screen= 'APBILL' AND @TransType = 'Revert' AND @Mode = 'Add' AND @fOrder < 0
	BEGIN
		
		SELECT @fOrder = isnull(sum(isnull(Adj.fOrder,0)),0)  FROM tblInventoryWHTrans   adj  WHERE adj.InvID=@InvID
		SET   @fOrder = @fOrder *-1

	END

		 INSERT INTO tblInventoryWHTrans
		 (InvID ,WarehouseID,LocationID,Hand,Balance,fOrder,Committed,Available,Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
		 VALUES
		 (@InvID ,@WarehouseID,@LocationID,@Hand,@Balance,@fOrder,@Committed,@Available,@Screen,@ScreenID,@Mode,Getdate(),@TransType,@Batch,GETDATE())
	 --END
	 exec CalculateInventory
END
GO
