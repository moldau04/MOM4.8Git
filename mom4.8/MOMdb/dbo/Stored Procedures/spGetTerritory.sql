CREATE PROCEDURE [dbo].[spGetTerritory]
@ID int,
@DbName varchar(50)

AS

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select 
ID,
Name + SDesc As Name,
SMan,
SDesc,
Remarks,
Count,
Symbol,
EN,
Address

from 
Terr'

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
