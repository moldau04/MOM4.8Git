
CREATE PROCEDURE [dbo].[spGetBillingCodeSearch]
	@SearchText VARCHAR(50)
AS
BEGIN
	--DECLARE @WOspacialchars VARCHAR(50) 
	--SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)

	SET NOCOUNT ON;

	 SELECT ID, Name from [dbo].[Inv] WHERE InUse = 0 AND (dbo.RemoveSpecialChars(Name) LIKE '%'+@SearchText+'%') 
	 ORDER BY ID

END
