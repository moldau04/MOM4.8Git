
create procedure [dbo].[spGetSignature] 
@workid int,
@Ticketid int

as

--declare @workid int = 4
--declare @Ticketid int = 6244
declare @text varchar(max)

set @text='
if exists(select 1 from sysobjects where name = ''PDA_'+CAST (@workid as varchar(50))+''')
begin
if exists(select 1  from PDA_'+CAST (@workid as varchar(50))+'  where PDATicketID='+CAST (@Ticketid as varchar(50))+')
begin
select count(1) as signatureCount  from PDA_'+CAST (@workid as varchar(50))+'  where PDATicketID='+CAST (@Ticketid as varchar(50))+'
end
else
begin
select count(1) as signatureCount  from pdaticketsignature where pdaticketid='+CAST (@Ticketid as varchar(50))+'
end
end
else 
begin
select count(1) as signatureCount  from pdaticketsignature where pdaticketid='+CAST (@Ticketid as varchar(50))+'
end
'

exec (@text)