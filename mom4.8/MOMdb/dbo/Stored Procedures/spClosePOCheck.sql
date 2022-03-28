CREATE PROCEDURE [dbo].[spClosePOCheck]
	@PO INT,
	@ReceiptNo INT
AS
BEGIN
	DECLARE @TotalAmount VARCHAR(50)
	DECLARE @TotalReceived VARCHAR(50)
	DECLARE @ThisReceiptAmount VARCHAR(50)
	DECLARE @Diff VARCHAR(50)
	DECLARE @RESULT VARCHAR(50)


	SET @TotalAmount=(SELECT ISNULL(SUM(Amount),0) FROM po WHERE po=@PO)
	PRINT(@TotalAmount)

	SET @TotalReceived=(SELECT ISNULL(SUM(Amount),0) FROM ReceivePO WHERE po=@PO and Status=1)
	PRINT(@TotalReceived)

	SET @ThisReceiptAmount=(SELECT ISNULL(SUM(Amount),0) FROM ReceivePO WHERE ID=@ReceiptNo)
	PRINT(@ThisReceiptAmount)

	SET @Diff=CAST(CAST(@TotalAmount AS FLOAT)- CAST(@TotalReceived AS FLOAT) AS VARCHAR(50))
	PRINT(@Diff)

	IF CAST(@Diff AS FLOAT)= CAST(@ThisReceiptAmount AS FLOAT)
		BEGIN
		    SET @RESULT='1'
		END
	ELSE
		BEGIN
			SET @RESULT='0'
		END

	SELECT @RESULT AS Result
	
END
GO