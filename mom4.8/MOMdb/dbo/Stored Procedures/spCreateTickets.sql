Create PROCEDURE [dbo].[spCreateTickets]

			@LID int,
			@Username nvarchar(100)
		
as
	begin
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

			DECLARE @TicketID INT
			DECLARE @WorkOrder    VARCHAR(10)
			SELECT @TicketID = Max([NewID]) + 1
								FROM   (SELECT Isnull(Max(TicketO.ID), 0) AS [NewID]
								FROM   TicketO
								UNION ALL
								SELECT Isnull(Max(TicketD.ID), 0) AS [NewID]
								FROM   TicketD) A
			
   

			IF( @WorkOrder = '' )
			  BEGIN
				  SET @WorkOrder = @TicketID
			  END

			

			IF ((SELECT COUNT(LoadTestItem.LID) cSkip FROM LoadTestItem INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc WHERE Loc.Credit=1 AND LoadTestItem.LID=@LID)=0  
				AND(SELECT COUNT(LID) cSkip FROM LoadTestItem WHERE ISNULL(Ticket ,0)>0 AND LoadTestItem.LID=@LID)=0)
				BEGIN  

						IF ((SELECT count(*) FROM TicketO WHERE TicketO.ID=@TicketID)=0)
							BEGIN
								INSERT INTO #TEMPLOADTEST
								SELECT LoadTestItem.LID, LoadTest.ID AS TID, LoadTestItem.Next, Rol.Name, Loc.Tag, Loc.Loc, Loc.ID, Loc.Route, Elev.Unit, Elev.ID as NID, 
									   LoadTest.Name as NTest, LoadTestItem.Last, LoadTestItem.Status,LoadTestItem.idRolCustomContact,Elev.fDesc,Elev.State,Rol.EN,
									   w.ID idWorker, w.fDesc CallSign, r.Id idRoute, r.Name RouteName,LoadTest.Level,LoadTest.Cat,LoadTest.Frequency,LoadTest.Authority 
									   
									   FROM ((((LoadTestItem INNER JOIN Elev ON LoadTestItem.Elev = Elev.ID) INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc)
										INNER JOIN Owner ON Loc.Owner = Owner.ID) INNER JOIN Rol ON Owner.Rol = Rol.ID) 
										INNER JOIN LoadTest ON LoadTestItem.ID = LoadTest.ID 
										left  JOIN Route r ON Loc.Route=r.ID 
										left  JOIN tblWork w ON r.Mech = w.ID
										WHERE LoadTestItem.LID=@LID
								IF EXISTS(SELECT 1 FROM #TEMPLOADTEST)
									BEGIN
									
											 

											SELECT l.*,isnull(j.id,0) idMaintJob 
											into #templocjob
											FROM Loc l
											INNER JOIN #TEMPLOADTEST ON l.Loc=#TEMPLOADTEST.LOC
											 LEFT JOIN Job j ON l.Loc=j.Loc AND j.Type=0 AND j.Status=0 and j.elev=#TEMPLOADTEST.ELEVID

											  INSERT INTO [TicketO]
															   (
															    [ID]
															   ,[CDate]
															   ,[DDate]
															   ,[EDate]
															   ,[Level]
															   ,[Est]
															   ,[fWork]
															   ,[DWork]
															   ,[Type]
															   ,[Cat]
															   ,[fDesc]
															   ,[Who]
															   ,[fBy]
															   ,[LType]
															   ,[LID]
															   ,[LElev]
															   ,[LDesc1]
															   ,[LDesc2]
															   ,[LDesc3]
															   ,[LDesc4]
															   ,[Nature]
															   ,[Job]
															   ,[Assigned]
															   ,[City]
															   ,[State]
															   ,[Zip]
															   ,[Owner]
															   ,[Route]
															   ,[Terr]
															   ,[fLong]
															   ,[Latt]
															   ,[CallIn]
															   ,[SpecType]
															   ,[SpecID]
															   ,[EN]															  
															   ,[WorkOrder]															 
															   ,[is_work_order]
															 )

															   (
															   SELECT @TicketID,convert(varchar, getdate(), 121),convert(varchar, getdate(), 121),convert(varchar, getdate(), 121),
															   #TEMPLOADTEST.Level,1.00,#TEMPLOADTEST.idWorker,#TEMPLOADTEST.CallSign,0,#TEMPLOADTEST.Cat,

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
														from #TEMPLOADTEST inner join #templocjob on #TEMPLOADTEST.LOC=#templocjob.Loc)
												 
									END
							END 
				END 


		if exists(SELECT TicketO.ID FROM TicketO WHERE TicketO.ID=@TicketID)
			select @TicketID
		else
			select 0 
	end