
/*--------------------------------------------------------------------
Created By: Thomas
Created On: 26 Feb 2019	
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spGetEstimateEquipmentsByGroupId]
	@GroupId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT eg.Id GroupId, eg.GroupName, egq.EquipmentId FROM tblEstimateGroup eg
	LEFT JOIN tblEstimateGroupEquipment egq ON eg.Id = egq.GroupId
	WHERE eg.Id = @GroupId
END
