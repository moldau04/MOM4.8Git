CREATE PROCEDURE [dbo].[spInsertSignDPDA]
@Userid int,
@ticketid int,
@sign image
as
declare @Text varchar(max)

SELECT @ticketid as ID , @sign  as signat INTO #ImageTemp

set @Text='
if not exists(select table_name from information_schema.TABLES where table_name = ''PDA_'+CAST( @Userid as varchar(20) )+''')
begin
 
create table PDA_'+CAST( @Userid as varchar(20) )+' 
(
	[PDATicketID] [int] NULL,
	[SignatureType] [char](1) NULL,
	[Signature] [image] NULL,
	[AID] [uniqueidentifier] NOT NULL
)

end

if not exists(select 1 from PDA_'+CAST( @Userid as varchar(20) )+' where PDATicketID = (select top 1 ID from #ImageTemp))
begin
insert into PDA_'+CAST( @Userid as varchar(20) )+' 
(PDATicketID,
SignatureType,
Signature,
AID
)	
select
ID,
''C'',
signat,
newid()
from #ImageTemp
end

else
begin
update p set 
p.Signature = t.signat
from PDA_'+CAST( @Userid as varchar(20) )+'  p
inner join #ImageTemp t on t.ID = p.PDATicketID

end
  '

exec (@Text)


drop table #ImageTemp