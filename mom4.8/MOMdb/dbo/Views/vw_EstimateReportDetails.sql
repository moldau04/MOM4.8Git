CREATE VIEW [dbo].[vw_EstimateReportDetails]
	AS 
	SELECT ID,Name,  
fdesc As Description,  
remarks As Remarks,  
Job,  
(SELECT Name  
 FROM   Rol  
 WHERE  ID = e.RolID) AS Contact,  
ffor As Type,  
CASE status  
WHEN 0 THEN 'Open'  
WHEN 1 THEN 'Canceled'  
WHEN 2 THEN 'Withdrawn'  
WHEN 3 THEN 'Disqualified'  
WHEN 4 THEN 'Sold'  
WHEN 5 THEN 'Competitor'  
END                   AS Status,  
ISNULL(e.Price,0) As [Estimate Price],    
ISNULL(e.Quoted,0) As [Quoted Price]      
FROM  Estimate e  
Where  EstTemplate = 0
