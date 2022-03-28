CREATE PROCEDURE [dbo].[spGetCustomerSearchWithInactive]
@SearchText varchar(50),
@Prospects int,
@EN int			=0,
@UserID int		= 0,
@IsSalesAsigned int =0

as

declare @WOspacialchars varchar(50) 
DECLARE @SalesAsignedTerrID int = 0
if( @IsSalesAsigned > 0)----If USER IS Salesperson
BEGIN
SELECT @SalesAsignedTerrID=isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=@IsSalesAsigned)
END
set @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)
IF(@EN = 1) ---- If User is company Access
      BEGIN
if(@Prospects=1)
begin
 select
 distinct TOP 50
 0 as prospect, 
 o.ID as value,
 r.Name as label,
 (ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc], r.ID as rolid,
  (SELECT CONVERT(VARCHAR(50), Rate)  FROM   STax  WHERE UType=0 and  NAME = (select top 1 STax from loc where Owner=o.id))     AS STaxRate,
                (select top 1 STax from loc where Owner=o.id) as STax
 from [Owner] o 
 left outer join Rol r on o.Rol=r.ID 
 left outer join Loc l on l.Owner=o.ID  
 left outer join Rol rl on l.Rol=rl.ID  
 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN 
 where 
 --o.status=0 and 
 UC.IsSel = 1 and UC.UserID = @UserID
 and
 ( 
 -------For Default Salesperson
 isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr,0)  end )
 or 
  -------For Second Salesperson
 isnull(l.Terr2,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr2,0)  end )
 )
  and
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
   
 select  distinct TOP 50
 1 as prospect, o.ID as value,o.CustomerName as label --r.Name as label
 , (ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc], r.ID as rolid,
 (select CONVERT(varchar(50), Rate)   from STax WHERE UType=0 and NAME=o.STax) AS STaxRate,o.STax
 from Prospect o 
 left outer join Rol r on o.Rol=r.ID
 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN
 where o.status=0 and UC.IsSel = 1 and UC.UserID = @UserID and ((dbo.RemoveSpecialChars(NAME ) LIKE '%'+@WOspacialchars+'%') 
 OR (Contact LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars(r. Address)  LIKE '%'+@WOspacialchars+'%')  
 OR (r.City LIKE '%'+@SearchText+'%')  
 OR (r.Zip LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%'+@WOspacialchars+'%') 
 OR (r.EMail LIKE '%'+@SearchText+'%') 
 OR (r.State = @SearchText)) 
 and  isnull(o.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(o.Terr,0)  end ) 
 order by r.name
 
 end
 else
 begin
  select  
 distinct TOP 100
 0 as prospect, 
 o.ID as value,
 r.Name as label,
 (ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc], r.ID as rolid,
  (SELECT CONVERT(VARCHAR(50), Rate)
                 FROM   STax 
                 WHERE UType=0 and NAME = (select top 1 STax from loc where Owner=o.id))     AS STaxRate,
                (select top 1 STax from loc where Owner=o.id) as STax
 from [Owner] o 
 left outer join Rol r on o.Rol=r.ID 
 left outer join Loc l on l.Owner=o.ID  
 left outer join Rol rl on l.Rol=rl.ID
 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN   
 where --o.status=0 and 
 UC.IsSel = 1 and UC.UserID = @UserID
 and (
  -------For Default Salesperson
 isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr,0)  end )
  or
   -------For Second Salesperson
 isnull(l.Terr2,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr2,0)  end )
  )and
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
 
 order by r.name
 end

 END
 ELSE
 BEGIN
 if(@Prospects=1)
begin

 select
 distinct TOP 50
 0 as prospect, 
 o.ID as value,
 r.Name as label,
 (ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc], r.ID as rolid,
  (SELECT CONVERT(VARCHAR(50), Rate)  FROM   STax  WHERE UType=0 and  NAME = (select top 1 STax from loc where Owner=o.id))     AS STaxRate,
                (select top 1 STax from loc where Owner=o.id) as STax
 from [Owner] o 
 left outer join Rol r on o.Rol=r.ID 
 left outer join Loc l on l.Owner=o.ID  
 left outer join Rol rl on l.Rol=rl.ID
 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN   
 --where o.status=0
 and
 ( 
 -------For Default Salesperson
 isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr,0)  end )
 or 
  -------For Second Salesperson
 isnull(l.Terr2,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr2,0)  end )
 )
  and
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
   
 select  distinct TOP 50
 1 as prospect, o.ID as value,o.CustomerName as label  --r.Name as label
 , (ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc], r.ID as rolid,
 (select CONVERT(varchar(50), Rate)   from STax WHERE UType=0 and NAME=o.STax) AS STaxRate,o.STax
 from Prospect o 
 left outer join Rol r on o.Rol=r.ID
 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN  
 where o.status=0 and ((dbo.RemoveSpecialChars(NAME ) LIKE '%'+@WOspacialchars+'%') 
 OR (Contact LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars(r. Address)  LIKE '%'+@WOspacialchars+'%')  
 OR (r.City LIKE '%'+@SearchText+'%')  
 OR (r.Zip LIKE '%'+@SearchText+'%') 
 OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%'+@WOspacialchars+'%') 
 OR (r.EMail LIKE '%'+@SearchText+'%') 
 OR (r.State = @SearchText)) 
 and  isnull(o.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(o.Terr,0)  end ) 
 order by r.name
 
 end
 else
 begin
 
  select  
 distinct TOP 100
 0 as prospect, 
 o.ID as value,
 r.Name as label,
 (ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc], r.ID as rolid,
  (SELECT CONVERT(VARCHAR(50), Rate)
                 FROM   STax
                 WHERE UType=0 and  NAME = (select top 1 STax from loc where Owner=o.id))     AS STaxRate,
                (select top 1 STax from loc where Owner=o.id) as STax
 from [Owner] o 
 left outer join Rol r on o.Rol=r.ID 
 left outer join Loc l on l.Owner=o.ID  
 left outer join Rol rl on l.Rol=rl.ID
 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN   
 where --o.status=0 and 
 (
  -------For Default Salesperson
 isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr,0)  end )
  or
   -------For Second Salesperson
 isnull(l.Terr2,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr2,0)  end )
  )and
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
 
 order by r.name
 end

 END
