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

public partial class ReceivePayment : System.Web.UI.Page
{
    #region "Variables"
    ReceivedPayment _objReceiPmt = new ReceivedPayment();
    BL_Deposit _objBL_Deposit = new BL_Deposit();

    Transaction _objTrans = new Transaction();
    BL_JournalEntry _objBLJe = new BL_JournalEntry();

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();
    BL_User objBL_User = new BL_User();

    Contracts objProp_Contracts = new Contracts();
    BL_Contracts objBL_Contracts = new BL_Contracts();
    public static int intEN = 0;

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    UpdateCustomerBalanceParam _UpdateCustomerBalance = new UpdateCustomerBalanceParam();
    DeletePaymentParam _DeletePayment = new DeletePaymentParam();
    GetAllReceivePaymentParam _GetAllReceivePayment = new GetAllReceivePaymentParam();
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

            ViewState["RadGrid_ReceivePaymentminimumRows"] = 0;
            ViewState["RadGrid_ReceivePaymentmaximumRows"] = 50;
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
            CompanyPermission();
            HighlightSideMenu("cstmMgr", "lnkReceivePayment", "cstmMgrSub");
        }
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
            //Equipment

            string ReceivePaymentPermission = ds.Rows[0]["Apply"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Apply"].ToString();

          string ReceivePaymentAdd = ReceivePaymentPermission.Length < 1 ? "Y" : ReceivePaymentPermission.Substring(0, 1);
            string ReceivePaymentEdit = ReceivePaymentPermission.Length < 2 ? "Y" : ReceivePaymentPermission.Substring(1, 1);
            string ReceivePaymentDelete = ReceivePaymentPermission.Length < 3 ? "Y" : ReceivePaymentPermission.Substring(2, 1);
            string ReceivePaymentView = ReceivePaymentPermission.Length < 4 ? "Y" : ReceivePaymentPermission.Substring(3, 1);

            if (ReceivePaymentAdd == "N")
            {
                lnkAddnew.Visible = false;
            }
            if (ReceivePaymentEdit == "N")
            {
                btnEdit.Visible = false;

            }
            if (ReceivePaymentDelete == "N")
            {
                lnkDelete.Visible = false;

            }
            if (ReceivePaymentView == "N")
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
            RadGrid_ReceivePayment.Columns[9].Visible = true;
        }
        else
        {
            RadGrid_ReceivePayment.Columns[9].Visible = false;
            Session["CmpChkDefault"] = "2";
        }
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_ReceivePayment.MasterTableView.FilterExpression != "" ||
            (RadGrid_ReceivePayment.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_ReceivePayment.MasterTableView.SortExpressions.Count > 0;
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
            string APINAME = "ReceivePaymentAPI/ReceivePaymentList_UpdateCustomerBalance";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateCustomerBalance);
        }
        else
        {
            objBL_Contracts.UpdateCustomerBalance(objProp_Contracts);
        }

    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addreceivepayment.aspx");
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_ReceivePayment.SelectedItems)
        {
            HiddenField hdnid = (HiddenField)di.FindControl("hdnid");
           // Label lblID = (Label)di.FindControl("lblId");
            Response.Redirect("addreceivepayment.aspx?id=" + hdnid.Value);
        }
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        bool _isApplied = false;
        bool Flag = false;

        foreach (GridDataItem di in RadGrid_ReceivePayment.SelectedItems)
        {
            HiddenField hdnid = (HiddenField)di.FindControl("hdnid");
            Label lblId = (Label)di.FindControl("lblId");
            Label lblStatus = (Label)di.FindControl("lblStatus");
            Label lblfDate = (Label)di.FindControl("lblfDate");
            Flag = CommonHelper.GetPeriodDetails(Convert.ToDateTime(lblfDate.Text));
            if (Flag)
            {
                if (Convert.ToInt16(lblStatus.Text).Equals(0))
                {
                    _objReceiPmt.ConnConfig = Session["config"].ToString();
                    _objReceiPmt.ID = Convert.ToInt32(hdnid.Value);

                    _DeletePayment.ConnConfig = Session["config"].ToString();
                    _DeletePayment.ID = Convert.ToInt32(hdnid.Value);

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "ReceivePaymentAPI/ReceivePaymentList_DeletePayment";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeletePayment);
                    }
                    else
                    {
                        _objBL_Deposit.DeletePayment(_objReceiPmt);
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
    protected void RadGrid_ReceivePayment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_ReceivePayment.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
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
                if (Session["ReceivePayment_FilterExpression"] != null && Convert.ToString(Session["ReceivePayment_FilterExpression"]) != "" && Session["ReceivePayment_Filters"] != null)
                {
                    RadGrid_ReceivePayment.MasterTableView.FilterExpression = Convert.ToString(Session["ReceivePayment_FilterExpression"]);
                    var filtersGet = Session["ReceivePayment_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_ReceivePayment.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["ReceivePayment_FilterExpression"] = null;
                Session["ReceivePayment_Filters"] = null;
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
        Session.Remove("ReceivedPayment");
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        ishowAllInvoice.Value = "1";
        txtFromDate.Text ="01/01/1900";
        txtToDate.Text = "01/01/2100";
        cleanFilter();
        Session["ReceivePayment_FilterExpression"] = null;
        Session["ReceivePayment_Filters"] = null;
        RadGrid_ReceivePayment.PageSize = 50;
        RadGrid_ReceivePayment.MasterTableView.PageSize = 50;
        RadGrid_ReceivePayment.Rebind();
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
        RadGrid_ReceivePayment.Rebind();    

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
        Session["ReceivePayment_FilterExpression"] = null;
        Session["ReceivePayment_Filters"] = null;
        BindPaymentGridDateFilter();
        RadGrid_ReceivePayment.Rebind();
    }

    public void cleanFilter()
    {
        foreach (GridColumn column in RadGrid_ReceivePayment.MasterTableView.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_ReceivePayment.MasterTableView.FilterExpression = string.Empty;
        //RadGrid_ReceivePayment.MasterTableView.Rebind();
        //RadGrid_ReceivePayment.Rebind();
    }

    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_ReceivePayment.Items)
        {
            HiddenField hdnid= (HiddenField)gr.FindControl("hdnid");
            Label lblID = (Label)gr.FindControl("lblId");
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkName");
            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addreceivepayment.aspx?id=" + hdnid.Value + "'";
        }
    }
    protected void RadGrid_ReceivePayment_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_ReceivePayment.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["ReceivePayment_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_ReceivePayment.MasterTableView.OwnerGrid.Columns)
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
            Session["ReceivePayment_Filters"] = filters;
        }
        else
        {
            Session["ReceivePayment_FilterExpression"] = null;
            Session["ReceivePayment_Filters"] = null;
        }
        #endregion  

        GeneralFunctions obj = new GeneralFunctions();
        if (Session["ReceivedPayment"] != null)
        {
            obj.CorrectTelerikPager(RadGrid_ReceivePayment);

            RowSelect();
        }
        if (RadGrid_ReceivePayment.MasterTableView.PageSize < 50)
        {
            RadGrid_ReceivePayment.PageSize = 50;
            RadGrid_ReceivePayment.MasterTableView.PageSize = 50;


        }

    }
    protected void RadGrid_ReceivePayment_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                //if (Convert.ToString(RadGrid_ReceivePayment.MasterTableView.FilterExpression) != "")
                //    lblRecordCount.Text = totalCount + " Record(s) found";
                //else
                //    lblRecordCount.Text = RadGrid_ReceivePayment.VirtualItemCount + " Record(s) found";
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
        String filterExpression = Convert.ToString(RadGrid_ReceivePayment.MasterTableView.FilterExpression);
        if (!IsPostBack)
        {
            if (filterExpression == "")
            {
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["ReceivePayment_FilterExpression"] != null && Convert.ToString(Session["ReceivePayment_FilterExpression"]) != "" && Session["ReceivePayment_Filters"] != null)
                    {
                        filterExpression = Convert.ToString(Session["ReceivePayment_FilterExpression"]);
                        RadGrid_ReceivePayment.MasterTableView.FilterExpression = Convert.ToString(Session["ReceivePayment_FilterExpression"]);
                        var filtersGet = Session["ReceivePayment_Filters"] as List<RetainFilter>;
                        if (filtersGet != null)
                        {
                            foreach (var _filter in filtersGet)
                            {

                                var filterCol = _filter.FilterColumn;
                                GridColumn column = RadGrid_ReceivePayment.MasterTableView.GetColumnSafe(_filter.FilterColumn);

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
                    Session["ReceivePayment_FilterExpression"] = null;
                    Session["ReceivePayment_Filters"] = null;
                }

            }
        }

        if (!IsGridPageIndexChanged)
        {
            RadGrid_ReceivePayment.CurrentPageIndex = 0;
            Session["RadGrid_ReceivePaymentCurrentPageIndex"] = 0;
            ViewState["RadGrid_ReceivePaymentminimumRows"] = 0;
            ViewState["RadGrid_ReceivePaymentmaximumRows"] = RadGrid_ReceivePayment.PageSize;
        }
        else
        {
            if (Session["RadGrid_ReceivePaymentCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_ReceivePaymentCurrentPageIndex"].ToString()) != 0
                && Convert.ToString(Request.QueryString["f"]) != "c")
            {
                RadGrid_ReceivePayment.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_ReceivePaymentCurrentPageIndex"].ToString());
                ViewState["RadGrid_ReceivePaymentminimumRows"] = RadGrid_ReceivePayment.CurrentPageIndex * RadGrid_ReceivePayment.PageSize;
                ViewState["RadGrid_ReceivePaymentmaximumRows"] = (RadGrid_ReceivePayment.CurrentPageIndex + 1) * RadGrid_ReceivePayment.PageSize;

            }
        }
        if (string.IsNullOrEmpty(filterExpression) && Session["ReceivePayment_FilterExpression"] != null)
        {
            filterExpression = Convert.ToString(Session["ReceivePayment_FilterExpression"]);
        }




        if (filterExpression != "")
        {
            Session["ReceivePayment_FilterExpression"] = filterExpression;
            foreach (GridColumn column in RadGrid_ReceivePayment.MasterTableView.OwnerGrid.Columns)
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

            Session["ReceivePayment_Filters"] = filters;
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
                _objReceiPmt.ConnConfig = Session["config"].ToString();
                _objReceiPmt.UserID = Convert.ToInt32(Session["UserID"].ToString());

                _GetAllReceivePayment.ConnConfig = Session["config"].ToString();
                _GetAllReceivePayment.UserID = Convert.ToInt32(Session["UserID"].ToString());
                if (Session["CmpChkDefault"].ToString() == "1")
                {

                    intEN = 1;
                }
                else
                {
                    intEN = 0;
                }
                _objReceiPmt.StartDate = Convert.ToDateTime(stdate);
                _objReceiPmt.EndDate = Convert.ToDateTime(enddate);

                _GetAllReceivePayment.StartDate = Convert.ToDateTime(stdate);
                _GetAllReceivePayment.EndDate = Convert.ToDateTime(enddate);

               
                 ds = _objBL_Deposit.GetAllReceivePayment(_objReceiPmt, filters, intEN);
                //DataTable dtResult = processDataFilter(ds.Tables[0]);
                DataTable dtResult = ds.Tables[0];

                Session["ReceivedPayment"] = dtResult;
                if (dtResult != null)
                {

                    RadGrid_ReceivePayment.VirtualItemCount = dtResult.Rows.Count;
                    RadGrid_ReceivePayment.DataSource = dtResult;
                    RadGrid_ReceivePayment.AllowCustomPaging = true;                  
                    lblRecordCount.Text = dtResult.Rows.Count.ToString() + " Record(s) Found.";
                }
                else
                {

                    RadGrid_ReceivePayment.VirtualItemCount = 0;
                    RadGrid_ReceivePayment.DataSource = String.Empty;
                    lblRecordCount.Text = "0 Record(s) Found.";
                    Session["ReceivedPayment"] = null;
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
                RadGrid_ReceivePayment.VirtualItemCount = 0;
                RadGrid_ReceivePayment.DataSource = String.Empty;
                lblRecordCount.Text =  "0 Record(s) found";
                Session["ReceivedPayment"] = null;
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
        RadGrid_ReceivePayment.MasterTableView.GetColumn("chkSelect").Visible = false;
        RadGrid_ReceivePayment.ExportSettings.FileName = "ReceivePayment";
        RadGrid_ReceivePayment.ExportSettings.IgnorePaging = true;
        RadGrid_ReceivePayment.ExportSettings.ExportOnlyData = true;
        RadGrid_ReceivePayment.ExportSettings.OpenInNewWindow = true;
        RadGrid_ReceivePayment.ExportSettings.HideStructureColumns = true;
        RadGrid_ReceivePayment.MasterTableView.UseAllDataFields = true;
        RadGrid_ReceivePayment.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_ReceivePayment.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_ReceivePayment_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_ReceivePayment.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_ReceivePayment.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
        Response.Redirect("addMultiReceivepayment.aspx");
    }

    protected void lnkSearchSelectDate_Click(object sender, EventArgs e)
    {
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtFromDate.Text;
        BindPaymentGridDateFilter();
        RadGrid_ReceivePayment.Rebind();
        ishowAllInvoice.Value = "0";
    }

    protected void lnkReceivePayment_Click(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_ReceivePayment.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["ReceivePayment_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_ReceivePayment.MasterTableView.OwnerGrid.Columns)
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
            Session["ReceivePayment_Filters"] = filters;
        }
        else
        {
            Session["ReceivePayment_FilterExpression"] = null;
            Session["ReceivePayment_Filters"] = null;
        }

        string urlString = "ReceivePaymentListReport.aspx?sd=" + txtFromDate.Text + "&ed=" + txtToDate.Text;

        Response.Redirect(urlString, true);
    }

    protected void RadGrid_ReceivePayment_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGrid_ReceivePaymentCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_ReceivePaymentminimumRows"] = e.NewPageIndex * RadGrid_ReceivePayment.PageSize;
            ViewState["RadGrid_ReceivePaymentmaximumRows"] = (e.NewPageIndex + 1) * RadGrid_ReceivePayment.PageSize;
        }
        catch { }
    }

    protected void RadGrid_ReceivePayment_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGrid_ReceivePaymentminimumRows"] = RadGrid_ReceivePayment.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_ReceivePaymentmaximumRows"] = (RadGrid_ReceivePayment.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }

    private DataTable processDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Session["ReceivePayment_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                if (Session["ReceivePayment_Filters"] != null)
                {
                    var filtersGet = Session["ReceivePayment_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {

                            GridColumn column = RadGrid_ReceivePayment.MasterTableView.GetColumnSafe(_filter.FilterColumn);
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
}