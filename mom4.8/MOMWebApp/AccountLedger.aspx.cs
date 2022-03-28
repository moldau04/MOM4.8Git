using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessEntity;
using BusinessLayer;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;

public partial class AccountLedger : System.Web.UI.Page
{
    #region "Variables"

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();
    CommonHelper _objComhelp = new CommonHelper();
    Frequency _objFrequency = new Frequency();
    BL_Bills _objBLBills = new BL_Bills();
    PJ _objPJ = new PJ();
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    //ReceivedPayment _objReceiPmt = new ReceivedPayment();
    PaymentDetails _objPayment = new PaymentDetails();
    BL_Deposit _objBL_Deposit = new BL_Deposit();

    Transaction _objTrans = new Transaction();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();

    CD _objCD = new CD();

    Double dPageTotal = 0;
    Double cPageTotal = 0;
    Double Balance = 0;
    Double TotalBalance = 0;
    private static int IntBalance = 0;
    private static int intAccountExportExcel = 0;

    #endregion

    #region "Events"

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["acctId"] = Convert.ToInt32(Request.QueryString["id"]);
        if (!IsPostBack)
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }
            if (Request.QueryString["id"] != null)
            {
                if (Request.QueryString["s"] != null && Request.QueryString["e"] != null)
                {
                    var startDate = Convert.ToDateTime(System.Web.HttpUtility.UrlDecode(Request.QueryString["s"].ToString())).ToShortDateString();
                    var endDate = Convert.ToDateTime(System.Web.HttpUtility.UrlDecode(Request.QueryString["e"].ToString())).ToShortDateString();
                    txtStartDate.Text = startDate;
                    txtEndDate.Text = endDate;
                }
                else
                {
                    DateTime _now = DateTime.Now;
                    var _startDate = new DateTime(_now.Year, 1, 1);
                    var _endDate = _startDate.AddMonths(12).AddDays(-1);
                    txtStartDate.Text = _startDate.ToShortDateString();
                    txtEndDate.Text = _endDate.ToShortDateString();

                }

                SetHeader();
            }
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("ChartOfAccount.aspx");
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        BindChartGrid(true);
        RadGrid_AccountLedger.Rebind();
    }

    #endregion

    #region "Custom Function"

    private void BindChartGrid(bool isReload = false)
    {
        try
        {
            DataSet dsChart = new DataSet();
            _objChart.ConnConfig = Session["config"].ToString();
            _objChart.ID = Convert.ToInt32(Request.QueryString["id"]);
            _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
            _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);

            DateTime temp;
            if (DateTime.TryParse(RadGrid_AccountLedger.MasterTableView.GetColumn(colFDates).CurrentFilterValue, out temp))
            {
                _objChart.filterDate = Convert.ToDateTime(RadGrid_AccountLedger.MasterTableView.GetColumn(colFDates).CurrentFilterValue);
            }

            if (RadGrid_AccountLedger.MasterTableView.GetColumn(colDebit).CurrentFilterValue != null && RadGrid_AccountLedger.MasterTableView.GetColumn(colDebit).CurrentFilterValue != "")
            {
                _objChart.filterDebit = Convert.ToDouble(RadGrid_AccountLedger.MasterTableView.GetColumn(colDebit).CurrentFilterValue);
            }
            if (RadGrid_AccountLedger.MasterTableView.GetColumn(colCredit).CurrentFilterValue != null && RadGrid_AccountLedger.MasterTableView.GetColumn(colCredit).CurrentFilterValue != "")
            {
                _objChart.filterCredit = Convert.ToDouble(RadGrid_AccountLedger.MasterTableView.GetColumn(colCredit).CurrentFilterValue);
            }
            if (RadGrid_AccountLedger.MasterTableView.GetColumn(colBalance).CurrentFilterValue != null && RadGrid_AccountLedger.MasterTableView.GetColumn(colCredit).CurrentFilterValue != "")
            {
                _objChart.filterBalance = Convert.ToDouble(RadGrid_AccountLedger.MasterTableView.GetColumn(colBalance).CurrentFilterValue);
            }

            _objChart.filterTypeText = RadGrid_AccountLedger.MasterTableView.GetColumn(colType).CurrentFilterValue;
            _objChart.filterRef = RadGrid_AccountLedger.MasterTableView.GetColumn(colRef).CurrentFilterValue;
            _objChart.filterfDesc = RadGrid_AccountLedger.MasterTableView.GetColumn(colDescription).CurrentFilterValue;

            _objChart.pageNumber = isReload ? 1 : RadGrid_AccountLedger.CurrentPageIndex + 1;
            _objChart.PageSize = RadGrid_AccountLedger.PageSize;
            dsChart = _objBLChart.GetAccountLedgerPaging(_objChart);

            if (dsChart.Tables[0].Rows.Count > 0)
            {
                double balance = Convert.ToDouble(dsChart.Tables[0].Rows[0]["Balance"]);
                ViewState["TotalBalance"] = balance;
            }

            Session["stardate"] = _objChart.StartDate;
            Session["enddate"] = _objChart.EndDate;

            int totalRow = 0;
            if (dsChart.Tables[0].Rows.Count > 0)
            {
                totalRow = Convert.ToInt32(dsChart.Tables[0].Rows[0]["TotalRow"]);
            }

            RadGrid_AccountLedger.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
            RadGrid_AccountLedger.VirtualItemCount = totalRow;
            RadGrid_AccountLedger.DataSource = dsChart.Tables[0];

            ViewState["Chart"] = dsChart.Tables[0];
            ViewState["GrandTotal"] = dsChart.Tables[2];
            lblRecordCount.Text = totalRow + " Record(s) found";
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void SetHeader()
    {
        DataSet dsChart = new DataSet();
        _objChart.ConnConfig = Session["config"].ToString();
        _objChart.ID = Convert.ToInt32(Request.QueryString["id"]);
        dsChart = _objBLChart.GetChart(_objChart);

        if (dsChart.Tables[0].Rows.Count > 0)
        {
            lblAccountName.Text = dsChart.Tables[0].Rows[0]["fDesc"].ToString();
            lblAccountNo.Text = dsChart.Tables[0].Rows[0]["Acct"].ToString();
        }
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        txtStartDate.Text = "01/01/2000";
        if (Session["enddate"] != null)
        {
            txtEndDate.Text = Convert.ToString(Session["enddate"]);
        }
        else
        {
            txtEndDate.Text = Convert.ToString(DateTime.Now);
        }

        BindChartGrid(true);

        foreach (GridColumn column in RadGrid_AccountLedger.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }

        RadGrid_AccountLedger.MasterTableView.FilterExpression = string.Empty;
        RadGrid_AccountLedger.Rebind();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        foreach (GridColumn column in RadGrid_AccountLedger.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }

        BindChartGrid(true);
        RadGrid_AccountLedger.MasterTableView.FilterExpression = string.Empty;
        RadGrid_AccountLedger.Rebind();
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_AccountLedger.MasterTableView.FilterExpression != "" ||
            (RadGrid_AccountLedger.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_AccountLedger.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_AccountLedger_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        Session["alId"] = null;
        RadGrid_AccountLedger.AllowCustomPaging = !ShouldApplySortFilterOrGroup();

        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["AccountLedger_FilterExpression"] != null && Convert.ToString(Session["AccountLedger_FilterExpression"]) != "" && Session["AccountLedger_Filters"] != null)
                {
                    RadGrid_AccountLedger.MasterTableView.FilterExpression = Convert.ToString(Session["AccountLedger_FilterExpression"]);
                    var filtersGet = Session["AccountLedger_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_AccountLedger.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["AccountLedger_FilterExpression"] = null;
                Session["AccountLedger_Filters"] = null;
            }
        }
        #endregion

        if (Request.QueryString["id"] != null)
        {
            Session["alId"] = Request.QueryString["id"];
            BindChartGrid();
        }
    }

    private void RowSelect()
    {
        foreach (GridDataItem gv in RadGrid_AccountLedger.Items)
        {
            HiddenField hdnlink = (HiddenField)gv.FindControl("hdnLink");
            gv.Attributes["ondblclick"] = "window.open('" + hdnlink.Value + "&page=AccountLedger');";
        }
    }

    protected void RadGrid_AccountLedger_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_AccountLedger.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["AccountLedger_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_AccountLedger.MasterTableView.OwnerGrid.Columns)
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
            Session["AccountLedger_Filters"] = filters;
            IntBalance = 0;
        }
        else
        {
            Session["AccountLedger_FilterExpression"] = null;
            Session["AccountLedger_Filters"] = null;
            IntBalance = 0;

        }

        RowSelect();
    }

    protected void RadGrid_AccountLedger_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            switch (e.Item.ItemType)
            {
                case GridItemType.Footer:
                    Label lblTotalDebit = default(Label);
                    lblTotalDebit = (Label)e.Item.FindControl("lblTotalDebit");

                    Label lblTotalCredit = default(Label);
                    lblTotalCredit = (Label)e.Item.FindControl("lblTotalCredit");

                    Label lblAccountBalance = default(Label);
                    lblAccountBalance = (Label)e.Item.FindControl("lblAccountBalance");

                    Label lblTotalBalance = default(Label);
                    lblTotalBalance = (Label)e.Item.FindControl("lblTotalBalance");

                    lblTotalDebit.Text = string.Format("{0:c}", dPageTotal);
                    lblTotalCredit.Text = string.Format("{0:c}", cPageTotal);
                    lblAccountBalance.Text = string.Format("{0:c}", Balance);
                    if (RadGrid_AccountLedger.Items.Count == 1)
                        lblTotalBalance.Text = string.Format("{0:c}", Balance);
                    else
                        lblTotalBalance.Text = string.Format("{0:c}", TotalBalance);
                    break;
                default:
                case GridItemType.Item:
                    HiddenField hdnType = default(HiddenField);
                    hdnType = (HiddenField)e.Item.FindControl("hdnRef");

                    if (hdnType != null && hdnType.Value != null)
                    {
                        Label lblBDebit = default(Label);
                        lblBDebit = (Label)e.Item.FindControl("lblBDebit");
                        dPageTotal += Convert.ToDouble(lblBDebit.Text);

                        Label lblBCredit = default(Label);
                        lblBCredit = (Label)e.Item.FindControl("lblBCredit");
                        cPageTotal += Convert.ToDouble(lblBCredit.Text);

                        Label lblACBalance = default(Label);
                        lblACBalance = (Label)e.Item.FindControl("lblACBalance");
                        if (IntBalance == 0)
                        {
                            Balance = Convert.ToDouble(lblACBalance.Text);
                            IntBalance++;
                        }
                        else
                        {
                            TotalBalance = Convert.ToDouble(lblACBalance.Text);
                        }
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void SetTotal()
    {
        try
        {
            if (RadGrid_AccountLedger.Items.Count > 0)
            {
                double debitAmt = 0;
                double creditAmt = 0;
                //double Balance = 0;
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["GrandTotal"];
                debitAmt = Convert.ToDouble(dt.Rows[0]["Debit"]);
                creditAmt = Convert.ToDouble(dt.Rows[0]["Credit"]);

                GridFooterItem footerItem = (GridFooterItem)RadGrid_AccountLedger.MasterTableView.GetItems(GridItemType.Footer)[0];
                Label lblGrandTotalDebit = footerItem.FindControl("lblGrandTotalDebit") as Label;
                Label lblGrandTotalCredit = footerItem.FindControl("lblGrandTotalCredit") as Label;

                lblGrandTotalDebit.Text = string.Format("{0:c}", debitAmt);
                lblGrandTotalCredit.Text = string.Format("{0:c}", creditAmt);

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_AccountLedger_ItemEvent(object sender, GridItemEventArgs e)
    {
        updpnl.Update();
        SetTotal();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        ViewState["export"] = 1;

        // intAccountExportExcel = 1;
        RadGrid_AccountLedgerExport.ExportSettings.FileName = "Ledger";
        RadGrid_AccountLedgerExport.ExportSettings.IgnorePaging = true;
        RadGrid_AccountLedgerExport.ExportSettings.ExportOnlyData = true;
        RadGrid_AccountLedgerExport.ExportSettings.OpenInNewWindow = true;
        RadGrid_AccountLedgerExport.ExportSettings.HideStructureColumns = true;
        RadGrid_AccountLedgerExport.MasterTableView.UseAllDataFields = true;
        RadGrid_AccountLedgerExport.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_AccountLedgerExport.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_AccountLedger_ItemCreated(object sender, GridItemEventArgs e)
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

    #endregion

    private string colFDates = "fDates";
    private string colType = "Type";
    private string colRef = "Ref";
    private string colDescription = "Description";
    private string colDebit = "Debit";
    private string colCredit = "Credit";
    private string colBalance = "Balance";

    protected void RadGrid_AccountLedgerExport_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (e.Worksheet.Table.Rows.Count == RadGrid_AccountLedgerExport.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_AccountLedgerExport.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
            RowElement row = new RowElement();

            //create new row for the footer aggregates
            for (int i = currentItem; i < footerItem.Cells.Count; i++)
            {
                TableCell fcell = footerItem.Cells[i];

                CellElement cell = new CellElement();
                cell.Data.DataItem = fcell.Text == "&nbsp;" ? "" : fcell.Text;
                row.Cells.Add(cell);
            }
            e.Worksheet.Table.Rows.Add(row);
        }
    }

    protected void RadGrid_AccountLedgerExport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            if (ViewState["export"] != null || Convert.ToInt32(ViewState["export"]) == 1)
            {
                DataSet dsChart = new DataSet();
                _objChart.ConnConfig = Session["config"].ToString();
                _objChart.ID = Convert.ToInt32(Request.QueryString["id"]);
                _objChart.StartDate = Convert.ToDateTime(txtStartDate.Text);
                _objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
                dsChart = _objBLChart.GetAccountLedger(_objChart);

                RadGrid_AccountLedgerExport.VirtualItemCount = dsChart.Tables[0].Rows.Count;
                RadGrid_AccountLedgerExport.DataSource = dsChart.Tables[0];

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_AccountLedgerExport_PreRender(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["export"]) == 1)
        {
            GeneralFunctions obj = new GeneralFunctions();
            obj.CorrectTelerikPager(RadGrid_AccountLedger);
            ViewState["export"] = 0;
        }
    }
}