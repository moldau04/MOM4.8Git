CREATE PROCEDURE [dbo].[spUpdateUserPassword] @UserName NVARCHAR(50),
                                             @Password VARCHAR(50),
											 @NewPassword VARCHAR(50),
                                             @DbName   VARCHAR(50),
                                             @DBType   VARCHAR(50)=''
AS
    DECLARE @DBNameSys VARCHAR(50)
    
    SET @DBNameSys=@DbName

    IF NOT EXISTS (SELECT 1
                   FROM   sys.databases
                   WHERE  NAME = @DBNameSys)
    BEGIN
        RAISERROR('Invalid Company Database',16,1)

        RETURN
    END

    IF NOT EXISTS (SELECT u.ID
                   FROM   tblUser u
                   WHERE  fUser = @UserName
				   UNION
                   SELECT o.ID
                   FROM   Owner o
                   WHERE  fLogin = @UserName
                   )
    BEGIN
        RAISERROR('Invalid Username',16,1)

        RETURN
    END

    IF NOT EXISTS (SELECT u.ID
                   FROM   tblUser u
                   WHERE  u.fUser = @UserName
                          --AND u.Password = @Password
						  AND Convert(varbinary, u.Password) = Convert(varbinary,@Password)
				   UNION
                   SELECT o.ID
                   FROM   Owner o
                   WHERE  o.fLogin = @UserName
                          --AND o.Password = @Password
						  AND Convert(varbinary, o.Password) = Convert(varbinary,@Password)	  
						  
						  )
    BEGIN
        RAISERROR('Invalid Password',16,1)

        RETURN
    END

	DECLARE @Field smallint
	DECLARE @Password0 varchar(50),@Password1 varchar(50),@Password2 varchar(50)

	IF EXISTS (SELECT u.ID
                   FROM   tblUser u
                   WHERE  fUser = @UserName)
	BEGIN
		SELECT @Password0=ISNULL(u.Password,''), @Password1=ISNULL(u.Password1,''), @Password2 = ISNULL(u.Password2,''), @Field = e.Field
		FROM tblUser u INNER JOIN Emp e ON e.CallSign = u.fUser
		WHERE u.fUser = @UserName
	END
	ELSE
	BEGIN
		IF EXISTS (SELECT o.ID
                   FROM   Owner o
                   WHERE  fLogin = @UserName)
		BEGIN
			SELECT @Password0=ISNULL(u.Password,''), @Password1=ISNULL(u.Password1,''), @Password2 = ISNULL(u.Password2,''), @Field = 2
			FROM Owner u
			WHERE u.fLogin = @UserName
		END
	END

	IF EXISTS(select 1 from Control 
				where ApplyPasswordRules = 1 
					and ((@field = 0 and ApplyPwRulesToOfficeUser = 1)
						or (@field = 2 and ApplyPwRulesToCustomerUser = 1)
						or (@field = 1 and ApplyPwRulesToFieldUser = 1)))
	BEGIN
		IF((@Password0 != '' AND Convert(varbinary, @NewPassword) = Convert(varbinary, @Password0))
			OR (@Password1 != '' AND Convert(varbinary, @NewPassword) = Convert(varbinary, @Password1))
			OR (@Password2 != '' AND Convert(varbinary, @NewPassword) = Convert(varbinary, @Password2)))
		BEGIN
			RAISERROR (
				'Your New Password should not match your last 3 passwords. Please use a new one!'
				,16
				,1
				)
			RETURN
		END
	END

	-- Office and Field user
	IF EXISTS (SELECT u.ID
                   FROM   tblUser u
                   WHERE  fUser = @UserName)
	BEGIN
		-- Update last login date and login failed attempts when logged in successfully: Login from MOM
		UPDATE tblUser SET LastLoginDate=null,LastUpdatePasswordDate=GETDATE(), LoginFailedAttempts = 0,
			Password = @NewPassword, Password1=Password, Password2=Password1 WHERE fUser = @UserName
		END
	ELSE
	BEGIN
		-- Customer user
		IF EXISTS (SELECT o.ID
                   FROM   Owner o
                   WHERE  fLogin = @UserName)
		BEGIN
			-- Update last login date and login failed attempts when logged in successfully: Login from MOM
			UPDATE Owner SET LastUpdatePasswordDate=GETDATE(), 
				LoginFailedAttempts = 0, Password = @NewPassword, Password1=Password, 
				Password2=Password1, ForgotPwRequest=0
			WHERE fLogin = @UserName
		END
	END