using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using Telerik.Web.UI;
using Telerik.Web.UI.PersistenceFramework;
using System.Linq.Dynamic;
using Telerik.Web.UI.GridExcelBuilder;
using BusinessEntity.APModels;
using BusinessEntity.Utility;
using MOMWebApp;
using BusinessEntity.CommonModel;

public partial class Vendors : System.Web.UI.Page
{
    #region "Variables"
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

    public static bool check = false;

    Vendor _objVendor = new Vendor();
    BL_Vendor _objBLVendor = new BL_Vendor();

    Rol _objRol = new Rol();
    BL_BankAccount _objBLBank = new BL_BankAccount();

    //API Variables
    //string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    APIIntegrationModel _objAPIIntegration = new APIIntegrationModel();
    GetAllVenderAjaxSearchParam _GetAllVenderAjaxSearch = new GetAllVenderAjaxSearchParam();
    getVendorTypeParam _vendorType = new getVendorTypeParam();
    GetStockReportsParam _stockReport = new GetStockReportsParam();
    IsExistVendorDetailsParam _IsExistVendorDetails = new IsExistVendorDetailsParam();
    DeleteRolByIDParam _DeleteRolByID = new DeleteRolByIDParam();
    DeleteVendorParam _DeleteVendor = new DeleteVendorParam();

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }

        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            BindSearchFilters();
            BindVendorTypeFilters();
            #region Show Selected Filter
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["ddlSearch_Vendor"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearch_Vendor"]);
                    ddlSearch.SelectedValue = selectedValue;

                    String searchValue = Convert.ToString(Session["ddlSearch_Value_Vendor"]);
                    if (selectedValue == "Rol.Type")
                    {
                        ddlType.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "Vendor.Status")
                    {
                        ddlStatus.SelectedValue = searchValue;
                    }
                    {
                        txtSearch.Text = searchValue;
                    }

                    SelectSearch();
                }
            }
            else
            {
                Session["ddlSearch_Vendor"] = null;
                Session["ddlSearch_Value_Vendor"] = null;
            }
            #endregion
            FillFilter();

            Permission();
            HighlightSideMenu("acctPayable", "lnkVendors", "acctPayableSub");
        }
        CompanyPermission();
        ConvertToJSON();
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
    public bool CheckAddEditPermission()
    {
        bool result = true;
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            /// Vendor ///////////////////------->

            string VendorPermission = ds.Rows[0]["Vendor"] == DBNull.Value ? "YYYY" : ds.Rows[0]["Vendor"].ToString();
            string ViewVendor = VendorPermission.Length < 4 ? "Y" : VendorPermission.Substring(3, 1);
            if (ViewVendor == "N")
            {
                result = false;
            }
        }

        return result;
    }

    #region Custom Functions
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();

        //GetUserByIdParam _GetUserById = new GetUserByIdParam();
        //_GetUserById.TypeID = Convert.ToInt32(Session["usertypeid"]);
        //_GetUserById.UserID = Convert.ToInt32(Session["userid"]);
        //_GetUserById.ConnConfig = Session["config"].ToString();
        //_GetUserById.DBName = Session["dbname"].ToString();

        DataSet ds = new DataSet();

        //List<UserViewModel> _UserViewModel = new List<UserViewModel>();

        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    string APINAME = "VendorAPI/VendorList_GetUserById";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetUserById);

        //    _UserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
        //    ds = CommonMethods.ToDataSet<UserViewModel>(_UserViewModel);
        //}
        //else
        //{
        ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        //}

        return ds.Tables[0];
    }

    private void Permission()
    {

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
            //Response.Redirect("addcustomer.aspx?uid=" + Session["userid"].ToString());
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

        //DataTable dt = new DataTable();
        //dt = (DataTable)Session["userinfo"];

        //string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
        //if (ProgFunc == "N")
        //{
        //    Response.Redirect("home.aspx");
        //}

        if (Session["type"].ToString() != "am")
        {
            DataTable dtUserPermission = new DataTable();
            dtUserPermission = GetUserById();
            /// AccountPayablemodulePermission ///////////////////------->

            string AccountPayablemodulePermission = dtUserPermission.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : dtUserPermission.Rows[0]["AccountPayablemodulePermission"].ToString();

            if (AccountPayablemodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            DataTable ds = new DataTable();

            // ds = (DataTable)Session["userinfo"];

            //VendorsPermission
            string VendorsPermission = dtUserPermission.Rows[0]["Vendor"] == DBNull.Value ? "YYYY" : dtUserPermission.Rows[0]["Vendor"].ToString();

            hdnAddVendors.Value = VendorsPermission.Length < 1 ? "Y" : VendorsPermission.Substring(0, 1);
            hdnEditVendors.Value = VendorsPermission.Length < 2 ? "Y" : VendorsPermission.Substring(1, 1);
            hdnDeleteVendors.Value = VendorsPermission.Length < 3 ? "Y" : VendorsPermission.Substring(2, 1);
            hdnViewVendors.Value = VendorsPermission.Length < 4 ? "Y" : VendorsPermission.Substring(3, 1);
            if (hdnAddVendors.Value == "N")
            {

                lnkAddnew.Visible = false;
            }
            if (hdnEditVendors.Value == "N")
            {
                btnEdit.Visible = false;
            }
            if (hdnDeleteVendors.Value == "N")
            {
                lnkDelete.Visible = false;

            }
            if (hdnViewVendors.Value == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            //if (hdnAddVendors.Value == "N")
            //{
            //    lnkAddnew.Enabled = false;
            //}
            //if (hdnEditVendors.Value == "N")
            //{
            //    btnEdit.Enabled = false;
            //}
            //if (hdnDeleteVendors.Value == "N")
            //{
            //    lnkDelete.Enabled = false;
            //}
        }
        else
        {
            hdnAddVendors.Value = "Y";
            hdnEditVendors.Value = "Y";
            hdnDeleteVendors.Value = "Y";
            hdnViewVendors.Value = "Y";
        }
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_Vendor.Columns[9].Visible = true;
        }
        else
        {
            RadGrid_Vendor.Columns[9].Visible = false;
            Session["CmpChkDefault"] = "2";
        }
    }

    private void BindSearchFilters()
    {
        Dictionary<string, string> listsearchitems = new Dictionary<string, string>();
        listsearchitems.Add("0", "Select");
        listsearchitems.Add("Vendor.Acct", "Vendor ID");
        listsearchitems.Add("Rol.Name", "Vendor name");
        listsearchitems.Add("Rol.Address", "Address");
        listsearchitems.Add("Rol.Contact", "Contact");
        listsearchitems.Add("Rol.Phone", "Phone");
        listsearchitems.Add("Rol.EMail", "Email");
        listsearchitems.Add("Rol.Type", "Type");
        listsearchitems.Add("Vendor.Status", "Status");


        ddlSearch.DataSource = listsearchitems;
        ddlSearch.DataTextField = "Value";
        ddlSearch.DataValueField = "Key";
        ddlSearch.DataBind();

    }

    // Start : Fill Type DropDownList : Juily 27-12-2019 

    private void BindVendorTypeFilters()
    {
        DataSet dsType = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        _vendorType.ConnConfig = Session["config"].ToString();

        List<GetVendorTypeViewModel> _GetVendorTypeViewModel = new List<GetVendorTypeViewModel>();
        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "VendorAPI/VendorList_getVendorType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _vendorType);

            _GetVendorTypeViewModel = (new JavaScriptSerializer()).Deserialize<List<GetVendorTypeViewModel>>(_APIResponse.ResponseData);
            dsType = CommonMethods.ToDataSet<GetVendorTypeViewModel>(_GetVendorTypeViewModel);
        }
        else
        {
            dsType = objBL_User.getVendorType(objProp_User);
        }
        ddlType.DataSource = dsType.Tables[0];
        ddlType.DataTextField = "Type";
        ddlType.DataValueField = "Type";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new ListItem("Select", "0"));
    }



    // End : Fill Type DropDownList : Juily 27-12-2019 
    #endregion
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectSearch();
    }
    private void SelectSearch()
    {
        if (ddlSearch.SelectedValue == "Rol.Type")
        {
            ddlType.Visible = true;
            ddlStatus.Visible = false;
            txtSearch.Visible = false;
            txtSearch.Text = "";
        }
        else if (ddlSearch.SelectedValue == "Vendor.Status")
        {
            ddlType.Visible = false;
            ddlStatus.Visible = true;
            txtSearch.Visible = false;
            txtSearch.Text = "";
            ddlType.SelectedIndex = 0;
        }
        else
        {
            ddlType.Visible = false;
            ddlStatus.Visible = false;
            txtSearch.Visible = true;
            ddlType.SelectedIndex = 0;

        }
        upPannelSearch.Update();
    }
    private void SaveFilter()
    {
        Dictionary<string, string> dictFilter = new Dictionary<string, string>();
        dictFilter["Search"] = ddlSearch.SelectedValue;
        dictFilter["status"] = ddlStatus.SelectedValue;
        dictFilter["type"] = ddlType.SelectedValue;
        dictFilter["searchtxt"] = txtSearch.Text.Trim();
        Session["FilterVendor"] = dictFilter;
    }
    private void FillFilter()
    {
        if (Session["FilterVendor"] != null)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                Dictionary<string, string> dictFilter = new Dictionary<string, string>();
                dictFilter = (Dictionary<string, string>)Session["FilterVendor"];
                ddlSearch.SelectedValue = dictFilter["Search"];
                SelectSearch();
                ddlStatus.SelectedValue = dictFilter["status"];
                ddlType.SelectedValue = dictFilter["type"];
                txtSearch.Text = dictFilter["searchtxt"];
            }
            else
            {
                Session["FilterVendor"] = null;
            }
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
            objProp_User.Type = "Vendor";


            //_stockReport.DBName = Session["dbname"].ToString();
            _stockReport.ConnConfig = Session["config"].ToString();
            //_stockReport.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _stockReport.Type = "Vendor";

            List<CustomerReportViewModel> _CustomerReportViewModel = new List<CustomerReportViewModel>();
            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "VendorAPI/VendorList_GetStockReports";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _stockReport);

                _CustomerReportViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerReportViewModel>>(_APIResponse.ResponseData);
                dsGetReports = CommonMethods.ToDataSet<CustomerReportViewModel>(_CustomerReportViewModel);
            }
            else
            {
                dsGetReports = objBL_ReportsData.GetStockReports(objProp_User);
            }

            if (dsGetReports.Tables.Count > 0)
                for (int i = 0; i <= dsGetReports.Tables[0].Rows.Count - 1; i++)
                {
                    CustomerReport objCustomerReport = new CustomerReport();

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
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        #region Search Filter
        SelectSearch();
        upPannelSearch.Update();
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_Vendor"] = selectedValue;

        if (selectedValue == "Rol.Type")
        {
            Session["ddlSearch_Value_Vendor"] = ddlType.SelectedValue;
        }
        else if (selectedValue == "Vendor.Status")
        {
            Session["ddlSearch_Value_Vendor"] = ddlStatus.SelectedValue;
        }
        else
        {
            Session["ddlSearch_Value_Vendor"] = txtSearch.Text;
        }
        #endregion

        RadGrid_Vendor.CurrentPageIndex = 0;
        RadGrid_Vendor.PageSize = 50;
        GetVendorList();
        RadGrid_Vendor.Rebind();
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        ddlType.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        SelectSearch();
        foreach (GridColumn column in RadGrid_Vendor.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_Vendor.MasterTableView.SortExpressions.Clear();
        Session["Vendor_FilterExpression"] = null;
        Session["Vendor_Filters"] = null;
        RadGrid_Vendor.MasterTableView.FilterExpression = "";
        RadGrid_Vendor.CurrentPageIndex = 1;
        upPannelSearch.Update();
        GetVendorList();
        RadGrid_Vendor.Rebind();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Vendor.SelectedItems)
        {
            Label lblVendorID = (Label)item.FindControl("lblId");
            Response.Redirect("AddVendor.aspx?id=" + lblVendorID.Text);
        }
    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddVendor.aspx");
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Vendor.SelectedItems)
        {
            Label lblVendorID = (Label)item.FindControl("lblId");
            Label lblRolID = (Label)item.FindControl("lblRol");
            DeleteVendor(lblVendorID.Text, lblRolID.Text);
        }
    }
    private void DeleteVendor(string VendorID, string RolID)
    {
        Vendor _objVendor = new Vendor();
        BL_Vendor _objBLVendor = new BL_Vendor();

        Rol _objRol = new Rol();
        BL_BankAccount _objBLBank = new BL_BankAccount();

        try
        {
            _objRol.ConnConfig = WebBaseUtility.ConnectionString;
            _objVendor.ConnConfig = WebBaseUtility.ConnectionString;
            _IsExistVendorDetails.ConnConfig = WebBaseUtility.ConnectionString;
            _DeleteRolByID.ConnConfig = WebBaseUtility.ConnectionString;
            _DeleteVendor.ConnConfig = WebBaseUtility.ConnectionString;

            if (VendorID != null && RolID != null)
            {
                _objVendor.ID = Convert.ToInt32(VendorID);
                _IsExistVendorDetails.ID = Convert.ToInt32(VendorID);
                _DeleteVendor.ID = Convert.ToInt32(VendorID);

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {

                    string APINAME = "VendorAPI/VendorList_IsExistVendorDetails";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _IsExistVendorDetails);

                    bool _IsExist = Convert.ToBoolean(_APIResponse.ResponseData);

                    if (!_IsExist)
                    {
                        _DeleteRolByID.ID = Convert.ToInt32(RolID);

                        string APINAME1 = "VendorAPI/VendorList_DeleteRolByID";

                        APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _DeleteRolByID);

                        string APINAME2 = "VendorAPI/VendorList_DeleteVendor";

                        APIResponse _APIResponse2 = new MOMWebUtility().CallMOMWebAPI(APINAME2, _DeleteVendor);

                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Vendor deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        GetVendorList();
                        RadGrid_Vendor.Rebind();
                    }
                    else
                    {
                        string str = "This vendor has transactions(s) posting to it and can therefore not be deleted.";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    if (!_objBLVendor.IsExistVendorDetails(_objVendor))
                    {
                        _objRol.ID = Convert.ToInt32(RolID);

                        _objBLBank.DeleteRolByID(_objRol);

                        _objBLVendor.DeleteVendor(_objVendor);

                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Vendor deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        GetVendorList();
                        RadGrid_Vendor.Rebind();
                    }
                    else
                    {
                        string str = "This vendor has transactions(s) posting to it and can therefore not be deleted.";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    }

                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Please select a vendor to delete.',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkchk_Click(object sender, EventArgs e)
    {
        if (lnkChk.Checked)
        {
            check = true;
            GetVendorList();
            RadGrid_Vendor.Rebind();
        }
        else
        {
            check = false;
            GetVendorList();
            RadGrid_Vendor.Rebind();
        }
    }
    private void GetVendorList()
    {
        try
        {
            string column = null;
            string searchTerm = null;
            if (ddlSearch.SelectedValue == "Rol.Type")
            {
                column = ddlSearch.SelectedValue;
                searchTerm = ddlType.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "Vendor.Status")
            {
                column = ddlSearch.SelectedValue;
                searchTerm = ddlStatus.SelectedValue;
                if (ddlStatus.SelectedValue == "InActive")
                //{ check = true; }
                //else { check = false; }
                { lnkChk.Checked = true; }
                else { lnkChk.Checked = false; }
            }
            else
            {
                column = ddlSearch.SelectedValue;
                searchTerm = txtSearch.Text;
            }

            //if (!check)
            if (lnkChk.Checked == false)
            {
                _objVendor.StatusDisplay = "Active";
                _GetAllVenderAjaxSearch.StatusDisplay = "Active";
            }
            else
            {
                _objVendor.StatusDisplay = "InActive";
                _GetAllVenderAjaxSearch.StatusDisplay = "InActive";
            }


            DataSet ds = new DataSet();
            DataTable dtFinal = new DataTable();
            _objVendor.ConnConfig = WebBaseUtility.ConnectionString;
            _objVendor.Cols = column != "0" ? column : null;
            _objVendor.SearchValue = searchTerm != string.Empty ? searchTerm : null;
            _objVendor.UserID = Convert.ToInt32(Session["UserID"].ToString());

            _GetAllVenderAjaxSearch.ConnConfig = WebBaseUtility.ConnectionString;
            _GetAllVenderAjaxSearch.Cols = column != "0" ? column : null;
            _GetAllVenderAjaxSearch.SearchValue = searchTerm != string.Empty ? searchTerm : null;
            _GetAllVenderAjaxSearch.UserID = Convert.ToInt32(Session["UserID"].ToString());

            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                _objVendor.EN = 1;
                _GetAllVenderAjaxSearch.EN = 1;
            }
            else
            {
                _objVendor.EN = 0;
                _GetAllVenderAjaxSearch.EN = 0;
            }

            _objVendor.PageNumber = RadGrid_Vendor.CurrentPageIndex + 1;
            _GetAllVenderAjaxSearch.PageNumber = RadGrid_Vendor.CurrentPageIndex + 1;

            string filterexpression = string.Empty;
            filterexpression = RadGrid_Vendor.MasterTableView.FilterExpression;
            if (filterexpression != "")
            {
                //int _sPageSize = 50;
                //if (Session["Vendor_VirtulItemCount"] != null)
                //{
                //    _sPageSize = Convert.ToInt32(Session["Vendor_VirtulItemCount"]);
                //}
                //_objVendor.PageSize = RadGrid_Vendor.VirtualItemCount;
                _objVendor.PageSize = 0;
                _GetAllVenderAjaxSearch.PageSize = 0;
                //_objVendor.PageSize = _sPageSize;
                //_GetAllVenderAjaxSearch.PageSize = _sPageSize;
            }
            else
            {
                _objVendor.PageSize = RadGrid_Vendor.PageSize;
                _GetAllVenderAjaxSearch.PageSize = RadGrid_Vendor.PageSize;
            }
            _objVendor.SortBy = "";
            _objVendor.SortType = "desc";
            _GetAllVenderAjaxSearch.SortBy = "";
            _GetAllVenderAjaxSearch.SortType = "desc";
            if (RadGrid_Vendor.MasterTableView.SortExpressions.Count > 0)
            {
                _objVendor.SortBy = RadGrid_Vendor.MasterTableView.SortExpressions[0].FieldName;
                _objVendor.SortType = RadGrid_Vendor.MasterTableView.SortExpressions[0].SortOrderAsString();
                _GetAllVenderAjaxSearch.SortBy = RadGrid_Vendor.MasterTableView.SortExpressions[0].FieldName;
                _GetAllVenderAjaxSearch.SortType = RadGrid_Vendor.MasterTableView.SortExpressions[0].SortOrderAsString();
            }

            List<GetAllVenderAjaxSearchModel> _lstGetAllVenderAjaxSearch = new List<GetAllVenderAjaxSearchModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {

                string APINAME = "VendorAPI/VendorList_GetAllVenderAjaxSearch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllVenderAjaxSearch);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetAllVenderAjaxSearch = serializer.Deserialize<List<GetAllVenderAjaxSearchModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetAllVenderAjaxSearchModel>(_lstGetAllVenderAjaxSearch);
                //ds.Tables[0].Columns.Remove("Status");
                //ds.Tables[0].Columns["Vstatus"].ColumnName = "Status";
                //ds.Tables[0].Columns.Remove("Type");
                //ds.Tables[0].Columns["VType"].ColumnName = "Type";
                //ds.Tables[0].Columns["Email"].ColumnName = "EMail";
            }
            else
            {
                ds = _objBLVendor.GetAllVenderAjaxSearch(_objVendor);
            }
            //if show inactive checked
            //if (check)
            if (lnkChk.Checked == true)
            {
                dtFinal = ds.Tables[0];
            }
            else
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    //if (!check)
                    if (lnkChk.Checked == false)
                    {
                        dtFinal = ds.Tables[0].Select("Status <> 'InActive'").CopyToDataTable();
                    }
                }
            }

            int totalRow = 0;
            //if (ds.Tables[0].Rows.Count > 0)
            //totalRow = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRow"]);

            if (dtFinal.Rows.Count > 0)
                totalRow = Convert.ToInt32(dtFinal.Rows[0]["TotalRow"]);
            RadGrid_Vendor.VirtualItemCount = totalRow;
            RadGrid_Vendor.AllowCustomPaging = true;
            ViewState["VirtualItemCount"] = totalRow;
            lblRecordCount.Text = totalRow + " Record(s) found";
            //RadGrid_Vendor.VirtualItemCount = dtFinal.Rows.Count;
            RadGrid_Vendor.DataSource = dtFinal;





            //Session["vendors"] = GetFilteredDataSource();
            DataTable dtID;
            if (dtFinal.Rows.Count > 0)
            {
                DataView view = new DataView(GetFilteredDataSource());
                dtID = view.ToTable("vendors", true, "ID");

                if (filterexpression != "")
                {
                    RadGrid_Vendor.AllowCustomPaging = true;
                    RadGrid_Vendor.VirtualItemCount = dtID.Rows.Count;
                    ViewState["VirtualItemCount"] = dtID.Rows.Count;
                    lblRecordCount.Text = dtID.Rows.Count + " Record(s) found";
                    //updpnl.Update();
                }


            }
            else
            {
                dtID = dtFinal;
            }
            Session["vendors"] = dtID;

            SaveFilter();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable GetFilteredDataSource()
    {
        DataTable DT = new DataTable();
        DataTable FilteredDT = new DataTable();
        string filterexpression = string.Empty;
        filterexpression = RadGrid_Vendor.MasterTableView.FilterExpression;
        if (filterexpression != "")
        {
            DT = (DataTable)RadGrid_Vendor.DataSource;
            FilteredDT = DT.AsEnumerable()
            .AsQueryable()
            .Where(filterexpression)
            .CopyToDataTable();
            return FilteredDT;
        }
        else
        {
            return (DataTable)RadGrid_Vendor.DataSource;
        }

    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Vendor.MasterTableView.FilterExpression != "" ||
            (RadGrid_Vendor.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Vendor.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Vendor_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Vendor.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Vendor_FilterExpression"] != null && Convert.ToString(Session["Vendor_FilterExpression"]) != "" && Session["Vendor_Filters"] != null)
                {
                    RadGrid_Vendor.MasterTableView.FilterExpression = Convert.ToString(Session["Vendor_FilterExpression"]);
                    var filtersGet = Session["Vendor_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_Vendor.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["Vendor_FilterExpression"] = null;
                Session["Vendor_Filters"] = null;
                //Session["Vendor_VirtulItemCount"] = null;
            }
            if (Request.QueryString["AddVendor"] != null)
            {
                if (Convert.ToString(Request.QueryString["AddVendor"]) == "Y")
                {
                    if (check == true)
                    {
                        lnkChk.Checked = true;
                    }
                    else
                    {
                        lnkChk.Checked = false;
                    }
                }
            }
        }

        #endregion
        GetVendorList();
    }
    protected void RadGrid_Vendor_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        //if (Session["vendors"] != null)
        //{
        //    DataTable dtID = (DataTable)Session["vendors"];
        //    lblRecordCount.Text = dtID.Rows.Count + " Record(s) found";
        //    updpnl.Update();
        //}
        rowCount = Convert.ToInt32(ViewState["VirtualItemCount"]);
        lblRecordCount.Text = rowCount + " Record(s) found";
        //updpnl.Update();
    }
    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_Vendor.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkName");
            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='AddVendor.aspx?id=" + lblID.Text + "'";
        }
    }
    protected void RadGrid_Vendor_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_Vendor.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Vendor_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Vendor.MasterTableView.OwnerGrid.Columns)
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

            Session["Vendor_Filters"] = filters;
            //Session["Vendor_VirtulItemCount"] = RadGrid_Vendor.VirtualItemCount;
        }
        else
        {
            Session["Vendor_FilterExpression"] = null;
            Session["Vendor_Filters"] = null;
            //Session["Vendor_VirtulItemCount"] = null;
        }
        #endregion  
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Vendor);
        RowSelect();


    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {


        ddlType.Visible = false;
        ddlStatus.Visible = false;
        txtSearch.Visible = true;
        ResetFormControlValues(this);
        check = false;
        lnkChk.Checked = false;

        foreach (GridColumn column in RadGrid_Vendor.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        Session["Vendor_FilterExpression"] = null;
        Session["Vendor_Filters"] = null;
        //Session["Vendor_VirtulItemCount"] = null;
        RadGrid_Vendor.MasterTableView.FilterExpression = "";
        //RadGrid_Vendor.MasterTableView.FilterExpression = string.Empty;
        //RadGrid_Vendor.PageSize = 50;
        ////lnkSearch_Click(sender, e);
        //Page_Load(sender, e);
        ////GetVendorList();
        ////RadGrid_Vendor.Rebind();
        Response.Redirect("vendors.aspx?f=c");

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
                        //if (c.ClientID.Contains("PageSizeComboBox"))
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

    protected void RadGrid_Vendor_ItemCreated(object sender, GridItemEventArgs e)
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
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Vendor.ExportSettings.FileName = "Vendor";
        RadGrid_Vendor.ExportSettings.IgnorePaging = true;
        RadGrid_Vendor.ExportSettings.ExportOnlyData = true;
        RadGrid_Vendor.ExportSettings.OpenInNewWindow = true;
        RadGrid_Vendor.ExportSettings.HideStructureColumns = true;
        RadGrid_Vendor.MasterTableView.UseAllDataFields = true;
        RadGrid_Vendor.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Vendor.MasterTableView.ExportToExcel();
    }
    protected void RadGrid_Vendor_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Vendor.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Vendor.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Vendor.SelectedItems)
        {
            Label lblCustID = (Label)item.FindControl("lblId");
            Response.Redirect("AddVendor.aspx?id=" + lblCustID.Text + "&t=c");
        }

    }
}

//public static class Test
//{
//    public static DataSet ToDataSet<T>(this IList<T> list)
//    {
//        Type elementType = typeof(T);
//        DataSet ds = new DataSet();
//        DataTable t = new DataTable();
//        ds.Tables.Add(t);

//        //add a column to table for each public property on T
//        foreach (var propInfo in elementType.GetProperties())
//        {
//            Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

//            t.Columns.Add(propInfo.Name, ColType);
//        }

//        //go through each property on T and add each value to the table
//        foreach (T item in list)
//        {
//            DataRow row = t.NewRow();

//            foreach (var propInfo in elementType.GetProperties())
//            {
//                row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
//            }

//            t.Rows.Add(row);
//        }

//        return ds;
//    }
//}