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
using Microsoft.Reporting.WebForms;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using BusinessLayer.Schedule;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Linq.Dynamic;
using Stimulsoft.Report.Web;
using Stimulsoft.Report;
using System.Text;
using System.Web.Configuration;
using System.Collections;

[Serializable]
public partial class TicketListView : Page
{
    BL_MapData objBL_MapData = new BL_MapData();

    MapData objMapData = new MapData();

    BL_User objBL_User = new BL_User();

    User objProp_User = new User();

    BL_Customer objBL_Customer = new BL_Customer();

    Customer objPropCustomer = new Customer();

    BL_Contracts objBL_Contracts = new BL_Contracts();

    Contracts objProp_Contracts = new Contracts();

    BL_General objBL_General = new BL_General();

    General objGeneral = new General();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

    BL_Report objBL_Report = new BL_Report();

    private const string ASCENDING = " ASC";

    private const string DESCENDING = " DESC";

    private string ddlSearchReportID = "0";

    private bool IsGridPageIndexChanged = false;

    int count1 = 0;

    public static int intLocationCount = 0;

    public Boolean isAssignProject = false;


    public int EmpId;

    #region Page Event
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now);
        Response.Cache.SetNoServerCaching();
        Response.Cache.SetNoStore();

        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            if (!IsPostBack)
            {
                SetDefaultWorker();
                ViewState["RadGvTicketListminimumRows"] = 0;
                ViewState["RadGvTicketListmaximumRows"] = 50;

                if (string.IsNullOrWhiteSpace(GetDefaultGridColumnSettingsFromDb()))
                {
                    // Get initial grid settings
                    var gridDefault = GetGridColumnSettings();
                    // Save default settings to database
                    objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
                    objProp_User.UserID = 0;// UserId = 0 for default
                    objProp_User.PageName = "TicketListView.aspx";
                    objProp_User.GridId = "RadGvTicketList";

                    objBL_User.UpdateUserGridCustomSettings(objProp_User, gridDefault);
                }

                DeleteTempDocumentFiles();
                GetControl();
                if (Session["type"].ToString() == "c")
                {
                    chkHideTicketDesc.Visible = false;
                    lnkVoided.Visible = false;
                    DataTable dtuserinfo = (DataTable)Session["userinfo"];
                    if (dtuserinfo.Rows[0]["openticket"].ToString() == "0")
                    {
                        ddlStatus.SelectedValue = "4";
                        ddlStatus.Enabled = false;
                        ddlStatus.Visible = false;
                        lblStatus.Visible = false;
                        divStatus.Visible = false;

                    }
                    else
                    {
                        ddlStatus.SelectedValue = "-2";
                        ddlStatus.Enabled = true;
                        ddlStatus.Visible = true;
                        lblStatus.Visible = true;
                        divStatus.Visible = true;
                    }

                    ddlReviewed.SelectedValue = "1";
                    ddlReviewed.Enabled = false;

                    lnkDelete.Visible = false;
                    lnkAddticket.Visible = false;
                    lnkRequestForService.Visible = true;
                    btnEdit.Visible = true;
                    btnEdit.Text = "View Ticket";
                    lnkPrint.Visible = false;

                    lnkCopy.Visible = false;
                    lnkExcel.Visible = false;
                    ddlReviewed.Visible = false;
                    lblReviewed.Visible = false;
                    lnkMailAll.Visible = false;
                    lnkShowAll.Visible = false;
                    ddlMobile.Visible = false;
                    lblMobile.Visible = false;

                    lblLoc.Visible = true;
                    ddllocation.Visible = true;
                    divLoc.Visible = true;

                    lblWO.Visible = true;
                    txtWo.Visible = true;
                    divWO.Visible = true;

                    ddlSearch.Items[1].Enabled = false;
                    ddlSearch.Items[2].Enabled = false;

                    ddlTimeS.Visible = false;
                    lblTimeSh.Visible = false;

                    ddlSuper.Visible = false;
                    ddlworker.Visible = false;
                    ddlRoute.Visible = false;

                    lblSuper.Visible = false;
                    lblWorker.Visible = false;
                    lblRoute.Visible = false;

                    divSuper.Visible = false;
                    divWorker.Visible = false;
                    divRoute.Visible = false;
                    divTimeSheet.Visible = false;
                    divMobile.Visible = false;
                    divReviewed.Visible = false;
                }

                string[] tokens = Session["config"].ToString().Split(';');
                if (tokens[1].ToString().ToLower().IndexOf("ahei") != -1)
                {
                    lnkCategoryDueReport.Visible = true;
                }
                else
                {
                    lnkCategoryDueReport.Visible = false;
                }

                if (tokens[1].ToString().ToLower().IndexOf("westcoast") != -1)
                {
                    lnkBuildingReportTemplate.Visible = true;
                }
                else
                {
                    lnkBuildingReportTemplate.Visible = false;
                }

                if (tokens[1].ToString().ToLower().IndexOf("madden") != -1)
                {
                    lnkLaborbyDepartment.Visible = true;
                }
                else
                {
                    lnkLaborbyDepartment.Visible = false;
                }

                if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("gable"))
                {
                    lnkTicketListPayroll.Visible = true;
                }

                if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("accredited"))
                {
                    RadGvTicketList.Columns.FindByUniqueName("customername").Visible = true;
                    RadGvTicketList.Columns.FindByUniqueName("State").Visible = true;
                    RadGvTicketList.Columns.FindByUniqueName("Zip").Visible = true;
                    RadGvTicketList.Columns.FindByUniqueName("Phone").Visible = true;
                    RadGvTicketList.Columns.FindByUniqueName("Email").Visible = true;
                    RadGvTicketList.Columns.FindByUniqueName("PassedInspection").Visible = true;
                }

                Locations();
                RestoreDateRange();
                FillSupervisor();
                FillWorker();
                FillRoute();
                FillCategory();
                FillLevels();
                FillDepartment();
                if (Session["type"].ToString() != "c")
                {
                    GetPreferences();
                }

                ConvertToJSON();

                RadGvRequestForServiceFill();

                LoadDataIntoReviewDropdownLists();
                if (Session["type"].ToString() == "c")
                {
                    lnkRequestForService.Visible = false;
                    //txtfromDate.Text = string.Empty;
                    //txtToDate.Text = string.Empty;
                    //txtfromDate.Attributes.Add("placeholder", "Select Start Date");
                    //txtToDate.Attributes.Add("placeholder", "Select End Date");
                }

                var userinfo = (DataTable)Session["userinfo"];
                int usertypeid = 0;
                if (userinfo != null)
                {
                    usertypeid = Convert.ToInt32(userinfo.Rows[0]["usertypeid"]);
                }

                if (usertypeid == 2)
                {
                    liLogs.Style["display"] = "none";
                    tbLogs.Style["display"] = "none";
                }
                else
                {
                    liLogs.Style["display"] = "inline-block";
                    tbLogs.Style["display"] = "block";
                }

                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["TicketReportFormat"]) || !ConfigurationManager.AppSettings["TicketReportFormat"].ToString().ToLower().Equals("mrt"))
                {
                    lnkTimesheetbyWageCategory.Visible = false;
                    hdnTicketReportFormat.Value = "rdlc";
                }
                else
                {
                    hdnTicketReportFormat.Value = ConfigurationManager.AppSettings["TicketReportFormat"].ToString().ToLower();
                }

                UpdateControlValues();
                lnkOpenTicketReportbyRoutes.Text = string.Format("Open Ticket by {0} Report", getOpenTicketReportName());
            }

            if (Convert.ToString(Session["MailTicketSend"]) == "true")
            {
                Session["MailTicketSend"] = null;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }

            Permission();
            CompanyPermission();
            HighlightSideMenu("schMgr", "lnkListView", "schdMgrSub");
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrPage_Load", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }


    private String getOpenTicketReportName()
    {
        BL_Customer objBL_Customer = new BL_Customer();
        Customer objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getReportName(objCustomer);
        try
        {
            return ds.Tables[0].Rows[0]["Label"].ToString();
        }
        catch
        {
            return "Routes";
        }
    }
    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }

    private void LoadDataIntoReviewDropdownLists()
    {
        FillReviewDepartment();
        FillReviewPayroll();
        FillBillCodesService();
        GetQBInt();
    }

    private void FillBillCodesService()
    {
        BL_Contracts objBL_Contracts = new BL_Contracts();
        Contracts objProp_Contracts = new Contracts();

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Contracts.GetBillcodesforTimeSheet(objProp_Contracts);

        ddlReviewService.DataSource = ds.Tables[0];
        ddlReviewService.DataTextField = "billcode";
        ddlReviewService.DataValueField = "QBinvid";
        ddlReviewService.DataBind();
        ddlReviewService.Items.Insert(0, new ListItem("Service Item", ""));
    }

    private void FillReviewPayroll()
    {
        BL_Contracts objBL_Contracts = new BL_Contracts();
        Contracts objProp_Contracts = new Contracts();

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        var ds = objBL_Contracts.GetPayrollforTimeSheet(objProp_Contracts);
        ddlReviewPayroll.DataSource = ds.Tables[0];
        ddlReviewPayroll.DataTextField = "fdesc";
        ddlReviewPayroll.DataValueField = "QBwageid";
        ddlReviewPayroll.DataBind();
        ddlReviewPayroll.Items.Insert(0, new ListItem("Payroll Item", ""));
    }

    private void FillReviewDepartment()
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        var ds = objBL_User.getDepartment(objProp_User);
        ddlReviewDepartment.DataSource = ds.Tables[0];
        ddlReviewDepartment.DataTextField = "type";
        ddlReviewDepartment.DataValueField = "id";
        ddlReviewDepartment.DataBind();
        ddlReviewDepartment.Items.Insert(0, new ListItem("Department", ""));
    }

    private void GetQBInt()
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        var ds = objBL_User.getControl(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["QBIntegration"].ToString() == "1")
            {
                ddlReviewPayrollDiv.Visible = true;
                ddlReviewServiceDiv.Visible = true;
                ddlReviewService.Visible = true;
                ddlReviewPayroll.Visible = true;
                ddlReviewDepartmentDiv.Visible = true;
                hdnIsQBInt.Value = "1";
            }
            else
            {
                ddlReviewPayrollDiv.Visible = false;
                ddlReviewServiceDiv.Visible = false;
                ddlReviewService.Visible = false;
                ddlReviewPayroll.Visible = false;
                ddlReviewDepartmentDiv.Visible = false;
                hdnIsQBInt.Value = "0";
            }
        }
    }

    protected void Page_PreRender(Object o, EventArgs e)
    {
        try
        {

            if (ViewState["PR"].ToString() == "False")
            {
                RadGvTicketList.MasterTableView.Columns.FindByUniqueName("PayrollHeader").Visible = false;
            }

            foreach (GridDataItem gr in RadGvTicketList.Items)
            {
                Image imgTimeS = (Image)gr.FindControl("imgTimeS");
                Image imgPortal = (Image)gr.FindControl("imgPortal");
                Image imgStaus = (Image)gr.FindControl("imgStaus");
                Image imgDoc = (Image)gr.FindControl("imgDoc");
                Image imgSign = (Image)gr.FindControl("imgSign");
                Image imgCharge = (Image)gr.FindControl("imgCharge");
                Image imgHigh = (Image)gr.FindControl("imgHigh");
                Image imgRecommend = (Image)gr.FindControl("imgRecommend");
                Image imgOverTime = (Image)gr.FindControl("imgOverTime");
                Image imgpayroll = (Image)gr.FindControl("imgpayroll");

                Label lblSumOfTotalTime = (Label)gr.FindControl("lblSumOfTotalTime");
                GridFooterItem footeritem = (GridFooterItem)RadGvTicketList.MasterTableView.GetItems(GridItemType.Footer)[0];
                Label lbl = (Label)footeritem.FindControl("lblfTottime");
                lbl.Text = lblSumOfTotalTime.Text;

                imgPortal.Visible = imgPortal.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                imgStaus.Visible = imgStaus.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                imgDoc.Visible = imgDoc.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                imgSign.Visible = imgSign.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                imgCharge.Visible = imgCharge.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                imgTimeS.Visible = imgTimeS.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                imgHigh.Visible = imgHigh.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                imgRecommend.Visible = imgRecommend.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                imgOverTime.Visible = imgOverTime.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                imgpayroll.Visible = imgpayroll.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;

                if (imgCharge.ImageUrl == "images/dollar.png") { imgCharge.ToolTip = "Invoiced"; }
                if (imgStaus.ImageUrl == "images/review.png") { imgStaus.ToolTip = "Reviewed"; }
                else if (imgStaus.ImageUrl == "images/1331034893_pda.png") { imgStaus.ToolTip = "MS"; }

                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                Label lblComp = (Label)gr.FindControl("lblComp");
                Label lblTicketId = (Label)gr.FindControl("lblTicketId");
                HyperLink lbllnk = (HyperLink)gr.FindControl("lbllnk");
                CheckBox chkPayroll = (CheckBox)gr.FindControl("chkPayroll");

                if (ViewState["PR"].ToString() == "False")
                {
                    chkPayroll.Visible = false;
                }

                if (Session["type"].ToString() == "c")
                {
                    if (btnEdit.Visible == true)
                    {
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TicketReportFormat"]) && ConfigurationManager.AppSettings["TicketReportFormat"].ToString().ToLower().Equals("mrt"))
                        {
                            lbllnk.Attributes["onclick"] = gr.Attributes["ondblclick"] = "window.open('TicketReport.aspx?id=" + lblTicketId.Text + "','_blank');";
                        }
                        else
                        {
                            lbllnk.Attributes["onclick"] = gr.Attributes["ondblclick"] = "window.open('Printticket.aspx?id=" + lblTicketId.Text + "&c=" + lblComp.Text + "&pop=1','_blank');";
                        }
                    }

                    imgpayroll.Visible = false;
                }
                else
                {
                    if (btnEdit.Visible == true)
                    {
                        if (hdnEditeTicket.Value == "Y" || hdnViewTicket.Value == "Y")
                        {
                            gr.Attributes["ondblclick"] = "location.href='addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text + "&pop=1&fr=tlv'";
                            lbllnk.NavigateUrl = "addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text + "&pop=1&fr=tlv";
                        }
                        else
                        {
                            lbllnk.Attributes["onclick"] = gr.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
                        }
                    }
                }

                Label lblRes = (Label)gr.FindControl("lblRes");
                if (Session["type"].ToString() == "c")
                {
                    chkHideTicketDesc.Checked = false;
                }
                else
                {
                    if (!chkHideTicketDesc.Checked)
                    {
                        gr.Attributes["onmouseover"] = "HoverMenutext('" + gr.ClientID + "','" + lblRes.ClientID + "',event);";
                        gr.Attributes["onmouseout"] = " $('#" + lblRes.ClientID + "').hide();";
                    }

                    lblRes.Visible = chkHideTicketDesc.Checked ? false : true;
                }
            }
        }
        catch { }
    }

    protected void Page_Init(object source, System.EventArgs e)
    {

        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        DefineGridStructure();
    }

    private void DefineGridStructure()
    {
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();

        DataSet ds = new DataSet();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "TicketListView.aspx";
        objProp_User.GridId = "RadGvTicketList";
        ds = objBL_User.GetGridUserSettings(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            var columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            var colIndex = 0;

            foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
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
        }
    }
    #endregion

    private void Permission()
    {



        if (Session["type"].ToString() == "c" || Session["MSM"].ToString() == "TS")

        {
            ViewState["PR"] = "False";
        }

        if (Session["type"].ToString() == "c")
        {
            cbReview.Visible =

            divPayroll.Visible =
            DivEmail.Visible = false;
        }


        ViewState["MassReviewCheck"] = "Y";

        ViewState["MassTimesheetCheck"] = "Y";

        ViewState["MassPayrollTicket"] = "Y";

        if (Session["MSM"].ToString() == "TS")
        {
            lnkDelete.Visible = false;
            cbReview.Visible = false;
        }
        else
        {
            if (Convert.ToInt32(Session["ISsupervisor"]) != 1)
            {
                if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
                {
                    DataTable dt = new DataTable();

                    dt = GetUserById();

                    if (dt.Rows[0]["massreview"].ToString() == "0")
                    {
                        cbReview.Visible = false;

                        ViewState["MassReviewCheck"] = "N";
                    }


                    ViewState["MassTimesheetCheck"] = dt.Rows[0]["MassTimesheetCheck"].ToString();

                    ViewState["MassPayrollTicket"] = dt.Rows[0]["MassPayrollTicket"].ToString();


                }
            }
        }



        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// RCmodulePermission ///////////////////------->
            EmpId = ds.Rows[0]["Empid"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Rows[0]["Empid"]);
            isAssignProject = ds.Rows[0]["IsAssignedProject"] == DBNull.Value ? false : Convert.ToBoolean(ds.Rows[0]["IsAssignedProject"]);

            string SchedulemodulePermission = ds.Rows[0]["SchedulemodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["SchedulemodulePermission"].ToString();

            if (SchedulemodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            string TicketVoidPermission = ds.Rows[0]["TicketVoidPermission"] == DBNull.Value ? "N" : ds.Rows[0]["TicketVoidPermission"].ToString();

            hdnTicketVoidPermission.Value = TicketVoidPermission == "1" ? "Y" : "N";

            string TicketPermission = ds.Rows[0]["dispatch"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["dispatch"].ToString();
            string ADD = hdnAddeTicket.Value = TicketPermission.Length < 1 ? "Y" : TicketPermission.Substring(0, 1);
            string Edit = hdnEditeTicket.Value = TicketPermission.Length < 2 ? "Y" : TicketPermission.Substring(1, 1);
            string Delete = hdnDeleteTicket.Value = TicketPermission.Length < 3 ? "Y" : TicketPermission.Substring(2, 1);
            string View = hdnViewTicket.Value = TicketPermission.Length < 4 ? "Y" : TicketPermission.Substring(3, 1);
            string Report = TicketPermission.Length < 6 ? "Y" : TicketPermission.Substring(5, 1);
            string ResolvedTicketPermission = ds.Rows[0]["Resolve"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Resolve"].ToString();
            string DeleteResolved = hdnDeleteResolvedTicket.Value = ResolvedTicketPermission.Length < 3 ? "Y" : ResolvedTicketPermission.Substring(2, 1);

            if (ADD == "N")
            {
                lnkAddticket.Visible = false;
            }
            if (Edit == "N")
            {
                btnEdit.Visible = false;
                lnkCopy.Visible = false;
            }
            if (Delete == "N" && DeleteResolved == "N")
            {
                lnkDelete.Visible = false;
            }
            if (Report == "N")
            {
                lnkPrint.Visible = false;
                lnkExcel.Visible = false;
                lnkPDF.Visible = false;
                lnkMailAll.Visible = false;
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

    private void CompanyPermission()
    {
        if (Session["COPer"] != null && Session["COPer"].ToString() == "1")
        {
            RadGvRequestForService.Columns.FindByUniqueName("Company").Visible = true;
            RadGvTicketList.Columns.FindByUniqueName("Company").Visible = true;

        }
        else
        {
            RadGvRequestForService.Columns.FindByUniqueName("Company").Visible = false;
            RadGvTicketList.Columns.FindByUniqueName("Company").Visible = false;
        }

        if (Session["type"].ToString() == "c")
        {

            RadGvTicketList.Columns.FindByUniqueName("manualinvoice").Visible = false;
            RadGvTicketList.MasterTableView.EditMode = Telerik.Web.UI.GridEditMode.InPlace;



        }
    }

    private void Locations()
    {

        try
        {
            if (Session["type"].ToString() == "c")
            {
                DataTable dtcust = new DataTable();
                dtcust = (DataTable)Session["userinfo"];
                int RoleID = 0;
                if (dtcust.Rows.Count > 0)
                {
                    RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                    objProp_User.RoleID = RoleID;
                }
            }

            DataSet ds = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();

            objProp_User.CustomerID = Convert.ToInt32(Session["custid"].ToString());

            ds = objBL_User.getLocationByCustomerID(objProp_User);

            ddllocation.DataSource = ds.Tables[0];
            ddllocation.DataTextField = "tag";
            ddllocation.DataValueField = "loc";
            ddllocation.DataBind();
            intLocationCount = ds.Tables[0].Rows.Count;
            ddllocation.Items.Insert(0, new ListItem("All", "0"));
            ddllocation.Items.Insert(0, new ListItem("Select a Location", "-1"));
        }
        catch { }
    }

    //protected void ddllocation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    btnSearch_Click(sender, e);
    //}

    private void FillCategory()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCategory(objProp_User);

        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "type";
        ddlCategory.DataValueField = "type";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("All", ""));
        ddlCategory.Items.Insert(1, new ListItem("None", "None"));

        RadComboBoxItem listitem1 = new RadComboBoxItem("======= ACTIVE CATEGORY ======= ", "Active");
        listitem1.CssClass = "GroupStatus";
        chkcatlist.Items.Add(listitem1);

        chkcatlist.Items.Add(new RadComboBoxItem("None", "None"));
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["Status"].ToString() == "True")
            {
                var cboItem = new RadComboBoxItem() { Text = dr["type"].ToString(), Value = dr["type"].ToString() };
                chkcatlist.Items.Add(cboItem);
            }
        }
        RadComboBoxItem listitem2 = new RadComboBoxItem("======= INACTIVE CATEGORY ======= ", "Inactive");
        listitem2.CssClass = "GroupStatus";
        chkcatlist.Items.Add(listitem2);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["Status"].ToString() == "False")
            {
                var cboItem = new RadComboBoxItem() { Text = dr["type"].ToString(), Value = dr["type"].ToString() };
                chkcatlist.Items.Add(cboItem);
            }
        }

        if (Session["TicketListViewCat"] != null & Request.QueryString["fil"] != null)
        {
            if (!string.IsNullOrEmpty(Session["TicketListViewCat"].ToString()))
            {
                List<string> result = Session["TicketListViewCat"].ToString().Split(',').ToList();

                foreach (RadComboBoxItem itm in chkcatlist.Items)
                {
                    string s1 = itm.Text;

                    foreach (var item in result)
                    {
                        string s2 = item.ToString().Replace("'", "");

                        if (s1 == s2)
                        {
                            itm.Checked = true;
                        }
                    }
                }
            }
        }

        else
        {
            foreach (RadComboBoxItem itm in chkcatlist.Items)
            {
                itm.Checked = true;
            }
        }
    }

    private void FillLevels()
    {
        DataSet dsL = new DataSet();

        objProp_User.ConnConfig = Session["config"].ToString();

        dsL = objBL_User.getLevels(objProp_User);
        chkrcbLevel.DataSource = dsL.Tables[0];
        chkrcbLevel.DataTextField = "Label";
        chkrcbLevel.DataValueField = "Name";
        chkrcbLevel.DataBind();
    }


    #region Ticket List Rad Grid 
    protected void RadGvTicketList_SortCommand(object source, Telerik.Web.UI.GridSortCommandEventArgs e)
    {


    }

    protected void RadGvTicketList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            #region Set the Grid Filters
            if (!IsPostBack && Request.QueryString["fil"] != null)
            {

                if (Session["RadGvTicketList_FilterExpression"] != null && Convert.ToString(Session["RadGvTicketList_FilterExpression"]) != "" && Session["RadGvTicketList_Filters"] != null)
                {
                    RadGvTicketList.MasterTableView.FilterExpression = Convert.ToString(Session["RadGvTicketList_FilterExpression"]);
                    var filtersGet = Session["RadGvTicketList_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGvTicketList.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
                else
                {
                    Session["RadGvTicketList_FilterExpression"] = null;
                    Session["RadGvTicketList_Filters"] = null;
                }
            }
            else
            {
                #region Save the Grid Filter
                String filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);
                if (filterExpression != "")
                {
                    Session["RadGvTicketList_FilterExpression"] = filterExpression;
                    List<RetainFilter> filters = new List<RetainFilter>();

                    foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
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

                    Session["RadGvTicketList_Filters"] = filters;
                }
                else
                {
                    Session["RadGvTicketList_FilterExpression"] = null;
                    Session["RadGvTicketList_Filters"] = null;
                }
                #endregion
            }
            #endregion
        }
        catch { }

        try
        {
            if (!IsGridPageIndexChanged)
            {
                RadGvTicketList.CurrentPageIndex = 0;
                Session["RadGvTicketListCurrentPageIndex"] = 0;
                ViewState["RadGvTicketListminimumRows"] = 0;
                ViewState["RadGvTicketListmaximumRows"] = RadGvTicketList.PageSize;
            }
            else
            {
                if (Session["RadGvTicketListCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGvTicketListCurrentPageIndex"].ToString()) != 0
                    && Request.QueryString["fil"] != null && Request.QueryString["fil"].ToString() == "1")
                {
                    RadGvTicketList.CurrentPageIndex = Convert.ToInt32(Session["RadGvTicketListCurrentPageIndex"].ToString());
                    ViewState["RadGvTicketListminimumRows"] = RadGvTicketList.CurrentPageIndex * RadGvTicketList.PageSize;
                    ViewState["RadGvTicketListmaximumRows"] = (RadGvTicketList.CurrentPageIndex + 1) * RadGvTicketList.PageSize;
                }
            }

            String filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);
            if (filterExpression != "")
            {
                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                {
                    if (column.UniqueName.ToLower() == "id")
                    {
                        String filterValues = column.CurrentFilterValue;
                        if (filterValues != "")
                        {
                            if (!string.IsNullOrWhiteSpace(txtfromDate.Text) && !string.IsNullOrWhiteSpace(txtToDate.Text))
                            {
                                ViewState["ToDate"] = txtToDate.Text;
                                ViewState["fromDate"] = txtfromDate.Text;
                                txtfromDate.Text = "";
                                txtToDate.Text = "";
                            }
                        }
                        else
                        {
                            if (ddlSearch.SelectedValue.Equals("t.ID", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (!string.IsNullOrWhiteSpace(txtfromDate.Text) && !string.IsNullOrWhiteSpace(txtToDate.Text))
                                {
                                    ViewState["ToDate"] = txtToDate.Text;
                                    ViewState["fromDate"] = txtfromDate.Text;
                                    txtfromDate.Text = "";
                                    txtToDate.Text = "";
                                }
                            }
                            else
                            {
                                RestoreDateRange();
                            }
                        }
                    }
                }
            }

            string srt = string.Empty;
            if (RadGvTicketList.MasterTableView.SortExpressions.Count > 0)
            {

                if (RadGvTicketList.MasterTableView.SortExpressions[0].FieldName != string.Empty)
                {
                    srt = RadGvTicketList.MasterTableView.SortExpressions[0].FieldName + " ";

                    if (RadGvTicketList.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending)
                    {
                        srt += " DESC";
                    }
                    else { srt += " ASC"; }

                    Session["sortexp"] = srt;
                }

            }
            FillRadGvTicketList(string.Empty);
            RadGvTicketList.MasterTableView.NoMasterRecordsText = "No records found";

            if (hdnHideDates.Value.ToString() == "1" || hdnIsShowAll.Value.ToString() == "1")
            {
                txtfromDate.Text = "";
                txtToDate.Text = "";
            }
            else
            {
                RestoreDateRange();
            }

            UpdateSearchCriteria();
        }
        catch
        {
        }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        int rowsPerSheet = 0;
        if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["MaxRecordsPerSheet"]))
        {
            try
            {
                rowsPerSheet = Convert.ToInt32(WebConfigurationManager.AppSettings["MaxRecordsPerSheet"]);
            }
            catch (Exception)
            {
                rowsPerSheet = 0;
            }
        }

        if (rowsPerSheet < 500)
        {
            RadGvTicketList.Columns.FindByUniqueName("ReasonforService").Visible = true;
            RadGvTicketList.Columns.FindByUniqueName("WorkCompleteDesc").Visible = true;
            RadGvTicketList.Columns.FindByUniqueName("Who").Visible = true;
            RadGvTicketList.Columns.FindByUniqueName("CPhone").Visible = true;
            RadGvTicketList.Columns.FindByUniqueName("CDate").Visible = true;
            RadGvTicketList.Columns.FindByUniqueName("Custom3").Visible = true;
            RadGvTicketList.Columns.FindByUniqueName("Custom4").Visible = true;
            RadGvTicketList.Columns.FindByUniqueName("ProjectDescription").Visible = true;
            RadGvTicketList.ExportSettings.FileName = "Ticket";
            RadGvTicketList.ExportSettings.IgnorePaging = true;
            RadGvTicketList.ExportSettings.ExportOnlyData = true;
            RadGvTicketList.ExportSettings.OpenInNewWindow = true;
            RadGvTicketList.ExportSettings.HideStructureColumns = true;
            RadGvTicketList.MasterTableView.UseAllDataFields = true;
            RadGvTicketList.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
            RadGvTicketList.MasterTableView.ExportToExcel();
        }
        else
        {
            try
            {
                string strORDERBY = "EDATE ASC";

                if (Session["sortexp"] != null)
                {
                    strORDERBY = Session["sortexp"].ToString();
                }

                SetValues();
                if (Session["type"].ToString() != "am")
                {
                    DataTable dtuserinfo = (DataTable)Session["userinfo"];
                    if (dtuserinfo.Rows[0]["groupbywo"].ToString() == "1")
                        objMapData.OrderBy = "l.id,t.workorder";
                    else
                        objMapData.OrderBy = strORDERBY;
                }
                else
                {
                    objMapData.OrderBy = strORDERBY;
                }

                objMapData.EmpID = EmpId;
                objMapData.IsAssignedProject = isAssignProject;

                if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
                {
                    if (chkShowAll.Checked == true)
                    {
                        if (ddlReviewed.SelectedValue == string.Empty || ddlReviewed.SelectedValue == "1")
                        {
                            if (ddlStatus.SelectedValue == "-1" || ddlStatus.SelectedValue == "4")
                            {
                                DataSet dsOthers = new DataSet();

                                objMapData.Supervisor = string.Empty;
                                objMapData.NonSuper = Session["username"].ToString();
                                objMapData.FilterReview = "1";
                            }
                        }
                    }
                }

                List<RetainFilter> filters = new List<RetainFilter>();

                var inclCustomField = false;
                if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("accredited"))
                {
                    inclCustomField = true;
                }

                #region Save the Grid Filter
                String filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);
                if (filterExpression != "")
                {
                    foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                    {
                        String filterValues = column.CurrentFilterValue;
                        if (filterValues != "")
                        {
                            String columnName = column.UniqueName;
                            filters.Add(new RetainFilter() { FilterColumn = columnName, FilterValue = filterValues });
                        }
                    }

                    Session["TicketListRadGVFilters"] = filters;
                }

                #endregion
                var tempFile = string.Format("TicketList{0}.xlsx", DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
                var fileName = Server.MapPath(string.Format("ReportFiles/ExportExcel/{0}", tempFile));
                var strFileName = (new BL_Tickets()).ExportTicketListDataToExcel(objMapData, filters, txtfromDate.Text, txtToDate.Text, new GeneralFunctions().GetSalesAsigned(), strORDERBY, fileName, rowsPerSheet, inclCustomField);
                if (strFileName.Length > 0)
                {
                    try
                    {
                        DownloadDocument(strFileName, "Ticket.xlsx");
                        // Delete after downloaded
                        if (File.Exists(strFileName))
                            File.Delete(strFileName);
                    }
                    catch (Exception)
                    {
                        // Delete after downloaded
                        if (File.Exists(strFileName))
                            File.Delete(strFileName);
                    }
                }
                else
                {
                    string str = "Export failed!";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrExportExcel", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrExportExcel", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
            FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            try
            {
                long startBytes = 0;
                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, System.Text.Encoding.UTF8) + lastUpdateTiemStamp;

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.AddHeader("Accept-Ranges", "bytes");
                HttpContext.Current.Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                HttpContext.Current.Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                HttpContext.Current.Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;



                //Send data
                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //Dividing the data in 1024 bytes package
                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                int i;
                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                {
                    HttpContext.Current.Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    HttpContext.Current.Response.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _BinaryReader.Close();
                myFile.Close();
                HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                //HttpContext.Current.Response.End();
            }
        }
        catch (FileNotFoundException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }

    }

    protected void RadGvTicketList_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        try
        {
            int currentItem = 0;
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
                currentItem = 6;
            else
                currentItem = 7;
            if (e.Worksheet.Table.Rows.Count == RadGvTicketList.Items.Count + 1)
            {
                GridFooterItem footerItem = RadGvTicketList.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
        catch { }

    }

    protected void gvOpenCalls_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    #endregion

    #region RadGvRequestForService


    private void RadGvRequestForServiceFill()
    {
        try
        {
            if (Session["type"].ToString() != "c")
            {

                DataSet ds1 = new DataSet();
                objMapData.ConnConfig = Session["config"].ToString();
                ds1 = objBL_MapData.GetRequestForServiceCall(objMapData, new GeneralFunctions().GetSalesAsigned());
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    RadGvRequestForService.VirtualItemCount = ds1.Tables[0].Rows.Count;
                    RadGvRequestForService.DataSource = ds1;

                    divRequestForService.Visible = true;
                }
                else
                {
                    RadGvRequestForService.VirtualItemCount = ds1.Tables[0].Rows.Count;
                    RadGvRequestForService.DataSource = null;

                    divRequestForService.Visible = false;
                }
            }
            else
            {
                divRequestForService.Visible = false;
            }
        }
        catch { }

    }

    protected void RadGvRequestForService_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGvRequestForService.AllowCustomPaging = !ShouldApplySortFilterOrGroup(RadGvRequestForService);
            RadGvRequestForServiceFill();
        }
        catch { }


    }

    #endregion

    bool isGrouping = false;

    public bool ShouldApplySortFilterOrGroup(RadGrid RD)
    {
        return RD.MasterTableView.FilterExpression != "" ||
            (RD.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RD.MasterTableView.SortExpressions.Count > 0;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["IsShownAll"] = "0";
            //UpdateSearchCriteria();
            if (ddlSearch.SelectedValue.Equals("t.ID", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    if (txtToDate.Text != "" && txtfromDate.Text != "")
                    {
                        ViewState["ToDate"] = txtToDate.Text;
                        ViewState["fromDate"] = txtfromDate.Text;
                        txtToDate.Text = "";
                        txtfromDate.Text = "";
                    }
                }
                else
                {
                    if (txtToDate.Text == "" && txtfromDate.Text == "")
                    {
                        RestoreDateRange();
                    }
                }
            }
            else
            {
                ViewState["ToDate"] = txtToDate.Text;
                ViewState["fromDate"] = txtfromDate.Text;
            }

            //GetCallHistory(string.Empty);
            RadGvRequestForServiceFill();
            RadGvTicketList.Rebind();
        }
        catch { }
    }

    /// <summary>
    /// Fill Worker
    /// </summary>

    private void FillWorker()
    {
        try
        {

            ddlworker.Items.Clear();
            DataSet ds = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.Supervisor = ddlSuper.SelectedValue;
            objProp_User.Status = 1;
            ds = objBL_User.getEMP(objProp_User);

            ddlworker.Items.Add(new ListItem() { Text = "All", Value = "" });
            ListItem listitem1 = new ListItem("<======= ACTIVE WORKER ======= >", "Active");
            ddlworker.Items.Add(listitem1);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["Status"].ToString() == "0")
                {
                    ListItem listitem = new ListItem(dr["fDesc"].ToString(), dr["fDesc"].ToString());
                    ddlworker.Items.Add(listitem);
                }
            }
            ListItem listitem2 = new ListItem("<====== INACTIVE WORKER ====== >", "Inactive");
            ddlworker.Items.Add(listitem2);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["Status"].ToString() == "1")
                {
                    ListItem listitem3 = new ListItem(dr["fDesc"].ToString(), dr["fDesc"].ToString());
                    ddlworker.Items.Add(listitem3);
                }
            }
        }
        catch { }
    }

    /// <summary>
    /// Fill Supervisor
    /// </summary>
    /// 

    private void FillSupervisor()
    {
        try
        {
            DataSet ds = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();
            ds = objBL_User.getEMPSuper(objProp_User);
            ddlSuper.DataSource = ds.Tables[0];
            ddlSuper.DataTextField = "super";
            ddlSuper.DataValueField = "super";
            ddlSuper.DataBind();

            ddlSuper.Items.Insert(0, new ListItem("All", ""));

            if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
            {
                ddlSuper.SelectedValue = Session["username"].ToString().ToUpper();
                ddlSuper.Enabled = false;
                chkShowAll.Visible = true;
                lblShowAll.Visible = true;
                divShowAllSuper.Visible = true;
            }
        }
        catch { }
    }

    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getDepartment(objProp_User);

        ddlDepartment.DataSource = ds.Tables[0];
        ddlDepartment.DataTextField = "type";
        ddlDepartment.DataValueField = "id";
        ddlDepartment.DataBind();

        ddlDepartment.Items.Insert(0, new ListItem("All", "-1"));
    }

    private void SetValues()
    {
        string stdate = txtfromDate.Text + " 00:00:00";
        string enddate = txtToDate.Text + " 23:59:59";
        if (ddlDateRange.SelectedValue == "2")
        {
            objMapData.InvoiceDate = "Yes";
        }
        else
        {
            objMapData.InvoiceDate = null;
        }
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.Worker = ddlworker.SelectedValue;
        objMapData.FilterCharge = ddlCharge.SelectedValue;
        objMapData.FilterReview = ddlReviewed.SelectedValue;
        objMapData.Supervisor = ddlSuper.SelectedValue;
        objMapData.NonSuper = string.Empty;
        objMapData.Department = Convert.ToInt32(ddlDepartment.SelectedValue);
        objMapData.Category = GetSelectedCategory(); //ddlCat.SelectedValue;
        Session["TicketListViewCat"] = objMapData.Category;
        //objMapData.Route = hdnRoute.Value;
        objMapData.Route = ddlRoute.SelectedValue;
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            objMapData.Supervisor = Session["username"].ToString();
            if (Session["username"].ToString().ToUpper() != ddlSuper.SelectedValue.ToUpper() && ddlSuper.SelectedValue != string.Empty)
            {
                objMapData.NonSuper = Session["username"].ToString();
            }
        }

        objMapData.CustID = Convert.ToInt32(Session["custid"].ToString());
        if (Convert.ToInt32(ddllocation.SelectedValue) == -1)
            objMapData.LocID = 0;
        else
            objMapData.LocID = Convert.ToInt32(ddllocation.SelectedValue);
        objMapData.IsList = 1;
        objMapData.Timesheet = ddlTimeS.SelectedValue;
        objMapData.Bremarks = ddlRecommendation.SelectedValue;
        objMapData.IsPortal = ddlportal.SelectedValue;
        objMapData.IsPayroll = Convert.ToInt32(ddlPayroll.SelectedValue);
        if (txtfromDate.Text != string.Empty)
        {
            objMapData.StartDate = Convert.ToDateTime(stdate);
        }
        else
        {
            objMapData.StartDate = DateTime.MinValue;
        }

        if (txtToDate.Text != string.Empty)
        {
            objMapData.EndDate = Convert.ToDateTime(enddate);
        }
        else
        {
            objMapData.EndDate = DateTime.MinValue;
        }
        objMapData.Mobile = Convert.ToInt32(ddlMobile.SelectedValue);

        objMapData.Workorder = txtWo.Text;

        objMapData.SearchBy = ddlSearch.SelectedValue;

        if (ddlSearch.SelectedValue == "t.cat")
        {
            objMapData.SearchValue = ddlCategory.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "t.Level")
        {
            objMapData.SearchValue = GetSelectedLevels();
        }
        else if (ddlSearch.SelectedValue.Equals("t.ID", StringComparison.InvariantCultureIgnoreCase))
        {
            objMapData.SearchValue = txtSearch.Text.Trim().Trim(',').Replace("'", "''");
        }
        else
        {
            objMapData.SearchValue = txtSearch.Text.Replace("'", "''");
        }
        #region Company Check
        objMapData.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objMapData.EN = 1;
        }
        else
        {
            objMapData.EN = 0;
        }
        #endregion 
        objMapData.InvoiceID = Convert.ToInt16(ddlInvoiced.SelectedValue);

        objMapData.IsEmailSend = Convert.ToInt16(ddlEmail.SelectedValue);

        if (Session["type"].ToString() == "c")
        {
            DataTable dtuserinfo = (DataTable)Session["userinfo"];
            if (dtuserinfo.Rows[0]["openticket"].ToString() == "0")
            {
                objMapData.Status = 1;
                objMapData.FilterReview = "1";
                objMapData.Assigned = 4;
            }
            else
            {
                objMapData.Status = 0;
                objMapData.FilterReview = "";
                ddlReviewed.SelectedValue = "";
                objMapData.Assigned = Convert.ToInt16(ddlStatus.SelectedValue);
            }

            DataTable dtcust = new DataTable();
            dtcust = (DataTable)Session["userinfo"];
            int RoleID = 0;
            if (dtcust.Rows.Count > 0)
            {
                RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                objMapData.RoleID = RoleID;
            }


        }
        if (Convert.ToInt32(ddlStatus.SelectedValue) == 6)
        {
            objMapData.Voided = 1;
            objMapData.Assigned = 4;
            objMapData.Mobile = 1;
        }
        else
        {
            objMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);
            objMapData.Voided = 0;

        }

        Session["TicketListFilters"] = objMapData;
    }

    #region GetTicket

    private DataTable GetTickets(string OrderBy, int IsCallForPrintTicketReport = 0, bool GetReportData = false)
    {
        DataSet ds = new DataSet();
        try
        {
            if (OrderBy == string.Empty)
            {
                //Session["sortexp"] = null; 
            }
            if (Session["sortexp"] != null)
            {
                OrderBy = Session["sortexp"].ToString();
            }

            SetValues();
            if (Session["type"].ToString() != "am")
            {
                DataTable dtuserinfo = (DataTable)Session["userinfo"];
                if (dtuserinfo.Rows[0]["groupbywo"].ToString() == "1")
                    objMapData.OrderBy = "l.id,t.workorder";
                else
                    objMapData.OrderBy = OrderBy;
            }
            else
            {
                objMapData.OrderBy = OrderBy;
                //objMapData.OrderBy = "l.id,t.workorder";
            }

            objMapData.EmpID = EmpId;
            objMapData.IsAssignedProject = isAssignProject;
            //ds = GetRadGvTicketListData(objMapData, Convert.ToString(ViewState["fromDate"]), Convert.ToString(ViewState["ToDate"]),
            //        new GeneralFunctions().GetSalesAsigned(), IsCallForPrintTicketReport,
            //        GetReportData);
            ds = GetRadGvTicketListData(objMapData, txtfromDate.Text, txtToDate.Text,
                    new GeneralFunctions().GetSalesAsigned(), IsCallForPrintTicketReport,
                    GetReportData);

            #region filter for supervisor login
            if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
            {
                if (chkShowAll.Checked == true)
                {
                    if (ddlReviewed.SelectedValue == string.Empty || ddlReviewed.SelectedValue == "1")
                    {
                        if (ddlStatus.SelectedValue == "-1" || ddlStatus.SelectedValue == "4")
                        {
                            DataSet dsOthers = new DataSet();

                            objMapData.Supervisor = string.Empty;
                            objMapData.NonSuper = Session["username"].ToString();
                            objMapData.FilterReview = "1";

                            //dsOthers = GetRadGvTicketListData(objMapData, ViewState["fromDate"].ToString(), ViewState["ToDate"].ToString(),
                            //    new GeneralFunctions().GetSalesAsigned(), IsCallForPrintTicketReport);
                            dsOthers = GetRadGvTicketListData(objMapData, txtfromDate.Text, txtToDate.Text,
                                new GeneralFunctions().GetSalesAsigned(), IsCallForPrintTicketReport);

                            if (dsOthers != null)
                                ds.Tables[0].Merge(dsOthers.Tables[0], true, MissingSchemaAction.Ignore);
                        }
                    }

                    if (ddlStatus.SelectedValue == "-1" || ddlStatus.SelectedValue == "-2" || ddlStatus.SelectedValue == "0")
                    {
                        if (ddlReviewed.SelectedValue != "1")
                        {
                            DataSet dsUnassigned = new DataSet();

                            objMapData.Assigned = 0;
                            objMapData.Supervisor = string.Empty;
                            objMapData.NonSuper = string.Empty;
                            objMapData.FilterReview = "0";
                            //dsUnassigned = GetRadGvTicketListData(objMapData, ViewState["fromDate"].ToString(), ViewState["ToDate"].ToString(),
                            //    new GeneralFunctions().GetSalesAsigned(), IsCallForPrintTicketReport);
                            dsUnassigned = GetRadGvTicketListData(objMapData, txtfromDate.Text, txtToDate.Text,
                                new GeneralFunctions().GetSalesAsigned(), IsCallForPrintTicketReport);

                            if (dsUnassigned != null)
                                ds.Tables[0].Merge(dsUnassigned.Tables[0], true, MissingSchemaAction.Ignore);
                        }
                    }
                }
            }
            #endregion

            return ds.Tables[0];
        }
        catch
        {
            DataTable tbl = new DataTable();
            return tbl;
        }
    }

    #endregion



    private void FillRadGvTicketList(string OrderBy)
    {
        //UpdateSearchCriteria();
        DataTable dt = new DataTable();

        if (Session["type"].ToString() != "c")
        {
            if (ddlStatus.SelectedValue != "4" && ddlReviewed.SelectedValue == "1")
            {
                ddlStatus.SelectedValue = "4";
            }
            if (ddlReviewed.SelectedValue == "1" && ddlMobile.SelectedValue == "2")
            {
                ddlMobile.SelectedValue = "0";
            }
        }

        if (Session["type"].ToString() == "c")
        {
            if ((string.IsNullOrEmpty(txtfromDate.Text)) & intLocationCount > 1)
            {

                RadGvTicketList.MasterTableView.NoMasterRecordsText = "Select a date range and a location to view tickets.";

            }

            txtfromDate.Text = Convert.ToString(ViewState["fromDate"]);


            if ((string.IsNullOrEmpty(txtToDate.Text)) & intLocationCount > 1)
            {

                RadGvTicketList.MasterTableView.NoMasterRecordsText = "Select a date range and a location to view tickets.";

            }
            else
                txtToDate.Text = Convert.ToString(ViewState["ToDate"]);
            if (Convert.ToInt32(ddllocation.SelectedValue) == -1 & intLocationCount > 1)
            {

                RadGvTicketList.MasterTableView.NoMasterRecordsText = "Select a date range and a location to view tickets.";

            }
        }

        dt = GetTickets(OrderBy);

        FillCallHistory(dt);

        BINDRadGvTicketList(dt);

        if (CheckFilterDateWithGridDates(dt))
        {
            hdnHideDates.Value = "1";
        }
        else
        {
            hdnHideDates.Value = "0";
        }
        if (ddlDateRange.SelectedValue == "2")
        {
            hdnHideDates.Value = "0";
        }
    }

    private void BINDRadGvTicketList(DataTable dt)
    {
        try
        {
            Int32 VirtualItemCount = 0;
            if (dt.Rows.Count > 0)
            {
                VirtualItemCount = Convert.ToInt32(dt.Rows[0]["MAXROWNO"]);
            }
            //Session["TicketSrch"] = dt;

            RadGvTicketList.VirtualItemCount = VirtualItemCount;
            RadGvTicketList.DataSource = dt;


            //RadGvTicketList.Rebind();
            lblRecordCount.Text = VirtualItemCount.ToString() + " Record(s) Found.";

            ViewState["RadGvTicketListminimumRows"] = RadGvTicketList.CurrentPageIndex * RadGvTicketList.PageSize;
            ViewState["RadGvTicketListmaximumRows"] = (RadGvTicketList.CurrentPageIndex + 1) * RadGvTicketList.PageSize;
        }
        catch (Exception ex)
        {
        }

    }

    private bool CheckFilterDateWithGridDates(DataTable gridDT)
    {
        try
        {
            //if (txtfromDate.Text != string.Empty && txtToDate.Text != string.Empty
            //    && gridDT.DataSet != null)
            if (ViewState["ToDate"] != null && ViewState["fromDate"] != null
                && ViewState["ToDate"].ToString() != string.Empty && ViewState["fromDate"].ToString() != string.Empty
                && gridDT.DataSet != null)
            {
                string eDateOne = Convert.ToDateTime(ViewState["fromDate"].ToString()).ToString("MM/dd/yyyy");
                string eDateTwo = Convert.ToDateTime(ViewState["ToDate"].ToString()).ToString("MM/dd/yyyy");
                string myfilter = "EdateWithDateOnly < #" + eDateOne + "# OR EdateWithDateOnly > #" + eDateTwo + "#";
                DataRow[] rows = gridDT.Select(myfilter);
                if (rows.Count() > 0)
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
        catch
        {
            return false;
        }

    }

    private void FillCallHistory(DataTable dt)
    {
        try
        {

            AddTicketNavigationSession(dt);

            if (Session["type"].ToString() != "am")
            {
                DataTable dtuserinfo = (DataTable)Session["userinfo"];
                if (dtuserinfo.Rows[0]["groupbywo"].ToString() == "1")
                {
                    btnEdit.Visible = false;
                    lnkPDF.Visible = false;
                    FillGroups(dt);
                    gvGroupedTickets.DataSource = dt;
                    gvGroupedTickets.DataBind();
                    RadGvTicketList.Attributes.Add("style", "display:none");
                }

            }

            BINDRadGvTicketList(dt);

        }
        catch { }

    }

    private void AddTicketNavigationSession(DataTable dt)
    {

        DataTable dttableid = new DataTable();
        dttableid.Columns.Add("ID");
        dttableid.Columns.Add("comp");
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                DataRow drtableid = dttableid.NewRow();
                drtableid["ID"] = dr["ID"];
                drtableid["comp"] = dr["comp"];
                dttableid.Rows.Add(drtableid);
            }
            Session["ticketids"] = dttableid;
        }
    }

    private void FillGroups(DataTable dt)
    {
        Session["groupdataticket"] = dt;
        var locations = dt.AsEnumerable()
                        .Select(row => new
                        {
                            locname = row.Field<string>("locname"),
                            lid = row.Field<Int32>("lid"),
                            tottime = row.Field<object>("tottime")
                        }).GroupBy(k => new { k.lid, k.locname })
                        .Select(g => new
                        {
                            locname = g.Key.locname,
                            lid = g.Key.lid,
                            total = g.Sum(x => Math.Round(Convert.ToDecimal(x.tottime), 2))
                        }).OrderBy(c => c.locname).ToList();

        repGroupTicket.DataSource = locations;
        repGroupTicket.DataBind();
        var totaltime = locations.Sum(od => Convert.ToDouble(od.total));
        Label lbltotalsub = repGroupTicket.Controls[repGroupTicket.Controls.Count - 1].Controls[0].FindControl("lblGrtotal") as Label;
        lbltotalsub.Text = totaltime.ToString();
        Session["groupdataticket"] = null;

    }

    protected void OnItemDataBound1(object sender, RepeaterItemEventArgs e)
    {
        var totaltime = 0.0;
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataTable dt = (DataTable)Session["groupdataticket"];
            Label lblLID = e.Item.FindControl("lblLid") as Label;
            var repGroupTicketSub1 = e.Item.FindControl("repGroupTicketSub1") as Repeater;
            var Workorders = dt.AsEnumerable()
                         .Select(row => new
                         {
                             workorder = row.Field<string>("workorder"),
                             lid = row.Field<Int32>("lid"),
                             tottime = row.Field<object>("tottime")
                         })
                         .Where(l => l.lid == Convert.ToInt32(lblLID.Text)).GroupBy(k => new { k.workorder, k.lid })
                         .Select(g =>
                                 new
                                 {
                                     workorder = g.Key.workorder,
                                     lid = g.Key.lid,
                                     total = g.Sum(x => Math.Round(Convert.ToDecimal(x.tottime), 2))
                                 })
                         .OrderBy(c => c.workorder).ToList();

            repGroupTicketSub1.DataSource = Workorders;
            repGroupTicketSub1.DataBind();

            totaltime = Workorders.Sum(od => Convert.ToDouble(od.total));
            Label lbltotalsub = repGroupTicketSub1.Controls[repGroupTicketSub1.Controls.Count - 1].Controls[0].FindControl("lbltotal") as Label;
            lbltotalsub.Text = totaltime.ToString();
        }
    }

    protected void OnItemDataBound2(object sender, RepeaterItemEventArgs f)
    {
        var totaltime = 0.0;
        if (f.Item.ItemType == ListItemType.Item || f.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataTable dt = (DataTable)Session["groupdataticket"];
            Label lblWO = f.Item.FindControl("lblWO") as Label;
            var repGroupTicketSub2 = f.Item.FindControl("repGroupTicketSub2") as Repeater;
            var Tickets = dt.AsEnumerable()
                .Select(row => new
                {
                    workorder = row.Field<string>("workorder"),
                    locname = row.Field<string>("locname"),
                    lid = row.Field<Int32>("lid"),
                    edate = row.Field<DateTime>("edate"),
                    dwork = row.Field<string>("dwork"),
                    cat = row.Field<string>("cat"),
                    tottime = row.Field<object>("tottime"),
                    id = row.Field<object>("id"),
                    comp = row.Field<object>("comp"),
                    assignname = row.Field<object>("assignname")
                })
                .Where(w => w.workorder == lblWO.Text).ToList();

            repGroupTicketSub2.DataSource = Tickets;
            repGroupTicketSub2.DataBind();

            totaltime = Tickets.Sum(od => Convert.ToDouble(od.tottime));
            Label lbltotalsub = repGroupTicketSub2.Controls[repGroupTicketSub2.Controls.Count - 1].Controls[0].FindControl("lbltotalsub") as Label;
            lbltotalsub.Text = totaltime.ToString();
        }
    }

    protected void OnDataBound(object sender, EventArgs e)
    {
        for (int i = gvGroupedTickets.Rows.Count - 1; i > 0; i--)
        {
            GridViewRow row = gvGroupedTickets.Rows[i];
            GridViewRow previousRow = gvGroupedTickets.Rows[i - 1];

            Label lblloc = (Label)row.FindControl("lbllocid");
            Label lbllocPrev = (Label)previousRow.FindControl("lbllocid");
            if (lblloc.Text == lbllocPrev.Text)
            {
                if (previousRow.Cells[0].RowSpan == 0)
                {
                    if (row.Cells[0].RowSpan == 0)
                    {
                        previousRow.Cells[0].RowSpan += 2;
                    }
                    else
                    {
                        previousRow.Cells[0].RowSpan = row.Cells[0].RowSpan + 1;
                    }
                    row.Cells[0].Visible = false;
                }
            }

            Label lblWo = (Label)row.FindControl("lblWo");
            Label lblWoPrev = (Label)previousRow.FindControl("lblWo");
            if (lblWo.Text == lblWoPrev.Text)
            {
                if (previousRow.Cells[1].RowSpan == 0)
                {
                    if (row.Cells[1].RowSpan == 0)
                    {
                        previousRow.Cells[1].RowSpan += 2;
                    }
                    else
                    {
                        previousRow.Cells[1].RowSpan = row.Cells[1].RowSpan + 1;
                    }
                    row.Cells[1].Visible = false;
                }
            }
        }
    }

    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {
        this.ModalPopupExtender1.Hide();
        iframeCustomer.Attributes["src"] = "";
        // FillRadGvTicketList(string.Empty);
    }

    protected void ddlSuper_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillWorker();
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void lnkAddticket_Click(object sender, EventArgs e)
    {
        Panel2.Attributes.Add("style", "display:none");
        //iframeCustomer.Attributes["src"] = "addticket.aspx";
        //this.ModalPopupExtender1.Show();

        string url = "addticket.aspx";
        string s = "window.open('" + url + "', '_blank');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", s, true);
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGvTicketList.Items)
        {
            CheckBox chkSelect = (CheckBox)di.Cells[0].FindControl("chkSelect");
            Label lblTicketId = (Label)di.Cells[1].FindControl("lblTicketId");

            if (chkSelect.Checked == true)
            {
                try
                {
                    Int32 TicketId = 0;
                    if (Int32.TryParse(lblTicketId.Text, out TicketId))
                    {
                        DeleteTicket(TicketId);
                        // FillRadGvTicketList(string.Empty);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "keyDelete", "noty({text: 'Ticket # " + lblTicketId.Text + " Deleted Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); ", true);
                        RadGvTicketList.Rebind();
                    }
                }
                catch (Exception ex)
                {
                    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelete", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
        }
    }

    private void DeleteTicket(int TicketID)
    {
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = TicketID;
        objMapData.Worker = Session["username"].ToString();
        objBL_MapData.DeleteTicket(objMapData);
    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {

        ResetSearchField();

        if (ddlSearch.SelectedValue.Equals("t.ID", StringComparison.InvariantCultureIgnoreCase))
        {
            if (!string.IsNullOrWhiteSpace(txtToDate.Text) && !string.IsNullOrWhiteSpace(txtfromDate.Text))
            {
                ViewState["ToDate"] = txtToDate.Text;
                ViewState["fromDate"] = txtfromDate.Text;
                //txtToDate.Text = "";
                //txtfromDate.Text = "";
            }
        }
        else
        {
            //if(txtToDate.Text == "" && txtfromDate.Text == "")
            if (string.IsNullOrWhiteSpace(txtToDate.Text) && string.IsNullOrWhiteSpace(txtfromDate.Text))
            {
                //txtfromDate.Text = Convert.ToString(ViewState["fromDate"]);
                //txtToDate.Text = Convert.ToString(ViewState["ToDate"]);
                RestoreDateRange();
            }
        }

        if (ddlSearch.SelectedValue == "")
        {
            txtSearch.Text = string.Empty;
        }
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            //hdnIsShowAll.Value = "1";
            ViewState["IsShownAll"] = "1";
            foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            ResetFormControlValues(this);
            //chkcatlist.ClearSelection();
            //chkrcbLevel.ClearSelection();
            chkcatlist.ClearCheckedItems();
            chkrcbLevel.ClearCheckedItems();
            //ddlSearch_SelectedIndexChanged(sender, e);
            ResetSearchField();
            ddlSuper_SelectedIndexChanged(sender, e);
            if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
            {
                ddlSuper.SelectedValue = Session["username"].ToString().ToUpper();
                ddlSuper.Enabled = false;
            }
            txtfromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            //UpdateSearchCriteria();
            RadGvTicketList.MasterTableView.FilterExpression = string.Empty;
            GetPreferences();
            RadGvTicketList.Rebind();
            //hdnIsShowAll.Value = "0";
        }
        catch { }
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        try
        {

            ResetFormControlValues(this);
            chkcatlist.ClearCheckedItems();
            chkrcbLevel.ClearCheckedItems();
            //chkcatlist.ClearSelection();
            //chkrcbLevel.ClearSelection();
            ddlSuper_SelectedIndexChanged(sender, e);
            ResetSearchField();

            //if (txtfromDate.Text == string.Empty || txtToDate.Text == string.Empty)
            //{
            //    int diff = DateTime.Now.DayOfWeek - DayOfWeek.Monday;
            //    if (diff < 0)
            //    {
            //        diff += 7;
            //    }
            //    DateTime firstDay = DateTime.Now.AddDays(-1 * diff).Date;
            //    DateTime lastDay = firstDay.AddDays(6).Date;
            //    txtfromDate.Text = firstDay.ToShortDateString();
            //    txtToDate.Text = lastDay.ToShortDateString();
            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "setDefaultDateRangeCss", "SetDefaultDateRangeCss();", true);
            //}

            if (txtfromDate.Text == string.Empty && txtToDate.Text == string.Empty)
            {
                if (ViewState["IsShownAll"] != null && ViewState["IsShownAll"].ToString() == "1")
                {
                    int diff = DateTime.Now.DayOfWeek - DayOfWeek.Monday;
                    if (diff < 0)
                    {
                        diff += 7;
                    }
                    DateTime firstDay = DateTime.Now.AddDays(-1 * diff).Date;
                    DateTime lastDay = firstDay.AddDays(6).Date;
                    txtfromDate.Text = firstDay.ToShortDateString();
                    txtToDate.Text = lastDay.ToShortDateString();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "setDefaultDateRangeCss", "SetDefaultDateRangeCss();", true);
                }
                else
                {
                    RestoreDateRange();
                }
                ViewState["IsShownAll"] = "0";

            }

            if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
            {
                ddlSuper.SelectedValue = Session["username"].ToString().ToUpper();
                ddlSuper.Enabled = false;
            }

            foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }

            //UpdateSearchCriteria();

            RadGvTicketList.MasterTableView.FilterExpression = string.Empty;
            RadGvTicketList.Rebind();
        }
        catch { }
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
                        TextBox txtControl = (TextBox)c;
                        if (txtControl != null && !txtControl.ID.Equals("txtfromDate", StringComparison.InvariantCultureIgnoreCase)
                            && !txtControl.ID.Equals("txtToDate", StringComparison.InvariantCultureIgnoreCase))
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

    private bool IsControlInIgnoreList(Control c)
    {
        var ignoreListControls = new List<Control>
        {
            cbReview,
            ddlReviewDepartment
        };

        if (cbReview.Checked)
        {
            ignoreListControls.Add(ddlStatus);
        }

        return ignoreListControls.Contains(c);
    }

    /// <summary>
    /// Deletes temporary files created when attaching documents to a ticket.
    /// </summary>
    /// 

    private void DeleteTempDocumentFiles()
    {
        try
        {
            DataSet ds = new DataSet();

            objMapData.ConnConfig = Session["config"].ToString();
            ds = objBL_MapData.SelectTempDocumentFile(objMapData);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DeleteFile(dr["path"].ToString(), Convert.ToInt32(dr["id"]));
            }
        }
        catch { }
    }

    private void DeleteFile(string StrFilename, int DocumentID)
    {
        try
        {
            //File.Delete(StrFilename);            

            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objBL_MapData.DeleteFile(objMapData);
        }
        catch
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    public string ChargeableImage(string charge, string invoice, string manualinvoice, string statusName, string QBinvoiceid)
    {
        string img = "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
        if (charge == "1")
        {
            if (invoice != "0" && invoice.Trim() != "")
            {
                img = "images/dollar.png";
            }
            else
            {
                img = "images/dollarRed.png";
            }
        }
        if (charge == "0" && invoice != "0" && invoice.Trim() != "")
        {
            img = "images/dollar.png";
        }
        if (charge == "0" && manualinvoice.Trim() != "" && statusName == "Marked as Pending")
        {
            img = "images/DollarOrange.png";
        }
        else if (charge == "0" && manualinvoice.Trim() != "" && statusName != "Marked as Pending")
        {
            img = "images/dollar.png";
        }


        if (QBinvoiceid != "")
        {
            img = "images/dollarblue.png";
        }

        if (charge == "1" && manualinvoice.Trim() != "" && invoice == "0")
        {
            img = "images/dollar.png";
        }

        return img;
    }

    public string StatusIcon(string confirm, string comp, string review)
    {
        string image = "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

        if (review.Trim() == "1")
        {
            image = "images/review.png";
        }
        //else if (comp.Trim() == "2")
        //{
        //    image = "images/1331034893_pda.png";
        //}
        else if (confirm == "1")
        {
            image = "images/1331036429_Check.png";
        }

        return image;
    }

    public string ShowHoverText(object desc, object reason, object unit, object ticket, object email, object emailtime, object tottime, object OT, object Reg, object NT, object DT, object TT)
    {
        string result = string.Empty;

        result = "<span style='font-size:18px;text-decoration: underline'><B>Ticket#</B> " + ticket + "</B></span><br/>";

        if (unit.ToString() != string.Empty)
        {
            if (unit.ToString().Length > 20)
            {

                result += "<i>Equipment</i>: " + Convert.ToString(unit).Replace("\n", "<br/>").Substring(0, 20) + " ..." + "<br/>";
            }
            else
            {
                result += "<i>Equipment</i>: " + Convert.ToString(unit).Replace("\n", "<br/>") + "<br/>";
            }
        }

        if (!string.IsNullOrEmpty(reason.ToString()) && reason.ToString().Length > 80)
        {
            result += "<i>Reason</i>: " + Convert.ToString(reason).Replace("\n", "<br/>").Substring(0, 80) + " ...";
        }
        else
        {
            result += "<i>Reason</i>: " + Convert.ToString(reason).Replace("\n", "<br/>");
        }

        if (!string.IsNullOrEmpty(Convert.ToString(desc)) && desc.ToString().Length > 80)
        {
            result += "<br/><i>Resolution</i>: " + Convert.ToString(desc).Replace("\n", "<br/>").Substring(0, 80) + " ...";
        }
        else
        {
            result += "<br/><i>Resolution</i>: " + Convert.ToString(desc).Replace("\n", "<br/>");
        }


        if (email.ToString() == "1")
            result += "<br/><img src='images/email_notf.png'> <span>" + emailtime.ToString() + "</span>";

        result += "<br/><i> Reg </i>:" + Reg.ToString();
        result += ", <i> TT </i>:" + TT.ToString();
        result += ", <i> OT </i>:" + OT.ToString();
        result += ", <i> 1.7</i>:" + NT.ToString();
        result += ", <i> DT </i>:" + DT.ToString();
        result += ", <i> Total Time </i>:" + tottime.ToString();

        return result;
    }

    public string UnitCount(object units)
    {
        string result = string.Empty;
        if (units.ToString() != string.Empty)
            result = units.ToString().Split(',').Count().ToString();
        return result;
    }

    public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
    {
        DataSet ds = (DataSet)ViewState["ticketdetailsr"];
        DataSet dsC = (DataSet)ViewState["controldatarep"];
        int ticketid = Convert.ToInt32(ds.Tables[0].Rows[count1]["id"]);
        DataTable dtTicket = getSubReportdata(ds.Tables[0], ticketid);
        DataTable dtPOitem = getSubReportTicketdata(ds.Tables[1], ticketid);
        DataTable dtTicketI = getSubReportTicketdata(ds.Tables[2], ticketid);

        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = ticketid;
        DataSet dsEquip = objBL_MapData.getElevByTicket(objMapData);

        e.DataSources.Add(new ReportDataSource("dtEquipDetails", dsEquip.Tables[0]));
        e.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));
        e.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
        e.DataSources.Add(new ReportDataSource("Ticket_dtMCP", fillREPHistory(ticketid)));
        if (ds.Tables.Count > 1)
            e.DataSources.Add(new ReportDataSource("Ticket_dtPOItem", dtPOitem));
        if (ds.Tables.Count > 2)
            e.DataSources.Add(new ReportDataSource("Ticket_dtTicketI", dtTicketI));

        if (count1 == ds.Tables[0].Rows.Count - 1)
        {
            ViewState["ticketdetailsr"] = null;
            ViewState["controldatarep"] = null;
        }
        count1++;
    }

    private DataTable fillREPHistory(int ticketid)
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.EquipID = 0;
        objProp_User.SearchBy = "rd.ticketID";
        objProp_User.SearchValue = ticketid.ToString();

        DataSet ds = new DataSet();
        ds = objBL_User.getequipREPDetails(objProp_User);

        return ds.Tables[0];
    }

    private void GetControl()
    {
        ViewState["PR"] = "True";

        DataSet ds = new DataSet();

        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getControl(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {


            // if (ds.Tables[0].Rows[0]["PR"] != DBNull.Value)

            // { ViewState["PR"] = ds.Tables[0].Rows[0]["PR"].ToString() == "True" ? "True" : "False"; }

            if (ds.Tables[0].Rows[0]["businessstart"] != DBNull.Value)

                ViewState["bstart"] = Convert.ToDateTime(ds.Tables[0].Rows[0]["businessstart"]).ToShortTimeString();

            if (ds.Tables[0].Rows[0]["businessend"] != DBNull.Value)

                ViewState["bend"] = Convert.ToDateTime(ds.Tables[0].Rows[0]["businessend"]).ToShortTimeString();


        }
    }

    public string AfterHours(object enroute, object comp)
    {
        string image = "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

        DateTime StartDate = new DateTime();

        DateTime EndDate = new DateTime();

        DateTime date_enroute = new DateTime();

        DateTime date_comp = new DateTime();

        if (ViewState["bstart"] != null)
        {
            StartDate = Convert.ToDateTime(ViewState["bstart"]);
            StartDate = Convert.ToDateTime(StartDate.ToShortTimeString());
        }
        if (ViewState["bend"] != null)
        {
            EndDate = Convert.ToDateTime(ViewState["bend"]);
            EndDate = Convert.ToDateTime(EndDate.ToShortTimeString());
        }

        if (enroute != DBNull.Value)
        {
            date_enroute = Convert.ToDateTime(enroute);
            date_enroute = Convert.ToDateTime(date_enroute.ToShortTimeString());
        }
        if (comp != DBNull.Value)
        {
            date_comp = Convert.ToDateTime(comp);
            date_comp = Convert.ToDateTime(date_comp.ToShortTimeString());
        }

        if (date_enroute != DateTime.MinValue && StartDate != DateTime.MinValue)
        {
            if (date_enroute < StartDate)
                image = "images/hours.png";
        }
        if (date_enroute != DateTime.MinValue && EndDate != DateTime.MinValue)
        {
            if (date_enroute > EndDate)
                image = "images/hours.png";
        }
        if (date_comp != DateTime.MinValue && EndDate != DateTime.MinValue)
        {
            if (date_comp > EndDate)
                image = "images/hours.png";
        }
        if (date_comp != DateTime.MinValue && StartDate != DateTime.MinValue)
        {
            if (date_comp < StartDate)
                image = "images/hours.png";
        }

        return image;
    }

    public string Weekend(object scheduledate)
    {
        string image = "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

        DateTime StartDate = new DateTime();
        if (scheduledate != DBNull.Value)
        {
            StartDate = Convert.ToDateTime(scheduledate);
            if (StartDate.DayOfWeek == DayOfWeek.Saturday || StartDate.DayOfWeek == DayOfWeek.Sunday)
                image = "images/weekend.png";
        }

        return image;
    }

    protected void ddlCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListBox lst = sender as ListBox;
        int selecteditem = lst.SelectedIndex;
        if (selecteditem == 0)
        {
            //ddlCat.ClearSelection();
            //ddlCat.SelectedIndex = selecteditem;
        }

    }

    private string GetSelectedCategory()
    {

        string selectedvals = string.Empty;

        // ListItemCollection lstitems = ddlCat.Items;

        foreach (RadComboBoxItem item in chkcatlist.CheckedItems)
        {
            if (item.Checked == true)
            {
                selectedvals += "'" + item.Value + "',";
            }
        }
        return selectedvals.TrimEnd(',');
    }
    private string GetSelectedLevels()
    {
        string selectedlevel = string.Empty;
        foreach (RadComboBoxItem item in chkrcbLevel.CheckedItems)
        {
            if (item.Checked == true)
            {
                selectedlevel += "'" + item.Value + "',";
            }
        }
        return selectedlevel.TrimEnd(',');
    }

    private string GetSelectedLevelNames()
    {
        string selectedlevel = string.Empty;
        foreach (RadComboBoxItem item in chkrcbLevel.CheckedItems)
        {
            if (item.Checked == true)
            {
                selectedlevel += item.Text + ",";
            }
        }

        return selectedlevel.TrimEnd(',');
    }
    protected void chkcatlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSelectedCategory();
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
            objProp_User.Type = "TicketList";
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

    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportsName());
        string reports = "var reports=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", reports, true);
    }

    protected void txtfromDate_TextChanged(object sender, EventArgs e)
    {
        ViewState["fromDate"] = txtfromDate.Text;

    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        ViewState["ToDate"] = txtToDate.Text;
    }

    protected void chkHideTicketDesc_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            addPreferences();
            GetPreferences();
            //  FillRadGvTicketList(string.Empty);
            //RadGvTicketList.Rebind();
        }
        catch { }
    }

    public void GetPreferences()
    {

        DataSet ds = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objProp_User.PreferenceID = 1;
        objProp_User.PageID = 1;
        ds = objBL_User.getPreferences(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            string st = ds.Tables[0].Rows[0]["Preferencevalue"] == DBNull.Value ? "0" : ds.Tables[0].Rows[0]["Preferencevalue"].ToString();
            chkHideTicketDesc.Checked = st == "1" ? true : false;
        }
        else { chkHideTicketDesc.Checked = false; }

    }

    public void addPreferences()
    {
        try
        {
            int val = chkHideTicketDesc.Checked ? 1 : 0;
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objProp_User.PreferenceID = 1;
            objProp_User.PageID = 1;
            objProp_User.PreferenceValues = val;
            objBL_User.AddPreferences(objProp_User);
        }
        catch { }
    }

    protected void rdDay_CheckedChanged(object sender, EventArgs e)
    {
        if (((RadioButton)sender).Checked != rdDay.Checked)
        {
            txtfromDate.Text = DateTime.Now.ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
            ViewState["ToDate"] = txtToDate.Text;
            ViewState["fromDate"] = txtfromDate.Text;
            btnSearch_Click(sender, e);
        }
    }

    protected void rdWeek_CheckedChanged(object sender, EventArgs e)
    {
        if (((RadioButton)sender).Checked != rdWeek.Checked)
        {
            var now = System.DateTime.Now;

            var FisrtDay = now.AddDays(-((now.DayOfWeek - System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7)).Date;
            txtfromDate.Text = FisrtDay.ToShortDateString();
            var LastDay = FisrtDay.AddDays(7).AddSeconds(-1);

            txtToDate.Text = LastDay.ToShortDateString();
            ViewState["ToDate"] = txtToDate.Text;
            ViewState["fromDate"] = txtfromDate.Text;
            btnSearch_Click(sender, e);
        }
    }

    protected void rdMonth_CheckedChanged(object sender, EventArgs e)
    {
        if (((RadioButton)sender).Checked != rdMonth.Checked)
        {
            var Date = System.DateTime.Now;
            var firstDayOfMonth = new DateTime(Date.Year, Date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            txtfromDate.Text = firstDayOfMonth.ToShortDateString();
            txtToDate.Text = lastDayOfMonth.ToShortDateString();
            ViewState["ToDate"] = txtToDate.Text;
            ViewState["fromDate"] = txtfromDate.Text;
            btnSearch_Click(sender, e);
        }
    }

    protected void rdQuarter_CheckedChanged(object sender, EventArgs e)
    {
        if (((RadioButton)sender).Checked != rdQuarter.Checked)
        {
            var date = System.DateTime.Now;
            int quarterNumber = (date.Month - 1) / 3 + 1;
            DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
            DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
            txtfromDate.Text = firstDayOfQuarter.ToShortDateString();
            txtToDate.Text = lastDayOfQuarter.ToShortDateString();
            ViewState["ToDate"] = txtToDate.Text;
            ViewState["fromDate"] = txtfromDate.Text;
            btnSearch_Click(sender, e);
        }
    }

    protected void rdYear_CheckedChanged(object sender, EventArgs e)
    {
        if (((RadioButton)sender).Checked != rdYear.Checked)
        {
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            DateTime lastDay = new DateTime(year, 12, 31);
            txtfromDate.Text = firstDay.ToShortDateString();
            txtToDate.Text = lastDay.ToShortDateString();
            ViewState["ToDate"] = txtToDate.Text;
            ViewState["fromDate"] = txtfromDate.Text;
            btnSearch_Click(sender, e);
        }
    }


    public string LockColor(int assigned, string assignname)
    {



        string url = "images/unlock_white.png";

        if (assignname == "Voided")
        {
            url = "images/Ticket-Voided.png";
        }
        else
        {
            switch (assigned)
            {
                case 0:
                    url = "images/unlock_black.png";
                    break;
                case 1:
                    url = "images/unlock_white.png";
                    break;
                case 2:
                    url = "images/lock.png";
                    break;
                case 3:
                    url = "images/unlock.png";
                    break;
                case 4:
                    url = "images/unlock_blue.png";
                    break;
                case 5:
                    url = "images/unlock_yellow.png";
                    break;
            }
        }
        return url;
    }

    #region ########## Region #######
    /// <summary>
    /// Report
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void lnkMonthlyMaintenanceTicketReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("MonthlyMaintenanceTicketReport.aspx?StartDate=" + txtfromDate.Text + "&EndDate=" + txtToDate.Text);
    }

    protected void lnkMonthlyServiceCallBackReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("MonthlyServiceCallBackReport.aspx?StartDate=" + txtfromDate.Text + "&EndDate=" + txtToDate.Text);
    }

    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["TicketReportFormat"]) && WebConfigurationManager.AppSettings["TicketReportFormat"].ToLower().Contains("mrt"))
        {
            ReportMRTDownload();
        }
        else
        {
            ReportDownload();
        }
    }

    private void ReportDownload()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = GetTickets(string.Empty, 1, true);
            ReportViewer ReportViewer1 = new ReportViewer();
            Detailreport(dt, ReportViewer1);

            byte[] buffer = null;
            buffer = ExportReportToPDF("", ReportViewer1);

            Response.Clear();
            MemoryStream ms = new MemoryStream(buffer);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Transfer-Encoding", "identity");
            Response.AddHeader("content-disposition", "attachment;filename=Tickets.pdf");
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
        }
        catch { }
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

    private void Detailreport(DataTable dt, ReportViewer ReportViewer1)
    {
        count1 = 0;
        DataSet dsTicket = getSubReportdata(dt);
        ViewState["ticketdetailsr"] = dsTicket;
        if (dsTicket.Tables[0].Rows.Count > 0)
        {
            DataSet dsC = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objProp_User);
            ViewState["controldatarep"] = dsC;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dsTicket.Tables[0]));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
            string reportPath = "Reports/TicketDetails.rdlc";
            string Report = WebConfigurationManager.AppSettings["TicketDetailsReport"].Trim();
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
            ReportViewer1.LocalReport.ReportPath = reportPath;
            ReportViewer1.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", "~/companylogo.ashx"));

            var paras = ReportViewer1.LocalReport.GetParameters();
            var dsCustom1 = GetCustomFields("Loc1");
            if (dsCustom1.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dsCustom1.Tables[0].Rows[0]["label"].ToString()) && paras.FirstOrDefault(x => x.Name == "Custom1Lable") != null)
            {
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Custom1Lable", dsCustom1.Tables[0].Rows[0]["label"].ToString()));
            }

            var dsCustom2 = GetCustomFields("Loc2");
            if (dsCustom2.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dsCustom2.Tables[0].Rows[0]["label"].ToString()) && paras.FirstOrDefault(x => x.Name == "Custom2Lable") != null)
            {
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Custom2Lable", dsCustom2.Tables[0].Rows[0]["label"].ToString()));
            }

            ReportViewer1.LocalReport.SetParameters(param1);
            ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
            ReportViewer1.LocalReport.Refresh();
        }
    }

    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objGeneral.CustomName = name;
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getCustomFields(objGeneral);

        return ds;
    }

    private void ReportMRTDownload()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = GetTickets(string.Empty, 1, true);

            byte[] buffer = null;

            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(DetailReportMRT(dt), stream, settings);
            buffer = stream.ToArray();

            Response.Clear();
            MemoryStream ms = new MemoryStream(buffer);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Transfer-Encoding", "identity");
            Response.AddHeader("content-disposition", "attachment;filename=Tickets.pdf");
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
        }
        catch { }
    }

    private StiReport DetailReportMRT(DataTable dt)
    {
        DataSet dsTicket = getSubReportdata(dt);
        ViewState["ticketdetailsr"] = dsTicket;
        if (dsTicket.Tables[0].Rows.Count > 0)
        {
            var reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketDetailsReport.mrt");
            if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["TicketDetailsReport"]) && WebConfigurationManager.AppSettings["TicketDetailsReport"].ToLower().Contains(".mrt"))
            {
                reportPathStimul = Server.MapPath($"StimulsoftReports/Tickets/{WebConfigurationManager.AppSettings["TicketDetailsReport"]}");
            }

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            if (report.Dictionary.Variables.Contains("CustomLabel1"))
            {
                var custom1 = GetCustomFields("Loc1");
                if (custom1.Tables[0].Rows.Count > 0)
                {
                    report.Dictionary.Variables["CustomLabel1"].Value = custom1.Tables[0].Rows[0]["label"].ToString();
                }
            }

            if (report.Dictionary.Variables.Contains("CustomLabel2"))
            {
                var custom2 = GetCustomFields("Loc2");
                if (custom2.Tables[0].Rows.Count > 0)
                {
                    report.Dictionary.Variables["CustomLabel2"].Value = custom2.Tables[0].Rows[0]["label"].ToString();
                }
            }

            if (report.Dictionary.Variables.Contains("CustomLabel6"))
            {
                var custom6 = GetCustomFields("Ticket6");
                if (custom6.Tables[0].Rows.Count > 0)
                {
                    report.Dictionary.Variables["CustomLabel6"].Value = custom6.Tables[0].Rows[0]["label"].ToString();
                }
            }

            if (report.Dictionary.Variables.Contains("CustomLabel7"))
            {
                var custom7 = GetCustomFields("Ticket7");
                if (custom7.Tables[0].Rows.Count > 0)
                {
                    report.Dictionary.Variables["CustomLabel7"].Value = custom7.Tables[0].Rows[0]["label"].ToString();
                }
            }

            DataSet dsC = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objProp_User);

            var listTicketID = dt.Rows.OfType<DataRow>()
                .Select(dr => dr.Field<int>("ID")).ToList();

            objMapData.ConnConfig = Session["config"].ToString();
            DataSet dsEquips = objBL_MapData.GetElevByTicketIDs(objMapData, string.Join(",", listTicketID));
            DataSet dsItems = objBL_MapData.GetTicketItemByIDs(objMapData, string.Join(",", listTicketID));

            report.RegData("CompanyDetails", dsC.Tables[0]);
            report.RegData("ReportData", dt);
            report.RegData("dtEquipment", dsEquips.Tables[0]);

            if (dsItems != null)
            {
                report.RegData("dtPOItem", dsItems.Tables[0]);
                report.RegData("dtTicketItem", dsItems.Tables[1]);
            }

            report.CacheAllData = true;
            report.Render();

            return report;
        }

        return null;
    }

    private DataSet getSubReportdata(DataTable dt)
    {
        DataTable dtTicket = new DataTable();
        dtTicket.Columns.Add("TicketID", typeof(int));

        foreach (DataRow dr in dt.Rows)
        {
            DataRow drTicket = dtTicket.NewRow();
            drTicket["TicketID"] = dr["ID"];
            dtTicket.Rows.Add(drTicket);
        }

        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.dtTickets = dtTicket;
        DataSet ds = objBL_MapData.getTicketdetailsReport(objMapData);
        ds.Tables[0].Columns.Add("rtaddress");
        ds.Tables[0].Columns.Add("osaddress");
        ds.Tables[0].Columns.Add("ctaddress");
        //return ds.Tables[0];
        return ds;
    }

    private DataTable getSubReportdata(DataTable dt, int ticketid)
    {
        DataTable dtimport = dt.Clone();
        DataRow dr = dt.Rows[count1];
        dtimport.ImportRow(dr);
        return dtimport;
    }

    private DataTable getSubReportTicketdata(DataTable dt, int ticketid)
    {
        IEnumerable<DataRow> query =
         from order in dt.AsEnumerable()
         where order.Field<int>("Ticket") == ticketid
         select order;
        DataTable dtc = dt.Clone();
        if (query.Count() > 0)
            dtc = query.CopyToDataTable<DataRow>();
        return dtc;
    }
    #endregion

    protected void btnMassUpdate_Click(object sender, EventArgs e)
    {
        MassUpdateTicket();
    }

    private void MassUpdateTicket()
    {

        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TicketID", typeof(int));
            dt.Columns.Add("Clearcheck", typeof(int));
            dt.Columns.Add("transfertime", typeof(int));
            dt.Columns.Add("internet", typeof(int));
            dt.Columns.Add("ClearPR", typeof(int));



            foreach (GridDataItem gr in RadGvTicketList.MasterTableView.Items)
            {
                Label lblTicketId = (Label)gr.FindControl("lblTicketId");
                Label lblProjectId = (Label)gr.FindControl("lblProjectId");
                CheckBox chkReview = (CheckBox)gr.FindControl("chkReview");
                CheckBox chkTimesheet = (CheckBox)gr.FindControl("chkTimesheet");
                CheckBox chkPayroll = (CheckBox)gr.FindControl("chkPayroll");

                //Ref SECO-450 we should not create projects automatically for mass review

                if (chkReview.Checked == true || chkTimesheet.Checked == true)
                {
                    DataRow dr = dt.NewRow();

                    dr["TicketID"] = Convert.ToInt32(lblTicketId.Text);
                    dr["Clearcheck"] = Convert.ToInt32(chkReview.Checked);
                    dr["transfertime"] = Convert.ToInt32(chkTimesheet.Checked);
                    dr["internet"] = 0;
                    dr["ClearPR"] = Convert.ToInt32(chkPayroll.Checked); ;

                    if (hdnIsQBInt.Value == "1")
                    {
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        if (lblProjectId.Text != "0" && lblProjectId.Text != "")
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.QBPayrollID = ddlReviewPayroll.SelectedValue;
                objMapData.QBServiceID = ddlReviewService.SelectedValue;
                objMapData.PayRoll = Convert.ToInt32(ViewState["MassPayrollTicket"].ToString() == "Y" ? 1 : 0);
                objMapData.LastUpdatedBy = Session["username"].ToString();
                objMapData.dtReview = dt;
                int res = new BusinessLayer.Schedule.BL_Tickets().UpdateReviewStatus(objMapData);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucce", "noty({text: 'Tickets Updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true,dismissQueue: true, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                if (res == 1)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "321123", "noty({text: 'Please note not all tickets could be reviewed due to missing wage category.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }

            RadGvTicketList.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnVoided_Click(object sender, EventArgs e)
    {
        try
        {


            int Tickets = 0;

            int LocID = 0;

            string ConnConfig = Session["config"].ToString();

            string UpdatedBy = Session["username"].ToString();

            foreach (GridDataItem gr in RadGvTicketList.MasterTableView.Items)
            {
                Label lblTicketId = (Label)gr.FindControl("lblTicketId");
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                Label lblStatus = (Label)gr.FindControl("lblStatus");


                if (lblStatus.Text != "Voided" && lblStatus.Text != "Completed")
                {
                    Tickets = Convert.ToInt32(lblTicketId.Text);

                    new BusinessLayer.Schedule.BL_Tickets().VoidedTickets(ConnConfig, LocID, UpdatedBy, Tickets);
                }



            }

            if (Tickets != 0)
            {

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccevoided", "noty({text: 'Tickets Voided successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true,dismissQueue: true, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                RadGvTicketList.Rebind();

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }



    protected void cbReview_CheckedChanged(object sender, EventArgs e)
    {



        try
        {
            #region 


            reviewPnl.Visible = cbReview.Checked;
            DivMassReview.Visible = cbReview.Checked;

            ddlReviewPayrollDiv.Visible = false;
            ddlReviewServiceDiv.Visible = false;
            ddlReviewDepartmentDiv.Visible = false;

            #endregion

            if (cbReview.Checked)
            {
                #region PAY ROLL

                CheckBox cbpayroll = new CheckBox();

                cbpayroll.Checked = ViewState["MassPayrollTicket"].ToString() == "Y" ? true : false;


                CheckBox cbMassTimesheetCheck = new CheckBox();

                cbMassTimesheetCheck.Checked = ViewState["MassTimesheetCheck"].ToString() == "Y" ? true : false;


                CheckBox cbMassReviewCheck = new CheckBox();

                cbMassReviewCheck.Checked = ViewState["MassReviewCheck"].ToString() == "Y" ? true : false;

                ViewState["MassReviewCheck"] = "Y";

                ViewState["MassTimesheetCheck"] = "Y";

                ViewState["MassPayrollTicket"] = "Y";


                #endregion

                RadGvTicketList.AllowPaging = false;
                ViewState["ddlStatus"] = ddlStatus.SelectedValue;
                ViewState["ddlReviewed"] = ddlReviewed.SelectedValue;
                //Reviewed = No
                ddlReviewed.SelectedValue = "0";
                //Status = Completed
                ddlStatus.SelectedValue = "4";
                ddlStatus.Enabled = false;
                RadGvTicketList.MasterTableView.Columns.FindByUniqueName("PayrollHeader").Visible = cbpayroll.Checked;
                RadGvTicketList.MasterTableView.Columns.FindByUniqueName("ReviewCheck").Visible = cbMassReviewCheck.Checked;
                RadGvTicketList.MasterTableView.Columns.FindByUniqueName("ReviewCheckTimesheet").Visible = cbMassTimesheetCheck.Checked;
            }
            else
            {

                RadGvTicketList.AllowPaging = true;
                ddlStatus.SelectedValue = (string)ViewState["ddlStatus"];
                ddlReviewed.SelectedValue = (string)ViewState["ddlReviewed"];
                ddlStatus.Enabled = true;
                RadGvTicketList.MasterTableView.Columns.FindByUniqueName("ReviewCheck").Visible = false;
                RadGvTicketList.MasterTableView.Columns.FindByUniqueName("ReviewCheckTimesheet").Visible = false;
                RadGvTicketList.MasterTableView.Columns.FindByUniqueName("PayrollHeader").Visible = false;
            }

            RadGvTicketList.Rebind();
        }
        catch { }
    }



    protected void chkHeadReview_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkHeadReview = sender as CheckBox;

            foreach (GridDataItem gr in RadGvTicketList.MasterTableView.Items)
            {
                CheckBox chkReview = (CheckBox)gr.FindControl("chkReview");
                chkReview.Checked = chkHeadReview.Checked;
            }

        }
        catch { }
    }

    protected void chkHeadPayroll_CheckedChanged(object sender, EventArgs e)
    {

        try
        {
            CheckBox chkHeadPayroll = sender as CheckBox;


            if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c" && ViewState["MassPayrollTicket"].ToString() == "N")
            {
                chkHeadPayroll.Checked = false;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "325512124", "noty({text: 'You do not have permissions to Mass payroll',  type : 'warning', layout:'topCenter',closeOnSelfClick:true,dismissQueue: true, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                foreach (GridDataItem gr in RadGvTicketList.MasterTableView.Items)
                {

                    CheckBox chkPayroll = (CheckBox)gr.FindControl("chkPayroll");
                    CheckBox chkReview = (CheckBox)gr.FindControl("chkReview");
                    CheckBox chkTimesheet = (CheckBox)gr.FindControl("chkTimesheet");

                    if (chkHeadPayroll.Checked)
                    {
                        chkPayroll.Checked = true; chkReview.Checked = true; chkTimesheet.Checked = true;
                    }
                    else
                    {

                        chkPayroll.Checked = false;
                    }
                }
            }
        }

        catch { }

    }


    protected void chkHeadTimesheet_CheckedChanged(object sender, EventArgs e)
    {

        try
        {
            CheckBox chkHeadTimesheet = sender as CheckBox;


            if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c" && ViewState["MassTimesheetCheck"].ToString() == "N")
            {
                chkHeadTimesheet.Checked = false;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "32551124", "noty({text: 'You do not have permissions to Mass Review TimesSheet!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true,dismissQueue: true, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                foreach (GridDataItem gr in RadGvTicketList.MasterTableView.Items)
                {

                    CheckBox chkTimesheet = (CheckBox)gr.FindControl("chkTimesheet");

                    chkTimesheet.Checked = chkHeadTimesheet.Checked;
                }
            }

        }
        catch { }
    }

    #region Get Tickets 

    public DataSet GetRadGvTicketListData(MapData objPropMapData, string fromDate, string toDate,
        Int32 IsSalesAsigned = 0, int IsCallForTicketReport = 0,
        bool GetReportData = false)
    {
        try
        {
            var inclCustomField = false;
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("accredited"))
            {
                inclCustomField = true;
            }

            string strORDERBY = "EDATE ASC";

            if (Session["sortexp"] != null)
            {
                strORDERBY = Session["sortexp"].ToString();
            }

            List<RetainFilter> filters = new List<RetainFilter>();

            #region Save the Grid Filter
            String filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);
            if (filterExpression != "")
            {
                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        String columnName = column.UniqueName;
                        filters.Add(new RetainFilter() { FilterColumn = columnName, FilterValue = filterValues });
                    }
                }

                Session["TicketListRadGVFilters"] = filters;
            }

            #endregion

            if (IsCallForTicketReport == 0)
            {
                int RadGvTicketListminimumRows = 0;
                int RadGvTicketListmaximumRows = 50;

                int.TryParse(ViewState["RadGvTicketListminimumRows"].ToString(), out RadGvTicketListminimumRows);
                int.TryParse(ViewState["RadGvTicketListmaximumRows"].ToString(), out RadGvTicketListmaximumRows);

                if (cbReview.Checked)
                {
                    RadGvTicketListmaximumRows = 0;
                }

                return new BL_Tickets().GetTicketListData(objMapData, filters, fromDate, toDate, new GeneralFunctions().GetSalesAsigned(), strORDERBY, RadGvTicketListminimumRows, RadGvTicketListmaximumRows, inclCustomField);
            }
            else if (IsCallForTicketReport == 2)// Case email all
            {
                int RadGvTicketListminimumRows = 0;
                int RadGvTicketListmaximumRows = 10000;

                if (cbReview.Checked)
                {
                    RadGvTicketListmaximumRows = 0;
                }

                return new BL_Tickets().GetTicketListData(objMapData, filters, fromDate, toDate, new GeneralFunctions().GetSalesAsigned(), strORDERBY, RadGvTicketListminimumRows, RadGvTicketListmaximumRows, inclCustomField);
            }
            else
            {
                DataSet dsrpt = new BL_Tickets().GetTicketListReportData(objMapData, filters, fromDate, toDate, new GeneralFunctions().GetSalesAsigned(), IsCallForTicketReport, strORDERBY, GetReportData);
                Session["TicketListQuery"] = objMapData.fBy.ToString();
                if (objMapData.Category != string.Empty && objMapData.Category != null)
                {
                    Session["ddlCategorylist"] = objMapData.Category.ToString();
                }
                if (objMapData.SearchBy != string.Empty && objMapData.SearchBy == "t.cat")
                {
                    Session["ddlCategorylist"] = objMapData.SearchValue.ToString();
                }

                return dsrpt;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrGetTicket1", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            DataSet ds = new DataSet();
            return ds;
        }
    }

    #endregion Get Tickets

    protected void RadGvTicketList_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGvTicketListCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGvTicketListminimumRows"] = e.NewPageIndex * RadGvTicketList.PageSize;
            ViewState["RadGvTicketListmaximumRows"] = (e.NewPageIndex + 1) * RadGvTicketList.PageSize;
        }
        catch { }

    }

    protected void RadGvTicketList_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGvTicketListminimumRows"] = RadGvTicketList.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGvTicketListmaximumRows"] = (RadGvTicketList.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }

    }

    protected void RadGvTicketList_ItemCreated(object sender, GridItemEventArgs e)
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

                // dropDown.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;


            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void RadGvTicketList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == RadGrid.FilterCommandName)
            {
                //PagedListResult<List<TaskSearchBO>> taskList = ListTickets(pageIndex, pageCount, sortExpression, sortDirection, filtrCriteria);
                //gvTask.DataSource = taskList.Result;
                // RadGvTicketList.PageSize = pageCount;
                //RadGvTicketList.VirtualItemCount = taskList.TotalRowCount;
                ViewState["IsShownAll"] = "0";
                var ticketIdColumn = ((RadGrid)sender).MasterTableView.GetColumn("ID");
                if (string.IsNullOrWhiteSpace(ticketIdColumn.CurrentFilterValue))
                {
                    //txtfromDate.Text = Convert.ToString(ViewState["fromDate"]);
                    //txtToDate.Text = Convert.ToString(ViewState["ToDate"]);
                    RestoreDateRange();
                }
            }

        }
        catch { }
    }

    #region  :: Report ::

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        try
        {
            int IsCallForPrintTicketReport = 1;
            DataTable dt = new DataTable();
            if (ddlStatus.SelectedValue != "4" && ddlStatus.SelectedValue != "-1" && ddlReviewed.SelectedValue == "1")
            {
                dt = null;
            }
            else if (ddlStatus.SelectedValue == "4" && ddlReviewed.SelectedValue == "1" && ddlMobile.SelectedValue == "2")
            {
                dt = null;
            }
            else
            {
                dt = GetTickets(string.Empty, IsCallForPrintTicketReport);
            }

            string queryString = string.Empty;

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TicketReportFormat"]) && ConfigurationManager.AppSettings["TicketReportFormat"].ToString().ToLower().Equals("mrt"))
            {
                queryString = "window.open('PrintTicketReport.aspx?uid=" + Convert.ToInt32(Request.QueryString["uid"]) + "&s=" + ddlStatus.SelectedValue + "&sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&sn=" + ddlStatus.SelectedItem.Text + "&Sup=" + ddlSuper.SelectedItem.Text + "&Wor=" + ddlworker.SelectedItem.Text + "&chr=" + ddlCharge.SelectedItem.Text + "&rev=" + ddlReviewed.SelectedItem.Text + "&ddlSearchReportID=" + ddlSearchReportID + "', 'PrintTicketReport', 'height=768,width=1280,scrollbars=yes');";
            }
            else
            {
                queryString = "window.open('Printlist.aspx?uid=" + Convert.ToInt32(Request.QueryString["uid"]) + "&s=" + ddlStatus.SelectedValue + "&sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&sn=" + ddlStatus.SelectedItem.Text + "&Sup=" + ddlSuper.SelectedItem.Text + "&Wor=" + ddlworker.SelectedItem.Text + "&chr=" + ddlCharge.SelectedItem.Text + "&rev=" + ddlReviewed.SelectedItem.Text + "&ddlSearchReportID=" + ddlSearchReportID + "', 'Printlist', 'height=768,width=1280,scrollbars=yes');";
            }

            ClientScript.RegisterStartupScript(this.GetType(), "pop", queryString, true);
        }
        catch { }
    }

    protected void lnkTicketReport_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "0";
        lnkPrint_Click(sender, e);
    }

    protected void lnkInvoiceno_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "0";
        lnkinvoice_Click(sender, e);
    }

    protected void lnkinvoice_Click(object sender, EventArgs e)
    {
        try
        {
            int IsCallForPrintTicketReport = 1;
            DataTable dt = new DataTable();
            if (ddlStatus.SelectedValue != "4" && ddlStatus.SelectedValue != "-1" && ddlReviewed.SelectedValue == "1")
            {
                dt = null;
            }
            string queryString = string.Empty;

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TicketReportFormat"]) && ConfigurationManager.AppSettings["TicketReportFormat"].ToString().ToLower().Equals("mrt"))
            {
                queryString = "window.open('PrintTicketReport.aspx?uid=" + Convert.ToInt32(Request.QueryString["uid"]) + "&s=" + ddlStatus.SelectedValue + "&sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&sn=" + ddlStatus.SelectedItem.Text + "&Sup=" + ddlSuper.SelectedItem.Text + "&Wor=" + ddlworker.SelectedItem.Text + "&chr=" + ddlCharge.SelectedItem.Text + "&rev=" + ddlReviewed.SelectedItem.Text + "&ddlSearchReportID=" + ddlSearchReportID + "', 'PrintTicketReport', 'height=768,width=1280,scrollbars=yes');";
            }
            else
            {
                queryString = "window.open('Printlist.aspx?uid=" + Convert.ToInt32(Request.QueryString["uid"]) + "&s=" + ddlStatus.SelectedValue + "&sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&sn=" + ddlStatus.SelectedItem.Text + "&Sup=" + ddlSuper.SelectedItem.Text + "&Wor=" + ddlworker.SelectedItem.Text + "&chr=" + ddlCharge.SelectedItem.Text + "&rev=" + ddlReviewed.SelectedItem.Text + "&ddlSearchReportID=" + ddlSearchReportID + "', 'Printlist', 'height=768,width=1280,scrollbars=yes');";
            }

            ClientScript.RegisterStartupScript(this.GetType(), "pop", queryString, true);
        }
        catch { }
    }

    protected void lnkExpenseReport_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "1";
        lnkPrint_Click(sender, e);
    }

    protected void lnkTimeSheetReport_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "2";
        lnkPrint_Click(sender, e);
    }

    protected void lnkTimeSheetCertifiedProject_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "16";
        lnkPrint_Click(sender, e);
    }

    protected void lnkModTimeSheetReport_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "17";
        lnkPrint_Click(sender, e);
    }

    protected void lnkCallbackReport_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "3";
        lnkPrint_Click(sender, e);
    }

    protected void lnkDetailsReport_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "4";
        lnkPrint_Click(sender, e);
    }

    protected void lnkTicketReportSignature_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "5";
        lnkPrint_Click(sender, e);
    }

    protected void lnkTicketReportbyWO_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "6";
        lnkPrint_Click(sender, e);
    }

    protected void lnkWorkerReport_Click(object sender, EventArgs e)
    {

        ddlSearchReportID = "7";
        lnkPrint_Click(sender, e);
    }

    protected void lnkTicketListReport_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "8";
        lnkPrint_Click(sender, e);

    }

    protected void lnkInstallationSchedule_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "9";
        lnkPrint_Click(sender, e);
    }

    protected void lnkServiceSchedule_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "11";
        lnkPrint_Click(sender, e);
    }

    protected void lnkLaborbyDepartment_Click(object sender, EventArgs e)
    {

        ddlSearchReportID = "12";
        lnkPrint_Click(sender, e);
    }

    protected void knkTimesheetbyDepartment_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "13";
        lnkPrint_Click(sender, e);
    }

    protected void lnkTimeSheetReportNoTT_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "14";
        lnkPrint_Click(sender, e);
    }

    protected void lnkTimesheetbyWageCategory_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "15";
        lnkPrint_Click(sender, e);
    }

    protected void lnkCompletedTicketReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchText = string.Empty;
            if (ddlSearch.SelectedValue == "t.cat")
            {
                searchText = ddlCategory.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text;
            }

            List<RetainFilter> filters = new List<RetainFilter>();

            var filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);

            if (!string.IsNullOrEmpty(filterExpression))
            {
                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        filters.Add(new RetainFilter() { FilterColumn = column.UniqueName, FilterValue = filterValues });
                    }
                }
            }

            Session["TicketListRadGVFilters"] = filters;

            string urlString = "CompletedTicketReport.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText;

            // Redirect when close the report
            var redirect = HttpUtility.UrlEncode(Request.RawUrl);
            urlString += "&redirect=" + redirect;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCompletedTicketSignatureReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchText = string.Empty;
            if (ddlSearch.SelectedValue == "t.cat")
            {
                searchText = ddlCategory.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text;
            }

            List<RetainFilter> filters = new List<RetainFilter>();
            var filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);
            if (!string.IsNullOrEmpty(filterExpression))
            {
                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        RetainFilter filter = new RetainFilter();
                        filter.FilterColumn = column.UniqueName;
                        filter.FilterValue = filterValues;
                        filters.Add(filter);
                    }
                }
            }

            Session["TicketListRadGVFilters"] = filters;

            string urlString = "CompletedTicketReport.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText + "&signature=true";

            // Redirect when close the report
            var redirect = HttpUtility.UrlEncode(Request.RawUrl);
            urlString += "&redirect=" + redirect;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCompletedTicketEntrapment_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var levels = GetSelectedLevels().Replace("'", "");
            var levelNames = GetSelectedLevelNames();
            var searchText = string.Empty;
            if (ddlSearch.SelectedValue == "t.cat")
            {
                searchText = ddlCategory.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text;
            }

            List<RetainFilter> filters = new List<RetainFilter>();
            var filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);
            if (!string.IsNullOrEmpty(filterExpression))
            {
                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        RetainFilter filter = new RetainFilter();
                        filter.FilterColumn = column.UniqueName;
                        filter.FilterValue = filterValues;
                        filters.Add(filter);
                    }
                }
            }

            if (filters.Count > 0)
            {
                Session["TicketListFilter"] = filters;
            }

            string urlString = "CompletedTicketEntrapment.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText;

            if (!string.IsNullOrEmpty(levels))
            {
                urlString += "&lev=" + levels;
            }

            if (!string.IsNullOrEmpty(levelNames))
            {
                urlString += "&levNames=" + levelNames;
            }

            // Redirect when close the report
            var redirect = HttpUtility.UrlEncode(Request.RawUrl);
            urlString += "&redirect=" + redirect;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCallWarningReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchText = string.Empty;
            if (ddlSearch.SelectedValue == "t.cat")
            {
                searchText = ddlCategory.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text;
            }

            List<RetainFilter> filters = new List<RetainFilter>();
            var filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);
            if (!string.IsNullOrEmpty(filterExpression))
            {
                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        RetainFilter filter = new RetainFilter();
                        filter.FilterColumn = column.UniqueName;
                        filter.FilterValue = filterValues;
                        filters.Add(filter);
                    }
                }
            }

            Session["TicketListRadGVFilters"] = filters;

            string urlString = "CallWarningReport.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText;

            // Redirect when close the report
            var redirect = HttpUtility.UrlEncode(Request.RawUrl);
            urlString += "&redirect=" + redirect;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCallWarningSalespersonReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchText = string.Empty;
            if (ddlSearch.SelectedValue == "t.cat")
            {
                searchText = ddlCategory.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text;
            }

            List<RetainFilter> filters = new List<RetainFilter>();
            var filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);
            if (!string.IsNullOrEmpty(filterExpression))
            {
                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        RetainFilter filter = new RetainFilter();
                        filter.FilterColumn = column.UniqueName;
                        filter.FilterValue = filterValues;
                        filters.Add(filter);
                    }
                }
            }

            Session["TicketListRadGVFilters"] = filters;

            string urlString = "CallWarningBySalesperson.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText;

            // Redirect when close the report
            var redirect = HttpUtility.UrlEncode(Request.RawUrl);
            urlString += "&redirect=" + redirect;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkNewCallWarningReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchText = string.Empty;
            if (ddlSearch.SelectedValue == "t.cat")
            {
                searchText = ddlCategory.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text;
            }

            List<RetainFilter> filters = new List<RetainFilter>();
            var filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);
            if (!string.IsNullOrEmpty(filterExpression))
            {
                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        RetainFilter filter = new RetainFilter();
                        filter.FilterColumn = column.UniqueName;
                        filter.FilterValue = filterValues;
                        filters.Add(filter);
                    }
                }
            }

            Session["TicketListRadGVFilters"] = filters;

            string urlString = "NewCarCallWarningReport.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText;

            // Redirect when close the report
            var redirect = HttpUtility.UrlEncode(Request.RawUrl);
            urlString += "&redirect=" + redirect;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCategoryDueReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchText = string.Empty;
            if (ddlSearch.SelectedValue == "t.cat")
            {
                searchText = ddlCategory.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text;
            }

            List<RetainFilter> filters = new List<RetainFilter>();
            var filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);
            if (!string.IsNullOrEmpty(filterExpression))
            {
                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        RetainFilter filter = new RetainFilter();
                        filter.FilterColumn = column.UniqueName;
                        filter.FilterValue = filterValues;
                        filters.Add(filter);
                    }
                }
            }

            Session["RadGvTicketList_Filters"] = filters;

            string urlString = "TicketCategoryDueReport.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText;

            // Redirect when close the report
            var redirect = HttpUtility.UrlEncode(Request.RawUrl);
            urlString += "&redirect=" + redirect;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkTicketListPayroll_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchText = string.Empty;
            if (ddlSearch.SelectedValue == "t.cat")
            {
                searchText = ddlCategory.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text;
            }

            List<RetainFilter> filters = new List<RetainFilter>();

            var filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);

            if (!string.IsNullOrEmpty(filterExpression))
            {
                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        filters.Add(new RetainFilter() { FilterColumn = column.UniqueName, FilterValue = filterValues });
                    }
                }
            }

            Session["TicketListRadGVFilters"] = filters;

            string urlString = "TicketListPayrollHoursReport.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText;

            // Redirect when close the report
            var redirect = HttpUtility.UrlEncode(Request.RawUrl);
            urlString += "&redirect=" + redirect;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCallsPerRouteReport_Click(object sender, EventArgs e)
    {
        // Redirect when close the report
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        string urlString = "CallsPerRoute.aspx?redirect=" + redirect;

        Response.Redirect(urlString, true);
    }

    protected void lnkOpenTicketsByMechanicReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            Response.Redirect("OpenTicketsByMechanicReport.aspx?StartDate=" + txtfromDate.Text + "&EndDate=" + txtToDate.Text);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkOpenTicketDetailsByMechanicReport_Click(object sender, EventArgs e)
    {
        if (ddlworker.SelectedValue != string.Empty)
        {
            if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
            {
                Response.Redirect("OpenTicketDetailsByMechanic.aspx?StartDate=" + txtfromDate.Text + "&EndDate=" + txtToDate.Text + "&Worker=" + ddlworker.SelectedValue);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Please select worker before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkEquipmentHistoryPastXDays_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            Response.Redirect("Schedule_EquipmentHistoryPastXDays.aspx?StartDate=" + txtfromDate.Text + "&EndDate=" + txtToDate.Text);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkTimeRecap_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            Response.Redirect("Schedule_TimeRecapReport.aspx?StartDate=" + txtfromDate.Text + "&EndDate=" + txtToDate.Text + "&ddlDeprt=" + ddlDepartment.SelectedValue + "&Screen=ticketList");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkMonthlyRecurringHoursReport_Click(object sender, EventArgs e)
    {
        string urlString = "MonthlyRecurringHoursReport.aspx";

        // Redirect when close the report
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        urlString += "?redirect=" + redirect;

        Response.Redirect(urlString, true);
    }

    protected void lnkBuildingReportTemplate_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtfromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
        {
            var searchText = string.Empty;
            if (ddlSearch.SelectedValue == "t.cat")
            {
                searchText = ddlCategory.SelectedValue;
            }
            else
            {
                searchText = txtSearch.Text;
            }

            List<RetainFilter> filters = new List<RetainFilter>();

            var filterExpression = Convert.ToString(RadGvTicketList.MasterTableView.FilterExpression);

            if (!string.IsNullOrEmpty(filterExpression))
            {
                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;
                    if (filterValues != "")
                    {
                        filters.Add(new RetainFilter() { FilterColumn = column.UniqueName, FilterValue = filterValues });
                    }
                }
            }

            Session["TicketListRadGVFilters"] = filters;

            string urlString = "BuildingReportTemplate.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText;

            // Redirect when close the report
            var redirect = HttpUtility.UrlEncode(Request.RawUrl);
            urlString += "&redirect=" + redirect;

            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningDateRange", "noty({text: 'Set your date range before selecting this report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    protected void lnkMailAll_Click(object sender, EventArgs e)
    {
        try
        {
            User objPropUser = new User();

            DataTable dtNew = GetTickets(string.Empty, 2);
            if (dtNew != null && dtNew.Rows.Count > 0)
            {
                byte[] buffer = null;
                objPropUser.ConnConfig = Session["config"].ToString();
                int totalTicket = dtNew != null ? dtNew.Rows.Count : 0;

                var temp = new DataView(dtNew);
                temp.RowFilter = "assigned = 4";

                DataTable dt = temp.ToTable();

                int totalCompletedTicket = dt != null ? dt.Rows.Count : 0;
                int totalSentEmails = 0;
                int totalNotSend = totalTicket - totalCompletedTicket;
                int totalSendErr = 0;
                if (totalCompletedTicket > 0)
                {
                    try
                    {
                        List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
                        List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
                        Tuple<int, string, string> emailSendError = null;
                        Tuple<int, string, string> emailGetSentError = null;
                        StringBuilder sbdSentError = new StringBuilder();
                        StringBuilder sbdGetSentError = new StringBuilder();

                        EmailLog emailLog = new EmailLog();
                        emailLog.ConnConfig = Session["config"].ToString();
                        emailLog.Function = "Email All";
                        emailLog.Screen = "TicketList";
                        emailLog.Username = Session["Username"].ToString();
                        emailLog.SessionNo = Guid.NewGuid().ToString();

                        BL_General bL_General = new BL_General();
                        EmailTemplate emailTemplate = new EmailTemplate();
                        emailTemplate.ConnConfig = Session["config"].ToString();
                        emailTemplate.Screen = "TicketList";
                        emailTemplate.FunctionName = "Email All";
                        string mailContent = bL_General.GetEmailTemplate(emailTemplate);

                        foreach (DataRow _dr in dt.Rows)
                        {
                            int lid = Convert.ToInt32(_dr["lid"]);

                            objPropUser.RolId = lid;
                            DataSet _dsLoc = objBL_User.GetLocByID(objPropUser);
                            if (_dsLoc.Tables[0].Rows.Count > 0)
                            {
                                emailLog.Ref = (int)_dr["id"];

                                if (!string.IsNullOrEmpty(_dsLoc.Tables[0].Rows[0]["custom14"].ToString()))
                                {
                                    byte[] ticketsToPrint = null;

                                    if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["TicketDetailsReport"]) && WebConfigurationManager.AppSettings["TicketDetailsReport"].ToLower().Contains(".mrt"))
                                    {
                                        ticketsToPrint = PrintTicketForMRT(_dr);
                                    }
                                    else
                                    {
                                        ticketsToPrint = PrintTicketForRDLC(_dr);
                                    }

                                    if (ticketsToPrint != null)
                                    {
                                        buffer = ticketsToPrint;
                                    }

                                    var fromEmail = Convert.ToString(ViewState["EmailFrom"]);
                                    if (string.IsNullOrEmpty(fromEmail))
                                    {
                                        fromEmail = WebBaseUtility.GetFromEmailAddress();
                                    }

                                    string toEmail = string.Empty;
                                    string ccEmail = string.Empty;
                                    toEmail = _dsLoc.Tables[0].Rows[0]["custom14"].ToString();

                                    if (!string.IsNullOrEmpty(_dsLoc.Tables[0].Rows[0]["custom15"].ToString()))
                                    {
                                        ccEmail = _dsLoc.Tables[0].Rows[0]["custom15"].ToString();
                                    }

                                    Mail mail = new Mail();
                                    mail.From = fromEmail;
                                    Boolean IsMailSend = false;

                                    foreach (var toaddress in toEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        IsMailSend = true;
                                        mail.To.Add(toaddress.Trim());
                                    }

                                    foreach (var ccaddress in ccEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        mail.Cc.Add(ccaddress.Trim());
                                    }

                                    mail.Title = "Ticket #" + _dr["id"].ToString() + " - " + _dsLoc.Tables[0].Rows[0]["Tag"].ToString();
                                    //var mailContent = ViewState["MailContent"].ToString().Replace(Environment.NewLine, "<BR/>");

                                    if (string.IsNullOrEmpty(mailContent))
                                    {
                                        mail.Text = ViewState["MailContent"].ToString().Replace(Environment.NewLine, "<BR/>");
                                    }
                                    else
                                    {
                                        mail.Text = mailContent.Replace("{TicketNo}", _dr["id"].ToString()).Replace("{LocationName}", _dsLoc.Tables[0].Rows[0]["Tag"].ToString());
                                    }
                                    var companySignature = ViewState["CompanyAddress"].ToString().Replace(Environment.NewLine, "<BR/>");
                                    //mail.Text = mailContent;
                                    mail.IsIncludeSignature = true;

                                    mail.attachmentBytes = buffer;
                                    mail.FileName = "TicketCompleted.pdf";
                                    mail.DeleteFilesAfterSend = true;
                                    mail.RequireAutentication = false;

                                    if (IsMailSend == true)
                                    {
                                        //if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams"))
                                        //{
                                        //    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                                        //    mail.SendOld();
                                        //}
                                        //else
                                        //{
                                        MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
                                        emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog, companySignature);
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
                                            }
                                        }
                                        //}
                                    }
                                }
                                else
                                {
                                    totalSendErr++;
                                    emailLog.To = string.Empty;
                                    emailLog.Status = 0;
                                    emailLog.UsrErrMessage = "Email address does not exist for this location.";
                                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
                                    bL_EmailLog.AddEmailLog(emailLog);
                                }
                            }
                        }

                        //if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams"))
                        //{
                        //    Session["MailTicketSend"] = "true";
                        //    Response.Redirect("TicketListView.aspx");
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
                                // reset ref 
                                emailLog.Ref = 0;
                                var take = 100;
                                try
                                {
                                    var strTakeSent = ConfigurationManager.AppSettings["TakeSentItemspertime"].ToString();
                                    take = Convert.ToInt32(strTakeSent);
                                }
                                catch (Exception)
                                {
                                }

                                if (totalSentEmails >= take)
                                {
                                    List<List<MimeKit.MimeMessage>> lstTenMessages = new List<List<MimeKit.MimeMessage>>();
                                    while (mimeSentMessages.Any())
                                    {
                                        lstTenMessages.Add(mimeSentMessages.Take(take).ToList());
                                        mimeSentMessages = mimeSentMessages.Skip(take).ToList();
                                    }

                                    int imapLoginTimes = 0;
                                    foreach (var lst in lstTenMessages)
                                    {
                                        imapLoginTimes++;
                                        if (imapLoginTimes >= 15)
                                        {
                                            imapLoginTimes = 0;
                                            Thread.Sleep(10000);
                                            mail = new Mail();
                                            WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                                        }
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
                                    + totalTicket + " tickets sent out successfully.";

                                if (totalSendErr > 0)
                                {
                                    successfullMess += "<br>Total " + totalSendErr + " failed of "
                                        + totalTicket + " tickets could not be sent.";
                                }

                                if (totalNotSend > 0)
                                {
                                    successfullMess += "<br>Total " + totalNotSend + " of "
                                        + totalTicket + " tickets have not been completed.";
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
                                        + totalTicket + " tickets could not be sent.";
                                }
                                if (totalNotSend > 0)
                                {
                                    str += "<br>Total " + totalNotSend + " of "
                                        + totalTicket + " tickets have not been completed.";
                                }

                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }

                        RadGrid_gvLogs.Rebind();
                        //}
                    }
                    catch (Exception exp)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarning", "noty({text: 'There are no completed tickets on your list.',  type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarning", "noty({text: 'There are no completed tickets on your list.',  type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private byte[] PrintTicketForMRT(DataRow _dr)
    {
        User objPropUser = new User();

        // Export to PDF
        StiWebViewer rvTicket = new StiWebViewer();

        try
        {
            DataSet ds = new DataSet();
            DataSet dsTicket = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
            }

            #region Get Company Address

            //string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            //address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            //address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            //address += "Tel: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            //address += dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;

            string address = WebBaseUtility.GetSignature();

            string mailContent = "";
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CustomerName"]) && ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("adams"))
            {
                mailContent = "Veuillez consulter le ticket ci-joint. " + Environment.NewLine +
                    "Please review the attached ticket." + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                mailContent = "Please review the attached ticket from: " + Environment.NewLine + Environment.NewLine;
            }

            ViewState["CompanyAddress"] = address;
            ViewState["MailContent"] = mailContent;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }
            #endregion

            string templateName = WebConfigurationManager.AppSettings["TicketDetailsReport"].Trim();
            if (string.IsNullOrEmpty(templateName))
            {
                templateName = "TicketDetailsReport.mrt";
            }

            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath(string.Format("StimulsoftReports/Tickets/{0}", templateName));
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            DataSet companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());
            report.RegData("CompanyDetails", companyInfo.Tables[0]);

            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.TicketID = Convert.ToInt32(_dr["ID"]);
            dsTicket = objBL_MapData.GetTicketByID(objMapData);
            var dtTicket = dsTicket.Tables[0];

            report.RegData("ReportData", dtTicket);

            report.Dictionary.Synchronize();
            report.Render();
            rvTicket.Report = report;
            byte[] buffer1 = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(rvTicket.Report, stream, settings);
            buffer1 = stream.ToArray();

            return buffer1;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr753", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private byte[] PrintTicketForRDLC(DataRow _dr)
    {
        User objPropUser = new User();
        ReportViewer rvTicket = new ReportViewer();

        // Export to PDF
        try
        {
            DataSet ds = new DataSet();
            DataSet dsTicket = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            count1 = 0;

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
            }

            #region Get Company Address

            //string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            //address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            //address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            //address += "Tel: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            //address += dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;

            string address = WebBaseUtility.GetSignature();

            string mailContent = "";
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CustomerName"]) && ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("adams"))
            {
                //address = "Cher client : " + Environment.NewLine + "Veuillez consulter le ticket ci-joint. " + Environment.NewLine +
                ////"Veuillez noter qu’il peut y avoir plusieurs factures contenues dans chaque pièce jointe. " + Environment.NewLine +
                //"Si vous avez besoin de clarifications, " + Environment.NewLine +
                //"n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +
                //"Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +
                //"Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +
                //"Please review the attached ticket." + Environment.NewLine +
                //"Please note there may be multiple ticket contained " + Environment.NewLine +
                //"in each attachment. Should you have any questions, " + Environment.NewLine +
                //"Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
                //"We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

                mailContent = "Veuillez consulter le ticket ci-joint. " + Environment.NewLine +
                    "Please review the attached ticket." + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                mailContent = "Please review the attached ticket from: " + Environment.NewLine + Environment.NewLine;
            }

            ViewState["CompanyAddress"] = address;
            ViewState["MailContent"] = mailContent;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }

            #endregion

            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.TicketID = Convert.ToInt32(_dr["ID"]);
            dsTicket = objBL_MapData.GetTicketByID(objMapData);
            var dtTicket = dsTicket.Tables[0];

            ViewState["ticketdetailsr"] = dsTicket;
            ViewState["controldatarep"] = dsC;

            rvTicket.LocalReport.DataSources.Clear();
            rvTicket.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dsTicket.Tables[0]));
            rvTicket.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
            string reportPath = "Reports/TicketDetails.rdlc";
            string Report = WebConfigurationManager.AppSettings["TicketDetailsReport"].Trim();
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
            rvTicket.LocalReport.ReportPath = reportPath;
            rvTicket.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", "~/companylogo.ashx"));

            var paras = rvTicket.LocalReport.GetParameters();
            var dsCustom1 = GetCustomFields("Loc1");
            if (dsCustom1.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dsCustom1.Tables[0].Rows[0]["label"].ToString()) && paras.FirstOrDefault(x => x.Name == "Custom1Lable") != null)
            {
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Custom1Lable", dsCustom1.Tables[0].Rows[0]["label"].ToString()));
            }

            var dsCustom2 = GetCustomFields("Loc2");
            if (dsCustom2.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dsCustom2.Tables[0].Rows[0]["label"].ToString()) && paras.FirstOrDefault(x => x.Name == "Custom2Lable") != null)
            {
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Custom2Lable", dsCustom2.Tables[0].Rows[0]["label"].ToString()));
            }

            rvTicket.LocalReport.SetParameters(param1);
            rvTicket.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
            rvTicket.LocalReport.Refresh();

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            byte[] buffer1 = rvTicket.LocalReport.Render(
                "PDF", null, out mimeType, out encoding, out filenameExtension,
                 out streamids, out warnings);

            return buffer1;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr753", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private void FillRoute()
    {
        User objPropUser = new User();
        Int32 LocID = 0;
        LocID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"].ToString());
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getRoute(objPropUser, 1, LocID, 0);//IsActive=1 :- Get Only Active Workers
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "Label";
        ddlRoute.DataValueField = "ID";
        ddlRoute.DataBind();

        if (ds.Tables[1].Rows.Count > 0)
        {

            if (ddlRoute.Items.Contains(new ListItem(ds.Tables[1].Rows[0][0].ToString())))
                ddlRoute.Items.FindByText(ds.Tables[1].Rows[0][0].ToString()).Selected = true;

        }

        ddlRoute.Items.Insert(0, new ListItem("All", ""));
        ddlRoute.Items.Insert(1, new ListItem("Unassigned", "0"));

    }

    private void SetDefaultWorker()
    {
        Customer objCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();

        var masterTableView = RadGvTicketList.MasterTableView;
        var column = masterTableView.GetColumn("Name");

        objCustomer.ConnConfig = Session["config"].ToString();
        string getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        if (!string.IsNullOrEmpty(getValue))
        {
            lblRoute.Text = getValue;
            column.HeaderText = getValue;
        }
        else
        {
            lblRoute.Text = "Default Worker";
            column.HeaderText = "Default Worker";
        }
    }

    protected void RadGrid_gvLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        DataSet dsLog = new DataSet();
        EmailLog emailLog = new EmailLog();
        emailLog.Screen = "TicketList";
        emailLog.ConnConfig = Session["config"].ToString();
        BL_EmailLog bL_EmailLog = new BL_EmailLog();
        dsLog = bL_EmailLog.GetEmailLogs(emailLog);
        if (dsLog.Tables[0].Rows.Count > 0)
        {
            var userinfo = (DataTable)Session["userinfo"];
            int usertypeid = 0;
            if (userinfo != null)
            {
                usertypeid = Convert.ToInt32(userinfo.Rows[0]["usertypeid"]);
            }

            if (usertypeid == 2)
            {
                RadGrid_gvLogs.DataSource = string.Empty;
            }
            else
            {
                RadGrid_gvLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
                RadGrid_gvLogs.DataSource = dsLog.Tables[0];
            }
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

    protected void lnkSaveGridSettings_Click(object sender, EventArgs e)
    {
        #region Grid user settings
        var columnSettings = GetGridColumnSettings();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "TicketListView.aspx";
        objProp_User.GridId = "RadGvTicketList";

        objBL_User.UpdateUserGridCustomSettings(objProp_User, columnSettings);
        #endregion
    }

    protected void lnkRestoreGridSettings_Click(object sender, EventArgs e)
    {
        #region Grid user settings
        var columnSettings = string.Empty;
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "TicketListView.aspx";
        objProp_User.GridId = "RadGvTicketList";

        var ds = objBL_User.DeleteUserGridCustomSettings(objProp_User);


        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            var colIndex = 0;

            foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
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
            RadGvTicketList.MasterTableView.Rebind();
        }
        else
        {
            //var arrColumnOrder = new string[3]{ "ReviewCheck", "Comp", "" };
            var colIndex = 0;
            foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                column.Display = true;
                //column.OrderIndex =
                //if(colIndex >= 3)
                //{
                //    column.HeaderStyle.Reset();
                //}
            }
            RadGvTicketList.MasterTableView.SortExpressions.Clear();
            RadGvTicketList.MasterTableView.GroupByExpressions.Clear();
            RadGvTicketList.EditIndexes.Clear();
            RadGvTicketList.Rebind();
        }
        #endregion
    }

    protected void RadGvTicketList_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSet ds = new DataSet();
            objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            objProp_User.PageName = "TicketListView.aspx";
            objProp_User.GridId = "RadGvTicketList";
            ds = objBL_User.GetGridUserSettings(objProp_User);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //string columnSettings = "[{Name: \"BType\", Display: true, Width: 300},{Name: \"MatItem\", Display: false, Width: 300}]";
                var columnSettings = ds.Tables[0].Rows[0][0].ToString();
                var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

                var colIndex = 0;

                foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
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

            try
            {
                foreach (GridDataItem gr in RadGvTicketList.Items)
                {
                    Image imgTimeS = (Image)gr.FindControl("imgTimeS");
                    Image imgPortal = (Image)gr.FindControl("imgPortal");
                    Image imgStaus = (Image)gr.FindControl("imgStaus");
                    Image imgDoc = (Image)gr.FindControl("imgDoc");
                    Image imgSign = (Image)gr.FindControl("imgSign");
                    Image imgCharge = (Image)gr.FindControl("imgCharge");
                    Image imgHigh = (Image)gr.FindControl("imgHigh");
                    Image imgRecommend = (Image)gr.FindControl("imgRecommend");
                    Image imgOverTime = (Image)gr.FindControl("imgOverTime");

                    Label lblSumOfTotalTime = (Label)gr.FindControl("lblSumOfTotalTime");
                    GridFooterItem footeritem = (GridFooterItem)RadGvTicketList.MasterTableView.GetItems(GridItemType.Footer)[0];
                    Label lbl = (Label)footeritem.FindControl("lblfTottime");
                    lbl.Text = lblSumOfTotalTime.Text;


                    imgPortal.Visible = imgPortal.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                    imgStaus.Visible = imgStaus.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                    imgDoc.Visible = imgDoc.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                    imgSign.Visible = imgSign.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                    imgCharge.Visible = imgCharge.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                    imgTimeS.Visible = imgTimeS.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                    imgHigh.Visible = imgHigh.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                    imgRecommend.Visible = imgRecommend.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;
                    imgOverTime.Visible = imgOverTime.ImageUrl == "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7" ? false : true;

                    if (imgCharge.ImageUrl == "images/dollar.png") { imgCharge.ToolTip = "Invoiced"; }
                    if (imgStaus.ImageUrl == "images/review.png") { imgStaus.ToolTip = "Reviewed"; }
                    //else if (imgStaus.ImageUrl == "images/1331034893_pda.png") { imgStaus.ToolTip = "MS"; }

                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    Label lblComp = (Label)gr.FindControl("lblComp");
                    Label lblTicketId = (Label)gr.FindControl("lblTicketId");
                    HyperLink lbllnk = (HyperLink)gr.FindControl("lbllnk");

                    if (Session["type"].ToString() == "c")
                    {
                        if (btnEdit.Visible == true)
                        {
                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TicketReportFormat"]) && ConfigurationManager.AppSettings["TicketReportFormat"].ToString().ToLower().Equals("mrt"))
                            {
                                lbllnk.Attributes["onclick"] = gr.Attributes["ondblclick"] = "window.open('TicketReport.aspx?id=" + lblTicketId.Text + "','_blank');";
                            }
                            else
                            {
                                lbllnk.Attributes["onclick"] = gr.Attributes["ondblclick"] = "window.open('Printticket.aspx?id=" + lblTicketId.Text + "&c=" + lblComp.Text + "&pop=1','_blank');";
                            }
                        }
                    }
                    else
                    {
                        if (btnEdit.Visible == true)
                        {
                            if (hdnEditeTicket.Value == "Y" || hdnViewTicket.Value == "Y")
                            {
                                gr.Attributes["ondblclick"] = "location.href='addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text + "&pop=1&fr=tlv'";
                                lbllnk.NavigateUrl = "addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text + "&pop=1&fr=tlv";
                            }
                            else
                            {
                                lbllnk.Attributes["onclick"] = gr.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
                            }
                        }
                    }

                    Label lblRes = (Label)gr.FindControl("lblRes");
                    if (Session["type"].ToString() == "c")
                    {
                        chkHideTicketDesc.Checked = false;
                    }
                    else
                    {

                        if (!chkHideTicketDesc.Checked)
                        {
                            gr.Attributes["onmouseover"] = "HoverMenutext('" + gr.ClientID + "','" + lblRes.ClientID + "',event);";
                            gr.Attributes["onmouseout"] = " $('#" + lblRes.ClientID + "').hide();";
                        }

                        lblRes.Visible = chkHideTicketDesc.Checked ? false : true;
                    }
                }
            }
            catch { }
        }
    }

    private void ResetSearchField()
    {
        if (ddlSearch.SelectedValue == "t.cat")
        {
            ddlCategory.Visible = true;
            txtSearch.Visible = false;
            chkrcbLevel.Visible = false;
        }
        else if (ddlSearch.SelectedValue == "t.Level")
        {
            ddlCategory.Visible = false;
            txtSearch.Visible = false;
            chkrcbLevel.Visible = true;
        }
        else
        {
            ddlCategory.Visible = false;
            txtSearch.Visible = true;
            chkrcbLevel.Visible = false;
        }

        if (ddlSearch.SelectedValue == "")
        {
            txtSearch.Text = string.Empty;
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = true;
        RadGvRequestForServiceFill();
        RadGvTicketList.Rebind();
    }

    private string GetGridColumnSettings()
    {
        var columnSettings = string.Empty;

        List<ColumnSettings> lstColSetts = new List<ColumnSettings>();
        foreach (GridColumn column in RadGvTicketList.MasterTableView.OwnerGrid.Columns)
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
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.PageName = "TicketListView.aspx";
        objProp_User.GridId = "RadGvTicketList";

        var ds = objBL_User.GetDefaultGridCustomSettings(objProp_User);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
        }

        return columnSettings;
    }

    private void UpdateSearchCriteria()
    {
        List<SearchCriteria> searchCriterias = new List<SearchCriteria>();
        // Get search controls in Advanced Search
        GetControlValues(RadAjaxPanelAdvancedSearch, searchCriterias);
        if (chkcatlist.CheckedItems.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in chkcatlist.CheckedItems)
                sb.Append(item.Value + ";");
            //sb.Append(item.Index + "-" + item.Value + ";");

            sb.Remove(sb.Length - 1, 1);
            searchCriterias.Add(new SearchCriteria() { Name = chkcatlist.ID, Value = sb.ToString(), Type = chkcatlist.GetType().ToString() });
        }

        if (chkrcbLevel.CheckedItems.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in chkrcbLevel.CheckedItems)
                sb.Append(item.Value + ";");
            //sb.Append(item.Index + "-" + item.Value + ";");

            sb.Remove(sb.Length - 1, 1);
            searchCriterias.Add(new SearchCriteria() { Name = chkrcbLevel.ID, Value = sb.ToString(), Type = chkrcbLevel.GetType().ToString() });
        }

        // Get search controls in Generic Search
        GetControlValues(RadAjaxPanelSearch, searchCriterias);
        searchCriterias.Add(new SearchCriteria() { Name = txtfromDate.ID, Value = txtfromDate.Text.ToString(), Type = txtfromDate.GetType().ToString() });
        searchCriterias.Add(new SearchCriteria() { Name = txtToDate.ID, Value = txtToDate.Text.ToString(), Type = txtToDate.GetType().ToString() });
        searchCriterias.Add(new SearchCriteria() { Name = ddlDateRange.ID, Value = ddlDateRange.SelectedValue.ToString(), Type = ddlDateRange.GetType().ToString() });
        if (rdDay.Checked)
            searchCriterias.Add(new SearchCriteria() { Name = rdDay.ID, Value = rdDay.Checked.ToString(), Type = rdDay.GetType().ToString() });
        else if (rdWeek.Checked)
            searchCriterias.Add(new SearchCriteria() { Name = rdWeek.ID, Value = rdWeek.Checked.ToString(), Type = rdWeek.GetType().ToString() });
        else if (rdMonth.Checked)
            searchCriterias.Add(new SearchCriteria() { Name = rdMonth.ID, Value = rdMonth.Checked.ToString(), Type = rdMonth.GetType().ToString() });
        else if (rdQuarter.Checked)
            searchCriterias.Add(new SearchCriteria() { Name = rdQuarter.ID, Value = rdQuarter.Checked.ToString(), Type = rdQuarter.GetType().ToString() });
        else if (rdYear.Checked)
            searchCriterias.Add(new SearchCriteria() { Name = rdYear.ID, Value = rdYear.Checked.ToString(), Type = rdYear.GetType().ToString() });
        Session["TLV_SearchCriteria"] = searchCriterias;
        //Session["TLV_SearchCriteria"] = Newtonsoft.Json.JsonConvert.SerializeObject(searchCriterias);
    }

    private void GetControlValues(Control parent, List<SearchCriteria> searchCriteria)
    {
        //var searchCriteria = new List<SearchCriteria>();
        if (searchCriteria == null)
        {
            searchCriteria = new List<SearchCriteria>();
        }
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                GetControlValues(c, searchCriteria);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        DropDownList ddlControl = (DropDownList)c;
                        if (ddlControl.SelectedIndex != 0)
                        {
                            var ddlVal = ddlControl.SelectedValue;
                            var ddlName = ddlControl.ID;
                            searchCriteria.Add(new SearchCriteria() { Name = ddlName, Value = ddlVal, Type = c.GetType().ToString() });
                        }
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        TextBox txtControl = (TextBox)c;
                        if (!string.IsNullOrEmpty(txtControl.Text))
                        {
                            searchCriteria.Add(new SearchCriteria() { Name = txtControl.ID, Value = txtControl.Text, Type = c.GetType().ToString() });
                        }
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        CheckBox chkControl = (CheckBox)c;
                        searchCriteria.Add(new SearchCriteria() { Name = chkControl.ID, Value = chkControl.Checked.ToString(), Type = c.GetType().ToString() });
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        RadioButton rdControl = (RadioButton)c;
                        searchCriteria.Add(new SearchCriteria() { Name = rdControl.ID, Value = rdControl.Checked.ToString(), Type = c.GetType().ToString() });
                        break;
                }
            }
        }
    }

    private void UpdateControlValues()
    {
        List<SearchCriteria> searchCriterias = (List<SearchCriteria>)Session["TLV_SearchCriteria"];
        if (searchCriterias != null && Request.QueryString["fil"] != null && Request.QueryString["fil"].ToString() == "1")
        {
            IsGridPageIndexChanged = true;
            foreach (var item in searchCriterias)
            {
                switch (item.Type)
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        DropDownList ddlControl = (DropDownList)FindControlRecursive(Page, item.Name);
                        if (ddlControl != null)
                        {
                            ddlControl.SelectedValue = item.Value;
                        }
                        DropDownList ddlDateRange = (DropDownList)FindControlRecursive(Page, item.Name);
                        if (ddlDateRange != null)
                        {
                            ddlDateRange.SelectedValue = item.Value;
                        }
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        TextBox txtControl = (TextBox)FindControlRecursive(Page, item.Name);
                        if (txtControl != null)
                        {
                            txtControl.Text = item.Value;
                            if (txtControl.ID == "txtfromDate")
                            {
                                ViewState["fromDate"] = txtControl.Text;

                            }
                            else if (txtControl.ID == "txtToDate")
                            {
                                ViewState["ToDate"] = txtControl.Text;
                            }
                        }
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        CheckBox chkControl = (CheckBox)FindControlRecursive(Page, item.Name);
                        if (chkControl != null)
                        {
                            chkControl.Checked = Convert.ToBoolean(item.Value);
                        }
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        RadioButton rdControl = (RadioButton)FindControlRecursive(Page, item.Name);
                        if (rdControl != null)
                        {
                            rdControl.Checked = Convert.ToBoolean(item.Value);
                        }
                        break;
                    case "Telerik.Web.UI.RadComboBox":
                        RadComboBox rcbControl = (RadComboBox)FindControlRecursive(Page, item.Name);
                        if (rcbControl != null)
                        {
                            var arr = item.Value.Split(';');
                            foreach (RadComboBoxItem cb in rcbControl.Items)
                            {
                                if (arr.Contains(cb.Value))
                                {
                                    cb.Checked = true;
                                }
                            }
                        }
                        break;
                }
            }
            // Update search panel
            if (ddlSearch.SelectedValue == "t.cat")
            {
                ddlCategory.Visible = true;
                txtSearch.Visible = false;
                chkrcbLevel.Visible = false;
            }
            else if (ddlSearch.SelectedValue == "t.Level")
            {
                ddlCategory.Visible = false;
                txtSearch.Visible = false;
                chkrcbLevel.Visible = true;
            }
            else
            {
                ddlCategory.Visible = false;
                txtSearch.Visible = true;
                chkrcbLevel.Visible = false;
            }

            string activeRange = string.Empty;
            if (rdDay.Checked)
                activeRange = "Day";
            else if (rdWeek.Checked)
                activeRange = "Week";
            else if (rdMonth.Checked)
                activeRange = "Month";
            else if (rdQuarter.Checked)
                activeRange = "Quarter";
            else if (rdYear.Checked)
                activeRange = "Year";

            if (ViewState["fromDate"] != null && ViewState["fromDate"].ToString() != ""
                && ViewState["ToDate"] != null && ViewState["ToDate"].ToString() != "")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "SetActiveDateRangeCss", "SetActiveDateRangeCss('" + activeRange + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "setDefaultDateRangeCss", "SetDefaultDateRangeCss();", true);
            }
            //
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "ClearActive", "CssClearLabel();", true);
            // release session after used
            Session["TLV_SearchCriteria"] = null;
        }
        else
        {
            // release session when refresh page
            Session["TLV_SearchCriteria"] = null;
        }
    }

    private Control FindControlRecursive(Control rootControl, string controlID)
    {
        if (rootControl.ID == controlID) return rootControl;

        foreach (Control controlToSearch in rootControl.Controls)
        {
            Control controlToReturn = FindControlRecursive(controlToSearch, controlID);
            if (controlToReturn != null) return controlToReturn;
        }
        return null;
    }

    private void RestoreDateRange()
    {
        int diff = DateTime.Now.DayOfWeek - DayOfWeek.Monday;
        if (diff < 0)
        {
            diff += 7;
        }

        DateTime firstDay = DateTime.Now.AddDays(-1 * diff).Date;
        DateTime lastDay = firstDay.AddDays(6).Date;

        try
        {
            if (ViewState["fromDate"] != null && ViewState["ToDate"] != null &&
                ViewState["fromDate"].ToString() != "" && ViewState["ToDate"].ToString() != "")
            {

                txtfromDate.Text = ViewState["fromDate"].ToString();
                txtToDate.Text = ViewState["ToDate"].ToString();

            }
            else
            {
                txtfromDate.Text = firstDay.ToShortDateString();
                txtToDate.Text = lastDay.ToShortDateString();

                ViewState["fromDate"] = txtfromDate.Text;
                ViewState["ToDate"] = txtToDate.Text;
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "setDefaultDateRangeCss", "SetDefaultDateRangeCss();", true);
            }
        }
        catch
        {
            txtfromDate.Text = firstDay.ToShortDateString();
            txtToDate.Text = lastDay.ToShortDateString();

            ViewState["fromDate"] = txtfromDate.Text;
            ViewState["ToDate"] = txtToDate.Text;

        }
    }

    //public class ColumnSettings
    //{
    //    public string Name { get; set; }
    //    public bool Display { get; set; }
    //    public int Width { get; set; }
    //    public int OrderIndex { get; set; }
    //}

    [Serializable]
    private class SearchCriteria
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }

    protected void RadGvTicketList_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        Boolean flag = false;
        DataTable dt = new DataTable();
        dt.Columns.Add("TicketID", typeof(int));
        dt.Columns.Add("ManualInvoice", typeof(string));
        //dt.Columns.Add("Value", typeof(int));
        //dt.Columns.Add("OldValue", typeof(int));
        try
        {
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                if ((command.Type == GridBatchEditingCommandType.Update))
                {
                    GridDataItem item = (GridDataItem)command.Item;
                    Label hdnid = (Label)item.FindControl("lblTicketId");


                    Hashtable newValues = command.NewValues;
                    Hashtable oldValues = command.OldValues;

                    if (!newValues.Equals(oldValues))
                    {
                        foreach (var key in newValues.Keys)
                        {
                            if (newValues[key] != null)
                            {
                                if (oldValues[key] != null)
                                {
                                    if (!newValues[key].ToString().Equals(oldValues[key].ToString()))
                                    {
                                        flag = true;
                                    }
                                }
                                else
                                {
                                    flag = true;
                                }

                            }

                            if (flag == true)
                            {
                                String oldData = "";
                                if (oldValues[key] != null)
                                {
                                    oldData = oldValues[key].ToString();
                                }
                                dt.Rows.Add(Convert.ToInt32(hdnid.Text), newValues[key].ToString());
                                flag = false;
                            }
                        }
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                BL_MapData obj = new BL_MapData();
                MapData data = new MapData();
                data.ConnConfig = Session["config"].ToString();
                data.dtTicketINV = dt;
                data.LastUpdatedBy = Session["userName"].ToString();
                obj.UpdateTicketManuaInvoice(data);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Ticket updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkTicketCategoryHistory_Click(object sender, EventArgs e)
    {
        ddlSearchReportID = "20";
        lnkPrint_Click(sender, e);
    }

    protected void lnkOpenTicketReportbyRoutes_Click(object sender, EventArgs e)
    {
        string urlString = "OpenTicketReportbyRoutes.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&RType=All";
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        urlString += "&redirect=" + redirect;
        Response.Redirect(urlString, true);
    }
}
