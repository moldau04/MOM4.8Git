using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessEntity;
using BusinessLayer;
using Microsoft.ApplicationBlocks.Data;
using Telerik.Web.UI;

public partial class AdminPanel : System.Web.UI.Page
{
    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();

    General objgeneral = new General();
    BL_General objBL_General = new BL_General();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["MSM"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (Session["MSM"].ToString() != "ADMIN")
        {
            Response.Redirect("home.aspx");
        }

        if (!IsPostBack)
        {
            LoadUserInfo();
           
            //FillControl();
        }
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addcompany.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            Label lblDBName = new Label();
            if(RadGrid_AP.SelectedItems!=null && RadGrid_AP.SelectedItems.Count > 0)
            {
                foreach (GridDataItem item in RadGrid_AP.SelectedItems)
                {
                    Label lblID = (Label)item.Cells[1].FindControl("lblId");
                    lblDBName = (Label)item.Cells[1].FindControl("lblDBName");
                    DropDatabase(Convert.ToInt32(lblID.Text));
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "delete", "noty({text: '" + lblDBName.Text + " deleted successfully.',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            else{
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "delete", "noty({text: 'Please select an item to delete',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }

            RadGrid_AP.Rebind();            
        }
        catch (Exception ex)
        {
            lblMsgAdmin.Text = ex.Message;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "delete", "noty({text: '" + ex.Message + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }        
    }

    private void FillControl()
    {
        DataSet ds = new DataSet();

        ds = objBL_User.getAdminControl(objPropUser);
        RadGrid_AP.DataSource = ds.Tables[0];
        //gvControl.DataBind();
        Session["ctrldata"] = ds.Tables[0];
    }

    //protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    //{
    //    GridViewRow gvrPager = RadGrid_AP.BottomPagerRow;
    //    DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

    //    gvControl.PageIndex = ddlPages.SelectedIndex;

    //    // a method to populate your grid
    //    FillGridPaged();
    //}

    //protected void Paginate(object sender, CommandEventArgs e)
    //{
    //    // get the current page selected
    //    int intCurIndex = gvControl.PageIndex;

    //    switch (e.CommandArgument.ToString().ToLower())
    //    {
    //        case "first":
    //            gvControl.PageIndex = 0;
    //            break;
    //        case "prev":
    //            gvControl.PageIndex = intCurIndex - 1;
    //            break;
    //        case "next":
    //            gvControl.PageIndex = intCurIndex + 1;
    //            break;
    //        case "last":
    //            gvControl.PageIndex = gvControl.PageCount;
    //            break;
    //    }

    //    // popultate the gridview control
    //    FillGridPaged();
    //}

    //private void FillGridPaged()
    //{
    //    DataTable dt = new DataTable();

    //    dt = PageSortData();
    //    gvControl.DataSource = dt;
    //    gvControl.DataBind();
    //}

    //private DataTable PageSortData()
    //{
    //    DataTable dt = new DataTable();
    //    dt = (DataTable)Session["ctrldata"];
    //    return dt;
    //}

    //protected void gvControl_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    string sortExpression = e.SortExpression;

    //    if (GridViewSortDirection == SortDirection.Ascending)
    //    {
    //        GridViewSortDirection = SortDirection.Descending;
    //        SortGridView(sortExpression, DESCENDING);
    //    }
    //    else
    //    {
    //        GridViewSortDirection = SortDirection.Ascending;
    //        SortGridView(sortExpression, ASCENDING);
    //    }
    //}

    //private void SortGridView(string sortExpression, string direction)
    //{
    //    DataTable dt = PageSortData();

    //    DataView dv = new DataView(dt);
    //    dv.Sort = sortExpression + direction;

    //    gvControl.DataSource = dv;
    //    gvControl.DataBind();
    //}

    //protected void gvControl_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gvControl.PageIndex = e.NewPageIndex;
    //    DataTable dt = new DataTable();
    //    dt = PageSortData();
    //    gvControl.DataSource = dt;
    //    gvControl.DataBind();
    //}

    //public SortDirection GridViewSortDirection
    //{
    //    get
    //    {
    //        if (ViewState["sortDirection"] == null)
    //            ViewState["sortDirection"] = SortDirection.Ascending;

    //        return (SortDirection)ViewState["sortDirection"];
    //    }
    //    set { ViewState["sortDirection"] = value; }
    //}

    //protected void gvControl_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        string gvrow = ((GridView)sender).DataKeys[e.Row.RowIndex].Value.ToString();
    //        HiddenField hdnSelected = (HiddenField)e.Row.FindControl("hdnSelected");
    //        CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
    //        Label lblID = (Label)e.Row.FindControl("lblId");

    //        e.Row.Attributes["ondblclick"] = "location.href='addexistingdb.aspx?id=" + gvrow + "'";
    //        e.Row.Attributes["onclick"] = "SelectRow('" + hdnSelected.ClientID + "','" + e.Row.ClientID + "','" + chkSelect.ClientID + "','" + gvControl.ClientID + "',event);";

    //    }
    //}

    //protected void gvControl_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    Paginate(sender, e);
    //}

    //protected void gvControl_DataBound(object sender, EventArgs e)
    //{
    //    GridViewRow gvrPager = gvControl.BottomPagerRow;

    //    if (gvrPager == null) return;

    //    // get your controls from the gridview
    //    DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
    //    Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

    //    if (ddlPages != null)
    //    {
    //        // populate pager
    //        for (int i = 0; i < gvControl.PageCount; i++)
    //        {

    //            int intPageNumber = i + 1;
    //            ListItem lstItem = new ListItem(intPageNumber.ToString());

    //            if (i == gvControl.PageIndex)
    //                lstItem.Selected = true;

    //            ddlPages.Items.Add(lstItem);
    //        }
    //    }

    //    // populate page count
    //    if (lblPageCount != null)
    //        lblPageCount.Text = gvControl.PageCount.ToString();
    //}

    //protected void gvControl_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    gvControl.EditIndex = e.NewEditIndex;
    //    FillControl();
    //}

    private void DropDatabase(int id)
    {
        //string constr = Config.MS;
        //SqlConnection connection = new SqlConnection(constr);
        //Server serversql = new Server(new ServerConnection(connection));        
        //serversql.Databases[dbname].Drop();

        objPropUser.CtrlID = id;
        objBL_User.DeleteCompany(objPropUser);
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        //TogglePopup();
    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        try
        {
            objPropUser.Password = SSTCryptographer.Encrypt(txtAdminPass.Text.Trim(), "pass");
            objPropUser.Username = txtAdminUser.Text.Trim();

            objBL_User.UpdateAdminPassword(objPropUser);
            lblMsgAdmin.Text = "Password updated successfully.";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Password updated successfully.',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            lblMsgAdmin.Text = ex.Message;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '"+ ex.Message + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    private void ConnectionStr(string dbname)
    {
        string server = Config.MS.Split(';')[0].Split('=')[1];
        string database = dbname;
        string user = Config.MS.Split(';')[2].Split('=')[1];
        string pass = Config.MS.Split(';')[3].Split('=')[1];

        string constr = "server=" + server + ";database=" + database + ";user=" + user + ";password=" + pass + "";
        Session["config"] = constr;
    }
    //private void TogglePopup()
    //{
       
    //    if (pnlOverlay.Visible == false)
    //    {
    //        pnlOverlay.Visible = true;
    //        pnlContact.Visible = true;
    //        this.programmaticModalPopup.Show();
    //    }
    //    else
    //    {
    //        pnlOverlay.Visible = false;
    //        pnlContact.Visible = false;
    //        this.programmaticModalPopup.Hide();
    //    }
    //}

    //private void ToggleGPSPopup()
    //{
    //    if (pnlOverlay.Visible == false)
    //    {
    //        pnlOverlay.Visible = true;
    //        pnlGPSSett.Visible = true;
    //        this.programmaticModalPopup.Show();
    //    }
    //    else
    //    {
    //        pnlOverlay.Visible = false;
    //        pnlGPSSett.Visible = false;
    //        this.programmaticModalPopup.Hide();
    //    }
    //}

    protected void lnkExisting_Click(object sender, EventArgs e)
    {
        Response.Redirect("addexistingdb.aspx?Type=MSM");
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_AP.Items)
        {
            HiddenField hdnSelected = (HiddenField)di.Cells[1].FindControl("hdnSelected");
            Label lblDBname = (Label)di.Cells[2].FindControl("lblDBname");
            Label lbltype = (Label)di.Cells[4].FindControl("lbltype");
            Label lblID = (Label)di.Cells[1].FindControl("lblId");

            if (hdnSelected.Value == "1")
            {
                Response.Redirect("addexistingdb.aspx?id=" + lblID.Text);
            }
        }
    }
    private void LoadUserInfo()
    {
        lblMsgAdmin.Text = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            ds = objBL_User.GetAdminPassword(objPropUser);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtAdminPass.Text = SSTCryptographer.Decrypt(ds.Tables[0].Rows[0]["password"].ToString(), "pass");
                txtAdminUser.Text = ds.Tables[0].Rows[0]["username"].ToString();
            }
        }
        catch (Exception ex)
        {
            lblMsgAdmin.Text = ex.Message;
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        lblMsgAdmin.Text = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            ds = objBL_User.GetAdminPassword(objPropUser);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtAdminPass.Text = SSTCryptographer.Decrypt(ds.Tables[0].Rows[0]["password"].ToString(), "pass");
                txtAdminUser.Text = ds.Tables[0].Rows[0]["username"].ToString();
            }
        }
        catch (Exception ex)
        {
            lblMsgAdmin.Text = ex.Message;
        }        
        //TogglePopup();
    }
    //protected void lnkGPS_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        objgeneral.GPSInterval = Convert.ToInt32(ddlGPSPing.SelectedValue);
    //        objBL_General.InsertGPSInterval(objgeneral);
    //        lblMSgGPS.Text = "GPS settings updated successfully.";
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'GPS settings updated successfully.',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMSgGPS.Text = ex.Message;
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + ex.Message + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //}
    protected void lnkCloseGPS_Click(object sender, EventArgs e)
    {
        //ToggleGPSPopup();
    }
    //protected void lnkGpsSettings_Click(object sender, EventArgs e)
    //{
    //    string strGPSPing = objBL_General.GetGPSInterval(objgeneral);

    //    if (strGPSPing != string.Empty)
    //    {
    //        ddlGPSPing.SelectedValue = strGPSPing;
    //    }
    //    lblMSgGPS.Text = "";
    //    ToggleGPSPopup();
    //}


    protected void RadGrid_AP_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        FillControl();
    }

    protected void RadGrid_AP_ItemCreated(object sender, GridItemEventArgs e)
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
                dropDown.Items.Add(cboItem);
            }
            dropDown.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;

        }
    }

    protected void RadGrid_AP_PreRender(object sender, EventArgs e)
    {
        RowSelect();
    }
    private void RowSelect()
    {
        try
        {
            foreach (GridDataItem item in RadGrid_AP.Items)
            {
                Label lblDBID = (Label)item.FindControl("lblId");
                item.Attributes["ondblclick"] = "window.open('addexistingdb.aspx?id=" + lblDBID.Text + "','_self');";
            }
        }
        catch (Exception)
        {
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    protected void lnkTS_Click(object sender, EventArgs e)
    {
        Response.Redirect("addexistingdb.aspx?Type=TS");
    }
}
