
CREATE PROCEDURE [dbo].[spAddCheckRecurr]  
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
 @PJID INT
AS  
BEGIN  
 SET NOCOUNT ON;  
   
 DECLARE @CDID INT = (SELECT ISNULL(MAX(ID),0)+1 FROM CDRecurr)  
  
BEGIN TRY  
BEGIN TRANSACTION  
   
 
 INSERT INTO CDRecurr(ID,fDate,Ref,fDesc,Amount,Bank,Type,Status,Vendor,Memo, IsRecon, ACH,Frequency,PJID)  
 VALUES (@CDID,@fDate,@NextC,@fDesc,@TotalPay,@Bank,@Type,0,@Vendor,@Memo, 0, 0,@Frequency,@PJID) -- CD.Status = 0 (Paid), CD.Status = 2 (Voided), CD.fDesc = Payee's name  

  
   
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