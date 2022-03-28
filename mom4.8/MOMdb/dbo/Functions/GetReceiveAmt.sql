CREATE FUNCTION [dbo].[GetReceiveAmt](@RID int)

 

RETURNS int
AS
BEGIN
	
	DECLARE @Value Decimal
	

	set @Value=(select sum( Amount) as OnOrder from RPOItem
				where ReceivePO=@RID 
				group by ReceivePO)

	if(@Value is null )
	Begin
	set @Value=0.00
	End

	RETURN @Value

END

