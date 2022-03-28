CREATE FUNCTION [dbo].[GetCommittedcount](@InvID int,@EN int,@UserID int)

 

RETURNS numeric(30,2)
AS
BEGIN
	
	DECLARE @Committedcount numeric(30,2)

	--if(@EN=1)
	--Begin
	--set @Committedcount = (select Sum([Committed]) As [Committed] from IWarehouseLocAdj IW

	--inner join Warehouse ww on ww.ID=IW.WareHouseID
	--left outer join tblUserCo UC on UC.CompanyID = ww.EN 
	--where IW.InvID = @InvID and UC.IsSel=1 and Uc.UserID=@UserID)

	--end
	--else
	--begin
	--set @Committedcount = (select Sum([Committed]) As [Committed] from IWarehouseLocAdj where InvID = @InvID)
	--end

	--Set @Committedcount = (select sum(QtyRequired) AS Quantity from BOM where MatItem=@InvID group by MatItem)

	Set @Committedcount = (select sum(QtyRequired) AS Quantity from BOM b
								left join estimateI ei on ei.ID = b.EstimateIId
								left join tblEstimateConvertToProject ep on ep.EstimateID = ei.Estimate 
							where ep.EstimateID is null and MatItem=@InvID
							group by MatItem)

	if(@Committedcount is null )
	Begin
	set @Committedcount=0.00
	End

	RETURN @Committedcount

END
