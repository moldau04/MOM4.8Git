CREATE FUNCTION [dbo].[GetCommittedValue](@InvID int)

 

RETURNS numeric(30,2)
AS
BEGIN
	
	DECLARE @CommittedValue Decimal
	--Set @CommittedValue = (select sum(BudgetExt) AS Quantity from BOM where MatItem=@InvID group by MatItem)
	Set @CommittedValue = (select sum(BudgetExt) AS Quantity from BOM b
								left join estimateI ei on ei.ID = b.EstimateIId
								left join tblEstimateConvertToProject ep on ep.EstimateID = ei.Estimate 
							where ep.EstimateID is null and MatItem=@InvID
							group by MatItem)

	if(@CommittedValue is null )
	Begin
		set @CommittedValue=0.00
	End

	RETURN @CommittedValue

END
