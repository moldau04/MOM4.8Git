/*--------------------------------------------------------------------
Modified By: Thomas
Modified On: 5 Feb 2020	
Description: Fixed issue for PCS 

Modified By: Thurstan
Modified On: 22 Dec Nov 2018	
Description: Re-Create sp SpGetlocationProspectSearch
--------------------------------------------------------------------*/

CREATE PROCEDURE  [dbo].[SpGetlocationProspectSearch] @SearchText     VARCHAR(50),
                                               @CustomerID     INT,
                                               @EN             INT =0,
                                               @UserID         INT = 0,
                                               @IsSalesAsigned INT =0,
                                               @Prospects      INT=0
AS
    DECLARE @WOspacialchars VARCHAR(50)
    DECLARE @SalesAsignedTerrID INT = 0
    -- GST CHECK AS PER COUNTRY
    DECLARE @Country AS VARCHAR(10)
    DECLARE @GST_RATE AS DECIMAL

    SET @Country=(SELECT Label
                  FROM   Custom
                  WHERE  NAME = 'Country')

    IF @Country = '1'
      BEGIN
          SET @GST_RATE=(SELECT Cast(Label AS DECIMAL)
                         FROM   Custom
                         WHERE  NAME = 'GSTRate')
      END
    ELSE
      BEGIN
          SET @GST_RATE=0
      END

    -- GST CHECK AS PER COUNTRY
    --- Company Check
    DECLARE @CompText AS VARCHAR(50)

    IF( @EN = 1 )
      BEGIN
          SET @CompText =' and UC.IsSel = 1 and UC.UserID ='
                         + CONVERT(NVARCHAR(50), @UserID)
      END
    ELSE
      BEGIN
          SET @CompText =' '
      END

    IF( @IsSalesAsigned > 0 )----If USER IS Salesperson
      BEGIN
          SELECT  @SalesAsignedTerrID=isnull(Terr.ID,0)  FROM  tblUser  inner join EMP on EMP.CallSign=tblUser.fUser  inner join Terr on EMP.ID=Terr.SMan   WHERE tblUser.id=@IsSalesAsigned
      END

    SET @WOspacialchars = dbo.Removespecialchars(@SearchText)

    IF( @CustomerID = 0 )
      BEGIN
          SELECT *
          FROM  (SELECT DISTINCT TOP 100 l.loc                       AS value,
                                       l.tag                         AS label,
                                       ( 'ID: ' + Isnull(l.ID ,'') + ', ' + 'Customer: ' + Isnull(ro.NAME COLLATE DATABASE_DEFAULT,'') + ', '
                                         + Isnull(r.Contact COLLATE DATABASE_DEFAULT,'') + ', ' + Isnull(l.Address ,'') + ', ' + Isnull(l.City ,'')+ ', '
                                         + Isnull(l.[State] ,'') + ', ' + Isnull(l.Zip ,'') + ', Phone: ' + Isnull(r.Phone COLLATE DATABASE_DEFAULT,'')
                                         + ', Email: ' + Isnull(r.EMail COLLATE DATABASE_DEFAULT,'') )  AS [desc],
                                       o.SageID                    AS custsageid,
                                       r.ID                        AS rolid,
                                       ro.NAME  COLLATE DATABASE_DEFAULT  AS CompanyName,
                                       (SELECT CONVERT(VARCHAR(50), Rate + @GST_RATE)
                                        FROM   STax
                                        WHERE  NAME = L.STax)      AS STaxRate,
                                       L.STax,
                                       0                           AS ProspectID,
									   ISNULL(bt.Description,'')   AS BusinessType
                 FROM   loc l
                        LEFT OUTER JOIN Rol r ON l.Rol = r.ID
                        INNER JOIN owner o ON o.id = l.owner
                        LEFT OUTER JOIN Rol ro ON o.Rol = ro.ID
                        LEFT OUTER JOIN tblUserCo UC ON UC.CompanyID = r.EN
						LEFT OUTER JOIN BusinessType bt ON bt.ID = l.BusinessType
                 WHERE  l.status = 0
                        AND o.status = 0
                        AND r.type = 4
                        AND (
                            --Default Salesperson
                            Isnull(l.Terr, 0) = ( CASE
                                                    WHEN( @IsSalesAsigned > 0
                                                          AND @SalesAsignedTerrID > 0 ) THEN CONVERT(NVARCHAR(10), @SalesAsignedTerrID)
                                                    ELSE Isnull(l.Terr, 0)
                                                  END )
                             OR
                            --Second Salesperson
                            Isnull(l.Terr2, 0) = ( CASE
                                                     WHEN( @IsSalesAsigned > 0
                                                           AND @SalesAsignedTerrID > 0 ) THEN CONVERT(NVARCHAR(10), @SalesAsignedTerrID)
                                                     ELSE Isnull(l.Terr2, 0)
                                                   END ) )
                        AND ( ( dbo.Removespecialchars(Tag) LIKE '%' + @WOspacialchars + '%' )
                               OR ( dbo.Removespecialchars(l.ID) LIKE '%' + @WOspacialchars + '%' )
                               OR ( r.Contact LIKE '%' + @SearchText + '%' )
                               OR ( dbo.Removespecialchars (r.Address) LIKE '%' + @WOspacialchars + '%' )
                               OR ( r.City LIKE '%' + @SearchText + '%' )
                               OR ( r.State = +@SearchText )
                               OR ( r.Zip LIKE '%' + @SearchText + '%' )
                               OR ( dbo.Removespecialchars(r.Phone) LIKE '%' + @WOspacialchars + '%' )
                               OR ( r.EMail LIKE '%' + @SearchText + '%' )
                               OR ( dbo.Removespecialchars (l.Address) LIKE '%' + @WOspacialchars + '%' )
                               OR ( l.City LIKE '%' + @SearchText + '%' )
                               OR ( l.State = +@SearchText )
                               OR ( l.Zip LIKE '%' + @SearchText + '%' )
                               OR ( dbo.Removespecialchars(ro.NAME) LIKE '%' + @WOspacialchars + '%' )
                               OR ( ro.Contact LIKE '%' + @SearchText + '%' )
                               OR ( dbo.Removespecialchars (ro.Address) LIKE '%' + @WOspacialchars + '%' )
                               OR ( ro.City LIKE '%' + @SearchText + '%' )
                               OR ( ro.Zip LIKE '%' + @SearchText + '%' )
                               OR ( dbo.Removespecialchars(ro.Phone) LIKE '%' + @SearchText + '%' )
                               OR ( ro.EMail LIKE '%' + @SearchText + '%' )
                               OR ( ro.state = @SearchText ) )
                 UNION
                 SELECT DISTINCT TOP 100 0                         AS value,
                                         Isnull(r.NAME COLLATE DATABASE_DEFAULT, ' ')       AS label,
                                         ( 'Customer: ' + Isnull(o.CustomerName, ' ')
                                           + ', ' + Isnull(r.Contact COLLATE DATABASE_DEFAULT, '') + ', '
                                           + Isnull(r.Address COLLATE DATABASE_DEFAULT, '') + ', '
                                           + Isnull(r.City COLLATE DATABASE_DEFAULT, '') + ', '
                                           + Isnull(r.[State] COLLATE DATABASE_DEFAULT, '') + ', ' + Isnull(r.Zip COLLATE DATABASE_DEFAULT, '')
                                           + ', Phone: ' + Isnull(r.Phone COLLATE DATABASE_DEFAULT, '') + ', Email: '
                                           + Isnull(r.EMail COLLATE DATABASE_DEFAULT, '') ) AS [desc],
                                         ''                        AS custsageid,
                                         r.ID                      AS rolid,
                                         o.CustomerName            AS CompanyName,
                                         (SELECT CONVERT(VARCHAR(50), Rate)
                                          FROM   STax
                                          WHERE  NAME = o.STax)    AS STaxRate,
                                         o.STax,
                                         o.ID                      AS ProspectID,
                                         ISNULL(o.BusinessType,'') AS BusinessType
                 FROM   Prospect o
                        INNER JOIN Rol r
                                ON o.Rol = r.ID
                        LEFT OUTER JOIN tblUserCo UC
                                     ON UC.CompanyID = r.EN
                 WHERE  o.status = 0
                        AND r.Type = 3
                        AND Isnull(UC.IsSel, 0) = CASE @EN
                                                    WHEN 1 THEN 1
                                                    ELSE Isnull(UC.IsSel, 0)
                                                  END
                        AND Isnull(UC.UserID, 0) = CASE @EN
                                                     WHEN 1 THEN @UserID
                                                     ELSE Isnull(UC.UserID, 0)
                                                   END
                        AND ( r.NAME LIKE  case  when len(@WOspacialchars)> 0 then  ( '%' + @WOspacialchars + '%') else r.NAME end )
                        AND Isnull(o.Terr, 0) = ( CASE
                                                    WHEN( @IsSalesAsigned > 0
                                                          AND @SalesAsignedTerrID > 0 ) THEN CONVERT(NVARCHAR(10), @SalesAsignedTerrID)
                                                    ELSE Isnull(o.Terr, 0)
                                                  END ))x
          ORDER  BY x.label
      END
    ELSE
	  -- if real customer
	  IF @Prospects = 0
      BEGIN
          SELECT DISTINCT TOP 100 l.loc                       AS value,
                                  l.tag                       AS label,
                                  ( 'ID: ' + l.ID COLLATE DATABASE_DEFAULT + ', ' + 'Customer: ' + ro.NAME COLLATE DATABASE_DEFAULT + ', '
                                    + r.Contact COLLATE DATABASE_DEFAULT + ', ' + l.Address + ', ' + l.City + ', '
                                    + l.[State] + ', ' + l.Zip + ', Phone: ' + r.Phone COLLATE DATABASE_DEFAULT
                                    + ', Email: ' + r.EMail COLLATE DATABASE_DEFAULT ) AS [desc],
                                  o.SageID                    AS custsageid,
                                  r.ID                        AS rolid,
                                  ro.NAME COLLATE DATABASE_DEFAULT AS CompanyName,
                                  (SELECT CONVERT(VARCHAR(50), Rate + @GST_RATE)
                                   FROM   STax
                                   WHERE  NAME = L.STax)      AS STaxRate,
                                  L.STax,
                                  0                           AS ProspectID,
								  ISNULL(bt.Description,'')   AS BusinessType
          FROM   loc l
                 LEFT OUTER JOIN Rol r ON l.Rol = r.ID
                 INNER JOIN owner o ON o.id = l.owner
                 LEFT OUTER JOIN Rol ro ON o.Rol = ro.ID
                 LEFT OUTER JOIN tblUserCo UC ON UC.CompanyID = r.EN
                 LEFT OUTER JOIN BusinessType bt ON bt.ID = l.BusinessType
          WHERE  l.status = 0
                 AND o.status = 0
                 AND r.type = 4
                 AND (
                     --Default Salesperson
                     Isnull(l.Terr, 0) = ( CASE
                                             WHEN( @IsSalesAsigned > 0
                                                   AND @SalesAsignedTerrID > 0 ) THEN CONVERT(NVARCHAR(10), @SalesAsignedTerrID)
                                             ELSE Isnull(l.Terr, 0)
                                           END )
                      OR
                     --Second Salesperson
                     Isnull(l.Terr2, 0) = ( CASE
                                              WHEN( @IsSalesAsigned > 0
                                                    AND @SalesAsignedTerrID > 0 ) THEN CONVERT(NVARCHAR(10), @SalesAsignedTerrID)
                                              ELSE Isnull(l.Terr2, 0)
                                            END ) )
                 AND ( ( dbo.Removespecialchars(Tag) LIKE '%' + @WOspacialchars + '%' )
                        OR ( dbo.Removespecialchars(l.ID) LIKE '%' + @WOspacialchars + '%' )
                        OR ( r.Contact LIKE '%' + @SearchText + '%' )
                        OR ( dbo.Removespecialchars (r.Address) LIKE '%' + @WOspacialchars + '%' )
                        OR ( r.City LIKE '%' + @SearchText + '%' )
                        OR ( r.State = +@SearchText )
                        OR ( r.Zip LIKE '%' + @SearchText + '%' )
                        OR ( dbo.Removespecialchars(r.Phone) LIKE '%' + @WOspacialchars + '%' )
                        OR ( r.EMail LIKE '%' + @SearchText + '%' )
                        OR ( dbo.Removespecialchars (l.Address) LIKE '%' + @WOspacialchars + '%' )
                        OR ( l.City LIKE '%' + @SearchText + '%' )
                        OR ( l.State = +@SearchText )
                        OR ( l.Zip LIKE '%' + @SearchText + '%' )
                        OR ( dbo.Removespecialchars(ro.NAME) LIKE '%' + @WOspacialchars + '%' )
                        OR ( ro.Contact LIKE '%' + @SearchText + '%' )
                        OR ( dbo.Removespecialchars (ro.Address) LIKE '%' + @WOspacialchars + '%' )
                        OR ( ro.City LIKE '%' + @SearchText + '%' )
                        OR ( ro.Zip LIKE '%' + @SearchText + '%' )
                        OR ( dbo.Removespecialchars(ro.Phone) LIKE '%' + @SearchText + '%' )
                        OR ( ro.EMail LIKE '%' + @SearchText + '%' )
                        OR ( ro.state = @SearchText ) )
                 AND l.Owner = @CustomerID
          ORDER  BY tag
      END
	  ELSE -- if customer is prospect
	  BEGIN
		SELECT DISTINCT TOP 100 0                         AS value,
                                         Isnull(r.NAME, ' ')       AS label,
                                         ( 'Customer: ' + Isnull(o.CustomerName, ' ')
                                           + ', ' + Isnull(r.Contact COLLATE DATABASE_DEFAULT, '') + ', '
                                           + Isnull(r.Address COLLATE DATABASE_DEFAULT, '') + ', '
                                           + Isnull(r.City COLLATE DATABASE_DEFAULT, '') + ', '
                                           + Isnull(r.[State] COLLATE DATABASE_DEFAULT, '') + ', ' + Isnull(r.Zip COLLATE DATABASE_DEFAULT, '')
                                           + ', Phone: ' + Isnull(r.Phone COLLATE DATABASE_DEFAULT, '') + ', Email: '
                                           + Isnull(r.EMail, '') ) AS [desc],
                                         ''                        AS custsageid,
                                         r.ID                      AS rolid,
                                         o.CustomerName            AS CompanyName,
                                         (SELECT CONVERT(VARCHAR(50), Rate)
                                          FROM   STax
                                          WHERE  NAME = o.STax)    AS STaxRate,
                                         o.STax,
                                         o.ID                      AS ProspectID,
                                         ISNULL(o.BusinessType,'') AS BusinessType
                 FROM   Prospect o
                        INNER JOIN Rol r
                                ON o.Rol = r.ID
                        LEFT OUTER JOIN tblUserCo UC
                                     ON UC.CompanyID = r.EN
                 WHERE  o.status = 0
                        AND r.Type = 3
                        AND Isnull(UC.IsSel, 0) = CASE @EN
                                                    WHEN 1 THEN 1
                                                    ELSE Isnull(UC.IsSel, 0)
                                                  END
                        AND Isnull(UC.UserID, 0) = CASE @EN
                                                     WHEN 1 THEN @UserID
                                                     ELSE Isnull(UC.UserID, 0)
                                                   END
                        AND ( r.NAME LIKE  case  when len(@WOspacialchars)> 0 then  ( '%' + @WOspacialchars + '%') else r.NAME end )
                        AND Isnull(o.Terr, 0) = ( CASE
                                                    WHEN( @IsSalesAsigned > 0
                                                          AND @SalesAsignedTerrID > 0 ) THEN CONVERT(NVARCHAR(10), @SalesAsignedTerrID)
                                                    ELSE Isnull(o.Terr, 0)
                                                  END )
						AND o.ID = @CustomerID
	  END
GO


