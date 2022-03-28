CREATE PROCEDURE [dbo].[spGetPOByTicketId]
	@TicketId int
AS
BEGIN
	SET NOCOUNT ON; 
	SELECT  p.PO, p.fDate, p.fDesc, p.Amount, v.Acct as VendorName, v.Acct# ,
	   (CASE p.Status
            WHEN 0 THEN 'Open'
            WHEN 1 THEN 'Closed'
            WHEN 2 THEN 'Void'
            WHEN 3 THEN 'Partial-Quantity'
            WHEN 4 THEN 'Partial-Amount'
            WHEN 5 THEN 'Closed At Received PO'
        END) AS Status , p.fDesc  as Comment
	FROM PO AS p, Vendor AS v, POItem AS Poitm 
	WHERE p.Vendor = v.ID AND p.po = Poitm.po  and Poitm.Ticket=@TicketId
END
