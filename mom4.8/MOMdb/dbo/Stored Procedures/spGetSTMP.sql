CREATE PROCEDURE [dbo].[spGetSTMP]
	@UserID INT
AS
BEGIN
	DECLARE @Host VARCHAR(50)
	DECLARE @UserName VARCHAR(50)
	DECLARE @Password VARCHAR(50)
	DECLARE @Port VARCHAR(50)
	DECLARE @SSL VARCHAR(50)
	DECLARE @From VARCHAR(50)
	DECLARE @BCCEmail NVARCHAR(50)

	SET @From=(SELECT TOP 1 r.EMail FROM 
			tbluser u INNER JOIN
				Emp e ON u.fUser=e.CallSign
					INNER JOIN Rol r ON e.Rol =r.ID WHERE u.Id=@UserID) 

	IF EXISTS (SELECT * FROM tblEmailAccounts WHERE UserId=@UserID)
		BEGIN
			SELECT @Host=OutServer,@UserName=OutUsername,@Password=OutPassword,@Port=OutPort, @SSL=[SSL],@BCCEmail=BccEmail FROM tblEmailAccounts WHERE UserId=@UserID
		END
	ELSE
		BEGIN
			SET @From=(SELECT R.EMail FROM tblUser U LEFT OUTER JOIN Emp E ON U.fUser = E.CallSign LEFT OUTER JOIN Rol R ON E.Rol =R.ID WHERE U.ID=@UserID)

			IF(@From IS NULL OR @From='')
				BEGIN
					SET @From=(SELECT TOP 1 Email FROM Control)
				END
		END

		
	SELECT @Host AS Host,@UserName AS UserName,@Password AS [Password],@Port AS Port,@SSL AS [SSL],@From AS [From],@BCCEmail AS [BCCEmail]

END