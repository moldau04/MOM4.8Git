using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using BusinessEntity;
using BusinessLayer;
using Telerik.Web.UI;
using System.Linq.Dynamic;
using Telerik.Web.UI.GridExcelBuilder;
using MOMWebApp;
using BusinessEntity.Payroll;
using BusinessEntity.CommonModel;
using BusinessEntity.APModels;
using BusinessEntity.Utility;
using Stimulsoft.Report;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using System.IO;
using context = System.Web.HttpContext;
using ACHBAL;
using System.Web.Configuration;
using Renci.SshNet;
using System.Text;

public partial class PayrollRegister : System.Web.UI.Page
{
    #region "Variables"
    CD _objCD = new CD();
    BL_Bills _objBLBill = new BL_Bills();
    PJ _objPJ = new PJ();
    PRReg _objPRReg = new PRReg();
    BL_Bills _objBLBills = new BL_Bills();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BL_User _objBLUser = new BL_User();
    BL_Wage objBL_Wage = new BL_Wage();
    public bool check = false;
    Emp objProp_Emp = new Emp();
    Bank _objBank = new Bank();
    GetAllBankNamesParam _getAllBankNames = new GetAllBankNamesParam();
    APIIntegrationModel _objAPIIntegration = new APIIntegrationModel();
    BL_BankAccount _objBL_Bank = new BL_BankAccount();
    GetCheckDetailsByBankAndRefParam _getCheckDetailsByBankAndRef = new GetCheckDetailsByBankAndRefParam();
    protected DataTable dti = new DataTable();
    protected DataTable dtpay = new DataTable();
    protected DataTable dtBank = new DataTable();
    Vendor _objVendor = new Vendor();
    GetVendorRolDetailsParam _getVendorRolDetails = new GetVendorRolDetailsParam();
    BL_Vendor _objBLVendor = new BL_Vendor();
    BL_User objBL_User = new BL_User();
    GetControlBranchParam _getControlBranch = new GetControlBranchParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetBankCDParam _getBankCD = new GetBankCDParam();
    GetVendorAcctParam _getVendorAcct = new GetVendorAcctParam();
    #endregion
    #region Events

    #region PAGELOAD
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


                string path = Server.MapPath("StimulsoftReports/PayrollChecks/");
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.mrt");
                foreach (FileInfo file in Files)
                {

                    string FileName = string.Empty;
                    if (file.Name.Contains(".mrt"))
                        FileName = file.Name.Replace(".mrt", " ");
                    ddlApTopCheckForLoad.Items.Add((FileName.Trim()));
                }

                ddlApTopCheckForLoad.DataBind();
                ddlApTopCheckForLoad.SelectedIndex = ddlApTopCheckForLoad.Items.IndexOf(ddlApTopCheckForLoad.Items.FindByValue("PRTopCheck-deluxe 081064"));

                // If you want to find text by value field.
                //var t = ddlApTopCheckForLoad.Items.FindByValue("Pr-deluxe 081064 ");



                if (ddlBank.SelectedValue == "")
                {
                    ddlBank.SelectedIndex = 1;
                }

                if (txtcheckfrom.Text == null || txtcheckfrom.Text == "")
                {
                    txtcheckfrom.Text = "1";
                }

                if (txtcheckto.Text == null || txtcheckto.Text == "")
                {
                    txtcheckto.Text = "1";
                }

                BindEmployee();
                BindTabTitle();
                Permission();
                UserPermission();
                FillBank();
                //for retaining filters
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {


                    if (Session["PayCheckfromDate"] != null && Session["PayCheckToDate"] != null)
                    {
                        txtFromDate.Text = Session["PayCheckfromDate"].ToString();
                        txtToDate.Text = Session["PayCheckToDate"].ToString();
                    }
                    else
                    {
                        txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                        txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                        Session["PayCheckfromDate"] = txtFromDate.Text;
                        Session["PayCheckToDate"] = txtToDate.Text;
                    }

                    if (Session["PayCheckInClosed"] != null)
                    {
                        if (Session["PayCheckInClosed"].ToString() == "True")
                        {
                            check = true;
                            lnkChk.Checked = true;
                        }
                        else
                        {
                            check = false;
                            lnkChk.Checked = false;
                        }
                    }
                    else
                    {
                        check = false;
                        lnkChk.Checked = false;
                    }
                }
                else
                {
                    Session["ddlsearch"] = null;
                    Session["txtSearch"] = null;
                    Session["ddlStatus"] = null;
                    Session["ddlBOMType"] = null;
                    Session["txtVendorSearch"] = null;
                    Session["PayCheckfromDate"] = null;
                    Session["PayCheckToDate"] = null;
                    Session["PayCheck_Type"] = null;
                    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                    Session["PayCheckfromDate"] = txtFromDate.Text;
                    Session["PayCheckToDate"] = txtToDate.Text;
                    hdnBillsSelectDtRange.Value = "Week";
                    Session["PayCheckInClosed"] = "False";
                    check = false;
                    lnkChk.Checked = false;
                }
            }

            //CompanyPermission();
            HighlightSideMenu("prID", "payrollregisterlink", "payrollmenutab");

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
    }
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_PayRegister.Columns[8].Visible = true;
        }
        else
        {
            RadGrid_PayRegister.Columns[8].Visible = false;
        }
    }
    #endregion
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addbills.aspx");
    }
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem di in RadGrid_PayRegister.SelectedItems)
            {
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                HiddenField hdnID = (HiddenField)di.FindControl("hdnID");

                if (chkSelect.Checked == true)
                {


                    Response.Redirect("addbills.aspx?id=" + hdnID.Value);

                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
        Session.Remove("Bills");

    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_PayRegister.MasterTableView.FilterExpression != "" ||
            (RadGrid_PayRegister.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_PayRegister.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_PayRegister_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_PayRegister.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["PayCheck_FilterExpression"] != null && Convert.ToString(Session["PayCheck_FilterExpression"]) != "" && Session["PayCheck_Filters"] != null)
                {
                    RadGrid_PayRegister.MasterTableView.FilterExpression = Convert.ToString(Session["PayCheck_FilterExpression"]);
                    var filtersGet = Session["PayCheck_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_PayRegister.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["PayCheck_FilterExpression"] = null;
                Session["PayCheck_Filters"] = null;
            }
        }
        #endregion
        //BindCheckGrid();


        Session["PayCheck_Type"] = "1";
        BindCheckGrid();





    }
    protected void RadGrid_PayRegister_ItemEvent(object sender, GridItemEventArgs e)
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
        foreach (GridDataItem gr in RadGrid_PayRegister.Items)
        {
            //Label lblID = (Label)gr.FindControl("lblIndex");
            HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

            HyperLink lnkName = (HyperLink)gr.FindControl("lblRef");
            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='EmpCheckDetail.aspx?id=" + hdnID.Value + "&frm=Register'";
            //lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addbills.aspx?id=" + lblID.Text + "&frm=MNG2'";

        }
    }
    protected void RadGrid_PayRegister_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_PayRegister.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["PayCheck_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_PayRegister.MasterTableView.OwnerGrid.Columns)
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

            Session["PayCheck_Filters"] = filters;
        }
        else
        {
            Session["PayCheck_FilterExpression"] = null;
            Session["PayCheck_Filters"] = null;
        }
        #endregion
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_PayRegister);
        RowSelect();
    }
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_PayRegister.MasterTableView.GetColumn("chkSelect").Visible = false;
        RadGrid_PayRegister.ExportSettings.FileName = "Bills";
        RadGrid_PayRegister.ExportSettings.IgnorePaging = true;
        RadGrid_PayRegister.ExportSettings.ExportOnlyData = true;
        RadGrid_PayRegister.ExportSettings.OpenInNewWindow = true;
        RadGrid_PayRegister.ExportSettings.HideStructureColumns = true;
        RadGrid_PayRegister.MasterTableView.UseAllDataFields = true;
        RadGrid_PayRegister.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_PayRegister.MasterTableView.ExportToExcel();
    }
    protected void RadGrid_PayRegister_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 5;
        else
            currentItem = 6;
        if (e.Worksheet.Table.Rows.Count == RadGrid_PayRegister.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_PayRegister.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
    protected void RadGrid_PayRegister_ItemCreated(object sender, GridItemEventArgs e)
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
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        bool _isClosed = false;
        bool Flag = false;
        foreach (GridDataItem di in RadGrid_PayRegister.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            if (chkSelect.Checked.Equals(true))
            {
                HiddenField hdnSel = (HiddenField)di.FindControl("hdnSel");
                HiddenField hdnID = (HiddenField)di.FindControl("hdnID");
                Label lblfDate = (Label)di.FindControl("lblfDate");


                Flag = CommonHelper.GetPeriodDetails(Convert.ToDateTime(lblfDate.Text));

                if (Flag)
                {

                    if (Convert.ToInt16(hdnSel.Value).Equals(0))
                    {
                        _objPRReg.ConnConfig = Session["config"].ToString();
                        _objPRReg.ID = Convert.ToInt32(hdnID.Value);
                        _objPRReg.MOMUSer = Convert.ToString(Session["Username"].ToString());

                        objBL_Wage.VoidPayCheck(_objPRReg);

                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Check voided successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        _isClosed = true;
                    }

                }

            }
        }
        if (!Flag)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "DatePermissionAlert('void');", true);
        }
        else
        {
            if (_isClosed.Equals(true))
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "closedMesg();", true);
            }
            else
            {

                Session["PayCheck_Type"] = "1";
                BindCheckGrid();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "DeleteBillMsg();", true);



                RadGrid_PayRegister.Rebind();
            }
        }
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {

        if (hdnCssActive.Value == "CssActive")
        {
            Session["PayCheckActive"] = "1";
        }
        else
        {
            Session["PayCheckActive"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }
        Session["PayCheckToDate"] = txtToDate.Text;
        Session["PayCheckfromDate"] = txtFromDate.Text;
        //BindCheckGrid();
        RadGrid_PayRegister.Rebind();


    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {

        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        Session["PayCheckfromDate"] = txtFromDate.Text;
        Session["PayCheckToDate"] = txtToDate.Text;


        Session["PayCheckInClosed"] = "True";
        lnkChk.Checked = true;

        Session["PayCheck_Type"] = "1";
        BindCheckGrid();


        RadGrid_PayRegister.Rebind();
    }
    #endregion
    private void BindCheckGridShowAll()
    {

        //try
        //{
        //    DataSet _dsPJ = new DataSet();
        //    _objPJ.ConnConfig = Session["config"].ToString();
        //    _objPJ.UserID = Convert.ToInt32(Session["UserID"].ToString());


        //    string stdate = "1900-01-01 00:00:00";
        //    string enddate = DateTime.Now.ToShortDateString() + " 23:59:59";
        //    _objPJ.StartDate = Convert.ToDateTime(stdate);
        //    _objPJ.EndDate = Convert.ToDateTime(enddate);



        //    if (ddlSearch.SelectedValue == "PO")
        //    {
        //        _objPJ.PO = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

        //    }
        //    if (ddlSearch.SelectedValue == "Projectnumber")
        //    {
        //        _objPJ.ProjectNumber = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);

        //    }

        //    if (ddlSearch.SelectedValue == "Vendor")
        //    {
        //        _objPJ.Vendor = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);


        //    }
        //    if (ddlSearch.SelectedValue == "Amount")
        //    {
        //        _objPJ.Amount = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);


        //    }
        //    if (ddlSearch.SelectedValue == "AmountDue")
        //    {
        //        _objPJ.Balance = string.IsNullOrEmpty(txtSearch.Text) ? 0 : Convert.ToInt16(txtSearch.Text);


        //    }
        //    if (ddlSearch.SelectedValue == "VendorName")
        //    {
        //        _objPJ.vendorName = txtVendorSearch.Text;

        //    }
        //    if (ddlSearch.SelectedValue == "Custom1")
        //    {
        //        _objPJ.Custom1 = txtVendorSearch.Text;

        //    }
        //    if (ddlSearch.SelectedValue == "Custom2")
        //    {
        //        _objPJ.Custom2 = txtVendorSearch.Text;

        //    }
        //    if (ddlSearch.SelectedValue == "Status")
        //    {
        //        _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue); //Convert.ToInt16(txtSearch.Text);

        //        Session["ddlStatus"] = ddlStatus.SelectedValue;
        //    }
        //    else
        //    {
        //        _objPJ.Status = 99;

        //    }
        //    if (ddlSearch.SelectedValue == "Code")
        //    {
        //        _objPJ.Terms = Convert.ToInt16(ddlBOMType.SelectedValue); //Convert.ToInt16(txtSearch.Text);

        //        Session["ddlBOMType"] = ddlBOMType.SelectedValue;
        //    }
        //    else
        //    {
        //        _objPJ.Terms = 99;

        //    }
        //    _objPJ.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);


        //    if (_objPJ.SearchValue.Equals(2))
        //    {
        //        if (string.IsNullOrEmpty(txtToDate.Text))
        //        {
        //            _objPJ.SearchDate = DateTime.Now;

        //            txtToDate.Text = DateTime.Now.ToShortDateString();
        //        }
        //        else
        //        {
        //            _objPJ.SearchDate = Convert.ToDateTime(txtToDate.Text);
        //        }
        //    }




        //    if (Session["CmpChkDefault"].ToString() == "1")
        //    {
        //        _objPJ.EN = 1;

        //    }
        //    else
        //    {
        //        _objPJ.EN = 0;

        //    }
        //    _objPJ.Ref = txtRef.Text.Trim();


        //    _dsPJ = _objBLBills.GetAllPJDetails(_objPJ);


        //    // Status open/closed filter 
        //    DataTable filterdt = new DataTable();
        //    DataSet FilteredDs = new DataSet();
        //    check = Convert.ToBoolean(Session["PayCheckInClosed"]);
        //    if (check)
        //    {
        //        lnkChk.Checked = true;
        //        FilteredDs = _dsPJ.Copy();
        //    }
        //    else
        //    {
        //        if (_dsPJ.Tables[0].Rows.Count > 0)
        //        {
        //            //DataRow[] dr = _dsPJ.Tables[0].Select("StatusName='Open' OR StatusName='Partial'");
        //            //if (dr.Length > 0)
        //            //{
        //            //    filterdt = dr.CopyToDataTable();
        //            //    FilteredDs.Tables.Add(filterdt);
        //            //}
        //            //else
        //            //{
        //            FilteredDs = _dsPJ.Copy();
        //            //}
        //        }
        //        else
        //        {
        //            FilteredDs = _dsPJ.Copy();

        //        }
        //    }
        //    RadGrid_PayRegister.VirtualItemCount = FilteredDs.Tables[0].Rows.Count;
        //    RadGrid_PayRegister.DataSource = FilteredDs;


        //    BusinessEntity.User objProp_User = new BusinessEntity.User();
        //    DataSet ds = new DataSet();
        //    objProp_User.ConnConfig = Session["config"].ToString();


        //        ds = _objBLUser.getControl(objProp_User);


        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsUseTaxAPBills"].ToString()) == false)
        //        {
        //            RadGrid_PayRegister.Columns[12].Visible = false;
        //        }
        //        else
        //        {
        //            RadGrid_PayRegister.Columns[12].Visible = true;
        //        }


        //    }




        //    string filterexpression = string.Empty;
        //    filterexpression = RadGrid_PayRegister.MasterTableView.FilterExpression;
        //    if (filterexpression == "" || filterexpression == null)
        //    {
        //        Session["Bills"] = FilteredDs.Tables[0];

        //    }
        //    else
        //    {
        //        if (FilteredDs.Tables[0].Rows.Count > 0)
        //        {
        //            //Session["Bills"] = FilteredDs.Tables[0].AsEnumerable().AsQueryable().Where(filterexpression).CopyToDataTable();
        //            Session["Bills"] = FilteredDs.Tables[0];
        //        }
        //        else
        //        {
        //            Session["Bills"] = FilteredDs.Tables[0];
        //        }

        //    }
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
    }
    #region Custom Functions
    private void BindCheckGrid()
    {

        try
        {

            txtFromDate.Text = Session["PayCheckfromDate"].ToString();
            txtToDate.Text = Session["PayCheckToDate"].ToString();
            if (txtToDate.Text == "")
            {
                txtFromDate.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
                txtToDate.Text = DateTime.Now.AddYears(100).ToShortDateString();
            }

            DataSet _dsPRReg = new DataSet();
            _objPRReg.ConnConfig = Session["config"].ToString();

            if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
            {
                txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            string stdate = txtFromDate.Text + " 00:00:00";
            string enddate = txtToDate.Text + " 23:59:59";
            _objPRReg.StartDate = Convert.ToDateTime(stdate);
            _objPRReg.EndDate = Convert.ToDateTime(enddate);
            _objPRReg.EmpID = Convert.ToInt32(ddlEmp.SelectedValue);

            _objPRReg.PageSize = 0;
            _objPRReg.SortBy = "";
            _objPRReg.SortType = "";

            _dsPRReg = objBL_Wage.GetPayrollRegister(_objPRReg);
            Session["PayRegister"] = _dsPRReg.Tables[0];
            RadGrid_PayRegister.VirtualItemCount = _dsPRReg.Tables[0].Rows.Count;
            RadGrid_PayRegister.DataSource = _dsPRReg;


            txtFromDate.Text = Session["PayCheckfromDate"].ToString();
            txtToDate.Text = Session["PayCheckToDate"].ToString();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion   
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();


        DataSet ds = new DataSet();


        ds = _objBLUser.GetUserPermissionByUserID(objPropUser);


        return ds.Tables[0];
    }
    private void UserPermission()
    {
        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// AccountPayablemodulePermission ///////////////////------->

            string AccountPayablemodulePermission = ds.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["AccountPayablemodulePermission"].ToString();

            if (AccountPayablemodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// Bill ///////////////////------->

            string BillPayPermission = ds.Rows[0]["Bill"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Bill"].ToString();
            string ADD = BillPayPermission.Length < 1 ? "Y" : BillPayPermission.Substring(0, 1);
            string Edit = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(1, 1);
            string Delete = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(2, 1);
            string View = BillPayPermission.Length < 4 ? "Y" : BillPayPermission.Substring(3, 1);
            if (ADD == "N")
            {

                lnkAddnew.Visible = false;
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
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
        }
    }
    private void BindEmployee()
    {
        try
        {

            DataSet ds = new DataSet();
            objProp_Emp.ConnConfig = Session["config"].ToString();

            ds = new BL_Wage().GetEmployeeList(objProp_Emp);
            if (ddlEmp.Items.Count > 0)
            {
                ddlEmp.Items.Clear();
            }
            ddlEmp.DataSource = ds.Tables[0];
            ddlEmp.DataTextField = "Name";
            ddlEmp.DataValueField = "ID";
            ddlEmp.DataBind();
            ddlEmp.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("All", "0")));

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    private void BindTabTitle()
    {
        try
        {

            DataSet ds = new DataSet();
            objProp_Emp.ConnConfig = Session["config"].ToString();

            ds = new BL_Wage().GetEmployeeTitle(objProp_Emp);
            rptDepartmentTab.DataSource = ds;
            rptDepartmentTab.DataBind();
            //tabTitle.DataTextField = "tablabel";
            //tabTitle.DataNavigateUrlField = "";
            //tabTitle.DataFieldID = "ID";
            //tabTitle.DataFieldParentID = "ParentID";

            //tabTitle.DataSource = ds;
            //tabTitle.DataBind();



        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        if (lnkChk.Checked)
        {
            Session["PayCheckInClosed"] = "True";
            check = true;

            Session["PayCheck_Type"] = "1";
            BindCheckGrid();

            RadGrid_PayRegister.Rebind();
            if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
            {
                Session["PayCheckToDate"] = txtToDate.Text;
                Session["PayCheckfromDate"] = txtFromDate.Text;
            }

        }
        else
        {
            Session["PayCheckInClosed"] = "False";
            check = false;

            Session["PayCheck_Type"] = "1";
            BindCheckGrid();

            RadGrid_PayRegister.Rebind();
            if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
            {
                Session["PayCheckToDate"] = txtToDate.Text;
                Session["PayCheckfromDate"] = txtFromDate.Text;
            }
        }
    }
    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_PayRegister.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblIndex = (Label)di.FindControl("lblIndex");

            if (chkSelect.Checked.Equals(true))
            {

                ///////// Check Account Active/Inactive in bill ///////
                _objPJ.ConnConfig = Session["config"].ToString();
                _objPJ.ID = Convert.ToInt32(lblIndex.Text);



                DataSet _dsPJ = new DataSet();


                _dsPJ = _objBLBills.GetPJAcctDetailByID(_objPJ);

                if (_dsPJ.Tables[0].Rows.Count > 0)
                {
                    DataRow _drPJ = _dsPJ.Tables[0].Rows[0];
                    string acctname = Convert.ToString(_drPJ["AcctName"]);
                    //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'This account is inactive. Please change the account name before proceeding.',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);                        
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "display warn msg", "notyConfirm('" + lblIndex.Text + "');", true);

                }
                else
                {
                    Response.Redirect("addbills.aspx?id=" + lblIndex.Text + "&t=c");
                }
                ///////// Check Account Active/Inactive in bill ///////
                //Response.Redirect("addbills.aspx?id=" + lblIndex.Text + "&t=c");
            }


        }
    }
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);
        txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
        txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
        Session["PayCheckfromDate"] = txtFromDate.Text;
        Session["PayCheckToDate"] = txtToDate.Text;


        Session["PayCheck_Type"] = "1";
        BindCheckGrid();

        foreach (GridColumn column in RadGrid_PayRegister.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_PayRegister.MasterTableView.FilterExpression = string.Empty;
        RadGrid_PayRegister.Rebind();


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


        check = false;
        Session["PayCheckInClosed"] = "False";
        lnkChk.Checked = false;


    }

    private void FillBank()
    {
        try
        {
            if (Session["COPer"].ToString() == "1")
            {
                //do nothing
            }
            else
            {
                _objBank.ConnConfig = Session["config"].ToString();
                _getAllBankNames.ConnConfig = Session["config"].ToString();

                DataSet _dsBank = new DataSet();

                List<GetAllBankNamesViewModel> _lstGetAllBankNamesViewModel = new List<GetAllBankNamesViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetAllBankNames";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllBankNames);

                    _lstGetAllBankNamesViewModel = (new JavaScriptSerializer()).Deserialize<List<GetAllBankNamesViewModel>>(_APIResponse.ResponseData);
                    _dsBank = CommonMethods.ToDataSet<GetAllBankNamesViewModel>(_lstGetAllBankNamesViewModel);
                }
                else
                {
                    _dsBank = _objBL_Bank.GetAllBankNames(_objBank);
                }
                if (_dsBank.Tables[0].Rows.Count > 0)
                {
                    ddlBank.Items.Clear();
                    ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(":: Select ::", "0"));
                    ddlBank.AppendDataBoundItems = true;

                    ddlBank.DataSource = _dsBank;
                    ddlBank.DataValueField = "ID";
                    ddlBank.DataTextField = "fDesc";
                    ddlBank.DataBind();
                }
                else
                {
                    ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void imgPrintTemp1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (ddlApTopCheckTransType.SelectedValue == "0")
            {
                if (ddlApTopCheckForLoad.SelectedItem.Text.Trim() != null)
                {
                    string reportApTopCheckPathStimul = Server.MapPath("StimulsoftReports/PayrollChecks/" + ddlApTopCheckForLoad.SelectedItem.Text.Trim() + ".mrt");
                    StiReport report = new StiReport();
                    FillReportApTopCheckDataSet(ddlApTopCheckForLoad.SelectedItem.Text.Trim());
                }
            }
            else if (ddlApTopCheckTransType.SelectedValue == "2")
            {
                string alert = string.Empty;
                bool Sent = false;
                string ACHfileResponseText = string.Empty;
                string ACHControleResponseText = string.Empty;
                string ACHfileName = string.Empty;
                string ACHControlefileName = string.Empty;
                string Time = DateTime.Now.ToString("yyyyMMdd.hhmmss");
                string FileCreationTime = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                string FileCreationDate = DateTime.Now.ToString("yy/MM/dd");
                string TempACHfileName = string.Empty;
                try
                {
                    /*Create file from user's bank detail and save it locally to path given in web.config*/
                    ACHfileName = CreateAchFile(Time, FileCreationTime, FileCreationDate, out TempACHfileName);
                    Sent = true;
                    /*Update transaction status in MOM*/
                    //MakePayment(TempACHfileName, Sent, ACHfileResponseText);
                    if (Sent)
                    {
                        alert = "ACH File has been sent successfully to PNC Server. <BR/>Your payment will be processed soon.";
                        Session["uidv"] = null;
                        string sGenName = "DDPAYROLMIDWEST.txt";
                        System.IO.FileStream fs = null;
                        fs = System.IO.File.Open(Server.MapPath("~/ACHFile/" +
                                 TempACHfileName + ""), System.IO.FileMode.Open);
                        byte[] btFile = new byte[fs.Length];
                        fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                        fs.Close();
                        Response.AddHeader("Content-disposition", "attachment; filename=" + sGenName);
                        Response.ContentType = "application/octet-stream";
                        Response.BinaryWrite(btFile);
                        //RadAjaxManager_WageDeduction.ResponseScripts.Add("ClosetemplateModal();");
                        Response.End();
                    }
                    else
                    {
                        alert = "ACH Payment Failed. <BR/> ACH File Response :-" + ACHfileResponseText;
                        alert += "<BR/> ACH Controle Response  :-" + ACHControleResponseText;
                        Session["uidv"] = null;
                    }
                }
                catch (Exception ex)
                {
                    //lblErr.Text = ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace;
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    /// <summary>
    /// Create Ach File
    /// </summary>
    /// <param name="Time"></param>
    /// <param name="FileCreationTime"></param>
    /// <param name="FileCreationDate"></param>
    /// <returns></returns>
    private string CreateAchFile(string Time, string FileCreationTime, string FileCreationDate, out string TempACHfileName)
    {
        string FileName = string.Empty;
        TempACHfileName = string.Empty;
        //string path = String.Format(WebConfigurationManager.AppSettings["ACH_File_Path"].Trim());
        string path = context.Current.Server.MapPath("~/ACHFile/");
         if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        // specify your path
        if (!string.IsNullOrEmpty(path))
        {
            FileName = path + "DDPAYROLLMIDWEST" + Time;
            TempACHfileName = "DDPAYROLLMIDWEST" + Time;
            filecontrolvariables.ResetValues();
            Append.ResetValues();
            _objBank.ConnConfig = Session["config"].ToString();
            _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue.ToString());
            DataSet _dtBankDetail = new DataSet();
            _dtBankDetail = _objBL_Bank.spGetBankACH(_objBank);
            DataSet _checkDS = new DataSet();
            _objBank.CheckNoFrom =Convert.ToInt32(txtcheckfrom.Text);
            _objBank.CheckNoTo = Convert.ToInt32(txtcheckto.Text);

            _checkDS = _objBL_Bank.spGetACHForCheck(_objBank);
            object sumObject;
            sumObject = _checkDS.Tables[0].Compute("Sum(Net)", string.Empty);
            int rownumber = 1;
            string m_str = "07100028";
            //---------------
            CreateFileHeader(FileName.Trim(), FileCreationTime, FileCreationDate, _dtBankDetail);
            //---------------
            foreach (DataRow _chRow in _checkDS.Tables[0].Rows)
            {
                AddEmpDetails(_chRow["ACHBank"].ToString(), _chRow["EmpName"].ToString(), _chRow["ACHBank"].ToString(), _chRow["Net"].ToString().ToString(), m_str + (rownumber.ToString().PadLeft(7, (char)48)), _dtBankDetail, _checkDS);
                rownumber++;
            }
            //---------------
            AddEntryDetails(_dtBankDetail.Tables[0].Rows[0]["nAcct"].ToString(), "MIDWEST ELEVATOR", "", sumObject.ToString(), _dtBankDetail, _checkDS);
            //---------------
            CreateBatchHeader(_dtBankDetail);
            //----------------
            CreateFileControle();

        }
        return FileName;
    }

    public void CreateFileHeader(string FileName, string FileCreationTime, string FileCreationDate, DataSet _dtBankDetail)
    {
        ACHFileheader objBL = new ACHFileheader();
        string ImmediateDestination = _dtBankDetail.Tables[0].Rows[0]["ACHFileHeaderStringB"].ToString();
        string ImmediateOrigin = "";
        string PriorityCode = "";
        string FormatCode = "";
        string FileIdModifier = _dtBankDetail.Tables[0].Rows[0]["ACHFileHeaderStringC"].ToString();
        string ReferenceTypeCode = "";
        string RecordSize = _dtBankDetail.Tables[0].Rows[0]["fDesc"].ToString();
        string BlockingFactor = "";
        //string FileCreationTime="2246";
        string ImmediateDestinationName = "";
        //string FileCreationDate="040520";
        string ImmediateOriginName = "MIDWEST ELEVATOR CO INC";// _dtBankDetail.Tables[0].Rows[0]["CompanyName"].ToString();
        string ReferenceCode = "";
        Append.FileName = FileName;
        if (ImmediateDestination != string.Empty &&
        FileIdModifier != string.Empty && RecordSize != string.Empty)
        {
            objBL.ReferenceTypeCode = ReferenceTypeCode;
            objBL.PriorityCode = PriorityCode;
            objBL.ImmediateDestination = ImmediateDestination;
            objBL.ImmediateOrigin = ImmediateOrigin;
            objBL.FileCreationDate = Convert.ToString(FileCreationDate);//.ToString("yy/MM/dd"));
            objBL.FileCreationTime = FileCreationTime.Replace(":", "");
            objBL.FileIdModifier = FileIdModifier;
            objBL.RecordSize = RecordSize;
            objBL.BlockingFactor = BlockingFactor;
            objBL.FormatCode = FormatCode;
            objBL.ImmediateDestinationName = ImmediateDestinationName;
            objBL.ImmediateOriginName = ImmediateOriginName;
            objBL.ReferenceCode = ReferenceCode;
            objBL.SaveFileHeaderMidwest(Append.FileName);
        }

    }
    public void AddEmpDetails(string BankRouting_OR_DFIAccountNumber, string AccountHolderName_OR_RecievingCompanyName, string BankAccount_OR_IdentificationNumber, string Amount, string TraceNumber, DataSet _dtBankDetail, DataSet _checkDS)
    {
        string RecordTypeCode = "6";
        string TransactioCode = "22";
        string RecievingDFIIdentification = _checkDS.Tables[0].Rows[0]["ACHRoute"].ToString();
        string CheckDigit = "6";
        string DiscretionaryData = "";
        string AddendaRecordIndicator = "0";
        //string TraceNumber = "071000280000001";
        EntryDetail objEntry = new EntryDetail();
        bool m_flag = false;
        //if (objEntry.BankRoutingNumberValidation(RecievingDFIIdentification) && TransactioCode != "Select" && RecievingDFIIdentification != string.Empty &&
        //    BankRouting_OR_DFIAccountNumber != string.Empty && Amount != string.Empty && AccountHolderName_OR_RecievingCompanyName != string.Empty && AddendaRecordIndicator != string.Empty)
        if (TransactioCode != "Select" && RecievingDFIIdentification != string.Empty &&
           BankRouting_OR_DFIAccountNumber != string.Empty && Amount != string.Empty && AccountHolderName_OR_RecievingCompanyName != string.Empty && AddendaRecordIndicator != string.Empty)
        {

            objEntry.RecordTypeCode = RecordTypeCode;
            objEntry.TransactioCode = TransactioCode.ToString().Substring(0, 2);
            objEntry.RecievingDFIIdentification = RecievingDFIIdentification;
            objEntry.DFIAccountNumber = BankRouting_OR_DFIAccountNumber;
            objEntry.Amount = Amount.Replace("$", "").Replace(".", "");
            objEntry.IdentificationNumber = BankAccount_OR_IdentificationNumber;
            objEntry.RecievingCompanyName = AccountHolderName_OR_RecievingCompanyName;
            objEntry.DiscretionaryData = DiscretionaryData;
            objEntry.AddendaRecordIndicator = AddendaRecordIndicator;
            objEntry.TraceNumber = TraceNumber;
            objEntry.saveEntryMidwest(Append.FileName);
        }

    }
    public void AddEntryDetails(string BankRouting_OR_DFIAccountNumber, string AccountHolderName_OR_RecievingCompanyName, string BankAccount_OR_IdentificationNumber, string Amount, DataSet _dtBankDetail, DataSet _checkDS)
    {
        string RecordTypeCode = "6";
        string TransactioCode = "27";
        string RecievingDFIIdentification = _dtBankDetail.Tables[0].Rows[0]["NRoute"].ToString();
        string CheckDigit = "6";
        string DiscretionaryData = "";
        string AddendaRecordIndicator = "0";
        string TraceNumber = "071000280000001";
        EntryDetail objEntry = new EntryDetail();
        bool m_flag = false;
        if (objEntry.BankRoutingNumberValidation(RecievingDFIIdentification) && TransactioCode != "Select" && RecievingDFIIdentification != string.Empty &&
            BankRouting_OR_DFIAccountNumber != string.Empty && Amount != string.Empty && AccountHolderName_OR_RecievingCompanyName != string.Empty && AddendaRecordIndicator != string.Empty)
        {

            objEntry.RecordTypeCode = RecordTypeCode;
            objEntry.TransactioCode = TransactioCode.ToString().Substring(0, 2);
            objEntry.RecievingDFIIdentification = RecievingDFIIdentification;
            objEntry.DFIAccountNumber = BankRouting_OR_DFIAccountNumber;
            objEntry.Amount = Amount.Replace("$", "").Replace(".", "");
            objEntry.IdentificationNumber = BankAccount_OR_IdentificationNumber;
            objEntry.RecievingCompanyName = AccountHolderName_OR_RecievingCompanyName;
            objEntry.DiscretionaryData = DiscretionaryData;
            objEntry.AddendaRecordIndicator = AddendaRecordIndicator;
            objEntry.TraceNumber = TraceNumber;
            objEntry.saveEntryMidwest(Append.FileName);
        }

    }
    public void CreateBatchHeader(DataSet _dtBankDetail)
    {
        bool m_flag = false;
        ACHBatchHeader objBatchHeader = new ACHBatchHeader();
        EntryDetail objEntry = new EntryDetail();
        int cmbServiceClassCode = 200;//ddl 
        string CompanyName = "MIDWEST ELEVATOR";
        string CompanyIdentification = _dtBankDetail.Tables[0].Rows[0]["ACHCompanyHeaderString1"].ToString();
        int StandardEntryClassCode = 0;//ddl
        string CompanyEntryDescription = "";
        string OriginatorStatusCode = "1";
        string OriginatingDFIIdentification = _dtBankDetail.Tables[0].Rows[0]["NRoute"].ToString();
        string CompanyDiscretionaryData = "";
        string RecordTypeCode = "5";
        string BatchNumber = "1071000288000001";
        string CompanyDescriptiveDate = "";
        string EffectiveEntryDate = DateTime.Now.ToString("yyMMdd");   //"131011";
        string JulianDate = "000";
        if (cmbServiceClassCode != -1 && CompanyName != string.Empty && CompanyIdentification != string.Empty &&
           StandardEntryClassCode == 0 && OriginatorStatusCode != string.Empty &&
               OriginatingDFIIdentification != string.Empty && objEntry.BankRoutingNumberValidation(OriginatingDFIIdentification))
        {

            objBatchHeader.RecordTypeCode = RecordTypeCode;
            objBatchHeader.ServiceClassCode = cmbServiceClassCode.ToString();
            objBatchHeader.CompanyName = CompanyName;
            objBatchHeader.CompanyDiscretionaryData = CompanyDiscretionaryData;
            objBatchHeader.CompanyIdentification = CompanyIdentification;
            //objBatchHeader.StandardEntryClassCode = StandardEntryClassCode.ToString() == "0" ? "PPD" : "CCD";
            objBatchHeader.StandardEntryClassCode = "";
            objBatchHeader.CompanyEntryDescription = CompanyEntryDescription;
            objBatchHeader.CompanyDescriptiveDate = Convert.ToString(CompanyDescriptiveDate);//ToString("yy/MM/dd"));
            objBatchHeader.EffectiveEntryDate = Convert.ToString(EffectiveEntryDate);//ToString("yy/MM/dd"));
            objBatchHeader.JulianDate = JulianDate.ToString();
            objBatchHeader.OriginatorStatusCode = OriginatorStatusCode;
            objBatchHeader.OriginatingDFIIdentification = OriginatingDFIIdentification.Substring(0, 8);
            objBatchHeader.BatchNumber = BatchNumber;
            //string FileName = frmmain._strPath;

            if (objBatchHeader.IsBatchValid())
            {
                string filedata = string.Empty;
                string strcontent = string.Empty;
                string srEnd = string.Empty;

                m_flag = true;
                if (m_flag)
                {
                    using (StreamReader sr = new StreamReader(Append.FileName))
                    {
                        while (sr.Peek() >= 0)
                        {
                            srEnd = sr.ReadLine();
                            //if (srEnd.StartsWith("9"))
                            if (srEnd.StartsWith("2"))
                            {
                                strcontent = srEnd;
                            }
                        }
                    }
                    //sr.Close();
                    using (StreamReader srNew = new StreamReader(Append.FileName))
                    {
                        while (srNew.Peek() >= 0)
                        {
                            filedata = srNew.ReadToEnd();
                            if (strcontent != string.Empty)
                                filedata = filedata.Replace(strcontent, "").TrimEnd(filecontrolvariables.charRemove);
                        }
                    }
                    //srNew.Close();
                    using (StreamWriter swwrite = new StreamWriter(Append.FileName))
                    {
                        swwrite.Write(filedata);
                    }
                    //swwrite.Close();
                    m_flag = false;
                    //sr.Close();
                }

                objBatchHeader.saveBatchHeaderMidwest(Append.FileName);
                //("Batch for the record saved successfully","Message"

            }


        }
        else if (cmbServiceClassCode != -1 && CompanyName != string.Empty && CompanyIdentification != string.Empty &&
           StandardEntryClassCode == 1 && OriginatorStatusCode != string.Empty &&
               OriginatingDFIIdentification != string.Empty && objEntry.BankRoutingNumberValidation(OriginatingDFIIdentification))
        {


            {
                objBatchHeader.RecordTypeCode = RecordTypeCode;
                objBatchHeader.ServiceClassCode = cmbServiceClassCode.ToString();
                objBatchHeader.CompanyName = CompanyName;
                objBatchHeader.CompanyDiscretionaryData = CompanyDiscretionaryData;
                objBatchHeader.CompanyIdentification = CompanyIdentification;
                objBatchHeader.StandardEntryClassCode = StandardEntryClassCode.ToString();
                objBatchHeader.CompanyEntryDescription = CompanyEntryDescription;
                objBatchHeader.CompanyDescriptiveDate = Convert.ToString(CompanyDescriptiveDate);//ToString("yy/MM/dd"));
                objBatchHeader.EffectiveEntryDate = Convert.ToString(EffectiveEntryDate);//ToString("yy/MM/dd"));
                objBatchHeader.JulianDate = "".PadRight(3, ' ').ToString();
                objBatchHeader.OriginatorStatusCode = OriginatorStatusCode;
                objBatchHeader.OriginatingDFIIdentification = OriginatingDFIIdentification.Substring(0, 8);
                objBatchHeader.BatchNumber = BatchNumber;
                if (objBatchHeader.IsBatchValid())
                {
                    string filedata = string.Empty;
                    string strcontent = string.Empty;
                    string srEnd = string.Empty;

                    m_flag = true;
                    if (m_flag)
                    {
                        using (StreamReader sr = new StreamReader(Append.FileName))
                        {
                            while (sr.Peek() >= 0)
                            {
                                srEnd = sr.ReadLine();
                                if (srEnd.StartsWith("9"))
                                {
                                    strcontent = srEnd;
                                }
                            }
                            //   sr.Close();
                        }
                        using (StreamReader srNew = new StreamReader(Append.FileName))
                        {
                            while (srNew.Peek() >= 0)
                            {
                                filedata = srNew.ReadToEnd();
                                if (strcontent != string.Empty)
                                    filedata = filedata.Replace(strcontent, "").TrimEnd(filecontrolvariables.charRemove);
                            }
                            //   srNew.Close();
                        }
                        using (StreamWriter swwrite = new StreamWriter(Append.FileName))
                        {
                            swwrite.Write(filedata);
                            //swwrite.Close();
                        }



                        m_flag = false;
                        // sr.Close();
                    }

                    objBatchHeader.saveBatchHeaderMidwest(Append.FileName);
                    //"Batch for the record saved successfully"

                }
                else
                {
                    //You must save atleast one entry" 
                }
            }
        }
    }
    public void CreateFileControle()
    {
        Fileentry objfileentry = new Fileentry();
        StringBuilder sb = new StringBuilder();
        objfileentry.createFileEntryMidwest(Append.FileName, out sb);
        //File.AppendAllText(Append.FileName, Environment.NewLine + sb.ToString());
        string lastlines = "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999";
        sb.Append(Environment.NewLine + lastlines);
        sb.Append(Environment.NewLine + lastlines);
        sb.Append(Environment.NewLine + lastlines);
        sb.Append(Environment.NewLine + lastlines);
        File.AppendAllText(Append.FileName, Environment.NewLine + sb.ToString());

    }
    List<byte[]> lstbyte = new List<byte[]>();
    List<byte[]> lstbyteNew = new List<byte[]>();
    private void FillReportApTopCheckDataSet(string reportName)
    {
        try
        {
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtcheckfrom.Text);
            _objCD.NextC = long.Parse(txtcheckto.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
            _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
            _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
            _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);


            DataSet _dsCheck = new DataSet();
            DataSet _dsCheckDecustionDetail = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();

            ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

                _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

                _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
                _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

            }
            else
            {
                //Prateek
                _dsCheck = _objBLBill.GetPRCheckDetailsByBankAndRef(_objCD);
                _dsCheckDecustionDetail = _objBLBill.GetPRCheckDetailsByBankAndRefDecustion(_objCD);
            }
            //STIMULSOFT 
            byte[] buffer1 = null;
            string reportPathStimul = Server.MapPath("StimulsoftReports/PayrollChecks/" + reportName.Trim() + ".mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
            //{
            //    report.Pages[0].Watermark.Enabled = true;
            //    string imagepath = Server.MapPath("images/icons/voidcheck.png");
            //    report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
            //    report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
            //    report.Pages[0].Watermark.ShowImageBehind = true;
            //}
            report.Compile();

            report.RegData("CheckDetail", _dsCheck.Tables[0]);
            report.RegData("CheckOtherDeductionDetail", _dsCheckDecustionDetail.Tables[0]);

            report.Render();

            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(report, stream, settings);
            buffer1 = stream.ToArray();
            lstbyte.Add(buffer1);

            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }

            Response.Clear();
            MemoryStream ms = new MemoryStream(buffer1);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + reportName.Trim() + ".pdf");
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void CreateTableInvoice()
    {

        dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        dti.Columns.Add(new DataColumn("Total", typeof(string)));
        dti.Columns.Add(new DataColumn("Disc", typeof(string)));
        dti.Columns.Add(new DataColumn("AmountPay", typeof(string)));
        dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        dti.Columns.Add(new DataColumn("Description", typeof(string)));
    }
    private void CreateTablePayee()
    {
        dtpay.Columns.Add(new DataColumn("Pay", typeof(string)));
        dtpay.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        dtpay.Columns.Add(new DataColumn("Date", typeof(string)));
        dtpay.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        dtpay.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("State", typeof(string)));
        dtpay.Columns.Add(new DataColumn("Zip", typeof(string)));
        dtpay.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
    }
    private void CreateTableBank()
    {
        dtBank.Columns.Add(new DataColumn("Name", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Address", typeof(string)));
        dtBank.Columns.Add(new DataColumn("City", typeof(string)));
        dtBank.Columns.Add(new DataColumn("State", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Zip", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NBranch", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NAcct", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NRoute", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Ref", typeof(string)));
    }

    public string ConvertNumberToCurrency(double _amount)
    {
        string _currencyInWord = ConvertNumbertoWords(Convert.ToInt32(Math.Truncate(_amount)));
        double d = _amount - Math.Truncate(_amount);
        if (d > 0)
        {
            d = Math.Round(d * 100);
            _currencyInWord = _currencyInWord + " And " + d.ToString() + " / 100";
        }
        _currencyInWord = "*** " + _currencyInWord + "****************";
        return _currencyInWord;
    }

    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000000) + " Million ";
            number %= 1000000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            //var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            //var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };


            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }

    protected void ImageButton7_Click(object sender, ImageClickEventArgs e)
    {
        //mpeTemplate.Hide();
        string reportName = ddlApTopCheckForLoad.SelectedItem.Text.Trim();
        StiReport report = FillDataSetToReport(reportName);
        //StiReport report = FillReportApTopCheckDataSet(reportName);
        StiWebDesigner1.Report = report;
        StiWebDesigner1.Visible = true;
        //SiteWebDesigner01
        Session["wc_first"] = "true";
        string script = "function f(){$find(\"" + RadWindowFirstReport.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    }

    private StiReport FillDataSetToReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();

        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtcheckfrom.Text);
        _objCD.NextC = long.Parse(txtcheckto.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
        _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
        _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
        _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheckDecustionDetail = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();
        ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

            _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

            _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
            _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();
        }
        else
        {
            //_dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            _dsCheck = _objBLBill.GetPRCheckDetailsByBankAndRef(_objCD);
            _dsCheckDecustionDetail = _objBLBill.GetPRCheckDetailsByBankAndRefDecustion(_objCD);
        }

        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        string reportPathStimul = Server.MapPath("StimulsoftReports/PayrollChecks/" + reportName.Trim() + ".mrt");
        //  string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/APTopCheckDefault.mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;

        report.RegData("CheckDetail", _dsCheck.Tables[0]);
        report.RegData("CheckOtherDeductionDetail", _dsCheckDecustionDetail.Tables[0]);
        report.Render();
        return report;
    }

    protected void lnkSaveDefault_Click(object sender, EventArgs e)
    {
        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/PayrollChecks/Pr-deluxe 081064.mrt");
            string filePath = Server.MapPath("StimulsoftReports/PayrollChecks");
            string tempPath = Server.MapPath("StimulsoftReports/PayrollChecks");
            string selValue = ddlApTopCheckForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    Response.Redirect("PayrollRegister.aspx");

                }
                else
                    throw new Exception("PRCheckDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }


    }

    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {

        string filePath = Server.MapPath("StimulsoftReports/PayrollChecks");

        string selValue = ddlApTopCheckForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("PayrollChecks"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                ddlApTopCheckForLoad.Items.Clear();

                string path = Server.MapPath("StimulsoftReports/PayrollChecks/");
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.mrt");
                foreach (FileInfo file in Files)
                {
                    string FileName = string.Empty;
                    if (file.Name.Contains(".mrt"))
                        FileName = file.Name.Replace(".mrt", " ");
                    ddlApTopCheckForLoad.Items.Add((FileName));
                }
                ddlApTopCheckForLoad.Items.Remove(selValue);

                ddlApTopCheckForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            // btnCutCheck.Visible = true;
        }
    }

    //protected void imgPrintTemp2_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + ddlApMiddleCheckForLoad.SelectedItem.Text.Trim() + ".mrt");
    //        StiReport report = new StiReport();
    //        FillReportMiddleDataSet(ddlApMiddleCheckForLoad.SelectedItem.Text.Trim());
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

    private void FillReportMiddleDataSet(string reportName)
    {
        try
        {
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtcheckfrom.Text);
            _objCD.NextC = long.Parse(txtcheckto.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
            _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
            _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
            _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            DataSet _dsCheck = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();

            ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

                _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);
                _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
                _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

            }
            else
            {
                _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            }



            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            dtNew.Columns.Add("CheckNo");

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            }
            else
            {
                foreach (DataRow drow in _dsCheck.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            }

            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = true;
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();

                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                string checkNo = Convert.ToString(dr["CheckNo"].ToString());

                //RAHIL'S IMPLEMENTATION
                double AmountPay = 0.00;

                DataView dtInv = new DataView();
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    dtInv = _dsCheck1.Tables[0].DefaultView;
                }
                else
                {
                    dtInv = _dsCheck.Tables[0].DefaultView;
                }

                dtInv.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                foreach (DataRow drow in dtInv.ToTable(true).Rows)
                {
                    _dri = dti.NewRow();
                    _dri["Ref"] = drow["Ref"].ToString();
                    _dri["Description"] = drow["Description"].ToString();
                    _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
                    _dri["Reference"] = drow["Refrerence"].ToString();
                    _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(drow["AmountPay"].ToString());
                    _dri["PayDate"] = drow["PayDate"].ToString();
                    _dri["CheckNo"] = drow["CheckNo"].ToString();
                    _dri["VendorID"] = drow["Vendor"].ToString();
                    _dri["VendorName"] = drow["VendorName"].ToString();
                    dti.Rows.Add(_dri);

                    dti.AcceptChanges();
                }
                if (isChecked)
                {
                    if (dti.Rows.Count > 0)
                    {
                        DataView dtcheck = new DataView();
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            dtcheck = _dsCheck2.Tables[0].DefaultView;
                        }
                        else
                        {
                            dtcheck = _dsCheck.Tables[1].DefaultView;
                        }

                        dtcheck.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                        ViewState["CheckStatus"] = "0";
                        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
                        {
                            _drC = dtpay.NewRow();
                            if (Convert.ToDouble(drow["Pay"]) > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = drow["ToOrder"].ToString();
                            _drC["Date"] = drow["Date"].ToString();
                            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
                            _drC["VendorAddress"] = drow["VendorAddress"].ToString();
                            _drC["RemitAddress"] = drow["RemitAddress"].ToString();
                            _drC["State"] = drow["State"].ToString();
                            _drC["Zip"] = drow["Zip"].ToString();
                            ViewState["CheckStatus"] = drow["Status"].ToString();
                            dtpay.Rows.Add(_drC);
                        }

                        var rowCount = 0;
                        var totalRows = dti.Rows.Count;
                        if (reportName.Contains("-"))
                        {
                            try
                            {
                                string[] reportNameArr = reportName.Split('-');
                                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                if (totalRows < rowCount)
                                    rowCount = totalRows;
                            }
                            catch (Exception ex) { rowCount = totalRows; }
                        }
                        else
                            rowCount = 6;

                        var dtiCopy = dti.Copy();
                        DataView dv = dtiCopy.DefaultView;
                        dv.Sort = "Ref asc";
                        DataTable sortedDT = dv.ToTable();
                        var dtCopy = sortedDT.Copy();
                        var firstHalf = dtCopy;
                        var secondHalf = dtCopy;
                        if (dtCopy.Rows.Count > rowCount)
                        {
                            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                            secondHalf = dtCopy.Clone();
                            if (totalRows > rowCount)
                            {
                                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                            }
                        }
                        else
                        {
                            firstHalf = dtCopy;
                        }

                        //STIMULSOFT 
                        byte[] buffer1 = null;
                        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");
                        StiReport report = new StiReport();
                        report.Load(reportPathStimul);
                        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
                        {
                            report.Pages[0].Watermark.Enabled = true;
                            string imagepath = Server.MapPath("images/icons/voidcheck.png");
                            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
                            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                            report.Pages[0].Watermark.ShowImageBehind = true;
                        }
                        report.Compile();
                        report["TotalAmountPay"] = AmountPay;

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                        }
                        else
                        {
                            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                        }

                        report["InvoiceCount"] = totalRows;
                        DataSet Invoice = new DataSet();
                        DataTable dtInvoice = firstHalf.Copy();
                        dtInvoice.TableName = "Invoice";
                        Invoice.Tables.Add(dtInvoice);
                        Invoice.DataSetName = "Invoice";

                        DataSet Check = new DataSet();
                        DataTable dtCheck = dtpay.Copy();
                        dtCheck.TableName = "Check";
                        Check.Tables.Add(dtCheck);
                        Check.DataSetName = "Check";

                        report.RegData("Invoice", Invoice);
                        report.RegData("Check", Check);

                        report.Render();
                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        buffer1 = stream.ToArray();
                        lstbyte.Add(buffer1);

                        if (totalRows > rowCount)
                        {
                            byte[] bufferNew = null;
                            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                            report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck1.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                            }
                            else
                            {
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                            }

                            report["InvoiceCount"] = totalRows;
                            Invoice = new DataSet();
                            dtInvoice = secondHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            Check = new DataSet();
                            dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.Render();
                            settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            service = new Stimulsoft.Report.Export.StiPdfExportService();
                            stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            bufferNew = stream.ToArray();

                            lstbyteNew.Add(bufferNew);
                        }
                    }

                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();
            }

            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=ApTopCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    //protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
    //{

    //    //mpeTemplate.Hide();

    //    string reportName = ddlApMiddleCheckForLoad.SelectedItem.Text.Trim();
    //    StiReport report = FillMiddleDataSetReport(reportName);
    //    StiWebDesigner2.Report = report;
    //    //ReportModalPopupExtender1.Show();
    //    Session["wc_second"] = "true";
    //    StiWebDesigner2.Visible = true;

    //    string script = "function f(){$find(\"" + RadWindowSecondReport.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
    //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    //}
    private StiReport FillMiddleDataSetReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));


        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtcheckfrom.Text);
        _objCD.NextC = long.Parse(txtcheckto.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
        _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
        _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
        _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();
        ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

            _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

            _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
            _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();
        }
        else
        {
            _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
        }
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Name");
        dtNew.Columns.Add("Vendor");

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        }
        else
        {
            foreach (DataRow drow in _dsCheck.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        }
        DataTable dtN = dtNew.DefaultView.ToTable(true);
        DataTable _dtAcct = new DataTable();
        int vid = Convert.ToInt32(dtN.Rows[0]["Vendor"].ToString());
        SumAmountpay = 0.00;
        DataView dtInv = new DataView();
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            dtInv = _dsCheck1.Tables[0].DefaultView;
        }
        else
        {
            dtInv = _dsCheck.Tables[0].DefaultView;
        }

        dtInv.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtInv.ToTable(true).Rows)
        {
            _dri = _dti.NewRow();
            _dri["Ref"] = drow["Ref"].ToString();
            _dri["Description"] = drow["Description"].ToString();
            _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
            _dri["Reference"] = drow["Refrerence"].ToString();
            _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                          NumberStyles.AllowThousands |
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
            _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
            SumAmountpay = SumAmountpay + Convert.ToDouble(drow["AmountPay"].ToString());
            _dri["PayDate"] = drow["PayDate"].ToString();
            _dri["CheckNo"] = drow["CheckNo"].ToString();
            _dri["VendorID"] = drow["Vendor"].ToString();
            _dri["VendorName"] = drow["VendorName"].ToString();
            _dti.Rows.Add(_dri);

            _dti.AcceptChanges();

        }
        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
            // _dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }
        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }

        DataView dtcheck = new DataView();
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            dtcheck = _dsCheck2.Tables[0].DefaultView;
        }
        else
        {
            dtcheck = _dsCheck.Tables[1].DefaultView;
        }

        dtcheck.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
        {
            _drC = _dtCheck.NewRow();
            if (Convert.ToDouble(drow["Pay"]) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                //_drC["Pay"] = ViewState["Dollar"].ToString();
            }
            else
            {
                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = drow["ToOrder"].ToString();
            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = drow["Date"].ToString();
            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;

            _drC["TotalAmountpay"] = SumAmountpay;
            _drC["State"] = drow["State"].ToString();
            _dtCheck.Rows.Add(_drC);
        }
        DataSet dsCC = new DataSet();
        User objPropUser = new User();
        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();

        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _getControlBranch.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() != "TS")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsCC = objBL_User.getControl(objPropUser);
            }
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);
            _getControlBranch.LocID = Convert.ToInt32(0);
            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsCC = objBL_User.getControlBranch(objPropUser);
            }
        }

        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");

        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
        }
        else
        {
            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
        }

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsCC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";

        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.Render();
        return report;
    }

    //protected void lnkSaveApMiddleCheck_Click(object sender, EventArgs e)
    //{

    //    try
    //    {
    //        string defaultpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/APMidCheckDefault.mrt");
    //        string filePath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");
    //        string tempPath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");

    //        string selValue = ddlApMiddleCheckForLoad.Text.TrimEnd();
    //        if (selValue != null)
    //        {
    //            filePath = filePath + "\\" + selValue + ".mrt";
    //            tempPath = tempPath + "\\" + selValue + "temp.mrt";
    //            if (File.Exists(defaultpath))
    //            {
    //                string[] lines = System.IO.File.ReadAllLines(defaultpath);
    //                var myfile = File.Create(tempPath);
    //                myfile.Close();
    //                using (TextWriter tw = new StreamWriter(tempPath))
    //                    foreach (string line in lines)
    //                    {
    //                        tw.WriteLine(line);
    //                    }
    //                File.Delete(defaultpath);
    //                if (File.Exists(filePath))
    //                {
    //                    string[] lines1 = System.IO.File.ReadAllLines(filePath);
    //                    var myfile1 = File.Create(defaultpath);
    //                    myfile1.Close();
    //                    using (TextWriter tw1 = new StreamWriter(defaultpath))
    //                        foreach (string line1 in lines1)
    //                        {
    //                            tw1.WriteLine(line1);
    //                        }
    //                    File.Delete(filePath);
    //                }
    //                if (File.Exists(tempPath))
    //                {
    //                    string[] lines2 = System.IO.File.ReadAllLines(tempPath);
    //                    var myfile2 = File.Create(filePath);
    //                    myfile2.Close();
    //                    using (TextWriter tw2 = new StreamWriter(filePath))
    //                        foreach (string line2 in lines2)
    //                        {
    //                            tw2.WriteLine(line2);
    //                        }
    //                    File.Delete(tempPath);
    //                }
    //                Response.Redirect("ManageChecks.aspx");

    //            }
    //            else
    //                throw new Exception("ApMiddleCheckDefault.mrt is not available");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }


    //}

    //protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
    //{

    //    string filePath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");

    //    string selValue = ddlApMiddleCheckForLoad.Text.Trim();
    //    if (selValue != null)
    //    {
    //        filePath = filePath + "\\" + selValue + ".mrt";
    //        if (!selValue.Equals("APMidCheckDefault"))
    //        {
    //            if (File.Exists(filePath))
    //            {
    //                File.Delete(filePath);
    //            }
    //            ddlApMiddleCheckForLoad.Items.Clear();
    //            string MidCheckpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/");
    //            DirectoryInfo dirMidPath = new DirectoryInfo(MidCheckpath);
    //            FileInfo[] FilesMid = dirMidPath.GetFiles("*.mrt");
    //            foreach (FileInfo fileMid in FilesMid)
    //            {
    //                string FileName = string.Empty;
    //                if (fileMid.Name.Contains(".mrt"))
    //                    FileName = fileMid.Name.Replace(".mrt", " ");
    //                ddlApMiddleCheckForLoad.Items.Add((FileName));
    //            }
    //            ddlApMiddleCheckForLoad.Items.Remove(selValue);
    //            ddlApMiddleCheckForLoad.DataBind();
    //            string str = "Template " + selValue + " Deleted!--";
    //            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
    //        }
    //    }
    //}

    //protected void imgPrintTemp6_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + ddlTopChecksForLoad.SelectedItem.Text.Trim() + ".mrt");
    //        StiReport report = new StiReport();
    //        FillReportMaddenDataSet(ddlTopChecksForLoad.SelectedItem.Text.Trim());

    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

    private void FillReportMaddenDataSet(string reportName)
    {
        try
        {
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtcheckfrom.Text);
            _objCD.NextC = long.Parse(txtcheckto.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
            _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
            _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
            _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            DataSet _dsCheck = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();

            ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

                _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);
                _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
                _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

            }
            else
            {
                _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            }



            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            dtNew.Columns.Add("CheckNo");

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            }
            else
            {
                foreach (DataRow drow in _dsCheck.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            }

            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = true;
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();


                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                string checkNo = Convert.ToString(dr["CheckNo"].ToString());
                DataRow _drB = null;
                DataRow _drA = null;
                double AmountPay = 0.00;

                DataView dtInv = new DataView();
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    dtInv = _dsCheck1.Tables[0].DefaultView;
                }
                else
                {
                    dtInv = _dsCheck.Tables[0].DefaultView;
                }

                dtInv.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                foreach (DataRow drow in dtInv.ToTable(true).Rows)
                {
                    _dri = dti.NewRow();
                    _dri["Ref"] = drow["Ref"].ToString();
                    _dri["Description"] = drow["Description"].ToString();
                    _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
                    _dri["Reference"] = drow["Refrerence"].ToString();
                    _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(drow["AmountPay"].ToString());
                    _dri["PayDate"] = drow["PayDate"].ToString();
                    _dri["CheckNo"] = drow["CheckNo"].ToString();
                    _dri["VendorID"] = drow["Vendor"].ToString();
                    _dri["VendorName"] = drow["VendorName"].ToString();
                    dti.Rows.Add(_dri);

                    dti.AcceptChanges();
                }
                if (isChecked)
                {
                    if (dti.Rows.Count > 0)
                    {
                        string chknos = null;
                        DataView dtcheck = new DataView();
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            dtcheck = _dsCheck2.Tables[0].DefaultView;
                        }
                        else
                        {
                            dtcheck = _dsCheck.Tables[1].DefaultView;
                        }

                        dtcheck.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                        ViewState["CheckStatus"] = "0";
                        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
                        {
                            _drC = dtpay.NewRow();
                            if (Convert.ToDouble(drow["Pay"]) > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = drow["ToOrder"].ToString();
                            _drC["Date"] = drow["Date"].ToString();
                            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
                            _drC["VendorAddress"] = drow["VendorAddress"].ToString();
                            _drC["RemitAddress"] = drow["RemitAddress"].ToString();
                            _drC["State"] = drow["State"].ToString();
                            _drC["Zip"] = drow["Zip"].ToString();
                            ViewState["CheckStatus"] = drow["Status"].ToString();
                            chknos = drow["CheckNo"].ToString();
                            dtpay.Rows.Add(_drC);
                        }

                        _objBank.ConnConfig = Session["config"].ToString();
                        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

                        _getBankCD.ConnConfig = Session["config"].ToString();
                        _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);

                        DataSet _dsB = new DataSet();
                        List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

                            _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                            _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                            _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
                            _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
                            _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
                            _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
                            _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";

                        }
                        else
                        {
                            _dsB = _objBLBill.GetBankCD(_objBank);
                        }

                        _drB = dtBank.NewRow();
                        if (_dsB.Tables[0].Rows.Count > 0)
                        {
                            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                        }
                        string checkNumber = string.Empty;
                        if (!string.IsNullOrEmpty(chknos))
                        {
                            checkNumber = chknos.ToString();
                        }
                        else
                        {
                            checkNumber = chknos.ToString();
                        }

                        if (checkNumber.Length == 1)
                        {
                            _drB["Ref"] = "00000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 2)
                        {
                            _drB["Ref"] = "0000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 3)
                        {
                            _drB["Ref"] = "000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 4)
                        {
                            _drB["Ref"] = "00000" + checkNumber;
                        }
                        else if (checkNumber.Length == 5)
                        {
                            _drB["Ref"] = "0000" + checkNumber;
                        }
                        else if (checkNumber.Length == 6)
                        {
                            _drB["Ref"] = "000" + checkNumber;
                        }
                        else if (checkNumber.Length == 7)
                        {
                            _drB["Ref"] = "00" + checkNumber;
                        }
                        else if (checkNumber.Length == 8)
                        {
                            _drB["Ref"] = "0" + checkNumber;
                        }
                        else
                        {
                            _drB["Ref"] = "000000000";
                        }

                        dtBank.Rows.Add(_drB);

                        _objVendor.ConnConfig = Session["config"].ToString();
                        _objVendor.ID = vid;

                        _getVendorAcct.ConnConfig = Session["config"].ToString();
                        _getVendorAcct.ID = vid;

                        DataSet _dsA = new DataSet();
                        List<GetVendorAcctList> _lstGetVendorAcctList = new List<GetVendorAcctList>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                            _lstGetVendorAcctList = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                            _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcctList);
                            _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                        }
                        else
                        {
                            _dsA = _objBLVendor.GetVendorAcct(_objVendor);
                        }

                        _drA = _dtAcct.NewRow();
                        _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
                        _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                        _dtAcct.Rows.Add(_drA);

                        var rowCount = 0;
                        var totalRows = dti.Rows.Count;
                        if (reportName.Contains("-"))
                        {
                            try
                            {
                                string[] reportNameArr = reportName.Split('-');
                                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                if (totalRows < rowCount)
                                    rowCount = totalRows;
                            }
                            catch (Exception ex) { rowCount = totalRows; }
                        }
                        else
                            rowCount = 6;
                        var dtiCopy = dti.Copy();
                        DataView dv = dtiCopy.DefaultView;
                        dv.Sort = "Ref asc";
                        DataTable sortedDT = dv.ToTable();
                        var dtCopy = sortedDT.Copy();
                        var firstHalf = dtCopy;
                        var secondHalf = dtCopy;
                        if (dtCopy.Rows.Count > rowCount)
                        {
                            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                            secondHalf = dtCopy.Clone();
                            if (totalRows > rowCount)
                            {
                                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                            }
                        }
                        else
                        {
                            firstHalf = dtCopy;
                        }

                        DataSet dsCC = new DataSet();
                        User objPropUser = new User();
                        objPropUser.ConnConfig = Session["config"].ToString();
                        _getConnectionConfig.ConnConfig = Session["config"].ToString();
                        _getControlBranch.ConnConfig = Session["config"].ToString();

                        if (Session["MSM"].ToString() != "TS")
                        {
                            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                                dsCC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                            }
                            else
                            {
                                dsCC = objBL_User.getControl(objPropUser);
                            }
                        }
                        else
                        {
                            objPropUser.LocID = Convert.ToInt32(0);
                            _getControlBranch.LocID = Convert.ToInt32(0);

                            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                                dsCC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                            }
                            else
                            {
                                dsCC = objBL_User.getControlBranch(objPropUser);
                            }

                        }

                        //STIMULSOFT 
                        byte[] buffer1 = null;
                        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
                        StiReport report = new StiReport();
                        report.Load(reportPathStimul);
                        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
                        {
                            report.Pages[0].Watermark.Enabled = true;
                            string imagepath = Server.MapPath("images/icons/voidcheck.png");
                            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
                            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
                            report.Pages[0].Watermark.ShowImageBehind = true;
                        }
                        report.Compile();
                        report["TotalAmountPay"] = AmountPay;

                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                        }
                        else
                        {
                            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                        }

                        report["InvoiceCount"] = totalRows;
                        if (dsCC.Tables[0].Rows.Count > 0)
                        {
                            report["CompanyName"] = dsCC.Tables[0].Rows[0]["Name"].ToString();
                            report["CompanyAddress"] = dsCC.Tables[0].Rows[0]["Address"].ToString();
                            report["CompanyCity"] = dsCC.Tables[0].Rows[0]["City"].ToString() + "' " + dsCC.Tables[0].Rows[0]["State"].ToString() + " - " + dsCC.Tables[0].Rows[0]["Zip"].ToString();
                        }
                        DataSet Invoice = new DataSet();
                        DataTable dtInvoice = firstHalf.Copy();
                        dtInvoice.TableName = "Invoice";
                        Invoice.Tables.Add(dtInvoice);
                        Invoice.DataSetName = "Invoice";

                        DataSet Check = new DataSet();
                        DataTable dtCheck = dtpay.Copy();
                        dtCheck.TableName = "Check";
                        Check.Tables.Add(dtCheck);
                        Check.DataSetName = "Check";

                        DataSet ControlBranch = new DataSet();
                        DataTable dtControlBranch = new DataTable();
                        dtControlBranch = dsCC.Tables[0].Copy();
                        ControlBranch.Tables.Add(dtControlBranch);
                        dtControlBranch.TableName = "ControlBranch";
                        ControlBranch.DataSetName = "ControlBranch";

                        DataSet Bank = new DataSet();
                        DataTable _dtBank = dtBank.Copy();
                        _dtBank.TableName = "Bank";
                        Bank.Tables.Add(_dtBank);
                        Bank.DataSetName = "Bank";

                        DataSet Account = new DataSet();
                        DataTable dtAccount = _dtAcct.Copy();
                        dtAccount.TableName = "Account";
                        Account.Tables.Add(dtAccount);
                        Account.DataSetName = "Account";

                        report.RegData("Invoice", Invoice);
                        report.RegData("Check", Check);
                        report.RegData("Bank", Bank);
                        report.RegData("Account", Account);
                        report.Render();

                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        buffer1 = stream.ToArray();
                        lstbyte.Add(buffer1);

                        if (totalRows > rowCount)
                        {
                            byte[] bufferNew = null;
                            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                            report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck1.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                            }
                            else
                            {
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                            }

                            report["InvoiceCount"] = totalRows;
                            Invoice = new DataSet();
                            dtInvoice = secondHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            Check = new DataSet();
                            dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            ControlBranch = new DataSet();
                            dtControlBranch = new DataTable();
                            dtControlBranch = dsCC.Tables[0].Copy();
                            ControlBranch.Tables.Add(dtControlBranch);
                            dtControlBranch.TableName = "ControlBranch";
                            ControlBranch.DataSetName = "ControlBranch";

                            Bank = new DataSet();
                            _dtBank = dtBank.Copy();
                            _dtBank.TableName = "Bank";
                            Bank.Tables.Add(_dtBank);
                            Bank.DataSetName = "Bank";

                            Account = new DataSet();
                            dtAccount = _dtAcct.Copy();
                            dtAccount.TableName = "Account";
                            Account.Tables.Add(dtAccount);
                            Account.DataSetName = "Account";

                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.RegData("Bank", Bank);
                            report.RegData("Account", Account);
                            report.Render();

                            settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            service = new Stimulsoft.Report.Export.StiPdfExportService();
                            stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            bufferNew = stream.ToArray();

                            lstbyteNew.Add(bufferNew);
                        }
                    }
                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();
            }

            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=ApTopCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    //protected void ImageButton9_Click(object sender, ImageClickEventArgs e)
    //{
    //    string reportName = ddlTopChecksForLoad.SelectedItem.Text.Trim();
    //    StiReport report = FillMaddenDataSetForReport(reportName);
    //    StiWebDesigner3.Report = report;
    //    Session["wc_third"] = "true";
    //    StiWebDesigner3.Visible = true;
    //    string script = "function f(){$find(\"" + RadWindowThirdReport.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
    //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    //}
    private StiReport FillMaddenDataSetForReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));

        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtcheckfrom.Text);
        _objCD.NextC = long.Parse(txtcheckto.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
        _getCheckDetailsByBankAndRef.Ref = long.Parse(txtcheckfrom.Text);
        _getCheckDetailsByBankAndRef.NextC = long.Parse(txtcheckto.Text);
        _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();

        ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

            _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);
            _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
            _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();
        }
        else
        {
            _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
        }
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Name");
        dtNew.Columns.Add("Vendor");

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        }
        else
        {
            foreach (DataRow drow in _dsCheck.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        }
        DataTable dtN = dtNew.DefaultView.ToTable(true);
        int vid = Convert.ToInt32(dtN.Rows[0]["Vendor"].ToString());
        SumAmountpay = 0.00;
        DataView dtInv = new DataView();
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            dtInv = _dsCheck1.Tables[0].DefaultView;
        }
        else
        {
            dtInv = _dsCheck.Tables[0].DefaultView;
        }

        dtInv.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtInv.ToTable(true).Rows)
        {
            _dri = _dti.NewRow();
            _dri["Ref"] = drow["Ref"].ToString();
            _dri["Description"] = drow["Description"].ToString();
            _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
            _dri["Reference"] = drow["Refrerence"].ToString();
            _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                          NumberStyles.AllowThousands |
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
            _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
            SumAmountpay = SumAmountpay + Convert.ToDouble(drow["AmountPay"].ToString());
            _dri["PayDate"] = drow["PayDate"].ToString();
            _dri["CheckNo"] = drow["CheckNo"].ToString();
            _dri["VendorID"] = drow["Vendor"].ToString();
            _dri["VendorName"] = drow["VendorName"].ToString();
            _dti.Rows.Add(_dri);

            _dti.AcceptChanges();

        }

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }
        string chknos = null;
        DataView dtcheck = new DataView();
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            dtcheck = _dsCheck2.Tables[0].DefaultView;
        }
        else
        {
            dtcheck = _dsCheck.Tables[1].DefaultView;
        }

        dtcheck.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
        {
            _drC = _dtCheck.NewRow();
            if (Convert.ToDouble(drow["Pay"]) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
            }
            else
            {
                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = drow["ToOrder"].ToString();
            _drC["Date"] = drow["Date"].ToString();
            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;

            _drC["TotalAmountpay"] = SumAmountpay;
            _drC["State"] = drow["State"].ToString();
            chknos = drow["CheckNo"].ToString();
            _dtCheck.Rows.Add(_drC);
        }

        DataSet dsCC = new DataSet();
        User objPropUser = new User();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _getControlBranch.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() != "TS")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsCC = objBL_User.getControl(objPropUser);
            }
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);

            _getControlBranch.LocID = Convert.ToInt32(0);

            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsCC = objBL_User.getControlBranch(objPropUser);
            }
        }
        CreateTableBank();

        DataRow _drB = null;
        DataRow _drA = null;
        _objBank.ConnConfig = Session["config"].ToString();
        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

        _getBankCD.ConnConfig = Session["config"].ToString();
        _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsB = new DataSet();
        List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

            _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
            _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
            _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
            _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
            _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
            _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
            _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
        }
        else
        {
            _dsB = _objBLBill.GetBankCD(_objBank);
        }

        _drB = dtBank.NewRow();
        if (_dsB.Tables[0].Rows.Count > 0)
        {
            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
            //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
            //_dtBank.Rows.Add(_drB);
        }

        string checkNumber = string.Empty;
        if (!string.IsNullOrEmpty(chknos))
        {
            checkNumber = chknos;
        }
        else
        {
            checkNumber = chknos.ToString();
        }

        if (checkNumber.Length == 1)
        {
            _drB["Ref"] = "00000000" + checkNumber;
        }
        else if (checkNumber.Length == 2)
        {
            _drB["Ref"] = "0000000" + checkNumber;
        }
        else if (checkNumber.Length == 3)
        {
            _drB["Ref"] = "000000" + checkNumber;
        }
        else if (checkNumber.Length == 4)
        {
            _drB["Ref"] = "00000" + checkNumber;
        }
        else if (checkNumber.Length == 5)
        {
            _drB["Ref"] = "0000" + checkNumber;
        }
        else if (checkNumber.Length == 6)
        {
            _drB["Ref"] = "000" + checkNumber;
        }
        else if (checkNumber.Length == 7)
        {
            _drB["Ref"] = "00" + checkNumber;
        }
        else if (checkNumber.Length == 8)
        {
            _drB["Ref"] = "0" + checkNumber;
        }
        else
        {
            _drB["Ref"] = "000000000";
        }

        dtBank.Rows.Add(_drB);

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorAcct.ConnConfig = Session["config"].ToString();
        _getVendorAcct.ID = vid;

        DataSet _dsA = new DataSet();
        List<GetVendorAcctList> _lstGetVendorAcctList = new List<GetVendorAcctList>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

            _lstGetVendorAcctList = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
            _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcctList);
            _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
        }
        else
        {
            _dsA = _objBLVendor.GetVendorAcct(_objVendor);
        }

        DataTable _dtAcct = new DataTable();
        _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));
        if (_dsA.Tables[0].Rows.Count > 0)
        {
            _drA = _dtAcct.NewRow();
            _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
            _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
            _dtAcct.Rows.Add(_drA);
        }

        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;


        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
        }
        else
        {
            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
        }

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsCC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";


        DataSet Bank = new DataSet();
        DataTable _dtBank = dtBank;
        dtBank.TableName = "Bank";
        Bank.Tables.Add(dtBank);
        Bank.DataSetName = "Bank";

        DataSet Account = new DataSet();
        DataTable dtAccount = _dtAcct;
        _dtAcct.TableName = "Account";
        Account.Tables.Add(_dtAcct);
        Account.DataSetName = "Account";


        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.RegData("dsBank", Bank);
        report.RegData("dsAccount", Account);
        report.Render();
        return report;
    }
    //protected void lnkTopChecks_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string defaultpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/TopCheckReportDefault.mrt");
    //        string filePath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");
    //        string tempPath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");
    //        string selValue = ddlTopChecksForLoad.Text.TrimEnd();
    //        if (selValue != null)
    //        {
    //            filePath = filePath + "\\" + selValue + ".mrt";
    //            tempPath = tempPath + "\\" + selValue + "temp.mrt";
    //            if (File.Exists(defaultpath))
    //            {
    //                string[] lines = System.IO.File.ReadAllLines(defaultpath);
    //                var myfile = File.Create(tempPath);
    //                myfile.Close();
    //                using (TextWriter tw = new StreamWriter(tempPath))
    //                    foreach (string line in lines)
    //                    {
    //                        tw.WriteLine(line);
    //                    }
    //                File.Delete(defaultpath);
    //                if (File.Exists(filePath))
    //                {
    //                    string[] lines1 = System.IO.File.ReadAllLines(filePath);
    //                    var myfile1 = File.Create(defaultpath);
    //                    myfile1.Close();
    //                    using (TextWriter tw1 = new StreamWriter(defaultpath))
    //                        foreach (string line1 in lines1)
    //                        {
    //                            tw1.WriteLine(line1);
    //                        }
    //                    File.Delete(filePath);
    //                }
    //                if (File.Exists(tempPath))
    //                {
    //                    string[] lines2 = System.IO.File.ReadAllLines(tempPath);
    //                    var myfile2 = File.Create(filePath);
    //                    myfile2.Close();
    //                    using (TextWriter tw2 = new StreamWriter(filePath))
    //                        foreach (string line2 in lines2)
    //                        {
    //                            tw2.WriteLine(line2);
    //                        }
    //                    File.Delete(tempPath);
    //                }
    //                Response.Redirect("ManageChecks.aspx");

    //            }
    //            else
    //                throw new Exception("TopCheckReportDefault.mrt is not available");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }

    //}
    //protected void ImageButton14_Click(object sender, ImageClickEventArgs e)
    //{
    //    string filePath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");

    //    string selValue = ddlTopChecksForLoad.Text.Trim();
    //    if (selValue != null)
    //    {
    //        filePath = filePath + "\\" + selValue + ".mrt";
    //        if (!selValue.Equals("TopCheckReportDefault"))
    //        {
    //            if (File.Exists(filePath))
    //            {
    //                File.Delete(filePath);
    //            }
    //            ddlTopChecksForLoad.Items.Clear();

    //            string TopCheckpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/");
    //            DirectoryInfo dirTopcheckPath = new DirectoryInfo(TopCheckpath);
    //            FileInfo[] FilesTop = dirTopcheckPath.GetFiles("*.mrt");
    //            foreach (FileInfo fileTop in FilesTop)
    //            {
    //                string FileName = string.Empty;
    //                if (fileTop.Name.Contains(".mrt"))
    //                    FileName = fileTop.Name.Replace(".mrt", " ");
    //                ddlTopChecksForLoad.Items.Add((FileName));
    //            }
    //            ddlTopChecksForLoad.Items.Remove((selValue));
    //            ddlTopChecksForLoad.DataBind();
    //            string str = "Template " + selValue + " Deleted!--";
    //            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
    //        }
    //    }
    //}
    protected void StiWebDesigner1_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        Session["wc_first"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + e.FileName));
    }
    protected void StiWebDesigner1_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + e.FileName));
    }
    protected void StiWebDesigner1_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("ManageChecks.aspx");
    }
    protected void StiWebDesigner2_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + e.FileName));
    }
    protected void StiWebDesigner3_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + e.FileName));
    }
    protected void StiWebDesigner2_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("ManageChecks.aspx");

    }
    protected void StiWebDesigner3_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("ManageChecks.aspx");
    }
    protected void StiWebDesigner2_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        Session["wc_second"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + e.FileName));
    }
    protected void StiWebDesigner3_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        Session["wc_third"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + e.FileName));
    }
}
