GO
/****** Object:  StoredProcedure [dbo].[spAddRecurringTickets]    Script Date: 4/4/2018 11:48:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER  PROCEDURE [dbo].[spAddRecurringTickets] @Loc    INT,
                                              @Remarks         VARCHAR(255),
                                              @PerContract     INT,
                                              @ContractRemarks INT,
                                              @Owner           INT,
                                              @Worker          VARCHAR(50),
                                              @StartDate       DATETIME,
                                              @EndDate         DATETIME,
                                              @OnDemand        SMALLINT,
											  @FlagEN		   INT,
											  @UserID		   INT	
AS

    -------------------------###Testing--------------------------> 
 ----   Declare     
 ----   @Loc             INT =524,
 ----   @Remarks         VARCHAR(255)='',
 ----   @PerContract     INT=0,
 ----   @ContractRemarks INT=0,
 ----   @Owner           INT=82,
 ----   @Worker          VARCHAR(50)='',
 ----   @StartDate       DATETIME='1/1/2018',
 ----   @EndDate         DATETIME='1/31/2018',
 ----   @OnDemand        SMALLINT=0,
 ----   @FlagEN		     INT=0,
 ----   @UserID		     INT=0	
	-------------------------###--------------------------> 




    DECLARE @LocID INT
	DECLARE @Credit INT
    DECLARE @LocTag VARCHAR(50)
    DECLARE @LocAdd VARCHAR(100)
    DECLARE @City VARCHAR(50)
    DECLARE @State VARCHAR(2)
    DECLARE @Zip VARCHAR(100)
    DECLARE @scycle INT
    DECLARE @Phone VARCHAR(28)
    DECLARE @Cell VARCHAR(50)
    DECLARE @CallDt DATETIME
    DECLARE @SchDt DATETIME
    DECLARE @Status SMALLINT
    DECLARE @Category VARCHAR(25)
    DECLARE @Unit INT
    DECLARE @custID INT
    DECLARE @JobRemarks VARCHAR(max)
    DECLARE @job INT
    DECLARE @Comp INT
    DECLARE @unitElev VARCHAR(20)
    DECLARE @dwork VARCHAR(50)
    DECLARE @Workerstatus SMALLINT
    DECLARE @ID INT
    DECLARE @customername VARCHAR(75)
    DECLARE @locidName VARCHAR(50)
    DECLARE @locname VARCHAR(75)
	Declare @EN int
    Declare @Company varchar(100)
    DECLARE @edate DATETIME
    DECLARE @cdate DATETIME
    DECLARE @est NUMERIC(30, 2)
    DECLARE @rem VARCHAR(255)
    DECLARE @text VARCHAR(max)
    DECLARE @text1 VARCHAR(max)
    DECLARE @textProcessedTicket VARCHAR(max)
    DECLARE @WhereText VARCHAR(max)=''
    DECLARE @ExpirationDate DATETIME
    DECLARE @ctype VARCHAR(15)
    DECLARE @swe SMALLINT

	  ----### OnDemand Condition  ###------
    IF( @OnDemand = 1 )
      BEGIN
          SET @WhereText+= ' and scycle = 12'
      END
    ELSE
      BEGIN
          SET @WhereText+= ' and scycle <> 12'
      END
	    ----### location Condition  ###------
    IF( @loc <> 0 )
      BEGIN
          SET @WhereText+=' and l.loc=' + CONVERT(NVARCHAR(50), @Loc)
      END
	   ----### Owner Condition  ###------
    IF( @Owner <> 0 )
      BEGIN
          SET @WhereText+=' and l.owner='
                          + CONVERT(NVARCHAR(50), @Owner)
      END
	      ----### Company Condition  ###------
	IF(@FlagEN = 1)
		BEGIN
	set @WhereText+=' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)
		END
	  ----### Worker Condition  ###------
    IF( @Worker <> '' )
      BEGIN
          IF( @Worker = '0' )
            BEGIN
                SET @WhereText+=' and j.Custom20=0 '
            END
          ELSE
            BEGIN
                --set @WhereText+=' and (select fdesc from tblwork tw where id = (select mech from route w where  w.ID=j.Custom20))=''' +convert(nvarchar(50),@Worker)+''''
                SET @WhereText+=' and ( select top 1 name from route where id = j.Custom20 ) ='''
                                + CONVERT(NVARCHAR(50), @Worker) + ''''
            END
      END
	  ----###  Create Recurring ticket Per Equipment ###------
    IF( @PerContract = 1 )
      BEGIN
          SET @text='select distinct
l.Loc as  loc,
l.Credit,
l.Address,
l.City,
l.State,
l.Zip,
scycle,
DATEADD(DAY, DATEDIFF(DAY, 0, getdate()), 0) as calldate,'

          IF( @OnDemand = 1 )
            BEGIN
                SET @text+=' null as edate, null as scheduledt, 0 as assigned, null as worker,0 as workerstatus, null as dwork,'
            END
          ELSE
            BEGIN
                SET @text+= ' 
CAST(CAST((cast(month(SStart)as varchar(50))+''/''+CAST( day(SStart) as varchar(50) )+''/''+CAST( year(sstart) as varchar(50) )) AS DATE) AS DATETIME) + cast(CAST(STime AS TIME)as datetime) as edate,
CAST(CAST((cast(month(SStart)as varchar(50))+''/''+CAST( day(SStart) as varchar(50) )+''/''+CAST( year(sstart) as varchar(50) )) AS DATE) AS DATETIME) + cast(CAST(STime AS TIME)as datetime) as scheduledt,	
case j.Custom20 when 0 then 0 else 1 end as assigned,
(select fdesc from tblwork tw where id = (select mech from route w where  w.ID=j.Custom20)) as worker,
(select status from tblwork tw where id =(select mech from route w where  w.ID=j.Custom20)) as workerstatus,
(select fdesc from tblwork tw where id = (select mech from route w where  w.ID=j.Custom20)) as dwork,
'
            END

          SET @text+= '
''Recurring'' as category, 
jej.Elev,
l.Owner,
CONVERT(VARCHAR(MAX), j.remarks)  as jobremarks, 
''' + @Remarks + ''' as remarks,
c.job,
0 as comp,
(SELECT Unit FROM   elev WHERE  id = jej.Elev)  AS unit,                   
0 as ID,
(SELECT TOP 1 name
FROM   rol
WHERE  id = (SELECT TOP 1 rol
             FROM   owner
             WHERE  id = l.Owner)) AS customername,
l.id AS locid,
l.tag AS locname, 
r.EN,
B.Name As Company,                                 
DATEADD(DAY, DATEDIFF(DAY, 0, getdate()), 0) as cdate,             
isnull(jej.hours,0.50)  as est,
ExpirationDate,ctype, isnull(swe,0) as swe	

from Contract c
left outer join tblJoinElevJob jej on jej.Job = c.Job 
and isnull((select top 1 isnull(status,0) from elev e where id = jej.elev),0) = 0
inner join Job j on j.ID = c.Job
inner join loc l on l.loc= c.loc
LEFT JOIN Rol r on r.ID = l.Rol
LEFT JOIN Branch B on B.ID = r.EN
LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN
where l.loc is not null and j.status=0  and c.scycle<>-1  
'
          SET @text+=@WhereText
      END
    ELSE
      BEGIN
          SET @text=' 
select distinct
l.Loc as loc,
l.Credit,
l.Address,
l.City,
l.State,
l.Zip,
scycle,
DATEADD(DAY, DATEDIFF(DAY, 0, getdate()), 0) as calldate,'

          IF( @OnDemand = 1 )
            BEGIN
                SET @text+=' null as edate, null as scheduledt, 0 as assigned, null as worker,0 as workerstatus, null as dwork,'
            END
          ELSE
            BEGIN
                SET @text+= ' 
	CAST(CAST((cast(month(SStart)as varchar(50))+''/''+CAST( day(SStart) as varchar(50) )+''/''+CAST( year(sstart) as varchar(50) )) AS DATE) AS DATETIME) + cast(CAST(STime AS TIME)as datetime) as edate,
	CAST(CAST((cast(month(SStart)as varchar(50))+''/''+CAST( day(SStart) as varchar(50) )+''/''+CAST( year(sstart) as varchar(50) )) AS DATE) AS DATETIME) + cast(CAST(STime AS TIME)as datetime) as scheduledt,	
	case j.Custom20 when 0 then 0 else 1 end as assigned,
	(select fdesc from tblwork tw where id =(select mech from route w where  w.ID=j.Custom20)) as worker,
	(select status from tblwork tw where id =(select mech from route w where  w.ID=j.Custom20)) as workerstatus,
	(select fdesc from tblwork tw where id =(select mech from route w where  w.ID=j.Custom20)) as dwork,
	'
            END

          SET @text+= '''Recurring'' as category, 
null as Elev,
l.Owner,
CONVERT(VARCHAR(MAX), j.remarks)  as jobremarks,
''' + @Remarks + ''' as remarks,
j.id as job,
0 as comp,
(SELECT Unit FROM   elev WHERE  id = Elev)  AS unit,                   
0 as ID,
(SELECT TOP 1 name
FROM   rol
WHERE  id = (SELECT TOP 1 rol
             FROM   owner
             WHERE  id = l.Owner)) AS customername,
l.id AS locid,
l.tag AS locname, 
r.EN,
B.Name As Company,           
DATEADD(DAY, DATEDIFF(DAY, 0, getdate()), 0) as cdate,             
isnull(hours,0.50)  as est,
ExpirationDate, ctype, isnull(swe,0) as swe	
from Job j 
inner join loc l on l.loc=j.loc
inner join contract c on c.job=j.id
LEFT JOIN Rol r on r.ID = l.Rol
LEFT JOIN Branch B on B.ID = r.EN
LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN
where l.loc is not null and j.status=0  and c.scycle<>-1
'
          SET @text+=@WhereText
      END

    SET @text +=' order by Loc'
	 
    IF( @OnDemand <> 1 )
      BEGIN

          SET @text1= '
select distinct *  from #tempFinal 
where 
edate>= convert(datetime, '''
                      + CONVERT(VARCHAR(50), @StartDate, 101)
                      + ''') 
and edate<  convert(datetime,'''
                      + CONVERT(VARCHAR(50), Dateadd(DAY, 1, @EndDate), 101)
                      + ''')
and edate <= isnull(ExpirationDate , cast(''12/31/9999'' as datetime))  
'

           -------------------------########## Select Query #######--------------------------------
----          SET @textProcessedTicket = '
----select * from #tempWeekendFilter 
----except
----(
----select distinct tf.*  from #tempWeekendFilter tf
---- INNER JOIN ticketo t
----               ON t.Job = tf.job 
----					and isnull(t.recurring, cast(''12/31/9999'' as datetime)) = convert(datetime, convert(date,  tf.scheduledt))
----                  --and datepart(month,  t.edate)=  datepart(month,  scheduledt) 
----                  --and datepart(year,  t.edate)=  datepart(year,  scheduledt) 
----                  ----AND isnull(t.lelev,0) = isnull(tf.elev,0)
----                  ----and t.lid=tf.loc
----where 
----tf.edate>= convert(datetime, '''
----                                     + CONVERT(VARCHAR(50), @StartDate, 101)
----                                     + ''') 
----and tf.edate<  convert(datetime,'''
----                                     + CONVERT(VARCHAR(50), Dateadd(DAY, 3, @EndDate), 101)
----                                     + ''')
----and t.job <>0 
------and t.cat=''Recurring'' 
----union
----select distinct tf.*  from #tempWeekendFilter tf
---- INNER JOIN ticketd t
----               ON t.Job = tf.job 
----					and isnull(t.recurring, cast(''12/31/9999'' as datetime)) = convert(datetime, convert(date,  tf.scheduledt) )    
----                  --and datepart(month,  t.edate)=  datepart(month,  scheduledt) 
----                  --and datepart(year,  t.edate)=  datepart(year,  scheduledt) 
----                 ----AND isnull(t.elev,0) = isnull(tf.elev,0)
----                 ----and t.loc=tf.loc
----where tf.edate>= convert(datetime, '''
----                                     + CONVERT(VARCHAR(50), @StartDate, 101)
----                                     + ''') 
----and tf.edate<  convert(datetime,'''
----                                     + CONVERT(VARCHAR(50), Dateadd(DAY, 3, @EndDate), 101)
----                                     + ''')
----and t.job <>0 
------and t.cat=''Recurring'' 
----)
----order by job
----'



           ------##########  CREATE Temp TABLE ##########---------------
      
	  
	      CREATE TABLE #temp
            (
               Loc            INT,
			   Credit         INT,
               Address        VARCHAR(255),
               city           VARCHAR(50),
               state          VARCHAR(2),
               zip            VARCHAR(10),
               scycle         INT,
               calldate       DATETIME,
               edate          DATETIME,
               scheduledt     DATETIME,
               assigned       INT,
               worker         VARCHAR(50),
               Workerstatus   SMALLINT,
               dwork          VARCHAR(50),
               category       VARCHAR(50),
               Elev           INT,
               Owner          INT,
               jobremarks     VARCHAR(max),
               remarks        VARCHAR(max),
               job            INT,
               Comp           INT,
               unit           VARCHAR(20),
               ID             INT,
               customername   VARCHAR(75),
               locid          VARCHAR(50),
               locname        VARCHAR(75),
			   EN			  INT,
			   Company		  VARCHAR(100),
               cdate          DATETIME,
               est            NUMERIC(30, 2),
               ExpirationDate DATETIME,
               ctype          VARCHAR(15),
               swe            SMALLINT
            )

          CREATE TABLE #tempMonthly
            (
               Loc            INT,
			   Credit         INT,
               Address        VARCHAR(255),
               city           VARCHAR(50),
               state          VARCHAR(2),
               zip            VARCHAR(10),
               scycle         INT,
               calldate       DATETIME,
               edate          DATETIME,
               scheduledt     DATETIME,
               assigned       INT,
               worker         VARCHAR(50),
               Workerstatus   SMALLINT,
               dwork          VARCHAR(50),
               category       VARCHAR(50),
               Elev           INT,
               Owner          INT,
               jobremarks     VARCHAR(max),
               remarks        VARCHAR(max),
               job            INT,
               Comp           INT,
               unit           VARCHAR(20),
               ID             INT,
               customername   VARCHAR(75),
               locid          VARCHAR(50),
               locname        VARCHAR(75),
			   EN			  INT,
			   Company		  VARCHAR(100),
               cdate          DATETIME,
               est            NUMERIC(30, 2),
               ExpirationDate DATETIME,
               ctype          VARCHAR(15),
               swe            SMALLINT
            )

          CREATE TABLE #tempFinal
            (
               Loc            INT,
			   Credit         INT,
               Address        VARCHAR(255),
               city           VARCHAR(50),
               state          VARCHAR(2),
               zip            VARCHAR(10),
               scycle         INT,
               calldate       DATETIME,
               edate          DATETIME,
               scheduledt     DATETIME,
               assigned       INT,
               worker         VARCHAR(50),
               Workerstatus   SMALLINT,
               dwork          VARCHAR(50),
               category       VARCHAR(50),
               Elev           INT,
               Owner          INT,
               jobremarks     VARCHAR(max),
               remarks        VARCHAR(max),
               job            INT,
               Comp           INT,
               unit           VARCHAR(20),
               ID             INT,
               customername   VARCHAR(75),
               locid          VARCHAR(50),
               locname        VARCHAR(75),
			   EN			  INT,
			   Company		  VARCHAR(100),
               cdate          DATETIME,
               est            NUMERIC(30, 2),
               ExpirationDate DATETIME,
               ctype          VARCHAR(15),
               swe            SMALLINT
            )

          CREATE TABLE #tempYearly
            (
               Loc            INT,
			   Credit         INT,
               Address        VARCHAR(255),
               city           VARCHAR(50),
               state          VARCHAR(2),
               zip            VARCHAR(10),
               scycle         INT,
               calldate       DATETIME,
               edate          DATETIME,
               scheduledt     DATETIME,
               assigned       INT,
               worker         VARCHAR(50),
               Workerstatus   SMALLINT,
               dwork          VARCHAR(50),
               category       VARCHAR(50),
               Elev           INT,
               Owner          INT,
               jobremarks     VARCHAR(max),
               remarks        VARCHAR(max),
               job            INT,
               Comp           INT,
               unit           VARCHAR(20),
               ID             INT,
               customername   VARCHAR(75),
               locid          VARCHAR(50),
               locname        VARCHAR(75),
			   EN			  INT,
			   Company		  VARCHAR(100),
               cdate          DATETIME,
               est            NUMERIC(30, 2),
               ExpirationDate DATETIME,
               ctype          VARCHAR(15),
               swe            SMALLINT
            )

          CREATE TABLE #tempWeekendFilter
            (
               Loc            INT,
			   Credit         INT,
               Address        VARCHAR(255),
               city           VARCHAR(50),
               state          VARCHAR(2),
               zip            VARCHAR(10),
               scycle         INT,
               calldate       DATETIME,
               edate          DATETIME,
               scheduledt     DATETIME,
               assigned       INT,
               worker         VARCHAR(50),
               Workerstatus   SMALLINT,
               dwork          VARCHAR(50),
               category       VARCHAR(50),
               Elev           INT,
               Owner          INT,
               jobremarks     VARCHAR(max),
               remarks        VARCHAR(max),
               job            INT,
               Comp           INT,
               unit           VARCHAR(20),
               ID             INT,
               customername   VARCHAR(75),
               locid          VARCHAR(50),
               locname        VARCHAR(75),
			   EN			  INT,
			   Company		  VARCHAR(100),
               cdate          DATETIME,
               est            NUMERIC(30, 2),
               ExpirationDate DATETIME,
               ctype          VARCHAR(15),
               swe            SMALLINT
            )

          INSERT INTO #Temp
          EXEC(@text)

		  ------##########  Start CURSOR ##########---------------
          DECLARE db_cursor CURSOR FOR
            SELECT *
            FROM   #temp

          OPEN db_cursor

          FETCH NEXT FROM db_cursor INTO @locid,
		                                 @Credit,
                                         @LocAdd,
                                         @City,
                                         @State,
                                         @Zip,
                                         @scycle,
                                         @CallDt,
                                         @edate,
                                         @SchDt,
                                         @Status,
                                         @Worker,
                                         @Workerstatus,
                                         @dwork,
                                         @Category,
                                         @Unit,
                                         @custID,
                                         @JobRemarks,
                                         @rem,
                                         @job,
                                         @Comp,
                                         @unitElev,
                                         @ID,
                                         @customername,
                                         @locidName,
                                         @locname,
										 @EN,
										 @Company,
                                         @cdate,
                                         @est,
                                         @ExpirationDate,
                                         @ctype,
                                         @swe

          WHILE @@FETCH_STATUS = 0
            BEGIN
                DECLARE @FlagConst INT = 12

                IF( @scycle = 5
                     OR @scycle = 6
                     OR @scycle = 7 )  
                  BEGIN
                      SET @FlagConst=Datepart (WEEK, '12/31/2013')
                  END
                ELSE IF( @scycle = 4
                     OR @scycle = 8
                     OR @scycle = 9
                     OR @scycle = 10
                     OR @scycle = 11 )
                  BEGIN
                      SET @FlagConst= 0
                  END
                ELSE IF( @scycle = 13 )
                  BEGIN
                      SET @FlagConst= 365
                  END

                DECLARE @Flag INT = 1
                DECLARE @intFlag INT
                DECLARE @intDayFlag INT
                DECLARE @intDaily INT

                SET @intFlag = Datepart (m, @SchDt)
                SET @intDayFlag = Datepart (WEEK, @SchDt)
                SET @intDaily = Datepart (DAY, @SchDt)

                WHILE ( @Flag <= @FlagConst )
                  BEGIN
                      DECLARE @sdate DATETIME

                      IF( @scycle = 5
                           OR @scycle = 6
                           OR @scycle = 7 )
                        BEGIN
                            SET @sdate=Dateadd(WEEK, @intDayFlag - Datepart(WEEK, @SchDt), @SchDt)
                        END
                      ELSE IF( @scycle = 13 )
                        BEGIN
                            SET @sdate=Dateadd(day, @intDaily - Datepart(DAY, @SchDt), @SchDt)
                        END
                      ELSE
                        BEGIN
                            SET @sdate=Dateadd(m, @intFlag - Month(@SchDt), @SchDt)
                        END 						
						Declare @TwiceAmonthShchdate datetime;
						----###Start  twice a month Condition ###------
						IF( @scycle = 14 ) 
						BEGIN
					        SET @TwiceAmonthShchdate=@sdate;						 
                            SET @TwiceAmonthShchdate = CASE
                                           WHEN (Day(@TwiceAmonthShchdate) < 15 ) THEN 
										   Dateadd(day,14, @TwiceAmonthShchdate)
                                           ELSE 
										   Dateadd(day,-14, @TwiceAmonthShchdate)
                                           END 
						END
						----###End  twice a month Condition ###------
                      IF( CONVERT(DATE, @sdate) <= CONVERT(DATE, Dateadd (year, 1, @SchDt)) )
                        BEGIN 
                           INSERT INTO #tempMonthly
                                              (Loc,
											   Credit,
                                               Address,
                                               city,
                                               state,
                                               zip,
                                               scycle,
                                               worker,
                                               calldate,
                                               scheduledt,
                                               assigned,
                                               category,
                                               Elev,
                                               Owner,
                                               jobremarks,
                                               remarks,
                                               job,
                                               Comp,
                                               unit,
                                               dwork,
                                               ID,
                                               customername,
                                               locid,
                                               locname,
											    EN,
											   Company,
                                               edate,
                                               cdate,
                                               est,
                                               ExpirationDate,
                                               ctype,
                                               swe,
                                               Workerstatus)
                                  VALUES      ( @locid,
								                @Credit,
                                                @LocAdd,
                                                @City,
                                                @State,
                                                @Zip,
                                                @scycle,
                                                @Worker,
                                                @CallDt,
                                                @sdate,
                                                @Status,
                                                @Category,
                                                @Unit,
                                                @custID,
                                                @JobRemarks,
                                                @Remarks,
                                                @job,
                                                @Comp,
                                                @unitElev,
                                                @dwork,
                                                @ID,
                                                @customername,
                                                @locidName,
                                                @locname,
												@EN,
											    @Company,
                                                @sdate,
                                                @CallDt,
                                                @est,
                                                @ExpirationDate,
                                                @ctype,
                                                @swe,
                                                @Workerstatus ) 
                     ----###Start  twice a month Condition ###------
                        IF( @scycle = 14 )
                        BEGIN
						   INSERT INTO #tempMonthly
                                              (Loc,
											   Credit,
                                               Address,
                                               city,
                                               state,
                                               zip,
                                               scycle,
                                               worker,
                                               calldate,
                                               scheduledt,
                                               assigned,
                                               category,
                                               Elev,
                                               Owner,
                                               jobremarks,
                                               remarks,
                                               job,
                                               Comp,
                                               unit,
                                               dwork,
                                               ID,
                                               customername,
                                               locid,
                                               locname,
											   EN,
											   Company,
                                               edate,
                                               cdate,
                                               est,
                                               ExpirationDate,
                                               ctype,
                                               swe,
                                               Workerstatus)
                                  VALUES      ( @locid,
								                @Credit,
                                                @LocAdd,
                                                @City,
                                                @State,
                                                @Zip,
                                                @scycle,
                                                @Worker,
                                                @CallDt,
                                                @TwiceAmonthShchdate,
                                                @Status,
                                                @Category,
                                                @Unit,
                                                @custID,
                                                @JobRemarks,
                                                @Remarks,
                                                @job,
                                                @Comp,
                                                @unitElev,
                                                @dwork,
                                                @ID,
                                                @customername,
                                                @locidName,
												@EN,
											    @Company,
                                                @locname,
                                                @TwiceAmonthShchdate,
                                                @CallDt,
                                                @est,
                                                @ExpirationDate,
                                                @ctype,
                                                @swe,
                                                @Workerstatus ) 
                        END
                        END 
				     ----###End  twice a month Condition ###------
                      /* Monthly */
                      IF( @scycle = 0 )
                        BEGIN
                            SET @intFlag = @intFlag + 1
                        END
                      ELSE
					  IF( @scycle = 14 )----------##### 14(Twice a month) ####-----
                        BEGIN
                            SET @intFlag = @intFlag + 1
                        END
                      ELSE
                      /* Bi-Monthly */
                      IF( @scycle = 1 )
                        BEGIN
                            SET @intFlag = @intFlag + 2
                        END
                      ELSE
                      /* Quarterly */
                      IF( @scycle = 2 )
                        BEGIN
                            SET @intFlag = @intFlag + 3
                        END
                      ELSE
                      /* Semiannually */
                      IF( @scycle = 3 )
                        BEGIN
                            SET @intFlag = @intFlag + 6
                        END
                      --else
                      /* Annually */
                      --if(@scycle=4)
                      --begin
                      --SET @intFlag = @intFlag + 12
                      --end
                      /* weekly */
                      ELSE IF( @scycle = 5 )
                        BEGIN
                            SET @intDayFlag = @intDayFlag + 1
                        END
                      /* Bi-weekly */
                      ELSE IF( @scycle = 6)
                        BEGIN
                            SET @intDayFlag = @intDayFlag + 2
                        END
                      /* 13-weekly */
                      ELSE IF( @scycle = 7 )
                        BEGIN
                            SET @intDayFlag = @intDayFlag + 13
                        END
                      /* Daily */
                      ELSE IF( @scycle = 13 )
                        BEGIN
                            SET @intDaily = @intDaily + 1
                        END

                      SET @Flag = @Flag + 1
                  END

                FETCH NEXT FROM db_cursor INTO @locid,
				                               @Credit,
                                               @LocAdd,
                                               @City,
                                               @State,
                                               @Zip,
                                               @scycle,
                                               @CallDt,
                                               @edate,
                                               @SchDt,
                                               @Status,
                                               @Worker,
                                               @Workerstatus,
                                               @dwork,
                                               @Category,
                                               @Unit,
                                               @custID,
                                               @JobRemarks,
                                               @rem,
                                               @job,
                                               @Comp,
                                               @unitElev,
                                               @ID,
                                               @customername,
                                               @locidName,
                                               @locname,
											   @EN,
											   @Company,
                                               @cdate,
                                               @est,
                                               @ExpirationDate,
                                               @ctype,
                                               @swe
            END

          CLOSE db_cursor

          DEALLOCATE db_cursor

		   ------##########  Close CURSOR ##########---------------
          INSERT INTO #tempFinal
          SELECT *
          FROM   #temp t
          UNION
          SELECT *
          FROM   #tempMonthly

          DROP TABLE #temp

          DROP TABLE #tempMonthly

          DECLARE db_cursor CURSOR FOR
            SELECT *
            FROM   #tempFinal

          OPEN db_cursor

          FETCH NEXT FROM db_cursor INTO @locid,
		                                 @Credit,
                                         @LocAdd,
                                         @City,
                                         @State,
                                         @Zip,
                                         @scycle,
                                         @CallDt,
                                         @edate,
                                         @SchDt,
                                         @Status,
                                         @Worker,
                                         @Workerstatus,
                                         @dwork,
                                         @Category,
                                         @Unit,
                                         @custID,
                                         @JobRemarks,
                                         @rem,
                                         @job,
                                         @Comp,
                                         @unitElev,
                                         @ID,
                                         @customername,
                                         @locidName,
                                         @locname,
										 @EN,
										 @Company,
                                         @cdate,
                                         @est,
                                         @ExpirationDate,
                                         @ctype,
                                         @swe

          WHILE @@FETCH_STATUS = 0
            BEGIN
                DECLARE @IntYear INT =Datepart (YEAR, @SchDt)

                WHILE ( @IntYear <= Datepart (YEAR, @EndDate) )
                  BEGIN
                      IF( @scycle = 8 )
                        BEGIN
                            SET @IntYear = @IntYear + 3
                        END
                      ELSE IF( @scycle = 9 )
                        BEGIN
                            SET @IntYear = @IntYear + 5
                        END
                      ELSE IF( @scycle = 10 )
                        BEGIN
                            SET @IntYear = @IntYear + 2
                        END
                      ELSE IF( @scycle = 11 )
                        BEGIN
                            SET @IntYear = @IntYear + 7
                        END
                      ELSE
                        BEGIN
                            SET @IntYear = @IntYear + 1
                        END

                      SET @sdate=Dateadd(YEAR, @IntYear - Year(@SchDt), @SchDt)

                      INSERT INTO #tempYearly
                                  (Loc,
								   Credit,
                                   Address,
                                   city,
                                   state,
                                   zip,
                                   scycle,
                                   worker,
                                   calldate,
                                   scheduledt,
                                   assigned,
                                   category,
                                   Elev,
                                   Owner,
                                   jobremarks,
                                   remarks,
                                   job,
                                   Comp,
                                   unit,
                                   dwork,
                                   ID,
                                   customername,
                                   locid,
                                   locname,
								   EN,
								   Company,
                                   edate,
                                   cdate,
                                   est,
                                   ExpirationDate,
                                   ctype,
                                   swe,
                                   Workerstatus)
                      VALUES      ( @locid,
					                @Credit,
                                    @LocAdd,
                                    @City,
                                    @State,
                                    @Zip,
                                    @scycle,
                                    @Worker,
                                    @CallDt,
                                    @sdate,
                                    @Status,
                                    @Category,
                                    @Unit,
                                    @custID,
                                    @JobRemarks,
                                    @Remarks,
                                    @job,
                                    @Comp,
                                    @unitElev,
                                    @dwork,
                                    @ID,
                                    @customername,
                                    @locidName,
                                    @locname,
									@EN,
									@Company,
                                    @sdate,
                                    @CallDt,
                                    @est,
                                    @ExpirationDate,
                                    @ctype,
                                    @swe,
                                    @Workerstatus )
                  END

                FETCH NEXT FROM db_cursor INTO @locid,
				                               @Credit,
                                               @LocAdd,
                                               @City,
                                               @State,
                                               @Zip,
                                               @scycle,
                                               @CallDt,
                                               @edate,
                                               @SchDt,
                                               @Status,
                                               @Worker,
                                               @Workerstatus,
                                               @dwork,
                                               @Category,
                                               @Unit,
                                               @custID,
                                               @JobRemarks,
                                               @rem,
                                               @job,
                                               @Comp,
                                               @unitElev,
                                               @ID,
                                               @customername,
                                               @locidName,
                                               @locname,
											   @EN,
											   @Company,
                                               @cdate,
                                               @est,
                                               @ExpirationDate,
                                               @ctype,
                                               @swe
            END

          CLOSE db_cursor

          DEALLOCATE db_cursor

          INSERT INTO #tempFinal
          SELECT *
          FROM   #tempYearly

          DROP TABLE #tempYearly

          INSERT INTO #tempWeekendFilter
          EXEC(@text1)

          DROP TABLE #tempFinal

		  ----###Start  Shift weekend ticket On Friday when day > 25 else Monday ###------
          UPDATE #tempWeekendFilter
          SET    scheduledt = case when (Datepart(day, scheduledt)) > 25
		 then (
		   CASE Datepart(WEEKDAY, scheduledt)
                                WHEN 1 THEN Dateadd (DAY, -2, scheduledt)
                                WHEN 7 THEN Dateadd (DAY, -1, scheduledt)
                                ELSE scheduledt
                              END)
							  
							  else (CASE Datepart(WEEKDAY, scheduledt)
                                WHEN 1 THEN Dateadd (DAY, 1, scheduledt)
                                WHEN 7 THEN Dateadd (DAY, 2, scheduledt)
                                ELSE scheduledt
                              END)
							  end
							  ,
                 edate = case when ( Datepart(day, edate)) > 25
				 then (CASE Datepart(WEEKDAY, edate)
                           WHEN 1 THEN Dateadd (DAY, -2, edate)
                           WHEN 7 THEN Dateadd (DAY, -1, edate)
                           ELSE edate
                         END)
						 else(CASE Datepart(WEEKDAY, edate)
                           WHEN 1 THEN Dateadd (DAY, 1, edate)
                           WHEN 7 THEN Dateadd (DAY, 2, edate)
                           ELSE edate
                         END)

						 end
          WHERE  swe = 0
		  ----###End  Shift weekend ticket On Friday when day > 25 else Monday ###------

          --select * from #tempWeekendFilter order by job
         -- EXEC (@textProcessedTicket)



----------------------------------------------------------------------------------------------------------####
 

 ------------------Final Select QUERY When On Demand <> 1-----------------

------- If PER Equipment Checked
 IF( @PerContract = 1 )
 SELECT 
	( case when exists (
	   select Top 1 t.id  from ticketo t where t.Job = tf.job 
       and isnull(t.recurring, cast('12/31/9999' as datetime)) = convert(datetime, convert(date,  tf.scheduledt))
       and t.LElev=tf.Elev 
	   )
	   then ( 
	   select Top 1 t.id  from ticketo t where t.Job = tf.job 
       and isnull(t.recurring, cast('12/31/9999' as datetime)) = convert(datetime, convert(date,  tf.scheduledt))
       and t.LElev=tf.Elev
	   )
	   else 
	   (
	   select Top 1 t.id  from TicketD t where t.Job = tf.job 
       and isnull(t.recurring, cast('12/31/9999' as datetime)) = convert(datetime, convert(date,  tf.scheduledt))
       and t.Elev=tf.Elev 
	   ) 
	   end
    )  
 as	TicketID, 
  '' TempEquipmentstr,
 * from #tempWeekendFilter tf 
  Else

  BEGIN 
   SELECT 
    (  
	   [dbo].[GetAlreadyCreatedTicketInCSV](tf.job,tf.scheduledt,'Ticket') 
    )  
 as	TicketID,  
   (	
       [dbo].[GetAlreadyCreatedTicketInCSV](tf.job,tf.scheduledt,'Equip')  
    )  
 as	TempEquipmentstr, 
 * from #tempWeekendFilter tf 
	
  END

----------------------------------------------------------------------------------------------------------#####


          DROP TABLE #tempWeekendFilter
      END
    ELSE
	 ----###  Create Recurring ticket On Demand ###------
      BEGIN
          CREATE TABLE #tempWeekendFilter1
            (
               Loc            INT,
			   Credit         INT,
               Address        VARCHAR(255),
               city           VARCHAR(50),
               state          VARCHAR(2),
               zip            VARCHAR(10),
               scycle         INT,
               calldate       DATETIME,
               edate          DATETIME,
               scheduledt     DATETIME,
               assigned       INT,
               worker         VARCHAR(50),
               workerstatus   SMALLINT,
               dwork          VARCHAR(50),
               category       VARCHAR(50),
               Elev           INT,
               Owner          INT,
               jobremarks     VARCHAR(max),
               remarks        VARCHAR(max),
               job            INT,
               Comp           INT,
               unit           VARCHAR(20),
               ID             INT,
               customername   VARCHAR(75),
               locid          VARCHAR(50),
               locname        VARCHAR(75),
			    EN			  INT,
			   Company		  VARCHAR(100),
               cdate          DATETIME,
               est            NUMERIC(30, 2),
               ExpirationDate DATETIME,
               ctype          VARCHAR(15),
               swe            SMALLINT
            )

          INSERT INTO #tempWeekendFilter1
          EXEC(@text)
		  
		    ----###Start  Shift weekend ticket On Friday when day > 25 else Monday ###------

          UPDATE #tempWeekendFilter1
          SET    scheduledt = CASE when Datepart(WEEKDAY, scheduledt) > 25
		  then (CASE Datepart(WEEKDAY, scheduledt)
                                WHEN 1 THEN Dateadd (DAY, -2, scheduledt)
                                WHEN 7 THEN Dateadd (DAY, -1, scheduledt)
                                ELSE scheduledt
                              END)
							  else 
							  (CASE Datepart(WEEKDAY, scheduledt)
                                WHEN 1 THEN Dateadd (DAY, 1, scheduledt)
                                WHEN 7 THEN Dateadd (DAY, 2, scheduledt)
                                ELSE scheduledt
                              END)
							  end ,

                 edate =case  when  Datepart(DAY, edate) > 25 
				 then 
				  (CASE Datepart(WEEKDAY, edate)
                           WHEN 1 THEN Dateadd (DAY, -2, edate)
                           WHEN 7 THEN Dateadd (DAY, -1, edate)
                           ELSE edate
                         END)
						 else (CASE Datepart(WEEKDAY, edate)
                           WHEN 1 THEN Dateadd (DAY, 1, edate)
                           WHEN 7 THEN Dateadd (DAY, 2, edate)
                           ELSE edate
                         END)
						 end 
				   
          WHERE  swe = 0

		    ----###End  Shift weekend ticket On Friday when day > 25 else Monday ###------

         ------------------Final Select QUERY When On Demand = 1-----------------
		  SELECT 
		  '' as	TicketID, 
          '' TempEquipmentstr,*
          FROM   #tempWeekendFilter1
          ORDER  BY job
         -----------------------------------------
          DROP TABLE #tempWeekendFilter1
      END

