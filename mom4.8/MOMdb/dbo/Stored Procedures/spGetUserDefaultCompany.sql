CREATE PROCEDURE [dbo].[spGetUserDefaultCompany]
@UserID int,
@DbName varchar(50)

AS

declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select
U.ID, U.EN, B.Name,
(select count(1) from tblUserCo UC Where U.ID=UC.UserID And UC.IsSel=1)  as NoOfCompany
from 
tblUser U  inner Join Branch B on U.EN = B.ID'

IF(@UserID >= 0)
BEGIN
set @Text += ' where U.ID='+convert(nvarchar(50),@UserID)  
END
ELSE
BEGIN
set @Text += ' Order By U.ID'
END

exec(@Text)

