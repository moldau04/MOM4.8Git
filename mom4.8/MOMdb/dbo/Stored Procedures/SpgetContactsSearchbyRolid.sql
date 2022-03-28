/*--------------------------------------------------------------------
Modified By: Thomas
Modified On: 27 Feb 2020	
Description: 

Modified By: Thurstan
Modified On: 22 Dec Nov 2018	
Description: Re-Create sp SpgetContactsSearchbyRolid
--------------------------------------------------------------------*/
CREATE PROC [dbo].[SpgetContactsSearchbyRolid] @SearchText VARCHAR(50),
                                               @RolID      INT=0
AS
    DECLARE @WOspacialchars VARCHAR(50)

    SET @WOspacialchars = dbo.Removespecialchars(@SearchText)

    SELECT DISTINCT top 50 p.ID  AS value,
        p.fDesc AS label,
        (Isnull( p.Title, '') + ', ' + Isnull( p.Phone, '') + ', ' + Isnull( p.Cell, '') + ', ' + Isnull( p.Email, '') ) AS [desc],
		p.Phone,
		p.Email,
        p.Fax,
        p.Cell,
        p.Title
    FROM   [Phone] p
    WHERE  p.Rol = @RolID
           AND ( p.fDesc LIKE '%' + @WOspacialchars + '%'
                  OR ( p.Phone LIKE '%' + @SearchText + '%' )
                  OR ( p.Title LIKE '%' + @SearchText + '%' )
                  OR ( p.Email LIKE '%' + @SearchText + '%' )
                  OR ( p.Cell LIKE '%' + @SearchText + '%' ) )

	--SELECT DISTINCT top 50 * FROM (
	--SELECT p.ID    AS value,
 --                   p.fDesc    AS label,
 --                   (  Isnull( p.Title, '')
 --                     + ', ' + Isnull( p.Phone, '') + ', '
 --                     + Isnull( p.Cell, '') + ', ' + Isnull( p.Email, '') ) AS [desc],
	--				p.Phone,
	--				p.Email
 --   FROM   [Phone] p
 --   WHERE  p.Rol = @RolID
 --          AND ( p.fDesc LIKE '%' + @WOspacialchars + '%'
 --                 OR ( p.Phone LIKE '%' + @SearchText + '%' )
 --                 OR ( p.Title LIKE '%' + @SearchText + '%' )
 --                 OR ( p.Email LIKE '%' + @SearchText + '%' )
 --                 OR ( p.Cell LIKE '%' + @SearchText + '%' ) )
	--UNION 
	--SELECT 0, contact, 'Main Contact', Phone, EMail
	--FROM rol r
	--WHERE r.id =@RolID AND r.Contact is not null AND r.Contact != ''
	--	AND (r.Contact LIKE '%' + @WOspacialchars + '%'
	--			  OR ( r.Phone LIKE '%' + @SearchText + '%' )
 --                 OR ( r.Email LIKE '%' + @SearchText + '%' )
 --                 OR ( r.Cellular LIKE '%' + @SearchText + '%' ) )

	--) as ContactList
GO


