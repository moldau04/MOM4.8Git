using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace MOMWebApp
{
    public partial class AddUserRole : System.Web.UI.Page
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        BL_ReportsData objBL_Report = new BL_ReportsData();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["userid"] == null)
                {
                    Response.Redirect("login.aspx");
                }

                if (Request.QueryString["uid"] != null)// Edit
                {
                    if (Request.QueryString["t"] != null && Request.QueryString["t"].ToString().ToLower() == "c")
                    {
                        ViewState["mode"] = "copy";
                        lblHeader.Text = "Copy User Role";
                        Page.Title = "Copy User Role || MOM";
                    }
                    else
                    {
                        ViewState["mode"] = "edit";
                        lblHeader.Text = "Edit User Role";
                        Page.Title = "Edit User Role || MOM";
                        liLogs.Style["display"] = "inline-block";
                        tbLogs.Style["display"] = "block";
                    }
                }
                else
                {
                    ViewState["mode"] = "add";
                    lblHeader.Text = "Add User Role";
                    Page.Title = "Add User Role || MOM";
                    divApprovePo.Style["display"] = "none";
                    divMinAmount.Style["display"] = "none";
                    divMaxAmount.Style["display"] = "none";
                }

                GetControlForPayroll();

                if (!IsPostBack)
                {
                    if (Request.QueryString["uid"] != null)// Edit/Copy
                    {
                        UserRole userRole = new UserRole();
                        userRole.ConnConfig = Session["config"].ToString();
                        userRole.RoleID = Convert.ToInt32(Request.QueryString["uid"]);
                        var ds = objBL_User.GetRoleByID(userRole);
                        FillRoleInfo(ds.Tables[0]);
                        FillRolePermission(ds.Tables[0]);

                        if(Session["AddEditRole_SuccMessage"] != null)
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                            Session["AddEditRole_SuccMessage"] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkFirst_Click(object sender, EventArgs e)
        {

        }

        protected void lnkPrevious_Click(object sender, EventArgs e)
        {

        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {

        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {

        }

        protected void lnkClose_Click(object sender, EventArgs e)
        {
            Session["UserActiveTabId"] = 2;
            Response.Redirect("users.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    UserRole userRole = new UserRole();
                    userRole.RoleName = txtRoleName.Text;
                    userRole.RoleDescription = txtRoleDescription.Text;
                    userRole.Status = Convert.ToInt32(rbStatus.SelectedValue);
                    userRole.UserName = Session["Username"].ToString();
                    userRole.Users = GetSelectedUsersFromUI();
                    //objPropUser.Status = Convert.ToInt32(rbStatus.SelectedValue);
                    //GeneralFunctions objgn = new GeneralFunctions();

                    

                    //objPropUser.SalesAssigned = chkSalesAssigned.Checked ? true : false;
                    //objPropUser.NotificationOnAddOpportunity = chkNotification.Checked ? true : false;
                    //if (chkScheduleBrd.Checked == true)
                    //{
                    //    objPropUser.Schedule = 1;
                    //}
                    //else
                    //{
                    //    objPropUser.Schedule = 0;
                    //}

                    //if (chkMap.Checked == true)
                    //{
                    //    objPropUser.Mapping = 1;
                    //}
                    //else
                    //{
                    //    objPropUser.Mapping = 0;
                    //}


                    Decimal poLimit = 0;
                    Decimal.TryParse(txtPOLimit.Text, out poLimit);
                    objPropUser.POLimit = poLimit;
                    objPropUser.POApprove = Convert.ToInt16(ddlPOApprove.SelectedValue);

                    if (chkSalesperson.Checked == true)
                    {
                        objPropUser.Salesperson = 1;
                    }
                    else
                    {
                        objPropUser.Salesperson = 0;
                    }

                    objPropUser.SalesAssigned = chkSalesAssigned.Checked ? true : false;
                    objPropUser.NotificationOnAddOpportunity = chkNotification.Checked ? true : false;

                    if (objPropUser.POApprove == 0)
                    {
                        objPropUser.POApproveAmt = -1;
                        objPropUser.MinAmount = 0;
                        objPropUser.MaxAmount = 0;
                    }
                    else
                    {
                        objPropUser.POApproveAmt = Convert.ToInt16(ddlPOApproveAmt.SelectedValue);
                        if (objPropUser.POApproveAmt == 0)
                        {
                            objPropUser.MinAmount = !String.IsNullOrEmpty(txtMinAmount.Text) ? Convert.ToDecimal(txtMinAmount.Text) : 0;
                            objPropUser.MaxAmount = !String.IsNullOrEmpty(txtMaxAmount.Text) ? Convert.ToDecimal(txtMaxAmount.Text) : 0;
                            if (objPropUser.MaxAmount <= objPropUser.MinAmount)
                            {
                                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Max Amount must be greater than Min amount',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                return;
                            }
                        }
                        else if (objPropUser.POApproveAmt == 1)
                        {
                            objPropUser.MinAmount = !String.IsNullOrEmpty(txtMinAmount.Text) ? Convert.ToDecimal(txtMinAmount.Text) : 0;
                            objPropUser.MaxAmount = 0;
                        }
                        else
                        {
                            objPropUser.MinAmount = 0;
                            objPropUser.MaxAmount = 0;
                        }
                    }


                    if (chkAccessUser.Checked == true)
                    {
                        objPropUser.AccessUser = "Y";
                    }
                    else
                    {
                        objPropUser.AccessUser = "N";
                    }


                    if (chkExpenses.Checked == true)
                    {
                        objPropUser.Expenses = "Y";
                    }
                    else
                    {
                        objPropUser.Expenses = "N";
                    }

                    if (chkLocationview.Checked == true)
                    {
                        objPropUser.LocationRemarks = "Y";
                    }
                    else
                    {
                        objPropUser.LocationRemarks = "N";
                    }

                    if (chkProgram.Checked == true)
                    {
                        objPropUser.ProgFunctions = "Y";
                    }
                    else
                    {
                        objPropUser.ProgFunctions = "N";
                    }

                    if (chkPOAdd.Checked == true)
                    {
                        objPropUser.PurchaseOrd = "Y";
                    }
                    else
                    {
                        objPropUser.PurchaseOrd = "N";
                    }



                    #region TicketPermissions



                    #endregion

                    #region Permission

                    ///      ProjectPermissions
                    string ProjectPermissions = string.Empty;
                    ProjectPermissions = chkProjectadd.Checked ? ProjectPermissions + "Y" : ProjectPermissions + "N";
                    ProjectPermissions = chkProjectEdit.Checked ? ProjectPermissions + "Y" : ProjectPermissions + "N";
                    ProjectPermissions = chkProjectDelete.Checked ? ProjectPermissions + "Y" : ProjectPermissions + "N";
                    ProjectPermissions = chkProjectView.Checked ? ProjectPermissions + "Y" : ProjectPermissions + "N";
                    objPropUser.ProjectPermissions = ProjectPermissions;

                    // Customer Module Permission
                    objPropUser.Customermodule = chkCustomerModule.Checked ? "Y" : "N";
                    ///CustomerPermissions
                    string CustomerPermissions = string.Empty;
                    CustomerPermissions = chkCustomeradd.Checked ? CustomerPermissions + "Y" : CustomerPermissions + "N";
                    CustomerPermissions = chkCustomeredit.Checked ? CustomerPermissions + "Y" : CustomerPermissions + "N";
                    CustomerPermissions = chkCustomerdelete.Checked ? CustomerPermissions + "Y" : CustomerPermissions + "N";
                    CustomerPermissions = chkCustomerview.Checked ? CustomerPermissions + "Y" : CustomerPermissions + "N";
                    objPropUser.CustomerPermissions = CustomerPermissions;

                    //LocationrPermissions
                    string LocationrPermissions = string.Empty;
                    LocationrPermissions = chkLocationadd.Checked ? LocationrPermissions + "Y" : LocationrPermissions + "N";
                    LocationrPermissions = chkLocationedit.Checked ? LocationrPermissions + "Y" : LocationrPermissions + "N";
                    LocationrPermissions = chkLocationdelete.Checked ? LocationrPermissions + "Y" : LocationrPermissions + "N";
                    LocationrPermissions = chkLocationview.Checked ? LocationrPermissions + "Y" : LocationrPermissions + "N";
                    objPropUser.LocationrPermissions = LocationrPermissions;

                    //Receive Payment Permissions
                    string ReceivePaymentPermissions = string.Empty;
                    ReceivePaymentPermissions = chkReceivePaymentAdd.Checked ? ReceivePaymentPermissions + "Y" : ReceivePaymentPermissions + "N";
                    ReceivePaymentPermissions = chkReceivePaymentEdit.Checked ? ReceivePaymentPermissions + "Y" : ReceivePaymentPermissions + "N";
                    ReceivePaymentPermissions = chkReceivePaymentDelete.Checked ? ReceivePaymentPermissions + "Y" : ReceivePaymentPermissions + "N";
                    ReceivePaymentPermissions = chkReceivePaymentView.Checked ? ReceivePaymentPermissions + "Y" : ReceivePaymentPermissions + "N";
                    objPropUser.ApplyPermissions = ReceivePaymentPermissions;

                    //DepositPermissions
                    string DepositPermissions = string.Empty;
                    DepositPermissions = chkMakeDepositAdd.Checked ? DepositPermissions + "Y" : DepositPermissions + "N";
                    DepositPermissions = chkMakeDepositEdit.Checked ? DepositPermissions + "Y" : DepositPermissions + "N";
                    DepositPermissions = chkMakeDepositDelete.Checked ? DepositPermissions + "Y" : DepositPermissions + "N";
                    DepositPermissions = chkMakeDepositView.Checked ? DepositPermissions + "Y" : DepositPermissions + "N";
                    objPropUser.DepositPermissions = DepositPermissions;


                    //CollectionsPermissions
                    string CollectionsPermissions = string.Empty;
                    CollectionsPermissions = chkCollectionsAdd.Checked ? CollectionsPermissions + "Y" : CollectionsPermissions + "N";
                    CollectionsPermissions = chkCollectionsEdit.Checked ? CollectionsPermissions + "Y" : CollectionsPermissions + "N";
                    CollectionsPermissions = chkCollectionsDelete.Checked ? CollectionsPermissions + "Y" : CollectionsPermissions + "N";
                    CollectionsPermissions = chkCollectionsView.Checked ? CollectionsPermissions + "Y" : CollectionsPermissions + "N";
                    CollectionsPermissions += "N";
                    CollectionsPermissions = chkCollectionsReport.Checked ? CollectionsPermissions + "Y" : CollectionsPermissions + "N";
                    objPropUser.CollectionsPermissions = CollectionsPermissions;


                    if (chkCreditHold.Checked == true)
                    {
                        objPropUser.CreditHoldPermission = "YYYY";
                    }
                    else
                    {
                        objPropUser.CreditHoldPermission = "NNNN";
                    }

                    if (chkCreditFlag.Checked == true)
                    {
                        objPropUser.CreditFlagPermission = "YYYY";
                    }
                    else
                    {
                        objPropUser.CreditFlagPermission = "NNNN";
                    }

                    //FinanceDataPermissions
                    string FinancePermission = string.Empty;
                    FinancePermission = chkViewFinance.Checked ? "Y" : "N";
                    objPropUser.FinancePermission = FinancePermission;

                    //ProjectListPermission
                    string ProjectListPermission = string.Empty;
                    ProjectListPermission = chkViewProjectList.Checked ? "Y" : "N";
                    objPropUser.ProjectListPermission = ProjectListPermission;

                    //BOMPermissions
                    string BOMPermissions = string.Empty;
                    BOMPermissions += chkAddBOM.Checked ? "Y" : "N";
                    BOMPermissions += chkEditBOM.Checked ? "Y" : "N";
                    BOMPermissions += chkDeleteBOM.Checked ? "Y" : "N";
                    BOMPermissions += chkViewBOM.Checked ? "Y" : "N";
                    objPropUser.BOMPermission = BOMPermissions;

                    //WIPPermissions
                    string WIPPermissions = string.Empty;
                    WIPPermissions += chkAddWIP.Checked ? "Y" : "N";
                    WIPPermissions += chkEditWIP.Checked ? "Y" : "N";
                    WIPPermissions += chkDeleteWIP.Checked ? "Y" : "N";
                    WIPPermissions += chkViewWIP.Checked ? "Y" : "N";
                    WIPPermissions += "N";
                    WIPPermissions += chkReportWIP.Checked ? "Y" : "N";
                    objPropUser.WIPPermission = WIPPermissions;

                    //MilestonesPermission
                    string MilestonesPermission = string.Empty;
                    MilestonesPermission += chkAddMilesStones.Checked ? "Y" : "N";
                    MilestonesPermission += chkEditMilesStones.Checked ? "Y" : "N";
                    MilestonesPermission += chkDeleteMilesStones.Checked ? "Y" : "N";
                    MilestonesPermission += chkViewMilesStones.Checked ? "Y" : "N";
                    objPropUser.MilestonesPermission = MilestonesPermission;

                    ///InventoryItemPermissions
                    string InventoryItemPermissions = string.Empty;
                    InventoryItemPermissions = chkInventoryItemadd.Checked ? InventoryItemPermissions + "Y" : InventoryItemPermissions + "N";
                    InventoryItemPermissions = chkInventoryItemedit.Checked ? InventoryItemPermissions + "Y" : InventoryItemPermissions + "N";
                    InventoryItemPermissions = chkInventoryItemdelete.Checked ? InventoryItemPermissions + "Y" : InventoryItemPermissions + "N";
                    InventoryItemPermissions = chkInventoryItemview.Checked ? InventoryItemPermissions + "Y" : InventoryItemPermissions + "N";
                    objPropUser.InventoryItemPermissions = InventoryItemPermissions;

                    ///InventoryAdjustmentPermissions
                    string InventoryAdjustmentPermissions = string.Empty;
                    InventoryAdjustmentPermissions = chkInventoryAdjustmentadd.Checked ? InventoryAdjustmentPermissions + "Y" : InventoryAdjustmentPermissions + "N";
                    InventoryAdjustmentPermissions = chkInventoryAdjustmentedit.Checked ? InventoryAdjustmentPermissions + "Y" : InventoryAdjustmentPermissions + "N";
                    InventoryAdjustmentPermissions = chkInventoryAdjustmentdelete.Checked ? InventoryAdjustmentPermissions + "Y" : InventoryAdjustmentPermissions + "N";
                    InventoryAdjustmentPermissions = chkInventoryAdjustmentview.Checked ? InventoryAdjustmentPermissions + "Y" : InventoryAdjustmentPermissions + "N";
                    objPropUser.InventoryAdjustmentPermissions = InventoryAdjustmentPermissions;

                    ///InventoryWarehousePermissions
                    string InventoryWarehousePermissions = string.Empty;
                    InventoryWarehousePermissions = chkInventoryWareHouseadd.Checked ? InventoryWarehousePermissions + "Y" : InventoryWarehousePermissions + "N";
                    InventoryWarehousePermissions = chkInventoryWareHouseedit.Checked ? InventoryWarehousePermissions + "Y" : InventoryWarehousePermissions + "N";
                    InventoryWarehousePermissions = chkInventoryWareHousedelete.Checked ? InventoryWarehousePermissions + "Y" : InventoryWarehousePermissions + "N";
                    InventoryWarehousePermissions = chkInventoryWareHouseview.Checked ? InventoryWarehousePermissions + "Y" : InventoryWarehousePermissions + "N";
                    objPropUser.InventoryWarehousePermissions = InventoryWarehousePermissions;

                    ///InventorysetupPermissions
                    string InventorysetupPermissions = string.Empty;
                    InventorysetupPermissions = chkInventorysetupadd.Checked ? InventorysetupPermissions + "Y" : InventorysetupPermissions + "N";
                    InventorysetupPermissions = chkInventoryItemedit.Checked ? InventorysetupPermissions + "Y" : InventorysetupPermissions + "N";
                    InventorysetupPermissions = chkInventorysetupdelete.Checked ? InventorysetupPermissions + "Y" : InventorysetupPermissions + "N";
                    InventorysetupPermissions = chkInventorysetupview.Checked ? InventorysetupPermissions + "Y" : InventorysetupPermissions + "N";
                    objPropUser.InventorysetupPermissions = InventorysetupPermissions;


                    //DocumentPermissions
                    string DocumentPermissions = string.Empty;
                    DocumentPermissions += chkDocumentAdd.Checked ? "Y" : "N";
                    DocumentPermissions += chkDocumentEdit.Checked ? "Y" : "N";
                    DocumentPermissions += chkDocumentDelete.Checked ? "Y" : "N";
                    DocumentPermissions += chkDocumentView.Checked ? "Y" : "N";
                    objPropUser.DocumentPermissions = DocumentPermissions;

                    //ContactPermissions
                    string ContactPermissions = string.Empty;
                    ContactPermissions += chkContactAdd.Checked ? "Y" : "N";
                    ContactPermissions += chkContactEdit.Checked ? "Y" : "N";
                    ContactPermissions += chkContactDelete.Checked ? "Y" : "N";
                    ContactPermissions += chkContactView.Checked ? "Y" : "N";
                    objPropUser.ContactPermission = ContactPermissions;

                    //VendorsPermissions
                    string VendorsPermissions = string.Empty;
                    VendorsPermissions += chkVendorsAdd.Checked ? "Y" : "N";
                    VendorsPermissions += chkVendorsEdit.Checked ? "Y" : "N";
                    VendorsPermissions += chkVendorsDelete.Checked ? "Y" : "N";
                    VendorsPermissions += chkVendorsView.Checked ? "Y" : "N";
                    objPropUser.VendorsPermission = VendorsPermissions;

                    ///InventoryFinancePermissions
                    string InventoryFinancePermissions = string.Empty;
                    InventoryFinancePermissions = chkInventoryFinanceAdd.Checked ? InventoryFinancePermissions + "Y" : InventoryFinancePermissions + "N";
                    InventoryFinancePermissions = chkInventoryFinanceedit.Checked ? InventoryFinancePermissions + "Y" : InventoryFinancePermissions + "N";
                    InventoryFinancePermissions = chkInventoryFinancedelete.Checked ? InventoryFinancePermissions + "Y" : InventoryFinancePermissions + "N";
                    InventoryFinancePermissions = chkInventoryFinanceview.Checked ? InventoryFinancePermissions + "Y" : InventoryFinancePermissions + "N";
                    objPropUser.InventoryFinancePermissions = InventoryFinancePermissions;

                    // Project Templates
                    string ProjectTempPermissions = string.Empty;
                    ProjectTempPermissions += chkProjectTempAdd.Checked ? "Y" : "N";
                    ProjectTempPermissions += chkProjectTempEdit.Checked ? "Y" : "N";
                    ProjectTempPermissions += chkProjectTempDelete.Checked ? "Y" : "N";
                    ProjectTempPermissions += chkProjectTempView.Checked ? "Y" : "N";
                    objPropUser.ProjectTempPermissions = ProjectTempPermissions;

                    // Financial Module Permission
                    objPropUser.Financialmodule = chkFinancialmodule.Checked ? "Y" : "N";
                    ///Chartof Account Permissions
                    string ChartPermissions = string.Empty;
                    ChartPermissions = chkChartAdd.Checked ? ChartPermissions + "Y" : ChartPermissions + "N";
                    ChartPermissions = chkChartEdit.Checked ? ChartPermissions + "Y" : ChartPermissions + "N";
                    ChartPermissions = chkChartDelete.Checked ? ChartPermissions + "Y" : ChartPermissions + "N";
                    ChartPermissions = chkChartView.Checked ? ChartPermissions + "Y" : ChartPermissions + "N";
                    objPropUser.ChartPermissions = ChartPermissions;

                    //LocationrPermissions
                    string JournalEntryPermissions = string.Empty;
                    JournalEntryPermissions = chkJournalEntryAdd.Checked ? JournalEntryPermissions + "Y" : JournalEntryPermissions + "N";
                    JournalEntryPermissions = chkJournalEntryEdit.Checked ? JournalEntryPermissions + "Y" : JournalEntryPermissions + "N";
                    JournalEntryPermissions = chkJournalEntryDelete.Checked ? JournalEntryPermissions + "Y" : JournalEntryPermissions + "N";
                    JournalEntryPermissions = chkJournalEntryView.Checked ? JournalEntryPermissions + "Y" : JournalEntryPermissions + "N";
                    objPropUser.JournalEntryPermissions = JournalEntryPermissions;

                    //Bank Reconciliation Permissions
                    string BankReconciliationPermissions = string.Empty;
                    BankReconciliationPermissions = chkBankAdd.Checked ? BankReconciliationPermissions + "Y" : BankReconciliationPermissions + "N";
                    BankReconciliationPermissions = chkBankEdit.Checked ? BankReconciliationPermissions + "Y" : BankReconciliationPermissions + "N";
                    BankReconciliationPermissions = chkBankDelete.Checked ? BankReconciliationPermissions + "Y" : BankReconciliationPermissions + "N";
                    BankReconciliationPermissions = chkBankView.Checked ? BankReconciliationPermissions + "Y" : BankReconciliationPermissions + "N";
                    objPropUser.BankReconciliationPermissions = BankReconciliationPermissions;

                    //Write off Permission
                    objPropUser.wirteOff = chkWriteOff.Checked ? "Y" : "N";
                    #region Billing module

                    objPropUser.Billingmodule = chkBillingmodule.Checked ? "Y" : "N";

                    //InvoivePermissions
                    string InvoivePermissions = string.Empty;
                    InvoivePermissions += chkInvoicesAdd.Checked ? "Y" : "N";
                    InvoivePermissions += chkInvoicesEdit.Checked ? "Y" : "N";
                    InvoivePermissions += chkInvoicesDelete.Checked ? "Y" : "N";
                    InvoivePermissions += chkInvoicesView.Checked ? "Y" : "N";
                    objPropUser.InvoivePermissions = InvoivePermissions;



                    //BillingCodesPermission
                    string BillingCodesPermission = string.Empty;
                    BillingCodesPermission += chkBillingcodesAdd.Checked ? "Y" : "N";
                    BillingCodesPermission += chkBillingcodesEdit.Checked ? "Y" : "N";
                    BillingCodesPermission += chkBillingcodesDelete.Checked ? "Y" : "N";
                    BillingCodesPermission += chkBillingcodesView.Checked ? "Y" : "N";
                    objPropUser.BillingCodesPermission = BillingCodesPermission;

                    //Payment HistoryPermission
                    string PaymentHistoryPermission = string.Empty;
                    PaymentHistoryPermission += chkPaymentHistoryAdd.Checked ? "Y" : "N";
                    PaymentHistoryPermission += chkPaymentHistoryEdit.Checked ? "Y" : "N";
                    PaymentHistoryPermission += chkPaymentHistoryDelete.Checked ? "Y" : "N";
                    PaymentHistoryPermission += chkPaymentHistoryView.Checked ? "Y" : "N";
                    objPropUser.PaymentHistoryPermission = PaymentHistoryPermission;

                    #endregion


                    #region Purchasing module
                    objPropUser.Purchasingmodule = chkPurchasingmodule.Checked ? "Y" : "N";

                    //POPermission
                    string POPermission = string.Empty;
                    POPermission += chkPOAdd.Checked ? "Y" : "N";
                    POPermission += chkPOEdit.Checked ? "Y" : "N";
                    POPermission += chkPODelete.Checked ? "Y" : "N";
                    POPermission += chkPOView.Checked ? "Y" : "N";
                    objPropUser.POPermission = POPermission;

                    //RPOPermission
                    string RPOPermission = string.Empty;
                    RPOPermission += chkRPOAdd.Checked ? "Y" : "N";
                    RPOPermission += chkRPOEdit.Checked ? "Y" : "N";
                    RPOPermission += chkRPODelete.Checked ? "Y" : "N";
                    RPOPermission += chkRPOView.Checked ? "Y" : "N";
                    objPropUser.RPOPermission = RPOPermission;

                    objPropUser.PONotification = chkPONotification.Checked ? "Y" : "N";

                    #endregion

                    #region AccountPayable module
                    objPropUser.AccountPayablemodule = chkAccountPayable.Checked ? "Y" : "N";

                    //BillPermission 
                    string BillPermission = string.Empty;
                    BillPermission += chkAddBills.Checked ? "Y" : "N";
                    BillPermission += chkEditBills.Checked ? "Y" : "N";
                    BillPermission += chkDeleteBills.Checked ? "Y" : "N";
                    BillPermission += chkViewBills.Checked ? "Y" : "N";
                    objPropUser.APBill = BillPermission;

                    //BillPayPermission
                    string BillPayPermission = string.Empty;
                    BillPayPermission += chkAddManageChecks.Checked ? "Y" : "N";
                    BillPayPermission += chkEditManageChecks.Checked ? "Y" : "N";
                    BillPayPermission += chkDeleteManageChecks.Checked ? "Y" : "N";
                    BillPayPermission += chkViewManageChecks.Checked ? "Y" : "N";
                    BillPayPermission += chkShowBankBalances.Checked ? "Y" : "N";
                    objPropUser.APBillPay = BillPayPermission;

                    //VendorPermission
                    string VendorPermission = string.Empty;
                    VendorPermission += chkVendorsAdd.Checked ? "Y" : "N";
                    VendorPermission += chkVendorsEdit.Checked ? "Y" : "N";
                    VendorPermission += chkVendorsDelete.Checked ? "Y" : "N";
                    VendorPermission += chkVendorsView.Checked ? "Y" : "N";
                    objPropUser.APVendor = VendorPermission;

                    #endregion

                    #region Recurring module
                    objPropUser.Recurringmodule = chkRecurring.Checked ? "Y" : "N";

                    //RecurringContractsPermission 
                    string RecurringContractsPermission = string.Empty;
                    RecurringContractsPermission += chkRecContractsAdd.Checked ? "Y" : "N";
                    RecurringContractsPermission += chkRecContractsEdit.Checked ? "Y" : "N";
                    RecurringContractsPermission += chkRecContractsDelete.Checked ? "Y" : "N";
                    RecurringContractsPermission += chkRecContractsView.Checked ? "Y" : "N";
                    objPropUser.RecurringContractsPermission = RecurringContractsPermission;

                    //RecurringTicketsPermission
                    string RecurringTicketsPermission = string.Empty;
                    RecurringTicketsPermission += chkRecTicketsAdd.Checked ? "Y" : "N";
                    RecurringTicketsPermission += chkRecTicketsEdit.Checked ? "Y" : "N";
                    RecurringTicketsPermission += chkRecTicketsDelete.Checked ? "Y" : "N";
                    RecurringTicketsPermission += chkRecTicketsView.Checked ? "Y" : "N";
                    objPropUser.ProcessT = RecurringTicketsPermission;

                    //RecurringInvoicesPermission
                    string RecurringInvoicesPermission = string.Empty;
                    RecurringInvoicesPermission += chkRecInvoicesAdd.Checked ? "Y" : "N";
                    RecurringInvoicesPermission += chkRecInvoicesEdit.Checked ? "Y" : "N";
                    RecurringInvoicesPermission += chkRecInvoicesDelete.Checked ? "Y" : "N";
                    RecurringInvoicesPermission += chkRecInvoicesView.Checked ? "Y" : "N";
                    objPropUser.ProcessC = RecurringInvoicesPermission;

                    //SafetyTestsPermission
                    string SafetyTestsPermission = string.Empty;
                    SafetyTestsPermission += chkSafetyTestsAdd.Checked ? "Y" : "N";
                    SafetyTestsPermission += chkSafetyTestsEdit.Checked ? "Y" : "N";
                    SafetyTestsPermission += chkSafetyTestsDelete.Checked ? "Y" : "N";
                    SafetyTestsPermission += chkSafetyTestsView.Checked ? "Y" : "N";
                    objPropUser.SafetyTestsPermission = SafetyTestsPermission;

                    //ViolationsPermission
                    string ViolationsPermission = string.Empty;
                    ViolationsPermission += chkViolationsAdd.Checked ? "Y" : "N";
                    ViolationsPermission += "N"; //Edit permission
                    ViolationsPermission += chkViolationsDelete.Checked ? "Y" : "N";
                    ViolationsPermission += "Y"; //View permission
                    objPropUser.ViolationPermission = ViolationsPermission;

                    //RenewEscalatePermission
                    string RenewEscalatePermission = string.Empty;
                    RenewEscalatePermission += chkRenewEscalateAdd.Checked ? "Y" : "N";
                    RenewEscalatePermission += chkRenewEscalateEdit.Checked ? "Y" : "N";
                    RenewEscalatePermission += chkRenewEscalateDelete.Checked ? "Y" : "N";
                    RenewEscalatePermission += chkRenewEscalateView.Checked ? "Y" : "N";
                    objPropUser.RenewEscalatePermission = RenewEscalatePermission;

                    #endregion

                    #region Schedule module
                    objPropUser.Schedulemodule = chkSchedule.Checked ? "Y" : "N";

                    /////ScheduleBoardPermission
                    ///
                    string ScheduleBoardPermission = string.Empty;
                    ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                    ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                    ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                    ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                    ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                    ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                    objPropUser.ScheduleBoardPermission = ScheduleBoardPermission;

                    //TicketPermission 
                    string TicketPermission = string.Empty;
                    TicketPermission += chkTicketListAdd.Checked ? "Y" : "N";//1
                    TicketPermission += chkTicketListEdit.Checked ? "Y" : "N";
                    TicketPermission += chkTicketListDelete.Checked ? "Y" : "N";
                    TicketPermission += chkTicketListView.Checked ? "Y" : "N";
                    TicketPermission += chkDispatch.Checked ? "Y" : "N";
                    TicketPermission += chkTicketListReport.Checked ? "Y" : "N";

                    objPropUser.TicketPermission = TicketPermission;

                    //TicketResolvedPermission 
                    string TicketResolvedPermission = string.Empty;
                    TicketResolvedPermission += chkResolveTicketAdd.Checked ? "Y" : "N";
                    TicketResolvedPermission += chkResolveTicketEdit.Checked ? "Y" : "N";
                    TicketResolvedPermission += chkResolveTicketDelete.Checked ? "Y" : "N";
                    TicketResolvedPermission += chkResolveTicketView.Checked ? "Y" : "N";
                    TicketResolvedPermission += "N";
                    TicketResolvedPermission += chkResolveTicketReport.Checked ? "Y" : "N";
                    objPropUser.TicketResolvedPermission = TicketResolvedPermission;


                    //objPropUser.TimeStamsFixedPermission = TimeStamsFixedPermission;

                    //Manual TimesheetPermission
                    string MTimesheetPermission = string.Empty;
                    MTimesheetPermission += chkTimesheetadd.Checked ? "Y" : "N";
                    MTimesheetPermission += chkTimesheetedit.Checked ? "Y" : "N";
                    MTimesheetPermission += chkTimesheetdelete.Checked ? "Y" : "N";
                    MTimesheetPermission += chkTimesheetview.Checked ? "Y" : "N";
                    MTimesheetPermission += "N";
                    MTimesheetPermission += chkTimesheetreport.Checked ? "Y" : "N";
                    objPropUser.MTimesheetPermission = MTimesheetPermission;

                    //E-TimesheetPermission
                    string ETimesheetPermission = string.Empty;
                    ETimesheetPermission += chkETimesheetadd.Checked ? "Y" : "N";
                    ETimesheetPermission += chkETimesheetedit.Checked ? "Y" : "N";
                    ETimesheetPermission += chkETimesheetdelete.Checked ? "Y" : "N";
                    ETimesheetPermission += chkETimesheetview.Checked ? "Y" : "N";
                    ETimesheetPermission += "N";
                    ETimesheetPermission += chkETimesheetreport.Checked ? "Y" : "N";
                    objPropUser.ETimesheetPermission = ETimesheetPermission;

                    //MapRPermission
                    string MapRPermission = string.Empty;
                    MapRPermission += chkMapAdd.Checked ? "Y" : "N";
                    MapRPermission += chkMapEdit.Checked ? "Y" : "N";
                    MapRPermission += chkMapDelete.Checked ? "Y" : "N";
                    MapRPermission += chkMapView.Checked ? "Y" : "N";
                    MapRPermission += "N";
                    MapRPermission += chkMapReport.Checked ? "Y" : "N";
                    objPropUser.MapRPermission = MapRPermission;

                    //RouteBuildePermission
                    string RouteBuildePermission = string.Empty;
                    RouteBuildePermission += chkRouteBuilderAdd.Checked ? "Y" : "N";
                    RouteBuildePermission += chkRouteBuilderEdit.Checked ? "Y" : "N";
                    RouteBuildePermission += chkRouteBuilderDelete.Checked ? "Y" : "N";
                    RouteBuildePermission += chkRouteBuilderView.Checked ? "Y" : "N";
                    RouteBuildePermission += "N";
                    RouteBuildePermission += chkRouteBuilderReport.Checked ? "Y" : "N";
                    objPropUser.RouteBuilderPermission = RouteBuildePermission;

                    //MassResolvePDATickets
                    objPropUser.MassTimesheetCheck = chkMassTimesheetCheck.Checked ? "Y" : "N";

                    //TicketVoidPermission
                    objPropUser.TicketVoidPermission = chkTicketVoidPermission.Checked ? 1 : 0;

                    // MassPayrollTicket
                    if (chkMassPayrollTicket.Visible)
                    {
                        objPropUser.MassPayrollTicket = chkMassPayrollTicket.Checked ? "Y" : "N";
                    }
                    else
                    {
                        objPropUser.MassPayrollTicket = chkMassPayrollTicket1.Checked ? "Y" : "N";
                    }
                    #endregion

                    #region Sales module

                    string LeadsPermission = string.Empty;
                    LeadsPermission += chkLeadAdd.Checked ? "Y" : "N";
                    LeadsPermission += chkLeadEdit.Checked ? "Y" : "N";
                    LeadsPermission += chkLeadDelete.Checked ? "Y" : "N";
                    LeadsPermission += chkLeadView.Checked ? "Y" : "N";
                    LeadsPermission += "N";
                    LeadsPermission += chkLeadReport.Checked ? "Y" : "N";
                    objPropUser.SalesPermission = LeadsPermission;

                    //MassResolvePDATickets
                    objPropUser.TasksPermission = chkTasks.Checked ? 1 : 0;

                    objPropUser.CompleteTasksPermission = chkCompleteTask.Checked ? 1 : 0;

                    objPropUser.FollowUpPermission = chkFollowUp.Checked ? "YYYYYY" : "NNNNNN";
                    //Opportunities  
                    string ProposalPermission = string.Empty;
                    ProposalPermission += chkOppAdd.Checked ? "Y" : "N";
                    ProposalPermission += chkOppEdit.Checked ? "Y" : "N";
                    ProposalPermission += chkOppDelete.Checked ? "Y" : "N";
                    ProposalPermission += chkOppView.Checked ? "Y" : "N";
                    ProposalPermission += "N";
                    ProposalPermission += chkOppReport.Checked ? "Y" : "N";
                    objPropUser.ProposalPermission = ProposalPermission;

                    //Estimate   
                    string EstimatePermission = string.Empty;
                    EstimatePermission += chkEstimateAdd.Checked ? "Y" : "N";
                    EstimatePermission += chkEstimateEdit.Checked ? "Y" : "N";
                    EstimatePermission += chkEstimateDelete.Checked ? "Y" : "N";
                    EstimatePermission += chkEstimateView.Checked ? "Y" : "N";
                    EstimatePermission += "N";
                    EstimatePermission += chkEstimateReport.Checked ? "Y" : "N";
                    objPropUser.EstimatePermission = EstimatePermission;

                    objPropUser.ConvertEstimatePermission = chkConvertEstimate.Checked ? "YYYYYY" : "NNNNNN";
                    objPropUser.SalesSetupPermission = chkSalesSetup.Checked ? "YYYYYY" : "NNNNNN";

                    objPropUser.EstApproveProposal = chkEstApprovalStatus.Checked ? true : false;
                    #endregion

                    #region payroll module

                    //Employee
                    string empPermission = string.Empty;
                    empPermission += empAdd.Checked ? "Y" : "N";
                    empPermission += empEdit.Checked ? "Y" : "N";
                    empPermission += empDelete.Checked ? "Y" : "N";
                    empPermission += empView.Checked ? "Y" : "N";
                    empPermission += "NN";
                    objPropUser.Employee = empPermission;


                    //Run Payroll

                    string RunPayrollPermission = string.Empty;
                    RunPayrollPermission += runAdd.Checked ? "Y" : "N";
                    RunPayrollPermission += runEdit.Checked ? "Y" : "N";
                    RunPayrollPermission += runDelete.Checked ? "Y" : "N";
                    RunPayrollPermission += runView.Checked ? "Y" : "N";
                    RunPayrollPermission += "NN";
                    objPropUser.PRProcess = RunPayrollPermission;

                    //payroll Check   
                    string PayrollCheckPermission = string.Empty;
                    PayrollCheckPermission += payrollchckAdd.Checked ? "Y" : "N";
                    PayrollCheckPermission += payrollchckEdit.Checked ? "Y" : "N";
                    PayrollCheckPermission += payrollchckDelete.Checked ? "Y" : "N";
                    PayrollCheckPermission += payrollchckView.Checked ? "Y" : "N";
                    PayrollCheckPermission += "NN";
                    objPropUser.PRRegister = PayrollCheckPermission;

                    //payroll Form   
                    string payrollformPermission = string.Empty;
                    payrollformPermission += payrollformAdd.Checked ? "Y" : "N";
                    payrollformPermission += payrollformEdit.Checked ? "Y" : "N";
                    payrollformPermission += payrollformDelete.Checked ? "Y" : "N";
                    payrollformPermission += payrollformView.Checked ? "Y" : "N";
                    payrollformPermission += "NN";
                    objPropUser.PRReport = payrollformPermission;

                    //Wages  
                    string wagesPermission = string.Empty;
                    wagesPermission += wagesadd.Checked ? "Y" : "N";
                    wagesPermission += wagesEdit.Checked ? "Y" : "N";
                    wagesPermission += wagesDelete.Checked ? "Y" : "N";
                    wagesPermission += wagesView.Checked ? "Y" : "N";
                    wagesPermission += "NN";
                    objPropUser.PRWage = wagesPermission;

                    //deductions 
                    string deductionsPermission = string.Empty;
                    deductionsPermission += deductionsAdd.Checked ? "Y" : "N";
                    deductionsPermission += deductionsEdit.Checked ? "Y" : "N";
                    deductionsPermission += deductionsDelete.Checked ? "Y" : "N";
                    deductionsPermission += deductionsView.Checked ? "Y" : "N";
                    deductionsPermission += "NN";
                    objPropUser.PRDeduct = deductionsPermission;

                    #endregion

                    objPropUser.PR = Convert.ToBoolean(payrollModulchck.Checked ? 1 : 0);

                    objPropUser.Projectmodule = chkProjectmodule.Checked ? "Y" : "N";

                    objPropUser.Inventorymodule = chkInventorymodule.Checked ? "Y" : "N";

                    objPropUser.JobClosePermission = chkJobClosePermission.Checked ? "YYYYYY" : "NNNNNN";

                    objPropUser.JobCompletedPermission = chkJobCompletedPermission.Checked ? "Y" : "N";

                    objPropUser.JobReopenPermission = chkJobReopenPermission.Checked ? "Y" : "N";

                    //Dispatch
                    if (chkDispatch.Checked == true)
                    {
                        objPropUser.Dispatch = "Y";
                    }
                    else
                    {
                        objPropUser.Dispatch = "N";
                    }

                    #endregion Permission

                    if (chkFinanceMgr.Checked == true)
                    {
                        objPropUser.FChart = 1;
                        objPropUser.FGLAdj = 1;
                    }
                    else
                    {
                        objPropUser.FChart = 0;
                        objPropUser.FGLAdj = 0;
                    }
                    if (chkFinanceStatement.Checked == true)
                    {
                        objPropUser.FinanStatement = 1;
                    }
                    else
                    {
                        objPropUser.FinanStatement = 0;
                    }

                    objPropUser.EmpMaintenance = Convert.ToInt16(chkEmpMainten.Checked);

                    objPropUser.SalesMgr = Convert.ToInt16(chkSalesMgr.Checked);

                    objPropUser.MassReview = Convert.ToInt32(chkMassReview.Checked);

                    objPropUser.Timestampfix = Convert.ToInt32(chkTimestampFix.Checked);

                    objPropUser.AddEquip = chkEquipmentsadd.Checked ? 1 : 0;

                    objPropUser.EditEquip = chkEquipmentsedit.Checked ? 1 : 0;

                    objPropUser.DeleteEquip = chkEquipmentsdelete.Checked ? 1 : 0;

                    objPropUser.ViewEquip = chkEquipmentsview.Checked ? 1 : 0;

                    //List<string> Departmentids = new List<string>();
                    //foreach (RadComboBoxItem item in ddlDepartment.Items)
                    //{
                    //    if (item.Checked)
                    //    {
                    //        Departmentids.Add(item.Value);
                    //    }
                    //}
                    //string strDepartment = string.Join(",", Departmentids.ToArray());
                    //objPropUser.Department = strDepartment;
                    //objPropUser.MOMUSer = txtMOMUserName.Text.Trim();
                    //objPropUser.MOMPASS = txtMOMPassword.Text.Trim();

                    objPropUser.ConnConfig = Session["config"].ToString();

                    objPropUser.DBName = Session["dbname"].ToString();
                    //DataSet dsinfo = new DataSet();
                    //dsinfo = objBL_User.getLicenseInfoUser(objPropUser);

                    objPropUser.IsProjectManager = chkProjectManager.Checked;
                    objPropUser.IsAssignedProject = chkAssignedProject.Checked;
                    //if (ddlMerchantID.SelectedValue != string.Empty)
                    //{
                    //    objPropUser.MerchantInfoId = Convert.ToInt32(ddlMerchantID.SelectedValue);
                    //}

                    //objPropUser.UserID = objBL_User.AddUserRole(userRole, objPropUser);

                    //if (Request.QueryString["uid"] != null)
                    if (ViewState["mode"] != null && ViewState["mode"].ToString()=="edit")
                    {
                        userRole.RoleID = Convert.ToInt32(Request.QueryString["uid"]);
                        objBL_User.AddUpdateUserRole(userRole, objPropUser);
                        RadGrid_Users.Rebind();
                        UserRole userRole1 = new UserRole();
                        userRole1.ConnConfig = Session["config"].ToString();
                        userRole1.RoleID = Convert.ToInt32(Request.QueryString["uid"]);
                        var ds = objBL_User.GetUsersOfRoleByID(userRole1);
                        UpdateSelectedUsers(ds.Tables[0]);
                        ScriptManager.RegisterStartupScript(this,Page.GetType(), "keySuccUp", "noty({text: 'Updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                        //if (rbStatus.SelectedValue == "1" && hdnLocCount.Value != "0")
                        //{
                        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyWarnUp", "noty({text: 'This user has assigned locations and cannot be set inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        //}
                        //else if (ddlUserType.SelectedValue == "0" && hdnLocCount.Value != "0")
                        //{
                        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyWarnUp", "noty({text: 'This user has assigned locations and cannot be removed from field.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        //}
                        //else
                        //{
                        //    var updatedBy = Session["Username"].ToString();
                        //    objBL_User.UpdateUser(objPropUser, updatedBy);
                        //    //objBL_User.UpdateUserPermission(objPropUser);
                        //    if (Session["COPer"].ToString() == "1")
                        //    {
                        //        SubmitCompany();
                        //        FillCompanySelected();
                        //    }
                        //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'User updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        //}

                        RadGrid_gvLogs.Rebind();

                    }
                    else
                    {
                        userRole.RoleID = 0;
                        userRole.RoleID = objBL_User.AddUpdateUserRole(userRole, objPropUser);
                        Session["AddEditRole_SuccMessage"] = "Added successfully!";
                        Response.Redirect("adduserrole.aspx?uid=" + userRole.RoleID);
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        
                        //var createdBy = Session["Username"].ToString();
                        //objPropUser.UserID = objBL_User.AddUserRole(objPropUser);
                        //if (Session["COPer"].ToString() == "1")
                        //{
                        //    ViewState["AddUserID"] = objPropUser.UserID;
                        //    SubmitCompany();
                        //}
                        //ViewState["mode"] = 0;
                        ////lblMsg.Text = "User added successfully.";

                        //string strsuper = "User";
                        //if (Request.QueryString["sup"] != null)
                        //{
                        //    strsuper = "Supervisor";
                        //}

                        //ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: '" + strsuper + " added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        //ClearControls();
                        //ResetFormControlValues(this);
                    }
                }
            }
            catch (Exception ex)
            {
                //lblMsg.Text = ex.Message;    
                //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                if (str.ToLower() == "error occured")
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                else
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }

        protected void RadGrid_Users_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            User objProp_User = new User();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.Status = 0;
            if (ViewState["mode"] != null && ViewState["mode"].ToString() == "edit")
            {
                //if (Request.QueryString["uid"] != null )// Edit/Copy
                objProp_User.RoleID = Convert.ToInt32(Request.QueryString["uid"]);
            }
            else
            {
                objProp_User.RoleID = 0;
            }

            BL_User objBL_User = new BL_User();

            var ds = objBL_User.GetUsersForRole(objProp_User);
            var result = ds.Tables[0];
            RadGrid_Users.VirtualItemCount = result.Rows.Count;
            RadGrid_Users.DataSource = result;
        }

        protected void RadGrid_Users_PreRender(object sender, EventArgs e)
        {
            if (Request.QueryString["uid"] != null && !IsPostBack)// Edit
            {
                UserRole userRole = new UserRole();
                userRole.ConnConfig = Session["config"].ToString();
                userRole.RoleID = Convert.ToInt32(Request.QueryString["uid"]);
                var ds = objBL_User.GetUsersOfRoleByID(userRole);
                UpdateSelectedUsers(ds.Tables[0]);
            }
        }

        protected void RadGrid_Users_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void RadGrid_Users_ItemEvent(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void RadGrid_Users_DataBound(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(Object o, EventArgs e)
        {
            foreach (GridDataItem gr in RadGrid_Users.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                chkSelect.Attributes["onclick"] = "SelectRowsUser();";
            }
        }

        private DataTable GetSelectedUsersFromUI()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("UserID", typeof(int));
                dt.Columns.Add("ApplyUserRolePermission", typeof(int));

                foreach (GridDataItem gvr in RadGrid_Users.Items)
                {
                    CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");

                    if (chkSelect.Checked == true)
                    {
                        DataRow dr = dt.NewRow();
                        Label lblUnit = (Label)gvr.FindControl("lblID");
                        DropDownList ddlApplyUserRolePermission = (DropDownList)gvr.FindControl("ddlApplyUserRolePermission");

                        dr["UserID"] = Convert.ToInt32(lblUnit.Text);
                        dr["ApplyUserRolePermission"] = Convert.ToInt32(ddlApplyUserRolePermission.SelectedValue);
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch { }
            return dt;
        }

        private void UpdateSelectedUsers(DataTable users)
        {
            // Reset all checkbox of RadgvEquip
            foreach (GridDataItem gr in RadGrid_Users.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                chkSelect.Checked = false;
            }

            foreach (DataRow dr in users.Rows)
            {
                foreach (GridDataItem gr in RadGrid_Users.Items)
                {
                    Label lblID = (Label)gr.FindControl("lblID");
                    Label lblname = (Label)gr.FindControl("lblunit");
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (dr["UserID"].ToString() == lblID.Text)
                    {
                        chkSelect.Checked = true;

                        if (txtUnit.Text != string.Empty)
                        {
                            txtUnit.Text = txtUnit.Text + ", " + lblname.Text;
                        }
                        else
                        {
                            txtUnit.Text = lblname.Text;
                        }
                    }
                }
            }
        }

        private void FillRoleInfo(DataTable roleInfo)
        {
            if (roleInfo != null && roleInfo.Rows.Count > 0)
            {
                foreach (DataRow dr in roleInfo.Rows)
                {
                    txtRoleName.Text = dr["RoleName"].ToString();
                    txtRoleDescription.Text = dr["Desc"].ToString();
                    rbStatus.SelectedValue = dr["Status"].ToString();
                }
            }
        }

        private void FillRolePermission(DataTable roleInfo)
        {
            if (roleInfo.Rows.Count > 0)
            {
                //string map = roleInfo.Rows[0]["ticket"].ToString().Substring(3, 1);
                //string sch = roleInfo.Rows[0]["ticket"].ToString().Substring(0, 1);

                //if (map == "Y")
                //{
                //    chkMap.Checked = true;
                //}

                //if (roleInfo.Rows[0]["dboard"] != DBNull.Value)
                //{
                //    if (roleInfo.Rows[0]["dboard"].ToString().Trim() != string.Empty)
                //    {
                //        int schTS = Convert.ToInt32(roleInfo.Rows[0]["dboard"]);
                //        if (schTS == 1)
                //        {
                //            chkScheduleBrd.Checked = true;
                //        }
                //        else
                //        {
                //            chkScheduleBrd.Checked = false;
                //        }
                //    }
                //}
                //}
                if (roleInfo.Rows[0]["salesp"].ToString() == "1")
                {
                    chkSalesperson.Checked = true;
                    //chkNotification.Enabled = chkSalesAssigned.Enabled = true;

                    chkSalesAssigned.Checked = roleInfo.Rows[0]["SalesAssigned"] == null ? false : Convert.ToBoolean(roleInfo.Rows[0]["SalesAssigned"]);
                    chkNotification.Checked = roleInfo.Rows[0]["NotificationOnAddOpportunity"] == null ? false : Convert.ToBoolean(roleInfo.Rows[0]["NotificationOnAddOpportunity"]);
                }
                else
                {
                    chkNotification.Checked = chkSalesAssigned.Checked = false;
                }
            }

            txtPOLimit.Text = roleInfo.Rows[0]["POLimit"].ToString();
            ddlPOApprove.SelectedValue = roleInfo.Rows[0]["POApprove"].ToString();
            ddlPOApproveAmt.SelectedValue = roleInfo.Rows[0]["POApproveAmt"].ToString();
            txtMinAmount.Text = roleInfo.Rows[0]["MinAmount"].ToString();
            txtMaxAmount.Text = roleInfo.Rows[0]["MaxAmount"].ToString();
            if (ddlPOApprove.SelectedValue == "0")
            {
                //divMinAmount.Attributes.Remove("visibility");
                //divMaxAmount.Attributes.Remove("visibility");
                //divApprovePo.Attributes.Remove("visibility");
                //divApprovePo.Visible = false;
                divApprovePo.Style["display"] = "none";
                divMinAmount.Style["display"] = "none";
                divMaxAmount.Style["display"] = "none";

            }
            else
            {
                divApprovePo.Style["display"] = "block";

                //divMinAmount.Style.Add("visibility", "visible");
                //divMaxAmount.Style.Add("visibility", "visible");
                //divApprovePo.Style.Add("visibility", "visible");
                //divApprovePo.Visible = true;

                if (ddlPOApproveAmt.SelectedValue == "0")
                {
                    divMinAmount.Style["display"] = "block";
                    divMaxAmount.Style["display"] = "block";
                    //divMinAmount.Style.Add("visibility", "visible");
                    //divMaxAmount.Style.Add("visibility", "visible");
                    //divMinAmount.Visible = true;
                    //divMaxAmount.Visible = true;
                }
                else if (ddlPOApproveAmt.SelectedValue == "1")
                {
                    divMinAmount.Style["display"] = "block";
                    divMaxAmount.Style["display"] = "none";
                    //divMaxAmount.Attributes.Remove("visibility");
                    //divMaxAmount.Visible = false;
                }
                else
                {
                    divMinAmount.Style["display"] = "none";
                    divMaxAmount.Style["display"] = "none";
                    //divMinAmount.Attributes.Remove("visibility");
                    //divMaxAmount.Attributes.Remove("visibility");
                    //divMinAmount.Visible = false;
                    //divMaxAmount.Visible = false;
                }
            }


            chkMassReview.Checked = Convert.ToBoolean(roleInfo.Rows[0]["massreview"]);
            chkProjectManager.Checked = Convert.ToBoolean(roleInfo.Rows[0]["IsProjectManager"]);
            chkAssignedProject.Checked = Convert.ToBoolean(roleInfo.Rows[0]["IsAssignedProject"]);
            // TODO: Thomas need to check for ES-33

            //if (ds.Tables[2] != null)
            //{

            //    foreach (DataRow dr in ds.Tables[2].Rows)
            //    {
            //        foreach (RadComboBoxItem li in ddlDepartment.Items)
            //        {
            //            if (dr["department"].ToString().Equals(li.Value))
            //                li.Checked = true;
            //        }
            //    }
            //}

            string LocnRemark = roleInfo.Rows[0]["Location"].ToString().Substring(3, 1);

            string PurcOrders = roleInfo.Rows[0]["PO"].ToString().Substring(0, 1);
            string Exp = roleInfo.Rows[0]["PO"].ToString().Substring(1, 1);
            string ProgFunc = string.Empty;
            string AccessUser = string.Empty;

            string Sales = roleInfo.Rows[0]["UserSales"].ToString().Substring(0, 1);
            string EmpMaintenace = roleInfo.Rows[0]["employeeMaint"].ToString().Substring(3, 1);
            string TCTimeFix = roleInfo.Rows[0]["TC"].ToString().Substring(1, 1);

            string _chart = roleInfo.Rows[0]["Chart"].ToString();
            string _glAdj = roleInfo.Rows[0]["GLAdj"].ToString();
            string _financeState = roleInfo.Rows[0]["Financial"].ToString().Substring(5, 1);

            string _apVendor = roleInfo.Rows[0]["Vendor"].ToString(); //check Account payable permission
            string _apBill = roleInfo.Rows[0]["Bill"].ToString();
            string _apBillPay = roleInfo.Rows[0]["BillPay"].ToString();

            if (_chart.Equals("YYYYYY") && _glAdj.Equals("YYYYYY"))
            {
                chkFinanceMgr.Checked = true;
            }

            if (_financeState.Equals("Y"))
            {
                chkFinanceStatement.Checked = true;
            }
            else
            {
                chkFinanceStatement.Checked = false;
            }

            //Equipment 
            string EquipmentPermission = roleInfo.Rows[0]["elevator"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["elevator"].ToString();

            string Addequip = EquipmentPermission.Length < 1 ? "Y" : EquipmentPermission.Substring(0, 1);
            string Editequip = EquipmentPermission.Length < 2 ? "Y" : EquipmentPermission.Substring(1, 1);
            string Deleteequip = EquipmentPermission.Length < 3 ? "Y" : EquipmentPermission.Substring(2, 1);
            string Viewquip = EquipmentPermission.Length < 4 ? "Y" : EquipmentPermission.Substring(3, 1);

            chkEquipmentsadd.Checked = (Addequip == "N") ? false : true;
            chkEquipmentsedit.Checked = (Editequip == "N") ? false : true;
            chkEquipmentsdelete.Checked = (Deleteequip == "N") ? false : true;
            chkEquipmentsview.Checked = (Viewquip == "N") ? false : true;

            //Project
            string ProjectPermission = roleInfo.Rows[0]["Job"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Job"].ToString();

            string Addejob = ProjectPermission.Length < 1 ? "Y" : ProjectPermission.Substring(0, 1);
            string Editejob = ProjectPermission.Length < 2 ? "Y" : ProjectPermission.Substring(1, 1);
            string Deleteejob = ProjectPermission.Length < 3 ? "Y" : ProjectPermission.Substring(2, 1);
            string Viewjob = ProjectPermission.Length < 4 ? "Y" : ProjectPermission.Substring(3, 1);

            chkProjectadd.Checked = (Addejob == "N") ? false : true;
            chkProjectEdit.Checked = (Editejob == "N") ? false : true;
            chkProjectDelete.Checked = (Deleteejob == "N") ? false : true;
            chkProjectView.Checked = (Viewjob == "N") ? false : true;

            //Location
            string LocationPermission = roleInfo.Rows[0]["Location"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Location"].ToString();

            string AddeLocation = LocationPermission.Length < 1 ? "Y" : LocationPermission.Substring(0, 1);
            string EditeLocation = LocationPermission.Length < 2 ? "Y" : LocationPermission.Substring(1, 1);
            string DeleteeLocation = LocationPermission.Length < 3 ? "Y" : LocationPermission.Substring(2, 1);
            string ViewLocation = LocationPermission.Length < 4 ? "Y" : LocationPermission.Substring(3, 1);

            chkLocationadd.Checked = (AddeLocation == "N") ? false : true;
            chkLocationedit.Checked = (EditeLocation == "N") ? false : true;
            chkLocationdelete.Checked = (DeleteeLocation == "N") ? false : true;
            chkLocationview.Checked = (ViewLocation == "N") ? false : true;

            //Owner
            string OwnerPermission = roleInfo.Rows[0]["Owner"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Owner"].ToString();

            string AddeOwner = OwnerPermission.Length < 1 ? "Y" : OwnerPermission.Substring(0, 1);
            string EditeOwner = OwnerPermission.Length < 2 ? "Y" : OwnerPermission.Substring(1, 1);
            string DeleteeOwner = OwnerPermission.Length < 3 ? "Y" : OwnerPermission.Substring(2, 1);
            string ViewOwner = OwnerPermission.Length < 4 ? "Y" : OwnerPermission.Substring(3, 1);

            chkCustomeradd.Checked = (AddeOwner == "N") ? false : true;
            chkCustomeredit.Checked = (EditeOwner == "N") ? false : true;
            chkCustomerdelete.Checked = (DeleteeOwner == "N") ? false : true;
            chkCustomerview.Checked = (ViewOwner == "N") ? false : true;

            //FinanceDataPermissions
            string ViewFinance = roleInfo.Rows[0]["FinancePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["FinancePermission"].ToString();
            chkViewFinance.Checked = (ViewFinance == "N") ? false : true;

            // ProjectListPermission
            string ViewProjectListPermission = roleInfo.Rows[0]["ProjectListPermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["ProjectListPermission"].ToString();

            chkViewProjectList.Checked = (ViewProjectListPermission == "N") ? false : true;

            //BOMPermissions
            string BOMPermission = roleInfo.Rows[0]["BOMPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["BOMPermission"].ToString();

            string AddBOM = BOMPermission.Length < 1 ? "Y" : BOMPermission.Substring(0, 1);
            string EditBOM = BOMPermission.Length < 2 ? "Y" : BOMPermission.Substring(1, 1);
            string DeleteBOM = BOMPermission.Length < 3 ? "Y" : BOMPermission.Substring(2, 1);
            string ViewBOM = BOMPermission.Length < 4 ? "Y" : BOMPermission.Substring(3, 1);

            chkAddBOM.Checked = (AddBOM == "N") ? false : true;
            chkEditBOM.Checked = (EditBOM == "N") ? false : true;
            chkDeleteBOM.Checked = (DeleteBOM == "N") ? false : true;
            chkViewBOM.Checked = (ViewBOM == "N") ? false : true;

            //WIPPermissions
            string WIPPermission = roleInfo.Rows[0]["WIPPermission"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["WIPPermission"].ToString();

            string AddWIP = WIPPermission.Length < 1 ? "Y" : WIPPermission.Substring(0, 1);
            string EditWIP = WIPPermission.Length < 2 ? "Y" : WIPPermission.Substring(1, 1);
            string DeleteWIP = WIPPermission.Length < 3 ? "Y" : WIPPermission.Substring(2, 1);
            string ViewWIP = WIPPermission.Length < 4 ? "Y" : WIPPermission.Substring(3, 1);
            string ReportWIP = WIPPermission.Length < 6 ? "Y" : WIPPermission.Substring(5, 1);

            chkAddWIP.Checked = (AddWIP == "N") ? false : true;
            chkEditWIP.Checked = (EditWIP == "N") ? false : true;
            chkDeleteWIP.Checked = (DeleteWIP == "N") ? false : true;
            chkViewWIP.Checked = (ViewWIP == "N") ? false : true;
            chkReportWIP.Checked = (ReportWIP == "N") ? false : true;

            //MilestonesPermission
            string MilesStonesPermission = roleInfo.Rows[0]["MilestonesPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["MilestonesPermission"].ToString();

            string AddMilesStones = MilesStonesPermission.Length < 1 ? "Y" : MilesStonesPermission.Substring(0, 1);
            string EditMilesStones = MilesStonesPermission.Length < 2 ? "Y" : MilesStonesPermission.Substring(1, 1);
            string DeleteMilesStones = MilesStonesPermission.Length < 3 ? "Y" : MilesStonesPermission.Substring(2, 1);
            string ViewMilesStones = MilesStonesPermission.Length < 4 ? "Y" : MilesStonesPermission.Substring(3, 1);

            chkAddMilesStones.Checked = (AddMilesStones == "N") ? false : true;
            chkEditMilesStones.Checked = (EditMilesStones == "N") ? false : true;
            chkDeleteMilesStones.Checked = (DeleteMilesStones == "N") ? false : true;
            chkViewMilesStones.Checked = (ViewMilesStones == "N") ? false : true;

            //Inventory Item
            string InventoryItemPermission = roleInfo.Rows[0]["Item"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Item"].ToString();

            string AddInventoryItem = InventoryItemPermission.Length < 1 ? "Y" : InventoryItemPermission.Substring(0, 1);
            string EditInventoryItem = InventoryItemPermission.Length < 2 ? "Y" : InventoryItemPermission.Substring(1, 1);
            string DeleteInventoryItem = InventoryItemPermission.Length < 3 ? "Y" : InventoryItemPermission.Substring(2, 1);
            string ViewInventoryItem = InventoryItemPermission.Length < 4 ? "Y" : InventoryItemPermission.Substring(3, 1);

            chkInventoryItemadd.Checked = (AddInventoryItem == "N") ? false : true;
            chkInventoryItemedit.Checked = (EditInventoryItem == "N") ? false : true;
            chkInventoryItemdelete.Checked = (DeleteInventoryItem == "N") ? false : true;
            chkInventoryItemview.Checked = (ViewInventoryItem == "N") ? false : true;

            //Inventory AdjustMent
            string InventoryAdjustmentPermission = roleInfo.Rows[0]["InvAdj"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["InvAdj"].ToString();

            string AddInventoryAdjustMent = InventoryAdjustmentPermission.Length < 1 ? "Y" : InventoryAdjustmentPermission.Substring(0, 1);
            string EditInventoryAdjustMent = InventoryAdjustmentPermission.Length < 2 ? "Y" : InventoryAdjustmentPermission.Substring(1, 1);
            string DeleteInventoryAdjustMent = InventoryAdjustmentPermission.Length < 3 ? "Y" : InventoryAdjustmentPermission.Substring(2, 1);
            string ViewInventoryAdjustMent = InventoryAdjustmentPermission.Length < 4 ? "Y" : InventoryAdjustmentPermission.Substring(3, 1);

            chkInventoryAdjustmentadd.Checked = (AddInventoryAdjustMent == "N") ? false : true;
            chkInventoryAdjustmentedit.Checked = (EditInventoryAdjustMent == "N") ? false : true;
            chkInventoryAdjustmentdelete.Checked = (DeleteInventoryAdjustMent == "N") ? false : true;
            chkInventoryAdjustmentview.Checked = (ViewInventoryAdjustMent == "N") ? false : true;

            //Inventory WareHouse
            string InventoryWareHousePermission = roleInfo.Rows[0]["Warehouse"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Warehouse"].ToString();

            string AddInventoryWareHouse = InventoryWareHousePermission.Length < 1 ? "Y" : InventoryWareHousePermission.Substring(0, 1);
            string EditInventoryWareHouse = InventoryWareHousePermission.Length < 2 ? "Y" : InventoryWareHousePermission.Substring(1, 1);
            string DeleteInventoryWareHouse = InventoryWareHousePermission.Length < 3 ? "Y" : InventoryWareHousePermission.Substring(2, 1);
            string ViewInventoryWareHouse = InventoryWareHousePermission.Length < 4 ? "Y" : InventoryWareHousePermission.Substring(3, 1);

            chkInventoryWareHouseadd.Checked = (AddInventoryWareHouse == "N") ? false : true;
            chkInventoryWareHouseedit.Checked = (EditInventoryWareHouse == "N") ? false : true;
            chkInventoryWareHousedelete.Checked = (DeleteInventoryWareHouse == "N") ? false : true;
            chkInventoryWareHouseview.Checked = (ViewInventoryWareHouse == "N") ? false : true;

            //Inventory Setup
            string InventorySetupPermission = roleInfo.Rows[0]["InvSetup"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["InvSetup"].ToString();

            string AddInventorySetup = InventorySetupPermission.Length < 1 ? "Y" : InventorySetupPermission.Substring(0, 1);
            string EditInventorySetup = InventorySetupPermission.Length < 2 ? "Y" : InventorySetupPermission.Substring(1, 1);
            string DeleteInventorySetup = InventorySetupPermission.Length < 3 ? "Y" : InventorySetupPermission.Substring(2, 1);
            string ViewInventorySetup = InventorySetupPermission.Length < 4 ? "Y" : InventorySetupPermission.Substring(3, 1);

            chkInventorysetupadd.Checked = (AddInventorySetup == "N") ? false : true;
            chkInventorysetupedit.Checked = (EditInventorySetup == "N") ? false : true;
            chkInventorysetupdelete.Checked = (DeleteInventorySetup == "N") ? false : true;
            chkInventorysetupview.Checked = (ViewInventorySetup == "N") ? false : true;

            //DocumentPermission
            string DocumentPermission = roleInfo.Rows[0]["DocumentPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["DocumentPermission"].ToString();

            string AddDocumentPermission = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
            string EditDocumentPermission = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
            string DeleteDocumentPermission = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
            string ViewDocumentPermission = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

            chkDocumentAdd.Checked = (AddDocumentPermission == "N") ? false : true;
            chkDocumentEdit.Checked = (EditDocumentPermission == "N") ? false : true;
            chkDocumentDelete.Checked = (DeleteDocumentPermission == "N") ? false : true;
            chkDocumentView.Checked = (ViewDocumentPermission == "N") ? false : true;

            //ContactPermission
            string ContactPermission = roleInfo.Rows[0]["ContactPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ContactPermission"].ToString();

            string AddContactPermission = ContactPermission.Length < 1 ? "Y" : ContactPermission.Substring(0, 1);
            string EditContactPermission = ContactPermission.Length < 2 ? "Y" : ContactPermission.Substring(1, 1);
            string DeleteContactPermission = ContactPermission.Length < 3 ? "Y" : ContactPermission.Substring(2, 1);
            string ViewContactPermission = ContactPermission.Length < 4 ? "Y" : ContactPermission.Substring(3, 1);

            chkContactAdd.Checked = (AddContactPermission == "N") ? false : true;
            chkContactEdit.Checked = (EditContactPermission == "N") ? false : true;
            chkContactDelete.Checked = (DeleteContactPermission == "N") ? false : true;
            chkContactView.Checked = (ViewContactPermission == "N") ? false : true;

            //VendorsPermission
            string VendorsPermission = roleInfo.Rows[0]["Vendor"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Vendor"].ToString();

            string AddVendorsPermission = VendorsPermission.Length < 1 ? "Y" : VendorsPermission.Substring(0, 1);
            string EditVendorsPermission = VendorsPermission.Length < 2 ? "Y" : VendorsPermission.Substring(1, 1);
            string DeleteVendorsPermission = VendorsPermission.Length < 3 ? "Y" : VendorsPermission.Substring(2, 1);
            string ViewVendorsPermission = VendorsPermission.Length < 4 ? "Y" : VendorsPermission.Substring(3, 1);

            chkVendorsAdd.Checked = (AddVendorsPermission == "N") ? false : true;
            chkVendorsEdit.Checked = (EditVendorsPermission == "N") ? false : true;
            chkVendorsDelete.Checked = (DeleteVendorsPermission == "N") ? false : true;
            chkVendorsView.Checked = (ViewVendorsPermission == "N") ? false : true;

            //Inventory Finance
            string InventoryFinancePermission = roleInfo.Rows[0]["InvViewer"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["InvViewer"].ToString();

            string AddInventoryFinance = InventoryFinancePermission.Length < 1 ? "Y" : InventoryFinancePermission.Substring(0, 1);
            string EditInventoryFinance = InventoryFinancePermission.Length < 2 ? "Y" : InventoryFinancePermission.Substring(1, 1);
            string DeleteInventoryFinance = InventoryFinancePermission.Length < 3 ? "Y" : InventoryFinancePermission.Substring(2, 1);
            string ViewInventoryFinance = InventoryFinancePermission.Length < 4 ? "Y" : InventoryFinancePermission.Substring(3, 1);

            chkInventoryFinanceAdd.Checked = (AddInventoryFinance == "N") ? false : true;
            chkInventoryFinanceedit.Checked = (EditInventoryFinance == "N") ? false : true;
            chkInventoryFinancedelete.Checked = (DeleteInventoryFinance == "N") ? false : true;
            chkInventoryFinanceview.Checked = (ViewInventoryFinance == "N") ? false : true;

            // Project templates
            string ProjecttempPermission = roleInfo.Rows[0]["ProjecttempPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ProjecttempPermission"].ToString();

            string AddProjecttempPermission = ProjecttempPermission.Length < 1 ? "Y" : ProjecttempPermission.Substring(0, 1);
            string EditProjecttempPermission = ProjecttempPermission.Length < 2 ? "Y" : ProjecttempPermission.Substring(1, 1);
            string DeleteProjecttempPermission = ProjecttempPermission.Length < 3 ? "Y" : ProjecttempPermission.Substring(2, 1);
            string ViewProjecttempPermission = ProjecttempPermission.Length < 4 ? "Y" : ProjecttempPermission.Substring(3, 1);

            chkProjectTempAdd.Checked = (AddProjecttempPermission == "N") ? false : true;
            chkProjectTempEdit.Checked = (EditProjecttempPermission == "N") ? false : true;
            chkProjectTempDelete.Checked = (DeleteProjecttempPermission == "N") ? false : true;
            chkProjectTempView.Checked = (ViewProjecttempPermission == "N") ? false : true;

            //BillingCodesPermission
            string BillingCodesPermission = roleInfo.Rows[0]["BillingCodesPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["BillingCodesPermission"].ToString();

            string AddBillingCodesPermission = BillingCodesPermission.Length < 1 ? "Y" : BillingCodesPermission.Substring(0, 1);
            string EditBillingCodesPermission = BillingCodesPermission.Length < 2 ? "Y" : BillingCodesPermission.Substring(1, 1);
            string DeleteBillingCodesPermission = BillingCodesPermission.Length < 3 ? "Y" : BillingCodesPermission.Substring(2, 1);
            string ViewBillingCodesPermission = BillingCodesPermission.Length < 4 ? "Y" : BillingCodesPermission.Substring(3, 1);

            chkBillingcodesAdd.Checked = (AddBillingCodesPermission == "N") ? false : true;
            chkBillingcodesEdit.Checked = (EditBillingCodesPermission == "N") ? false : true;
            chkBillingcodesDelete.Checked = (DeleteBillingCodesPermission == "N") ? false : true;
            chkBillingcodesView.Checked = (ViewBillingCodesPermission == "N") ? false : true;

            // Invoice Permission
            string InvoicePermission = roleInfo.Rows[0]["Invoice"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Invoice"].ToString();

            string AddInvoicePermission = InvoicePermission.Length < 1 ? "Y" : InvoicePermission.Substring(0, 1);
            string EditInvoicePermission = InvoicePermission.Length < 2 ? "Y" : InvoicePermission.Substring(1, 1);
            string DeleteInvoicePermission = InvoicePermission.Length < 3 ? "Y" : InvoicePermission.Substring(2, 1);
            string ViewInvoicePermission = InvoicePermission.Length < 4 ? "Y" : InvoicePermission.Substring(3, 1);

            chkInvoicesAdd.Checked = (AddInvoicePermission == "N") ? false : true;
            chkInvoicesEdit.Checked = (EditInvoicePermission == "N") ? false : true;
            chkInvoicesDelete.Checked = (DeleteInvoicePermission == "N") ? false : true;
            chkInvoicesView.Checked = (ViewInvoicePermission == "N") ? false : true;

            //POPermission
            string POPermission = roleInfo.Rows[0]["PO"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["PO"].ToString();

            string AddPOPermission = POPermission.Length < 1 ? "Y" : POPermission.Substring(0, 1);
            string EditPOPermission = POPermission.Length < 2 ? "Y" : POPermission.Substring(1, 1);
            string DeletePOPermission = POPermission.Length < 3 ? "Y" : POPermission.Substring(2, 1);
            string ViewPOPermission = POPermission.Length < 4 ? "Y" : POPermission.Substring(3, 1);

            chkPOAdd.Checked = (AddPOPermission == "N") ? false : true;
            chkPOEdit.Checked = (EditPOPermission == "N") ? false : true;
            chkPODelete.Checked = (DeletePOPermission == "N") ? false : true;
            chkPOView.Checked = (ViewPOPermission == "N") ? false : true;

            //RPOPermission
            string RPOPermission = roleInfo.Rows[0]["RPO"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["RPO"].ToString();

            string AddRPOPermission = RPOPermission.Length < 1 ? "Y" : RPOPermission.Substring(0, 1);
            string EditRPOPermission = RPOPermission.Length < 2 ? "Y" : RPOPermission.Substring(1, 1);
            string DeleteRPOPermission = RPOPermission.Length < 3 ? "Y" : RPOPermission.Substring(2, 1);
            string ViewRPOPermission = RPOPermission.Length < 4 ? "Y" : RPOPermission.Substring(3, 1);

            chkRPOAdd.Checked = (AddRPOPermission == "N") ? false : true;
            chkRPOEdit.Checked = (EditRPOPermission == "N") ? false : true;
            chkRPODelete.Checked = (DeleteRPOPermission == "N") ? false : true;
            chkRPOView.Checked = (ViewRPOPermission == "N") ? false : true;

            //PurchasingmodulePermission
            string PurchasingmodulePermission = roleInfo.Rows[0]["PurchasingmodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["PurchasingmodulePermission"].ToString();
            chkPurchasingmodule.Checked = (PurchasingmodulePermission == "N") ? false : true;

            //PONotification
            string PONotification = roleInfo.Rows[0]["PONotification"] == DBNull.Value ? "N" : roleInfo.Rows[0]["PONotification"].ToString();
            chkPONotification.Checked = (PONotification == "N") ? false : true;

            //BillingmodulePermission
            string BillingmodulePermission = roleInfo.Rows[0]["BillingmodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["BillingmodulePermission"].ToString();

            chkBillingmodule.Checked = (BillingmodulePermission == "N") ? false : true;

            //BillPermission
            string BillPermission = roleInfo.Rows[0]["Bill"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Bill"].ToString();

            string AddBillPermission = BillPermission.Length < 1 ? "Y" : BillPermission.Substring(0, 1);
            string EditBillPermission = BillPermission.Length < 2 ? "Y" : BillPermission.Substring(1, 1);
            string DeleteBillPermission = BillPermission.Length < 3 ? "Y" : BillPermission.Substring(2, 1);
            string ViewBillPermission = BillPermission.Length < 4 ? "Y" : BillPermission.Substring(3, 1);

            chkAddBills.Checked = (AddBillPermission == "N") ? false : true;
            chkEditBills.Checked = (EditBillPermission == "N") ? false : true;
            chkDeleteBills.Checked = (DeleteBillPermission == "N") ? false : true;
            chkViewBills.Checked = (ViewBillPermission == "N") ? false : true;

            //BillPayPermission
            string BillPayPermission = roleInfo.Rows[0]["BillPay"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["BillPay"].ToString();
            /*
            1: Add
            2: Edit
            3: Delete
            4: View
            5: Show Bank Balances (this case)
            6: Report
            */
            string AddBillPayPermission = BillPayPermission.Length < 1 ? "Y" : BillPayPermission.Substring(0, 1);
            string EditBillPayPermission = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(1, 1);
            string DeleteBillPayPermission = BillPayPermission.Length < 3 ? "Y" : BillPayPermission.Substring(2, 1);
            string ViewBillPayPermission = BillPayPermission.Length < 4 ? "Y" : BillPayPermission.Substring(3, 1);
            string ShowBankBalancesPermission = BillPayPermission.Length < 5 ? "Y" : BillPayPermission.Substring(4, 1);

            chkAddManageChecks.Checked = (AddBillPayPermission == "N") ? false : true;
            chkEditManageChecks.Checked = (EditBillPayPermission == "N") ? false : true;
            chkDeleteManageChecks.Checked = (DeleteBillPayPermission == "N") ? false : true;
            chkViewManageChecks.Checked = (ViewBillPayPermission == "N") ? false : true;
            chkShowBankBalances.Checked = (ShowBankBalancesPermission == "N") ? false : true;

            //VendorPermission
            string VendorPermission = roleInfo.Rows[0]["Vendor"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Vendor"].ToString();

            string AddVendorPermission = VendorPermission.Length < 1 ? "Y" : VendorPermission.Substring(0, 1);
            string EditVendorPermission = VendorPermission.Length < 2 ? "Y" : VendorPermission.Substring(1, 1);
            string DeleteVendorPermission = VendorPermission.Length < 3 ? "Y" : VendorPermission.Substring(2, 1);
            string ViewVendorPermission = VendorPermission.Length < 4 ? "Y" : VendorPermission.Substring(3, 1);

            chkVendorsAdd.Checked = (AddVendorPermission == "N") ? false : true;
            chkVendorsEdit.Checked = (EditVendorPermission == "N") ? false : true;
            chkVendorsDelete.Checked = (DeleteVendorPermission == "N") ? false : true;
            chkVendorsView.Checked = (ViewVendorPermission == "N") ? false : true;

            //AccountPayablemodulePermission
            string AccountPayablemodulePermission = roleInfo.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["AccountPayablemodulePermission"].ToString();

            chkAccountPayable.Checked = (AccountPayablemodulePermission == "N") ? false : true;

            //PaymentHistoryPermission
            string PaymentHistoryPermission = roleInfo.Rows[0]["PaymentHistoryPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["PaymentHistoryPermission"].ToString();
            string ViewPaymentHistoryPermission = PaymentHistoryPermission.Length < 4 ? "Y" : PaymentHistoryPermission.Substring(3, 1);

            chkPaymentHistoryView.Checked = (ViewPaymentHistoryPermission == "N") ? false : true;

            //CustomermodulePermission
            string CustomermodulePermission = roleInfo.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["CustomermodulePermission"].ToString();

            chkCustomerModule.Checked = (CustomermodulePermission == "N") ? false : true;

            //CreditHoldPermissions
            string CreditHoldPermissions = roleInfo.Rows[0]["CreditHold"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["CreditHold"].ToString();
            string ViewCreditHoldPermissions = CreditHoldPermissions.Length < 4 ? "Y" : CreditHoldPermissions.Substring(3, 1);
            chkCreditHold.Checked = (ViewCreditHoldPermissions == "N") ? false : true;

            //CreditFlagPermissions
            string CreditFlagPermissions = roleInfo.Rows[0]["CreditFlag"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["CreditFlag"].ToString();
            string ViewCreditFlagPermissions = CreditFlagPermissions.Length < 4 ? "Y" : CreditFlagPermissions.Substring(3, 1);
            chkCreditFlag.Checked = (ViewCreditFlagPermissions == "N") ? false : true;

            //WriteOffPermissions
            string WriteOffPermissions = roleInfo.Rows[0]["WriteOff"] == DBNull.Value ? "N" : roleInfo.Rows[0]["WriteOff"].ToString().Substring(0, 1);
            chkWriteOff.Checked = (WriteOffPermissions == "N") ? false : true;

            //FinancialmodulePermission
            string FinancialmodulePermission = roleInfo.Rows[0]["FinancialmodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["FinancialmodulePermission"].ToString();

            chkFinancialmodule.Checked = (FinancialmodulePermission == "N") ? false : true;

            //ChartPermission
            string ChartPermission = roleInfo.Rows[0]["Chart"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Chart"].ToString();

            string AddChartPermission = ChartPermission.Length < 1 ? "Y" : ChartPermission.Substring(0, 1);
            string EditChartPermission = ChartPermission.Length < 2 ? "Y" : ChartPermission.Substring(1, 1);
            string DeleteChartPermission = ChartPermission.Length < 3 ? "Y" : ChartPermission.Substring(2, 1);
            string ViewChartPermission = ChartPermission.Length < 4 ? "Y" : ChartPermission.Substring(3, 1);

            chkChartAdd.Checked = (AddChartPermission == "N") ? false : true;
            chkChartEdit.Checked = (EditChartPermission == "N") ? false : true;
            chkChartDelete.Checked = (DeleteChartPermission == "N") ? false : true;
            chkChartView.Checked = (ViewChartPermission == "N") ? false : true;

            //JournalEntryPermission
            string JournalEntryPermission = roleInfo.Rows[0]["GLAdj"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["GLAdj"].ToString();

            string AddJournalEntryPermission = JournalEntryPermission.Length < 1 ? "Y" : JournalEntryPermission.Substring(0, 1);
            string EditJournalEntryPermission = JournalEntryPermission.Length < 2 ? "Y" : JournalEntryPermission.Substring(1, 1);
            string DeleteJournalEntryPermission = JournalEntryPermission.Length < 3 ? "Y" : JournalEntryPermission.Substring(2, 1);
            string ViewJournalEntryPermission = JournalEntryPermission.Length < 4 ? "Y" : JournalEntryPermission.Substring(3, 1);

            chkJournalEntryAdd.Checked = (AddJournalEntryPermission == "N") ? false : true;
            chkJournalEntryEdit.Checked = (EditJournalEntryPermission == "N") ? false : true;
            chkJournalEntryDelete.Checked = (DeleteJournalEntryPermission == "N") ? false : true;
            chkJournalEntryView.Checked = (ViewJournalEntryPermission == "N") ? false : true;

            //BankrecPermission
            string BankrecPermission = roleInfo.Rows[0]["bankrec"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["bankrec"].ToString();

            string AddBankrecPermission = BankrecPermission.Length < 1 ? "Y" : BankrecPermission.Substring(0, 1);
            string EditBankrecPermission = BankrecPermission.Length < 2 ? "Y" : BankrecPermission.Substring(1, 1);
            string DeleteBankrecPermission = BankrecPermission.Length < 3 ? "Y" : BankrecPermission.Substring(2, 1);
            string ViewBankrecPermission = BankrecPermission.Length < 4 ? "Y" : BankrecPermission.Substring(3, 1);

            chkBankAdd.Checked = (AddBankrecPermission == "N") ? false : true;
            chkBankEdit.Checked = (EditBankrecPermission == "N") ? false : true;
            chkBankDelete.Checked = (DeleteBankrecPermission == "N") ? false : true;
            chkBankView.Checked = (ViewBankrecPermission == "N") ? false : true;

            //RCmodulePermission
            string RCmodulePermission = roleInfo.Rows[0]["RCmodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["RCmodulePermission"].ToString();

            chkRecurring.Checked = (RCmodulePermission == "N") ? false : true;

            //ProcessRCPermission
            string ProcessRCPermission = roleInfo.Rows[0]["ProcessRCPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ProcessRCPermission"].ToString();

            string AddProcessRCPermissionn = ProcessRCPermission.Length < 1 ? "Y" : ProcessRCPermission.Substring(0, 1);
            string EditProcessRCPermission = ProcessRCPermission.Length < 2 ? "Y" : ProcessRCPermission.Substring(1, 1);
            string DeleteProcessRCPermission = ProcessRCPermission.Length < 3 ? "Y" : ProcessRCPermission.Substring(2, 1);
            string ViewProcessRCPermission = ProcessRCPermission.Length < 4 ? "Y" : ProcessRCPermission.Substring(3, 1);

            chkRecContractsAdd.Checked = (AddProcessRCPermissionn == "N") ? false : true;
            chkRecContractsEdit.Checked = (EditProcessRCPermission == "N") ? false : true;
            chkRecContractsDelete.Checked = (DeleteProcessRCPermission == "N") ? false : true;
            chkRecContractsView.Checked = (ViewProcessRCPermission == "N") ? false : true;

            //RCRenewEscalatePermission
            string RCRenewEscalatePermission = roleInfo.Rows[0]["RCRenewEscalatePermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["RCRenewEscalatePermission"].ToString();

            string AddRCRenewEscalatePermission = RCRenewEscalatePermission.Length < 1 ? "Y" : RCRenewEscalatePermission.Substring(0, 1);
            string ViewRCRenewEscalatePermission = RCRenewEscalatePermission.Length < 4 ? "Y" : RCRenewEscalatePermission.Substring(3, 1);

            chkRenewEscalateAdd.Checked = (AddRCRenewEscalatePermission == "N") ? false : true;
            chkRenewEscalateView.Checked = (ViewRCRenewEscalatePermission == "N") ? false : true;
            //ProcessCPermission
            string ProcessCPermission = roleInfo.Rows[0]["ProcessC"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ProcessC"].ToString();

            string AddProcessCPermission = ProcessCPermission.Length < 1 ? "Y" : ProcessCPermission.Substring(0, 1);
            string EditProcessCPermission = ProcessCPermission.Length < 2 ? "Y" : ProcessCPermission.Substring(1, 1);
            string DeleteProcessCPermission = ProcessCPermission.Length < 3 ? "Y" : ProcessCPermission.Substring(2, 1);
            string ViewProcessCPermission = ProcessCPermission.Length < 4 ? "Y" : ProcessCPermission.Substring(3, 1);

            chkRecInvoicesAdd.Checked = (AddProcessCPermission == "N") ? false : true;
            //chkRecInvoicesEdit.Checked = (EditProcessCPermission == "N") ? false : true;
            chkRecInvoicesDelete.Checked = (DeleteProcessCPermission == "N") ? false : true;
            chkRecInvoicesView.Checked = (ViewProcessCPermission == "N") ? false : true;

            // Recurring TicketPermission
            string ProcessTPermission = roleInfo.Rows[0]["ProcessT"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ProcessT"].ToString();

            string AddProcessTPermission = ProcessTPermission.Length < 1 ? "Y" : ProcessTPermission.Substring(0, 1);
            string EditProcessTPermission = ProcessTPermission.Length < 2 ? "Y" : ProcessTPermission.Substring(1, 1);
            string DeleteProcessTPermission = ProcessTPermission.Length < 3 ? "Y" : ProcessTPermission.Substring(2, 1);
            string ViewProcessTPermission = ProcessTPermission.Length < 4 ? "Y" : ProcessTPermission.Substring(3, 1);

            chkRecTicketsAdd.Checked = (AddProcessTPermission == "N") ? false : true;
            //chkRecTicketsEdit.Checked = (EditProcessTPermission == "N") ? false : true;
            chkRecTicketsDelete.Checked = (DeleteProcessTPermission == "N") ? false : true;
            chkRecTicketsView.Checked = (ViewProcessTPermission == "N") ? false : true;

            //SafetyTestPermission
            string SafetyTestPermission = roleInfo.Rows[0]["SafetyTestsPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["SafetyTestsPermission"].ToString();

            string AddSafetyTestPermission = SafetyTestPermission.Length < 1 ? "Y" : SafetyTestPermission.Substring(0, 1);
            string EditSafetyTestPermission = SafetyTestPermission.Length < 2 ? "Y" : SafetyTestPermission.Substring(1, 1);
            string DeleteSafetyTestPermission = SafetyTestPermission.Length < 3 ? "Y" : SafetyTestPermission.Substring(2, 1);
            string ViewSafetyTestPermission = SafetyTestPermission.Length < 4 ? "Y" : SafetyTestPermission.Substring(3, 1);

            chkSafetyTestsAdd.Checked = (AddSafetyTestPermission == "N") ? false : true;
            chkSafetyTestsEdit.Checked = (EditSafetyTestPermission == "N") ? false : true;
            chkSafetyTestsDelete.Checked = (DeleteSafetyTestPermission == "N") ? false : true;
            chkSafetyTestsView.Checked = (ViewSafetyTestPermission == "N") ? false : true;

            //ViolationPermission
            string ViolationPermission = roleInfo.Rows[0]["ViolationPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ViolationPermission"].ToString();

            string AddViolationPermission = ViolationPermission.Length < 1 ? "Y" : ViolationPermission.Substring(0, 1);
            string EditViolationPermission = ViolationPermission.Length < 2 ? "Y" : ViolationPermission.Substring(1, 1);
            string DeleteViolationPermission = ViolationPermission.Length < 3 ? "Y" : ViolationPermission.Substring(2, 1);
            string ViewViolationPermission = ViolationPermission.Length < 4 ? "Y" : ViolationPermission.Substring(3, 1);

            chkViolationsAdd.Checked = (AddViolationPermission == "N") ? false : true;
            chkViolationsDelete.Checked = (DeleteViolationPermission == "N") ? false : true;
          

            //ScheduleModulePermission
            string SchedulemodulePermission = roleInfo.Rows[0]["SchedulemodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["SchedulemodulePermission"].ToString();

            chkSchedule.Checked = (SchedulemodulePermission == "N") ? false : true;

            //ProjectModulePermission
            string ProjectModulePermission = roleInfo.Rows[0]["ProjectModulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["ProjectModulePermission"].ToString();

            chkProjectmodule.Checked = (ProjectModulePermission == "N") ? false : true;

            //InventoryModulePermission
            string InventoryModulePermission = roleInfo.Rows[0]["InventoryModulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["InventoryModulePermission"].ToString();

            chkInventorymodule.Checked = (InventoryModulePermission == "N") ? false : true;

            //JobClosePermission
            string JobClosePermission = roleInfo.Rows[0]["JobClosePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["JobClosePermission"].ToString();
            if (JobClosePermission.Contains("Y")) chkJobClosePermission.Checked = true; else chkJobClosePermission.Checked = false;

            //JobCompletedPermission
            string JobCompletedPermission = roleInfo.Rows[0]["JobCompletedPermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["JobCompletedPermission"].ToString();
            if (JobCompletedPermission.Contains("Y")) chkJobCompletedPermission.Checked = true; else chkJobCompletedPermission.Checked = false;

            //JobReopenPermission
            string JobReopenPermission = roleInfo.Rows[0]["JobReopenPermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["JobReopenPermission"].ToString();
            if (JobReopenPermission.Contains("Y")) chkJobReopenPermission.Checked = true; else chkJobReopenPermission.Checked = false;

            //SchedulePermission
            string SchedulePermission = roleInfo.Rows[0]["Ticket"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Ticket"].ToString();
            string ViewSchedulePermission = SchedulePermission.Length < 4 ? "Y" : SchedulePermission.Substring(3, 1);
            string TicketReportPer = SchedulePermission.Length < 6 ? "Y" : SchedulePermission.Substring(5, 1);

            chkScheduleBoard.Checked = (ViewSchedulePermission == "N") ? false : true;

            //Dispatch
            string TicketPermission = roleInfo.Rows[0]["Dispatch"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Dispatch"].ToString();

            string AddTicketePermission = TicketPermission.Length < 1 ? "Y" : TicketPermission.Substring(0, 1);
            string EditTicketePermission = TicketPermission.Length < 2 ? "Y" : TicketPermission.Substring(1, 1);
            string DeleteTicketePermission = TicketPermission.Length < 3 ? "Y" : TicketPermission.Substring(2, 1);
            string ViewTicketePermission = TicketPermission.Length < 4 ? "Y" : TicketPermission.Substring(3, 1);
            //string ReportTicketePermission = TicketPermission.Length < 5 ? "Y" : TicketPermission.Substring(4, 1);
            //string EmailDispatchPermission = TicketPermission.Length < 6 ? "Y" : TicketPermission.Substring(5, 1);

            /** Thomas: I am changing this to make it the same other fields (Leads, Opportunities, Estimates)
            1: Add
            2: Edit
            3: Delete
            4: View
            5: Dispatch (this case)
            6: Report
            */
            string EmailDispatchPermission = TicketPermission.Length < 5 ? "Y" : TicketPermission.Substring(4, 1);
            string ReportTicketePermission = TicketPermission.Length < 6 ? "Y" : TicketPermission.Substring(5, 1);

            chkTicketListAdd.Checked = (AddTicketePermission == "N") ? false : true;
            chkTicketListEdit.Checked = (EditTicketePermission == "N") ? false : true;
            chkTicketListDelete.Checked = (DeleteTicketePermission == "N") ? false : true;
            chkTicketListView.Checked = (ViewTicketePermission == "N") ? false : true;
            chkTicketListReport.Checked = (ReportTicketePermission == "N") ? false : true;
            chkDispatch.Checked = (EmailDispatchPermission == "N") ? false : true;

            //Manual Timesheet Permission
            string MTimesheetPermission = roleInfo.Rows[0]["MTimesheet"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["MTimesheet"].ToString();

            string Timesheetadd = MTimesheetPermission.Length < 1 ? "Y" : MTimesheetPermission.Substring(0, 1);
            string Timesheetedit = MTimesheetPermission.Length < 2 ? "Y" : MTimesheetPermission.Substring(1, 1);
            string Timesheetdelete = MTimesheetPermission.Length < 3 ? "Y" : MTimesheetPermission.Substring(2, 1);
            string Timesheetview = MTimesheetPermission.Length < 4 ? "Y" : MTimesheetPermission.Substring(3, 1);
            string Timesheetreport = MTimesheetPermission.Length < 6 ? "Y" : MTimesheetPermission.Substring(5, 1);

            chkTimesheetadd.Checked = (Timesheetadd == "N") ? false : true;
            chkTimesheetedit.Checked = (Timesheetedit == "N") ? false : true;
            chkTimesheetdelete.Checked = (Timesheetdelete == "N") ? false : true;
            chkTimesheetview.Checked = (Timesheetview == "N") ? false : true;
            chkTimesheetreport.Checked = (Timesheetreport == "N") ? false : true;

            //E Timesheet Permission
            string ETimesheetPermission = roleInfo.Rows[0]["ETimesheet"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["ETimesheet"].ToString();

            string ETimesheetaddPermission = ETimesheetPermission.Length < 1 ? "Y" : ETimesheetPermission.Substring(0, 1);
            string ETimesheeteditPermission = ETimesheetPermission.Length < 2 ? "Y" : ETimesheetPermission.Substring(1, 1);
            string ETimesheetdeletePermission = ETimesheetPermission.Length < 3 ? "Y" : ETimesheetPermission.Substring(2, 1);
            string ETimesheetviewPermission = ETimesheetPermission.Length < 4 ? "Y" : ETimesheetPermission.Substring(3, 1);
            string ETimesheetreportPermission = ETimesheetPermission.Length < 6 ? "Y" : ETimesheetPermission.Substring(5, 1);

            chkETimesheetadd.Checked = (ETimesheetaddPermission == "N") ? false : true;
            chkETimesheetedit.Checked = (ETimesheeteditPermission == "N") ? false : true;
            chkETimesheetdelete.Checked = (ETimesheetdeletePermission == "N") ? false : true;
            chkETimesheetview.Checked = (ETimesheetviewPermission == "N") ? false : true;
            chkETimesheetreport.Checked = (ETimesheetreportPermission == "N") ? false : true;

            //MapR Permission
            string MapRPermission = roleInfo.Rows[0]["MapR"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["MapR"].ToString();

            string MapaddPermission = MapRPermission.Length < 1 ? "Y" : MapRPermission.Substring(0, 1);
            string MapeditPermission = MapRPermission.Length < 2 ? "Y" : MapRPermission.Substring(1, 1);
            string MapdeletePermission = MapRPermission.Length < 3 ? "Y" : MapRPermission.Substring(2, 1);
            string MapviewPermission = MapRPermission.Length < 4 ? "Y" : MapRPermission.Substring(3, 1);
            string MapPermission = MapRPermission.Length < 6 ? "Y" : MapRPermission.Substring(5, 1);

            chkMapAdd.Checked = (MapaddPermission == "N") ? false : true;
            chkMapEdit.Checked = (MapeditPermission == "N") ? false : true;
            chkMapDelete.Checked = (MapdeletePermission == "N") ? false : true;
            chkMapView.Checked = (MapviewPermission == "N") ? false : true;
            chkMapReport.Checked = (MapPermission == "N") ? false : true;

            //RouteBuilder Permission
            string RouteBuilderPermission = roleInfo.Rows[0]["RouteBuilder"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["RouteBuilder"].ToString();

            string RouteBuilderaddPermission = RouteBuilderPermission.Length < 1 ? "Y" : RouteBuilderPermission.Substring(0, 1);
            string RouteBuildereditPermission = RouteBuilderPermission.Length < 2 ? "Y" : RouteBuilderPermission.Substring(1, 1);
            string RouteBuilderdeletePermission = RouteBuilderPermission.Length < 3 ? "Y" : RouteBuilderPermission.Substring(2, 1);
            string RouteBuilderviewPermission = RouteBuilderPermission.Length < 4 ? "Y" : RouteBuilderPermission.Substring(3, 1);
            string RouteBuilderReportPermission = RouteBuilderPermission.Length < 6 ? "Y" : RouteBuilderPermission.Substring(5, 1);

            chkRouteBuilderAdd.Checked = (RouteBuilderaddPermission == "N") ? false : true;
            chkRouteBuilderEdit.Checked = (RouteBuildereditPermission == "N") ? false : true;
            chkRouteBuilderDelete.Checked = (RouteBuilderdeletePermission == "N") ? false : true;
            chkRouteBuilderView.Checked = (RouteBuilderviewPermission == "N") ? false : true;
            chkRouteBuilderReport.Checked = (RouteBuilderReportPermission == "N") ? false : true;

            //TicketSchedulePermission
            string TicketSchedulePermission = roleInfo.Rows[0]["Resolve"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Resolve"].ToString();

            string AddTicketeSchedulePermission = TicketSchedulePermission.Length < 1 ? "Y" : TicketSchedulePermission.Substring(0, 1);
            string EditTicketeSchedulePermission = TicketSchedulePermission.Length < 2 ? "Y" : TicketSchedulePermission.Substring(1, 1);
            string DeleteTicketeSchedulePermission = TicketSchedulePermission.Length < 3 ? "Y" : TicketSchedulePermission.Substring(2, 1);
            string ViewTicketeSchedulePermission = TicketSchedulePermission.Length < 4 ? "Y" : TicketSchedulePermission.Substring(3, 1);
            string ReportTicketeSchedulePermission = TicketSchedulePermission.Length < 6 ? "Y" : TicketSchedulePermission.Substring(5, 1);

            chkResolveTicketAdd.Checked = (AddTicketeSchedulePermission == "N") ? false : true;
            chkResolveTicketEdit.Checked = (EditTicketeSchedulePermission == "N") ? false : true;
            chkResolveTicketDelete.Checked = (DeleteTicketeSchedulePermission == "N") ? false : true;
            chkResolveTicketView.Checked = (ViewTicketeSchedulePermission == "N") ? false : true;
            chkResolveTicketReport.Checked = (ReportTicketeSchedulePermission == "N") ? false : true;

            // Sales Module Permission
            string SalesModulePermission = roleInfo.Rows[0]["SalesManager"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["SalesManager"].ToString();

            chkSalesMgr.Checked = (SalesModulePermission == "N") ? false : true;

            //Sales Permission
            string SalesPermission = roleInfo.Rows[0]["UserSales"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["UserSales"].ToString();

            string SalesaddPermission = SalesPermission.Length < 1 ? "Y" : SalesPermission.Substring(0, 1);
            string SaleseditPermission = SalesPermission.Length < 2 ? "Y" : SalesPermission.Substring(1, 1);
            string SalesdeletePermission = SalesPermission.Length < 3 ? "Y" : SalesPermission.Substring(2, 1);
            string SalesviewPermission = SalesPermission.Length < 4 ? "Y" : SalesPermission.Substring(3, 1);
            string SalesReportPermission = SalesPermission.Length < 6 ? "Y" : SalesPermission.Substring(5, 1);

            chkLeadAdd.Checked = (SalesaddPermission == "N") ? false : true;
            chkLeadEdit.Checked = (SaleseditPermission == "N") ? false : true;
            chkLeadDelete.Checked = (SalesdeletePermission == "N") ? false : true;
            chkLeadView.Checked = (SalesviewPermission == "N") ? false : true;
            chkLeadReport.Checked = (SalesReportPermission == "N") ? false : true;

            //Payroll check
            bool PRPermission = Convert.ToBoolean(roleInfo.Rows[0]["PR"] == DBNull.Value ? false : roleInfo.Rows[0]["PR"]);
            payrollModulchck.Checked = (PRPermission == false) ? false : true;

            //Payroll Permission Employee
            string employeePermission = roleInfo.Rows[0]["EmployeeMaint"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["EmployeeMaint"].ToString();

            string AddemployeePermission = employeePermission.Length < 1 ? "Y" : employeePermission.Substring(0, 1);
            string EditemployeePermission = employeePermission.Length < 2 ? "Y" : employeePermission.Substring(1, 1);
            string DeleteemployeePermission = employeePermission.Length < 3 ? "Y" : employeePermission.Substring(2, 1);
            string ViewemployeePermission = employeePermission.Length < 4 ? "Y" : employeePermission.Substring(3, 1);

            empAdd.Checked = (AddemployeePermission == "N") ? false : true;
            empEdit.Checked = (EditemployeePermission == "N") ? false : true;
            empDelete.Checked = (DeleteemployeePermission == "N") ? false : true;
            empView.Checked = (ViewemployeePermission == "N") ? false : true;

            //Run Payroll Permission 
            string RunPayrollPermission = roleInfo.Rows[0]["PRProcess"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["PRProcess"].ToString();
            string AddRunPayroll = RunPayrollPermission.Length < 1 ? "Y" : RunPayrollPermission.Substring(0, 1);
            string EditRunPayrollPermission = RunPayrollPermission.Length < 2 ? "Y" : RunPayrollPermission.Substring(1, 1);
            string DeleteRunPayrollPermission = RunPayrollPermission.Length < 3 ? "Y" : RunPayrollPermission.Substring(2, 1);
            string ViewRunPayrollPermission = RunPayrollPermission.Length < 4 ? "Y" : RunPayrollPermission.Substring(3, 1);

            runAdd.Checked = (AddRunPayroll == "N") ? false : true;
            runEdit.Checked = (EditRunPayrollPermission == "N") ? false : true;
            runDelete.Checked = (DeleteRunPayrollPermission == "N") ? false : true;
            runView.Checked = (ViewRunPayrollPermission == "N") ? false : true;

            //Payroll check Permission 
            string payrollCheckPermission = roleInfo.Rows[0]["PRRegister"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["PRRegister"].ToString();

            string AddpayrollCheckPermission = payrollCheckPermission.Length < 1 ? "Y" : payrollCheckPermission.Substring(0, 1);
            string EditpayrollCheckPermission = payrollCheckPermission.Length < 2 ? "Y" : payrollCheckPermission.Substring(1, 1);
            string DeletepayrollCheckPermission = payrollCheckPermission.Length < 3 ? "Y" : payrollCheckPermission.Substring(2, 1);
            string ViewpayrollCheckPermission = payrollCheckPermission.Length < 4 ? "Y" : payrollCheckPermission.Substring(3, 1);

            payrollchckAdd.Checked = (AddpayrollCheckPermission == "N") ? false : true;
            payrollchckEdit.Checked = (EditpayrollCheckPermission == "N") ? false : true;
            payrollchckDelete.Checked = (DeletepayrollCheckPermission == "N") ? false : true;
            payrollchckView.Checked = (ViewpayrollCheckPermission == "N") ? false : true;

            //Payroll Form Permission 
            string payrollFormPermission = roleInfo.Rows[0]["PRReport"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["PRReport"].ToString();

            string AddpayrollFormPermission = payrollFormPermission.Length < 1 ? "Y" : payrollFormPermission.Substring(0, 1);
            string EdipayrollFormPermission = payrollFormPermission.Length < 2 ? "Y" : payrollFormPermission.Substring(1, 1);
            string DeletepayrollFormPermission = payrollFormPermission.Length < 3 ? "Y" : payrollFormPermission.Substring(2, 1);
            string ViewpayrollFormPermission = payrollFormPermission.Length < 4 ? "Y" : payrollFormPermission.Substring(3, 1);

            payrollformAdd.Checked = (AddpayrollFormPermission == "N") ? false : true;
            payrollformEdit.Checked = (EdipayrollFormPermission == "N") ? false : true;
            payrollformDelete.Checked = (DeletepayrollFormPermission == "N") ? false : true;
            payrollformView.Checked = (ViewpayrollFormPermission == "N") ? false : true;

            //wages Permission 
            string wagesPermission = roleInfo.Rows[0]["PRWage"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["PRWage"].ToString();

            string AddwagesPermission = wagesPermission.Length < 1 ? "Y" : wagesPermission.Substring(0, 1);
            string EditwagesPermission = wagesPermission.Length < 2 ? "Y" : wagesPermission.Substring(1, 1);
            string DeletewagesPermission = wagesPermission.Length < 3 ? "Y" : wagesPermission.Substring(2, 1);
            string ViewwagesPermission = wagesPermission.Length < 4 ? "Y" : wagesPermission.Substring(3, 1);

            wagesadd.Checked = (AddwagesPermission == "N") ? false : true;
            wagesEdit.Checked = (EditwagesPermission == "N") ? false : true;
            wagesDelete.Checked = (DeletewagesPermission == "N") ? false : true;
            wagesView.Checked = (ViewwagesPermission == "N") ? false : true;

            //Payroll Permission Employee
            string deductionPermission = roleInfo.Rows[0]["PRDeduct"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["PRDeduct"].ToString();

            string AddedeductionPermission = deductionPermission.Length < 1 ? "Y" : deductionPermission.Substring(0, 1);
            string EditdeductionPermission = deductionPermission.Length < 2 ? "Y" : deductionPermission.Substring(1, 1);
            string DeletedeductionPermission = deductionPermission.Length < 3 ? "Y" : deductionPermission.Substring(2, 1);
            string ViewdeductionPermission = deductionPermission.Length < 4 ? "Y" : deductionPermission.Substring(3, 1);

            deductionsAdd.Checked = (AddedeductionPermission == "N") ? false : true;
            deductionsEdit.Checked = (EditdeductionPermission == "N") ? false : true;
            deductionsDelete.Checked = (DeletedeductionPermission == "N") ? false : true;
            deductionsView.Checked = (ViewdeductionPermission == "N") ? false : true;

            int taskPermission = roleInfo.Rows[0]["ToDo"] == DBNull.Value ? 0 : Convert.ToInt32(roleInfo.Rows[0]["ToDo"]);
            if (taskPermission == 1)
            {
                chkTasks.Checked = true;
            }
            else
            {
                chkTasks.Checked = false;
            }

            int completeTaskPermission = roleInfo.Rows[0]["ToDoC"] == DBNull.Value ? 0 : Convert.ToInt32(roleInfo.Rows[0]["ToDoC"]);
            if (completeTaskPermission == 1)
            {
                chkCompleteTask.Checked = true;
            }
            else
            {
                chkCompleteTask.Checked = false;
            }

            string FollowUpPermission = roleInfo.Rows[0]["FU"] == DBNull.Value ? "YYYYYY" : roleInfo.Rows[0]["FU"].ToString();

            chkFollowUp.Checked = (FollowUpPermission == "NNNNNN") ? false : true;

            string EstimatesPermission = roleInfo.Rows[0]["Estimates"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Estimates"].ToString();
            string EstimatesaddPermission = EstimatesPermission.Length < 1 ? "Y" : EstimatesPermission.Substring(0, 1);
            string EstimateseditPermission = EstimatesPermission.Length < 2 ? "Y" : EstimatesPermission.Substring(1, 1);
            string EstimatesdeletePermission = EstimatesPermission.Length < 3 ? "Y" : EstimatesPermission.Substring(2, 1);
            string EstimatesviewPermission = EstimatesPermission.Length < 4 ? "Y" : EstimatesPermission.Substring(3, 1);
            string EstimatesReportPermission = EstimatesPermission.Length < 6 ? "Y" : EstimatesPermission.Substring(5, 1);

            chkEstimateAdd.Checked = (EstimatesaddPermission == "N") ? false : true;
            chkEstimateEdit.Checked = (EstimateseditPermission == "N") ? false : true;
            chkEstimateDelete.Checked = (EstimatesdeletePermission == "N") ? false : true;
            chkEstimateView.Checked = (EstimatesviewPermission == "N") ? false : true;
            chkEstimateReport.Checked = (EstimatesReportPermission == "N") ? false : true;

            string AwardEstimatesPermission = roleInfo.Rows[0]["AwardEstimates"] == DBNull.Value ? "YYYYYY" : roleInfo.Rows[0]["AwardEstimates"].ToString();

            chkConvertEstimate.Checked = (AwardEstimatesPermission == "NNNNNN") ? false : true;

            string salesSetupPermission = roleInfo.Rows[0]["salessetup"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["salessetup"].ToString();

            chkSalesSetup.Checked = (salesSetupPermission == "NNNNNN") ? false : true;

            string proposalPermission = roleInfo.Rows[0]["Proposal"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Proposal"].ToString();
            string oppaddPermission = proposalPermission.Length < 1 ? "Y" : proposalPermission.Substring(0, 1);
            string oppeditPermission = proposalPermission.Length < 2 ? "Y" : proposalPermission.Substring(1, 1);
            string oppdeletePermission = proposalPermission.Length < 3 ? "Y" : proposalPermission.Substring(2, 1);
            string oppviewPermission = proposalPermission.Length < 4 ? "Y" : proposalPermission.Substring(3, 1);
            string oppReportPermission = proposalPermission.Length < 6 ? "Y" : proposalPermission.Substring(5, 1);

            chkOppAdd.Checked = (oppaddPermission == "N") ? false : true;
            chkOppEdit.Checked = (oppeditPermission == "N") ? false : true;
            chkOppDelete.Checked = (oppdeletePermission == "N") ? false : true;
            chkOppView.Checked = (oppviewPermission == "N") ? false : true;
            chkOppReport.Checked = (oppReportPermission == "N") ? false : true;

            #region ticketPermission


            /////// ticketPermission New

            // TicketVoidPermission

            string strTicketVoidPermission = roleInfo.Rows[0]["TicketVoidPermission"] == DBNull.Value ? "N" : roleInfo.Rows[0]["TicketVoidPermission"].ToString();

            chkTicketVoidPermission.Checked = (strTicketVoidPermission == "0") ? false : true;

            // MassPayrollTicket
            string strMassPayrollTicket = roleInfo.Rows[0]["MassPayrollTicket"] == DBNull.Value ? "N" : roleInfo.Rows[0]["MassPayrollTicket"].ToString();

            chkMassPayrollTicket1.Checked = chkMassPayrollTicket.Checked = (strMassPayrollTicket == "Y") ? true : false;

            // MassTimesheetCheck

            string strMassTimesheetCheck = roleInfo.Rows[0]["MassTimesheetCheck"] == DBNull.Value ? "N" : roleInfo.Rows[0]["MassTimesheetCheck"].ToString();

            chkMassTimesheetCheck.Checked = (strMassTimesheetCheck == "N") ? false : true;

            string ticketPermission = roleInfo.Rows[0]["Dispatch"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Dispatch"].ToString();

            string AddTicket = ticketPermission.Length < 1 ? "Y" : ticketPermission.Substring(0, 1);
            string EditTicket = ticketPermission.Length < 2 ? "Y" : ticketPermission.Substring(1, 1);
            string DeleteTicket = ticketPermission.Length < 3 ? "Y" : ticketPermission.Substring(2, 1);
            string ViewTicket = ticketPermission.Length < 4 ? "Y" : ticketPermission.Substring(3, 1);

            chkTicketListAdd.Checked = (AddTicket == "N") ? false : true;
            chkTicketListEdit.Checked = (EditTicket == "N") ? false : true;
            chkTicketListDelete.Checked = (DeleteTicket == "N") ? false : true;
            chkTicketListView.Checked = (ViewTicket == "N") ? false : true;

            /////// ApplyPermission New

            string ApplyPermission = roleInfo.Rows[0]["Apply"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Apply"].ToString();
            string AddReceivePayment = ApplyPermission.Length < 1 ? "Y" : ApplyPermission.Substring(0, 1);
            string EditReceivePayment = ApplyPermission.Length < 2 ? "Y" : ApplyPermission.Substring(1, 1);
            string DeleteReceivePayment = ApplyPermission.Length < 3 ? "Y" : ApplyPermission.Substring(2, 1);
            string ViewReceivePayment = ApplyPermission.Length < 4 ? "Y" : ApplyPermission.Substring(3, 1);


            chkReceivePaymentAdd.Checked = (AddReceivePayment == "N") ? false : true;
            chkReceivePaymentEdit.Checked = (EditReceivePayment == "N") ? false : true;
            chkReceivePaymentDelete.Checked = (DeleteReceivePayment == "N") ? false : true;
            chkReceivePaymentView.Checked = (ViewReceivePayment == "N") ? false : true;

            //DepositPermission
            string DepositPermission = roleInfo.Rows[0]["Deposit"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Deposit"].ToString();

            string AddDepositPermission = DepositPermission.Length < 1 ? "Y" : DepositPermission.Substring(0, 1);
            string EditDepositPermission = DepositPermission.Length < 2 ? "Y" : DepositPermission.Substring(1, 1);
            string DeleteDepositPermission = DepositPermission.Length < 3 ? "Y" : DepositPermission.Substring(2, 1);
            string ViewDepositPermission = DepositPermission.Length < 4 ? "Y" : DepositPermission.Substring(3, 1);

            chkMakeDepositAdd.Checked = (AddDepositPermission == "N") ? false : true;
            chkMakeDepositEdit.Checked = (EditDepositPermission == "N") ? false : true;
            chkMakeDepositDelete.Checked = (DeleteDepositPermission == "N") ? false : true;
            chkMakeDepositView.Checked = (ViewDepositPermission == "N") ? false : true;


            //CollectionsPermission
            string CollectionsPermission = roleInfo.Rows[0]["Collection"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Collection"].ToString();

            string AddCollectionsPermission = CollectionsPermission.Length < 1 ? "Y" : CollectionsPermission.Substring(0, 1);
            string EditCollectionsPermission = CollectionsPermission.Length < 2 ? "Y" : CollectionsPermission.Substring(1, 1);
            string DeleteCollectionsPermission = CollectionsPermission.Length < 3 ? "Y" : CollectionsPermission.Substring(2, 1);
            string ViewCollectionsPermission = CollectionsPermission.Length < 4 ? "Y" : CollectionsPermission.Substring(3, 1);
            string ReportCollectionsPermission = CollectionsPermission.Length < 6 ? "Y" : CollectionsPermission.Substring(5, 1);

            chkCollectionsAdd.Checked = (AddCollectionsPermission == "N") ? false : true;
            chkCollectionsEdit.Checked = (EditCollectionsPermission == "N") ? false : true;
            chkCollectionsDelete.Checked = (DeleteCollectionsPermission == "N") ? false : true;
            chkCollectionsView.Checked = (ViewCollectionsPermission == "N") ? false : true;
            chkCollectionsReport.Checked = (ReportCollectionsPermission == "N") ? false : true;

            #endregion

            if (roleInfo.Rows[0]["Control"].ToString() != string.Empty)
            {
                ProgFunc = roleInfo.Rows[0]["Control"].ToString().Substring(0, 1);
            }
            if (roleInfo.Rows[0]["users"].ToString() != string.Empty)
            {
                AccessUser = roleInfo.Rows[0]["users"].ToString().Substring(0, 1);
            }

            if (EmpMaintenace == "Y")
            {
                chkEmpMainten.Checked = true;
            }
            else
            {
                chkEmpMainten.Checked = false;
            }

            if (TCTimeFix == "Y")
                chkTimestampFix.Checked = true;
            else
                chkTimestampFix.Checked = false;

            if (LocnRemark == "Y")
            {
                chkLocationview.Checked = true;
            }
            else
            {
                chkLocationview.Checked = false;
            }

            if (PurcOrders == "Y")
            {
                chkPOAdd.Checked = true;
            }
            else
            {
                chkPOAdd.Checked = false;
            }

            if (Exp == "Y")
            {
                chkExpenses.Checked = true;
            }
            else
            {
                chkExpenses.Checked = false;
            }

            if (ProgFunc == "Y")
            {
                chkProgram.Checked = true;
            }
            else
            {
                chkProgram.Checked = false;
            }

            if (AccessUser == "Y")
            {
                chkAccessUser.Checked = true;
            }
            else
            {
                chkAccessUser.Checked = false;
            }

            if (Sales == "Y")
            {
                chkSalesMgr.Checked = true;
            }
            else
            {
                chkSalesMgr.Checked = false;
            }

            chkEstApprovalStatus.Checked = roleInfo.Rows[0]["EstApproveProposal"] == null ? false : Convert.ToBoolean(roleInfo.Rows[0]["EstApproveProposal"]);
        }

        protected void lnkLoadLogs_Click(object sender, EventArgs e)
        {
            hdnLoadLogs.Value = "1";
            RadGrid_gvLogs.Rebind();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
        }

        protected void RadGrid_gvLogs_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (totalCount == 0) totalCount = 1000;

                GeneralFunctions obj = new GeneralFunctions();

                var sizes = obj.TelerikPageSize(totalCount);

                dropDown.Items.Clear();

                foreach (var size in sizes)
                {
                    var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                    cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                    if (e.Item.OwnerTableView.PageSize.ToString() == size.Value) cboItem.Selected = true;
                    dropDown.Items.Add(cboItem);
                }
            }
        }

        protected void RadGrid_gvLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                BindLogs();
            }
            catch { }
        }

        private void BindLogs()
        {
            if (Request.QueryString["uid"] != null && hdnLoadLogs.Value == "1")
            {
                DataSet dsLog = new DataSet();
                Customer objProp_Customer = new Customer();
                BL_Customer objBL_Customer = new BL_Customer();
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.LogRefId = Convert.ToInt32(Request.QueryString["uid"]);
                objProp_Customer.LogScreen = "User Role";
                dsLog = objBL_Customer.GetLogs(objProp_Customer);
                if (dsLog.Tables[0].Rows.Count > 0)
                {
                    RadGrid_gvLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
                    RadGrid_gvLogs.DataSource = dsLog.Tables[0];
                }
                else
                {
                    RadGrid_gvLogs.DataSource = string.Empty;
                }
            }
            else
            {
                RadGrid_gvLogs.DataSource = string.Empty;
            }
        }

        private void GetControlForPayroll()
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_Report.GetControlForPayroll(objPropUser);
            bool PR = Convert.ToBoolean(DBNull.Value.Equals(ds.Tables[0].Rows[0]["PR"]) ? 0 : ds.Tables[0].Rows[0]["PR"]);

            if (PR == true)
            {
                payrollsection.Visible = true;
                chkEmpMainten.Visible = false;
                chkMassPayrollTicket.Visible = false;
                chkMassPayrollTicket1.Visible = true;
            }
            else
            {
                chkMassPayrollTicket1.Visible = false;
                chkMassPayrollTicket.Visible = true;
            }
        }
    }
}