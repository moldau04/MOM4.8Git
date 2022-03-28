/*
Modified by: Thomas
Modified on: 30 Oct 2019
Description: Including product field on the returned data

Created by: Thomas
Created on: 04 Mar 2019
Description: An update for spGetOpportunity: I removed some unsuitable fields after changing the flow: 1 opportunity --> 1 estimate to 1 oppotunity --> n estimate.
			Also for fixing bug on ES-1182: Opportunity error duplicate.  The error duplicate come from the way we calculate SixtyDay field.
			I checked on source and saw that we were nolonger use that field anymore, so that I removed it for bug fixing.
			Please refferrence to store procedure spGetOpportunity if any problems
*/

CREATE proc [dbo].[spGetOpportunityNew]
	@SearchBy varchar(50),
	@SearchValue varchar(250),
	@startdate varchar(50),
	@enddate varchar(50),
	@rol int,
	@IsSalesAsigned INT =0,
	@EN INT				=0,
	@UserID int			=0
AS
 
DECLARE @Query varchar(max)

SET @Query = '
	SELECT DISTINCT l.ID,
		r.Name,
		l.fDesc,
		l.RolType,
		r.EN, 
		B.Name As Company,
		s.Name as Status,      
		CASE l.Probability
			WHEN 0 THEN ''Excellent''
			WHEN 1 THEN ''Very Good''
			WHEN 2 THEN ''Good''
			WHEN 3 THEN ''Average''
			WHEN 4 THEN ''Poor''
		END as Probability,
		sv.Description as Product,
		l.Profit,
		l.CreateDate,
		l.Rol,
		--l.closedate,
		CASE WHEN l.closedate is not null THEN l.closedate
		ELSE (SELECT TOP 1 Estimate.BDate FROM Estimate WHERE Estimate.Opportunity = l.ID ORDER BY Estimate.BDate asc)
		END closedate,
		l.Remarks,
		CASE isnull(l.closed,0) WHEN 1 THEN ''Yes'' ELSE ''No'' END as closed, 
		l.revenue, 
		l.fuser, 
		l.CompanyName,
		(select top 1 (select top 1 name from terr where ID = lo.terr) from loc lo where lo.rol = r.ID) as defsales,
		(select count(d.Filename) from documents d  where screenid = (isnull(l.TicketID,0)) and screen = ''Ticket'') as DocumentCount,
		--l.Estimate,
		(SELECT STUFF((SELECT '', '' + Convert(varchar,ID) from Estimate where Opportunity = l.ID FOR XML PATH('''')),1,1,'''')) as [Estimate],
		p.Referral,
		st.Description AS Stage,
		(SELECT STUFF((SELECT '', '' + Convert(varchar,job) from Estimate where Opportunity = l.ID FOR XML PATH('''')),1,1,'''')) as job,
		--e.fFor,
		(CASE r.Type
               --WHEN 0 THEN ''Customer''
               --WHEN 1 THEN ''Vendor''
               --WHEN 2 THEN ''Bank''
               WHEN 3 THEN ''Lead''
               WHEN 4 THEN ''ACCOUNT''
               --WHEN 5 THEN ''Employee''
               --ELSE ''Misc''
			   ELSE ''''
             END) fFor,
		CASE ISNULL((SELECT TOP 1 Discounted from Estimate where Opportunity = l.ID AND Discounted = 1), 0) WHEN 0 THEN ''No''
			WHEN 1 THEN ''Yes''
			ELSE '''' 
		END AS EstimateDiscounted,
		ISNULL((Select SUM(Price) FROM Estimate WHERE Opportunity = l.ID), 0) as BidPrice,
		ISNULL((Select SUM(Quoted) FROM Estimate WHERE Opportunity = l.ID), 0) as FinalBid,
		CASE WHEN l.Department is null THEN ( SELECT TOP 1 jt1.Type FROM Estimate e1 
											 LEFT JOIN JobT j1 ON j1.ID = e1.Template 
											 LEFT JOIN JobType jt1 ON jt1.ID = j1.Type
											 WHERE e1.Opportunity = l.ID)
											 --jt.Type
			ELSE (SELECT Type FROM JobType WHERE ID = l.Department) END as Dept
	FROM   Lead l
		INNER JOIN Rol r
               ON l.Rol = r.ID 
		LEFT OUTER JOIN Prospect p on p.Rol = l.Rol
		LEFT OUTER JOIN OEStatus s ON l.Status= s.ID
		LEFT OUTER JOIN Stage st ON l.OpportunityStageID = st.ID
		LEFT JOIN Branch B on B.ID = r.EN   
		LEFT JOIN Loc lc on lc.Rol = l.Rol
		LEFT JOIN Estimate e on e.Opportunity = l.id
		LEFT JOIN Service sv on sv.ID = l.Product
		LEFT JOIN JobT j on j.id = e.Template
		LEFT JOIN JobType jt on j.Type=jt.ID
	'                          
IF(@EN = 1)  
BEGIN  
    SET @Query +=' LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN '
END
SET @Query += ' where r.type in (3,4)'         
           
	
IF(@SearchBy ='l.CloseDate')
BEGIN
	IF(@startdate <> '')
	BEGIN		
		SET @Query +=' and  CONVERT(date,l.CloseDate,101) >= CONVERT(date,'''+@startdate+''',101)'
	END
       
	IF(@enddate <> '')
	BEGIN		
		SET @Query +=' and  CONVERT(date,l.CloseDate,101) <= CONVERT(date,'''+@enddate+''',101)'
	END     
END
ELSE 
BEGIN
	IF(@startdate <> '')
	BEGIN		
		SET @Query +=' and  CONVERT(date,l.CreateDate,101) >= CONVERT(date,'''+@startdate+''',101)'
	END
       
	IF(@enddate <> '')
	BEGIN		
		SET @Query +=' and  CONVERT(date,l.CreateDate,101) <= CONVERT(date,'''+@enddate+''',101)'
	END 
END
	 
IF(@EN = 1)
BEGIN
    SET @Query+=' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)                    
END  	
	
IF(@rol <> 0)
BEGIN		
	SET @Query +=' and  l.rol = '+convert(varchar(25),@rol)    
END     
               
IF(@SearchBy<>'')
BEGIN
	IF(@SearchBy = 'r.Name' or @SearchBy = 'l.fdesc' or  @SearchBy= 'l.CompanyName' or @SearchBy = 'r.Phone' or @SearchBy = 'r.Cellular' or @SearchBy = 'r.City' )
	BEGIN
		SET @Query +=' and ' +@SearchBy +' like '''+@SearchValue+'%'''
	END
	ELSE IF(@SearchBy = 'r.State' or @SearchBy = 'l.type' or @SearchBy = 'l.Probability')
	BEGIN
		SET @Query +=' and ' +@SearchBy +' = '''+@SearchValue+''''
	END
	ELSE IF(@SearchBy = 'l.status')
	BEGIN
		SET @Query +=' and ' +@SearchBy +' = '+@SearchValue
	END
	ELSE IF(@SearchBy = 'r.Address' or @SearchBy = 'r.Email')
	BEGIN
		SET @Query +=' and ' +@SearchBy +' like ''%'+@SearchValue+'%'''
	END	
	ELSE IF(@SearchBy = 'l.rol')
	BEGIN
		SET @Query +=' and ' +@SearchBy +' = '+@SearchValue
	END
	ELSE IF(@SearchBy = 'l.fuser')
	BEGIN
		SET @Query +=' and ' +@SearchBy +' = '''+@SearchValue+''''
	END
	ELSE IF(@SearchBy = 'l.ID')
	BEGIN
		SET @Query +=' and ' +@SearchBy +' = '''+@SearchValue+''''
	END
END

IF( @IsSalesAsigned > 0  )
BEGIN
	SET @Query+=' and  l.AssignedToID = '+ CONVERT(NVARCHAR(10), (@IsSalesAsigned)) 
END 
              
--SET @Query +=' ORDER  BY l.CreateDate, Ltrim(Rtrim(r.Name)) '

EXEC( @Query)