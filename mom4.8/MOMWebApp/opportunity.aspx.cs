using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Linq.Dynamic;
using System.Linq;
using System.Configuration;

public partial class opportunity : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
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

            FillOpportunityStatus();
            // fillDates();
            FillUsers();
            #region Show Selected Filter
            if (Request.QueryString["fil"] != null)
            {
                IsGridPageIndexChanged = true;
                if (Session["ddlSearch"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearch"]);
                    ddlSearch.SelectedValue = selectedValue;
                    ShowHideFilterSearch();

                    if (Session["from_opp"] != null && Session["end_opp"] != null)
                    {
                        txtFrom.Text = Convert.ToString(Session["from_opp"]);
                        txtTo.Text = Convert.ToString(Session["end_opp"]);
                    }

                    String searchValue = Convert.ToString(Session["ddlSearch_Value"]);
                    if (selectedValue == "r.name" || selectedValue == "l.fdesc")
                    {
                        txtSearch.Text = searchValue;
                    }
                    else if (selectedValue == "l.status")
                    {
                        ddlStatus.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "l.Probability")
                    {
                        ddlProbab.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "l.fuser")
                    {
                        ddlAssigned.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "l.CloseDate")
                    {
                        //do nothing ..
                    }
                    else
                    {
                        txtSearch.Text = searchValue;
                    }
                }
            }

            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["from_opp"] == null && Session["end_opp"] == null)
                {
                    //txtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    //txtTo.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                    txtFrom.Text = string.Empty;
                    txtTo.Text = string.Empty;
                }
                else
                {
                    txtFrom.Text = Session["from_opp"].ToString();
                    txtTo.Text = Session["end_opp"].ToString();
                }
            }
            else
            {
                Session["ddlSearch"] = null;
                Session["ddlSearch_Value"] = null;

                txtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtTo.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                Session["from_opp"] = txtFrom.Text;
                Session["end_opp"] = txtTo.Text;
            }
            #endregion

            string[] tokens = Session["config"].ToString().Split(';');
            if (tokens[1].ToString().ToLower().IndexOf("transel") != -1 || ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("transel"))
            {
                lnkOpportunityForecastReport.Visible = true;
                lnkOpportunityForecast1Report.Visible = true;
            }
            else
            {
                lnkOpportunityForecastReport.Visible = false;
                lnkOpportunityForecast1Report.Visible = false;
            }
        }

        //Permission();
        UserPermission();
        HighlightSideMenu();
        CompanyPermission();
        ConvertToJSON();
    }

    private void HighlightSideMenu()
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkOpportunities");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        div.Style.Add("display", "block");
    }

    private void FillOpportunityStatus()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();

        ds = objBL_Customer.getOpportunityStatus(objProp_Customer);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlStatus.DataSource = ds.Tables[0];
            ddlStatus.DataTextField = "Name";
            ddlStatus.DataValueField = "ID";
            ddlStatus.DataBind();
        }

    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            //RadGrid_Opportunity.Columns[11].Visible = true;
            RadGrid_Opportunity.Columns.FindByDataField("Company").Visible = true;
        }
        else
        {
            //RadGrid_Opportunity.Columns[11].Visible = false;
            RadGrid_Opportunity.Columns.FindByDataField("Company").Visible = false;
        }
    }

    protected void LinkButton_Click(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Estimate #")
        {
            Response.Redirect("addestimate.aspx?uid=" + e.CommandArgument + "&page=opp");
        }
        else if (e.CommandName == "Project #")
        {
            Response.Redirect("addProject.aspx?uid=" + e.CommandArgument);
        }
    }

    private void FillUsers()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        // ds = objBL_User.getSalesPerson(objPropUser, new GeneralFunctions().GetSalesAsigned());
        ds = objBL_User.getTerritory(objPropUser, new GeneralFunctions().GetSalesAsigned(), 0,0, "t.SDesc");
        ddlAssigned.DataSource = ds.Tables[0];
        ddlAssigned.DataTextField = "SDesc";
        ddlAssigned.DataValueField = "SDesc";
        ddlAssigned.DataBind();
        ddlAssigned.Items.Insert(0, new ListItem("None", ""));
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

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.Master.FindControl("lnkOpportunities");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
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

            string ProposalPermission = ds.Rows[0]["Proposal"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Proposal"].ToString();
            string ADD = ProposalPermission.Length < 1 ? "Y" : ProposalPermission.Substring(0, 1);
            string Edit = ProposalPermission.Length < 2 ? "Y" : ProposalPermission.Substring(1, 1);
            string Delete = ProposalPermission.Length < 2 ? "Y" : ProposalPermission.Substring(2, 1);
            string View = ProposalPermission.Length < 4 ? "Y" : ProposalPermission.Substring(3, 1);
            string Report = ProposalPermission.Length < 6 ? "Y" : ProposalPermission.Substring(5, 1);

            if (ADD == "N")
            {
                lnknAdd.Visible = false;
                lnkCopy.Visible = false;
            }
            if (Edit == "N")
            {
                lnkEdit.Visible = false;
            }
            if (Delete == "N")
            {
                lnkDelete.Visible = false;

            }
            if (Report == "N")
            {
                lnkReport.Visible = false;
                lnkExcel.Visible = false;
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
        ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        if (hdnCssActive.Value == "CssActive")
        {
            Session["lblOppActive"] = "1";
        }
        else
        {
            Session["lblOppActive"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }

        #region Search Filter
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch"] = selectedValue;


        Session["from_opp"] = txtFrom.Text;
        Session["end_opp"] = txtTo.Text;


        //if (selectedValue == "r.name" || selectedValue == "l.fdesc" || selectedValue == "l.ID" || selectedValue == "l.estimate" || selectedValue == "job")
        if (selectedValue == "r.name" || selectedValue == "l.fdesc" || selectedValue == "l.ID")
        {
            Session["ddlSearch_Value"] = txtSearch.Text;
        }
        else if (selectedValue == "l.status")
        {
            Session["ddlSearch_Value"] = ddlStatus.SelectedValue;
        }
        else if (selectedValue == "l.Probability")
        {
            Session["ddlSearch_Value"] = ddlProbab.SelectedValue;
        }
        else if (selectedValue == "l.fuser")
        {
            Session["ddlSearch_Value"] = ddlAssigned.SelectedValue;
        }
        else if (selectedValue == "l.CloseDate")
        {
            Session["ddlSearch_Value"] = "CloseDate";
        }
        else
        {
            Session["ddlSearch_Value"] = txtSearch.Text;
        }
        #endregion

        OpportunityList();
        RadGrid_Opportunity.Rebind();
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        Session["ddlSearch"] = null;
        Session["ddlSearch_Value"] = null;
        Session["from_opp"] = null;
        Session["end_opp"] = null;
        objGeneralFunctions.ResetFormControlValues(this);
        // Clear filters on grid
        if (Session["Opp_FilterExpression"] != null && Convert.ToString(Session["Opp_FilterExpression"]) != "" && Session["Opp_Filters"] != null)
        {
            foreach (GridColumn column in RadGrid_Opportunity.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            RadGrid_Opportunity.MasterTableView.FilterExpression = string.Empty;
        }

        // Remove session of filters
        Session["Opp_FilterExpression"] = null;
        Session["Opp_Filters"] = null;
        //fillDates();
        // ddlSearch_SelectedIndexChanged(sender, e);
        OpportunityList();
        RadGrid_Opportunity.Rebind();
        ShowHideFilterSearch();
    }

    //protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    String selectedValue = ddlSearch.SelectedValue;
    //    ShowHideFilterSearch(selectedValue);
    //}

    private void ShowHideFilterSearch()
    {
      
        ddlStatus.Style.Add("display", "none");
        txtSearch.Style.Add("display", "none");
        ddlProbab.Style.Add("display", "none");
        ddlAssigned.Style.Add("display", "none");
      if(ddlSearch.SelectedValue == "l.status")
        {
            ddlStatus.Style.Add("display", "block");
        }
        else if (ddlSearch.SelectedValue == "l.Probability")
        {
            ddlProbab.Style.Add("display", "block");
        }
        else if (ddlSearch.SelectedValue == "l.fuser")
        {
            ddlAssigned.Style.Add("display", "block");
        }
        else if (ddlSearch.SelectedValue == "l.CloseDate")
        {
           //do nothing
        }
        else
        {
            txtSearch.Style.Add("display", "block");
        }

        //if (selectedValue == "r.name" || selectedValue == "l.fdesc")
        //{
        //    txtSearch.Visible = true;
        //    ddlProbab.Visible = false;
        //    ddlStatus.Visible = false;
        //    ddlAssigned.Visible = false;
        //    txtSearch.Text = "";
        //}
        //else if (selectedValue == "l.status")
        //{
        //    txtSearch.Visible = false;
        //    ddlProbab.Visible = false;
        //    ddlStatus.Visible = true;
        //    ddlAssigned.Visible = false;
        //    ddlStatus.SelectedIndex = 0;
        //}
        //else if (selectedValue == "l.Probability")
        //{
        //    txtSearch.Visible = false;
        //    ddlProbab.Visible = true;
        //    ddlStatus.Visible = false;
        //    ddlAssigned.Visible = false;

        //}
        //else if (selectedValue == "l.fuser")
        //{
        //    txtSearch.Visible = false;
        //    ddlProbab.Visible = false;
        //    ddlStatus.Visible = false;
        //    ddlAssigned.Visible = true;
        //    ddlAssigned.SelectedIndex = 0;
        //}
        //else if (selectedValue == "l.CloseDate")
        //{
        //    txtSearch.Visible = false;
        //    ddlProbab.Visible = false;
        //    ddlStatus.Visible = false;
        //    ddlAssigned.Visible = false;
        //}
        //else
        //{
        //    txtSearch.Visible = true;
        //    txtSearch.Text = "";
        //    ddlProbab.Visible = false;
        //    ddlStatus.Visible = false;
        //    ddlAssigned.Visible = false;
        //}
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem item in RadGrid_Opportunity.SelectedItems)
            {
                Label lblID = (Label)item.FindControl("lblId");
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.OpportunityID = Convert.ToInt32(lblID.Text);
                objBL_Customer.DeleteOpportunity(objProp_Customer);
                OpportunityList();
                RadGrid_Opportunity.Rebind();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyS", "noty({text: 'Opportunity deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Opportunity.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            Response.Redirect("addopprt.aspx?uid=" + lblID.Text);
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
            objPropUser.Type = "Opportunity";
            dsGetReports = objBL_ReportsData.GetStockReports(objPropUser);
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

    protected void RadGrid_Opportunity_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_Opportunity.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Opp_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Opportunity.MasterTableView.OwnerGrid.Columns)
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

            Session["Opp_Filters"] = filters;
        }
        else
        {
            Session["Opp_FilterExpression"] = null;
            Session["Opp_Filters"] = null;
        }
        #endregion  

        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Opportunity);
        foreach (GridDataItem gr in RadGrid_Opportunity.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            gr.Attributes["ondblclick"] = "location.href='addopprt.aspx?uid=" + lblID.Text + "'";
        }
    }

    protected void RadGrid_Opportunity_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Opportunity.AllowCustomPaging = !ShouldApplySortFilterOrGroup();

        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Opp_FilterExpression"] != null && Convert.ToString(Session["Opp_FilterExpression"]) != "" && Session["Opp_Filters"] != null)
                {
                    RadGrid_Opportunity.MasterTableView.FilterExpression = Convert.ToString(Session["Opp_FilterExpression"]);
                    var filtersGet = Session["Opp_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_Opportunity.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["Opp_FilterExpression"] = null;
                Session["Opp_Filters"] = null;
            }

        }
        else
        {
            String filterExpression = Convert.ToString(RadGrid_Opportunity.MasterTableView.FilterExpression);
            if (filterExpression != "")
            {
                Session["Opp_FilterExpression"] = filterExpression;
                List<RetainFilter> filters = new List<RetainFilter>();

                foreach (GridColumn column in RadGrid_Opportunity.MasterTableView.OwnerGrid.Columns)
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

                Session["Opp_Filters"] = filters;
            }
            else
            {
                Session["Opp_FilterExpression"] = null;
                Session["Opp_Filters"] = null;
            }
        }
        #endregion

        OpportunityList();
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Opportunity.MasterTableView.FilterExpression != "" ||
            (RadGrid_Opportunity.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Opportunity.MasterTableView.SortExpressions.Count > 0;
    }

    private void OpportunityList()
    {
        if (!IsGridPageIndexChanged)
        {
            RadGrid_Opportunity.CurrentPageIndex = 0;
            Session["RadGrid_OpportunityCurrentPageIndex"] = 0;
            ViewState["RadGrid_OpportunityminimumRows"] = 0;
            ViewState["RadGrid_OpportunitymaximumRows"] = RadGrid_Opportunity.PageSize;
        }
        else
        {
            if (Session["RadGrid_OpportunityCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_OpportunityCurrentPageIndex"].ToString()) != 0
                && Request.QueryString["fil"] != null && Request.QueryString["fil"].ToString() == "1")
            {
                RadGrid_Opportunity.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_OpportunityCurrentPageIndex"].ToString());
                ViewState["RadGrid_OpportunityminimumRows"] = RadGrid_Opportunity.CurrentPageIndex * RadGrid_Opportunity.PageSize;
                ViewState["RadGrid_OpportunitymaximumRows"] = (RadGrid_Opportunity.CurrentPageIndex + 1) * RadGrid_Opportunity.PageSize;

            }
        }

        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = ddlSearch.SelectedValue;

        if (ddlSearch.SelectedValue == "r.name" || ddlSearch.SelectedValue == "l.fdesc" || ddlSearch.SelectedValue == "l.CompanyName" || ddlSearch.SelectedValue == "l.ID")
        {
            objProp_Customer.SearchValue = txtSearch.Text.Trim().Replace("'", "''");
        }
        else if (ddlSearch.SelectedValue == "l.status")
        {
            objProp_Customer.SearchValue = ddlStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "l.Probability")
        {
            objProp_Customer.SearchValue = ddlProbab.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "l.fuser")
        {
            objProp_Customer.SearchValue = ddlAssigned.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "l.fuser")
        {
            objProp_Customer.SearchValue = ddlAssigned.SelectedValue;
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

        #region Company Checkjob
        objProp_Customer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objProp_Customer.EN = 1;
        }
        else
        {
            objProp_Customer.EN = 0;
        }
        #endregion

        ds = objBL_Customer.getOpportunityNew(objProp_Customer, new GeneralFunctions().GetSalesAsigned());

        DataTable result = ProcessDataFilter(ds.Tables[0]);

        RadGrid_Opportunity.VirtualItemCount = result.Rows.Count;
        RadGrid_Opportunity.DataSource = result;

        Session["opps"] = result;
    }

    private DataTable ProcessDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Session["Opp_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                var filtersGet = Session["Opp_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Opportunity.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                        if (column != null)
                        {
                            if(column.UniqueName == "id" || column.UniqueName == "revenue" || column.UniqueName == "BidPrice" 
                                || column.UniqueName == "FinalBid" || column.UniqueName == "estimate" || column.UniqueName == "job" || column.UniqueName == "closedate")
                            {
                                sql = sql + " And " + column.UniqueName + " = " + _filter.FilterValue;
                            }
                            else
                            {
                                sql = sql + " And " + column.UniqueName + " like '%" + _filter.FilterValue + "%'";
                            }
                        }
                    }
                }

                return result.Select(sql).CopyToDataTable();
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

    protected void RadGrid_Opportunity_ItemCreated(object sender, GridItemEventArgs e)
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

    protected void RadGrid_Opportunity_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }

        lblRecordCount.Text = rowCount + " Record(s) found";
       // updpnl.Update();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Opportunity.ExportSettings.FileName = "Opportunity";
        RadGrid_Opportunity.ExportSettings.IgnorePaging = true;
        RadGrid_Opportunity.ExportSettings.ExportOnlyData = true;
        RadGrid_Opportunity.ExportSettings.OpenInNewWindow = true;
        RadGrid_Opportunity.ExportSettings.HideStructureColumns = true;
        RadGrid_Opportunity.MasterTableView.UseAllDataFields = true;
        RadGrid_Opportunity.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Opportunity.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_Opportunity_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Opportunity.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Opportunity.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

    protected void lnkOpportunityReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("OpportunityReport.aspx");
    }

    protected void lnkOpportunityForecastReport_Click(object sender, EventArgs e)
    {
        string urlString = "OpportunityForecastReport.aspx?sd=" + txtFrom.Text + "&ed=" + txtTo.Text + "&type=0";

        Response.Redirect(urlString, true);
    }

    protected void lnkOpportunityForecast1Report_Click(object sender, EventArgs e)
    {
        string urlString = "OpportunityForecastReport.aspx?sd=" + txtFrom.Text + "&ed=" + txtTo.Text + "&type=1";

        Response.Redirect(urlString, true);
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        // Clear all search values
        objGeneralFunctions.ResetFormControlValues(this);
        Session["ddlSearch"] = null;
        Session["ddlSearch_Value"] = null;
        // Reset value for daterange
        //txtFrom.Text = Session["from_opp"].ToString();
        //txtTo.Text = Session["end_opp"].ToString();

        if (Session["from_opp"]==null && Session["end_opp"] == null)
        {
            txtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtTo.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
        }
        else
        {
            if (String.IsNullOrEmpty(Session["from_opp"].ToString()) && String.IsNullOrEmpty(Session["end_opp"].ToString()))
            {
                txtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtTo.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            else
            {
                txtFrom.Text = Session["from_opp"].ToString();
                txtTo.Text = Session["end_opp"].ToString();
            }
               
        }

        // Clear filters on grid
        if (Session["Opp_FilterExpression"] != null && Convert.ToString(Session["Opp_FilterExpression"]) != "" && Session["Opp_Filters"] != null)
        {
            foreach (GridColumn column in RadGrid_Opportunity.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            RadGrid_Opportunity.MasterTableView.FilterExpression = string.Empty;
        }

        // Remove session of filters
        Session["Opp_FilterExpression"] = null;
        Session["Opp_Filters"] = null;
        // Update search form by selection change event
        //ddlSearch_SelectedIndexChanged(sender, e);
        RadGrid_Opportunity.Rebind();
        ShowHideFilterSearch();
    }

    protected void RadGrid_Opportunity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Clear();
        dt.Columns.Add("EstimateID");
        dt.Columns.Add("Last");

        DataTable dtProject = new DataTable();
        dtProject.Clear();
        dtProject.Columns.Add("ProjectID");
        dtProject.Columns.Add("Last");

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

            Repeater projectRepeater = e.Item.FindControl("rptProjects") as Repeater;
            HiddenField hdnGridProject = e.Item.FindControl("hdnGridProject") as HiddenField;
            if (hdnGridProject != null && !string.IsNullOrEmpty(hdnGridProject.Value))
            {
                var projArr = hdnGridProject.Value.Trim().Split(',');
                for (int i = 0; i < projArr.Length; i++)
                {
                    DataRow _temp = dtProject.NewRow();
                    _temp["ProjectID"] = projArr[i].Trim();
                    if (i == projArr.Length - 1)
                    {
                        _temp["Last"] = "true";
                    }
                    else
                    {
                        _temp["Last"] = "false";
                    }

                    dtProject.Rows.Add(_temp);
                }
            }
            //Get the instance of the right type
            if (projectRepeater != null)
            {
                projectRepeater.DataSource = dtProject;
                projectRepeater.DataBind();
            }
        }
    }

    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Opportunity.SelectedItems)
        {
            TableCell cell = di["ClientSelectColumn"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblID = (Label)di.FindControl("lblId");

            if (chkSelect.Checked == true)
            {
                Response.Redirect("AddOpprt.aspx?uid=" + lblID.Text + "&t=c");
            }
        }
    }

    private DataTable GetFilteredDataSource(RadGrid radGrid)
    {
        DataTable DT = new DataTable();
        DataTable FilteredDT;
        string filterexpression = string.Empty;
        filterexpression = radGrid.MasterTableView.FilterExpression;
        DT = (DataTable)radGrid.DataSource;
        try
        {
            FilteredDT = DT.AsEnumerable().AsQueryable()
                .Where(filterexpression)
                .CopyToDataTable();
        }
        catch (Exception)
        {
            FilteredDT = new DataTable();
        }

        return FilteredDT;
    }

    protected void RadGrid_Opportunity_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGrid_OpportunityCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_OpportunityminimumRows"] = e.NewPageIndex * RadGrid_Opportunity.PageSize;
            ViewState["RadGrid_OpportunitymaximumRows"] = (e.NewPageIndex + 1) * RadGrid_Opportunity.PageSize;
        }
        catch { }
    }

    protected void RadGrid_Opportunity_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGrid_OpportunityminimumRows"] = RadGrid_Opportunity.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_OpportunitymaximumRows"] = (RadGrid_Opportunity.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }
}
