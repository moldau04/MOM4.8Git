CREATE PROCEDURE [dbo].[spUpdateProjectStatus]
	@ID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Status tinyint
	
BEGIN TRY
BEGIN TRANSACTION
	
	SELECT @Status=Status FROM JobT WHERE ID=@ID

	IF(@Status = 0)
	BEGIN
		SET @Status = 1
	END
	ELSE IF(@Status = 1)
	BEGIN
		SET @Status = 0
	END

	UPDATE JobT SET Status = @Status WHERE ID = @ID

	
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

