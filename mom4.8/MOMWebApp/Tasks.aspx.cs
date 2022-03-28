using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using System.Web.Script.Serialization;
using Telerik.Web.UI;

public partial class Tasks : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

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
            //fillDates();
            #region Show Selected Filter
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["fromDate"] == null && Session["ToDate"] == null)
                {
                    txtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtTo.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                }
                else
                {
                    txtFrom.Text = Session["fromDate"].ToString();
                    txtTo.Text = Session["ToDate"].ToString();
                }

                if (Session["ShowAllTask"] != null)
                {
                    chkAllTask.Checked = Convert.ToBoolean(Session["ShowAllTask"]);
                }

                if (Session["ShowAllUsers"] != null)
                {
                    chkAllUsers.Checked = Convert.ToBoolean(Session["ShowAllUsers"]);
                }

                if (Session["ddlSearch_Task"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearch_Task"]);
                    ddlSearch.SelectedValue = selectedValue;

                    String searchValue = Convert.ToString(Session["ddlSearch_Value_Task"]);
                    if (selectedValue == "t.fuser")
                    {
                        ddlUser.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "days")
                    {
                        ddlcompare.SelectedValue = searchValue;
                        txtSearch.Text = Convert.ToString(Session["ddlSearch_Value_Task_Day"]);
                    }
                    else if (selectedValue == "status")
                    {
                        ddlStatus.SelectedValue = searchValue;
                    }
                    else
                    {
                        txtSearch.Text = searchValue;
                    }

                    ShowHideFilter();
                }
            }
            else
            {
                Session["fromDate"] = null;
                Session["ToDate"] = null;
                Session["ddlSearch_Task"] = null;
                Session["ddlSearch_Value_Task"] = null;
                Session["ddlSearch_Value_Task_Day"] = null;
                Session["ShowAllTask"] = null;
                Session["ShowAllUsers"] = null;

                txtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtTo.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            #endregion


            FillUsers();
            FillTasks();
            UserPermission();
            //Permission();
            HighlightSideMenu();
        }

        CompanyPermission();
        ConvertToJSON();
    }

    private void HighlightSideMenu()
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl("SalesMgr");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkTasks");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");

        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        div.Style.Add("display", "block");
    }

    private void fillDates()
    {
        DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
        DateTime lastDay = firstDay.AddDays(DaysinMonth);
        txtFrom.Text = firstDay.ToShortDateString();
        txtTo.Text = lastDay.ToShortDateString();
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgr");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.Master.FindControl("lnkTasks");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        HtmlGenericControl ul = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgrSub");
       
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            lnkDelete.Visible = false;
        }

        if (Session["type"].ToString() != "am")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];
            string Sales = dt.Rows[0]["sales"].ToString().Substring(0, 1);

            if (Sales == "N")
            {
                Response.Redirect("home.aspx");
            }
        }
    }

    private void UserPermission()
    {
        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            string SalesManagermodulePermission = ds.Rows[0]["SalesManager"] == DBNull.Value ? "Y" : ds.Rows[0]["SalesManager"].ToString();

            if (SalesManagermodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            int taskPermission = ds.Rows[0]["ToDo"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Rows[0]["ToDo"]);

            if (taskPermission == 0)
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
            RadGrid_Task.Columns[9].Visible = true;
        }
        else
        {
            RadGrid_Task.Columns[9].Visible = false;
        }
    }

    private void FillTasks()
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = ddlSearch.SelectedValue;
        if (ddlSearch.SelectedValue == "r.name" || ddlSearch.SelectedValue == "t.subject" || ddlSearch.SelectedValue == "t.remarks" || ddlSearch.SelectedValue == "t.result")
        {
            objProp_Customer.SearchValue = txtSearch.Text.Trim().Replace("'", "''");
        }
        else if (ddlSearch.SelectedValue == "t.fuser")
        {
            objProp_Customer.SearchValue = ddlUser.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "status")
        {
            objProp_Customer.SearchValue = ddlStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "days")
        {
            string strDays = string.Empty;

            if (ddlcompare.SelectedValue == "0")
                strDays = " = '" + txtSearch.Text.Trim();
            else if (ddlcompare.SelectedValue == "1")
                strDays = " >= '" + txtSearch.Text.Trim();
            else if (ddlcompare.SelectedValue == "2")
                strDays = " <= '" + txtSearch.Text.Trim();
            else if (ddlcompare.SelectedValue == "3")
                strDays = " > '" + txtSearch.Text.Trim();
            else if (ddlcompare.SelectedValue == "4")
                strDays = " < '" + txtSearch.Text.Trim();
            else
                strDays = " = '" + txtSearch.Text.Trim();

            objProp_Customer.SearchValue = strDays;
        }
        else
        {
            objProp_Customer.SearchValue = string.Empty;
        }

        objProp_Customer.StartDate = string.Empty;
        if (txtFrom.Text.Trim() != string.Empty)
        {
            DateTime dtst = new DateTime();
            if (DateTime.TryParse(txtFrom.Text.Trim(), out dtst))
            {
                objProp_Customer.StartDate = txtFrom.Text.Trim();
            }
        }

        objProp_Customer.EndDate = string.Empty;
        if (txtTo.Text.Trim() != string.Empty)
        {
            DateTime dtst = new DateTime();
            if (DateTime.TryParse(txtTo.Text.Trim(), out dtst))
            {
                objProp_Customer.EndDate = txtTo.Text.Trim();
            }
        }

        int mode = Convert.ToInt16(chkAllTask.Checked);
        if (mode == 1)
        {
            mode = 5;
        }
        else if (mode == 0)
        {
            mode = 1;
        }

        objProp_Customer.Mode = mode;
        objProp_Customer.Screen = "";
        objProp_Customer.Ref = 0;

        #region Company Check

        if (chkAllUsers.Checked)
        {
            objProp_Customer.UserID = 0;
        }
        else
        {
            objProp_Customer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        }
        
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objProp_Customer.EN = 1;
        }
        else
        {
            objProp_Customer.EN = 0;
        }

        #endregion

        ds = objBL_Customer.getTasks(objProp_Customer, new GeneralFunctions().GetSalesAsigned());
        Session["tasks"] = ds.Tables[0];
        RadGrid_Task.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Task.DataSource = ds.Tables[0];
        lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString() + " Record(s) found";
    }

    private void FillRecent()
    {
        //NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
        //masterSalesMaster.FillRecentProspect();        
    }

    private void FillUsers()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.getTaskUsers(objPropUser);

        ddlUser.DataSource = ds.Tables[0];
        ddlUser.DataTextField = "fuser";
        ddlUser.DataValueField = "username";
        ddlUser.DataBind();

        ddlUser.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowHideFilter();
    }

    private void ShowHideFilter()
    {
        if (ddlSearch.SelectedValue == "r.name" || ddlSearch.SelectedValue == "t.subject" || ddlSearch.SelectedValue == "t.remarks")
        {
            txtSearch.Visible = true;
            ddlcompare.Visible = false;
            ddlUser.Visible = false;
            ddlStatus.Visible = false;
        }
        else if (ddlSearch.SelectedValue == "t.fuser")
        {
            txtSearch.Visible = false;
            ddlcompare.Visible = false;
            ddlUser.Visible = true;
            ddlStatus.Visible = false;
        }
        else if (ddlSearch.SelectedValue == "days")
        {
            txtSearch.Visible = true;
            ddlcompare.Visible = true;
            ddlUser.Visible = false;
            ddlStatus.Visible = false;
        }
        else if (ddlSearch.SelectedValue == "status")
        {
            txtSearch.Visible = false;
            ddlcompare.Visible = false;
            ddlUser.Visible = false;
            ddlStatus.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
            ddlcompare.Visible = false;
            ddlUser.Visible = false;
            ddlStatus.Visible = false;
        }
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        Session["fromDate"] = txtFrom.Text;
        Session["ToDate"] = txtTo.Text;
        Session["ShowAllTask"] = chkAllTask.Checked;
        Session["ShowAllUsers"] = chkAllUsers.Checked;

        #region Search Filter
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_Task"] = selectedValue;

        if (selectedValue == "t.fuser")
        {
            Session["ddlSearch_Value_Task"] = ddlUser.SelectedValue;
        }
        else if (selectedValue == "days")
        {
            Session["ddlSearch_Value_Task"] = ddlcompare.SelectedValue;
            Session["ddlSearch_Value_Task_Day"] = txtSearch.Text;
        }
        else if (selectedValue == "status")
        {
            Session["ddlSearch_Value_Task"] = ddlStatus.SelectedValue;
        }
        else
        {
            Session["ddlSearch_Value_Task"] = txtSearch.Text;
        }
        #endregion

        if (hdnCssActive.Value == "CssActive")
        {
            Session["lblTaskActive"] = "1";
        }
        else
        {
            Session["lblTaskActive"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }

        FillTasks();
        FillRecent();
        RadGrid_Task.Rebind();
    }

    protected void lnkTaskSummaryReport_Click(object sender, EventArgs e)
    {
        var searchBy = ddlSearch.SelectedValue;
        var searchValue = string.Empty;

        if (ddlSearch.SelectedValue == "r.name" || ddlSearch.SelectedValue == "t.subject" || ddlSearch.SelectedValue == "t.remarks" || ddlSearch.SelectedValue == "t.result")
        {
            searchValue = txtSearch.Text.Trim().Replace("'", "''");
        }
        else if (ddlSearch.SelectedValue == "t.fuser")
        {
            searchValue = ddlUser.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "status")
        {
            searchValue = ddlStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "days")
        {
            string strDays = string.Empty;

            if (ddlcompare.SelectedValue == "0")
                strDays = " = '" + txtSearch.Text.Trim();
            else if (ddlcompare.SelectedValue == "1")
                strDays = " >= '" + txtSearch.Text.Trim();
            else if (ddlcompare.SelectedValue == "2")
                strDays = " <= '" + txtSearch.Text.Trim();
            else if (ddlcompare.SelectedValue == "3")
                strDays = " > '" + txtSearch.Text.Trim();
            else if (ddlcompare.SelectedValue == "4")
                strDays = " < '" + txtSearch.Text.Trim();
            else
                strDays = " = '" + txtSearch.Text.Trim();

            searchValue = strDays;
        }

        var startDate = string.Empty;
        if (txtFrom.Text.Trim() != string.Empty)
        {
            DateTime dtst = new DateTime();
            if (DateTime.TryParse(txtFrom.Text.Trim(), out dtst))
            {
                startDate = txtFrom.Text.Trim();
            }
        }

        var endDate = string.Empty;
        if (txtTo.Text.Trim() != string.Empty)
        {
            DateTime dtst = new DateTime();
            if (DateTime.TryParse(txtTo.Text.Trim(), out dtst))
            {
                endDate = txtTo.Text.Trim();
            }
        }

        int mode = Convert.ToInt16(chkAllTask.Checked);
        if (mode == 1)
        {
            mode = 5;
        }
        else if (mode == 0)
        {
            mode = 1;
        }

       string urlString = "TaskSummaryReport.aspx?sd=" + startDate + "&ed=" + endDate + "&stype=" + searchBy + "&stext=" + searchValue + "&mode=" + mode + "&chkAllTask=" + chkAllTask.Checked + "&chkAllUsers=" + chkAllUsers.Checked;

       Response.Redirect(urlString, true);
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        objGeneralFunctions.ResetFormControlValues(this);
        ddlSearch_SelectedIndexChanged(sender, e);
        FillTasks();
        FillRecent();
        RadGrid_Task.Rebind();
    }

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("addtask.aspx");
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            var counDeleted = 0;
            foreach (GridDataItem item in RadGrid_Task.SelectedItems)
            {
                Label lblID = (Label)item.FindControl("lblId");
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.TaskID = Convert.ToInt32(lblID.Text);
                objBL_Customer.DeleteTask(objProp_Customer);
                counDeleted++;
                FillTasks();
                FillRecent();
            }

            if (counDeleted > 0)
            {
                RadGrid_Task.Rebind();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Task deleted successfully',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);//ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Task.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            Response.Redirect("addtask.aspx?uid=" + lblID.Text);
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void lnkFollowup_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Task.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            Response.Redirect("addtask.aspx?fl=1&uid=" + lblID.Text);
        }
    }

    protected void chkAllTask_CheckedChanged(object sender, EventArgs e)
    {
        FillTasks();
        RadGrid_Task.Rebind();
    }

    protected void chkAllUsers_CheckedChanged(object sender, EventArgs e)
    {
        FillTasks();
        RadGrid_Task.Rebind();
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
            objPropUser.Type = "Task";
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

    protected void RadGrid_Task_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Task.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Task_FilterExpression"] != null && Convert.ToString(Session["Task_FilterExpression"]) != "" && Session["Task_Filters"] != null)
                {
                    RadGrid_Task.MasterTableView.FilterExpression = Convert.ToString(Session["Task_FilterExpression"]);
                    var filtersGet = Session["Task_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_Task.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["Task_FilterExpression"] = null;
                Session["Task_Filters"] = null;
            }
        }
        #endregion
        FillTasks();
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Task.MasterTableView.FilterExpression != "" ||
            (RadGrid_Task.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Task.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Task_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter

        String filterExpression = Convert.ToString(RadGrid_Task.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Task_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Task.MasterTableView.OwnerGrid.Columns)
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

            Session["Task_Filters"] = filters;
        }
        else
        {
            Session["Task_FilterExpression"] = null;
            Session["Task_Filters"] = null;
        }

        #endregion  

        foreach (GridDataItem gr in RadGrid_Task.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            gr.Attributes["ondblclick"] = "location.href='addtask.aspx?uid=" + lblID.Text + "'";
        }
    }

    protected void RadGrid_Task_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (Convert.ToString(RadGrid_Task.MasterTableView.FilterExpression) != "")
                    lblRecordCount.Text = totalCount + " Record(s) found";
                else
                    lblRecordCount.Text = RadGrid_Task.VirtualItemCount + " Record(s) found";
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
}