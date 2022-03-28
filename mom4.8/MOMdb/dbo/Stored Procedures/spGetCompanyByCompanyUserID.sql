CReate PROCEDURE [dbo].[spGetCompanyByCompanyUserID]
@UserID int,
@CompanyID int,
@DbName varchar(50)

AS

declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select
B.ID, B.Name, UC.UserID, UC.CompanyID, UC.OfficeID, UC.IsSel
from 
Branch B inner Join tblUserCo UC on B.ID=UC.CompanyID'

IF(@UserID >= 0 AND @CompanyID > 0)
BEGIN
set @Text += ' where UC.UserID='+convert(nvarchar(50),@UserID) + ' AND UC.CompanyID =' +convert(nvarchar(50),@CompanyID)  
END
ELSE
BEGIN
set @Text += ' Order By B.ID'
END

exec(@Text)
GO