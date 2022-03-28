CREATE  Procedure [dbo].[spGetAllLoadTestItemSchedule] 
 @LID int
AS 
BEGIN

--Scheduled Status
--	0:Pending
--	1:Notified
--	2:Accepted
--	3:Cancelled

SELECT  ID
          , [LID]
		   ,[ScheduledYear]    
		   ,[ScheduledDate]    
           ,[ScheduledStatus] AS ScheduledStatusID
		   ,CASE [ScheduledStatus] 
				WHEN 0 THEN 'Pending'
				WHEN 1 THEN 'Notified'
				WHEN 2 THEN 'Accepted'
				WHEN 3 THEN 'Cancelled'
				END AS ScheduledStatus

		   ,[Worker]
		   ,[CreatedBy] 
		   ,TicketID
		   ,TicketStatus
		    ,CASE TicketStatus 
				WHEN 1 THEN 'Assigned'
				WHEN 4 THEN  'Completed'				
				END AS TicketStatusName
		   FROM LoadTestItemSchedule
		   WHERE LID=@LID
         
END


