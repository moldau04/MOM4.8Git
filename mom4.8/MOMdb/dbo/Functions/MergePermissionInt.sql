CREATE FUNCTION [dbo].[MergePermissionInt](@int1 int, @int2 int)    
RETURNS int
AS
BEGIN
	DECLARE @retVal int
	if (@int1 = 0) 
	BEGIN
		Set @retVal = @int2
	END
    else 
	BEGIN
		Set @retVal = @int1
	END

	Return @retVal 
END
