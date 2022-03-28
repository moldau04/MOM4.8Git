using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using Microsoft.Reporting.WebForms;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using BusinessLayer.Schedule;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Data.OleDb;
using System.Threading;
using Telerik.Web.UI.GridExcelBuilder;
using System.Collections;
using System.Globalization;

public partial class addProject : System.Web.UI.Page
{
    #region "Variables"

    bool WIPSaveSuccess = false;

    String PageSize = "10000";

    int count_inv = 0;

    bool IsGst = false;

    string reportPath = "";

    byte[] array = null;

    BL_Job objBL_Job = new BL_Job();

    JobT objJob = new JobT();
    PRDed _objPRDed = new PRDed();
    Wage _objWage = new Wage();
    BL_Wage objBL_Wage = new BL_Wage();

    protected DataTable dtBomType = new DataTable();

    protected DataTable dtBomItem = new DataTable();

    protected DataTable dtApplicationStatus = new DataTable();

    protected DataTable dtBillingCodeData = new DataTable();

    BL_Contracts objBL_Contracts = new BL_Contracts();

    Contracts objProp_Contracts = new Contracts();

    BL_Customer objBL_Customer = new BL_Customer();

    Customer objProp_Customer = new Customer();

    BL_MapData objBL_MapData = new BL_MapData();

    MapData objMapData = new MapData();

    GeneralFunctions objgn = new GeneralFunctions();

    BL_BankAccount objBL_Bank = new BL_BankAccount();

    BL_Report bL_Report = new BL_Report();

    State objState = new State();

    User objPropUser = new User();

    BL_User objBL_User = new BL_User();

    BL_General objBL_General = new BL_General();

    General objGeneral = new General();

    BusinessEntity.Planner objPlanner = new BusinessEntity.Planner();

    BL_Planner objBL_Planner = new BL_Planner();

    protected DataTable dtCustomField = new DataTable();

    //protected DataTable dtMat = new DataTable();
    //protected DataTable dtLab = new DataTable();

    Wage objWage = new Wage();
    BL_ReportsData objBL_Report = new BL_ReportsData();
    private const string ASCENDING = " ASC";

    private const string DESCENDING = " DESC";
    //todo
    private Boolean isCanada;
    //private static int intExpExcelFlag = 0;
    #endregion

    #region Events
    public bool IsCustomExist
    {
        get
        {
            if (ViewState["IsCustomExist"] == null)
                ViewState["IsCustomExist"] = false;
            return (bool)ViewState["IsCustomExist"];
        }
        set
        {
            ViewState["IsCustomExist"] = value;
        }
    }
    #region PAGELOAD

    protected void Page_Load(object sender, EventArgs e)
    {

        if (ddlDateRange.SelectedIndex == 0)
        {

            txtfromDate.Style.Add("display", "none");
            txtToDate.Style.Add("display", "none");
        }
        else
        {

            txtfromDate.Style.Add("display", "block");
            txtToDate.Style.Add("display", "block");
        }


        //try
        //{

        //gvBudget.RowDataBound += new GridViewRowEventHandler(gvBudget_RowDataBound);
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
            return;
        }
        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }

        WebBaseUtility.UpdatePageTitle(this, "Project", Request.QueryString["uid"], null);

        FillBomType();

        FillApplicationStatus();

        FillBillCodes();

        if (!IsPostBack)
        {
            // Reset project group
            Session["NewProjGroupList"] = null;

            if (Session["txtfrmDtValforEditjob"] != null && Session["txttoDtValforEditJob"] != null && Session["ddlDateRangeFieldforEditJob"] != null)
            {
                if (Session["ddlDateRangeFieldforEditJob"].ToString() == "2" || Session["ddlDateRangeFieldforEditJob"].ToString() == "5")
                {
                    txtfromDate.Text = Session["txtfrmDtValforEditjob"].ToString();
                    txtToDate.Text = Session["txttoDtValforEditJob"].ToString();
                    ddlDateRange.SelectedIndex = 1;

                    txtfromDate.Style.Add("display", "block");
                    txtToDate.Style.Add("display", "block");

                }

            }
            if (string.IsNullOrWhiteSpace(GetDefaultGridColumnSettingsFromDb()))
            {
                BL_User objBL_User = new BL_User();
                User objProp_User = new User();

                // Get initial grid settings
                var gridDefault = GetGridColumnSettings();
                // Save default settings to database
                objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
                objProp_User.UserID = 0;// UserId = 0 for default
                objProp_User.PageName = "AddProject.aspx";
                objProp_User.GridId = "gvWIPs";

                objBL_User.UpdateUserGridCustomSettings(objProp_User, gridDefault);
            }

            if (!System.IO.Directory.Exists(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice"))
                System.IO.Directory.CreateDirectory(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice");
            FillLocationType();
            SetDefaultWorker();
            FillRoute();
            fillJobCustom();
            fillJobAttributeGeneralCustom();
            Fillterritory();
            GetControl();
            FillProjectsTemplate();
            FillState();
            //FillStages();
            FillJobType();
            FillJobStatus();
            FillPosting();
            FillCategory();
            FillWorker();
            getDiagnosticCategory();
            FillTerms();
            txtProjCreationDate.Text = DateTime.Now.ToShortDateString();
            DisableControl();
            Initialize();

            FillContractType("", "", -1, -1);
            GetControlForPayroll();
            GetWageCategoryList();




            if (Request.QueryString["locid"] != null)
            {
                hdnLocID.Value = Request.QueryString["locid"].ToString();
                btnSelectLoc_Click(sender, e);

            }
            else if (!string.IsNullOrEmpty(Request.QueryString["cust"]) && !string.IsNullOrEmpty(Request.QueryString["custName"]))
            {
                hdnCustID.Value = Request.QueryString["cust"].ToString();
                txtCustomer.Text = Request.QueryString["custName"].ToString();
                btnSelectCustomer_Click(sender, e);
            }
            liLinkEstimate.Visible = false;
            if (Request.QueryString["uid"] != null)
            {
                divEstimate.Visible = true;
                liLogs.Style["display"] = "inline-block";
                tbLogs.Style["display"] = "block";
                dateragediv.Visible = true;
                lnkProjectVarianceReport.Visible = true;
                lnkProjectGLCrossReferenceReport.Visible = true;
                lnkProjectVendorByProject.Visible = true;
                lnkProjectVendorByVendor.Visible = true;
                liLinkEstimate.Visible = true;
                //Panel10.Style["display"] = "block";
                Panel10.Visible = true;
                hdnprojectID.Value = Request.QueryString["uid"];
                if (Request.QueryString["tab"] != null)
                {
                    if (Request.QueryString["tab"] == "budget")
                    {
                        //TabContainerHeader.ActiveTab = tbpnlFinance;
                        //TabContainerFinance.ActiveTab = tbpnlBudgets;
                        liaccrdFinance.Visible = true;
                        tbpnlBudgets.Visible = true;
                        tbpnlBudgets.Focus();
                    }
                }
                //pnlNext.Visible = true;
                lblHeader.Text = "Edit Project";
                GetData();

                //GetAttachment();
                FillTasks(hdnLocRolID.Value);

                if (!ddlTemplate.SelectedValue.Equals("Select Template"))
                {
                    //ref ES-2768 TEI -  Edit Project allow user to edit project template

                    //ddlTemplate.Enabled = false;
                }
                objJob.ConnConfig = Session["config"].ToString();
                objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);

                BindSummary();

                #region Check on Planner Tab Data
                PlannerCheck();
                #endregion


            }
            else
            {
                ddlJobStatus.SelectedValue = "0";
                divAP.Visible = false;
                divTickets.Visible = false;
                divInvoices.Visible = false;
                divJC.Visible = false;
                liaccrdDocument.Visible = false;
                liTeam.Visible = false;
                liaccrdContacts.Visible = false;
                liaccrdTicket.Visible = false;
                btnSave.Visible = false;
                btnWIPCancel.Visible = false;
                btnGenerateInvoice.Visible = false;
                liaccrdPlanner.Visible = false;
                liEquipment.Visible = false;
                liGC.Visible = false;
                liHomeowner.Visible = false;
                liBudgets.Visible = false;
                liBilling.Visible = false;
                tbWIP.Visible = false;
                liBilling_ProgressBillings.Visible = false;
                divProjectNoteButton.Visible = false;
                divEstimate.Visible = false;
                lifinancebudgethead.Visible = false;
                lifinancebillinghead.Visible = false;
                hdnOrgStageID.Value = "";
            }


            divJC.Visible = false;

            ddlJobType.Enabled = false;
            ddlType.Enabled = false;
            Permission();

            HighlightSideMenu("ProjectMgr", "lnkProject", "ProjectMgrSub");
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



        }
        if (Page.IsPostBack)                // comment 8.16.16
        {
            #region rebind custom fields
            if (IsCustomExist)                    // commented by Mayuri on 28th july, 16 incomplete custom field func.
            {
                bool IsTemplateDdl = false;
                //Control ctrl = null;

                string ctrlName = Page.Request.Params.Get("__EVENTTARGET");

                if (!String.IsNullOrEmpty(ctrlName))
                {
                    //ctrl = Page.FindControl(ctrlName);
                    if (ctrlName.Replace("ctl00$ContentPlaceHolder1$", "") == ddlTemplate.ID)       //if ddltemplate caused postback then do not update previous template's custom fields
                    {
                        IsTemplateDdl = true;
                    }
                    if (ctrlName.Replace("ctl00$ContentPlaceHolder1$", "") == lnkSaveTemplate.ID)
                    {
                        IsTemplateDdl = true;
                    }
                }
                if (!IsTemplateDdl)
                {
                    objJob.ConnConfig = Session["config"].ToString();
                    if (ddlTemplate.SelectedValue.ToString() != "Select Template")
                    {
                        objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);
                    }
                    if (Request.QueryString["uid"] != null)
                    {
                        Panel10.Visible = true;
                        //Panel10.Style["display"] = "block";
                        objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
                    }
                    else
                    {
                        objJob.Job = 0;
                    }
                    DataSet dsTemp = new DataSet();
                    dsTemp = objBL_Job.GetProjectTemplateCustomFields(objJob);
                    DataTable dtCustom = GetCustomItems();
                    if (dtCustom == null)
                        dtCustom = dsTemp.Tables[0];
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        CreateCustomTable();
                        DisplayCustomByTab(dtCustom, dsTemp.Tables[1], objJob.ID, objJob.Job);
                    }
                }


            }
            #endregion
            if (hdnEmailPopupOpened.Value == "1")
            {
                ModalPopupSendEmail.Show();
            }
        }
        CompanyPermission();
        if (Request.Form["__EVENTTARGET"] == "lnkbLoadDoc")
        {
            DoPostBack();
            //GetAttachment();
        }
        if (Request.QueryString["uid"] != null)
        {
            BindBudget(1);
        }
        if (ddlDateRange.SelectedIndex == 0)
        {
            txtfromDate.Text = txtToDate.Text = "";
        }

        GetPeriodDate();

        Locstatus();
    }
    #endregion

    

    #region Milestones
    protected void RadMenuMilGrid_ItemClick(object sender, RadMenuEventArgs e)
    {
        DataTable dt = GetMilestoneGridItems();
        switch (e.Item.Text)
        {
            case "Add Row Above":
                AddNewRowGrid(dt, "above", "Milestones");
                break;

            case "Add Row Below":
                AddNewRowGrid(dt, "below", "Milestones");
                break;
        }
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

    public void PlannerCheck()
    {
        objPlanner.ConnConfig = Session["config"].ToString();
        objPlanner.ProjectID = Convert.ToInt32(Request.QueryString["uid"]);
        DataSet dsPlanner = new DataSet();
        dsPlanner = objBL_Planner.GetPlannerByProjectID(objPlanner);
        if (dsPlanner.Tables[0] != null && dsPlanner.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToString(dsPlanner.Tables[0].Rows[0]["ID"]) != "" && Convert.ToString(dsPlanner.Tables[0].Rows[0]["ID"]) != "0")
            {
                lnkAddPlanner.Visible = false;
                //lnkPlannerID.Visible = true;
                lnkSavePlanner.Visible = true;
                lnkDeletePlanner.Visible = true;
                PlannerInfo.Visible = true;

                //TO DO -- Need to make this Information Dynamic.
                lnkPlannerID.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["ID"]);
                lnkPlannerID.NavigateUrl = "Planner.aspx?projid=" + Request.QueryString["uid"] + "&plnid=" + lnkPlannerID.Text;
                //ViewState["PlannerID"] = lnkPlannerID.Text;
                //lblPlannerDescription.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["Desc"]);
                txtPlannerTitle.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["Desc"]);
                if (Convert.ToString(dsPlanner.Tables[0].Rows[0]["StartDate"]) != "")
                {
                    lblStartDate.Text = Convert.ToDateTime(dsPlanner.Tables[0].Rows[0]["StartDate"]).ToShortDateString();
                }

                if (Convert.ToString(dsPlanner.Tables[0].Rows[0]["EndDate"]) != "")
                {
                    lblFinishDate.Text = Convert.ToDateTime(dsPlanner.Tables[0].Rows[0]["EndDate"]).ToShortDateString();
                }
                lblNumberofTasks.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["NoOfTask"]);
                lblNextDueTask.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["NextDueTask"]);
                lblInProgressTask.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["ProgressTask"]);
                lblTotalHours.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["TotalHours"]);
                lblTotalDays.Text = Convert.ToString(dsPlanner.Tables[0].Rows[0]["TotalDays"]);
            }
            else
            {
                //lnkPlannerID.Visible = false;
                PlannerInfo.Visible = false;
                lnkAddPlanner.Visible = true;
                lnkSavePlanner.Visible = false;
                lnkDeletePlanner.Visible = false;
                //ViewState["PlannerID"] = null;
            }

        }
        else
        {
            //lnkPlannerID.Visible = false;
            PlannerInfo.Visible = false;
            lnkAddPlanner.Visible = true;
            lnkSavePlanner.Visible = false;
            lnkDeletePlanner.Visible = false;
            //ViewState["PlannerID"] = null;
        }
    }

    public bool CheckAddEditPermission()
    {
        bool result = true;
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            /// job ///////////////////------->

            string jobPermission = ds.Rows[0]["job"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["job"].ToString();

            string stAddejob = jobPermission.Length < 1 ? "Y" : jobPermission.Substring(0, 1);
            string stEditejob = jobPermission.Length < 2 ? "Y" : jobPermission.Substring(1, 1);
            string stDeleteJob = jobPermission.Length < 3 ? "Y" : jobPermission.Substring(2, 1);
            string stViewJob = jobPermission.Length < 4 ? "Y" : jobPermission.Substring(3, 1);


            if (Request.QueryString["uid"] != null)
            {
                if (stViewJob == "N")
                {
                    result = false;
                }
                else if (stViewJob == "Y" && stEditejob == "N")
                {
                    lnkSaveTemplate.Visible = false;
                }
            }
            else
            {
                if (stAddejob == "N")
                {
                    result = false;
                }
            }


            //Finance
            string FinancePermission = ds.Rows[0]["FinancePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["FinancePermission"].ToString();
            gvBudgetGrid.Visible = FinancePermission == "N" ? false : true;
            hdnFinancePermission.Value = FinancePermission;

            //bom
            string BOMPermission = ds.Rows[0]["BOMPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["BOMPermission"].ToString();

            string stViewBOM = hdngvBOMViewPermission.Value = BOMPermission.Length < 4 ? "Y" : BOMPermission.Substring(3, 1);

            string stEditBOM = BOMPermission.Length < 2 ? "Y" : BOMPermission.Substring(1, 1);

            // gvBOM.Visible = stViewBOM == "N" ? false : true;
            hdnBOMPermission.Value = BOMPermission;

            chkMaterial.Visible = chkLabor.Visible = stViewBOM == "N" ? false : true;

            chkMaterial.Enabled = chkLabor.Enabled = stEditBOM == "N" ? false : true;

            //Milestones
            string MilestonesPermission = ds.Rows[0]["MilestonesPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["MilestonesPermission"].ToString();
            hdngvMilestonesViewPermission.Value = MilestonesPermission.Length < 4 ? "Y" : MilestonesPermission.Substring(3, 1);

            hdnMilestonesPermission.Value = MilestonesPermission;
            //  gvMilestones.Visible  = stViewMilestones == "N" ? false : true;

            DataTable dsss = new DataTable();
            dsss = GetUserById();

            /////////////JOB STATUS PERMISSION

            //JobClosePermission
            string JobClosePermission = dsss.Rows[0]["JobClosePermission"] == DBNull.Value ? "YYYYYY" : dsss.Rows[0]["JobClosePermission"].ToString();

            if (!JobClosePermission.Contains("Y"))
            {
                hdnClosePermission.Value = "N";
                ViewState["ClosePermission"] = "N";
            }
            else { ViewState["ClosePermission"] = "Y"; }

            //CompletedJObPermission
            string CompletedJObPermission = dsss.Rows[0]["JobCompletedPermission"] == DBNull.Value ? "Y" : dsss.Rows[0]["JobCompletedPermission"].ToString();

            if (!CompletedJObPermission.Contains("Y"))
            {
                hdnCompletedJObPermission.Value = "N";
                ViewState["CompletePermission"] = "N";
            }
            else { ViewState["CompletePermission"] = "Y"; }

            //JobReopenPermission
            string JobReopenPermission = dsss.Rows[0]["JobReopenPermission"] == DBNull.Value ? "Y" : dsss.Rows[0]["JobReopenPermission"].ToString();

            if (!JobReopenPermission.Contains("Y"))
            {
                hdnJobReopenPermission.Value = "N";
                ViewState["ReopenPermission"] = "N";
            }
            else { ViewState["ReopenPermission"] = "Y"; }

        }

        return result;
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


    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtCustomValueChanged = new DataTable();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.Username = Session["Username"].ToString();
            objProp_Customer.UserID = Convert.ToInt32(Session["UserID"].ToString());
            DataTable dtCustom = (DataTable)ViewState["CustomFields"];

            UpdatingCustomDataTableValue(dtCustom);
            var dtCustom1 = new DataTable();
            if (dtCustom != null && dtCustom.Rows.Count > 0)
            {
                dtCustom1 = dtCustom.Copy();
                dtCustom1.Columns.Remove("ControlID");
                dtCustom1.Columns.Remove("IsValueChanged");
                dtCustom1.Columns.Remove("OldValue");

                //var temp = dtCustom.AsEnumerable().Where(m => m.Field<bool?>("IsValueChanged") != null && m.Field<bool?>("IsValueChanged") == true && m.Field<bool>("IsAlert") == true);
                var temp = dtCustom.AsEnumerable().Where(m => m.Field<bool?>("IsValueChanged") != null && m.Field<bool?>("IsValueChanged") == true);

                if (temp.Count() > 0)
                {
                    //dtCustomValueChangedAlert = temp.CopyToDataTable();
                    dtCustomValueChanged = temp.CopyToDataTable();
                }
            }

            objProp_Customer.DtCustom = dtCustom1;
            objProp_Customer.DtTeam = GetTeamItems();

            if (!string.IsNullOrEmpty(hdnCustID.Value))
            {
                objProp_Customer.CustomerID = Convert.ToInt32(hdnCustID.Value);
            }

            if (!string.IsNullOrEmpty(hdnLocID.Value))
            {
                objProp_Customer.LocID = Convert.ToInt32(hdnLocID.Value);
            }

            objProp_Customer.Name = txtREPdesc.Text.Trim();

            if (!ddlJobStatus.SelectedValue.Equals("Select Status"))
            {
                objProp_Customer.Status = Convert.ToInt32(ddlJobStatus.SelectedValue);

                if (objProp_Customer.Status == 1 || objProp_Customer.Status == 3)
                {
                    if (!string.IsNullOrEmpty(txtClosedDate.Text))
                    {
                        objProp_Customer.CloseDate = Convert.ToDateTime(txtClosedDate.Text);
                    }
                }
            }

            if (!ddlJobType.SelectedValue.Equals("Select Type"))
            {
                objProp_Customer.Type = ddlJobType.SelectedValue;
            }

            if (!ddlProjectManger.SelectedValue.Equals("Select Project Manager"))
            {
                objProp_Customer.ProjectManagerUserID = Convert.ToInt32(ddlProjectManger.SelectedItem.Value);
            }

            if (!ddlSupervisor.SelectedValue.Equals("Select Supervisor"))
            {
                objProp_Customer.SupervisorUserID = Convert.ToInt32(ddlSupervisor.SelectedItem.Value);
            }

            objProp_Customer.Remarks = txtREPremarks.Text.Trim();

            if (!ddlTemplate.SelectedValue.Equals("Select Template"))
            {
                objProp_Customer.TemplateID = Convert.ToInt32(ddlTemplate.SelectedValue);
            }

            if (!string.IsNullOrEmpty(txtProjCreationDate.Text))
            {
                objProp_Customer.ProjectCreationDate = Convert.ToDateTime(txtProjCreationDate.Text);
            }
            else
            {
                objProp_Customer.ProjectCreationDate = DateTime.Now;
            }

            objProp_Customer.PO = txtPO.Text;
            objProp_Customer.SO = txtSalesOrder.Text;
            //add attach PO
            objProp_Customer.Certified = Convert.ToInt16(chkCertifiedJob.Checked);
            objProp_Customer.Custom1 = txtCustom1.Text;
            objProp_Customer.Custom2 = txtCustom2.Text;
            objProp_Customer.Custom3 = txtCustom3.Text;
            objProp_Customer.Custom4 = txtCustom4.Text;

            if (!string.IsNullOrEmpty(txtCustom5.Text))
            {
                objProp_Customer.Custom5 = Convert.ToDateTime(txtCustom5.Text);
            }

            #region rol detail
            objProp_Customer.RolName = txtName.Text;
            objProp_Customer.City = txtCity.Text;
            objProp_Customer.Address = GctxtAddress.Text;

            if (!ddlState.SelectedValue.Equals("Select State"))
            {
                objProp_Customer.State = ddlState.SelectedValue;
            }
            objProp_Customer.Zip = txtPostalCode.Text;
            objProp_Customer.Country = txtCountry.Text;
            objProp_Customer.Phone = txtPhone.Text;
            objProp_Customer.Cellular = txtMobile.Text;
            objProp_Customer.Fax = txtFax.Text;
            objProp_Customer.Contact = txtContactName.Text;
            objProp_Customer.Email = txtEmailWeb.Text;
            objProp_Customer.RolRemarks = txtRemarks.Text;
            objProp_Customer.RolType = 8;

            #endregion

            #region finance-general

            if (!string.IsNullOrEmpty(uc_InvExpGL._hdnAcctID.Value))
            {
                objProp_Customer.InvExp = Convert.ToInt32(uc_InvExpGL._hdnAcctID.Value);
            }
            if (!string.IsNullOrEmpty(uc_InterestGL._hdnAcctID.Value))
            {
                objProp_Customer.GLInt = Convert.ToInt32(uc_InterestGL._hdnAcctID.Value);
            }
            if (!string.IsNullOrEmpty(hdnInvServiceID.Value))
            {
                objProp_Customer.InvServ = Convert.ToInt32(hdnInvServiceID.Value);
            }
            if (!string.IsNullOrEmpty(hdnPrevilWageID.Value))
            {
                objProp_Customer.Wage = Convert.ToInt32(hdnPrevilWageID.Value);
            }

            if (!ddlContractType1.SelectedValue.Equals("0"))
            {
                objProp_Customer.ctypeName = ddlContractType1.SelectedValue;
            }
            if (!ddlPostingMethod.SelectedValue.Equals("0"))
            {
                objProp_Customer.Post = Convert.ToInt16(ddlPostingMethod.SelectedValue);
            }
            if (chkChargeInt.Checked.Equals(true))
            {
                objProp_Customer.fInt = 1;
            }
            else
                objProp_Customer.fInt = 0;

            if (chkInvoicing.Checked.Equals(true))
            {
                objProp_Customer.JobClose = 1;
            }
            else
                objProp_Customer.JobClose = 0;

            if (chkChargeable.Checked.Equals(true))
            {
                objProp_Customer.Charge = 1;
            }
            else
                objProp_Customer.Charge = 0;

            #endregion

            #region Milestones
            DataTable dtMilestone = new DataTable();
            dtMilestone = GetMilestoneItems();
            dtMilestone.Columns.Remove("Department");
            dtMilestone.Columns.Remove("isUsed");


            int mline = 1;

            if (ViewState["mLine"] == null)
            {
                dtMilestone.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtMilestone.AcceptChanges();
            }
            else
            {
                mline = (Int16)ViewState["mLine"];
                mline++;
                dtMilestone.Select("Line = 0")
                    .AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtMilestone.AcceptChanges();
            }

            for (int i = 0; i < dtMilestone.Rows.Count; i++)
            {
                dtMilestone.Rows[i]["OrderNo"] = i + 1;
            }
            #endregion

            #region BOM

            DataTable dtBom = GetBOMGridItems();

            dtBom.AcceptChanges();
            int bline = 1;

            if (ViewState["bLine"] == null)
            {
                dtBom.AsEnumerable().ToList().ForEach(t => t["Line"] = bline++);
                dtBom.AcceptChanges();
            }
            else
            {
                bline = (Int16)ViewState["bLine"];
                bline++;
                dtBom.Select("Line = 0").AsEnumerable().ToList().ForEach(t => t["Line"] = bline++);
                dtBom.AcceptChanges();
            }


            #endregion

            if (Request.QueryString["uid"] != null)
            {
                objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            }

            ///////////////////////////////////////////////////////---->

            objProp_Customer.DtBOM = dtBom;

            if (objProp_Customer.DtBOM.Columns.Contains("GroupName"))
            {
                objProp_Customer.DtBOM.Columns.Remove("GroupName");
            }
            if (objProp_Customer.DtBOM.Columns.Contains("TargetHours"))
            {
                objProp_Customer.DtBOM.Columns.Remove("TargetHours");
            }
            if (objProp_Customer.DtBOM.Columns.Contains("BudgetHours"))
            {
                objProp_Customer.DtBOM.Columns.Remove("BudgetHours");
            }
            if (objProp_Customer.DtBOM.Columns.Contains("CodeDesc"))
            {
                objProp_Customer.DtBOM.Columns.Remove("CodeDesc");
            }
            if (objProp_Customer.DtBOM.Columns.Contains("txtLabItem"))
            {
                objProp_Customer.DtBOM.Columns.Remove("txtLabItem");
            }
            if (objProp_Customer.DtBOM.Columns.Contains("BTypes"))
            {
                objProp_Customer.DtBOM.Columns.Remove("BTypes");
            } 

            //////////////////////////////////////////////////////----->
            
            objProp_Customer.DtMilestone = dtMilestone;

            if (objProp_Customer.DtMilestone.Columns.Contains("GroupName"))
            {
                objProp_Customer.DtMilestone.Columns.Remove("GroupName");
            }
            if (objProp_Customer.DtMilestone.Columns.Contains("CodeDesc"))
            {
                objProp_Customer.DtMilestone.Columns.Remove("CodeDesc");
            } 

            //////////////////////////

            if (!string.IsNullOrEmpty(txtBillRate.Text))
            {
                objProp_Customer.BillRate = double.Parse(txtBillRate.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                        NumberStyles.AllowThousands |
                                                                        NumberStyles.AllowDecimalPoint);

            }
            if (!string.IsNullOrEmpty(txtOt.Text))
            {
                objProp_Customer.RateOT = double.Parse(txtOt.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                        NumberStyles.AllowThousands |
                                                                        NumberStyles.AllowDecimalPoint);
            }
            if (!string.IsNullOrEmpty(txtNt.Text))
            {
                objProp_Customer.RateNT = double.Parse(txtNt.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                        NumberStyles.AllowThousands |
                                                                        NumberStyles.AllowDecimalPoint);
            }
            if (!string.IsNullOrEmpty(txtDt.Text))
            {
                objProp_Customer.RateDT = double.Parse(txtDt.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                        NumberStyles.AllowThousands |
                                                                        NumberStyles.AllowDecimalPoint);
            }
            if (!string.IsNullOrEmpty(txtTravel.Text))
            {
                objProp_Customer.RateTravel = double.Parse(txtTravel.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                        NumberStyles.AllowThousands |
                                                                        NumberStyles.AllowDecimalPoint);
            }
            if (!string.IsNullOrEmpty(txtMileage.Text))
            {
                objProp_Customer.Mileage = double.Parse(txtMileage.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                        NumberStyles.AllowThousands |
                                                                        NumberStyles.AllowDecimalPoint);
            }
            if (hdnGCID.Value != string.Empty)

            objProp_Customer.RoleID = Convert.ToInt32(hdnGCID.Value);
            objProp_Customer.dtTaskCode = TaskCodes();
            objProp_Customer.taskcategory = ddlCodeCat.SelectedValue;
            objProp_Customer.SRemarks = txtSpecialInstructions.Text;
            objProp_Customer.Handle = chkspnotes.Checked ? 1 : 0;
            objProp_Customer.IsRenewalNotes = chkRenew.Checked ? 1 : 0;
            objProp_Customer.RenewalNotes = txtRenew.Text;

            #region  add GC and HomeOwner in Role Table

            DataTable tblGCandHomeOwner = new DataTable();

            tblGCandHomeOwner.Columns.Add("ID", typeof(int));
            tblGCandHomeOwner.Columns.Add("NAME", typeof(string));
            tblGCandHomeOwner.Columns.Add("City", typeof(string));
            tblGCandHomeOwner.Columns.Add("State", typeof(string));
            tblGCandHomeOwner.Columns.Add("Zip", typeof(string));
            tblGCandHomeOwner.Columns.Add("Phone", typeof(string));
            tblGCandHomeOwner.Columns.Add("Fax", typeof(string));
            tblGCandHomeOwner.Columns.Add("Contact", typeof(string));
            tblGCandHomeOwner.Columns.Add("Remarks", typeof(string));
            tblGCandHomeOwner.Columns.Add("Country", typeof(string));
            tblGCandHomeOwner.Columns.Add("Cellular", typeof(string));
            tblGCandHomeOwner.Columns.Add("EMail", typeof(string));
            tblGCandHomeOwner.Columns.Add("Type", typeof(int));
            tblGCandHomeOwner.Columns.Add("Address", typeof(string));
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                DataRow drGC = tblGCandHomeOwner.NewRow();

                if (!string.IsNullOrEmpty(hdnGContractorID.Value))
                { drGC["ID"] = hdnGContractorID.Value; }
                else { drGC["ID"] = "0"; }

                drGC["Type"] = 1;
                drGC["NAME"] = txtName.Text;
                drGC["City"] = txtCity.Text;
                drGC["Address"] = GctxtAddress.Text;
                if (!ddlState.SelectedValue.Equals("Select State"))
                    drGC["State"] = ddlState.SelectedValue;
                else drGC["State"] = "0";

                drGC["Zip"] = txtPostalCode.Text;
                drGC["Phone"] = txtPhone.Text;
                drGC["Fax"] = txtFax.Text;
                drGC["Contact"] = txtContactName.Text;
                drGC["Remarks"] = txtRemarks.Text;
                drGC["Country"] = txtCountry.Text;
                drGC["Cellular"] = txtMobile.Text;
                drGC["EMail"] = txtEmailWeb.Text;
                tblGCandHomeOwner.Rows.Add(drGC);
            }
            if (!string.IsNullOrEmpty(hotxtname.Text))
            {
                DataRow drHomeOwner = tblGCandHomeOwner.NewRow();
                if (!string.IsNullOrEmpty(hdnHomeOwnerID.Value))
                { drHomeOwner["ID"] = hdnHomeOwnerID.Value; }
                else { drHomeOwner["ID"] = "0"; }
                drHomeOwner["Type"] = 2;
                drHomeOwner["NAME"] = hotxtname.Text;
                drHomeOwner["City"] = hotxtcity.Text;
                drHomeOwner["Address"] = HotxtAddress.Text;
                if (!hotddlstate.SelectedValue.Equals("Select State"))
                    drHomeOwner["State"] = hotddlstate.SelectedValue;
                else drHomeOwner["State"] = "0";

                drHomeOwner["Zip"] = hotxtZIP.Text;
                drHomeOwner["Phone"] = hotxtPhone.Text;
                drHomeOwner["Fax"] = HotxtFax.Text;
                drHomeOwner["Contact"] = hotxtContactName.Text;
                drHomeOwner["Remarks"] = hotxtRemarks.Text;
                drHomeOwner["Country"] = hotxtCountry.Text;
                drHomeOwner["Cellular"] = hotxtMobile.Text;
                drHomeOwner["EMail"] = HotxtEmailWeb.Text;
                tblGCandHomeOwner.Rows.Add(drHomeOwner);


            }
            #endregion

            objProp_Customer.tblGCandHomeOwner = tblGCandHomeOwner;
            objProp_Customer.PWIP = chkProgressBilling.Checked;
            if (!string.IsNullOrEmpty(hdnUnrecognizedRevenue.Value))
                objProp_Customer.UnrecognizedRevenue = Convert.ToInt32(hdnUnrecognizedRevenue.Value);
            if (!string.IsNullOrEmpty(hdnUnrecognizedExpense.Value))
                objProp_Customer.UnrecognizedExpense = Convert.ToInt32(hdnUnrecognizedExpense.Value);
            if (!string.IsNullOrEmpty(hdnRetainageReceivable.Value))
                objProp_Customer.RetainageReceivable = Convert.ToInt32(hdnRetainageReceivable.Value);
            objProp_Customer.ArchitectName = txtArchitectName.Text;
            objProp_Customer.ArchitectAdress = txtArchitectAdress.Text;
            objProp_Customer.PType = Convert.ToInt16(ddlPType.SelectedValue);

            objProp_Customer.JBillingDate = !string.IsNullOrEmpty(txtBillingDate.Text) ? Convert.ToDateTime(txtBillingDate.Text) : DateTime.Now;
            objProp_Customer.JPeriodDate = !string.IsNullOrEmpty(txtPeriodDate.Text) ? Convert.ToDateTime(txtPeriodDate.Text) : DateTime.Now;
            objProp_Customer.JRevisionDate = !string.IsNullOrEmpty(txtRevisionDate.Text) ? Convert.ToDateTime(txtRevisionDate.Text) : DateTime.Now;
            objProp_Customer.ExpectedClosingDate = !string.IsNullOrEmpty(txtExpectedClosingDate.Text) ? Convert.ToDateTime(txtExpectedClosingDate.Text) : (DateTime?)null;
            objProp_Customer.IRemarks = txtInvoiceRemark.Text;

            if (!string.IsNullOrEmpty(txtAmount.Text))
            {
                objProp_Customer.Amount = Convert.ToDouble(txtAmount.Text.Replace('$', '0'));
            }
            else
                objProp_Customer.Amount = 0;
            if (Request.QueryString["uid"] == null)
            {
                objProp_Customer.ProjectJobID = 0;
                objProp_Customer.job = 0; // This is not used in this method but still updating it
            }

            objProp_Customer.TargetHPermission = chkTargetHours.Checked == true ? 1 : 0;

            // Get
            string groupIds = string.Empty;
            if (objProp_Customer.ProjectJobID == 0 && Session["NewProjGroupList"] != null)
            {
                DataTable dt = (DataTable)Session["NewProjGroupList"];

                foreach (DataRow item in dt.Rows)
                {
                    groupIds += item["Id"].ToString() + ",";
                }
                if (!string.IsNullOrEmpty(groupIds))
                {
                    groupIds = groupIds.TrimEnd(',');
                }
            }

    
            if (objProp_Customer.Status == 1 && ViewState["ClosePermission"] != null && ViewState["ClosePermission"].ToString() == "N")
            {
                // Show message: you have no permission to close project
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "closePermissionWarning", "noty({text: 'You do not have permission to close project!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }

            if (objProp_Customer.Status == 3 && ViewState["CompletePermission"] != null && ViewState["CompletePermission"].ToString() == "N")
            {
                // Show message: you have no permission to complete project
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "completePermissionWarning", "noty({text: 'You do not have permission to complete project!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }


            objProp_Customer.ProjectStageID = Convert.ToInt32(ddlStage.SelectedValue);

            //  int jobid = objBL_Customer.AddProject(objProp_Customer, groupIds);


            ///////////////////////////////////////////////////////////////// CREATED NEW METHOD
            int jobid = 0;

            if (Request.QueryString["uid"] == null)
            {
                 jobid = new BL_Job().AddProject_New2021(objProp_Customer, groupIds);
            }
            else
            {
                jobid = new BL_Job().Update_New2021(objProp_Customer, groupIds);  
            }
            //////////////////////////////////////////////////////////////////////
            Emp _objEmp = new Emp();
            _objEmp.ConnConfig = Session["config"].ToString();
            _objEmp.ID = jobid;
            _objEmp.dtWageCategory = GetWageTableItem();
            _objEmp.dtWageDeduction = GetWageDeductionTableItem();
            if (Request.QueryString["uid"] != null)
            {
                _objEmp.Type = 1;
            }
            else
            {
                _objEmp.Type = 0;
            }
            objBL_Wage.AddWageProject(_objEmp);
            ////////////////////////////////////////////////////////////////



            //Update job custom
            updateJobCustom(jobid);
            // Send notification to Member

            if (ViewState["AllProjectTeamMemberList"] != null)
            {

                DataTable lstProjectTeamMember = (DataTable)ViewState["AllProjectTeamMemberList"];
                int totalMembers = 0;
                int totalSentEmails = 0;
                int totalSendErr = 0;

                List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
                List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
                Tuple<int, string, string> emailSendError = null;
                Tuple<int, string, string> emailGetSentError = null;
                StringBuilder sbdSentError = new StringBuilder();
                StringBuilder sbdGetSentError = new StringBuilder();

                EmailLog emailLog = new EmailLog();
                emailLog.ConnConfig = Session["config"].ToString();
                emailLog.Function = "Workflow Fields Alert";
                emailLog.Screen = "Project";
                emailLog.Username = Session["Username"].ToString();
                emailLog.SessionNo = Guid.NewGuid().ToString();
                emailLog.Ref = objProp_Customer.ProjectJobID;

                //foreach (DataRow item in dtCustomValueChangedAlert.Rows)
                foreach (DataRow item in dtCustomValueChanged.Rows)
                {
                    var memberkeys = item["TeamMember"].ToString().Split(';');
                    totalMembers += memberkeys.Count();

                    foreach (var key in memberkeys)
                    {
                        var tempArr = key.Split('_');
                        var intUserType = tempArr[0];
                        if (intUserType != "6" && intUserType != "7")
                        {
                            var newkey = key;
                            if (tempArr.Length == 3)
                            {
                                var tempArr1 = tempArr.Where((source, index) => index != 2).ToArray();
                                newkey = string.Join("_", tempArr1);
                            }
                            var changedItem = lstProjectTeamMember.Select("memberkey='" + newkey.Trim() + "'").FirstOrDefault();
                            var mailTitle = string.Format("{2} - project#: {0} - {1}", objProp_Customer.ProjectJobID, objProp_Customer.Name, item["Label"]);
 

                            StringBuilder sbdTaskRemask = new StringBuilder();
                            sbdTaskRemask.AppendLine("This project has changes to alert you.");
                            sbdTaskRemask.AppendFormat("Project# {0} - {1}", objProp_Customer.ProjectJobID, objProp_Customer.Name);
                            sbdTaskRemask.AppendLine();
                            sbdTaskRemask.AppendFormat("Project location: {0}", txtLocation.Text);
                            sbdTaskRemask.AppendLine();

                            int format = Convert.ToInt32(item["Format"]);
                            string strFormat = string.Empty;
                            string strOldValue = item["OldValue"] != null ? item["OldValue"].ToString() : "";
                            string strNewValue = item["Value"] != null ? item["Value"].ToString() : "";
                            switch (format)
                            {
                                case 1:
                                    strFormat = "Currency";
                                    break;
                                case 2:
                                    strFormat = "Date";
                                    break;
                                case 3:
                                    strFormat = "Text";
                                    break;
                                case 4:
                                    strFormat = "Dropdown";
                                    if (string.IsNullOrEmpty(strOldValue)) strOldValue = "Select";
                                    if (string.IsNullOrEmpty(strNewValue)) strNewValue = "Select";
                                    break;
                                case 5:
                                    strFormat = "Checkbox";
                                    break;
                                case 6:
                                    strFormat = "Notes";
                                    break;
                                case 7:
                                    strFormat = "CheckboxWithComment";
                                    break;
                            }

                            var updatedDate = Convert.ToDateTime(item["UpdatedDate"]);
                            //sbdContent.AppendFormat("{0} - {1} value changed from {4} to {5} by {2} - {3}"
                            //    , item["Label"], strFormat, item["Username"], updatedDate.ToString("MM/dd/yyyy HH:mm tt"), strOldValue, strNewValue);

                            sbdTaskRemask.AppendFormat("{0} - {1} value changed from {4} to {5} by {2} - {3}"
                                , item["Label"], strFormat, item["Username"], updatedDate.ToString("MM/dd/yyyy HH:mm tt"), strOldValue, strNewValue);


                            // If user type is Office or Field then check and create tasks
                            if (intUserType == "0" || intUserType == "1")
                            {
                                if (tempArr.Length == 3 && tempArr[2] == "1")
                                {
                                    var memberUserName = (string)changedItem["fUser"];
                                    CreateTaskOnProjectUpdated(mailTitle, sbdTaskRemask.ToString(), memberUserName, jobid);
                                }
                                else if (Convert.ToBoolean(item["IsAlert"]))
                                {
                                    if (changedItem != null && changedItem["email"] != null && changedItem["email"].ToString() != string.Empty)
                                    {
                                        SendingAlertEmailOnWorkflowChanged(item, changedItem["email"].ToString().Trim(), emailSendError, sbdSentError, mimeErrorMessages
                                            , emailLog, mimeSentMessages, ref totalSendErr);
                                    }
                                }
                            }
                            else if (Convert.ToBoolean(item["IsAlert"]))
                            {
                                if (changedItem != null && changedItem["email"] != null && changedItem["email"].ToString() != string.Empty)
                                {
                                    SendingAlertEmailOnWorkflowChanged(item, changedItem["email"].ToString().Trim(), emailSendError, sbdSentError, mimeErrorMessages
                                            , emailLog, mimeSentMessages, ref totalSendErr);
                                }
                            }
                        }
                        else
                        {
                            var newkey = key;
                            if (intUserType == "6")
                            {
                                if (tempArr.Length == 3)
                                {
                                    var tempArr1 = tempArr.Where((source, index) => index != 2).ToArray();
                                    newkey = string.Join("_", tempArr1);
                                }
                            }
                            else
                            {
                                if (tempArr.Length >= 3)
                                {
                                    var tempArr1 = tempArr.Where((source, index) => index != (tempArr.Length - 1)).ToArray();
                                    newkey = string.Join("_", tempArr1);
                                }
                            }


                            var changedItem = lstProjectTeamMember.Select("memberkey='" + newkey.Trim() + "'").FirstOrDefault();
                            var role = changedItem["RoleName"].ToString();
                            var mailTitle = string.Format("{2} - project#: {0} - {1}", objProp_Customer.ProjectJobID, objProp_Customer.Name, item["Label"]);

                            StringBuilder sbdTaskRemask = new StringBuilder();
                            sbdTaskRemask.AppendLine("This project has changes to alert you.");
                            sbdTaskRemask.AppendFormat("Project# {0} - {1}", objProp_Customer.ProjectJobID, objProp_Customer.Name);
                            sbdTaskRemask.AppendLine();
                            sbdTaskRemask.AppendFormat("Project location: {0}", txtLocation.Text);
                            sbdTaskRemask.AppendLine();

                            int format = Convert.ToInt32(item["Format"]);
                            string strFormat = string.Empty;
                            string strOldValue = item["OldValue"] != null ? item["OldValue"].ToString() : "";
                            string strNewValue = item["Value"] != null ? item["Value"].ToString() : "";
                            switch (format)
                            {
                                case 1:
                                    strFormat = "Currency";
                                    break;
                                case 2:
                                    strFormat = "Date";
                                    break;
                                case 3:
                                    strFormat = "Text";
                                    break;
                                case 4:
                                    strFormat = "Dropdown";
                                    if (string.IsNullOrEmpty(strOldValue)) strOldValue = "Select";
                                    if (string.IsNullOrEmpty(strNewValue)) strNewValue = "Select";
                                    break;
                                case 5:
                                    strFormat = "Checkbox";
                                    break;
                                case 6:
                                    strFormat = "Notes";
                                    break;
                                case 7:
                                    strFormat = "CheckboxWithComment";
                                    break;
                            }

                            var updatedDate = Convert.ToDateTime(item["UpdatedDate"]);
                            //sbdContent.AppendFormat("{0} - {1} value changed from {4} to {5} by {2} - {3}"
                            //    , item["Label"], strFormat, item["Username"], updatedDate.ToString("MM/dd/yyyy HH:mm tt"), strOldValue, strNewValue);

                            sbdTaskRemask.AppendFormat("{0} - {1} value changed from {4} to {5} by {2} - {3}"
                                , item["Label"], strFormat, item["Username"], updatedDate.ToString("MM/dd/yyyy HH:mm tt"), strOldValue, strNewValue);


                            var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "' AND UserType<>'UserRole' AND UserType<>'Project Team Title'").ToList();
                            totalMembers += projTeamUsers.Count;
                            if (projTeamUsers.Count > 0)
                            {
                                foreach (var projUser in projTeamUsers)
                                {
                                    if (intUserType == "6")
                                    {
                                        if (tempArr.Length == 3 && tempArr[2] == "1")
                                        {
                                            var memberUserName = (string)projUser["fUser"];
                                            if (!string.IsNullOrEmpty(memberUserName))
                                            {
                                                // Get user information from usename and email: firstname, lastname, isapplypolicy, username
                                                User objProp_User = new User();
                                                objProp_User.Username = memberUserName;
                                                objProp_User.DBName = Session["dbname"].ToString();
                                                objProp_User.DBType = "";
                                                objProp_User.ConnConfig = Session["config"].ToString();

                                                // get user info
                                                var ds = objBL_User.GetUserInfoByUsername(objProp_User);
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    CreateTaskOnProjectUpdated(mailTitle, sbdTaskRemask.ToString(), memberUserName, jobid);
                                                }
                                                else if (Convert.ToBoolean(item["IsAlert"]))
                                                {
                                                    if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                    {
                                                        SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                        , emailLog, mimeSentMessages, ref totalSendErr);
                                                    }
                                                }
                                            }
                                            else if (Convert.ToBoolean(item["IsAlert"]))
                                            {
                                                if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                {
                                                    SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                    , emailLog, mimeSentMessages, ref totalSendErr);
                                                }
                                            }
                                        }
                                        else if (Convert.ToBoolean(item["IsAlert"]))
                                        {
                                            if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                            {
                                                SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                , emailLog, mimeSentMessages, ref totalSendErr);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (tempArr.Length >= 3)
                                        {
                                            var tempTitle = tempArr[tempArr.Length - 2];
                                            if (tempArr[tempArr.Length - 1] == "1" && tempTitle.Substring(tempTitle.Length - 1) == "|")
                                            {
                                                var memberUserName = (string)projUser["fUser"];
                                                if (!string.IsNullOrEmpty(memberUserName))
                                                {
                                                    // Get user information from usename and email: firstname, lastname, isapplypolicy, username
                                                    User objProp_User = new User();
                                                    objProp_User.Username = memberUserName;
                                                    objProp_User.DBName = Session["dbname"].ToString();
                                                    objProp_User.DBType = "";
                                                    objProp_User.ConnConfig = Session["config"].ToString();

                                                    // get user info
                                                    var ds = objBL_User.GetUserInfoByUsername(objProp_User);
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        CreateTaskOnProjectUpdated(mailTitle, sbdTaskRemask.ToString(), memberUserName, jobid);
                                                    }
                                                    else if (Convert.ToBoolean(item["IsAlert"]))
                                                    {
                                                        if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                        {
                                                            SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                            , emailLog, mimeSentMessages, ref totalSendErr);
                                                        }
                                                    }
                                                }
                                                else if (Convert.ToBoolean(item["IsAlert"]))
                                                {
                                                    if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                    {
                                                        SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                        , emailLog, mimeSentMessages, ref totalSendErr);
                                                    }
                                                }
                                            }
                                            else if (Convert.ToBoolean(item["IsAlert"]))
                                            {
                                                if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                {
                                                    SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                    , emailLog, mimeSentMessages, ref totalSendErr);
                                                }
                                            }
                                        }
                                        else if (Convert.ToBoolean(item["IsAlert"]))
                                        {
                                            if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                            {
                                                SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                , emailLog, mimeSentMessages, ref totalSendErr);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Email in case Project Stage Changed
                if (ddlStage.SelectedValue != "0" && ddlStage.SelectedValue != hdnOrgStageID.Value)
                {
                    emailLog.Function = lblStage.InnerText + " Changed Alert";
                    var stageItem = objBL_Customer.GetProjectStageItemByID(Session["config"].ToString(), ddlStage.SelectedValue);
                    if (stageItem.Tables.Count > 0 && stageItem.Tables[0].Rows.Count > 0)
                    {
                        var item = stageItem.Tables[0].Rows[0];
                        var memberkeys = item["TeamMember"].ToString().Split(';');
                        totalMembers += memberkeys.Count();

                        foreach (var key in memberkeys)
                        {
                            var tempArr = key.Split('_');
                            var intUserType = tempArr[0];
                            if (intUserType != "6" && intUserType != "7")
                            {
                                var newkey = key;
                                if (tempArr.Length == 3)
                                {
                                    var tempArr1 = tempArr.Where((source, index) => index != 2).ToArray();
                                    newkey = string.Join("_", tempArr1);
                                }
                                var changedItem = lstProjectTeamMember.Select("memberkey='" + newkey.Trim() + "'").FirstOrDefault();
                                var mailTitle = string.Format("{2} - project#: {0} - {1}", objProp_Customer.ProjectJobID, objProp_Customer.Name, item["Label"]);

                                //StringBuilder sbdContent = new StringBuilder();
                                //sbdContent.Append("This project has changes to alert you.<br/><br/>");
                                //sbdContent.AppendFormat("Project# {0} - {1}<br/>", objProp_Customer.ProjectJobID, objProp_Customer.Name);
                                //sbdContent.AppendFormat("Project location: {0}<br/><br/>", txtLocation.Text);

                                StringBuilder sbdTaskRemask = new StringBuilder();
                                sbdTaskRemask.AppendLine("This project stage has changes to alert you.");
                                sbdTaskRemask.AppendFormat("Project# {0} - {1}", objProp_Customer.ProjectJobID, objProp_Customer.Name);
                                sbdTaskRemask.AppendLine();
                                sbdTaskRemask.AppendFormat("Project location: {0}", txtLocation.Text);
                                sbdTaskRemask.AppendLine();

                                string strOldValue = hdnOrgStage.Value != null ? hdnOrgStage.Value : "";
                                string strNewValue = item["Label"] != null ? item["Label"].ToString() : "";

                                sbdTaskRemask.AppendFormat("Project stage changed from {0} to {1} by {2} - {3}"
                                        , strOldValue, strNewValue, Session["Username"].ToString(), DateTime.Now.ToString("MM/dd/yyyy HH:mm tt"));
                            

                                // If user type is Office or Field then check and create tasks
                                if (intUserType == "0" || intUserType == "1")
                                {
                                    if (tempArr.Length == 3 && tempArr[2] == "1")
                                    {
                                        var memberUserName = (string)changedItem["fUser"];
                                        CreateTaskOnProjectUpdated(mailTitle, sbdTaskRemask.ToString(), memberUserName, jobid);
                                    }
                                    else if (Convert.ToBoolean(item["IsAlert"]))
                                    {
                                        if (changedItem != null && changedItem["email"] != null && changedItem["email"].ToString() != string.Empty)
                                        {
                                            SendingAlertEmailOnStageChanged(strNewValue, strOldValue, changedItem["email"].ToString().Trim(), emailSendError, sbdSentError, mimeErrorMessages
                                                , emailLog, mimeSentMessages, ref totalSendErr);
                                        }
                                    }
                                }
                                else if (Convert.ToBoolean(item["IsAlert"]))
                                {
                                    if (changedItem != null && changedItem["email"] != null && changedItem["email"].ToString() != string.Empty)
                                    {
                                        SendingAlertEmailOnStageChanged(strNewValue, strOldValue, changedItem["email"].ToString().Trim(), emailSendError, sbdSentError, mimeErrorMessages
                                                , emailLog, mimeSentMessages, ref totalSendErr);
                                    }
                                }
                            }
                            else
                            {
                                var newkey = key;
                                if (intUserType == "6")
                                {
                                    if (tempArr.Length == 3)
                                    {
                                        var tempArr1 = tempArr.Where((source, index) => index != 2).ToArray();
                                        newkey = string.Join("_", tempArr1);
                                    }
                                }
                                else
                                {
                                    if (tempArr.Length >= 3)
                                    {
                                        var tempArr1 = tempArr.Where((source, index) => index != (tempArr.Length - 1)).ToArray();
                                        newkey = string.Join("_", tempArr1);
                                    }
                                }


                                var changedItem = lstProjectTeamMember.Select("memberkey='" + newkey.Trim() + "'").FirstOrDefault();
                                var role = changedItem["RoleName"].ToString();
                                var mailTitle = string.Format("{2} - project#: {0} - {1}", objProp_Customer.ProjectJobID, objProp_Customer.Name, item["Label"]);

                                StringBuilder sbdTaskRemask = new StringBuilder();
                                sbdTaskRemask.AppendLine("This project stage has changes to alert you.");
                                sbdTaskRemask.AppendFormat("Project# {0} - {1}", objProp_Customer.ProjectJobID, objProp_Customer.Name);
                                sbdTaskRemask.AppendLine();
                                sbdTaskRemask.AppendFormat("Project location: {0}", txtLocation.Text);
                                sbdTaskRemask.AppendLine();

                                string strOldValue = hdnOrgStage.Value != null ? hdnOrgStage.Value : "";
                                string strNewValue = item["Label"] != null ? item["Label"].ToString() : "";

                                sbdTaskRemask.AppendFormat("Project stage changed from {0} to {1} by {2} - {3}"
                                        , strOldValue, strNewValue, Session["Username"].ToString(), DateTime.Now.ToString("MM/dd/yyyy HH:mm tt"));
                              

                                //var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "' AND UserType='Project Team'").ToList();
                                var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "' AND UserType<>'UserRole' AND UserType<>'Project Team Title'").ToList();
                                totalMembers += projTeamUsers.Count;
                                if (projTeamUsers.Count > 0)
                                {
                                    foreach (var projUser in projTeamUsers)
                                    {
                                        if (intUserType == "6")
                                        {
                                            if (tempArr.Length == 3 && tempArr[2] == "1")
                                            {
                                                var memberUserName = (string)projUser["fUser"];
                                                if (!string.IsNullOrEmpty(memberUserName))
                                                {
                                                    // Get user information from usename and email: firstname, lastname, isapplypolicy, username
                                                    User objProp_User = new User();
                                                    objProp_User.Username = memberUserName;
                                                    objProp_User.DBName = Session["dbname"].ToString();
                                                    objProp_User.DBType = "";
                                                    objProp_User.ConnConfig = Session["config"].ToString();

                                                    // get user info
                                                    var ds = objBL_User.GetUserInfoByUsername(objProp_User);
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        CreateTaskOnProjectUpdated(mailTitle, sbdTaskRemask.ToString(), memberUserName, jobid);
                                                    }
                                                    else if (Convert.ToBoolean(item["IsAlert"]))
                                                    {
                                                        if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                        {
                                                            SendingAlertEmailOnStageChanged(strNewValue, strOldValue, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                            , emailLog, mimeSentMessages, ref totalSendErr);
                                                        }
                                                    }
                                                }
                                                else if (Convert.ToBoolean(item["IsAlert"]))
                                                {
                                                    if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                    {
                                                        SendingAlertEmailOnStageChanged(strNewValue, strOldValue, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                        , emailLog, mimeSentMessages, ref totalSendErr);
                                                    }
                                                }
                                            }
                                            else if (Convert.ToBoolean(item["IsAlert"]))
                                            {
                                                if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                {
                                                    SendingAlertEmailOnStageChanged(strNewValue, strOldValue, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                    , emailLog, mimeSentMessages, ref totalSendErr);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (tempArr.Length >= 3)
                                            {
                                                var tempTitle = tempArr[tempArr.Length - 2];
                                                if (tempArr[tempArr.Length - 1] == "1" && tempTitle.Substring(tempTitle.Length - 1) == "|")
                                                {
                                                    var memberUserName = (string)projUser["fUser"];
                                                    if (!string.IsNullOrEmpty(memberUserName))
                                                    {
                                                        // Get user information from usename and email: firstname, lastname, isapplypolicy, username
                                                        User objProp_User = new User();
                                                        objProp_User.Username = memberUserName;
                                                        objProp_User.DBName = Session["dbname"].ToString();
                                                        objProp_User.DBType = "";
                                                        objProp_User.ConnConfig = Session["config"].ToString();

                                                        // get user info
                                                        var ds = objBL_User.GetUserInfoByUsername(objProp_User);
                                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                        {
                                                            CreateTaskOnProjectUpdated(mailTitle, sbdTaskRemask.ToString(), memberUserName, jobid);
                                                        }
                                                        else if (Convert.ToBoolean(item["IsAlert"]))
                                                        {
                                                            if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                            {
                                                                SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                                , emailLog, mimeSentMessages, ref totalSendErr);
                                                            }
                                                        }
                                                    }
                                                    else if (Convert.ToBoolean(item["IsAlert"]))
                                                    {
                                                        if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                        {
                                                            SendingAlertEmailOnStageChanged(strNewValue, strOldValue, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                            , emailLog, mimeSentMessages, ref totalSendErr);
                                                        }
                                                    }
                                                }
                                                else if (Convert.ToBoolean(item["IsAlert"]))
                                                {
                                                    if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                    {
                                                        SendingAlertEmailOnStageChanged(strNewValue, strOldValue, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                        , emailLog, mimeSentMessages, ref totalSendErr);
                                                    }
                                                }
                                            }
                                            else if (Convert.ToBoolean(item["IsAlert"]))
                                            {
                                                if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                {
                                                    SendingAlertEmailOnStageChanged(strNewValue, strOldValue, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                    , emailLog, mimeSentMessages, ref totalSendErr);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
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
                                lstTenMessages.Add(mimeSentMessages.Take(10).ToList());
                                mimeSentMessages = mimeSentMessages.Skip(10).ToList();
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


                if (totalSentEmails > 0)
                {
                    //var successfullMess = "There were " + totalSentEmails + " of "
                    //    + totalMembers + " members sent out successfully.";
                    var successfullMess = "There were " + totalSentEmails + " emails sent out successfully.";
                    if (totalSendErr > 0)
                    {
                        //successfullMess += "<br>Total " + totalSendErr + " failed of "
                        //    + totalMembers + " members could not be sent.";
                        successfullMess += "<br>Total " + totalSendErr + " emails" + " could not be sent.";
                    }
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    if (emailGetSentError != null)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    if (totalSendErr > 0)
                    {
                        string str = "There were no emails sent out.";
                        //str += "<br>Total " + totalSendErr + " failed of "
                        //    + totalMembers + " members could not be sent.";
                        str += "<br>Total " + totalSendErr + " emails could not be sent.";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }

            }

            // Reset Project Group session
            Session["NewProjGroupList"] = null;

            if (Request.QueryString["uid"] != null)
            {
                GetData();
                FillTasks(hdnLocRolID.Value);
                UpdateTodoTasksNumberMasterpage();
                RadGrid_gvLogs.Rebind();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Project Updated Successfully! <BR/>Project# " + jobid.ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                //objgn.ResetFormControlValues(this);
                ////Response.Redirect(Page.Request.RawUrl, false);
                //Initialize();
                if (!string.IsNullOrEmpty(Request.QueryString["redirect"]))
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Project Added Successfully! <BR/>Project# " + jobid.ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false, modal : true}); setTimeout(function(){ window.location = 'addproject.aspx?uid=" + jobid.ToString() + "&redirect=" + HttpUtility.UrlEncode(Request.QueryString["redirect"]) + "'; }, 3000);", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Project Added Successfully! <BR/>Project# " + jobid.ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false, modal : true}); setTimeout(function(){ window.location = 'addproject.aspx?uid=" + jobid.ToString() + "'; }, 3000);", true);
                }
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch')) window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch').click();}", true);

        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", "<br/>");
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            if (str == "You do not have permission to reopen project!")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "reopenPermissionWarning", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }

        }
    }

    protected void lnkCloseTemplate_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["redirect"]))
        {
            Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
        }
        else
        {
            Response.Redirect("project.aspx?fil=1", false);
        }
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            dvCompanyPermission.Visible = true;
        }
        else
        {
            dvCompanyPermission.Visible = false;
        }
    }

    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        FillLoc();
        FillGCCustomer();
    }

    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        FillAddress();
        BindEquip();
    }
    protected void ddlContractType1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlTemplate.SelectedValue = ddlTemplate.SelectedValue;

        DataSet _dsContract = new DataSet();

        _dsContract = new BusinessLayer.Programs.BL_ServiceType().spGetProjectServiceTypeinfo(Session["config"].ToString(), ddlContractType1.SelectedValue.ToString(), Convert.ToInt32(ddlJobType.SelectedValue), ddlType.SelectedValue.ToString(), Convert.ToInt32(ddlRoute.SelectedValue));
        if (_dsContract.Tables[0].Rows.Count > 0)
        {
            hdnInvServiceID.Value = _dsContract.Tables[0].Rows[0]["BillingValue"].ToString();
            txtInvService.Text = _dsContract.Tables[0].Rows[0]["BillingName"].ToString();
            hdnPrevilWageID.Value = _dsContract.Tables[0].Rows[0]["LaborWageValue"].ToString();
            txtPrevilWage.Text = _dsContract.Tables[0].Rows[0]["LaborWageCNAME"].ToString();
            uc_InterestGL._txtGLAcct.Text = _dsContract.Tables[0].Rows[0]["InterestGLNAME"].ToString();
            uc_InterestGL._hdnAcctID.Value = _dsContract.Tables[0].Rows[0]["InterestGLValue"].ToString();
            uc_InvExpGL._txtGLAcct.Text = _dsContract.Tables[0].Rows[0]["ExpenseGLNAME"].ToString();
            uc_InvExpGL._hdnAcctID.Value = _dsContract.Tables[0].Rows[0]["ExpenseGLValue"].ToString();
        }

    }
    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!ddlTemplate.SelectedValue.Equals("Select Template"))
            {
                EnableControl();
                objJob.ConnConfig = Session["config"].ToString();
                objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);
                GetJobTemplate(objJob.ID);
                //ddlTemplate.SelectedValue = ddlTemplate.SelectedValue;
                objProp_Customer.ProjectJobID = Convert.ToInt32(ddlTemplate.SelectedValue);
                objProp_Customer.ConnConfig = Session["config"].ToString();
                DataSet ds = objBL_Customer.getJobTemplateByID(objProp_Customer);

                if (ds.Tables[0].Rows[0]["TargetHPermission"].ToString() == "1")

                    lnktargeted.Visible = lnktargeted1.Visible = chkTargetHours.Checked = true;


                //GetTemplateData(ddlTemplate.SelectedValue); // Fill details into BOM tab

                bool IsExistsb = false;
                bool IsExistsm = false;

                // IN  the Add Mode user can able to change project template.

                if (Request.QueryString["uid"] != null)
                {
                    IsExistsb = IsExistsBOM();
                    IsExistsm = IsExistsMilestone();
                    objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
                }
                else
                {
                    objJob.Job = 0;
                }

                if (!IsExistsb)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        BindgvBOM(ds.Tables[1]);
                        hdnloadbomtab.Value = "1";
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "InitializeDragDropGvBom", "InitializeDragDropGvBom();", true);

                        //Session["gvBOM"] = ds.Tables[1];
                    }
                }

                if (!IsExistsm)
                {
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        BindgvMilestones(ds.Tables[2]);

                    }
                }

                #region Bind Workflow fields
                // comment 8.16.16
                DataSet dsTemp = new DataSet();                   //commented by Mayuri on 28th july,16 incomplete custom field functionality
                dsTemp = objBL_Job.GetProjectTemplateCustomFields(objJob);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    ViewState["IsCustomExist"] = true;
                    CreateCustomTable();
                    DisplayCustomByTab(dsTemp.Tables[0], dsTemp.Tables[1], objJob.ID, objJob.Job);
                }

                #endregion
                DataSet dsJ = objBL_Job.GetJobTById(objJob);

                if (dsJ.Tables[0].Rows.Count > 0)
                {

                    if (ddlJobType.Items.FindByValue(dsJ.Tables[0].Rows[0]["Type"].ToString()) != null)
                    {
                        ddlJobType.SelectedValue = dsJ.Tables[0].Rows[0]["Type"].ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keydepartment", "noty({text: 'Please setup department at project template level!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                    }
                }
                else
                {
                    ddlJobType.SelectedValue = "Select Type";
                }
                if (IsExistsb || IsExistsm)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "WarnTemplate", "WarningTemplate();", true);
                }
                if (string.IsNullOrEmpty(txtREPdesc.Text))
                {
                    txtREPdesc.Text = ddlTemplate.SelectedItem.Text;
                }
                if (ddlJobType.SelectedIndex != 0)
                {
                    FillProjectManager(Convert.ToInt32(ddlJobType.SelectedItem.Value));
                    FillSupervisor(Convert.ToInt32(ddlJobType.SelectedItem.Value));
                    FillStages(ddlJobType.SelectedValue);
                }
                //string TemplatServicetype = ddlContractType1.SelectedValue;
                string TemplatServicetype = "";
                if (ddlType.SelectedValue.ToString() != "" && ddlJobType.SelectedValue.ToString() != "" && ddlRoute.SelectedValue.ToString() != "" && ddlContractType1.SelectedValue != "0")
                {
                    if (ViewState["Editctype"] != null)
                    {
                        TemplatServicetype = Convert.ToString(ViewState["Editctype"]);
                    }
                    FillContractType(ddlType.SelectedValue.ToString(), TemplatServicetype, Convert.ToInt32(ddlJobType.SelectedValue.ToString()), Convert.ToInt32(ddlRoute.SelectedValue.ToString()));
                    //ddlContractType1.SelectedValue = TemplatServicetype;
                }
                //else { FillContractType(ddlType.SelectedValue.ToString(), "", Convert.ToInt32(ddlJobType.SelectedValue.ToString()), Convert.ToInt32(ddlRoute.SelectedValue.ToString())); }

            }
            else
            {
                DisableControl();
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region Budget

    #region Tickets
    protected void ddlPagesOpenCall_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridDataItem gvrPager = (GridDataItem)gvTickets.NamingContainer;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        gvTickets.CurrentPageIndex = ddlPages.SelectedIndex;
        GetOpenCalls(string.Empty);
        //FillGridPaged();
    }

    protected void gvOpenCalls_DataBound(object sender, EventArgs e)
    {
        //GridDataItem gvrPager = (GridDataItem)gvTickets.SelectedItems[0];
        //if (gvrPager == null) return;

        //// get your controls from the gridview
        //DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        //Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        //if (ddlPages != null)
        //{
        //    // populate pager
        //    for (int i = 0; i < gvTickets.PageCount; i++)
        //    {

        //        int intPageNumber = i + 1;
        //        ListItem lstItem = new ListItem(intPageNumber.ToString());

        //        if (i == gvTickets.CurrentPageIndex)
        //            lstItem.Selected = true;

        //        ddlPages.Items.Add(lstItem);
        //    }
        //}

        //// populate page count
        //if (lblPageCount != null)
        //    lblPageCount.Text = gvTickets.PageCount.ToString();
    }

    protected void PaginateOpencalls(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvTickets.CurrentPageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvTickets.CurrentPageIndex = 0;
                break;
            case "prev":
                gvTickets.CurrentPageIndex = intCurIndex - 1;
                break;
            case "next":
                gvTickets.CurrentPageIndex = intCurIndex + 1;
                break;
            case "last":
                gvTickets.CurrentPageIndex = gvTickets.PageCount;
                break;
        }

        // popultate the gridview control
        //GetOpenCalls(string.Empty);
        //  FillGridPaged();
    }

    protected void gvOpenCalls_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        PaginateOpencalls(sender, e);
    }

    public string ShowHoverText(object desc, object reason, object ticket)
    {
        string result = string.Empty;

        result = "<span style='font-size:18px;text-decoration: underline'><B>Ticket#</B> " + ticket + "</B></span><br/>";

        result += "<i>Reason</i>: " + Convert.ToString(reason).Replace("\n", "<br/>");

        if (!string.IsNullOrEmpty(Convert.ToString(desc)))
            result += "<br/><i>Resolution</i>: " + Convert.ToString(desc).Replace("\n", "<br/>");

        return result;
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GetOpenCalls(string.Empty);
        BindTicketList(string.Empty, 1);
        string[] array = { "-2", "0", "5" };
        if (array.Contains(ddlStatus.SelectedValue))
            pnlTicketButtons.Visible = true;
        else
            pnlTicketButtons.Visible = false;
    }

    protected void ddlComp_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindTicketList(string.Empty, 1);
    }
    #endregion

    #region Invoice
    protected void ddlPagesInvoice_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridDataItem gvrPager = (GridDataItem)gvInvoice.NamingContainer;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        gvInvoice.CurrentPageIndex = ddlPages.SelectedIndex;
        GetInvoices();
    }


    protected void gvInvoice_DataBound(object sender, EventArgs e)
    {
        //GridDataItem gvrPager = (GridDataItem)gvInvoice.SelectedItems[0];

        //if (gvrPager == null) return;

        //// get your controls from the gridview
        //DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        //Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        //if (ddlPages != null)
        //{
        //    // populate pager
        //    for (int i = 0; i < gvInvoice.PageCount; i++)
        //    {

        //        int intPageNumber = i + 1;
        //        ListItem lstItem = new ListItem(intPageNumber.ToString());

        //        if (i == gvInvoice.CurrentPageIndex)
        //            lstItem.Selected = true;

        //        ddlPages.Items.Add(lstItem);
        //    }
        //}

        //// populate page count
        //if (lblPageCount != null)
        //    lblPageCount.Text = gvInvoice.PageCount.ToString();
    }

    protected void PaginateInvoice(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvInvoice.CurrentPageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvInvoice.CurrentPageIndex = 0;
                break;
            case "prev":
                gvInvoice.CurrentPageIndex = intCurIndex - 1;
                break;
            case "next":
                gvInvoice.CurrentPageIndex = intCurIndex + 1;
                break;
            case "last":
                gvInvoice.CurrentPageIndex = gvInvoice.PageCount;
                break;
        }

        // popultate the gridview control
        GetInvoices();
    }

    protected void gvInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        PaginateInvoice(sender, e);
    }

    #endregion

    protected void ddlInvoiceStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetInvoices();
    }

    #endregion

    #region Attributes
    protected void gvTeamItems_RowCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddTeam"))
        {
            int rowIndex = gvTeamItems.Items.Count - 1;

            HiddenField hdnIndex = gvTeamItems.Items[rowIndex].Cells[0].FindControl("hdnIndex") as HiddenField;

            DataTable dt = GetTeamItems();
            if (dt.Rows.Count < 1)
            {
                for (int j = 0; j <= rowIndex; j++)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
            }
            DataRow dr2 = dt.NewRow();
            dt.Rows.Add(dr2);

            gvTeamItems.DataSource = dt;
            gvTeamItems.DataBind();
        }
    }
    #endregion


    protected void gvArInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //PaginateInvoice(sender, e);
    }

    protected void ddlJobType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!ddlJobType.SelectedValue.Equals("Select Template"))
            {
                objJob.ConnConfig = Session["config"].ToString();

                objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);
                objJob.Type = Convert.ToInt16(ddlJobType.SelectedValue);
                objJob.IsExist = objBL_Job.IsExistProjectTempByType(objJob);
                if (!objJob.IsExist.Equals(true))
                {
                    ddlTemplate.SelectedValue = "0";
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    
   
   

    #region Custom Functions
    private void GetData()
    {
        try
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objProp_Customer.Type = string.Empty;

            DataSet ds = objBL_Customer.getJobProjectByJobID(objProp_Customer);

            if (ds.Tables[0].Rows[0]["IsContract"].ToString() == "1")
            {
                lblHyperlinklabel.Visible = true;
                lblHeaderLabeldf.Visible = true;
                lblHeaderLabeldf.NavigateUrl = "AddRecContract?uid=" + ds.Tables[0].Rows[0]["ID"].ToString();
                lblHeaderLabeldf.Text = ds.Tables[0].Rows[0]["ID"].ToString();
            }
            else
            {
                lblHyperlinklabel.Visible = false;
                lblHeaderLabeldf.Visible = false;
            }
            if (ds.Tables[0].Rows[0]["template"].ToString() != string.Empty)
            {
                Session["projectname"] = ds.Tables[0].Rows[0]["fDesc"];
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["estimateid"].ToString() != "")
                {
                    trEstimate.Visible = true;
                    lnkEstimateID.Text = ds.Tables[0].Rows[0]["estimateid"].ToString() + " - " + ds.Tables[0].Rows[0]["estimate"].ToString();
                    lnkEstimate.NavigateUrl = "addestimate.aspx?uid=" + ds.Tables[0].Rows[0]["estimateid"].ToString();
                    ViewState["FirstLinkEst"] = ds.Tables[0].Rows[0]["estimateid"].ToString();
                }
                txtREPdesc.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
                txtBillingDate.Text = ds.Tables[0].Rows[0]["billingdate"] != null && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["billingdate"].ToString()) ? Convert.ToDateTime(ds.Tables[0].Rows[0]["billingdate"]).ToString("M/dd/yyyy") : DateTime.Now.ToString("M/dd/yyyy");
                txtRevisionDate.Text = ds.Tables[0].Rows[0]["revisiondate"] != null && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["revisiondate"].ToString()) ? Convert.ToDateTime(ds.Tables[0].Rows[0]["revisiondate"]).ToString("M/dd/yyyy") : DateTime.Now.ToString("M/dd/yyyy");
                txtPeriodDate.Text = ds.Tables[0].Rows[0]["perioddate"] != null && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["perioddate"].ToString()) ? Convert.ToDateTime(ds.Tables[0].Rows[0]["perioddate"]).ToString("M/dd/yyyy") : DateTime.Now.ToString("M/dd/yyyy");
                txtClosedDate.Text = ds.Tables[0].Rows[0]["CloseDate"] != null && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CloseDate"].ToString()) ? Convert.ToDateTime(ds.Tables[0].Rows[0]["CloseDate"]).ToString("M/dd/yyyy") : string.Empty;

                lblProjectNo.Text = txtREPdesc.Text + " # " + ds.Tables[0].Rows[0]["ID"].ToString();

                txtInvoiceRemark.Text = ds.Tables[0].Rows[0]["iremarks"].ToString();
                txtREPremarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                if (ds.Tables[0].Rows[0]["template"].ToString() != string.Empty)
                    ddlTemplate.SelectedValue = ds.Tables[0].Rows[0]["template"].ToString();
                txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                hdnLocID.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                hdnLocRolID.Value = ds.Tables[0].Rows[0]["locRolId"].ToString();
                //Set Hyperlink  For Loc / Customer
                if (hdnLocID.Value != "0")
                {
                    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;

                    lnkaddTicket.Value = "addticket.aspx?locid=" + hdnLocID.Value + "&JobID=" + ds.Tables[0].Rows[0]["ID"].ToString() + "&JobName=" + txtREPdesc.Text;

                    GetDataEquip();
                }
                else
                    lnkLocationID.NavigateUrl = "";
                hdnCustID.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                if (hdnCustID.Value != "0")
                    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustID.Value;
                else
                    lnkCustomerID.NavigateUrl = "";
                txtCustomer.Text = ds.Tables[0].Rows[0]["customerName"].ToString();
                ddlJobType.SelectedValue = ds.Tables[0].Rows[0]["Type"].ToString();
                ddlJobStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
                hdnddlJobStatus.Value = ds.Tables[0].Rows[0]["Status"].ToString();
                ddlCodeCat.SelectedValue = ds.Tables[0].Rows[0]["taskcategory"].ToString();

                if (Convert.ToBoolean(ds.Tables[0].Rows[0]["Certified"]))
                {
                    chkCertifiedJob.Checked = true;
                }

                FillAddress();

                if (ds.Tables[0].Rows[0]["template"].ToString() != string.Empty)
                {
                    objJob.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["template"].ToString());
                    objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    DataSet dsCustom = objBL_Job.GetProjectTemplateCustomFields(objJob);
                    Session["dtCustom"] = dsCustom.Tables[0];
                    if (dsCustom.Tables[0].Rows.Count > 0)
                    {
                        btnExport.Visible = true;
                        ViewState["IsCustomExist"] = true;
                        CreateCustomTable();
                        DisplayCustomByTab(dsCustom.Tables[0], dsCustom.Tables[1], objJob.ID, objJob.Job);
                    }

                    RadGrid_Emails.Rebind();
                    //InitTeamMemberGridView();
                }

                if (divJC.Visible)
                    GetJobCost();

                if (ds.Tables[2].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[2].Rows[0]["bLine"].ToString()))
                    {
                        ViewState["bLine"] = Convert.ToInt16(ds.Tables[2].Rows[0]["bLine"].ToString());
                    }
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0]["mLine"].ToString()))
                    {
                        ViewState["mLine"] = Convert.ToInt16(ds.Tables[3].Rows[0]["mLine"].ToString());
                    }
                }

                if (ds.Tables[0].Rows[0]["TargetHPermission"].ToString() == "1")
                    lnktargeted.Visible = lnktargeted1.Visible = chkTargetHours.Checked = true;

                #region Top Summary
                ///////////  BindSummary();
                #endregion

                #region Attributes 

                #region General
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ProjCreationDate"].ToString()))
                {
                    txtProjCreationDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjCreationDate"]).ToString("MM/dd/yyyy");
                }
                txtPO.Text = ds.Tables[0].Rows[0]["PO"].ToString();
                txtSalesOrder.Text = ds.Tables[0].Rows[0]["SO"].ToString();
                txtCustom1.Text = ds.Tables[0].Rows[0]["Custom21"].ToString();
                txtCustom2.Text = ds.Tables[0].Rows[0]["Custom22"].ToString();
                txtCustom3.Text = ds.Tables[0].Rows[0]["Custom23"].ToString();
                txtCustom4.Text = ds.Tables[0].Rows[0]["Custom24"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Custom25"].ToString()))
                {
                    txtCustom5.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Custom25"]).ToString("MM/dd/yyyy");
                }
                #endregion

                #region  Team
                DataSet TeamDS = objBL_Customer.getJobProject_Team(objProp_Customer);
                if (TeamDS.Tables[0].Rows.Count > 0)
                {
                    gvTeamItems.DataSource = TeamDS.Tables[0];
                    gvTeamItems.DataBind();
                }
                #endregion

                FillProjectManager(Convert.ToInt32(ddlJobType.SelectedItem.Value));
                FillSupervisor(Convert.ToInt32(ddlJobType.SelectedItem.Value));
                // Stage
                FillStages(ddlJobType.SelectedValue);
                ddlStage.SelectedValue = ds.Tables[0].Rows[0]["Stage"].ToString();
                hdnOrgStageID.Value = ds.Tables[0].Rows[0]["Stage"].ToString();
                hdnOrgStage.Value = ddlStage.SelectedItem.Text;
                if (ds.Tables[0].Rows[0]["ProjectManagerUserID"] != null && Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectManagerUserID"]) != 0)
                {
                    ddlProjectManger.SelectedValue = ds.Tables[0].Rows[0]["ProjectManagerUserID"].ToString();
                }

                if (ds.Tables[0].Rows[0]["SupervisorUserID"] != null && Convert.ToInt32(ds.Tables[0].Rows[0]["SupervisorUserID"]) != 0)
                {
                    ddlSupervisor.SelectedValue = ds.Tables[0].Rows[0]["SupervisorUserID"].ToString();
                }

                #region GC Info
                if (!string.IsNullOrEmpty(hdnLocID.Value))
                    GetGC_HOInfo(hdnLocID.Value);
                #endregion

                #region Equipment
                BindEquip();
                #endregion

                #region Notes
                txtSpecialInstructions.Text = ds.Tables[0].Rows[0]["SRemarks"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["SRemarks"]) : string.Empty;
                chkspnotes.Checked = ds.Tables[0].Rows[0]["SPHandle"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["SPHandle"]) : false;
                txtRenew.Text = ds.Tables[0].Rows[0]["RenewalNotes"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["RenewalNotes"]) : string.Empty;
                chkRenew.Checked = ds.Tables[0].Rows[0]["IsRenewalNotes"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRenewalNotes"]) : false;
                #endregion

                #endregion

                #region Finance Tab

                #region finance-general
                if (!Convert.ToInt32(ds.Tables[0].Rows[0]["Template"]).Equals(0))
                {
                    EnableControl();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ctype"].ToString()))
                    {

                        if (ddlType.SelectedValue.ToString() != "" && ddlJobType.SelectedValue.ToString() != "" && ddlRoute.SelectedValue.ToString() != "" && ds.Tables[0].Rows[0]["ctype"].ToString() != "0")
                        {
                            FillContractType(ddlType.SelectedValue.ToString(), ds.Tables[0].Rows[0]["ctype"].ToString(), Convert.ToInt32(ddlJobType.SelectedValue.ToString()), Convert.ToInt32(ddlRoute.SelectedValue.ToString()));


                        }

                        ddlContractType1.SelectedValue = ds.Tables[0].Rows[0]["ctype"].ToString();

                        ViewState["Editctype"] = ds.Tables[0].Rows[0]["ctype"].ToString();
                    }
                    hdnInvServiceID.Value = ds.Tables[0].Rows[0]["InvServ"].ToString();
                    txtInvService.Text = ds.Tables[0].Rows[0]["InvServiceName"].ToString();
                    hdnPrevilWageID.Value = ds.Tables[0].Rows[0]["Wage"].ToString();
                    txtPrevilWage.Text = ds.Tables[0].Rows[0]["WageName"].ToString();
                    uc_InterestGL._txtGLAcct.Text = ds.Tables[0].Rows[0]["GLName"].ToString();
                    uc_InterestGL._hdnAcctID.Value = ds.Tables[0].Rows[0]["GLInt"].ToString();
                    uc_InvExpGL._txtGLAcct.Text = ds.Tables[0].Rows[0]["InvExpName"].ToString();
                    uc_InvExpGL._hdnAcctID.Value = ds.Tables[0].Rows[0]["InvExp"].ToString();
                }
                else
                {
                    DisableControl();
                }
                #endregion

                #region finance-Wage
                BindWageCategoryGrid();
                BindWageDeductionGrid();
                #endregion

                //#region finance-Budgets
                //BindBudget(1);
                //BindExpense(1);
                //#endregion

                //#region finance-Billing
                //GetInvoices();
                //GetAPInvoices();
                //#endregion

                #endregion

                #region Ticket Tab
                SelectTaskCategory();
                GetJobtaskCategory();
                //BindTicketList(string.Empty, 1);
                //if (Convert.ToBoolean(ViewState["tasks"]) == true)
                //{
                //    DataTable dtopencall = GetOpenCalls(string.Empty);
                //    rptTicketTask.DataSource = dtopencall;
                //    rptTicketTask.DataBind();
                //}
                #endregion

                #region BOM Tab
                DataSet BomDS = objBL_Customer.getJobProject_BOM(objProp_Customer);
                if (BomDS.Tables[0].Rows.Count > 0)
                {
                    BindgvBOM(BomDS.Tables[0]);

                }
                else
                {
                    CreateBOMTable();
                }
                #endregion

                #region Belling Tab
                #region  Details
                DataSet MilestonesDS = objBL_Customer.getJobProject_Milestone(objProp_Customer);
                if (MilestonesDS.Tables[0].Rows.Count > 0)
                {
                    BindgvMilestones(MilestonesDS.Tables[0]);
                }
                #endregion

                #region WIP 

                if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["PWIP"])))
                    chkProgressBilling.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["PWIP"].ToString());
                else
                    chkProgressBilling.Checked = false;
                hdnUnrecognizedRevenue.Value = ds.Tables[0].Rows[0]["UnrecognizedRevenue"].ToString();
                if (!String.IsNullOrEmpty(hdnUnrecognizedRevenue.Value))
                    AssignBillingCodeInGrid(hdnUnrecognizedRevenue.Value);
                txtUnrecognizedRevenue.Text = ds.Tables[0].Rows[0]["UnrecognizedRevenueName"].ToString();
                hdnUnrecognizedExpense.Value = ds.Tables[0].Rows[0]["UnrecognizedExpense"].ToString();
                txtUnrecognizedExpense.Text = ds.Tables[0].Rows[0]["UnrecognizedExpenseName"].ToString();
                hdnRetainageReceivable.Value = ds.Tables[0].Rows[0]["RetainageReceivable"].ToString();
                txtRetainageReceivable.Text = ds.Tables[0].Rows[0]["RetainageReceivableName"].ToString();
                txtExpectedClosingDate.Text = !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ExpectedClosingDate"].ToString()) ? Convert.ToDateTime(ds.Tables[0].Rows[0]["ExpectedClosingDate"].ToString()).ToString("M/dd/yyyy") : "";
                txtArchitectName.Text = ds.Tables[0].Rows[0]["ArchitectName"].ToString();
                txtArchitectAdress.Text = ds.Tables[0].Rows[0]["ArchitectAdress"].ToString();
                ddlPType.SelectedValue = ds.Tables[0].Rows[0]["PType"].ToString();
                txtAmount.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["Amount"]));

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Post"].ToString()))
                {
                    ddlPostingMethod.SelectedValue = ds.Tables[0].Rows[0]["Post"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Charge"].ToString().Equals("1"))
                {
                    chkChargeable.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["fInt"].ToString().Equals("1"))
                {
                    chkChargeInt.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["JobClose"].ToString().Equals("1"))
                {
                    chkInvoicing.Checked = true;
                }
                txtBillRate.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                txtOt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                txtNt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
                txtDt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                txtMileage.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));
                txtTravel.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));

                #endregion
                #endregion

                #region Contact
                Fill_gvContacts();
                #endregion

                //Estimate
                ShowLinkEstimate();
                //todo

                String TaxType = "0";

                DataTable tblStaxType = ds.Tables[4];
                if (tblStaxType.Rows.Count > 0)
                {
                    TaxType = tblStaxType.Rows[0]["TaxType"].ToString();
                    hdnGST.Value = tblStaxType.Rows[0]["GSTRate"].ToString();
                    hdnTaxType.Value = tblStaxType.Rows[0]["TaxType"].ToString();
                }
                ViewState["TaxType"] = TaxType;
                SetupCanadaCompanyUI();
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void ShowLinkEstimate()
    {
        if (Request.QueryString["uid"] != null)
        {
            StringBuilder str = new StringBuilder();
            if (ViewState["FirstLinkEst"] != null)
            {
                str.Append("<div style='float:left;'> Estimate:</div>");
                str.AppendFormat("<a style='float :left' href='addestimate?uid={0}&redirect={1}'>#{0}</a>  ", ViewState["FirstLinkEst"].ToString(), HttpUtility.UrlEncode(Request.RawUrl));
                divLinks.InnerHtml = str.ToString().Substring(0, str.ToString().Length - 1);
            }
            else
            {
                //List<String> str = new List<string>();           

                DataSet dsEstimate = objBL_Customer.GetAllEstimateLinkToProject(Session["config"].ToString(), Convert.ToInt32(Request.QueryString["uid"].ToString()));

                if (dsEstimate.Tables[0].Rows.Count > 0)
                {
                    str.Append("<div style='float:left;'> Estimate:</div>");
                    for (int i = 0; i < dsEstimate.Tables[0].Rows.Count; i++)
                    {
                        //str.Add("<a style='float :left' href='addestimate?uid=" + dsEstimate.Tables[0].Rows[i]["ID"].ToString() + "'>#" + dsEstimate.Tables[0].Rows[i]["ID"].ToString() + "</a>");
                        str.AppendFormat("<a style='float :left' href='addestimate?uid={0}&redirect={1}'>#{0}</a>  ", dsEstimate.Tables[0].Rows[i]["ID"].ToString(), HttpUtility.UrlEncode(Request.RawUrl));
                        ViewState["FirstLinkEst"] = dsEstimate.Tables[0].Rows[i]["ID"].ToString();
                        break;
                    }
                    //divLinkEstimate.Visible = true;
                    divLinks.InnerHtml = str.ToString().Substring(0, str.ToString().Length - 1);
                }

            }

        }

    }
    private void BindSummary()
    {
        #region Top Summary
        objJob.ConnConfig = Session["config"].ToString();
        objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
        string StartDate = "", EndDate = "";
        StartDate = txtfromDate.Text;
        EndDate = txtToDate.Text;
        DataSet ds2 = objBL_Job.GetBudgetSummaryGridDataByJob(objJob, StartDate, EndDate);
        if (ds2.Tables[0].Rows.Count > 0)
        {
            gvBudgetGrid.DataSource = ds2.Tables[0];
            gvBudgetGrid.DataBind();
            GridFooterItem footeritem = (GridFooterItem)gvBudgetGrid.MasterTableView.GetItems(GridItemType.Footer)[0];
            if (footeritem != null)
            {
                Label lblRevFooter = footeritem.FindControl("lblRevFooter") as Label;
                Label lblLaborExpFooter = footeritem.FindControl("lblLaborExpFooter") as Label;
                Label lblMaterialExpFooter = footeritem.FindControl("lblMatExpFooter") as Label;
                Label lblOtherExpFooter = footeritem.FindControl("lblOtherExpFooter") as Label;
                Label lblTotalExpFooter = footeritem.FindControl("lblCostFooter") as Label;
                Label lblProfitAmtFooter = footeritem.FindControl("lblProfitFooter") as Label;
                Label lblNetProfitFooter = footeritem.FindControl("lblRatioFooter") as Label;
                Label lblHoursFooter = footeritem.FindControl("lblHoursFooter") as Label;
                Label lblTotalOnOrderFooter = footeritem.FindControl("lblOnOrderFooter") as Label;
                Label lblTotalReceivePOFooter = footeritem.FindControl("lblReceivePOFooter") as Label;

                DataRow dr1 = ds2.Tables[0].Rows[0];
                DataRow dr2 = ds2.Tables[0].Rows[1];

                lblRevFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Rev"]) - Convert.ToDouble(dr2["Rev"]));
                lblLaborExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Labor"]) - Convert.ToDouble(dr2["Labor"]));
                lblMaterialExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Mat"]) - Convert.ToDouble(dr2["Mat"]));
                lblOtherExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["OtherExp"]) - Convert.ToDouble(dr2["OtherExp"]));
                lblTotalExpFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Cost"]) - Convert.ToDouble(dr2["Cost"]));
                lblProfitAmtFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["Profit"]) - Convert.ToDouble(dr2["Profit"]));
                lblNetProfitFooter.Text = string.Format("{0:n}", Convert.ToDouble(dr1["Ratio"]) - Convert.ToDouble(dr2["Ratio"]));
                lblHoursFooter.Text = string.Format("{0:n}", Convert.ToDouble(dr1["hour"]) - Convert.ToDouble(dr2["hour"]));
                lblTotalOnOrderFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["OnOrder"]) - Convert.ToDouble(dr2["OnOrder"]));
                lblTotalReceivePOFooter.Text = string.Format("{0:c}", Convert.ToDouble(dr1["ReceivePO"]) - Convert.ToDouble(dr2["ReceivePO"]));
            }

        }
        #endregion
    }

    private void Permission()
    {

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        ////ul.Attributes.Remove("class");
        //ul.Style.Add("display", "block");

        if (Session["type"].ToString() != "am")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            /// Ticket ///////////////////------->

            string ticketPermission = ds.Rows[0]["TicketPermission"] == DBNull.Value ? "YYNYYY" : ds.Rows[0]["TicketPermission"].ToString();
            hdnAddeTicket.Value = ticketPermission.Length < 1 ? "Y" : ticketPermission.Substring(0, 1);
            hdnEditeTicket.Value = ticketPermission.Length < 2 ? "Y" : ticketPermission.Substring(1, 1);
            hdnDeleteTicket.Value = ticketPermission.Length < 3 ? "Y" : ticketPermission.Substring(2, 1);
            hdnviewTicket.Value = ticketPermission.Length < 4 ? "Y" : ticketPermission.Substring(3, 1);

            //Contact
            string ContactPermission = ds.Rows[0]["ContactPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["ContactPermission"].ToString();
            hdnAddeContact.Value = ContactPermission.Length < 1 ? "Y" : ContactPermission.Substring(0, 1);
            hdnEditeContact.Value = ContactPermission.Length < 2 ? "Y" : ContactPermission.Substring(1, 1);
            hdnDeleteContact.Value = ContactPermission.Length < 3 ? "Y" : ContactPermission.Substring(2, 1);
            hdnViewContact.Value = ContactPermission.Length < 4 ? "Y" : ContactPermission.Substring(3, 1);

            if (hdnAddeContact.Value == "N")
            {
                imgAddContact.Enabled = false;
                btnEditcontact.Enabled = false;
            }

            gvContacts.Visible = panel1.Visible = hdnViewContact.Value == "N" ? false : true;

            //Document
            string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
            hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
            //hdnEditeDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
            //hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
            //hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

            //if (hdnAddeDocument.Value == "N")
            //{
            //    lnkUploadDoc.Enabled = false;
            //}

            //pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;
        }

    }

    private void Initialize()
    {
        CreateBOMTable();
        CreateMilestoneTable();
        CreateTeamTable();
        BindEquip();
    }

   


     
    private void FillItems(DropDownList ddlItem)
    {
        ddlItem.Items.Clear();
        ddlItem.Items.Add(new ListItem("No data found", "0"));
        ddlItem.DataBind();
    }
    #endregion

    private void FillLoc()
    {
        DataSet ds = new DataSet();
        objPropUser.SearchValue = "";
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnCustID.Value);
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }
        #endregion
        ds = objBL_User.getLocationAutojquery(objPropUser);

        if (ds.Tables[0].Rows.Count == 1)
        {
            hdnLocID.Value = ds.Tables[0].Rows[0]["value"].ToString();
            txtLocation.Text = ds.Tables[0].Rows[0]["label"].ToString();
            hdnLocRolID.Value = ds.Tables[0].Rows[0]["rolid"].ToString();
            FillAddress();
            BindEquip();
        }
        //Set Hyperlink  For Loc / Customer
        if (hdnLocID.Value != "0")
        {
            lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;
            GetDataEquip();
        }
        else
            lnkLocationID.NavigateUrl = "";

        if (hdnCustID.Value != "0")
            lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustID.Value;


    }

    private void Fill_gvContacts()
    {

        if (hdnloadcontact.Value == "1")
        {
            int JobID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
            ViewState["mode"] = "0";
            if (JobID > 0)
            {
                ViewState["mode"] = "1";
                objJob.ConnConfig = Session["config"].ToString();
                objJob.Job = JobID;
                DataSet ds = objBL_Job.GetContactForJob(objJob, new GeneralFunctions().GetSalesAsigned());

                if (ds.Tables[0].Rows.Count > 0)
                {

                    gvContacts.DataSource = ds;
                    gvContacts.DataBind();


                    foreach (GridDataItem row in gvContacts.Items)
                    {
                        CheckBox check = (CheckBox)row.FindControl("chkBoxIsRecd");
                        if (check.Checked) { row.BackColor = System.Drawing.Color.Orange; }
                    }
                }
                else
                {
                    gvContacts.DataSource = null;
                    gvContacts.DataBind();

                }
            }
        }
    }

    private void FillGCCustomer()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnCustID.Value);
        ds = objBL_User.getGCCustomer(objPropUser);

        if (ds.Tables[0].Rows.Count == 1)
        {
            if (ds.Tables[0].Rows[0]["type"].ToString().Equals("General Contractor", StringComparison.CurrentCultureIgnoreCase))
            {
                hdnGCID.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                hdnGCIDtemp.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                hdnGCName.Value = ds.Tables[0].Rows[0]["name"].ToString();
                txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                GctxtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                txtCountry.Text = ds.Tables[0].Rows[0]["country"].ToString();
                txtPostalCode.Text = ds.Tables[0].Rows[0]["zip"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                txtContactName.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                txtPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                txtEmailWeb.Text = ds.Tables[0].Rows[0]["email"].ToString();
                txtMobile.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                hdnGContractorID.Value = ds.Tables[0].Rows[0]["Rol"].ToString();
                hdnGCNameupdate.Value = "0";
            }
            else
            {
                hdnGCID.Value = string.Empty;
                hdnGCIDtemp.Value = string.Empty;
                hdnGCName.Value = string.Empty;
                txtName.Text = string.Empty;
                txtCity.Text = string.Empty;
                GctxtAddress.Text = string.Empty;
                ddlState.SelectedValue = "Select State";
                txtCountry.Text = string.Empty;
                txtPostalCode.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtContactName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtFax.Text = string.Empty;
                txtEmailWeb.Text = string.Empty;
                txtMobile.Text = string.Empty;
                hdnGContractorID.Value = string.Empty;
                hdnGCNameupdate.Value = string.Empty;
            }

            if (ds.Tables[0].Rows[0]["type"].ToString().Equals("Homeowner", StringComparison.CurrentCultureIgnoreCase))
            {
                hotxtname.Text = ds.Tables[0].Rows[0]["name"].ToString();
                hotxtcity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                HotxtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["state"].ToString()))
                {
                    hotddlstate.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                }
                hotxtZIP.Text = ds.Tables[0].Rows[0]["zip"].ToString();
                hotxtCountry.Text = ds.Tables[0].Rows[0]["country"].ToString();
                hotxtPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                hotxtMobile.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                HotxtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                HotxtEmailWeb.Text = ds.Tables[0].Rows[0]["email"].ToString();
                hotxtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                hotxtContactName.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                hdnHomeOwnerID.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                hdnHOName.Value = ds.Tables[0].Rows[0]["name"].ToString();
                hdnHONameupdate.Value = "0";
            }
            else
            {
                hotxtname.Text = string.Empty;
                hotxtcity.Text = string.Empty;
                HotxtAddress.Text = string.Empty;
                hotddlstate.SelectedValue = "Select State";
                hotxtZIP.Text = string.Empty;
                hotxtCountry.Text = string.Empty;
                hotxtPhone.Text = string.Empty;
                hotxtMobile.Text = string.Empty;
                HotxtFax.Text = string.Empty;
                HotxtEmailWeb.Text = string.Empty;
                hotxtRemarks.Text = string.Empty;
                hotxtContactName.Text = string.Empty;
                hdnHomeOwnerID.Value = string.Empty;
                hdnHOName.Value = string.Empty;
                hdnHONameupdate.Value = string.Empty;
            }
        }
    }

    private void FillJobStatus()
    {
        objJob.ConnConfig = Session["config"].ToString();
        DataSet _ds = objBL_Job.GetJobStatus(objJob);
        if (_ds.Tables[0].Rows.Count > 0)
        {
            ddlJobStatus.Items.Clear();
            ddlJobStatus.Items.Add(new ListItem("Select Status"));
            ddlJobStatus.AppendDataBoundItems = true;
            ddlJobStatus.DataSource = _ds;
            ddlJobStatus.DataValueField = "ID";
            ddlJobStatus.DataTextField = "Status";
            ddlJobStatus.DataBind();


        }
        else
        {
            ddlJobStatus.Items.Clear();
            ddlJobStatus.Items.Add(new ListItem("No data found", "Select Status"));
        }
    }

    private void FillAddress()
    {
        if (!string.IsNullOrEmpty(hdnLocID.Value))
        {
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
            objPropUser.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_User.getLocationByID(objPropUser);

            if (ds.Tables[0].Rows.Count > 0)
            {
                #region Salesperson1  and Salesperson2 
                string Terr = ds.Tables[0].Rows[0]["Terr"].ToString();
                string Terr2 = ds.Tables[0].Rows[0]["Terr2"].ToString();
                string ddlTyp = ds.Tables[0].Rows[0]["Type"].ToString();
                string drpval = ds.Tables[0].Rows[0]["Route"].ToString();
                if (ddlTerr.Items.FindByValue(Terr) != null)
                {
                    ddlTerr.SelectedValue = Terr;
                }
                else
                {
                    ddlTerr.SelectedValue = "";
                }
                if (ddlTerr2.Items.FindByValue(Terr2) != null)
                {
                    ddlTerr2.SelectedValue = Terr2;
                }
                else
                {
                    ddlTerr2.SelectedValue = "";
                }
                if (ddlType.Items.FindByValue(ddlTyp) != null)
                {
                    ddlType.SelectedValue = ddlTyp;
                }
                else
                {
                    ddlType.SelectedValue = "";
                }
                if (ddlRoute.Items.FindByValue(drpval) != null)
                {
                    ddlRoute.SelectedValue = drpval;
                }
                else
                {
                    ddlRoute.SelectedValue = "";
                }

                #endregion
                txtLocation.Text = ds.Tables[0].Rows[0]["tag"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["LocAddress"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["Loccity"].ToString() + ", " + ds.Tables[0].Rows[0]["LocState"].ToString() + ", " + ds.Tables[0].Rows[0]["LocZip"].ToString();
                txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
                hdnLocRolID.Value = ds.Tables[0].Rows[0]["Rol"].ToString();
                if (string.IsNullOrEmpty(hdnCustID.Value))
                {
                    txtCustomer.Text = ds.Tables[0].Rows[0]["custname"].ToString();
                    hdnCustID.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                }
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["stax"].ToString()))
                {
                    txtSalesTax.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
                    hdnGlobal_SalesTax.Value = Convert.ToString(ds.Tables[0].Rows[0]["Rate"]);
                    lblSalesTax.Text = ds.Tables[0].Rows[0]["stax"].ToString() + " - " + ds.Tables[0].Rows[0]["Rate"].ToString() + " %";
                }
                try
                {
                    ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["defaultterms"].ToString();
                }
                catch
                {
                }

                hdnGlobal_Terms.Value = ds.Tables[0].Rows[0]["defaultterms"].ToString();
                //Set Hyperlink  For Loc / Customer
                if (hdnLocID.Value != "0")
                {
                    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;
                    GetDataEquip();
                }
                else
                    lnkLocationID.NavigateUrl = "";

                if (hdnCustID.Value != "0")
                    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustID.Value;
                else
                    lnkCustomerID.NavigateUrl = "";

                if (ds.Tables[0].Columns.Contains("BillRate"))
                {
                    if (ds.Tables[0].Rows[0]["BillRate"].ToString() == null)
                        txtBillRate.Text = string.Empty;
                    else
                        txtBillRate.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                    if (ds.Tables[0].Rows[0]["RateOT"].ToString() == null)
                        txtOt.Text = string.Empty;
                    else
                        txtOt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                    if (ds.Tables[0].Rows[0]["RateNT"].ToString() == null)
                        txtNt.Text = string.Empty;
                    else
                        txtNt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));

                    if (ds.Tables[0].Rows[0]["RateDT"].ToString() == null)
                        txtDt.Text = string.Empty;
                    else
                        txtDt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                    if (ds.Tables[0].Rows[0]["RateMileage"].ToString() == null)
                        txtMileage.Text = string.Empty;
                    else
                        txtMileage.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));
                    if (ds.Tables[0].Rows[0]["RateTravel"].ToString() == null)
                        txtTravel.Text = string.Empty;
                    else
                        txtTravel.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));
                    if (Request.QueryString["locid"] != null)
                    {
                        if (txtREPdesc.Text.Trim() == string.Empty)
                            txtREPdesc.Text = txtLocation.Text = ds.Tables[0].Rows[0]["tag"].ToString();
                    }
                    // Fill GC and Homeowner info 
                    if (!string.IsNullOrEmpty(hdnLocID.Value))
                        GetGC_HOInfo(hdnLocID.Value);
                }
            }
            else
            {
                txtAddress.Text = "0.00";
                txtBillRate.Text = "0.00";
                txtOt.Text = "0.00";
                txtNt.Text = "0.00";
                txtDt.Text = "0.00";
                txtMileage.Text = "0.00";
                txtTravel.Text = "0.00";
                if (ddlRoute.SelectedValue == "")
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyerrr", "noty({text: 'Please select an active " + lblDefaultWorker.InnerText + " for this location',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }

        }
    }

    private void FillJobType()
    {
        try
        {
            DataSet _dsJob = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            _dsJob = objBL_Job.GetAllJobType(objJob);
            if (_dsJob.Tables[0].Rows.Count > 0)
            {
                ddlJobType.Items.Add(new ListItem("Select Type", "Select Type"));
                ddlJobType.AppendDataBoundItems = true;
                ddlJobType.DataSource = _dsJob;
                ddlJobType.DataValueField = "ID";
                ddlJobType.DataTextField = "Type";
                ddlJobType.DataBind();
            }
            else
            {
                ddlJobType.Items.Add(new ListItem("No data found", "Select Type"));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillProjectsTemplate()
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        int Job = 0;

        if (Request.QueryString["uid"] != null)
        {
            int.TryParse(Request.QueryString["uid"], out Job);
        }

        ds = objBL_Customer.getJobProjectTemp(objProp_Customer, Job);
        if (ds.Tables[0].Rows.Count > 0)
        {
            rfvTemplateType.InitialValue = "Select Template";
            if (Request.QueryString["uid"] == null)
            {
                DataRow dr = ds.Tables[0].Select("Type = 0").FirstOrDefault();
                if (dr != null)
                {
                    ds.Tables[0].Rows.Remove(dr);
                }
            }
            ddlTemplate.DataSource = ds.Tables[0];
            ddlTemplate.DataTextField = "Fdesc";
            ddlTemplate.DataValueField = "id";
            ddlTemplate.DataBind();
            ddlTemplate.Items.Insert(0, new ListItem("Select Template"));
        }
        else
        {
            rfvTemplateType.InitialValue = "No data found";
            ddlTemplate.Items.Insert(0, new ListItem("No data found", "0"));
        }
    }

    #region Attribute
    private DataTable GetTeamItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("JobID", typeof(int));
        dt.Columns.Add("Title", typeof(string));
        dt.Columns.Add("UserID", typeof(string));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Mobile", typeof(string));

        try
        {
            string strItems = hdnItemTeamJSON.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                //if(objEstimateItemData.Count > 0)
                //{
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["txtTitle"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Line"] = i;
                    if (Request.QueryString["uid"] != null)
                    {
                        dr["JobID"] = Convert.ToInt32(Request.QueryString["uid"]);
                    }
                    else
                    {
                        dr["JobID"] = 0;
                    }
                    dr["Title"] = dict["txtTitle"].ToString().Trim();
                    dr["UserID"] = dict["txtUserID"].ToString().Trim();
                    dr["FirstName"] = dict["txtFirstName"].ToString().Trim();
                    dr["LastName"] = dict["txtLastName"].ToString().Trim();
                    dr["Email"] = dict["txtEmail"].ToString().Trim();
                    dr["Mobile"] = dict["txtMobile"].ToString().Trim();
                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    private DataTable GetTeamItemsForProject()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("JobID", typeof(int));
        dt.Columns.Add("Title", typeof(string));
        dt.Columns.Add("UserID", typeof(string));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Mobile", typeof(string));
        int i = 0;
        try
        {
            foreach (GridDataItem item in gvTeamItems.Items)
            {

                Label lblIndex = (Label)item.FindControl("lblIndex");
                TextBox txtTitle = (TextBox)item.FindControl("txtTitle");
                TextBox txtUserID = (TextBox)item.FindControl("txtUserID");
                HiddenField hdnUserID = (HiddenField)item.FindControl("hdnUserID");
                TextBox txtFirstName = (TextBox)item.FindControl("txtFirstName");
                TextBox txtLastName = (TextBox)item.FindControl("txtLastName");
                TextBox txtEmail = (TextBox)item.FindControl("txtEmail");
                TextBox txtMobile = (TextBox)item.FindControl("txtMobile");
                if (txtTitle.Text.ToString().Trim() == string.Empty)
                {
                    return dt;
                }
                i++;
                DataRow dr = dt.NewRow();
                dr["Line"] = i;
                if (Request.QueryString["uid"] != null)
                {
                    dr["JobID"] = Convert.ToInt32(Request.QueryString["uid"]);
                }
                else
                {
                    dr["JobID"] = 0;
                }
                dr["Title"] = txtTitle.Text.ToString().Trim();
                dr["UserID"] = txtUserID.Text.ToString().Trim();
                dr["FirstName"] = txtFirstName.Text.ToString().Trim();
                dr["LastName"] = txtLastName.Text.ToString().Trim();
                dr["Email"] = txtEmail.Text.ToString().Trim();
                dr["Mobile"] = txtMobile.Text.ToString().Trim();
                dt.Rows.Add(dr);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    private void FillState()
    {
        try
        {
            DataSet dsState = new DataSet();

            objState.ConnConfig = Session["config"].ToString();
            dsState = objBL_Bank.GetStates(objState);
            if (dsState.Tables[0].Rows.Count > 0)
            {
                ddlState.Items.Add(new ListItem("Select State"));
                ddlState.AppendDataBoundItems = true;

                ddlState.DataSource = dsState;
                ddlState.DataValueField = "Name";
                ddlState.DataTextField = "fDesc";
                ddlState.DataBind();


                hotddlstate.Items.Add(new ListItem("Select State"));
                hotddlstate.AppendDataBoundItems = true;

                hotddlstate.DataSource = dsState;
                hotddlstate.DataValueField = "Name";
                hotddlstate.DataTextField = "fDesc";
                hotddlstate.DataBind();
            }
            else
            {
                hotddlstate.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void CreateTeamTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Title", typeof(string));
        dt.Columns.Add("UserID", typeof(string));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Mobile", typeof(string));

        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dt.Rows.Add(dr1);

        ViewState["TeamItems"] = dt;
        gvTeamItems.DataSource = dt;
        gvTeamItems.DataBind();
    }


    private void FillContractType(string LocType, string EditSType, int department = -1, int route = -1)
    {
        try
        {
            DataSet _dsContract = new DataSet();

            objPropUser.ConnConfig = Session["config"].ToString();

            _dsContract = new BusinessLayer.Programs.BL_ServiceType().GetActiveServiceTypeContract(objPropUser.ConnConfig, LocType, EditSType, department, route);

            if (_dsContract.Tables[0].Rows.Count > 0)
            {

                ddlContractType1.Items.Clear();

                ddlContractType1.DataSource = _dsContract.Tables[0];
                ddlContractType1.DataTextField = "type";
                ddlContractType1.DataValueField = "type";
                ddlContractType1.SelectedIndex = -1;
                ddlContractType1.DataBind();
                ddlContractType1.Items.Insert(0, new ListItem(":: Select ::", "0"));
            }
            else
            {
                ddlContractType1.Items.Clear();
                ddlContractType1.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void BindEquip()
    {
        if (hdnloadequipment.Value == "1")
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            if (string.IsNullOrEmpty(hdnLocID.Value))
            {
                objPropUser.LocID = 0;
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
            }

            DataSet ds = objBL_User.getElevByLoc(objPropUser);

            rtEquips.DataSource = ds.Tables[0];
            rtEquips.DataBind();
        }
    }
    #endregion

    #region Budget
    private DataTable GetOpenCalls(string sort)
    {

        DataSet ds = new DataSet();
        objMapData = new MapData();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);
        if (sort == string.Empty)
            objMapData.OrderBy = "edate desc";
        else
            objMapData.OrderBy = sort;
        objMapData.Department = -1;
        objMapData.IsList = 1;

        ds = new BL_Tickets().getCallHistory(objMapData);
        return ds.Tables[0];
    }

    private void BindTicketList(String OrderBy, Int32 PageIndex, bool isrebind = true)
    {
        gvTickets.Visible = true;
        DataSet ds = new DataSet();
        objMapData = new MapData();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);
        objMapData.PageSize = Convert.ToInt32(10000);
        objMapData.PageIndex = PageIndex;

        if (OrderBy == string.Empty)
            objMapData.OrderBy = "edate desc";
        else
            objMapData.OrderBy = OrderBy;

        ViewState["TicketOrderBy"] = objMapData.OrderBy;

        string StartDate = "NA"; string EndDate = "NA";
        if (txtfromDate.Text != "" && txtToDate.Text != "")
        {
            StartDate = txtfromDate.Text;
            EndDate = txtToDate.Text;
        }
        ds = objBL_MapData.GetProjectTickets(objMapData, StartDate, EndDate);

        DataTable FinalDt = new DataTable();
        if (ddlComp.SelectedValue != "")
        {
            if (ddlComp.SelectedValue == "2")
            {
                var rows = ds.Tables[0].AsEnumerable().Where(r => r.Field<int>("Comp") == 2);
                FinalDt = rows.Any() ? rows.CopyToDataTable() : ds.Tables[0].Clone();
            }
            else
            {
                var rows = ds.Tables[0].AsEnumerable().Where(r => r.Field<int>("Comp") != 2);
                FinalDt = rows.Any() ? rows.CopyToDataTable() : ds.Tables[0].Clone();
            }
        }
        else
            FinalDt = ds.Tables[0];
        gvTickets.DataSource = FinalDt;
        if (isrebind) gvTickets.Rebind();

        //if the user does not have permission to view project finance then  hide the Labor Expense and Expenses column.

        if (hdnFinancePermission.Value == "N")
        {
            foreach (GridColumn col in gvTickets.Columns)
            {
                if (col.HeaderText == "Labor Expenses" || col.HeaderText == "Expenses")
                {
                    col.Visible = false;
                }
            }
        }
    }

    private void GetInvoices()
    {
        if (hdnLoadAP_ARInvoices.Value == "1")
        {
            DataSet ds = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            #region Company Check
            objProp_Contracts.UserID = Convert.ToString(Session["UserID"]);
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objProp_Contracts.EN = 1;
            }
            else
            {
                objProp_Contracts.EN = 0;
            }
            #endregion
            objProp_Contracts.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());
            if (ddlInvoiceStatus.SelectedValue != "-1")
            {
                objProp_Contracts.SearchBy = "i.Status";
                objProp_Contracts.SearchValue = ddlInvoiceStatus.SelectedValue;
            }
            if (txtfromDate.Text != "" && txtToDate.Text != "")
            {
                objProp_Contracts.StartDate = Convert.ToDateTime(txtfromDate.Text);
                objProp_Contracts.EndDate = Convert.ToDateTime(txtToDate.Text);
            }
            ds = objBL_Contracts.GetProjectARInvoices(objProp_Contracts);
            gvInvoice.DataSource = ds.Tables[0];
            ViewState["ARInvoices"] = ds.Tables[0];
            gvInvoice.Rebind();
            calculateInvoice(ds.Tables[0]);
        }
    }

    private void GetAPInvoices()
    {
        if (hdnLoadAP_ARInvoices.Value == "1")
        {
            DataSet ds = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());

            if (txtfromDate.Text != "" && txtToDate.Text != "")
            {
                objProp_Contracts.StartDate = Convert.ToDateTime(txtfromDate.Text);
                objProp_Contracts.EndDate = Convert.ToDateTime(txtToDate.Text);
            }

            ds = objBL_Contracts.GetAPInvoices(objProp_Contracts);
            gvAPInvoices.DataSource = ds.Tables[0];
            gvAPInvoices.DataBind();
        }
    }

    private void GetJobCost()
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());

        ds = objBL_Contracts.GetJobCostItems(objProp_Contracts);
        gvJOBC.DataSource = ds.Tables[0];
        gvJOBC.DataBind();
    }

    private void CalculateBalance(DataTable dt)
    {

        if (dt.Rows.Count > 0)
        {
            #region Commented Code
            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (dr["tottime"].ToString() != string.Empty)
            //    {
            //        dblBalTotal += Convert.ToDouble(dr["tottime"].ToString());
            //    }
            //    if (dr["expenses"].ToString() != string.Empty)
            //    {
            //        dblExpenses += Convert.ToDouble(dr["expenses"].ToString());
            //    }
            //    if (dr["est"].ToString() != string.Empty)
            //    {
            //        dblEST += Convert.ToDouble(dr["est"].ToString());
            //    }
            //    if (dr["laborexp"].ToString() != string.Empty)
            //    {
            //        dblLabExpenses += Convert.ToDouble(dr["laborexp"].ToString());
            //    }
            //    if (dr["reg"].ToString() != string.Empty)
            //    {
            //        dblRT += Convert.ToDouble(dr["reg"].ToString());
            //    }
            //    if (dr["ot"].ToString() != string.Empty)
            //    {
            //        dblOT += Convert.ToDouble(dr["ot"].ToString());
            //    }
            //    if (dr["nt"].ToString() != string.Empty)
            //    {
            //        dblNT += Convert.ToDouble(dr["nt"].ToString());
            //    }
            //    if (dr["dt"].ToString() != string.Empty)
            //    {
            //        dblDT += Convert.ToDouble(dr["dt"].ToString());
            //    }
            //    if (dr["tt"].ToString() != string.Empty)
            //    {
            //        dblTT += Convert.ToDouble(dr["tt"].ToString());
            //    }
            //}
            #endregion  
            GridFooterItem footeritem = (GridFooterItem)gvTickets.MasterTableView.GetItems(GridItemType.Footer)[0];
            Label lblTotalFooter = (Label)footeritem.FindControl("lblTotalFooter");
            Label lblExpenseFooter = (Label)footeritem.FindControl("lblExpenseFooter");
            Label lblESTFooter = (Label)footeritem.FindControl("lblESTFooter");
            Label lblLabExpenseFooter = (Label)footeritem.FindControl("lblLabExpenseFooter");
            Label lblRTFooter = (Label)footeritem.FindControl("lblRTFooter");
            Label lblOTFooter = (Label)footeritem.FindControl("lblOTFooter");
            Label lblNTFooter = (Label)footeritem.FindControl("lblNTFooter");
            Label lblDTFooter = (Label)footeritem.FindControl("lblDTFooter");
            Label lblTTFooter = (Label)footeritem.FindControl("lblTTFooter");

            //lblTotalFooter.Text = string.Format("{0:n}", dblBalTotal);
            //lblExpenseFooter.Text = string.Format("{0:c}", dblExpenses);
            //lblESTFooter.Text = string.Format("{0:n}", dblEST);
            //lblLabExpenseFooter.Text = string.Format("{0:c}", dblLabExpenses);
            //lblRTFooter.Text = string.Format("{0:n}", dblRT);
            //lblOTFooter.Text = string.Format("{0:n}", dblOT);
            //lblNTFooter.Text = string.Format("{0:n}", dblNT);
            //lblDTFooter.Text = string.Format("{0:n}", dblDT);
            //lblTTFooter.Text = string.Format("{0:n}", dblTT);

            if (dt.Rows[0]["tottime"] != DBNull.Value)
            {
                lblTotalFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["tottime"]));
            }
            if (dt.Rows[0]["expenses"] != DBNull.Value)
            {
                lblExpenseFooter.Text = string.Format("{0:c}", Convert.ToDouble(dt.Rows[0]["expenses"]));
            }
            if (dt.Rows[0]["EST"] != DBNull.Value)
            {
                lblESTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["EST"]));
            }
            if (dt.Rows[0]["laborexp"] != DBNull.Value)
            {
                lblLabExpenseFooter.Text = string.Format("{0:c}", Convert.ToDouble(dt.Rows[0]["laborexp"]));
            }
            if (dt.Rows[0]["Reg"] != DBNull.Value)
            {
                lblRTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["Reg"]));
            }
            if (dt.Rows[0]["OT"] != DBNull.Value)
            {
                lblOTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["OT"]));
            }
            if (dt.Rows[0]["NT"] != DBNull.Value)
            {
                lblNTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["NT"]));
            }
            if (dt.Rows[0]["DT"] != DBNull.Value)
            {
                lblDTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["DT"]));
            }
            if (dt.Rows[0]["TT"] != DBNull.Value)
            {
                lblTTFooter.Text = string.Format("{0:n}", Convert.ToDouble(dt.Rows[0]["TT"]));
            }
        }
    }

    private void CalculateItems(DataTable dt)
    {
        double dblActual = 0;
        double dblBudget = 0;
        double dblPercent = 0;

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["actual"].ToString() != string.Empty)
                {
                    dblActual += Convert.ToDouble(dr["actual"].ToString());
                }
                if (dr["budget"].ToString() != string.Empty)
                {
                    dblBudget += Convert.ToDouble(dr["budget"].ToString());
                }
                if (dr["Percent"].ToString() != string.Empty)
                {
                    dblPercent += Convert.ToDouble(dr["Percent"].ToString());
                }
            }
            GridFooterItem footeritem = (GridFooterItem)gvBOM.MasterTableView.GetItems(GridItemType.Footer)[0];

            TextBox txtTActual = (TextBox)footeritem.FindControl("txtTActual");
            TextBox txtTBudget = (TextBox)footeritem.FindControl("txtTBudget");
            TextBox txtTPercent = (TextBox)footeritem.FindControl("txtTPercent");

            txtTActual.Text = string.Format("{0:n}", dblActual);
            txtTBudget.Text = string.Format("{0:n}", dblBudget);
            txtTPercent.Text = string.Format("{0:n}", dblPercent);
        }
    }

    private void calculateInvoice(DataTable dt)
    {
        //todo
        lblCountInvoice.Text = dt.Rows.Count.ToString() + " Record(s) Found.";

        if (dt.Rows.Count > 0)
        {
            GridFooterItem footeritem = (GridFooterItem)gvInvoice.MasterTableView.GetItems(GridItemType.Footer)[0];

            Label lblTotalPretaxAmt = (Label)footeritem.FindControl("lblTotalPretaxAmt");
            Label lblTotalSalesTax = (Label)footeritem.FindControl("lblTotalSalesTax");
            Label lblTotalGSTAmount = (Label)footeritem.FindControl("lblTotalGSTAmount");
            Label lblTotalInvoice = (Label)footeritem.FindControl("InvTotalInvoice");
            Label lblTotalDue = (Label)footeritem.FindControl("InvTotalDue");

            double TotalPretaxAmt = 0;
            double TotalGSTAmount = 0;
            double TotalSalesTax = 0;
            double TotalInvoice = 0;
            double TotalDue = 0;

            double PretaxAmt = 0;
            double SalesTax = 0;
            double GSTTax = 0;
            double Invoice = 0;
            double Due = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["amount"] != DBNull.Value && dr["amount"].ToString() != "")
                {
                    PretaxAmt = Convert.ToDouble(dr["amount"]);
                }

                if (dr["stax"] != DBNull.Value && dr["stax"].ToString() != "")
                {
                    SalesTax = Convert.ToDouble(dr["stax"]);
                }

                if (dr["GSTAmount"] != DBNull.Value && dr["GSTAmount"].ToString() != "")
                {
                    GSTTax = Convert.ToDouble(dr["GSTAmount"]);
                }

                if (dr["total"] != DBNull.Value && dr["total"].ToString() != "")
                {
                    Invoice = Convert.ToDouble(dr["total"]);
                }

                Due = Convert.ToDouble(dr["balance"]);

                TotalPretaxAmt += PretaxAmt;
                TotalSalesTax += SalesTax;
                TotalGSTAmount += GSTTax;
                TotalInvoice += Invoice;
                TotalDue += Due;
            }

            lblTotalPretaxAmt.Text = string.Format("{0:c}", TotalPretaxAmt);
            lblTotalGSTAmount.Text = string.Format("{0:c}", TotalGSTAmount);
            lblTotalSalesTax.Text = string.Format("{0:c}", TotalSalesTax);
            lblTotalInvoice.Text = string.Format("{0:c}", TotalInvoice);
            lblTotalDue.Text = string.Format("{0:c}", TotalDue);
        }
    }
    #endregion

    #region Finance

    #region Budget
    private void BindBudget(int pageIndex)
    {
        if (hdnLoadgvBudget.Value == "1")
        {
            try
            {
                #region Budget
                DataSet ds = new DataSet();

                objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objJob.PageSize = Convert.ToInt32(PageSize);
                objJob.PageIndex = pageIndex;

                if (Request.QueryString["uid"] != null)
                {
                    if (Request.QueryString["s"] != null && Request.QueryString["e"] != null)
                    {
                        objJob.StartDate = Convert.ToDateTime(System.Web.HttpUtility.UrlDecode(Request.QueryString["s"].ToString()));
                        objJob.EndDate = Convert.ToDateTime(System.Web.HttpUtility.UrlDecode(Request.QueryString["e"].ToString()));
                    }
                    else
                    if (txtfromDate.Text != "" && txtToDate.Text != "")
                    {
                        objJob.StartDate = Convert.ToDateTime(txtfromDate.Text);
                        objJob.EndDate = Convert.ToDateTime(txtToDate.Text);
                    }
                }



                ds = objBL_Job.GetJobCostByJob(objJob);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ViewState["Budget"] = ds.Tables[0];

                    gvBudget.DataSource = ds.Tables[0];
                    gvBudget.DataBind();

                    Int32 TotalRecord = Convert.ToInt32(ds.Tables[2].Rows[0][0]);

                }

                #endregion

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    private void BindExpense()
    {
        try
        {
            #region Expense
            DataSet dsExp = new DataSet();

            objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objJob.PageSize = Convert.ToInt32(1000);
            objJob.PageIndex = 0;

            if (Request.QueryString["s"] != null && Request.QueryString["e"] != null)
            {
                dsExp = objBL_Job.GetExpenseJobCostByDate(objJob);
            }
            else
            {
                dsExp = objBL_Job.GetExpenseJobCost(objJob);
            }

            if (dsExp.Tables[0].Rows.Count > 0)
            {
                gvExpenses.DataSource = dsExp.Tables[0];
                gvExpenses.DataBind();

                GridFooterItem footeritem = (GridFooterItem)gvExpenses.MasterTableView.GetItems(GridItemType.Footer)[0];

                Label lblFooterMatAct = footeritem.FindControl("lblFooterMatAct") as Label;
                Label lblFooterMatBgt = footeritem.FindControl("lblFooterMatBgt") as Label;
                Label lblFooterMatMod = footeritem.FindControl("lblFooterMatMod") as Label;
                Label lblFooterMatDiff = footeritem.FindControl("lblFooterMatDiff") as Label;
                Label lblFooterHours = footeritem.FindControl("lblFooterHours") as Label;
                Label lblFooterHoursBgt = footeritem.FindControl("lblFooterHoursBgt") as Label;
                Label lblFooterLabAct = footeritem.FindControl("lblFooterLabAct") as Label;
                Label lblFooterLabBgt = footeritem.FindControl("lblFooterLabBgt") as Label;
                Label lblFooterLabMod = footeritem.FindControl("lblFooterLabMod") as Label;
                Label lblFooterLabDiff = footeritem.FindControl("lblFooterLabDiff") as Label;

                lblFooterMatAct.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(MatAct)", string.Empty)));
                lblFooterMatBgt.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(MatBgt)", string.Empty)));
                lblFooterMatMod.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(MatMod)", string.Empty)));
                lblFooterMatDiff.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(MatDiff)", string.Empty)));
                lblFooterHours.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(HourAct)", string.Empty)));
                lblFooterHoursBgt.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(HourBgt)", string.Empty)));
                lblFooterLabAct.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(LaborAct)", string.Empty)));
                lblFooterLabBgt.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(LaborBgt)", string.Empty)));
                lblFooterLabMod.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(LaborMod)", string.Empty)));
                lblFooterLabDiff.Text = string.Format("{0:c}", Convert.ToDouble(dsExp.Tables[0].Compute("SUM(LabDiff)", string.Empty)));

                Int32 TotalRecord = Convert.ToInt32(dsExp.Tables[1].Rows[0][0]);
                //this.PopulatePager(TotalRecord, pageIndex, rptExpensesPager);
            }

            #endregion
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        //gvBudget.RowDataBound();
    }

    #endregion

    private void GetJobTemplate(int tid)
    {
        try
        {
            objJob.ConnConfig = Session["config"].ToString();

            objJob.ID = tid;

            DataSet ds = objBL_Job.GetJobTFinanceByID(objJob);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow _dr = ds.Tables[0].Rows[0];

                hdnInvServiceID.Value = _dr["InvServ"].ToString();

                txtInvService.Text = _dr["InvServiceName"].ToString();

                hdnPrevilWageID.Value = _dr["Wage"].ToString();

                txtPrevilWage.Text = _dr["WageName"].ToString();

                uc_InterestGL._txtGLAcct.Text = _dr["GLName"].ToString();

                uc_InterestGL._hdnAcctID.Value = _dr["GLInt"].ToString();

                uc_InvExpGL._txtGLAcct.Text = _dr["InvExpName"].ToString();

                uc_InvExpGL._hdnAcctID.Value = _dr["InvExp"].ToString();



                if (!string.IsNullOrEmpty(_dr["CType"].ToString()))
                {
                    ddlContractType1.SelectedValue = _dr["CType"].ToString();
                }
                if (!string.IsNullOrEmpty(_dr["Post"].ToString()))
                {
                    ddlPostingMethod.SelectedValue = _dr["Post"].ToString();
                }
                hdnUnrecognizedRevenue.Value = _dr["UnrecognizedRevenue"].ToString();
                if (!String.IsNullOrEmpty(hdnUnrecognizedRevenue.Value))
                    AssignBillingCodeInGrid(hdnUnrecognizedRevenue.Value);
                txtUnrecognizedRevenue.Text = _dr["UnrecognizedRevenueName"].ToString();
                hdnUnrecognizedExpense.Value = _dr["UnrecognizedExpense"].ToString();
                txtUnrecognizedExpense.Text = _dr["UnrecognizedExpenseName"].ToString();
                hdnRetainageReceivable.Value = _dr["RetainageReceivable"].ToString();
                txtRetainageReceivable.Text = _dr["RetainageReceivableName"].ToString();
                if (_dr["Charge"].ToString().Equals("1"))
                {
                    chkChargeable.Checked = true;
                }
                if (_dr["fInt"].ToString().Equals("1"))
                {
                    chkChargeInt.Checked = true;
                }
                if (_dr["JobClose"].ToString().Equals("1"))
                {
                    chkInvoicing.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillPosting()
    {
        try
        {
            DataSet _dsPost = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            _dsPost = objBL_Job.GetPosting(objJob);
            if (_dsPost.Tables[0].Rows.Count > 0)
            {
                ddlPostingMethod.DataSource = _dsPost;
                ddlPostingMethod.DataBind();
                ddlPostingMethod.DataValueField = "ID";
                ddlPostingMethod.DataTextField = "Post";
                ddlPostingMethod.DataBind();
            }
            else
            {
                ddlPostingMethod.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DisableControl()       // Finance > General
    {
        ClearTempControl();

        uc_InvExpGL._txtGLAcct.Enabled = false;
        uc_InterestGL._txtGLAcct.Enabled = false;
        txtInvService.Enabled = false;
        txtPrevilWage.Enabled = false;
        //ddlPostingMethod.Enabled = false;
        ddlContractType1.Enabled = false;
        chkChargeInt.Enabled = false;
        chkInvoicing.Enabled = false;
        chkChargeable.Enabled = false;
    }

    private void ClearTempControl()     //clear project template field's controls. Finance > General
    {
        uc_InvExpGL._txtGLAcct.Text = "";
        uc_InterestGL._txtGLAcct.Text = "";
        txtInvService.Text = "";
        txtPrevilWage.Text = "";
        ddlPostingMethod.SelectedIndex = 0;
        ddlContractType1.SelectedIndex = 0;
        chkChargeInt.Checked = false;
        chkInvoicing.Checked = false;
        chkChargeable.Checked = false;
    }

    private void EnableControl()       // Finance > General
    {
        ClearTempControl();

        uc_InvExpGL._txtGLAcct.Enabled = true;
        uc_InterestGL._txtGLAcct.Enabled = true;
        txtInvService.Enabled = true;
        txtPrevilWage.Enabled = true;
        //ddlpostingMethod.Enabled = true;
        ddlContractType1.Enabled = true;
        chkChargeInt.Enabled = true;
        chkInvoicing.Enabled = true;
        chkChargeable.Enabled = true;
    }
    #endregion

    #endregion

    #region Custom Field

    protected override void OnInit(EventArgs e)       // comment 8.16.16
    {
        base.OnInit(e);
        // dt = (DataTable)ViewState["CustomFields"];
    }

    protected override void LoadViewState(object savedState)
    {
        TrackViewState();
        base.LoadViewState(savedState);

        if (!ddlTemplate.SelectedValue.Equals("0") && !ddlTemplate.SelectedValue.Equals(""))      // comment 8.16.16
        {
            objJob.ConnConfig = Session["config"].ToString();
            objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);
            if (Request.QueryString["uid"] != null)
            {
                objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
            }
            else
            {
                objJob.Job = 0;
            }

            DataSet ds = new DataSet();
            ds = objBL_Job.GetProjectTemplateCustomFields(objJob);
            DisplayCustomByTab(ds.Tables[0], ds.Tables[1], objJob.ID, objJob.Job);
        }
    }

    private void CreateCustomTable()
    {
        dtCustomField = new DataTable();
        dtCustomField.Columns.Add("ID", typeof(int));
        dtCustomField.Columns.Add("tblTabID", typeof(int));
        dtCustomField.Columns.Add("Label", typeof(string));
        dtCustomField.Columns.Add("Line", typeof(Int16));
        dtCustomField.Columns.Add("Value", typeof(string));
        dtCustomField.Columns.Add("Format", typeof(Int16));
        dtCustomField.Columns.Add("ControlID", typeof(string));
        dtCustomField.Columns.Add("UpdatedDate", typeof(DateTime));
        dtCustomField.Columns.Add("Username", typeof(string));
        dtCustomField.Columns.Add("IsAlert", typeof(bool));
        dtCustomField.Columns.Add("IsTask", typeof(bool));
        dtCustomField.Columns.Add("TeamMember", typeof(string));
        dtCustomField.Columns.Add("TeamMemberDisplay", typeof(string));
        dtCustomField.Columns.Add("UserRole", typeof(string));
        dtCustomField.Columns.Add("UserRoleDisplay", typeof(string));
        dtCustomField.Columns.Add("IsValueChanged", typeof(bool));
        dtCustomField.Columns.Add("OldValue", typeof(string));
    }

    //private string DisplayCustomField(DataTable dtCust, DataTable dtValue, PlaceHolder tbContainer, int isEquipmentTab = 0)
    //{
    //    try
    //    {
    //        StringBuilder html = new StringBuilder();
    //        string varControl = "";
    //        DataTable dtItem = new DataTable();
    //        DataTable dtc = dtCust;
    //        DataTable dtCustomValue = dtValue;

    //        int rowCount = dtc.Rows.Count;
    //        int rowcount1 = rowCount / 2;
    //        int count = 0;

    //        if (isEquipmentTab == 1)
    //            html.Append("   <div class='col-md-4 col-lg-4'> ");
    //        else
    //        {
    //            if (ViewState["TabId"].ToString().Equals("1"))
    //            {
    //                //html.Append("   <div class='col-md-6 col-lg-6'> ");
    //                html.Append("   <div class='form-section-row'> ");
    //            }
    //            else
    //            {
    //                //html.Append("   <div class='col-md-6 col-lg-6'> ");
    //                html.Append("   <div class='form-section-row'> ");
    //            }
    //        }
    //        int i = 0;
    //        var rTake = Convert.ToDecimal(Convert.ToDecimal(rowCount) / Convert.ToDecimal(3));
    //        Int32 _take = Convert.ToInt32(Math.Ceiling(rTake));
    //        Int32 _skip = 0;
    //        for (int j = 0; j < 3; j++)
    //        {
    //            html.Append(" <div class='form-section3'>      \n");

    //            DataTable dtccc = dtc.AsEnumerable().Skip(_skip).Take(_take).CopyToDataTable();
    //            _skip = _skip + _take;


    //            foreach (DataRow drc in dtccc.Rows)
    //            {
    //                #region Commented Code
    //                //if (rowcount1.Equals(count))
    //                //{
    //                //    html.Append("   </div>                          \n");
    //                //    if (isEquipmentTab == 1)
    //                //        html.Append("   <div class='col-md-4 col-lg-4'> \n");
    //                //    else
    //                //    {
    //                //        if (ViewState["TabId"].ToString().Equals("1"))
    //                //        {
    //                //            html.Append("   <div class='col-md-4 col-lg-4'> \n");
    //                //        }
    //                //        else
    //                //        {
    //                //            html.Append("   <div class='col-md-6 col-lg-6'> \n");
    //                //        }
    //                //    }
    //                //}
    //                #endregion  

    //                string ctrlValue = drc["Value"].ToString();
    //                string ctrlName = drc["Line"].ToString();

    //                #region Code Comment
    //                //html.Append(" <div class='form-group'>      \n");
    //                //html.Append("   <div class='form-col'>      \n");
    //                //html.Append("      <div class='fc-label'>   \n");
    //                //html.Append(drc["Label"].ToString());
    //                //html.Append("      </div> \n");
    //                //html.Append("      <div class='fc-input'>   \n");
    //                #endregion


    //                html.Append("   <div class='input-field col s12' style='margin-top:7px !important;'>      \n");

    //                if (Convert.ToInt16(drc["Format"]) == 1 || Convert.ToInt16(drc["Format"]) == 2 || Convert.ToInt16(drc["Format"]) == 3)
    //                {
    //                    html.Append("   <div class='row'>      \n");
    //                    html.Append(" <label for='txt" + ctrlName + "'>" + Convert.ToString(drc["Label"]) + "</label>   \n");
    //                }
    //                else if (Convert.ToInt16(drc["Format"]) == 4)
    //                {
    //                    html.Append("   <div class='row'>      \n");
    //                    html.Append(" <label class='drpdwn-label'>" + Convert.ToString(drc["Label"]) + "</label>   \n");
    //                }
    //                else if (Convert.ToInt16(drc["Format"]) == 5)
    //                {
    //                    html.Append("  <div class='checkrow'>   \n");
    //                    html.Append("    <span class='tro'>   \n");
    //                }

    //                StringWriter sw = new StringWriter(html);
    //                HtmlTextWriter writer = new HtmlTextWriter(sw);

    //                CustomTextBox txt = new CustomTextBox();

    //                #region append with control

    //                switch (Convert.ToInt16(drc["Format"]))
    //                {
    //                    case 1:                                 ////////////////////////// Currency

    //                        txt = new CustomTextBox();
    //                        txt.ID = "txt" + ctrlName;
    //                        txt.Text = ctrlValue;
    //                        txt.CssClass = "custom currency";
    //                        txt.MaxLength = 14;
    //                        varControl = "txt" + ctrlName;

    //                        txt.RenderControl(writer);

    //                        break;

    //                    case 2:                                 ////////////////////////// Date
    //                        txt = new CustomTextBox();

    //                        txt.ID = "txt" + ctrlName;
    //                        txt.Text = ctrlValue;
    //                        //txt.CssClass = "custom date-picker";
    //                        txt.CssClass = "custom datepicker_mom";
    //                        txt.MaxLength = 12;
    //                        varControl = "txt" + ctrlName;

    //                        txt.RenderControl(writer);

    //                        //CalendarExtender ce = new CalendarExtender();
    //                        //ce.Enabled = true;
    //                        //ce.TargetControlID = txt.ID;
    //                        //ce.ID = varControl + "_CalendarExtender";

    //                        #region comment
    //                        //writer = new HtmlTextWriter(sw);
    //                        //sw = new StringWriter(html);

    //                        //uc_Datepicker ucDate = (uc_Datepicker)LoadControl("uc_Datepicker.ascx");

    //                        //ucDate.ID = "txt" + ctrlName;
    //                        //ucDate._txtDate.Text = ctrlValue;
    //                        //ucDate.RenderControl(writer);

    //                        #endregion

    //                        break;

    //                    case 3:                                 ////////////////////////// Text

    //                        txt = new CustomTextBox();
    //                        txt.ID = "txt" + ctrlName;
    //                        txt.Text = ctrlValue;
    //                        txt.CssClass = "custom";
    //                        txt.MaxLength = 50;
    //                        varControl = "txt" + ctrlName;

    //                        txt.RenderControl(writer);

    //                        break;

    //                    case 4:                                 ////////////////////////// Dropdown

    //                        CustomDropDownList ddl = new CustomDropDownList();
    //                        ddl.ID = "ddl" + ctrlName;
    //                        ddl.Text = ctrlName;
    //                        ddl.CssClass = "browser-default custom";
    //                        ddl.Items.Add("Select");
    //                        ddl.SelectedValue = "Select";


    //                        dtItem = new DataTable();
    //                        var rows = dtCustomValue.AsEnumerable().Where(x => ((Int16)x["Line"]) == Convert.ToInt16(drc["Line"]));
    //                        if (rows.Any())
    //                            dtItem = rows.CopyToDataTable();
    //                        //dtItem = dtCustomValue.Select("Line="+drc["Format"].ToString()).CopyToDataTable();
    //                        foreach (DataRow drItem in dtItem.Rows)
    //                        {
    //                            ddl.Items.Add(drItem["Value"].ToString());
    //                        }

    //                        if (drc.ItemArray.Length > 6)
    //                        {
    //                            if (!string.IsNullOrEmpty(drc["Value"].ToString()))
    //                            {
    //                                ddl.SelectedValue = drc["Value"].ToString();
    //                            }
    //                        }
    //                        varControl = "ddl" + ctrlName;

    //                        ddl.RenderControl(writer);

    //                        break;


    //                    case 5:                                 ////////////////////////// Checkbox
    //                        CustomCheckBox chk = new CustomCheckBox();
    //                        chk.CssClass = "css-checkbox custom";
    //                        chk.Text = Convert.ToString(drc["Label"]);
    //                        chk.ID = "chk" + ctrlName;

    //                        if (!string.IsNullOrEmpty(drc["Value"].ToString()))
    //                        {
    //                            if (drc["Value"].ToString().ToLower().Equals("true"))
    //                            {
    //                                chk.Checked = true;
    //                            }
    //                            else if (drc["Value"].ToString().ToLower().Equals("on"))
    //                            {
    //                                chk.Checked = true;
    //                            }
    //                        }
    //                        varControl = "chk" + ctrlName;

    //                        chk.RenderControl(writer);

    //                        if (drc["Value"].ToString().ToLower().Equals("true"))
    //                        {
    //                            if (drc["UpdatedDate"] != DBNull.Value && drc["Username"] != DBNull.Value)
    //                            {
    //                                string username = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " + Convert.ToDateTime(drc["UpdatedDate"]).ToString("MM/dd/yy hh:mm tt") + " " + drc["Username"].ToString();
    //                                Label lbl = new Label();
    //                                lbl.ID = "custom";
    //                                lbl.ID = "lbl" + ctrlName;
    //                                lbl.Text = username;

    //                                sw = new StringWriter(html);
    //                                writer = new HtmlTextWriter(sw);
    //                                lbl.RenderControl(writer);
    //                            }
    //                        }
    //                        break;

    //                }

    //                DataRow dr = dtCustomField.NewRow();
    //                dr["ID"] = Convert.ToInt32(drc["ID"]);
    //                dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
    //                dr["Line"] = Convert.ToInt16(drc["Line"]);
    //                dr["Label"] = drc["Label"].ToString();
    //                dr["Value"] = drc["Value"].ToString();
    //                dr["Format"] = Convert.ToInt16(drc["Format"]);
    //                dr["ControlID"] = varControl;

    //                dtCustomField.Rows.Add(dr);

    //                #endregion

    //                if (Convert.ToInt16(drc["Format"]) == 5)
    //                {
    //                    html.Append("     </span>   \n");
    //                }

    //                html.Append("      </div>                        \n");
    //                html.Append("   </div>                           \n");


    //                count++;
    //                i++;


    //            }

    //            html.Append(" </div>                             \n");

    //            if (j < 2)
    //            {
    //                html.Append(" <div class='form-section3-blank'>&nbsp;</div>   \n");
    //            }
    //        }

    //        html.Append("   </div>                               \n");

    //        ViewState["CustomFields"] = dtCust;
    //        return html.ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    private void DisplayCustomByTab(DataTable dtCustom, DataTable dtVal, int jobTId, int jobId)
    {
        try
        {
            DataTable dtCustomTab = new DataTable();
            DataTable dtItem = new DataTable();
            //fetch all custom field from tblCustomTab and tblCustom
            DataTable dtCust = new DataTable();
            DataTable dtValue = new DataTable();

            //DataTable dtc = dtCustom;
            DataTable dtv = dtVal;
            objJob.ConnConfig = Session["config"].ToString();
            objJob.PageUrl = "addproject.aspx";

            DataSet _ds = objBL_Job.GetTabByPageUrl(objJob);

            objJob.ConnConfig = Session["config"].ToString();
            objJob.ID = jobTId;
            objJob.Job = jobId;
            DataSet dsTab = objBL_Job.GetProjectCustomTab(objJob);
            string html = string.Empty;
            ViewState["TabId"] = 0;

            pnlCustomAttributesGeneral.Visible = false;
            pnlCustomAttriEquip.Visible = false;
            pnlCustomAttributesGC.Visible = false;
            pnlCustomAttrNotes.Visible = false;
            pnlCustomFinanceBill.Visible = false;
            pnlCustomFinanceGeneral.Visible = false;
            pnlCustomFinceBudget.Visible = false;
            pnlCustomHeader.Visible = false;
            pnlCustomTicketList.Visible = false;
            pnlCustomBOM.Visible = false;
            pnlCustomHomeownerInfor.Visible = false;
            pnlCustomMilestone.Visible = false;
            pnlCustomTask.Visible = false;

            foreach (DataRow drCus in dsTab.Tables[0].Rows)
            {
                ViewState["TabId"] = 0;
                //html = string.Empty;
                switch (drCus["tblTabID"].ToString())
                {
                    case "1":                           //  Header

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 1);
                            if (rowsdt.Any())
                            {
                                pnlCustomHeader.Visible = true;
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 1);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                    ViewState["RadGrid_CustomHeader_dtValue"] = dtValue;
                                }

                                RadGrid_CustomHeader.DataSource = dtCust;
                                RadGrid_CustomHeader.DataBind();

                                //BindCustomDropDown(RadGrid_CustomHeader, dtValue);
                                foreach (DataRow drc in dtCust.Rows)
                                {
                                    DataRow dr = dtCustomField.NewRow();
                                    dr["ID"] = Convert.ToInt32(drc["ID"]);
                                    dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                                    dr["Line"] = Convert.ToInt16(drc["Line"]);
                                    dr["Label"] = drc["Label"].ToString();
                                    dr["Value"] = drc["Value"].ToString();
                                    dr["Format"] = Convert.ToInt16(drc["Format"]);
                                    dr["UpdatedDate"] = drc["UpdatedDate"];
                                    dr["Username"] = drc["Username"];
                                    dr["IsAlert"] = drc["IsAlert"];
                                    //dr["IsTask"] = drc["IsTask"];
                                    dr["TeamMember"] = drc["TeamMember"];
                                    dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];
                                    //dr["UserRole"] = drc["UserRole"];
                                    //dr["UserRoleDisplay"] = drc["UserRoleDisplay"];

                                    dtCustomField.Rows.Add(dr);
                                }
                            }
                        }
                        ViewState["TabId"] = 1;



                        //html = DisplayCustomField(dtCust, dtValue, PlaceHolderHeader);
                        //PlaceHolderHeader.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "2":                           //  Attributes - General

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 2);
                            if (rowsdt.Any())
                            {
                                pnlCustomAttributesGeneral.Visible = true;
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 2);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                    ViewState["RadGrid_CustomAttributesGeneral_dtValue"] = dtValue;
                                }
                                RadGrid_CustomAttributesGeneral.DataSource = dtCust;
                                RadGrid_CustomAttributesGeneral.DataBind();

                                //BindCustomDropDown(RadGrid_CustomAttributesGeneral, dtValue);
                                foreach (DataRow drc in dtCust.Rows)
                                {
                                    DataRow dr = dtCustomField.NewRow();
                                    dr["ID"] = Convert.ToInt32(drc["ID"]);
                                    dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                                    dr["Line"] = Convert.ToInt16(drc["Line"]);
                                    dr["Label"] = drc["Label"].ToString();
                                    dr["Value"] = drc["Value"].ToString();
                                    dr["Format"] = Convert.ToInt16(drc["Format"]);
                                    dr["UpdatedDate"] = drc["UpdatedDate"];
                                    dr["Username"] = drc["Username"];
                                    dr["IsAlert"] = drc["IsAlert"];
                                    //dr["IsTask"] = drc["IsTask"];
                                    dr["TeamMember"] = drc["TeamMember"];
                                    dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];
                                    //dr["UserRole"] = drc["UserRole"];
                                    //dr["UserRoleDisplay"] = drc["UserRoleDisplay"];

                                    dtCustomField.Rows.Add(dr);
                                }
                            }
                        }


                        //html = DisplayCustomField(dtCust, dtValue, PlaceHolderAttrGeneral);
                        //PlaceHolderAttrGeneral.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "3":                           //  Attributes - GC Info

                        //dtCust = new DataTable();
                        //dtValue = new DataTable();

                        //if (dtCustom.Rows.Count > 0)
                        //{
                        //    var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 3);
                        //    if (rowsdt.Any())
                        //    {
                        //        dtCust = rowsdt.CopyToDataTable();

                        //        var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 3);
                        //        if (rows.Any())
                        //        {
                        //            dtValue = rows.CopyToDataTable();
                        //        }

                        //        pnlCustomAttributesGC.Visible = true;
                        //        RadGrid_CustomAttributesGC.DataSource = dtCust;
                        //        RadGrid_CustomAttributesGC.DataBind();
                        //        foreach (DataRow drc in dtCust.Rows)
                        //        {
                        //            DataRow dr = dtCustomField.NewRow();
                        //            dr["ID"] = Convert.ToInt32(drc["ID"]);
                        //            dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                        //            dr["Line"] = Convert.ToInt16(drc["Line"]);
                        //            dr["Label"] = drc["Label"].ToString();
                        //            dr["Value"] = drc["Value"].ToString();
                        //            dr["Format"] = Convert.ToInt16(drc["Format"]);
                        //            dr["UpdatedDate"] = drc["UpdatedDate"];
                        //            dr["Username"] = drc["Username"];
                        //            dr["IsAlert"] = drc["IsAlert"];
                        //            dr["TeamMember"] = drc["TeamMember"];
                        //            dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];

                        //            dtCustomField.Rows.Add(dr);
                        //        }
                        //    }
                        //}

                        ////html = DisplayCustomField(dtCust, dtValue, PlaceHolderAttriGC);
                        ////PlaceHolderAttriGC.Controls.Add(new Literal { Text = html.ToString() });

                        //break;
                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 3);
                            if (rowsdt.Any())
                            {
                                pnlCustomAttributesGC.Visible = true;
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 3);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                    ViewState["RadGrid_CustomAttributesGC_dtValue"] = dtValue;
                                }
                                RadGrid_CustomAttributesGC.DataSource = dtCust;
                                RadGrid_CustomAttributesGC.DataBind();

                                //BindCustomDropDown(RadGrid_CustomAttributesGC, dtValue);
                                foreach (DataRow drc in dtCust.Rows)
                                {
                                    DataRow dr = dtCustomField.NewRow();
                                    dr["ID"] = Convert.ToInt32(drc["ID"]);
                                    dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                                    dr["Line"] = Convert.ToInt16(drc["Line"]);
                                    dr["Label"] = drc["Label"].ToString();
                                    dr["Value"] = drc["Value"].ToString();
                                    dr["Format"] = Convert.ToInt16(drc["Format"]);
                                    dr["UpdatedDate"] = drc["UpdatedDate"];
                                    dr["Username"] = drc["Username"];
                                    dr["IsAlert"] = drc["IsAlert"];
                                    //dr["IsTask"] = drc["IsTask"];
                                    dr["TeamMember"] = drc["TeamMember"];
                                    dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];
                                    //dr["UserRole"] = drc["UserRole"];
                                    //dr["UserRoleDisplay"] = drc["UserRoleDisplay"];

                                    dtCustomField.Rows.Add(dr);
                                }
                            }
                        }


                        //html = DisplayCustomField(dtCust, dtValue, PlaceHolderAttrGeneral);
                        //PlaceHolderAttrGeneral.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "4":                           //  Attributes - Equipment

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 4);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 4);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                    ViewState["RadGrid_CustomAttriEquip_dtValue"] = dtValue;
                                }

                                pnlCustomAttriEquip.Visible = true;
                                RadGrid_CustomAttriEquip.DataSource = dtCust;
                                RadGrid_CustomAttriEquip.DataBind();

                                //BindCustomDropDown(RadGrid_CustomAttriEquip, dtValue);
                                foreach (DataRow drc in dtCust.Rows)
                                {
                                    DataRow dr = dtCustomField.NewRow();
                                    dr["ID"] = Convert.ToInt32(drc["ID"]);
                                    dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                                    dr["Line"] = Convert.ToInt16(drc["Line"]);
                                    dr["Label"] = drc["Label"].ToString();
                                    dr["Value"] = drc["Value"].ToString();
                                    dr["Format"] = Convert.ToInt16(drc["Format"]);
                                    dr["UpdatedDate"] = drc["UpdatedDate"];
                                    dr["Username"] = drc["Username"];
                                    dr["IsAlert"] = drc["IsAlert"];
                                    //dr["IsTask"] = drc["IsTask"];
                                    dr["TeamMember"] = drc["TeamMember"];
                                    dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];
                                    //dr["UserRole"] = drc["UserRole"];
                                    //dr["UserRoleDisplay"] = drc["UserRoleDisplay"];

                                    dtCustomField.Rows.Add(dr);
                                }
                            }
                        }

                        //html = DisplayCustomField(dtCust, dtValue, PlaceHolderAttriEquip, 1);
                        //PlaceHolderAttriEquip.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "5":                           //  Finance - General

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 5);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 5);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                    ViewState["RadGrid_CustomFinanceGeneral_dtValue"] = dtValue;
                                }

                                pnlCustomFinanceGeneral.Visible = true;
                                RadGrid_CustomFinanceGeneral.DataSource = dtCust;
                                RadGrid_CustomFinanceGeneral.DataBind();

                                //BindCustomDropDown(RadGrid_CustomFinanceGeneral, dtValue);
                                foreach (DataRow drc in dtCust.Rows)
                                {
                                    DataRow dr = dtCustomField.NewRow();
                                    dr["ID"] = Convert.ToInt32(drc["ID"]);
                                    dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                                    dr["Line"] = Convert.ToInt16(drc["Line"]);
                                    dr["Label"] = drc["Label"].ToString();
                                    dr["Value"] = drc["Value"].ToString();
                                    dr["Format"] = Convert.ToInt16(drc["Format"]);
                                    dr["UpdatedDate"] = drc["UpdatedDate"];
                                    dr["Username"] = drc["Username"];
                                    dr["IsAlert"] = drc["IsAlert"];
                                    //dr["IsTask"] = drc["IsTask"];
                                    dr["TeamMember"] = drc["TeamMember"];
                                    dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];
                                    //dr["UserRole"] = drc["UserRole"];
                                    //dr["UserRoleDisplay"] = drc["UserRoleDisplay"];

                                    dtCustomField.Rows.Add(dr);
                                }
                            }
                        }

                        //html = DisplayCustomField(dtCust, dtValue, PlaceHolderFinceGeneral);
                        //PlaceHolderFinceGeneral.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "6":                           //  Finance - Billing

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 6);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 6);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                    ViewState["RadGrid_CustomFinanceBill_dtValue"] = dtValue;
                                }

                                pnlCustomFinanceBill.Visible = true;
                                RadGrid_CustomFinanceBill.DataSource = dtCust;
                                RadGrid_CustomFinanceBill.DataBind();

                                //BindCustomDropDown(RadGrid_CustomFinanceBill, dtValue);
                                foreach (DataRow drc in dtCust.Rows)
                                {
                                    DataRow dr = dtCustomField.NewRow();
                                    dr["ID"] = Convert.ToInt32(drc["ID"]);
                                    dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                                    dr["Line"] = Convert.ToInt16(drc["Line"]);
                                    dr["Label"] = drc["Label"].ToString();
                                    dr["Value"] = drc["Value"].ToString();
                                    dr["Format"] = Convert.ToInt16(drc["Format"]);
                                    dr["UpdatedDate"] = drc["UpdatedDate"];
                                    dr["Username"] = drc["Username"];
                                    dr["IsAlert"] = drc["IsAlert"];
                                    //dr["IsTask"] = drc["IsTask"];
                                    dr["TeamMember"] = drc["TeamMember"];
                                    dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];
                                    //dr["UserRole"] = drc["UserRole"];
                                    //dr["UserRoleDisplay"] = drc["UserRoleDisplay"];

                                    dtCustomField.Rows.Add(dr);
                                }
                            }
                        }

                        //html = DisplayCustomField(dtCust, dtValue, PlaceHolderFinceBill);
                        //PlaceHolderFinceBill.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "7":                           //  Finance - Budgets

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 7);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();
                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 7);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                    ViewState["RadGrid_CustomFinceBudget_dtValue"] = dtValue;
                                }

                                pnlCustomFinceBudget.Visible = true;
                                RadGrid_CustomFinceBudget.DataSource = dtCust;
                                RadGrid_CustomFinceBudget.DataBind();

                                //BindCustomDropDown(RadGrid_CustomFinceBudget, dtValue);
                                foreach (DataRow drc in dtCust.Rows)
                                {
                                    DataRow dr = dtCustomField.NewRow();
                                    dr["ID"] = Convert.ToInt32(drc["ID"]);
                                    dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                                    dr["Line"] = Convert.ToInt16(drc["Line"]);
                                    dr["Label"] = drc["Label"].ToString();
                                    dr["Value"] = drc["Value"].ToString();
                                    dr["Format"] = Convert.ToInt16(drc["Format"]);
                                    dr["UpdatedDate"] = drc["UpdatedDate"];
                                    dr["Username"] = drc["Username"];
                                    dr["IsAlert"] = drc["IsAlert"];
                                    //dr["IsTask"] = drc["IsTask"];
                                    dr["TeamMember"] = drc["TeamMember"];
                                    dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];
                                    //dr["UserRole"] = drc["UserRole"];
                                    //dr["UserRoleDisplay"] = drc["UserRoleDisplay"];

                                    dtCustomField.Rows.Add(dr);
                                }
                            }
                        }

                        //html = DisplayCustomField(dtCust, dtValue, PlaceHolderFinceBudget);
                        //PlaceHolderFinceBudget.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "8":                           //  Ticketlist

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 8);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 8);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                    ViewState["RadGrid_CustomTicketList_dtValue"] = dtValue;
                                }

                                pnlCustomTicketList.Visible = true;
                                RadGrid_CustomTicketList.DataSource = dtCust;
                                RadGrid_CustomTicketList.DataBind();

                                //BindCustomDropDown(RadGrid_CustomTicketList, dtValue);
                                foreach (DataRow drc in dtCust.Rows)
                                {
                                    DataRow dr = dtCustomField.NewRow();
                                    dr["ID"] = Convert.ToInt32(drc["ID"]);
                                    dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                                    dr["Line"] = Convert.ToInt16(drc["Line"]);
                                    dr["Label"] = drc["Label"].ToString();
                                    dr["Value"] = drc["Value"].ToString();
                                    dr["Format"] = Convert.ToInt16(drc["Format"]);
                                    dr["UpdatedDate"] = drc["UpdatedDate"];
                                    dr["Username"] = drc["Username"];
                                    dr["IsAlert"] = drc["IsAlert"];
                                    //dr["IsTask"] = drc["IsTask"];
                                    dr["TeamMember"] = drc["TeamMember"];
                                    dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];
                                    //dr["UserRole"] = drc["UserRole"];
                                    //dr["UserRoleDisplay"] = drc["UserRoleDisplay"];

                                    dtCustomField.Rows.Add(dr);
                                }
                            }
                        }

                        //html = DisplayCustomField(dtCust, dtValue, PlaceHolderTicket);
                        //PlaceHolderTicket.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "9":                           //  BOM

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 9);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();
                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 9);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                    ViewState["RadGrid_CustomBOM_dtValue"] = dtValue;
                                }
                                pnlCustomBOM.Visible = true;
                                RadGrid_CustomBOM.DataSource = dtCust;
                                RadGrid_CustomBOM.DataBind();

                                //BindCustomDropDown(RadGrid_CustomBOM, dtValue);
                                foreach (DataRow drc in dtCust.Rows)
                                {
                                    DataRow dr = dtCustomField.NewRow();
                                    dr["ID"] = Convert.ToInt32(drc["ID"]);
                                    dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                                    dr["Line"] = Convert.ToInt16(drc["Line"]);
                                    dr["Label"] = drc["Label"].ToString();
                                    dr["Value"] = drc["Value"].ToString();
                                    dr["Format"] = Convert.ToInt16(drc["Format"]);
                                    dr["UpdatedDate"] = drc["UpdatedDate"];
                                    dr["Username"] = drc["Username"];
                                    dr["IsAlert"] = drc["IsAlert"];
                                    //dr["IsTask"] = drc["IsTask"];
                                    dr["TeamMember"] = drc["TeamMember"];
                                    dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];
                                    //dr["UserRole"] = drc["UserRole"];
                                    //dr["UserRoleDisplay"] = drc["UserRoleDisplay"];

                                    dtCustomField.Rows.Add(dr);
                                }
                            }
                        }

                        //html = DisplayCustomField(dtCust, dtValue, PlaceHolderBOM);
                        //PlaceHolderBOM.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "10":                           //  Milestones

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 10);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();
                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 10);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                    ViewState["RadGrid_CustomMilestone_dtValue"] = dtValue;
                                }

                                pnlCustomMilestone.Visible = true;
                                RadGrid_CustomMilestone.DataSource = dtCust;
                                RadGrid_CustomMilestone.DataBind();

                                //BindCustomDropDown(RadGrid_CustomMilestone, dtValue);
                                foreach (DataRow drc in dtCust.Rows)
                                {
                                    DataRow dr = dtCustomField.NewRow();
                                    dr["ID"] = Convert.ToInt32(drc["ID"]);
                                    dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                                    dr["Line"] = Convert.ToInt16(drc["Line"]);
                                    dr["Label"] = drc["Label"].ToString();
                                    dr["Value"] = drc["Value"].ToString();
                                    dr["Format"] = Convert.ToInt16(drc["Format"]);
                                    dr["UpdatedDate"] = drc["UpdatedDate"];
                                    dr["Username"] = drc["Username"];
                                    dr["IsAlert"] = drc["IsAlert"];
                                    //dr["IsTask"] = drc["IsTask"];
                                    dr["TeamMember"] = drc["TeamMember"];
                                    dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];
                                    //dr["UserRole"] = drc["UserRole"];
                                    //dr["UserRoleDisplay"] = drc["UserRoleDisplay"];

                                    dtCustomField.Rows.Add(dr);
                                }
                            }
                        }

                        //html = DisplayCustomField(dtCust, dtValue, PlaceHolderMilestone);
                        //PlaceHolderMilestone.Controls.Add(new Literal { Text = html.ToString() });

                        break;

                    case "11":                           //  Task

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 11);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 11);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                    ViewState["RadGrid_CustomTask_dtValue"] = dtValue;
                                }

                                pnlCustomTask.Visible = true;
                                RadGrid_CustomTask.DataSource = dtCust;
                                RadGrid_CustomTask.DataBind();

                                //BindCustomDropDown(RadGrid_CustomTask, dtValue);
                                foreach (DataRow drc in dtCust.Rows)
                                {
                                    DataRow dr = dtCustomField.NewRow();
                                    dr["ID"] = Convert.ToInt32(drc["ID"]);
                                    dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                                    dr["Line"] = Convert.ToInt16(drc["Line"]);
                                    dr["Label"] = drc["Label"].ToString();
                                    dr["Value"] = drc["Value"].ToString();
                                    dr["Format"] = Convert.ToInt16(drc["Format"]);
                                    dr["UpdatedDate"] = drc["UpdatedDate"];
                                    dr["Username"] = drc["Username"];
                                    dr["IsAlert"] = drc["IsAlert"];
                                    //dr["IsTask"] = drc["IsTask"];
                                    dr["TeamMember"] = drc["TeamMember"];
                                    dr["TeamMemberDisplay"] = drc["TeamMemberDisplay"];
                                    //dr["UserRole"] = drc["UserRole"];
                                    //dr["UserRoleDisplay"] = drc["UserRoleDisplay"];

                                    dtCustomField.Rows.Add(dr);
                                }
                            }
                        }

                        //html = DisplayCustomField(dtCust, dtValue, TaskPlaceholder);
                        //TaskPlaceholder.Controls.Add(new Literal { Text = html.ToString() });
                        break;
                }
            }
            ViewState["CustomFields"] = dtCustomField;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable GetCustomItems()
    {
        string strItems = hdnCustomJSON.Value;
        DataTable dt = (DataTable)ViewState["CustomFields"];
        try
        {
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objItemData = new List<Dictionary<object, object>>();
                objItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                if (objItemData.Count > 0)
                {
                    DataColumnCollection dt_col = dt.Columns;
                    if (dt_col.Contains("ControlID"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            var customDs = objItemData.Where(y => y.Keys.Contains(dr["ControlID"].ToString())).FirstOrDefault();
                            if (customDs != null)
                            {
                                if (customDs.Count > 0)
                                {
                                    dr["Value"] = customDs.Values.First();
                                }
                            }
                        }
                    }
                    //dt.Columns.Remove("ControlID");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    #endregion

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            Response.Redirect("addproject.aspx?uid=" + dt.Rows[index + 1]["id"], false);
        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            Response.Redirect("addproject.aspx?uid=" + dt.Rows[index - 1]["id"], false);
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        if (dt != null)
            Response.Redirect("addproject.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["id"], false);
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        if (dt != null)
            Response.Redirect("addproject.aspx?uid=" + dt.Rows[0]["id"], false);
    }

    protected void rtEquips_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Repeater rtEquipCustom = e.Item.FindControl("rtEquipCustom") as Repeater;
            Label lblID = e.Item.FindControl("lblID") as Label;
            System.Web.UI.WebControls.Image imgPlus = e.Item.FindControl("imgPlus") as System.Web.UI.WebControls.Image;
            DataTable dt = getequipCustomItemsByElevID(Convert.ToInt32(lblID.Text));
            if (dt.Rows.Count > 0)
                imgPlus.Visible = true;
            else
                imgPlus.Visible = false;
            rtEquipCustom.DataSource = dt;
            rtEquipCustom.DataBind();
        }
    }

    private DataTable getequipCustomItemsByElevID(int equipid)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = equipid;
        DataSet ds = new DataSet();
        ds = objBL_User.getequipCustomItemsByElevID(objPropUser);
        return ds.Tables[0];
    }

    protected void lnkEditEq_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem di in rtEquips.Items)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text);
            }
        }
    }

    protected void btnCopyEQ_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem di in rtEquips.Items)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?t=c&uid=" + lblUserID.Text);
            }
        }
    }

    protected void lnkAddEQ_Click(object sender, EventArgs e)
    {
        string url = "addequipment.aspx";
        if (txtLocation.Text.Trim() != null & hdnLocID.Value.Trim() != null)
        {
            url += "?lid=" + hdnLocID.Value.Trim() + "&locname=" + txtLocation.Text.Trim();
        }
        Response.Redirect(url);
    }

    private List<ListItem> Paginate(List<ListItem> items, int page, HiddenField hdnpages, HiddenField hdnpagescount)
    {
        int rows = 2;
        int cols = 4;
        int total = rows * cols;

        int noofrecordsperpage = total;
        int pagecount = 1;
        pagecount = (items.Count + noofrecordsperpage - 1) / noofrecordsperpage;

        //if (page > pagecount)
        //    page = Convert.ToInt32(lblpages.Text);

        int MaxRecord = (noofrecordsperpage * Convert.ToInt32(page)) - 1;

        int MinRecord = (MaxRecord - noofrecordsperpage) < 0 ? 0 : (MaxRecord - noofrecordsperpage) + 1;



        List<ListItem> cuurentlist = new List<ListItem>();

        for (int i = 0; i < items.Count; i++)
        {
            //Check If the records found are less than the no of reords that needs to be displayed in the gird
            if (pagecount > 0)
            {
                if (i >= MinRecord && i <= MaxRecord)
                    cuurentlist.Add(items[i]);
            }
            else
                cuurentlist.Add(items[i]);
        }

        if (hdnpages != null)
        {
            hdnpages.Value = page.ToString();
        }
        if (hdnpagescount != null)
        {
            hdnpagescount.Value = pagecount.ToString();
        }
        return cuurentlist;

    }

    protected void rptTicketTask_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Repeater rtEquipCustom = e.Item.FindControl("rtTasks") as Repeater;
            Label lblID = e.Item.FindControl("lblID") as Label;
            DataTable dt = getTicketTaskByTicketID(Convert.ToInt32(lblID.Text));
            rtEquipCustom.DataSource = dt;
            rtEquipCustom.DataBind();
        }
    }

    private DataTable getTicketTaskByTicketID(int TicketID)
    {
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = TicketID;
        DataSet ds = objBL_MapData.getTicketTaskByTicketID(objMapData);
        return ds.Tables[0];
    }

    private void GetControl()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objPropUser);
        if (ds.Tables[0].Rows.Count > 0)
        {
            bool tasks = Convert.ToBoolean(ds.Tables[0].Rows[0]["TaskCode"]);
            ViewState["tasks"] = tasks;
            liaccrdTicketTask.Visible = tasks;
            //liaccrdTicketTask.Visible = tasks;

            int codes = 0;
            if (ds.Tables[0].Rows[0]["codes"] != DBNull.Value)
                codes = Convert.ToInt16(ds.Tables[0].Rows[0]["codes"]);

            if (codes == 0)
            {
                liaccrdTicketTask.Visible = false;
                //liaccrdTicketTask.Visible = false;
                ViewState["tasks"] = false;
                rfvTaskCategory.Enabled = false;
            }
            else if (codes == 1)
            {
                liaccrdTicketTask.Visible = true;
                //liaccrdTicketTask.Visible = true;
                ViewState["tasks"] = false;
                pnlCodes.Visible = true;
                rfvTaskCategory.Enabled = true;
            }
            else if (codes == 2)
            {
                liaccrdTicketTask.Visible = true;
                //liaccrdTicketTask.Visible = true;
                ViewState["tasks"] = true;
                pnlCodes.Visible = false;
                rfvTaskCategory.Enabled = false;
            }

            //Show TabPaneHomeowner
            //TabPaneHomeowner.Visible = Convert.ToBoolean(ds.Tables[0].Rows[0]["ISshowHomeowner"]);
            liHomeowner.Visible = Convert.ToBoolean(ds.Tables[0].Rows[0]["ISshowHomeowner"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["ISshowHomeowner"]);
            //added hidden value to execute the autocomplete for homeowner fields only if homeowner is visible 
            hdnIsHO.Value = Convert.ToBoolean(ds.Tables[0].Rows[0]["ISshowHomeowner"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["ISshowHomeowner"]).ToString();

            if (ds.Tables[0].Rows[0]["TargetHPermission"].ToString() == "1")

            {
                lnktargeted.Visible = lnktargeted1.Visible = chkTargetHours.Visible = true;
                hdnTHPermission.Value = "1";
            }
            else
            {
                lnktargeted.Visible = lnktargeted1.Visible = chkTargetHours.Visible = false;
                hdnTHPermission.Value = "0";
            }

        }
    }

    private void FillCategory()
    {
    }

    private void FillWorker()
    {

    }
    private void FillLocationType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getlocationType(objPropUser);
        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "Type";
        ddlType.DataValueField = "Type";
        ddlType.DataBind();

        ddlType.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    private void FillRoute()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        Int32 ContractID = 0;
        ContractID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"].ToString());
        ds = objBL_User.getRoute(objPropUser, 1, 0, ContractID);//IsActive=1 :- Get Only Active Workers
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "label";
        ddlRoute.DataValueField = "ID";
        ddlRoute.DataBind();

        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));
        ddlRoute.Items.Insert(1, new ListItem("Unassigned", "0"));
    }
    private void SetDefaultWorker()
    {
        Customer objCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        string getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        if (!string.IsNullOrEmpty(getValue))
        {
            lblDefaultWorker.InnerText = getValue;
        }
        else
        {
            lblDefaultWorker.InnerText = "Default Worker";
        }
    }
    protected void lnkSaveTicket_Click(object sender, EventArgs e)
    {


    }

    protected void gvTickets_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;
        String _OrderBy = "";
        if (hdTicketListOrderBy.Value == ASCENDING)
        {
            _OrderBy = ASCENDING;
        }
        else
        {
            _OrderBy = Convert.ToString(hdTicketListOrderBy.Value);
        }

        if (_OrderBy == ASCENDING)
        {
            String OrderBy = sortExpression + " " + DESCENDING;
            hdTicketListOrderBy.Value = DESCENDING;
            BindTicketList(OrderBy, 1);

        }
        else
        {
            String OrderBy = sortExpression + " " + ASCENDING;
            hdTicketListOrderBy.Value = ASCENDING;
            BindTicketList(OrderBy, 1);
        }


    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    private void getDiagnosticCategory()
    {
        DataSet ds = new DataSet();
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getDiagnosticCategory(objGeneral);
        ddlCodeCat.DataSource = ds.Tables[0];
        ddlCodeCat.DataTextField = "category";
        ddlCodeCat.DataValueField = "category";
        ddlCodeCat.DataBind();

        ddlCodeCat.Items.Insert(0, new ListItem("Select", ""));
    }

    private void getDiagnosticCodes()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        objGeneral.CodeCategory = ddlCodeCat.SelectedValue;
        objGeneral.CodeType = 1;
        DataSet ds = new DataSet();
        ds = objBL_General.getDiagnostic(objGeneral);
        rptCodesList.DataSource = ds.Tables[0];
        rptCodesList.DataBind();

    }

    private DataTable TaskCodes()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("ticket_id", typeof(int));
        dt.Columns.Add("task_code", typeof(string));
        dt.Columns.Add("Category", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("job", typeof(int));
        dt.Columns.Add("username", typeof(string));
        dt.Columns.Add("dateupdated", typeof(DateTime));

        foreach (RepeaterItem lst in rptCodesList.Items)
        {
            CheckBox chkCode = (CheckBox)lst.FindControl("chkCode");
            HiddenField hdnCodeCat = (HiddenField)lst.FindControl("hdnCodeCat");
            HiddenField hdnTicket = (HiddenField)lst.FindControl("hdnTicket");
            HiddenField hdnChecked = (HiddenField)lst.FindControl("hdnChecked");
            HiddenField hdnUsername = (HiddenField)lst.FindControl("hdnUsername");
            HiddenField hdnDate = (HiddenField)lst.FindControl("hdnDate");

            if (chkCode.Checked)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = 0;
                if (hdnTicket.Value != string.Empty)
                    dr["ticket_id"] = Convert.ToInt32(hdnTicket.Value);
                //dr["ticket_id"] = (hdnTicket.Value != string.Empty) ? Convert.ToInt32(hdnTicket.Value) : 0;
                dr["task_code"] = chkCode.Text;
                dr["Category"] = hdnCodeCat.Value;
                if (hdnChecked.Value == "1")
                {
                    dr["username"] = Session["username"].ToString();
                    dr["dateupdated"] = System.DateTime.Now;
                }
                else
                {
                    dr["username"] = hdnUsername.Value;
                    dr["dateupdated"] = Convert.ToDateTime(hdnDate.Value);
                }
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    private void getProjectTasks()
    {
        if (Request.QueryString["uid"] != null)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
            DataSet ds = objBL_Customer.getJobTasks(objProp_Customer);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (RepeaterItem rt in rptCodesList.Items)
                {
                    CheckBox chkCode = (CheckBox)rt.FindControl("chkCode");
                    HiddenField hdnCodeCat = (HiddenField)rt.FindControl("hdnCodeCat");
                    HiddenField hdnTicket = (HiddenField)rt.FindControl("hdnTicket");
                    HiddenField hdnUsername = (HiddenField)rt.FindControl("hdnUsername");
                    HiddenField hdnDate = (HiddenField)rt.FindControl("hdnDate");
                    Label lblDesc = (Label)rt.FindControl("lblDesc");
                    HyperLink lnkTicket = (HyperLink)rt.FindControl("lnkTicket");

                    if (hdnCodeCat.Value.ToLower() == dr["category"].ToString().ToLower() && chkCode.Text.ToLower() == dr["task_code"].ToString().ToLower())
                    {
                        chkCode.Checked = true;
                        lblDesc.Text = dr["username"].ToString() + "  " + String.Format("{0:MM/dd/yyyy hh:mm tt}", dr["dateupdated"]);
                        hdnTicket.Value = dr["ticket_id"].ToString();
                        hdnUsername.Value = dr["username"].ToString();
                        hdnDate.Value = dr["dateupdated"].ToString();
                        lnkTicket.Text = dr["ticket_id"].ToString();
                        lnkTicket.NavigateUrl = "addticket.aspx?comp=0&id=" + dr["ticket_id"].ToString();
                    }
                }
            }
        }
    }

    protected void ddlCodeCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectTaskCategory();
    }

    private void SelectTaskCategory()
    {
        getDiagnosticCodes();
        getProjectTasks();
    }

    private void GetJobtaskCategory()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
        string cat = objBL_Customer.getJobTasksCategory(objProp_Customer);
        if (cat.Trim() != string.Empty)
        {
            //ddlCodeCat.SelectedValue = cat;
            ddlCodeCat.Enabled = false;
            getDiagnosticCodes();
            getProjectTasks();
        }
    }

    private void GetGC_HOInfo(string LocID)
    {
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.LocID = Convert.ToInt32(LocID);
        objPropUser.ConnConfig = Session["config"].ToString();
        var dsGCandHower = objBL_User.GetGCandHowerLocID(objPropUser);

        #region gc information-------------------------------------------------------->

       

        // Fill GC Infomation---------------------------------------------------------->

        // Fill GC Infomation---------------------------------------------------------->
        //objgn.ResetFormControlValues(tbpnlGCInfo);
        ResetFormControlValues(tbpnlGCInfo);
        objgn.ResetFormControlValues(tbpnlHomeowner);
        if (dsGCandHower.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsGCandHower.Tables[0].Rows[0]["RolID"].ToString()))
            {
                if (!Convert.ToInt32(dsGCandHower.Tables[0].Rows[0]["RolID"]).Equals(0) && Convert.ToInt32(dsGCandHower.Tables[0].Rows[0]["LocContactType"]).Equals(1))
                {
                    hdnGCID.Value = dsGCandHower.Tables[0].Rows[0]["RolID"].ToString();
                    hdnGCIDtemp.Value = dsGCandHower.Tables[0].Rows[0]["RolID"].ToString();
                    txtName.Text = dsGCandHower.Tables[0].Rows[0]["RolName"].ToString();
                    txtCity.Text = dsGCandHower.Tables[0].Rows[0]["city"].ToString();
                    GctxtAddress.Text = dsGCandHower.Tables[0].Rows[0]["Address"].ToString();
                    if (!string.IsNullOrEmpty(dsGCandHower.Tables[0].Rows[0]["state"].ToString()))
                    {
                        ddlState.SelectedValue = dsGCandHower.Tables[0].Rows[0]["state"].ToString();
                    }
                    txtPostalCode.Text = dsGCandHower.Tables[0].Rows[0]["zip"].ToString();
                    txtCountry.Text = dsGCandHower.Tables[0].Rows[0]["country"].ToString();
                    txtPhone.Text = dsGCandHower.Tables[0].Rows[0]["phone"].ToString();
                    txtMobile.Text = dsGCandHower.Tables[0].Rows[0]["cellular"].ToString();
                    txtFax.Text = dsGCandHower.Tables[0].Rows[0]["fax"].ToString();
                    txtEmailWeb.Text = dsGCandHower.Tables[0].Rows[0]["email"].ToString();
                    txtRemarks.Text = dsGCandHower.Tables[0].Rows[0]["rolRemarks"].ToString();
                    txtContactName.Text = dsGCandHower.Tables[0].Rows[0]["contact"].ToString();
                    hdnGContractorID.Value = dsGCandHower.Tables[0].Rows[0]["RolID"].ToString();
                    hdnGCNameupdate.Value = "0";
                }
            }
        }
        // Fill Homeowner Infomation---------------------------------------------------------->
        if (dsGCandHower.Tables[1].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsGCandHower.Tables[1].Rows[0]["RolID"].ToString()))
            {
                if (!Convert.ToInt32(dsGCandHower.Tables[1].Rows[0]["RolID"]).Equals(0) && Convert.ToInt32(dsGCandHower.Tables[1].Rows[0]["LocContactType"]).Equals(2))
                {
                    hotxtname.Text = dsGCandHower.Tables[1].Rows[0]["RolName"].ToString();
                    hotxtcity.Text = dsGCandHower.Tables[1].Rows[0]["city"].ToString();
                    HotxtAddress.Text = dsGCandHower.Tables[1].Rows[0]["Address"].ToString();
                    if (!string.IsNullOrEmpty(dsGCandHower.Tables[1].Rows[0]["state"].ToString()))
                    {
                        hotddlstate.SelectedValue = dsGCandHower.Tables[1].Rows[0]["state"].ToString();
                    }
                    hotxtZIP.Text = dsGCandHower.Tables[1].Rows[0]["zip"].ToString();
                    hotxtCountry.Text = dsGCandHower.Tables[1].Rows[0]["country"].ToString();
                    hotxtPhone.Text = dsGCandHower.Tables[1].Rows[0]["phone"].ToString();
                    hotxtMobile.Text = dsGCandHower.Tables[1].Rows[0]["cellular"].ToString();
                    HotxtFax.Text = dsGCandHower.Tables[1].Rows[0]["fax"].ToString();
                    HotxtEmailWeb.Text = dsGCandHower.Tables[1].Rows[0]["email"].ToString();
                    hotxtRemarks.Text = dsGCandHower.Tables[1].Rows[0]["rolRemarks"].ToString();
                    hotxtContactName.Text = dsGCandHower.Tables[1].Rows[0]["contact"].ToString();
                    hdnHomeOwnerID.Value = dsGCandHower.Tables[1].Rows[0]["RolID"].ToString();
                    hdnHOName.Value = dsGCandHower.Tables[1].Rows[0]["RolName"].ToString();
                    hdnHONameupdate.Value = "0";
                }
            }
        }

        #endregion
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        string ticketid = string.Empty;
        try
        {
            foreach (GridDataItem di in gvTickets.Items)
            {
                CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
                Label lblTicketId = (Label)di.FindControl("lblTicketId");

                if (chkSelect.Checked == true)
                {
                    if (ticketid != string.Empty)
                        ticketid += ", " + lblTicketId.Text;
                    else
                        ticketid = lblTicketId.Text;
                    DeleteTicket(Convert.ToInt32(lblTicketId.Text));
                    GetOpenCalls(string.Empty);
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyDelete", "noty({text: 'Ticket # " + ticketid + " Deleted Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); ", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelete", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void DeleteTicket(int TicketID)
    {
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = TicketID;
        objMapData.Worker = Session["username"].ToString();
        objBL_MapData.DeleteTicket(objMapData);
    }

    #region Add Attachments

    /// <summary>
    /// Allow the user to add Attachments in the Project screen 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>



    #endregion

    protected void lbtnTypeSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            objJob.ConnConfig = Session["config"].ToString();
            objJob.TypeName = txtBomType.Text;
            objJob.TypeId = objBL_Job.AddBOMType(objJob);

            DataSet ds = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            ds = objBL_Job.GetBomType(objJob);
            dtBomType = ds.Tables[0];

            foreach (GridDataItem gr in gvBOM.Items)
            {
                DropDownList ddlBType = (DropDownList)gr.FindControl("ddlBType");
                ddlBType.Items.Add(new ListItem(objJob.TypeName.ToString(), objJob.TypeId.ToString()));
            }
            txtBomType.Text = string.Empty;
            mpeAddBomType.Hide();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


   

    protected void chkBoxIsRecd_OnCheckedChanged(object sender, EventArgs e)
    {
        int JobID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
        int IsHighLighted = 0;
        if (JobID > 0)
        {
            foreach (GridDataItem row in gvContacts.Items)
            {
                CheckBox check = (CheckBox)row.FindControl("chkBoxIsRecd");
                IsHighLighted = check.Checked == true ? 1 : 0;
                Label PhoneID = (Label)row.FindControl("PhoneID");
                objJob.ConnConfig = Session["config"].ToString();
                objJob.Job = JobID;
                objBL_Job.AddJoinPhoneJob(objJob, Convert.ToInt32(PhoneID.Text), Convert.ToInt32(IsHighLighted));
            }
        }
        Fill_gvContacts();
    }

    private void BindgvMilestones(DataTable dt)
    {
        gvMilestones.DataSource = dt;
        gvMilestones.DataBind();
        GetWIP();
    }

    private void BindgvBOM(DataTable dt, bool Rebind = true)
    {
        lnktargeted.Visible = lnktargeted1.Visible = gvBOM.GroupingEnabled = chkTargetHours.Checked;
        if (gvBOM.GroupingEnabled)
        {
            GridGroupByExpression expression = new GridGroupByExpression();
            GridGroupByField gridGroupByField = new GridGroupByField();
            gridGroupByField = new GridGroupByField();
            gridGroupByField.FieldName = "GroupName";
            gridGroupByField.HeaderText = "GroupName";
            expression.SelectFields.Add(gridGroupByField);

            gridGroupByField = new GridGroupByField();
            gridGroupByField.FieldName = "GroupName";
            expression.GroupByFields.Add(gridGroupByField);
            gvBOM.MasterTableView.GroupByExpressions.Add(expression);


            expression = new GridGroupByExpression();
            gridGroupByField = new GridGroupByField();
            gridGroupByField.FieldName = "Code";
            gridGroupByField.HeaderText = "Code";
            expression.SelectFields.Add(gridGroupByField);

            gridGroupByField = new GridGroupByField();
            gridGroupByField.FieldName = "Code";
            expression.GroupByFields.Add(gridGroupByField);
            gvBOM.MasterTableView.GroupByExpressions.Add(expression);

            // TargetHours
            expression = new GridGroupByExpression();
            gridGroupByField = new GridGroupByField();
            gridGroupByField.FieldName = "TargetHours";
            gridGroupByField.HeaderText = "TargetHours";
            expression.SelectFields.Add(gridGroupByField);

            gridGroupByField = new GridGroupByField();
            gridGroupByField.FieldName = "BudgetHours";
            gridGroupByField.HeaderText = "BudgetHours";
            expression.SelectFields.Add(gridGroupByField);

            gridGroupByField = new GridGroupByField();
            gridGroupByField.FieldName = "TargetHours";
            expression.GroupByFields.Add(gridGroupByField);
            gvBOM.MasterTableView.GroupByExpressions.Add(expression);
        }

        gvBOM.DataSource = dt;

       

        try
        {
            if (Rebind) gvBOM.Rebind();
        }
        catch { }

        SetgvBOMEnable(false);

        SaveIntoTempDt(dt);
       


    }

    private void SetgvBOMEnable(bool IsYes)
    {
        DataTable ds = new DataTable();
        ds = (DataTable)Session["userinfo"];

        foreach (GridDataItem item in gvBOM.Items)
        {

            DropDownList drpBType = (DropDownList)item.FindControl("ddlBType");

            if (drpBType.SelectedIndex == 0) {

                item.BackColor = System.Drawing.Color.LightGray;
            }

            if (Session["type"].ToString() != "am")
            {
                //DropDownList ddlMatItem = (DropDownList)item.FindControl("ddlMatItem");
               

              

                //bom
                string BOMPermission = ds.Rows[0]["BOMPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["BOMPermission"].ToString();
                string stEditBOM = BOMPermission.Length < 2 ? "Y" : BOMPermission.Substring(1, 1);

                TextBox txtLabItem = (TextBox)item.FindControl("txtLabItem");
                TextBox txtCode = (TextBox)item.FindControl("txtCode");
                TextBox txtScope = (TextBox)item.FindControl("txtScope");
                TextBox txtQtyReq = (TextBox)item.FindControl("txtQtyReq");
                TextBox txtUM = (TextBox)item.FindControl("txtUM");
                TextBox txtBudgetUnit = (TextBox)item.FindControl("txtBudgetUnit");
                TextBox txtMatMod = (TextBox)item.FindControl("txtMatMod");
                TextBox txtVendor = (TextBox)item.FindControl("txtVendor");
                TextBox txtHours = (TextBox)item.FindControl("txtHours");
                TextBox txtLabRate = (TextBox)item.FindControl("txtLabRate");
                TextBox txtLabMod = (TextBox)item.FindControl("txtLabMod");
                TextBox txtSDate = (TextBox)item.FindControl("txtSDate");

                TextBox txtMatItem = (TextBox)item.FindControl("txtMatItem");

                if (stEditBOM == "N")
                {
                    txtVendor.ReadOnly = txtHours.ReadOnly = txtLabRate.ReadOnly = txtLabMod.ReadOnly = txtSDate.ReadOnly = txtMatMod.ReadOnly = txtBudgetUnit.ReadOnly = txtUM.ReadOnly = txtQtyReq.ReadOnly = txtScope.ReadOnly = txtCode.ReadOnly = !IsYes;



                    txtMatItem.ReadOnly = !IsYes;

                    if (!IsYes)
                        item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
                    else { item.Attributes["ondblclick"] = ""; }
                }
                else
                {
                    item.Attributes["ondblclick"] = "";
                }
            }
        }
    }

    public string Truncate(string Value, int length)
    {
        if (Value.Length > length)
        {
            Value = Value.Substring(0, length);
        }
        return Value;
    }


    #region TicketEquipment

    private void GetDataEquip()
    {

    }



    #endregion

    private void PopulatePager(int recordCount, int currentPage, Repeater rptPager)
    {
        double dblPageCount = (double)((decimal)recordCount / decimal.Parse(PageSize));
        int pageCount = (int)Math.Ceiling(dblPageCount);
        List<ListItem> pages = new List<ListItem>();
        if (pageCount > 0)
        {
            pages.Add(new ListItem("First", "1", currentPage > 1));
            if (currentPage == 1 && pageCount <= 10)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            else if (currentPage == 1 && pageCount > 10)
            {
                for (int i = 1; i <= 10; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            else if (currentPage != 1 && pageCount <= 10)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            else if (currentPage != 1 && pageCount > 10 && currentPage != pageCount)
            {
                int z = currentPage - 5;
                int x, y;
                if (z < 1)
                {
                    z = 1;
                    for (int i = z; i <= 10; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else if (z >= 1)
                {
                    if (pageCount < currentPage + 4)
                    {
                        y = (z - ((currentPage + 4) - pageCount));
                        x = pageCount;
                    }
                    else
                    {
                        x = currentPage + 4;
                        y = z;
                    }
                    for (int i = y; i <= x; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
            }
            else if (currentPage == pageCount)
            {
                for (int i = pageCount - 9; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            pages.Add(new ListItem("Last", pageCount.ToString(), currentPage < pageCount));
        }
        rptPager.DataSource = pages;
        rptPager.DataBind();
    }

    protected void BudgetPage_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        this.BindBudget(pageIndex);
    }

    //protected void ExpensesPage_Changed(object sender, EventArgs e)
    //{
    //    int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
    //    this.BindExpense(pageIndex);
    //}

    protected void lnkTicketPage_Click(object sender, EventArgs e)
    {
        String OrderBy = Convert.ToString(ViewState["TicketOrderBy"]);
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        this.BindTicketList(OrderBy, pageIndex);
    }


    #region Contact
    protected void btnEditcontact_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["mode"]) == 1)
        {
            int Countrow = 0;
            foreach (GridDataItem row in gvContacts.Items)
            {

                CheckBox check = (CheckBox)row.FindControl("chkEditcontact");
                if (check.Checked)
                {
                    Countrow += 1;
                    ClearContactControl();
                    divAddContact.Visible = lnkContactClose.Visible = lnkContactSave.Visible = true;
                    imgAddContact.Visible = btnEditcontact.Visible = false;
                    lblconinfo.Text = "Edit Contact";

                    Label PhoneID = (Label)row.FindControl("PhoneID");
                    Label lblName = (Label)row.FindControl("lblName");
                    Label lblTitle = (Label)row.FindControl("lblTitle");
                    Label lblPhn = (Label)row.FindControl("lblPhn");
                    Label lblFx = (Label)row.FindControl("lblFx");
                    Label lblCell = (Label)row.FindControl("lblCell");
                    Label lblEmail = (Label)row.FindControl("lblEmail");
                    Label lblContactType = (Label)row.FindControl("lblContactType");

                    txtContcName.Text = lblName.Text;
                    txtTitle.Text = lblName.Text;
                    txtContPhone.Text = lblPhn.Text;
                    txtContFax.Text = lblFx.Text;
                    txtContCell.Text = lblCell.Text;
                    txtContEmail.Text = lblEmail.Text;
                    hdncontactID.Value = PhoneID.Text == null ? "0" : PhoneID.Text;
                    ddlContactType.SelectedIndex = lblContactType.Text == "Location" ? 0 : 1;
                    chkEmailTicket.Checked = Convert.ToBoolean(lblEmail.Text == "True" ? 1 : 0);
                }
            }
            if (Countrow == 0)
            {
                string str = "Please select contact. ";
                ScriptManager.RegisterStartupScript(this, GetType(),
             "error1212", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }

    }

    protected void imgAddContact_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["mode"]) == 1)
        {
            ClearContactControl();
            divAddContact.Visible = lnkContactClose.Visible = lnkContactSave.Visible = true;
            imgAddContact.Visible = btnEditcontact.Visible = false;
            lblconinfo.Text = "Add Contact";
        }
    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        if (txtContcName.Text != string.Empty)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ContactID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Fax", typeof(string));
            dt.Columns.Add("Cell", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("EmailTicket", typeof(bool));

            DataRow dr = dt.NewRow();
            dr["ContactID"] = Convert.ToInt16(hdncontactID.Value == null ? "0" : hdncontactID.Value);
            dr["Name"] = Truncate(txtContcName.Text, 50);
            dr["Phone"] = Truncate(txtContPhone.Text, 22);
            dr["Fax"] = Truncate(txtContFax.Text, 22);
            dr["Cell"] = Truncate(txtContCell.Text, 22);
            dr["Email"] = Truncate(txtContEmail.Text, 50);
            dr["Title"] = Truncate(txtTitle.Text, 50);
            dr["EmailTicket"] = chkEmailTicket.Checked;
            if (ViewState["mode"].ToString() == "1")
            {
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                SubmitContact(dt, Convert.ToInt16(hdncontactID.Value == null ? "0" : hdncontactID.Value));
                Fill_gvContacts();
            }
        }
    }

    private void SubmitContact(DataTable dt, int ContactID = 0)
    {
        try
        {

            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                objPropUser.ContactData = dt;
                objPropUser.ConnConfig = Session["config"].ToString();
                string ContactType = ddlContactType.SelectedItem.ToString();
                int ContactTypeID = ddlContactType.SelectedItem.ToString() == "Customer" ? Convert.ToInt32(hdnCustID.Value) : Convert.ToInt32(hdnLocID.Value);
                int JobID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                objBL_User.AddContactFromProjectScreen(objPropUser, ContactType, ContactTypeID, JobID);
                ClearContactControl();
                divAddContact.Visible = lnkContactClose.Visible = lnkContactSave.Visible = false;
                imgAddContact.Visible = btnEditcontact.Visible = true;
                lblconinfo.Text = "";
                string str = ContactID == 0 ? "Contact added successfully!" : "Contact updated successfully!";
                ScriptManager.RegisterStartupScript(this, GetType(),
            "error1212", "noty({text: '" + str + "',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str1 = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: '" + str1 + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    private void ClearContactControl()
    {
        hdncontactID.Value = "0";
        txtContcName.Text = "";
        txtContPhone.Text = "";
        txtContFax.Text = "";
        txtContCell.Text = "";
        txtContEmail.Text = "";
        txtTitle.Text = "";
        chkEmailTicket.Checked = false;
    }


    protected void LinkButton4_Click(object sender, EventArgs e)
    {
        ClearContactControl();
        divAddContact.Visible = lnkContactClose.Visible = lnkContactSave.Visible = false;
        imgAddContact.Visible = btnEditcontact.Visible = true;
        lblconinfo.Text = "";
    }

    #endregion Contact

    private void Fillterritory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        //ds = objBL_User.getTerritory(objPropUser, 0);
        int refID = Request.QueryString["uid"] != null ? Convert.ToInt32(Request.QueryString["uid"].ToString()) : 0;
        ds = objBL_User.GetSalesPerson(objPropUser, new GeneralFunctions().GetSalesAsigned(), refID, "JOB", "t.Name");

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            //Default Salesperson
            ddlTerr.DataSource = ds.Tables[0];
            ddlTerr.DataTextField = "Name";
            ddlTerr.DataValueField = "ID";
            ddlTerr.DataBind();
            ddlTerr.Items.Insert(0, new ListItem("", ""));
            // Second Salesperson  
            ddlTerr2.DataSource = ds.Tables[0];
            ddlTerr2.DataTextField = "Name";
            ddlTerr2.DataValueField = "ID";
            ddlTerr2.DataBind();
            ddlTerr2.Items.Insert(0, new ListItem("", ""));
        }
    }


    protected void lnkAddPlanner_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            var projectID = Convert.ToInt32(Request.QueryString["uid"]);
            BusinessEntity.Planner objPlanner = new BusinessEntity.Planner();
            BL_Planner objBL_Planner = new BL_Planner();
            objPlanner.ConnConfig = Session["config"].ToString();
            objPlanner.ProjectID = projectID;
            //objPlanner.PlannerID = plannerId;
            var isPlannerExisted = objBL_Planner.IsProjectPlannerExisted(objPlanner);
            if (!isPlannerExisted)
            {
                var plannerID = CalculatePlannerDataNew();
                Response.Redirect("Planner.aspx?projid=" + Request.QueryString["uid"] + "&plnid=" + plannerID.ToString());
                //CalculatePlannerData();
                //Session["PlannerProjectID"] = Convert.ToInt32(Request.QueryString["uid"]);
                //Response.Redirect("AddPlanner.aspx?uid=" + Request.QueryString["uid"] + "&projid="+ Request.QueryString["uid"] + "&plnid=" + plannerID.ToString());
            }
            else
            {
                #region Check on Planner Tab Data
                PlannerCheck();
                #endregion
            }
        }
    }


    private int CalculatePlannerDataNew()
    {
        #region Planner
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objProp_Customer.Type = string.Empty;
        //DataSet ds = objBL_Customer.getJobProjectByJobID(objProp_Customer);

        DataSet BomDS = objBL_Customer.getJobProject_BOM(objProp_Customer);
        DataTable dttBOM = new DataTable();
        dttBOM = BomDS.Tables[0];

        var pSDate = BomDS.Tables[1].Rows[0][0];
        DateTime ProjSDate = DateTime.Now;
        if (pSDate != null && pSDate.ToString() != "")
        {
            ProjSDate = Convert.ToDateTime(pSDate.ToString());
        }

        DataSet MilestonesDS = objBL_Customer.getJobProject_Milestone(objProp_Customer);
        DataTable dttMilestone = new DataTable();
        dttMilestone = MilestonesDS.Tables[0];

        List<PlannerProjectModel> listPlanner = new List<PlannerProjectModel>();

        foreach (DataRow dr in dttBOM.Rows)
        {
            PlannerProjectModel data = new PlannerProjectModel();
            data.ID = Convert.ToInt32(dr["ID"]);
            data.Group = dr["GroupName"].ToString();
            data.Code = string.IsNullOrEmpty(Convert.ToString(dr["CodeDesc"])) ? Convert.ToString(dr["code"])
                : Convert.ToString(dr["code"]) + " - " + Convert.ToString(dr["CodeDesc"]);
            data.fDesc = Convert.ToString(dr["fDesc"]);
            data.Type = "BOM";
            data.Duration = Convert.ToDouble(dr["LabHours"]);
            data.DurationUnit = "h";
            data.StartDate = Convert.ToDateTime(dr["SDate"].ToString());
            data.EndDate = Convert.ToDateTime(dr["EDate"].ToString());
            data.GroupStartDate = Convert.ToDateTime(dr["GroupSDate"].ToString());
            data.GroupEndDate = Convert.ToDateTime(dr["GroupEDate"].ToString());
            data.CodeStartDate = Convert.ToDateTime(dr["CodeSDate"].ToString());
            data.CodeEndDate = Convert.ToDateTime(dr["CodeEDate"].ToString());
            try
            {
                data.VendorID = dr["VendorId"] != null ? Convert.ToInt32(dr["VendorId"]) : 0;
            }
            catch (Exception)
            {
                data.VendorID = 0;
            }

            data.VendorName = dr["Vendor"].ToString();
            data.ActualHours = Convert.ToDouble(dr["ActualHours"]);
            listPlanner.Add(data);
        }

        foreach (DataRow dr in dttMilestone.Rows)
        {
            PlannerProjectModel data = new PlannerProjectModel();
            data.ID = Convert.ToInt32(dr["ID"]);
            data.Group = dr["GroupName"].ToString();
            data.Code = string.IsNullOrEmpty(Convert.ToString(dr["CodeDesc"])) ? Convert.ToString(dr["jcode"])
                : Convert.ToString(dr["jcode"]) + " - " + Convert.ToString(dr["CodeDesc"]);
            var exist = listPlanner.Where(x => x.Code == data.Code && x.Group == data.Group).FirstOrDefault();
            if (exist != null)
            {
                data.StartDate = exist.StartDate;
                data.EndDate = exist.StartDate;
                data.GroupStartDate = exist.GroupStartDate;
                data.GroupEndDate = exist.GroupEndDate;
                data.CodeStartDate = exist.CodeStartDate;
                data.CodeEndDate = exist.CodeEndDate;
            }
            else
            {
                data.StartDate = ProjSDate;
                data.EndDate = ProjSDate;
                data.GroupStartDate = ProjSDate;
                data.GroupEndDate = ProjSDate;
                data.CodeStartDate = ProjSDate;
                data.CodeEndDate = ProjSDate;
            }
            data.fDesc = Convert.ToString(dr["fDesc"]);
            data.Type = "Milestone";
            data.Duration = 0;
            data.DurationUnit = "h";
            data.ActualHours = Convert.ToDouble(dr["ActualHours"]);

            listPlanner.Add(data);
        }

        #region Save Data In Planner Table
        var projectID = Convert.ToInt32(Request.QueryString["uid"]);
        var plannerID = 0;
        objPlanner.ConnConfig = Session["config"].ToString();
        objPlanner.ProjectID = projectID;
        objPlanner.Desc = "Project " + projectID + " Planner";
        objPlanner.Type = "Project";
        plannerID = objBL_Planner.AddPlannerNew(objPlanner);
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Planner Created Successfully! ', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
        #endregion  

        var results = (from p in listPlanner
                       group p by p.Group into g
                       let f = g.FirstOrDefault()
                       where f != null
                       select new
                       {
                           Code = f.Group,
                           StarDate = g.Min(c => c.GroupStartDate),
                           EndDate = g.Max(c => c.GroupEndDate)
                       }).ToList();


        Int32 Pidx = 0;
        // Reset Notes for tasks
        objPlanner.Desc = "";
        foreach (var dt in results)
        {
            #region Add Parent Data
            String Group = dt.Code;
            objPlanner.ParentID = Convert.ToInt32("0");
            objPlanner.TaskName = Group;
            objPlanner.idx = Pidx;
            objPlanner.TaskType = "Group";
            objPlanner.ProjectID = projectID;
            objPlanner.PlannerID = plannerID;
            objPlanner.Duration = 0;
            objPlanner.DurationUnit = "h";
            objPlanner.StartDate = dt.StarDate.Value;
            objPlanner.EndDate = dt.EndDate.Value;
            objPlanner.Summary = true;
            objPlanner.VendorID = 0;
            objPlanner.VendorName = string.Empty;
            objPlanner.ItemRefID = 0;
            String ParentID = objBL_Planner.AddGanttTasksFromMOM(objPlanner);
            Pidx = Pidx + 1;
            #endregion

            //var subOppSequList = (from p in listPlanner
            //                   where p.Group == Group
            //                      select p).ToList();
            var subOppSequList = (from p in listPlanner
                                  where p.Group == Group
                                  group p by new { p.Group, p.Code } into g
                                  let f = g.FirstOrDefault()
                                  where f != null
                                  select new
                                  {
                                      Code = f.Code,
                                      //StarDate = g.Min(c => c.CodeStartDate),
                                      //EndDate = g.Max(c => c.CodeEndDate)
                                      StarDate = f.CodeStartDate,
                                      EndDate = f.CodeEndDate
                                  }).ToList();

            Int32 idx = 0;
            foreach (var sdt in subOppSequList)
            {
                objPlanner.TaskName = sdt.Code;
                objPlanner.ParentID = Convert.ToInt32(ParentID);
                objPlanner.idx = idx;
                objPlanner.TaskType = "OpSequence";
                objPlanner.ProjectID = projectID;
                objPlanner.PlannerID = plannerID;
                objPlanner.Duration = 0;
                objPlanner.DurationUnit = "h";
                objPlanner.StartDate = sdt.StarDate.Value;
                objPlanner.EndDate = sdt.EndDate.Value;
                objPlanner.Summary = true;
                objPlanner.VendorID = 0;
                objPlanner.VendorName = string.Empty;
                objPlanner.ItemRefID = 0;
                String ParentID1 = objBL_Planner.AddGanttTasksFromMOM(objPlanner);
                idx = idx + 1;

                #region Add Sub Task Data
                var subTaskList = (from p in listPlanner
                                   where p.Code == sdt.Code && p.Group == Group
                                   select p).ToList();
                Int32 idx1 = 0;
                foreach (var sdt1 in subTaskList)
                {
                    objPlanner.TaskName = sdt1.fDesc;
                    objPlanner.ParentID = Convert.ToInt32(ParentID1);
                    objPlanner.idx = idx1;
                    objPlanner.TaskType = sdt1.Type;
                    objPlanner.ProjectID = projectID;
                    objPlanner.PlannerID = plannerID;
                    objPlanner.Duration = sdt1.Duration;
                    objPlanner.DurationUnit = sdt1.DurationUnit;
                    objPlanner.StartDate = sdt1.StartDate.HasValue ? sdt1.StartDate.Value : ProjSDate;
                    objPlanner.EndDate = sdt1.EndDate.HasValue ? sdt1.EndDate.Value : ProjSDate;
                    objPlanner.Summary = false;
                    objPlanner.VendorID = sdt1.VendorID;
                    objPlanner.VendorName = sdt1.VendorName;
                    objPlanner.ItemRefID = sdt1.ID;
                    objPlanner.ActualHours = sdt1.ActualHours;
                    objBL_Planner.AddGanttTasksFromMOM(objPlanner);
                    idx1 = idx1 + 1;
                }
                #endregion
            }
        }

        #endregion

        return plannerID;
    }

    protected void btnConfirmEmail_Click(object sender, EventArgs e)
    {
        //btnGenerateReport.Visible = false;
        //btnOpenEmail.Visible = true;
        ModalPopupGenerateReport.Show();
        pnlGenerateReport.Visible = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveToThirdTab3", "MoveToThirdTab();", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHidePreviewInvoice", "ShowHidePreviewInvoice();", true);
    }

    protected void btnOpenEmail_Click(object sender, EventArgs e)
    {
        GenerateSendPDF(sender, e);
        string filename = ViewState["InvoicePDF"] != null && ViewState["InvoicePDF"].ToString() != "" ? ViewState["InvoicePDF"].ToString() : "Invoice.pdf";
        string filefullname = HttpContext.Current.Server.MapPath("~\\TempPDF\\SendInvoice\\" + filename);
        if (File.Exists(filefullname))
        {
            myframe.Attributes["src"] = "Handler.ashx?filename=" + filename;

            ArrayList lstPath = new ArrayList();

            lstPath.Add("Invoice.pdf");

            ViewState["pathmailatt"] = lstPath;
            dlAttachmentsDelete.DataSource = lstPath;
            dlAttachmentsDelete.DataBind();

            ModalPopupOpenEmail.Show();
            pnlOpenEmail.Visible = true;
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveToThirdTab3", "MoveToThirdTab();", true);
    }

    protected void GenerateSendPDF(object sender, EventArgs e)
    {
        if (!chkPrintInvoice.Checked && !chkAIAReport.Checked)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "GenerateReport", "noty({text: 'Please select atleast one to generate report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return;
        }

        byte[] PrintByte = null;
        byte[] Byte = null;
        byte[] Byte2 = null;
        byte[] Byte3 = null;
        byte[] Byte4 = null;
        if (chkPrintInvoice.Checked)
        {
            reportPath = "Reports/Invoices.rdlc";
            Byte = lnkPDF_Click(sender, e);
        }

        if (chkAIAReport.Checked)
        {
            reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/AIAReport.mrt";

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CustomAIAReport"]) && ConfigurationManager.AppSettings["CustomAIAReport"].Contains(".mrt"))
            {
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/" + ConfigurationManager.AppSettings["CustomAIAReport"];
            }

            Byte4 = AIAReport(sender, e, null);
        }
        // Delete Invoice File
        var dtStrNow = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        ViewState["InvoicePDF"] = "Invoice" + dtStrNow + ".pdf";
        string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "Invoice" + dtStrNow + ".pdf");
        if (File.Exists(filename))
            File.Delete(filename);

        string filename1 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "InvoiceReport" + dtStrNow + ".pdf");
        string filename2 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "InvoiceWithTicket" + dtStrNow + ".pdf");
        string filename3 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "BillingInvoice" + dtStrNow + ".pdf");
        string filename4 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "AIAStatmentReport" + dtStrNow + ".pdf");
        if (Byte != null)
        {
            if (File.Exists(filename1))
                File.Delete(filename1);
            using (var fs = new FileStream(filename1, FileMode.Create))
            {
                fs.Write(Byte, 0, Byte.Length);
                fs.Close();
            }
        }
        if (Byte2 != null)
        {
            if (File.Exists(filename2))
                File.Delete(filename2);
            using (var fs = new FileStream(filename2, FileMode.Create))
            {
                fs.Write(Byte2, 0, Byte2.Length);
                fs.Close();
            }
        }
        if (Byte3 != null)
        {
            if (File.Exists(filename3))
                File.Delete(filename3);
            using (var fs = new FileStream(filename3, FileMode.Create))
            {
                fs.Write(Byte3, 0, Byte3.Length);
                fs.Close();
            }
        }
        if (Byte4 != null)
        {
            if (File.Exists(filename4))
                File.Delete(filename4);
            using (var fs = new FileStream(filename4, FileMode.Create))
            {
                fs.Write(Byte4, 0, Byte4.Length);
                fs.Close();
            }
        }
        List<byte[]> pdfByteContent = new List<byte[]>();
        if (File.Exists(filename1))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(filename1));
        if (File.Exists(filename2))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(filename2));
        if (File.Exists(filename3))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(filename3));
        if (File.Exists(filename4))
            pdfByteContent.Add(System.IO.File.ReadAllBytes(filename4));

        if (File.Exists(filename1))
            File.Delete(filename1);
        if (File.Exists(filename2))
            File.Delete(filename2);
        if (File.Exists(filename3))
            File.Delete(filename3);
        if (File.Exists(filename4))
            File.Delete(filename4);

        if (pdfByteContent.Count > 0)
        {
            PrintByte = concatAndAddContent(pdfByteContent);
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                fs.Write(PrintByte, 0, PrintByte.Length);
                fs.Close();
            }
        }
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsC = objBL_User.getControl(objPropUser);

        var invId = string.Empty;
        var invCount = 0;

        foreach (GridDataItem row in gvProgressBilling.Items)
        {
            if (row.ItemType == GridItemType.Item || row.ItemType == GridItemType.AlternatingItem)
            {
                CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                if (chkRow.Checked)
                {
                    Label lblIn = (Label)row.FindControl("lblInvoiceId");
                    if (lblIn != null && !String.IsNullOrEmpty(lblIn.Text))
                    {
                        invCount++;
                        invId = invId + lblIn.Text + ",";
                    }
                }
            }
        }
        if (invId.Length > 0) invId = invId.Remove(invId.Length - 1, 1);
        StringBuilder address = new StringBuilder();

        address.AppendFormat("{0}", txtCustomer.Text);
        address.AppendLine();
        address.AppendLine();
        address.Append("</br></br>");

        if (invCount == 1)
        {
            address.AppendLine("Thank you for giving us the opportunity to serve you. Attached to this email");
            address.AppendFormat("is invoice {0}.", invId);
        }
        else
        {
            address.AppendLine("Thank you for giving us the opportunity to serve you. Please see the attached invoices.");
        }

        //address.AppendLine("Thank you for giving us the opportunity to serve you. Attached to this email");
        //address.AppendFormat("are invoices of project {0}.", Request.QueryString["uid"].ToString());
        address.AppendLine();
        address.AppendLine();
        address.Append("</br></br>");
        address.Append("Kind Regards,");
        address.AppendLine();
        address.AppendLine();
        address.Append("</br></br>");

        if (dsC.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["name"])))
            {
                address.AppendFormat("{0}", dsC.Tables[0].Rows[0]["name"].ToString());
                address.AppendLine();
                address.Append("</br>");
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Address"])))
            {
                address.AppendFormat("{0}", dsC.Tables[0].Rows[0]["Address"].ToString());
                address.AppendLine();
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["city"])))
            {
                address.AppendFormat("{0}, ", dsC.Tables[0].Rows[0]["city"].ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["state"])))
            {
                address.AppendFormat("{0}, ", dsC.Tables[0].Rows[0]["state"].ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["zip"])))
            {
                address.AppendFormat("{0}", dsC.Tables[0].Rows[0]["zip"].ToString());
            }
            address.AppendLine();
            address.Append("</br>");
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Phone"])))
            {
                address.AppendFormat("Phone: {0}", dsC.Tables[0].Rows[0]["Phone"].ToString());
                address.AppendLine();
                address.Append("</br>");
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["fax"])))
            {
                address.AppendFormat("Fax: {0}", dsC.Tables[0].Rows[0]["Phone"].ToString());
                address.AppendLine();
                address.Append("</br>");
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["email"])))
            {
                address.AppendFormat("Email: {0}", dsC.Tables[0].Rows[0]["email"].ToString());
                address.AppendLine();
                address.Append("</br>");
            }
            ViewState["Company"] = address.ToString();
            txtBody.Text = address.ToString();
        }

        txtFrom.Text = WebBaseUtility.GetFromEmailAddress(); ;
        string subject = string.Empty;
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
        DataSet dsloc = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsloc = objBL_User.getLocationByID(objPropUser);
        if (dsloc.Tables[0].Rows.Count > 0)
        {
            txtTo.Text = dsloc.Tables[0].Rows[0]["custom12"].ToString();
            txtCC.Text = dsloc.Tables[0].Rows[0]["custom13"].ToString();

            subject = dsloc.Tables[0].Rows[0]["tag"].ToString();

            //string address = ds.Tables[0].Rows[0]["Name"].ToString() + Environment.NewLine + "<br />";
            //address += dsloc.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine + "<br />";
            //address += dsloc.Tables[0].Rows[0]["locAddress"].ToString() + Environment.NewLine + "<br />";
            //address += dsloc.Tables[0].Rows[0]["locCity"].ToString() + ", " + dsloc.Tables[0].Rows[0]["locState"].ToString() + ", " + dsloc.Tables[0].Rows[0]["locZip"].ToString() + Environment.NewLine + "<br />";
            //address += "Phone: " + dsloc.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine + "<br />";
            //address += "Fax: " + dsloc.Tables[0].Rows[0]["Fax"].ToString() + Environment.NewLine + "<br />";
            //address += "Email: " + dsloc.Tables[0].Rows[0]["EMail"].ToString() + Environment.NewLine + "<br />";
            //address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + "<br />" + "<br />" + address;

        }
        ViewState["subject"] = subject;

        if (invCount == 1)
        {
            txtSubject.Text = "Invoice " + invId + " - " + subject;
        }
        else
        {
            txtSubject.Text = "Invoices - " + subject;
        }
        //string FileName = string.Format("Invoice{0}.pdf", invId);
        ModalPopupSendEmail.Show();
        hdnEmailPopupOpened.Value = "1"; // Opened
        pnlSendEmail.Visible = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveToThirdTab6", "MoveToThirdTab();", true);
    }

    protected void btnEmailInvoice_Click(object sender, EventArgs e)
    {
        if (txtTo.Text.Trim() != string.Empty)
        {
            try
            {
                Mail mail = new Mail();
                mail.From = txtFrom.Text.Trim();
                mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
                if (txtCC.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtCC.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = txtSubject.Text.Trim(); //"Invoice " + Request.QueryString["uid"].ToString() + " - " + ViewState["subject"].ToString();
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                }
                else
                {
                    mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                }

                //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "\\TempPDF\\SendInvoice", "Invoice.pdf");
                string filename = ViewState["InvoicePDF"] != null && ViewState["InvoicePDF"].ToString() != "" ? ViewState["InvoicePDF"].ToString() : "Invoice.pdf";
                string filefullname = HttpContext.Current.Server.MapPath("~\\TempPDF\\SendInvoice\\" + filename);
                ArrayList lst = new ArrayList();
                if (ViewState["pathmailatt"] != null)
                {
                    lst = (ArrayList)ViewState["pathmailatt"];

                    foreach (string strpath in lst)
                    {
                        if (strpath == "Invoice.pdf")
                        {
                            mail.attachmentBytes = System.IO.File.ReadAllBytes(filefullname);
                            mail.FileName = "Invoice.pdf";
                        }
                        else
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }
                // ES-33:Task#2

                mail.DeleteFilesAfterSend = true;
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                //mail.Send();

                EmailLog emailLog = new EmailLog();
                emailLog.ConnConfig = Session["config"].ToString();
                emailLog.Function = "Progress Billing";
                emailLog.Screen = "Project";
                emailLog.Username = Session["Username"].ToString();
                emailLog.SessionNo = Guid.NewGuid().ToString();
                emailLog.Ref = 0;
                emailLog.Refs = new List<int>();

                // Save Email fields in wip header
                DataTable dtNew = (DataTable)Session["InvoiceSrch"];
                string _ref = "";
                foreach (DataRow _dr in dtNew.Rows)
                {
                    if (dtNew.Rows.IndexOf(_dr) == 0)
                        _ref = _dr["Ref"].ToString();
                    else
                        _ref = _ref + ", " + _dr["Ref"].ToString();

                    emailLog.Refs.Add((int)_dr["Ref"]);
                }

                Tuple<int, string, string> emailSendError = null;
                StringBuilder sbdSentError = new StringBuilder();

                MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
                emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog);
                if (emailSendError != null && emailSendError.Item1 == 1)
                {
                    sbdSentError.Append(emailSendError.Item2);
                }
                else
                {
                    emailSendError = mail.SendSingleMailWithLog(mimeMessage, true, emailLog);
                    if (emailSendError != null)
                    {
                        if (emailSendError.Item1 == 1)
                        {
                            sbdSentError.Append(emailSendError.Item2);
                        }
                        else
                        {
                            sbdSentError.Append(emailSendError.Item2);
                        }
                    }
                }

                if (File.Exists(filefullname))
                {
                    myframe.Attributes["src"] = "";
                    File.Delete(filefullname);
                }


                DataTable tblWIPHeader = new DataTable();
                tblWIPHeader.Columns.Add("Id", typeof(string));
                tblWIPHeader.Columns.Add("JobId", typeof(int));
                tblWIPHeader.Columns.Add("SendTo", typeof(string));
                tblWIPHeader.Columns.Add("SendBy", typeof(string));
                tblWIPHeader.Columns.Add("SendOn", typeof(DateTime));

                DataRow drWIP = tblWIPHeader.NewRow();

                drWIP["Id"] = _ref;
                drWIP["JobId"] = Convert.ToInt32(Request.QueryString["uid"].ToString());
                drWIP["SendTo"] = txtTo.Text;
                drWIP["SendBy"] = Session["username"].ToString();
                drWIP["SendOn"] = DateTime.Now;
                tblWIPHeader.Rows.Add(drWIP);

                DataSet tblWIPr = new DataSet();
                tblWIPr.Tables.Add(tblWIPHeader);

                objProp_Customer.WIP = tblWIPr;
                objProp_Customer.ConnConfig = Session["config"].ToString();

                DataSet ds = objBL_Customer.UpdateMailFields(objProp_Customer);
                DataTable tblProgressBilling = ds.Tables[0];
                if (tblProgressBilling.Rows.Count > 0)
                {
                    gvProgressBilling.DataSource = tblProgressBilling;
                    gvProgressBilling.DataBind();
                }
                this.ModalPopupSendEmail.Hide();
                hdnEmailPopupOpened.Value = "";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                //this.ModalPopupSendEmail.Hide();
                //hdnEmailPopupOpened.Value = "1";
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {

        ModalPopupGenerateReport.Show();
        pnlGenerateReport.Visible = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveToThirdTab3", "MoveToThirdTab();", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideGenerateInvoice", "ShowHideGenerateInvoice();", true);
    }

    protected void btnGenerateAIAReport_Click(object sender, EventArgs e)
    {

        reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/AIAReport.mrt";

        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CustomAIAReport"]) && ConfigurationManager.AppSettings["CustomAIAReport"].Contains(".mrt"))
        {
            reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/" + ConfigurationManager.AppSettings["CustomAIAReport"];
        }

        byte[] PrintByte = AIAReport(sender, e, null);
        if (PrintByte != null)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=AIAReport.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (PrintByte.Length).ToString());
            Response.BinaryWrite(PrintByte);
            Response.End();
        }

    }

    protected void btnGenerateReport_Click(object sender, EventArgs e)
    {
        if (!chkPrintInvoice.Checked && !chkAIAReport.Checked)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "GenerateReport", "noty({text: 'Please select at least one to generate report.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        else
        {
            byte[] PrintByte = null;
            byte[] Byte = null;
            byte[] Byte2 = null;
            byte[] Byte3 = null;
            byte[] Byte4 = null;
            if (chkPrintInvoice.Checked)
            {
                reportPath = "Reports/Invoices.rdlc";
                Byte = lnkPDF_Click(sender, e);
            }

            if (chkAIAReport.Checked)
            {
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/AIAReport.mrt";

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CustomAIAReport"]) && ConfigurationManager.AppSettings["CustomAIAReport"].Contains(".mrt"))
                {
                    reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/" + ConfigurationManager.AppSettings["CustomAIAReport"];
                }

                Byte4 = AIAReport(sender, e, null);
            }

            // Delete Invoice File
            var dtStrNow = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "Invoice" + dtStrNow + ".pdf");
            if (File.Exists(filename))
                File.Delete(filename);

            string filename1 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "InvoiceReport" + dtStrNow + ".pdf");
            string filename2 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "InvoiceWithTicket" + dtStrNow + ".pdf");
            string filename3 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "BillingInvoice" + dtStrNow + ".pdf");
            string filename4 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "AIAStatmentReport" + dtStrNow + ".pdf");
            // Delete Invoice File
            //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "Invoice.pdf");
            //if (File.Exists(filename))
            //    File.Delete(filename);

            //string filename1 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "InvoiceReport.pdf");
            //string filename2 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "InvoiceWithTicket.pdf");
            //string filename3 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "BillingInvoice.pdf");
            //string filename4 = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "AIAStatmentReport.pdf");
            if (Byte != null)
            {
                if (File.Exists(filename1))
                    File.Delete(filename1);
                using (var fs = new FileStream(filename1, FileMode.Create))
                {
                    fs.Write(Byte, 0, Byte.Length);
                    fs.Close();
                }
            }
            if (Byte2 != null)
            {
                if (File.Exists(filename2))
                    File.Delete(filename2);
                using (var fs = new FileStream(filename2, FileMode.Create))
                {
                    fs.Write(Byte2, 0, Byte2.Length);
                    fs.Close();
                }
            }
            if (Byte3 != null)
            {
                if (File.Exists(filename3))
                    File.Delete(filename3);
                using (var fs = new FileStream(filename3, FileMode.Create))
                {
                    fs.Write(Byte3, 0, Byte3.Length);
                    fs.Close();
                }
            }
            if (Byte4 != null)
            {
                if (File.Exists(filename4))
                    File.Delete(filename4);
                using (var fs = new FileStream(filename4, FileMode.Create))
                {
                    fs.Write(Byte4, 0, Byte4.Length);
                    fs.Close();
                }
            }
            List<byte[]> pdfByteContent = new List<byte[]>();
            if (File.Exists(filename1))
                pdfByteContent.Add(System.IO.File.ReadAllBytes(filename1));
            if (File.Exists(filename2))
                pdfByteContent.Add(System.IO.File.ReadAllBytes(filename2));
            if (File.Exists(filename3))
                pdfByteContent.Add(System.IO.File.ReadAllBytes(filename3));
            if (File.Exists(filename4))
                pdfByteContent.Add(System.IO.File.ReadAllBytes(filename4));

            if (pdfByteContent.Count > 0)
            {
                PrintByte = concatAndAddContent(pdfByteContent);

                // Delete all 
                if (File.Exists(filename))
                    File.Delete(filename);
                if (File.Exists(filename1))
                    File.Delete(filename1);
                if (File.Exists(filename2))
                    File.Delete(filename2);
                if (File.Exists(filename3))
                    File.Delete(filename3);
                if (File.Exists(filename4))
                    File.Delete(filename4);

                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (PrintByte.Length).ToString());
                Response.BinaryWrite(PrintByte);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "GenerateReport", "noty({text: 'Report Generated successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                // ScriptManager.RegisterStartupScript(this, Page.GetType(), "GenerateReport", "noty({text: 'There is no data to generate report.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }

        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveToThirdTab1", "MoveToThirdTab();", true);


    }

    protected byte[] AIAReport(object sender, EventArgs e, int? WIPID)
    {
        byte[] buffer = null;

        DataTable dt = new DataTable();
        dt.Columns.Add("Ref", typeof(int));

        foreach (GridDataItem row in gvProgressBilling.Items)
        {
            if (row.ItemType == GridItemType.Item || row.ItemType == GridItemType.AlternatingItem)
            {
                CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                if (chkRow.Checked)
                {
                    DataRow drInv = dt.NewRow();
                    Label lblIn = (Label)row.FindControl("lblID");
                    if (lblIn != null && !String.IsNullOrEmpty(lblIn.Text))
                    {
                        drInv["Ref"] = Convert.ToInt32(lblIn.Text);
                        dt.Rows.Add(drInv);
                    }
                }
            }
            if (gvProgressBilling.Items.Count - 1 == row.ItemIndex)
            {
                if (dt.Rows.Count == 0)
                {
                    DataRow drInv = dt.NewRow();
                    Label lblIn = (Label)row.FindControl("lblID");
                    if (lblIn != null && !String.IsNullOrEmpty(lblIn.Text))
                    {
                        drInv["Ref"] = Convert.ToInt32(lblIn.Text);
                        dt.Rows.Add(drInv);
                    }
                }
            }
        }
        DataTable dtInv = dt.Clone();
        if (dt.Rows.Count > 0)
        {
            dtInv.ImportRow(dt.Rows[dt.Rows.Count - 1]);

            if (dtInv.Rows.Count > 0)
            {
                if (Request.QueryString["uid"] != null)
                    objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objProp_Customer.ConnConfig = Session["config"].ToString();

                for (int i = 0; i < dtInv.Rows.Count; i++)
                {
                    objProp_Customer.WIPID = Convert.ToInt32(dtInv.Rows[i][0]);
                    DataSet ds = new DataSet();
                    ds = objBL_Customer.GetAIAReportData(objProp_Customer);
                    StiReport report = new StiReport();
                    report.Load(reportPath);
                    report.Compile();

                    DataSet AIAHeader = new DataSet();
                    DataTable hTable = ds.Tables[0].Copy();
                    hTable.TableName = "AIAHeader";
                    AIAHeader.Tables.Add(hTable);
                    AIAHeader.DataSetName = "AIAHeader";

                    DataSet AIADetails = new DataSet();
                    DataTable dTable = ds.Tables[1].Copy();
                    dTable.TableName = "AIADetails";
                    AIADetails.Tables.Add(dTable);
                    AIADetails.DataSetName = "AIADetails";

                    report.RegData("AIAHeader", AIAHeader);
                    report.RegData("AIADetails", AIADetails);
                    report.Dictionary.Synchronize();
                    report.Render();

                    var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                    var service = new Stimulsoft.Report.Export.StiPdfExportService();
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    service.ExportTo(report, stream, settings);
                    buffer = stream.ToArray();

                    string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/SendInvoice", "AIA_" + i.ToString() + ".pdf");

                    if (buffer != null)
                    {
                        if (File.Exists(filename))
                            File.Delete(filename);
                        using (var fs = new FileStream(filename, FileMode.Create))
                        {
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Close();
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return buffer;
    }

    protected byte[] lnkPDF_Click(object sender, EventArgs e)
    {
        byte[] buffer = null;
        try
        {
            DataTable dt = new DataTable();

            DataTable dtInv = new DataTable();
            dtInv.Columns.Add("Ref", typeof(int));

            foreach (GridDataItem row in gvProgressBilling.Items)
            {
                //if (row.RowType == DataControlRowType.DataRow)
                if (row.ItemType == GridItemType.Item || row.ItemType == GridItemType.AlternatingItem)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                    if (chkRow.Checked)
                    {
                        DataRow drInv = dtInv.NewRow();
                        Label lblIn = (Label)row.FindControl("lblInvoiceId");
                        if (lblIn != null && !String.IsNullOrEmpty(lblIn.Text))
                        {
                            drInv["Ref"] = Convert.ToInt32(lblIn.Text);
                            dtInv.Rows.Add(drInv);
                        }
                    }
                }

            }
            var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
            Session["InvoiceSrch"] = dtInv;
            if (reportFormat.Equals("RDLC"))
            {
                if (dtInv.Rows.Count > 0)
                {
                    ReportViewer rvInvoices = new ReportViewer();

                    PrintInvoices(rvInvoices);

                    buffer = ExportReportToPDF("", rvInvoices);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            else if (reportFormat.Equals("MRT"))
            {
                if (dtInv.Rows.Count > 0)
                {
                    StiWebViewer rvInvoices = new StiWebViewer();

                    List<byte[]> invoicesToPrint = PrintInvoices(rvInvoices);

                    string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "Invoices.pdf");

                    if (invoicesToPrint != null)
                    {
                        buffer = concatAndAddContent(invoicesToPrint);
                    }
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

        return buffer;
    }

    private List<byte[]> PrintInvoices(StiWebViewer rvInvoices)
    {
        // Export to PDF
        List<byte[]> invoicesAsBytes = new List<byte[]>();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            DataSet dsEquip = new DataSet();
            DataTable dtInstallationItems = new DataTable();

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

                DataTable dt = (DataTable)ViewState["InvoiceReport"];
                DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
                int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
                DataTable _dtInvItems = GetInvoiceItems(refId);

                DataSet companyLogo = new DataSet();
                companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());

                DataTable cTable = BuildCompanyDetailsTable();
                var cRow = cTable.NewRow();
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

                // Template name
                string configRepostName = ConfigurationManager.AppSettings["InvoiceReport"].ToString();

                // Load for Port City Installation template
                bool pceInstallation = false;

                if (ConfigurationManager.AppSettings["CustomerName"] == "PCE")
                {
                    if (_dtInvoice.Rows.Count > 0)
                    {
                        var _objUser = new User();
                        _objUser.ConnConfig = Session["config"].ToString();
                        _objUser.SearchBy = string.Empty;

                        _objUser.LocID = Convert.ToInt32(_dtInvoice.Rows[0]["Loc"]);
                        _objUser.InstallDate = string.Empty;
                        _objUser.ServiceDate = string.Empty;
                        _objUser.Price = string.Empty;
                        _objUser.Manufacturer = string.Empty;
                        _objUser.Status = -1;
                        _objUser.building = "All";

                        dsEquip = objBL_User.getElev(_objUser);

                        if (_dtInvoice.Rows[0]["TypeName"] != null && _dtInvoice.Rows[0]["TypeName"].ToString() == "Installation")
                        {
                            pceInstallation = true;
                            configRepostName = "InstallationInvoicesDefault-PortCity.mrt";
                            dtInstallationItems = GetPCEInstallationInvoiceItems(_dtInvoice, _dtInvItems);
                        }
                    }
                }

                string reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + configRepostName);
                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                report.RegData("Invoices", _dtInvoice);
                report.RegData("CompanyDetails", cTable);
                report.RegData("Invoice_dtInvoice", ds.Tables[0]);
                report.RegData("Ticket_Company", dsC.Tables[0]);

                if (pceInstallation)
                {
                    report.RegData("InvoiceItems", dtInstallationItems);
                }
                else
                {
                    report.RegData("InvoiceItems", _dtInvItems);
                }

                if (report.DataSources.Contains("Equipments") && dsEquip.Tables.Count > 0)
                {
                    report.RegData("Equipments", dsEquip.Tables[0]);
                }

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

    private void PrintInvoices(ReportViewer rvInvoices)
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
            rvInvoices.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", _dtInvoice));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
            rvInvoices.LocalReport.ReportPath = reportPath;
            rvInvoices.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            if (reportPath == "Reports/Invoices.rdlc" || reportPath == "Reports/InvoicesForAdamMaintenance.rdlc" || reportPath == "Reports/InvoicesForAdamBill.rdlc")
            {
                param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));// ViewState["IsGst"].ToString() ##Needtochange
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

    protected DataTable BuildCompanyDetailsTable()
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

    public static byte[] concatAndAddContent(List<byte[]> pdfByteContent)
    {
        MemoryStream ms = new MemoryStream();
        iTextSharp.text.Document doc = new iTextSharp.text.Document();
        iTextSharp.text.pdf.PdfSmartCopy copy = new iTextSharp.text.pdf.PdfSmartCopy(doc, ms);

        doc.Open();

        //Loop through each byte array
        foreach (var p in pdfByteContent)
        {
            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(p);
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

    protected byte[] lnkPDFTI_Click(object sender, EventArgs e)
    {
        byte[] allbyte = null;
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

            DataTable dtSelectedInv = new DataTable();
            dtSelectedInv.Columns.Add("Ref", typeof(int));
            foreach (GridDataItem row in gvProgressBilling.Items)
            {
                if (row.ItemType == GridItemType.Item || row.ItemType == GridItemType.AlternatingItem)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                    if (chkRow.Checked)
                    {
                        DataRow drInv = dtSelectedInv.NewRow();
                        Label lblIn = (Label)row.FindControl("lblInvoiceId");
                        if (lblIn != null && !String.IsNullOrEmpty(lblIn.Text))
                        {
                            drInv["Ref"] = Convert.ToInt32(lblIn.Text);
                            dtSelectedInv.Rows.Add(drInv);
                        }
                    }
                }
            }

            Session["InvoiceSrch"] = dtSelectedInv;

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
                List<byte[]> lstbyte = new List<byte[]>();

                foreach (DataRow drow in _dtInvoice.Rows)
                {
                    ReportViewer rvInvoices = new ReportViewer();
                    int invoiceNo = (int)drow[1];
                    ViewState["invoiceNo"] = invoiceNo;
                    string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();
                    if (Report != null && Path.GetExtension(Report) == ".mrt")
                    {
                        //PrintInvoiceWithTicketMRT(invoiceNo, Report);
                        List<byte[]> invoicesToPrint = GetInvoiceWithTicketReport(invoiceNo, Report);
                        if (invoicesToPrint != null)
                        {
                            byte[] buffer1 = null;
                            buffer1 = concatAndAddContent(invoicesToPrint);
                            lstbyte.Add(buffer1);
                        }
                    }
                    else
                    {
                        PrintInvoicesTicket(rvInvoices, invoiceNo);
                        array = ExportReportToPDF("", rvInvoices);

                        lstbyte.Add(array);
                    }
                }

                allbyte = addProject.concatAndAddContent(lstbyte);
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

        return allbyte;
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
                    i++;
                }
            }
        }

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
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString())); //##Needtochange
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

    private void ClearWIPHeader()
    {
        txtProgressBilling.Text = "";
        txtInvoiceNO.Text = "";
        //txtBillingDate.Text = DateTime.Now.ToString("M/dd/yyyy");
        //txtPeriodDate.Text = DateTime.Now.ToString("M/dd/yyyy");
        //txtRevisionDate.Text = DateTime.Now.ToString("M/dd/yyyy");
        //txtArchitectName.Text = "";
        //txtArchitectAdress.Text = "";
        btnSave.Text = "Save WIP";
        if (!String.IsNullOrEmpty(hdnGlobal_Terms.Value))
            ddlTerms.SelectedValue = hdnGlobal_Terms.Value;
        txtSalesTax.Text = hdnGlobal_SalesTax.Value;
    }

    private void FillBillCodes()
    {
        DataSet ds = new DataSet();

        objJob.ConnConfig = Session["config"].ToString();
        ds = objBL_Job.GetInvService(objJob);

        DataRow dr = ds.Tables[0].NewRow();
        dr["ID"] = 0;
        dr["label"] = "Select";
        ds.Tables[0].Rows.InsertAt(dr, 0);
        ds.Tables[0].Columns["label"].ColumnName = "BillType";

        dtBillingCodeData = ds.Tables[0];
    }

    private void AssignBillingCodeInGrid(string ID)
    {
        foreach (GridDataItem item in gvWIPs.Items)
        {
            DropDownList _ddlBillingCode = ((DropDownList)item.FindControl("ddlBillingCode"));
            _ddlBillingCode.SelectedIndex = _ddlBillingCode.Items.IndexOf(_ddlBillingCode.Items.FindByValue(ID));
        }
    }


    private void GetWIP()
    {
        //todo
        // SetupCanadaCompanyUI();
        ClearWIPHeader();
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();

        if (Request.QueryString["uid"] != null)
            objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());

        if (!String.IsNullOrEmpty(hdnWIPID.Value))
            objProp_Customer.WIPID = Convert.ToInt32(hdnWIPID.Value);
        else
            objProp_Customer.WIPID = null;

        ds = objBL_Customer.GetWIP(objProp_Customer);
        DataTable tblWIPHeader = ds.Tables[0];
        DataTable tblWIPDetails = ds.Tables[1];




        if (String.IsNullOrEmpty(hdnWIPID.Value)) // Add Mode
        {
            DataTable tblWIPDetailsClone = ds.Tables[1].Clone();
            foreach (GridDataItem gr in gvMilestones.Items)
            {
                DataRow drWIPDet = tblWIPDetailsClone.NewRow();
                if (((HiddenField)gr.FindControl("hdnLine")).Value != "")
                {
                    drWIPDet["Line"] = Convert.ToInt32(((HiddenField)gr.FindControl("hdnLine")).Value);
                    drWIPDet["RowNo"] = drWIPDet["Line"];
                }
                else
                {
                    drWIPDet["Line"] = gr.RowIndex + 1;
                    drWIPDet["RowNo"] = gr.RowIndex + 1;
                }
                var maxID = tblWIPDetails.Compute("MAX(ID)", "Line = " + Convert.ToString(drWIPDet["Line"]));
                Decimal PreTotalBilled = 0;
                if (maxID.ToString() != "")
                {
                    if (tblWIPDetails.Select("ID = " + maxID).Count() > 0)
                    {
                        DataRow[] dr = tblWIPDetails.Select("ID = " + maxID);
                        PreTotalBilled = Convert.ToDecimal(dr[0]["TotalCompletedAndStored"]);
                    }
                }
                // var PreTotalBilled = tblWIPDetails.Compute("MAX(TotalCompletedAndStored)", "Line = " + Convert.ToString(drWIPDet["Line"]));
                var RetainageAmount = tblWIPDetails.Compute("SUM(RetainageAmount)", "Line = " + Convert.ToString(drWIPDet["Line"]));

                TextBox fdec = (TextBox)gr.FindControl("txtScope");
                if (fdec != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(fdec.Text)))
                        drWIPDet["WIPDesc"] = Convert.ToString(fdec.Text);
                }
                drWIPDet["ContractAmount"] = 0;
                TextBox txtAmount = (TextBox)gr.FindControl("txtAmount");
                if (txtAmount != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(txtAmount.Text)))
                        drWIPDet["ContractAmount"] = Convert.ToDecimal(txtAmount.Text);
                }

                drWIPDet["PresentlyStored"] = 0;
                drWIPDet["ChangeOrder"] = 0;
                CheckBox chkChangeOrder = (CheckBox)gr.FindControl("chkChangeOrder");
                if (chkChangeOrder != null)
                {
                    if (chkChangeOrder.Checked == true)
                    {
                        drWIPDet["ChangeOrder"] = drWIPDet["ContractAmount"];
                        drWIPDet["ContractAmount"] = 0;
                    }
                }

                drWIPDet["ScheduledValues"] = 0;
                drWIPDet["PreviousBilled"] = PreTotalBilled;
                drWIPDet["CompletedThisPeriod"] = 0;
                drWIPDet["TotalCompletedAndStored"] = 0;
                drWIPDet["PerComplete"] = 0;
                drWIPDet["BalanceToFinsh"] = 0;
                drWIPDet["RetainageAmount"] = 0;
                drWIPDet["RetainagePer"] = 0;
                if (!String.IsNullOrEmpty(Convert.ToString(RetainageAmount)))
                    drWIPDet["RetainageCumAmount"] = Convert.ToDouble(RetainageAmount);
                drWIPDet["TotalBilled"] = 0;
                if (!String.IsNullOrEmpty(hdnUnrecognizedRevenue.Value))
                    drWIPDet["BillingCode"] = Convert.ToInt32(hdnUnrecognizedRevenue.Value);
                else
                    drWIPDet["BillingCode"] = 0;
                drWIPDet["Taxable"] = false;
                if (isCanada)
                {
                    drWIPDet["GSTable"] = false;
                    drWIPDet["PSTAmount"] = 0;
                    drWIPDet["GSTAmount"] = 0;
                }
                drWIPDet["PerBilled"] = 0;
                if (!String.IsNullOrEmpty(Convert.ToString(drWIPDet["PreviousBilled"])) && !String.IsNullOrEmpty(Convert.ToString(drWIPDet["ContractAmount"])) && Convert.ToDecimal(drWIPDet["ContractAmount"]) != 0)
                {
                    drWIPDet["PerBilled"] = ((Convert.ToDecimal(drWIPDet["PreviousBilled"])) / (Convert.ToDecimal(drWIPDet["ContractAmount"]))) * 100;
                }
                TextBox txtCode = (TextBox)gr.FindControl("txtCode");
                if (txtCode != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(txtCode.Text)))
                        drWIPDet["jCode"] = Convert.ToString(txtCode.Text);
                }
                TextBox txtGroup = (TextBox)gr.FindControl("txtGroup");
                if (txtGroup != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(txtGroup.Text)))
                        drWIPDet["GroupName"] = Convert.ToString(txtGroup.Text);
                }

                TextBox lblCodeDesc = (TextBox)gr.FindControl("lblCodeDesc");
                if (lblCodeDesc != null)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(lblCodeDesc.Text)))
                        drWIPDet["CodeDesc"] = Convert.ToString(lblCodeDesc.Text);
                }

                tblWIPDetailsClone.Rows.Add(drWIPDet);
            }
            tblWIPDetailsClone.DefaultView.Sort = "RowNo ASC";
            gvWIPs.DataSource = tblWIPDetailsClone;
            gvWIPs.DataBind();

            // Progress Billings Tab
            if (tblWIPHeader.Rows.Count > 0)
            {
                gvProgressBilling.DataSource = tblWIPHeader;
                gvProgressBilling.DataBind();
            }
            else
            {
                gvProgressBilling.DataSource = null;
                gvProgressBilling.DataBind();
            }
        }
        else // Edit Mode
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                hdnWIPID.Value = ds.Tables[0].Rows[0]["Id"].ToString();
                txtProgressBilling.Text = ds.Tables[0].Rows[0]["ProgressBillingNo"].ToString();
                txtInvoiceNO.Text = ds.Tables[0].Rows[0]["InvoiceId"].ToString();
                txtBillingDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["BillingDate"].ToString()).ToString("M/dd/yyyy");
                //ddlApplicationStatus.SelectedValue = ds.Tables[0].Rows[0]["ApplicationStatusId"].ToString();
                txtInvoiceRemark.Text = ds.Tables[0].Rows[0]["invoiceDescription"].ToString();
                ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["Terms"].ToString();
                txtSalesTax.Text = ds.Tables[0].Rows[0]["SalesTax"].ToString();
                //txtArchitectName.Text = ds.Tables[0].Rows[0]["ArchitectName"].ToString().Trim();
                //txtArchitectAdress.Text = ds.Tables[0].Rows[0]["ArchitectAddress"].ToString();
                txtPeriodDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PeriodDate"].ToString()).ToString("M/dd/yyyy");
                txtRevisionDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["RevisionDate"].ToString()).ToString("M/dd/yyyy");
                btnSave.Text = "Edit WIP";
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                //for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                //{
                //    ds.Tables[1].Rows[i]["RetainageCumAmount"] = 0;
                //}
                gvWIPs.DataSource = ds.Tables[1];
                gvWIPs.DataBind();
            }
            else
            {
                tblWIPDetails = ds.Tables[1];
                foreach (GridDataItem gr in gvMilestones.Items)
                {
                    DataRow drWIPDet = tblWIPDetails.NewRow();
                    if (((HiddenField)gr.FindControl("hdnLine")).Value != "")
                    {
                        drWIPDet["Line"] = Convert.ToInt16(((HiddenField)gr.FindControl("hdnLine")).Value);
                        drWIPDet["RowNo"] = drWIPDet["Line"];
                    }
                    else
                    {
                        drWIPDet["Line"] = gr.RowIndex + 1;
                        drWIPDet["RowNo"] = gr.RowIndex + 1;
                    }
                    TextBox fdec = (TextBox)gr.FindControl("txtScope");
                    if (!string.IsNullOrEmpty(Convert.ToString(fdec)))
                        drWIPDet["WIPDesc"] = Convert.ToString(fdec.Text);
                    TextBox txtAmount = (TextBox)gr.FindControl("txtAmount");
                    if (!string.IsNullOrEmpty(Convert.ToString(txtAmount)))
                        drWIPDet["ContractAmount"] = Convert.ToDecimal(txtAmount.Text);
                    else
                        drWIPDet["ContractAmount"] = 0;

                    drWIPDet["ChangeOrder"] = 0;
                    drWIPDet["ScheduledValues"] = 0;
                    drWIPDet["PreviousBilled"] = 0;
                    drWIPDet["CompletedThisPeriod"] = 0;
                    drWIPDet["TotalCompletedAndStored"] = 0;
                    drWIPDet["PerComplete"] = 0;
                    drWIPDet["BalanceToFinsh"] = 0;
                    drWIPDet["RetainagePer"] = 0;
                    drWIPDet["RetainageAmount"] = 0;
                    drWIPDet["TotalBilled"] = 0;
                    if (!String.IsNullOrEmpty(hdnUnrecognizedRevenue.Value))
                        drWIPDet["BillingCode"] = Convert.ToInt32(hdnUnrecognizedRevenue.Value);
                    drWIPDet["Taxable"] = false;
                    drWIPDet["GSTable"] = false;
                    drWIPDet["PSTAmount"] = 0;
                    drWIPDet["GSTAmount"] = 0;
                    tblWIPDetails.Rows.Add(drWIPDet);
                }
                tblWIPDetails.DefaultView.Sort = "RowNo ASC";
                gvWIPs.DataSource = tblWIPDetails;
                gvWIPs.DataBind();
            }
        }


    }

    protected void SaveWIP_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet tblWIPr = new DataSet();
            string ValidationMsg = "";
            // WIP Header Part
            DataTable tblWIPHeader = new DataTable();
            tblWIPHeader.Columns.Add("Id", typeof(int));
            tblWIPHeader.Columns.Add("JobId", typeof(int));
            tblWIPHeader.Columns.Add("ProgressBillingNo", typeof(string));
            tblWIPHeader.Columns.Add("InvoiceId", typeof(int)).AllowDBNull = true;
            tblWIPHeader.Columns.Add("BillingDate", typeof(DateTime));
            tblWIPHeader.Columns.Add("ApplicationStatusId", typeof(int));
            tblWIPHeader.Columns.Add("Terms", typeof(int));
            tblWIPHeader.Columns.Add("SalesTax", typeof(Decimal));
            tblWIPHeader.Columns.Add("PeriodDate", typeof(DateTime));
            tblWIPHeader.Columns.Add("RevisionDate", typeof(DateTime));
            tblWIPHeader.Columns.Add("Remarks", typeof(string));
            //tblWIPHeader.Columns.Add("ArchitectName", typeof(string));
            //tblWIPHeader.Columns.Add("ArchitectAddress", typeof(string));

            DataRow drWIP = tblWIPHeader.NewRow();
            if (!string.IsNullOrEmpty(hdnWIPID.Value))
            {
                drWIP["Id"] = Convert.ToInt32(hdnWIPID.Value);
            }
            drWIP["JobId"] = Convert.ToInt32(Request.QueryString["uid"].ToString());
            drWIP["ProgressBillingNo"] = txtProgressBilling.Text;
            drWIP["InvoiceId"] = DBNull.Value;
            drWIP["BillingDate"] = Convert.ToDateTime(txtBillingDate.Text);
            //drWIP["ApplicationStatusId"] = 1;// Convert.ToInt32(ddlApplicationStatus.SelectedValue);
            drWIP["Terms"] = Convert.ToInt32(ddlTerms.SelectedValue);
            if (!string.IsNullOrEmpty(txtSalesTax.Text))
                drWIP["SalesTax"] = Convert.ToDecimal(txtSalesTax.Text);
            drWIP["PeriodDate"] = Convert.ToDateTime(txtPeriodDate.Text);
            drWIP["RevisionDate"] = Convert.ToDateTime(txtRevisionDate.Text);
            drWIP["Remarks"] = txtInvoiceRemark.Text;
            //drWIP["ArchitectName"] = txtArchitectName.Text;
            //drWIP["ArchitectAddress"] = txtArchitectAdress.Text;

            tblWIPHeader.Rows.Add(drWIP);
            tblWIPr.Tables.Add(tblWIPHeader);

            // WIP Details Part
            DataTable tblWIPDetails = new DataTable();
            tblWIPDetails.Columns.Add("Id", typeof(int));
            tblWIPDetails.Columns.Add("WIPId", typeof(int));
            tblWIPDetails.Columns.Add("Line", typeof(int));
            tblWIPDetails.Columns.Add("WIPDesc", typeof(string));
            tblWIPDetails.Columns.Add("ContractAmount", typeof(decimal));
            tblWIPDetails.Columns.Add("ChangeOrder", typeof(decimal));
            tblWIPDetails.Columns.Add("ScheduledValues", typeof(decimal));
            tblWIPDetails.Columns.Add("PreviousBilled", typeof(decimal));
            tblWIPDetails.Columns.Add("CompletedThisPeriod", typeof(decimal));
            tblWIPDetails.Columns.Add("PresentlyStored", typeof(decimal));
            tblWIPDetails.Columns.Add("TotalCompletedAndStored", typeof(decimal));
            tblWIPDetails.Columns.Add("PerComplete", typeof(decimal));
            tblWIPDetails.Columns.Add("BalanceToFinsh", typeof(decimal));
            tblWIPDetails.Columns.Add("RetainagePer", typeof(decimal));
            tblWIPDetails.Columns.Add("RetainageAmount", typeof(decimal));
            tblWIPDetails.Columns.Add("TotalBilled", typeof(decimal));
            tblWIPDetails.Columns.Add("BillingCode", typeof(int));
            tblWIPDetails.Columns.Add("Taxable", typeof(bool));
            tblWIPDetails.Columns.Add("GSTable", typeof(bool));



            foreach (GridDataItem item in gvWIPs.Items)
            {
                DataRow drWIPDet = tblWIPDetails.NewRow();

                if ((HiddenField)item.FindControl("hdnID") != null && ((HiddenField)item.FindControl("hdnID")).Value != "")
                    drWIPDet["Id"] = Convert.ToInt32(((HiddenField)item.FindControl("hdnID")).Value);
                else
                    drWIPDet["Id"] = 0;
                if (!string.IsNullOrEmpty(hdnWIPID.Value))
                    drWIPDet["WIPId"] = Convert.ToInt32(hdnWIPID.Value);
                drWIPDet["Line"] = Convert.ToInt32(((HiddenField)item.FindControl("hdnLine")).Value);
                drWIPDet["WIPDesc"] = Convert.ToString(((TextBox)item.FindControl("txtWIPDesc")).Text);
                drWIPDet["ContractAmount"] = Convert.ToDecimal(((TextBox)item.FindControl("txtContractAmount")).Text);
                drWIPDet["ChangeOrder"] = Convert.ToDecimal(((TextBox)item.FindControl("txtChangeOrder")).Text);
                drWIPDet["ScheduledValues"] = Convert.ToDecimal(((TextBox)item.FindControl("txtScheduledValues")).Text);
                drWIPDet["PreviousBilled"] = Convert.ToDecimal(((TextBox)item.FindControl("txtPreviousBilled")).Text);
                drWIPDet["CompletedThisPeriod"] = Convert.ToDecimal(((TextBox)item.FindControl("txtCompletedThisPeriod")).Text);
                drWIPDet["PresentlyStored"] = Convert.ToDecimal(((TextBox)item.FindControl("txtPresentlyStored")).Text);
                //if (Convert.ToDecimal(((TextBox)item.FindControl("txtCompletedThisPeriod")).Text) > 0)
                //    drWIPDet["TotalCompletedAndStored"] = Convert.ToDecimal(((TextBox)item.FindControl("txtTotalCompletedAndStored")).Text) - Convert.ToDecimal(((TextBox)item.FindControl("txtRetainageCumAmount")).Text.Replace("(", "").Replace(")", ""));
                //else
                drWIPDet["TotalCompletedAndStored"] = Convert.ToDecimal(((TextBox)item.FindControl("txtTotalCompletedAndStored")).Text);
                drWIPDet["PerComplete"] = Convert.ToDecimal(((TextBox)item.FindControl("txtPerComplete")).Text);
                if (Convert.ToDecimal(((TextBox)item.FindControl("txtCompletedThisPeriod")).Text) >= 0)
                    // drWIPDet["BalanceToFinsh"] = Convert.ToDecimal(((TextBox)item.FindControl("txtBalanceToFinsh")).Text) + Convert.ToDecimal(((TextBox)item.FindControl("txtRetainageAmount")).Text.Replace("(", "").Replace(")", ""));
                    drWIPDet["BalanceToFinsh"] = Convert.ToDecimal(((TextBox)item.FindControl("txtBalanceToFinsh")).Text);

                else
                    drWIPDet["BalanceToFinsh"] = Convert.ToDecimal(((TextBox)item.FindControl("txtBalanceToFinsh")).Text);
                drWIPDet["RetainagePer"] = Convert.ToDecimal(((TextBox)item.FindControl("txtRetainagePer")).Text);
                drWIPDet["RetainageAmount"] = Convert.ToDecimal(((TextBox)item.FindControl("txtRetainageAmount")).Text.Replace("(", "").Replace(")", ""));
                drWIPDet["TotalBilled"] = Convert.ToDecimal(((TextBox)item.FindControl("txtTotalBilled")).Text);
                if (Convert.ToDecimal(drWIPDet["TotalBilled"]) > 0 && ((DropDownList)item.FindControl("ddlBillingCode")).SelectedValue == "0")
                {
                    ValidationMsg = "This project is missing codes under the Billing tab in the project screen";
                    ((DropDownList)item.FindControl("ddlBillingCode")).Focus();
                    break;
                }
                drWIPDet["BillingCode"] = Convert.ToInt32(((DropDownList)item.FindControl("ddlBillingCode")).SelectedValue);
                drWIPDet["Taxable"] = Convert.ToBoolean(((CheckBox)item.FindControl("chkTaxable")).Checked);
                //todo
                drWIPDet["GSTable"] = false;
                if (isCanadaCompany())
                {
                    drWIPDet["GSTable"] = Convert.ToBoolean(((CheckBox)item.FindControl("chkEnableGSTTax")).Checked);
                }

                tblWIPDetails.Rows.Add(drWIPDet);
            }
            tblWIPr.Tables.Add(tblWIPDetails);

            if (ValidationMsg == "")
            {
                objProp_Customer.WIP = tblWIPr;
                objProp_Customer.ConnConfig = Session["config"].ToString();
                //todo
                int WIPId = objBL_Customer.SaveWIPNew(objProp_Customer);
                if (btnSave.Text == "Edit WIP")
                {
                    //GridEditableItem edit = gvProgressBilling.Items[gvProgressBilling.Items.Count - 1];
                    //DropDownList ddlApplicationStatus = (DropDownList)edit.FindControl("ddlApplicationStatus");                  
                    //UpdateStatusWIP(Convert.ToInt32((ddlApplicationStatus.SelectedValue)));
                    UpdateAmountWIPInvoice(WIPId);
                }
                else
                {
                    if (WIPId > 0)
                    {
                        WIPSaveSuccess = true;
                        hdnWIPID.Value = "";// WIPId.ToString();
                        BindSummary();
                        GetWIP();
                        btnGenerateInvoice.Visible = true;

                        objProp_Customer.ConnConfig = Session["config"].ToString();
                        objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                        objProp_Customer.Type = string.Empty;
                        DataSet MilestonesDS = objBL_Customer.getJobProject_Milestone(objProp_Customer);
                        if (MilestonesDS.Tables[0].Rows.Count > 0)
                        {
                            BindgvMilestones(MilestonesDS.Tables[0]);
                        }


                        ScriptManager.RegisterStartupScript(this, this.GetType(), "HiddenDetailTab", "HiddenDetailTab();", true);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'WIP details are saved Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});MoveToThirdTab();", true);
                    }
                }


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyValidation", "noty({text: '" + ValidationMsg + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});MoveTab();", true);
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelete", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnGenerateInvoice_Click(object sender, EventArgs e)
    {
        SaveWIP_Click(sender, e);
        if (WIPSaveSuccess)
        {
            // Approve Status
            GridEditableItem edit = gvProgressBilling.Items[gvProgressBilling.Items.Count - 1];
            DropDownList ddlApplicationStatus = (DropDownList)edit.FindControl("ddlApplicationStatus");
            ddlApplicationStatus.SelectedValue = "3"; //##HardCode
                                                      // ddlApplicationStatus_SelectedIndexChanged(ddlApplicationStatus, EventArgs.Empty);

            processUpdateWipStatus();

            // Make last row selected
            edit = gvProgressBilling.Items[gvProgressBilling.Items.Count - 1];
            (edit.Cells[0].FindControl("chkSelect") as CheckBox).Checked = true;

            // Generate Report

            ModalPopupGenerateReport.Show();
            pnlGenerateReport.Visible = true;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveToThirdTab7", "MoveToThirdTab();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowHideGenerateInvoice", "ShowHideGenerateInvoice();", true);
        }
    }

    protected void lnkProgressBillingNo_Click(object sender, EventArgs e)
    {
        LinkButton lnkProgressBillingNo = (LinkButton)sender;
        hdnWIPID.Value = lnkProgressBillingNo.Attributes["data-id"].ToString();
        btnGenerateInvoice.Visible = false;
        GetWIP();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveTab", "MoveTab();", true);
    }

    protected void ibDelete_Click(object sender, EventArgs e)
    {
        ImageButton ibDelete = (ImageButton)sender;
        hdnWIPID.Value = ibDelete.Attributes["data-id"].ToString();
        DeleteWIP();
    }

    protected void lnkDeleteWIP_Click(object sender, EventArgs e)
    {
        GridEditableItem edit = gvProgressBilling.Items[gvProgressBilling.Items.Count - 1];
        ImageButton ibDelete = (ImageButton)edit.FindControl("ibDeleteWIP");

        hdnWIPID.Value = ibDelete.Attributes["data-id"].ToString();
        DeleteWIP();
    }

    protected void DeleteWIP()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
        if (!String.IsNullOrEmpty(hdnWIPID.Value))
            objProp_Customer.WIPID = Convert.ToInt32(hdnWIPID.Value);
        int count = objBL_Customer.DeleteWIP(objProp_Customer);
        if (count > 0)
        {
            hdnWIPID.Value = "";
            GetWIP();
            BindSummary();
            BindBudget(1);
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objProp_Customer.Type = string.Empty;
            DataSet MilestonesDS = objBL_Customer.getJobProject_Milestone(objProp_Customer);
            if (MilestonesDS.Tables[0].Rows.Count > 0)
            {
                BindgvMilestones(MilestonesDS.Tables[0]);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HiddenDetailTab", "HiddenDetailTab();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoadGvBudget", "LoadGvBudget();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'WIP record deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});MoveToThirdTab();", true);
        }
    }

    protected void ddlApplicationStatus_SelectedIndexChanged(object sender, EventArgs e)
    {


    }

    protected void UpdateStatusWIP(int WIPStatus)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
        if (!String.IsNullOrEmpty(hdnWIPID.Value))
            objProp_Customer.WIPID = Convert.ToInt32(hdnWIPID.Value);
        objProp_Customer.WIPStatus = WIPStatus;
        objProp_Customer.Username = Session["username"].ToString();
        //todo
        int count = objBL_Customer.EditWIPStatus(objProp_Customer);
        if (count > 0)
        {
            objProp_Customer.WIPStatus = null;
            hdnWIPID.Value = "";
            GetWIP();
            BindSummary();
            BindBudget(1);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Status updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});MoveToThirdTab();", true);
        }
    }
    protected void UpdateAmountWIPInvoice(int WIPStatus)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        if (!String.IsNullOrEmpty(hdnWIPID.Value))
            objProp_Customer.WIPID = Convert.ToInt32(hdnWIPID.Value);
        objProp_Customer.WIPStatus = WIPStatus;
        objProp_Customer.Username = Session["username"].ToString();
        //todo
        int count = objBL_Customer.EditAmountWIPInvoice(objProp_Customer);
        if (count > 0)
        {
            btnSave.Text = "Save WIP";
            btnGenerateInvoice.Visible = true;
            objProp_Customer.WIPStatus = null;
            hdnWIPID.Value = "";
            BindSummary();
            GetWIP();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "HiddenDetailTab", "HiddenDetailTab();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowProgressBilling", "ShowProgressBilling();", true);
            //BindSummary();
            //BindBudget(1);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});MoveToThirdTab();", true);
        }
    }
    private void FillApplicationStatus()
    {
        try
        {
            DataSet _dsAS = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            _dsAS = objBL_Job.GetApplicationStatus(objJob);
            if (_dsAS.Tables[0].Rows.Count > 0)
            {
                dtApplicationStatus = _dsAS.Tables[0];
            }
            else
            {
                DataRow toInsert = _dsAS.Tables[0].NewRow();
                toInsert[0] = "0";
                toInsert[1] = "No data found";
                _dsAS.Tables[0].Rows.Add(toInsert);
                dtApplicationStatus = _dsAS.Tables[0];
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillTerms()
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            ds = objBL_User.getTerms(objPropUser);

            ddlTerms.DataSource = ds.Tables[0];
            ddlTerms.DataTextField = "name";
            ddlTerms.DataValueField = "id";
            ddlTerms.DataBind();

            ddlTerms.Items.Insert(0, new ListItem(":: Select ::", "-1"));
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void gvProgressBilling_PreRender(object sender, EventArgs e)
    {
        int count = gvProgressBilling.Items.Count;
        for (int i = 0; i < count; i++)
        {
            if (i != (count - 1))
            {
                LinkButton lnkProgressBillingNo = (LinkButton)gvProgressBilling.Items[i].FindControl("lnkProgressBillingNo");
                lnkProgressBillingNo.Enabled = false;
                lnkProgressBillingNo.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
                lnkProgressBillingNo.OnClientClick = null;

                ImageButton ibDeleteWIP = (ImageButton)gvProgressBilling.Items[i].FindControl("ibDeleteWIP");
                ibDeleteWIP.Visible = false;

                DropDownList ddlApplicationStatus = (DropDownList)gvProgressBilling.Items[i].FindControl("ddlApplicationStatus");
                ddlApplicationStatus.Enabled = false;
            }
        }
    }

    protected void btnWIPCancel_Click(object sender, EventArgs e)
    {
        btnGenerateInvoice.Visible = true;
        hdnWIPID.Value = "";
        GetWIP();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveToThirdTab5", "MoveToThirdTab();", true);
    }

    protected void gvBudget_ItemDataBound(object sender, GridItemEventArgs e)
    {

        double Cost_Actual = 0;
        double Cost_Comm = 0;
        double Cost_ReceivePO = 0;
        double Cost_Total = 0;
        double Cost_Budget = 0;
        double Cost_Variance = 0;
        double Cost_Hours = 0;

        double Revenues_Actual = 0;
        double Revenues_Comm = 0;
        double Revenues_ReceivePO = 0;
        double Revenues_Total = 0;
        double Revenues_Budget = 0;
        double Revenues_Variance = 0;
        double Revenues_Hours = 0;
        //if (intExpExcelFlag == 0)
        //{
        foreach (GridGroupFooterItem groupFooter in gvBudget.MasterTableView.GetItems(GridItemType.GroupFooter))
        {
            DataRowView groupDataRow = (DataRowView)groupFooter.DataItem;

            GridGroupFooterItem item = (GridGroupFooterItem)groupFooter;
            //item["Ratio"].Text = Math.Round(Double.Parse(item["Ratio"].Text.Replace('%', ' ')), 2) + "%";

            if (groupDataRow != null)
            {
                if (groupDataRow["JobType"].ToString().ToLower() == "cost")
                {
                    string value = item["Actual"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Cost_Actual = Double.Parse(value);

                    value = item["Comm"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Cost_Comm = Double.Parse(value);

                    value = item["ReceivePO"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Cost_ReceivePO = Double.Parse(value);

                    value = item["Total"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Cost_Total = Double.Parse(value);

                    value = item["Budget"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Cost_Budget = Double.Parse(value);

                    value = item["Variance"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Cost_Variance = Double.Parse(value);

                    value = item["BHours"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Cost_Hours = Double.Parse(value);


                }
                if (groupDataRow["JobType"].ToString().ToLower() == "revenue")
                {
                    string value = item["Actual"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Revenues_Actual = Double.Parse(value);

                    value = item["Comm"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Revenues_Comm = Double.Parse(value);

                    value = item["ReceivePO"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Revenues_ReceivePO = Double.Parse(value);

                    value = item["Total"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Revenues_Total = Double.Parse(value);

                    value = item["Budget"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Revenues_Budget = Double.Parse(value);

                    value = item["Variance"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Revenues_Variance = Double.Parse(value);

                    value = item["BHours"].Text.Replace("$", "").Replace("(", "-").Replace(")", "");
                    Revenues_Hours = Double.Parse(value);


                }
            }
        }
        foreach (GridFooterItem footerItem in gvBudget.MasterTableView.GetItems(GridItemType.Footer))
        {
            if (Convert.ToDouble(Revenues_Actual - Cost_Actual) < 0)
                footerItem["Actual"].Controls.Add(new LiteralControl(string.Format("<span style='color:red;'>{0:c}</span>", Convert.ToDouble(Revenues_Actual - Cost_Actual))));
            else
                footerItem["Actual"].Controls.Add(new LiteralControl(string.Format("{0:c}", Convert.ToDouble(Revenues_Actual - Cost_Actual))));

            //if (Convert.ToDouble(Revenues_Comm - Cost_Comm) < 0)
            //    footerItem["Comm"].Controls.Add(new LiteralControl(string.Format("<span style='color:red;'>{0:c}</span>", Convert.ToDouble(Revenues_Comm - Cost_Comm))));
            //else
            footerItem["Comm"].Controls.Add(new LiteralControl(string.Format("{0:c}", Convert.ToDouble(Cost_Comm))));

            if (Convert.ToDouble(Revenues_ReceivePO - Cost_ReceivePO) < 0)
                footerItem["ReceivePO"].Controls.Add(new LiteralControl(string.Format("<span style='color:red;'>{0:c}</span>", Convert.ToDouble(Revenues_ReceivePO - Cost_ReceivePO))));
            else
                footerItem["ReceivePO"].Controls.Add(new LiteralControl(string.Format("{0:c}", Convert.ToDouble(Revenues_ReceivePO - Cost_ReceivePO))));

            if (Convert.ToDouble(Revenues_Total - Cost_Total) < 0)
                footerItem["Total"].Controls.Add(new LiteralControl(string.Format("<span style='color:red;'>{0:c}</span>", Convert.ToDouble(Revenues_Total - Cost_Total))));
            else
                footerItem["Total"].Controls.Add(new LiteralControl(string.Format("{0:c}", Convert.ToDouble(Revenues_Total - Cost_Total))));

            if (Convert.ToDouble(Revenues_Budget - Cost_Budget) < 0)
                footerItem["Budget"].Controls.Add(new LiteralControl(string.Format("<span style='color:red;'>{0:c}</span>", Convert.ToDouble(Revenues_Budget - Cost_Budget))));
            else
                footerItem["Budget"].Controls.Add(new LiteralControl(string.Format("{0:c}", Convert.ToDouble(Revenues_Budget - Cost_Budget))));

            if (Convert.ToDouble(Revenues_Variance - Cost_Variance) < 0)
            {
                footerItem["Variance"].Controls.Add(new LiteralControl(string.Format("<span style='color:red;'>{0:c}</span>", Convert.ToDouble(Revenues_Variance - Cost_Variance))));

            }
            else
            {
                footerItem["Variance"].Controls.Add(new LiteralControl(string.Format("{0:c}", Convert.ToDouble(Revenues_Variance - Cost_Variance))));

            }

            double fRatio = 0;
            double fVariance = Revenues_Variance - Cost_Variance;
            double fbudget = 1;
            if (Revenues_Budget - Cost_Budget != 0) { fbudget = Revenues_Budget - Cost_Budget; }



            fRatio = ((fVariance) / ((fbudget)) * 100);

            if (fRatio < 0)
                footerItem["Rationew"].Controls.Add(new LiteralControl(string.Format("<span style='color:red;'>{0:n4}</span>", Convert.ToDouble(fRatio))));
            else
                footerItem["RatioNew"].Controls.Add(new LiteralControl(string.Format("{0:n4}", Convert.ToDouble(fRatio))));

            if (Convert.ToDouble(Revenues_Hours - Cost_Hours) < 0)
                footerItem["BHours"].Controls.Add(new LiteralControl(string.Format("<span style='color:red;'>{0:n}</span>", Convert.ToDouble(Revenues_Hours - Cost_Hours))));
            else
                footerItem["BHours"].Controls.Add(new LiteralControl(string.Format("{0:n}", Convert.ToDouble(Revenues_Hours - Cost_Hours))));
        }
        // }
    }



    protected void gvBudget_CustomAggregate(object sender, GridCustomAggregateEventArgs e)
    {



    }

    private int Calculate(GridGroupHeaderItem group, int count)
    {
        foreach (GridItem item in group.GetChildItems())
        {
            if (item is GridGroupHeaderItem)
            {
                count = Calculate(item as GridGroupHeaderItem, count);
            }
            if (item is GridDataItem)
            {
                count++;
            }
        }
        return count;
    }


    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string MIME = string.Empty;
            if (fuAttachPO.HasFile)
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";
                filename = fuAttachPO.FileName;
                filename = filename.Replace(",", "");
                fullpath = savepath + filename;
                MIME = System.IO.Path.GetExtension(fuAttachPO.PostedFile.FileName).Substring(1);

                if (File.Exists(fullpath))
                {
                    for (int i = 1; i < 100; i++)
                    {
                        string tmpFileName = string.Empty;
                        if (MIME != null)
                        {
                            tmpFileName = filename.Replace("." + MIME, "(" + i + ")." + MIME);
                        }
                        else
                        {
                            tmpFileName = filename + "(" + i + ")";
                        }
                        fullpath = savepath + tmpFileName;
                        if (!File.Exists(fullpath))
                        {
                            filename = tmpFileName;
                            fullpath = savepath + filename;
                            break;
                        }
                    }
                }

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                fuAttachPO.SaveAs(fullpath);
            }

            objMapData.Screen = "Project";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objMapData.TempId = "0";
            objMapData.FileName = filename;
            objMapData.DocTypeMIME = MIME;
            objMapData.FilePath = fullpath;
            objMapData.DocID = 0;
            objMapData.Mode = 0;
            objMapData.ConnConfig = Session["config"].ToString();
            objBL_MapData.AddFile(objMapData);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadSuccess1", "noty({text: 'File uploaded successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

        }
    }

    protected void DoPostBack()
    {

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "$('#tbpnlAttri').hide(); showDocument();", true);
    }

    protected void lnkProjectVarianceReport_Click(object sender, EventArgs e)
    {
        byte[] buffer = null;
        DataTable dt = new DataTable();

        if (Request.QueryString["uid"] != null)
            objProp_Customer.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objProp_Customer.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_Customer.GetProjectVarianceReport(objProp_Customer);
        StiReport report = new StiReport();
        reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Project Variance Report.mrt";
        report.Load(reportPath);
        report.Compile();

        DataSet AIAHeader = new DataSet();
        DataTable hTable = ds.Tables[0].Copy();
        hTable.TableName = "Job";
        AIAHeader.Tables.Add(hTable);
        AIAHeader.DataSetName = "Job";

        DataSet AIADetails = new DataSet();
        DataTable dTable = ds.Tables[1].Copy();
        dTable.TableName = "BOMItem";
        AIADetails.Tables.Add(dTable);
        AIADetails.DataSetName = "BOMItem";

        report.RegData("Job", AIAHeader);
        report.RegData("BOMItem", AIADetails);
        report.Dictionary.Synchronize();
        report.Render();

        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
        var service = new Stimulsoft.Report.Export.StiPdfExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(report, stream, settings);
        buffer = stream.ToArray();

        string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF/", "ProjectVariance_" + objProp_Customer.job.ToString() + ".pdf");

        if (buffer != null)
        {
            if (File.Exists(filename))
                File.Delete(filename);
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
        }
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Buffer = true;
        Response.AddHeader("Content-Disposition", "attachment;filename=" + "ProjectVariance_" + objProp_Customer.job.ToString() + ".pdf");
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Length", (buffer.Length).ToString());
        Response.BinaryWrite(buffer);
    }

    protected void lnkProjectGLCrossReferenceReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            string urlString = "ProjectGLCrossReferenceReport.aspx?id=" + Request.QueryString["uid"] + "&df=" + txtfromDate.Text + "&dt=" + txtToDate.Text + "&type=" + ddlDateRange.SelectedItem.Value;

            Response.Redirect(urlString, true);
        }
    }

    protected void lnkProjectVendorByProject_Click(object sender, EventArgs e)
    {
        string url = "ProjectVendorDemandByProject.aspx?sb=j.ID&sv=" + Request.QueryString["uid"] + "&page=addProject&show=true";
        Response.Redirect(url);
    }

    protected void lnkProjectVendorByVendor_Click(object sender, EventArgs e)
    {
        string url = "ProjectVendorDemandByVendor.aspx?sb=j.ID&sv=" + Request.QueryString["uid"] + "&page=addProject&show=true";
        Response.Redirect(url);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dtCustom = (DataTable)Session["dtCustom"];
        DataTable dtCustomExport = new DataTable();

        dtCustomExport = dtCustom.Copy();

        foreach (DataColumn dc in dtCustom.Columns)
        {
            var columnName = dc.ColumnName;
            if (dc.ColumnName != "Label" && dc.ColumnName != "UpdatedDate" && dc.ColumnName != "Username")
            {
                dtCustomExport.Columns.Remove(columnName);
            }
        }

        var fileName = Session["projectname"] + ".xlsx";
        CreateExcelDocument(dtCustomExport, fileName, Response);
    }

    #region Export to Excel function
    public static bool CreateExcelDocument(DataTable dt, string filename, System.Web.HttpResponse Response)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            CreateExcelDocumentAsStream(ds, filename, Response);
            ds.Tables.Remove(dt);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /// <summary>
    /// Create an Excel file, and write it out to a MemoryStream (rather than directly to a file)
    /// </summary>
    /// <param name="ds">DataSet containing the data to be written to the Excel.</param>
    /// <param name="filename">The filename (without a path) to call the new Excel file.</param>
    /// <param name="Response">HttpResponse of the current page.</param>
    /// <returns>Either a MemoryStream, or NULL if something goes wrong.</returns>
    public static bool CreateExcelDocumentAsStream(DataSet ds, string filename, System.Web.HttpResponse Response)
    {
        try
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
            {
                WriteExcelFile(ds, document);
            }
            stream.Flush();
            stream.Position = 0;

            Response.ClearContent();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";

            //  NOTE: If you get an "HttpCacheability does not exist" error on the following line, make sure you have
            //  manually added System.Web to this project's References.

            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.AddHeader("content-disposition", "attachment; filename=" + filename);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] data1 = new byte[stream.Length];
            stream.Read(data1, 0, data1.Length);
            stream.Close();
            Response.BinaryWrite(data1);
            Response.Flush();
            Response.End();

            return true;
        }
        catch (Exception ex)
        {
            //Trace.WriteLine("Failed, exception thrown: " + ex.Message);
            return false;
        }


    }
    private static void WriteExcelFile(DataSet ds, SpreadsheetDocument spreadsheet)
    {
        //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
        //  to a file, or writing to a MemoryStream.
        spreadsheet.AddWorkbookPart();
        spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

        //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
        spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

        //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
        WorkbookStylesPart workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
        Stylesheet stylesheet = new Stylesheet();
        workbookStylesPart.Stylesheet = stylesheet;

        //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
        uint worksheetNumber = 1;
        foreach (DataTable dt in ds.Tables)
        {
            //  For each worksheet you want to create
            string workSheetID = "rId" + worksheetNumber.ToString();
            string worksheetName = dt.TableName;

            WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet();
            // Set custom width for excel columns
            Columns columns = new Columns();

            columns.Append(new Column() { Min = 1, Max = 3, Width = 20, CustomWidth = true });
            columns.Append(new Column() { Min = 4, Max = 4, Width = 30, CustomWidth = true });
            newWorksheetPart.Worksheet.Append(columns);
            // create sheet data
            newWorksheetPart.Worksheet.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.SheetData());

            // save worksheet
            WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
            newWorksheetPart.Worksheet.Save();

            // create the worksheet to workbook relation
            if (worksheetNumber == 1)
                spreadsheet.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

            spreadsheet.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>().AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheet()
            {
                Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                SheetId = (uint)worksheetNumber,
                Name = dt.TableName
            });

            worksheetNumber++;
        }


        spreadsheet.WorkbookPart.Workbook.Save();
    }

    private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart)
    {
        var worksheet = worksheetPart.Worksheet;
        var sheetData = worksheet.GetFirstChild<SheetData>();

        string cellValue = "";

        //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
        //
        //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
        //  cells of data, we'll know if to write Text values or Numeric cell values.
        int numberOfColumns = dt.Columns.Count;
        bool[] IsNumericColumn = new bool[numberOfColumns];

        string[] excelColumnNames = new string[numberOfColumns];
        for (int n = 0; n < numberOfColumns; n++)
            excelColumnNames[n] = GetExcelColumnName(n);

        //
        //  Create the Header row in our Excel Worksheet
        //
        uint rowIndex = 1;

        var headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
        sheetData.Append(headerRow);

        for (int colInx = 0; colInx < numberOfColumns; colInx++)
        {
            DataColumn col = dt.Columns[colInx];
            AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName, headerRow);
            IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32");
        }

        //
        //  Now, step through each row of data in our DataTable...
        //
        double cellNumericValue = 0;
        foreach (DataRow dr in dt.Rows)
        {
            // ...create a new row, and append a set of this row's data to it.
            ++rowIndex;
            var newExcelRow = new DocumentFormat.OpenXml.Spreadsheet.Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(newExcelRow);

            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                cellValue = dr.ItemArray[colInx].ToString();

                // Create cell with data
                if (IsNumericColumn[colInx])
                {
                    //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                    //  If this numeric value is NULL, then don't write anything to the Excel file.
                    cellNumericValue = 0;
                    if (double.TryParse(cellValue, out cellNumericValue))
                    {
                        cellValue = cellNumericValue.ToString();
                        AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                    }
                }
                else
                {
                    //  For text cells, just write the input data straight out to the Excel file.
                    AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                }
            }
        }
    }

    private static void AppendTextCell(string cellReference, string cellStringValue, DocumentFormat.OpenXml.Spreadsheet.Row excelRow)
    {
        //  Add a new Excel Cell to our Row 
        Cell cell = new Cell() { CellReference = cellReference, DataType = CellValues.String };
        CellValue cellValue = new CellValue();
        cellValue.Text = cellStringValue;
        cell.Append(cellValue);
        excelRow.Append(cell);
    }

    private static void AppendNumericCell(string cellReference, string cellStringValue, DocumentFormat.OpenXml.Spreadsheet.Row excelRow)
    {
        //  Add a new Excel Cell to our Row 
        Cell cell = new Cell() { CellReference = cellReference };
        CellValue cellValue = new CellValue();
        cellValue.Text = cellStringValue;
        cell.Append(cellValue);
        excelRow.Append(cell);
    }

    private static string GetExcelColumnName(int columnIndex)
    {
        //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
        //
        //  eg  GetExcelColumnName(0) should return "A"
        //      GetExcelColumnName(1) should return "B"
        //      GetExcelColumnName(25) should return "Z"
        //      GetExcelColumnName(26) should return "AA"
        //      GetExcelColumnName(27) should return "AB"
        //      ..etc..
        //
        if (columnIndex < 26)
            return ((char)('A' + columnIndex)).ToString();

        char firstChar = (char)('A' + (columnIndex / 26) - 1);
        char secondChar = (char)('A' + (columnIndex % 26));

        return string.Format("{0}{1}", firstChar, secondChar);
    }

    protected void lnkExcelBud_Click(object sender, EventArgs e)
    {
        //intExpExcelFlag = 1;
        gvBudget.ExportSettings.FileName = "Budget";
        gvBudget.ExportSettings.IgnorePaging = true;
        gvBudget.ExportSettings.OpenInNewWindow = true;
        gvBudget.MasterTableView.UseAllDataFields = true;
        gvBudget.ExportSettings.ExportOnlyData = true;
        gvBudget.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        gvBudget.ExportSettings.HideStructureColumns = false;
        gvBudget.MasterTableView.HierarchyDefaultExpanded = true;
        gvBudget.ExportSettings.SuppressColumnDataFormatStrings = false;
        gvBudget.MasterTableView.GroupsDefaultExpanded = true;
        gvBudget.MasterTableView.ExportToExcel();
    }
    protected void lnkExcelTicket_Click(object sender, EventArgs e)
    {
        gvTickets.MasterTableView.GetColumn("chkSelect").Visible = false;
        gvTickets.MasterTableView.GetColumn("imgMobileService").Visible = false;
        gvTickets.MasterTableView.GetColumn("chkHold").Visible = false;
        gvTickets.ExportSettings.FileName = "Ticket";
        gvTickets.ExportSettings.IgnorePaging = true;
        gvTickets.ExportSettings.OpenInNewWindow = true;
        gvTickets.MasterTableView.UseAllDataFields = true;
        gvTickets.ExportSettings.ExportOnlyData = true;
        gvTickets.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        gvTickets.ExportSettings.HideStructureColumns = false;
        gvTickets.MasterTableView.HierarchyDefaultExpanded = true;
        gvTickets.ExportSettings.SuppressColumnDataFormatStrings = false;
        gvTickets.MasterTableView.GroupsDefaultExpanded = true;
        gvTickets.MasterTableView.ExportToExcel();
    }
    #endregion

    #region NeedDataSource

    public void RadGrid_gvContacts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int JobID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
        ViewState["mode"] = "0";
        if (JobID > 0)
        {
            ViewState["mode"] = "1";
            objJob.ConnConfig = Session["config"].ToString();
            objJob.Job = JobID;
            DataSet ds = objBL_Job.GetContactForJob(objJob, new GeneralFunctions().GetSalesAsigned());
            gvContacts.VirtualItemCount = ds.Tables[0].Rows.Count;
            gvContacts.DataSource = ds;
        }
    }

    public void RadGrid_gvBudget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        if (Request.QueryString["uid"] != null && hdnLoadgvBudget.Value == "1")
        {
            objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objJob.PageSize = Convert.ToInt32(PageSize);
            objJob.PageIndex = 1;

            if (Request.QueryString["uid"] != null)
            {
                if (Request.QueryString["s"] != null && Request.QueryString["e"] != null)
                {
                    objJob.StartDate = Convert.ToDateTime(System.Web.HttpUtility.UrlDecode(Request.QueryString["s"].ToString()));
                    objJob.EndDate = Convert.ToDateTime(System.Web.HttpUtility.UrlDecode(Request.QueryString["e"].ToString()));
                }
            }
            ds = objBL_Job.GetJobCostByJob(objJob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvBudget.VirtualItemCount = ds.Tables[0].Rows.Count;
                gvBudget.DataSource = ds.Tables[0];

            }
        }
    }





    public void gvExpenses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet dsExp = new DataSet();
        if (Request.QueryString["uid"] != null)
        {
            objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objJob.PageSize = Convert.ToInt32(PageSize);
            objJob.PageIndex = 1;

            if (Request.QueryString["s"] != null && Request.QueryString["e"] != null)
                dsExp = objBL_Job.GetExpenseJobCostByDate(objJob);
            else
                dsExp = objBL_Job.GetExpenseJobCost(objJob);
            if (dsExp.Tables[0].Rows.Count > 0)
            {
                gvExpenses.VirtualItemCount = dsExp.Tables[0].Rows.Count;
                gvExpenses.DataSource = dsExp.Tables[0];
            }
        }
    }

    public void gvInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["uid"] != null && hdnLoadAP_ARInvoices.Value == "1")
        {
            DataSet ds = new DataSet();

            objProp_Contracts.ConnConfig = Session["config"].ToString();

            #region Company Check
            objProp_Contracts.UserID = Convert.ToString(Session["UserID"]);
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objProp_Contracts.EN = 1;
            }
            else
            {
                objProp_Contracts.EN = 0;
            }
            #endregion

            objProp_Contracts.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());

            if (ddlInvoiceStatus.SelectedValue != "-1")
            {
                objProp_Contracts.SearchBy = "i.Status";
                objProp_Contracts.SearchValue = ddlInvoiceStatus.SelectedValue;
            }

            if (txtfromDate.Text != "" && txtToDate.Text != "")
            {
                objProp_Contracts.StartDate = Convert.ToDateTime(txtfromDate.Text);
                objProp_Contracts.EndDate = Convert.ToDateTime(txtToDate.Text);
            }

            //ds = objBL_Contracts.GetInvoices(objProp_Contracts, null, null, null);
            ds = objBL_Contracts.GetProjectARInvoices(objProp_Contracts);

            gvInvoice.VirtualItemCount = ds.Tables[0].Rows.Count;
            gvInvoice.DataSource = ds.Tables[0];
            ViewState["ARInvoices"] = ds.Tables[0];
        }
    }

    public void gvAPInvoices_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["uid"] != null && hdnLoadAP_ARInvoices.Value == "1")
        {
            DataSet ds = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());

            if (txtfromDate.Text != "" && txtToDate.Text != "")
            {
                objProp_Contracts.StartDate = Convert.ToDateTime(txtfromDate.Text);
                objProp_Contracts.EndDate = Convert.ToDateTime(txtToDate.Text);
            }

            ds = objBL_Contracts.GetAPInvoices(objProp_Contracts);
            gvAPInvoices.VirtualItemCount = ds.Tables[0].Rows.Count;
            gvAPInvoices.DataSource = ds.Tables[0];
        }
    }

    public void gvTickets_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["uid"] != null && hdnloadticket.Value == "1")
        {
            BindTicketList("", 0, false);
        }
    }

    #endregion




    protected void lnkDeleteTicket_Click(object sender, EventArgs e)
    {
        var successMess = new StringBuilder();
        var errorMess = new StringBuilder();
        var permissionErrorMess = new StringBuilder();
        var deletedCount = 0;
        var delFailedCount = 0;
        foreach (GridDataItem di in gvTickets.Items)
        {
            CheckBox chkSelect = (CheckBox)di.Cells[0].FindControl("chkSelect");
            Label lblTicketId = (Label)di.Cells[1].FindControl("lblTicketId");
            Label lblStatus = (Label)di.Cells[5].FindControl("lblStatus");

            if (chkSelect.Checked == true)
            {
                try
                {
                    if (lblStatus.Text.Equals("Assigned", StringComparison.InvariantCultureIgnoreCase)
                        || lblStatus.Text.Equals("Hold", StringComparison.InvariantCultureIgnoreCase)
                        || lblStatus.Text.Equals("Open", StringComparison.InvariantCultureIgnoreCase))
                    {

                        Int32 TicketId = 0;
                        if (Int32.TryParse(lblTicketId.Text, out TicketId))
                        {
                            DeleteTicket(TicketId);
                            successMess.AppendFormat("# {0}, ", lblTicketId.Text);
                            deletedCount++;

                        }
                    }
                    else
                    {
                        permissionErrorMess.AppendFormat("# {0}, ", lblTicketId.Text);
                    }
                }
                catch (Exception ex)
                {

                    errorMess.AppendFormat("# {0}, ", lblTicketId.Text);
                    delFailedCount++;

                }
            }
        }


        if (successMess.Length > 0)
        {
            successMess.Remove(successMess.Length - 2, 2);
            if (deletedCount > 1)
            {
                successMess.Insert(0, "Tickets ");
                successMess.Append(" were deleted successful!");
            }
            else
            {
                successMess.Insert(0, "Ticket ");
                successMess.Append(" was deleted successful!");
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyDelete", "noty({text: '" + successMess.ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); ", true);
        }

        if (errorMess.Length > 0)
        {
            errorMess.Remove(errorMess.Length - 2, 2);
            if (delFailedCount > 1)
            {
                errorMess.Insert(0, "Delete failed! Tickets ");
            }
            else
            {
                errorMess.Insert(0, "Delete failed! Ticket ");
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelete", "noty({text: '" + errorMess.ToString() + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }

        if (permissionErrorMess.Length > 0)
        {
            permissionErrorMess.Remove(permissionErrorMess.Length - 2, 2);
            permissionErrorMess.Insert(0, "Cannot delete tickets ");
            permissionErrorMess.Append(". Only tickets which status: \"Hold\", \"Assigned\", \"Open\" can be deleted!");
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDelete", "noty({text: '" + permissionErrorMess.ToString() + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }

        gvTickets.Rebind();
    }

    protected void RadGrid_CustomHeader_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }


    protected void RadGrid_Emails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        //RadGrid_Emails.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        if (!IsPostBack)
        {

            if (Session["Emails_FilterExpression"] != null && Convert.ToString(Session["Emails_FilterExpression"]) != "" && Session["Emails_Filters"] != null)
            {
                RadGrid_Emails.MasterTableView.FilterExpression = Convert.ToString(Session["Emails_FilterExpression"]);
                var filtersGet = Session["Emails_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Emails.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }

        }

        InitTeamMemberGridView();
    }

    protected void RadGrid_Emails_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Emails.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Emails_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Emails.MasterTableView.OwnerGrid.Columns)
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

            Session["Emails_Filters"] = filters;
        }
        else
        {
            Session["Emails_FilterExpression"] = null;
            Session["Emails_Filters"] = null;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "bindingClickCheckbox", "BindClickEventForGridCheckBox();", true);
    }



    protected void RadGrid_Custom_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            var gridId = ((RadGrid)sender).ID;
            var customViewStateName = gridId + "_dtValue";
            GridDataItem item = (GridDataItem)e.Item;
            CheckBox chkSelectAlert = (CheckBox)item.FindControl("chkSelectAlert");
            //CheckBox chkSelectTask = (CheckBox)item.FindControl("chkSelectTask");

            int format = Convert.ToInt32(DataBinder.Eval(item.DataItem, "Format"));
            int id = Convert.ToInt32(DataBinder.Eval(item.DataItem, "ID"));
            string textValue = DataBinder.Eval(item.DataItem, "Value").ToString();
            Label lblLine = (Label)item.FindControl("lblLine");

            Boolean isAlert = !string.IsNullOrEmpty(DataBinder.Eval(item.DataItem, "IsAlert").ToString()) ? Convert.ToBoolean(DataBinder.Eval(item.DataItem, "IsAlert")) : false;
            Boolean isTask = !string.IsNullOrEmpty(DataBinder.Eval(item.DataItem, "IsTask").ToString()) ? Convert.ToBoolean(DataBinder.Eval(item.DataItem, "IsTask")) : false;

            chkSelectAlert.Checked = isAlert;
            //chkSelectTask.Checked = isTask;

            switch (format)
            {
                case 1:
                    Panel divFormatCurrent = (Panel)item.FindControl("divFormatCurrent");
                    divFormatCurrent.Visible = true;
                    TextBox txtFormatCurrent = (TextBox)item.FindControl("txtFormatCurrent");
                    txtFormatCurrent.Text = textValue;
                    break;
                case 2:
                    Panel divFormatDate = (Panel)item.FindControl("divFormatDate");
                    divFormatDate.Visible = true;
                    TextBox txtFormatDate = (TextBox)item.FindControl("txtFormatDate");
                    txtFormatDate.Text = textValue;
                    break;
                case 3:
                    Panel divFormatText = (Panel)item.FindControl("divFormatText");
                    divFormatText.Visible = true;
                    TextBox txtFormatText = (TextBox)item.FindControl("txtFormatText");
                    txtFormatText.Text = textValue;
                    break;

                case 4:
                    Panel divFormatDrop = (Panel)item.FindControl("divFormatDrop");
                    divFormatDrop.Visible = true;
                    DropDownList drpdwnCustom = (DropDownList)item.FindControl("drpdwnCustom");
                    if (ViewState[customViewStateName] != null)
                    {
                        DataTable dtCustomval = (DataTable)ViewState[customViewStateName];
                        DataTable dataTemp = dtCustomval.Clone();

                        if (dtCustomval != null && dtCustomval.Rows.Count > 0 && drpdwnCustom != null && drpdwnCustom.Visible)
                        {
                            DataTable dt = dtCustomval.Clone();
                            DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                            foreach (DataRow row in result)
                            {
                                dt.ImportRow(row);
                            }

                            if (dt.Rows.Count > 0)
                            {
                                dt.DefaultView.Sort = "Value  ASC";
                                dt = dt.DefaultView.ToTable();
                            }

                            drpdwnCustom.DataSource = dt;
                            drpdwnCustom.DataTextField = "Value";
                            drpdwnCustom.DataValueField = "Value";
                            drpdwnCustom.DataBind();
                            drpdwnCustom.Items.Insert(0, (new ListItem("--Select item--", "")));
                            drpdwnCustom.SelectedValue = textValue;
                        }

                    }
                    break;
                case 5:
                    Panel divFormatCheckbox = (Panel)item.FindControl("divFormatCheckbox");
                    divFormatCheckbox.Visible = true;
                    CheckBox chkCustomFormat = (CheckBox)item.FindControl("chkCustomFormat");
                    try
                    {
                        chkCustomFormat.Checked = textValue == "" ? false : Convert.ToBoolean(textValue);
                    }
                    catch (Exception ex)
                    {
                        chkCustomFormat.Checked = false;
                    }

                    break;
                case 6:
                    Panel divFormatNotes = (Panel)item.FindControl("divFormatNotes");
                    divFormatNotes.Visible = true;
                    TextBox txtFormatNotes = (TextBox)item.FindControl("txtFormatNotes");
                    txtFormatNotes.Text = textValue;

                    break;
                case 7:
                    // TODO: checkboxWithComment
                    Panel divFormatChkWithComment = (Panel)item.FindControl("divFormatChkWithComment");
                    divFormatChkWithComment.Visible = true;
                    var chkVal = string.Empty;
                    var chkComment = string.Empty;
                    var arrVal = textValue.Split('|');
                    if (arrVal.Length > 1)
                    {
                        chkVal = arrVal[0];
                        chkComment = textValue.Substring(chkVal.Length + 1, textValue.Length - chkVal.Length - 1);
                    }
                    else
                    {
                        chkVal = "False";
                    }

                    CheckBox chkWithComment = (CheckBox)item.FindControl("chkWithComment");
                    TextBox txtChkComment = (TextBox)item.FindControl("txtChkComment");
                    txtChkComment.Text = chkComment;
                    try
                    {
                        chkWithComment.Checked = Convert.ToBoolean(chkVal);
                    }
                    catch (Exception ex)
                    {
                        chkWithComment.Checked = false;
                    }
                    break;
            }
        }
    }

    protected void RadGrid_CustomAttributesGeneral_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }


    private void UpdatingCustomDataTableValue(DataTable retDataTable)
    {
        Dictionary<Panel, RadGrid> panels = new Dictionary<Panel, RadGrid>();
        panels.Add(pnlCustomAttributesGC, RadGrid_CustomAttributesGC);
        panels.Add(pnlCustomAttributesGeneral, RadGrid_CustomAttributesGeneral);
        panels.Add(pnlCustomAttriEquip, RadGrid_CustomAttriEquip);
        panels.Add(pnlCustomAttrNotes, RadGrid_CustomAttrNotes);
        panels.Add(pnlCustomBOM, RadGrid_CustomBOM);
        panels.Add(pnlCustomFinanceBill, RadGrid_CustomFinanceBill);
        panels.Add(pnlCustomFinanceGeneral, RadGrid_CustomFinanceGeneral);
        panels.Add(pnlCustomFinceBudget, RadGrid_CustomFinceBudget);
        panels.Add(pnlCustomHeader, RadGrid_CustomHeader);
        panels.Add(pnlCustomHomeownerInfor, RadGrid_CustomHomeownerInfor);
        panels.Add(pnlCustomMilestone, RadGrid_CustomMilestone);
        panels.Add(pnlCustomTask, RadGrid_CustomTask);
        panels.Add(pnlCustomTicketList, RadGrid_CustomTicketList);

        foreach (var panel in panels)
        {
            if (panel.Key.Visible)
            {
                foreach (GridDataItem row in panel.Value.Items) // loops through each rows in RadGrid
                {
                    Label lblID = (Label)row.FindControl("lblID");

                    if (lblID != null && !string.IsNullOrEmpty(lblID.Text))
                    {
                        DataRow rowtb = retDataTable.Select("ID=" + lblID.Text).FirstOrDefault();
                        if (rowtb != null)
                        {
                            var formatType = rowtb["Format"].ToString();

                            switch (formatType)
                            {
                                case "1":
                                    TextBox txtFormatCurrent = (TextBox)row.FindControl("txtFormatCurrent");
                                    if (!rowtb["Value"].Equals(txtFormatCurrent.Text))
                                    {
                                        rowtb["OldValue"] = rowtb["Value"];
                                        rowtb["Value"] = txtFormatCurrent.Text;
                                        rowtb["IsValueChanged"] = true;
                                    }

                                    break;
                                case "2":
                                    TextBox txtFormatDate = (TextBox)row.FindControl("txtFormatDate");
                                    if (!rowtb["Value"].Equals(txtFormatDate.Text))
                                    {
                                        rowtb["OldValue"] = rowtb["Value"];
                                        rowtb["Value"] = txtFormatDate.Text;
                                        rowtb["IsValueChanged"] = true;
                                    }
                                    //rowtb["Value"] = txtFormatDate.Text;
                                    break;
                                case "3":
                                    TextBox txtbox = (TextBox)row.FindControl("txtFormatText");
                                    if (!rowtb["Value"].Equals(txtbox.Text))
                                    {
                                        rowtb["OldValue"] = rowtb["Value"];
                                        rowtb["Value"] = txtbox.Text;
                                        rowtb["IsValueChanged"] = true;
                                    }
                                    //rowtb["Value"] = txtbox.Text;
                                    break;
                                case "4":
                                    DropDownList ddlCustomValue = (DropDownList)row.FindControl("drpdwnCustom");
                                    if (!rowtb["Value"].Equals(ddlCustomValue.SelectedValue))
                                    {
                                        rowtb["OldValue"] = rowtb["Value"];
                                        rowtb["Value"] = ddlCustomValue.SelectedValue;
                                        rowtb["IsValueChanged"] = true;
                                    }
                                    //rowtb["Value"] = ddlCustomValue.SelectedValue;
                                    break;
                                case "5":
                                    CheckBox chkCustomFormat = (CheckBox)row.FindControl("chkCustomFormat");
                                    if (string.IsNullOrEmpty(rowtb["Value"].ToString())) rowtb["Value"] = "False";
                                    if (!rowtb["Value"].ToString().Equals(chkCustomFormat.Checked.ToString(), StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        rowtb["OldValue"] = rowtb["Value"];
                                        rowtb["Value"] = chkCustomFormat.Checked.ToString();
                                        rowtb["IsValueChanged"] = true;
                                    }
                                    //rowtb["Value"] = chkCustomFormat.Checked.ToString();
                                    break;
                                case "6":
                                    TextBox txtNotes = (TextBox)row.FindControl("txtFormatNotes");
                                    if (!rowtb["Value"].Equals(txtNotes.Text))
                                    {
                                        rowtb["OldValue"] = rowtb["Value"];
                                        rowtb["Value"] = txtNotes.Text;
                                        rowtb["IsValueChanged"] = true;
                                    }
                                    break;
                                case "7":

                                    var chkVal = string.Empty;
                                    var textValue = rowtb["Value"].ToString();
                                    var arrVal = textValue.Split('|');
                                    if (arrVal.Length > 1)
                                    {
                                        chkVal = arrVal[0];
                                    }
                                    else
                                    {
                                        chkVal = "False";
                                    }

                                    bool chkBoolVal = false;
                                    try
                                    {
                                        chkBoolVal = Convert.ToBoolean(chkVal);
                                    }
                                    catch (Exception)
                                    {
                                    }

                                    CheckBox chkWithComment = (CheckBox)row.FindControl("chkWithComment");
                                    TextBox txtChkComment = (TextBox)row.FindControl("txtChkComment");
                                    if (!chkBoolVal.Equals(chkWithComment.Checked))
                                    {
                                        rowtb["OldValue"] = rowtb["Value"];
                                        rowtb["Value"] = chkWithComment.Checked.ToString() + "|" + txtChkComment.Text;
                                        rowtb["IsValueChanged"] = true;
                                    }
                                    else
                                    {
                                        rowtb["OldValue"] = rowtb["Value"];
                                        rowtb["Value"] = chkWithComment.Checked.ToString() + "|" + txtChkComment.Text;
                                        rowtb["IsValueChanged"] = false;
                                    }
                                    break;
                            }

                            CheckBox chkSelectAlert = (CheckBox)row.FindControl("chkSelectAlert");
                            rowtb["IsAlert"] = chkSelectAlert.Checked;
                            //CheckBox chkSelectTask = (CheckBox)row.FindControl("chkSelectTask");
                            //rowtb["IsTask"] = chkSelectTask.Checked;
                            HiddenField hdnMembers = (HiddenField)row.FindControl("hdnMembers");
                            rowtb["TeamMember"] = hdnMembers.Value;
                            TextBox txtMembers = (TextBox)row.FindControl("txtMembers");
                            rowtb["TeamMemberDisplay"] = txtMembers.Text;
                            //HiddenField hdnUserRoles = (HiddenField)row.FindControl("hdnUserRoles");
                            //rowtb["UserRole"] = hdnUserRoles.Value;
                            //TextBox txtUserRoles = (TextBox)row.FindControl("txtUserRoles");
                            //rowtb["UserRoleDisplay"] = txtUserRoles.Text;
                            rowtb["UpdatedDate"] = DateTime.Now;
                            rowtb["Username"] = Session["username"].ToString();
                        }
                    }
                }
            }
        }
    }

    public void ResetFormControlValues(System.Web.UI.Control parent)
    {
        foreach (System.Web.UI.Control c in parent.Controls)
        {
            if (c.Controls.Count > 0 && c.ID != "pnlCustomAttributesGC" && c.ID != "pnlCustomHomeownerInfor")
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

    protected void lnkBomExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            LoadDataExportBOM();
            gvBOM.ExportSettings.FileName = "BOM";
            gvBOM.ExportSettings.IgnorePaging = true;
            gvBOM.ExportSettings.OpenInNewWindow = true;
            gvBOM.MasterTableView.UseAllDataFields = true;
            gvBOM.ExportSettings.ExportOnlyData = true;
            gvBOM.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
            gvBOM.ExportSettings.HideStructureColumns = false;
            gvBOM.MasterTableView.HierarchyDefaultExpanded = true;
            gvBOM.ExportSettings.SuppressColumnDataFormatStrings = false;
            gvBOM.MasterTableView.GroupsDefaultExpanded = true;
            gvBOM.MasterTableView.ExportToExcel();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }


    }

    private void LoadDataExportBOM()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        DataSet BomDS = objBL_Customer.getJobProject_BOM(objProp_Customer);
        if (BomDS.Tables[0].Rows.Count > 0)
        {
            gvBOM.DataSource = BomDS.Tables[0];
            gvBOM.DataBind();
        }
    }
    protected void lnkFileUploaded_Click(object sender, EventArgs e)
    {
        try
        {
            string[] validFileTypes = { ".csv", ".xls", ".xlsx" };
            string ext = System.IO.Path.GetExtension(FileUploadControl.PostedFile.FileName).ToLower();
            var results = Array.FindAll(validFileTypes, s => s.Equals(ext));
            if (results.Length == 0)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Please upload a csv or excel file.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                DataTable dtCurrent = GetMilestoneItems();
                DataTable dt = new DataTable();
                if (FileUploadControl.HasFile)
                {
                    if (ext == ".csv")
                        dt = ReadCsvFile();
                    if (ext == ".xls" || ext == ".xlsx")
                        dt = ReadExcelFile();

                    int line = dtCurrent.Rows.Count;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // search whole table
                        {
                            line = line + 1;
                            dr["Line"] = line;
                            dr["OrderNo"] = line;
                        }
                        dtCurrent.Merge(dt);
                    }

                    BindgvMilestones(dtCurrent);

                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    protected void btnDownloadCSV_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TempPDF/ImportBills/DetailSample.csv");
    }
    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TempPDF/ImportBills/DetailSample.xls");
    }

    public DataTable ReadCsvFile()
    {
        DataTable dtCsv = new DataTable();
        string FileSaveWithPath = Server.MapPath("~\\TempPDF\\ImportBills\\Import" + System.DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".csv");
        try
        {

            dtCsv.Columns.Add("JobT", typeof(int));
            dtCsv.Columns.Add("Job", typeof(int));
            dtCsv.Columns.Add("ID", typeof(int));
            dtCsv.Columns.Add("jType", typeof(int));//1
            dtCsv.Columns.Add("fDesc", typeof(string));//2
            dtCsv.Columns.Add("jcode", typeof(string));//3
            dtCsv.Columns.Add("Line", typeof(int));//4
            dtCsv.Columns.Add("MilesName", typeof(string));//5
            dtCsv.Columns.Add("RequiredBy", typeof(DateTime));//6
            dtCsv.Columns.Add("LeadTime", typeof(double));
            dtCsv.Columns.Add("ProjAcquistDate", typeof(string));
            dtCsv.Columns.Add("ActAcquDate", typeof(string));
            dtCsv.Columns.Add("Comments", typeof(string));
            dtCsv.Columns.Add("Type", typeof(int));//7
            dtCsv.Columns.Add("Department", typeof(string));//8
            dtCsv.Columns.Add("Quantity", typeof(double));//9
            dtCsv.Columns.Add("Price", typeof(double));//9
            dtCsv.Columns.Add("Amount", typeof(double));//9
            dtCsv.Columns.Add("OrderNo", typeof(int));
            dtCsv.Columns.Add("GroupId", typeof(int));
            dtCsv.Columns.Add("GroupName", typeof(string));
            dtCsv.Columns.Add("CodeDesc", typeof(string));//10
            dtCsv.Columns.Add("isUsed", typeof(int));



            string Fulltext;

            FileUploadControl.SaveAs(FileSaveWithPath);
            using (StreamReader sr = new StreamReader(FileSaveWithPath))
            {
                while (!sr.EndOfStream)
                {
                    Fulltext = sr.ReadToEnd().ToString();
                    Fulltext = Fulltext.TrimEnd();
                    string[] rows = Fulltext.Split('\n');

                    for (int i = 1; i < rows.Count(); i++)
                    {
                        System.Text.RegularExpressions.Regex CSVParser = new System.Text.RegularExpressions.Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        String[] rowValues = CSVParser.Split(rows[i]);

                        DataRow dr = dtCsv.NewRow();

                        dr["JobT"] = DBNull.Value;
                        dr["Job"] = DBNull.Value;
                        dr["ID"] = DBNull.Value;
                        dr["jType"] = DBNull.Value;
                        dr["fDesc"] = rowValues[7].ToString();
                        dr["jcode"] = rowValues[2].ToString();
                        dr["Line"] = rowValues[0].ToString();
                        dr["MilesName"] = rowValues[6].ToString();
                        dr["RequiredBy"] = rowValues[11].ToString();
                        dr["LeadTime"] = DBNull.Value;
                        dr["ProjAcquistDate"] = DBNull.Value;
                        dr["ActAcquDate"] = DBNull.Value;
                        dr["Comments"] = DBNull.Value;
                        dr["Type"] = rowValues[4].ToString() == "" ? 0 : Convert.ToInt32(rowValues[4].ToString());
                        dr["Department"] = rowValues[5].ToString();
                        dr["Quantity"] = rowValues[8].ToString() == "" ? 0 : Convert.ToDouble(rowValues[8].ToString());
                        dr["Price"] = rowValues[9].ToString() == "" ? 0 : Convert.ToDouble(rowValues[9].ToString());

                        dr["Amount"] = Convert.ToDouble(dr["Quantity"]) * Convert.ToDouble(dr["Price"]);
                        dr["OrderNo"] = DBNull.Value;


                        if (rowValues[1].ToString() != "")
                        {
                            JobT _objJob = new JobT();
                            BL_Job _objBLJob = new BL_Job();
                            DataSet dsGroup = new DataSet();
                            _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
                            if (hdnprojectID.Value != "")
                            {
                                _objJob.ID = Convert.ToInt32(hdnprojectID.Value);
                            }

                            _objJob.SearchValue = rowValues[1].ToString().Trim();
                            dsGroup = _objBLJob.GetGroupImport(_objJob);
                            if (dsGroup.Tables[0].Rows.Count > 0)
                            {
                                dr["GroupName"] = rowValues[1].ToString().Trim();
                                dr["GroupId"] = Convert.ToInt32(dsGroup.Tables[0].Rows[0]["value"]);
                            }
                            else
                            {
                                dr["GroupName"] = "";
                                dr["GroupId"] = 0.00;
                            }
                        }
                        else
                        {
                            dr["GroupId"] = 0.00;
                            dr["GroupName"] = "";
                        }





                        dr["CodeDesc"] = rowValues[3].ToString();
                        dr["isUsed"] = 0;

                        dtCsv.Rows.Add(dr);
                    }
                }
            }
            if (File.Exists(FileSaveWithPath))
                File.Delete(FileSaveWithPath);

        }
        catch (Exception ex)
        {
            if (File.Exists(FileSaveWithPath))
                File.Delete(FileSaveWithPath);
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        }

        return dtCsv;

    }
    public DataTable ReadExcelFile()
    {
        DataTable dtExcelData = new DataTable();
        try
        {
            dtExcelData.Columns.Add("JobT", typeof(int));
            dtExcelData.Columns.Add("Job", typeof(int));
            dtExcelData.Columns.Add("ID", typeof(int));
            dtExcelData.Columns.Add("jType", typeof(int));//1
            dtExcelData.Columns.Add("fDesc", typeof(string));//2
            dtExcelData.Columns.Add("jcode", typeof(string));//3
            dtExcelData.Columns.Add("Line", typeof(int));//4
            dtExcelData.Columns.Add("MilesName", typeof(string));//5
            dtExcelData.Columns.Add("RequiredBy", typeof(DateTime));//6
            dtExcelData.Columns.Add("LeadTime", typeof(double));
            dtExcelData.Columns.Add("ProjAcquistDate", typeof(string));
            dtExcelData.Columns.Add("ActAcquDate", typeof(string));
            dtExcelData.Columns.Add("Comments", typeof(string));
            dtExcelData.Columns.Add("Type", typeof(int));//7
            dtExcelData.Columns.Add("Department", typeof(string));//8
            dtExcelData.Columns.Add("Quantity", typeof(double));//9
            dtExcelData.Columns.Add("Price", typeof(double));//9
            dtExcelData.Columns.Add("Amount", typeof(double));//9
            dtExcelData.Columns.Add("OrderNo", typeof(int));
            dtExcelData.Columns.Add("GroupId", typeof(int));
            dtExcelData.Columns.Add("GroupName", typeof(string));
            dtExcelData.Columns.Add("CodeDesc", typeof(string));//10
            dtExcelData.Columns.Add("IsUsed", typeof(int));//10


            string ext = System.IO.Path.GetExtension(FileUploadControl.PostedFile.FileName).ToLower();

            string FileSaveWithPath = Server.MapPath("~\\TempPDF\\ImportBills\\Import" + System.DateTime.Now.ToString("ddMMyyyy_hhmmss") + ext);
            FileUploadControl.SaveAs(FileSaveWithPath);

            OleDbConnection oledbConn = new OleDbConnection();
            if (ext == ".xls")
                oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileSaveWithPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
            else if (ext == ".xlsx")
                oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileSaveWithPath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");

            oledbConn.Open();

            System.Data.DataTable dt = null;
            // Get the data table containg the schema guid.
            dt = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt != null && dt.Rows.Count > 0)
            {

                string firstSheetName = dt.Rows[0]["TABLE_NAME"].ToString();

                OleDbCommand ocmd = new OleDbCommand("select * from [" + firstSheetName + "]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter(ocmd);
                DataSet ds = new DataSet();
                oleda.Fill(ds);
                oledbConn.Close();

                if (File.Exists(FileSaveWithPath))
                    File.Delete(FileSaveWithPath);

                DataTable dtExcel = ds.Tables[0].Copy();

                for (int i = 0; dtExcel.Rows.Count > i; i++)
                {

                    DataRow dr = dtExcelData.NewRow();

                    dr["JobT"] = DBNull.Value;
                    dr["Job"] = DBNull.Value;
                    dr["ID"] = DBNull.Value;
                    dr["jType"] = DBNull.Value;
                    dr["fDesc"] = dtExcel.Rows[i]["Description"];
                    dr["jcode"] = dtExcel.Rows[i]["Sequence"];//jcode
                    dr["Line"] = dtExcel.Rows[i]["Line"];
                    dr["MilesName"] = dtExcel.Rows[i]["MilesName"];
                    dr["RequiredBy"] = dtExcel.Rows[i]["RequiredBy"];
                    dr["LeadTime"] = DBNull.Value;
                    dr["ProjAcquistDate"] = DBNull.Value;
                    dr["ActAcquDate"] = DBNull.Value;
                    dr["Comments"] = DBNull.Value;
                    dr["Type"] = dtExcel.Rows[i]["Type"];
                    dr["Department"] = dtExcel.Rows[i]["Function"];//Department
                    if (dtExcel.Rows[i]["Quantity"].ToString() != "")
                    {
                        dr["Quantity"] = dtExcel.Rows[i]["Quantity"];
                    }
                    else
                    {
                        dr["Quantity"] = 0.00;
                    }
                    if (dtExcel.Rows[i]["Price"].ToString() != "")
                    {
                        dr["Price"] = dtExcel.Rows[i]["Price"];
                    }
                    else
                    {
                        dr["Quantity"] = 0.00;
                    }


                    dr["Amount"] = Convert.ToDouble(dr["Quantity"]) * Convert.ToDouble(dr["Price"]);
                    dr["OrderNo"] = DBNull.Value;

                    if (dtExcel.Rows[i]["Group"].ToString() != "")
                    {
                        JobT _objJob = new JobT();
                        BL_Job _objBLJob = new BL_Job();
                        DataSet dsGroup = new DataSet();
                        _objJob.ConnConfig = HttpContext.Current.Session["config"].ToString();
                        if (hdnprojectID.Value != "")
                        {
                            _objJob.ID = Convert.ToInt32(hdnprojectID.Value);
                        }
                        else
                        {
                            _objJob.ID = 0;
                        }

                        _objJob.SearchValue = dtExcel.Rows[i]["Group"].ToString();
                        dsGroup = _objBLJob.GetGroupImport(_objJob);
                        if (dsGroup.Tables[0].Rows.Count > 0)
                        {
                            dr["GroupName"] = dtExcel.Rows[i]["Group"].ToString();
                            dr["GroupId"] = Convert.ToInt32(dsGroup.Tables[0].Rows[0]["value"]);
                        }
                        else
                        {
                            dr["GroupName"] = "";
                            dr["GroupId"] = 0.00;
                        }
                    }
                    else
                    {
                        dr["GroupName"] = "";
                        dr["GroupId"] = 0.00;
                    }





                    dr["CodeDesc"] = dtExcel.Rows[i]["CodeDesc"];
                    dr["IsUsed"] = 0;

                    dtExcelData.Rows.Add(dr);
                }
                return dtExcelData;
            }
            else
            {
                throw new Exception("Can't find a data sheet in import file. Please check your file again.");
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private void BindCustomDropDown(RadGrid RadGrid_Custom, DataTable dtCustomval)
    {
        try
        {
            foreach (GridDataItem gr in RadGrid_Custom.Items)
            {
                Label lblLine = (Label)gr.FindControl("lblLine");
                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("drpdwnCustom");

                if (dtCustomval != null && dtCustomval.Rows.Count > 0 && ddlCustomValue != null && ddlCustomValue.Visible)
                {
                    DataTable dt = dtCustomval.Clone();
                    DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                    foreach (DataRow row in result)
                    {
                        dt.ImportRow(row);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.DefaultView.Sort = "Value  ASC";
                        dt = dt.DefaultView.ToTable();
                    }

                    ddlCustomValue.DataSource = dt;
                    ddlCustomValue.DataTextField = "Value";
                    ddlCustomValue.DataValueField = "Value";
                    ddlCustomValue.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void InitTeamMemberGridView()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        if (Request.QueryString["uid"] != null)
        {
            objPropUser.JobId = Convert.ToInt32(Request.QueryString["uid"]);
        }
        else
        {
            objPropUser.JobId = 0;
        }

        //ds = objBL_User.GetUsersForTeamMemberList(objPropUser);
        ds = objBL_User.GetUsersAndRolesForTeamMemberList(objPropUser);
        var teamMembers = ds.Tables[0];

        // Get contacts list from exchange server
        //DataTable contactList = GetContactsForExchServer();
        DataTable contactList = WebBaseUtility.GetContactListOnExchangeServer();
        if (contactList != null && contactList.Rows.Count > 0)
        {
            // Merge this list to teamMembers
            foreach (DataRow item in contactList.Rows)
            {
                DataRow _dr = teamMembers.NewRow();
                if (string.IsNullOrEmpty(item["GroupName"].ToString()))
                {
                    _dr["memberkey"] = "3_" + item["Type"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange " + item["Type"];
                }
                else
                {
                    _dr["memberkey"] = "4_" + item["GroupName"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange Group: " + item["GroupName"];
                }
                _dr["fUser"] = item["MemberName"];
                _dr["email"] = item["MemberEmail"];
                teamMembers.Rows.Add(_dr);
            }
        }

        ViewState["AllProjectTeamMemberList"] = teamMembers;
        if (teamMembers != null && teamMembers.Rows.Count > 0)
        {
            RadGrid_Emails.DataSource = teamMembers;
            RadGrid_Emails.VirtualItemCount = teamMembers.Rows.Count;
        }
        else
        {
            RadGrid_Emails.DataSource = null;
        }
    }


    #region 'Job Custom'
    private void fillJobCustom()
    {
        try
        {
            DataSet ds = new DataSet();

            objJob.ConnConfig = Session["config"].ToString();
            objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
            ds = objBL_Job.GetJobCustomValueByJobId(objJob);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    lblJobCust1.InnerHtml = ds.Tables[0].Rows[0]["CustomLabel"].ToString();
                    txtJobCust1.Text = ds.Tables[0].Rows[0]["CustomValue"].ToString();


                    lblJobCust2.InnerHtml = ds.Tables[0].Rows[1]["CustomLabel"].ToString();
                    txtJobCust2.Text = ds.Tables[0].Rows[1]["CustomValue"].ToString();


                    lblJobCust3.InnerHtml = ds.Tables[0].Rows[2]["CustomLabel"].ToString();
                    txtJobCust3.Text = ds.Tables[0].Rows[2]["CustomValue"].ToString();


                    lblJobCust4.InnerHtml = ds.Tables[0].Rows[3]["CustomLabel"].ToString();
                    txtJobCust4.Text = ds.Tables[0].Rows[3]["CustomValue"].ToString();


                    lblJobCust5.InnerHtml = ds.Tables[0].Rows[4]["CustomLabel"].ToString();
                    txtJobCust5.Text = ds.Tables[0].Rows[4]["CustomValue"].ToString();

                    // -----------
                    lblJobCust6.InnerHtml = ds.Tables[0].Rows[5]["CustomLabel"].ToString();
                    txtJobCust6.Text = ds.Tables[0].Rows[5]["CustomValue"].ToString();

                    lblJobCust7.InnerHtml = ds.Tables[0].Rows[6]["CustomLabel"].ToString();
                    txtJobCust7.Text = ds.Tables[0].Rows[6]["CustomValue"].ToString();

                    lblJobCust8.InnerHtml = ds.Tables[0].Rows[7]["CustomLabel"].ToString();
                    txtJobCust8.Text = ds.Tables[0].Rows[7]["CustomValue"].ToString();

                    lblJobCust9.InnerHtml = ds.Tables[0].Rows[8]["CustomLabel"].ToString();
                    txtJobCust9.Text = ds.Tables[0].Rows[8]["CustomValue"].ToString();

                    lblJobCust10.InnerHtml = ds.Tables[0].Rows[9]["CustomLabel"].ToString();
                    txtJobCust10.Text = ds.Tables[0].Rows[9]["CustomValue"].ToString();
                    // -----------
                    lblJobCust11.InnerHtml = ds.Tables[0].Rows[10]["CustomLabel"].ToString();
                    txtJobCust11.Text = ds.Tables[0].Rows[10]["CustomValue"].ToString();

                    lblJobCust12.InnerHtml = ds.Tables[0].Rows[11]["CustomLabel"].ToString();
                    txtJobCust12.Text = ds.Tables[0].Rows[11]["CustomValue"].ToString();

                    lblJobCust13.InnerHtml = ds.Tables[0].Rows[12]["CustomLabel"].ToString();
                    txtJobCust13.Text = ds.Tables[0].Rows[12]["CustomValue"].ToString();

                    lblJobCust14.InnerHtml = ds.Tables[0].Rows[13]["CustomLabel"].ToString();
                    txtJobCust14.Text = ds.Tables[0].Rows[13]["CustomValue"].ToString();

                    lblJobCust15.InnerHtml = ds.Tables[0].Rows[14]["CustomLabel"].ToString();
                    txtJobCust15.Text = ds.Tables[0].Rows[14]["CustomValue"].ToString();
                    // -----------
                    lblJobCust16.InnerHtml = ds.Tables[0].Rows[15]["CustomLabel"].ToString();
                    txtJobCust16.Text = ds.Tables[0].Rows[15]["CustomValue"].ToString();

                    lblJobCust17.InnerHtml = ds.Tables[0].Rows[16]["CustomLabel"].ToString();
                    txtJobCust17.Text = ds.Tables[0].Rows[16]["CustomValue"].ToString();

                    lblJobCust18.InnerHtml = ds.Tables[0].Rows[17]["CustomLabel"].ToString();
                    txtJobCust18.Text = ds.Tables[0].Rows[17]["CustomValue"].ToString();

                    lblJobCust19.InnerHtml = ds.Tables[0].Rows[18]["CustomLabel"].ToString();
                    txtJobCust19.Text = ds.Tables[0].Rows[18]["CustomValue"].ToString();

                    lblJobCust20.InnerHtml = ds.Tables[0].Rows[19]["CustomLabel"].ToString();
                    txtJobCust20.Text = ds.Tables[0].Rows[19]["CustomValue"].ToString();
                }
            }


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void fillJobAttributeGeneralCustom()
    {
        try
        {
            DataSet ds = new DataSet();

            objJob.ConnConfig = Session["config"].ToString();
            objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
            ds = objBL_Job.GetJobAttributeGeneralCustomValueByJobId(objJob);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {

                    lblCustom1.InnerHtml = ds.Tables[0].Rows[0]["CustomLabel"].ToString();
                    //txtCustom1.Text = ds.Tables[0].Rows[0]["CustomValue"].ToString();

                    lblCustom2.InnerHtml = ds.Tables[0].Rows[1]["CustomLabel"].ToString();
                    //txtCustom2.Text = ds.Tables[0].Rows[1]["CustomValue"].ToString();


                    lblCustom3.InnerHtml = ds.Tables[0].Rows[2]["CustomLabel"].ToString();
                    //txtCustom3.Text = ds.Tables[0].Rows[2]["CustomValue"].ToString();


                    lblCustom4.InnerHtml = ds.Tables[0].Rows[3]["CustomLabel"].ToString();
                    //txtCustom4.Text = ds.Tables[0].Rows[3]["CustomValue"].ToString();


                    lblCustom5.InnerHtml = ds.Tables[0].Rows[4]["CustomLabel"].ToString();
                    //txtCustom5.Text = ds.Tables[0].Rows[4]["CustomValue"].ToString();


                }
            }


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void updateJobCustom(int jobid)
    {

        objJob.ConnConfig = Session["config"].ToString();
        //objJob.Username = Session["Username"].ToString();
        objJob.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objJob.Job = Convert.ToInt32(jobid);
        objJob.custom1 = txtJobCust1.Text;
        objJob.custom2 = txtJobCust2.Text;
        objJob.custom3 = txtJobCust3.Text;
        objJob.custom4 = txtJobCust4.Text;
        objJob.custom5 = txtJobCust5.Text;
        objJob.custom6 = txtJobCust6.Text;
        objJob.custom7 = txtJobCust7.Text;
        objJob.custom8 = txtJobCust8.Text;
        objJob.custom9 = txtJobCust9.Text;
        objJob.custom10 = txtJobCust10.Text;
        objJob.custom11 = txtJobCust11.Text;
        objJob.custom12 = txtJobCust12.Text;
        objJob.custom13 = txtJobCust13.Text;
        objJob.custom14 = txtJobCust14.Text;
        objJob.custom15 = txtJobCust15.Text;
        objJob.custom16 = txtJobCust16.Text;
        objJob.custom17 = txtJobCust17.Text;
        objJob.custom18 = txtJobCust18.Text;
        objJob.custom19 = txtJobCust19.Text;
        objJob.custom20 = txtJobCust20.Text;
        objBL_Job.UpdateJobCustomValue(objJob);
    }
    #endregion



    protected void lnkSearch_Click(object sender, EventArgs e)
    {

        Session["txtfrmDtValforEditjob"] = txtfromDate.Text;
        Session["txttoDtValforEditJob"] = txtToDate.Text;
        Session["ddlDateRangeFieldforEditJob"] = ddlDateRange.SelectedValue.ToString();


        if (Request.QueryString["uid"] != null)
        {
            BindSummary();
            ResetOnDemand();
        }
    }


    private void ResetOnDemand()
    {


        /////hdn
        //     hdnLoadLogs.Value= 
        //    hdnloadbomtab.Value= 
        //    hdnLoadgvBudget.Value= 
        //    hdnloadcontact.Value = 
        //    hdnloadequipment.Value= 
        //    hdnloadticket.Value = 
        //    hdnLoadAP_ARInvoices.Value = "0"; 

        //gvExpenses.Visible = false;
        //gvBudget.Visible = false;
        //gvTickets.Visible = false;
        //gvAPInvoices.Visible = false;
        //rtEquips.Visible = false;
        //gvInvoice.Visible = false; 
        //RadGrid_gvLogs.Visible = false;

        //divcontact.Visible = false; 
        //gvBOM.Visible = false;


    }


   

    protected void lnkTicketpostback_Click(object sender, EventArgs e)
    {
        if (hdnloadticket.Value == "0")
        {
            hdnloadticket.Value = "1";
            BindTicketList(string.Empty, 1);
        }
    }

    protected void lnkLoadAP_ARInvoices_Click(object sender, EventArgs e)
    {
        if (hdnLoadAP_ARInvoices.Value == "0")
        {
            hdnLoadAP_ARInvoices.Value = "1";
            GetAPInvoices();
            GetInvoices();
            gvAPInvoices.Visible = true;
            gvInvoice.Visible = true;
        }
    }

    protected void lnkloadgvBudget_Click(object sender, EventArgs e)
    {
        if (hdnLoadgvBudget.Value == "0")
        {
            hdnLoadgvBudget.Value = "1";
            BindBudget(1);
            gvBudget.Visible = true;
        }
    }


    protected void lnkloadbomtab_Click(object sender, EventArgs e)
    {
        FillGroupNameDropdown(Convert.ToInt32(Request.QueryString["uid"]));
        GetDataEquipmentForGrid();
        UpdateSelectedEquipmentBySelectedGroup(Convert.ToInt32(ddlProjectGroup.SelectedValue));
        gvBOM.Visible = true;
        hdnloadbomtab.Value = "1";

    }

    protected void lnkloadeqipment_Click(object sender, EventArgs e)
    {
        rtEquips.Visible = true;
        hdnloadequipment.Value = "1";
        BindEquip();


    }

    protected void lnkloadcontact_Click(object sender, EventArgs e)
    {
        hdnloadcontact.Value = "1";
        divcontact.Visible = true;
        Fill_gvContacts();
        gvContacts.Visible = true;
    }

   

    private void FillProjectManager(int typeId)
    {
        try
        {
            DataSet _dsProjectManager = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            objJob.TypeId = typeId;
            _dsProjectManager = objBL_Job.GetProjectManagerByTypeId(objJob);
            ddlProjectManger.Items.Clear();
            if (_dsProjectManager.Tables[0].Rows.Count > 0)
            {

                ddlProjectManger.Items.Add(new ListItem("Select Project Manager", "Select Project Manager"));
                ddlProjectManger.AppendDataBoundItems = true;
                ddlProjectManger.DataSource = _dsProjectManager;
                ddlProjectManger.DataValueField = "ID";
                ddlProjectManger.DataTextField = "fUser";
                ddlProjectManger.DataBind();
            }
            else
            {
                ddlProjectManger.Items.Add(new ListItem("No data found", "Select Project Manager"));
            }
            //if (_dsProjectManager.Tables[0].Rows.Count == 1)
            //{
            //    ddlProjectManger.SelectedValue = _dsProjectManager.Tables[0].Rows[0]["ID"].ToString();
            //    updateTeamAfterSelectProjectManager();
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlProjectManger_SelectedIndexChanged(object sender, EventArgs e)
    {
        updateTeamAfterSelectProjectManager();

    }

    private void updateTeamAfterSelectProjectManager()
    {
        String first = "";
        String last = "";
        String email = "";
        String phone = "";
        if (!ddlProjectManger.SelectedValue.Equals("Select Project Manager"))
        {
            DataSet ds = new DataSet();
            ds = objBL_Job.GetUserDetailByEmpID(Session["config"].ToString(), Convert.ToInt32(ddlProjectManger.SelectedValue));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    last = ds.Tables[0].Rows[0]["LastName"].ToString();
                    email = ds.Tables[0].Rows[0]["Email"].ToString();
                    phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                }
            }

            if (Request.QueryString["uid"] != null)
            {
                //Get Team
                DataTable dt = GetTeamItemsForProject();
                DataRow[] foundUser = dt.Select("UserID = '" + ddlProjectManger.SelectedItem.Text + "'");
                if (foundUser.Length == 0)
                {

                    DataRow[] foundProjectManager = dt.Select("Title = 'Project Manager'");
                    if (foundProjectManager.Length > 0)
                    {
                        foreach (DataRow dr in dt.Rows) // search whole table
                        {
                            if (dr["Title"].ToString() == "Project Manager")
                            {
                                dr["UserID"] = ddlProjectManger.SelectedItem.Text;
                                dr["FirstName"] = first;
                                dr["LastName"] = last;
                                dr["Email"] = email;
                                dr["Mobile"] = phone;
                                gvTeamItems.DataSource = dt;
                                gvTeamItems.DataBind();
                                break;
                            }
                        }
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);
                        dr["JobID"] = Convert.ToInt32(Request.QueryString["uid"]);
                        dr["Title"] = "Project Manager";
                        dr["UserID"] = ddlProjectManger.SelectedItem.Text;
                        dr["FirstName"] = first;
                        dr["LastName"] = last;
                        dr["Email"] = email;
                        dr["Mobile"] = phone;
                        gvTeamItems.DataSource = dt;
                        gvTeamItems.DataBind();

                    }
                }
            }
        }
    }
    //Projects note

    private void LoadProjectNoteExport(String ls)
    {
        BL_Customer objBL_Customer = new BL_Customer();
        Customer objProp_Note = new Customer();
        objProp_Note.ConnConfig = Session["config"].ToString();
        objProp_Note.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
        DataSet dsNotes = objBL_Customer.GetJobProject_NotesExport(objProp_Note, ls);
        if (dsNotes.Tables[0].Rows.Count > 0)
        {
            RadGrid_CollectionNotes_Export.DataSource = dsNotes.Tables[0];


        }
    }

    protected void RadGrid_CollectionNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            getProjectNotes();
        }

    }

    private void getProjectNotes()
    {
        BL_Customer objBL_Customer = new BL_Customer();
        Customer objProp_Note = new Customer();
        objProp_Note.ConnConfig = Session["config"].ToString();
        objProp_Note.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
        DataSet dsNotes = objBL_Customer.GetJobProject_Notes(objProp_Note);
        if (dsNotes.Tables[0].Rows.Count > 0)
        {
            RadGrid_CollectionNotes.VirtualItemCount = dsNotes.Tables[0].Rows.Count;
            RadGrid_CollectionNotes.DataSource = dsNotes.Tables[0];


        }
    }

    protected void lnkSaveNote_Click(object sender, EventArgs e)
    {

        BL_Customer objBL_Customer = new BL_Customer();
        Customer objProp_Note = new Customer();
        objProp_Note.ConnConfig = Session["config"].ToString();
        objProp_Note.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objProp_Note.Notes = txtNote.Text;
        objProp_Note.UserID = Convert.ToInt32(Session["UserID"]);
        objBL_Customer.AddJobProject_Notes(objProp_Note);
        getProjectNotes();
        RadGrid_CollectionNotes.Rebind();
        RadGrid_gvLogs.Rebind();
        UpdatePanel8.Update();
        txtNote.Text = "";
    }


    protected void lnkExportNotes_Click(object sender, EventArgs e)
    {
        String listID = "";
        foreach (GridDataItem item in RadGrid_CollectionNotes.SelectedItems)
        {

            HiddenField hdnProjectNoteID = (HiddenField)item.FindControl("hdnProjectNoteID");
            listID = listID + hdnProjectNoteID.Value + ",";
        }
        //ViewState["NoteSelected"] = listID;
        hdnlsNoteID.Value = listID;
        LoadProjectNoteExport(listID);
        RadGrid_CollectionNotes_Export.Rebind();
        RadGrid_CollectionNotes_Export.ExportSettings.FileName = "ProjectNote";
        RadGrid_CollectionNotes_Export.ExportSettings.IgnorePaging = true;
        RadGrid_CollectionNotes_Export.ExportSettings.OpenInNewWindow = true;
        RadGrid_CollectionNotes_Export.ExportSettings.ExportOnlyData = true;
        RadGrid_CollectionNotes_Export.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_CollectionNotes_Export.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_CollectionNotes_ExcelMLExportRowCreated(object sender, Telerik.Web.UI.GridExcelBuilder.GridExportExcelMLRowCreatedArgs e)
    {
        List<String> lsID = new List<String>();
        //if (ViewState["NoteSelected"] != null)
        //{
        //    lsID = ViewState["NoteSelected"].ToString().Split(',').ToList();

        //}
        if (hdnlsNoteID.Value != "")
        {
            lsID = hdnlsNoteID.Value.ToString().Split(',').ToList();

        }

        foreach (GridDataItem item in RadGrid_CollectionNotes.Items)
        {
            if (!lsID.Contains(item.ItemIndex.ToString()) && e.RowType == GridExportExcelMLRowType.DataRow)
            {
                e.Worksheet.Table.Rows.Remove(e.Row);
            }

        }
        //ViewState["NoteSelected"] = null;
        hdnlsNoteID.Value = "";
    }

    protected void RadGrid_CollectionNotes_Export_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        if (ViewState["NoteSelected"] != null)
        {
            LoadProjectNoteExport(ViewState["NoteSelected"].ToString());
        }

    }

    private void ExportNoteToExcel()
    {
        String listID = "";
        foreach (GridDataItem item in RadGrid_CollectionNotes.SelectedItems)
        {

            HiddenField hdnProjectNoteID = (HiddenField)item.FindControl("hdnProjectNoteID");
            listID = listID + hdnProjectNoteID.Value + ",";
        }
        ViewState["NoteSelected"] = listID;
        LoadProjectNoteExport(listID);
        RadGrid_CollectionNotes_Export.Rebind();
        RadGrid_CollectionNotes_Export.ExportSettings.FileName = "ProjectNote";
        RadGrid_CollectionNotes_Export.ExportSettings.IgnorePaging = true;
        RadGrid_CollectionNotes_Export.ExportSettings.OpenInNewWindow = true;
        RadGrid_CollectionNotes_Export.ExportSettings.ExportOnlyData = true;
        RadGrid_CollectionNotes_Export.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_CollectionNotes_Export.MasterTableView.ExportToExcel();
    }


    protected void lnkEmailNote_Click(object sender, EventArgs e)
    {

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

    protected void RadGrid_gvLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
            BindLogs();
        }
        catch { }
    }

    protected void lnkLoadLogs_Click(object sender, EventArgs e)
    {
        hdnLoadLogs.Value = "1";
        RadGrid_gvLogs.Rebind();
        RadGrid_gvLogs.Visible = true;
    }

    private void BindLogs()
    {
        if (Request.QueryString["uid"] != null && hdnLoadLogs.Value == "1")
        {
            DataSet dsLog = new DataSet();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.LogRefId = Convert.ToInt32(Request.QueryString["uid"]);
            objProp_Customer.LogScreen = "Project";
            dsLog = objBL_Customer.GetLogs(objProp_Customer);
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
        else
        {
            RadGrid_gvLogs.DataSource = string.Empty;
        }
    }

    protected void lnkSetTH_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {

            foreach (GridDataItem row in RadTHGrid.Items) // loops through each rows in RadGrid
            {
                TextBox TargetHours = (TextBox)row.FindControl("txtTH");

                HiddenField hdnTargetHours = (HiddenField)row.FindControl("hdnTargetHours");

                Label Code = (Label)row.FindControl("lblcode");

                Label GroupName = (Label)row.FindControl("lblgp");

                string ConnConfig = Session["config"].ToString();

                int ProjectId = Convert.ToInt32(Request.QueryString["uid"]);

                int HoursReduce = 0;

                int isMassupdatetargetedhoursby = chkMassupdatetargetedhoursby.Checked == true ? 1 : 0;

                int isCopytargetedhoursoverbudgethours = chkCopytargetedhoursoverbudgethours.Checked == true ? 1 : 0;

                int isMassupdatetargeted = chkCopytargetedhoursoverbudgethours.Checked == true ? 1 : 0;


                int.TryParse(txthoursreduce.Text, out HoursReduce);


                if (TargetHours.Text != string.Empty && !string.IsNullOrEmpty(TargetHours.Text))
                {

                    objBL_Job.spSetTargetHours(ConnConfig, ProjectId, Code.Text, GroupName.Text, TargetHours.Text, HoursReduce, isMassupdatetargetedhoursby, isCopytargetedhoursoverbudgethours, isMassupdatetargeted);
                }
            }

            LoadDataExportBOM();

            gvBOM.Rebind();

            BindBudget(1);

            gvBudget.Rebind();

            chkMassupdatetargetedhoursby.Checked = chkCopytargetedhoursoverbudgethours.Checked = chkCopytargetedhoursoverbudgethours.Checked = false;

            txthoursreduce.Text = "";
        }
        string script = "function f(){$find(\"" + RadWindowth.ClientID + "\").hide(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key2", script, true);
    }

    protected void lnktargeted_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            string ConnConfig = Session["config"].ToString();

            int ProjectId = Convert.ToInt32(Request.QueryString["uid"]);

            DataSet ds = new DataSet();

            ds = objBL_Job.spGetTargetHours(ConnConfig, ProjectId);

            RadTHGrid.DataSource = ds.Tables[0];

            RadTHGrid.DataBind();

            string script = "function f(){$find(\"" + RadWindowth.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key1", script, true);
        }
    }

    protected void gvBudget_PreRender(object sender, EventArgs e)
    {

        gvBudget.MasterTableView.GroupByExpressions.Clear();

        if (Request.QueryString["uid"] != null)
        {

            GridGroupByField field = new GridGroupByField();
            GridGroupByExpression ex = new GridGroupByExpression();
            field.FieldName = "JobType";
            field.HeaderText = "ProjectType";
            field.SetSortOrder("Descending");
            ex.GroupByFields.Add(field);
            GridGroupByField _field = new GridGroupByField();
            _field.FieldName = "JobType";
            _field.HeaderText = "ProjectType";
            ex.SelectFields.Add(_field);

            gvBudget.MasterTableView.GroupByExpressions.Add(ex);



            if (chkTargetHours.Checked)
            {
                gvBudget.Columns.FindByUniqueName("TargetHours").Visible = true;
                gvBudget.Columns.FindByUniqueName("TVH").Visible = true;


                GridGroupByField field1 = new GridGroupByField();
                GridGroupByExpression ex1 = new GridGroupByExpression();
                field1.FieldName = "GroupName";
                field1.HeaderText = "GroupName";
                ex1.GroupByFields.Add(field1);
                ex1.SelectFields.Add(field1);
                gvBudget.MasterTableView.GroupByExpressions.Add(ex1);




                GridGroupByField field2 = new GridGroupByField();
                GridGroupByExpression ex2 = new GridGroupByExpression();
                field2.FieldName = "Code";
                field2.HeaderText = "Code";
                ex2.GroupByFields.Add(field2);
                ex2.SelectFields.Add(field2);

                GridGroupByField field3 = new GridGroupByField();
                field3.FieldName = "GTargetHours";
                field3.HeaderText = "TargetHours";
                ex2.GroupByFields.Add(field3);
                ex2.SelectFields.Add(field3);



                GridGroupByField field4 = new GridGroupByField();
                field4.FieldName = "GBudgetHours";
                field4.HeaderText = "BudgetHours";
                ex2.SelectFields.Add(field4);

                GridGroupByField field5 = new GridGroupByField();
                field5.FieldName = "THours";
                field5.HeaderText = "ActualHours";
                field5.Aggregate = GridAggregateFunction.Sum;
                ex2.SelectFields.Add(field5);


                gvBudget.MasterTableView.GroupByExpressions.Add(ex2);
            }
            else
            {

                gvBudget.Columns.FindByUniqueName("TargetHours").Visible = false;
                gvBudget.Columns.FindByUniqueName("TVH").Visible = false;
            }


            gvBudget.Rebind();
        }
    }
    protected void lnkPopupUpdateGroup_Click(object sender, EventArgs e)
    {
        try
        {
            var isAddNew = string.IsNullOrEmpty(ddlProjectGroup.SelectedValue) || ddlProjectGroup.SelectedValue == "0" ? true : false;
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.GroupName = txtPopupGroupName.Text;
            objProp_Customer.LocID = Convert.ToInt32(hdnLocID.Value);
            objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"]);
            objProp_Customer.GroupId = Convert.ToInt32(ddlProjectGroup.SelectedValue);
            objProp_Customer.DtEquips = GetSelectedEquipmentsFromUI();
            DataSet ds = new DataSet();
            //if (objProp_Customer.ProjectJobID != 0)
            //{
            if (!string.IsNullOrWhiteSpace(objProp_Customer.GroupName))
            {
                ds = objBL_Customer.AddProjectGroup(objProp_Customer);

                if (ds.Tables.Count > 0)
                {
                    if (objProp_Customer.ProjectJobID != 0)
                    {
                        ddlProjectGroup.DataSource = ds.Tables[0];
                    }
                    else
                    {
                        var groupId = ds.Tables[1].Rows[0][0];
                        DataTable dt;
                        if (Session["NewProjGroupList"] != null)
                        {
                            dt = (DataTable)Session["NewProjGroupList"];
                            DataRow row = dt.Select("Id=" + groupId.ToString()).FirstOrDefault();
                            if (row != null)
                            {
                                row["GroupName"] = objProp_Customer.GroupName;
                            }
                            else
                            {
                                DataRow dr = dt.NewRow();
                                dr["GroupName"] = objProp_Customer.GroupName;
                                dr["Id"] = ds.Tables[1].Rows[0][0];
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            dt = new DataTable();
                            dt.Columns.Add(new DataColumn("GroupName", typeof(string)));
                            dt.Columns.Add(new DataColumn("Id", typeof(Int32)));
                            DataRow dr = dt.NewRow();
                            dr["GroupName"] = objProp_Customer.GroupName;
                            dr["Id"] = ds.Tables[1].Rows[0][0];
                            dt.Rows.Add(dr);
                        }

                        ddlProjectGroup.DataSource = dt;
                        Session["NewProjGroupList"] = dt;
                    }
                    ddlProjectGroup.DataTextField = "GroupName";
                    ddlProjectGroup.DataValueField = "Id";
                    ddlProjectGroup.DataBind();
                    ddlProjectGroup.Items.Insert(0, new ListItem("Select Group", "0"));

                    var currentSelectedVal = ds.Tables[1].Rows[0][0].ToString();
                    //SetSelectedValueForDDL(ddlEstimateGroup, currentSelectedVal);
                    if (ddlProjectGroup.Items.FindByValue(currentSelectedVal) != null)
                    {
                        ddlProjectGroup.SelectedValue = currentSelectedVal;
                    }
                    else
                    {
                        ddlProjectGroup.SelectedValue = null;
                    }
                }
               
                if (!isAddNew)
                {
                    DataSet BomDS = objBL_Customer.getJobProject_BOM(objProp_Customer);
                    if (BomDS.Tables[0].Rows.Count > 0)
                    {
                        BindgvBOM(BomDS.Tables[0]);

                    }
                    else
                    {
                        CreateBOMTable();
                    }
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Group updated successful!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'A new group was created successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "closeScript", "CloseRadWindowGroup();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
            }
            else
            {
                string strErr = "The group name cannot be blank";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + strErr + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCancelUpdateStatus_Click(object sender, EventArgs e)
    {
        hdnWIPID.Value = "";
        GetWIP();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "HiddenDetailTab", "HiddenDetailTab();", true);

    }

    protected void lnkUpdateStatus_Click(object sender, EventArgs e)
    {
        processUpdateWipStatus();
    }

    private void processUpdateWipStatus()
    {
        GridEditableItem edit = gvProgressBilling.Items[gvProgressBilling.Items.Count - 1];
        DropDownList ddlApplicationStatus = (DropDownList)edit.FindControl("ddlApplicationStatus");

        hdnWIPID.Value = ddlApplicationStatus.Attributes["data-id"].ToString();
        String status = ddlApplicationStatus.Attributes["data-InvStatus"].ToString();
        if (status == "0")
        {
            UpdateStatusWIP(Convert.ToInt32((ddlApplicationStatus.SelectedValue)));
        }
        else
        {
            hdnWIPID.Value = "";
            GetWIP();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HiddenDetailTab", "HiddenDetailTab();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'This invoice is paid and cannot be edited.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});MoveToThirdTab();", true);
        }
    }

    protected void ddlProjectGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateSelectedEquipmentBySelectedGroup(Convert.ToInt32(ddlProjectGroup.SelectedValue));
        if (!string.IsNullOrEmpty(ddlProjectGroup.SelectedValue) && ddlProjectGroup.SelectedValue != "0")
        {
            lnkAddGroupName.ToolTip = "Edit Group Name";
        }
        else
        {
            lnkAddGroupName.ToolTip = "Add Group Name";
        }
    }

    private void FillGroupNameDropdown(int projectId)
    {
        //DropDownList ddlTemplate = gvTemplateItems.FooterRow.FindControl("ddlTemplate") as DropDownList;
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ProjectJobID = projectId;
        if (projectId != 0)
        {
            ds = objBL_Customer.GetProjectGroupNames(objProp_Customer);
            ddlProjectGroup.DataSource = ds.Tables[0];
        }
        else
        {
            DataTable dt;
            if (Session["NewProjGroupList"] != null)
            {
                dt = (DataTable)Session["NewProjGroupList"];
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("GroupName", typeof(string)));
                dt.Columns.Add(new DataColumn("Id", typeof(Int32)));
            }

            ddlProjectGroup.DataSource = dt;
        }
        ddlProjectGroup.DataTextField = "GroupName";
        ddlProjectGroup.DataValueField = "Id";
        ddlProjectGroup.DataBind();

        ddlProjectGroup.Items.Insert(0, new ListItem("Select Group", "0"));

        if (ddlProjectGroup.Items.Count == 2)
        {
            ddlProjectGroup.SelectedIndex = 1;
        }
    }

    protected void RadgvEquip_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

    }

    protected void RadgvEquip_DataBound(object sender, EventArgs e)
    {

    }

    private void GetDataEquipmentForGrid()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        if (!string.IsNullOrEmpty(hdnLocID.Value) && hdnLocID.Value != "0")
        {
            objPropUser.SearchBy = string.Empty;
            objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
            //HyperLinkAddEquip.NavigateUrl = "addequipment.aspx?lid=" + hdnLocID.Value + "&locname=" + txtCont.Text + "&addFrom=Ticket";
            //objPropUser.SearchBy = "e.loc";
            //objPropUser.SearchValue = hdnLocId.Value;
            objPropUser.InstallDate = string.Empty;
            objPropUser.ServiceDate = string.Empty;
            objPropUser.Price = string.Empty;
            objPropUser.Manufacturer = string.Empty;
            objPropUser.Status = -1;
            #region Company Check
            objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            if (Convert.ToString(HttpContext.Current.Session["CmpChkDefault"]) == "1")
            {
                objPropUser.EN = 1;
            }
            else
            {
                objPropUser.EN = 0;
            }
            #endregion
            ds = objBL_User.getElev(objPropUser);
            RadgvEquip.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadgvEquip.DataSource = ds.Tables[0];
            RadgvEquip.DataBind();
        }
        //else
        //{
        //    objPropUser.ProspectID = Convert.ToInt32(hdProspect.Value);
        //    ds = objBL_User.getLeadEquip(objPropUser);

        //    RadgvEquip.VirtualItemCount = ds.Tables[0].Rows.Count;
        //    RadgvEquip.DataSource = ds.Tables[0];
        //    RadgvEquip.DataBind();
        //}
    }

    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridDataItem gr in RadgvEquip.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            chkSelect.Attributes["onclick"] = "SelectRowsEq1();";
        }
    }

    private DataTable GetSelectedEquipmentsFromUI()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("EquipmentID", typeof(int));

            foreach (GridDataItem gvr in RadgvEquip.Items)
            {
                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");

                if (chkSelect.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    Label lblUnit = (Label)gvr.FindControl("lblID");

                    dr["EquipmentID"] = Convert.ToInt32(lblUnit.Text);

                    dt.Rows.Add(dr);
                }
            }
        }
        catch { }
        return dt;
    }

    private void UpdateSelectedEquipmentBySelectedGroup(int groupID)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.GroupId = groupID;
        ds = objBL_Customer.GetEquipmentsByGroupId(objProp_Customer);
        txtGroupEquips.Text = string.Empty;
        // Reset all checkbox of RadgvEquip
        foreach (GridDataItem gr in RadgvEquip.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            chkSelect.Checked = false;
        }

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            foreach (GridDataItem gr in RadgvEquip.Items)
            {
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblname = (Label)gr.FindControl("lblunit");
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (dr["EquipmentID"].ToString() == lblID.Text)
                {
                    chkSelect.Checked = true;

                    if (txtGroupEquips.Text != string.Empty)
                    {
                        txtGroupEquips.Text = txtGroupEquips.Text + ", " + lblname.Text;
                    }
                    else
                    {
                        txtGroupEquips.Text = lblname.Text;
                    }
                }
            }
        }

        //SelectRowsEq1();
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "SelectEquipment", "SelectRowsEq1();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void btnAttachmentDel_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string path = btn.CommandArgument;
        DownloadDocument(path, Path.GetFileName(path));
    }

    protected void imgDelAttach_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        string path = btn.CommandArgument;
        if (hdnFirstAttachement.Value == path)
        {
            hdnFirstAttachement.Value = "-1";
        }
        ArrayList lstPath = (ArrayList)ViewState["pathmailatt"];
        lstPath.Remove(path);
        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();
        DeleteFile(path);
        // To keep open this popup
        ModalPopupSendEmail.Show();
        //PrintInvoices(false);
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
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, Encoding.UTF8) + lastUpdateTiemStamp;

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = Encoding.UTF8;

                //Send data
                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //Dividing the data in 1024 bytes package
                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                int i;
                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                {
                    Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    Response.Flush();
                }
                ////if blocks transfered not equals total number of blocks
                //if (i < maxCount)
                //    return false;
                //return true; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Response.End();
                _BinaryReader.Close();
                myFile.Close();
            }
        }
        catch (FileNotFoundException ex)
        {
            if (DownloadFileName == "Invoice.pdf")
            {
                //byte[] buffer1 = null;

                //var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                //var service = new Stimulsoft.Report.Export.StiPdfExportService();
                //System.IO.MemoryStream stream = new System.IO.MemoryStream();
                //service.ExportTo(PrintInvoices(false), stream, settings);
                //buffer1 = stream.ToArray();

                //Response.Clear();
                //MemoryStream ms = new MemoryStream(buffer1);
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
                //Response.Buffer = true;
                //ms.WriteTo(Response.OutputStream);
                //Response.End();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    private void DeleteFile(string filepath)
    {
        ////this should delete the file in the next reboot, not now.
        //MoveFileEx(filepath, null, MoveFileFlags.MOVEFILE_DELAY_UNTIL_REBOOT);

        if (System.IO.File.Exists(filepath))
        {
            // Use a try block to catch IOExceptions, to 
            // handle the case of the file already being 
            // opened by another process. 
            try
            {
                System.IO.File.Delete(filepath);
            }
            catch //(System.IO.IOException e)
            {
                //Console.WriteLine(e.Message);
                //return;
            }
        }
    }

    protected void lnkUploadAttachment_Click(object sender, EventArgs e)
    {
        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        string savepath = savepathconfig + @"\mailattach\";
        string filename = FileUpload1.FileName;

        if (filename.Equals("Invoice.pdf", StringComparison.InvariantCultureIgnoreCase))
        {
            filename = objgn.generateRandomString(4) + "_" + filename;
        }

        string fullpath = savepath + filename;

        if (File.Exists(fullpath))
        {
            filename = objgn.generateRandomString(4) + "_" + filename;
            fullpath = savepath + filename;
        }

        if (!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }
        FileUpload1.SaveAs(fullpath);


        ArrayList lstPath = new ArrayList();
        if (ViewState["pathmailatt"] != null)
        {
            lstPath = (ArrayList)ViewState["pathmailatt"];
            lstPath.Add(fullpath);
        }
        else
        {
            lstPath.Add(fullpath);
        }

        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();
        //txtBodyCKE.Focus();
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
            DataSet dsEquip = new DataSet();
            DataTable dtInstallationItems = new DataTable();

            objProp_Contracts.ConnConfig = Session["config"].ToString();

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

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

            // Load for Port City Installation template
            bool pceInstallation = false;

            if (ConfigurationManager.AppSettings["CustomerName"] == "PCE")
            {
                if (dtInvoice.Rows.Count > 0)
                {
                    var _objUser = new User();
                    _objUser.ConnConfig = Session["config"].ToString();
                    _objUser.SearchBy = string.Empty;

                    _objUser.LocID = Convert.ToInt32(dtInvoice.Rows[0]["Loc"]);
                    _objUser.InstallDate = string.Empty;
                    _objUser.ServiceDate = string.Empty;
                    _objUser.Price = string.Empty;
                    _objUser.Manufacturer = string.Empty;
                    _objUser.Status = -1;
                    _objUser.building = "All";

                    dsEquip = objBL_User.getElev(_objUser);

                    if (dtInvoice.Rows[0]["TypeName"] != null && dtInvoice.Rows[0]["TypeName"].ToString() == "Installation")
                    {
                        pceInstallation = true;
                        templateName = "InstallationInvoiceWithTicket-PortCity.mrt";
                        dtInstallationItems = GetPCEInstallationInvoiceItems(dtInvoice, dtInvItems);
                    }
                }
            }

            string reportPathStimul = Server.MapPath(string.Format("StimulsoftReports/Invoices/{0}", templateName));
            StiReport report = new StiReport();
            report.Load(reportPathStimul);

            report.RegData("CompanyDetails", companyInfo.Tables[0]);
            report.RegData("InvoiceInfo", dtInvoice);
            report.RegData("Tickets", dtTicket);

            if (pceInstallation)
            {
                report.RegData("InvoiceItems", dtInstallationItems);
            }
            else
            {
                report.RegData("InvoiceItems", dtInvItems);
            }

            if (report.DataSources.Contains("Equipments") && dsEquip.Tables.Count > 0)
            {
                report.RegData("Equipments", dsEquip.Tables[0]);
            }

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

    private void CreateTaskOnProjectUpdated(string strSubject, string strRemarks, string assignedTo, int projectId, string strMailTo = "")
    {
        var objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.ROL = Convert.ToInt32(hdnLocRolID.Value);
        objCustomer.DueDate = DateTime.Now;
        objCustomer.TimeDue = Convert.ToDateTime("01/01/1900 " + DateTime.Now.ToShortTimeString());
        objCustomer.Subject = strSubject;
        objCustomer.Remarks = strRemarks;
        objCustomer.AssignedTo = assignedTo;
        double dblDuration = 0.5;
        objCustomer.Duration = dblDuration;
        objCustomer.Name = Session["Username"].ToString();
        //objProp_Customer.Contact = txtContact.Text;
        objCustomer.Status = 0;//Open
        objCustomer.Resolution = "";
        objCustomer.LastUpdateUser = Session["username"].ToString();
        objCustomer.Category = "To Do";
        objCustomer.IsAlert = true;

        try
        {
            objCustomer.TaskID = 0;
            objCustomer.Mode = 0;
            objCustomer.Screen = "Project";

            objCustomer.Ref = projectId;
            objBL_Customer.AddTask(objCustomer);

            #region Thomas: Send email with a appointment to login user 
            if (objCustomer.IsAlert)
            {
                // Create
                BusinessEntity.User objPropUser = new BusinessEntity.User();
                BL_User objBL_User = new BL_User();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Username = assignedTo;
                //objPropUser.UserID = Convert.ToInt32(ddlAssigned.SelectedItem.Value);
                var mailTo = string.Empty;
                if (!string.IsNullOrEmpty(strMailTo))
                {
                    mailTo = strMailTo;
                }
                else
                {
                    mailTo = objBL_User.getUserEmail(objPropUser);
                }

                if (!string.IsNullOrEmpty(mailTo))
                {
                    Mail mail = new Mail();
                    try
                    {
                        var uri = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath + "/addTask?uid=" + objCustomer.TaskID.ToString());
                        foreach (var toaddress in mailTo.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            mail.To.Add(toaddress.Trim());
                        }
                        //mail.To.Add(mailTo);
                        mail.From = WebBaseUtility.GetFromEmailAddress();
                        //mail.Title = "Task Appointment";
                        mail.Title = txtLocation.Text + ": " + objCustomer.Subject;
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendFormat("Dear {0}<br><br>", objCustomer.AssignedTo);
                        stringBuilder.Append("You are receiving an appointment task from MOM-->Sales-->Tasks<br><br>");
                        stringBuilder.AppendFormat("Customer Name: {0}<br>", txtCustomer.Text);
                        stringBuilder.AppendFormat("Location Name: {0}<br>", txtLocation.Text);
                        //stringBuilder.AppendFormat("Contact Name: {0}, Phone: {1}, Email: {2}<br>", txtContact.Text, txtContactPhone.Text, txtContactEmail.Text);
                        stringBuilder.AppendFormat("Subject: {0}<br>", objCustomer.Subject);
                        stringBuilder.AppendFormat("Description: {0}<br>", objCustomer.Remarks);
                        stringBuilder.AppendFormat("Due on: {0} {1}<br><br>", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        stringBuilder.Append("Attached files is a task appointment assigned to you.<br>");
                        stringBuilder.Append("To add this appointment to your calendar, please open and save it<br><br>");
                        stringBuilder.AppendFormat("<a href={0}>{0}</a><br><br>", uri);
                        stringBuilder.Append("Thanks");

                        mail.Text = stringBuilder.ToString();

                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                        //var apSubject = string.Format("Task name: {0}", objCustomer.Subject);
                        var apSubject = string.Format("Task name: {0}", txtCustomer.Text);

                        //StringBuilder apBody = new StringBuilder();
                        //strRemarks = strRemarks.Replace("\r\n", "=0D=0A").Replace("\n", "=0D=0A");
                        //apBody.AppendFormat("{0} =0D=0A", strRemarks);
                        //apBody.AppendFormat("Due on: {0} {1} =0D=0A", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        //apBody.Append("Attached files is a task appointment assigned to you. =0D=0A");
                        //apBody.Append("To add this appointment to your calendar, please open and save it. =0D=0A");
                        ////apBody.AppendFormat("{0}=0D=0A", uri);
                        //apBody.Append("Thanks");

                        StringBuilder apBody = new StringBuilder();
                        var _strRemarks = objCustomer.Remarks.Replace("\r\n", "=0D=0A").Replace("\n", "=0D=0A");
                        apBody.AppendFormat("{0}.=0D=0A", _strRemarks);
                        apBody.AppendFormat("Due on: {0} {1}. =0D=0A ", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        apBody.Append("Attached files is a task appointment assigned to you.  =0D=0A");
                        //apBody.Append("To add this appointment to your calendar, please open and save it.=0D=0A");
                        apBody.Append("To add this appointment to your calendar, please open and save it.=0D=0A");
                        //apBody.Append("=0D=0A");
                        //apBody.AppendFormat("{0}", uri);
                        //apBody.Append("=0D=0A");
                        apBody.Append("Thanks");


                        var strStartDate = string.Format("{0} {1}", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        var apStart = Convert.ToDateTime(strStartDate);
                        var apEnd = apStart.AddHours(objCustomer.Duration);

                        var icsAttachmentContentsStr = WebBaseUtility.CreateICSAttachmentCalendarStr(apSubject
                            , apBody.ToString()
                            , txtLocation.Text
                            , apStart
                            , apEnd
                            , 60
                            );
                        var myByteArray = System.Text.Encoding.UTF8.GetBytes(icsAttachmentContentsStr);
                        mail.attachmentBytes = myByteArray;
                        mail.FileName = "TaskAppointment.ics";
                        mail.Send();
                    }
                    catch (Exception ex)
                    {
                        //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
            }
            #endregion

        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }

        //  Response.Redirect("AddProspect.aspx?uid=" + ProspectID);
    }

    protected void lnkAddTask_Click(object sender, EventArgs e)
    {

    }

    protected void RadGrid_Tasks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

    }

    private void FillTasks(string name)
    {
        try
        {
            DataSet ds = new DataSet();
            var _objCustomer = new Customer();
            _objCustomer.ConnConfig = Session["config"].ToString();
            _objCustomer.SearchBy = "t.rol";
            _objCustomer.SearchValue = name;
            _objCustomer.StartDate = string.Empty;
            _objCustomer.EndDate = string.Empty;
            _objCustomer.Screen = "Project";
            _objCustomer.Ref = Convert.ToInt32(Request.QueryString["uid"].ToString());
            _objCustomer.Mode = 5;

            ds = objBL_Customer.getTasks(_objCustomer);

            RadGrid_Tasks.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Tasks.DataSource = ds.Tables[0];
            RadGrid_Tasks.DataBind();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyFilTaskErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    private void UpdateTodoTasksNumberMasterpage()
    {
        var lblTodoNumber = Page.Master.FindControl("lblTodoNumber") as Label;
        var hdnItemJSON = Page.Master.FindControl("hdnItemJSON") as HiddenField;
        var todoNotify = Page.Master.FindControl("todoNotify") as Panel;

        string jsonString = string.Empty;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        var _objCustomer = new Customer();
        _objCustomer.ConnConfig = Session["config"].ToString();
        _objCustomer.Username = Session["username"].ToString();
        _objCustomer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _objCustomer.DueDate = DateTime.Now;

        ds = objBL_Customer.GetTodoTasksOfUserForTheDate(_objCustomer);

        GeneralFunctions objGeneral = new GeneralFunctions();
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        jsonString = sr.Serialize(dictListEval);

        if (hdnItemJSON != null)
        {
            hdnItemJSON.Value = jsonString;
        }

        if (lblTodoNumber != null && todoNotify != null)
        {
            if (dictListEval.Count > 0)
            {
                lblTodoNumber.Visible = true;
                todoNotify.Visible = true;
            }
            else
            {
                lblTodoNumber.Visible = false;
                todoNotify.Visible = false;
            }

            lblTodoNumber.Text = dictListEval.Count.ToString();
        }
    }

    // DataRow item of dtCustomValueChanged
    //Tuple<int, string, string> emailSendError = null;
    //Tuple<int, string, string> emailGetSentError = null;
    //List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
    //List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
    private void SendingAlertEmailOnWorkflowChanged(DataRow item, string mailTo, Tuple<int, string, string> emailSendError, StringBuilder sbdSentError
        , List<MimeKit.MimeMessage> mimeErrorMessages, EmailLog emailLog, List<MimeKit.MimeMessage> mimeSentMessages, ref int totalSendErr)
    {
        Mail mail = new Mail();
        mail.From = WebBaseUtility.GetFromEmailAddress();
        mail.Title = string.Format("{2} - project#: {0} - {1}", objProp_Customer.ProjectJobID, objProp_Customer.Name, item["Label"]);
        StringBuilder sbdContent = new StringBuilder();
        sbdContent.Append("This project has changes to alert you.<br/><br/>");
        sbdContent.AppendFormat("Project# {0} - {1}<br/>", objProp_Customer.ProjectJobID, objProp_Customer.Name);
        sbdContent.AppendFormat("Project location: {0}<br/><br/>", txtLocation.Text);
        int format = Convert.ToInt32(item["Format"]);
        string strFormat = string.Empty;
        string strOldValue = item["OldValue"] != null ? item["OldValue"].ToString() : "";
        string strNewValue = item["Value"] != null ? item["Value"].ToString() : "";
        switch (format)
        {
            case 1:
                strFormat = "Currency";
                break;
            case 2:
                strFormat = "Date";
                break;
            case 3:
                strFormat = "Text";
                break;
            case 4:
                strFormat = "Dropdown";
                if (string.IsNullOrEmpty(strOldValue)) strOldValue = "Select";
                if (string.IsNullOrEmpty(strNewValue)) strNewValue = "Select";
                break;
            case 5:
                strFormat = "Checkbox";
                break;
            case 6:
                strFormat = "Notes";
                break;
            case 7:
                strFormat = "CheckboxWithComment";
                break;
        }

        var updatedDate = Convert.ToDateTime(item["UpdatedDate"]);
        sbdContent.AppendFormat("{0} - {1} value changed from {4} to {5} by {2} - {3}"
            , item["Label"], strFormat, item["Username"], updatedDate.ToString("MM/dd/yyyy HH:mm tt"), strOldValue, strNewValue);
        mail.Text = sbdContent.ToString();
        //mail.Text = string.Format("Workflow label name: {0}, Old value: {1}, New value: {2}", item["Label"], item["OldValue"], item["Value"]);
        foreach (var toaddress in mailTo.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
        {
            mail.To.Add(toaddress.Trim());
        }
        //mail.To = mailTo;
        mail.IsBodyHtml = true;

        MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
        emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog);
        if (emailSendError != null && emailSendError.Item1 == 1)
        {
            sbdSentError.Append(emailSendError.Item2);
            //break;
        }
        else
        {
            emailSendError = mail.Send(mimeMessage, true, emailLog);
            if (emailSendError != null)
            {
                if (emailSendError.Item1 == 1)
                {
                    sbdSentError.Append(emailSendError.Item2);
                    //break;
                }
                else
                {
                    sbdSentError.Append(emailSendError.Item2);
                    mimeErrorMessages.Add(mimeMessage);
                    //ticketIdsError.Add("Ticket #" + _dr["id"].ToString());
                    totalSendErr++;
                }
            }
            else
            {
                mimeSentMessages.Add(mimeMessage);
                //ticketIdsSentEmail.Add("Ticket #" + _dr["id"].ToString());
            }
        }
    }

    private void SendingAlertEmailOnStageChanged(string strNewStage, string strOldStage, string mailTo, Tuple<int, string, string> emailSendError, StringBuilder sbdSentError
        , List<MimeKit.MimeMessage> mimeErrorMessages, EmailLog emailLog, List<MimeKit.MimeMessage> mimeSentMessages, ref int totalSendErr)
    {
        Mail mail = new Mail();
        mail.From = WebBaseUtility.GetFromEmailAddress();
        mail.Title = string.Format("{2} - project#: {0} - {1}", objProp_Customer.ProjectJobID, objProp_Customer.Name, strNewStage);
        StringBuilder sbdContent = new StringBuilder();
        sbdContent.Append("This project stage has changes to alert you.<br/><br/>");
        sbdContent.AppendFormat("Project# {0} - {1}<br/>", objProp_Customer.ProjectJobID, objProp_Customer.Name);
        sbdContent.AppendFormat("Project location: {0}<br/><br/>", txtLocation.Text);

        //string strNewValue = item["Label"] != null ? item["Label"].ToString() : "";

        sbdContent.AppendFormat("Project stage changed from {0} to {1} by {2} - {3}"
            , strOldStage, strNewStage, Session["Username"].ToString(), DateTime.Now.ToString("MM/dd/yyyy HH:mm tt"));
        //if (!string.IsNullOrEmpty(item["UpdatedDate"].ToString()))
        //{
        //    var updatedDate = Convert.ToDateTime(item["UpdatedDate"]);
        //    //sbdContent.AppendFormat("{0} - {1} value changed from {4} to {5} by {2} - {3}"
        //    //    , item["Label"], strFormat, item["Username"], updatedDate.ToString("MM/dd/yyyy HH:mm tt"), strOldValue, strNewValue);

        //    sbdContent.AppendFormat("Project stage changed from {0} to {1} by {2} - {3}"
        //        , strOldStage, strNewValue, item["Username"].ToString(), updatedDate.ToString("MM/dd/yyyy HH:mm tt"));
        //}
        //else
        //{
        //    sbdContent.AppendFormat("Project stage changed from {0} to {1} by {2}"
        //        , strOldStage, strNewValue, item["Username"].ToString());
        //}
        mail.Text = sbdContent.ToString();
        //mail.Text = string.Format("Workflow label name: {0}, Old value: {1}, New value: {2}", item["Label"], item["OldValue"], item["Value"]);
        foreach (var toaddress in mailTo.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
        {
            mail.To.Add(toaddress.Trim());
        }
        //mail.To = mailTo;
        mail.IsBodyHtml = true;

        MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
        emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog);
        if (emailSendError != null && emailSendError.Item1 == 1)
        {
            sbdSentError.Append(emailSendError.Item2);
            //break;
        }
        else
        {
            emailSendError = mail.Send(mimeMessage, true, emailLog);
            if (emailSendError != null)
            {
                if (emailSendError.Item1 == 1)
                {
                    sbdSentError.Append(emailSendError.Item2);
                    //break;
                }
                else
                {
                    sbdSentError.Append(emailSendError.Item2);
                    mimeErrorMessages.Add(mimeMessage);
                    //ticketIdsError.Add("Ticket #" + _dr["id"].ToString());
                    totalSendErr++;
                }
            }
            else
            {
                mimeSentMessages.Add(mimeMessage);
                //ticketIdsSentEmail.Add("Ticket #" + _dr["id"].ToString());
            }
        }
    }


    protected void RadGrid_Estimate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            if (Request.QueryString["uid"] != null)
            {
                DataSet ds = objBL_Customer.GetEstimateByLoc(Session["config"].ToString(), Convert.ToInt32(hdnLocID.Value));
                DataTable filteredData = new DataTable();
                if (ds.Tables[0].Select("Job  <>" + Request.QueryString["uid"].ToString()).Count() > 0)
                {
                    filteredData = ds.Tables[0].Select("Job <>" + Request.QueryString["uid"].ToString()).CopyToDataTable();
                    RadGrid_Estimate.VirtualItemCount = filteredData.Rows.Count;
                    RadGrid_Estimate.DataSource = filteredData;
                }
                else
                {
                    RadGrid_Estimate.VirtualItemCount = 0;
                    RadGrid_Estimate.DataSource = String.Empty;
                }

            }
        }
        catch (Exception)
        {
            RadGrid_Estimate.VirtualItemCount = 0;
            RadGrid_Estimate.DataSource = String.Empty;
        }




    }

    protected void RadGrid_Estimate_ItemCreated(object sender, GridItemEventArgs e)
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

    protected void RadGrid_Estimate_PreRender(object sender, EventArgs e)
    {

    }

    protected void btnSaveEstimate_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean flag = false;
            int oppId = 0;
            if (!string.IsNullOrEmpty(hdnOpportunityId.Value) && !string.IsNullOrEmpty(txtOpportunity.Text))
                oppId = Convert.ToInt32(hdnOpportunityId.Value);
            
            foreach (GridDataItem item in RadGrid_Estimate.SelectedItems)
            {
                HiddenField hdnEstimateID = (HiddenField)item.FindControl("hdnEstimateID");
                objBL_Customer.LinkEstimateToProject(Session["config"].ToString(), Convert.ToInt32(hdnEstimateID.Value), Convert.ToInt32(Request.QueryString["uid"]), oppId, Session["Username"].ToString());
                flag = true;
                // Reset oppId to 0 for skip running spUpdateEstimatesOpportinityForProjectLinking in the second time
                oppId = 0;
            }

            if (flag)
            {
                hdnOpportunityId.Value = "";
                txtOpportunity.Text = "";

                RadGrid_Estimate.Rebind();
                ShowLinkEstimate();
                RadGrid_EstimateProject.Rebind();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccess", "noty({text: 'The estimate is successfully linked to the project', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseEstimateWindow();", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }


    }

    private void FillEstimate()
    {
        try
        {


            DataSet ds = new DataSet();
            objProp_Customer.ConnConfig = Session["config"].ToString();


            ds = objBL_Customer.getEstimateByProject(Session["config"].ToString(), Convert.ToInt32(Request.QueryString["uid"]));
            //Session["Estimate"] = ds.Tables[0];

            RadGrid_EstimateProject.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_EstimateProject.DataSource = ds;

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_EstimateProject_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            FillEstimate();
        }

    }

    protected void btnLinkEstimate_Click(object sender, EventArgs e)
    {

    }


    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_WageCategoryList.MasterTableView.FilterExpression != "" ||
            (RadGrid_WageCategoryList.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_WageCategoryList.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_WageCategoryList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_WageCategoryList.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["WageCategory_FilterExpressionProject"] != null && Convert.ToString(Session["WageCategory_FilterExpressionProject"]) != "" && Session["WageCategory_FiltersProject"] != null)
                {
                    RadGrid_WageCategoryList.MasterTableView.FilterExpression = Convert.ToString(Session["WageCategory_FilterExpressionProject"]);
                    var filtersGet = Session["WageCategory_FiltersProject"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_WageCategoryList.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["WageCategory_FilterExpressionProject"] = null;
                Session["WageCategory_FiltersProject"] = null;
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
        //GetWageCategoryList();
    }
    protected void RadGrid_WageDeductionList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_WageDeductionList.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["WageDeduction_FilterExpressionProject"] != null && Convert.ToString(Session["WageDeduction_FilterExpressionProject"]) != "" && Session["WageCategory_FiltersProject"] != null)
                {
                    RadGrid_WageDeductionList.MasterTableView.FilterExpression = Convert.ToString(Session["WageDeduction_FilterExpressionProject"]);
                    var filtersGet = Session["WageDeductionProject"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_WageDeductionList.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["WageDeduction_FilterExpressionProject"] = null;
                Session["WageDeductionProject"] = null;
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

    private void GetWageCategoryList()
    {
        try
        {

            DataSet ds = new DataSet();
            _objWage.ConnConfig = Session["config"].ToString();

            ds = new BL_Wage().getWage(_objWage);

            RadGrid_WageCategoryList.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_WageCategoryList.DataSource = ds.Tables[0];
            ViewState["VirtualItemCountWageCategoryList"] = ds.Tables[0].Rows.Count;
            RadGrid_WageCategoryList.DataBind();
            //lblRecordCount.Text = ds.Tables[0].Rows.Count + " Record(s) found";
            //Session["WageDeductionList"] = ds.Tables[0];
            //SaveFilter();


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void GetWageDeductionList()
    {
        try
        {
            DataSet ds = new DataSet();
            _objPRDed.ConnConfig = Session["config"].ToString();
            ds = new BL_Wage().GetWageDeduction(_objPRDed);
            ds.Tables[0].DefaultView.RowFilter = "Job = 1";
            DataTable dtded = (ds.Tables[0].DefaultView).ToTable();

            RadGrid_WageDeductionList.VirtualItemCount = dtded.Rows.Count;
            RadGrid_WageDeductionList.DataSource = dtded;
            ViewState["VirtualItemCountDeductionList"] = dtded;

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void RadGrid_WageCategoryList_ItemEvent(object sender, GridItemEventArgs e)
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
        rowCount = Convert.ToInt32(ViewState["VirtualItemCountWageCategoryList"]);
        //lblRecordCount.Text = rowCount + " Record(s) found";
        //updpnl.Update();
    }
    protected void RadGrid_WageDeductionList_ItemEvent(object sender, GridItemEventArgs e)
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
        rowCount = Convert.ToInt32(ViewState["VirtualItemCountDeductionList"]);
        //lblRecordCount.Text = rowCount + " Record(s) found";
        //updpnl.Update();
    }
    private void RowSelectWageCategoryList()
    {
        foreach (GridDataItem gr in RadGrid_WageCategoryList.Items)
        {
            //Label lblID = (Label)gr.FindControl("lblId");
            //HyperLink lnkName = (HyperLink)gr.FindControl("lblWageFdesc");
            //lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='WageCategory.aspx?id=" + lblID.Text + "'";
        }
    }
    private void RowSelectWageDeductionList()
    {
        foreach (GridDataItem gr in RadGrid_WageDeductionList.Items)
        {
            //Label lblID = (Label)gr.FindControl("lblId");
            //HyperLink lnkName = (HyperLink)gr.FindControl("lblWageFdesc");
            //lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='WageCategory.aspx?id=" + lblID.Text + "'";
        }
    }
    protected void RadGrid_WageCategoryList_ItemCreated(object sender, GridItemEventArgs e)
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
    protected void RadGrid_WageCategoryList_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_WageCategoryList.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["WageCategory_FilterExpressionProject"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_WageCategoryList.MasterTableView.OwnerGrid.Columns)
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

            Session["WageCategory_FiltersProject"] = filters;
            //Session["Vendor_VirtulItemCount"] = RadGrid_WageCategoryList.VirtualItemCount;
        }
        else
        {
            Session["WageCategory_FilterExpressionProject"] = null;
            Session["WageCategory_FiltersProject"] = null;
            //Session["Vendor_VirtulItemCount"] = null;
        }
        #endregion  
        RowSelectWageCategoryList();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "bindingClickCheckbox", "BindClickEventForGridCheckBox();", true);

    }
    protected void RadGrid_WageDeductionList_ItemCreated(object sender, GridItemEventArgs e)
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
    protected void RadGrid_WageDeductionList_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_WageDeductionList.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["WageDeduction_FilterExpressionProject"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_WageDeductionList.MasterTableView.OwnerGrid.Columns)
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

            Session["WageDeduction_Filters"] = filters;
            //Session["Vendor_VirtulItemCount"] = RadGrid_WageCategoryList.VirtualItemCount;
        }
        else
        {
            Session["WageDeduction_FilterExpressionProject"] = null;
            Session["WageDeduction_Filters"] = null;
            //Session["Vendor_VirtulItemCount"] = null;
        }
        #endregion  
        RowSelectWageDeductionList();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "bindingClickCheckboxDeduction", "BindClickEventForGridCheckBoxDeduction();", true);

    }

    private void FillWageCategory()
    {
        try
        {
            if (ViewState["dtRadGrid_WageCategory"] == null)
            {
                DataTable dt = GetWageTable();
                RadGrid_WageCategory.DataSource = dt;
            }
            else
            {
                DataTable dt = (DataTable)ViewState["dtRadGrid_WageCategory"];
                RadGrid_WageCategory.DataSource = dt;
            }
            //ddlDefaultWage.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        catch (Exception ex)
        {
            throw ex;
            //FillOtherIncomeWage
        }
    }
    public DataTable GetWageTable()
    {
        //DataSet ds = new DataSet();
        //_objWage.ConnConfig = Session["config"].ToString();
        //_objWage.ID = Convert.ToInt32(0);
        //ds = objBL_Wage.GetWageByID(_objWage);

        //return ds.Tables[0];


        DataTable dt = new DataTable();
        dt.Columns.Add("id", typeof(int));
        dt.Columns.Add("GL", typeof(int));
        dt.Columns.Add("GLName", typeof(string));
        dt.Columns.Add("Reg", typeof(double));
        dt.Columns.Add("OT", typeof(double));
        dt.Columns.Add("DT", typeof(double));
        dt.Columns.Add("TT", typeof(double));
        dt.Columns.Add("FIT", typeof(int));
        dt.Columns.Add("FICA", typeof(int));
        dt.Columns.Add("MEDI", typeof(int));
        dt.Columns.Add("FUTA", typeof(int));
        dt.Columns.Add("SIT", typeof(int));
        dt.Columns.Add("Vac", typeof(int));
        dt.Columns.Add("Wc", typeof(int));
        dt.Columns.Add("Uni", typeof(int));
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("Sick", typeof(int));
        dt.Columns.Add("Status", typeof(int));

        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDH", typeof(double));
        dt.Columns.Add("OYTD", typeof(double));
        dt.Columns.Add("OYTDH", typeof(double));
        dt.Columns.Add("DYTD", typeof(double));
        dt.Columns.Add("DYTDH", typeof(double));
        dt.Columns.Add("TYTD", typeof(double));
        dt.Columns.Add("TYTDH", typeof(double));
        dt.Columns.Add("NT", typeof(double));
        dt.Columns.Add("NYTD", typeof(double));
        dt.Columns.Add("NYTDH", typeof(double));
        dt.Columns.Add("VacR", typeof(string));
        dt.Columns.Add("CReg", typeof(double));
        dt.Columns.Add("COT", typeof(double));
        dt.Columns.Add("CDT", typeof(double));
        dt.Columns.Add("CNT", typeof(double));
        dt.Columns.Add("CTT", typeof(double));
        dt.Columns.Add("fdesc", typeof(string));

        return dt;
    }
    protected void lnkWageCategorySave_Click(object sender, EventArgs e)
    {
        //try
        // {
        DataTable dt = GetWageTable();


        foreach (GridDataItem gr in RadGrid_WageCategoryList.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                DataRow dr = dt.NewRow();

                DataSet ds = new DataSet();
                _objWage.ConnConfig = Session["config"].ToString();
                _objWage.ID = Convert.ToInt32(lblID.Text);
                ds = objBL_Wage.GetWageByID(_objWage);
                //dr = ds.Tables[0].Rows[0];
                //dt.Rows.Add(dr.ItemArray);


                dr["id"] = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                dr["GL"] = Convert.ToInt32(ds.Tables[0].Rows[0]["GL"]);
                dr["GLName"] = Convert.ToString(ds.Tables[0].Rows[0]["GLName"]);
                dr["Reg"] = Convert.ToDouble(ds.Tables[0].Rows[0]["Reg"]);
                dr["OT"] = Convert.ToString(ds.Tables[0].Rows[0]["OT1"]);
                dr["DT"] = Convert.ToInt32(ds.Tables[0].Rows[0]["OT2"]);
                dr["TT"] = Convert.ToInt32(ds.Tables[0].Rows[0]["TT"]);
                dr["FIT"] = Convert.ToInt32(ds.Tables[0].Rows[0]["FIT"]);
                dr["FICA"] = Convert.ToInt32(ds.Tables[0].Rows[0]["FICA"]);
                dr["MEDI"] = Convert.ToInt32(ds.Tables[0].Rows[0]["MEDI"]);
                dr["FUTA"] = Convert.ToInt32(ds.Tables[0].Rows[0]["FUTA"]);
                dr["SIT"] = Convert.ToInt32(ds.Tables[0].Rows[0]["SIT"]);
                dr["Vac"] = Convert.ToInt32(ds.Tables[0].Rows[0]["Vac"]);
                dr["Wc"] = Convert.ToInt32(ds.Tables[0].Rows[0]["Wc"]);
                dr["Uni"] = Convert.ToInt32(ds.Tables[0].Rows[0]["Uni"]);
                dr["InUse"] = Convert.ToInt32(0);
                dr["Sick"] = Convert.ToInt32(ds.Tables[0].Rows[0]["Sick"]);
                dr["Status"] = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"]);

                dr["YTD"] = 0.00;
                dr["YTDH"] = 0.00;
                dr["OYTD"] = 0.00;
                dr["OYTDH"] = 0.00;
                dr["DYTD"] = 0.00;
                dr["DYTDH"] = 0.00;
                dr["TYTD"] = 0.00;
                dr["TYTDH"] = 0.00;
                dr["NT"] = Convert.ToDouble(ds.Tables[0].Rows[0]["NT"]);
                dr["NYTD"] = 0.00;
                dr["NYTDH"] = 0.00;
                dr["VacR"] = "";
                dr["CReg"] = Convert.ToDouble(ds.Tables[0].Rows[0]["CReg"]);
                dr["COT"] = Convert.ToDouble(ds.Tables[0].Rows[0]["COT"]);
                dr["CDT"] = Convert.ToDouble(ds.Tables[0].Rows[0]["CDT"]);
                dr["CNT"] = Convert.ToDouble(ds.Tables[0].Rows[0]["CNT"]);
                dr["CTT"] = Convert.ToDouble(ds.Tables[0].Rows[0]["CTT"]);
                dr["fdesc"] = Convert.ToString(ds.Tables[0].Rows[0]["fDesc"]);

                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }
            //HyperLink lnkName = (HyperLink)gr.FindControl("lblWageFdesc");
            //lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='WageCategory.aspx?id=" + lblID.Text + "'";
        }
        ViewState["dtRadGrid_WageCategory"] = dt;
        RadGrid_WageCategory.DataSource = dt;
        RadGrid_WageCategory.DataBind();
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    string strerror = "warning";

        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddServicetype", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
    }

    private void FillWageDeduction()
    {
        try
        {

            DataTable dt = GetWageDeductionTable();
            RadGrid_WageDeduction.DataSource = dt;

        }
        catch (Exception ex)
        {
            throw ex;
            //FillOtherIncomeWage
        }
    }
    public DataTable GetWageDeductionTable()
    {
        //DataSet ds = new DataSet();
        //_objPRDed.ConnConfig = Session["config"].ToString();
        //_objPRDed.ID = Convert.ToInt32(0);
        //ds = new BL_Wage().GetWageDeductionByID(_objPRDed);
        //return ds.Tables[0];


        DataTable dt = new DataTable();
        dt.Columns.Add("id", typeof(int));
        dt.Columns.Add("BasedOn", typeof(int));
        dt.Columns.Add("AccruedOn", typeof(int));
        dt.Columns.Add("ByW", typeof(int));
        dt.Columns.Add("EmpRate", typeof(double));
        dt.Columns.Add("EmpTop", typeof(double));
        dt.Columns.Add("EmpGL", typeof(int));
        dt.Columns.Add("EmpGLName", typeof(string));
        dt.Columns.Add("CompRate", typeof(double));
        dt.Columns.Add("CompTop", typeof(double));
        dt.Columns.Add("CompGL", typeof(int));
        dt.Columns.Add("CompGLName", typeof(string));
        dt.Columns.Add("CompGLE", typeof(int));
        dt.Columns.Add("CompGLEName", typeof(string));
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDC", typeof(double));
        dt.Columns.Add("fdesc", typeof(string));

        dt.Columns.Add("BasedOnDesc", typeof(string));
        dt.Columns.Add("AccruedOnDesc", typeof(string));
        dt.Columns.Add("ByWDesc", typeof(string));

        return dt;



    }
    protected void lnkWageDeductionListSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = GetWageDeductionTable();


            foreach (GridDataItem gr in RadGrid_WageDeductionList.Items)
            {
                Label lblID = (Label)gr.FindControl("lblId");
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectD");
                if (chkSelect.Checked == true)
                {
                    DataRow dr = dt.NewRow();

                    DataSet ds = new DataSet();
                    _objPRDed.ConnConfig = Session["config"].ToString();
                    _objPRDed.ID = Convert.ToInt32(lblID.Text);
                    ds = new BL_Wage().GetWageDeductionByID(_objPRDed);
                    //dr = ds.Tables[0].Rows[0];

                    //dt.Rows.Add(dr.ItemArray);


                    dr["id"] = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
                    dr["BasedOn"] = Convert.ToInt32(ds.Tables[0].Rows[0]["BasedOn"]);
                    dr["AccruedOn"] = Convert.ToInt32(ds.Tables[0].Rows[0]["AccruedOn"]);
                    dr["ByW"] = Convert.ToInt32(ds.Tables[0].Rows[0]["ByW"]);
                    dr["EmpRate"] = Convert.ToDouble(ds.Tables[0].Rows[0]["EmpRate"]);
                    dr["EmpTop"] = Convert.ToDouble(ds.Tables[0].Rows[0]["EmpTop"]);
                    dr["EmpGL"] = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpGL"]);
                    dr["EmpGLName"] = Convert.ToString(ds.Tables[0].Rows[0]["EmpGLAcct"]);
                    dr["CompRate"] = Convert.ToDouble(ds.Tables[0].Rows[0]["CompRate"]);
                    dr["CompTop"] = Convert.ToDouble(ds.Tables[0].Rows[0]["CompTop"]);
                    dr["CompGL"] = Convert.ToInt32(ds.Tables[0].Rows[0]["CompGL"]);
                    dr["CompGLName"] = Convert.ToString(ds.Tables[0].Rows[0]["CompGLAcct"]);
                    dr["CompGLE"] = Convert.ToInt32(ds.Tables[0].Rows[0]["CompGLE"]);
                    dr["CompGLEName"] = Convert.ToString(ds.Tables[0].Rows[0]["CompGLEAcct"]);
                    dr["InUse"] = 0;
                    dr["YTD"] = 0.00;
                    dr["YTDC"] = 0.00;
                    dr["fDesc"] = Convert.ToString(ds.Tables[0].Rows[0]["fDesc"]);
                    dr["BasedOnDesc"] = Convert.ToInt32(ds.Tables[0].Rows[0]["BasedOnDesc"]);
                    dr["AccruedOnDesc"] = Convert.ToInt32(ds.Tables[0].Rows[0]["AccruedOn"]);
                    dr["ByWDesc"] = Convert.ToInt32(ds.Tables[0].Rows[0]["ByWDesc"]);
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }

            }
            RadGrid_WageDeduction.DataSource = dt;
            ViewState["RadGrid_WageDeductionDataSource"] = dt;
            RadGrid_WageDeduction.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string strerror = "warning";

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddServicetype", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnAddWages_Click(object sender, EventArgs e)
    {
        FillWageCategory();
        RadGrid_WageCategory.DataBind();

    }
    protected void btndeductionAdd_Click(object sender, EventArgs e)
    {
        FillWageDeduction();
        RadGrid_WageDeduction.DataBind();

    }
    protected void RadGrid_WageCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        FillWageCategory();
    }
    protected void RadGrid_WageCategory_PreRender(object sender, EventArgs e)
    {
        //RowSelectWageCategory();
    }
    protected void RadGrid_WageCategory_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            CheckBox checkColumn = item.FindControl("checkColumnWageCategory") as CheckBox;
            checkColumn.Attributes.Add("onclick", "uncheckWageCategory(this);");
        }
    }
    protected void RadGrid_WageDeduction_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        FillWageDeduction();
    }
    protected void RadGrid_WageDeduction_PreRender(object sender, EventArgs e)
    {
        //RowSelectWageDeduction();
    }
    protected void RadGrid_WageDeduction_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            CheckBox checkColumn = item.FindControl("checkColumnWageDeduction") as CheckBox;
            checkColumn.Attributes.Add("onclick", "uncheckWageDeduction(this);");
        }
    }
    private void GetControlForPayroll()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());

        DataSet ds = new DataSet();
        ds = objBL_Report.GetControlForPayroll(objPropUser);
        bool PR = Convert.ToBoolean(DBNull.Value.Equals(ds.Tables[0].Rows[0]["PR"]) ? 0 : ds.Tables[0].Rows[0]["PR"]);

        bool PRUser = objBL_User.getPRUserByID(objPropUser);

        if (PR == true && PRUser == true)
        {
            liWageCategories.Visible = true;
            liDeductionCat.Visible = true;
        }
        else
        {
            if (PR == true && Session["User"].ToString() == "Maintenance")
            {
                liWageCategories.Visible = true;
                liDeductionCat.Visible = true;
            }
            else
            {
                lifinancewagecategoryhead.Style.Add("display", "none");
                lifinancewagedeductionhead.Style.Add("display", "none");
            }

        }
    }
    protected void lnkbtnRemovededcutionCategory_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtWageDeduction = (DataTable)ViewState["RadGrid_WageDeductionDataSource"];
            foreach (GridDataItem gr in RadGrid_WageDeduction.Items)
            {
                HiddenField hdndeductioncateid = (HiddenField)gr.FindControl("hdndeductioncateid");
                CheckBox checkColumnWageDeduction = (CheckBox)gr.FindControl("checkColumnWageDeduction");
                if (checkColumnWageDeduction.Checked == true)
                {
                    dtWageDeduction.Rows.RemoveAt(gr.DataSetIndex);
                    dtWageDeduction.AcceptChanges();
                }
            }
            RadGrid_WageDeduction.DataSource = dtWageDeduction;
            RadGrid_WageDeduction.DataBind();
            ViewState["RadGrid_WageDeductionDataSource"] = dtWageDeduction;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string strerror = "warning";

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddServicetype", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkbtnRemoveWageCategory_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtWageCategory = (DataTable)ViewState["dtRadGrid_WageCategory"];
            foreach (GridDataItem gr in RadGrid_WageCategory.Items)
            {
                HiddenField hdnwagecateid = (HiddenField)gr.FindControl("hdnwagecateid");
                CheckBox checkColumnWageCategory = (CheckBox)gr.FindControl("checkColumnWageCategory");
                if (checkColumnWageCategory.Checked == true)
                {
                    dtWageCategory.Rows.RemoveAt(gr.DataSetIndex);
                    dtWageCategory.AcceptChanges();
                }
            }
            RadGrid_WageCategory.DataSource = dtWageCategory;
            RadGrid_WageCategory.DataBind();
            ViewState["dtRadGrid_WageCategory"] = dtWageCategory;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string strerror = "warning";

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddServicetype", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    public DataTable GetWageTableItem()
    {

        DataTable dt = new DataTable();
        dt.Columns.Add("WageC", typeof(int));
        dt.Columns.Add("Reg", typeof(double));
        dt.Columns.Add("OT", typeof(double));
        dt.Columns.Add("DT", typeof(double));
        dt.Columns.Add("TT", typeof(double));
        dt.Columns.Add("NT", typeof(double));
        dt.Columns.Add("GL", typeof(int));
        dt.Columns.Add("Fringe1", typeof(double));
        dt.Columns.Add("Fringe2", typeof(double));
        dt.Columns.Add("Fringe3", typeof(double));
        dt.Columns.Add("Fringe4", typeof(double));
        dt.Columns.Add("PF1", typeof(int));
        dt.Columns.Add("PF2", typeof(int));
        dt.Columns.Add("PF3", typeof(int));
        dt.Columns.Add("PF4", typeof(int));
        dt.Columns.Add("FringeGL", typeof(int));
        dt.Columns.Add("CReg", typeof(double));
        dt.Columns.Add("COT", typeof(double));
        dt.Columns.Add("CDT", typeof(double));
        dt.Columns.Add("CTT", typeof(double));
        dt.Columns.Add("CNT", typeof(double));


        foreach (GridDataItem gr in RadGrid_WageCategory.Items)
        {
            DataRow dr = dt.NewRow();

            HiddenField hdnwagecateid = (HiddenField)gr.FindControl("hdnwagecateid");
            HiddenField hdnwagecategl = (HiddenField)gr.FindControl("hdnwagecategl");
            TextBox txtregratepay = (TextBox)gr.FindControl("txtregratepay");
            TextBox txtotratepay = (TextBox)gr.FindControl("txtotratepay");
            TextBox txtntratepay = (TextBox)gr.FindControl("txtntratepay");
            TextBox txtdtratepay = (TextBox)gr.FindControl("txtdtratepay");
            TextBox txtttratepay = (TextBox)gr.FindControl("txtttratepay");
            TextBox txtregrateburden = (TextBox)gr.FindControl("txtregrateburden");
            TextBox txtotrateburden = (TextBox)gr.FindControl("txtotrateburden");
            TextBox txtntrateburden = (TextBox)gr.FindControl("txtntrateburden");
            TextBox txtdtrateburden = (TextBox)gr.FindControl("txtdtrateburden");
            TextBox txtttrateburden = (TextBox)gr.FindControl("txtttrateburden");

            dr["WageC"] = Convert.ToInt32(hdnwagecateid.Value);
            dr["GL"] = Convert.ToInt32(hdnwagecategl.Value);
            if (txtregratepay.Text.Trim() == "")
            {
                dr["Reg"] = 0.00;
            }
            else
            {
                dr["Reg"] = Convert.ToDouble(txtregratepay.Text);
            }
            if (txtotratepay.Text.Trim() == "")
            {
                dr["OT"] = 0.00;
            }
            else
            {
                dr["OT"] = Convert.ToDouble(txtotratepay.Text);
            }
            if (txtttratepay.Text.Trim() == "")
            {
                dr["TT"] = 0.00;
            }
            else
            {
                dr["TT"] = Convert.ToDouble(txtttratepay.Text);
            }
            if (txtntratepay.Text.Trim() == "")
            {
                dr["NT"] = 0.00;
            }
            else
            {
                dr["NT"] = Convert.ToDouble(txtntratepay.Text);
            }
            if (txtdtratepay.Text.Trim() == "")
            {
                dr["DT"] = 0.00;
            }
            else
            {
                dr["DT"] = Convert.ToDouble(txtdtratepay.Text);
            }

            dr["Fringe1"] = 0.00;
            dr["Fringe2"] = 0.00;
            dr["Fringe3"] = 0.00;
            dr["Fringe4"] = 0.00;
            dr["PF1"] = 0;
            dr["PF2"] = 0;
            dr["PF3"] = 0;
            dr["PF4"] = 0;
            if (txtregrateburden.Text.Trim() == "")
            {
                dr["CReg"] = 0.00;
            }
            else
            {
                dr["CReg"] = Convert.ToDouble(txtregrateburden.Text);
            }
            if (txtotrateburden.Text.Trim() == "")
            {
                dr["COT"] = 0.00;
            }
            else
            {
                dr["COT"] = Convert.ToDouble(txtotrateburden.Text);
            }
            if (txtdtrateburden.Text.Trim() == "")
            {
                dr["CDT"] = 0.00;
            }
            else
            {
                dr["CDT"] = Convert.ToDouble(txtdtrateburden.Text);
            }
            if (txtntrateburden.Text.Trim() == "")
            {
                dr["CNT"] = 0.00;
            }
            else
            {
                dr["CNT"] = Convert.ToDouble(txtntrateburden.Text);
            }
            if (txtttrateburden.Text.Trim() == "")
            {
                dr["CTT"] = 0.00;
            }
            else
            {
                dr["CTT"] = Convert.ToDouble(txtttrateburden.Text);
            }

            dt.Rows.Add(dr);
            dt.AcceptChanges();
        }

        return dt;
    }
    public DataTable GetWageDeductionTableItem()
    {

        DataTable dt = new DataTable();
        dt.Columns.Add("Ded", typeof(int));

        foreach (GridDataItem gr in RadGrid_WageDeduction.Items)
        {
            DataRow dr = dt.NewRow();

            HiddenField hdndeductioncateid = (HiddenField)gr.FindControl("hdndeductioncateid");

            dr["Ded"] = Convert.ToInt32(hdndeductioncateid.Value);

            dt.Rows.Add(dr);
            dt.AcceptChanges();
        }

        return dt;


    }
    private void BindWageCategoryGrid()
    {
        DataSet ds = new DataSet();
        _objWage.ConnConfig = Session["config"].ToString();
        _objWage.ID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        ds = new BL_Wage().getWageCategorybyProjectID(_objWage);

        RadGrid_WageCategory.DataSource = ds.Tables[0];
        RadGrid_WageCategory.DataBind();
        ViewState["dtRadGrid_WageCategory"] = ds.Tables[0];

        if (ds.Tables[0].Rows.Count > 0)
        {
            rdYesMultiWage.Checked = true;
            rdNoMultiWage.Checked = false;
        }
        else
        {
            rdYesMultiWage.Checked = false;
            rdNoMultiWage.Checked = true;
        }
    }
    private void BindWageDeductionGrid()
    {
        DataSet ds = new DataSet();
        _objPRDed.ConnConfig = Session["config"].ToString();
        _objPRDed.ID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        ds = new BL_Wage().getDeductionCategorybyProjectID(_objPRDed);

        RadGrid_WageDeduction.DataSource = ds.Tables[0];
        RadGrid_WageDeduction.DataBind();
        ViewState["RadGrid_WageDeductionDataSource"] = ds.Tables[0];
        if (ds.Tables[0].Rows.Count > 0)
        {
            rdYesMultiWageD.Checked = true;
            rdNoMultiWageD.Checked = false;
        }
        else
        {
            rdYesMultiWageD.Checked = false;
            rdNoMultiWageD.Checked = true;
        }
    }
    protected void gvWIPs_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BL_User objBL_User = new BL_User();
            User objProp_User = new User();

            DataSet ds = new DataSet();
            objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            objProp_User.PageName = "AddProject.aspx";
            objProp_User.GridId = "gvWIPs";
            ds = objBL_User.GetGridUserSettings(objProp_User);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //string columnSettings = "[{Name: \"BType\", Display: true, Width: 300},{Name: \"MatItem\", Display: false, Width: 300}]";
                var columnSettings = ds.Tables[0].Rows[0][0].ToString();
                var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

                var colIndex = 0;

                foreach (GridColumn column in gvWIPs.MasterTableView.OwnerGrid.Columns)
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
        //todo
        foreach (GridDataItem gr in gvWIPs.Items)
        {

            CheckBox chkEnableGSTTax = (CheckBox)gr.FindControl("chkEnableGSTTax");
            CheckBox chkTaxable = (CheckBox)gr.FindControl("chkTaxable");
            chkEnableGSTTax.Attributes["onclick"] = "CalculateGridAmount();";
            chkTaxable.Attributes["onclick"] = "CalculateGridAmount();";

        }
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
        isCanada = isCanadaCompany();
        if (isCanada)
        {

            //for grid
            gvWIPs.Columns.FindByUniqueName("InvoiceTotal").Visible = true;
            gvWIPs.Columns.FindByUniqueName("GSTable").Visible = true;
            gvWIPs.Columns.FindByUniqueName("GSTAmount").Visible = true;
            gvWIPs.Columns.FindByUniqueName("PSTAmount").Visible = true;
            gvWIPs.Columns.FindByUniqueName("PSTAmount").HeaderText = "HST/PST Tax";
            gvWIPs.Columns.FindByUniqueName("Taxable").HeaderText = "HST/PST";

            //gvInvoice
            gvInvoice.Columns.FindByUniqueName("PreTaxAmount").HeaderText = "Pretax Amount";
            gvInvoice.Columns.FindByUniqueName("PSTTaxAmount").Visible = true;
            gvInvoice.Columns.FindByUniqueName("GSTTaxAmount").Visible = true;

            //gvProgressBilling
            gvProgressBilling.Columns.FindByUniqueName("GSTAmount").Visible = true;
            gvProgressBilling.Columns.FindByUniqueName("PSTAmount").Visible = true;


            if (ViewState["TaxType"].ToString() == "2")
            {
                gvWIPs.Columns.FindByUniqueName("GSTable").Visible = false;
                gvWIPs.Columns.FindByUniqueName("GSTAmount").Visible = false;
                gvWIPs.Columns.FindByUniqueName("PSTAmount").HeaderText = "HST Tax";
                gvWIPs.Columns.FindByUniqueName("Taxable").HeaderText = "HST";

                gvProgressBilling.Columns.FindByUniqueName("GSTAmount").Visible = false;
                gvProgressBilling.Columns.FindByUniqueName("PSTAmount").Visible = true;
                gvProgressBilling.Columns.FindByUniqueName("PSTAmount").HeaderText = "HST Tax";

                gvInvoice.Columns.FindByUniqueName("PSTTaxAmount").HeaderText = "HST Tax";
                gvInvoice.Columns.FindByUniqueName("GSTTaxAmount").Visible = false;
            }
            else
            {
                gvWIPs.Columns.FindByUniqueName("GSTable").Visible = true;
                gvWIPs.Columns.FindByUniqueName("GSTAmount").Visible = true;
                gvWIPs.Columns.FindByUniqueName("PSTAmount").HeaderText = "PST Tax";
                gvWIPs.Columns.FindByUniqueName("Taxable").HeaderText = "PST";

                gvProgressBilling.Columns.FindByUniqueName("GSTAmount").Visible = true;
                gvProgressBilling.Columns.FindByUniqueName("PSTAmount").HeaderText = "PST Tax";

                gvInvoice.Columns.FindByUniqueName("PSTTaxAmount").HeaderText = "PST Tax";
                gvInvoice.Columns.FindByUniqueName("GSTTaxAmount").Visible = true;

                //  }
            }



        }
        else
        {
            //for grid
            gvWIPs.Columns.FindByUniqueName("InvoiceTotal").Visible = false;
            gvWIPs.Columns.FindByUniqueName("GSTable").Visible = false;
            gvWIPs.Columns.FindByUniqueName("GSTAmount").Visible = false;
            gvWIPs.Columns.FindByUniqueName("PSTAmount").Visible = false;
            gvWIPs.Columns.FindByUniqueName("Taxable").HeaderText = "Tax";

            //gvProgressBilling
            gvProgressBilling.Columns.FindByUniqueName("GSTAmount").Visible = false;
            gvProgressBilling.Columns.FindByUniqueName("PSTAmount").HeaderText = "Sales Tax";

            //gvInvoice
            gvInvoice.Columns.FindByUniqueName("PreTaxAmount").HeaderText = "Amt";
            gvInvoice.Columns.FindByUniqueName("PSTTaxAmount").HeaderText = "Sales Tax";
            gvInvoice.Columns.FindByUniqueName("GSTTaxAmount").Visible = false;
        }

        gvWIPs.Rebind();
        gvProgressBilling.Rebind();
        gvInvoice.Rebind();
    }

    protected void lnkjobticketSearch_Click(object sender, EventArgs e)
    {
        BindTicketList(string.Empty, 1, true);
    }

    protected void gvInvoice_PreRender(object sender, EventArgs e)
    {
        if (ViewState["ARInvoices"] != null)
        {
            calculateInvoice((DataTable)ViewState["ARInvoices"]);
        }

    }

    private void FillSupervisor(int typeId)
    {
        try
        {
            DataSet _dsSupervisor = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            objJob.TypeId = typeId;
            _dsSupervisor = objBL_Job.GetSupervisorByTypeId(objJob);
            ddlSupervisor.Items.Clear();
            if (_dsSupervisor.Tables[0].Rows.Count > 0)
            {

                ddlSupervisor.Items.Add(new ListItem("Select Supervisor", "Select Supervisor"));
                ddlSupervisor.AppendDataBoundItems = true;
                ddlSupervisor.DataSource = _dsSupervisor;
                ddlSupervisor.DataValueField = "ID";
                ddlSupervisor.DataTextField = "fUser";
                ddlSupervisor.DataBind();
            }
            else
            {
                ddlSupervisor.Items.Add(new ListItem("No data found", "Select Supervisor"));
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkSavePlanner_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(lnkPlannerID.Text))
            {
                var plannerID = Convert.ToInt32(lnkPlannerID.Text);
                objPlanner.ConnConfig = Session["config"].ToString();
                objPlanner.PlannerID = plannerID;
                objPlanner.Desc = txtPlannerTitle.Text;
                objBL_Planner.UpdatePlannerTitle(objPlanner);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDelPlnSucc", "noty({text: 'Planner updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyUpdatePlnErr", "noty({text: 'Planner number cannot be empty',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkDeletePlanner_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(lnkPlannerID.Text))
            {
                var plannerID = Convert.ToInt32(lnkPlannerID.Text);
                objPlanner.ConnConfig = Session["config"].ToString();
                objPlanner.PlannerID = plannerID;
                objBL_Planner.DeletePlanner(objPlanner);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDelPlnSucc", "noty({text: 'Planner deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                lnkSavePlanner.Visible = false;
                lnkDeletePlanner.Visible = false;
                lnkAddPlanner.Visible = true;
                //lnkPlannerID.Visible = false;
                PlannerInfo.Visible = false;
                //ViewState["PlannerID"] = null;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDelPlnErr", "noty({text: 'Planner number cannot be empty',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keykeyDelPlnErrErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillStages(string depId)
    {
        DataSet ds = new DataSet();
        ds = objBL_Customer.GetStagesByDepartment(Session["config"].ToString(), depId);

        ddlStage.DataSource = ds.Tables[0];
        ddlStage.DataTextField = "Stage";
        ddlStage.DataValueField = "ID";
        ddlStage.DataBind();
        ddlStage.Items.Insert(0, new ListItem("Select", "0"));
        lblStage.InnerText = "Stage";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblStage.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    private DataTable GetPCEInstallationInvoiceItems(DataTable dtInvoice, DataTable dtInvItems)
    {
        DataTable dtInstallationItems = dtInvItems.Clone();

        var positiveAmount = dtInvItems.AsEnumerable()
            .Where(y => y.Field<decimal>("Amount") > 0)
            .Sum(x => x.Field<decimal>("Amount"));

        if (positiveAmount != 0)
        {
            var dr = dtInstallationItems.NewRow();
            dr["fDesc"] = dtInvoice.Rows[0]["fDesc"];
            dr["Amount"] = positiveAmount;

            dtInstallationItems.Rows.Add(dr);
        }

        var negativeAmount = dtInvItems.AsEnumerable()
            .Where(y => y.Field<decimal>("Amount") < 0)
            .Sum(x => x.Field<decimal>("Amount"));

        if (negativeAmount != 0)
        {
            var dr = dtInstallationItems.NewRow();
            dr["fDesc"] = "Deposit";
            dr["Amount"] = negativeAmount;

            dtInstallationItems.Rows.Add(dr);
        }

        return dtInstallationItems;
    }
    protected void lnkSaveGridSettings_Click(object sender, EventArgs e)
    {
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        #region Grid user settings
        var columnSettings = GetGridColumnSettings();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "AddProject.aspx";
        objProp_User.GridId = "gvWIPs";

        objBL_User.UpdateUserGridCustomSettings(objProp_User, columnSettings);

        ScriptManager.RegisterStartupScript(this, this.GetType(), "HiddenDetailTab", "HiddenDetailTab();", true);
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
        objProp_User.PageName = "AddProject.aspx";
        objProp_User.GridId = "gvWIPs";

        var ds = objBL_User.DeleteUserGridCustomSettings(objProp_User);


        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            var colIndex = 0;

            foreach (GridColumn column in gvWIPs.MasterTableView.OwnerGrid.Columns)
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
            //gvWIPs.MasterTableView.Rebind();
        }
        else
        {
            //var arrColumnOrder = new string[3]{ "ReviewCheck", "Comp", "" };
            var colIndex = 0;
            foreach (GridColumn column in gvWIPs.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                column.Display = true;

            }
            gvWIPs.MasterTableView.SortExpressions.Clear();
            gvWIPs.MasterTableView.GroupByExpressions.Clear();
            gvWIPs.EditIndexes.Clear();
            gvWIPs.Rebind();
        }
        //CompanyPermission();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "HiddenDetailTab", "HiddenDetailTab();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'WIP are restored Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        #endregion
    }

    private string GetGridColumnSettings()
    {
        var columnSettings = string.Empty;

        List<ColumnSettings> lstColSetts = new List<ColumnSettings>();
        foreach (GridColumn column in gvWIPs.MasterTableView.OwnerGrid.Columns)
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
        objProp_User.PageName = "AddProject.aspx";
        objProp_User.GridId = "gvWIPs";

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
        objProp_User.PageName = "AddProject.aspx";
        objProp_User.GridId = "gvWIPs";
        ds = objBL_User.GetGridUserSettings(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //string columnSettings = "[{Name: \"BType\", Display: true, Width: 300},{Name: \"MatItem\", Display: false, Width: 300}]";
            var columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            var colIndex = 0;

            foreach (GridColumn column in gvWIPs.MasterTableView.OwnerGrid.Columns)
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
    public void GetPeriodDate()
    {
        if (HttpContext.Current.Session["PeriodClose"] != null)
        {
            DataTable _dt = (DataTable)HttpContext.Current.Session["PeriodClose"];
            if (_dt.Rows.Count > 0)
            {
                if ((!string.IsNullOrEmpty(_dt.Rows[0]["fStart"].ToString())) && (!string.IsNullOrEmpty(_dt.Rows[0]["fEnd"].ToString())))
                {
                    DateTime _startDate = Convert.ToDateTime(_dt.Rows[0]["fStart"]);
                    DateTime _endDate = Convert.ToDateTime(_dt.Rows[0]["fEnd"]);

                    hdnPeriodStartDate.Value = _startDate.ToString();
                    hdnPeriodEndDate.Value = _endDate.ToString();
                }
            }
        }
    }

    protected void lnkBillingReverse_Click(object sender, EventArgs e)
    {
        DataSet tblReverseWIP = new DataSet();
        // Reverse Header Part
        DataTable tblReverseHeader = new DataTable();
        tblReverseHeader.Columns.Add("WIPs", typeof(string));
        tblReverseHeader.Columns.Add("JobId", typeof(int));
        tblReverseHeader.Columns.Add("ProgressBillingNo", typeof(string));
        tblReverseHeader.Columns.Add("BillingDate", typeof(DateTime));
        tblReverseHeader.Columns.Add("ApplicationStatusId", typeof(int));
        tblReverseHeader.Columns.Add("Terms", typeof(int));
        tblReverseHeader.Columns.Add("SalesTax", typeof(Decimal));
        tblReverseHeader.Columns.Add("PeriodDate", typeof(DateTime));
        tblReverseHeader.Columns.Add("RevisionDate", typeof(DateTime));
        tblReverseHeader.Columns.Add("UserName", typeof(string));

        DataRow drReverse = tblReverseHeader.NewRow();
        if (!string.IsNullOrEmpty(hdnWIPIDs.Value))
        {
            drReverse["WIPs"] = hdnWIPIDs.Value;
        }
        drReverse["JobId"] = Convert.ToInt32(Request.QueryString["uid"].ToString());
        drReverse["ProgressBillingNo"] = txtProgressBilling.Text;
        drReverse["BillingDate"] = Convert.ToDateTime(txtReserveDate.Text);
        drReverse["Terms"] = Convert.ToInt32(ddlTerms.SelectedValue);
        if (!string.IsNullOrEmpty(txtSalesTax.Text))
            drReverse["SalesTax"] = Convert.ToDecimal(txtSalesTax.Text);
        drReverse["PeriodDate"] = Convert.ToDateTime(txtPeriodDate.Text);
        drReverse["RevisionDate"] = Convert.ToDateTime(txtRevisionDate.Text);
        drReverse["username"] = Session["username"].ToString();
        objProp_Customer.ConnConfig = Session["config"].ToString();

        tblReverseHeader.Rows.Add(drReverse);
        tblReverseWIP.Tables.Add(tblReverseHeader);
        try
        {
            objProp_Customer.Reverse = tblReverseWIP;
            objBL_Customer.SaveReverseWip(objProp_Customer);
            GetWIP();
            string str = "Reverse Successfully!";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HiddenDetailTab", "HiddenDetailTab();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseReverseWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccessReverseWIP", "noty({text: '" + str + "',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});MoveToThirdTab();", true);
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HiddenDetailTab", "HiddenDetailTab();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrReverseWIP", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});MoveToThirdTab();", true);
            throw;
        }
    }
    public class ColumnSettings
    {
        public string Name { get; set; }
        public bool Display { get; set; }
        public int Width { get; set; }
        public int OrderIndex { get; set; }
    }
    public void Locstatus()
    {

        if (hdnLocID.Value != "")
        {

            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
            objPropUser.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_User.getLocationByID(objPropUser);


            string LoCStatus = ds.Tables[0].Rows[0]["status"].ToString();
            if (LoCStatus == "1")
            {
                lnkSaveTemplate.Visible = false;
                ClientScript.RegisterStartupScript(Page.GetType(), "keyerrrinactive", "noty({text: 'Location is inactive!.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }

    }
    protected void chkExpense_CheckedChanged(object sender, EventArgs e)
    {

        if (chkExpense.Checked)
        {
            gvBudget.Visible = false;
            gvExpenses.Visible = true;

            BindExpense();
        }
        else
        {
            gvBudget.Visible = true;
            gvExpenses.Visible = false;
        }


        //     $('#<%=chkExpense.ClientID%>').change(function() {
        //    if ($(this).is (':checked')) {
        //                $('#<%=gvBudget.ClientID%>').hide();
        //                $('#divBudgetPager').hide();
        //                $('#<%=gvExpenses.ClientID%>').show();
        //                $('#divExpensesPager').show();
        //    }
        //            else
        //    {
        //                $('#<%=gvBudget.ClientID%>').show();
        //                $('#divBudgetPager').show();
        //                $('#<%=gvExpenses.ClientID%>').hide();
        //                $('#divExpensesPager').hide();
        //    }
        //});



    }

    protected void lnkRefresh_Click(object sender, EventArgs e)
    {
        //BindBudget(1);
    }

    protected void lnkReloadEstimates_Click(object sender, EventArgs e)
    {
        var strOpp = hdnOpportunityId.Value;
        if (!string.IsNullOrEmpty(strOpp))
        {
            DataSet ds = new DataSet();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.OpportunityID = Convert.ToInt32(strOpp);

            ds = objBL_Customer.GetEstimatesByOpportunityIDForProjectLinking(objProp_Customer);
            if (ds.Tables[0].Rows.Count > 0)
            {
                RadGrid_Estimate.VirtualItemCount = ds.Tables[0].Rows.Count;
                RadGrid_Estimate.DataSource = ds.Tables[0];
                RadGrid_Estimate.DataBind();
                //RadGrid_Estimate.Rebind();
            }
            else
            {
                RadGrid_Estimate.VirtualItemCount = 0;
                RadGrid_Estimate.DataSource = String.Empty;
                RadGrid_Estimate.DataBind();
            }

        }
        else
        {
            RadGrid_Estimate.Rebind();
        }

    }

}



[Serializable]
public class PlannerProjectModel
{
    public int ID { get; set; }
    public String Group { get; set; }
    public String Code { get; set; }
    public String fDesc { get; set; }
    public String Type { get; set; }
    public Double Duration { get; set; }
    public string DurationUnit { get; set; }
    public Nullable<DateTime> StartDate { get; set; }
    public Nullable<DateTime> EndDate { get; set; }
    public Nullable<DateTime> GroupStartDate { get; set; }
    public Nullable<DateTime> GroupEndDate { get; set; }
    public Nullable<DateTime> CodeStartDate { get; set; }
    public Nullable<DateTime> CodeEndDate { get; set; }
    public string VendorName { get; set; }
    public int VendorID { get; set; }
    public Double ActualHours { get; set; }
}



