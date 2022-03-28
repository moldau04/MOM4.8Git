CREATE PROCEDURE [dbo].[spGetLocationSearchWithInactive]
@SearchText varchar(50),
@CustomerID int,
@EN int			=0,
@UserID int		= 0,
@IsSalesAsigned int =0
AS
DECLARE @WOspacialchars varchar(50) 
DECLARE @SalesAsignedTerrID int = 0

-- GST CHECK AS PER COUNTRY
	DECLARE @Country AS VARCHAR(10)
	DECLARE @GST_RATE AS DECIMAL
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
Declare @CompText As Varchar(50)
IF(@EN = 1)
      BEGIN
          SET @CompText =' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)                    
      END
	  Else
	  BEGIN
	  SET @CompText =' '
	  END
if( @IsSalesAsigned > 0)----If USER IS Salesperson
BEGIN
SELECT @SalesAsignedTerrID=isnull(id,0) 
FROM  Terr 
WHERE Name=(SELECT fUser FROM  tblUser WHERE id=@IsSalesAsigned)
END
SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)
IF(@EN = 1)
      BEGIN
if(@CustomerID = 0)
begin
SELECT DISTINCT TOP 100 l.loc as value,l.tag as label,('ID: '+l.ID+', '+'Customer: '+ro.Name+', '+r.Contact+', '+l.Address+', '+l.City+', '+l.[State]+', '+l.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc]
 , o.SageID as custsageid, r.ID as rolid,ro.Name as CompanyName,(select CONVERT(varchar(50), Rate+@GST_RATE)  from STax WHERE UType=0 and NAME=L.STax) AS STaxRate,L.STax,l.ID ,o.ID AS OwnerID,l.loc,l.status AS LocStatus
 from loc l 
 left outer join Rol r on l.Rol=r.ID  
 inner join owner o on o.id = l.owner 
 left outer join Rol ro on o.Rol=ro.ID  
 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN 
 where --l.status = 0 and o.status = 0  and 
 r.type=4 
  and (
  --Default Salesperson
  isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) 
  then convert(nvarchar(10),@SalesAsignedTerrID) 
  else isnull(l.Terr,0)  end )
  or 
  --Second Salesperson
  isnull(l.Terr2,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) 
  then convert(nvarchar(10),@SalesAsignedTerrID) 
  else isnull(l.Terr2,0)  end )  
  )
 and ( 
  (dbo.RemoveSpecialChars( Tag) LIKE '%'+@WOspacialchars+'%') 
  OR (dbo.RemoveSpecialChars( l.ID) LIKE '%'+@WOspacialchars+'%') 
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
	OR (ro.state = @SearchText) 
  )
   and UC.IsSel = 1 and UC.UserID = @UserID
  order by tag
  
  end
  
  else 
  begin
  select distinct TOP 100 l.loc as value,l.tag as label,('ID: '+l.ID+', '+'Customer: '+ro.Name+', '+r.Contact+', '+l.Address+', '+l.City+', '+l.[State]+', '+l.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc]
 , o.SageID as custsageid,r.ID as rolid,ro.Name as CompanyName, (select CONVERT(varchar(50), Rate+@GST_RATE)   from STax WHERE UType=0 and NAME=L.STax) AS STaxRate,L.STax,l.ID ,o.ID  AS OwnerID,l.loc,l.Status as LocStatus
 from loc l 
 left outer join Rol r on l.Rol=r.ID  
 inner join owner o on o.id = l.owner 
 left outer join Rol ro on o.Rol=ro.ID
 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN   
 where --l.status = 0 and o.status = 0  and 
 r.type=4 
 and 
 (
  --Default Salesperson
 isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) 
 then convert(nvarchar(10),@SalesAsignedTerrID) 
 else isnull(l.Terr,0)  end )

 or
  --Second Salesperson
 isnull(l.Terr2,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) 
 then convert(nvarchar(10),@SalesAsignedTerrID) 
 else isnull(l.Terr2,0)  end )
 )
 and ( 
  (dbo.RemoveSpecialChars( Tag) LIKE '%'+@WOspacialchars+'%') 
  OR (dbo.RemoveSpecialChars( l.ID) LIKE '%'+@WOspacialchars+'%') 
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
	OR (ro.state = @SearchText) 
  )
  and l.Owner =@CustomerID and UC.IsSel = 1 and UC.UserID = @UserID
  
  order by tag
    
  end
END
ELSE
BEGIN
if(@CustomerID = 0)
begin
SELECT DISTINCT TOP 100 l.loc as value,l.tag as label,('ID: '+l.ID+', '+'Customer: '+ro.Name+', '+r.Contact+', '+l.Address+', '+l.City+', '+l.[State]+', '+l.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc]
 , o.SageID as custsageid, r.ID as rolid,ro.Name as CompanyName,(select CONVERT(varchar(50), Rate+@GST_RATE)   from STax WHERE UType=0 and NAME=L.STax) AS STaxRate,L.STax,l.ID ,o.ID AS OwnerID,l.loc,l.Status as LocStatus
 from loc l 
 left outer join Rol r on l.Rol=r.ID  
 inner join owner o on o.id = l.owner 
 left outer join Rol ro on o.Rol=ro.ID  
 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN 
 where --l.status = 0 and o.status = 0  and 
 r.type=4 
  and (
  --Default Salesperson
  isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) 
  then convert(nvarchar(10),@SalesAsignedTerrID) 
  else isnull(l.Terr,0)  end )
  or 
  --Second Salesperson
  isnull(l.Terr2,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) 
  then convert(nvarchar(10),@SalesAsignedTerrID) 
  else isnull(l.Terr2,0)  end )  
  )
 and ( 
  (dbo.RemoveSpecialChars( Tag) LIKE '%'+@WOspacialchars+'%') 
  OR (dbo.RemoveSpecialChars( l.ID) LIKE '%'+@WOspacialchars+'%') 
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
	OR (ro.state = @SearchText) 
  )
  
  order by tag
  
  end
  
  else 
  begin
  select distinct TOP 100 l.loc as value,l.tag as label,('ID: '+l.ID+', '+'Customer: '+ro.Name+', '+r.Contact+', '+l.Address+', '+l.City+', '+l.[State]+', '+l.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc]
 , o.SageID as custsageid,r.ID as rolid,ro.Name as CompanyName, (select CONVERT(varchar(50), Rate+@GST_RATE)   from STax WHERE UType=0 and NAME=L.STax) AS STaxRate,L.STax,l.ID ,o.ID AS OwnerID,l.loc,l.Status as LocStatus
 from loc l 
 left outer join Rol r on l.Rol=r.ID  
 inner join owner o on o.id = l.owner 
 left outer join Rol ro on o.Rol=ro.ID  
 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN 
 where --l.status = 0 and o.status = 0  and 
 r.type=4 
 and 
 (
  --Default Salesperson
 isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) 
 then convert(nvarchar(10),@SalesAsignedTerrID) 
 else isnull(l.Terr,0)  end )

 or
  --Second Salesperson
 isnull(l.Terr2,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) 
 then convert(nvarchar(10),@SalesAsignedTerrID) 
 else isnull(l.Terr2,0)  end )
 )
 and ( 
  (dbo.RemoveSpecialChars( Tag) LIKE '%'+@WOspacialchars+'%') 
  OR (dbo.RemoveSpecialChars( l.ID) LIKE '%'+@WOspacialchars+'%') 
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
	OR (ro.state = @SearchText) 
  )
  and l.Owner =@CustomerID
  
  order by tag
    
  end
END