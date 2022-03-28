CREATE PROCEDURE [dbo].[spUpdateUserCompanyReset]  
@ID int,  
@IsSel bit,  
@UserID int 
As  
  
BEGIN TRANSACTION  
    
UPDATE tblUserCo  
SET  
IsSel   = @IsSel  
Where UserID  = @UserID  

UPDATE tblUserCo  
SET  
IsSel   = 1  
Where UserID  = @UserID AND CompanyID = @ID
  
IF @@ERROR <> 0 AND @@TRANCOUNT > 0  
 BEGIN    
 RAISERROR ('Error Occured', 16, 1)    
    ROLLBACK TRANSACTION      
    RETURN  
 END  
  
 COMMIT TRANSACTION  
   
 return (@ID)  
GO