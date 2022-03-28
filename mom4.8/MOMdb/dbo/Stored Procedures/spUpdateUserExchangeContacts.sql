CREATE PROCEDURE [dbo].[spUpdateUserExchangeContacts]
	@UserID int,
	@ContactsList tblTypeExchangeContact readonly
AS

BEGIN TRY
BEGIN TRANSACTION
	SELECT * FROM @ContactsList
	-- Delete the old session
	DELETE tblUserExchangeContacts WHERE UserID = @UserID
	INSERT INTO tblUserExchangeContacts SELECT @UserID, * FROM @ContactsList
	
	COMMIT 
END TRY
BEGIN CATCH

	SELECT ERROR_MESSAGE()

	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

END CATCH
