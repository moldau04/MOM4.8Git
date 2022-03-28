CREATE  PROCEDURE [dbo].[spGetLocationByID]
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
LTRIM(RTRIM(l.Address))  as locAddress,
l.City as locCity,
l.State as locState,
l.Zip as locZip,
l.Rol,
l.Type ,
isnull(l.Route,0) Route,
Terr,
Terr2,
r.City,
r.State,
r.Zip,
r.Country,
LTRIM(RTRIM(r.Address)) as Address,
l.Remarks,
r.Contact,
r.Contact as Name,
r.Phone,
r.Website,
r.EMail,
r.Cellular,
r.Fax,
(Select r.EN  from Rol r inner join Owner o on r.ID = o.Rol inner Join Loc l on l.Owner = o.ID Where l.Loc = '+convert(nvarchar(50),@LocID)+') AS EN,
(Select Name from  Branch  where ID = (Select r.EN  from Rol r inner join Owner o on r.ID = o.Rol inner Join Loc l on l.Owner = o.ID Where l.Loc = '+convert(nvarchar(50),@LocID)+')) AS Company,
l.owner,
(select top 1 name from '+@DbName+'rol where id=(select top 1 rol from '+@DbName+'owner where id= l.owner)) as custname,
l.stax,
l.STax2,
l.UTax,
l.Zone,
l.Country As locCountry,
r.Lat,r.Lng,l.custom1,l.custom2,l.custom14,l.custom15,l.custom12,l.custom13,l.status,
(select name from '+@DbName+'route rt where rt.id=l.route) as defwork,
isnull(l.credit,0) as credit, isnull(l.CreditFlag,0) as CreditFlag, isnull(l.dispalert,0) as dispalert, l.creditreason,
(select top 1 sageid from '+@DbName+'owner where id = l.owner) as custsageid,
l.Billing,
qblocid,
defaultterms,
isnull(l.BillRate,0) as BillRate,
 isnull(l.RateOT,0) as RateOT,
 isnull(l.RateNT,0) as RateNT,
 isnull(l.RateDT,0) as RateDT,
 isnull(l.RateTravel,0) as RateTravel,
 isnull(l.RateMileage,0) as RateMileage,
 isnull(stax.Rate,0) AS Rate,
 case when (select Label from '+@DbName+'custom where name =''Country'') = 1 
	then 
		Convert(numeric(30,2),(select Label As GstRate from '+@DbName+'custom where Name=''GSTRate''))
	else 0.00 
 end As GstRate,
 l.PrintInvoice,
 l.EmailInvoice,
 l.Balance,
 l.Loc,
 isnull(l.NoCustomerStatement,0) as NoCustomerStatement,
 l.Address + '', ''+ l.City + '', '' + l.State + '' '' + l.Zip   As LocationName,
 tr.Name AS Salesperson,
 rt.Name AS RouteName,
 ISNULL(cst.Name, ''None'') AS ConsultantName,
 o.Custom1 AS OwnerName,
 rl.name as Customer,
 (select count(1) from  elev e with (nolock) where e.loc=l.loc) as Elevs,
 l.BusinessType as BusinessTypeID,
 stax.Type as sTaxType

from '+@DbName+'Loc l
left outer join ' + @DbName + 'Owner o ON o.id = l.owner
left outer join ' + @DbName + 'Rol rl ON o.rol = rl.id
left outer join ' + @DbName + 'Rol r on l.Rol=r.ID and r.Type=4
left outer join ' + @DbName + 'stax on stax.name = l.stax
left outer join ' + @DbName + 'Branch B on B.ID = r.EN
left outer join ' + @DbName + 'Terr tr with (nolock)  ON l.Terr = tr.ID 
left outer join ' + @DbName + 'Route rt with (nolock) ON l.Route = rt.ID 
left outer join ' + @DbName + 'tblConsult cst with (nolock) ON cst.ID = l.Consult
where l.Loc='+convert(nvarchar(50),@LocID)
--Table[0]
exec(@Text)

SET @ParmDefinition = N'@retvalOUT int OUTPUT';
set @a='select @retvalOUT=rol from '+@DbName+'Loc where Loc ='+convert(nvarchar(50),@LocID)
exec sp_executesql @a,@ParmDefinition, @retvalOUT=@rolID OUTPUT;
--select @rolId=Rol from Loc where Loc =@LocID
--Table[1]
exec spGetlocContactByRolID @rolId,@db
--SELECT * FROM PHONE WHERE PHONE.ROL=@rolId order by ID desc

--Table[2]
select Top 1 Balance from Owner Where ID IN(Select Top 1 Owner from Loc Where Loc=@LocID)
--Table[3]
select * from (
select * from Log2 where ref= @LocID and (Screen='Location' )
union all
select * from Log2 where ref in (select Owner from Loc where Loc=@LocID) and  Screen ='iCollections Popup'
)as Logs  order by CreatedStamp desc
--Table[4]
--Get all Open Tickets of this Location
Select ID as TicketID, Assigned as Status from TicketO Where LID = @LocID and Assigned != 4