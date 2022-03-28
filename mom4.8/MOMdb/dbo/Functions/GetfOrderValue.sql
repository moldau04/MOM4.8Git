CREATE FUNCTION [dbo].[GetfOrderValue](@InvID int)

 

RETURNS numeric(30,2)
AS
BEGIN
	
	DECLARE @fOrderValue numeric(30,2)
	

	set @fOrderValue=(select sum( POItem.Balance) as OnOrder from PO
				left outer join POItem on PO.PO=POItem.PO
				where PO.Status in (0,4) and POItem.Inv=@InvID
				group by POItem.Inv)

	if(@fOrderValue is null )
	Begin
	set @fOrderValue=0.00
	End

	RETURN @fOrderValue

END
