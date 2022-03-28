CREATE PROCEDURE [dbo].[spGetLocations]
@SearchBy varchar(20)= null ,
@SearchValue varchar(100) = null,
@DbName varchar(50)=null,
@IsSalesAsigned int =0,
 @EN int				=0,
@UserID int		= 0
AS

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'
DECLARE @SalesAsignedTerrID int = 0
if( @IsSalesAsigned > 0)
BEGIN
SELECT @SalesAsignedTerrID=isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=@IsSalesAsigned)
END
set @Text= 
'select distinct loc , LTRIM(RTRIM(l.ID))  as locid, LTRIM(RTRIM(r.Name)) as Name,l.Type,l.Status,(select count(1) from '+@DbName+'elev e where e.loc=l.loc) as Elevs,isnull(Balance,0) as Balance, LTRIM(RTRIM(Tag)) as Tag,l.Address,l.City,
(select count(1) from '+@DbName+'ticketo t where t.lid=l.loc and ltype=0)+ (select count(1) from '+@DbName+'ticketd t where t.loc=l.loc) as opencall
,l.state,l.zip,r.lat,r.lng,l.rol,qblocid,(Select Name From rol where id =(select rol from Owner where id=l.Owner)) as Customer,
r.EN, B.Name As Company
from '+@DbName+'Loc l
left outer join '+@DbName+'Rol r on l.Rol=r.ID
left outer join ' + @DbName + 'tblUserCo UC on UC.CompanyID = r.EN  
left outer join '+@DbName+' Branch B on B.ID= r.EN and r.Type=4 '

if @SearchBy is not null
begin

if (@SearchBy= 'l.Address' or @SearchBy='l.ID' or @SearchBy='tag')
begin
set @Text += ' where  '+@SearchBy +' like ''%'+@SearchValue+'%''';
IF(@IsSalesAsigned > 0 and @SalesAsignedTerrID >0) set @Text+=' and    l.Terr='+convert(nvarchar(10),(@SalesAsignedTerrID));
end
ELSE IF ( @SearchBy = 'B.Name' AND @EN = 1 )
				BEGIN
				SET @Text+=' where UC.IsSel = 1 and r.EN =' +convert(nvarchar(50),@SearchValue) + ' and UC.UserID ='+convert(nvarchar(50),@UserID) 
				END
else
begin
set @Text += ' where  '+@SearchBy +' like '''+@SearchValue+'%''';
 IF(@IsSalesAsigned > 0 and @SalesAsignedTerrID >0) set @Text+=' and    l.Terr='+convert(nvarchar(10),(@SalesAsignedTerrID));
end

END
ELSE IF(@IsSalesAsigned > 0 and @SalesAsignedTerrID >0)
BEGIN
SET @Text+=' where   l.Terr='+convert(nvarchar(10),(@SalesAsignedTerrID))+ ' or isnull(l.Terr2,0)='+convert(nvarchar(10),(@SalesAsignedTerrID));
END
 ELSE IF(@EN = 1)
      BEGIN
          SET @Text+=' where UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)                    
      END
SET @Text +=' order by locid'

EXEC (@Text)
GO
