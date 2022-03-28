CREATE PROCEDURE [dbo].[spAddUser] 
     @UserName NVARCHAR(50)
	,@Password NVARCHAR(50)
	,@PDA TINYINT
	,@Field SMALLINT
	,@status SMALLINT
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
	,@DtHired DATETIME
	,@DtFired DATETIME
	,@CreateTicket CHAR(1)
	,@WorkDate CHAR(1)
	,@LocationRemarks CHAR(1)
	,@ServiceHist CHAR(1)
	,@PurchaseOrd CHAR(1)
	,@Expenses CHAR(1)
	,@ProgFunctions CHAR(1)
	,@AccessUser CHAR(1)
	,@Remarks VARCHAR(8000)
	,@Mapping INT
	,@Schedule INT
	,@DeviceID VARCHAR(100)
	,@Pager VARCHAR(100)
	,@Super VARCHAR(50)
	,@salesp INT
	,@str NVARCHAR(400)
	,@userlicID INT
	,@Lang VARCHAR(25)
	,@MerchantInfoId INT
	,@DefaultWorker SMALLINT
	,@Dispatch CHAR(1)
	,@SalesMgr SMALLINT
	,@MassReview SMALLINT
	,@MSMUser VARCHAR(50)
	,@MSMPass VARCHAR(50)
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
	,@HourlyRate NUMERIC(30, 2)
	,@EmployeeMainten SMALLINT
	,@TimestamFixed SMALLINT
	,@PayMethod SMALLINT
	,@PHours NUMERIC(30, 2)
	,@Salary NUMERIC(30, 2)
	,@Department VARCHAR(100)
	,@Ref VARCHAR(15)
	,@PayPeriod SMALLINT
	,@mileagerate NUMERIC(30, 2)
	,@addequip SMALLINT = 1
	,@editequip SMALLINT = 1
	,@FChart SMALLINT
	,@addFChart SMALLINT
	,@editFChart SMALLINT
	,@viewFChart SMALLINT
	,@FGLAdj SMALLINT
	,@addFGLAdj SMALLINT
	,@editFGLAdj SMALLINT
	,@viewFGLAdj SMALLINT
	,@FDeposit SMALLINT
	,@AddDeposit SMALLINT
	,@EditDeposit SMALLINT
	,@ViewDeposit SMALLINT
	,@FCustomerPayment SMALLINT
	,@AddCustomerPayment SMALLINT
	,@EditCustomerPayment SMALLINT
	,@ViewCustomerPayment SMALLINT
	,@FStatement SMALLINT
	,@StartDate DATETIME
	,@EndDate DATETIME
    ,@APVendor VARCHAR(4) = 'NNNN'
	,@APBill VARCHAR(4) = 'NNNN'	
	,@APBillPay VARCHAR(4) = 'NNNN' 
	,@APBillSelect   SMALLINT
	,@WageItems TBLTYPEWAGEITEMS readonly
	,@CustomerPermissions VARCHAR(4) = 'NNNN'
	,@LocationrPermissions VARCHAR(4) = 'NNNN'
	,@ProjectPermissions VARCHAR(10) = 'NNNNNN'
	,@DeleteEquip SMALLINT = 1
	,@ViewEquip SMALLINT = 1
	,@MSAuthorisedDeviceOnly INT = 0
	,@TicketDelete VARCHAR(1) = 'N'
	,@ProjectListPermission NCHAR(1) = 'N'
	,@FinancePermission NCHAR(1) = 'N'
	,@BOMPermission NCHAR(4) = 'NNNN'
	,@WIPPermission NCHAR(6) = 'NNNNNN'
	,@MilestonesPermission NCHAR(4) = 'NNNN'
	,@InventoryItemPermissions VARCHAR(10) = 'NNNNNN'
	,@InventoryAdjustmentPermissions VARCHAR(10) = 'NNNNNN'
	,@InventoryWarehousePermissions VARCHAR(10) = 'NNNNNN'
	,@InventorysetupPermissions VARCHAR(10) = 'NNNNNN'
	,@InventoryFinancePermissions VARCHAR(10) = 'NNNNNN'
	,@DocumentPermission NCHAR(4) = 'NNNN'
	,@ContactPermission NCHAR(4) = 'NNNN'
	,@SalesAssigned BIT = 0
	,@ProjecttempPermission NCHAR(4) = 'NNNN'
	,@NotificationOnAddOpportunity BIT = 0
	,@VendorsPermission NCHAR(4) = 'NNNN'
	,@POLimit NUMERIC(30, 2)
	,@POApprove SMALLINT = 0
	,@POApproveAmt SMALLINT = 0
	,@Lng NVARCHAR(100)=NULL
	,@Lat NVARCHAR(100)=NULL
	,@Country NVARCHAR(50)=NULL
	,@authdevID NVARCHAR(100)=NULL
	,@EmNum NVARCHAR(100)=NULL
	,@EmName NVARCHAR(100)=NULL
	,@Title NVARCHAR(100)=NULL
	,@InvoicePermission VARCHAR(4) = 'NNNN'
	,@BillingCodesPermission VARCHAR(4) = 'NNNN'
	,@POPermission VARCHAR(4) = 'NNNN'
	,@PurchasingmodulePermission CHAR(1) = 'N' 
	,@BillingmodulePermission CHAR(1) = 'N'
	,@RPOPermission VARCHAR(4) = 'NNNN'
	,@TakeASentEmailCopy BIT,
	 @AccountPayablemodulePermission CHAR(1) = 'N' 
	,@PaymentHistoryPermission VARCHAR(4) = 'NNNN'
	,@CustomermodulePermission CHAR(1) = 'N' 
	,@ApplyPermissions  VARCHAR(4) = 'NNNN'
	,@DepositPermissions  VARCHAR(4) = 'NNNN'
	,@CollectionsPermissions  VARCHAR(4) = 'NNNN'
	,@Financialmodule CHAR(1) = 'N' 
	,@ChartPermissions  VARCHAR(4) = 'NNNN'
	,@JournalEntryPermissions  VARCHAR(4) = 'NNNN'
	,@BankReconciliationPermissions  VARCHAR(4) = 'NNNN'
	,@RCmodulePermission CHAR(1) = 'N' 
	,@ProcessRCPermission  VARCHAR(4) = 'NNNN'
	,@ProcessC  VARCHAR(4) = 'NNNN'
	,@ProcessT  VARCHAR(4) = 'NNNN'
	,@SafetyTestsPermission  VARCHAR(4) = 'NNNN'		
	,@RCRenewEscalatePermission VARCHAR(4) = 'NNNN'
	,@Schedulemodule CHAR(1) = 'N' 
	,@ScheduleBoardPermission VARCHAR(6) = 'NNNNN'
	,@TicketPermission VARCHAR(6) = 'NNNNN'
	,@TicketResolvedPermission VARCHAR(6) = 'NNNNN'
	,@MTimesheetPermission VARCHAR(6) = 'NNNNN'
    ,@ETimesheetPermission VARCHAR(6) = 'NNNNN'
    ,@MapRPermission VARCHAR(6) = 'NNNNN'
    ,@RouteBuilderPermission VARCHAR(6) = 'NNNNN'
	,@MassTimesheetCheck CHAR(1) = 'N'
	,@CreditHold  VARCHAR(4) = 'NNNN'
	,@CreditFlag  VARCHAR(4) = 'NNNN'
	,@SSN varchar(11) = NULL
	,@Sex varchar(10) = NULL
	,@DBirth DATETIME = NULL
	,@Race varchar(40) = NULL
	,@SalesPermission VARCHAR(6) = 'NNNNN' 
	,@TasksPermission SMALLINT = 0
	,@CompleteTasksPermission SMALLINT = 0
	,@FollowUpPermission VARCHAR(6) = 'NNNNN'
	,@ProposalPermission VARCHAR(6) = 'NNNNN'
    ,@EstimatePermission VARCHAR(6) = 'NNNNN'
    ,@ConvertEstimatePermission VARCHAR(6) = 'NNNNN'
    ,@SalesSetupPermission VARCHAR(6) = 'NNNNN'	
	,@PONotification CHAR(1) = 'N'	
	,@JobClosePermission  CHAR(6) = 'NNNNN'	
	,@InventoryModulePermission CHAR(1) = 'N'	
	,@ProjectModulePermission   CHAR(1) = 'N'	
	,@JobCompletedPermission    Char(1) ='N'
	,@JobReopenPermission    Char(1) ='N'
	,@writeOff   VARCHAR(6) = 'NNNNN'
	,@IsProjectManager BIT
	,@IsAssignedProject BIT
	,@IsReCalculateLaborExpense BIT
	,@MinAmount NUMERIC(30, 2)
	,@MaxAmount NUMERIC(30, 2)
	,@CreatedBy VARCHAR (50)
	,@TicketVoidPermission  int =0
	,@Employees varchar(6)
	,@PRProcess varchar(6),
	@PRRegister varchar(6),
	@PRReport varchar(6),
	@PRWage varchar(6),
	@PRDeduct varchar(6),
	@PR bit,
	@UserRoleId int,
	@ApplyUserRolePermission smallint,
	@MassPayrollTicket varchar(1),
	@ViolationPermission  VARCHAR(4) = 'NNNN'	
AS
DECLARE @Rol INT
DECLARE @work INT
DECLARE @Ticket VARCHAR(10)
DECLARE @empid INT
DECLARE @userid INT
DECLARE @sales VARCHAR(10)
DECLARE @Employee VARCHAR(10)
DECLARE @Elevator VARCHAR(10)
DECLARE @Chart VARCHAR(10) = 'NNNNNN'
DECLARE @GLAdj VARCHAR(10) = 'NNNNNN'
DECLARE @Deposit VARCHAR(10) = 'NNNNNN'
DECLARE @CustomerPayment VARCHAR(10) = 'NNNNNN'
DECLARE @FinanceState VARCHAR(10) = 'NNNNNN'
DECLARE @Vendor VARCHAR(6) = 'NNNNNN'
DECLARE @Bill VARCHAR(10) = 'NNNNNN'
DECLARE @BillSelect VARCHAR(10) = 'NNNNNN'
DECLARE @BillPay VARCHAR(10) = 'NNNNNN'
DECLARE @Type INT = 5
DECLARE @TC VARCHAR(6)
DECLARE @SalesManager VARCHAR(1)
-- For logs
Declare @Screen varchar(100) = 'User';
Declare @RefId int;
Declare @empPR bit

SELECT @empPR =ISNULL(PR, 0) FROM Control 

IF ([dbo].Checkwagesisrequired(0, 0) = 1)
BEGIN
	IF NOT EXISTS (
			SELECT 1
			FROM @WageItems
			)
	BEGIN
		RAISERROR (
				'Please add atleast a single wage item!'
				,16
				,1
				)

		RETURN
	END
END

 

IF (@FStatement = 1)
BEGIN
	SET @FinanceState = Substring(@FinanceState, 1, 5) + 'Y'
END
ELSE
BEGIN
	SET @FinanceState = Substring(@FinanceState, 1, 5) + 'N'
END

IF (@FChart = 1)
BEGIN
	SET @Chart = 'YYYYYY'
END
ELSE
BEGIN
	SET @Chart = 'NNNNNN'
END

IF (@addFChart = 1)
BEGIN
	SET @Chart = 'Y' + Substring(@Chart, 2, 5)
END

IF (@editFChart = 1)
BEGIN
	SET @Chart = Substring(@Chart, 1, 1) + 'Y' + Substring(@Chart, 3, 4)
END

IF (@viewFChart = 1)
BEGIN
	SET @Chart = Substring(@Chart, 1, 3) + 'Y' + Substring(@Chart, 5, 2)
END

IF (@FGLAdj = 1)
BEGIN
	SET @GLAdj = 'YYYYYY'
END
ELSE
BEGIN
	SET @GLAdj = 'NNNNNN'
END

IF (@addFGLAdj = 1)
BEGIN
	SET @GLAdj = 'Y' + Substring(@GLAdj, 2, 5)
END

IF (@editFGLAdj = 1)
BEGIN
	SET @GLAdj = Substring(@GLAdj, 1, 1) + 'Y' + Substring(@GLAdj, 3, 4)
END

IF (@viewFGLAdj = 1)
BEGIN
	SET @GLAdj = Substring(@GLAdj, 1, 3) + 'Y' + Substring(@GLAdj, 5, 2)
END



IF (@FCustomerPayment = 1)
BEGIN
	SET @CustomerPayment = 'YYYYYY'
END
ELSE
BEGIN
	SET @CustomerPayment = 'NNNNNN'
END

IF (@AddCustomerPayment = 1)
BEGIN
	SET @CustomerPayment = 'Y' + Substring(@CustomerPayment, 2, 5)
END

IF (@EditCustomerPayment = 1)
BEGIN
	SET @CustomerPayment = Substring(@CustomerPayment, 1, 1) + 'Y' + Substring(@CustomerPayment, 3, 4)
END

IF (@ViewCustomerPayment = 1)
BEGIN
	SET @CustomerPayment = Substring(@CustomerPayment, 1, 3) + 'Y' + Substring(@CustomerPayment, 5, 2)
END

IF (@addequip = 1)
BEGIN
	SET @Elevator = 'YNNNNN'
END
ELSE
BEGIN
	SET @Elevator = 'NNNNNN'
END

IF (@editequip = 1)
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 1) + 'YNNNN'
END
ELSE
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 1) + 'NNNNN'
END

IF (@DeleteEquip = 1)
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 2) + 'YNNN'
END
ELSE
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 2) + 'NNNN'
END

IF (@ViewEquip = 1)
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 3) + 'YNN'
END
ELSE
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 3) + 'NNN'
END

IF (@SalesMgr = 1)
BEGIN
	--SET @sales = 'YNNNNN'
	SET @SalesManager = 'Y';
END
ELSE
BEGIN
	--SET @sales = 'NNNNNN'
	SET @SalesManager = 'N';
END

-----SET @Ticket=substring(@TicketPermissions,1,4)+'NN';    
IF (@Schedule = 1)
BEGIN
	SET @Ticket = 'YYYYYY'
END
ELSE
BEGIN
	SET @Ticket = 'NYYYYY'
END

IF (@Mapping = 1)
BEGIN
	SET @Ticket = Substring(@Ticket, 1, 1) + 'YYYYY';
END
ELSE
BEGIN
	SET @Ticket = Substring(@Ticket, 1, 1) + 'YYNYY';
END

IF(@empPR =0)
BEGIN
	IF (@EmployeeMainten = 1)
	BEGIN
		SET @Employee = 'NNNYNN'
	END
	ELSE
	BEGIN
		SET @Employee = 'NNNNNN'
	END
END
ELSE
BEGIN
  SET  @Employees = @Employees  
END


IF (@TimestamFixed = 1)
BEGIN
	SET @TC = 'NYNNNN'
END
ELSE
BEGIN
	SET @TC = 'NNNNNN'
END

BEGIN TRANSACTION

IF (@Field <> 2)
BEGIN
	IF NOT EXISTS (
			SELECT 1
			FROM tblUser
			WHERE fUser = @UserName
			--WHERE  msmuser = @MSMUser    
			
			UNION
			
			SELECT 1
			FROM OWNER
			WHERE fLogin = @UserName
			--WHERE  msmuser = @MSMUser    
			
			UNION
			
			SELECT 1
			FROM tblLocationRole
			WHERE Username = @UserName
			)
	BEGIN
		IF (@DefaultWorker = 1)
		BEGIN
			UPDATE tbluser
			SET defaultworker = 0
		END

		INSERT INTO tblUser (
			fUser
			,Password
			,PDA
			,STATUS
			,MassResolvePDATickets
			,ListsAdmin
			,Dispatch
			,Location
			,PO
			,CONTROL
			,UserS
			,UserType
			,Remarks
			,Ticket
			,Lang
			,MerchantInfoID
			,LastUpdateDate
			,DefaultWorker
			,Sales
			,MassReview
			,EmailAccount
			,Employee
			,TC
			,Elevator
			,Chart
			,GLAdj
			,Deposit
			,CustomerPayment
			,Financial
			,Vendor
			,Bill
			,BillPay
			,fStart
			,fEnd
			,Job
			,OWNER
			,MSAuthorisedDeviceOnly
			,ProjectListPermission
			,FinancePermission
			,BOMPermission
			,WIPPermission
			,MilestonesPermission
			,Item
			,InvAdj
			,Warehouse
			,InvSetup
			,InvViewer
			,DocumentPermission
			,ContactPermission
			,SalesAssigned
			,ProjecttempPermission
			,NotificationOnAddOpportunity
			,POLimit
			,POApprove
			,POApproveAmt
			,MinAmount
			,MaxAmount
			,Lng
			,Lat
			,Country
			,Title 
			,Invoice 
			,BillingCodesPermission 
			,PurchasingmodulePermission  
	        ,BillingmodulePermission
			,RPO
			,AccountPayablemodulePermission
			,PaymentHistoryPermission
			,CustomermodulePermission
			,Apply
			,Collection
			,BankRec
			,FinancialmodulePermission
			,RCmodulePermission
			,ProcessRCPermission 
		    ,ProcessC
		    ,ProcessT
			,RCRenewEscalatePermission
			,RCSafteyTest	
		    ,SchedulemodulePermission	
		    ,Resolve
		    ,MTimesheet
		    ,ETimesheet
            ,MapR
		    ,RouteBuilder
			,MassTimesheetCheck
			,CreditHold
			,CreditFlag
			,SalesManager
			,ToDo
			,ToDoC
			,FU
			,Proposal
			,Estimates
			,AwardEstimates
			,SalesSetup
			,PONotification
			,JobClose	
	        ,InventoryModulePermission 	
	        ,ProjectModulePermission  
			,JobCompletedPermission
			,JobReopenPermission
			,WriteOff
			,IsProjectManager
			,IsAssignedProject
			,IsReCalculateLaborExpense
			,TicketVoidPermission
			,PRProcess
			,PRRegister
			,PRReport
			,PRWage
			,PRDeduct
			,PR
			,ApplyUserRolePermission
			,MassPayrollTicket
			,ViolationPermission
			)
	   
		VALUES (
			@UserName
			,@Password
			,@PDA
			,@status
			,0
			,0
			--,Substring(@TicketPermission, 1, 4) +'NN'
			--,Substring(@TicketPermission, 1, 4) +'N' + @Dispatch
			,@TicketPermission
			,@LocationrPermissions + 'NN'
			,@POPermission+'NN'
			,@ProgFunctions + 'NNNNN'
			,@AccessUser + 'NNNNN'
			,@Field
			,@Remarks
			--,Substring(@ScheduleBoardPermission, 1, 4) +'NN'
			,Substring(@ScheduleBoardPermission, 1, 4) + Substring(@TicketPermission, 5, 2)
			,@Lang
			,@MerchantInfoId
			,Getdate()
			,@DefaultWorker
			,@SalesPermission
			,@MassReview
			,@EmailAccount
			,@Employee
			,@TC
			,@Elevator
			,@ChartPermissions+ 'NN'
			,@JournalEntryPermissions + 'NN'
			,@DepositPermissions + 'NN'
			,@CustomerPayment
			,@FinanceState
			,@APVendor  + 'NN'
			,@APBill  + 'NN'
			,@APBillPay  + 'NN'
			,@StartDate
			,@EndDate
			,@ProjectPermissions + 'NN'
			,@CustomerPermissions + 'NN'
			,@MSAuthorisedDeviceOnly
			,@ProjectListPermission
			,@FinancePermission
			,@BOMPermission
			,@WIPPermission
			,@MilestonesPermission
			,@InventoryItemPermissions + 'NN'
			,@InventoryAdjustmentPermissions + 'NN'
			,@InventoryWarehousePermissions + 'NN'
			,@InventorysetupPermissions + 'NN'
			,@InventoryFinancePermissions + 'NN'
			,@DocumentPermission
			,@ContactPermission
			,@SalesAssigned
			,@ProjecttempPermission
			,@NotificationOnAddOpportunity
			,@POLimit
			,@POApprove
			,@POApproveAmt
			,@MinAmount
			,@MaxAmount
			,@Lng
			,@Lat
			,@Country
			,@Title 
			,@InvoicePermission+'NN' 
			,@BillingCodesPermission
			,@PurchasingmodulePermission 
	        ,@BillingmodulePermission
			,@RPOPermission+'NN'
			,@AccountPayablemodulePermission
			,@PaymentHistoryPermission
			,@CustomermodulePermission
			,@ApplyPermissions + 'NN'
			,@CollectionsPermissions + 'NN'
			,@BankReconciliationPermissions + 'NN'
			,@Financialmodule
			,@RCmodulePermission
			,@ProcessRCPermission+'NN'
		    ,@ProcessC
		    ,@ProcessT
			,@RCRenewEscalatePermission
			,@SafetyTestsPermission
			,@Schedulemodule
			,@TicketResolvedPermission
			,@MTimesheetPermission 
			,@ETimesheetPermission
            ,@MapRPermission
			,@RouteBuilderPermission
		    ,@MassTimesheetCheck
			,@CreditHold
			,@CreditFlag
			,@SalesManager
			,@TasksPermission
			,@CompleteTasksPermission
			,@FollowUpPermission
			,@ProposalPermission
			,@EstimatePermission
			,@ConvertEstimatePermission
			,@SalesSetupPermission 
			,@PONotification 
			,@JobClosePermission	
	        ,@InventoryModulePermission 	
	        ,@ProjectModulePermission 
			,@JobCompletedPermission
			,@JobReopenPermission
			,@WriteOff + 'NNNNN'
			,@IsProjectManager
			,@IsAssignedProject
			,@IsReCalculateLaborExpense
			,@TicketVoidPermission
			,@PRProcess
			,@PRRegister
			,@PRReport
			,@PRWage 
			,@PRDeduct
			,@PR
			,0
			,@MassPayrollTicket
			,@ViolationPermission
			) 
		SET @userid = Scope_identity()

		-- Update or delete role of user
		IF (ISNULL(@UserRoleId, 0) != 0)
		BEGIN
			INSERT INTO tblUserRole (RoleId, UserId, UpdatedBy, UpdatedDate) VALUES (@UserRoleId, @userid, @CreatedBy, GETDATE())
			UPDATE tblUser SET ApplyUserRolePermission = @ApplyUserRolePermission WHERE ID = @userid
		END
	END
	ELSE
	BEGIN
		RAISERROR (
				'Username already exists, please use different username!'
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
	,Remarks
		,[Type]
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
	 ,@Remarks    
	,@Type
	,@EmName
	,@EmNum
	)

SET @Rol = Scope_identity()

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

--IF( @Field = 2 )    
--  BEGIN    
--      IF NOT EXISTS (    
--      SELECT 1    
--                     FROM   Owner    
--                     WHERE  fLogin = @UserName    
--                     UNION    
--                     SELECT 1    
--                     FROM   tblUser    
--                     WHERE  fUser = @UserName    
--                     )    
--        BEGIN    
--            INSERT INTO Owner    
--                        (fLogin,    
--                         Password,    
--                         Status,    
--                         TicketO,    
--                         TicketD,    
--                         Internet,    
--                         Rol)    
--            VALUES      ( @UserName,    
--                          @Password,    
--                          @status,    
--                          @Schedule,    
--                          @Mapping,    
--                          1,    
--                          @Rol )    
--        END    
--      ELSE    
--        BEGIN    
--            RAISERROR ('Username already exixts, please use different username!',16,1)    
--            ROLLBACK TRANSACTION    
--            RETURN    
--        END    
--  END    
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

IF (@Field = 1)
BEGIN
IF(@Super<>'-1')
BEGIN
INSERT INTO tblWork (
		fDesc
		,Type
		,STATUS
		,GeoLock
		,Super
		,DBoard
		,HourlyRate
		)
	VALUES (
		@UserName
		,0
		,@status
		,0
		,@Super
		,@Schedule
		,@HourlyRate
		)

	SET @work = Scope_identity()


	

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

	INSERT INTO Route (
		NAME
		,Mech
		,Loc
		,Elev
		,Hour
		,Amount
		,Symbol
		,EN
		)
	VALUES (
		@UserName
		,@work
		,0
		,0
		,0
		,0
		,1
		,1
		)

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

	EXEC Spcreatepda_userid @work

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
ELSE
BEGIN
	SET @work = NULL
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

IF (@Field <> 2)
BEGIN
	IF NOT EXISTS (
			SELECT 1
			FROM emp
			WHERE DeviceID = @DeviceID
				AND @DeviceID <> ''
			)
	BEGIN
		INSERT INTO Emp (
			Field
			,STATUS
			,fFirst
			,Middle
			,Last
			,DHired
			,DFired
			,CallSign
			,Rol
			,fWork
			,Sales
			,InUse
			,NAME
			,DeviceID
			,Pager
			,PMethod
			,PHour
			,Salary
			,PFixed
			,Ref
			,PayPeriod
			,MileageRate
			,MSDeviceId
			,SSN
			,Sex
			,DBirth
			,Race
			,Title
			)
		VALUES (
			@Field
			,@status
			,@FName
			,@MName
			,@LName
			,@DtHired
			,@DtFired
			,@UserName
			,@Rol
			,@work
			,@salesp
			,0
			,@FName + ', ' + @LName
			,@DeviceID
			,@Pager
			,CASE @PayMethod
				WHEN 2
					THEN 1
				ELSE @PayMethod
				END
			,CASE @PayMethod
				WHEN 1
					THEN 0
				ELSE @PHours
				END
			,@Salary
			,CASE @PayMethod
				WHEN 2
					THEN 0
				ELSE 1
				END
			,@Ref
			,@PayPeriod
			,@mileagerate
			,@authdevID
			,@SSN
			,@Sex
			,@DBirth
			,@Race
			,@Title
			)

		SET @empid = Scope_identity()

		INSERT INTO tblJoinEmpDepartment (
			Emp
			,Department
			)
		SELECT @empid
			,*
		FROM dbo.Split(@Department, ',')

		IF EXISTS (
				SELECT TOP 1 1
				FROM @WageItems
				)
		BEGIN
			INSERT INTO [dbo].[PRWageItem] (
				[Wage]
				,[Emp]
				,[Reg]
				,[OT]
				,[DT]
				,[TT]
				,[NT]
				,[CReg]
				,[COT]
				,[CDT]
				,[CTT]
				,[CNT]
				,[Status]
				,[InUse]
				,[YTD]
				,[YTDH]
				,[OYTD]
				,[OYTDH]
				,[DYTD]
				,[DYTDH]
				,[TYTD]
				,[TYTDH]
				,[NYTD]
				,[NYTDH]
				,[Sick]
				,[VacR]
				,[FIT]
				,[FICA]
				,[MEDI]
				,[FUTA]
				,[SIT]
				,[Vac]
				,[WC]
				,[Uni]
				)
			SELECT Wage
				,@empid
				,Reg
				,OT
				,DT
				,TT
				,NT
				,CReg
				,COT
				,CDT
				,CTT
				,CNT
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,''
				,0
				,0
				,0
				,0
				,0
				,0
				,0
				,0
			FROM @WageItems

			UPDATE pitem
			SET GL = p.GL
				,FIT = p.FIT
				,FICA = p.FICA
				,MEDI = p.MEDI
				,FUTA = p.FUTA
				,SIT = p.SIT
				,Vac = p.Vac
				,WC = p.WC
				,Uni = p.Uni
			FROM PRWageItem pitem
			INNER JOIN PRWage p ON pitem.Wage = p.ID
			WHERE pitem.Emp = @EmpID
		END
	END
	ELSE
	BEGIN
		RAISERROR (
				'Device ID already exixts, please use different Device ID!'
				,16
				,1
				)

		ROLLBACK TRANSACTION

		RETURN
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

IF (@DefaultWorker = 1)
BEGIN
	UPDATE Loc
	SET Route = (
			SELECT TOP 1 id
			FROM route
			WHERE NAME = @UserName
			)
	WHERE Route IS NULL

	UPDATE Loc
	SET Terr = (
			SELECT TOP 1 id
			FROM Terr
			WHERE NAME = @UserName
			)
	WHERE Terr IS NULL
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

   
DECLARE @lid INT

IF @userlicID <> 0
BEGIN
	IF (
			(
				SELECT Count(1)
				FROM MSM2_Admin.dbo.tblUserAuth
				WHERE str = (
						SELECT str
						FROM MSM2_Admin.dbo.tblUserAuth
						WHERE ID = @userlicID
						)
				) = 1
			)
	BEGIN
		INSERT INTO MSM2_Admin.dbo.tbljoinauth (
			userid
			,lid
			,DATE
			,STATUS
			,dbname
			)
		VALUES (
			@userid
			,@userlicID
			,Getdate()
			,0
			,(
				SELECT Db_name()
				)
			)

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

		UPDATE MSM2_Admin.dbo.tblUserAuth
		SET str = @str
			,used = 1
			,dateupdate = Getdate()
		WHERE ID = @userlicID

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

/* Start Logs */
Set @RefId = @userid;
-- First Name
IF(ISNULL(@FName,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'First Name','',@FName
END
-- Middle Name
IF(ISNULL(@MName,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Middle Name','',@MName
END
-- Last Name
IF(ISNULL(@LName,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Last Name','',@LName
END
-- Customer Name
-- Address
IF(ISNULL(@Address,'') != '')
BEGIN 	
	DECLARE @logAddress VARCHAR(1000) = Convert(VARCHAR(1000), ISNULL(@Address,''))
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Address','',@logAddress
END
-- Latitude
IF(ISNULL(@Lat,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Latitude','',@Lat
END
-- Longitude
IF(ISNULL(@Lng,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Longitude','',@Lng
END
-- City
IF(ISNULL(@City,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'City','',@City
END
-- Zip
IF(ISNULL(@Zip,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Zip','',@Zip
END
-- State/Province
IF(ISNULL(@State,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'State/Province','',@State
END
-- Country
IF(ISNULL(@Country,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Country','',@Country
END
-- Username
IF(ISNULL(@UserName,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Username','',@UserName
END
-- Password
-- Status
IF(@status = 0)
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Status','','Active'
END
ELSE
BEGIN
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Status','','Inactive'
END
-- User Type
IF(ISNULL(@Field,0) = 0)
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'User Type','','Office'
END
ELSE
BEGIN
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'User Type','','Field'
END
-- Department
IF(ISNULL(@Department,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Department','',@Department
END
-- Start Date
IF(@StartDate is not null And @StartDate != '')
BEGIN 	
	DECLARE @logStartDate varchar(150)
	SELECT @logStartDate = convert(varchar, @StartDate, 101)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Start Date','',@logStartDate
END
-- End Date
IF(@EndDate is not null And @EndDate != '')
BEGIN 	
	DECLARE @logEndDate varchar(150)
	SELECT @logEndDate = convert(varchar, @EndDate, 101)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'End Date','',@logEndDate
END
-- Project Manager
IF(ISNULL(@IsProjectManager,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Project Manager','','Yes'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Project Manager','','No'
-- Assigned Projects
IF(ISNULL(@IsAssignedProject,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Assigned Projects','','Yes'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Assigned Projects','','No'
-- Recalculate Labor Expense
IF(ISNULL(@IsReCalculateLaborExpense,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Recalculate Labor Expense','','Yes'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Recalculate Labor Expense','','No'
-- Phone 
IF(ISNULL(@Tel,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Phone','',@Tel
END
-- Cell
IF(ISNULL(@Cell,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Cell','',@Cell
END
-- Emergency Contact Name
IF(ISNULL(@EmName,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Emergency Contact Name','',@EmName
END
-- Emergency Number
IF(ISNULL(@EmNum,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Emergency Number','',@EmNum
END
-- Remarks
IF(ISNULL(@Remarks,'') != '')
BEGIN 	
	DECLARE @logRemarks VARCHAR(1000) = Convert(VARCHAR(1000), ISNULL(@Remarks,''))
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Remarks','',@logRemarks
END
-- Email
IF(ISNULL(@Email,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Email','',@Email
END
-- Text Message
IF(ISNULL(@Pager,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Text Message','',@Pager
END
-- Salesperson
IF(ISNULL(@salesp,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Salesperson','','Yes'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Salesperson','','No'
-- Sales Assigned
IF(ISNULL(@SalesAssigned,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Sales Assigned','','Yes'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Sales Assigned','','No'
-- Opportunity Notification
IF(ISNULL(@NotificationOnAddOpportunity,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Opportunity Notification','','Yes'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Opportunity Notification','','No'
-- Email Account
IF(ISNULL(@EmailAccount,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Email Account','','Yes'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Email Account','','No'
-- Incoming Mail Server
IF(ISNULL(@InServer,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Incoming Mail Server','',@InServer
END
-- SSL
IF(ISNULL(@SSL,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'SSL','','Yes'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'SSL','','No'
-- Incoming Port
IF(ISNULL(@InPort,0) != 0)
BEGIN 	
	DECLARE @logInPort Varchar(10) = Convert(varchar(10),@InPort)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Incoming Port','',@logInPort
END
-- Incoming Email Username
IF(ISNULL(@InUsername,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Incoming Email Username','',@InUsername
END
-- Incoming Password
-- Bcc Email
IF(ISNULL(@BccEmail,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Bcc Email','',@BccEmail
END
-- Outgoing Mail Server
IF(ISNULL(@OutServer,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Outgoing Mail Server','',@OutServer
END
-- Outgoing Port
IF(ISNULL(@OutPort,0) != 0)
BEGIN 	
	DECLARE @logOutPort Varchar(10) = Convert(varchar(10),@OutPort)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Outgoing Port','',@logOutPort
END
-- Outgoing Email Username
IF(ISNULL(@OutUsername,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Outgoing Email Username','',@OutUsername
END
-- Outgoing Password
-- Send Copy to "Sent Items"
IF(ISNULL(@TakeASentEmailCopy,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Send Copy to "Sent Items"','','Yes'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Send Copy to "Sent Items"','','No'

/* Permissions */
-- Permissions - PO Limit
IF(ISNULL(@POLimit,0) != 0)
BEGIN 	
	DECLARE @logPOLimit Varchar(40) = Convert(varchar(40),@POLimit)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - PO Limit','',@logPOLimit
END
-- Permissions - Approve PO
IF(ISNULL(@POApprove,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Approve PO','','Yes'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Approve PO','','No'
-- Permissions - Approve PO Amount, Min Amount, Max Amount
DECLARE @logMinAmount Varchar(40) = Convert(varchar(40),@MinAmount)
DECLARE @logMaxAmount Varchar(40) = Convert(varchar(40),@MaxAmount)
IF(ISNULL(@POApproveAmt,-1) = 0)
BEGIN
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Approve PO Amount','','Starting and max'
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Min Amount','',@logMinAmount
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Max Amount','',@logMaxAmount
END
ELSE IF(ISNULL(@POApproveAmt,-1) = 1)
BEGIN
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Approve PO Amount','','Greater than'
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Min Amount','',@logMinAmount
END

-- Customers module


--IF(ISNULL(@CustomerPermissions,'') != '')
--BEGIN 	
--	Declare @logCustomerPermissions varchar(255) = UpdatePermissionStringForLogs(@CustomerPermissions)
--	--Set @logCustomerPermissions = 'Add - ' + SUBSTRING(@CustomerPermissions,1,1)
--	--							+ ', Edit - ' + SUBSTRING(@CustomerPermissions,2,1)
--	--							+ ', Delete - ' + SUBSTRING(@CustomerPermissions,3,1)
--	--							+ ', View - ' + SUBSTRING(@CustomerPermissions,4,1)
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Customer','',@logCustomerPermissions
--END

-- Customer
IF(ISNULL(@CustomermodulePermission,'N') != '')
BEGIN 	
	Declare @logCustomermodulePermission varchar(10) = ISNULL(@CustomermodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Customers module','',@logCustomermodulePermission
END
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Customer', @CustomerPermissions
-- Location
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Location', @LocationrPermissions
-- Equipments
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Equipments', @Elevator
-- Receive Payment
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Receive Payment', @ApplyPermissions
-- Make Deposit
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Make Deposit', @DepositPermissions
-- Collections 
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Collections', @CollectionsPermissions, '4'
-- Credit Hold
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Credit Hold', @CreditHold, '4'
-- Credit Flag
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Credit Flag', @CreditFlag, '4'
--IF(ISNULL(@CreditHold,'') = 'YYYY')
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Credit Hold','','Y'
--ELSE 
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Credit Hold','','N'
-- Write off
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Write off', @writeOff, '4'

-- Recurring module
IF(ISNULL(@RCmodulePermission,'N') != '')
BEGIN 	
	Declare @logRCmodulePermission varchar(10) = ISNULL(@RCmodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Recurring module','',@logRCmodulePermission
END
-- Recurring Contracts
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Recurring Contracts', @ProcessRCPermission
-- Safety Tests 
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Safety Tests', @SafetyTestsPermission
-- Recurring Invoices @ProcessC
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Recurring Invoices', @ProcessC, '134'
-- Recurring Tickets
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Recurring Tickets', @ProcessT, '134'
-- Renew/Escalate
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Renew/Escalate', @RCRenewEscalatePermission, '14'

-- Schedule module
IF(ISNULL(@Schedulemodule,'N') != '')
BEGIN 	
	Declare @logSchedulemodule varchar(10) = ISNULL(@Schedulemodule,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Schedule module','',@logSchedulemodule
END
-- Ticket
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Ticket', @TicketPermission, '12346'
-- Completed Ticket 
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Completed Ticket', @TicketResolvedPermission
-- Mass Review Ticket @MassReview
IF (ISNULL(@MassReview,0) = 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Mass Review Ticket','','N'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Mass Review Ticket','','Y'
-- Mass Review Timesheet @MassTimesheetCheck
IF(ISNULL(@MassTimesheetCheck,'N') != '')
BEGIN 	
	Declare @logMassTimesheetCheck varchar(10) = ISNULL(@MassTimesheetCheck,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Mass Review Timesheet','',@logMassTimesheetCheck
END
-- Schedule Board @ScheduleBoardPermission
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Schedule Board', @ScheduleBoardPermission, '4'
-- Route Builder @RouteBuilderPermission
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Route Builder', @RouteBuilderPermission, '4'
-- Timestamps Fixed @TimestamFixed
IF (ISNULL(@TimestamFixed,0) = 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Timestamps Fixed','','N'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Timestamps Fixed','','Y'
-- Timesheet Entry @MTimesheetPermission
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Timesheet Entry', @MTimesheetPermission, '4'
-- e-Timesheet (Payroll data) @ETimesheetPermission
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - e-Timesheet (Payroll data)', @ETimesheetPermission, '4'
-- Map @MapRPermission
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Map', @MapRPermission,'4'

-- Project module
IF(ISNULL(@ProjectModulePermission,'N') != '')
BEGIN 	
	Declare @logProjectModulePermission varchar(10) = ISNULL(@ProjectModulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Project module','',@logProjectModulePermission
END
-- Project
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Project', @ProjectPermissions
-- Project Template
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Project Template', @ProjecttempPermission
-- BOM
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - BOM', @BOMPermission
-- WIP
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - WIP', @WIPPermission, '12346'
-- Milestones
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Billing', @MilestonesPermission
-- Project status
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Project status - Close', @JobClosePermission, '1'
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Project status - Complete', @JobCompletedPermission, '1'
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Project status - Reopen', @JobReopenPermission, '1'
-- ProjectList Finance
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - ProjectList Finance', @ProjectListPermission, '1'
-- Project Finance 
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Project Finance', @FinancePermission, '1'

-- Inventory module
IF(ISNULL(@InventoryModulePermission,'N') != '')
BEGIN 	
	Declare @logInventoryModulePermission varchar(10) = ISNULL(@InventoryModulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Inventory module','',@logInventoryModulePermission
END
-- Inventory Item List @InventoryItemPermissions
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory Item List', @InventoryItemPermissions
-- Inventory Adjustment
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory Adjustment', @InventoryAdjustmentPermissions
-- Inventory WareHouse
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory WareHouse', @InventoryWarehousePermissions
-- Inventory setup
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory setup', @InventorysetupPermissions
-- Inventory Finance
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory Finance', @InventoryFinancePermissions

-- Sales module
IF(ISNULL(@SalesManager,'N') != '')
BEGIN 	
	Declare @logSalesManager varchar(10) = ISNULL(@SalesManager,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Inventory module','',@logSalesManager
END
-- Leads 
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Leads', @SalesPermission, '12346'
-- Opportunities
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Opportunities', @ProposalPermission, '12346'
-- Estimate
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Estimate', @EstimatePermission, '12346'
-- Complete Task
IF(ISNULL(@CompleteTasksPermission,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Complete Task','','Y'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Complete Task','','N'
-- Task FollowUp 
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Task FollowUp', @FollowUpPermission, '1'
-- Tasks
IF(ISNULL(@TasksPermission,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Tasks','','Y'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Tasks','','N'
-- Convert Estimate
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Convert Estimate', @ConvertEstimatePermission, '1'
-- Sales Setup
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Sales Setup', @ConvertEstimatePermission, '1'

-- AP module @AccountPayablemodulePermission
IF(ISNULL(@AccountPayablemodulePermission,'N') != '')
BEGIN 	
	Declare @logAccountPayablemodulePermission varchar(10) = ISNULL(@AccountPayablemodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - AP module','',@logAccountPayablemodulePermission
END
-- Vendors
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Vendors', @APVendor
-- Bills
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Bills', @APBill
-- Manage Checks
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Manage Checks', @APBillPay

-- Financial module 
IF(ISNULL(@Financialmodule,'N') != '')
BEGIN 	
	Declare @logFinancialmodule varchar(10) = ISNULL(@Financialmodule,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Financial module','',@logFinancialmodule
END
-- Chart of Accounts 
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Chart of Accounts', @ChartPermissions
-- Journal Entry
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Journal Entry', @JournalEntryPermissions
-- Bank Reconciliation
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Bank Reconciliation', @BankReconciliationPermissions
-- Financial Statement Module
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Financial Statement Module', @FinanceState, '6'

-- Billing module @BillingmodulePermission
IF(ISNULL(@BillingmodulePermission,'N') != '')
BEGIN 	
	Declare @logBillingmodulePermission varchar(10) = ISNULL(@BillingmodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Billing module','',@logBillingmodulePermission
END
-- Invoices
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Invoices', @InvoicePermission
-- Billing Codes
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Billing Codes', @BillingCodesPermission
-- Online Payment
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Online Payment', @PaymentHistoryPermission, '4'

-- Purchasing module
IF(ISNULL(@PurchasingmodulePermission,'N') != '')
BEGIN 	
	Declare @logPurchasingmodulePermission varchar(10) = ISNULL(@PurchasingmodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Purchasing module','',@logPurchasingmodulePermission
END
-- PO
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - PO', @POPermission
-- Receive PO
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Receive PO', @RPOPermission
-- PO Notification
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - PO Notification', @PONotification, '1'

-- Program Module @ProgFunctions
IF(ISNULL(@ProgFunctions,'N') != '')
BEGIN 	
	Declare @logProgFunctions varchar(10) = ISNULL(@ProgFunctions,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Program Module','',@logProgFunctions
END
-- Employee Maintenance @Employee
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Employee Maintenance', @Employee, '4'
-- Users 
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Users', @AccessUser, '1'
-- Enter expenses @Expenses
--Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Enter expenses', @Expenses, '1'
-- Email Dispatch 
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Email Dispatch', @Dispatch, '1'

-- Document/Contact 
-- Document
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Document', @DocumentPermission
-- Contact
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Contact', @ContactPermission

/* End Permissions */
/* Payment */
-- Payment - Emp ID
IF(ISNULL(@Ref,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Emp ID','',@Ref
END
-- Payment - SSN/SIN
IF(ISNULL(@SSN,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - SSN/SIN','',@SSN
END
-- Payment - Date Of Birth 
IF(@DBirth is not null And @DBirth != '')
BEGIN 	
	DECLARE @logDBirth varchar(150)
	SELECT @logDBirth = convert(varchar, @DBirth, 101)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Date Of Birth','',@logDBirth
END
-- Payment - Payment Method
IF(ISNULL(@PayMethod,-1) = 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Payment Method','','Salaried'
ELSE IF(ISNULL(@PayMethod,-1) = 1)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Payment Method','','Hourly'
ELSE IF(ISNULL(@PayMethod,-1) = 2)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Payment Method','','Fixed Hours'
-- Payment - Payment Period
DECLARE @logPaymentPeriod Varchar(50) = ''
SELECT @logPaymentPeriod = 
	CASE 
		WHEN ISNULL(@PayPeriod, -1) = 0 THEN 'Weekly'
		WHEN ISNULL(@PayPeriod, -1) = 1 THEN 'Bi-Weekly'
		WHEN ISNULL(@PayPeriod, -1) = 2 THEN 'Semi-Monthly'
		WHEN ISNULL(@PayPeriod, -1) = 3 THEN 'Monthly'
		WHEN ISNULL(@PayPeriod, -1) = 4 THEN 'Semi-Annually'
		WHEN ISNULL(@PayPeriod, -1) = 5 THEN 'Annually'
		ELSE ''
	END
IF (@logPaymentPeriod != '')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Payment Period','',@logPaymentPeriod
-- Payment - Gender
IF(ISNULL(@Sex,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Gender','',@Sex
END
-- Payment - Date of Hiring 
IF(@DtHired is not null And @DtHired != '')
BEGIN 	
	DECLARE @logDtHired varchar(150)
	SELECT @logDtHired = convert(varchar, @DtHired, 101)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Date of Hiring','',@logDtHired
END
-- Payment - Amount @Salary
IF(ISNULL(@Salary,0) != 0)
BEGIN 	
	DECLARE @logSalary Varchar(40) = Convert(varchar(40),@Salary)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Amount','',@logSalary
END
-- Payment - Hourly Rate 
IF(ISNULL(@HourlyRate,0) != 0)
BEGIN 	
	DECLARE @logHourlyRate Varchar(40) = Convert(varchar(40),@HourlyRate)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Hourly Rate','',@logHourlyRate
END
-- Payment - Ethnicity
IF(ISNULL(@Race,'') != '')
BEGIN 	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Ethnicity','',@Race
END
-- Payment - Date of Termination 
IF(@DtFired is not null And @DtFired != '')
BEGIN 	
	DECLARE @logDtFired varchar(150)
	SELECT @logDtFired = convert(varchar, @DtFired, 101)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Date of Termination','',@logDtFired
END
-- Payment - Hours  
IF(ISNULL(@PHours,0) != 0)
BEGIN 	
	DECLARE @logPHours Varchar(40) = Convert(varchar(40),@PHours)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Hours','',@logPHours
END
-- Payment - Mileage Rate
IF(ISNULL(@mileagerate,0) != 0)
BEGIN 	
	DECLARE @logmileagerate Varchar(40) = Convert(varchar(40),@mileagerate)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Payment - Mileage Rate','',@logmileagerate
END
/* End Payment */

/* Field Worker Options*/
IF(@Schedule = 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Schedule Board','','No'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Schedule Board','','Yes'

--
IF(@MSAuthorisedDeviceOnly = 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Authorized Device','','No'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Authorized Device','','Yes'

IF(@Mapping = 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Maps','','No'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Maps','','Yes'


IF(@MerchantInfoId is not null AND @MerchantInfoId != 0)
BEGIN
	DECLARE @logMerchantInfoId varchar(100)
	SELECT @logMerchantInfoId = MerchantId from tblGatewayInfo where id = @MerchantInfoId
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Merchant ID','',@logMerchantInfoId
END

--@DeviceID VARCHAR(100)
IF(ISNULL(@DeviceID,'') != '')
BEGIN
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Device ID','',@DeviceID
END
--@authdevID NVARCHAR(100)
IF(ISNULL(@authdevID,'') != '')
BEGIN
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Authorized Device ID','',@authdevID
END
--@Super
IF(ISNULL(@Super,'') != '')
BEGIN
	IF (@Super = @UserName)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Is Supervisor','','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Is Supervisor','','No'

	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Supervisor','',@Super
END
--@DefaultWorker
IF(ISNULL(@DefaultWorker,0) = 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Default Worker','','No'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Field Worker - Default Worker','','Yes'
/* End Field Worker Options*/

-- User Role
IF(ISNULL(@UserRoleId,0) != 0)
BEGIN
	DECLARE @logUserRoleName varchar(255)
	SELECT @logUserRoleName = r.RoleName FROM tblRole r WHERE r.Id = @UserRoleId
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'User Role','',@logUserRoleName

	IF(@ApplyUserRolePermission = 1)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Applying User Role Permission','','Merge'
	ELSE IF(@ApplyUserRolePermission = 2)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Applying User Role Permission','','Override'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Applying User Role Permission','','None'
END
-- Violation
Exec spInsertCRUDPermissionLogs @CreatedBy, @RefId, 'Permissions - Violation', @ViolationPermission

IF(ISNULL(@Title,'') != '')
BEGIN
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'User Title','',@Title
END
/* End Logs */

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

COMMIT TRANSACTION

SELECT @userid
