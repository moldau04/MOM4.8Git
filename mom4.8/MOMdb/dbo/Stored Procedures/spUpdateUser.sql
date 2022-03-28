CREATE PROCEDURE [dbo].[spUpdateUser] 
	@UserName NVARCHAR(50)
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
	,@UserID INT
	,@RolID INT
	,@WorkID INT
	,@EmpID INT
	,@Mapping INT
	,@Schedule INT
	,@Password VARCHAR(10)
	,@DeviceID VARCHAR(100)
	,@Pager VARCHAR(100)
	,@Super VARCHAR(50)
	,@salesp INT
	,@str NVARCHAR(400)
	,@userlicID INT
	,@Remarks VARCHAR(8000)
	,@Lang VARCHAR(25)
	,@MerchantInfoId INT
	,@DefaultWorker SMALLINT
	,@DispatchCheck CHAR(1)
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
	,@addequip    SMALLINT
	,@editequip   SMALLINT
	,@FChart      SMALLINT
	,@FGLAdj      SMALLINT
	,@addFChart   SMALLINT
	,@editFChart  SMALLINT
	,@viewFChart  SMALLINT
	,@addFGLAdj   SMALLINT
	,@editFGLAdj  SMALLINT
	,@viewFGLAdj  SMALLINT
	,@FDeposit    SMALLINT
	,@AddDeposit  SMALLINT
	,@EditDeposit SMALLINT
	,@ViewDeposit SMALLINT
	,@FCustomerPayment    SMALLINT
	,@AddCustomerPayment  SMALLINT
	,@EditCustomerPayment SMALLINT
	,@ViewCustomerPayment SMALLINT
	,@FStatement SMALLINT
	,@StartDate DATETIME
	,@EndDate DATETIME
	,@APVendor VARCHAR(4) = 'NNNN'
	,@APBill VARCHAR(4) = 'NNNN'
	,@APBillSelect   SMALLINT
	,@APBillPay VARCHAR(4) = 'NNNN' 
	,@WageItems TBLTYPEWAGEITEMS readonly
	,@CustomerPermissions VARCHAR(4) = 'NNNN'
	,@LocationrPermissions VARCHAR(4) = 'NNNN'
	,@ProjectPermissions VARCHAR(4) = 'NNNN'
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
	,@POApproveAmt SMALLINT = 0,
     @Lng NVARCHAR(100)=NULL
	,@Lat NVARCHAR(100)=NULL
	,@Country NVARCHAR(100)=NULL
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
	,@JournalEntryPermissions  VARCHAR(10) = 'NNNN'
	,@BankReconciliationPermissions  VARCHAR(10) = 'NNNN'
	,@RCmodulePermission CHAR(1) = 'N' 
	,@ProcessRCPermission  VARCHAR(4) = 'NNNN'
	,@ProcessC  VARCHAR(4) = 'NNNN'
	,@ProcessT  VARCHAR(4) = 'NNNN'	
	,@SafetyTestsPermission  VARCHAR(4) = 'NNNN'	
	,@RCRenewEscalatePermission VARCHAR(4) = 'NNNN'
	,@MinAmount NUMERIC(30, 2)
	,@MaxAmount NUMERIC(30, 2)
	,@Schedulemodule CHAR(1) = 'N' 
	,@ScheduleboardPermission VARCHAR(6) = 'NNNNN'
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
	,@PONotification Char(1) = 'N'
	,@JobClosePermission CHAR(6) = 'NNNNN'	
	,@InventoryModulePermission CHAR(1) = 'N'	
	,@ProjectModulePermission CHAR(1) = 'N'
	,@JobCompletedPermission CHAR(1) = 'N'
	,@JobReopenPermission CHAR(1) = 'N'
	,@WriteOff VARCHAR(6)='NNNNNN'	
	,@IsProjectManager BIT
	,@IsAssignedProject BIT
	,@IsReCalculateLaborExpense BIT
	,@UpdatedBy VARCHAR (50)
	--,@LastLoginDate DateTime
	--,@LoginFailedAttempts INT
    ,@TicketVoidPermission  int = 0
	,@Employees varchar(6)
	,@PRPRcess varchar(6)
	,@PRRegister varchar(6)
	,@PRReport varchar(6)
	,@PRWage varchar(6)
	,@PRDeduct varchar(6)
	,@PR bit
	,@UserRoleId int
	,@ApplyUserRolePermission smallint
	,@MassPayrollTicket varchar(1)
	,@ViolationPermission  VARCHAR(4)='NNNN'
AS

DECLARE @Rol INT
DECLARE @Ticket VARCHAR(10)
DECLARE @work INT
 
DECLARE @Location VARCHAR(10)
DECLARE @PO VARCHAR(10)
DECLARE @Control VARCHAR(10)
DECLARE @UserS VARCHAR(10)
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
Declare @EmpPR bit

SELECT @EmpPR =ISNULL(PR,0) FROM Control 


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

SELECT @Ticket = Isnull(Ticket, 'NNNNNN')
 
	,@Location = Isnull(Location, 'NNNNNN') 
	,@Control = Isnull(CONTROL, 'NNNNNN')
	,@UserS = Isnull(UserS, 'NNNNNN')
	--,@sales = Isnull(Sales, 'NNNNNN')
	,@Employee = Isnull(Employee, 'NNNNNN')
	,@Elevator = Isnull(Elevator, 'NNNNNN')
	,@Chart = Isnull(Chart, 'NNNNNN')
	,@GLAdj = Isnull(GLAdj, 'NNNNNN')
	,@TC = Isnull(TC, 'NNNNNN')
FROM tblUser
WHERE ID = @UserID

--IF( @Schedule = 1 )    
--  BEGIN    
--      SET @Ticket='Y'+ SUBSTRING ( @Ticket ,2, 5)    
--  END    
--ELSE    
--  BEGIN    
--      SET @Ticket='N'+ SUBSTRING ( @Ticket ,2, 5)    
--  END    
--------------- Ticket Permissions     
------set @Ticket=substring(@TicketPermissions,1,4)+isnull(substring(@Ticket,5,2),'NN');    
IF (@Mapping = 1)
BEGIN
	SET @Ticket = Substring(@Ticket, 1, 3) + 'Y' + Substring(@Ticket, 5, 2);
END
ELSE
BEGIN
	SET @Ticket = Substring(@Ticket, 1, 3) + 'N' + Substring(@Ticket, 5, 2);
END

--------------Ticket Permissions     
-----Vender Permissions    
--IF (@APVendor = 1)
--BEGIN
--	SET @Vendor = @VendorsPermission + 'YY'
--END
--ELSE
--BEGIN
--	SET @Vendor = @VendorsPermission + 'NN'
--END

--IF (@APBill = 1)
--BEGIN
--	SET @Bill = 'YYYYYY'
--END
--ELSE
--BEGIN
--	SET @Bill = 'NNNNNN'
--END

IF (@APBillSelect = 1)
BEGIN
	SET @BillSelect = 'YYYYYY'
END
ELSE
BEGIN
	SET @BillSelect = 'NNNNNN'
END

--IF (@APBillPay = 1)
--BEGIN
--	SET @BillPay = 'YYYYYY'
--END
--ELSE
--BEGIN
--	SET @BillPay = 'NNNNNN'
--END

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
	SET @Elevator = 'Y' + Substring(@Elevator, 2, 5)
END
ELSE
BEGIN
	SET @Elevator = 'N' + Substring(@Elevator, 2, 5)
END

IF (@editequip = 1)
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 1) + 'Y' + Substring(@Elevator, 3, 4)
END
ELSE
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 1) + 'N' + Substring(@Elevator, 3, 4)
END

IF (@DeleteEquip = 1)
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 2) + 'Y' + Substring(@Elevator, 4, 3)
END
ELSE
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 2) + 'N' + Substring(@Elevator, 4, 3)
END

IF (@ViewEquip = 1)
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 3) + 'Y' + Substring(@Elevator, 5, 2)
END
ELSE
BEGIN
	SET @Elevator = Substring(@Elevator, 1, 3) + 'N' + Substring(@Elevator, 5, 2)
END

IF (@SalesMgr = 1)
BEGIN
	--SET @sales = 'Y' + Substring(@sales, 1, 1);
	SET @SalesManager = 'Y';
END
ELSE
BEGIN
	SET @SalesManager = 'N';
END


IF(@EmpPR =0)
BEGIN
	IF (@EmployeeMainten = 1)
	BEGIN
		SET @Employee = Substring(@Employee, 1, 3) + 'Y' + Substring(@Employee, 5, 2);
	END
	ELSE
	BEGIN
		SET @Employee = Substring(@Employee, 1, 3) + 'N' + Substring(@Employee, 5, 2);
	END
	End
ELSE
BEGIN
  SET @Employee =@Employees
END

IF (@TimestamFixed = 1)
BEGIN
	SET @TC = Substring(@TC, 1, 1) + 'Y' + Substring(@TC, 3, 4);
END
ELSE
BEGIN
	SET @TC = Substring(@TC, 1, 1) + 'N' + Substring(@TC, 3, 4);
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
		IF (@AccessUser != 'y')
		BEGIN
			RAISERROR (
				'Removing user permission error. This user was used for reseting password.'
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
		
		/* Add commom logs*/
		Exec spUpdateUserCommonLogs @Field  	
				,@status 	
				,@FName 	
				,@MName 	
				,@LName 	
				,@Address 	
				,@City 		
				,@State 	
				,@Zip	 	
				,@Tel		
				,@Cell 		
				,@Email 	
				,@UserID 	
				,@Schedule 	
				,@Pager 	
				,@salesp 	
				,@Remarks 	
				,@InServer 	
				,@InServerType 	
				,@InUsername  	
				,@InPassword  	
				,@InPort   		
				,@OutServer 	
				,@OutUsername 	
				,@OutPassword 	
				,@OutPort 		
				,@SSL 			
				,@BccEmail 		
				,@EmailAccount 	
				,@Department 	
				,@StartDate 	
				,@EndDate 		
				,@SalesAssigned 
				,@NotificationOnAddOpportunity 	
				,@Lng							
				,@Lat 							
				,@Country 						
				,@EmNum 						
				,@EmName 						
				,@TakeASentEmailCopy 			
				,@IsProjectManager				
				,@IsAssignedProject 	
				,@IsReCalculateLaborExpense
				,@Ref 					
				,@SSN 					
				,@DBirth 				
				,@PayMethod 	
				,@PayPeriod
				,@Sex 					
				,@DtHired 				
				,@Salary 				
				,@HourlyRate			
				,@Race 					
				,@DtFired				
				,@PHours 				
				,@mileagerate			
				,@MSAuthorisedDeviceOnly
				,@Mapping 				
				,@MerchantInfoId 		
				,@DeviceID 				
				,@authdevID 			
				,@Super 				
				,@UserName 				
				,@DefaultWorker 	
				,@EmpID
				,@UserRoleId
				,@ApplyUserRolePermission
				,@UpdatedBy 		
				,@Title
				
		Exec spUpdateUserPermissionLogs 	@APVendor 						
											,@APBill 						
											,@APBillPay 					
											,@CustomerPermissions 			
											,@LocationrPermissions 			
											,@ProjectPermissions 			
											,@MSAuthorisedDeviceOnly		
											,@TicketDelete 					
											,@ProjectListPermission			
											,@FinancePermission 			
											,@BOMPermission					
											,@WIPPermission					
											,@MilestonesPermission 			
											,@InventoryItemPermissions 		
											,@InventoryAdjustmentPermissions
											,@InventoryWarehousePermissions	
											,@InventorysetupPermissions		
											,@InventoryFinancePermissions	
											,@DocumentPermission			
											,@ContactPermission				
											,@ProjecttempPermission			
											,@NotificationOnAddOpportunity	
											,@VendorsPermission				
											,@InvoicePermission				
											,@BillingCodesPermission		
											,@POPermission					
											,@PurchasingmodulePermission	
											,@BillingmodulePermission		
											,@RPOPermission					
											,@AccountPayablemodulePermission
											,@PaymentHistoryPermission		
											,@CustomermodulePermission		
											,@ApplyPermissions 				
											,@DepositPermissions			
											,@CollectionsPermissions		
											,@Financialmodule				
											,@ChartPermissions				
											,@JournalEntryPermissions		
											,@BankReconciliationPermissions	
											,@RCmodulePermission			
											,@ProcessRCPermission 			
											,@ProcessC						
											,@ProcessT						
											,@SafetyTestsPermission			
											,@RCRenewEscalatePermission		
											,@Schedulemodule				
											,@ScheduleboardPermission		
											,@TicketPermission				
											,@TicketResolvedPermission		
											,@MTimesheetPermission			
											,@ETimesheetPermission			
											,@MapRPermission				
											,@RouteBuilderPermission		
											,@MassTimesheetCheck			
											,@CreditHold					
											,@CreditFlag					
											,@SalesPermission				
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
											,@WriteOff						
											,@Elevator						
											,@SalesManager					
											,@FinanceState					
											,@Employee						
											,@TC							
											,@MassReview 					
											,@ProgFunctions					
											,@AccessUser	
											,@BillSelect
											,@DispatchCheck
											,@POLimit
											,@POApprove
											,@POApproveAmt
											,@MinAmount
											,@MaxAmount
											,@UserID						
											,@UpdatedBy 
											,@MassPayrollTicket
											,@ViolationPermission
											
		IF (@DefaultWorker = 1)
		BEGIN
			UPDATE tbluser
			SET defaultworker = 0
		END

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
			----fUser = @UserName,    
			--msmuser=@MSMUser,    
			--msmpass=@MSMPass,    
			PDA = @PDA
			,STATUS = @status
			--,Password = @Password
			,Dispatch = @TicketPermission 
			,Location = @LocationrPermissions + Isnull(Substring(Location, 5, 2), 'NN')
			,PO = @POPermission  + Isnull(Substring(PO, 5, 2), 'NN')
			,RPO = @RPOPermission  + Isnull(Substring(RPO, 5, 2), 'NN')
			,CONTROL = @ProgFunctions + Substring(@Control, 2, 5)
			,UserS = @AccessUser + Substring(@UserS, 2, 5)
			--,Ticket = @Ticket
			,Remarks = @remarks
			,Lang = @Lang
			,MerchantInfoID = @MerchantInfoId
			,LastUpdateDate = Getdate()
			,DefaultWorker = @DefaultWorker
			,Sales = @SalesPermission
			,MassReview = @MassReview
			,EmailAccount = @EmailAccount
			,Employee = @Employee
			,TC =  @TC
			,Elevator = @Elevator
			,Chart = @ChartPermissions + Isnull(Substring(Chart, 5, 2), 'NN')		
			,GLAdj =@JournalEntryPermissions + Isnull(Substring(GLAdj, 5, 2), 'NN')
			,Deposit = @DepositPermissions + Isnull(Substring(Deposit, 5, 2), 'NN')
			,CustomerPayment = @CustomerPayment
			,Financial = @FinanceState
			,Vendor = @APVendor  + Isnull(Substring(Vendor, 5, 2), 'NN')
			,Bill = @APBill  + Isnull(Substring(Bill, 5, 2), 'NN')
			,BillSelect = @BillSelect 
			,BillPay = @APBillPay  + Isnull(Substring(BillPay, 5, 2), 'NN')
			,fStart = @StartDate
			,fEnd = @EndDate
			,Job = @ProjectPermissions + Isnull(Substring(Job, 5, 2), 'NN')
			,OWNER = @CustomerPermissions + Isnull(Substring(OWNER, 5, 2), 'NN')
			,MSAuthorisedDeviceOnly = @MSAuthorisedDeviceOnly
			,ProjectListPermission = @ProjectListPermission
			,FinancePermission = @FinancePermission
			,BOMPermission = @BOMPermission
			,WIPPermission = @WIPPermission
			,MilestonesPermission = @MilestonesPermission
			,Item = @InventoryItemPermissions + Isnull(Substring(Item, 5, 2), 'NN')
			,InvAdj = @InventoryAdjustmentPermissions + Isnull(Substring(Item, 5, 2), 'NN')
			,Warehouse = @InventoryWarehousePermissions + Isnull(Substring(Item, 5, 2), 'NN')
			,InvSetup = @InventorysetupPermissions + Isnull(Substring(Item, 5, 2), 'NN')
			,InvViewer = @InventoryFinancePermissions + Isnull(Substring(Item, 5, 2), 'NN')
			,DocumentPermission = @DocumentPermission
			,ContactPermission = @ContactPermission
			,SalesAssigned = @SalesAssigned
			,ProjecttempPermission = @ProjecttempPermission
			,NotificationOnAddOpportunity = @NotificationOnAddOpportunity
			,POLimit = @POLimit
			,POApprove = @POApprove
			,POApproveAmt = @POApproveAmt
			,MinAmount = @MinAmount
			,MaxAmount = @MaxAmount
			,Lng = @Lng
			,Lat = @Lat
			,Country = @Country
			,Title = @Title 
			,invoice=@InvoicePermission +Isnull(Substring(invoice, 5, 2), 'NN')
			,BillingCodesPermission= @BillingCodesPermission 
			,PurchasingmodulePermission=@PurchasingmodulePermission   
	        ,BillingmodulePermission=@BillingmodulePermission,  
			 AccountPayablemodulePermission=@AccountPayablemodulePermission
			 ,PaymentHistoryPermission=@PaymentHistoryPermission
			 ,CustomermodulePermission=@CustomermodulePermission
			 ,Apply=@ApplyPermissions + Isnull(Substring(Apply, 5, 2), 'NN')
			 ,Collection=@CollectionsPermissions + Isnull(Substring(Collection, 5, 2), 'NN')
			 ,BankRec=@BankReconciliationPermissions + Isnull(Substring(BankRec, 5, 2), 'NN')
			 ,FinancialmodulePermission=@Financialmodule
			   ,RCmodulePermission =@RCmodulePermission
			    ,ProcessRCPermission=@ProcessRCPermission + Isnull(Substring(ProcessRCPermission, 5, 2), 'NN')
				 ,ProcessC=@ProcessC + Isnull(Substring(ProcessC, 5, 2), 'NN')	
				 ,ProcessT=@ProcessT + Isnull(Substring(ProcessT, 5, 2), 'NN')
				 ,RCRenewEscalatePermission=@RCRenewEscalatePermission
				 ,RCSafteyTest= @SafetyTestsPermission +Isnull(Substring(RCSafteyTest, 5, 2), 'NN')	
				 ,SchedulemodulePermission=@Schedulemodule
				 --,Dispatch=@SchedulePermission + Isnull(Substring(Dispatch, 5, 1), 'N')++Isnull(Substring(@SchedulePermission, 6, 1), 'N')
			     ,Ticket=Substring(@ScheduleboardPermission, 1, 4)  + Isnull(Substring(Ticket, 5, 1), 'N') + Isnull(Substring(@TicketPermission, 6, 1), 'N')
				  ,Resolve=Substring(@TicketResolvedPermission, 1, 4) + Isnull(Substring(Resolve, 5, 1), 'N')+Isnull(Substring(@TicketResolvedPermission, 6, 1), 'N')
				 ,MTimesheet= Substring(@MTimesheetPermission, 1, 4)  + Isnull(Substring(MTimesheet, 5, 1), 'N')+Isnull(Substring(@MTimesheetPermission, 6, 1), 'N')
				 ,ETimesheet=Substring(@ETimesheetPermission, 1, 4)  + Isnull(Substring(ETimesheet, 5, 1), 'N')+Isnull(Substring(@ETimesheetPermission, 6, 1), 'N')
                 ,MapR=Substring(@MapRPermission, 1, 4)  + Isnull(Substring(MapR, 5, 1), 'N')+Isnull(Substring(@MapRPermission, 6, 1), 'N')
				 ,RouteBuilder=Substring(@RouteBuilderPermission, 1, 4)  + Isnull(Substring(RouteBuilder, 5, 1), 'N')+Isnull(Substring(@RouteBuilderPermission, 6, 1), 'N')
				 ,MassTimesheetCheck=@MassTimesheetCheck
				 ,CreditHold=@CreditHold
				 ,CreditFlag=@CreditFlag
				 ,SalesManager=@SalesManager
				 ,ToDo=@TasksPermission
				 ,ToDoC=@CompleteTasksPermission
				 ,FU= @FollowUpPermission
				 ,Proposal= @ProposalPermission
				 ,Estimates= @EstimatePermission
				 ,AwardEstimates= @ConvertEstimatePermission
				 ,SalesSetup=@SalesSetupPermission 
				 ,PONotification=@PONotification  
				 ,JobClose=@JobClosePermission	
	             ,InventoryModulePermission=@InventoryModulePermission 	
	             ,ProjectModulePermission=@ProjectModulePermission 
				 ,JobCompletedPermission=@JobCompletedPermission
				 ,JobReopenPermission=@JobReopenPermission
				 ,WriteOff=@WriteOff + ISNULL(SUBSTRING(ISNULL(WriteOff,'NNNNNN'),2,5),'NNNNN')
				 ,IsProjectManager=@IsProjectManager
				,IsAssignedProject=@IsAssignedProject 
				,IsReCalculateLaborExpense = @IsReCalculateLaborExpense
				,TicketVoidPermission=@TicketVoidPermission
				,PRProcess=@PRPRcess
				,PRRegister=@PRRegister
				,PRReport =@PRReport
				,PRWage=@PRWage
				,@PRDeduct=@PRDeduct
				,PR=@PR
				,MassPayrollTicket=@MassPayrollTicket
				,ViolationPermission=@ViolationPermission

		WHERE ID = @UserID

		-- Update or delete role of user
		DELETE tblUserRole WHERE UserId = @UserID
		IF (ISNULL(@UserRoleId, 0) != 0)
		BEGIN
			INSERT INTO tblUserRole (RoleId, UserId, UpdatedBy, UpdatedDate) VALUES (@UserRoleId, @UserID, @UpdatedBy, GETDATE())
			UPDATE tblUser SET ApplyUserRolePermission = @ApplyUserRolePermission WHERE ID = @UserID
		END
		ELSE
		BEGIN
			UPDATE tblUser SET ApplyUserRolePermission = 0 WHERE ID = @UserID
		END
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

UPDATE tblWork
SET STATUS = 1
WHERE fDesc = @UserName

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
		,Remarks
				,Contact
		,Website
		,[Type]
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
				,@EmName
		,@EmNum
		,@Type
		)

	SET @Rol = Scope_identity()
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

--IF( @Field = 2 )    
--  BEGIN    
--      IF NOT EXISTS (SELECT 1    
--                     FROM   Owner    
--                     WHERE  fLogin = @UserName    
--                            AND ID <> @UserID    
--                     UNION    
--                     SELECT 1    
--                     FROM   tblUser    
--                     WHERE  fUser = @UserName)    
--   BEGIN    
--            UPDATE Owner    
--            SET    fLogin = @UserName,    
--                   Password = @Password,    
--                   Status = @status,    
--                   TicketO = @Schedule,    
--                   TicketD = @Mapping,    
--                   Rol = @RolID    
--            WHERE  ID = @UserID    
--        END    
--      ELSE    
--        BEGIN    
--            RAISERROR ('Username already exixts, please use different username!',16,1)    
--            ROLLBACK TRANSACTION    
--            RETURN    
--        END    
--  END    
--IF @@ERROR <> 0    
--   AND @@TRANCOUNT > 0    
--  BEGIN    
--      RAISERROR ('Error Occured',16,1)    
--      ROLLBACK TRANSACTION    
--      RETURN    
--  END    
IF (@Field = 1)
BEGIN
	IF NOT EXISTS (
			SELECT 1
			FROM tblWork
			WHERE fDesc = @UserName
			)
	BEGIN
	
		if @Super <>'-1'
		begin
		
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
		end
		
	END
	ELSE
	BEGIN
		if @Super <>'-1'
		begin
		UPDATE tblWork
		SET fDesc = @UserName
			,STATUS = @status
			,Super = @Super
			,DBoard = @Schedule
			,HourlyRate = @HourlyRate
		WHERE fDesc = @UserName

		
		END
		SET @work = (
				SELECT ID
				FROM tblWork
				WHERE fDesc = @UserName
				)
	END
END
ELSE
BEGIN
	SET @work = NULL
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
		IF NOT EXISTS (
				SELECT 1
				FROM emp
				WHERE DeviceID = @DeviceID
					AND ID <> @EmpID
					AND @DeviceID <> ''
				)
		BEGIN
			UPDATE Emp
			SET Field = @Field
				,STATUS = @status
				,fFirst = @FName
				,Middle = @MName
				,Last = @LName
				,DHired = @DtHired
				,DFired = @DtFired
				,CallSign = @UserName
				,NAME = @FName + ', ' + @LName
				,DeviceID = @DeviceID
				,Pager = @Pager
				,fWork = @work
				,Sales = @salesp
				,PMethod = CASE @PayMethod
					WHEN 2
						THEN 1
					ELSE @PayMethod
					END
				,PHour = CASE @PayMethod
					WHEN 1
						THEN 0
					ELSE @PHours
					END
				,Salary = @Salary
				,PFixed = CASE @PayMethod
					WHEN 2
						THEN 0
					ELSE 1
					END
				,Ref = @Ref
				,PayPeriod = @PayPeriod
				,MileageRate = @mileagerate
				,MSDeviceId = @authdevID
				,SSN		= @SSN
				,Sex		= @Sex
				,DBirth		= @DBirth
				,Race		= @Race
				,Title = @Title
			WHERE ID = @EmpID

			DELETE
			FROM tblJoinEmpDepartment
			WHERE Emp = @EmpID

			INSERT INTO tblJoinEmpDepartment (
				Emp
				,Department
				)
			SELECT @EmpID
				,*
			FROM dbo.Split(@Department, ',')

			DELETE
			FROM PRWageItem
			WHERE Emp = @EmpID

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
				,@EmpID
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
	END
	ELSE
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
				,SSN,Sex,DBirth,Race
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
				,@SSN,@Sex,@DBirth,@Race
				,@Title
				)

			SET @EmpID = Scope_identity()

			INSERT INTO tblJoinEmpDepartment (
				Emp
				,Department
				)
			SELECT @empid
				,*
			FROM dbo.Split(@Department, ',')
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

SELECT TOP 1 @lid = LID
FROM MSM2_Admin.dbo.tblJoinAuth ja
WHERE DBname = (
		SELECT Db_name()
		)
	AND ja.UserID = @userid
	AND STATUS = 0

IF (@status = 1)
BEGIN
	UPDATE MSM2_Admin.dbo.tblJoinAuth
	SET STATUS = 1
		,DATE = Getdate()
	WHERE UserID = @UserID
		AND STATUS = 0
		AND dbname = (
			SELECT Db_name()
			)

	UPDATE MSM2_Admin.dbo.tblUserAuth
	SET used = 0
		,dateupdate = Getdate()
	WHERE ID = @lid
END
ELSE IF (@status = 0)
BEGIN
	IF (@lid IS NULL)
	BEGIN
		
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
	END
END

COMMIT TRANSACTION
