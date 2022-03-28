CREATE PROCEDURE [dbo].[spUpdateUserRolePermissionLogs]
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
	,@CreatedBy VARCHAR (50)
	,@ViolationPermission CHAR(4) = 'NNNN'	
AS

DECLARE @currElevator VARCHAR(10)
DECLARE @currSalesManager VARCHAR(1)
DECLARE @currFinanceState VARCHAR(10) = 'NNNNNN'
DECLARE @currEmployee VARCHAR(10)
DECLARE @currTC VARCHAR(6)
DECLARE @currMassReview SMALLINT
DECLARE @currTimestamFixed SMALLINT
DECLARE @currProgFunctions CHAR(1)
DECLARE @currAccessUser CHAR(1)
DECLARE @currExpenses CHAR(1)
DECLARE @currDispatchCheck CHAR(1)
DECLARE @currBillSelect VARCHAR(10) = 'NNNNNN'
DECLARE @currPOLimit NUMERIC(30, 2)
DECLARE @currPOApprove SMALLINT = 0
DECLARE @currPOApproveAmt SMALLINT = 0
DECLARE @currMinAmount NUMERIC(30, 2)
DECLARE @currMaxAmount NUMERIC(30, 2)


/* Start Logs */
-- Get current values before update
DECLARE @currAPVendor VARCHAR(4) = 'NNNN'
DECLARE @currAPBill VARCHAR(4) = 'NNNN'
--DECLARE @currAPBillSelect   SMALLINT
DECLARE @currAPBillPay VARCHAR(4) = 'NNNN' 
DECLARE @currCustomerPermissions VARCHAR(4) = 'NNNN'
DECLARE @currLocationrPermissions VARCHAR(4) = 'NNNN'
DECLARE @currProjectPermissions VARCHAR(4) = 'NNNN'
--DECLARE @currDeleteEquip SMALLINT = 1
--DECLARE @currViewEquip SMALLINT = 1
DECLARE @currMSAuthorisedDeviceOnly INT = 0
DECLARE @currTicketDelete VARCHAR(1) = 'N'
DECLARE @currProjectListPermission NCHAR(1) = 'N'
DECLARE @currFinancePermission NCHAR(1) = 'N'
DECLARE @currBOMPermission NCHAR(4) = 'NNNN'
DECLARE @currWIPPermission NCHAR(6) = 'NNNNNN'
DECLARE @currMilestonesPermission NCHAR(4) = 'NNNN'
DECLARE @currInventoryItemPermissions VARCHAR(10) = 'NNNNNN'
DECLARE @currInventoryAdjustmentPermissions VARCHAR(10) = 'NNNNNN'
DECLARE @currInventoryWarehousePermissions VARCHAR(10) = 'NNNNNN'
DECLARE @currInventorysetupPermissions VARCHAR(10) = 'NNNNNN'
DECLARE @currInventoryFinancePermissions VARCHAR(10) = 'NNNNNN'
DECLARE @currDocumentPermission NCHAR(4) = 'NNNN'
DECLARE @currContactPermission NCHAR(4) = 'NNNN'
DECLARE @currProjecttempPermission NCHAR(4) = 'NNNN'
DECLARE @currNotificationOnAddOpportunity BIT = 0
DECLARE @currVendorsPermission NCHAR(4) = 'NNNN'
DECLARE @currInvoicePermission VARCHAR(4) = 'NNNN'
DECLARE @currBillingCodesPermission VARCHAR(4) = 'NNNN'
DECLARE @currPOPermission VARCHAR(4) = 'NNNN'
DECLARE @currPurchasingmodulePermission CHAR(1) = 'N' 
DECLARE @currBillingmodulePermission CHAR(1) = 'N'
DECLARE @currRPOPermission VARCHAR(4) = 'NNNN'
DECLARE @currAccountPayablemodulePermission CHAR(1) = 'N' 
DECLARE @currPaymentHistoryPermission VARCHAR(4) = 'NNNN'
DECLARE @currCustomermodulePermission CHAR(1) = 'N' 
DECLARE @currApplyPermissions  VARCHAR(4) = 'NNNN'
DECLARE @currDepositPermissions  VARCHAR(4) = 'NNNN'
DECLARE @currCollectionsPermissions  VARCHAR(4) = 'NNNN'
DECLARE @currFinancialmodule CHAR(1) = 'N' 
DECLARE @currChartPermissions  VARCHAR(4) = 'NNNN'
DECLARE @currJournalEntryPermissions  VARCHAR(10) = 'NNNN'
DECLARE @currBankReconciliationPermissions  VARCHAR(10) = 'NNNN'
DECLARE @currRCmodulePermission CHAR(1) = 'N' 
DECLARE @currProcessRCPermission  VARCHAR(4) = 'NNNN'
DECLARE @currProcessC  VARCHAR(4) = 'NNNN'
DECLARE @currProcessT  VARCHAR(4) = 'NNNN'	
DECLARE @currSafetyTestsPermission  VARCHAR(4) = 'NNNN'	
DECLARE @currRCRenewEscalatePermission VARCHAR(4) = 'NNNN'
DECLARE @currSchedulemodule CHAR(1) = 'N' 
DECLARE @currScheduleboardPermission VARCHAR(6) = 'NNNNN'
DECLARE @currTicketPermission VARCHAR(6) = 'NNNNNN'
DECLARE @currTicketResolvedPermission VARCHAR(6) = 'NNNNN'
DECLARE @currMTimesheetPermission VARCHAR(6) = 'NNNNN'
DECLARE @currETimesheetPermission VARCHAR(6) = 'NNNNN'
DECLARE @currMapRPermission VARCHAR(6) = 'NNNNN'
DECLARE @currRouteBuilderPermission VARCHAR(6) = 'NNNNN'
DECLARE @currMassTimesheetCheck CHAR(1) = 'N'
DECLARE @currCreditHold  VARCHAR(4) = 'NNNN'
DECLARE @currCreditFlag  VARCHAR(4) = 'NNNN'
DECLARE @currSalesPermission VARCHAR(6) = 'NNNNN' 
DECLARE @currTasksPermission SMALLINT = 0
DECLARE @currCompleteTasksPermission SMALLINT = 0
DECLARE @currFollowUpPermission VARCHAR(6) = 'NNNNN'
DECLARE @currProposalPermission VARCHAR(6) = 'NNNNN'
DECLARE @currEstimatePermission VARCHAR(6) = 'NNNNN'
DECLARE @currConvertEstimatePermission VARCHAR(6) = 'NNNNN'
DECLARE @currSalesSetupPermission VARCHAR(6) = 'NNNNN'
DECLARE @currPONotification Char(1) = 'N'
DECLARE @currJobClosePermission CHAR(6) = 'NNNNN'	
DECLARE @currInventoryModulePermission CHAR(1) = 'N'	
DECLARE @currProjectModulePermission CHAR(1) = 'N'
DECLARE @currJobCompletedPermission CHAR(1) = 'N'
DECLARE @currJobReopenPermission CHAR(1) = 'N'
DECLARE @currWriteOff VARCHAR(6)='NNNNNN'

SELECT 
	@currAPVendor = u.Vendor
	, @currAPBill = u.Bill
	, @currAPBillPay = BillPay
	, @currCustomerPermissions = OWNER
	, @currLocationrPermissions = Location
	, @currProjectPermissions = Job
	, @currElevator = Elevator
	--, @currMSAuthorisedDeviceOnly = MSAuthorisedDeviceOnly
	--, @currTicketDelete
	, @currProjectListPermission = ProjectListPermission
	, @currFinancePermission = FinancePermission
	, @currBOMPermission = BOMPermission
	, @currWIPPermission = WIPPermission
	, @currMilestonesPermission = MilestonesPermission
	, @currInventoryItemPermissions = Item
	, @currInventoryAdjustmentPermissions = InvAdj
	, @currInventoryWarehousePermissions = Warehouse
	, @currInventorysetupPermissions = InvSetup
	, @currInventoryFinancePermissions = InvViewer
	, @currDocumentPermission = DocumentPermission
	, @currContactPermission = ContactPermission
	, @currProjecttempPermission =ProjecttempPermission
	, @currNotificationOnAddOpportunity = NotificationOnAddOpportunity
	--, @currVendorsPermission
	, @currInvoicePermission 				= invoice
	, @currBillingCodesPermission 			= BillingCodesPermission
	, @currPOPermission 					= PO
	, @currPurchasingmodulePermission       = PurchasingmodulePermission
	, @currBillingmodulePermission 			= BillingmodulePermission
	, @currRPOPermission 					= RPO
	, @currAccountPayablemodulePermission 	= AccountPayablemodulePermission
	, @currPaymentHistoryPermission 		= PaymentHistoryPermission
	, @currCustomermodulePermission 		= CustomermodulePermission
	, @currApplyPermissions  				= Apply
	, @currDepositPermissions  				= Deposit
	, @currCollectionsPermissions  			= Collection
	, @currFinancialmodule 					= FinancialmodulePermission
	, @currChartPermissions  				= Chart
	, @currJournalEntryPermissions  		= GLAdj
	, @currBankReconciliationPermissions  	= BankRec
	, @currRCmodulePermission 				= RCmodulePermission
	, @currProcessRCPermission  			= ProcessRCPermission
	, @currProcessC 				 		= ProcessC
	, @currProcessT 						= ProcessT
	, @currSafetyTestsPermission 			= RCSafteyTest
	, @currRCRenewEscalatePermission		= RCRenewEscalatePermission
	, @currSchedulemodule 					= SchedulemodulePermission
	, @currScheduleboardPermission 			= Ticket
	--, @currTicketPermission					= Substring(Dispatch, 1, 5) + Substring(Ticket, 6, 1)
	, @currTicketPermission					= Dispatch
	, @currTicketResolvedPermission			= Resolve
	, @currMTimesheetPermission 			= MTimesheet
	, @currETimesheetPermission 			= ETimesheet
	, @currMapRPermission				 	= MapR
	, @currRouteBuilderPermission			= RouteBuilder
	, @currMassTimesheetCheck			 	= MassTimesheetCheck
	, @currCreditHold  						= CreditHold
	, @currCreditFlag  						= CreditFlag
	, @currSalesPermission					= u.Sales
	, @currTasksPermission 					= ToDo
	, @currCompleteTasksPermission 			= ToDoC
	, @currFollowUpPermission 				= FU
	, @currProposalPermission 				= Proposal
	, @currEstimatePermission 				= Estimates
	, @currConvertEstimatePermission		= AwardEstimates
	, @currSalesSetupPermission 			= SalesSetup
	, @currPONotification 					= PONotification
	, @currJobClosePermission 				= JobClose
	, @currInventoryModulePermission		= InventoryModulePermission
	, @currProjectModulePermission 			= ProjectModulePermission
	, @currJobCompletedPermission			= JobCompletedPermission
	, @currJobReopenPermission 				= JobReopenPermission
	, @currWriteOff 						= WriteOff 
	, @currElevator							= Elevator
	, @currSalesManager						= SalesManager
	, @currFinanceState						= Financial
	, @currEmployee							= Employee
	, @currTC								= TC
	, @currMassReview						= massreview
	--, @currTimestamFixed					= Times
	, @currProgFunctions					= CONTROL
	, @currAccessUser						= UserS
	, @currBillSelect						= BillSelect
	, @currDispatchCheck					= Substring(Dispatch, 5, 1)
	, @currPOLimit							= POLimit
	, @currPOApprove						= POApprove
	, @currPOApproveAmt						= POApproveAmt
	, @currMinAmount						= MinAmount 
	, @currMaxAmount						= MaxAmount 
	--, @currExpenses							= 
FROM tblRole u
WHERE u.ID = @UserRoleID

-- For logs
Declare @Screen varchar(100) = 'User Role';
Declare @RefId int;
Set @RefId = @UserRoleID;
/* Permissions */
-- Permissions - PO Limit
IF(ISNULL(@POLimit,0) != ISNULL(@currPOLimit,0))
BEGIN 	
	DECLARE @logPOLimit Varchar(40) = Convert(varchar(40),@POLimit)
	DECLARE @logCurrPOLimit Varchar(40) = Convert(varchar(40),@currPOLimit)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - PO Limit',@logCurrPOLimit,@logPOLimit
END
-- Permissions - Approve PO
IF(ISNULL(@POApprove,0) != ISNULL(@currPOApprove,0))
BEGIN
	IF(ISNULL(@POApprove,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Approve PO','No','Yes'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Approve PO','Yes','No'
END
-- Permissions - Approve PO Amount
IF(ISNULL(@POApproveAmt,-1) != ISNULL(@currPOApproveAmt,-1)) 
BEGIN
	DECLARE @logPOApproveAmt Varchar(50)
	DECLARE @logCurrPOApproveAmt Varchar(50)
	SET @logPOApproveAmt = CASE WHEN ISNULL(@POApproveAmt,-1) = 0 THEN 'Starting and max'
							WHEN ISNULL(@POApproveAmt, -1) = 1 THEN 'Greater than'
							ELSE '' END
							
	SET @logCurrPOApproveAmt = CASE WHEN ISNULL(@currPOApproveAmt,-1) = 0 THEN 'Starting and max'
							WHEN ISNULL(@currPOApproveAmt, -1) = 1 THEN 'Greater than'
							ELSE '' END
	
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Approve PO Amount',@logCurrPOApproveAmt,@logPOApproveAmt
END
-- Permissions - Min Amount
IF(ISNULL(@MinAmount,0) != ISNULL(@currMinAmount,0))
BEGIN 	
	DECLARE @logMinAmount Varchar(40) = Convert(varchar(40),@MinAmount)
	DECLARE @logCurrMinAmount Varchar(40) = Convert(varchar(40),@currMinAmount)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Min Amount',@logCurrMinAmount,@logMinAmount
END
-- Permissions - Max Amount
IF(ISNULL(@MaxAmount,0) != ISNULL(@currMaxAmount,0))
BEGIN 	
	DECLARE @logMaxAmount Varchar(40) = Convert(varchar(40),@MaxAmount)
	DECLARE @logCurrMaxAmount Varchar(40) = Convert(varchar(40),@currMaxAmount)
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Max Amount',@logCurrMaxAmount,@logMaxAmount
END
-- Customer
IF(ISNULL(@CustomermodulePermission,'N') != ISNULL(@currCustomermodulePermission,'N'))
BEGIN 	
	Declare @logCustomermodulePermission varchar(10) = ISNULL(@CustomermodulePermission,'N')
	Declare @logCurrCustomermodulePermission varchar(10) = ISNULL(@currCustomermodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Customers module',@logCurrCustomermodulePermission,@logCustomermodulePermission
END
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Customer',@currCustomerPermissions, @CustomerPermissions
-- Location
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Location', @currLocationrPermissions, @LocationrPermissions
-- Equipments
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Equipments', @currElevator, @Elevator
-- Receive Payment
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Receive Payment', @currApplyPermissions, @ApplyPermissions
-- Make Deposit
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Make Deposit', @currDepositPermissions, @DepositPermissions
-- Collections 
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Collections', @currCollectionsPermissions, @CollectionsPermissions, '4'
-- Credit Hold
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Credit Hold', @currCreditHold, @CreditHold, '4'
-- Credit Flag
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Credit Flag', @currCreditFlag, @CreditFlag, '4'
--IF(ISNULL(@CreditHold,'') = 'YYYY')
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Credit Hold','','Y'
--ELSE 
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Credit Hold','','N'
-- Write off
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Write off',@currwriteOff, @writeOff, '1'

-- Recurring module
IF(ISNULL(@RCmodulePermission,'N') != ISNULL(@currRCmodulePermission,'N'))
BEGIN 	
	Declare @logRCmodulePermission varchar(10) = ISNULL(@RCmodulePermission,'N')
	Declare @logCurrRCmodulePermission varchar(10) = ISNULL(@currRCmodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Recurring module',@logCurrRCmodulePermission,@logRCmodulePermission
END
-- Recurring Contracts
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Recurring Contracts', @currProcessRCPermission, @ProcessRCPermission
-- Safety Tests 
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Safety Tests', @currSafetyTestsPermission, @SafetyTestsPermission
-- Recurring Invoices @ProcessC
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Recurring Invoices',@currProcessC, @ProcessC, '134'
-- Recurring Tickets
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Recurring Tickets',@currProcessT, @ProcessT, '134'
-- Renew/Escalate
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Renew/Escalate', @currRCRenewEscalatePermission, @RCRenewEscalatePermission, '14'

-- Schedule module
IF(ISNULL(@Schedulemodule,'N') != ISNULL(@currSchedulemodule,'N'))
BEGIN 	
	Declare @logSchedulemodule varchar(10) = ISNULL(@Schedulemodule,'N')
	Declare @logCurrSchedulemodule varchar(10) = ISNULL(@currSchedulemodule,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Schedule module',@logCurrSchedulemodule,@logSchedulemodule
END
-- Ticket
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Ticket',@currTicketPermission, @TicketPermission, '12346'
-- Completed Ticket 
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Completed Ticket',@currTicketResolvedPermission, @TicketResolvedPermission
-- Mass Review Ticket @MassReview
IF (ISNULL(@MassReview,0) != ISNULL(@currMassReview,0))
BEGIN
	IF (ISNULL(@MassReview,0) = 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Mass Review Ticket','Y','N'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Mass Review Ticket','N','Y'
END
-- Mass Review Timesheet @MassTimesheetCheck
IF(ISNULL(@MassTimesheetCheck,'N') != ISNULL(@currMassTimesheetCheck,'N'))
BEGIN 	
	Declare @logMassTimesheetCheck varchar(10) = ISNULL(@MassTimesheetCheck,'N')
	Declare @logCurrMassTimesheetCheck varchar(10) = ISNULL(@currMassTimesheetCheck,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Mass Review Timesheet',@logCurrMassTimesheetCheck,@logMassTimesheetCheck
END
-- Schedule Board @ScheduleBoardPermission
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Schedule Board', @currScheduleBoardPermission, @ScheduleBoardPermission, '4'
-- Route Builder @RouteBuilderPermission
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Route Builder', @currRouteBuilderPermission, @RouteBuilderPermission, '4'
-- Timestamps Fixed @TimestamFixed
--IF (ISNULL(@TimestamFixed,0) = 0)
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Timestamps Fixed','','N'
--ELSE
--	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Timestamps Fixed','','Y'
-- Timesheet Entry @MTimesheetPermission
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Timesheet Entry', @currMTimesheetPermission, @MTimesheetPermission, '4'
-- e-Timesheet (Payroll data) @ETimesheetPermission
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - e-Timesheet (Payroll data)', @currETimesheetPermission, @ETimesheetPermission, '4'
-- Map @MapRPermission
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Map', @currMapRPermission, @MapRPermission,'4'

-- Project module
IF(ISNULL(@ProjectModulePermission,'N') != ISNULL(@currProjectModulePermission,'N'))
BEGIN 	
	Declare @logProjectModulePermission varchar(10) = ISNULL(@ProjectModulePermission,'N')
	Declare @logCurrProjectModulePermission varchar(10) = ISNULL(@currProjectModulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Project module',@logCurrProjectModulePermission,@logProjectModulePermission
END
-- Project
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project', @currProjectPermissions, @ProjectPermissions
-- Project Template
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project Template', @currProjecttempPermission, @ProjecttempPermission
-- BOM
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - BOM', @currBOMPermission, @BOMPermission
-- WIP
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - WIP', @currWIPPermission, @WIPPermission, '12346'
-- Milestones
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Billing', @currMilestonesPermission, @MilestonesPermission
-- Project status
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project status - Close', @currJobClosePermission, @JobClosePermission, '1'
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project status - Complete', @currJobCompletedPermission, @JobCompletedPermission, '1'
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project status - Reopen', @currJobReopenPermission, @JobReopenPermission, '1'
-- ProjectList Finance
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - ProjectList Finance', @currProjectListPermission, @ProjectListPermission, '1'
-- Project Finance 
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Project Finance', @currFinancePermission, @FinancePermission, '1'

-- Inventory module
IF(ISNULL(@InventoryModulePermission,'N') != ISNULL(@currInventoryModulePermission,'N'))
BEGIN 	
	Declare @logInventoryModulePermission varchar(10) = ISNULL(@InventoryModulePermission,'N')
	Declare @logCurrInventoryModulePermission varchar(10) = ISNULL(@currInventoryModulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Inventory module',@logCurrInventoryModulePermission,@logInventoryModulePermission
END
-- Inventory Item List @InventoryItemPermissions
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory Item List', @currInventoryItemPermissions, @InventoryItemPermissions
-- Inventory Adjustment
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory Adjustment', @currInventoryAdjustmentPermissions, @InventoryAdjustmentPermissions
-- Inventory WareHouse
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory WareHouse', @currInventoryWarehousePermissions, @InventoryWarehousePermissions
-- Inventory setup
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory setup', @currInventorysetupPermissions, @InventorysetupPermissions
-- Inventory Finance
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Inventory Finance', @currInventoryFinancePermissions, @InventoryFinancePermissions

-- Sales module
IF(ISNULL(@SalesManager,'N') != ISNULL(@currSalesManager,'N'))
BEGIN 	
	Declare @logSalesManager varchar(10) = ISNULL(@SalesManager,'N')
	Declare @logCurrSalesManager varchar(10) = ISNULL(@currSalesManager,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Sales module',@logCurrSalesManager,@logSalesManager
END
-- Leads 
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Leads', @currSalesPermission, @SalesPermission, '12346'
-- Opportunities
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Opportunities', @currProposalPermission, @ProposalPermission, '12346'
-- Estimate
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Estimate', @currEstimatePermission, @EstimatePermission, '12346'
-- Complete Task
IF(ISNULL(@CompleteTasksPermission,0) != ISNULL(@currCompleteTasksPermission,0))
BEGIN
	IF(ISNULL(@CompleteTasksPermission,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Complete Task','N','Y'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Complete Task','Y','N'
END
-- Task FollowUp 
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Task FollowUp', @currFollowUpPermission, @FollowUpPermission, '1'
-- Tasks
IF(ISNULL(@TasksPermission,0) != ISNULL(@currTasksPermission,0))
BEGIN
	IF(ISNULL(@TasksPermission,0) != 0)
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Tasks','N','Y'
	ELSE
		EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Tasks','Y','N'
END
-- Convert Estimate
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Convert Estimate', @currConvertEstimatePermission, @ConvertEstimatePermission, '1'
-- Sales Setup
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Sales Setup', @currConvertEstimatePermission, @ConvertEstimatePermission, '1'

-- AP module @AccountPayablemodulePermission
IF(ISNULL(@AccountPayablemodulePermission,'N') != ISNULL(@currAccountPayablemodulePermission,'N'))
BEGIN 	
	Declare @logAccountPayablemodulePermission varchar(10) = ISNULL(@AccountPayablemodulePermission,'N')
	Declare @logCurrAccountPayablemodulePermission varchar(10) = ISNULL(@currAccountPayablemodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - AP module',@logCurrAccountPayablemodulePermission,@logAccountPayablemodulePermission
END
-- Vendors
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Vendors',@currAPVendor, @APVendor
-- Bills
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Bills', @currAPBill, @APBill
-- Manage Checks
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Manage Checks', @currAPBillPay, @APBillPay

-- Financial module 
IF(ISNULL(@Financialmodule,'N') != ISNULL(@currFinancialmodule,'N'))
BEGIN 	
	Declare @logFinancialmodule varchar(10) = ISNULL(@Financialmodule,'N')
	Declare @logCurrFinancialmodule varchar(10) = ISNULL(@currFinancialmodule,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Financial module',@logCurrFinancialmodule,@logFinancialmodule
END
-- Chart of Accounts 
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Chart of Accounts', @currChartPermissions, @ChartPermissions
-- Journal Entry
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Journal Entry', @currJournalEntryPermissions, @JournalEntryPermissions
-- Bank Reconciliation
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Bank Reconciliation', @currBankReconciliationPermissions, @BankReconciliationPermissions
-- Financial Statement Module
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Financial Statement Module', @currFinanceState, @FinanceState, '6'

-- Billing module @BillingmodulePermission
IF(ISNULL(@BillingmodulePermission,'N') != ISNULL(@currBillingmodulePermission,'N'))
BEGIN 	
	Declare @logBillingmodulePermission varchar(10) = ISNULL(@BillingmodulePermission,'N')
	Declare @logCurrBillingmodulePermission varchar(10) = ISNULL(@currBillingmodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Billing module',@logCurrBillingmodulePermission,@logBillingmodulePermission
END
-- Invoices
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Invoices', @currInvoicePermission, @InvoicePermission
-- Billing Codes
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Billing Codes', @currBillingCodesPermission, @BillingCodesPermission
-- Online Payment
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Online Payment', @currPaymentHistoryPermission, @PaymentHistoryPermission, '4'

-- Purchasing module
IF(ISNULL(@PurchasingmodulePermission,'N') != ISNULL(@currPurchasingmodulePermission,'N'))
BEGIN 	
	Declare @logPurchasingmodulePermission varchar(10) = ISNULL(@PurchasingmodulePermission,'N')
	Declare @logCurrPurchasingmodulePermission varchar(10) = ISNULL(@currPurchasingmodulePermission,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Purchasing module',@logCurrPurchasingmodulePermission,@logPurchasingmodulePermission
END
-- PO
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - PO', @currPOPermission,@POPermission
-- Receive PO
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Receive PO', @currRPOPermission, @RPOPermission
-- PO Notification
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - PO Notification', @currPONotification, @PONotification, '1'

-- Program Module @ProgFunctions
IF(ISNULL(@ProgFunctions,'N') != ISNULL(@currProgFunctions,'N'))
BEGIN 	
	Declare @logProgFunctions varchar(10) = ISNULL(@ProgFunctions,'N')
	Declare @logCurrProgFunctions varchar(10) = ISNULL(@currProgFunctions,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Program Module',@logCurrProgFunctions,@logProgFunctions
END
-- Employee Maintenance @Employee
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Employee Maintenance', @currEmployee, @Employee, '4'
-- Users 
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Users', @currAccessUser, @AccessUser, '1'
-- Enter expenses @Expenses
--Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Enter expenses', @Expenses, '1'
-- Email Dispatch 
IF(ISNULL(@DispatchCheck,'N') != ISNULL(@currDispatchCheck,'N'))
BEGIN 	
	Declare @logDispatchCheck varchar(1) = ISNULL(@DispatchCheck,'N')
	Declare @logCurrDispatchCheck varchar(1) = ISNULL(@currDispatchCheck,'N')
	EXEC log2_insert @CreatedBy,@Screen,@RefId,'Permissions - Email Dispatch',@logCurrDispatchCheck,@logDispatchCheck
END
-- Document/Contact 
-- Document
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Document',@currDocumentPermission, @DocumentPermission
-- Contact
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Contact', @currContactPermission, @ContactPermission

-- Safety Tests 
Exec spUpdateCRUDUserRolePermissionLogs @CreatedBy, @RefId, 'Permissions - Violation', @currSafetyTestsPermission, @ViolationPermission,'13'
/* End Permissions */