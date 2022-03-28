CREATE PROCEDURE [dbo].[spDelWageCategory](  
@Id int  
)  
AS  
BEGIN  
DECLARE @Name VARCHAR  
 SET NOCOUNT ON;  
BEGIN TRY  
BEGIN TRANSACTION  
 IF NOT EXISTS(SELECT Wage FROM PRWageItem WHERE Wage = @Id)  
 BEGIN  
  SELECT @Name = fDesc FROM PRWage WHERE ID = @Id  
  DELETE FROM PRWage WHERE ID = @Id  
    
 END  
 ELSE   
  RAISERROR( 'Wage category is in use, you can not delete.',16,1)  
    
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