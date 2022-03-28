
  
CREATE PROCEDURE [dbo].[spGetCompany]
@SearchBy varchar(20)= null ,
@SearchValue varchar(100) = null,
@DbName varchar(50)

as

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'

set @Text= 
'select distinct B.ID As CompanyID,B.Name As CompanyName, B.ID,B.Name,B.Manager,B.Address,B.OType,B.City,B.State,B.Zip,B.Phone,B.Fax,B.CostCenter,
B.InvRemarks,ISNULL(B.Status,0) as Status,
(select count(1) from '+@DbName+'[BRCompany] BR Where B.ID=BR.Company)  as NoOfOffices

from '+@DbName+'[Branch] B 
left join '+@DbName+'BRCompany BR on B.ID=BR.Company'


if (@SearchBy != '')
begin
set @Text += ' where '+@SearchBy +' like ''%'+@SearchValue+'%'''

end
else
begin
set @Text += ' order by B.ID'

end

exec (@Text)
GO

