CREATE PROCEDURE [dbo].[spAddProspectContact]
	@ContactData As [dbo].[tbltypePContacts]   Readonly,
	@ProspectID INT
AS
BEGIN
	DECLARE @Rol INT
    SELECT @Rol=Rol FROM Prospect WHERE ID=@ProspectID

	 EXEC spUpdateProspectContact @ContactData,@Rol
END
GO
