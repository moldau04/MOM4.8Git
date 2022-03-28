Create PROCEDURE [dbo].[spGetLocContactByRolID]
--@CustomerID int
@rolID int,
@DbName varchar(50)
as
set @DbName='['+ @DbName+'].[dbo].'
declare @StatusId int = 0
declare @Text varchar(max)

set @Text='
select ID as contactid,fDesc as name, Phone,Fax,Cell,Email,isnull(EmailRecTicket,0 ) as EmailTicket,Title  from '+@DbName+'Phone 
where 
Rol='+ CONVERT(varchar(50), @rolID)
--Rol=(select Rol from Owner where ID=@CustomerID)

exec (@Text)
