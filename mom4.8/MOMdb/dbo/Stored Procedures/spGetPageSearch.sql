CREATE PROCEDURE spGetPageSearch
	@SearchText varchar(75) = NULL
AS

declare @WOspacialchars varchar(75) 
set @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)

BEGIN
	SET NOCOUNT ON;

	DECLARE @text varchar(max)

	SET @text = '	SELECT * FROM [tblPages]	'
	IF(@SearchText != '')
	BEGIN
		SET @text += '	WHERE dbo.RemoveSpecialChars(PageName) LIKE ''%'+@WOspacialchars+'%''	'
	END
	
	exec(@text)
END
