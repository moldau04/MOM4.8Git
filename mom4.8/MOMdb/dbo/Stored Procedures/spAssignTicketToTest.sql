CREATE PROCEDURE [dbo].[spAssignTicketToTest]	
	@id int	 
	,@Ticket int =NULL
	,@TestYear int  
	,@UpdatedBy varchar(200)
	,@returnval INT OUTPUT
	,@OldTicket INT=0
	
AS 
BEGIN 		
BEGIN TRY 
DECLARE @curLoc VARCHAR(200)

	DECLARE @work VARCHAR(200)
	DECLARE @fwork INT
    DECLARE @Who VARCHAR(200)
	DECLARE @schedule VARCHAR	(200)
	DECLARE @currentTicketStatus VARCHAR(10)
	SET @work=''
	SET @schedule=''
	SET @currentTicketStatus=''

	DECLARE  @ElevID INT
    	SET @ElevID=(SELECT elev FROM LoadTestItem WHERE LID=@ID)	
	
	SELECT TOP 1 @work=worker, @schedule= CONVERT(varchar, EDate, 101), @currentTicketStatus=Status,  @curLoc=Loc, @Who=Who,@fwork=fwork
	FROM (
			SELECT TicketD.ID, w.fDesc AS worker,EDate AS EDate,Assigned AS Status, Loc AS  Loc ,Who AS Who,fWork AS fwork
			FROM TicketD   
			LEFT OUTER JOIN tblWork w WITH(NOLOCK) on TicketD.fWork=w.ID  
			WHERE TicketD.ID=@Ticket	
			UNION
			SELECT TicketO.ID,w.fDesc, EDate AS EDate,Assigned , LID ,who,fWork FROM TicketO    
			LEFT OUTER JOIN tblWork w WITH(NOLOCK) on TicketO.fWork=w.ID  
			WHERE TicketO.ID=@Ticket 
			UNION
			SELECT TicketDPDA.ID,w.fDesc,EDate,0 , Loc,Who ,fWork FROM TicketDPDA 
			LEFT OUTER JOIN tblWork w WITH(NOLOCK) on TicketDPDA.fWork=w.ID  
		WHERE TicketDPDA.ID=@Ticket 
	) t



	IF (SELECT COUNT(1) FROM LoadTestItem WHERE  LID=@ID AND Loc= @curLoc)>0
	BEGIN
		IF (SELECT COUNT(1) FROM LoadTestItemHistory WHERE LID=@ID AND TestYear=@TestYear)=1
		BEGIN
			UPDATE [dbo].[LoadTestItemHistory]
			SET
			[TestStatus]=1
			,[TicketID]=@Ticket
			,[UpdatedBy]=@UpdatedBy
			,[TicketStatus]=@currentTicketStatus
			,[Worker]=@work
			,[fWork]=@fwork
			,[Who]=@Who
			,[Schedule]=@schedule
			WHERE [LID]=@id	AND TestYear=@TestYear

			IF(@OldTicket!=0)
			BEGIN
				UPDATE LoadTestItemSchedule
				SET TicketId=@Ticket
				,[TicketStatus]=@currentTicketStatus
				,[Worker]=@work
				,[ScheduledDate]=@schedule
				WHERE TicketId=@OldTicket AND [LID]=@id
            END 

			If (select count (*) from LoadTestItemSchedule where LID=@ID and ScheduledYear=@TestYear)=0
			Begin
				INSERT INTO LoadTestItemSchedule (LID,TicketId,TicketStatus,ScheduledDate,Worker,ScheduledYear,ScheduledStatus)
				VALUES (@ID,@Ticket,@currentTicketStatus,@schedule,@work,@TestYear,0)
			
			END
		
			
			

		END 
		ELSE
		BEGIN
			DECLARE @year INT
			DECLARE @Next DATETIME
			SET @Next =(SELECT Next FROM LoadTestItem WHERE LID=@id)
			SET @year =@TestYear-YEAR(@Next)		


			INSERT INTO LoadTestItemHistory ([LID],[TestYear],[TestStatus],[Last],[Next],[TicketID],[TicketStatus],[Worker],[Schedule] ,[UpdatedBy],[LastDue],fWork,Who,IsActive)
			SELECT @ID,@TestYear,1,Last,DATEADD(year, @year, Next) ,@Ticket,@currentTicketStatus,@work,@schedule,@UpdatedBy,LastDue,@fwork,@who,1 from LoadTestItem WHERE LID=@id

			-- Create schedule
				
				INSERT INTO LoadTestItemSchedule (LID,TicketId,TicketStatus,ScheduledDate,Worker,ScheduledYear,ScheduledStatus)
				VALUES (@ID,@Ticket,@currentTicketStatus,@schedule,@work,@TestYear,0)
		END

		DECLARE @NextDueDate DATETIME, @LastDueDate DATETIME , @Last datetime

		select  @NextDueDate =Next, @LastDueDate=LastDue,@Last=Last from LoadTestItemHistory	WHERE [LID]=@id	AND TestYear=@TestYear

		INSERT INTO [dbo].[TestHistory]
		( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
		VALUES
		(@ID ,GETDATE(),@UpdatedBy ,'Assigned' ,@Last,1,null ,@Ticket, @currentTicketStatus,@NextDueDate,@LastDueDate		   )

		SET @returnval=1


		DELETE FROM multiple_equipments WHERE  ticket_id = @OldTicket AND elev_id=  @ElevID
			IF @currentTicketStatus =1
			BEGIN
			INSERT INTO multiple_equipments (ticket_id, elev_id, labor_percentage) VALUES(@Ticket,@ElevID,0)
            END 
			ELSE
            BEGIN
			INSERT INTO multiple_equipments (ticket_id, elev_id, labor_percentage) VALUES(@Ticket,@ElevID,100)
            END 

    END
    ELSE
    BEGIN
		SET @returnval=0
    END
END TRY
BEGIN CATCH
	SET @returnval=2
END CATCH 
	
	return @returnval
	
END 


