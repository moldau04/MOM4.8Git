CREATE PROCEDURE [dbo].[spGetSafetyTestDetails]			
         		
			@Stdate datetime=NULL,
			@Endate datetime=NULL,			
			@TestType int=null,
			@Cols nvarchar(100)=null,
			@SearchVal nvarchar(250)=null,
			@FlagEN int = 0,
			@UserID int	= 0,
			@Proposal VARCHAR(50),
			@strfilter VARCHAR(Max),
			@SortOrderBy varchar(200) ='LID',
			@SortType varchar(200) ='desc'

AS
BEGIN 
--Declare @TestType int=null
Declare @StartDate Datetime
Declare @EndDate Datetime
Declare @startyear int
Declare @Endyear int


SET @StartDate=@Stdate
SET @EndDate=@Endate
--set @TestType=13
IF OBJECT_ID('tempdb..#TEMPSAFETYDETAILS') IS NOT NULL DROP TABLE  #TEMPSAFETYDETAILS
CREATE TABLE #TEMPSAFETYDETAILS
(
SafetyTestID INT IDENTITY(1, 1),
LID INT NULL,
Last DateTime,
Next varchar(10),
HasChild int,
--TestType
NTest  VARCHAR(300) NULL,
LTID int NULL,
IsCoverTestType INT NULL,

--Location
Name  VARCHAR(300) NULL,
Tag  VARCHAR(300) NULL,
Loc int NULL,
ID  VARCHAR(300) NULL,
LocState VARCHAR(100),
City VARCHAR(100),
EN INT NULL,
Company	VARCHAR(100) NULL,
Address VARCHAR(200) NULL,
RouteName VARCHAR(500) NULL,
RecurringContract VARCHAR(10),

--Elev
Elev Int,
Unit  VARCHAR(300) NULL,
NID int NULL,
State  VARCHAR(300) NULL,
Classification NVARCHAR(100) null,
Capacity VARCHAR(100) NULL, 

--Price
BillingAmount NUMERIC (32,2),
IsDefaultAmount varchar(20),
ThirdPartyName VARCHAR(200) NULL,
Chargeable int null,
ChargeableName VARCHAR (10),

--Item detail
idTicket NVARCHAR(300) NULL,
TicketStatus INT NULL,
TicketStatusText NVARCHAR(300) NULL,
idWorker INT NULL,
EDate NVARCHAR(100) NULL,
CallSign VARCHAR(100) NULL,
StatusValue int,
Status NVARCHAR(100),
--Proposal
ProposalID INT,
ProposalStatus VARCHAR(50) NULL,
SendEmailStatusID INT,
SendMailStatus  VARCHAR(50) NULL,
PDFFilePath VARCHAR(500) NULL,
--ScheduledDate VARCHAR(MAX),
ScheduledStatus INT,
--ScheduledWorker VARCHAR(MAX),	
--ServiceDate VARCHAR(MAX),
--ServiceStatus INT,
--ServiceWorker VARCHAR(MAX)	

TestYear INT
,IsTemp INT

)


If(year(@Stdate)=1900)
BEGIN 
SET @startyear =(select year( MIN(Next)) 
	from LoadTestItem item
	LEFT join LoadTest ttype on ttype.ID=item.ID	
	inner join Loc l on l.Loc=item.Loc
	inner join Elev e on e.ID=Item.Elev	
	where
	l.Status<>1 and e.Status =0 and YEAR(Next)<>1900)

	SET @Endyear =(select  Year(Max(Next) )
	from LoadTestItem item
	LEFT join LoadTest ttype on ttype.ID=item.ID	
	inner join Loc l on l.Loc=item.Loc
	inner join Elev e on e.ID=Item.Elev	
	where
	l.Status<>1 and e.Status =0 )
END
else
begin
	SET @startyear=Year(@StartDate)
SET @Endyear=Year(@EndDate)

end

--select @startyear, @Endyear


WHILE @startyear < = @Endyear
BEGIN
	Insert into #TEMPSAFETYDETAILS

	select item.LID,itemHistory.Last
	
	,CONVERT(VARCHAR(10)
	, ISNULL(itemHistory.Next,
		DATEADD(month,((@startyear-year(item.Next))*12/ (ttype.Frequency))* (ttype.Frequency), item.Next)
	
	),101)


	,0 as hasChild
	--TestType
	,ttype.Name as NTest, ttype.ID as LTID
	,(select count(1) from TestTypeCover where TestTypeID= ttype.ID ) as IsCoverTestType

	--location
	,Rol.Name, l.Tag, l.Loc, l.ID,  l.State AS LocState	 ,isnull(l.City,'') as City
	,Rol.EN,ISNULL(Branch.Name,'')Company,l.Address
	, isnull(rt.Name + (SELECT TOP 1 (CASE  WHEN tblwork.fdesc=rt.Name THEN ''  ELSE'-'+ tblwork.fdesc END) FROM tblwork WHERE tblwork.id=rt.mech ), 'Unassigned') AS RouteName
	,case isnull(eJob.Elev,0) when 0 then 'No' Else 'Yes' End  AS RecurringContract
 
	 --Elev
	,e.ID ,e.Unit, e.ID as NID,e.State,isnull(e.Classification,'')
	 ,isnull((SELECT top 1 eti2.Value FROM   ElevTItem eti2	INNER JOIN ElevT et2 ON eti2.ElevT = et2.ID WHERE  eti2.Elev = e.ID and eti2.fDesc='CAPACITY'),'') as Capacity
	 --Price
	,CASE ISNULL(price.OverrideAmount,0) WHEN 0 THEN isnull(price.DefaultAmount,0) ELSE isnull(price.OverrideAmount,0) END BillingAmount
	,case  When ISNULL(price.OverrideAmount,0) !=0 THEN 'Override Amount' ELSE 'Default Amount' END AS IsDefaultAmount
	,isnull(price.ThirdPartyName,'')
	,isnull(price.Chargeable,ISNULL(item.Chargeable,1))
	,CASE isnull(price.Chargeable,ISNULL(item.Chargeable,1)) WHEN 0 THEN 'No' ELSE 'Yes' END AS ChargeableName


	--Ticket
	,itemHistory.TicketID
	,itemHistory.TestStatus
	, Case isnull(itemHistory.TicketID,0)
		When 0 THEN ''
		ELSE
			CASE ISNULL(itemHistory.TicketStatus,-1)
					   WHEN 0 THEN 'Open'
					   WHEN 1 THEN 'Assigned'
					   WHEN 2 THEN 'En Route'
					   WHEN 3 THEN 'On Site'
					   WHEN 4 THEN 'Completed'
					   WHEN 5 THEN 'On Hold'
					   WHEN -1 THEN ''
					END
		END
	,itemHistory.fWork
	, CONVERT(varchar, itemHistory.Schedule, 101)
	,isnull(itemHistory.Worker,'') CallSign
	,isnull(itemHistory.TestStatus,'0') as TestStatus
	,ISNULL(ListConfig.ItemName,'Open') Status  

	--Proposal
	,pfd.ProposalID
	,isnull(pfd.Status,'')
	--,(select top 1 SendMailStatus from ProposalForm where ProposalForm.ID=isnull(pfd.ProposalID,0)) AS SendEmailStatusID
	--,(select top 1 CASE ISNULL(SendMailStatus,0) WHEN 0 THEN 'No' ELSE 'Yes' END SendEmailStatus from ProposalForm where ID=pfd.ProposalID) AS SendMailStatus 
	--,(select PdfFilePath from ProposalForm where ID=pfd.ProposalID) AS PDFFilePath
	,ISNULL(pf.SendMailStatus,0) SendEmailStatusID
	, CASE ISNULL(pf.SendMailStatus,0) WHEN 0 THEN 'No' ELSE 'Yes' END SendMailStatus
	,pf.PdfFilePath
	,0 as ScheduledStatus
	----Schedule
	--,sc.ScheduledDate, sc.ScheduledStatus,sc.Worker
	----Service
	--,ser.ServiceDate, ser.ServiceStatus,ser.Worker


	,@startyear
	,Case isnull(itemHistory.LID,0) when 0 THEN 0 ELSE 1 END
	
	from LoadTestItem item
	LEFT join LoadTest ttype on ttype.ID=item.ID
	 
	inner join Loc l on l.Loc=item.Loc
	INNER JOIN Owner ON l.Owner = Owner.ID       
	INNER JOIN Rol ON Owner.Rol = Rol.ID  
	LEFT JOIN  Route rt with (nolock) ON l.Route = rt.ID 
	LEFT JOIN Branch on Branch.ID = Rol.EN 
	LEFT JOIN tblUserCo on tblUserCo.CompanyID = Rol.EN


	inner join Elev e on e.ID=Item.Elev
	LEFT JOIN LoadTestItemHistoryPrice price ON price.LID=item.LID AND price.PriceYear=@startyear
	LEFT JOIN tblJoinElevJob eJob ON eJob.Elev=e.ID AND price.PriceYear=@startyear
	--Ticket
	LEFT JOIN LoadTestItemHistory itemHistory ON itemHistory.LID=item.LID AND itemHistory.TestYear=@startyear
		LEFT JOIN ListConfig ON ListConfig.ListName='Test.Status' AND itemHistory.TestStatus = ListConfig.ItemValue    
	LEFT JOIN TicketO  ON TicketO.ID=itemHistory.TicketID 
	LEFT JOIN tblWork ON itemHistory.fWork=tblWork.ID 
	--Proposal
	LEFT JOIN ProposalFormDetail pfd on pfd.TestID=item.LID and pfd.YearProposal=@startyear
	LEFT JOIN ProposalForm pf ON pf.ID=pfd.ProposalID and pf.YearProposal=@startyear
	----schedule
	--LEFT JOIN LoadTestItemSchedule sc ON sc.LID=item.LID AND sc.ScheduledYear=@startyear
	----Service
	--LEFT JOIN LoadTestItemService ser ON ser.LID=item.LID AND ser.ServiceYear=@startyear
	
	where
	l.Status<>1 and e.Status =0 and

	DATEADD(MONTH,((@startyear-year(item.Next))*12/ (ttype.Frequency))* (ttype.Frequency), item.Next)>=@StartDate 
		
	and DATEADD(month,((@startyear-year(item.Next))*12/ (ttype.Frequency))* (ttype.Frequency), item.Next)<=@EndDate

	and year(item.Next)<=@startyear
	--and item.LID=18787
	and (select count (1) from #TEMPSAFETYDETAILS where LID=item.LID and #TEMPSAFETYDETAILS.Next= DATEADD(month,((@startyear-year(item.Next))*12/ (ttype.Frequency))* (ttype.Frequency), item.Next)
	 )=0
	and (Isnull(@TestType,0)=0 or @TestType=ttype.ID)
	AND ISNULL(itemHistory.TestStatus,0)<>3

	 SET @startyear = @startyear + 1 ;
	-- print @startyear

END

Update #TEMPSAFETYDETAILS
set HasChild = (SELECT count(1) FROM #TEMPSAFETYDETAILS tsub 
				WHERE #TEMPSAFETYDETAILS.Elev=tsub.Elev and  tsub.TestYear=#TEMPSAFETYDETAILS.TestYear and tsub.LTID  in(select top 1 TestTypeCoverID from TestTypeCover where TestTypeID=#TEMPSAFETYDETAILS.LTID))

where IsCoverTestType=1

Update #TEMPSAFETYDETAILS
set Chargeable = 0
,BillingAmount=0
,ChargeableName='No'
where IsCoverTestType=0 and (SELECT count(1) FROM #TEMPSAFETYDETAILS tsub 
				WHERE #TEMPSAFETYDETAILS.Elev=tsub.Elev and  tsub.TestYear=#TEMPSAFETYDETAILS.TestYear and tsub.LTID  in(select top 1 TestTypeID from TestTypeCover where TestTypeCoverID=#TEMPSAFETYDETAILS.LTID))=1

Update #TEMPSAFETYDETAILS
SET BillingAmount=ISNULL((SELECT TOP 1 [Amount] FROM EquipmentTestPricing WHERE PriceYear=#TEMPSAFETYDETAILS.TestYear AND TestTypeId=#TEMPSAFETYDETAILS.LTID AND Classification=#TEMPSAFETYDETAILS.Classification),0)
WHERE BillingAmount=0 AND #TEMPSAFETYDETAILS.IsDefaultAmount= 'Default Amount' AND Chargeable=1

Declare  @queryCustom   VARCHAR(MAX)
Declare  @column   VARCHAR(MAX)
Declare  @c_ID   int
Declare  @c_Label   VARCHAR(MAX)
Declare  @c_format   int

set @queryCustom=''
set @column=''
DECLARE cur_Field CURSOR FOR 	
	select ID,Label from tblTestCustomFields			
OPEN cur_Field  
FETCH NEXT FROM cur_Field INTO @c_ID,@c_Label
WHILE @@FETCH_STATUS = 0  
	BEGIN
		set @column=@column +','+ REPLACE(@c_Label,' ','') +'_' + CONVERT(VARCHAR(50),@c_ID) + ' VARCHAR(300) NULL'

		set @queryCustom=+ @queryCustom +   ',isnull((select [Value] FROM [tblTestCustomFieldsValue] Where [TestID] =#TEMPSAFETYDETAILS.LID and [EquipmentID]=#TEMPSAFETYDETAILS.NID and TestYear=#TEMPSAFETYDETAILS.TestYear and tblTestCustomFieldsID='+ CONVERT(VARCHAR(50),@c_ID) + '),'''') as [' + REPLACE(@c_Label,' ','') +'_'+  CONVERT(VARCHAR(50),@c_ID) +']' + CHAR(13)+CHAR(10)
	FETCH NEXT FROM cur_Field INTO @c_ID,@c_Label
	END	
CLOSE cur_Field  
DEALLOCATE cur_Field  




DECLARE @SQLQuery AS VARCHAR(max)

DECLARE @ProposoalCondition varchar(200)

IF (@Proposal='Yes')
	BEGIN
		SET @ProposoalCondition = 'ProposalStatus!=''''' 
    END
ELSE
    BEGIN
		SET @ProposoalCondition = 'ProposalStatus=''''' 
    END

set @SQLQuery=''	
	if(@Cols is not null and @SearchVal is not null)	
		Begin
			print'11'
			set @SQLQuery= @SQLQuery + '  select * from (select * '
			set @SQLQuery= @SQLQuery +  @queryCustom		
			set @SQLQuery= @SQLQuery + ' FROM #TEMPSAFETYDETAILS) t WHERE ' +@Cols+' LIKE (''%'+ @SearchVal+'%'')' + IIF (@Proposal='ALL','',' and ' + @ProposoalCondition ) + ' and ' + @strfilter
		

		END
			

        else if(@Cols is null and @SearchVal is not null)
			begin
					if exists( SELECT TOP 1 1 FROM #TEMPSAFETYDETAILS WHERE #TEMPSAFETYDETAILS.Name LIKE('%'+ @SearchVal+'%'))
					BEGIN
						print'10'
						set @SQLQuery= @SQLQuery + ' select * from (select *  '
						set @SQLQuery= @SQLQuery +  @queryCustom	
						set @SQLQuery= @SQLQuery + ' FROM #TEMPSAFETYDETAILS) t WHERE Name LIKE (''%'+ @SearchVal+'%'')' + IIF (@Proposal='ALL','',' and ' + @ProposoalCondition ) + ' and ' + @strfilter

					END
						
					else if exists(SELECT TOP 1 1 FROM #TEMPSAFETYDETAILS WHERE #TEMPSAFETYDETAILS.ID LIKE('%'+ @SearchVal+'%'))
					BEGIN
						print'111'
						set @SQLQuery= @SQLQuery + ' select * from (select * '
						set @SQLQuery= @SQLQuery +  @queryCustom	
						set @SQLQuery= @SQLQuery + ' FROM #TEMPSAFETYDETAILS) t WHERE  ID LIKE (''%'+ @SearchVal+'%'')' + IIF (@Proposal='ALL','',' and ' + @ProposoalCondition ) + ' and ' + @strfilter

					END

						
					else if exists(SELECT TOP 1 1 FROM #TEMPSAFETYDETAILS WHERE #TEMPSAFETYDETAILS.Tag LIKE('%'+ @SearchVal+'%'))
					BEGIN
					print'21'
						set @SQLQuery= @SQLQuery + ' select * from (select * '
						set @SQLQuery= @SQLQuery +  @queryCustom		
						set @SQLQuery= @SQLQuery + ' FROM #TEMPSAFETYDETAILS) t WHERE  Tag LIKE (''%'+ @SearchVal+'%'')' + IIF (@Proposal='ALL','',' and ' + @ProposoalCondition ) + ' and ' + @strfilter
					
					END						
					ELSE if exists(SELECT TOP 1 1 FROM #TEMPSAFETYDETAILS WHERE #TEMPSAFETYDETAILS.Company LIKE('%'+ @SearchVal+'%'))
					BEGIN
						print'31'
						set @SQLQuery= @SQLQuery + '  select * from (select * '
						set @SQLQuery= @SQLQuery +  @queryCustom	
						set @SQLQuery= @SQLQuery + ' FROM #TEMPSAFETYDETAILS) t WHERE Company LIKE (''%'+ @SearchVal+'%'')' + IIF (@Proposal='ALL','',' and ' + @ProposoalCondition ) + ' and ' + @strfilter
						
					END
						
					ELSE 
					BEGIN
						print'41'
						set @SQLQuery= @SQLQuery + ' select * from (select * '
						set @SQLQuery= @SQLQuery +  @queryCustom	
						set @SQLQuery= @SQLQuery + ' FROM #TEMPSAFETYDETAILS) t WHERE NID LIKE (''%'+ @SearchVal+'%'')' + IIF (@Proposal='ALL','',' and ' + @ProposoalCondition )  + ' and ' + @strfilter
								
					END
						
			END
		ELSE
		BEGIN
			print'51'
			set @SQLQuery= @SQLQuery + ' select * from (select * '
			set @SQLQuery= @SQLQuery +  @queryCustom	
			set @SQLQuery= @SQLQuery + ' FROM #TEMPSAFETYDETAILS ) t where  1=1  '+ IIF (@Proposal='ALL','',' and ' + @ProposoalCondition ) + ' and ' + @strfilter
						
		END			

		--select @SQLQuery

		

	 execute (@SQLQuery + ' order by ' + @SortOrderBy + ' ' + @SortType)	
END


