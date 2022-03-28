CREATE PROCEDURE spGetEquipmentTypeCount 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	SELECT t1.EDesc AS TypeName, COUNT(t2.ID) AS Total FROM ElevatorSpec t1 LEFT JOIN Elev t2 ON t2.Type = t1.EDesc WHERE  t1.ECat = 1 AND t2.Status = 0 GROUP BY t1.EDesc UNION SELECT 'Unassigned' AS Building, COUNT(*) FROM [Elev] WHERE (Type IS NULL OR Type = 'None' OR Type = '') AND Status = 0
END
