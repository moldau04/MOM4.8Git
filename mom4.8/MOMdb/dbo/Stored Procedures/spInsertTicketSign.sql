create PROCEDURE [dbo].[spInsertTicketSign]
@ticketid int,
@sign image
as
if not exists (select 1 from PDATicketSignature where PDATicketID=@ticketid)
begin
insert into PDATicketSignature 
(
PDATicketID,
SignatureType,
Signature,
AID
)
values
(
@ticketid,
'C',
@sign,
NEWID()
)
end
else
begin 
update PDATicketSignature set
Signature=@sign
where PDATicketID=@ticketid
end
