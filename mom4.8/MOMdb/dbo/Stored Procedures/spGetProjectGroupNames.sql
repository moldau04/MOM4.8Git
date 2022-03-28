CREATE PROCEDURE [dbo].[spGetProjectGroupNames]
	@ProjectId int = 0
AS
	SELECT eg.GroupName, eg.Id FROM tblProjectGroup pg INNER JOIN tblEstimateGroup eg ON eg.Id = pg.GroupId
	WHERE pg.ProjectId = @ProjectId

