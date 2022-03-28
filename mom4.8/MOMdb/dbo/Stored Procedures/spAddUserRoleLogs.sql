CREATE PROCEDURE [dbo].[spAddUserRoleLogs]
	@APVendor VARCHAR(4) = 'NNNN'
	,@APBill VARCHAR(4) = 'NNNN'
	--,@APBillSelect   SMALLINT
	,@APBillPay VARCHAR(4) = 'NNNN' 
	,@CustomerPermissions VARCHAR(4) = 'NNNN'
	,@LocationrPermissions VARCHAR(4) = 'NNNN'
	,@ProjectPermissions VARCHAR(4) = 'NNNN'
	--,@DeleteEquip SMALLINT = 1
	--,@ViewEquip SMALLINT = 1
	--,@MSAuthorisedDeviceOnly INT = 0
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
	,@ProjecttempPermission NCHAR(4) = 'NNNN'
	,@NotificationOnAddOpportunity BIT = 0
	,@VendorsPermission NCHAR(4) = 'NNNN'
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
	,@JournalEntryPermissions  VARCHAR(10) = 'NNNN'
	,@BankReconciliationPermissions  VARCHAR(10) = 'NNNN'
	,@RCmodulePermission CHAR(1) = 'N' 
	,@ProcessRCPermission  VARCHAR(4) = 'NNNN'
	,@ProcessC  VARCHAR(4) = 'NNNN'
	,@ProcessT  VARCHAR(4) = 'NNNN'	
	,@SafetyTestsPermission  VARCHAR(4) = 'NNNN'	
	,@RCRenewEscalatePermission VARCHAR(4) = 'NNNN'
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

	,@Elevator VARCHAR(10)
	,@SalesManager VARCHAR(1)
	,@FinanceState VARCHAR(10) = 'NNNNNN'
	,@Employee VARCHAR(10)
	,@TC VARCHAR(6)

	--,@addequip SMALLINT = 1
	--,@editequip SMALLINT = 1
	,@MassReview SMALLINT
	--,@TimestamFixed SMALLINT
	--,@SalesMgr SMALLINT
	--,@FStatement SMALLINT
	,@ProgFunctions CHAR(1)
	--,@EmployeeMainten SMALLINT
	,@AccessUser CHAR(1)
	,@BillSelect VARCHAR(10) = 'NNNNNN'
	,@DispatchCheck CHAR(1)

	,@POLimit NUMERIC(30, 2)
	,@POApprove SMALLINT = 0
	,@POApproveAmt SMALLINT = 0
	,@MinAmount NUMERIC(30, 2)
	,@MaxAmount NUMERIC(30, 2)

	--,@Expenses CHAR(1)
	,@UserRoleID int
	,@RoleName varchar(255)
	,@RoleDescription varchar(max)
	,@Status smallint
	,@CreatedBy VARCHAR (50)
	,@ViolationPermission VARCHAR(4)='NNNN'
AS

-- For logs
Declare @Screen varchar(100) = 'User Role';
Declare @RefId int;
Set @RefId = @UserRoleID;

EXEC log2_insert @CreatedBy,@Screen,@RefId,'User Role Name','',@RoleName

EXEC log2_insert @CreatedBy,@Screen,@RefId,'Description','',@RoleDescription


DECLARE @StatusStr varchar(20)
SELECT @StatusStr = CASE @Status WHEN 1 THEN 'Inactive' ELSE 'Active' END

EXEC log2_insert @CreatedBy,@Screen,@RefId,'Status','',@StatusStr

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

-- Customer
IF(ISNULL(@CustomermodulePermission,'N') != '')
BEGIN 	
	Declare @logCustomermodulePermission varchar(10) = ISNULL(@CustomermodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Customers module','',@logCustomermodulePermission
END
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Customer', @CustomerPermissions
-- Location
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Location', @LocationrPermissions
-- Equipments
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Equipments', @Elevator
-- Receive Payment
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Receive Payment', @ApplyPermissions
-- Make Deposit
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Make Deposit', @DepositPermissions
-- Collections 
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Collections', @CollectionsPermissions, '4'
-- Credit Hold
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Credit Hold', @CreditHold, '4'
-- Credit Flag
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Credit Flag', @CreditFlag, '4'
--IF(ISNULL(@CreditHold,'') = 'YYYY')
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Credit Hold','','Y'
--ELSE 
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Credit Hold','','N'
-- Write off
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Write off', @writeOff, '4'

-- Recurring module
IF(ISNULL(@RCmodulePermission,'N') != '')
BEGIN 	
	Declare @logRCmodulePermission varchar(10) = ISNULL(@RCmodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Recurring module','',@logRCmodulePermission
END
-- Recurring Contracts
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Recurring Contracts', @ProcessRCPermission
-- Safety Tests 
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Safety Tests', @SafetyTestsPermission
-- Recurring Invoices @ProcessC
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Recurring Invoices', @ProcessC, '134'
-- Recurring Tickets
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Recurring Tickets', @ProcessT, '134'
-- Renew/Escalate
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Renew/Escalate', @RCRenewEscalatePermission, '14'

-- Schedule module
IF(ISNULL(@Schedulemodule,'N') != '')
BEGIN 	
	Declare @logSchedulemodule varchar(10) = ISNULL(@Schedulemodule,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Schedule module','',@logSchedulemodule
END
-- Ticket
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Ticket', @TicketPermission, '12346'
-- Completed Ticket 
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Completed Ticket', @TicketResolvedPermission
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
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Schedule Board', @ScheduleBoardPermission, '4'
-- Route Builder @RouteBuilderPermission
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Route Builder', @RouteBuilderPermission, '4'
-- Timestamps Fixed @TimestamFixed
--IF (ISNULL(@TimestamFixed,0) = 0)
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Timestamps Fixed','','N'
--ELSE
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Timestamps Fixed','','Y'
-- Timesheet Entry @MTimesheetPermission
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Timesheet Entry', @MTimesheetPermission, '4'
-- e-Timesheet (Payroll data) @ETimesheetPermission
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - e-Timesheet (Payroll data)', @ETimesheetPermission, '4'
-- Map @MapRPermission
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Map', @MapRPermission,'4'

-- Project module
IF(ISNULL(@ProjectModulePermission,'N') != '')
BEGIN 	
	Declare @logProjectModulePermission varchar(10) = ISNULL(@ProjectModulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Project module','',@logProjectModulePermission
END
-- Project
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project', @ProjectPermissions
-- Project Template
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project Template', @ProjecttempPermission
-- BOM
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - BOM', @BOMPermission
-- WIP
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - WIP', @WIPPermission, '12346'
-- Milestones
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Billing', @MilestonesPermission
-- Project status
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project status - Close', @JobClosePermission, '1'
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project status - Complete', @JobCompletedPermission, '1'
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project status - Reopen', @JobReopenPermission, '1'
-- ProjectList Finance
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - ProjectList Finance', @ProjectListPermission, '1'
-- Project Finance 
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project Finance', @FinancePermission, '1'

-- Inventory module
IF(ISNULL(@InventoryModulePermission,'N') != '')
BEGIN 	
	Declare @logInventoryModulePermission varchar(10) = ISNULL(@InventoryModulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Inventory module','',@logInventoryModulePermission
END
-- Inventory Item List @InventoryItemPermissions
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory Item List', @InventoryItemPermissions
-- Inventory Adjustment
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory Adjustment', @InventoryAdjustmentPermissions
-- Inventory WareHouse
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory WareHouse', @InventoryWarehousePermissions
-- Inventory setup
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory setup', @InventorysetupPermissions
-- Inventory Finance
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory Finance', @InventoryFinancePermissions

-- Sales module
IF(ISNULL(@SalesManager,'N') != '')
BEGIN 	
	Declare @logSalesManager varchar(10) = ISNULL(@SalesManager,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Inventory module','',@logSalesManager
END
-- Leads 
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Leads', @SalesPermission, '12346'
-- Opportunities
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Opportunities', @ProposalPermission, '12346'
-- Estimate
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Estimate', @EstimatePermission, '12346'
-- Complete Task
IF(ISNULL(@CompleteTasksPermission,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Complete Task','','Y'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Complete Task','','N'
-- Task FollowUp 
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Task FollowUp', @FollowUpPermission, '1'
-- Tasks
IF(ISNULL(@TasksPermission,0) != 0)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Tasks','','Y'
ELSE
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Tasks','','N'
-- Convert Estimate
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Convert Estimate', @ConvertEstimatePermission, '1'
-- Sales Setup
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Sales Setup', @ConvertEstimatePermission, '1'

-- AP module @AccountPayablemodulePermission
IF(ISNULL(@AccountPayablemodulePermission,'N') != '')
BEGIN 	
	Declare @logAccountPayablemodulePermission varchar(10) = ISNULL(@AccountPayablemodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - AP module','',@logAccountPayablemodulePermission
END
-- Vendors
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Vendors', @APVendor
-- Bills
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Bills', @APBill
-- Manage Checks
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Manage Checks', @APBillPay

-- Financial module 
IF(ISNULL(@Financialmodule,'N') != '')
BEGIN 	
	Declare @logFinancialmodule varchar(10) = ISNULL(@Financialmodule,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Financial module','',@logFinancialmodule
END
-- Chart of Accounts 
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Chart of Accounts', @ChartPermissions
-- Journal Entry
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Journal Entry', @JournalEntryPermissions
-- Bank Reconciliation
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Bank Reconciliation', @BankReconciliationPermissions
-- Financial Statement Module
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Financial Statement Module', @FinanceState, '6'

-- Billing module @BillingmodulePermission
IF(ISNULL(@BillingmodulePermission,'N') != '')
BEGIN 	
	Declare @logBillingmodulePermission varchar(10) = ISNULL(@BillingmodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Billing module','',@logBillingmodulePermission
END
-- Invoices
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Invoices', @InvoicePermission
-- Billing Codes
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Billing Codes', @BillingCodesPermission
-- Online Payment
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Online Payment', @PaymentHistoryPermission, '4'

-- Purchasing module
IF(ISNULL(@PurchasingmodulePermission,'N') != '')
BEGIN 	
	Declare @logPurchasingmodulePermission varchar(10) = ISNULL(@PurchasingmodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Purchasing module','',@logPurchasingmodulePermission
END
-- PO
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - PO', @POPermission
-- Receive PO
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Receive PO', @RPOPermission
-- PO Notification
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - PO Notification', @PONotification, '1'

-- Program Module @ProgFunctions
IF(ISNULL(@ProgFunctions,'N') != '')
BEGIN 	
	Declare @logProgFunctions varchar(10) = ISNULL(@ProgFunctions,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Program Module','',@logProgFunctions
END
-- Employee Maintenance @Employee
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Employee Maintenance', @Employee, '4'
-- Users 
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Users', @AccessUser, '1'
-- Enter expenses @Expenses
--Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Enter expenses', @Expenses, '1'
-- Email Dispatch 
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Email Dispatch', @DispatchCheck, '1'

-- Document/Contact 
-- Document
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Document', @DocumentPermission
-- Contact
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Contact', @ContactPermission

-- Safety Tests 
Exec spInsertCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Violation', @ViolationPermission,'13'

/* End Permissions */