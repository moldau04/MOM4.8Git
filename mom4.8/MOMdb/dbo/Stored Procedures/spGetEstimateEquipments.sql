
/*--------------------------------------------------------------------
Created By: Thomas
Created On: 26 Feb 2019	
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spGetEstimateEquipments]
	@EstimateNo INT
AS
BEGIN
	SET NOCOUNT ON;
	--SELECT * FROM EstimateGroupEquipments WHERE EstimateID = @EstimateNo
	SELECT e.GroupId, e.ID EstimateId, eg.GroupName, egq.EquipmentId FROM Estimate e
	LEFT JOIN tblEstimateGroup eg ON e.GroupId = eg.Id
	LEFT JOIN tblEstimateGroupEquipment egq ON eg.Id = egq.GroupId
	WHERE e.Id = @EstimateNo
END
