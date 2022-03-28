CREATE PROCEDURE [dbo].[spUpdateUserCoverImage] 
	 @UserName NVARCHAR(50)
	,@Field SMALLINT
	,@UserID INT
	,@CoverImage NVARCHAR(MAX)
AS


BEGIN TRANSACTION

IF (@Field <> 2)
BEGIN  
	UPDATE tblUser
	SET			
		LastUpdateDate = Getdate()
		--,ProfileImage = Isnull(@ProfileImage,ProfileImage)
		,CoverImage = Isnull(@CoverImage,CoverImage)

	WHERE ID = @UserID
	
END
ELSE -- case of customer
BEGIN
	UPDATE Owner SET
		CoverImage = Isnull(@CoverImage,CoverImage)
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
 