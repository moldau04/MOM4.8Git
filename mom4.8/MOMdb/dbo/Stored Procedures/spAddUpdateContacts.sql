CREATE PROCEDURE [dbo].[spAddUpdateContacts]
	@IsUpdate BIT,
	@ID INT,
	@Name VARCHAR(250),
	@Phone VARCHAR(250),
	@Fax VARCHAR(250),
	@Cell VARCHAR(250),
	@Email VARCHAR(250),
	@Title VARCHAR(250),
	@Tickets BIT,
	@InvoiceStatements BIT,
	@Shutdown BIT,
	@Tests BIT,
	@CType VARCHAR(250),
	@LocID INT,
	@CustID INT
AS
BEGIN
	IF(@IsUpdate=1)
		BEGIN
			UPDATE [dbo].[Phone] SET 
			   [fDesc] = @Name
			  ,[Phone] = @Phone
			  ,[Fax] = @Fax
			  ,[Title] = @Title
			  ,[Cell] = @Cell
			  ,[Email] = @Email
			  ,[EmailRecInvoice] =@InvoiceStatements
			  ,[EmailRecTicket] = @Tickets
			  ,[ShutdownAlert] = @Shutdown
			  ,[EmailRecTestProp] = @Tests
			WHERE ID=@ID
		END
	ELSE
		BEGIN
			DECLARE @ROL_ID AS INT
			IF(@CType='Location')
				BEGIN
					SET @ROL_ID=(SELECT TOP 1 Rol FROM Loc WHERE LOC=@LocID)
				END
			ELSE
				BEGIN
					SET @ROL_ID=(SELECT TOP 1  Rol From Owner Where ID=@CustID)
				END

			INSERT INTO [dbo].[Phone]
           ([Rol]
           ,[fDesc]
           ,[Phone]
           ,[Fax]
           ,[Title]
           ,[Cell]
           ,[Email]
           ,[EmailRecInvoice]
           ,[EmailRecTicket]
           ,[ShutdownAlert]
           ,[EmailRecTestProp])
     VALUES
           (@ROL_ID
           ,@Name
           ,@Phone
           ,@Fax
           ,@Title
           ,@Cell
           ,@Email
           ,@InvoiceStatements
           ,@Tickets
           ,@Shutdown
           ,@Tests)
		END
END
GO
