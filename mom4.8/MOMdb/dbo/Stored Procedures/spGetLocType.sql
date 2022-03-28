CREATE PROCEDURE [dbo].[spGetLocType]
@ID int,
@DbName varchar(50)

AS

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select 
Type,
Count,
Remarks,
QBlocTypeID,
LastUpdateDate

from 
LocType '

IF(@ID > 0)
BEGIN
set @Text += ' where Type='+convert(nvarchar(50),@ID)
END
ELSE
BEGIN
set @Text += ' Order By Type'
END

exec(@Text)
GO