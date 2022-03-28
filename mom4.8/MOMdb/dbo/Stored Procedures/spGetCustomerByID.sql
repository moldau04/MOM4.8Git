CREATE PROCEDURE [dbo].[spGetCustomerByID] 
@CustomerID int,
@DbName varchar(50),
@IsSalesAsigned int =0
AS

DECLARE @SalesAsignedTerrID int = 0
if( @IsSalesAsigned > 0) --If User is  Salesperson
BEGIN
SELECT @SalesAsignedTerrID=isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=@IsSalesAsigned) 
END

DECLARE @ParmDefinition nvarchar(500);
declare @a nvarchar(500)
declare @Text varchar(max)
declare @StatusId int = 0
declare @rolId int = 0
declare @db varchar(50)
SET @db=@DbName
SET @DbName='['+ @DbName+'].[dbo].'


set @Text='
select 
r.Name,
B.Name As Company,
r.City,
r.State,
r.Zip,
r.Address,
GeoLock,
Remarks,
o.Type,
r.Country,
fLogin,
Password,
o.Status,
TicketO,
TicketD,
Internet,
Rol,
r.EN,
r.Lat,
r.Lng,
Contact,
r.Phone,
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
 r.fax,
Title,
ProfileImage,
CoverImage,
o.Balance,
o.Custom1,
o.Custom2,
o.CNotes,
isnull(o.ShutdownAlert,0) as ShutdownAlert,
o.ShutdownMessage
from 
Owner o
left outer join Rol r on o.Rol=r.ID left Outer join Branch B on B.ID = r.EN
where o.ID='+convert(nvarchar(50),@CustomerID)


SET @ParmDefinition = N'@retvalOUT int OUTPUT';
set @a='select @retvalOUT=rol from Owner where ID ='+convert(nvarchar(50),@CustomerID)

exec(@Text)

exec sp_executesql @a,@ParmDefinition, @retvalOUT=@rolID OUTPUT;
exec spGetContactByRolID @rolId,@db
exec spGetLocationByCustID @CustomerID,@db,@IsSalesAsigned


---   [dbo].[spGetCustomerByID] 11,'TestM',85
GO