CREATE FUNCTION [dbo].[CalculateInvoiceDue]
(
	@Ref INT,
	@RID INT
	
)
RETURNS FLOAT
AS
BEGIN
	DECLARE @VAL AS FLOAT

	DECLARE @CDATE AS DATETIME

	SET @CDATE=(SELECT PaymentReceivedDate FROM ReceivedPayment Where ID=@RID)

	SET @VAL=(select (select Amount from Trans where ref=@Ref and type=1)-sum(Trans.Amount)
	 from Trans where ID in(select TransID from PaymentDetails where InvoiceID=@Ref AND isnull( IsInvoice,0)=1) and fDate<=@CDATE)

	IF @VAL IS NULL
		BEGIN
			SET @VAL=(SELECT CASE i.Status WHEN 0 THEN i.Total   WHEN 1 THEN 0 WHEN 3 THEN isnull((select Balance from OpenAR where Ref= @Ref),0) END FROM Invoice i WHERE i.Ref=@Ref)

			
		END

	RETURN @VAL
END
