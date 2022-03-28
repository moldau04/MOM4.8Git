CREATE PROCEDURE [dbo].[spGetOfficeSearch]
@SearchBy varchar(20)= null ,
@SearchValue varchar(100) = null,
@DbName varchar(50)

as

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'

set @Text= 
'select
B.ID As CompanyID,B.Name As CompanyName,BR.ID,BR.Name,BR.Manager,BR.Address,BR.City,BR.State,BR.Zip,
BR.Phone,BR.Fax,BR.CostCenter,BR.InvRemarks,BR.Logo,BR.LogoPath,BR.BillRemit,BR.PORemit,BR.LocDTerr,
BR.LocDRoute,BR.LocDZone,BR.LocDStax,BR.LocType,BR.ARTerms,BR.ChargeInt,BR.ADP,BR.CB,BR.ARContact,
BR.OType,BR.DArea,BR.DState,BR.MileRate,BR.PriceD1,BR.PriceD2,BR.PriceD3,BR.PriceD4,BR.PriceD5,BR.UTaxR,
BR.UTax,BR.Company,BR.Status,
(select count(1) from '+@DbName+'[BRCompany] BR Where B.ID=BR.Company)  as NoOfOffices

from '+@DbName+'[Branch] B 
inner join '+@DbName+'BRCompany BR on B.ID=BR.Company'


if (@SearchBy != '')
begin
set @Text += ' where '+@SearchBy +' like ''%'+@SearchValue+'%'''

end
else
begin
set @Text += ' order by BR.ID'

end

exec (@Text)
GO
