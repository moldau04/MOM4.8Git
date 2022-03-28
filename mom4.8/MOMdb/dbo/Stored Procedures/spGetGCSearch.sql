CREATE proc [dbo].[spGetGCSearch]
@SearchText varchar(50)
as

declare @WOspacialchars varchar(50) 
set @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)

select    
 0 as prospect, 
 o.ID as value,
 r.Name as label,
 (ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc] 
 ,r.ID as rolid,r.city, r.state, r.country,r.zip,r.contact,r.phone,r.fax,r.email,r.cellular,r.remarks
 from [Owner] o 
 left outer join Rol r on o.Rol=r.ID      
 where o.status=0 and o.Type ='General Contractor' and 
 (
 (dbo.RemoveSpecialChars( r.NAME ) LIKE '%'+@WOspacialchars+'%') 
 OR (r.Contact LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars ( r.Address ) LIKE '%'+@WOspacialchars+'%')  
 OR (r.City LIKE '%'+@SearchText+'%')  
 OR (r.Zip LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%'+@SearchText+'%') 
 OR (r.EMail LIKE '%'+@SearchText+'%') 
 OR (r.state = @SearchText)  
 )
 
 order by r.name