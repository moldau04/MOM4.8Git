CREATE PROCEDURE [dbo].[spGetLocationByID]
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
l.Consult,
l.ID ,
Tag,
l.Address as locAddress,
l.City as locCity,
l.State as locState,
l.Zip as locZip,
Rol,
l.Type ,
Route,
Terr,
Terr2,
r.City,
r.State,
r.Zip,
r.Address,
l.Remarks,
r.Contact,
r.Contact as Name,
r.Phone,
r.Website,
r.EMail,
r.Cellular,
r.Country,
r.Fax,
r.EN,
B.Name As Company,
l.owner,
(select top 1 name from '+@DbName+'rol where id=(select top 1 rol from '+@DbName+'owner where id= l.owner)) as custname,
l.stax,
l.STax2,
l.UTax,
l.Zone,
l.Country,
r.Lat,r.Lng,l.custom1,l.custom2,l.custom14,l.custom15,l.custom12,l.custom13,l.status,
(select name from '+@DbName+'route rt where rt.id=l.route) as defwork,
isnull(l.credit,0)as credit,isnull(l.dispalert,0)as dispalert,l.creditreason,
(select top 1 sageid from '+@DbName+'owner where id = l.owner) as custsageid,
l.Billing,
qblocid,
defaultterms,
isnull(BillRate,0) as BillRate,
 isnull(RateOT,0) as RateOT,
 isnull(RateNT,0) as RateNT,
 isnull(RateDT,0) as RateDT,
 isnull(RateTravel,0) as RateTravel,
 isnull(RateMileage,0) as RateMileage,
 isnull(stax.Rate,0) AS Rate,
 case when (select Label from '+@DbName+'custom where name =''Country'') = 1 
	then 
		Convert(numeric(30,2),(select Label As GstRate from '+@DbName+'custom where Name=''GSTRate''))
	else 0.00 
 end As GstRate,
 l.PrintInvoice,
 l.EmailInvoice,
 l.Balance
from '+@DbName+'Loc l
left outer join '+@DbName+'Rol r on l.Rol=r.ID and r.Type=4
left outer join '+@DbName+'stax on stax.name = l.stax
left outer join ' + @DbName + 'Branch B on B.ID = r.EN
where l.Loc='+convert(nvarchar(50),@LocID)

exec(@Text)

SET @ParmDefinition = N'@retvalOUT int OUTPUT';
set @a='select @retvalOUT=rol from '+@DbName+'Loc where Loc ='+convert(nvarchar(50),@LocID)
exec sp_executesql @a,@ParmDefinition, @retvalOUT=@rolID OUTPUT;
--select @rolId=Rol from Loc where Loc =@LocID

exec spGetlocContactByRolID @rolId,@db


select Top 1 Balance from Owner Where ID IN(Select Top 1 Owner from Loc Where Loc=@LocID)

select * from Log2 where ref= @LocID and Screen='Location' order by CreatedStamp desc
