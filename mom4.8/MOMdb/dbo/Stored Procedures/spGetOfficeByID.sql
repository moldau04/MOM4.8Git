CREATE PROCEDURE [dbo].[spGetOfficeByID]
@CompanyID int,
@DbName varchar(50)

AS

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select
B.ID As CompanyID,B.Name As CompanyName,BR.ID,BR.Name,BR.Manager,BR.Address,BR.City,BR.State,BR.Zip,
BR.Phone,BR.Fax,BR.CostCenter,BR.InvRemarks,BR.Logo,BR.LogoPath,BR.BillRemit,BR.PORemit,BR.LocDTerr,
BR.LocDRoute,BR.LocDZone,BR.LocDStax,BR.LocType,BR.ARTerms,BR.ChargeInt,BR.ADP,BR.CB,BR.ARContact,
BR.OType,BR.DArea,BR.DState,BR.MileRate,BR.PriceD1,BR.PriceD2,BR.PriceD3,BR.PriceD4,BR.PriceD5,BR.UTaxR,
BR.UTax,BR.Company,BR.Status
from 
BRCompany BR left outer join Branch B on B.ID=BR.Company'

IF(@CompanyID > 0)
BEGIN
set @Text += ' where BR.ID='+convert(nvarchar(50),@CompanyID)
END
ELSE
BEGIN
set @Text += ' Order By BR.ID'
END

exec(@Text)
GO



