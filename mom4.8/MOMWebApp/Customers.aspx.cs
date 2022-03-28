using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using QBFC12Lib;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Linq.Dynamic;
using BusinessEntity.APModels;
using BusinessEntity.Utility;
using MOMWebApp;
using BusinessEntity.Payroll;
using BusinessEntity.CustomersModel;
using BusinessEntity.payroll;
using System.Collections.Specialized;
using System.Web.Configuration;

public partial class NewCustomers : System.Web.UI.Page
{

    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    private bool booSessionBegun;
    public bool chkActive = false;

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    getCustomFieldsParam _getCustomFields = new getCustomFieldsParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetUserByIdParam _GetUserById = new GetUserByIdParam();
    GetCompanyByCustomerParam _GetCompanyByCustomer = new GetCompanyByCustomerParam();
    GetStockReportsParam _GetStockReports = new GetStockReportsParam();
    getCustomerTypeParam _getCustomerType = new getCustomerTypeParam();
    DeleteCustomerParam _DeleteCustomer = new DeleteCustomerParam();
    AddQBCustomerTypeParam _AddQBCustomerType = new AddQBCustomerTypeParam();
    GetMSMCustomertypeParam _GetMSMCustomertype = new GetMSMCustomertypeParam();
    UpdateQBcustomertypeIDParam _UpdateQBcustomertypeID = new UpdateQBcustomertypeIDParam();
    AddQBLocTypeParam _AddQBLocType = new AddQBLocTypeParam();
    GetMSMLoctypeParam _GetMSMLoctype = new GetMSMLoctypeParam();
    UpdateQBJobtypeIDParam _UpdateQBJobtypeID = new UpdateQBJobtypeIDParam();
    AddQBSalesTaxParam _AddQBSalesTax = new AddQBSalesTaxParam();
    GetMSMSalesTaxParam _GetMSMSalesTax = new GetMSMSalesTaxParam();
    UpdateQBsalestaxIDParam _UpdateQBsalestaxID = new UpdateQBsalestaxIDParam();
    AddCustomerQBParam _AddCustomerQB = new AddCustomerQBParam();
    AddQBLocationParam _AddQBLocation = new AddQBLocationParam();
    GetMSMCustomersParam _GetMSMCustomers = new GetMSMCustomersParam();
    UpdateQBCustomerIDParam _UpdateQBCustomerID = new UpdateQBCustomerIDParam();
    GetQBCustomersParam _GetQBCustomers = new GetQBCustomersParam();
    GetMSMLocationParam _GetMSMLocation = new GetMSMLocationParam();
    UpdateQBLocationIDParam _UpdateQBLocationID = new UpdateQBLocationIDParam();
    GetQBLocationParam _GetQBLocation = new GetQBLocationParam();
    UpdateQBLastSyncParam _UpdateQBLastSync = new UpdateQBLastSyncParam();
    GetCustomerSearchParam _GetCustomerSearch = new GetCustomerSearchParam();
    GetCustomersParam _GetCustomers = new GetCustomersParam();
    string ReportsUrl = WebConfigurationManager.AppSettings["CoreUrl"];
    private bool IsGridPageIndexChanged = false;

    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        Permission();
        if (!IsPostBack)
        {

            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            FillStatus();
            FillCustomerType();
            FillCompany();
            ConvertToJSON();
            DataSet dscstm = new DataSet();

            dscstm = GetCustomFields("Owner1");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                string strCustom1 = dscstm.Tables[0].Rows[0]["label"].ToString();
                ddlSearch.Items.FindByValue("o.Custom1").Text = strCustom1;
            }
            dscstm = GetCustomFields("Owner2");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                string strCustom2 = dscstm.Tables[0].Rows[0]["label"].ToString();
                ddlSearch.Items.FindByValue("o.Custom2").Text = strCustom2;
            }
            #region Show Selected Filter
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["lnkChk_estimate"] != null)
                {
                    lnkChk.Checked = Convert.ToBoolean(Session["lnkChk_estimate"]);
                }
                if (Session["ddlSearch_Cus"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearch_Cus"]);
                    ddlSearch.SelectedValue = selectedValue;

                    String searchValue = Convert.ToString(Session["ddlSearch_Value_Cus"]);
                    if (selectedValue == "o.Status")
                    {
                        rbStatus.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "o.type")
                    {
                        ddlUserType.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "B.Name")
                    {
                        ddlCompany.SelectedValue = searchValue;
                    }
                    else
                    {
                        txtSearch.Text = searchValue;
                    }

                    ShowHideFilter();
                }
                IsGridPageIndexChanged = true;
            }
            else
            {
                Session["ddlSearch_Cus"] = null;
                Session["ddlSearch_Value_Cus"] = null;
                Session["Customer_FilterExpression"] = null;
                Session["Customer_Filters"] = null;
                Session["Cus_TypeFilters"] = null;
                Session["lnkChk_estimate"] = null;
            }
            #endregion

            string user = Session["userid"].ToString();
        }

        //*******************REMEMBER TO UNCOMMENT THIS WHEN SIDEPANEL IS FINISHED**********************************//

        HighlightSideMenu("cstmMgr", "lnkCustomersSmenu", "cstmMgrSub");


        CompanyPermission();
    }


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

    private void ShowHideFilter()
    {
        div_txtSearch.Style.Add("display", "none");
        div_rbStatus.Style.Add("display", "none");
        div_ddlUserType.Style.Add("display", "none");
        div_ddlCompany.Style.Add("display", "none");


        if (ddlSearch.SelectedValue == "o.Status")
        {

            div_rbStatus.Style.Add("display", "block");
        }
        else if (ddlSearch.SelectedValue == "o.type")
        {
            div_ddlUserType.Style.Add("display", "block");
        }
        else if (ddlSearch.SelectedValue == "B.Name")
        {
            div_ddlCompany.Style.Add("display", "block");
        }
        else
        {
            div_txtSearch.Style.Add("display", "block");
        }
    }
    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objGeneral.CustomName = name;
        objGeneral.ConnConfig = Session["config"].ToString();

        _getCustomFields.CustomName = name;
        _getCustomFields.ConnConfig = Session["config"].ToString();
        DataSet dsCustom = new DataSet();
        List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/CustomersList_GetCustomFields";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstCustomViewModel = serializer.Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
        }
        else
        {
            ds = objBL_General.getCustomFields(objGeneral);
        }

        return ds;
    }
    private void ShowQBSyncControls()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        _getConnectionConfig.ConnConfig = Session["config"].ToString();

        List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/CustomersList_GetControl";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
        }
        else
        {
            ds = objBL_User.getControl(objProp_User);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["QBIntegration"].ToString() == "1")
            {
                lnkSyncQB.Visible = true;
            }
            else
            {
                lnkSyncQB.Visible = false;
            }
        }
    }

    private void Permission()
    {

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
            //Response.Redirect("newaddcustomer.aspx?uid=" + Session["userid"].ToString());
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            //pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            //ds = (DataTable)Session["userinfo"];
            ds = GetUserById();
            //OWNER
            string OwnerPermission = ds.Rows[0]["Owner"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Owner"].ToString();
            string ADD = hdnAddeOwner.Value = OwnerPermission.Length < 1 ? "Y" : OwnerPermission.Substring(0, 1);
            string Edit = hdnEditeOwner.Value = OwnerPermission.Length < 2 ? "Y" : OwnerPermission.Substring(1, 1);
            string Delete = hdnDeleteeOwner.Value = OwnerPermission.Length < 3 ? "Y" : OwnerPermission.Substring(2, 1);
            string View = hdnViewOwner.Value = OwnerPermission.Length < 4 ? "Y" : OwnerPermission.Substring(3, 1);

            if (ADD == "N")
            {

                lnkAddnew.Visible = false;
                btnCopy.Visible = false;
            }
            if (Edit == "N")
            {
                btnEdit.Visible = false;

            }
            if (Delete == "N")
            {
                btnDelete.Visible = false;

            }
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
        }

        //string ProgFunc=dt.Rows[0]["Control"].ToString().Substring(0,1);
        //if(ProgFunc=="N")
        //{
        //    Response.Redirect("home.aspx");
        //}
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_Customer.MasterTableView.Columns.FindByUniqueName("Company").Visible = true;
            ddlSearch.Items.FindByValue("B.Name").Enabled = true;
        }
        else
        {
            RadGrid_Customer.MasterTableView.Columns.FindByUniqueName("Company").Visible = false;
            ddlSearch.Items.FindByValue("B.Name").Enabled = false;
        }
        var isQBSyncIntegrated = IsQBSyncIntegrated();
        if (isQBSyncIntegrated)
        {
            RadGrid_Customer.MasterTableView.Columns.FindByUniqueName("ImageQB").Visible = true;
        }
        else
        {
            RadGrid_Customer.MasterTableView.Columns.FindByUniqueName("ImageQB").Visible = false;
        }

        var isSageSyncIntegrated = IsSageSyncIntegrated();
        if (isSageSyncIntegrated)
        {
            RadGrid_Customer.MasterTableView.Columns.FindByUniqueName("sageid").Visible = true;
        }
        else
        {
            RadGrid_Customer.MasterTableView.Columns.FindByUniqueName("sageid").Visible = false;
        }
    }

    private bool IsQBSyncIntegrated()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet dsLastSync = objBL_General.getQBlatsync(objGeneral);
        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["qbintegration"]);
        if (intintegration == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsSageSyncIntegrated()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
        if (intintegration == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }
    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();

        _GetCompanyByCustomer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetCompanyByCustomer.DBName = Session["dbname"].ToString();
        _GetCompanyByCustomer.ConnConfig = Session["config"].ToString();

        DataSet dc = new DataSet();

        List<CompanyOfficeViewModel> _lstCompanyOffice = new List<CompanyOfficeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/CustomersList_GetCompanyByCustomer";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCompanyByCustomer);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstCompanyOffice = serializer.Deserialize<List<CompanyOfficeViewModel>>(_APIResponse.ResponseData);
            dc = CommonMethods.ToDataSet<CompanyOfficeViewModel>(_lstCompanyOffice);
        }
        else
        {
            dc = objBL_Company.getCompanyByCustomer(objCompany);
        }

        if (dc.Tables.Count > 0)
        {
            ddlCompany.DataSource = dc.Tables[0];
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataBind();
        }
    }
    private List<CustomerReport> GetReportsName()
    {
        List<CustomerReport> lstCustomerReport = new List<CustomerReport>();
        try
        {
            DataSet dsGetReports = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objProp_User.Type = "Customer";

            _GetStockReports.DBName = Session["dbname"].ToString();
            _GetStockReports.ConnConfig = Session["config"].ToString();
            _GetStockReports.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _GetStockReports.Type = "Customer";

            List<CustomerReportViewModel> _lstCustomerReport = new List<CustomerReportViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetStockReports";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetStockReports);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstCustomerReport = serializer.Deserialize<List<CustomerReportViewModel>>(_APIResponse.ResponseData);
                dsGetReports = CommonMethods.ToDataSet<CustomerReportViewModel>(_lstCustomerReport);
            }
            else
            {
                dsGetReports = objBL_ReportsData.GetStockReports(objProp_User);
            }

            //if (dsGetReports.Tables.Count > 0)
            for (int i = 0; i <= dsGetReports.Tables[0].Rows.Count - 1; i++)
            {
                CustomerReport objCustomerReport = new CustomerReport();
                //drpReports.DataSource = dsGetReports.Tables[0];
                //drpReports.DataTextField = "ReportName";
                //drpReports.DataValueField = "Id";
                //drpReports.DataBind();

                //drpReports.Items.Insert(0, "Print");
                //drpReports.SelectedIndex = 0;

                objCustomerReport.ReportId = Convert.ToInt32(dsGetReports.Tables[0].Rows[i]["Id"]);
                objCustomerReport.ReportName = dsGetReports.Tables[0].Rows[i]["ReportName"].ToString();
                objCustomerReport.IsGlobal = Convert.ToBoolean(dsGetReports.Tables[0].Rows[i]["IsGlobal"]);

                lstCustomerReport.Add(objCustomerReport);
            }
        }
        catch (Exception ex)
        {
            //
        }
        return lstCustomerReport;
    }

    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportsName());
        string reports = "var reports=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", reports, true);
    }

    private void FillCustomerType()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        _getCustomerType.ConnConfig = Session["config"].ToString();

        List<GetCustomerTypeViewModel> _lstGetCustomerType = new List<GetCustomerTypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/CustomersList_GetCustomerType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomerType);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetCustomerType = serializer.Deserialize<List<GetCustomerTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetCustomerTypeViewModel>(_lstGetCustomerType);
        }
        else
        {
            ds = objBL_User.getCustomerType(objProp_User);
        }

        ddlUserType.DataSource = ds.Tables[0];
        ddlUserType.DataTextField = "Type";
        ddlUserType.DataValueField = "Type";
        ddlUserType.DataBind();
    }
    private void FillStatus()
    {
        rbStatus.Items.Clear();
        rbStatus.Items.Add(new ListItem("Active", "0"));
        rbStatus.Items.Add(new ListItem("Inactive", "1"));
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Customer.SelectedItems)
        {
            Label lblCustID = (Label)item.FindControl("lblId");
            Response.Redirect("addcustomer.aspx?uid=" + lblCustID.Text);
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Customer.SelectedItems)
        {
            Label lblCustID = (Label)item.FindControl("lblId");
            Response.Redirect("addcustomer.aspx?uid=" + lblCustID.Text + "&t=c");
        }

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Customer.SelectedItems)
        {
            Label lblCustID = (Label)item.FindControl("lblId");
            DeleteCustomer(Convert.ToInt32(lblCustID.Text));
        }

    }

    private void DeleteCustomer(int CustID)
    {
        objProp_User.CustomerID = CustID;
        objProp_User.ConnConfig = Session["config"].ToString();

        _DeleteCustomer.CustomerID = CustID;
        _DeleteCustomer.ConnConfig = Session["config"].ToString();

        try
        {
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_DeleteCustomer";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteCustomer);
            }
            else
            {
                objBL_User.DeleteCustomer(objProp_User);
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Customer deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            GetCustomerList();
            RadGrid_Customer.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addcustomer.aspx");
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);
        ddlSearch_SelectedIndexChanged(sender, e);
        // GetCustomerList();
        foreach (GridColumn column in RadGrid_Customer.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
            column.ListOfFilterValues = null;
        }
        Session["ddlSearch_Cus"] = null;
        Session["ddlSearch_Value_Cus"] = null;
        Session["Customer_FilterExpression"] = null;
        Session["Customer_Filters"] = null;
        Session["Cus_TypeFilters"] = null;
        Session["lnkChk_estimate"] = null;
        RadGrid_Customer.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Customer.CurrentPageIndex = 0;
        RadGrid_Customer.PageSize = 50;
        RadGrid_Customer.MasterTableView.PageSize = 50;
        RadGrid_Customer.MasterTableView.CurrentPageIndex = 0;
        RadGrid_Customer.Rebind();
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        #region Search Filter
        Session["lnkChk_estimate"] = lnkChk.Checked;
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_Cus"] = selectedValue;

        if (selectedValue == "o.Status")
        {
            Session["ddlSearch_Value_Cus"] = rbStatus.SelectedValue;
        }
        else if (selectedValue == "o.type")
        {
            Session["ddlSearch_Value_Cus"] = ddlUserType.SelectedValue;
        }
        else if (selectedValue == "B.Name")
        {
            Session["ddlSearch_Value_Cus"] = ddlCompany.SelectedValue;
        }
        else
        {
            Session["ddlSearch_Value_Cus"] = txtSearch.Text;
        }
        #endregion

        GetCustomerList();
        RadGrid_Customer.Rebind();
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }


    private string IsNull(string input, string replacement)
    {
        string output = string.Empty;

        if (!string.IsNullOrEmpty(input))
        {
            output = input;
        }
        else
        {
            output = replacement;
        }

        return output;
    }

    private string SuffixSpace(string input)
    {
        string output = string.Empty;

        if (input != string.Empty)
        {
            output = input + " ";
        }

        return output;
    }

    #region QB Sync

    public void QBCustomerSync()
    {
        #region Table Schema
        DataTable dt = new DataTable();
        dt.Columns.Add("ListID", typeof(string));
        dt.Columns.Add("CustomerName", typeof(string));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MainContact", typeof(string));
        dt.Columns.Add("Phone", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Cell", typeof(string));
        dt.Columns.Add("Address", typeof(string));
        dt.Columns.Add("City", typeof(string));
        dt.Columns.Add("State", typeof(string));
        dt.Columns.Add("Zip", typeof(string));
        dt.Columns.Add("IsJob", typeof(string));
        dt.Columns.Add("Fax", typeof(string));
        dt.Columns.Add("ParentCustID", typeof(string));
        dt.Columns.Add("BillAddress", typeof(string));
        dt.Columns.Add("BillCity", typeof(string));
        dt.Columns.Add("BillState", typeof(string));
        dt.Columns.Add("BillZip", typeof(string));
        dt.Columns.Add("LastUpdateDate", typeof(DateTime));
        dt.Columns.Add("Type", typeof(string));
        dt.Columns.Add("LocType", typeof(string));
        dt.Columns.Add("Status", typeof(bool));

        #endregion

        QBSessionManager sessionManager = null;
        booSessionBegun = false;

        try
        {
            #region Connection to QB

            string path = "";
            DataSet dsC = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();

            _getConnectionConfig.ConnConfig = Session["config"].ToString();

            List<GetControlViewModel> _lstGetControl = new List<GetControlViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetControl = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<GetControlViewModel>(_lstGetControl);
            }
            else
            {
                dsC = objBL_User.getControl(objProp_User);
            }

            if (dsC.Tables[0].Rows.Count > 0)
            {
                path = dsC.Tables[0].Rows[0]["QBPath"].ToString();
            }

            IMsgSetRequest requestMsgSet;
            IMsgSetResponse responseMsgSet;
            //QBSessionManager sessionManager = null;
            //bool booSessionBegun = false;            
            sessionManager = new QBSessionManager();
            sessionManager.CommunicateOutOfProcess(true);
            sessionManager.QBAuthPreferences.PutIsReadOnly(false);
            sessionManager.QBAuthPreferences.PutUnattendedModePref(ENUnattendedModePrefType.umptRequired);
            sessionManager.QBAuthPreferences.PutPersonalDataPref(ENPersonalDataPrefType.pdptRequired);
            sessionManager.OpenConnection2("", "Mobile Service Manager", ENConnectionType.ctLocalQBD);
            sessionManager.BeginSession(path, ENOpenMode.omDontCare);

            //"C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Company Files\\Elevator Refurbishing Corp.qbw"
            //sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2007\\IdeavateSol.qbw", ENOpenMode.omDontCare);

            if (sessionManager.QBAuthPreferences.WasAuthPreferencesObeyed() != true)
            {
                throw new Exception("Auth Not Obeyed!!");
            }
            booSessionBegun = true;
            requestMsgSet = getLatestMsgSetRequest(sessionManager);
            requestMsgSet.Attributes.OnError = ENRqOnError.roeStop;
            #endregion

            DateTime LastSycnDate = System.DateTime.MinValue;
            DataSet dsTime = new DataSet();
            //String FormatDate = string.Empty;
            objProp_User.ConnConfig = Session["config"].ToString(); ;

            List<GetControlViewModel> _lstgetControl = new List<GetControlViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstgetControl = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsTime = CommonMethods.ToDataSet<GetControlViewModel>(_lstgetControl);
            }
            else
            {
                dsTime = objBL_User.getControl(objProp_User);
            }

            if (dsTime.Tables[0].Rows[0]["QBLastSync"] != DBNull.Value)
            {
                LastSycnDate = Convert.ToDateTime(dsTime.Tables[0].Rows[0]["QBLastSync"]);
                //FormatDate = LastSycnDate.ToString("yyyy-MM-ddTHH:mm:ss");
            }

            #region Sync Customer type
            /// Import
            requestMsgSet.ClearRequests();
            ICustomerTypeQuery Custtype = requestMsgSet.AppendCustomerTypeQueryRq();
            if (LastSycnDate != System.DateTime.MinValue)
            {
                Custtype.ORListQuery.ListFilter.FromModifiedDate.SetValue(LastSycnDate, false);
            }
            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            IResponse responses = responseMsgSet.ResponseList.GetAt(0);
            ICustomerTypeRetList invoiceRets = responses.Detail as ICustomerTypeRetList;
            if (invoiceRets != null)
            {
                if (!(invoiceRets.Count == 0))
                {
                    int rowcount = invoiceRets.Count;
                    int fCount = 0;
                    for (int ndx = 0; ndx < rowcount; ndx++)
                    {
                        ICustomerTypeRet invoiceRet1 = invoiceRets.GetAt(ndx);

                        objProp_User.QBCustomerTypeID = invoiceRet1.ListID.GetValue();
                        objProp_User.CustomerType = objGeneralFunctions.Truncate(invoiceRet1.Name.GetValue(), 50);
                        objProp_User.Remarks = invoiceRet1.FullName.GetValue();
                        objProp_User.ConnConfig = Session["config"].ToString();

                        //API
                        _AddQBCustomerType.QBCustomerTypeID = invoiceRet1.ListID.GetValue();
                        _AddQBCustomerType.CustomerType = objGeneralFunctions.Truncate(invoiceRet1.Name.GetValue(), 50);
                        _AddQBCustomerType.Remarks = invoiceRet1.FullName.GetValue();
                        _AddQBCustomerType.ConnConfig = Session["config"].ToString();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "CustomersAPI/CustomersList_AddQBCustomerType";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddQBCustomerType);
                        }
                        else
                        {
                            objBL_User.AddQBCustomerType(objProp_User);
                        }

                    }
                }
            }

            /// Export
            objProp_User.ConnConfig = Session["config"].ToString();

            _GetMSMCustomertype.ConnConfig = Session["config"].ToString();

            DataSet dsCustomertype = new DataSet();

            List<GetMSMCustomertypeViewModel> _lstGetMSMCustomertype = new List<GetMSMCustomertypeViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetMSMCustomertype";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetMSMCustomertype);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetMSMCustomertype = serializer.Deserialize<List<GetMSMCustomertypeViewModel>>(_APIResponse.ResponseData);
                dsCustomertype = CommonMethods.ToDataSet<GetMSMCustomertypeViewModel>(_lstGetMSMCustomertype);
            }
            else
            {
                dsCustomertype = objBL_User.getMSMCustomertype(objProp_User);
            }

            foreach (DataRow dr in dsCustomertype.Tables[0].Rows)
            {
                requestMsgSet.ClearRequests();
                ICustomerTypeAdd CustomertypeReq = requestMsgSet.AppendCustomerTypeAddRq();
                CustomertypeReq.Name.SetValue(objGeneralFunctions.Truncate(dr["type"].ToString(), 31));

                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                if (thisResponse.StatusCode == 0)
                {
                    ICustomerTypeRet customertype = (ICustomerTypeRet)thisResponse.Detail;
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.QBCustomerTypeID = customertype.ListID.GetValue();
                    objProp_User.CustomerType = dr["type"].ToString();

                    //API
                    _UpdateQBcustomertypeID.ConnConfig = Session["config"].ToString();
                    _UpdateQBcustomertypeID.QBCustomerTypeID = customertype.ListID.GetValue();
                    _UpdateQBcustomertypeID.CustomerType = dr["type"].ToString();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "CustomersAPI/CustomersList_UpdateQBcustomertypeID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateQBcustomertypeID);
                    }
                    else
                    {
                        objBL_User.UpdateQBcustomertypeID(objProp_User);
                    }
                }
            }

            #endregion

            #region Sync Location Type
            /// Import
            requestMsgSet.ClearRequests();
            IJobTypeQuery LocType = requestMsgSet.AppendJobTypeQueryRq();
            if (LastSycnDate != System.DateTime.MinValue)
            {
                LocType.ORListQuery.ListFilter.FromModifiedDate.SetValue(LastSycnDate, false);
            }
            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            IResponse responseJobType = responseMsgSet.ResponseList.GetAt(0);
            IJobTypeRetList invoiceRetJobType = responseJobType.Detail as IJobTypeRetList;
            if (invoiceRetJobType != null)
            {
                if (!(invoiceRetJobType.Count == 0))
                {
                    int rowcount = invoiceRetJobType.Count;
                    int fCount = 0;
                    for (int ndx = 0; ndx < rowcount; ndx++)
                    {
                        IJobTypeRet invoiceRet1 = invoiceRetJobType.GetAt(ndx);

                        objProp_User.QBCustomerTypeID = invoiceRet1.ListID.GetValue();
                        objProp_User.CustomerType = objGeneralFunctions.Truncate(invoiceRet1.Name.GetValue(), 50);
                        objProp_User.Remarks = invoiceRet1.FullName.GetValue();
                        objProp_User.ConnConfig = Session["config"].ToString();

                        //API
                        _AddQBLocType.QBCustomerTypeID = invoiceRet1.ListID.GetValue();
                        _AddQBLocType.CustomerType = objGeneralFunctions.Truncate(invoiceRet1.Name.GetValue(), 50);
                        _AddQBLocType.Remarks = invoiceRet1.FullName.GetValue();
                        _AddQBLocType.ConnConfig = Session["config"].ToString();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "CustomersAPI/CustomersList_AddQBLocType";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddQBLocType);
                        }
                        else
                        {
                            objBL_User.AddQBLocType(objProp_User);
                        }
                    }
                }
            }

            /// Export
            objProp_User.ConnConfig = Session["config"].ToString();
            _GetMSMCustomertype.ConnConfig = Session["config"].ToString();
            DataSet dsJobtype = new DataSet();

            List<GetMSMLoctypeViewModel> _lstGetMSMLoctype = new List<GetMSMLoctypeViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetMSMLoctype";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetMSMCustomertype);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetMSMLoctype = serializer.Deserialize<List<GetMSMLoctypeViewModel>>(_APIResponse.ResponseData);
                dsJobtype = CommonMethods.ToDataSet<GetMSMLoctypeViewModel>(_lstGetMSMLoctype);
            }
            else
            {
                dsJobtype = objBL_User.getMSMLoctype(objProp_User);
            }

            foreach (DataRow dr in dsJobtype.Tables[0].Rows)
            {
                requestMsgSet.ClearRequests();
                IJobTypeAdd JobtypeReq = requestMsgSet.AppendJobTypeAddRq();
                JobtypeReq.Name.SetValue(objGeneralFunctions.Truncate(dr["type"].ToString(), 31));

                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                if (thisResponse.StatusCode == 0)
                {
                    IJobTypeRet jobtype = (IJobTypeRet)thisResponse.Detail;
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.QBCustomerTypeID = jobtype.ListID.GetValue();
                    objProp_User.CustomerType = dr["type"].ToString();

                    //API
                    _UpdateQBJobtypeID.ConnConfig = Session["config"].ToString();
                    _UpdateQBJobtypeID.QBCustomerTypeID = jobtype.ListID.GetValue();
                    _UpdateQBJobtypeID.CustomerType = dr["type"].ToString();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "CustomersAPI/CustomersList_UpdateQBJobtypeID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateQBJobtypeID);
                    }
                    else
                    {
                        objBL_User.UpdateQBJobtypeID(objProp_User);
                    }
                }
            }

            #endregion

            #region Sync Sales Tax
            /// Import
            requestMsgSet.ClearRequests();
            ISalesTaxCodeQuery SalesTax = requestMsgSet.AppendSalesTaxCodeQueryRq();
            if (LastSycnDate != System.DateTime.MinValue)
            {
                SalesTax.ORListQuery.ListFilter.FromModifiedDate.SetValue(LastSycnDate, false);
            }
            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            IResponse responseSalesTax = responseMsgSet.ResponseList.GetAt(0);
            ISalesTaxCodeRetList SalesTaxRet = responseSalesTax.Detail as ISalesTaxCodeRetList;
            if (SalesTaxRet != null)
            {
                if (!(SalesTaxRet.Count == 0))
                {
                    int rowcount = SalesTaxRet.Count;
                    int fCount = 0;
                    for (int ndx = 0; ndx < rowcount; ndx++)
                    {
                        ISalesTaxCodeRet invoiceRet1 = SalesTaxRet.GetAt(ndx);

                        objProp_User.QBSalesTaxID = invoiceRet1.ListID.GetValue();
                        objProp_User.ConnConfig = Session["config"].ToString();
                        objProp_User.SalesTax = objGeneralFunctions.Truncate(invoiceRet1.Name.GetValue(), 25);
                        objProp_User.SalesDescription = objGeneralFunctions.Truncate(invoiceRet1.Desc.GetValue(), 75);
                        objProp_User.SalesRate = 0;
                        objProp_User.State = "";
                        objProp_User.Remarks = "";
                        objProp_User.IsTaxable = Convert.ToInt32(invoiceRet1.IsTaxable.GetValue());

                        //API
                        _AddQBSalesTax.QBSalesTaxID = invoiceRet1.ListID.GetValue();
                        _AddQBSalesTax.ConnConfig = Session["config"].ToString();
                        _AddQBSalesTax.SalesTax = objGeneralFunctions.Truncate(invoiceRet1.Name.GetValue(), 25);
                        _AddQBSalesTax.SalesDescription = objGeneralFunctions.Truncate(invoiceRet1.Desc.GetValue(), 75);
                        _AddQBSalesTax.SalesRate = 0;
                        _AddQBSalesTax.State = "";
                        _AddQBSalesTax.Remarks = "";
                        _AddQBSalesTax.IsTaxable = Convert.ToInt32(invoiceRet1.IsTaxable.GetValue());

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "CustomersAPI/CustomersList_AddQBSalesTax";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddQBSalesTax);
                        }
                        else
                        {
                            objBL_User.AddQBSalesTax(objProp_User);
                        }
                    }
                }
            }

            /// Export
            objProp_User.ConnConfig = Session["config"].ToString();
            _GetMSMSalesTax.ConnConfig = Session["config"].ToString();

            DataSet ds = new DataSet();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetMSMSalesTax";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetMSMSalesTax);
            }
            else
            {
                ds = objBL_User.getMSMSalesTax(objProp_User);
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                requestMsgSet.ClearRequests();
                ISalesTaxCodeAdd SalestaxReq = requestMsgSet.AppendSalesTaxCodeAddRq();
                SalestaxReq.Name.SetValue(objGeneralFunctions.Truncate(dr["name"].ToString(), 3));
                SalestaxReq.Desc.SetValue(objGeneralFunctions.Truncate(dr["fdesc"].ToString(), 31));
                SalestaxReq.IsTaxable.SetValue(Convert.ToBoolean(dr["IStax"].ToString()));

                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                if (thisResponse.StatusCode == 0)
                {
                    ISalesTaxCodeRet salestax = (ISalesTaxCodeRet)thisResponse.Detail;
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.QBSalesTaxID = salestax.ListID.GetValue();
                    objProp_User.SalesTax = dr["name"].ToString();

                    //API
                    _UpdateQBsalestaxID.ConnConfig = Session["config"].ToString();
                    _UpdateQBsalestaxID.QBSalesTaxID = salestax.ListID.GetValue();
                    _UpdateQBsalestaxID.SalesTax = dr["name"].ToString();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "CustomersAPI/CustomersList_UpdateQBsalestaxID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateQBsalestaxID);
                    }
                    else
                    {
                        objBL_User.UpdateQBsalestaxID(objProp_User);
                    }
                }
            }
            ////DataSet dsQBSalestax = new DataSet();
            ////dsQBSalestax = objBL_User.getQBSalesTax(objProp_User);
            ////foreach (DataRow dr in dsQBSalestax.Tables[0].Rows)
            ////{
            ////    requestMsgSet.ClearRequests();
            ////    ISalesTaxCodeQuery salestaxQ = requestMsgSet.AppendSalesTaxCodeQueryRq();
            ////    salestaxQ.ORListQuery.ListIDList.Add(dr["QBstaxID"].ToString());
            ////    responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            ////    IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
            ////    ISalesTaxCodeRetList salestaxRetList = thisResponse.Detail as ISalesTaxCodeRetList;
            ////    if (salestaxRetList != null)
            ////    {
            ////        if (!(salestaxRetList.Count == 0))
            ////        {
            ////            ISalesTaxCodeRet salestaxRet = salestaxRetList.GetAt(0);
            ////            string editSequence = salestaxRet.EditSequence.GetValue();
            ////            DateTime lastUpdateDateQB = salestaxRet.TimeModified.GetValue();

            ////            if (dr["LastUpdateDate"] != DBNull.Value)
            ////            {
            ////                if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
            ////                {
            ////                    requestMsgSet.ClearRequests();
            ////                    ISalesTaxCodeMod staxcodeMod = requestMsgSet.AppendSalesTaxCodeModRq();
            ////                    staxcodeMod.ListID.SetValue(dr["QBstaxID"].ToString());
            ////                    staxcodeMod.EditSequence.SetValue(editSequence);
            ////                    staxcodeMod.Name.SetValue(dr["name"].ToString());

            ////                    responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            ////                    IResponse thisResponse1 = responseMsgSet.ResponseList.GetAt(0);
            ////                }
            ////            }
            ////        }
            ////        else
            ////        {

            ////        }
            ////    }
            ////}
            #endregion

            #region Import data from QuickBooks

            requestMsgSet.ClearRequests();
            ICustomerQuery invoiceAdd = requestMsgSet.AppendCustomerQueryRq();
            //invoiceAdd.ORCustomerListQuery.CustomerListFilter.ActiveStatus.SetValue(ENActiveStatus.asActiveOnly);
            if (LastSycnDate != System.DateTime.MinValue)
            {
                invoiceAdd.ORCustomerListQuery.CustomerListFilter.FromModifiedDate.SetValue(LastSycnDate, false);
            }
            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
            IResponse response = responseMsgSet.ResponseList.GetAt(0);
            ICustomerRetList invoiceRet = response.Detail as ICustomerRetList;
            if (invoiceRet != null)
            {
                if (!(invoiceRet.Count == 0))
                {
                    int rowcount = invoiceRet.Count;
                    int fCount = 0;
                    for (int ndx = 0; ndx < rowcount; ndx++)
                    {
                        ICustomerRet invoiceRet1 = invoiceRet.GetAt(ndx);

                        DataRow dr = dt.NewRow();

                        if (invoiceRet1.IsActive != null)
                            if (invoiceRet1.IsActive.GetValue() == true)
                                dr["Status"] = false;
                            else
                                dr["Status"] = true;
                        else
                            dr["Status"] = 0;

                        if (invoiceRet1.TimeModified != null && invoiceRet1.TimeModified.ToString() != "")
                            dr["LastUpdateDate"] = invoiceRet1.TimeModified.GetValue();
                        else
                            dr["LastUpdateDate"] = string.Empty;

                        if (invoiceRet1.Name != null && invoiceRet1.Name.ToString() != "")
                            dr["CustomerName"] = invoiceRet1.Name.GetValue();
                        else
                            dr["CustomerName"] = string.Empty;

                        if (dr["CustomerName"] == string.Empty)
                        {
                            if (invoiceRet1.CompanyName != null && invoiceRet1.CompanyName.ToString() != "")
                                dr["CustomerName"] = invoiceRet1.CompanyName.GetValue();
                            else
                                dr["CustomerName"] = string.Empty;
                        }

                        //if (invoiceRet1.Notes != null && invoiceRet1.Notes.ToString() != "")
                        //    dr["Remarks"] = invoiceRet1.Notes.GetValue();
                        //else
                        //    dr["Remarks"] = string.Empty;

                        //if (invoiceRet1.Contact != null && invoiceRet1.Contact.ToString() != "")
                        //{
                        //    dr["MainContact"] = invoiceRet1.Contact.GetValue();
                        //}
                        //else
                        //{
                        //    dr["MainContact"] = string.Empty;
                        //}
                        string FirstName = string.Empty;
                        string MiddleName = string.Empty;
                        string LastName = string.Empty;
                        if (invoiceRet1.FirstName != null && invoiceRet1.FirstName.ToString() != "")
                        {
                            FirstName = invoiceRet1.FirstName.GetValue();
                        }
                        if (invoiceRet1.MiddleName != null && invoiceRet1.MiddleName.ToString() != "")
                        {
                            MiddleName = invoiceRet1.MiddleName.GetValue();
                        }
                        if (invoiceRet1.LastName != null && invoiceRet1.LastName.ToString() != "")
                        {
                            LastName = invoiceRet1.LastName.GetValue();
                        }

                        if (MiddleName.Trim() == string.Empty)
                        {
                            dr["MainContact"] = FirstName.Trim() + " " + LastName.Trim();
                        }
                        else
                        {
                            dr["MainContact"] = FirstName.Trim() + " " + MiddleName.Trim() + " " + LastName.Trim();
                        }


                        if (invoiceRet1.Phone != null && invoiceRet1.Phone.ToString() != "")
                            dr["Phone"] = invoiceRet1.Phone.GetValue();
                        else
                            dr["Phone"] = string.Empty;

                        if (invoiceRet1.Email != null && invoiceRet1.Email.ToString() != "")
                            dr["Email"] = invoiceRet1.Email.GetValue();
                        else
                            dr["Email"] = string.Empty;

                        //if (invoiceRet1.Mobile != null && invoiceRet1.Mobile.ToString() != "")
                        //    dr["Cell"] = invoiceRet1.Mobile.GetValue();
                        //else
                        //    dr["Cell"] = string.Empty;

                        if (invoiceRet1.ListID != null && invoiceRet1.ListID.ToString() != "")
                            dr["ListID"] = invoiceRet1.ListID.GetValue();
                        else
                            dr["ListID"] = string.Empty;

                        if (invoiceRet1.Sublevel != null && invoiceRet1.Sublevel.ToString() != "")
                            dr["IsJob"] = invoiceRet1.Sublevel.GetValue();
                        else
                            dr["IsJob"] = string.Empty;

                        if (invoiceRet1.Fax != null && invoiceRet1.Fax.ToString() != "")
                            dr["Fax"] = invoiceRet1.Fax.GetValue();
                        else
                            dr["Fax"] = string.Empty;

                        if (invoiceRet1.CustomerTypeRef != null)
                        {
                            if (invoiceRet1.CustomerTypeRef.FullName != null)
                                dr["type"] = invoiceRet1.CustomerTypeRef.FullName.GetValue();
                            else
                                dr["type"] = string.Empty;
                        }

                        if (invoiceRet1.JobTypeRef != null)
                        {
                            if (invoiceRet1.JobTypeRef.FullName != null)
                                dr["loctype"] = invoiceRet1.JobTypeRef.FullName.GetValue();
                            else
                                dr["loctype"] = string.Empty;
                        }

                        if (invoiceRet1.ParentRef != null)
                            dr["ParentCustID"] = invoiceRet1.ParentRef.ListID.GetValue();
                        else
                            dr["ParentCustID"] = string.Empty;


                        if (invoiceRet1.BillAddress != null)
                        {
                            string BillAdd1 = string.Empty;
                            string BillAdd2 = string.Empty;
                            string BillAdd3 = string.Empty;
                            string BillAdd4 = string.Empty;
                            string BillAdd5 = string.Empty;
                            string BillCity = string.Empty;
                            string BillState = string.Empty;
                            string BillZip = string.Empty;
                            string BillNotes = string.Empty;

                            if (invoiceRet1.BillAddress.Addr1 != null)
                                BillAdd1 = invoiceRet1.BillAddress.Addr1.GetValue();

                            if (invoiceRet1.BillAddress.Addr2 != null)
                                BillAdd2 = invoiceRet1.BillAddress.Addr2.GetValue();

                            if (invoiceRet1.BillAddress.Addr3 != null)
                                BillAdd3 = invoiceRet1.BillAddress.Addr3.GetValue();

                            //if (invoiceRet1.BillAddress.Addr4 != null)
                            //    BillAdd4 = invoiceRet1.BillAddress.Addr4.GetValue();

                            //if (invoiceRet1.BillAddress.Addr5 != null)
                            //    BillAdd5 = invoiceRet1.BillAddress.Addr5.GetValue();

                            if (invoiceRet1.BillAddress.City != null)
                                BillCity = invoiceRet1.BillAddress.City.GetValue();

                            if (invoiceRet1.BillAddress.State != null)
                                BillState = invoiceRet1.BillAddress.State.GetValue();

                            if (invoiceRet1.BillAddress.PostalCode != null)
                                BillZip = invoiceRet1.BillAddress.PostalCode.GetValue();

                            string BillAddress = SuffixSpace(BillAdd1) + Environment.NewLine + SuffixSpace(BillAdd2) + Environment.NewLine + SuffixSpace(BillAdd3);

                            //if (invoiceRet1.BillAddress.Note != null)
                            //    BillNotes = invoiceRet1.BillAddress.Note.GetValue();

                            dr["BillAddress"] = BillAddress.Trim();
                            dr["BillCity"] = BillCity.Trim();
                            dr["BillState"] = BillState.Trim();
                            dr["BillZip"] = BillZip.Trim();
                            dr["Remarks"] = BillNotes.Trim();
                        }

                        if (invoiceRet1.ShipAddress != null)
                        {
                            string ShipAdd1 = string.Empty;
                            string ShipAdd2 = string.Empty;
                            string ShipAdd3 = string.Empty;
                            string ShipAdd4 = string.Empty;
                            string ShipAdd5 = string.Empty;
                            string ShipCity = string.Empty;
                            string ShipState = string.Empty;
                            string ShipZip = string.Empty;
                            string ShipNotes = string.Empty;

                            if (invoiceRet1.ShipAddress.Addr1 != null)
                                ShipAdd1 = invoiceRet1.ShipAddress.Addr1.GetValue();

                            if (invoiceRet1.ShipAddress.Addr2 != null)
                                ShipAdd2 = invoiceRet1.ShipAddress.Addr2.GetValue();

                            if (invoiceRet1.ShipAddress.Addr3 != null)
                                ShipAdd3 = invoiceRet1.ShipAddress.Addr3.GetValue();

                            //if (invoiceRet1.ShipAddress.Addr4 != null)
                            //    ShipAdd4 = invoiceRet1.ShipAddress.Addr4.GetValue();

                            //if (invoiceRet1.ShipAddress.Addr5 != null)
                            //    ShipAdd5 = invoiceRet1.ShipAddress.Addr5.GetValue();

                            if (invoiceRet1.ShipAddress.City != null)
                                ShipCity = invoiceRet1.ShipAddress.City.GetValue();

                            if (invoiceRet1.ShipAddress.State != null)
                                ShipState = invoiceRet1.ShipAddress.State.GetValue();

                            if (invoiceRet1.ShipAddress.PostalCode != null)
                                ShipZip = invoiceRet1.ShipAddress.PostalCode.GetValue();

                            string ShipAddress = SuffixSpace(ShipAdd1) + Environment.NewLine + SuffixSpace(ShipAdd2) + Environment.NewLine + SuffixSpace(ShipAdd3);

                            //if (invoiceRet1.ShipAddress.Note != null)
                            //    ShipNotes = invoiceRet1.ShipAddress.Note.GetValue();

                            dr["Address"] = ShipAddress.Trim();
                            dr["City"] = ShipCity.Trim();
                            dr["State"] = ShipState.Trim();
                            dr["Zip"] = ShipZip.Trim();
                            if (ShipNotes.Trim() != string.Empty)
                            {
                                dr["Remarks"] = ShipNotes.Trim();
                            }
                        }

                        if (dr["Address"].ToString().Trim() == string.Empty)
                        {
                            dr["Address"] = dr["BillAddress"];
                            dr["City"] = dr["BillCity"];
                            dr["State"] = dr["BillState"];
                            dr["Zip"] = dr["BillZip"];
                        }

                        if (dr["Address"].ToString().Trim() == string.Empty)
                        {
                            dr["Address"] = dr["CustomerName"].ToString();
                        }

                        dt.Rows.Add(dr);

                        fCount++;
                    }

                    #region Import Customers from QB

                    var query = from row in dt.AsEnumerable()
                                where row.Field<string>("IsJob").Equals("0")
                                select row;

                    DataTable dtnew = dt.Clone();
                    foreach (var record in query)
                    {
                        DataRow drRow = dtnew.NewRow();
                        drRow = record;

                        dtnew.ImportRow(drRow);
                    }

                    foreach (DataRow dr in dtnew.Rows)
                    {
                        objProp_User.FirstName = dr["CustomerName"].ToString();
                        objProp_User.Remarks = dr["Remarks"].ToString();
                        objProp_User.MainContact = dr["MainContact"].ToString();
                        objProp_User.Phone = dr["Phone"].ToString();
                        objProp_User.Email = dr["Email"].ToString();
                        objProp_User.Cell = dr["Cell"].ToString();
                        objProp_User.QBCustomerID = dr["ListID"].ToString();
                        objProp_User.Address = dr["Address"].ToString();
                        objProp_User.City = dr["City"].ToString();
                        objProp_User.State = dr["State"].ToString();
                        objProp_User.Zip = dr["Zip"].ToString();
                        objProp_User.LastUpdateDate = Convert.ToDateTime(dr["LastUpdateDate"]);
                        objProp_User.Status = Convert.ToInt16(dr["Status"]);

                        objProp_User.Username = "";
                        objProp_User.Password = "";
                        objProp_User.Website = "";
                        objProp_User.Type = dr["type"].ToString();
                        objProp_User.Schedule = 0;
                        objProp_User.Mapping = 0;
                        objProp_User.Internet = 0;
                        objProp_User.ConnConfig = Session["config"].ToString();

                        //API
                        _AddCustomerQB.FirstName = dr["CustomerName"].ToString();
                        _AddCustomerQB.Remarks = dr["Remarks"].ToString();
                        _AddCustomerQB.MainContact = dr["MainContact"].ToString();
                        _AddCustomerQB.Phone = dr["Phone"].ToString();
                        _AddCustomerQB.Email = dr["Email"].ToString();
                        _AddCustomerQB.Cell = dr["Cell"].ToString();
                        _AddCustomerQB.QBCustomerID = dr["ListID"].ToString();
                        _AddCustomerQB.Address = dr["Address"].ToString();
                        _AddCustomerQB.City = dr["City"].ToString();
                        _AddCustomerQB.State = dr["State"].ToString();
                        _AddCustomerQB.Zip = dr["Zip"].ToString();
                        _AddCustomerQB.LastUpdateDate = Convert.ToDateTime(dr["LastUpdateDate"]);
                        _AddCustomerQB.Status = Convert.ToInt16(dr["Status"]);

                        _AddCustomerQB.Username = "";
                        _AddCustomerQB.Password = "";
                        _AddCustomerQB.Website = "";
                        _AddCustomerQB.Type = dr["type"].ToString();
                        _AddCustomerQB.Schedule = 0;
                        _AddCustomerQB.Mapping = 0;
                        _AddCustomerQB.Internet = 0;
                        _AddCustomerQB.ConnConfig = Session["config"].ToString();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "CustomersAPI/CustomersList_AddCustomerQB";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddCustomerQB);
                        }
                        else
                        {
                            objBL_User.AddCustomerQB(objProp_User);
                        }
                    }
                    #endregion


                    #region Import Locations from QB
                    var queryLocation = from row in dt.AsEnumerable()
                                        where row.Field<string>("IsJob").Equals("1")
                                        select row;

                    DataTable dtnewLoc = dt.Clone();
                    foreach (var record in queryLocation)
                    {
                        DataRow drRow = dtnewLoc.NewRow();
                        drRow = record;

                        dtnewLoc.ImportRow(drRow);
                    }

                    DataTable dtLoc = dtnewLoc.Clone();
                    foreach (DataRow drrow in dtnew.Rows)
                    {
                        int isHavingJob = 0;
                        foreach (DataRow drrow2 in dtnewLoc.Rows)
                        {
                            if (drrow["ListID"].ToString() == drrow2["ParentCustID"].ToString())
                            {
                                isHavingJob = 1;
                            }
                        }
                        if (isHavingJob == 0)
                        {
                            drrow["ParentCustID"] = drrow["listid"];
                            dtLoc.ImportRow(drrow);
                        }
                    }
                    foreach (DataRow dr in dtLoc.Rows)
                    {
                        dtnewLoc.ImportRow(dr);
                    }

                    foreach (DataRow dr in dtnewLoc.Rows)
                    {
                        objProp_User.AccountNo = dr["CustomerName"].ToString();
                        objProp_User.Locationname = dr["CustomerName"].ToString();
                        objProp_User.Address = dr["Address"].ToString();
                        objProp_User.Status = Convert.ToInt16(dr["Status"]);
                        objProp_User.City = dr["City"].ToString();
                        objProp_User.State = dr["State"].ToString();
                        objProp_User.Zip = dr["Zip"].ToString();
                        objProp_User.Remarks = dr["Remarks"].ToString();
                        objProp_User.MainContact = dr["MainContact"].ToString();
                        objProp_User.Phone = dr["Phone"].ToString();
                        objProp_User.Fax = dr["Fax"].ToString();
                        objProp_User.Cell = dr["Cell"].ToString();
                        objProp_User.Email = dr["Email"].ToString();
                        objProp_User.RolAddress = dr["BillAddress"].ToString();
                        objProp_User.RolCity = dr["BillCity"].ToString();
                        objProp_User.RolState = dr["BillState"].ToString();
                        objProp_User.RolZip = dr["BillZip"].ToString();
                        objProp_User.QBlocationID = dr["ListID"].ToString();
                        objProp_User.QBCustomerID = dr["ParentCustID"].ToString();
                        objProp_User.LastUpdateDate = Convert.ToDateTime(dr["LastUpdateDate"]);
                        objProp_User.Type = dr["loctype"].ToString();
                        objProp_User.ConnConfig = Session["config"].ToString();

                        //API
                        _AddQBLocation.AccountNo = dr["CustomerName"].ToString();
                        _AddQBLocation.Locationname = dr["CustomerName"].ToString();
                        _AddQBLocation.Address = dr["Address"].ToString();
                        _AddQBLocation.Status = Convert.ToInt16(dr["Status"]);
                        _AddQBLocation.City = dr["City"].ToString();
                        _AddQBLocation.State = dr["State"].ToString();
                        _AddQBLocation.Zip = dr["Zip"].ToString();
                        _AddQBLocation.Remarks = dr["Remarks"].ToString();
                        _AddQBLocation.MainContact = dr["MainContact"].ToString();
                        _AddQBLocation.Phone = dr["Phone"].ToString();
                        _AddQBLocation.Fax = dr["Fax"].ToString();
                        _AddQBLocation.Cell = dr["Cell"].ToString();
                        _AddQBLocation.Email = dr["Email"].ToString();
                        _AddQBLocation.RolAddress = dr["BillAddress"].ToString();
                        _AddQBLocation.RolCity = dr["BillCity"].ToString();
                        _AddQBLocation.RolState = dr["BillState"].ToString();
                        _AddQBLocation.RolZip = dr["BillZip"].ToString();
                        _AddQBLocation.QBlocationID = dr["ListID"].ToString();
                        _AddQBLocation.QBCustomerID = dr["ParentCustID"].ToString();
                        _AddQBLocation.LastUpdateDate = Convert.ToDateTime(dr["LastUpdateDate"]);
                        _AddQBLocation.Type = dr["loctype"].ToString();
                        _AddQBLocation.ConnConfig = Session["config"].ToString();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "CustomersAPI/CustomersList_AddQBLocation";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddQBLocation);
                        }
                        else
                        {
                            objBL_User.AddQBLocation(objProp_User);
                        }

                    }
                    #endregion
                }
            }

            #endregion

            #region Export data to Quickbooks

            #region Export/Add customers to QB
            objProp_User.ConnConfig = Session["config"].ToString();

            //API
            _GetMSMCustomers.ConnConfig = Session["config"].ToString();

            _GetQBCustomers.ConnConfig = Session["config"].ToString();
            DataSet dsSalestax = new DataSet();

            List<GetMSMCustomersViewModel> _lstGetMSMCustomers = new List<GetMSMCustomersViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetMSMCustomers";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetMSMCustomers);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetMSMCustomers = serializer.Deserialize<List<GetMSMCustomersViewModel>>(_APIResponse.ResponseData);
                dsSalestax = CommonMethods.ToDataSet<GetMSMCustomersViewModel>(_lstGetMSMCustomers);
            }
            else
            {
                dsSalestax = objBL_User.getMSMCustomers(objProp_User);
            }

            foreach (DataRow dr in dsSalestax.Tables[0].Rows)
            {
                bool active;
                if (dr["Status"].ToString() == "1")
                {
                    active = false;
                }
                else
                {
                    active = true;
                }

                string firstname = string.Empty;
                string middlename = string.Empty;
                string lastname = string.Empty;
                if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                {
                    string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int contactLength = contact.Count();

                    if (contactLength > 0)
                    {
                        firstname = contact[0].Trim();
                    }
                    if (contactLength == 2)
                    {
                        lastname = contact[1].Trim();
                    }
                    if (contactLength > 2)
                    {
                        middlename = contact[1].Trim();
                        lastname = contact[2].Trim();
                    }
                }

                string[] strAddress = dr["address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int intAddCount = strAddress.Count();
                string addr1 = string.Empty;
                string addr2 = string.Empty;
                string addr3 = string.Empty;
                if (intAddCount > 0)
                {
                    addr1 = objGeneralFunctions.Truncate(strAddress[0].Trim(), 41);
                }
                if (intAddCount > 1)
                {
                    addr2 = objGeneralFunctions.Truncate(strAddress[1].Trim(), 41);
                }
                if (intAddCount > 2)
                {
                    if (!string.IsNullOrEmpty(strAddress[2].Trim()))
                    {
                        addr3 = strAddress[2].Trim();
                    }
                }
                if (intAddCount > 3)
                {
                    for (int i = 3; i < strAddress.Count(); i++)
                    {
                        addr3 += " " + strAddress[i].Trim();
                    }
                }
                addr3 = objGeneralFunctions.Truncate(addr3, 41);

                requestMsgSet.ClearRequests();
                ICustomerAdd customerReq = requestMsgSet.AppendCustomerAddRq();
                customerReq.CompanyName.SetValue(objGeneralFunctions.Truncate(dr["name"].ToString(), 41));
                customerReq.Name.SetValue(objGeneralFunctions.Truncate(dr["name"].ToString(), 41));
                customerReq.IsActive.SetValue(active);
                //customerReq.Notes.SetValue(dr["remarks"].ToString());
                //customerReq.Contact.SetValue(dr["contact"].ToString());
                customerReq.FirstName.SetValue(objGeneralFunctions.Truncate(firstname, 25));
                customerReq.MiddleName.SetValue(objGeneralFunctions.Truncate(middlename, 5));
                customerReq.LastName.SetValue(objGeneralFunctions.Truncate(lastname, 25));
                customerReq.ShipAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                customerReq.ShipAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                customerReq.ShipAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                customerReq.ShipAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                customerReq.ShipAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                customerReq.ShipAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                //customerReq.ShipAddress.Note.SetValue(dr["remarks"].ToString());
                customerReq.BillAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                customerReq.BillAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                customerReq.BillAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                customerReq.BillAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                customerReq.BillAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                customerReq.BillAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                customerReq.Phone.SetValue(objGeneralFunctions.Truncate(dr["Phone"].ToString(), 21));
                customerReq.Email.SetValue(objGeneralFunctions.Truncate(dr["Email"].ToString(), 99));
                customerReq.Fax.SetValue(objGeneralFunctions.Truncate(dr["Fax"].ToString(), 21));
                customerReq.CustomerTypeRef.ListID.SetValue(dr["QBCustomertypeID"].ToString());

                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                if (thisResponse.StatusCode == 0)
                {
                    ICustomerRet customer = (ICustomerRet)thisResponse.Detail;
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.QBCustomerID = customer.ListID.GetValue();
                    objProp_User.CustomerID = Convert.ToInt32(dr["id"]);

                    //API
                    _UpdateQBCustomerID.ConnConfig = Session["config"].ToString();
                    _UpdateQBCustomerID.QBCustomerID = customer.ListID.GetValue();
                    _UpdateQBCustomerID.CustomerID = Convert.ToInt32(dr["id"]);



                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "CustomersAPI/CustomersList_UpdateQBCustomerID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateQBCustomerID);
                    }
                    else
                    {
                        objBL_User.UpdateQBCustomerID(objProp_User);
                    }

                }
            }
            #endregion

            #region Export/Update Customers to QB
            DataSet dsQB = new DataSet();

            List<GetQBCustomersViewModel> _lstGetQBCustomers = new List<GetQBCustomersViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetQBCustomers";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetQBCustomers);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetQBCustomers = serializer.Deserialize<List<GetQBCustomersViewModel>>(_APIResponse.ResponseData);
                dsQB = CommonMethods.ToDataSet<GetQBCustomersViewModel>(_lstGetQBCustomers);
            }
            else
            {
                dsQB = objBL_User.getQBCustomers(objProp_User);
            }

            foreach (DataRow dr in dsQB.Tables[0].Rows)
            {
                requestMsgSet.ClearRequests();
                ICustomerQuery CustQ = requestMsgSet.AppendCustomerQueryRq();
                CustQ.ORCustomerListQuery.ListIDList.Add(dr["QBCustomerID"].ToString());
                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                ICustomerRetList customerRetList = thisResponse.Detail as ICustomerRetList;
                if (customerRetList != null)
                {
                    if (!(customerRetList.Count == 0))
                    {
                        ICustomerRet customerRet = customerRetList.GetAt(0);
                        string editSequence = customerRet.EditSequence.GetValue();
                        DateTime lastUpdateDateQB = customerRet.TimeModified.GetValue();
                        bool active;
                        if (dr["Status"].ToString() == "1")
                        {
                            active = false;
                        }
                        else
                        {
                            active = true;
                        }
                        string firstname = string.Empty;
                        string middlename = string.Empty;
                        string lastname = string.Empty;
                        if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                        {
                            string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            int contactLength = contact.Count();

                            if (contactLength > 0)
                            {
                                firstname = contact[0].Trim();
                            }
                            if (contactLength == 2)
                            {
                                lastname = contact[1].Trim();
                            }
                            if (contactLength > 2)
                            {
                                middlename = contact[1].Trim();
                                lastname = contact[2].Trim();
                            }
                        }
                        string[] strAddress = dr["address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        int intAddCount = strAddress.Count();
                        string addr1 = string.Empty;
                        string addr2 = string.Empty;
                        string addr3 = string.Empty;
                        if (intAddCount > 0)
                        {
                            addr1 = objGeneralFunctions.Truncate(strAddress[0].Trim(), 41);
                        }
                        if (intAddCount > 1)
                        {
                            addr2 = objGeneralFunctions.Truncate(strAddress[1].Trim(), 41);
                        }
                        if (intAddCount > 2)
                        {
                            if (!string.IsNullOrEmpty(strAddress[2].Trim()))
                            {
                                addr3 = strAddress[2].Trim();
                            }
                        }
                        if (intAddCount > 3)
                        {
                            for (int i = 3; i < strAddress.Count(); i++)
                            {
                                addr3 += " " + strAddress[i].Trim();
                            }
                        }
                        addr3 = objGeneralFunctions.Truncate(addr3, 41);

                        if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
                        {
                            requestMsgSet.ClearRequests();
                            ICustomerMod CustMod = requestMsgSet.AppendCustomerModRq();
                            CustMod.ListID.SetValue(dr["QBCustomerID"].ToString());
                            CustMod.EditSequence.SetValue(editSequence);
                            CustMod.CompanyName.SetValue(objGeneralFunctions.Truncate(dr["name"].ToString(), 41));
                            CustMod.Name.SetValue(objGeneralFunctions.Truncate(dr["name"].ToString(), 41));
                            CustMod.IsActive.SetValue(active);
                            //CustMod.Notes.SetValue(dr["remarks"].ToString());
                            //CustMod.Contact.SetValue(dr["contact"].ToString());
                            CustMod.FirstName.SetValue(objGeneralFunctions.Truncate(firstname, 25));
                            CustMod.MiddleName.SetValue(objGeneralFunctions.Truncate(middlename, 5));
                            CustMod.LastName.SetValue(objGeneralFunctions.Truncate(lastname, 25));
                            CustMod.ShipAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                            CustMod.ShipAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                            CustMod.ShipAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                            CustMod.ShipAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                            CustMod.ShipAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                            CustMod.ShipAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                            //CustMod.ShipAddress.Note.SetValue(dr["remarks"].ToString());
                            CustMod.BillAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                            CustMod.BillAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                            CustMod.BillAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                            CustMod.BillAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                            CustMod.BillAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                            CustMod.BillAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                            CustMod.Phone.SetValue(objGeneralFunctions.Truncate(dr["Phone"].ToString(), 21));
                            CustMod.Email.SetValue(objGeneralFunctions.Truncate(dr["Email"].ToString(), 99));
                            CustMod.Fax.SetValue(objGeneralFunctions.Truncate(dr["Fax"].ToString(), 21));
                            CustMod.CustomerTypeRef.ListID.SetValue(dr["QBCustomertypeID"].ToString());

                            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                            IResponse thisResponse1 = responseMsgSet.ResponseList.GetAt(0);
                        }
                    }
                    else
                    {

                    }
                }
            }
            #endregion

            #region Export/Add Location to QB
            objProp_User.ConnConfig = Session["config"].ToString();

            _GetMSMLocation.ConnConfig = Session["config"].ToString();

            _GetQBLocation.ConnConfig = Session["config"].ToString();

            DataSet dsLoc = new DataSet();

            List<GetMSMLocationViewModel> _lstGetMSMLocation = new List<GetMSMLocationViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetMSMLocation";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetMSMLocation);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetMSMLocation = serializer.Deserialize<List<GetMSMLocationViewModel>>(_APIResponse.ResponseData);
                dsLoc = CommonMethods.ToDataSet<GetMSMLocationViewModel>(_lstGetMSMLocation);
            }
            else
            {
                dsLoc = objBL_User.getMSMLocation(objProp_User);
            }

            foreach (DataRow dr in dsLoc.Tables[0].Rows)
            {
                bool active;
                if (dr["Status"].ToString() == "1")
                {
                    active = false;
                }
                else
                {
                    active = true;
                }
                string firstname = string.Empty;
                string middlename = string.Empty;
                string lastname = string.Empty;
                if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                {
                    string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int contactLength = contact.Count();

                    if (contactLength > 0)
                    {
                        firstname = contact[0].Trim();
                    }
                    if (contactLength == 2)
                    {
                        lastname = contact[1].Trim();
                    }
                    if (contactLength > 2)
                    {
                        middlename = contact[1].Trim();
                        lastname = contact[2].Trim();
                    }
                }

                string[] strAddress = dr["address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int intAddCount = strAddress.Count();
                string addr1 = string.Empty;
                string addr2 = string.Empty;
                string addr3 = string.Empty;
                if (intAddCount > 0)
                {
                    addr1 = objGeneralFunctions.Truncate(strAddress[0].Trim(), 41);
                }
                if (intAddCount > 1)
                {
                    addr2 = objGeneralFunctions.Truncate(strAddress[1].Trim(), 41);
                }
                if (intAddCount > 2)
                {
                    if (!string.IsNullOrEmpty(strAddress[2].Trim()))
                    {
                        addr3 = strAddress[2].Trim();
                    }
                }
                if (intAddCount > 3)
                {
                    for (int i = 3; i < strAddress.Count(); i++)
                    {
                        addr3 += " " + strAddress[i].Trim();
                    }
                }
                addr3 = objGeneralFunctions.Truncate(addr3, 41);

                string[] strShipAddress = dr["ShipAddress"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int intShipAddCount = strShipAddress.Count();
                string Shipaddr1 = string.Empty;
                string Shipaddr2 = string.Empty;
                string Shipaddr3 = string.Empty;
                if (intShipAddCount > 0)
                {
                    Shipaddr1 = objGeneralFunctions.Truncate(strShipAddress[0].Trim(), 41);
                }
                if (intShipAddCount > 1)
                {
                    Shipaddr2 = objGeneralFunctions.Truncate(strShipAddress[1].Trim(), 41);
                }
                if (intShipAddCount > 2)
                {
                    if (!string.IsNullOrEmpty(strShipAddress[2].Trim()))
                    {
                        Shipaddr3 = strShipAddress[2].Trim();
                    }
                }
                if (intShipAddCount > 3)
                {
                    for (int i = 3; i < strShipAddress.Count(); i++)
                    {
                        Shipaddr3 += " " + strShipAddress[i].Trim();
                    }
                }
                Shipaddr3 = objGeneralFunctions.Truncate(Shipaddr3, 41);

                requestMsgSet.ClearRequests();
                ICustomerAdd customerReq = requestMsgSet.AppendCustomerAddRq();
                customerReq.CompanyName.SetValue(objGeneralFunctions.Truncate(dr["tag"].ToString(), 41));
                customerReq.Name.SetValue(objGeneralFunctions.Truncate(dr["tag"].ToString(), 41));
                customerReq.IsActive.SetValue(active);
                customerReq.FirstName.SetValue(objGeneralFunctions.Truncate(firstname, 25));
                customerReq.MiddleName.SetValue(objGeneralFunctions.Truncate(middlename, 5));
                customerReq.LastName.SetValue(objGeneralFunctions.Truncate(lastname, 25));
                //customerReq.Notes.SetValue(dr["remarks"].ToString());
                //customerReq.Contact.SetValue(dr["contact"].ToString());
                customerReq.ShipAddress.Addr1.SetValue(objGeneralFunctions.Truncate(Shipaddr1, 41));
                customerReq.ShipAddress.Addr2.SetValue(objGeneralFunctions.Truncate(Shipaddr2, 41));
                customerReq.ShipAddress.Addr3.SetValue(objGeneralFunctions.Truncate(Shipaddr3, 41));
                customerReq.ShipAddress.City.SetValue(objGeneralFunctions.Truncate(dr["shipcity"].ToString(), 21));
                customerReq.ShipAddress.State.SetValue(objGeneralFunctions.Truncate(dr["shipstate"].ToString(), 21));
                customerReq.ShipAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["shipzip"].ToString(), 13));
                // customerReq.ShipAddress.Note.SetValue(dr["remarks"].ToString());
                customerReq.BillAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                customerReq.BillAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                customerReq.BillAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                customerReq.BillAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                customerReq.BillAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                customerReq.BillAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                customerReq.Phone.SetValue(objGeneralFunctions.Truncate(dr["Phone"].ToString(), 21));
                customerReq.Email.SetValue(objGeneralFunctions.Truncate(dr["Email"].ToString(), 99));
                customerReq.Fax.SetValue(objGeneralFunctions.Truncate(dr["Fax"].ToString(), 21));
                //customerReq.ParentRef.FullName.SetValue(dr["Name"].ToString());
                customerReq.ParentRef.ListID.SetValue(dr["qbcustomerid"].ToString());
                customerReq.JobTypeRef.ListID.SetValue(dr["QBlocTypeID"].ToString());

                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                if (thisResponse.StatusCode == 0)
                {
                    ICustomerRet customer = (ICustomerRet)thisResponse.Detail;
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.QBlocationID = customer.ListID.GetValue();
                    objProp_User.LocID = Convert.ToInt32(dr["id"]);

                    //API
                    _UpdateQBLocationID.ConnConfig = Session["config"].ToString();
                    _UpdateQBLocationID.QBlocationID = customer.ListID.GetValue();
                    _UpdateQBLocationID.LocID = Convert.ToInt32(dr["id"]);

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "CustomersAPI/CustomersList_UpdateQBLocationID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateQBLocationID);
                    }
                    else
                    {
                        objBL_User.UpdateQBLocationID(objProp_User);
                    }
                }
            }
            #endregion

            #region Export/Update Location to QB
            DataSet dsQBLoc = new DataSet();

            List<GetQBLocationViewModel> _lstGetQBLocation = new List<GetQBLocationViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetQBLocation";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetQBLocation);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetQBLocation = serializer.Deserialize<List<GetQBLocationViewModel>>(_APIResponse.ResponseData);
                dsQBLoc = CommonMethods.ToDataSet<GetQBLocationViewModel>(_lstGetQBLocation);
            }
            else
            {
                dsQBLoc = objBL_User.getQBLocation(objProp_User);
            }

            foreach (DataRow dr in dsQBLoc.Tables[0].Rows)
            {
                requestMsgSet.ClearRequests();
                ICustomerQuery CustQ = requestMsgSet.AppendCustomerQueryRq();
                CustQ.ORCustomerListQuery.ListIDList.Add(dr["QBlocID"].ToString());
                responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                IResponse thisResponse = responseMsgSet.ResponseList.GetAt(0);
                ICustomerRetList customerRetList = thisResponse.Detail as ICustomerRetList;
                if (customerRetList != null)
                {
                    if (!(customerRetList.Count == 0))
                    {
                        ICustomerRet customerRet = customerRetList.GetAt(0);
                        string editSequence = customerRet.EditSequence.GetValue();
                        DateTime lastUpdateDateQB = customerRet.TimeModified.GetValue();

                        bool active;
                        if (dr["Status"].ToString() == "1")
                        {
                            active = false;
                        }
                        else
                        {
                            active = true;
                        }
                        string firstname = string.Empty;
                        string middlename = string.Empty;
                        string lastname = string.Empty;
                        if (!string.IsNullOrEmpty(dr["Contact"].ToString().Trim()))
                        {
                            string[] contact = dr["Contact"].ToString().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            int contactLength = contact.Count();

                            if (contactLength > 0)
                            {
                                firstname = contact[0].Trim();
                            }
                            if (contactLength == 2)
                            {
                                lastname = contact[1].Trim();
                            }
                            if (contactLength > 2)
                            {
                                middlename = contact[1].Trim();
                                lastname = contact[2].Trim();
                            }
                        }

                        string[] strAddress = dr["address"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        int intAddCount = strAddress.Count();
                        string addr1 = string.Empty;
                        string addr2 = string.Empty;
                        string addr3 = string.Empty;
                        if (intAddCount > 0)
                        {
                            addr1 = objGeneralFunctions.Truncate(strAddress[0].Trim(), 41);
                        }
                        if (intAddCount > 1)
                        {
                            addr2 = objGeneralFunctions.Truncate(strAddress[1].Trim(), 41);
                        }
                        if (intAddCount > 2)
                        {
                            if (!string.IsNullOrEmpty(strAddress[2].Trim()))
                            {
                                addr3 = strAddress[2].Trim();
                            }
                        }
                        if (intAddCount > 3)
                        {
                            for (int i = 3; i < strAddress.Count(); i++)
                            {
                                addr3 += " " + strAddress[i].Trim();
                            }
                        }
                        addr3 = objGeneralFunctions.Truncate(addr3, 41);

                        string[] strShipAddress = dr["ShipAddress"].ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        int intShipAddCount = strShipAddress.Count();
                        string Shipaddr1 = string.Empty;
                        string Shipaddr2 = string.Empty;
                        string Shipaddr3 = string.Empty;
                        if (intShipAddCount > 0)
                        {
                            Shipaddr1 = objGeneralFunctions.Truncate(strShipAddress[0].Trim(), 41);
                        }
                        if (intShipAddCount > 1)
                        {
                            Shipaddr2 = objGeneralFunctions.Truncate(strShipAddress[1].Trim(), 41);
                        }
                        if (intShipAddCount > 2)
                        {
                            if (!string.IsNullOrEmpty(strShipAddress[2].Trim()))
                            {
                                Shipaddr3 = strShipAddress[2].Trim();
                            }
                        }
                        if (intShipAddCount > 3)
                        {
                            for (int i = 3; i < strShipAddress.Count(); i++)
                            {
                                Shipaddr3 += " " + strShipAddress[i].Trim();
                            }
                        }
                        Shipaddr3 = objGeneralFunctions.Truncate(Shipaddr3, 41);

                        if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
                        {
                            requestMsgSet.ClearRequests();
                            ICustomerMod CustMod = requestMsgSet.AppendCustomerModRq();
                            CustMod.ListID.SetValue(dr["QBlocID"].ToString());
                            CustMod.EditSequence.SetValue(editSequence);
                            CustMod.CompanyName.SetValue(objGeneralFunctions.Truncate(dr["tag"].ToString(), 41));
                            CustMod.Name.SetValue(objGeneralFunctions.Truncate(dr["tag"].ToString(), 41));
                            CustMod.IsActive.SetValue(active);
                            CustMod.FirstName.SetValue(objGeneralFunctions.Truncate(firstname, 25));
                            CustMod.MiddleName.SetValue(objGeneralFunctions.Truncate(middlename, 5));
                            CustMod.LastName.SetValue(objGeneralFunctions.Truncate(lastname, 25));
                            //CustMod.Notes.SetValue(dr["remarks"].ToString());
                            //CustMod.Contact.SetValue(dr["contact"].ToString());
                            CustMod.ShipAddress.Addr1.SetValue(objGeneralFunctions.Truncate(Shipaddr1, 41));
                            CustMod.ShipAddress.Addr2.SetValue(objGeneralFunctions.Truncate(Shipaddr2, 41));
                            CustMod.ShipAddress.Addr3.SetValue(objGeneralFunctions.Truncate(Shipaddr3, 41));
                            CustMod.ShipAddress.City.SetValue(objGeneralFunctions.Truncate(dr["shipcity"].ToString(), 21));
                            CustMod.ShipAddress.State.SetValue(objGeneralFunctions.Truncate(dr["shipstate"].ToString(), 21));
                            CustMod.ShipAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["shipzip"].ToString(), 13));
                            // CustMod.ShipAddress.Note.SetValue(dr["remarks"].ToString());
                            CustMod.BillAddress.Addr1.SetValue(objGeneralFunctions.Truncate(addr1, 41));
                            CustMod.BillAddress.Addr2.SetValue(objGeneralFunctions.Truncate(addr2, 41));
                            CustMod.BillAddress.Addr3.SetValue(objGeneralFunctions.Truncate(addr3, 41));
                            CustMod.BillAddress.City.SetValue(objGeneralFunctions.Truncate(dr["city"].ToString(), 21));
                            CustMod.BillAddress.State.SetValue(objGeneralFunctions.Truncate(dr["State"].ToString(), 21));
                            CustMod.BillAddress.PostalCode.SetValue(objGeneralFunctions.Truncate(dr["zip"].ToString(), 13));
                            CustMod.Phone.SetValue(objGeneralFunctions.Truncate(dr["Phone"].ToString(), 21));
                            CustMod.Email.SetValue(objGeneralFunctions.Truncate(dr["Email"].ToString(), 99));
                            CustMod.Fax.SetValue(objGeneralFunctions.Truncate(dr["Fax"].ToString(), 21));
                            CustMod.JobTypeRef.ListID.SetValue(dr["QBlocTypeID"].ToString());
                            CustMod.ParentRef.ListID.SetValue(dr["qbcustomerid"].ToString());

                            responseMsgSet = sessionManager.DoRequests(requestMsgSet);
                            IResponse thisResponse1 = responseMsgSet.ResponseList.GetAt(0);
                        }
                    }
                    else
                    {

                    }
                }
            }
            #endregion

            #endregion

            objGeneral.ConnConfig = Session["config"].ToString();

            _UpdateQBLastSync.ConnConfig = Session["config"].ToString();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_UpdateQBLastSync";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateQBLastSync);
            }
            else
            {
                objBL_General.UpdateQBLastSync(objGeneral);
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Sync completed successfully!', dismissQueue: true, type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);//</br>Quickbooks not running. Quickbooks must be running with Admin account on server.
        }
        finally
        {
            #region Close QB Connection

            if (booSessionBegun)
            {
                sessionManager.EndSession();
            }
            booSessionBegun = false;
            sessionManager.CloseConnection();

            #endregion
        }
    }

    public IMsgSetRequest getLatestMsgSetRequest(QBSessionManager sessionManager)
    {
        // Find and adapt to supported version of QuickBooks
        double supportedVersion = QBFCLatestVersion(sessionManager);

        short qbXMLMajorVer = 0;
        short qbXMLMinorVer = 0;

        if (supportedVersion >= 6.0)
        {
            qbXMLMajorVer = 6;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 5.0)
        {
            qbXMLMajorVer = 5;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 4.0)
        {
            qbXMLMajorVer = 4;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 3.0)
        {
            qbXMLMajorVer = 3;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 2.0)
        {
            qbXMLMajorVer = 2;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 1.1)
        {
            qbXMLMajorVer = 1;
            qbXMLMinorVer = 1;
        }
        else
        {
            qbXMLMajorVer = 1;
            qbXMLMinorVer = 0;
            //Response.Write("It seems that you are running QuickBooks 2002 Release 1. We strongly recommend that you use QuickBooks' online update feature to obtain the latest fixes and enhancements");
        }

        // Create the message set request object
        IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", qbXMLMajorVer, qbXMLMinorVer);
        return requestMsgSet;
    }

    private double QBFCLatestVersion(QBSessionManager SessionManager)
    {
        // Use oldest version to ensure that this application work with any QuickBooks (US)
        IMsgSetRequest msgset = SessionManager.CreateMsgSetRequest("US", 1, 0);
        msgset.AppendHostQueryRq();
        IMsgSetResponse QueryResponse = SessionManager.DoRequests(msgset);


        IResponse response = QueryResponse.ResponseList.GetAt(0);

        // Please refer to QBFC Developers Guide for details on why 
        // "as" clause was used to link this derrived class to its base class
        IHostRet HostResponse = response.Detail as IHostRet;
        IBSTRList supportedVersions = HostResponse.SupportedQBXMLVersionList as IBSTRList;

        int i;
        double vers;
        double LastVers = 0;
        string svers = null;

        for (i = 0; i <= supportedVersions.Count - 1; i++)
        {
            svers = supportedVersions.GetAt(i);
            vers = Convert.ToDouble(svers);
            if (vers > LastVers)
            {
                LastVers = vers;
            }
        }
        return LastVers;
    }

    protected void lnkSyncQB_Click(object sender, EventArgs e)
    {
        QBCustomerSync();
        // GetDataAll();
        GetCustomerList();
        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;

    }

    //public void addCustomerToQB()
    //{
    //    QBSessionManager aSession = null;
    //    IMsgSetRequest requests;
    //    IMsgSetResponse responses;
    //    bool connected = false;
    //    aSession = new QBSessionManager();
    //    aSession.OpenConnection2("", "ADDCUST", ENConnectionType.ctLocalQBD);
    //    aSession.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2007\\IdeavateSol.qbw", ENOpenMode.omDontCare);
    //    connected = true;
    //    requests = getLatestMsgSetRequest(aSession);
    //    requests.Attributes.OnError = ENRqOnError.roeStop;



    //    objProp_User.ConnConfig = Session["config"].ToString();
    //    DataSet ds = new DataSet();
    //    ds = objBL_User.getMSMCustomers(objProp_User);
    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {
    //        requests.ClearRequests();
    //        ICustomerAdd customerReq = requests.AppendCustomerAddRq();
    //        customerReq.CompanyName.SetValue(dr["name"].ToString());
    //        customerReq.Name.SetValue(dr["name"].ToString());
    //        customerReq.Notes.SetValue(dr["remarks"].ToString());
    //        customerReq.Contact.SetValue(dr["contact"].ToString());
    //        customerReq.Phone.SetValue(dr["phone"].ToString());
    //        customerReq.Email.SetValue(dr["email"].ToString());
    //        customerReq.Fax.SetValue(dr["fax"].ToString());
    //        customerReq.BillAddress.Addr2.SetValue(dr["address"].ToString());
    //        customerReq.BillAddress.City.SetValue(dr["city"].ToString());
    //        customerReq.BillAddress.State.SetValue(dr["state"].ToString());
    //        customerReq.BillAddress.PostalCode.SetValue(dr["zip"].ToString());

    //        responses = aSession.DoRequests(requests);
    //        IResponse thisResponse = responses.ResponseList.GetAt(0);
    //        if (thisResponse.StatusCode == 0)
    //        {
    //            ICustomerRet customer = (ICustomerRet)thisResponse.Detail;
    //            objProp_User.ConnConfig = Session["config"].ToString();
    //            objProp_User.QBCustomerID = customer.ListID.GetValue();
    //            objProp_User.CustomerID = Convert.ToInt32(dr["id"]);
    //            objBL_User.UpdateQBCustomerID(objProp_User);
    //        }
    //    }



    //    DataSet dsQB = new DataSet();
    //    dsQB = objBL_User.getQBCustomers(objProp_User);
    //    foreach (DataRow dr in dsQB.Tables[0].Rows)
    //    {
    //        requests.ClearRequests();
    //        ICustomerQuery CustQ = requests.AppendCustomerQueryRq();
    //        CustQ.ORCustomerListQuery.ListIDList.Add(dr["QBCustomerID"].ToString());
    //        responses = aSession.DoRequests(requests);
    //        IResponse thisResponse = responses.ResponseList.GetAt(0);
    //        ICustomerRetList customerRetList = thisResponse.Detail as ICustomerRetList;
    //        ICustomerRet customerRet = customerRetList.GetAt(0);
    //        string editSequence = customerRet.EditSequence.GetValue();
    //        DateTime lastUpdateDateQB = customerRet.TimeModified.GetValue();


    //        if (lastUpdateDateQB < Convert.ToDateTime(dr["LastUpdateDate"].ToString()))
    //        {
    //            requests.ClearRequests();
    //            ICustomerMod CustMod = requests.AppendCustomerModRq();
    //            CustMod.ListID.SetValue(dr["QBCustomerID"].ToString());
    //            CustMod.EditSequence.SetValue(editSequence);
    //            CustMod.CompanyName.SetValue(dr["name"].ToString());
    //            CustMod.Name.SetValue(dr["name"].ToString());
    //            CustMod.Notes.SetValue(dr["remarks"].ToString());
    //            CustMod.Contact.SetValue(dr["contact"].ToString());
    //            CustMod.Phone.SetValue(dr["phone"].ToString());
    //            CustMod.Email.SetValue(dr["email"].ToString());
    //            CustMod.Fax.SetValue(dr["fax"].ToString());
    //            CustMod.BillAddress.Addr2.SetValue(dr["address"].ToString());
    //            CustMod.BillAddress.City.SetValue(dr["city"].ToString());
    //            CustMod.BillAddress.State.SetValue(dr["state"].ToString());
    //            CustMod.BillAddress.PostalCode.SetValue(dr["zip"].ToString());

    //            responses = aSession.DoRequests(requests);
    //            IResponse thisResponse1 = responses.ResponseList.GetAt(0);
    //        }
    //    }

    //    aSession.EndSession();
    //    connected = false;
    //    aSession.CloseConnection();
    //}			

    #endregion

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);
        ddlSearch_SelectedIndexChanged(sender, e);
        // GetCustomerList();
        foreach (GridColumn column in RadGrid_Customer.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
            column.ListOfFilterValues = null;
        }
        Session["ddlSearch_Cus"] = null;
        Session["ddlSearch_Value_Cus"] = null;
        Session["Customer_FilterExpression"] = null;
        Session["Customer_Filters"] = null;
        Session["Cus_TypeFilters"] = null;
        Session["lnkChk_estimate"] = null;
        RadGrid_Customer.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Customer.Rebind();

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
    }

    protected void lnkchk_Click(object sender, EventArgs e)
    {
        Session["lnkChk_estimate"] = lnkChk.Checked;
        // GetCustomerList();
        RadGrid_Customer.Rebind();
    }

    protected void RadGrid_Customer_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //RadGrid_Customer.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        //#region Set the Grid Filters
        //if (!IsPostBack)
        //{
        //    if (Convert.ToString(Request.QueryString["f"]) != "c")
        //    {
        //        if (Session["Cus_FilterExpression"] != null && Convert.ToString(Session["Cus_FilterExpression"]) != "" && Session["Cus_Filters"] != null)
        //        {
        //            RadGrid_Customer.MasterTableView.FilterExpression = Convert.ToString(Session["Cus_FilterExpression"]);
        //            var filtersGet = Session["Cus_Filters"] as List<RetainFilter>;
        //            if (filtersGet != null)
        //            {
        //                foreach (var _filter in filtersGet)
        //                {
        //                    GridColumn column = RadGrid_Customer.MasterTableView.GetColumnSafe(_filter.FilterColumn);
        //                    column.CurrentFilterValue = _filter.FilterValue;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Session["Cus_FilterExpression"] = null;
        //        Session["Cus_Filters"] = null;
        //    }
        //}
        try
        {
            RadGrid_Customer.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
            #region Set the Grid Filters
            if (!IsPostBack)
            {
                // Set filter for grid column except Department and Route
                if (Session["Customer_FilterExpression"] != null
                    && Convert.ToString(Session["Customer_FilterExpression"]) != ""
                    && Session["Customer_Filters"] != null)
                {
                    RadGrid_Customer.MasterTableView.FilterExpression = Convert.ToString(Session["Customer_FilterExpression"]);
                    var filtersGet = Session["Customer_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_Customer.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            if (column.UniqueName != "Type")
                            {
                                column.CurrentFilterValue = _filter.FilterValue;
                            }

                        }
                    }
                }
            }
            else
            {
                #region Save the Grid Filter
                String filterExpression = Convert.ToString(RadGrid_Customer.MasterTableView.FilterExpression);
                if (filterExpression == "")
                {
                    filterExpression = Session["Customer_FilterExpression"] != null ? Session["Customer_FilterExpression"].ToString() : "";
                }

                if (filterExpression != "")
                {
                    Session["Customer_FilterExpression"] = filterExpression;
                    List<RetainFilter> filters = new List<RetainFilter>();

                    foreach (GridColumn column in RadGrid_Customer.MasterTableView.OwnerGrid.Columns)
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

                    Session["Customer_Filters"] = filters;
                }
                else
                {
                    Session["Customer_FilterExpression"] = null;
                    Session["Customer_Filters"] = null;
                }
                #endregion
            }

            // Reset filter for Department column
            if (Session["Cus_TypeFilters"] != null && !string.IsNullOrEmpty(Session["Cus_TypeFilters"].ToString()))
            {
                var strTypeFilters = Session["Cus_TypeFilters"].ToString();
                string[] typeItems = strTypeFilters.Split(',');

                GridColumn typeColumn = RadGrid_Customer.MasterTableView.GetColumn("Type");
                typeColumn.ListOfFilterValues = typeItems;
            }
            #endregion
        }
        catch { }

        GetCustomerList();

    }
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlSearch.SelectedValue == "o.Status")
        //{
        //    rbStatus.Visible = true;
        //    txtSearch.Visible = false;
        //    ddlUserType.Visible = false;
        //    ddlCompany.Visible = false;
        //}
        //else if (ddlSearch.SelectedValue == "o.type")
        //{
        //    rbStatus.Visible = false;
        //    txtSearch.Visible = false;
        //    ddlUserType.Visible = true;
        //    ddlCompany.Visible = false;
        //}
        //else if (ddlSearch.SelectedValue == "B.Name")
        //{
        //    rbStatus.Visible = false;
        //    txtSearch.Visible = false;
        //    ddlUserType.Visible = false;
        //    ddlCompany.Visible = true;
        //}
        //else
        //{
        //    rbStatus.Visible = false;
        //    txtSearch.Visible = true;
        //    ddlUserType.Visible = false;
        //    ddlCompany.Visible = false;
        //}
    }
    protected void RadGrid_Customer_FilterCheckListItemsRequested(object sender, Telerik.Web.UI.GridFilterCheckListItemsRequestedEventArgs e)
    {

        string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Type = DataField;
        DataSet ds = new DataSet();
        ds = objBL_User.GetCustomerType(objProp_User);
        if (ds.Tables[0] != null)
        {
            e.ListBox.DataSource = ds.Tables[0];
            e.ListBox.DataKeyField = DataField;
            e.ListBox.DataTextField = DataField;
            e.ListBox.DataValueField = DataField;
            e.ListBox.DataBind();
        }

    }

    protected void RadGrid_Customer_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_Customer.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Customer_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Customer.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = String.Empty;
                if (column.UniqueName == "Type" && column.ListOfFilterValues != null)
                {
                    filterValues = String.Join(",", column.ListOfFilterValues);
                }
                else
                {
                    filterValues = column.CurrentFilterValue;
                }

                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Customer_Filters"] = filters;
        }
        else
        {
            Session["Customer_FilterExpression"] = null;
            Session["Customer_Filters"] = null;
        }
        #endregion  


        GeneralFunctions obj = new GeneralFunctions();
        if (Session["customers"] != null)
        {
            obj.CorrectTelerikPager(RadGrid_Customer);

            RowSelect();
        }
    }

    protected void lnkCustomerLabel5160_Click(object sender, EventArgs e)
    {
        NameValueCollection data = new NameValueCollection();
        UserAuthentication us = new UserAuthentication();

        data.Add("Token", Session["API_Token"].ToString());
        //data.Add("username", Session["username"].ToString());
        //data.Add("checkdefltcomp", Session["CmpChkDefault"].ToString());
        //data.Add("ReportName", "Schedule_TimeRecapReport");
        data.Add("type", "all");
        string Postd = ddlSearch.SelectedValue + "|||" + "cuatomer" + "|||" + Session["CmpChkDefault"].ToString() + "|||" + "CustomerLable5160" + "|||" + Session["username"].ToString() + "|||" + "all";
        data.Add("Data", Postd);
        string ReportUrl = ReportsUrl + "Report";
        HttpHelper.RedirectAndPOST(this, ReportUrl, data);
        // Response.Redirect("Schedule_TimeRecapReport.aspx?StartDate=" + txtFromDate.Text + "&EndDate=" + txtToDate.Text + "&type=all&ddlDeprt=" + ddlDepartment.SelectedValue + "&Screen=etimesheet");
    }

    private void RowSelect()
    {
        try
        {
            foreach (GridDataItem item in RadGrid_Customer.Items)
            {
                Label lblCustID = (Label)item.FindControl("lblId");
                HyperLink lnkName = (HyperLink)item.FindControl("lnkName");

                if (hdnEditeOwner.Value == "Y" || hdnViewOwner.Value == "Y")
                {
                    lnkName.NavigateUrl = "addcustomer.aspx?uid=" + lblCustID.Text;

                    item.Attributes["ondblclick"] = "window.open('addcustomer.aspx?uid=" + lblCustID.Text + "','_self');";
                }
                else
                {
                    item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
                }
            }

            objGeneral.ConnConfig = Session["config"].ToString();
            _getConnectionConfig.ConnConfig = Session["config"].ToString();

            DataSet dsLastSync = new DataSet();

            List<GeneralViewModel> _lstGeneral = new List<GeneralViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetSagelatsync";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGeneral = serializer.Deserialize<List<GeneralViewModel>>(_APIResponse.ResponseData);
                dsLastSync = CommonMethods.ToDataSet<GeneralViewModel>(_lstGeneral);
            }
            else
            {
                dsLastSync = objBL_General.getSagelatsync(objGeneral);
            }


            int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
            if (intintegration == 1)
                RadGrid_Customer.Columns[3].Visible = true;
            else
                RadGrid_Customer.Columns[3].Visible = false;

        }
        catch (Exception)
        {
        }

    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Customer.MasterTableView.FilterExpression != "" ||
            (RadGrid_Customer.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Customer.MasterTableView.SortExpressions.Count > 0;



    }

    private void GetCustomerList()
    {
        DataSet ds = new DataSet();
        DataTable dtFinal = new DataTable();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.DBName = Session["dbname"].ToString();

        _GetCustomerSearch.ConnConfig = Session["config"].ToString();
        _GetCustomerSearch.DBName = Session["dbname"].ToString();

        _GetCustomers.ConnConfig = Session["config"].ToString();
        _GetCustomers.DBName = Session["dbname"].ToString();

        #region Company Check
        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetCustomerSearch.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetCustomers.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objProp_User.EN = 1;
            _GetCustomerSearch.EN = 1;
            _GetCustomers.EN = 1;
        }
        else
        {
            objProp_User.EN = 0;
            _GetCustomerSearch.EN = 0;
            _GetCustomers.EN = 0;
        }
        objProp_User.inclInactive = lnkChk.Checked;
        _GetCustomerSearch.inclInactive = lnkChk.Checked;
        _GetCustomers.inclInactive = lnkChk.Checked;

        #endregion
        if (ddlSearch.SelectedIndex != 0)
        {
            objProp_User.SearchBy = ddlSearch.SelectedValue;
            objProp_User.SearchValue = txtSearch.Text.Replace("'", "''");

            _GetCustomerSearch.SearchBy = ddlSearch.SelectedValue;
            _GetCustomerSearch.SearchValue = txtSearch.Text.Replace("'", "''");

            if (ddlSearch.SelectedValue == "o.Status")
            {
                objProp_User.SearchValue = rbStatus.SelectedValue;
                _GetCustomerSearch.SearchValue = rbStatus.SelectedValue;
            }
            if (ddlSearch.SelectedValue == "o.type")
            {
                objProp_User.SearchValue = ddlUserType.SelectedValue;
                _GetCustomerSearch.SearchValue = ddlUserType.SelectedValue;
            }
            if (ddlSearch.SelectedValue == "B.Name")
            {
                objProp_User.SearchValue = ddlCompany.SelectedValue;
                _GetCustomerSearch.SearchValue = ddlCompany.SelectedValue;
            }

            List<GetCustomerSearchViewModel> _lstGetCustomerSearch = new List<GetCustomerSearchViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetCustomerSearch";

                _GetCustomerSearch.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerSearch);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetCustomerSearch = serializer.Deserialize<List<GetCustomerSearchViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetCustomerSearchViewModel>(_lstGetCustomerSearch);
            }
            else
            {
                ds = objBL_User.getCustomerSearch(objProp_User, new GeneralFunctions().GetSalesAsigned());
            }

            dtFinal = ds.Tables[0];
        }
        else
        {
            List<GetCustomersViewModel> _lstGetCustomer = new List<GetCustomersViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetCustomers";

                _GetCustomers.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomers);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetCustomer = serializer.Deserialize<List<GetCustomersViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetCustomersViewModel>(_lstGetCustomer);
            }
            else
            {
                ds = objBL_User.getCustomers(objProp_User, new GeneralFunctions().GetSalesAsigned());
            }

        }
        if (ds.Tables.Count > 0)
        {
            if (lnkChk.Checked == true)
            {
                dtFinal = ds.Tables[0];
            }
            else
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    if (ddlSearch.SelectedValue == "o.Status")
                    {
                        if (lnkChk.Checked == false && rbStatus.SelectedValue == "1")
                            dtFinal = null;
                        else
                            dtFinal = ds.Tables[0].Select("Status = 'Active'").CopyToDataTable();
                    }
                    else
                    {
                        if (ds.Tables[0].Select("Status = 'Active'").Count() > 0)
                        {
                            dtFinal = ds.Tables[0].Select("Status = 'Active'").CopyToDataTable();
                        }
                        else
                        {
                            dtFinal = null;
                        }
                    }

                }
                else
                {
                    dtFinal = ds.Tables[0];
                }
            }
        }
        DataTable result = processDataFilter(dtFinal);

        if (result != null)
        {
            RadGrid_Customer.VirtualItemCount = result.Rows.Count;
            RadGrid_Customer.DataSource = result;
        }
        else
        {
            RadGrid_Customer.VirtualItemCount = 0;
            RadGrid_Customer.DataSource = String.Empty;
        }
        if (!IsGridPageIndexChanged)
        {
            RadGrid_Customer.CurrentPageIndex = 0;
            RadGrid_Customer.PageSize = 50;
            Session["RadGrid_CustomerCurrentPageIndex"] = 0;
            ViewState["RadGrid_CustomerminimumRows"] = 0;
            ViewState["RadGrid_CustomermaximumRows"] = RadGrid_Customer.PageSize;


        }
        else
        {
            if (Session["RadGrid_CustomerCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_CustomerCurrentPageIndex"].ToString()) != 0
              )
            {
                RadGrid_Customer.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_CustomerCurrentPageIndex"].ToString());
                ViewState["RadGrid_CustomerminimumRows"] = RadGrid_Customer.CurrentPageIndex * RadGrid_Customer.PageSize;
                ViewState["RadGrid_CustomermaximumRows"] = (RadGrid_Customer.CurrentPageIndex + 1) * RadGrid_Customer.PageSize;

            }
        }



        Session["customers"] = result;

        string user = Session["userid"].ToString();
        // uplblcount.Update();
    }


    protected void btnExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Customer.MasterTableView.GetColumn("ClientSelectColumn").Visible = false;
        RadGrid_Customer.MasterTableView.GetColumn("ImageQB").Visible = false;
        RadGrid_Customer.MasterTableView.GetColumn("State").Visible = true;
        RadGrid_Customer.MasterTableView.GetColumn("Zip").Visible = true;
        RadGrid_Customer.ExportSettings.FileName = "Customer";
        RadGrid_Customer.ExportSettings.IgnorePaging = true;
        RadGrid_Customer.ExportSettings.ExportOnlyData = true;
        RadGrid_Customer.ExportSettings.OpenInNewWindow = true;
        RadGrid_Customer.ExportSettings.HideStructureColumns = true;
        RadGrid_Customer.MasterTableView.UseAllDataFields = true;
        RadGrid_Customer.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Customer.MasterTableView.ExportToExcel();
    }
    protected void RadGrid_Customer_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 6;
        else
            currentItem = 7;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Customer.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Customer.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

    }
    private void PrepareRadGridForExport()
    {
        foreach (GridDataItem gi in RadGrid_Customer.MasterTableView.Items)
        {
            var hypFirstName = (HyperLink)gi.FindControl("lnkName");
            gi["Name"].Text = hypFirstName.Text;
        }
    }
    protected void RadGrid_Customer_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (Convert.ToString(RadGrid_Customer.MasterTableView.FilterExpression) != "")
                    lblRecordCount.Text = totalCount + " Record(s) found";
                else
                    lblRecordCount.Text = RadGrid_Customer.VirtualItemCount + " Record(s) found";
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

    private DataTable processDataFilter(DataTable dt)
    {
        //if (!IsPostBack)
        //{
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Session["Customer_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                if (Session["Customer_Filters"] != null)
                {
                    var filtersGet = Session["Customer_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_Customer.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                            if (column.UniqueName != "Type")
                            {
                                if (column.UniqueName == "balance" || column.UniqueName == "equip" || column.UniqueName == "loc" || column.UniqueName == "opencall")
                                {
                                    sql = sql + " And " + column.UniqueName + "=" + _filter.FilterValue;
                                }
                                else
                                {
                                    sql = sql + " And " + column.UniqueName + " like '%" + _filter.FilterValue + "%'";
                                }

                            }


                        }
                    }

                }

                if (Session["Cus_TypeFilters"] != null)
                {
                    sql = sql + " And Type in ('" + Session["Cus_TypeFilters"].ToString().Replace(",", "','") + "')";
                }

                return result.Select(sql).CopyToDataTable();

            }
            else
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            return dt;
        }


    }

    protected void RadGrid_Customer_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Filter")
        {

            string[] routeItems = ((RadGrid)sender).MasterTableView.GetColumn("Type").ListOfFilterValues;
            if (routeItems != null)
            {
                Session["Cus_TypeFilters"] = string.Join(",", routeItems);
            }

        }
    }
    protected void RadGrid_Customer_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGrid_CustomerCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_CustomerminimumRows"] = e.NewPageIndex * RadGrid_Customer.PageSize;
            ViewState["RadGrid_CustomermaximumRows"] = (e.NewPageIndex + 1) * RadGrid_Customer.PageSize;
        }
        catch { }
    }

    protected void RadGrid_Customer_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGrid_CustomerminimumRows"] = RadGrid_Customer.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_CustomermaximumRows"] = (RadGrid_Customer.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }

    protected void lnkCustomerCategoryDetail_Click(object sender, EventArgs e)
    {
        //NameValueCollection data = new NameValueCollection();
        //UserAuthentication us = new UserAuthentication();

        //data.Add("Token", Session["API_Token"].ToString());
        ////data.Add("username", Session["username"].ToString());
        ////data.Add("checkdefltcomp", Session["CmpChkDefault"].ToString());
        ////data.Add("ReportName", "Schedule_TimeRecapReport");
        //data.Add("type", "all");
        //string Postd = ddlSearch.SelectedValue + "|||" + "cuatomer" + "|||" + Session["CmpChkDefault"].ToString() + "|||" + "CustomerLable5160" + "|||" + Session["username"].ToString() + "|||" + "all";
        //data.Add("Data", Postd);
        //string ReportUrl = ReportsUrl + "Report";
        //HttpHelper.RedirectAndPOST(this, ReportUrl, data);
        // Response.Redirect("Schedule_TimeRecapReport.aspx?StartDate=" + txtFromDate.Text + "&EndDate=" + txtToDate.Text + "&type=all&ddlDeprt=" + ddlDepartment.SelectedValue + "&Screen=etimesheet");

        string urlString = "CustomerCategoryDetailReport.aspx";
        Response.Redirect(urlString, true);
    }
}
