CREATE FUNCTION [dbo].[GetOnHandValue](@InvID int,@EN int,@UserID int)

 

RETURNS numeric(30,2)
AS
BEGIN
	
	DECLARE @OnHandValue numeric(30,2)
	DECLARE @Committed numeric(30,2)
	if(@EN =1)
	Begin
	SET @OnHandValue = (select Sum(Balance) As handValue from IWarehouseLocAdj IW

	inner join Warehouse ww on ww.ID=IW.WareHouseID
	left outer join tblUserCo UC on UC.CompanyID = ww.EN 
	where IW.InvID = @InvID and UC.IsSel=1 and UC.UserID=@UserID)
	end

	else
	begin

	SET @OnHandValue = (SELECT SUM(Balance) AS handValue FROM IWarehouseLocAdj WHERE InvID = @InvID)

	ENd
	IF(@OnHandValue is null  )
	BEGIN
		
		SET @OnHandValue=0.00
	
	END
	--	ELSE
	--BEGIN
	--	SET @Committed=(SELECT SUM(Committed) FROM IWarehouseLocAdj where InvID = @InvID)
	--	IF @Committed IS NULL
	--	BEGIN
	--		SET @Committed=0
	--	END
	--	SET @OnHandcount=@OnHandcount+@Committed
	--END

	RETURN @OnHandValue

END
