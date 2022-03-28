using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using Telerik.Web.UI;
using System.Threading;
using System.Web.Script.Serialization;
using BusinessEntity.Utility;
using MOMWebApp;
using BusinessEntity.InventoryModel;

public partial class InventoryAdjustments : System.Web.UI.Page
{

    private Dictionary<string, string> SearchList;

    private Hashtable HsSearchList;

    BL_User objBL_User = new BL_User();
    BL_Inventory objBL_Inventory = new BL_Inventory();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BusinessEntity.InventoryAdjustment objProp_InventoryAdjustment = new BusinessEntity.InventoryAdjustment();

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetAllInventoryAdjustmentByDateParam _GetAllInvAdjustmentByDate = new GetAllInventoryAdjustmentByDateParam();
    DeleteAdjustmentParam _DeleteAdjustment = new DeleteAdjustmentParam();


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }
            Permission();

            HighlightSideMenu("InventoryMgr", "lnkAdjustment", "ulInventoryMgr");
            if (!IsPostBack)
            {
                string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

                if (Request.Url.Scheme == "http" && SSL == "1")
                {
                    string URL = Request.Url.ToString();
                    URL = URL.Replace("http://", "https://");
                    Response.Redirect(URL);
                }

                var now = DateTime.Now;
                var startDate = now.AddDays(-((now.DayOfWeek - Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7)).Date;
                txtStartDate.Text = startDate.ToShortDateString();
                var endDate = startDate.AddDays(7).AddSeconds(-1);

                txtEndDate.Text = endDate.ToShortDateString();
                lblWeek.Attributes["class"] = "labelactive";
                //Session["fromDate"] = txtStartDate.Text;
                //Session["ToDate"] = txtEndDate.Text;                
                #region Show Selected Filter
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["ddlSearch_InvAdj"] != null)
                    {
                        String selectedValue = Convert.ToString(Session["ddlSearch_InvAdj"]);
                        ddlSearch.SelectedValue = selectedValue;
                        if (Session["ddlSearch_Value_InvAdj"]!=null)
                        {
                            txtSearch.Text = Session["ddlSearch_Value_InvAdj"].ToString();
                        }                        
                    }
                    if(Session["fromDate"] != null && Session["ToDate"] != null)
                    {
                        txtStartDate.Text = Convert.ToDateTime(Session["fromDate"].ToString()).ToShortDateString();
                        txtEndDate.Text = Convert.ToDateTime(Session["ToDate"].ToString()).ToShortDateString();
                    }
                }
                else
                {
                    Session["ddlSearch_InvAdj"] = null;
                    Session["ddlSearch_Value_InvAdj"] = null;
                    Session["fromDate"] = txtStartDate.Text;
                    Session["ToDate"] = txtEndDate.Text;
                }
                #endregion

                BindSearchItems();
                if(Session["InventoryAdj_SuccessCount"] != null)
                {
                    var addSuccessCount = (int)Session["InventoryAdj_SuccessCount"];
                    if (addSuccessCount > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDel", "noty({text: ' Adjustments Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    // Clean session after showing message;
                    Session["InventoryAdj_SuccessCount"] = null;
                }
                
            }          

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    #region ::Method::


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
      

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

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
        if (Session["type"].ToString() != "am")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            //Inventory item
            string InventoryAdjustmentPermission = ds.Rows[0]["InvAdj"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["InvAdj"].ToString();
            hdnAddInventoryAdjustment.Value = InventoryAdjustmentPermission.Length < 1 ? "Y" : InventoryAdjustmentPermission.Substring(0, 1);
            //hdnEditInventoryItem.Value = InventoryItemPermission.Length < 2 ? "Y" : InventoryItemPermission.Substring(1, 1);
            //hdnDeleteInventoryItem.Value = InventoryItemPermission.Length < 3 ? "Y" : InventoryItemPermission.Substring(2, 1);
            //hdnViewInventoryItem.Value = InventoryItemPermission.Length < 4 ? "Y" : InventoryItemPermission.Substring(3, 1);

        }

        //DataTable dt = new DataTable();
        //dt = (DataTable)Session["userinfo"];

        //string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
        //if (ProgFunc == "N")
        //{
        //    Response.Redirect("home.aspx");
        //}
    }

    private void GetDataAll()
    {

        var ds = new DataSet();
        objProp_InventoryAdjustment.ConnConfig = Session["config"].ToString();//Session["dbname"].ToString();

        _GetAllInvAdjustmentByDate.ConnConfig = Session["config"].ToString();

        List<GetAllInvAdjustmentByDateViewModel> _lstGetAllInvAdjustmentByDate = new List<GetAllInvAdjustmentByDateViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "ItemAdjustmentAPI/InventoryAdjustmentsList_GetAllInventoryAdjustmentByDate";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllInvAdjustmentByDate);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetAllInvAdjustmentByDate = serializer.Deserialize<List<GetAllInvAdjustmentByDateViewModel>>(_APIResponse.ResponseData);

            ds = CommonMethods.ToDataSet<GetAllInvAdjustmentByDateViewModel>(_lstGetAllInvAdjustmentByDate);

        }
        else
        {
            ds = objBL_Inventory.GetAllInventoryAdjustmentByDate(objProp_InventoryAdjustment);
        }


        if (Session["InventoryAdj"] != null)
            Session["InventoryAdj"] = null;
        if (Session["searchdata"] != null)
            Session["searchdata"] = null;

        if (ds.Tables.Count > 0)
        {
            int pagecount = 1;
            int noofrecordsperpage = string.IsNullOrEmpty(System.Web.Configuration.WebConfigurationManager.AppSettings["RecordPerPage"].Trim()) ? 10 : Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["RecordPerPage"].Trim()); ;
            lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString() + " Record(s) found.";
            //  BindGridDatatable(ds.Tables[0]);

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

            pagecount = Convert.ToInt32(Math.Round((ds.Tables[0].Rows.Count / Convert.ToDecimal(noofrecordsperpage))));
            Dictionary<int, string> listpages = new Dictionary<int, string>();
            for (int i = 0; i < pagecount; i++)
            {
                int val = (i + 1);
                listpages.Add(val, val.ToString());
            }

            //ddlPages.DataSource = listpages;
            //ddlPages.DataTextField = "Value";
            //ddlPages.DataValueField = "Key";
            //ddlPages.DataBind();
        }



        ddlSearch.DataSource = SearchList;
        ddlSearch.DataTextField = "Value";
        ddlSearch.DataValueField = "Key";
        ddlSearch.DataBind();

    }
    private void BindSearchItems()
    {
        Dictionary<string, string> listsearchitems = new Dictionary<string, string>();
        listsearchitems.Add("0", "Select");
        listsearchitems.Add("Name", "Item");
        listsearchitems.Add("fDesc", "Description");


        ddlSearch.DataSource = listsearchitems;
        ddlSearch.DataTextField = "Value";
        ddlSearch.DataValueField = "Key";
        ddlSearch.DataBind();

    }
    #endregion

    #region ::Events::

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        RadGrid_InventoryAdjustment.Rebind();
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
        // Session.Remove("PO");
    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddInventoryAdjustment.aspx");
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        //Response.Redirect("AddInventoryAdjustment.aspx");
        if(RadGrid_InventoryAdjustment.SelectedItems.Count == 0)
        {
            return;
        }

        try
        {
            foreach (GridDataItem item in RadGrid_InventoryAdjustment.SelectedItems)
            {
                Label lblCustID = (Label)item.FindControl("lblId");
                Label lblDate = (Label)item.FindControl("lblDate");
                if (CommonHelper.GetPeriodDetails(Convert.ToDateTime(lblDate.Text)))
                {
                    DeleteAdjustment(Convert.ToInt32(lblCustID.Text));
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Adjustment deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                    RadGrid_InventoryAdjustment.Rebind();
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: ' These month/year period is closed out. You do not have permission to delete this record.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }               
            }

           
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DeleteAdjustment(int id)
    {
        BL_Inventory _objBLInventory = new BL_Inventory();
        InventoryAdjustment _objInventoryAdjustment = new InventoryAdjustment();
        _objInventoryAdjustment.ConnConfig = WebBaseUtility.ConnectionString;
        _objInventoryAdjustment.ID = id;

        _DeleteAdjustment.ConnConfig = WebBaseUtility.ConnectionString;
        _DeleteAdjustment.ID = id;

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "ItemAdjustmentAPI/InventoryAdjustmentsList_DeleteAdjustment";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteAdjustment);
        }
        else
        {
            _objBLInventory.DeleteAdjustment(_objInventoryAdjustment);
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {

    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        rdDay.Checked = false;
        rdWeek.Checked = false;
        rdMonth.Checked = false;
        rdQuarter.Checked = false;
        rdYear.Checked = false;

        if (!IsDateSelectionValid())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showMessage", string.Format("ShowMessage('{0}','{1}');", "Invalid date format.", "error"), true);
            RadGrid_InventoryAdjustment.Rebind();
            return;
        }

        #region Search Filter
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_InvAdj"] = selectedValue;
        Session["ddlSearch_Value_InvAdj"] = txtSearch.Text;
        Session["fromDate"] = txtStartDate.Text;
        Session["ToDate"] = txtEndDate.Text;
        #endregion
        
        RadGrid_InventoryAdjustment.Rebind();
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_InventoryAdjustment.SelectedItems)
        {
            Label lblID = (Label)di.FindControl("lblId");
            Response.Redirect("AddInventoryAdjustment.aspx?id=" + lblID.Text);
        }
    }
    #endregion

    #region ::Query data::

    private void QueryData()
    {
        var searchTerm = txtSearch.Text;
        var column = ddlSearch.SelectedValue;
        var startDate = txtStartDate.Text + " 00:00:00"; 
        var endDate = txtEndDate.Text + " 23:59:59";
        GetAdj(searchTerm, column, startDate, endDate);
    }

    private bool IsDateSelectionValid()
    {
        DateTime result;
        return DateTime.TryParse(txtStartDate.Text, out result) && DateTime.TryParse(txtEndDate.Text, out result);
    }

    private void GetAdj(string searchTerm, string column, string stdate, string enddate)
    {

        if (!IsDateSelectionValid())
        {
            RadGrid_InventoryAdjustment.DataSource = string.Empty;
            RadGrid_InventoryAdjustment.VirtualItemCount = 0;
            return;
        }

        BL_Inventory _objBLInventory = new BL_Inventory();
        InventoryAdjustment _objInventoryAdjustment = new InventoryAdjustment();

       

        _objInventoryAdjustment.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        _GetAllInvAdjustmentByDate.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        if (HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
        {
            _objInventoryAdjustment.EN = 1;
            _GetAllInvAdjustmentByDate.EN = 1;
        }
        else
        {
            _objInventoryAdjustment.EN = 0;
            _GetAllInvAdjustmentByDate.EN = 0;
        }

        _objInventoryAdjustment.ConnConfig = WebBaseUtility.ConnectionString;
        _GetAllInvAdjustmentByDate.ConnConfig = WebBaseUtility.ConnectionString;
        try
        {
            _objInventoryAdjustment.Stdate = stdate;
            _objInventoryAdjustment.Enddate = enddate;

            _GetAllInvAdjustmentByDate.Stdate = stdate;
            _GetAllInvAdjustmentByDate.Enddate = enddate;

            DataSet ds = new DataSet();

            List<GetAllInvAdjustmentByDateViewModel> _lstGetAllInvAdjustmentByDate = new List<GetAllInvAdjustmentByDateViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ItemAdjustmentAPI/InventoryAdjustmentsList_GetAllInventoryAdjustmentByDate";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllInvAdjustmentByDate);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetAllInvAdjustmentByDate = serializer.Deserialize<List<GetAllInvAdjustmentByDateViewModel>>(_APIResponse.ResponseData);

                ds = CommonMethods.ToDataSet<GetAllInvAdjustmentByDateViewModel>(_lstGetAllInvAdjustmentByDate);

            }
            else
            {
                ds = _objBLInventory.GetAllInventoryAdjustmentByDate(_objInventoryAdjustment);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                var searchedDt = SearchByColumns(column, searchTerm, ds.Tables[0]);
                RadGrid_InventoryAdjustment.DataSource = searchedDt;
                RadGrid_InventoryAdjustment.MasterTableView.AllowFilteringByColumn = true;
                //RadGrid_InventoryAdjustment.Rebind();
                lblRecordCount.Text = searchedDt.Rows.Count + " Record(s) Found.";
                Session["InventoryAdjustments"] = searchedDt;
                RadGrid_InventoryAdjustment.VirtualItemCount = searchedDt.Rows.Count;
                RadPersistence1.SaveState();
            }
            else
            {
                RadGrid_InventoryAdjustment.DataSource = string.Empty;
                RadGrid_InventoryAdjustment.VirtualItemCount = 0;
                lblRecordCount.Text = 0 + " Record(s) Found.";
            }

        }
        catch
        {
            RadGrid_InventoryAdjustment.DataSource = string.Empty;
            RadGrid_InventoryAdjustment.VirtualItemCount = 0;
            //TODO: Log error
        }

    }

    private DataTable SearchByColumns(string column,string searchTerm, DataTable input)
    {
        if(string.IsNullOrWhiteSpace(column) || column == "0" || string.IsNullOrWhiteSpace(searchTerm))
        {
            return input;
        }

        var output = input.AsEnumerable()
            .Where(r => r.Field<String>(column).ToLower().Contains(searchTerm.ToLower()));

        if(output.Any())
        {
            return output.CopyToDataTable();
        }

        return input.Clone();
    }


    #endregion
    protected void RadGrid_InventoryAdjustment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["InvAdj_FilterExpression"] != null && Convert.ToString(Session["InvAdj_FilterExpression"]) != "" && Session["InvAdj_Filters"] != null)
                {
                    RadGrid_InventoryAdjustment.MasterTableView.FilterExpression = Convert.ToString(Session["InvAdj_FilterExpression"]);
                    var filtersGet = Session["InvAdj_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_InventoryAdjustment.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["InvAdj_FilterExpression"] = null;
                Session["InvAdj_Filters"] = null;
            }
        }
        #endregion

        QueryData();
    }

    protected void incDate_Click(object sender, EventArgs e)
    {
        if (rdDay.Checked)
        {
            txtStartDate.Text = Convert.ToDateTime(txtStartDate.Text).AddDays(1).ToShortDateString();
            txtEndDate.Text = Convert.ToDateTime(txtEndDate.Text).AddDays(1).ToShortDateString();
            Session["ToDate"] = txtEndDate.Text;
            Session["fromDate"] = txtStartDate.Text;
        }

        if (rdWeek.Checked)
        {

            txtStartDate.Text = Convert.ToDateTime(txtStartDate.Text).AddDays(7).ToShortDateString();
            txtEndDate.Text = Convert.ToDateTime(txtEndDate.Text).AddDays(7).ToShortDateString();
            Session["ToDate"] = txtEndDate.Text;
            Session["fromDate"] = txtStartDate.Text;
        }
        if (rdMonth.Checked)
        {
            var first = Convert.ToDateTime(txtStartDate.Text).AddMonths(1);
            txtStartDate.Text = new DateTime(first.Year, first.Month, 1).ToShortDateString();

            var last = Convert.ToDateTime(txtEndDate.Text).AddMonths(1);
            var lastDateInMonth = DateTime.DaysInMonth(last.Year, last.Month);
            txtEndDate.Text = new DateTime(last.Year, last.Month, lastDateInMonth).ToShortDateString();

            Session["ToDate"] = txtEndDate.Text;
            Session["fromDate"] = txtStartDate.Text;
        }
        if (rdQuarter.Checked)
        {
            var first = Convert.ToDateTime(txtStartDate.Text).AddMonths(3);
            txtStartDate.Text = new DateTime(first.Year, first.Month, 1).ToShortDateString();

            var last = Convert.ToDateTime(txtEndDate.Text).AddMonths(3);
            var lastDateInMonth = DateTime.DaysInMonth(last.Year, last.Month);
            txtEndDate.Text = new DateTime(last.Year, last.Month, lastDateInMonth).ToShortDateString();
            Session["ToDate"] = txtEndDate.Text;
            Session["fromDate"] = txtStartDate.Text;
        }
        if (rdYear.Checked)
        {
            txtStartDate.Text = Convert.ToDateTime(txtStartDate.Text).AddYears(1).ToShortDateString();
            txtEndDate.Text = Convert.ToDateTime(txtEndDate.Text).AddYears(1).ToShortDateString();
            Session["ToDate"] = txtEndDate.Text;
            Session["fromDate"] = txtStartDate.Text;
        }

    }
    protected void decDate_Click(object sender, EventArgs e)
    {
        if (rdDay.Checked)
        {
            txtStartDate.Text = Convert.ToDateTime(txtStartDate.Text).AddDays(-1).ToShortDateString();
            txtEndDate.Text = Convert.ToDateTime(txtEndDate.Text).AddDays(-1).ToShortDateString();
            Session["ToDate"] = txtEndDate.Text;
            Session["fromDate"] = txtStartDate.Text;
        }

        if (rdWeek.Checked)
        {

            txtStartDate.Text = Convert.ToDateTime(txtStartDate.Text).AddDays(-7).ToShortDateString();
            txtEndDate.Text = Convert.ToDateTime(txtEndDate.Text).AddDays(-7).ToShortDateString();
            Session["ToDate"] = txtEndDate.Text;
            Session["fromDate"] = txtStartDate.Text;
        }
        if (rdMonth.Checked)
        {

            var first = Convert.ToDateTime(txtStartDate.Text).AddMonths(-1);
            txtStartDate.Text = new DateTime(first.Year, first.Month, 1).ToShortDateString();

            var last = Convert.ToDateTime(txtEndDate.Text).AddMonths(-1);
            var lastDateInMonth = DateTime.DaysInMonth(last.Year, last.Month);
            txtEndDate.Text = new DateTime(last.Year, last.Month, lastDateInMonth).ToShortDateString();
            Session["ToDate"] = txtEndDate.Text;
            Session["fromDate"] = txtStartDate.Text;
        }
        if (rdQuarter.Checked)
        {
            var first = Convert.ToDateTime(txtStartDate.Text).AddMonths(-3);
            txtStartDate.Text = new DateTime(first.Year, first.Month, 1).ToShortDateString();

            var last = Convert.ToDateTime(txtEndDate.Text).AddMonths(-3);
            var lastDateInMonth = DateTime.DaysInMonth(last.Year, last.Month);
            txtEndDate.Text = new DateTime(last.Year, last.Month, lastDateInMonth).ToShortDateString();
            Session["ToDate"] = txtEndDate.Text;
            Session["fromDate"] = txtStartDate.Text;
        }

        if (rdYear.Checked)
        {
            txtStartDate.Text = Convert.ToDateTime(txtStartDate.Text).AddYears(-1).ToShortDateString();
            txtEndDate.Text = Convert.ToDateTime(txtEndDate.Text).AddYears(-1).ToShortDateString();
            Session["ToDate"] = txtEndDate.Text;
            Session["fromDate"] = txtStartDate.Text;
        }

    }
    protected void rdDay_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblDay.Attributes["class"] = "labelactive";
        txtStartDate.Text = DateTime.Now.ToShortDateString();
        txtEndDate.Text = DateTime.Now.ToShortDateString();
        Session["ToDate"] = txtEndDate.Text;
        Session["fromDate"] = txtStartDate.Text;
    }
    protected void rdWeek_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblWeek.Attributes["class"] = "labelactive";
        var now = System.DateTime.Now;
        var FisrtDay = now.AddDays(-((now.DayOfWeek - System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7)).Date;
        txtStartDate.Text = FisrtDay.ToShortDateString();
        var LastDay = FisrtDay.AddDays(7).AddSeconds(-1);

        txtEndDate.Text = LastDay.ToShortDateString();
        Session["ToDate"] = txtEndDate.Text;
        Session["fromDate"] = txtStartDate.Text;
    }

    protected void rdMonth_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblMonth.Attributes["class"] = "labelactive";
        var Date = System.DateTime.Now;
        var firstDayOfMonth = new DateTime(Date.Year, Date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        txtStartDate.Text = firstDayOfMonth.ToShortDateString();
        txtEndDate.Text = lastDayOfMonth.ToShortDateString();
        Session["ToDate"] = txtEndDate.Text;
        Session["fromDate"] = txtStartDate.Text;

    }
    protected void rdQuarter_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblQuarter.Attributes["class"] = "labelactive";
        var date = System.DateTime.Now;
        int quarterNumber = (date.Month - 1) / 3 + 1;
        DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
        DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
        txtStartDate.Text = firstDayOfQuarter.ToShortDateString();
        txtEndDate.Text = lastDayOfQuarter.ToShortDateString();
        Session["ToDate"] = txtEndDate.Text;
        Session["fromDate"] = txtStartDate.Text;
    }
    protected void rdYear_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblYear.Attributes["class"] = "labelactive";
        int year = DateTime.Now.Year;
        DateTime firstDay = new DateTime(year, 1, 1);
        DateTime lastDay = new DateTime(year, 12, 31);
        txtStartDate.Text = firstDay.ToShortDateString();
        txtEndDate.Text = lastDay.ToShortDateString();
        Session["ToDate"] = txtEndDate.Text;
        Session["fromDate"] = txtStartDate.Text;
    }

    private void RemoveActiveLabel()
    {
        lblDay.Attributes["class"] = string.Empty;
        lblWeek.Attributes["class"] = string.Empty;
        lblMonth.Attributes["class"] = string.Empty;
        lblQuarter.Attributes["class"] = string.Empty;
        lblYear.Attributes["class"] = string.Empty;
    }

    protected void RadGrid_InventoryAdjustment_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_InventoryAdjustment.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["InvAdj_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_InventoryAdjustment.MasterTableView.OwnerGrid.Columns)
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

            Session["InvAdj_Filters"] = filters;
        }
        else
        {
            Session["InvAdj_FilterExpression"] = null;
            Session["InvAdj_Filters"] = null;
        }
        #endregion  

        foreach (GridDataItem gr in RadGrid_InventoryAdjustment.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");            
            gr.Attributes["ondblclick"] = "location.href='AddInventoryAdjustment.aspx?id=" + lblID.Text + "'";
        }
    }

    protected void RadGrid_InventoryAdjustment_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        updpnl.Update();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {        
        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        foreach (GridColumn column in RadGrid_InventoryAdjustment.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_InventoryAdjustment.MasterTableView.FilterExpression = string.Empty;
        RadGrid_InventoryAdjustment.Rebind();
    }

    protected void RadGrid_InventoryAdjustment_ItemCreated(object sender, GridItemEventArgs e)
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
                dropDown.Items.Add(cboItem);
            }
            var pageZize = dropDown.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString());
            if (pageZize != null)
            {
                pageZize.Selected = true;
            }
            else
            {
                dropDown.Items.Last().Selected = true;
            }
        }
    }
   
}