using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using Telerik.Web.UI;
using Stimulsoft.Report;
using System.Configuration;
using System.Linq.Dynamic;
using Telerik.Web.UI.GridExcelBuilder;
using System.Globalization;
using System.Net;
using Stimulsoft.Report.Web;
using System.Text;

public partial class ManagePO : System.Web.UI.Page
{
    #region "Variables"
    public static bool isChecked = false;
    PJ _objPJ = new PJ();
    BL_Bills _objBLBills = new BL_Bills();
    PO _objPO = new PO();
    ApprovePOStatus _objApprovePOStatus = new ApprovePOStatus();

    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();

    Customer objPropCustomer = new Customer();
    BL_Customer bL_Customer = new BL_Customer();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BL_Report bL_Report = new BL_Report();

    #endregion

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

                if (string.IsNullOrWhiteSpace(GetDefaultGridColumnSettingsFromDb()))
                {
                    // Get initial grid settings
                    var gridDefault = GetGridColumnSettings();
                    // Save default settings to database
                    _objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
                    _objPropUser.UserID = 0;// UserId = 0 for default
                    _objPropUser.PageName = "ManagePO.aspx";
                    _objPropUser.GridId = "RadGrid_ManagePO";

                    _objBLUser.UpdateUserGridCustomSettings(_objPropUser, gridDefault);
                }

                BindSearchFilters();
                // Load PO Limit Data
                _objPO.UserID = Convert.ToInt32(Session["userid"]);
                _objPO.ConnConfig = Session["config"].ToString();
                DataSet ApprovePO = _objBLBills.GetPOApproveDetails(_objPO);
                if (ApprovePO.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ApprovePO.Tables[0].Rows[0];
                    hdnPOLimit.Value = Convert.ToString(row["POLimit"]);
                    hdnMinAmount.Value = Convert.ToString(row["MinAmount"]);
                    hdnMaxAmount.Value = Convert.ToString(row["MaxAmount"]);
                    hdnPOApproveAmt.Value = Convert.ToString(row["POApproveAmt"]);
                    hdnPOApprove.Value = Convert.ToString(row["POApprove"]);
                }

                #region Show Selected Filter
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["from_ManagePO"] != null && Session["end_ManagePO"] != null)
                    {
                        txtFromDate.Text = Convert.ToString(Session["from_ManagePO"]);
                        txtToDate.Text = Convert.ToString(Session["end_ManagePO"]);
                    }
                    else
                    {
                        txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                        txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                    }
                    if (Session["ddlSearch_ManagePO"] != null)
                    {
                        String selectedValue = Convert.ToString(Session["ddlSearch_ManagePO"]);
                        ddlSearch.SelectedValue = selectedValue;

                        String searchValue = Convert.ToString(Session["ddlSearch_Value_ManagePO"]);
                        if (selectedValue == "p.PO")
                        {
                            txtSearch.Visible = false;
                            txtManagePOSearch.Visible = true;
                            txtManagePOSearch.Text = searchValue;
                        }
                        else if (selectedValue == "p.Status")
                        {
                            ddlSearch_SelectedIndexChanged(sender, e);
                            ddlStatus.SelectedValue = searchValue;
                        }
                        else if (selectedValue == "vs.Status")
                        {
                            ddlSearch_SelectedIndexChanged(sender, e);
                            ddlApprovalStatus.SelectedValue = searchValue;
                        }
                        else
                        {
                            txtSearch.Visible = true;
                            txtSearch.Text = searchValue;
                            txtManagePOSearch.Visible = false;
                        }
                    }
                }
                else
                {
                    Session["ddlSearch_ManagePO"] = null;
                    Session["ddlSearch_Value_ManagePO"] = null;
                    Session["from_ManagePO"] = null;
                    Session["end_ManagePO"] = null;

                    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                }
                #endregion

                // Add Custom PO template menu
                listCustomPO.DataSource = GetListPOTemplate();
                listCustomPO.DataBind();

                Permission();
                HighlightSideMenu("purchaseMgr", "lnkPO", "purchaseMgrSub");

                lnkPrintCustomPOReport.Visible = false;
                lnkMailCustomPOReport.Visible = false;

                if (ConfigurationManager.AppSettings["CustomPOReport"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["CustomPOReport"]))
                {
                    lnkPrintCustomPOReport.Visible = true;
                    lnkMailCustomPOReport.Visible = true;

                    lnkPrintCustomPOReport.Text = $"<i class='fa fa-file-pdf-o' style='color:red;background-color:transparent' aria-hidden='true'></i>&nbsp; {ConfigurationManager.AppSettings["CustomerName"]} - PO Report <i class='fa fa-download' aria-hidden='true' style='background-color:transparent;'></i>";
                    lnkMailCustomPOReport.Text = $"<i class='fa fa-file-pdf-o' style='color:red;background-color:transparent' aria-hidden='true'></i>&nbsp; {ConfigurationManager.AppSettings["CustomerName"]} - PO Report <i class='fa fa-download' aria-hidden='true' style='background-color:transparent;'></i>";
                }
            }

            CompanyPermission();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Custom functions


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
            //Response.Redirect("home.aspx");
            //pnlGridButtons.Visible = false;
        }
        // Removed this for ES-4346 BL - User joe unable to acees the modules he have access
        //if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        //{
        //    Response.Redirect("home.aspx");
        //}

        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            //ds = (DataTable)Session["userinfo"];
            ds = GetUserById();


            /// PurchasingmodulePermission ///////////////////------->

            string PurchasingmodulePermission = ds.Rows[0]["PurchasingmodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["PurchasingmodulePermission"].ToString();

            if (PurchasingmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// POPermission ///////////////////------->

            string POPermission = ds.Rows[0]["PO"] == DBNull.Value ? "YYYY" : ds.Rows[0]["PO"].ToString();
            string ADD = POPermission.Length < 1 ? "Y" : POPermission.Substring(0, 1);
            string Edit = POPermission.Length < 2 ? "Y" : POPermission.Substring(1, 1);
            string Delete = POPermission.Length < 2 ? "Y" : POPermission.Substring(2, 1);
            string View = POPermission.Length < 4 ? "Y" : POPermission.Substring(3, 1);

            if (ADD == "N")
            {

                lnkAddPO.Visible = false;
                lnkCopy.Visible = false;
            }
            if (Edit == "N")
            {
                lnkEdit.Visible = false;

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
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_ManagePO.Columns[7].Visible = true;
        }
        else
        {
            RadGrid_ManagePO.Columns[7].Visible = false;
        }
    }
    private void BindManagePO()
    {
        BL_Bills _objBLBills = new BL_Bills();
        PO _objPO = new PO();
        DataSet _dsPJ = new DataSet();

        //if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
        //{
        //    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
        //    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
        //}

        if (ddlSearch.SelectedValue == "vs.Status")
        {
            if (ddlApprovalStatus.SelectedValue != "")
                _objApprovePOStatus.Status = Convert.ToInt16(ddlApprovalStatus.SelectedValue);
            if (txtFromDate.Text != "")
                _objApprovePOStatus.ApproveFrom = Convert.ToDateTime(txtFromDate.Text);
            else
                _objApprovePOStatus.ApproveFrom = null;
            if (txtToDate.Text != "")
                _objApprovePOStatus.ApproveTo = Convert.ToDateTime(txtToDate.Text);
            else
                _objApprovePOStatus.ApproveTo = null;
        }
        else
        {
            _objPO.SearchValue = txtManagePOSearch.Text;
        }

        _objPO.SearchBy = ddlSearch.SelectedValue;
        if (ddlSearch.SelectedValue == "p.Status")
        {
            _objPO.SearchValue = ddlStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "r.Name" || ddlSearch.SelectedValue == "Projectnumber" || ddlSearch.SelectedValue == "v.Acct" || ddlSearch.SelectedValue == "p.fBy" || ddlSearch.SelectedValue == "p.RequestedBy" || ddlSearch.SelectedValue == "p.Custom1" || ddlSearch.SelectedValue == "p.Custom2")
        {
            _objPO.SearchValue = txtSearch.Text;
        }

        if (ddlSearch.SelectedValue != "vs.Status")
        {
            string stdate = txtFromDate.Text + " 00:00:00";
            string enddate = txtToDate.Text + " 23:59:59";
            if (!string.IsNullOrEmpty(txtFromDate.Text))
                _objPO.StartDate = Convert.ToDateTime(stdate);
            if (!string.IsNullOrEmpty(txtToDate.Text))
                _objPO.EndDate = Convert.ToDateTime(enddate);
        }
        _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _objPO.ConnConfig = Session["config"].ToString();
        if (Session["CmpChkDefault"].ToString() == "1")
        {
            _objPO.EN = 1;
        }
        else
        {
            _objPO.EN = 0;
        }
        try
        {
            if (chkUserApprove.Checked == true)
                _objApprovePOStatus.UserID = _objPO.UserID;
            else
                _objApprovePOStatus.UserID = null;

            _objPO._ApprovePOStatus = _objApprovePOStatus;
            _dsPJ = _objBLBills.GetAllPOAjaxSearch(_objPO);
            // Status open/closed filter 
            DataTable filterdt = new DataTable();
            DataSet FilteredDs = new DataSet();
            if (isChecked)
            {
                lnkChk.Checked = true;
                FilteredDs = _dsPJ.Copy();
            }
            else
            {
                //filterdt = dt.Tables[0].Select("Status <> '1' AND Status <> '2' AND Status <> '5'").CopyToDataTable();
                if (_dsPJ.Tables[0].Rows.Count > 0)
                {
                    if (_objPO.SearchBy != "p.Status")
                    {
                        DataRow[] dr = _dsPJ.Tables[0].Select("Status <> '1' AND Status <> '2' AND Status <> '5'");
                        if (dr.Length > 0)
                        {
                            filterdt = dr.CopyToDataTable();
                            FilteredDs.Tables.Add(filterdt);
                        }
                        else
                        {
                            FilteredDs = _dsPJ.Clone();
                        }
                    }
                    else
                    {
                        FilteredDs = _dsPJ.Copy();
                    }
                }
                else
                {
                    FilteredDs = _dsPJ.Copy();

                }
            }
            if (ddlSearch.SelectedValue == "Projectnumber")
            {
                string searchTerm = txtSearch.Text;
                string column = ddlSearch.SelectedValue;
                DataTable dtFil = new DataTable();
                dtFil = FilteredDs.Tables[0];
                int searchindex = dtFil.Columns[column].Ordinal;
                var dr = dtFil.Rows.Cast<DataRow>().ToList().Select(x => x.ItemArray);


                var matchedData = (from items in dr select new { Id = items[0], SearchCol = Convert.ToString(items[searchindex]) }).ToList();
                if (matchedData != null)
                {

                    var found = matchedData.Where(x => x.Id == "").ToList();

                    if (column == "Projectnumber")
                    {
                        List<object> projectitemid = new List<object>();
                        var projectmatched = (from projectitem in matchedData select new { Id = projectitem.Id, projects = projectitem.SearchCol.Split(',') }).ToList();
                        var itemsfoundinPN = (from x in projectmatched where x.projects.Any(c => c == searchTerm) == true select new { Id = x.Id }).ToList();

                        found = (from x in matchedData join y in itemsfoundinPN on x.Id equals y.Id select x).ToList();

                    }
                    else
                        found = matchedData.Where(c => c.SearchCol.ToUpper().Contains(searchTerm.ToUpper())).ToList();

                    var itemsfound = (from x in dr join o in found on x[0] equals o.Id select x).ToList();

                    var llist = found.Select(x => x.Id).ToList();
                    DataTable dtitemsfound = new DataTable();
                    dtitemsfound = ToDataTable(llist, dtFil);
                    FilteredDs = new DataSet();
                    FilteredDs.Tables.Add(dtitemsfound);
                }
            }
            RadGrid_ManagePO.VirtualItemCount = FilteredDs.Tables[0].Rows.Count;
            RadGrid_ManagePO.DataSource = FilteredDs.Tables[0];
            // Session["PO"] = FilteredDs.Tables[0];
            Session["PO"] = GetFilteredDataSource();
            RadGrid_ManagePO.VirtualItemCount = GetFilteredDataSource().Rows.Count;
            UpdateSearchInfoSessions();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable GetFilteredDataSource()
    {
        DataTable DT = new DataTable();
        DataTable FilteredDT = new DataTable();
        string filterexpression = string.Empty;
        filterexpression = RadGrid_ManagePO.MasterTableView.FilterExpression;

        try
        {
            if (filterexpression != "")
            {
                DT = (DataTable)RadGrid_ManagePO.DataSource;
                FilteredDT = DT.AsEnumerable()
                .AsQueryable()
                .Where(filterexpression)
                .CopyToDataTable();
                return FilteredDT;
            }
            else
            {
                return (DataTable)RadGrid_ManagePO.DataSource;
            }
        }
        catch { return (DataTable)RadGrid_ManagePO.DataSource; }
    }
    public static DataTable ToDataTable(List<object> list, DataTable dt)
    {
        DataTable dataTable = dt.Clone();
        foreach (object dd in list)
        {
            String ID = Convert.ToString(dd);
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToString(dr["PO"]) == ID)
                {
                    dataTable.ImportRow(dr);
                }
            }

        }
        return dataTable;
    }
    #endregion

    #region Events
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_ManagePO.SelectedItems)
        {
            HiddenField hdnID = (HiddenField)di.FindControl("hdnID");
            Response.Redirect("addpo.aspx?id=" + hdnID.Value);
        }
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        bool Flag = false;
        try
        {
            foreach (GridDataItem di in RadGrid_ManagePO.SelectedItems)
            {
                Flag = true;
                HiddenField hdnID = (HiddenField)di.FindControl("hdnID");
                BL_Bills _objBLBills = new BL_Bills();
                PO _objPO = new PO();
                _objPO.ConnConfig = Session["config"].ToString();
                if (hdnID.Value != null)
                {
                    _objPO.POID = Convert.ToInt32(hdnID.Value);

                    DataSet ds = _objBLBills.GetPOById(_objPO);
                    if (ds.Tables.Count > 0)
                    {
                        //if (Convert.ToInt16(ds.Tables[0].Rows[0]["Status"]) == 0 || Convert.ToInt16(ds.Tables[0].Rows[0]["Status"]) == 3 || Convert.ToInt16(ds.Tables[0].Rows[0]["Status"]) == 4)
                        if (Convert.ToInt16(ds.Tables[0].Rows[0]["Status"]) == 0 )
                        {

                            if ((Convert.ToInt16(ds.Tables[0].Rows[0]["Status"]).Equals(3) || Convert.ToInt16(ds.Tables[0].Rows[0]["Status"]).Equals(4)))
                            {
                                _objPO.Status = 1;                          // if partially paid then status = 1 (closed)
                                double total = 0;
                                foreach (DataRow dr in ds.Tables[1].Rows)
                                {
                                    total = total + Convert.ToDouble(dr["Amount"]);
                                }
                                _objPO.Amount = total;
                            }
                            else
                            {
                                _objPO.Status = 2;                          // if partially paid then status = 2 (void)
                                _objPO.Amount = 0.0;
                            }
                            _objPO.fDesc = "Voided on " + DateTime.Now.ToString("MM/dd/yyyy") + " by " + Session["Username"].ToString() + " " + ds.Tables[0].Rows[0]["fDesc"].ToString();

                            //_objBLBills.UpdatePOStatusById(_objPO);
                            //_objBLBills.DeletePOById(_objPO);
                            //_objBLBills.UpdatePOBalance(_objPO);

                            _objBLBills.DeletePOById(_objPO);
                            //foreach (DataRow dr in ds.Tables[1].Rows)
                            //{
                            //    if (Convert.ToInt16(dr["JobID"]) > 0)
                            //    {
                            //        _objPO.jobID = Convert.ToInt16(dr["JobID"]);
                            //        _objBLBills.UpdateJobComm(_objPO);
                            //    }
                            //}
                            BindManagePO();
                            RadGrid_ManagePO.Rebind();
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "DeleteSuccessMesg();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "WarningMsg();", true);
                        }
                    }
                }
            }
            if (!Flag)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "closedMesg();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkAddPO_Click(object sender, EventArgs e)
    {
        Response.Redirect("addpo.aspx");
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        //#region Search Filter
        //String selectedValue = ddlSearch.SelectedValue;
        //Session["ddlSearch_ManagePO"] = selectedValue;

        //Session["from_ManagePO"] = txtFromDate.Text;
        //Session["end_ManagePO"] = txtToDate.Text;

        //if (selectedValue == "p.PO")
        //{
        //    Session["ddlSearch_Value_ManagePO"] = txtManagePOSearch.Text;
        //}
        //else if (selectedValue == "p.Status")
        //{
        //    Session["ddlSearch_Value_ManagePO"] = ddlStatus.SelectedValue;
        //}
        //else if (selectedValue == "vs.Status")
        //{
        //    Session["ddlSearch_Value_ManagePO"] = ddlApprovalStatus.SelectedValue;
        //}
        //else
        //{
        //    Session["ddlSearch_Value_ManagePO"] = txtSearch.Text;
        //}
        //if (hdnCssActive.Value == "CssActive")
        //{
        //    Session["lblManagePOActive"] = "1";
        //}
        //else
        //{
        //    Session["lblManagePOActive"] = "2";
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        //}
        //#endregion
        BindManagePO();
        RadGrid_ManagePO.Rebind();
    }
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        String selectedValue = ddlSearch.SelectedValue;
        ShowHideFilterSearch(selectedValue);
    }
    private void ShowHideFilterSearch(String selectedValue)
    {
        if (selectedValue == "p.Status")
        {
            ddlApprovalStatus.Visible = false;
            txtSearch.Visible = false;
            txtManagePOSearch.Visible = false;
            ddlStatus.Visible = true;
            ddlStatus.SelectedIndex = 0;
        }
        else if (selectedValue == "r.Name" || selectedValue == "Projectnumber" || selectedValue == "v.Acct" || selectedValue == "p.fBy" || selectedValue == "p.RequestedBy" || selectedValue == "p.Custom1" || selectedValue == "p.Custom2")
        {
            ddlApprovalStatus.Visible = false;
            txtSearch.Visible = true;
            txtManagePOSearch.Visible = false;
            ddlStatus.Visible = false;
        }
        else if (selectedValue == "vs.Status")
        {
            ddlApprovalStatus.SelectedIndex = 0;
            ddlApprovalStatus.Visible = true;
            txtSearch.Visible = false;
            txtManagePOSearch.Visible = false;
            ddlStatus.Visible = false;
        }
        else
        {
            ddlApprovalStatus.Visible = false;
            txtSearch.Visible = false;
            txtManagePOSearch.Visible = true;
            ddlStatus.Visible = false;
        }
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        //txtSearch.Text = string.Empty;
        //ddlSearch.SelectedIndex = 0;
        //txtFromDate.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
        //txtToDate.Text = DateTime.Now.ToShortDateString();
        //BindManagePO();
        //RadGrid_ManagePO.Rebind();

        ResetAllSearchForm();
        ClearGridFilters();
        BindManagePO();
        RadGrid_ManagePO.Rebind();
    }
    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        if (lnkChk.Checked)
        {
            isChecked = true;
        }
        else
        {
            isChecked = false;
        }
        BindManagePO();
        RadGrid_ManagePO.Rebind();
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_ManagePO.MasterTableView.FilterExpression != "" ||
            (RadGrid_ManagePO.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_ManagePO.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_ManagePO_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_ManagePO.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["ManagePO_FilterExpression"] != null && Convert.ToString(Session["ManagePO_FilterExpression"]) != "" && Session["ManagePO_Filters"] != null)
                {
                    RadGrid_ManagePO.MasterTableView.FilterExpression = Convert.ToString(Session["ManagePO_FilterExpression"]);
                    var filtersGet = Session["ManagePO_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_ManagePO.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["ManagePO_FilterExpression"] = null;
                Session["ManagePO_Filters"] = null;
            }
        }
        #endregion
        BindManagePO();
    }
    protected void RadGrid_ManagePO_ItemEvent(object sender, GridItemEventArgs e)
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
        foreach (GridDataItem gr in RadGrid_ManagePO.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkfDesc");
            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addpo.aspx?id=" + lblID.Text + "'";

            // Check Box hide show 
            Label lblAmount = (Label)gr.FindControl("lblAmount");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectSelectCheckBox");
            chkSelect.Visible = false;
            if (Convert.ToDecimal(hdnPOApprove.Value) == 1)
            {
                if (hdnPOApproveAmt.Value == "0")
                {
                    if (lblAmount.Text != null && hdnMinAmount.Value != null)
                    {
                        var amount = double.Parse(lblAmount.Text.Replace("$", string.Empty), NumberStyles.AllowParentheses |
                                                    NumberStyles.AllowThousands |
                                                    NumberStyles.AllowDecimalPoint |
                                                    NumberStyles.AllowTrailingSign |
                                                    NumberStyles.Float);
                        if (amount >= Convert.ToDouble(hdnMinAmount.Value) && amount <= Convert.ToDouble(hdnMaxAmount.Value))
                        {
                            chkSelect.Visible = true;
                        }
                    }
                }
                else if (hdnPOApproveAmt.Value == "1")
                {
                    if (lblAmount.Text != null && hdnMinAmount.Value != null)
                    {
                        if (Convert.ToDecimal(hdnMinAmount.Value) >= Convert.ToDecimal(lblAmount.Text.Replace("$", string.Empty)))
                        {
                            chkSelect.Visible = true;
                        }
                    }
                }
            }
        }
    }
    protected void RadGrid_ManagePO_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_ManagePO.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["ManagePO_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_ManagePO.MasterTableView.OwnerGrid.Columns)
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

            Session["ManagePO_Filters"] = filters;
        }
        else
        {
            Session["ManagePO_FilterExpression"] = null;
            Session["ManagePO_Filters"] = null;
        }
        #endregion

        if (!IsPostBack)
        {
            DataSet ds = new DataSet();
            _objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            _objPropUser.PageName = "ManagePO.aspx";
            _objPropUser.GridId = "RadGrid_ManagePO";
            ds = _objBLUser.GetGridUserSettings(_objPropUser);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //string columnSettings = "[{Name: \"BType\", Display: true, Width: 300},{Name: \"MatItem\", Display: false, Width: 300}]";
                var columnSettings = ds.Tables[0].Rows[0][0].ToString();
                var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

                var colIndex = 0;

                foreach (GridColumn column in RadGrid_ManagePO.MasterTableView.OwnerGrid.Columns)
                {
                    colIndex++;
                    var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (clSetting != null)
                    {
                        column.Display = clSetting.Display;
                        if (colIndex >= 3 && clSetting.Width != 0)
                            column.HeaderStyle.Width = clSetting.Width;

                        column.OrderIndex = clSetting.OrderIndex;
                    }
                }
                //RadGvTicketList.MasterTableView.Rebind();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showhidebutton", "ShowRestoreGridSettingsButton();", true);
            }
        }
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_ManagePO);
        RowSelect();


    }
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_ManagePO.MasterTableView.GetColumn("chkSelect").Visible = false;
        RadGrid_ManagePO.ExportSettings.FileName = "ManagePO";
        RadGrid_ManagePO.ExportSettings.IgnorePaging = true;
        RadGrid_ManagePO.ExportSettings.ExportOnlyData = true;
        RadGrid_ManagePO.ExportSettings.OpenInNewWindow = true;
        RadGrid_ManagePO.ExportSettings.HideStructureColumns = true;
        RadGrid_ManagePO.MasterTableView.UseAllDataFields = true;
        RadGrid_ManagePO.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_ManagePO.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_ManagePO_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 3;
        else
            currentItem = 4;
        if (e.Worksheet.Table.Rows.Count == RadGrid_ManagePO.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_ManagePO.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
    protected void RadGrid_ManagePO_ItemCreated(object sender, GridItemEventArgs e)
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

    #region PO Report

    protected void lnkPrintPOReport_Click(object sender, EventArgs e)
    {

        byte[] array = null;
        List<byte[]> lstbyte = new List<byte[]>();
        try
        {
            DataTable dt = new DataTable();

            DataTable dtPO = (DataTable)Session["PO"];
            if (dtPO.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["POReport"]) && ConfigurationManager.AppSettings["POReport"].ToLower().Contains(".rdlc"))
                {
                    foreach (DataRow drow in dtPO.Rows)
                    {
                        int PIID = Convert.ToInt32(drow["PO"]);
                        ReportViewer rvPO = new ReportViewer();

                        PrintPO(rvPO, PIID);

                        array = ExportReportToPDF("", rvPO);
                        lstbyte.Add(array);
                    }
                }
                else
                {
                    lstbyte = PrintPOReport();
                }

                byte[] allbyte = ManagePO.concatAndAddContent(lstbyte);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=POReport.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (allbyte.Length).ToString());
                Response.BinaryWrite(allbyte);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Record found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    protected void lnkPrintCustomPOReport_Click(object sender, EventArgs e)
    {
        byte[] array = null;
        List<byte[]> lstbyte = new List<byte[]>();
        try
        {
            DataTable dt = new DataTable();

            DataTable dtPO = (DataTable)Session["PO"];
            if (dtPO.Rows.Count > 0)
            {
                if (ConfigurationManager.AppSettings["CustomPOReport"].ToLower().Contains(".mrt"))
                {
                    foreach (DataRow drow in dtPO.Rows)
                    {
                        int PIID = Convert.ToInt32(drow["PO"]);
                        StiReport report = new StiReport();

                        PrintCustomPOReportMRT(report, PIID);

                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        array = stream.ToArray();

                        lstbyte.Add(array);
                    }
                }
                else
                {
                    foreach (DataRow drow in dtPO.Rows)
                    {
                        int PIID = Convert.ToInt32(drow["PO"]);
                        ReportViewer rvPO = new ReportViewer();

                        PrintCustomPOReportRDLC(rvPO, PIID);

                        array = ExportReportToPDF("", rvPO);
                        lstbyte.Add(array);
                    }
                }

                byte[] allbyte = concatAndAddContent(lstbyte);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + ConfigurationManager.AppSettings["CustomerName"].ToString() + "POReport.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (allbyte.Length).ToString());
                Response.BinaryWrite(allbyte);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Record found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private byte[] ExportReportToPDF(string reportName, ReportViewer ReportViewer1)
    {
        try
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            byte[] bytes = ReportViewer1.LocalReport.Render(
                "PDF", null, out mimeType, out encoding, out filenameExtension,
                 out streamids, out warnings);
            return bytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private List<byte[]> PrintPOReport()
    {
        // Export to PDF
        List<byte[]> poAsBytes = new List<byte[]>();
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/POReport.mrt");
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Innovative"))
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/POReport-Innovative.mrt");
            }
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToUpper() == ("Metro").ToString().ToUpper())
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/POReport-MEI.mrt");
            }
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToUpper() == ("allcity").ToString().ToUpper())
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/POReport-allcity.mrt");
            }

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            _objPO.ConnConfig = Session["config"].ToString();

            DataTable dtPO = (DataTable)Session["PO"];
            List<string> listPOs = dtPO.AsEnumerable().Select(x => x["PO"].ToString()).ToList();

            DataSet ds = _objBLBills.GetListPO(_objPO, string.Join(",", listPOs));

            string vendorAdd = ds.Tables[0].Rows[0]["VendorAddress"].ToString() + Environment.NewLine;
            vendorAdd += ds.Tables[0].Rows[0]["VendorCity"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorState"].ToString() + "  " + ds.Tables[0].Rows[0]["VendorZip"].ToString();
            ds.Tables[0].Rows[0]["Address"] = vendorAdd;

            DataSet dsC = new DataSet();
            _objPropUser.ConnConfig = Session["config"].ToString();
            dsC = _objBLUser.getControl(_objPropUser);

            report.RegData("CompanyDetails", dsC.Tables[0]);
            report.RegData("dsPO", ds.Tables[0]);
            report.RegData("POItems", ds.Tables[1]);

            report.Render();

            byte[] buffer1 = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(report, stream, settings);
            buffer1 = stream.ToArray();
            poAsBytes.Add(buffer1);

            return poAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr753", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return poAsBytes;
        }
    }

    private void PrintPO(ReportViewer rvPO, int PO_ID)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        _objPO.ConnConfig = Session["config"].ToString();
        _objPO.POID = Convert.ToInt32(PO_ID);
        #region Company Check
        _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            _objPO.EN = 1;
        }
        else
        {
            _objPO.EN = 0;
        }
        #endregion
        DataSet ds = _objBLBills.GetPOById(_objPO);

        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        if (Session["MSM"].ToString() != "TS")
        {
            dsC = objBL_User.getControl(objPropUser);
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
            dsC = objBL_User.getControlBranch(objPropUser);
        }


        DataSet dsTerm = _objBLBills.GetAddPOTerms(_objPO);
        if (dsTerm.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].Rows[0]["TC"] = dsTerm.Tables[0].Rows[0]["TermsConditions"].ToString();
        }

        string vendorAdd = ds.Tables[0].Rows[0]["VendorAddress"].ToString() + Environment.NewLine;
        vendorAdd += ds.Tables[0].Rows[0]["VendorCity"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorState"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorZip"].ToString();
        ds.Tables[0].Rows[0]["Address"] = vendorAdd;
        rvPO.LocalReport.DataSources.Clear();
        rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsPO", ds.Tables[0]));
        rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsPOItem", ds.Tables[1]));
        rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));
        string reportPath = "Reports/POReport.rdlc";

        rvPO.LocalReport.ReportPath = reportPath;
        rvPO.LocalReport.EnableExternalImages = true;

        List<ReportParameter> param1 = new List<ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
        rvPO.LocalReport.SetParameters(param1);
        rvPO.LocalReport.Refresh();
    }

    private void PrintCustomPOReportRDLC(ReportViewer report, int PO_ID)
    {
        try
        {
            User objPropUser = new User();
            BL_User objBL_User = new BL_User();

            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.POID = Convert.ToInt32(PO_ID);

            #region Company Check
            _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                _objPO.EN = 1;
            }
            else
            {
                _objPO.EN = 0;
            }
            #endregion

            DataSet ds = _objBLBills.GetPOById(_objPO);

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
            }

            DataSet dsTerm = _objBLBills.GetAddPOTerms(_objPO);
            if (dsTerm.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Rows[0]["TC"] = dsTerm.Tables[0].Rows[0]["TermsConditions"].ToString();
            }

            string vendorAdd = ds.Tables[0].Rows[0]["VendorAddress"].ToString() + Environment.NewLine;
            vendorAdd += ds.Tables[0].Rows[0]["VendorCity"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorState"].ToString() + " " + ds.Tables[0].Rows[0]["VendorZip"].ToString();
            ds.Tables[0].Rows[0]["Address"] = vendorAdd;

            report.LocalReport.DataSources.Clear();
            report.LocalReport.DataSources.Add(new ReportDataSource("dsPO", ds.Tables[0]));
            report.LocalReport.DataSources.Add(new ReportDataSource("dsMaddenPOItem", ds.Tables[1]));
            report.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));

            string reportPath = "Reports/" + ConfigurationManager.AppSettings["CustomerName"].ToString().Trim() + "POReport.rdlc";
            report.LocalReport.ReportPath = reportPath;
            report.ProcessingMode = ProcessingMode.Local;
            report.LocalReport.EnableExternalImages = true;

            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", "~/companylogo.ashx"));
            report.LocalReport.SetParameters(param1);

            report.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void PrintCustomPOReportMRT(StiReport report, int PO_ID)
    {
        try
        {
            User objPropUser = new User();
            BL_User objBL_User = new BL_User();

            string reportPathStimul = Server.MapPath($"StimulsoftReports/{ConfigurationManager.AppSettings["CustomPOReport"]}");
            report.Load(reportPathStimul);
            //report.Compile();

            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.POID = PO_ID;
            #region Company Check
            _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                _objPO.EN = 1;
            }
            else
            {
                _objPO.EN = 0;
            }
            #endregion 

            DataSet ds = _objBLBills.GetPOById(_objPO);

            DataSet dsTerm = _objBLBills.GetAddPOTerms(_objPO);
            if (dsTerm.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Rows[0]["TC"] = dsTerm.Tables[0].Rows[0]["TermsConditions"].ToString();
            }

            string vendorAdd = ds.Tables[0].Rows[0]["VendorAddress"].ToString() + Environment.NewLine;
            vendorAdd += ds.Tables[0].Rows[0]["VendorCity"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorState"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorZip"].ToString();
            ds.Tables[0].Rows[0]["Address"] = vendorAdd;

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
            }

            if (report.DataSources.Contains("Notes"))
            {
                var jobs = ds.Tables[1].DefaultView.ToTable(true, "JobID");
                if (jobs.Rows.Count > 0)
                {
                    objPropCustomer.ConnConfig = Session["config"].ToString();
                    objPropCustomer.job = Convert.ToInt32(jobs.Rows[0][0]);
                    var notes = bL_Customer.GetJobProject_Notes(objPropCustomer);

                    report.RegData("Notes", notes.Tables[0]);
                }
            }

            report.RegData("CompanyDetails", dsC.Tables[0]);
            report.RegData("dsPO", ds.Tables[0]);
            report.RegData("POItems", ds.Tables[1]);

            report.Render();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static byte[] concatAndAddContent(List<byte[]> pdfByteContent)
    {
        MemoryStream ms = new MemoryStream();
        Document doc = new Document();
        PdfSmartCopy copy = new PdfSmartCopy(doc, ms);

        doc.Open();

        //Loop through each byte array
        foreach (var p in pdfByteContent)
        {
            PdfReader reader = new PdfReader(p);
            int n = reader.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader.GetPageContent(i);
                if (red.Length < 1000)
                {
                    n = n - 1;
                }
            }
            for (int page = 0; page < n;)
            {
                copy.AddPage(copy.GetImportedPage(reader, ++page));
            }
        }
        doc.Close();
        //Return just before disposing
        return ms.ToArray();
    }
    #endregion

    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_ManagePO.SelectedItems)
        {
            HiddenField hdnID = (HiddenField)di.FindControl("hdnID");
            Response.Redirect("addpo.aspx?id=" + hdnID.Value + "&t=c");
        }
    }
    protected void lnkMail_Click(object sender, EventArgs e)
    {
        string POIDs = "";
        try
        {
            if (Confirm_Value.Value.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (GridDataItem di in RadGrid_ManagePO.Items)
                {
                    HiddenField hdnID = (HiddenField)di.FindControl("hdnID");
                    if (POIDs == "")
                        POIDs = hdnID.Value;
                    else
                    {
                        POIDs = POIDs + "," + hdnID.Value;
                    }
                }
                if (POIDs != "")
                {
                    byte[] buffer = null;

                    string reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order - Box Shadow.mrt";
                    StiReport report = new StiReport();
                    report.Load(reportPath);
                    //report.Compile();

                    _objApprovePOStatus.ConnConfig = Session["config"].ToString();
                    _objApprovePOStatus.POIDs = POIDs;
                    _objApprovePOStatus.UserID = Convert.ToInt32(Session["userid"]);

                    DataSet DS = _objBLBills.GetPODetailsForMailALL(_objApprovePOStatus);
                    DataTable dt_PO = DS.Tables[0];
                    DataTable dt_POItem = DS.Tables[1];
                    DataTable dt_Vender = DS.Tables[2];

                    //int total = 0;
                    int totalPOs = RadGrid_ManagePO.Items.Count;
                    int totalSentEmails = 0;
                    int totalSendErr = 0;
                    int totalNotSend = 0;
                    List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
                    List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
                    //List<string> ticketIdsSentEmail = new List<string>();
                    //List<string> ticketIdsError = new List<string>();
                    Tuple<int, string, string> emailSendError = null;
                    Tuple<int, string, string> emailGetSentError = null;
                    StringBuilder sbdSentError = new StringBuilder();
                    StringBuilder sbdGetSentError = new StringBuilder();

                    EmailLog emailLog = new EmailLog();
                    emailLog.ConnConfig = Session["config"].ToString();
                    emailLog.Function = "Email All: PO Approval Report";
                    emailLog.Screen = "PO";
                    emailLog.Username = Session["Username"].ToString();
                    emailLog.SessionNo = Guid.NewGuid().ToString();

                    BL_General bL_General = new BL_General();
                    EmailTemplate emailTemplate = new EmailTemplate();
                    emailTemplate.ConnConfig = Session["config"].ToString();
                    emailTemplate.Screen = "PO";
                    emailTemplate.FunctionName = "Email All: PO Approval Report";
                    string mailContent = bL_General.GetEmailTemplate(emailTemplate);

                    foreach (DataRow V_dr in dt_Vender.Rows)
                    {
                        int VenderID = Convert.ToInt32(V_dr["VenderID"]);
                        var toEmailAddress = V_dr["Email"] != null ? V_dr["Email"].ToString() : string.Empty;

                        foreach (DataRow PO_dr in dt_PO.Select("VenderID = " + VenderID))
                        {
                            //total++;
                            int POID = Convert.ToInt32(PO_dr["PO"]);
                            emailLog.Ref = POID;
                            if (!string.IsNullOrEmpty(toEmailAddress))
                            {
                                DataTable dt_POHead = dt_PO.Clone();
                                dt_POHead.ImportRow(PO_dr);

                                DataTable dt_PODetail = dt_POItem.Clone();
                                foreach (DataRow POIt_dr in dt_POItem.Select("PO = " + POID))
                                {
                                    dt_PODetail.Rows.Add(POIt_dr.ItemArray);
                                }
                                // Report Start
                                DataSet POHead = new DataSet();
                                DataTable hTable = dt_POHead;
                                hTable.TableName = "POHead";
                                POHead.Tables.Add(hTable);
                                POHead.DataSetName = "POHead";

                                DataSet POItem = new DataSet();
                                DataTable dTable = dt_PODetail;
                                dTable.TableName = "POItem";
                                POItem.Tables.Add(dTable);
                                POItem.DataSetName = "POItem";

                                report.RegData("AIAHeader", POHead);
                                report.RegData("AIADetails", POItem);
                                report.Dictionary.Synchronize();
                                report.Render();

                                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                                service.ExportTo(report, stream, settings);
                                buffer = stream.ToArray();
                                string FileName = "NewPOReport_" + POID + ".pdf";
                                //SendMail(V_dr, buffer, FileName);

                                Mail mail = new Mail();
                                //mail.From = "info@mom.com";
                                mail.From = WebBaseUtility.GetFromEmailAddress();
                                mail.To = toEmailAddress.Split(';', ',').OfType<string>().ToList();
                                mail.Title = "PO Details from MOM Application";
                                //mail.Text = "Please find attached PO";
                                if (string.IsNullOrEmpty(mailContent))
                                {
                                    mail.Text = "Please find attached PO";
                                }
                                else
                                {
                                    mail.Text = mailContent.Replace("{PO}", POID.ToString());
                                }
                                mail.FileName = FileName;
                                mail.attachmentBytes = buffer;
                                mail.DeleteFilesAfterSend = true;
                                mail.RequireAutentication = false;
                                // ES-33
                                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);

                                MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
                                emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog);
                                if (emailSendError != null && emailSendError.Item1 == 1)
                                {
                                    sbdSentError.Append(emailSendError.Item2);
                                    break;
                                }
                                else
                                {
                                    emailSendError = mail.Send(mimeMessage, true, emailLog);
                                    if (emailSendError != null)
                                    {
                                        if (emailSendError.Item1 == 1)
                                        {
                                            sbdSentError.Append(emailSendError.Item2);
                                            break;
                                        }
                                        else
                                        {
                                            sbdSentError.Append(emailSendError.Item2);
                                            mimeErrorMessages.Add(mimeMessage);
                                            totalSendErr++;
                                            //ticketIdsError.Add("PO #" + POID.ToString());
                                        }
                                    }
                                    else
                                    {
                                        mimeSentMessages.Add(mimeMessage);
                                        //ticketIdsSentEmail.Add("PO #" + POID.ToString());
                                    }
                                }
                            }
                            else
                            {
                                //ticketIdsError.Add("PO #" + POID.ToString());
                                totalSendErr++;
                                emailLog.To = string.Empty;
                                emailLog.Status = 0;
                                emailLog.UsrErrMessage = "Email address does not exist for this vendor";
                                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                                bL_EmailLog.AddEmailLog(emailLog);
                            }

                        }
                    }

                    totalSentEmails = mimeSentMessages.Count;
                    if (totalSentEmails > 0)
                    {
                        Mail mail = new Mail();
                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                        if (mail.TakeASentEmailCopy)
                        {
                            // reset ref 
                            emailLog.Ref = 0;
                            if (totalSentEmails >= 10)
                            {
                                List<List<MimeKit.MimeMessage>> lstTenMessages = new List<List<MimeKit.MimeMessage>>();
                                while (mimeSentMessages.Any())
                                {
                                    lstTenMessages.Add(mimeSentMessages.Take(11).ToList());
                                    mimeSentMessages = mimeSentMessages.Skip(11).ToList();
                                }

                                foreach (var lst in lstTenMessages)
                                {
                                    emailGetSentError = mail.GetSentItems(lst, true, emailLog);
                                }
                            }
                            else
                            {
                                emailGetSentError = mail.GetSentItems(mimeSentMessages, true, emailLog);
                            }
                        }
                    }

                    if (sbdSentError.Length > 0)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(sbdSentError.ToString());
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        if (emailGetSentError != null)
                        {
                            string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str1 + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }
                    else
                    {
                        if (totalSentEmails > 0)
                        {
                            var successfullMess = "There were " + totalSentEmails + " of "
                                + totalPOs.ToString() + " PO sent out successfully.";
                            if (totalSendErr > 0)
                            {
                                successfullMess += "<br>Total " + totalSendErr + " failed of "
                                    + totalPOs.ToString() + " POs could not be sent.";
                            }
                            totalNotSend = totalPOs - totalSentEmails - totalSendErr;
                            if (totalNotSend > 0)
                            {
                                successfullMess += "<br>Total " + totalNotSend + " of "
                                    + totalPOs.ToString() + " POs that their vendors haven&#39;t allowed PO&#39;s notification.";
                            }
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            if (emailGetSentError != null)
                            {
                                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }
                        else
                        {
                            string str = "There were no emails sent out.";
                            if (totalSendErr > 0)
                            {
                                str += "<br>Total " + totalSendErr + " failed of "
                                    + totalPOs.ToString() + " POs could not be sent.";
                            }
                            totalNotSend = totalPOs - totalSendErr;
                            if (totalNotSend > 0)
                            {
                                str += "<br>Total " + totalNotSend + " of "
                                    + totalPOs.ToString() + " POs that their vendors haven&#39;t allowed PO&#39;s notification.";
                            }
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }

                    RadGrid_gvLogs.Rebind();
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'No records are available to send email',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
        catch (Exception exp)
        {
            //string str = exp.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    //protected void SendMail(DataRow V_dr, byte[] buffer, string FileName)
    //{
    //    Mail mail = new Mail();
    //    //mail.From = "info@mom.com";
    //    mail.From = WebBaseUtility.GetFromEmailAddress();
    //    mail.To = V_dr["Email"].ToString().Split(';', ',').OfType<string>().ToList();
    //    mail.Title = "PO Details from MOM Application";
    //    mail.Text = "Please find atteched PO";
    //    mail.FileName = FileName;
    //    mail.attachmentBytes = buffer;
    //    mail.DeleteFilesAfterSend = true;
    //    mail.RequireAutentication = false;
    //    // ES-33
    //    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
    //    mail.Send();
    //}
    protected void lnkMailPOReport_Click(object sender, EventArgs e)
    {
        string POIDs = "";
        try
        {
            if (Confirm_Value.Value.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                DataTable dtPO = (DataTable)Session["PO"];
                if (dtPO.Rows.Count > 0)
                {
                    foreach (DataRow drow in dtPO.Rows)
                    {
                        string PIID = Convert.ToString(drow["PO"]);
                        if (POIDs == "")
                            POIDs = PIID;
                        else
                        {
                            POIDs = POIDs + "," + PIID;
                        }
                    }
                }
                if (POIDs != "")
                {
                    byte[] buffer = null;

                    _objApprovePOStatus.ConnConfig = Session["config"].ToString();
                    _objApprovePOStatus.POIDs = POIDs;
                    _objApprovePOStatus.UserID = Convert.ToInt32(Session["userid"]);

                    DataSet DS = _objBLBills.GetVenderDetailsForMailALL(_objApprovePOStatus);
                    DataTable dt_Vender = DS.Tables[0];
                    int totalPOs = dtPO.Rows.Count;
                    //int total = 0;

                    if (dt_Vender != null && dt_Vender.Rows.Count > 0)
                    {
                        //int total = 0;
                        int totalSentEmails = 0;
                        int totalSendErr = 0;
                        int totalNotSend = 0;
                        List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
                        List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
                        //List<string> ticketIdsSentEmail = new List<string>();
                        //List<string> ticketIdsError = new List<string>();
                        Tuple<int, string, string> emailSendError = null;
                        Tuple<int, string, string> emailGetSentError = null;
                        StringBuilder sbdSentError = new StringBuilder();
                        StringBuilder sbdGetSentError = new StringBuilder();

                        EmailLog emailLog = new EmailLog();
                        emailLog.ConnConfig = Session["config"].ToString();
                        emailLog.Function = "Email All: PO Report";
                        emailLog.Screen = "PO";
                        emailLog.Username = Session["Username"].ToString();
                        emailLog.SessionNo = Guid.NewGuid().ToString();

                        BL_General bL_General = new BL_General();
                        EmailTemplate emailTemplate = new EmailTemplate();
                        emailTemplate.ConnConfig = Session["config"].ToString();
                        emailTemplate.Screen = "PO";
                        emailTemplate.FunctionName = "Email All: PO Report";
                        string mailContent = bL_General.GetEmailTemplate(emailTemplate);

                        foreach (DataRow V_dr in dt_Vender.Rows)
                        {
                            int VenderID = Convert.ToInt32(V_dr["VenderID"]);
                            var toEmailAddress = V_dr["Email"] != null ? V_dr["Email"].ToString() : string.Empty;

                            foreach (DataRow drow in dtPO.Select("Vendor = " + VenderID))
                            {
                                //total++;
                                int PIID = Convert.ToInt32(drow["PO"]);
                                emailLog.Ref = PIID;
                                if (!string.IsNullOrEmpty(toEmailAddress))
                                {
                                    ReportViewer rvPO = new ReportViewer();
                                    PrintPO(rvPO, PIID);
                                    buffer = ExportReportToPDF("", rvPO);
                                    string FileName = "POReport_" + PIID + ".pdf"; ;
                                    //SendMail(V_dr, buffer, FileName);

                                    Mail mail = new Mail();
                                    //mail.From = "info@mom.com";
                                    mail.From = WebBaseUtility.GetFromEmailAddress();
                                    mail.To = toEmailAddress.Split(';', ',').OfType<string>().ToList();
                                    mail.Title = "PO Details from MOM Application";

                                    if (string.IsNullOrEmpty(mailContent))
                                    {
                                        mail.Text = "Please find attached PO";
                                    }
                                    else
                                    {
                                        mail.Text = mailContent.Replace("{PO}", PIID.ToString());
                                    }
                                    mail.FileName = FileName;
                                    mail.attachmentBytes = buffer;
                                    mail.DeleteFilesAfterSend = true;
                                    mail.RequireAutentication = false;
                                    // ES-33
                                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);

                                    MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
                                    emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog);
                                    if (emailSendError != null && emailSendError.Item1 == 1)
                                    {
                                        sbdSentError.Append(emailSendError.Item2);
                                        break;
                                    }
                                    else
                                    {
                                        emailSendError = mail.Send(mimeMessage, true, emailLog);
                                        if (emailSendError != null)
                                        {
                                            if (emailSendError.Item1 == 1)
                                            {
                                                sbdSentError.Append(emailSendError.Item2);
                                                break;
                                            }
                                            else
                                            {
                                                sbdSentError.Append(emailSendError.Item2);
                                                mimeErrorMessages.Add(mimeMessage);
                                                //ticketIdsError.Add("PO #" + PIID.ToString());
                                                totalSendErr++;
                                            }
                                        }
                                        else
                                        {
                                            mimeSentMessages.Add(mimeMessage);
                                            //ticketIdsSentEmail.Add("PO #" + PIID.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    //ticketIdsError.Add("PO #" + PIID.ToString());
                                    totalSendErr++;
                                    emailLog.To = string.Empty;
                                    emailLog.Status = 0;
                                    emailLog.UsrErrMessage = "Email address does not exist for this vendor";
                                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
                                    bL_EmailLog.AddEmailLog(emailLog);
                                }
                            }

                        }

                        totalSentEmails = mimeSentMessages.Count;
                        if (totalSentEmails > 0)
                        {
                            Mail mail = new Mail();
                            WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                            if (mail.TakeASentEmailCopy)
                            {
                                // reset ref 
                                emailLog.Ref = 0;
                                if (totalSentEmails >= 10)
                                {
                                    List<List<MimeKit.MimeMessage>> lstTenMessages = new List<List<MimeKit.MimeMessage>>();
                                    while (mimeSentMessages.Any())
                                    {
                                        lstTenMessages.Add(mimeSentMessages.Take(11).ToList());
                                        mimeSentMessages = mimeSentMessages.Skip(11).ToList();
                                    }

                                    foreach (var lst in lstTenMessages)
                                    {
                                        emailGetSentError = mail.GetSentItems(lst, true, emailLog);
                                    }
                                }
                                else
                                {
                                    emailGetSentError = mail.GetSentItems(mimeSentMessages, true, emailLog);
                                }
                            }
                        }

                        if (sbdSentError.Length > 0)
                        {
                            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(sbdSentError.ToString());
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            if (emailGetSentError != null)
                            {
                                string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str1 + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }
                        else
                        {
                            if (totalSentEmails > 0)
                            {
                                var successfullMess = "There were " + totalSentEmails + " of "
                                    + totalPOs.ToString() + " POs sent out successfully.";
                                //if (ticketIdsError.Count > 0)
                                if (totalSendErr > 0)
                                {
                                    successfullMess += "<br>Total " + totalSendErr + " failed of "
                                        + totalPOs.ToString() + " POs could not be sent.";
                                }
                                totalNotSend = totalPOs - totalSentEmails - totalSendErr;
                                if (totalNotSend > 0)
                                {
                                    successfullMess += "<br>Total " + totalNotSend + " of "
                                        + totalPOs.ToString() + " POs that their vendors haven&#39;t allowed PO&#39;s notification.";
                                }
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                if (emailGetSentError != null)
                                {
                                    string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                            }
                            else
                            {
                                string str = "There were no emails sent out.";
                                if (totalSendErr > 0)
                                {
                                    str += "<br>Total " + totalSendErr + " failed of "
                                        + totalPOs.ToString() + " POs could not be sent.";
                                }
                                totalNotSend = totalPOs - totalSendErr;
                                if (totalNotSend > 0)
                                {
                                    str += "<br>Total " + totalNotSend + " of "
                                        + totalPOs.ToString() + " POs that their vendors haven&#39;t allowed PO&#39;s notification.";
                                }
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }

                        RadGrid_gvLogs.Rebind();
                    }
                    else
                    {
                        string str = "There were no emails sent out.";

                        str += "<br>Total " + totalPOs.ToString() + " of "
                            + totalPOs.ToString() + " POs that their vendors haven&#39;t allowed PO&#39;s notification.";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Error: No records are available to send email',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
        catch (Exception exp)
        {
            //string str = exp.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkMailCustomPOReport_Click(object sender, EventArgs e)
    {
        string POIDs = "";
        try
        {
            if (Confirm_Value.Value.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                DataTable dtPO = (DataTable)Session["PO"];
                if (dtPO.Rows.Count > 0)
                {
                    foreach (DataRow drow in dtPO.Rows)
                    {
                        string PIID = Convert.ToString(drow["PO"]);
                        if (POIDs == "")
                            POIDs = PIID;
                        else
                        {
                            POIDs = POIDs + "," + PIID;
                        }
                    }
                }
                if (POIDs != "")
                {
                    byte[] buffer = null;

                    _objApprovePOStatus.ConnConfig = Session["config"].ToString();
                    _objApprovePOStatus.POIDs = POIDs;
                    _objApprovePOStatus.UserID = Convert.ToInt32(Session["userid"]);

                    DataSet DS = _objBLBills.GetVenderDetailsForMailALL(_objApprovePOStatus);
                    DataTable dt_Vender = DS.Tables[0];
                    int totalPOs = dtPO.Rows.Count;
                    //int total = 0;
                    int totalSentEmails = 0;
                    int totalSendErr = 0;
                    int totalNotSend = 0;
                    List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
                    List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
                    //List<string> ticketIdsSentEmail = new List<string>();
                    //List<string> ticketIdsError = new List<string>();
                    Tuple<int, string, string> emailSendError = null;
                    Tuple<int, string, string> emailGetSentError = null;
                    StringBuilder sbdSentError = new StringBuilder();
                    StringBuilder sbdGetSentError = new StringBuilder();

                    EmailLog emailLog = new EmailLog();
                    emailLog.ConnConfig = Session["config"].ToString();
                    emailLog.Function = "Email All: Custom PO Report";
                    emailLog.Screen = "PO";
                    emailLog.Username = Session["Username"].ToString();
                    emailLog.SessionNo = Guid.NewGuid().ToString();

                    BL_General bL_General = new BL_General();
                    EmailTemplate emailTemplate = new EmailTemplate();
                    emailTemplate.ConnConfig = Session["config"].ToString();
                    emailTemplate.Screen = "PO";
                    emailTemplate.FunctionName = "Email All: Custom PO Report";
                    string mailContent = bL_General.GetEmailTemplate(emailTemplate);

                    foreach (DataRow V_dr in dt_Vender.Rows)
                    {
                        int VenderID = Convert.ToInt32(V_dr["VenderID"]);
                        var toEmailAddress = V_dr["Email"] != null ? V_dr["Email"].ToString() : string.Empty;

                        foreach (DataRow drow in dtPO.Select("Vendor = " + VenderID))
                        {
                            //total++;
                            int PIID = Convert.ToInt32(drow["PO"]);
                            emailLog.Ref = PIID;
                            if (!string.IsNullOrEmpty(toEmailAddress))
                            {
                                if (ConfigurationManager.AppSettings["CustomPOReport"].ToLower().Contains(".mrt"))
                                {
                                    StiReport report = new StiReport();
                                    PrintCustomPOReportMRT(report, PIID);

                                    var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                                    var service = new Stimulsoft.Report.Export.StiPdfExportService();
                                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                                    service.ExportTo(report, stream, settings);
                                    buffer = stream.ToArray();
                                }
                                else
                                {
                                    ReportViewer rvPO = new ReportViewer();
                                    PrintCustomPOReportRDLC(rvPO, PIID);
                                    buffer = ExportReportToPDF("", rvPO);
                                }

                                string FileName = ConfigurationManager.AppSettings["CustomerName"].ToString().Trim() + "POReport_" + PIID + ".pdf"; ;
                                //SendMail(V_dr, buffer, FileName);

                                Mail mail = new Mail();
                                //mail.From = "info@mom.com";
                                mail.From = WebBaseUtility.GetFromEmailAddress();
                                mail.To = toEmailAddress.Split(';', ',').OfType<string>().ToList();
                                mail.Title = "PO Details from MOM Application";
                                //mail.Text = "Please find attached PO";
                                if (string.IsNullOrEmpty(mailContent))
                                {
                                    mail.Text = "Please find attached PO";
                                }
                                else
                                {
                                    mail.Text = mailContent.Replace("{PO}", PIID.ToString());
                                }
                                mail.FileName = FileName;
                                mail.attachmentBytes = buffer;
                                mail.DeleteFilesAfterSend = true;
                                mail.RequireAutentication = false;
                                // ES-33
                                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);

                                MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
                                emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog);
                                if (emailSendError != null && emailSendError.Item1 == 1)
                                {
                                    sbdSentError.Append(emailSendError.Item2);
                                    break;
                                }
                                else
                                {
                                    emailSendError = mail.Send(mimeMessage, true, emailLog);
                                    if (emailSendError != null)
                                    {
                                        if (emailSendError.Item1 == 1)
                                        {
                                            sbdSentError.Append(emailSendError.Item2);
                                            break;
                                        }
                                        else
                                        {
                                            sbdSentError.Append(emailSendError.Item2);
                                            mimeErrorMessages.Add(mimeMessage);
                                            //ticketIdsError.Add("PO #" + PIID.ToString());
                                            totalSendErr++;
                                        }
                                    }
                                    else
                                    {
                                        mimeSentMessages.Add(mimeMessage);
                                        //ticketIdsSentEmail.Add("PO #" + PIID.ToString());
                                    }
                                }
                            }
                            else
                            {
                                //ticketIdsError.Add("PO #" + PIID.ToString());
                                totalSendErr++;
                                emailLog.To = string.Empty;
                                emailLog.Status = 0;
                                emailLog.UsrErrMessage = "Email address does not exist for this vendor";
                                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                                bL_EmailLog.AddEmailLog(emailLog);
                            }
                        }
                    }

                    totalSentEmails = mimeSentMessages.Count;
                    if (totalSentEmails > 0)
                    {
                        Mail mail = new Mail();
                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                        if (mail.TakeASentEmailCopy)
                        {
                            // reset ref 
                            emailLog.Ref = 0;
                            if (totalSentEmails >= 10)
                            {
                                List<List<MimeKit.MimeMessage>> lstTenMessages = new List<List<MimeKit.MimeMessage>>();
                                while (mimeSentMessages.Any())
                                {
                                    lstTenMessages.Add(mimeSentMessages.Take(11).ToList());
                                    mimeSentMessages = mimeSentMessages.Skip(11).ToList();
                                }

                                foreach (var lst in lstTenMessages)
                                {
                                    emailGetSentError = mail.GetSentItems(lst, true, emailLog);
                                }
                            }
                            else
                            {
                                emailGetSentError = mail.GetSentItems(mimeSentMessages, true, emailLog);
                            }
                        }
                    }

                    if (sbdSentError.Length > 0)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(sbdSentError.ToString());
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        if (emailGetSentError != null)
                        {
                            string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str1 + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }
                    else
                    {
                        if (totalSentEmails > 0)
                        {
                            var successfullMess = "There were " + totalSentEmails + " of "
                                + totalPOs.ToString() + " POs sent out successfully.";
                            if (totalSendErr > 0)
                            {
                                successfullMess += "<br>Total " + totalSendErr + " failed of "
                                    + totalPOs.ToString() + " POs could not be sent.";
                            }
                            totalNotSend = totalPOs - totalSentEmails - totalSendErr;
                            if (totalNotSend > 0)
                            {
                                successfullMess += "<br>Total " + totalNotSend + " of "
                                    + totalPOs.ToString() + " POs that their vendors haven&#39;t allowed PO&#39;s notification.";
                            }

                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            if (emailGetSentError != null)
                            {
                                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }
                        else
                        {
                            string str = "There were no emails sent out.";
                            if (totalSendErr > 0)
                            {
                                str += "<br>Total " + totalSendErr + " failed of "
                                    + totalPOs.ToString() + " POs could not be sent.";
                            }
                            totalNotSend = totalPOs - totalSentEmails - totalSendErr;
                            if (totalNotSend > 0)
                            {
                                str += "<br>Total " + totalNotSend + " of "
                                    + totalPOs.ToString() + " POs that their vendors haven&#39;t allowed PO&#39;s notification.";
                            }

                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }

                    RadGrid_gvLogs.Rebind();
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'No records are available to send email',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
        catch (Exception exp)
        {
            //string str = exp.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        //ResetFormControlValues(this);

        //BindManagePO();
        //foreach (GridColumn column in RadGrid_ManagePO.MasterTableView.OwnerGrid.Columns)
        //{
        //    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
        //    column.CurrentFilterValue = string.Empty;
        //}
        //RadGrid_ManagePO.MasterTableView.FilterExpression = string.Empty;
        //RadGrid_ManagePO.Rebind();

        ClearGridFilters();
        // reset search values
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = "";
        txtManagePOSearch.Text = "";

        ddlSearch_SelectedIndexChanged(sender, e);
        RadGrid_ManagePO.Rebind();
    }

    protected void btnDownloadPOTemplate_Click(object sender, CommandEventArgs e)
    {
        try
        {
            var argument = e.CommandArgument.ToString();
            if (!string.IsNullOrEmpty(argument))
            {
                int poID = 0;
                if (RadGrid_ManagePO.Items.Count == 1)
                {
                    GridDataItem item = RadGrid_ManagePO.Items[0];
                    poID = Convert.ToInt32(item.GetDataKeyValue("PO"));
                }
                else
                {
                    foreach (GridDataItem item in RadGrid_ManagePO.SelectedItems)
                    {
                        poID = Convert.ToInt32(item.GetDataKeyValue("PO"));
                    }
                }

                if (poID != 0)
                {
                    StiWebViewer rvTemplate = new StiWebViewer();
                    List<byte[]> invoicesToPrint = PrintTemplate(rvTemplate, string.Format("{0}.mrt", argument), poID);

                    string filePath = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", string.Format("{0}.pdf", argument));

                    if (invoicesToPrint != null)
                    {
                        byte[] buffer1 = null;

                        buffer1 = ConcatAndAddContent(invoicesToPrint);

                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            fs.Write(buffer1, 0, buffer1.Length);
                            fs.Close();
                        }
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.pdf", argument));
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Length", (buffer1.Length).ToString());
                        Response.BinaryWrite(buffer1);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDownloafPOTemplate", "noty({text: 'Please select any PO to download.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDownloafPOTemplate", "noty({text: 'Please select any template to download.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception exp)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
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
    }

    private List<string> GetListPOTemplate()
    {
        List<string> listFile = new List<string>();
        string appDirectory = HttpContext.Current.Server.MapPath(string.Empty);

        DirectoryInfo dir = new DirectoryInfo(string.Format("{0}/Templates/POTemplates", appDirectory));

        if (dir.Exists)
        {
            FileInfo[] Files = dir.GetFiles("*.*");
            foreach (FileInfo file in Files)
            {
                listFile.Add(Path.GetFileNameWithoutExtension(file.Name));
            }
        }

        return listFile;
    }

    private List<byte[]> PrintTemplate(StiWebViewer rvTemplate, string templateName, int poID)
    {
        // Export to PDF
        List<byte[]> templateAsBytes = new List<byte[]>();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();

            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath(string.Format("Templates/POTemplates/{0}", templateName));
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            DataSet companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

            report.RegData("CompanyDetails", companyInfo.Tables[0]);

            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.POID = poID;

            #region Company Check
            _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                _objPO.EN = 1;
            }
            else
            {
                _objPO.EN = 0;
            }
            #endregion

            DataSet dsPO = _objBLBills.GetPOById(_objPO);

            if (dsPO != null)
            {
                report.RegData("POInfo", dsPO.Tables[0]);
                report.RegData("POItems", dsPO.Tables[1]);
            }

            report.Dictionary.Synchronize();
            report.Render();
            rvTemplate.Report = report;
            byte[] buffer1 = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(rvTemplate.Report, stream, settings);
            buffer1 = stream.ToArray();
            templateAsBytes.Add(buffer1);

            return templateAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr753", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return templateAsBytes;
        }
    }

    private DataTable BuildCompanyDetailsTable()
    {
        DataTable companyDetailsTable = new DataTable();
        companyDetailsTable.Columns.Add("CompanyAddress");
        companyDetailsTable.Columns.Add("CompanyName");
        companyDetailsTable.Columns.Add("ContactNo");
        companyDetailsTable.Columns.Add("Email");
        companyDetailsTable.Columns.Add("LogoURL");
        companyDetailsTable.Columns.Add("City");
        companyDetailsTable.Columns.Add("State");
        companyDetailsTable.Columns.Add("Zip");
        companyDetailsTable.Columns.Add("Fax");
        companyDetailsTable.Columns.Add("Phone");

        return companyDetailsTable;
    }

    public static byte[] ConcatAndAddContent(List<byte[]> pdfByteContent)
    {
        MemoryStream ms = new MemoryStream();
        iTextSharp.text.Document doc = new iTextSharp.text.Document();
        PdfSmartCopy copy = new PdfSmartCopy(doc, ms);

        doc.Open();

        //Loop through each byte array
        foreach (var p in pdfByteContent)
        {
            PdfReader reader = new PdfReader(p);
            int n = reader.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader.GetPageContent(i);
                if (red.Length < 1000)
                {
                    n = n - 1;
                }
            }
            for (int page = 0; page < n;)
            {
                copy.AddPage(copy.GetImportedPage(reader, ++page));
            }
        }
        doc.Close();
        //Return just before disposing
        return ms.ToArray();
    }
    protected void RadGrid_gvLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        DataSet dsLog = new DataSet();
        EmailLog emailLog = new EmailLog();
        emailLog.Screen = "PO";
        emailLog.ConnConfig = Session["config"].ToString();
        BL_EmailLog bL_EmailLog = new BL_EmailLog();
        dsLog = bL_EmailLog.GetEmailLogs(emailLog);
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
    bool isGroupLog = false;

    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return RadGrid_gvLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            RadGrid_gvLogs.MasterTableView.SortExpressions.Count > 0;
    }

    private void BindSearchFilters()
    {
        Dictionary<string, string> listsearchitems = new Dictionary<string, string>();
        listsearchitems.Add("", "Select");
        listsearchitems.Add("p.PO", "PO#");
        listsearchitems.Add("Projectnumber", "Project number");
        listsearchitems.Add("v.Acct", "VendorID #");
        listsearchitems.Add("r.Name", "Vendor Name");
        listsearchitems.Add("p.Status", "Status");
        listsearchitems.Add("vs.Status", "Approval Status");
        listsearchitems.Add("p.fBy", "Created by");
        listsearchitems.Add("p.RequestedBy", "Requested by");

        DataSet ds = new DataSet();
        General objPropGeneral = new General();
        BL_General objBL_General = new BL_General();
        String cusLabel = "";
        for (int i = 1; i < 3; i++)
        {
            objPropGeneral.CustomName = "PO" + Convert.ToString(i);
            objPropGeneral.ConnConfig = Session["config"].ToString();
            ds = objBL_General.getCustomFields(objPropGeneral);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cusLabel = ds.Tables[0].Rows[0]["label"].ToString() == "" ? "Custom " + i : ds.Tables[0].Rows[0]["label"].ToString();
                listsearchitems.Add("p.Custom" + i, cusLabel);
            }
        }

        ddlSearch.DataSource = listsearchitems;
        ddlSearch.DataTextField = "Value";
        ddlSearch.DataValueField = "Key";
        ddlSearch.DataBind();

    }

    private void ResetAllSearchForm()
    {
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
    }

    // Clear grid filter
    private void ClearGridFilters()
    {
        if (Session["ManagePO_FilterExpression"] != null && Convert.ToString(Session["ManagePO_FilterExpression"]) != "" && Session["ManagePO_Filters"] != null)
        {
            foreach (GridColumn column in RadGrid_ManagePO.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            RadGrid_ManagePO.MasterTableView.FilterExpression = string.Empty;
        }


        Session["ManagePO_FilterExpression"] = null;
        Session["ManagePO_Filters"] = null;
    }

    private void UpdateSearchInfoSessions()
    {
        #region Search Filter
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_ManagePO"] = selectedValue;

        Session["from_ManagePO"] = txtFromDate.Text;
        Session["end_ManagePO"] = txtToDate.Text;

        if (selectedValue == "p.PO")
        {
            Session["ddlSearch_Value_ManagePO"] = txtManagePOSearch.Text;
        }
        else if (selectedValue == "p.Status")
        {
            Session["ddlSearch_Value_ManagePO"] = ddlStatus.SelectedValue;
        }
        else if (selectedValue == "vs.Status")
        {
            Session["ddlSearch_Value_ManagePO"] = ddlApprovalStatus.SelectedValue;
        }
        else
        {
            Session["ddlSearch_Value_ManagePO"] = txtSearch.Text;
        }
        if (hdnCssActive.Value == "CssActive")
        {
            Session["lblManagePOActive"] = "1";
        }
        else
        {
            Session["lblManagePOActive"] = "2";
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }
        #endregion
    }
    protected void lnkRestoreGridSettings_Click(object sender, EventArgs e)
    {
        #region Grid user settings
        var columnSettings = string.Empty;
        _objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        _objPropUser.PageName = "ManagePO.aspx";
        _objPropUser.GridId = "RadGrid_ManagePO";

        var ds = _objBLUser.DeleteUserGridCustomSettings(_objPropUser);


        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            var colIndex = 0;

            foreach (GridColumn column in RadGrid_ManagePO.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (clSetting != null)
                {
                    column.Display = clSetting.Display;
                    if (colIndex >= 3 && clSetting.Width != 0)
                        column.HeaderStyle.Width = clSetting.Width;

                    column.OrderIndex = clSetting.OrderIndex;
                }
            }
            RadGrid_ManagePO.MasterTableView.Rebind();
        }
        else
        {
            //var arrColumnOrder = new string[3]{ "ReviewCheck", "Comp", "" };
            var colIndex = 0;
            foreach (GridColumn column in RadGrid_ManagePO.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                column.Display = true;
                //column.OrderIndex =
                //if(colIndex >= 3)
                //{
                //    column.HeaderStyle.Reset();
                //}
            }
            RadGrid_ManagePO.MasterTableView.SortExpressions.Clear();
            RadGrid_ManagePO.MasterTableView.GroupByExpressions.Clear();
            RadGrid_ManagePO.EditIndexes.Clear();
            RadGrid_ManagePO.Rebind();
        }
        #endregion
    }
    protected void lnkSaveGridSettings_Click(object sender, EventArgs e)
    {
        #region Grid user settings
        var columnSettings = GetGridColumnSettings();
        _objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        _objPropUser.PageName = "ManagePO.aspx";
        _objPropUser.GridId = "RadGrid_ManagePO";

        _objBLUser.UpdateUserGridCustomSettings(_objPropUser, columnSettings);
        #endregion
    }
    private string GetGridColumnSettings()
    {
        var columnSettings = string.Empty;

        List<ColumnSettings> lstColSetts = new List<ColumnSettings>();
        foreach (GridColumn column in RadGrid_ManagePO.MasterTableView.OwnerGrid.Columns)
        {
            var colSett = new ColumnSettings();
            colSett.Name = column.UniqueName;
            colSett.Display = column.Display;
            colSett.Width = (int)column.HeaderStyle.Width.Value;
            colSett.OrderIndex = column.OrderIndex;
            lstColSetts.Add(colSett);
        }

        columnSettings = Newtonsoft.Json.JsonConvert.SerializeObject(lstColSetts);
        return columnSettings;
    }

    private string GetDefaultGridColumnSettingsFromDb()
    {
        var columnSettings = string.Empty;
        _objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objPropUser.PageName = "ManagePO.aspx";
        _objPropUser.GridId = "RadGrid_ManagePO";

        var ds = _objBLUser.GetDefaultGridCustomSettings(_objPropUser);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
        }

        return columnSettings;
    }
}