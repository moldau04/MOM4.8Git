CREATE PROCEDURE [dbo].[UpdateEstimateWithLoc]
	@LocID INT
AS
BEGIN
	DECLARE @CustomerID AS INT
	DECLARE @CustomerName AS VARCHAR(50)
	DECLARE @LocationName AS VARCHAR(50)
	IF EXISTS (SELECT * FROM Loc WHERE Loc=@LocID)
	BEGIN
	
			SET @CustomerID=(SELECT TOP 1 Owner FROM LOC WHERE Loc=@LocID)
			SET @LocationName=(SELECT TOP 1 Tag FROM LOC WHERE Loc=@LocID)
			SET @CustomerName=(SELECT TOP 1 Name FROM ROL WHERE ID=(SELECT TOP 1 Rol FROM  Owner WHERE ID=@CustomerID))
			print(@CustomerID)
				print(@LocationName)
					print(@CustomerName)

			UPDATE Estimate SET LocID=@LocID,fFor='ACCOUNT' WHERE CompanyName=@CustomerName AND EstimateAddress=@LocationName

			UPDATE ROL SET Type=4 WHERE ID IN(SELECT RolID FROM Estimate  WHERE CompanyName=@CustomerName AND EstimateAddress=@LocationName)
	END
	
END
