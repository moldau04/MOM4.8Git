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
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Net;

public partial class EmployeeList : System.Web.UI.Page
{
    #region "Variables"
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BL_Wage objBL_Wage = new BL_Wage();
    Emp objProp_Emp = new Emp();
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
            HighlightSideMenu("prID", "Employeelink", "payrollmenutab");
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

            hdnAddEmployee.Value = VendorsPermission.Length < 1 ? "Y" : VendorsPermission.Substring(0, 1);
            hdnEditEmployee.Value = VendorsPermission.Length < 2 ? "Y" : VendorsPermission.Substring(1, 1);
            hdnDeleteEmployee.Value = VendorsPermission.Length < 3 ? "Y" : VendorsPermission.Substring(2, 1);
            hdnViewDedcutions.Value = VendorsPermission.Length < 4 ? "Y" : VendorsPermission.Substring(3, 1);
            if (hdnAddEmployee.Value == "N")
            {

                lnkAddnew.Visible = false;
            }
            if (hdnEditEmployee.Value == "N")
            {
                btnEdit.Visible = false;
            }
            if (hdnDeleteEmployee.Value == "N")
            {
                lnkDelete.Visible = false;

            }
            if (hdnViewDedcutions.Value == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            //if (hdnAddEmployee.Value == "N")
            //{
            //    lnkAddnew.Enabled = false;
            //}
            //if (hdnEditEmployee.Value == "N")
            //{
            //    btnEdit.Enabled = false;
            //}
            //if (hdnDeleteEmloyee.Value == "N")
            //{
            //    lnkDelete.Enabled = false;
            //}
        }
        else
        {
            hdnAddEmployee.Value = "Y";
            hdnEditEmployee.Value = "Y";
            hdnDeleteEmployee.Value = "Y";
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
            //RadGrid_Employee.Columns[9].Visible = true;
        }
        else
        {
            //RadGrid_Employee.Columns[9].Visible = false;
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

        RadGrid_Employee.CurrentPageIndex = 0;
        RadGrid_Employee.PageSize = 50;
        GetWageEmployeeList();
        RadGrid_Employee.Rebind();
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        Session["InActiveEmp"] = "True";
        check = true;

        txtSearch.Text = string.Empty;
        ddlSearch.SelectedIndex = 0;
        ddlType.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        SelectSearch();
        foreach (GridColumn column in RadGrid_Employee.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_Employee.MasterTableView.SortExpressions.Clear();
        Session["Employee_FilterExpression"] = null;
        Session["Employee_Filters"] = null;
        RadGrid_Employee.MasterTableView.FilterExpression = "";
        RadGrid_Employee.CurrentPageIndex = 1;
        upPannelSearch.Update();
        GetWageEmployeeList();
        RadGrid_Employee.Rebind();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Employee.SelectedItems)
        {
            Label lblDeductID = (Label)item.FindControl("lblId");
            Label lblUserID = (Label)item.FindControl("lblUserID");
            Label lblTypeid = (Label)item.FindControl("lblTypeid");
            //Response.Redirect("AddEmp.aspx?id=" + lblDeductID.Text);
            Response.Redirect("AddEmp.aspx?uid=" + lblUserID.Text + "&type=" + lblTypeid.Text);
        }
    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddEmp.aspx");
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {


        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_Employee.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");
                if (chkSelect.Checked == true)
                {
                    objProp_Emp.ConnConfig = Session["config"].ToString();
                    objProp_Emp.ID = Convert.ToInt32(lblId.Text);
                    objBL_Wage.DeleteEmployeeByID(objProp_Emp);
                    GetWageEmployeeList();
                    RadGrid_Employee.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Employee " + lblId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningServyp", "noty({text: 'Please select Employee to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
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
            Session["InActiveEmp"] = "True";
            check = true;
            GetWageEmployeeList();
            RadGrid_Employee.Rebind();
        }
        else
        {
            Session["InActiveEmp"] = "False";
            check = true;
            GetWageEmployeeList();
            RadGrid_Employee.Rebind();
        }
    }
    private void GetWageEmployeeList()
    {
        try
        {

            DataSet ds = new DataSet();
            objProp_Emp.ConnConfig = Session["config"].ToString();

            ds = new BL_Wage().GetEmployeeList(objProp_Emp);



            DataTable filterdt = new DataTable();
            DataSet FilteredDs = new DataSet();
            check = Convert.ToBoolean(Session["InActiveEmp"]);
            if (check)
            {
                lnkChk.Checked = true;
                FilteredDs = ds.Copy();
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = ds.Tables[0].Select("Status=0");
                    if (dr.Length > 0)
                    {
                        filterdt = dr.CopyToDataTable();
                        FilteredDs.Tables.Add(filterdt);
                    }
                    else
                    {
                        FilteredDs = ds.Clone();
                    }
                }
                else
                {
                    FilteredDs = ds.Copy();

                }
            }
            RadGrid_Employee.VirtualItemCount = FilteredDs.Tables[0].Rows.Count;
            RadGrid_Employee.DataSource = FilteredDs.Tables[0];
            ViewState["VirtualItemCount"] = FilteredDs.Tables[0].Rows.Count;
            lblRecordCount.Text = FilteredDs.Tables[0].Rows.Count + " Record(s) found";
            Session["EmployeeList"] = FilteredDs.Tables[0];
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
        filterexpression = RadGrid_Employee.MasterTableView.FilterExpression;
        if (filterexpression != "")
        {
            DT = (DataTable)RadGrid_Employee.DataSource;
            FilteredDT = DT.AsEnumerable()
            .AsQueryable()
            .Where(filterexpression)
            .CopyToDataTable();
            return FilteredDT;
        }
        else
        {
            return (DataTable)RadGrid_Employee.DataSource;
        }

    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Employee.MasterTableView.FilterExpression != "" ||
            (RadGrid_Employee.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Employee.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Employee_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Employee.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Employee_FilterExpression"] != null && Convert.ToString(Session["Employee_FilterExpression"]) != "" && Session["Employee_Filters"] != null)
                {
                    RadGrid_Employee.MasterTableView.FilterExpression = Convert.ToString(Session["Employee_FilterExpression"]);
                    var filtersGet = Session["Employee_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_Employee.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["Employee_FilterExpression"] = null;
                Session["Employee_Filters"] = null;
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
        GetWageEmployeeList();
    }
    protected void RadGrid_Employee_ItemEvent(object sender, GridItemEventArgs e)
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
        foreach (GridDataItem gr in RadGrid_Employee.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            Label lblUserID = (Label)gr.FindControl("lblUserID");
            Label lblTypeid = (Label)gr.FindControl("lblTypeid");
            HyperLink lblLast = (HyperLink)gr.FindControl("lblLast");
            lblLast.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='AddEmp.aspx?uid=" + lblUserID.Text + "&type=" + lblTypeid.Text + "'";


        }
    }
    protected void RadGrid_Employee_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_Employee.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Employee_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Employee.MasterTableView.OwnerGrid.Columns)
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

            Session["Employee_Filters"] = filters;
            //Session["Vendor_VirtulItemCount"] = RadGrid_Employee.VirtualItemCount;
        }
        else
        {
            Session["Employee_FilterExpression"] = null;
            Session["Employee_Filters"] = null;
            //Session["Vendor_VirtulItemCount"] = null;
        }
        #endregion  
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Employee);
        RowSelect();


    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        Session["InActiveEmp"] = "False";
        check = true;

        ddlType.Visible = false;
        ddlStatus.Visible = false;
        txtSearch.Visible = true;
        ResetFormControlValues(this);
        check = false;
        lnkChk.Checked = false;

        foreach (GridColumn column in RadGrid_Employee.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        Session["Employee_FilterExpression"] = null;
        Session["Employee_Filters"] = null;
        //Session["Vendor_VirtulItemCount"] = null;
        RadGrid_Employee.MasterTableView.FilterExpression = "";
        //RadGrid_Employee.MasterTableView.FilterExpression = string.Empty;
        //RadGrid_Employee.PageSize = 50;
        ////lnkSearch_Click(sender, e);
        //Page_Load(sender, e);
        ////GetWageEmployeeList();
        ////RadGrid_Employee.Rebind();
        // Response.Redirect("vendors.aspx?f=c");

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
    public Dictionary<string, string> TelerikPageSizeEmployee(Int32 TotalCount)
    {
        var sizes = new Dictionary<string, string>() { { "15", "15" }, { "20", "20" }, { "30", "30" }, { "50", "50" }, { "100", "100" } };
        sizes.Add("All", Convert.ToString(TotalCount));
        return sizes;
    }
    protected void RadGrid_Employee_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (totalCount == 0) totalCount = 1000;
                GeneralFunctions obj = new GeneralFunctions();
                //var sizes = obj.TelerikPageSize(totalCount);
                var sizes = TelerikPageSizeEmployee(totalCount);
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
        RadGrid_Employee.ExportSettings.FileName = "Vendor";
        RadGrid_Employee.ExportSettings.IgnorePaging = true;
        RadGrid_Employee.ExportSettings.ExportOnlyData = true;
        RadGrid_Employee.ExportSettings.OpenInNewWindow = true;
        RadGrid_Employee.ExportSettings.HideStructureColumns = true;
        RadGrid_Employee.MasterTableView.UseAllDataFields = true;
        RadGrid_Employee.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Employee.MasterTableView.ExportToExcel();
    }
    protected void RadGrid_Employee_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Employee.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Employee.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
        foreach (GridDataItem item in RadGrid_Employee.SelectedItems)
        {
            Label lblDeductID = (Label)item.FindControl("lblId");
            Label lblUserID = (Label)item.FindControl("lblUserID");
            Label lblTypeid = (Label)item.FindControl("lblTypeid");
            //Response.Redirect("AddEmp.aspx?id=" + lblDeductID.Text + "&t=c");
            Response.Redirect("AddEmp.aspx?uid=" + lblUserID.Text + "&t=c" + "&type=" + lblTypeid.Text);


        }

    }
    protected void lnkAdjustYTD_Click(object sender, EventArgs e)
    {
        //IsDelete = false;
        //try
        //{
        //    foreach (GridDataItem di in RadGrid_Employee.SelectedItems)
        //    {
        //        IsDelete = true;
        //        TableCell cell = di["chkSelect"];
        //        CheckBox chkSelect = (CheckBox)cell.Controls[0];
        //        Label lblId = (Label)di.FindControl("lblId");
        //        if (chkSelect.Checked == true)
        //        {
        //            objProp_Emp.ConnConfig = Session["config"].ToString();
        //            objProp_Emp.ID = Convert.ToInt32(lblId.Text);
        //            objBL_Wage.DeleteEmployeeByID(objProp_Emp);
        //            GetWageEmployeeList();
        //            RadGrid_Employee.Rebind();
        //            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Employee " + lblId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        //        }
        //    }
        //    if (!IsDelete)
        //    {
        //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningServyp", "noty({text: 'Please select Employee to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelServyp", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        //}

    }
    public DataTable GetTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Employee", typeof(string));
        dt.Columns.Add("Address", typeof(string));
        dt.Columns.Add("City", typeof(string));
        dt.Columns.Add("State", typeof(string));
        dt.Columns.Add("Zip", typeof(string));
        return dt;
    }

    public static string CallWebService(string xmlDoc)
    {
        var _url = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";
        var _action = "http://xxxxxxxx/Service1.asmx?op=HelloWorld";

        XmlDocument soapEnvelopeXml = CreateSoapEnvelope(xmlDoc);
        HttpWebRequest webRequest = CreateWebRequest(_url, _action, "nmishra@986057068", "fkl8TM2E");
        InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

        // begin async call to web request.
        IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

        // suspend this thread until call is complete. You might want to
        // do something usefull here like update your UI.
        asyncResult.AsyncWaitHandle.WaitOne();

        // get the response from the completed web request.
        string soapResult;
        using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
        {
            using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
            {
                soapResult = rd.ReadToEnd();
            }
            //Console.Write(soapResult);
        }
        return soapResult;
    }

    private static HttpWebRequest CreateWebRequest(string url, string action, string username, string passWord)
    {
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        //webRequest.Headers.Add("SOAPAction", action);
        webRequest.Headers.Add("javax.xml.ws.security.auth.username", username);
        webRequest.Headers.Add("javax.xml.ws.security.auth.password", passWord);
        webRequest.ContentType = "text/xml;charset=\"utf-8\"";
        webRequest.Accept = "text/xml";
        webRequest.Method = "POST";
        return webRequest;
    }

    private static XmlDocument CreateSoapEnvelope(string xmlDoc)
    {
        XmlDocument soapEnvelopeDocument = new XmlDocument();
        soapEnvelopeDocument.LoadXml(xmlDoc);
        //soapEnvelopeDocument.LoadXml(
        //    @"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" 
        //           xmlns:xsi=""http://www.w3.org/1999/XMLSchema-instance"" 
        //           xmlns:xsd=""http://www.w3.org/1999/XMLSchema"">
        //    <SOAP-ENV:Body>
        //        <HelloWorld xmlns=""http://tempuri.org/"" 
        //            SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
        //            <int1 xsi:type=""xsd:integer"">12</int1>
        //            <int2 xsi:type=""xsd:integer"">32</int2>
        //        </HelloWorld>
        //    </SOAP-ENV:Body>
        //</SOAP-ENV:Envelope>");
        return soapEnvelopeDocument;
    }

    private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
    {
        using (Stream stream = webRequest.GetRequestStream())
        {
            soapEnvelopeXml.Save(stream);
        }
    }


    private string GetGeoCode(DataTable dt, int id, string _sName, string _sAddres, string _sCity, string _sState, string _sZip, out string exceptionmsg)
    {
        string geo = "";
        string strerrorMessage = "";
        string code = "";
        string username = ConfigurationManager.AppSettings["vertexApiUsername"].ToString(); // "dread@1000";
        string passWord = ConfigurationManager.AppSettings["vertexApiPassword"].ToString(); // "K3CHccxQ";
        string uri = ConfigurationManager.AppSettings["vertexAddressURL"].ToString();
        exceptionmsg = "";
        string addrClnXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic = \"http://EiCalc/\">"
            + "<soapenv:Header/>"
            + "<soapenv:Body>"
            + "<eic:AddrCleanse>"
            + "<Request>"
            + "<![CDATA["
                + "<ADDRESS_CLEANSE_REQUEST>"
                    + "<StreetAddress1>" + _sAddres + "</StreetAddress1>"
                    + "<CityName>" + _sCity + "</CityName>"
                    + "<StateName>" + _sState + "</StateName>"
                    + "<ZipCode>" + _sZip + "</ZipCode>"
                + "</ADDRESS_CLEANSE_REQUEST>]]>"
            + "</Request>"
            + "</eic:AddrCleanse>"
            + "</soapenv:Body>"
            + "</soapenv:Envelope>";

        try
        {

            string resulttt = CallWebService(addrClnXML);


            XmlDocument responseXML = new XmlDocument();
            responseXML.LoadXml(resulttt);
            XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());
            string responseXMLPrettystr = responseXMLPretty.ToString();
            responseXMLPrettystr = responseXMLPrettystr.Replace("\"", "'");
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(responseXMLPrettystr);

            XmlNode Errornode = Doc.GetElementsByTagName("Error").Item(0);
            if (Errornode == null)
            {
                XmlNode nodedoc = Doc.GetElementsByTagName("GeoCode").Item(0);
                if (nodedoc != null)
                {
                    geo = nodedoc.ChildNodes.Item(0).InnerText;
                }
                else
                {
                    geo = "Error in Address Geocode";
                }

            }
            else
            {
                geo = "Error in Address Geocode";

            }



            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //HttpClient cl = new HttpClient();
            //cl.BaseAddress = new Uri(uri);
            //cl.DefaultRequestHeaders.Clear();
            //cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.username", username);
            //cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.password", passWord);
            //HttpContent soapAddressEnvelope = new StringContent(addrClnXML);
            //soapAddressEnvelope.Headers.Clear();
            //soapAddressEnvelope.Headers.Add("Content-Type", "text/xml");
            //using (HttpResponseMessage response = cl.PostAsync(uri, soapAddressEnvelope).Result)
            //{
            //    string rawString = getMessage(response).Result;

            //    XmlDocument responseXML = new XmlDocument();
            //    responseXML.LoadXml(rawString);
            //    XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());
            //    string responseXMLPrettystr = responseXMLPretty.ToString();
            //    responseXMLPrettystr = responseXMLPrettystr.Replace("\"", "'");
            //    XmlDocument Doc = new XmlDocument();
            //    Doc.LoadXml(responseXMLPrettystr);

            //    XmlNode Errornode = Doc.GetElementsByTagName("Error").Item(0);
            //    if (Errornode == null)
            //    {
            //        XmlNode nodedoc = Doc.GetElementsByTagName("GeoCode").Item(0);
            //        if (nodedoc != null)
            //        {
            //            geo = nodedoc.ChildNodes.Item(0).InnerText;
            //        }
            //        else
            //        {
            //            geo = "Error in Address Geocode";
            //        }

            //    }
            //    else
            //    {
            //        geo = "Error in Address Geocode";

            //    }


            //    //geo = nodedoc.ChildNodes.Item(0).InnerText;
            //    soapAddressEnvelope.Dispose();
            //    cl.Dispose();
            //}
        }

        //catch (AggregateException aggEx)
        //{
        //    Console.WriteLine("A Connection error occurred");
        //    Console.WriteLine("----------------------------------------------------");
        //    Console.WriteLine("Error Code: " + aggEx.Message.ToString() + Environment.NewLine + "Message: " + aggEx.InnerException.Message.ToString());
        //}
        catch (Exception Ex)
        {
            exceptionmsg = "An Exception occurred: " + Ex.InnerException.Message.ToString();
        }

        async Task<string> getMessage(HttpResponseMessage messageFromServer)
        {
            //await Task.Delay(6000);
            code = await messageFromServer.Content.ReadAsStringAsync();
            return code;
        }
        if (geo.Trim() == "" || geo.Trim() == "Error in Address Geocode")
        {
            DataRow dr = dt.NewRow();
            dr["ID"] = id.ToString();
            dr["Employee"] = _sName.ToString();
            dr["Address"] = _sAddres.ToString();
            dr["City"] = _sCity.ToString();
            dr["State"] = _sState.ToString();
            dr["Zip"] = _sZip.ToString();
            dt.Rows.Add(dr);
        }


        return geo;
    }
    protected void lnkUpdateGeocode_Click(object sender, EventArgs e)
    {
        DataTable dt = GetTable();
        string _exceptionmsg = "";
        if (Session["EmployeeList"] != null)
        {
            DataTable empDt = (DataTable)Session["EmployeeList"];
            foreach (GridDataItem item in RadGrid_Employee.Items)
            //foreach(DataRow rw in empDt.Rows)
            {
                Label lblUserID = (Label)item.FindControl("lblUserID");
                Label lblTypeid = (Label)item.FindControl("lblTypeid");
                Label lblFirst = (Label)item.FindControl("lblFirst");
                HyperLink lblLast = (HyperLink)item.FindControl("lblLast");
                //Response.Redirect("AddEmp.aspx?uid=" + lblUserID.Text + "&t=c" + "&type=" + lblTypeid.Text);
                DataSet ds = new DataSet();
                objProp_Emp.ConnConfig = Session["config"].ToString();
                //objProp_Emp.ID = Convert.ToInt32(rw["UserID"]);
                //objProp_Emp.Type = Convert.ToInt32(rw["usertypeid"]);
                objProp_Emp.ID = Convert.ToInt32(lblUserID.Text);
                objProp_Emp.Type = Convert.ToInt32(lblTypeid.Text);
                objProp_Emp.DBName = Session["dbname"].ToString();
                ds = objBL_Wage.getEmpByIDforGeocode(objProp_Emp);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //string _sGeocode = GetGeoCode(dt, Convert.ToInt32(rw["UserID"]), Convert.ToString(rw["fFirst"]) + " " + Convert.ToString(rw["Last"]), ds.Tables[0].Rows[0]["Address"].ToString(), ds.Tables[0].Rows[0]["City"].ToString(), ds.Tables[0].Rows[0]["State"].ToString(), ds.Tables[0].Rows[0]["Zip"].ToString(), out _exceptionmsg);
                    string _sGeocode = GetGeoCode(dt, Convert.ToInt32(lblUserID.Text), Convert.ToString(lblFirst.Text) + " " + Convert.ToString(lblLast.Text), ds.Tables[0].Rows[0]["Address"].ToString(), ds.Tables[0].Rows[0]["City"].ToString(), ds.Tables[0].Rows[0]["State"].ToString(), ds.Tables[0].Rows[0]["Zip"].ToString(), out _exceptionmsg);
                    if (_exceptionmsg.Contains("An Exception occurred:") == true)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningGeocode", "noty({text: '" + _exceptionmsg + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        break;
                    }
                    if (_sGeocode.Trim() == "" || _sGeocode.Trim() == "Error in Address Geocode")
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningGeocode", "noty({text: 'Error in Address Geocode for "+ lblFirst .Text +" "+ lblLast .Text+ "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        //string script = "function f(){OpenErrorModal(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    }

                    else
                    {
                        objProp_Emp.ConnConfig = Session["config"].ToString();
                        objProp_Emp.ID = Convert.ToInt32(lblUserID.Text);
                        objProp_Emp.MOMUSer = Session["Username"].ToString();
                        objProp_Emp.Geocode = _sGeocode;
                        objBL_Wage.UpdateGeocode(objProp_Emp);
                    }
                }


            }
            if (_exceptionmsg == "")
            {
                if (dt.Rows.Count > 0)
                {
                    gv_Errorrows.DataSource = dt;
                    gv_Errorrows.DataBind();
                    //gv_Errorrows.Rebind();


                    lblInvalidRows.Text = "Total Employees:" + Convert.ToString(dt.Rows.Count);

                    string script = "function f(){OpenErrorModal(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningGeocode", "noty({text: 'Geocode updated successfully.', dismissQueue: true,  type : 'sucess', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningGeocode", "noty({text: '" + _exceptionmsg + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningGeocode", "noty({text: 'No Employee Found !', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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