create PROCEDURE [dbo].[spGetCustomerByID]
@CustomerID int,
@DbName varchar(50)
as

DECLARE @ParmDefinition nvarchar(500);
declare @a nvarchar(500)
declare @Text varchar(max)
declare @StatusId int = 0
declare @rolId int = 0
declare @db varchar(50)
set @db=@DbName
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select 
Name,
City,
State,
Zip,
Address,
GeoLock,
Remarks,
o.Type,
Country,
fLogin,
Password,
Status,
TicketO,
TicketD,
Internet,
Rol,
Contact,
Phone,
Website,EMail,
Cellular,
ledger,
msmpass,
msmuser,
isnull(CPEquipment,0) as CPEquipment,
sageid,
isnull(ownerid,sageid) as ownerid,
Billing,
Central,
QBcustomerID,
 ISNULL(GroupbyWO,0) as GroupbyWO ,
 isnull(openticket,0) as openticket,
 isnull(BillRate,0) as BillRate,
 isnull(RateOT,0) as RateOT,
 isnull(RateNT,0) as RateNT,
 isnull(RateDT,0) as RateDT,
 isnull(RateTravel,0) as RateTravel,
 isnull(RateMileage,0) as RateMileage,
 r.fax
from 
'+@DbName+'Owner o
left outer join '+@DbName+'Rol r on o.Rol=r.ID
where o.ID='+convert(nvarchar(50),@CustomerID)


SET @ParmDefinition = N'@retvalOUT int OUTPUT';
set @a='select @retvalOUT=rol from '+@DbName+'Owner where ID ='+convert(nvarchar(50),@CustomerID)

exec(@Text)

exec sp_executesql @a,@ParmDefinition, @retvalOUT=@rolID OUTPUT;
exec spGetContactByRolID @rolId,@db
exec spGetLocationByCustID @CustomerID,@db

