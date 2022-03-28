CREATE PROCEDURE [dbo].[spGetUserInfoByUsername]
	@Username varchar(50),
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
                )
BEGIN
    RAISERROR('Invalid Username',16,1)

    RETURN
END

SELECT u.fUser,
	ISNULL(e.fFirst, '') fFirst,
	ISNULL(e.Last, '') Last,
	e.Field UserType,
	(SELECT TOP 1 CASE WHEN e.Field  = 0 THEN ApplyPwRulesToOfficeUser
						WHEN e.Field  = 1 THEN ApplyPwRulesToFieldUser
						WHEN e.Field  = 2 THEN ApplyPwRulesToCustomerUser
					END
		FROM Control) UserApplyPwRules,
	(SELECT TOP 1 ApplyPasswordRules FROM Control) ApplyPasswordRules,
	ISNULL(r.EMail, '') EMail
FROM tblUser u 
LEFT JOIN Emp e on e.CallSign = u.fUser 
LEFT JOIN Rol r on r.Id = e.Rol
WHERE u.fUser = @Username