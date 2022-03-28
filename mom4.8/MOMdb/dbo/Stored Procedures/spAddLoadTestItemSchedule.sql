CREATE Procedure [dbo].[spAddLoadTestItemSchedule] (
            @ID      INT = NULL OUTPUT
           ,@LID     INT 
		   ,@ScheduleYear Int
           ,@ScheduledDate varchar(max) 
           ,@Status     INT
		   ,@Worker varchar(max) 
		   ,@Username VARCHAR(100)
      
)

AS 
BEGIN

--Scheduled Status
--	0:Pending
--	1:Notified
--	2:Accepted
--	3:Cancelled

INSERT INTO LoadTestItemSchedule
           ([LID]
		   ,[ScheduledYear]    
		   ,[ScheduledDate]    
           ,[ScheduledStatus]
		   ,[Worker]
		   ,[CreatedBy]
          )
     VALUES (           
           @LID
		   ,@ScheduleYear
           ,@ScheduledDate
           ,@Status
		   ,@Worker
		   ,@Username)
Set @ID= @@IDENTITY

INSERT INTO LoadTestItemScheduleDetail
SELECT @ID,items
FROM   dbo.Idsplit(@ScheduledDate, ',')    

END



