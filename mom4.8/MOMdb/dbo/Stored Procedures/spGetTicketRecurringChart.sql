CREATE PROCEDURE [dbo].[spGetTicketRecurringChart]
	-- Add the parameters for the stored procedure here
	@EN INT,
	@StartDate DATETIME,
	@EndDate DATETIME
	
AS
BEGIN

    IF(@EN = 1)
	BEGIN
	       SELECT wk.fDesc AS DWork,SUM(ISNULL(Est,0)) AS Total FROM TicketO tk                                     
		   INNER JOIN tblWork wk ON tk.fWork = wk.ID        
           INNER JOIN Category ct ON tk.Cat = ct.Type       
           INNER JOIN Loc l on l.Loc = tk.LID               
           INNER JOIN Owner ow on l.Owner = ow.ID           
           INNER JOIN Rol rl  ON rl.ID = ow.Rol             
           INNER JOIN tblUserCo uc on uc.CompanyID = rl.EN  
           WHERE Assigned<> 4  
		   AND Edate >= @StartDate AND  Edate < @EndDate
           AND ct.ISDefault = 1                             
           AND uc.IsSel = 1                                 
           AND uc.UserID = 1       
           GROUP BY wk.fDesc      
		   
		   SELECT wk.fDesc AS DWork, SUM(ISNULL(Total,0)) AS Total  FROM TicketDPDA tk                                  
           INNER JOIN tblWork wk ON tk.fWork = wk.ID        
           INNER JOIN Category ct ON tk.Cat = ct.Type       
           INNER JOIN Loc l on l.Loc = tk.Loc               
           INNER JOIN Owner ow on l.Owner = ow.ID           
           INNER JOIN Rol rl  ON rl.ID = ow.Rol             
           INNER JOIN tblUserCo uc on uc.CompanyID = rl.EN  
           WHERE 
	       Edate >= @StartDate AND  Edate < @EndDate
           AND ct.ISDefault = 1                             
           AND uc.IsSel = 1                                 
           AND uc.UserID = 1       
           GROUP BY wk.fDesc                                
         
		   UNION ALL                                           
           
		   SELECT wk.fDesc AS DWork,SUM(ISNULL(Total,0)) AS Total FROM TicketD tk 
           INNER JOIN tblWork wk ON tk.fWork = wk.ID        
           INNER JOIN Category ct ON tk.Cat = ct.Type       
           INNER JOIN Loc l on l.Loc = tk.Loc               
           INNER JOIN Owner ow on l.Owner = ow.ID           
           INNER JOIN Rol rl  ON rl.ID = ow.Rol             
           INNER JOIN tblUserCo uc on uc.CompanyID = rl.EN  
           WHERE		                 
		   Edate >= @StartDate AND  Edate < @EndDate
           AND ct.ISDefault = 1                             
           AND uc.IsSel = 1                                 
           AND uc.UserID = 1       
           GROUP BY wk.fDesc                                

	END
	ELSE
	BEGIN
	print 'test'
	    SELECT tblWork.fDesc AS DWork, SUM(ISNULL(Est,0)) AS Total FROM TicketO LEFT JOIN tblWork ON TicketO.fWork = tblWork.ID LEFT JOIN Category ON TicketO.Cat = Category.Type 
		WHERE Assigned<> 4 AND 
		Edate >= @StartDate AND  Edate < @EndDate
		AND Category.ISDefault = 1 GROUP BY tblWork.fDesc


		SELECT tblWork.fDesc AS DWork, SUM(ISNULL(Total,0)) AS Total FROM TicketDPDA LEFT JOIN tblWork ON TicketDPDA.fWork = tblWork.ID LEFT JOIN Category ON TicketDPDA.Cat = Category.Type
		WHERE
		Edate >= @StartDate AND  Edate < @EndDate
		AND Category.ISDefault = 1 GROUP BY tblWork.fDesc 
		UNION ALL 
		SELECT tblWork.fDesc AS DWork, SUM(ISNULL(Total,0)) AS Total FROM TicketD LEFT JOIN tblWork ON TicketD.fWork = tblWork.ID LEFT JOIN Category ON TicketD.Cat = Category.Type 
		WHERE
		Edate >= @StartDate AND  Edate < @EndDate
		AND Category.ISDefault = 1 
		GROUP BY tblWork.fDesc
	END
END