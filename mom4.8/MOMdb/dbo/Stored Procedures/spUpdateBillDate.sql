CREATE PROCEDURE [dbo].[spUpdateBillDate]
	@PJID int,
	@Batch int,
	@TRID int,
	@PostDate datetime,
	@IDate datetime,
	@Due datetime
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
	BEGIN TRANSACTION

	UPDATE PJ 
		SET fDate = @PostDate, 
			IDate = @IDate 
		WHERE ID = @PJID

	UPDATE Paid	
		SET fDate = @IDate
		WHERE TRID = @TRID

	UPDATE OpenAP
		SET fDate = @IDate,
			Due = @Due
		WHERE	Type = 0 AND 
				PJID = @PJID

	UPDATE JobI
		SET fDate = @PostDate
		WHERE TransID IN (SELECT ID FROM Trans WHERE Batch = @Batch) AND Type = 1

	UPDATE Trans
		SET fDate = @PostDate
		WHERE Batch = @Batch

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