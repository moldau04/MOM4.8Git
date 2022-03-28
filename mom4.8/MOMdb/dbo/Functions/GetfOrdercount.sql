CREATE FUNCTION [dbo].[GetfOrdercount](@InvID int,@EN int,@UserID int)

 

RETURNS numeric(30,2)
AS
BEGIN
	
	DECLARE @fOrdercount numeric(30,2)
	--if(@EN=1)
	--begin
	--set @fOrdercount = (select Sum(fOrder) As fOrder  from IWarehouseLocAdj IW

	--inner join Warehouse ww on ww.ID=IW.WareHouseID
	--left outer join tblUserCo UC on UC.CompanyID = ww.EN 
	--where IW.InvID = @InvID and UC.IsSel=1 and UC.UserID=@UserID)
	--End
	--else
	--begin

	--set @fOrdercount = (select Sum(fOrder) As fOrder from IWarehouseLocAdj where InvID = @InvID)
	--end

	set @fOrdercount=(select sum( POItem.BalanceQuan) as OnOrder from PO
				left outer join POItem on PO.PO=POItem.PO
				where PO.Status in (0,4) and POItem.Inv=@InvID
				group by POItem.Inv)

	if(@fOrdercount is null )
	Begin
	set @fOrdercount=0.00
	End

	RETURN @fOrdercount

END
