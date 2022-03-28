CREATE PROCEDURE [dbo].[spLoginAuthorization] @UserName NVARCHAR(50),
                                             @Password VARCHAR(50),
                                             @DbName   VARCHAR(50),
                                             @DBType   VARCHAR(50)=''
AS
    DECLARE @DBNameSys VARCHAR(50)
    DECLARE @Query VARCHAR(max)
    DECLARE @StatusId INT = 0
    DECLARE @CountUserTable INT

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

	-- Check if user account is locked
	--DECLARE @IsLocked bit = 0
	DECLARE @PwResetDays int
	DECLARE @ApplyPwPolicy bit--, @ApplyToOfficeUser bit, @ApplyToFieldUser bit, @ApplyToCustomerUser bit
	DECLARE @UserType smallint
	DECLARE @UserAppliedPwPolicy bit
	--DECLARE @LastLoginDate DateTime
	DECLARE @LoginFailedAttempts int 
	DECLARE @FirstName varchar(50)
	DECLARE @LastName varchar(50)
	DECLARE @PwResetUserName varchar(50)
	DECLARE @IsResetPwByResetDays bit
	DECLARE @ApplyPwResetDays bit
	DECLARE @LastUpdatePasswordDate DateTime


	--SELECT TOP 1 @ApplyPwPolicy=ApplyPasswordRules
	--	, @ApplyToOfficeUser=@ApplyToOfficeUser
	--	, @ApplyToFieldUser=ApplyPwRulesToFieldUser 
	--	, @ApplyToCustomerUser = ApplyPwRulesToCustomerUser
	--FROM Control 
	IF EXISTS (SELECT u.ID
                   FROM   tblUser u
                   WHERE  fUser = @UserName)
	BEGIN
		SELECT @UserType=e.Field
			--, @LastLoginDate = ISNULL(u.LastLoginDate, GETDATE())
			, @LastUpdatePasswordDate = ISNULL(u.LastUpdatePasswordDate, GETDATE())
			, @LoginFailedAttempts = ISNULL(u.LoginFailedAttempts, 0) 
			, @FirstName = e.fFirst
			, @LastName = e.Last
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
			--, @LastLoginDate = ISNULL(u.LastLoginDate, GETDATE())
			, @LastUpdatePasswordDate = ISNULL(u.LastUpdatePasswordDate, GETDATE())
			, @LoginFailedAttempts = ISNULL(u.LoginFailedAttempts, 0) 
			, @FirstName = r.Name
			, @LastName = r.Name
		FROM Owner u 
		INNER JOIN Rol r ON r.ID = u.Rol
		END
	END

	SELECT TOP 1 @UserAppliedPwPolicy = 
		CASE WHEN @UserType = 0 THEN ApplyPwRulesToOfficeUser
			WHEN @UserType = 1 THEN ApplyPwRulesToFieldUser
			WHEN @UserType = 2 THEN ApplyPwRulesToCustomerUser
		END,
		@ApplyPwPolicy = ApplyPasswordRules,
		@PwResetDays = PwResetDays,
		@ApplyPwResetDays = ApplyPwReset,
		@IsResetPwByResetDays = 
			CASE WHEN ApplyPwReset > 0 AND ISNULL(PwResetDays,0) > 0 AND DATEADD(DD,PwResetDays,@LastUpdatePasswordDate) < GETDATE() THEN 1
			ELSE 0 END,
		@PwResetUserName = ISNULL(u.fUser,'')
	FROM Control c left join tblUser u ON c.PwResetUserID = u.ID


	IF (ISNULL(@UserAppliedPwPolicy, 0) = 1 AND ISNULL(@ApplyPwPolicy, 0) = 1)-- AND ISNULL(@PwResetDays,0) > 0)
	BEGIN
		IF (@LoginFailedAttempts >=3 AND @PwResetUserName != @UserName)-- OR (ISNULL(@PwResetDays,0) > 0 AND DATEADD(DD,@PwResetDays,@LastLoginDate) < GETDATE()))-- SET @IsLocked = 1
		--IF(@IsLocked = 0)
		BEGIN
			RAISERROR ('User is locked',16,1)
			RETURN
		END

		--IF (ISNULL(@PwResetDays,0) > 0 AND DATEADD(DD,@PwResetDays,@LastLoginDate) < GETDATE())
		--BEGIN
		--	RAISERROR ('User is locked',16,1)
		--	RETURN
		--END

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
                   UNION
                   SELECT 1
                   FROM   tblLocationRole
                   WHERE  Username = @UserName
                          --AND Password = @Password)
						  AND Convert(varbinary, Password) = Convert(varbinary,@Password)
					)
      BEGIN
		  -- Update login failed attempts: Login from MOM
		  UPDATE tblUser SET LoginFailedAttempts = ISNULL(LoginFailedAttempts, 0) + 1 WHERE fUser = @UserName
		  -- TODO: need to update in case login from other system

          RAISERROR('Invalid Password',16,1)

          RETURN
      END
	END
	ELSE
	BEGIN
		IF NOT EXISTS (SELECT u.ID
					FROM   tblUser u
					WHERE  u.fUser = @UserName
                        AND u.Password = @Password
					UNION
					SELECT o.ID
					FROM   Owner o
					WHERE  o.fLogin = @UserName
						AND o.Password = @Password
					UNION
					SELECT 1
					FROM   tblLocationRole
					WHERE  Username = @UserName
                        AND Password = @Password
					)
		BEGIN
			-- Update login failed attempts: Login from MOM
			--UPDATE tblUser SET LoginFailedAttempts = ISNULL(LoginFailedAttempts, 0) + 1 WHERE fUser = @UserName
			-- TODO: need to update in case login from other system

			RAISERROR('Invalid Password',16,1)

			RETURN
		END
	END

    

    IF NOT EXISTS (SELECT Count(1)
                   FROM   tblUser u
                   WHERE  fUser = @UserName
                          AND Password = @Password
						  --AND Convert(varbinary, Password) = Convert(varbinary,@Password)
                          AND u.Status = 0
                   UNION
                   SELECT Count(1)
                   FROM   Owner o
                   WHERE  fLogin = @UserName
                          AND Password = @Password
						  --AND Convert(varbinary, Password) = Convert(varbinary,@Password)
                          AND o.Status = 0
                  --union
                  --select count(1) from tblLocationRole r  
                  --inner join Loc l on l.RoleID = r.ID 
                  --where 
                  --r.Username=@UserName and r.Password=@Password 
                  --and l.Status=0
                  )
      BEGIN
          RAISERROR ('User is not active',16,1)

          RETURN
      END   

	

	--Check the password policy for current password
	
	--IF (ISNULL(@UserAppliedPwPolicy, 0) = 1 AND ISNULL(@ApplyPwPolicy, 0) = 1)
	--BEGIN
	--	DECLARE @IsPassedPwPolicy bit = 1
	--	-- Minimum characters length should be 6 and maximum characters length should be 10 for the password when privacy policy applied.
	--	IF(@IsPassedPwPolicy = 1 AND len(@Password) < 6 AND len(@Password) > 10) SET @IsPassedPwPolicy=0;
	--	-- Should not contain the user's account name or parts of the user's full name that exceed two consecutive characters
	--	IF(@IsPassedPwPolicy = 1 AND ((@Password LIKE '%' + Substring(@UserName,1,3) + '%') 
	--		OR (@Password LIKE '%' + Substring(@FirstName,1,3) + '%')
	--		OR (@Password LIKE '%' + Substring(@LastName,1,3) + '%')
	--		)
	--	) SET @IsPassedPwPolicy=0;

	--	--var containUppercase = false;
 -- --      var containLowercase = false;
 -- --      var containNumerical = false;
 -- --      var containNonAlphabetic = false;
 -- --      var strUpperCase = "ABCDEFGHIJKLMNOPQRSTUVXYZW";
 -- --      var strLowerCase = strUpperCase.ToLower();
 -- --      var strNumberical = "123456789";
 -- --      foreach (var ch in strPassword)
 -- --      {
 -- --          if (strUpperCase.Contains(ch)) containUppercase = true;
 -- --          if (strLowerCase.Contains(ch)) containLowercase = true;
 -- --          if (strNumberical.Contains(ch)) containNumerical = true;
 -- --          if (!char.IsLetterOrDigit(ch)) containNonAlphabetic = true;
 -- --      }
	--	DECLARE @strUpperCase VARCHAR(50) = 'ABCDEFGHIJKLMNOPQRSTUVXYZW';
		
	--	IF (@IsPassedPwPolicy=0)
	--	BEGIN
	--		RAISERROR ('Your password is not passed the password policy.',16,1)
	--		RETURN
	--	END
	--END

	-- Update last login date and login failed attempts when logged in successfully: Login from MOM
	UPDATE tblUser SET LastLoginDate=GETDATE(), LoginFailedAttempts = 0 WHERE fUser = @UserName

    SELECT e.ID,
           e.fFirst,
           e.Last,
           u.ID                                 AS userid,
           Isnull(Dispatch, 'NNNNNN')           AS Dispatch,
           Isnull(Location, 'NNNNNN')           AS Location,
           PO,
           Control,
           UserS,
           'e'                                  AS usertype,
           fUser,
           1                                    AS ticketd,
           1                                    AS ledger,
           0                                    AS custid,
           Isnull(u.sales, 'NNNNNN')            AS sales,
           Isnull(massreview, 0)                AS massreview,
           0                                    AS Roleid,
           ''                                   AS Role,
           Isnull(u.Employee, 'NNNNNN')         AS EmployeeMaint,
           Isnull(u.TC, 'NNNNNN')               AS TC,
           1                                    AS CPEquipment,
           Isnull(u.Chart, 'NNNNNN')            AS Chart,
           Isnull(u.GLAdj, 'NNNNNN')            AS GLAdj,
           Isnull(u.CustomerPayment, 'NNNNNN')  AS CustomerPayment,
           Isnull(u.Deposit, 'NNNNNN')          AS Deposit,
           Isnull(u.Financial, 'NNNNNN')        AS Financial,
           Isnull(u.Vendor, 'NNNNNN')           AS Vendor,
           Isnull(u.Bill, 'NNNNNN')             AS Bill,
           Isnull(u.BillSelect, 'NNNNNN')       AS BillSelect,
           Isnull(u.BillPay, 'NNNNNN')          AS BillPay,
           0                                    AS GroupbyWO,
           0                                    AS openticket,
           Isnull(u.Owner, 'NNNNNN')            AS Owner,
           Isnull(u.Job, 'NNNNNN')              AS Job,
           Isnull(u.Elevator, 'NNNNNN')         AS Elevator,
           Isnull(u.Dispatch, 'NNNNNN')         AS TicketPermission,-- For Ticket Permission use "Dispatch" Field by Ref "TS"
           Isnull(u.ProjectListPermission, 'N') AS ProjectListPermission,
		   Isnull(u.FinancePermission, 'N')     AS FinancePermission,
           Isnull(u.BOMPermission, 'NNNN')      AS BOMPermission,
           Isnull(u.WIPPermission, 'NNNNNN')    AS WIPPermission,
           Isnull(u.MilestonesPermission, 'NNNN') AS MilestonesPermission,
		   Isnull(u.Item, 'NNNNNN')            AS Item,
		   Isnull(u.InvAdj, 'NNNNNN')            AS InvAdj,
		   Isnull(u.Warehouse, 'NNNNNN')            AS Warehouse,
		   Isnull(u.InvSetup, 'NNNNNN')            AS InvSetup,
		   Isnull(u.InvViewer, 'NNNNNN')            AS InvViewer,		   
		   Isnull(u.DocumentPermission, 'NNNN') AS DocumentPermission,
		   Isnull(u.ContactPermission, 'NNNN') AS ContactPermission,
		   Isnull(u.SalesAssigned, 0) as SalesAssigned,
		   Isnull(u.ProjecttempPermission, 'NNNN')   AS ProjecttempPermission,
		   case when isnull(e.fWork,'')='' then 0 else 1  end as usertypeid,
		   isnull(u.BillingCodesPermission, 'NNNN') As BillingCodesPermission, 
           isnull(u.Invoice, 'NNNN') As Invoice,
           isnull(u.PurchasingmodulePermission, 'N')  PurchasingmodulePermission ,
           isnull(u.BillingmodulePermission, 'N')  BillingmodulePermission,
		    isnull(u.AccountPayablemodulePermission, 'N')  AccountPayablemodulePermission,		   
		   RPO,
		   u.JobClose JobClosePermission 
		   , u.JobCompletedPermission CompletedJObPermission 
		   ,u.JobReopenPermission
		   , isnull(u.Proposal, 'NNNNNN') AS Proposal
           ------- Addnew
           , isnull(u.MSAuthorisedDeviceOnly,0) AS MSAuthorisedDeviceOnly
           , isnull(u.PaymentHistoryPermission, 'NNNN')  PaymentHistoryPermission
           , isnull(u.CustomermodulePermission, 'N')  CustomermodulePermission
           , isnull(u.Apply, 'NNNNNN') As Apply,    
            isnull(u.Collection, 'NNNNNN') As Collection,
            isnull(u.bankrec,'NNNNNN')  As bankrec,
            isnull(u.FinancialmodulePermission, 'N') As  FinancialmodulePermission,
            isnull(u.RCmodulePermission, 'N') As  RCmodulePermission,
            isnull(u.ProcessRCPermission, 'NNNNNN') As ProcessRCPermission,    
            isnull(u.ProcessC, 'NNNNNN') As ProcessC,
            isnull(u.ProcessT,'NNNNNN')  As ProcessT,
            isnull(u.RCSafteyTest,'NNNNNN')  As SafetyTestsPermission,
            isnull(u.RCRenewEscalatePermission, 'NNNN') AS RCRenewEscalatePermission,
            isnull(u.SchedulemodulePermission, 'N') AS SchedulemodulePermission,
            isnull(u.Resolve, 'NNNNNN') AS Resolve,
            --isnull(u.TicketPermission, ''NNNNNN'') AS TicketPermission,
            isnull(u.MTimesheet, 'NNNNNN') AS MTimesheet,
            isnull(u.ETimesheet, 'NNNNNN') AS ETimesheet,
            isnull(u.MapR, 'NNNNNN') AS MapR,
            isnull(u.RouteBuilder, 'NNNNNN') AS RouteBuilder,
            isnull(u.MassTimesheetCheck, 'N') AS MassTimesheetCheck,
            isnull(u.CreditHold, 'NNNN') AS CreditHold,
            isnull(u.CreditFlag, 'NNNN') AS CreditFlag,
            --'+CONVERT(VARCHAR(10), @LocCount) + ' AS LocCount,
            isnull(u.salesmanager, 'N') AS salesmanager,
            isnull(u.Sales, 'NNNNNN') AS UserSales,
            isnull(u.ToDo, 0) AS ToDo,
            isnull(u.ToDoC, 0) AS ToDoC,
            isnull(u.FU, 'NNNNNN') AS FU,
            isnull(u.Proposal, 'NNNNNN') AS Proposal,
            isnull(u.Estimates, 'NNNNNN') AS Estimates,
            isnull(u.AwardEstimates, 'NNNNNN') AS AwardEstimates,
            isnull(u.salessetup, 'NNNNNN') AS salessetup,
            isnull(u.PONotification, 'N') AS PONotification,
            isnull(u.writeOff, 'NNNNNN') AS WriteOff,
            u.ProjectModulePermission  ,
            u.InventoryModulePermission ,
            u.JobClose JobClosePermission ,
            u.JobCompletedPermission ,
            u.JobReopenPermission,
            isnull(u.IsProjectManager,0) as IsProjectManager,
            isnull(u.IsAssignedProject,0) as IsAssignedProject
            ,isnull(u.TicketVoidPermission,0) as TicketVoidPermission,
            isnull(u.Employee, 'NNNNNN') AS Employee,  
            isnull(u.PRProcess, 'NNNNNN') AS PRProcess,
            isnull(u.PRRegister, 'NNNNNN') AS PRRegister,  
            isnull(u.PRReport, 'NNNNNN') AS PRReport,  
            isnull(u.PRWage, 'NNNNNN') AS PRWage,  
            isnull(u.PRDeduct, 'NNNNNN') AS PRDeduct  ,
            isnull(u.PR, '0') AS PR,
            isnull(u.ticket,'NNNNNN') as ticket,
            e.ID EmpID,
            ISNULL(u.ApplyUserRolePermission, 0) as ApplyUserRolePermission,
			isnull(u.MassPayrollTicket, 'N') as MassPayrollTicket
    FROM   tblUser u
           LEFT OUTER JOIN Emp e
                        ON u.fUser = e.CallSign
    WHERE  u.fUser = @UserName
           AND Password = @Password
		   --AND Convert(varbinary, Password) = Convert(varbinary,@Password)
           AND u.Status = 0
    UNION
    SELECT 0,
           r.NAME,
           r.NAME,
           o.ID                   AS userid,
           'NNNNNN',
           'NNNNNN',
           'NNNNNN',
           'NNNNNN',
           'NNNNNN',
           'c'                    AS usertype,
           fLogin,
           ticketd,
           ledger,
           o.id                   AS custid,
           'NNNNNN'               AS sales,
           0                      AS massreview,
           0                      AS Roleid,
           ''                     AS Role,
           'NNNNNN'               AS EmployeeMaint,
           'NNNNNN'               AS TC,
           Isnull(CPEquipment, 0) AS CPEquipment,
           'NNNNNN'               AS Chart,
           'NNNNNN'               AS GLAdj,
           'NNNNNN'               AS CustomerPayment,
           'NNNNNN'               AS Deposit,
           'NNNNNN'               AS Financial,
           'NNNNNN'               AS Vendor,
           'NNNNNN'               AS Bill,
           'NNNNNN'               AS BillSelect,
           'NNNNNN'               AS BillPay,
           Isnull(GroupbyWO, 0)   AS GroupbyWO,
           Isnull(openticket, 0)  AS openticket,
           'NNNNNN'               AS Owner,
           'NNNNNN'               AS Job,
           'NNNNNN'               AS Elevator,
           --'YYYYYY'               AS Ticket 
           'NNNNNN'               AS TicketPermission,-- For Ticket Permission
           'N'                    AS ProjectListPermission,
		   'N'                    AS FinancePermission,
           'NNNN'                 AS BOMPermission,
           'NNNNNN'               AS WIPPermission,
           'NNNN'                 AS MilestonesPermission,
		   'NNNNNN'               AS Item,
		   'NNNNNN'               AS InvAdj,
		   'NNNNNN'               AS Warehouse,
		   'NNNNNN'               AS InvSetup,
		    'NNNN' AS DocumentPermission,
		   'NNNN' AS ContactPermission ,
		    'NNNNNN'               AS InvViewer,
			0 as SalesAssigned,
		   'NNNN'  AS ProjecttempPermission,
		   '2' as usertypeid,
		      'NNNN'  BillingCodesPermission, 
            'NNNN' Invoice,
             'N'  PurchasingmodulePermission ,
             'N'  BillingmodulePermission ,
			 'NNNNNN',
			 'N'  AccountPayablemodulePermission,
			 'N' JobClosePermission,
			 'N' CompletedJObPermission
			  , 'N' JobReopenPermission
		    ,'NNNNNN'
            ------- Addnew
            , 0 AS MSAuthorisedDeviceOnly
           , 'NNNN'  PaymentHistoryPermission
           , 'N'  CustomermodulePermission
           , 'NNNNNN' As Apply,    
            'NNNNNN' As Collection,
            'NNNNNN'  As bankrec,
            'N' As  FinancialmodulePermission,
            'N' As  RCmodulePermission,
            'NNNNNN' As ProcessRCPermission,    
            'NNNNNN' As ProcessC,
            'NNNNNN'  As ProcessT,
            'NNNNNN'  As SafetyTestsPermission,
            'NNNN' AS RCRenewEscalatePermission,
            'N' AS SchedulemodulePermission,
            'NNNNNN' AS Resolve,
            --isnull(u.TicketPermission, ''NNNNNN'') AS TicketPermission,
            'NNNNNN' AS MTimesheet,
            'NNNNNN' AS ETimesheet,
            'NNNNNN' AS MapR,
            'NNNNNN' AS RouteBuilder,
            'N' AS MassTimesheetCheck,
            'NNNN' AS CreditHold,
            'NNNN' AS CreditFlag,
            --'+CONVERT(VARCHAR(10), @LocCount) + ' AS LocCount,
            'N' AS salesmanager,
            'NNNNNN' AS UserSales,
            0 AS ToDo,
            0 AS ToDoC,
            'NNNNNN' AS FU,
            'NNNNNN' AS Proposal,
            'NNNNNN' AS Estimates,
            'NNNNNN' AS AwardEstimates,
            'NNNNNN' AS salessetup,
            'N' AS PONotification,
            'NNNNNN' AS WriteOff,
            'N' ProjectModulePermission  ,
            'N' InventoryModulePermission ,
            'N' JobClosePermission ,
            'N' JobCompletedPermission ,
            'N' JobReopenPermission,
            0 as IsProjectManager,
            0 as IsAssignedProject
            ,0 as TicketVoidPermission,
            'NNNNNN' AS Employee,  
            'NNNNNN' AS PRProcess,
            'NNNNNN' AS PRRegister,  
            'NNNNNN' AS PRReport,  
            'NNNNNN' AS PRWage,  
            'NNNNNN' AS PRDeduct  ,
            '0' AS PR,
            'NNNNNN' as ticket,
            0 EmpID,
            0 ApplyUserRolePermission,
			 'N'  as MassPayrollTicket
    FROM   Owner o
           LEFT OUTER JOIN Rol r
                        ON o.Rol = r.ID
    WHERE  o.fLogin = @UserName
           AND Password = @Password
		   --AND Convert(varbinary, Password) = Convert(varbinary,@Password)
           AND o.Status = 0
    UNION
    SELECT 0,
           r.NAME,
           r.NAME,
           o.ID                   AS userid,
           'NNNNNN',
           'NNNNNN',
           'NNNNNN',
           'NNNNNN',
           'NNNNNN',
           'c'                    AS usertype,
           fLogin,
           ticketd,
           ledger,
           o.id                   AS custid,
           'NNNNNN'               AS sales,
           0                      AS massreview,
           lr.ID                  AS Roleid,
           lr.Role,
           'NNNNNN'               AS EmployeeMaint,
           'NNNNNN'               AS TC,
           Isnull(CPEquipment, 0) AS CPEquipment,
           'NNNNNN'               AS Chart,
           'NNNNNN'               AS GLAdj,
           'NNNNNN'               AS CustomerPayment,
           'NNNNNN'               AS Deposit,
           'NNNNNN'               AS Financial,
           'NNNNNN'               AS Vendor,
           'NNNNNN'               AS Bill,
           'NNNNNN'               AS BillSelect,
           'NNNNNN'               AS BillPay,
           Isnull(GroupbyWO, 0)   AS GroupbyWO,
           Isnull(openticket, 0)  AS openticket,
           'NNNNNN'               AS Owner,
           'NNNNNN'               AS Job,
           'NNNNNN'               AS Elevator,
           --'YYYYYY'               AS Ticket 
           'NNNNNN'               AS TicketPermission,-- For Ticket Permission
           'N'                    AS ProjectListPermission,
		   'N'                    AS FinancePermission,
           'NNNN'                 AS BOMPermission,
           'NNNNNN'               AS WIPPermission,
           'NNNN'                 AS MilestonesPermission,
		   'NNNNNN'               AS Item,  
		   'NNNNNN'               AS InvAdj,
		   'NNNNNN'               AS Warehouse,
		   'NNNNNN'               AS InvSetup,
		   'NNNNNN'               AS InvViewer ,
		    'NNNN'  AS DocumentPermission,
		    'NNNN'  AS ContactPermission,
			0 as SalesAssigned ,
			'NNNN' AS ProjecttempPermission,
			'2' as usertypeid,
			  'NNNN'  BillingCodesPermission, 
            'NNNN' Invoice,
             'N'  PurchasingmodulePermission ,
             'N'  BillingmodulePermission ,
			 'NNNNNN',
			  'N'  AccountPayablemodulePermission
			  , 'N' JobClosePermission
			  , 'N' CompletedJObPermission
		   , 'N' JobReopenPermission
		   ,'NNNNNN'
           ------- Addnew
            , 0 AS MSAuthorisedDeviceOnly
           , 'NNNN'  PaymentHistoryPermission
           , 'N'  CustomermodulePermission
           , 'NNNNNN' As Apply,    
            'NNNNNN' As Collection,
            'NNNNNN'  As bankrec,
            'N' As  FinancialmodulePermission,
            'N' As  RCmodulePermission,
            'NNNNNN' As ProcessRCPermission,    
            'NNNNNN' As ProcessC,
            'NNNNNN'  As ProcessT,
            'NNNNNN'  As SafetyTestsPermission,
            'NNNN' AS RCRenewEscalatePermission,
            'N' AS SchedulemodulePermission,
            'NNNNNN' AS Resolve,
            --isnull(u.TicketPermission, ''NNNNNN'') AS TicketPermission,
            'NNNNNN' AS MTimesheet,
            'NNNNNN' AS ETimesheet,
            'NNNNNN' AS MapR,
            'NNNNNN' AS RouteBuilder,
            'N' AS MassTimesheetCheck,
            'NNNN' AS CreditHold,
            'NNNN' AS CreditFlag,
            --'+CONVERT(VARCHAR(10), @LocCount) + ' AS LocCount,
            'N' AS salesmanager,
            'NNNNNN' AS UserSales,
            0 AS ToDo,
            0 AS ToDoC,
            'NNNNNN' AS FU,
            'NNNNNN' AS Proposal,
            'NNNNNN' AS Estimates,
            'NNNNNN' AS AwardEstimates,
            'NNNNNN' AS salessetup,
            'N' AS PONotification,
            'NNNNNN' AS WriteOff,
            'N' ProjectModulePermission  ,
            'N' InventoryModulePermission ,
            'N' JobClosePermission ,
            'N' JobCompletedPermission ,
            'N' JobReopenPermission,
            0 as IsProjectManager,
            0 as IsAssignedProject
            ,0 as TicketVoidPermission,
            'NNNNNN' AS Employee,  
            'NNNNNN' AS PRProcess,
            'NNNNNN' AS PRRegister,  
            'NNNNNN' AS PRReport,  
            'NNNNNN' AS PRWage,  
            'NNNNNN' AS PRDeduct  ,
            '0' AS PR,
            'NNNNNN' as ticket,
            0 EmpID,
            0 ApplyUserRolePermission,
		    'N' as MassPayrollTicket
    FROM   Owner o
           LEFT OUTER JOIN Rol r
                        ON o.Rol = r.ID
           INNER JOIN tblLocationRole lr
                   ON lr.Owner = o.ID
    WHERE  lr.Username = @UserName
           AND lr.Password = @Password
		   --AND Convert(varbinary, lr.Password) = Convert(varbinary,@Password)
    UNION 
    -- Including the permission of user's roles
	SELECT '',
           '',
           '',
           0                                 AS userid,
           Isnull(u.Dispatch, 'NNNNNN')           AS Dispatch,
           Isnull(u.Location, 'NNNNNN')           AS Location,
           u.PO,
           u.Control,
           u.UserS,
           ''                                  AS usertype,
           '' fUser,
           1                                    AS ticketd,
           1                                    AS ledger,
           0                                    AS custid,
           Isnull(u.sales, 'NNNNNN')            AS sales,
           Isnull(u.massreview, 0)                AS massreview,
           0                                    AS Roleid,
           ''                                   AS Role,
           Isnull(u.Employee, 'NNNNNN')         AS EmployeeMaint,
           Isnull(u.TC, 'NNNNNN')               AS TC,
           1                                    AS CPEquipment,
           Isnull(u.Chart, 'NNNNNN')            AS Chart,
           Isnull(u.GLAdj, 'NNNNNN')            AS GLAdj,
           Isnull(u.CustomerPayment, 'NNNNNN')  AS CustomerPayment,
           Isnull(u.Deposit, 'NNNNNN')          AS Deposit,
           Isnull(u.Financial, 'NNNNNN')        AS Financial,
           Isnull(u.Vendor, 'NNNNNN')           AS Vendor,
           Isnull(u.Bill, 'NNNNNN')             AS Bill,
           Isnull(u.BillSelect, 'NNNNNN')       AS BillSelect,
           Isnull(u.BillPay, 'NNNNNN')          AS BillPay,
           0                                    AS GroupbyWO,
           0                                    AS openticket,
           Isnull(u.Owner, 'NNNNNN')            AS Owner,
           Isnull(u.Job, 'NNNNNN')              AS Job,
           Isnull(u.Elevator, 'NNNNNN')         AS Elevator,
           Isnull(u.Dispatch, 'NNNNNN')         AS TicketPermission,-- For Ticket Permission use "Dispatch" Field by Ref "TS"
           Isnull(u.ProjectListPermission, 'N') AS ProjectListPermission,
		   Isnull(u.FinancePermission, 'N')     AS FinancePermission,
           Isnull(u.BOMPermission, 'NNNN')      AS BOMPermission,
           Isnull(u.WIPPermission, 'NNNNNN')    AS WIPPermission,
           Isnull(u.MilestonesPermission, 'NNNN') AS MilestonesPermission,
		   Isnull(u.Item, 'NNNNNN')            AS Item,
		   Isnull(u.InvAdj, 'NNNNNN')            AS InvAdj,
		   Isnull(u.Warehouse, 'NNNNNN')            AS Warehouse,
		   Isnull(u.InvSetup, 'NNNNNN')            AS InvSetup,
		   Isnull(u.InvViewer, 'NNNNNN')            AS InvViewer,		   
		   Isnull(u.DocumentPermission, 'NNNN') AS DocumentPermission,
		   Isnull(u.ContactPermission, 'NNNN') AS ContactPermission,
		   Isnull(u.SalesAssigned, 0) as SalesAssigned,
		   Isnull(u.ProjecttempPermission, 'NNNN')   AS ProjecttempPermission,
		   0 as usertypeid,
		   isnull(u.BillingCodesPermission, 'NNNN') As BillingCodesPermission, 
           isnull(u.Invoice, 'NNNN') As Invoice,
           isnull(u.PurchasingmodulePermission, 'N')  PurchasingmodulePermission ,
           isnull(u.BillingmodulePermission, 'N')  BillingmodulePermission,
		    isnull(u.AccountPayablemodulePermission, 'N')  AccountPayablemodulePermission,		   
		   u.RPO,
		   u.JobClose JobClosePermission 
		   , u.JobCompletedPermission CompletedJObPermission 
		   ,u.JobReopenPermission
		   , isnull(u.Proposal, 'NNNNNN') AS Proposal
           ------- Addnew
           , 0 AS MSAuthorisedDeviceOnly
           , isnull(u.PaymentHistoryPermission, 'NNNN')  PaymentHistoryPermission
           , isnull(u.CustomermodulePermission, 'N')  CustomermodulePermission
           , isnull(u.Apply, 'NNNNNN') As Apply,    
            isnull(u.Collection, 'NNNNNN') As Collection,
            isnull(u.bankrec,'NNNNNN')  As bankrec,
            isnull(u.FinancialmodulePermission, 'N') As  FinancialmodulePermission,
            isnull(u.RCmodulePermission, 'N') As  RCmodulePermission,
            isnull(u.ProcessRCPermission, 'NNNNNN') As ProcessRCPermission,    
            isnull(u.ProcessC, 'NNNNNN') As ProcessC,
            isnull(u.ProcessT,'NNNNNN')  As ProcessT,
            isnull(u.RCSafteyTest,'NNNNNN')  As SafetyTestsPermission,
            isnull(u.RCRenewEscalatePermission, 'NNNN') AS RCRenewEscalatePermission,
            isnull(u.SchedulemodulePermission, 'N') AS SchedulemodulePermission,
            isnull(u.Resolve, 'NNNNNN') AS Resolve,
            --isnull(u.TicketPermission, ''NNNNNN'') AS TicketPermission,
            isnull(u.MTimesheet, 'NNNNNN') AS MTimesheet,
            isnull(u.ETimesheet, 'NNNNNN') AS ETimesheet,
            isnull(u.MapR, 'NNNNNN') AS MapR,
            isnull(u.RouteBuilder, 'NNNNNN') AS RouteBuilder,
            isnull(u.MassTimesheetCheck, 'N') AS MassTimesheetCheck,
            isnull(u.CreditHold, 'NNNN') AS CreditHold,
            isnull(u.CreditFlag, 'NNNN') AS CreditFlag,
            --'+CONVERT(VARCHAR(10), @LocCount) + ' AS LocCount,
            isnull(u.salesmanager, 'N') AS salesmanager,
            isnull(u.Sales, 'NNNNNN') AS UserSales,
            isnull(u.ToDo, 0) AS ToDo,
            isnull(u.ToDoC, 0) AS ToDoC,
            isnull(u.FU, 'NNNNNN') AS FU,
            isnull(u.Proposal, 'NNNNNN') AS Proposal,
            isnull(u.Estimates, 'NNNNNN') AS Estimates,
            isnull(u.AwardEstimates, 'NNNNNN') AS AwardEstimates,
            isnull(u.salessetup, 'NNNNNN') AS salessetup,
            isnull(u.PONotification, 'N') AS PONotification,
            isnull(u.writeOff, 'NNNNNN') AS WriteOff,
            u.ProjectModulePermission  ,
            u.InventoryModulePermission ,
            u.JobClose JobClosePermission ,
            u.JobCompletedPermission ,
            u.JobReopenPermission,
            isnull(u.IsProjectManager,0) as IsProjectManager,
            isnull(u.IsAssignedProject,0) as IsAssignedProject
            ,isnull(u.TicketVoidPermission,0) as TicketVoidPermission,
            isnull(u.Employee, 'NNNNNN') AS Employee,  
            isnull(u.PRProcess, 'NNNNNN') AS PRProcess,
            isnull(u.PRRegister, 'NNNNNN') AS PRRegister,  
            isnull(u.PRReport, 'NNNNNN') AS PRReport,  
            isnull(u.PRWage, 'NNNNNN') AS PRWage,  
            isnull(u.PRDeduct, 'NNNNNN') AS PRDeduct  ,
            isnull(u.PR, '0') AS PR,
            isnull(u.ticket,'NNNNNN') as ticket,
            0 EmpID,
            ISNULL(ur.ApplyUserRolePermission,0) ApplyUserRolePermission,
			isnull(ur.MassPayrollTicket, 'N') as MassPayrollTicket
    FROM   tblRole u
           inner JOIN tblUserRole e
                        ON u.id = e.RoleId
		   inner join tblUser ur on ur.id = e.UserId
    WHERE  ur.fUser = @UserName
           AND u.Status = 0
	

    SELECT ISNULL(@UserAppliedPwPolicy, 0) UserApplyPwRules
		, ISNULL(@ApplyPwPolicy, 0) ApplyPasswordRules
		, ISNULL(@IsResetPwByResetDays, 0) IsResetPwByResetDays