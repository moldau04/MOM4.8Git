CREATE PROCEDURE [dbo].[Usp_UnitInspectedTrimNotCompleteReport]                   
AS                  
BEGIN       

	SELECT 
		j.ID AS ProjectId, 
		ro.Phone AS MainPhoneNumber, 
		ro.Cellular AS CustomerCell, 
		(SELECT TOP 1 Tag FROM Loc WHERE Loc = j.Loc) AS ProjectLocation, 
		(SELECT TOP 1 City FROM Loc WHERE Loc = j.Loc) AS ProjectCity, 
		(SELECT TOP 1 Name FROM Rol WHERE ID = (SELECT TOP 1 Rol FROM Owner WHERE ID = j.Owner)) AS CustomerName , 
		(SELECT ISNULL(Value, '') FROM ElevTItem WHERE fDesc IN ('Cab Wood' ) AND Elev = e.ID ) AS CabWood, 
		(SELECT ISNULL(Value, '') FROM ElevTItem WHERE fDesc IN ('Cab Finish') AND Elev = e.ID ) AS CabFinish, 
		vw.[Passed Inspection] AS PassedInsepction, 
		vw.[Trim Complete] AS TrimComplete,
		vw.SalesPerson
	FROM Job j 
		INNER JOIN Elev e ON e.Loc = j.loc 
		LEFT JOIN Rol ro ON ro.ID = j.Rol 
		LEFT JOIN JobT jt ON j.template=jt.ID 
		JOIN [vw_OpenJobReport] vw ON vw.[Project #] = j.ID 
	WHERE (vw.[Trim Complete] = '' OR vw.[Trim Complete] IS NULL) 
		AND vw.[Passed Inspection] != '' 
		AND vw.[Passed Inspection] IS NOT NULL
	ORDER BY j.ID DESC 
 
 END