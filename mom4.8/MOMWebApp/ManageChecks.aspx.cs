using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using System.Web.UI.HtmlControls;
using System.Globalization;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using Stimulsoft.Report;
using System.IO;
using Microsoft.Reporting.WebForms;
using BusinessEntity.Utility;
using MOMWebApp;
using System.Web.Script.Serialization;
using BusinessEntity.APModels;
using BusinessEntity.Payroll;
using BusinessEntity.CommonModel;
using System.Text.RegularExpressions;

public partial class ManageChecks : System.Web.UI.Page
{
    #region "Variables"
    CD _objCD = new CD();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BL_Bills _objBLBill = new BL_Bills();
    BL_User objBL_User = new BL_User();
    Paid _objPaid = new Paid();

    Transaction _objTrans = new Transaction();
    Journal _objJournal = new Journal();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();

    JobI _objJobI = new JobI();
    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    PJ _objPJ = new PJ();
    OpenAP _objOpenAP = new OpenAP();

    Vendor _objVendor = new Vendor();
    BL_Vendor _objBLVendor = new BL_Vendor();
    Bank _objBank = new Bank();
    BL_BankAccount _objBL_Bank = new BL_BankAccount();
    protected DataTable dti = new DataTable();
    protected DataTable dtpay = new DataTable();
    protected DataTable dtBank = new DataTable();

    PRReg _objPRReg = new PRReg();
    BL_Wage objBL_Wage = new BL_Wage();
    Emp objProp_Emp = new Emp();

    //API Variables
    //string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    APIIntegrationModel _objAPIIntegration = new APIIntegrationModel();
    GetAllBankNamesParam _getAllBankNames = new GetAllBankNamesParam();
    GetCDByIDParam _getCDByID = new GetCDByIDParam();
    UpdateCDVoidParam _updateCDVoid = new UpdateCDVoidParam();
    GetTransByIDParam _getTransByID = new GetTransByIDParam();
    UpdateTransVoidCheckParam _updateTransVoidCheck = new UpdateTransVoidCheckParam();
    UpdateTransVoidCheckByBatchParam _updateTransVoidCheckByBatch = new UpdateTransVoidCheckByBatchParam();
    GetPaidDetailByIDParam _getPaidDetailByID = new GetPaidDetailByIDParam();
    GetTransByBatchParam _getTransByBatch = new GetTransByBatchParam();
    GetMaxTransBatchParam _getMaxTransBatch = new GetMaxTransBatchParam();
    GetMaxTransRefParam _getMaxTransRef = new GetMaxTransRefParam();
    AddGLAParam _addGLA = new AddGLAParam();
    GetAcctPayableParam _getAcctPayable = new GetAcctPayableParam();
    AddJournalTransParam _addJournalTrans = new AddJournalTransParam();
    UpdateChartBalanceParam _updateChartBalance = new UpdateChartBalanceParam();
    GetBankAcctIDParam _getBankAcctID = new GetBankAcctIDParam();
    GetJobIByTransIDParam _getJobIByTransID = new GetJobIByTransIDParam();
    AddJobIParam _addJobI = new AddJobIParam();
    GetPJDetailByBatchParam _getPJDetailByBatch = new GetPJDetailByBatchParam();
    AddPJParam _addPJ = new AddPJParam();
    UpdatePJOnVoidCheckParam _updatePJOnVoidCheck = new UpdatePJOnVoidCheckParam();
    GetOpenAPByPJIDParam _getOpenAPByPJID = new GetOpenAPByPJIDParam();
    AddOpenAPParam _addOpenAP = new AddOpenAPParam();
    UpdateVendorBalanceParam _updateVendorBalance = new UpdateVendorBalanceParam();
    GetAllCDParam _getAllCD = new GetAllCDParam();
    DeleteRecurrCheckParam _deleteRecurrCheck = new DeleteRecurrCheckParam();
    GetProcessRecurrCheckCountParam _getProcessRecurrCheckCount = new GetProcessRecurrCheckCountParam();
    DeleteCheckDetailsParam _deleteCheckDetails = new DeleteCheckDetailsParam();
    GetCheckRecurrDetailsParam _getCheckRecurrDetails = new GetCheckRecurrDetailsParam();
    UpdateOpenAPBalanceParam _updateOpenAPBalance = new UpdateOpenAPBalanceParam();
    GetDataTypeCDParam _getDataTypeCD = new GetDataTypeCDParam();
    GetCheckDetailsByBankAndRefParam _getCheckDetailsByBankAndRef = new GetCheckDetailsByBankAndRefParam();
    GetVendorRolDetailsParam _getVendorRolDetails = new GetVendorRolDetailsParam();
    GetControlBranchParam _getControlBranch = new GetControlBranchParam();
    UpdateCDVoidOpenParam _updateCDVoidOpen = new UpdateCDVoidOpenParam();
    UpdateAPCDVoidLogParam _updateAPCDVoidLog = new UpdateAPCDVoidLogParam();
    UpdateTransVoidCheckOpenParam _updateTransVoidCheckOpen = new UpdateTransVoidCheckOpenParam();
    UpdateTransVoidCheckByBatchOpenParam _updateTransVoidCheckByBatchOpen = new UpdateTransVoidCheckByBatchOpenParam();
    ProcessRecurCheckParam _processRecurCheck = new ProcessRecurCheckParam();
    UpdatePaidOnVoidCheckParam _updatePaidOnVoidCheck = new UpdatePaidOnVoidCheckParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetUserByIdParam _getUserById = new GetUserByIdParam();
    UpdateCDCheckNoParam _updateCDCheckNo = new UpdateCDCheckNoParam();
    UpdateTransCheckNoByBatchParam _updateTransCheckNoByBatch = new UpdateTransCheckNoByBatchParam();
    GetBankCDParam _getBankCD = new GetBankCDParam();
    GetVendorAcctParam _getVendorAcct = new GetVendorAcctParam();
    #endregion

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            BindSearchFilters();
            BindPayType();
            BindStatus();
            BindEmployee();
            GetControlForPayroll();
            #region Show Selected Filter
            if (Session["from_Checks"] != null && Session["end_Checks"] != null)
            {
                txtFromDate.Text = Convert.ToString(Session["from_Checks"]);
                txtToDate.Text = Convert.ToString(Session["end_Checks"]);
            }
            else
            {
                DateTime _now = DateTime.Now;
                var _startDate = new DateTime(_now.Year, _now.Month, 1);
                var _endDate = _startDate.AddMonths(1).AddDays(-1);
                txtFromDate.Text = _startDate.ToShortDateString();
                txtToDate.Text = _endDate.ToShortDateString();
                Session["from_Checks"] = txtFromDate.Text;
                Session["end_Checks"] = txtToDate.Text;
            }
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["PayCheckfromDate"] != null && Session["PayCheckToDate"] != null)
                {
                    txtFromDatePayCheck.Text = Session["PayCheckfromDate"].ToString();
                    txtToDatePayCheck.Text = Session["PayCheckToDate"].ToString();
                }
                else
                {
                    txtFromDatePayCheck.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtToDatePayCheck.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                    Session["PayCheckfromDate"] = txtFromDatePayCheck.Text;
                    Session["PayCheckToDate"] = txtToDatePayCheck.Text;
                }

                if (Session["ddlSearch_Checks"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearch_Checks"]);
                    ddlSearch.SelectedValue = selectedValue;

                    String searchValue = Convert.ToString(Session["ddlSearch_Value_Checks"]);
                    if (selectedValue == "Vendor" || selectedValue == "CheckNum" || selectedValue == "Bank")
                    {
                        txtSearch.Text = searchValue;
                    }
                    else if (selectedValue == "Status")
                    {
                        ddlSearch_SelectedIndexChanged(sender, e);
                        ddlStatus.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "PayType")
                    {
                        ddlSearch_SelectedIndexChanged(sender, e);
                        ddlPaytype.SelectedValue = searchValue;
                    }
                    else
                    {
                        txtSearch.Text = searchValue;
                    }

                }
                if (Session["Check_Type"] == null)
                {
                    rdocheck.Checked = true;
                    rdoRecurring.Checked = false;
                    //--------------Start: By Juily - 19-12-2019----------------------//
                    //lnkVoidCheck.Style["display"] = "block";
                    //lnkPrint.Style["display"] = "block";
                    //lnkEditCheckNum.Style["display"] = "block";
                    //btnReprintRange.Style["display"] = "block";
                    //btnchecknobill.Style["display"] = "block";
                    lnkVoidCheck.Visible = true;
                    lnkPrint.Visible = true;
                    lnkEditCheckNum.Visible = true;
                    btnReprintRange.Visible = true;
                    btnchecknobill.Visible = true;
                    lnkProcess.Visible = false;
                }
                else
                {
                    if (Session["Check_Type"].ToString() == "1")
                    {
                        rdocheck.Checked = true;
                        rdoRecurring.Checked = false;
                        //--------------Start: By Juily - 19-12-2019----------------------//
                        //lnkVoidCheck.Style["display"] = "block";
                        //lnkPrint.Style["display"] = "block";
                        //lnkEditCheckNum.Style["display"] = "block";
                        //btnReprintRange.Style["display"] = "block";
                        //btnchecknobill.Style["display"] = "block";
                        lnkVoidCheck.Visible = true;
                        lnkPrint.Visible = true;
                        lnkEditCheckNum.Visible = true;
                        btnReprintRange.Visible = true;
                        btnchecknobill.Visible = true;
                        lnkProcess.Visible = false;
                        //--------------------End: By Juily - 19-12-2019-----------------//

                    }
                    else
                    {
                        rdocheck.Checked = false;
                        rdoRecurring.Checked = true;
                        //--------------Start: By Juily - 19-12-2019----------------------//
                        //lnkVoidCheck.Style["display"] = "none";
                        //lnkPrint.Style["display"] = "none";
                        //lnkEditCheckNum.Style["display"] = "none";
                        //btnReprintRange.Style["display"] = "none";
                        //btnchecknobill.Style["display"] = "none";
                        lnkVoidCheck.Visible = false;
                        lnkPrint.Visible = false;
                        lnkEditCheckNum.Visible = false;
                        btnReprintRange.Visible = false;
                        btnchecknobill.Visible = false;
                        //--------------------End: By Juily - 19-12-2019-----------------//
                        rdoRecurring_CheckedChanged(sender, e);
                    }
                }
            }
            else
            {
                Session["ddlSearch_Checks"] = null;
                Session["ddlSearch_Value_Checks"] = null;
                Session["Check_Type"] = null;
                lnkProcess.Visible = false;
                rdocheck.Checked = true;

                Session["from_Checks"] = null;
                Session["end_Checks"] = null;
                DateTime _now = DateTime.Now;
                var _startDate = new DateTime(_now.Year, _now.Month, 1);
                var _endDate = _startDate.AddMonths(1).AddDays(-1);
                txtFromDate.Text = _startDate.ToShortDateString();
                txtToDate.Text = _endDate.ToShortDateString();
                Session["from_Checks"] = txtFromDate.Text;
                Session["end_Checks"] = txtToDate.Text;

                txtFromDatePayCheck.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtToDatePayCheck.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                Session["PayCheckfromDate"] = txtFromDatePayCheck.Text;
                Session["PayCheckToDate"] = txtToDatePayCheck.Text;
            }
            #endregion            
            UserPermission();
            FillBank();
            if (!IsPostBack)
            {
                string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

                if (Request.Url.Scheme == "http" && SSL == "1")
                {
                    string URL = Request.Url.ToString();
                    URL = URL.Replace("http://", "https://");
                    Response.Redirect(URL);
                }

                string path = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/");
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.mrt");
                foreach (FileInfo file in Files)
                {

                    string FileName = string.Empty;
                    if (file.Name.Contains(".mrt"))
                        FileName = file.Name.Replace(".mrt", " ");
                    ddlApTopCheckForLoad.Items.Add((FileName));
                }

                ddlApTopCheckForLoad.DataBind();


                string MidCheckpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/");
                DirectoryInfo dirMidPath = new DirectoryInfo(MidCheckpath);
                FileInfo[] FilesMid = dirMidPath.GetFiles("*.mrt");
                foreach (FileInfo fileMid in FilesMid)
                {
                    string FileName = string.Empty;
                    if (fileMid.Name.Contains(".mrt"))
                        FileName = fileMid.Name.Replace(".mrt", " ");
                    ddlApMiddleCheckForLoad.Items.Add((FileName));
                }

                ddlApMiddleCheckForLoad.DataBind();


                string TopCheckpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/");
                DirectoryInfo dirTopcheckPath = new DirectoryInfo(TopCheckpath);
                FileInfo[] FilesTop = dirTopcheckPath.GetFiles("*.mrt");
                foreach (FileInfo fileTop in FilesTop)
                {
                    string FileName = string.Empty;
                    if (fileTop.Name.Contains(".mrt"))
                        FileName = fileTop.Name.Replace(".mrt", " ");
                    ddlTopChecksForLoad.Items.Add((FileName));
                }
                ddlTopChecksForLoad.DataBind();

                //objProp_User.ConnConfig = Session["config"].ToString();
                //DataSet dsControl = new DataSet();
                //dsControl = objBL_User.getControl(objProp_User);


                //if (dsControl.Tables[0].Rows.Count > 0)
                //{
                //    lbltopcom.Text = dsControl.Tables[0].Rows[0]["Name"].ToString();
                //    lblmidcom.Text = dsControl.Tables[0].Rows[0]["Name"].ToString();
                //    lbldetailcom.Text = dsControl.Tables[0].Rows[0]["Name"].ToString();
                //    lbltopdd.Text = dsControl.Tables[0].Rows[0]["Address"].ToString() + " " + dsControl.Tables[0].Rows[0]["City"].ToString() + " " + dsControl.Tables[0].Rows[0]["State"].ToString() + ", " + dsControl.Tables[0].Rows[0]["Zip"].ToString();
                //    lblmidadd.Text = dsControl.Tables[0].Rows[0]["Address"].ToString() + " " + dsControl.Tables[0].Rows[0]["City"].ToString() + " " + dsControl.Tables[0].Rows[0]["State"].ToString() + ", " + dsControl.Tables[0].Rows[0]["Zip"].ToString();
                //    lbldetailadd.Text = dsControl.Tables[0].Rows[0]["Address"].ToString() + " " + dsControl.Tables[0].Rows[0]["City"].ToString() + " " + dsControl.Tables[0].Rows[0]["State"].ToString() + ", " + dsControl.Tables[0].Rows[0]["Zip"].ToString();
                //    lbltopemail.Text = dsControl.Tables[0].Rows[0]["Email"].ToString();
                //    lblmidemail.Text = dsControl.Tables[0].Rows[0]["Email"].ToString();
                //    lbldetailemail.Text = dsControl.Tables[0].Rows[0]["Email"].ToString();
                //}
            }

        }
        CompanyPermission();
        HighlightSideMenu("acctPayable", "lnkWriteCheck2", "acctPayableSub");
    }
    #endregion
    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();

        _getUserById.TypeID = Convert.ToInt32(Session["usertypeid"]);
        _getUserById.UserID = Convert.ToInt32(Session["userid"]);
        _getUserById.ConnConfig = Session["config"].ToString();
        _getUserById.DBName = Session["dbname"].ToString();

        DataSet ds = new DataSet();
        List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetUserById";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getUserById);

            _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
        }
        else
        {
            ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        }
        return ds.Tables[0];
    }
    private void UserPermission()
    {
        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// AccountPayablemodulePermission ///////////////////------->

            string AccountPayablemodulePermission = ds.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["AccountPayablemodulePermission"].ToString();

            if (AccountPayablemodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// BillPay ///////////////////------->

            string BillPayPermission = ds.Rows[0]["BillPay"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["BillPay"].ToString();
            string ADD = BillPayPermission.Length < 1 ? "Y" : BillPayPermission.Substring(0, 1);
            string Edit = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(1, 1);
            string Delete = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(2, 1);
            string View = BillPayPermission.Length < 4 ? "Y" : BillPayPermission.Substring(3, 1);
            if (ADD == "N")
            {

                lnkWriteCheck.Visible = false;
            }
            if (Edit == "N")
            {
                lnkEditCheck.Visible = false;
                lnkVoidCheck.Visible = false;
                lnkEditCheckNum.Visible = false;

            }
            if (Delete == "N")
            {
                lnkDeleteCheck.Visible = false;

            }
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
        }
    }
    private void CompanyPermission()
    {
        if (Session["COPer"] != null)
        {
            if (Session["COPer"].ToString() == "1")
            {
                RadGrid_Checks.Columns[9].Visible = true;
            }
            else
            {
                RadGrid_Checks.Columns[9].Visible = false;
            }
        }
    }
    protected void lnkWriteCheck_Click(object sender, EventArgs e)
    {
        if (rdoRecurring.Checked == true)
        {
            Response.Redirect("writechecks.aspx?bill=c");
        }
        else
        {
            Response.Redirect("writechecks.aspx");
        }
    }
    protected void btnchecknobill_Click(object sender, EventArgs e)
    {
        Response.Redirect("writechecks.aspx?bill=c");
    }
    protected void lnkEditCheck_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Checks.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            HiddenField hdnID = (HiddenField)di.FindControl("hdnID");
            if (chkSelect.Checked == true)
            {
                if (rdocheck.Checked == true)
                {
                    Response.Redirect("editcheck.aspx?id=" + hdnID.Value);
                }
                else if (rdoRecurring.Checked == true)
                {
                    //Response.Redirect("editcheck.aspx?id=" + hdnID.Value + "&r=1");
                    Response.Redirect("WriteChecksRecur.aspx?bill=c&id=" + hdnID.Value + "&frm=MNG");
                    
                }
            }
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
        Session.Remove("Checks");
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(hdnCDID.Value))
            {
                bool flag = GetPeriodDetails(Convert.ToDateTime(txtVoidDate.Text));
                if (!flag)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'This month/year period is closed out. You do not have permission to void check. Please maintain the journal entries manually.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
                else
                {
                    UpdateCheckDetails(Convert.ToInt32(hdnCDID.Value));
                    BindChecks();
                    RadGrid_Checks.Rebind();
                }
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_Checks"] = selectedValue;

        Session["from_Checks"] = txtFromDate.Text;
        Session["end_Checks"] = txtToDate.Text;

        if (selectedValue == "Vendor" || selectedValue == "CheckNum" || selectedValue == "Bank")
        {
            Session["ddlSearch_Value_Checks"] = txtSearch.Text;
        }
        else if (selectedValue == "Status")
        {
            Session["ddlSearch_Value_Checks"] = ddlStatus.SelectedValue;
        }
        else if (selectedValue == "PayType")
        {
            Session["ddlSearch_Value_Checks"] = ddlPaytype.SelectedValue;
        }
        else
        {
            Session["ddlSearch_Value_Checks"] = txtSearch.Text;
        }
        if (hdnCssActive.Value == "CssActive")
        {
            Session["lblChecksActive"] = "1";
        }
        else
        {
            Session["lblChecksActive"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }
        //BindChecks();
        RadGrid_Checks.Rebind();
    }

    [System.Web.Services.WebMethod]
    public static EditManageCheckModel VoidCheckEdit(string lblIndex)
    {

        String _Index = lblIndex.Replace("\n", "").Replace(" ", "");
        DataTable dt = (DataTable)HttpContext.Current.Session["Checks"];
        DataRow dr = dt.AsEnumerable().Where(x => x.Field<int>("ID") == Convert.ToInt32(_Index)).FirstOrDefault();//dt.Rows[Convert.ToInt32(_Index)];

        EditManageCheckModel data = new EditManageCheckModel();
        data.ID = dr["ID"].ToString();
        data.Sel = dr["Sel"].ToString();
        data.Ref = dr["Ref"].ToString();
        data.VoidDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
        data.fDate = dr["fDate"].ToString();
        data.Bank = dr["Bank"].ToString();
        data.lblIndex = lblIndex;
        return data;
    }
    protected void lnkDeleteCheck_Click(object sender, EventArgs e)
    {
        bool _isClosed = false;                             //here validation has been checked from javascript and backend side both.
        foreach (GridDataItem di in RadGrid_Checks.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];

            HiddenField hdnID = (HiddenField)di.FindControl("hdnID");
            HiddenField hdnSel = (HiddenField)di.FindControl("hdnSel");
            if (chkSelect.Checked == true)
            {
                if (Convert.ToInt16(hdnSel.Value).Equals(0))    //if check is open then one can delete the check details
                {
                    if (rdoRecurring.Checked == true)
                    {

                        _objCD.ConnConfig = Session["config"].ToString();
                        _objCD.ID = Convert.ToInt32(hdnID.Value);

                        _deleteRecurrCheck.ConnConfig = Session["config"].ToString();
                        _deleteRecurrCheck.ID = Convert.ToInt32(hdnID.Value);

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/CheckList_DeleteRecurrCheck";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _deleteRecurrCheck);
                        }
                        else
                        {
                            _objBLBill.DeleteRecurrCheck(_objCD);
                        }
                        
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Recurring Check deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Recurring JE deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        Button mpbtnNotifyRecur = this.Master.FindControl("btnNotifyRecur") as Button;
                        if (mpbtnNotifyRecur != null)
                        {
                            DataSet _dsRecurrCount = new DataSet();
                            _objCD.ConnConfig = Session["config"].ToString();
                            _getProcessRecurrCheckCount.ConnConfig = Session["config"].ToString();

                            List<CDViewModel> _lstCDViewModel = new List<CDViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_GetProcessRecurrCheckCount";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getProcessRecurrCheckCount);

                                _lstCDViewModel = (new JavaScriptSerializer()).Deserialize<List<CDViewModel>>(_APIResponse.ResponseData);
                                _dsRecurrCount = CommonMethods.ToDataSet<CDViewModel>(_lstCDViewModel);
                            }
                            else
                            {
                                _dsRecurrCount = _objBLBill.GetProcessRecurrCheckCount(_objCD);
                            }
                            if (_dsRecurrCount != null)
                            {
                                int _recurCount = Convert.ToInt32(_dsRecurrCount.Tables[0].Rows[0]["CountRecur"]);
                                mpbtnNotifyRecur.Text = _recurCount.ToString();
                            }
                        }
                        break;
                    }
                    else if (rdocheck.Checked == true)
                    {
                        _objCD.ConnConfig = Session["config"].ToString();
                        _objCD.ID = Convert.ToInt32(hdnID.Value);

                        _deleteCheckDetails.ConnConfig = Session["config"].ToString();
                        _deleteCheckDetails.ID = Convert.ToInt32(hdnID.Value);

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/CheckList_DeleteCheckDetails";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _deleteCheckDetails);
                        }
                        else
                        {
                            _objBLBill.DeleteCheckDetails(_objCD);
                        }
                    }
                }
                else
                {
                    _isClosed = true;
                }
            }
        }
        if (_isClosed)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "closedCheckWarning();", true);
        }
        else
        {
            if (rdocheck.Checked)
            {
                Session["Check_Type"] = "1";
                BindChecks();
            }
            else
            {
                Session["Check_Type"] = "2";
                BindRecurringGrid();

            }

            //BindChecks();
            RadGrid_Checks.Rebind();


        }
    }

    protected void lbtnCheckSave_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem di in RadGrid_Checks.SelectedItems)
            {
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                if (chkSelect.Checked == true)
                {
                    Label lblBatch = (Label)di.FindControl("lblBatch");

                    _objCD.ConnConfig = Session["config"].ToString();
                    _objCD.ID = Convert.ToInt32(hdnCD.Value);
                    //_objCD.fDate = Convert.ToDateTime(txtCheckDate.Text);
                    _objCD.Ref = Convert.ToInt32(txtCheckNo.Text);

                    _updateCDCheckNo.ConnConfig = Session["config"].ToString();
                    _updateCDCheckNo.ID = Convert.ToInt32(hdnCD.Value);
                    //_objCD.fDate = Convert.ToDateTime(txtCheckDate.Text);
                    _updateCDCheckNo.Ref = Convert.ToInt32(txtCheckNo.Text);

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_UpdateCDCheckNo";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateCDCheckNo);
                    }
                    else
                    {
                        _objBLBill.UpdateCDCheckNo(_objCD);
                    }

                    _objTrans.ConnConfig = Session["config"].ToString();
                    _objTrans.BatchID = Convert.ToInt32(lblBatch.Text);
                    //_objTrans.TransDate = Convert.ToDateTime(txtCheckDate.Text);
                    _objTrans.Ref = Convert.ToInt32(txtCheckNo.Text);

                    _updateTransCheckNoByBatch.ConnConfig = Session["config"].ToString();
                    _updateTransCheckNoByBatch.BatchID = Convert.ToInt32(lblBatch.Text);
                    //_objTrans.TransDate = Convert.ToDateTime(txtCheckDate.Text);
                    _updateTransCheckNoByBatch.Ref = Convert.ToInt32(txtCheckNo.Text);

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_UpdateTransCheckNoByBatch";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateTransCheckNoByBatch);
                    }
                    else
                    {
                        _objBLJournal.UpdateTransCheckNoByBatch(_objTrans);
                    }
                    // mpeEditCheckNo.Hide();                   
                    BindChecks();
                    RadGrid_Checks.Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region custom functions
    private void BindChecks()
    {
        
        //if (txtToDate.Text == "")
        //{
        //    DateTime _now = DateTime.Now;
        //    var _startDate = new DateTime(_now.Year, _now.Month, 1);
        //    var _endDate = _startDate.AddMonths(1).AddDays(-1);
        //    txtFromDate.Text = _startDate.ToShortDateString();
        //    txtToDate.Text = _endDate.ToShortDateString();
                        
        //}
        txtFromDate.Text = Session["from_Checks"].ToString();
        txtToDate.Text = Session["end_Checks"].ToString();
        if (txtToDate.Text == "")
        {
            txtFromDate.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
            txtToDate.Text = DateTime.Now.AddYears(100).ToShortDateString();
        }

        if (IsValidDate())
        {
            //txtFromDate.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
            //txtToDate.Text = DateTime.Now.ToShortDateString();

            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.StartDate = Convert.ToDateTime(txtFromDate.Text);
            _objCD.EndDate = Convert.ToDateTime(txtToDate.Text);
            _objCD.searchterm = ddlSearch.SelectedValue;
            _objCD.UserID = Convert.ToInt32(Session["UserID"].ToString());

            _getAllCD.ConnConfig = Session["config"].ToString();
            _getAllCD.StartDate = Convert.ToDateTime(txtFromDate.Text);
            _getAllCD.EndDate = Convert.ToDateTime(txtToDate.Text);
            _getAllCD.searchterm = ddlSearch.SelectedValue;
            _getAllCD.UserID = Convert.ToInt32(Session["UserID"].ToString());

            if (ddlSearch.SelectedValue == "Status")
            {
                _objCD.searchvalue = ddlStatus.SelectedValue;
                _getAllCD.searchvalue = ddlStatus.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "PayType")
            {
                _objCD.searchvalue = ddlPaytype.SelectedValue;
                _getAllCD.searchvalue = ddlPaytype.SelectedValue;

            }
            else
            {
                _objCD.searchvalue = txtSearch.Text;
                _getAllCD.searchvalue = txtSearch.Text;
            }
            if (Session["CmpChkDefault"].ToString() == "1")
            {
                _objCD.EN = 1;
                _getAllCD.EN = 1;
            }
            else
            {
                _objCD.EN = 0;
                _getAllCD.EN = 0;
            }
            DataSet _dsCheck = new DataSet();

            List<GetAllCDViewModel> _lstGetAllCD = new List<GetAllCDViewModel>();

            _objCD.PageNumber= RadGrid_Checks.CurrentPageIndex + 1;
            //_objCD.PageSize = RadGrid_Checks.PageSize;

            _getAllCD.PageNumber = RadGrid_Checks.CurrentPageIndex + 1;
            //_getAllCD.PageSize = RadGrid_Checks.PageSize;

            string filterexpression = string.Empty;
            filterexpression = RadGrid_Checks.MasterTableView.FilterExpression;
            if (filterexpression != "")
            {
                _objCD.PageSize = RadGrid_Checks.VirtualItemCount;
                _getAllCD.PageSize = RadGrid_Checks.VirtualItemCount;
            }
            else
            {
                _objCD.PageSize = RadGrid_Checks.PageSize;
                _getAllCD.PageSize = RadGrid_Checks.PageSize;
                
            }

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetAllCD";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllCD);

                _lstGetAllCD = (new JavaScriptSerializer()).Deserialize<List<GetAllCDViewModel>>(_APIResponse.ResponseData);
                _dsCheck = CommonMethods.ToDataSet<GetAllCDViewModel>(_lstGetAllCD);
            }
            else
            {
                _dsCheck = _objBLBill.GetAllCD(_objCD);
            }

            Session["Checks"] = _dsCheck.Tables[0];
            //RadGrid_Checks.VirtualItemCount = _dsCheck.Tables[0].Rows.Count;
            int totalRow = 0;
            if (_dsCheck.Tables[0].Rows.Count > 0)
                totalRow = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["TotalRow"]);
            RadGrid_Checks.VirtualItemCount = totalRow;
            lblRecordCount.Text = totalRow.ToString() + " Record(s) found";
            RadGrid_Checks.DataSource = _dsCheck;

        }
        txtFromDate.Text = Session["from_Checks"].ToString();
        txtToDate.Text = Session["end_Checks"].ToString();
    }
    private void BindCheckshowAll()
    {
        //if (IsValidDate())
        //{

            string stdate = "1900-01-01 00:00:00";
            string enddate = DateTime.Now.ToShortDateString() + " 23:59:59";

            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.StartDate = Convert.ToDateTime(stdate);
            _objCD.EndDate = Convert.ToDateTime(enddate);
            _objCD.searchterm = ddlSearch.SelectedValue;
            _objCD.UserID = Convert.ToInt32(Session["UserID"].ToString());

            _getAllCD.ConnConfig = Session["config"].ToString();
            _getAllCD.StartDate = Convert.ToDateTime(stdate);
            _getAllCD.EndDate = Convert.ToDateTime(enddate);
            _getAllCD.searchterm = ddlSearch.SelectedValue;
            _getAllCD.UserID = Convert.ToInt32(Session["UserID"].ToString());

            if (ddlSearch.SelectedValue == "Status")
            {
                _objCD.searchvalue = ddlStatus.SelectedValue;
                _getAllCD.searchvalue = ddlStatus.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "PayType")
            {
                _objCD.searchvalue = ddlPaytype.SelectedValue;
                _getAllCD.searchvalue = ddlPaytype.SelectedValue;

            }
            else
            {
                _objCD.searchvalue = txtSearch.Text;
                _getAllCD.searchvalue = txtSearch.Text;
            }
            if (Session["CmpChkDefault"].ToString() == "1")
            {
                _objCD.EN = 1;
                _getAllCD.EN = 1;
            }
            else
            {
                _objCD.EN = 0;
                _getAllCD.EN = 0;
            }
            DataSet _dsCheck = new DataSet();

            List<GetAllCDViewModel> _lstGetAllCD = new List<GetAllCDViewModel>();

            _objCD.PageNumber = RadGrid_Checks.CurrentPageIndex + 1;
            //_objCD.PageSize = RadGrid_Checks.PageSize;

            _getAllCD.PageNumber = RadGrid_Checks.CurrentPageIndex + 1;
            //_getAllCD.PageSize = RadGrid_Checks.PageSize;

            string filterexpression = string.Empty;
            filterexpression = RadGrid_Checks.MasterTableView.FilterExpression;
            if (filterexpression != "")
            {
                _objCD.PageSize = RadGrid_Checks.VirtualItemCount;
                _getAllCD.PageSize = RadGrid_Checks.VirtualItemCount;
            }
            else
            {
                _objCD.PageSize = RadGrid_Checks.PageSize;
                _getAllCD.PageSize = RadGrid_Checks.PageSize;

            }

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ManageChecksAPI/CheckList_GetAllCD";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllCD);

                _lstGetAllCD = (new JavaScriptSerializer()).Deserialize<List<GetAllCDViewModel>>(_APIResponse.ResponseData);
                _dsCheck = CommonMethods.ToDataSet<GetAllCDViewModel>(_lstGetAllCD);
            }
            else
            {
                _dsCheck = _objBLBill.GetAllCD(_objCD);
            }

            Session["Checks"] = _dsCheck.Tables[0];
            //RadGrid_Checks.VirtualItemCount = _dsCheck.Tables[0].Rows.Count;
            int totalRow = 0;
            if (_dsCheck.Tables[0].Rows.Count > 0)
                totalRow = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["TotalRow"]);
            RadGrid_Checks.VirtualItemCount = totalRow;
        lblRecordCount.Text = totalRow.ToString() + " Record(s) found";
        RadGrid_Checks.DataSource = _dsCheck;

        //}
    }

    private void UpdateCheckDetails(int _cdId)
    {
        try
        {
            bool flag = GetPeriodDetails(Convert.ToDateTime(txtVoidDate.Text));
            if (!flag)    // Period Close Out 
            {
                _objCD.ConnConfig = Session["config"].ToString();
                _objCD.ID = Convert.ToInt32(hdnCDID.Value);

                _getCDByID.ConnConfig = Session["config"].ToString();
                _getCDByID.ID = Convert.ToInt32(hdnCDID.Value);

                _updateCDVoid.ID = Convert.ToInt32(hdnCDID.Value);

                DataSet _dsCD = new DataSet();
                
                List<GetCDByIDViewModel> _lstGetCDByIDViewModel = new List<GetCDByIDViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetCDByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCDByID);

                    _lstGetCDByIDViewModel = (new JavaScriptSerializer()).Deserialize<List<GetCDByIDViewModel>>(_APIResponse.ResponseData);
                    _dsCD = CommonMethods.ToDataSet<GetCDByIDViewModel>(_lstGetCDByIDViewModel);
                    _dsCD.Tables[0].Columns["Acct"].ColumnName = "Acct#";
                }
                else
                {
                    _dsCD = _objBLBill.GetCDByID(_objCD);
                }
                if (_dsCD.Tables[0].Rows.Count > 0)
                {
                    DataRow _drCD = _dsCD.Tables[0].Rows[0];
                    _objCD.ConnConfig = Session["config"].ToString();
                    _objCD.fDesc = "Voided by " + Session["username"].ToString() + " on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " - " + Convert.ToString(_drCD["fDesc"]);


                    _updateCDVoid.ConnConfig = Session["config"].ToString();
                    _updateCDVoid.fDesc = "Voided by " + Session["username"].ToString() + " on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " - " + Convert.ToString(_drCD["fDesc"]);

                    if (_objCD.fDesc.Length > 249)
                    {
                        _objCD.fDesc = _objCD.fDesc.Substring(0, 248);
                    }

                    if (_updateCDVoid.fDesc.Length > 249)
                    {
                        _updateCDVoid.fDesc = _updateCDVoid.fDesc.Substring(0, 248);
                    }

                    _objCD.Status = 2;
                    _updateCDVoid.Status = 2;

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_UpdateCDVoid";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateCDVoid);
                    }
                    else
                    {
                        _objBLBill.UpdateCDVoid(_objCD);
                    }

                    _objTrans.ConnConfig = Session["config"].ToString();
                    _objTrans.ID = Convert.ToInt32(_drCD["TransID"]);
                    _objTrans.BatchID = Convert.ToInt32(_drCD["Batch"]);

                    _getTransByID.ConnConfig = Session["config"].ToString();
                    _getTransByID.ID = Convert.ToInt32(_drCD["TransID"]);
                    _getTransByID.BatchID = Convert.ToInt32(_drCD["Batch"]);

                    _updateTransVoidCheck.ConnConfig = Session["config"].ToString();
                    _updateTransVoidCheck.ID = Convert.ToInt32(_drCD["TransID"]);
                    _updateTransVoidCheckByBatch.BatchID = Convert.ToInt32(_drCD["Batch"]);
                    _updateTransVoidCheckByBatch.ConnConfig = Session["config"].ToString();

                    DataSet _dsTrans = new DataSet();
                    List <TransactionViewModel> _lstTransactionViewModel = new List<TransactionViewModel>();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetTransByID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getTransByID);

                        _lstTransactionViewModel = (new JavaScriptSerializer()).Deserialize<List<TransactionViewModel>>(_APIResponse.ResponseData);
                        _dsTrans = CommonMethods.ToDataSet<TransactionViewModel>(_lstTransactionViewModel);
                        _dsTrans.Tables[0].Columns["BatchID"].ColumnName = "Batch";
                    }
                    else
                    {
                        _dsTrans = _objBLJournal.GetTransByID(_objTrans);
                    }

                    if (_dsTrans.Tables[0].Rows.Count > 0)
                    {
                        _objTrans.Type = 21;
                        _objTrans.TransDescription = "Voided on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " - " + Convert.ToString(_dsTrans.Tables[0].Rows[0]["fDesc"]);

                        _updateTransVoidCheck.Type = 21;
                        _updateTransVoidCheck.TransDescription = "Voided on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " - " + Convert.ToString(_dsTrans.Tables[0].Rows[0]["fDesc"]);

                        _updateTransVoidCheckByBatch.TransDescription = "Voided on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " - " + Convert.ToString(_dsTrans.Tables[0].Rows[0]["fDesc"]);

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/CheckList_UpdateTransVoidCheck";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateTransVoidCheck);
                        }
                        else
                        {
                            _objBLJournal.UpdateTransVoidCheck(_objTrans);
                        }
                        _objTrans.Sel = 2;
                        _objTrans.Type = 20;

                        _updateTransVoidCheckByBatch.Sel = 2;
                        _updateTransVoidCheckByBatch.Type = 20;

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/CheckList_UpdateTransVoidCheckByBatch";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateTransVoidCheckByBatch);
                        }
                        else
                        {
                            _objBLJournal.UpdateTransVoidCheckByBatch(_objTrans);
                        }
                    }

                    AddVoidTransaction(Convert.ToInt32(hdnCDID.Value),(_drCD["Ref"]).ToString(), Convert.ToInt32(_drCD["Bank"]), Convert.ToDateTime(_drCD["fDate"]));
                }

            }
            else
            {
                // // Period Open then
                _objCD.ConnConfig = Session["config"].ToString();
                _objCD.ID = Convert.ToInt32(hdnCDID.Value);


                _getCDByID.ConnConfig = Session["config"].ToString();
                _getCDByID.ID = Convert.ToInt32(hdnCDID.Value);

                _updateCDVoidOpen.ID = Convert.ToInt32(hdnCDID.Value);

                DataSet _dsCD = new DataSet();

                List<GetCDByIDViewModel> _lstGetCDByIDViewModel = new List<GetCDByIDViewModel>();


                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetCDByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCDByID);

                    _lstGetCDByIDViewModel = (new JavaScriptSerializer()).Deserialize<List<GetCDByIDViewModel>>(_APIResponse.ResponseData);
                    _dsCD = CommonMethods.ToDataSet<GetCDByIDViewModel>(_lstGetCDByIDViewModel);
                    _dsCD.Tables[0].Columns["Acct"].ColumnName = "Acct#";
                }
                else
                {
                    _dsCD = _objBLBill.GetCDByID(_objCD);
                }

                if (_dsCD.Tables[0].Rows.Count > 0)
                {
                    DataRow _drCD = _dsCD.Tables[0].Rows[0];
                    _objCD.ConnConfig = Session["config"].ToString();
                    _objCD.fDesc = "Voided by " + Session["username"].ToString() + " on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " - " + Convert.ToString(_drCD["fDesc"]);

                    _updateCDVoidOpen.ConnConfig = Session["config"].ToString();
                    _updateCDVoidOpen.fDesc = "Voided by " + Session["username"].ToString() + " on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " - " + Convert.ToString(_drCD["fDesc"]);

                    if (_objCD.fDesc.Length > 249)
                    {
                        _objCD.fDesc = _objCD.fDesc.Substring(0, 248);
                    }

                    if (_updateCDVoidOpen.fDesc.Length > 249)
                    {
                        _updateCDVoidOpen.fDesc = _updateCDVoidOpen.fDesc.Substring(0, 248);
                    }

                    _objCD.Status = 2;
                    _objCD.Amount = 0;

                    _updateCDVoidOpen.Status = 2;
                    _updateCDVoidOpen.Amount = 0;

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_UpdateCDVoidOpen";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateCDVoidOpen);
                    }
                    else
                    {
                        _objBLBill.UpdateCDVoidOpen(_objCD);
                    }

                    _objCD.ConnConfig = Session["config"].ToString();
                    _objCD.French = "APCheck";
                    //_objCD.ID = Convert.ToInt32(_drCD["Ref"]);
                    _objCD.ID = Convert.ToInt32(hdnCDID.Value);
                    _objCD.Memo = "Status";
                    _objCD.searchterm = "Open";
                    _objCD.searchvalue = "Void";
                    _objCD.MOMUSer = Session["Username"].ToString();


                    _updateAPCDVoidLog.ConnConfig = Session["config"].ToString();
                    _updateAPCDVoidLog.French = "APCheck";
                    //_updateAPCDVoidLog.ID = Convert.ToInt32(_drCD["Ref"]);
                    _updateAPCDVoidLog.ID = Convert.ToInt32(hdnCDID.Value);
                    _updateAPCDVoidLog.Memo = "Status";
                    _updateAPCDVoidLog.searchterm = "Open";
                    _updateAPCDVoidLog.searchvalue = "Void";
                    _updateAPCDVoidLog.MOMUSer = Session["Username"].ToString();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_UpdateAPCDVoidLog";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateAPCDVoidLog);
                    }
                    else
                    {
                        _objBLBill.UpdateAPCDVoidLog(_objCD);
                    }

                    _objCD.ConnConfig = Session["config"].ToString();
                    _objCD.French = "APCheck";
                    //_objCD.ID = Convert.ToInt32(_drCD["Ref"]);
                    _objCD.ID = Convert.ToInt32(hdnCDID.Value);
                    _objCD.Memo = "Amount";
                    _objCD.searchterm = Convert.ToString(_drCD["Amount"]);
                    _objCD.searchvalue = "0";
                    _objCD.MOMUSer = Session["Username"].ToString();


                    _updateAPCDVoidLog.ConnConfig = Session["config"].ToString();
                    _updateAPCDVoidLog.French = "APCheck";
                    //_updateAPCDVoidLog.ID = Convert.ToInt32(_drCD["Ref"]);
                    _updateAPCDVoidLog.ID = Convert.ToInt32(hdnCDID.Value);
                    _updateAPCDVoidLog.Memo = "Amount";
                    _updateAPCDVoidLog.searchterm = Convert.ToString(_drCD["Amount"]);
                    _updateAPCDVoidLog.searchvalue = "0";
                    _updateAPCDVoidLog.MOMUSer = Session["Username"].ToString();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_UpdateAPCDVoidLog";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateAPCDVoidLog);
                    }
                    else
                    {
                        _objBLBill.UpdateAPCDVoidLog(_objCD);
                    }


                    _objTrans.ConnConfig = Session["config"].ToString();
                    _objTrans.ID = Convert.ToInt32(_drCD["TransID"]);
                    _objTrans.BatchID = Convert.ToInt32(_drCD["Batch"]);

                    _getTransByID.ConnConfig = Session["config"].ToString();
                    _getTransByID.ID = Convert.ToInt32(_drCD["TransID"]);
                    _getTransByID.BatchID = Convert.ToInt32(_drCD["Batch"]);

                    _updateTransVoidCheckOpen.ConnConfig = Session["config"].ToString();
                    _updateTransVoidCheckOpen.ID = Convert.ToInt32(_drCD["TransID"]);
                    _updateTransVoidCheckByBatchOpen.ConnConfig = Session["config"].ToString();
                    _updateTransVoidCheckByBatchOpen.BatchID = Convert.ToInt32(_drCD["Batch"]);

                    DataSet _dsTrans = new DataSet();

                    List <TransactionViewModel> _lstTransactionViewModel = new List<TransactionViewModel>();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetTransByID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getTransByID);

                        _lstTransactionViewModel = (new JavaScriptSerializer()).Deserialize<List<TransactionViewModel>>(_APIResponse.ResponseData);
                        _dsTrans = CommonMethods.ToDataSet<TransactionViewModel>(_lstTransactionViewModel);
                        _dsTrans.Tables[0].Columns["BatchID"].ColumnName = "Batch";
                    }
                    else
                    {
                        _dsTrans = _objBLJournal.GetTransByID(_objTrans);
                    }
                    if (_dsTrans.Tables[0].Rows.Count > 0)
                    {
                        _objTrans.Type = 21;
                        _objTrans.Amount = 0;
                        _objTrans.TransDescription = "Voided on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " - " + Convert.ToString(_dsTrans.Tables[0].Rows[0]["fDesc"]);

                        _updateTransVoidCheckOpen.Type = 21;
                        _updateTransVoidCheckOpen.Amount = 0;
                        _updateTransVoidCheckOpen.TransDescription = "Voided on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " - " + Convert.ToString(_dsTrans.Tables[0].Rows[0]["fDesc"]);

                        _updateTransVoidCheckByBatchOpen.TransDescription = "Voided on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " - " + Convert.ToString(_dsTrans.Tables[0].Rows[0]["fDesc"]);

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/CheckList_UpdateTransVoidCheckOpen";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateTransVoidCheckOpen);
                        }
                        else
                        {
                            _objBLJournal.UpdateTransVoidCheckOpen(_objTrans);
                        }

                        _objTrans.Sel = 2;
                        _objTrans.Amount = 0;
                        _objTrans.Type = 20;

                        _updateTransVoidCheckByBatchOpen.Sel = 2;
                        _updateTransVoidCheckByBatchOpen.Amount = 0;
                        _updateTransVoidCheckByBatchOpen.Type = 20;

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/CheckList_UpdateTransVoidCheckByBatchOpen";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateTransVoidCheckByBatchOpen);
                        }
                        else
                        {
                            _objBLJournal.UpdateTransVoidCheckByBatchOpen(_objTrans);
                        }
                    }

                    AddVoidTransaction(Convert.ToInt32(hdnCDID.Value), (_drCD["Ref"]).ToString(), Convert.ToInt32(_drCD["Bank"]), Convert.ToDateTime(_drCD["fDate"]));
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void AddVoidTransaction(int _cdId, string _ref, int _bankId, DateTime _checkDate)
    {
        try
        {
            bool flag = GetPeriodDetails(Convert.ToDateTime(txtVoidDate.Text));
            if (!flag)    // Period Close Out 
            {
                var voidDate = Convert.ToDateTime(txtVoidDate.Text);

                _objPaid.ConnConfig = Session["config"].ToString();
                _objPaid.PITR = _cdId;

                _getPaidDetailByID.ConnConfig = Session["config"].ToString();
                _getPaidDetailByID.PITR = _cdId;

                DataSet _dsPaid = new DataSet();

                List<PaidViewModel> _lstPaidViewModel = new List<PaidViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetPaidDetailByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getPaidDetailByID);

                    _lstPaidViewModel = (new JavaScriptSerializer()).Deserialize<List<PaidViewModel>>(_APIResponse.ResponseData);
                    _dsPaid = CommonMethods.ToDataSet<PaidViewModel>(_lstPaidViewModel);
                    _dsPaid.Tables[0].Columns["Paid1"].ColumnName = "Paid";
                }
                else
                {
                   _dsPaid = _objBLBill.GetPaidDetailByID(_objPaid);
                }

                foreach (DataRow dr in _dsPaid.Tables[0].Rows)
                {

                    double paidpercentage = Convert.ToDouble(dr["TBalance"]) * 100 / Convert.ToDouble(dr["Original"]);

                    //Get AP invoice transaction details.
                    _objTrans.ConnConfig = Session["config"].ToString();
                    _objTrans.BatchID = Convert.ToInt32(dr["Batch"]);

                    _getTransByBatch.ConnConfig = Session["config"].ToString();
                    _getTransByBatch.BatchID = Convert.ToInt32(dr["Batch"]);

                    DataSet _dsJrn = new DataSet();

                    List<TransactionViewModel> _lstTransactionViewModel = new List<TransactionViewModel>();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetTransByBatch";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getTransByBatch);

                        _lstTransactionViewModel = (new JavaScriptSerializer()).Deserialize<List<TransactionViewModel>>(_APIResponse.ResponseData);
                        _dsJrn = CommonMethods.ToDataSet<TransactionViewModel>(_lstTransactionViewModel);
                    }
                    else
                    {
                        _dsJrn = _objBLJournal.GetTransByBatch(_objTrans);
                    }
                    DataRow[] _dr = _dsJrn.Tables[0].Select("Type = 41");

                    _objJournal.ConnConfig = Session["config"].ToString();

                    _getMaxTransBatch.ConnConfig = Session["config"].ToString();

                    int _batch = 0;

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetMaxTransBatch";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getMaxTransBatch);

                        _batch = Convert.ToInt32(_APIResponse.ResponseData);
                    }
                    else
                    {
                        _batch = _objBLJournal.GetMaxTransBatch(_objJournal);
                    }

                    int _refGL = 0;
                    _getMaxTransRef.ConnConfig = Session["config"].ToString();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetMaxTransRef";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getMaxTransRef);

                        _refGL = Convert.ToInt32(_APIResponse.ResponseData);
                    }
                    else
                    {
                        _refGL = _objBLJournal.GetMaxTransRef(_objJournal);
                    }

                    _objJournal.ConnConfig = Session["config"].ToString();
                    _objJournal.Ref = _refGL;
                    _objJournal.BatchID = _batch;
                    _objJournal.GLDate = voidDate;
                    _objJournal.GLDesc = "Voided check #" + _ref;
                    _objJournal.Internal = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString();


                    _addGLA.ConnConfig = Session["config"].ToString();
                    _addGLA.Ref = _refGL;
                    _addGLA.BatchID = _batch;
                    _addGLA.GLDate = voidDate;
                    _addGLA.GLDesc = "Voided check #" + _ref;
                    _addGLA.Internal = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString();


                    // Add into GLA table
                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_AddGLA";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addGLA);
                    }
                    else
                    {
                        _objBLJournal.AddGLA(_objJournal);
                    }
                    // Get Account Payable
                    _objChart.ConnConfig = Session["config"].ToString();
                    _getAcctPayable.ConnConfig = Session["config"].ToString();
                    _getBankAcctID.ConnConfig = Session["config"].ToString();

                    DataSet dsPA = new DataSet();
                    List <ChartViewModel> _lstChartViewModel = new List<ChartViewModel>();
                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetAcctPayable";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAcctPayable);

                        _lstChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse.ResponseData);
                        dsPA = CommonMethods.ToDataSet<ChartViewModel>(_lstChartViewModel);
                    }
                    else
                    {
                        dsPA = _objBLChart.GetAcctPayable(_objChart);
                    }
                    int i = 0;

                    if (_checkDate != voidDate)
                    {
                        foreach (var _row in _dr)
                        {
                            _objTrans = new Transaction();

                            _objTrans.ConnConfig = Session["config"].ToString();
                            _objTrans.BatchID = _batch;
                            _objTrans.Ref = _refGL;
                            _objTrans.TransDate = voidDate;
                            _objTrans.Line = i;
                            _objTrans.TransDescription = Convert.ToString(_row["fDesc"]) + " - Voided check #" + _ref;
                            _objTrans.Acct = Convert.ToInt32(_row["Acct"]);
                            _objTrans.Amount = (Convert.ToDouble(_row["Amount"]) * -1);
                            _objTrans.Sel = 0;
                            _objTrans.Type = 31;

                            _addJournalTrans = new AddJournalTransParam();

                            _addJournalTrans.ConnConfig = Session["config"].ToString();
                            _addJournalTrans.BatchID = _batch;
                            _addJournalTrans.Ref = _refGL;
                            _addJournalTrans.TransDate = voidDate;
                            _addJournalTrans.Line = i;
                            _addJournalTrans.TransDescription = Convert.ToString(_row["fDesc"]) + " - Voided check #" + _ref;
                            _addJournalTrans.Acct = Convert.ToInt32(_row["Acct"]);
                            _addJournalTrans.Amount = (Convert.ToDouble(_row["Amount"]) * -1);
                            _addJournalTrans.Sel = 0;
                            _addJournalTrans.Type = 31;

                            if (!string.IsNullOrEmpty(_row["VInt"].ToString()))
                            {
                                _objTrans.JobInt = Convert.ToInt32(_row["VInt"]);
                                _addJournalTrans.JobInt = Convert.ToInt32(_row["VInt"]);
                            }
                            if (!string.IsNullOrEmpty(_row["VDoub"].ToString()))
                            {
                                _objTrans.PhaseDoub = Convert.ToDouble(_row["VDoub"]);
                                _addJournalTrans.PhaseDoub = Convert.ToDouble(_row["VDoub"]);
                            }

                            //Add Journal Entry
                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_AddJournalTrans";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addJournalTrans);
                            }
                            else
                            {
                                _objBLJournal.AddJournalTrans(_objTrans);
                            }

                            UpdateChartBalance();

                            i++;
                        }

                        // Add Accounts Payable if the check voided for bill paid partially
                        if (Convert.ToDouble(dr["Original"].ToString()) - Convert.ToDouble(dr["Paid"].ToString()) != 0)
                        {
                            if (dsPA != null && dsPA.Tables[0].Rows.Count > 0)
                            {
                                var rowPA = dsPA.Tables[0].Rows[0];
                                _objTrans = new Transaction();
                                _objTrans.ConnConfig = Session["config"].ToString();
                                _objTrans.BatchID = _batch;
                                _objTrans.Ref = _refGL;
                                _objTrans.TransDate = voidDate;
                                _objTrans.Line = i;
                                _objTrans.TransDescription = "Voided check #" + _ref;
                                _objTrans.Acct = Convert.ToInt32(rowPA["ID"]);
                                _objTrans.Sel = 2;
                                _objTrans.Type = 30;
                                _objTrans.AcctSub = _bankId;
                                _objTrans.Amount = Convert.ToDouble(dr["Original"].ToString()) - Convert.ToDouble(dr["Paid"].ToString());

                                _addJournalTrans = new AddJournalTransParam();
                                _addJournalTrans.ConnConfig = Session["config"].ToString();
                                _addJournalTrans.BatchID = _batch;
                                _addJournalTrans.Ref = _refGL;
                                _addJournalTrans.TransDate = voidDate;
                                _addJournalTrans.Line = i;
                                _addJournalTrans.TransDescription = "Voided check #" + _ref;
                                _addJournalTrans.Acct = Convert.ToInt32(rowPA["ID"]);
                                _addJournalTrans.Sel = 2;
                                _addJournalTrans.Type = 30;
                                _addJournalTrans.AcctSub = _bankId;
                                _addJournalTrans.Amount = Convert.ToDouble(dr["Original"].ToString()) - Convert.ToDouble(dr["Paid"].ToString());

                                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                                //if (IsAPIIntegrationEnable == "YES")
                                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                {
                                    string APINAME = "ManageChecksAPI/CheckList_AddJournalTrans";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addJournalTrans);
                                }
                                else
                                {
                                    _objBLJournal.AddJournalTrans(_objTrans);
                                }
                                UpdateChartBalance();

                                i++;
                            }
                        }
                    }
                    else
                    {
                        if (dsPA != null && dsPA.Tables[0].Rows.Count > 0)
                        {
                            var rowPA = dsPA.Tables[0].Rows[0];
                            _objTrans = new Transaction();
                            _objTrans.ConnConfig = Session["config"].ToString();
                            _objTrans.BatchID = _batch;
                            _objTrans.Ref = _refGL;
                            _objTrans.TransDate = voidDate;
                            _objTrans.Line = i;
                            _objTrans.TransDescription = "Voided check #" + _ref;
                            _objTrans.Acct = Convert.ToInt32(rowPA["ID"]);
                            _objTrans.Sel = 2;
                            _objTrans.Type = 30;
                            _objTrans.AcctSub = _bankId;
                            _objTrans.Amount = -Convert.ToDouble(dr["Paid"].ToString());


                            _addJournalTrans = new AddJournalTransParam();
                            _addJournalTrans.ConnConfig = Session["config"].ToString();
                            _addJournalTrans.BatchID = _batch;
                            _addJournalTrans.Ref = _refGL;
                            _addJournalTrans.TransDate = voidDate;
                            _addJournalTrans.Line = i;
                            _addJournalTrans.TransDescription = "Voided check #" + _ref;
                            _addJournalTrans.Acct = Convert.ToInt32(rowPA["ID"]);
                            _addJournalTrans.Sel = 2;
                            _addJournalTrans.Type = 30;
                            _addJournalTrans.AcctSub = _bankId;
                            _addJournalTrans.Amount = -Convert.ToDouble(dr["Paid"].ToString());
                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_AddJournalTrans";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addJournalTrans);
                            }
                            else
                            {
                                _objBLJournal.AddJournalTrans(_objTrans);
                            }

                            UpdateChartBalance();

                            i++;
                        }
                    }

                    _objChart.Bank = _bankId;
                    _getBankAcctID.Bank = _bankId;
                    int BankGL;
                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetBankAcctID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankAcctID);
                        BankGL =Convert.ToInt32(_APIResponse.ResponseData);
                    }
                    else
                    {
                        BankGL = _objBLChart.GetBankAcctID(_objChart);
                    }

                    _objTrans = new Transaction();
                    _objTrans.ConnConfig = Session["config"].ToString();
                    _objTrans.BatchID = _batch;
                    _objTrans.Ref = _refGL;
                    _objTrans.TransDate = voidDate;
                    _objTrans.Line = i;
                    _objTrans.TransDescription = "Voided check #" + _ref;
                    _objTrans.Acct = BankGL;
                    _objTrans.Sel = 2;
                    _objTrans.Type = 30;
                    _objTrans.AcctSub = _bankId;

                    _addJournalTrans = new AddJournalTransParam();
                    _addJournalTrans.ConnConfig = Session["config"].ToString();
                    _addJournalTrans.BatchID = _batch;
                    _addJournalTrans.Ref = _refGL;
                    _addJournalTrans.TransDate = voidDate;
                    _addJournalTrans.Line = i;
                    _addJournalTrans.TransDescription = "Voided check #" + _ref;
                    _addJournalTrans.Acct = BankGL;
                    _addJournalTrans.Sel = 2;
                    _addJournalTrans.Type = 30;
                    _addJournalTrans.AcctSub = _bankId;

                    //Add Journal Entry
                    if (!string.IsNullOrEmpty(dr["Paid"].ToString()))
                    {
                        _objTrans.Amount = Convert.ToDouble(dr["Paid"].ToString());
                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/CheckList_AddJournalTrans";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addJournalTrans);
                        }
                        else
                        {
                            _objBLJournal.AddJournalTrans(_objTrans);
                        }

                        UpdateChartBalance();
                    }

                    UpdateAPInvoice(Convert.ToInt32(dr["Batch"]),_ref.ToString(), paidpercentage);

                }
            }
            else
            {
                // Period Open
                var voidDate = Convert.ToDateTime(txtVoidDate.Text);

                _objPaid.ConnConfig = Session["config"].ToString();
                _objPaid.PITR = _cdId;

                _getPaidDetailByID.ConnConfig = Session["config"].ToString();
                _getPaidDetailByID.PITR = _cdId;

                DataSet _dsPaid = new DataSet();

                List<PaidViewModel> _lstPaidViewModel = new List<PaidViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetPaidDetailByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getPaidDetailByID);

                    _lstPaidViewModel = (new JavaScriptSerializer()).Deserialize<List<PaidViewModel>>(_APIResponse.ResponseData);
                    _dsPaid = CommonMethods.ToDataSet<PaidViewModel>(_lstPaidViewModel);
                    _dsPaid.Tables[0].Columns["Paid1"].ColumnName = "Paid";
                }
                else
                {
                    _dsPaid = _objBLBill.GetPaidDetailByID(_objPaid);
                }
                foreach (DataRow dr in _dsPaid.Tables[0].Rows)
                {

                    double paidpercentage = Convert.ToDouble(dr["TBalance"]) * 100 / Convert.ToDouble(dr["Original"]);

                    //Get AP invoice transaction details.
                    _objTrans.ConnConfig = Session["config"].ToString();
                    _objTrans.BatchID = Convert.ToInt32(dr["Batch"]);

                    _getTransByBatch.ConnConfig = Session["config"].ToString();
                    _getTransByBatch.BatchID = Convert.ToInt32(dr["Batch"]);

                    DataSet _dsJrn = new DataSet();

                    List<TransactionViewModel> _lstTransactionViewModel = new List<TransactionViewModel>();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetTransByBatch";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getTransByBatch);

                        _lstTransactionViewModel = (new JavaScriptSerializer()).Deserialize<List<TransactionViewModel>>(_APIResponse.ResponseData);
                        _dsJrn = CommonMethods.ToDataSet<TransactionViewModel>(_lstTransactionViewModel);
                    }
                    else
                    {
                        _dsJrn = _objBLJournal.GetTransByBatch(_objTrans);
                    }
                    DataRow[] _dr = _dsJrn.Tables[0].Select("Type = 41");

                    _objJournal.ConnConfig = Session["config"].ToString();
                    _getMaxTransBatch.ConnConfig = Session["config"].ToString();

                    int _batch = 0;
                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetMaxTransBatch";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getMaxTransBatch);

                        _batch = Convert.ToInt32(_APIResponse.ResponseData);
                    }
                    else
                    {
                        _batch = _objBLJournal.GetMaxTransBatch(_objJournal);

                    }

                    int _refGL = 0;
                    _getMaxTransRef.ConnConfig = Session["config"].ToString();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetMaxTransRef";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getMaxTransRef);

                        _refGL = Convert.ToInt32(_APIResponse.ResponseData);
                    }
                    else
                    {
                        _refGL = _objBLJournal.GetMaxTransRef(_objJournal);
                    }

                    //_objJournal.ConnConfig = Session["config"].ToString();
                    //_objJournal.Ref = _refGL;
                    //_objJournal.BatchID = _batch;
                    //_objJournal.GLDate = voidDate;
                    //_objJournal.GLDesc = "Voided check #" + _ref;
                    //_objJournal.Internal = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString();

                    //// Add into GLA table
                    //_objBLJournal.AddGLA(_objJournal);

                    // Get Account Payable
                    _objChart.ConnConfig = Session["config"].ToString();
                    _getAcctPayable.ConnConfig = Session["config"].ToString();

                    DataSet dsPA = new DataSet();
                    List<ChartViewModel> _lstChartViewModel = new List<ChartViewModel>();
                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetAcctPayable";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAcctPayable);

                        _lstChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse.ResponseData);
                        dsPA = CommonMethods.ToDataSet<ChartViewModel>(_lstChartViewModel);
                    }
                    else
                    {
                        dsPA = _objBLChart.GetAcctPayable(_objChart);
                    }

                    int i = 0;

                    //if (_checkDate != voidDate)
                    //{
                    //    foreach (var _row in _dr)
                    //    {
                    //        _objTrans = new Transaction();

                    //        _objTrans.ConnConfig = Session["config"].ToString();
                    //        _objTrans.BatchID = _batch;
                    //        _objTrans.Ref = _refGL;
                    //        _objTrans.TransDate = voidDate;
                    //        _objTrans.Line = i;
                    //        _objTrans.TransDescription = Convert.ToString(_row["fDesc"]) + " - Voided check #" + _ref;
                    //        _objTrans.Acct = Convert.ToInt32(_row["Acct"]);
                    //        _objTrans.Amount = (Convert.ToDouble(_row["Amount"]) * -1);
                    //        _objTrans.Sel = 0;
                    //        _objTrans.Type = 31;
                    //        if (!string.IsNullOrEmpty(_row["VInt"].ToString()))
                    //        {
                    //            _objTrans.JobInt = Convert.ToInt32(_row["VInt"]);
                    //        }
                    //        if (!string.IsNullOrEmpty(_row["VDoub"].ToString()))
                    //        {
                    //            _objTrans.PhaseDoub = Convert.ToDouble(_row["VDoub"]);
                    //        }

                    //        //Add Journal Entry
                    //        _objBLJournal.AddJournalTrans(_objTrans);

                    //        UpdateChartBalance();

                    //        i++;
                    //    }

                    //    // Add Accounts Payable if the check voided for bill paid partially
                    //    if (Convert.ToDouble(dr["Original"].ToString()) - Convert.ToDouble(dr["Paid"].ToString()) != 0)
                    //    {
                    //        if (dsPA != null && dsPA.Tables[0].Rows.Count > 0)
                    //        {
                    //            var rowPA = dsPA.Tables[0].Rows[0];
                    //            _objTrans = new Transaction();
                    //            _objTrans.ConnConfig = Session["config"].ToString();
                    //            _objTrans.BatchID = _batch;
                    //            _objTrans.Ref = _refGL;
                    //            _objTrans.TransDate = voidDate;
                    //            _objTrans.Line = i;
                    //            _objTrans.TransDescription = "Voided check #" + _ref;
                    //            _objTrans.Acct = Convert.ToInt32(rowPA["ID"]);
                    //            _objTrans.Sel = 2;
                    //            _objTrans.Type = 30;
                    //            _objTrans.AcctSub = _bankId;
                    //            _objTrans.Amount = Convert.ToDouble(dr["Original"].ToString()) - Convert.ToDouble(dr["Paid"].ToString());
                    //            _objBLJournal.AddJournalTrans(_objTrans);

                    //            UpdateChartBalance();

                    //            i++;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (dsPA != null && dsPA.Tables[0].Rows.Count > 0)
                    //    {
                    //        var rowPA = dsPA.Tables[0].Rows[0];
                    //        _objTrans = new Transaction();
                    //        _objTrans.ConnConfig = Session["config"].ToString();
                    //        _objTrans.BatchID = _batch;
                    //        _objTrans.Ref = _refGL;
                    //        _objTrans.TransDate = voidDate;
                    //        _objTrans.Line = i;
                    //        _objTrans.TransDescription = "Voided check #" + _ref;
                    //        _objTrans.Acct = Convert.ToInt32(rowPA["ID"]);
                    //        _objTrans.Sel = 2;
                    //        _objTrans.Type = 30;
                    //        _objTrans.AcctSub = _bankId;
                    //        _objTrans.Amount = -Convert.ToDouble(dr["Paid"].ToString());
                    //        _objBLJournal.AddJournalTrans(_objTrans);

                    //        UpdateChartBalance();

                    //        i++;
                    //    }
                    //}

                    //_objChart.Bank = _bankId;
                    //int BankGL = _objBLChart.GetBankAcctID(_objChart);

                    //_objTrans = new Transaction();
                    //_objTrans.ConnConfig = Session["config"].ToString();
                    //_objTrans.BatchID = _batch;
                    //_objTrans.Ref = _refGL;
                    //_objTrans.TransDate = voidDate;
                    //_objTrans.Line = i;
                    //_objTrans.TransDescription = "Voided check #" + _ref;
                    //_objTrans.Acct = BankGL;
                    //_objTrans.Sel = 2;
                    //_objTrans.Type = 30;
                    //_objTrans.AcctSub = _bankId;

                    ////Add Journal Entry
                    //if (!string.IsNullOrEmpty(dr["Paid"].ToString()))
                    //{
                    //    _objTrans.Amount = Convert.ToDouble(dr["Paid"].ToString());
                    //    _objBLJournal.AddJournalTrans(_objTrans);
                    //    UpdateChartBalance();
                    //}

                    UpdateAPBillStatus(Convert.ToInt32(dr["Batch"]), _ref.ToString(), paidpercentage, Convert.ToDouble(dr["Paid"]), _cdId);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private bool GetPeriodDetails(DateTime invoicedt)
    {
        bool flag = CommonHelper.GetPeriodDetails(invoicedt);
        //if (flag)
        //{
        //    flag = CommonHelper.GetPeriodDetails(postdt);
        //}
        //ViewState["FlagPeriodClose"] = flag;
        return flag;

    }
    private void UpdateAPBillStatus(int _batch, string _ref, double _paidpercentage, double _paidant, int _cdId)
    {
        try
        {
            string fDesc = string.Empty;
            _objTrans.ConnConfig = Session["config"].ToString();
            _objTrans.BatchID = _batch;

            _getTransByBatch.ConnConfig = Session["config"].ToString();
            _getTransByBatch.BatchID = _batch;

            DataSet _dsAPTrans = new DataSet();

            List<TransactionViewModel> _lstTransactionViewModel = new List<TransactionViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetTransByBatch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getTransByBatch);

                _lstTransactionViewModel = (new JavaScriptSerializer()).Deserialize<List<TransactionViewModel>>(_APIResponse.ResponseData);
                _dsAPTrans = CommonMethods.ToDataSet<TransactionViewModel>(_lstTransactionViewModel);
                _dsAPTrans.Tables[0].Columns["BatchID"].ColumnName = "Batch";
            }
            else
            {
                _dsAPTrans = _objBLJournal.GetTransByBatch(_objTrans);
            }

            int _batchID = 0;
            _getMaxTransBatch.ConnConfig = Session["config"].ToString();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetMaxTransBatch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getMaxTransBatch);

                _batchID = Convert.ToInt32(_APIResponse.ResponseData);
            }
            else
            {
                _batchID = _objBLJournal.GetMaxTransBatch(_objJournal);
            }
            int i = 0;
            int _transID = 0;
            int trans = 0;
            int _pjID = 0, _PjTrID = 0, _vendorID = 0;
            string _fpjdesc = "";
            if (_dsAPTrans.Tables[0].Rows.Count > 0)
            {
                #region Add AP Invoice transaction
                //foreach (DataRow _dr in _dsAPTrans.Tables[0].Rows)
                //{
                //    _objTrans = new Transaction();

                //    _objTrans.ConnConfig = Session["config"].ToString();
                //    _objTrans.BatchID = _batchID;
                //    _objTrans.Ref = 0;
                //    _objTrans.TransDate = Convert.ToDateTime(txtVoidDate.Text);
                //    _objTrans.Line = i;
                //    if (Convert.ToInt32(_dr["Type"]).Equals(41))
                //    {
                //        _objTrans.TransDescription = Convert.ToString(_dr["fDesc"]) + " Voided check #" + _ref;
                //    }
                //    else
                //    {
                //        _objTrans.TransDescription = Convert.ToString(_dr["fDesc"]);
                //    }
                //    _objTrans.Acct = Convert.ToInt32(_dr["Acct"]);
                //    //_objTrans.Amount = Convert.ToDouble(_dr["Amount"]);
                //    _objTrans.Amount = Convert.ToDouble(_dr["Amount"]) * _paidpercentage / 100;
                //    _objTrans.Sel = 0;
                //    _objTrans.Type = Convert.ToInt32(_dr["Type"]);
                //    if (Convert.ToInt32(_dr["Type"]).Equals(40))
                //    {
                //        if (!string.IsNullOrEmpty(_dr["AcctSub"].ToString()))
                //        {
                //            _objTrans.AcctSub = Convert.ToInt32(_dr["AcctSub"]);
                //            _objTrans.strRef = Convert.ToString(_dr["strRef"]);
                //        }
                //    }
                //    _transID = _objBLJournal.AddJournalTrans(_objTrans);   //Add Journal Entry

                //    //If Job Cost Expense has been added then job cost details into JobI table.
                //    #region Add JobI
                //    if (!string.IsNullOrEmpty(_dr["VInt"].ToString()))
                //    {
                //        if (Convert.ToInt32(_dr["VInt"]) > 0)
                //        {
                //            _objJobI.ConnConfig = Session["config"].ToString();
                //            _objJobI.TransID = Convert.ToInt32(_dr["ID"]);
                //            DataSet _dsJobI = _objBLBill.GetJobIByTransID(_objJobI);

                //            if (_dsJobI.Tables[0].Rows.Count > 0)
                //            {
                //                _objJobI.ConnConfig = Session["config"].ToString();
                //                _objJobI.Job = _objTrans.JobInt;
                //                _objJobI.Phase = Convert.ToInt16(_objTrans.PhaseDoub);
                //                _objJobI.fDate = Convert.ToDateTime(txtVoidDate.Text);
                //                _objJobI.Ref = Convert.ToString(_dr["strRef"]);
                //                _objJobI.fDesc = _objTrans.TransDescription + " Voided check #" + _ref.ToString();
                //                _objJobI.Amount = _objTrans.Amount;
                //                _objJobI.TransID = _transID;
                //                _objJobI.Type = 1;
                //                //_objJobI.UseTax = _lst.IsUseTax ? 1 : 0;
                //                _objBLBill.AddJobI(_objJobI);
                //            }
                //        }
                //    }

                //    #endregion

                //    UpdateChartBalance();
                //    if (Convert.ToInt32(_dr["Type"]).Equals(40))
                //    {
                //        trans = _transID;
                //    }

                //    i++;
                //}
                #endregion

                _objPJ.ConnConfig = Session["config"].ToString();
                _objPJ.Batch = _batch;

                _getPJDetailByBatch.ConnConfig = Session["config"].ToString();
                _getPJDetailByBatch.Batch = _batch;

                DataSet _dsPJ = new DataSet();

               List <GetPJDetailByBatchViewModel> _lstGetPJDetailByBatchViewModel = new List<GetPJDetailByBatchViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetPJDetailByBatch";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getPJDetailByBatch);

                    _lstGetPJDetailByBatchViewModel = (new JavaScriptSerializer()).Deserialize<List<GetPJDetailByBatchViewModel>>(_APIResponse.ResponseData);
                    _dsPJ = CommonMethods.ToDataSet<GetPJDetailByBatchViewModel>(_lstGetPJDetailByBatchViewModel);
                }
                else
                {
                    _dsPJ = _objBLBill.GetPJDetailByBatch(_objPJ);
                }

                if (_dsPJ.Tables[0].Rows.Count > 0)
                {
                    #region Add PJ Details
                    DataRow _drPJ = _dsPJ.Tables[0].Rows[0];

                    _pjID = Convert.ToInt32(_drPJ["ID"]);
                    _fpjdesc = Convert.ToString(_drPJ["fDesc"]);
                    _PjTrID = Convert.ToInt32(_drPJ["TRID"]);

                    //_objPJ.ConnConfig = Session["config"].ToString();           //Copy PJ details previous details and add
                    //_objPJ.fDate = Convert.ToDateTime(txtVoidDate.Text);
                    //_objPJ.Ref = _drPJ["Ref"].ToString();
                    //fDesc = Convert.ToString(_drPJ["fDesc"]) + " created from voided check #" + _ref;
                    //_objPJ.fDesc = fDesc;
                    ////_objPJ.Amount = Convert.ToDouble(_drPJ["Amount"]);
                    //_objPJ.Amount = Convert.ToDouble(_drPJ["Amount"]) * _paidpercentage / 100;
                    //_objPJ.Vendor = Convert.ToInt32(_drPJ["Vendor"]);
                    //_objPJ.Status = 0;
                    //_objPJ.Batch = _batchID;
                    //_objPJ.Terms = Convert.ToInt16(_drPJ["Terms"]);
                    //if (!string.IsNullOrEmpty(_drPJ["PO"].ToString()))
                    //{
                    //    _objPJ.PO = Convert.ToInt32(_drPJ["PO"]);
                    //}
                    //if (!string.IsNullOrEmpty(_drPJ["ReceivePO"].ToString()))
                    //{
                    //    _objPJ.ReceivePo = Convert.ToInt32(_drPJ["ReceivePO"]);
                    //}
                    //_objPJ.TRID = trans;
                    //_objPJ.Spec = 0;  // Default to Zero During AP Invoice
                    //_objPJ.IDate = Convert.ToDateTime(_drPJ["IDate"]);
                    //_objPJ.Disc = !string.IsNullOrEmpty(Convert.ToString(_drPJ["Disc"])) ? Convert.ToDouble(_drPJ["Disc"]) : 0;
                    //_objPJ.Custom1 = Convert.ToString(_drPJ["Custom1"]);
                    //_objPJ.Custom2 = Convert.ToString(_drPJ["Custom2"]);
                    //_newPJID = _objBLBill.AddPJ(_objPJ);
                    //_vendorID = Convert.ToInt32(_drPJ["Vendor"]);
                    ////_amount = Convert.ToDouble(_drPJ["Amount"]);
                    //_amount = Convert.ToDouble(_drPJ["Amount"]) * _paidpercentage / 100;
                    #endregion

                    #region Update PJ

                    //_objPJ = new PJ();
                    //_objPJ.ConnConfig = Session["config"].ToString();
                    //_objPJ.ID = _pjID;
                    //_objPJ.fDesc = Convert.ToString(_drPJ["fDesc"]);
                    //_objPJ.Status = _pjstatus;
                    //_objBLBill.UpdatePJOnVoidCheck(_objPJ);

                    #endregion

                }

                #region Add OpenAP details
                short _pjstatus = 1;
                _objOpenAP.PJID = _pjID;
                _objOpenAP.ConnConfig = Session["config"].ToString();

                _getOpenAPByPJID.PJID = _pjID;
                _getOpenAPByPJID.ConnConfig = Session["config"].ToString();

                DataSet _dsOpen = new DataSet();
                List <OpenAP> _lstOpenAP = new List<OpenAP>();

                List<OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();
                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetOpenAPByPJID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getOpenAPByPJID);

                    _lstOpenAPViewModel = (new JavaScriptSerializer()).Deserialize<List<OpenAPViewModel>>(_APIResponse.ResponseData);
                    _dsOpen = CommonMethods.ToDataSet<OpenAPViewModel>(_lstOpenAPViewModel);
                }
                else
                {
                    _dsOpen = _objBLBill.GetOpenAPByPJID(_objOpenAP);
                }

                if (_dsOpen.Tables[0].Rows.Count > 0)
                {
                    DataRow _drOpen = _dsOpen.Tables[0].Rows[0];

                    _objOpenAP.ConnConfig = Session["config"].ToString();
                    _objOpenAP.PJID = _pjID;

                    _objOpenAP.Balance = Convert.ToDouble(_drOpen["Balance"]) + _paidant;
                    _objOpenAP.Selected = Convert.ToDouble(_drOpen["Selected"]) - _paidant;

                    _updateOpenAPBalance.ConnConfig = Session["config"].ToString();
                    _updateOpenAPBalance.PJID = _pjID;
                    _updateOpenAPBalance.Balance = Convert.ToDouble(_drOpen["Balance"]) + _paidant;
                    _updateOpenAPBalance.Selected = Convert.ToDouble(_drOpen["Selected"]) - _paidant;

                    if (Convert.ToDouble(_drOpen["Selected"]) - _paidant == 0)
                    {
                        _pjstatus = 0;
                    }
                    else if ((Convert.ToDouble(_drOpen["Selected"]) - _paidant != 0) && (Convert.ToDouble(_drOpen["Original"]) > (Convert.ToDouble(_drOpen["Selected"])- _paidant)))
                    {
                        _pjstatus = 3;
                    }
                    _objOpenAP.IsSelected = 0;
                    _updateOpenAPBalance.IsSelected = 0;

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_UpdateOpenAPBalance";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateOpenAPBalance);
                    }
                    else
                    {
                        _objBLBill.UpdateOpenAPBalance(_objOpenAP);
                    }

                    _objPJ = new PJ();
                    _objPJ.ConnConfig = Session["config"].ToString();
                    _objPJ.ID = _pjID;
                    _objPJ.fDesc = _fpjdesc;
                    _objPJ.Status = _pjstatus;

                    _updatePJOnVoidCheck = new UpdatePJOnVoidCheckParam();
                    _updatePJOnVoidCheck.ConnConfig = Session["config"].ToString();
                    _updatePJOnVoidCheck.ID = _pjID;
                    _updatePJOnVoidCheck.fDesc = _fpjdesc;
                    _updatePJOnVoidCheck.Status = _pjstatus;

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_UpdatePJOnVoidCheck";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updatePJOnVoidCheck);
                    }
                    else
                    {
                        _objBLBill.UpdatePJOnVoidCheck(_objPJ);
                        
                    }

                    // Update Paid Table , CD Table And Also Trans Table Amount Should be 0 For Void Check And Also Update CD Table Log
                    _objPaid = new Paid();
                    _objPaid.ConnConfig = Session["config"].ToString();
                    _objPaid.PITR = _cdId;
                    _objPaid.TRID = _PjTrID;

                    _updatePaidOnVoidCheck = new UpdatePaidOnVoidCheckParam();
                    _updatePaidOnVoidCheck.ConnConfig = Session["config"].ToString();
                    _updatePaidOnVoidCheck.PITR = _cdId;
                    _updatePaidOnVoidCheck.TRID = _PjTrID;

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_UpdatePaidOnVoidCheck";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updatePaidOnVoidCheck);
                    }
                    else
                    {
                        _objBLBill.UpdatePaidOnVoidCheck(_objPaid);
                    }



                }



                #endregion



                #region Update Vendor balance

                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = _vendorID;

                _updateVendorBalance.ConnConfig = Session["config"].ToString();
                _updateVendorBalance.ID = _vendorID;

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_UpdateVendorBalance";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateVendorBalance);
                }
                else
                {
                    _objBLVendor.UpdateVendorBalance(_objVendor);
                }
                #endregion

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void UpdateAPInvoice(int _batch, string _ref, double _paidpercentage)
    {
        try
        {
            
            string fDesc = string.Empty;
            _objTrans.ConnConfig = Session["config"].ToString();
            _objTrans.BatchID = _batch;

            _getTransByBatch.ConnConfig = Session["config"].ToString();
            _getTransByBatch.BatchID = _batch;

            DataSet _dsAPTrans = new DataSet();

            List<TransactionViewModel> _lstTransactionViewModel = new List<TransactionViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetTransByBatch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getTransByBatch);

                _lstTransactionViewModel = (new JavaScriptSerializer()).Deserialize<List<TransactionViewModel>>(_APIResponse.ResponseData);
                _dsAPTrans = CommonMethods.ToDataSet<TransactionViewModel>(_lstTransactionViewModel);
            }
            else
            {
                _dsAPTrans = _objBLJournal.GetTransByBatch(_objTrans);
            }

            int _batchID = 0;
            _getMaxTransBatch.ConnConfig = Session["config"].ToString();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetMaxTransBatch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getMaxTransBatch);

                _batchID = Convert.ToInt32(_APIResponse.ResponseData);
            }
            else
            {
                _batchID = _objBLJournal.GetMaxTransBatch(_objJournal);
            }

          
            int i = 0;
            int _transID = 0;
            int trans = 0;
            int _pjID = 0, _vendorID = 0;
            double _amount = 0.00;
            if (_dsAPTrans.Tables[0].Rows.Count > 0)
            {
                #region Add AP Invoice transaction
                foreach (DataRow _dr in _dsAPTrans.Tables[0].Rows)
                {
                    _objTrans = new Transaction();

                    _objTrans.ConnConfig = Session["config"].ToString();
                    _objTrans.BatchID = _batchID;
                    _objTrans.Ref = 0;
                    _objTrans.TransDate = Convert.ToDateTime(txtVoidDate.Text);
                    _objTrans.Line = i;

                    _addJournalTrans = new AddJournalTransParam();

                    _addJournalTrans.ConnConfig = Session["config"].ToString();
                    _addJournalTrans.BatchID = _batchID;
                    _addJournalTrans.Ref = 0;
                    _addJournalTrans.TransDate = Convert.ToDateTime(txtVoidDate.Text);
                    _addJournalTrans.Line = i;

                    if (Convert.ToInt32(_dr["Type"]).Equals(41))
                    {
                        _objTrans.TransDescription = Convert.ToString(_dr["fDesc"]) + " Voided check #" + _ref;
                        _addJournalTrans.TransDescription = Convert.ToString(_dr["fDesc"]) + " Voided check #" + _ref;
                    }
                    else
                    {
                        _objTrans.TransDescription = Convert.ToString(_dr["fDesc"]);
                        _addJournalTrans.TransDescription = Convert.ToString(_dr["fDesc"]);
                    }
                    _objTrans.Acct = Convert.ToInt32(_dr["Acct"]);
                    //_objTrans.Amount = Convert.ToDouble(_dr["Amount"]);
                    _objTrans.Amount = Convert.ToDouble(_dr["Amount"]) * _paidpercentage / 100;
                    _objTrans.Sel = 0;
                    _objTrans.Type = Convert.ToInt32(_dr["Type"]);

                    _addJournalTrans.Acct = Convert.ToInt32(_dr["Acct"]);
                    //_objTrans.Amount = Convert.ToDouble(_dr["Amount"]);
                    _addJournalTrans.Amount = Convert.ToDouble(_dr["Amount"]) * _paidpercentage / 100;
                    _addJournalTrans.Sel = 0;
                    _addJournalTrans.Type = Convert.ToInt32(_dr["Type"]);

                    if (Convert.ToInt32(_dr["Type"]).Equals(40))
                    {
                        if (!string.IsNullOrEmpty(_dr["AcctSub"].ToString()))
                        {
                            _objTrans.AcctSub = Convert.ToInt32(_dr["AcctSub"]);
                            _objTrans.strRef = Convert.ToString(_dr["strRef"]);

                            _addJournalTrans.AcctSub = Convert.ToInt32(_dr["AcctSub"]);
                            _addJournalTrans.strRef = Convert.ToString(_dr["strRef"]);
                        }
                    }

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_AddJournalTrans";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addJournalTrans);

                        _transID = Convert.ToInt32(_APIResponse.ResponseData);
                    }
                    else
                    {
                        _transID = _objBLJournal.AddJournalTrans(_objTrans);   //Add Journal Entry
                    }
                    //If Job Cost Expense has been added then job cost details into JobI table.
                    #region Add JobI
                    if (!string.IsNullOrEmpty(_dr["VInt"].ToString()))
                    {
                        if (Convert.ToInt32(_dr["VInt"]) > 0)
                        {
                            _objJobI.ConnConfig = Session["config"].ToString();
                            _objJobI.TransID = Convert.ToInt32(_dr["ID"]);

                            _getJobIByTransID.ConnConfig = Session["config"].ToString();
                            _getJobIByTransID.TransID = Convert.ToInt32(_dr["ID"]);

                            DataSet _dsJobI = new DataSet();

                            List <JobIViewModel> _lstJobIViewModel = new List<JobIViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_GetJobIByTransID";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getJobIByTransID);

                                _lstJobIViewModel = (new JavaScriptSerializer()).Deserialize<List<JobIViewModel>>(_APIResponse.ResponseData);
                                _dsJobI = CommonMethods.ToDataSet<JobIViewModel>(_lstJobIViewModel);
                            }
                            else
                            {
                                _dsJobI = _objBLBill.GetJobIByTransID(_objJobI);
                            }

                            if (_dsJobI.Tables[0].Rows.Count > 0)
                            {
                                _objJobI.ConnConfig = Session["config"].ToString();
                                _objJobI.Job = _objTrans.JobInt;
                                _objJobI.Phase = Convert.ToInt16(_objTrans.PhaseDoub);
                                _objJobI.fDate = Convert.ToDateTime(txtVoidDate.Text);
                                _objJobI.Ref = Convert.ToString(_dr["strRef"]);
                                _objJobI.fDesc = _objTrans.TransDescription + " Voided check #" + _ref.ToString();
                                _objJobI.Amount = _objTrans.Amount;
                                _objJobI.TransID = _transID;
                                _objJobI.Type = 1;

                                _addJobI.ConnConfig = Session["config"].ToString();
                                _addJobI.Job = _objTrans.JobInt;
                                _addJobI.Phase = Convert.ToInt16(_objTrans.PhaseDoub);
                                _addJobI.fDate = Convert.ToDateTime(txtVoidDate.Text);
                                _addJobI.Ref = Convert.ToString(_dr["strRef"]);
                                _addJobI.fDesc = _objTrans.TransDescription + " Voided check #" + _ref.ToString();
                                _addJobI.Amount = _objTrans.Amount;
                                _addJobI.TransID = _transID;
                                _addJobI.Type = 1;

                                //_objJobI.UseTax = _lst.IsUseTax ? 1 : 0;

                                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                                //if (IsAPIIntegrationEnable == "YES")
                                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                {
                                    string APINAME = "ManageChecksAPI/CheckList_AddJobI";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addJobI);
                                }
                                else
                                {
                                    _objBLBill.AddJobI(_objJobI);
                                }
                            }
                        }
                    }

                    #endregion

                    UpdateChartBalance();
                    if (Convert.ToInt32(_dr["Type"]).Equals(40))
                    {
                        trans = _transID;
                    }

                    i++;
                }
                #endregion

                _objPJ.ConnConfig = Session["config"].ToString();
                _objPJ.Batch = _batch;

                _getPJDetailByBatch.ConnConfig = Session["config"].ToString();
                _getPJDetailByBatch.Batch = _batch;

                int _newPJID = 0;
                DataSet _dsPJ = new DataSet();

                List <GetPJDetailByBatchViewModel> _lstGetPJDetailByBatchViewModel = new List<GetPJDetailByBatchViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetPJDetailByBatch";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getPJDetailByBatch);

                    _lstGetPJDetailByBatchViewModel = (new JavaScriptSerializer()).Deserialize<List<GetPJDetailByBatchViewModel>>(_APIResponse.ResponseData);
                    _dsPJ = CommonMethods.ToDataSet<GetPJDetailByBatchViewModel>(_lstGetPJDetailByBatchViewModel);
                }
                else
                {
                    _dsPJ = _objBLBill.GetPJDetailByBatch(_objPJ);
                }
                if (_dsPJ.Tables[0].Rows.Count > 0)
                {
                    #region Add PJ Details
                    DataRow _drPJ = _dsPJ.Tables[0].Rows[0];

                    _pjID = Convert.ToInt32(_drPJ["ID"]);
                    _objPJ.ConnConfig = Session["config"].ToString();           //Copy PJ details previous details and add
                    _objPJ.fDate = Convert.ToDateTime(txtVoidDate.Text);
                    _objPJ.Ref = _drPJ["Ref"].ToString();
                    fDesc = Convert.ToString(_drPJ["fDesc"]) + " created from voided check #" + _ref;
                    _objPJ.fDesc = fDesc;
                    //_objPJ.Amount = Convert.ToDouble(_drPJ["Amount"]);
                    _objPJ.Amount = Convert.ToDouble(_drPJ["Amount"]) * _paidpercentage / 100;
                    _objPJ.Vendor = Convert.ToInt32(_drPJ["Vendor"]);
                    _objPJ.Status = 0;
                    _objPJ.Batch = _batchID;
                    _objPJ.Terms = Convert.ToInt16(_drPJ["Terms"]);

                    _addPJ.ConnConfig = Session["config"].ToString();           //Copy PJ details previous details and add
                    _addPJ.fDate = Convert.ToDateTime(txtVoidDate.Text);
                    _addPJ.Ref = _drPJ["Ref"].ToString();
                    _addPJ.fDesc = fDesc;
                    //_objPJ.Amount = Convert.ToDouble(_drPJ["Amount"]);
                    _addPJ.Amount = Convert.ToDouble(_drPJ["Amount"]) * _paidpercentage / 100;
                    _addPJ.Vendor = Convert.ToInt32(_drPJ["Vendor"]);
                    _addPJ.Status = 0;
                    _addPJ.Batch = _batchID;
                    _addPJ.Terms = Convert.ToInt16(_drPJ["Terms"]);

                    if (!string.IsNullOrEmpty(_drPJ["PO"].ToString()))
                    {
                        _objPJ.PO = Convert.ToInt32(_drPJ["PO"]);
                        _addPJ.PO = Convert.ToInt32(_drPJ["PO"]);
                    }
                    if (!string.IsNullOrEmpty(_drPJ["ReceivePO"].ToString()))
                    {
                        _objPJ.ReceivePo = Convert.ToInt32(_drPJ["ReceivePO"]);
                        _addPJ.ReceivePo = Convert.ToInt32(_drPJ["ReceivePO"]);
                    }
                    _objPJ.TRID = trans;
                    _objPJ.Spec = 0;  // Default to Zero During AP Invoice
                    _objPJ.IDate = Convert.ToDateTime(_drPJ["IDate"]);
                    _objPJ.Disc = !string.IsNullOrEmpty(Convert.ToString(_drPJ["Disc"])) ? Convert.ToDouble(_drPJ["Disc"]) : 0;
                    _objPJ.Custom1 = Convert.ToString(_drPJ["Custom1"]);
                    _objPJ.Custom2 = Convert.ToString(_drPJ["Custom2"]);

                    _addPJ.TRID = trans;
                    _addPJ.Spec = 0;  // Default to Zero During AP Invoice
                    _addPJ.IDate = Convert.ToDateTime(_drPJ["IDate"]);
                    _addPJ.Disc = !string.IsNullOrEmpty(Convert.ToString(_drPJ["Disc"])) ? Convert.ToDouble(_drPJ["Disc"]) : 0;
                    _addPJ.Custom1 = Convert.ToString(_drPJ["Custom1"]);
                    _addPJ.Custom2 = Convert.ToString(_drPJ["Custom2"]);


                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_AddPJ";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addPJ);
                        _newPJID = Convert.ToInt32(_APIResponse.ResponseData);
                    }
                    else
                    {
                        _newPJID = _objBLBill.AddPJ(_objPJ);
                    }

                    _vendorID = Convert.ToInt32(_drPJ["Vendor"]);
                    //_amount = Convert.ToDouble(_drPJ["Amount"]);
                    _amount = Convert.ToDouble(_drPJ["Amount"]) * _paidpercentage / 100;
                    #endregion

                    #region Update PJ

                    _objPJ = new PJ();
                    _objPJ.ConnConfig = Session["config"].ToString();
                    _objPJ.ID = _pjID;
                    _objPJ.fDesc = "Check voided on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " by " + Session["username"].ToString() + " - " + Convert.ToString(_drPJ["fDesc"]);
                    _objPJ.Status = 2;

                    _updatePJOnVoidCheck = new UpdatePJOnVoidCheckParam();
                    _updatePJOnVoidCheck.ConnConfig = Session["config"].ToString();
                    _updatePJOnVoidCheck.ID = _pjID;
                    _updatePJOnVoidCheck.fDesc = "Check voided on " + Convert.ToDateTime(txtVoidDate.Text).ToString("MM/dd/yyyy") + " by " + Session["username"].ToString() + " - " + Convert.ToString(_drPJ["fDesc"]);
                    _updatePJOnVoidCheck.Status = 2;

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_UpdatePJOnVoidCheck";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updatePJOnVoidCheck);
                    }
                    else
                    {
                        _objBLBill.UpdatePJOnVoidCheck(_objPJ);
                    }
                    #endregion

                }

                #region Add OpenAP details
                _objOpenAP.PJID = _pjID;
                _objOpenAP.ConnConfig = Session["config"].ToString();

                _getOpenAPByPJID.PJID = _pjID;
                _getOpenAPByPJID.ConnConfig = Session["config"].ToString();

                DataSet _dsOpen = new DataSet();

                List <OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();
                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetOpenAPByPJID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getOpenAPByPJID);

                    _lstOpenAPViewModel = (new JavaScriptSerializer()).Deserialize<List<OpenAPViewModel>>(_APIResponse.ResponseData);
                    _dsOpen = CommonMethods.ToDataSet<OpenAPViewModel>(_lstOpenAPViewModel);
                }
                else
                {
                    _dsOpen = _objBLBill.GetOpenAPByPJID(_objOpenAP);
                }

                if (_dsOpen.Tables[0].Rows.Count > 0)
                {
                    DataRow _drOpen = _dsOpen.Tables[0].Rows[0];

                    _objOpenAP.ConnConfig = Session["config"].ToString();
                    _objOpenAP.PJID = _newPJID;
                    _objOpenAP.Vendor = Convert.ToInt32(_drOpen["Vendor"]);
                    // _objOpenAP.fDate = Convert.ToDateTime(_drOpen["fDate"]); // in question; as per client Calculated based on Idate and terms
                    _objOpenAP.fDate = Convert.ToDateTime(txtVoidDate.Text);
                    _objOpenAP.Due = Convert.ToDateTime(_drOpen["Due"]);
                    _objOpenAP.Type = 0; // Default Type to Zero During AP Invoice
                    _objOpenAP.fDesc = fDesc;
                    //_objOpenAP.Original = Convert.ToDouble(_drOpen["Original"]);
                    //_objOpenAP.Balance = Convert.ToDouble(_drOpen["Original"]);
                    _objOpenAP.Original = Convert.ToDouble(_amount);
                    _objOpenAP.Balance = Convert.ToDouble(_amount);
                    _objOpenAP.Selected = 0;
                    _objOpenAP.Disc = Convert.ToDouble(_drOpen["Disc"]);
                    _objOpenAP.TRID = trans;
                    _objOpenAP.Ref = _drOpen["Ref"].ToString();


                    _addOpenAP.ConnConfig = Session["config"].ToString();
                    _addOpenAP.PJID = _newPJID;
                    _addOpenAP.Vendor = Convert.ToInt32(_drOpen["Vendor"]);
                    // _objOpenAP.fDate = Convert.ToDateTime(_drOpen["fDate"]); // in question; as per client Calculated based on Idate and terms
                    _addOpenAP.fDate = Convert.ToDateTime(txtVoidDate.Text);
                    _addOpenAP.Due = Convert.ToDateTime(_drOpen["Due"]);
                    _addOpenAP.Type = 0; // Default Type to Zero During AP Invoice
                    _addOpenAP.fDesc = fDesc;
                    //_objOpenAP.Original = Convert.ToDouble(_drOpen["Original"]);
                    //_objOpenAP.Balance = Convert.ToDouble(_drOpen["Original"]);
                    _addOpenAP.Original = Convert.ToDouble(_amount);
                    _addOpenAP.Balance = Convert.ToDouble(_amount);
                    _addOpenAP.Selected = 0;
                    _addOpenAP.Disc = Convert.ToDouble(_drOpen["Disc"]);
                    _addOpenAP.TRID = trans;
                    _addOpenAP.Ref = _drOpen["Ref"].ToString();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_AddOpenAP";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addOpenAP);
                    }
                    else
                    {
                        _objBLBill.AddOpenAP(_objOpenAP);
                    }
                }
                #endregion

                #region delete Open AP

                // Commented on 06-Dec-2019 Because When Delete from OpenAp Then Check Not Showing Bill in Check (Partial Payment Issue)
                //_objOpenAP.ConnConfig = Session["config"].ToString();
                //_objOpenAP.PJID = _pjID;
                //_objBLBill.DeleteOpenAPByPJID(_objOpenAP);

                #endregion

                #region Update Vendor balance

                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = _vendorID;

                _updateVendorBalance.ConnConfig = Session["config"].ToString();
                _updateVendorBalance.ID = _vendorID;

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_UpdateVendorBalance";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateVendorBalance);
                }
                else
                {
                    _objBLVendor.UpdateVendorBalance(_objVendor);
                }
                #endregion

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void UpdateChartBalance()
    {
        try
        {
            _objChart.ConnConfig = Session["config"].ToString();
            _objChart.ID = _objTrans.Acct;
            _objChart.Amount = _objTrans.Amount;

            _updateChartBalance.ConnConfig = Session["config"].ToString();
            _updateChartBalance.ID = _objTrans.Acct;
            _updateChartBalance.Amount = _objTrans.Amount;

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_UpdateChartBalance";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateChartBalance);
            }
            else
            {
                _objBLChart.UpdateChartBalance(_objChart);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private bool IsValidDate()
    {
        DateTime dateValue;
        string[] formats = {"M/d/yyyy", "M/d/yyyy",
                "MM/dd/yyyy", "M/d/yyyy",
                "M/d/yyyy", "M/d/yyyy",
                "M/d/yyyy", "M/d/yyyy",
                "MM/dd/yyyy", "M/dd/yyyy"};
        var sdt = DateTime.TryParseExact(txtFromDate.Text.ToString(), formats,
                            new CultureInfo("en-US"),
                            DateTimeStyles.None,
                            out dateValue);
        var edt = DateTime.TryParseExact(txtToDate.Text.ToString(), formats,
                                new CultureInfo("en-US"),
                                DateTimeStyles.None,
                                out dateValue);

        if (sdt & edt)
        {
            return true;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Please enter valid start date and end date.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return false;
        }
    }
    #endregion
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        String selectedValue = ddlSearch.SelectedValue;
        ShowHideFilterSearch(selectedValue);
        
        
    }
    private void ShowHideFilterSearch(String selectedValue)
    {
        txtSearch.Text = "";
        ddlPaytype.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        if (selectedValue == "Status")
        {
            
            txtSearch.Visible = false;
            ddlPaytype.Visible = false;
            ddlStatus.Visible = true;

        }
        else if (selectedValue == "PayType")
        {
            txtSearch.Visible = false;
            ddlPaytype.Visible = true;
            ddlStatus.Visible = false;
        }
        else
        {
            txtSearch.Visible = true;
            ddlPaytype.Visible = false;
            ddlStatus.Visible = false;
        }
    }
    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchText = string.Empty;
            if (ddlSearch.SelectedValue == "Status")
            {
                searchText = ddlStatus.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "PayType")
            {
                searchText = ddlPaytype.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text;
            }

            List<RetainFilter> filters = new List<RetainFilter>();
            var filterExpression = Convert.ToString(RadGrid_Checks.MasterTableView.FilterExpression);
            if (!string.IsNullOrEmpty(filterExpression))
            {
                foreach (GridColumn column in RadGrid_Checks.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        RetainFilter filter = new RetainFilter();
                        filter.FilterColumn = column.UniqueName;
                        filter.FilterValue = filterValues;
                        filters.Add(filter);
                    }
                }
            }

            Session["ChecksListRadGVFilters"] = filters;

            string urlString = "ChecksReport.aspx?sd=" + txtFromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText;

            // Redirect when close the report
            var redirect = HttpUtility.UrlEncode(Request.RawUrl);
            urlString += "&redirect=" + redirect;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        ddlSearch_SelectedIndexChanged(sender, e);
        //txtFromDate.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
        //txtToDate.Text = DateTime.Now.ToShortDateString();
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        Session["from_Checks"] = txtFromDate.Text;
        Session["end_Checks"] = txtToDate.Text;
        if (rdocheck.Checked)
        {
            Session["Check_Type"] = "1";
            BindChecks();
        }
        else
        {
            Session["Check_Type"] = "2";
            BindRecurringGrid();

        }

        //BindChecks();
        foreach (GridColumn column in RadGrid_Checks.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_Checks.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Checks.Rebind();

        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = "";
        ddlStatus.SelectedIndex = 0;
        ddlPaytype.SelectedIndex = 0;
        upPannelSearch.Update();

    }
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        ddlPaytype.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        ddlSearch_SelectedIndexChanged(sender, e);

                
            DateTime _now = DateTime.Now;
            var _startDate = new DateTime(_now.Year, _now.Month, 1);
            var _endDate = _startDate.AddMonths(1).AddDays(-1);
            txtFromDate.Text = _startDate.ToShortDateString();
            txtToDate.Text = _endDate.ToShortDateString();
            Session["from_Checks"] = txtFromDate.Text;
            Session["end_Checks"] = txtToDate.Text;
        



        if (rdocheck.Checked)
        {
            Session["Check_Type"] = "1";
            BindChecks();
        }
        else
        {
            Session["Check_Type"] = "2";
            BindRecurringGrid();

        }


        //BindChecks();
        foreach (GridColumn column in RadGrid_Checks.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_Checks.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Checks.Rebind();
        upPannelSearch.Update();
    }
    private void BindSearchFilters()
    {
        Dictionary<string, string> listsearchitems = new Dictionary<string, string>();
        listsearchitems.Add("0", "Select");
        listsearchitems.Add("Vendor", "Vendor");
        listsearchitems.Add("CheckNum", "Check Number");
        listsearchitems.Add("Status", "Status");
        listsearchitems.Add("PayType", "Payment type");
        listsearchitems.Add("Bank", "Bank");
        ddlSearch.DataSource = listsearchitems;
        ddlSearch.DataTextField = "Value";
        ddlSearch.DataValueField = "Key";
        ddlSearch.DataBind();

    }
    private void BindStatus()
    {
        Dictionary<string, string> listsearchitems = new Dictionary<string, string>();
        listsearchitems.Add("0", "Open");
        listsearchitems.Add("1", "Cleared");
        listsearchitems.Add("2", "Voided");
        ddlStatus.DataSource = listsearchitems;
        ddlStatus.DataTextField = "Value";
        ddlStatus.DataValueField = "Key";
        ddlStatus.DataBind();
    }
    private void BindPayType()
    {

        ddlPaytype.Items.Add(new ListItem("Check", "0"));
        ddlPaytype.Items.Add(new ListItem("Cash", "1"));
        ddlPaytype.Items.Add(new ListItem("Wire Transfer", "2"));
        ddlPaytype.Items.Add(new ListItem("ACH", "3"));
        ddlPaytype.Items.Add(new ListItem("Credit Card", "4"));

    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Checks.MasterTableView.FilterExpression != "" ||
            (RadGrid_Checks.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Checks.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Checks_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Checks.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Checks_FilterExpression"] != null && Convert.ToString(Session["Checks_FilterExpression"]) != "" && Session["Checks_Filters"] != null)
                {
                    RadGrid_Checks.MasterTableView.FilterExpression = Convert.ToString(Session["Checks_FilterExpression"]);
                    var filtersGet = Session["Checks_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_Checks.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["Checks_FilterExpression"] = null;
                Session["Checks_Filters"] = null;
            }
        }
        #endregion
        // BindChecks();

        if (rdocheck.Checked)
        {
            Session["Check_Type"] = "1";
            BindChecks();
        }
        else
        {
            Session["Check_Type"] = "2";
            BindRecurringGrid();

        }
    }
    protected void RadGrid_Checks_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        //lblRecordCount.Text = rowCount + " Record(s) found";
        //updpnl.Update();
    }
    protected void RadGrid_Checks_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
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
        catch { }
    }
    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_Checks.Items)
        {
            Label lblID = (Label)gr.FindControl("lblIndex");
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkRef");
            //lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='editcheck.aspx?id=" + lblID.Text + "'";
            if (rdocheck.Checked==true)
            {
                lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='editcheck.aspx?id=" + lblID.Text + "&frm=MNG'";
            }
            else if (rdoRecurring.Checked == true)
            {
                lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='WriteChecksRecur.aspx?bill=c&id=" + lblID.Text + "&frm=MNG'";
            }
        }
    }
    protected void RadGrid_Checks_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_Checks.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Checks_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Checks.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }
            Session["Checks_Filters"] = filters;
        }
        else
        {
            Session["Checks_FilterExpression"] = null;
            Session["Checks_Filters"] = null;
        }

        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Checks);
        #endregion  
        RowSelect();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Checks.MasterTableView.GetColumn("chkSelect").Visible = false;
        RadGrid_Checks.ExportSettings.FileName = "ManageCheck";
        RadGrid_Checks.ExportSettings.IgnorePaging = true;
        RadGrid_Checks.ExportSettings.ExportOnlyData = true;
        RadGrid_Checks.ExportSettings.OpenInNewWindow = true;
        RadGrid_Checks.ExportSettings.HideStructureColumns = true;
        RadGrid_Checks.MasterTableView.UseAllDataFields = true;
        RadGrid_Checks.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Checks.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_Checks_ExcelMLExportRowCreated(object sender, Telerik.Web.UI.GridExcelBuilder.GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 5;
        else
            currentItem = 6;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Checks.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Checks.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
            RowElement row = new RowElement(); //create new row for the footer aggregates
            for (int i = currentItem; i < footerItem.Cells.Count; i++)
            {
                TableCell fcell = footerItem.Cells[i];
                CellElement cell = new CellElement();
                // cell.Data.DataItem =  fcell.Text == "&nbsp;" ? "" : fcell.Text;
                if (i == currentItem)
                    cell.Data.DataItem = "Total:-";
                else
                    cell.Data.DataItem = fcell.Text == "&nbsp;" ? "" : fcell.Text;
                row.Cells.Add(cell);
            }
            e.Worksheet.Table.Rows.Add(row);

        }

        DataSet dsCDDataType = new DataSet();
        _objCD.ConnConfig = Session["config"].ToString();
        _getDataTypeCD.ConnConfig = Session["config"].ToString();

        List<CDViewModel> _lstCDViewModel = new List<CDViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetDataTypeCD";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getDataTypeCD);

            _lstCDViewModel = (new JavaScriptSerializer()).Deserialize<List<CDViewModel>>(_APIResponse.ResponseData);
            dsCDDataType = CommonMethods.ToDataSet<CDViewModel>(_lstCDViewModel);
        }
        else
        {
            dsCDDataType = _objBLBill.GetDataTypeCD(_objCD);
        }

        var getDataType = "";
        foreach (DataRow dr in dsCDDataType.Tables[0].Rows)
        {
            getDataType = dr.ItemArray[1].ToString();
            if (getDataType == "datetime")
            {
                e.Row.Cells.GetCellByName(dr.ItemArray[0].ToString()).StyleValue = "DateTimeStyle";
            }
            else if (getDataType == "numeric")
            {
                e.Row.Cells.GetCellByName(dr.ItemArray[0].ToString()).StyleValue = "NumericStyle";
            }
        }
    }

    protected void RadGrid_Checks_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
    {
        StyleElement datetime = new StyleElement("DateTimeStyle");
        datetime.NumberFormat.FormatType = NumberFormatType.ShortDate;
        e.Styles.Add(datetime);
        StyleElement numeric = new StyleElement("NumericStyle");
        numeric.NumberFormat.FormatType = NumberFormatType.Fixed;
        e.Styles.Add(numeric);
    }
    private void FillBank()
    {
        try
        {
            if (Session["COPer"].ToString() == "1")
            {
                //do nothing
            }
            else
            {
                _objBank.ConnConfig = Session["config"].ToString();
                _getAllBankNames.ConnConfig = Session["config"].ToString();

                DataSet _dsBank = new DataSet();

                List<GetAllBankNamesViewModel> _lstGetAllBankNamesViewModel = new List<GetAllBankNamesViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetAllBankNames";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllBankNames);

                    _lstGetAllBankNamesViewModel = (new JavaScriptSerializer()).Deserialize<List<GetAllBankNamesViewModel>>(_APIResponse.ResponseData);
                    _dsBank = CommonMethods.ToDataSet<GetAllBankNamesViewModel>(_lstGetAllBankNamesViewModel);
                }
                else
                {
                    _dsBank = _objBL_Bank.GetAllBankNames(_objBank);
                }
                if (_dsBank.Tables[0].Rows.Count > 0)
                {
                    ddlBank.Items.Clear();
                    ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(":: Select ::", "0"));
                    ddlBank.AppendDataBoundItems = true;

                    ddlBank.DataSource = _dsBank;
                    ddlBank.DataValueField = "ID";
                    ddlBank.DataTextField = "fDesc";
                    ddlBank.DataBind();
                }
                else
                {
                    ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnSubmit_Click1(object sender, EventArgs e)
    {


        try
        {

            string script = "function f(){$find(\"" + RadWindowTemplates.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

        }
        catch (Exception ex)
        {
            string str = "These month/year period is closed out. You do not have permission to process the check.";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});setTimeout(function(){ location.reload(); }, 5000);", true);
        }

    }
    protected void ddlApTopCheckForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {


    }
    List<byte[]> lstbyte = new List<byte[]>();
    List<byte[]> lstbyteNew = new List<byte[]>();
    protected void imgPrintTemp1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            byte[] buffer1 = null;
            if (ddlApTopCheckForLoad.SelectedItem.Text.Trim() != null)
            {

                string reportApTopCheckPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + ddlApTopCheckForLoad.SelectedItem.Text.Trim() + ".mrt");
                //  string reportPathStimul = Server.MapPath("StimulsoftReports/APTopCheck.mrt");

                StiReport report = new StiReport();

                FillReportApTopCheckDataSet(ddlApTopCheckForLoad.SelectedItem.Text.Trim());
                //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "document.getElementById('loaders').style.display = 'block';", true);
                //report = FillDataSetToReport(ddlApTopCheckForLoad.SelectedItem.Text.Trim());
                //var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                //var service = new Stimulsoft.Report.Export.StiPdfExportService();
                //System.IO.MemoryStream stream = new System.IO.MemoryStream();
                //service.ExportTo(report, stream, settings);
                //buffer1 = stream.ToArray();

                //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "APCheckTop.pdf");

                //if (buffer1 != null)
                //{
                //    if (File.Exists(filename))
                //        File.Delete(filename);
                //    using (var fs = new FileStream(filename, FileMode.Create))
                //    {
                //        fs.Write(buffer1, 0, buffer1.Length);
                //        fs.Close();
                //    }
                //}

                ////END


                ////rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsInvoices", _dti));
                ////rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsCheck", _dtCheck));

                ////rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));
                ////string reportPath = "Reports/ReportCheck.rdlc";

                ////rvChecks.LocalReport.ReportPath = reportPath;

                ////rvChecks.LocalReport.EnableExternalImages = true;
                ////List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
                ////string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
                ////param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));

                ////rvChecks.LocalReport.SetParameters(param1);

                ////rvChecks.LocalReport.Refresh();

                ////byte[] buffer = null;
                ////buffer = ExportReportToPDF("", rvChecks);
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.AddHeader("Content-Disposition", "attachment;filename=PrintCheck.pdf");
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-Length", (buffer1.Length).ToString());
                //Response.BinaryWrite(buffer1);
                //Response.Flush();
                //Response.Close();
            }
        }



        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ImageButton7_Click(object sender, ImageClickEventArgs e)
    {
        //mpeTemplate.Hide();
        string reportName = ddlApTopCheckForLoad.SelectedItem.Text.Trim();
        StiReport report = FillDataSetToReport(reportName);
        StiWebDesigner1.Report = report;
        //ReportModalPopupExtender.Show();
        StiWebDesigner1.Visible = true;

        Session["wc_first"] = "true";

        string script = "function f(){$find(\"" + RadWindowFirstReport.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    }
    protected void StiWebDesigner1_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        //ReportModalPopupExtender.Hide();
        Session["wc_first"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + e.FileName));

    }

    protected void StiWebDesigner2_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        // ReportModalPopupExtender1.Hide();
        Session["wc_second"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + e.FileName));

    }

    protected void StiWebDesigner3_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        // ReportModalPopupExtender2.Hide();
        Session["wc_third"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + e.FileName));

    }
    protected void StiWebDesigner1_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + e.FileName));
    }
    protected void StiWebDesigner1_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("ManageChecks.aspx");
    }

    protected void StiWebDesigner2_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + e.FileName));
    }

    protected void StiWebDesigner3_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + e.FileName));
    }

    protected void StiWebDesigner2_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("ManageChecks.aspx");

    }

    protected void StiWebDesigner3_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("ManageChecks.aspx");
    }

    private StiReport FillDataSetToReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));

        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtcheckfrom.Text);
        _objCD.NextC = long.Parse(txtcheckto.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
        _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
        _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
        _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();
        ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

            _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

            _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
            _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();
        }
        else
        {
            _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
        }


        //int vid = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["Vendor"].ToString());
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Name");
        dtNew.Columns.Add("Vendor");

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        }
        else
        {
            foreach (DataRow drow in _dsCheck.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        }

        DataTable dtN = dtNew.DefaultView.ToTable(true);
        DataTable _dtAcct = new DataTable();
        //foreach (DataRow dr in dtN.Rows)
        //{
        int vid = Convert.ToInt32(dtN.Rows[0]["Vendor"].ToString());
        double AmountPay = 0.00;
        SumAmountpay = 0.00;



        //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
        DataView dtInv = new DataView();

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            dtInv = _dsCheck1.Tables[0].DefaultView;
        }
        else
        {
            dtInv = _dsCheck.Tables[0].DefaultView;
        }

        dtInv.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtInv.ToTable(true).Rows)
        {
            _dri = _dti.NewRow();
            _dri["Ref"] = drow["Ref"].ToString();
            _dri["Description"] = drow["Description"].ToString();
            _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
            _dri["Reference"] = drow["Refrerence"].ToString();
            _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                          NumberStyles.AllowThousands |
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
            _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
            SumAmountpay = SumAmountpay + Convert.ToDouble(drow["AmountPay"].ToString());
            _dri["PayDate"] = drow["PayDate"].ToString();
            _dri["CheckNo"] = drow["CheckNo"].ToString();


            //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
            _dri["VendorID"] = drow["Vendor"].ToString();
            //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
            _dri["VendorName"] = drow["VendorName"].ToString();
            _dti.Rows.Add(_dri);

            _dti.AcceptChanges();

        }










        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
            //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }


        DataView dtcheck = new DataView();
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            dtcheck = _dsCheck2.Tables[0].DefaultView;
        }
        else
        {
            dtcheck = _dsCheck.Tables[1].DefaultView;
        }

        dtcheck.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
        {
            _drC = _dtCheck.NewRow();
            if (Convert.ToDouble(drow["Pay"]) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                //_drC["Pay"] = ViewState["Dollar"].ToString();
            }
            else
            {
                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = drow["ToOrder"].ToString();
            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = drow["Date"].ToString();
            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;

            _drC["TotalAmountpay"] = SumAmountpay;
            _drC["State"] = drow["State"].ToString();
            _dtCheck.Rows.Add(_drC);
        }






        DataSet dsCC = new DataSet();
        User objPropUser = new User();
        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _getControlBranch.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() != "TS")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsCC = objBL_User.getControl(objPropUser);
            }
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);
            _getControlBranch.LocID = Convert.ToInt32(0);

            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsCC = objBL_User.getControlBranch(objPropUser);
            }
        }
        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + reportName.Trim() + ".mrt");
        //  string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/APTopCheckDefault.mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
        }
        else
        {
            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
        }

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsCC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";

        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;


    }
    protected void lnkSaveDefault_Click(object sender, EventArgs e)
    {
        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/ApTopCheckDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");
            string selValue = ddlApTopCheckForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    Response.Redirect("ManageChecks.aspx");

                }
                else
                    throw new Exception("ApTopCheckDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }


    }
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {

        string filePath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");

        string selValue = ddlApTopCheckForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("ApTopCheckDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                ddlApTopCheckForLoad.Items.Clear();

                string path = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/");
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.mrt");
                foreach (FileInfo file in Files)
                {
                    string FileName = string.Empty;
                    if (file.Name.Contains(".mrt"))
                        FileName = file.Name.Replace(".mrt", " ");
                    ddlApTopCheckForLoad.Items.Add((FileName));
                }
                ddlApTopCheckForLoad.Items.Remove(selValue);

                ddlApTopCheckForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            // btnCutCheck.Visible = true;
        }
    }
    protected void ddlApMiddleCheckForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void imgPrintTemp2_Click(object sender, ImageClickEventArgs e)
    {                                                                        //                AP – check middle 
        try
        {


            byte[] buffer1 = null;
            //  string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/APMidCheckDefault.mrt");
            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + ddlApMiddleCheckForLoad.SelectedItem.Text.Trim() + ".mrt");

            StiReport report = new StiReport();
            //  report = FillMiddleDataSetReport("APMidCheckDefault");
            FillReportMiddleDataSet(ddlApMiddleCheckForLoad.SelectedItem.Text.Trim());
            //AddCheck();
            //var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            //var service = new Stimulsoft.Report.Export.StiPdfExportService();
            //System.IO.MemoryStream stream = new System.IO.MemoryStream();
            //service.ExportTo(report, stream, settings);
            //buffer1 = stream.ToArray();

            //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "APCheckMiddle.pdf");

            //if (buffer1 != null)
            //{
            //    if (File.Exists(filename))
            //        File.Delete(filename);
            //    using (var fs = new FileStream(filename, FileMode.Create))
            //    {
            //        fs.Write(buffer1, 0, buffer1.Length);
            //        fs.Close();
            //    }
            //}

            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.AddHeader("Content-Disposition", "attachment;filename=PrintCheck.pdf");
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Length", (buffer1.Length).ToString());
            //Response.BinaryWrite(buffer1);
            //Response.Flush();
            //Response.Close();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
    {

        //mpeTemplate.Hide();

        string reportName = ddlApMiddleCheckForLoad.SelectedItem.Text.Trim();
        StiReport report = FillMiddleDataSetReport(reportName);
        StiWebDesigner2.Report = report;
        //ReportModalPopupExtender1.Show();
        Session["wc_second"] = "true";
        StiWebDesigner2.Visible = true;

        string script = "function f(){$find(\"" + RadWindowSecondReport.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    }
    private StiReport FillMiddleDataSetReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));


        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtcheckfrom.Text);
        _objCD.NextC = long.Parse(txtcheckto.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
        _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
        _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
        _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();
        ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

            _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

            _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
            _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();
        }
        else
        {
            _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
        }


        //int vid = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["Vendor"].ToString());
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Name");
        dtNew.Columns.Add("Vendor");

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        }
        else
        {
            foreach (DataRow drow in _dsCheck.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        }

        DataTable dtN = dtNew.DefaultView.ToTable(true);
        DataTable _dtAcct = new DataTable();
        //foreach (DataRow dr in dtN.Rows)
        //{
        int vid = Convert.ToInt32(dtN.Rows[0]["Vendor"].ToString());
        double AmountPay = 0.00;
        SumAmountpay = 0.00;



        //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
        DataView dtInv = new DataView();
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            dtInv = _dsCheck1.Tables[0].DefaultView;
        }
        else
        {
            dtInv = _dsCheck.Tables[0].DefaultView;
        }

        dtInv.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtInv.ToTable(true).Rows)
        {
            _dri = _dti.NewRow();
            _dri["Ref"] = drow["Ref"].ToString();
            _dri["Description"] = drow["Description"].ToString();
            _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
            _dri["Reference"] = drow["Refrerence"].ToString();
            _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                          NumberStyles.AllowThousands |
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
            _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
            SumAmountpay = SumAmountpay + Convert.ToDouble(drow["AmountPay"].ToString());
            _dri["PayDate"] = drow["PayDate"].ToString();
            _dri["CheckNo"] = drow["CheckNo"].ToString();


            //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
            _dri["VendorID"] = drow["Vendor"].ToString();
            //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
            _dri["VendorName"] = drow["VendorName"].ToString();
            _dti.Rows.Add(_dri);

            _dti.AcceptChanges();

        }










        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
           // _dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }
        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }


        DataView dtcheck = new DataView();
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            dtcheck = _dsCheck2.Tables[0].DefaultView;
        }
        else
        {
            dtcheck = _dsCheck.Tables[1].DefaultView;
        }

        dtcheck.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
        {
            _drC = _dtCheck.NewRow();
            if (Convert.ToDouble(drow["Pay"]) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                //_drC["Pay"] = ViewState["Dollar"].ToString();
            }
            else
            {
                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = drow["ToOrder"].ToString();
            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = drow["Date"].ToString();
            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;

            _drC["TotalAmountpay"] = SumAmountpay;
            _drC["State"] = drow["State"].ToString();
            _dtCheck.Rows.Add(_drC);
        }






        DataSet dsCC = new DataSet();
        User objPropUser = new User();
        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
       
        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _getControlBranch.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() != "TS")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsCC = objBL_User.getControl(objPropUser);
            }
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);
            _getControlBranch.LocID = Convert.ToInt32(0);
            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsCC = objBL_User.getControlBranch(objPropUser);
            }
        }

        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");

        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
        }
        else
        {
            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
        }

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsCC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";

        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;





    }
    protected void lnkSaveApMiddleCheck_Click(object sender, EventArgs e)
    {

        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/APMidCheckDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");

            string selValue = ddlApMiddleCheckForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    Response.Redirect("ManageChecks.aspx");

                }
                else
                    throw new Exception("ApMiddleCheckDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }


    }
    protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
    {

        string filePath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");

        string selValue = ddlApMiddleCheckForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("APMidCheckDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                ddlApMiddleCheckForLoad.Items.Clear();
                string MidCheckpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/");
                DirectoryInfo dirMidPath = new DirectoryInfo(MidCheckpath);
                FileInfo[] FilesMid = dirMidPath.GetFiles("*.mrt");
                foreach (FileInfo fileMid in FilesMid)
                {
                    string FileName = string.Empty;
                    if (fileMid.Name.Contains(".mrt"))
                        FileName = fileMid.Name.Replace(".mrt", " ");
                    ddlApMiddleCheckForLoad.Items.Add((FileName));
                }
                ddlApMiddleCheckForLoad.Items.Remove(selValue);
                ddlApMiddleCheckForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            //btnCutCheck.Visible = true;
        }
    }
    protected void ddlTopChecksForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void imgPrintTemp6_Click(object sender, ImageClickEventArgs e)
    {                                                                           //              MADDEN – check top 
        try
        {

            //F:\ESS\ESSMOM\MOM\MOM - NewDesign\MSWeb\StimulsoftReports\APChecks\APTopCheck
            byte[] buffer1 = null;
            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + ddlTopChecksForLoad.SelectedItem.Text.Trim() + ".mrt");
            StiReport report = new StiReport();
            //  FillDataSetReport2();
            // report = FillMaddenDataSetForReport("TopCheckReportDefault");
            //report = FillMaddenDataSetForReport(ddlTopChecksForLoad.SelectedItem.Text.Trim());
            //var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            //var service = new Stimulsoft.Report.Export.StiPdfExportService();
            //System.IO.MemoryStream stream = new System.IO.MemoryStream();
            //service.ExportTo(report, stream, settings);
            //buffer1 = stream.ToArray();

            //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "\\TempPDF", "TopCheckReport.pdf");

            //if (buffer1 != null)
            //{
            //    if (File.Exists(filename))
            //        File.Delete(filename);
            //    using (var fs = new FileStream(filename, FileMode.Create))
            //    {
            //        fs.Write(buffer1, 0, buffer1.Length);
            //        fs.Close();
            //    }
            //}

            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.AddHeader("Content-Disposition", "attachment;filename=PrintCheck.pdf");
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Length", (buffer1.Length).ToString());
            //Response.BinaryWrite(buffer1);
            //Response.Flush();
            //Response.Close();
            FillReportMaddenDataSet(ddlTopChecksForLoad.SelectedItem.Text.Trim());

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ImageButton9_Click(object sender, ImageClickEventArgs e)
    {

        string reportName = ddlTopChecksForLoad.SelectedItem.Text.Trim();
        // mpeTemplate.Hide();
        StiReport report = FillMaddenDataSetForReport(reportName);
        StiWebDesigner3.Report = report;
        //ReportModalPopupExtender2.Show();
        Session["wc_third"] = "true";
        StiWebDesigner3.Visible = true;

        string script = "function f(){$find(\"" + RadWindowThirdReport.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);


    }
    private StiReport FillMaddenDataSetForReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));

        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtcheckfrom.Text);
        _objCD.NextC = long.Parse(txtcheckto.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
        _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
        _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
        _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();

        ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

            _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);
            _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
            _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

        }
        else
        {
            _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
        }

       
        //int vid = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["Vendor"].ToString());
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Name");
        dtNew.Columns.Add("Vendor");

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        }
        else
        {
            foreach (DataRow drow in _dsCheck.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        }

        DataTable dtN = dtNew.DefaultView.ToTable(true);

        //foreach (DataRow dr in dtN.Rows)
        //{
        int vid = Convert.ToInt32(dtN.Rows[0]["Vendor"].ToString());
        double AmountPay = 0.00;
        SumAmountpay = 0.00;



        //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
        DataView dtInv = new DataView();
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            dtInv = _dsCheck1.Tables[0].DefaultView;
        }
        else
        {
            dtInv = _dsCheck.Tables[0].DefaultView;
        }

        dtInv.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtInv.ToTable(true).Rows)
        {
            _dri = _dti.NewRow();
            _dri["Ref"] = drow["Ref"].ToString();
            _dri["Description"] = drow["Description"].ToString();
            _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
            _dri["Reference"] = drow["Refrerence"].ToString();
            _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                          NumberStyles.AllowThousands |
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
            _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
            SumAmountpay = SumAmountpay + Convert.ToDouble(drow["AmountPay"].ToString());
            _dri["PayDate"] = drow["PayDate"].ToString();
            _dri["CheckNo"] = drow["CheckNo"].ToString();


            //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
            _dri["VendorID"] = drow["Vendor"].ToString();
            //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
            _dri["VendorName"] = drow["VendorName"].ToString();
            _dti.Rows.Add(_dri);

            _dti.AcceptChanges();

        }

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
            //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }


        string chknos = null;
        int checkno = 0;

        DataView dtcheck = new DataView();
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            dtcheck = _dsCheck2.Tables[0].DefaultView;
        }
        else
        {
            dtcheck = _dsCheck.Tables[1].DefaultView;
        }

        dtcheck.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
        {
            _drC = _dtCheck.NewRow();
            if (Convert.ToDouble(drow["Pay"]) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                //_drC["Pay"] = ViewState["Dollar"].ToString();
            }
            else
            {
                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = drow["ToOrder"].ToString();
            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = drow["Date"].ToString();
            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;

            _drC["TotalAmountpay"] = SumAmountpay;
            _drC["State"] = drow["State"].ToString();
            chknos = drow["CheckNo"].ToString();
            _dtCheck.Rows.Add(_drC);
        }

        DataSet dsCC = new DataSet();
        User objPropUser = new User();
        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _getControlBranch.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() != "TS")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsCC = objBL_User.getControl(objPropUser);
            }
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);

            _getControlBranch.LocID = Convert.ToInt32(0);

            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsCC = objBL_User.getControlBranch(objPropUser);
            }
        }
        //dsBank

        CreateTableBank();

        DataRow _drB = null;
        DataRow _drA = null;
        _objBank.ConnConfig = Session["config"].ToString();
        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

        _getBankCD.ConnConfig = Session["config"].ToString();
        _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsB = new DataSet();
        List <BankViewModel> _lstBankViewModel = new List<BankViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

            _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
            _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
            _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
            _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
            _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
            _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
            _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
        }
        else
        {
            _dsB = _objBLBill.GetBankCD(_objBank);
        }

        _drB = dtBank.NewRow();
        if (_dsB.Tables[0].Rows.Count > 0)
        {
            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
            //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
            //_dtBank.Rows.Add(_drB);
        }

        string checkNumber = string.Empty;
        if (!string.IsNullOrEmpty(chknos))
        {
            checkNumber = chknos;
        }
        else
        {
            checkNumber = chknos.ToString();
        }

        if (checkNumber.Length == 1)
        {
            _drB["Ref"] = "00000000" + checkNumber;
        }
        else if (checkNumber.Length == 2)
        {
            _drB["Ref"] = "0000000" + checkNumber;
        }
        else if (checkNumber.Length == 3)
        {
            _drB["Ref"] = "000000" + checkNumber;
        }
        else if (checkNumber.Length == 4)
        {
            _drB["Ref"] = "00000" + checkNumber;
        }
        else if (checkNumber.Length == 5)
        {
            _drB["Ref"] = "0000" + checkNumber;
        }
        else if (checkNumber.Length == 6)
        {
            _drB["Ref"] = "000" + checkNumber;
        }
        else if (checkNumber.Length == 7)
        {
            _drB["Ref"] = "00" + checkNumber;
        }
        else if (checkNumber.Length == 8)
        {
            _drB["Ref"] = "0" + checkNumber;
        }
        else
        {
            _drB["Ref"] = "000000000";
        }

        dtBank.Rows.Add(_drB);

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorAcct.ConnConfig = Session["config"].ToString();
        _getVendorAcct.ID = vid;

        DataSet _dsA = new DataSet();
        List<GetVendorAcctList> _lstGetVendorAcctList = new List<GetVendorAcctList>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

            _lstGetVendorAcctList = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
            _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcctList);
            _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
        }
        else
        {
            _dsA = _objBLVendor.GetVendorAcct(_objVendor);
        }

        DataTable _dtAcct = new DataTable();
        _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));
        if (_dsA.Tables[0].Rows.Count > 0)
        {
            _drA = _dtAcct.NewRow();
            _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
            _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
            _dtAcct.Rows.Add(_drA);
        }


        //dsBank end


        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;


        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
        }
        else
        {
            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
        }

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsCC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";


        DataSet Bank = new DataSet();
        DataTable _dtBank = dtBank;
        dtBank.TableName = "Bank";
        Bank.Tables.Add(dtBank);
        Bank.DataSetName = "Bank";

        DataSet Account = new DataSet();
        DataTable dtAccount = _dtAcct;
        _dtAcct.TableName = "Account";
        Account.Tables.Add(_dtAcct);
        Account.DataSetName = "Account";


        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.RegData("dsBank", Bank);
        report.RegData("dsAccount", Account);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;
    }
    protected void lnkTopChecks_Click(object sender, EventArgs e)
    {

        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/TopCheckReportDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");
            string selValue = ddlTopChecksForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    Response.Redirect("ManageChecks.aspx");

                }
                else
                    throw new Exception("TopCheckReportDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    protected void ImageButton14_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");

        string selValue = ddlTopChecksForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("TopCheckReportDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                ddlTopChecksForLoad.Items.Clear();

                string TopCheckpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/");
                DirectoryInfo dirTopcheckPath = new DirectoryInfo(TopCheckpath);
                FileInfo[] FilesTop = dirTopcheckPath.GetFiles("*.mrt");
                foreach (FileInfo fileTop in FilesTop)
                {
                    string FileName = string.Empty;
                    if (fileTop.Name.Contains(".mrt"))
                        FileName = fileTop.Name.Replace(".mrt", " ");
                    ddlTopChecksForLoad.Items.Add((FileName));
                }
                ddlTopChecksForLoad.Items.Remove((selValue));
                ddlTopChecksForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        //btnCutCheck.Visible = true;

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

    }


    private void FillReportApTopCheckDataSet(string reportName)
    {
        try
        {
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtcheckfrom.Text);
            _objCD.NextC = long.Parse(txtcheckto.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
            _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
            _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
            _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);


            DataSet _dsCheck = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();

            ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

                _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

                _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
                _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

            }
            else
            {
                _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            }
            DataSet ControlBranch = new DataSet();
            DataTable dtControlBranch = new DataTable();
            dtControlBranch = _dsCheck.Tables[2].Copy();
            ControlBranch.Tables.Add(dtControlBranch);
            dtControlBranch.TableName = "ControlBranch";
            ControlBranch.DataSetName = "ControlBranch";

            DataSet AccountAz = new DataSet();
            DataTable dtAccountAz = new DataTable();
            dtAccountAz = _dsCheck.Tables[3].Copy();
            AccountAz.Tables.Add(dtAccountAz);
            dtAccountAz.TableName = "Account";
            AccountAz.DataSetName = "Account";

            DataSet BankAz = new DataSet();
            DataTable dtBankAz = new DataTable();
            dtBankAz = _dsCheck.Tables[4].Copy();
            BankAz.Tables.Add(dtBankAz);
            dtBankAz.TableName = "Bank";
            BankAz.DataSetName = "Bank";

            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            dtNew.Columns.Add("CheckNo");
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            }
            else
            {
                foreach (DataRow drow in _dsCheck.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            }

            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = true;
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();

                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                string checkNo = Convert.ToString(dr["CheckNo"].ToString());
                double AmountPay = 0.00;

                DataView dtInv = new DataView();
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    dtInv = _dsCheck1.Tables[0].DefaultView;
                }
                else
                {
                    dtInv = _dsCheck.Tables[0].DefaultView;
                }
                dtInv.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                foreach (DataRow drow in dtInv.ToTable(true).Rows)
                {
                    _dri = dti.NewRow();
                    _dri["Ref"] = drow["Ref"].ToString();
                    _dri["Description"] = drow["Description"].ToString();
                    _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
                    _dri["Reference"] = drow["Refrerence"].ToString();
                    _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(drow["AmountPay"].ToString());
                    _dri["PayDate"] = drow["PayDate"].ToString();
                    _dri["CheckNo"] = drow["CheckNo"].ToString();

                    _dri["VendorID"] = drow["Vendor"].ToString();
                    _dri["VendorName"] = drow["VendorName"].ToString();
                    dti.Rows.Add(_dri);

                    dti.AcceptChanges();
                }

                if (isChecked)
                {
                    if (dti.Rows.Count > 0)
                    {
                        DataView dtcheck = new DataView();
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            dtcheck = _dsCheck2.Tables[0].DefaultView;
                        }
                        else
                        {
                            dtcheck = _dsCheck.Tables[1].DefaultView;
                        }

                        dtcheck.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                        ViewState["CheckStatus"] = "0";
                        //dtcheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));

                        _objVendor.ConnConfig = Session["config"].ToString();
                        _objVendor.ID = vid;

                        //API
                        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
                        _getVendorRolDetails.ID = vid;
                        DataSet _dsV = new DataSet();
                        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();
                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

                            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                        }
                        else
                        {
                            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
                        }

                        string vendAddress = "";
                        string vendAddress2 = "";
                        if (_dsV.Tables[0].Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
                            {
                                string add1 = Regex.Replace(_dsV.Tables[0].Rows[0]["Address"].ToString(), @"( |\r?\n)\1+", "$1");
                                vendAddress = add1 + ", ";
                            }

                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                            {
                                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
                            }
                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
                            {
                                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                                {
                                    vendAddress2 += ", ";
                                }
                                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " ;
                            }
                        }

                        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
                        {
                            _drC = dtpay.NewRow();
                            if (Convert.ToDouble(drow["Pay"]) > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = drow["ToOrder"].ToString();
                            _drC["Date"] = drow["Date"].ToString();
                            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
                            _drC["VendorAddress"] = drow["VendorAddress"].ToString();
                            _drC["ToOrderAddress"] = drow["VendorAddress"].ToString();
                            _drC["RemitAddress"] = _dsV.Tables[0].Rows[0]["RemitAddress"].ToString();
                            _drC["State"] = drow["State"].ToString();
                            _drC["Zip"] = drow["Zip"].ToString();
                            _drC["VendorAcct"] = drow["VendorAcct"].ToString();
                            ViewState["CheckStatus"] = drow["Status"].ToString();
                            dtpay.Rows.Add(_drC);
                        }

                        var rowCount = 0;
                        var totalRows = dti.Rows.Count;
                        if (reportName.Contains("-"))
                        {
                            try
                            {
                                string[] reportNameArr = reportName.Split('-');
                                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                if (totalRows < rowCount)
                                    rowCount = totalRows;
                            }
                            catch (Exception ex) { rowCount = totalRows; }
                        }
                        else
                            rowCount = 6;
                        var dtiCopy = dti.Copy();
                        DataView dv = dtiCopy.DefaultView;
                        dv.Sort = "Ref asc";
                        DataTable sortedDT = dv.ToTable();
                        var dtCopy = sortedDT.Copy();
                        var firstHalf = dtCopy;
                        var secondHalf = dtCopy;
                        if (dtCopy.Rows.Count > rowCount)
                        {
                            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                            secondHalf = dtCopy.Clone();
                            if (totalRows > rowCount)
                            {
                                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                            }
                        }
                        else
                        {
                            firstHalf = dtCopy;
                        }

                        //STIMULSOFT 
                        byte[] buffer1 = null;
                        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + reportName.Trim() + ".mrt");
                        StiReport report = new StiReport();
                        report.Load(reportPathStimul);
                        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
                        {
                            report.Pages[0].Watermark.Enabled = true;
                            string imagepath = Server.MapPath("images/icons/voidcheck.png");
                            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
                            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
                            report.Pages[0].Watermark.ShowImageBehind = true;
                        }
                        report.Compile();
                        report["TotalAmountPay"] = AmountPay;
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                        }
                        else
                        {
                            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                        }


                        report["InvoiceCount"] = totalRows;
                        DataSet Invoice = new DataSet();
                        DataTable dtInvoice = firstHalf.Copy();
                        dtInvoice.TableName = "Invoice";
                        Invoice.Tables.Add(dtInvoice);
                        Invoice.DataSetName = "Invoice";

                        DataSet Check = new DataSet();
                        DataTable dtCheck = dtpay.Copy();
                        dtCheck.TableName = "Check";
                        Check.Tables.Add(dtCheck);
                        Check.DataSetName = "Check";

                        report.RegData("Invoice", Invoice);
                        report.RegData("Check", Check);
                        report.RegData("ControlBranch", ControlBranch);
                        report.RegData("dsBank", BankAz);
                        report.RegData("dsAccount", AccountAz);

                        report.Render();
                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        buffer1 = stream.ToArray();
                        lstbyte.Add(buffer1);

                        if (totalRows > rowCount)
                        {
                            byte[] bufferNew = null;
                            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                            report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck1.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                            }
                            else
                            {
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                            }

                            report["InvoiceCount"] = totalRows;
                            Invoice = new DataSet();
                            dtInvoice = secondHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            Check = new DataSet();
                            dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.Render();
                            settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            service = new Stimulsoft.Report.Export.StiPdfExportService();
                            stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            bufferNew = stream.ToArray();

                            lstbyteNew.Add(bufferNew);
                        }
                    }
                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();
            }

            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=ApTopCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillReportMaddenDataSet(string reportName)
    {
        try
        {
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtcheckfrom.Text);
            _objCD.NextC = long.Parse(txtcheckto.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
            _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
            _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
            _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            DataSet _dsCheck = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();

            ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

                _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);
                _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
                _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

            }
            else
            {
                _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            }
            DataSet ControlBranch = new DataSet();
            DataTable dtControlBranch = new DataTable();
            dtControlBranch = _dsCheck.Tables[2].Copy();
            ControlBranch.Tables.Add(dtControlBranch);
            dtControlBranch.TableName = "ControlBranch";
            ControlBranch.DataSetName = "ControlBranch";


            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            dtNew.Columns.Add("CheckNo");

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            }
            else
            {
                foreach (DataRow drow in _dsCheck.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            }

            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = true;
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();


                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                string checkNo = Convert.ToString(dr["CheckNo"].ToString());
                DataRow _drB = null;
                DataRow _drA = null;
                double AmountPay = 0.00;

                DataView dtInv = new DataView();
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    dtInv = _dsCheck1.Tables[0].DefaultView;
                }
                else
                {
                    dtInv = _dsCheck.Tables[0].DefaultView;
                }

                dtInv.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                foreach (DataRow drow in dtInv.ToTable(true).Rows)
                {
                    _dri = dti.NewRow();
                    _dri["Ref"] = drow["Ref"].ToString();
                    _dri["Description"] = drow["Description"].ToString();
                    _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
                    _dri["Reference"] = drow["Refrerence"].ToString();
                    _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(drow["AmountPay"].ToString());
                    _dri["PayDate"] = drow["PayDate"].ToString();
                    _dri["CheckNo"] = drow["CheckNo"].ToString();
                    _dri["VendorID"] = drow["Vendor"].ToString();
                    _dri["VendorName"] = drow["VendorName"].ToString();
                    dti.Rows.Add(_dri);

                    dti.AcceptChanges();
                }
                if (isChecked)
                {
                    if (dti.Rows.Count > 0)
                    {
                        string chknos = null;
                        DataView dtcheck = new DataView();
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            dtcheck = _dsCheck2.Tables[0].DefaultView;
                        }
                        else
                        {
                            dtcheck = _dsCheck.Tables[1].DefaultView;
                        }

                        dtcheck.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                        ViewState["CheckStatus"] = "0";
                        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
                        {
                            _drC = dtpay.NewRow();
                            if (Convert.ToDouble(drow["Pay"]) > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = drow["ToOrder"].ToString();
                            _drC["Date"] = drow["Date"].ToString();
                            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
                            _drC["VendorAddress"] = drow["VendorAddress"].ToString();             
                            _drC["RemitAddress"] = drow["RemitAddress"].ToString();       
                            _drC["State"] = drow["State"].ToString();
                            _drC["Zip"] = drow["Zip"].ToString();
                            ViewState["CheckStatus"] = drow["Status"].ToString();
                            chknos = drow["CheckNo"].ToString();
                            dtpay.Rows.Add(_drC);
                        }

                        _objBank.ConnConfig = Session["config"].ToString();
                        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

                        _getBankCD.ConnConfig = Session["config"].ToString();
                        _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);

                        DataSet _dsB = new DataSet();
                        List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

                            _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                            _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                            _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
                            _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
                            _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
                            _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
                            _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";

                        }
                        else
                        {
                            _dsB = _objBLBill.GetBankCD(_objBank);
                        }

                        _drB = dtBank.NewRow();
                        if (_dsB.Tables[0].Rows.Count > 0)
                        {
                            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                        }
                        string checkNumber = string.Empty;
                        if (!string.IsNullOrEmpty(chknos))
                        {
                            checkNumber = chknos.ToString();
                        }
                        else
                        {
                            checkNumber = chknos.ToString();
                        }

                        if (checkNumber.Length == 1)
                        {
                            _drB["Ref"] = "00000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 2)
                        {
                            _drB["Ref"] = "0000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 3)
                        {
                            _drB["Ref"] = "000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 4)
                        {
                            _drB["Ref"] = "00000" + checkNumber;
                        }
                        else if (checkNumber.Length == 5)
                        {
                            _drB["Ref"] = "0000" + checkNumber;
                        }
                        else if (checkNumber.Length == 6)
                        {
                            _drB["Ref"] = "000" + checkNumber;
                        }
                        else if (checkNumber.Length == 7)
                        {
                            _drB["Ref"] = "00" + checkNumber;
                        }
                        else if (checkNumber.Length == 8)
                        {
                            _drB["Ref"] = "0" + checkNumber;
                        }
                        else
                        {
                            _drB["Ref"] = "000000000";
                        }

                        dtBank.Rows.Add(_drB);

                        _objVendor.ConnConfig = Session["config"].ToString();
                        _objVendor.ID = vid;

                        _getVendorAcct.ConnConfig = Session["config"].ToString();
                        _getVendorAcct.ID = vid;

                        DataSet _dsA = new DataSet();
                        List<GetVendorAcctList> _lstGetVendorAcctList = new List<GetVendorAcctList>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                            _lstGetVendorAcctList = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                            _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcctList);
                            _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                        }
                        else
                        {
                            _dsA = _objBLVendor.GetVendorAcct(_objVendor);
                        }
                        
                        _drA = _dtAcct.NewRow();
                        _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
                        _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                        _dtAcct.Rows.Add(_drA);

                        var rowCount = 0;
                        var totalRows = dti.Rows.Count;
                        if (reportName.Contains("-"))
                        {
                            try
                            {
                                string[] reportNameArr = reportName.Split('-');
                                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                if (totalRows < rowCount)
                                    rowCount = totalRows;
                            }
                            catch (Exception ex) { rowCount = totalRows; }
                        }
                        else
                            rowCount = 6;
                        var dtiCopy = dti.Copy();
                        DataView dv = dtiCopy.DefaultView;
                        dv.Sort = "Ref asc";
                        DataTable sortedDT = dv.ToTable();
                        var dtCopy = sortedDT.Copy();
                        var firstHalf = dtCopy;
                        var secondHalf = dtCopy;
                        if (dtCopy.Rows.Count > rowCount)
                        {
                            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                            secondHalf = dtCopy.Clone();
                            if (totalRows > rowCount)
                            {
                                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                            }
                        }
                        else
                        {
                            firstHalf = dtCopy;
                        }

                        DataSet dsCC = new DataSet();
                        User objPropUser = new User();
                        objPropUser.ConnConfig = Session["config"].ToString();
                        _getConnectionConfig.ConnConfig = Session["config"].ToString();
                        _getControlBranch.ConnConfig = Session["config"].ToString();

                        if (Session["MSM"].ToString() != "TS")
                        {
                            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                                dsCC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                            }
                            else
                            {
                                dsCC = objBL_User.getControl(objPropUser);
                            }
                        }
                        else
                        {
                            objPropUser.LocID = Convert.ToInt32(0);
                            _getControlBranch.LocID = Convert.ToInt32(0);

                            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                                dsCC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                            }
                            else
                            {
                                dsCC = objBL_User.getControlBranch(objPropUser);
                            }

                        }

                        //STIMULSOFT 
                        byte[] buffer1 = null;
                        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
                        StiReport report = new StiReport();
                        report.Load(reportPathStimul);
                        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
                        {
                            report.Pages[0].Watermark.Enabled = true;
                            string imagepath = Server.MapPath("images/icons/voidcheck.png");
                            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
                            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
                            report.Pages[0].Watermark.ShowImageBehind = true;
                        }
                        report.Compile();
                        report["TotalAmountPay"] = AmountPay;

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                        }
                        else
                        {
                            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                        }

                        report["InvoiceCount"] = totalRows;
                        if (dsCC.Tables[0].Rows.Count > 0)
                        {
                            report["CompanyName"] = dsCC.Tables[0].Rows[0]["Name"].ToString();
                            report["CompanyAddress"] = dsCC.Tables[0].Rows[0]["Address"].ToString();
                            report["CompanyCity"] = dsCC.Tables[0].Rows[0]["City"].ToString() + "' " + dsCC.Tables[0].Rows[0]["State"].ToString() + " - " + dsCC.Tables[0].Rows[0]["Zip"].ToString();
                        }
                        DataSet Invoice = new DataSet();
                        DataTable dtInvoice = firstHalf.Copy();
                        dtInvoice.TableName = "Invoice";
                        Invoice.Tables.Add(dtInvoice);
                        Invoice.DataSetName = "Invoice";

                        DataSet Check = new DataSet();
                        DataTable dtCheck = dtpay.Copy();
                        dtCheck.TableName = "Check";
                        Check.Tables.Add(dtCheck);
                        Check.DataSetName = "Check";

                        //DataSet ControlBranch = new DataSet();
                        //DataTable dtControlBranch = new DataTable();
                        //dtControlBranch = dsCC.Tables[0].Copy();
                        //ControlBranch.Tables.Add(dtControlBranch);
                        //dtControlBranch.TableName = "ControlBranch";
                        //ControlBranch.DataSetName = "ControlBranch";

                        DataSet Bank = new DataSet();
                        DataTable _dtBank = dtBank.Copy();
                        _dtBank.TableName = "Bank";
                        Bank.Tables.Add(_dtBank);
                        Bank.DataSetName = "Bank";

                        DataSet Account = new DataSet();
                        DataTable dtAccount = _dtAcct.Copy();
                        dtAccount.TableName = "Account";
                        Account.Tables.Add(dtAccount);
                        Account.DataSetName = "Account";

                        report.RegData("Invoice", Invoice);
                        report.RegData("Check", Check);                        
                        report.RegData("ControlBranch", ControlBranch);
                        report.RegData("dsBank", Bank);
                        report.RegData("dsAccount", Account);
                        report.Render();

                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        buffer1 = stream.ToArray();
                        lstbyte.Add(buffer1);

                        if (totalRows > rowCount)
                        {
                            byte[] bufferNew = null;
                            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                            report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck1.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                            }
                            else
                            {
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                            }

                            report["InvoiceCount"] = totalRows;
                            Invoice = new DataSet();
                            dtInvoice = secondHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            Check = new DataSet();
                            dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            ControlBranch = new DataSet();
                            dtControlBranch = new DataTable();
                            dtControlBranch = dsCC.Tables[0].Copy();
                            ControlBranch.Tables.Add(dtControlBranch);
                            dtControlBranch.TableName = "ControlBranch";
                            ControlBranch.DataSetName = "ControlBranch";

                            Bank = new DataSet();
                            _dtBank = dtBank.Copy();
                            _dtBank.TableName = "Bank";
                            Bank.Tables.Add(_dtBank);
                            Bank.DataSetName = "Bank";

                            Account = new DataSet();
                            dtAccount = _dtAcct.Copy();
                            dtAccount.TableName = "Account";
                            Account.Tables.Add(dtAccount);
                            Account.DataSetName = "Account";

                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.RegData("Bank", Bank);
                            report.RegData("Account", Account);
                            report.Render();

                            settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            service = new Stimulsoft.Report.Export.StiPdfExportService();
                            stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            bufferNew = stream.ToArray();

                            lstbyteNew.Add(bufferNew);
                        }
                    }
                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();
            }

            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=TopDetailCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillReportMiddleDataSet(string reportName)
    {
        try
        {
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtcheckfrom.Text);
            _objCD.NextC = long.Parse(txtcheckto.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
            _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
            _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
            _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            DataSet _dsCheck = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();

            ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

                _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);
                _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
                _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

            }
            else
            {
                _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            }
            DataSet ControlBranch = new DataSet();
            DataTable dtControlBranch = new DataTable();
            dtControlBranch = _dsCheck.Tables[2].Copy();
            ControlBranch.Tables.Add(dtControlBranch);
            dtControlBranch.TableName = "ControlBranch";
            ControlBranch.DataSetName = "ControlBranch";

            DataSet AccountAz = new DataSet();
            DataTable dtAccountAz = new DataTable();
            dtAccountAz = _dsCheck.Tables[3].Copy();
            AccountAz.Tables.Add(dtAccountAz);
            dtAccountAz.TableName = "Account";
            AccountAz.DataSetName = "Account";

            DataSet BankAz = new DataSet();
            DataTable dtBankAz = new DataTable();
            dtBankAz = _dsCheck.Tables[4].Copy();
            BankAz.Tables.Add(dtBankAz);
            dtBankAz.TableName = "Bank";
            BankAz.DataSetName = "Bank";

            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            dtNew.Columns.Add("CheckNo");

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            }
            else
            {
                foreach (DataRow drow in _dsCheck.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            }

            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = true;
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();

                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                string checkNo = Convert.ToString(dr["CheckNo"].ToString());

                //RAHIL'S IMPLEMENTATION
                double AmountPay = 0.00;

                DataView dtInv = new DataView();
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    dtInv = _dsCheck1.Tables[0].DefaultView;
                }
                else
                {
                    dtInv = _dsCheck.Tables[0].DefaultView;
                }

                dtInv.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                foreach (DataRow drow in dtInv.ToTable(true).Rows)
                {
                    _dri = dti.NewRow();
                    _dri["Ref"] = drow["Ref"].ToString();
                    _dri["Description"] = drow["Description"].ToString();
                    _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
                    _dri["Reference"] = drow["Refrerence"].ToString();
                    _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(drow["AmountPay"].ToString());
                    _dri["PayDate"] = drow["PayDate"].ToString();
                    _dri["CheckNo"] = drow["CheckNo"].ToString();
                    _dri["VendorID"] = drow["Vendor"].ToString();
                    _dri["VendorName"] = drow["VendorName"].ToString();
                    dti.Rows.Add(_dri);

                    dti.AcceptChanges();
                }
                if (isChecked)
                {
                    if (dti.Rows.Count > 0)
                    {
                        DataView dtcheck = new DataView();
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            dtcheck = _dsCheck2.Tables[0].DefaultView;
                        }
                        else
                        {
                            dtcheck = _dsCheck.Tables[1].DefaultView;
                        }

                        dtcheck.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                        ViewState["CheckStatus"] = "0";
                        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
                        {
                            _drC = dtpay.NewRow();
                            if (Convert.ToDouble(drow["Pay"]) > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = drow["ToOrder"].ToString();
                            _drC["Date"] = drow["Date"].ToString();
                            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
                            _drC["VendorAddress"] = drow["VendorAddress"].ToString();  
                            _drC["RemitAddress"] = drow["RemitAddress"].ToString();
                            _drC["State"] = drow["State"].ToString();
                            _drC["Zip"] = drow["Zip"].ToString();
                            _drC["VendorAcct"] = drow["VendorAcct"].ToString();
                            ViewState["CheckStatus"] = drow["Status"].ToString();
                            dtpay.Rows.Add(_drC);
                        }

                        var rowCount = 0;
                        var totalRows = dti.Rows.Count;
                        if (reportName.Contains("-"))
                        {
                            try
                            {
                                string[] reportNameArr = reportName.Split('-');
                                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                if (totalRows < rowCount)
                                    rowCount = totalRows;
                            }
                            catch (Exception ex) { rowCount = totalRows; }
                        }
                        else
                            rowCount = 6;

                        var dtiCopy = dti.Copy();
                        DataView dv = dtiCopy.DefaultView;
                        dv.Sort = "Ref asc";
                        DataTable sortedDT = dv.ToTable();
                        var dtCopy = sortedDT.Copy();
                        var firstHalf = dtCopy;
                        var secondHalf = dtCopy;
                        if (dtCopy.Rows.Count > rowCount)
                        {
                            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                            secondHalf = dtCopy.Clone();
                            if (totalRows > rowCount)
                            {
                                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                            }
                        }
                        else
                        {
                            firstHalf = dtCopy;
                        }

                        //STIMULSOFT 
                        byte[] buffer1 = null;
                        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");
                        StiReport report = new StiReport();
                        report.Load(reportPathStimul);
                        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
                        {
                            report.Pages[0].Watermark.Enabled = true;
                            string imagepath = Server.MapPath("images/icons/voidcheck.png");
                            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
                            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                            report.Pages[0].Watermark.ShowImageBehind = true;
                        }
                        report.Compile();
                        report["TotalAmountPay"] = AmountPay;

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                        }
                        else
                        {
                            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                        }

                        report["InvoiceCount"] = totalRows;
                        DataSet Invoice = new DataSet();
                        DataTable dtInvoice = firstHalf.Copy();
                        dtInvoice.TableName = "Invoice";
                        Invoice.Tables.Add(dtInvoice);
                        Invoice.DataSetName = "Invoice";

                        DataSet Check = new DataSet();
                        DataTable dtCheck = dtpay.Copy();
                        dtCheck.TableName = "Check";
                        Check.Tables.Add(dtCheck);
                        Check.DataSetName = "Check";

                        report.RegData("Invoice", Invoice);
                        report.RegData("Check", Check);
                        report.RegData("ControlBranch", ControlBranch);
                        report.RegData("dsBank", BankAz);
                        report.RegData("dsAccount", AccountAz);
                        report.Render();
                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        buffer1 = stream.ToArray();
                        lstbyte.Add(buffer1);

                        if (totalRows > rowCount)
                        {
                            byte[] bufferNew = null;
                            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                            report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck1.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                            }
                            else
                            {
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                            }

                            report["InvoiceCount"] = totalRows;
                            Invoice = new DataSet();
                            dtInvoice = secondHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            Check = new DataSet();
                            dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.Render();
                            settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            service = new Stimulsoft.Report.Export.StiPdfExportService();
                            stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            bufferNew = stream.ToArray();

                            lstbyteNew.Add(bufferNew);
                        }
                    }

                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();
            }

            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=MidCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void CreateTableInvoice()
    {

        dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        dti.Columns.Add(new DataColumn("Total", typeof(string)));
        dti.Columns.Add(new DataColumn("Disc", typeof(string)));
        dti.Columns.Add(new DataColumn("AmountPay", typeof(string)));
        dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        dti.Columns.Add(new DataColumn("Description", typeof(string)));
    }
    private void CreateTablePayee()
    {
        dtpay.Columns.Add(new DataColumn("Pay", typeof(string)));
        dtpay.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        dtpay.Columns.Add(new DataColumn("Date", typeof(string)));
        dtpay.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        dtpay.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("State", typeof(string)));
        dtpay.Columns.Add(new DataColumn("Zip", typeof(string)));
        dtpay.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("VendorAcct", typeof(string)));
    }
    private void CreateTableBank()
    {
        dtBank.Columns.Add(new DataColumn("Name", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Address", typeof(string)));
        dtBank.Columns.Add(new DataColumn("City", typeof(string)));
        dtBank.Columns.Add(new DataColumn("State", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Zip", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NBranch", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NAcct", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NRoute", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Ref", typeof(string)));
    }
    public string ConvertNumberToCurrency(double _amount)
    {
        string _currencyInWord = ConvertNumbertoWords(Convert.ToInt32(Math.Truncate(_amount)));
        double d = _amount - Math.Truncate(_amount);
        if (d > 0)
        {
            d = Math.Round(d * 100);
            _currencyInWord = _currencyInWord + " And " + d.ToString() + " / 100";
        }
        _currencyInWord = "*** " + _currencyInWord + "****************";
        return _currencyInWord;
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000000) + " Million ";
            number %= 1000000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            //var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            //var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };


            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    protected void rdocheck_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (rdocheck.Checked == true)
            {
                Session["Journal_Type"] = "1";
                RadGrid_Checks.Rebind();
                //BindJournalGrid();
                lnkProcess.Visible = false;
                //lblHeader.Text = "Journal Entries";
                //--------------Start: By Juily - 19-12-2019----------------------//
                //lnkVoidCheck.Style["display"] = "block";
                //lnkPrint.Style["display"] = "block";
                //lnkEditCheckNum.Style["display"] = "block";
                //btnReprintRange.Style["display"] = "block";
                lnkVoidCheck.Visible = true;
                lnkPrint.Visible = true;
                lnkEditCheckNum.Visible = true;
                btnReprintRange.Visible = true;
                btnchecknobill.Visible = true;
                //--------------End: By Juily - 19-12-2019----------------------//
            }
            else
            {
                Session["Journal_Type"] = "2";
                RadGrid_Checks.DataSource = new string[] { };
                RadGrid_Checks.DataBind();
                //lblRecordCount.Text = "0 Record(s) found.";
                //--------------Start: By Juily - 19-12-2019----------------------//
                //lnkVoidCheck.Style["display"] = "none";
                //lnkPrint.Style["display"] = "none";
                //lnkEditCheckNum.Style["display"] = "none";
                //btnReprintRange.Style["display"] = "none";
                lnkVoidCheck.Visible = false;
                lnkPrint.Visible = false;
                lnkEditCheckNum.Visible = false;
                btnReprintRange.Visible = false;
                //--------------End: By Juily - 19-12-2019----------------------//
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void rdoRecurring_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (rdoRecurring.Checked == true)
            {
                Session["Journal_Type"] = "2";
                RadGrid_Checks.Rebind();
                lnkProcess.Visible = true;
                //--------------Start: By Juily - 19-12-2019----------------------//   
                //lnkVoidCheck.Style["display"] = "none";
                //lnkPrint.Style["display"] = "none";
                //lnkEditCheckNum.Style["display"] = "none";
                //btnReprintRange.Style["display"] = "none";
                //btnchecknobill.Style["display"] = "none";
                lnkVoidCheck.Visible = false;
                lnkPrint.Visible = false;
                lnkEditCheckNum.Visible = false;
                btnReprintRange.Visible = false;
                btnchecknobill.Visible = false;
                //--------------End: By Juily - 19-12-2019----------------------//
            }
            else
            {
                Session["Journal_Type"] = "1";
                RadGrid_Checks.DataSource = new string[] { };
                RadGrid_Checks.DataBind();
                //--------------Start: By Juily - 19-12-2019----------------------//
                //lnkVoidCheck.Style["display"] = "block";
                //lnkPrint.Style["display"] = "block";
                //lnkEditCheckNum.Style["display"] = "block";
                //btnReprintRange.Style["display"] = "block";
                //btnchecknobill.Style["display"] = "block";
                lnkVoidCheck.Visible = true;
                lnkPrint.Visible = true;
                lnkEditCheckNum.Visible = true;
                btnReprintRange.Visible = true;
                btnchecknobill.Visible = true;
                //--------------End: By Juily - 19-12-2019----------------------//
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void BindRecurringGrid()
    {

        try
        {
            txtFromDate.Text = Session["from_Checks"].ToString();
            txtToDate.Text = Session["end_Checks"].ToString();
            if (txtToDate.Text == "")
            {
                txtFromDate.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
                txtToDate.Text = DateTime.Now.AddYears(100).ToShortDateString();
            }
            if (IsValidDate())
            {
                _objCD.ConnConfig = Session["config"].ToString();
                _objCD.StartDate = Convert.ToDateTime(txtFromDate.Text);
                _objCD.EndDate = Convert.ToDateTime(txtToDate.Text);
                _objCD.searchterm = ddlSearch.SelectedValue;
                _objCD.UserID = Convert.ToInt32(Session["UserID"].ToString());

                _getCheckRecurrDetails.ConnConfig = Session["config"].ToString();
                _getCheckRecurrDetails.StartDate = Convert.ToDateTime(txtFromDate.Text);
                _getCheckRecurrDetails.EndDate = Convert.ToDateTime(txtToDate.Text);
                _getCheckRecurrDetails.searchterm = ddlSearch.SelectedValue;
                _getCheckRecurrDetails.UserID = Convert.ToInt32(Session["UserID"].ToString());

                if (ddlSearch.SelectedValue == "Status")
                {
                    _objCD.searchvalue = ddlStatus.SelectedValue;
                    _getCheckRecurrDetails.searchvalue = ddlStatus.SelectedValue;
                }
                else if (ddlSearch.SelectedValue == "PayType")
                {
                    _objCD.searchvalue = ddlPaytype.SelectedValue;
                    _getCheckRecurrDetails.searchvalue = ddlPaytype.SelectedValue;

                }
                else
                {
                    _objCD.searchvalue = txtSearch.Text;
                    _getCheckRecurrDetails.searchvalue = txtSearch.Text;
                }
                if (Session["CmpChkDefault"].ToString() == "1")
                {
                    _objCD.EN = 1;
                    _getCheckRecurrDetails.EN = 1;
                }
                else
                {
                    _objCD.EN = 0;
                    _getCheckRecurrDetails.EN = 0;
                }
                _objCD.PageNumber = RadGrid_Checks.CurrentPageIndex + 1;
                _objCD.PageSize = RadGrid_Checks.PageSize;

                _getCheckRecurrDetails.PageNumber = RadGrid_Checks.CurrentPageIndex + 1;
                _getCheckRecurrDetails.PageSize = RadGrid_Checks.PageSize;

                DataSet _dsCheck = new DataSet();

                List <CDRecurrViewModel> _lstCDRecurrViewModel = new List<CDRecurrViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetCheckRecurrDetails";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckRecurrDetails);

                    _lstCDRecurrViewModel = (new JavaScriptSerializer()).Deserialize<List<CDRecurrViewModel>>(_APIResponse.ResponseData);
                    _dsCheck = CommonMethods.ToDataSet<CDRecurrViewModel>(_lstCDRecurrViewModel);
                }
                else
                {
                    _dsCheck = _objBLBill.GetCheckRecurrDetails(_objCD);
                }

                Session["Checks"] = _dsCheck.Tables[0];
                int totalRow = 0;
                if (_dsCheck.Tables[0].Rows.Count > 0)
                    totalRow = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["TotalRow"]);
                RadGrid_Checks.VirtualItemCount = totalRow;
                //RadGrid_Checks.VirtualItemCount = _dsCheck.Tables[0].Rows.Count;
                RadGrid_Checks.DataSource = _dsCheck;
                lblRecordCount.Text = totalRow.ToString() + " Record(s) found";

            }
            txtFromDate.Text = Session["from_Checks"].ToString();
            txtToDate.Text = Session["end_Checks"].ToString();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void BindRecurringGridShowAll()
    {

        try
        {

            //if (IsValidDate())
            //{
                string stdate = "1900-01-01 00:00:00";
                string enddate = DateTime.Now.ToShortDateString() + " 23:59:59";

                _objCD.ConnConfig = Session["config"].ToString();
                _objCD.StartDate = Convert.ToDateTime(stdate);
                _objCD.EndDate = Convert.ToDateTime(enddate);
                _objCD.searchterm = ddlSearch.SelectedValue;
                _objCD.UserID = Convert.ToInt32(Session["UserID"].ToString());

                _getCheckRecurrDetails.ConnConfig = Session["config"].ToString();
                _getCheckRecurrDetails.StartDate = Convert.ToDateTime(stdate);
                _getCheckRecurrDetails.EndDate = Convert.ToDateTime(enddate);
                _getCheckRecurrDetails.searchterm = ddlSearch.SelectedValue;
                _getCheckRecurrDetails.UserID = Convert.ToInt32(Session["UserID"].ToString());

                if (ddlSearch.SelectedValue == "Status")
                {
                    _objCD.searchvalue = ddlStatus.SelectedValue;
                    _getCheckRecurrDetails.searchvalue = ddlStatus.SelectedValue;
                }
                else if (ddlSearch.SelectedValue == "PayType")
                {
                    _objCD.searchvalue = ddlPaytype.SelectedValue;
                    _getCheckRecurrDetails.searchvalue = ddlPaytype.SelectedValue;

                }
                else
                {
                    _objCD.searchvalue = txtSearch.Text;
                    _getCheckRecurrDetails.searchvalue = txtSearch.Text;
                }
                if (Session["CmpChkDefault"].ToString() == "1")
                {
                    _objCD.EN = 1;
                    _getCheckRecurrDetails.EN = 1;
                }
                else
                {
                    _objCD.EN = 0;
                    _getCheckRecurrDetails.EN = 0;
                }
                _objCD.PageNumber = RadGrid_Checks.CurrentPageIndex + 1;
                _objCD.PageSize = RadGrid_Checks.PageSize;

                _getCheckRecurrDetails.PageNumber = RadGrid_Checks.CurrentPageIndex + 1;
                _getCheckRecurrDetails.PageSize = RadGrid_Checks.PageSize;

                DataSet _dsCheck = new DataSet();

                List<CDRecurrViewModel> _lstCDRecurrViewModel = new List<CDRecurrViewModel>();
                string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetCheckRecurrDetails";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckRecurrDetails);

                    _lstCDRecurrViewModel = (new JavaScriptSerializer()).Deserialize<List<CDRecurrViewModel>>(_APIResponse.ResponseData);
                    _dsCheck = CommonMethods.ToDataSet<CDRecurrViewModel>(_lstCDRecurrViewModel);
                }
                else
                {
                    _dsCheck = _objBLBill.GetCheckRecurrDetails(_objCD);
                }

                Session["Checks"] = _dsCheck.Tables[0];
                int totalRow = 0;
                if (_dsCheck.Tables[0].Rows.Count > 0)
                    totalRow = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["TotalRow"]);
                RadGrid_Checks.VirtualItemCount = totalRow;
            lblRecordCount.Text = totalRow.ToString() + " Record(s) found";
            //RadGrid_Checks.VirtualItemCount = _dsCheck.Tables[0].Rows.Count;
            RadGrid_Checks.DataSource = _dsCheck;

            //}


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkProcess_Click(object sender, EventArgs e)
    {
        try
        {
            int RecurCount = 0;
            foreach (GridDataItem gr in RadGrid_Checks.SelectedItems)
            {
                //CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                _objCD.ConnConfig = Session["config"].ToString();
                _objCD.ID = Convert.ToInt32(hdnID.Value);
                _objCD.MOMUSer = Session["Username"].ToString();

                _processRecurCheck.ConnConfig = Session["config"].ToString();
                _processRecurCheck.ID = Convert.ToInt32(hdnID.Value);
                _processRecurCheck.MOMUSer = Session["Username"].ToString();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_ProcessRecurCheck";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _processRecurCheck);
                    RecurCount = Convert.ToInt32(_APIResponse.ResponseData);
                }
                else
                {
                    RecurCount = _objBLBill.ProcessRecurCheck(_objCD);
                }
            }

            Button mpbtnNotifyRecur = this.Master.FindControl("btnNotifyRecur") as Button;
            if (mpbtnNotifyRecur != null)
            {
                mpbtnNotifyRecur.Text = RecurCount.ToString();
            }
            //BindRecurringGrid();
            RadGrid_Checks.Rebind();
            ScriptManager.RegisterStartupScript(this, GetType(), "keySuccUp", "noty({text: 'Successfully entry processed!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    protected void RadGrid_PayChecks_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_PayChecks.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["PayChecks_FilterExpression"] != null && Convert.ToString(Session["PayChecks_FilterExpression"]) != "" && Session["PayChecks_Filters"] != null)
                {
                    RadGrid_PayChecks.MasterTableView.FilterExpression = Convert.ToString(Session["PayChecks_FilterExpression"]);
                    var filtersGet = Session["PayChecks_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_PayChecks.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["PayChecks_FilterExpression"] = null;
                Session["PayChecks_Filters"] = null;
            }
        }
        #endregion
        BindPayChecks();



    }
    protected void RadGrid_PayChecks_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        //lblRecordCount.Text = rowCount + " Record(s) found";
        //updpnl.Update();
    }
    protected void RadGrid_PayChecks_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
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
        catch { }
    }
    protected void RadGrid_PayChecks_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_PayChecks.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["PayChecks_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_PayChecks.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }
            Session["PayChecks_Filters"] = filters;
        }
        else
        {
            Session["PayChecks_FilterExpression"] = null;
            Session["PayChecks_Filters"] = null;
        }

        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_PayChecks);
        #endregion  
        PayRowSelect();
    }
    private void PayRowSelect()
    {
        foreach (GridDataItem gr in RadGrid_PayChecks.Items)
        {
            HiddenField hdnID = (HiddenField)gr.FindControl("hdnIDPay");
            HyperLink lnkName = (HyperLink)gr.FindControl("lblRefPay");
            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='EmpCheckDetail.aspx?id=" + hdnID.Value + "&frm=MNG'";

        }
    }
    protected void RadGrid_PayChecks_ExcelMLExportStylesCreated(object sender, GridExportExcelMLStyleCreatedArgs e)
    {
        StyleElement datetime = new StyleElement("DateTimeStyle");
        datetime.NumberFormat.FormatType = NumberFormatType.ShortDate;
        e.Styles.Add(datetime);
        StyleElement numeric = new StyleElement("NumericStyle");
        numeric.NumberFormat.FormatType = NumberFormatType.Fixed;
        e.Styles.Add(numeric);
    }
    protected void RadGrid_PayChecks_ExcelMLExportRowCreated(object sender, Telerik.Web.UI.GridExcelBuilder.GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["PayCmpChkDefault"]) == "1")
            currentItem = 5;
        else
            currentItem = 6;
        if (e.Worksheet.Table.Rows.Count == RadGrid_PayChecks.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_PayChecks.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
            RowElement row = new RowElement(); //create new row for the footer aggregates
            for (int i = currentItem; i < footerItem.Cells.Count; i++)
            {
                TableCell fcell = footerItem.Cells[i];
                CellElement cell = new CellElement();
                // cell.Data.DataItem =  fcell.Text == "&nbsp;" ? "" : fcell.Text;
                if (i == currentItem)
                    cell.Data.DataItem = "Total:-";
                else
                    cell.Data.DataItem = fcell.Text == "&nbsp;" ? "" : fcell.Text;
                row.Cells.Add(cell);
            }
            e.Worksheet.Table.Rows.Add(row);

        }

        DataSet dsCDDataType = new DataSet();
        _objCD.ConnConfig = Session["config"].ToString();
        //_getDataTypeCD.ConnConfig = Session["config"].ToString();

        //List<CDViewModel> _lstCDViewModel = new List<CDViewModel>();

        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    string APINAME = "ManageChecksAPI/CheckList_GetDataTypeCD";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getDataTypeCD);

        //    _lstCDViewModel = (new JavaScriptSerializer()).Deserialize<List<CDViewModel>>(_APIResponse.ResponseData);
        //    dsCDDataType = CommonMethods.ToDataSet<CDViewModel>(_lstCDViewModel);
        //}
        //else
        //{
        dsCDDataType = _objBLBill.GetDataTypeCD(_objCD);
        //}

        var getDataType = "";
        foreach (DataRow dr in dsCDDataType.Tables[0].Rows)
        {
            getDataType = dr.ItemArray[1].ToString();
            if (getDataType == "datetime")
            {
                e.Row.Cells.GetCellByName(dr.ItemArray[0].ToString()).StyleValue = "DateTimeStyle";
            }
            else if (getDataType == "numeric")
            {
                e.Row.Cells.GetCellByName(dr.ItemArray[0].ToString()).StyleValue = "NumericStyle";
            }
        }
    }
    private void BindPayChecks()
    {

        try
        {

            txtFromDatePayCheck.Text = Session["PayCheckfromDate"].ToString();
            txtToDatePayCheck.Text = Session["PayCheckToDate"].ToString();
            if (txtToDatePayCheck.Text == "")
            {
                txtFromDatePayCheck.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
                txtToDatePayCheck.Text = DateTime.Now.AddYears(100).ToShortDateString();
            }

            DataSet _dsPRReg = new DataSet();
            _objPRReg.ConnConfig = Session["config"].ToString();

            if (string.IsNullOrEmpty(txtFromDatePayCheck.Text) && string.IsNullOrEmpty(txtToDatePayCheck.Text))
            {
                txtFromDatePayCheck.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtToDatePayCheck.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            string stdate = txtFromDatePayCheck.Text + " 00:00:00";
            string enddate = txtToDatePayCheck.Text + " 23:59:59";
            _objPRReg.StartDate = Convert.ToDateTime(stdate);
            _objPRReg.EndDate = Convert.ToDateTime(enddate);
            _objPRReg.EmpID = Convert.ToInt32(ddlEmp.SelectedValue);

            _objPRReg.PageSize = 0;
            _objPRReg.SortBy = "";
            _objPRReg.SortType = "";

            _dsPRReg = objBL_Wage.GetPayrollRegister(_objPRReg);
            Session["PayRegister"] = _dsPRReg.Tables[0];
            RadGrid_PayChecks.VirtualItemCount = _dsPRReg.Tables[0].Rows.Count;
            RadGrid_PayChecks.DataSource = _dsPRReg;


            txtFromDatePayCheck.Text = Session["PayCheckfromDate"].ToString();
            txtToDatePayCheck.Text = Session["PayCheckToDate"].ToString();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        
    }
    protected void lnkShowAllPayCheck_Click(object sender, EventArgs e)
    {

        txtFromDatePayCheck.Text = string.Empty;
        txtToDatePayCheck.Text = string.Empty;
        Session["PayCheckfromDate"] = txtFromDatePayCheck.Text;
        Session["PayCheckToDate"] = txtToDatePayCheck.Text;


        Session["PayCheckInClosed"] = "True";
        lnkChk.Checked = true;

        Session["PayCheck_Type"] = "1";
        BindPayChecks();

        RadGrid_PayChecks.Rebind();
    }
    protected void lnkClearPayCheck_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);
        txtFromDatePayCheck.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
        txtToDatePayCheck.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
        Session["PayCheckfromDate"] = txtFromDatePayCheck.Text;
        Session["PayCheckToDate"] = txtToDatePayCheck.Text;


        Session["PayCheck_Type"] = "1";
        BindPayChecks();

        foreach (GridColumn column in RadGrid_PayChecks.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_PayChecks.MasterTableView.FilterExpression = string.Empty;
        RadGrid_PayChecks.Rebind();


    }
    private void ResetFormControlValues(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                ResetFormControlValues(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        ((CheckBox)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                }
            }
        }


        //check = false;
        Session["PayCheckInClosed"] = "False";
        lnkChk.Checked = false;


    }
    private void BindEmployee()
    {
        try
        {

            DataSet ds = new DataSet();
            objProp_Emp.ConnConfig = Session["config"].ToString();

            ds = new BL_Wage().GetEmployeeList(objProp_Emp);
            if (ddlEmp.Items.Count > 0)
            {
                ddlEmp.Items.Clear();
            }
            ddlEmp.DataSource = ds.Tables[0];
            ddlEmp.DataTextField = "Name";
            ddlEmp.DataValueField = "ID";
            ddlEmp.DataBind();
            ddlEmp.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("All", "0")));

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    protected void lnkSearchPayCheck_Click(object sender, EventArgs e)
    {

        if (hdnCssActive.Value == "CssActive")
        {
            Session["PayCheckActive"] = "1";
        }
        else
        {
            Session["PayCheckActive"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }
        Session["PayCheckToDate"] = txtToDatePayCheck.Text;
        Session["PayCheckfromDate"] = txtFromDatePayCheck.Text;
        //BindCheckGrid();
        RadGrid_PayChecks.Rebind();


    }
    private void GetControlForPayroll()
    {
        User objPropUser = new User();
        BL_ReportsData objBL_Report = new BL_ReportsData();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());

        DataSet ds = new DataSet();
        ds = objBL_Report.GetControlForPayroll(objPropUser);
        bool PR = Convert.ToBoolean(DBNull.Value.Equals(ds.Tables[0].Rows[0]["PR"]) ? 0 : ds.Tables[0].Rows[0]["PR"]);

        bool PRUser = objBL_User.getPRUserByID(objPropUser);

        if (PR == true && PRUser == true)
        {
            liVendorCheck.Visible = true;
            liPayCheck.Visible = true;
        }
        else
        {
            if (PR == true && Session["User"].ToString() == "Maintenance")
            {
                liVendorCheck.Visible = true;
                liPayCheck.Visible = true;
            }
            else
            {
                liVendorCheckhead.Style.Add("display", "none");
                
                liPayCheckhead.Style.Add("display", "none");
                tabProject.Style.Add("visibility", "hidden");
                //tabProject.Visible = false;
            }

        }
    }

}
public class EditManageCheckModel
{
    public string ID { get; set; }
    public string Sel { get; set; }
    public string Ref { get; set; }
    public string VoidDate { get; set; }
    public string fDate { get; set; }
    public string Bank { get; set; }
    public String lblIndex { get; set; }
}
