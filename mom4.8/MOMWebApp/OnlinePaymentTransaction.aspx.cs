using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Linq.Dynamic;
using Telerik.Web.UI.GridExcelBuilder;
using BusinessEntity.Utility;
using MOMWebApp;
using System.Web.Script.Serialization;
using BusinessEntity.CustomersModel;


public partial class OnlinePaymentTransaction : System.Web.UI.Page
{
    #region "Variables"
    OnlinePayment objOnlinePayment = new OnlinePayment();
    BL_OnlinePayment objBL_OnlinePayment = new BL_OnlinePayment();
    BL_Deposit objBL_Deposit = new BL_Deposit();

    Transaction _objTrans = new Transaction();
    BL_JournalEntry _objBLJe = new BL_JournalEntry();

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();
    BL_User objBL_User = new BL_User();

    Contracts objProp_Contracts = new Contracts();
    BL_Contracts objBL_Contracts = new BL_Contracts();

    ReceivedPayment objReceivePay = new ReceivedPayment();

    public static int intEN = 0;
    public static int paymentReceiveId = 0;

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    UpdateCustomerBalanceParam _UpdateCustomerBalance = new UpdateCustomerBalanceParam();
    DeleteOnlinePaymentParam _DeleteOnlinePayment = new DeleteOnlinePaymentParam();
    GetAllOnlinePaymentParam _GetAllOnlinePayment = new GetAllOnlinePaymentParam();
    #endregion

    private bool IsGridPageIndexChanged = false;

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

            ViewState["RadGrid_OnlinePaymentTransactionminimumRows"] = 0;
            ViewState["RadGrid_OnlinePaymentTransactionmaximumRows"] = 50;

            Permission();

            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
               
                IsGridPageIndexChanged = true;
                if (Session["fromDate"] == null && Session["ToDate"] == null)
                {
                    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                }
                else
                {
                    if (Session["fromDate"].ToString()== "01/01/1900")
                    {
                        txtFromDate.Text = String.Empty;
                        txtToDate.Text = String.Empty;
                        ishowAllInvoice.Value = "1";
                    }
                    else
                    {
                        txtFromDate.Text = Session["fromDate"].ToString();
                        txtToDate.Text = Session["ToDate"].ToString();
                    }
                }
            }
            else
            {
                txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                hdnRcvPymtSelectDtRange.Value = "Week";
            }

            //**CompanyPermission();

            HighlightSideMenu("cstmMgr", "lnkOnlinePaymentTransaction", "cstmMgrSub");
        }
    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        //**HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        //**lnkUsersSmenu.Style.Add("color", "#316b9d");
        //**lnkUsersSmenu.Style.Add("font-weight", "normal");
        //**lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
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
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
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

            string OnlinePaymentPermission = ds.Rows[0]["OnlinePayment"] == DBNull.Value ? "NNNNNN" : ds.Rows[0]["OnlinePayment"].ToString();

            string OnlinePaymentAdd = OnlinePaymentPermission.Length < 1 ? "Y" : OnlinePaymentPermission.Substring(0, 1);
            string OnlinePaymentEdit = OnlinePaymentPermission.Length < 2 ? "Y" : OnlinePaymentPermission.Substring(1, 1);
            string OnlinePaymentDelete = OnlinePaymentPermission.Length < 3 ? "Y" : OnlinePaymentPermission.Substring(2, 1);
            string OnlinePaymentView = OnlinePaymentPermission.Length < 4 ? "Y" : OnlinePaymentPermission.Substring(3, 1);
            string OnlinePaymentReport = OnlinePaymentPermission.Length < 5 ? "Y" : OnlinePaymentPermission.Substring(4, 1);
            string OnlinePaymentApprove = OnlinePaymentPermission.Length < 6 ? "Y" : OnlinePaymentPermission.Substring(5, 1);

            //if (OnlinePaymentAdd == "N")
            //{
            //    lnkAdd.Visible = false;
            //}
            //if (OnlinePaymentEdit == "N")
            //{
            //    btnEdit.Visible = false;
            //}
            //if (OnlinePaymentDelete == "N")
            //{
            //    lnkDelete.Visible = false;
            //}
            if (OnlinePaymentView == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            //if (OnlinePaymentReport == "N")
            //{
            //    lnkDelete.Visible = false;
            //}
            if (OnlinePaymentApprove == "N")
            {
                lnkApprove.Visible = false;
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
            RadGrid_OnlinePaymentTransaction.Columns[9].Visible = true;
        }
        else
        {
            RadGrid_OnlinePaymentTransaction.Columns[9].Visible = false;
            Session["CmpChkDefault"] = "2";
        }
    }

    bool isGrouping = false;

    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_OnlinePaymentTransaction.MasterTableView.FilterExpression != "" ||
            (RadGrid_OnlinePaymentTransaction.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_OnlinePaymentTransaction.MasterTableView.SortExpressions.Count > 0;
    }

    private void UpdateCustomerBalance(int _loc, double _amount)
    {
        objProp_Contracts.ConnConfig = Session["config"].ToString();    // subtract previous amount from of owner and loc: balance
        objProp_Contracts.Loc = _loc;
        objProp_Contracts.Amount = (_amount * -1);

        _UpdateCustomerBalance.ConnConfig = Session["config"].ToString();    // subtract previous amount from of owner and loc: balance
        _UpdateCustomerBalance.Loc = _loc;
        _UpdateCustomerBalance.Amount = (_amount * -1);

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "OnlinePaymentTransactionAPI/OnlinePaymentTransactionList_UpdateCustomerBalance";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateCustomerBalance);
        }
        else
        {
            objBL_Contracts.UpdateCustomerBalance(objProp_Contracts);
        }
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addOnlinePaymentTransaction.aspx");
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_OnlinePaymentTransaction.SelectedItems)
        {
            HiddenField hdnId = (HiddenField)di.FindControl("hdnId");
           // Label lblID = (Label)di.FindControl("lblID");
            Response.Redirect("addOnlinePaymentTransaction.aspx?id=" + hdnId.Value);
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        bool _isApplied = false;
        bool Flag = false;
        bool bStatus;

        foreach (GridDataItem di in RadGrid_OnlinePaymentTransaction.SelectedItems)
        {
            HiddenField hdnId = (HiddenField)di.FindControl("hdnId");
            Label lblID = (Label)di.FindControl("lblID");
            //Label lblStatus = (Label)di.FindControl("lblStatus");
            HiddenField hdnStatus = (HiddenField)di.FindControl("hdnStatus");
            Boolean.TryParse(hdnStatus.Value, out bStatus);
            Label lblTransactionDate = (Label)di.FindControl("lblTransactionDate");
            Flag = CommonHelper.GetPeriodDetails(Convert.ToDateTime(lblTransactionDate.Text));
            if (Flag)
            {
                //**if (Convert.ToInt16(lblStatus.Text).Equals(0))
                if (Convert.ToInt16(bStatus).Equals(0))
                {
                    objOnlinePayment.ConnConfig = Session["config"].ToString();
                    objOnlinePayment.OnlinePaymentTransactionId = Convert.ToInt32(hdnId.Value);

                    _DeleteOnlinePayment.ConnConfig = Session["config"].ToString();
                    _DeleteOnlinePayment.ID = Convert.ToInt32(hdnId.Value);

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "OnlinePaymentTransactionAPI/OnlinePaymentTransactionList_DeletePayment";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteOnlinePayment);
                    }
                    else
                    {
                        objBL_OnlinePayment.OnlinePaymentDelete(objOnlinePayment);
                    }

                    BindPaymentGridDateFilter();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "DeleteSuccessMesg();", true);
                }
                else
                {
                    _isApplied = true;
                }
            }
        }

        if (!Flag)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "DatePermissionAlert('delete');", true);
        }

        if (_isApplied.Equals(true))
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "closedMesg();", true);
        }
    }

    protected void RadGrid_OnlinePaymentTransaction_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_OnlinePaymentTransaction.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (txtFromDate.Text == "" || txtToDate.Text == "")
        {
            txtFromDate.Text = "01/01/1900";
            txtToDate.Text = "01/01/2100";
        }
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["OnlinePaymentTransaction_FilterExpression"] != null && Convert.ToString(Session["OnlinePaymentTransaction_FilterExpression"]) != "" && Session["OnlinePaymentTransaction_Filters"] != null)
                {
                    RadGrid_OnlinePaymentTransaction.MasterTableView.FilterExpression = Convert.ToString(Session["OnlinePaymentTransaction_FilterExpression"]);
                    var filtersGet = Session["OnlinePaymentTransaction_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_OnlinePaymentTransaction.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["OnlinePaymentTransaction_FilterExpression"] = null;
                Session["OnlinePaymentTransaction_Filters"] = null;
            }
        }
        #endregion

        if (txtFromDate.Text == "" || txtToDate.Text == "")
        {
            txtFromDate.Text = "01/01/1900";
            txtToDate.Text = "01/01/2100";
        }

        BindPaymentGridDateFilter();
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
        Session.Remove("OnlinePaymentTransaction");
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        ishowAllInvoice.Value = "1";
        txtFromDate.Text ="01/01/1900";
        txtToDate.Text = "01/01/2100";
        cleanFilter();
        Session["OnlinePaymentTransaction_FilterExpression"] = null;
        Session["OnlinePaymentTransaction_Filters"] = null;
        RadGrid_OnlinePaymentTransaction.PageSize = 50;
        RadGrid_OnlinePaymentTransaction.MasterTableView.PageSize = 50;
        RadGrid_OnlinePaymentTransaction.Rebind();
        ishowAllInvoice.Value = "1";
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        if (txtFromDate.Text == String.Empty || txtToDate.Text == String.Empty)
        {
            txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            ishowAllInvoice.Value = "0";
        }      
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtFromDate.Text;
        BindPaymentGridDateFilter();
        RadGrid_OnlinePaymentTransaction.Rebind();    
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = true;
        if (ishowAllInvoice.Value == "1")
        {
            //if (Session["ToDate"] == null)
            //{
            //    txtToDate.Text = DateTime.Now.AddMonths(1).Date.ToShortDateString();
            //}
            //else
            //{
            //    txtToDate.Text = Session["ToDate"].ToString();
            //}
            //if (Session["fromDate"] == null)
            //{
            //    //txtInvDtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            //    txtFromDate.Text = DateTime.Now.AddMonths(-1).Date.ToShortDateString();
            //}
            //else
            //{
            //    txtFromDate.Text = Session["fromDate"].ToString();
            //}
            txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            hdnRcvPymtSelectDtRange.Value = "Week";
            ishowAllInvoice.Value = "0";
        }
        cleanFilter();
        Session["OnlinePaymentTransaction_FilterExpression"] = null;
        Session["OnlinePaymentTransaction_Filters"] = null;
        BindPaymentGridDateFilter();
        RadGrid_OnlinePaymentTransaction.Rebind();
    }

    public void cleanFilter()
    {
        foreach (GridColumn column in RadGrid_OnlinePaymentTransaction.MasterTableView.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_OnlinePaymentTransaction.MasterTableView.FilterExpression = string.Empty;
        //RadGrid_OnlinePaymentTransaction.MasterTableView.Rebind();
        //RadGrid_OnlinePaymentTransaction.Rebind();
    }

    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_OnlinePaymentTransaction.Items)
        {
            HiddenField hdnId= (HiddenField)gr.FindControl("hdnId");
            Label lblID = (Label)gr.FindControl("lblID");
            HyperLink lnkCustomerName = (HyperLink)gr.FindControl("lnkCustomerName");
            lnkCustomerName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addOnlinePaymentTransaction.aspx?id=" + hdnId.Value + "'";
        }
    }

    protected void RadGrid_OnlinePaymentTransaction_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_OnlinePaymentTransaction.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["OnlinePaymentTransaction_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_OnlinePaymentTransaction.MasterTableView.OwnerGrid.Columns)
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
            Session["OnlinePaymentTransaction_Filters"] = filters;
        }
        else
        {
            Session["OnlinePaymentTransaction_FilterExpression"] = null;
            Session["OnlinePaymentTransaction_Filters"] = null;
        }
        #endregion  

        GeneralFunctions obj = new GeneralFunctions();
        if (Session["OnlinePaymentTransaction"] != null)
        {
            obj.CorrectTelerikPager(RadGrid_OnlinePaymentTransaction);

            RowSelect();
        }
        if (RadGrid_OnlinePaymentTransaction.MasterTableView.PageSize < 50)
        {
            RadGrid_OnlinePaymentTransaction.PageSize = 50;
            RadGrid_OnlinePaymentTransaction.MasterTableView.PageSize = 50;
        }
    }

    protected void RadGrid_OnlinePaymentTransaction_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                //if (Convert.ToString(RadGrid_OnlinePaymentTransaction.MasterTableView.FilterExpression) != "")
                //    lblRecordCount.Text = totalCount + " Record(s) found";
                //else
                //    lblRecordCount.Text = RadGrid_OnlinePaymentTransaction.VirtualItemCount + " Record(s) found";
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

    private void BindPaymentGridDateFilter()
    {
        List<RetainFilter> filters = new List<RetainFilter>();
        String filterExpression = Convert.ToString(RadGrid_OnlinePaymentTransaction.MasterTableView.FilterExpression);
        if (!IsPostBack)
        {
            if (filterExpression == "")
            {
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["OnlinePaymentTransaction_FilterExpression"] != null && Convert.ToString(Session["OnlinePaymentTransaction_FilterExpression"]) != "" && Session["OnlinePaymentTransaction_Filters"] != null)
                    {
                        filterExpression = Convert.ToString(Session["OnlinePaymentTransaction_FilterExpression"]);
                        RadGrid_OnlinePaymentTransaction.MasterTableView.FilterExpression = Convert.ToString(Session["OnlinePaymentTransaction_FilterExpression"]);
                        var filtersGet = Session["OnlinePaymentTransaction_Filters"] as List<RetainFilter>;
                        if (filtersGet != null)
                        {
                            foreach (var _filter in filtersGet)
                            {
                                var filterCol = _filter.FilterColumn;
                                GridColumn column = RadGrid_OnlinePaymentTransaction.MasterTableView.GetColumnSafe(_filter.FilterColumn);

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
                    Session["OnlinePaymentTransaction_FilterExpression"] = null;
                    Session["OnlinePaymentTransaction_Filters"] = null;
                }
            }
        }

        if (!IsGridPageIndexChanged)
        {
            RadGrid_OnlinePaymentTransaction.CurrentPageIndex = 0;
            Session["RadGrid_OnlinePaymentTransactionCurrentPageIndex"] = 0;
            ViewState["RadGrid_OnlinePaymentTransactionminimumRows"] = 0;
            ViewState["RadGrid_OnlinePaymentTransactionmaximumRows"] = RadGrid_OnlinePaymentTransaction.PageSize;
        }
        else
        {
            if (Session["RadGrid_OnlinePaymentTransactionCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_OnlinePaymentTransactionCurrentPageIndex"].ToString()) != 0
                && Convert.ToString(Request.QueryString["f"]) != "c")
            {
                RadGrid_OnlinePaymentTransaction.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_OnlinePaymentTransactionCurrentPageIndex"].ToString());
                ViewState["RadGrid_OnlinePaymentTransactionminimumRows"] = RadGrid_OnlinePaymentTransaction.CurrentPageIndex * RadGrid_OnlinePaymentTransaction.PageSize;
                ViewState["RadGrid_OnlinePaymentTransactionmaximumRows"] = (RadGrid_OnlinePaymentTransaction.CurrentPageIndex + 1) * RadGrid_OnlinePaymentTransaction.PageSize;

            }
        }

        if (string.IsNullOrEmpty(filterExpression) && Session["OnlinePaymentTransaction_FilterExpression"] != null)
        {
            filterExpression = Convert.ToString(Session["OnlinePaymentTransaction_FilterExpression"]);
        }

        if (filterExpression != "")
        {
            Session["OnlinePaymentTransaction_FilterExpression"] = filterExpression;
            foreach (GridColumn column in RadGrid_OnlinePaymentTransaction.MasterTableView.OwnerGrid.Columns)
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

            Session["OnlinePaymentTransaction_Filters"] = filters;
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
                string stdate = "";
                string enddate = "";
                stdate = txtFromDate.Text + " 00:00:00";
                enddate = txtToDate.Text + " 23:59:59";
               
                DataTable dts = new DataTable();
                DataSet ds = new DataSet();

                objOnlinePayment.ConnConfig = Session["config"].ToString();
                objOnlinePayment.UserID = Convert.ToInt32(Session["UserID"].ToString());

                _GetAllOnlinePayment.ConnConfig = Session["config"].ToString();
                _GetAllOnlinePayment.UserID = Convert.ToInt32(Session["UserID"].ToString());
                if (Session["CmpChkDefault"].ToString() == "1")
                {
                    intEN = 1;
                }
                else
                {
                    intEN = 0;
                }
                objOnlinePayment.StartDate = Convert.ToDateTime(stdate);
                objOnlinePayment.EndDate = Convert.ToDateTime(enddate);

                _GetAllOnlinePayment.StartDate = Convert.ToDateTime(stdate);
                _GetAllOnlinePayment.EndDate = Convert.ToDateTime(enddate);

                ds = objBL_OnlinePayment.OnlinePaymentSelect(objOnlinePayment, filters, intEN);
                DataTable dtResult = ds.Tables[0];

                Session["OnlinePaymentTransaction"] = dtResult;
                if (dtResult != null)
                {
                    RadGrid_OnlinePaymentTransaction.VirtualItemCount = dtResult.Rows.Count;
                    RadGrid_OnlinePaymentTransaction.DataSource = dtResult;
                    RadGrid_OnlinePaymentTransaction.AllowCustomPaging = true;                  
                    lblRecordCount.Text = dtResult.Rows.Count.ToString() + " Record(s) Found.";
                }
                else
                {
                    RadGrid_OnlinePaymentTransaction.VirtualItemCount = 0;
                    RadGrid_OnlinePaymentTransaction.DataSource = String.Empty;
                    lblRecordCount.Text = "0 Record(s) Found.";
                    Session["OnlinePaymentTransaction"] = null;
                }

                if (txtFromDate.Text == "01/01/1900" || txtToDate.Text == "01/01/2100")
                {
                    txtFromDate.Text = String.Empty;
                    txtToDate.Text = String.Empty;
                    Session["ToDate"] = txtToDate.Text;
                    Session["fromDate"] = txtFromDate.Text;
                }     
            }
            else
            {
                RadGrid_OnlinePaymentTransaction.VirtualItemCount = 0;
                RadGrid_OnlinePaymentTransaction.DataSource = String.Empty;
                lblRecordCount.Text =  "0 Record(s) found";
                Session["OnlinePaymentTransaction"] = null;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
     }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_OnlinePaymentTransaction.MasterTableView.GetColumn("chkSelect").Visible = false;
        RadGrid_OnlinePaymentTransaction.ExportSettings.FileName = "OnlinePaymentTransaction";
        RadGrid_OnlinePaymentTransaction.ExportSettings.IgnorePaging = true;
        RadGrid_OnlinePaymentTransaction.ExportSettings.ExportOnlyData = true;
        RadGrid_OnlinePaymentTransaction.ExportSettings.OpenInNewWindow = true;
        RadGrid_OnlinePaymentTransaction.ExportSettings.HideStructureColumns = true;
        RadGrid_OnlinePaymentTransaction.MasterTableView.UseAllDataFields = true;
        RadGrid_OnlinePaymentTransaction.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_OnlinePaymentTransaction.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_OnlinePaymentTransaction_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_OnlinePaymentTransaction.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_OnlinePaymentTransaction.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Please enter valid start date and end date.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            return false;
        }
    }

    public DataTable FilterTable(DataTable table, DateTime startDate, DateTime endDate)
    {
        var filteredRows =
            from row in table.Rows.OfType<DataRow>()
            where (DateTime)row[8] >= startDate
            where (DateTime)row[8] <= endDate
            orderby (int)row[0] descending
            select row;

        var filteredTable = table.Clone();

        filteredRows.ToList().ForEach(r => filteredTable.ImportRow(r));

        return filteredTable;
    }

    protected void lnkAddMulti_Click(object sender, EventArgs e)
    {
        Response.Redirect("addMultiOnlinePaymentTransaction.aspx");
    }

    protected void lnkSearchSelectDate_Click(object sender, EventArgs e)
    {
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtFromDate.Text;
        BindPaymentGridDateFilter();
        RadGrid_OnlinePaymentTransaction.Rebind();
        ishowAllInvoice.Value = "0";
    }

    protected void lnkOnlinePaymentTransaction_Click(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_OnlinePaymentTransaction.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["OnlinePaymentTransaction_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_OnlinePaymentTransaction.MasterTableView.OwnerGrid.Columns)
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
            Session["OnlinePaymentTransaction_Filters"] = filters;
        }
        else
        {
            Session["OnlinePaymentTransaction_FilterExpression"] = null;
            Session["OnlinePaymentTransaction_Filters"] = null;
        }

        string urlString = "OnlinePaymentTransactionListReport.aspx?sd=" + txtFromDate.Text + "&ed=" + txtToDate.Text;

        Response.Redirect(urlString, true);
    }

    protected void RadGrid_OnlinePaymentTransaction_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGrid_OnlinePaymentTransactionCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_OnlinePaymentTransactionminimumRows"] = e.NewPageIndex * RadGrid_OnlinePaymentTransaction.PageSize;
            ViewState["RadGrid_OnlinePaymentTransactionmaximumRows"] = (e.NewPageIndex + 1) * RadGrid_OnlinePaymentTransaction.PageSize;
        }
        catch { }
    }

    protected void RadGrid_OnlinePaymentTransaction_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGrid_OnlinePaymentTransactionminimumRows"] = RadGrid_OnlinePaymentTransaction.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_OnlinePaymentTransactionmaximumRows"] = (RadGrid_OnlinePaymentTransaction.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }

    private DataTable processDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Session["OnlinePaymentTransaction_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                if (Session["OnlinePaymentTransaction_Filters"] != null)
                {
                    var filtersGet = Session["OnlinePaymentTransaction_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {

                            GridColumn column = RadGrid_OnlinePaymentTransaction.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            if (column.UniqueName == "PaymentReceivedDate")
                            {
                                sql = sql + " And " + column.UniqueName + "='" + _filter.FilterValue + "'";
                            }
                            else
                            {

                                if (column.UniqueName == "ID" || column.UniqueName == "Amount" || column.UniqueName == "DepID" || column.UniqueName == "BatchReceipt")
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

    protected void lnkApprove_Click(object sender, EventArgs e)
    {
        int sInvoiceId;
        string sPaymentMode;
        string sGatewayTransactionId;
        bool Flag = false;
        bool bStatus;
        bool isSuccess = false;

        foreach (GridDataItem di in RadGrid_OnlinePaymentTransaction.SelectedItems)
        {
            sInvoiceId = 0;
            sPaymentMode = "";
            HiddenField hdnId = (HiddenField)di.FindControl("hdnId");
            HiddenField hidInvoiceId = (HiddenField) di.FindControl("hidInvoiceId");
            Int32.TryParse(hidInvoiceId.Value, out sInvoiceId);
            sPaymentMode = di["PaymentMode"].Text;
            sGatewayTransactionId = di["GatewayTransactionId"].Text;
            //Label lblStatus = (Label)di.FindControl("lblStatus");
            HiddenField hdnStatus = (HiddenField)di.FindControl("hdnStatus");
            Boolean.TryParse(hdnStatus.Value, out bStatus);
            Label lblTransactionDate = (Label)di.FindControl("lblTransactionDate");

            Flag = CommonHelper.GetPeriodDetails(Convert.ToDateTime(lblTransactionDate.Text));
            if (Flag)
            {
                if (Convert.ToInt16(bStatus).Equals(1))
                {
                    //ReceivedPaymentInsert(sInvoiceId, sPaymentMode);

                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                    DataSet ds = new DataSet();
                    objProp_Contracts.ConnConfig = Session["config"].ToString();

                    objProp_Contracts.InvoiceID = sInvoiceId;

                    ds = objBL_Contracts.GetInvoiceByInvoiceID(objProp_Contracts);

                    int CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["Owner"].ToString());
                    objReceivePay.Rol = CustomerId;

                    decimal amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"]);
                    string LocationId = ds.Tables[0].Rows[0]["Loc"].ToString();
                    //string InvoiceId = Convert.ToString(ds.Tables[0].Rows[0]["ref"]);

                    objReceivePay.Loc = Convert.ToInt32(LocationId);

                    CreateDatatable();
                    DataTable dt = (DataTable)ViewState["ReceivPay"];
                    DataTable dtReceive = dt.Clone();
                    DataRow dr = dtReceive.NewRow();
                    dr["InvoiceID"] = sInvoiceId;
                    dr["Loc"] = LocationId;
                    dr["Type"] = 0; // 0 = Invoice
                    dr["PayAmount"] = amount;
                    dr["Status"] = 1;
                    dr["IsCredit"] = 1;
                    dr["RefTranID"] = Convert.ToString(ds.Tables[0].Rows[0]["TransID"]);
                    dtReceive.Rows.Add(dr);

                    objReceivePay.DtPay = dtReceive;

                    if (sPaymentMode.Contains("Bank"))
                    {
                        objReceivePay.PaymentMethod = 3; // Bank data
                    }
                    else
                    {
                        objReceivePay.PaymentMethod = 4; // Card data
                    }

                    DateTime receivePay = DateTime.Now;

                    //objReceivePay.ID = Convert.ToInt32(Request.QueryString["InvoiceId"]);
                    objReceivePay.ID = sInvoiceId;

                    objReceivePay.Status = 1; // Deposited
                    objReceivePay.AmountDue = 0;
                    objReceivePay.PaymentReceivedDate = receivePay;
                    objReceivePay.CheckNumber = sGatewayTransactionId;
                    objReceivePay.fDesc = "Online payment";
                    objReceivePay.MOMUSer = "";
                    objReceivePay.Amount = (double)amount;

                    objReceivePay.ConnConfig = Session["config"].ToString();

                    paymentReceiveId = objBL_Deposit.AddReceivePayment(objReceivePay);


                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                    if (paymentReceiveId > 0)
                    {
                        isSuccess = true;

                        // update Online payment Approve flag
                        objOnlinePayment.ConnConfig = Session["config"].ToString();
                        objOnlinePayment.OnlinePaymentTransactionId = Convert.ToInt32(hdnId.Value);
                        objBL_OnlinePayment.OnlinePaymentApprove(objOnlinePayment);
                    }
                }
                else
                {
                    isSuccess = false;
                }
            }

            if (!Flag)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "DatePermissionAlert('delete');", true);
            }

            if (isSuccess.Equals(false))
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "ApproveFailedMsg();", true);
                //string message = "alert('This Online payment has been failed and cannot be approved.')";
                //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
            else if (isSuccess.Equals(true))
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "ApproveSuccessMsg();", true);
            }

        }
    }


    private void ReceivedPaymentInsert(int sInvoiceId, string sPaymentMode)
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();

        objProp_Contracts.InvoiceID = sInvoiceId;

        ds = objBL_Contracts.GetInvoiceByInvoiceID(objProp_Contracts);

        int CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["Owner"].ToString());
        objReceivePay.Rol = CustomerId;

        decimal amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"]);
        string LocationId = ds.Tables[0].Rows[0]["Loc"].ToString();
        //string InvoiceId = Convert.ToString(ds.Tables[0].Rows[0]["ref"]);

        objReceivePay.Loc = Convert.ToInt32(LocationId);

        CreateDatatable();
        DataTable dt = (DataTable) ViewState["ReceivPay"];
        DataTable dtReceive = dt.Clone();
        DataRow dr = dtReceive.NewRow();
        dr["InvoiceID"] = sInvoiceId;
        dr["Loc"] = LocationId;
        dr["Type"] = 0; // 0 = Invoice
        dr["PayAmount"] = amount;
        dr["Status"] = 1;
        dr["IsCredit"] = 1;
        dr["RefTranID"] = Convert.ToString(ds.Tables[0].Rows[0]["TransID"]);
        dtReceive.Rows.Add(dr);

        objReceivePay.DtPay = dtReceive;

        if (sPaymentMode.Contains("Bank"))
        {
            objReceivePay.PaymentMethod = 3; // Bank data
        }
        else
        {
            objReceivePay.PaymentMethod = 4; // Card data
        }

        DateTime receivePay = DateTime.Now;

        //objReceivePay.ID = Convert.ToInt32(Request.QueryString["InvoiceId"]);
        objReceivePay.ID = sInvoiceId;

        objReceivePay.Status = 1; // Deposited
        objReceivePay.AmountDue = 0;
        objReceivePay.PaymentReceivedDate = receivePay;
        objReceivePay.CheckNumber = "";
        objReceivePay.fDesc = "online payment";
        objReceivePay.MOMUSer = "";
        objReceivePay.Amount = (double) amount;

        objReceivePay.ConnConfig = Session["config"].ToString();

        paymentReceiveId = objBL_Deposit.AddReceivePayment(objReceivePay);

    }


    private void CreateDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("InvoiceID", typeof(int));
        dt.Columns.Add("Status", typeof(Int16));
        dt.Columns.Add("PayAmount", typeof(double));
        dt.Columns.Add("IsCredit", typeof(Int16));
        dt.Columns.Add("Type", typeof(Int16));
        dt.Columns.Add("Loc", typeof(Int32));
        dt.Columns.Add("RefTranID", typeof(Int32));

        DataRow dr = dt.NewRow();
        dr["InvoiceID"] = DBNull.Value;
        dr["Status"] = DBNull.Value;
        dr["PayAmount"] = 0;
        dr["IsCredit"] = DBNull.Value;
        dr["Loc"] = DBNull.Value;
        dr["Type"] = DBNull.Value;
        dr["RefTranID"] = DBNull.Value;
        dt.Rows.Add(dr);
        ViewState["ReceivPay"] = dt;
    }




}