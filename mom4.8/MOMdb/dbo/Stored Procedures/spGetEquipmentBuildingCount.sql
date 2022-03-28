CREATE PROCEDURE spGetEquipmentBuildingCount	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SELECT t1.EDesc AS Building, COUNT(t2.ID) AS Total FROM [ElevatorSpec] t1 LEFT JOIN [Elev] t2 ON t2.Building = t1.Edesc WHERE t1.ECat = 2 AND t2.Status = 0 GROUP BY t1.EDesc UNION SELECT 'Unassigned' AS Building, COUNT(*) FROM [Elev] WHERE (Building IS NULL OR Building = 'None' OR Building = '') AND Status = 0
END
GO
