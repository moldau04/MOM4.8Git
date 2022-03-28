 
-- =============================================
-- Author:		thomas
-- Create date: <11 March 2019>
-- Description:	<Get Group Name>
-- =============================================
CREATE PROCEDURE spGetLocGroupNotInProj 
	 @locId int = 0,
	 @projectId int = 0,
	 @SearchValue Varchar(255)=''

AS
BEGIN 
	DECLARE @WOspacialchars Varchar(255)
	SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchValue)

	DECLARE @RolId INT
	SELECT @RolId=rol from loc where loc=@LocId

	SELECT Distinct g.GroupName label, g.Id as value FROM tblEstimateGroup g 
	left join tblProjectGroup pg on pg.GroupId = g.Id
	WHERE g.RolId = @RolId and (pg.ProjectId != @projectId or pg.ProjectId is null) and g.GroupName like '%' + @WOspacialchars + '%'
END
 
