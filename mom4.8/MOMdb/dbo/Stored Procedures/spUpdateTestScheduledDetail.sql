CREATE PROCEDURE [dbo].[spUpdateTestScheduledDetail]	
	@id int	 
	,@ScheduledDate VARCHAR(MAX)
	,@ScheduledYear int  
	,@ScheduledStatus INT
    ,@ScheduledWorker VARCHAR(MAX)
	,@UpdatedBy varchar(200)
	,@ScheduleID int	 
	
AS 
BEGIN 	
IF @ScheduleID =0
BEGIN
	IF (SELECT COUNT(1) FROM LoadTestItemSchedule WHERE LID=@ID AND [ScheduledYear]=@ScheduledYear and ScheduledDate=@ScheduledDate and Worker=@ScheduledWorker)=0
	BEGIN
	print 'NEw'
		IF(@ScheduledWorker='')
		BEGIN
			INSERT INTO LoadTestItemSchedule ([LID],[ScheduledYear],[ScheduledDate],[Worker],[ScheduledStatus], [CreatedBy])
			values( @id,@ScheduledYear,@ScheduledDate, @ScheduledWorker,@ScheduledStatus,@UpdatedBy)
		END
		ELSE
		BEGIN
			INSERT INTO LoadTestItemSchedule ([LID],[ScheduledYear],[ScheduledDate],[Worker],[ScheduledStatus], [CreatedBy])
			SELECT @id,@ScheduledYear,@ScheduledDate, items,@ScheduledStatus,@UpdatedBy from [dbo].[Split](@ScheduledWorker , ',')
		END

			
	END
				
END
ELSE
BEGIN
print 'UpdATE'
	UPDATE LoadTestItemSchedule
	SET Worker=@ScheduledWorker, ScheduledDate=@ScheduledDate,ScheduledYear=@ScheduledYear,ScheduledStatus=@ScheduledStatus
	WHERE ID=@ScheduleID
END

IF (SELECT COUNT(1) FROM LoadTestItemHistory WHERE LID=@ID AND TestYear=@ScheduledYear)=0
		
BEGIN
	DECLARE @newLast DATETIME
	DECLARE @newNext DATETIME
	DECLARE @newLastDue DATETIME
    
	SELECT TOP 1
		@newLastDue=isnull( convert(varchar(10),DATEADD(year,(((@ScheduledYear-year(item.Next))/ (ttype.Frequency/12.0))*(ttype.Frequency/12.0))-(ttype.Frequency/12.0), item.Next) ,101),'')
	,@newNext=isnull( convert(varchar(10),DATEADD(year,((@ScheduledYear-year(item.Next))/ (ttype.Frequency/12.0))* (ttype.Frequency/12.0), item.Next) ,101),'')
	FROM LoadTestItem item	
	LEFT join LoadTest ttype on ttype.ID=item.ID
	WHERE item.LID=@id
	INSERT INTO LoadTestItemHistory ([LID],[TestYear],[TestStatus],[Last],[Next],[TicketID],[TicketStatus],[Worker],[Schedule] ,[UpdatedBy],[LastDue],IsActive)
	SELECT @ID,@ScheduledYear,0,null,@newNext ,null,null,@ScheduledWorker,@ScheduledDate,@UpdatedBy,@newLastDue,1 from LoadTestItem WHERE LID=@id

	--If we have multi worker 
	INSERT INTO LoadTestItemSchedule ([LID],[ScheduledYear],[ScheduledDate],[Worker],[ScheduledStatus], [CreatedBy])
	SELECT @id,@ScheduledYear,@ScheduledStatus, items,@ScheduledStatus,@UpdatedBy from [dbo].[Split](@ScheduledWorker , ',')
			
END



	

	DECLARE @lsScheduledDate VARCHAR(200)

	DECLARE @lsworker VARCHAR(200)

	--if (SELECT COUNT(*) FROM LoadTestItemSchedule  WHERE LID=@ID AND ScheduledYear=@ScheduledYear AND TicketId IS not null)=0
	--begin
	
		
		SELECT @lsworker = COALESCE(@lsworker + ',', '') + worker from
		(select  distinct worker
			FROM LoadTestItemSchedule WHERE LID=@ID AND ScheduledYear=@ScheduledYear and Worker!='') t

		SELECT @lsScheduledDate = COALESCE(@lsScheduledDate + ',', '') + ScheduledDate from
		(select  distinct ScheduledDate
			FROM LoadTestItemSchedule WHERE LID=@ID AND ScheduledYear=@ScheduledYear and ScheduledYear!='') t
	
			if (select CHARINDEX(',',@lsworker))=1
			BEGIN
				SET @lsworker=(SELECT SUBSTRING(@lsworker, 1, len(@lsworker)))
			END
			if (select CHARINDEX(',',@lsScheduledDate))=1
			BEGIN
				SET @lsScheduledDate=(SELECT SUBSTRING(@lsScheduledDate, 1, len(@lsScheduledDate)))
			END
						
			

		UPDATE LoadTestItemHistory
		SET worker=@lsworker, Schedule=@lsScheduledDate
		WHERE LID=@ID AND TestYear=@ScheduledYear
		
	--end
		
END 
