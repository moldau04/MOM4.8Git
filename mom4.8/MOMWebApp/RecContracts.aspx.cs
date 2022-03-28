using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using System.Configuration;
using Telerik.Web.UI.GridExcelBuilder;
using System.Linq;
using System.IO;
using System.Web;
using Stimulsoft.Report;
using System.Text;
using System.Threading;

public partial class RecContracts : System.Web.UI.Page
{
    #region "Variables"
    BL_Contracts objBL_Contracts = new BL_Contracts();

    Contracts objProp_Contracts = new Contracts();

    BL_User objBL_User = new BL_User();

    BusinessEntity.User objProp_User = new BusinessEntity.User();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    BL_Report objBL_Report = new BL_Report();

    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();

    BL_Company objBL_Company = new BL_Company();

    private const string ASCENDING = " ASC";

    private const string DESCENDING = " DESC";

    private static readonly string CookieName = "RecContracts";
    private bool IsGridPageIndexChanged = false;
    private bool IsEscalationGridPageIndexChanged = false;
    protected void Page_Init(object sender, EventArgs e)
    {
        //RadPersistenceRecContracts.StorageProviderKey = CookieName;
        //RadPersistenceRecContracts.StorageProvider = new CookieStorageProvider(CookieName);
    }
    #endregion

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
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

            SetDefaultWorker();
            FillRoute();
            FillServiceType();
            FillCompany();
            SetDefaultValueForEmail();

            if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("transel"))
            {
                lnkMonthlyRecurringHoursCategories.Visible = true;
            }

            if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("gable"))
            {
                lnkEquipmentContractByCustomer.Visible = true;
            }

            if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("accredited"))
            {
                lnkServiceSalesCheckUpReport.Visible = true;
            }

            if (Session["rt"] != null)
            { hdnTab.Value = "2"; }
            else
            { hdnTab.Value = "0";
                Session["rt"] = null;
            }

            if (Request.QueryString["fil"] != null && Request.QueryString["fil"].ToString() == "c")
            {
                IsGridPageIndexChanged = true;
                IsEscalationGridPageIndexChanged = true;
                FillFilter();
            }
            else
            {
                Session.Remove("RecContracts_FilterExpression");
                Session.Remove("RecContracts_Filters");
                Session.Remove("FilterRecContr");
                Session.Remove("Escalation_FilterExpression");
                Session.Remove("Escalation_Filters");
                Session.Remove("FilterRecContr");
            }

            txtEscDate.Text = DateTime.Now.ToShortDateString();
            txtNextEscdate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();


            //GetEscalationContracts();
            //ConvertToJSON();
        }
        ConvertToJSON();
        Permission();
        CompanyPermission();
        HighlightSideMenu("cntractsMgr", "lnkContractsMenu", "recurMgrSub");
    }
    #endregion

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
            //Response.Redirect("addreccontract.aspx?uid=" + Session["userid"].ToString());
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

            /// RCmodulePermission ///////////////////------->

            string RCmodulePermission = ds.Rows[0]["RCmodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["RCmodulePermission"].ToString();

            if (RCmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// BillPay ///////////////////------->

            string ProcessRCPermission = ds.Rows[0]["ProcessRCPermission"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["ProcessRCPermission"].ToString();
            string ADD = ProcessRCPermission.Length < 1 ? "Y" : ProcessRCPermission.Substring(0, 1);
            string Edit = ProcessRCPermission.Length < 2 ? "Y" : ProcessRCPermission.Substring(1, 1);
            string Delete = ProcessRCPermission.Length < 2 ? "Y" : ProcessRCPermission.Substring(2, 1);
            string View = ProcessRCPermission.Length < 4 ? "Y" : ProcessRCPermission.Substring(3, 1);
            if (ADD == "N")
            {

                lnkAddnew.Visible = false;
            }
            if (Edit == "N")
            {
                btnEdit.Visible = false;
                btnCopy.Visible = false;
            }
            if (Delete == "N")
            {
                btnDelete.Visible = false;

            }
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            string RCRenewEscalatePermission = ds.Rows[0]["RCRenewEscalatePermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["RCRenewEscalatePermission"].ToString();
            string ADDRenewEscalate = RCRenewEscalatePermission.Length < 1 ? "Y" : RCRenewEscalatePermission.Substring(0, 1);
            string ViewRenewEscalate = RCRenewEscalatePermission.Length < 4 ? "Y" : RCRenewEscalatePermission.Substring(3, 1);
            if (ADDRenewEscalate == "N")
            {
                divRevnew.Style.Add("display", "none");
            }
            if (ViewRenewEscalate == "N")
            {
                // lnkExcelRenewEscalte.Attributes.Add("OnClick", "");
                //tabRenew.Attributes.Add("href", "");
                divRevnewEsc.Style.Add("display", "none");
                hdnRenewEsclateView.Value = "N";
                // tabRenew.Visible = false;
                //lnkExcelRenewEscalte
                //OnClick = "lnkExcelRenewEscalte_Click"
                // Response.Redirect("Home.aspx?permission=no"); return;
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
    private void FillRoute()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getRoute(objProp_User, 1, 0, 0);//IsActive=1 :- Get Only Active Workers
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "label";
        ddlRoute.DataValueField = "ID";
        ddlRoute.DataBind();
    }

    private void FillServiceType()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        ds = new BusinessLayer.Programs.BL_ServiceType().GetActiveServiceType(objProp_User.ConnConfig);

        ddlServiceType.DataSource = ds.Tables[0];
        ddlServiceType.DataTextField = "type";
        ddlServiceType.DataValueField = "type";
        ddlServiceType.DataBind();
        ddlServiceType.Items.Insert(0, new ListItem("None", "None"));
    }

    private void CompanyPermission()
    {
        if (Convert.ToString(Session["COPer"]) == "1" || Convert.ToString(ConfigurationManager.AppSettings["CustomerName"]).Equals("SECO"))
        {
            RadGrid_RecContracts.Columns[5].Visible = true;
            ddlSearch.Items.FindByValue("B.Name").Enabled = true;

        }
        else
        {
            RadGrid_RecContracts.Columns[5].Visible = false;
            ddlSearch.Items.FindByValue("B.Name").Enabled = false;
        }
    }

    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet dc = new DataSet();
        dc = objBL_Company.getCompanyByUserID(objCompany);
        if (dc.Tables.Count > 0)
        {
            ddlCompany.DataSource = dc.Tables[0];
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataBind();
        }
    }

    private void GetDataAll()
    {
        List<RetainFilter> filters = new List<RetainFilter>();
        String filterExpression = Convert.ToString(RadGrid_RecContracts.MasterTableView.FilterExpression);
        if (!IsPostBack)
        {
            if (filterExpression == "")
            {
                if (Convert.ToString(Request.QueryString["fil"]) == "c")
                {
                    if (Session["RecContracts_FilterExpression"] != null && Convert.ToString(Session["RecContracts_FilterExpression"]) != "" && Session["RecContracts_Filters"] != null)
                    {
                        filterExpression = Convert.ToString(Session["RecContracts_FilterExpression"]);
                        RadGrid_RecContracts.MasterTableView.FilterExpression = Convert.ToString(Session["RecContracts_FilterExpression"]);
                        var filtersGet = Session["RecContracts_Filters"] as List<RetainFilter>;
                        if (filtersGet != null)
                        {
                            foreach (var _filter in filtersGet)
                            {

                                var filterCol = _filter.FilterColumn;
                                GridColumn column = RadGrid_RecContracts.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                                if (column != null)
                                {
                                    column.CurrentFilterValue = _filter.FilterValue;
                                }
                            }
                        }

                    }

                }
                else
                {
                    Session["RecContracts_FilterExpression"] = null;
                    Session["RecContracts_Filters"] = null;
                }


            }
        }
        if (!IsGridPageIndexChanged)
        {
            RadGrid_RecContracts.CurrentPageIndex = 0;
            Session["RadGrid_RecContractsCurrentPageIndex"] = 0;
            ViewState["RadGrid_RecContractsminimumRows"] = 0;
            ViewState["RadGrid_RecContractsmaximumRows"] = RadGrid_RecContracts.PageSize;

        }
        else
        {
            if (Session["RadGrid_RecContractsCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_RecContractsCurrentPageIndex"].ToString()) != 0
                && Request.QueryString["fil"] != null && Request.QueryString["fil"].ToString() == "c")
            {
                RadGrid_RecContracts.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_RecContractsCurrentPageIndex"].ToString());
                ViewState["RadGrid_RecContractsminimumRows"] = RadGrid_RecContracts.CurrentPageIndex * RadGrid_RecContracts.PageSize;
                ViewState["RadGrid_RecContractsmaximumRows"] = (RadGrid_RecContracts.CurrentPageIndex + 1) * RadGrid_RecContracts.PageSize;

            }
        }

        //if (string.IsNullOrEmpty(filterExpression) && Session["RecContracts_FilterExpression"] != null)
        //{
        //    filterExpression = Convert.ToString(Session["RecContracts_FilterExpression"]);
        //}

        if (filterExpression != "")
        {
            Session["RecContracts_FilterExpression"] = filterExpression;
            foreach (GridColumn column in RadGrid_RecContracts.MasterTableView.OwnerGrid.Columns)
            {

                String filterValues = String.Empty;
                String columnName = column.UniqueName;

                filterValues = column.CurrentFilterValue;

                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);

                }

            }

            Session["RecContracts_Filters"] = filters;
            var test = Session["RecContracts_Filters"];
        }
        else
        {
            Session["RecContracts_FilterExpression"] = null;
            Session["RecContracts_Filters"] = null;
        }

        DataSet ds = new DataSet();
        objProp_Contracts.UserID = Session["UserID"].ToString();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.SearchBy = ddlSearch.SelectedValue;

        if (ddlSearch.SelectedValue == "j.ctype")
        {
            objProp_Contracts.SearchValue = ddlServiceType.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "c.Status")
        {
            objProp_Contracts.SearchValue = rbStatus.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "c.bcycle")
        {
            objProp_Contracts.SearchValue = ddlBillFreq.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "c.scycle")
        {
            objProp_Contracts.SearchValue = ddlTicketFreq.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "j.custom20")
        {
            objProp_Contracts.SearchValue = ddlRoute.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "j.SPHandle")
        {
            objProp_Contracts.SearchValue = ddlSpecialNotes.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "B.Name")
        {
            objProp_Contracts.SearchValue = ddlCompany.SelectedValue;
        }
        else
        {
            objProp_Contracts.SearchValue = txtSearch.Text.Trim().Replace("'", "''");
        }
        #region Company Check
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objProp_Contracts.EN = 1;
        }
        else
        {
            objProp_Contracts.EN = 0;
        }
        #endregion
        ds = objBL_Contracts.getContractsData(objProp_Contracts);
        DataTable dtFinal = new DataTable();

        if (ds != null && ds.Tables[0].Rows.Count > 0 && !chkcontractInactive.Checked)
        {
            if (ds.Tables[0].Select("Status <> 'Closed'").Count() > 0)
            {
                dtFinal = ds.Tables[0].Select("Status <> 'Closed'").CopyToDataTable();
            }
            else
            {
                dtFinal = null;
            }
        }
        else if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            dtFinal = ds.Tables[0];
        }
        BindGridDatatable(dtFinal);
    }
    private void BindGridDatatable(DataTable dt)
    {
        try
        {
            DataTable dtResult = processDataFilter(dt);
            Session["ContrSrch"] = dtResult;
            if (dtResult != null)
            {
                RadGrid_RecContracts.VirtualItemCount = dtResult.Rows.Count;

                RadGrid_RecContracts.DataSource = dtResult;
                RadGrid_RecContracts.AllowCustomPaging = true;
                if (dtResult != null)
                {
                    lblRecordCount.Text = dtResult.Rows.Count + " Record(s) found";
                }
                else
                {
                    lblRecordCount.Text = "0 Record(s) found";
                }
            }
            else
            {
                RadGrid_RecContracts.VirtualItemCount = 0;
                RadGrid_RecContracts.DataSource = string.Empty;
                lblRecordCount.Text = "0 Record(s) found";
            }

            //RadGrid_RecContracts.MasterTableView.FilterExpression = string.Empty;

        }
        catch (Exception ex)
        {

        }

    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addreccontract.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_RecContracts.SelectedItems)
        {
            Label lblID = (Label)di.FindControl("lblID");
            DeleteContracts(Convert.ToInt32(lblID.Text));
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_RecContracts.SelectedItems)
        {
            Label lblID = (Label)di.FindControl("lblID");
            Response.Redirect("addreccontract.aspx?uid=" + lblID.Text + "&t=c");
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_RecContracts.SelectedItems)
        {
            Label lblID = (Label)di.FindControl("lblID");
            Response.Redirect("addreccontract.aspx?uid=" + lblID.Text);
        }
    }

    private void DeleteContracts(int ID)
    {
        objProp_Contracts.JobId = ID;
        objProp_Contracts.ConnConfig = Session["config"].ToString();

        try
        {
            objBL_Contracts.DeleteContract(objProp_Contracts);
            GetDataAll();
            RadGrid_RecContracts.Rebind();
            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Contract deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectSearch();
    }

    private void SelectSearch()
    {
        //if (ddlSearch.SelectedValue == "j.ctype")
        //{
        //    ddlServiceType.SelectedIndex = 0;
        //    ddlServiceType.Visible = true;
        //    rbStatus.Visible = false;
        //    ddlBillFreq.Visible = false;
        //    ddlTicketFreq.Visible = false;
        //    txtSearch.Visible = false;
        //    ddlRoute.Visible = false;
        //    ddlCompany.Visible = false;
        //}
        //else if (ddlSearch.SelectedValue == "c.Status")
        //{
        //    rbStatus.SelectedIndex = 0;
        //    rbStatus.Visible = true;
        //    ddlServiceType.Visible = false;
        //    ddlBillFreq.Visible = false;
        //    ddlTicketFreq.Visible = false;
        //    txtSearch.Visible = false;
        //    ddlRoute.Visible = false;
        //    ddlCompany.Visible = false;
        //}
        //else if (ddlSearch.SelectedValue == "c.bcycle")
        //{
        //    ddlBillFreq.SelectedIndex = 0;
        //    ddlBillFreq.Visible = true;
        //    rbStatus.Visible = false;
        //    ddlServiceType.Visible = false;
        //    ddlTicketFreq.Visible = false;
        //    txtSearch.Visible = false;
        //    ddlRoute.Visible = false;
        //    ddlCompany.Visible = false;
        //}
        //else if (ddlSearch.SelectedValue == "c.scycle")
        //{
        //    ddlTicketFreq.SelectedIndex = 0;
        //    ddlTicketFreq.Visible = true;
        //    rbStatus.Visible = false;
        //    ddlServiceType.Visible = false;
        //    ddlBillFreq.Visible = false;
        //    txtSearch.Visible = false;
        //    ddlRoute.Visible = false;
        //    ddlCompany.Visible = false;
        //}
        //else if (ddlSearch.SelectedValue == "j.custom20")
        //{
        //    ddlTicketFreq.SelectedIndex = 0;
        //    ddlTicketFreq.Visible = false;
        //    rbStatus.Visible = false;
        //    ddlServiceType.Visible = false;
        //    ddlBillFreq.Visible = false;
        //    txtSearch.Visible = false;
        //    ddlRoute.Visible = true;
        //    ddlCompany.Visible = false;
        //}
        //else if (ddlSearch.SelectedValue == "B.Name")
        //{
        //    ddlTicketFreq.SelectedIndex = 0;
        //    ddlTicketFreq.Visible = false;
        //    rbStatus.Visible = false;
        //    ddlServiceType.Visible = false;
        //    ddlBillFreq.Visible = false;
        //    txtSearch.Visible = false;
        //    ddlRoute.Visible = false;
        //    ddlCompany.Visible = true;
        //}
        //else
        //{
        //    ddlTicketFreq.Visible = false;
        //    rbStatus.Visible = false;
        //    ddlServiceType.Visible = false;
        //    ddlBillFreq.Visible = false;
        //    txtSearch.Visible = true;
        //    ddlRoute.Visible = false;
        //    ddlCompany.Visible = false;
        //}
    }

    bool isGrouping = false;

    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_RecContracts.MasterTableView.FilterExpression != "" ||
            (RadGrid_RecContracts.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_RecContracts.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_RecContracts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {


        SaveFilter();
        GetDataAll();
        //FillFilter();
    }

    protected void RadGrid_RecContracts_ItemEvent(object sender, GridItemEventArgs e)
    {



    }

    public bool ShouldEscalationApplySortFilterOrGroup()
    {
        return RadGrid_Escalation.MasterTableView.FilterExpression != "" ||
            (RadGrid_Escalation.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Escalation.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Escalation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //RadGrid_Escalation.AllowCustomPaging = !ShouldEscalationApplySortFilterOrGroup();
        GetEscalationContracts();
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        SaveFilter();
        GetDataAll();
        RadGrid_RecContracts.Rebind();
    }

    private void RowSelect()
    {
        DataTable dtContract = new DataTable();
        dtContract.Columns.Add("Job");
        DataRow dr = null;
        foreach (GridDataItem gr in RadGrid_RecContracts.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            HyperLink lnkName = (HyperLink)gr.FindControl("lblLocName");
            dr = dtContract.NewRow();
            //add values to each rows
            dr["Job"] = lblID.Text;
            //add the row to DataTable
            dtContract.Rows.Add(dr);
            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addreccontract.aspx?uid=" + lblID.Text + "'";
        }
        Session["ContractJobID"] = dtContract;
       
    }

    protected void RadGrid_RecContracts_PreRender(object sender, EventArgs e)
    {
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_RecContracts);
        RowSelect();
    }

    private void RowSelectEscalation()
    {
        foreach (GridDataItem gr in RadGrid_Escalation.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkJob");
            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addreccontract.aspx?rt=2&uid=" + lblID.Text + "'";
           
        }
    }

    protected void RadGrid_Escalation_PreRender(object sender, EventArgs e)
    {
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Escalation);
        RowSelectEscalation();
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        chkcontractInactive.Checked = false;
        ddlSearch.SelectedIndex = 0;
        ddlSearch_SelectedIndexChanged(sender, e);
        SaveFilter();
        GetDataAll();
        foreach (GridColumn column in RadGrid_RecContracts.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        Session.Remove("RecContracts_FilterExpression");
        Session.Remove("RecContracts_Filters");
        RadGrid_RecContracts.MasterTableView.FilterExpression = string.Empty;
        RadGrid_RecContracts.PageSize = 50;
        RadGrid_RecContracts.MasterTableView.PageSize = 50;
        RadGrid_RecContracts.Rebind();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = 0;
        ddlSearch_SelectedIndexChanged(sender, e);
        txtSearch.Text = "";
        chkcontractInactive.Checked = false;
        SaveFilter();

        foreach (GridColumn column in RadGrid_RecContracts.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        Session.Remove("RecContracts_FilterExpression");
        Session.Remove("RecContracts_Filters");
        //GetDataAll();
        RadGrid_RecContracts.PageSize = 50;
        RadGrid_RecContracts.MasterTableView.PageSize = 50;
        RadGrid_RecContracts.MasterTableView.FilterExpression = string.Empty;
        RadGrid_RecContracts.Rebind();
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

    private void SaveFilter()
    {
        Dictionary<string, string> dictFilter = new Dictionary<string, string>();
        dictFilter["Search"] = ddlSearch.SelectedValue;
        dictFilter["status"] = rbStatus.SelectedValue;
        dictFilter["servicetype"] = ddlServiceType.SelectedValue;
        dictFilter["billfreq"] = ddlBillFreq.SelectedValue;
        dictFilter["ticktfreq"] = ddlTicketFreq.SelectedValue;
        dictFilter["searchtxt"] = txtSearch.Text.Trim();
        dictFilter["route"] = ddlRoute.SelectedValue;
        dictFilter["CompanyId"] = ddlCompany.SelectedValue;
        dictFilter["IncActive"] = Convert.ToString(chkcontractInactive.Checked);
        dictFilter["ddlSpecialNotes"] = ddlSpecialNotes.SelectedValue;
        if (ViewState["sortExpression"] != null)
        {
            dictFilter["sortExpression"] = ViewState["sortExpression"].ToString();
        }
        else
        {
            dictFilter["sortExpression"] = "";
        }
        if (ViewState["sortExpression"] != null)
        {
            dictFilter["sortDirection"] = ViewState["sortdir"].ToString();
        }
        else
        { dictFilter["sortDirection"] = ""; }
        Session["FilterRecContr"] = dictFilter;
    }

    private void FillFilter()
    {
        if (Session["FilterRecContr"] != null)
        {
            Dictionary<string, string> dictFilter = new Dictionary<string, string>();
            dictFilter = (Dictionary<string, string>)Session["FilterRecContr"];
            ddlSearch.SelectedValue = dictFilter["Search"];
            SelectSearch();
            rbStatus.SelectedValue = dictFilter["status"];
            ddlServiceType.SelectedValue = dictFilter["servicetype"];
            ddlBillFreq.SelectedValue = dictFilter["billfreq"];
            ddlTicketFreq.SelectedValue = dictFilter["ticktfreq"];
            ddlRoute.SelectedValue = dictFilter["route"];
            ddlCompany.SelectedValue = dictFilter["CompanyId"];
            txtSearch.Text = dictFilter["searchtxt"];
            ddlSpecialNotes.SelectedValue = dictFilter["ddlSpecialNotes"];
            chkcontractInactive.Checked = false;
            if (dictFilter["IncActive"].ToLower() == "true")
            {
                chkcontractInactive.Checked = true;
            }
            showFilterSearch();
        }
    }
    private List<CustomerReport> GetReportsName()
    {
        List<CustomerReport> lstCustomerReport = new List<CustomerReport>();
        try
        {
            DataSet dsGetReports = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objProp_User.Type = "'Recurring','Escalation'";
            dsGetReports = objBL_ReportsData.GetMultipleStockReports(objProp_User);
            //if (dsGetReports.Tables.Count > 0)
            for (int i = 0; i <= dsGetReports.Tables[0].Rows.Count - 1; i++)
            {
                CustomerReport objCustomerReport = new CustomerReport();

                objCustomerReport.ReportId = Convert.ToInt32(dsGetReports.Tables[0].Rows[i]["Id"]);
                objCustomerReport.ReportName = dsGetReports.Tables[0].Rows[i]["ReportName"].ToString();
                objCustomerReport.ReportType = dsGetReports.Tables[0].Rows[i]["ReportType"].ToString();
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

    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportsName());
        string reports = "var reports=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", reports, true);
    }

    private void GetEscalationContracts()
    {
        {
            List<RetainFilter> filters = new List<RetainFilter>();
            String filterExpression = Convert.ToString(RadGrid_Escalation.MasterTableView.FilterExpression);
            if (!IsPostBack)
            {
                if (filterExpression == "")
                {
                    if (Convert.ToString(Request.QueryString["fil"]) == "c")
                    {
                        if (Session["Escalation_FilterExpression"] != null && Convert.ToString(Session["Escalation_FilterExpression"]) != "" && Session["Escalation_Filters"] != null)
                        {
                            filterExpression = Convert.ToString(Session["Escalation_FilterExpression"]);
                            RadGrid_Escalation.MasterTableView.FilterExpression = Convert.ToString(Session["Escalation_FilterExpression"]);
                            var filtersGet = Session["Escalation_Filters"] as List<RetainFilter>;
                            if (filtersGet != null)
                            {
                                foreach (var _filter in filtersGet)
                                {

                                    var filterCol = _filter.FilterColumn;
                                    GridColumn column = RadGrid_Escalation.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                                    if (column != null)
                                    {
                                        column.CurrentFilterValue = _filter.FilterValue;
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        Session["Escalation_FilterExpression"] = null;
                        Session["Escalation_Filters"] = null;
                    }

                }
            }
            if (!IsEscalationGridPageIndexChanged)
            {
                RadGrid_Escalation.CurrentPageIndex = 0;
                Session["RadGrid_EscalationCurrentPageIndex"] = 0;
                ViewState["RadGrid_EscalationminimumRows"] = 0;
                ViewState["RadGrid_EscalationmaximumRows"] = RadGrid_Escalation.PageSize;
                Session["EscalationPriorto"] = txtEscDate.Text;

            }
            else
            {
                if (Session["RadGrid_EscalationCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_EscalationCurrentPageIndex"].ToString()) != 0
                    && Request.QueryString["fil"] != null && Request.QueryString["fil"].ToString() == "c")
                {
                    RadGrid_Escalation.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_EscalationCurrentPageIndex"].ToString());
                    ViewState["RadGrid_EscalationminimumRows"] = RadGrid_Escalation.CurrentPageIndex * RadGrid_Escalation.PageSize;
                    ViewState["RadGrid_EscalationmaximumRows"] = (RadGrid_Escalation.CurrentPageIndex + 1) * RadGrid_Escalation.PageSize;
                }

            }

            //if (string.IsNullOrEmpty(filterExpression) && Session["Escalation_FilterExpression"] != null)
            //{
            //    filterExpression = Convert.ToString(Session["Escalation_FilterExpression"]);
            //}

            if (filterExpression != "")
            {
                Session["Escalation_FilterExpression"] = filterExpression;
                foreach (GridColumn column in RadGrid_Escalation.MasterTableView.OwnerGrid.Columns)
                {

                    String filterValues = String.Empty;
                    String columnName = column.UniqueName;

                    filterValues = column.CurrentFilterValue;

                    if (filterValues != "")
                    {

                        RetainFilter filter = new RetainFilter();
                        filter.FilterColumn = columnName;
                        filter.FilterValue = filterValues;
                        filters.Add(filter);

                    }



                }

                hdnEscalationContracts.Value = "1";
                Session["Escalation_Filters"] = filters;

            }
            else
            {
                hdnEscalationContracts.Value = "1";

                Session["Escalation_FilterExpression"] = null;
                Session["Escalation_Filters"] = null;
            }


            DataTable dtFinal = new DataTable();
            DataSet ds = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            txtEscDate.Text = Session["EscalationPriorto"].ToString();
            objProp_Contracts.EscalationLast = Convert.ToDateTime(txtEscDate.Text);
            objProp_Contracts.UserID = Convert.ToString(Session["userId"]);

            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objProp_Contracts.EN = 1;
            }
            else
            {
                objProp_Contracts.EN = 0;
            }

            ds = objBL_Contracts.GetEscalationContracts(objProp_Contracts, hdnEscalationContracts.Value, lnkChk.Checked);

            if (ds.Tables.Count > 0)
            {
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        dtFinal = ds.Tables[0];
                    }
                }
            }

            DataTable dtResult = processEscalationDataFilter(dtFinal);

            if (dtResult != null)
            {

                RadGrid_Escalation.VirtualItemCount = dtResult.Rows.Count;
                RadGrid_Escalation.DataSource = String.Empty;
                RadGrid_Escalation.DataSource = dtResult;
                RadGrid_Escalation.AllowCustomPaging = true;

                lblRenewRecord.Text = dtResult.Rows.Count.ToString() + " Record(s) Found.";
                hdnRenewRrcords.Value = dtResult.Rows.Count.ToString();
            }
            else
            {

                RadGrid_Escalation.VirtualItemCount = 0;
                RadGrid_Escalation.DataSource = String.Empty;
                lblRenewRecord.Text = "0 Record(s) Found.";
                hdnRenewRrcords.Value = "0";
            }

            #region

            DataTable _dtravi = new DataTable();

            _dtravi.Columns.Add("ContractID");

            if (dtResult != null)
            {
                foreach (DataRow item in dtResult.Rows)
                {
                    DataRow _ravi = _dtravi.NewRow(); _ravi["ContractID"] = item["job"]; _dtravi.Rows.Add(_ravi);
                }
            }

            ViewState["_dtravi"] = _dtravi;

            #endregion
        }
    }

    protected void lnkRefreshEsc_Click(object sender, EventArgs e)
    {
        hdnEscalationContracts.Value = "1";
        GetEscalationContracts();
        RadGrid_Escalation.Rebind();
    }

    protected void lnkESCfact_ServerClick(object sender, EventArgs e)
    {
        string str = string.Empty;

        foreach (GridDataItem item in RadGrid_Escalation.Items)
        {

            double MassUpdateEscFactor = 0.0;

            double.TryParse(txtMassUpdateEscFactor.Text, out MassUpdateEscFactor);

            if (MassUpdateEscFactor > 0)
            {

                Label jobID = (Label)item.FindControl("lblID");

                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.jobid = Convert.ToInt32(jobID.Text);
                objProp_Contracts.EscalationFactor = Convert.ToDouble(txtMassUpdateEscFactor.Text);
                objProp_Contracts.Fuser = Session["Username"].ToString();
                objBL_Contracts.UpdateEscalationFactor(objProp_Contracts);

                str += jobID.Text + ",";
            }
        }

        if (str != string.Empty)
        {
            hdnEscalationContracts.Value = "1";
            GetEscalationContracts();
            RadGrid_Escalation.Rebind();
            txtMassUpdateEscFactor.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keySucc125fact4", "noty({text: 'Contract Escalation Factor updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkEscalate_Click(object sender, EventArgs e)
    {
        string str = string.Empty;

        foreach (GridDataItem item in RadGrid_Escalation.Items)
        {
            Label jobID = (Label)item.FindControl("lblID");

            CheckBox chkbox = (CheckBox)item.FindControl("chkSelect");

            if (chkbox.Checked)
            {
                str += jobID.Text + ",";
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.jobid = Convert.ToInt32(jobID.Text);
                objProp_Contracts.EscalationPriorto = Convert.ToDateTime(txtEscDate.Text);
                objProp_Contracts.EscalationDate = Convert.ToDateTime(txtNextEscdate.Text);
                objProp_Contracts.Fuser = Session["Username"].ToString();
                objBL_Contracts.EscalateContract(objProp_Contracts);
            }

        }

        if (str != string.Empty)
        {
            hdnEscalationContracts.Value = "1";
            GetEscalationContracts();
            RadGrid_Escalation.Rebind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "keySucc1254", "noty({text: 'Contract Renew/Escalate successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keySucc1254dssd", "noty({text: 'Please select a Contract to Renew/Escalate ',  type : 'warring', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        GetEscalationContracts();
        RadGrid_Escalation.Rebind();
    }

    protected void chkcontractInactive_CheckedChanged(object sender, EventArgs e)
    {
        IsEscalationGridPageIndexChanged = false;
        SaveFilter();
        GetDataAll();
    }

    protected void RadGrid_Escalation_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (Convert.ToString(RadGrid_Escalation.MasterTableView.FilterExpression) != "")
                {
                    lblRenewRecord.Text = totalCount + " Record(s) found";
                    hdnRenewRrcords.Value = totalCount.ToString();
                }

                else
                {
                    lblRenewRecord.Text = RadGrid_Escalation.VirtualItemCount + " Record(s) found";
                    hdnRenewRrcords.Value = totalCount.ToString();
                }

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

    protected void RadGrid_Escalation_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 3;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 3;
        else
            currentItem = 4;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Escalation.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Escalation.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

    protected void RadGrid_RecContracts_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (Convert.ToString(RadGrid_RecContracts.MasterTableView.FilterExpression) != "")
                    lblRecordCount.Text = totalCount + " Record(s) found";
                else
                    lblRecordCount.Text = RadGrid_RecContracts.VirtualItemCount + " Record(s) found";
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
        catch (Exception ex)
        {

        }
    }

    protected void lnkExcelRenewEscalte_Click(object sender, EventArgs e)
    {
        hdnEscalationContracts.Value = "1";

        RadGrid_Escalation.ExportSettings.FileName = "RenewEscalate";
        RadGrid_Escalation.Columns.FindByUniqueName("Customer").Visible = true;
        RadGrid_Escalation.Columns.FindByUniqueName("Address").Visible = true;
        RadGrid_Escalation.Columns.FindByUniqueName("City").Visible = true;
        RadGrid_Escalation.Columns.FindByUniqueName("State").Visible = true;
        RadGrid_Escalation.Columns.FindByUniqueName("Zip").Visible = true;
        RadGrid_Escalation.Columns.FindByUniqueName("LocationCompanyName").Visible = true;

        if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("accredited"))
        {
            RadGrid_Escalation.Columns.FindByUniqueName("Hours").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("ScheduleFrequency").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("OriginalContract").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("LastRenew").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("ExpirationType").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("BLenght").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("BStart").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("SStart").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("ScheduledTime").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("BillingRate").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("RateOT").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("RateNT").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("RateMileage").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("RateDT").Visible = true;
            RadGrid_Escalation.Columns.FindByUniqueName("RateTravel").Visible = true;
        }

        RadGrid_Escalation.ExportSettings.IgnorePaging = true;
        RadGrid_Escalation.ExportSettings.ExportOnlyData = true;
        RadGrid_Escalation.ExportSettings.OpenInNewWindow = true;
        RadGrid_Escalation.ExportSettings.HideStructureColumns = true;
        RadGrid_Escalation.MasterTableView.UseAllDataFields = true;
        RadGrid_Escalation.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Escalation.MasterTableView.ExportToExcel();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_RecContracts.ExportSettings.FileName = "AllContract";

        if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("accredited"))
        {
            RadGrid_RecContracts.Columns.FindByUniqueName("OriginalContract").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("LastRenew").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("ExpirationType").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("ExpirationDate").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("BLenght").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("BStart").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("SStart").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("ScheduledTime").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("RenewalNotes").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("BillingRate").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("RateOT").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("RateNT").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("RateMileage").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("RateDT").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("RateTravel").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("EscalationType").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("BEscCycle").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("BEscFact").Visible = true;
            RadGrid_RecContracts.Columns.FindByUniqueName("EscLast").Visible = true;
        }

        RadGrid_RecContracts.ExportSettings.IgnorePaging = true;
        RadGrid_RecContracts.ExportSettings.ExportOnlyData = true;
        RadGrid_RecContracts.ExportSettings.OpenInNewWindow = true;
        RadGrid_RecContracts.ExportSettings.HideStructureColumns = true;
        RadGrid_RecContracts.MasterTableView.UseAllDataFields = true;
        RadGrid_RecContracts.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_RecContracts.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_RecContracts_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 3;
        else
            currentItem = 4;
        if (e.Worksheet.Table.Rows.Count == RadGrid_RecContracts.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_RecContracts.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

    private void SetDefaultWorker()
    {
        Customer objCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();

        var masterTableView = RadGrid_RecContracts.MasterTableView;
        var column = masterTableView.GetColumn("Worker");
        objCustomer.ConnConfig = Session["config"].ToString();
        string getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        if (!string.IsNullOrEmpty(getValue))
        {
            column.HeaderText = getValue;
            ddlSearch.Items.Insert(9, new ListItem(getValue, "j.custom20"));
        }
        else
        {
            column.HeaderText = "Default Worker";
            ddlSearch.Items.Insert(9, new ListItem("Default Worker", "j.custom20"));
        }
    }

    public void showFilterSearch()
    {

        txtSearch.Style.Add("display", "none");
        ddlServiceType.Style.Add("display", "none");
        rbStatus.Style.Add("display", "none");
        ddlBillFreq.Style.Add("display", "none");
        ddlTicketFreq.Style.Add("display", "none");
        ddlRoute.Style.Add("display", "none");

        if (ddlSearch.SelectedValue == "j.ctype")
        {

            ddlServiceType.Style.Add("display", "Block");

        }
        else if (ddlSearch.SelectedValue == "c.Status")
        {

            rbStatus.Style.Add("display", "Block");

        }
        else if (ddlSearch.SelectedValue == "c.bcycle")
        {

            ddlBillFreq.Style.Add("display", "Block");

        }
        else if (ddlSearch.SelectedValue == "c.scycle")
        {

            ddlTicketFreq.Style.Add("display", "Block");

        }
        else if (ddlSearch.SelectedValue == "j.custom20")
        {

            ddlRoute.Style.Add("display", "Block");

        }
        else if (ddlSearch.SelectedValue == "j.SPHandle")
        {

            ddlSpecialNotes.Style.Add("display", "Block");

        }
        else if (ddlSearch.SelectedValue == "B.Name")
        {
            ddlCompany.Style.Add("display", "Block");
        }
        else
        {
            txtSearch.Style.Add("display", "Block");

        }
    }

    protected void RadGrid_RecContracts_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGrid_RecContractsminimumRows"] = RadGrid_RecContracts.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_RecContractsmaximumRows"] = (RadGrid_RecContracts.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }

    }

    protected void RadGrid_RecContracts_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGrid_RecContractsCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_RecContractsminimumRows"] = e.NewPageIndex * RadGrid_RecContracts.PageSize;
            ViewState["RadGrid_RecContractsmaximumRows"] = (e.NewPageIndex + 1) * RadGrid_RecContracts.PageSize;
        }
        catch { }
    }

    private DataTable processDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Session["RecContracts_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                if (Session["RecContracts_Filters"] != null)
                {
                    var filtersGet = Session["RecContracts_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {

                            GridColumn column = RadGrid_RecContracts.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                            if (column.UniqueName == "Job" || column.UniqueName == "MonthlyBill" || column.UniqueName == "BAmt"
                                || column.UniqueName == "MonthlyHours" || column.UniqueName == "Hours" || column.UniqueName == "Hours")
                            {
                                sql = sql + " And " + column.UniqueName + "=" + _filter.FilterValue.Replace(",", "");
                            }
                            else
                            {
                                sql = sql + " And " + column.UniqueName + " like '%" + _filter.FilterValue + "%'";
                            }


                        }
                    }
                }
                if (result.Select(sql).Count() > 0)
                {
                    return result.Select(sql).CopyToDataTable();
                }
                else
                {
                    return null;
                }


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

    protected void RadGrid_Escalation_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsEscalationGridPageIndexChanged = true;
            ViewState["RadGrid_EscalationsminimumRows"] = RadGrid_Escalation.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_EscalationsmaximumRows"] = (RadGrid_Escalation.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }

    protected void RadGrid_Escalation_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsEscalationGridPageIndexChanged = true;
            Session["RadGrid_EscalationCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_EscalationsminimumRows"] = e.NewPageIndex * RadGrid_Escalation.PageSize;
            ViewState["RadGrid_EscalationsmaximumRows"] = (e.NewPageIndex + 1) * RadGrid_Escalation.PageSize;
        }
        catch { }
    }

    private DataTable processEscalationDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Session["Escalation_Filters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                if (Session["Escalation_Filters"] != null)
                {
                    var filtersGet = Session["Escalation_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {

                            GridColumn column = RadGrid_Escalation.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                            if (column.UniqueName == "Job" || column.UniqueName == "BAmt" || column.UniqueName == "newamt")
                            {
                                sql = sql + " And " + column.UniqueName + "=" + _filter.FilterValue.Replace(",", "");
                            }
                            else
                            {
                                sql = sql + " And " + column.UniqueName + " like '%" + _filter.FilterValue + "%'";
                            }


                        }
                    }
                }
                if (result.Select(sql).Count() > 0)
                {
                    return result.Select(sql).CopyToDataTable();
                }
                else
                {
                    return null;
                }


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

    //Maintenance Equipment Count By Date Report Lin Action Event
    protected void lnkMaintenanceEquipmentReport_Click(object sender, EventArgs e)
    {
        var searchValue = string.Empty;

        if (ddlSearch.SelectedValue == "j.ctype")
        {
            if (ddlSearch.SelectedValue == "None")
            {
                searchValue = string.Empty;
            }
            searchValue = ddlServiceType.SelectedValue;
        }
        if (ddlSearch.SelectedValue == "c.Status")
        {
            searchValue = rbStatus.SelectedValue;
        }
        if (ddlSearch.SelectedValue == "c.bcycle")
        {
            searchValue = ddlBillFreq.SelectedValue;
        }
        if (ddlSearch.SelectedValue == "c.scycle")
        {
            searchValue = ddlTicketFreq.SelectedValue;
        }
        if (ddlSearch.SelectedValue == "r.name")
        {
            searchValue = txtSearch.Text;
        }
        if (ddlSearch.SelectedValue == "l.tag")
        {
            searchValue = txtSearch.Text;
        }
        if (ddlSearch.SelectedValue == "B.Name")
        {
            searchValue = txtSearch.Text;
        }
        if (ddlSearch.SelectedValue == "r.State")
        {
            searchValue = txtSearch.Text;
        }
        if (ddlSearch.SelectedValue == "j.SPHandle")
        {
            searchValue = ddlSpecialNotes.SelectedValue;
        }

        string urlString = "MaintenanceEquipmentCountByDate.aspx?stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchValue;
        Response.Redirect(urlString, true);
    }

    protected void lnkEquipmentContractByCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            byte[] buffer1 = null;
            StiReport report = new StiReport();
            report = LoadReport();
            var settings = new Stimulsoft.Report.Export.StiExcelExportSettings();
            settings.UseOnePageHeaderAndFooter = true;
            var service = new Stimulsoft.Report.Export.StiExcelExportService();
            MemoryStream stream = new MemoryStream();
            service.ExportTo(report, stream, settings);
            buffer1 = stream.ToArray();

            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", $"attachment;filename=EquipmentContractByCustomer.xls");
            Response.ContentType = "application/xls";
            Response.AddHeader("Content-Length", (buffer1.Length).ToString());
            Response.BinaryWrite(buffer1);
            Response.Flush();
            Response.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkServiceSalesCheckUpReport_Click(object sender, EventArgs e)
    {
        var searchValue = string.Empty;

        if (ddlSearch.SelectedValue == "j.ctype")
        {
            if (ddlSearch.SelectedValue == "None")
            {
                searchValue = string.Empty;
            }
            searchValue = ddlServiceType.SelectedValue;
        }
        if (ddlSearch.SelectedValue == "c.Status")
        {
            searchValue = rbStatus.SelectedValue;
        }
        if (ddlSearch.SelectedValue == "c.bcycle")
        {
            searchValue = ddlBillFreq.SelectedValue;
        }
        if (ddlSearch.SelectedValue == "c.scycle")
        {
            searchValue = ddlTicketFreq.SelectedValue;
        }
        if (ddlSearch.SelectedValue == "r.name")
        {
            searchValue = txtSearch.Text;
        }
        if (ddlSearch.SelectedValue == "l.tag")
        {
            searchValue = txtSearch.Text;
        }
        if (ddlSearch.SelectedValue == "B.Name")
        {
            searchValue = txtSearch.Text;
        }
        if (ddlSearch.SelectedValue == "r.State")
        {
            searchValue = txtSearch.Text;
        }
        if (ddlSearch.SelectedValue == "j.SPHandle")
        {
            searchValue = ddlSpecialNotes.SelectedValue;
        }

        string urlString = "ServiceSalesCheckUpReport.aspx?stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchValue + "&close=" + chkcontractInactive.Checked;
        Response.Redirect(urlString, true);
    }

    private StiReport LoadReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/EquipmentContractByCustomerReport.mrt");

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            Customer objPropCustomer = new Customer();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            var searchValue = string.Empty;

            if (ddlSearch.SelectedValue == "j.ctype")
            {
                if (ddlSearch.SelectedValue == "None")
                {
                    searchValue = string.Empty;
                }
                searchValue = ddlServiceType.SelectedValue;
            }
            if (ddlSearch.SelectedValue == "c.Status")
            {
                searchValue = rbStatus.SelectedValue;
            }
            if (ddlSearch.SelectedValue == "c.bcycle")
            {
                searchValue = ddlBillFreq.SelectedValue;
            }
            if (ddlSearch.SelectedValue == "c.scycle")
            {
                searchValue = ddlTicketFreq.SelectedValue;
            }
            if (ddlSearch.SelectedValue == "r.name")
            {
                searchValue = txtSearch.Text;
            }
            if (ddlSearch.SelectedValue == "l.tag")
            {
                searchValue = txtSearch.Text;
            }
            if (ddlSearch.SelectedValue == "B.Name")
            {
                searchValue = txtSearch.Text;
            }
            if (ddlSearch.SelectedValue == "r.State")
            {
                searchValue = txtSearch.Text;
            }
            if (ddlSearch.SelectedValue == "j.SPHandle")
            {
                searchValue = ddlSpecialNotes.SelectedValue;
            }

            objProp_Contracts.SearchBy = ddlSearch.SelectedValue;
            objProp_Contracts.SearchValue = searchValue;

            List<RetainFilter> filters = new List<RetainFilter>();
            if (Session["RecContracts_Filters"] != null)
            {
                filters = (List<RetainFilter>)Session["RecContracts_Filters"];
            }

            objProp_Contracts.EndDate = DateTime.Now;

            DataSet ds = objBL_Report.GetEquipmentContractByCustomer(objProp_Contracts, filters, chkcontractInactive.Checked);
            if (ds != null)
            {
                report.RegData("ReportData", ds.Tables[0]);
                report.RegData("TopCustomers", ds.Tables[1]);
                report.RegData("Top25Customers", ds.Tables[2]);

                var totalRev = ds.Tables[1].Compute("Sum(SumOfRev)", string.Empty);
                var totalElev = ds.Tables[1].Compute("Sum(SumOfElevs)", string.Empty);

                report.Dictionary.Variables["TotalRev"].Value = totalRev.ToString();
                report.Dictionary.Variables["TotalElev"].Value = totalElev.ToString();
            }

            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["EndDate"].Value = objProp_Contracts.EndDate.ToShortDateString();

            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    protected void lnkExpirationEmail_Click(object sender, EventArgs e)
    {
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        DataSet ds = objBL_Contracts.GetContractsExpireIn10Days(objProp_Contracts);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            //string script = "function f(){$find(\"" + RadWindowEmail.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
            string script = "function f(){ShowEmailWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarning", "noty({text: 'There is no expiration contracts in 10 days',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void CheckValidEmails(List<string> emails, StringBuilder sbdInValidEmails)
    {
        if (sbdInValidEmails == null) sbdInValidEmails = new StringBuilder();

        foreach (var item in emails.ToList())
        {
            if (!WebBaseUtility.IsValidEmailAddress1(item))
            {
                emails.Remove(item);
                sbdInValidEmails.AppendFormat("{0}</br>", item);
            }
        }
    }

    private void SetDefaultValueForEmail()
    {
        txtSubject.Text = "Contract# {ContractNo} will expire on {ExpirationDate}";

        txtBody.Text =
                "Hi <br/><br/>" +
                "We are sending you a notify for the expiration of your contract# {ContractNo} <br/>" +
                "Below is some information from this contract: <br/>" +
                " - Expiration Date: {ExpirationDate}<br/>" +
                " - Last Renew Date: {LastRenewDate}<br/>" +
                " - Amount: {BillingAmount}<br/>" +
                " - Service Type: {ServiceType}<br/>" +
                " - Description: {Description}<br/><br/>" +
                "Let me know if any question on this<br/><br/>" +
                "Thanks";

        //txtBody.Text =
        //        "Hi \n\n" +
        //        "We are sending you a notify for the expiration of your contract# {ContractNo} \n" +
        //        "Below is some information from this contract: \n" +
        //        " - Expiration Date: {ExpirationDate}\n" +
        //        " - Last Renew Date: {LastRenewDate}\n" +
        //        " - Amount: {BillingAmount}\n" +
        //        " - Service Type: {ServiceType}\n" +
        //        " - Description: {Description}\n\n" +
        //        "Let me know if any question on this\n\n" +
        //        "Thanks";
    }

    protected void lnkSend_Click(object sender, EventArgs e)
    {
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        DataSet ds = objBL_Contracts.GetContractsExpireIn10Days(objProp_Contracts);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            var dt = ds.Tables[0];
            string _fromEmail = string.Empty;
            // Indivudial invoices for location
            int totalInvoicesInList = dt != null ? dt.Rows.Count : 0;

            int totalInvoicesForEmail = totalInvoicesInList;
            int totalSentEmails = 0;
            int totalSendErr = 0;
            //int totalNotSend = totalInvoicesInList - totalInvoicesForEmail;
            int mailCount = 0;
            List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
            List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
            List<string> invoiceIdsSentEmail = new List<string>();
            Tuple<int, string, string> emailSendError = null;
            Tuple<int, string, string> emailGetSentError = null;
            StringBuilder sbdSentError = new StringBuilder();
            StringBuilder sbdGetSentError = new StringBuilder();

            EmailLog emailLog = new EmailLog();
            emailLog.ConnConfig = Session["config"].ToString();
            emailLog.Function = "Email Expiration Contract";
            emailLog.Screen = "Contract";
            emailLog.Username = Session["Username"].ToString();
            emailLog.SessionNo = Guid.NewGuid().ToString();
            StringBuilder sbdInValidEmails = new StringBuilder();
            try
            {

                foreach (DataRow _dr in dt.Rows)
                {
                    int _ref = Convert.ToInt32(_dr["ContractNo"]);
                    objProp_Contracts.Ref = _ref;
                    emailLog.Ref = _ref;
                    if (!string.IsNullOrEmpty(_dr["custom12"].ToString()))
                    {
                        if (mailCount == 4)
                        {
                            Thread.Sleep(10000);
                            mailCount = 0;
                        }

                        _fromEmail = WebBaseUtility.GetFromEmailAddress();
                        string _toEmail = "";
                        string _ccEmail = "";

                        _toEmail = _dr["custom12"].ToString();

                        if (!string.IsNullOrEmpty(_dr["custom13"].ToString()))
                        {
                            _ccEmail = _dr["custom13"].ToString();
                        }

                        Mail mail = new Mail();
                        mail.From = _fromEmail;
                        //Boolean IsMailSend = false;

                        foreach (var toaddress in _toEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            //IsMailSend = true;
                            mail.To.Add(toaddress);
                        }

                        foreach (var ccaddress in _ccEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            mail.Cc.Add(ccaddress);
                        }
                        StringBuilder _sbdInValidEmails = new StringBuilder();
                        CheckValidEmails(mail.To, _sbdInValidEmails);
                        CheckValidEmails(mail.Cc, _sbdInValidEmails);
                        if (_sbdInValidEmails.Length > 0)
                        {
                            sbdInValidEmails.Append(_sbdInValidEmails);
                        }

                        if (mail.To.Count == 0)
                        {
                            totalSendErr++;
                            emailLog.To = _sbdInValidEmails.ToString();
                            emailLog.Status = 0;
                            emailLog.UsrErrMessage = "Invalid emails address";
                            BL_EmailLog bL_EmailLog = new BL_EmailLog();
                            bL_EmailLog.AddEmailLog(emailLog);
                            continue;
                        }

                        //mail.Title = string.Format("Contract# {0} will expire on {1}", _ref.ToString(), _dr["ExpirationDate"].ToString());
                        //string emailContent = "Hi \n" +
                        //    "I am sending you about the expiration of your contract# {ContractNo} \n" +
                        //    "Below is some information from your contract: \n" +
                        //    " - Expiration Date: {ExpirationDate}\n" +
                        //    " - Last Renew Date: {LastRenewDate}\n" +
                        //    " - Amount: {BillingAmount}\n" +
                        //    " - Service Type: {ServiceType}\n" +
                        //    " - Description: {Description}\n\n" +
                        //    "Let me know if any question on this\n" +
                        //    "Thanks";
                        mail.Title = txtSubject.Text.Replace("{ContractNo}", _ref.ToString())
                            .Replace("{ExpirationDate}", _dr["ExpirationDate"].ToString())
                            .Replace("{LastRenewDate}", _dr["LastRenewDate"].ToString())
                            .Replace("{BillingAmount}", _dr["BillingAmount"].ToString())
                            .Replace("{ServiceType}", _dr["ServiceType"].ToString())
                            .Replace("{Description}", _dr["Description"].ToString())
                            ; ;
                        string emailContent = txtBody.Text;//.Replace("\n", "<br/>");
                        var mailContent = emailContent.Replace("{ContractNo}", _ref.ToString())
                            .Replace("{ExpirationDate}", _dr["ExpirationDate"].ToString())
                            .Replace("{LastRenewDate}", _dr["LastRenewDate"].ToString())
                            .Replace("{BillingAmount}", _dr["BillingAmount"].ToString())
                            .Replace("{ServiceType}", _dr["ServiceType"].ToString())
                            .Replace("{Description}", _dr["Description"].ToString())
                            ;
                        //var companySignature = ViewState["CompanyAddress"].ToString().Replace(Environment.NewLine, "<BR/>");
                        mail.Text = mailContent;
                        mail.IsIncludeSignature = true;


                        mail.RequireAutentication = false;

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
                                }
                            }
                            else
                            {
                                mimeSentMessages.Add(mimeMessage);
                                invoiceIdsSentEmail.Add("Contract #" + _ref.ToString());
                            }
                        }
                    }
                    else
                    {
                        totalSendErr++;
                        emailLog.To = string.Empty;
                        emailLog.Status = 0;
                        emailLog.UsrErrMessage = "Email address does not exist for this location";
                        BL_EmailLog bL_EmailLog = new BL_EmailLog();
                        bL_EmailLog.AddEmailLog(emailLog);
                    }
                }

                totalSentEmails = mimeSentMessages.Count;

                if (totalSentEmails > 0)
                {
                    Mail mail = new Mail();
                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                    if (mail.TakeASentEmailCopy)
                    {
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
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnSentError", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue:true});", true);
                    if (emailGetSentError != null)
                    {
                        string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyGetSentError", "noty({text: '" + str1 + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue:true});", true);
                    }
                }
                else
                {
                    if (totalSentEmails > 0)
                    {
                        var successfullMess = "There were " + totalSentEmails + " emails sent out successfully.";

                        if (totalSendErr > 0)
                        {
                            successfullMess += "<br>And " + totalSendErr + " could not be sent.";
                        }

                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue:true});", true);
                        if (emailGetSentError != null)
                        {
                            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyGetSentError", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue:true});", true);
                        }
                    }
                    else
                    {
                        string str = "There were no emails sent out.";

                        if (totalSendErr > 0)
                        {
                            str += "<br>Total " + totalSendErr + " could not be sent.";
                        }

                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoEmail", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue:true});", true);
                    }
                }

                if (sbdInValidEmails.Length > 0)
                {
                    sbdInValidEmails.Insert(0, "Invalid emails address:</br>");
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnInValidEmails", "noty({text: '" + sbdInValidEmails.ToString() + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', closable: false, dismissQueue: true});", true);
                }

            }
            catch (Exception exp)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue:true});", true);
            }


            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySendingEmail", "noty({text: 'There are " + ds.Tables[0].Rows.Count + " contracts will expire in arround 10 days',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            RadGrid_gvLogs.Rebind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarning", "noty({text: 'There is no expiration contracts in 10 days',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_gvLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        DataSet dsLog = new DataSet();
        EmailLog emailLog = new EmailLog();
        emailLog.Screen = "Contract";
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
}