using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using Telerik.Web.UI;
using System.Collections.Generic;

public partial class ProjectTemplate : System.Web.UI.Page
{
    #region Variables
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    JobT _objJob = new JobT();
    BL_User objBL_User = new BL_User();
    private const string _asc = " ASC";
    private const string _desc = " DESC";
    #endregion

    #region events
    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Role"] != null && Request.QueryString["Role"] == "SalesManager")
        {
            Session["PageEstimateTemplate"] = 1;
        }
        else
        {
            Session["PageEstimateTemplate"] = 0;
        }
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

            HighlightSideMenu();
            Permission();


        }
    }


    #endregion


    private void HighlightSideMenu()
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkEstimateTempl");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        div.Style.Add("display", "block");
    }
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        //foreach (GridViewRow di in gvProject.Rows)
        //{
        //    CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
        //    Label lblID = (Label)di.FindControl("lblId");

        //    if (chkSelect.Checked == true)
        //    {
        //        Response.Redirect("addprojecttemp.aspx?uid=" + lblID.Text);
        //    }
        //}

        foreach (GridDataItem item in RadGrid_ProjectTemplate.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            Response.Redirect("addprojecttemp.aspx?uid=" + lblID.Text);
        }
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            //foreach (GridViewRow di in gvProject.Rows)
            //{
            //    CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
            //    Label lblID = (Label)di.FindControl("lblID");

            //    if (chkSelect.Checked == true)
            //    {
            //        //objProp_Customer.ConnConfig = Session["config"].ToString();
            //        //objProp_Customer.dtLaborItems = null;
            //        //objProp_Customer.dtItems = null;
            //        //objProp_Customer.Mode = 2;
            //        //objProp_Customer.TemplateID = Convert.ToInt32(lblProspectID.Text);
            //        //objBL_Customer.AddEstimate(objProp_Customer);
            //        _objJob.ConnConfig = Session["config"].ToString();
            //        _objJob.ID = Convert.ToInt32(lblID.Text);

            //        objBL_Customer.DeleteProjectTemplate(_objJob);

            //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDel", "noty({text: 'Template Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            //        FillProjects();
            //    }
            //}

            foreach (GridDataItem item in RadGrid_ProjectTemplate.SelectedItems)
            {
                Label lblID = (Label)item.FindControl("lblID");
                _objJob.ConnConfig = Session["config"].ToString();
                _objJob.ID = Convert.ToInt32(lblID.Text);
                objBL_Customer.DeleteProjectTemplate(_objJob);
                //Response.Redirect("addprojecttemp.aspx?uid=" + lblID.Text);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDel", "noty({text: 'Template Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                ProjectTemplateList();
                RadGrid_ProjectTemplate.Rebind();
            }
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            if(str.Equals("Deleting Error",StringComparison.InvariantCultureIgnoreCase))
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDelErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            else
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDelWarning", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("addprojecttemp.aspx");
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }





    #endregion

    #region Custom Functions
    private void Permission()
    {
        //HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("ProjectMgr");
        ////li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        //li.Attributes.Add("class", "start active open");

        //HyperLink a = (HyperLink)Page.Master.FindControl("ProjectLink");
        ////a.Style.Add("color", "#2382b2");

        //HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkProjectTempl");
        ////lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        //lnkUsersSmenu.Style.Add("color", "#316b9d");
        //lnkUsersSmenu.Style.Add("font-weight", "normal");
        //lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        ////ul.Attributes.Remove("class");
        //ul.Style.Add("display", "block");
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }
        if (Session["PageEstimateTemplate"].ToString() == "1")
        {
            lblHeader.Text = "Templates";
        }
        if (Session["type"].ToString() != "am")
        {
            DataTable ds = new DataTable();
            //ds = (DataTable)Session["userinfo"];
           
            ds = GetUserById();

            //ProjecttempPermission
            string ProjecttempPermission = ds.Rows[0]["ProjecttempPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["ProjecttempPermission"].ToString();
            hdnAddePTemp.Value = ProjecttempPermission.Length < 1 ? "Y" : ProjecttempPermission.Substring(0, 1);
            hdnEditePTemp.Value = ProjecttempPermission.Length < 2 ? "Y" : ProjecttempPermission.Substring(1, 1);
            hdnDeletePTemp.Value = ProjecttempPermission.Length < 3 ? "Y" : ProjecttempPermission.Substring(2, 1);
            hdnViewPTemp.Value = ProjecttempPermission.Length < 4 ? "Y" : ProjecttempPermission.Substring(3, 1);
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
    #endregion

    protected void RadGrid_ProjectTemplate_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_ProjectTemplate.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["ProTem_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_ProjectTemplate.MasterTableView.OwnerGrid.Columns)
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

            Session["ProTem_Filters"] = filters;
        }
        else
        {
            Session["ProTem_FilterExpression"] = null;
            Session["ProTem_Filters"] = null;
        }
        #endregion  


        foreach (GridDataItem gr in RadGrid_ProjectTemplate.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            if (hdnEditePTemp.Value == "Y" || hdnViewPTemp.Value == "Y")
            {
                gr.Attributes["ondblclick"] = "location.href='addprojecttemp.aspx?uid=" + lblID.Text + "'";
            }
            else
            {
                gr.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }
        }
    }


    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        // Clear grid filter
        ClearGridFilters();

        ProjectTemplateList();
        RadGrid_ProjectTemplate.Rebind();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
    }

   

    private void ClearGridFilters()
    {
        if (Session["ProTem_FilterExpression"] != null && Convert.ToString(Session["ProTem_FilterExpression"]) != "" && Session["ProTem_Filters"] != null)
        {
            foreach (GridColumn column in RadGrid_ProjectTemplate.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            RadGrid_ProjectTemplate.MasterTableView.FilterExpression = string.Empty;
        }


        Session["ProTem_FilterExpression"] = null;
        Session["ProTem_Filters"] = null;
    }


    protected void lnkClear_Click(object sender, EventArgs e)
    {
        // Clear grid filter
        ClearGridFilters();
        // reset search values

        ProjectTemplateList();
        RadGrid_ProjectTemplate.Rebind();
    }


    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_ProjectTemplate.MasterTableView.FilterExpression != "" ||
            (RadGrid_ProjectTemplate.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_ProjectTemplate.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_ProjectTemplate_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_ProjectTemplate.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["ProTem_FilterExpression"] != null && Convert.ToString(Session["ProTem_FilterExpression"]) != "" && Session["ProTem_Filters"] != null)
                {
                    RadGrid_ProjectTemplate.MasterTableView.FilterExpression = Convert.ToString(Session["ProTem_FilterExpression"]);
                    var filtersGet = Session["ProTem_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_ProjectTemplate.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["ProTem_FilterExpression"] = null;
                Session["ProTem_Filters"] = null;
            }
        }
        #endregion
        ProjectTemplateList();
    }

    private void ProjectTemplateList()
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        if (lnkChk.Checked == true)
        {
            objProp_Customer.Status = 1;
        }
        else
        {
            objProp_Customer.Status = 0;
        }
        ds = objBL_Customer.getJobProjectTemplate(objProp_Customer);
        RadGrid_ProjectTemplate.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_ProjectTemplate.DataSource = ds.Tables[0];
        lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString() + " Record(s) found";
        Session["ProjectTemp"] = ds.Tables[0];
    }

    protected void RadGrid_ProjectTemplate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //if (e.CommandName == "UpdateStatus")
        //{
        //    int index = Convert.ToInt32(e.CommandArgument);
        //    GridDataItem row = e.Item as GridDataItem;
        //    Label lblID = (Label)row.FindControl("lblID");

        //    _objJob.ConnConfig = Session["config"].ToString();
        //    _objJob.ID = Convert.ToInt32(lblID.Text);
        //    objBL_Customer.UpdateTemplateStatus(_objJob);

        //    ProjectTemplateList();
        //    RadGrid_ProjectTemplate.Rebind();
        //}
    }
    protected void RadGrid_ProjectTemplate_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (Convert.ToString(RadGrid_ProjectTemplate.MasterTableView.FilterExpression) != "")
                    lblRecordCount.Text = totalCount + " Record(s) found";
                else
                    lblRecordCount.Text = RadGrid_ProjectTemplate.VirtualItemCount + " Record(s) found";
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
    protected void RadGrid_ProjectTemplate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //try
        //{
        //    switch (e.Item.ItemType)
        //    {
        //        case GridItemType.Item:

        //            GridDataItem row = e.Item as GridDataItem;
        //            LinkButton btnStatus = (LinkButton)row.FindControl("btnStatus");
        //            Label lblStatus = (Label)row.FindControl("lblStatus");

        //            if (lblStatus.Text.ToString().Equals("Active"))
        //            {
        //                btnStatus.CssClass = "active";
        //            }
        //            else
        //            {
        //                btnStatus.CssClass = "dactive";
        //            }

        //            break;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
    }

    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        ProjectTemplateList();
        RadGrid_ProjectTemplate.Rebind();
    }

    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_ProjectTemplate.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            Response.Redirect("addprojecttemp.aspx?uid=" + lblID.Text + "&t=c");
        }
    }
}
