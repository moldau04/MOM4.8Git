using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using Telerik.Web.UI;
using BusinessLayer.Billing;

public partial class BillingCodes : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

        if (Request.Url.Scheme == "http" && SSL == "1")
        {
            string URL = Request.Url.ToString();
            URL = URL.Replace("http://", "https://");
            Response.Redirect(URL);
        }

        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
      
        Permission();
        HighlightSideMenu("acctMgr", "lnkBillcodeSMenu", "billMgrSub");
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
       
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();


            /// BillingmodulePermission ///////////////////------->

            string BillingmodulePermission = ds.Rows[0]["BillingmodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["BillingmodulePermission"].ToString();

            if (BillingmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// BillingCodesPermission ///////////////////------->

            string BillingCodesPermission = ds.Rows[0]["BillingCodesPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["BillingCodesPermission"].ToString();
            string ADD = BillingCodesPermission.Length < 1 ? "Y" : BillingCodesPermission.Substring(0, 1);
            string Edit = BillingCodesPermission.Length < 2 ? "Y" : BillingCodesPermission.Substring(1, 1);
            string Delete = BillingCodesPermission.Length < 2 ? "Y" : BillingCodesPermission.Substring(2, 1);
            string View = BillingCodesPermission.Length < 4 ? "Y" : BillingCodesPermission.Substring(3, 1);
            hdnIsEdit.Value = Edit;
            if (ADD == "N")
            {
                lnkAddnew.Visible = false;  
            }
            if (Edit == "N")
            {
               
                lnkBtnEdit.Visible = false;
            }
            if (Delete == "N")
            {
                lnkDeleteBillingCodes.Visible = false; 
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
    private void FillBillCodes()
    {
        DataSet ds = new DataSet();
        DataTable dtFinal = new DataTable();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Type = "1";
        ds =  new BL_BillCodes().getBillCodes(objProp_User);
        if (lnkChk.Checked)
        {
            dtFinal = ds.Tables[0];
        }
        else
        {
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    dtFinal = ds.Tables[0].Select("Statusid = 0").CopyToDataTable();
                }
                catch(Exception ex) { }
            }
        }
        BindGridDatatable(dtFinal);
    }

    private void BindGridDatatable(DataTable dt)
    {
        Session["BillCodeSrch"] = dt;
        RadGrid_BillCodes.VirtualItemCount = dt.Rows.Count;
        RadGrid_BillCodes.DataSource = dt;
       // lblRecordCount.Text = dt.Rows.Count + " Record(s) found";
      
    }
    protected void lnkDeleteBillingCodes_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem di in RadGrid_BillCodes.SelectedItems)
            {
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblBillID = (Label)di.FindControl("lblIndexID");
                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.BillCode = Convert.ToInt32(lblBillID.Text);
                    new BL_BillCodes().DeleteBillingCode(objProp_User);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Billing Code deleted successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    FillBillCodes();
                    RadGrid_BillCodes.Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.Message == "This is default billing code. You cannot delete!")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningbillcodes", "noty({text: 'This is default billing code. You cannot delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
            else if (ex.Message == "This billing code is in use and cannot be deleted!")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningbillcodes", "noty({text: 'This billing code is in use and cannot be deleted!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelbillcodes", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }  
    protected void lnkBillingCodesSave_Click(object sender, EventArgs e)
    {
        Boolean changeDefault = false;
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.ContactName = txtBillCode.Text;
        objProp_User.SalesDescription = txtBillDesc.Text.Replace("\n", "<br /> \r\n").Replace("'", "''"); 
        objProp_User.CatStatus = Convert.ToInt32(ddlStatus.SelectedValue);
        objProp_User.Balance = Convert.ToDouble(txtBillBal.Text);
        objProp_User.Measure = txtBillMeasure.Text;
        objProp_User.Remarks = txtBillRemarks.Text.Replace("\n", "<br /> \r\n").Replace("'", "''");
        objProp_User.Type = "1";
        if (hdnPatientId.Value != string.Empty)
            objProp_User.sAcct = Convert.ToInt32(hdnPatientId.Value);
        else
            objProp_User.sAcct = 0;
        if (hdnAddEdit.Value == "0")
        {
            objBL_User.AddBillCode(objProp_User);            
          
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Billing Code added successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseBillingCodeWindow();", true);
        }
        else if (hdnAddEdit.Value == "1")
        {
            objProp_User.BillCode = Convert.ToInt32(hdnBillID.Value);
            if (objProp_User.ContactName.ToLower() != hdnBillName.Value.ToLower())
            {
                if (hdnBillName.Value.ToLower() == "recurring" || hdnBillName.Value.ToLower() == "time spent" || hdnBillName.Value.ToLower() == "mileage" || hdnBillName.Value.ToLower() == "expenses")
                {
                    objProp_User.ContactName = hdnBillName.Value;
                    changeDefault = true;
                }
            }
           
            objBL_User.UpdateBillCode(objProp_User);          
            if (changeDefault)
            {    
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'The billing code name cannot be edited!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Billing Code updated successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
          
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseBillingCodeWindow();", true);
        }
        objProp_User.Type = null;
        FillBillCodes();
        RadGrid_BillCodes.Rebind();
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        lnkChk.Checked = false;
       
        foreach (GridColumn column in RadGrid_BillCodes.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
            column.ListOfFilterValues = null;
        }
        RadGrid_BillCodes.MasterTableView.FilterExpression = string.Empty;       

        FillBillCodes();
        RadGrid_BillCodes.PageSize = 50;
        RadGrid_BillCodes.MasterTableView.PageSize = 50;
        RadGrid_BillCodes.Rebind();
        
    }
    protected void lnkchk_Click(object sender, EventArgs e)
    {
        FillBillCodes();
        RadGrid_BillCodes.Rebind();
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_BillCodes.MasterTableView.FilterExpression != "" ||
            (RadGrid_BillCodes.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_BillCodes.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_BillCodes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_BillCodes.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        FillBillCodes();
    }
    protected void RadGrid_BillCodes_ItemCreated(object sender, GridItemEventArgs e)
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
    protected void RadGrid_BillCodes_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        updpnl.Update();
    }

    protected void RadGrid_BillCodes_PreRender(object sender, EventArgs e)
    {

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        RadGrid_BillCodes.ExportSettings.FileName = "BillingCodes";
        RadGrid_BillCodes.ExportSettings.IgnorePaging = true;
        RadGrid_BillCodes.ExportSettings.ExportOnlyData = true;
        RadGrid_BillCodes.ExportSettings.OpenInNewWindow = true;
        RadGrid_BillCodes.ExportSettings.HideStructureColumns = true;
        RadGrid_BillCodes.MasterTableView.UseAllDataFields = true;
        RadGrid_BillCodes.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_BillCodes.MasterTableView.ExportToExcel();
    }
}
