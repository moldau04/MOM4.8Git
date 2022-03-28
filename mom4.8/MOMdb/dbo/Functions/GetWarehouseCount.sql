CREATE FUNCTION [dbo].[GetWarehouseCount](@InvID int,@EN int,@UserID int)

 

RETURNS int
AS
BEGIN
	
	DECLARE @Warehousecount int
	if(@EN=1)
	Begin

	set @Warehousecount = (select Count(*)  from InvWarehouse Inmerge
							 inner join Warehouse ww on ww.ID=Inmerge.WareHouseID
							left outer join tblUserCo UC on UC.CompanyID = ww.EN 
	where Inmerge.InvID = @InvID and UC.IsSel=1  and UC.UserID=@UserID)
	END
	else
	Begin

	set @Warehousecount = (select Count(*)  from InvWarehouse Inmerge
							 
	where Inmerge.InvID = @InvID)
	END

	RETURN @Warehousecount

END
