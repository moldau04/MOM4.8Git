CREATE FUNCTION [dbo].[GetOnBalancecount](@InvID int,@EN int,@UserID int)

 

RETURNS numeric(30, 2)
AS
BEGIN
	
	DECLARE @OnBalancecount numeric(30, 2)

	if(@EN=1)
	begin
	set @OnBalancecount = (select Sum(Balance) As hand from IWarehouseLocAdj IW

	inner join Warehouse ww on ww.ID=IW.WareHouseID
	left outer join tblUserCo UC on UC.CompanyID = ww.EN 
	where IW.InvID = @InvID and UC.IsSel=1 and Uc.UserID=@UserID)
	end
	else
	begin
	set @OnBalancecount = (select Sum(Balance) As hand from IWarehouseLocAdj where InvID = @InvID)
	end
	if(@OnBalancecount is null )
	Begin
	set @OnBalancecount=0.00
	End

	RETURN @OnBalancecount

END
