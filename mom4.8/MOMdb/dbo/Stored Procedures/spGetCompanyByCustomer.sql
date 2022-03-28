
CREATE PROCEDURE [dbo].[spGetCompanyByCustomer]
@UserID int,
@DbName varchar(50)

AS

declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select
UC.ID, B.Name, UC.UserID, UC.CompanyID, UC.OfficeID, UC.IsSel
from 
Branch B inner Join tblUserCo UC on B.ID=UC.CompanyID'

IF(@UserID >= 0)
BEGIN
set @Text += ' where UC.UserID='+convert(nvarchar(50),@UserID) + 'AND UC.IsSel = 1'
END
ELSE
BEGIN
set @Text += ' Order By UC.ID'
END

exec(@Text)
