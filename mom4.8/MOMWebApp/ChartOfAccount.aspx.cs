using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Collections;
using System.Configuration;

public partial class ChartOfAccount : System.Web.UI.Page
{
    #region Variables

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();
    BL_User objBL_User = new BL_User();
    AccountType _objAcType = new AccountType();
    BL_AccountType _objBLAcType = new BL_AccountType();

    private const string _asc = " ASC";
    private const string _desc = " DESC";
    Double dPageTotal = 0;
    Double cPageTotal = 0;
    private static int intExportExcel = 0;
    #endregion

    #region Events

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

                if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("gable"))
                {
                    lnkDashboardReport.Visible = true;
                }

                txtSearch.Visible = false;
                ddlBalanceCondition.Visible = false;
                RecalCOA();
                FillStatus();
                FillAccountType();
                FillSubAccount();
                #region Show Selected Filter
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["ddlSearch_Value_COAType"] != null)
                    {
                        string selectedType = Convert.ToString(Session["ddlSearch_Value_COAType"]);
                        ddlType.SelectedValue = selectedType;
                    }
                    if (Session["ddlSearch_Value_COACategory"] != null)
                    {
                        string selectedCategory = Convert.ToString(Session["ddlSearch_Value_COACategory"]);
                        ddlSubAcCategory.SelectedItem.Text = selectedCategory;
                    }
                    if (Session["ddlSearch_Value_COAStatus"] != null)
                    {
                        string selectedstatus = Convert.ToString(Session["ddlSearch_Value_COAStatus"]);
                        ddlStatus.SelectedValue = selectedstatus;
                    }
                    if (Session["ddlSearch_COA"] != null)
                    {
                        string selectedValue = Convert.ToString(Session["ddlSearch_COA"]);
                        ddlSearch.SelectedValue = selectedValue;

                        string searchValueCOA = Convert.ToString(Session["ddlSearch_Value_COA"]);
                        string SearchValueCOABalnace = Convert.ToString(Session["ddlSearch_Value_COABal"]);
                        if (selectedValue == "0")
                        {
                            txtSearch.Visible = false;
                            ddlBalanceCondition.Visible = false;
                        }
                        else if (selectedValue == "1")
                        {
                            ddlSearch_SelectedIndexChanged(sender, e);
                            txtSearch.Text = searchValueCOA;
                        }
                        else if (selectedValue == "2")
                        {
                            ddlSearch_SelectedIndexChanged(sender, e);
                            txtSearch.Text = searchValueCOA;
                        }
                        else if (selectedValue == "3")
                        {
                            ddlSearch_SelectedIndexChanged(sender, e);
                            txtSearch.Text = searchValueCOA;
                            ddlBalanceCondition.SelectedItem.Text = SearchValueCOABalnace;
                        }
                        else if (selectedValue == "4")
                        {
                            ddlSearch_SelectedIndexChanged(sender, e);
                            txtSearch.Text = searchValueCOA;
                        }
                    }

                }
                else
                {
                    Session["ddlSearch_COA"] = null;
                    Session["ddlSearch_Value_COA"] = null;
                    Session["ddlSearch_Value_COABal"] = null;
                    Session["ddlSearch_Value_COAType"] = null;
                    Session["ddlSearch_Value_COACategory"] = null;
                    Session["ddlSearch_Value_COAStatus"] = null;
                }
                #endregion
            }
            Permission();
            CompanyPermission();
            HighlightSideMenu("financeMgr", "lnkCOA", "financeMgrSub");
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

        //DataTable dt = new DataTable();
        //dt = (DataTable)Session["userinfo"];

        //string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
        //if (ProgFunc == "N")
        //{
        //    Response.Redirect("home.aspx");
        //}
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            //ds = (DataTable)Session["userinfo"];
            ds = GetUserById();
            //Equipment

            string ChartofAccountPermission = ds.Rows[0]["chart"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["chart"].ToString();

            string chartAdd = ChartofAccountPermission.Length < 1 ? "Y" : ChartofAccountPermission.Substring(0, 1);
            string chartEdit = ChartofAccountPermission.Length < 2 ? "Y" : ChartofAccountPermission.Substring(1, 1);
            string chartDelete = ChartofAccountPermission.Length < 3 ? "Y" : ChartofAccountPermission.Substring(2, 1);
            string chartView = ChartofAccountPermission.Length < 4 ? "Y" : ChartofAccountPermission.Substring(3, 1);

            if (chartAdd == "N")
            {
                lnkAddnew.Visible = false;
            }
            if (chartEdit == "N")
            {
                btnEdit.Visible = false;
                lnkCopy.Visible = false;
            }
            if (chartDelete == "N")
            {
                lnkDelete.Visible = false;

            }
            if (chartView == "N")
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
            RadGrid_COA.Columns[10].Visible = true;
        }
        else
        {
            RadGrid_COA.Columns[10].Visible = false;
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addcoa.aspx", false);
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem di in RadGrid_COA.SelectedItems)
            {
                Label lblID = (Label)di.FindControl("lblId");
                Response.Redirect("addcoa.aspx?id=" + lblID.Text, false);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem di in RadGrid_COA.SelectedItems)
            {
                Label lblID = (Label)di.FindControl("lblId");


                _objChart.ID = Convert.ToInt32(lblID.Text);

                //check if balance is zero
                _objChart.ConnConfig = Session["config"].ToString();

                DataSet _dsChartDetail = new DataSet();
                _dsChartDetail = _objBLChart.GetChart(_objChart);
                double _balance = Convert.ToDouble(_dsChartDetail.Tables[0].Rows[0]["Balance"].ToString());
                string _defaultAcct = Convert.ToString(_dsChartDetail.Tables[0].Rows[0]["DefaultNo"].ToString());

                if (_defaultAcct.Equals("0"))
                {
                    if (!_balance.Equals(0))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "notifyDelete();", true);
                    }
                    else
                    {
                        _objBLChart.DeleteChart(_objChart);
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Chart of Account deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        BindSearchChartList();
                        RadGrid_COA.Rebind();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "cannotDeleteAccount();", true);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem di in RadGrid_COA.SelectedItems)
            {
                Label lblID = (Label)di.FindControl("lblId");
                Response.Redirect("addcoa.aspx?id=" + lblID.Text + "&c=1");
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void BindSearchChartList()
    {
        int selectedValue = ddlSearch.SelectedIndex;
        Session["ddlSearch_COA"] = selectedValue;

        if (selectedValue == 0)
        {
            Session["ddlSearch_Value_COA"] = "";
            Session["ddlSearch_Value_COABal"] = ddlBalanceCondition.SelectedItem.Text;
        }
        else if (selectedValue == 1)
        {
            Session["ddlSearch_Value_COA"] = txtSearch.Text;
        }
        else if (selectedValue == 2)
        {
            Session["ddlSearch_Value_COA"] = txtSearch.Text;
        }
        else if (selectedValue == 3)
        {
            Session["ddlSearch_Value_COA"] = txtSearch.Text;
            Session["ddlSearch_Value_COABal"] = ddlBalanceCondition.SelectedItem.Text;
        }
        else if (selectedValue == 4)
        {
            Session["ddlSearch_Value_COA"] = txtSearch.Text;
        }

        Session["ddlSearch_Value_COAType"] = ddlType.SelectedValue;
        Session["ddlSearch_Value_COACategory"] = ddlSubAcCategory.SelectedItem.Text;
        Session["ddlSearch_Value_COAStatus"] = ddlStatus.SelectedValue;

        try
        {
            Chart _objC = new Chart();
            if (ddlSearch.SelectedIndex != 0)
            {
                _objC.SearchIndex = Convert.ToInt32(ddlSearch.SelectedIndex);
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    switch (ddlSearch.SelectedIndex)
                    {
                        case 1:
                            _objC.SearchBy = txtSearch.Text;
                            break;
                        case 2:
                            _objC.SearchBy = txtSearch.Text;
                            break;
                        case 3:
                            if (ddlBalanceCondition.SelectedIndex != 0)
                            {
                                _objC.SearchBy = txtSearch.Text;
                                _objC.Condition = ddlBalanceCondition.SelectedItem.Text;
                            }
                            break;
                        case 4:
                            _objC.SearchBy = txtSearch.Text;
                            break;
                    }
                }
            }

            if (ddlStatus.SelectedValue != " Select Status ")
            {
                _objC.SearchStatus = Convert.ToInt32(ddlStatus.SelectedValue);
            }

            if (ddlType.SelectedValue != " Select Account Type ")
            {
                _objC.SearchAcctType = Convert.ToInt32(ddlType.SelectedValue);
            }

            if (ddlSubAcCategory.SelectedValue != " Select Sub Category ")
            {
                _objC.Sub = ddlSubAcCategory.SelectedItem.Text;
            }

            _objC.ConnConfig = Session["config"].ToString();
            _objC.UserID = Convert.ToInt32(Session["UserID"].ToString());

            if (Session["CmpChkDefault"].ToString() == "1")
            {
                _objC.EN = 1;
            }
            else
            {
                _objC.EN = 0;
            }

            DataSet _dsChart = new DataSet();
            _dsChart = _objBLChart.GetAccountData(_objC);

            if (_dsChart != null && _dsChart.Tables.Count > 0)
            {
                var filteredTable = RemoveDuplicateRows(_dsChart.Tables[0], "Acct");
                var finalData = ProcessDataFilter(filteredTable);

                Session["Chart"] = filteredTable;
                RadGrid_COA.VirtualItemCount = finalData.Rows.Count;
                RadGrid_COA.DataSource = finalData;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    public DataTable RemoveDuplicateRows(DataTable table, string DistinctColumn)
    {
        try
        {
            ArrayList UniqueRecords = new ArrayList();
            ArrayList DuplicateRecords = new ArrayList();

            // Check if records is already added to UniqueRecords otherwise,
            // Add the records to DuplicateRecords
            foreach (DataRow dRow in table.Rows)
            {
                if (UniqueRecords.Contains(dRow[DistinctColumn]))
                    DuplicateRecords.Add(dRow);
                else
                    UniqueRecords.Add(dRow[DistinctColumn]);
            }

            // Remove duplicate rows from DataTable added to DuplicateRecords
            foreach (DataRow dRow in DuplicateRecords)
            {
                table.Rows.Remove(dRow);
            }

            // Return the clean DataTable which contains unique records.
            return table;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private DataTable ProcessDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            string sql = "1=1";

            if (Session["COA_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                if (Session["COA_Filters"] != null)
                {
                    var filtersGet = Session["COA_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var filter in filtersGet)
                        {
                            GridColumn column = RadGrid_COA.MasterTableView.GetColumnSafe(filter.FilterColumn);

                            if (column.UniqueName == "Debit" || column.UniqueName == "Credit")
                            {
                                sql = sql + " AND " + column.UniqueName + " = " + filter.FilterValue;
                            }
                            else
                            {
                                sql = sql + " AND " + column.UniqueName + " LIKE '%" + filter.FilterValue + "%'";
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

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        BindSearchChartList();
        RadGrid_COA.Rebind();
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        Session["COA_FilterExpression"] = null;
        Session["COA_Filters"] = null;

        txtSearch.Text = "";
        ddlSearch.SelectedIndex = 0;
        ddlBalanceCondition.SelectedIndex = 0;
        ddlType.SelectedIndex = 0;
        ddlSubAcCategory.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        txtSearch.Visible = false;
        ddlBalanceCondition.Visible = false;
        BindSearchChartList();
        FillSubAccount();

        foreach (GridColumn column in RadGrid_COA.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }

        RadGrid_COA.MasterTableView.PageSize = 50;
        RadGrid_COA.MasterTableView.FilterExpression = string.Empty;
        RadGrid_COA.Rebind();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        Session["COA_FilterExpression"] = null;
        Session["COA_Filters"] = null;

        txtSearch.Text = "";
        ddlSearch.SelectedIndex = 0;
        ddlBalanceCondition.SelectedIndex = 0;
        ddlType.SelectedIndex = 0;
        ddlSubAcCategory.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        txtSearch.Visible = false;
        ddlBalanceCondition.Visible = false;
        BindSearchChartList();
        FillSubAccount();

        foreach (GridColumn column in RadGrid_COA.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }

        RadGrid_COA.MasterTableView.PageSize = 50;
        RadGrid_COA.MasterTableView.FilterExpression = string.Empty;
        RadGrid_COA.Rebind();
    }
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedValue = ddlSearch.SelectedIndex;
        ShowHideFilterSearch(selectedValue);
    }
    private void ShowHideFilterSearch(int selectedValue)
    {
        txtSearch.Text = "";
        ddlBalanceCondition.SelectedIndex = 0;
        if (ddlSearch.SelectedIndex == 0)
        {
            txtSearch.Visible = false;
            ddlBalanceCondition.Visible = false;
        }
        else if (ddlSearch.SelectedIndex == 1)
        {
            txtSearch.Visible = true;
            ddlBalanceCondition.Visible = false;
        }
        else if (ddlSearch.SelectedIndex == 2)
        {
            txtSearch.Visible = true;
            ddlBalanceCondition.Visible = false;
        }
        else if (ddlSearch.SelectedIndex == 3)
        {
            txtSearch.Visible = true;
            ddlBalanceCondition.Visible = true;
        }
        else if (ddlSearch.SelectedIndex == 4)
        {
            txtSearch.Visible = true;
            ddlBalanceCondition.Visible = false;
        }
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSubAccount();   
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    #endregion

    #region Custom Functions

    #region Fill Status
    private void FillStatus()
    {
        try
        {
            DataSet _dsStatus = new DataSet();
            _dsStatus = _objBLChart.GetAllStatus(_objChart);
            if (_dsStatus.Tables.Count > 0)
            {
                ddlStatus.Items.Add(new ListItem(" Select Status "));
                ddlStatus.AppendDataBoundItems = true;
                ddlStatus.DataSource = _dsStatus;
                ddlStatus.DataValueField = "ID";
                ddlStatus.DataTextField = "Status";
                ddlStatus.DataBind();
            }
            else
            {
                ddlStatus.Items.Insert(0, new ListItem(" No Status Available ", "0"));
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Fill Account Type
    private void FillAccountType()
    {
        try
        {
            _objAcType.ConnConfig = Session["config"].ToString();
            DataSet _dsType = new DataSet();
            _dsType = _objBLAcType.GetAllType(_objAcType);

            if (_dsType.Tables.Count > 0)
            {
                ddlType.Items.Add(new ListItem(" Select Account Type "));
                ddlType.AppendDataBoundItems = true;
                ddlType.DataSource = _dsType;
                ddlType.DataValueField = "ID";
                ddlType.DataTextField = "Type";
                ddlType.DataBind();
            }
            else
            {
                ddlType.Items.Insert(0, new ListItem(" No Account Type Available ", "0"));
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Fill Sub Account
    private void FillSubAccount()
    {
        try
        {
            if (ddlType.SelectedValue != " Select Account Type ")
            {
                _objAcType.ConnConfig = Session["config"].ToString();
                _objAcType.CType = Convert.ToInt32(ddlType.SelectedValue);

                DataSet _dsSubType = new DataSet();
                _dsSubType = _objBLAcType.GetTypeByAccount(_objAcType);

                if (_dsSubType != null)
                {
                    ddlSubAcCategory.Items.Clear();

                    if (_dsSubType.Tables.Count > 0)
                    {
                        ddlSubAcCategory.Items.Add(new ListItem(" Select Sub Category "));
                        ddlSubAcCategory.Items.Add(new ListItem(" < Add New > ", "0"));
                        ddlSubAcCategory.AppendDataBoundItems = true;
                        ddlSubAcCategory.DataSource = _dsSubType;
                        ddlSubAcCategory.DataValueField = "ID";
                        ddlSubAcCategory.DataTextField = "SubType";
                        ddlSubAcCategory.DataBind();
                    }
                    else
                    {
                        ddlSubAcCategory.Items.Add(new ListItem(" Select Sub Category "));
                        ddlSubAcCategory.Items.Add(new ListItem(" < Add New > ", "0"));
                    }
                }
            }
            else
            {
                ddlSubAcCategory.Items.Clear();
                ddlSubAcCategory.Items.Add(new ListItem(" Select Sub Category "));
                ddlSubAcCategory.Items.Add(new ListItem(" < Add New > ", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion
    private void RecalCOA()
    {
        try
        {
            _objChart.ConnConfig = Session["config"].ToString();
            _objBLChart.CalChartBalance(_objChart);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    protected void RadGrid_COA_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_COA.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["COA_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_COA.MasterTableView.OwnerGrid.Columns)
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
            Session["COA_Filters"] = filters;
        }
        else
        {
            Session["COA_FilterExpression"] = null;
            Session["COA_Filters"] = null;
        }
        #endregion
        if (intExportExcel == 1)
        {
            GeneralFunctions obj = new GeneralFunctions();
            obj.CorrectTelerikPager(RadGrid_COA);
            intExportExcel = 0;
        }
        foreach (GridDataItem gr in RadGrid_COA.Items)
        {
            Label lblId = (Label)gr.FindControl("lblId");

            gr.Cells[1].Attributes["ondblclick"] = "window.location.assign('addcoa.aspx?id=" + lblId.Text + "');";
            gr.Cells[2].Attributes["ondblclick"] = "window.location.assign('addcoa.aspx?id=" + lblId.Text + "');";
            gr.Cells[3].Attributes["ondblclick"] = "window.location.assign('addcoa.aspx?id=" + lblId.Text + "');";
            gr.Cells[4].Attributes["ondblclick"] = "window.location.assign('addcoa.aspx?id=" + lblId.Text + "');";
            gr.Cells[5].Attributes["ondblclick"] = "window.location.assign('addcoa.aspx?id=" + lblId.Text + "');";
            gr.Cells[6].Attributes["ondblclick"] = "window.location.assign('addcoa.aspx?id=" + lblId.Text + "');";
            gr.Cells[7].Attributes["ondblclick"] = "window.location.assign('addcoa.aspx?id=" + lblId.Text + "');";
            gr.Cells[8].Attributes["ondblclick"] = "window.location.assign('addcoa.aspx?id=" + lblId.Text + "');";

            gr.Cells[9].Attributes["ondblclick"] = "window.location.assign('accountledger.aspx?id=" + lblId.Text + "');";
            gr.Cells[10].Attributes["ondblclick"] = "window.location.assign('accountledger.aspx?id=" + lblId.Text + "');";
        }
    }

    protected void RadGrid_COA_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_COA.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["COA_FilterExpression"] != null && Convert.ToString(Session["COA_FilterExpression"]) != "" && Session["COA_Filters"] != null)
                {
                    RadGrid_COA.MasterTableView.FilterExpression = Convert.ToString(Session["COA_FilterExpression"]);
                    var filtersGet = Session["COA_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_COA.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["COA_FilterExpression"] = null;
                Session["COA_Filters"] = null;
            }
        }
        #endregion
        BindSearchChartList();
    }
    protected void RadGrid_COA_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        updpnl.Update();
        SetTotal();
    }
    protected void RadGrid_COA_ItemCreated(object sender, GridItemEventArgs e)
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
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_COA.MasterTableView.FilterExpression != "" ||
            (RadGrid_COA.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_COA.MasterTableView.SortExpressions.Count > 0;
    }

    private void BindChartList()
    {
        try
        {
            DataSet dsChart = new DataSet();
            _objChart.ConnConfig = Session["config"].ToString();
            _objChart.UserID = Convert.ToInt32(Session["UserID"].ToString());
            if (Session["CmpChkDefault"].ToString() == "1")
            {
                _objChart.EN = 1;
            }
            else
            {
                _objChart.EN = 0;
            }
            dsChart = _objBLChart.GetAll(_objChart);

            double _debitVal = 0;
            double _creditVal = 0;

            if (ViewState["GrandTotalDebit"] == null || ViewState["GrandTotalCredit"] == null)
            {
                if (dsChart.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsChart.Tables[0].Rows)
                    {
                        if (!Convert.ToInt16(dr["Type"]).Equals(7))
                        {
                            if (Convert.ToDouble(dr["Balance"]) < 0)
                                _creditVal = _creditVal + (Convert.ToDouble(dr["Balance"]) * -1);
                            else
                                _debitVal = _debitVal + Convert.ToDouble(dr["Balance"]);
                        }
                    }
                }

                ViewState["GrandTotalDebit"] = _debitVal;
                ViewState["GrandTotalCredit"] = _creditVal;
            }

            Session["Chart"] = dsChart.Tables[0];
            RadGrid_COA.VirtualItemCount = dsChart.Tables[0].Rows.Count;
            RadGrid_COA.DataSource = dsChart.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", false);
        }
    }
    protected void RadGrid_COA_RowDataBound(object sender, GridItemEventArgs e)
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

                    lblTotalDebit.Text = string.Format("{0:c}", dPageTotal);
                    lblTotalCredit.Text = string.Format("{0:c}", cPageTotal);
                    break;
                default:
                case GridItemType.Item:

                    HiddenField hdnType = default(HiddenField);
                    hdnType = (HiddenField)e.Item.FindControl("hdnTypeId");

                    if (hdnType != null && hdnType.Value != "7")
                    {
                        Label lblBDebit = default(Label);
                        lblBDebit = (Label)e.Item.FindControl("lblBDebit");
                        dPageTotal += Convert.ToDouble(lblBDebit.Text);

                        Label lblBCredit = default(Label);
                        lblBCredit = (Label)e.Item.FindControl("lblBCredit");
                        cPageTotal += Convert.ToDouble(lblBCredit.Text);
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
            if (RadGrid_COA.Items.Count > 0)
            {
                double debitAmt = 0;
                double creditAmt = 0;
                DataTable dt = new DataTable();
                dt = (DataTable)Session["Chart"];
                debitAmt = Convert.ToDouble(dt.Compute("Sum(Debit)", "Type <> 7"));
                creditAmt = Convert.ToDouble(dt.Compute("Sum(Credit)", "Type <> 7"));
                //debitAmt = Convert.ToDouble(dt.Compute("Sum(Debit)", string.Empty));
                //creditAmt = Convert.ToDouble(dt.Compute("Sum(Credit)", string.Empty));

                GridFooterItem footerItem = (GridFooterItem)RadGrid_COA.MasterTableView.GetItems(GridItemType.Footer)[0];
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
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        intExportExcel = 1;
        RadGrid_COA.ExportSettings.FileName = "ChartOfAccount";
        RadGrid_COA.ExportSettings.IgnorePaging = true;
        RadGrid_COA.ExportSettings.ExportOnlyData = true;
        RadGrid_COA.ExportSettings.OpenInNewWindow = true;
        RadGrid_COA.ExportSettings.HideStructureColumns = true;
        RadGrid_COA.MasterTableView.UseAllDataFields = true;
        RadGrid_COA.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_COA.MasterTableView.ExportToExcel();
    }
    protected void RadGrid_COA_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (e.Worksheet.Table.Rows.Count == RadGrid_COA.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_COA.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
            RowElement row = new RowElement(); //create new row for the footer aggregates
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
}