CREATE PROCEDURE [dbo].[spGetContactByRolID]
--@CustomerID int
@rolID int,
@DbName varchar(50)
as
set @DbName='['+ @DbName+'].[dbo].'
declare @StatusId int = 0
declare @Text varchar(max)

set @Text='
select ID as contactid,fDesc as name, Phone,Fax,Cell,Email,Title,isnull(EmailRecTicket,0 ) as EmailTicket ,isnull(EmailRecInvoice,0 ) as EmailRecInvoice,isnull(ShutdownAlert,0 ) as ShutdownAlert,isnull(EmailRecTestProp,0 ) as EmailRecTestProp  from Phone 
where 
Rol='+ CONVERT(varchar(50), @rolID)
--Rol=(select Rol from Owner where ID=@CustomerID)

exec (@Text)
GO

