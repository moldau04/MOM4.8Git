CREATE PROCEDURE [dbo].[spGetProjectListDataMVC]   
@SearchBy    varchar(150),            
@SearchValue varchar(150),            
@StartDate   varchar(30),            
@EndDate     varchar(30),            
@Range       smallint = 1,          
@Type        smallint = -1,              
@IsSalesAsigned int = 0,        
@EN             int = 0,            
@UserID         int = 0,            
@IncludeClose   int = 0,    
@FiltersData as dbo.TblTypeFilters READONLY,
@SearchTeamMemberValue varchar(150)
AS
BEGIN  

SET NOCOUNT ON;            
            
DECLARE @text varchar(max);  
DECLARE @IsAssignedProject INT;
DECLARE @EmpID INT;
SET	@IsAssignedProject =isnull((SELECT TOP 1 IsAssignedProject FROM tblUser WHERE ID=@UserID),0);
SET @EmpID = ISNULL((SELECT e.ID FROM tblUser INNER JOIN Emp e ON e.CallSign= tblUser.fUser WHERE tblUser.ID=@UserID),0)

  
DECLARE @Filters_Customer varchar(500), @FiltersQuery VARCHAR(5000)  ,@Filters_Tag varchar(500)  ,@Filters_ID varchar(500) ,          
@Filters_fdesc varchar(500)  ,@Filters_Status varchar(500) ,@Filters_Company varchar(500) ,          
@Filters_CType varchar(500)  ,@Filters_TemplateDesc varchar(500) ,@Filters_Type varchar(500) ,          
@Filters_SalesPerson varchar(500) ,@Filters_Route varchar(500)  ,@Filters_NHour decimal(18, 3) ,          
@Filters_ContractPrice decimal(18, 3) ,@Filters_NotBilledYet decimal(18, 3) ,          
@Filters_NComm decimal(18, 3)  ,@Filters_NRev decimal(18, 3) ,          
@Filters_NLabor decimal(18, 3)  ,@Filters_NMat decimal(18, 3) ,          
@Filters_NOMat decimal(18, 3)  ,@Filters_NCost decimal(18, 3) ,          
@Filters_NProfit decimal(18, 3)  ,@Filters_NRatio decimal(18, 3)   ,        
@Filters_RouteFilters varchar(5000) ,  @Filters_DepartmentFilters varchar(5000), 
@Filters_ProjectManager varchar(500)
          
          
SELECT @Filters_Customer = Customer  ,@Filters_Tag = Tag    ,@Filters_ID = ID,          
@Filters_fdesc = fdesc    ,@Filters_Status = Status  ,@Filters_Company = Company ,          
@Filters_SalesPerson = SalesPerson  ,@Filters_Route = Route   ,@Filters_NHour = NHour ,          
@Filters_NComm    = NComm   ,@Filters_NRev   = NRev   ,@Filters_NLabor   = NLabor,              
@Filters_NMat   =  NMat   ,@Filters_NOMat    =  NOMat  ,@Filters_NCost  = NCost,          
@Filters_NProfit  = NProfit   ,@Filters_NRatio = NRatio  ,@Filters_ContractPrice = isnull(ContractPrice,0) ,          
@Filters_NotBilledYet= NotBilledYet,          
@Filters_CType= CType    ,@Filters_TemplateDesc = TemplateDesc ,@Filters_Type = Type   ,        
@Filters_RouteFilters = RouteFilters  ,  @Filters_DepartmentFilters = DepartmentFilters 
,@Filters_ProjectManager=ProjectManagerUserName
FROM  @FiltersData   
 
       
SET @text = '

 DECLARE @jobcostLabor TABLE   ( amt numeric(30,2)  , job int  not null  PRIMARY KEY (job));
 DECLARE @jobcostMaterial TABLE   ( amt numeric(30,2)  , job int not null  PRIMARY KEY (job) );
 DECLARE @jobcostOthers TABLE   ( amt numeric(30,2)  , job int  not null  PRIMARY KEY (job));
 DECLARE @jobcosttOthers TABLE   ( amt numeric(30,2)  , job int not null  PRIMARY KEY (job));
 DECLARE @jobcostNCost TABLE   ( amt numeric(30,2)  , job int not null  PRIMARY KEY (job));
 DECLARE @jobcostNRev TABLE   ( amt numeric(30,2)  , job int not null  PRIMARY KEY (job));
 DECLARE @jobcostNHour TABLE   ( NHour numeric(30,2)  , job int not null  PRIMARY KEY (job));
 DECLARE @jobcostBudget TABLE   ( Budget numeric(30,2)  , job int not null  PRIMARY KEY (job));
 DECLARE @jobcostcomm TABLE  ( amt numeric(30,2)  , job int not null  PRIMARY KEY (job));
 DECLARE @jobcostrevPO TABLE ( amt numeric(30,2)  , job int not null  PRIMARY KEY (job));
 DECLARE @jobcostStartDate TABLE ( StartDate date  , job int not null  PRIMARY KEY (job));
 DECLARE @jobNK TABLE   ( job int  not null  PRIMARY KEY (job))  ;



insert into @jobNK 
SELECT   distinct  j.Id as job  
FROM Job j WITH(NOLOCK)            
inner join loc l WITH(NOLOCK)   on l.loc=j.loc  
inner join Owner o WITH(NOLOCK)   on o.ID=l.owner            
inner join rol r WITH(NOLOCK)   on o.Rol=r.ID
left join team t WITH(NOLOCK) on t.jobid = j.id
where 1=1 
'       
 
 if (@IsAssignedProject=1)
 BEGIN
	SET @text += ' and j.ProjectManagerUserID=' + CONVERT(nvarchar(50), @EmpID)   
 END
------------If Company feature is Active            
IF (@EN = 1)            
BEGIN            
SET @text += ' and  isnull(r.EN,0)  in ( select UC.CompanyID  from tblUserCo UC where UC.IsSel = 1 and UC.UserID =' + CONVERT(nvarchar(50), @UserID)  +')'           
END  



------------If sales person    
										 
IF (@IsSalesAsigned > 0)            
BEGIN            
SET @text +=   '  and l.Terr in (	SELECT  ISNULL(id, 0)   FROM Terr 	WHERE NAME = (SELECT    fUser  FROM tblUser WHERE id = '''   + CAST(@UserID AS nvarchar(10)) + '''   ) ) '           
END 

------------If @IncludeClose unChecked 

if( @IncludeClose=0 and @Range <> 3)  
													 
SET @text +=   '  and j.status <> 1' 

------------If @IncludeClose unChecked 

IF( isnull(@Type,-1) > =0)    SET @text += ' AND j.Type='''   + CAST(@Type AS nvarchar(10)) + ''''    


------------  If @Range = 1 Cumulative  

------------  If @Range = 2 Date Range - Activity 
													
------------  If @Range = 3 Date Range - Closed

IF (@Range = 3)   
												 

SET @text += '  and j.status = 1  and cast( j.CloseDate as date)  > = '''    + CONVERT(varchar(50), @StartDate)   + ''' AND cast ( j.CloseDate as date) <= '''  + CONVERT(varchar(50), @EndDate) + '''  '   
													 
------------If @Range = 4 Date Range - Created 

IF (@Range = 4) 

SET @text += ' and cast( j.fDate as date)  > = '''    + CONVERT(varchar(50), @StartDate)   + ''' AND cast ( j.fDate as date) <= '''  + CONVERT(varchar(50), @EndDate) + '''  '   

--- Search DLL Filter
IF (isnull(@SearchBy,'') <> ''   AND isnull(@SearchValue,'') <> '')
													  
BEGIN 
													     
IF (@SearchBy = 'l.tag') SET @text += ' and ' + @SearchBy + ' like ''%'   + @SearchValue + '%'''
													 
ELSE IF (@SearchBy = 'l.City') SET @text += ' and ' + @SearchBy + ' like ''%'    + @SearchValue + '%'''
														 
ELSE IF (@SearchBy = 'l.State') SET @text += ' and ' + @SearchBy + ' like ''%'   + @SearchValue + '%'''
														 
ELSE IF (@SearchBy = 'r.Name') SET @text += ' and ' + @SearchBy + ' like ''%'  + @SearchValue + '%'''													

ELSE IF (@SearchBy = 'j.id') SET @text += ' and cast ( j.id as varchar(50))  = '  +  @SearchValue 

ELSE IF (@SearchBy = 'j.PWIP') SET @text += ' and cast ( isnull(j.PWIP,0) as varchar(50))  = '  +  @SearchValue 

ELSE IF (@SearchBy = 'j.Certified') SET @text += ' and cast ( isnull(j.Certified,0) as varchar(50))  = '  +  @SearchValue 

ELSE IF (@SearchBy = 'j.loc') SET @text += ' and cast ( j.loc as varchar(50))  = '  +  @SearchValue   

ELSE IF (@SearchBy = 'j.fdate')    SET @text += ' and cast( j.fDate as date)  > = '''    + CONVERT(varchar(50), @SearchValue)   + ''' AND cast ( j.fDate as date) <= '''  + CONVERT(varchar(50), @SearchValue) + '''  '     

ELSE IF (@SearchBy = 'j.Status') SET @text += ' and cast ( j.Status as varchar(50)) = '''  + @SearchValue + ''''

ELSE IF (@SearchBy = 'j.fdesc') SET @text += ' and ' + @SearchBy + ' like ''%'  + @SearchValue + '%'''

ELSE IF (@SearchBy = 't.title')
BEGIN
	SET @text += ' and ' + @SearchBy + ' = '''  + @SearchValue + ''''	
	IF (@SearchTeamMemberValue != '') SET @text += ' and t.MomUserID like ''%'  + @SearchTeamMemberValue + '%'''	
END

ELSE IF (@SearchBy = 'j.Custom1') SET @text += ' and j.Custom1 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom2') SET @text += ' and j.Custom2 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom3') SET @text += ' and j.Custom3 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom4') SET @text += ' and j.Custom4 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom5') SET @text += ' and j.Custom5 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom6') SET @text += ' and j.Custom6 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom7') SET @text += ' and j.Custom7 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom8') SET @text += ' and j.Custom8 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom9') SET @text += ' and j.Custom9 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom10') SET @text += ' and j.Custom10 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom11') SET @text += ' and j.Custom11 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom12') SET @text += ' and j.Custom12 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom13') SET @text += ' and j.Custom13 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom14') SET @text += ' and j.Custom14 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom15') SET @text += ' and j.Custom15 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom16') SET @text += ' and j.Custom16 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom17') SET @text += ' and j.Custom17 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom18') SET @text += ' and j.Custom18 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom19') SET @text += ' and j.Custom19 like ''%'  + @SearchValue + '%'' '		
ELSE IF (@SearchBy = 'j.Custom20') SET @text += ' and j.Custom20 like ''%'  + @SearchValue + '%'' '		
END
	 

----------**************************************************************************-----      
--- telerik filter

IF(ISNULL(@Filters_ID,'') <> '')   

SET @text += ' and cast (  j.id as varchar(50))  in  ('  +  @Filters_ID +')'  --- Multiple job ID filter 

IF(ISNULL(@Filters_Customer,'') <> '')   

SET @text += ' and r.Name like ''%'  + @Filters_Customer + '%'''	

IF(ISNULL(@Filters_Tag,'') <> '')   

SET @text += ' and l.tag like ''%'  + @Filters_Tag + '%'''
	  
IF(ISNULL(@Filters_ID,'') <> '')   

SET @text += ' and cast ( j.id as varchar(50))  in  ('  +  @Filters_ID +')'  --- Multiple job ID filter  

IF(ISNULL(@Filters_fdesc,'') <> '')   

SET @text += ' and j.fdesc like ''%'  + @Filters_fdesc + '%'''


	   
IF(ISNULL(@Filters_Status,'') <> '')          
BEGIN  
SET @Filters_Status = 0
if exists ( select 1 from @FiltersData where Status like  '%open%'  )  SET @Filters_Status = 0; 
if exists ( select 1 from @FiltersData where Status like  '%closed%'  )  SET @Filters_Status = 1;
if exists ( select 1 from @FiltersData where Status like  '%hold%'  )  SET @Filters_Status = 2; 
if exists ( select 1 from @FiltersData where Status like  '%completed%'  )  SET @Filters_Status = 3;        
SET @text += ' AND  j.Status = '+ @Filters_Status + ''   
END  

IF(ISNULL(@Filters_Ctype,'') <> '')   SET @text += ' and  j.[CType]    LIKE ''%'+ @Filters_Ctype + '%''' 
  
----------************************************JOC CAST CALCULATION**************************************-----  

IF (@Range = 2) ---########

BEGIN

SET @text += ' 

   DECLARE @jobItemp TABLE   (  	
    [Job] [int]  ,	[Phase] [smallint] NULL,
	[fDate] [datetime]  ,	[Ref] [varchar](50) NULL,
	[fDesc] [varchar](max) NULL,	[Amount] [numeric](30, 2) NULL,
	[TransID] [int] NULL,	[Type] [smallint] NULL,
	[Labor] [smallint] NULL,	[Billed] [int] NULL,
	[Invoice] [int] NULL,	[UseTax] [bit] NULL,
	[APTicket] [int] NULL );

	insert into @jobItemp
	SELECT  [Job]      ,[Phase]
      ,[fDate]      ,[Ref]
      ,[fDesc]      ,[Amount]
      ,[TransID]      ,[Type]
      ,[Labor]      ,[Billed]
      ,[Invoice]      ,[UseTax]
      ,[APTicket]
  FROM  [JobI]
  where job is not null and fDate is not null  and isnull(amount,0) <> 0
   AND cast( JobI.fDate as date) >= '''  + CONVERT(varchar(50), cast( @StartDate as date ) )+ ''' 
   AND cast( JobI.fDate as date) <='''   + CONVERT(varchar(50), cast( @EndDate   as date )) + ''' 
  
insert into @jobcostLabor 

SELECT  SUM(ISNULL(JobI.Amount,0) ) amt  ,   jobi.job   
FROM @jobItemp JobI    
WHERE  isnull(JobI.Labor,0)= 1  and jobi.fDesc not in (''Mileage on Ticket'',''Expenses on Ticket'')
AND isnull(jobi.Type,0) <> 0   and jobi.job in (select job  from  @jobNK)
'

IF (@Range = 2)
SET @text += 	' AND cast( JobI.fDate as date) >= '''  + CONVERT(varchar(50), @StartDate)+ ''' AND    cast( JobI.fDate as date) <='''   + CONVERT(varchar(50), @EndDate) + '''  ' 
SET @text +=  ' group by jobi.job  

insert into @jobcostLabor SELECT  0 amt  ,     job  FROM @jobNK where job not in (select job from @jobcostLabor)

'
 
SET @text +=  '  
  
insert into @jobcostMaterial 

SELECT  SUM(ISNULL(JobI.Amount,0) ) amt  ,   jobi.job   
FROM @jobItemp JobI   
inner join  JobTItem on JobTItem.Line=jobi.Phase   and JobTItem.Job=jobi.job                                                
inner join bom on bom.JobTItemID=JobTItem.ID                                                
inner join BOMT on bomt.ID =bom.Type   AND (  bomt.Type=''Materials''    or bomt.Type=''Inventory''        )       
WHERE  isnull(JobI.Labor,0) <> 1  and jobi.fDesc not in (''Mileage on Ticket'',''Expenses on Ticket'')
AND isnull(jobi.Type,0) <> 0   and jobi.job in (select job  from  @jobNK)
'

IF (@Range = 2)
SET @text += 	' AND cast( JobI.fDate as date) >= '''  + CONVERT(varchar(50), @StartDate)+ ''' AND    cast( JobI.fDate as date) <='''   + CONVERT(varchar(50), @EndDate) + '''  ' 
SET @text +=  ' group by jobi.job  

insert into @jobcostMaterial SELECT  0 amt  ,      job  FROM @jobNK where job  not in (select job from @jobcostMaterial)
 
'

SET @text +=  '  
  
insert into @jobcostOthers 

SELECT  SUM(ISNULL(JobI.Amount,0) ) amt  ,   jobi.job   
FROM @jobItemp JobI   
inner join  JobTItem on JobTItem.Line=jobi.Phase   and JobTItem.Job=jobi.job                                                
inner join bom on bom.JobTItemID=JobTItem.ID                                                
inner join BOMT on bomt.ID =bom.Type   AND   bomt.Type<>''Materials''  and bomt.Type <> ''Labor''  and bomt.Type<>''Inventory''        
WHERE  isnull(JobI.Labor,0) <> 1  and jobi.fDesc not in (''Mileage on Ticket'',''Expenses on Ticket'')
AND isnull(jobi.Type,0) <> 0   and jobi.job in (select job  from  @jobNK) 
'

IF (@Range = 2)
SET @text += 	' AND cast( JobI.fDate as date) >= '''  + CONVERT(varchar(50), @StartDate)+ ''' AND    cast( JobI.fDate as date) <='''   + CONVERT(varchar(50), @EndDate) + '''  ' 
SET @text +=  ' group by jobi.job 

insert into @jobcostOthers SELECT  0 amt  ,       job  FROM @jobNK where job  not in (select job from @jobcostOthers)


'

SET @text +=  ' 
  
insert into @jobcosttOthers 
SELECT  SUM(ISNULL(JobI.Amount,0) ) amt  ,   jobi.job   
FROM @jobItemp JobI  WHERE    jobi.fDesc   in (''Mileage on Ticket'',''Expenses on Ticket'') AND isnull(jobi.Type,0) <> 0   and jobi.job in (select job  from  @jobNK) 
'

IF (@Range = 2)
SET @text += 	' AND cast( JobI.fDate as date) >= '''  + CONVERT(varchar(50), @StartDate)+ ''' AND    cast( JobI.fDate as date) <='''   + CONVERT(varchar(50), @EndDate) + '''  ' 
SET @text +=  ' group by jobi.job   


insert into @jobcosttOthers SELECT  0 amt  ,      job  FROM @jobNK where job  not in (select job from @jobcosttOthers)
 
'
 
SET @text +=  '  
  
insert into @jobcostNCost 
SELECT  SUM(ISNULL(JobI.Amount,0) ) amt  ,   jobi.job  FROM @jobItemp JobI WHERE  isnull(jobi.Type,0) = 1    and jobi.job in (select job  from  @jobNK)
'

IF (@Range = 2)
SET @text += 	' AND cast( JobI.fDate as date) >= '''  + CONVERT(varchar(50), @StartDate)+ ''' AND    cast( JobI.fDate as date) <='''   + CONVERT(varchar(50), @EndDate) + '''  ' 
SET @text +=  ' group by jobi.job 


insert into @jobcostNCost SELECT  0 amt  ,      job  FROM @jobNK where job  not in (select job from @jobcostNCost)

'
 

SET @text +=  '  
  
insert into @jobcostNRev 
SELECT  SUM(ISNULL(JobI.Amount,0) ) amt  ,   jobi.job  FROM @jobItemp JobI  WHERE  isnull(jobi.Type,0) = 0   and jobi.job in (select job  from  @jobNK)
'

IF (@Range = 2)
SET @text += 	' AND cast( JobI.fDate as date) >= '''  + CONVERT(varchar(50), @StartDate)+ ''' AND    cast( JobI.fDate as date) <='''   + CONVERT(varchar(50), @EndDate) + '''  ' 
SET @text +=  ' group by jobi.job 

insert into @jobcostNRev SELECT  0 amt  ,      job  FROM @jobNK where job  not in (select job from @jobcostNRev)

'
 

SET @text +=  ' 
insert into @jobcostNHour select  SUM(ISNULL(Total,0) ) , job   FROM TicketD  WHERE  isnull(job,0) <> 0    and  job in (select job  from  @jobNK)
'

IF (@Range = 2)
SET @text += 	' AND cast( EDate as date) >= '''  + CONVERT(varchar(50), @StartDate)+ ''' AND    cast( EDate as date) <='''   + CONVERT(varchar(50), @EndDate) + '''  ' 
SET @text +=  ' group by  job 

insert into @jobcostNHour SELECT  0 NHour  ,      job  FROM @jobNK where job  not in (select job from @jobcostNHour)

'
 
SET @text +=  '    
insert into @jobcostBudget 
select  SUM(ISNULL(Budget,0) ) , ji.job   FROM JobTItem JI WHERE JI.Type = 0   and ji.job in (select job  from  @jobNK)  group by  ji.job   
insert into @jobcostBudget SELECT  0 Budget  ,      job  FROM @jobNK where job  not in (select job from @jobcostBudget)'
 
SET @text +=  '  
     
insert into @jobcostcomm
SELECT  sum(isnull(p.Balance,0)),p.job   from POItem p 		INNER JOIN PO on p.po = po.po		WHERE  po.status in (0,3,4) 	and isnull(p.job,0) <> 0'

IF (@Range = 2)
SET @text += 	' AND cast( po.fdate as date) >= '''  + CONVERT(varchar(50), @StartDate)+ ''' 
AND    cast( po.fdate as date) <='''   + CONVERT(varchar(50), @EndDate) + '''  ' 
	 
											
SET @text +=  '  group by p.job '

SET @text +=  '  insert into @jobcostcomm SELECT  0 amt  ,      job  FROM @jobNK where job  not in (select job from @jobcostcomm) '
 
SET @text +=  ' 
  
INSERT INTO @jobcostrevPO 
SELECT Sum(ISNULL(rp.Amount,0)) AS ReceivePO , p.Job FROM RPOItem rp  INNER JOIN ReceivePO r on r.ID = rp.ReceivePO  LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line WHERE ISNULL(r.Status,0) = 0  and isnull(p.Job,0)  <>  0 and  p.job in (select job  from  @jobNK)'		
	
IF (@Range = 2)
SET @text += 	' AND cast( r.fDate as date) >= '''  + CONVERT(varchar(50), @StartDate)+ ''' AND    cast( r.fDate as date) <='''   + CONVERT(varchar(50), @EndDate) + '''  ' 
	

SET @text +=  '  group by p.job '


SET @text +=  ' insert into @jobcostrevPO SELECT  0 amt  ,      job  FROM @jobNK where job  not in (select job from @jobcostrevPO)'

 

 
 SET @text +=  '  
  
insert into @jobcostStartDate 
SELECT   MIN(fDate) StartDate , jobi.job
						FROM  @jobItemp JobI 
 where  jobi.job in (select job  from  @jobNK)
'

IF (@Range = 2)
SET @text += 	' 
AND cast( JobI.fDate as date) >= '''  + CONVERT(varchar(50), @StartDate)+ ''' 
AND cast( JobI.fDate as date) <='''   + CONVERT(varchar(50), @EndDate) + '''  ' 
SET @text +=  ' group by jobi.job 

insert into @jobcostStartDate  SELECT  null StartDate  ,   job  FROM @jobNK where job  not in (select job from @jobcostStartDate)

'


 END ----############

 else 

 BEGIN

 SET @text +=   '

  INSERT INTO  @jobcostLabor   
  select j.Labor , j.id from @jobNK nk inner join job j on j.id=nk.job
  INSERT INTO  @jobcostMaterial     
   select j.Mat , j.id from @jobNK nk inner join job j on j.id=nk.job
  INSERT INTO  @jobcostOthers    
   select j.OtherExp , j.id from @jobNK nk inner join job j on j.id=nk.job
  INSERT INTO  @jobcosttOthers    
   select 0 , j.id from @jobNK nk inner join job j on j.id=nk.job
  INSERT INTO  @jobcostNCost   
   select j.Cost , j.id from @jobNK nk inner join job j on j.id=nk.job
  INSERT INTO  @jobcostNRev   
   select j.Rev , j.id from @jobNK nk inner join job j on j.id=nk.job
  INSERT INTO  @jobcostNHour       
   select j.Hour , j.id from @jobNK nk inner join job j on j.id=nk.job
  INSERT INTO  @jobcostBudget      
   select j.BRev , j.id from @jobNK nk inner join job j on j.id=nk.job
  INSERT INTO  @jobcostcomm    
   select j.Comm , j.id from @jobNK nk inner join job j on j.id=nk.job
  INSERT INTO  @jobcostrevPO    
   select 0 , j.id from @jobNK nk inner join job j on j.id=nk.job 

   ' 
 

  SET @text +=  '  
  
insert into @jobcostStartDate 
SELECT   MIN(fDate) StartDate , jobi.job
						FROM    JobI 
 where  jobi.job in (select job  from  @jobNK)
'
SET @text +=  ' group by jobi.job 

insert into @jobcostStartDate  SELECT  null StartDate  ,   job  FROM @jobNK where job  not in (select job from @jobcostStartDate)

'

 END

--------------------------------------------------------------------------------------------------------------------------------------------

 
         
            --Select all Projects/Job            
        SET @text +=  '  select  *,( NK.NOtherExp + NK.NTicketOtherExp) as Nomat from    (  SELECT  j.[ID]            
                ,l.ID AS LID
				,j.[fDesc]            
                ,  (SELECT type from JobType WITH(NOLOCK)   WHERE JobType.Id = j.type)      Type            
                ,j.[Loc]  
				, l.type LocationType
				, isnull((select top 1 bt.Description from BusinessType bt where bt.ID =  l.BusinessType),'''') as BuildingType  
				, l.tag
				,l.Address
                ,j.[Owner]            
                ,j.[Elev]                                    
                , CASE j.status WHEN 0 THEN ''Open'' WHEN 1 THEN ''Closed'' WHEN 2 THEN ''Hold'' WHEN 3 THEN ''Completed'' END as Status          
                ,j.[PO]            
                ,j.[Rev]            
                ,j.[Mat]            
                ,j.[OtherExp]            
                ,j.[Labor]            
                ,j.[Cost]            
                ,j.[Profit]            
                ,j.[Ratio]            
                ,j.[Reg]            
                ,j.[OT]            
                ,j.[DT]            
                ,j.[TT]            
                ,j.[Hour]            
                ,j.[BRev]            
                ,j.[BMat]            
                ,j.[BLabor]            
                ,j.[BCost]            
                ,j.[BProfit]            
                ,j.[BRatio]            
                ,j.[BHour]            
                ,j.[Template]            
                ,j.[fDate]            
                ,j.[CloseDate]            
                ,j.[Comm]            
                ,j.[WageC]            
                ,j.[NT]            
                ,j.[Post]            
                ,j.[EN]            
                ,(Select B.Name From Branch B WITH(NOLOCK)   inner Join Rol r WITH(NOLOCK)   on B.ID = r.EN where r.id =(o.rol)) as Company            
                ,j.[Certified]            
                ,j.[Apprentice]            
                ,j.[UseCat]            
                ,j.[UseDed]            
                ,j.[BillRate]            
                ,j.[Markup]            
                ,j.[PType]            
                ,j.[Charge]            
                , 0 Amount            
                ,j.[GL]            
                ,j.[GLRev]            
                ,j.[GandA]            
                ,j.[OHLabor]            
                ,j.[LastOH]            
                ,j.[etc]            
                ,j.[ETCModifier]            
                ,j.[FP]            
                ,j.[fGroup]            
                ,j.[CType]            
                ,j.[Elevs]            
                ,j.[RateTravel]            
                ,j.[RateOT]            
                ,j.[RateNT]            
                ,j.[RateDT]            
                ,j.[RateMileage]            
                ,j.Type AS [NType]
				,j.Custom1
				,j.Custom2
				,j.Custom3
				,j.Custom4
				,j.Custom5
				,j.Custom6  
				,ji.StartDate          
                ,r.name as Customer            
                ,CONVERT(VARCHAR(MAX), j.Remarks) AS Remarks   

				,  (SELECT fDesc from JobT WITH(NOLOCK)   WHERE JobT.Id = j.Template)  AS TemplateDesc 													      
														
				,  (SELECT Name from Terr WITH(NOLOCK)   WHERE Terr.Id = l.Terr)  AS Salesperson 

				,  (SELECT Name from Route WITH(NOLOCK)   WHERE Route.Id = l.Route)  AS Route , 
										                       
															
						t1NHour.NHour  AS NHour, 
						t1lexp.amt  AS NLabor, 
						t1mat.amt  AS NMat,  
						t1oth.amt  AS NOtherExp, 
						t1toth.amt   AS NTicketOtherExp, 
						t1NCost.amt  AS NCost,
						t1NRev.amt   AS NRev, 
					(isnull(t1Budget.Budget,0)	-   isnull(t1NRev.amt,0) )AS NotBilledYet, 
						t1comm.amt as NComm,
					t1rpo.amt AS ReceivePO  ,  
					(t1NRev.amt-t1NCost.amt)  as NProfit,  
					cast ( ( (t1NRev.amt-t1NCost.amt)/ (case t1NRev.amt when 0 then 1 else t1NRev.amt end ) * 100 ) as numeric(30,2))as NRatio,  
					0 AS Bill,            
					0 AS BillPercent          
					,'''' AS Url    
					,(isnull(t1Budget.Budget,0))  AS ContractPrice
					,j.ProjectManagerUserID 
					,j.AssignedProjectUserID 
				   ,(select top 1 Emp.CallSign  from  Emp where Emp.ID=j.ProjectManagerUserID ) as ProjectManagerUserName
					FROM Job j WITH(NOLOCK)  
                    inner join loc l WITH(NOLOCK)   on l.loc=j.loc  
                    inner join Owner o WITH(NOLOCK)   on o.ID=l.owner            
                    inner join rol r WITH(NOLOCK)   on o.Rol=r.ID 
					inner join @jobcostLabor t1LExp   on t1LExp.job=j.id
					inner join @jobcostMaterial t1mat   on t1mat.job=j.id
					inner join @jobcostothers t1oth   on t1oth.job=j.id
					inner join @jobcosttothers t1toth   on t1toth.job=j.id
					inner join @jobcostNCost t1NCost   on t1NCost.job=j.id 
					inner join @jobcostNRev t1NRev   on t1NRev.job=j.id 
					inner join @jobcostNHour t1NHour   on t1NHour.job=j.id
					inner join @jobcostBudget t1Budget   on t1Budget.job=j.id
					inner join @jobcostcomm t1comm   on t1comm.job=j.id
					inner join @jobcostrevPO t1rpo   on t1rpo.job=j.id 
					inner join @jobcostStartDate ji on j.ID = ji.Job 				 		 							  
                    ' 

			-- NK  ########################
			 								  
													   
													  
SET @text +=   '  where 1=1'  

IF(ISNULL(@Filters_ProjectManager,'') <> '')   

SET @text += ' and isnull(Emp.CallSign,'''') like ''%'  + @Filters_ProjectManager + '%'''

											 

SET @text += '  ) NK where 1=1 '    
         
          
----------**************************************************************************-----      
--- telerik filter 	
IF(ISNULL(@Filters_TemplateDesc,'') <> '')          
BEGIN          
SET @Text += ' AND  NK.TemplateDesc  LIKE ''%'+ @Filters_TemplateDesc + '%'''           
END 
      
IF(ISNULL(@Filters_SalesPerson,'') <> '')          
BEGIN          
SET @Text += ' AND  NK.SalesPerson  LIKE ''%'+ @Filters_SalesPerson + '%'''           
END 

IF(ISNULL(@Filters_Type,'') <> '')          
BEGIN          
SET @Text += ' AND  NK.Type   LIKE ''%'+ @Filters_Type + '%'''         
END 
	
IF(ISNULL(@Filters_DepartmentFilters,'') <> '')          
BEGIN                
SET @Text += '   AND  NK.Type  IN    ('+ @Filters_DepartmentFilters + ')'             
END 
         
IF(ISNULL(@Filters_Route,'') <> '')          
BEGIN          
SET @Text += ' AND  NK.Route   LIKE ''%'+ @Filters_Route + '%'''           
END          
        
IF(ISNULL(@Filters_RouteFilters,'') <> '')          
BEGIN          
SET @Text += '   AND  NK.Route  IN    ('+ @Filters_RouteFilters + ')'             
END          
        
IF(ISNULL(@Filters_NHour,0)>0)          
BEGIN      
SET @Text += ' AND  ISNULL(NK.NHour,0)   = '+ CAST( @Filters_NHour as nvarchar(10)) + ''          
END          
    
IF(ISNULL(@Filters_ContractPrice,0)>0)          
BEGIN          
SET @Text += ' AND  ISNULL(NK.ContractPrice,0)  =   '+ CAST(@Filters_ContractPrice  as varchar) + ''           
END          
          
IF(ISNULL(@Filters_NotBilledYet,0)>0)          
BEGIN          
SET @Text += ' AND   ISNULL(NK.NotBilledYet,0)  =  '+ CAST(@Filters_NotBilledYet  AS varchar)+ ''           
END          
      
IF(ISNULL(@Filters_NRev,0)>0 )          
BEGIN        
SET @Text += ' AND  ISNULL(NK.NRev,0)   = '+ CAST(@Filters_NRev as varchar) + ''          
END    
  
IF(ISNULL(@Filters_NComm,0)>0 )          
BEGIN        
SET @Text += ' AND  ISNULL(NK.Ncomm,0)   = '+ CAST(@Filters_NComm as varchar) + ''          
END    
    
IF(ISNULL(@Filters_NLabor,0)>0 )          
BEGIN          
SET @Text += ' AND ISNULL(NK.NLabor,0)  = '+ CAST(@Filters_NLabor as varchar) + ''          
END   
  
IF(ISNULL(@Filters_NCost,0)>0 )          
BEGIN     
SET @Text += ' AND ISNULL(NK.NCost,0) = '+ CAST(@Filters_NCost as varchar) + ''          
END   
  
IF(ISNULL(@Filters_NProfit ,0)<> 0 )          
BEGIN          
SET @Text += '  AND ISNULL(NK.NProfit,0) = '+ CAST(@Filters_NProfit as varchar) + ''          
END   
  
IF(ISNULL(@Filters_NRatio ,0)>0 )          
BEGIN       
SET @Text += ' AND ISNULL(NK.NRatio,0)  = '+ CAST(@Filters_NRatio as varchar) + ''          
END   
  
IF(ISNULL(@Filters_NMat,0)>0)          
BEGIN          
SET @Text += ' AND  ISNULL(NK.NMat,0)  = '+ CAST(@Filters_NMat  AS varchar)+ ''           
END          
          
IF(ISNULL(@Filters_NOMat,0)>0)          
BEGIN          
SET @Text += ' AND  ISNULL(( NK.NOtherExp + NK.NTicketOtherExp),0)  =   '+ CAST( @Filters_NOMat AS varchar) + ''           
END  
   
----------**************************************************************************-----          

EXEC (@text)  

END