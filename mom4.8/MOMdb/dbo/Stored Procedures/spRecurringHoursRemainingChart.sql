CREATE PROCEDURE spRecurringHoursRemainingChart 
	-- Add the parameters for the stored procedure here
	@StartDate DATETIME,
	@EndDate DATETIME
AS
BEGIN
	-- 1 Show first bar Total Hours = Add the TicketO.EST and TicketD.Est
	SELECT SUM(ISNULL(t.Est,0)) AS Total, r.Name AS Route 
    FROM TicketO t 
	INNER JOIN Loc l ON l.Loc = t.LID 
	LEFT JOIN route r ON l.Route = r.ID   
	LEFT JOIN Category c ON t.Cat = c.Type  
    WHERE
	Edate >= @StartDate AND EDate <@EndDate
    AND c.ISDefault = 1  
    GROUP BY r.Name  
    UNION ALL   
    SELECT   
	SUM(ISNULL(t.Est,0)) AS Total, 
	r.Name AS Route  
    FROM TicketD t 
	LEFT JOIN Loc l ON t.Loc = l.Loc  
	LEFT JOIN Route r ON l.Route = r.ID  
	LEFT JOIN Category c ON t.Cat = c.Type  
    WHERE 
	Edate >= @StartDate AND EDate <@EndDate
	AND c.ISDefault = 1  
    GROUP BY r.Name 

	-- 2 Show second bar Completed = TicketDPDA.Total + TicketD.Total
	SELECT SUM(ISNULL(t.Total,0)) AS Total, r.Name AS Route   
    FROM TicketDPDA t 
	LEFT JOIN loc l ON t.Loc = l.Loc  
	LEFT JOIN Route r ON r.id = l.Route  
	LEFT JOIN Category c ON t.Cat = c.Type  
    WHERE 
	Edate >= @StartDate AND EDate <@EndDate
	AND c.ISDefault = 1  
    GROUP BY r.Name   
    UNION ALL  
    SELECT  
	SUM(ISNULL(t.Total,0)) as Total, 
	r.Name as Route  
    FROM TicketD t 
	LEFT JOIN Loc l ON t.Loc = l.Loc  
	LEFT JOIN Route r ON r.ID = l.Route  
	LEFT JOIN Category c ON t.Cat = c.Type  
    WHERE 
	Edate >= @StartDate AND EDate <@EndDate
	AND c.ISDefault = 1  
    GROUP BY r.Name 

	--3 Total Hours for open tickets
	SELECT 	SUM(ISNULL(t.Est,0)) AS Total,r.Name AS Route 
    FROM TicketO t 
	INNER JOIN Loc l ON l.Loc = t.LID 
	LEFT JOIN route r on L.Route = r.ID 
	LEFT JOIN Category c on t.Cat = c.Type 
    WHERE Assigned <> 4 AND 
	Edate >= @StartDate AND EDate <@EndDate
	AND c.ISDefault = 1 
    GROUP BY r.Name 
END
