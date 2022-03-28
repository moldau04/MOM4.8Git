using BusinessEntity;
using BusinessLayer;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PeriodCloseout : System.Web.UI.Page
{
    #region "Variables"
    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    Transaction _objTrans = new Transaction();
    Journal _objJournal = new Journal();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();
    User _objUser = new User();
    BL_User objBLUser = new BL_User();
    BL_Report objBL_Report = new BL_Report();
    bool checkChanged = false;
    #endregion

    #region "events"

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
            

            //setDateAndUserinfo();
            //SetLastYearDate();
            SetUserDatePermission();
            //Permission();
            HighlightSideMenu("progMgr", "lnkPeriodCloseOut", "progMgrSub");
            FillEarningGL();
        }
    }
    #endregion


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
    private void SetLastYearDate()
    {
        DateTime _lastYearMonth = new DateTime(DateTime.Now.AddYears(-1).Year, 12, 1);
        DateTime _endDateOfMonth = _lastYearMonth.AddMonths(1).AddDays(-1);
        txtCloseOutDate.Text = _endDateOfMonth.ToString("MM/dd/yyyy");
    }


    private void SetUserDatePermission()
    {
        DataSet dsUserDatePermission = new DataSet();
        _objUser.ConnConfig = Session["config"].ToString();
        dsUserDatePermission = objBLUser.GetUserDatePermission(_objUser);
        var checkValidUserDate = dsUserDatePermission.Tables[0].Rows[0][0].ToString();
        DateTime _fStart;
        DateTime _fEnd;
        if (String.IsNullOrEmpty(txtCloseOutDate.Text))
            SetLastYearDate();
        DateTime _closeOutDate = Convert.ToDateTime(txtCloseOutDate.Text);
        if (checkValidUserDate == "0")
        {
            _fStart = _closeOutDate.AddDays(+1);
            _fEnd = _closeOutDate.AddDays(+1).AddMonths(1).AddDays(-1);
        }
        else
        {
            _fStart = Convert.ToDateTime(dsUserDatePermission.Tables[0].Rows[0]["fStart"].ToString());
            _fEnd = Convert.ToDateTime(dsUserDatePermission.Tables[0].Rows[0]["fEnd"].ToString());
        }

        txtStartDate.Text = _fStart.ToString("MM/dd/yyyy");
        txtEndDate.Text = _fEnd.ToString("MM/dd/yyyy");

    }

    private void SetCloseOutDate()
    {
        _objUser.ConnConfig = Session["config"].ToString();
        _objUser.CODt = Convert.ToDateTime(txtCloseOutDate.Text.ToString());
        objBLUser.SetCloseOutDate(_objUser);
    }

    private void SetDefaultCloseOutDate(DateTime lastprocessperiod)
    {
        bool checkNullCloseOutDate = false;
        _objUser.ConnConfig = Session["config"].ToString();
        checkNullCloseOutDate = objBLUser.CheckNullCODt(_objUser);
        DataSet dsCODt = new DataSet();
        if (checkNullCloseOutDate)
        {
            txtCloseOutDate.Text = lastprocessperiod.AddMonths(1).ToString("MM/dd/yyyy");
        }
        else
        {
            dsCODt = objBLUser.GetCODt(_objUser);
            txtCloseOutDate.Text = dsCODt.Tables[0].Rows[0][0].ToString();
        }



    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Period closed out date
        _objUser.ConnConfig = Session["config"].ToString();
        DateTime lastCloseoutDate = Convert.ToDateTime(txtCloseOutDate.Text);
        DateTime _fStart = Convert.ToDateTime(txtStartDate.Text);
        DateTime _fEnd = Convert.ToDateTime(txtEndDate.Text);
        var str = "";
        var checkFStartAndCODate = DateTime.Compare(_fStart, lastCloseoutDate);
        var checkFStartAndFEnd = DateTime.Compare(_fStart, _fEnd);

        if (checkFStartAndCODate <= 0)
        {
            str = "Start Date must be later than the Close Out Date.";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        else if (checkFStartAndFEnd > 0)
        {
            str = "End Date must be later than the Start Date.";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        else
        {
            _objUser.FStart = Convert.ToDateTime(txtStartDate.Text);
            _objUser.FEnd = Convert.ToDateTime(txtEndDate.Text);
            UpdatePeriodCloseoutLog();
            objBLUser.UpdatePeriodClosedDate(_objUser);
            SetCloseOutDate();
            GetPeriodClosedDetails();
            
        }

        #endregion

        if (!(ddlRetainedGLAcct.SelectedValue.Equals(":: Select ::") && ddlCurrentGLAcct.SelectedValue.Equals(":: Select ::")))
        {
            AddTransaction();
        }
        RadGrid_gvLogs.Rebind();
        ResetFormControlValues(this);
        //Response.Redirect(Request.RawUrl);
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    

    private void GetPeriodClosedDetails()
    {
        try
        {
            _objUser.ConnConfig = Session["config"].ToString();
            _objUser.Username = Session["Username"].ToString();

            DataSet _dsPeriodClose = new DataSet();

            _dsPeriodClose = objBL_Report.GetPeriodClosedYear(_objUser);
            Session["PeriodClose"] = _dsPeriodClose.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    #endregion

    #region Custom Functions
 
    private void FillEarningGL()
    {
        _objChart.ConnConfig = Session["config"].ToString();
        DataSet _dsStock = _objBLChart.GetStock(_objChart);
        DataSet _dsCurrentEr = _objBLChart.GetCurrentEarn(_objChart);
        DataSet _dsDistri = _objBLChart.GetDistribution(_objChart);
        DataSet _dsRetainEr = _objBLChart.GetRetainedEarn(_objChart);

        ddlRetainedGLAcct.Items.Add(new ListItem(":: Select ::"));
        ddlCurrentGLAcct.Items.Add(new ListItem(":: Select ::"));
        if (_dsStock.Tables[0].Rows.Count > 0)
        {
            ddlRetainedGLAcct.Items.Add(new ListItem(_dsStock.Tables[0].Rows[0]["fDesc"].ToString(), _dsStock.Tables[0].Rows[0]["ID"].ToString()));
            ddlCurrentGLAcct.Items.Add(new ListItem(_dsStock.Tables[0].Rows[0]["fDesc"].ToString(), _dsStock.Tables[0].Rows[0]["ID"].ToString()));
        }
        if (_dsCurrentEr.Tables[0].Rows.Count > 0)
        {
            ddlRetainedGLAcct.Items.Add(new ListItem(_dsCurrentEr.Tables[0].Rows[0]["fDesc"].ToString(), _dsCurrentEr.Tables[0].Rows[0]["ID"].ToString()));
            ddlCurrentGLAcct.Items.Add(new ListItem(_dsCurrentEr.Tables[0].Rows[0]["fDesc"].ToString(), _dsCurrentEr.Tables[0].Rows[0]["ID"].ToString()));
        }
        if (_dsDistri.Tables[0].Rows.Count > 0)
        {
            ddlRetainedGLAcct.Items.Add(new ListItem(_dsDistri.Tables[0].Rows[0]["fDesc"].ToString(), _dsDistri.Tables[0].Rows[0]["ID"].ToString()));
            ddlCurrentGLAcct.Items.Add(new ListItem(_dsDistri.Tables[0].Rows[0]["fDesc"].ToString(), _dsDistri.Tables[0].Rows[0]["ID"].ToString()));
        }
        if (_dsRetainEr.Tables[0].Rows.Count > 0)
        {
            ddlRetainedGLAcct.Items.Add(new ListItem(_dsRetainEr.Tables[0].Rows[0]["fDesc"].ToString(), _dsRetainEr.Tables[0].Rows[0]["ID"].ToString()));
            ddlCurrentGLAcct.Items.Add(new ListItem(_dsRetainEr.Tables[0].Rows[0]["fDesc"].ToString(), _dsRetainEr.Tables[0].Rows[0]["ID"].ToString()));
        }
    }
    private double CalculateCurrentEarn()
    {
        double _currentExpense = 0.0, _totalRevenue = 0.00, _totalCostSale = 0.00, _totalExpense = 0.00;

        DateTime _selectDate = Convert.ToDateTime(txtCloseOutDate.Text);
        
        _objChart.ConnConfig = Session["config"].ToString();
        _objChart.EndDate = _selectDate;
        var year = _objChart.EndDate.Year;
        string strQuery = "select [dbo].[Control].[YE] from [dbo].[control]";
        int? yearEnd; int startMonth = 0; int financialYear;

        var dataSet = SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, strQuery);
        if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
        {
            yearEnd = int.Parse(dataSet.Tables[0].Rows[0][0].ToString());
            if (yearEnd < 11)
                startMonth = int.Parse(yearEnd.ToString()) + 2;
            else
                startMonth = 1;
        }
        financialYear = _objChart.EndDate.AddMonths(-12).Year;
        TimeSpan diffResult = _objChart.EndDate.Subtract(Convert.ToDateTime(startMonth + "/01/" + financialYear));
        if (diffResult.TotalDays > 365)
            financialYear = financialYear + 1;
        _objChart.StartDate = DateTime.Parse(startMonth + "/01/" + financialYear);
        DataSet _dsIncome = objBL_Report.GetIncomeStatementDetails(_objChart);
        string _netAmount = GetNetAmount(_dsIncome.Tables[0]).ToString();

        return double.Parse(_netAmount);
    }

    private double GetNetAmount(DataTable dt)
    {
        double netAmount = 0.00;
        try
        {
            if (dt.Rows.Count > 0)
            {
                double perRev = 0;
                double preCost = 0;
                double perExp = 0;
                double percent = 0;
                double NRev = 0;
                double revenueTotal = 0.00; double expenseTotal = 0.00;
                double costsaleTotal = 0.00;
                double grossProfit = 0.00;

                DataRow[] drI = dt.Select("Type = 3");
                if (drI.Count() > 0)
                {
                    revenueTotal = Convert.ToDouble(dt.Compute("SUM(Amount)", "Type = 3"));
                    if (revenueTotal != 0)
                    {
                        perRev = Math.Round((revenueTotal / revenueTotal) * 100 * 100) / 100;
                    }
                }

                drI = dt.Select("Type = 4");
                if (drI.Count() > 0)
                {
                    costsaleTotal = Convert.ToDouble(dt.Compute("SUM(Amount)", "Type = 4"));
                    if (revenueTotal != 0)
                    {
                        preCost = Math.Round((costsaleTotal / revenueTotal) * 100 * 100) / 100;
                    }
                }

                drI = dt.Select("Type = 5");
                if (drI.Count() > 0)
                {
                    expenseTotal = Convert.ToDouble(dt.Compute("SUM(Amount)", "Type = 5"));
                    if (revenueTotal != 0)
                    {
                        perExp = Math.Round((expenseTotal / revenueTotal) * 100 * 100) / 100;
                    }
                }
                NRev = revenueTotal;
                percent = (perRev - preCost) - perExp;
                grossProfit = revenueTotal - costsaleTotal;
                netAmount = grossProfit - expenseTotal;

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return netAmount;
    }


    private void AddTransaction() //Year end closed out transaction
    {
        double _currentEarnAmount = CalculateCurrentEarn();
        _objJournal.ConnConfig = Session["config"].ToString();

        _objJournal.Ref = _objBLJournal.GetMaxTransRef(_objJournal);
        _objJournal.BatchID = _objBLJournal.GetMaxTransBatch(_objJournal);
        _objJournal.GLDate = Convert.ToDateTime(txtCloseOutDate.Text);
        _objJournal.GLDesc = "Year-end Transfer";
        _objJournal.Internal = Convert.ToDateTime(txtCloseOutDate.Text).Year.ToString();
        _objBLJournal.AddGLA(_objJournal);

        for (int i = 0; i < 2; i++)
        {
            _objTrans = new Transaction();

            _objTrans.ConnConfig = Session["config"].ToString();
            _objTrans.BatchID = _objJournal.BatchID;
            _objTrans.Ref = _objJournal.Ref;
            _objTrans.TransDate = _objJournal.GLDate;
            _objTrans.Line = i;
            _objTrans.TransDescription = "Year-end Transfer";
            _objTrans.Type = 50;
            _objTrans.Sel = 0;
            if (_currentEarnAmount > 0)
            {
                if (i.Equals(0))     //Credit Retained Earning
                {
                    _objTrans.Acct = Convert.ToInt32(ddlRetainedGLAcct.SelectedValue);
                    _objTrans.Amount = _currentEarnAmount * -1;
                }
                else                //Debit Current Earning
                {
                    _objTrans.Acct = Convert.ToInt32(ddlCurrentGLAcct.SelectedValue);
                    _objTrans.Amount = _currentEarnAmount;
                }
            }
            else
            {
                if (i.Equals(0))     //Credit Current Earning
                {
                    _objTrans.Acct = Convert.ToInt32(ddlCurrentGLAcct.SelectedValue);
                    _objTrans.Amount = _currentEarnAmount;
                }
                else                //Debit Retained Earning
                {
                    _objTrans.Acct = Convert.ToInt32(ddlRetainedGLAcct.SelectedValue);
                    _objTrans.Amount = _currentEarnAmount * -1;
                }
            }
            _objBLJournal.AddJournalTrans(_objTrans);

            UpdateChartBalance();
        }

    }
    private void UpdateChartBalance()
    {
        _objChart.ConnConfig = Session["config"].ToString();
        _objChart.ID = _objTrans.Acct;
        _objChart.Amount = _objTrans.Amount;
        _objBLChart.UpdateChartBalance(_objChart);
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
                    //case "System.Web.UI.WebControls.TextBox":
                    //    ((TextBox)c).Text = "";
                    //    break;
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
    #endregion

    #region logs

    private void UpdatePeriodCloseoutLog()
    {
        _objUser.ConnConfig = Session["config"].ToString();
        _objUser.CODt = Convert.ToDateTime(txtCloseOutDate.Text.ToString());
        _objUser.YearEndClose = Convert.ToInt32(chkYearEndClose.Checked);
        if (!(ddlRetainedGLAcct.SelectedValue.Equals(":: Select ::")))
        {
            _objUser.RetainedGLAcct = ddlRetainedGLAcct.SelectedItem.Text;

        }
        else
            _objUser.RetainedGLAcct = null;
        if (!(ddlCurrentGLAcct.SelectedValue.Equals(":: Select ::")))
        {
            _objUser.CurrentGLAcct = ddlCurrentGLAcct.SelectedItem.Text;

        }
        else
            _objUser.CurrentGLAcct = null;
        _objUser.FStart = Convert.ToDateTime(txtStartDate.Text);
        _objUser.FEnd = Convert.ToDateTime(txtEndDate.Text); 
        _objUser.UserID = Convert.ToInt32(Session["UserID"]);
        _objUser.MOMUSer = Session["User"].ToString();
        objBLUser.AddPeriodCloseoutLogs(_objUser);
    }
    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        DataSet dsLog = new DataSet();
        _objUser.ConnConfig = Session["config"].ToString();
        dsLog = objBLUser.GetPeriodCloseoutLogs(_objUser);
        if (dsLog.Tables[0].Rows.Count > 0)
        {
            RadGrid_gvLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
            RadGrid_gvLogs.DataSource = dsLog.Tables[0];
        }
        else
        {
            RadGrid_gvLogs.DataSource = string.Empty;
        }
    }
    bool isGroupLog = false;
    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return RadGrid_gvLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            RadGrid_gvLogs.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_gvLogs_ItemCreated(object sender, GridItemEventArgs e)
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
    #endregion
}