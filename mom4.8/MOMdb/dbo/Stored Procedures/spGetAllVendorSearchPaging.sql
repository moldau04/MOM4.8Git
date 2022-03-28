CREATE  PROCEDURE [dbo].[spGetAllVendorSearchPaging] 
@Cols      NVARCHAR(100)=NULL,
@SearchVal NVARCHAR(250)=NULL,
@EN        INT,
@UserID    INT = 0,
@PageNumber Int = 1,
@PageSize Int = 0,
@Status NVARCHAR(50)=NULL,
@SortBy NVARCHAR(50),
@SortType NVARCHAR(50)
AS
  BEGIN

  if @SortBy = ''
  BEGIN
	SET @SortBy = 'Name '
  END
 

  if @PageSize = 0
  BEGIN
  SELECT @PageSize = Count(1) FROM Vendor WHERE STATUS IN (0,case when @Status='InActive' then 1 when @Status='Active' then 0 else 0 end)   
  END

      DECLARE @SQLQuery VARCHAR(max)
	  DECLARE @SQL VARCHAR(max)
	  DECLARE @Status1 NVARCHAR(50) ='Active'
	  DECLARE @Sttus1 NVARCHAR = '0'
	  DECLARE @Sttus NVARCHAR = '0'
	  SET @Sttus = CASE WHEN @Status  = 'Active' THEN '0' WHEN  @Status = 'InActive' THEN '1' ELSE '2' END
	  --Validate pagination parameters
	IF(@PageNumber IS NULL Or @PageNumber <= 0) SET @PageNumber = 1
	IF(@PageSize IS NULL Or @PageSize <= 0) SET @PageSize = 50
	
	--Calculate start and end row to return
	Declare @StartRow Int = ((@PageNumber - 1) * @PageSize) + 1      
	Declare @EndRow Int = @PageNumber * @PageSize

	--DECLARE @Vendor TABLE (
	CREATE TABLE #Vendor (
 State VARCHAR(500) NULL,
 City VARCHAR(500) NULL,
 Zip VARCHAR(500) NULL,
 Address VARCHAR(MAX) NULL,
 Contact VARCHAR(500) NULL,
 Phone VARCHAR(500) NULL,
 EMail VARCHAR(500) NULL,
 ID int NULL,
 Rol int NULL,
 Name VARCHAR(MAX) NULL,
 EN INT NULL,
 Company VARCHAR(MAX) NULL,
 Acct VARCHAR(MAX) NULL,
 Type VARCHAR(MAX) NULL,
 Status VARCHAR(MAX) NULL,
 Balance decimal(17,2) NULL   
);

      IF( @Cols IS NOT NULL
          AND @SearchVal IS NOT NULL )
        BEGIN
            IF( @Cols NOT IN( 'Vendor.Status', 'Rol.Type' ) )
			BEGIN
			print '1'
              SET @SQLQuery = ' select * from (SELECT (s.fDesc) as State,Rol.City,Rol.Zip, Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,case when Rol.Type=1 then ''Cost Of Sales'' else ''Overhead'' end as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 LEFT join State s on Rol.State = s.Name'
			    IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +=' left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
				  END
			   SET @SQLQuery += ' left outer join Branch B on B.ID = Rol.EN  WHERE '
                              + @Cols + ' LIKE (''%' + @SearchVal + '%'') AND Vendor.Status IN (''' + @Sttus +''',''' + @Sttus1 +''') '
			IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and UC.IsSel = 1 and UC.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  s.fDesc,Rol.City,Rol.Zip,Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN,B.Name, Vendor.Acct, Rol.Type, Vendor.Status,Vendor.Balance ) as t WHERE 1= 1 '
							  END
            ELSE IF ( @Cols = 'Vendor.Status' )
			BEGIN
              SET @SQLQuery = '
select * from (SELECT   (s.fDesc) as State,Rol.City,Rol.Zip, Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,case when Rol.Type=1 then ''Cost Of Sales'' else ''Overhead'' end as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance '
			   IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +=' , UC.IsSel, UC.UserID '
				  END 
			  
			 SET @SQLQuery +=' FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 left join State s on Rol.State = s.Name '
			 IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +='  left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
				  END 
			  SET @SQLQuery +=' left outer join Branch B on B.ID = Rol.EN)AS X WHERE '
                              + Replace(@Cols, 'Vendor', 'X') + ' LIKE (''' + @SearchVal + ''') AND X.Status IN (''' + @Status +''',''' + @Status1 +''')  '
		IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and X.IsSel = 1 and X.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  X.State,X.City,X.Zip,X.Address,X.Contact,X.Phone,X.EMail,X.ID,X.Rol,X.Name,X.EN,X.Name, X.Acct, X.Type, X.Status,X.Balance,X.Company '
							  END



							   ELSE IF ( @Cols = 'Rol.Type' )
			BEGIN
              SET @SQLQuery = '
select * from (SELECT   (s.fDesc) as State,Rol.City,Rol.Zip, Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,Vendor.Type as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance '
			   IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +=' , UC.IsSel, UC.UserID '
				  END 
			  
			 SET @SQLQuery +=' FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 left join State s on Rol.State = s.Name '
			 IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +='  left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
				  END 
			  SET @SQLQuery +=' left outer join Branch B on B.ID = Rol.EN)AS X WHERE '
                              + Replace(@Cols, 'Rol', 'X') + ' LIKE (''' + @SearchVal + ''') AND X.Status IN (''' + @Status +''',''' + @Status1 +''')  '
		IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and X.IsSel = 1 and X.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  X.State,X.City,X.Zip,X.Address,X.Contact,X.Phone,X.EMail,X.ID,X.Rol,X.Name,X.EN,X.Name, X.Acct, X.Type, X.Status,X.Balance,X.Company '
							  END




            ELSE
			BEGIN
              SET @SQLQuery = '
select * from (SELECT   (s.fDesc) as State,Rol.City,Rol.Zip, Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Vendor.Acct,case when Rol.Type=1 then ''Cost Of Sales'' else ''Overhead'' end as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance '
			   IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +=' , UC.IsSel, UC.UserID '
				  END 
			SET @SQLQuery +='   FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 left join State s on Rol.State = s.Name '
			IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +='  left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
				  END 
			  SET @SQLQuery +=' left outer join Branch B on B.ID = Rol.EN)AS X WHERE '
                              + Replace(@Cols, 'Rol', 'X') + ' LIKE (''' + @SearchVal + ''') AND X.Status IN (''' + @Status +''',''' + @Status1 +''')  '
					IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and X.IsSel = 1 and X.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END		
		SET @SQLQuery +=' GROUP BY  X.State,X.City,X.Zip,X.Address,X.Contact,X.Phone,X.EMail,X.ID,X.Rol,X.Name,X.EN,X.Name, X.Acct, X.Type, X.Status,X.Balance '
							  END
        END
      ELSE IF( @Cols IS NULL
          AND @SearchVal IS NOT NULL )
        BEGIN
            SET @SQLQuery ='
select * from (SELECT   (s.fDesc) as State,Rol.City,Rol.Zip, Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,case when Rol.Type=1 then ''Cost Of Sales'' else ''Overhead'' end as Type,
					case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance
					FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 LEFT join State s on Rol.State = s.Name ' 
					  IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +=' left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
				  END
			 SET @SQLQuery +=' left outer join Branch B on B.ID = Rol.EN 
					where  Vendor.ID LIKE ''%'
                           + @SearchVal + '%'' OR Vendor.Acct LIKE ''%'
                           + @SearchVal + '%'' or Rol.Name LIKE ''%'
                           + @SearchVal + '%'' or Rol.Type LIKE ''%'
                           + @SearchVal
                           + '%'' or Vendor.Status LIKE ''%'
                           --+ @SearchVal + '%'' AND Vendor.Status = CASE WHEN ''' + @Status +''' = ''Active'' THEN 0 WHEN ''' + @Status + '''=''InActive'' THEN 1 ELSE 2 END;'
						   + @SearchVal + '%'' AND Vendor.Status IN (''' + @Sttus +''',''' + @Sttus1 +''') ;'
						   IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and UC.IsSel = 1 and UC.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  s.fDesc,Rol.City,Rol.Zip,Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN,B.Name, Vendor.Acct, Rol.Type, Vendor.Status,Vendor.Balance '
          END
		  ELSE IF( @Cols IS not  NULL
          AND @SearchVal IS NULL )
        BEGIN
		print @Cols
            SET @SQLQuery = '
select * from ( SELECT   (s.fDesc) as State,Rol.City,Rol.Zip,Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,case when Rol.Type=1 then ''Cost Of Sales'' else ''Overhead'' end as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 LEFT join State s on Rol.State = s.Name '
       IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +='  left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
        END
		--SET @SQLQuery +='  left outer join Branch B on B.ID = Rol.EN WHERE 1=1 AND Vendor.Status = CASE WHEN ''' + @Status +''' = ''Active'' THEN 0 WHEN ''' + @Status + '''=''InActive'' THEN 1 ELSE 2 END '
		SET @SQLQuery +='  left outer join Branch B on B.ID = Rol.EN WHERE 1=1 AND Vendor.Status IN ('''+ @Sttus +''',''' + @Sttus1 +''') '
		IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and UC.IsSel = 1 and UC.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  s.fDesc,Rol.City,Rol.Zip,Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN,B.Name, Vendor.Acct, Rol.Type, Vendor.Status,Vendor.Balance ) as p WHERE 1= 1 '
	    END
      ELSE IF( @Cols IS NULL
          AND @SearchVal IS NULL )
        BEGIN
            SET @SQLQuery = '
select * from (SELECT  (s.fDesc) as State,Rol.City,Rol.Zip,Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,Vendor.Type as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 left join State s on Rol.State = s.Name '
        IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
        END
		SET @SQLQuery +=' left outer join Branch B on B.ID = Rol.EN)AS X WHERE 1=1 AND X.Status IN (''' + @Status +''',''' + @Status1 +''') '
	     
		
		IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and UC.IsSel = 1 and UC.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  X.State,X.City,X.Zip,X.Address,X.Contact,X.Phone,X.EMail,X.ID,X.Rol,X.Name,X.EN,X.Name, X.Acct, X.Type, X.Status,X.Balance,X.Company '

	    END
      --SET @SQLQuery +=' Order by Name'
	  Print @SQLQuery;
      INSERT INTO #Vendor EXECUTE( @SQLQuery)

	  
	  SET @SQL =' SELECT * FROM ( select ROW_NUMBER() OVER(Order By '+@SortBy+'  '+@SortType+') RowNumber, COUNT(1) OVER() TotalRow,* from  (SELECT * FROM #Vendor  ) as T )  as p 
	  WHERE p.RowNumber BETWEEN '+CONVERT(NVARCHAR(50), @StartRow)+' And '+CONVERT(NVARCHAR(50), @EndRow)+''
	  print @SQL
	  EXECUTE(@SQL)
	  

  END 
