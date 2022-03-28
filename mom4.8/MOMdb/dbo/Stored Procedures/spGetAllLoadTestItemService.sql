CREATE Procedure [dbo].[spGetAllLoadTestItemService] 
 @LID int
AS 
BEGIN

--Service Status
--	0:Pending
--	1:Notified
--	2:Accepted
--	3:Cancelled

SELECT  
           ID,[LID]
		   ,[ServiceYear]    
		   ,[ServiceDate]    
           ,[ServiceStatus] AS ServiceStatusID
		   ,CASE [ServiceStatus] 
				WHEN 0 THEN 'Pending'
				WHEN 1 THEN 'Notified'
				WHEN 2 THEN 'Accepted'
				WHEN 3 THEN 'Cancelled'
				END AS ServiceStatus

		   ,[Worker]
		   ,[CreatedBy] FROM LoadTestItemService
		   WHERE LID=@LID
         
END




