CREATE PROCEDURE [dbo].[spGetRoute]
@ID int,
@DbName varchar(50)

AS

declare @StatusId int = 0
declare @Text varchar(max)
set @DbName='['+ @DbName+'].[dbo].'


set @Text='
select 
R.ID,
R.Name,
R.Mech,
R.Loc,
R.Elev,
R.Hour,
R.Amount,
R.Remarks,
R.Symbol,
R.EN


from 
Route R left outer join Branch B on R.EN = B.ID'

IF(@ID > 0)
BEGIN
set @Text += ' where R.ID='+convert(nvarchar(50),@ID)
END
ELSE
BEGIN
set @Text += ' Order By R.Name'
END

exec(@Text)
GO