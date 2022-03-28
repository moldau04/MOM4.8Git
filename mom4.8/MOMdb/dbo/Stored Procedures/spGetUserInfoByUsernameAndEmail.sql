CREATE PROCEDURE [dbo].[spGetUserInfoByUsernameAndEmail]
	@Username varchar(50),
	@Email VARCHAR (100),
	@DbName   VARCHAR(50),
    @DBType   VARCHAR(50)='',
	@ForgotPwRequest bit
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
                   UNION
                   SELECT 1
                   FROM   tblLocationRole
                   WHERE  Username = @UserName)
BEGIN
    RAISERROR('Invalid Username',16,1)

    RETURN
END

-- Check if password policy is applied for this type of user
DECLARE @ApplyPwPolicy bit--, @ApplyToOfficeUser bit, @ApplyToFieldUser bit, @ApplyToCustomerUser bit
DECLARE @UserType smallint
DECLARE @UserAppliedPwPolicy bit

IF EXISTS (SELECT u.ID
                   FROM   tblUser u
                   WHERE  fUser = @UserName)
BEGIN
	SELECT @UserType=e.Field
	FROM tblUser u 
	INNER JOIN Emp e ON e.CallSign = u.fUser 
	WHERE u.fUser = @UserName
END
ELSE
BEGIN
	IF EXISTS (SELECT o.ID
                FROM   Owner o
                WHERE  fLogin = @UserName)
	BEGIN
		SELECT @UserType=2
	END
END

SELECT TOP 1 @UserAppliedPwPolicy = 
	CASE WHEN @UserType = 0 THEN ApplyPwRulesToOfficeUser
		WHEN @UserType = 1 THEN ApplyPwRulesToFieldUser
		WHEN @UserType = 2 THEN ApplyPwRulesToCustomerUser
	END,
	@ApplyPwPolicy = ApplyPasswordRules--,
FROM Control c left join tblUser u ON c.PwResetUserID = u.ID

IF (ISNULL(@UserAppliedPwPolicy, 0) = 0)
BEGIN
    RAISERROR('Invalid User type',16,1)

    RETURN
END

IF NOT EXISTS (SELECT u.ID
                FROM   tblUser u INNER JOIN Emp e on e.CallSign = u.fUser 
				INNER JOIN Rol r on e.Rol=r.ID
                WHERE  fUser = @UserName AND r.EMail = @Email
				UNION
				SELECT o.ID from Owner o
				INNER JOIN rol r on r.id = o.Rol 
				WHERE  fLogin = @UserName AND r.EMail = @Email
                )
BEGIN
    RAISERROR('Username and Email Address are invalid, Please enter correct Username and Email Address',16,1)

    RETURN
END

IF EXISTS (SELECT u.ID
                FROM   tblUser u INNER JOIN Emp e on e.CallSign = u.fUser 
				INNER JOIN Rol r on e.Rol=r.ID
                WHERE  fUser = @UserName AND r.EMail = @Email)
BEGIN
	SELECT u.fUser,
		e.fFirst,
		e.Last,
		e.Field UserType,
		CASE WHEN e.Field  = 0 THEN 'Office'
							WHEN e.Field  = 1 THEN 'Field'
							--WHEN e.Field  = 2 THEN 'Customer'
						END UserTypeName,
		(SELECT TOP 1 CASE WHEN e.Field  = 0 THEN ApplyPwRulesToOfficeUser
							WHEN e.Field  = 1 THEN ApplyPwRulesToFieldUser
							--WHEN e.Field  = 2 THEN ApplyPwRulesToCustomerUser
						END
			FROM Control) UserApplyPwRules,
		(SELECT TOP 1 ApplyPasswordRules FROM Control) ApplyPasswordRules,
		IsNull(ForgotPwRequest,0) ForgotPwRequest
	FROM tblUser u 
	INNER JOIN Emp e on e.CallSign = u.fUser 
	WHERE u.fUser = @Username

	IF (@ForgotPwRequest = 1)
	BEGIN
		UPDATE tblUser SET ForgotPwRequest = @ForgotPwRequest WHERE fUser = @Username
	END
END
ELSE
BEGIN
	SELECT o.fLogin fUser,
		r.Name fFirst,
		r.Name Last,
		2 UserType,
		'Customer' UserTypeName,
		(SELECT TOP 1 ApplyPwRulesToCustomerUser FROM Control) UserApplyPwRules,
		(SELECT TOP 1 ApplyPasswordRules FROM Control) ApplyPasswordRules,
		IsNull(o.ForgotPwRequest,0) ForgotPwRequest
	FROM Owner o 
	INNER JOIN Rol r on r.ID = o.Rol 
	WHERE o.fLogin = @Username

	IF (@ForgotPwRequest = 1)
	BEGIN
		UPDATE Owner SET ForgotPwRequest = @ForgotPwRequest WHERE fLogin = @Username
	END
END
