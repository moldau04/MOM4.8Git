using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using BusinessEntity;
using BusinessLayer;
using Telerik.Web.UI;

public partial class Routes : System.Web.UI.Page
{
    #region "Variables"
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    private static readonly string CookieName = "Rad_Routes";
    protected void Page_Init(object sender, EventArgs e)
    {
        RadPersistenceRoutes.StorageProviderKey = CookieName;
        RadPersistenceRoutes.StorageProvider = new CookieStorageProvider(CookieName);
    }
    #endregion
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        //var masterHeader = Master.FindControl("divHeader");
        //masterHeader.Visible = false;
        //var masterMenu = Master.FindControl("menu");
        //masterMenu.Visible = false;

        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            if (Request.Cookies[CookieName] != null)
            {
                RadPersistenceRoutes.LoadState();
                RadGrid_Routes.Rebind();
                updpnl.Update();
            }
            Permission();
            //HighlightSideMenu("schMgr", "lnkRoutes", "schdMgrSub");
        }
        CompanyPermission();
        ConvertToJSON();
    }
    #endregion
    #region Custom Function

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        //HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        ////li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        //aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        ////HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        ////a.Style.Add("color", "#2382b2");

        //HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        ////lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        //lnkUsersSmenu.Style.Add("color", "#316b9d");
        //lnkUsersSmenu.Style.Add("font-weight", "normal");
        //lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        ////AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        ////hm.Enabled = false;
        //HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        //div.Style.Add("display", "block");
    }

    private void FillRoute()
    {
        SetDefaultWorker();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion

        objPropUser.IsActiveInactive = lnkChk.Checked;

        ds = objBL_User.getRoutesGrid(objPropUser);
        if (ds.Tables[0].Rows.Count > 0)
        {

            RadGrid_Routes.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Routes.DataSource = ds.Tables[0];
            RadPersistenceRoutes.SaveState();
        }
    }
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_Routes.Columns[4].Visible = true;
        }
        else
        {
            RadGrid_Routes.Columns[4].Visible = false;
        }
    }
    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportsName());
        string reports = "var reports=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", reports, true);
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
    }
    #endregion
    #region Events
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Routes.SelectedItems)
        {
            Label lblID = (Label)di.FindControl("lblID");
            Response.Redirect("addroute.aspx?uid=" + lblID.Text);
        }
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        bool Flag = false;
        try
        {
            foreach (GridDataItem di in RadGrid_Routes.SelectedItems)
            {
                Flag = true;
                Label lblID = (Label)di.FindControl("lblID");
                objPropUser.ID = Convert.ToInt32(lblID.Text);
                objPropUser.ConnConfig = Session["config"].ToString();
                objBL_User.DeleteRoutes(objPropUser);
                FillRoute();
                RadGrid_Routes.Rebind();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "DeleteSuccessMesg();", true);
            }
            if (!Flag)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "closedMesg();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("addroute.aspx");
    }
    //protected void lnkClose_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("home.aspx");
    //}

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Routes.MasterTableView.FilterExpression != "" ||
            (RadGrid_Routes.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Routes.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Routes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Routes.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        FillRoute();
    }
    protected void RadGrid_Routes_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        updpnl.Update();
    }
    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_Routes.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkName");
            Panel txtColor = (Panel)gr.FindControl("txtColor");
            Label lblColor = (Label)gr.FindControl("lblColor");
            txtColor.Style.Add("background-color", "#" + lblColor.Text);
            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addroute.aspx?uid=" + lblID.Text + "'";
        }
    }
    protected void RadGrid_Routes_PreRender(object sender, EventArgs e)
    {
        RowSelect();
    }
    protected void RadGrid_Routes_ItemCreated(object sender, GridItemEventArgs e)
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
    private List<CustomerReport> GetReportsName()
    {
        List<CustomerReport> lstCustomerReport = new List<CustomerReport>();
        try
        {
            DataSet dsGetReports = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objProp_User.Type = "Route";
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

    protected void lnkchk_Click(object sender, EventArgs e)
    {
        FillRoute();
        RadGrid_Routes.Rebind();
    }
    private void SetDefaultWorker()
    {
        Customer objCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();

        var masterTableView = RadGrid_Routes.MasterTableView;
        var column = masterTableView.GetColumn("DRoute");
        objCustomer.ConnConfig = Session["config"].ToString();
        string getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        if (!string.IsNullOrEmpty(getValue))
        {
            column.HeaderText = getValue;
            spnHead.InnerHtml = getValue;
        }
        else
        {
            column.HeaderText = "Default Worker";
            spnHead.InnerHtml = "Default Worker";
        }

    }
}