using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
public partial class Project : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();

    User objProp_User = new User();

    BL_User objBL_User = new BL_User();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

    JobT objProp_job = new JobT();

    BL_Customer objBL_Customer = new BL_Customer();

    BL_Job objBL_Project = new BL_Job();

    BL_Report objBL_Report = new BL_Report();

    public static bool isCheck = false;

    public static bool isExportCustomLabel = false;

    public static bool hasNoRow = false;

    private bool IsGridPageIndexChanged = false;

    private void Fill_BT()
    {
        Customer objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getBT(objCustomer);

        string BT = "Business Type";

        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            BT = ds.Tables[0].Rows[0]["Label"].ToString();

        }
        RadGrid_Project.Columns.FindByDataField("BuildingType").HeaderText = BT;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        DefineGridStructure();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            Fill_BT();

            if (!Page.IsPostBack)
            {

                ViewState["defaultPageLoad"] = "1";

                int CurrentPageSize = 50; int CurrentPageIndex = 0;

                ViewState["CurrentPageSize"] = CurrentPageSize;

                ViewState["CurrentPageIndex"] = CurrentPageIndex;

                ViewState["isReCalculateLaborExpense"] = false;

                if (Session["txtfrmDtValforEditjob"] != null && Session["txttoDtValforEditJob"] != null && Session["ddlDateRangeFieldforEditJob"] != null)
                {
                    if (Session["ddlDateRangeFieldforEditJob"].ToString() == "2" || Session["ddlDateRangeFieldforEditJob"].ToString() == "5")
                    {
                        txtfromDate.Text = Session["txtfrmDtValforEditjob"].ToString();
                        txtToDate.Text = Session["txttoDtValforEditJob"].ToString();
                        ddlDateRange.SelectedValue = Session["ddlDateRangeFieldforEditJob"].ToString();

                    }

                }


            }


            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (Session["type"].ToString() == "am" && (ddlDateRange.SelectedValue == "2" || ddlDateRange.SelectedValue == "5"))
            {
                lnkLaborExpenses.Visible = true;
            }
            else
            {
                lnkLaborExpenses.Visible = Convert.ToBoolean(ViewState["isReCalculateLaborExpense"]);
            }


            if (ddlDateRange.SelectedValue == "2" || ddlDateRange.SelectedValue == "3" || ddlDateRange.SelectedValue == "4" || ddlDateRange.SelectedValue == "5")
            {
                txtfromDate.Visible = true;
                txtToDate.Visible = true;
            }

            if (ddlDateRange.SelectedValue == "1")
            {
                txtfromDate.Visible = false;
                txtToDate.Visible = false;

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

                hdnsessiontype.Value = Session["type"].ToString();

                if (Convert.ToString(Request.QueryString["f"]) == "c")
                {
                    // Refresh sessions
                    Session["PROJ_OtherFilterExpression"] = null;
                    Session["PROJ_OtherFilters"] = null;
                    Session["PROJ_DepartmentFilters"] = null;
                    Session["PROJ_RouteFilters"] = null;
                    Session["PROJ_StageFilters"] = null;

                }

                //InitDataForFiltersMenu();

                BindProjectDepartments();
                Permission();
                ConvertToJSON();
                isExportCustomLabel = false;
                lnkChk.Checked = false;
                hasNoRow = false;
                isCheck = false;
                setValueFitler();
                FillSearchFilter();
                UpdateControlValues();
            }

            if (lnkChk.Checked == false)
            {
                isCheck = false;
            }

            string[] tokens = Session["config"].ToString().Split(';');

            if ((tokens[1].ToString().ToLower().IndexOf("ahei") != -1) || (tokens[1].ToString().ToLower().IndexOf("ahes") != -1) || ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("accredited"))
            {
                lnkDelivered.Visible = true;
                lnkInspected.Visible = true;
                lnkDrawing.Visible = true;
                lnkReDrawing.Visible = true;
                lnkDcaApproval.Visible = true;
                lnkApprovedByDca.Visible = true;
                lnkUnitInspectedReport.Visible = true;
                lnkUnitFinishedReport.Visible = true;
                lnkSubstantialCompleteDeliveryNotPaidReport.Visible = true;
                lnkSubstantialCompleteFinalNotPaidReport.Visible = true;
                lnkOutofWarrantyreport.Visible = true;
                lnkOpenJob.Visible = true;
            }
            else
            {
                lnkDelivered.Visible = false;
                lnkInspected.Visible = false;
                lnkDrawing.Visible = false;
                lnkReDrawing.Visible = false;
                lnkDcaApproval.Visible = false;
                lnkApprovedByDca.Visible = false;
                lnkUnitInspectedReport.Visible = false;
                lnkUnitFinishedReport.Visible = false;
                lnkSubstantialCompleteDeliveryNotPaidReport.Visible = false;
                lnkSubstantialCompleteFinalNotPaidReport.Visible = false;
                lnkOutofWarrantyreport.Visible = false;
                lnkOpenJob.Visible = false;
            }

            if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("gable") || ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("VRS"))
            {
                lnkProjectActualVsBudgetReportGable.Visible = true;
            }

            CompanyPermission();
            HighlightSideMenu("ProjectMgr", "lnkProject", "ProjectMgrSub");
        }
        catch { }
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

        DataTable ds = new DataTable();
        ds = GetUserById();

        if (Session["type"].ToString() != "am")
        {
            //Job
            string jobPermission = ds.Rows[0]["job"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["job"].ToString();
            string stAddejob = jobPermission.Length < 1 ? "Y" : jobPermission.Substring(0, 1);
            string stEditejob = jobPermission.Length < 2 ? "Y" : jobPermission.Substring(1, 1);

            hdnAddeJob.Value = jobPermission.Length < 1 ? "Y" : jobPermission.Substring(0, 1);
            hdnEditeJob.Value = jobPermission.Length < 2 ? "Y" : jobPermission.Substring(1, 1);
            hdnDeleteJob.Value = jobPermission.Length < 3 ? "Y" : jobPermission.Substring(2, 1);
            hdnViewJob.Value = jobPermission.Length < 4 ? "Y" : jobPermission.Substring(3, 1);

            if (hdnViewJob.Value == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            //Project List
            string ProjectList = ds.Rows[0]["ProjectListPermission"] == DBNull.Value ? "Y" : ds.Rows[0]["ProjectListPermission"].ToString();
            hdnProjectList.Value = ProjectList;
        }

        //WIP
        string wipPermission = ds.Rows[0]["WIPPermission"] == DBNull.Value ? "NNNNNN" : ds.Rows[0]["WIPPermission"].ToString();
        string reportWIP = wipPermission.Length < 6 ? "N" : wipPermission.Substring(5, 1);
        if (reportWIP == "N")
        {
            lnkWIP.Visible = false;
        }
        else
        {
            lnkWIP.Visible = true;
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
            RadGrid_Project.Columns[7].Visible = true;
            //hdnRadProject.Columns[6].Visible = true;
        }
        else
        {
            RadGrid_Project.Columns[7].Visible = false;
            //hdnRadProject.Columns[6].Visible = false;
        }
    }

    private void BindProjectDepartments()
    {
        DataSet ds = new DataSet();
        objProp_job.ConnConfig = Session["config"].ToString();
        ds = objBL_Project.GetAllJobTypeForSearch(objProp_job);

        rptDepartmentTab.DataSource = ds.Tables[0];
        rptDepartmentTab.DataBind();
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        if (hdnEditeJob.Value == "Y")
        {
            foreach (GridDataItem item in RadGrid_Project.SelectedItems)
            {
                Label lblID = (Label)item.Cells[1].FindControl("lblID");
                Response.Redirect("AddProject.aspx?uid=" + lblID.Text);
            }
        }
    }

    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        if (hdnEditeJob.Value == "Y")
        {
            foreach (GridDataItem item in RadGrid_Project.SelectedItems)
            {
                Label lblID = (Label)item.Cells[1].FindControl("lblID");
                Response.Redirect("AddProject.aspx?uid=" + lblID.Text + "&t=c");
            }
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnDeleteJob.Value == "Y")
            {
                BL_Customer objBL_Customer = new BL_Customer();

                Customer objProp_Customer = new Customer();

                foreach (GridDataItem item in RadGrid_Project.SelectedItems)
                {
                    Label lblID = (Label)item.Cells[1].FindControl("lblID");

                    objProp_Customer.ConnConfig = WebBaseUtility.ConnectionString;

                    objProp_Customer.ProjectJobID = Convert.ToInt32(lblID.Text);

                    objProp_Customer.Username = Session["Username"].ToString();

                    objBL_Customer.DeleteProject(objProp_Customer);
                }

                string CurrentPageSize = ViewState["CurrentPageSize"].ToString();

                string CurrentPageIndex = ViewState["CurrentPageIndex"].ToString();

                ProjectList(CurrentPageSize, CurrentPageIndex);

                RadGrid_Project.Rebind();
            }
        }
        catch (Exception ex)
        {

            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            if (str.IndexOf("The project is being used") >= 0 || str.IndexOf("The project is completed/closed") >= 0)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }

        }
    }

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("addproject.aspx");
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
        Session.Remove("ProjectTemp");
        Session.Remove("strUrl");
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
            objProp_User.Type = "Projects";
            dsGetReports = objBL_ReportsData.GetStockReports(objProp_User);
            //if (dsGetReports.Tables.Count > 0)
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

    protected void lnkactualVsbudgeted_Click(object sender, EventArgs e)
    {
        string url = "PrintJobActualBudget_New.aspx?sb=" + ddlSearch.SelectedValue + "&sv=" + txtSearch.Text + "&rng=" + ddlDateRange.SelectedValue + "&df=" + txtfromDate.Text + "&dt=" + txtToDate.Text;
        Response.Redirect(url);
    }

    protected void lnkJobSummary_Click(object sender, EventArgs e)
    {
        string tabId = hdnTabId.Value;
        string url = "PrintProjectSummaryListing.aspx?sb=" + ddlSearch.SelectedValue + "&sv=" + txtSearch.Text + "&rng=" + ddlDateRange.SelectedValue + "&df=" + txtfromDate.Text + "&dt=" + txtToDate.Text + "&tab=" + tabId + "&close=" + lnkChk.Checked;
        Response.Redirect(url);
    }

    protected void lnkJobWithBudgets_Click(object sender, EventArgs e)
    {
        string tabId = hdnTabId.Value;
        string url = "ProjectBacklogWithBudgetsReport.aspx?sb=" + ddlSearch.SelectedValue + "&sv=" + txtSearch.Text + "&rng=" + ddlDateRange.SelectedValue + "&df=" + txtfromDate.Text + "&dt=" + txtToDate.Text + "&tab=" + tabId + "&close=" + lnkChk.Checked;
        Response.Redirect(url);
    }

    protected void lnkProjectBacklog_Click(object sender, EventArgs e)
    {
        string tabId = hdnTabId.Value;
        string url = "ProjectBacklogReport.aspx?sb=" + ddlSearch.SelectedValue + "&sv=" + txtSearch.Text + "&rng=" + ddlDateRange.SelectedValue + "&df=" + txtfromDate.Text + "&dt=" + txtToDate.Text + "&tab=" + tabId + "&close=" + lnkChk.Checked;
        Response.Redirect(url);
    }

    protected void lnkVendorSchedule_Click(object sender, EventArgs e)
    {
        string script = "function f(){$find(\"" + VendorScheduleWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void lnkProjectVendorByProject_Click(object sender, EventArgs e)
    {
        string tabId = hdnTabId.Value != "-1" ? hdnTabId.Value : "";
        string searchValue = string.Empty;
        string searchTeamMemberVal = string.Empty;

        if (ddlSearch.SelectedValue == "j.fdate")
        {
            searchValue = txtInvDt.Text;
        }
        else if (ddlSearch.SelectedValue == "j.Status")
        {
            searchValue = ddlStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "j.PWIP")
        {
            searchValue = ddlProgressBilling.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "j.Certified")
        {
            searchValue = ddlCertified.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "t.title")
        {
            searchValue = ddlTeamTitle.SelectedValue;
            searchTeamMemberVal = txtSearchTeamMember.Text;
        }
        else
        {
            searchValue = txtSearch.Text;
        }

        string url = "ProjectVendorDemandByProject.aspx?sb=" + ddlSearch.SelectedValue + "&sv=" + searchValue + "&sMember=" + searchTeamMemberVal
            + "&rng=" + ddlDateRange.SelectedValue + "&df=" + txtfromDate.Text + "&dt=" + txtToDate.Text + "&depts=" + tabId + "&close=" + lnkChk.Checked + "&show=true";
        Response.Redirect(url);
    }

    protected void lnkProjectVendorByVendor_Click(object sender, EventArgs e)
    {
        string tabId = hdnTabId.Value != "-1" ? hdnTabId.Value : "";
        string searchValue = string.Empty;
        string searchTeamMemberVal = string.Empty;

        if (ddlSearch.SelectedValue == "j.fdate")
        {
            searchValue = txtInvDt.Text;
        }
        else if (ddlSearch.SelectedValue == "j.Status")
        {
            searchValue = ddlStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "j.PWIP")
        {
            searchValue = ddlProgressBilling.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "j.Certified")
        {
            searchValue = ddlCertified.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "t.title")
        {
            searchValue = ddlTeamTitle.SelectedValue;
            searchTeamMemberVal = txtSearchTeamMember.Text;
        }
        else
        {
            searchValue = txtSearch.Text;
        }

        string url = "ProjectVendorDemandByVendor.aspx?sb=" + ddlSearch.SelectedValue + "&sv=" + searchValue + "&sMember=" + searchTeamMemberVal
            + "&rng=" + ddlDateRange.SelectedValue + "&df=" + txtfromDate.Text + "&dt=" + txtToDate.Text + "&depts=" + tabId + "&close=" + lnkChk.Checked + "&show=true";
        Response.Redirect(url);
    }

    protected void lnkProjectSummaryReport_Click(object sender, EventArgs e)
    {
        var column = RadGrid_Project.MasterTableView.Columns.FindByUniqueName("Department");

        if (column != null && column.ListOfFilterValues != null && column.ListOfFilterValues.Count() > 0)
        {
            Session["ProjectDepartmentFilters"] = column.ListOfFilterValues;
        }

        string tabId = hdnTabId.Value != "-1" ? hdnTabId.Value : "";
        string searchValue = string.Empty;
        string searchTeamMemberVal = string.Empty;

        if (ddlSearch.SelectedValue == "j.fdate")
        {
            searchValue = txtInvDt.Text;
        }
        else if (ddlSearch.SelectedValue == "j.Status")
        {
            searchValue = ddlStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "j.PWIP")
        {
            searchValue = ddlProgressBilling.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "j.Certified")
        {
            searchValue = ddlCertified.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "t.title")
        {
            searchValue = ddlTeamTitle.SelectedValue;
            searchTeamMemberVal = txtSearchTeamMember.Text;
        }
        else
        {
            searchValue = txtSearch.Text;
        }

        string url = "ProjectSummaryReport.aspx?sb=" + ddlSearch.SelectedValue + "&sv=" + searchValue + "&sMember=" + searchTeamMemberVal
            + "&rng=" + ddlDateRange.SelectedValue + "&df=" + txtfromDate.Text + "&dt=" + txtToDate.Text + "&depts=" + tabId + "&close=" + lnkChk.Checked;
        Response.Redirect(url);
    }

    //lnkProjectSummaryReport_Click

    protected void lnkProjectSummarybyProjectManagerReport_Click(object sender, EventArgs e)
    {
        var column = RadGrid_Project.MasterTableView.Columns.FindByUniqueName("Department");

        if (column != null && column.ListOfFilterValues != null && column.ListOfFilterValues.Count() > 0)
        {
            Session["ProjectDepartmentFilters"] = column.ListOfFilterValues;
        }

        string tabId = hdnTabId.Value != "-1" ? hdnTabId.Value : "";
        string searchValue = string.Empty;
        string searchTeamMemberVal = string.Empty;

        if (ddlSearch.SelectedValue == "j.fdate")
        {
            searchValue = txtInvDt.Text;
        }
        else if (ddlSearch.SelectedValue == "j.Status")
        {
            searchValue = ddlStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "j.PWIP")
        {
            searchValue = ddlProgressBilling.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "j.Certified")
        {
            searchValue = ddlCertified.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "t.title")
        {
            searchValue = ddlTeamTitle.SelectedValue;
            searchTeamMemberVal = txtSearchTeamMember.Text;
        }
        else
        {
            searchValue = txtSearch.Text;
        }

        string url = "ProjectSummarybyProjectManagerReport.aspx?sb=" + ddlSearch.SelectedValue + "&sv=" + searchValue + "&sMember=" + searchTeamMemberVal
            + "&rng=" + ddlDateRange.SelectedValue + "&df=" + txtfromDate.Text + "&dt=" + txtToDate.Text + "&depts=" + tabId + "&close=" + lnkChk.Checked;
        Response.Redirect(url);
    }
    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {

        if (lnkChk.Checked)
        {
            isCheck = true;
        }
        else
        {
            isCheck = false;
        }


        string CurrentPageSize = ViewState["CurrentPageSize"].ToString();


        string CurrentPageIndex = ViewState["CurrentPageIndex"].ToString();


        ProjectList(CurrentPageSize, CurrentPageIndex);


        RadGrid_Project.Rebind();
    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Session["ddlStatusVal"] = ddlStatus.SelectedItem.Text;
    }


    protected void RadGrid_Project_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Project.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
            #region Set the Grid Filters
            if (!IsPostBack)
            {
                // Set filter for grid column except Department and Route
                if (Session["PROJ_OtherFilterExpression"] != null
                    && Convert.ToString(Session["PROJ_OtherFilterExpression"]) != ""
                    && Session["PROJ_OtherFilters"] != null)
                {
                    RadGrid_Project.MasterTableView.FilterExpression = Convert.ToString(Session["PROJ_OtherFilterExpression"]);
                    var filtersGet = Session["PROJ_OtherFilters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_Project.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                #region Save the Grid Filter
                String filterExpression = Convert.ToString(RadGrid_Project.MasterTableView.FilterExpression);
                if (filterExpression == "")
                {
                    filterExpression = Session["PROJ_OtherFilterExpression"] != null ? Session["PROJ_OtherFilterExpression"].ToString() : "";
                }

                if (filterExpression != "")
                {
                    Session["PROJ_OtherFilterExpression"] = filterExpression;
                    List<RetainFilter> filters = new List<RetainFilter>();

                    foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
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

                    Session["PROJ_OtherFilters"] = filters;
                }
                else
                {
                    Session["PROJ_OtherFilterExpression"] = null;
                    Session["PROJ_OtherFilters"] = null;
                }
                #endregion
            }

            // Reset filter for Department column
            if (Session["PROJ_DepartmentFilters"] != null && !string.IsNullOrEmpty(Session["PROJ_DepartmentFilters"].ToString()))
            {
                var strDepartmentFilters = Session["PROJ_DepartmentFilters"].ToString();

                string[] departmentItems = strDepartmentFilters.Split(',');
                for (int i = 0; i < departmentItems.Length; i++)
                {
                    departmentItems[i] = departmentItems[i].Trim('\'');
                }

                GridColumn departmentColumn = RadGrid_Project.MasterTableView.GetColumn("Department");
                departmentColumn.ListOfFilterValues = departmentItems;
            }

            // Reset filter for Route column
            if (Session["PROJ_RouteFilters"] != null && !string.IsNullOrEmpty(Session["PROJ_RouteFilters"].ToString()))
            {
                var strRouteFilters = Session["PROJ_RouteFilters"].ToString();
                //string[] departmentItems = ((RadGrid)sender).MasterTableView.GetColumn("DRoute").ListOfFilterValues;
                string[] routeItems = strRouteFilters.Split(',');
                GridColumn routeColumn = RadGrid_Project.MasterTableView.GetColumn("DRoute");
                for (int i = 0; i < routeItems.Length; i++)
                {
                    routeItems[i] = routeItems[i].Trim('\'');
                }
                routeColumn.ListOfFilterValues = routeItems;
            }

            // Reset filter for Stage column
            if (Session["PROJ_StageFilters"] != null && !string.IsNullOrEmpty(Session["PROJ_StageFilters"].ToString()))
            {
                var strStageFilters = Session["PROJ_StageFilters"].ToString();
                //string[] departmentItems = ((RadGrid)sender).MasterTableView.GetColumn("DRoute").ListOfFilterValues;
                string[] stageItems = strStageFilters.Split(',');
                GridColumn stageColumn = RadGrid_Project.MasterTableView.GetColumn("Stage");
                for (int i = 0; i < stageItems.Length; i++)
                {
                    stageItems[i] = stageItems[i].Trim('\'');
                }
                stageColumn.ListOfFilterValues = stageItems;
            }
            #endregion
        }
        catch { }



        string CurrentPageSize = ViewState["CurrentPageSize"].ToString();


        string CurrentPageIndex = ViewState["CurrentPageIndex"].ToString();


        ProjectList(CurrentPageSize, CurrentPageIndex);


    }

    protected void RadGrid_Project_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridFooterItem)
        //{
        //    GridFooterItem footerItem = (GridFooterItem)RadGrid_Project.MasterTableView.GetItems(GridItemType.Footer)[0];
        //    if (!string.IsNullOrEmpty(footerItem["NProfit"].Text) && !string.IsNullOrEmpty(footerItem["NRev"].Text))
        //    {

        //        char sign = '+';
        //        if ((footerItem["NProfit"].Text.IndexOf('(') == -1 && footerItem["NRev"].Text.IndexOf('(') == -1) || (footerItem["NProfit"].Text.IndexOf('(') > -1 && footerItem["NRev"].Text.IndexOf('(') > -1))
        //            sign = '+';
        //        else
        //            sign = '-';
        //        string sum1 = (footerItem["NProfit"].Text.Replace("$", "").Replace("(", "").Replace(")", ""));
        //        string sum2 = (footerItem["NRev"].Text.Replace("$", "").Replace("(", "").Replace(")", ""));
        //        decimal sum1Value = 0, sum2Value = 0, total = 0;
        //        if (decimal.TryParse(sum1, out sum1Value) && decimal.TryParse(sum2, out sum2Value))
        //        {
        //            total = Convert.ToDecimal(sum1) / ((Convert.ToDecimal(sum2) == 0) ? 1 : Convert.ToDecimal(sum2));
        //        }

        //        if (sign == '-')
        //            total = total * -1;
        //        Label lblPerFooter = footerItem.FindControl("footerNet") as Label;
        //        lblPerFooter.Text = string.Format("{0:N2} %", (total * 100));
        //    }
        //    else
        //    {
        //        Label lblPerFooter = footerItem.FindControl("footerNet") as Label;
        //        lblPerFooter.Text = "-";
        //    }
        //}

        DataTable dt = new DataTable();
        dt.Clear();
        dt.Columns.Add("EstimateID");
        dt.Columns.Add("Last");



        if (e.Item is GridDataItem)
        {
            var sss = e.Item.DataItem;
            Repeater InterestsRepeater = e.Item.FindControl("rptEstimates") as Repeater;
            HiddenField hdnEstimate = e.Item.FindControl("hdnGridEstimate") as HiddenField;
            if (hdnEstimate != null && !string.IsNullOrEmpty(hdnEstimate.Value))
            {
                var estArr = hdnEstimate.Value.Trim().Split(',');
                for (int i = 0; i < estArr.Length; i++)
                {
                    DataRow _temp = dt.NewRow();
                    _temp["EstimateID"] = estArr[i].Trim();
                    if (i == estArr.Length - 1)
                    {
                        _temp["Last"] = "true";
                    }
                    else
                    {
                        _temp["Last"] = "false";
                    }

                    dt.Rows.Add(_temp);
                }
            }
            //Get the instance of the right type
            if (InterestsRepeater != null)
            {
                InterestsRepeater.DataSource = dt;
                InterestsRepeater.DataBind();
            }


        }
    }

    protected void RadGrid_Project_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Filter")
        {
            ViewState["IsShownAll"] = "0";
            //string colName = ((Triplet)e.CommandArgument).First.ToString();
            string[] routeItems = ((RadGrid)sender).MasterTableView.GetColumn("DRoute").ListOfFilterValues;
            StringBuilder strRouteFilters = new StringBuilder();
            if (routeItems != null)
            {
                foreach (string route in routeItems)
                {
                    if (strRouteFilters.ToString() != string.Empty)
                    {
                        strRouteFilters.Append("," + "'" + route + "'");
                    }
                    else { strRouteFilters.Append("'" + route + "'"); }

                }
                Session["PROJ_RouteFilters"] = strRouteFilters.ToString();
            }
            else
            {
                Session["PROJ_RouteFilters"] = string.Empty;
            }

            string[] stageItems = ((RadGrid)sender).MasterTableView.GetColumn("Stage").ListOfFilterValues;
            StringBuilder strStageFilters = new StringBuilder();
            if (stageItems != null)
            {
                if (stageItems.Length > 0)
                {
                    foreach (string stage in stageItems)
                    {
                        if (strStageFilters.ToString() != string.Empty)
                        {
                            strStageFilters.Append("," + "'" + stage + "'");
                        }
                        else { strStageFilters.Append("'" + stage + "'"); }
                    }
                }

                Session["PROJ_StageFilters"] = strStageFilters.ToString();
            }
            else
            {
                Session["PROJ_StageFilters"] = string.Empty;
            }

            string[] departmentItems = ((RadGrid)sender).MasterTableView.GetColumn("Department").ListOfFilterValues;
            strRouteFilters = new StringBuilder();
            if (departmentItems != null)
            {
                foreach (string department in departmentItems)
                {
                    if (strRouteFilters.ToString() != string.Empty)
                    {
                        strRouteFilters.Append("," + "'" + department + "'");
                    }
                    else { strRouteFilters.Append("'" + department + "'"); }

                }
                Session["PROJ_DepartmentFilters"] = strRouteFilters.ToString();
            }
            else
            {
                Session["PROJ_DepartmentFilters"] = string.Empty;
            }
        }
    }

    private DataSet ProjectList(string CurrentPageSize = "0", string CurrentPageIndex = "0")
    {


        ScriptManager.RegisterStartupScript(this, GetType(), "dllsearch_method11", "dllsearch();", true);

        ScriptManager.RegisterStartupScript(this, GetType(), "fddldate_method22", "fddldate();", true);

        UpdateSearchCriteria();

        List<RetainFilter> filters = new List<RetainFilter>();

        try
        {
            foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
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

            Session["GridFilters"] = filters;
        }
        catch { }

        #region Company Check
        objProp_Customer.ConnConfig = WebBaseUtility.ConnectionString;
        objProp_Customer.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());
        if (System.Web.HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
            objProp_Customer.EN = 1;
        else
            objProp_Customer.EN = 0;
        #endregion

        #region SearchValue
        objProp_Customer.SearchBy = ddlSearch.SelectedValue;
        String searchedVal = "";
        String value = ddlSearch.SelectedValue;
        String searchedTeamMemberVal = "";

        if (value == "j.Status")
        {
            searchedVal = ddlStatus.SelectedValue;
        }
        else if (value == "j.PWIP")
        {
            searchedVal = ddlProgressBilling.SelectedValue;
        }
        else if (value == "j.Certified")
        {
            searchedVal = ddlCertified.SelectedValue;
        }
        else if (value == "t.title")
        {
            searchedVal = ddlTeamTitle.SelectedValue;
            searchedTeamMemberVal = txtSearchTeamMember.Text;
        }

        else
        {
            searchedVal = txtSearch.Text;
        }

        #endregion

        SetDefaultWorker();

        SetDefaultStage();

        objProp_Customer.SearchValue = searchedVal;

        objProp_Customer.Username = searchedTeamMemberVal;

        objProp_Customer.JobType = Convert.ToInt16(hdDept.Value);

        objProp_Customer.StartDate = txtfromDate.Text;

        objProp_Customer.EndDate = txtToDate.Text;

        objProp_Customer.Range = Convert.ToInt16(ddlDateRange.SelectedValue);

        HttpContext.Current.Session["Department"] = Convert.ToInt16(hdDept.Value);

        HttpContext.Current.Session["Range"] = Convert.ToInt16(ddlDateRange.SelectedValue);

        int IncludeClose = 0;

        IncludeClose = lnkChk.Checked ? 1 : 0;

        objProp_Customer.Status = IncludeClose;

        Session["JobListobjBL_Customer"] = objProp_Customer;
        setValueFitler();

        ///get filters to data table
        DataTable dtFilters = CreateFiltersToDataTable(filters);

        /////
        ///  SQL PAGING 
        ////
        string strORDERBY = ""; string strFieldName = "";

        if (RadGrid_Project.MasterTableView.SortExpressions.Count > 0)
        {
            if (RadGrid_Project.MasterTableView.SortExpressions[0].FieldName != string.Empty)
            {
                strFieldName = RadGrid_Project.MasterTableView.SortExpressions[0].FieldName + " "; strORDERBY = RadGrid_Project.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "DESC" : "ASC";

            }
        }
        strORDERBY = strFieldName + " " + strORDERBY;

        if (Session["JobListobjBL_Customer"] != null)
        {
            ViewState["defaultPageLoad"] = "0";
            objProp_Customer = (Customer)Session["JobListobjBL_Customer"];
        }


        if (ViewState["defaultPageLoad"].ToString() == "1")
        {
            objProp_Customer.SearchBy = "j.id";

            objProp_Customer.SearchValue = "-1";
        }
        Session["New_Report_GridFilters"] = dtFilters;
        DataSet ds = objBL_Customer.getJobProject(objProp_Customer, dtFilters, new GeneralFunctions().GetSalesAsigned(), IncludeClose, CurrentPageSize, CurrentPageIndex, strORDERBY);


        ViewState["defaultPageLoad"] = "0";

        ViewState["ProjectFoot"] = ds.Tables[1];


        DataTable ds_New = new DataTable();

        ds_New = ds.Tables[0];

        DataTable dtJPI = new DataTable();

        dtJPI = ds_New.Copy();

        foreach (DataColumn dc in ds_New.Columns)
        {
            var columnName = dc.ColumnName;

            if (dc.ColumnName != "ID")
            {
                dtJPI.Columns.Remove(columnName);
            }
        }

        int rc = 0;

        int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out rc);


        RadGrid_Project.VirtualItemCount = rc;

        RadGrid_Project.DataSource = ds_New;

        // ViewState["dtProject"] = ds_New;

        //if (filters != null && filters.Count > 0)
        {
            RadGrid_Project.MasterTableView.FilterExpression = string.Empty;
        }


        lblRecordCount.Text = ds.Tables[1].Rows[0][0].ToString() + " Record(s) found";

        DataTable navtbl = new DataTable();

        navtbl.Columns.Add("ID");


        foreach (DataRow dr in ds_New.Rows)
        {
            DataRow _ravi = navtbl.NewRow(); _ravi["ID"] = dr["ID"].ToString(); navtbl.Rows.Add(_ravi);
        }

        Session["projids"] = navtbl;

        return ds;
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Project.MasterTableView.FilterExpression != "" ||
            (RadGrid_Project.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Project.MasterTableView.SortExpressions.Count > 0;
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {


        txtSearch.Text = "";

        int CurrentPageSize = 50; int CurrentPageIndex = 0;

        ViewState["CurrentPageSize"] = CurrentPageSize;

        ViewState["CurrentPageIndex"] = CurrentPageIndex;

        ddlSearch.SelectedIndex = 0;

        RadGrid_Project.CurrentPageIndex = 0;

        RadGrid_Project.PageSize = 50;

        lnkChk.Checked = false;

        // ResetFormControlValuesExceptDateRange(UpdatePanel1);


        ddlDateRange.SelectedValue = "5";
        txtfromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();
        txtToDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToShortDateString();
        txtfromDate.Visible = txtToDate.Visible = true;
        ViewState["IsShownAll"] = "0";

        foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;

            column.CurrentFilterValue = string.Empty;

            if (column is GridBoundColumn)
            {
                GridBoundColumn boundColumn = column as GridBoundColumn;
                GridFilteringItem filter = RadGrid_Project.MasterTableView.GetItems(GridItemType.FilteringItem)[0] as GridFilteringItem;
                boundColumn.EvaluateFilterExpression(filter);
            }
        }

        Session["PROJ_DepartmentFilters"] = string.Empty;
        Session["PROJ_RouteFilters"] = string.Empty;
        Session["PROJ_StageFilters"] = string.Empty;
        RadGrid_Project.MasterTableView.FilterExpression = string.Empty;
        Session["PROJ_OtherFilterExpression"] = null;
        RadGrid_Project.Rebind();
        Response.Redirect("project.aspx");



        ScriptManager.RegisterStartupScript(this, GetType(), "dllsearch_method1", "dllsearch();", true);

        ScriptManager.RegisterStartupScript(this, GetType(), "fddldate_method2", "fddldate();", true);

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
    }

    private void ResetFormControlValuesExceptDateRange(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                ResetFormControlValuesExceptDateRange(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        var ddlControl = (DropDownList)c;
                        if (ddlControl.ID != ddlDateRange.ID)
                            ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        var txtControl = (TextBox)c;
                        if (txtControl.ID != txtfromDate.ID && txtControl.ID != txtToDate.ID)
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
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        ddlDateRange.SelectedIndex = 0;
        txtfromDate.Text = txtToDate.Text = "";

        ScriptManager.RegisterStartupScript(this, GetType(), "dllsearch_method123", "dllsearch();", true);

        ScriptManager.RegisterStartupScript(this, GetType(), "fddldate_method2232", "fddldate();", true);

        ViewState["IsShownAll"] = "1";

        ResetFormControlValues(this);

        foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
            if (column is GridBoundColumn)
            {
                GridBoundColumn boundColumn = column as GridBoundColumn;
                GridFilteringItem filter = RadGrid_Project.MasterTableView.GetItems(GridItemType.FilteringItem)[0] as GridFilteringItem;
                boundColumn.EvaluateFilterExpression(filter);
            }
        }
        lnkChk.Checked = false;
        Session["PROJ_DepartmentFilters"] = string.Empty;
        Session["PROJ_RouteFilters"] = string.Empty;
        Session["PROJ_StageFilters"] = string.Empty;
        hdDept.Value = "-1";
        Session["PROJ_OtherFilterExpression"] = "";
        RadGrid_Project.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Project.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "selectTab", "$('#tabProject > li > a#" + hdDept.Value + "')[0].click();", true);


    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        if (ddlSearch.SelectedValue == "j.id")
        {
            ddlDateRange.SelectedIndex = 0;

            txtfromDate.Text = txtToDate.Text = "";
        }
        else if (ddlDateRange.SelectedIndex != 0 && txtfromDate.Text == "" && txtToDate.Text == "")
        {


            txtfromDate.Text = txtToDate.Text = "";

            txtfromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();

            txtToDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToShortDateString();
        }

        ViewState["IsShownAll"] = "0";

        RadGrid_Project.Rebind();

        ScriptManager.RegisterStartupScript(this, GetType(), "dllsearch_method12df3", "dllsearch();", true);

        ScriptManager.RegisterStartupScript(this, GetType(), "fddldate_method2dd232", "fddldate();", true);

    }

    protected void RadGrid_Project_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

                if (totalCount == 0) totalCount = 100000;
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
        try
        {
            if (hasNoRow)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "alertNoRow", "alert('Nothing to export');", true);
            }
            else
            {
                DataSet ds = ProjectList();

                DataTable dtProjectGrid = ds.Tables[0];

                hasNoRow = false;

                DataTable dtProjectGridExport = CreateProjectListForExport(dtProjectGrid);

                dtProjectGridExport.Columns.Remove("ID");

                dtProjectGridExport.AcceptChanges();

                var listDT = SplitDataTable(dtProjectGridExport, 65535);
                var tempFile = string.Format("ProjectList{0}.xlsx", DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
                var fileName = Server.MapPath(string.Format("ReportFiles/ExportExcel/{0}", tempFile));

                //DataTable dtProjectGridExport = new DataTable();
                var isfirst = true;
                foreach (DataTable dtProjectExport in listDT)
                {
                    ExportToOxml(isfirst, fileName, dtProjectExport);
                    isfirst = false;
                }

                var strFileName = fileName;
                if (strFileName.Length > 0)
                {
                    try
                    {
                        DownloadDocument(strFileName, "ProjectList.xlsx");
                        // Delete after downloaded
                        if (File.Exists(strFileName))
                            File.Delete(strFileName);
                    }
                    catch (Exception)
                    {
                        // Delete after downloaded
                        if (File.Exists(strFileName))
                            File.Delete(strFileName);
                    }
                }
                else
                {
                    string str = "Export failed!";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrExportExcel", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkExporttoExcelCustomLabel_Click(object sender, EventArgs e)
    {
        try
        {
            if (hasNoRow)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "alertNoRow", "alert('Nothing to export');", true);
            }
            else
            {
                hasNoRow = false;

                DataSet ds1 = ProjectList();

                DataTable dtProjectGrid = ds1.Tables[0];

                DataTable dtProjectGridExport = CreateProjectListForExport(dtProjectGrid);

                objProp_Customer.ConnConfig = Session["config"].ToString();
                var joblst = from dt in dtProjectGridExport.AsEnumerable() select dt.Field<int>("ID");
                DataTable newDataTable = new DataTable();
                newDataTable.Columns.Add("ID", typeof(int));
                newDataTable.Columns.Add("Template", typeof(int));
                foreach (var array in joblst)
                {
                    newDataTable.Rows.Add(array);
                }
                objProp_Customer.dtCustom = newDataTable;
                DataSet ds = new DataSet();

                ds = objBL_Customer.GetProjectsWithWorkflows(objProp_Customer);

                DataTable newDtProjectGrid = new DataTable();

                if (ds.Tables.Count > 0)
                {
                    DataTable dtCustomLabel = ds.Tables[0];
                    ViewState["dtCustomLabel"] = dtCustomLabel;

                    newDtProjectGrid = JoinDataTableByColumn(dtProjectGridExport, dtCustomLabel, "ID");
                }
                else
                {
                    newDtProjectGrid = dtProjectGridExport;
                }
                newDtProjectGrid.Columns.Remove("ID");
                newDtProjectGrid.Columns.Remove("Budgeted Labor");
                newDtProjectGrid.Columns.Remove("Budgeted Hours");
                newDtProjectGrid.AcceptChanges();

                var listDT = SplitDataTable(newDtProjectGrid, 65535);

                var tempFile = string.Format("ProjectListWithWorkflow{0}.xlsx", DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
                var fileName = Server.MapPath(string.Format("ReportFiles/ExportExcel/{0}", tempFile));

                var isfirst = true;
                foreach (DataTable dt in listDT)
                {
                    ExportToOxml(isfirst, fileName, dt);
                    isfirst = false;
                }

                var strFileName = fileName;
                if (strFileName.Length > 0)
                {
                    try
                    {
                        DownloadDocument(strFileName, "ProjectListWithWorkflow.xlsx");
                        // Delete after downloaded
                        if (File.Exists(strFileName))
                            File.Delete(strFileName);
                    }
                    catch (Exception)
                    {
                        // Delete after downloaded
                        if (File.Exists(strFileName))
                            File.Delete(strFileName);
                    }
                }
                else
                {
                    string str = "Export failed!";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrExportExcel", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void setValueFitler()
    {


        if (HttpContext.Current.Request.QueryString["sb"] != null && HttpContext.Current.Request.QueryString["sb"].ToString() != "")
        {
            //ddlSearch.SelectedIndex = 2;
            ////ddlSearch.ClearSelection(); //making sure the previous selection has been cleared
            ////ddlSearch.Items.FindByValue("j.id").Selected = true;
            ddlSearch.SelectedIndex = ddlSearch.Items.IndexOf(ddlSearch.Items.FindByValue(HttpContext.Current.Request.QueryString["sb"].ToString())); // If you want to find text by value field.
            //ddlSearch.SelectedValue = ddlSearch.Items.IndexOf(ddlSearch.Items.FindByValue("j.id"));
        }

        if (HttpContext.Current.Request.QueryString["sv"] != null && HttpContext.Current.Request.QueryString["sv"].ToString() != "")
        {
            txtSearch.Text = HttpContext.Current.Request.QueryString["sv"].ToString();
        }
        if (HttpContext.Current.Request.QueryString["rng"] != null && HttpContext.Current.Request.QueryString["rng"].ToString() != "")
        {
            ddlDateRange.SelectedValue = HttpContext.Current.Request.QueryString["rng"].ToString();
            if (ddlDateRange.SelectedValue == "2" || ddlDateRange.SelectedValue == "3" || ddlDateRange.SelectedValue == "4" || ddlDateRange.SelectedValue == "5")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "dllsearch_method11", "dllsearch();", true);

                ScriptManager.RegisterStartupScript(this, GetType(), "fddldate_method22", "fddldate();", true);

            }
        }

        if (HttpContext.Current.Request.QueryString["tab"] != null && HttpContext.Current.Request.QueryString["tab"].ToString() != "")
        {
            hdnTabId.Value = HttpContext.Current.Request.QueryString["tab"].ToString();
        }


        if (HttpContext.Current.Request.QueryString["df"] != null && HttpContext.Current.Request.QueryString["df"].ToString() != "")
        {
            txtfromDate.Text = HttpContext.Current.Request.QueryString["df"].ToString();
        }

        if (HttpContext.Current.Request.QueryString["dt"] != null && HttpContext.Current.Request.QueryString["dt"].ToString() != "")
        {
            txtToDate.Text = HttpContext.Current.Request.QueryString["dt"].ToString();
        }

        if (HttpContext.Current.Request.QueryString["close"] != null && HttpContext.Current.Request.QueryString["close"].ToString() != "")
        {
            if (Convert.ToBoolean(HttpContext.Current.Request.QueryString["close"].ToString()))
            {
                lnkChk.Checked = Convert.ToBoolean(HttpContext.Current.Request.QueryString["close"].ToString());
                isCheck = lnkChk.Checked;
            }

        }
    }

    private void SetDefaultWorker()
    {
        Customer objCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();

        //var hdnMasterTableView = hdnRadProject.MasterTableView;
        var masterTableView = RadGrid_Project.MasterTableView;
        //var hdnColumn = hdnMasterTableView.GetColumn("DRoute");
        var column = masterTableView.GetColumn("DRoute");
        objCustomer.ConnConfig = Session["config"].ToString();
        string getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        if (!string.IsNullOrEmpty(getValue))
        {
            column.HeaderText = getValue;
            //hdnColumn.HeaderText = getValue;
        }
        else
        {
            column.HeaderText = "Default Worker";
            //hdnColumn.HeaderText = "Default Worker";
        }
    }

    private void SetDefaultStage()
    {
        Customer objCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();

        var masterTableView = RadGrid_Project.MasterTableView;
        var column = masterTableView.GetColumn("Stage");
        objCustomer.ConnConfig = Session["config"].ToString();
        string getValue = objBL_Customer.GetProjectStageHeader(objCustomer);
        if (!string.IsNullOrEmpty(getValue))
        {
            column.HeaderText = getValue;
        }
        else
        {
            column.HeaderText = "Stage";
        }
    }

    protected void RadGrid_Project_PreRender(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["ProjectFoot"];

            #region Save the Grid Filter
            String filterExpression = Convert.ToString(RadGrid_Project.MasterTableView.FilterExpression);

            foreach (GridDataItem gr in RadGrid_Project.Items)
            {
                double isNegative = 0.00;
                GridFooterItem footeritem = (GridFooterItem)RadGrid_Project.MasterTableView.GetItems(GridItemType.Footer)[0];
                Label lblfooterContractPrice = (Label)footeritem.FindControl("footerContractPrice");

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_ContractPrice"].ToString(), out isNegative);
                lblfooterContractPrice.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                lblfooterContractPrice.Text = "$" + dt.Rows[0]["F_ContractPrice"].ToString();

                Label lblfooterNotBilledYet = (Label)footeritem.FindControl("footerNotBilledYet");
                lblfooterNotBilledYet.Text = "$" + dt.Rows[0]["F_NotBilledYet"].ToString();

                isNegative = 0.00;

                double.TryParse(dt.Rows[0]["F_NotBilledYet"].ToString(), out isNegative);
                lblfooterNotBilledYet.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterNRev = (Label)footeritem.FindControl("footerNRev");
                lblfooterNRev.Text = "$" + dt.Rows[0]["F_TotalBilled"].ToString();

                isNegative = 0.00;
                double.TryParse(dt.Rows[0]["F_TotalBilled"].ToString(), out isNegative);
                lblfooterNRev.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterNHour = (Label)footeritem.FindControl("footerNHour");
                lblfooterNHour.Text = dt.Rows[0]["F_AtualHour"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_AtualHour"].ToString(), out isNegative);
                lblfooterNHour.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterNLabor = (Label)footeritem.FindControl("footerNLabor");
                lblfooterNLabor.Text = "$" + dt.Rows[0]["F_LaborExpense"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_LaborExpense"].ToString(), out isNegative);
                lblfooterNLabor.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterNMat = (Label)footeritem.FindControl("footerNMat");
                lblfooterNMat.Text = "$" + dt.Rows[0]["F_MaterialExpense"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_MaterialExpense"].ToString(), out isNegative);
                lblfooterNMat.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterNOMat = (Label)footeritem.FindControl("footerNOMat");
                lblfooterNOMat.Text = "$" + dt.Rows[0]["F_OtherExpense"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_OtherExpense"].ToString(), out isNegative);
                lblfooterNOMat.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterNCost = (Label)footeritem.FindControl("footerNCost");
                lblfooterNCost.Text = "$" + dt.Rows[0]["F_TotalsExpense"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_TotalsExpense"].ToString(), out isNegative);
                lblfooterNCost.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterTotalBudgetedExpense = (Label)footeritem.FindControl("footerTotalBudgetedExpense");
                lblfooterTotalBudgetedExpense.Text = "$" + dt.Rows[0]["F_TotalBudgetedExpense"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_TotalBudgetedExpense"].ToString(), out isNegative);
                lblfooterTotalBudgetedExpense.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterOpenAR = (Label)footeritem.FindControl("footerOpenARBalance");
                lblfooterOpenAR.Text = "$" + dt.Rows[0]["F_OpenARBalance"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_OpenARBalance"].ToString(), out isNegative);
                lblfooterOpenAR.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterBLabor = (Label)footeritem.FindControl("footerBLabor");
                lblfooterBLabor.Text = "$" + dt.Rows[0]["F_BudgetedLabor"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_BudgetedLabor"].ToString(), out isNegative);
                lblfooterBLabor.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterBHour = (Label)footeritem.FindControl("footerBHour");
                lblfooterBHour.Text = dt.Rows[0]["F_BudgetedHour"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_BudgetedHour"].ToString(), out isNegative);
                lblfooterBHour.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterNComm = (Label)footeritem.FindControl("footerNComm");
                lblfooterNComm.Text = "$" + dt.Rows[0]["F_TotalPOOrder"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_TotalPOOrder"].ToString(), out isNegative);
                lblfooterNComm.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterReceivePO = (Label)footeritem.FindControl("footerReceivePO");
                lblfooterReceivePO.Text = "$" + dt.Rows[0]["F_TotalReceivePO"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_TotalReceivePO"].ToString(), out isNegative);
                lblfooterReceivePO.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterNProfit = (Label)footeritem.FindControl("footerNProfit");
                lblfooterNProfit.Text = "$" + dt.Rows[0]["F_NetProfit"].ToString();

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_NetProfit"].ToString(), out isNegative);
                lblfooterNProfit.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                Label lblfooterNet = (Label)footeritem.FindControl("footerNet");
                lblfooterNet.Text = dt.Rows[0]["F_perinProfit"].ToString() + "%";

                isNegative = 0;
                double.TryParse(dt.Rows[0]["F_perinProfit"].ToString(), out isNegative);
                lblfooterNet.ForeColor = isNegative < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            }

            // Show Restore Grid button
            DataSet ds = new DataSet();
            objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            objProp_User.PageName = "Project.aspx";
            objProp_User.GridId = "RadGrid_Project";
            ds = objBL_User.GetGridUserSettings(objProp_User);

            if (ds.Tables[0].Rows.Count > 0)
            {
                var columnSettings = ds.Tables[0].Rows[0][0].ToString();
                var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);
                var colIndex = 0;

                foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
                {
                    colIndex++;
                    var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (clSetting != null)
                    {
                        column.Display = clSetting.Display;
                        if (colIndex >= 3 && clSetting.Width != 0)
                            column.HeaderStyle.Width = clSetting.Width;

                        column.OrderIndex = clSetting.OrderIndex;
                    }
                }

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showhidebutton", "ShowRestoreGridSettingsButton();", true);
            }
        }
        catch { }

        #endregion
    }

    private DataTable CreateFiltersToDataTable(List<RetainFilter> filters)
    {
        //create new table to add filter values.
        DataTable dtFilters = new DataTable();
        dtFilters.Clear();
        dtFilters.Columns.Add("Customer");
        dtFilters.Columns.Add("Tag");
        dtFilters.Columns.Add("ID");
        dtFilters.Columns.Add("fdesc");
        dtFilters.Columns.Add("Status");
        dtFilters.Columns.Add("Stage");
        dtFilters.Columns.Add("Company");
        dtFilters.Columns.Add("CType");
        dtFilters.Columns.Add("TemplateDesc");
        dtFilters.Columns.Add("Type");
        dtFilters.Columns.Add("SalesPerson");
        dtFilters.Columns.Add("Route");
        dtFilters.Columns.Add("NHour");
        dtFilters.Columns.Add("ContractPrice");
        dtFilters.Columns.Add("NotBilledYet");
        dtFilters.Columns.Add("NComm");
        dtFilters.Columns.Add("NRev");
        dtFilters.Columns.Add("NLabor");
        dtFilters.Columns.Add("NMat");
        dtFilters.Columns.Add("NOMat");
        dtFilters.Columns.Add("NCost");
        dtFilters.Columns.Add("NProfit");
        dtFilters.Columns.Add("NRatio");
        dtFilters.Columns.Add("RouteFilters");
        dtFilters.Columns.Add("StageFilters");
        dtFilters.Columns.Add("DepartmentFilters");
        dtFilters.Columns.Add("ProjectManagerUserName");
        dtFilters.Columns.Add("LocationType");
        dtFilters.Columns.Add("BuildingType");
        dtFilters.Columns.Add("TotalBudgetedExpense");
        dtFilters.Columns.Add("SupervisorUserName");
        dtFilters.Columns.Add("OpenARBalance");
        dtFilters.Columns.Add("OpenAPBalance");
        dtFilters.Columns.Add("ExpectedClosingDate");
        dtFilters.Columns.Add("Estimate");

        DataRow dtFiltersRow = dtFilters.NewRow();



        if (filters != null && filters.Count > 0)
        {
            decimal filterValue = -1900;

            dtFiltersRow["NHour"] = filterValue;
            dtFiltersRow["TotalBudgetedExpense"] = filterValue;
            dtFiltersRow["NRatio"] = filterValue;
            dtFiltersRow["ContractPrice"] = filterValue;
            dtFiltersRow["NComm"] = filterValue;
            dtFiltersRow["NotBilledYet"] = filterValue;
            dtFiltersRow["NRev"] = filterValue;
            dtFiltersRow["NLabor"] = filterValue;
            dtFiltersRow["NMat"] = filterValue;
            dtFiltersRow["NOMat"] = filterValue;
            dtFiltersRow["NCost"] = filterValue;
            dtFiltersRow["NProfit"] = filterValue;
            foreach (var items in filters)
            {
                if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                {

                    if (items.FilterColumn == "LocationType")
                    {
                        dtFiltersRow["LocationType"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "BuildingType")
                    {
                        dtFiltersRow["BuildingType"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "Customer")
                    {
                        dtFiltersRow["Customer"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "Tag")
                    {
                        dtFiltersRow["Tag"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "ProjectManagerUserName")
                    {
                        dtFiltersRow["ProjectManagerUserName"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "SupervisorUserName")
                    {
                        dtFiltersRow["SupervisorUserName"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "OpenARBalance")
                    {
                        dtFiltersRow["OpenARBalance"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "OpenAPBalance")
                    {
                        dtFiltersRow["OpenAPBalance"] = items.FilterValue;
                    }
                    /// Int Filter
                    int FilterValue = 0; string[] filterArrayValue;

                    StringBuilder filteredQuery = new StringBuilder();

                    if (items.FilterColumn == "ID")
                    {
                        filterArrayValue = items.FilterValue.ToString().Split(',');
                        foreach (var filtered in filterArrayValue)
                        {
                            if (int.TryParse(filtered, out FilterValue))
                            {
                                if (filteredQuery.Length == 0)
                                {
                                    filteredQuery.Append(filtered);
                                }
                                else
                                {
                                    filteredQuery.Append("," + filtered);
                                }
                            }


                        }
                        dtFiltersRow["ID"] = filteredQuery.ToString();
                    }

                    if (items.FilterColumn == "fdesc")
                    {
                        dtFiltersRow["fdesc"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Status")
                    {
                        dtFiltersRow["Status"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Stage")
                    {
                        dtFiltersRow["Stage"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Company")
                    {
                        dtFiltersRow["Company"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "CType")
                    {
                        dtFiltersRow["CType"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "TemplateDesc")
                    {
                        dtFiltersRow["TemplateDesc"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Department")
                    {
                        dtFiltersRow["Type"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Salesperson")
                    {
                        dtFiltersRow["SalesPerson"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "DRoute")
                    {
                        dtFiltersRow["Route"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "LocationType")
                    {
                        dtFiltersRow["LocationType"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "BuildingType")
                    {
                        dtFiltersRow["BuildingType"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "ExpectedClosingDate")
                    {
                        dtFiltersRow["ExpectedClosingDate"] = items.FilterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "ContractPrice" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["ContractPrice"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NotBilledYet" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NotBilledYet"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NComm" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NComm"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NRev" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NRev"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NLabor" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NLabor"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NMat" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NMat"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NOMat" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NOMat"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NCost" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NCost"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NProfit" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NProfit"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NRatio" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NRatio"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "TotalBudgetedExpense" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["TotalBudgetedExpense"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NHour" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NHour"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "OpenARBalance" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["OpenARBalance"] = filterValue;
                    }

                    if (items.FilterColumn == "OpenAPBalance" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["OpenAPBalance"] = filterValue;
                    }

                    if (items.FilterColumn.ToLower() == "estimate")
                    {
                        dtFiltersRow["Estimate"] = items.FilterValue;
                    }
                }
            }
        }

        if (Session["PROJ_RouteFilters"] != null && Session["PROJ_RouteFilters"].ToString() != string.Empty)
        {
            dtFiltersRow["RouteFilters"] = Session["PROJ_RouteFilters"].ToString();
        }

        if (Session["PROJ_StageFilters"] != null && Session["PROJ_StageFilters"].ToString() != string.Empty)
        {
            dtFiltersRow["StageFilters"] = Session["PROJ_StageFilters"].ToString();
        }

        if (Session["PROJ_DepartmentFilters"] != null && Session["PROJ_DepartmentFilters"].ToString() != string.Empty)
        {
            dtFiltersRow["DepartmentFilters"] = Session["PROJ_DepartmentFilters"].ToString();
        }


        dtFilters.Rows.Add(dtFiltersRow);

        return dtFilters;

    }

    protected void RadGrid_Project_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;


            int CurrentPageSize = RadGrid_Project.PageSize; int CurrentPageIndex = e.NewPageIndex;

            ViewState["CurrentPageSize"] = CurrentPageSize;

            ViewState["CurrentPageIndex"] = CurrentPageIndex;

        }
        catch { }
    }

    protected void RadGrid_Project_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {


            int CurrentPageSize = e.NewPageSize; int CurrentPageIndex = RadGrid_Project.CurrentPageIndex;

            ViewState["CurrentPageSize"] = CurrentPageSize;

            ViewState["CurrentPageIndex"] = CurrentPageIndex;

        }
        catch { }
    }

    protected void lnkLaborExpenses_Click(object sender, EventArgs e)
    {
        try
        {

            if (DateTime.TryParse(txtfromDate.Text, out objProp_job.StartDate) == true && DateTime.TryParse(txtToDate.Text, out objProp_job.EndDate) == true)
            {
                DataSet ds = new DataSet();
                objProp_job.ConnConfig = Session["config"].ToString();
                objProp_job.StartDate = Convert.ToDateTime(txtfromDate.Text);
                objProp_job.EndDate = Convert.ToDateTime(txtToDate.Text);
                objBL_Project.RecalculateLaborExpenses(objProp_job);
                RadGrid_Project.Rebind();

            }
        }
        catch (Exception ex) { }
    }

    protected void lnkWIP_Click(object sender, EventArgs e)
    {
        Response.Redirect("projectwip.aspx");
    }

    private void FillSearchFilter()
    {
        ddlSearch.Items.Add(new ListItem("Select", string.Empty));
        ddlSearch.Items.Add(new ListItem("Project #", "j.id"));
        ddlSearch.Items.Add(new ListItem("Location", "l.tag"));
        ddlSearch.Items.Add(new ListItem("Location City", "l.City"));
        ddlSearch.Items.Add(new ListItem("Location State", "l.State"));
        ddlSearch.Items.Add(new ListItem("Status", "j.Status"));
        ddlSearch.Items.Add(new ListItem("Description", "j.fdesc"));
        ddlSearch.Items.Add(new ListItem("Customer", "r.Name"));
        ddlSearch.Items.Add(new ListItem("Progress Billing", "j.PWIP"));
        ddlSearch.Items.Add(new ListItem("Certified Payroll", "j.Certified"));
        ddlSearch.Items.Add(new ListItem("User Role", "t.title"));
        DataSet ds = new DataSet();
        DataSet dsJobAttribute = new DataSet();

        BL_Job objBL_Job = new BL_Job();
        JobT objJob = new JobT();
        objJob.ConnConfig = Session["config"].ToString();
        objJob.Job = 0;
        ds = objBL_Job.GetJobCustomValueByJobId(objJob);
        dsJobAttribute = objBL_Job.GetJobAttributeGeneralCustomValueByJobId(objJob);
        int item = 1;
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlSearch.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CustomLabel"].ToString(), "j.Custom" + item));
                    item = item + 1;
                }

            }
        }

        if (dsJobAttribute != null)
        {
            if (dsJobAttribute.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsJobAttribute.Tables[0].Rows.Count; i++)
                {
                    ddlSearch.Items.Add(new ListItem(dsJobAttribute.Tables[0].Rows[i]["CustomLabel"].ToString(), "j.Custom" + item));
                    item = item + 1;
                }

            }
        }



        BL_User objBL_User = new BL_User();
        User objUser = new User();
        objUser.ConnConfig = Session["config"].ToString();
        //objJob.Job = 0;
        ds = objBL_User.GetTeamMemberTitle(objUser, true);
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlTeamTitle.Items.Add(new ListItem(ds.Tables[0].Rows[i]["Title"].ToString(), ds.Tables[0].Rows[i]["Title"].ToString()));
                }
            }
        }

    }

    private void UpdateSearchCriteria()
    {
        List<SearchCriteria> searchCriterias = new List<SearchCriteria>();
        // Get search controls in Generic Search
        //GetControlValues(UpdatePanel1, searchCriterias);

        Session["PROJ_SearchCriteria"] = searchCriterias;
        Session["PROJ_DepartmentTab"] = hdDept.Value;
        Session["txtfrmDtValforEditjob"] = txtfromDate.Text;
        Session["txttoDtValforEditJob"] = txtToDate.Text;
        Session["ddlDateRangeFieldforEditJob"] = Convert.ToInt16(ddlDateRange.SelectedValue);

    }

    private void GetControlValues(Control parent, List<SearchCriteria> searchCriteria)
    {

        if (searchCriteria == null)
        {
            searchCriteria = new List<SearchCriteria>();
        }
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                GetControlValues(c, searchCriteria);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        DropDownList ddlControl = (DropDownList)c;
                        if (ddlControl.SelectedIndex != 0)
                        {
                            var ddlVal = ddlControl.SelectedValue;
                            var ddlName = ddlControl.ID;
                            searchCriteria.Add(new SearchCriteria() { Name = ddlName, Value = ddlVal, Type = c.GetType().ToString() });
                        }
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        TextBox txtControl = (TextBox)c;
                        if (!string.IsNullOrEmpty(txtControl.Text))
                        {
                            searchCriteria.Add(new SearchCriteria() { Name = txtControl.ID, Value = txtControl.Text, Type = c.GetType().ToString() });
                        }
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        CheckBox chkControl = (CheckBox)c;
                        searchCriteria.Add(new SearchCriteria() { Name = chkControl.ID, Value = chkControl.Checked.ToString(), Type = c.GetType().ToString() });
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        RadioButton rdControl = (RadioButton)c;
                        searchCriteria.Add(new SearchCriteria() { Name = rdControl.ID, Value = rdControl.Checked.ToString(), Type = c.GetType().ToString() });
                        break;
                }
            }
        }
    }

    private void UpdateControlValues()
    {
        List<SearchCriteria> searchCriterias = (List<SearchCriteria>)Session["PROJ_SearchCriteria"];
        if (searchCriterias != null && Request.QueryString["fil"] != null && Request.QueryString["fil"].ToString() == "1")
        {
            IsGridPageIndexChanged = true;
            foreach (var item in searchCriterias)
            {
                switch (item.Type)
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        DropDownList ddlControl = (DropDownList)FindControlRecursive(Page, item.Name);
                        if (ddlControl != null)
                        {
                            ddlControl.SelectedValue = item.Value;
                        }
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        TextBox txtControl = (TextBox)FindControlRecursive(Page, item.Name);
                        if (txtControl != null)
                        {
                            txtControl.Text = item.Value;
                            if (txtControl.ID == "txtfromDate")
                            {
                                ViewState["fromDate"] = txtControl.Text;

                            }
                            else if (txtControl.ID == "txtToDate")
                            {
                                ViewState["ToDate"] = txtControl.Text;
                            }
                        }
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        CheckBox chkControl = (CheckBox)FindControlRecursive(Page, item.Name);
                        if (chkControl != null)
                        {
                            chkControl.Checked = Convert.ToBoolean(item.Value);
                        }
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        RadioButton rdControl = (RadioButton)FindControlRecursive(Page, item.Name);
                        if (rdControl != null)
                        {
                            rdControl.Checked = Convert.ToBoolean(item.Value);
                        }
                        break;
                    case "Telerik.Web.UI.RadComboBox":
                        RadComboBox rcbControl = (RadComboBox)FindControlRecursive(Page, item.Name);
                        if (rcbControl != null)
                        {
                            var arr = item.Value.Split(';');
                            foreach (RadComboBoxItem cb in rcbControl.Items)
                            {
                                if (arr.Contains(cb.Value))
                                {
                                    cb.Checked = true;
                                }
                            }
                        }
                        break;
                }
            }

            if (Session["PROJ_DepartmentTab"] != null)
            {
                hdDept.Value = Session["PROJ_DepartmentTab"].ToString();

            }

            // release session after used
            Session["PROJ_SearchCriteria"] = null;
            Session["PROJ_DepartmentTab"] = null;
        }
        else
        {
            // Set default value for DateRange
            ddlDateRange.SelectedValue = "5";
            txtfromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();
            txtToDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToShortDateString();
            // release session when refresh page
            Session["PROJ_SearchCriteria"] = null;
        }



        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
           "dllsearch_method", "dllsearch();", true);



        objProp_User.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objProp_User.UserID = Convert.ToInt32(Session["UserID"]);
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.GetUserInfoByID(objProp_User);

        bool isReCalculateLaborExpense = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsReCalculateLaborExpense"]);

        lnkLaborExpenses.Visible = false;

        // Update date-range panel
        if (ddlDateRange.SelectedValue == "1")
        {
            txtfromDate.Visible = false;
            txtToDate.Visible = false;

        }
        else
        {
            if (ddlDateRange.SelectedValue == "2" || ddlDateRange.SelectedValue == "5")
            {
                if (Session["type"].ToString() == "am")
                {
                    lnkLaborExpenses.Visible = true;
                    ViewState["isReCalculateLaborExpense"] = true;
                }
                else if (Session["type"].ToString() != "c")
                {
                    lnkLaborExpenses.Visible = isReCalculateLaborExpense;
                    ViewState["isReCalculateLaborExpense"] = isReCalculateLaborExpense;
                }

                txtfromDate.Visible = true;
                txtToDate.Visible = true;
            }
            else
            {
                txtfromDate.Visible = true;
                txtToDate.Visible = true;
            }
        }
    }

    private Control FindControlRecursive(Control rootControl, string controlID)
    {
        if (rootControl.ID == controlID) return rootControl;

        foreach (Control controlToSearch in rootControl.Controls)
        {
            Control controlToReturn = FindControlRecursive(controlToSearch, controlID);
            if (controlToReturn != null) return controlToReturn;
        }
        return null;
    }

    [Serializable]
    private class SearchCriteria
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }

    protected void ddlProgressBilling_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void LinkButton_Click(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Estimate #")
        {
            Response.Redirect("addestimate.aspx?uid=" + e.CommandArgument + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl));
        }
    }

    private int CreateGanttData(int refID)
    {
        BusinessEntity.Planner objPlanner = new BusinessEntity.Planner();
        BL_Planner objBL_Planner = new BL_Planner();
        #region Planner
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.Ref = refID;

        DataSet BomDS = objBL_Customer.GetBOMItemsByVendor(objProp_Customer);
        DataTable dttBOM = new DataTable();
        dttBOM = BomDS.Tables[0];

        //var pSDate = BomDS.Tables[1].Rows[0][0];
        DateTime ProjSDate = DateTime.Now;
        //if (pSDate != null && pSDate.ToString() != "")
        //{
        //    ProjSDate = Convert.ToDateTime(pSDate.ToString());
        //}

        //DataSet MilestonesDS = objBL_Customer.getJobProject_Milestone(objProp_Customer);
        //DataTable dttMilestone = new DataTable();
        //dttMilestone = MilestonesDS.Tables[0];

        List<PlannerVendorModel> listPlanner = new List<PlannerVendorModel>();

        foreach (DataRow dr in dttBOM.Rows)
        {
            PlannerVendorModel data = new PlannerVendorModel();
            data.ID = Convert.ToInt32(dr["ID"]);
            data.Group = dr["GroupName"].ToString();
            data.Code = string.IsNullOrEmpty(Convert.ToString(dr["CodeDesc"])) ? Convert.ToString(dr["code"])
                : Convert.ToString(dr["code"]) + " - " + Convert.ToString(dr["CodeDesc"]);
            data.fDesc = Convert.ToString(dr["fDesc"]);
            data.Type = "BOM";
            data.Duration = Convert.ToDouble(dr["LabHours"]);
            data.DurationUnit = "h";
            data.StartDate = Convert.ToDateTime(dr["SDate"].ToString());
            data.EndDate = Convert.ToDateTime(dr["EDate"].ToString());
            data.GroupStartDate = Convert.ToDateTime(dr["GroupSDate"].ToString());
            data.GroupEndDate = Convert.ToDateTime(dr["GroupEDate"].ToString());
            data.CodeStartDate = Convert.ToDateTime(dr["CodeSDate"].ToString());
            data.CodeEndDate = Convert.ToDateTime(dr["CodeEDate"].ToString());
            try
            {
                data.VendorID = dr["VendorId"] != null ? Convert.ToInt32(dr["VendorId"]) : 0;
            }
            catch (Exception)
            {
                data.VendorID = 0;
            }

            data.VendorName = dr["Vendor"].ToString();
            data.RootVendorID = refID;
            data.RootVendorName = data.VendorName;
            data.ProjectId = Convert.ToInt32(dr["ProjectId"]);
            data.ProjectName = dr["ProjectName"].ToString();
            listPlanner.Add(data);
        }

        #region Save Data In Planner Table
        //var projectID = Convert.ToInt32(Request.QueryString["uid"]);
        var plannerID = 0;
        objPlanner.ConnConfig = Session["config"].ToString();
        objPlanner.ProjectID = refID;
        objPlanner.Desc = "Vendor " + refID + " Planner";
        objPlanner.Type = "Vendor";
        plannerID = objBL_Planner.AddPlannerNew(objPlanner);
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Planner Created Successfully! ', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
        #endregion  

        var results = (from p in listPlanner
                       group p by p.RootVendorName into g
                       let f = g.FirstOrDefault()
                       where f != null
                       select new
                       {
                           Code = f.RootVendorName,

                           StarDate = g.Min(c => c.GroupStartDate),
                           EndDate = g.Max(c => c.GroupEndDate)
                       }).ToList();


        Int32 Pidx = 0;
        // Reset Notes for tasks
        objPlanner.Desc = "";
        foreach (var dt in results)
        {
            #region Add Parent Data
            String vendor = dt.Code;
            objPlanner.ParentID = Convert.ToInt32("0");
            objPlanner.TaskName = vendor;
            objPlanner.idx = Pidx;
            objPlanner.TaskType = "Vendor";
            objPlanner.ProjectID = 0;
            objPlanner.PlannerID = plannerID;
            objPlanner.Duration = 0;
            objPlanner.DurationUnit = "h";
            objPlanner.StartDate = dt.StarDate.Value;
            objPlanner.EndDate = dt.EndDate.Value;
            objPlanner.Summary = true;
            objPlanner.VendorID = 0;
            objPlanner.VendorName = string.Empty;
            objPlanner.RootVendorID = refID;
            objPlanner.RootVendorName = vendor;
            objPlanner.ProjectName = string.Empty;


            String ParentID = objBL_Planner.AddGanttTasksFromMOM(objPlanner);
            Pidx = Pidx + 1;
            #endregion

            var subProjectList = (from p in listPlanner
                                  where p.RootVendorID == refID
                                  group p by new { p.RootVendorID, p.ProjectId } into g
                                  let f = g.FirstOrDefault()
                                  where f != null
                                  select new
                                  {
                                      Code = f.ProjectName,
                                      ID = f.ProjectId,
                                      //StarDate = g.Min(c => c.CodeStartDate),
                                      //EndDate = g.Max(c => c.CodeEndDate)
                                      StarDate = f.GroupStartDate,
                                      EndDate = f.GroupEndDate
                                  }).ToList();
            Int32 idProj = 0;
            foreach (var proj in subProjectList)
            {
                var projectId = proj.ID;
                objPlanner.TaskName = proj.Code;
                objPlanner.ParentID = Convert.ToInt32(ParentID);
                objPlanner.idx = idProj;
                objPlanner.TaskType = "Project";
                objPlanner.ProjectID = projectId;
                objPlanner.PlannerID = plannerID;
                objPlanner.Duration = 0;
                objPlanner.DurationUnit = "h";
                objPlanner.StartDate = proj.StarDate.Value;
                objPlanner.EndDate = proj.EndDate.Value;
                objPlanner.Summary = true;
                objPlanner.VendorID = 0;
                objPlanner.VendorName = string.Empty;
                objPlanner.RootVendorID = refID;
                objPlanner.RootVendorName = vendor;
                objPlanner.ProjectName = proj.Code;

                String ParentID1 = objBL_Planner.AddGanttTasksFromMOM(objPlanner);
                idProj = idProj + 1;

                var subGroupList = (from p in listPlanner
                                    where p.RootVendorID == refID && p.ProjectId == projectId
                                    group p by new { p.RootVendorID, p.ProjectId, p.Group } into g
                                    let f = g.FirstOrDefault()
                                    where f != null
                                    select new
                                    {
                                        Code = f.Group,
                                        StarDate = f.GroupStartDate,
                                        EndDate = f.GroupEndDate
                                    }).ToList();

                Int32 idGroup = 0;
                foreach (var gr in subGroupList)
                {
                    objPlanner.TaskName = gr.Code;
                    objPlanner.ParentID = Convert.ToInt32(ParentID1);
                    objPlanner.idx = idGroup;
                    objPlanner.TaskType = "Group";
                    objPlanner.ProjectID = projectId;
                    objPlanner.PlannerID = plannerID;
                    objPlanner.Duration = 0;
                    objPlanner.DurationUnit = "h";
                    objPlanner.StartDate = gr.StarDate.Value;
                    objPlanner.EndDate = gr.EndDate.Value;
                    objPlanner.Summary = true;
                    objPlanner.VendorID = 0;
                    objPlanner.VendorName = string.Empty;
                    objPlanner.RootVendorID = refID;
                    objPlanner.RootVendorName = vendor;
                    objPlanner.ProjectName = proj.Code;

                    String ParentID2 = objBL_Planner.AddGanttTasksFromMOM(objPlanner);
                    idGroup = idGroup + 1;
                    //var subOppSequList = (from p in listPlanner
                    //                   where p.Group == Group
                    //                      select p).ToList();
                    var subOppSequList = (from p in listPlanner
                                          where p.RootVendorID == refID && p.ProjectId == projectId && p.Group == gr.Code
                                          group p by new { p.Group, p.Code } into g
                                          let f = g.FirstOrDefault()
                                          where f != null
                                          select new
                                          {
                                              Code = f.Code,
                                              //StarDate = g.Min(c => c.CodeStartDate),
                                              //EndDate = g.Max(c => c.CodeEndDate)
                                              StarDate = f.CodeStartDate,
                                              EndDate = f.CodeEndDate
                                          }).ToList();

                    Int32 idx = 0;
                    foreach (var sdt in subOppSequList)
                    {
                        objPlanner.TaskName = sdt.Code;
                        objPlanner.ParentID = Convert.ToInt32(ParentID2);
                        objPlanner.idx = idx;
                        objPlanner.TaskType = "OpSequence";
                        objPlanner.ProjectID = projectId;
                        objPlanner.PlannerID = plannerID;
                        objPlanner.Duration = 0;
                        objPlanner.DurationUnit = "h";
                        objPlanner.StartDate = sdt.StarDate.Value;
                        objPlanner.EndDate = sdt.EndDate.Value;
                        objPlanner.Summary = true;
                        objPlanner.VendorID = 0;
                        objPlanner.VendorName = string.Empty;
                        objPlanner.RootVendorID = refID;
                        objPlanner.RootVendorName = vendor;
                        objPlanner.ProjectName = proj.Code;
                        String ParentID3 = objBL_Planner.AddGanttTasksFromMOM(objPlanner);
                        idx = idx + 1;

                        #region Add Sub Task Data
                        var subTaskList = (from p in listPlanner
                                           where p.Code == sdt.Code && p.RootVendorID == refID && p.ProjectId == projectId && p.Group == gr.Code
                                           select p).ToList();
                        Int32 idx1 = 0;
                        foreach (var sdt1 in subTaskList)
                        {
                            objPlanner.TaskName = sdt1.fDesc;
                            objPlanner.ParentID = Convert.ToInt32(ParentID3);
                            objPlanner.idx = idx1;
                            objPlanner.TaskType = sdt1.Type;
                            objPlanner.ProjectID = projectId;
                            objPlanner.PlannerID = plannerID;
                            objPlanner.Duration = sdt1.Duration;
                            objPlanner.DurationUnit = sdt1.DurationUnit;
                            objPlanner.StartDate = sdt1.StartDate.HasValue ? sdt1.StartDate.Value : ProjSDate;
                            objPlanner.EndDate = sdt1.EndDate.HasValue ? sdt1.EndDate.Value : ProjSDate;
                            objPlanner.Summary = false;
                            objPlanner.VendorID = sdt1.VendorID;
                            objPlanner.VendorName = sdt1.VendorName;
                            objPlanner.RootVendorID = refID;
                            objPlanner.RootVendorName = vendor;
                            objPlanner.ProjectName = proj.Code;
                            objBL_Planner.AddGanttTasksFromMOM(objPlanner);
                            idx1 = idx1 + 1;
                        }
                        #endregion
                    }
                }
            }
        }

        #endregion

        return plannerID;
    }

    protected void RadGrid_VendorSchedule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BusinessEntity.Planner objPlanner = new BusinessEntity.Planner();
            objPlanner.ConnConfig = Session["config"].ToString();
            DataSet dsPlanner = new DataSet();
            BL_Planner objBL_Planner = new BL_Planner();
            dsPlanner = objBL_Planner.GetAllVendorSchedules(objPlanner);
            RadGrid_VendorSchedule.VirtualItemCount = dsPlanner.Tables[0].Rows.Count;
            RadGrid_VendorSchedule.DataSource = dsPlanner.Tables[0];
        }
        catch (Exception)
        {
            RadGrid_VendorSchedule.VirtualItemCount = 0;
            RadGrid_VendorSchedule.DataSource = String.Empty;
        }
    }

    protected void lnkGenerateVendorSchedule_Click(object sender, EventArgs e)
    {
        CreateGanttData(2613);
    }

    private DataTable GetTemplateIDs(DataTable dt)
    {
        DataTable dtJobT = new DataTable();

        dtJobT = dt.DefaultView.ToTable(true, "Template");

        return dtJobT;

    }

    private DataTable JoinDataTableByColumn(DataTable dtProjectGrid, DataTable dtCustomLabel, string colName)
    {
        foreach (DataColumn dc in dtCustomLabel.Columns)
        {
            if (!dc.ToString().Equals(colName, StringComparison.InvariantCultureIgnoreCase))
            {
                dtProjectGrid.Columns.Add(dc.ColumnName, dc.DataType);
            }
        }

        for (int i = 0; i < dtProjectGrid.Rows.Count; i++)
        {
            var joinRows = dtCustomLabel.AsEnumerable().Where(row2 => row2.Field<int>(colName) == (int)dtProjectGrid.Rows[i][colName]).FirstOrDefault();

            if (joinRows != null)
            {
                foreach (DataColumn dc in joinRows.Table.Columns)
                {
                    if (!dc.ToString().Equals(colName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        dtProjectGrid.Rows[i][dc.ColumnName] = joinRows[dc.ColumnName];

                    }
                }
            }
        }

        return dtProjectGrid;
    }

    #region Export excel with large data
    private DataTable CreateProjectListForExport(DataTable projDT)
    {
        DataTable dt = new DataTable();
        foreach (GridColumn col in RadGrid_Project.Columns)
        {
            if (col.Visible && !string.IsNullOrEmpty(col.HeaderText))
            {
                DataColumn colString = new DataColumn(col.HeaderText);
                colString.DataType = col.DataType;
                dt.Columns.Add(colString);
            }
        }

        if (!dt.Columns.Contains("ID"))
        {
            dt.Columns.Add("ID", typeof(int));
        }

        if (!dt.Columns.Contains("Budgeted Labor"))
        {
            dt.Columns.Add("Budgeted Labor", typeof(double));
        }

        if (!dt.Columns.Contains("Budgeted Hours"))
        {
            dt.Columns.Add("Budgeted Hours", typeof(double));
        }

        if (!dt.Columns.Contains("Remarks"))
        {
            dt.Columns.Add("Remarks", typeof(string));
        }

        if (!dt.Columns.Contains("Address"))
        {
            dt.Columns.Add("Address", typeof(string));
        }

        if (!dt.Columns.Contains("Zip"))
        {
            dt.Columns.Add("Zip", typeof(string));
        }

        if (!dt.Columns.Contains("City"))
        {
            dt.Columns.Add("City", typeof(string));
        }

        if (!dt.Columns.Contains("State"))
        {
            dt.Columns.Add("State", typeof(string));
        }

        var iscompany = dt.Columns.Contains("Company");

        GridColumn routeColumn = RadGrid_Project.MasterTableView.GetColumn("DRoute");

        var routeColHeader = "Route";
        if (routeColumn != null) routeColHeader = routeColumn.HeaderText;

        GridColumn stageColumn = RadGrid_Project.MasterTableView.GetColumn("Stage");

        var stageColHeader = "Stage";
        if (stageColumn != null) stageColHeader = stageColumn.HeaderText;

        GridColumn buildingTypeColumn = RadGrid_Project.MasterTableView.GetColumn("BuildingType");

        var buildingTypeColHeader = "Business Type";
        if (buildingTypeColumn != null) buildingTypeColHeader = buildingTypeColumn.HeaderText;

        //DataRow row = dt.NewRow();
        //dt.Columns["ID"].SetOrdinal = item["ID"];
        dt.Columns["Customer"].SetOrdinal(0);
        dt.Columns["Location"].SetOrdinal(1);
        dt.Columns["Project#"].SetOrdinal(2);
        dt.Columns["Desc"].SetOrdinal(3);
        if (iscompany) dt.Columns["Company"].SetOrdinal(4);
        dt.Columns["Status"].SetOrdinal(5);
        dt.Columns[stageColHeader].SetOrdinal(6);
        dt.Columns["Date Created"].SetOrdinal(7);
        dt.Columns["Service Type"].SetOrdinal(8);
        dt.Columns["Template Type"].SetOrdinal(9);
        dt.Columns["Department"].SetOrdinal(10);
        dt.Columns["Default Salesperson"].SetOrdinal(11);
        dt.Columns[routeColHeader].SetOrdinal(12);
        dt.Columns["Contract Price"].SetOrdinal(13);
        dt.Columns["Not Billed Yet"].SetOrdinal(14);
        dt.Columns["Total Billed"].SetOrdinal(15);
        dt.Columns["Open AR"].SetOrdinal(16);
        dt.Columns["Actual Hours"].SetOrdinal(17);
        dt.Columns["Labor Expense"].SetOrdinal(18);
        dt.Columns["Material Expense"].SetOrdinal(19);
        dt.Columns["Other Expense"].SetOrdinal(20);
        dt.Columns["Total Expenses"].SetOrdinal(21);
        dt.Columns["Total Budgeted Expense"].SetOrdinal(22);
        dt.Columns["Budgeted Labor"].SetOrdinal(23);
        dt.Columns["Budgeted Hours"].SetOrdinal(24);
        dt.Columns["Total PO Order"].SetOrdinal(25);
        dt.Columns["ReceivePO"].SetOrdinal(26);
        dt.Columns["Net Profit"].SetOrdinal(27);
        dt.Columns["% in Profit"].SetOrdinal(28);
        dt.Columns["Remarks"].SetOrdinal(29);
        dt.Columns["Project Manager"].SetOrdinal(30);
        dt.Columns["Supervisor"].SetOrdinal(31);
        dt.Columns["Location Type"].SetOrdinal(32);
        //dt.Columns["Building Type"].SetOrdinal(32);
        dt.Columns[buildingTypeColHeader].SetOrdinal(33);
        dt.Columns["Estimate #"].SetOrdinal(34);
        dt.Columns["Expected Closing Date"].SetOrdinal(35);
        dt.Columns["Address"].SetOrdinal(36);
        dt.Columns["City"].SetOrdinal(37);
        dt.Columns["State"].SetOrdinal(38);
        dt.Columns["Zip"].SetOrdinal(39);
        dt.Columns["ExpenseGL"].SetOrdinal(40);
        dt.Columns["IntersetGL"].SetOrdinal(41);
        dt.Columns["BillingCode"].SetOrdinal(42);
        dt.Columns["BillingCodeGL"].SetOrdinal(43);
        dt.Columns["LaborWage"].SetOrdinal(44);

        foreach (DataRow item in projDT.Rows)
        {
            DataRow row = dt.NewRow();
            row["ID"] = item["ID"];
            row["Customer"] = item["Customer"];
            row["Location"] = item["Tag"];
            row["Address"] = item["Address"];
            row["Zip"] = item["Zip"];
            row["City"] = item["City"];
            row["State"] = item["State"];
            row["Project#"] = item["ID"];
            row["Desc"] = item["fDesc"];
            if (iscompany) row["Company"] = item["Company"];
            row["Status"] = item["Status"];
            row[stageColHeader] = item["Stage"];
            row["Date Created"] = item["fDate"];
            row["Service Type"] = item["CType"];
            row["Template Type"] = item["TemplateDesc"];
            row["Department"] = item["Type"];
            row["Default Salesperson"] = item["Salesperson"];
            row[routeColHeader] = item["Route"];
            row["Contract Price"] = item["ContractPrice"];
            row["Not Billed Yet"] = item["NotBilledYet"];
            row["Total Billed"] = item["NRev"];
            row["Open AR"] = item["OpenARBalance"];
            row["Actual Hours"] = item["NHour"];
            row["Labor Expense"] = item["NLabor"];
            row["Material Expense"] = item["NMat"];
            row["Other Expense"] = item["NOMat"];
            row["Total Expenses"] = item["NCost"];
            row["Total Budgeted Expense"] = item["TotalBudgetedExpense"];
            row["Budgeted Labor"] = item["BLabor"];
            row["Budgeted Hours"] = item["BHour"];
            row["Total PO Order"] = item["NComm"];
            row["ReceivePO"] = item["ReceivePO"];
            row["Net Profit"] = item["NProfit"];
            row["% in Profit"] = item["NRatio"];
            row["Remarks"] = item["Remarks"];
            row["Project Manager"] = item["ProjectManagerUserName"];
            row["Supervisor"] = item["SupervisorUserName"];
            row["Location Type"] = item["LocationType"];
            //row["Building Type"] = item["BuildingType"];
            row[buildingTypeColHeader] = item["BuildingType"];
            row["Estimate #"] = item["estimate"];
            row["Expected Closing Date"] = item["ExpectedClosingDate"];
            row["ExpenseGL"] = item["ExpenseGL"];
            row["IntersetGL"] = item["IntersetGL"];
            row["BillingCode"] = item["BillingCode"];
            row["BillingCodeGL"] = item["BillingCodeGL"];
            row["LaborWage"] = item["LaborWage"];

            dt.Rows.Add(row);
        }

        return dt;
    }

    private List<DataTable> SplitDataTable(DataTable orgDt, int len)
    {
        List<DataTable> retDTs = new List<DataTable>();
        DataTable dt = orgDt.Clone();
        var count = 0;
        foreach (DataRow item in orgDt.Rows)
        {
            count++;
            dt.ImportRow(item);
            if (count >= len)
            {
                DataTable a = dt.Copy();
                retDTs.Add(a);
                dt.Clear();
                count = 0;
            }
        }

        if (retDTs.Count == 0)
        {
            retDTs.Add(dt);
        }
        else if (dt.Rows.Count > 0)
        {
            retDTs.Add(dt);
        }

        return retDTs;
    }

    private void ExportToOxml(bool firstTime, string fileName, DataTable ResultsData)
    {
        //Delete the file if it exists. 
        if (firstTime && File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        uint sheetId = 1; //Start at the first sheet in the Excel workbook.

        if (firstTime)
        {
            //This is the first time of creating the excel file and the first sheet.
            // Create a spreadsheet document by supplying the filepath.
            // By default, AutoSave = true, Editable = true, and Type = xlsx.
            DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadsheetDocument = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.
                Create(fileName, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            DocumentFormat.OpenXml.Packaging.WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            // Add a WorksheetPart to the WorkbookPart.
            var worksheetPart = workbookpart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
            var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
            worksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);


            var bold1 = new DocumentFormat.OpenXml.Spreadsheet.Bold();
            DocumentFormat.OpenXml.Spreadsheet.CellFormat cf = new DocumentFormat.OpenXml.Spreadsheet.CellFormat();


            // Add Sheets to the Workbook.
            DocumentFormat.OpenXml.Spreadsheet.Sheets sheets;
            sheets = spreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

            // Append a new worksheet and associate it with the workbook.
            var sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.
                    GetIdOfPart(worksheetPart),
                SheetId = sheetId,
                Name = "Sheet" + sheetId
            };
            sheets.Append(sheet);

            //Add Header Row.
            var headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
            foreach (DataColumn column in ResultsData.Columns)
            {
                var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell { DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String, CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName) };
                //var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                //if (column.DataType == typeof(Decimal)
                //        || column.DataType == typeof(Double)
                //        || column.DataType == typeof(Int16)
                //        || column.DataType == typeof(Int32)
                //        || column.DataType == typeof(Int64)
                //        || column.DataType == typeof(Single)
                //        || column.DataType == typeof(UInt16)
                //        || column.DataType == typeof(UInt32)
                //        || column.DataType == typeof(UInt64)
                //        )
                //    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                //else
                //    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;

                cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                headerRow.AppendChild(cell);
            }
            sheetData.AppendChild(headerRow);

            foreach (DataRow row in ResultsData.Rows)
            {
                var newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                foreach (DataColumn col in ResultsData.Columns)
                {
                    //var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell
                    //{
                    //    DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String,
                    //    CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(row[col].ToString())
                    //};
                    var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    //if (col.DataType == typeof(DateTime))
                    //    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Date;
                    //else 
                    if (col.ColumnName.ToUpper() == "PROJECT#")
                    {
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                    }
                    else
                    {
                        if (col.DataType == typeof(Decimal)
                            || col.DataType == typeof(Double)
                            || col.DataType == typeof(Int16)
                            || col.DataType == typeof(Int32)
                            || col.DataType == typeof(Int64)
                            || col.DataType == typeof(Single)
                            || col.DataType == typeof(UInt16)
                            || col.DataType == typeof(UInt32)
                            || col.DataType == typeof(UInt64)
                            )
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                        //else if (col.DataType == typeof(DateTime))
                        //    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Date;
                        else
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    }

                    if (col.DataType == typeof(DateTime) && !string.IsNullOrEmpty(row[col].ToString()))
                    {
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(((DateTime)row[col]).ToString("MM/dd/yyyy HH:mm tt"));
                    }
                    else
                    {
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(row[col].ToString());
                    }

                    //cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(row[col].ToString());
                    newRow.AppendChild(cell);
                }

                sheetData.AppendChild(newRow);
            }
            workbookpart.Workbook.Save();

            spreadsheetDocument.Close();
        }
        else
        {
            // Open the Excel file that we created before, and start to add sheets to it.
            var spreadsheetDocument = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(fileName, true);

            var workbookpart = spreadsheetDocument.WorkbookPart;
            if (workbookpart.Workbook == null)
                workbookpart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            var worksheetPart = workbookpart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
            var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
            worksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);
            var sheets = spreadsheetDocument.WorkbookPart.Workbook.Sheets;

            if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Any())
            {
                //Set the new sheet id
                sheetId = sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Max(s => s.SheetId.Value) + 1;
            }
            else
            {
                sheetId = 1;
            }

            // Append a new worksheet and associate it with the workbook.
            var sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.
                    GetIdOfPart(worksheetPart),
                SheetId = sheetId,
                Name = "Sheet" + sheetId
            };
            sheets.Append(sheet);

            //Add the header row here.
            var headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

            foreach (DataColumn column in ResultsData.Columns)
            {
                var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell { DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String, CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName) };
                //var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                //if (column.DataType == typeof(Decimal)
                //        || column.DataType == typeof(Double)
                //        || column.DataType == typeof(Int16)
                //        || column.DataType == typeof(Int32)
                //        || column.DataType == typeof(Int64)
                //        || column.DataType == typeof(Single)
                //        || column.DataType == typeof(UInt16)
                //        || column.DataType == typeof(UInt32)
                //        || column.DataType == typeof(UInt64)
                //        )
                //    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                //else
                //    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;

                cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                headerRow.AppendChild(cell);
            }
            sheetData.AppendChild(headerRow);

            foreach (DataRow row in ResultsData.Rows)
            {
                var newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                foreach (DataColumn col in ResultsData.Columns)
                {
                    //var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell
                    //{
                    //    DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String,
                    //    CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(row[col].ToString())
                    //};
                    var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    //if (col.DataType == typeof(DateTime))
                    //    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Date;
                    //else 
                    if (col.ColumnName.ToUpper() == "PROJECT#")
                    {
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                    }
                    else
                    {
                        if (col.DataType == typeof(Decimal)
                            || col.DataType == typeof(Double)
                            || col.DataType == typeof(Int16)
                            || col.DataType == typeof(Int32)
                            || col.DataType == typeof(Int64)
                            || col.DataType == typeof(Single)
                            || col.DataType == typeof(UInt16)
                            || col.DataType == typeof(UInt32)
                            || col.DataType == typeof(UInt64)
                            )
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                        //else if (col.DataType == typeof(DateTime))
                        //    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Date;
                        else
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    }

                    //cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(row[col].ToString());
                    if (col.DataType == typeof(DateTime) && !string.IsNullOrEmpty(row[col].ToString()))
                    {
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(((DateTime)row[col]).ToString("MM/dd/yyyy HH:mm tt"));
                    }
                    else
                    {
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(row[col].ToString());
                    }
                    newRow.AppendChild(cell);
                }

                sheetData.AppendChild(newRow);
            }

            workbookpart.Workbook.Save();

            // Close the document.
            spreadsheetDocument.Close();
        }
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
            FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            try
            {
                long startBytes = 0;
                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, System.Text.Encoding.UTF8) + lastUpdateTiemStamp;

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.AddHeader("Accept-Ranges", "bytes");
                HttpContext.Current.Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                HttpContext.Current.Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                HttpContext.Current.Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

                //Send data
                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //Dividing the data in 1024 bytes package
                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                int i;
                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                {
                    HttpContext.Current.Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    HttpContext.Current.Response.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _BinaryReader.Close();
                myFile.Close();
                HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                //HttpContext.Current.Response.End();
            }
        }
        catch (FileNotFoundException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }

    }
    #endregion

    #region show/hidden column

    protected void lnkSaveGridSettings_Click(object sender, EventArgs e)
    {
        var columnSettings = GetGridColumnSettings();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "Project.aspx";
        objProp_User.GridId = "RadGrid_Project";

        objBL_User.UpdateUserGridCustomSettings(objProp_User, columnSettings);
    }

    protected void lnkRestoreGridSettings_Click(object sender, EventArgs e)
    {
        var columnSettings = string.Empty;
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "Project.aspx";
        objProp_User.GridId = "RadGrid_Project";

        var ds = objBL_User.DeleteUserGridCustomSettings(objProp_User);

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
            {
                var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (clSetting != null)
                {
                    column.Display = clSetting.Display;
                    if (clSetting.Width != 0)
                        column.HeaderStyle.Width = clSetting.Width;

                    column.OrderIndex = clSetting.OrderIndex;
                }
            }

            RadGrid_Project.MasterTableView.Rebind();
        }
        else
        {
            var colIndex = 0;

            foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                column.Display = true;
                column.OrderIndex = colIndex;
            }

            RadGrid_Project.MasterTableView.SortExpressions.Clear();
            RadGrid_Project.MasterTableView.GroupByExpressions.Clear();
            RadGrid_Project.EditIndexes.Clear();
            RadGrid_Project.Rebind();
        }
    }

    private string GetGridColumnSettings()
    {
        var columnSettings = string.Empty;
        List<ColumnSettings> lstColSetts = new List<ColumnSettings>();

        foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
        {
            var colSett = new ColumnSettings();
            colSett.Name = column.UniqueName;
            colSett.Display = column.Display;
            colSett.Width = (int)column.HeaderStyle.Width.Value;
            colSett.OrderIndex = column.OrderIndex;
            lstColSetts.Add(colSett);
        }

        columnSettings = Newtonsoft.Json.JsonConvert.SerializeObject(lstColSetts);

        return columnSettings;
    }

    private string GetDefaultGridColumnSettingsFromDb()
    {
        var columnSettings = string.Empty;
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.PageName = "Project.aspx";
        objProp_User.GridId = "RadGrid_Project";
        var ds = objBL_User.GetDefaultGridCustomSettings(objProp_User);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
        }

        return columnSettings;
    }

    private void DefineGridStructure()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "Project.aspx";
        objProp_User.GridId = "RadGrid_Project";
        ds = objBL_User.GetGridUserSettings(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            var columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            foreach (GridColumn column in RadGrid_Project.MasterTableView.OwnerGrid.Columns)
            {
                var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (clSetting != null)
                {
                    column.Display = clSetting.Display;
                    if (clSetting.Width != 0)
                        column.HeaderStyle.Width = clSetting.Width;

                    column.OrderIndex = clSetting.OrderIndex;
                }
            }
        }
    }

    public class ColumnSettings
    {
        public string Name { get; set; }
        public bool Display { get; set; }
        public int Width { get; set; }
        public int OrderIndex { get; set; }
    }

    #endregion

    protected void lnkProjectLaborCostReport_Click(object sender, EventArgs e)
    {
        DataTable dtJobs = GetSelectedProjectFromUI();
        Session["dtJobslaborcost"] = dtJobs;
        Response.Redirect("ProjectLaborCostReport.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text);
    }

    private DataTable GetSelectedProjectFromUI()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Template", typeof(int));
            DataSet ds = ProjectList();

            DataTable dtProjectGrid = ds.Tables[0];
            foreach (DataRow item in dtProjectGrid.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = item["ID"];
                dr["Template"] = 0;

                dt.Rows.Add(dr);
            }

        }
        catch { }
        return dt;
    }

    protected void lnkProjectActualVsBudgetReportGable_Click(object sender, EventArgs e)
    {
        string url = "PrintJobActualBudget_NewGable.aspx?sb=" + ddlSearch.SelectedValue + "&sv=" + txtSearch.Text + "&rng=" + ddlDateRange.SelectedValue + "&df=" + txtfromDate.Text + "&dt=" + txtToDate.Text;
        Response.Redirect(url);
    }
}

[Serializable]
public class PlannerVendorModel
{
    public int ID { get; set; }
    public string RootVendorName { get; set; }
    public int RootVendorID { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
    public String Group { get; set; }
    public String Code { get; set; }
    public String fDesc { get; set; }
    public String Type { get; set; }
    public Double Duration { get; set; }
    public string DurationUnit { get; set; }
    public Nullable<DateTime> StartDate { get; set; }
    public Nullable<DateTime> EndDate { get; set; }
    public Nullable<DateTime> GroupStartDate { get; set; }
    public Nullable<DateTime> GroupEndDate { get; set; }
    public Nullable<DateTime> CodeStartDate { get; set; }
    public Nullable<DateTime> CodeEndDate { get; set; }
    public string VendorName { get; set; }
    public int VendorID { get; set; }
}

