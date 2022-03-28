CREATE PROCEDURE [dbo].[spGetCompanyName]
	@LeadID INT
AS
BEGIN
	DECLARE @Rol AS INT
	DECLARE @Rol_Owner AS INT
	DECLARE @TYPE AS INT
	DECLARE @OwnerID AS INT
	DECLARE @CompanyName AS VARCHAR(50)

	SET @Rol =(SELECT TOP 1 Rol FROM Lead WHERE ID=@LeadID)
	SET @TYPE=(SELECT TOP 1 Type FROM Rol WHERE ID = @Rol)

	IF @TYPE = 3
		BEGIN
			SET @CompanyName=(SELECT TOP 1 CustomerName FROM Prospect WHERE Prospect.Rol=@Rol)
		END
	ELSE
		BEGIN
			SET @OwnerID=(SELECT TOP 1 [Owner] FROM Loc WHERE Rol = @Rol)
			SET @Rol_Owner=(SELECT TOP 1 Rol FROM Owner WHERE ID = @OwnerID)
			SET @CompanyName=(SELECT TOP 1 Name FROM Rol WHERE ID = @Rol_Owner)
		END
	SELECT @CompanyName AS CompanyName
END

