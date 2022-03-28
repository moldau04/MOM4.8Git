CREATE PROCEDURE [dbo].[spAddContactAsPerRole]
	@RolID INT,
	@Name VARCHAR(50),
	@Phone VARCHAR(50),
	@Fax VARCHAR(50),
	@Cell VARCHAR(50),
	@Email VARCHAR(50),
	@Title VARCHAR(50)
AS
BEGIN

	 INSERT INTO Phone(Rol,fDesc,Phone,Fax,Cell,Email,Title)VALUES
	 (@RolID,@Name,@Phone,@Fax,@Cell,@Email,@Title)

END
GO
