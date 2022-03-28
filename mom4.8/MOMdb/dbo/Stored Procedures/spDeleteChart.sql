CREATE PROCEDURE [dbo].[spDeleteChart] 
	@ID int
AS
BEGIN
	
	SET NOCOUNT ON;
	
	BEGIN TRY
	BEGIN TRANSACTION
	
		DELETE FROM Chart WHERE ID= @ID
		DELETE FROM Rol Where ID IN (Select Rol From Bank Where Chart = @ID and Type = 2)
		DELETE Bank Where Chart = @ID		
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
GO