create PROC [dbo].[spGetHomeOwnerSearch] @SearchText VARCHAR(50) ,@type VARCHAR(10)
AS
    DECLARE @WOspacialchars VARCHAR(50),
			@OwnerType varchar(50)
			
    SET @WOspacialchars = DBO.Removespecialchars( @SearchText )
	if(@type=1)
	set @OwnerType='General Contractor'
	else if(@type=2)
	set @OwnerType='Homeowner'

    SELECT top 50 LOCADDCON.RolID               AS GC_HOid,
           R.NAME                     AS label,
           ( Isnull( R.Contact, '') + ', '
             + Isnull( R.Address, '') + ', '
             + Isnull( R.City, '') + ', '
             + Isnull( R.[State], '') + ', ' + Isnull( R.Zip, '')
             + ', Phone: ' + Isnull( R.Phone, '') + ', Email: '
             + Isnull( R.EMail, '') ) AS [desc],
           R.ID                       AS rolid,
           R.city,
           R.state,
           R.country,
           R.zip,
           R.contact,
           R.phone,
           R.fax,
           R.email,
           R.cellular,
           R.remarks,
		   R.Address
    FROM   [tblLocAddlContact] LOCADDCON
           INNER JOIN Rol R
                   ON LOCADDCON.RolID = R.ID
    WHERE  LOCADDCON.LocContactTypeID = @type
           AND ( ( DBO.Removespecialchars( R.NAME ) LIKE '%' + @WOspacialchars + '%' )
                  OR ( R.Contact LIKE '%' + @SearchText + '%' )
                  OR ( DBO.Removespecialchars ( R.Address ) LIKE '%' + @WOspacialchars + '%' )
                  OR ( R.City LIKE '%' + @SearchText + '%' )
                  OR ( R.Zip LIKE '%' + @SearchText + '%' )
                  OR ( DBO.Removespecialchars( R.Phone ) LIKE '%' + @SearchText + '%' )
                  OR ( R.EMail LIKE '%' + @SearchText + '%' )
                  OR ( R.state = @SearchText ) )
    --ORDER  BY R.NAME 
	union all 
	SELECT top 50 R.ID                AS GC_HOid,
           R.NAME                     AS label,
           ( Isnull( R.Contact, '') + ', '
             + Isnull( R.Address, '') + ', '
             + Isnull( R.City, '') + ', '
             + Isnull( R.[State], '') + ', ' + Isnull( R.Zip, '')
             + ', Phone: ' + Isnull( R.Phone, '') + ', Email: '
             + Isnull( R.EMail, '') ) AS [desc],
           R.ID                       AS rolid,
           R.city,
           R.state,
           R.country,
           R.zip,
           R.contact,
           R.phone,
           R.fax,
           R.email,
           R.cellular,
           R.remarks,
		   R.Address
    FROM   [Owner] o 
 left outer join Rol r on o.Rol=r.ID      
 where o.status=0 
 and o.Type =@OwnerType 
 and r.id not in (select RolID from tblLocAddlContact)
           AND ( ( DBO.Removespecialchars( R.NAME ) LIKE '%' + @WOspacialchars + '%' )
                  OR ( R.Contact LIKE '%' + @SearchText + '%' )
                  OR ( DBO.Removespecialchars ( R.Address ) LIKE '%' + @WOspacialchars + '%' )
                  OR ( R.City LIKE '%' + @SearchText + '%' )
                  OR ( R.Zip LIKE '%' + @SearchText + '%' )
                  OR ( DBO.Removespecialchars( R.Phone ) LIKE '%' + @SearchText + '%' )
                  OR ( R.EMail LIKE '%' + @SearchText + '%' )
                  OR ( R.state = @SearchText ) )
    ORDER  BY R.NAME 
