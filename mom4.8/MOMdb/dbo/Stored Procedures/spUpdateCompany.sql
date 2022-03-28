CREATE PROCEDURE [dbo].[spUpdateCompany] @CompanyName   VARCHAR(75),
                                        @Address       VARCHAR(255),
                                        @City          VARCHAR(50),
                                        @State         VARCHAR(2),
                                        @Zip           VARCHAR(10),
                                        @Tel           VARCHAR(20),
                                        @fax           VARCHAR(20),
                                        @Email         VARCHAR(50),
                                        @Web           VARCHAR(50),
                                        @Contact       VARCHAR(50),
                                        @Remarks       VARCHAR(200),
                                        @logo          IMAGE,
                                        @custRegister  SMALLINT,
                                        @QBPath        VARCHAR(500),
                                        @MultiLang     SMALLINT,
                                        @Qbintegration SMALLINT,
                                        @EmailMS smallint,
                                        @QBFirstSync smallint,
                                        @ServiceItem varchar(100),
                                        @ServiceItemLabor varchar(100),
                                        @ServiceItemExp varchar(100),
										@YE int,
										@GSTreg varchar(20),
										@Lat varchar(50),
										@Lng varchar(50),
										@consultAPI SMALLINT,

                                        --,
                                        --@TransferTimesheet smallint,
                                        --@TransferInvoice smallint
                                        --,
                                        --@MerchantID    VARCHAR(100),
                                        --@LoginID       VARCHAR(100),
                                        --@Username      VARCHAR(20),
                                        --@Password      VARCHAR(100)
										@ApplyPasswordRules bit,
										@ApplyPwRulesToFieldUser bit,
										@ApplyPwRulesToOfficeUser bit,
										@ApplyPwRulesToCustomerUser bit,
										@ApplyPwReset bit,
										@PwResetDays int,
										@PwResetting smallint,
										--@EmailAdministrator varchar(50),
										@UserID int,
										@Payroll SMALLINT
AS
    BEGIN TRANSACTION

	--SET @EmailAdministrator = ''

	--SELECT @EmailAdministrator = r.EMail FROM tblUser u inner join Emp e on e.CallSign = u.fUser
	--Inner join rol r on r.Id = e.Rol
	--WHERE u.ID = @UserID 

	DECLARE @CurrApplyPwReset bit,
		@CurrApplyPwRulesToFieldUser bit,
		@CurrApplyPwRulesToOfficeUser bit,
		@CurrApplyPwRulesToCustomerUser bit,
		@CurrPwResetDays int,
		@IsResetLastUpdatePasswordDate bit = 0

	SELECT TOP 1 @CurrApplyPwReset=ISNULL(ApplyPwReset,0)
		, @CurrApplyPwRulesToFieldUser = ApplyPwRulesToFieldUser
		, @CurrApplyPwRulesToOfficeUser = ApplyPwRulesToOfficeUser
		, @CurrApplyPwRulesToCustomerUser = ApplyPwRulesToCustomerUser
		, @CurrPwResetDays = ISNULL(PwResetDays, 0)
	FROM [Control]

	IF @ApplyPwReset = 1
	BEGIN
		IF @CurrApplyPwReset = 0
		BEGIN
			SET @IsResetLastUpdatePasswordDate = 1
		END
		ELSE
		BEGIN
			IF @CurrPwResetDays != @PwResetDays
			BEGIN
				SET @IsResetLastUpdatePasswordDate = 1
			END
		END

		IF @IsResetLastUpdatePasswordDate = 1
		BEGIN
			-- Set last update password date by the date applied reset password by days
			UPDATE tblUser SET LastUpdatePasswordDate = GETDATE()
			UPDATE Owner SET LastUpdatePasswordDate = GETDATE()
		END
		ELSE
		BEGIN
			IF @ApplyPwRulesToFieldUser = 1 AND @CurrApplyPwRulesToFieldUser != @ApplyPwRulesToFieldUser
			BEGIN
				UPDATE u SET u.LastUpdatePasswordDate = GETDATE() FROM tblUser u INNER JOIN Emp e ON e.CallSign = u.fUser WHERE e.Field = 1
			END

			IF @ApplyPwRulesToOfficeUser = 1 AND @CurrApplyPwRulesToOfficeUser != @ApplyPwRulesToOfficeUser
			BEGIN
				UPDATE u SET u.LastUpdatePasswordDate = GETDATE() FROM tblUser u INNER JOIN Emp e ON e.CallSign = u.fUser WHERE e.Field = 0
			END

			IF @ApplyPwRulesToCustomerUser = 1 AND @CurrApplyPwRulesToCustomerUser != @ApplyPwRulesToCustomerUser
			BEGIN
				UPDATE Owner SET LastUpdatePasswordDate = GETDATE()
			END
		END
	END

	

    UPDATE Control
    SET    Name = @CompanyName,
           Address = @Address,
           City = @City,
           State = @State,
           Zip = @Zip,
           Phone = @Tel,
           Fax = @fax,
           Email = @Email,
           WebAddress = @Web,
           Contact = @Contact,
           Remarks = @Remarks,
           Logo = @logo,
           Custweb = @custRegister,
           QBPath = @QBPath,
           MultiLang = @MultiLang,
           QBIntegration = @Qbintegration,
           MSEmail=@EmailMS,
           QBFirstSync=@QBFirstSync,
           QBServiceItem=@ServiceItem,
           QBServiceItemLabor=@ServiceItemLabor,
           QBServiceItemExp=@ServiceItemExp,
		   YE=@YE,
		   GSTreg = @GSTreg,
		   Lat = @Lat,
		   Lng = @Lng,
		   consultAPI = @consultAPI,
           --,
           --SyncInvoice=@TransferInvoice,
           --SyncTimesheet=@TransferTimesheet
		   ApplyPasswordRules = @ApplyPasswordRules,
		   ApplyPwRulesToFieldUser = @ApplyPwRulesToFieldUser,
		   ApplyPwRulesToOfficeUser = @ApplyPwRulesToOfficeUser,
		   ApplyPwRulesToCustomerUser = @ApplyPwRulesToCustomerUser,
		   ApplyPwReset = @ApplyPwReset,
		   PwResetDays = @PwResetDays,
		   PwResetting = @PwResetting,
		   --EmailAdministrator = @EmailAdministrator,
		   PwResetUserID = @UserID,PR = @Payroll
    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    --IF NOT EXISTS (SELECT 1
    --               FROM   tblGatewayInfo)
    --  BEGIN
    --      INSERT INTO tblGatewayInfo
    --                  (MerchantId,
    --                   LoginId,
    --                   Username,
    --                   Password)
    --      VALUES      ( @MerchantID,
    --                    @LoginID,
    --                    @Username,
    --                    @Password )
    --  END
    --ELSE
    --  BEGIN
    --      UPDATE tblGatewayInfo
    --      SET    MerchantId = @MerchantID,
    --             LoginId = @LoginID,
    --             Username = @Username,
    --             Password = @Password
    --  END
      
    --  IF @@ERROR <> 0
    --   AND @@TRANCOUNT > 0
    --  BEGIN
    --      RAISERROR ('Error Occured',16,1)

    --      ROLLBACK TRANSACTION

    --      RETURN
    --  END

    COMMIT TRANSACTION

