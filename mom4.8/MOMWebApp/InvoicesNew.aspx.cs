using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using Microsoft.Reporting.WebForms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Threading;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Linq.Dynamic;
using Stimulsoft.Report;
using System.Collections;
using Stimulsoft.Report.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Text;

public partial class InvoicesNew : System.Web.UI.Page
{
    #region Variables
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();

    GeneralFunctions objGeneralFunction = new GeneralFunctions();

    Journal _objJe = new Journal();
    Transaction _objTrans = new Transaction();
    BL_JournalEntry _objBLJe = new BL_JournalEntry();

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    int count_inv = 0;
    bool _filteredbyloc = false;

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
    BL_Report bL_Report = new BL_Report();

    bool IsGst = false;

    byte[] buffer = null;
    byte[] array = null;
    Boolean isGridFilterInvoice = false;
    #endregion
    private bool IsGridPageIndexChanged = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            isSeco.Visible = false;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CustomerName"].ToString()) && ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("SECO"))
            {
                isSeco.Visible = true;
            }
            isNoneTS.Visible = true;
            isTS.Visible = false;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["isDBTotalService"]))
            {
                if (ConfigurationManager.AppSettings["isDBTotalService"].ToString().ToLower().Equals("true".ToLower()))
                {
                    isNoneTS.Visible = false;
                    isTS.Visible = true;
                }
            }


            if (SSL == "1")
            {
                bool isLocal = HttpContext.Current.Request.IsLocal;
                if (!isLocal)
                {
                    bool isSecure = HttpContext.Current.Request.IsSecureConnection;
                    string webPath = System.Web.Configuration.WebConfigurationManager.AppSettings["webPath"].Trim();
                    if (!isSecure)
                    {
                        if (Session["type"].ToString() == "c")
                        {
                            bool port = HttpContext.Current.Request.Url.IsDefaultPort;
                            string Auth = HttpContext.Current.Request.Url.Authority;
                            if (!port)
                            {
                                Auth = HttpContext.Current.Request.Url.DnsSafeHost;
                            }
                            string URL = Auth + webPath;
                            string redirect = "HTTPS://" + URL + "/Invoices.aspx";
                            int ii = 0;
                            foreach (String key in Request.QueryString.AllKeys)
                            {
                                if (ii == 0)
                                    redirect += "?" + key + "=" + Request.QueryString[key];
                                else
                                    redirect += "&" + key + "=" + Request.QueryString[key];
                                ii++;
                            }
                            Response.Redirect(redirect);
                        }
                    }
                }
            }

            if (!IsPostBack)
            {
                ViewState["RadGvTicketListminimumRows"] = 0;
                ViewState["RadGvTicketListmaximumRows"] = 50;
                ViewState["IncDays"] = 0;

                if (string.IsNullOrWhiteSpace(GetDefaultGridColumnSettingsFromDb()))
                {

                    BL_User objBL_User = new BL_User();
                    User objProp_User = new User();

                    // Get initial grid settings
                    var gridDefault = GetGridColumnSettings();
                    // Save default settings to database
                    objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
                    objProp_User.UserID = 0;// UserId = 0 for default
                    objProp_User.PageName = "Invoices.aspx";
                    objProp_User.GridId = "RadGrid_Invoice";

                    objBL_User.UpdateUserGridCustomSettings(objProp_User, gridDefault);
                }
                else
                {

                }

                #region Check IsGstRate
                objGeneral.ConnConfig = Session["config"].ToString();
                objGeneral.CustomName = "Country";
                DataSet dsCustom = objBL_General.getCustomFields(objGeneral);

                if (dsCustom.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
                    {
                        IsGst = true;
                    }
                }
                #endregion
                ViewState["IsGst"] = IsGst;
                ViewState["pay"] = 0;
                Locations();
                FillDepartment();
                //BindPaidUnPaid();
                BindPrintOnly();
                if (Request.QueryString["fil"] != null)
                {
                    UpdateControl();
                }



                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["InvfromDate"] == null && Session["InvToDate"] == null)
                    {
                        txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                        txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                        lblWeek.Attributes.Add("class", "labelactive");
                        Session["FreqToDate"] = txtToDate.Text;
                        Session["FreqfromDate"] = txtFromDate.Text;
                    }
                    else
                    {
                        txtFromDate.Text = Session["FreqfromDate"].ToString();
                        txtToDate.Text = Session["FreqToDate"].ToString();
                    }
                }
                else
                {
                    Session["InvfromDate"] = null;
                    Session["InvToDate"] = null;

                    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                    lblWeek.Attributes.Add("class", "labelactive");
                    Session["FreqToDate"] = txtToDate.Text;
                    Session["FreqfromDate"] = txtFromDate.Text;
                }


                if (Session["type"].ToString() == "c")
                {
                    GetInvoices(1);
                }

            }

            HighlightSideMenu("acctMgr", "lnkInvoicesSMenu", "billMgrSub");
            Permission();
            CompanyPermission();
            ConvertToJSON();
            if (!IsPostBack)
            {
                string InvoicesCheckpath = Server.MapPath("StimulsoftReports/Invoices/");
                DirectoryInfo dirPath = new DirectoryInfo(InvoicesCheckpath);
                FileInfo[] Files = dirPath.GetFiles("*.mrt"); //Getting Text files
                foreach (FileInfo file in Files)
                {
                    string FileName = string.Empty;
                    if (file.Name.Contains(".mrt"))
                        FileName = file.Name.Replace(".mrt", " ");
                    //if (FileName.TrimEnd() != "TopCheckReportDefault")
                    ddlInvoicesForLoad.Items.Add((FileName));
                }


                ddlInvoicesForLoad.DataBind();
            }

            string Report1 = string.Empty;
            string Report2 = string.Empty;
            lnk_InvoiceMaint.Visible = false;
            lnk_InvoiceException.Visible = false;
            Report1 = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
            Report2 = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
            if (Report1 == string.Empty || Report2 == string.Empty)
            {
                lnkMaintenance.Visible = false;
                lnkException.Visible = false;
            }

            if (Session["dbname"].ToString() == "adams")
            {
                lnkAdamMaintenance.Visible = true;

            }
            else
            {
                lnkAdamMaintenance.Visible = false;
            }


            if (Convert.ToString(Session["MailSend"]) == "true")
            {
                Session["MailSend"] = null;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            SetupCanadaCompanyUI();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'There is an error. Please try again later.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //this.Page.ClientScript.GetPostBackEventReference(lnkDelete, string.Empty);
    }

    protected void Page_Init(object source, System.EventArgs e)
    {
        DefineGridStructure();
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

    protected DataTable BuildCompanyDetailsTable()
    {
        DataTable companyDetailsTable = new DataTable();
        companyDetailsTable.Columns.Add("CompanyAddress");
        companyDetailsTable.Columns.Add("CompanyName");
        companyDetailsTable.Columns.Add("ContactNo");
        companyDetailsTable.Columns.Add("Email");
        companyDetailsTable.Columns.Add("LogoURL");
        companyDetailsTable.Columns.Add("Logo");
        companyDetailsTable.Columns.Add("City");
        companyDetailsTable.Columns.Add("State");
        companyDetailsTable.Columns.Add("Zip");
        companyDetailsTable.Columns.Add("Fax");
        companyDetailsTable.Columns.Add("Phone");
        companyDetailsTable.Columns.Add("GSTreg");

        return companyDetailsTable;
    }

    //private void BindPaidUnPaid()
    //{
    //    ddlpaidunpaid.Items.Add(new System.Web.UI.WebControls.ListItem("All", "All"));
    //    ddlpaidunpaid.Items.Add(new System.Web.UI.WebControls.ListItem("Open", "Open"));
    //    ddlpaidunpaid.Items.Add(new System.Web.UI.WebControls.ListItem("Paid", "Paid"));
    //    ddlpaidunpaid.Items.Add(new System.Web.UI.WebControls.ListItem("Voided", "Voided"));
    //}
    private void BindPrintOnly()
    {
        ddlPrintOnly.Items.Add(new System.Web.UI.WebControls.ListItem("All", "All"));
        ddlPrintOnly.Items.Add(new System.Web.UI.WebControls.ListItem("Print Only", "PrintOnly"));
        ddlPrintOnly.Items.Add(new System.Web.UI.WebControls.ListItem("Email", "Mail"));
    }
    private void Locations()
    {
        if (Session["type"].ToString() == "c")
        {
            DataTable dtcust = new DataTable();
            dtcust = (DataTable)Session["userinfo"];
            int RoleID = 0;
            if (dtcust.Rows.Count > 0)
            {
                RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                objPropUser.RoleID = RoleID;
            }
        }

        DataSet ds = new DataSet();
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(Session["custid"].ToString());
        if (Convert.ToInt32(Session["custid"].ToString()) != 0)
        {
            ds = objBL_User.getLocationByCustomerID(objPropUser);
        }
        else
        {
            ds = objBL_User.getLocations(objPropUser);
        }
        ddllocation.Items.Add(new System.Web.UI.WebControls.ListItem("Select Location", "0"));
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ddllocation.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(ds.Tables[0].Rows[i]["tag"]), Convert.ToString(ds.Tables[0].Rows[i]["loc"])));
        }
    }

    private void Permission()
    {

        if (Session["type"].ToString() == "c")
        {
            //Response.Redirect("addinvoiceNew.aspx?uid=" + Session["userid"].ToString());
            ddlSearch.Items[5].Enabled = false;
            lnkAddnew.Visible = false;
            lnkDelete.Visible = false;
            btnEdit.Text = "View Invoice";
            // pnlGridButtons.Visible = false;
            //LI1pnlGridButtons.Visible = false;
            //LI1pnlGridButtons.Visible = true;
            pnlGridButtons.Visible = true;
            LI1pnlGridButtons.Visible = false;
            LI2pnlGridButtons.Visible = true;
            btnEdit.Visible = false;
            lnkExcel.Visible = false;
            lnkPdf.InnerText = "PDF";
            lnkPdf.InnerHtml = "PDF";
            pnlPaymentList.Visible = true;
            btnCopy.Visible = false;
            btnVoidInvoice.Visible = false;
            lnkPrint.Visible = true;
            lnkPDFTI.Visible = false;
            lnkMailAll.Visible = false;
            ddlpaidunpaid.Visible = false;
            ddlPrintOnly.Visible = false;
            // lnkClose.Visible = false;
        }

        if (Session["MSM"].ToString() == "TS")
        {
            lnkAddnew.Visible = false;
            lnkDelete.Visible = false;
            btnEdit.Text = "View Invoice";
            if (Session["type"].ToString() != "c")
            { }
            //    Response.Redirect("home.aspx");
            ////pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

        if (Convert.ToInt16(Session["payment"]) != 1)
        {
            pnlPaymentList.Visible = false;
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

            /// Invoice ///////////////////------->

            string InvoicePermission = ds.Rows[0]["Invoice"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Invoice"].ToString();
            string ADD = InvoicePermission.Length < 1 ? "Y" : InvoicePermission.Substring(0, 1);
            string Edit = InvoicePermission.Length < 2 ? "Y" : InvoicePermission.Substring(1, 1);
            string Delete = InvoicePermission.Length < 3 ? "Y" : InvoicePermission.Substring(2, 1);
            string View = InvoicePermission.Length < 4 ? "Y" : InvoicePermission.Substring(3, 1);
            if (ADD == "N")
            {

                lnkAddnew.Visible = false;
                btnCopy.Visible = false;
            }
            if (Edit == "N")
            {
                btnEdit.Visible = false;

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
    private void CompanyPermission()
    {
        if (Session["COPer"] != null)
        {
            if (Session["COPer"].ToString() == "1")
            {
                // RadGrid_Invoice.Columns[7].Visible = true;
                //RadGrid_PaymentInv.Columns[11].Visible = true;
                RadGrid_Invoice.MasterTableView.GetColumn("Company").Display = true;
                RadGrid_PaymentInv.MasterTableView.GetColumn("Company").Display = true;

            }
            else
            {
                RadGrid_Invoice.MasterTableView.GetColumn("Company").Display = false;
                RadGrid_PaymentInv.MasterTableView.GetColumn("Company").Display = false;
                /*RadGrid_Invoice.Columns[7].Visible = false*/
                ;
                // RadGrid_PaymentInv.Columns[11].Visible = false;
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
    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getDepartment(objPropUser);

        ddlDepartment.DataSource = ds.Tables[0];
        ddlDepartment.DataTextField = "type";
        ddlDepartment.DataValueField = "id";
        ddlDepartment.DataBind();

        ddlDepartment.Items.Insert(0, new System.Web.UI.WebControls.ListItem(":: Select ::", ""));
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        String reportPath = "printinvoice.aspx?uid=";
        var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
        if (!reportFormat.ToUpper().Equals("RDLC"))
            reportPath = "PreviewInvoice.aspx?uid=";

        foreach (GridDataItem di in RadGrid_Invoice.SelectedItems)
        {
            Label lblUserID = (Label)di.FindControl("lblId");
            HiddenField hdnJobStatus = (HiddenField)di.FindControl("hdnJobStatus");
            //if (hdnJobStatus.Value == "1")
            //{
            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "EditInvoiceInProjectClose();", true);
            //}
            //else
            //{
            if (Session["type"].ToString() == "c")
            {
                Response.Redirect(reportPath + lblUserID.Text);
            }
            else
            {
                Response.Redirect("addinvoiceNew.aspx?uid=" + lblUserID.Text);
            }
            //}

        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem di in RadGrid_Invoice.SelectedItems)
            {
                Label lblId = (Label)di.FindControl("lblId");
                Response.Redirect("addinvoiceNew.aspx?uid=" + lblId.Text + "&c=1");
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            bool Flag = false;
            bool IsDelete = false;
            bool IsClosed = false;
            bool IsWipInvocie = false;
            bool IsProjectClose = false;
            int NRef = 0;
            foreach (GridDataItem di in RadGrid_Invoice.SelectedItems)
            {
                Label lblId = (Label)di.FindControl("lblId");
                TableCell cell = di["chkSelect"];
                CheckBox chkSelected = (CheckBox)cell.Controls[0];
                if (chkSelected.Checked == true)
                {
                    HiddenField hdnBatch = (HiddenField)di.FindControl("hdnBatch");
                    Label lblLoc = (Label)di.FindControl("lblLoc");
                    Label lblAmount = (Label)di.FindControl("lblAmount");
                    Label lblInvDate = (Label)di.FindControl("lblInvDate");
                    Label lblInvStatus = (Label)di.FindControl("lblInvStatus");
                    HiddenField hdnWipInvoice = (HiddenField)di.FindControl("hdnWipInvoice");
                    HiddenField hdnJobStatus = (HiddenField)di.FindControl("hdnJobStatus");
                    objProp_Contracts.Status = Convert.ToInt16(lblInvStatus.Text);

                    objProp_Contracts.ConnConfig = Session["config"].ToString();
                    objProp_Contracts.Ref = Convert.ToInt32(lblId.Text);
                    NRef = objProp_Contracts.Ref;
                    objProp_Contracts.Date = Convert.ToDateTime(lblInvDate.Text);

                    Flag = CommonHelper.GetPeriodDetails(objProp_Contracts.Date);
                    if (Flag)
                    {
                        if (hdnWipInvoice.Value != "0")
                        {
                            IsWipInvocie = true;
                        }
                        else
                        {
                            if (hdnJobStatus.Value == "1")
                            {
                                IsProjectClose = true;
                            }
                            else
                            {
                                if (objProp_Contracts.Status.Equals(0))
                                {
                                    objProp_Contracts.Batch = Convert.ToInt32(hdnBatch.Value);
                                    objProp_Contracts.Loc = Convert.ToInt32(lblLoc.Text);
                                    objProp_Contracts.Amount = Convert.ToDouble(lblAmount.Text);
                                    objBL_Contracts.DeleteInvoice(objProp_Contracts);
                                    IsDelete = true;
                                }
                                else
                                {
                                    IsClosed = true;
                                }
                            }

                        }

                    }
                    break;
                }

            }
            if (!Flag)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "displyDeleteAlert('delete');", true);
            }
            else
            {
                if (IsWipInvocie)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "WipInvWarning();", true);
                }
                else
                {
                    if (IsProjectClose)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "DeleteInvoiceInProjectClose();", true);
                    }
                    else
                    {
                        if (IsClosed)
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "ClosedInvoice('" + NRef + "');", true);
                        }
                        else
                        {
                            if (IsDelete)
                            {
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Invoice# " + NRef + " deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                            }

                            DataTable dt = (DataTable)Session["InvoiceSrch"];
                            DataRow dr = dt.Select("Ref = " + objProp_Contracts.Ref).SingleOrDefault();

                            if (dr != null)
                            {
                                dt.Rows.Remove(dr);
                            }
                            Session["InvoiceSrch"] = dt;
                            RadGrid_Invoice.DataSource = dt;
                            RadGrid_Invoice.Rebind();
                        }
                    }

                }

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addinvoiceNew.aspx");
    }

    private void GetInvoices(int Paid)
    {
        DataTable dtFilter = new DataTable();
        dtFilter.Columns.Add("ColumnName", typeof(string));
        dtFilter.Columns.Add("ColumnValue", typeof(string));

        List<RetainFilter> filters = new List<RetainFilter>();
        String filterExpression = Convert.ToString(RadGrid_Invoice.MasterTableView.FilterExpression);
        if (!IsPostBack)
        {
            if (filterExpression == "")
            {
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["Invoice_FilterExpression"] != null && Convert.ToString(Session["Invoice_FilterExpression"]) != "" && Session["Invoice_Filters"] != null)
                    {
                        filterExpression = Convert.ToString(Session["Invoice_FilterExpression"]);
                        RadGrid_Invoice.MasterTableView.FilterExpression = Convert.ToString(Session["Invoice_FilterExpression"]);
                        var filtersGet = Session["Invoice_Filters"] as List<RetainFilter>;
                        if (filtersGet != null)
                        {
                            foreach (var _filter in filtersGet)
                            {

                                var filterCol = _filter.FilterColumn;
                                if (filterCol == "Status")
                                {
                                    GridColumn column = RadGrid_Invoice.MasterTableView.GetColumnSafe("Status");

                                    if (column != null)
                                    {
                                        column.ListOfFilterValues = _filter.FilterValue.Replace("'", "").Split(',');
                                    }
                                }
                                else
                                {
                                    GridColumn column = RadGrid_Invoice.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                                    if (column != null)
                                    {
                                        column.CurrentFilterValue = _filter.FilterValue;
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {
                    Session["Invoice_FilterExpression"] = null;
                    Session["Invoice_Filters"] = null;
                }

            }
        }
        if (!IsGridPageIndexChanged)
        {
            RadGrid_Invoice.CurrentPageIndex = 0;
            Session["RadGrid_InvoiceCurrentPageIndex"] = 0;
            ViewState["RadGrid_InvoiceminimumRows"] = 0;
            ViewState["RadGrid_InvoicemaximumRows"] = RadGrid_Invoice.PageSize;
        }
        else
        {
            if (Session["RadGrid_InvoiceCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_InvoiceCurrentPageIndex"].ToString()) != 0
                && Request.QueryString["fil"] != null && Request.QueryString["fil"].ToString() == "1")
            {
                RadGrid_Invoice.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_InvoiceCurrentPageIndex"].ToString());
                ViewState["RadGrid_InvoiceminimumRows"] = RadGrid_Invoice.CurrentPageIndex * RadGrid_Invoice.PageSize;
                ViewState["RadGrid_InvoicemaximumRows"] = (RadGrid_Invoice.CurrentPageIndex + 1) * RadGrid_Invoice.PageSize;

            }
        }

        if (string.IsNullOrEmpty(filterExpression) && Session["Invoice_FilterExpression"] != null)
        {
            filterExpression = Convert.ToString(Session["Invoice_FilterExpression"]);
        }


        isGridFilterInvoice = false;

        if (filterExpression != "")
        {
            Session["Invoice_FilterExpression"] = filterExpression;
            foreach (GridColumn column in RadGrid_Invoice.MasterTableView.OwnerGrid.Columns)
            {

                String filterValues = String.Empty;
                String columnName = column.UniqueName;

                if (column.UniqueName == "Status")
                {
                    if (column.ListOfFilterValues != null)
                    {
                        List<string> listFil = new List<string>(column.ListOfFilterValues);
                        filterValues = String.Join(",", listFil.Select(x => string.Format("{0}", x)));
                        columnName = "Status";
                    }
                    else
                    {
                        filterValues = column.CurrentFilterValue;
                    }
                }
                else
                {
                    filterValues = column.CurrentFilterValue;
                }

                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                    if (column.UniqueName == "InvoiceRef")
                    {
                        isGridFilterInvoice = true;
                        objProp_Contracts.isGridFilterInvoice = true;
                    }
                    var cRow = dtFilter.NewRow();
                    cRow["ColumnName"] = columnName;
                    cRow["ColumnValue"] = filterValues;
                    dtFilter.Rows.Add(cRow);
                }

            }

            Session["Invoice_Filters"] = filters;
        }

        objProp_Contracts.dtFilterByColumn = dtFilter;


        _filteredbyloc = false;
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        if (Paid == 0)
        {
            objProp_Contracts.SearchBy = ddlSearch.SelectedValue;
            if (ddlSearch.SelectedValue == "i.Type")
            {
                objProp_Contracts.SearchValue = ddlDepartment.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "i.Status")
            {
                objProp_Contracts.SearchValue = ddlStatus.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "i.fdate")
            {
                objProp_Contracts.SearchValue = txtInvDt.Text;
            }
            else if (ddlSearch.SelectedValue == "l.loc")
            {
                objProp_Contracts.SearchValue = ddllocation.SelectedValue;
                _filteredbyloc = true;
            }
            else if (ddlSearch.SelectedValue == "i.ref")

            {
                if (txtSearch.Text != string.Empty)
                {


                    if ((txtSearch.Text[0] == '=' || txtSearch.Text[0] == '>' || txtSearch.Text[0] == '<' || txtSearch.Text[0] == 'b' || txtSearch.Text[0] == 'B'))
                    {
                        if (txtSearch.Text.Trim().Length >= 2)
                        {
                            txtSearch.Text = " " + txtSearch.Text;
                            objProp_Contracts.SearchValue = txtSearch.Text.Replace("'", "''");
                        }
                    }
                    else
                    {
                        if (txtSearch.Text.IndexOf('=') > -1 || txtSearch.Text.IndexOf('>') > -1 || txtSearch.Text.IndexOf('<') > -1)
                            txtSearch.Text = txtSearch.Text;
                        else
                            txtSearch.Text = "=" + txtSearch.Text;
                        objProp_Contracts.SearchValue = txtSearch.Text.Replace("'", "''");
                    }
                }
                else
                {
                    txtSearch.Text = "=-1";
                    objProp_Contracts.SearchValue = txtSearch.Text.Replace("'", "''");
                }
            }
            else
            {
                objProp_Contracts.SearchValue = txtSearch.Text.Replace("'", "''");
            }
            if (txtFromDate.Text != string.Empty)
            {
                objProp_Contracts.StartDate = Convert.ToDateTime(txtFromDate.Text);
            }
            else
            {
                objProp_Contracts.StartDate = System.DateTime.MinValue;
                objProp_Contracts.isShowAll = 1;
            }

            if (txtToDate.Text != string.Empty)
            {
                objProp_Contracts.EndDate = Convert.ToDateTime(txtToDate.Text);

            }
            else
            {
                objProp_Contracts.EndDate = System.DateTime.MinValue;
                objProp_Contracts.isShowAll = 1;
            }

            var typeSearchValue = string.Empty;
            if (ddlpaidunpaid.CheckedItems.Count > 0)
            {
                foreach (var item in ddlpaidunpaid.CheckedItems)
                {
                    typeSearchValue += item.Value + ",";
                }

                typeSearchValue = typeSearchValue.TrimEnd(',');

                objProp_Contracts.SearchAmtPaidUnpaid = "(" + typeSearchValue + ")";
            }
            if (ddlPrintOnly.SelectedValue == "All")
                objProp_Contracts.SearchPrintMail = string.Empty;
            else if (ddlPrintOnly.SelectedValue == "PrintOnly")
                objProp_Contracts.SearchPrintMail = "P";
            else if (ddlPrintOnly.SelectedValue == "Mail")
                objProp_Contracts.SearchPrintMail = "M";
        }
        else
        {
            objProp_Contracts.SearchValue = string.Empty;
            objProp_Contracts.SearchBy = string.Empty;
            objProp_Contracts.StartDate = System.DateTime.MinValue;
            objProp_Contracts.EndDate = System.DateTime.MinValue;
        }


        objProp_Contracts.CustID = Convert.ToInt32(Session["custid"].ToString());
        objProp_Contracts.Paid = Paid;

        if (Session["type"].ToString() == "c")
        {
            DataTable dtcust = new DataTable();
            dtcust = (DataTable)Session["userinfo"];
            int RoleID = 0;
            if (dtcust.Rows.Count > 0)
            {
                RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                objProp_Contracts.RoleId = RoleID;
            }
        }
        /****Get from MS_Invoice tables the invoices masrked as pending from Mobile Service in case of TS database****/
        if (Session["MSM"].ToString() == "TS")
        {
            if (Session["type"].ToString() != "c")
                objProp_Contracts.isTS = 1;
        }
        /***/
        #region Company Check
        objProp_Contracts.UserID = Session["UserID"].ToString();
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objProp_Contracts.EN = 1;
        }
        else
        {
            objProp_Contracts.EN = 0;
        }
        #endregion
        try
        {
            objProp_Contracts.PageNumber = RadGrid_Invoice.CurrentPageIndex + 1;
            objProp_Contracts.PageSize = RadGrid_Invoice.PageSize;
            objProp_Contracts.SortOrderBy = "";
            objProp_Contracts.SortType = "desc";
            if (RadGrid_Invoice.MasterTableView.SortExpressions.Count > 0)
            {
                objProp_Contracts.SortOrderBy = RadGrid_Invoice.MasterTableView.SortExpressions[0].FieldName;
                objProp_Contracts.SortType = RadGrid_Invoice.MasterTableView.SortExpressions[0].SortOrderAsString();
            }
            ds = objBL_Contracts.GetInvoicesPaging(objProp_Contracts);
            int totalRow = 0;
            if (ds.Tables[0].Rows.Count > 0)
                totalRow = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRow"]);


            RadGrid_Invoice.MasterTableView.VirtualItemCount = totalRow;
            lblRecordCount.Text = totalRow + " Record(s) found";

            //duplicate
            DataTable dt = RemoveDuplicateRows(ds.Tables[0], "Ref");
            if (Paid != 1)
            {
                BindGridDatatable(dt);
                if (isGridFilterInvoice == true)
                {

                    if (hdnHideDates.Value.ToString() == "1")
                    {
                        txtFromDate.Text = "";
                        txtToDate.Text = "";
                        txtSearch.Visible = true;
                        if (ddlSearch.SelectedValue != "i.ref")
                        {
                            txtSearch.Text = "";
                            ddlSearch.SelectedIndex = 0;
                        }
                        txtInvDt.Visible = false;

                        ddlDepartment.Style.Add("display", "none");
                        ddllocation.Style.Add("display", "none");
                        ddlUserType.Style.Add("display", "none");
                        ddlStatus.Style.Add("display", "none");

                        ddlpaidunpaid.SelectedIndex = 0;
                        ddlPrintOnly.SelectedIndex = 0;
                        lblDay.Attributes.Remove("class");
                        lblWeek.Attributes.Remove("class");
                        lblMonth.Attributes.Remove("class");
                        lblQuarter.Attributes.Remove("class");
                        lblYear.Attributes.Remove("class");
                    }
                    else
                    {
                        txtFromDate.Text = Convert.ToString(Session["FreqfromDate"]);
                        txtToDate.Text = Convert.ToString(Session["FreqToDate"]);
                    }
                }

            }
            else
            {
                BindPaymentGridDatatable(dt);
            }
            if (ds.Tables[0].Rows.Count > 0)
                totalRow = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRow"]);
            RadGrid_Invoice.AllowCustomPaging = true;
            lblRecordCount.Text = totalRow + " Record(s) found";
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            String type = "error";
            if (str.Contains("Incorrect syntax near") || str.Contains("Invalid column name"))
            {
                str = "Please input correct values";
                type = "warning";
            }

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : '" + type + "', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            RadGrid_PaymentInv.DataSource = String.Empty;//createEmptyTable();
            RadGrid_Invoice.DataSource = String.Empty;//createEmptyTable();
            lblRecordCount.Text = "0 Record(s) found";
        }
    }

    private DataTable RemoveDuplicateRows(DataTable dTable, string colName)
    {
        Hashtable hTable = new Hashtable();
        ArrayList duplicateList = new ArrayList();
        DataView dv = dTable.DefaultView;
        // dv.Sort = "PaymentReceivedDate desc";
        dTable = dv.ToTable();
        foreach (DataRow drow in dTable.Rows)
        {
            if (hTable.Contains(drow[colName]))
                duplicateList.Add(drow);
            else
                hTable.Add(drow[colName], string.Empty);
        }

        foreach (DataRow dRow in duplicateList)
            dTable.Rows.Remove(dRow);
        DataView dv1 = dTable.DefaultView;
        //dv1.Sort = "Ref asc";
        if (RadGrid_Invoice.MasterTableView.SortExpressions.Count > 0)
        {
            dv1.Sort = RadGrid_Invoice.MasterTableView.SortExpressions[0].ToString();
        }
        else
        {
            dv1.Sort = "Ref asc";
        }

        dTable = dv1.ToTable();
        return dTable;
    }

    private void BindGridDatatable(DataTable dt)
    {
        try
        {
            RadGrid_Invoice.VirtualItemCount = dt.Rows.Count;
            Session["InvoiceSrch"] = dt;
            RadGrid_Invoice.DataSource = dt;
            RadGrid_Invoice.MasterTableView.FilterExpression = string.Empty;
            RowSelect();
            if (Session["MSM"].ToString() == "TS")
            {
                RadGrid_Invoice.Columns[3].Visible = false;
            }
            if (CheckFilterDateWithGridDates(dt))
            {
                hdnHideDates.Value = "0";
            }
            else
            {
                hdnHideDates.Value = "1";
            }
        }
        catch (Exception ex)
        {

        }

    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Invoice.MasterTableView.FilterExpression != "" ||
            (RadGrid_Invoice.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Invoice.MasterTableView.SortExpressions.Count > 0;
    }
    public bool ShouldApplySortFilterOrGroupPayment()
    {
        return RadGrid_PaymentInv.MasterTableView.FilterExpression != "" ||
            (RadGrid_PaymentInv.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_PaymentInv.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Invoice_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //RadGrid_Invoice.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        //#region Set the Grid Filters
        //if (!IsPostBack)
        //{
        //    if (Convert.ToString(Request.QueryString["f"]) != "c")
        //    {
        //        if (Session["Invoice_FilterExpression"] != null && Convert.ToString(Session["Invoice_FilterExpression"]) != "" && Session["Invoice_Filters"] != null)
        //        {
        //            RadGrid_Invoice.MasterTableView.FilterExpression = Convert.ToString(Session["Invoice_FilterExpression"]);
        //            var filtersGet = Session["Invoice_Filters"] as List<RetainFilter>;
        //            if (filtersGet != null)
        //            {
        //                foreach (var _filter in filtersGet)
        //                {
        //                    GridColumn column = RadGrid_Invoice.MasterTableView.GetColumnSafe(_filter.FilterColumn);
        //                    column.CurrentFilterValue = _filter.FilterValue;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Session["Invoice_FilterExpression"] = null;
        //        Session["Invoice_Filters"] = null;
        //    }
        //}
        //#endregion



        SaveFilter();

        GetInvoices(0);


        UpdateControl();

        if (txtToDate.Text == "")
        {
            txtToDate.Text = Convert.ToString(Session["FreqToDate"]);
            txtFromDate.Text = Convert.ToString(Session["FreqfromDate"]);
        }

        if (isGridFilterInvoice == true)
        {
            if (hdnHideDates.Value.ToString() == "1")
            {
                txtToDate.Text = "";
                txtFromDate.Text = "";
            }
        }

    }
    //protected void RadGrid_Invoice_ItemEvent(object sender, GridItemEventArgs e)
    //{
    //    //int rowCount = 0;
    //    //if (e.EventInfo is GridInitializePagerItem)
    //    //{
    //    //    rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
    //    //}
    //    //lblRecordCount.Text = rowCount + " Record(s) found";
    //    //updpnl.Update();
    //}
    protected void RadGrid_Invoice_ItemCreated(object sender, GridItemEventArgs e)
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
        RadGrid_Invoice.ExportSettings.FileName = "Invoice";
        RadGrid_Invoice.ExportSettings.IgnorePaging = true;
        RadGrid_Invoice.ExportSettings.ExportOnlyData = true;
        RadGrid_Invoice.ExportSettings.OpenInNewWindow = true;
        RadGrid_Invoice.ExportSettings.HideStructureColumns = true;
        RadGrid_Invoice.MasterTableView.UseAllDataFields = true;
        RadGrid_Invoice.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Invoice.MasterTableView.ExportToExcel();
    }
    protected void RadGrid_Invoice_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Invoice.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Invoice.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
    protected void RadGrid_PaymentInv_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_PaymentInv.AllowCustomPaging = !ShouldApplySortFilterOrGroupPayment();
        GetInvoices(0);

    }
    private void RowSelect()
    {
        String reportPath = "location.href='printinvoice.aspx?uid=";
        var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
        if (!reportFormat.ToUpper().Equals("RDLC"))
            reportPath = "location.href='PreviewInvoice.aspx?uid=";

        foreach (GridDataItem gr in RadGrid_Invoice.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            HyperLink lblInv = (HyperLink)gr.FindControl("lblInv");

            if (ViewState["pay"].ToString() != "1" && lblInv != null)
            {
                if (Session["type"].ToString() == "c")
                {
                    gr.Attributes["ondblclick"] = reportPath + lblID.Text + "'";


                }
                else
                {
                    if (Session["MSM"].ToString() == "TS")
                        lblInv.Attributes["onclick"] = gr.Attributes["ondblclick"] = reportPath + lblID.Text + "'";
                    else
                        lblInv.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addinvoiceNew.aspx?uid=" + lblID.Text + "'";
                }
            }

            Label lblLocRemarkHover = (Label)gr.FindControl("lblLocRemarkHover");
            Label lblJobRemarkHover = (Label)gr.FindControl("lblJobRemarkHover");
            if (lblLocRemarkHover != null && lblLocRemarkHover.Text.Trim() != string.Empty)
            {
                var lblLocRemarkCELL = gr.Cells[gr.Cells.Count - 2];
                lblLocRemarkCELL.Attributes["onmousemove"] = "HoverMenutext('" + lblLocRemarkCELL.ClientID + "','" + lblLocRemarkHover.ClientID + "',event);";
                lblLocRemarkCELL.Attributes["onmouseout"] = " $('#" + lblLocRemarkHover.ClientID + "').hide();";
            }
            if (lblJobRemarkHover != null && lblJobRemarkHover.Text.Trim() != string.Empty)
            {
                var lblJobRemarkCELL = gr.Cells[gr.Cells.Count - 1];
                lblJobRemarkCELL.Attributes["onmousemove"] = "HoverMenutext('" + lblJobRemarkCELL.ClientID + "','" + lblJobRemarkHover.ClientID + "',event);";
                lblJobRemarkCELL.Attributes["onmouseout"] = " $('#" + lblJobRemarkHover.ClientID + "').hide();";
            }
        }
    }
    protected void RadGrid_Invoice_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BL_User objBL_User = new BL_User();
            User objProp_User = new User();

            DataSet ds = new DataSet();
            objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            objProp_User.PageName = "Invoices.aspx";
            objProp_User.GridId = "RadGrid_Invoice";
            ds = objBL_User.GetGridUserSettings(objProp_User);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //string columnSettings = "[{Name: \"BType\", Display: true, Width: 300},{Name: \"MatItem\", Display: false, Width: 300}]";
                var columnSettings = ds.Tables[0].Rows[0][0].ToString();
                var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

                var colIndex = 0;

                foreach (GridColumn column in RadGrid_Invoice.MasterTableView.OwnerGrid.Columns)
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

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showhidebutton", "ShowRestoreGridSettingsButton();", true);
            }
        }


        RowSelect();
    }
    private void BindPaymentGridDatatable(DataTable dt)
    {
        RadGrid_PaymentInv.VirtualItemCount = dt.Rows.Count;
        RadGrid_PaymentInv.DataSource = dt;
        lblCountPayment.Text = dt.Rows.Count.ToString() + " Record(s) Found.";
        if (Session["MSM"].ToString() == "TS")
        {
            RadGrid_PaymentInv.Columns[3].Visible = false;
        }
    }


    protected void lnkShowAll_Click(object sender, EventArgs e)
    {

        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        Session["InvToDate"] = txtToDate.Text;
        Session["InvfromDate"] = txtFromDate.Text;
        Session["FreqToDate"] = txtToDate.Text;
        Session["FreqfromDate"] = txtFromDate.Text;
        ddlSearch.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        ddllocation.SelectedIndex = 0;
        ddlUserType.SelectedIndex = 0;
        txtInvDt.Text = "";
        txtSearch.Text = "";
        ddlpaidunpaid.ClearCheckedItems();
        ddlPrintOnly.SelectedIndex = 0;
        if (Session["Invoice_FilterExpression"] != null && Convert.ToString(Session["Invoice_FilterExpression"]) != "" && Session["Invoice_Filters"] != null)
        {

            foreach (GridColumn column in RadGrid_Invoice.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
                column.ListOfFilterValues = null;
            }
            RadGrid_Invoice.MasterTableView.FilterExpression = string.Empty;
        }
        Session["Invoice_FilterExpression"] = null;
        Session["Invoice_Filters"] = null;


        GetInvoices(0);
        RadGrid_Invoice.Rebind();
        isShowAll.Value = "1";
        SaveFilter();
        _filteredbyloc = false;
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        lblDay.Attributes.Remove("class");
        lblWeek.Attributes.Remove("class");
        lblMonth.Attributes.Remove("class");
        lblQuarter.Attributes.Remove("class");
        lblYear.Attributes.Remove("class");
        showFilterSearch();
        IsGridPageIndexChanged = false;
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        isShowAll.Value = "0";
        hdnInvoiceSelectDtRange.Value = "";
        lblDay.Attributes.Remove("class");
        lblWeek.Attributes.Remove("class");
        lblMonth.Attributes.Remove("class");
        lblQuarter.Attributes.Remove("class");
        lblYear.Attributes.Remove("class");
        if (txtToDate.Text == "")
        {
            txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
        }
        Session["InvToDate"] = txtToDate.Text;
        Session["InvfromDate"] = txtFromDate.Text;
        Session["FreqToDate"] = txtToDate.Text;
        Session["FreqfromDate"] = txtFromDate.Text;
        SaveFilter();
        GetInvoices(0);
        RadGrid_Invoice.Rebind();
        //showFilterSearch();
        UpdateControl();

        if (txtToDate.Text == "")
        {
            txtToDate.Text = Convert.ToString(Session["FreqToDate"]);
            txtFromDate.Text = Convert.ToString(Session["FreqfromDate"]);
        }

        if (isGridFilterInvoice == true)
        {
            if (hdnHideDates.Value.ToString() == "1")
            {
                txtToDate.Text = "";
                txtFromDate.Text = "";
            }
        }
    }

    protected void lnkSearchFilter_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        isShowAll.Value = "0";
        Session["InvToDate"] = txtToDate.Text;
        Session["InvfromDate"] = txtFromDate.Text;
        Session["FreqToDate"] = txtToDate.Text;
        Session["FreqfromDate"] = txtFromDate.Text;

        SaveFilter();
        GetInvoices(0);
        RadGrid_Invoice.Rebind();
        UpdateControl();

        if (txtToDate.Text == "")
        {
            txtToDate.Text = Convert.ToString(Session["FreqToDate"]);
            txtFromDate.Text = Convert.ToString(Session["FreqfromDate"]);
        }

        if (isGridFilterInvoice == true)
        {
            if (hdnHideDates.Value.ToString() == "1")
            {
                txtToDate.Text = "";
                txtFromDate.Text = "";
            }
        }
    }
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        Session["filterstate"] = null;

        if (Session["Invoice_FilterExpression"] != null && Convert.ToString(Session["Invoice_FilterExpression"]) != "" && Session["Invoice_Filters"] != null)
        {

            foreach (GridColumn column in RadGrid_Invoice.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
                column.ListOfFilterValues = null;
            }
            RadGrid_Invoice.MasterTableView.FilterExpression = string.Empty;
        }


        Session["Invoice_FilterExpression"] = null;
        Session["Invoice_Filters"] = null;
        if (isShowAll.Value == "1")
        {
            txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();

            resetShowAll();
        }
        else
        {
            txtFromDate.Text = Session["FreqfromDate"].ToString();
            txtToDate.Text = Session["FreqToDate"].ToString();

            resetClear();
        }
        // isShowAll.Value = "0";
        ddlSearch.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        ddllocation.SelectedIndex = 0;
        ddlUserType.SelectedIndex = 0;
        // ddlSuper.SelectedIndex = 0;
        txtInvDt.Text = "";
        txtSearch.Text = "";
        ddlpaidunpaid.ClearCheckedItems();
        ddlPrintOnly.SelectedIndex = 0;


        _filteredbyloc = false;
        RadGrid_Invoice.Rebind();
        if (isShowAll.Value == "1")
        {
            txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
        }
        else
        {
            txtFromDate.Text = Session["FreqfromDate"].ToString();
            txtToDate.Text = Session["FreqToDate"].ToString();
        }
        isShowAll.Value = "0";
        Session["FreqToDate"] = txtToDate.Text;
        Session["FreqfromDate"] = txtFromDate.Text;
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
                    case "System.Web.UI.WebControls.HiddenField":
                        ((HiddenField)c).Value = "";
                        break;
                }
            }
        }
    }

    private void SaveFilter()
    {

        var paidunpaid = string.Empty;
        if (ddlpaidunpaid.CheckedItems.Count > 0)
        {
            foreach (var item in ddlpaidunpaid.CheckedItems)
            {
                paidunpaid += item.Value + ",";
            }

            paidunpaid = paidunpaid.TrimEnd(',');

        }



        Session["filterstate"] = ddlSearch.SelectedValue + ";"
            + ddllocation.SelectedValue + ";"
            + ddlStatus.SelectedValue + ";"
            + ddlDepartment.SelectedValue + ";"
            + txtInvDt.Text + ";" + txtSearch.Text + ";"
            + txtFromDate.Text + ";"
            + txtToDate.Text + ";"
            + ddlpaidunpaid + ";"
            + paidunpaid + ";"
            + ddlPrintOnly.SelectedValue + ";"
            + hdnInvoiceSelectDtRange.Value + ";"
            + isShowAll.Value;

    }
    protected void lnkMakePayment_Click(object sender, EventArgs e)
    {
        PaymentRedirect(false);
    }

    private string CheckAmountExceed(List<Dictionary<string, string>> lstInv, string invlist)
    {
        string discard = string.Empty;
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.InvoiceIDCustom = invlist;
        DataSet ds = objBL_Contracts.GetInvoicesAmount(objProp_Contracts);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            foreach (Dictionary<string, string> dict in lstInv)
            {
                if (string.Equals(dr["ref"].ToString(), dict["inv"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (Convert.ToDouble(dict["amt"]) > Convert.ToDouble(dr["balance"]))
                    {
                        discard = dr["ref"].ToString();
                    }
                }
            }
        }
        return discard;
    }

    private List<byte[]> PrintInvoices(StiWebViewer rvInvoices)
    {
        // Export to PDF
        List<byte[]> invoicesAsBytes = new List<byte[]>();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            foreach (DataRow _dr in dtNew.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);

                objProp_Contracts.InvoiceID = _ref;
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                _dtInvoice = ds.Tables[0];
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

                int _days = 0;
                for (int i = 0; i < _dtInvoice.Rows.Count; i++)
                {

                    #region Determine Pay Terms
                    if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                    {
                        _days = 0;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                    {
                        _days = 10;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                    {
                        _days = 15;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                    {
                        _days = 30;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                    {
                        _days = 45;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                    {
                        _days = 60;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                    {
                        _days = 30;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                    {
                        _days = 90;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                    {
                        _days = 180;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                    {
                        _days = 0;
                    }
                    #endregion
                    if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                    {
                        _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                        _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                    }
                }
                #region Get Company Address
                string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
                address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
                address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
                address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
                if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
                {
                    address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
                        "Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
                        "dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
                        "n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

                        "Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


                        "Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

                        "Please review the attached invoice(s) for processing." + Environment.NewLine +
                        "Please note there may be multiple invoices contained " + Environment.NewLine +
                        "in each attachment. Should you have any questions, " + Environment.NewLine +
                        "Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
                        "We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

                }
                else
                {
                    string extContentFilename = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempPDF/" + "InvoicesEmailAll_ExtraContent.txt");
                    if (File.Exists(extContentFilename))
                    {
                        string[] lines = System.IO.File.ReadAllLines(extContentFilename);
                        string extContent = "";
                        foreach (var item in lines)
                        {
                            extContent += item + Environment.NewLine;
                        }

                        address = extContent + "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
                    }
                    else
                    {
                        address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
                    }
                }

                ViewState["CompanyAddress"] = address;

                ViewState["EmailFrom"] = "";
                if (Session["MSM"].ToString() != "TS")
                {
                    ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
                }
                #endregion
                ViewState["InvoiceReport"] = _dtInvoice;
                ViewState["CompanyReport"] = dsC.Tables[0];
                Session["InvoiceReportDetails"] = _dtInvoice;

                //rvInvoices.LocalReport.DataSources.Clear();
                DataTable dt = (DataTable)ViewState["InvoiceReport"];
                DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
                int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
                DataTable _dtInvItems1 = GetInvoiceItems(refId);

                string reportPathStimul = string.Empty;

                reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + ConfigurationManager.AppSettings["InvoiceReport"].ToString());
                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                //report.Compile();

                DataSet companyLogo = new DataSet();
                companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());

                DataTable cTable = BuildCompanyDetailsTable();
                var cRow = cTable.NewRow();
                //cRow["LogoURL"] = uri.AbsolutePath;
                cRow["CompanyName"] = companyLogo.Tables[0].Rows[0]["Name"].ToString();
                cRow["CompanyAddress"] = companyLogo.Tables[0].Rows[0]["Address"].ToString();
                cRow["ContactNo"] = companyLogo.Tables[0].Rows[0]["Contact"].ToString();
                cRow["Email"] = companyLogo.Tables[0].Rows[0]["Email"].ToString();
                cRow["City"] = companyLogo.Tables[0].Rows[0]["City"].ToString();
                cRow["State"] = companyLogo.Tables[0].Rows[0]["State"].ToString();
                cRow["Phone"] = companyLogo.Tables[0].Rows[0]["Phone"].ToString();
                cRow["Fax"] = companyLogo.Tables[0].Rows[0]["Fax"].ToString();
                cRow["Zip"] = companyLogo.Tables[0].Rows[0]["Zip"].ToString();
                cRow["GSTreg"] = companyLogo.Tables[0].Rows[0]["GSTreg"].ToString();
                cTable.Rows.Add(cRow);

                report.RegData("Invoices", _dtInvoice);
                report.RegData("CompanyDetails", cTable);
                report.RegData("Invoice_dtInvoice", ds.Tables[0]);
                report.RegData("Ticket_Company", dsC.Tables[0]);
                report.RegData("InvoiceItems", _dtInvItems1);
                report.Dictionary.Synchronize();
                report.Render();
                rvInvoices.Report = report;

                byte[] buffer1 = null;
                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(rvInvoices.Report, stream, settings);
                buffer1 = stream.ToArray();
                invoicesAsBytes.Add(buffer1);

                j++;
            }

            return invoicesAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return invoicesAsBytes;
        }
    }
    private byte[] ExportReportToPDF1(string reportName, ReportViewer reportviewer1)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        reportviewer1.ProcessingMode = ProcessingMode.Local;
        byte[] bytes = reportviewer1.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;

        //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", reportName);
        //using (var fs = new FileStream(filename, FileMode.Create))
        //{
        //    fs.Write(bytes, 0, bytes.Length);
        //    fs.Close();
        //}

        //return filename;
    }

    private void GenerateReport(ReportViewer rv, DataTable dtInvoice)
    {
        DataTable dtCompany = new DataTable();
        if (ViewState["RecurCompany"] == null)
        {
            DataSet dsCompany = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            //objPropUser.DBName = Session["dbname"].ToString();
            //objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
            //DataSet dsloc = new DataSet();
            //dsloc = objBL_User.getLocationByID(objPropUser);

            //if (Session["MSM"].ToString() != "TS")
            //{
            dsCompany = objBL_User.getControl(objPropUser);
            //}
            //else
            //{
            //    objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
            //    dsCompany = objBL_User.getControlBranch(objPropUser);
            //}
            ViewState["RecurCompany"] = dsCompany.Tables[0];
            dtCompany = dsCompany.Tables[0];
        }
        else
        {
            dtCompany = (DataTable)ViewState["RecurCompany"];
        }

        foreach (DataRow dr in dtInvoice.Rows)
        {
            //billTo = Regex.Replace(billTo, @"( |\r?\n)\1+", "$1");  // to remove first new line.
            string billTo = Regex.Replace(dr["Billto"].ToString(), @"\t|\n|\r", "");          // to remove all new lines.
            billTo = Regex.Replace(billTo, @"^,+|,+$|,+(,\w)", "$1");
            billTo = billTo.Split(new[] { ',' }, 2).First() + ",\n" + billTo.Split(new[] { ',' }, 2).Last();
            dr["Billto"] = billTo;
        }

        rv.LocalReport.DataSources.Clear();  //added by dev 15th march, 16

        rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemSubReportProcessing);

        string sessval = (string)Session["InvoiceName"];
        string Report = string.Empty;

        if (sessval == "Invoice")
        {
            Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesReport"].Trim();
        }

        if (sessval == "InvoiceMaint")
        {
            Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
        }

        if (sessval == "InvoiceException")
        {
            Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
        }

        if (sessval == "Invoice-LNY")
        {
            Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNY"].Trim();
        }


        if (Report == "Madden_Invoices.rdlc" || Report == "InvoicesAdams.rdlc" || Report == string.Empty || Report == "InvoicesInFrench.rdlc" || Report == "ReportInvoicePME.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", dtInvoice));
            }
            else
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", dtInvoice));
            }
        }
        else if (Report == "PESMTC_InvoicesMaint.rdlc" || Report == "PESMTC_InvoicesExceptions.rdlc" || Report == "Invoice-LNY.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_PESdtInvoice", dtInvoice));
            }
        }

        rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dtCompany));

        string reportPath = string.Empty;

        if (sessval == "Invoice")
        {
            reportPath = "Reports/Invoices.rdlc";

            if (Report == "Madden_Invoices.rdlc" || Report == "InvoicesInFrench.rdlc")
            {
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    reportPath = "Reports/" + Report.Trim();
                }
            }
        }
        else if (sessval == "InvoiceMaint")
        {
            if (Report == "PESMTC_InvoicesMaint.rdlc")
            {
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    reportPath = "Reports/" + Report.Trim();
                }
            }
        }
        else if (sessval == "InvoiceException")
        {
            if (Report == "PESMTC_InvoicesExceptions.rdlc")
            {
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    reportPath = "Reports/" + Report.Trim();
                }
            }
        }

        else if (sessval == "Invoice-LNY")
        {
            if (Report == "Invoice-LNY.rdlc")
            {
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    reportPath = "Reports/" + Report.Trim();
                }
            }
        }

        if (Report == "InvoicesAdams.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
        }
        rv.LocalReport.ReportPath = reportPath;

        rv.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
        if (Report == "InvoicesInFrench.rdlc" || Report == "InvoicesAdams.rdlc" || Report == "")
        {
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
        }
        rv.LocalReport.SetParameters(param1);

        rv.LocalReport.Refresh();

    }

    private List<byte[]> PrintInvoicesForIndivudial(StiWebViewer rvInvoices, DataRow _dr)
    {
        // Export to PDF
        List<byte[]> invoicesAsBytes = new List<byte[]>();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            //foreach (DataRow _dr in dtNew.Rows)
            //{
            int _ref = Convert.ToInt32(_dr["Ref"]);

            objProp_Contracts.InvoiceID = _ref;
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

            _dtInvoice = ds.Tables[0];
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

            int _days = 0;
            for (int i = 0; i < _dtInvoice.Rows.Count; i++)
            {

                #region Determine Pay Terms
                if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                {
                    _days = 0;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                {
                    _days = 10;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                {
                    _days = 15;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                {
                    _days = 45;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                {
                    _days = 60;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                {
                    _days = 90;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                {
                    _days = 180;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                {
                    _days = 0;
                }
                #endregion
                if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                {
                    _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                    _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                }
            }
            #region Get Company Address

            string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
            if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
            {
                address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
                    "Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
                    "dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
                    "n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

                    "Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


                    "Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

                    "Please review the attached invoice(s) for processing." + Environment.NewLine +
                    "Please note there may be multiple invoices contained " + Environment.NewLine +
                    "in each attachment. Should you have any questions, " + Environment.NewLine +
                    "Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
                    "We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

            }
            else
            {
                string extContentFilename = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempPDF/" + "InvoicesEmailAll_ExtraContent.txt");
                if (File.Exists(extContentFilename))
                {
                    string[] lines = System.IO.File.ReadAllLines(extContentFilename);
                    string extContent = "";
                    foreach (var item in lines)
                    {
                        extContent += item + Environment.NewLine;
                    }

                    address = extContent + "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
                }
                else
                {
                    address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
                }
            }

            ViewState["CompanyAddress"] = address;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }
            #endregion
            ViewState["InvoiceReport"] = _dtInvoice;
            ViewState["CompanyReport"] = dsC.Tables[0];
            Session["InvoiceReportDetails"] = _dtInvoice;

            //rvInvoices.LocalReport.DataSources.Clear();
            DataTable dt = (DataTable)ViewState["InvoiceReport"];
            DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
            int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
            DataTable _dtInvItems1 = GetInvoiceItems(refId);

            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + ConfigurationManager.AppSettings["InvoiceReport"].ToString());
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            DataSet companyLogo = new DataSet();
            companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            var imageString = companyLogo.Tables[0].Rows[0]["Logo"].ToString();
            byte[] barrImg = (byte[])(companyLogo.Tables[0].Rows[0]["Logo"]);
            string strfn = Convert.ToString(Server.MapPath(Request.ApplicationPath + "/TempImages/" + DateTime.Now.ToFileTime().ToString()));
            FileStream fs = new FileStream(strfn,
                              FileMode.CreateNew, FileAccess.Write);
            fs.Write(barrImg, 0, barrImg.Length);
            fs.Flush();
            fs.Close();

            System.Uri uri = new Uri(strfn);
            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();
            cRow["LogoURL"] = uri.AbsolutePath;
            cRow["CompanyName"] = companyLogo.Tables[0].Rows[0]["Name"].ToString();
            cRow["CompanyAddress"] = companyLogo.Tables[0].Rows[0]["Address"].ToString();
            cRow["ContactNo"] = companyLogo.Tables[0].Rows[0]["Contact"].ToString();
            cRow["Email"] = companyLogo.Tables[0].Rows[0]["Email"].ToString();

            cRow["City"] = companyLogo.Tables[0].Rows[0]["City"].ToString();
            cRow["State"] = companyLogo.Tables[0].Rows[0]["State"].ToString();
            cRow["Phone"] = companyLogo.Tables[0].Rows[0]["Phone"].ToString();
            cRow["Fax"] = companyLogo.Tables[0].Rows[0]["Fax"].ToString();
            cRow["Zip"] = companyLogo.Tables[0].Rows[0]["Zip"].ToString();

            cTable.Rows.Add(cRow);

            DataSet CompanyDetails = new DataSet();
            cTable.TableName = "CompanyDetails";
            CompanyDetails.Tables.Add(cTable);
            CompanyDetails.DataSetName = "CompanyDetails";

            DataSet Invoices = new DataSet();
            DataTable dtInvoice1 = _dtInvoice.Copy();
            dtInvoice1.TableName = "Invoices";
            Invoices.Tables.Add(dtInvoice1.Copy());
            Invoices.DataSetName = "Invoices";

            DataSet InvoiceItems = new DataSet();
            DataTable dtIInvItems = _dtInvItems1.Copy();
            dtIInvItems.TableName = "InvoiceItems";
            InvoiceItems.Tables.Add(dtIInvItems);
            InvoiceItems.DataSetName = "InvoiceItems";

            DataSet Ticket_Company = new DataSet();
            DataTable dtTicketCompany = new DataTable();
            dtTicketCompany = dsC.Tables[0].Copy();
            Ticket_Company.Tables.Add(dtTicketCompany);
            dtTicketCompany.TableName = "Ticket_Company";
            Ticket_Company.DataSetName = "Ticket_Company";

            DataSet Invoice_dtInvoice = new DataSet();
            DataTable dtInvoice = new DataTable();
            dtInvoice = ds.Tables[0].Copy();
            Invoice_dtInvoice.Tables.Add(dtInvoice);
            dtInvoice.TableName = "Invoice_dtInvoice";
            Invoice_dtInvoice.DataSetName = "Invoice_dtInvoice";

            report.RegData("Invoices", Invoices);
            report.RegData("CompanyDetails", CompanyDetails);

            report.RegData("Invoice_dtInvoice", Invoice_dtInvoice);

            report.RegData("Ticket_Company", Ticket_Company);
            report.RegData("InvoiceItems", InvoiceItems);
            report.Dictionary.Synchronize();
            report.Render();
            rvInvoices.Report = report;
            byte[] buffer1 = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(rvInvoices.Report, stream, settings);
            buffer1 = stream.ToArray();
            invoicesAsBytes.Add(buffer1);

            j++;

            return invoicesAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return invoicesAsBytes;
        }
    }

    private List<byte[]> PrintInvoicesForIndivudialRDLC(ReportViewer rvInvoices, DataRow _dr)
    {
        // Export to PDF
        List<byte[]> invoicesAsBytes = new List<byte[]>();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            int _ref = Convert.ToInt32(_dr["Ref"]);

            objProp_Contracts.InvoiceID = _ref;
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

            _dtInvoice = ds.Tables[0];
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

            int _days = 0;
            for (int i = 0; i < _dtInvoice.Rows.Count; i++)
            {

                #region Determine Pay Terms
                if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                {
                    _days = 0;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                {
                    _days = 10;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                {
                    _days = 15;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                {
                    _days = 45;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                {
                    _days = 60;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                {
                    _days = 90;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                {
                    _days = 180;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                {
                    _days = 0;
                }
                #endregion
                if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                {
                    _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                    _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                }
            }
            #region Get Company Address

            string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
            if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
            {
                address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
"Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
"dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
"n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

"Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


"Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

"Please review the attached invoice(s) for processing." + Environment.NewLine +
"Please note there may be multiple invoices contained " + Environment.NewLine +
"in each attachment. Should you have any questions, " + Environment.NewLine +
"Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
"We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

            }
            else
            {
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
            }

            ViewState["CompanyAddress"] = address;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }
            #endregion
            ViewState["InvoiceReport"] = _dtInvoice;
            ViewState["CompanyReport"] = dsC.Tables[0];
            Session["InvoiceReportDetails"] = _dtInvoice;

            rvInvoices.LocalReport.DataSources.Clear();
            DataTable dt = (DataTable)ViewState["InvoiceReport"];
            DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
            int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
            DataTable _dtInvItems1 = GetInvoiceItems(refId);


            rvInvoices.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", _dtInvoice));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));

            string reportPath = "Reports/Invoices.rdlc";
            string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceDetailsReport"].Trim();
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
            string eventTarget = this.Request.Params.Get("__EVENTTARGET");
            if (eventTarget.Contains("lnkAdamMaintenance"))
            {
                reportPath = "Reports/InvoicesForAdamMaintenance.rdlc";
            }
            if (eventTarget.Contains("lnkAdamBilling"))
            {
                reportPath = "Reports/InvoicesForAdamBill.rdlc";
            }

            rvInvoices.LocalReport.ReportPath = reportPath;

            rvInvoices.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            if (string.IsNullOrEmpty(Report.Trim()) || reportPath == "Reports/InvoicesForAdamMaintenance.rdlc" || reportPath == "Reports/InvoicesAdams.rdlc")
            {
                param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
            }


            rvInvoices.LocalReport.SetParameters(param1);

            rvInvoices.LocalReport.Refresh();


            DataSet companyLogo = new DataSet();
            companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            var imageString = companyLogo.Tables[0].Rows[0]["Logo"].ToString();
            byte[] barrImg = (byte[])(companyLogo.Tables[0].Rows[0]["Logo"]);
            string strfn = Convert.ToString(Server.MapPath(Request.ApplicationPath + "/TempImages/" + DateTime.Now.ToFileTime().ToString()));
            FileStream fs = new FileStream(strfn,
                              FileMode.CreateNew, FileAccess.Write);
            fs.Write(barrImg, 0, barrImg.Length);
            fs.Flush();
            fs.Close();

            System.Uri uri = new Uri(strfn);
            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();
            cRow["LogoURL"] = uri.AbsolutePath;
            cRow["CompanyName"] = companyLogo.Tables[0].Rows[0]["Name"].ToString();
            cRow["CompanyAddress"] = companyLogo.Tables[0].Rows[0]["Address"].ToString();
            cRow["ContactNo"] = companyLogo.Tables[0].Rows[0]["Contact"].ToString();
            cRow["Email"] = companyLogo.Tables[0].Rows[0]["Email"].ToString();

            cRow["City"] = companyLogo.Tables[0].Rows[0]["City"].ToString();
            cRow["State"] = companyLogo.Tables[0].Rows[0]["State"].ToString();
            cRow["Phone"] = companyLogo.Tables[0].Rows[0]["Phone"].ToString();
            cRow["Fax"] = companyLogo.Tables[0].Rows[0]["Fax"].ToString();
            cRow["Zip"] = companyLogo.Tables[0].Rows[0]["Zip"].ToString();


            cTable.Rows.Add(cRow);

            DataSet CompanyDetails = new DataSet();
            cTable.TableName = "CompanyDetails";
            CompanyDetails.Tables.Add(cTable);
            CompanyDetails.DataSetName = "CompanyDetails";


            DataSet Invoices = new DataSet();
            DataTable dtInvoice1 = _dtInvoice.Copy();
            dtInvoice1.TableName = "Invoices";
            Invoices.Tables.Add(dtInvoice1.Copy());
            Invoices.DataSetName = "Invoices";

            DataSet InvoiceItems = new DataSet();
            DataTable dtIInvItems = _dtInvItems1.Copy();
            dtIInvItems.TableName = "InvoiceItems";
            InvoiceItems.Tables.Add(dtIInvItems);
            InvoiceItems.DataSetName = "InvoiceItems";


            DataSet Ticket_Company = new DataSet();
            DataTable dtTicketCompany = new DataTable();
            dtTicketCompany = dsC.Tables[0].Copy();
            Ticket_Company.Tables.Add(dtTicketCompany);
            dtTicketCompany.TableName = "Ticket_Company";
            Ticket_Company.DataSetName = "Ticket_Company";


            DataSet Invoice_dtInvoice = new DataSet();
            DataTable dtInvoice = new DataTable();
            dtInvoice = ds.Tables[0].Copy();
            Invoice_dtInvoice.Tables.Add(dtInvoice);
            dtInvoice.TableName = "Invoice_dtInvoice";
            Invoice_dtInvoice.DataSetName = "Invoice_dtInvoice";

            byte[] buffer1 = null;
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            buffer1 = ExportReportToPDF("", rvInvoices);
            invoicesAsBytes.Add(buffer1);
            j++;
            return invoicesAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return invoicesAsBytes;
        }
    }
    private List<byte[]> PrintInvoices(ReportViewer rvInvoices)
    {
        // Export to PDF
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            foreach (DataRow _dr in dtNew.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);

                objProp_Contracts.InvoiceID = _ref;
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                if (j > 0)
                {
                    _dtInvoice.Merge(ds.Tables[0], true);
                }
                else
                {
                    _dtInvoice = ds.Tables[0];
                }
                j++;
            }
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

            int _days = 0;
            for (int i = 0; i < _dtInvoice.Rows.Count; i++)
            {
                //_dtInvoice.Rows[i]["iAmount"] = _dtInvoice.Rows[i]["Total"];
                //_dtInvoice.Rows[i]["iSTax"] = _dtInvoice.Rows[i]["STax"];

                #region Determine Pay Terms
                if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                {
                    _days = 0;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                {
                    _days = 10;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                {
                    _days = 15;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                {
                    _days = 45;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                {
                    _days = 60;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                {
                    _days = 90;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                {
                    _days = 180;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                {
                    _days = 0;
                }
                #endregion
                if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                {
                    _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                    _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                }
            }
            #region Get Company Address

            string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
            if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
            {
                address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
"Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
"dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
"n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

"Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


"Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

"Please review the attached invoice(s) for processing." + Environment.NewLine +
"Please note there may be multiple invoices contained " + Environment.NewLine +
"in each attachment. Should you have any questions, " + Environment.NewLine +
"Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
"We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

            }
            else
            {
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
            }

            ViewState["CompanyAddress"] = address;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }
            #endregion

            //foreach (DataRow dr in _dtInvoice.Rows)
            //{
            //    //billTo = Regex.Replace(billTo, @"( |\r?\n)\1+", "$1");  // to remove first new line.
            //    //string billTo = Regex.Replace(dr["Billto"].ToString(), @"\t|\n|\r", "");          // to remove all new lines.
            //    //billTo = Regex.Replace(billTo, @"^,+|,+$|,+(,\w)", "$1");
            //    //billTo = billTo.Split(new[] { ',' }, 2).First() + ",\n" + billTo.Split(new[] { ',' }, 2).Last();                
            //    //dr["Billto"] = billTo;
            //}
            ViewState["InvoiceReport"] = _dtInvoice;
            ViewState["CompanyReport"] = dsC.Tables[0];
            Session["InvoiceReportDetails"] = _dtInvoice;

            rvInvoices.LocalReport.DataSources.Clear();

            rvInvoices.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", _dtInvoice));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));

            string reportPath = "Reports/Invoices.rdlc";
            string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceDetailsReport"].Trim();
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
            string eventTarget = this.Request.Params.Get("__EVENTTARGET");
            if (eventTarget.Contains("lnkAdamMaintenance"))
            {
                reportPath = "Reports/InvoicesForAdamMaintenance.rdlc";
            }
            if (eventTarget.Contains("lnkAdamBilling"))
            {
                reportPath = "Reports/InvoicesForAdamBill.rdlc";
            }

            rvInvoices.LocalReport.ReportPath = reportPath;

            rvInvoices.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            if (string.IsNullOrEmpty(Report.Trim()) || reportPath == "Reports/InvoicesInFrench.rdlc" || reportPath == "Reports/InvoicesForAdamMaintenance.rdlc" || reportPath == "Reports/InvoicesAdams.rdlc")
            {
                param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
            }




            rvInvoices.LocalReport.SetParameters(param1);

            rvInvoices.LocalReport.Refresh();

            List<byte[]> invoicesAsBytes = new List<byte[]>();
            byte[] buffer1 = null;
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            buffer1 = ExportReportToPDF("", rvInvoices);
            buffer1 = stream.ToArray();
            invoicesAsBytes.Add(buffer1);

            return invoicesAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private DataTable GetInvoiceItems(int _refId)
    {
        DataTable _dtItem = new DataTable();
        try
        {
            objProp_Contracts.InvoiceID = _refId;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet _dsItemDetails = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            if (_dsItemDetails.Tables[0].Rows.Count < 1)
            {
                _dtItem = LoadInvoiceDetails(_dsItemDetails.Tables[0], _refId);
            }
            else
                _dtItem = _dsItemDetails.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return _dtItem;
    }
    public void ItemDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["InvoiceReport"];
            DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
            int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
            DataTable _dtInvItems = GetInvoiceItems(refId);

            if (_dtInvItems.Rows.Count > 0)
            {
                ReportDataSource rdsItems = new ReportDataSource("dtInvoiceItems", _dtInvItems);

                e.DataSources.Add(rdsItems);
            }
            if (count_inv == dt.Rows.Count - 1)
            {
                ViewState["InvoiceReport"] = null;
                ViewState["CompanyReport"] = null;
            }
            count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private DataTable LoadInvoiceDetails(DataTable _dt, int _idRef)
    {
        DataRow _dr = _dt.NewRow();
        _dr["Ref"] = _idRef;
        _dr["Acct"] = 0;
        _dr["Quan"] = 0;
        _dr["fDesc"] = string.Empty;
        _dr["Price"] = 0.00;
        _dr["Amount"] = 0.00;
        _dr["STax"] = 0.00;
        _dr["billcode"] = string.Empty;
        _dr["staxAmt"] = 0.00;
        _dr["balance"] = 0.00;
        _dr["amtpaid"] = 0.00;
        _dr["total"] = 0.00;
        _dt.Rows.Add(_dr);
        return _dt;
    }

    public StiReport FillDataSetToReport(string reportName)
    {
        DataSet ds = new DataSet();
        DataSet dsInv = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() == "TS")
        {
            if (Session["type"].ToString() != "c")
                objProp_Contracts.isTS = 1;
        }

        DataTable dtNew = (DataTable)Session["InvoiceSrch"];
        DataTable _dtInvoice = new DataTable();
        DataSet _dsInvoice = new DataSet();
        int j = 0;

        foreach (DataRow _dr in dtNew.Rows)
        {
            int _ref = Convert.ToInt32(_dr["Ref"]);

            objProp_Contracts.InvoiceID = _ref;
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

            if (j > 0)
            {
                _dtInvoice.Merge(ds.Tables[0], true);
            }
            else
            {
                _dtInvoice = ds.Tables[0];
            }
            j++;
        }
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

        int _days = 0;
        for (int i = 0; i < _dtInvoice.Rows.Count; i++)
        {

            #region Determine Pay Terms
            if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
            {
                _days = 0;
            }
            else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
            {
                _days = 10;
            }
            else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
            {
                _days = 15;
            }
            else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
            {
                _days = 30;
            }
            else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
            {
                _days = 45;
            }
            else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
            {
                _days = 60;
            }
            else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
            {
                _days = 30;
            }
            else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
            {
                _days = 90;
            }
            else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
            {
                _days = 180;
            }
            else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
            {
                _days = 0;
            }
            #endregion
            if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
            {
                _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
            }
        }
        #region Get Company Address

        string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
        address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
        address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
        address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
        address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
        address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
        if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
        {
            address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
"Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
"dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
"n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

"Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


"Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

"Please review the attached invoice(s) for processing." + Environment.NewLine +
"Please note there may be multiple invoices contained " + Environment.NewLine +
"in each attachment. Should you have any questions, " + Environment.NewLine +
"Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
"We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

        }
        else
        {
            address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
        }

        ViewState["CompanyAddress"] = address;

        ViewState["EmailFrom"] = "";
        if (Session["MSM"].ToString() != "TS")
        {
            ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
        }
        #endregion
        ViewState["InvoiceReport"] = _dtInvoice;
        ViewState["CompanyReport"] = dsC.Tables[0];
        Session["InvoiceReportDetails"] = _dtInvoice;

        // New table
        DataTable dt = (DataTable)ViewState["InvoiceReport"];
        DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
        int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
        DataTable _dtInvItems1 = GetInvoiceItems(refId);

        //STIMULSOFT 
        string reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + reportName + ".mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        //report.Compile();

        DataSet Invoices = new DataSet();
        DataTable dtInvoice1 = _dtInvItems1;
        _dtInvItems1.TableName = "Invoices";
        Invoices.Tables.Add(_dtInvItems1.Copy());
        Invoices.DataSetName = "Invoices";

        DataSet Ticket_Company = new DataSet();
        DataTable dtTicketCompany = new DataTable();
        dtTicketCompany = dsC.Tables[0].Copy();
        Ticket_Company.Tables.Add(dtTicketCompany);
        dtTicketCompany.TableName = "Ticket_Company";
        Ticket_Company.DataSetName = "Ticket_Company";

        DataSet Invoice_dtInvoice = new DataSet();
        DataTable dtInvoice = new DataTable();
        dtInvoice = ds.Tables[0].Copy();
        Invoice_dtInvoice.Tables.Add(dtInvoice);
        dtInvoice.TableName = "Invoice_dtInvoice";
        Invoice_dtInvoice.DataSetName = "Invoice_dtInvoice";

        report.RegData("dsInv", Invoices);
        report.RegData("dsInvoices", Invoice_dtInvoice);
        report.RegData("dsTicket", Ticket_Company);
        report.Render();

        return report;
    }

    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtInv = (DataTable)Session["InvoiceSrch"];

            #region Apply Grid Filter
            string filterexpression = string.Empty;
            filterexpression = Convert.ToString(RadGrid_Invoice.MasterTableView.FilterExpression);
            if (filterexpression != "")
            {
                if (dtInv.AsEnumerable().AsQueryable().Where(filterexpression) != null && dtInv.AsEnumerable().AsQueryable().Where(filterexpression).Count() > 0)
                    Session["InvoiceSrch"] = dtInv.AsEnumerable().AsQueryable().Where(filterexpression).CopyToDataTable();

                dtInv = null;
                dtInv = (DataTable)Session["InvoiceSrch"];
            }
            #endregion

            var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
            if (reportFormat.ToUpper().Equals("MRT"))
            {
                if (dtInv.Rows.Count > 0)
                {
                    if (ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString().ToUpper().Equals("MRT"))
                    {
                        StiWebViewer rvInvoices = new StiWebViewer();

                        List<byte[]> invoicesToPrint = PrintInvoices(rvInvoices);

                        string fileName = string.Empty;
                        if (dtInv.Rows.Count == 1)
                        {
                            int _ref = Convert.ToInt32(dtInv.Rows[0]["Ref"]);
                            fileName = string.Format("Invoice{0}.pdf", _ref.ToString());
                        }
                        else
                        {
                            fileName = "Invoices.pdf";
                        }
                        string file = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempPDF/" + fileName);

                        if (invoicesToPrint != null)
                        {
                            byte[] buffer1 = null;

                            buffer1 = concatAndAddContent(invoicesToPrint);

                            if (File.Exists(file))
                                File.Delete(file);
                            using (var fs = new FileStream(file, FileMode.Create))
                            {
                                fs.Write(buffer1, 0, buffer1.Length);
                                fs.Close();
                            }
                            Response.ClearContent();
                            Response.ClearHeaders();
                            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("Content-Length", (buffer1.Length).ToString());
                            Response.BinaryWrite(buffer1);
                        }
                    }
                    else
                    {
                        ReportViewer rvInvoices = new ReportViewer();

                        List<byte[]> invoicesToPrint = PrintInvoices(rvInvoices);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            else if (reportFormat.ToUpper().Equals("RDLC"))
            {
                if (dtInv.Rows.Count > 0)
                {
                    ReportViewer rvInvoices = new ReportViewer();

                    PrintInvoices(rvInvoices);

                    byte[] buffer = null;
                    buffer = ExportReportToPDF("", rvInvoices);
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Length", (buffer.Length).ToString());
                    Response.BinaryWrite(buffer);
                    // Response.Flush();
                    //Response.Close();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
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
    protected void lnkMailAll_Click(object sender, EventArgs e)
    {
        try
        {
            string[] searchitems = Convert.ToString(Session["filterstate"]).Split(';');
            if (Convert.ToString(searchitems[0]) != "")
            {
                _filteredbyloc = searchitems.Length > 0 && Convert.ToString(searchitems[0]) == "l.loc" ? true : false;
            }
            else
            {
                _filteredbyloc = false;
            }

            // DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable dtNew = GetDataForEmail(0);
            #region Apply Grid Filter
            string filterexpression = string.Empty;
            filterexpression = Convert.ToString(RadGrid_Invoice.MasterTableView.FilterExpression);
            if (filterexpression != "")
            {
                DataTable temp = new DataTable();
                if (dtNew.AsEnumerable().AsQueryable().Where(filterexpression) != null && dtNew.AsEnumerable().AsQueryable().Where(filterexpression).Count() > 0)
                    temp = dtNew.AsEnumerable().AsQueryable().Where(filterexpression).CopyToDataTable();

                dtNew = null;
                dtNew = temp;
            }
            #endregion
            byte[] buffer = null;
            objProp_Contracts.ConnConfig = WebBaseUtility.ConnectionString;
            //iF FILTERED BY LOCATION ID THEN GRP BY LOC OTHERWISE PROCESS INDUVIDUAL CUSTOMERS/LOCATIONS
            //DataTable dt = _filteredbyloc ? dtNew.AsEnumerable()
            //    .GroupBy(r => r.Field<int>("Loc"))
            //    .Select(g => g.First())
            //    .CopyToDataTable() : dtNew.AsEnumerable().CopyToDataTable();
            DataTable dt = new DataTable();
            if (dtNew != null && dtNew.Rows.Count > 0)
            {
                if (_filteredbyloc)
                {
                    var dt1 = dtNew.AsEnumerable()
                    .GroupBy(r => r.Field<int>("Loc"))
                    .Select(g => g.First());
                    //.CopyToDataTable()
                    if (dt1.Count() > 0)
                    {
                        dt = dt1.CopyToDataTable();
                    }
                }
                else
                {
                    dt = dtNew.AsEnumerable().CopyToDataTable();
                }
            }

            string _fromEmail = string.Empty;
            #region single Invoice pdf processing for  
            List<byte[]> invoicesToPrint = new List<byte[]>();
            if (_filteredbyloc)
            {
                if (ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString().ToUpper().Equals("MRT"))
                {
                    StiWebViewer rvInvoices = new StiWebViewer();
                    invoicesToPrint = PrintInvoices(rvInvoices);
                }
                else
                {
                    ReportViewer rvInvoices = new ReportViewer();
                    invoicesToPrint = PrintInvoices(rvInvoices);
                }

                string filename = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempPDF/" + "Invoices.pdf");

                if (invoicesToPrint != null)
                {
                    buffer = concatAndAddContent(invoicesToPrint);
                    if (File.Exists(filename))
                        File.Delete(filename);
                    using (var fs = new FileStream(filename, FileMode.Create))
                    {
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Close();
                    }
                }

                //_fromEmail = Convert.ToString(ViewState["EmailFrom"]);
                //if (string.IsNullOrEmpty(_fromEmail))
                //{
                //    _fromEmail = WebBaseUtility.GetFromEmailAddress();
                //}

                _fromEmail = WebBaseUtility.GetFromEmailAddress();
                foreach (DataRow _dr in dt.Rows)
                {
                    try
                    {
                        int _ref = Convert.ToInt32(_dr["Ref"]);

                        objProp_Contracts.Ref = _ref;
                        DataSet _dsCon = objBL_Contracts.GetEmailDetailByLoc(objProp_Contracts);
                        if (_dsCon.Tables[0].Rows.Count > 0)
                        {
                            string _toEmail = "";
                            string _ccEmail = "";
                            if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
                            {
                                _toEmail = _dsCon.Tables[0].Rows[0]["custom12"].ToString();

                                if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom13"].ToString()))
                                {
                                    _ccEmail = _dsCon.Tables[0].Rows[0]["custom13"].ToString();
                                }
                            }

                            txtTo.Text = _toEmail;
                            txtFrom.Text = _fromEmail;
                            txtCC.Text = _ccEmail;
                            txtSubject.Text = "Invoices - " + _dsCon.Tables[0].Rows[0]["Tag"].ToString();
                            txtBody.Text = ViewState["CompanyAddress"].ToString();//.Replace(Environment.NewLine, "<BR/>");
                            lnkattachment.Text = "Invoices.pdf";
                            hdnattcahment.Value = System.Convert.ToBase64String(buffer);

                            this.programmaticModalPopup.Show();
                        }
                        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    catch
                    {

                    }
                }
            }
            #endregion
            #region Indivudial invoices for location
            else
            {
                int totalInvoicesInList = dt != null ? dt.Rows.Count : 0;
                var temp = dt.AsEnumerable().Where(t => t.Field<Int16>("InvStatus") != 2);
                if (temp.Count() > 0)
                {
                    dt = temp.CopyToDataTable();
                }

                int totalInvoicesForEmail = temp.Count();//dt != null ? dt.Rows.Count : 0;
                int totalSentEmails = 0;
                int totalSendErr = 0;
                int totalNotSend = totalInvoicesInList - totalInvoicesForEmail;
                int mailCount = 0;
                List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
                List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
                List<string> invoiceIdsSentEmail = new List<string>();
                //List<string> invoiceIdsError = new List<string>();
                Tuple<int, string, string> emailSendError = null;
                Tuple<int, string, string> emailGetSentError = null;
                StringBuilder sbdSentError = new StringBuilder();
                StringBuilder sbdGetSentError = new StringBuilder();

                EmailLog emailLog = new EmailLog();
                emailLog.ConnConfig = Session["config"].ToString();
                emailLog.Function = "Email All";
                emailLog.Screen = "Invoice";
                emailLog.Username = Session["Username"].ToString();
                emailLog.SessionNo = Guid.NewGuid().ToString();
                if (totalInvoicesForEmail > 0)
                {
                    try
                    {
                        foreach (DataRow _dr in dt.Rows)
                        {
                            int _ref = Convert.ToInt32(_dr["Ref"]);
                            objProp_Contracts.Ref = _ref;
                            DataSet _dsCon = objBL_Contracts.GetEmailDetailByLoc(objProp_Contracts);
                            if (_dsCon.Tables[0].Rows.Count > 0)
                            {
                                emailLog.Ref = _ref;
                                if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
                                {
                                    #region Invoice processing for individual locations
                                    count_inv = 0;

                                    if (ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString().ToUpper().Equals("MRT"))
                                    {
                                        StiWebViewer rvInvoices = new StiWebViewer();
                                        invoicesToPrint = PrintInvoicesForIndivudial(rvInvoices, _dr);
                                    }
                                    else
                                    {
                                        ReportViewer rvInvoices = new ReportViewer();
                                        invoicesToPrint = PrintInvoicesForIndivudialRDLC(rvInvoices, _dr);
                                    }

                                    string filename = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempPDF/" + "Invoices.pdf");

                                    if (invoicesToPrint != null)
                                    {
                                        buffer = concatAndAddContent(invoicesToPrint);
                                        if (File.Exists(filename))
                                            File.Delete(filename);
                                        using (var fs = new FileStream(filename, FileMode.Create))
                                        {
                                            fs.Write(buffer, 0, buffer.Length);
                                            fs.Close();
                                        }
                                    }
                                    //System.IO.File.WriteAllBytes("C:\\Users\\Rashmi\\Desktop\\hello" + _ref + ".pdf", buffer);

                                    //_fromEmail = Convert.ToString(ViewState["EmailFrom"]);
                                    //if (string.IsNullOrEmpty(_fromEmail))
                                    //{
                                    //    _fromEmail = WebBaseUtility.GetFromEmailAddress();
                                    //}

                                    _fromEmail = WebBaseUtility.GetFromEmailAddress();
                                    #endregion

                                    if (mailCount == 4)
                                    {
                                        Thread.Sleep(10000);
                                        mailCount = 0;
                                    }


                                    string _toEmail = "";
                                    string _ccEmail = "";
                                    if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
                                    {
                                        _toEmail = _dsCon.Tables[0].Rows[0]["custom12"].ToString();

                                        if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom13"].ToString()))
                                        {
                                            _ccEmail = _dsCon.Tables[0].Rows[0]["custom13"].ToString();
                                        }
                                    }

                                    Mail mail = new Mail();
                                    mail.From = _fromEmail;
                                    Boolean IsMailSend = false;
                                    foreach (var toaddress in _toEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        IsMailSend = true;
                                        mail.To.Add(toaddress);
                                    }
                                    foreach (var ccaddress in _ccEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        mail.Cc.Add(ccaddress);
                                    }

                                    mail.Title = string.Format("Invoice {0} - {1}", _ref.ToString(), _dsCon.Tables[0].Rows[0]["Tag"].ToString());

                                    mail.Text = ViewState["CompanyAddress"].ToString().Replace(Environment.NewLine, "<BR/>");

                                    mail.attachmentBytes = buffer;
                                    mail.FileName = string.Format("Invoice{0}.pdf", _ref.ToString());

                                    mail.DeleteFilesAfterSend = true;
                                    mail.RequireAutentication = false;
                                    if (IsMailSend == true)
                                    {
                                        // ES-33:Task#2: Need to update email configuration before calling send function
                                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                                        //if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams"))
                                        //{
                                        //    mail.SendOld();
                                        //}
                                        //else
                                        //{
                                        //mail.Send();
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
                                                    //invoiceIdsError.Add("Invoice #" + _ref.ToString());
                                                    totalSendErr++;
                                                }
                                            }
                                            else
                                            {
                                                mimeSentMessages.Add(mimeMessage);
                                                invoiceIdsSentEmail.Add("Invoice #" + _ref.ToString());
                                            }
                                        }
                                        //}
                                    }
                                }
                                else
                                {
                                    //invoiceIdsError.Add("Invoice #" + _ref.ToString());
                                    totalSendErr++;
                                    emailLog.To = string.Empty;
                                    emailLog.Status = 0;
                                    emailLog.UsrErrMessage = "Email address does not exist for this location";
                                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
                                    bL_EmailLog.AddEmailLog(emailLog);
                                }

                            }
                        }

                        //if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams"))
                        //{
                        //    Session["MailSend"] = "true";
                        //    Response.Redirect("invoices.aspx");
                        //}
                        //else
                        //{
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
                                    + totalInvoicesInList.ToString() + " invoices sent out successfully.";
                                //if (invoiceIdsError.Count > 0)
                                //{
                                //    successfullMess += "<br>Total " + invoiceIdsError.Count + " failed of "
                                //        + totalInvoices.ToString() + " invoices could not be sent.";
                                //}
                                if (totalSendErr > 0)
                                {
                                    successfullMess += "<br>Total " + totalSendErr + " failed of "
                                        + totalInvoicesInList.ToString() + " invoices could not be sent.";
                                }
                                if (totalNotSend > 0)
                                {
                                    //successfullMess += "<br>Total " + ticketIdsError.Count + " failed of "
                                    //    + totalCompletedTicket.ToString() + " tickets could not be sent.";
                                    successfullMess += "<br>Total " + totalNotSend + " of "
                                        + totalInvoicesInList + " invoices were voided.";
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
                                //if (invoiceIdsError.Count > 0)
                                //{
                                //    str += "<br>Total " + invoiceIdsError.Count + " failed of "
                                //        + totalInvoices.ToString() + " invoices could not be sent.";
                                //}
                                if (totalSendErr > 0)
                                {
                                    str += "<br>Total " + totalSendErr + " failed of "
                                        + totalInvoicesInList.ToString() + " invoices could not be sent.";
                                }
                                if (totalNotSend > 0)
                                {
                                    //successfullMess += "<br>Total " + ticketIdsError.Count + " failed of "
                                    //    + totalCompletedTicket.ToString() + " tickets could not be sent.";
                                    str += "<br>Total " + totalNotSend + " of "
                                        + totalInvoicesInList + " invoices were voided.";
                                }
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }
                        //}

                        RadGrid_gvLogs.Rebind();
                        //Session["MailSend"] = "true";
                        //Response.Redirect("invoices.aspx");
                    }
                    catch (Exception exp)
                    {
                        //string str = exp.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarning", "noty({text: 'No invoice for sending email. Please check your list again!',  type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });", true);
                }
            }
            #endregion
            // TODO: need to check again. This function always set Session["MailSend"] = "true";
            //Session["MailSend"] = "true";
            //Response.Redirect("invoices.aspx");

        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    /// <summary>
    /// This method generates a individual pdf for different invoices
    /// </summary>
    /// <param name="rvInvoices"></param>
    /// <param name="_dr"></param>
    private void PrintInvoices2(ReportViewer rvInvoices, DataRow _dr)
    {
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int _ref = Convert.ToInt32(_dr["Ref"]);

            objProp_Contracts.InvoiceID = _ref;
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

            _dtInvoice = ds.Tables[0];

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

            int _days = 0;
            for (int i = 0; i < _dtInvoice.Rows.Count; i++)
            {
                #region Determine Pay Terms
                if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                {
                    _days = 0;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                {
                    _days = 10;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                {
                    _days = 15;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                {
                    _days = 45;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                {
                    _days = 60;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                {
                    _days = 90;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                {
                    _days = 180;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                {
                    _days = 0;
                }
                #endregion
                if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                {
                    _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                    _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                }
            }
            #region Get Company Address

            string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
            if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
            {
                address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
"Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
"dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
"n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

"Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


"Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

"Please review the attached invoice(s) for processing." + Environment.NewLine +
"Please note there may be multiple invoices contained " + Environment.NewLine +
"in each attachment. Should you have any questions, " + Environment.NewLine +
"Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
"We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

            }
            else
            {
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
            }

            ViewState["CompanyAddress"] = address;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }
            #endregion
            ViewState["InvoiceReport"] = _dtInvoice;
            ViewState["CompanyReport"] = dsC.Tables[0];

            rvInvoices.LocalReport.DataSources.Clear();


            rvInvoices.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", _dtInvoice));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));

            string reportPath = "Reports/Invoices.rdlc";
            string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceDetailsReport"].Trim();
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }

            rvInvoices.LocalReport.ReportPath = reportPath;

            rvInvoices.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            //param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
            if (Report == "" || Report == "InvoicesAdams.rdlc")
            {
                param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
            }
            rvInvoices.LocalReport.SetParameters(param1);

            rvInvoices.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    /// <summary>
    /// This method generates a single pdf for different invoices belonging to single locations
    /// </summary>
    /// <param name="rvInvoices"></param>
    /// <param name="dtNew"></param>
    private void PrintInvoices1(ReportViewer rvInvoices, DataTable dtNew)
    {
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();

            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }


            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            foreach (DataRow _dr in dtNew.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);

                objProp_Contracts.InvoiceID = _ref;
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                if (j > 0)
                {
                    _dtInvoice.Merge(ds.Tables[0], true);
                }
                else
                {
                    _dtInvoice = ds.Tables[0];
                }
                j++;
            }
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

            int _days = 0;
            for (int i = 0; i < _dtInvoice.Rows.Count; i++)
            {

                #region Determine Pay Terms
                if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                {
                    _days = 0;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                {
                    _days = 10;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                {
                    _days = 15;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                {
                    _days = 45;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                {
                    _days = 60;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                {
                    _days = 90;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                {
                    _days = 180;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                {
                    _days = 0;
                }
                #endregion
                if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                {
                    _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                    _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                }
            }
            #region Get Company Address

            string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;

            if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
            {
                address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
"Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
"dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
"n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

"Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


"Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

"Please review the attached invoice(s) for processing." + Environment.NewLine +
"Please note there may be multiple invoices contained " + Environment.NewLine +
"in each attachment. Should you have any questions, " + Environment.NewLine +
"Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
"We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

            }
            else
            {
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
            }

            ViewState["CompanyAddress"] = address;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }
            #endregion

            ViewState["InvoiceReport"] = _dtInvoice;
            ViewState["CompanyReport"] = dsC.Tables[0];

            rvInvoices.LocalReport.DataSources.Clear();

            rvInvoices.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", _dtInvoice));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));

            string reportPath = "Reports/Invoices.rdlc";
            string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceDetailsReport"].Trim();
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }

            rvInvoices.LocalReport.ReportPath = reportPath;

            rvInvoices.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            if (Report == "" || Report == "InvoicesAdams.rdlc")
            {
                param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
            }

            rvInvoices.LocalReport.SetParameters(param1);

            rvInvoices.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            string _toEmail = string.Empty;
            string _ccEmail = string.Empty;
            string _fromEmail = string.Empty;
            string _title = string.Empty;
            string _body = string.Empty;

            _toEmail = txtTo.Text;
            _fromEmail = txtFrom.Text;
            _ccEmail = txtCC.Text;
            _title = txtSubject.Text;
            _body = txtBody.Text;
            Byte[] pdfBytes = System.Convert.FromBase64String(hdnattcahment.Value);



            //List<string> _toEmaillst = new List<string>();
            //_toEmaillst.Add(_toEmail);
            //List<string> _ccEmaillst = new List<string>();
            //_ccEmaillst.Add(_ccEmail);



            Mail mail = new Mail();
            mail.From = _fromEmail;
            foreach (var toaddress in _toEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                mail.To.Add(toaddress);
            }
            foreach (var ccaddress in _ccEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                mail.Cc.Add(ccaddress);
            }


            mail.Title = "Invoices - " + _title;

            mail.Text = _body.Replace(Environment.NewLine, "<BR/>");

            mail.attachmentBytes = pdfBytes;
            mail.FileName = "Invoices.pdf";

            mail.DeleteFilesAfterSend = true;
            mail.RequireAutentication = false;
            // ES-33:Task#2: add
            WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
            //if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams"))
            //    mail.SendOld();
            //else
            mail.Send();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        hideModal();

    }

    protected void btnVoidInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            bool _flag = false;
            bool _flagJob = true;
            bool IsClosed = false;
            int NRef = 0;
            foreach (GridDataItem di in RadGrid_Invoice.SelectedItems)
            {
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                if (chkSelect.Checked == true)
                {
                    Label lblID = (Label)di.FindControl("lblID");
                    Label lblInvDate = (Label)di.FindControl("lblInvDate");
                    Label lblInvStatus = (Label)di.FindControl("lblInvStatus");
                    HiddenField hdnJobStatus = (HiddenField)di.FindControl("hdnJobStatus");

                    if (!string.IsNullOrEmpty(lblID.Text))
                    {
                        objProp_Contracts.ConnConfig = Session["config"].ToString();
                        objProp_Contracts.Ref = Convert.ToInt32(lblID.Text);
                        objProp_Contracts.Date = Convert.ToDateTime(lblInvDate.Text);
                        objProp_Contracts.Status = Convert.ToInt16(lblInvStatus.Text);
                        NRef = objProp_Contracts.Ref;
                        _flag = CommonHelper.GetPeriodDetails(objProp_Contracts.Date);
                        if (_flag)
                        {
                            if (hdnJobStatus.Value == "1")
                            {
                                _flagJob = false;
                            }
                            if (_flagJob)
                            {
                                if (objProp_Contracts.Status.Equals(0))
                                {
                                    objProp_Contracts.ConnConfig = Session["config"].ToString();
                                    objProp_Contracts.Ref = Convert.ToInt32(lblID.Text);
                                    objProp_Contracts.Date = DateTime.Now;
                                    objProp_Contracts.Fuser = Session["username"].ToString();
                                    objBL_Contracts.UpdateVoidInvoiceDetails(objProp_Contracts);
                                    GetInvoices(0);
                                    RadGrid_Invoice.Rebind();
                                }
                                else
                                {
                                    IsClosed = true;
                                }
                            }

                        }
                    }
                }
            }
            if (!_flag)
            {

                if (!_flagJob)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "VoidedInvoiceInProjectClose();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "displyDeleteAlert('void');", true);
                }
            }
            else
            {
                if (!_flagJob)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "VoidedInvoiceInProjectClose();", true);
                }
                else
                {
                    if (IsClosed)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "ClosedInvoice('" + NRef + "');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void hideModal()
    {

        txtTo.Text = string.Empty;
        txtFrom.Text = string.Empty;
        txtCC.Text = string.Empty;
        txtSubject.Text = string.Empty;
        lnkattachment.Text = string.Empty;
        hdnattcahment.Value = "";
        txtBody.Text = string.Empty;
        this.programmaticModalPopup.Hide();

    }

    protected void lnkattachment_Click(object sender, EventArgs e)
    {
        Byte[] pdfBytes = System.Convert.FromBase64String(hdnattcahment.Value);
        Response.ClearContent();
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Disposition", "inline");
        Response.AddHeader("Content-Length", pdfBytes.Length.ToString());
        Response.BinaryWrite(pdfBytes);
        Response.End();

    }

    protected void hideModalPopupViaClientButton_Click(object sender, EventArgs e)
    {
        hideModal();
    }

    private List<CustomerReport> GetReportsName()
    {

        List<CustomerReport> lstCustomerReport = new List<CustomerReport>();
        try
        {
            DataSet dsGetReports = new DataSet();
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objPropUser.Type = "Invoices";
            dsGetReports = objBL_ReportsData.GetStockReports(objPropUser);
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

    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportsName());
        string reports = "var reports=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", reports, true);
    }
    protected void lnk_InvoiceMaint_Click(object sender, EventArgs e)
    {
        Session["InvoiceName"] = "InvoiceMaint";
        DataSet ds = new DataSet();
        DataSet dsInv = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() == "TS")
        {
            if (Session["type"].ToString() != "c")
                objProp_Contracts.isTS = 1;
        }

        DataTable dtNew = (DataTable)Session["InvoiceSrch"];
        DataTable _dtInvoice = new DataTable();
        DataSet _dsInvoice = new DataSet();
        int j = 0;

        foreach (DataRow _dr in dtNew.Rows)
        {
            int _ref = Convert.ToInt32(_dr["Ref"]);

            objProp_Contracts.InvoiceID = _ref;
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

            if (j > 0)
            {
                _dtInvoice.Merge(ds.Tables[0], true);
            }
            else
            {
                _dtInvoice = ds.Tables[0];
            }
            j++;
        }
        Session["InvoiceReportDetails"] = _dtInvoice;
        Response.Redirect("Printinvoices.aspx?Check=0");
    }
    protected void lnk_InvoiceException_Click(object sender, EventArgs e)
    {
        Session["InvoiceName"] = "InvoiceException";
        DataSet ds = new DataSet();
        DataSet dsInv = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() == "TS")
        {
            if (Session["type"].ToString() != "c")
                objProp_Contracts.isTS = 1;
        }

        DataTable dtNew = (DataTable)Session["InvoiceSrch"];
        DataTable _dtInvoice = new DataTable();
        DataSet _dsInvoice = new DataSet();
        int j = 0;

        foreach (DataRow _dr in dtNew.Rows)
        {
            int _ref = Convert.ToInt32(_dr["Ref"]);

            objProp_Contracts.InvoiceID = _ref;
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

            if (j > 0)
            {
                _dtInvoice.Merge(ds.Tables[0], true);
            }
            else
            {
                _dtInvoice = ds.Tables[0];
            }
            j++;
        }
        Session["InvoiceReportDetails"] = _dtInvoice;
        Response.Redirect("Printinvoices.aspx?Check=0");
    }
    protected void lnkException_Click(object sender, EventArgs e)
    {
        try
        {

            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            foreach (DataRow _dr in dtNew.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);

                objProp_Contracts.InvoiceID = _ref;
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                if (j > 0)
                {
                    _dtInvoice.Merge(ds.Tables[0], true);
                }
                else
                {
                    _dtInvoice = ds.Tables[0];
                }
                j++;
            }
            Session["InvoiceReportDetails"] = _dtInvoice;
            ViewState["InvoicesSubReportResult"] = _dtInvoice;

            DataTable dt = new DataTable();

            DataTable dtInv = (DataTable)Session["InvoiceSrch"];
            if (dtInv.Rows.Count > 0)
            {
                ReportViewer rvInvoices = new ReportViewer();

                PrintExceptionInvoices(rvInvoices, _dtInvoice);

                byte[] buffer = null;
                buffer = ExportReportToPDF("", rvInvoices);
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (buffer.Length).ToString());
                Response.BinaryWrite(buffer);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkMaintenance_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            foreach (DataRow _dr in dtNew.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);

                objProp_Contracts.InvoiceID = _ref;
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                if (j > 0)
                {
                    _dtInvoice.Merge(ds.Tables[0], true);
                }
                else
                {
                    _dtInvoice = ds.Tables[0];
                }
                j++;
            }

            Session["InvoiceReportDetails"] = _dtInvoice;
            ViewState["InvoicesSubReportResult"] = _dtInvoice;

            DataTable dt = new DataTable();

            DataTable dtInv = (DataTable)Session["InvoiceSrch"];
            if (dtInv.Rows.Count > 0)
            {
                ReportViewer rvInvoices = new ReportViewer();

                PrintMaintInvoices(rvInvoices, _dtInvoice);

                byte[] buffer = null;
                buffer = ExportReportToPDF("", rvInvoices);
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (buffer.Length).ToString());
                Response.BinaryWrite(buffer);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void PrintMaintInvoices(ReportViewer rv, DataTable dtInvoice)
    {
        DataTable dtCompany = new DataTable();
        if (ViewState["RecurCompany"] == null)
        {
            DataSet dsCompany = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsCompany = objBL_User.getControl(objPropUser);
            ViewState["RecurCompany"] = dsCompany.Tables[0];
            dtCompany = dsCompany.Tables[0];
        }
        else
        {
            dtCompany = (DataTable)ViewState["RecurCompany"];
        }
        rv.LocalReport.DataSources.Clear();  //added by dev 15th march, 16

        rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemSubReportProcessing);


        string Report = string.Empty;

        Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();

        if (Report == "PESMTC_InvoicesMaint.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_PESdtInvoice", dtInvoice));
            }
        }

        rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dtCompany));

        string reportPath = string.Empty;


        if (Report == "PESMTC_InvoicesMaint.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
        }


        rv.LocalReport.ReportPath = reportPath;

        rv.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
        if (Report == "")
        {
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
        }
        rv.LocalReport.SetParameters(param1);

        rv.LocalReport.Refresh();
    }

    private void PrintExceptionInvoices(ReportViewer rv, DataTable dtInvoice)
    {
        DataTable dtCompany = new DataTable();
        if (ViewState["RecurCompany"] == null)
        {
            DataSet dsCompany = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            //objPropUser.DBName = Session["dbname"].ToString();
            //objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
            //DataSet dsloc = new DataSet();
            //dsloc = objBL_User.getLocationByID(objPropUser);

            //if (Session["MSM"].ToString() != "TS")
            //{
            dsCompany = objBL_User.getControl(objPropUser);
            //}
            //else
            //{
            //    objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
            //    dsCompany = objBL_User.getControlBranch(objPropUser);
            //}
            ViewState["RecurCompany"] = dsCompany.Tables[0];
            dtCompany = dsCompany.Tables[0];
        }
        else
        {
            dtCompany = (DataTable)ViewState["RecurCompany"];
        }

        //foreach (DataRow dr in dtInvoice.Rows)
        //{
        //    //billTo = Regex.Replace(billTo, @"( |\r?\n)\1+", "$1");  // to remove first new line.
        //    string billTo = Regex.Replace(dr["Billto"].ToString(), @"\t|\n|\r", "");          // to remove all new lines.
        //    billTo = Regex.Replace(billTo, @"^,+|,+$|,+(,\w)", "$1");
        //    billTo = billTo.Split(new[] { ',' }, 2).First() + ",\n" + billTo.Split(new[] { ',' }, 2).Last();
        //    dr["Billto"] = billTo;
        //}

        rv.LocalReport.DataSources.Clear();  //added by dev 15th march, 16

        rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemSubExceptionReportProcessing);


        string Report = string.Empty;

        Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();

        if (Report == "PESMTC_InvoicesExceptions.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_PESdtInvoice", dtInvoice));
            }
        }

        rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dtCompany));

        string reportPath = string.Empty;


        if (Report == "PESMTC_InvoicesExceptions.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
        }


        rv.LocalReport.ReportPath = reportPath;

        rv.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
        if (Report == "")
        {
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
        }
        rv.LocalReport.SetParameters(param1);

        rv.LocalReport.Refresh();
    }
    private void ItemSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["InvoicesSubReportResult"];
            DataTable dtItems = new DataTable();

            objProp_Contracts.InvoiceID = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            if (ds.Tables[0].Rows.Count < 1)
            {
                dtItems = LoadInvoiceDetails(ds.Tables[0], objProp_Contracts.InvoiceID);    // if none line item exists of invoice
            }
            else
                dtItems = ds.Tables[0];

            ReportDataSource rdsItems = null;
            if (dtItems.Rows.Count > 0)
            {
                //string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesReport"].Trim();

                string Report = string.Empty;

                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();

                if (Report == "PESMTC_InvoicesMaint.rdlc")
                {
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems);
                    }
                }

                e.DataSources.Add(rdsItems);
            }
            if (count_inv == dt.Rows.Count - 1)
            {
                ViewState["InvoicesSubReportResult"] = null;
            }
            count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void ItemSubExceptionReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["InvoicesSubReportResult"];
            DataTable dtItems = new DataTable();

            objProp_Contracts.InvoiceID = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            if (ds.Tables[0].Rows.Count < 1)
            {
                dtItems = LoadInvoiceDetails(ds.Tables[0], objProp_Contracts.InvoiceID);    // if none line item exists of invoice
            }
            else
                dtItems = ds.Tables[0];

            ReportDataSource rdsItems = null;
            if (dtItems.Rows.Count > 0)
            {
                //string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesReport"].Trim();

                string Report = string.Empty;

                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();

                if (Report == "PESMTC_InvoicesExceptions.rdlc")
                {
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems);
                    }
                }

                e.DataSources.Add(rdsItems);
            }
            if (count_inv == dt.Rows.Count - 1)
            {
                ViewState["InvoicesSubReportResult"] = null;
            }
            count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    int count = 0;
    List<byte[]> lstbyte = new List<byte[]>();
    protected void lnkPDFTI_Click(object sender, EventArgs e)
    {
        string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            foreach (DataRow _dr in dtNew.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);

                objProp_Contracts.InvoiceID = _ref;
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                if (j > 0)
                {
                    _dtInvoice.Merge(ds.Tables[0], true);
                }
                else
                {
                    _dtInvoice = ds.Tables[0];
                }
                j++;
            }
            Session["InvoiceReportDetails"] = _dtInvoice;
            ViewState["InvoicesSubReportResult"] = _dtInvoice;

            DataTable dt = new DataTable();

            DataTable dtInv = (DataTable)Session["InvoiceSrch"];
            if (dtInv.Rows.Count > 0)
            {
                if (Report != null && Path.GetExtension(Report) == ".mrt")
                {
                    PrintInvoiceWithTicketMRT(_dtInvoice, Report);
                }
                else
                {
                    foreach (DataRow drow in _dtInvoice.Rows)
                    {
                        ReportViewer rvInvoices = new ReportViewer();
                        int invoiceNo = (int)drow[1];
                        ViewState["invoiceNo"] = invoiceNo;
                        if (Report == "InvoiceLNY-WithTicket-Adams.rdlc")
                        {
                            PrintInvoiceWithTicketForAdams(rvInvoices, invoiceNo);
                        }
                        else
                        {
                            PrintInvoicesTicket(rvInvoices, invoiceNo);
                        }
                        array = ExportReportToPDF("", rvInvoices);

                        lstbyte.Add(array);

                    }
                    byte[] allbyte = Invoices.concatAndAddContent(lstbyte);
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.AddHeader("Content-Disposition", "attachment;filename=InvoicesWithTicket.pdf");
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Length", (allbyte.Length).ToString());
                    Response.BinaryWrite(allbyte);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void PrintInvoicesTicket(ReportViewer rv, int invoiceNo)
    {
        DataTable dtCompany = new DataTable();
        if (ViewState["RecurCompany"] == null)
        {
            DataSet dsCompany = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            dsCompany = objBL_User.getControl(objPropUser);

            ViewState["RecurCompany"] = dsCompany.Tables[0];
            dtCompany = dsCompany.Tables[0];
        }
        else
        {
            dtCompany = (DataTable)ViewState["RecurCompany"];
        }
        DataTable dtInvoice = (DataTable)Session["InvoiceReportDetails"];
        dtInvoice = dtInvoice.Select("Ref=" + invoiceNo).CopyToDataTable();
        string Report = string.Empty;

        Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();

        DataTable dtEquip = new DataTable();
        DataTable dtTicket = new DataTable();
        DataTable dtTicketPO = new DataTable();
        DataTable dtTicketI = new DataTable();
        DataTable dtDetails = new DataTable();

        if (Report == "Invoice_Ticket-LNY.rdlc")
        {
            int i = 0;

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = invoiceNo;

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataSet TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);

            foreach (DataRow item in TicketID.Tables[0].Rows)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = (int)item[0];
                DataSet dsEquip = objBL_MapData.getElevByTicket(objMapData);
                DataSet dsTicket = objBL_MapData.GetTicketByID(objMapData);
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.EquipID = 0;
                objPropUser.SearchBy = "rd.ticketID";
                objPropUser.SearchValue = item[0].ToString();
                DataSet dsDetails = objBL_User.getequipREPDetails(objPropUser);
                if (i == 0)
                {
                    dtEquip = dsEquip.Tables[0];
                    dtTicket = dsTicket.Tables[0];
                    dtTicketPO = dsTicket.Tables[1];
                    dtTicketI = dsTicket.Tables[2];
                    dtDetails = dsDetails.Tables[0];
                    i++;
                }
                else
                {
                    if (dtEquip.Rows.Count > 0)
                    {
                        dtEquip.Rows.Add(dsEquip.Tables[0].Rows[0].ItemArray);
                    }

                    dtTicket.Rows.Add(dsTicket.Tables[0].Rows[0].ItemArray);
                    //dtTicketPO.Rows.Add(dsTicket.Tables[1].Rows[0].ItemArray);
                    //dtTicketI.Rows.Add(dsTicket.Tables[2].Rows[0].ItemArray);
                    //dtDetails.Rows.Add(dsDetails.Tables[0].Rows[0].ItemArray);
                    i++;
                }
            }
        }

        //foreach (DataRow dr in dtInvoice.Rows)
        //{
        //    //billTo = Regex.Replace(billTo, @"( |\r?\n)\1+", "$1");  // to remove first new line.
        //    string billTo = Regex.Replace(dr["Billto"].ToString(), @"\t|\n|\r", "");          // to remove all new lines.
        //    billTo = Regex.Replace(billTo, @"^,+|,+$|,+(,\w)", "$1");
        //    billTo = billTo.Split(new[] { ',' }, 2).First() + ",\n" + billTo.Split(new[] { ',' }, 2).Last();
        //    dr["Billto"] = billTo;
        //}

        rv.LocalReport.DataSources.Clear();  //added by dev 15th march, 16

        rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemSubInvoiceTicketReportProcessing);

        if (Report == "Invoice_Ticket-LNY.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_PESdtInvoice", dtInvoice));
                rv.LocalReport.DataSources.Add(new ReportDataSource("dtEquipDetails", dtEquip));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtMCP", dtDetails));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtPOItem", dtTicketPO));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicketI", dtTicketI));
            }
        }
        else
        {
            rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));
        }

        rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dtCompany));

        DataTable rowCount = (DataTable)Session["InvoiceReportDetails"];

        //if (count == rowCount.Rows.Count - 1)
        //{

        string reportPath = string.Empty;


        if (Report == "Invoice_Ticket-LNY.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
        }


        rv.LocalReport.ReportPath = reportPath;

        rv.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
        if (Report == "")
        {
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
        }
        rv.LocalReport.SetParameters(param1);

        rv.LocalReport.Refresh();
        //}
    }
    private void ItemSubInvoiceTicketReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["InvoicesSubReportResult"];
            DataTable dtItems = new DataTable();
            objProp_Contracts.InvoiceID = (int)ViewState["invoiceNo"];
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            if (ds.Tables[0].Rows.Count < 1)
            {
                dtItems = LoadInvoiceDetails(ds.Tables[0], objProp_Contracts.InvoiceID);    // if none line item exists of invoice
            }
            else
            {
                dtItems = ds.Tables[0];
            }

            DataTable dtEquip = new DataTable();

            int i = 0;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = (int)ViewState["invoiceNo"];

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataSet TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);
            foreach (DataRow item in TicketID.Tables[0].Rows)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = (int)item[0];
                DataSet dsEquip = objBL_MapData.getElevByTicketID(objMapData);
                if (i == 0)
                {
                    dtEquip = dsEquip.Tables[0];
                    i++;
                }
                else
                {
                    if (dtEquip.Rows.Count > 0)
                    {
                        dtEquip.Rows.Add(dsEquip.Tables[0].Rows[0].ItemArray);
                    }
                    i++;
                }
            }
            ReportDataSource rdsItems = null;
            if (dtItems.Rows.Count > 0)
            {
                string Report = string.Empty;

                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();

                if (Report == "Invoice_Ticket-LNY.rdlc")
                {
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        e.DataSources.Add(rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems));
                        e.DataSources.Add(rdsItems = new ReportDataSource("dtEquipDetailsID", dtEquip));
                    }
                }
                else
                {
                    e.DataSources.Add(rdsItems = new ReportDataSource("dtInvoiceItems", dtItems));
                }
            }
            if (count_inv == dtItems.Rows.Count - 1)
            {
                ViewState["InvoicesSubReportResult"] = null;
            }
            count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    static byte[] mergedPdf = null;
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

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        PaymentRedirect(true);
    }

    private void PaymentRedirect(bool ACH)
    {
        int count = 0;
        string invoices = string.Empty;
        List<Dictionary<string, string>> lstInv = new List<Dictionary<string, string>>();
        try
        {
            foreach (GridDataItem gr in RadGrid_PaymentInv.Items)
            {
                // TableCell cell = gr["chkPaySelect"];
                // CheckBox chkSelect = (CheckBox)cell.Controls[0];
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                TextBox txtBalance = (TextBox)gr.FindControl("txtBalance");
                Label lblInvocie = (Label)gr.FindControl("lblID");
                Dictionary<string, string> dInv = new Dictionary<string, string>();
                if (chkSelect.Checked == true)
                {
                    if (Convert.ToDouble(objGeneralFunction.IsNull(txtBalance.Text.Trim(), "0")) == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "key0", "noty({text: 'Invoice# " + lblInvocie.Text + " payment amount can not be zero.',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "key2", "SelectedRowStyle('" + RadGrid_PaymentInv.ClientID + "');", true);
                        return;
                    }

                    dInv.Add("inv", lblInvocie.Text);
                    dInv.Add("amt", Convert.ToString(Math.Round(Convert.ToDecimal(objGeneralFunction.IsNull(txtBalance.Text.Trim(), "0")), 2)));

                    lstInv.Add(dInv);

                    if (count == 0)
                        invoices += lblInvocie.Text;
                    else
                        invoices += "," + lblInvocie.Text;

                    count++;
                }
            }
            //JavaScriptSerializer sr = new JavaScriptSerializer();
            //string inv = sr.Serialize(lstInv);
            string discarded = CheckAmountExceed(lstInv, invoices);
            if (discarded == string.Empty)
            {
                bool port = HttpContext.Current.Request.Url.IsDefaultPort;
                string Auth = HttpContext.Current.Request.Url.Authority;
                if (!port)
                {
                    Auth = HttpContext.Current.Request.Url.DnsSafeHost;
                }

                string webPath = System.Web.Configuration.WebConfigurationManager.AppSettings["webPath"].Trim();
                string URL = Auth + webPath;
                Session["uidv"] = lstInv;

                string paymentscreen = System.Web.Configuration.WebConfigurationManager.AppSettings["PayGateway"].Trim();
                //Response.Redirect("https://" + URL + "fdggpay.aspx?inv=invoices.aspx");
                if (!ACH)
                    Response.Redirect("https://" + URL + paymentscreen + "?inv=invoices.aspx");
                else
                    Response.Redirect("https://" + URL + "achpayment.aspx?inv=invoices.aspx");

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key4", "noty({text: 'Invoice(s)# " + discarded + " exceeds the payment amount.',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key2", "SelectedRowStyle('" + RadGrid_PaymentInv.ClientID + "');", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key2", "SelectedRowStyle('" + RadGrid_Invoice.ClientID + "');", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void lnkLoadInvoices_Click(object sender, EventArgs e)
    {
        byte[] buffer1 = null;
        string reportName = ddlInvoicesForLoad.SelectedItem.Text.Trim();
        if (reportName != "")
        {
            string reportApTopCheckPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + reportName + ".mrt");
            StiReport report = new StiReport();
            report = FillDataSetToReport(reportName);
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(report, stream, settings);
            buffer1 = stream.ToArray();

            string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "Invoices.pdf");

            if (buffer1 != null)
            {
                if (File.Exists(filename))
                    File.Delete(filename);
                using (var fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(buffer1, 0, buffer1.Length);
                    fs.Close();
                }
            }
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=PrintCheck.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (buffer1.Length).ToString());
            Response.BinaryWrite(buffer1);
            Response.Flush();
            Response.Close();
        }
    }

    protected void lnkEditInvoices_Click(object sender, EventArgs e)
    {
        string reportName = ddlInvoicesForLoad.SelectedItem.Text.Trim();
        StiReport report = FillDataSetToReport(reportName);
        StiWebDesigner1.Report = report;
        ReportModalPopupExtender.Show();
        StiWebDesigner1.Visible = true;
    }

    protected void lnkDeleteInvoices_Click(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("StimulsoftReports/Invoices");
        string selValue = ddlInvoicesForLoad.SelectedItem.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("InvoicesDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            string str = "Template " + selValue + " Deleted!--";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Invoice# " + str + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkCancelInvoices_Click(object sender, EventArgs e)
    {
        Response.Redirect("Invoices.aspx");
    }
    protected void lnkSaveInvoices_Click(object sender, EventArgs e)
    {
        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/Invoices/" + ConfigurationManager.AppSettings["InvoiceReport"].ToString());
            string filePath = Server.MapPath("StimulsoftReports/Invoices");
            string tempPath = Server.MapPath("StimulsoftReports/Invoices");
            string selValue = ddlInvoicesForLoad.SelectedItem.Text.TrimEnd();
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
                    Response.Redirect("Invoices.aspx");

                }
                else
                    throw new Exception("Invoices.mrt is not available");
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    protected void lnkCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Invoices.aspx");
    }

    protected void StiWebDesigner1_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        ReportModalPopupExtender.Hide();
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/Invoices/" + ConfigurationManager.AppSettings["InvoiceReport"].ToString()));
    }
    protected void StiWebDesigner1_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/Invoices/" + e.FileName));
    }
    protected void StiWebDesigner1_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("Invoices.aspx");
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void lnk_EditReportTemplate_Click(object sender, EventArgs e)
    {
        SaveFilter();
        Response.Redirect("EditReportTemplate.aspx");
    }

    protected void lnkInvoicesReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchValue = string.Empty;

            if (ddlSearch.SelectedValue == "i.Type")
            {
                searchValue = ddlDepartment.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "i.Status")
            {
                searchValue = ddlStatus.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "i.fdate")
            {
                searchValue = txtInvDt.Text;
            }
            else if (ddlSearch.SelectedValue == "l.loc")
            {
                searchValue = ddllocation.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "i.ref")
            {
                if (txtSearch.Text != string.Empty)
                {
                    searchValue = txtSearch.Text.Replace("'", "''");
                }
            }
            else
            {
                searchValue = txtSearch.Text.Replace("'", "''");
            }

            //var searchAmtPaidUnpaid = string.Empty;
            //if (ddlpaidunpaid.SelectedValue == "All")
            //{
            //    searchAmtPaidUnpaid = string.Empty;
            //}
            //else if (ddlpaidunpaid.SelectedValue == "Paid")
            //{
            //    searchAmtPaidUnpaid = "P";
            //}
            //else if (ddlpaidunpaid.SelectedValue == "Open")
            //{
            //    searchAmtPaidUnpaid = "O";
            //}
            var searchAmtPaidUnpaid = string.Empty;
            if (ddlpaidunpaid.CheckedItems.Count > 0)
            {
                foreach (var item in ddlpaidunpaid.CheckedItems)
                {
                    searchAmtPaidUnpaid += item.Value + ",";
                }

                searchAmtPaidUnpaid = searchAmtPaidUnpaid.TrimEnd(',');


            }

            var searchPrintMail = string.Empty;
            if (ddlPrintOnly.SelectedValue == "All")
            {
                searchPrintMail = string.Empty;
            }
            else if (ddlPrintOnly.SelectedValue == "PrintOnly")
            {
                searchPrintMail = "P";
            }
            else if (ddlPrintOnly.SelectedValue == "Mail")
            {
                searchPrintMail = "M";
            }

            string urlString = "InvoicesReport.aspx?sd=" + txtFromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchValue + "&amtpaid=" + searchAmtPaidUnpaid + "&printemail=" + searchPrintMail + "&page=invoices";
            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCustomerStatement_Click(object sender, EventArgs e)
    {
        var url = "customerstatement.aspx?page=invoices";
        string reportCustom = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"];
        if (!string.IsNullOrEmpty(reportCustom) && reportCustom.ToLower().Contains(".mrt"))
        {
            url = "customerstatementreport.aspx?page=invoices";
        }
        SaveFilter();
        Response.Redirect(url);
    }

    protected void lnkPrintInvoiceRegister_Click(object sender, EventArgs e)
    {
        var url = "PrintInvoiceRegister.aspx?sd=" + txtFromDate.Text + "&ed=" + txtToDate.Text + "&page=invoices";

        Response.Redirect(url);
    }

    //Adams Invoice With Ticket

    private void PrintInvoiceWithTicketForAdams(ReportViewer rvInvoices, int invoiceNo)
    {
        // Export to PDF
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            DataSet dsTicket = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }


            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            objProp_Contracts.InvoiceID = invoiceNo;
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
            _dtInvoice = ds.Tables[0];


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

            int _days = 0;
            for (int i = 0; i < _dtInvoice.Rows.Count; i++)
            {

                #region Determine Pay Terms
                if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                {
                    _days = 0;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                {
                    _days = 10;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                {
                    _days = 15;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                {
                    _days = 45;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                {
                    _days = 60;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                {
                    _days = 90;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                {
                    _days = 180;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                {
                    _days = 0;
                }
                #endregion
                if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                {
                    _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                    _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                }
            }
            #region Get Company Address

            string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
            if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
            {
                address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
"Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
"dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
"n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

"Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


"Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

"Please review the attached invoice(s) for processing." + Environment.NewLine +
"Please note there may be multiple invoices contained " + Environment.NewLine +
"in each attachment. Should you have any questions, " + Environment.NewLine +
"Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
"We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

            }
            else
            {
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
            }

            //ViewState["CompanyAddress"] = address;

            //ViewState["EmailFrom"] = "";
            //if (Session["MSM"].ToString() != "TS")
            //{
            //    ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            //}
            #endregion

            //ViewState["InvoiceReport"] = _dtInvoice;
            //ViewState["CompanyReport"] = dsC.Tables[0];
            //Session["InvoiceReportDetails"] = _dtInvoice;

            rvInvoices.LocalReport.DataSources.Clear();

            // rvInvoices.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);

            DataTable _dtInvItems = GetInvoiceItems(invoiceNo);

            //////Get Ticket /////

            DataTable dtTicket = new DataTable();
            int ii = 0;

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = invoiceNo;

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataSet TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);

            foreach (DataRow item in TicketID.Tables[0].Rows)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = (int)item[0];
                dsTicket = objBL_MapData.GetTicketByID(objMapData);
                if (ii == 0)
                {
                    dtTicket = dsTicket.Tables[0];
                    ii++;
                }
                else
                {
                    dtTicket.Rows.Add(dsTicket.Tables[0].Rows[0].ItemArray);
                    ii++;
                }
            }

            //////////////// 
            List<ReportParameter> param1 = new List<ReportParameter>();
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("dtInvoiceItems", _dtInvItems));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", _dtInvoice));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));
            if (dtTicket.Rows.Count > 0)
            {
                param1.Add(new ReportParameter("ISTicket", "1"));
            }
            else
            {
                param1.Add(new ReportParameter("ISTicket", "0"));
            }

            ///Email//
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.LocID = Convert.ToInt32(_dtInvoice.Rows[0]["Loc"]);
            DataSet dsloc = new DataSet();
            dsloc = objBL_User.getLocationByID(objPropUser);
            string EmailTo = "";
            string EmailCC = "";
            string LocID = "";
            if (dsloc.Tables[0].Rows.Count > 0)
            {
                LocID = dsloc.Tables[0].Rows[0]["ID"].ToString();
                EmailTo = dsloc.Tables[0].Rows[0]["custom12"].ToString();
                EmailCC = dsloc.Tables[0].Rows[0]["custom13"].ToString();
                ////
            }
            //param1.Add(new ReportParameter("EmailTo", EmailTo));
            //param1.Add(new ReportParameter("EmailCC", EmailCC));
            //param1.Add(new ReportParameter("LocID", LocID));
            string reportPath = "Reports/InvoiceLNY-WithTicket-Adams.rdlc";
            rvInvoices.LocalReport.ReportPath = reportPath;
            rvInvoices.LocalReport.EnableExternalImages = true;
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
            rvInvoices.LocalReport.SetParameters(param1);
            rvInvoices.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void PrintInvoiceWithTicketMRT(int invoiceNo, string templateName)
    {
        // Export to PDF
        try
        {
            List<byte[]> invoicesToPrint = GetInvoiceWithTicketReport(invoiceNo, templateName);

            if (invoicesToPrint != null)
            {
                byte[] buffer1 = null;
                buffer1 = concatAndAddContent(invoicesToPrint);

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", string.Format("attachment;filename=InvoiceWithTicket_{0}.pdf", invoiceNo));
                Response.AddHeader("Content-Length", (buffer1.Length).ToString());
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private List<byte[]> GetInvoiceWithTicketReport(int invoiceNo, string templateName)
    {
        // Export to PDF
        List<byte[]> templateAsBytes = new List<byte[]>();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            DataSet dsTicket = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath(string.Format("StimulsoftReports/Invoices/{0}", templateName));
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

            report.RegData("CompanyDetails", companyInfo.Tables[0]);

            DataTable dtInvoice = new DataTable();
            objProp_Contracts.InvoiceID = invoiceNo;
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
            dtInvoice = ds.Tables[0];

            DataTable dtInvItems = GetInvoiceItems(invoiceNo);

            DataTable dtTicket = new DataTable();
            int ii = 0;

            objProp_Contracts.InvoiceID = invoiceNo;
            DataSet TicketID = objBL_Contracts.GetAllTicketID(objProp_Contracts);

            foreach (DataRow item in TicketID.Tables[0].Rows)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = (int)item[0];
                dsTicket = objBL_MapData.GetTicketByID(objMapData);
                if (ii == 0)
                {
                    dtTicket = dsTicket.Tables[0];
                    ii++;
                }
                else
                {
                    dtTicket.Rows.Add(dsTicket.Tables[0].Rows[0].ItemArray);
                    ii++;
                }
            }

            report.RegData("InvoiceInfo", dtInvoice);
            report.RegData("InvoiceItems", dtInvItems);
            report.RegData("Tickets", dtTicket);

            var listTicketID = TicketID.Tables[0].Rows.OfType<DataRow>()
                  .Select(dr => dr.Field<int>("ID")).ToList();

            if (listTicketID.Count > 0)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                DataSet dsEquips = objBL_MapData.GetElevByTicketIDs(objMapData, string.Join(",", listTicketID));

                if (dsEquips != null)
                {
                    report.RegData("dtEquipment", dsEquips.Tables[0]);
                }
            }

            report.CacheAllData = true;
            report.Dictionary.Synchronize();
            report.Render();

            byte[] buffer1 = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(report, stream, settings);
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

    private bool CheckFilterDateWithGridDates(DataTable gridDT)
    {
        String fDate = txtFromDate.Text;
        String eDate = txtToDate.Text;
        if (fDate == "" && eDate == "")
        {
            eDate = Convert.ToString(Session["FreqToDate"]);
            fDate = Convert.ToString(Session["FreqfromDate"]);

        }

        if (eDate != "" && fDate != ""
            && gridDT.Rows.Count > 0)
        {
            string experssion = "fDate >=#" + Convert.ToDateTime(fDate).ToString("MM/dd/yyyy") + "#"
                       + " And fDate <= #" + Convert.ToDateTime(eDate).ToString("MM/dd/yyyy") + "#";

            // "date >= #" + from_date + "# AND date <= #" + to_date + "#"
            DataRow[] rows = gridDT.Select(experssion);

            if (rows.Count() == gridDT.Rows.Count)
            {
                return true;
            }
            else { return false; }
        }
        else
        {
            return false;
        }
    }

    protected void RadGrid_gvLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        DataSet dsLog = new DataSet();
        EmailLog emailLog = new EmailLog();
        emailLog.Screen = "Invoice";
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


    public void showFilterSearch()
    {
        ddlStatus.Style.Add("display", "none");
        txtSearch.Style.Add("display", "none");
        ddlDepartment.Style.Add("display", "none");
        txtInvDt.Style.Add("display", "none");
        ddllocation.Style.Add("display", "none");
        if (ddlSearch.SelectedValue == "i.Type")
        {

            ddlDepartment.Style.Add("display", "block");


        }
        else if (ddlSearch.SelectedValue == "i.Status")
        {
            ddlStatus.Style.Add("display", "block");

        }
        else if (ddlSearch.SelectedValue == "i.fdate")
        {

            txtInvDt.Style.Add("display", "block");

        }
        else if (ddlSearch.SelectedValue == "l.loc")
        {

            ddllocation.Style.Add("display", "block");
        }
        else
        {
            txtSearch.Style.Add("display", "block");
        }
    }
    public void UpdateControl()
    {
        IsGridPageIndexChanged = true;
        if (Session["filterstate"] != null)
        {
            if (Session["filterstate"].ToString() != string.Empty)
            {
                string[] strFilter = Session["filterstate"].ToString().Split(';');
                ddlSearch.SelectedValue = strFilter[0];
                ddllocation.SelectedValue = strFilter[1];
                ddlStatus.SelectedValue = strFilter[2];
                ddlDepartment.SelectedValue = strFilter[3];
                txtInvDt.Text = strFilter[4];
                txtSearch.Text = strFilter[5];
                txtFromDate.Text = strFilter[6];
                txtToDate.Text = strFilter[7];


                if (!string.IsNullOrEmpty(strFilter[9]))
                {
                    var selectedValArr = strFilter[9].Split(',');
                    foreach (RadComboBoxItem item in ddlpaidunpaid.Items)
                    {
                        if (Array.IndexOf(selectedValArr, item.Value) >= 0)
                        {
                            item.Checked = true;
                        }
                    }
                }

                ddlPrintOnly.SelectedValue = strFilter[10];
                hdnInvoiceSelectDtRange.Value = strFilter[11];
                isShowAll.Value = strFilter[12];


                lblDay.Attributes.Remove("class");
                lblWeek.Attributes.Remove("class");
                lblMonth.Attributes.Remove("class");
                lblQuarter.Attributes.Remove("class");
                lblYear.Attributes.Remove("class");
                switch (hdnInvoiceSelectDtRange.Value)
                {
                    case "Day":
                        lblDay.Attributes.Add("class", "labelactive");
                        break;
                    case "Week":
                        lblWeek.Attributes.Add("class", "labelactive");
                        break;
                    case "Month":
                        lblMonth.Attributes.Add("class", "labelactive");
                        break;
                    case "Quarter":
                        lblQuarter.Attributes.Add("class", "labelactive");
                        break;
                    case "Year":
                        lblYear.Attributes.Add("class", "labelactive");
                        break;
                        //default:
                        //    lblWeek.Attributes.Add("class", "labelactive");
                        //    break;
                }

                showFilterSearch();
            }
        }
    }
    private void resetClear()
    {
        IsGridPageIndexChanged = true;
        ddlSearch.SelectedValue = "";
        ddllocation.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        txtInvDt.Text = "";
        txtSearch.Text = "";
        ddlpaidunpaid.ClearCheckedItems();
        lblDay.Attributes.Remove("class");
        lblWeek.Attributes.Remove("class");
        lblMonth.Attributes.Remove("class");
        lblQuarter.Attributes.Remove("class");
        lblYear.Attributes.Remove("class");
        switch (hdnInvoiceSelectDtRange.Value)
        {
            case "Day":
                lblDay.Attributes.Add("class", "labelactive");
                break;
            case "Week":
                lblWeek.Attributes.Add("class", "labelactive");
                break;
            case "Month":
                lblMonth.Attributes.Add("class", "labelactive");
                break;
            case "Quarter":
                lblQuarter.Attributes.Add("class", "labelactive");
                break;
            case "Year":
                lblYear.Attributes.Add("class", "labelactive");
                break;
                //default:
                //    lblWeek.Attributes.Add("class", "labelactive");
                //    break;
        }
        showFilterSearch();
    }

    private void resetShowAll()
    {
        IsGridPageIndexChanged = true;
        lblDay.Attributes.Remove("class");
        lblWeek.Attributes.Remove("class");
        lblMonth.Attributes.Remove("class");
        lblQuarter.Attributes.Remove("class");
        lblYear.Attributes.Remove("class");
        ddlSearch.SelectedValue = "";
        ddllocation.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        txtInvDt.Text = "";
        txtSearch.Text = "";
        lblWeek.Attributes.Add("class", "labelactive");
    }

    protected void RadGrid_Invoice_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGrid_InvoiceCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_InvoiceminimumRows"] = e.NewPageIndex * RadGrid_Invoice.PageSize;
            ViewState["RadGrid_InvoicemaximumRows"] = (e.NewPageIndex + 1) * RadGrid_Invoice.PageSize;
        }
        catch { }
    }

    protected void RadGrid_Invoice_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGrid_InvoiceminimumRows"] = RadGrid_Invoice.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_InvoicemaximumRows"] = (RadGrid_Invoice.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }

    protected void RadGrid_Invoice_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    #region show/hidden column
    protected void lnkSaveGridSettings_Click(object sender, EventArgs e)
    {
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        #region Grid user settings
        var columnSettings = GetGridColumnSettings();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "Invoices.aspx";
        objProp_User.GridId = "RadGrid_Invoice";

        objBL_User.UpdateUserGridCustomSettings(objProp_User, columnSettings);
        #endregion
    }
    protected void lnkRestoreGridSettings_Click(object sender, EventArgs e)
    {
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        #region Grid user settings
        var columnSettings = string.Empty;
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "Invoices.aspx";
        objProp_User.GridId = "RadGrid_Invoice";

        var ds = objBL_User.DeleteUserGridCustomSettings(objProp_User);


        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            var colIndex = 0;

            foreach (GridColumn column in RadGrid_Invoice.MasterTableView.OwnerGrid.Columns)
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
            RadGrid_Invoice.MasterTableView.Rebind();
        }
        else
        {
            //var arrColumnOrder = new string[3]{ "ReviewCheck", "Comp", "" };
            var colIndex = 0;
            foreach (GridColumn column in RadGrid_Invoice.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                column.Display = true;

            }
            RadGrid_Invoice.MasterTableView.SortExpressions.Clear();
            RadGrid_Invoice.MasterTableView.GroupByExpressions.Clear();
            RadGrid_Invoice.EditIndexes.Clear();
            RadGrid_Invoice.Rebind();
        }
        CompanyPermission();
        #endregion
    }
    private string GetGridColumnSettings()
    {
        var columnSettings = string.Empty;

        List<ColumnSettings> lstColSetts = new List<ColumnSettings>();
        foreach (GridColumn column in RadGrid_Invoice.MasterTableView.OwnerGrid.Columns)
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
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        var columnSettings = string.Empty;
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.PageName = "Invoices.aspx";
        objProp_User.GridId = "RadGrid_Invoice";

        var ds = objBL_User.GetDefaultGridCustomSettings(objProp_User);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
        }

        return columnSettings;
    }

    private void DefineGridStructure()
    {
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();

        DataSet ds = new DataSet();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "Invoices.aspx";
        objProp_User.GridId = "RadGrid_Invoice";
        ds = objBL_User.GetGridUserSettings(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //string columnSettings = "[{Name: \"BType\", Display: true, Width: 300},{Name: \"MatItem\", Display: false, Width: 300}]";
            var columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            var colIndex = 0;

            foreach (GridColumn column in RadGrid_Invoice.MasterTableView.OwnerGrid.Columns)
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

            // ScriptManager.RegisterStartupScript(this, Page.GetType(), "showhidebutton", "ShowRestoreGridSettingsButton();", true);
        }
    }
    public class ColumnSettings
    {
        public string Name { get; set; }
        public bool Display { get; set; }
        public int Width { get; set; }
        public int OrderIndex { get; set; }
    }
    private DataTable createEmptyTable()
    {

        DataTable dt = new DataTable();
        dt.Columns.Add("Ref", typeof(int));
        dt.Columns.Add("PaymentReceivedDate", typeof(DateTime));
        dt.Columns.Add("SDesc", typeof(string));
        dt.Columns.Add("Loc", typeof(int));
        dt.Columns.Add("ID", typeof(string));
        dt.Columns.Add("Tag", typeof(string));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("locRemarks", typeof(string));
        dt.Columns.Add("JobRemarks", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("STax", typeof(double));
        dt.Columns.Add("Total", typeof(double));
        dt.Columns.Add("InvStatus", typeof(int));
        dt.Columns.Add("manualInv", typeof(int));
        dt.Columns.Add("status", typeof(string));
        dt.Columns.Add("EN", typeof(int));
        dt.Columns.Add("Company", typeof(string));
        dt.Columns.Add("PO", typeof(string));
        dt.Columns.Add("customername", typeof(string));
        dt.Columns.Add("type", typeof(string));
        dt.Columns.Add("Batch", typeof(int));
        dt.Columns.Add("Invbalance", typeof(double));
        dt.Columns.Add("balance", typeof(double));
        dt.Columns.Add("ddate", typeof(DateTime));
        dt.Columns.Add("WeekDate", typeof(DateTime));
        dt.Columns.Add("WipInvoice", typeof(int));
        dt.Columns.Add("JobStatus", typeof(int));
        return dt;

    }
    #endregion



    private void PrintInvoiceWithTicketMRT(DataTable _dtInvoice, string templateName)
    {
        // Export to PDF
        int invoiceNo = 0;
        List<byte[]> lstbyte = new List<byte[]>();
        try
        {
            foreach (DataRow drow in _dtInvoice.Rows)
            {
                invoiceNo = (int)drow[1];
                ViewState["invoiceNo"] = invoiceNo;
                List<byte[]> invoicesToPrint = GetInvoiceWithTicketReport(invoiceNo, templateName);

                if (invoicesToPrint != null)
                {
                    byte[] buffer1 = null;
                    buffer1 = concatAndAddContent(invoicesToPrint);
                    lstbyte.Add(buffer1);
                }

            }
            String fileName = "InvoiceWithTicket.pdf";
            if (_dtInvoice.Rows.Count == 1)
            {
                fileName = "InvoiceWithTicket_" + invoiceNo + ".pdf";
            }
            byte[] allbyte = Invoices.concatAndAddContent(lstbyte);
            Response.Clear();
            MemoryStream ms = new MemoryStream(allbyte);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", fileName));
            Response.AddHeader("Content-Length", (allbyte.Length).ToString());
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
    protected void RadGrid_Invoice_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
    {
        string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Contracts.GetInvoiceStatus(objProp_Contracts);
        if (ds.Tables[0] != null)
        {
            e.ListBox.DataSource = ds.Tables[0];
            e.ListBox.DataKeyField = DataField;
            e.ListBox.DataTextField = DataField;
            e.ListBox.DataValueField = DataField;
            e.ListBox.DataBind();
        }
    }

    protected void lnkInvoicesReportwithPayment_Click(object sender, EventArgs e)
    {
        processRedirect("InvoicesReportwithPayment.aspx?page=invoices");

    }

    protected void lnkARAgingReport_Click(object sender, EventArgs e)
    {
        processRedirect("ARAgingReport.aspx?page=invoices");

    }

    protected void lnkARAgingReportCust_Click(object sender, EventArgs e)
    {
        processRedirect("ARAgingReportCust.aspx?page=invoices");

    }

    protected void lnkARAgingReportByLocation_Click(object sender, EventArgs e)
    {
        processRedirect("ARAgingReportByLocation.aspx?page=invoices");

    }

    protected void lnkARAgingReportDep_Click(object sender, EventArgs e)
    {
        processRedirect("ARAgingReportDep.aspx?page=invoices");

    }

    protected void lnkARAgingReportByTerritory_Click(object sender, EventArgs e)
    {
        processRedirect("ARAgingReportByTerritory.aspx?page=invoices");

    }

    protected void lnkSalesTaxReport_Click(object sender, EventArgs e)
    {
        processRedirect("SalesTaxReport.aspx?redirect=Invoices.aspx?fil=1");

    }

    protected void lnkSalesTax2Report_Click(object sender, EventArgs e)
    {
        processRedirect("SalesTax2Report.aspx?page=invoices");

    }

    protected void lnkPrintInvoiceRegisterGL_Click(object sender, EventArgs e)
    {
        processRedirect("PrintInvoiceRegisterGL.aspx?page=invoices");

    }

    protected void lnkInvoiceByRecurringFrequency_Click(object sender, EventArgs e)
    {
        processRedirect("InvoiceByRecurringFrequency.aspx?page=invoices");

    }

    protected void lnkARAgingReportByJobType_Click(object sender, EventArgs e)
    {
        processRedirect("ARAgingReportByJobType.aspx?page=invoices");

    }

    protected void lnkARAgingReportByLocType_Click(object sender, EventArgs e)
    {
        processRedirect("ARAgingReportByLocType.aspx?page=invoices");

    }

    protected void lnkAgedReceivableReport_Click(object sender, EventArgs e)
    {
        processRedirect("AgedReceivableReport.aspx?page=invoices");

    }

    protected void lnkSalesTaxCollectedReport_Click(object sender, EventArgs e)
    {
        processRedirect("SalesTaxCollectedReport.aspx?redirect=invoices?fil=1");
    }
    private void processRedirect(string url)
    {
        SaveFilter();
        Response.Redirect(url, true);
    }

    private DataTable GetDataForEmail(int Paid)
    {
        List<RetainFilter> filters = new List<RetainFilter>();
        String filterExpression = Convert.ToString(RadGrid_Invoice.MasterTableView.FilterExpression);
        if (!IsPostBack)
        {
            if (filterExpression == "")
            {
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["Invoice_FilterExpression"] != null && Convert.ToString(Session["Invoice_FilterExpression"]) != "" && Session["Invoice_Filters"] != null)
                    {
                        filterExpression = Convert.ToString(Session["Invoice_FilterExpression"]);
                        RadGrid_Invoice.MasterTableView.FilterExpression = Convert.ToString(Session["Invoice_FilterExpression"]);
                        var filtersGet = Session["Invoice_Filters"] as List<RetainFilter>;
                        if (filtersGet != null)
                        {
                            foreach (var _filter in filtersGet)
                            {                           
                                var filterCol = _filter.FilterColumn;
                                if (filterCol == "Status")
                                {
                                    GridColumn column = RadGrid_Invoice.MasterTableView.GetColumnSafe("Status");

                                    if (column != null)
                                    {
                                        column.ListOfFilterValues = _filter.FilterValue.Replace("'", "").Split(',');
                                    }
                                }
                                else
                                {
                                    GridColumn column = RadGrid_Invoice.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                                    if (column != null)
                                    {
                                        column.CurrentFilterValue = _filter.FilterValue;
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {
                    Session["Invoice_FilterExpression"] = null;
                    Session["Invoice_Filters"] = null;
                }

            }
        }
        //if (!IsGridPageIndexChanged)
        //{
        //    RadGrid_Invoice.CurrentPageIndex = 0;
        //    Session["RadGrid_InvoiceCurrentPageIndex"] = 0;
        //    ViewState["RadGrid_InvoiceminimumRows"] = 0;
        //    ViewState["RadGrid_InvoicemaximumRows"] = RadGrid_Invoice.PageSize;
        //}
        //else
        //{
        //    if (Session["RadGrid_InvoiceCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_InvoiceCurrentPageIndex"].ToString()) != 0
        //        && Request.QueryString["fil"] != null && Request.QueryString["fil"].ToString() == "1")
        //    {
        //        RadGrid_Invoice.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_InvoiceCurrentPageIndex"].ToString());
        //        ViewState["RadGrid_InvoiceminimumRows"] = RadGrid_Invoice.CurrentPageIndex * RadGrid_Invoice.PageSize;
        //        ViewState["RadGrid_InvoicemaximumRows"] = (RadGrid_Invoice.CurrentPageIndex + 1) * RadGrid_Invoice.PageSize;

        //    }
        //}

        if (string.IsNullOrEmpty(filterExpression) && Session["Invoice_FilterExpression"] != null)
        {
            filterExpression = Convert.ToString(Session["Invoice_FilterExpression"]);
        }


        isGridFilterInvoice = false;

        if (filterExpression != "")
        {
            Session["Invoice_FilterExpression"] = filterExpression;
            foreach (GridColumn column in RadGrid_Invoice.MasterTableView.OwnerGrid.Columns)
            {
                
                String filterValues = String.Empty;
                String columnName = column.UniqueName;

                if (column.UniqueName == "Status")
                {
                    if (column.ListOfFilterValues != null)
                    {
                        List<string> listFil = new List<string>(column.ListOfFilterValues);
                        filterValues = String.Join(",", listFil.Select(x => string.Format("{0}", x)));
                        columnName = "Status";
                    }
                    else
                    {
                        filterValues = column.CurrentFilterValue;
                    }
                }
                else
                {
                    filterValues = column.CurrentFilterValue;
                }

                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                    if (column.UniqueName == "InvoiceRef")
                    {
                        isGridFilterInvoice = true;
                    }
                }

            }

            Session["Invoice_Filters"] = filters;
        }

        _filteredbyloc = false;
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        if (Paid == 0)
        {
            objProp_Contracts.SearchBy = ddlSearch.SelectedValue;
            if (ddlSearch.SelectedValue == "i.Type")
            {
                objProp_Contracts.SearchValue = ddlDepartment.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "i.Status")
            {
                objProp_Contracts.SearchValue = ddlStatus.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "i.fdate")
            {
                objProp_Contracts.SearchValue = txtInvDt.Text;
            }
            else if (ddlSearch.SelectedValue == "l.loc")
            {
                objProp_Contracts.SearchValue = ddllocation.SelectedValue;
                _filteredbyloc = true;
            }
            else if (ddlSearch.SelectedValue == "i.ref")

            {
                if (txtSearch.Text != string.Empty)
                {


                    if ((txtSearch.Text[0] == '=' || txtSearch.Text[0] == '>' || txtSearch.Text[0] == '<' || txtSearch.Text[0] == 'b' || txtSearch.Text[0] == 'B'))
                    {
                        if (txtSearch.Text.Trim().Length >= 2)
                        {
                            txtSearch.Text = " " + txtSearch.Text;
                            objProp_Contracts.SearchValue = txtSearch.Text.Replace("'", "''");
                        }
                    }
                    else
                    {
                        if (txtSearch.Text.IndexOf('=') > -1 || txtSearch.Text.IndexOf('>') > -1 || txtSearch.Text.IndexOf('<') > -1)
                            txtSearch.Text = txtSearch.Text;
                        else
                            txtSearch.Text = "=" + txtSearch.Text;
                        objProp_Contracts.SearchValue = txtSearch.Text.Replace("'", "''");
                    }                   
                }
                else
                {
                    txtSearch.Text = "=-1";
                    objProp_Contracts.SearchValue = txtSearch.Text.Replace("'", "''");
                }
            }
            else
            {
                objProp_Contracts.SearchValue = txtSearch.Text.Replace("'", "''");
            }
            if (txtFromDate.Text != string.Empty)
            {
                objProp_Contracts.StartDate = Convert.ToDateTime(txtFromDate.Text);
            }
            else
            {
                objProp_Contracts.StartDate = System.DateTime.MinValue;
            }

            if (txtToDate.Text != string.Empty)
            {
                objProp_Contracts.EndDate = Convert.ToDateTime(txtToDate.Text);
            }
            else
            {
                objProp_Contracts.EndDate = System.DateTime.MinValue;
            }

            var typeSearchValue = string.Empty;
            if (ddlpaidunpaid.CheckedItems.Count > 0)
            {
                foreach (var item in ddlpaidunpaid.CheckedItems)
                {
                    typeSearchValue += item.Value + ",";
                }

                typeSearchValue = typeSearchValue.TrimEnd(',');

                objProp_Contracts.SearchAmtPaidUnpaid = "(" + typeSearchValue + ")";
            }


            if (ddlPrintOnly.SelectedValue == "All")
                objProp_Contracts.SearchPrintMail = string.Empty;
            else if (ddlPrintOnly.SelectedValue == "PrintOnly")
                objProp_Contracts.SearchPrintMail = "P";
            else if (ddlPrintOnly.SelectedValue == "Mail")
                objProp_Contracts.SearchPrintMail = "M";
        }
        else
        {
            objProp_Contracts.SearchValue = string.Empty;
            objProp_Contracts.SearchBy = string.Empty;
            objProp_Contracts.StartDate = System.DateTime.MinValue;
            objProp_Contracts.EndDate = System.DateTime.MinValue;
        }


        objProp_Contracts.CustID = Convert.ToInt32(Session["custid"].ToString());
        objProp_Contracts.Paid = Paid;

        if (Session["type"].ToString() == "c")
        {
            DataTable dtcust = new DataTable();
            dtcust = (DataTable)Session["userinfo"];
            int RoleID = 0;
            if (dtcust.Rows.Count > 0)
            {
                RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                objProp_Contracts.RoleId = RoleID;
            }
        }
        /****Get from MS_Invoice tables the invoices masrked as pending from Mobile Service in case of TS database****/
        if (Session["MSM"].ToString() == "TS")
        {
            if (Session["type"].ToString() != "c")
                objProp_Contracts.isTS = 1;
        }
        /***/
        #region Company Check
        objProp_Contracts.UserID = Session["UserID"].ToString();
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objProp_Contracts.EN = 1;
        }
        else
        {
            objProp_Contracts.EN = 0;
        }
        #endregion
        try
        {            

            ds = objBL_Contracts.GetInvoices(objProp_Contracts, filters, Convert.ToString(Session["FreqfromDate"]), Convert.ToString(Session["FreqToDate"]));

            //duplicate
            DataTable dt = RemoveDuplicateRows(ds.Tables[0], "Ref");
            return dt;
            
        }
        catch (Exception ex)
        {
            return null;
        }

    }

  
    protected void lnkARAging360ReportByLocation_Click(object sender, EventArgs e)
    {
        processRedirect("ARAgingReport360ByLocation?page=invoices");
    }

    protected void lnkARAging360ReportByCust_Click(object sender, EventArgs e)
    {
        processRedirect("ARAgingReport360ByCustomer?page=invoices");
    }
    public Boolean isCanadaCompany()
    {
        //todo
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();
        Boolean flag = false;
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = _objBLGeneral.getCompanyCountry(_objPropGeneral);
        try
        {
            if (_dsCustom.Tables[0].Rows[0]["Country"].ToString() == "Canada")
            {
                flag = true;
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }

    private void SetupCanadaCompanyUI()
    {
        //todo
        BL_General objBL_General = new BL_General();
        General objGenerals = new General();
       
        if (isCanadaCompany())
        {

            //for grid
            RadGrid_Invoice.Columns.FindByUniqueName("SalesTax").HeaderText = "HST/PST Tax";
            RadGrid_Invoice.Columns.FindByUniqueName("GSTTax").Visible = true;

        }
        else
        {
            RadGrid_Invoice.Columns.FindByUniqueName("SalesTax").HeaderText = "Sales Tax";
            RadGrid_Invoice.Columns.FindByUniqueName("GSTTax").Visible = false;

        }


    }
}

