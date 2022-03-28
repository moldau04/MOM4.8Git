CREATE proc  [dbo].[spGetLocationSearch]	
	--DECLARE
	@SearchText varchar(50)='',
	@CustomerID int=0,
	@EN int			=0,
	@UserID int		= 1,
	@IsSalesAsigned int =0
AS
DECLARE @WOspacialchars varchar(50) 
DECLARE @SalesAsignedTerrID int = 0

-- GST CHECK AS PER COUNTRY
DECLARE @Country AS VARCHAR(10)
DECLARE @GST_RATE AS DECIMAL

SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)
SET @Country=(SELECT Label FROM Custom WHERE NAME='Country')
IF @Country='1'
	BEGIN
		SET @GST_RATE=(SELECT  CAST(Label as decimal) FROM Custom WHERE NAME='GSTRate')
	END
ELSE
	BEGIN
		SET @GST_RATE=0
	END
-- GST CHECK AS PER COUNTRY
--- Company Check
DECLARE @CompText As Varchar(50)
 


      select   TOP 50 l.loc as value,l.tag as label,('ID: '+l.ID+', '+'Customer: '+ro.Name+', '+r.Contact+', '+l.Address+', '+l.City+', '+l.[State]+', '+l.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc]
	  , o.SageID as custsageid,r.ID as rolid,ro.Name as CompanyName, (select CONVERT(varchar(50), Rate+@GST_RATE)   from STax WHERE UType=0 and NAME=L.STax) AS STaxRate,L.STax
		from loc l 
		inner join Rol r on l.Rol=r.ID  
		inner join owner o on o.id = l.owner 
		inner join Rol ro on o.Rol=ro.ID  
	    LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN 
		where l.status = 0 and o.status = 0  and r.type=4 
			and 
			(
				--Default Salesperson
				isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) 
				then convert(nvarchar(10),@SalesAsignedTerrID)  else isnull(l.Terr,0)  end )

				or
				--Second Salesperson
				isnull(l.Terr2,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) 
				then convert(nvarchar(10),@SalesAsignedTerrID)	else isnull(l.Terr2,0)  end )
			)
			and ( 
				(dbo.RemoveSpecialChars(Tag) LIKE '%'+@WOspacialchars+'%') 
				OR (dbo.RemoveSpecialChars(l.ID) LIKE '%'+@WOspacialchars+'%') 
				OR (r.Contact LIKE '%'+@SearchText+'%') 
				OR (dbo.RemoveSpecialChars ( r.Address) LIKE '%'+@WOspacialchars+'%')  
				OR (r.City LIKE '%'+@SearchText+'%')  
				OR (r.State = +@SearchText)  
				OR (r.Zip LIKE '%'+@SearchText+'%') 
				OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%'+@WOspacialchars+'%') 
				OR (r.EMail LIKE '%'+@SearchText+'%')  
				OR (dbo.RemoveSpecialChars ( l.Address) LIKE '%'+@WOspacialchars+'%')  
				OR (l.City LIKE '%'+@SearchText+'%')  
				OR (l.State = +@SearchText)  
				OR (l.Zip LIKE '%'+@SearchText+'%')   
				OR (dbo.RemoveSpecialChars(ro.Name) LIKE '%'+@WOspacialchars+'%')
				OR (ro.Contact LIKE '%'+@SearchText+'%') 
				OR (dbo.RemoveSpecialChars ( ro.Address ) LIKE '%'+@WOspacialchars+'%')  
				OR (ro.City LIKE '%'+@SearchText+'%')  
				OR (ro.Zip LIKE '%'+@SearchText+'%') 
				OR (dbo.RemoveSpecialChars(ro.Phone) LIKE '%'+@SearchText+'%') 
				OR (ro.EMail LIKE '%'+@SearchText+'%') 
				OR (ro.state = @SearchText
				 
				) 
			)

			and l.Owner =case @CustomerID when 0 then l.Owner else @CustomerID end

			-----If User is company Access

			and isnull(UC.IsSel,0) = case @EN when 1 then 1 else   isnull(UC.IsSel,0) end 
			
			and isnull(UC.UserID,0) =case @EN when 1 then @UserID else  isnull(UC.UserID,0) end

		order by tag
 
GO