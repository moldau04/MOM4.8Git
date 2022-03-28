
CREATE PROCEDURE [dbo].[spUpdateUserCompany]
@ID	int,
@IsSel	bit	
As

BEGIN TRANSACTION
  
UPDATE tblUserCo
SET
IsSel			=	@IsSel
Where ID = @ID

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

 COMMIT TRANSACTION
 
 return (@ID)
