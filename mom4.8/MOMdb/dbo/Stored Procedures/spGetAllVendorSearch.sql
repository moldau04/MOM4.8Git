CREATE  PROCEDURE [dbo].[spGetAllVendorSearch] 
@Cols      NVARCHAR(100)=NULL,
@SearchVal NVARCHAR(250)=NULL,
@EN        INT,
@UserID    INT = 0
AS
  BEGIN
      DECLARE @SQLQuery VARCHAR(max)

      IF( @Cols IS NOT NULL
          AND @SearchVal IS NOT NULL )
        BEGIN
            IF( @Cols NOT IN( 'Vendor.Status', 'Rol.Type' ) )
			BEGIN
              SET @SQLQuery = 'SELECT Rol.State,Rol.City,Rol.Zip, Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,case when Rol.Type=1 then ''Cost Of Sales'' else ''Overhead'' end as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 '
			    IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +=' left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
				  END
			   SET @SQLQuery += ' left outer join Branch B on B.ID = Rol.EN  WHERE '
                              + @Cols + ' LIKE (''%' + @SearchVal + '%'') '
			IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and UC.IsSel = 1 and UC.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  Rol.State,Rol.City,Rol.Zip,Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN,B.Name, Vendor.Acct, Rol.Type, Vendor.Status,Vendor.Balance'
							  END
            ELSE IF ( @Cols = 'Vendor.Status' )
			BEGIN
              SET @SQLQuery = 'select * from(SELECT   Rol.State,Rol.City,Rol.Zip, Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,case when Rol.Type=1 then ''Cost Of Sales'' else ''Overhead'' end as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance '
			   IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +=' , UC.IsSel, UC.UserID '
				  END 
			  
			 SET @SQLQuery +=' FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 '
			 IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +='  left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
				  END 
			  SET @SQLQuery +=' left outer join Branch B on B.ID = Rol.EN)AS X WHERE '
                              + Replace(@Cols, 'Vendor', 'X') + ' LIKE (''' + @SearchVal + ''') '
		IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and X.IsSel = 1 and X.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  X.State,X.City,X.Zip,X.Address,X.Contact,X.Phone,X.EMail,X.ID,X.Rol,X.Name,X.EN,X.Name, X.Acct, X.Type, X.Status,X.Balance,X.Company'
							  END



							   ELSE IF ( @Cols = 'Rol.Type' )
			BEGIN
              SET @SQLQuery = 'select * from(SELECT   Rol.State,Rol.City,Rol.Zip, Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,Vendor.Type as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance '
			   IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +=' , UC.IsSel, UC.UserID '
				  END 
			  
			 SET @SQLQuery +=' FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 '
			 IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +='  left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
				  END 
			  SET @SQLQuery +=' left outer join Branch B on B.ID = Rol.EN)AS X WHERE '
                              + Replace(@Cols, 'Rol', 'X') + ' LIKE (''' + @SearchVal + ''') '
		IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and X.IsSel = 1 and X.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  X.State,X.City,X.Zip,X.Address,X.Contact,X.Phone,X.EMail,X.ID,X.Rol,X.Name,X.EN,X.Name, X.Acct, X.Type, X.Status,X.Balance,X.Company'
							  END




            ELSE
			BEGIN
              SET @SQLQuery = 'select * from(SELECT   Rol.State,Rol.City,Rol.Zip, Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Vendor.Acct,case when Rol.Type=1 then ''Cost Of Sales'' else ''Overhead'' end as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance '
			   IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +=' , UC.IsSel, UC.UserID '
				  END 
			SET @SQLQuery +='   FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 '
			IF( @EN = 1 )
				 BEGIN
					 SET @SQLQuery +='  left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
				  END 
			  SET @SQLQuery +=' left outer join Branch B on B.ID = Rol.EN)AS X WHERE '
                              + Replace(@Cols, 'Rol', 'X') + ' LIKE (''' + @SearchVal + ''') '
					IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and X.IsSel = 1 and X.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END		
		SET @SQLQuery +=' GROUP BY  X.State,X.City,X.Zip,X.Address,X.Contact,X.Phone,X.EMail,X.ID,X.Rol,X.Name,X.EN,X.Name, X.Acct, X.Type, X.Status,X.Balance'
							  END
        END
      ELSE IF( @Cols IS NULL
          AND @SearchVal IS NOT NULL )
        BEGIN
            SET @SQLQuery ='SELECT   Rol.State,Rol.City,Rol.Zip, Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,case when Rol.Type=1 then ''Cost Of Sales'' else ''Overhead'' end as Type,
					case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance
					FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 ' 
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
                           + @SearchVal + '%'';'
						   IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and UC.IsSel = 1 and UC.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  Rol.State,Rol.City,Rol.Zip,Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN,B.Name, Vendor.Acct, Rol.Type, Vendor.Status,Vendor.Balance'
          END
		  ELSE IF( @Cols IS not  NULL
          AND @SearchVal IS NULL )
        BEGIN
            SET @SQLQuery = 'SELECT   Rol.State,Rol.City,Rol.Zip,Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,case when Rol.Type=1 then ''Cost Of Sales'' else ''Overhead'' end as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 '
       IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +='  left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
        END
		SET @SQLQuery +='  left outer join Branch B on B.ID = Rol.EN WHERE 1=1  '
		IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and UC.IsSel = 1 and UC.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  Rol.State,Rol.City,Rol.Zip,Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN,B.Name, Vendor.Acct, Rol.Type, Vendor.Status,Vendor.Balance'
	    END
      ELSE IF( @Cols IS NULL
          AND @SearchVal IS NULL )
        BEGIN
            SET @SQLQuery = 'select * from(SELECT  Rol.State,Rol.City,Rol.Zip,Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN, LTRIM(RTRIM(B.Name)) As Company,Vendor.Acct,Vendor.Type as Type,case when Vendor.Status=1 then ''InActive'' when Vendor.Status=0 then ''Active'' else ''Hold'' end as Status,isnull(Vendor.Balance,0)*-1 as Balance FROM Vendor Join Rol on Vendor.Rol=Rol.ID left join PJ ON PJ.Vendor = Vendor.ID and PJ.Status = 0 '
        IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
        END
		SET @SQLQuery +=' left outer join Branch B on B.ID = Rol.EN)AS X WHERE 1=1 '
	     
		
		IF( @EN = 1 )
        BEGIN
            SET @SQLQuery +=' and UC.IsSel = 1 and UC.UserID =' + CONVERT(NVARCHAR(50), @UserID)
        END
		SET @SQLQuery +=' GROUP BY  X.State,X.City,X.Zip,X.Address,X.Contact,X.Phone,X.EMail,X.ID,X.Rol,X.Name,X.EN,X.Name, X.Acct, X.Type, X.Status,X.Balance,X.Company'

	    END
	--SET @SQLQuery +=' GROUP BY  Rol.State,Rol.City,Rol.Zip,Rol.Address,Rol.Contact,Rol.Phone,Rol.EMail,Vendor.ID,Vendor.Rol,Rol.Name,Rol.EN,B.Name, Vendor.Acct, Rol.Type, Vendor.Status,Vendor.Balance'
      SET @SQLQuery +=' Order by Name'
	Print @SQLQuery;
      EXECUTE(@SQLQuery)
  END 
