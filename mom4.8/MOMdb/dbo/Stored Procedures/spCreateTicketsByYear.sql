CREATE PROCEDURE [dbo].[spCreateTicketsByYear]
	@LID int,
	@Username varchar(100),
	@TestYear INT,
	@ListTicketID VARCHAR(100)=null Output
		
AS 
BEGIN 
Declare @curLoc int
set @curLoc= (select top 1 Loc from LoadTestItem where LID=@LID)
SET @ListTicketID='0'

DECLARE @tempWork varchar(200)
Declare @chil_LID Int
Declare @parent_LID Int
DECLARE  @ElevID int
--Ticket
DECLARE @WorkOrder    VARCHAR(10)
DECLARE @TicketID    VARCHAR(10)
DECLARE @work VARCHAR(200)
DECLARE @fwork INT
DECLARE @Who VARCHAR(200)
DECLARE @schedule VARCHAR	(200)
DECLARE @currentTicketStatus VARCHAR(10)
DECLARE @TestType INT 
DECLARE @Loc Int 
DECLARE @c_LID Int 
DECLARE @NewWorker VARCHAR(200)
DECLARE @NewSchedule VARCHAR	(200)
CREATE TABLE #TEMPLOADTEST
(
	LID INT,
	TID INT,
	Next DATETIME,
	Name VARCHAR(300),
	Tag VARCHAR(300),
	LOC INT,
	LOCID VARCHAR(300),
	ROUTE INT,
	UNIT VARCHAR(300),
	ELEVID INT,
	TESTTYPE VARCHAR(300),
	Last DATETIME,
	Status INT,
	idRolCustomContact INT,
	ELEVDESC VARCHAR(300),
	ELEVSTATE VARCHAR(300),
	EN INT,
	idWorker INT,
	CallSign VARCHAR(300),
	idRoute INT,
	RouteName VARCHAR(300),
	Level int,
	Cat varchar(300),
	Frequency int,
	Authority varchar(300)
)
CREATE TABLE #templocjob
(

Address   VARCHAR (255) 
,City      VARCHAR (50)    
,State   VARCHAR (2)    
,Zip   VARCHAR (10)    
,Terr  INT           
,fLong INT          
,Latt INT             
,Loc  INT
,Owner INT
,Status  SMALLINT       
,idMaintJob INT
)
IF OBJECT_ID('tempdb..#tempLID') IS NOT NULL DROP TABLE #tempLID
CREATE Table #tempLID(
LID         INT 	
)
DECLARE @NextDueDate DATETIME, @LastDueDate DATETIME , @Last datetime
Insert into LoadTestItemHistory (LID,TestYear,TestStatus,Last,Next,LastDue,isTestDefault,IsActive)
	select 
	item.LID
	,@TestYear as TestYear
	,0
	,item.Last
	,isnull( convert(varchar(10),DATEADD(year,((@TestYear-year(item.Next))/ (ttype.Frequency/12.0))* (ttype.Frequency/12.0), item.Next) ,101),'')
	,isnull( convert(varchar(10),DATEADD(year,(((@TestYear-year(item.Next))/ (ttype.Frequency/12.0))*(ttype.Frequency/12.0))-(ttype.Frequency/12.0), item.Next) ,101),'')	
	,0
	,1
	from LoadTestItem item
	LEFT join LoadTest ttype on ttype.ID=item.ID
	INNER JOIN ListConfig ON ListConfig.ListName='Test.Status' AND item.Status = ListConfig.ItemValue     
	inner join Loc l on l.Loc=item.Loc	
	INNER JOIN Elev e on e.ID=Item.Elev
	where
	l.Status<>1 and e.Status =0 
	and year(item.Next)<=@TestYear
	and Year(isnull( convert(varchar(10),DATEADD(year,((@TestYear-year(item.Next))/ (ttype.Frequency/12.0))* (ttype.Frequency/12.0), item.Next) ,101),''))=@TestYear
	and item.Loc=@curLoc
	and (select count(1) from LoadTestItemHistory where LoadTestItemHistory.LID=Item.LID and LoadTestItemHistory.TestYear=@TestYear)=0


	
	SET @ElevID=(SELECT elev FROM LoadTestItem WHERE LID=@LID)	

	IF ((SELECT COUNT(LoadTestItem.LID) cSkip FROM LoadTestItem INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc WHERE Loc.Credit=1 AND LoadTestItem.LID=@LID)=0  
	--AND(SELECT COUNT(LID) cSkip FROM LoadTestItemHistory WHERE ISNULL(Ticketid ,0)>0 AND LID=@LID AND TestYear=@TestYear)=0
	)
	BEGIN  
		IF(SELECT COUNT(*) FROM LoadTestItemSchedule WHERE LID=@LID AND ScheduledYear=@TestYear AND TicketId IS null)>0
		BEGIN
		-- Create ticket in case have multi worker
			PRINT 'Create Ticket for Test has schedule worker'
			DECLARE @c_worker VARCHAR(100)
			DECLARE @c_workerId VARCHAR(100)
			DECLARE @c_tempDate VARCHAR(100)
			DECLARE @c_ScheduleDate VARCHAR(100)

			DECLARE cur_Worker CURSOR FOR 	
				SELECT worker,ScheduledDate FROM LoadTestItemSchedule WHERE LID=@LID AND ScheduledYear=@TestYear AND TicketId IS null
			OPEN cur_Worker  
			FETCH NEXT FROM cur_Worker INTO @c_worker,@c_tempDate
			WHILE @@FETCH_STATUS = 0  
			BEGIN
			print @c_worker
			IF(@c_worker='')
			BEGIN
			SET @c_workerId= (SELECT TOP 1
					w.ID idWorker
									   
					FROM 
					LoadTestItem  INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc
					INNER JOIN Owner ON Loc.Owner = Owner.ID 
					INNER JOIN Rol ON Owner.Rol = Rol.ID 	
					LEFT  JOIN Route r ON Loc.Route=r.ID 
					left  JOIN tblWork w ON r.Mech = w.ID
					WHERE LoadTestItem.LID=@LID)
			END
			ELSE
			BEGIN
				SET @c_workerId= ISNULL((SELECT  w.id  FROM tblwork w   WHERE w.id IS NOT NULL  AND w.status=0  and w.fDesc=@c_worker),0)
			END

			
			IF @c_workerId<>0
			BEGIN
				set  @NewWorker=''
			   set  @NewWorker=(SELECT  w.fDesc FROM tblwork w   WHERE w.id IS NOT NULL  AND w.status=0  and w.id =@c_workerId)
				--Get Ticket ID
				SELECT @TicketID = Max([NewID]) + 1
				FROM   (SELECT Isnull(Max(TicketO.ID), 0) AS [NewID]
						FROM   TicketO
						UNION ALL
						SELECT Isnull(Max(TicketD.ID), 0) AS [NewID]
						FROM   TicketD) A	

				SET @WorkOrder = @TicketID		
				
				SET @c_ScheduleDate= ISNULL((SELECT TOP 1 items FROM dbo.Split(@c_tempDate,',')),GETDATE());
				SET @NewSchedule=@c_ScheduleDate
				if(select  DATEPART(HOUR,@c_ScheduleDate)) =0 and (select LEN(@c_ScheduleDate))<11
				begin
					SET @NewSchedule = (select convert(varchar,@c_ScheduleDate, 121) +' '+ convert(varchar,DATEPART(HOUR,GETDATE())) +':'+convert(varchar,DATEPART(MINUTE,GETDATE())) )
				end
				
				DELETE FROM #TEMPLOADTEST
				DELETE FROM #templocjob

				INSERT INTO #TEMPLOADTEST
				SELECT LoadTestItem.LID, LoadTest.ID AS TID, itemHistory.Next, Rol.Name, Loc.Tag, Loc.Loc, Loc.ID, Loc.Route, Elev.Unit, Elev.ID as NID, 
						LoadTest.Name as NTest, itemHistory.Last, LoadTestItem.Status,LoadTestItem.idRolCustomContact,Elev.fDesc,Elev.State,Rol.EN,
						isnull(@c_workerId,w.ID) idWorker, @NewWorker CallSign, r.Id idRoute, r.Name RouteName,LoadTest.Level,LoadTest.Cat,LoadTest.Frequency,LoadTest.Authority 
									   
						FROM 
						LoadTestItem INNER JOIN Elev ON LoadTestItem.Elev = Elev.ID INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc
						INNER JOIN Owner ON Loc.Owner = Owner.ID INNER JOIN Rol ON Owner.Rol = Rol.ID 
						INNER JOIN LoadTest ON LoadTestItem.ID = LoadTest.ID 
						INNER JOIN LoadTestItemHistory itemHistory ON itemHistory.LID=LoadTestItem.LID
						left  JOIN Route r ON Loc.Route=r.ID 
						left  JOIN tblWork w ON r.Mech = w.ID
						WHERE LoadTestItem.LID=@LID AND itemHistory.TestYear=@TestYear

				IF EXISTS(SELECT 1 FROM #TEMPLOADTEST)
				BEGIN
					INSERT INTO  #templocjob
					SELECT l.Address,l.City,l.State,l.zip	,l.Terr,l.fLong,l.Latt,l.Loc,l.Owner,l.Status,isnull(j.id,0) idMaintJob 					
					FROM Loc l
					INNER JOIN #TEMPLOADTEST ON l.Loc=#TEMPLOADTEST.LOC
					LEFT JOIN Job j ON l.Loc=j.Loc AND j.Type=0 AND j.Status=0 and j.elev=#TEMPLOADTEST.ELEVID
					
					SET @tempWork=(SELECT #TEMPLOADTEST.CallSign FROM #TEMPLOADTEST inner join #templocjob ON #TEMPLOADTEST.LOC=#templocjob.Loc)
					print 'tempWorker'
					print @tempWork
					If (SELECT count(1)  FROM LoadTestItemSchedule WHERE LID=@LID  AND ScheduledYear=@TestYear and (isnull(Worker,'')='' or Worker=@tempWork) and TicketID is null)>=1
					BEGIN
						INSERT INTO [TicketO]
							(
								[ID]
								,[CDate],[DDate],[EDate]
								,[Level],[Est]
								,[fWork],[DWork]
								,[Type],[Cat]
								,[fDesc],[Who]
								,[fBy]
								,[LType],[LID],[LElev]
								,[LDesc1],[LDesc2],[LDesc3],[LDesc4]
								,[Nature]
								,[Job],[Assigned]
								,[City],[State],[Zip],[Owner]
								,[Route],[Terr]
								,[fLong],[Latt]
								,[CallIn],[SpecType],[SpecID]
								,[EN]	,[WorkOrder],[is_work_order]
							)												
							SELECT 
								@TicketID
								,convert(varchar, getdate(), 121)
								,convert(varchar, getdate(), 121)
								,convert(varchar,@NewSchedule, 121)
								,#TEMPLOADTEST.Level
								,1.00
								,#TEMPLOADTEST.idWorker
								,#TEMPLOADTEST.CallSign,0,#TEMPLOADTEST.Cat,

								'Perform '+ #TEMPLOADTEST.TESTTYPE+' on unit ' + #TEMPLOADTEST.UNIT+  ' ' 
								+#TEMPLOADTEST.ELEVDESC+' (State ID: '+#TEMPLOADTEST.ELEVSTATE+') at '
								+#TEMPLOADTEST.Tag+'. This test is performed every '+cast(#TEMPLOADTEST.Frequency as varchar) 
								+' months. This test is under the authority of '+#TEMPLOADTEST.Authority+'.',

								@Username,
								@Username,0,
								#TEMPLOADTEST.LOC,
								#TEMPLOADTEST.ELEVID,
								cast( #TEMPLOADTEST.Name as varchar) Name,
								#TEMPLOADTEST.Tag,#templocjob.Address,#templocjob.City+', '+#templocjob.State+' '+#templocjob.Zip,1,#templocjob.idMaintJob,
								case #TEMPLOADTEST.ROUTE when 0 then 0 else 1 end,
								#templocjob.City,#templocjob.State,#templocjob.Zip,#templocjob.Owner,#TEMPLOADTEST.idRoute,
								#templocjob.Terr,#templocjob.fLong,#templocjob.Latt,0,#TEMPLOADTEST.TID,#TEMPLOADTEST.LID,#TEMPLOADTEST.EN,
								@WorkOrder,0
							FROM #TEMPLOADTEST inner join #templocjob on #TEMPLOADTEST.LOC=#templocjob.Loc

							 DELETE FROM multiple_equipments WHERE  ticket_id = @TicketID  
							 INSERT INTO multiple_equipments (ticket_id, elev_id, labor_percentage) VALUES(@TicketID,@ElevID,0)
					END
					ELSE
					BEGIN
						SET @TicketID=-1
					END

							
												 
				END

				--Update information for Test by year
				IF  EXISTS(SELECT TicketO.ID FROM TicketO WHERE TicketO.ID=@TicketID)
				BEGIN
        
				--	PRINT 'Update Test infor'
					

					SELECT 
					@work=w.fDesc, @schedule= CONVERT(varchar, EDate, 101), @currentTicketStatus=Status,  @Who=Who,@fwork=fwork
					FROM TicketO    
					LEFT OUTER JOIN tblWork w WITH(NOLOCK) on TicketO.fWork=w.ID  
					WHERE TicketO.ID=@TicketID 

					print @work
					UPDATE [dbo].[LoadTestItemHistory]
					SET
					[TestStatus]=1
					,[TicketID]=@TicketID   
					,[Worker]=@work
					,[fWork]=@fwork
					,[Who]=@Who
					,[Schedule]=@NewSchedule
					,[TicketStatus]=1
					WHERE [LID]=@LID	AND TestYear=@TestYear

					UPDATE LoadTestItemSchedule
					SET TicketId=@TicketID
					,TicketStatus=1
					,ScheduledDate=@NewSchedule
					,Worker=@NewWorker
					WHERE [LID]=@LID	AND ScheduledYear=@TestYear AND Worker=@c_worker and ScheduledDate=@c_tempDate


				
					select  @NextDueDate =Next, @LastDueDate=LastDue,@Last=Last from LoadTestItemHistory	WHERE [LID]=@LID	AND TestYear=@TestYear

					INSERT INTO [dbo].[TestHistory]
					( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
					VALUES
					(@LID ,GETDATE(),@Username ,'Assigned' ,@Last,1,null ,@TicketID, 1,@NextDueDate,@LastDueDate)

					--Update for Test Cover
					
					SELECT @TestType=ID, @Loc=Loc FROM LoadTestItem WHERE [LID]=@LID
					
					--Update for Test Cover
			
				SELECT @TestType=ID, @Loc=Loc FROM LoadTestItem WHERE [LID]=@LID
				
				
				SET @chil_LID =isnull((SELECT item.LID 
				FROM [LoadTestItem] item
				INNER JOIN LoadTestItemHistory itemHistory ON item.LID=itemHistory.LID
				WHERE --ISNULL(TicketID,0)=0 AND 
				Loc =@Loc 
					AND item.LID IN (SELECT subItemHistory.LID FROM [LoadTestItemHistory] subItemHistory
								INNER JOIN LoadTestItem subTest ON subTest.LID=subItemHistory.LID 
								WHERE subTest.Elev=item.Elev and subItemHistory.TestYear=@TestYear and Loc=@loc AND ID in(select TestTypeCoverID from TestTypeCover where TestTypeID=@TestType))
					AND item.LID !=@LID AND ItemHistory.TestYear=@TestYear and Item.Elev=@ElevID),0)
					print 'child'
					print @chil_LID
					print @LID
				IF (@chil_LID!=0)
				BEGIN
					IF(SELECT count(1) FROM LoadTestItemSchedule WHERE LID=@chil_LID and ScheduledYear=@TestYear and Worker=@c_worker and ScheduledDate=@schedule)=0
					BEGIN
						-- Create schedule
						print 'Create schedule for child'
						print @chil_LID
						INSERT INTO LoadTestItemSchedule (LID,TicketId,TicketStatus,ScheduledDate,Worker,ScheduledYear,ScheduledStatus)
						VALUES (@chil_LID,@TicketID,1,@schedule,@work,@TestYear,0)

						UPDATE [dbo].[LoadTestItemHistory]
						SET
						[TestStatus]=1
						,[TicketID]=@TicketID   
						,[Worker]=@work
						,[fWork]=@fwork
						,[Who]=@Who
						,[Schedule]=@schedule
						,[TicketStatus]=1
						WHERE [LID]=@chil_LID	AND TestYear=@TestYear			

											

					select  @NextDueDate =Next, @LastDueDate=LastDue,@Last=Last from LoadTestItemHistory	WHERE [LID]=@chil_LID	AND TestYear=@TestYear

					INSERT INTO [dbo].[TestHistory]
					( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
					VALUES
					(@chil_LID ,GETDATE(),@Username ,'Assigned' ,@Last,1,null ,@TicketID, 1,@NextDueDate,@LastDueDate)

					END
					ELSE
					BEGIN
						UPDATE [dbo].[LoadTestItemSchedule]
						SET
						TicketStatus=1
						,[TicketID]=@TicketID   
						
						WHERE  LID=@chil_LID and ScheduledYear=@TestYear and Worker=@work and ScheduledDate=@schedule and TicketID is null

						UPDATE [dbo].[LoadTestItemHistory]
						SET
						[TestStatus]=1
						,[TicketID]=(select top 1 Convert(varchar,TicketId) from LoadTestItemSchedule where LID=@chil_LID and ScheduledYear=@TestYear and TicketStatus=1 order by TicketId desc)
						,[Worker]=@work
						,[fWork]=@fwork
						,[Who]=@Who
						,[Schedule]=@schedule
						,[TicketStatus]=1

						WHERE [LID]=@chil_LID	AND TestYear=@TestYear	

						select  @NextDueDate =Next, @LastDueDate=LastDue,@Last=Last from LoadTestItemHistory	WHERE [LID]=@chil_LID	AND TestYear=@TestYear

						INSERT INTO [dbo].[TestHistory]
						( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
						VALUES
						(@chil_LID ,GETDATE(),@Username ,'Assigned' ,@Last,1,null ,@TicketID, 1,@NextDueDate,@LastDueDate)

					END
				END

				SET @parent_LID=(SELECT item.LID FROM [LoadTestItem] item
					INNER JOIN LoadTestItemHistory itemHistory ON item.LID=itemHistory.LID	
					WHERE --ISNULL(TicketID,0)=0 AND 
					Loc =@Loc 
					AND item.LID IN (SELECT subItemHistory.LID FROM [LoadTestItemHistory] subItemHistory
								INNER JOIN LoadTestItem subTest ON subTest.LID=subItemHistory.LID 
								WHERE  subTest.Elev=item.Elev and subItemHistory.TestYear=@TestYear and Loc=@loc AND ID in(select TestTypeID from TestTypeCover where TestTypeCoverID=@TestType))
					AND item.LID !=@LID AND ItemHistory.TestYear=@TestYear  and Item.Elev=@ElevID)
					print 'parent_LID'
					print @parent_LID
					print @LID
				IF (@parent_LID!=0)
				BEGIN
					IF(SELECT count(1) FROM LoadTestItemSchedule WHERE LID=@parent_LID and ScheduledYear=@TestYear and Worker=@c_worker and ScheduledDate=@schedule)=0
					BEGIN
						-- Create schedule
						print 'Create schedule for Parent'
						print @parent_LID
						print @TicketID
							print @schedule
								print @work
								print'========'
						INSERT INTO LoadTestItemSchedule (LID,TicketId,TicketStatus,ScheduledDate,Worker,ScheduledYear,ScheduledStatus)
						VALUES (@parent_LID,@TicketID,1,@schedule,@work,@TestYear,0)

						UPDATE [dbo].[LoadTestItemHistory]
						SET
						[TestStatus]=1
						,[TicketID]=@TicketID   
						,[Worker]=@work
						,[fWork]=@fwork
						,[Who]=@Who
						,[Schedule]=@schedule
						,[TicketStatus]=1
						WHERE [LID]=@parent_LID	AND TestYear=@TestYear	


						select  @NextDueDate =Next, @LastDueDate=LastDue,@Last=Last from LoadTestItemHistory	WHERE [LID]=@parent_LID	AND TestYear=@TestYear

						INSERT INTO [dbo].[TestHistory]
						( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
						VALUES
						(@parent_LID ,GETDATE(),@Username ,'Assigned' ,@Last,1,null ,@TicketID, 1,@NextDueDate,@LastDueDate)
						
					END
					ELSE
					BEGIN
						UPDATE [dbo].[LoadTestItemSchedule]
						SET
						TicketStatus=1
						,[TicketID]=@TicketID   
						
						WHERE  LID=@parent_LID and ScheduledYear=@TestYear and Worker=@work and ScheduledDate=@schedule and TicketID is null

						UPDATE [dbo].[LoadTestItemHistory]
						SET
						[TestStatus]=1
						,[TicketID]=(select top 1 Convert(varchar,TicketId) from LoadTestItemSchedule where LID=@parent_LID and ScheduledYear=@TestYear and TicketStatus=1 order by TicketId desc)
						,[Worker]=@work
						,[fWork]=@fwork
						,[Who]=@Who
						,[Schedule]=@schedule
						,[TicketStatus]=1
						WHERE [LID]=@parent_LID	AND TestYear=@TestYear	


						select  @NextDueDate =Next, @LastDueDate=LastDue,@Last=Last from LoadTestItemHistory	WHERE [LID]=@parent_LID	AND TestYear=@TestYear

						INSERT INTO [dbo].[TestHistory]
						( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
						VALUES
						(@parent_LID ,GETDATE(),@Username ,'Assigned' ,@Last,1,null ,@TicketID, 1,@NextDueDate,@LastDueDate)

					END
				END
				
				SET @ListTicketID=@ListTicketID+ ',' + @TicketID
				END 

				END

				FETCH NEXT FROM cur_Worker INTO @c_worker,@c_tempDate
			END	
			CLOSE cur_Worker  
			DEALLOCATE cur_Worker  
			
		END 
		ELSE
		BEGIN
			PRINT 'Create Ticket for Test does not have schedule worker'
			Print @LID
			--Get Ticket ID
			Delete from #TEMPLOADTEST
			Delete from #templocjob

			SELECT @TicketID = Max([NewID]) + 1
			FROM   (SELECT Isnull(Max(TicketO.ID), 0) AS [NewID]
					FROM   TicketO
					UNION ALL
					SELECT Isnull(Max(TicketD.ID), 0) AS [NewID]
					FROM   TicketD) A	

			SET @WorkOrder = @TicketID	
			print @TicketID
			
			INSERT INTO #TEMPLOADTEST
			SELECT LoadTestItem.LID, LoadTest.ID AS TID, itemHistory.Next, Rol.Name, Loc.Tag, Loc.Loc, Loc.ID, Loc.Route, Elev.Unit, Elev.ID as NID, 
					LoadTest.Name as NTest, itemHistory.Last, LoadTestItem.Status,LoadTestItem.idRolCustomContact,Elev.fDesc,Elev.State,Rol.EN,
					w.ID idWorker, w.fDesc CallSign, r.Id idRoute, r.Name RouteName,LoadTest.Level,LoadTest.Cat,LoadTest.Frequency,LoadTest.Authority 
									   
					FROM 
					LoadTestItem INNER JOIN Elev ON LoadTestItem.Elev = Elev.ID INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc
					INNER JOIN Owner ON Loc.Owner = Owner.ID INNER JOIN Rol ON Owner.Rol = Rol.ID 
					INNER JOIN LoadTest ON LoadTestItem.ID = LoadTest.ID 
					INNER JOIN LoadTestItemHistory itemHistory ON itemHistory.LID=LoadTestItem.LID
					left  JOIN Route r ON Loc.Route=r.ID 
					left  JOIN tblWork w ON r.Mech = w.ID
					WHERE LoadTestItem.LID=@LID AND itemHistory.TestYear=@TestYear
			IF EXISTS(SELECT 1 FROM #TEMPLOADTEST)
			BEGIN
				INSERT INTO  #templocjob
				SELECT l.Address,l.City,l.State,l.zip	,l.Terr,l.fLong,l.Latt,l.Loc,l.Owner,l.Status,isnull(j.id,0) idMaintJob 	
				FROM Loc l
				INNER JOIN #TEMPLOADTEST ON l.Loc=#TEMPLOADTEST.LOC
				LEFT JOIN Job j ON l.Loc=j.Loc AND j.Type=0 AND j.Status=0 and j.elev=#TEMPLOADTEST.ELEVID

				SET @tempWork=(SELECT 
							#TEMPLOADTEST.CallSign
						FROM #TEMPLOADTEST inner join #templocjob on #TEMPLOADTEST.LOC=#templocjob.Loc)
				
				If (select count(1)  FROM LoadTestItemSchedule WHERE LID=@LID  AND ScheduledYear=@TestYear and Worker=@tempWork)=0
				BEGIN
					INSERT INTO [TicketO]
						(
							[ID]
							,[CDate],[DDate],[EDate]
							,[Level],[Est]
							,[fWork],[DWork]
							,[Type],[Cat]
							,[fDesc],[Who]
							,[fBy]
							,[LType],[LID],[LElev]
							,[LDesc1],[LDesc2],[LDesc3],[LDesc4]
							,[Nature]
							,[Job],[Assigned]
							,[City],[State],[Zip],[Owner]
							,[Route],[Terr]
							,[fLong],[Latt]
							,[CallIn],[SpecType],[SpecID]
							,[EN]	,[WorkOrder],[is_work_order]
						)												
						SELECT 
							@TicketID
							,convert(varchar, getdate(), 121)
							,convert(varchar, getdate(), 121)
							,convert(varchar, getdate(), 121)
							,#TEMPLOADTEST.Level
							,1.00
							,#TEMPLOADTEST.idWorker
							,#TEMPLOADTEST.CallSign,0,#TEMPLOADTEST.Cat,

							'Perform '+ #TEMPLOADTEST.TESTTYPE+' on unit ' + #TEMPLOADTEST.UNIT+  ' ' 
							+#TEMPLOADTEST.ELEVDESC+' (State ID: '+#TEMPLOADTEST.ELEVSTATE+') at '
							+#TEMPLOADTEST.Tag+'. This test is performed every '+cast(#TEMPLOADTEST.Frequency as varchar) 
							+' months. This test is under the authority of '+#TEMPLOADTEST.Authority+'.',

							@Username,
							@Username,0,
							#TEMPLOADTEST.LOC,
							#TEMPLOADTEST.ELEVID,
							cast( #TEMPLOADTEST.Name as varchar) Name,
							#TEMPLOADTEST.Tag,#templocjob.Address,#templocjob.City+', '+#templocjob.State+' '+#templocjob.Zip,1,#templocjob.idMaintJob,
							case #TEMPLOADTEST.ROUTE when 0 then 0 else 1 end,
							#templocjob.City,#templocjob.State,#templocjob.Zip,#templocjob.Owner,#TEMPLOADTEST.idRoute,
							#templocjob.Terr,#templocjob.fLong,#templocjob.Latt,0,#TEMPLOADTEST.TID,#TEMPLOADTEST.LID,#TEMPLOADTEST.EN,
							@WorkOrder,0
						FROM #TEMPLOADTEST inner join #templocjob on #TEMPLOADTEST.LOC=#templocjob.Loc


						 DELETE FROM multiple_equipments WHERE  ticket_id = @TicketID  
							 INSERT INTO multiple_equipments (ticket_id, elev_id, labor_percentage) VALUES(@TicketID,@ElevID,0)
				END
				ELSE
				BEGIN
					SET @TicketID=-1
				END
						
												 
			END

			--Update information for Test by year
			IF  EXISTS(SELECT TicketO.ID FROM TicketO WHERE TicketO.ID=@TicketID)
			BEGIN
        
				PRINT 'Update Test infor'				

				SELECT 
				@work=w.fDesc, @schedule= CONVERT(varchar, EDate, 101), @currentTicketStatus=Status,  @Who=Who,@fwork=fwork
				FROM TicketO    
				LEFT OUTER JOIN tblWork w WITH(NOLOCK) on TicketO.fWork=w.ID  
				WHERE TicketO.ID=@TicketID 
						
				
				-- Update TestHistory
				UPDATE [dbo].[LoadTestItemHistory]
				SET
				[TestStatus]=1
				,[TicketID]=@TicketID   
				,[Worker]=@work
				,[fWork]=@fwork
				,[Who]=@Who
				,[Schedule]=@schedule
				,[TicketStatus]=1
				WHERE [LID]=@LID	AND TestYear=@TestYear

				-- Create schedule
				DELETE FROM LoadTestItemSchedule WHERE LID=@LID  AND ScheduledYear=@TestYear and Worker is null and TicketID is null
				INSERT INTO LoadTestItemSchedule (LID,TicketId,TicketStatus,ScheduledDate,Worker,ScheduledYear,ScheduledStatus)
				VALUES (@LID,@TicketID,1,@schedule,@work,@TestYear,0)

				--Update for Test Cover
			
				SELECT @TestType=ID, @Loc=Loc FROM LoadTestItem WHERE [LID]=@LID				
				
				SET @chil_LID =isnull((SELECT item.LID 
				FROM [LoadTestItem] item
				INNER JOIN LoadTestItemHistory itemHistory ON item.LID=itemHistory.LID
				WHERE --ISNULL(TicketID,0)=0 AND 
				Loc =@Loc 
					AND item.LID IN (SELECT subItemHistory.LID FROM [LoadTestItemHistory] subItemHistory
								INNER JOIN LoadTestItem subTest ON subTest.LID=subItemHistory.LID 
								WHERE subTest.Elev=item.Elev and subItemHistory.TestYear=@TestYear and Loc=@loc AND ID in(select TestTypeCoverID from TestTypeCover where TestTypeID=@TestType))
					AND item.LID !=@LID AND ItemHistory.TestYear=@TestYear and Item.Elev=@ElevID),0)

				IF (@chil_LID!=0)
				BEGIN
					IF(SELECT count(1) FROM LoadTestItemSchedule WHERE LID=@chil_LID and ScheduledYear=@TestYear and Worker=@work and ScheduledDate=@schedule)=0
					BEGIN
						-- Create schedule
						INSERT INTO LoadTestItemSchedule (LID,TicketId,TicketStatus,ScheduledDate,Worker,ScheduledYear,ScheduledStatus)
						VALUES (@chil_LID,@TicketID,1,@schedule,@work,@TestYear,0)

						UPDATE [dbo].[LoadTestItemHistory]
						SET
						[TestStatus]=1
						,[TicketID]=@TicketID   
						,[Worker]=@work
						,[fWork]=@fwork
						,[Who]=@Who
						,[Schedule]=@schedule
						,[TicketStatus]=1
						WHERE [LID]=@chil_LID	AND TestYear=@TestYear		
						
						select  @NextDueDate =Next, @LastDueDate=LastDue,@Last=Last from LoadTestItemHistory	WHERE [LID]=@chil_LID	AND TestYear=@TestYear

						INSERT INTO [dbo].[TestHistory]
						( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
						VALUES
						(@chil_LID ,GETDATE(),@Username ,'Assigned' ,@Last,1,null ,@TicketID, 1,@NextDueDate,@LastDueDate)

					END
					ELSE
					BEGIN
						UPDATE [dbo].[LoadTestItemSchedule]
						SET
						TicketStatus=1
						,[TicketID]=@TicketID   
						
						WHERE  LID=@chil_LID and ScheduledYear=@TestYear and Worker=@work and ScheduledDate=@schedule and TicketID is null

						UPDATE [dbo].[LoadTestItemHistory]
						SET
						[TestStatus]=1
						,[TicketID]=(select top 1 Convert(varchar,TicketId) from LoadTestItemSchedule where LID=@chil_LID and ScheduledYear=@TestYear and TicketStatus=1 order by TicketId desc)
						,[Worker]=@work
						,[fWork]=@fwork
						,[Who]=@Who
						,[Schedule]=@schedule
						,[TicketStatus]=1
						WHERE [LID]=@chil_LID	AND TestYear=@TestYear	


						select  @NextDueDate =Next, @LastDueDate=LastDue,@Last=Last from LoadTestItemHistory	WHERE [LID]=@chil_LID	AND TestYear=@TestYear

						INSERT INTO [dbo].[TestHistory]
						( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
						VALUES
						(@chil_LID ,GETDATE(),@Username ,'Assigned' ,@Last,1,null ,@TicketID, 1,@NextDueDate,@LastDueDate)

					END
				END

				SET @parent_LID=(SELECT item.LID FROM [LoadTestItem] item
					INNER JOIN LoadTestItemHistory itemHistory ON item.LID=itemHistory.LID	
					WHERE --ISNULL(TicketID,0)=0 AND 
					Loc =@Loc 
					AND item.LID IN (SELECT subItemHistory.LID FROM [LoadTestItemHistory] subItemHistory
								INNER JOIN LoadTestItem subTest ON subTest.LID=subItemHistory.LID 
								WHERE  subTest.Elev=item.Elev and subItemHistory.TestYear=@TestYear and Loc=@loc AND ID in(select TestTypeID from TestTypeCover where TestTypeCoverID=@TestType))
					AND item.LID !=@LID AND ItemHistory.TestYear=@TestYear  and Item.Elev=@ElevID)

				IF (@parent_LID!=0)
				BEGIN
					IF(SELECT count(1) FROM LoadTestItemSchedule WHERE LID=@parent_LID and ScheduledYear=@TestYear and Worker=@work and ScheduledDate=@schedule)=0
					BEGIN
						-- Create schedule
						INSERT INTO LoadTestItemSchedule (LID,TicketId,TicketStatus,ScheduledDate,Worker,ScheduledYear,ScheduledStatus)
						VALUES (@parent_LID,@TicketID,1,@schedule,@work,@TestYear,0)

						UPDATE [dbo].[LoadTestItemHistory]
						SET
						[TestStatus]=1
						,[TicketID]=@TicketID   
						,[Worker]=@work
						,[fWork]=@fwork
						,[Who]=@Who
						,[Schedule]=@schedule
						,[TicketStatus]=1
						WHERE [LID]=@parent_LID	AND TestYear=@TestYear	

						select  @NextDueDate =Next, @LastDueDate=LastDue,@Last=Last from LoadTestItemHistory	WHERE [LID]=@parent_LID	AND TestYear=@TestYear

						INSERT INTO [dbo].[TestHistory]
						( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
						VALUES
						(@parent_LID ,GETDATE(),@Username ,'Assigned' ,@Last,1,null ,@TicketID, 1,@NextDueDate,@LastDueDate)
					END
					ELSE
					BEGIN
						UPDATE [dbo].[LoadTestItemSchedule]
						SET
						TicketStatus=1
						,[TicketID]=@TicketID   
						
						WHERE  LID=@parent_LID and ScheduledYear=@TestYear and Worker=@work and ScheduledDate=@schedule and TicketID is null

						UPDATE [dbo].[LoadTestItemHistory]
						SET
						[TestStatus]=1
						,[TicketID]=(select top 1 Convert(varchar,TicketId) from LoadTestItemSchedule where LID=@parent_LID and ScheduledYear=@TestYear and TicketStatus=1 order by TicketId desc)
						,[Worker]=@work
						,[fWork]=@fwork
						,[Who]=@Who
						,[Schedule]=@schedule
						,[TicketStatus]=1
						WHERE [LID]=@parent_LID	AND TestYear=@TestYear	

						select  @NextDueDate =Next, @LastDueDate=LastDue,@Last=Last from LoadTestItemHistory	WHERE [LID]=@parent_LID	AND TestYear=@TestYear

						INSERT INTO [dbo].[TestHistory]
						( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
						VALUES
						(@parent_LID ,GETDATE(),@Username ,'Assigned' ,@Last,1,null ,@TicketID, 1,@NextDueDate,@LastDueDate)

					END
				END				
				
				SET @ListTicketID=@ListTicketID+ ',' + @TicketID
			END 
				
		END
			
	END 

	SET @ListTicketID= (select top 1 Convert(varchar,TicketId) from LoadTestItemSchedule where LID=@LID and ScheduledYear=@TestYear and TicketStatus=1 order by TicketId desc)

END 
