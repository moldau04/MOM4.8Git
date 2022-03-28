CREATE FUNCTION TableToCSV 
(
		@table tblTypeStringToCSV  readonly
)
RETURNS varchar(max)
AS
BEGIN

DECLARE @listStr VARCHAR(MAX)
SELECT @listStr = COALESCE(@listStr+',' ,'') + Name
FROM @table

return @listStr

END
GO

