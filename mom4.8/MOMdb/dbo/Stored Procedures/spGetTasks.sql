/*--------------------------------------------------------------------
Modified By: Thurstan
Modified On: 13 dec 2018	
Description: Add Contact column into select statement
@Screen = '': Don't care screen
@Screen = 'Sales': Get all task create in Sales module
@Screen = 'Opportunity'
@Screen = 'Lead'
@Screen = 'Project'
@Screen = 'Equipment'
--------------------------------------------------------------------*/

CREATE PROC [dbo].[spGetTasks]
@SearchBy VARCHAR(50),
@SearchValue VARCHAR(250),
@startdate VARCHAR(50),
@enddate VARCHAR(50),
@Open SMALLINT,
@IsSalesAsigned INT =0,
@EN INT				=0,
@UserID int			=0,
@Screen varchar(100),
@Ref int
AS
DECLARE @SalesAsignedTerrID INT = 0
	 
IF( @IsSalesAsigned > 0 )
BEGIN
    SELECT @SalesAsignedTerrID = Isnull(id, 0)
    FROM   Terr
    WHERE  NAME = (SELECT fUser
                    FROM   tblUser
                    WHERE  id = @IsSalesAsigned)
END
SET @SearchValue=Replace(@SearchValue,'''','')

DECLARE @Query VARCHAR(max)
SET @Query = '
SELECT T.ID,
       T.Type,
       T.DateDue,
       T.TimeDue,
       T.Subject,
       T.Remarks AS Remarks,
       T.Keyword,       
       T.fUser,
	   T.Contact,
       R.Name,
	   r.EN, 
	   CASE r.Type 
			WHEN 4 THEN (
				SELECT TOP 1 ro.NAME AS CustomerName
				FROM Loc l
					LEFT OUTER JOIN Rol rl ON l.Rol = rl.ID
					INNER JOIN Owner o ON o.ID = l.Owner
					LEFT OUTER JOIN Rol ro ON o.Rol = ro.ID
				WHERE  rl.ID = T.Rol)
			WHEN 3 THEN (SELECT TOP 1 CustomerName FROM Prospect WHERE  Rol = r.ID)
		END AS CustomerName,
		(CASE r.Type
               WHEN 3 THEN ''Lead''
               WHEN 4 THEN ''Existing''
			   ELSE ''''
       END) TypeName,
	   Ltrim(Rtrim(B.Name))   As Company,
       CAST(CAST(DateDue AS DATE) AS DATETIME) + CAST(CAST(TimeDue AS TIME) AS DATETIME) as duedate,
      ( Datediff(day, DateDue, Getdate()) ) AS days,
      ''Open'' as status ,
      '''' as result   ,
      0 as statusid  ,
	  isnull(screen,'''') screen,
	  isnull(ref,0) ref,
	  T.fBy as CreatedBy,
	  CAST(CAST(fDate AS DATE) AS DATETIME) + CAST(CAST(fTime AS TIME) AS DATETIME) as CreatedDate
FROM   ToDo T
		INNER JOIN Rol r ON T.Rol = R.ID '

IF(@UserID <> 0)
BEGIN
	--SET @Query +=
	--	' INNER JOIN (SELECT e.Last+'', ''+e.fFirst as fuser,u.fuser as username FROM tbluser u 
	--					INNER JOIN emp e ON u.fuser = e.callsign 
	--						WHERE 
	--							--e.sales = 1 AND
	--							e.Status=0 
	--							AND u.ID = '''+Convert(varchar(50),@userID)+''') as a
	--	ON a.fuser = T.fUser '
	SET @Query +=
		' INNER JOIN (SELECT u.fuser FROM tbluser u 
						INNER JOIN emp e ON u.fuser = e.callsign 
							WHERE 
								e.Status=0 
								AND u.ID = '''+Convert(varchar(50),@userID)+''') as a
		ON a.fuser = T.fUser '
END
SET @Query +=' LEFT  JOIN Branch B on B.ID = r.EN'
IF(@EN = 1)  
BEGIN  
	SET @Query+=' LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN '
END

SET @Query+= ' where r.type in (3,4)' 

IF(@Screen = 'Sales')
BEGIN
	SET @Query +=' and  (T.Screen <> ''Project'' OR T.Screen <> ''Equipment'') '
END
ELSE IF(ISNULL(@Screen,'') != '')
BEGIN
	SET @Query +=' and  T.Screen = '''+ISNULL(@Screen,'')+''' '
	IF (ISNULL(@Ref, 0) != 0)
	BEGIN
		SET @Query +=' and T.Ref = '''+CONVERT(VARCHAR(50),@Ref)+''' '
	END
END
				    
IF(@startdate <> '')
BEGIN
	SET @Query +=' and  T.DateDue >= '''+@startdate+''''
END
       
IF(@enddate <> '')
BEGIN
	SET @Query +=' and  T.DateDue <= '''+@enddate+''''
END

IF(@EN = 1)
BEGIN
    SET @Query +=' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)                     
END 
IF( @IsSalesAsigned > 0  AND @SalesAsignedTerrID > 0) ----If user is sales person and IsSalesAsigned is true 
BEGIN
	SET @Query +=' AND r.ID = CASE '++CONVERT(NVARCHAR(10), (@IsSalesAsigned))+'
                        WHEN 0 THEN r.ID
                        ELSE ( CASE r.type
                                 WHEN 3 THEN (SELECT TOP 1 Rol
                                              FROM   Prospect
                                              WHERE  Prospect.Terr = '+CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))
                                                    +' AND Prospect.Rol = r.ID)
                                 WHEN 4 THEN (SELECT TOP 1 Rol
                                              FROM   loc
                                              WHERE loc.Rol = r.ID
											  
											  and (  isnull(loc.Terr,0) = '+ CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))+'    or   isnull(loc.Terr2,0) = '+ CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))+' )
											  
											  )
                                 ELSE r.ID
                               END )
                      END '
END
          
IF(@SearchBy<>'')
BEGIN
	IF(@SearchBy = 'r.Name' OR @SearchBy = 't.subject')
	BEGIN
		SET @Query +=' and REPLACE( ' +@SearchBy +','''''''','''') like '''+@SearchValue+'%'''
	END
	ELSE IF(@SearchBy = 't.fuser')
	BEGIN
		SET @Query +=' and ' +@SearchBy +' = '''+@SearchValue+''''
	END
	ELSE IF(@SearchBy = 't.remarks' OR @SearchBy = 't.result')
	BEGIN
		SET @Query +=' and t.remarks like ''%'+@SearchValue+'%'''
	END
	ELSE IF(@SearchBy = 't.rol')
	BEGIN
		SET @Query +=' and ' +@SearchBy +' = '+@SearchValue
	END
	ELSE IF(@SearchBy = 'days')
	BEGIN
		SET @Query +=' and ( Datediff(day,DateDue, Getdate()) ) '+@SearchValue+''''
	END
	ELSE IF(@SearchBy = 'status')
	BEGIN
		IF(@SearchValue=1)
		BEGIN
		SET @Query +=' and t.id = 0'
		END
	END
END

IF (@Open=0)
BEGIN
	SET @Query +=' and t.id = 0'
END

SET @Query+=' union all'

SET @Query+='   
SELECT T.ID,
       T.Type,
       T.Datedone as DateDue,
       T.Timedone as timeDue,
       T.Subject,
       T.Remarks AS Remarks,
       T.Keyword,      
       T.fUser,
	   T.Contact,
       R.Name,
	   r.EN, 
	    CASE r.Type 
			WHEN 4 THEN (
				SELECT TOP 1 ro.NAME AS CustomerName
				FROM Loc l
					LEFT OUTER JOIN Rol rl ON l.Rol = rl.ID
					INNER JOIN Owner o ON o.ID = l.Owner
					LEFT OUTER JOIN Rol ro ON o.Rol = ro.ID
				WHERE  rl.ID = T.Rol)
			WHEN 3 THEN (SELECT TOP 1 CustomerName FROM Prospect WHERE  Rol = r.ID)
		END AS CustomerName,
		(CASE r.Type
               WHEN 3 THEN ''Lead''
               WHEN 4 THEN ''Existing''
			   ELSE ''''
       END) TypeName,
	   Ltrim(Rtrim(B.Name))   As Company,
       CAST(CAST(Datedone AS DATE) AS DATETIME) + CAST(CAST(Timedone AS TIME) AS DATETIME) as duedate,
      ( Datediff(day, Datedone, Getdate()) ) AS days,
      ''Completed'' as status    ,
      result  ,
      1 as statusid,
	  isnull(screen,'''') screen,
	  isnull(ref,0) ref,
	  T.fBy as CreatedBy,
	  CAST(CAST(fDate AS DATE) AS DATETIME) + CAST(CAST(fTime AS TIME) AS DATETIME) as CreatedDate
FROM   done T
		INNER JOIN Rol r ON T.Rol = R.ID '
IF(@UserID <> 0)
BEGIN
	--SET @Query +=
	--	' INNER JOIN (SELECT e.Last+'', ''+e.fFirst as fuser,u.fuser as username FROM tbluser u 
	--					INNER JOIN emp e ON u.fuser = e.callsign 
	--						WHERE 
	--							e.Status=0 
	--							AND u.ID = '''+Convert(varchar(50),@userID)+''') as a
	--	ON a.fuser = T.fUser '
	SET @Query +=
		' INNER JOIN (SELECT u.fuser FROM tbluser u 
						INNER JOIN emp e ON u.fuser = e.callsign 
							WHERE 
								e.Status=0 
								AND u.ID = '''+Convert(varchar(50),@userID)+''') as a
		ON a.fuser = T.fUser '
END
SET @Query += ' LEFT  JOIN Branch B on B.ID = r.EN '
IF(@EN = 1)  
BEGIN  
    SET @Query+=' LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN '
END
SET @Query+= ' where r.type in (3,4)' 

IF(@startdate <> '')
BEGIN
	SET @Query +=' and  T.Datedone >= '''+@startdate+''''
END
       
IF(@enddate <> '')
BEGIN
	SET @Query +=' and  T.Datedone <= '''+@enddate+''''
END
    
IF(@EN = 1)
BEGIN
    SET @Query +=' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)                     
END  

IF(@SearchBy<>'')
BEGIN
	IF(@SearchBy = 'r.Name' OR @SearchBy = 't.subject')
	BEGIN
		SET @Query +=' and REPLACE( ' +@SearchBy +','''''''','''') like '''+@SearchValue+'%'''
	END
	ELSE IF(@SearchBy = 't.fuser')
	BEGIN
		SET @Query +=' and ' +@SearchBy +' = '''+@SearchValue+''''
	END
	ELSE IF(@SearchBy = 't.remarks' OR @SearchBy = 't.result')
	BEGIN
		SET @Query +=' and ' +@SearchBy +' like ''%'+@SearchValue+'%'''
	END
	ELSE IF(@SearchBy = 't.rol')
	BEGIN
		SET @Query +=' and ' +@SearchBy +' = '+@SearchValue
	END
	ELSE IF(@SearchBy = 'days')
	BEGIN
		SET @Query +=' and ( Datediff(day,Datedone, Getdate()) ) '+@SearchValue+''''
	END
	ELSE IF(@SearchBy = 'status')
	BEGIN
		IF(@SearchValue=0)
		BEGIN
		SET @Query +=' and t.id = 0'
		END
	END
END

IF (@Open=1)
BEGIN
	SET @Query +=' and t.id = 0'
END

IF( @IsSalesAsigned > 0  AND @SalesAsignedTerrID > 0) ----If user is sales person and IsSalesAsigned is true 
BEGIN
	SET @Query +=' AND r.ID = CASE '++CONVERT(NVARCHAR(10), (@IsSalesAsigned))+'
                        WHEN 0 THEN r.ID
                        ELSE ( CASE r.type
                                 WHEN 3 THEN (SELECT TOP 1 Rol
                                              FROM   Prospect
                                              WHERE  Prospect.Terr = '+CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))
                                                    +' AND Prospect.Rol = r.ID)
                                 WHEN 4 THEN (SELECT TOP 1 Rol
                                              FROM   loc
                                              WHERE loc.Rol = r.ID											  
											  and   (    isnull(loc.Terr,0) = '+ CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))+' or   isnull(loc.Terr2,0) = '+ CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))+' ) 
											  
											  )
                                 ELSE r.ID
                               END )
                      END '
END
	           
SET @Query +=' ORDER  BY DateDue desc, TimeDue desc '

EXEC( @Query)

