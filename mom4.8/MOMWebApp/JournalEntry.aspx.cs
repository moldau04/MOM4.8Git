using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Web.Script.Serialization;
using Telerik.Web.UI;

public partial class JournalEntry : System.Web.UI.Page
{
    Journal _objJe = new Journal();
    BL_JournalEntry _objBLJe = new BL_JournalEntry();

    BL_GLARecur _objBLRecurr = new BL_GLARecur();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();

    User objPropUser = new User();
    BL_User _objBLUser = new BL_User();
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

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

                // pair radio button with label for add date Increment tab
                List<Tuple<RadioButton, HtmlGenericControl>> listRadio = new List<Tuple<RadioButton, HtmlGenericControl>>();
                if (Request.QueryString["r"] != null)
                {
                    if (Request.QueryString["r"].ToString() == "1")
                    {
                        DataSet _dsMinDate = new DataSet();
                        _objJe.ConnConfig = Session["config"].ToString();
                        _dsMinDate = _objBLRecurr.GetMinRecurDate(_objJe);
                        if (_dsMinDate.Tables[0].Rows[0]["MinDate"] != null)
                        {
                            if (_dsMinDate.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(_dsMinDate.Tables[0].Rows[0]["MinDate"].ToString()))
                                {
                                    var sdate = Convert.ToDateTime(_dsMinDate.Tables[0].Rows[0]["MinDate"]).ToShortDateString();
                                    txtFromDate.Text = sdate;
                                }
                            }
                        }
                        txtToDate.Text = DateTime.Now.ToShortDateString();
                        rdoRecurring.Checked = true;
                    }
                }
                else
                {
                    lnkProcess.Visible = false;
                    rdoJournal.Checked = true;
                }

                #region Show selected filter
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["JEStartDate"] != null  && !string.IsNullOrEmpty(Session["JEStartDate"].ToString()) 
                        && Session["JEEndDate"] != null && !string.IsNullOrEmpty(Session["JEEndDate"].ToString()))
                    {
                        txtFromDate.Text = Convert.ToDateTime(Session["JEStartDate"].ToString()).ToShortDateString();
                        txtToDate.Text = Convert.ToDateTime(Session["JEEndDate"].ToString()).ToShortDateString();
                        if (Session["Journal_Type"] == null || Session["Journal_Type"].ToString() == "1")
                        {
                            rdoJournal.Checked = true;
                            rdoRecurring.Checked = false;
                            lnkJEDetailedReport.Visible = true;
                        }
                        else
                        {
                            rdoJournal.Checked = false;
                            rdoRecurring.Checked = true;
                            rdoRecurring_CheckedChanged(sender, e);
                            lnkJEDetailedReport.Visible = false;
                        }

                        Session.Remove("JEStartDate");
                        Session.Remove("JEEndDate");
                    }
                    else
                    {
                        txtFromDate.Text = DateTime.Now.AddDays(-1 * (int)(DateTime.Now.DayOfWeek)).ToShortDateString();
                        txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Saturday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                    }
                }
                else
                {
                    var now = System.DateTime.Now;
                    var FisrtDay = now.AddDays(-1 * (int)(DateTime.Now.DayOfWeek));
                    txtFromDate.Text = FisrtDay.ToShortDateString();
                    var LastDay = FisrtDay.AddDays(7).AddSeconds(-1);
                    txtToDate.Text = LastDay.ToShortDateString();
                    hdnJournalSelectDtRange.Value = "Week";

                    Session.Remove("JEStartDate");
                    Session.Remove("JEEndDate");
                    Session.Remove("Journal_Type");
                    Session.Remove("Journal_FilterExpression");
                    Session.Remove("Journal_Filters");
                }

                #endregion

                Permission();
                ConvertToJSON();
                HighlightSideMenu("financeMgr", "lnkJournalEntry", "financeMgrSub");
            }

            ConvertToJSON();
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

        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            string JournalEntry = ds.Rows[0]["GLAdj"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["GLAdj"].ToString();

            string Add = JournalEntry.Length < 1 ? "Y" : JournalEntry.Substring(0, 1);
            string Edit = JournalEntry.Length < 2 ? "Y" : JournalEntry.Substring(1, 1);
            string Delete = JournalEntry.Length < 3 ? "Y" : JournalEntry.Substring(2, 1);
            string View = JournalEntry.Length < 4 ? "Y" : JournalEntry.Substring(3, 1);

            if (Add == "N")
            {
                lnkAddnew.Visible = false;
            }
            if (Edit == "N")
            {
                btnEdit.Visible = false;
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

    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = _objBLUser.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addjournalentry.aspx");
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            bool Flag = false;
            foreach (GridDataItem gr in RadGrid_JournalEntry.SelectedItems)
            {
                Label lblID = (Label)gr.FindControl("lblId");
                Label lblfDate = (Label)gr.FindControl("lblfDate");
                Label lblRef = (Label)gr.FindControl("lblRef");
                Label lblBatch = (Label)gr.FindControl("lblBatch");
                Label lblCleared = (Label)gr.FindControl("lblCleared");

                _objJe.ConnConfig = Session["config"].ToString();
                _objJe.Ref = Convert.ToInt32(lblID.Text);

                _objJe.fDate = Convert.ToDateTime(lblfDate.Text);
                _objJe.BatchID = Convert.ToInt32(lblBatch.Text);
                _objJe.IsCleared = Convert.ToInt16(lblCleared.Text);
                _objJe.fDate = Convert.ToDateTime(lblfDate.Text);
                Flag = CommonHelper.GetPeriodDetails(_objJe.fDate);

                if (Flag)
                {
                    if (rdoRecurring.Checked == true)
                    {
                        _objBLRecurr.DeleteGLARecur(_objJe);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Recurring JE deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        Button mpbtnNotifyRecur = this.Master.FindControl("btnNotifyRecur") as Button;
                        if (mpbtnNotifyRecur != null)
                        {
                            DataSet _dsRecurrCount = new DataSet();
                            _objJe.ConnConfig = Session["config"].ToString();
                            _dsRecurrCount = _objBLRecurr.GetProcessRecurrCount(_objJe);
                            if (_dsRecurrCount != null)
                            {
                                int _recurCount = Convert.ToInt32(_dsRecurrCount.Tables[0].Rows[0]["CountRecur"]);
                                mpbtnNotifyRecur.Text = _recurCount.ToString();
                            }
                        }
                        break;
                    }
                    else if (rdoJournal.Checked == true)
                    {
                        if (_objJe.IsCleared != 1)
                        {
                            //Delete data from GLA table
                            _objBLJe.DeleteGLA(_objJe);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'JE deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "closedMesg();", true);
                        }
                        break;
                    }
                }
            }

            RadGrid_JournalEntry.Rebind();

            if (!Flag)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "displayDeleteAlert();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            //bool _editFinance = (bool)Session["EditFinance"];
            foreach (GridDataItem di in RadGrid_JournalEntry.SelectedItems)
            {
                Label lblID = (Label)di.FindControl("lblId");
                if (rdoJournal.Checked == true)
                {
                    Response.Redirect("addjournalentry.aspx?id=" + lblID.Text + "&frm=MNG");
                }
                else if (rdoRecurring.Checked == true)
                {
                    Response.Redirect("addjournalentry.aspx?id=" + lblID.Text + "&r=1&frm=MNG");
                }
            }
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
            foreach (GridDataItem gr in RadGrid_JournalEntry.SelectedItems)
            {
                Label lblID = (Label)gr.FindControl("lblId");
                _objJe.ConnConfig = Session["config"].ToString();
                _objJe.Ref = Convert.ToInt32(lblID.Text);
                RecurCount = _objBLJournal.ProcessRecurJE(_objJe);
            }

            Button mpbtnNotifyRecur = this.Master.FindControl("btnNotifyRecur") as Button;
            if (mpbtnNotifyRecur != null)
            {
                mpbtnNotifyRecur.Text = RecurCount.ToString();
            }
            
            RadGrid_JournalEntry.Rebind();
            ScriptManager.RegisterStartupScript(this, GetType(), "keySuccUp", "noty({text: 'Successfully entry processed!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnReverse_Click(object sender, EventArgs e)
    {
        try
        {
            if (RadGrid_JournalEntry.SelectedItems.Count > 0)
            {
                foreach (GridDataItem row in RadGrid_JournalEntry.SelectedItems)
                {
                    Label lblID = (Label)row.FindControl("lblId");
                    string urlString = "AddJournalEntry.aspx?rid=" + lblID.Text;

                    if (rdoRecurring.Checked)
                    {
                        urlString += "&r=1";
                    }

                    Response.Redirect(urlString);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keywarning", "noty({text: 'Please select a Journal Entry',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        if (IsValidDate())
        {
            RadGrid_JournalEntry.Rebind();

            ConvertToJSON();

            Session["JEStartDate"] = txtFromDate.Text;
            Session["JEEndDate"] = txtToDate.Text;

            String filterExpression = Convert.ToString(RadGrid_JournalEntry.MasterTableView.FilterExpression);
            if (filterExpression != "")
            {
                Session["Journal_FilterExpression"] = filterExpression;
                List<RetainFilter> filters = new List<RetainFilter>();

                foreach (GridColumn column in RadGrid_JournalEntry.MasterTableView.OwnerGrid.Columns)
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

                Session["Journal_Filters"] = filters;
            }
        }
        else
        {
            RadGrid_JournalEntry.DataSource = new string[] { };
            RadGrid_JournalEntry.DataBind();
        }

    }

    protected void rdoJournal_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdoJournal.Checked == true && IsValidDate())
            {
                Session["Journal_Type"] = "1";
                RadGrid_JournalEntry.Rebind();
                lnkProcess.Visible = false;
                lnkJEDetailedReport.Visible = true;
            }
            else
            {
                Session["Journal_Type"] = "2";
                RadGrid_JournalEntry.DataSource = new string[] { };
                RadGrid_JournalEntry.DataBind();
                lnkJEDetailedReport.Visible = false;
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
            if (rdoRecurring.Checked == true && IsValidDate())
            {
                Session["Journal_Type"] = "2";
                RadGrid_JournalEntry.Rebind();
                lnkProcess.Visible = true;
                lnkJEDetailedReport.Visible = false;
            }
            else
            {
                Session["Journal_Type"] = "1";
                RadGrid_JournalEntry.DataSource = new string[] { };
                RadGrid_JournalEntry.DataBind();
                lnkJEDetailedReport.Visible = true;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void BindJournalGrid()
    {
        try
        {
            if (IsValidDate())
            {
                string stdate = txtFromDate.Text + " 00:00:00";
                string enddate = txtToDate.Text + " 23:59:59";
                _objJe.StartDate = Convert.ToDateTime(stdate);
                _objJe.EndDate = Convert.ToDateTime(enddate);

                Session["JEStartDate"] = _objJe.StartDate;
                Session["JEEndDate"] = _objJe.EndDate;

                DataSet _dsJe = new DataSet();
                _objJe.ConnConfig = Session["config"].ToString();
                _dsJe = _objBLJe.GetAllJEByDate(_objJe);
                ViewState["ds_Journal"] = _dsJe;
                ViewState["Journal"] = _dsJe.Tables[0];
                Session["Journal"] = _dsJe.Tables[0];
                RadGrid_JournalEntry.DataSource = _dsJe;
                RadGrid_JournalEntry.VirtualItemCount = _dsJe.Tables[0].Rows.Count;
                RadPersistenceJournalEntry.SaveState();
                updpnl.Update();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Please enter valid start date and end date.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void BindRecurringGrid()
    {
        try
        {
            if (IsValidDate())
            {
                DataSet _dsRecurr = new DataSet();
                string stdate = txtFromDate.Text + " 00:00:00";
                string enddate = txtToDate.Text + " 23:59:59";
                _objJe.StartDate = Convert.ToDateTime(stdate);
                _objJe.EndDate = Convert.ToDateTime(enddate);

                _objJe.ConnConfig = Session["config"].ToString();
                _dsRecurr = _objBLRecurr.GetProcessTransByDate(_objJe);

                ViewState["ds_RecJournal"] = _dsRecurr;
                ViewState["Journal"] = _dsRecurr.Tables[0];
                Session["Journal"] = _dsRecurr.Tables[0];
                RadGrid_JournalEntry.DataSource = _dsRecurr.Tables[0];
                updpnl.Update();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private bool IsValidDate()
    {
        DateTime dateValue;
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
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Please enter valid start date and end date.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
            return false;
        }
    }

    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem di in RadGrid_JournalEntry.SelectedItems)
            {
                Label lblID = (Label)di.FindControl("lblId");
                if (rdoRecurring.Checked)
                {
                    Response.Redirect("addjournalentry.aspx?id=" + lblID.Text + "&c=2");
                }
                else
                {
                    Response.Redirect("addjournalentry.aspx?id=" + lblID.Text + "&c=1");
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void RemoveActiveLabel()
    {
        if (hdnCssActive.Value == "CssActive")
        {
            Session["lblActive"] = "1";
        }
        else
        {
            Session["lblActive"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }
    }

    private List<CustomerReport> GetReportsName()
    {
        List<CustomerReport> lstCustomerReport = new List<CustomerReport>();
        try
        {
            DataSet dsGetReports = new DataSet();
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objPropUser.Type = "Journal";
            dsGetReports = objBL_ReportsData.GetStockReports(objPropUser);
           
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

    protected void RadGrid_JournalEntry_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Journal_FilterExpression"] != null && Convert.ToString(Session["Journal_FilterExpression"]) != "" && Session["Journal_Filters"] != null)
                {
                    RadGrid_JournalEntry.MasterTableView.FilterExpression = Convert.ToString(Session["Journal_FilterExpression"]);
                    var filtersGet = Session["Journal_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_JournalEntry.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }

                    Session.Remove("Journal_FilterExpression");
                    Session.Remove("Journal_Filters");
                }
            }
            else
            {
                Session.Remove("Journal_FilterExpression");
                Session.Remove("Journal_Filters");
            }
        }
        #endregion

        if (rdoJournal.Checked)
        {
            Session["Journal_Type"] = "1";
            BindJournalGrid();
            lblTitle.Text = "Journal Entries";
            udpTitle.Update();
        }
        else
        {
            Session["Journal_Type"] = "2";
            BindRecurringGrid();
            lblTitle.Text = "Recurring Adjustment";
            udpTitle.Update();
        }
    }

    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_JournalEntry.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            HyperLink lnkId = (HyperLink)gr.FindControl("lnkId");

            if (rdoJournal.Checked == true)
            {
                lnkId.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addjournalentry.aspx?id=" + lblID.Text + "&frm=MNG'";
            }
            else if (rdoRecurring.Checked == true)
            {
                lnkId.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addjournalentry.aspx?id=" + lblID.Text + "&r=1&frm=MNG'";
            }
        }
    }

    protected void RadGrid_JournalEntry_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_JournalEntry.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Journal_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_JournalEntry.MasterTableView.OwnerGrid.Columns)
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

            Session["Journal_Filters"] = filters;
        }
        else
        {
            Session.Remove("Journal_FilterExpression");
            Session.Remove("Journal_Filters");
        }
        #endregion

        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_JournalEntry);
        RowSelect();
    }

    protected void RadGrid_JournalEntry_ItemCreated(object sender, GridItemEventArgs e)
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

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_JournalEntry.MasterTableView.GetColumn("chkSelect").Visible = false;
        RadGrid_JournalEntry.ExportSettings.FileName = "JournalEntry";
        RadGrid_JournalEntry.ExportSettings.IgnorePaging = true;
        RadGrid_JournalEntry.ExportSettings.ExportOnlyData = true;
        RadGrid_JournalEntry.ExportSettings.OpenInNewWindow = true;
        RadGrid_JournalEntry.ExportSettings.HideStructureColumns = true;
        RadGrid_JournalEntry.MasterTableView.UseAllDataFields = true;
        RadGrid_JournalEntry.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_JournalEntry.MasterTableView.ExportToExcel();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Session.Remove("Journal_FilterExpression");
        Session.Remove("Journal_Filters");

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "switchGrid", "switchGrid();", true);
        foreach (GridColumn column in RadGrid_JournalEntry.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }

        RadGrid_JournalEntry.MasterTableView.FilterExpression = string.Empty;
        RadGrid_JournalEntry.Rebind();
    }

    protected void RadGrid_JournalEntry_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        updpnl.Update();

    }

    protected void lnkJEDetailedReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            string urlString = "JournalEntryDetailedReport.aspx?sd=" + txtFromDate.Text + "&ed=" + txtToDate.Text;

            String filterExpression = Convert.ToString(RadGrid_JournalEntry.MasterTableView.FilterExpression);
            if (filterExpression != "")
            {
                Session["Journal_FilterExpression"] = filterExpression;
                List<RetainFilter> filters = new List<RetainFilter>();

                foreach (GridColumn column in RadGrid_JournalEntry.MasterTableView.OwnerGrid.Columns)
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

                Session["Journal_Filters"] = filters;
            }

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
}