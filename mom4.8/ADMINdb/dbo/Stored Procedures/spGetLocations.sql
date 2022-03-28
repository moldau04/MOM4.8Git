CREATE PROCEDURE [dbo].[spGetLocations]
@SearchBy varchar(20)= null ,
@SearchValue varchar(100) = null,
@DbName varchar(50)
as

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'

set @Text= 
'select distinct loc , LTRIM(RTRIM(l.ID))  as locid, LTRIM(RTRIM(Name)) as Name,l.Type,l.Status,(select count(1) from '+@DbName+'elev e where e.loc=l.loc) as Elevs,isnull(Balance,0) as Balance, LTRIM(RTRIM(Tag)) as Tag,l.Address,l.City,
(select count(1) from '+@DbName+'ticketo t where t.lid=l.loc and ltype=0)+ (select count(1) from '+@DbName+'ticketd t where t.loc=l.loc) as opencall
,l.state,l.zip,r.lat,r.lng,l.rol,qblocid
from '+@DbName+'Loc l
left outer join '+@DbName+'Rol r on l.Rol=r.ID and r.Type=4 '

if @SearchBy is not null
begin

if (@SearchBy= 'l.Address' or @SearchBy='l.ID' or @SearchBy='tag')
begin
set @Text += ' where  '+@SearchBy +' like ''%'+@SearchValue+'%'''
end
else
begin
set @Text += ' where  '+@SearchBy +' like '''+@SearchValue+'%'''
end

end

set @Text +=' order by locid'

exec (@Text)
