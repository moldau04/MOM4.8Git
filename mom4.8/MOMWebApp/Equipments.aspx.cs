using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Linq;
using System.Linq.Dynamic;
using BusinessEntity.APModels;
using BusinessEntity.Utility;
using MOMWebApp;
using BusinessEntity.CustomersModel;
using System.Web;
using System.IO;
using System.Configuration;

public partial class Equipments : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();
    User objProp_User = new User();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

    CompanyOffice objCompany = new CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    BL_Report objBL_Report = new BL_Report();

    public Boolean isAssignProject = false;
    public int EmpId;
    public int isShowAll = 0;
    private bool IsGridPageIndexChanged = false;

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetStockReportsParam _GetStockReports = new GetStockReportsParam();
    GetEquiptypeParam _GetEquiptype = new GetEquiptypeParam();
    GetEquipmentCategoryParam _GetEquipmentCategory = new GetEquipmentCategoryParam();
    GetActiveServiceTypeParam _GetActiveServiceType = new GetActiveServiceTypeParam();
    GetCompanyByUserIDParam _GetCompanyByUserID = new GetCompanyByUserIDParam();
    GetBuildingElevParam _GetBuildingElev = new GetBuildingElevParam();
    GetEquipmentParam _GetEquipment = new GetEquipmentParam();
    DeleteEquipmentParam _DeleteEquipment = new DeleteEquipmentParam();
    GetEquipClassificationParam _GetEquipClassification = new GetEquipClassificationParam();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            userpermissions();
            FillEquiptype();
            FillServiceType();
            FillEquipCategory();
            FillEquipClassification();
            FillBuilding();
            FillCompany();
            FillRoute();
            Fillterritory();
            #region Show Selected Filter
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                IsGridPageIndexChanged = true;
                if (Session["ddlSearch_Equi"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearch_Equi"]);
                    ddlSearch.SelectedValue = selectedValue;

                    String searchValue = Convert.ToString(Session["ddlSearch_Value_Equi"]);
                    if (selectedValue == "e.Status")
                    {
                        rbStatus.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "e.type")
                    {
                        ddlType.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "e.Cat")
                    {
                        ddlServiceType.SelectedValue = searchValue;
                    }
                    else if (ddlSearch.SelectedValue == "B.Name")
                    {
                        ddlCompany.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "rt.Name")
                    {
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            var selectedValArr = searchValue.Split(',');
                            foreach (RadComboBoxItem item in ddlRoute.Items)
                            {
                                if (Array.IndexOf(selectedValArr, item.Value) >= 0)
                                {
                                    item.Checked = true;
                                }
                            }
                        }

                    }
                    else if (selectedValue == "tr.Name")
                    {
                        ddlTerr.SelectedValue = searchValue;
                    }
                    else
                    {
                        txtSearch.Text = searchValue;
                    }

                    ShowHideFilter();
                }
                if (Session["filterAdvanEquip"] != null)
                {
                    if (Session["filterAdvanEquip"].ToString() != string.Empty)
                    {
                        string[] strFilter = Session["filterAdvanEquip"].ToString().Split(';');
                        ddlFilterStatus.SelectedValue = strFilter[0];
                        ddlFilterType.SelectedValue = strFilter[1];
                        ddlFilterCategory.SelectedValue = strFilter[2];
                        ddlFilterClassification.SelectedValue = strFilter[3];
                        ddlBuilding.SelectedValue = strFilter[4];
                        txtManufact.Text = strFilter[5];
                        ddlcompare.SelectedValue = strFilter[6];
                        txtLastServiceDt.Text = strFilter[7];
                        ddlComareI.SelectedValue = strFilter[8];
                        txtInstallDt.Text = strFilter[9];
                        ddlCompareP.SelectedValue = strFilter[10];
                        txtPrice.Text = strFilter[11];
                        isShowAll = Convert.ToInt16(strFilter[12]);
                        lnkChk.Checked = Convert.ToBoolean(strFilter[13]);
                    }
                }
            }
            else
            {
                Session["ddlSearch_Equi"] = null;
                Session["ddlSearch_Value_Equi"] = null;
                Session.Remove("Equip_FilterExpression");
                Session.Remove("Equip_Filters");
            }
            #endregion

            HighlightSideMenu("cstmMgr", "lnkEquipmentsSMenu", "cstmMgrSub");
        }

        Permission();
        ConvertToJSON();
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

    private void Permission()
    {


        if (Session["type"].ToString() == "c")
        {
            //    Response.Redirect("home.aspx");
            //   // Response.Redirect("addequipment.aspx?uid=" + Session["userid"].ToString());
            btnCopy.Visible = false;
            btnDelete.Visible = false;
            btnEdit.Visible = false;
            lnkAddnew.Visible = false;
            lnkQRCode.Visible = false;
            lnkMassMCP.Visible = false;
            printbtn.Visible = false;
        }

        if (Session["MSM"].ToString() == "TS")
        {
            //Response.Redirect("home.aspx");
            //pnlGridButtons.Visible = false;
            lnkAddnew.Visible = false;
            btnCopy.Visible = false;
            btnEdit.Visible = false;
            btnDelete.Visible = false;
        }

        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            //Response.Redirect("home.aspx");
        }
        try
        {
            DataTable ds = new DataTable();
            //ds = (DataTable)Session["userinfo"];
            ds = GetUserById();

            EmpId = ds.Rows[0]["Empid"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Rows[0]["Empid"]);
            isAssignProject = ds.Rows[0]["IsAssignedProject"] == DBNull.Value ? false : Convert.ToBoolean(ds.Rows[0]["IsAssignedProject"]);
            if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
            {

                //Equipment

                string ElevatorPermission = ds.Rows[0]["Elevator"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Elevator"].ToString();

                hdnAddeEquipment.Value = ElevatorPermission.Length < 1 ? "Y" : ElevatorPermission.Substring(0, 1);
                hdnEditeEquipment.Value = ElevatorPermission.Length < 2 ? "Y" : ElevatorPermission.Substring(1, 1);
                hdnDeleteEquipment.Value = ElevatorPermission.Length < 3 ? "Y" : ElevatorPermission.Substring(2, 1);
                hdnViewEquipment.Value = ElevatorPermission.Length < 4 ? "Y" : ElevatorPermission.Substring(3, 1);

                if (hdnAddeEquipment.Value == "N")
                {

                    lnkAddnew.Visible = false;
                    btnCopy.Visible = false;
                }
                if (hdnEditeEquipment.Value == "N")
                {
                    btnEdit.Visible = false;

                }
                if (hdnDeleteEquipment.Value == "N")
                {
                    btnDelete.Visible = false;

                }
                if (hdnViewEquipment.Value == "N")
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }
        }
        catch (Exception ex)
        {

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
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_Equip.Columns.FindByDataField("Company").Visible = true;
            ddlSearch.Items.FindByValue("B.Name").Enabled = true;

        }
        else
        {
            RadGrid_Equip.Columns.FindByDataField("Company").Visible = false;
            ddlSearch.Items.FindByValue("B.Name").Enabled = false;
            Session["CmpChkDefault"] = "2";
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
            objProp_User.Type = "Equipment";

            //API
            _GetStockReports.DBName = Session["dbname"].ToString();
            _GetStockReports.ConnConfig = Session["config"].ToString();
            _GetStockReports.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _GetStockReports.Type = "Equipment";

            List<CustomerReportViewModel> _lstCustomerReport = new List<CustomerReportViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentsList_GetStockReports";

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

    private void userpermissions()
    {
        //if (Session["type"].ToString() != "c")
        //{
        //    if (Session["type"].ToString() != "am")
        //    {
        //        objProp_User.ConnConfig = Session["config"].ToString();
        //        objProp_User.Username = Session["username"].ToString();
        //        objProp_User.PageName = "equipments.aspx";
        //        DataSet dspage = objBL_User.getScreensByUser(objProp_User);
        //        if (dspage.Tables[0].Rows.Count > 0)
        //        {
        //            if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
        //            {
        //                Response.Redirect("home.aspx");
        //            }
        //        }
        //        else
        //        {
        //            Response.Redirect("home.aspx");
        //        }
        //    }
        //}
    }

    private void FillEquiptype()
    {
        Session["EquipTypeLabel"] = null;
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        _GetEquiptype.ConnConfig = Session["config"].ToString();

        List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentsList_GetEquiptype";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquiptype);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
        }
        else
        {
            ds = objBL_User.getEquiptype(objProp_User);
        }

        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "edesc";
        ddlType.DataValueField = "edesc";
        ddlType.DataBind();

        ddlType.Items.Insert(0, new ListItem("None", "None"));

        ddlFilterType.DataSource = ds.Tables[0];
        ddlFilterType.DataTextField = "edesc";
        ddlFilterType.DataValueField = "edesc";
        ddlFilterType.DataBind();

        ddlFilterType.Items.Insert(0, new ListItem("None", "None"));
        ddlFilterType.Items.Insert(0, new ListItem("All", ""));
        lblType.Text = "Type";
        //RadGrid_Equip.Columns[5].HeaderText = "Type";
        RadGrid_Equip.Columns.FindByDataField("Type").HeaderText = "Type";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            Session["EquipTypeLabel"] = ds.Tables[0].Rows[0]["Label"].ToString();
            lblType.Text = ds.Tables[0].Rows[0]["Label"].ToString();
            RadGrid_Equip.Columns.FindByDataField("Type").HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    private void FillEquipCategory()
    {
        Session["EquipCatLabel"] = null;
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        _GetEquipmentCategory.ConnConfig = Session["config"].ToString();
        List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentsList_GetEquipmentCategory";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipmentCategory);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
        }
        else
        {
            ds = objBL_User.getEquipmentCategory(objProp_User);
        }

        ddlFilterCategory.DataSource = ds.Tables[0];
        ddlFilterCategory.DataTextField = "edesc";
        ddlFilterCategory.DataValueField = "edesc";
        ddlFilterCategory.DataBind();
        ddlFilterCategory.Items.Insert(0, new ListItem("All", ""));
        ddlFilterCategory.Items.Insert(1, new ListItem("None", "None"));
        //ddlFilterCategory.Items.Add(new ListItem("New", "New"));
        //ddlFilterCategory.Items.Add(new ListItem("Refurbished", "Refurbished"));
        lblCategory.Text = "Category";
        //RadGrid_Equip.Columns[11].HeaderText = "Category";
        RadGrid_Equip.Columns.FindByDataField("Category").HeaderText = "Category";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            Session["EquipCatLabel"] = ds.Tables[0].Rows[0]["Label"].ToString();
            lblCategory.Text = ds.Tables[0].Rows[0]["Label"].ToString();
            RadGrid_Equip.Columns.FindByDataField("Category").HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    private void FillServiceType()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        _GetActiveServiceType.ConnConfig = Session["config"].ToString();
        //ds = objBL_User.GetServiceType(objProp_User);

        List<GetActiveServiceTypeViewModel> _lstGetActiveServiceType = new List<GetActiveServiceTypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentsList_GetActiveServiceType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetActiveServiceType);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetActiveServiceType = serializer.Deserialize<List<GetActiveServiceTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetActiveServiceTypeViewModel>(_lstGetActiveServiceType);
        }
        else
        {
            ds = new BusinessLayer.Programs.BL_ServiceType().GetActiveServiceType(objProp_User.ConnConfig);
        }

        ddlServiceType.DataSource = ds.Tables[0];
        ddlServiceType.DataTextField = "type";
        ddlServiceType.DataValueField = "type";
        ddlServiceType.DataBind();
        ddlServiceType.Items.Insert(0, new ListItem("None", "None"));
    }
    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();

        _GetCompanyByUserID.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetCompanyByUserID.DBName = Session["dbname"].ToString();
        _GetCompanyByUserID.ConnConfig = Session["config"].ToString();

        DataSet dc = new DataSet();

        List<GetCompanyByUserIDViewModel> _lstGetCompanyByUserID = new List<GetCompanyByUserIDViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentsList_GetCompanyByUserID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCompanyByUserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetCompanyByUserID = serializer.Deserialize<List<GetCompanyByUserIDViewModel>>(_APIResponse.ResponseData);
            dc = CommonMethods.ToDataSet<GetCompanyByUserIDViewModel>(_lstGetCompanyByUserID);
        }
        else
        {
            dc = objBL_Company.getCompanyByUserID(objCompany);
        }

        if (dc.Tables.Count > 0)
        {
            ddlCompany.DataSource = dc.Tables[0];
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataBind();
        }
    }

    private void FillBuilding()
    {
        Session["EquipLabel"] = null;
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        _GetBuildingElev.ConnConfig = Session["config"].ToString();
        List<GetBuildingElevViewModel> _lstGetBuildingElev = new List<GetBuildingElevViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentsList_GetBuildingElev";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBuildingElev);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetBuildingElev = serializer.Deserialize<List<GetBuildingElevViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetBuildingElevViewModel>(_lstGetBuildingElev);
        }
        else
        {
            ds = objBL_User.getBuildingElev(objProp_User);
        }

        ddlBuilding.DataSource = ds.Tables[0];
        ddlBuilding.DataTextField = "EDesc";
        ddlBuilding.DataValueField = "EDesc";
        ddlBuilding.DataBind();
        ddlBuilding.Items.Insert(0, new ListItem("All", "All"));
        lblBuilding.Text = "Building";
        //RadGrid_Equip.Columns[9].HeaderText = "Building";
        RadGrid_Equip.Columns.FindByDataField("building").HeaderText = "Building";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            Session["EquipLabel"] = ds.Tables[0].Rows[0]["Label"].ToString();
            lblBuilding.Text = ds.Tables[0].Rows[0]["Label"].ToString();
            RadGrid_Equip.Columns.FindByDataField("building").HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    private void GetDataAll()
    {
        List<RetainFilter> filters = new List<RetainFilter>();
        String filterExpression = Convert.ToString(RadGrid_Equip.MasterTableView.FilterExpression);
        if (!IsPostBack)
        {
            if (filterExpression == "")
            {
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["Equip_FilterExpression"] != null && Convert.ToString(Session["Equip_FilterExpression"]) != "" && Session["Equip_Filters"] != null)
                    {
                        filterExpression = Convert.ToString(Session["Equip_FilterExpression"]);
                        RadGrid_Equip.MasterTableView.FilterExpression = Convert.ToString(Session["Equip_FilterExpression"]);
                        var filtersGet = Session["Equip_Filters"] as List<RetainFilter>;
                        if (filtersGet != null)
                        {
                            foreach (var _filter in filtersGet)
                            {

                                var filterCol = _filter.FilterColumn;
                                GridColumn column = RadGrid_Equip.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                                if (column != null)
                                {
                                    column.CurrentFilterValue = _filter.FilterValue;
                                }
                            }
                        }

                    }
                }
                else
                {
                    Session["Equip_FilterExpression"] = null;
                    Session["Equip_Filters"] = null;
                }

            }
        }

        if (!IsGridPageIndexChanged)
        {
            RadGrid_Equip.CurrentPageIndex = 0;
            Session["RadGrid_EquipCurrentPageIndex"] = 0;
            ViewState["RadGrid_EquipminimumRows"] = 0;
            ViewState["RadGrid_EquipmaximumRows"] = RadGrid_Equip.PageSize;
        }
        else
        {
            if (Session["RadGrid_EquipCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_EquipCurrentPageIndex"].ToString()) != 0
                && Convert.ToString(Request.QueryString["f"]) != "c")
            {
                RadGrid_Equip.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_EquipCurrentPageIndex"].ToString());
                ViewState["RadGrid_EquipminimumRows"] = RadGrid_Equip.CurrentPageIndex * RadGrid_Equip.PageSize;
                ViewState["RadGrid_EquipmaximumRows"] = (RadGrid_Equip.CurrentPageIndex + 1) * RadGrid_Equip.PageSize;

            }
        }
        if (string.IsNullOrEmpty(filterExpression) && Session["Equip_FilterExpression"] != null)
        {
            filterExpression = Convert.ToString(Session["Equip_FilterExpression"]);
        }




        if (filterExpression != "")
        {
            Session["Equip_FilterExpression"] = filterExpression;
            foreach (GridColumn column in RadGrid_Equip.MasterTableView.OwnerGrid.Columns)
            {

                String filterValues = String.Empty;
                String columnName = column.UniqueName;

                filterValues = column.CurrentFilterValue;

                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }

            }

            Session["Equip_Filters"] = filters;
        }
        SaveFilter();
        DataTable dtFinal = new DataTable();

        DataSet ds = new DataSet();
        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.SearchBy = ddlSearch.SelectedValue;

        _GetEquipment.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetEquipment.ConnConfig = Session["config"].ToString();
        _GetEquipment.SearchBy = ddlSearch.SelectedValue;

        if (ddlSearch.SelectedValue == "e.Status")
        {
            objProp_User.SearchValue = rbStatus.SelectedValue;
            _GetEquipment.SearchValue = rbStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "e.type")
        {
            objProp_User.SearchValue = ddlType.SelectedValue;
            _GetEquipment.SearchValue = ddlType.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "e.Cat")
        {
            objProp_User.SearchValue = ddlServiceType.SelectedValue;
            _GetEquipment.SearchValue = ddlServiceType.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "B.Name")
        {
            objProp_User.SearchValue = ddlCompany.SelectedValue;
            _GetEquipment.SearchValue = ddlCompany.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "rt.Name")
        {
            var routeSearchValue = string.Empty;
            if (ddlRoute.CheckedItems.Count > 0)
            {
                foreach (var item in ddlRoute.CheckedItems)
                {
                    routeSearchValue += item.Value + ",";
                }

                routeSearchValue = routeSearchValue.TrimEnd(',');
            }

            objProp_User.SearchValue = routeSearchValue;
            _GetEquipment.SearchValue = routeSearchValue;
        }
        else if (ddlSearch.SelectedValue == "tr.Name")
        {
            Session["ddlSearch_Value_Equi"] = ddlTerr.SelectedValue;
            objProp_User.SearchValue = ddlTerr.SelectedValue;
            _GetEquipment.SearchValue = ddlTerr.SelectedValue;
        }
        else
        {
            objProp_User.SearchValue = txtSearch.Text.Replace("'", "''");
            _GetEquipment.SearchValue = txtSearch.Text.Replace("'", "''");
        }

        string strInstall = string.Empty;
        if (txtInstallDt.Text.Trim() != string.Empty)
        {
            if (ddlComareI.SelectedValue == "0")
                strInstall = " = '" + txtInstallDt.Text.Trim();
            else if (ddlComareI.SelectedValue == "1")
                strInstall = " >= '" + txtInstallDt.Text.Trim();
            else if (ddlComareI.SelectedValue == "2")
                strInstall = " <= '" + txtInstallDt.Text.Trim();
            else if (ddlComareI.SelectedValue == "3")
                strInstall = " > '" + txtInstallDt.Text.Trim();
            else if (ddlComareI.SelectedValue == "4")
                strInstall = " < '" + txtInstallDt.Text.Trim();
            else
                strInstall = " = '" + txtInstallDt.Text.Trim();
        }
        objProp_User.InstallDateString = strInstall;
        objProp_User.InstallDate = string.Empty;

        _GetEquipment.InstallDateString = strInstall;
        _GetEquipment.InstallDate = string.Empty;

        string strDays = string.Empty;
        if (txtLastServiceDt.Text.Trim() != string.Empty)
        {
            if (ddlcompare.SelectedValue == "0")
                strDays = " = '" + txtLastServiceDt.Text.Trim();
            else if (ddlcompare.SelectedValue == "1")
                strDays = " >= '" + txtLastServiceDt.Text.Trim();
            else if (ddlcompare.SelectedValue == "2")
                strDays = " <= '" + txtLastServiceDt.Text.Trim();
            else if (ddlcompare.SelectedValue == "3")
                strDays = " > '" + txtLastServiceDt.Text.Trim();
            else if (ddlcompare.SelectedValue == "4")
                strDays = " < '" + txtLastServiceDt.Text.Trim();
            else
                strDays = " = '" + txtLastServiceDt.Text.Trim();
        }
        objProp_User.ServiceDate = string.Empty;
        objProp_User.ServiceDateString = strDays;

        _GetEquipment.ServiceDate = string.Empty;
        _GetEquipment.ServiceDateString = strDays;

        string strPrice = string.Empty;
        if (txtPrice.Text.Trim() != string.Empty)
        {
            if (ddlCompareP.SelectedValue == "0")
                strPrice = " = '" + txtPrice.Text.Trim();
            else if (ddlCompareP.SelectedValue == "1")
                strPrice = " >= '" + txtPrice.Text.Trim();
            else if (ddlCompareP.SelectedValue == "2")
                strPrice = " <= '" + txtPrice.Text.Trim();
            else if (ddlCompareP.SelectedValue == "3")
                strPrice = " > '" + txtPrice.Text.Trim();
            else if (ddlCompareP.SelectedValue == "4")
                strPrice = " < '" + txtPrice.Text.Trim();
            else
                strPrice = " = '" + txtPrice.Text.Trim();
        }
        objProp_User.Price = string.Empty;
        objProp_User.PriceString = strPrice;

        objProp_User.Manufacturer = txtManufact.Text;
        objProp_User.CustomerID = Convert.ToInt32(Session["custid"].ToString());

        _GetEquipment.Price = string.Empty;
        _GetEquipment.PriceString = strPrice;

        _GetEquipment.Manufacturer = txtManufact.Text;
        _GetEquipment.CustomerID = Convert.ToInt32(Session["custid"].ToString());
        if (Session["type"].ToString() == "c")
        {
            DataTable dtcust = new DataTable();
            dtcust = (DataTable)Session["userinfo"];
            int RoleID = 0;
            if (dtcust.Rows.Count > 0)
            {
                RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                objProp_User.RoleID = RoleID;
                _GetEquipment.RoleID = RoleID;
            }
        }
        objProp_User.Type = ddlFilterType.SelectedValue;
        objProp_User.Category = ddlFilterCategory.SelectedValue;
        objProp_User.Status = Convert.ToInt16(ddlFilterStatus.SelectedValue);
        objProp_User.building = ddlBuilding.SelectedValue;
        objProp_User.Classification = ddlFilterClassification.SelectedValue;

        _GetEquipment.Type = ddlFilterType.SelectedValue;
        _GetEquipment.Category = ddlFilterCategory.SelectedValue;
        _GetEquipment.Status = Convert.ToInt16(ddlFilterStatus.SelectedValue);
        _GetEquipment.building = ddlBuilding.SelectedValue;
        _GetEquipment.Classification = ddlFilterClassification.SelectedValue;

        #region Company Check
        if (Session["CmpChkDefault"].ToString() == "1")
        {
            objProp_User.EN = 1;
            _GetEquipment.EN = 1;
        }
        else
        {
            objProp_User.EN = 0;
            _GetEquipment.EN = 0;
        }
        #endregion

        //For filter by user login
        objProp_User.EmpId = EmpId;
        objProp_User.IsAssignedProject = isAssignProject;

        _GetEquipment.EmpId = EmpId;
        _GetEquipment.IsAssignedProject = isAssignProject;

        List<GetEquipmentViewModel> _lstGetEquipment = new List<GetEquipmentViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentsList_GetEquipment";

            _GetEquipment.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipment);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetEquipment = serializer.Deserialize<List<GetEquipmentViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetEquipmentViewModel>(_lstGetEquipment);
        }
        else
        {
            ds = objBL_User.GetEquipment(objProp_User, new GeneralFunctions().GetSalesAsigned());
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
                    if (ds.Tables[0].Select("Status = 'Active'").Count() > 0)
                    {
                        dtFinal = ds.Tables[0].Select("Status = 'Active'").CopyToDataTable();
                    }
                    else
                    {
                        dtFinal = createEmptyTable();
                    }

                }
                else
                {
                    dtFinal = ds.Tables[0];
                }
            }
        }

        DataTable result = processDataFilter(dtFinal);
        RadGrid_Equip.VirtualItemCount = result.Rows.Count;
        RadGrid_Equip.DataSource = result;

        updPanelAdvSrch.Update();
        // updpnl.Update();

        Session["ElevSrch"] = result;
        var eqIds = GetFilteredElevIds();
        Session["ElevFilteredIds"] = eqIds;

    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        isShowAll = 0;
        #region Search Filter
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_Equi"] = selectedValue;

        if (selectedValue == "e.Status")
        {
            Session["ddlSearch_Value_Equi"] = rbStatus.SelectedValue;
        }
        else if (selectedValue == "e.type")
        {
            Session["ddlSearch_Value_Equi"] = ddlType.SelectedValue;
        }
        else if (selectedValue == "e.Cat")
        {
            Session["ddlSearch_Value_Equi"] = ddlServiceType.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "B.Name")
        {
            Session["ddlSearch_Value_Equi"] = ddlCompany.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "tr.Name")
        {
            Session["ddlSearch_Value_Equi"] = ddlTerr.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "rt.Name")
        {

            var routeSearchValue = string.Empty;
            if (ddlRoute.CheckedItems.Count > 0)
            {
                foreach (var item in ddlRoute.CheckedItems)
                {
                    routeSearchValue += item.Value + ",";
                }

                routeSearchValue = routeSearchValue.TrimEnd(',');
            }

            objProp_User.SearchValue = routeSearchValue;
            Session["ddlSearch_Value_Equi"] = routeSearchValue;
        }
        else
        {
            Session["ddlSearch_Value_Equi"] = txtSearch.Text;
        }
        #endregion

        GetDataAll();
        RadGrid_Equip.Rebind();
        IsGridPageIndexChanged = false;
    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addequipment.aspx");
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Equip.Items)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                DeleteEquipment(Convert.ToInt32(lblUserID.Text));
            }
        }
    }

    private void DeleteEquipment(int EquipID)
    {
        objProp_User.EquipID = EquipID;
        objProp_User.ConnConfig = Session["config"].ToString();

        _DeleteEquipment.EquipID = EquipID;
        _DeleteEquipment.ConnConfig = Session["config"].ToString();

        try
        {
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentsList_DeleteEquipment";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteEquipment);
            }
            else
            {
                objBL_User.DeleteEquipment(objProp_User);
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Equipment deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            GetDataAll();

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            if(str == "The selected equipment is in use, cannot be deleted!")
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            else
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Equip.Items)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text + "&t=c");
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Equip.Items)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text);
            }
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ShowHideFilter();
    }
    private void ShowHideFilter()
    {

        if (ddlSearch.SelectedValue == "e.Status")
        {
            rbStatus.Style["display"] = "block";
            txtSearch.Style["display"] = "none";
            ddlType.Style["display"] = "none";
            ddlServiceType.Style["display"] = "none";
            ddlCompany.Style["display"] = "none";
        }
        else if (ddlSearch.SelectedValue == "e.type")
        {
            rbStatus.Style["display"] = "none";
            txtSearch.Style["display"] = "none";
            ddlType.Style["display"] = "block";
            ddlServiceType.Style["display"] = "none";
            ddlCompany.Style["display"] = "none";
        }
        else if (ddlSearch.SelectedValue == "e.Cat")
        {
            rbStatus.Style["display"] = "none";
            txtSearch.Style["display"] = "none";
            ddlType.Style["display"] = "none";
            ddlServiceType.Style["display"] = "block";
            ddlCompany.Style["display"] = "none";
        }
        else if (ddlSearch.SelectedValue == "B.Name")
        {
            rbStatus.Style["display"] = "none";
            txtSearch.Style["display"] = "none";
            ddlType.Style["display"] = "none";
            ddlServiceType.Style["display"] = "none";
            ddlCompany.Style["display"] = "block";
        }
        else if (ddlSearch.SelectedValue == "rt.Name")
        {
            rbStatus.Style["display"] = "none";
            txtSearch.Style["display"] = "none";
            ddlType.Style["display"] = "none";
            ddlServiceType.Style["display"] = "none";
            ddlRoute.Style["display"] = "block";
            ddlTerr.Style["display"] = "none";
        }
        else if (ddlSearch.SelectedValue == "tr.Name")
        {
            rbStatus.Style["display"] = "none";
            txtSearch.Style["display"] = "none";
            ddlType.Style["display"] = "none";
            ddlServiceType.Style["display"] = "none";
            ddlRoute.Style["display"] = "none";
            ddlTerr.Style["display"] = "block";
        }

        else
        {
            txtSearch.Style["display"] = "block";
            rbStatus.Style["display"] = "none";
            ddlType.Style["display"] = "none";
            ddlServiceType.Style["display"] = "none";
            ddlCompany.Style["display"] = "none";
        }

    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        isShowAll = 1;
        ResetFormControlValues(this);
        ddlSearch.SelectedIndex = 0;
        ddlType.SelectedIndex = 0;
        ddlServiceType.SelectedIndex = 0;
        rbStatus.SelectedIndex = 0;
        lnkChk.Checked = false;
        txtSearch.Text = string.Empty;
        txtManufact.Text = string.Empty;
        txtPrice.Text = string.Empty;
        txtInstallDt.Text = string.Empty;
        txtLastServiceDt.Text = string.Empty;
        //ddlSearch_SelectedIndexChanged(sender, e);
        cleanFilter();

        Session["ddlSearch_Equi"] = null;
        Session["ddlSearch_Value_Equi"] = null;
        Session["Equip_Filters"] = null;
        Session["Equip_FilterExpression"] = null;
        GetDataAll();
        IsGridPageIndexChanged = false;
        RadGrid_Equip.Rebind();
        updPanelAdvSrch.Update();
        // updpnl.Update();
        txtSearch.Style["display"] = "block";
        rbStatus.Style["display"] = "none";
        ddlType.Style["display"] = "none";
        ddlServiceType.Style["display"] = "none";
        ddlCompany.Style["display"] = "none";
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);
        ddlSearch_SelectedIndexChanged(sender, e);
    }

    protected void lnkDowntimeEquipmentReport_Click(object sender, EventArgs e)
    {
        var searchText = string.Empty;

        if (ddlSearch.SelectedValue == "e.Status")
        {
            searchText = rbStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "e.type")
        {
            searchText = ddlType.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "e.Cat")
        {
            searchText = ddlServiceType.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "B.Name")
        {
            searchText = ddlCompany.SelectedValue;
        }
        else
        {
            searchText = txtSearch.Text.Replace("'", "''");
        }

        string urlString = "DowntimeEquipmentReport.aspx?stype=" + HttpUtility.UrlEncode(ddlSearch.SelectedItem.Value) + "&stext=" + HttpUtility.UrlEncode(searchText) + "&inclInactive=" + lnkChk.Checked;

        Response.Redirect(urlString, true);
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
    protected void lnkQRCode_Click(object sender, EventArgs e)
    {
        Response.Redirect("qrprint.aspx?f=c");
    }
    protected void lnkMassMCP_Click(object sender, EventArgs e)
    {
        Response.Redirect("massmcp.aspx");
    }
    protected void lnkchk_Click(object sender, EventArgs e)
    {

        GetDataAll();
        RadGrid_Equip.Rebind();

    }
    protected void RadGrid_Equip_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Equip.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        GetDataAll();
    }
    protected void RadGrid_Equip_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_Equip.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Equip_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Equip.MasterTableView.OwnerGrid.Columns)
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

            Session["Equip_Filters"] = filters;
        }
        else
        {
            Session["Equip_FilterExpression"] = null;
            Session["Equip_Filters"] = null;
        }
        #endregion
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Equip);
        RowSelect();
    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Equip.MasterTableView.FilterExpression != "" ||
            (RadGrid_Equip.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Equip.MasterTableView.SortExpressions.Count > 0;
    }

    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_Equip.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            //Label lblUnit = (Label)gr.FindControl("lblUnit");
            HyperLink lnkUnit = (HyperLink)gr.FindControl("lnkUnit");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (hdnEditeEquipment.Value == "Y" || hdnViewEquipment.Value == "Y")
            {
                lnkUnit.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addequipment.aspx?uid=" + lblID.Text + "'";
            }
            else
            {
                lnkUnit.Attributes["onclick"] = gr.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }

            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gr.ClientID + "',event);";

        }
    }
    protected void RadGrid_Equip_ItemEvent(object sender, GridItemEventArgs e)
    {
        if (RadGrid_Equip.Items.Count > 0)
        {
            GridFooterItem footerItem = (GridFooterItem)RadGrid_Equip.MasterTableView.GetItems(GridItemType.Footer).FirstOrDefault();
            if (footerItem != null)
            {
                var dt = (DataTable)Session["ElevSrch"];
                var totalActive = dt.Select("Status = 'Active'").Count();
                var totalInActive = dt.Rows.Count - totalActive;

                Label lblTotalActive = footerItem.FindControl("lblTotalActive") as Label;
                Label lblTotalInActive = footerItem.FindControl("lblTotalInActive") as Label;
                lblTotalActive.Text = string.Format("Active: {0:N0}", totalActive);
                lblTotalInActive.Text = string.Format("Inactive: {0:N0}", totalInActive);
            }
        }
    }
    protected void RadGrid_Equip_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (Convert.ToString(RadGrid_Equip.MasterTableView.FilterExpression) != "")
        {
            lblRecordCount.Text = RadGrid_Equip.MasterTableView.DataSourceCount + " Record(s) found";
            RadGrid_Equip.VirtualItemCount = RadGrid_Equip.MasterTableView.DataSourceCount;
        }
        else
        {
            lblRecordCount.Text = RadGrid_Equip.VirtualItemCount + " Record(s) found";
        }
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

                if (Convert.ToString(RadGrid_Equip.MasterTableView.FilterExpression) != "")
                {
                    totalCount = RadGrid_Equip.MasterTableView.DataSourceCount;
                }


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
        ///RadGrid_Equip.MasterTableView.GetColumn("ClientSelectColumn").Visible = false;
       // RadGrid_Equip.MasterTableView.GetColumn("ImageQB").Visible = false;
        RadGrid_Equip.ExportSettings.FileName = "Equipments";
        RadGrid_Equip.ExportSettings.IgnorePaging = true;
        RadGrid_Equip.ExportSettings.ExportOnlyData = true;
        RadGrid_Equip.ExportSettings.OpenInNewWindow = true;
        RadGrid_Equip.ExportSettings.HideStructureColumns = true;
        RadGrid_Equip.MasterTableView.UseAllDataFields = true;
        // RadGrid_Equip.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");
        RadGrid_Equip.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Equip.MasterTableView.ExportToExcel();
    }
    protected void RadGrid_Equip_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Equip.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Equip.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

    private void FillEquipClassification()
    {
        Session["EquipClassificationLabel"] = null;
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        _GetEquipClassification.ConnConfig = Session["config"].ToString();

        List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentsList_GetEquipClassification";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipClassification);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
        }
        else
        {
            ds = objBL_User.getEquipClassification(objProp_User);
        }

        ddlFilterClassification.DataSource = ds.Tables[0];
        ddlFilterClassification.DataTextField = "edesc";
        ddlFilterClassification.DataValueField = "edesc";
        ddlFilterClassification.DataBind();
        ddlFilterClassification.Items.Insert(0, new ListItem("All", ""));
        lblClassification.Text = "Classification";
        //RadGrid_Equip.Columns[6].HeaderText = "Classification";
        RadGrid_Equip.Columns.FindByDataField("Classification").HeaderText = "Classification";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            Session["EquipClassificationLabel"] = ds.Tables[0].Rows[0]["Label"].ToString();
            lblClassification.Text = ds.Tables[0].Rows[0]["Label"].ToString();
            RadGrid_Equip.Columns.FindByDataField("Classification").HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    protected void RadGrid_Equip_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void lnkEqSdActReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("EquipmentShutdownReport.aspx?type=1&filtered=1");
    }

    private DataTable GetFilteredDataSource()
    {
        DataTable DT = new DataTable();
        DataTable FilteredDT = new DataTable();
        string filterexpression = string.Empty;
        filterexpression = RadGrid_Equip.MasterTableView.FilterExpression;
        DT = (DataTable)RadGrid_Equip.DataSource;
        if (!string.IsNullOrEmpty(filterexpression))
        {
            FilteredDT = DT.AsEnumerable()
                .AsQueryable()
                .Where(filterexpression)
                .CopyToDataTable();
        }
        else
        {
            FilteredDT = DT;
        }

        return FilteredDT;
    }

    private string GetFilteredElevIds()
    {
        DataTable DT = new DataTable();
        //DataTable FilteredDT = new DataTable();
        int[] FilteredDT;
        string elevIds = string.Empty;
        string filterexpression = string.Empty;
        filterexpression = RadGrid_Equip.MasterTableView.FilterExpression;
        DT = (DataTable)RadGrid_Equip.DataSource;
        if (!string.IsNullOrEmpty(filterexpression))
        {
            FilteredDT = DT.AsEnumerable()
                .AsQueryable()
                .Where(filterexpression)
                .Select(e => e.Field<int>(DT.Columns["id"])).ToArray();
        }
        else
        {
            FilteredDT = DT.AsEnumerable()
                .AsQueryable()
                .Select(e => e.Field<int>(DT.Columns["id"])).ToArray();
        }

        if (FilteredDT.Length > 0)
        {
            elevIds = string.Join(",", FilteredDT);
        }

        return elevIds;
    }

    private DataTable processDataFilter(DataTable dt)
    {
        DataTable result = dt;
        //if (!IsPostBack)
        //{
        try
        {
            String sql = "1=1";
            if (Session["Equip_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();


                if (Session["Equip_Filters"] != null)
                {


                    var filtersGet = Session["Equip_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_Equip.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            if (column.UniqueName == "ID")
                            {
                                sql = sql + " And " + column.UniqueName + " = " + _filter.FilterValue;

                            }
                            else
                            {
                                if (column.UniqueName == "last" || column.UniqueName == "Install")
                                {
                                    sql = sql + " And (" + column.UniqueName + " >= '" + _filter.FilterValue + " 00:00:00 AM' and " + column.UniqueName + " <= '" + _filter.FilterValue + " 23:59:59 PM' ) ";
                                }
                                else
                                {
                                    sql = sql + " And " + column.UniqueName + " like '%" + _filter.FilterValue + "%'";
                                }


                            }

                        }
                    }

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
        //}
        //else
        //{
        //    return dt;
        //}
    }

    private void SaveFilter()
    {

        Session["filterAdvanEquip"] = ddlFilterStatus.SelectedValue + ";"
            + ddlFilterType.SelectedValue + ";"
            + ddlFilterCategory.SelectedValue + ";"
            + ddlFilterClassification.SelectedValue + ";"
            + ddlBuilding.SelectedValue + ";"
            + txtManufact.Text + ";"
            + ddlcompare.SelectedValue + ";"
            + txtLastServiceDt.Text + ";"
            + ddlComareI.SelectedValue + ";"
            + txtInstallDt.Text + ";"
            + ddlCompareP.SelectedValue + ";"
            + txtPrice.Text + ";"
            + isShowAll.ToString() + ";"
            + lnkChk.Checked.ToString();
    }
    public void cleanFilter()
    {
        try
        {
            foreach (GridColumn column in RadGrid_Equip.MasterTableView.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            RadGrid_Equip.MasterTableView.FilterExpression = string.Empty;
            RadGrid_Equip.MasterTableView.Rebind();
        }
        catch (Exception ex)
        {

        }

        // RadGrid_Equip.Rebind();
    }
    private DataTable createEmptyTable()
    {
        DataTable DT = new DataTable();
        DT.Columns.Add("state", typeof(String));
        DT.Columns.Add("cat", typeof(String));
        DT.Columns.Add("category", typeof(String));
        DT.Columns.Add("Classification", typeof(String));
        DT.Columns.Add("manuf", typeof(String));
        DT.Columns.Add("price", typeof(double));
        DT.Columns.Add("last", typeof(DateTime));
        DT.Columns.Add("Install", typeof(DateTime));
        DT.Columns.Add("id", typeof(int));
        DT.Columns.Add("unit", typeof(String));
        DT.Columns.Add("type", typeof(String));
        DT.Columns.Add("fdesc", typeof(String));
        DT.Columns.Add("status", typeof(String));
        DT.Columns.Add("shut_down", typeof(String));
        DT.Columns.Add("ShutdownReason", typeof(String));
        DT.Columns.Add("building", typeof(String));
        DT.Columns.Add("EN", typeof(int));
        DT.Columns.Add("Company", typeof(String));
        DT.Columns.Add("name", typeof(String));
        DT.Columns.Add("locid", typeof(String));
        DT.Columns.Add("tag", typeof(String));
        DT.Columns.Add("address", typeof(String));
        DT.Columns.Add("Loc", typeof(int));
        DT.Columns.Add("unitid", typeof(int));
        return DT;
    }

    protected void RadGrid_Equip_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {

        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGrid_EquipCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_EquipminimumRows"] = e.NewPageIndex * RadGrid_Equip.PageSize;
            ViewState["RadGrid_EquipmaximumRows"] = (e.NewPageIndex + 1) * RadGrid_Equip.PageSize;
        }
        catch { }
    }

    protected void RadGrid_Equip_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGrid_EquipminimumRows"] = RadGrid_Equip.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_EquipmaximumRows"] = (RadGrid_Equip.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }
    private void FillRoute()
    {

        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getRoute(objProp_User, 1, 0, 0);
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "label";
        ddlRoute.DataValueField = "ID";
        ddlRoute.DataBind();
    }
    private void Fillterritory()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getTerritory(objProp_User, new GeneralFunctions().GetSalesAsigned());
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlTerr.DataSource = ds.Tables[0];
            ddlTerr.DataTextField = "Name";
            ddlTerr.DataValueField = "ID";
            ddlTerr.DataBind();
            ddlTerr.Items.Insert(0, new ListItem(":: Select ::", ""));

        }
    }
}
