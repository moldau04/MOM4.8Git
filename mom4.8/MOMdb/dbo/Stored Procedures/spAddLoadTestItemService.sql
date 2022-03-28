CREATE Procedure [dbo].[spAddLoadTestItemService] (
            @ID      INT = NULL OUTPUT
           ,@LID     INT 
		   ,@ServiceYear Int
           ,@ServiceDate varchar(max) 
           ,@Status     INT
		   ,@Worker varchar(max) 
		   ,@Username VARCHAR(100)
      
)

AS 
BEGIN

--Service Status
--	0:Pending
--	1:Notified
--	2:Accepted
--	3:Cancelled

INSERT INTO LoadTestItemService 
           ([LID]
		   ,[ServiceYear]  
		   ,[ServiceDate]
           ,[ServiceStatus]
		   ,[Worker]
		   ,[CreatedBy]
          )
     VALUES (           
            @LID
		   ,@ServiceYear
           ,@ServiceDate
           ,@Status
		   ,@Worker 
		   ,@Username)
Set @ID= @@IDENTITY

INSERT INTO LoadTestItemServiceDetail
SELECT @ID,items
FROM   dbo.Idsplit(@ServiceDate, ',')    
END



