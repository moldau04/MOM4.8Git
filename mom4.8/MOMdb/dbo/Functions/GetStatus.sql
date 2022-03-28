CREATE FUNCTION [dbo].[GetStatus]()
RETURNS int
AS
BEGIN
	
	DECLARE @Status int

	SELECT @Status = (max(convert(int,Status))+1) FROM trans WHERE Type = 99

	RETURN @Status

END