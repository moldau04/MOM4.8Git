CREATE PROCEDURE [dbo].[spUpdateUserAvatar] 
	 @UserName NVARCHAR(50)
	,@Field SMALLINT
	,@UserID INT
	,@ProfileImage NVARCHAR(MAX)
AS


BEGIN TRANSACTION

IF (@Field <> 2)
BEGIN  
	UPDATE tblUser
	SET			
		LastUpdateDate = Getdate()
		,ProfileImage = Isnull(@ProfileImage,ProfileImage)

	WHERE ID = @UserID
END
ELSE -- case of customer
BEGIN
	UPDATE Owner SET
		ProfileImage = Isnull(@ProfileImage,ProfileImage)
	WHERE  ID = @UserID
END

IF @@ERROR <> 0
	AND @@TRANCOUNT > 0
BEGIN
	RAISERROR (
			'Error Occured'
			,16
			,1
			)

	ROLLBACK TRANSACTION

	RETURN
END

COMMIT TRANSACTION
 