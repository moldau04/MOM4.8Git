CREATE PROCEDURE [dbo].[spGetAllViolationCode] 
AS 
BEGIN
 SELECT vc.ID AS ID, vc.Code AS Code, vc.[Description] AS [Desc], vc.SectionID AS SectionID ,section.Name AS SectionName, vc.CategoryID AS CategoryID, cat.Name AS CategoryName 
 FROM ViolationCode vc 
 LEFT JOIN ViolationSection section ON vc.SectionID=section.ID
 LEFT JOIN ViolationCategory cat ON cat.ID= vc.CategoryID

END

