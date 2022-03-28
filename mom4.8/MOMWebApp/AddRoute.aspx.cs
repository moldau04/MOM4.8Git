using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AddRoute : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    GeneralFunctions objGn = new GeneralFunctions();
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
        if (Request.QueryString["uid"] == null)
        {
            Page.Title = "Add Route || MOM";
            lnkFirst.Visible = false;
            lnkPrevious.Visible = false;
            lnkNext.Visible = false;
            lnkLast.Visible = false;
            lblRouteWorkerName.Visible = false;
        }
        if (Request.QueryString["uid"] != null)
        {
            Page.Title = "Edit Route || MOM";
            tbLogs.Visible = true;
        }
        if (!IsPostBack)
        {
            FillWorker();
            GetData();
            SetDefaultWorker();
            HighlightSideMenu("schMgr", "lnkRoutes", "schdMgrSub");
        }
    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
    //    HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
    //    //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
    //    aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

    //    //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
    //    //a.Style.Add("color", "#2382b2");

    //    HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
    //    //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
    //    lnkUsersSmenu.Style.Add("color", "#316b9d");
    //    lnkUsersSmenu.Style.Add("font-weight", "normal");
    //    lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
    //    //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
    //    //hm.Enabled = false;
    //    HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
    //    div.Style.Add("display", "block");
    }
    private void FillWorker()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 1;
        ds = objBL_User.getEMP(objPropUser);
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "fDesc";
        ddlRoute.DataValueField = "ID";
        ddlRoute.DataBind();
        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void GetData()
    {
        if (Request.QueryString["uid"] != null)
        {
            lblHeader.Text = "Edit Route";
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Route = Convert.ToInt32(Request.QueryString["uid"].ToString());
            DataSet ds = objBL_User.getRouteByID(objPropUser);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["mech"].ToString();
                lblRouteWorkerName.Text = txtName.Text + " | " + ddlRoute.SelectedItem.Text;
                txtremarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                ddlStatus.SelectedValue = Convert.ToBoolean(ds.Tables[0].Rows[0]["Status"]) ? "1" : "0";
                lblLocCount.Text = ds.Tables[0].Rows[0]["LocCount"].ToString();
                lblContCount.Text = ds.Tables[0].Rows[0]["ContCount"].ToString();

                string color = ds.Tables[0].Rows[0]["Color"].ToString();

                if (!string.IsNullOrEmpty(color))
                {
                    txtColor.SelectedColor = System.Drawing.ColorTranslator.FromHtml("#" + color);
                }
                if (isFirstItem())
                {
                    lnkFirst.Enabled = false;
                    lnkPrevious.Enabled = false;
                }
                if (isLastItem())
                {
                    lnkLast.Enabled = false;
                    lnkNext.Enabled = false;
                }
            }
        }
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.ContactName = txtName.Text.Trim();
            objPropUser.WorkId = Convert.ToInt32(ddlRoute.SelectedValue);
            objPropUser.Remarks = txtremarks.Text;
            objPropUser.Color = ColorTranslator.ToHtml(txtColor.SelectedColor).Replace("#", "");
            objPropUser.MOMUSer = Session["User"].ToString();
            objPropUser.Status = Convert.ToInt16(hdnStatus.Value);
            if (Request.QueryString["uid"] != null)
            {
                objPropUser.Route = Convert.ToInt32(Request.QueryString["uid"].ToString());
                int res = objBL_User.AddRoute(objPropUser);
                if (res == 1)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: '" + hdnDefaultVal.Value + " Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj1", "noty({text: 'Please move assigned locations before making it inactive!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                }

                RadGrid_gvLogs.Rebind();
            }
            else
            {
                objBL_User.AddRoute(objPropUser);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: '" + hdnDefaultVal.Value + " Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                objGn.ResetFormControlValues(this);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("routes.aspx");
    }
    private bool isFirstItem()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 1;
        DataSet ds = objBL_User.getRoutesGrid(objPropUser);
        DataTable dt = ds.Tables[0];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["id"];
        dt.PrimaryKey = keyColumns;
        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        if (index == 0)
            return true;
        return false;
    }
    private bool isLastItem()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 1;
        DataSet ds = objBL_User.getRoutesGrid(objPropUser);
        DataTable dt = ds.Tables[0];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["id"];
        dt.PrimaryKey = keyColumns;
        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        if (index == dt.Rows.Count - 1)
            return true;
        return false;
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 1;
        DataSet ds = objBL_User.getRoutesGrid(objPropUser);
        DataTable dt = ds.Tables[0];
        Response.Redirect("addroute.aspx?uid=" + dt.Rows[0]["id"]);
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 1;
        DataSet ds = objBL_User.getRoutesGrid(objPropUser);
        DataTable dt = ds.Tables[0];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["id"];
        dt.PrimaryKey = keyColumns;
        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        if (index > 0)
        {
            Response.Redirect("addroute.aspx?uid=" + dt.Rows[index - 1]["id"]);
        }
        if (index == 0)
        {
            Response.Redirect("addroute.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["id"]);
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 1;
        DataSet ds = objBL_User.getRoutesGrid(objPropUser);
        DataTable dt = ds.Tables[0];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["id"];
        dt.PrimaryKey = keyColumns;
        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            Response.Redirect("addroute.aspx?uid=" + dt.Rows[index + 1]["id"]);
        }
        if (index == c)
        {
            Response.Redirect("addroute.aspx?uid=" + dt.Rows[0]["id"]);
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 1;
        DataSet ds = objBL_User.getRoutesGrid(objPropUser);
        DataTable dt = ds.Tables[0];
        Response.Redirect("addroute.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["id"]);
    }
    #region logs
    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        if (Request.QueryString["uid"] != null)
        {
            DataSet dsLog = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Route = Convert.ToInt32(Request.QueryString["uid"].ToString());
            dsLog = objBL_User.GetRouteLogs(objPropUser);
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

    private void SetDefaultWorker()
    {
        Customer objCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();

        objCustomer.ConnConfig = Session["config"].ToString();
        string getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        if (!string.IsNullOrEmpty(getValue))
        {
            if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                lblHeader.Text = "Edit " + getValue;
            }
            else
            {
                lblHeader.Text = "Add " + getValue;
            }
            dvRoutesInfo.InnerHtml = getValue + " Info";
            hdnDefaultVal.Value = getValue;
        }
        else
        {
            if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                lblHeader.Text = "Edit Default Worker";
            }
            else
            {
                lblHeader.Text = "Add Default Worker";
            }
            dvRoutesInfo.InnerHtml = "Default Worker Info";
            hdnDefaultVal.Value = "Default Worker";
        }

    }
}