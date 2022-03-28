using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
//using QBFC12Lib;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Script.Services;
//using System.Data.Linq.SqlClient;
using System.Collections;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using BusinessEntity.APModels;
using BusinessEntity.Utility;
using MOMWebApp;
using BusinessEntity.InventoryModel;

public partial class Inventory : System.Web.UI.Page
{

    #region ::Declaration::
    private Dictionary<string, string> SearchList;

    private Hashtable HsSearchList;

    BL_User objBL_User = new BL_User();
    BL_Inventory objBL_Inventory = new BL_Inventory();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BusinessEntity.Inventory objProp_Inventory = new BusinessEntity.Inventory();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    private bool booSessionBegun;
    public DateTime createdtime;
    private static int intExportExcel = 0;
    DataSet ds = null;

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetStockReportsParam _GetStockReports = new GetStockReportsParam();
    GetInventoryParam _GetInventory = new GetInventoryParam();
    GetSearchInventoryParam _GetSearchInventory = new GetSearchInventoryParam();
    ReadAllCommodityParam _ReadAllCommodity = new ReadAllCommodityParam();
    DeleteInventoryByInvIDParam _DeleteInventoryByInvID = new DeleteInventoryByInvIDParam();
    GetALLInventoryParam _GetALLInventory = new GetALLInventoryParam();
    #endregion

    #region ::Page Events::
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        Permission();
        HighlightSideMenu("InventoryMgr", "lnkItemMaster", "ulInventoryMgr");

        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            #region Show Selected Filter
            if (Convert.ToString(Request.QueryString["fil"]) == "1")
            {
                if (Session["ddlSearchInv"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearchInv"]);
                    ddlSearch.SelectedValue = selectedValue;
                    ShowHideFilterSearch(selectedValue);

                    String searchValue = Convert.ToString(Session["ddlSearch_Value_Inv"]);
                    if (selectedValue == "ApprovedVendor")
                    {
                        ddlInventoryApprovedVendor.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "Commodity")
                    {
                        ddlCommodity.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "ABCClass")
                    {
                        ddlABC.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "Status")
                    {
                        ddlInvStatus.SelectedValue = searchValue;
                    }
                    else
                    {
                        txtSearch.Text = searchValue;
                    }
                }
            }
            else
            {
                Session.Remove("ddlSearchInv");
                Session.Remove("ddlSearch_Value_Inv");
                Session.Remove("Inv_FilterExpression");
                Session.Remove("Inv_Filters");
            }

            #endregion

            BindControls();
            BindSearchDropDown();
            ConvertToJSON();
        }
    }

    #endregion

    #region Report
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

            //API
            _GetStockReports.DBName = Session["dbname"].ToString();
            _GetStockReports.ConnConfig = Session["config"].ToString();
            _GetStockReports.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _GetStockReports.Type = "Customer";

            List<CustomerReportViewModel> _lstCustomerReport = new List<CustomerReportViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/InventoryList_GetStockReports";

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

    #endregion

    #region ::Methods::

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }

    private void Permission()
    {
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

        if (Session["type"].ToString() != "am")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            //Inventory item
            string InventoryItemPermission = ds.Rows[0]["Item"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Item"].ToString();
            hdnAddInventoryItem.Value = InventoryItemPermission.Length < 1 ? "Y" : InventoryItemPermission.Substring(0, 1);
            hdnEditInventoryItem.Value = InventoryItemPermission.Length < 2 ? "Y" : InventoryItemPermission.Substring(1, 1);
            hdnDeleteInventoryItem.Value = InventoryItemPermission.Length < 3 ? "Y" : InventoryItemPermission.Substring(2, 1);
            hdnViewInventoryItem.Value = InventoryItemPermission.Length < 4 ? "Y" : InventoryItemPermission.Substring(3, 1);

        }
    }

    private void BindSearchDropDown()
    {
        ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();

        objProp_Inventory.ConnConfig = Session["config"].ToString();
        objProp_Inventory.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objProp_Inventory.SearchField = ddlSearch.SelectedValue;


        _GetInventory.ConnConfig = Session["config"].ToString();
        _GetInventory.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetInventory.SearchField = ddlSearch.SelectedValue;

        _GetSearchInventory.ConnConfig = Session["config"].ToString();
        _GetSearchInventory.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetSearchInventory.SearchField = ddlSearch.SelectedValue;


        if (ddlSearch.SelectedValue == "Status")
        {
            objProp_Inventory.SearchValue = ddlInvStatus.SelectedValue;
            _GetInventory.SearchValue = ddlInvStatus.SelectedValue;
            _GetSearchInventory.SearchValue = ddlInvStatus.SelectedValue;
        }

        if (ddlSearch.SelectedValue == "ABCClass")
        {
            objProp_Inventory.SearchValue = ddlABC.SelectedValue;
            _GetInventory.SearchValue = ddlABC.SelectedValue;
            _GetSearchInventory.SearchValue = ddlABC.SelectedValue;
        }

        if (ddlSearch.SelectedValue == "Commodity")
        {
            objProp_Inventory.SearchValue = ddlCommodity.SelectedValue;
            _GetInventory.SearchValue = ddlCommodity.SelectedValue;
            _GetSearchInventory.SearchValue = ddlCommodity.SelectedValue;
        }

        if (ddlSearch.SelectedValue == "ApprovedVendor")
        {
            objProp_Inventory.SearchValue = ddlInventoryApprovedVendor.SelectedValue;
            _GetInventory.SearchValue = ddlInventoryApprovedVendor.SelectedValue;
            _GetSearchInventory.SearchValue = ddlInventoryApprovedVendor.SelectedValue;
        }
        else
        {
            objProp_Inventory.SearchValue = txtSearch.Text;
            _GetInventory.SearchValue = txtSearch.Text;
            _GetSearchInventory.SearchValue = txtSearch.Text;
        }

        if (Session["CmpChkDefault"].ToString() == "1")
        {
            objProp_Inventory.EN = 1;
            _GetInventory.EN = 1;
            _GetSearchInventory.EN = 1;
        }
        else
        {
            objProp_Inventory.EN = 0;
            _GetInventory.EN = 0;
            _GetSearchInventory.EN = 0;
        }

        if (ddlSearch.SelectedValue == "Select" || ddlSearch.SelectedValue == "")
        {
            ListGetInventory _lstInventory = new ListGetInventory();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/InventoryList_GetInventory";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInventory);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstInventory = serializer.Deserialize<ListGetInventory>(_APIResponse.ResponseData);

                ds1 = _lstInventory.lstTable1.ToDataSet();
                ds2 = _lstInventory.lstTable2.ToDataSet();

            }
            else
            {
                ds = objBL_Inventory.GetInventory(objProp_Inventory);
            }
        }
        else
        {
            ListGetSearchInventory _lstSearchInventory = new ListGetSearchInventory();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/InventoryList_GetSearchInventory";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetSearchInventory);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstSearchInventory = serializer.Deserialize<ListGetSearchInventory>(_APIResponse.ResponseData);

                ds1 = _lstSearchInventory.lstTable1.ToDataSet();
                ds2 = _lstSearchInventory.lstTable2.ToDataSet();
            }
            else
            {
                ds = objBL_Inventory.GetSearchInventory(objProp_Inventory);
            }
        }

        if (Session["Inventory"] != null)
            Session["Inventory"] = null;
        if (Session["searchdata"] != null)
            Session["searchdata"] = null;

        if (IsAPIIntegrationEnable == "YES")
        {
            if (ds1.Tables.Count > 0)
            {
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    HsSearchList = new Hashtable();
                    SearchList = new Dictionary<string, string>();
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {

                        InventorySearchConditions searchItem = new InventorySearchConditions();
                        searchItem.Id = (int)ds2.Tables[0].Rows[i]["Id"];
                        searchItem.DisplayName = (string)ds2.Tables[0].Rows[i]["DisplayName"];
                        searchItem.MappingColumn = (string)ds2.Tables[0].Rows[i]["MappingColumn"];
                        SearchList.Add(searchItem.MappingColumn, searchItem.DisplayName);

                    }
                    if (!HsSearchList.ContainsKey("SerachItems"))
                        HsSearchList.Add("SerachItems", SearchList);
                    else
                        HsSearchList["SerachItems"] = SearchList;
                }
            }
        }
        else
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    HsSearchList = new Hashtable();
                    SearchList = new Dictionary<string, string>();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {

                        InventorySearchConditions searchItem = new InventorySearchConditions();
                        searchItem.Id = (int)ds.Tables[1].Rows[i]["Id"];
                        searchItem.DisplayName = (string)ds.Tables[1].Rows[i]["DisplayName"];
                        searchItem.MappingColumn = (string)ds.Tables[1].Rows[i]["MappingColumn"];
                        SearchList.Add(searchItem.MappingColumn, searchItem.DisplayName);

                    }
                    if (!HsSearchList.ContainsKey("SerachItems"))
                        HsSearchList.Add("SerachItems", SearchList);
                    else
                        HsSearchList["SerachItems"] = SearchList;
                }
            }
        }

        ddlSearch.DataSource = SearchList;
        ddlSearch.DataTextField = "Value";
        ddlSearch.DataValueField = "Key";
        ddlSearch.DataBind();
    }

    private void BindControls()
    {
        #region Status
        ddlInvStatus.Items.Add(new ListItem("Active", "0"));
        ddlInvStatus.Items.Add(new ListItem("Inactive", "1"));
        ddlInvStatus.Items.Add(new ListItem("On Hold", "2"));

        #endregion

        #region ABC Class
        ddlABC.Items.Add(new ListItem("None", "0"));
        ddlABC.Items.Add(new ListItem("Class A", "A"));
        ddlABC.Items.Add(new ListItem("Class B", "B"));
        ddlABC.Items.Add(new ListItem("Class C", "C"));
        #endregion

        #region commodity
        Commodity objBL_commodity = new Commodity();
        objBL_commodity.ConnConfig = Session["config"].ToString();

        _ReadAllCommodity.ConnConfig = Session["config"].ToString();

        DataSet dscommodity = new DataSet();
        List <CommodityViewModel> _lstCommodity = new List<CommodityViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "InventoryAPI/InventoryList_ReadAllCommodity";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ReadAllCommodity);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstCommodity = serializer.Deserialize<List<CommodityViewModel>>(_APIResponse.ResponseData);
            dscommodity = CommonMethods.ToDataSet<CommodityViewModel>(_lstCommodity);
        }
        else
        {
            dscommodity = objBL_Inventory.ReadAllCommodity(objBL_commodity);
        }

        if (dscommodity != null)
        {
            if (dscommodity.Tables.Count > 0)
            {
                if (dscommodity.Tables[0].Rows.Count > 0)
                {
                    var dtcommodity = from c in dscommodity.Tables[0].Select() select new { Id = c[0], DisplayVal = c[1] + " - " + c[2] };

                    ddlCommodity.DataSource = dtcommodity;
                    ddlCommodity.DataTextField = "DisplayVal";
                    ddlCommodity.DataValueField = "Id";
                    ddlCommodity.DataBind();
                    ddlCommodity.Items.Add(new ListItem("Select Commodity", "0"));
                    ddlCommodity.SelectedValue = "0";
                }
            }
        }
        #endregion

        #region Vendors

        objBL_Inventory = new BL_Inventory();
        Dictionary<string, string> lstvendor = objBL_Inventory.GetAllVendor(Session["config"].ToString());
        ddlInventoryApprovedVendor.DataValueField = "Key";
        ddlInventoryApprovedVendor.DataTextField = "Value";
        ddlInventoryApprovedVendor.DataSource = lstvendor;
        ddlInventoryApprovedVendor.DataBind();

        #endregion
    }
    #endregion

    #region ::Events::

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Inv.SelectedItems)
        {
            Label lblUserID = (Label)item.Cells[1].FindControl("lblId");
            Response.Redirect("AddInventory.aspx?uid=" + lblUserID.Text);
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Inv.SelectedItems)
        {
            Label lblUserID = (Label)item.Cells[1].FindControl("lblId");
            Response.Redirect("AddInventory.aspx?uid=" + lblUserID.Text + "&t=c");
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Inv.SelectedItems)
        {
            Label lblInvID = (Label)item.Cells[1].FindControl("lblId");
            DeleteInventory(Convert.ToInt32(lblInvID.Text));
        }
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddInventory.aspx");
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        Session.Remove("ddlSearchInv");
        Session.Remove("ddlSearch_Value_Inv");
        Session.Remove("Inv_FilterExpression");
        Session.Remove("Inv_Filters");

        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        GetInvList(true);

        foreach (GridColumn column in RadGrid_Inv.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }

        ShowHideFilterSearch(ddlSearch.SelectedValue);

        RadGrid_Inv.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Inv.Rebind();
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        #region Search Filter
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearchInv"] = selectedValue;

        if (selectedValue == "ApprovedVendor")
        {
            Session["ddlSearch_Value_Inv"] = ddlInventoryApprovedVendor.SelectedValue;
        }
        else if (selectedValue == "Commodity")
        {
            Session["ddlSearch_Value_Inv"] = ddlCommodity.SelectedValue;
        }
        else if (selectedValue == "ABCClass")
        {
            Session["ddlSearch_Value_Inv"] = ddlABC.SelectedValue;
        }
        else if (selectedValue == "Status")
        {
            Session["ddlSearch_Value_Inv"] = ddlInvStatus.SelectedValue;
        }
        else
        {
            Session["ddlSearch_Value_Inv"] = txtSearch.Text;
        }
        #endregion

        GetInvList();
        RadGrid_Inv.Rebind();
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    #endregion

    #region ::WebMethods::
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetInventory(string searchTerm, string column, string page)
    {
        string strOut = string.Empty;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();
        DataSet dt = null;
        BL_Inventory objBL_JsonInventory = new BL_Inventory();
        BusinessEntity.Inventory objProp_JsonInventory = new BusinessEntity.Inventory();
        int noofrecordsperpage = string.IsNullOrEmpty(System.Web.Configuration.WebConfigurationManager.AppSettings["RecordPerPage"].Trim()) ? 10 : Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["RecordPerPage"].Trim());
        page = string.IsNullOrEmpty(page) ? "1" : page;
        int MaxRecord = (noofrecordsperpage * Convert.ToInt32(page)) - 1;

        int MinRecord = (MaxRecord - noofrecordsperpage) < 0 ? 0 : (MaxRecord - noofrecordsperpage) + 1;

        int pagecount = 1;

        try
        {
            ListGetInventory _lstInventory = new ListGetInventory();
            DataSet dt1 = new DataSet();
            DataSet dt2 = new DataSet();
            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
            GetInventoryParam _GetInventory = new GetInventoryParam();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/InventoryList_GetInventory";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInventory);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstInventory = serializer.Deserialize<ListGetInventory>(_APIResponse.ResponseData);

                dt = _lstInventory.lstTable1.ToDataSet();
                //dt2 = _lstInventory.lstTable2.ToDataSet();
            }
            else
            {
                dt = objBL_JsonInventory.GetInventory(objProp_JsonInventory);
            }


            if (dt != null)
            {
                if (dt.Tables[0].Rows.Count > 0)
                {

                    #region Valid SearchTerms
                    if (!string.IsNullOrEmpty(searchTerm) && dt.Tables[0].Columns[column] != null)
                    {

                        int searchindex = dt.Tables[0].Columns[column].Ordinal;


                        var dr = dt.Tables[0].Rows.Cast<DataRow>().ToList().Select(x => x.ItemArray);

                        var matchedData = (from items in dr select new { Id = items[0], SearchCol = (string)items[searchindex] }).ToList();
                        if (matchedData != null)
                        {
                            //var itemsfound = matchedData.Where(c => SqlMethods.Like(c.SearchCol, "%" + searchTerm + "%")).ToList();
                            var itemsfound = matchedData.Where(c => c.SearchCol.ToUpper().Contains(searchTerm.ToUpper())).ToList();

                            if (itemsfound != null)
                            {
                                if (itemsfound.Count > 0)
                                {
                                    pagecount = Convert.ToInt32(Math.Round((itemsfound.Count / Convert.ToDecimal(noofrecordsperpage))));
                                    List<object> pageditems = new List<object>();

                                    for (int i = 0; i < itemsfound.Count; i++)
                                    {
                                        //Check If the records found are less than the no of reords that needs to be displayed in the gird
                                        if (pagecount > 0)
                                        {
                                            if (i >= MinRecord && i <= MaxRecord)
                                                pageditems.Add(itemsfound[i].Id);
                                        }
                                        else
                                            pageditems.Add(itemsfound[i].Id);

                                    }

                                    dictionary.Add("ID", pageditems.ToArray());
                                    dictionary.Add("PageCount", pagecount > 0 ? pagecount : 1);

                                }
                            }
                        }

                    }

                    #endregion

                    #region Typo
                    else if (!string.IsNullOrEmpty(searchTerm))
                    {
                        var dr = dt.Tables[0].Rows.Cast<DataRow>().ToList().Select(x => x.ItemArray);

                        var matchedData = (from items in dr select new { Id = items[0], PN = (string)items[1], Description = (string)items[2], status = (string)items[4], DateCreated = (string)items[40] }).ToList();
                        if (matchedData != null)
                        {
                            //var itemsfound = matchedData.Where(c => SqlMethods.Like(c.SearchCol, "%" + searchTerm + "%")).ToList();
                            var itemsfound = matchedData.Where(c => c.PN.ToUpper().Contains(searchTerm.ToUpper()) || c.Description.ToUpper().Contains(searchTerm.ToUpper()) || c.status.ToUpper().Contains(searchTerm.ToUpper()) || c.DateCreated.ToUpper().Contains(searchTerm.ToUpper())).ToList();

                            if (itemsfound != null)
                            {
                                if (itemsfound.Count > 0)
                                {
                                    pagecount = Convert.ToInt32(Math.Round((itemsfound.Count / Convert.ToDecimal(noofrecordsperpage))));
                                    List<object> pageditems = new List<object>();

                                    for (int i = 0; i < itemsfound.Count; i++)
                                    {
                                        //Check If the records found are less than the no of reords that needs to be displayed in the gird
                                        if (pagecount > 0)
                                        {
                                            if (i >= MinRecord && i <= MaxRecord)
                                                pageditems.Add(itemsfound[i].Id);
                                        }
                                        else
                                            pageditems.Add(itemsfound[i].Id);

                                    }

                                    dictionary.Add("ID", pageditems.ToArray());
                                    dictionary.Add("PageCount", pagecount > 0 ? pagecount : 1);

                                }
                            }
                        }
                    }
                    #endregion

                    #region Empty Search
                    else
                    {

                        var dr = dt.Tables[0].Rows.Cast<DataRow>().ToList().Select(x => x.ItemArray);

                        var matchedData = (from items in dr select new { Id = items[0] }).ToList();
                        if (matchedData != null)
                        {
                            if (matchedData.Count > 0)
                            {
                                pagecount = Convert.ToInt32(Math.Round((matchedData.Count / Convert.ToDecimal(noofrecordsperpage))));
                                List<object> pageditems = new List<object>();

                                for (int i = 0; i < matchedData.Count; i++)
                                {
                                    //Check If the records found are less than the no of reords that needs to be displayed in the gird
                                    if (pagecount > 0)
                                    {
                                        if (i >= MinRecord && i <= MaxRecord)
                                            pageditems.Add(matchedData[i].Id);
                                    }
                                    else
                                        pageditems.Add(matchedData[i].Id);
                                }
                                dictionary.Add("ID", pageditems.ToArray());
                                dictionary.Add("PageCount", pagecount > 0 ? pagecount : 1);
                            }

                        }


                    }
                    #endregion

                }

            }

            dictionaryList.Add(dictionary);

            strOut = sr.Serialize(dictionary);
        }
        catch (Exception ex)
        {
            dictionary.Add("Success", "0");
            dictionary.Add("ErrMsg", ex.Message);
            dictionary.Add("timestamp", System.DateTime.Now.ToString());
            dictionaryList.Add(dictionary);
            strOut = sr.Serialize(dictionary);
        }

        return strOut;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetInventoryAndSort(string searchTerm, string column, string page, string sortcol)
    {
        string strOut = string.Empty;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();
        DataSet ds = null;
        BL_Inventory objBL_JsonInventory = new BL_Inventory();
        BusinessEntity.Inventory objProp_JsonInventory = new BusinessEntity.Inventory();
        int noofrecordsperpage = string.IsNullOrEmpty(System.Web.Configuration.WebConfigurationManager.AppSettings["RecordPerPage"].Trim()) ? 10 : Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["RecordPerPage"].Trim());
        page = string.IsNullOrEmpty(page) ? "1" : page;
        int MaxRecord = (noofrecordsperpage * Convert.ToInt32(page)) - 1;

        int MinRecord = (MaxRecord - noofrecordsperpage) < 0 ? 0 : (MaxRecord - noofrecordsperpage);

        int pagecount = 1;

        try
        {
            ListGetInventory _lstInventory = new ListGetInventory();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            GetInventoryParam _GetInventory = new GetInventoryParam();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/InventoryList_GetInventory";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInventory);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstInventory = serializer.Deserialize<ListGetInventory>(_APIResponse.ResponseData);

                ds1 = _lstInventory.lstTable1.ToDataSet();
                ds2 = _lstInventory.lstTable2.ToDataSet();
            }
            else
            {
                ds = objBL_JsonInventory.GetInventory(objProp_JsonInventory);
            }

            DataTable dt = ds.Tables[0];
            dt.DefaultView.Sort = sortcol;




            dt = dt.DefaultView.Table.DefaultView.ToTable();
            dt.AcceptChanges();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    #region Valid SearchTerms
                    if (!string.IsNullOrEmpty(searchTerm) && dt.Columns[column] != null)
                    {

                        int searchindex = dt.Columns[column].Ordinal;


                        var dr = dt.Rows.Cast<DataRow>().ToList().Select(x => x.ItemArray);

                        var matchedData = (from items in dr select new { Id = items[0], SearchCol = (string)items[searchindex] }).ToList();
                        if (matchedData != null)
                        {
                            //var itemsfound = matchedData.Where(c => SqlMethods.Like(c.SearchCol, "%" + searchTerm + "%")).ToList();
                            var itemsfound = matchedData.Where(c => c.SearchCol.ToUpper().Contains(searchTerm.ToUpper())).ToList();

                            if (itemsfound != null)
                            {
                                if (itemsfound.Count > 0)
                                {
                                    pagecount = Convert.ToInt32(Math.Round((itemsfound.Count / Convert.ToDecimal(noofrecordsperpage))));
                                    List<object> pageditems = new List<object>();

                                    for (int i = 0; i < itemsfound.Count; i++)
                                    {
                                        //Check If the records found are less than the no of reords that needs to be displayed in the gird
                                        if (pagecount > 0)
                                        {
                                            if (i >= MinRecord && i <= MaxRecord)
                                                pageditems.Add(itemsfound[i].Id);
                                        }
                                        else
                                            pageditems.Add(itemsfound[i].Id);

                                    }

                                    dictionary.Add("ID", pageditems.ToArray());
                                    dictionary.Add("PageCount", pagecount > 0 ? pagecount : 1);

                                }
                            }
                        }

                    }

                    #endregion

                    #region Empty Search
                    else
                    {

                        var dr = dt.Rows.Cast<DataRow>().ToList().Select(x => x.ItemArray);

                        var matchedData = (from items in dr select new { Id = items[0] }).ToList();
                        if (matchedData != null)
                        {
                            if (matchedData.Count > 0)
                            {
                                pagecount = Convert.ToInt32(Math.Round((matchedData.Count / Convert.ToDecimal(noofrecordsperpage))));
                                List<object> pageditems = new List<object>();

                                for (int i = 0; i < matchedData.Count; i++)
                                {
                                    //Check If the records found are less than the no of reords that needs to be displayed in the gird
                                    if (pagecount > 0)
                                    {
                                        if (i >= MinRecord && i <= MaxRecord)
                                            pageditems.Add(matchedData[i].Id);
                                    }
                                    else
                                        pageditems.Add(matchedData[i].Id);
                                }
                                dictionary.Add("ID", pageditems.ToArray());
                                dictionary.Add("PageCount", pagecount > 0 ? pagecount : 1);
                            }

                        }


                    }
                    #endregion

                }

            }

            dictionaryList.Add(dictionary);

            strOut = sr.Serialize(dictionary);
        }
        catch (Exception ex)
        {
            dictionary.Add("Success", "0");
            dictionary.Add("ErrMsg", ex.Message);
            dictionary.Add("timestamp", System.DateTime.Now.ToString());
            dictionaryList.Add(dictionary);
            strOut = sr.Serialize(dictionary);
        }

        return strOut;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string DeleteInventory(object Items)
    {
        string strOut = string.Empty;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();

        BL_Inventory objBL_JsonInventory = new BL_Inventory();
        BusinessEntity.Inventory objProp_JsonInventory = new BusinessEntity.Inventory();
        try
        {
            object[] itemsarray = Items as object[];
            string strxml = string.Empty;
            if (itemsarray.Length > 0)
            {

                strxml += "<Inventory>";
                for (int i = 0; i < itemsarray.Length; i++)
                {
                    strxml += "<Inventory> <ID>" + itemsarray[i] + "</ID> </Inventory>";
                }
                strxml += "</Inventory>";
            }

            DataSet dsresult = objBL_JsonInventory.DeleteInventory(strxml);
            System.Collections.Generic.List<int> ids = new List<int>();
            if (dsresult != null)
            {
                if (dsresult.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsresult.Tables[0].Rows.Count; i++)
                    {
                        ids.Add((int)dsresult.Tables[0].Rows[i]["ID"]);
                    }
                }
            }

            if (ids.Count > 0)
            {
                dictionary.Add("Error", "1");
                dictionary.Add("ID", ids.ToArray());
            }
            else
            {
                dictionary.Add("Success", "1");
            }
        }
        catch (Exception ex)
        {
            dictionary.Add("Success", "0");
            dictionary.Add("ErrMsg", ex.Message);
            dictionary.Add("timestamp", System.DateTime.Now.ToString());
            dictionaryList.Add(dictionary);

        }
        strOut = sr.Serialize(dictionary);

        return strOut;
    }

    #endregion

    protected void lnkchk_Click(object sender, EventArgs e)
    {
        GetInvList();
        RadGrid_Inv.Rebind();
    }

    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_Inv.Items)
        {
            Label lblUserID = (Label)gr.Cells[1].FindControl("lblId");
            HyperLink lnkWarehouseCount = (HyperLink)gr.FindControl("lnkWarehouseCount");
            lnkWarehouseCount.NavigateUrl = "AddInventory.aspx?uid=" + lblUserID.Text + "&TabWarehouse=TabWarehouse";
            gr.Attributes["ondblclick"] = "window.open('AddInventory.aspx?uid=" + lblUserID.Text + "','_self');";
        }
    }

    private void DeleteInventory(int InvID)
    {
        String RetVal = "";
        objProp_Inventory.ID = InvID;
        objProp_Inventory.ConnConfig = Session["config"].ToString();

        _DeleteInventoryByInvID.ID = InvID;
        _DeleteInventoryByInvID.ConnConfig = Session["config"].ToString();

        try
        {
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/InventoryList_DeleteInventoryByInvID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteInventoryByInvID);

                RetVal = Convert.ToString(_APIResponse.ResponseData);
            }
            else
            {
                RetVal = objBL_Inventory.DeleteInventoryByInvID(objProp_Inventory);
            }

            if (RetVal == "false")
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Inventory Item deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                GetInvList();
                RadGrid_Inv.Rebind();
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: '" + RetVal + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    public void CorrectTelerikPager(RadGrid grid)
    {
        //Fix page count in pager
        if (grid.MasterTableView.Items.Count == 0)
        {
            grid.MasterTableView.PagerStyle.PagerTextFormat = "{4}<strong>0</strong> items in <strong>0</strong> pages";
        }
        else if (grid.MasterTableView.Items.Count == 1)
        {
            grid.MasterTableView.PagerStyle.PagerTextFormat = "{4}<strong>{5}</strong> item in <strong>1</strong> page";
        }
        else if (grid.PageSize == int.MaxValue || grid.PageCount <= 1)
        {
            grid.MasterTableView.PagerStyle.PagerTextFormat = "{4}<strong>{5}</strong> items in <strong>1</strong> page";
        }
        else
        {
            grid.MasterTableView.PagerStyle.PagerTextFormat = "{4}<strong>{5}</strong> items in <strong>{1}</strong> pages";
        }

        grid.Rebind();
    }

    protected void RadGrid_Inv_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Inv.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Inv_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Inv.MasterTableView.OwnerGrid.Columns)
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
            Session["Inv_Filters"] = filters;
        }
        else
        {
            Session["Inv_FilterExpression"] = null;
            Session["Inv_Filters"] = null;
        }

        if (intExportExcel == 1)
        {
            GeneralFunctions obj = new GeneralFunctions();
            obj.CorrectTelerikPager(RadGrid_Inv);
            intExportExcel = 0;
        }

        RowSelect();
    }

    protected void RadGrid_Inv_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Inv.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        if (!IsPostBack)
        {
            if (Session["Inv_FilterExpression"] != null && Convert.ToString(Session["Inv_FilterExpression"]) != "" && Session["Inv_Filters"] != null)
            {
                RadGrid_Inv.MasterTableView.FilterExpression = Convert.ToString(Session["Inv_FilterExpression"]);
                var filtersGet = Session["Inv_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Inv.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }
            else
            {
                Session["Inv_FilterExpression"] = null;
                Session["Inv_Filters"] = null;
            }
        }
        else
        {
            String filterExpression = Convert.ToString(RadGrid_Inv.MasterTableView.FilterExpression);
            if (filterExpression != "")
            {
                Session["Inv_FilterExpression"] = filterExpression;
                List<RetainFilter> filters = new List<RetainFilter>();

                foreach (GridColumn column in RadGrid_Inv.MasterTableView.OwnerGrid.Columns)
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

                Session["Inv_Filters"] = filters;
            }
            else
            {
                Session["Inv_FilterExpression"] = null;
                Session["Inv_Filters"] = null;
            }
        }

        GetInvList();
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Inv.MasterTableView.FilterExpression != "" ||
            (RadGrid_Inv.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
             RadGrid_Inv.MasterTableView.SortExpressions.Count > 0;
    }

    private void GetInvList(Boolean IsShowAll = false)
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        DataTable dtFinal = new DataTable();

        if (IsShowAll == true)
        {
            objProp_Inventory.ConnConfig = Session["config"].ToString();
            _GetALLInventory.ConnConfig = Session["config"].ToString();

            ListGetALLInventory _lstInventory = new ListGetALLInventory();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/InventoryList_GetALLInventory";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetALLInventory);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstInventory = serializer.Deserialize<ListGetALLInventory>(_APIResponse.ResponseData);
                ds = _lstInventory.lstTable1.ToDataSet();
            }
            else
            {
                ds = objBL_Inventory.GetAllInventory(objProp_Inventory);
            }
            
            Session.Remove("ddlSearchInv");
            Session.Remove("ddlSearch_Value_Inv");
            Session.Remove("Inv_FilterExpression");
            Session.Remove("Inv_Filters");
        }
        else
        {
            objProp_Inventory.ConnConfig = Session["config"].ToString();
            objProp_Inventory.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objProp_Inventory.SearchField = ddlSearch.SelectedValue;

            _GetInventory.ConnConfig = Session["config"].ToString();
            _GetInventory.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _GetInventory.SearchField = ddlSearch.SelectedValue;

            _GetSearchInventory.ConnConfig = Session["config"].ToString();
            _GetSearchInventory.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _GetSearchInventory.SearchField = ddlSearch.SelectedValue;

            if (ddlSearch.SelectedValue == "Status")
            {
                objProp_Inventory.SearchValue = ddlInvStatus.SelectedValue;
                _GetInventory.SearchValue = ddlInvStatus.SelectedValue;
                _GetSearchInventory.SearchValue = ddlInvStatus.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "ABCClass")
            {
                objProp_Inventory.SearchValue = ddlABC.SelectedValue;
                _GetInventory.SearchValue = ddlABC.SelectedValue;
                _GetSearchInventory.SearchValue = ddlABC.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "Commodity")
            {
                objProp_Inventory.SearchValue = ddlCommodity.SelectedValue;
                _GetInventory.SearchValue = ddlCommodity.SelectedValue;
                _GetSearchInventory.SearchValue = ddlCommodity.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "ApprovedVendor")
            {
                objProp_Inventory.SearchValue = ddlInventoryApprovedVendor.SelectedValue;
                _GetInventory.SearchValue = ddlInventoryApprovedVendor.SelectedValue;
                _GetSearchInventory.SearchValue = ddlInventoryApprovedVendor.SelectedValue;
            }
            else
            {
                objProp_Inventory.SearchValue = txtSearch.Text;
                _GetInventory.SearchValue = txtSearch.Text;
                _GetSearchInventory.SearchValue = txtSearch.Text;
            }

            if (Session["CmpChkDefault"].ToString() == "1")
            {
                objProp_Inventory.EN = 1;
                _GetInventory.EN = 1;
                _GetSearchInventory.EN = 1;
            }
            else
            {
                objProp_Inventory.EN = 0;
                _GetInventory.EN = 0;
                _GetSearchInventory.EN = 0;
            }

            if (lnkChk.Checked)
            {
                objProp_Inventory.Status = 1;
                _GetInventory.Status = 1;
                _GetSearchInventory.Status = 1;
            }
            else
            {
                objProp_Inventory.Status = 0;
                _GetInventory.Status = 0;
                _GetSearchInventory.Status = 0;
            }

            if (ddlSearch.SelectedValue == "Select")
            {
                ListGetInventory _lstInventory = new ListGetInventory();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "InventoryAPI/InventoryList_GetInventory";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInventory);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstInventory = serializer.Deserialize<ListGetInventory> (_APIResponse.ResponseData);

                    ds = _lstInventory.lstTable1.ToDataSet();
                    //ds2 = _lstInventory.lstTable2.ToDataSet();
                }
                else
                {
                    ds = objBL_Inventory.GetInventory(objProp_Inventory);
                }

            }
            else
            {
                ListGetSearchInventory _lstSearchInventory = new ListGetSearchInventory();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "InventoryAPI/InventoryList_GetSearchInventory";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetSearchInventory);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstSearchInventory = serializer.Deserialize<ListGetSearchInventory>(_APIResponse.ResponseData);
                    ds = _lstSearchInventory.lstTable1.ToDataSet();
                }
                else
                {
                    ds = objBL_Inventory.GetSearchInventory(objProp_Inventory);
                }

            }

        }

        if (ds.Tables.Count > 1)
        {
            ds.Tables.RemoveAt(1);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (lnkChk.Checked)
            {
                dtFinal = ds.Tables[0];
            }
            else
            {
                var dt = ds.Tables[0].Select("Status = 0");
                if (dt != null && dt.Length > 0)
                {
                    dtFinal = dt.CopyToDataTable();
                }
            }
        }

        DataTable result = ProcessDataFilter(dtFinal);

        RadGrid_Inv.VirtualItemCount = result.Rows.Count;
        RadGrid_Inv.DataSource = result;
        Session["InventoryList"] = result;
        // Filter Expression
        if (Request.QueryString["fil"] == "1")
        {
            if (Session["Inv_FilterExpression"] != null && Convert.ToString(Session["Inv_FilterExpression"]) != "" && Session["Inv_Filters"] != null)
            {
                RadGrid_Inv.MasterTableView.FilterExpression = Convert.ToString(Session["Inv_FilterExpression"]);
                var filtersGet = Session["Inv_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Inv.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }
        }
        else
        {
            Session.Remove("Inv_FilterExpression");
            Session.Remove("Inv_Filters");
        }
    }

    protected void RadGrid_Inv_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (Convert.ToString(RadGrid_Inv.MasterTableView.FilterExpression) != "")
                    lblRecordCount.Text = totalCount + " Record(s) found";
                else
                    lblRecordCount.Text = RadGrid_Inv.VirtualItemCount + " Record(s) found";
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

    protected void RadGrid_Inv_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }

        lblRecordCount.Text = rowCount + " Record(s) found";
    }

    protected void RadGrid_Inv_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 4;

        if (e.Worksheet.Table.Rows.Count == RadGrid_Inv.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Inv.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
            RowElement row = new RowElement();

            //create new row for the footer aggregates
            for (int i = currentItem; i < footerItem.Cells.Count; i++)
            {
                TableCell fcell = footerItem.Cells[i];

                CellElement cell = new CellElement();
                if (i == currentItem)
                    cell.Data.DataItem = "Total:-";
                else
                    cell.Data.DataItem = fcell.Text == "&nbsp;" ? "" : fcell.Text;

                row.Cells.Add(cell);
            }

            e.Worksheet.Table.Rows.Add(row);
        }
    }

    private DataTable ProcessDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1 = 1";
            if (Session["Inv_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                var filtersGet = Session["Inv_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {

                        GridColumn column = RadGrid_Inv.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        if (column.UniqueName == "Hand"
                            || column.UniqueName == "fOrder"
                            || column.UniqueName == "Committed"
                            || column.UniqueName == "Available"
                            || column.UniqueName == "WarehouseCount"
                            || column.UniqueName == "UnitCost"
                            || column.UniqueName == "Balance")
                        {
                            sql = sql + " And " + column.UniqueName + "=" + _filter.FilterValue;
                        }
                        else
                        {
                            sql = sql + " And " + column.UniqueName + " like '%" + _filter.FilterValue + "%'";
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
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        intExportExcel = 1;
        RadGrid_Inv.ExportSettings.FileName = "Inventory";
        RadGrid_Inv.ExportSettings.IgnorePaging = true;
        RadGrid_Inv.ExportSettings.ExportOnlyData = true;
        RadGrid_Inv.ExportSettings.OpenInNewWindow = true;
        RadGrid_Inv.ExportSettings.HideStructureColumns = true;
        RadGrid_Inv.MasterTableView.UseAllDataFields = true;
        RadGrid_Inv.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Inv.MasterTableView.ExportToExcel();
    }

    private void ShowHideFilterSearch(String selectedValue)
    {
        div_txtSearch.Style.Add("display", "none");
        div_rbStatus.Style.Add("display", "none");
        div_rbABC.Style.Add("display", "none");
        div_rbCommodity.Style.Add("display", "none");
        div_rbApprovedVendor.Style.Add("display", "none");

        switch (selectedValue)
        {
            case "Status":
                div_rbStatus.Style.Add("display", "block");
                break;
            case "ABCClass":
                div_rbABC.Style.Add("display", "block");
                break;
            case "Commodity":
                div_rbCommodity.Style.Add("display", "block");
                break;
            case "ApprovedVendor":
                div_rbApprovedVendor.Style.Add("display", "block");
                break;
            default:
                div_txtSearch.Style.Add("display", "block");
                break;
        }
    }

    protected void lnkInventoryReport_Click(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Inv.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Inv_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Inv.MasterTableView.OwnerGrid.Columns)
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

            Session["Inv_Filters"] = filters;
        }
        else
        {
            Session["Inv_FilterExpression"] = null;
            Session["Inv_Filters"] = null;
        }

        var searchText = string.Empty;
        if (ddlSearch.SelectedValue == "Status")
        {
            searchText = ddlInvStatus.SelectedValue;
        }
        if (ddlSearch.SelectedValue == "ABCClass")
        {
            searchText = ddlABC.SelectedValue;
        }
        if (ddlSearch.SelectedValue == "Commodity")
        {
            searchText = ddlCommodity.SelectedValue;
        }
        if (ddlSearch.SelectedValue == "ApprovedVendor")
        {
            searchText = ddlInventoryApprovedVendor.SelectedValue;
        }
        else
        {
            searchText = txtSearch.Text;
        }

        string urlString = "InventoryReport.aspx?stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText + "&inclInactive=" + lnkChk.Checked;

        Response.Redirect(urlString, true);
    }
}

