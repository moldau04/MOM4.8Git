using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using Telerik.Web.UI;
using Telerik.Web.UI.PersistenceFramework;
using System.Linq.Dynamic;
using Telerik.Web.UI.GridExcelBuilder;
using BusinessEntity.APModels;
using BusinessEntity.Utility;
using MOMWebApp;

public partial class WageDeductList : System.Web.UI.Page
{
    #region "Variables"
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BL_Wage objBL_Wage = new BL_Wage();
    PRDed objProp_PRDed = new PRDed();
    public static bool check = false;
    public static bool IsAddEdit = false;
    public static bool IsDelete = false;
    BL_BankAccount _objBLBank = new BL_BankAccount();

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }
        
        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            BindSearchFilters();
            
            #region Show Selected Filter
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["ddlSearch_Vendor"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearch_Vendor"]);
                    ddlSearch.SelectedValue = selectedValue;

                    String searchValue = Convert.ToString(Session["ddlSearch_Value_Vendor"]);
                    if (selectedValue == "Rol.Type")
                    {
                        ddlType.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "Vendor.Status")
                    {
                        ddlStatus.SelectedValue = searchValue;
                    }
                    {
                        txtSearch.Text = searchValue;
                    }

                    SelectSearch();
                }
            }
            else
            {
                Session["ddlSearch_Vendor"] = null;
                Session["ddlSearch_Value_Vendor"] = null;
            }
            #endregion
            FillFilter();

            Permission();
            HighlightSideMenu("prID", "wagedeductionslink", "payrollmenutab");
        }
        CompanyPermission();
        //ConvertToJSON();
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
    public bool CheckAddEditPermission()
    {
        bool result = true;
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            /// Vendor ///////////////////------->

            string VendorPermission = ds.Rows[0]["Vendor"] == DBNull.Value ? "YYYY" : ds.Rows[0]["Vendor"].ToString();
            string ViewVendor = VendorPermission.Length < 4 ? "Y" : VendorPermission.Substring(3, 1);
            if (ViewVendor == "N")
            {
                result = false;
            }
        }

        return result;
    }

    

    private void Permission()
    {
       
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

        if (Session["type"].ToString() != "am")
        {
            DataTable dtUserPermission = new DataTable();
            dtUserPermission = GetUserById();
            /// AccountPayablemodulePermission ///////////////////------->

            string AccountPayablemodulePermission = dtUserPermission.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : dtUserPermission.Rows[0]["AccountPayablemodulePermission"].ToString();

            if (AccountPayablemodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            DataTable ds = new DataTable();

           // ds = (DataTable)Session["userinfo"];

            //VendorsPermission
            string VendorsPermission = dtUserPermission.Rows[0]["Vendor"] == DBNull.Value ? "YYYY" : dtUserPermission.Rows[0]["Vendor"].ToString();

            hdnAddDedcutions.Value = VendorsPermission.Length < 1 ? "Y" : VendorsPermission.Substring(0, 1);
            hdnEditDedcutions.Value = VendorsPermission.Length < 2 ? "Y" : VendorsPermission.Substring(1, 1);
            hdnDeleteDedcutions.Value = VendorsPermission.Length < 3 ? "Y" : VendorsPermission.Substring(2, 1);
            hdnViewDedcutions.Value = VendorsPermission.Length < 4 ? "Y" : VendorsPermission.Substring(3, 1);
             if (hdnAddDedcutions.Value == "N")
            {

                lnkAddnew.Visible = false;
            }
            if (hdnEditDedcutions.Value == "N")
            {
                btnEdit.Visible = false;               
            }
            if (hdnDeleteDedcutions.Value == "N")
            {
                lnkDelete.Visible = false;

            }
            if (hdnViewDedcutions.Value == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            //if (hdnAddDedcutions.Value == "N")
            //{
            //    lnkAddnew.Enabled = false;
            //}
            //if (hdnEditDedcutions.Value == "N")
            //{
            //    btnEdit.Enabled = false;
            //}
            //if (hdnDeleteDedcutions.Value == "N")
            //{
            //    lnkDelete.Enabled = false;
            //}
        }
        else
        {
            hdnAddDedcutions.Value = "Y";
            hdnEditDedcutions.Value = "Y";
            hdnDeleteDedcutions.Value = "Y";
            hdnViewDedcutions.Value = "Y";
        }
    }
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();

        GetUserByIdParam _GetUserById = new GetUserByIdParam();
        _GetUserById.TypeID = Convert.ToInt32(Session["usertypeid"]);
        _GetUserById.UserID = Convert.ToInt32(Session["userid"]);
        _GetUserById.ConnConfig = Session["config"].ToString();
        _GetUserById.DBName = Session["dbname"].ToString();

        DataSet ds = new DataSet();

        
            ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            //RadGrid_WageDeduction.Columns[9].Visible = true;
        }
        else
        {
            //RadGrid_WageDeduction.Columns[9].Visible = false;
            Session["CmpChkDefault"] = "2";
        }
    }

    private void BindSearchFilters()
    {
        Dictionary<string, string> listsearchitems = new Dictionary<string, string>();
        listsearchitems.Add("0", "Select");
        listsearchitems.Add("Vendor.Acct", "Vendor ID");
        listsearchitems.Add("Rol.Name", "Vendor name");
        listsearchitems.Add("Rol.Address", "Address");
        listsearchitems.Add("Rol.Contact", "Contact");
        listsearchitems.Add("Rol.Phone", "Phone");
        listsearchitems.Add("Rol.EMail", "Email");
        listsearchitems.Add("Rol.Type", "Type");
        listsearchitems.Add("Vendor.Status", "Status");


        ddlSearch.DataSource = listsearchitems;
        ddlSearch.DataTextField = "Value";
        ddlSearch.DataValueField = "Key";
        ddlSearch.DataBind();

    }

    // Start : Fill Type DropDownList : Juily 27-12-2019 
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectSearch();
    }
    private void SelectSearch()
    {
        if (ddlSearch.SelectedValue == "Rol.Type")
        {
            ddlType.Visible = true;
            ddlStatus.Visible = false;
            txtSearch.Visible = false;
            txtSearch.Text = "";            
        }
        else if (ddlSearch.SelectedValue == "Vendor.Status")
        {
            ddlType.Visible = false;
            ddlStatus.Visible = true;
            txtSearch.Visible = false;
            txtSearch.Text = "";
            ddlType.SelectedIndex = 0;
        }
        else
        {
            ddlType.Visible = false;
            ddlStatus.Visible = false;
            txtSearch.Visible = true;
            ddlType.SelectedIndex = 0;
            
        }
        upPannelSearch.Update();
    }
    private void SaveFilter()
    {
        Dictionary<string, string> dictFilter = new Dictionary<string, string>();
        dictFilter["Search"] = ddlSearch.SelectedValue;
        dictFilter["status"] = ddlStatus.SelectedValue;
        dictFilter["type"] = ddlType.SelectedValue;
        dictFilter["searchtxt"] = txtSearch.Text.Trim();
        Session["FilterVendor"] = dictFilter;
    }
    private void FillFilter()
    {
        if (Session["FilterVendor"] != null)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                Dictionary<string, string> dictFilter = new Dictionary<string, string>();
                dictFilter = (Dictionary<string, string>)Session["FilterVendor"];
                ddlSearch.SelectedValue = dictFilter["Search"];
                SelectSearch();
                ddlStatus.SelectedValue = dictFilter["status"];
                ddlType.SelectedValue = dictFilter["type"];
                txtSearch.Text = dictFilter["searchtxt"];
            }
            else
            {
                Session["FilterVendor"] = null;
            }
        }
    }
    
    
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        #region Search Filter
        SelectSearch();
        upPannelSearch.Update();
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_Vendor"] = selectedValue;

        if (selectedValue == "Rol.Type")
        {
            Session["ddlSearch_Value_Vendor"] = ddlType.SelectedValue;
        }
        else if (selectedValue == "Vendor.Status")
        {
            Session["ddlSearch_Value_Vendor"] = ddlStatus.SelectedValue;
        }
        else
        {
            Session["ddlSearch_Value_Vendor"] = txtSearch.Text;
        }
        #endregion

        RadGrid_WageDeduction.CurrentPageIndex = 0;
        RadGrid_WageDeduction.PageSize = 50;
        GetWageDeductionList();
        RadGrid_WageDeduction.Rebind();
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        ddlType.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        SelectSearch();
        foreach (GridColumn column in RadGrid_WageDeduction.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
            RadGrid_WageDeduction.MasterTableView.SortExpressions.Clear();
        Session["Deduction_FilterExpression"] = null;
        Session["Deduction_Filters"] = null;
        RadGrid_WageDeduction.MasterTableView.FilterExpression = "";
        RadGrid_WageDeduction.CurrentPageIndex = 1;        
        upPannelSearch.Update();
        GetWageDeductionList();
        RadGrid_WageDeduction.Rebind();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_WageDeduction.SelectedItems)
        {
            Label lblDeductID = (Label)item.FindControl("lblId");
            Response.Redirect("WageDeduction.aspx?id=" + lblDeductID.Text);
        }
    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("WageDeduction.aspx");
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_WageDeduction.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");
                HyperLink lbldesc = (HyperLink)di.FindControl("lbldesc");
                
                if (chkSelect.Checked == true)
                {
                    objProp_PRDed.ConnConfig = Session["config"].ToString();
                    objProp_PRDed.ID = Convert.ToInt32(lblId.Text);
                    objBL_Wage.DeleteWageDeductionByID(objProp_PRDed);
                    GetWageDeductionList();
                    RadGrid_WageDeduction.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Wage Deduction " + lbldesc.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningServyp", "noty({text: 'Please select Wage Deduction to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelServyp", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }

    }
    
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkchk_Click(object sender, EventArgs e)
    {
        if (lnkChk.Checked)
        {
            check = true;
            GetWageDeductionList();
            RadGrid_WageDeduction.Rebind();
        }
        else
        {
            check = false;
            GetWageDeductionList();
            RadGrid_WageDeduction.Rebind();
        }
    }
    private void GetWageDeductionList()
    {
        try
        {
            DataSet ds = new DataSet();
            objProp_PRDed.ConnConfig = Session["config"].ToString();
            ds = new BL_Wage().GetWageDeduction(objProp_PRDed);
            RadGrid_WageDeduction.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_WageDeduction.DataSource = ds.Tables[0];
            ViewState["VirtualItemCount"] = ds.Tables[0].Rows.Count;
            lblRecordCount.Text = ds.Tables[0].Rows.Count + " Record(s) found";
            Session["WageDeductionList"] = ds.Tables[0];
            SaveFilter();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private DataTable GetFilteredDataSource()
    {
        DataTable DT = new DataTable();
        DataTable FilteredDT = new DataTable();
        string filterexpression = string.Empty;
        filterexpression = RadGrid_WageDeduction.MasterTableView.FilterExpression;
        if (filterexpression != "")
        {
            DT = (DataTable)RadGrid_WageDeduction.DataSource;
            FilteredDT = DT.AsEnumerable()
            .AsQueryable()
            .Where(filterexpression)
            .CopyToDataTable();
            return FilteredDT;
        }
        else
        {
            return (DataTable)RadGrid_WageDeduction.DataSource;
        }
    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_WageDeduction.MasterTableView.FilterExpression != "" ||
            (RadGrid_WageDeduction.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_WageDeduction.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_WageDeduction_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_WageDeduction.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Deduction_FilterExpression"] != null && Convert.ToString(Session["Deduction_FilterExpression"]) != "" && Session["Deduction_Filters"] != null)
                {
                    RadGrid_WageDeduction.MasterTableView.FilterExpression = Convert.ToString(Session["Deduction_FilterExpression"]);
                    var filtersGet = Session["Deduction_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_WageDeduction.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["Deduction_FilterExpression"] = null;
                Session["Deduction_Filters"] = null;
                //Session["Vendor_VirtulItemCount"] = null;
            }
            //if (Request.QueryString["AddVendor"] != null)
            //{
            //    if (Convert.ToString(Request.QueryString["AddVendor"]) == "Y")
            //    {
            //        if (check == true)
            //        {
            //            lnkChk.Checked = true;
            //        }
            //        else
            //        {
            //            lnkChk.Checked = false;
            //        }
            //    }
            //}
        }
        
        #endregion
        GetWageDeductionList();
    }
    protected void RadGrid_WageDeduction_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        //if (Session["vendors"] != null)
        //{
        //    DataTable dtID = (DataTable)Session["vendors"];
        //    lblRecordCount.Text = dtID.Rows.Count + " Record(s) found";
        //    updpnl.Update();
        //}
        rowCount = Convert.ToInt32(ViewState["VirtualItemCount"]);
        lblRecordCount.Text = rowCount + " Record(s) found";
        //updpnl.Update();
    }
    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_WageDeduction.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            HyperLink lnkName = (HyperLink)gr.FindControl("lbldesc");
            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='WageDeduction.aspx?id=" + lblID.Text + "'";
        }
    }
    protected void RadGrid_WageDeduction_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_WageDeduction.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Deduction_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_WageDeduction.MasterTableView.OwnerGrid.Columns)
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

            Session["Deduction_Filters"] = filters;
            //Session["Vendor_VirtulItemCount"] = RadGrid_WageDeduction.VirtualItemCount;
        }
        else
        {
            Session["Deduction_FilterExpression"] = null;
            Session["Deduction_Filters"] = null;
            //Session["Vendor_VirtulItemCount"] = null;
        }
        #endregion  
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_WageDeduction);
        RowSelect();

        
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        

        ddlType.Visible = false;
        ddlStatus.Visible = false;
        txtSearch.Visible = true;
        ResetFormControlValues(this);
        check = false;
        lnkChk.Checked = false;        
        
        foreach (GridColumn column in RadGrid_WageDeduction.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        Session["Deduction_FilterExpression"] = null;
        Session["Deduction_Filters"] = null;
        //Session["Vendor_VirtulItemCount"] = null;
        RadGrid_WageDeduction.MasterTableView.FilterExpression = "";
        //RadGrid_WageDeduction.MasterTableView.FilterExpression = string.Empty;
        //RadGrid_WageDeduction.PageSize = 50;
        ////lnkSearch_Click(sender, e);
        //Page_Load(sender, e);
        ////GetWageDeductionList();
        ////RadGrid_WageDeduction.Rebind();
        Response.Redirect("vendors.aspx?f=c");
        
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
                        //if (c.ClientID.Contains("PageSizeComboBox"))
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

    protected void RadGrid_WageDeduction_ItemCreated(object sender, GridItemEventArgs e)
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
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_WageDeduction.ExportSettings.FileName = "Vendor";
        RadGrid_WageDeduction.ExportSettings.IgnorePaging = true;
        RadGrid_WageDeduction.ExportSettings.ExportOnlyData = true;
        RadGrid_WageDeduction.ExportSettings.OpenInNewWindow = true;
        RadGrid_WageDeduction.ExportSettings.HideStructureColumns = true;
        RadGrid_WageDeduction.MasterTableView.UseAllDataFields = true;
        RadGrid_WageDeduction.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_WageDeduction.MasterTableView.ExportToExcel();
    }
    protected void RadGrid_WageDeduction_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_WageDeduction.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_WageDeduction.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_WageDeduction.SelectedItems)
        {
            Label lblDeductID = (Label)item.FindControl("lblId");
            Response.Redirect("WageDeduction.aspx?id=" + lblDeductID.Text + "&t=c");
        }        

    }
}

//public static class Test
//{
//    public static DataSet ToDataSet<T>(this IList<T> list)
//    {
//        Type elementType = typeof(T);
//        DataSet ds = new DataSet();
//        DataTable t = new DataTable();
//        ds.Tables.Add(t);

//        //add a column to table for each public property on T
//        foreach (var propInfo in elementType.GetProperties())
//        {
//            Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

//            t.Columns.Add(propInfo.Name, ColType);
//        }

//        //go through each property on T and add each value to the table
//        foreach (T item in list)
//        {
//            DataRow row = t.NewRow();

//            foreach (var propInfo in elementType.GetProperties())
//            {
//                row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
//            }

//            t.Rows.Add(row);
//        }

//        return ds;
//    }
//}