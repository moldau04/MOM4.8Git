using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using BusinessEntity;
using BusinessLayer;
using Telerik.Web.UI;
using System.Linq.Dynamic;
using Telerik.Web.UI.GridExcelBuilder;
using BusinessEntity.APModels;
using BusinessEntity.Utility;
using MOMWebApp;
using BusinessEntity.Payroll;
using BusinessEntity.CommonModel;
using System.Configuration;

public partial class ManageBills : System.Web.UI.Page
{
    #region "Variables"
    PJ _objPJ = new PJ();
    BL_Bills _objBLBills = new BL_Bills();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    BL_User _objBLUser = new BL_User();
    public bool check = false;

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    APIIntegrationModel _objAPIIntegration = new APIIntegrationModel();
    GetAllPJDetailsParam _getAllPJDetails = new GetAllPJDetailsParam();
    GetAllPJRecurrDetailsParam _getAllPJRecurrDetails = new GetAllPJRecurrDetailsParam();
    ProcessRecurBillParam _processRecurBill = new ProcessRecurBillParam();
    DeleteAPBillRecurrParam _deleteAPBillRecurr = new DeleteAPBillRecurrParam();
    GetProcessRecurrCountParam _getProcessRecurrCount = new GetProcessRecurrCountParam();
    DeleteAPBillParam _deleteAPBill = new DeleteAPBillParam();
    UpdateAPDatesParam _updateAPDates = new UpdateAPDatesParam();
    GetPJAcctDetailByIDParam _getPJAcctDetailByID = new GetPJAcctDetailByIDParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetStockReportsParam _getStockReports = new GetStockReportsParam();
    GetUserByIdParam _getUserById = new GetUserByIdParam();
    getCustomFieldsParam _getCustomFields = new getCustomFieldsParam();
    getVendorTypeParam _getVendorType = new getVendorTypeParam();
    #endregion
    #region Events

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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

                BindSearchFilters();
                FillVendorType();
                Permission();
                UserPermission();

                //for retaining filters
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["ddlsearch"] != null)
                    {
                        ddlSearch.SelectedValue = Session["ddlSearch"].ToString();
                        if (ddlSearch.SelectedValue == "VendorName" || ddlSearch.SelectedValue == "Custom1" || ddlSearch.SelectedValue == "Custom2")
                        {
                            txtVendorSearch.Visible = true;
                            txtSearch.Visible = false;
                            txtVendorSearch.Text = Session["txtVendorSearch"].ToString();
                        }
                        else if (ddlSearch.SelectedValue == "Status")
                        {
                            String searchValue = Convert.ToString(Session["ddlStatus"]);
                            if (Session["ddlStatus"] != null)
                            {
                                ddlSearch_SelectedIndexChanged(sender, e);
                                ddlStatus.SelectedValue = searchValue;
                            }
                        }
                        else if (ddlSearch.SelectedValue == "Code")
                        {
                            String searchValue = Convert.ToString(Session["ddlBOMType"]);
                            if (Session["ddlBOMType"] != null)
                            {
                                ddlSearch_SelectedIndexChanged(sender, e);
                                ddlBOMType.SelectedValue = searchValue;
                            }
                        }
                        else if (ddlSearch.SelectedValue == "VendorType")
                        {
                            String searchValue = Convert.ToString(Session["ddlType"]);
                            if (Session["ddlType"] != null)
                            {
                                ddlSearch_SelectedIndexChanged(sender, e);
                                ddlType.SelectedValue = searchValue;
                            }
                        }
                        else
                        {
                            txtVendorSearch.Visible = false;
                            txtSearch.Visible = true;
                            txtSearch.Text = Session["txtSearch"].ToString();
                        }
                    }

                    if (Session["BillFilterDate"] != null)
                    {
                        ddlFilterDate.SelectedValue = Session["BillFilterDate"].ToString();                        
                    }
                    else
                    {                        
                        Session["BillFilterDate"] = ddlFilterDate.SelectedValue;                        
                    }

                    if (Session["BillfromDate"] != null && Session["BillToDate"] != null)
                    {
                        txtFromDate.Text = Session["BillfromDate"].ToString();
                        txtToDate.Text = Session["BillToDate"].ToString();
                    }
                    else
                    {
                        txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                        txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                        Session["BillfromDate"] = txtFromDate.Text;
                        Session["BillToDate"] = txtToDate.Text;
                    }

                    if (Session["InClosed"] != null)
                    {
                        if (Session["InClosed"].ToString() == "True")
                        {
                            check = true;
                            lnkChk.Checked = true;
                        }
                        else
                        {
                            check = false;
                            lnkChk.Checked = false;
                        }
                    }
                    else
                    {
                        check = false;
                        lnkChk.Checked = false;
                    }

                    if (Session["Journal_Type"].ToString() == "1")
                    {
                        rdobill.Checked = true;
                        rdoRecurring.Checked = false;
                        lnkProcess.Visible = false;
                    }
                    else
                    {
                        rdobill.Checked = false;
                        rdoRecurring.Checked = true;
                        rdoRecurring_CheckedChanged(sender, e);
                    }
                }
                else
                {
                    Session["ddlsearch"] = null;
                    Session["txtSearch"] = null;
                    Session["ddlStatus"] = null;
                    Session["ddlBOMType"] = null;
                    Session["ddlType"] = null;
                    Session["txtVendorSearch"] = null;
                    Session["BillfromDate"] = null;
                    Session["BillToDate"] = null;
                    Session["BillFilterDate"] = null;
                    Session["Journal_Type"] = null;
                    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                    Session["BillfromDate"] = txtFromDate.Text;
                    Session["BillToDate"] = txtToDate.Text;
                    hdnBillsSelectDtRange.Value = "Week";
                    lnkProcess.Visible = false;
                    rdobill.Checked = true;

                    Session["InClosed"] = "False";
                    check = false;
                    lnkChk.Checked = false;
                }

                if (Session["txtRef"] != null)
                {
                    txtRef.Text = Session["txtRef"].ToString();
                }
            }

            ConvertToJSON();
            CompanyPermission();
            HighlightSideMenu("acctPayable", "lnkAddBill", "acctPayableSub");
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
            foreach (GridDataItem gr in RadGrid_Bills.SelectedItems)
            {
                //CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                _objPJ.ConnConfig = Session["config"].ToString();
                _objPJ.ID = Convert.ToInt32(hdnID.Value);

                //API
                _processRecurBill.ConnConfig = Session["config"].ToString();
                _processRecurBill.ID = Convert.ToInt32(hdnID.Value);
                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "BillAPI/BillsList_ProcessRecurBill";
                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _processRecurBill);
                    RecurCount = Convert.ToInt32(_APIResponse.ResponseData);

                }
                else
                {
                    RecurCount = _objBLBills.ProcessRecurBill(_objPJ);
                }
            }

            Button mpbtnNotifyRecur = this.Master.FindControl("btnNotifyRecur") as Button;
            if (mpbtnNotifyRecur != null)
            {
                mpbtnNotifyRecur.Text = RecurCount.ToString();
            }
            //BindRecurringGrid();
            RadGrid_Bills.Rebind();
            ScriptManager.RegisterStartupScript(this, GetType(), "keySuccUp", "noty({text: 'Successfully entry processed!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_Bills.Columns[8].Visible = true;
        }
        else
        {
            RadGrid_Bills.Columns[8].Visible = false;
        }
    }
    #endregion

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addbills.aspx");
    }
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem di in RadGrid_Bills.SelectedItems)
            {
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                HiddenField hdnID = (HiddenField)di.FindControl("hdnID");

                if (chkSelect.Checked == true)
                {
                    
                    if (rdobill.Checked == true)
                    {
                        Response.Redirect("addbills.aspx?id=" + hdnID.Value);
                    }
                    else if (rdoRecurring.Checked == true)
                    {                        
                        Response.Redirect("addbills.aspx?id=" + hdnID.Value + "&r=1");
                    }
                }
            }
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
        Session.Remove("Bills");
        
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Bills.MasterTableView.FilterExpression != "" ||
            (RadGrid_Bills.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Bills.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Bills_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Bills.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Bills_FilterExpression"] != null && Convert.ToString(Session["Bills_FilterExpression"]) != "" && Session["Bills_Filters"] != null)
                {
                    RadGrid_Bills.MasterTableView.FilterExpression = Convert.ToString(Session["Bills_FilterExpression"]);
                    var filtersGet = Session["Bills_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_Bills.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["Bills_FilterExpression"] = null;
                Session["Bills_Filters"] = null;
            }
        }
        #endregion
        //BindBillGrid();

        if (rdobill.Checked)
        {
            Session["Journal_Type"] = "1";
            BindBillGrid();
        }
        else
        {
            Session["Journal_Type"] = "2";
            BindRecurringGrid();
            
        }

        

    }
    protected void RadGrid_Bills_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        updpnl.Update();
    }
    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_Bills.Items)
        {
            Label lblID = (Label)gr.FindControl("lblIndex");
            HyperLink lnkName = (HyperLink)gr.FindControl("lblRef");
            //lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addbills.aspx?id=" + lblID.Text + "'";

            if (rdobill.Checked == true)
            {
                //lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addbills.aspx?id=" + lblID.Text + "'";
                lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addbills.aspx?id=" + lblID.Text + "&frm=MNG2'";
            }
            else if (rdoRecurring.Checked == true)
            {
                
                lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addbills.aspx?id=" + lblID.Text + "&r=1'";
            }
        }
    }
    protected void RadGrid_Bills_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_Bills.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Bills_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Bills.MasterTableView.OwnerGrid.Columns)
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

            Session["Bills_Filters"] = filters;
        }
        else
        {
            Session["Bills_FilterExpression"] = null;
            Session["Bills_Filters"] = null;
        }
        #endregion
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Bills);
        RowSelect();
    }
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Bills.MasterTableView.GetColumn("chkSelect").Visible = false;
        RadGrid_Bills.ExportSettings.FileName = "Bills";
        RadGrid_Bills.ExportSettings.IgnorePaging = true;
        RadGrid_Bills.ExportSettings.ExportOnlyData = true;
        RadGrid_Bills.ExportSettings.OpenInNewWindow = true;
        RadGrid_Bills.ExportSettings.HideStructureColumns = true;
        RadGrid_Bills.MasterTableView.UseAllDataFields = true;
        RadGrid_Bills.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Bills.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_Bills_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 5;
        else
            currentItem = 6;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Bills.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Bills.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
    protected void RadGrid_Bills_ItemCreated(object sender, GridItemEventArgs e)
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
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        bool _isClosed = false;
        bool Flag = false;
        foreach (GridDataItem di in RadGrid_Bills.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            if (chkSelect.Checked.Equals(true))
            {
                Label lblStatus = (Label)di.FindControl("lblStatus");
                Label lblIndex = (Label)di.FindControl("lblIndex");
                Label lblPostDt = (Label)di.FindControl("lblPostDate");
                Label lblIDate = (Label)di.FindControl("lblIDate");

                Flag = CommonHelper.GetPeriodDetails(Convert.ToDateTime(lblPostDt.Text));
                if (Flag)
                {
                    Flag = CommonHelper.GetPeriodDetails(Convert.ToDateTime(lblIDate.Text));
                }
                if (Flag)
                {

                    if (rdoRecurring.Checked == true)
                    {
                        //_objBLRecurr.DeleteGLARecur(_objJe);
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Recurring JE deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        //Button mpbtnNotifyRecur = this.Master.FindControl("btnNotifyRecur") as Button;
                        //if (mpbtnNotifyRecur != null)
                        //{
                        //    DataSet _dsRecurrCount = new DataSet();
                        //    _objJe.ConnConfig = Session["config"].ToString();
                        //    _dsRecurrCount = _objBLRecurr.GetProcessRecurrCount(_objJe);
                        //    if (_dsRecurrCount != null)
                        //    {
                        //        int _recurCount = Convert.ToInt32(_dsRecurrCount.Tables[0].Rows[0]["CountRecur"]);
                        //        mpbtnNotifyRecur.Text = _recurCount.ToString();
                        //    }
                        //}
                        //break;

                        _objPJ.ConnConfig = Session["config"].ToString();
                        _objPJ.ID = Convert.ToInt32(lblIndex.Text);

                        //API
                        _deleteAPBillRecurr.ConnConfig = Session["config"].ToString();
                        _deleteAPBillRecurr.ID = Convert.ToInt32(lblIndex.Text);

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "BillAPI/BillsList_DeleteAPBillRecurr";
                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _deleteAPBillRecurr);                         
                        }
                        else
                        {
                            _objBLBills.DeleteAPBillRecurr(_objPJ);
                        }
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Recurring Bill deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Recurring JE deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        Button mpbtnNotifyRecur = this.Master.FindControl("btnNotifyRecur") as Button;
                        if (mpbtnNotifyRecur != null)
                        {
                            DataSet _dsRecurrCount = new DataSet();
                            _objPJ.ConnConfig = Session["config"].ToString();

                            //API
                            _getProcessRecurrCount.ConnConfig = Session["config"].ToString();

                            List<CDViewModel> _lstProcessRecurrCount = new List<CDViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "BillAPI/BillsList_GetProcessRecurrCount";
                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getProcessRecurrCount);
                                _lstProcessRecurrCount = (new JavaScriptSerializer()).Deserialize<List<CDViewModel>>(_APIResponse.ResponseData);
                                _dsRecurrCount = CommonMethods.ToDataSet<CDViewModel>(_lstProcessRecurrCount);                               
                            }
                            else
                            {

                                _dsRecurrCount = _objBLBills.GetProcessRecurrCount(_objPJ);
                            }
                            if (_dsRecurrCount != null)
                            {
                                int _recurCount = Convert.ToInt32(_dsRecurrCount.Tables[0].Rows[0]["CountRecur"]);
                                mpbtnNotifyRecur.Text = _recurCount.ToString();
                            }
                        }
                        break;
                    }
                    else if (rdobill.Checked == true)
                    {

                        if (Convert.ToInt16(lblStatus.Text).Equals(0))
                        {
                            _objPJ.ConnConfig = Session["config"].ToString();
                            _objPJ.ID = Convert.ToInt32(lblIndex.Text);

                            //API
                            _deleteAPBill.ConnConfig = Session["config"].ToString();
                            _deleteAPBill.ID = Convert.ToInt32(lblIndex.Text);

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "BillAPI/BillsList_DeleteAPBill";
                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _deleteAPBill);
                            }
                            else
                            {
                                _objBLBills.DeleteAPBill(_objPJ);
                            }
                            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Bill Code deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        }
                        else
                        {
                            _isClosed = true;
                        }
                    }
                }

            }
        }
        if (!Flag)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "DatePermissionAlert('delete');", true);
        }
        else
        {
            if (_isClosed.Equals(true))
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "closedMesg();", true);
            }
            else
            {
                //BindBillGrid();
                if (rdobill.Checked)
                {
                    Session["Journal_Type"] = "1";
                    BindBillGrid();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "DeleteBillMsg();", true);
                    
                }
                else
                {
                    Session["Journal_Type"] = "2";
                    BindRecurringGrid();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "DeleteRecrrBillMsg();", true);
                    

                }

                RadGrid_Bills.Rebind();
            }
        }
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {

        if (hdnCssActive.Value == "CssActive")
        {
            Session["lblBillActive"] = "1";
        }
        else
        {
            Session["lblBillActive"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }
        Session["BillToDate"] = txtToDate.Text;
        Session["BillfromDate"] = txtFromDate.Text;
        //BindBillGrid();
        //RadGrid_Bills.Rebind();

        if (rdobill.Checked)
        {
            Session["Journal_Type"] = "1";
            BindBillGrid();           
        }
        else
        {
            Session["Journal_Type"] = "2";
            BindRecurringGrid();
        }


    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;        
        ddlSearch.SelectedIndex = 0;
        //txtFromDate.Text = "1900-01-01";
        //txtFromDate.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
        //txtToDate.Text = DateTime.Now.ToShortDateString();
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        Session["BillfromDate"] = txtFromDate.Text;
        Session["BillToDate"] = txtToDate.Text;
        
        //BindBillGrid();
        //BindBillGridShowAll();
        Session["InClosed"] = "True";
        lnkChk.Checked = true;
        if (rdobill.Checked)
        {
            Session["Journal_Type"] = "1";
            BindBillGrid();
        }
        else
        {
            Session["Journal_Type"] = "2";
            BindRecurringGrid();

        }


        RadGrid_Bills.Rebind();
    }
    #endregion
    private void BindBillGridShowAll()
    {
        Session["txtSearch"] = txtSearch.Text;
        Session["txtRef"] = txtRef.Text;
        Session["txtVendorSearch"] = txtVendorSearch.Text;
        try
        {
            DataSet _dsPJ = new DataSet();
            _objPJ.ConnConfig = Session["config"].ToString();
            _objPJ.UserID = Convert.ToInt32(Session["UserID"].ToString());

            //API
            _getAllPJDetails.ConnConfig = Session["config"].ToString();
            _getAllPJDetails.UserID = Convert.ToInt32(Session["UserID"].ToString());

            //if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
            //{
            //    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            //    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            //}

            //string stdate = txtFromDate.Text + " 00:00:00";
            //string enddate = txtToDate.Text + " 23:59:59";
            string stdate = "1900-01-01 00:00:00";
            string enddate = DateTime.Now.ToShortDateString() + " 23:59:59";
            _objPJ.StartDate = Convert.ToDateTime(stdate);
            _objPJ.EndDate = Convert.ToDateTime(enddate);

            _getAllPJDetails.StartDate = Convert.ToDateTime(stdate);
            _getAllPJDetails.EndDate = Convert.ToDateTime(enddate);

            if (ddlSearch.SelectedValue == "PO")
            {
                _objPJ.PO = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJDetails.PO = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
            }
            if (ddlSearch.SelectedValue == "Projectnumber")
            {
                _objPJ.ProjectNumber = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJDetails.ProjectNumber = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
            }

            if (ddlSearch.SelectedValue == "Vendor")
            {
                _objPJ.Vendor = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJDetails.Vendor = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

            }
            if (ddlSearch.SelectedValue == "Amount")
            {
                _objPJ.Amount = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJDetails.Amount = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

            }
            if (ddlSearch.SelectedValue == "AmountDue")
            {
                _objPJ.Balance = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJDetails.Balance = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

            }
            if (ddlSearch.SelectedValue == "VendorName")
            {
                _objPJ.vendorName = txtVendorSearch.Text;
                _getAllPJDetails.vendorName = txtVendorSearch.Text;
            }
            if (ddlSearch.SelectedValue == "Custom1")
            {
                _objPJ.Custom1 = txtVendorSearch.Text;
                _getAllPJDetails.Custom1 = txtVendorSearch.Text;
            }
            if (ddlSearch.SelectedValue == "Custom2")
            {
                _objPJ.Custom2 = txtVendorSearch.Text;
                _getAllPJDetails.Custom2 = txtVendorSearch.Text;
            }
            //if (ddlSearch.SelectedValue == "Status")
            //{
            //    _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue); //Convert.ToInt16(txtSearch.Text);
            //    _getAllPJDetails.Status = Convert.ToInt16(ddlStatus.SelectedValue); //Convert.ToInt16(txtSearch.Text);
            //    Session["ddlStatus"] = ddlStatus.SelectedValue;
            //}
            //else
            //{
            //    _objPJ.Status = 99;
            //    _getAllPJDetails.Status = 99;
            //}
            if (ddlSearch.SelectedValue == "Status")
            {
                var Status = string.Empty;
                if (ddlStatus.CheckedItems.Count > 0)
                {
                    foreach (var item in ddlStatus.CheckedItems)
                    {
                        Status += item.Value + ",";
                    }

                    Status = Status.TrimEnd(',');

                    _objPJ.SearchwithmultipleStatus = "(" + Status + ")";
                    _getAllPJDetails.SearchwithmultipleStatus = "(" + Status + ")";
                    Session["ddlStatus"] = "(" + Status + ")";

                }
            }
            else
            {
                _objPJ.SearchwithmultipleStatus = "99";

                _getAllPJDetails.SearchwithmultipleStatus = "99";
            }
            if (ddlSearch.SelectedValue == "Code")
            {
                _objPJ.Terms = Convert.ToInt16(ddlBOMType.SelectedValue); //Convert.ToInt16(txtSearch.Text);
                _getAllPJDetails.Terms = Convert.ToInt16(ddlBOMType.SelectedValue); //Convert.ToInt16(txtSearch.Text);
                Session["ddlBOMType"] = ddlBOMType.SelectedValue;
            }
            else
            {
                _objPJ.Terms = 99;
                _getAllPJDetails.Terms = 99;
            }
            if (ddlSearch.SelectedValue == "VendorType")
            {
                _objPJ.VendorType = Convert.ToString(ddlType.SelectedValue); 
                _getAllPJDetails.VendorType = Convert.ToString(ddlType.SelectedValue); 
                Session["ddlType"] = ddlType.SelectedValue;
            }
            else
            {
                _objPJ.VendorType = "";
                _getAllPJDetails.VendorType = "";
            }
            _objPJ.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);
            _getAllPJDetails.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);

            if (_objPJ.SearchValue.Equals(2))
            {
                if (string.IsNullOrEmpty(txtToDate.Text))
                {
                    _objPJ.SearchDate = DateTime.Now;

                    txtToDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    _objPJ.SearchDate = Convert.ToDateTime(txtToDate.Text);
                }
            }

            if (_getAllPJDetails.SearchValue.Equals(2))
            {
                if (string.IsNullOrEmpty(txtToDate.Text))
                {
                    _getAllPJDetails.SearchDate = DateTime.Now;

                    txtToDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    _getAllPJDetails.SearchDate = Convert.ToDateTime(txtToDate.Text);
                }
            }


            if (Session["CmpChkDefault"].ToString() == "1")
            {
                _objPJ.EN = 1;
                _getAllPJDetails.EN = 1;
            }
            else
            {
                _objPJ.EN = 0;
                _getAllPJDetails.EN = 0;
            }
            _objPJ.Ref = txtRef.Text.Trim();
            _getAllPJDetails.Ref = txtRef.Text.Trim();
            List<GetAllPJDetailsViewModel> _lstGetAllPJDetails = new List<GetAllPJDetailsViewModel>();
            // _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            //{
            //    string APINAME = "BillAPI/BillsList_GetAllPJDetails";
            //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllPJDetails);
            //    _lstGetAllPJDetails = (new JavaScriptSerializer()).Deserialize<List<GetAllPJDetailsViewModel>>(_APIResponse.ResponseData);
            //    _dsPJ = CommonMethods.ToDataSet<GetAllPJDetailsViewModel>(_lstGetAllPJDetails);

            //}
            //else
            //{
            _dsPJ = _objBLBills.GetAllPJDetails(_objPJ);
            //}

            // Status open/closed filter 
            DataTable filterdt = new DataTable();
            DataSet FilteredDs = new DataSet();
            check = Convert.ToBoolean(Session["InClosed"]) ;
            if (check)
            {
                lnkChk.Checked = true;
                FilteredDs = _dsPJ.Copy();
            }
            else
            {
                if (_dsPJ.Tables[0].Rows.Count > 0)
                {
                    //DataRow[] dr = _dsPJ.Tables[0].Select("StatusName='Open' OR StatusName='Partial'");
                    //if (dr.Length > 0)
                    //{
                    //    filterdt = dr.CopyToDataTable();
                    //    FilteredDs.Tables.Add(filterdt);
                    //}
                    //else
                    //{
                    FilteredDs = _dsPJ.Copy();
                    //}
                }
                else
                {
                    FilteredDs = _dsPJ.Copy();

                }
            }
            RadGrid_Bills.VirtualItemCount = FilteredDs.Tables[0].Rows.Count;
            RadGrid_Bills.DataSource = FilteredDs;


            BusinessEntity.User objProp_User = new BusinessEntity.User();
            DataSet ds = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();

            //API
            _getConnectionConfig.ConnConfig = Session["config"].ToString();

            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();
            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "BillAPI/BillsList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                ds = _objBLUser.getControl(objProp_User);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsUseTaxAPBills"].ToString()) == false)
                {
                    RadGrid_Bills.Columns[14].Visible = false;
                }
                else
                {
                    RadGrid_Bills.Columns[14].Visible = true;
                }


            }




            string filterexpression = string.Empty;
            filterexpression = RadGrid_Bills.MasterTableView.FilterExpression;
            if (filterexpression == "" || filterexpression == null)
            {
                Session["Bills"] = FilteredDs.Tables[0];

            }
            else
            {
                if (FilteredDs.Tables[0].Rows.Count > 0)
                {
                    //Session["Bills"] = FilteredDs.Tables[0].AsEnumerable().AsQueryable().Where(filterexpression).CopyToDataTable();
                    Session["Bills"] = FilteredDs.Tables[0];
                }
                else
                {
                    Session["Bills"] = FilteredDs.Tables[0];
                }

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #region Custom Functions
    private void BindBillGrid()
    {
        


        Session["txtSearch"] = txtSearch.Text;
        Session["txtRef"] = txtRef.Text;
        Session["txtVendorSearch"] = txtVendorSearch.Text;
        try
        {

            txtFromDate.Text = Session["BillfromDate"].ToString();
            txtToDate.Text = Session["BillToDate"].ToString();
            if (txtToDate.Text == "")
            {
                txtFromDate.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
                txtToDate.Text = DateTime.Now.AddYears(100).ToShortDateString();
            }

            DataSet _dsPJ = new DataSet();
            _objPJ.ConnConfig = Session["config"].ToString();
            _objPJ.UserID = Convert.ToInt32(Session["UserID"].ToString());

            //API
            _getAllPJDetails.ConnConfig = Session["config"].ToString();
            _getAllPJDetails.UserID = Convert.ToInt32(Session["UserID"].ToString());

            if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
            {
                txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            string stdate = txtFromDate.Text + " 00:00:00";
            string enddate = txtToDate.Text + " 23:59:59";
            _objPJ.StartDate =  Convert.ToDateTime(stdate);
            _objPJ.EndDate =  Convert.ToDateTime(enddate);

            _getAllPJDetails.StartDate = Convert.ToDateTime(stdate);
            _getAllPJDetails.EndDate = Convert.ToDateTime(enddate);

            if (ddlSearch.SelectedValue == "PO")
            {
                _objPJ.PO = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt32(txtSearch.Text);
                _getAllPJDetails.PO = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt32(txtSearch.Text);
            }
            if (ddlSearch.SelectedValue == "Projectnumber")
            {
                _objPJ.ProjectNumber = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJDetails.ProjectNumber = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
            }

            if (ddlSearch.SelectedValue == "Vendor")
            {
                _objPJ.Vendor = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJDetails.Vendor = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

            }
            if (ddlSearch.SelectedValue == "Amount")
            {
                _objPJ.Amount = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToDouble(txtSearch.Text);
                _getAllPJDetails.Amount = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToDouble(txtSearch.Text);

            }
            if (ddlSearch.SelectedValue == "AmountDue")
            {
                _objPJ.Balance = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToDouble(txtSearch.Text);
                _getAllPJDetails.Balance = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToDouble(txtSearch.Text);

            }
            if (ddlSearch.SelectedValue == "VendorName")
            {
                _objPJ.vendorName = txtVendorSearch.Text;
                _getAllPJDetails.vendorName = txtVendorSearch.Text;

            }
            if (ddlSearch.SelectedValue == "Custom1")
            {
                _objPJ.Custom1 = txtVendorSearch.Text;
                _getAllPJDetails.Custom1 = txtVendorSearch.Text;

            }
            if (ddlSearch.SelectedValue == "Custom2")
            {
                _objPJ.Custom2 = txtVendorSearch.Text;
                _getAllPJDetails.Custom2 = txtVendorSearch.Text;
            }
            //if (ddlSearch.SelectedValue == "Status")
            //{
            //    _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue); //Convert.ToInt16(txtSearch.Text);
            //    _getAllPJDetails.Status = Convert.ToInt16(ddlStatus.SelectedValue); //Convert.ToInt16(txtSearch.Text);
            //    Session["ddlStatus"] = ddlStatus.SelectedValue;
            //}            
            //else
            //{
            //    _objPJ.Status = 99;
            //    _getAllPJDetails.Status = 99;
            //}

            if (ddlSearch.SelectedValue == "Status")
            {
                var Status = string.Empty;
                if (ddlStatus.CheckedItems.Count > 0)
                {
                    foreach (var item in ddlStatus.CheckedItems)
                    {
                        Status += item.Value + ",";
                    }

                    Status = Status.TrimEnd(',');

                    _objPJ.SearchwithmultipleStatus = "(" + Status + ")";
                    _getAllPJDetails.SearchwithmultipleStatus = "(" + Status + ")";
                    Session["ddlStatus"] = "(" + Status + ")";

                }
            }
            else
            {
                _objPJ.SearchwithmultipleStatus = "99";

                _getAllPJDetails.SearchwithmultipleStatus = "99";
            }
            if (ddlSearch.SelectedValue == "Code")
            {
                _objPJ.Terms = Convert.ToInt16(ddlBOMType.SelectedValue); //Convert.ToInt16(txtSearch.Text);
                _getAllPJDetails.Terms = Convert.ToInt16(ddlBOMType.SelectedValue); //Convert.ToInt16(txtSearch.Text);
                Session["ddlBOMType"] = ddlBOMType.SelectedValue;
            }
            else
            {
                _objPJ.Terms = 99;
                _getAllPJDetails.Terms = 99;
            }
            if (ddlSearch.SelectedValue == "VendorType")
            {
                _objPJ.VendorType = Convert.ToString(ddlType.SelectedValue);
                _getAllPJDetails.VendorType = Convert.ToString(ddlType.SelectedValue);
                Session["ddlType"] = ddlType.SelectedValue;
            }
            else
            {
                _objPJ.VendorType = "";
                _getAllPJDetails.VendorType = "";
            }
            _objPJ.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);
            _getAllPJDetails.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);

            if (_objPJ.SearchValue.Equals(2))
            {
                if (string.IsNullOrEmpty(txtToDate.Text))
                {
                    _objPJ.SearchDate = DateTime.Now;

                    txtToDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    _objPJ.SearchDate = Convert.ToDateTime(txtToDate.Text);
                }
            }

            if (_getAllPJDetails.SearchValue.Equals(2))
            {
                if (string.IsNullOrEmpty(txtToDate.Text))
                {
                    _getAllPJDetails.SearchDate = DateTime.Now;

                    txtToDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    _getAllPJDetails.SearchDate = Convert.ToDateTime(txtToDate.Text);
                }
            }

            if (Session["CmpChkDefault"].ToString() == "1")
            {
                _objPJ.EN = 1;
                _getAllPJDetails.EN = 1;
            }
            else
            {
                _objPJ.EN = 0;
                _getAllPJDetails.EN = 0;
            }
            _objPJ.Ref = txtRef.Text.Trim();
            _getAllPJDetails.Ref = txtRef.Text.Trim();
            _objPJ.Custom = ddlFilterDate.SelectedValue.ToString();

            List<PJViewModel> _PJViewModel = new List<PJViewModel>();
            //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            // if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            //{
            //    string APINAME = "BillAPI/BillsList_GetAllPJDetails";
            //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllPJDetails);
            //    _PJViewModel = (new JavaScriptSerializer()).Deserialize<List<PJViewModel>>(_APIResponse.ResponseData);
            //    _dsPJ = CommonMethods.ToDataSet<PJViewModel>(_PJViewModel);

            //}
            //else
            //{
            //    _dsPJ = _objBLBills.GetAllPJDetails(_objPJ);
            //}
            _dsPJ = _objBLBills.GetAllPJDetails(_objPJ);
            // Status open/closed filter 
            DataTable filterdt = new DataTable();
            DataSet FilteredDs = new DataSet();
            check = Convert.ToBoolean(Session["InClosed"]);
            if (check)
            {
                lnkChk.Checked = true;
                FilteredDs = _dsPJ.Copy();
            }
            else
            {
                if (_dsPJ.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = _dsPJ.Tables[0].Select("StatusName='Open' OR StatusName='Partial'");
                    if (dr.Length > 0)
                    {
                        filterdt = dr.CopyToDataTable();
                        FilteredDs.Tables.Add(filterdt);
                    }
                    else
                    {
                        FilteredDs = _dsPJ.Clone();
                    }
                }
                else
                {
                    FilteredDs = _dsPJ.Copy();

                }
            }
            RadGrid_Bills.VirtualItemCount = FilteredDs.Tables[0].Rows.Count;
            RadGrid_Bills.DataSource = FilteredDs;

            
                BusinessEntity.User objProp_User = new BusinessEntity.User();
                DataSet ds = new DataSet();
                objProp_User.ConnConfig = Session["config"].ToString();

            //API
            _getConnectionConfig.ConnConfig = Session["config"].ToString();

            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "BillAPI/BillsList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                ds = _objBLUser.getControl(objProp_User);
            }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsUseTaxAPBills"].ToString()) == false)
                    {
                        RadGrid_Bills.Columns[14].Visible = false;
                    }
                    else
                    {
                        RadGrid_Bills.Columns[14].Visible = true;
                    }


                }
                
            


            string filterexpression = string.Empty;
            filterexpression = RadGrid_Bills.MasterTableView.FilterExpression;
            if (filterexpression == "" || filterexpression == null)
            {
                Session["Bills"] = FilteredDs.Tables[0];

            }
            else
            {
                if (FilteredDs.Tables[0].Rows.Count > 0)
                {
                    //Session["Bills"] = FilteredDs.Tables[0].AsEnumerable().AsQueryable().Where(filterexpression).CopyToDataTable();
                    Session["Bills"] = FilteredDs.Tables[0];
                }
                else
                {
                    Session["Bills"] = FilteredDs.Tables[0];
                }

            }
            txtFromDate.Text = Session["BillfromDate"].ToString();
            txtToDate.Text = Session["BillToDate"].ToString();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void rdobill_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (rdobill.Checked == true )
            {
                Session["Journal_Type"] = "1";
                RadGrid_Bills.Rebind();
                //BindJournalGrid();
                lnkProcess.Visible = false;
                //lblHeader.Text = "Journal Entries";
            }
            else
            {
                Session["Journal_Type"] = "2";
                RadGrid_Bills.DataSource = new string[] { };
                RadGrid_Bills.DataBind();
                //lblRecordCount.Text = "0 Record(s) found.";
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

            if (rdoRecurring.Checked == true )
            {
                Session["Journal_Type"] = "2";
                RadGrid_Bills.Rebind();
                lnkProcess.Visible = true;
            }
            else
            {
                Session["Journal_Type"] = "1";
                RadGrid_Bills.DataSource = new string[] { };
                RadGrid_Bills.DataBind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void BindRecurringGridShowAll()
    {
        Session["txtSearch"] = txtSearch.Text;
        Session["txtRef"] = txtRef.Text;
        Session["txtVendorSearch"] = txtVendorSearch.Text;
        try
        {
            DataSet _dsPJ = new DataSet();
            _objPJ.ConnConfig = Session["config"].ToString();
            _objPJ.UserID = Convert.ToInt32(Session["UserID"].ToString());

            //API
            _getAllPJRecurrDetails.ConnConfig = Session["config"].ToString();
            _getAllPJRecurrDetails.UserID = Convert.ToInt32(Session["UserID"].ToString());

            //if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
            //{
            //    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            //    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            //}
            //string stdate = txtFromDate.Text + " 00:00:00";
            //string enddate = txtToDate.Text + " 23:59:59";
            string stdate = "1900-01-01 00:00:00";
            string enddate = DateTime.Now.ToShortDateString() + " 23:59:59";
            _objPJ.StartDate = Convert.ToDateTime(stdate);
            _objPJ.EndDate = Convert.ToDateTime(enddate);
            

            _getAllPJRecurrDetails.StartDate = Convert.ToDateTime(stdate);
            _getAllPJRecurrDetails.EndDate = Convert.ToDateTime(enddate);

            if (ddlSearch.SelectedValue == "PO")
            {
                _objPJ.PO = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.PO = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

            }
            if (ddlSearch.SelectedValue == "Projectnumber")
            {
                _objPJ.ProjectNumber = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.ProjectNumber = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

            }

            if (ddlSearch.SelectedValue == "Vendor")
            {
                _objPJ.Vendor = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.Vendor = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
            }
            if (ddlSearch.SelectedValue == "Amount")
            {
                _objPJ.Amount = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.Amount = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

            }
            if (ddlSearch.SelectedValue == "AmountDue")
            {
                _objPJ.Balance = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.Balance = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
            }
            if (ddlSearch.SelectedValue == "VendorName")
            {
                _objPJ.vendorName = txtVendorSearch.Text;
                _getAllPJRecurrDetails.vendorName = txtVendorSearch.Text;
            }
            if (ddlSearch.SelectedValue == "Custom1")
            {
                _objPJ.Custom1 = txtVendorSearch.Text;
                _getAllPJRecurrDetails.Custom1 = txtVendorSearch.Text;
            }
            if (ddlSearch.SelectedValue == "Custom2")
            {
                _objPJ.Custom2 = txtVendorSearch.Text;
                _getAllPJRecurrDetails.Custom2 = txtVendorSearch.Text;
            }
            //if (ddlSearch.SelectedValue == "Status")
            //{
            //    _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue); //Convert.ToInt16(txtSearch.Text);
            //    _getAllPJRecurrDetails.Status = Convert.ToInt16(ddlStatus.SelectedValue); //Convert.ToInt16(txtSearch.Text);
            //    Session["ddlStatus"] = ddlStatus.SelectedValue;
            //}
            //else
            //{
            //    _objPJ.Status = 99;
            //    _getAllPJRecurrDetails.Status = 99;
            //}
            if (ddlSearch.SelectedValue == "Status")
            {
                var Status = string.Empty;
                if (ddlStatus.CheckedItems.Count > 0)
                {
                    foreach (var item in ddlStatus.CheckedItems)
                    {
                        Status += item.Value + ",";
                    }

                    Status = Status.TrimEnd(',');

                    _objPJ.SearchwithmultipleStatus = "(" + Status + ")";
                    _getAllPJDetails.SearchwithmultipleStatus = "(" + Status + ")";
                    Session["ddlStatus"] = "(" + Status + ")";

                }
            }
            else
            {
                _objPJ.SearchwithmultipleStatus = "99";
                _getAllPJDetails.SearchwithmultipleStatus = "99";
            }

            if (ddlSearch.SelectedValue == "Code")
            {
                _objPJ.Terms = Convert.ToInt16(ddlBOMType.SelectedValue); //Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.Terms = Convert.ToInt16(ddlBOMType.SelectedValue); //Convert.ToInt16(txtSearch.Text);
                Session["ddlBOMType"] = ddlBOMType.SelectedValue;
            }
            else
            {
                _objPJ.Terms = 99;
                _getAllPJRecurrDetails.Terms = 99;
            }
            if (ddlSearch.SelectedValue == "VendorType")
            {
                _objPJ.VendorType = Convert.ToString(ddlType.SelectedValue);
                _getAllPJDetails.VendorType = Convert.ToString(ddlType.SelectedValue);
                Session["ddlType"] = ddlType.SelectedValue;
            }
            else
            {
                _objPJ.VendorType = "";
                _getAllPJDetails.VendorType = "";
            }
            _objPJ.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);
            _getAllPJRecurrDetails.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);

            if (_objPJ.SearchValue.Equals(2))
            {
                if (string.IsNullOrEmpty(txtToDate.Text))
                {
                    _objPJ.SearchDate = DateTime.Now;

                    txtToDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    _objPJ.SearchDate = Convert.ToDateTime(txtToDate.Text);
                }
            }

            if (_getAllPJRecurrDetails.SearchValue.Equals(2))
            {
                if (string.IsNullOrEmpty(txtToDate.Text))
                {
                    _getAllPJRecurrDetails.SearchDate = DateTime.Now;

                    txtToDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    _getAllPJRecurrDetails.SearchDate = Convert.ToDateTime(txtToDate.Text);
                }
            }

            if (Session["CmpChkDefault"].ToString() == "1")
            {
                _objPJ.EN = 1;
                _getAllPJRecurrDetails.EN = 1;
            }
            else
            {
                _objPJ.EN = 0;
                _getAllPJRecurrDetails.EN = 0;
            }
            _objPJ.Ref = txtRef.Text.Trim();
            _getAllPJRecurrDetails.Ref = txtRef.Text.Trim();
            _objPJ.Custom = ddlFilterDate.SelectedValue.ToString();
            List<GetAllPJRecurrDetailsViewModel> _lstGetAllPJRecurrDetails = new List<GetAllPJRecurrDetailsViewModel>();

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            //if (IsAPIIntegrationEnable == "YES")
            //{
            //    string APINAME = "BillAPI/BillsList_GetAllPJRecurrDetails";
            //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllPJRecurrDetails);
            //    _lstGetAllPJRecurrDetails = (new JavaScriptSerializer()).Deserialize<List<GetAllPJRecurrDetailsViewModel>>(_APIResponse.ResponseData);
            //    _dsPJ = CommonMethods.ToDataSet<GetAllPJRecurrDetailsViewModel>(_lstGetAllPJRecurrDetails);

            //}
            //else
            //{
            _dsPJ = _objBLBills.GetAllPJRecurrDetails(_objPJ);
            //}

            // Status open/closed filter 
            DataTable filterdt = new DataTable();
            DataSet FilteredDs = new DataSet();
            check = Convert.ToBoolean(Session["InClosed"]);
            if (check)
            {
                lnkChk.Checked = true;
                FilteredDs = _dsPJ.Copy();
            }
            else
            {
                if (_dsPJ.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = _dsPJ.Tables[0].Select("StatusName='Open'");
                    if (dr.Length > 0)
                    {
                        filterdt = dr.CopyToDataTable();
                        FilteredDs.Tables.Add(filterdt);
                    }
                    else
                    {
                        FilteredDs = _dsPJ.Clone();
                    }
                }
                else
                {
                    FilteredDs = _dsPJ.Copy();

                }
            }
            RadGrid_Bills.VirtualItemCount = FilteredDs.Tables[0].Rows.Count;
            RadGrid_Bills.DataSource = FilteredDs;

            BusinessEntity.User objProp_User = new BusinessEntity.User();
            DataSet ds = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();

            //API
            _getConnectionConfig.ConnConfig = Session["config"].ToString();

            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "BillAPI/BillsList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                ds = _objBLUser.getControl(objProp_User);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsUseTaxAPBills"].ToString()) == false)
                {
                    RadGrid_Bills.Columns[14].Visible = false;
                }
                else
                {
                    RadGrid_Bills.Columns[14].Visible = true;
                }


            }




            string filterexpression = string.Empty;
            filterexpression = RadGrid_Bills.MasterTableView.FilterExpression;
            if (filterexpression == "" || filterexpression == null)
            {
                Session["Bills"] = FilteredDs.Tables[0];

            }
            else
            {
                if (FilteredDs.Tables[0].Rows.Count > 0)
                {
                    //Session["Bills"] = FilteredDs.Tables[0].AsEnumerable().AsQueryable().Where(filterexpression).CopyToDataTable();
                    Session["Bills"] = FilteredDs.Tables[0];
                }
                else
                {
                    Session["Bills"] = FilteredDs.Tables[0];
                }

            }








            //string filterexpression = string.Empty;
            //filterexpression = RadGrid_Bills.MasterTableView.FilterExpression;
            //if (filterexpression == "" || filterexpression == null)
            //{
            //    Session["Bills"] = FilteredDs.Tables[0];

            //}
            //else
            //{
            //    Session["Bills"] = FilteredDs.Tables[0].AsEnumerable().AsQueryable().Where(filterexpression).CopyToDataTable();

            //}
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    

    private void BindRecurringGrid()
    {
        Session["txtSearch"] = txtSearch.Text;
        Session["txtRef"] = txtRef.Text;
        Session["txtVendorSearch"] = txtVendorSearch.Text;
        try
        {
            txtFromDate.Text = Session["BillfromDate"].ToString();
            txtToDate.Text = Session["BillToDate"].ToString();
            if (txtToDate.Text == "")
            {
                txtFromDate.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
                txtToDate.Text = DateTime.Now.AddYears(100).ToShortDateString();
            }

            DataSet _dsPJ = new DataSet();
            _objPJ.ConnConfig = Session["config"].ToString();
            _objPJ.UserID = Convert.ToInt32(Session["UserID"].ToString());

            //API
            _getAllPJRecurrDetails.ConnConfig = Session["config"].ToString();
            _getAllPJRecurrDetails.UserID = Convert.ToInt32(Session["UserID"].ToString());

            if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
            {
                txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            string stdate = txtFromDate.Text + " 00:00:00";
            string enddate = txtToDate.Text + " 23:59:59";
            _objPJ.StartDate = Convert.ToDateTime(stdate);
            _objPJ.EndDate = Convert.ToDateTime(enddate);

            _getAllPJRecurrDetails.StartDate = Convert.ToDateTime(stdate);
            _getAllPJRecurrDetails.EndDate = Convert.ToDateTime(enddate);

            if (ddlSearch.SelectedValue == "PO")
            {
                _objPJ.PO = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.PO = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

            }
            if (ddlSearch.SelectedValue == "Projectnumber")
            {
                _objPJ.ProjectNumber = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.ProjectNumber = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

            }

            if (ddlSearch.SelectedValue == "Vendor")
            {
                _objPJ.Vendor = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.Vendor = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
            }
            if (ddlSearch.SelectedValue == "Amount")
            {
                _objPJ.Amount = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.Amount = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

            }
            if (ddlSearch.SelectedValue == "AmountDue")
            {
                _objPJ.Balance = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.Balance = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);
            }
            if (ddlSearch.SelectedValue == "VendorName")
            {
                _objPJ.vendorName = txtVendorSearch.Text;
                _getAllPJRecurrDetails.vendorName = txtVendorSearch.Text;
            }
            if (ddlSearch.SelectedValue == "Custom1")
            {
                _objPJ.Custom1 = txtVendorSearch.Text;
                _getAllPJRecurrDetails.Custom1 = txtVendorSearch.Text;
            }
            if (ddlSearch.SelectedValue == "Custom2")
            {
                _objPJ.Custom2 = txtVendorSearch.Text;
                _getAllPJRecurrDetails.Custom2 = txtVendorSearch.Text;
            }
            //if (ddlSearch.SelectedValue == "Status")
            //{
            //    _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue); //Convert.ToInt16(txtSearch.Text);
            //    _getAllPJRecurrDetails.Status = Convert.ToInt16(ddlStatus.SelectedValue); //Convert.ToInt16(txtSearch.Text);
            //    Session["ddlStatus"] = ddlStatus.SelectedValue;
            //}
            //else
            //{
            //    _objPJ.Status = 99;
            //    _getAllPJRecurrDetails.Status = 99;
            //}
            if (ddlSearch.SelectedValue == "Status")
            {
                var Status = string.Empty;
                if (ddlStatus.CheckedItems.Count > 0)
                {
                    foreach (var item in ddlStatus.CheckedItems)
                    {
                        Status += item.Value + ",";
                    }

                    Status = Status.TrimEnd(',');

                    _objPJ.SearchwithmultipleStatus = "(" + Status + ")";
                    _getAllPJDetails.SearchwithmultipleStatus = "(" + Status + ")";
                    Session["ddlStatus"] = "(" + Status + ")";

                }
            }
            else
            {
                _objPJ.SearchwithmultipleStatus = "99";

                _getAllPJDetails.SearchwithmultipleStatus = "99";
            }
            if (ddlSearch.SelectedValue == "Code")
            {
                _objPJ.Terms = Convert.ToInt16(ddlBOMType.SelectedValue); //Convert.ToInt16(txtSearch.Text);
                _getAllPJRecurrDetails.Terms = Convert.ToInt16(ddlBOMType.SelectedValue); //Convert.ToInt16(txtSearch.Text);
                Session["ddlBOMType"] = ddlBOMType.SelectedValue;
            }
            else
            {
                _objPJ.Terms = 99;
                _getAllPJRecurrDetails.Terms = 99;
            }
            if (ddlSearch.SelectedValue == "VendorType")
            {
                _objPJ.VendorType = Convert.ToString(ddlType.SelectedValue);
                _getAllPJDetails.VendorType = Convert.ToString(ddlType.SelectedValue);
                Session["ddlType"] = ddlType.SelectedValue;
            }
            else
            {
                _objPJ.VendorType = "";
                _getAllPJDetails.VendorType = "";
            }
            _objPJ.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);
            _getAllPJRecurrDetails.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);

            if (_objPJ.SearchValue.Equals(2))
            {
                if (string.IsNullOrEmpty(txtToDate.Text))
                {
                    _objPJ.SearchDate = DateTime.Now;

                    txtToDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    _objPJ.SearchDate = Convert.ToDateTime(txtToDate.Text);
                }
            }

            if (_getAllPJRecurrDetails.SearchValue.Equals(2))
            {
                if (string.IsNullOrEmpty(txtToDate.Text))
                {
                    _getAllPJRecurrDetails.SearchDate = DateTime.Now;

                    txtToDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    _getAllPJRecurrDetails.SearchDate = Convert.ToDateTime(txtToDate.Text);
                }
            }

            if (Session["CmpChkDefault"].ToString() == "1")
            {
                _objPJ.EN = 1;
                _getAllPJRecurrDetails.EN = 1;
            }
            else
            {
                _objPJ.EN = 0;
                _getAllPJRecurrDetails.EN = 0;
            }
            _objPJ.Ref = txtRef.Text.Trim();
            _getAllPJRecurrDetails.Ref = txtRef.Text.Trim();
            _objPJ.Custom = ddlFilterDate.SelectedValue.ToString();
            List<GetAllPJRecurrDetailsViewModel> _lstGetAllPJRecurrDetails = new List<GetAllPJRecurrDetailsViewModel>();
            //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            //{
            //    string APINAME = "BillAPI/BillsList_GetAllPJRecurrDetails";
            //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllPJRecurrDetails);
            //    _lstGetAllPJRecurrDetails = (new JavaScriptSerializer()).Deserialize<List<GetAllPJRecurrDetailsViewModel>>(_APIResponse.ResponseData);
            //    _dsPJ = CommonMethods.ToDataSet<GetAllPJRecurrDetailsViewModel>(_lstGetAllPJRecurrDetails);

            //}
            //else
            //{
            _dsPJ = _objBLBills.GetAllPJRecurrDetails(_objPJ);
            //}

            // Status open/closed filter 
            DataTable filterdt = new DataTable();
            DataSet FilteredDs = new DataSet();
            check = Convert.ToBoolean(Session["InClosed"]);
            if (check)
            {
                lnkChk.Checked = true;
                FilteredDs = _dsPJ.Copy();
            }
            else
            {
                if (_dsPJ.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = _dsPJ.Tables[0].Select("StatusName='Open'");
                    if (dr.Length > 0)
                    {
                        filterdt = dr.CopyToDataTable();
                        FilteredDs.Tables.Add(filterdt);
                    }
                    else
                    {
                        FilteredDs = _dsPJ.Clone();
                    }
                }
                else
                {
                    FilteredDs = _dsPJ.Copy();

                }
            }
            RadGrid_Bills.VirtualItemCount = FilteredDs.Tables[0].Rows.Count;
            RadGrid_Bills.DataSource = FilteredDs;

            BusinessEntity.User objProp_User = new BusinessEntity.User();
            DataSet ds = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();

            //API
            _getConnectionConfig.ConnConfig = Session["config"].ToString();

            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "BillAPI/BillsList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                ds = _objBLUser.getControl(objProp_User);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsUseTaxAPBills"].ToString()) == false)
                {
                    RadGrid_Bills.Columns[14].Visible = false;
                }
                else
                {
                    RadGrid_Bills.Columns[14].Visible = true;
                }


            }




            string filterexpression = string.Empty;
            filterexpression = RadGrid_Bills.MasterTableView.FilterExpression;
            if (filterexpression == "" || filterexpression == null)
            {
                Session["Bills"] = FilteredDs.Tables[0];

            }
            else
            {
                if (FilteredDs.Tables[0].Rows.Count > 0)
                {
                    //Session["Bills"] = FilteredDs.Tables[0].AsEnumerable().AsQueryable().Where(filterexpression).CopyToDataTable();
                    Session["Bills"] = FilteredDs.Tables[0];
                }
                else
                {
                    Session["Bills"] = FilteredDs.Tables[0];
                }

            }


            txtFromDate.Text = Session["BillfromDate"].ToString();
            txtToDate.Text = Session["BillToDate"].ToString();





            //string filterexpression = string.Empty;
            //filterexpression = RadGrid_Bills.MasterTableView.FilterExpression;
            //if (filterexpression == "" || filterexpression == null)
            //{
            //    Session["Bills"] = FilteredDs.Tables[0];

            //}
            //else
            //{
            //    Session["Bills"] = FilteredDs.Tables[0].AsEnumerable().AsQueryable().Where(filterexpression).CopyToDataTable();

            //}
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    protected void ddlInvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInvoice.SelectedValue.Equals("2"))
        {
            txtToDate.Visible = true;
            txtFromDate.Visible = false;
            //lblStart.Visible = false;
        }
        else
        {
            txtFromDate.Visible = true;
            //lblStart.Visible = true;
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
            objProp_User.Type = "Bill";

            //API
            //_getStockReports.DBName = Session["dbname"].ToString();
            _getStockReports.ConnConfig = Session["config"].ToString();
            //_getStockReports.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _getStockReports.Type = "Bill";

            List<CustomerReportViewModel> _CustomerReportViewModel = new List<CustomerReportViewModel>();
            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {

                string APINAME = "BillAPI/BillsList_GetStockReports";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getStockReports);

                _CustomerReportViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomerReportViewModel>>(_APIResponse.ResponseData);
                dsGetReports = CommonMethods.ToDataSet<CustomerReportViewModel>(_CustomerReportViewModel);
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
            throw ex;
        }
        return lstCustomerReport;
    }
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();

        //API
        _getUserById.TypeID = Convert.ToInt32(Session["usertypeid"]);
        _getUserById.UserID = Convert.ToInt32(Session["userid"]);
        _getUserById.ConnConfig = Session["config"].ToString();
        _getUserById.DBName = Session["dbname"].ToString();

        DataSet ds = new DataSet();

        List<UserViewModel> _UserViewModel = new List<UserViewModel>();
        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {

            string APINAME = "BillAPI/AddBills_GetUserById";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getUserById);

            _UserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<UserViewModel>(_UserViewModel);
        }
        else
        {
            ds = _objBLUser.GetUserPermissionByUserID(objPropUser);
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

            /// Bill ///////////////////------->

            string BillPayPermission = ds.Rows[0]["Bill"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Bill"].ToString();
            string ADD = BillPayPermission.Length < 1 ? "Y" : BillPayPermission.Substring(0, 1);
            string Edit = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(1, 1);
            string Delete = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(2, 1);
            string View = BillPayPermission.Length < 4 ? "Y" : BillPayPermission.Substring(3, 1);
            if (ADD == "N")
            {

                lnkAddnew.Visible = false;
            }
            if (Edit == "N")
            {
                lnkEdit.Visible = false;
                lnkEditBillDate.Visible = false;
                lnkCopy.Visible = false;

            }
            if (Delete == "N")
            {
                lnkDelete.Visible = false;

            }
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
        }
    }
    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportsName());
        string reports = "var reports=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", reports, true);
    }

    
    protected void lbtnDateSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool Flag = false;
            Flag = CommonHelper.GetPeriodDetails(Convert.ToDateTime(txtPostingDate.Text));
            if (Flag)
            {

                _objPJ.ConnConfig = Session["config"].ToString();
                _objPJ.ID = Convert.ToInt32(hdnID.Value);
                _objPJ.Batch = Convert.ToInt32(hdnBatch.Value);
                _objPJ.TRID = Convert.ToInt32(hdnTRID.Value);
                _objPJ.PostDate = Convert.ToDateTime(txtPostingDate.Text);
                _objPJ.IDate = Convert.ToDateTime(txtDate.Text);
                _objPJ.Due = Convert.ToDateTime(txtDueDate.Text);
                _objPJ.MOMUSer = Session["User"].ToString();
                //API
                _updateAPDates.ConnConfig = Session["config"].ToString();
                _updateAPDates.ID = Convert.ToInt32(hdnID.Value);
                _updateAPDates.Batch = Convert.ToInt32(hdnBatch.Value);
                _updateAPDates.TRID = Convert.ToInt32(hdnTRID.Value);
                _updateAPDates.PostDate = Convert.ToDateTime(txtPostingDate.Text);
                _updateAPDates.IDate = Convert.ToDateTime(txtDate.Text);
                _updateAPDates.Due = Convert.ToDateTime(txtDueDate.Text);

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "BillAPI/BillsList_UpdateAPDates";
                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateAPDates);
                }
                else
                {
                    _objBLBills.UpdateAPDates(_objPJ);
                }

                DataTable dt = (DataTable)Session["Bills"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    DataRow dr = dt.Select("ID =" + _updateAPDates.ID).SingleOrDefault();

                    if (dr != null)
                    {
                        dr["fDate"] = _updateAPDates.PostDate;
                        dr["PostingDate"] = _updateAPDates.PostDate;
                        dr["Date"] = _updateAPDates.IDate;
                        dr["Due"] = _updateAPDates.Due;
                    }
                }
                else
                {
                    DataRow dr = dt.Select("ID =" + _objPJ.ID).SingleOrDefault();

                    if (dr != null)
                    {
                        dr["fDate"] = _objPJ.PostDate;
                        dr["PostingDate"] = _objPJ.PostDate;
                        dr["Date"] = _objPJ.IDate;
                        dr["Due"] = _objPJ.Due;
                    }
                }

                dt.AcceptChanges();
                Session["Bills"] = dt;
                ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Bill Date Updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                BindBillGrid();
                RadGrid_Bills.Rebind();
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'These month/year period is closed out. You do not have permission to add/update this record.',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void BindSearchFilters()
    {
        Dictionary<string, string> listsearchitems = new Dictionary<string, string>();
        listsearchitems.Add("0", "Select");
        listsearchitems.Add("PO", "PO#");
        listsearchitems.Add("Projectnumber", "Project number");
        //listsearchitems.Add("Vendor", "Vendor ID");
        listsearchitems.Add("VendorName", "Vendor Name");
        listsearchitems.Add("Status", "Status");
        listsearchitems.Add("Amount", "Amount");
        listsearchitems.Add("Balance", "AmountDue");
        listsearchitems.Add("Code", "BOM Type");
        listsearchitems.Add("VendorType", "Vendor Type");
        DataSet ds = new DataSet();
        General objPropGeneral = new General();
        BL_General objBL_General = new BL_General();
        String cusLabel = "";
        for (int i = 1; i < 3; i++)
        {
            objPropGeneral.CustomName = "PO" + Convert.ToString(i);
            objPropGeneral.ConnConfig = Session["config"].ToString();

            //API
            _getCustomFields.CustomName = "PO" + Convert.ToString(i);
            _getCustomFields.ConnConfig = Session["config"].ToString();

            List<CustomViewModel> _CustomViewModel = new List<CustomViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "BillAPI/AddBills_GetCustomFields";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);

                _CustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<CustomViewModel>(_CustomViewModel);
            }
            else
            {
                ds = objBL_General.getCustomFields(objPropGeneral);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                cusLabel = ds.Tables[0].Rows[0]["label"].ToString() == "" ? "Custom " + i : ds.Tables[0].Rows[0]["label"].ToString();
                listsearchitems.Add( "Custom" + i, cusLabel);
            }
        }

        ddlSearch.DataSource = listsearchitems;
        ddlSearch.DataTextField = "Value";
        ddlSearch.DataValueField = "Key";
        ddlSearch.DataBind();

    }
   
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ddlSearch"] = ddlSearch.SelectedValue;
        if (ddlSearch.SelectedValue == "Status")
        {
            txtSearch.Visible = false;
            ddlStatus.Visible = true;
            ddlBOMType.Visible = false;
            ddlType.Visible = false;
            txtVendorSearch.Visible = false;
            ddlStatus.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            ddlInvoice.SelectedIndex = 0;
        }
        else if (ddlSearch.SelectedValue == "VendorName" || ddlSearch.SelectedValue== "Custom1" || ddlSearch.SelectedValue == "Custom2")
        {
            txtSearch.Visible = false;
            ddlStatus.Visible = false;
            ddlBOMType.Visible = false;
            ddlType.Visible = false;
            txtVendorSearch.Visible = true;
            txtVendorSearch.Text = "";
            ddlType.SelectedIndex = 0;
            ddlInvoice.SelectedIndex = 0;
        }
        else if (ddlSearch.SelectedValue == "Code")
        {
            txtSearch.Visible = false;
            ddlBOMType.Visible = true;
            ddlStatus.Visible = false;
            ddlType.Visible = false;
            txtVendorSearch.Visible = false;
            ddlBOMType.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            ddlInvoice.SelectedIndex = 0;
        }
        else if (ddlSearch.SelectedValue == "VendorType")
        {
            txtSearch.Visible = false;
            ddlBOMType.Visible = false;
            ddlStatus.Visible = false;
            ddlType.Visible = true;
            txtVendorSearch.Visible = false;
            ddlBOMType.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            ddlInvoice.SelectedIndex = 0;
        }
        else
        {
            txtSearch.Visible = true;
            txtVendorSearch.Visible = false;
            ddlStatus.Visible = false;
            ddlBOMType.Visible = false;
            ddlType.Visible = false;
            txtSearch.Text = "";
            ddlType.SelectedIndex = 0;
            ddlInvoice.SelectedIndex = 0;
        }
    }
    protected void ddlFilterDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["BillFilterDate"] = ddlFilterDate.SelectedValue;
        
    }
    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        if (lnkChk.Checked)
        {
            Session["InClosed"] = "True";
            check = true;
            //BindBillGrid();
            if (rdobill.Checked)
            {
                Session["Journal_Type"] = "1";
                BindBillGrid();
            }
            else
            {
                Session["Journal_Type"] = "2";
                BindRecurringGrid();

            }
            RadGrid_Bills.Rebind();
            if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
            {
                Session["BillToDate"] = txtToDate.Text;
                Session["BillfromDate"] = txtFromDate.Text;
            }

        }
        else
        {
            Session["InClosed"] = "False";
            check = false;
            //BindBillGrid();
            if (rdobill.Checked)
            {
                Session["Journal_Type"] = "1";
                BindBillGrid();
            }
            else
            {
                Session["Journal_Type"] = "2";
                BindRecurringGrid();

            }
            RadGrid_Bills.Rebind();
            if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
            {
                Session["BillToDate"] = txtToDate.Text;
                Session["BillfromDate"] = txtFromDate.Text;
            }
        }
    }
    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Bills.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblIndex = (Label)di.FindControl("lblIndex");

            if (chkSelect.Checked.Equals(true))
            {
                //Response.Redirect("addbills.aspx?id=" + lblIndex.Text + "&t=c");
                if (rdobill.Checked == true)
                {
                    ///////// Check Account Active/Inactive in bill ///////
                    _objPJ.ConnConfig = Session["config"].ToString();
                    _objPJ.ID = Convert.ToInt32(lblIndex.Text);

                    //API
                    _getPJAcctDetailByID.ConnConfig = Session["config"].ToString();
                    _getPJAcctDetailByID.ID = Convert.ToInt32(lblIndex.Text);

                    DataSet _dsPJ = new DataSet();

                    List<PJViewModel> _PJViewModel = new List<PJViewModel>();
                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "BillAPI/BillsList_GetPJAcctDetailByID";
                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getPJAcctDetailByID);
                        _PJViewModel = (new JavaScriptSerializer()).Deserialize<List<PJViewModel>>(_APIResponse.ResponseData);
                        _dsPJ = CommonMethods.ToDataSet<PJViewModel>(_PJViewModel);
                    }
                    else
                    {
                         _dsPJ = _objBLBills.GetPJAcctDetailByID(_objPJ);
                    }
                    if (_dsPJ.Tables[0].Rows.Count > 0)
                    {
                        DataRow _drPJ = _dsPJ.Tables[0].Rows[0];                        
                        string acctname = Convert.ToString(_drPJ["AcctName"]);
                        //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'This account is inactive. Please change the account name before proceeding.',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);                        
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "display warn msg", "notyConfirm('"+ lblIndex.Text + "');", true);
                        
                    }
                    else
                    {
                        Response.Redirect("addbills.aspx?id=" + lblIndex.Text + "&t=c");
                    }
                    ///////// Check Account Active/Inactive in bill ///////
                    //Response.Redirect("addbills.aspx?id=" + lblIndex.Text + "&t=c");
                }
                else if (rdoRecurring.Checked == true)
                {                    
                    Response.Redirect("addbills.aspx?id=" + lblIndex.Text + "&t=c&r=1");
                }
            }
        }
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);
            txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            Session["BillfromDate"] = txtFromDate.Text;
            Session["BillToDate"] = txtToDate.Text;
        
        //BindBillGrid();
        if (rdobill.Checked)
        {
            Session["Journal_Type"] = "1";
            BindBillGrid();
        }
        else
        {
            Session["Journal_Type"] = "2";
            BindRecurringGrid();

        }
        foreach (GridColumn column in RadGrid_Bills.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_Bills.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Bills.Rebind();

        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = "";
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
        
        rdobill.Checked = true;
        udpRdo.Update();
        check = false;
        Session["InClosed"]= "False";
        lnkChk.Checked = false;
        txtRef.Text = "";
        txtVendorSearch.Text = "";
        ddlInvoice.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;

    }

    protected void lnkBillsReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchText = string.Empty;
            //if (ddlSearch.SelectedValue == "Status")
            //{
            //    searchText = ddlStatus.SelectedValue;
            //}

            if (ddlSearch.SelectedValue == "Status")
            {
                var Status = string.Empty;
                if (ddlStatus.CheckedItems.Count > 0)
                {
                    foreach (var item in ddlStatus.CheckedItems)
                    {
                        Status += item.Value + ",";
                    }

                    Status = Status.TrimEnd(',');
                    searchText = "(" + Status + ")";
                   
                }
            }
          
            else if (ddlSearch.SelectedValue == "VendorName" || ddlSearch.SelectedValue == "Custom1" || ddlSearch.SelectedValue == "Custom2")
            {
                searchText = txtVendorSearch.Text.Trim();
            }
            else if (ddlSearch.SelectedValue == "Code")
            {
                searchText = ddlBOMType.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "VendorType")
            {
                searchText = ddlType.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text.Trim();
            }

            string urlString = "BillsReport.aspx?dtype=" + ddlFilterDate.SelectedValue + "&sd=" + txtFromDate.Text + "&ed=" + txtToDate.Text
                + "&stype=" + ddlSearch.SelectedValue + "&stext=" + searchText + "&ref=" + txtRef.Text.Trim() + "&itype=" + ddlInvoice.SelectedValue + "&inclClose=" + lnkChk.Checked;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkBillRegisterGL_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchText = string.Empty;
            //if (ddlSearch.SelectedValue == "Status")
            //{
            //    searchText = ddlStatus.SelectedValue;
            //}
            if (ddlSearch.SelectedValue == "Status")
            {
                var Status = string.Empty;
                if (ddlStatus.CheckedItems.Count > 0)
                {
                    foreach (var item in ddlStatus.CheckedItems)
                    {
                        Status += item.Value + ",";
                    }

                    Status = Status.TrimEnd(',');
                    searchText = "(" + Status + ")";

                }
            }
            else if (ddlSearch.SelectedValue == "VendorName" || ddlSearch.SelectedValue == "Custom1" || ddlSearch.SelectedValue == "Custom2")
            {
                searchText = txtVendorSearch.Text.Trim();
            }
            else if (ddlSearch.SelectedValue == "Code")
            {
                searchText = ddlBOMType.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "VendorType")
            {
                searchText = ddlType.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text.Trim();
            }

            string urlString = "PrintBillRegisterGL.aspx?dtype=" + ddlFilterDate.SelectedValue + "&sd=" + txtFromDate.Text + "&ed=" + txtToDate.Text 
                + "&stype=" + ddlSearch.SelectedValue + "&stext=" + searchText + "&ref=" + txtRef.Text.Trim() + "&itype=" + ddlInvoice.SelectedValue + "&inclClose=" + lnkChk.Checked;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillVendorType()
    {
        DataSet ds = new DataSet();
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getVendorType.ConnConfig = Session["config"].ToString();

        List<GetVendorTypeViewModel> _GetVendorTypeViewModel = new List<GetVendorTypeViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {

            string APINAME = "VendorAPI/VendorList_getVendorType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorType);

            _GetVendorTypeViewModel = (new JavaScriptSerializer()).Deserialize<List<GetVendorTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetVendorTypeViewModel>(_GetVendorTypeViewModel);
        }
        else
        {
            ds = objBL_User.getVendorType(objPropUser);
        }
        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "Type";
        ddlType.DataValueField = "Type";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new ListItem("Select", "0"));


    }
}
public class EditManageBillCodeModel
{
    public String ID { get; set; }
    public String TRID { get; set; }
    public String Ref { get; set; }
    public String PostingDate { get; set; }
    public String Date { get; set; }
    public String Due { get; set; }
    public string Batch { get; set; }
    public String lblIndex { get; set; }
}