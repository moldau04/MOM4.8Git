CREATE PROCEDURE [dbo].[spCreateTestHistory]			
         
			@testid int,
			@Username varchar(100),
			@TestStatus varchar(50),
			@LastDate datetime,
			@TestStatusid int,
			@TestTicketID int ,
			@TestTicketStatus varchar(50)='UnAssigned'

		
as
	begin

		DECLARE @NextDueDate datetime, @LastDueDate datetime

		select  @NextDueDate =Next, @LastDueDate=LastDue from LoadTestItem where lid=@testid
		 
		INSERT INTO [dbo].[TestHistory]
           (
		    [idTest]
           ,[StatusDate]
           ,[UserName]
           ,[TestStatus]
           ,[LastDate]
           ,[idTestStatus]
           ,[ActualDate]
		   ,[TicketID]
		   ,[TicketStatus]
		   ,[NextDueDate]
		   ,[LastDueDate]
		    )
     VALUES
           (		  
		   @testid
           ,GETDATE()
           ,@Username
           ,@TestStatus
           ,@LastDate
           ,@TestStatusid
           ,null
		   ,@TestTicketID,
		    @TestTicketStatus
		   ,@NextDueDate
		   ,@LastDueDate
		   )
	end
GO

