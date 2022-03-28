CREATE PROCEDURE [dbo].[spUpdateCheckRecurr]  
 @fDate Datetime,  
 @fDesc VARCHAR(250),  
 @Bank INT,  
 @Vendor INT,  
 @Memo VARCHAR(75),  
 @NextC BIGINT,  
 @DiscGL INT,  
 @Type INT,  
 @fUser VARCHAR(50), 
 @TotalPay NUMERIC(30,2) ,
 @Frequency INT,  
 @PJID INT,
 @CDID INT
AS  
BEGIN  
 SET NOCOUNT ON;  
    DECLARE @CurrentBank int
    DECLARE @CurrentVendor int
    DECLARE @CurrentfDesc varchar(350)
    DECLARE @CurrentfDate varchar(50)
    DECLARE @CurrentMemo varchar(100)
    DECLARE @CurrentNextC BIGint
    DECLARE @CurrentDiscGL int
    DECLARE @CurrentType int
    DECLARE @CurrentfUser varchar(50)
    DECLARE @Currentfrequency int
    DECLARE @CurrentPJID int
    DECLARE @CurrentAmount numeric(30,2)
	SELECT
        @CurrentAmount = Amount,
        @CurrentNextC = Ref,
        @CurrentfDesc = fDesc,
        @CurrentfDate = CONVERT(varchar(50), fDate, 101),
        @CurrentBank = Bank,
        @CurrentType = Type,
        @CurrentVendor = Vendor,
        @CurrentMemo = Memo,
        @Currentfrequency = Frequency,
        @CurrentPJID = PJID
    FROM CDRecurr
    WHERE ID = @CDID
 --DECLARE @CDID INT = (SELECT ISNULL(MAX(ID),0)+1 FROM CDRecurr)  
  
BEGIN TRY  
BEGIN TRANSACTION  
 
    --DELETE FROM CDRecurr WHERE ID = @CDID
 
    --INSERT INTO CDRecurr(ID,fDate,Ref,fDesc,Amount,Bank,Type,Status,Vendor,Memo, IsRecon, ACH,Frequency,PJID)  
    --VALUES (@CDID,@fDate,@NextC,@fDesc,@TotalPay,@Bank,@Type,0,@Vendor,@Memo, 0, 0,@Frequency,@PJID) -- CD.Status = 0 (Paid), CD.Status = 2 (Voided), CD.fDesc = Payee's name  
    UPDATE CDRecurr SET Ref = @NextC,fDesc=@fDesc,Amount= @TotalPay,Bank= @Bank,Type= @Type,Vendor= @Vendor,Memo= @Memo,Frequency= @Frequency,PJID= @PJID
    WHERE ID = @CDID

	IF (@CurrentAmount <> @TotalPay)
    BEGIN
		EXEC log2_insert @fUser,'RecCheck',@CDID,'Amount', @CurrentAmount,@TotalPay
	END  
	IF (@Currentfrequency <> @Frequency)
    BEGIN
		EXEC log2_insert @fUser,'RecCheck',@CDID,'Frequency', @Currentfrequency,@Frequency
	END 
	IF (@CurrentNextC <> @NextC)
    BEGIN
		EXEC log2_insert @fUser,'RecCheck',@CDID,'CheckNo', @CurrentNextC,@NextC
	END
	IF (@CurrentfDesc <> @fDesc)
    BEGIN
		EXEC log2_insert @fUser,'RecCheck',@CDID,'Description', @CurrentfDesc,@fDesc
	END
	IF (@CurrentBank <> @Bank)
    BEGIN
		EXEC log2_insert @fUser,'RecCheck',@CDID,'Bank', @CurrentBank,@Bank
	END
	IF (@CurrentVendor <> @Vendor)
    BEGIN
		EXEC log2_insert @fUser,'RecCheck',@CDID,'Vendor', @CurrentVendor,@Vendor
	END
	IF (@CurrentMemo <> @Memo)
    BEGIN
		EXEC log2_insert @fUser,'RecCheck',@CDID,'Memo', @CurrentMemo,@Memo
	END
	DECLARE @NewfDate varchar(50) = CONVERT(varchar(50), @fDate, 101)
	IF (@CurrentfDate <> @NewfDate)
    BEGIN
		EXEC log2_insert @fUser,'RecCheck',@CDID,'Date', @CurrentfDate,@NewfDate
	END

    

COMMIT   
 END TRY  
 BEGIN CATCH  
  
 SELECT ERROR_MESSAGE()  
  
    IF @@TRANCOUNT>0  
        ROLLBACK   
  RAISERROR ('An error has occurred on this page.',16,1)  
        RETURN  
  
 END CATCH  
 RETURN @CDID  
END  