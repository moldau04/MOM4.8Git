CREATE PROCEDURE [dbo].[spDeleteDocument]
	@DocId int = 0,
	@UpdatedBy varchar(100)
AS
BEGIN TRY
	BEGIN TRANSACTION
		DECLARE @ScreenId int
		DECLARE @Screen varchar(50)
		DECLARE @DocName varchar(1000)

		SELECT @Screen = Screen, @ScreenId = ScreenID, @DocName = [Filename] FROM Documents WHERE ID = @DocId
		DELETE FROM documents WHERE id=@DocId
		IF @Screen = 'PO'
		BEGIN
			EXEC log2_insert @UpdatedBy,@Screen,@ScreenId,'Delete document', @DocName,''
		END
	COMMIT
END TRY
BEGIN CATCH

	SELECT ERROR_MESSAGE()

	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

END CATCH
