﻿CREATE PROCEDURE [dbo].[spCalWageCateDeduction]  
AS  
BEGIN  
 SET NOCOUNT ON;  
BEGIN TRY  
BEGIN TRANSACTION  
  UPDATE PRWage SET Count = ISNULL (t.EMP , 0) FROM PRWage c LEFT JOIN (SELECT Wage,Count(EMP) AS EMP FROM PRWageItem GROUP BY Wage) t ON c.ID = t.Wage   
  UPDATE PRDed SET Count = ISNULL (t.EMP , 0) FROM PRDed c LEFT JOIN (SELECT Ded,Count(EMP) AS EMP FROM PRDedItem GROUP BY Ded) t ON c.ID = t.Ded   
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