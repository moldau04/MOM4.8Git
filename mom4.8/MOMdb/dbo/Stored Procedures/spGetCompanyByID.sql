
CREATE PROCEDURE [dbo].[spGetCompanyByID]
@CompanyID int,
@DbName varchar(50)

AS

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select 
B.ID As CompanyID,B.Name As CompanyName,
B.ID,
B.Name,
B.Manager,
B.Address,
B.City,
B.State,
B.Zip,
B.Phone,
B.Fax,
B.CostCenter,
B.InvRemarks,
B.Logo,
B.LogoPath,
B.BillRemit,
B.PORemit,
B.LocDTerr,
B.LocDRoute,
B.LocDZone,
B.LocDStax,
B.LocType,
B.ARTerms,
B.ADP,
B.CB,
B.ARContact,
B.OType,
B.DArea,
B.DState,
B.MileRate,
B.PriceD1,
B.PriceD2,
B.PriceD3,
B.PriceD4,
B.PriceD5,
B.UTaxR,
B.UTax,
B.Status,
(select count(1) from [BRCompany] BR Where B.ID=BR.Company)  as NoOfOffices,
B.DInvAcct,
B.Longitude,
B.Latitude,
B.Country
from 
Branch B'

IF(@CompanyID > 0)
BEGIN
set @Text += ' where B.ID='+convert(nvarchar(50),@CompanyID)
END
ELSE
BEGIN
set @Text += ' Order By B.ID'
END


exec(@Text)

GO
