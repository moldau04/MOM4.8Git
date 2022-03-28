CREATE PROCEDURE [dbo].[spGetCustomers]
@SearchBy varchar(20)= null ,
@SearchValue varchar(100) = null,
@DbName varchar(50)

as

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'

set @Text= 
'select distinct o.ID, LTRIM(RTRIM(r.Name)) as Name,fLogin,o.Status, Address,isnull(Balance,0) as Balance,o.type,city,phone,website,email,cellular,
(select count(1) from '+@DbName+'loc l where l.owner=o.id) as loc,
(select count(1) from '+@DbName+'elev e where e.owner=o.id) as equip,
(select count(1) from '+@DbName+'ticketo t where t.owner=o.id) + (select count(1) from '+@DbName+'ticketd t where (select owner from '+@DbName+'loc l where l.Loc = t.Loc)=o.ID) as opencall
, sageid,qbcustomerid
from '+@DbName+'[Owner] o 
left outer join '+@DbName+'Rol r on o.Rol=r.ID'
--from '+@dbname+'.dbo.[Owner] o 
--inner join '+@dbname+'.dbo.Rol r on o.Rol=r.ID'
 --o.Status='+CONVERT(varchar(20),@StatusID)

if @SearchBy is not null
begin

--set @Text += ' where '+@SearchBy +' like '''+@SearchValue+'%''' 
if (@SearchBy= 'Address' or @SearchBy='name')
begin
set @Text += ' where '+@SearchBy +' like ''%'+@SearchValue+'%'''
end
else
begin
set @Text += ' where '+@SearchBy +' like '''+@SearchValue+'%'''
end

end



set @Text +=' order by name'

exec (@Text)


