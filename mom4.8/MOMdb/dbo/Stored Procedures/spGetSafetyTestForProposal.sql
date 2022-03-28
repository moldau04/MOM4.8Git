CREATE  PROCEDURE [dbo].[spGetSafetyTestForProposal]	
			@Stdate datetime=NULL,
			@Endate datetime=NULL,			
			@TestType int=null,
			@Cols nvarchar(100)=null,
			@SearchVal nvarchar(250)=null,
			@FlagEN int = 0,
			@UserID int	= 0		
as
	begin
		DECLARE @SQLQuery AS NVARCHAR(500)

		CREATE TABLE #TEMPSAFETYDETAILS
		(
			LTest INT NULL,
			LID INT NULL,
			Name  VARCHAR(300) NULL,
			Tag  VARCHAR(300) NULL,
			Loc int NULL,
			ID  VARCHAR(300) NULL,
			Unit  VARCHAR(300) NULL,
			NID int NULL,
			NTest  VARCHAR(300) NULL,
			LTID int NULL,
			Last NVARCHAR(100),
			LastDue DATETIME NULL,
			Next DATETIME NULL,
			StatusValue INT NULL,
			Status NVARCHAR(100),
			Custom1  VARCHAR(300) NULL, Custom2  VARCHAR(300) NULL, Custom3  VARCHAR(300) NULL, Custom4  VARCHAR(300) NULL, Custom5  VARCHAR(300) NULL, 
				Custom6  VARCHAR(300) NULL, Custom7  VARCHAR(300) NULL, Custom8  VARCHAR(300) NULL, Custom9  VARCHAR(300) NULL, Custom10  VARCHAR(300) NULL,
				idRolCustomContact INT NULL,
				Remarks NTEXT NULL,
				State  VARCHAR(300) NULL,
				Route INT NULL,
				idTicket NVARCHAR(300) NULL,
				TicketStatus INT NULL,
				TicketStatusText NVARCHAR(300) NULL,
				idWorker INT NULL,
				EDate NVARCHAR(100) NULL,
				CallSign VARCHAR(100) NULL,
				EN INT NULL,
				Company	VARCHAR(100) NULL,
				Address VARCHAR(200) NULL,
				Jobid int null,
				Classification NVARCHAR(100) null,
				ThirdPartyName VARCHAR(200) NULL,
				Chargeable int null,			
				
		)

IF(@FlagEN = 1)
BEGIN
		IF (@Stdate IS NOT NULL AND @Endate IS NOT NULL)
			BEGIN
					IF(@TestType=0 OR @TestType IS NULL)
						INSERT INTO #TEMPSAFETYDETAILS
						SELECT Job.Charge as LTest, LoadTestItem.LID,
							Rol.Name, Loc.Tag, Loc.Loc, Loc.ID, 
							Elev.Unit, Elev.ID as NID, 
							LoadTest.Name as NTest, LoadTest.ID as LTID ,
							isnull(convert(varchar(10),LoadTestItem.Last,101),'')Last, 
							LoadTestItem.LastDue,isnull( convert(varchar(10),LoadTestItem.Next,101),'')Next,
							 LoadTestItem.Status StatusValue, ListConfig.ItemName Status       ,
							LoadTestItem.Custom1, LoadTestItem.Custom2, LoadTestItem.Custom3, LoadTestItem.Custom4, LoadTestItem.Custom5, 
							LoadTestItem.Custom6, LoadTestItem.Custom7, LoadTestItem.Custom8, LoadTestItem.Custom9, LoadTestItem.Custom10,
							LoadTestItem.idRolCustomContact, LoadTestItem.Remarks,Elev.State,Loc.Route, 
							isnull(cast (TicketO.ID as nvarchar),'')idTicket, TicketO.Assigned TicketStatus,  
							isnull(dbo.TicketStatusAsText(TicketO.Assigned),'') TicketStatusText,TicketO.fWork idWorker,
							isnull( convert(varchar(10),TicketO.EDate,101),'')	EDate,isnull(tblWork.fDesc,'') CallSign,Rol.EN,ISNULL(Branch.Name,'')Company,
							 Loc.Address,LoadTestItem.JobId ,Elev.Classification,LoadTestItem.ThirdPartyName,LoadTestItem.Chargeable
						
							FROM LoadTestItem       INNER JOIN Elev ON LoadTestItem.Elev = Elev.ID       INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc   
							INNER JOIN Owner ON Loc.Owner = Owner.ID       INNER JOIN Rol ON Owner.Rol = Rol.ID       INNER JOIN LoadTest ON LoadTestItem.ID = LoadTest.ID    
							INNER JOIN ListConfig ON ListConfig.ListName='Test.Status' AND LoadTestItem.Status = ListConfig.ItemValue      
							INNER JOIN LatestInstanceOfEachStatusPerTest x ON LoadTestItem.LID=x.idTest AND LoadTestItem.Status=x.idTestStatus and x.StatusRank = 1     
							LEFT JOIN Job ON Loc.Job=Job.ID       LEFT JOIN TicketO ON LoadTestItem.Ticket=TicketO.Id      
							LEFT JOIN tblWork ON TicketO.fWork=tblWork.ID  LEFT JOIN Branch on Branch.ID = Rol.EN LEFT JOIN tblUserCo on tblUserCo.CompanyID = Rol.EN
							
							WHERE LoadTestItem.Next>=@Stdate AND LoadTestItem.Next<=@Endate  
							AND Loc.Status <> 1 AND Elev.Status=0 AND tblUserCo.IsSel = 1 AND tblUserCo.UserID = @UserID  
							ORDER BY Rol.Name, Loc.Tag, LoadTestItem.Elev, LoadTestItem.ID
					ELSE
						INSERT INTO #TEMPSAFETYDETAILS 
						SELECT Job.Charge as LTest, LoadTestItem.LID,
						Rol.Name, Loc.Tag, Loc.Loc, Loc.ID, 
						Elev.Unit, Elev.ID as NID, 
						LoadTest.Name as NTest, LoadTest.ID as LTID ,
						isnull(convert(varchar(10),LoadTestItem.Last,101),'')Last, 
						LoadTestItem.LastDue,isnull( convert(varchar(10),LoadTestItem.Next,101),'')Next,
						 LoadTestItem.Status StatusValue, ListConfig.ItemName Status       ,
						LoadTestItem.Custom1, LoadTestItem.Custom2, LoadTestItem.Custom3, LoadTestItem.Custom4, LoadTestItem.Custom5, 
						LoadTestItem.Custom6, LoadTestItem.Custom7, LoadTestItem.Custom8, LoadTestItem.Custom9, LoadTestItem.Custom10,
						LoadTestItem.idRolCustomContact, LoadTestItem.Remarks,Elev.State,Loc.Route, 
						isnull(cast (TicketO.ID as nvarchar),'')idTicket, TicketO.Assigned TicketStatus,  
						isnull(dbo.TicketStatusAsText(TicketO.Assigned),'') TicketStatusText,TicketO.fWork idWorker,
						isnull( convert(varchar(10),TicketO.EDate,101),'')	EDate,isnull(tblWork.fDesc,'') CallSign,Rol.EN, ISNULL(Branch.Name,'')Company,
						Loc.Address,LoadTestItem.JobId,Elev.Classification ,LoadTestItem.ThirdPartyName,LoadTestItem.Chargeable
			
						FROM LoadTestItem       INNER JOIN Elev ON LoadTestItem.Elev = Elev.ID       INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc   
						INNER JOIN Owner ON Loc.Owner = Owner.ID       INNER JOIN Rol ON Owner.Rol = Rol.ID       INNER JOIN LoadTest ON LoadTestItem.ID = LoadTest.ID    
						INNER JOIN ListConfig ON ListConfig.ListName='Test.Status' AND LoadTestItem.Status = ListConfig.ItemValue      
						INNER JOIN LatestInstanceOfEachStatusPerTest x ON LoadTestItem.LID=x.idTest AND LoadTestItem.Status=x.idTestStatus and x.StatusRank = 1     
						LEFT JOIN Job ON Loc.Job=Job.ID       LEFT JOIN TicketO ON LoadTestItem.Ticket=TicketO.Id      
						LEFT JOIN tblWork ON TicketO.fWork=tblWork.ID LEFT JOIN Branch on Branch.ID = Rol.EN LEFT JOIN tblUserCo on tblUserCo.CompanyID = Rol.EN
						
						WHERE LoadTestItem.Next>=@Stdate AND LoadTestItem.Next<=@Endate  
						AND Loc.Status <> 1 AND Elev.Status=0 AND tblUserCo.IsSel = 1 AND tblUserCo.UserID = @UserID AND LoadTest.ID=@TestType 
						
						ORDER BY Rol.Name, Loc.Tag, LoadTestItem.Elev, LoadTestItem.ID
						
			END
END

ELSE
BEGIN
IF (@Stdate IS NOT NULL AND @Endate IS NOT NULL)
			BEGIN
					IF(@TestType=0 OR @TestType IS NULL)
						INSERT INTO #TEMPSAFETYDETAILS
						SELECT Job.Charge as LTest, LoadTestItem.LID,
							Rol.Name, Loc.Tag, Loc.Loc, Loc.ID, 
							Elev.Unit, Elev.ID as NID, 
							LoadTest.Name as NTest, LoadTest.ID as LTID ,
							isnull(convert(varchar(10),LoadTestItem.Last,101),'')Last, 
							LoadTestItem.LastDue,isnull( convert(varchar(10),LoadTestItem.Next,101),'')Next,
							 LoadTestItem.Status StatusValue, ListConfig.ItemName Status       ,
							LoadTestItem.Custom1, LoadTestItem.Custom2, LoadTestItem.Custom3, LoadTestItem.Custom4, LoadTestItem.Custom5, 
							LoadTestItem.Custom6, LoadTestItem.Custom7, LoadTestItem.Custom8, LoadTestItem.Custom9, LoadTestItem.Custom10,
							LoadTestItem.idRolCustomContact, LoadTestItem.Remarks,Elev.State,Loc.Route, 
							isnull(cast (TicketO.ID as nvarchar),'')idTicket, TicketO.Assigned TicketStatus,  
							isnull(dbo.TicketStatusAsText(TicketO.Assigned),'') TicketStatusText,TicketO.fWork idWorker,
							isnull( convert(varchar(10),TicketO.EDate,101),'')	EDate,isnull(tblWork.fDesc,'') CallSign,Rol.EN,ISNULL(Branch.Name,'')Company,
							 Loc.Address,LoadTestItem.JobId,Elev.Classification ,LoadTestItem.ThirdPartyName,LoadTestItem.Chargeable
							
							FROM LoadTestItem       INNER JOIN Elev ON LoadTestItem.Elev = Elev.ID       INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc   
							INNER JOIN Owner ON Loc.Owner = Owner.ID       INNER JOIN Rol ON Owner.Rol = Rol.ID       INNER JOIN LoadTest ON LoadTestItem.ID = LoadTest.ID    
							INNER JOIN ListConfig ON ListConfig.ListName='Test.Status' AND LoadTestItem.Status = ListConfig.ItemValue      
							INNER JOIN LatestInstanceOfEachStatusPerTest x ON LoadTestItem.LID=x.idTest AND LoadTestItem.Status=x.idTestStatus and x.StatusRank = 1     
							LEFT JOIN Job ON Loc.Job=Job.ID       LEFT JOIN TicketO ON LoadTestItem.Ticket=TicketO.Id      
							LEFT JOIN tblWork ON TicketO.fWork=tblWork.ID  LEFT JOIN Branch on Branch.ID = Rol.EN 
							
							WHERE LoadTestItem.Next>=@Stdate AND LoadTestItem.Next<=@Endate  
							AND Loc.Status <> 1 AND Elev.Status=0 
							
							ORDER BY Rol.Name, Loc.Tag, LoadTestItem.Elev, LoadTestItem.ID
					ELSE
						INSERT INTO #TEMPSAFETYDETAILS 
						SELECT Job.Charge as LTest, LoadTestItem.LID,
						Rol.Name, Loc.Tag, Loc.Loc, Loc.ID, 
						Elev.Unit, Elev.ID as NID, 
						LoadTest.Name as NTest, LoadTest.ID as LTID ,
						isnull(convert(varchar(10),LoadTestItem.Last,101),'')Last, 
						LoadTestItem.LastDue,isnull( convert(varchar(10),LoadTestItem.Next,101),'')Next,
						 LoadTestItem.Status StatusValue, ListConfig.ItemName Status       ,
						LoadTestItem.Custom1, LoadTestItem.Custom2, LoadTestItem.Custom3, LoadTestItem.Custom4, LoadTestItem.Custom5, 
						LoadTestItem.Custom6, LoadTestItem.Custom7, LoadTestItem.Custom8, LoadTestItem.Custom9, LoadTestItem.Custom10,
						LoadTestItem.idRolCustomContact, LoadTestItem.Remarks,Elev.State,Loc.Route, 
						isnull(cast (TicketO.ID as nvarchar),'')idTicket, TicketO.Assigned TicketStatus,  
						isnull(dbo.TicketStatusAsText(TicketO.Assigned),'') TicketStatusText,TicketO.fWork idWorker,
						isnull( convert(varchar(10),TicketO.EDate,101),'')	EDate,isnull(tblWork.fDesc,'') CallSign,Rol.EN, ISNULL(Branch.Name,'')Company,
						Loc.Address,LoadTestItem.JobId ,Elev.Classification,LoadTestItem.ThirdPartyName,LoadTestItem.Chargeable
						
						FROM LoadTestItem       INNER JOIN Elev ON LoadTestItem.Elev = Elev.ID       INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc   
						INNER JOIN Owner ON Loc.Owner = Owner.ID       INNER JOIN Rol ON Owner.Rol = Rol.ID       INNER JOIN LoadTest ON LoadTestItem.ID = LoadTest.ID    
						INNER JOIN ListConfig ON ListConfig.ListName='Test.Status' AND LoadTestItem.Status = ListConfig.ItemValue      
						INNER JOIN LatestInstanceOfEachStatusPerTest x ON LoadTestItem.LID=x.idTest AND LoadTestItem.Status=x.idTestStatus and x.StatusRank = 1     
						LEFT JOIN Job ON Loc.Job=Job.ID       LEFT JOIN TicketO ON LoadTestItem.Ticket=TicketO.Id      
						LEFT JOIN tblWork ON TicketO.fWork=tblWork.ID LEFT JOIN Branch on Branch.ID = Rol.EN 
						
						WHERE LoadTestItem.Next>=@Stdate AND LoadTestItem.Next<=@Endate  
						AND Loc.Status <> 1 AND Elev.Status=0 AND LoadTest.ID=@TestType
					
						ORDER BY Rol.Name, Loc.Tag, LoadTestItem.Elev, LoadTestItem.ID
						
			END
END		


			DECLARE @ProposoalCondition varchar(200)


		if(@Cols is not null and @SearchVal is not null)	
			SET @SQLQuery = 'SELECT * FROM #TEMPSAFETYDETAILS WHERE ' +@Cols+' LIKE (''%'+ @SearchVal+'%'')'
        else if(@Cols is null and @SearchVal is not null)
			begin
					if exists( SELECT TOP 1 1 FROM #TEMPSAFETYDETAILS WHERE #TEMPSAFETYDETAILS.Name LIKE('%'+ @SearchVal+'%'))
						SET @SQLQuery = 'SELECT * FROM #TEMPSAFETYDETAILS WHERE Name LIKE (''%'+ @SearchVal+'%'')' 
					else if exists(SELECT TOP 1 1 FROM #TEMPSAFETYDETAILS WHERE #TEMPSAFETYDETAILS.ID LIKE('%'+ @SearchVal+'%'))
						SET @SQLQuery = 'SELECT * FROM #TEMPSAFETYDETAILS WHERE ID LIKE (''%'+ @SearchVal+'%'')'
					else if exists(SELECT TOP 1 1 FROM #TEMPSAFETYDETAILS WHERE #TEMPSAFETYDETAILS.Tag LIKE('%'+ @SearchVal+'%'))
						SET @SQLQuery = 'SELECT * FROM #TEMPSAFETYDETAILS WHERE Tag LIKE (''%'+ @SearchVal+'%'')' 
					else if exists(SELECT TOP 1 1 FROM #TEMPSAFETYDETAILS WHERE #TEMPSAFETYDETAILS.Company LIKE('%'+ @SearchVal+'%'))
						SET @SQLQuery = 'SELECT * FROM #TEMPSAFETYDETAILS WHERE Company LIKE (''%'+ @SearchVal+'%'')' 
					else 
						SET @SQLQuery = 'SELECT * FROM #TEMPSAFETYDETAILS WHERE NID LIKE (''%'+ @SearchVal+'%'')'
			end
		else
			SET @SQLQuery = 'SELECT * FROM #TEMPSAFETYDETAILS' 

                
            execute(@SQLQuery)

	DROP TABLE #TEMPSAFETYDETAILS

	end