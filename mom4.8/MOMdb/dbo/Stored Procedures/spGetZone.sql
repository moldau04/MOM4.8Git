
CREATE PROCEDURE [dbo].[spGetZone]
@ID int,
@DbName varchar(50)

AS

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select 
ID,
Name,
Surcharge,
Bonus,
Count,
Remarks,
Price1,
Price2,
Price3,
Price4,
Price5,
IDistance,
ODistance,
Color,
fDesc,
Tax

from 
Zone'

IF(@ID > 0)
BEGIN
set @Text += ' where ID='+convert(nvarchar(50),@ID)
END
ELSE
BEGIN
set @Text += ' Order By Name'
END

exec(@Text)
GO
