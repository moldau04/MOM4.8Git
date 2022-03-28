CREATE PROCEDURE [dbo].[spGetLocInfoForEstimateByID]
	@LocID int,
	@DbName varchar(50)

as

declare @a nvarchar(500)
DECLARE @ParmDefinition nvarchar(500);
declare @Text varchar(max)
declare @StatusId int = 0
declare @rolID int=0
declare @db varchar(50)
set @db=@DbName
set @DbName='['+ @DbName+'].[dbo].'

set @Text='
select
	Rol,
	Terr,
	r.Contact,
	r.Contact as Name,
	r.Phone,
	r.EMail,
	r.Cellular,
	r.Fax,
	(Select r.EN  from Rol r inner join Owner o on r.ID = o.Rol inner Join Loc l on l.Owner = o.ID Where l.Loc = '+convert(nvarchar(50),@LocID)+') AS EN,
	(Select Name from  Branch  where ID = (Select r.EN  from Rol r inner join Owner o on r.ID = o.Rol inner Join Loc l on l.Owner = o.ID Where l.Loc = '+convert(nvarchar(50),@LocID)+')) AS Company,
	l.owner,
	(select top 1 name from '+@DbName+'rol where id=(select top 1 rol from '+@DbName+'owner where id= l.owner)) as custname
from '+@DbName+'Loc l
left outer join '+@DbName+'Rol r on l.Rol=r.ID and r.Type=4
left outer join ' + @DbName + 'Branch B on B.ID = r.EN
where l.Loc='+convert(nvarchar(50),@LocID)

exec(@Text)

--SET @ParmDefinition = N'@retvalOUT int OUTPUT';
--set @a='select @retvalOUT=rol from '+@DbName+'Loc where Loc ='+convert(nvarchar(50),@LocID)
--exec sp_executesql @a,@ParmDefinition, @retvalOUT=@rolID OUTPUT;
select @rolId=Rol from Loc where Loc =@LocID

--exec spGetlocContactByRolID @rolId,@db
SELECT ID, fDesc FROM PHONE WHERE PHONE.ROL=@rolId
--UNION
--SELECT 0, contact FROM Rol WHERE ID = @rolId AND Contact is not null AND Contact != ''