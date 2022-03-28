
CREATE PROCEDURE [dbo].[spUpdateUserCompanyID]
@UserID	int,
@EN	int	
As

BEGIN TRANSACTION
  
UPDATE tblUser
SET
EN			=	@EN
Where ID = @UserID

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

 COMMIT TRANSACTION
 
 return (@UserID)
GO
