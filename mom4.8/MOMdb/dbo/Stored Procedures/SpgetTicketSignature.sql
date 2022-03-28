
create PROCEDURE [dbo].[SpgetTicketSignature] @workid   INT,
                                       @Ticketid INT
AS

    DECLARE @text VARCHAR(max)

    SET @text='
if exists(select 1 from sysobjects where name = ''PDA_'
              + Cast (@workid AS VARCHAR(50))
              + ''')
begin
if exists(select 1  from PDA_'
              + Cast (@workid AS VARCHAR(50))
              + '  where PDATicketID='
              + Cast (@Ticketid AS VARCHAR(50))
              + ')
begin
select top 1 signature from PDA_'
              + Cast (@workid AS VARCHAR(50))
              + '  where PDATicketID='
              + Cast (@Ticketid AS VARCHAR(50))
              + '
end
else
begin
select top 1 signature  from pdaticketsignature where pdaticketid='
              + Cast (@Ticketid AS VARCHAR(50))
              + '
end
end
else 
begin
select top 1 signature  from pdaticketsignature where pdaticketid='
              + Cast (@Ticketid AS VARCHAR(50)) + '
end
'

    EXEC (@text)