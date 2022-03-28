CREATE proc [dbo].[spGetTaskContactsSearch]

@SearchText varchar(50)

as

declare @WOspacialchars varchar(50) 

set @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)

 select  
 distinct 
 0 as prospect, 
 r.ID as value,
 r.Name as label,
 (ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc] 
 from [Owner] o 
 left outer join Rol r on o.Rol=r.ID 
 left outer join Loc l on l.Owner=o.ID  
 left outer join Rol rl on l.Rol=rl.ID   
 where o.status = 0 and
 (
 (dbo.RemoveSpecialChars( r.NAME ) LIKE '%'+@WOspacialchars+'%') 
 OR (r.Contact LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars ( r.Address ) LIKE '%'+@WOspacialchars+'%')  
 OR (r.City LIKE '%'+@SearchText+'%')  
 OR (r.Zip LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%'+@SearchText+'%') 
 OR (r.EMail LIKE '%'+@SearchText+'%') 
 OR (r.state = @SearchText) 
   
 OR (dbo.RemoveSpecialChars( l.tag ) LIKE '%'+@WOspacialchars +'%')  
 OR (dbo.RemoveSpecialChars( l.ID) LIKE '%'+@WOspacialchars  +'%') 
 OR (dbo.RemoveSpecialChars( l.Address ) LIKE '%'+@WOspacialchars +'%')  
 OR (l.City LIKE '%'+@SearchText+'%')   
 OR (l.Zip LIKE '%'+@SearchText+'%') 
 OR (l.State = @SearchText)
 
 OR (rl.Contact LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars( rl.Address ) LIKE '%'+@WOspacialchars +'%')  
 OR (rl.City LIKE '%'+@SearchText+'%')   
 OR (rl.Zip LIKE '%'+@SearchText+'%')  
 OR (dbo.RemoveSpecialChars(rl.Phone) LIKE '%'+@SearchText+'%')  
 OR (rl.EMail LIKE '%'+@SearchText+'%')  
 OR (rl.state = @SearchText) )  
 
 union
   
 select distinct 
 1 as prospect, 
 r.ID as value,
 r.Name as label, 
 (ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc] 
 from Prospect o 
 left outer join Rol r on o.Rol=r.ID  
 where 
 o.status = 0 and
 ((dbo.RemoveSpecialChars(NAME ) LIKE '%'+@WOspacialchars+'%') 
 OR (Contact LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars(r. Address)  LIKE '%'+@WOspacialchars+'%')  
 OR (r.City LIKE '%'+@SearchText+'%')  
 OR (r.Zip LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%'+@WOspacialchars+'%') 
 OR (r.EMail LIKE '%'+@SearchText+'%') 
 OR (r.State = @SearchText)
  OR (dbo.RemoveSpecialChars(o. Address)  LIKE '%'+@WOspacialchars+'%')  
 OR (o.City LIKE '%'+@SearchText+'%')  
 OR (o.Zip LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars(o.Phone) LIKE '%'+@WOspacialchars+'%') 
 OR (o.State = @SearchText)) 
 
 union
 
 select distinct 
  2 as prospect,
 r.ID as value,
 l.tag as label,
 ('Customer: '+ro.Name+', '+r.Contact+', '+l.Address+', '+l.City+', '+l.[State]+', '+l.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc]
 from loc l 
 left outer join Rol r on l.Rol=r.ID  
 inner join owner o on o.id = l.owner 
 left outer join Rol ro on o.Rol=ro.ID  
 where l.status = 0 and o.status = 0 and r.type=4 
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
 
 
 order by r.name
