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
using System.Collections;
using Telerik.Web.UI.GridExcelBuilder;
using System.Web;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Linq;
using BusinessEntity.APModels;
using BusinessEntity.Utility;
using MOMWebApp;
using BusinessEntity.CustomersModel;
using Newtonsoft.Json;
using System.Text;
using System.Xml;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;

public partial class Locations : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    BL_General objBL_General = new BL_General();
    General objGeneral = new General();
    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetCompanyByCustomerParam _GetCompanyByCustomer = new GetCompanyByCustomerParam();
    GetZoneParam _GetZone = new GetZoneParam();
    GetTerritoryParam _GetTerritory = new GetTerritoryParam();
    getCustomFieldsControlParam _getCustomFieldsControl = new getCustomFieldsControlParam();
    GetStockReportsParam _GetStockReports = new GetStockReportsParam();
    DeleteLocationParam _DeleteLocation = new DeleteLocationParam();
    GetLocationDataSearchParam _GetLocationDataSearch = new GetLocationDataSearchParam();
    GetLocationsDataParam _GetLocationsData = new GetLocationsDataParam();
    GetLocationTypeParam _GetLocationType = new GetLocationTypeParam();
    ImportDataForMassAttachDocumentsParam _ImportDataForMassAttachDocuments = new ImportDataForMassAttachDocumentsParam();
    GetDefaultWorkerHeaderParam _GetDefaultWorkerHeader = new GetDefaultWorkerHeaderParam();
    getlocationTypeParam _getlocationType = new getlocationTypeParam();
    GetRouteParam _GetRoute = new GetRouteParam();
    GetBTParam _GetBT = new GetBTParam();
    private bool IsGridPageIndexChanged = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        objProp_User.ConnConfig = Session["config"].ToString();

        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            FillCompany();
            Fillterritory();
            FillZone();
        }


        if (!IsPostBack)
        {
            FillSearchFilter();
            FillLocationType();
            FillRoute();
            FillBusinessType();
            FillBilling();
            FillCredit();
            FillDispAlert();
            FillPrintInvoice();
            FillEmailInvoice();
            FillNoCustomerStatement();
            FilMaint();
            FillStatus();
            #region Show Selected Filter
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["lnkChk_Location"] != null)
                {
                    lnkChk.Checked = Convert.ToBoolean(Session["lnkChk_Location"]);
                }
                if (Session["ddlSearch_Location"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearch_Location"]);
                    ddlSearch.SelectedValue = selectedValue;

                    String searchValue = Convert.ToString(Session["ddlSearch_Value_Location"]);
                    if (selectedValue.ToLower() == "l.status")
                    {
                        rbStatus.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "b.name")
                    {
                        ddlCompany.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "l.maint")
                    {
                        rbMaint.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "l.billing")
                    {
                        rbBilling.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "l.credit")
                    {
                        rbCredit.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "l.credit")
                    {
                        rbCredit.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "l.emailinvoice")
                    {
                        rbEmailInvoice.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "l.printinvoice")
                    {
                        rbPrintInvoice.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "l.dispalert")
                    {
                        rbDispAlert.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "l.terr")
                    {
                        ddlTerr.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "l.terr2")
                    {
                        ddlTerr2.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "l.nocustomerstatement")
                    {
                        rbNoCustomerStatement.SelectedValue = searchValue;
                    }
                    else if (selectedValue.ToLower() == "l.zone")
                    {
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            var selectedValArr = searchValue.Split(',');
                            foreach (RadComboBoxItem item in ddlZone1.Items)
                            {
                                if (Array.IndexOf(selectedValArr, item.Value) >= 0)
                                {
                                    item.Checked = true;
                                }
                            }
                        }

                    }
                    else if (selectedValue.ToLower() == "l.type")
                    {
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            var selectedValArr = searchValue.Split(',');
                            foreach (RadComboBoxItem item in ddlUserType.Items)
                            {
                                if (Array.IndexOf(selectedValArr, item.Value) >= 0)
                                {
                                    item.Checked = true;
                                }
                            }
                        }

                    }
                    else if (selectedValue.ToLower() == "rt.name")
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
                    else if (selectedValue.ToLower() == "l.businesstype")
                    {
                        ddlBusinessType.SelectedValue = searchValue;
                    }
                    else
                    {
                        txtSearch.Text = searchValue;
                    }
                }
                IsGridPageIndexChanged = true;

                showFilter();
            }
            else
            {
                Session.Remove("ddlSearch_Location");
                Session.Remove("ddlSearch_Value_Location");
                Session.Remove("FilterConditions");
                Session.Remove("Location_FilterExpression");
                Session.Remove("Location_Filters");
                Session.Remove("Loc_TypeFilters");
                Session.Remove("lnkChk_Location");
            }
            #endregion

            lnkLocationBusinessType.Text = string.Format("Location by {0} Report", getBusinessTypeLabel());

            HighlightSideMenu("cstmMgr", "lnkLocationsSMenu", "cstmMgrSub");

            if (Session["type"].ToString() != "c")
            {
                GetPreferences();
            }
        }

        Permission();
        ConvertToJSON();
        CompanyPermission();
        GridFilterFunction.IllegalStrings = new string[] { " LIKE , AND , \",>,<,<>,@, !,-, NULL , IS ,#,$,%,&,',(,),*,+,-,.,/,:,;,<,=,>,?,@,[,],^,_,`,{,|,},~" };
        RadGrid_Location.EnableLinqExpressions = false;
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

    protected void Page_Init(object sender, EventArgs e)
    {

    }

    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();

        //API
        _GetCompanyByCustomer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetCompanyByCustomer.DBName = Session["dbname"].ToString();
        _GetCompanyByCustomer.ConnConfig = Session["config"].ToString();

        DataSet dc = new DataSet();

        List<CompanyOfficeViewModel> _lstCompanyOffice = new List<CompanyOfficeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetCompanyByCustomer";

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

    private void FillZone()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        _GetZone.ConnConfig = Session["config"].ToString();

        List<GetZoneViewModel> _lstGetZone = new List<GetZoneViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetZone";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetZone);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetZone = serializer.Deserialize<List<GetZoneViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetZoneViewModel>(_lstGetZone);
        }
        else
        {
            ds = objBL_User.getZone(objProp_User);
        }

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlZone1.DataSource = ds.Tables[0];
            ddlZone1.DataTextField = "Name";
            ddlZone1.DataValueField = "ID";
            ddlZone1.DataBind();
            //ddlZone1.Items.Insert(0, new RadComboBoxItem(":: Select ::", ""));
        }
    }

    private void Fillterritory()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        _GetTerritory.ConnConfig = Session["config"].ToString();

        List<GetTerritoryViewModel> _lstGetTerritory = new List<GetTerritoryViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetTerritory";

            _GetTerritory.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetTerritory);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetTerritory = serializer.Deserialize<List<GetTerritoryViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetTerritoryViewModel>(_lstGetTerritory);
        }
        else
        {
            ds = objBL_User.getTerritory(objProp_User, new GeneralFunctions().GetSalesAsigned());
        }

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlTerr.DataSource = ds.Tables[0];
            ddlTerr.DataTextField = "Name";
            ddlTerr.DataValueField = "ID";
            ddlTerr.DataBind();
            ddlTerr.Items.Insert(0, new ListItem(":: Select ::", ""));
            // Second Salesperson  
            ddlTerr2.DataSource = ds.Tables[0];
            ddlTerr2.DataTextField = "Name";
            ddlTerr2.DataValueField = "ID";
            ddlTerr2.DataBind();
            ddlTerr2.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
    }

    private void Permission()
    {

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() != "am")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];

            //string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
            //if (ProgFunc == "N")
            //{
            //    Response.Redirect("home.aspx");
            //}
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
            //Location
            string LocationPermission = ds.Rows[0]["Location"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Location"].ToString();
            hdnAddeLoc.Value = LocationPermission.Length < 1 ? "Y" : LocationPermission.Substring(0, 1);
            hdnEditeLoc.Value = LocationPermission.Length < 2 ? "Y" : LocationPermission.Substring(1, 1);
            hdnDeleteLoc.Value = LocationPermission.Length < 3 ? "Y" : LocationPermission.Substring(2, 1);
            hdnViewLoc.Value = LocationPermission.Length < 4 ? "Y" : LocationPermission.Substring(3, 1);

            if (hdnAddeLoc.Value == "N")
            {
                lnkAddnew.Visible = false;
                btnCopy.Visible = false;
            }
            if (hdnEditeLoc.Value == "N")
            {
                btnEdit.Visible = false;

            }
            if (hdnDeleteLoc.Value == "N")
            {
                btnDelete.Visible = false;

            }
            if (hdnViewLoc.Value == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
        }
        else
        {
            hdnAddeLoc.Value = "Y";
            hdnEditeLoc.Value = "Y";
            hdnDeleteLoc.Value = "Y";
            hdnViewLoc.Value = "Y";
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

    private void RowSelect()
    {
        DataTable dtLocID = new DataTable();
        dtLocID.Columns.Add("loc");
        DataRow dr = null;
        foreach (GridDataItem gr in RadGrid_Location.Items)
        {
            Label lblUserID = (Label)gr.FindControl("lblloc");
            HiddenField hdnSelected = (HiddenField)gr.FindControl("hdnSelected");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkName");
            dr = dtLocID.NewRow();
            //add values to each rows
            dr["loc"] = lblUserID.Text;
            //add the row to DataTable
            dtLocID.Rows.Add(dr);
            if (hdnEditeLoc.Value == "Y" || hdnViewLoc.Value == "Y")
            {
                lnkName.NavigateUrl = "addlocation.aspx?uid=" + lblUserID.Text;
                lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "window.open('addlocation.aspx?uid=" + lblUserID.Text + "','_self');";
            }
            else
            {
                lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }

        }
        Session["locationsId"] = dtLocID;
    }

    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportsName());
        string reports = "var reports=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", reports, true);
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            //RadGrid_Location.Columns[11].Visible = true;            
            RadGrid_Location.MasterTableView.Columns.FindByUniqueName("Company").Visible = true;
            ddlSearch.Items.FindByValue("B.Name").Enabled = true;

        }
        else
        {

            RadGrid_Location.MasterTableView.Columns.FindByUniqueName("Company").Visible = false;
            ddlSearch.Items.FindByValue("B.Name").Enabled = false;
        }

        var isQBSyncIntegrated = IsQBSyncIntegrated();
        if (isQBSyncIntegrated)
        {
            RadGrid_Location.MasterTableView.Columns.FindByUniqueName("ImageQB").Visible = true;
        }
        else
        {
            RadGrid_Location.MasterTableView.Columns.FindByUniqueName("ImageQB").Visible = false;
        }

        var isSageSyncIntegrated = IsSageSyncIntegrated();
        if (isSageSyncIntegrated)
        {
            RadGrid_Location.MasterTableView.Columns.FindByUniqueName("sageid").Visible = true;
        }
        else
        {
            RadGrid_Location.MasterTableView.Columns.FindByUniqueName("sageid").Visible = false;
        }

        if (IsAllowMassAttachDocumments())
        {
            lnkMassAttachDocuments.Visible = true;
        }
        else
        {
            lnkMassAttachDocuments.Visible = false;
        }
        if (isShowZone())
        {
            RadGrid_Location.MasterTableView.Columns.FindByUniqueName("Zone").Visible = true;
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

    private Boolean isShowZone()
    {
        Boolean flag = false;
        objGeneral.ConnConfig = Session["config"].ToString();
        _getCustomFieldsControl.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = new DataSet();
        List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetCustomFieldsControl";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFieldsControl);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstCustomViewModel = serializer.Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
        }
        else
        {
            _dsCustom = objBL_General.getCustomFieldsControl(objGeneral);

        }
        if (_dsCustom.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            {
                if (_dr["Name"].ToString().Equals("Zone"))
                {
                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() == "1")
                    {
                        flag = true;
                    }

                }

            }
        }
        return flag;
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
            objProp_User.Type = "Location";

            _GetStockReports.DBName = Session["dbname"].ToString();
            _GetStockReports.ConnConfig = Session["config"].ToString();
            _GetStockReports.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _GetStockReports.Type = "Location";

            List<CustomerReportViewModel> _lstCustomerReport = new List<CustomerReportViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/LocationsList_GetStockReports";

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

    protected void btnCustEdit_Click(object sender, EventArgs e)
    {
        if (hdnEditeLoc.Value == "Y")
        {
            foreach (GridDataItem item in RadGrid_Location.SelectedItems)
            {
                Label lblUserID = (Label)item.Cells[1].FindControl("lblloc");
                Response.Redirect("addlocation.aspx?uid=" + lblUserID.Text);
            }
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        if (hdnAddeLoc.Value == "Y")
        {
            foreach (GridDataItem item in RadGrid_Location.SelectedItems)
            {
                Label lblUserID = (Label)item.Cells[1].FindControl("lblloc");
                Response.Redirect("addlocation.aspx?uid=" + lblUserID.Text + "&t=c");
            }
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (hdnDeleteLoc.Value == "Y")
        {
            foreach (GridDataItem item in RadGrid_Location.SelectedItems)
            {
                Label lblUserID = (Label)item.Cells[1].FindControl("lblloc");
                DeleteLocation(Convert.ToInt32(lblUserID.Text));
            }
        }
    }

    private void DeleteLocation(int LocID)
    {
        objProp_User.LocID = LocID;
        objProp_User.ConnConfig = Session["config"].ToString();

        _DeleteLocation.LocID = LocID;
        _DeleteLocation.ConnConfig = Session["config"].ToString();

        try
        {
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/LocationsList_DeleteLocation";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteLocation);
            }
            else
            {
                objBL_User.DeleteLocation(objProp_User);
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Location deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            GetLocationList();
            RadGrid_Location.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    private void showFilter()
    {
        div_rbStatus.Style.Add("display", "none");
        div_txtSearch.Style.Add("display", "none");
        div_ddlCompany.Style.Add("display", "none");
        div_rbBilling.Style.Add("display", "none");
        div_rbCredit.Style.Add("display", "none");
        div_rbDispAlert.Style.Add("display", "none");
        div_rbEmailInvoice.Style.Add("display", "none");
        div_rbPrintInvoice.Style.Add("display", "none");
        div_ddlTerr.Style.Add("display", "none");
        div_ddlTerr2.Style.Add("display", "none");
        div_rbNoCustomerStatement.Style.Add("display", "none");
        div_ddlZone1.Style.Add("display", "none");
        div_ddlRoute.Style.Add("display", "none");
        div_ddlBusinessType.Style.Add("display", "none");
        div_ddlUserType.Style.Add("display", "none");


        switch (ddlSearch.SelectedValue.ToLower())
        {
            case "l.status":
                div_rbStatus.Style.Add("display", "block");
                break;
            case "r.name":
                div_ddlCompany.Style.Add("display", "block");
                break;
            case "l.maint":
                div_rbMaint.Style.Add("display", "block");
                break;
            case "l.billing":
                div_rbBilling.Style.Add("display", "block");
                break;
            case "l.credit":
                div_rbCredit.Style.Add("display", "block");
                break;
            case "l.emailinvoice":
                div_rbEmailInvoice.Style.Add("display", "block");
                break;
            case "l.printinvoice":
                div_rbPrintInvoice.Style.Add("display", "block");
                break;
            case "l.dispalert":
                div_rbDispAlert.Style.Add("display", "block");
                break;
            case "l.terr":
                div_ddlTerr.Style.Add("display", "block");
                break;
            case "l.terr2":
                div_ddlTerr2.Style.Add("display", "block");
                break;
            case "l.nocustomerstatement":
                div_rbNoCustomerStatement.Style.Add("display", "block");
                break;
            case "l.zone":
                div_ddlZone1.Style.Add("display", "block");
                break;
            case "l.type":
                div_ddlUserType.Style.Add("display", "block");
                break;
            case "rt.name":
                div_ddlRoute.Style.Add("display", "block");
                break;
            case "l.businesstype":
                div_ddlBusinessType.Style.Add("display", "block");
                break;
            default:
                div_txtSearch.Style.Add("display", "block");
                break;
        }

    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  txtSearch.Text = string.Empty;
        // VisibleSearchDropdownBySearchField(ddlSearch.SelectedValue);
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        #region Search Filter
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_Location"] = selectedValue;
        Session["lnkChk_Location"] = lnkChk.Checked;

        ArrayList criteria = new ArrayList();
        if (Session["FilterConditions"] != null)
            criteria = (ArrayList)Session["FilterConditions"];

        switch (selectedValue.ToLower())
        {
            case "l.status":
                Session["ddlSearch_Value_Location"] = rbStatus.SelectedValue;
                break;
            case "b.name":
                Session["ddlSearch_Value_Location"] = ddlCompany.SelectedItem.Text.Trim();
                break;
            case "l.maint":
                Session["ddlSearch_Value_Location"] = rbMaint.SelectedValue;
                break;
            case "l.billing":
                Session["ddlSearch_Value_Location"] = rbBilling.SelectedValue;
                break;
            case "l.credit":
                Session["ddlSearch_Value_Location"] = rbCredit.SelectedValue;
                break;
            case "l.emailinvoice":
                Session["ddlSearch_Value_Location"] = rbEmailInvoice.SelectedValue;
                break;
            case "l.printinvoice":
                Session["ddlSearch_Value_Location"] = rbPrintInvoice.SelectedValue;
                break;
            case "l.dispalert":
                Session["ddlSearch_Value_Location"] = rbDispAlert.SelectedValue;
                break;
            case "l.terr":
                Session["ddlSearch_Value_Location"] = ddlTerr.SelectedValue;
                break;
            case "l.terr2":
                Session["ddlSearch_Value_Location"] = ddlTerr2.SelectedValue;
                break;
            case "l.nocustomerstatement":
                Session["ddlSearch_Value_Location"] = rbNoCustomerStatement.SelectedValue;
                break;
            case "l.zone":
                var zoneSearchValue = string.Empty;
                if (ddlZone1.CheckedItems.Count > 0)
                {
                    foreach (var item in ddlZone1.CheckedItems)
                    {
                        zoneSearchValue += item.Value + ",";
                    }

                    zoneSearchValue = zoneSearchValue.TrimEnd(',');
                }

                objProp_User.SearchValue = zoneSearchValue;
                Session["ddlSearch_Value_Location"] = zoneSearchValue;
                break;
            case "l.type":
                var typeSearchValue = string.Empty;
                if (ddlUserType.CheckedItems.Count > 0)
                {
                    foreach (var item in ddlUserType.CheckedItems)
                    {
                        typeSearchValue += item.Value + ",";
                    }

                    typeSearchValue = typeSearchValue.TrimEnd(',');
                }

                objProp_User.SearchValue = typeSearchValue;
                Session["ddlSearch_Value_Location"] = typeSearchValue;
                break;
            case "rt.name":
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
                Session["ddlSearch_Value_Location"] = routeSearchValue;
                break;
            case "l.businesstype":

                Session["ddlSearch_Value_Location"] = ddlBusinessType.SelectedValue;
                break;

            default:
                Session["ddlSearch_Value_Location"] = txtSearch.Text;
                break;
        }
        #endregion

        GetLocationList();
        RadGrid_Location.Rebind();
    }

    protected void lnkchk_Click(object sender, EventArgs e)
    {
        Session["lnkChk_Location"] = lnkChk.Checked;
        GetLocationList();
        addPreferences();
        GetPreferences();
        RadGrid_Location.Rebind();
    }

    //Add Preference for User Selection
    public void GetPreferences()
    {

        DataSet ds = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objProp_User.PreferenceID = 1;
        objProp_User.PageID = 3;
        ds = objBL_User.getPreferences(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            string st = ds.Tables[0].Rows[0]["Preferencevalue"] == DBNull.Value ? "0" : ds.Tables[0].Rows[0]["Preferencevalue"].ToString();
            lnkChk.Checked = st == "1" ? true : false;
        }
        else { lnkChk.Checked = false; }

    }
    public void addPreferences()
    {
        try
        {
            int val = lnkChk.Checked ? 1 : 0;
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objProp_User.PreferenceID = 1;
            objProp_User.PageID = 3;
            objProp_User.PreferenceValues = val;
            objBL_User.AddPreferences(objProp_User);
        }
        catch { }
    }
    //

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        ddlZone1.ClearCheckedItems();
        ddlUserType.ClearCheckedItems();
        ddlRoute.ClearCheckedItems();
        lnkChk.Checked = false;

        foreach (GridColumn column in RadGrid_Location.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
            column.ListOfFilterValues = null;
        }

        Session.Remove("ddlSearch_Location");
        Session.Remove("ddlSearch_Value_Location");
        Session.Remove("FilterConditions");
        Session.Remove("Location_FilterExpression");
        Session.Remove("Location_Filters");
        Session.Remove("Loc_TypeFilters");
        Session.Remove("lnkChk_Location");
        RadGrid_Location.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Location.CurrentPageIndex = 0;
        RadGrid_Location.PageSize = 50;
        RadGrid_Location.MasterTableView.PageSize = 50;
        RadGrid_Location.MasterTableView.CurrentPageIndex = 0;
        RadGrid_Location.Rebind();
    }

    protected void RadGrid_Location_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //RadGrid_Location.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
            #region Set the Grid Filters
            if (!IsPostBack)
            {
                // Set filter for grid column except Department and Route
                if (Session["Location_FilterExpression"] != null
                    && Convert.ToString(Session["Location_FilterExpression"]) != ""
                    && Session["Location_Filters"] != null)
                {
                    RadGrid_Location.MasterTableView.FilterExpression = Convert.ToString(Session["Location_FilterExpression"]);
                    var filtersGet = Session["Location_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            var filterCol = _filter.FilterColumn;

                            if (filterCol == "Types")
                            {
                                GridColumn column = RadGrid_Location.MasterTableView.GetColumnSafe("Type");

                                if (column != null)
                                {
                                    column.ListOfFilterValues = _filter.FilterValue.Replace("'", "").Split(',');
                                }
                            }
                            else
                            {
                                GridColumn column = RadGrid_Location.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                                if (column != null)
                                {
                                    column.CurrentFilterValue = _filter.FilterValue;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                #region Save the Grid Filter
                String filterExpression = Convert.ToString(RadGrid_Location.MasterTableView.FilterExpression);
                if (filterExpression == "")
                {
                    filterExpression = Session["Location_FilterExpression"] != null ? Session["Location_FilterExpression"].ToString() : "";
                }

                if (filterExpression != "")
                {
                    Session["Location_FilterExpression"] = filterExpression;
                    List<RetainFilter> filters = new List<RetainFilter>();

                    foreach (GridColumn column in RadGrid_Location.MasterTableView.OwnerGrid.Columns)
                    {
                        String filterValues = String.Empty;
                        String columnName = column.UniqueName;

                        if (column.UniqueName == "Type")
                        {
                            if (column.ListOfFilterValues != null)
                            {
                                List<string> listFil = new List<string>(column.ListOfFilterValues);
                                filterValues = String.Join(",", listFil.Select(x => string.Format("'{0}'", x)));
                                columnName = "Types";
                            }
                            else
                            {
                                filterValues = column.CurrentFilterValue;
                            }
                        }
                        else
                        {
                            filterValues = column.CurrentFilterValue;
                        }

                        if (filterValues != "")
                        {
                            RetainFilter filter = new RetainFilter();
                            filter.FilterColumn = columnName;
                            filter.FilterValue = filterValues;
                            filters.Add(filter);
                        }
                    }

                    Session["Location_Filters"] = filters;
                }
                else
                {
                    Session.Remove("Location_FilterExpression");
                    Session.Remove("Location_Filters");
                }
                #endregion
            }

            // Reset filter for Department column
            if (Session["Loc_TypeFilters"] != null && !string.IsNullOrEmpty(Session["Loc_TypeFilters"].ToString()))
            {
                var strTypeFilters = Session["Loc_TypeFilters"].ToString();
                string[] typeItems = strTypeFilters.Split(',');

                GridColumn typeColumn = RadGrid_Location.MasterTableView.GetColumn("Type");
                typeColumn.ListOfFilterValues = typeItems;
            }
            #endregion
        }
        catch { }

        GetLocationList();
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Location.MasterTableView.FilterExpression != "" ||
            (RadGrid_Location.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Location.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Location_PreRender(object sender, EventArgs e)
    {
        //Rename column Header
        RadGrid_Location.Columns.FindByUniqueName("RouteName").HeaderText = getRouteLabel();

        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_Location.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Location_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Location.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = String.Empty;
                String columnName = column.UniqueName;

                if (column.UniqueName == "Type")
                {
                    if (column.ListOfFilterValues != null)
                    {
                        List<string> listFil = new List<string>(column.ListOfFilterValues);
                        filterValues = String.Join(",", listFil.Select(x => string.Format("'{0}'", x)));
                        columnName = "Types";
                    }
                    else
                    {
                        filterValues = column.CurrentFilterValue;
                    }
                }
                else
                {
                    filterValues = column.CurrentFilterValue;
                }

                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Location_Filters"] = filters;
        }
        else
        {
            Session.Remove("Location_FilterExpression");
            Session.Remove("Location_Filters");
        }
        #endregion
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Location);
        RowSelect();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        ddlZone1.ClearCheckedItems();
        ddlUserType.ClearCheckedItems();
        ddlRoute.ClearCheckedItems();
        lnkChk.Checked = false;

        foreach (GridColumn column in RadGrid_Location.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
            column.ListOfFilterValues = null;
        }

        Session.Remove("ddlSearch_Location");
        Session.Remove("ddlSearch_Value_Location");
        Session.Remove("FilterConditions");
        Session.Remove("Location_FilterExpression");
        Session.Remove("Location_Filters");
        Session.Remove("Loc_TypeFilters");
        Session.Remove("lnkChk_Location");
        RadGrid_Location.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Location.Rebind();
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

    private void GetLocationList()
    {
        DataSet ds = new DataSet();
        DataTable dtFinal = new DataTable();

        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.DBName = Session["dbname"].ToString();

        _GetLocationDataSearch.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetLocationDataSearch.ConnConfig = Session["config"].ToString();
        _GetLocationDataSearch.DBName = Session["dbname"].ToString();

        _GetLocationsData.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetLocationsData.ConnConfig = Session["config"].ToString();
        _GetLocationsData.DBName = Session["dbname"].ToString();

        #region Company Check
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objProp_User.EN = 1;
            _GetLocationDataSearch.EN = 1;
            _GetLocationsData.EN = 1;
        }
        else
        {
            objProp_User.EN = 0;
            _GetLocationDataSearch.EN = 0;
            _GetLocationsData.EN = 0;
        }
        objProp_User.inclInactive = lnkChk.Checked;
        _GetLocationDataSearch.inclInactive = lnkChk.Checked;
        _GetLocationsData.inclInactive = lnkChk.Checked;


        #endregion

        if (ddlSearch.SelectedIndex != 0)
        {
            objProp_User.SearchBy = ddlSearch.SelectedValue;
            _GetLocationDataSearch.SearchBy = ddlSearch.SelectedValue;
            _GetLocationsData.SearchBy = ddlSearch.SelectedValue;

            switch (ddlSearch.SelectedValue.ToLower())
            {
                case "l.status":
                    objProp_User.SearchValue = rbStatus.SelectedValue;
                    _GetLocationDataSearch.SearchValue = rbStatus.SelectedValue;
                    _GetLocationsData.SearchValue = rbStatus.SelectedValue;
                    break;
                case "b.name":
                    objProp_User.SearchValue = ddlCompany.SelectedValue;
                    _GetLocationDataSearch.SearchValue = ddlCompany.SelectedValue;
                    _GetLocationsData.SearchValue = ddlCompany.SelectedValue;
                    break;
                case "l.maint":
                    objProp_User.SearchValue = rbMaint.SelectedValue;
                    _GetLocationDataSearch.SearchValue = rbMaint.SelectedValue;
                    _GetLocationsData.SearchValue = rbMaint.SelectedValue;

                    break;
                case "l.billing":
                    objProp_User.SearchValue = rbBilling.SelectedValue;
                    _GetLocationDataSearch.SearchValue = rbBilling.SelectedValue;
                    _GetLocationsData.SearchValue = rbBilling.SelectedValue;
                    break;
                case "l.credit":
                    objProp_User.SearchValue = rbCredit.SelectedValue;
                    _GetLocationDataSearch.SearchValue = rbCredit.SelectedValue;
                    _GetLocationsData.SearchValue = rbCredit.SelectedValue;
                    break;
                case "l.emailinvoice":
                    objProp_User.SearchValue = rbEmailInvoice.SelectedValue;
                    _GetLocationDataSearch.SearchValue = rbEmailInvoice.SelectedValue;
                    _GetLocationsData.SearchValue = rbEmailInvoice.SelectedValue;
                    break;
                case "l.printinvoice":
                    objProp_User.SearchValue = rbPrintInvoice.SelectedValue;
                    _GetLocationDataSearch.SearchValue = rbPrintInvoice.SelectedValue;
                    _GetLocationsData.SearchValue = rbPrintInvoice.SelectedValue;
                    break;
                case "l.dispalert":
                    objProp_User.SearchValue = rbDispAlert.SelectedValue;
                    _GetLocationDataSearch.SearchValue = rbDispAlert.SelectedValue;
                    _GetLocationsData.SearchValue = rbDispAlert.SelectedValue;
                    break;
                case "l.terr":
                    objProp_User.SearchValue = ddlTerr.SelectedValue;
                    _GetLocationDataSearch.SearchValue = ddlTerr.SelectedValue;
                    _GetLocationsData.SearchValue = ddlTerr.SelectedValue;
                    break;
                case "l.terr2":
                    objProp_User.SearchValue = ddlTerr2.SelectedValue;
                    _GetLocationDataSearch.SearchValue = ddlTerr2.SelectedValue;
                    _GetLocationsData.SearchValue = ddlTerr2.SelectedValue;
                    break;
                case "l.nocustomerstatement":
                    objProp_User.SearchValue = rbNoCustomerStatement.SelectedValue;
                    _GetLocationDataSearch.SearchValue = rbNoCustomerStatement.SelectedValue;
                    _GetLocationsData.SearchValue = rbNoCustomerStatement.SelectedValue;
                    break;
                case "l.zone":
                    var zoneSearchValue = string.Empty;
                    if (ddlZone1.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlZone1.CheckedItems)
                        {
                            zoneSearchValue += item.Value + ",";
                        }

                        zoneSearchValue = zoneSearchValue.TrimEnd(',');
                    }

                    objProp_User.SearchValue = zoneSearchValue;
                    _GetLocationDataSearch.SearchValue = zoneSearchValue;
                    _GetLocationsData.SearchValue = zoneSearchValue;
                    break;
                case "l.type":
                    var typeSearchValue = string.Empty;
                    if (ddlUserType.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlUserType.CheckedItems)
                        {
                            typeSearchValue += item.Value + ",";
                        }

                        typeSearchValue = typeSearchValue.TrimEnd(',');
                    }

                    objProp_User.SearchValue = typeSearchValue;
                    _GetLocationDataSearch.SearchValue = typeSearchValue;
                    _GetLocationsData.SearchValue = typeSearchValue;
                    break;
                case "rt.name":
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
                    _GetLocationDataSearch.SearchValue = routeSearchValue;
                    _GetLocationsData.SearchValue = routeSearchValue;
                    break;
                case "l.businesstype":
                    objProp_User.SearchValue = ddlBusinessType.SelectedValue;
                    _GetLocationDataSearch.SearchValue = ddlBusinessType.SelectedValue;
                    _GetLocationsData.SearchValue = ddlBusinessType.SelectedValue;
                    break;
                default:
                    objProp_User.SearchValue = txtSearch.Text.Replace("'", "''");
                    _GetLocationDataSearch.SearchValue = txtSearch.Text.Replace("'", "''");
                    _GetLocationsData.SearchValue = txtSearch.Text.Replace("'", "''");
                    break;
            }

            List<GetLocationDataSearchViewModel> _lstGetLocationDataSearch = new List<GetLocationDataSearchViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/LocationsList_GetLocationDataSearch";

                _GetLocationDataSearch.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationDataSearch);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetLocationDataSearch = serializer.Deserialize<List<GetLocationDataSearchViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetLocationDataSearchViewModel>(_lstGetLocationDataSearch);
            }
            else
            {
                ds = objBL_User.getLocationDataSearch(objProp_User, new GeneralFunctions().GetSalesAsigned());
            }

        }
        else
        {
            List<GetLocationDataSearchViewModel> _lstGetLocationData = new List<GetLocationDataSearchViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/LocationsList_GetLocationsData";

                _GetLocationDataSearch.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationsData);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetLocationData = serializer.Deserialize<List<GetLocationDataSearchViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetLocationDataSearchViewModel>(_lstGetLocationData);
            }
            else
            {
                ds = objBL_User.getLocationsData(objProp_User, new GeneralFunctions().GetSalesAsigned());
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
                    if (lnkChk.Checked == false && rbStatus.SelectedValue == "1")
                        dtFinal = ds.Tables[0].Select("Status = '1'").CopyToDataTable();
                    else
                        dtFinal = ds.Tables[0].Select("Status = '0'").CopyToDataTable();
                }
                else
                {
                    dtFinal = ds.Tables[0];
                }
            }
        }

        DataTable result = processDataFilter(dtFinal);
        Session["locations"] = result;
        RadGrid_Location.VirtualItemCount = result.Rows.Count;
        RadGrid_Location.DataSource = result;

        // Filter Expressio

        if (!IsGridPageIndexChanged)
        {
            //RadGrid_Location.CurrentPageIndex = 0;
            //RadGrid_Location.PageSize = 50;
            //Session["RadGrid_LocationCurrentPageIndex"] = 0;
            //ViewState["RadGrid_LocationminimumRows"] = 0;
            //ViewState["RadGrid_LocationmaximumRows"] = RadGrid_Location.PageSize;


        }
        else
        {
            if (Session["RadGrid_LocationCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_LocationCurrentPageIndex"].ToString()) != 0
              )
            {
                //RadGrid_Location.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_LocationCurrentPageIndex"].ToString());
                //ViewState["RadGrid_LocationminimumRows"] = RadGrid_Location.CurrentPageIndex * RadGrid_Location.PageSize;
                //ViewState["RadGrid_LocationmaximumRows"] = (RadGrid_Location.CurrentPageIndex + 1) * RadGrid_Location.PageSize;

            }
        }


        if (Request.QueryString["f"] != "c")
        {
            if (Session["Loaction_FilterExpression"] != null && Convert.ToString(Session["Loaction_FilterExpression"]) != "" && Session["LocationListRadGVFilters"] != null)
            {
                var filterExpression = Convert.ToString(Session["Loaction_FilterExpression"]);
                RadGrid_Location.MasterTableView.FilterExpression = filterExpression;
                var filtersGet = Session["LocationListRadGVFilters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Location.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }

                RadGrid_Location.Rebind();
            }
        }
        else
        {
            Session.Remove("Location_FilterExpression");
            Session.Remove("Location_Filters");
        }
    }

    protected void lnkCustomReports_Click(object sender, EventArgs e)
    {
        ArrayList criteria = new ArrayList();
        ArrayList columns = new ArrayList();
        if (Session["FilterConditions"] != null)
            criteria = (ArrayList)Session["FilterConditions"];

        var condition = "";
        foreach (GridFilteringItem gfi in RadGrid_Location.MasterTableView.GetItems(GridItemType.FilteringItem))
        {
            foreach (GridColumn column in RadGrid_Location.MasterTableView.Columns)
            {
                if (gfi[column.UniqueName].Controls.Count > 0)
                {
                    var ctrl = (TextBox)gfi[column.UniqueName].Controls[0];
                    var val = ctrl.Text;
                    if (!column.UniqueName.Equals("B.Name"))
                        columns.Add(column.UniqueName);
                    if (!string.IsNullOrEmpty(ctrl.Text))
                    {
                        condition = column.UniqueName + ",Contains," + ctrl.Text.Trim();
                        if (!criteria.Contains(condition))
                            criteria.Add(condition);
                    }
                }
            }
        }

        for (int i = 0; i < criteria.Count; i++)
        {
            var _condition = criteria[i];
            var filtered = _condition.ToString().Split(',');
            var columnName = filtered[0];
            if (!columns.Contains(columnName))
                columns.Add(columnName);

        }
        Session["Module"] = "Locations";
        Session["ChildTables"] = "Owner,Branch,Territory,Role";
        Session["SelectedColumns"] = columns;
        Session["FilterConditions"] = criteria;
        Response.Redirect("CustomReports.aspx?Generate=1");
    }

    protected void lnkEquipmentListReport_Click(object sender, EventArgs e)
    {
        var searchText = string.Empty;
        if (ddlSearch.SelectedIndex != 0)
        {
            switch (ddlSearch.SelectedValue.ToLower())
            {
                case "l.status":
                    searchText = rbStatus.SelectedValue;
                    break;
                case "b.name":
                    searchText = ddlCompany.SelectedValue;
                    break;
                case "l.maint":
                    searchText = rbMaint.SelectedValue;
                    break;
                case "l.billing":
                    searchText = rbBilling.SelectedValue;
                    break;
                case "l.credit":
                    searchText = rbCredit.SelectedValue;
                    break;
                case "l.emailinvoice":
                    searchText = rbEmailInvoice.SelectedValue;
                    break;
                case "l.printinvoice":
                    searchText = rbPrintInvoice.SelectedValue;
                    break;
                case "l.dispalert":
                    searchText = rbDispAlert.SelectedValue;
                    break;
                case "l.terr":
                    searchText = ddlTerr.SelectedValue;
                    break;
                case "l.terr2":
                    searchText = ddlTerr2.SelectedValue;
                    break;
                case "l.nocustomerstatement":
                    searchText = rbNoCustomerStatement.SelectedValue;
                    break;
                case "l.zone":
                    var zoneSearchValue = string.Empty;
                    if (ddlZone1.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlZone1.CheckedItems)
                        {
                            zoneSearchValue += item.Value + ",";
                        }

                        zoneSearchValue = zoneSearchValue.TrimEnd(',');
                    }

                    searchText = zoneSearchValue;
                    break;
                case "l.type":
                    var typeSearchValue = string.Empty;
                    if (ddlUserType.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlUserType.CheckedItems)
                        {
                            typeSearchValue += item.Value + ",";
                        }

                        typeSearchValue = typeSearchValue.TrimEnd(',');
                    }

                    searchText = typeSearchValue;
                    break;
                case "rt.name":
                    var routeSearchValue = string.Empty;
                    if (ddlRoute.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlRoute.CheckedItems)
                        {
                            routeSearchValue += item.Value + ",";
                        }

                        routeSearchValue = routeSearchValue.TrimEnd(',');
                    }

                    searchText = routeSearchValue;
                    break;
                case "l.businesstype":
                    searchText = ddlBusinessType.SelectedValue;
                    break;
                default:
                    searchText = txtSearch.Text.Replace("'", "''");
                    break;
            }
        }

        Session["ddlSearch_Location"] = ddlSearch.SelectedValue;
        Session["ddlSearch_Value_Location"] = searchText;
        Session["lnkChk_Location"] = lnkChk.Checked;


        var filterExpression = RadGrid_Location.MasterTableView.FilterExpression;
        if (!string.IsNullOrEmpty(filterExpression))
        {
            Session["Location_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Location.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = String.Empty;
                String columnName = column.UniqueName;

                if (column.UniqueName == "Type")
                {
                    if (column.ListOfFilterValues != null)
                    {
                        List<string> listFil = new List<string>(column.ListOfFilterValues);
                        filterValues = String.Join(",", listFil.Select(x => string.Format("'{0}'", x)));
                        columnName = "Types";
                    }
                    else
                    {
                        filterValues = column.CurrentFilterValue;
                    }
                }
                else
                {
                    filterValues = column.CurrentFilterValue;
                }

                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Location_Filters"] = filters;
        }
        else
        {
            Session.Remove("Location_FilterExpression");
            Session.Remove("Location_Filters");
        }

        string urlString = "LocationEquipmentListReport.aspx?stype=" + ddlSearch.SelectedValue + "&stext=" + searchText + "&inclInactive=" + lnkChk.Checked;

        // Redirect when close the report
        urlString += "&redirect=Locations.aspx";

        Response.Redirect(urlString, true);
    }

    protected void lnkLocationBusinessType_Click(object sender, EventArgs e)
    {
        var searchText = string.Empty;
        if (ddlSearch.SelectedIndex != 0)
        {
            switch (ddlSearch.SelectedValue.ToLower())
            {
                case "l.status":
                    searchText = rbStatus.SelectedValue;
                    break;
                case "b.name":
                    searchText = ddlCompany.SelectedValue;
                    break;
                case "l.maint":
                    searchText = rbMaint.SelectedValue;
                    break;
                case "l.billing":
                    searchText = rbBilling.SelectedValue;
                    break;
                case "l.credit":
                    searchText = rbCredit.SelectedValue;
                    break;
                case "l.emailinvoice":
                    searchText = rbEmailInvoice.SelectedValue;
                    break;
                case "l.printinvoice":
                    searchText = rbPrintInvoice.SelectedValue;
                    break;
                case "l.dispalert":
                    searchText = rbDispAlert.SelectedValue;
                    break;
                case "l.terr":
                    searchText = ddlTerr.SelectedValue;
                    break;
                case "l.terr2":
                    searchText = ddlTerr2.SelectedValue;
                    break;
                case "l.nocustomerstatement":
                    searchText = rbNoCustomerStatement.SelectedValue;
                    break;
                case "l.zone":
                    var zoneSearchValue = string.Empty;
                    if (ddlZone1.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlZone1.CheckedItems)
                        {
                            zoneSearchValue += item.Value + ",";
                        }

                        zoneSearchValue = zoneSearchValue.TrimEnd(',');
                    }

                    searchText = zoneSearchValue;
                    break;
                case "l.type":
                    var typeSearchValue = string.Empty;
                    if (ddlUserType.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlUserType.CheckedItems)
                        {
                            typeSearchValue += item.Value + ",";
                        }

                        typeSearchValue = typeSearchValue.TrimEnd(',');
                    }

                    searchText = typeSearchValue;
                    break;
                case "rt.name":
                    var routeSearchValue = string.Empty;
                    if (ddlRoute.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlRoute.CheckedItems)
                        {
                            routeSearchValue += item.Value + ",";
                        }

                        routeSearchValue = routeSearchValue.TrimEnd(',');
                    }

                    searchText = routeSearchValue;
                    break;
                case "l.businesstype":
                    searchText = ddlBusinessType.SelectedValue;
                    break;
                default:
                    searchText = txtSearch.Text.Replace("'", "''");
                    break;
            }
        }

        Session["ddlSearch_Location"] = ddlSearch.SelectedValue;
        Session["ddlSearch_Value_Location"] = searchText;
        Session["lnkChk_Location"] = lnkChk.Checked;

        var filterExpression = RadGrid_Location.MasterTableView.FilterExpression;
        if (!string.IsNullOrEmpty(filterExpression))
        {
            Session["Location_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Location.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = String.Empty;
                String columnName = column.UniqueName;

                if (column.UniqueName == "Type")
                {
                    if (column.ListOfFilterValues != null)
                    {
                        List<string> listFil = new List<string>(column.ListOfFilterValues);
                        filterValues = String.Join(",", listFil.Select(x => string.Format("'{0}'", x)));
                        columnName = "Types";
                    }
                    else
                    {
                        filterValues = column.CurrentFilterValue;
                    }
                }
                else
                {
                    filterValues = column.CurrentFilterValue;
                }

                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Location_Filters"] = filters;
        }
        else
        {
            Session.Remove("Location_FilterExpression");
            Session.Remove("Location_Filters");
        }

        string urlString = "LocationBusinessTypeReport.aspx?stype=" + ddlSearch.SelectedValue + "&stext=" + searchText + "&inclInactive=" + lnkChk.Checked;

        // Redirect when close the report
        urlString += "&redirect=Locations.aspx";

        Response.Redirect(urlString, true);
    }

    protected void lnkLocationDetails_Click(object sender, EventArgs e)
    {
        var searchText = string.Empty;
        if (ddlSearch.SelectedIndex != 0)
        {
            switch (ddlSearch.SelectedValue.ToLower())
            {
                case "l.status":
                    searchText = rbStatus.SelectedValue;
                    break;
                case "b.name":
                    searchText = ddlCompany.SelectedValue;
                    break;
                case "l.maint":
                    searchText = rbMaint.SelectedValue;
                    break;
                case "l.billing":
                    searchText = rbBilling.SelectedValue;
                    break;
                case "l.credit":
                    searchText = rbCredit.SelectedValue;
                    break;
                case "l.emailinvoice":
                    searchText = rbEmailInvoice.SelectedValue;
                    break;
                case "l.printinvoice":
                    searchText = rbPrintInvoice.SelectedValue;
                    break;
                case "l.dispalert":
                    searchText = rbDispAlert.SelectedValue;
                    break;
                case "l.terr":
                    searchText = ddlTerr.SelectedValue;
                    break;
                case "l.terr2":
                    searchText = ddlTerr2.SelectedValue;
                    break;
                case "l.nocustomerstatement":
                    searchText = rbNoCustomerStatement.SelectedValue;
                    break;
                case "l.zone":
                    var zoneSearchValue = string.Empty;
                    if (ddlZone1.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlZone1.CheckedItems)
                        {
                            zoneSearchValue += item.Value + ",";
                        }

                        zoneSearchValue = zoneSearchValue.TrimEnd(',');
                    }

                    searchText = zoneSearchValue;
                    break;
                case "l.type":
                    var typeSearchValue = string.Empty;
                    if (ddlUserType.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlUserType.CheckedItems)
                        {
                            typeSearchValue += item.Value + ",";
                        }

                        typeSearchValue = typeSearchValue.TrimEnd(',');
                    }

                    searchText = typeSearchValue;
                    break;
                case "rt.name":
                    var routeSearchValue = string.Empty;
                    if (ddlRoute.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlRoute.CheckedItems)
                        {
                            routeSearchValue += item.Value + ",";
                        }

                        routeSearchValue = routeSearchValue.TrimEnd(',');
                    }

                    searchText = routeSearchValue;
                    break;
                case "l.businesstype":
                    searchText = ddlBusinessType.SelectedValue;
                    break;
                default:
                    searchText = txtSearch.Text.Replace("'", "''");
                    break;
            }
        }

        Session["ddlSearch_Location"] = ddlSearch.SelectedValue;
        Session["ddlSearch_Value_Location"] = searchText;
        Session["lnkChk_Location"] = lnkChk.Checked;

        var filterExpression = RadGrid_Location.MasterTableView.FilterExpression;
        if (!string.IsNullOrEmpty(filterExpression))
        {
            Session["Location_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Location.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = String.Empty;
                String columnName = column.UniqueName;

                if (column.UniqueName == "Type")
                {
                    if (column.ListOfFilterValues != null)
                    {
                        List<string> listFil = new List<string>(column.ListOfFilterValues);
                        filterValues = String.Join(",", listFil.Select(x => string.Format("'{0}'", x)));
                        columnName = "Types";
                    }
                    else
                    {
                        filterValues = column.CurrentFilterValue;
                    }
                }
                else
                {
                    filterValues = column.CurrentFilterValue;
                }

                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Location_Filters"] = filters;
        }
        else
        {
            Session.Remove("Location_FilterExpression");
            Session.Remove("Location_Filters");
        }

        string urlString = "LocationDetailsReport.aspx?stype=" + ddlSearch.SelectedValue + "&stext=" + searchText + "&inclInactive=" + lnkChk.Checked;

        // Redirect when close the report
        urlString += "&redirect=Locations.aspx";

        Response.Redirect(urlString, true);
    }

    protected void lnkLocationHomeOwner_Click(object sender, EventArgs e)
    {
        var searchText = string.Empty;
        if (ddlSearch.SelectedIndex != 0)
        {
            switch (ddlSearch.SelectedValue.ToLower())
            {
                case "l.status":
                    searchText = rbStatus.SelectedValue;
                    break;
                case "b.name":
                    searchText = ddlCompany.SelectedValue;
                    break;
                case "l.maint":
                    searchText = rbMaint.SelectedValue;
                    break;
                case "l.billing":
                    searchText = rbBilling.SelectedValue;
                    break;
                case "l.credit":
                    searchText = rbCredit.SelectedValue;
                    break;
                case "l.emailinvoice":
                    searchText = rbEmailInvoice.SelectedValue;
                    break;
                case "l.printinvoice":
                    searchText = rbPrintInvoice.SelectedValue;
                    break;
                case "l.dispalert":
                    searchText = rbDispAlert.SelectedValue;
                    break;
                case "l.terr":
                    searchText = ddlTerr.SelectedValue;
                    break;
                case "l.terr2":
                    searchText = ddlTerr2.SelectedValue;
                    break;
                case "l.nocustomerstatement":
                    searchText = rbNoCustomerStatement.SelectedValue;
                    break;
                case "l.zone":
                    var zoneSearchValue = string.Empty;
                    if (ddlZone1.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlZone1.CheckedItems)
                        {
                            zoneSearchValue += item.Value + ",";
                        }

                        zoneSearchValue = zoneSearchValue.TrimEnd(',');
                    }

                    searchText = zoneSearchValue;
                    break;
                case "l.type":
                    var typeSearchValue = string.Empty;
                    if (ddlUserType.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlUserType.CheckedItems)
                        {
                            typeSearchValue += item.Value + ",";
                        }

                        typeSearchValue = typeSearchValue.TrimEnd(',');
                    }

                    searchText = typeSearchValue;
                    break;
                case "rt.name":
                    var routeSearchValue = string.Empty;
                    if (ddlRoute.CheckedItems.Count > 0)
                    {
                        foreach (var item in ddlRoute.CheckedItems)
                        {
                            routeSearchValue += item.Value + ",";
                        }

                        routeSearchValue = routeSearchValue.TrimEnd(',');
                    }

                    searchText = routeSearchValue;
                    break;
                case "l.businesstype":
                    searchText = ddlBusinessType.SelectedValue;
                    break;
                default:
                    searchText = txtSearch.Text.Replace("'", "''");
                    break;
            }
        }

        Session["ddlSearch_Location"] = ddlSearch.SelectedValue;
        Session["ddlSearch_Value_Location"] = searchText;
        Session["lnkChk_Location"] = lnkChk.Checked;

        var filterExpression = RadGrid_Location.MasterTableView.FilterExpression;
        if (!string.IsNullOrEmpty(filterExpression))
        {
            Session["Location_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Location.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = String.Empty;
                String columnName = column.UniqueName;

                if (column.UniqueName == "Type")
                {
                    if (column.ListOfFilterValues != null)
                    {
                        List<string> listFil = new List<string>(column.ListOfFilterValues);
                        filterValues = String.Join(",", listFil.Select(x => string.Format("'{0}'", x)));
                        columnName = "Types";
                    }
                    else
                    {
                        filterValues = column.CurrentFilterValue;
                    }
                }
                else
                {
                    filterValues = column.CurrentFilterValue;
                }

                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Location_Filters"] = filters;
        }
        else
        {
            Session.Remove("Location_FilterExpression");
            Session.Remove("Location_Filters");
        }

        string urlString = "LocationWithHomeownerReport.aspx?stype=" + ddlSearch.SelectedValue + "&stext=" + searchText + "&inclInactive=" + lnkChk.Checked;

        // Redirect when close the report
        urlString += "&redirect=Locations.aspx";

        Response.Redirect(urlString, true);
    }

    public void UpdateControl()
    {
        if (Session["LocationSearchState"] != null)
        {
            if (Session["LocationSearchState"].ToString() != string.Empty)
            {
                string[] strFilter = Session["LocationSearchState"].ToString().Split(';');

                if (strFilter.Length == 3)
                {
                    if (!string.IsNullOrEmpty(strFilter[0]))
                    {
                        var searchText = strFilter[1];
                        ddlSearch.SelectedValue = strFilter[0];

                        if (ddlSearch.SelectedValue == "l.Status")
                        {
                            rbStatus.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.Billing")
                        {
                            rbBilling.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.Credit")
                        {
                            rbCredit.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.DispAlert")
                        {
                            rbDispAlert.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.EmailInvoice")
                        {
                            rbPrintInvoice.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.PrintInvoice")
                        {
                            rbEmailInvoice.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.NoCustomerStatement")
                        {
                            rbNoCustomerStatement.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.Terr")
                        {
                            ddlTerr.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.Terr2")
                        {
                            ddlTerr2.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.Maint")
                        {
                            rbMaint.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.Zone")
                        {
                            ddlZone1.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.type")
                        {
                            ddlUserType.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "rt.Name")
                        {
                            ddlRoute.SelectedValue = searchText;
                        }
                        else if (ddlSearch.SelectedValue == "l.businesstype")
                        {
                            ddlBusinessType.SelectedValue = searchText;
                        }
                        else
                        {
                            txtSearch.Text = searchText;
                        }
                    }

                    lnkChk.Checked = Convert.ToBoolean(strFilter[2]);
                }
            }
        }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Location.MasterTableView.GetColumn("ClientSelectColumn").Visible = false;
        RadGrid_Location.MasterTableView.GetColumn("ImageQB").Visible = false;
        RadGrid_Location.MasterTableView.GetColumn("ImgCreditH").Visible = false;
        RadGrid_Location.MasterTableView.GetColumn("CustomerName").Visible = true;
        RadGrid_Location.MasterTableView.GetColumn("LocationName").Visible = true;
        RadGrid_Location.MasterTableView.GetColumn("Email").Visible = true;
        RadGrid_Location.MasterTableView.GetColumn("ContactName").Visible = true;
        RadGrid_Location.MasterTableView.GetColumn("BusinessType").Visible = true;
        RadGrid_Location.MasterTableView.GetColumn("BusinessType").HeaderText = getBusinessTypeLabel();
        RadGrid_Location.ExportSettings.FileName = "Locations";
        RadGrid_Location.ExportSettings.IgnorePaging = true;
        RadGrid_Location.ExportSettings.ExportOnlyData = true;
        RadGrid_Location.ExportSettings.OpenInNewWindow = true;
        RadGrid_Location.ExportSettings.HideStructureColumns = true;
        RadGrid_Location.MasterTableView.UseAllDataFields = true;
        // RadGrid_Location.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");
        RadGrid_Location.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Location.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_Location_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 7;
        else
            currentItem = 8;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Location.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Location.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

    protected void RadGrid_Location_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (Convert.ToString(RadGrid_Location.MasterTableView.FilterExpression) != "")
                    lblRecordCount.Text = totalCount + " Record(s) found";
                else
                    lblRecordCount.Text = RadGrid_Location.VirtualItemCount + " Record(s) found";
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

    protected void RadGrid_Location_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
    {
        string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Type = DataField;

        _GetLocationType.ConnConfig = Session["config"].ToString();
        _GetLocationType.Type = DataField;

        DataSet ds = new DataSet();

        List<GetLocationTypeViewModel> _lstGetLocationType = new List<GetLocationTypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GridGetLocationType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationType);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetLocationType = serializer.Deserialize<List<GetLocationTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetLocationTypeViewModel>(_lstGetLocationType);
        }
        else
        {
            ds = objBL_User.GetLocationType(objProp_User);
        }

        if (ds.Tables[0] != null)
        {
            e.ListBox.DataSource = ds.Tables[0];
            e.ListBox.DataKeyField = DataField;
            e.ListBox.DataTextField = DataField;
            e.ListBox.DataValueField = DataField;
            e.ListBox.DataBind();
        }
    }

    protected void lnkMassAttachDocuments_Click(object sender, EventArgs e)
    {
        // Get folder name which contains all documents from web.config
        // Get all file names in that folder and insert into a table (tblMassAttachDocumentsName)
        // Collect all locations base on the file names
        // Insert all data for location's documents
        try
        {
            if (IsAllowMassAttachDocumments())
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["MassAttachDocuments"].Trim();
                if (!string.IsNullOrEmpty(savepathconfig))
                {
                    if (Directory.Exists(savepathconfig))
                    {
                        var strConfig = Session["config"].ToString();
                        _ImportDataForMassAttachDocuments.strConfig = Session["config"].ToString();

                        DataTable dt = new DataTable();
                        dt.Columns.Add(new DataColumn("AccountID", typeof(string)));
                        dt.Columns.Add(new DataColumn("Path", typeof(string)));
                        dt.Columns.Add(new DataColumn("Filename", typeof(string)));
                        dt.Columns.Add(new DataColumn("FileExt", typeof(string)));
                        string[] fileArray = Directory.GetFiles(savepathconfig);
                        List<string> fileNames = new List<string>();
                        foreach (var item in fileArray)
                        {
                            //var arr = item.Split('\\');
                            //var name = arr[arr.Length - 1];
                            var name = System.IO.Path.GetFileName(item);
                            var nameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(item);
                            var ext = System.IO.Path.GetExtension(name).Substring(1);
                            var locAccountID = string.Empty;
                            try
                            {
                                var locAccountIDArr = nameWithoutExt.Split('_');
                                if (locAccountIDArr.Length > 1)
                                {
                                    var fileVersion = locAccountIDArr[locAccountIDArr.Length - 1];
                                    locAccountID = nameWithoutExt.Substring(0, nameWithoutExt.Length - fileVersion.Length - 1);
                                }
                                else
                                {
                                    locAccountID = nameWithoutExt;
                                }

                            }
                            catch (Exception)
                            {
                            }

                            DataRow dr = dt.NewRow();
                            dr["AccountID"] = locAccountID;
                            dr["Path"] = item;
                            dr["Filename"] = name;
                            dr["FileExt"] = ext;//UpdateFiletypeFollowingDoctypeData(ext);
                            dt.Rows.Add(dr);
                            //fileNames.Add(locAccountID);
                        }

                        BL_MapData bL_MapData = new BL_MapData();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "LocationsAPI/LocationsList_ImportDataForMassAttachDocuments";

                            _ImportDataForMassAttachDocuments.dataTable = dt;

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ImportDataForMassAttachDocuments);
                        }
                        else
                        {
                            bL_MapData.ImportDataForMassAttachDocuments(strConfig, dt);
                        }

                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysucc", "noty({text: 'Mass attach documents finished',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private string UpdateFiletypeFollowingDoctypeData(string ext)
    {
        string retVal = "Other";
        if (!string.IsNullOrEmpty(ext))
        {
            switch (ext.ToLower())
            {
                case "xlsx":
                case "xls":
                    retVal = "xls";
                    break;
                case "docx":
                case "doc":
                    retVal = "doc";
                    break;
                case "png":
                case "jpg":
                case "bmp":
                case "gif":
                    retVal = "Picture";
                    break;
                default:
                    retVal = "Other";
                    break;
            }
        }
        return retVal;
    }

    private bool IsAllowMassAttachDocumments()
    {
        try
        {
            var companyName = ConfigurationManager.AppSettings["CustomerName"].ToString();

            return
                companyName.Equals("Transel", StringComparison.InvariantCultureIgnoreCase) &&
                Convert.ToInt32(Session["UserID"].ToString()) == 1 &&
                (string)Session["User"] == "Maintenance" &&
                (string)Session["type"] == "am" &&
                (string)Session["Username"] == "Maintenance";
        }
        catch (Exception)
        {
            return false;
        }

    }

    private void FillSearchFilter()
    {
        ddlSearch.Items.Clear();
        ddlSearch.Items.Add(new ListItem("Select", string.Empty));
        ddlSearch.Items.Add(new ListItem("AcctNo", "l.ID"));
        ddlSearch.Items.Add(new ListItem("Location", "l.tag"));
        ddlSearch.Items.Add(new ListItem("Customer", "r.Name"));
        ddlSearch.Items.Add(new ListItem("Address", "l.Address"));
        ddlSearch.Items.Add(new ListItem("City", "l.City"));
        ddlSearch.Items.Add(new ListItem("State", "l.State"));
        ddlSearch.Items.Add(new ListItem("LocationPrice", "l.PriceL"));
        ddlSearch.Items.Add(new ListItem("Zip", "l.Zip"));
        ddlSearch.Items.Add(new ListItem("Zone", "l.Zone"));
        ddlSearch.Items.Add(new ListItem("Status", "l.Status"));
        ddlSearch.Items.Add(new ListItem("Type", "l.type"));
        ddlSearch.Items.Add(new ListItem("Equip", "l.Elevs"));
        ddlSearch.Items.Add(new ListItem("Balance", "l.Balance"));
        ddlSearch.Items.Add(new ListItem("Company", "B.Name"));
        ddlSearch.Items.Add(new ListItem("PaidNumber", "l.PaidNumb"));
        ddlSearch.Items.Add(new ListItem("PaidDays", "l.PaidDays"));
        ddlSearch.Items.Add(new ListItem("Maint", "l.Maint"));
        ddlSearch.Items.Add(new ListItem("WriteOff", "l.WriteOff"));
        ddlSearch.Items.Add(new ListItem("Remarks", "l.Remarks"));
        ddlSearch.Items.Add(new ListItem("CreditReason", "l.CreditReason"));
        ddlSearch.Items.Add(new ListItem("EmailTicket", "l.Custom12"));
        ddlSearch.Items.Add(new ListItem("EmailTicketCopy", "l.Custom13"));
        ddlSearch.Items.Add(new ListItem("EmailAddressInvoice", "l.Custom14"));
        ddlSearch.Items.Add(new ListItem("InvoiceCopy", "l.Custom15"));
        ddlSearch.Items.Add(new ListItem("LocationCountry", "l.Country"));
        //Get Route label
        ddlSearch.Items.Add(new ListItem(getRouteLabel(), "rt.Name"));

        ddlSearch.Items.Add(new ListItem("Mech", "rt.Mech"));
        ddlSearch.Items.Add(new ListItem("sTax", "l.sTax"));
        ddlSearch.Items.Add(new ListItem("sTaxRate", "s.sTaxRate"));
        ddlSearch.Items.Add(new ListItem("sTaxRemarks", "s.sTaxRemarks"));
        ddlSearch.Items.Add(new ListItem("Billing", "l.Billing"));
        ddlSearch.Items.Add(new ListItem("Credit", "l.Credit"));
        ddlSearch.Items.Add(new ListItem("DispAlert", "l.DispAlert"));
        ddlSearch.Items.Add(new ListItem("EmailInvoice", "l.EmailInvoice"));
        ddlSearch.Items.Add(new ListItem("PrintInvoice", "l.PrintInvoice"));
        ddlSearch.Items.Add(new ListItem("CreditReason", "l.CreditReason"));
        ddlSearch.Items.Add(new ListItem("BillRate", "l.BillRate"));
        ddlSearch.Items.Add(new ListItem("RateOT", "l.RateOT"));
        ddlSearch.Items.Add(new ListItem("RateNT", "l.RateNT"));
        ddlSearch.Items.Add(new ListItem("RateDT", "l.RateDT"));
        ddlSearch.Items.Add(new ListItem("RateTravel", "l.RateTravel"));
        ddlSearch.Items.Add(new ListItem("RateMileage", "l.RateMileage"));
        ddlSearch.Items.Add(new ListItem("ColRemarks", "l.ColRemarks"));
        ddlSearch.Items.Add(new ListItem("Salesperson", "l.Terr"));
        ddlSearch.Items.Add(new ListItem("Salesperson2", "l.Terr2"));
        ddlSearch.Items.Add(new ListItem("NoCustomerStatement", "l.NoCustomerStatement"));
        ddlSearch.Items.Add(new ListItem(getBusinessTypeLabel(), "l.businesstype"));

    }

    private String getRouteLabel()
    {
        BL_Customer objBL_Customer = new BL_Customer();
        Customer objCustomer = new Customer();

        objCustomer.ConnConfig = Session["config"].ToString();
        _GetDefaultWorkerHeader.ConnConfig = Session["config"].ToString();

        string getValue;

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetDefaultWorkerHeader";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDefaultWorkerHeader);

            object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
            getValue = JsonData.ToString();
        }
        else
        {
            getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        }

        if (!string.IsNullOrEmpty(getValue))
        {
            return getValue;
        }
        else
        {
            return "Default Worker";
        }
    }

    private void FillLocationType()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        _getlocationType.ConnConfig = Session["config"].ToString();

        List<GetLocationTypeViewModel> _lstgetlocationType = new List<GetLocationTypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_getlocationType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getlocationType);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstgetlocationType = serializer.Deserialize<List<GetLocationTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetLocationTypeViewModel>(_lstgetlocationType);
        }
        else
        {
            ds = objBL_User.getlocationType(objProp_User);
        }
        ddlUserType.DataSource = ds.Tables[0];
        ddlUserType.DataTextField = "Type";
        ddlUserType.DataValueField = "Type";
        ddlUserType.DataBind();
    }

    private void FillRoute()
    {

        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        DataSet ds3 = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        _GetRoute.ConnConfig = Session["config"].ToString();

        ListGetRouteViewModel _lstGetRoute = new ListGetRouteViewModel();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetRoute";

            _GetRoute.IsActive = 1;
            _GetRoute.LocID = 0;
            _GetRoute.ContractID = 0;

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetRoute);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetRoute = serializer.Deserialize<ListGetRouteViewModel>(_APIResponse.ResponseData);

            ds1 = _lstGetRoute.lstTable1.ToDataSet();
            ds2 = _lstGetRoute.lstTable2.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1 = ds1.Tables[0];
            dt2 = ds2.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";
            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });

            if (_lstGetRoute.lstTable3 != null)
            {
                ds3 = _lstGetRoute.lstTable3.ToDataSet();
                DataTable dt3 = new DataTable();
                dt3 = ds3.Tables[0];
                dt3.TableName = "Table3";
                ds.Tables.AddRange(new DataTable[] { dt3.Copy() });
            }
        }
        else
        {
            ds = objBL_User.getRoute(objProp_User, 1, 0, 0);//IsActive=1 :- Get Only Active Workers
        }

        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "label";
        ddlRoute.DataValueField = "ID";
        ddlRoute.DataBind();



    }

    private String getBusinessTypeLabel()
    {
        BL_Customer objBL_Customer = new BL_Customer();
        Customer objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();

        _GetBT.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();

        List<GetBTViewModel> _lstGetBT = new List<GetBTViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetBT";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBT);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetBT = serializer.Deserialize<List<GetBTViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetBTViewModel>(_lstGetBT);
        }
        else
        {
            ds = objBL_Customer.getBT(objCustomer);
        }

        try
        {
            return ds.Tables[0].Rows[0]["Label"].ToString();
        }
        catch
        {
            return "Business Type";
        }
    }

    private void FillBusinessType()
    {

        BL_Customer objBL_Customer = new BL_Customer();
        Customer objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();

        _GetBT.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();

        List<GetBTViewModel> _lstGetBT = new List<GetBTViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetBT";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBT);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetBT = serializer.Deserialize<List<GetBTViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetBTViewModel>(_lstGetBT);
        }
        else
        {
            ds = objBL_Customer.getBT(objCustomer);
        }

        ddlBusinessType.DataSource = ds.Tables[0];
        ddlBusinessType.DataTextField = "Description";
        ddlBusinessType.DataValueField = "ID";
        ddlBusinessType.DataBind();
    }

    private DataTable processDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Session["Location_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                if (Session["Location_Filters"] != null)
                {
                    var filtersGet = Session["Location_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            if (_filter.FilterColumn != "Types")
                            {
                                GridColumn column = RadGrid_Location.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                                if (column.UniqueName == "opencall" || column.UniqueName == "Elevs" || column.UniqueName == "Balance")
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

                if (Session["Loc_TypeFilters"] != null)
                {
                    sql = sql + " And Type in ('" + Session["Loc_TypeFilters"].ToString().Replace(",", "','") + "')";
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

    protected void RadGrid_Location_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Filter")
        {

            string[] routeItems = ((RadGrid)sender).MasterTableView.GetColumn("Type").ListOfFilterValues;
            if (routeItems != null)
            {
                Session["Loc_TypeFilters"] = string.Join(",", routeItems);
            }

        }
    }

    private void FillBilling()
    {
        rbBilling.Items.Clear();
        rbBilling.Items.Add(new ListItem("No", "0"));
        rbBilling.Items.Add(new ListItem("Yes", "1"));
    }

    private void FillCredit()
    {
        rbCredit.Items.Clear();
        rbCredit.Items.Add(new ListItem("No", "0"));
        rbCredit.Items.Add(new ListItem("Yes", "1"));
    }

    private void FillDispAlert()
    {
        rbDispAlert.Items.Clear();
        rbDispAlert.Items.Add(new ListItem("No", "0"));
        rbDispAlert.Items.Add(new ListItem("Yes", "1"));
    }

    private void FillPrintInvoice()
    {
        rbPrintInvoice.Items.Clear();
        rbPrintInvoice.Items.Add(new ListItem("False", "0"));
        rbPrintInvoice.Items.Add(new ListItem("True", "1"));
    }

    private void FillEmailInvoice()
    {
        rbEmailInvoice.Items.Clear();
        rbEmailInvoice.Items.Add(new ListItem("False", "0"));
        rbEmailInvoice.Items.Add(new ListItem("True", "1"));
    }

    private void FillNoCustomerStatement()
    {
        rbNoCustomerStatement.Items.Clear();
        rbNoCustomerStatement.Items.Add(new ListItem("False", "0"));
        rbNoCustomerStatement.Items.Add(new ListItem("True", "1"));
    }

    private void FilMaint()
    {
        rbMaint.Items.Clear();
        rbMaint.Items.Add(new ListItem("Yes", "1"));
        rbMaint.Items.Add(new ListItem("No", "0"));
    }

    private void FillStatus()
    {
        rbStatus.Items.Clear();
        rbStatus.Items.Add(new ListItem("Active", "0"));
        rbStatus.Items.Add(new ListItem("Inactive", "1"));
    }

    protected void RadGrid_Location_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGrid_LocationCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_LocationminimumRows"] = e.NewPageIndex * RadGrid_Location.PageSize;
            ViewState["RadGrid_LocationmaximumRows"] = (e.NewPageIndex + 1) * RadGrid_Location.PageSize;
        }
        catch { }
    }

    protected void RadGrid_Location_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGrid_LocationminimumRows"] = RadGrid_Location.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_LocationmaximumRows"] = (RadGrid_Location.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }

    public string getAllContractByID(object loc, object status)
    {
        StringBuilder str = new StringBuilder();
        if (Convert.ToString(status) != "")
        {
            BL_Customer objBL_Customer = new BL_Customer();
            Customer objCustomer = new Customer();

            //Get Customer Note
            DataSet dsContract = new DataSet();
            dsContract = objBL_Customer.GetContractByLoc(Convert.ToString(Session["config"]), Convert.ToInt32(loc));

            if (dsContract != null)
            {
                if (dsContract.Tables[0].Rows.Count > 0)
                {

                    str.Append("<table><thead><tr><th>Contract #</th><th>Created date</th><th>Status</th><th>Service type</th><th>Closed/Hold date</th></tr></thead>");
                    str.Append("<tbody>");
                    foreach (DataRow row in dsContract.Tables[0].Rows)
                    {
                        str.AppendFormat("<tr><td>{0}</td>", row["Job"]);
                        str.AppendFormat("<td>{0}</td>", row["fDate"].ToString() == "" ? "" : string.Format("{0:M/d/yyyy}", Convert.ToDateTime(row["fDate"])));
                        str.AppendFormat("<td>{0}</td>", row["StatusName"]);
                        str.AppendFormat("<td>{0}</td>", row["CType"]);
                        str.AppendFormat("<td>{0}</td></tr>", row["CloseDate"].ToString() == "" ? "" : string.Format("{0:M/d/yyyy}", Convert.ToDateTime(row["CloseDate"])));
                    }

                    str.Append(" </tbody></table>");
                }
            }
        }

        return str.ToString();
    }
    public DataTable GetTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Location", typeof(string));
        dt.Columns.Add("Address", typeof(string));
        dt.Columns.Add("City", typeof(string));
        dt.Columns.Add("State", typeof(string));
        dt.Columns.Add("Zip", typeof(string));
        return dt;
    }
    public static string CallWebService(string xmlDoc)
    {
        var _url = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";
        var _action = "http://xxxxxxxx/Service1.asmx?op=HelloWorld";

        XmlDocument soapEnvelopeXml = CreateSoapEnvelope(xmlDoc);
        HttpWebRequest webRequest = CreateWebRequest(_url, _action, "nmishra@986057068", "fkl8TM2E");
        InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

        // begin async call to web request.
        IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

        // suspend this thread until call is complete. You might want to
        // do something usefull here like update your UI.
        asyncResult.AsyncWaitHandle.WaitOne();

        // get the response from the completed web request.
        string soapResult;
        using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
        {
            using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
            {
                soapResult = rd.ReadToEnd();
            }
            //Console.Write(soapResult);
        }
        return soapResult;
    }

    private static HttpWebRequest CreateWebRequest(string url, string action, string username, string passWord)
    {
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        //webRequest.Headers.Add("SOAPAction", action);
        webRequest.Headers.Add("javax.xml.ws.security.auth.username", username);
        webRequest.Headers.Add("javax.xml.ws.security.auth.password", passWord);
        webRequest.ContentType = "text/xml;charset=\"utf-8\"";
        webRequest.Accept = "text/xml";
        webRequest.Method = "POST";
        return webRequest;
    }

    private static XmlDocument CreateSoapEnvelope(string xmlDoc)
    {
        XmlDocument soapEnvelopeDocument = new XmlDocument();
        soapEnvelopeDocument.LoadXml(xmlDoc);
        return soapEnvelopeDocument;
    }

    private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
    {
        using (Stream stream = webRequest.GetRequestStream())
        {
            soapEnvelopeXml.Save(stream);
        }
    }


    private string GetGeoCode(DataTable dt, int id, string _sName, string _sAddres, string _sCity, string _sState, string _sZip, out string exceptionmsg)
    {
        string geo = "";
        string strerrorMessage = "";
        string code = "";
        string username = ConfigurationManager.AppSettings["vertexApiUsername"].ToString(); // "dread@1000";
        string passWord = ConfigurationManager.AppSettings["vertexApiPassword"].ToString(); // "K3CHccxQ";
        string uri = ConfigurationManager.AppSettings["vertexAddressURL"].ToString();
        exceptionmsg = "";
        string addrClnXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic = \"http://EiCalc/\">"
            + "<soapenv:Header/>"
            + "<soapenv:Body>"
            + "<eic:AddrCleanse>"
            + "<Request>"
            + "<![CDATA["
                + "<ADDRESS_CLEANSE_REQUEST>"
                    + "<StreetAddress1>" + _sAddres + "</StreetAddress1>"
                    + "<CityName>" + _sCity + "</CityName>"
                    + "<StateName>" + _sState + "</StateName>"
                    + "<ZipCode>" + _sZip + "</ZipCode>"
                + "</ADDRESS_CLEANSE_REQUEST>]]>"
            + "</Request>"
            + "</eic:AddrCleanse>"
            + "</soapenv:Body>"
            + "</soapenv:Envelope>";

        // try
        // {

        string resulttt = CallWebService(addrClnXML);


        XmlDocument responseXML = new XmlDocument();
        responseXML.LoadXml(resulttt);
        XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());
        string responseXMLPrettystr = responseXMLPretty.ToString();
        responseXMLPrettystr = responseXMLPrettystr.Replace("\"", "'");
        XmlDocument Doc = new XmlDocument();
        Doc.LoadXml(responseXMLPrettystr);

        XmlNode Errornode = Doc.GetElementsByTagName("Error").Item(0);
        if (Errornode == null)
        {
            XmlNode nodedoc = Doc.GetElementsByTagName("GeoCode").Item(0);
            if (nodedoc != null)
            {
                geo = nodedoc.ChildNodes.Item(0).InnerText;
            }
            else
            {
                geo = "Error in Address Geocode";
            }

        }
        else
        {
            geo = "Error in Address Geocode";

        }



        //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        //HttpClient cl = new HttpClient();
        //cl.BaseAddress = new Uri(uri);
        //cl.DefaultRequestHeaders.Clear();
        //cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.username", username);
        //cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.password", passWord);
        //HttpContent soapAddressEnvelope = new StringContent(addrClnXML);
        //soapAddressEnvelope.Headers.Clear();
        //soapAddressEnvelope.Headers.Add("Content-Type", "text/xml");
        //using (HttpResponseMessage response = cl.PostAsync(uri, soapAddressEnvelope).Result)
        //{
        //    string rawString = getMessage(response).Result;

        //    XmlDocument responseXML = new XmlDocument();
        //    responseXML.LoadXml(rawString);
        //    XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());
        //    string responseXMLPrettystr = responseXMLPretty.ToString();
        //    responseXMLPrettystr = responseXMLPrettystr.Replace("\"", "'");
        //    XmlDocument Doc = new XmlDocument();
        //    Doc.LoadXml(responseXMLPrettystr);

        //    XmlNode Errornode = Doc.GetElementsByTagName("Error").Item(0);
        //    if (Errornode == null)
        //    {
        //        XmlNode nodedoc = Doc.GetElementsByTagName("GeoCode").Item(0);
        //        if (nodedoc != null)
        //        {
        //            geo = nodedoc.ChildNodes.Item(0).InnerText;
        //        }
        //        else
        //        {
        //            geo = "Error in Address Geocode";
        //        }

        //    }
        //    else
        //    {
        //        geo = "Error in Address Geocode";

        //    }


        //    //geo = nodedoc.ChildNodes.Item(0).InnerText;
        //    soapAddressEnvelope.Dispose();
        //    cl.Dispose();
        //}
        //}

        //catch (AggregateException aggEx)
        //{
        //    Console.WriteLine("A Connection error occurred");
        //    Console.WriteLine("----------------------------------------------------");
        //    Console.WriteLine("Error Code: " + aggEx.Message.ToString() + Environment.NewLine + "Message: " + aggEx.InnerException.Message.ToString());
        //}
        //catch (Exception Ex)
        //{
        //    exceptionmsg = "An Exception occurred: " + Ex.InnerException.Message.ToString();
        //}




        async Task<string> getMessage(HttpResponseMessage messageFromServer)
        {
            //await Task.Delay(6000);
            code = await messageFromServer.Content.ReadAsStringAsync();
            return code;
        }
        if (geo.Trim() == "" || geo.Trim() == "Error in Address Geocode" )
        {
            DataRow dr = dt.NewRow();
            dr["ID"] = id.ToString();
            dr["Location"] = _sName.ToString();
            dr["Address"] = _sAddres.ToString();
            dr["City"] = _sCity.ToString();
            dr["State"] = _sState.ToString();
            dr["Zip"] = _sZip.ToString();
            dt.Rows.Add(dr);
        }


        return geo;
    }
    protected void lnkUpdateGeocode_Click(object sender, EventArgs e)
    {
        string _exceptionmsg = "";
        string _exceptionmsgloc = "";
        try
        {

            DataTable dt = GetTable();

            Loc objProp_Loc = new Loc();
            if (Session["locations"] != null)
            {
                foreach (GridDataItem item in RadGrid_Location.Items)
                //foreach(DataRow rw in empDt.Rows)
                {
                    Label lblloc = (Label)item.FindControl("lblloc");
                    HyperLink lnkName = (HyperLink)item.FindControl("lnkName");
                    string AddressValue = item["Address"].Text;
                    _exceptionmsgloc = lnkName.Text;
                    string CityValue = item["City"].Text;
                    string StateValue = item["State"].Text;
                    string ZipValue = item["Zip"].Text;

                    string _sGeocode = GetGeoCode(dt, Convert.ToInt32(lblloc.Text), Convert.ToString(lnkName.Text), AddressValue.ToString(), CityValue.ToString(), StateValue.ToString(), ZipValue.ToString(), out _exceptionmsg);

                    if (_exceptionmsg.Contains("An Exception occurred:") == true)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningGeocode", "noty({text: '" + _exceptionmsg + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        //break;
                        continue;
                    }
                    if (_sGeocode.Trim() == "" || _sGeocode.Trim() == "Error in Address Geocode")
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningGeocode", "noty({text: 'Error in Address Geocode for "+ lblFirst .Text +" "+ lblLast .Text+ "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        //string script = "function f(){OpenErrorModal(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    }

                    else
                    {
                        objProp_Loc.ConnConfig = Session["config"].ToString();
                        objProp_Loc.LocID = Convert.ToInt32(lblloc.Text);
                        objProp_Loc.MOMUSer = Session["Username"].ToString();
                        objProp_Loc.geoCode = _sGeocode;
                        objBL_User.spUpdateLocGeocode(objProp_Loc);
                    }

                }
                if (_exceptionmsg == "")
                {
                    if (dt.Rows.Count > 0)
                    {
                        gv_Errorrows.DataSource = dt;
                        gv_Errorrows.DataBind();
                        //gv_Errorrows.Rebind();


                        lblInvalidRows.Text = "Total Location:" + Convert.ToString(dt.Rows.Count);

                        string script = "function f(){OpenErrorModal(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningGeocode", "noty({text: 'Geocode updated successfully.', dismissQueue: true,  type : 'sucess', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningGeocode", "noty({text: '" + _exceptionmsg + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningGeocode", "noty({text: 'No Employee Found !', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception excp)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningGeocode", "noty({text: '" + _exceptionmsgloc + " : " + excp.Message.ToString() + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
}

