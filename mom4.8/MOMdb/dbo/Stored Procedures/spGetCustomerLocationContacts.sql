CREATE PROCEDURE [dbo].[spGetCustomerLocationContacts] 
	@CustID INT,
	@LocID INT
AS
BEGIN
	
select ID as contactid,fDesc as name, Phone,Fax,Cell,Email,Title,isnull(EmailRecTicket,0 ) as EmailTicket ,isnull(EmailRecInvoice,0 ) as EmailRecInvoice,isnull(ShutdownAlert,0 ) as ShutdownAlert,isnull(EmailRecTestProp,0 ) as EmailRecTestProp,'Customer' as ctype  from Phone 
where Rol IN(select Rol from Owner Where ID=@CustID)

union all

select ID as contactid,fDesc as name, Phone,Fax,Cell,Email,Title,isnull(EmailRecTicket,0 ) as EmailTicket ,isnull(EmailRecInvoice,0 ) as EmailRecInvoice,isnull(ShutdownAlert,0 ) as ShutdownAlert,isnull(EmailRecTestProp,0 ) as EmailRecTestProp, 'Location' as ctype  from Phone 
where Rol IN(select Rol from Loc Where Loc=@LocID)

END
