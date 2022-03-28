CREATE PROCEDURE [dbo].[spUpdateContactByID]
	@RolID INT,
	@Name VARCHAR(50),
	@Phone VARCHAR(50),
	@Fax VARCHAR(50),
	@Cell VARCHAR(50),
	@Email VARCHAR(50),
	@Title VARCHAR(50),
	@ID INT
AS
BEGIN

     UPDATE Phone SET fDesc=@Name, Phone=@Phone, Fax=@Fax, Cell=@Cell, Email=@Email, Title=@Title WHERE ID=@ID
	
END
