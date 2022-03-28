CREATE PROCEDURE [dbo].[spDeleteRecurrCheck]  
(  
 @CDID INT  
)  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
   BEGIN TRANSACTION  
  
   IF EXISTS(SELECT 1   
              FROM   [dbo].[CDRecurr]   
              WHERE  ID = @CDID)   
   BEGIN   
  DECLARE @Batch INT;  
  DECLARE @Vendor INT  
  DECLARE @Bank INT  
  DECLARE @Amount numeric(30,2)  
  
  
  
  
  DELETE FROM CDRecurr WHERE ID = @CDID;  
    
  
  
   END   
   COMMIT TRANSACTION  
END  