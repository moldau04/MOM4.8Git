using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Linq.Dynamic;

public partial class Prospects : System.Web.UI.Page
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

            GetProspectType();
            //FillRecentProspect();
            FillSalesPerson();
            
            #region Show Selected Filter
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["ddlSearch_Lead"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearch_Lead"]);
                    ddlSearch.SelectedValue = selectedValue;

                    var chkInclInactive = Convert.ToBoolean(Session["InclInactiveChk"]);
                    lnkChk.Checked = chkInclInactive;

                    ShowHideFilter();

                    String searchValue = Convert.ToString(Session["ddlSearch_Value_Lead"]);
                    switch (selectedValue.ToLower())
                    {
                        case "p.terr":
                            ddlSalesperson.SelectedValue = searchValue;
                            break;
                        case "r.state":
                            ddlStateSearch.SelectedValue = searchValue;
                            break;
                        case "p.type":
                            ddlTypeSearch.SelectedValue = searchValue;
                            break;

                        case "p.status":
                            ddlStatusSearch.SelectedValue = searchValue;
                            break;
                        case "":
                            txtSearch.Text = searchValue;
                            var daysCompare = Convert.ToString(Session["ddlSearch_Lead_Days"]);
                            ddlcompare.SelectedValue = daysCompare;
                            break;
                        default:
                            txtSearch.Text = searchValue;
                            break;
                    }
                }
            }
            else
            {
                Session["ddlSearch_Lead"] = null;
                Session["ddlSearch_Value_Lead"] = null;
                Session["ddlSearch_Lead_Days"] = null;
                Session["InclInactiveChk"] = null;
            }
            #endregion

            //NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
            //masterSalesMaster.FillPendingLeads();
            //RadGrid_Pro.Rebind();
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

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkProspect");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
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
            lnkDelete.Visible = false;
        }
        //if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        //{
        //    Response.Redirect("home.aspx");
        //}

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

            string UserSalesPermission = ds.Rows[0]["UserSales"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["UserSales"].ToString();
            string ADD = UserSalesPermission.Length < 1 ? "Y" : UserSalesPermission.Substring(0, 1);
            string Edit = UserSalesPermission.Length < 2 ? "Y" : UserSalesPermission.Substring(1, 1);
            string Delete = UserSalesPermission.Length < 2 ? "Y" : UserSalesPermission.Substring(2, 1);
            string View = UserSalesPermission.Length < 4 ? "Y" : UserSalesPermission.Substring(3, 1);
            string Report = UserSalesPermission.Length < 6 ? "Y" : UserSalesPermission.Substring(5, 1);

            // string ResolvedTicketPermission = ds.Rows[0]["Resolve"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Resolve"].ToString();

            // string DeleteResolved = hdnDeleteResolvedTicket.Value = ResolvedTicketPermission.Length < 3 ? "Y" : ResolvedTicketPermission.Substring(2, 1);

            if (ADD == "N")
            {
                lnkAdd.Visible = false;
            }
            if (Edit == "N")
            {
                lnkEdit.Visible = false;
                lnkCopy.Visible = false;
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
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_Pro.Columns[10].Visible = true;
        }
        else
        {
            RadGrid_Pro.Columns[10].Visible = false;
        }
    }


    private void FillSalesPerson()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getTerritory(objPropUser, new GeneralFunctions().GetSalesAsigned());

        ddlSalesperson.DataSource = ds.Tables[0];
        ddlSalesperson.DataTextField = "name";
        ddlSalesperson.DataValueField = "id";
        ddlSalesperson.DataBind();

        ddlSalesperson.Items.Insert(0, new ListItem("Select", "0"));
    }

    #region Grid buttons

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Pro.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            Response.Redirect("addprospect.aspx?uid=" + lblID.Text);
        }

    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem item in RadGrid_Pro.SelectedItems)
            {
                Label lblProspectID = (Label)item.FindControl("lblID");
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.ProspectID = Convert.ToInt32(lblProspectID.Text);

                objBL_Customer.DeleteProspect(objProp_Customer);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDel", "noty({text: 'Lead Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                //BindProspectList();
                RadGrid_Pro.Rebind();
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("addprospect.aspx");
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    #endregion

    public void ResetFormControlValues(Control parent)
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
                    //case "System.Web.UI.WebControls.CheckBox":
                    //    ((CheckBox)c).Checked = false;
                    //    break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                }
            }
        }
    }

    private void GetProspectType()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getProspectType(objProp_Customer);
        //ddlType.DataSource = ds.Tables[0];
        //ddlType.DataTextField = "type";
        //ddlType.DataValueField = "type";
        //ddlType.DataBind();

        ddlTypeSearch.DataSource = ds.Tables[0];
        ddlTypeSearch.DataTextField = "type";
        ddlTypeSearch.DataValueField = "type";
        ddlTypeSearch.DataBind();

        //ddlType.Items.Insert(0, new ListItem("--Select--", ""));
        ddlTypeSearch.Items.Insert(0, new ListItem("Select", ""));

    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        #region Search Filter
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_Lead"] = selectedValue;

        if (selectedValue == "r.name" || selectedValue == "r.city" || selectedValue == "r.address")
        {
            Session["ddlSearch_Value_Lead"] = txtSearch.Text;
        }
        else if (selectedValue == "p.terr")
        {
            Session["ddlSearch_Value_Lead"] = ddlSalesperson.SelectedValue;
        }
        else if (selectedValue == "r.state")
        {
            Session["ddlSearch_Value_Lead"] = ddlStateSearch.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "p.type")
        {
            Session["ddlSearch_Value_Lead"] = ddlTypeSearch.SelectedValue;
        }
        else
        {
            Session["ddlSearch_Value_Lead"] = txtSearch.Text;
        }
        #endregion

        //BindProspectList();
        RadGrid_Pro.Rebind();
    }

    protected void lnkRecent_Click(object sender, EventArgs e)
    {
        LinkButton lnkRecent = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)lnkRecent.NamingContainer;
        Label lblListType = (Label)gvr.FindControl("lblListType");
        Label lblID = (Label)gvr.FindControl("lblID");

        if (lblListType.Text == "0")
        {
            ////FillProspectScreen(lblID.Text);
            //iframeLead.Attributes["src"] = "addprospect.aspx?uid=" + lblID.Text;
            //programmaticModalPopup.Show();
            Response.Redirect("addprospect.aspx?uid=" + lblID.Text);
        }
    }

    public string RecentListIcons(string listtype)
    {
        string imagePath = string.Empty;
        if (listtype == "0")
        {
            imagePath = "images/leader_s.png";
        }
        else if (listtype == "1")
        {
            imagePath = "images/leader_s.png";
        }
        else if (listtype == "2")
        {
            imagePath = "images/leader_s.png";
        }
        return imagePath;
    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowHideFilter();
    }

    private void ShowHideFilter()
    {
        txtSearch.Visible = false;
        ddlStateSearch.Visible = false;
        ddlTypeSearch.Visible = false;
        ddlStatusSearch.Visible = false;
        ddlcompare.Visible = false;
        ddlSalesperson.Visible = false;
        var selectedValue = ddlSearch.SelectedValue;
        switch (selectedValue.ToLower())
        {
            case "p.terr":
                ddlSalesperson.Visible = true;
                ddlSalesperson.SelectedIndex = 0;
                break;
            case "r.state":
                ddlStateSearch.Visible = true;
                ddlStateSearch.SelectedIndex = 0;
                break;
            case "p.type":
                ddlTypeSearch.Visible = true;
                ddlTypeSearch.SelectedIndex = 0;
                break;

            case "p.status":
                ddlStatusSearch.Visible = true;
                ddlStatusSearch.SelectedIndex = 0;
                break;
            case "":
                ddlcompare.Visible = true;
                ddlcompare.SelectedIndex = 0;
                txtSearch.Visible = true;
                txtSearch.Text = string.Empty;
                break;
            default:
                txtSearch.Visible = true;
                txtSearch.Text = string.Empty;
                break;
        }
        
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);
        lnkChk.Checked = false;
        //ddlSearch_SelectedIndexChanged(sender, e);
        ClearGridFilters();
        //BindProspectList();
        RadGrid_Pro.Rebind();
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
            objPropUser.Type = "Lead";
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

    protected void lnkListOFLeadsreport_Click(object sender, EventArgs e)
    {
        objProp_Customer.SearchBy = ddlSearch.SelectedValue;
        if (ddlSearch.SelectedValue == "r.name" || ddlSearch.SelectedValue == "r.city" || ddlSearch.SelectedValue == "r.address" || ddlSearch.SelectedValue == "p.CustomerName")
        {
            objProp_Customer.SearchValue = txtSearch.Text.Trim().Replace("'", "''");
        }
        else if (ddlSearch.SelectedValue == "p.terr")
        {
            objProp_Customer.SearchValue = ddlSalesperson.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "r.state")
        {
            objProp_Customer.SearchValue = ddlStateSearch.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "p.type")
        {
            objProp_Customer.SearchValue = ddlTypeSearch.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "p.status")
        {
            objProp_Customer.SearchValue = ddlStatusSearch.SelectedValue;
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

        Response.Redirect("ListOfLeadsReport.aspx?SearchBy=" + objProp_Customer.SearchBy + "&SearchValue=" + objProp_Customer.SearchValue);
    }

    protected void lnkListOfLeadsByContactReport_Click(object sender, EventArgs e)
    {
        objProp_Customer.SearchBy = ddlSearch.SelectedValue;
        if (ddlSearch.SelectedValue == "r.name" || ddlSearch.SelectedValue == "r.city" || ddlSearch.SelectedValue == "r.address" || ddlSearch.SelectedValue == "p.CustomerName")
        {
            objProp_Customer.SearchValue = txtSearch.Text.Trim().Replace("'", "''");
        }
        else if (ddlSearch.SelectedValue == "p.terr")
        {
            objProp_Customer.SearchValue = ddlSalesperson.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "r.state")
        {
            objProp_Customer.SearchValue = ddlStateSearch.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "p.type")
        {
            objProp_Customer.SearchValue = ddlTypeSearch.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "p.status")
        {
            objProp_Customer.SearchValue = ddlStatusSearch.SelectedValue;
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

        Response.Redirect("ListOfLeadsByContact.aspx?SearchBy=" + objProp_Customer.SearchBy + "&SearchValue=" + objProp_Customer.SearchValue);
    }

    protected void RadGrid_Pro_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_Pro.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Pro_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Pro.MasterTableView.OwnerGrid.Columns)
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

            Session["Pro_Filters"] = filters;
        }
        else
        {
            Session["Pro_FilterExpression"] = null;
            Session["Pro_Filters"] = null;
        }
        #endregion  

        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Pro);

        foreach (GridDataItem gr in RadGrid_Pro.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            HyperLink lnkCustomerName = (HyperLink)gr.FindControl("lnkCustomerName");
            lnkCustomerName.NavigateUrl = "addprospect.aspx?uid=" + lblID.Text;
            gr.Attributes["ondblclick"] = "location.href='addprospect.aspx?uid=" + lblID.Text + "'";
        }
    }
    bool isGrouping = false;

    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Pro.MasterTableView.FilterExpression != "" ||
            (RadGrid_Pro.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Pro.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Pro_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Pro.AllowCustomPaging = !ShouldApplySortFilterOrGroup();

        
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Pro_FilterExpression"] != null && Convert.ToString(Session["Pro_FilterExpression"]) != "" && Session["Pro_Filters"] != null)
                {
                    RadGrid_Pro.MasterTableView.FilterExpression = Convert.ToString(Session["Pro_FilterExpression"]);
                    var filtersGet = Session["Pro_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_Pro.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["Pro_FilterExpression"] = null;
                Session["Pro_Filters"] = null;
            }
        }
        #endregion
        BindProspectList();
    }

    private DataTable GetFilteredDataSource()
    {
        DataTable DT = new DataTable();
        DataTable FilteredDT = new DataTable();
        string filterexpression = string.Empty;
        filterexpression = RadGrid_Pro.MasterTableView.FilterExpression;

        try
        {
            if (filterexpression != "")
            {
                DT = (DataTable)RadGrid_Pro.DataSource;
                FilteredDT = DT.AsEnumerable()
                .AsQueryable()
                .Where(filterexpression)
                .CopyToDataTable();
                return FilteredDT;
            }
            else
            {
                return (DataTable)RadGrid_Pro.DataSource;
            }
        }
        catch { return (DataTable)RadGrid_Pro.DataSource; }
    }

    private void BindProspectList()
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = ddlSearch.SelectedValue;
        if (ddlSearch.SelectedValue == "r.name" || ddlSearch.SelectedValue == "r.city" || ddlSearch.SelectedValue == "r.address" || ddlSearch.SelectedValue == "p.CustomerName")
        {
            objProp_Customer.SearchValue = txtSearch.Text.Trim().Replace("'", "''");
        }
        else if (ddlSearch.SelectedValue == "p.terr")
        {
            objProp_Customer.SearchValue = ddlSalesperson.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "r.state")
        {
            objProp_Customer.SearchValue = ddlStateSearch.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "p.type")
        {
            objProp_Customer.SearchValue = ddlTypeSearch.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "p.status")
        {
            objProp_Customer.SearchValue = ddlStatusSearch.SelectedValue;
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
        #region Company Check
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

        if (lnkChk.Checked == true)
        {
            objProp_Customer.Status = 1;
        }
        else
        {
            objProp_Customer.Status = 0;
        }
        ds = objBL_Customer.getProspect(objProp_Customer, new GeneralFunctions().GetSalesAsigned());
        //RadGrid_Pro.VirtualItemCount = ds.Tables[0].Rows.Count;
        Session["prospects"] = ds.Tables[0];
        RadGrid_Pro.DataSource = ds.Tables[0];
        //lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString() + " Record(s) found";
        var newCount = GetFilteredDataSource().Rows.Count;
        RadGrid_Pro.VirtualItemCount = newCount;
        lblRecordCount.Text = newCount + " Record(s) found";
        UpdateSearchInfoSessions();
        //upSearch.Update();
    }


    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Pro.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            Response.Redirect("addprospect.aspx?uid=" + lblID.Text + "&t=c");
        }
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);
        lnkChk.Checked = false;
        ddlSearch_SelectedIndexChanged(sender, e);
        ClearGridFilters();
        //BindProspectList();
        RadGrid_Pro.Rebind();
    }

    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        //BindProspectList();
        RadGrid_Pro.Rebind();
    }
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Pro.ExportSettings.FileName = "Leads";
        RadGrid_Pro.ExportSettings.IgnorePaging = true;
        RadGrid_Pro.ExportSettings.ExportOnlyData = true;
        RadGrid_Pro.ExportSettings.OpenInNewWindow = true;
        RadGrid_Pro.ExportSettings.HideStructureColumns = true;
        RadGrid_Pro.MasterTableView.UseAllDataFields = true;
        RadGrid_Pro.MasterTableView.GetColumn("Remarks").Display = true;
        RadGrid_Pro.MasterTableView.GetColumn("Zip").Display = true;
        RadGrid_Pro.MasterTableView.GetColumn("BusinessType").Visible = true;
        RadGrid_Pro.MasterTableView.GetColumn("BusinessType").HeaderText = getBusinessTypeLabel();
        RadGrid_Pro.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Pro.MasterTableView.ExportToExcel();
    }
    protected void RadGrid_Pro_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 3;
        else
            currentItem = 4;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Pro.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Pro.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
    protected void RadGrid_Pro_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (Convert.ToString(RadGrid_Pro.MasterTableView.FilterExpression) != "")
                    lblRecordCount.Text = totalCount + " Record(s) found";
                else
                    lblRecordCount.Text = RadGrid_Pro.VirtualItemCount + " Record(s) found";
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

    private String getBusinessTypeLabel()
    {
        BL_Customer objBL_Customer = new BL_Customer();
        Customer objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getBT(objCustomer);
        try
        {
            return ds.Tables[0].Rows[0]["Label"].ToString();
        }
        catch
        {
            return "Business Type";
        }
    }

    // Clear grid filter
    private void ClearGridFilters()
    {
        if (Session["Pro_FilterExpression"] != null && Convert.ToString(Session["Pro_FilterExpression"]) != "" && Session["Pro_Filters"] != null)
        {
            foreach (GridColumn column in RadGrid_Pro.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            RadGrid_Pro.MasterTableView.FilterExpression = string.Empty;
        }

        Session["Pro_FilterExpression"] = null;
        Session["Pro_Filters"] = null;
    }

    private void UpdateSearchInfoSessions()
    {
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_Lead"] = selectedValue;
        Session["InclInactiveChk"] = lnkChk.Checked;

        switch (selectedValue.ToLower())
        {
            case "p.terr":
                Session["ddlSearch_Value_Lead"] = ddlSalesperson.SelectedValue;
                break;
            case "r.state":
                Session["ddlSearch_Value_Lead"] = ddlStateSearch.SelectedValue;
                break;
            case "p.type":
                Session["ddlSearch_Value_Lead"] = ddlTypeSearch.SelectedValue;
                break;
            
            case "p.status":
                Session["ddlSearch_Value_Lead"] = ddlStatusSearch.SelectedValue;
                break;
            case "":
                Session["ddlSearch_Value_Lead"] = txtSearch.Text;
                Session["ddlSearch_Lead_Days"] = ddlcompare.SelectedValue;
                break;
            default:
                Session["ddlSearch_Value_Lead"] = txtSearch.Text;
                break;
        }


    }
}

