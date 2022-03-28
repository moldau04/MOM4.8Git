CREATE PROCEDURE [dbo].[spGetLocationByCustID]
@CustomerID int,
@DbName varchar(50)

as
set @DbName='['+ @DbName+'].[dbo].'
declare @StatusId int = 0
declare @Text varchar(max)

set @Text='
select distinct l.ID as locid,Name,l.Type,l.Status,(select count(1) from '+@DbName+'elev e where e.loc=l.loc) as Elevs,Balance,Tag,l.Address,l.City,l.loc
,isnull(RoleID,0) as roleid
from '+@DbName+'Loc l
left outer join '+@DbName+'Rol r on l.Rol=r.ID 
where Owner='+ CONVERT(varchar(50), @CustomerID)+' and r.Type=4'

exec (@Text)
