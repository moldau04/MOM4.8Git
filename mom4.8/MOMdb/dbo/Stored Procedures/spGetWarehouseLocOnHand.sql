CREATE PROCEDURE [dbo].[spGetWarehouseLocOnHand]
	@WarehouseID varchar(5),
	@InvID int,
	@locationID int

AS

BEGIN
	
	SET NOCOUNT ON;
		
		--if not exists(select 1 from IWarehouseLocAdj where InvID=@InvID and WareHouseID=@WarehouseID and LocationID= @locationID)
		--	begin
		--		if not exists(select 1 from IWarehouseLocAdj where InvID=@InvID and WareHouseID=@WarehouseID and LocationID IS NULL )
		--			begin	
		--				Select TOP 1 0.00 As ID,0.00 As Hand,0.00 As Balance 
		--			end
		--		else
		--			begin
		--				Select   ID,Hand, Balance from IWarehouseLocAdj where  InvID=@InvID and WareHouseID=@WarehouseID 
		--			end
		--	end 
			
		--else
		--	begin
		--		Select   ID,Hand, Balance from IWarehouseLocAdj where InvID=@InvID and WareHouseID=@WarehouseID and LocationID= @locationID
		--	end
	EXEC CalculateInventory
	if not exists(select 1 from tblInventoryWHTrans where InvID=@InvID and WareHouseID=@WarehouseID and LocationID= @locationID)
	BEGIN		
		Select @InvID As ID,0.00 As Hand,0.00 As Balance 
	END
	ELSE
	BEGIN
		SELECT InvID AS ID,SUM(ISNULL(Hand,0)) as Hand,SUM(ISNULL(Balance,0)) Balance FROM tblInventoryWHTrans WHERE InvID = @InvID and WareHouseID=@WarehouseID and LocationID= 0 GROUP BY InvID
	END

	
	
END
