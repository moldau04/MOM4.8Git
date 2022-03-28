CREATE FUNCTION [dbo].[GetIssuesToOpenJobcount](@InvID int)

 

RETURNS numeric(30,2)
AS
BEGIN
	
	DECLARE @IssuesToOpenJobcount numeric(30,2)
	

	set @IssuesToOpenJobcount=(select sum(POItem.Quan) as Comitted from PO
				inner join POItem on PO.PO=POItem.PO
				inner join Job on POItem.Job=Job.ID
				where Job.Status=0 and POItem.Inv=@InvID)

	if(@IssuesToOpenJobcount is null )
	Begin
	set @IssuesToOpenJobcount=0.00
	End

	RETURN @IssuesToOpenJobcount

END
