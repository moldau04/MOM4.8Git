CREATE PROCEDURE [dbo].[spGetUseTax]
@ID int,
@DbName varchar(50)

AS

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select 
Name + convert(nvarchar(50),Rate) As Name,
fDesc,
Rate,
State,
Remarks,
Count,
GL,
Type,
UType,
PSTReg,
IsTaxable,
QBStaxID,
LastUpdateDate

from 
STax Where Type = 1'

IF(@ID > 0)
BEGIN
set @Text += ' where GL='+convert(nvarchar(50),@ID)
END
ELSE
BEGIN
set @Text += ' Order By Name'
END

exec(@Text)
GO