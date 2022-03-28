CREATE PROC [dbo].[SpResetisSendmailtosalesper](@TicketID INT)
AS
    UPDATE lead
    SET    IsSendMailToSalesPer = 0
    WHERE  TicketID = @TicketID 
