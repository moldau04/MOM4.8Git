CREATE PROCEDURE [dbo].[spInsertSign]
@Userid int,
@ticketid int
as
declare @Text varchar(max)

--set @Text='
--if exists(select table_name from information_schema.TABLES where table_name = ''PDA_'+CAST( @Userid as varchar(20) )+''')
--begin
--	if not exists(select 1 from PDATicketSignature where PDATicketID='+CAST( @ticketid as varchar(20) )+' ) 
--		begin
--			insert into PDATicketSignature select * from PDA_'+CAST( @Userid as varchar(20) )+' where PDATicketID='+CAST( @ticketid as varchar(20) )+' 
--		end
--		else
--		begin 
--			 update PDATicketSignature set
--			 PDATicketSignature.signature=pd.signature
--			 from PDATicketSignature inner join PDA_'+CAST( @Userid as varchar(20) )+' pd on PDATicketSignature.PDATicketID=pd.PDATicketID
--		end
		
--		 delete from PDA_'+CAST( @Userid as varchar(20) )+' where PDATicketID='+CAST( @ticketid as varchar(20) )+'
--end  
--  '
	 
	 set @Text='
if exists(select table_name from information_schema.TABLES where table_name = ''PDA_'+CAST( @Userid as varchar(20) )+''')
begin		
	delete from PDA_'+CAST( @Userid as varchar(20) )+' where PDATicketID='+CAST( @ticketid as varchar(20) )+'
end  
  '

exec (@Text)
