CREATE PROCEDURE [dbo].[spGetEstimateGroupNameByRol]
	@Id INT
AS

SELECT eg.Id, eg.GroupName from tblEstimateGroup eg WHERE eg.RolId = @Id