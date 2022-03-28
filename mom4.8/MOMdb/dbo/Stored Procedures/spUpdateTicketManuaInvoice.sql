CREATE PROCEDURE [dbo].[spUpdateTicketManuaInvoice] 
            @dtTicketINV AS tblTypeTicketManualinvoice READONLY 
AS 
DECLARE @TicketId INT
DECLARE @Loc INT
DECLARE @ManualInvoice int
DECLARE cur_Ticket CURSOR FOR 	
	SELECT TicketID,ManualInvoice FROM @dtTicketINV	
OPEN cur_Ticket  
FETCH NEXT FROM cur_Ticket INTO @TicketId,@ManualInvoice
WHILE @@FETCH_STATUS = 0  
	BEGIN
		UPDATE TicketD
			SET ManualInvoice=@ManualInvoice
		WHERE ID=@TicketId
		--if ISNUMERIC(@ManualInvoice)=1
		--BEGIN
		--	Set @Loc=(SELECT Loc from TicketD WHERE ID=@TicketId )
		--	IF (SELECT COUNT(1) FROM Invoice WHERE Ref=@ManualInvoice and Loc=@Loc)>0
		--	BEGIN 			
		--		UPDATE TicketD
		--		SET Invoice=@ManualInvoice
		--		WHERE ID=@TicketId
		--	END
		--END

		
	FETCH NEXT FROM cur_Ticket INTO @TicketId,@ManualInvoice
	END	
CLOSE cur_Ticket  
DEALLOCATE cur_Ticket  
