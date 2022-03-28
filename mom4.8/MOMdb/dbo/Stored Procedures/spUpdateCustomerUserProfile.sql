-- =============================================
-- Created by:  Quant
-- Created on:  23 Apr 2018
-- Description: Add column
-- Modified by: Quant
-- Modified on: 23 Apr 2018
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateCustomerUserProfile] 
											 @UserName    NVARCHAR(15),
											 @Password    NVARCHAR(10),
											 @FName       VARCHAR(75),
											 @Address     VARCHAR(8000),
											 @City        VARCHAR(50),
											 @State       VARCHAR(2),
											 @Zip         VARCHAR(10),
											 @country     VARCHAR(50),
											 @Title		  NVARCHAR(100),	
											 @Lng NVARCHAR(100),
											 @Lat NVARCHAR(100),
											 @CustomerId  INT,										
											 @phone       VARCHAR(28),											
											 @email       VARCHAR(50),
											 @Cell        VARCHAR(28),
											 @ProfileImage NVARCHAR(MAX),
											 @CoverImage NVARCHAR(MAX)
											 										
                                         
AS
    
DECLARE @Rol INT
DECLARE @work INT
DECLARE @DucplicateCustName INT = 0

SELECT @DucplicateCustName = Count(1)
FROM   Rol r
		INNER JOIN Owner o
				ON o.Rol = r.ID
WHERE  Name = @FName
		AND o.ID <> @CustomerId

IF( @DucplicateCustName = 0 )
BEGIN
    BEGIN TRANSACTION

    SELECT @Rol = Rol
    FROM   Owner
    WHERE  ID = @CustomerId

    UPDATE Rol
    SET    Name = @FName,
            City = @City,
            State = @State,
            Zip = @Zip,
            Address = @Address,
			Lat =@Lat,
			Lng =@Lng,
            Country = @country,
            Phone = @phone,
            EMail = @email,
            Cellular = @Cell,
            LastUpdateDate = Getdate()
    WHERE  id = @Rol

    IF @@ERROR <> 0 AND @@TRANCOUNT > 0
    BEGIN
        RAISERROR ('Error Occured',16,1)

        ROLLBACK TRANSACTION

        RETURN
    END

    IF NOT EXISTS (SELECT 1
                    FROM   Owner
                    WHERE  fLogin = @UserName
                            AND ID <> @CustomerId
                            AND @UserName <> ''
                    UNION
                    SELECT 1
                    FROM   tblUser
                    WHERE  fUser = @UserName
                               
                    UNION
                    SELECT 1
                    FROM   tblLocationRole
                    WHERE  Username = @UserName
                    )
    BEGIN
		-- Check and reset LastLoginDate, LoginFailedAttempts in update password
		IF NOT EXISTS (select 1 from Owner WHERE fLogin = @UserName AND Convert(varbinary, Password) = Convert(varbinary,@Password))
		BEGIN
			DECLARE @Password0 varchar(50),@Password1 varchar(50),@Password2 varchar(50)
			SELECT @Password0=ISNULL(Password,''), @Password1=ISNULL(Password1,''), @Password2 = ISNULL(Password2,'') FROM Owner WHERE fLogin = @UserName
			IF EXISTS(select 1 from Control 
						where ApplyPasswordRules = 1 
							and ApplyPwRulesToCustomerUser = 1)
			BEGIN
				IF(@Password1 != '' AND Convert(varbinary, @Password) = Convert(varbinary, @Password1))
					OR (@Password2 != '' AND Convert(varbinary, @Password) = Convert(varbinary, @Password2))
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

			UPDATE Owner SET Password = @Password, Password1=@Password0, Password2=@Password1, LoginFailedAttempts = 0, LastUpdatePasswordDate = GETDATE()
			WHERE fLogin = @UserName
		END

		UPDATE Owner SET
			fLogin = @UserName,
			--Password = @Password,
			ProfileImage = Isnull(@ProfileImage,ProfileImage),
			CoverImage = Isnull(@CoverImage,CoverImage),
			Title = @Title							
		WHERE  ID = @CustomerId
    END
    ELSE
    BEGIN
        RAISERROR ('Username already exists, please use different username!',16,1)

        ROLLBACK TRANSACTION

        RETURN
    END

    IF @@ERROR <> 0
        AND @@TRANCOUNT > 0
	BEGIN
		RAISERROR ('Error Occured',16,1)

		ROLLBACK TRANSACTION

		RETURN
	END

			
    COMMIT TRANSACTION
END
ELSE
BEGIN
	RAISERROR ('Customer name already exists, please use different Customer name !',16,1)

	RETURN
END

