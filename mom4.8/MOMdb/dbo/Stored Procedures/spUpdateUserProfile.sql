-- =============================================
-- Created by:  Quant
-- Created on:  23 Apr 2018
-- Description: Add column
-- Modified by: Quant
-- Modified on: 23 Apr 2018
-- Description: 
--
-- Modified by: Quant
-- Modified on: 19 Sept 2018
-- Description: Remove change status = 1 when updating profile
-- =============================================

CREATE PROCEDURE [dbo].[spUpdateUserProfile] 
	 @UserName NVARCHAR(50)
	,@Field SMALLINT
	,@FName VARCHAR(15)
	,@MName VARCHAR(15)
	,@LName VARCHAR(25)
	,@Address VARCHAR(8000)
	,@City VARCHAR(50)
	,@State VARCHAR(2)
	,@Zip VARCHAR(10)
	,@Tel VARCHAR(22)
	,@Cell VARCHAR(22)
	,@Email VARCHAR(50)
	,@Pager VARCHAR(100)
	,@UserID INT
	,@Password VARCHAR(10)
	,@salesp INT	
	,@InServer VARCHAR(100)
	,@InServerType VARCHAR(10)
	,@InUsername VARCHAR(100)
	,@InPassword VARCHAR(50)
	,@InPort INT
	,@OutServer VARCHAR(100)
	,@OutUsername VARCHAR(100)
	,@OutPassword VARCHAR(50)
	,@OutPort INT
	,@SSL BIT
	,@BccEmail VARCHAR(100) = NULL
	,@EmailAccount INT	
	,@Lng NVARCHAR(100)
	,@Lat NVARCHAR(100)
	,@Country NVARCHAR(100)	
	,@EmNum NVARCHAR(100)
	,@EmName NVARCHAR(100)
	,@Title NVARCHAR(100)
	,@ProfileImage NVARCHAR(MAX)
	,@CoverImage NVARCHAR(MAX)
	,@TakeASentEmailCopy BIT
AS
DECLARE @RolID INT
DECLARE @EmpID INT
SELECT @EmpID = Isnull(e.ID, 0)
	,@RolID = Isnull(e.Rol, 0)
FROM Emp e
INNER JOIN tblUser u ON u.fUser = e.CallSign
WHERE u.fUser = @UserName

IF (@EmpID IS NULL)
BEGIN
	SET @EmpID = 0
END

IF (@RolID IS NULL)
BEGIN
	SET @RolID = 0
END


BEGIN TRANSACTION

IF (@Field <> 2)
BEGIN  

	IF EXISTS (select 1 from Control c
			inner join tblUser u on u.id = c.PwResetUserID
			where ApplyPasswordRules = 1
			and u.ID = @UserID)
	BEGIN
		IF (@EmailAccount = 0)
		BEGIN
			RAISERROR (
				'Removing email account error. This email settings were used for reseting password.'
				,16
				,1
				)
			ROLLBACK TRANSACTION
			RETURN
		END
	END

	IF NOT EXISTS (
			SELECT 1
			FROM tblUser
			WHERE fUser = @UserName
				AND ID <> @UserID
			
			UNION
			
			SELECT 1
			FROM OWNER
			WHERE fLogin = @UserName
			
			UNION
			
			SELECT 1
			FROM tblLocationRole
			WHERE Username = @UserName
			)
	BEGIN		
		-- Check and reset LastLoginDate, LoginFailedAttempts in update password
		IF NOT EXISTS (select 1 from tblUser WHERE ID = @UserID AND Convert(varbinary, Password) = Convert(varbinary,@Password))
		BEGIN
			DECLARE @Password0 varchar(50),@Password1 varchar(50),@Password2 varchar(50)
			SELECT @Password0=ISNULL(Password,''), @Password1=ISNULL(Password1,''), @Password2 = ISNULL(Password2,'') FROM tblUser WHERE ID = @UserID
			IF EXISTS(select 1 from Control 
						where ApplyPasswordRules = 1 
							and ((@field = 0 and ApplyPwRulesToOfficeUser = 1)
								--or (@field = 2 and ApplyPwRulesToCustomerUser = 1)
								or (@field = 1 and ApplyPwRulesToFieldUser = 1)))
			BEGIN
				IF(--(@Password0 != '' AND Convert(varbinary, @Password) = Convert(varbinary, @Password0))
					--OR 
					(@Password1 != '' AND Convert(varbinary, @Password) = Convert(varbinary, @Password1))
					OR (@Password2 != '' AND Convert(varbinary, @Password) = Convert(varbinary, @Password2)))
				BEGIN
					RAISERROR (
						'Your New Password should not match your last 3 passwords. Please use a new one!'
						,16
						,1
						)
					ROLLBACK TRANSACTION
					RETURN
				END
			END

			UPDATE tblUser SET Password = @Password, Password1=@Password0, Password2=@Password1, LastLoginDate = null, LoginFailedAttempts = 0, LastUpdatePasswordDate = GETDATE()
			WHERE ID = @UserID
		END

		UPDATE tblUser
		SET			
			--Password = @Password	
			--,
			LastUpdateDate = Getdate()
			,EmailAccount = @EmailAccount			
			,Lng = @Lng
			,Lat = @Lat
			,Country = @Country
			,Title = @Title
			,ProfileImage = Isnull(@ProfileImage,ProfileImage)
			,CoverImage = Isnull(@CoverImage,CoverImage)

		WHERE ID = @UserID
	END
	ELSE
	BEGIN
		RAISERROR (
				'Username already exixts, please use different username!'
				,16
				,1
				)

		ROLLBACK TRANSACTION

		RETURN
	END
END

IF @@ERROR <> 0
	AND @@TRANCOUNT > 0
BEGIN
	RAISERROR (
			'Error Occured'
			,16
			,1
			)

	ROLLBACK TRANSACTION

	RETURN
END

--IF @@ERROR <> 0
--	AND @@TRANCOUNT > 0
--BEGIN
--	RAISERROR (
--			'Error Occured'
--			,16
--			,1
--			)

--	ROLLBACK TRANSACTION

--	RETURN
--END

IF (@RolID <> 0)
BEGIN
	UPDATE Rol
	SET NAME = @FName + ', ' + @LName
		,City = @City
		,STATE = @State
		,Zip = @Zip
		,Phone = @Tel
		,Address = @Address
		,EMail = @Email
		,Cellular = @Cell
		,Contact = @EmName
		,Website = @EmNum
	WHERE ID = @RolID
END
ELSE
BEGIN
	INSERT INTO Rol (
		NAME
		,City
		,STATE
		,Zip
		,Phone
		,Address
		,EMail
		,Cellular
		,GeoLock
		,Contact
		,Website
		)
	VALUES (
		@FName + ', ' + @LName
		,@City
		,@State
		,@Zip
		,@Tel
		,@Address
		,@Email
		,@Cell
		,0
		,@EmName
		,@EmNum
		)

	SET @RolID = Scope_identity()
END

IF @@ERROR <> 0
	AND @@TRANCOUNT > 0
BEGIN
	RAISERROR (
			'Error Occured'
			,16
			,1
			)

	ROLLBACK TRANSACTION

	RETURN
END 

IF (@Field <> 2)
BEGIN
	IF (@EmpID <> 0)
	BEGIN	
		UPDATE Emp
		SET Field = @Field
			,Pager = @Pager
			,fFirst = @FName
			,Middle = @MName
			,Last = @LName				
			,CallSign = @UserName
			,NAME = @FName + ', ' + @LName
			,Title = @Title
		WHERE ID = @EmpID
			
	END
	ELSE
	BEGIN
		INSERT INTO Emp (
				Field
				,STATUS
				,fFirst
				,Middle
				,Last
				,CallSign
				,Rol
				,Sales
				,InUse
				,NAME
				,Pager
				,Title
				)
			VALUES (
				@Field
				,0
				,@FName
				,@MName
				,@LName
				,@UserName
				,@RolID
				,@salesp
				,0
				,@FName + ', ' + @LName
				,@Pager
				,@Title
				)
		SET @EmpID = Scope_identity()
	END

	IF @@ERROR <> 0
		AND @@TRANCOUNT > 0
	BEGIN
		RAISERROR (
				'Error Occured'
				,16
				,1
				)

		ROLLBACK TRANSACTION

		RETURN
	END

	IF (@salesp = 1)
	BEGIN
		IF (@EmpID <> 0)
		BEGIN
			IF NOT EXISTS (
					SELECT 1
					FROM Terr
					WHERE SMan = @EmpID
					)
			BEGIN
				INSERT INTO Terr (
					NAME
					,SMan
					,SDesc
					,Count
					,Symbol
					,EN
					)
				VALUES (
					@UserName
					,@empid
					,@LName + ', ' + @FName
					,0
					,1
					,1
					)
			END

			IF @@ERROR <> 0
				AND @@TRANCOUNT > 0
			BEGIN
				RAISERROR (
						'Error Occured'
						,16
						,1
						)

				ROLLBACK TRANSACTION

				RETURN
			END
		END

	END

	IF (@EmailAccount = 1)
	BEGIN
		EXEC Spaddemailaccount @InServer
			,@InServerType
			,@InUsername
			,@InPassword
			,@InPort
			,@OutServer
			,@OutUsername
			,@OutPassword
			,@OutPort
			,@SSL
			,@UserID
			,@BccEmail
			,@TakeASentEmailCopy
	END

	IF @@ERROR <> 0
		AND @@TRANCOUNT > 0
	BEGIN
		RAISERROR (
				'Error Occured'
				,16
				,1
				)

		ROLLBACK TRANSACTION

		RETURN
	END
END
COMMIT TRANSACTION
 