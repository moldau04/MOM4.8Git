CREATE PROCEDURE [dbo].[spGetVendorSearchProject] 
		@SearchText VARCHAR(50)
AS
	DECLARE @WOspacialchars VARCHAR(50) 
	SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)
BEGIN
	
SET NOCOUNT ON;
	IF @SearchText = ''
		BEGIN
				 SELECT DISTINCT TOP 100 v.ID As ID ,r.Name As Name, v.Terms
				 FROM [dbo].[Vendor] v, [dbo].[Rol] r 
				 WHERE v.Rol=r.ID AND 
				 (Status = 0 OR Status=2)
				 ORDER BY r.Name
		END
	ELSE
		BEGIN
				 SELECT DISTINCT TOP 100 v.ID As ID ,r.Name As Name, v.Terms
				 FROM [dbo].[Vendor] v, [dbo].[Rol] r 
				 WHERE v.Rol=r.ID AND 
				 (Status = 0 OR Status=2) AND (dbo.RemoveSpecialChars(r.Name) LIKE '%'+@WOspacialchars+'%') 
				 ORDER BY r.Name
		END
	

END