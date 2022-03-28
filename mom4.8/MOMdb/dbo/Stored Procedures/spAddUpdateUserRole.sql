CREATE PROCEDURE [dbo].[spAddUpdateUserRole] 
	@RoleID int
	,@RoleName varchar(255)
	,@RoleDescription varchar(Max)
	,@Status smallint
	,@UpdatedBy varchar(50)
	,@Users tblTypeUser readonly
	,@PurchaseOrd CHAR(1)
	,@Expenses CHAR(1)
	,@ProgFunctions CHAR(1)
	,@AccessUser CHAR(1)
	--,@Mapping INT
	--,@Schedule INT
	,@salesp INT
	,@Dispatch CHAR(1)
	,@SalesMgr SMALLINT
	,@MassReview SMALLINT
	,@EmployeeMainten SMALLINT
	,@TimestamFixed SMALLINT
	--,@Department VARCHAR(100)
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
	,@APVendor VARCHAR(4) = 'NNNN'
	,@APBill VARCHAR(4) = 'NNNN'	
	,@APBillPay VARCHAR(4) = 'NNNN' 
	,@APBillSelect   SMALLINT
	,@CustomerPermissions VARCHAR(4) = 'NNNN'
	,@LocationrPermissions VARCHAR(4) = 'NNNN'
	,@ProjectPermissions VARCHAR(10) = 'NNNNNN'
	,@DeleteEquip SMALLINT = 1
	,@ViewEquip SMALLINT = 1
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
	,@InvoicePermission VARCHAR(4) = 'NNNN'
	,@BillingCodesPermission VARCHAR(4) = 'NNNN'
	,@POPermission VARCHAR(4) = 'NNNN'
	,@PurchasingmodulePermission CHAR(1) = 'N' 
	,@BillingmodulePermission CHAR(1) = 'N'
	,@RPOPermission VARCHAR(4) = 'NNNN'
	,@AccountPayablemodulePermission CHAR(1) = 'N' 
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
	,@MinAmount NUMERIC(30, 2)
	,@MaxAmount NUMERIC(30, 2)
	,@TicketVoidPermission  int =0
	,@Employees varchar(6)
	,@PRProcess varchar(6),
	@PRRegister varchar(6),
	@PRReport varchar(6),
	@PRWage varchar(6),
	@PRDeduct varchar(6),
	@PR BIT,
	@ViolationPermission  VARCHAR(4) = 'NNNN'		


AS
DECLARE @Rol INT
DECLARE @work INT
DECLARE @Ticket VARCHAR(10)
DECLARE @empid INT
DECLARE @userid INT
--DECLARE @RoleId INT
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
Declare @Screen varchar(100) = 'User Role';
Declare @RefId int;
Declare @empPR bit

SELECT @empPR =PR FROM Control 


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

--IF (@Schedule = 1)
--BEGIN
--	SET @Ticket = 'YYYYYY'
--END
--ELSE
--BEGIN
--	SET @Ticket = 'NYYYYY'
--END

--IF (@Mapping = 1)
--BEGIN
--	SET @Ticket = Substring(@Ticket, 1, 1) + 'YYYYY';
--END
--ELSE
--BEGIN
--	SET @Ticket = Substring(@Ticket, 1, 1) + 'YYNYY';
--END

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
	IF (@RoleName is null OR LTRIM(RTrim(@RoleName)) = '')
	BEGIN
		RAISERROR ('Error: Role name cannot be empty',16,1)
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		RETURN
	END

	IF (@RoleID = 0)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM tblRole WHERE RoleName = @RoleName)
		BEGIN
			INSERT INTO tblRole(
				--Role info
				RoleName
				,[Desc]
				,Status
				,CreatedDate
				,UpdatedDate
				,UpdatedBy
				--Start for Role Permissions
				,MassResolvePDATickets
				,ListsAdmin
				,Dispatch
				,Location
				,PO
				,CONTROL
				,UserS
				,Ticket
				,Sales
				,MassReview
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
				,Job
				,OWNER
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
				,TicketVoidPermission
				,PRProcess
				,PRRegister
				,PRReport
				,PRWage
				,PRDeduct
				,PR
				,Salesp
				,ViolationPermission
				)
	   
			VALUES (
				--Role info
				@RoleName
				,@RoleDescription
				,@Status
				,GETDATE()
				,GETDATE()
				,@UpdatedBy
				--Start for Role Permissions
				,0
				,0
				,@TicketPermission
				,@LocationrPermissions + 'NN'
				,@POPermission+'NN'
				,@ProgFunctions + 'NNNNN'
				,@AccessUser + 'NNNNN'
				,Substring(@ScheduleBoardPermission, 1, 4) + Substring(@TicketPermission, 5, 2)
				,@SalesPermission
				,@MassReview
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
				,@ProjectPermissions + 'NN'
				,@CustomerPermissions + 'NN'
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
				,@TicketVoidPermission
				,@PRProcess
				,@PRRegister
				,@PRReport
				,@PRWage 
				,@PRDeduct
				,@PR
				,@salesp
				,@ViolationPermission
				) 
			SET @RoleId = Scope_identity()

			Exec spAddUserRoleLogs 	@APVendor 						
									,@APBill 						
									,@APBillPay 					
									,@CustomerPermissions 			
									,@LocationrPermissions 			
									,@ProjectPermissions 			
									--,@MSAuthorisedDeviceOnly		
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
									,@Dispatch
									,@POLimit
									,@POApprove
									,@POApproveAmt
									,@MinAmount
									,@MaxAmount
									,@RoleID	
									,@RoleName
									,@RoleDescription
									,@Status
									,@UpdatedBy
									,@ViolationPermission

			IF @@ERROR <> 0
			BEGIN
				RAISERROR ('Adding user role error.',16,1)
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION
				RETURN
			END
		END
		ELSE
		BEGIN
			RAISERROR ('User Role name already exists.',16,1)
			IF @@TRANCOUNT > 0
				ROLLBACK TRANSACTION
			RETURN
		END
	END
	ELSE
	BEGIN
		DECLARE @CurRoleName varchar(255);
		DECLARE @CurDescription varchar(max);
		DECLARE @CurStatus smallint;
		DECLARE @ErrorMessage varchar(Max);
		SELECT @CurRoleName=RoleName, @CurDescription = [Desc],@CurStatus=[Status] FROM tblRole WHERE Id = @RoleID
		
		IF @CurRoleName is null
		BEGIN
			RAISERROR ('Cannot find this user role.',16,1)
			IF @@TRANCOUNT > 0
				ROLLBACK TRANSACTION
			RETURN
		END
		ELSE
		BEGIN
			IF @CurRoleName = 'Project Manager' OR @CurRoleName = 'Default Salesperson' OR @CurRoleName = 'Salesperson 2' OR @CurRoleName = 'Supervisor'
			BEGIN
				IF @CurRoleName != @RoleName
				BEGIN
					SET @ErrorMessage = 'Cannot change this default ' + @CurRoleName + ' user role.';
					RAISERROR (@ErrorMessage,16,1)
					IF @@TRANCOUNT > 0
						ROLLBACK TRANSACTION
					RETURN
				END

				IF @Status = 1
				BEGIN
					SET @ErrorMessage = 'Cannot make this default '+ @CurRoleName +' user role to inactive.'
					RAISERROR (@ErrorMessage,16,1)
					IF @@TRANCOUNT > 0
						ROLLBACK TRANSACTION
					RETURN
				END
			END
			ELSE
			BEGIN
				IF @CurRoleName != @RoleName
				BEGIN
					IF EXISTS (SELECT 1 FROM tblRole WHERE Id != @RoleID AND RoleName = @RoleName)
					BEGIN
						RAISERROR ('User Role name already exists.',16,1)
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION
						RETURN
					END
				END
			END
		END
		-- Logs
		SET @RefId = @RoleID
		IF @CurRoleName != @RoleName
		BEGIN
			EXEC log2_insert @UpdatedBy,@Screen,@RefId,'User Role Name',@CurRoleName,@RoleName
		END

		IF @CurDescription != @RoleDescription
		BEGIN
			EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Description',@CurDescription,@RoleDescription
		END

		IF @CurStatus != @Status
		BEGIN
			DECLARE @CurStatusStr varchar(20)
			DECLARE @StatusStr varchar(20)
			SELECT @CurStatusStr = CASE @CurStatus WHEN 1 THEN 'Inactive' ELSE 'Active' END,
				@StatusStr = CASE @Status WHEN 1 THEN 'Inactive' ELSE 'Active' END

			EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Status',@CurStatusStr,@StatusStr
		END
		
		-- Logs for user role permission
		Exec spUpdateUserRolePermissionLogs 	@APVendor 						
											,@APBill 						
											,@APBillPay 					
											,@CustomerPermissions 			
											,@LocationrPermissions 			
											,@ProjectPermissions 			
											--,@MSAuthorisedDeviceOnly		
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
											,@Dispatch
											,@POLimit
											,@POApprove
											,@POApproveAmt
											,@MinAmount
											,@MaxAmount
											,@RoleID						
											,@UpdatedBy
											,@ViolationPermission
			
		UPDATE tblRole SET 
			RoleName						= @RoleName		
			,[Desc]                         = @RoleDescription
			,Status                         = @Status
			,CreatedDate                    = GETDATE()
			,UpdatedDate                    = GETDATE()
			,UpdatedBy                      = @UpdatedBy
			--Start for Role Permissions  
			,MassResolvePDATickets          = 0
			,ListsAdmin                     = 0
			,Dispatch                       = @TicketPermission
			,Location                       = @LocationrPermissions + 'NN'
			,PO                             = @POPermission+'NN'
			,CONTROL                        = @ProgFunctions + 'NNNNN'
			,UserS                          = @AccessUser + 'NNNNN'
			,Ticket                         = Substring(@ScheduleBoardPermission, 1, 4) + Substring(@TicketPermission, 5, 2)
			,Sales                          = @SalesPermission
			,MassReview                     = @MassReview
			,Employee                       = @Employee
			,TC                             = @TC
			,Elevator                       = @Elevator
			,Chart                          = @ChartPermissions+ 'NN'
			,GLAdj                          = @JournalEntryPermissions + 'NN'
			,Deposit                        = @DepositPermissions + 'NN'
			,CustomerPayment                = @CustomerPayment
			,Financial                      = @FinanceState
			,Vendor                         = @APVendor  + 'NN'
			,Bill                           = @APBill  + 'NN'
			,BillPay                        = @APBillPay  + 'NN'
			,Job                            = @ProjectPermissions + 'NN'
			,OWNER                          = @CustomerPermissions + 'NN'
			,ProjectListPermission          = @ProjectListPermission
			,FinancePermission              = @FinancePermission
			,BOMPermission                  = @BOMPermission
			,WIPPermission                  = @WIPPermission
			,MilestonesPermission           = @MilestonesPermission
			,Item                           = @InventoryItemPermissions + 'NN'
			,InvAdj                         = @InventoryAdjustmentPermissions + 'NN'
			,Warehouse                      = @InventoryWarehousePermissions + 'NN'
			,InvSetup                       = @InventorysetupPermissions + 'NN'
			,InvViewer                      = @InventoryFinancePermissions + 'NN'
			,DocumentPermission             = @DocumentPermission
			,ContactPermission              = @ContactPermission
			,SalesAssigned                  = @SalesAssigned
			,ProjecttempPermission          = @ProjecttempPermission
			,NotificationOnAddOpportunity   = @NotificationOnAddOpportunity
			,POLimit                        = @POLimit
			,POApprove                      = @POApprove
			,POApproveAmt                   = @POApproveAmt
			,MinAmount                      = @MinAmount
			,MaxAmount                      = @MaxAmount
			,Invoice                        = @InvoicePermission+'NN' 
			,BillingCodesPermission         = @BillingCodesPermission
			,PurchasingmodulePermission     = @PurchasingmodulePermission 
			,BillingmodulePermission        = @BillingmodulePermission
			,RPO                            = @RPOPermission+'NN'
			,AccountPayablemodulePermission = @AccountPayablemodulePermission
			,PaymentHistoryPermission       = @PaymentHistoryPermission
			,CustomermodulePermission       = @CustomermodulePermission
			,Apply                          = @ApplyPermissions + 'NN'
			,Collection                     = @CollectionsPermissions + 'NN'
			,BankRec                        = @BankReconciliationPermissions + 'NN'
			,FinancialmodulePermission      = @Financialmodule
			,RCmodulePermission             = @RCmodulePermission
			,ProcessRCPermission            = @ProcessRCPermission+'NN'
			,ProcessC                       = @ProcessC
			,ProcessT                       = @ProcessT
			,RCRenewEscalatePermission      = @RCRenewEscalatePermission
			,RCSafteyTest	                = @SafetyTestsPermission
			,SchedulemodulePermission	    = @Schedulemodule
			,Resolve                        = @TicketResolvedPermission
			,MTimesheet                     = @MTimesheetPermission 
			,ETimesheet                     = @ETimesheetPermission
			,MapR                           = @MapRPermission
			,RouteBuilder                   = @RouteBuilderPermission
			,MassTimesheetCheck             = @MassTimesheetCheck
			,CreditHold                     = @CreditHold
			,CreditFlag                     = @CreditFlag
			,SalesManager                   = @SalesManager
			,ToDo                           = @TasksPermission
			,ToDoC                          = @CompleteTasksPermission
			,FU                             = @FollowUpPermission
			,Proposal                       = @ProposalPermission
			,Estimates                      = @EstimatePermission
			,AwardEstimates                 = @ConvertEstimatePermission
			,SalesSetup                     = @SalesSetupPermission 
			,PONotification                 = @PONotification 
			,JobClose	                    = @JobClosePermission	
			,InventoryModulePermission 	    = @InventoryModulePermission 	
			,ProjectModulePermission        = @ProjectModulePermission 
			,JobCompletedPermission         = @JobCompletedPermission
			,JobReopenPermission            = @JobReopenPermission
			,WriteOff                       = @WriteOff + 'NNNNN'
			,IsProjectManager               = @IsProjectManager
			,IsAssignedProject              = @IsAssignedProject
			,TicketVoidPermission           = @TicketVoidPermission
			,PRProcess                      = @PRProcess
			,PRRegister                     = @PRRegister
			,PRReport                       = @PRReport
			,PRWage                         = @PRWage 
			,PRDeduct                       = @PRDeduct
			,PR                             = @PR
			,Salesp							= @salesp
			,ViolationPermission	        = @ViolationPermission
		WHERE Id = @RoleID

		IF @@ERROR <> 0
		BEGIN
			RAISERROR ('Updating user role error.',16,1)
			IF @@TRANCOUNT > 0
				ROLLBACK TRANSACTION
			RETURN
		END
		
	END

	------------------------ BEGIN INSERT Users to Role -------------------------
	-- Adding Logs for user -------------
	-- Find the users were removed
	INSERT INTO [Log2] (
            [fUser]
           ,[Screen]
           ,[Ref]
           ,[Field]
           ,[OldVal]
           ,[NewVal])
    Select @UpdatedBy
		, 'User'
		, ur.UserId
		, 'User Role'
		, r.RoleName
		, ''
	from tblUserRole ur 
	left join @Users un on un.UserID = ur.UserId
	left join tblRole r on r.Id = ur.RoleId
	where ur.RoleId = @RoleID
	and un.UserID is null

	-- Find the users were added
	INSERT INTO [Log2] (
            [fUser]
           ,[Screen]
           ,[Ref]
           ,[Field]
           ,[OldVal]
           ,[NewVal])
	Select @UpdatedBy
		, 'User'
		, un.UserID
		, 'User Role'
		, ''
		, r.RoleName
	from @Users un
	left join tblUserRole ur  on un.UserID = ur.UserId
	inner join tblRole r on r.Id = @RoleID
	where ur.UserId is null

	INSERT INTO [Log2] (
            [fUser]
           ,[Screen]
           ,[Ref]
           ,[Field]
           ,[OldVal]
           ,[NewVal])
	Select @UpdatedBy
		, 'User'
		, un.UserID
		, 'Applying User Role Permission'
		, ''
		, Case u.ApplyUserRolePermission when 2 then 'Override' when 1 then 'Merge' else 'None' end
	from @Users un
	left join tblUserRole ur  on un.UserID = ur.UserId
	inner join tblUser u on u.ID = un.UserID
	WHERE  ur.UserId is null

	INSERT INTO [Log2] (
            [fUser]
           ,[Screen]
           ,[Ref]
           ,[Field]
           ,[OldVal]
           ,[NewVal])
    Select @UpdatedBy
		, 'User'
		, ur.UserId
		, 'Applying User Role Permission'
		, case u.ApplyUserRolePermission when 2 then 'Override' when 1 then 'Merge' else 'None' end
		, case un.ApplyUserRolePermission when 2 then 'Override' when 1 then 'Merge' else 'None' end
	from tblUserRole ur 
	inner join @Users un on un.UserID = ur.UserId
	inner join tblUser u on u.ID = un.UserID
	where u.ApplyUserRolePermission != un.ApplyUserRolePermission

	-- Updated logs for user's permission
	DECLARE @cursorUserId int, @cursorApplyUserRolePermission smallint
	DECLARE @uTickectPer varchar(6) = Substring(@ScheduleBoardPermission, 1, 4) + Substring(@TicketPermission, 5, 2)
	DECLARE
		@newAPVendor VARCHAR(4) = 'NNNN'
		,@newAPBill VARCHAR(4) = 'NNNN'
		,@newAPBillPay VARCHAR(4) = 'NNNN' 
		,@newCustomerPermissions VARCHAR(4) = 'NNNN'
		,@newLocationrPermissions VARCHAR(4) = 'NNNN'
		,@newProjectPermissions VARCHAR(4) = 'NNNN'
		,@newMSAuthorisedDeviceOnly INT = 0
		,@newTicketDelete VARCHAR(1) = 'N'
		,@newProjectListPermission NCHAR(1) = 'N'
		,@newFinancePermission NCHAR(1) = 'N'
		,@newBOMPermission NCHAR(4) = 'NNNN'
		,@newWIPPermission NCHAR(4) = 'NNNNNN'
		,@newMilestonesPermission NCHAR(4) = 'NNNN'
		,@newInventoryItemPermissions VARCHAR(10) = 'NNNNNN'
		,@newInventoryAdjustmentPermissions VARCHAR(10) = 'NNNNNN'
		,@newInventoryWarehousePermissions VARCHAR(10) = 'NNNNNN'
		,@newInventorysetupPermissions VARCHAR(10) = 'NNNNNN'
		,@newInventoryFinancePermissions VARCHAR(10) = 'NNNNNN'
		,@newDocumentPermission NCHAR(4) = 'NNNN'
		,@newContactPermission NCHAR(4) = 'NNNN'
		,@newProjecttempPermission NCHAR(4) = 'NNNN'
		,@newNotificationOnAddOpportunity BIT = 0
		,@newVendorsPermission NCHAR(4) = 'NNNN'
		,@newInvoicePermission VARCHAR(4) = 'NNNN'
		,@newBillingCodesPermission VARCHAR(4) = 'NNNN'
		,@newPOPermission VARCHAR(4) = 'NNNN'
		,@newPurchasingmodulePermission CHAR(1) = 'N' 
		,@newBillingmodulePermission CHAR(1) = 'N'
		,@newRPOPermission VARCHAR(4) = 'NNNN'
		,@newAccountPayablemodulePermission CHAR(1) = 'N' 
		,@newPaymentHistoryPermission VARCHAR(4) = 'NNNN'
		,@newCustomermodulePermission CHAR(1) = 'N' 
		,@newApplyPermissions  VARCHAR(4) = 'NNNN'
		,@newDepositPermissions  VARCHAR(4) = 'NNNN'
		,@newCollectionsPermissions  VARCHAR(4) = 'NNNN'
		,@newFinancialmodule CHAR(1) = 'N' 
		,@newChartPermissions  VARCHAR(4) = 'NNNN'
		,@newJournalEntryPermissions  VARCHAR(10) = 'NNNN'
		,@newBankReconciliationPermissions  VARCHAR(10) = 'NNNN'
		,@newRCmodulePermission CHAR(1) = 'N' 
		,@newProcessRCPermission  VARCHAR(4) = 'NNNN'
		,@newProcessC  VARCHAR(4) = 'NNNN'
		,@newProcessT  VARCHAR(4) = 'NNNN'	
		,@newSafetyTestsPermission  VARCHAR(4) = 'NNNN'	
		,@newRCRenewEscalatePermission VARCHAR(4) = 'NNNN'
		,@newSchedulemodule CHAR(1) = 'N' 
		,@newScheduleboardPermission VARCHAR(6) = 'NNNNN'
		,@newTicketPermission VARCHAR(6) = 'NNNNN'
		,@newTicketResolvedPermission VARCHAR(6) = 'NNNNN'
		,@newMTimesheetPermission VARCHAR(6) = 'NNNNN'
		,@newETimesheetPermission VARCHAR(6) = 'NNNNN'
		,@newMapRPermission VARCHAR(6) = 'NNNNN'
		,@newRouteBuilderPermission VARCHAR(6) = 'NNNNN'
		,@newMassTimesheetCheck CHAR(1) = 'N'
		,@newCreditHold  VARCHAR(4) = 'NNNN'
		,@newCreditFlag  VARCHAR(4) = 'NNNN'
		,@newSalesPermission VARCHAR(6) = 'NNNNN' 
		,@newTasksPermission SMALLINT = 0
		,@newCompleteTasksPermission SMALLINT = 0
		,@newFollowUpPermission VARCHAR(6) = 'NNNNN'
		,@newProposalPermission VARCHAR(6) = 'NNNNN'
		,@newEstimatePermission VARCHAR(6) = 'NNNNN'
		,@newConvertEstimatePermission VARCHAR(6) = 'NNNNN'
		,@newSalesSetupPermission VARCHAR(6) = 'NNNNN'
		,@newPONotification Char(1) = 'N'
		,@newJobClosePermission CHAR(6) = 'NNNNN'	
		,@newInventoryModulePermission CHAR(1) = 'N'	
		,@newProjectModulePermission CHAR(1) = 'N'
		,@newJobCompletedPermission CHAR(1) = 'N'
		,@newJobReopenPermission CHAR(1) = 'N'
		,@newWriteOff VARCHAR(6)='NNNNNN'
		,@newElevator VARCHAR(10)
		,@newSalesManager VARCHAR(1)
		,@newFinanceState VARCHAR(10) = 'NNNNNN'
		,@newEmployee VARCHAR(10)
		,@newTC VARCHAR(6)
		,@newMassReview SMALLINT
		,@newProgFunctions CHAR(1)
		,@newAccessUser CHAR(1)
		,@newBillSelect VARCHAR(10) = 'NNNNNN'
		,@newDispatchCheck CHAR(1)
		,@newPOLimit NUMERIC(30, 2)
		,@newPOApprove SMALLINT = 0
		,@newPOApproveAmt SMALLINT = 0
		,@newMinAmount NUMERIC(30, 2)
		,@newMaxAmount NUMERIC(30, 2)
		,@newViolationPermission  VARCHAR(4) = 'NNNN'	
	

	DECLARE db_cursor_Users CURSOR FOR 
	SELECT UserID, ApplyUserRolePermission
	FROM @Users
	OPEN db_cursor_Users  
	FETCH NEXT FROM db_cursor_Users INTO 
			@cursorUserId, @cursorApplyUserRolePermission
	WHILE @@FETCH_STATUS = 0
	BEGIN  	
		-- case override
		IF(@cursorApplyUserRolePermission = 2)
		BEGIN
			SELECT @newMSAuthorisedDeviceOnly = u.MSAuthorisedDeviceOnly
				--, @newDispatchCheck = Substring(u.Dispatch, 5, 1)
			FROM tblUser u 
			WHERE u.ID = @cursorUserId

			Exec spUpdateUserPermissionLogs 	@APVendor 						
											,@APBill 						
											,@APBillPay 					
											,@CustomerPermissions 			
											,@LocationrPermissions 			
											,@ProjectPermissions 			
											,@newMSAuthorisedDeviceOnly		
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
											,@Dispatch
											,@POLimit
											,@POApprove
											,@POApproveAmt
											,@MinAmount
											,@MaxAmount
											,@cursorUserId						
											,@UpdatedBy 	
											,@ViolationPermission
		END
		ELSE IF (@cursorApplyUserRolePermission = 1)
		BEGIN
			SELECT
				--Start for Role Permissions  
				  @newTicketPermission                        =   dbo.MergePermissionString(Dispatch, @TicketPermission)
				, @newLocationrPermissions                       =   dbo.MergePermissionString(Location, @LocationrPermissions + 'NN')
				, @newPOPermission                             =   dbo.MergePermissionString(PO, @POPermission+'NN')
				, @newProgFunctions                        =   dbo.MergePermissionString(CONTROL, @ProgFunctions + 'NNNNN'						)									
				, @newAccessUser                          =   dbo.MergePermissionString(UserS, @AccessUser + 'NNNNN'							)
				, @newScheduleboardPermission                         =   dbo.MergePermissionString(Ticket, @uTickectPer									)
				, @newSalesPermission                          =   dbo.MergePermissionString(Sales                         , @SalesPermission								)
				, @newMassReview                     =   dbo.MergePermissionInt(MassReview                    , @MassReview									)
				, @newEmployee                       =   dbo.MergePermissionString(Employee                      , @Employee										)
				, @newTC                             =   dbo.MergePermissionString(TC                            , @TC											)
				, @newElevator                       =   dbo.MergePermissionString(Elevator                      , @Elevator										)
				, @newChartPermissions                          =   dbo.MergePermissionString(Chart                         , @ChartPermissions+ 'NN'						)
				, @newJournalEntryPermissions                          =   dbo.MergePermissionString(GLAdj                         , @JournalEntryPermissions + 'NN'				)
				, @newDepositPermissions                        =   dbo.MergePermissionString(Deposit                       , @DepositPermissions + 'NN'					)
				--, @newCustomerPayment                =   dbo.MergePermissionString(CustomerPayment               , @CustomerPayment								)
				, @newFinanceState                      =   dbo.MergePermissionString(Financial                     , @FinanceState									)
				, @newAPVendor                         =   dbo.MergePermissionString(Vendor                        , @APVendor  + 'NN'								)
				, @newAPBill                           =   dbo.MergePermissionString(Bill                          , @APBill  + 'NN'								)
				, @newAPBillPay                        =   dbo.MergePermissionString(BillPay                       , @APBillPay  + 'NN'							)
				, @newProjectPermissions                            =   dbo.MergePermissionString(Job                           , @ProjectPermissions + 'NN'					)
				, @newCustomerPermissions                          =   dbo.MergePermissionString(OWNER                         , @CustomerPermissions + 'NN'					)
				, @newProjectListPermission          =   dbo.MergePermissionString(ProjectListPermission         , @ProjectListPermission						)
				, @newFinancePermission              =   dbo.MergePermissionString(FinancePermission             , @FinancePermission							)
				, @newBOMPermission                  =   dbo.MergePermissionString(BOMPermission                 , @BOMPermission								)
				, @newWIPPermission                  =   dbo.MergePermissionString(WIPPermission                 , @WIPPermission								)
				, @newMilestonesPermission           =   dbo.MergePermissionString(MilestonesPermission          , @MilestonesPermission							)
				, @newInventoryItemPermissions                           =   dbo.MergePermissionString(Item                          , @InventoryItemPermissions + 'NN'				)
				, @newInventoryAdjustmentPermissions                         =   dbo.MergePermissionString(InvAdj                        , @InventoryAdjustmentPermissions + 'NN'		)
				, @newInventoryWarehousePermissions                      =   dbo.MergePermissionString(Warehouse                     , @InventoryWarehousePermissions + 'NN'			)
				, @newInventorysetupPermissions                       =   dbo.MergePermissionString(InvSetup                      , @InventorysetupPermissions + 'NN'				)
				, @newInventoryFinancePermissions                      =   dbo.MergePermissionString(InvViewer                     , @InventoryFinancePermissions + 'NN'			)
				, @newDocumentPermission             =   dbo.MergePermissionString(DocumentPermission            , @DocumentPermission							)
				, @newContactPermission              =   dbo.MergePermissionString(ContactPermission             , @ContactPermission							)
				--, @newSalesAssigned                  =   dbo.MergePermissionInt(SalesAssigned                 , @SalesAssigned								)
				, @newProjecttempPermission				 =   dbo.MergePermissionString(ProjecttempPermission         , @ProjecttempPermission						)
				, @newNotificationOnAddOpportunity			 =   dbo.MergePermissionInt(NotificationOnAddOpportunity     , @NotificationOnAddOpportunity					)
				, @newInvoicePermission				 =   dbo.MergePermissionString(Invoice                       , @InvoicePermission+'NN' 						)
				, @newBillingCodesPermission				 =   dbo.MergePermissionString(BillingCodesPermission        , @BillingCodesPermission						)
				, @newPurchasingmodulePermission 			 =   dbo.MergePermissionString(PurchasingmodulePermission    , @PurchasingmodulePermission 					)
				, @newBillingmodulePermission				 =   dbo.MergePermissionString(BillingmodulePermission       , @BillingmodulePermission						)
				, @newRPOPermission				 =   dbo.MergePermissionString(RPO                           , @RPOPermission+'NN'							)
				, @newAccountPayablemodulePermission		 =   dbo.MergePermissionString(AccountPayablemodulePermission, @AccountPayablemodulePermission				)
				, @newPaymentHistoryPermission				 =   dbo.MergePermissionString(PaymentHistoryPermission      , @PaymentHistoryPermission						)
				, @newCustomermodulePermission				 =   dbo.MergePermissionString(CustomermodulePermission      , @CustomermodulePermission						)
				, @newApplyPermissions				 =   dbo.MergePermissionString(Apply                         , @ApplyPermissions + 'NN'						)
				, @newCollectionsPermissions		 =   dbo.MergePermissionString(Collection                    , @CollectionsPermissions + 'NN'				)
				, @newBankReconciliationPermissions	 =   dbo.MergePermissionString(BankRec                       , @BankReconciliationPermissions + 'NN'			)
				, @newFinancialmodule						 =   dbo.MergePermissionString(FinancialmodulePermission     , @Financialmodule								)
				, @newRCmodulePermission					 =   dbo.MergePermissionString(RCmodulePermission            , @RCmodulePermission							)
				, @newProcessRCPermission			 =   dbo.MergePermissionString(ProcessRCPermission           , @ProcessRCPermission+'NN'						)
				, @newProcessC								 =   dbo.MergePermissionString(ProcessC                      , @ProcessC										)
				, @newProcessT								 =   dbo.MergePermissionString(ProcessT                      , @ProcessT										)
				, @newRCRenewEscalatePermission			 =   dbo.MergePermissionString(RCRenewEscalatePermission     , @RCRenewEscalatePermission					)
				, @newSafetyTestsPermission				 =   dbo.MergePermissionString(RCSafteyTest	                , @SafetyTestsPermission							)
				, @newSchedulemodule						 =   dbo.MergePermissionString(SchedulemodulePermission	    , @Schedulemodule								)
				, @newTicketResolvedPermission				 =   dbo.MergePermissionString(Resolve                       , @TicketResolvedPermission						)
				, @newMTimesheetPermission 				 =   dbo.MergePermissionString(MTimesheet                    , @MTimesheetPermission 						)
				, @newETimesheetPermission					 =   dbo.MergePermissionString(ETimesheet                    , @ETimesheetPermission							)
				, @newMapRPermission						 =   dbo.MergePermissionString(MapR                          , @MapRPermission								)
				, @newRouteBuilderPermission				 =   dbo.MergePermissionString(RouteBuilder                  , @RouteBuilderPermission						)
				, @newMassTimesheetCheck					 =   dbo.MergePermissionString(MassTimesheetCheck            , @MassTimesheetCheck							)
				, @newCreditHold							 =   dbo.MergePermissionString(CreditHold                    , @CreditHold									)
				, @newCreditFlag							 =   dbo.MergePermissionString(CreditFlag                    , @CreditFlag									)
				, @newSalesManager							 =   dbo.MergePermissionString(SalesManager                  , @SalesManager									)
				, @newTasksPermission						 =   dbo.MergePermissionInt(ToDo                             , @TasksPermission								)
				, @newCompleteTasksPermission				 =   dbo.MergePermissionInt(ToDoC                            , @CompleteTasksPermission						)
				, @newFollowUpPermission					 =   dbo.MergePermissionString(FU                            , @FollowUpPermission							)
				, @newProposalPermission					 =   dbo.MergePermissionString(Proposal                      , @ProposalPermission							)
				, @newEstimatePermission					 =   dbo.MergePermissionString(Estimates                     , @EstimatePermission							)
				, @newConvertEstimatePermission			 =   dbo.MergePermissionString(AwardEstimates                , @ConvertEstimatePermission					)
				, @newSalesSetupPermission 				 =   dbo.MergePermissionString(SalesSetup                    , @SalesSetupPermission 						)
				, @newPONotification 						 =   dbo.MergePermissionString(PONotification                , @PONotification 								)
				, @newJobClosePermission					 =   dbo.MergePermissionString(JobClose	                    , @JobClosePermission							)
				, @newInventoryModulePermission 			 =   dbo.MergePermissionString(InventoryModulePermission 	, @InventoryModulePermission 					)
				, @newProjectModulePermission 				 =   dbo.MergePermissionString(ProjectModulePermission       , @ProjectModulePermission 						)
				, @newJobCompletedPermission				 =   dbo.MergePermissionString(JobCompletedPermission        , @JobCompletedPermission						)
				, @newJobReopenPermission					 =   dbo.MergePermissionString(JobReopenPermission           , @JobReopenPermission							)
				, @newWriteOff				 =   dbo.MergePermissionString(WriteOff                      , @WriteOff + 'NNNNN'							)
				--, @newIsProjectManager						 =   dbo.MergePermissionInt(IsProjectManager                 , @IsProjectManager								)
				--, @newIsAssignedProject					 =   dbo.MergePermissionInt(IsAssignedProject                , @IsAssignedProject							)
				--, @newTicketVoidPermission					 =   dbo.MergePermissionInt(TicketVoidPermission             , @TicketVoidPermission							)
				--, @newPRProcess							 =   dbo.MergePermissionString(PRProcess                     , @PRProcess									)
				--, @newPRRegister							 =   dbo.MergePermissionString(PRRegister                    , @PRRegister									)
				--, @newPRReport								 =   dbo.MergePermissionString(PRReport                      , @PRReport										)
				--, @newPRWage 								 =   dbo.MergePermissionString(PRWage                        , @PRWage 										)
				--, @newPRDeduct								 =   dbo.MergePermissionString(PRDeduct                      , @PRDeduct										)
				--, @newPR									 =   dbo.MergePermissionInt(PR                               , @PR											)
				, @newPOLimit                        =   case when POLimit = 0 then @POLimit	else POLimit end
				, @newPOApproveAmt                   =   case when POApprove = 0 and @POApprove = 1 then @POApproveAmt else POApproveAmt end
				, @newMinAmount						=   case when POApprove = 0 and @POApprove = 1 then @MinAmount else MinAmount end
				, @newMaxAmount						=   case when POApprove = 0 and @POApprove = 1 then @MaxAmount else MaxAmount end
				, @newPOApprove                      =   dbo.MergePermissionInt(POApprove                     , @POApprove									)
				, @newMSAuthorisedDeviceOnly = u.MSAuthorisedDeviceOnly
				, @newDispatchCheck = dbo.MergePermissionString(SUBSTRING(Dispatch,5,1), @Dispatch)
				, @newViolationPermission				 =   dbo.MergePermissionString(ViolationPermission	                , @ViolationPermission						)
			FROM tblUser u 
			WHERE u.ID = @cursorUserId

			Exec spUpdateUserPermissionLogs 	
											 @newAPVendor 						
											,@newAPBill 						
											,@newAPBillPay 					
											,@newCustomerPermissions 			
											,@newLocationrPermissions 			
											,@newProjectPermissions 			
											,@newMSAuthorisedDeviceOnly		
											,@newTicketDelete 					
											,@newProjectListPermission			
											,@newFinancePermission 			
											,@newBOMPermission					
											,@newWIPPermission					
											,@newMilestonesPermission 			
											,@newInventoryItemPermissions 		
											,@newInventoryAdjustmentPermissions
											,@newInventoryWarehousePermissions	
											,@newInventorysetupPermissions		
											,@newInventoryFinancePermissions	
											,@newDocumentPermission			
											,@newContactPermission				
											,@newProjecttempPermission			
											,@newNotificationOnAddOpportunity	
											,@newVendorsPermission				
											,@newInvoicePermission				
											,@newBillingCodesPermission		
											,@newPOPermission					
											,@newPurchasingmodulePermission	
											,@newBillingmodulePermission		
											,@newRPOPermission					
											,@newAccountPayablemodulePermission
											,@newPaymentHistoryPermission		
											,@newCustomermodulePermission		
											,@newApplyPermissions 				
											,@newDepositPermissions			
											,@newCollectionsPermissions		
											,@newFinancialmodule				
											,@newChartPermissions				
											,@newJournalEntryPermissions		
											,@newBankReconciliationPermissions	
											,@newRCmodulePermission			
											,@newProcessRCPermission 			
											,@newProcessC						
											,@newProcessT						
											,@newSafetyTestsPermission			
											,@newRCRenewEscalatePermission		
											,@newSchedulemodule				
											,@newScheduleboardPermission		
											,@newTicketPermission				
											,@newTicketResolvedPermission		
											,@newMTimesheetPermission			
											,@newETimesheetPermission			
											,@newMapRPermission				
											,@newRouteBuilderPermission		
											,@newMassTimesheetCheck			
											,@newCreditHold					
											,@newCreditFlag					
											,@newSalesPermission				
											,@newTasksPermission				
											,@newCompleteTasksPermission		
											,@newFollowUpPermission			
											,@newProposalPermission			
											,@newEstimatePermission			
											,@newConvertEstimatePermission		
											,@newSalesSetupPermission 			
											,@newPONotification				
											,@newJobClosePermission			
											,@newInventoryModulePermission		
											,@newProjectModulePermission		
											,@newJobCompletedPermission		
											,@newJobReopenPermission 			
											,@newWriteOff						
											,@newElevator						
											,@newSalesManager					
											,@newFinanceState					
											,@newEmployee						
											,@newTC							
											,@newMassReview 					
											,@newProgFunctions					
											,@newAccessUser	
											,@newBillSelect
											,@newDispatchCheck
											,@newPOLimit
											,@newPOApprove
											,@newPOApproveAmt
											,@newMinAmount
											,@newMaxAmount
											,@cursorUserId						
											,@UpdatedBy
											,@newViolationPermission

			SET @newAPVendor 						= null
			SET @newAPBill 						= null
			SET @newAPBillPay 						= null
			SET @newCustomerPermissions 			= null
			SET @newLocationrPermissions 			= null
			SET @newProjectPermissions 			= null
			SET @newTicketDelete 					= null
			SET @newProjectListPermission			= null
			SET @newFinancePermission 				= null
			SET @newBOMPermission					= null
			SET @newWIPPermission					= null
			SET @newMilestonesPermission 			= null
			SET @newInventoryItemPermissions 		= null
			SET @newInventoryAdjustmentPermissions	= null
			SET @newInventoryWarehousePermissions	= null
			SET @newInventorysetupPermissions		= null
			SET @newInventoryFinancePermissions	= null
			SET @newDocumentPermission				= null
			SET @newContactPermission				= null
			SET @newProjecttempPermission			= null
			SET @newNotificationOnAddOpportunity	= null
			SET @newVendorsPermission				= null
			SET @newInvoicePermission				= null
			SET @newBillingCodesPermission			= null
			SET @newPOPermission					= null
			SET @newPurchasingmodulePermission		= null
			SET @newBillingmodulePermission		= null
			SET @newRPOPermission					= null
			SET @newAccountPayablemodulePermission	= null
			SET @newPaymentHistoryPermission		= null
			SET @newCustomermodulePermission		= null
			SET @newApplyPermissions 				= null
			SET @newDepositPermissions				= null
			SET @newCollectionsPermissions			= null
			SET @newFinancialmodule				= null
			SET @newChartPermissions				= null
			SET @newJournalEntryPermissions		= null
			SET @newBankReconciliationPermissions	= null
			SET @newRCmodulePermission				= null
			SET @newProcessRCPermission 			= null
			SET @newProcessC						= null
			SET @newProcessT						= null
			SET @newSafetyTestsPermission			= null
			SET @newRCRenewEscalatePermission		= null
			SET @newSchedulemodule					= null
			SET @newScheduleboardPermission		= null
			SET @newTicketPermission				= null
			SET @newTicketResolvedPermission		= null
			SET @newMTimesheetPermission			= null
			SET @newETimesheetPermission			= null
			SET @newMapRPermission					= null
			SET @newRouteBuilderPermission			= null
			SET @newMassTimesheetCheck				= null
			SET @newCreditHold						= null
			SET @newCreditFlag						= null
			SET @newSalesPermission				= null
			SET @newTasksPermission				= null
			SET @newCompleteTasksPermission		= null
			SET @newFollowUpPermission				= null
			SET @newProposalPermission				= null
			SET @newEstimatePermission				= null
			SET @newConvertEstimatePermission		= null
			SET @newSalesSetupPermission 			= null
			SET @newPONotification					= null
			SET @newJobClosePermission				= null
			SET @newInventoryModulePermission		= null
			SET @newProjectModulePermission		= null
			SET @newJobCompletedPermission			= null
			SET @newJobReopenPermission 			= null
			SET @newWriteOff						= null
			SET @newElevator						= null
			SET @newSalesManager					= null
			SET @newFinanceState					= null
			SET @newEmployee						= null
			SET @newTC								= null
			SET @newMassReview 					= null
			SET @newProgFunctions					= null
			SET @newAccessUser						= null
			SET @newBillSelect						= null
			SET @newPOLimit						= null
			SET @newPOApprove						= null
			SET @newPOApproveAmt					= null
			SET @newMinAmount						= null
			SET @newMaxAmount						= null
			SET @cursorUserId						= NULL
            SET @newViolationPermission			= null
		END

		FETCH NEXT FROM db_cursor_Users INTO
			@cursorUserId, @cursorApplyUserRolePermission
	END  
	CLOSE db_cursor_Users  
	DEALLOCATE db_cursor_Users


	-- End Logs for user -------------

	-- Get all users that have status is Override
	UPDATE u
	SET
		--Start for Role Permissions  
		Dispatch                       = @TicketPermission
		,Location                       = @LocationrPermissions + 'NN'
		,PO                             = @POPermission+'NN'
		,CONTROL                        = @ProgFunctions + 'NNNNN'
		,UserS                          = @AccessUser + 'NNNNN'
		,Ticket                         = Substring(@ScheduleBoardPermission, 1, 4) + Substring(@TicketPermission, 5, 2)
		,Sales                          = @SalesPermission
		,MassReview                     = @MassReview
		,Employee                       = @Employee
		,TC                             = @TC
		,Elevator                       = @Elevator
		,Chart                          = @ChartPermissions+ 'NN'
		,GLAdj                          = @JournalEntryPermissions + 'NN'
		,Deposit                        = @DepositPermissions + 'NN'
		,CustomerPayment                = @CustomerPayment
		,Financial                      = @FinanceState
		,Vendor                         = @APVendor  + 'NN'
		,Bill                           = @APBill  + 'NN'
		,BillPay                        = @APBillPay  + 'NN'
		,Job                            = @ProjectPermissions + 'NN'
		,OWNER                          = @CustomerPermissions + 'NN'
		,ProjectListPermission          = @ProjectListPermission
		,FinancePermission              = @FinancePermission
		,BOMPermission                  = @BOMPermission
		,WIPPermission                  = @WIPPermission
		,MilestonesPermission           = @MilestonesPermission
		,Item                           = @InventoryItemPermissions + 'NN'
		,InvAdj                         = @InventoryAdjustmentPermissions + 'NN'
		,Warehouse                      = @InventoryWarehousePermissions + 'NN'
		,InvSetup                       = @InventorysetupPermissions + 'NN'
		,InvViewer                      = @InventoryFinancePermissions + 'NN'
		,DocumentPermission             = @DocumentPermission
		,ContactPermission              = @ContactPermission
		,SalesAssigned                  = @SalesAssigned
		,ProjecttempPermission          = @ProjecttempPermission
		,NotificationOnAddOpportunity   = @NotificationOnAddOpportunity
		,POLimit                        = @POLimit
		,POApprove                      = @POApprove
		,POApproveAmt                   = @POApproveAmt
		,MinAmount                      = @MinAmount
		,MaxAmount                      = @MaxAmount
		,Invoice                        = @InvoicePermission+'NN' 
		,BillingCodesPermission         = @BillingCodesPermission
		,PurchasingmodulePermission     = @PurchasingmodulePermission 
		,BillingmodulePermission        = @BillingmodulePermission
		,RPO                            = @RPOPermission+'NN'
		,AccountPayablemodulePermission = @AccountPayablemodulePermission
		,PaymentHistoryPermission       = @PaymentHistoryPermission
		,CustomermodulePermission       = @CustomermodulePermission
		,Apply                          = @ApplyPermissions + 'NN'
		,Collection                     = @CollectionsPermissions + 'NN'
		,BankRec                        = @BankReconciliationPermissions + 'NN'
		,FinancialmodulePermission      = @Financialmodule
		,RCmodulePermission             = @RCmodulePermission
		,ProcessRCPermission            = @ProcessRCPermission+'NN'
		,ProcessC                       = @ProcessC
		,ProcessT                       = @ProcessT
		,RCRenewEscalatePermission      = @RCRenewEscalatePermission
		,RCSafteyTest	                = @SafetyTestsPermission
		,SchedulemodulePermission	    = @Schedulemodule
		,Resolve                        = @TicketResolvedPermission
		,MTimesheet                     = @MTimesheetPermission 
		,ETimesheet                     = @ETimesheetPermission
		,MapR                           = @MapRPermission
		,RouteBuilder                   = @RouteBuilderPermission
		,MassTimesheetCheck             = @MassTimesheetCheck
		,CreditHold                     = @CreditHold
		,CreditFlag                     = @CreditFlag
		,SalesManager                   = @SalesManager
		,ToDo                           = @TasksPermission
		,ToDoC                          = @CompleteTasksPermission
		,FU                             = @FollowUpPermission
		,Proposal                       = @ProposalPermission
		,Estimates                      = @EstimatePermission
		,AwardEstimates                 = @ConvertEstimatePermission
		,SalesSetup                     = @SalesSetupPermission 
		,PONotification                 = @PONotification 
		,JobClose	                    = @JobClosePermission	
		,InventoryModulePermission 	    = @InventoryModulePermission 	
		,ProjectModulePermission        = @ProjectModulePermission 
		,JobCompletedPermission         = @JobCompletedPermission
		,JobReopenPermission            = @JobReopenPermission
		,WriteOff                       = @WriteOff + 'NNNNN'
		,IsProjectManager               = @IsProjectManager
		,IsAssignedProject              = @IsAssignedProject
		,TicketVoidPermission           = @TicketVoidPermission
		,PRProcess                      = @PRProcess
		,PRRegister                     = @PRRegister
		,PRReport                       = @PRReport
		,PRWage                         = @PRWage 
		,PRDeduct                       = @PRDeduct
		,PR                             = @PR
		-- Update ApplyUserRolePermission
		,u.ApplyUserRolePermission = 2
		,u.LastUpdateDate = GETDATE()
		,ViolationPermission	                = @ViolationPermission
	FROM tblUser u
	INNER JOIN @Users u1 ON u.ID = u1.UserID
	--INNER JOIN tblUserRole ur ON ur.UserId = u1.UserID
	--INNER JOIN tblRole r ON r.Id = ur.RoleId
	WHERE u1.ApplyUserRolePermission = 2

	--DECLARE @uTickectPer varchar(6) = Substring(@ScheduleBoardPermission, 1, 4) + Substring(@TicketPermission, 5, 2)
	UPDATE u
	SET
		--Start for Role Permissions  
		Dispatch                        =   dbo.MergePermissionString(Dispatch, @TicketPermission)
		,Location                       =   dbo.MergePermissionString(Location, @LocationrPermissions + 'NN')
		,PO                             =   dbo.MergePermissionString(PO, @POPermission+'NN')
		,CONTROL                        =   dbo.MergePermissionString(CONTROL, @ProgFunctions + 'NNNNN'						)									
		,UserS                          =   dbo.MergePermissionString(UserS, @AccessUser + 'NNNNN'							)
		,Ticket                         =   dbo.MergePermissionString(Ticket, @uTickectPer									)
		,Sales                          =   dbo.MergePermissionString(Sales                         , @SalesPermission								)
		,MassReview                     =   dbo.MergePermissionInt(MassReview                    , @MassReview									)
		,Employee                       =   dbo.MergePermissionString(Employee                      , @Employee										)
		,TC                             =   dbo.MergePermissionString(TC                            , @TC											)
		,Elevator                       =   dbo.MergePermissionString(Elevator                      , @Elevator										)
		,Chart                          =   dbo.MergePermissionString(Chart                         , @ChartPermissions+ 'NN'						)
		,GLAdj                          =   dbo.MergePermissionString(GLAdj                         , @JournalEntryPermissions + 'NN'				)
		,Deposit                        =   dbo.MergePermissionString(Deposit                       , @DepositPermissions + 'NN'					)
		,CustomerPayment                =   dbo.MergePermissionString(CustomerPayment               , @CustomerPayment								)
		,Financial                      =   dbo.MergePermissionString(Financial                     , @FinanceState									)
		,Vendor                         =   dbo.MergePermissionString(Vendor                        , @APVendor  + 'NN'								)
		,Bill                           =   dbo.MergePermissionString(Bill                          , @APBill  + 'NN'								)
		,BillPay                        =   dbo.MergePermissionString(BillPay                       , @APBillPay  + 'NN'							)
		,Job                            =   dbo.MergePermissionString(Job                           , @ProjectPermissions + 'NN'					)
		,OWNER                          =   dbo.MergePermissionString(OWNER                         , @CustomerPermissions + 'NN'					)
		,ProjectListPermission          =   dbo.MergePermissionString(ProjectListPermission         , @ProjectListPermission						)
		,FinancePermission              =   dbo.MergePermissionString(FinancePermission             , @FinancePermission							)
		,BOMPermission                  =   dbo.MergePermissionString(BOMPermission                 , @BOMPermission								)
		,WIPPermission                  =   dbo.MergePermissionString(WIPPermission                 , @WIPPermission								)
		,MilestonesPermission           =   dbo.MergePermissionString(MilestonesPermission          , @MilestonesPermission							)
		,Item                           =   dbo.MergePermissionString(Item                          , @InventoryItemPermissions + 'NN'				)
		,InvAdj                         =   dbo.MergePermissionString(InvAdj                        , @InventoryAdjustmentPermissions + 'NN'		)
		,Warehouse                      =   dbo.MergePermissionString(Warehouse                     , @InventoryWarehousePermissions + 'NN'			)
		,InvSetup                       =   dbo.MergePermissionString(InvSetup                      , @InventorysetupPermissions + 'NN'				)
		,InvViewer                      =   dbo.MergePermissionString(InvViewer                     , @InventoryFinancePermissions + 'NN'			)
		,DocumentPermission             =   dbo.MergePermissionString(DocumentPermission            , @DocumentPermission							)
		,ContactPermission              =   dbo.MergePermissionString(ContactPermission             , @ContactPermission							)
		,SalesAssigned                  =   dbo.MergePermissionInt(SalesAssigned                 , @SalesAssigned								)
		,ProjecttempPermission          =   dbo.MergePermissionString(ProjecttempPermission         , @ProjecttempPermission						)
		,NotificationOnAddOpportunity   =   dbo.MergePermissionInt(NotificationOnAddOpportunity  , @NotificationOnAddOpportunity					)
		
		,Invoice                        =   dbo.MergePermissionString(Invoice                       , @InvoicePermission+'NN' 						)
		,BillingCodesPermission         =   dbo.MergePermissionString(BillingCodesPermission        , @BillingCodesPermission						)
		,PurchasingmodulePermission     =   dbo.MergePermissionString(PurchasingmodulePermission    , @PurchasingmodulePermission 					)
		,BillingmodulePermission        =   dbo.MergePermissionString(BillingmodulePermission       , @BillingmodulePermission						)
		,RPO                            =   dbo.MergePermissionString(RPO                           , @RPOPermission+'NN'							)
		,AccountPayablemodulePermission =   dbo.MergePermissionString(AccountPayablemodulePermission, @AccountPayablemodulePermission				)
		,PaymentHistoryPermission       =   dbo.MergePermissionString(PaymentHistoryPermission      , @PaymentHistoryPermission						)
		,CustomermodulePermission       =   dbo.MergePermissionString(CustomermodulePermission      , @CustomermodulePermission						)
		,Apply                          =   dbo.MergePermissionString(Apply                         , @ApplyPermissions + 'NN'						)
		,Collection                     =   dbo.MergePermissionString(Collection                    , @CollectionsPermissions + 'NN'				)
		,BankRec                        =   dbo.MergePermissionString(BankRec                       , @BankReconciliationPermissions + 'NN'			)
		,FinancialmodulePermission      =   dbo.MergePermissionString(FinancialmodulePermission     , @Financialmodule								)
		,RCmodulePermission             =   dbo.MergePermissionString(RCmodulePermission            , @RCmodulePermission							)
		,ProcessRCPermission            =   dbo.MergePermissionString(ProcessRCPermission           , @ProcessRCPermission+'NN'						)
		,ProcessC                       =   dbo.MergePermissionString(ProcessC                      , @ProcessC										)
		,ProcessT                       =   dbo.MergePermissionString(ProcessT                      , @ProcessT										)
		,RCRenewEscalatePermission      =   dbo.MergePermissionString(RCRenewEscalatePermission     , @RCRenewEscalatePermission					)
		,RCSafteyTest	                =   dbo.MergePermissionString(RCSafteyTest	               , @SafetyTestsPermission							)
		,SchedulemodulePermission	    =   dbo.MergePermissionString(SchedulemodulePermission	   , @Schedulemodule								)
		,Resolve                        =   dbo.MergePermissionString(Resolve                       , @TicketResolvedPermission						)
		,MTimesheet                     =   dbo.MergePermissionString(MTimesheet                    , @MTimesheetPermission 						)
		,ETimesheet                     =   dbo.MergePermissionString(ETimesheet                    , @ETimesheetPermission							)
		,MapR                           =   dbo.MergePermissionString(MapR                          , @MapRPermission								)
		,RouteBuilder                   =   dbo.MergePermissionString(RouteBuilder                  , @RouteBuilderPermission						)
		,MassTimesheetCheck             =   dbo.MergePermissionString(MassTimesheetCheck            , @MassTimesheetCheck							)
		,CreditHold                     =   dbo.MergePermissionString(CreditHold                    , @CreditHold									)
		,CreditFlag                     =   dbo.MergePermissionString(CreditFlag                    , @CreditFlag									)
		,SalesManager                   =   dbo.MergePermissionString(SalesManager                  , @SalesManager									)
		,ToDo                           =   dbo.MergePermissionInt(ToDo                          , @TasksPermission								)
		,ToDoC                          =   dbo.MergePermissionInt(ToDoC                         , @CompleteTasksPermission						)
		,FU                             =   dbo.MergePermissionString(FU                            , @FollowUpPermission							)
		,Proposal                       =   dbo.MergePermissionString(Proposal                      , @ProposalPermission							)
		,Estimates                      =   dbo.MergePermissionString(Estimates                     , @EstimatePermission							)
		,AwardEstimates                 =   dbo.MergePermissionString(AwardEstimates                , @ConvertEstimatePermission					)
		,SalesSetup                     =   dbo.MergePermissionString(SalesSetup                    , @SalesSetupPermission 						)
		,PONotification                 =   dbo.MergePermissionString(PONotification                , @PONotification 								)
		,JobClose	                    =   dbo.MergePermissionString(JobClose	                    , @JobClosePermission							)
		,InventoryModulePermission 	    =   dbo.MergePermissionString(InventoryModulePermission 	, @InventoryModulePermission 					)
		,ProjectModulePermission        =   dbo.MergePermissionString(ProjectModulePermission       , @ProjectModulePermission 						)
		,JobCompletedPermission         =   dbo.MergePermissionString(JobCompletedPermission        , @JobCompletedPermission						)
		,JobReopenPermission            =   dbo.MergePermissionString(JobReopenPermission           , @JobReopenPermission							)
		,WriteOff                       =   dbo.MergePermissionString(WriteOff                      , @WriteOff + 'NNNNN'							)
		,IsProjectManager               =   dbo.MergePermissionInt(IsProjectManager              , @IsProjectManager								)
		,IsAssignedProject              =   dbo.MergePermissionInt(IsAssignedProject             , @IsAssignedProject							)
		,TicketVoidPermission           =   dbo.MergePermissionInt(TicketVoidPermission          , @TicketVoidPermission							)
		,PRProcess                      =   dbo.MergePermissionString(PRProcess                     , @PRProcess									)
		,PRRegister                     =   dbo.MergePermissionString(PRRegister                    , @PRRegister									)
		,PRReport                       =   dbo.MergePermissionString(PRReport                      , @PRReport										)
		,PRWage                         =   dbo.MergePermissionString(PRWage                        , @PRWage 										)
		,PRDeduct                       =   dbo.MergePermissionString(PRDeduct                      , @PRDeduct										)
		,PR                             =   dbo.MergePermissionInt(PR                            , @PR)
		,POLimit                        =   case when POLimit = 0 then @POLimit	else POLimit end
		,POApproveAmt                   =   case when POApprove = 0 and @POApprove = 1 then @POApproveAmt else POApproveAmt end
		,MinAmount						=   case when POApprove = 0 and @POApprove = 1 then @MinAmount else MinAmount end
		,MaxAmount						=   case when POApprove = 0 and @POApprove = 1 then @MaxAmount else MaxAmount end
		,POApprove                      =   dbo.MergePermissionInt(POApprove                     , @POApprove									)
		-- Update ApplyUserRolePermission
		,u.ApplyUserRolePermission = 1
		,u.LastUpdateDate = GETDATE()
		,ViolationPermission	                =   dbo.MergePermissionString(ViolationPermission	               , @ViolationPermission				)
	FROM tblUser u
	INNER JOIN @Users u1 ON u.ID = u1.UserID
	--INNER JOIN tblUserRole ur ON ur.UserId = u1.UserID
	--INNER JOIN tblRole r ON r.Id = ur.RoleId
	WHERE u1.ApplyUserRolePermission = 1

	UPDATE u
	SET
		u.ApplyUserRolePermission = u1.ApplyUserRolePermission
		,u.LastUpdateDate = GETDATE()
	FROM tblUser u
	INNER JOIN @Users u1 ON u.ID = u1.UserID
	WHERE u1.ApplyUserRolePermission = 0

	UPDATE u
	SET
		u.ApplyUserRolePermission = 0
		,u.LastUpdateDate = GETDATE()
	FROM tblUser u 
	INNER JOIN tblUserRole ur ON ur.UserId = u.ID
	LEFT JOIN @Users u1 ON u1.UserID = u.ID
	WHERE u1.UserID is null

	-- Delete all the old equipment the group
	DELETE tblUserRole WHERE RoleId = @RoleId

	-- We only insert new users to role incase that role is Active (Status = 0)
	IF @Status = 0
	BEGIN
		DECLARE @UserIDErr varchar(50) = '';
		SELECT @UserIDErr=u1.fUser FROM @Users u 
		INNER JOIN tblUserRole ur ON u.UserID = ur.UserId
		INNER JOIN tblUser u1 ON u1.ID = ur.UserId
		-- Check in case user already 
		IF (ISNULL(@UserIDErr,'') = '')
		BEGIN
			-- And replace it by the new one
			INSERT INTO tblUserRole (RoleId, UserId, UpdatedBy, UpdatedDate) SELECT @RoleId, UserID, @UpdatedBy, GETDATE() FROM @Users
		END
		ELSE
		BEGIN
			DECLARE @errorMess varchar(Max) = 'User ' + @UserIDErr + ' already set role';
			RAISERROR (@errorMess,16,1)
			IF @@TRANCOUNT > 0
				ROLLBACK TRANSACTION
			RETURN
		END
	END

	------------------------ END INSERT Users to Role ------------------------
	IF @@ERROR <> 0
	BEGIN
		RAISERROR ('Error occured',16,1)
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		RETURN
	END

COMMIT TRANSACTION

SELECT @RoleId
