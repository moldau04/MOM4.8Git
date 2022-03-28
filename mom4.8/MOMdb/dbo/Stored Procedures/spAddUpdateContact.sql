CREATE PROCEDURE [dbo].[spAddUpdateContact]
	@ID INT,
	@RolID int,
	@Name varchar(50),
	@Title varchar(50),
	@Phone varchar(50),
	@Fax varchar(22),
	@Cell varchar(22),
	@Email varchar(100)
AS
BEGIN
--BEGIN TRANSACTION
	IF @ID = 0 OR @ID is null
	BEGIN
		IF(ISNULL(@Name,'') != '')
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM Phone WHERE Rol =@RolID and fDesc = @Name)
			BEGIN 
				INSERT INTO Phone
				(
					Rol,fDesc,Phone,Fax,Cell,Email,Title
				)
				VALUES
				(
					@RolID,@Name,@Phone,@Fax,@Cell,@Email, @Title
				)
			END
			ELSE
			BEGIN
				RAISERROR ('Contact name already existed', 16, 1)  
				--ROLLBACK TRANSACTION    
				RETURN
			END
		END	 
	END
	ELSE
	BEGIN
		IF EXISTS(SELECT 1 FROM Phone WHERE ID = @ID)
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM Phone WHERE ID != @ID AND Rol =@RolID and fDesc = @Name)
			BEGIN
				UPDATE Phone SET fDesc = @Name, Title = @Title, Phone = @Phone, Fax = @Fax, Cell = @Cell, Email = @Email WHERE ID = @ID
			END
			ELSE
			BEGIN
				RAISERROR ('Contact name already existed', 16, 1)  
				ROLLBACK TRANSACTION    
				RETURN
			END
		END
		ELSE
		BEGIN
			RAISERROR ('Contact did not exist', 16, 1)  
			--ROLLBACK TRANSACTION    
			RETURN
		END
	END
END
