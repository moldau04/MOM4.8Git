using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Linq.Dynamic;
using Telerik.Web.UI.GridExcelBuilder;
using BusinessEntity.CustomersModel;
using System.Collections.Generic;
using BusinessEntity.Utility;
using MOMWebApp;
using System.Web.Script.Serialization;

public partial class ManageDeposit : System.Web.UI.Page
{
    #region "Variables"

    Dep _objDep = new Dep();
    BL_Deposit _objBL_Deposit = new BL_Deposit();
    BL_User objBL_User = new BL_User();
    private static readonly string CookieDeposit = "CkDeposit";

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetAllDepositsParam _GetAllDeposits = new GetAllDepositsParam();
    DeleteDepositParam _DeleteDeposit = new DeleteDepositParam();
    private bool IsGridPageIndexChanged = false;
    protected void Page_Init(object sender, EventArgs e)
    {
        //RadPersistenceDeposit.StorageProviderKey = CookieDeposit;
        //RadPersistenceDeposit.StorageProvider = new CookieStorageProvider(CookieDeposit);
    }
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
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }
            if (Convert.ToString(Request.QueryString["f"]) == null)
            {
                IsGridPageIndexChanged = true;
                if (Session["DepfromDate"] == null && Session["DepToDate"] == null)
                {
                    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                    hdnDepositSelectDtRange.Value = "Week";
                }
                else
                {
                    if (txtFromDate.Text == "01/01/1900" && txtToDate.Text == "01/01/2100")
                    {
                        ishowAllInvoice.Value = "1";
                    }
                    txtFromDate.Text = Session["DepfromDate"].ToString();
                    txtToDate.Text = Session["DepToDate"].ToString();
                }
            }
            else
            {
                txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                hdnDepositSelectDtRange.Value = "Week";

                Session.Remove("Deposit_FilterExpression");
                Session.Remove("Deposit_Filters");

            }
            Permission();
            //if (ishowAllInvoice.Value == "")
            //{
            //    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            //    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            //    hdnDepositSelectDtRange.Value = "Week";
            //}
            //else
            //{
            //    if (Session["DepfromDate"] == null && Session["DepToDate"] == null)
            //    {
            //        txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            //        txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            //        hdnDepositSelectDtRange.Value = "Week";
            //    }
            //    else
            //    {
            //        txtFromDate.Text = Session["DepfromDate"].ToString();
            //        txtToDate.Text = Session["DepToDate"].ToString();
            //    }
            //}

            //if (Request.Cookies[CookieDeposit] != null)
            //{
            //    RadPersistenceDeposit.LoadState();
            //    RadGrid_Deposit.Rebind();
            //    //updpnl.Update();
            //}
        }
        CompanyPermission();

        HighlightSideMenu("cstmMgr", "lnkDeposit", "cstmMgrSub");
    }
    #endregion



    #region custom functions

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
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            //ds = (DataTable)Session["userinfo"];
            ds = GetUserById();
            //Equipment

            string DepositPermission = ds.Rows[0]["Deposit"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Deposit"].ToString();

            string DepositAdd = DepositPermission.Length < 1 ? "Y" : DepositPermission.Substring(0, 1);
            string RDepositEdit = DepositPermission.Length < 2 ? "Y" : DepositPermission.Substring(1, 1);
            string DepositDelete = DepositPermission.Length < 3 ? "Y" : DepositPermission.Substring(2, 1);
            string RDepositView = DepositPermission.Length < 4 ? "Y" : DepositPermission.Substring(3, 1);

            if (DepositAdd == "N")
            {
                lnkAddnew.Visible = false;
            }
            if (RDepositEdit == "N")
            {
                btnEdit.Visible = false;

            }
            if (DepositDelete == "N")
            {
                btnDelete.Visible = false;

            }
            if (RDepositView == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
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
            // RadGrid_Deposit.Columns[6].Visible = true;
            RadGrid_Deposit.MasterTableView.GetColumn("Company").Display = true;
        }
        else
        {
            //RadGrid_Deposit.Columns[6].Visible = false;
            RadGrid_Deposit.MasterTableView.GetColumn("Company").Display = false;
            Session["CmpChkDefault"] = "2";
        }
    }
    private void BindDepositGrid()
    {
        //List<RetainFilter> filters = new List<RetainFilter>();
        //String filterExpression = Convert.ToString(RadGrid_Deposit.MasterTableView.FilterExpression);
        //if (!IsPostBack)
        //{
        //    if (filterExpression == "")
        //    {
        //        if (Convert.ToString(Request.QueryString["f"]) == null)
        //        {
        //            if (Session["Deposit_FilterExpression"] != null && Convert.ToString(Session["Deposit_FilterExpression"]) != "" && Session["Deposit_Filters"] != null)
        //            {
        //                filterExpression = Convert.ToString(Session["Deposit_FilterExpression"]);
        //                RadGrid_Deposit.MasterTableView.FilterExpression = Convert.ToString(Session["Deposit_FilterExpression"]);
        //                var filtersGet = Session["Deposit_Filters"] as List<RetainFilter>;
        //                if (filtersGet != null)
        //                {
        //                    foreach (var _filter in filtersGet)
        //                    {

        //                        var filterCol = _filter.FilterColumn;
        //                        GridColumn column = RadGrid_Deposit.MasterTableView.GetColumnSafe(_filter.FilterColumn);

        //                        if (column != null)
        //                        {
        //                            column.CurrentFilterValue = _filter.FilterValue;
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //        else
        //        {
        //            Session["Deposit_FilterExpression"] = null;
        //            Session["Deposit_Filters"] = null;
        //        }

        //    }
        //}

        if (!IsGridPageIndexChanged)
        {
            RadGrid_Deposit.CurrentPageIndex = 0;
            Session["RadGrid_DepositCurrentPageIndex"] = 0;
            ViewState["RadGrid_DepositminimumRows"] = 0;
            ViewState["RadGrid_DepositmaximumRows"] = RadGrid_Deposit.PageSize;
        }
        else
        {
            if (Session["RadGrid_DepositCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_DepositCurrentPageIndex"].ToString()) != 0
                && Convert.ToString(Request.QueryString["f"]) == null)
            {
                RadGrid_Deposit.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_DepositCurrentPageIndex"].ToString());
                ViewState["RadGrid_DepositminimumRows"] = RadGrid_Deposit.CurrentPageIndex * RadGrid_Deposit.PageSize;
                ViewState["RadGrid_DepositmaximumRows"] = (RadGrid_Deposit.CurrentPageIndex + 1) * RadGrid_Deposit.PageSize;

            }
        }

        try
        {
            if (IsValidDate())
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    txtFromDate.Text = "01/01/1900";
                    txtToDate.Text = "01/01/2100";
                }
                string stdate = txtFromDate.Text + " 00:00:00";
                string enddate = txtToDate.Text + " 23:59:59";
                DataSet _ds = new DataSet();
                _objDep.UserID = Convert.ToInt32(Session["UserID"].ToString());
                _objDep.ConnConfig = Session["config"].ToString();
                _objDep.StartDate = Convert.ToDateTime(stdate);
                _objDep.EndDate = Convert.ToDateTime(enddate);

                _GetAllDeposits.UserID = Convert.ToInt32(Session["UserID"].ToString());
                _GetAllDeposits.ConnConfig = Session["config"].ToString();
                _GetAllDeposits.StartDate = Convert.ToDateTime(stdate);
                _GetAllDeposits.EndDate = Convert.ToDateTime(enddate);
                if (Session["CmpChkDefault"].ToString() == "1")
                {
                    _objDep.EN = 1;
                    _GetAllDeposits.EN = 1;
                }
                else
                {
                    _objDep.EN = 0;
                    _GetAllDeposits.EN = 0;
                }

                List<GetAllDepositsViewModel> _lstGetAllDeposits = new List<GetAllDepositsViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "MakeDepositAPI/ManageDepositList_GetAllDeposits";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllDeposits);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetAllDeposits = serializer.Deserialize<List<GetAllDepositsViewModel>>(_APIResponse.ResponseData);
                    _ds = CommonMethods.ToDataSet<GetAllDepositsViewModel>(_lstGetAllDeposits);
                }
                else
                {
                    _ds = _objBL_Deposit.GetAllDeposits(_objDep);
                }


                DataTable dtResult = new DataTable();
                //string filterexpression = string.Empty;
                //filterexpression = RadGrid_Deposit.MasterTableView.FilterExpression;
                //if (filterexpression == "" || filterexpression == null)
                //{
                //    dtResult = _ds.Tables[0];

                //}
                //else
                //{
                //    dtResult = _ds.Tables[0].AsEnumerable().AsQueryable().Where(filterexpression).CopyToDataTable();

                //}
                dtResult = processDataFilter(_ds.Tables[0]);
                Session["Deposit"] = dtResult;
                if (dtResult != null)
                {

                    RadGrid_Deposit.VirtualItemCount = dtResult.Rows.Count;
                    RadGrid_Deposit.DataSource = dtResult;
                    RadGrid_Deposit.AllowCustomPaging = true;
                    // RadPersistenceDeposit.SaveState();
                    lblRecordCount.Text = dtResult.Rows.Count.ToString() + " Record(s) Found.";
                }
                else
                {

                    RadGrid_Deposit.VirtualItemCount = 0;
                    RadGrid_Deposit.DataSource = String.Empty;
                    lblRecordCount.Text = "0 Record(s) Found.";
                    Session["Deposit"] = null;
                }


                if (txtFromDate.Text == "01/01/1900" || txtToDate.Text == "01/01/2100")
                {
                    txtFromDate.Text = String.Empty;
                    txtToDate.Text = String.Empty;
                    Session["DepToDate"] = txtToDate.Text;
                    Session["DepfromDate"] = txtFromDate.Text;
                }
            }
            else
            {
                RadGrid_Deposit.VirtualItemCount = 0;
                RadGrid_Deposit.DataSource = String.Empty;
                lblRecordCount.Text = "0 Record(s) Found.";
                Session["Deposit"] = null;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    private bool IsValidDate()
    {
        DateTime dateValue;
        if (txtFromDate.Text == "" || txtToDate.Text == "")
        {
            txtFromDate.Text = "01/01/1900";
            txtToDate.Text = "01/01/2100";
        }
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

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Deposit.MasterTableView.FilterExpression != "" ||
            (RadGrid_Deposit.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Deposit.MasterTableView.SortExpressions.Count > 0;
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("adddeposit.aspx");
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Deposit.SelectedItems)
        {
            Label lblID = (Label)di.FindControl("lblID");
            Response.Redirect("adddeposit.aspx?id=" + lblID.Text);
        }
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            bool IsClear = false;
            bool Flag = false;
            foreach (GridDataItem di in RadGrid_Deposit.SelectedItems)
            {
                Label lblfDate = (Label)di.FindControl("lblfDate");
                Flag = CommonHelper.GetPeriodDetails(Convert.ToDateTime(lblfDate.Text));
                if (Flag)
                {
                    Label lblStatus = (Label)di.FindControl("lblStatus");
                    Label lblID = (Label)di.FindControl("lblID");
                    if (Convert.ToInt16(lblStatus.Text).Equals(0))
                    {
                        _objDep.ConnConfig = Session["config"].ToString();
                        _objDep.Ref = Convert.ToInt32(lblID.Text);
                        _objDep.isDeleteReceive = Convert.ToBoolean(Confirm_Value.Value);

                        _DeleteDeposit.ConnConfig = Session["config"].ToString();
                        _DeleteDeposit.Ref = Convert.ToInt32(lblID.Text);
                        _DeleteDeposit.isDeleteReceive = Convert.ToBoolean(Confirm_Value.Value);

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "MakeDepositAPI/ManageDepositList_DeleteDeposit";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteDeposit);
                        }
                        else
                        {
                            _objBL_Deposit.DeleteDeposit(_objDep);
                        }

                    }
                    else
                    {
                        IsClear = true;
                    }
                }
            }
            if (!Flag)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "DatePermissionAlert('delete');", true);
            }
            else
            {
                if (IsClear)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "closedMesg();", true);
                }
                else
                {
                    BindDepositGrid();
                    RadGrid_Deposit.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "DeleteSuccessMesg();", true);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void RadGrid_Deposit_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        try
        {
            RadGrid_Deposit.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
            RadGrid_Deposit.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
            if (txtFromDate.Text == "" || txtToDate.Text == "")
            {
                txtFromDate.Text = "01/01/1900";
                txtToDate.Text = "01/01/2100";
            }
            #region Set the Grid Filters
            if (!IsPostBack)
            {
                // Set filter for grid column except Department and Route
                if (Session["Deposit_FilterExpression"] != null
                    && Convert.ToString(Session["Deposit_FilterExpression"]) != ""
                    && Session["Deposit_Filters"] != null)
                {
                    RadGrid_Deposit.MasterTableView.FilterExpression = Convert.ToString(Session["Deposit_FilterExpression"]);
                    var filtersGet = Session["Deposit_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            var filterCol = _filter.FilterColumn;

                            GridColumn column = RadGrid_Deposit.MasterTableView.GetColumnSafe(_filter.FilterColumn);

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
                #region Save the Grid Filter
                String filterExpression = Convert.ToString(RadGrid_Deposit.MasterTableView.FilterExpression);
                if (filterExpression == "")
                {
                    filterExpression = Session["Deposit_FilterExpression"] != null ? Session["Deposit_FilterExpression"].ToString() : "";
                }

                if (filterExpression != "")
                {
                    Session["Deposit_FilterExpression"] = filterExpression;
                    List<RetainFilter> filters = new List<RetainFilter>();

                    foreach (GridColumn column in RadGrid_Deposit.MasterTableView.OwnerGrid.Columns)
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

                    Session["Deposit_Filters"] = filters;
                }
                else
                {
                    Session.Remove("Deposit_FilterExpression");
                    Session.Remove("Deposit_Filters");
                }
                #endregion
            }


            #endregion
        }
        catch { }


        BindDepositGrid();
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        txtFromDate.Text = "01/01/1900";
        txtToDate.Text = "01/01/2100";
        foreach (GridColumn column in RadGrid_Deposit.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_Deposit.MasterTableView.FilterExpression = string.Empty;
        Session.Remove("Deposit_FilterExpression");
        Session.Remove("Deposit_Filters");
       // BindDepositGrid();
        //txtFromDate.Text = String.Empty;
        //txtToDate.Text = String.Empty;
        //Session["DepToDate"] = txtToDate.Text;
        //Session["DepfromDate"] = txtFromDate.Text;
       
      
        RadGrid_Deposit.PageSize = 50;
        RadGrid_Deposit.MasterTableView.PageSize = 50;
        RadGrid_Deposit.Rebind();
        ishowAllInvoice.Value = "1";
    }
    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_Deposit.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkBank");
            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='adddeposit.aspx?id=" + lblID.Text + "'";
        }
    }
    protected void RadGrid_Deposit_PreRender(object sender, EventArgs e)
    {
        GeneralFunctions obj = new GeneralFunctions();
        if (Session["Deposit"] != null)
        {
            obj.CorrectTelerikPager(RadGrid_Deposit);
            RowSelect();
        }

    }
    protected void RadGrid_Deposit_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        //updpnl.Update();
    }
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Deposit.MasterTableView.GetColumn("chkSelect").Visible = false;
        RadGrid_Deposit.ExportSettings.FileName = "Deposit";
        RadGrid_Deposit.ExportSettings.IgnorePaging = true;
        RadGrid_Deposit.ExportSettings.ExportOnlyData = true;
        RadGrid_Deposit.ExportSettings.OpenInNewWindow = true;
        RadGrid_Deposit.ExportSettings.HideStructureColumns = true;
        RadGrid_Deposit.MasterTableView.UseAllDataFields = true;
        // RadGrid_Deposit.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");
        RadGrid_Deposit.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Deposit.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_Deposit_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Deposit.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Deposit.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
    protected void RadGrid_Deposit_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (Convert.ToString(RadGrid_Deposit.MasterTableView.FilterExpression) != "")
                    lblRecordCount.Text = totalCount + " Record(s) found";
                else
                    lblRecordCount.Text = RadGrid_Deposit.VirtualItemCount + " Record(s) found";
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
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        if (txtFromDate.Text==String.Empty || txtToDate.Text == String.Empty)
        {
            txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            hdnDepositSelectDtRange.Value = "Week";
            ishowAllInvoice.Value = "0";
        }
        //
        foreach (GridColumn column in RadGrid_Deposit.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        Session.Remove("Deposit_FilterExpression");
        Session.Remove("Deposit_Filters");

        RadGrid_Deposit.MasterTableView.FilterExpression = string.Empty;
        BindDepositGrid();
        RadGrid_Deposit.PageSize = 50;
        RadGrid_Deposit.MasterTableView.PageSize = 50;
        RadGrid_Deposit.Rebind();
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        //if (hdnCssActive.Value == "CssActive")
        //{
        //    Session["lblDepActive"] = "1";
        //}
        //else
        //{
        //    Session["lblDepActive"] = "2";
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        //}
        IsGridPageIndexChanged = false;
        if (txtFromDate.Text == String.Empty || txtToDate.Text == String.Empty)
        {
            txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            hdnDepositSelectDtRange.Value = "Week";
            ishowAllInvoice.Value = "0";
        }
        Session["DepToDate"] = txtToDate.Text;
        Session["DepfromDate"] = txtFromDate.Text;
        BindDepositGrid();
        RadGrid_Deposit.PageSize = 50;
        RadGrid_Deposit.MasterTableView.PageSize = 50;
        RadGrid_Deposit.Rebind();
    }

    protected void lnkDepositListSalesPerson_Click(object sender, EventArgs e)
    {
        String startDate = txtFromDate.Text;
        String endDate = txtToDate.Text;
        String url = "DepositListBySalepersonReport.aspx?page=managedeposit&sdate=" + startDate + "&edate=" + endDate;
        Response.Redirect(url);
    }

    protected void lnkDepositList_Click(object sender, EventArgs e)
    {
        String startDate = txtFromDate.Text;
        String endDate = txtToDate.Text;
        String url = "DepositListReport.aspx?page=managedeposit&sdate=" + startDate + "&edate=" + endDate;
        Response.Redirect(url);
    }

    protected void lnkSearchDate_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        Session["DepToDate"] = txtToDate.Text;
        Session["DepfromDate"] = txtFromDate.Text;

        BindDepositGrid();
        RadGrid_Deposit.PageSize = 50;
        RadGrid_Deposit.MasterTableView.PageSize = 50;
        RadGrid_Deposit.Rebind();
        ishowAllInvoice.Value = "0";
    }

    protected void RadGrid_Deposit_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void RadGrid_Deposit_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGrid_DepositminimumRows"] = RadGrid_Deposit.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_DepositmaximumRows"] = (RadGrid_Deposit.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }

    protected void RadGrid_Deposit_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGrid_DepositCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_DepositminimumRows"] = e.NewPageIndex * RadGrid_Deposit.PageSize;
            ViewState["RadGrid_DepositmaximumRows"] = (e.NewPageIndex + 1) * RadGrid_Deposit.PageSize;
        }
        catch { }
    }
    private DataTable processDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Session["Deposit_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                if (Session["Deposit_Filters"] != null)
                {
                    var filtersGet = Session["Deposit_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {

                            GridColumn column = RadGrid_Deposit.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            if (column.UniqueName == "fDate")
                            {
                                sql = sql + " And " + column.UniqueName + "='" + _filter.FilterValue+"'";
                            }
                            else
                            {
                               
                                if (column.UniqueName == "Ref" || column.UniqueName == "Amount")
                                {
                                    sql = sql + " And " + column.UniqueName + "=" + _filter.FilterValue.Replace(",", "");
                                }
                                else
                                {
                                    sql = sql + " And " + column.UniqueName + " like '%" + _filter.FilterValue + "%'";
                                }
                            }                          

                        }
                    }
                }
                if (result.Select(sql).Count() > 0)
                {
                    return result.Select(sql).CopyToDataTable();
                }
                else
                {
                    return null;
                }


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
    protected void RadGrid_Deposit_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataitem = (GridDataItem)e.Item;
            Label lblStatus = (Label)dataitem.FindControl("lblStatus");            
            CheckBox checkBox = (CheckBox)dataitem["chkSelect"].Controls[0];
            if (Convert.ToInt16(lblStatus.Text).Equals(0))
            {
                checkBox.Attributes.Add("onclick", "closedMesg1('true');");
            }
            else
            {
                checkBox.Attributes.Add("onclick", "closedMesg1('false');");
            }
            
        }
    }
}