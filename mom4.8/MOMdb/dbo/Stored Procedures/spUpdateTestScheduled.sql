CREATE PROCEDURE [dbo].[spUpdateTestScheduled]	
	@id int	 
	,@ScheduledDate VARCHAR(MAX)
	,@ScheduledYear int  
	,@ScheduledStatus INT
    ,@ScheduledWorker VARCHAR(MAX)
	,@UpdatedBy varchar(200)
	,@ReturnValue INT OUTPUT  
	
AS 
BEGIN 		
	--1: Successfull
	--2: Invalid date
	--3: Invalid Worker

	DECLARE @countWorker INT
	DECLARE @invalidDate int


	IF OBJECT_ID('tempdb..#TempDate') IS NOT NULL DROP TABLE  #TempDate
	CREATE TABLE #TempDate
	(
	  ScheduleDate VARCHAR(MAX)
	)
	IF OBJECT_ID('tempdb..#TempWorker') IS NOT NULL DROP TABLE  #TempWorker
	CREATE TABLE #TempWorker
	(
	  worker VARCHAR(MAX)
	)

	IF OBJECT_ID('tempdb..#TempSchedule') IS NOT NULL DROP TABLE  #TempSchedule
	CREATE TABLE #TempSchedule
	(
	 ScheduleDate VARCHAR(MAX),
	  worker VARCHAR(MAX)
	)



	INSERT INTO #TempDate (ScheduleDate)
	SELECT items  from   [dbo].[Split](@ScheduledDate , ',')

	

	INSERT INTO #TempWorker (worker)
	SELECT items  from   [dbo].[Split](@ScheduledWorker , ',')
	
	

	IF @ScheduledDate!=''
	BEGIN
		IF @ScheduledWorker!=''
		BEGIN
			INSERT INTO #TempSchedule (ScheduleDate,worker)
			SELECT isnull(#TempDate.ScheduleDate,''),isnull(#TempWorker.worker,'') FROM #TempDate,#TempWorker
		END
		ELSE
		BEGIN
			INSERT INTO #TempSchedule (ScheduleDate,worker)
			SELECT ScheduleDate,'' FROM #TempDate
		END
	END
	ELSE
	BEGIN
		IF @ScheduledWorker!=''
		BEGIN
			INSERT INTO #TempSchedule (ScheduleDate,worker)
			SELECT '',worker FROM #TempWorker
		END
		
	END


	SET @ReturnValue=1
	SET @countWorker=0;
	SET @invalidDate=0;

	IF @ScheduledWorker!=''
	BEGIN
	print '@ScheduledWorker'
		SET @countWorker=(SELECT COUNT(*) from tblWork where Status= 0 AND fDesc IN(SELECT worker FROM #TempWorker ))
		IF @countWorker != (select Count(*) from #TempWorker)
		BEGIN
			set @ReturnValue =3
		END
		
	END
	
	IF @ScheduledDate!=''
	BEGIN	
	print '@@ScheduledDate'
	SELECT * from #TempSchedule WHERE ISDATE(ScheduleDate)=0
		SET @invalidDate=(SELECT COUNT(*) from #TempSchedule WHERE ISDATE(ScheduleDate)=0)	
		if @invalidDate!=0
		BEGIN	
			set @ReturnValue =2
		END
	END
	

	IF @ReturnValue =1
	BEGIN
		print 'LoadTestItemHistory'
		--Update LoadTestItemHistory
		IF (SELECT COUNT(1) FROM LoadTestItemHistory WHERE LID=@ID AND TestYear=@ScheduledYear)=1
		BEGIN
			--If we have multi worker 
			DELETE FROM LoadTestItemSchedule WHERE LID=@ID AND ScheduledYear=@ScheduledYear	

			INSERT INTO LoadTestItemSchedule ([LID],[ScheduledYear],[ScheduledDate],[Worker],[ScheduledStatus], [CreatedBy])
			SELECT @id,@ScheduledYear,ScheduleDate, worker,@ScheduledStatus,@UpdatedBy from  #TempSchedule			
		END 
		ELSE
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
			SELECT @id,@ScheduledYear,ScheduleDate, worker,@ScheduledStatus,@UpdatedBy from  #TempSchedule			
		END

		DECLARE @lsScheduledDate VARCHAR(MAX)

		DECLARE @lsworker VARCHAR(MAX)

		
		SELECT @lsworker = COALESCE(@lsworker + ',', '') + worker from
		(SELECT DISTINCT worker
			FROM LoadTestItemSchedule WHERE LID=@ID AND ScheduledYear=@ScheduledYear and Worker!='') t

		SELECT @lsScheduledDate = COALESCE(@lsScheduledDate + ',', '') + ScheduledDate from
			(SELECT DISTINCT ScheduledDate
			FROM LoadTestItemSchedule WHERE LID=@ID AND ScheduledYear=@ScheduledYear and ScheduledYear!='') t
	
		IF (select CHARINDEX(',',@lsworker))=1
		BEGIN
			SET @lsworker=(SELECT SUBSTRING(@lsworker, 1, len(@lsworker)))
		END
		IF (select CHARINDEX(',',@lsScheduledDate))=1
		BEGIN
			SET @lsScheduledDate=(SELECT SUBSTRING(@lsScheduledDate, 1, len(@lsScheduledDate)))
		END						
			

		UPDATE LoadTestItemHistory
		SET worker=@lsworker, Schedule=@lsScheduledDate
		WHERE LID=@ID AND TestYear=@ScheduledYear

	END

	return @ReturnValue

	
END 
