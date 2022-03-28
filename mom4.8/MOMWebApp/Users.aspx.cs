using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using static CommonHelper;
using System.Text.RegularExpressions;

public partial class Users : System.Web.UI.Page
{
    #region "Variables"
    
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    public static bool isChecked = false;
    Dictionary<string, CustomField> DynamicControls = new Dictionary<string, CustomField>();
    List<String> ls = new List<string>();
    List<CustomFieldID> lsFormula = new List<CustomFieldID>();
    List<String> lsControlName = new List<string>();

    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        createDataSource();

        Userpermissions();

        HighlightSideMenu("progMgr", "lnkUsersSMenu", "progMgrSub");

        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            if (Session["UserActiveTabId"] != null)
            {
                hdnActiveTab.Value = Session["UserActiveTabId"].ToString();
                Session["UserActiveTabId"] = null;
            }

            DefineCustomColumnStructure();

            FillSupervisor();

            #region Show Selected Filter 
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["ddlSearch_User"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearch_User"]);
                    ddlSearch.SelectedValue = selectedValue;

                    String searchValue = Convert.ToString(Session["ddlSearch_Value_User"]);
                    if (selectedValue == "usertype")
                    {
                        ddlUserType.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "u.Status")
                    {
                        rbStatus.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "w.super")
                    {
                        ddlSuper.SelectedValue = searchValue;
                    }
                    else
                    {
                        txtSearch.Visible = true;
                        txtSearch.Text = searchValue;
                    }
                }
            }
            else
            {
                Session.Remove("ddlSearch_User");
                Session.Remove("ddlSearch_Value_User");
                Session.Remove("Uesrs_FilterExpression");
                Session.Remove("Uesrs_Filters");
            }

            #endregion

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "selectTab", "$('#tabProject > li > a#" + hdnActiveTab.Value + "')[0].click();", true);
        }

        ShowHideFilterSearch(ddlSearch.SelectedValue);
        ShowHideRoleSearchControl(ddlSearchRole.SelectedValue);
    }
    #endregion

    [Serializable]
    public class CustomFieldID
    {
        public String controlName { get; set; }
        public String controlID { get; set; }
        public int xFireDay { get; set; }
        public int xScheduleDay { get; set; }
        public String ControlUpdate { get; set; }
        public CustomField ControlType { get; set; }
    }

    private void createDataSource()
    {
        ArrayList item = new ArrayList();
        //dynamic column
        DataSet dst = new DataSet();
        BL_UserCustom _objblusercustom = new BL_UserCustom();
        dst = _objblusercustom.GetAllUserCustom(Session["config"].ToString(), Session["dbname"].ToString());
        DataTable dtColumns = dst.Tables[0];
        RadGrid_Uesrs.AutoGenerateColumns = false;
        foreach (DataRow row in dtColumns.Rows)
        {
            if (row["Format"].ToString() == "4")
            {
                if (!item.Contains(row["Label"].ToString() + "_" + row["ID"].ToString()))
                {
                    SqlDataSource sourceID = new SqlDataSource();
                    sourceID.ID = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                    sourceID.ConnectionString = Session["config"].ToString();
                    sourceID.SelectCommand = "SELECT '' AS [Value] UNION SELECT [Value] FROM tblUserCustom where [tblUserCustomFieldsID]=" + row["ID"].ToString();
                    sourceID.DataBind();
                    this.Page.Controls.Add(sourceID);
                    item.Add(sourceID.ID);
                }
            }
            if (row["Format"].ToString() == "5")
            {
                if (!item.Contains(row["Label"].ToString() + "_" + row["ID"].ToString()))
                {
                    SqlDataSource sourceID = new SqlDataSource();
                    sourceID.ID = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                    sourceID.ConnectionString = Session["config"].ToString();
                    sourceID.SelectCommand = "select 'True'  as [Value] union select 'False'  as [Value]";
                    sourceID.DataBind();
                    this.Page.Controls.Add(sourceID);
                    item.Add(sourceID.ID);
                }
            }
        }
    }

    private void DefineCustomColumnStructure()
    {
        String FireTestDate = "";
        //dynamic column
        DataSet dst = new DataSet();
        BL_UserCustom _objblusercustom = new BL_UserCustom();
        dst = _objblusercustom.GetAllUserCustom(Session["config"].ToString(), Session["dbname"].ToString());
        DataTable dtColumns = dst.Tables[0];

        foreach (DataRow row in dtColumns.Rows)
        {
            DynamicControls.Add("CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString()
                        , (CustomField)Enum.Parse(typeof(CustomField), row["Format"].ToString()));
            if (Convert.ToInt32(row["Format"].ToString()) != 5)
            {
                if (Convert.ToBoolean(row["UseFormula"]) == true)
                {
                    if (row["Formula"].ToString() != "")
                    {
                        //Formular : if    [Fire Test Date] == ''   then [Schedule date] + Days(90)  Else [Fire Test Date]   + Days(90)
                        Regex regex = new Regex(@"Else *.*");
                        Match match = regex.Match(row["Formula"].ToString());
                        String strElse = match.Value;

                        //Get  control Name
                        Regex regex_ControlID = new Regex(@"\[.*?\]");
                        Match match_ThenControlID = regex_ControlID.Match(strElse);
                        String strControlID = match_ThenControlID.Value.Replace("[", "").Replace("]", "");
                        
                        //Get Date
                        Regex regex_XDay = new Regex(@"\(.*?\)");
                        Match match_XDay = regex_XDay.Match(strElse);
                        String strXDay = match_XDay.Value.Replace("(", "").Replace(")", "");

                        //Get Shcedule Date
                        Match match_XScheduleDay = regex_XDay.Match(row["Formula"].ToString());
                        String strXScheduleDay = match_XScheduleDay.Value.Replace("(", "").Replace(")", "");

                        if (!ls.Contains(strControlID) && strXDay != "")
                        {
                            CustomFieldID custom = new CustomFieldID();
                            custom.controlName = strControlID;
                            custom.ControlUpdate = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                            try
                            {
                                custom.xFireDay = Convert.ToInt32(strXDay);
                                custom.xScheduleDay = Convert.ToInt32(strXScheduleDay);
                            }
                            catch (Exception ex)
                            {
                                custom.xFireDay = 0;
                                custom.xScheduleDay = 0;
                            }
                            lsFormula.Add(custom);
                        }
                    }
                }
                else
                {
                    lsControlName.Add("CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString());
                }
            }
            else
            {
                lsControlName.Add("CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString());
            }
        }

        RadGrid_Uesrs.AutoGenerateColumns = false;

        foreach (DataRow row in dtColumns.Rows)
        {
            if (row["Label"].ToString().Replace(" ", "") == "FireTestDate")
            {
                FireTestDate = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
            }

            if (Convert.ToInt32(row["Format"].ToString()) == 4)
            {
                GridDropDownColumn ncol = new GridDropDownColumn();
                RadGrid_Uesrs.MasterTableView.Columns.Add(ncol);
                ncol.DataField = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                ncol.HeaderText = row["Label"].ToString();
                ncol.HeaderStyle.Width = 100;
                ncol.AutoPostBackOnFilter = true;
                ncol.ShowFilterIcon = false;
                ncol.DataSourceID = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                ncol.ListValueField = "Value";
                ncol.ListTextField = "Value";
                ncol.DataType = typeof(System.String);
            }
            else
            {
                if (Convert.ToInt32(row["Format"].ToString()) == 5)
                {
                    GridDropDownColumn ncol = new GridDropDownColumn();
                    RadGrid_Uesrs.MasterTableView.Columns.Add(ncol);
                    ncol.DataField = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                    ncol.HeaderText = row["Label"].ToString();
                    ncol.HeaderStyle.Width = 100;
                    ncol.AutoPostBackOnFilter = true;
                    ncol.ShowFilterIcon = false;
                    ncol.DataSourceID = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                    ncol.ListValueField = "Value";
                    ncol.ListTextField = "Value";
                    ncol.DataType = typeof(System.String);
                }
                else
                {
                    String id = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                    GridBoundColumn ncol = new GridBoundColumn();
                    RadGrid_Uesrs.MasterTableView.Columns.Add(ncol);
                    ncol.DataField = id;
                    ncol.HeaderText = row["Label"].ToString();
                    ncol.HeaderStyle.Width = 100;
                    ncol.AutoPostBackOnFilter = true;
                    ncol.ShowFilterIcon = false;
                    ncol.DataType = typeof(System.String);
                    ncol.CurrentFilterFunction = GridKnownFunction.Contains;

                    foreach (CustomFieldID item in lsFormula)
                    {
                        if (item.controlName == row["Label"].ToString())
                        {
                            item.controlID = id;
                        }
                    }
                }
            }
        }
        Session["FormulaControl"] = lsFormula;
        Session["CustomControl"] = lsControlName;
        Session["DynamicControls"] = DynamicControls;

        hdnFormularFieldID.Value = String.Join(",", ls);
        hdnFormularValue.Value = String.Join(",", lsFormula);
        hdnFireTestDate.Value = FireTestDate;
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

    #region Custom Function
    private void Userpermissions()
    {


        if (Session["type"].ToString() != "am")
        {
            DataTable dt = new DataTable(); dt = (DataTable)Session["userinfo"];

            string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
            string AccessUser = dt.Rows[0]["users"].ToString().Substring(0, 1);

            if (ProgFunc == "N")
            {
                Response.Redirect("home.aspx?permission=no");
            }
            if (AccessUser == "N")
            {
                Response.Redirect("home.aspx?permission=no");
            }

        }
        if (Session["MSM"].ToString() == "TS")
        {
            pnlGridButtons.Visible = false;
        }
    }

    private void FillGridSearch()
    {
        DataSet ds = new DataSet();
        DataTable dtFinal = new DataTable();

        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.IsSuper = 0;
        objProp_User.Supervisor = Session["username"].ToString();

        if (ddlSearch.SelectedIndex != 0)
        {
            objProp_User.SearchBy = ddlSearch.SelectedValue;
            objProp_User.SearchValue = txtSearch.Text.Replace("'", "''");

            if (ddlSearch.SelectedValue == "u.Status")
            {
                objProp_User.SearchValue = rbStatus.SelectedValue;
            }
            if (ddlSearch.SelectedValue == "usertype")
            {
                objProp_User.SearchValue = ddlUserType.SelectedValue;
            }
            if (ddlSearch.SelectedValue == "w.super")
            {
                objProp_User.SearchValue = ddlSuper.SelectedValue;
            }

            if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
            {
                objProp_User.IsSuper = 1;

                ds = objBL_User.getUserSearch(objProp_User);
            }
            else
            {
                ds = objBL_User.getUserSearch(objProp_User);
            }
        }
        else
        {
            if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
            {

                ddlSearch.Items.Remove(ddlSearch.Items.FindByValue("w.super"));
                ddlSearch.Items.Remove(ddlSearch.Items.FindByValue("usertype"));
                objProp_User.IsSuper = 1;
                objProp_User.SearchBy = "w.super";
                objProp_User.SearchValue = Session["username"].ToString();
                ds = objBL_User.getUserSearch(objProp_User);
            }
            else
            {
                ds = objBL_User.getUser(objProp_User);
            }
        }

        if (ds.Tables.Count > 0)
        {
            if (lnkChk.Checked == true)
            {
                dtFinal = ds.Tables[0];
            }
            else
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (lnkChk.Checked == false && rbStatus.SelectedValue == "1")
                        dtFinal = ds.Tables[0].Select("Status = '1'").CopyToDataTable();
                    else
                        dtFinal = ds.Tables[0].Select("Status = '0'").CopyToDataTable();
                }
                else
                {
                    dtFinal = ds.Tables[0];
                }
            }
        }

        DataTable result = ProcessDataFilter(dtFinal);

        Session["usersearch"] = result;
        RadGrid_Uesrs.VirtualItemCount = result.Rows.Count;
        RadGrid_Uesrs.DataSource = result;

        // Filter Expression
        if (Request.QueryString["f"] != "c")
        {
            if (Session["Uesrs_FilterExpression"] != null && Convert.ToString(Session["Uesrs_FilterExpression"]) != "")
            {
                var filterExpression = Convert.ToString(Session["Uesrs_FilterExpression"]);
                RadGrid_Uesrs.MasterTableView.FilterExpression = filterExpression;
                var filtersGet = Session["Uesrs_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Uesrs.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }
        }
        else
        {
            Session.Remove("Uesrs_FilterExpression");
            Session.Remove("Uesrs_Filters");
        }
    }

    private DataTable ProcessDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Session["Uesrs_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                var filtersGet = Session["Uesrs_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {

                        GridColumn column = RadGrid_Uesrs.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                        if (column != null)
                        {
                            sql = sql + " And " + column.UniqueName + " like '%" + _filter.FilterValue + "%'";
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

    private void FillSupervisor()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getSupervisor(objProp_User);

        ddlSuper.DataSource = ds.Tables[0];
        ddlSuper.DataTextField = "fuser";
        ddlSuper.DataValueField = "fuser";
        ddlSuper.DataBind();
    }
    #endregion

    #region Events
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        if (hdnActiveTab.Value == "1")
        {
            foreach (GridDataItem di in RadGrid_Uesrs.SelectedItems)
            {
                Label lblUserID = (Label)di.FindControl("lblId");
                Label lbltypeid = (Label)di.FindControl("lblTypeid");
                if (lbltypeid.Text != "2")
                    Response.Redirect("adduser.aspx?uid=" + lblUserID.Text + "&type=" + lbltypeid.Text);
                else
                    Response.Redirect("customeruser.aspx?uid=" + lblUserID.Text + "&type=" + lbltypeid.Text);
            }
        }
        else if (hdnActiveTab.Value == "2")
        {
            foreach (GridDataItem di in RadGrid_Roles.SelectedItems)
            {
                Label lblRoleID = (Label)di.FindControl("lblId");
                Response.Redirect("adduserrole.aspx?uid=" + lblRoleID.Text);
            }
        }
    }

    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        if (hdnActiveTab.Value == "1")
        {
            foreach (GridDataItem di in RadGrid_Uesrs.SelectedItems)
            {
                Label lblUserID = (Label)di.FindControl("lblId");
                Label lbltypeid = (Label)di.FindControl("lblTypeid");
                if (lbltypeid.Text != "2")
                    Response.Redirect("adduser.aspx?uid=" + lblUserID.Text + "&t=c" + "&type=" + lbltypeid.Text);
                else
                    Response.Redirect("customeruser.aspx?uid=" + lblUserID.Text + "&t=c" + "&type=" + lbltypeid.Text);
            }
        }else if(hdnActiveTab.Value == "2")
        {
            foreach (GridDataItem di in RadGrid_Roles.SelectedItems)
            {
                Label lblRoleID = (Label)di.FindControl("lblId");
                Response.Redirect("adduserrole.aspx?uid=" + lblRoleID.Text + "&t=c");
            }
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Uesrs.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblTicketId = (Label)di.FindControl("lblId");
            if (chkSelect.Checked == true)
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.UserID = Convert.ToInt32(lblTicketId.Text);
                // objBL_User.DeleteUser(objProp_User); Pending with Anita Required Condition to Delete User
                FillGridSearch();
                RadGrid_Uesrs.Rebind();
            }
        }
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        if(hdnActiveTab.Value == "1")
            Response.Redirect("adduser.aspx");
        else if(hdnActiveTab.Value == "2")
            Response.Redirect("adduserrole.aspx");
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        Session.Remove("ddlSearch_User");
        Session.Remove("ddlSearch_Value_User");
        Session.Remove("Uesrs_FilterExpression");
        Session.Remove("Uesrs_Filters");

        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        FillGridSearch();
        foreach (GridColumn column in RadGrid_Uesrs.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }

        ShowHideFilterSearch(ddlSearch.SelectedValue);

        RadGrid_Uesrs.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Uesrs.Rebind();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        Session.Remove("ddlSearch_User");
        Session.Remove("ddlSearch_Value_User");
        Session.Remove("Uesrs_FilterExpression");
        Session.Remove("Uesrs_Filters");

        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;

        FillGridSearch();
        foreach (GridColumn column in RadGrid_Uesrs.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }

        ShowHideFilterSearch(ddlSearch.SelectedValue);

        RadGrid_Uesrs.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Uesrs.Rebind();
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        #region Search Filter
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_User"] = selectedValue;

        if (selectedValue == "usertype")
        {
            Session["ddlSearch_Value_User"] = ddlUserType.SelectedValue;
        }
        else if (selectedValue == "u.Status")
        {
            Session["ddlSearch_Value_User"] = rbStatus.SelectedValue;
        }
        else if (selectedValue == "w.super")
        {
            Session["ddlSearch_Value_User"] = ddlSuper.SelectedValue;
        }
        else
        {
            Session["ddlSearch_Value_User"] = txtSearch.Text;
        }
        #endregion

        FillGridSearch();
        RadGrid_Uesrs.Rebind();
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void ShowHideFilterSearch(string selectedValue)
    {
        div_txtSearch.Style.Add("display", "none");
        div_rbStatus.Style.Add("display", "none");
        div_ddlUserType.Style.Add("display", "none");
        div_ddlSuper.Style.Add("display", "none");

        switch (ddlSearch.SelectedValue.ToLower())
        {
            case "u.status":
                div_rbStatus.Style.Add("display", "block");
                break;
            case "usertype":
                div_ddlUserType.Style.Add("display", "block");
                break;
            case "w.super":
                div_ddlSuper.Style.Add("display", "block");
                break;
            default:
                div_txtSearch.Style.Add("display", "block");
                break;
        }
    }

    protected void lnkchk_Click(object sender, EventArgs e)
    {

        if (lnkChk.Checked)
        {
            isChecked = true;

        }
        else
        {
            isChecked = false;

        }
        FillGridSearch();
        RadGrid_Uesrs.Rebind();
        // UpdatePanel1.Update();
    }

    protected void lnkUserReport_Click(object sender, EventArgs e)
    {
        string urlString = "UserReport.aspx?incInactive=" + lnkChk.Checked;

        Response.Redirect(urlString, true);
    }

    protected void lnkUserPayment_Click(object sender, EventArgs e)
    {
        var searchBy = ddlSearch.SelectedValue;
        var searchValue = txtSearch.Text.Replace("'", "''");
        var isSuper = 0;

        if (ddlSearch.SelectedValue == "u.Status")
        {
            searchValue = rbStatus.SelectedValue;
        }

        if (ddlSearch.SelectedValue == "usertype")
        {
            searchValue = ddlUserType.SelectedValue;
        }

        if (ddlSearch.SelectedValue == "w.super")
        {
            searchValue = ddlSuper.SelectedValue;
        }

        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            isSuper = 1;
        }

        List<RetainFilter> filters = new List<RetainFilter>();
        var filterExpression = Convert.ToString(RadGrid_Uesrs.MasterTableView.FilterExpression);
        if (!string.IsNullOrEmpty(filterExpression))
        {
            foreach (GridColumn column in RadGrid_Uesrs.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = column.UniqueName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }
        }

        Session["Uesrs_Filters"] = filters;

        string urlString = "UserPaymentReport.aspx?incInactive=" + lnkChk.Checked + "&stype=" + searchBy.Trim() + "&svalue=" + searchValue.Trim() + "&isSuper=" + isSuper;

        Response.Redirect(urlString, true);
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Uesrs.MasterTableView.FilterExpression != "" ||
            (RadGrid_Uesrs.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Uesrs.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Uesrs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Uesrs.AllowCustomPaging = !ShouldApplySortFilterOrGroup();

        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Session["Uesrs_FilterExpression"] != null && Convert.ToString(Session["Uesrs_FilterExpression"]) != "" && Session["Uesrs_Filters"] != null)
            {
                RadGrid_Uesrs.MasterTableView.FilterExpression = Convert.ToString(Session["Uesrs_FilterExpression"]);
                var filtersGet = Session["Uesrs_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Uesrs.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }
            else
            {
                Session["Uesrs_FilterExpression"] = null;
                Session["Uesrs_Filters"] = null;
            }
        }
        else
        {
            String filterExpression = Convert.ToString(RadGrid_Uesrs.MasterTableView.FilterExpression);
            if (filterExpression != "")
            {
                Session["Uesrs_FilterExpression"] = filterExpression;
                List<RetainFilter> filters = new List<RetainFilter>();

                foreach (GridColumn column in RadGrid_Uesrs.MasterTableView.OwnerGrid.Columns)
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

                Session["Uesrs_Filters"] = filters;
            }
            else
            {
                Session["Uesrs_FilterExpression"] = null;
                Session["Uesrs_Filters"] = null;
            }
        }
        #endregion

        FillGridSearch();
    }

    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_Uesrs.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            Label lblTypeID = (Label)gr.FindControl("lblTypeid");
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkName");
            if (lblTypeID.Text != "2")
                lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='adduser.aspx?uid=" + lblID.Text + "&type=" + lblTypeID.Text + "'";
            else
                lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='customeruser.aspx?uid=" + lblID.Text + "&type=" + lblTypeID.Text + "'";
        }
    }

    protected void RadGrid_Uesrs_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_Uesrs.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Uesrs_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Uesrs.MasterTableView.OwnerGrid.Columns)
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

            Session["Uesrs_Filters"] = filters;
        }
        else
        {
            Session["Uesrs_FilterExpression"] = null;
            Session["Uesrs_Filters"] = null;
        }
        #endregion
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Uesrs);
        RowSelect();
    }
    
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Uesrs.ExportSettings.FileName = "User";
        RadGrid_Uesrs.ExportSettings.IgnorePaging = true;
        RadGrid_Uesrs.ExportSettings.ExportOnlyData = true;
        RadGrid_Uesrs.ExportSettings.OpenInNewWindow = true;
        RadGrid_Uesrs.ExportSettings.HideStructureColumns = true;
        RadGrid_Uesrs.MasterTableView.UseAllDataFields = true;
        RadGrid_Uesrs.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Uesrs.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_Uesrs_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

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

    protected void RadGrid_Uesrs_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        updpnl.Update();
    }

    #endregion

    protected void btnSearchRole_Click(object sender, EventArgs e)
    {
        FillRoleGridSearch();
        RadGrid_Roles.Rebind();
    }

    protected void lnkShowAllRole_Click(object sender, EventArgs e)
    {
        txtSearchRole.Text = string.Empty;
        ddlSearchRole.SelectedIndex = 0;
        FillRoleGridSearch();
        foreach (GridColumn column in RadGrid_Roles.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }

        ShowHideRoleSearchControl(ddlSearch.SelectedValue);

        RadGrid_Roles.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Roles.Rebind();
    }

    protected void lnkClearRole_Click(object sender, EventArgs e)
    {

    }

    protected void chkIncInactiveRole_CheckedChanged(object sender, EventArgs e)
    {
        FillRoleGridSearch();
        RadGrid_Roles.Rebind();
    }

    bool isRoleGrouping = false;
    public bool ShouldApplySortFilterOrGroupRole()
    {
        return RadGrid_Roles.MasterTableView.FilterExpression != "" ||
            (RadGrid_Roles.MasterTableView.GroupByExpressions.Count > 0 || isRoleGrouping) ||
            RadGrid_Roles.MasterTableView.SortExpressions.Count > 0;
    }
    
    protected void RadGrid_Roles_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Roles.AllowCustomPaging = !ShouldApplySortFilterOrGroupRole();

        #region Set the Grid Filters
        //if (!IsPostBack)
        //{
        //    if (Session["UsersRole_FilterExpression"] != null && Convert.ToString(Session["UsersRole_FilterExpression"]) != "" && Session["UsersRole_Filters"] != null)
        //    {
        //        RadGrid_Roles.MasterTableView.FilterExpression = Convert.ToString(Session["UsersRole_FilterExpression"]);
        //        var filtersGet = Session["UsersRole_Filters"] as List<RetainFilter>;
        //        if (filtersGet != null)
        //        {
        //            foreach (var _filter in filtersGet)
        //            {
        //                GridColumn column = RadGrid_Roles.MasterTableView.GetColumnSafe(_filter.FilterColumn);
        //                column.CurrentFilterValue = _filter.FilterValue;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Session["UsersRole_FilterExpression"] = null;
        //        Session["UsersRole_Filters"] = null;
        //    }
        //}
        //else
        //{
        //    String filterExpression = Convert.ToString(RadGrid_Roles.MasterTableView.FilterExpression);
        //    if (filterExpression != "")
        //    {
        //        Session["UsersRole_FilterExpression"] = filterExpression;
        //        List<RetainFilter> filters = new List<RetainFilter>();

        //        foreach (GridColumn column in RadGrid_Roles.MasterTableView.OwnerGrid.Columns)
        //        {
        //            String filterValues = column.CurrentFilterValue;
        //            if (filterValues != "")
        //            {
        //                String columnName = column.UniqueName;
        //                RetainFilter filter = new RetainFilter();
        //                filter.FilterColumn = columnName;
        //                filter.FilterValue = filterValues;
        //                filters.Add(filter);
        //            }
        //        }

        //        Session["UsersRole_Filters"] = filters;
        //    }
        //    else
        //    {
        //        Session["UsersRole_FilterExpression"] = null;
        //        Session["UsersRole_Filters"] = null;
        //    }
        //}
        #endregion

        FillRoleGridSearch();
    }

    protected void RadGrid_Roles_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_Roles.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["UsersRole_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Roles.MasterTableView.OwnerGrid.Columns)
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

            Session["UsersRole_Filters"] = filters;
        }
        else
        {
            Session["UsersRole_FilterExpression"] = null;
            Session["UsersRole_Filters"] = null;
        }
        #endregion
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Roles);
        //RowSelect();
        UserRoleRowSelect();
    }

    protected void RadGrid_Roles_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

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

    protected void RadGrid_Roles_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordRoleCount.Text = rowCount + " Record(s) found";
        upnlRecordRoleCount.Update();
    }

    private void FillRoleGridSearch()
    {
        DataSet ds = new DataSet();
        UserRole userRole = new UserRole();
        userRole.ConnConfig = Session["config"].ToString();

        userRole.SearchBy = "";
        userRole.SearchValue = "";

        if (ddlSearchRole.SelectedIndex != 0)
        {
            userRole.SearchBy = ddlSearchRole.SelectedValue;
            userRole.SearchValue = txtSearchRole.Text.Replace("'", "''");

            if (ddlSearchRole.SelectedValue == "status")
            {
                userRole.SearchValue = ddlRoleStatus.SelectedValue;
            }
        }
        var isIncInactive = chkIncInactiveRole.Checked;

        ds = objBL_User.GetRoleSearch(userRole, isIncInactive);

        //DataTable result = ProcessDataFilterRole(ds.Tables[0]);
        DataTable result = ds.Tables[0];
        RadGrid_Roles.VirtualItemCount = result.Rows.Count;
        RadGrid_Roles.DataSource = result;

        // Filter Expression
        //if (Request.QueryString["f"] != "c")
        //{
        //    if (Session["UsersRole_FilterExpression"] != null && Convert.ToString(Session["UsersRole_FilterExpression"]) != "")
        //    {
        //        var filterExpression = Convert.ToString(Session["UsersRole_FilterExpression"]);
        //        RadGrid_Roles.MasterTableView.FilterExpression = filterExpression;
        //        var filtersGet = Session["UsersRole_FilterExpression"] as List<RetainFilter>;
        //        if (filtersGet != null)
        //        {
        //            foreach (var _filter in filtersGet)
        //            {
        //                GridColumn column = RadGrid_Roles.MasterTableView.GetColumnSafe(_filter.FilterColumn);
        //                column.CurrentFilterValue = _filter.FilterValue;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    Session.Remove("UsersRole_FilterExpression");
        //    Session.Remove("UsersRole_Filters");
        //}
    }

    private DataTable ProcessDataFilterRole(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Session["UsersRole_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                var filtersGet = Session["UsersRole_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {

                        GridColumn column = RadGrid_Roles.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                        if (column != null)
                        {
                            sql = sql + " And " + column.UniqueName + " like '%" + _filter.FilterValue + "%'";
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

    private void ShowHideRoleSearchControl(string selectedValue)
    {
        div_txtSearchRole.Style.Add("display", "none");
        div_ddlRoleStatus.Style.Add("display", "none");

        switch (ddlSearchRole.SelectedValue.ToLower())
        {
            case "status":
                div_ddlRoleStatus.Style.Add("display", "block");
                break;
            default:
                div_txtSearchRole.Style.Add("display", "block");
                break;
        }
    }

    private void UserRoleRowSelect()
    {
        foreach (GridDataItem gr in RadGrid_Roles.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            gr.Attributes["ondblclick"] = "location.href='addUserRole.aspx?uid=" + lblID.Text + "'";
        }
    }

    protected void lnkUserRoleExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Roles.ExportSettings.FileName = "User Roles";
        RadGrid_Roles.ExportSettings.IgnorePaging = true;
        RadGrid_Roles.ExportSettings.ExportOnlyData = true;
        RadGrid_Roles.ExportSettings.OpenInNewWindow = true;
        RadGrid_Roles.ExportSettings.HideStructureColumns = true;
        RadGrid_Roles.MasterTableView.UseAllDataFields = true;
        RadGrid_Roles.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Roles.MasterTableView.ExportToExcel();
    }
}
