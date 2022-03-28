CREATE PROC [dbo].[spGetTaskRolSearch] 
@SearchText     VARCHAR(50),
@EN int			=0,
@UserID int		=0,
@IsSalesAsigned INT =0
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

    DECLARE @WOspacialchars VARCHAR(50)

    SET @WOspacialchars = dbo.Removespecialchars(@SearchText)
IF(@EN = 1) ---- If User is company Access
      BEGIN
    SELECT DISTINCT ( CASE r.Type
                        WHEN 0 THEN 'Customer'
                        WHEN 1 THEN 'Vendor'
                        WHEN 2 THEN 'Bank'
                        WHEN 3 THEN 'Lead'
                        WHEN 4 THEN 'Existing'
                        WHEN 5 THEN 'Employee'
                        ELSE 'Misc'
                      END )                    AS type,
                    r.Type                     AS prospect,
                    r.ID                       AS value,
                    Isnull(r.NAME, '')         AS label,
                    ( Isnull( r.Contact, '') + ', '
                      + Isnull( r.Address, '') + ', '
                      + Isnull( r.City, '') + ', '
                      + Isnull( r.[State], '') + ', ' + Isnull( r.Zip, '')
                      + ', Phone: ' + Isnull( r.Phone, '') + ', Email: '
                      + Isnull( r.EMail, '') ) AS [desc],
                    CASE r.Type
                      WHEN 4 THEN (SELECT TOP 1 Loc
                                   FROM   Loc
                                   WHERE  Rol = r.ID)
                      WHEN 3 THEN (SELECT TOP 1 ID
                                   FROM   Prospect
                                   WHERE  Rol = r.ID)
                    END                        AS Custom2,
                    CASE r.Type
                      WHEN 4 THEN (SELECT TOP 1 Stax
                                   FROM   Loc
                                   WHERE  Rol = r.ID)
                      WHEN 3 THEN (SELECT TOP 1 Stax
                                   FROM   Prospect
                                   WHERE  Rol = r.ID)
                    END                        AS STax
    FROM   Rol r LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN 
    WHERE UC.IsSel = 1 and UC.UserID = @UserID AND r.type IN ( 3, 4 )
           AND ( ( dbo.Removespecialchars(r.NAME) LIKE '%' + @WOspacialchars + '%' )
                  OR ( r.Contact LIKE '%' + @SearchText + '%' )
                  OR ( dbo.Removespecialchars (r.Address) LIKE '%' + @WOspacialchars + '%' )
                  OR ( r.City LIKE '%' + @SearchText + '%' )
                  OR ( r.Zip LIKE '%' + @SearchText + '%' )
                  OR ( dbo.Removespecialchars(r.Phone) LIKE '%' + @SearchText + '%' )
                  OR ( r.EMail LIKE '%' + @SearchText + '%' )
                  OR ( r.state = @SearchText ) )
           AND r.ID = CASE @IsSalesAsigned
                        WHEN 0 THEN r.ID
                        ELSE ( CASE r.type
                                 WHEN 3 THEN (SELECT TOP 1 Rol
                                              FROM   Prospect
                                              WHERE  Prospect.Terr = CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))
                                                     AND Prospect.Rol = r.ID)
                                 WHEN 4 THEN (SELECT TOP 1 Rol
                                              FROM   loc
                                              WHERE   loc.Rol = r.ID											  
											  and (											  
											  isnull(loc.Terr,0) = CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))
											  or 
											  isnull(loc.Terr2,0) = CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))
											  )
											  )
                                 ELSE r.ID
                               END )
                      END
    ORDER  BY r.Type,
              Isnull(r.NAME, '')
END
ELSE
BEGIN
 SELECT DISTINCT ( CASE r.Type
                        WHEN 0 THEN 'Customer'
                        WHEN 1 THEN 'Vendor'
                        WHEN 2 THEN 'Bank'
                        WHEN 3 THEN 'Lead'
                        WHEN 4 THEN 'Existing'
                        WHEN 5 THEN 'Employee'
                        ELSE 'Misc'
                      END )                    AS type,
                    r.Type                     AS prospect,
                    r.ID                       AS value,
                    Isnull(r.NAME, '')         AS label,
                    ( Isnull( r.Contact, '') + ', '
                      + Isnull( r.Address, '') + ', '
                      + Isnull( r.City, '') + ', '
                      + Isnull( r.[State], '') + ', ' + Isnull( r.Zip, '')
                      + ', Phone: ' + Isnull( r.Phone, '') + ', Email: '
                      + Isnull( r.EMail, '') ) AS [desc],
                    CASE r.Type
                      WHEN 4 THEN (SELECT TOP 1 Loc
                                   FROM   Loc
                                   WHERE  Rol = r.ID)
                      WHEN 3 THEN (SELECT TOP 1 ID
                                   FROM   Prospect
                                   WHERE  Rol = r.ID)
                    END                        AS Custom2,
                    CASE r.Type
                      WHEN 4 THEN (SELECT TOP 1 Stax
                                   FROM   Loc
                                   WHERE  Rol = r.ID)
                      WHEN 3 THEN (SELECT TOP 1 Stax
                                   FROM   Prospect
                                   WHERE  Rol = r.ID)
                    END                        AS STax
    FROM   Rol r LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN
    WHERE  r.type IN ( 3, 4 )
           AND ( ( dbo.Removespecialchars(r.NAME) LIKE '%' + @WOspacialchars + '%' )
                  OR ( r.Contact LIKE '%' + @SearchText + '%' )
                  OR ( dbo.Removespecialchars (r.Address) LIKE '%' + @WOspacialchars + '%' )
                  OR ( r.City LIKE '%' + @SearchText + '%' )
                  OR ( r.Zip LIKE '%' + @SearchText + '%' )
                  OR ( dbo.Removespecialchars(r.Phone) LIKE '%' + @SearchText + '%' )
                  OR ( r.EMail LIKE '%' + @SearchText + '%' )
                  OR ( r.state = @SearchText ) )
           AND r.ID = CASE @IsSalesAsigned
                        WHEN 0 THEN r.ID
                        ELSE ( CASE r.type
                                 WHEN 3 THEN (SELECT TOP 1 Rol
                                              FROM   Prospect
                                              WHERE  Prospect.Terr = CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))
                                                     AND Prospect.Rol = r.ID)
                                 WHEN 4 THEN (SELECT TOP 1 Rol
                                              FROM   loc
                                              WHERE   loc.Rol = r.ID											  
											  and (											  
											  isnull(loc.Terr,0) = CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))
											  or 
											  isnull(loc.Terr2,0) = CONVERT(NVARCHAR(10), (@SalesAsignedTerrID))
											  )
											  )
                                 ELSE r.ID
                               END )
                      END
    ORDER  BY r.Type,
              Isnull(r.NAME, '')
END
GO