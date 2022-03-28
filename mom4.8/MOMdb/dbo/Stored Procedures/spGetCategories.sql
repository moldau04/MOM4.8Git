CREATE Procedure [dbo].[spGetCategories]
as
Begin 
SELECT Type as Category FROM Category ORDER BY Category
End
