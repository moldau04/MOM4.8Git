CREATE PROCEDURE [dbo].[spDeleteContact]
	@ID int
AS
IF @ID = 0 OR @ID is null
BEGIN
	RAISERROR ('Contact did not exist', 16, 1)  
	RETURN
END
ELSE
BEGIN
	IF EXISTS(SELECT 1 FROM Phone WHERE ID = @ID)
	BEGIN
		DELETE Phone WHERE ID = @ID
		IF @@ERROR <> 0
		BEGIN
			RAISERROR ('Error on delete contact', 16, 1)  
			RETURN
		END
	END
	ELSE
	BEGIN
		RAISERROR ('Contact did not exist', 16, 1)  
		RETURN
	END
END