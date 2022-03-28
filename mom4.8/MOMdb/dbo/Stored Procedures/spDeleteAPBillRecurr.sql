CREATE PROCEDURE [dbo].[spDeleteAPBillRecurr]  
 @id int  
AS  
BEGIN  
   
 SET NOCOUNT ON;  
  
BEGIN TRY  
BEGIN TRANSACTION  
	DELETE FROM PJRecurr WHERE ID = @id
	DELETE FROM PJRecurrI WHERE PJID = @id
  
  -- End Update status of PO and  Receive PO ---  
  COMMIT   
 END TRY  
 BEGIN CATCH  
  
 SELECT ERROR_MESSAGE()  
  
    IF @@TRANCOUNT>0  
        ROLLBACK   
  RAISERROR ('An error has occurred on this page.',16,1)  
        RETURN  
  
 END CATCH  
   
END  
GO