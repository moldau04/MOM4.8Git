CREATE FUNCTION [dbo].[GetReceiveQuan](@RID int)

 

RETURNS int
AS
BEGIN
	
	DECLARE @Value Decimal
	

	set @Value=(select sum( Quan) as Quan from RPOItem
				where ReceivePO=@RID 
				group by ReceivePO)

	if(@Value is null )
	Begin
	set @Value=0.00
	End

	RETURN @Value

END


