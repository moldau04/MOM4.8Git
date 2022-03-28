using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Net;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text;
using Novacode;
using System.Text.RegularExpressions;
using Spire.Doc;
using Telerik.Web.UI;
using Spire.Doc.Documents;
using Newtonsoft.Json;
using Stimulsoft.Report;
using MOMWebApp;
using Newtonsoft.Json.Linq;
using Table = Novacode.Table;
using System.Drawing;
using System.Threading;
using System.Web.Configuration;
using BusinessLayer.Utility;

public partial class AddEstimate : System.Web.UI.Page
{
    #region ::Declaration::
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    protected DataTable dtBomType = new DataTable();

    protected DataTable dtBomItem = new DataTable();

    protected DataTable dtCurrency = new DataTable();
    JobT _objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();

    Wage _objWage = new Wage();
    BL_User objBL_User = new BL_User();
    User _objUser = new User();
    MapData objMapData = new MapData();
    BL_MapData objBL_MapData = new BL_MapData();

    protected DataTable dtMat = new DataTable();

    protected DataTable dtLab = new DataTable();

    BusinessEntity.EstimateForm objEF = new BusinessEntity.EstimateForm();
    Lead leadData = new Lead();
    BL_Lead objBL_Lead = new BL_Lead();

    STax staxData = new STax();
    BL_STax objBL_STax = new BL_STax();

    BL_EstimateForm objBL_EF = new BL_EstimateForm();

    BusinessEntity.EstimateTemplate objET = new BusinessEntity.EstimateTemplate();
    BL_EstimateTemplate objBL_ET = new BL_EstimateTemplate();
    protected DataTable dtCustomField = new DataTable();
    #endregion

    #region ::PageEvents::
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            UserPermission();

            WebBaseUtility.UpdatePageTitle(this, "Estimate", Request.QueryString["uid"], Request.QueryString["t"]);
            txtJobType.Enabled = false;
            Label11.Attributes.Add("class", "active");
            FillBomType();
            FillWage();
            if (!IsPostBack)
            {
                InitData();

                objProp_Customer.ConnConfig = Session["config"].ToString();
                ViewState["edit"] = AddEstimate.EstimatMode.Add;
                hdnEstimateMode.Value = ViewState["edit"].ToString();

                if (Request.QueryString["uid"] != null)
                {

                    // Edit
                    if (Request.QueryString["t"] != "c")
                    {
                        ViewState["edit"] = AddEstimate.EstimatMode.Edit;
                        hdnEstimateMode.Value = ViewState["edit"].ToString();
                        GetEstimate(AddEstimate.EstimatMode.Edit);
                        hdnestimateid.Value = Request.QueryString["uid"];
                        GetDocuments();
                        GetEstimateForms();
                        //lnkConvert.Visible = true;
                        //lnkUndoConvert.Visible = false;
                        //liLinkProject.Visible = true;
                        Label13.Text = "Edit Estimate";
                        liLogs.Style["display"] = "inline-block";
                        tbLogs.Style["display"] = "block";
                        FillTasks(hdnROLId.Value, chkShowAllTasks.Checked);
                        pnlApproveProposal.Visible = true;
                    }
                    else // Copy
                    {
                        ViewState["edit"] = AddEstimate.EstimatMode.Copy;
                        hdnEstimateMode.Value = ViewState["edit"].ToString();
                        GetEstimate(AddEstimate.EstimatMode.Copy);
                        hdnestimateid.Value = Request.QueryString["uid"];
                        GetDocuments();
                        GetEstimateForms();
                        //lnkConvert.Visible = false;
                        //lnkUndoConvert.Visible = false;
                        //liLinkProject.Visible = false;
                        pnlReportButton.Visible = false;
                        Label13.Text = "Copy Estimate";
                        pnlApproveProposal.Visible = false;
                    }

                    GetDataEquipmentForGrid();
                    UpdateSelectedEquipmentBySelectedGroup(Convert.ToInt32(ddlEstimateGroup.SelectedValue));

                    if (Session["linkEstimate"] != null)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccess", "noty({text: '" + Session["linkEstimate"].ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        Session["linkEstimate"] = null;
                    }
                    //RadGrid_Project.Rebind();
                    //ShowLinkProject();
                }
                else
                {
                    TxtDate.Text = DateTime.Now.ToShortDateString();
                    CreateBOMTable();
                    CreateMilestoneTable();

                    //SetExchange();
                    // CreateTable();
                    pnlDocumentButtons.Visible = true;
                    pnlReportButton.Visible = false;
                    lnkConvert.Visible = false;
                    lnkUndoConvert.Visible = false;
                    hdnLinkedProject.Value = "";
                    liLinkProject.Visible = false;
                    pnlApproveProposal.Visible = false;
                    Label13.Text = "Add Estimate";

                    // Init value for hdnOpportunity.Value
                    if (Request.QueryString["opp"] != null)
                    {
                        hdnOpportunity.Value = Request.QueryString["opp"];
                        GetOpportunity(Convert.ToInt32(hdnOpportunity.Value));
                        // Disable opp info
                        txtOppName.Enabled = false;
                        //ddlOppStage.Enabled = false;
                        FillOpportunityDropdown(Convert.ToInt32(hdnROLId.Value));
                        //SetSelectedValueForDDL(ddlOpportunity, hdnOpportunity.Value);
                        SetSelectedValueForddlOpportunity(hdnOpportunity.Value);
                        FillTemplateDropdown(Convert.ToInt32(hdnOpportunity.Value), 0, "");
                        FillGroupNameDropdown(Convert.ToInt32(hdnROLId.Value));
                        GetDataEquipmentForGrid();
                        //divGroupEquipments.Visible = true;
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateGroupPanel", "ShowHideGroupEquipments(true);", true);
                    }
                    else
                    {
                        hdnOpportunity.Value = string.Empty;
                        // Enable opp info
                        txtOppName.Enabled = true;
                        //ddlOppStage.Enabled = true;
                        FillTemplateDropdown(0, 0, "");
                        //divGroupEquipments.Visible = false;
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateGroupPanel", "ShowHideGroupEquipments(false);", true);
                    }

                    if (ddlEstimateGroup.SelectedValue != "0" && ddlEstimateGroup.SelectedValue != "")
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateEquimentPanel", "ShowHideEquipments(true);", true);
                    else
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateEquimentPanel", "ShowHideEquipments(false);", true);
                }

                if (!string.IsNullOrEmpty(hdnROLId.Value) && hdnROLId.Value != "0")
                {
                    lnkAddnewContact.Visible = true;
                    btnEditContact.Visible = true;
                    btnDeleteContact.Visible = true;
                }
                else
                {
                    lnkAddnewContact.Visible = false;
                    btnEditContact.Visible = false;
                    btnDeleteContact.Visible = false;
                }

                //DataTable dtCustom = GetCustomItems();
                BindCustomGrid();
                if (ddlEstimateType.SelectedValue == "bid")
                {
                    gvMilestones.MasterTableView.GetColumn("Quantity").Display = false;
                    gvMilestones.MasterTableView.GetColumn("Price").Display = false;
                    gvMilestones.MasterTableView.GetColumn("AmountPer").Display = true;
                    txtOverride.Style["display"] = "block";
                    lblFinalBid.Style["display"] = "none";
                }
                else
                {
                    gvMilestones.MasterTableView.GetColumn("Quantity").Display = true;
                    gvMilestones.MasterTableView.GetColumn("Price").Display = true;
                    gvMilestones.MasterTableView.GetColumn("AmountPer").Display = false;
                    txtOverride.Style["display"] = "none";
                    lblFinalBid.Style["display"] = "block";
                }
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateDiscountPanel", "ShowHideDiscountNotes();", true);
            }
            HighlightSideMenu();
            // Permission();
            CompanyPermission();

            pnlNext.Visible = false;
            if (Request.QueryString["uid"] != null)
            {
                if (Request.QueryString["t"] != "c")
                {
                    pnlNext.Visible = true;
                    RevisionNotes_1.Visible = true;
                    //RevisionNotes_2.Visible = true;
                    lnkRevisionSave.Visible = true;
                }
                else
                {
                    pnlNext.Visible = false;
                    //liForms.Style["display"] = "none";
                    //five.Style["display"] = "none";
                }
            }
            else
            {
                //liForms.Style["display"] = "none";
                //five.Style["display"] = "none";
            }

            if (Session["es_status"] != null)
            {
                if (Convert.ToString(Session["es_status"]) == "a")
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysuccess", "noty({text: 'Estimate Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
                else if (Convert.ToString(Session["es_status"]) == "u")
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysuccess", "noty({text: 'Estimate# " + objProp_Customer.estimateno + " Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
                else if (Convert.ToString(Session["es_status"]) == "c")
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Estimate Converted to Project Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                }
                Session["es_status"] = null;
            }
            if (Session["ConvertToRecContractSucc"] != null && Session["ConvertToRecContractSucc"].ToString() == "1")
            {
                Session["ConvertToRecContractSucc"] = null;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyConvertSuccess", "noty({text: 'Estimate Converted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }

    }


    private void HighlightSideMenu()
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkEstimate");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        div.Style.Add("display", "block");
    }

    private void InitData()
    {
        DataSet ds = new DataSet();
        var connConfig = Session["config"].ToString();
        int EstimateID = Request.QueryString["uid"] != null ? Convert.ToInt32(Request.QueryString["uid"].ToString()) : 0;
        int saleAssigned = new GeneralFunctions().GetSalesAsigned();
        ds = objBL_Job.GetDataOnInitialEstimate(connConfig, saleAssigned, EstimateID, 1);

        if (ds.Tables.Count > 4)
        {
            // Category
            drpCategory.DataSource = ds.Tables[0];
            drpCategory.DataTextField = "Category";
            drpCategory.DataValueField = "Category";
            drpCategory.DataBind();

            drpCategory.Items.Insert(0, new ListItem("Select", "Select"));
            drpCategory.Items.Add(new ListItem("Other", "Other"));

            // Stage
            ddlOppStage.DataSource = ds.Tables[1];
            ddlOppStage.DataTextField = "DescWithProbability";
            ddlOppStage.DataValueField = "ID";
            ddlOppStage.DataBind();
            ddlOppStage.Items.Insert(0, new ListItem("Select Opportunity Stage", "0"));

            // Status
            if (ds.Tables[2].Rows.Count > 0)
            {
                ddlStatus.DataSource = ds.Tables[2];
                ddlStatus.DataTextField = "Name";
                ddlStatus.DataValueField = "ID";
                ddlStatus.DataBind();
            }
            ddlStatus.Items.Insert(0, new ListItem("Select", ""));
            ddlStatus.SelectedValue = "1";

            // SaleTax
            if (ds.Tables[3] != null)
            {
                drpSaleTax.DataSource = ds.Tables[3];
                drpSaleTax.DataTextField = "NameRate";
                drpSaleTax.DataValueField = "Name";
                drpSaleTax.DataBind();
                drpSaleTax.Items.Insert(0, new ListItem("Select Sales Tax", "0"));
            }

            // Employee
            if (ds.Tables[4].Rows.Count > 0)
            {
                ddlEmployees.DataSource = ds.Tables[4];
                ddlEmployees.DataTextField = "SDesc";
                ddlEmployees.DataValueField = "ID";
                ddlEmployees.DataBind();
            }
            ddlEmployees.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    //private void BindCategory()
    //{
    //    objProp_Customer.ConnConfig = Session["config"].ToString();
    //    DataSet ds = new DataSet();

    //    ds = objBL_Customer.getEstimateCategory(objProp_Customer);

    //    drpCategory.DataSource = ds.Tables[0];
    //    drpCategory.DataTextField = "Category";
    //    drpCategory.DataValueField = "Category";
    //    drpCategory.DataBind();

    //    drpCategory.Items.Insert(0, new ListItem("Select", "Select"));
    //    drpCategory.Items.Add(new ListItem("Other", "Other"));
    //}

    //private void FillOpportunityStatus()
    //{
    //    objProp_Customer.ConnConfig = Session["config"].ToString();
    //    DataSet ds = new DataSet();

    //    ds = objBL_Customer.getOpportunityStatus(objProp_Customer);

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        //lblProduct.Text = ds.Tables[0].Rows[0]["Label"].ToString();
    //        ddlStatus.DataSource = ds.Tables[0];
    //        ddlStatus.DataTextField = "Name";
    //        ddlStatus.DataValueField = "ID";
    //        ddlStatus.DataBind();
    //    }
    //    else
    //    {
    //        //lblProduct.Text = "Products;
    //        //ddlProduct.Items.Insert(0, new ListItem("--Select--", ""));
    //    }
    //    ddlStatus.Items.Insert(0, new ListItem("Select", ""));

    //    ddlStatus.SelectedValue = "1";
    //    //ddlStatus.SelectedIndex = 0;
    //}

    //private void BindOpportunityStage()
    //{
    //    leadData.ConnConfig = Session["config"].ToString();
    //    var ds = objBL_Lead.GetAllStage(leadData);
    //    if (ds != null && ds.Tables[0] != null)
    //    {
    //        ddlOppStage.DataSource = ds.Tables[0];
    //        ddlOppStage.DataTextField = "DescWithProbability";
    //        ddlOppStage.DataValueField = "ID";
    //        ddlOppStage.DataBind();
    //        ddlOppStage.Items.Insert(0, new ListItem("Select Opportunity Stage", "0"));
    //    }
    //}

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

    //private void BindSalesTax()
    //{
    //    staxData.ConnConfig = Session["config"].ToString();
    //    staxData.UType = 1;
    //    var ds = objBL_STax.GetAllSTaxByUType(staxData);
    //    if (ds != null && ds.Tables[0] != null)
    //    {
    //        drpSaleTax.DataSource = ds.Tables[0];
    //        drpSaleTax.DataTextField = "NameRate";
    //        //drpSaleTax.DataValueField = "Rate";
    //        drpSaleTax.DataValueField = "Name";
    //        drpSaleTax.DataBind();
    //        drpSaleTax.Items.Insert(0, new ListItem("Select Sales Tax", "0"));
    //    }
    //}
    #endregion

    #region ::Methods::

    //private void Permission()
    //{
    //    //HyperLink li = (HyperLink)Page.Master.FindControl("salesMgr");

    //    ////li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
    //    //li.Attributes.Add("class", "start active open");

    //    //HyperLink a = (HyperLink)Page.Master.FindControl("SalesLink");
    //    ////a.Style.Add("color", "#2382b2");

    //    //HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkEstimate");
    //    ////lnkUsersSmenu.Style.Add("color", "#FF7A0A");
    //    //lnkUsersSmenu.Style.Add("color", "#316b9d");
    //    //lnkUsersSmenu.Style.Add("font-weight", "normal");
    //    //lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
    //    ////AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
    //    ////hm.Enabled = false;
    //    //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
    //    ////ul.Attributes.Remove("class");
    //    ////ul.Style.Add("display", "block");

    //    if (Session["type"].ToString() == "c")
    //    {
    //        Response.Redirect("home.aspx");
    //    }

    //    if (Session["MSM"].ToString() == "TS")
    //    {
    //        //lnkDelete.Visible = false;
    //    }
    //    //if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
    //    //{
    //    //    Response.Redirect("home.aspx");
    //    //}

    //    if (Session["type"].ToString() != "am")
    //    {
    //        DataTable dt = new DataTable();
    //        dt = (DataTable)Session["userinfo"];
    //        string Sales = dt.Rows[0]["sales"].ToString().Substring(0, 1);

    //        if (Sales == "N")
    //        {
    //            Response.Redirect("home.aspx");
    //        }
    //    }

    //}
    private void UserPermission()
    {

        // This validation only need to Customer and Location so that, we need to remove it on Sale module
        //if (Session["type"].ToString() == "c")
        //{
        //    Response.Redirect("home.aspx");
        //}

        //if (Session["MSM"].ToString() == "TS")
        //{
        //    Response.Redirect("home.aspx");

        //}
        //if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        //{
        //    Response.Redirect("home.aspx");
        //}

        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable dt = new DataTable();
            dt = GetUserById();

            /// SalesManager ///////////////////------->

            string SalesManagerPermission = dt.Rows[0]["SalesManager"] == DBNull.Value ? "Y" : dt.Rows[0]["SalesManager"].ToString();

            if (SalesManagerPermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// UserSales ///////////////////------->

            string EstimatesPermission = dt.Rows[0]["Estimates"] == DBNull.Value ? "YYYYYY" : dt.Rows[0]["Estimates"].ToString();
            string ADD = EstimatesPermission.Length < 1 ? "Y" : EstimatesPermission.Substring(0, 1);
            string Edit = EstimatesPermission.Length < 2 ? "Y" : EstimatesPermission.Substring(1, 1);
            string Delete = EstimatesPermission.Length < 3 ? "Y" : EstimatesPermission.Substring(2, 1);
            string View = EstimatesPermission.Length < 4 ? "Y" : EstimatesPermission.Substring(3, 1);
            string Report = EstimatesPermission.Length < 6 ? "Y" : EstimatesPermission.Substring(5, 1);

            string AwardEstimatesPermission = dt.Rows[0]["AwardEstimates"] == DBNull.Value ? "YYYYYY" : dt.Rows[0]["AwardEstimates"].ToString();

            string AwardEstimates = AwardEstimatesPermission.Length < 3 ? "Y" : AwardEstimatesPermission.Substring(2, 1);
            var isApproveProposal = dt.Rows[0]["EstApproveProposal"] == null ? false : Convert.ToBoolean(dt.Rows[0]["EstApproveProposal"]);

            if (isApproveProposal)
            {
                //lnkApplyApproveStatus.Visible = true;
                ViewState["EstimateApproveProposalPermission"] = "Y";
            }
            else
            {
                //lnkApplyApproveStatus.Visible = false;
                ViewState["EstimateApproveProposalPermission"] = "N";
            }

            if (AwardEstimates == "N")
            {
                //lnkConvert.Visible = false;
                //lnkUndoConvert.Visible = false;
                Session["EstimateConvertPermission"] = "N";
            }
            else
            {
                //lnkConvert.Visible = true;
                //lnkUndoConvert.Visible = true;
                Session["EstimateConvertPermission"] = "Y";
            }

            //if (Request.QueryString["uid"] != null)
            //{
            //    aImport.Visible = false;
            //}

            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            else if (Request.QueryString["uid"] == null)
            {
                if (ADD == "N")
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }
            else if (Edit == "N")
            {
                if (View == "Y")
                {
                    lnkSaveEstimate.Visible = false;
                    //btnSubmitJob.Visible = false;
                    Session["EstimateEditPermission"] = "N";
                }
                else
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }

            if (Edit == "Y")
            {
                Session["EstimateEditPermission"] = "Y";
            }

            if (Report == "N")
            {
                pnlReportButton.Visible = false;
            }

            
        }
        else if (Session["type"].ToString() == "am")
        {
            Session["EstimateConvertPermission"] = "Y";
            Session["EstimateEditPermission"] = "Y";
            ViewState["EstimateApproveProposalPermission"] = "Y";
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

    private void GetEstimate(AddEstimate.EstimatMode edit)
    {
        try
        {
            if (edit == AddEstimate.EstimatMode.Edit)
            {
                Label13.Text = "Edit Estimate";

                // btnnewlab.Visible = false;
                ViewState["edit"] = AddEstimate.EstimatMode.Edit;
                hdnEstimateMode.Value = ViewState["edit"].ToString();
                btnGenerate.Visible = true;
                //btnSendEmail.Visible = true;

                DataSet ds = new DataSet();
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
                //ds = objBL_Customer.getEstimateTemplateByID(objProp_Customer);
                ds = objBL_Customer.GetEstimateByID(objProp_Customer);

                hdnCustId.Value = ds.Tables[6].Rows[0]["Rol"].ToString();
                hdnLocID.Value = ds.Tables[0].Rows[0]["locid"].ToString();
         

                    #region Phone
                    //if (Convert.ToString(ds.Tables[0].Rows[0]["rolid"]) != "")
                    //{
                    //    Int32 Rol = Convert.ToInt32(ds.Tables[0].Rows[0]["rolid"]);

                    //    #region Bind Contact Name Drop Down
                    //    if (Convert.ToString(Request.QueryString["t"]) != "c")
                    //    {
                    //        objProp_Customer.ROL = Rol;
                    //        objProp_Customer.ProspectID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProspectID"]);
                    //        DataSet dsPhone = new DataSet();
                    //        dsPhone = objBL_Customer.getPhoneByRol(objProp_Customer);
                    //        ddlContact.Items.Clear();
                    //        ddlContact.DataSource = dsPhone.Tables[0];
                    //        ddlContact.ClearSelection();
                    //        ddlContact.DataTextField = "fDesc";
                    //        ddlContact.DataValueField = "ID";
                    //        ddlContact.SelectedValue = null;
                    //        ddlContact.DataBind();
                    //    }
                    //    #endregion

                    //    SetSelectedValueForDDL(ddlContact, ds.Tables[0].Rows[0]["Contact"].ToString());
                    //}
                    #endregion


                    if (ds.Tables[0].Rows.Count > 0)
                {
                    var estProjectID = ds.Tables[0].Rows[0]["job"].ToString();
                    if (estProjectID != "")
                    {
                        //trProj.Visible = true;
                        //lnkProject.Text = "# " + ds.Tables[0].Rows[0]["job"].ToString();
                        //lnkProject.NavigateUrl = "addproject.aspx?uid=" + ds.Tables[0].Rows[0]["job"].ToString();
                        string url = "<span  style='float :left' >Project #</span><a style='float :left' href='addproject?uid=" + estProjectID + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) + "'>" + estProjectID + "</a>";
                        trProj.InnerHtml = url.ToString();

                        //lnkConvert.Enabled = false;
                        string vr = "Estimate already converted/linked to project" + "# " + estProjectID + "!";
                        lnkConvert.Attributes["onclick"] = " noty({ text: '" + vr + "', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";

                        liLinkProject.Visible = false;
                        lnkConvert.Visible = false;
                        lnkUndoConvert.Visible = Session["EstimateConvertPermission"] != null && Session["EstimateConvertPermission"].ToString() == "Y";
                        lnkSaveEstimate.Visible = false;

                        var convertId = ds.Tables[0].Rows[0]["ConvertID"].ToString();
                        var estConvertedId = ds.Tables[0].Rows[0]["EstConvertId"].ToString();
                        if (!string.IsNullOrEmpty(convertId) && !string.IsNullOrEmpty(estConvertedId))
                        {
                            lnkUndoConvert.Text = "Undo Convert";
                        }
                        else
                        {
                            lnkUndoConvert.Text = "Unlink Project";
                        }

                        hdnLinkedProject.Value = estProjectID;
                    }
                    else
                    {
                        liLinkProject.Visible = true;
                        lnkConvert.Visible = Session["EstimateConvertPermission"] != null && Session["EstimateConvertPermission"].ToString() == "Y";
                        lnkUndoConvert.Visible = false;
                        lnkSaveEstimate.Visible = Session["EstimateEditPermission"] != null && Session["EstimateEditPermission"].ToString() == "Y";
                        hdnLinkedProject.Value = "";
                    }

                    // Check and disable convert button in case estimate status != 'Open'
                    var estStatus = ds.Tables[0].Rows[0]["status"].ToString();
                    if (estStatus != "1")// not Open
                    {
                        liLinkProject.Visible = false;
                        lnkConvert.Visible = false;
                    }

                    TxtEstimateNo.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                    //  txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtREPdesc.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
                    txtComment.Text = ds.Tables[0].Rows[0]["Comment"].ToString();
                    txtREPremarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                    txtCont.Text = ds.Tables[0].Rows[0]["LocationName"].ToString();
                  
                    if (ds.Tables[0].Rows[0]["fFor"].ToString().Equals("ACCOUNT", StringComparison.InvariantCultureIgnoreCase))
                    {
                        hdnLocID.Value = ds.Tables[0].Rows[0]["locid"].ToString();
                        hdnROLId.Value = ds.Tables[0].Rows[0]["rolid"].ToString();
                     
                        hdProspect.Value = string.Empty;
                    }
                    else
                    {
                        hdnLocID.Value = string.Empty;
                        hdnROLId.Value = ds.Tables[0].Rows[0]["rolid"].ToString();
                        hdProspect.Value = ds.Tables[0].Rows[0]["ProspectID"].ToString();
                    }

                    FillGroupNameDropdown(Convert.ToInt32(hdnROLId.Value));

                    //SetSelectedValueForDDL(ddlEstimateGroup, ds.Tables[0].Rows[0]["GroupId"].ToString());
                    SetSelectedValueForddlEstimateGroup(ds.Tables[0].Rows[0]["GroupId"].ToString());

                    FillOpportunityDropdown(Convert.ToInt32(hdnROLId.Value));
                    #region Get Opportunity Data
                    if (Convert.ToString(ds.Tables[0].Rows[0]["Opportunity"]) != "")
                    {
                        objProp_Customer.OpportunityID = Convert.ToInt32(ds.Tables[0].Rows[0]["Opportunity"]);
                        DataSet dsLead = new DataSet();
                        dsLead = objBL_Customer.getOpportunityByID(objProp_Customer);
                        if (dsLead.Tables[0].Rows.Count > 0)
                        {
                            //SetSelectedValueForDDL(ddlOpportunity, dsLead.Tables[0].Rows[0]["ID"].ToString());
                            SetSelectedValueForddlOpportunity(dsLead.Tables[0].Rows[0]["ID"].ToString());
                            hdnOpportunity.Value = ddlOpportunity.SelectedValue;
                            txtOppName.Text = dsLead.Tables[0].Rows[0]["fDesc"].ToString();
                            SetSelectedValueForDDL(ddlOppStage, dsLead.Tables[0].Rows[0]["OpportunityStageID"].ToString());
                        }
                    }
                    else
                    {
                        txtOppName.Text = txtREPdesc.Text;
                    }
                    #endregion


                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["BDate"].ToString()))
                    {
                        txtBidDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["BDate"].ToString()).ToShortDateString();
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["fDate"].ToString()))
                    {
                        TxtDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["fDate"].ToString()).ToShortDateString();
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["SoldDate"].ToString()))
                    {
                        txtSoldDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["SoldDate"].ToString()).ToShortDateString();
                    }
                    //   txtBillAddress.Text = ds.Tables[0].Rows[0]["EstimateBillAddress"].ToString();
                    txtPhone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                    txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
                    //  txtContactAddress.Text = ds.Tables[0].Rows[0]["EstimateAddress"].ToString();

                    lblHeaderLabel.Text = "Estimate# " + Convert.ToString(ds.Tables[0].Rows[0]["ID"]);
                    if (Convert.ToString(ds.Tables[0].Rows[0]["EstimateAddress"]) != "")
                    {
                        lblHeaderLabel.Text = lblHeaderLabel.Text + " | " + Convert.ToString(ds.Tables[0].Rows[0]["EstimateAddress"]);
                    }

                    txtEmail.Text = ds.Tables[0].Rows[0]["EstimateEmail"].ToString();
                    txtCellNew.Text = ds.Tables[0].Rows[0]["EstimateCell"].ToString();
                    txtContact.Text = ds.Tables[0].Rows[0]["Contact"].ToString();
                    txtJobType.Text = ds.Tables[0].Rows[0]["JobType"].ToString();
                    SetSelectedValueForDDL(ddlStatus, ds.Tables[0].Rows[0]["status"].ToString());
                    txtCompanyName.Text = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                    String comonayName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                    //FillLocInfo();
                    string Terr = ds.Tables[0].Rows[0]["EstimateUserId"].ToString();
                    SetSelectedValueForDDL(ddlEmployees, Terr);
                    SetSelectedValueForDDL(ddlEstimateType, ds.Tables[0].Rows[0]["EstimateType"].ToString());
                    chkSglBilAmt.Checked = (bool)ds.Tables[0].Rows[0]["IsSglBilAmt"];
                    chkBilFrmBOM.Checked = (bool)ds.Tables[0].Rows[0]["IsBilFrmBOM"];
                    chkDiscounted.Checked = (bool)ds.Tables[0].Rows[0]["Discounted"];
                    chkPayCertified.Checked = (bool)ds.Tables[0].Rows[0]["IsCertifiedProject"];
                    txtDiscountedNotes.Text = ds.Tables[0].Rows[0]["DiscountedNotes"].ToString();
                    //New
                    txtComment.Text = ds.Tables[0].Rows[0]["Comment"].ToString();

                    DataTable dtitems = ds.Tables[1].Copy();
                    DataSet dsLabor = new DataSet();
                    objProp_Customer.ConnConfig = Session["config"].ToString();
                    objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());

                    lblBidPrice.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["BidPrice"]));
                    hdnBidPrice.Value = ds.Tables[0].Rows[0]["BidPrice"].ToString();
                    txtOverride.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FinalBid"].ToString()) ? "" : string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["FinalBid"]));
                    hdnFinalBid.Value = ds.Tables[0].Rows[0]["FinalBid"].ToString();
                    lblFinalBid.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FinalBid"].ToString()) ? "" : string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["FinalBid"]));
                    txtContingencies.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["Cont"]));
                    txtPerContingencies.Text = Convert.ToString(ds.Tables[0].Rows[0]["ContPer"]);
                    txtOH.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["OH"]));
                    //hd_oh.Value = ds.Tables[0].Rows[0]["OH"].ToString();
                    txtHOPercentAge.Text = Convert.ToString(ds.Tables[0].Rows[0]["OHPer"]);
                    txtMarkupPercentAge.Text = Convert.ToString(ds.Tables[0].Rows[0]["MarkupPer"]);
                    hd_markup.Value = Convert.ToString(ds.Tables[0].Rows[0]["MarkupVal"]);
                    //hd_markupper.Value = Convert.ToString(ds.Tables[0].Rows[0]["MarkupPer"]);
                    txtPercentAgeCommission.Text = Convert.ToString(ds.Tables[0].Rows[0]["CommissionPer"]);
                    txtCommission.Text = Convert.ToString(ds.Tables[0].Rows[0]["CommissionVal"]);

                    #region Retain the Sales Tax
                    String _query = Convert.ToString(ds.Tables[0].Rows[0]["STaxName"]);
                    if (_query != "Select Sales Tax" && _query != "")
                    {
                        String[] staxSubStrings = _query.Split('/');
                        String _staxSelectedValue = "";
                        if (staxSubStrings.Length > 1)
                        {
                            for (int i = 0; i < staxSubStrings.Count() - 1; i++)
                            {
                                _staxSelectedValue = _staxSelectedValue + staxSubStrings[i] + "/";
                            }
                            _staxSelectedValue = _staxSelectedValue.Trim();
                            if (_staxSelectedValue.Length > 0)
                            {
                                _staxSelectedValue = _staxSelectedValue.Substring(0, _staxSelectedValue.Length - 1);
                            }
                        }
                        else
                        {
                            _staxSelectedValue = _query;
                        }

                        SetSelectedValueForDDL(drpSaleTax, _staxSelectedValue.Trim());
                    }
                    #endregion

                    SetSelectedValueForDDL(drpCategory, ds.Tables[0].Rows[0]["Category"].ToString());

                    hdnExchangeRate.Value = ds.Tables[0].Rows[0]["cadexchange"].ToString();
                    //hdnSelectedTemplate.Value = ds.Tables[0].Rows[0]["template"].ToString();
                    var template = ds.Tables[0].Rows[0]["template"].ToString();
                    dsLabor = objBL_Customer.GetEstimateLaborForEstimate(objProp_Customer);
                    ddlTemplate.Enabled = true;
                    if (!string.IsNullOrEmpty(hdnOpportunity.Value))
                    {
                        //FillTemplateDropdown(Convert.ToInt32(hdnOpportunity.Value), objProp_Customer.estimateno, hdnSelectedTemplate.Value);
                        FillTemplateDropdown(Convert.ToInt32(hdnOpportunity.Value), objProp_Customer.estimateno, template);
                    }
                    else
                    {
                        //FillTemplateDropdown(0, objProp_Customer.estimateno, hdnSelectedTemplate.Value);
                        FillTemplateDropdown(0, objProp_Customer.estimateno, template);
                    }

                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        gvMilestones.DataSource = ds.Tables[3];
                        gvMilestones.DataBind();
                    }
                    else
                    {
                        CreateMilestoneTable();
                    }


                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        //gvBOM.DataSource = ds.Tables[4];
                        //gvBOM.DataBind();
                        BindgvBOM(ds.Tables[4]);
                    }
                    else
                    {
                        CreateBOMTable();
                    }

                    #region Billing
                    txtBillRate.Text = Convert.ToString(ds.Tables[0].Rows[0]["BillRate"]);
                    txtOTRate.Text = Convert.ToString(ds.Tables[0].Rows[0]["OT"]);
                    txtRateNT.Text = Convert.ToString(ds.Tables[0].Rows[0]["RateNT"]);
                    txtDTRate.Text = Convert.ToString(ds.Tables[0].Rows[0]["DT"]);
                    txtTravelRate.Text = Convert.ToString(ds.Tables[0].Rows[0]["RateTravel"]);
                    txtMileageRate.Text = Convert.ToString(ds.Tables[0].Rows[0]["RateMileage"]);
                    txtAmount.Text = Convert.ToString(ds.Tables[0].Rows[0]["Amount"]);
                    SetSelectedValueForDDL(ddlPType, Convert.ToString(ds.Tables[0].Rows[0]["PType"]));
                    #endregion
                    string ffor = ds.Tables[0].Rows[0]["ffor"].ToString();

                    if (!ffor.Equals("ACCOUNT"))
                    {
                        objProp_Customer.ConnConfig = Session["config"].ToString();
                        objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
                        DataSet dsProspect = objBL_Customer.getProspectIDbyEstimateID(objProp_Customer);
                        if (dsProspect.Tables[0].Rows.Count > 0)
                        {
                            string prospectID = dsProspect.Tables[0].Rows[0]["ID"].ToString();
                            Session["prospectID"] = prospectID;
                            //Session["estimateID"] = Request.QueryString["uid"].ToString();
                            //lnkConvert.Visible = true;
                        }
                        else
                        {
                            Session["prospectID"] = null;
                            //lnkConvert.Visible = false;
                        }
                        if (hdnLocID.Value == "" || hdnLocID.Value == "0")
                        {
                            hdnCustId.Value = dsProspect.Tables[0].Rows[0]["ID"].ToString();
                        }
                    }
                    else
                    {
                        //lnkConvert.Visible = false;
                    }
                 

                    // Proposal Approval Status
                    var apprStatus = ds.Tables[0].Rows[0]["ApprovalStatus"].ToString();
                    
                    SetSelectedValueForDDL(ddlApprovalStatus, apprStatus);
                    txtApproveStatusComment.Text = ds.Tables[0].Rows[0]["ApprovedStatusComment"].ToString();
                    ViewState["CurrApproveStatus"] = apprStatus;
                    ViewState["CurrApproveStatusNote"] = txtApproveStatusComment.Text;
                    BL_General objBL_General = new BL_General();
                    var isApprove = objBL_General.GetSalesApproveEstimate(Session["config"].ToString());
                    //if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["ApplyEstApproveProposal"]) && WebConfigurationManager.AppSettings["ApplyEstApproveProposal"] == "1")
                    if (isApprove)
                    {
                        if (apprStatus == "1")//Approved
                        {
                            btnSendEmail.Visible = true;
                        }
                        else
                        {
                            btnSendEmail.Visible = false;
                        }
                    }
                    else
                    {
                        btnSendEmail.Visible = true;
                    }
                    //New
                    //if(hdnLocID.Value =="" ||hdnLocID.Value =="0")
                    //{
                     
                    //    lnkLocationID.NavigateUrl = " addprospect? uid=" + prospectID;
                    //}
                   
                    if (hdnLocID.Value != "0" && hdnLocID.Value!="")
                    {

                        lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;
                        GetDataEquip();
                    }
                    else if (hdnLocID.Value == "" || hdnLocID.Value == "0")
                    {
                        lnkCustomerID.NavigateUrl = "addprospect.aspx?uid=" + hdnCustId.Value;
                    }
                    else
                        lnkLocationID.NavigateUrl = "";

                    if (hdnCustId.Value != "0" && hdnCustId.Value!="" && hdnLocID.Value!="")
                    {
                        lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustId.Value;
                    }
                       
                    else if(hdnLocID.Value == "" || hdnLocID.Value == "0")
                    {
                        lnkCustomerID.NavigateUrl ="addprospect.aspx?uid=" + hdnCustId.Value;

                        lnkLocationID.NavigateUrl = "addprospect.aspx?uid=" + hdnCustId.Value;
                    }
                    else
                        lnkCustomerID.NavigateUrl = "";
                }
            }
            else if (edit == AddEstimate.EstimatMode.Copy)
            {
                Label13.Text = "Copy Estimate";

                // btnnewlab.Visible = false;
                ViewState["edit"] = AddEstimate.EstimatMode.Copy;
                hdnEstimateMode.Value = ViewState["edit"].ToString(); ;
                btnGenerate.Visible = false;
                btnSendEmail.Visible = false;
                TxtEstimateNo.Text = string.Empty;
                lblHeaderLabel.Text = string.Empty;
                trProj.Visible = false;
                lnkConvert.Visible = false;
                lnkUndoConvert.Visible = false;
                liLinkProject.Visible = false;
                hdnLinkedProject.Value = "";

                DataSet ds = new DataSet();
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
                ds = objBL_Customer.GetEstimateByID(objProp_Customer);

                #region Phone
                //if (Convert.ToString(ds.Tables[0].Rows[0]["rolid"]) != "")
                //{
                //    Int32 Rol = Convert.ToInt32(ds.Tables[0].Rows[0]["rolid"]);

                //    SetSelectedValueForDDL(ddlContact, ds.Tables[0].Rows[0]["Contact"].ToString());
                //}
                #endregion

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtREPdesc.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
                    txtREPremarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                    txtCont.Text = ds.Tables[0].Rows[0]["LocationName"].ToString();

                    if (ds.Tables[0].Rows[0]["fFor"].ToString().Equals("ACCOUNT", StringComparison.InvariantCultureIgnoreCase))
                    {
                        hdnLocID.Value = ds.Tables[0].Rows[0]["locid"].ToString();
                        hdnROLId.Value = ds.Tables[0].Rows[0]["rolid"].ToString();
                        hdProspect.Value = string.Empty;
                    }
                    else
                    {
                        hdnLocID.Value = string.Empty;
                        hdnROLId.Value = ds.Tables[0].Rows[0]["rolid"].ToString();
                        hdProspect.Value = ds.Tables[0].Rows[0]["ProspectID"].ToString();
                    }
                    FillGroupNameDropdown(Convert.ToInt32(hdnROLId.Value));

                    FillOpportunityDropdown(Convert.ToInt32(hdnROLId.Value));
                    #region Get Opportunity Data
                    if (Convert.ToString(ds.Tables[0].Rows[0]["Opportunity"]) != "")
                    {
                        objProp_Customer.OpportunityID = Convert.ToInt32(ds.Tables[0].Rows[0]["Opportunity"]);
                        DataSet dsLead = new DataSet();
                        dsLead = objBL_Customer.getOpportunityByID(objProp_Customer);
                        if (dsLead.Tables[0].Rows.Count > 0)
                        {
                            //SetSelectedValueForDDL(ddlOpportunity, dsLead.Tables[0].Rows[0]["ID"].ToString());
                            SetSelectedValueForddlOpportunity(dsLead.Tables[0].Rows[0]["ID"].ToString());
                            hdnOpportunity.Value = ddlOpportunity.SelectedValue;
                            txtOppName.Text = dsLead.Tables[0].Rows[0]["fDesc"].ToString();
                            SetSelectedValueForDDL(ddlOppStage, dsLead.Tables[0].Rows[0]["OpportunityStageID"].ToString());
                        }
                    }
                    #endregion


                    SetSelectedValueForddlEstimateGroup(ds.Tables[0].Rows[0]["GroupId"].ToString());

                    txtBidDate.Text = DateTime.Now.ToShortDateString();
                    TxtDate.Text = DateTime.Now.ToShortDateString();

                    //   txtBillAddress.Text = ds.Tables[0].Rows[0]["EstimateBillAddress"].ToString();
                    txtPhone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                    txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
                    //  txtContactAddress.Text = ds.Tables[0].Rows[0]["EstimateAddress"].ToString();

                    txtEmail.Text = ds.Tables[0].Rows[0]["EstimateEmail"].ToString();
                    txtCellNew.Text = ds.Tables[0].Rows[0]["EstimateCell"].ToString();
                    txtContact.Text = ds.Tables[0].Rows[0]["Contact"].ToString();

                    //hdnSelectedTemplate.Value = ds.Tables[0].Rows[0]["template"].ToString();
                    var template = ds.Tables[0].Rows[0]["template"].ToString();

                    if (!string.IsNullOrEmpty(hdnOpportunity.Value))
                    {
                        //FillTemplateDropdown(Convert.ToInt32(hdnOpportunity.Value), objProp_Customer.estimateno, hdnSelectedTemplate.Value);
                        FillTemplateDropdown(Convert.ToInt32(hdnOpportunity.Value), objProp_Customer.estimateno, template);
                    }
                    else
                    {
                        //FillTemplateDropdown(0, objProp_Customer.estimateno, hdnSelectedTemplate.Value);
                        FillTemplateDropdown(0, objProp_Customer.estimateno, template);
                    }

                    txtJobType.Text = ds.Tables[0].Rows[0]["JobType"].ToString();
                    //SetSelectedValueForDDL(ddlStatus, ds.Tables[0].Rows[0]["status"].ToString());
                    txtCompanyName.Text = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                    String comonayName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                    //FillLocInfo();
                    string Terr = ds.Tables[0].Rows[0]["EstimateUserId"].ToString();
                    SetSelectedValueForDDL(ddlEmployees, Terr);
                    SetSelectedValueForDDL(ddlEstimateType, ds.Tables[0].Rows[0]["EstimateType"].ToString());
                    chkSglBilAmt.Checked = (bool)ds.Tables[0].Rows[0]["IsSglBilAmt"];
                    chkBilFrmBOM.Checked = (bool)ds.Tables[0].Rows[0]["IsBilFrmBOM"];
                    chkDiscounted.Checked = (bool)ds.Tables[0].Rows[0]["Discounted"];
                    txtDiscountedNotes.Text = ds.Tables[0].Rows[0]["DiscountedNotes"].ToString();
                    //new
                    txtComment.Text = ds.Tables[0].Rows[0]["Comment"].ToString();
                    DataTable dtitems = ds.Tables[1].Copy();
                    //DataSet dsLabor = new DataSet();
                    objProp_Customer.ConnConfig = Session["config"].ToString();
                    objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());

                    lblBidPrice.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["BidPrice"]));
                    hdnBidPrice.Value = ds.Tables[0].Rows[0]["BidPrice"].ToString();
                    //txtOverride.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["FinalBid"]));
                    //txtOverride.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FinalBid"].ToString()) ? "" : string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["FinalBid"]));
                    txtOverride.Text = string.Empty;
                    hdnFinalBid.Value = string.Empty;
                    lblFinalBid.Text = string.Empty;
                    txtContingencies.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["Cont"]));
                    txtPerContingencies.Text = Convert.ToString(ds.Tables[0].Rows[0]["ContPer"]);
                    txtOH.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["OH"]));
                    txtHOPercentAge.Text = Convert.ToString(ds.Tables[0].Rows[0]["OHPer"]);
                    txtMarkupPercentAge.Text = Convert.ToString(ds.Tables[0].Rows[0]["MarkupPer"]);
                    hd_markup.Value = Convert.ToString(ds.Tables[0].Rows[0]["MarkupVal"]);
                    //hd_markupper.Value = Convert.ToString(ds.Tables[0].Rows[0]["MarkupPer"]);
                    txtPercentAgeCommission.Text = Convert.ToString(ds.Tables[0].Rows[0]["CommissionPer"]);
                    //txtCommission.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["CommissionVal"]));
                    txtCommission.Text = Convert.ToString(ds.Tables[0].Rows[0]["CommissionVal"]);

                    #region Retain the Sales Tax
                    String _query = Convert.ToString(ds.Tables[0].Rows[0]["STaxName"]);
                    if (_query != "Select Sales Tax" && _query != "")
                    {
                        String[] staxSubStrings = _query.Split('/');
                        String _staxSelectedValue = "";
                        for (int i = 0; i < staxSubStrings.Count() - 1; i++)
                        {
                            _staxSelectedValue = _staxSelectedValue + staxSubStrings[i] + "/";
                        }
                        _staxSelectedValue = _staxSelectedValue.Trim();
                        _staxSelectedValue = _staxSelectedValue.Substring(0, _staxSelectedValue.Length - 1);
                        SetSelectedValueForDDL(drpSaleTax, _staxSelectedValue.Trim());
                    }
                    #endregion

                    SetSelectedValueForDDL(drpCategory, ds.Tables[0].Rows[0]["Category"].ToString());

                    //SetExchange();
                    //dsLabor = (DataSet)ViewState["labor"];
                    ddlTemplate.Enabled = true;

                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        gvMilestones.DataSource = ds.Tables[3];
                        gvMilestones.DataBind();
                    }
                    else
                    {
                        CreateMilestoneTable();
                    }
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        //gvBOM.DataSource = ds.Tables[4];
                        //gvBOM.DataBind();
                        BindgvBOM(ds.Tables[4]);
                    }
                    else
                    {
                        CreateBOMTable();
                    }

                    #region Billing
                    txtBillRate.Text = Convert.ToString(ds.Tables[0].Rows[0]["BillRate"]);
                    txtOTRate.Text = Convert.ToString(ds.Tables[0].Rows[0]["OT"]);
                    txtRateNT.Text = Convert.ToString(ds.Tables[0].Rows[0]["RateNT"]);
                    txtDTRate.Text = Convert.ToString(ds.Tables[0].Rows[0]["DT"]);
                    txtTravelRate.Text = Convert.ToString(ds.Tables[0].Rows[0]["RateTravel"]);
                    txtMileageRate.Text = Convert.ToString(ds.Tables[0].Rows[0]["RateMileage"]);
                    txtAmount.Text = Convert.ToString(ds.Tables[0].Rows[0]["Amount"]);
                    SetSelectedValueForDDL(ddlPType, Convert.ToString(ds.Tables[0].Rows[0]["PType"]));
                    #endregion
                    string ffor = ds.Tables[0].Rows[0]["ffor"].ToString();
                    //if (!string.IsNullOrEmpty(ffor))
                    //{
                    //    lnkConvert.Visible = ffor.Equals("ACCOUNT") ? false : true;
                    //}

                    if (!ffor.Equals("ACCOUNT"))
                    {
                        objProp_Customer.ConnConfig = Session["config"].ToString();
                        objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
                        DataSet dsProspect = objBL_Customer.getProspectIDbyEstimateID(objProp_Customer);
                        if (dsProspect.Tables[0].Rows.Count > 0)
                        {
                            string prospectID = dsProspect.Tables[0].Rows[0]["ID"].ToString();
                            Session["prospectID"] = prospectID;
                            //Session["estimateID"] = Request.QueryString["uid"].ToString();
                            //lnkConvert.Visible = true;
                        }
                        else
                        {
                            Session["prospectID"] = null;
                            //lnkConvert.Visible = false;
                        }
                    }
                    else
                    {
                        //lnkConvert.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    private void IntializeControls()
    {
        GeneralFunctions objgn = new GeneralFunctions();
        objgn.ResetFormControlValues(this);

        //TxtDate.Text = DateTime.Now.ToShortDateString();
        CreateBOMTable();
        CreateMilestoneTable();
    }

    private DataTable JSONtoTableRestore(string strItems, DataTable dt, DataSet dsLabor, bool includeLaborItems)
    {
        if (strItems != string.Empty)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            int i = 0;
            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["Scope"] = dict["txtScope"];
                dr["vendor"] = dict["txtVendor"];
                dr["code"] = dict["txtCode"];
                dr["CodeDesc"] = dict["lblCodeDesc"].ToString().Trim();
                if (dict["ddlMeasure"].ToString().Trim() != string.Empty)
                    dr["measure"] = Convert.ToInt32(dict["ddlMeasure"]);

                if (dict["txtQuan"].ToString().Trim() != string.Empty)
                    dr["Quantity"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtQuan"].ToString(), "0"));

                if (dict["txtUnitCost"].ToString().Trim() != string.Empty)
                    dr["cost"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtUnitCost"].ToString(), "0"));

                dr["currency"] = dict["ddlCurrency"];

                if (dict["txtTotal"].ToString().Trim() != string.Empty)
                    dr["amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

                if (includeLaborItems)
                {
                    foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                    {
                        if (dict.ContainsKey("txt" + drLabor["item"].ToString()))
                        {
                            if (dict["txt" + drLabor["item"].ToString()].ToString().Trim() != string.Empty)
                                dr[drLabor["item"].ToString()] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txt" + drLabor["item"].ToString()].ToString(), "0"));
                        }
                    }
                }
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    private void FillTemplateDropdown(int oppId, int estimateId, string selectedValue)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.OpportunityID = oppId;
        objProp_Customer.estimateno = estimateId;
        //ds = objBL_Customer.getJobProjectTemplate(objProp_Customer);
        //ds = objBL_Customer.getEstimateTemplate(objProp_Customer);
        ds = objBL_Customer.GetTemplateByOppID(objProp_Customer);
        ddlTemplate.DataSource = ds.Tables[0];
        ddlTemplate.DataTextField = "fDesc";
        ddlTemplate.DataValueField = "id";
        ddlTemplate.DataBind();

        ddlTemplate.Items.Insert(0, new ListItem("Select Template", "0"));
        if (!string.IsNullOrEmpty(selectedValue))
        {
            SetSelectedValueForDDL(ddlTemplate, selectedValue);
        }
    }

    private void FillOpportunityDropdown(int rolId)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.RoleID = rolId;

        ds = objBL_Customer.GetAllOpportunityIDs(objProp_Customer);
        ddlOpportunity.DataSource = ds.Tables[0];
        ddlOpportunity.DataTextField = "name";
        ddlOpportunity.DataValueField = "id";
        ddlOpportunity.DataBind();

        ddlOpportunity.Items.Insert(0, new ListItem("Select Opportunity", "0"));
    }

    private ExchanceRate parseWebXML(string WebAddress)
    {
        XmlTextReader xmlReader;

        ExchanceRate dsVa = new ExchanceRate();

        DataRow newRowVa = null;

        try
        {
            //Read data from the XML-file over the interNET
            xmlReader = new XmlTextReader(WebAddress);
        }
        catch (WebException)
        {
            throw new WebException("Problem Getting ExchangeRates");
        }

        try
        {
            while (xmlReader.Read())
            {
                if (xmlReader.Name != "")
                {
                    //Check that there are node call gesmes:name
                    //if (xmlReader.Name == "gesmes:name")
                    //{
                    //    _author = xmlReader.ReadString();
                    //}

                    for (int i = 0; i < xmlReader.AttributeCount; i++)
                    {
                        //Check that there are node call Cube
                        if (xmlReader.Name == "Cube")
                        {
                            //Check that there are 1 attribut, then get the date
                            if (xmlReader.AttributeCount == 1)
                            {
                                xmlReader.MoveToAttribute("time");

                                DateTime tim = DateTime.Parse(xmlReader.Value);
                                newRowVa = null;
                                DataRow newRowCo = null;

                                newRowVa = dsVa.Exchance.NewRow();
                                newRowVa["Date"] = tim;
                                dsVa.Exchance.Rows.Add(newRowVa);

                                newRowCo = dsVa.Country.NewRow();
                                newRowCo["Initial"] = "EUR";
                                newRowCo["Name"] = "EUR";		// Find Country name from ISO code
                                newRowCo["Rate"] = 1.0;
                                dsVa.Country.Rows.Add(newRowCo);

                                newRowCo.SetParentRow(newRowVa);	// Make Key to subtable
                            }

                            //If the number of attributs are 2, so get the ExchangeRate-node
                            if (xmlReader.AttributeCount == 2)
                            {
                                xmlReader.MoveToAttribute("currency");
                                string cur = xmlReader.Value;

                                xmlReader.MoveToAttribute("rate");
                                decimal rat = decimal.Parse(xmlReader.Value.Replace(".", ",")); // I am using "," as a decimal symbol

                                DataRow newRowCo = null;

                                newRowCo = dsVa.Country.NewRow();
                                newRowCo["Initial"] = cur;
                                newRowCo["Name"] = cur;
                                newRowCo["Rate"] = rat;
                                dsVa.Country.Rows.Add(newRowCo);

                                newRowCo.SetParentRow(newRowVa);
                            }

                            xmlReader.MoveToNextAttribute();
                        }
                    }
                }
            }
        }
        catch (WebException)
        {
            throw new WebException("connections lost");
        }
        return dsVa;
    }

    private void RebindEmployees()
    {
        int EstimateID = Request.QueryString["uid"] != null ? Convert.ToInt32(Request.QueryString["uid"].ToString()) : 0;

        _objUser.ConnConfig = HttpContext.Current.Session["config"].ToString();

        //DataSet ds = objBL_User.GetSalesPerson(_objUser, new GeneralFunctions().GetSalesAsigned(), EstimateID, 0, "t.SDesc");
        DataSet ds = objBL_User.GetSalesPerson(_objUser, new GeneralFunctions().GetSalesAsigned(), EstimateID, "ESTIMATE", "t.SDesc");

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlEmployees.DataSource = ds.Tables[0];
            ddlEmployees.DataTextField = "SDesc";
            ddlEmployees.DataValueField = "ID";
            ddlEmployees.DataBind();
        }
        ddlEmployees.Items.Insert(0, new ListItem("Select", "0"));
    }

    private void GetOpportunity(int oppId)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.OpportunityID = oppId;
        DataSet ds = new DataSet();
        ds = objBL_Customer.getOpportunityByID(objProp_Customer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtOppName.Text = ds.Tables[0].Rows[0]["fDesc"].ToString();
            if (ds.Tables[0].Rows[0]["closedate"] != DBNull.Value)
            {
                txtBidDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["closedate"]).ToShortDateString();
            }
            SetSelectedValueForDDL(ddlStatus, ds.Tables[0].Rows[0]["status"].ToString());
            if (ddlEmployees.SelectedIndex == 0)
                SetSelectedValueForDDL(ddlEmployees, ds.Tables[0].Rows[0]["AssignedToID"].ToString());
            SetSelectedValueForDDL(ddlOppStage, ds.Tables[0].Rows[0]["OpportunityStageID"].ToString());
            txtCont.Text = ds.Tables[0].Rows[0]["Name"].ToString();
            //txtCont.Text = ds.Tables[0].Rows[0]["LocationName"].ToString();
            txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
            txtPhone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
            txtCellNew.Text = ds.Tables[0].Rows[0]["Cellular"].ToString();
            txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
            txtCompanyName.Text = ds.Tables[0].Rows[0]["CompanyName"].ToString();
            txtContact.Text = ds.Tables[0].Rows[0]["Contact"].ToString();
            //String Contact = Convert.ToString(ds.Tables[0].Rows[0]["Contact"]);
            hdnROLId.Value = ds.Tables[0].Rows[0]["Rol"].ToString();
            Int32 Rol = Convert.ToInt32(ds.Tables[0].Rows[0]["Rol"]);
            var rolType = ds.Tables[0].Rows[0]["RolType"].ToString();
            objProp_Customer.RoleID = Rol;

            #region Bind Contact Name Drop Down
            objProp_Customer.ROL = Rol;
            if (hdProspect.Value == "")
            {
                objProp_Customer.ProspectID = 0;
            }
            else
            {
                objProp_Customer.ProspectID = Convert.ToInt32(hdProspect.Value);
            }

            #endregion

        }
    }

    #endregion

    #region ::Events::

    protected void txtPercntge_TextChanged(object sender, EventArgs e)
    {
        TextBox txtPercntge = sender as TextBox;
        GridViewRow gvr = txtPercntge.NamingContainer as GridViewRow;
        TextBox txtAmt = gvr.FindControl("txtAmt") as TextBox;
        Label lblTotalPrice = gvr.FindControl("lblTotalPrice") as Label;
        //Label lblBudgetExt = gvr.FindControl("lblBudgetExt") as Label;

        TextBox txtBudgetUnit = gvr.FindControl("txtBudgetUnit") as TextBox;
        int totalamt = Convert.ToInt32((Convert.ToDecimal(txtBudgetUnit.Text) * Convert.ToInt32(txtPercntge.Text)) / 100);
        txtAmt.Text = Convert.ToString(totalamt);
        lblTotalPrice.Text = Convert.ToString(Convert.ToDecimal(txtBudgetUnit.Text) + totalamt);
        txtAmt.Enabled = false;

    }

    protected void txtAmt_TextChanged(object sender, EventArgs e)
    {
        TextBox txtAmt = sender as TextBox;
        GridViewRow gvr = txtAmt.NamingContainer as GridViewRow;
        TextBox txtPercntge = gvr.FindControl("txtPercntge") as TextBox;
        Label lblTotalPrice = gvr.FindControl("lblTotalPrice") as Label;

        TextBox txtBudgetUnit = gvr.FindControl("txtBudgetUnit") as TextBox;
        int percentage = Convert.ToInt32((Convert.ToInt32(txtAmt.Text) * 100) / Convert.ToDecimal(txtBudgetUnit.Text));
        txtPercntge.Text = Convert.ToString(percentage);
        lblTotalPrice.Text = Convert.ToString(Convert.ToDecimal(txtBudgetUnit.Text) + Convert.ToInt32(txtAmt.Text));
        txtPercntge.Enabled = false;
    }

    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        JobT objProp_jobT = new JobT();
        objProp_jobT.ConnConfig = Session["config"].ToString();
        objProp_jobT.ID = Convert.ToInt32(ddlTemplate.SelectedValue);

        if ((AddEstimate.EstimatMode)ViewState["edit"] == AddEstimate.EstimatMode.Add)
        {
            ds = objBL_Job.GetEstimateProjectTemplateByID(objProp_jobT);


            if (ds.Tables.Count > 3)
            {
                // Set some default value from Template
                hdnDefOHPer.Value = ds.Tables[0].Rows[0]["OHPer"].ToString();
                hdnDefCOMMSPer.Value = ds.Tables[0].Rows[0]["COMMSPer"].ToString();
                hdnDefMarkupPer.Value = ds.Tables[0].Rows[0]["MARKUPPer"].ToString();
                hdnDefSTaxName.Value = ds.Tables[0].Rows[0]["STaxName"].ToString();

                txtHOPercentAge.Text = ds.Tables[0].Rows[0]["OHPer"].ToString();
                txtPercentAgeCommission.Text = ds.Tables[0].Rows[0]["COMMSPer"].ToString();
                txtMarkupPercentAge.Text = ds.Tables[0].Rows[0]["MARKUPPer"].ToString();
                var staxname = ds.Tables[0].Rows[0]["STaxName"].ToString();
                SetSelectedValueForDDL(drpSaleTax, staxname);
                SetSelectedValueForDDL(ddlEstimateType, ds.Tables[0].Rows[0]["EstimateType"].ToString());
                chkSglBilAmt.Checked = (bool)ds.Tables[0].Rows[0]["IsSglBilAmt"];
                chkBilFrmBOM.Checked = (bool)ds.Tables[0].Rows[0]["IsBilFrmBOM"];

                if (ds.Tables[1].Rows.Count > 0)
                {
                    BindgvBOM(ds.Tables[1]);
                }
                else
                {
                    CreateBOMTable();
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    ds.Tables[2].Columns.Add("EstimateItemId");
                    //ViewState["TempMilestone"] = ds.Tables[2];
                    gvMilestones.DataSource = ds.Tables[2];
                    gvMilestones.DataBind();

                    //#region Count MileStone Total
                    //Int32 milestoneSum = 0;
                    //foreach (DataRow dr in ds.Tables[2].Rows)
                    //{
                    //    milestoneSum = milestoneSum + Convert.ToInt32(dr["Amount"]);
                    //}
                    //txtOverride.Text = Convert.ToString(milestoneSum);
                    //#endregion
                    if (ddlEstimateType.SelectedValue == "bid")
                    {
                        gvMilestones.MasterTableView.GetColumn("Quantity").Display = false;
                        gvMilestones.MasterTableView.GetColumn("Price").Display = false;
                        gvMilestones.MasterTableView.GetColumn("AmountPer").Display = true;

                        txtOverride.Text = hdnFinalBid.Value;
                        txtOverride.Style["display"] = "block";
                        lblFinalBid.Style["display"] = "none";
                    }
                    else
                    {
                        gvMilestones.MasterTableView.GetColumn("Quantity").Display = true;
                        gvMilestones.MasterTableView.GetColumn("Price").Display = true;
                        gvMilestones.MasterTableView.GetColumn("AmountPer").Display = false;
                        txtOverride.Style["display"] = "none";
                        lblFinalBid.Style["display"] = "block";
                    }
                }
                else
                {
                    CreateMilestoneTable();
                }
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "calculateBOM", "CalculateEstimateBOM();CalculateEstimateMilestone();", true);
                if (ds.Tables[3].Rows.Count > 0)
                {
                    txtJobType.Text = ds.Tables[3].Rows[0]["Type"].ToString();
                }
                else
                {
                    txtJobType.Text = string.Empty;
                }
            }
            else
            {
                txtJobType.Text = string.Empty;
            }
        }
        else
        {
            var isReplace = hdnChangeTemplateConfirmStatus.Value == "yes";
            if (isReplace)
            {
                ds = objBL_Job.GetEstimateProjectTemplateByID(objProp_jobT);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    //gvBOM.DataSource = ds.Tables[1];
                    //gvBOM.DataBind();
                    BindgvBOM(ds.Tables[1]);
                }
                else
                {
                    CreateBOMTable();
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    ds.Tables[2].Columns.Add("EstimateItemId");
                    //ViewState["TempMilestone"] = ds.Tables[2];
                    gvMilestones.DataSource = ds.Tables[2];
                    gvMilestones.DataBind();

                    //#region Count MileStone Total
                    //Int32 milestoneSum = 0;
                    //foreach (DataRow dr in ds.Tables[2].Rows)
                    //{
                    //    milestoneSum = milestoneSum + Convert.ToInt32(dr["Amount"]);
                    //}
                    //txtOverride.Text = Convert.ToString(milestoneSum);
                    //#endregion

                }
                else
                {
                    CreateMilestoneTable();
                }

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "calculateBOM", "CalculateEstimateBOM();CalculateEstimateMilestone();", true);

                if (ds.Tables[3].Rows.Count > 0)
                {
                    txtJobType.Text = ds.Tables[3].Rows[0]["Type"].ToString();
                }
                else
                {
                    txtJobType.Text = string.Empty;
                }
            }
            else
            {
                ds = objBL_Job.GetDepartmentByTemplateId(objProp_jobT);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtJobType.Text = ds.Tables[0].Rows[0]["Type"].ToString();
                }
                else
                {
                    txtJobType.Text = string.Empty;
                }
            }
        }
    }

    protected void lnkConvert_Click(object sender, EventArgs e)
    {

        string prospectID = "";
        DataSet ds = new DataSet();


        if (Request.QueryString["uid"] != null)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
            ds = objBL_Customer.GetEstimateByID(objProp_Customer);
            string ffor = ds.Tables[0].Rows[0]["ffor"].ToString();
            var templateId = ds.Tables[0].Rows[0]["Template"].ToString();
            if (!string.IsNullOrEmpty(templateId) && templateId != "0")
            {
                var oppID = ds.Tables[0].Rows[0]["Opportunity"].ToString();
                if (!string.IsNullOrEmpty(oppID) && oppID != "0")
                {
                    if (ffor.Equals("ACCOUNT"))
                    {
                        string jobTypeID = ds.Tables[0].Rows[0]["JobTypeID"].ToString();
                        if (!string.IsNullOrEmpty(jobTypeID) && jobTypeID != "0")
                        {
                            ConvertToProject();
                        }
                        else if (jobTypeID == "0")
                        {
                            //bool isExistProj = ds.Tables[0].Rows[0]["IsExistProj"].ToString() == "1";
                            //if (!isExistProj)
                            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Convert Recurring Contract Wizard.'); window.location.href='addreccontract.aspx?eid=" + Request.QueryString["uid"].ToString() + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) + "';", true);
                            //else
                            //    ConvertToProject();
                            var projectId = ds.Tables[0].Rows[0]["job"].ToString();
                            if (string.IsNullOrEmpty(projectId) || projectId == "0")
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Convert Recurring Contract Wizard.'); window.location.href='addreccontract.aspx?eid=" + Request.QueryString["uid"].ToString() + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) + "';", true);
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyConvertValidation", "noty({text: 'Estimate already converted to project!',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }
                    }
                    else
                    {
                        if (Session["prospectID"] != null)
                        {
                            prospectID = Session["prospectID"].ToString();
                            Session["prospectID"] = null;
                            string alertMess = "The lead needs to first be converted to a customer and/or location, continuing to lead conversion wizard.";
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('" + alertMess + "'); window.location.href='addprospect.aspx?uid=" + prospectID + "&estimateid=" + Request.QueryString["uid"].ToString() + "';", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyConvertValidation", "noty({text: 'Please update your estimate with a opportunity before converting',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyConvertValidation", "noty({text: 'Please update your estimate with a template before converting',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    private void ConvertToProject()
    {
        try
        {
            //Response.Redirect("project.aspx?uid=" + lblID.Text);
            objProp_Customer.ConnConfig = Session["config"].ToString();
            if (Request.QueryString["uid"] != null)
            {
                // objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
            }

            objProp_Customer.Username = Session["Username"].ToString();
            int jobid;
            String retVal = objBL_Customer.ConvertEstimateToProject(objProp_Customer);
            bool isNumeric = int.TryParse(retVal, out jobid);
            if (isNumeric == true)
            {
                #region Calculate Budget
                var estimateBOM = objBL_Customer.GetEstimateBOM(objProp_Customer);
                var estimateMile = objBL_Customer.GetEstimateMilestone(objProp_Customer);
                decimal MatPrice = 0, LabPrice = 0, OtherPrice = 0, Hours = 0, Rev = 0, Profit = 0, ProfitPer = 0, Cost = 0; ;

                foreach (DataRow dr in estimateBOM.Tables[0].Rows)
                {
                    decimal LabPriceNK = 0; decimal LabHoursNK = 0;

                    if (Convert.ToString(dr["BType"]) == "1" || Convert.ToString(dr["BType"]) == "8")// Materials or Inventory
                    {
                        MatPrice = MatPrice + Convert.ToDecimal(dr["BudgetExt"] == DBNull.Value ? "0" : dr["BudgetExt"]);
                        //MatPrice = MatPrice + Convert.ToDecimal(dr["MatPrice"] == DBNull.Value ? "0" : dr["MatPrice"]);
                    }
                    else
                    {
                        OtherPrice = OtherPrice + Convert.ToDecimal(dr["BudgetExt"] == DBNull.Value ? "0" : dr["BudgetExt"]);
                    }

                    if (dr["LabExt"] != DBNull.Value && dr["LabExt"].ToString() != "")
                    {
                        decimal.TryParse(dr["LabExt"].ToString(), out LabPriceNK);
                        LabPrice = LabPrice + LabPriceNK;
                    }
                    //if (dr["LabPrice"] != DBNull.Value && dr["LabPrice"].ToString() != "")
                    //{
                    //    decimal.TryParse(dr["LabPrice"].ToString(), out LabPriceNK);
                    //    LabPrice = LabPrice + LabPriceNK;
                    //}
                    if (dr["LabHours"] != DBNull.Value && dr["LabHours"].ToString() != "")
                    {
                        decimal.TryParse(dr["LabHours"].ToString(), out LabHoursNK);
                        Hours = Hours + LabHoursNK;
                    }
                }

                foreach (DataRow dr in estimateMile.Tables[0].Rows)
                {
                    Rev = Rev + Convert.ToDecimal(dr["Amount"] == DBNull.Value ? "0" : dr["Amount"]);
                }

                Cost = MatPrice + LabPrice + OtherPrice;

                Profit = Rev - Cost;

                if (Rev != 0)
                {
                    ProfitPer = (Profit / Rev) * 100;
                    ProfitPer = Convert.ToDecimal(String.Format("{0:0.00}", ProfitPer));
                }
                else
                {
                    ProfitPer = Convert.ToDecimal(String.Format("{0:0.00}", 0));
                }


                objBL_Customer.UpdateEstimateToProject(Session["config"].ToString(), Convert.ToInt32(retVal), Rev, LabPrice, MatPrice, OtherPrice, Cost, Profit, ProfitPer, Hours);
                #endregion

                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Estimate Converted to Project Successfully! <BR/>Project# " + retVal + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                Session["es_status"] = "c";
                //Response.Redirect("addestimate.aspx?uid=" + objProp_Customer.estimateno);
                if (!string.IsNullOrEmpty(Request.QueryString["redirect"]))
                    Response.Redirect("addestimate.aspx?uid=" + objProp_Customer.estimateno + "&redirect=" + HttpUtility.UrlEncode(Request.QueryString["redirect"]));
                else
                    Response.Redirect("addestimate.aspx?uid=" + objProp_Customer.estimateno);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + retVal + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }


        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkSaveEstimate_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtCustomValueChanged = new DataTable();
            DataTable dtCustom = (DataTable)ViewState["CustomTable"];

            UpdatingCustomDataTableValue(dtCustom);
            var dtCustom1 = new DataTable();
            if (dtCustom != null && dtCustom.Rows.Count > 0)
            {
                dtCustom1 = dtCustom.Copy();
                dtCustom1.Columns.Remove("ControlID");
                dtCustom1.Columns.Remove("IsValueChanged");
                dtCustom1.Columns.Remove("OldValue");
                dtCustom1.Columns.Remove("FieldControl");

                var temp = dtCustom.AsEnumerable().Where(m => m.Field<bool?>("IsValueChanged") != null && m.Field<bool?>("IsValueChanged") == true);

                if (temp.Count() > 0)
                {
                    dtCustomValueChanged = temp.CopyToDataTable();
                }
            }
            else
            {
                dtCustom1 = dtCustom.Clone();
                dtCustom1.Columns.Remove("ControlID");
                dtCustom1.Columns.Remove("IsValueChanged");
                dtCustom1.Columns.Remove("OldValue");
                dtCustom1.Columns.Remove("FieldControl");
            }
            objProp_Customer.DtCustom = dtCustom1;

            objProp_Customer.ConnConfig = Session["config"].ToString();
            //   objProp_Customer.Name = txtName.Text.Trim();
            objProp_Customer.Username = Session["Username"].ToString();
            objProp_Customer.Description = txtREPdesc.Text.Trim();
            objProp_Customer.Remarks = txtREPremarks.Text.Trim();
            //MAIN CONTACT INFO
            if (!string.IsNullOrEmpty(hdnROLId.Value))
            {
                objProp_Customer.ROL = Convert.ToInt32(hdnROLId.Value);
            }

            //     objProp_Customer.Address = txtContactAddress.Text;
            //END MAIN CONTACT INFO
            if (!String.IsNullOrEmpty(hdnLocID.Value))
            {
                objProp_Customer.LocID = Convert.ToInt32(hdnLocID.Value);
            }
            if (!string.IsNullOrEmpty(hdnExchangeRate.Value))
            {
                objProp_Customer.CADExchange = Convert.ToDouble(hdnExchangeRate.Value);
            }
            //  objProp_Customer.IsItemEdited = Convert.ToInt16(gvTemplateItems.Enabled);
            objProp_Customer.Status = Convert.ToInt16(ddlStatus.SelectedValue);
            objProp_Customer.date = Convert.ToDateTime(TxtDate.Text);
            objProp_Customer.estimateno = Request.QueryString["uid"] != null ? Convert.ToInt32(Request.QueryString["uid"].ToString()) : 0;// string.IsNullOrEmpty(TxtEstimateNo.Text) ? 0 : Convert.ToInt32(TxtEstimateNo.Text);
            objProp_Customer.Type = ddlTemplate.SelectedValue;
            //phone or contact
            //objProp_Customer.Contact = ddlContact.SelectedValue != "" ? ddlContact.SelectedValue : "0";
            objProp_Customer.Contact = txtContact.Text;
            objProp_Customer.Phone = txtPhone.Text;
            objProp_Customer.Fax = txtFax.Text;
            objProp_Customer.Billaddress = txtCont.Text;
            objProp_Customer.Email = txtEmail.Text;
            objProp_Customer.Cellular = txtCellNew.Text;
            objProp_Customer.CompanyName = txtCompanyName.Text;
            objProp_Customer.Comment = txtComment.Text;

            objProp_Customer.Discounted = chkDiscounted.Checked;
            if (objProp_Customer.Discounted)
            {
                objProp_Customer.DiscountedNotes = txtDiscountedNotes.Text;
            }
            else
            {
                objProp_Customer.DiscountedNotes = string.Empty;
            }


            if (drpCategory.SelectedValue == "Other")
            {
                objProp_Customer.Category = txtCategory.Text;
            }
            else
            {
                objProp_Customer.Category = drpCategory.SelectedValue;
            }
            //     objProp_Customer.Billaddress = txtBillAddress.Text;
            //end
            objProp_Customer.Worker = Convert.ToInt32(ddlEmployees.SelectedValue);
            objProp_Customer.EstimateType = ddlEstimateType.SelectedValue;
            objProp_Customer.IsSglBilAmt = chkSglBilAmt.Checked;
            objProp_Customer.IsBilFrmBOM = chkBilFrmBOM.Checked;
            objProp_Customer.TimeDue = Convert.ToDateTime(txtBidDate.Text);//Bid date 

            //DataTable dtBomEstimateItems = GetBOMItems();

            DataTable dtBomEstimateItems = GetBOMGridItems();

            dtBomEstimateItems.Columns.Remove("MatName");
            dtBomEstimateItems.Columns.Remove("BTypeName");
            dtBomEstimateItems.Columns.Remove("LabItemName");
            //dtBomEstimateItems.AcceptChanges();

            DataTable dtM = GetMilestoneItems();
            //dtM.Columns.Remove("Department");

            int bline = 1;
            int mline = 1;
            //int borderno = 1;
            //int morderno = 1;
            if (ViewState["Line"] == null)
            {
                dtBomEstimateItems.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = bline++);
                dtBomEstimateItems.AcceptChanges();

                dtM.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtM.AcceptChanges();

            }
            else
            {
                bline = (Int16)ViewState["Line"];
                bline++;
                dtBomEstimateItems.Select("Line = 0")
                            .AsEnumerable().ToList()
                            .ForEach(t => t["Line"] = bline++);
                dtBomEstimateItems.AcceptChanges();

                mline = (Int16)ViewState["Line"];
                mline++;
                dtM.Select("Line = 0")
                    .AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtM.AcceptChanges();

            }
            for (int i = 0; i < dtBomEstimateItems.Rows.Count; i++)
            {
                dtBomEstimateItems.Rows[i]["OrderNo"] = i + 1;
            }
            for (int i = 0; i < dtM.Rows.Count; i++)
            {
                dtM.Rows[i]["OrderNo"] = i + 1;
            }

            //objProp_Customer.BidPrice = ConvertCurrentCurrencyFormatToDbl(lblBidPrice.Text);
            objProp_Customer.BidPrice = ConvertCurrentCurrencyFormatToDbl(hdnBidPrice.Value);
            if (string.IsNullOrEmpty(txtOverride.Text))
            {
                //objProp_Customer.Override = objProp_Customer.BidPrice;
                objProp_Customer.Override = null;
            }
            else
            {
                objProp_Customer.Override = ConvertCurrentCurrencyFormatToDbl(txtOverride.Text);
            }

            objProp_Customer.Cont = ConvertCurrentCurrencyFormatToDbl(txtContingencies.Text);
            objProp_Customer.ContPer = ConvertCurrentCurrencyFormatToDbl(txtPerContingencies.Text);
            objProp_Customer.OH = ConvertCurrentCurrencyFormatToDbl(txtOH.Text);
            objProp_Customer.OHPer = ConvertCurrentCurrencyFormatToDbl(txtHOPercentAge.Text);
            objProp_Customer.MarkupPer = ConvertCurrentCurrencyFormatToDbl(txtMarkupPercentAge.Text);
            //objProp_Customer.MarkupPer = ConvertCurrentCurrencyFormatToDbl(hd_markupper.Value);
            objProp_Customer.CommissionPer = ConvertCurrentCurrencyFormatToDbl(txtPercentAgeCommission.Text);
            objProp_Customer.CommissionVal = ConvertCurrentCurrencyFormatToDbl(txtCommission.Text);

            dtM.Columns.Remove("Department");
            objProp_Customer.DtMilestone = dtM;
            objProp_Customer.DtBOM = dtBomEstimateItems;

            objProp_Customer.DtMilestone.Columns.Remove("CodeDesc");
            objProp_Customer.DtBOM.Columns.Remove("CodeDesc");

            objProp_Customer.TemplateID = ddlTemplate.SelectedValue != "" ? Convert.ToInt32(ddlTemplate.SelectedValue) : 0;

            objProp_Customer.STax = drpSaleTax.SelectedItem.Text;

            if (drpSaleTax.SelectedItem.Text == "Select Sales Tax")
            {
                objProp_Customer.STaxRate = 0.00;
            }
            else
            {
                objProp_Customer.STaxRate = double.Parse(drpSaleTax.SelectedItem.Text.Split('/').LastOrDefault());
            }

            objProp_Customer.DtEquips = GetSelectedEquipmentsFromUI();
            objProp_Customer.GroupId = Convert.ToInt32(ddlEstimateGroup.SelectedValue);

            #region Sales Tax
            if (drpSaleTax.SelectedItem.Text != "Select Sales Tax")
            {
                String _query = Convert.ToString(drpSaleTax.SelectedItem.Text);
                String[] staxSubStrings = _query.Split('/');
                String _staxSelectedValue = "";
                for (int i = 0; i < staxSubStrings.Count() - 1; i++)
                {
                    _staxSelectedValue = _staxSelectedValue + staxSubStrings[i] + "/";
                }
                _staxSelectedValue = _staxSelectedValue.Trim();
                _staxSelectedValue = _staxSelectedValue.Substring(0, _staxSelectedValue.Length - 1);
                objProp_Customer.Sales_Tax = _staxSelectedValue.Trim();
            }
            #endregion

            #region Extra Tags Data
            if (!String.IsNullOrEmpty(hd_markup.Value) && (hd_markup.Value !=""))
            {
                objProp_Customer.MarkupVal = double.Parse(hd_markup.Value);
            }
            else
            {
                objProp_Customer.MarkupVal = 0;
            }
            if (!String.IsNullOrEmpty(hd_salestax.Value) && (hd_salestax.Value != ""))
            {
                objProp_Customer.STaxVal = double.Parse(hd_salestax.Value);
            }
            else
            {
                objProp_Customer.STaxVal = 0;
            }
            if (!String.IsNullOrEmpty(hd_materialexp.Value) && (hd_materialexp.Value != ""))
            {
                objProp_Customer.MatExp = double.Parse(hd_materialexp.Value);
            }
            else
            {
                objProp_Customer.MatExp = 0;
            }
            if (!String.IsNullOrEmpty(hd_laborexp.Value) && (hd_laborexp.Value != ""))
            {
                objProp_Customer.LabExp = double.Parse(hd_laborexp.Value);
            }
            else
            {
                objProp_Customer.LabExp = 0;
            }
            if (!String.IsNullOrEmpty(hd_otherexp.Value) && (hd_otherexp.Value != ""))
            {
                objProp_Customer.OtherExp = double.Parse(hd_otherexp.Value);
            }
            else
            {
                objProp_Customer.OtherExp = 0;
            }
            if (!String.IsNullOrEmpty(hd_subtotal.Value) && (hd_subtotal.Value != ""))
            {
                objProp_Customer.SubToalVal = double.Parse(hd_subtotal.Value);
            }
            else
            {
                objProp_Customer.SubToalVal = 0;
            }
            if (!String.IsNullOrEmpty(hd_totalcost.Value) && (hd_totalcost.Value != ""))
            {
                objProp_Customer.TotalCostVal = double.Parse(hd_totalcost.Value);
            }
            else
            {
                objProp_Customer.TotalCostVal = 0;
            }
            if (!String.IsNullOrEmpty(hd_pretax.Value) && (hd_pretax.Value != ""))
            {
                objProp_Customer.PretaxTotalVal = double.Parse(hd_pretax.Value);
            }
            else
            {
                objProp_Customer.PretaxTotalVal = 0;
            }

            
            #endregion

            #region Grid user settings
            DataTable dt = new DataTable();
            dt.Columns.Add("UserId", typeof(int));
            dt.Columns.Add("PageName", typeof(string));
            dt.Columns.Add("GridId", typeof(string));
            dt.Columns.Add("ColumnSettings", typeof(string));

            //var userId = 1;
            var pageName = "addestimate.aspx";
            var gridId = "gvBOM";
            var columnSettings = string.Empty;

            List<ColumnSettings> lstColSetts = new List<ColumnSettings>();
            foreach (GridColumn column in gvBOM.MasterTableView.OwnerGrid.Columns)
            {
                var colSett = new ColumnSettings();
                colSett.Name = column.UniqueName;
                colSett.Display = column.Display;
                colSett.Width = 0;
                lstColSetts.Add(colSett);
            }

            columnSettings = Newtonsoft.Json.JsonConvert.SerializeObject(lstColSetts);

            DataRow dr = dt.NewRow();
            dr["UserId"] = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            dr["PageName"] = pageName;
            dr["GridId"] = gridId;
            dr["ColumnSettings"] = columnSettings;
            dt.Rows.Add(dr);

            objProp_Customer.DtGridUserSettings = dt;

            #endregion

            #region Bill
            if (!string.IsNullOrEmpty(txtBillRate.Text))
            {
                objProp_Customer.BillRate = Convert.ToDouble(txtBillRate.Text);
            }
            else
            {
                objProp_Customer.BillRate = Convert.ToDouble("0.00");
            }

            if (!string.IsNullOrEmpty(txtOTRate.Text))
            {
                objProp_Customer.RateOT = Convert.ToDouble(txtOTRate.Text);
            }
            else
            {
                objProp_Customer.RateOT = Convert.ToDouble("0.00");
            }

            if (!string.IsNullOrEmpty(txtRateNT.Text))
            {
                objProp_Customer.RateNT = Convert.ToDouble(txtRateNT.Text);
            }
            else
            {
                objProp_Customer.RateNT = Convert.ToDouble("0.00");
            }

            if (!string.IsNullOrEmpty(txtDTRate.Text))
            {
                objProp_Customer.RateDT = Convert.ToDouble(txtDTRate.Text);
            }
            else
            {
                objProp_Customer.RateDT = Convert.ToDouble("0.00");
            }

            if (!string.IsNullOrEmpty(txtTravelRate.Text))
            {
                objProp_Customer.RateTravel = Convert.ToDouble(txtTravelRate.Text);
            }
            else
            {
                objProp_Customer.RateTravel = Convert.ToDouble("0.00");
            }

            if (!string.IsNullOrEmpty(txtMileageRate.Text))
            {
                objProp_Customer.Mileage = Convert.ToDouble(txtMileageRate.Text);
            }
            else
            {
                objProp_Customer.Mileage = Convert.ToDouble("0.00");
            }

            if (!string.IsNullOrEmpty(txtAmount.Text))
            {
                objProp_Customer.Amount = Convert.ToDouble(txtAmount.Text);
            }
            else
            {
                objProp_Customer.Amount = 0.00;
            }

            objProp_Customer.PType = Convert.ToInt16(ddlPType.SelectedValue);
            #endregion

            objProp_Customer.OpportunityStageID = ddlOppStage.SelectedValue;
            objProp_Customer.OpportunityName = txtOppName.Text;
            objProp_Customer.IsCertifiedProject = chkPayCertified.Checked;

            if (txtSoldDate.Text != "")
            {
                objProp_Customer.SoldDate = Convert.ToDateTime(txtSoldDate.Text);
            }
            else if (ddlStatus.SelectedValue == "5")
            {
                txtSoldDate.Text = DateTime.Now.ToShortDateString();
                objProp_Customer.SoldDate = DateTime.Now;
            }

            if ((AddEstimate.EstimatMode)ViewState["edit"] != AddEstimate.EstimatMode.Edit)
            {
                //dt.Columns.RemoveAt(dt.Columns.Count - 2);
                objProp_Customer.Mode = 0;

                if (!string.IsNullOrEmpty(hdnOpportunity.Value) && hdnOpportunity.Value != "0")
                {
                    objProp_Customer.OpportunityID = int.Parse(hdnOpportunity.Value);
                }
                else
                {
                    objProp_Customer.OpportunityID = 0;
                }

                objProp_Customer.estimateno = objBL_Customer.AddEstimate(objProp_Customer);

                //Update  Attachment Doc INFO                 
                UpdateTempDateWhenCreatingNewEstimate(objProp_Customer.estimateno);
                UpdateDocInfo();
                CustomChangedAlert_Task(dtCustomValueChanged, objProp_Customer.estimateno);

                if (!string.IsNullOrEmpty(hdStatus.Value) && !hdStatus.Value.Equals("0"))
                {
                    Session["prospectID"] = hdProspect.Value;
                }

                IntializeControls();
                Session["es_status"] = "a";
                //if (Request.QueryString["opp"] != null)
                //    Response.Redirect("addestimate.aspx?uid=" + objProp_Customer.estimateno + "&opp=" + Request.QueryString["opp"]);
                //else
                //    Response.Redirect("addestimate.aspx?uid=" + objProp_Customer.estimateno);
                if (!string.IsNullOrEmpty(Request.QueryString["redirect"]))
                    Response.Redirect("addestimate.aspx?uid=" + objProp_Customer.estimateno + "&redirect=" + HttpUtility.UrlEncode(Request.QueryString["redirect"]));
                else
                    Response.Redirect("addestimate.aspx?uid=" + objProp_Customer.estimateno);
            }
            else if ((AddEstimate.EstimatMode)ViewState["edit"] == AddEstimate.EstimatMode.Edit)
            {
                objProp_Customer.Mode = 1;

                // TODO: Need to review that should we update Opportunity when updating the related Estimate
                #region Update Opportunity Data
                if (ddlOpportunity.SelectedValue != "0")
                {

                    objProp_Customer.OpportunityID = Convert.ToInt32(ddlOpportunity.SelectedValue);
                }
                else
                {
                    objProp_Customer.OpportunityID = 0;

                }

                #endregion

                objBL_Customer.UpdateEstimate(objProp_Customer);
                //Update  Attachment Doc INFO

                UpdateDocInfo();

                CustomChangedAlert_Task(dtCustomValueChanged, objProp_Customer.estimateno);
                //UpdateTodoTasksNumberMasterpage();
                Session["es_status"] = "u";
                Response.Redirect(Page.Request.RawUrl, false);

            }

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter', theme : 'noty_theme_default',  closable : false});", true);
        }
    }


    protected void lnkCloseEstimate_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["redirect"]))
        {
            Response.Redirect(Request.QueryString["redirect"]);
        }
        else
        {
            if (Request.QueryString["opp"] != null)
            {
                Response.Redirect("addopprt.aspx?uid=" + Request.QueryString["opp"]);
            }
            else if (Request.QueryString["page"] == "opp")
            {
                Response.Redirect("opportunity.aspx");
            }
            else
            {
                Response.Redirect("estimate.aspx");
            }
        }
    }

    #endregion



    protected void ibDeleteMilestone_Click(object sender, EventArgs e)
    {
        List<int> listItemDelete = new List<int>();
        try
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "removeLine('" + gvMilestones.ClientID + "')", true);
            foreach (GridDataItem gr in gvMilestones.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked.Equals(true))
                {
                    Label lblLine = gr.FindControl("lblLine") as Label;

                    listItemDelete.Add(Convert.ToInt32(lblLine.Text));
                    DeleteGridItem(listItemDelete, "Milestones");
                }
            }

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DeleteGridItem(List<int> listItemDelete, string gridName)
    {
        DataTable dt = new DataTable();
        if (gridName == "Bom")
        {
            dt = GetBOMGridItems();
        }
        else if (gridName == "Milestones")
        {
            dt = GetMilestoneGridItems();
        }
        List<DataRow> rowsToDelete = new List<DataRow>();

        foreach (DataRow row in dt.Rows)
        {
            if (listItemDelete.Contains(Int32.Parse(row["Line"].ToString())))
            {
                rowsToDelete.Add(row);
            }
        }
        foreach (DataRow row in rowsToDelete)
        {
            dt.Rows.Remove(row);
        }
        if (gridName == "Bom")
        {
            //gvBOM.DataSource = dt;
            //gvBOM.DataBind();
            BindgvBOM(dt);
        }
        if (gridName == "Milestones")
        {
            gvMilestones.DataSource = dt;
            gvMilestones.DataBind();
        }
    }
    #region BOM
    private DataSet JSONToDatatableGridItems(string strItems, DataTable dtnew, DataTable dtLaborItemsNew, int i, bool includeLaborItems)
    {
        DataTable dt = dtnew.Copy();
        DataTable dtLaborItems = dtLaborItemsNew.Copy();

        DataSet ds = new DataSet();
        //string strItems = hdnItemJSON.Value.Trim();
        if (strItems != string.Empty)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            //DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);
            DataSet dsLabor = (DataSet)ViewState["labor"];

            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            //List<EstimateItemData> objEstimateItemData = new List<EstimateItemData>();
            //objEstimateItemData = sr.Deserialize<List<EstimateItemData>>(strItems);
            //int i = 0;
            //foreach (EstimateItemData eid in objEstimateItemData)
            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                if (dict["txtScope"].ToString().Trim() != string.Empty)// || dict["txtUnitCost"].ToString().Trim() == string.Empty|| dict["txtQuan"].ToString().Trim() == string.Empty ||  dict["txtTotal"].ToString().Trim() == string.Empty
                {
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Estimate"] = 0;
                    dr["Line"] = i;
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();

                    if (dict["txtQuan"].ToString().Trim() != string.Empty)
                        dr["Quan"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtQuan"].ToString(), "0"));

                    if (dict["txtUnitCost"].ToString().Trim() != string.Empty)
                        dr["Cost"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtUnitCost"].ToString(), "0"));

                    if (dict["txtTotal"].ToString().Trim() != string.Empty)
                        dr["Price"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

                    dr["Hours"] = 0;
                    dr["Rate"] = 0;
                    dr["Labor"] = 0;

                    if (dict["txtTotal"].ToString().Trim() != string.Empty)
                        dr["Amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

                    dr["STax"] = 0;
                    dr["code"] = string.Empty;
                    dr["Vendor"] = dict["txtVendor"].ToString().Trim();
                    dr["Currency"] = dict["ddlCurrency"].ToString().Trim();
                    dr["code"] = dict["txtCode"];
                    dr["CodeDesc"] = dict["lblCodeDesc"].ToString().Trim();
                    if (dict["ddlMeasure"].ToString().Trim() != string.Empty)
                        dr["measure"] = Convert.ToInt32(dict["ddlMeasure"]);

                    dt.Rows.Add(dr);

                    if (includeLaborItems)
                    {
                        foreach (DataRow drlab in dsLabor.Tables[0].Rows)
                        {
                            if (dict["txt" + drlab["Item"]].ToString().Trim() != string.Empty)
                            {
                                DataRow drLaborItems = dtLaborItems.NewRow();
                                drLaborItems["Line"] = i;
                                drLaborItems["LabourID"] = drlab["ID"];
                                drLaborItems["Amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txt" + drlab["Item"]].ToString(), "0"));
                                dtLaborItems.Rows.Add(drLaborItems);
                            }
                        }
                    }
                }
            }
        }

        ds.Tables.Add(dt);
        ds.Tables.Add(dtLaborItems);

        return ds;
    }

    private void CreateBOMTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItemID", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("BType", typeof(int));
        dt.Columns.Add("QtyReq", typeof(double));
        dt.Columns.Add("UM", typeof(string));
        dt.Columns.Add("BudgetUnit", typeof(double));
        dt.Columns.Add("BudgetExt", typeof(double));    // JobTItem.Budget
        dt.Columns.Add("MatItem", typeof(int));         // BOM.MatItem

        dt.Columns.Add("MatMod", typeof(double));       // JobTItem.Modifier
        dt.Columns.Add("MatPrice", typeof(double));     //new column
        dt.Columns.Add("MatMarkup", typeof(double));
        dt.Columns.Add("STax", typeof(byte));
        dt.Columns.Add("Currency", typeof(string));
        dt.Columns.Add("LabItem", typeof(int));         // BOM.LabItem
        dt.Columns.Add("LabMod", typeof(double));       // JobTItem.ETCMod
        dt.Columns.Add("LabExt", typeof(double));       // JobTItem.ETC
        dt.Columns.Add("LabRate", typeof(double));      // BOM.LabRate
        dt.Columns.Add("LabHours", typeof(double));        //JobTItem.BHours
        dt.Columns.Add("SDate", typeof(DateTime));      // BOM.SDate
        dt.Columns.Add("VendorId", typeof(int));
        dt.Columns.Add("Vendor", typeof(string));
        dt.Columns.Add("TotalExt", typeof(double));
        dt.Columns.Add("LabPrice", typeof(double));     // new column
        dt.Columns.Add("LabMarkup", typeof(double));
        dt.Columns.Add("LStax", typeof(byte));
        dt.Columns.Add("EstimateItemID", typeof(int));
        dt.Columns.Add("OrderNo", typeof(int));

        dt.Columns.Add("MatName", typeof(string));         // BOM.MatName
        dt.Columns.Add("BTypeName", typeof(string));         // BOM.MatName
        dt.Columns.Add("LabItemName", typeof(string));         // BOM.MatName
        dt.Columns.Add("ChangeOrder", typeof(int)); // BOM.ChangeOrder
        BindgvBOM(dt);
    }

    private void FillBomType()
    {
        try
        {
            if (ViewState["dtBomType"] == null)
            {
                DataSet ds = new DataSet();
                _objJob.ConnConfig = Session["config"].ToString();
                ds = objBL_Job.GetBomType(_objJob);
                ViewState["dtBomType"] = ds.Tables[0];
                dtBomType = ds.Tables[0];
            }
            else
            {
                dtBomType = (DataTable)ViewState["dtBomType"];
            }

            dtCurrency = new DataTable();
            dtCurrency.Columns.Add("ID");
            dtCurrency.Columns.Add("Name");


            dtCurrency.Rows.Add("0", "Currency");
            dtCurrency.Rows.Add("1", "US");
            dtCurrency.Rows.Add("2", "CDN");
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillWage()
    {
        try
        {
            if (ViewState["dtLab"] == null)
            {
                _objUser.ConnConfig = Session["config"].ToString();
                DataSet dsWage = objBL_User.getWage(_objUser);

                DataRow dr = dsWage.Tables[0].NewRow();
                dr["LabItem"] = 0;
                dr["LabDesc"] = "Select Labor";
                dsWage.Tables[0].Rows.InsertAt(dr, 0);
                ViewState["dtLab"] = dsWage.Tables[0];
                dtLab = dsWage.Tables[0];
            }
            else
            {
                dtLab = (DataTable)ViewState["dtLab"];
            }

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    #region Milestone
    private void CreateMilestoneTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquistDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("EstimateItemId", typeof(int));
        dt.Columns.Add("AmountPer", typeof(string));
        dt.Columns.Add("OrderNo", typeof(double));
        dt.Columns.Add("Quantity", typeof(double));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("ChangeOrder", typeof(int));

        //DataTable dt = CreateMilestoneDataTable();
        ViewState["MProjectTemplate"] = dt;
        gvMilestones.DataSource = dt;
        gvMilestones.DataBind();
    }

    private DataTable CreateMilestoneDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("EstimateItemId", typeof(int));
        dt.Columns.Add("AmountPer", typeof(string));
        dt.Columns.Add("OrderNo", typeof(double));
        dt.Columns.Add("Quantity", typeof(double));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("ChangeOrder", typeof(int));

        return dt;
    }

    private DataTable GetMilestoneItems()
    {
        DataTable dt = CreateMilestoneDataTable();

        //try
        //{
        string strItems = hdnMilestone.Value.Trim();

        if (strItems != string.Empty)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            int i = 0;

            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                if (dict["txtScope"].ToString().Trim() != string.Empty)
                {

                    i++;
                    DataRow dr = dt.NewRow();

                    if (dict["hdnLine"].ToString().Trim() != string.Empty)
                    {
                        dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                    }
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();
                    dr["jcode"] = dict["txtCode"].ToString().Trim();
                    dr["CodeDesc"] = dict["lblCodeDesc"].ToString().Trim();
                    dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

                    dr["MilesName"] = dict["txtName"].ToString().Trim();

                    if (dict["txtRequiredBy"].ToString() != string.Empty)
                    {
                        dr["RequiredBy"] = Convert.ToDateTime(dict["txtRequiredBy"]);
                    }
                    if (dict["txtAmount"].ToString() != string.Empty && dict["txtAmount"].ToString().Trim() != "0.00")
                    {
                        dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                    }
                    else
                    {
                        dr["Amount"] = 0;
                    }


                    //dr["LeadTime"] = dict["txtLeadTime"].ToString();
                    if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                    {
                        dr["Type"] = dict["hdnType"].ToString();
                        dr["Department"] = dict["txtSType"].ToString();
                    }
                    if (dict["hdnEstimateItemId"] != null)
                    {
                        if (dict["hdnEstimateItemId"].ToString().Trim() != string.Empty)
                        {
                            dr["EstimateItemId"] = Convert.ToInt32(dict["hdnEstimateItemId"]);
                        }
                        else
                        {
                            dr["EstimateItemId"] = 0;
                        }
                    }

                    if (dict["txtPerAmount"].ToString() != string.Empty)
                    {
                        dr["AmountPer"] = Convert.ToString(dict["txtPerAmount"]);
                    }
                    else
                    {
                        dr["AmountPer"] = "";
                    }
                    //if (!string.IsNullOrEmpty(dict["txtActAcquiDate"].ToString()))
                    //{
                    //    dr["ActAcquDate"] = dict["txtActAcquiDate"].ToString();
                    //}
                    //dr["Comments"] = dict["txtComments"].ToString();
                    if (dict["hdnOrderNoMil"].ToString().Trim() != string.Empty)
                    {
                        dr["OrderNo"] = Convert.ToInt32(dict["hdnOrderNoMil"].ToString());
                    }

                    if (dict["txtQuantity"].ToString() != string.Empty)
                    {
                        dr["Quantity"] = Convert.ToDouble(dict["txtQuantity"]);
                    }
                    else
                    {
                        dr["Quantity"] = 0;
                    }

                    if (dict["txtPrice"].ToString() != string.Empty)
                    {
                        dr["Price"] = Convert.ToDouble(dict["txtPrice"]);
                    }
                    else
                    {
                        dr["Price"] = 0;
                    }
                    if (dict["hdnChangeOrderChk"].ToString().Trim() != string.Empty)
                    {
                        if (dict["hdnChangeOrderChk"].ToString().ToLower() == "true")
                        {
                            dr["ChangeOrder"] = 1;
                        }
                        else
                        {
                            dr["ChangeOrder"] = 0;
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
        }

        return dt;
    }

    private DataTable GetMilestoneGridItems()       //get all items in milestone grid
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));

        dt.Columns.Add("EstimateItemId", typeof(int));
        dt.Columns.Add("AmountPer", typeof(string));
        dt.Columns.Add("OrderNo", typeof(double));
        dt.Columns.Add("Quantity", typeof(double));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("ChangeOrder", typeof(int));
        //try
        //{
        string strItems = hdnMilestone.Value.Trim();

        if (strItems != string.Empty)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            int i = 0;

            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                if (dict["hdnLine"].ToString().Trim() == string.Empty)
                {
                    return dt;
                }
                i++;
                DataRow dr = dt.NewRow();
                if (dict["hdnLine"].ToString().Trim() != string.Empty)
                {
                    dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                }
                dr["fDesc"] = dict["txtScope"].ToString().Trim();
                dr["jcode"] = dict["txtCode"].ToString().Trim();
                dr["CodeDesc"] = dict["lblCodeDesc"].ToString().Trim();
                dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

                dr["MilesName"] = dict["txtName"].ToString().Trim();

                if (dict["txtRequiredBy"].ToString() != string.Empty)
                {
                    dr["RequiredBy"] = Convert.ToDateTime(dict["txtRequiredBy"]);
                }
                if (dict["txtAmount"].ToString() != string.Empty && dict["txtAmount"].ToString().Trim() != "0.00")
                {
                    dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                }
                else
                {
                    dr["Amount"] = 0;
                }
                if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                {
                    dr["Type"] = dict["hdnType"].ToString();
                    dr["Department"] = dict["txtSType"].ToString();
                }
                if (dict["hdnEstimateItemId"] != null)
                {
                    if (dict["hdnEstimateItemId"].ToString().Trim() != string.Empty)
                    {
                        dr["EstimateItemId"] = Convert.ToInt32(dict["hdnEstimateItemId"]);
                    }
                    else
                    {
                        dr["EstimateItemId"] = 0;
                    }

                }

                if (dict["txtPerAmount"].ToString() != string.Empty)
                {
                    dr["AmountPer"] = Convert.ToString(dict["txtPerAmount"]);
                }
                else
                {
                    dr["AmountPer"] = "";
                }
                if (dict["hdnOrderNoMil"].ToString().Trim() != string.Empty)
                {
                    dr["OrderNo"] = Convert.ToInt32(dict["hdnOrderNoMil"].ToString());
                }
                if (dict["txtQuantity"].ToString() != string.Empty)
                {
                    dr["Quantity"] = Convert.ToDouble(dict["txtQuantity"]);
                }
                else
                {
                    dr["Quantity"] = 0;
                }

                if (dict["txtPrice"].ToString() != string.Empty)
                {
                    dr["Price"] = Convert.ToDouble(dict["txtPrice"]);
                }
                else
                {
                    dr["Price"] = 0;
                }
                if (dict["hdnChangeOrderChk"].ToString().Trim() != string.Empty)
                {
                    if (dict["hdnChangeOrderChk"].ToString().ToLower() == "true")
                    {
                        dr["ChangeOrder"] = 1;
                    }
                    else
                    {
                        dr["ChangeOrder"] = 0;
                    }
                }
                dt.Rows.Add(dr);
            }
        }
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        return dt;
    }

    #endregion
    private void SetExchange()
    {

        if (Session["ExchangeRate"] == null)
        {
            //webservicex_Currency.CurrencyConvertor crn = new webservicex_Currency.CurrencyConvertor();
            //double rate = crn.ConversionRate(webservicex_Currency.Currency.CAD, webservicex_Currency.Currency.USD);

            ExchanceRate dsV = new ExchanceRate();
            dsV = parseWebXML(@"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
            double USD = 0;
            double CAD = 0;
            foreach (DataRow dr in dsV.Tables[1].Rows)
            {
                if (dr["name"].ToString().Trim().ToUpper() == "USD")
                {
                    USD = Convert.ToDouble(dr["rate"]);
                }
                if (dr["name"].ToString().Trim().ToUpper() == "CAD")
                {
                    CAD = Convert.ToDouble(dr["rate"]);
                }
            }

            double rate = 0;
            rate = (1 / CAD) * USD;
            rate = Math.Round(rate, 2);

            Session["ExchangeRate"] = rate.ToString();
        }
        hdnExchangeRate.Value = Session["ExchangeRate"].ToString();
    }

    protected void ddlSTax_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //_objUser.Stax = ddlSTax.SelectedValue;
            //_objUser.ConnConfig = Session["config"].ToString();
            //DataSet ds = objBL_User.GetSTaxByName(_objUser);
            //hdnStax.Value = ds.Tables[0].Rows[0]["Rate"].ToString();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Add Attachment
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {

            string filename = string.Empty;
            string fullpath = string.Empty;
            string mime = string.Empty;
            var savepath = string.Empty;
           
            var mainDirectory = string.Empty;

            if (Request.QueryString["uid"] != null)
            {
                mainDirectory = Request.QueryString["uid"].ToString();
            }
            else
            {

                if (ViewState["TempUploadDirectory"] == null)
                {
                    ViewState["TempUploadDirectory"] = Guid.NewGuid().ToString("N");
                }

                mainDirectory = ViewState["TempUploadDirectory"] as string;
            }

            savepath = GetUploadDirectory(mainDirectory);


            //if (FileUpload1.HasFile)
            //if (!string.IsNullOrEmpty(FileUpload1.FileName))
            //{
            if (Request.QueryString["uid"] != null)
            {
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objMapData.TempId = "0";

                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {
                    filename = postedFile.FileName;
                    fullpath = savepath + filename;
                    mime = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                    if (File.Exists(fullpath))
                    {
                        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                        filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                        fullpath = savepath + filename;
                    }

                    using (new NetworkConnection())
                    {
                        if (!Directory.Exists(savepath))
                        {
                            Directory.CreateDirectory(savepath);
                        }

                        postedFile.SaveAs(fullpath);
                    }

                    objMapData.Screen = "Estimate";
                    objMapData.FileName = filename;
                    objMapData.DocTypeMIME = mime;
                    objMapData.FilePath = fullpath;
                    objMapData.DocID = 0;
                    objMapData.Mode = 0;
                    objMapData.ConnConfig = Session["config"].ToString();
                    objMapData.Worker = Session["User"].ToString();
                    objBL_MapData.AddFile(objMapData);
                }

                UpdateDocInfo();
                RadGrid_Documents.Rebind();
                //GetDocuments();
            }
            else
            {
                var tempTable = new DataTable();
                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {
                    filename = postedFile.FileName;
                    fullpath = savepath + filename;
                    mime = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                    if (File.Exists(fullpath))
                    {
                        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                        filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                        fullpath = savepath + filename;
                    }

                    using (new NetworkConnection())
                    {
                        if (!Directory.Exists(savepath))
                        {
                            Directory.CreateDirectory(savepath);
                        }

                        postedFile.SaveAs(fullpath);
                    }

                    tempTable = SaveAttachedFilesWhenAddingEstimate(filename, fullpath, mime);
                }
                
                RadGrid_Documents.DataSource = tempTable;
                RadGrid_Documents.VirtualItemCount = tempTable.Rows.Count;
                RadGrid_Documents.DataBind();
               
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private string GetUploadDirectory(string mainDirectory)
    {
        var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        return savepathconfig + @"\" + Session["dbname"] + @"\ld_" + mainDirectory + @"\";
    }

    private string GetProposalUploadDirectory(string mainDirectory)
    {
        var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        return savepathconfig + @"\" + Session["dbname"] + @"\" + mainDirectory + @"\";
    }

    private DataTable SaveAttachedFilesWhenAddingEstimate(string fileName, string fullPath, string doctype)
    {
        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;

        if (tempAttachedFiles == null)
        {
            tempAttachedFiles = new DataTable();
            tempAttachedFiles.Columns.Add("id", typeof(int));
            tempAttachedFiles.Columns.Add("filename", typeof(string));
            tempAttachedFiles.Columns.Add("doctype", typeof(string));
            tempAttachedFiles.Columns.Add("Portal", typeof(bool));
            tempAttachedFiles.Columns.Add("Path", typeof(string));
            tempAttachedFiles.Columns.Add("remarks", typeof(string));
            tempAttachedFiles.Columns.Add("MSVisible", typeof(byte));
            tempAttachedFiles.Columns.Add("TempId", typeof(string));
            ViewState["AttachedFiles"] = tempAttachedFiles;
        }

        var row = tempAttachedFiles.NewRow();
        row["id"] = 0;
        row["filename"] = fileName;
        row["doctype"] = doctype;
        row["Portal"] = false;
        row["Path"] = fullPath;
        row["remarks"] = string.Empty;
        row["MSVisible"] = false;
        row["TempId"] = Guid.NewGuid().ToString("N");
        tempAttachedFiles.Rows.Add(row);
        return tempAttachedFiles;
    }

    private void UpdateTempDateWhenCreatingNewEstimate(int estimateId)
    {
        if (ViewState["TempUploadDirectory"] == null)
        {
            return;
        }
        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;

        var mainDirectory = ViewState["TempUploadDirectory"] as string;

        if (tempAttachedFiles == null)
        {
            return;
        }

        var sourceDirectory = GetUploadDirectory(mainDirectory);
        var newDirectory = GetUploadDirectory(estimateId.ToString());
        Directory.Move(sourceDirectory, newDirectory);

        foreach (DataRow row in tempAttachedFiles.Rows)
        {
            objMapData.Screen = "Estimate";
            objMapData.TicketID = estimateId;
            objMapData.TempId = "0";
            objMapData.FileName = row.Field<string>("filename");
            objMapData.DocTypeMIME = row.Field<string>("doctype");
            objMapData.FilePath = row.Field<string>("Path").Replace(sourceDirectory, newDirectory);

            objMapData.DocID = 0;
            objMapData.Mode = 0;
            objMapData.ConnConfig = Session["config"].ToString();
            objBL_MapData.AddFile(objMapData);
        }

        ViewState["TempUploadDirectory"] = null;
        ViewState["AttachedFiles"] = null;


        //get document     
        objMapData.Screen = "Estimate";
        objMapData.TicketID = estimateId;
        objMapData.TempId = "0";
        objMapData.Mode = 1;
        objMapData.ConnConfig = Session["config"].ToString();
        var ds = objBL_MapData.GetDocuments(objMapData);
        var saveDocsRows = ds.Tables[0].AsEnumerable();

        foreach (GridDataItem item in RadGrid_Documents.Items)
        {
            Label lblID = (Label)item.FindControl("lblID");
            HiddenField hdnTempId = (HiddenField)item.FindControl("hdnTempId");
            LinkButton lblName = (LinkButton)item.FindControl("lblName");

            var docRow = saveDocsRows.FirstOrDefault(t => t.Field<string>("Filename") == lblName.Text);
            if (docRow != null)
            {
                lblID.Text = docRow.Field<int>("ID").ToString();
            }

        }
    }

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Documents.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            HiddenField hdnTempId = (HiddenField)item.FindControl("hdnTempId");
            if (lblID.Text == "0")
            {
                DeleteDocFromTempTable(hdnTempId.Value);
            }

            DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
        }

        ScriptManager.RegisterStartupScript(this, GetType(), "DeleteDoc", "$('.dropify').dropify();", true);
    }

    private void DeleteDocFromTempTable(string tempId)
    {
        if (string.IsNullOrWhiteSpace(tempId))
        {
            return;
        }

        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;

        if (tempAttachedFiles == null)
        {
            return;
        }

        var deleteFileRow = tempAttachedFiles.AsEnumerable().FirstOrDefault(t => t.Field<string>("TempId") == tempId);

        if (deleteFileRow != null)
        {
            tempAttachedFiles.Rows.Remove(deleteFileRow);
        }
    }

    private DataTable SaveDocInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MSVisible", typeof(byte));


        foreach (GridDataItem item in RadGrid_Documents.Items)
        {
            Label lblID = (Label)item.FindControl("lblID");
            TextBox txtRemarks = (TextBox)item.FindControl("txtRemarks");
            CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
            CheckBox chkMSVisible = (CheckBox)item.FindControl("chkMSVisible");
            DataRow dr = dt.NewRow();
            dr["ID"] = lblID.Text;
            dr["Portal"] = chkPortal.Checked;
            dr["Remarks"] = txtRemarks.Text;
            dr["MSVisible"] = chkMSVisible.Checked;
            dt.Rows.Add(dr);
        }

        return dt;
    }

    private void GetDocuments()
    {
        if (Request.QueryString["uid"] != null)
        {
            objMapData.Screen = "Estimate";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objMapData.TempId = "0";
            objMapData.Mode = 1;
            objMapData.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_MapData.GetDocuments(objMapData);
            //gvDocuments.DataSource = ds.Tables[0];
            //gvDocuments.DataBind();
            RadGrid_Documents.DataSource = ds.Tables[0];
            RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
           // RadGrid_Documents.DataBind();
        }
        else
        {
            var source = ViewState["AttachedFiles"] as DataTable;
            pnlDocumentButtons.Visible = true;
            RadGrid_Documents.DataSource = source;
            RadGrid_Documents.VirtualItemCount = source != null ? source.Rows.Count : 0;
           // RadGrid_Documents.DataBind();
        }
    }

    public void DeleteFileFromFolder(string StrFilename, int DocumentID)
    {
        try
        {
            //File.Delete(StrFilename);
            DeleteFile(DocumentID);
        }
        catch (FileNotFoundException ex)
        {
            DeleteFile(DocumentID);
        }
        catch (UnauthorizedAccessException ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteAccessWarning", "noty({text: 'Please provide delete permissions to the file path.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);

            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteErrorWarning", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objBL_MapData.DeleteFile(objMapData);
            UpdateDocInfo();
            //GetDocuments();
            RadGrid_Documents.Rebind();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void UpdateDocInfo()
    {
        _objUser.ConnConfig = Session["config"].ToString();
        _objUser.dtDocs = SaveDocInfo();
        objBL_User.UpdateDocInfo(_objUser);
    }

    protected void lbtnTypeSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            _objJob.ConnConfig = Session["config"].ToString();
            _objJob.TypeName = txtBomType.Text;
            _objJob.TypeId = objBL_Job.AddBOMType(_objJob);

            DataSet ds = new DataSet();
            _objJob.ConnConfig = Session["config"].ToString();
            ds = objBL_Job.GetBomType(_objJob);
            dtBomType = ds.Tables[0];

            foreach (GridDataItem gr in gvBOM.Items)
            {
                DropDownList ddlBType = (DropDownList)gr.FindControl("ddlBType");
                ddlBType.Items.Add(new ListItem(_objJob.TypeName.ToString(), _objJob.TypeId.ToString()));
            }
            txtBomType.Text = string.Empty;
            string script = "function f(){$find(\"" + RadWindowBOMType.ClientID + "\").hide(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void GetEstimateForms()
    {
        if (Request.QueryString["uid"] != null)
        {
            //TODO
            //tbpnlForms.Visible = true;
            objEF.Estimate = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objEF.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_EF.GetEstimateFormsByEstimateId(objEF);

            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            dt.Columns.Add("IsLatest", typeof(bool));

            foreach (DataRow dr in dt.Rows)
            {
                String fileName = Convert.ToString(dr["FileName"]);
                String addedOn = Convert.ToString(dr["AddedOn"]);
                String estimateNo = Convert.ToString(dr["Estimate"]);
                bool manualUpload = Convert.ToString(dr["JobTID"]) == "0" || Convert.ToString(dr["JobTID"]) == "" ? true : false;

                //String newFileName = fileName.Split('.').FirstOrDefault() + "-" + estimateNo + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(dr["AddedOn"])).Replace("/", "") + "." + fileName.Split('.').LastOrDefault();

                if (!manualUpload)
                {
                    string strFileEx = fileName.Split('.').Length > 1 ? "." + fileName.Split('.').LastOrDefault() : "";
                    String newFileName = "";
                    if (strFileEx != "")
                    {
                        newFileName = fileName.Replace(strFileEx, "") + "-" + estimateNo + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(addedOn)) + strFileEx;
                    }
                    else
                    {
                        newFileName = fileName + "-" + estimateNo + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(addedOn));
                    }

                    dr["FileName"] = newFileName;
                }
                // Set all with default value = false
                dr["IsLatest"] = false;
            }

            // Set the first record as a latest
            if(dt.Rows.Count > 0) dt.Rows[0]["IsLatest"] = true;

            RadGrid_Forms.DataSource = dt;
            RadGrid_Forms.DataBind();

            if (dt.Rows.Count > 0)
            {
                lnkLatestProposal.Visible = true;
                lblLatestProposalTooltip.Visible = true;
                DataRow latestProposal = dt.Rows[0];
                lblLatestProposalTooltip.Text = ShowHoverText(latestProposal["Estimate"], latestProposal["FileName"], latestProposal["AddedOn"]
                    , latestProposal["AddedBy"], latestProposal["JobTID"]);
                lnkLatestProposal.Attributes["onmouseover"] = "HoverMenutext('" + lblLatestProposalTooltip.ClientID + "',event);";
                lnkLatestProposal.Attributes["onmouseout"] = " $('#" + lblLatestProposalTooltip.ClientID + "').hide();";
            }
            else
            {
                lnkLatestProposal.Visible = false;
                lblLatestProposalTooltip.Visible = false;
            }
        }
        else
        {
            lnkLatestProposal.Visible = false;
            lblLatestProposalTooltip.Visible = false;
        }
    }

    protected void lnkFileName_Click(object sender, EventArgs e)
    {
        objEF.ConnConfig = Session["config"].ToString();
        LinkButton btn = (LinkButton)sender;
        objEF.Id = Convert.ToInt32("0" + btn.CommandArgument);
        objBL_EF.GetEstimateFormById(objEF);
        //String newFileName = objEF.FileName.Split('.').FirstOrDefault() + "-" + objEF.Estimate + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(objEF.AddedOn)) + "." + objEF.FileName.Split('.').LastOrDefault();
        String fileName = objEF.FileName.ToString();
        string strFileEx = fileName.Split('.').Length > 1 ? "." + fileName.Split('.').LastOrDefault() : "";

        if (objEF.JobTID == 0)// Case manual upload proposal
        {
            DownloadDocument(objEF.FilePath, fileName);
        }
        else
        {
            String newFileName = fileName.Replace(strFileEx, "") + "-" + objEF.Estimate + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(objEF.AddedOn)) + ".docx";
            DownloadDocument(objEF.FilePath, newFileName);
        }
    }

    protected void lnkPdfFileName_Click(object sender, EventArgs e)
    {
        objEF.ConnConfig = Session["config"].ToString();
        LinkButton btn = (LinkButton)sender;
        objEF.Id = Convert.ToInt32("0" + btn.CommandArgument);
        objBL_EF.GetEstimateFormById(objEF);
        //String newFileName = objEF.FileName.Split('.').FirstOrDefault() + "-" + objEF.Estimate + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(objEF.AddedOn)) + "." + objEF.FileName.Split('.').LastOrDefault();
        String fileName = objEF.FileName.ToString();
        string strFileEx = fileName.Split('.').Length > 1 ? "." + fileName.Split('.').LastOrDefault() : "";

        if (objEF.JobTID == 0)// Case manual upload proposal
        {
            DownloadDocument(objEF.PdfFilePath, fileName);
        }
        else
        {
            String newFileName = fileName.Replace(strFileEx, "") + "-" + objEF.Estimate + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(objEF.AddedOn)) + ".pdf";

            DownloadDocument(objEF.PdfFilePath, newFileName);
        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        #region Bind Grid
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
        //DataSet estimate = objBL_Customer.GetEstimateOpportunityByEstimateID(objProp_Customer);

        //objEF.ConnConfig = Session["config"].ToString();
        objET.ConnConfig = Session["config"].ToString();
        objET.JobTID = Convert.ToInt32("0" + ddlTemplate.SelectedValue.ToString());

        //objEF.JobTID = objET.JobTID;
        //objEF.Estimate = Convert.ToInt32(Request.QueryString["uid"].ToString());

        DataSet ds = objBL_ET.GetEstimateFormsByJobTId(objET);

        gvEstimateTemplate.DataSource = ds.Tables[0];
        gvEstimateTemplate.DataBind();
        #endregion

        //ModalPopupFormTemplates.Show();
        //pnlFormTemplates.Visible = true;
        string script = "function f(){$find(\"" + RadWindowTemplate.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    }

    public void GenerateForms(DataSet estimate, BusinessEntity.EstimateTemplate et, EstimateForm ef, String formsPath)
    {
        string guid = System.Guid.NewGuid().ToString();
        ef.FilePath = formsPath + guid + "." + et.MIME; //docx
        ef.PdfFilePath = formsPath + guid + ".pdf";
        File.Copy(et.FilePath, ef.FilePath);


        #region Replace Data
        DataRow dr;
        using (DocX document = DocX.Load(ef.FilePath))
        {
            dr = estimate.Tables[0].Rows[0];

            double fontSize = 10;
            var font = new FontFamily("Calibri");
            var color = Color.FromName("Black");
            var tableFont = document.FindUniqueByPattern(@"\[\[TableFont:[ ]*[0-9]*,[ a-zA-Z]*,[ a-zA-Z]*\]\]", RegexOptions.Singleline);
            if (tableFont.Count > 0)
            {
                var fontStr = tableFont[0].Replace("[[", "").Replace("TableFont:", "").Replace("]]", "");
                var fontArr = fontStr.Split(',');
                if (fontArr.Length >= 3)
                {
                    try
                    {
                        fontSize = Convert.ToDouble(fontArr[0].Trim());
                    }
                    catch (Exception)
                    {
                        fontSize = 10;
                    }
                    try
                    {
                        font = new FontFamily(fontArr[1].Trim());
                    }
                    catch (Exception)
                    {
                        font = new FontFamily("Calibri");
                    }
                    try
                    {
                        color = Color.FromName(fontArr[2].Trim());
                    }
                    catch (Exception)
                    {
                        color = Color.FromName("Black");
                    }
                }
                document.ReplaceText(tableFont[0], "", false, RegexOptions.IgnoreCase);
            }

            #region Billing Table
            //List<int> it = document.FindAll("{BillingT&M}");
            var it = document.FindUniqueByPattern(@"{BillingT&M[ ]*:[ ]*[0-9]*,[ a-zA-Z]*,[ a-zA-Z]*}", RegexOptions.Singleline);
            it.AddRange(document.FindUniqueByPattern(@"{BillingT&M}", RegexOptions.Singleline));
            //document.ReplaceText("{BillingT&M}", "", false, RegexOptions.IgnoreCase);
            if (it.Count > 0)
            {
                foreach (var item in it)
                {
                    var fontSizeBilling = fontSize;
                    var fontBilling = font;
                    var colorBilling = color;

                    var fontStr = item.Replace("{", "").Replace("BillingT&M", "").Replace(":", "").Replace("}", "");
                    var fontArr = fontStr.Split(',');
                    if (fontArr.Length >= 3)
                    {
                        try
                        {
                            fontSizeBilling = Convert.ToDouble(fontArr[0].Trim());
                        }
                        catch (Exception)
                        {
                            fontSizeBilling = 10;
                        }
                        try
                        {
                            fontBilling = new FontFamily(fontArr[1].Trim());
                        }
                        catch (Exception)
                        {
                            fontBilling = new FontFamily("Calibri");
                        }
                        try
                        {
                            colorBilling = Color.FromName(fontArr[2].Trim());
                        }
                        catch (Exception)
                        {
                            colorBilling = Color.FromName("Black");
                        }
                    }

                    var estType = dr["EstimateType"].ToString();
                    if (string.IsNullOrEmpty(estType) || estType.ToLower() == "bid")
                    {
                        DataTable billingDt = estimate.Tables[1];
                        var rowCount = billingDt.Rows.Count;
                        Table t = document.AddTable(rowCount + 3, 4);
                        // Specify some properties for this Table.
                        t.Alignment = Alignment.center;
                        t.SetColumnWidth(0, 1000);
                        t.SetColumnWidth(1, 2000);
                        t.SetColumnWidth(2, 4200);
                        t.SetColumnWidth(3, 2000);
                        //t.Design = TableDesign.MediumGrid1Accent2;
                        // Add content to this Table.
                        t.Rows[0].Cells[0].Paragraphs.First().Append("#").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[0].Cells[1].Paragraphs.First().Append("Items").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[0].Cells[2].Paragraphs.First().Append("Description").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[0].Cells[3].Paragraphs.First().Append("Price").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[0].TableHeader = true;

                        int i = 1;
                        double totalAmount = 0;
                        foreach (DataRow dataRow in billingDt.Rows)
                        {
                            t.Rows[i].Cells[0].Paragraphs.First().Append(i.ToString()).Font(fontBilling).Color(colorBilling).FontSize(fontSizeBilling);
                            t.Rows[i].Cells[1].Paragraphs.First().Append(dataRow["MilesName"].ToString()).Font(fontBilling).Color(colorBilling).FontSize(fontSizeBilling);
                            t.Rows[i].Cells[2].Paragraphs.First().Append(dataRow["fDesc"].ToString()).Font(fontBilling).Color(colorBilling).FontSize(fontSizeBilling);
                            t.Rows[i].Cells[3].Paragraphs.First().Append(Convert.ToDecimal(dataRow["Amount"] == DBNull.Value ? "0.00" : dataRow["Amount"]).ToString("C")).Font(fontBilling).Color(colorBilling).FontSize(fontSizeBilling);
                            totalAmount += Convert.ToDouble(dataRow["Amount"].ToString());
                            i++;
                        }
                        i++;
                        Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.two, 0, Color.Black);
                        Border d = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.five, 0, Color.Black);
                        t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Bottom, c);
                        t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Right, c);

                        t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Bottom, c);
                        t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Right, c);

                        t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Bottom, c);
                        t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Right, c);

                        t.Rows[i].Cells[2].Paragraphs.First().Append("Total").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[i].Cells[3].Paragraphs.First().Append(totalAmount.ToString("C")).Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        // Insert the Table into the document.
                        try
                        {
                            foreach (var paragraph in document.Paragraphs)
                            {
                                paragraph.FindAll(item).ForEach(index => paragraph.InsertTableAfterSelf((t)));
                            }
                            document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                        }
                        catch (Exception)
                        {
                            document.InsertTable(t);
                        }
                    }
                    else
                    {
                        DataTable billingDt = estimate.Tables[1];
                        //DataTable test = CreateLaborFromBillingItems(billingDt);
                        var rowCount = billingDt.Rows.Count;
                        Table t = document.AddTable(rowCount + 3, 6);
                        // Specify some properties for this Table.
                        t.Alignment = Alignment.center;
                        t.SetColumnWidth(0, 500);
                        t.SetColumnWidth(1, 1500);
                        t.SetColumnWidth(2, 3000);
                        t.SetColumnWidth(3, 1400);
                        t.SetColumnWidth(4, 1400);
                        t.SetColumnWidth(5, 1500);
                        //t.Design = TableDesign.MediumGrid1Accent2;
                        // Add content to this Table.
                        t.Rows[0].Cells[0].Paragraphs.First().Append("#").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[0].Cells[1].Paragraphs.First().Append("Items").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[0].Cells[2].Paragraphs.First().Append("Description").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[0].Cells[3].Paragraphs.First().Append("Quantity").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[0].Cells[4].Paragraphs.First().Append("Price").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[0].Cells[5].Paragraphs.First().Append("Amount").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[0].TableHeader = true;
                        //.Font(fontBilling).Color(colorBilling).FontSize(fontSizeBilling)
                        int i = 1;
                        double totalAmount = 0;
                        foreach (DataRow dataRow in billingDt.Rows)
                        {
                            t.Rows[i].Cells[0].Paragraphs.First().Append(i.ToString()).Font(fontBilling).Color(colorBilling).FontSize(fontSizeBilling);
                            t.Rows[i].Cells[1].Paragraphs.First().Append(dataRow["MilesName"].ToString()).Font(fontBilling).Color(colorBilling).FontSize(fontSizeBilling);
                            t.Rows[i].Cells[2].Paragraphs.First().Append(dataRow["fDesc"].ToString()).Font(fontBilling).Color(colorBilling).FontSize(fontSizeBilling);
                            t.Rows[i].Cells[3].Paragraphs.First().Append(Convert.ToDecimal(dataRow["Quantity"] == DBNull.Value ? "0.00" : dataRow["Quantity"]).ToString("C")).Font(fontBilling).Color(colorBilling).FontSize(fontSizeBilling);
                            t.Rows[i].Cells[4].Paragraphs.First().Append(Convert.ToDecimal(dataRow["Price"] == DBNull.Value ? "0.00" : dataRow["Price"]).ToString("C")).Font(fontBilling).Color(colorBilling).FontSize(fontSizeBilling);
                            t.Rows[i].Cells[5].Paragraphs.First().Append(Convert.ToDecimal(dataRow["Amount"] == DBNull.Value ? "0.00" : dataRow["Amount"]).ToString("C")).Font(fontBilling).Color(colorBilling).FontSize(fontSizeBilling);
                            totalAmount += Convert.ToDouble(dataRow["Amount"].ToString());
                            i++;
                        }
                        i++;
                        Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.two, 0, Color.Black);
                        Border d = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.five, 0, Color.Black);
                        t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Bottom, c);
                        t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Right, c);

                        t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Bottom, c);
                        t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Right, c);

                        t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Bottom, c);
                        t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Right, c);

                        t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Bottom, c);
                        t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Right, c);

                        t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Bottom, c);
                        t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Right, c);

                        t.Rows[i].Cells[4].Paragraphs.First().Append("Total").Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        t.Rows[i].Cells[5].Paragraphs.First().Append(totalAmount.ToString("C")).Font(fontBilling).Bold().Color(colorBilling).FontSize(fontSizeBilling);
                        // Insert the Table into the document.
                        //try
                        //{
                        //    foreach (var paragraph in document.Paragraphs)
                        //    {
                        //        paragraph.FindAll("{BillingT&M}").ForEach(index => paragraph.InsertTableAfterSelf((t)));
                        //    }
                        //    document.ReplaceText("{BillingT&M}", "", false, RegexOptions.IgnoreCase);
                        //}
                        //catch (Exception)
                        //{
                        //    document.InsertTable(it[0], t);
                        //}

                        // Insert the Table into the document.
                        try
                        {
                            foreach (var paragraph in document.Paragraphs)
                            {
                                paragraph.FindAll(item).ForEach(index => paragraph.InsertTableAfterSelf((t)));
                            }
                            document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                        }
                        catch (Exception)
                        {
                            document.InsertTable(t);
                        }
                    }
                }
            }
            #endregion

            #region BOM Table
            #region BOM Generic
            // {BOM:[Line | 700 |#][fDesc|1500|Item][Quan|1500|Quantity][UM|1000|U/M][Unit|1000|Unit Price][MarkupPrice|1000|Extended Price]}
            // {BOM: [Line | 10 |#] [fDesc|30|Item][Quan|20|Quantity][UM|10|U/M][Unit|15|Unit Price][MarkupPrice|15|Extended Price]}
            // {BOM:[Line|700|#][fDesc|1500|Item][Quan|1500|Quantity]  [Unit|1500|Unit Price][MarkupPrice|1500|Extended Price]}
            //var lstBOMIndexes = document.FindUniqueByPattern(@"{BOM:(\[[ |#/,;0-9a-zA-Z]*\])(\[[ |#/,;0-9a-zA-Z]*\])*}", RegexOptions.Singleline);
            var lstBOMIndexes = document.FindUniqueByPattern(@"{BOM[ ]*:[ ]*(\[[ #/,;0-9a-zA-Z][ #/,;0-9a-zA-Z]*\|[0-9 ][0-9 ]*(\|[ #/,;0-9a-zA-Z]*)*\])[ ]*(\[[ #/,;0-9a-zA-Z][ #/,;0-9a-zA-Z]*\|[0-9 ][0-9 ]*(\|[ #/,;0-9a-zA-Z]*)*\][ ]*)*}", RegexOptions.Singleline);
            if (lstBOMIndexes.Count > 0)
            {
                foreach (var item in lstBOMIndexes)
                {
                    var fontSizeBOM = fontSize;
                    var fontBOM = font;
                    var colorBOM = color;

                    string value = ("([[ |#/,0-9a-zA-Z]*])");
                    var colsStr = item.Replace("{", "").Replace("BOM:", "").Replace("}", "");
                    Match[] matches = Regex.Matches(colsStr, @value)
                       .Cast<Match>()
                       .ToArray();

                    List<string> colsName = new List<string>();
                    List<string> colsHeader = new List<string>();
                    List<string> colsWidth = new List<string>();

                    DataTable bomDt = estimate.Tables[5];
                    var rowCount = bomDt.Rows.Count;
                    Table t = document.AddTable(rowCount + 1, matches.Length);
                    // Specify some properties for this Table.
                    t.Alignment = Alignment.left;

                    for (int j = 0; j < matches.Length; j++)
                    {
                        var temp = matches[j].ToString().Trim().Replace("[","").Replace("]","").Split('|');
                        colsName.Add(temp[0].Trim());
                        var colWidth = 0;
                        if (temp.Length > 2)
                        {
                            colsWidth.Add(temp[1]);
                            colWidth = Convert.ToInt32(temp[1]);
                            colsHeader.Add(temp[2]);
                        }
                        else if (temp.Length == 2)
                        {
                            colsWidth.Add(temp[1]);
                            colWidth = Convert.ToInt32(temp[1]);
                            colsHeader.Add(temp[0]);
                        }

                        t.SetColumnWidth(j, colWidth);
                    }

                    int k = 0;
                    foreach (var col in colsName)
                    {
                        t.Rows[0].Cells[k].Paragraphs.First().Append(colsHeader[k]).Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                        k++;
                    }
                 
                    t.Rows[0].TableHeader = true;
                    Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.two, 0, Color.Black);
                    Border d = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.five, 0, Color.Black);
                    int i = 1;
                    foreach (DataRow dataRow in bomDt.Rows)
                    {
                        k = 0;
                        foreach (var col in colsName)
                        {
                            var dtType = bomDt.Columns[k].DataType;

                            //if ((dtType == typeof(Double) || dtType == typeof(Decimal)) && col != "Quan")
                            if (col == "Unit" || col == "MarkupPrice")
                            {
                                t.Rows[i].Cells[k].Paragraphs.First().Append(Convert.ToDecimal(dataRow[col] == DBNull.Value ? "0.00" : dataRow[col]).ToString("C")).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                            }
                            else
                            {
                                t.Rows[i].Cells[k].Paragraphs.First().Append(dataRow[col].ToString()).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                            }

                            k++;
                        }

                        i++;
                    }

                    // Insert the Table into the document.
                    try
                    {
                        foreach (var paragraph in document.Paragraphs)
                        {
                            paragraph.FindAll(item).ForEach(index => paragraph.InsertTableAfterSelf((t)));
                        }
                        document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                    }
                    catch (Exception)
                    {
                        document.InsertTable(t);
                    }
                }
            }
            #endregion

            #region With HST

            var lstIndexes = document.FindUniqueByPattern(@"{BOM_T&M[ ]*:[ ]*[0-9]*,[ a-zA-Z]*,[ a-zA-Z]*}", RegexOptions.Singleline);
            lstIndexes.AddRange(document.FindUniqueByPattern(@"{BOM_T&M}", RegexOptions.Singleline));
            //List<int> lstIndexes = document.FindAll("{BOM_T&M}");
            //document.ReplaceText("{BOM_T&M}", "", false, RegexOptions.IgnoreCase);

            if (lstIndexes.Count > 0)
            {
                foreach (var item in lstIndexes)
                {
                    var fontSizeBOM = fontSize;
                    var fontBOM = font;
                    var colorBOM = color;

                    var fontStr = item.Replace("{", "").Replace("BOM_T&M", "").Replace(":", "").Replace("}", "");
                    var fontArr = fontStr.Split(',');
                    if (fontArr.Length >= 3)
                    {
                        try
                        {
                            fontSizeBOM = Convert.ToDouble(fontArr[0].Trim());
                        }
                        catch (Exception)
                        {
                            fontSizeBOM = 10;
                        }
                        try
                        {
                            fontBOM = new FontFamily(fontArr[1].Trim());
                        }
                        catch (Exception)
                        {
                            fontBOM = new FontFamily("Calibri");
                        }
                        try
                        {
                            colorBOM = Color.FromName(fontArr[2].Trim());
                        }
                        catch (Exception)
                        {
                            colorBOM = Color.FromName("Black");
                        }
                    }
                    DataTable bomDt = estimate.Tables[5];
                    var rowCount = bomDt.Rows.Count;
                    Table t = document.AddTable(rowCount + 4, 6);
                    // Specify some properties for this Table.
                    t.Alignment = Alignment.center;

                    t.SetColumnWidth(0, 800);
                    t.SetColumnWidth(1, 3400);
                    t.SetColumnWidth(2, 1100);
                    t.SetColumnWidth(3, 900);
                    t.SetColumnWidth(4, 1500);
                    t.SetColumnWidth(5, 1500);
                    //t.Design = TableDesign.MediumGrid1Accent2;
                    // Add content to this Table.
                    //.Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM)
                    //.Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM)
                    t.Rows[0].Cells[0].Paragraphs.First().Append("#").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].Cells[1].Paragraphs.First().Append("Item").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].Cells[2].Paragraphs.First().Append("Quantity").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].Cells[3].Paragraphs.First().Append("U/M").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].Cells[4].Paragraphs.First().Append("Unit Price").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].Cells[5].Paragraphs.First().Append("Extended Price").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].TableHeader = true;

                    int i = 1;
                    foreach (DataRow dataRow in bomDt.Rows)
                    {
                        t.Rows[i].Cells[0].Paragraphs.First().Append(i.ToString()).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        t.Rows[i].Cells[1].Paragraphs.First().Append(dataRow["fDesc"].ToString()).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        t.Rows[i].Cells[2].Paragraphs.First().Append(dataRow["Quan"].ToString()).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        t.Rows[i].Cells[3].Paragraphs.First().Append(dataRow["UM"].ToString()).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        t.Rows[i].Cells[4].Paragraphs.First().Append(Convert.ToDecimal(dataRow["Unit"] == DBNull.Value ? "0.00" : dataRow["Unit"]).ToString("C")).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        t.Rows[i].Cells[5].Paragraphs.First().Append(Convert.ToDecimal(dataRow["MarkupPrice"] == DBNull.Value ? "0.00" : dataRow["MarkupPrice"]).ToString("C")).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        i++;
                    }

                    Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.two, 0, Color.Black);
                    //Border d = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.five, 0, Color.Black);

                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].Paragraphs.First().Append("Sub total").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    var subToalVal = Convert.ToDecimal(dr["SubToalVal"] == DBNull.Value ? "0.00" : dr["SubToalVal"]);
                    t.Rows[i].Cells[5].Paragraphs.First().Append(subToalVal.ToString("C")).Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);

                    i++;

                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].Paragraphs.First().Append("HST").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    var salesTaxVal = Convert.ToDecimal(dr["STaxVal"] == DBNull.Value ? "0.00" : dr["STaxVal"]);
                    t.Rows[i].Cells[5].Paragraphs.First().Append(salesTaxVal.ToString("C")).Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);

                    i++;
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].Paragraphs.First().Append("Total Price").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    var totalPrice = Convert.ToDecimal(dr["BidPrice"] == DBNull.Value ? "0.00" : dr["BidPrice"]);
                    t.Rows[i].Cells[5].Paragraphs.First().Append(totalPrice.ToString("C")).Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    // Insert the Table into the document.
                    try
                    {
                        foreach (var paragraph in document.Paragraphs)
                        {
                            paragraph.FindAll(item).ForEach(index => paragraph.InsertTableAfterSelf((t)));
                        }
                        document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                    }
                    catch (Exception)
                    {
                        document.InsertTable(t);
                    }
                }
            }
            #endregion
            #region With SalesTax
            //List<int> lstBOMtbs = document.FindAll("{BOM_T&M_ST}");
            var lstBOMtbs = document.FindUniqueByPattern(@"{BOM_T&M_ST[ ]*:[ ]*[0-9]*,[ a-zA-Z]*,[ a-zA-Z]*}", RegexOptions.Singleline);
            lstBOMtbs.AddRange(document.FindUniqueByPattern(@"{BOM_T&M_ST}", RegexOptions.Singleline));
            //document.ReplaceText("{BOM_T&M}", "", false, RegexOptions.IgnoreCase);

            if (lstBOMtbs.Count > 0)
            {
                foreach (var item in lstBOMtbs)
                {
                    var fontSizeBOM = fontSize;
                    var fontBOM = font;
                    var colorBOM = color;

                    var fontStr = item.Replace("{", "").Replace("BOM_T&M_ST", "").Replace(":", "").Replace("}", "");
                    var fontArr = fontStr.Split(',');
                    if (fontArr.Length >= 3)
                    {
                        try
                        {
                            fontSizeBOM = Convert.ToDouble(fontArr[0].Trim());
                        }
                        catch (Exception)
                        {
                            fontSizeBOM = 10;
                        }
                        try
                        {
                            fontBOM = new FontFamily(fontArr[1].Trim());
                        }
                        catch (Exception)
                        {
                            fontBOM = new FontFamily("Calibri");
                        }
                        try
                        {
                            colorBOM = Color.FromName(fontArr[2].Trim());
                        }
                        catch (Exception)
                        {
                            colorBOM = Color.FromName("Black");
                        }
                    }

                    DataTable bomDt = estimate.Tables[5];
                    var rowCount = bomDt.Rows.Count;
                    Table t = document.AddTable(rowCount + 4, 6);
                    // Specify some properties for this Table.
                    t.Alignment = Alignment.center;

                    t.SetColumnWidth(0, 800);
                    t.SetColumnWidth(1, 3400);
                    t.SetColumnWidth(2, 1100);
                    t.SetColumnWidth(3, 900);
                    t.SetColumnWidth(4, 1500);
                    t.SetColumnWidth(5, 1500);
                    //t.Design = TableDesign.MediumGrid1Accent2;
                    // Add content to this Table.
                    //.Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM)
                    //.Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM)
                    t.Rows[0].Cells[0].Paragraphs.First().Append("#").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].Cells[1].Paragraphs.First().Append("Item").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].Cells[2].Paragraphs.First().Append("Quantity").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].Cells[3].Paragraphs.First().Append("U/M").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].Cells[4].Paragraphs.First().Append("Unit Price").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].Cells[5].Paragraphs.First().Append("Extended Price").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    t.Rows[0].TableHeader = true;

                    int i = 1;
                    foreach (DataRow dataRow in bomDt.Rows)
                    {
                        t.Rows[i].Cells[0].Paragraphs.First().Append(i.ToString()).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        t.Rows[i].Cells[1].Paragraphs.First().Append(dataRow["fDesc"].ToString()).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        t.Rows[i].Cells[2].Paragraphs.First().Append(dataRow["Quan"].ToString()).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        t.Rows[i].Cells[3].Paragraphs.First().Append(dataRow["UM"].ToString()).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        t.Rows[i].Cells[4].Paragraphs.First().Append(Convert.ToDecimal(dataRow["Unit"] == DBNull.Value ? "0.00" : dataRow["Unit"]).ToString("C")).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        t.Rows[i].Cells[5].Paragraphs.First().Append(Convert.ToDecimal(dataRow["MarkupPrice"] == DBNull.Value ? "0.00" : dataRow["MarkupPrice"]).ToString("C")).Font(fontBOM).Color(colorBOM).FontSize(fontSizeBOM);
                        i++;
                    }

                    Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.two, 0, Color.Black);
                    Border d = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.five, 0, Color.Black);

                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].Paragraphs.First().Append("Sub total").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    var subToalVal = Convert.ToDecimal(dr["SubToalVal"] == DBNull.Value ? "0.00" : dr["SubToalVal"]);
                    t.Rows[i].Cells[5].Paragraphs.First().Append(subToalVal.ToString("C")).Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);

                    i++;

                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].Paragraphs.First().Append("Sales Tax").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    var salesTaxVal = Convert.ToDecimal(dr["STaxVal"] == DBNull.Value ? "0.00" : dr["STaxVal"]);
                    t.Rows[i].Cells[5].Paragraphs.First().Append(salesTaxVal.ToString("C")).Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);

                    i++;
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[3].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[i].Cells[4].SetBorder(TableCellBorderType.Right, c);

                    t.Rows[i].Cells[4].Paragraphs.First().Append("Total Price").Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    var totalPrice = Convert.ToDecimal(dr["BidPrice"] == DBNull.Value ? "0.00" : dr["BidPrice"]);
                    t.Rows[i].Cells[5].Paragraphs.First().Append(totalPrice.ToString("C")).Font(fontBOM).Bold().Color(colorBOM).FontSize(fontSizeBOM);
                    // Insert the Table into the document.
                    try
                    {
                        foreach (var paragraph in document.Paragraphs)
                        {
                            paragraph.FindAll(item).ForEach(index => paragraph.InsertTableAfterSelf((t)));
                        }
                        document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                    }
                    catch (Exception)
                    {
                        document.InsertTable(t);
                    }
                }

            }
            #endregion
            #endregion

            #region Custom for TEI: CONTRACT CHANGE REQUEST: CCR

            #region CCR_Labor
            //List<int> labIndex = document.FindAll("{CCR_Labor}");
            var labIndex = document.FindUniqueByPattern(@"{CCR_Labor[ ]*:[ ]*[0-9]*,[ a-zA-Z]*,[ a-zA-Z]*}", RegexOptions.Singleline);
            labIndex.AddRange(document.FindUniqueByPattern(@"{CCR_Labor}", RegexOptions.Singleline));
            if (labIndex.Count > 0)
            {
                foreach (var item in labIndex)
                {
                    var fontSizeCCR_Labor = fontSize;
                    var fontCCR_Labor = font;
                    var colorCCR_Labor = color;
                    var fontStr = item.Replace("{", "").Replace("CCR_Labor", "").Replace(":", "").Replace("}", "");
                    var fontArr = fontStr.Split(',');
                    if (fontArr.Length >= 3)
                    {
                        try
                        {
                            fontSizeCCR_Labor = Convert.ToDouble(fontArr[0].Trim());
                        }
                        catch (Exception)
                        {
                            fontSizeCCR_Labor = 10;
                        }
                        try
                        {
                            fontCCR_Labor = new FontFamily(fontArr[1].Trim());
                        }
                        catch (Exception)
                        {
                            fontCCR_Labor = new FontFamily("Calibri");
                        }
                        try
                        {
                            colorCCR_Labor = Color.FromName(fontArr[2].Trim());
                        }
                        catch (Exception)
                        {
                            colorCCR_Labor = Color.FromName("Black");
                        }
                    }


                    DataTable bomDt = estimate.Tables[4];
                    DataTable labDT = CreateLaborFromBOMItems(bomDt);
                    var rowCount = labDT.Rows.Count;
                    Table t = document.AddTable(rowCount + 3, 9);
                    // Specify some properties for this Table.
                    t.Alignment = Alignment.center;
                    //t.SetColumnWidth(0, 0);
                    t.SetColumnWidth(0, 1800);
                    t.SetColumnWidth(1, 1100);
                    t.SetColumnWidth(2, 1100);
                    t.SetColumnWidth(3, 1100);
                    t.SetColumnWidth(4, 1100);
                    t.SetColumnWidth(5, 1100);
                    t.SetColumnWidth(6, 1100);
                    t.SetColumnWidth(7, 1500);
                    //t.Design = TableDesign.MediumGrid1Accent2;
                    // Add content to this Table.
                    Border c = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.one, 0, Color.Black);
                    Border d = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 0, Color.Black);
                    t.SetBorder(TableBorderType.Top, d);
                    t.SetBorder(TableBorderType.Left, d);
                    t.SetBorder(TableBorderType.Bottom, d);
                    t.SetBorder(TableBorderType.Right, d);
                    t.SetBorder(TableBorderType.InsideH, d);
                    t.SetBorder(TableBorderType.InsideV, d);
                    t.Rows[0].Height = 17;
                    t.Rows[0].Cells[2 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[0].Cells[2 - 1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[0].Cells[3 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[0].Cells[4 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[0].Cells[5 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[0].Cells[5 - 1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[0].Cells[6 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[0].Cells[7 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[0].Cells[7 - 1].SetBorder(TableCellBorderType.Right, c);

                    //t.Rows[0].Cells[0].Paragraphs.First().Append("LABOR:").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[0].Cells[1 - 1].Paragraphs.First().Append("LABOR").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[0].Cells[2 - 1].Paragraphs.First().Append("").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[0].Cells[3 - 1].Paragraphs.First().Append("HOURS").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[0].Cells[4 - 1].Paragraphs.First().Append("").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[0].Cells[5 - 1].Paragraphs.First().Append("").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[0].Cells[6 - 1].Paragraphs.First().Append("RATE").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[0].Cells[7 - 1].Paragraphs.First().Append("").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[0].Cells[8 - 1].Paragraphs.First().Append("TOTAL").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[0].Cells[8 - 1].Paragraphs.First().Alignment = Alignment.right;
                    t.Rows[0].Cells[8 - 1].VerticalAlignment = Novacode.VerticalAlignment.Bottom;
                    t.Rows[0].TableHeader = true;
                    t.Rows[1].Height = 17;
                    //t.Rows[1].Cells[0].SetBorder(TableCellBorderType.Top, c);
                    //t.Rows[1].Cells[0].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[1].Cells[1 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[1].Cells[1 - 1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[1].Cells[2 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[1].Cells[2 - 1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[1].Cells[3 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[1].Cells[3 - 1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[1].Cells[4 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[1].Cells[4 - 1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[1].Cells[5 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[1].Cells[5 - 1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[1].Cells[6 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[1].Cells[6 - 1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[1].Cells[7 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[1].Cells[7 - 1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[1].Cells[8 - 1].SetBorder(TableCellBorderType.Top, c);
                    t.Rows[1].Cells[8 - 1].SetBorder(TableCellBorderType.Left, c);
                    t.Rows[1].Cells[8 - 1].SetBorder(TableCellBorderType.Right, c);

                    //t.Rows[1].Cells[0].Paragraphs.First().Append("# OF MEN").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[1].Cells[1 - 1].Paragraphs.First().Append("CLASSIFICATION").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[1].Cells[2 - 1].Paragraphs.First().Append("ST").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[1].Cells[3 - 1].Paragraphs.First().Append("PT").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[1].Cells[4 - 1].Paragraphs.First().Append("DT").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[1].Cells[5 - 1].Paragraphs.First().Append("ST").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[1].Cells[6 - 1].Paragraphs.First().Append("PT").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[1].Cells[7 - 1].Paragraphs.First().Append("DT").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[1].Cells[8 - 1].Paragraphs.First().Append("").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[1].TableHeader = true;

                    int i = 2;
                    double totalAmount = 0;
                    foreach (DataRow dataRow in labDT.Rows)
                    {
                        t.Rows[i].Height = 17;
                        //t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Top, c);
                        //t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[1 - 1].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[1 - 1].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[2 - 1].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[2 - 1].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[3 - 1].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[3 - 1].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[4 - 1].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[4 - 1].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[5 - 1].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[5 - 1].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[6 - 1].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[6 - 1].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[7 - 1].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[7 - 1].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[8 - 1].SetBorder(TableCellBorderType.Top, c);
                        t.Rows[i].Cells[8 - 1].SetBorder(TableCellBorderType.Left, c);
                        t.Rows[i].Cells[8 - 1].SetBorder(TableCellBorderType.Right, c);
                        t.Rows[i].Cells[8 - 1].Paragraphs.First().Alignment = Alignment.right;

                        //t.Rows[i].Cells[0].Paragraphs.First().Append(dataRow["NoOfMen"].ToString()).Font(fontCCR_Labor).Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                        t.Rows[i].Cells[1 - 1].Paragraphs.First().Append(dataRow["Classification"].ToString()).Font(fontCCR_Labor).Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                        t.Rows[i].Cells[2 - 1].Paragraphs.First().Append(Convert.ToDecimal(dataRow["ST_Hour"] == DBNull.Value ? "0" : dataRow["ST_Hour"]).ToString()).Font(fontCCR_Labor).Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                        t.Rows[i].Cells[3 - 1].Paragraphs.First().Append(Convert.ToDecimal(dataRow["PT_Hour"] == DBNull.Value ? "0" : dataRow["PT_Hour"]).ToString()).Font(fontCCR_Labor).Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                        t.Rows[i].Cells[4 - 1].Paragraphs.First().Append(Convert.ToDecimal(dataRow["DT_Hour"] == DBNull.Value ? "0" : dataRow["DT_Hour"]).ToString()).Font(fontCCR_Labor).Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                        t.Rows[i].Cells[5 - 1].Paragraphs.First().Append(Convert.ToDecimal(dataRow["ST_Rate"] == DBNull.Value ? "0" : dataRow["ST_Rate"]).ToString("C")).Font(fontCCR_Labor).Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                        t.Rows[i].Cells[6 - 1].Paragraphs.First().Append(Convert.ToDecimal(dataRow["PT_Rate"] == DBNull.Value ? "0" : dataRow["PT_Rate"]).ToString("C")).Font(fontCCR_Labor).Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                        t.Rows[i].Cells[7 - 1].Paragraphs.First().Append(Convert.ToDecimal(dataRow["DT_Rate"] == DBNull.Value ? "0" : dataRow["DT_Rate"]).ToString("C")).Font(fontCCR_Labor).Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                        t.Rows[i].Cells[8 - 1].Paragraphs.First().Append(Convert.ToDecimal(dataRow["Total"] == DBNull.Value ? "0.00" : dataRow["Total"]).ToString("C")).Font(fontCCR_Labor).Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                        totalAmount += Convert.ToDouble(dataRow["Total"].ToString());
                        i++;
                    }

                    //t.Rows[i-1].Cells[0].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i - 1].Cells[1 - 1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i - 1].Cells[2 - 1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i - 1].Cells[3 - 1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i - 1].Cells[4 - 1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i - 1].Cells[5 - 1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i - 1].Cells[6 - 1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i - 1].Cells[7 - 1].SetBorder(TableCellBorderType.Bottom, c);
                    t.Rows[i - 1].Cells[8 - 1].SetBorder(TableCellBorderType.Bottom, c);

                    //t.Rows[i].MergeCells(6, 7);
                    t.Rows[i].MergeCells(5, 6);
                    t.Rows[i].Cells[6 - 1].Paragraphs.First().Append("LABOR TOTAL:").Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[i].Cells[6 - 1].Paragraphs.First().Alignment = Alignment.right;
                    t.Rows[i].Cells[7 - 1].Paragraphs.First().Append(totalAmount.ToString("C")).Font(fontCCR_Labor).Bold().Color(colorCCR_Labor).FontSize(fontSizeCCR_Labor);
                    t.Rows[i].Cells[7 - 1].Paragraphs.First().Alignment = Alignment.right;
                    // Insert the Table into the document.
                    try
                    {
                        foreach (var paragraph in document.Paragraphs)
                        {
                            paragraph.FindAll(item).ForEach(index => paragraph.InsertTableAfterSelf((t)));
                        }
                        document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                    }
                    catch (Exception)
                    {
                        document.InsertTable(t);
                    }
                }
            }
            #endregion

            #region CCR_Materials

            //List<int> matIndex = document.FindAll("{CCR_Material}");
            var matIndex = document.FindUniqueByPattern(@"{CCR_Material[ ]*:[ ]*[0-9]*,[ a-zA-Z]*,[ a-zA-Z]*}", RegexOptions.Singleline);
            matIndex.AddRange(document.FindUniqueByPattern(@"{CCR_Material}", RegexOptions.Singleline));
            if (matIndex.Count > 0)
            {
                foreach (var item in matIndex)
                {
                    var fontSizeCCR_Material = fontSize;
                    var fontCCR_Material = font;
                    var colorCCR_Material = color;

                    var fontStr = item.Replace("{", "").Replace("CCR_Material", "").Replace(":", "").Replace("}", "");
                    var fontArr = fontStr.Split(',');
                    if (fontArr.Length >= 3)
                    {
                        try
                        {
                            fontSizeCCR_Material = Convert.ToDouble(fontArr[0].Trim());
                        }
                        catch (Exception)
                        {
                            fontSizeCCR_Material = 10;
                        }
                        try
                        {
                            fontCCR_Material = new FontFamily(fontArr[1].Trim());
                        }
                        catch (Exception)
                        {
                            fontCCR_Material = new FontFamily("Calibri");
                        }
                        try
                        {
                            colorCCR_Material = Color.FromName(fontArr[2].Trim());
                        }
                        catch (Exception)
                        {
                            colorCCR_Material = Color.FromName("Black");
                        }
                    }
                    var matDT = estimate.Tables[4].Select("BType = '1' AND fDesc <> 'Use Tax'").ToList();
                    var rowCount = matDT.Count;
                    Table t = document.AddTable(rowCount + 2, 4);
                    // Specify some properties for this Table.
                    t.Alignment = Alignment.center;
                    t.SetColumnWidth(0, 5400);
                    t.SetColumnWidth(1, 1500);
                    t.SetColumnWidth(2, 1500);
                    t.SetColumnWidth(3, 1500);
                    //t.Design = TableDesign.MediumGrid1Accent2;
                    // Add content to this Table.
                    Border c = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.one, 0, Color.Black);
                    Border d = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 0, Color.Black);
                    t.SetBorder(TableBorderType.Top, c);
                    t.SetBorder(TableBorderType.Left, c);
                    t.SetBorder(TableBorderType.Bottom, c);
                    t.SetBorder(TableBorderType.Right, c);
                    t.SetBorder(TableBorderType.InsideH, c);
                    t.SetBorder(TableBorderType.InsideV, c);
                    t.Rows[0].Height = 17;
                    t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Top, d);
                    t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Right, d);
                    t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Top, d);
                    t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Right, d);
                    t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Top, d);
                    t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Right, d);
                    t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Top, d);
                    t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Right, d);

                    t.Rows[0].Cells[0].Paragraphs.First().Append("MATERIALS:").Font(fontCCR_Material).Bold().Color(colorCCR_Material).FontSize(fontSizeCCR_Material);
                    t.Rows[0].Cells[0].VerticalAlignment = Novacode.VerticalAlignment.Bottom;
                    t.Rows[0].Cells[1].Paragraphs.First().Append("QUANTITY").Font(fontCCR_Material).Bold().Color(colorCCR_Material).FontSize(fontSizeCCR_Material);
                    t.Rows[0].Cells[1].VerticalAlignment = Novacode.VerticalAlignment.Bottom;
                    t.Rows[0].Cells[2].Paragraphs.First().Append("UNIT PRICE").Font(fontCCR_Material).Bold().Color(colorCCR_Material).FontSize(fontSizeCCR_Material);
                    t.Rows[0].Cells[2].VerticalAlignment = Novacode.VerticalAlignment.Bottom;
                    t.Rows[0].Cells[3].Paragraphs.First().Append("TOTAL").Font(fontCCR_Material).Bold().Color(colorCCR_Material).FontSize(fontSizeCCR_Material);
                    t.Rows[0].Cells[3].Paragraphs.First().Alignment = Alignment.right;
                    t.Rows[0].Cells[3].VerticalAlignment = Novacode.VerticalAlignment.Bottom;
                    t.Rows[0].TableHeader = true;

                    int i = 1;
                    double totalAmount = 0;
                    foreach (DataRow dataRow in matDT)
                    {
                        t.Rows[i].Height = 17;
                        t.Rows[i].Cells[0].Paragraphs.First().Append(dataRow["fdesc"].ToString()).Font(fontCCR_Material).Color(colorCCR_Material).FontSize(fontSizeCCR_Material);
                        t.Rows[i].Cells[1].Paragraphs.First().Append(Convert.ToDecimal(dataRow["QtyReq"] == DBNull.Value ? "0" : dataRow["QtyReq"]).ToString()).Font(fontCCR_Material).Color(colorCCR_Material).FontSize(fontSizeCCR_Material);
                        t.Rows[i].Cells[2].Paragraphs.First().Append(Convert.ToDecimal(dataRow["BudgetUnit"] == DBNull.Value ? "0" : dataRow["BudgetUnit"]).ToString()).Font(fontCCR_Material).Color(colorCCR_Material).FontSize(fontSizeCCR_Material);
                        t.Rows[i].Cells[3].Paragraphs.First().Append(Convert.ToDecimal(dataRow["BudgetExt"] == DBNull.Value ? "0" : dataRow["BudgetExt"]).ToString("C")).Font(fontCCR_Material).Color(colorCCR_Material).FontSize(fontSizeCCR_Material);
                        t.Rows[i].Cells[3].Paragraphs.First().Alignment = Alignment.right;
                        totalAmount += Convert.ToDouble(dataRow["BudgetExt"] == DBNull.Value ? "0" : dataRow["BudgetExt"]);
                        i++;
                    }

                    t.Rows[i].MergeCells(1, 2);
                    t.Rows[i].Cells[1].Paragraphs.First().Append("MATERIAL TOTAL:").Bold().Font(fontCCR_Material).Color(colorCCR_Material).FontSize(fontSizeCCR_Material);
                    t.Rows[i].Cells[1].Paragraphs.First().Alignment = Alignment.right;
                    t.Rows[i].Cells[2].Paragraphs.First().Append(totalAmount.ToString("C")).Bold().Font(fontCCR_Material).Color(colorCCR_Material).FontSize(fontSizeCCR_Material);
                    t.Rows[i].Cells[2].Paragraphs.First().Alignment = Alignment.right;

                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Bottom, d);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Right, d);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Bottom, d);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Right, d);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Bottom, d);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Right, d);

                    // Insert the Table into the document.
                    try
                    {
                        foreach (var paragraph in document.Paragraphs)
                        {
                            paragraph.FindAll(item).ForEach(index => paragraph.InsertTableAfterSelf((t)));
                        }
                        document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                    }
                    catch (Exception)
                    {
                        document.InsertTable(t);
                    }
                }
            }
            #endregion

            var useTaxDT = estimate.Tables[4].Select("BType = '1' AND fDesc = 'Use Tax'").ToList();
            if (useTaxDT != null)
            {
                double useTaxVal = 0;
                foreach (DataRow dataRow in useTaxDT)
                {
                    useTaxVal += Convert.ToDouble(dataRow["BudgetExt"] == DBNull.Value ? "0" : dataRow["BudgetExt"]);
                }

                document.ReplaceText("{UseTax}", useTaxVal.ToString("C"), false, RegexOptions.IgnoreCase);
            }

            #region CCR_Miscellaneous
            //List<int> misIndex = document.FindAll("{CCR_Miscellaneous}");
            var misIndex = document.FindUniqueByPattern(@"{CCR_Miscellaneous[ ]*:[ ]*[0-9]*,[ a-zA-Z]*,[ a-zA-Z]*}", RegexOptions.Singleline);
            misIndex.AddRange(document.FindUniqueByPattern(@"{CCR_Miscellaneous}", RegexOptions.Singleline));

            if (misIndex.Count > 0)
            {
                foreach (var item in misIndex)
                {
                    var fontSizeCCR_Miscellaneous = fontSize;
                    var fontCCR_Miscellaneous = font;
                    var colorCCR_Miscellaneous = color;

                    var fontStr = item.Replace("{", "").Replace("CCR_Miscellaneous", "").Replace(":", "").Replace("}", "");
                    var fontArr = fontStr.Split(',');
                    if (fontArr.Length >= 3)
                    {
                        try
                        {
                            fontSizeCCR_Miscellaneous = Convert.ToDouble(fontArr[0].Trim());
                        }
                        catch (Exception)
                        {
                            fontSizeCCR_Miscellaneous = 10;
                        }
                        try
                        {
                            fontCCR_Miscellaneous = new FontFamily(fontArr[1].Trim());
                        }
                        catch (Exception)
                        {
                            fontCCR_Miscellaneous = new FontFamily("Calibri");
                        }
                        try
                        {
                            colorCCR_Miscellaneous = Color.FromName(fontArr[2].Trim());
                        }
                        catch (Exception)
                        {
                            colorCCR_Miscellaneous = Color.FromName("Black");
                        }
                    }

                    var misDT = estimate.Tables[4].Select("BType <> '1' and BType <> '2'").ToList();
                    var rowCount = misDT.Count;
                    Table t = document.AddTable(rowCount + 2, 4);
                    // Specify some properties for this Table.
                    t.Alignment = Alignment.center;
                    t.Rows[0].Height = 17;
                    t.SetColumnWidth(0, 5400);
                    t.SetColumnWidth(1, 1500);
                    t.SetColumnWidth(2, 1500);
                    t.SetColumnWidth(3, 1500);
                    //t.Design = TableDesign.MediumGrid1Accent2;
                    // Add content to this Table.
                    Border c = new Border(Novacode.BorderStyle.Tcbs_single, BorderSize.one, 0, Color.Black);
                    Border d = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 0, Color.Black);
                    t.SetBorder(TableBorderType.Top, c);
                    t.SetBorder(TableBorderType.Left, c);
                    t.SetBorder(TableBorderType.Bottom, c);
                    t.SetBorder(TableBorderType.Right, c);
                    t.SetBorder(TableBorderType.InsideH, c);
                    t.SetBorder(TableBorderType.InsideV, c);

                    t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Top, d);
                    t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Right, d);
                    t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Top, d);
                    t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Right, d);
                    t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Top, d);
                    t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Right, d);
                    t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Top, d);
                    t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Right, d);

                    t.Rows[0].Cells[0].Paragraphs.First().Append("MISCELLANEOUS:").Bold().Font(fontCCR_Miscellaneous).Color(colorCCR_Miscellaneous).FontSize(fontSizeCCR_Miscellaneous);
                    t.Rows[0].Cells[0].VerticalAlignment = Novacode.VerticalAlignment.Bottom;
                    t.Rows[0].Cells[1].Paragraphs.First().Append("QUANTITY").Bold().Font(fontCCR_Miscellaneous).Color(colorCCR_Miscellaneous).FontSize(fontSizeCCR_Miscellaneous);
                    t.Rows[0].Cells[1].VerticalAlignment = Novacode.VerticalAlignment.Bottom;
                    t.Rows[0].Cells[2].Paragraphs.First().Append("UNIT PRICE").Bold().Font(fontCCR_Miscellaneous).Color(colorCCR_Miscellaneous).FontSize(fontSizeCCR_Miscellaneous);
                    t.Rows[0].Cells[2].VerticalAlignment = Novacode.VerticalAlignment.Bottom;
                    t.Rows[0].Cells[3].Paragraphs.First().Append("TOTAL").Bold().Font(fontCCR_Miscellaneous).Color(colorCCR_Miscellaneous).FontSize(fontSizeCCR_Miscellaneous);
                    t.Rows[0].Cells[3].Paragraphs.First().Alignment = Alignment.right;
                    t.Rows[0].Cells[3].VerticalAlignment = Novacode.VerticalAlignment.Bottom;
                    t.Rows[0].TableHeader = true;

                    int i = 1;
                    double totalAmount = 0;
                    foreach (DataRow dataRow in misDT)
                    {
                        t.Rows[i].Height = 17;
                        t.Rows[i].Cells[0].Paragraphs.First().Append(dataRow["fdesc"].ToString()).Font(fontCCR_Miscellaneous).Color(colorCCR_Miscellaneous).FontSize(fontSizeCCR_Miscellaneous);
                        t.Rows[i].Cells[1].Paragraphs.First().Append(Convert.ToDecimal(dataRow["QtyReq"] == DBNull.Value ? "0" : dataRow["QtyReq"]).ToString()).Font(fontCCR_Miscellaneous).Color(colorCCR_Miscellaneous).FontSize(fontSizeCCR_Miscellaneous);
                        t.Rows[i].Cells[2].Paragraphs.First().Append(Convert.ToDecimal(dataRow["BudgetUnit"] == DBNull.Value ? "0" : dataRow["BudgetUnit"]).ToString()).Font(fontCCR_Miscellaneous).Color(colorCCR_Miscellaneous).FontSize(fontSizeCCR_Miscellaneous);
                        t.Rows[i].Cells[3].Paragraphs.First().Append(Convert.ToDecimal(dataRow["BudgetExt"] == DBNull.Value ? "0" : dataRow["BudgetExt"]).ToString("C")).Font(fontCCR_Miscellaneous).Color(colorCCR_Miscellaneous).FontSize(fontSizeCCR_Miscellaneous);
                        t.Rows[i].Cells[3].Paragraphs.First().Alignment = Alignment.right;
                        totalAmount += Convert.ToDouble(dataRow["BudgetExt"] == DBNull.Value ? "0" : dataRow["BudgetExt"]);
                        i++;
                    }

                    t.Rows[i].MergeCells(1, 2);
                    t.Rows[i].Cells[1].Paragraphs.First().Append("MISCELLANEOUS TOTAL:").Bold().FontSize(fontSizeCCR_Miscellaneous).Font(fontCCR_Miscellaneous).Color(colorCCR_Miscellaneous).FontSize(fontSizeCCR_Miscellaneous);
                    t.Rows[i].Cells[1].Paragraphs.First().Alignment = Alignment.right;
                    t.Rows[i].Cells[2].Paragraphs.First().Font(fontCCR_Miscellaneous).Append(totalAmount.ToString("C")).Bold().Font(fontCCR_Miscellaneous).Color(colorCCR_Miscellaneous).FontSize(fontSizeCCR_Miscellaneous);
                    t.Rows[i].Cells[2].Paragraphs.First().Alignment = Alignment.right;

                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Bottom, d);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[i].Cells[0].SetBorder(TableCellBorderType.Right, d);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Bottom, d);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[i].Cells[1].SetBorder(TableCellBorderType.Right, d);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Bottom, d);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Left, d);
                    t.Rows[i].Cells[2].SetBorder(TableCellBorderType.Right, d);

                    // Insert the Table into the document.
                    try
                    {
                        foreach (var paragraph in document.Paragraphs)
                        {
                            paragraph.FindAll(item).ForEach(index => paragraph.InsertTableAfterSelf((t)));
                        }
                        document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                    }
                    catch (Exception)
                    {
                        document.InsertTable(t);
                    }
                }
            }
            #endregion

            #endregion

            document.ReplaceText("{EstimateNo}", Convert.ToString(dr["EstimateNo"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{EstimateDate}", String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dr["EstimateDate"])), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{BidCloseDate}", String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dr["BidCloseDate"])), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Location}", Convert.ToString(dr["Location"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Email}", Convert.ToString(dr["Email"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Phone}", Convert.ToString(dr["Phone"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Mobile}", Convert.ToString(dr["Mobile"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Fax}", Convert.ToString(dr["Fax"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Desc}", Convert.ToString(dr["Desc"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{OpportunityName}", Convert.ToString(dr["OpportunityName"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{OpportunityNumber}", Convert.ToString(dr["OpportunityNumber"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{ContactName}", Convert.ToString(dr["ContactName"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Category}", Convert.ToString(dr["Category"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{CompanyName}", Convert.ToString(dr["CompanyName"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Remarks}", Convert.ToString(dr["Remarks"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{STax}", Convert.ToString(dr["STax"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Template}", Convert.ToString(dr["Template"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Department}", Convert.ToString(dr["Department"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{FinalBidPrice}", Convert.ToDecimal(dr["FinalBidPrice"] == DBNull.Value ? "0.00" : dr["FinalBidPrice"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{BidPrice}", Convert.ToDecimal(dr["BidPrice"] == DBNull.Value ? "0.00" : dr["BidPrice"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Profit}", Convert.ToDecimal(dr["MarkupVal"] == DBNull.Value ? "0.00" : dr["MarkupVal"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{STaxAmount}", Convert.ToDecimal(dr["STaxVal"] == DBNull.Value ? "0.00" : dr["STaxVal"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{MaterialExp}", Convert.ToDecimal(dr["MatExp"] == DBNull.Value ? "0.00" : dr["MatExp"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{LaborExp}", Convert.ToDecimal(dr["LabExp"] == DBNull.Value ? "0.00" : dr["LabExp"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{OtherExp}", Convert.ToDecimal(dr["OtherExp"] == DBNull.Value ? "0.00" : dr["OtherExp"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{SubTotal}", Convert.ToDecimal(dr["SubToalVal"] == DBNull.Value ? "0.00" : dr["SubToalVal"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{TotalCost}", Convert.ToDecimal(dr["TotalCostVal"] == DBNull.Value ? "0.00" : dr["TotalCostVal"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{PretaxTotal}", Convert.ToDecimal(dr["PretaxTotalVal"] == DBNull.Value ? "0.00" : dr["PretaxTotalVal"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Commission}", Convert.ToDecimal(dr["CommissionVal"] == DBNull.Value ? "0.00" : dr["CommissionVal"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{ProfitPercentage}", Convert.ToDecimal(dr["MarkupPer"] == DBNull.Value ? "0.00" : dr["MarkupPer"]).ToString("N"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{OHPercentage}", Convert.ToDecimal(dr["OHPer"] == DBNull.Value ? "0.00" : dr["OHPer"]).ToString("N"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{CommissionPercentage}", Convert.ToDecimal(dr["CommissionPer"] == DBNull.Value ? "0.00" : dr["CommissionPer"]).ToString("N"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{OHAmount}", Convert.ToDecimal(dr["Overhead"] == DBNull.Value ? "0.00" : dr["Overhead"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Contingencies}", Convert.ToDecimal(dr["Cont"] == DBNull.Value ? "0.00" : dr["Cont"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{TotalPrice}", Convert.ToDecimal(dr["BidPrice"] == DBNull.Value ? "0.00" : dr["BidPrice"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{CustomerSageID}", Convert.ToString(dr["CustomerSageID"]), false, RegexOptions.IgnoreCase);
            String CustAddress = Convert.ToString(dr["CustomerAddress"]);
            CustAddress = CustAddress.TrimEnd('\r', '\n');
            CustAddress = CustAddress.Replace("\r\n", "\n");
            document.ReplaceText("{CustomerAddress}", CustAddress, false, RegexOptions.IgnoreCase);
            document.ReplaceText("{CustomerZip}", Convert.ToString(dr["CustomerZip"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{CustomerState}", Convert.ToString(dr["CustomerState"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{CustomerCity}", Convert.ToString(dr["CustomerCity"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{CustomerFax}", Convert.ToString(dr["CustomerFax"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{LocationAcct}", Convert.ToString(dr["LocationAcct"]), false, RegexOptions.IgnoreCase);
            String LocationAddress = Convert.ToString(dr["LocationAddress"]);
            LocationAddress = LocationAddress.TrimEnd('\r', '\n');
            LocationAddress = LocationAddress.Replace("\r\n", "\n");
            document.ReplaceText("{LocationAddress}", LocationAddress, false, RegexOptions.IgnoreCase);
            document.ReplaceText("{LocationZip}", Convert.ToString(dr["LocationZip"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{LocationState}", Convert.ToString(dr["LocationState"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{LocationCity}", Convert.ToString(dr["LocationCity"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{LocationName}", Convert.ToString(dr["LocationName"]), false, RegexOptions.IgnoreCase);

            String LocBillingAddress = Convert.ToString(dr["LocBillingAddress"]);
            LocBillingAddress = LocBillingAddress.TrimEnd('\r', '\n');
            LocBillingAddress = LocBillingAddress.Replace("\r\n", "\n");
            document.ReplaceText("{LocBillingAddress}", LocBillingAddress, false, RegexOptions.IgnoreCase);
            document.ReplaceText("{LocBillingZip}", Convert.ToString(dr["LocBillingZip"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{LocBillingState}", Convert.ToString(dr["LocBillingState"]), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{LocBillingCity}", Convert.ToString(dr["LocBillingCity"]), false, RegexOptions.IgnoreCase);

            document.ReplaceText("{Taxable}", Convert.ToDecimal(dr["Taxable"] == DBNull.Value ? "0.00" : dr["Taxable"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{NonTaxable}", Convert.ToDecimal(dr["NonTaxable"] == DBNull.Value ? "0.00" : dr["NonTaxable"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{SubTotalNew}", Convert.ToDecimal(dr["SubTotal"] == DBNull.Value ? "0.00" : dr["SubTotal"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{ProjectDesc}", Convert.ToString(dr["ProjectDesc"]), false, RegexOptions.IgnoreCase);

            //{FinalBidPricetoText}
            //{BidAmountPricetoText}
            
            String strFinalBidPrice = "";
            strFinalBidPrice = new BL_Utility().ConvertToWordsNoCurrency1(Convert.ToDecimal(dr["FinalBidPrice"] == DBNull.Value ? "0" : dr["FinalBidPrice"]).ToString());
            document.ReplaceText("{FinalBidPricetoText}", strFinalBidPrice, false, RegexOptions.IgnoreCase);

            String strBidAmountPrice = "";
            strBidAmountPrice = new BL_Utility().ConvertToWordsNoCurrency1(Convert.ToDecimal(dr["BidPrice"] == DBNull.Value ? "0" : dr["BidPrice"]).ToString());
            document.ReplaceText("{BidAmountPricetoText}", strBidAmountPrice, false, RegexOptions.IgnoreCase);

            String strFinalBidPriceWithCurrency = "";
            strFinalBidPriceWithCurrency = new BL_Utility().ConvertToWords2(Convert.ToDecimal(dr["FinalBidPrice"] == DBNull.Value ? "0" : dr["FinalBidPrice"]).ToString());
            document.ReplaceText("{FinalBidPricetoTextC}", strFinalBidPriceWithCurrency, false, RegexOptions.IgnoreCase);

            String strBidAmountPriceWithCurrency = "";
            strBidAmountPriceWithCurrency = new BL_Utility().ConvertToWords2(Convert.ToDecimal(dr["BidPrice"] == DBNull.Value ? "0" : dr["BidPrice"]).ToString());
            document.ReplaceText("{BidAmountPricetoTextC}", strBidAmountPriceWithCurrency, false, RegexOptions.IgnoreCase);

            String strFinalBidPrice1 = "";
            strFinalBidPrice1 = new BL_Utility().ConvertToWordsNoCurrency(Convert.ToDecimal(dr["FinalBidPrice"] == DBNull.Value ? "0" : dr["FinalBidPrice"]).ToString());
            document.ReplaceText("{FinalBidPricetoText1}", strFinalBidPrice1, false, RegexOptions.IgnoreCase);

            String strBidAmountPrice1 = "";
            strBidAmountPrice1 = new BL_Utility().ConvertToWordsNoCurrency(Convert.ToDecimal(dr["BidPrice"] == DBNull.Value ? "0" : dr["BidPrice"]).ToString());
            document.ReplaceText("{BidAmountPricetoText1}", strBidAmountPrice1, false, RegexOptions.IgnoreCase);

            String strFinalBidPriceWithCurrency1 = "";
            strFinalBidPriceWithCurrency1 = new BL_Utility().ConvertToWords1(Convert.ToDecimal(dr["FinalBidPrice"] == DBNull.Value ? "0" : dr["FinalBidPrice"]).ToString());
            document.ReplaceText("{FinalBidPricetoTextC1}", strFinalBidPriceWithCurrency1, false, RegexOptions.IgnoreCase);

            String strBidAmountPriceWithCurrency1 = "";
            strBidAmountPriceWithCurrency1 = new BL_Utility().ConvertToWords1(Convert.ToDecimal(dr["BidPrice"] == DBNull.Value ? "0" : dr["BidPrice"]).ToString());
            document.ReplaceText("{BidAmountPricetoTextC1}", strBidAmountPriceWithCurrency1, false, RegexOptions.IgnoreCase);

            document.ReplaceText("{FinalBidPriceNoC}", Convert.ToDecimal(dr["FinalBidPrice"] == DBNull.Value ? "0.00" : dr["FinalBidPrice"]).ToString(), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{BidPriceNoC}", Convert.ToDecimal(dr["BidPrice"] == DBNull.Value ? "0.00" : dr["BidPrice"]).ToString(), false, RegexOptions.IgnoreCase);

            
            #region Estimate Status
            String statusName = "";
            var statusID = Convert.ToString(dr["EstimateStatus"]);
            if (statusID == "1")
            {
                statusName = "Open";
            }
            else if (statusID == "2")
            {
                statusName = "Canceled";
            }
            else if (statusID == "3")
            {
                statusName = "Withdrawn";
            }
            else if (statusID == "4")
            {
                statusName = "Disqualified";
            }
            else if (statusID == "5")
            {
                statusName = "Sold";
            }
            else if (statusID == "6")
            {
                statusName = "Competitor";
            }
            else
            {
                statusName = "Quoted";
            }
            document.ReplaceText("{EstimateStatus}", statusName, false, RegexOptions.IgnoreCase);
            #endregion

            #region Opportunity Stage
            leadData.ConnConfig = Session["config"].ToString();
            leadData.ID = Convert.ToInt32(dr["OpportunityStageID"] == DBNull.Value ? "0" : dr["OpportunityStageID"]);
            var dsOpportunityStage = objBL_Lead.GetStageByID(leadData);
            if (dsOpportunityStage.Tables[0] != null && dsOpportunityStage.Tables[0].Rows.Count > 0)
            {
                String OpportunityStage = Convert.ToString(dsOpportunityStage.Tables[0].Rows[0]["Description"]);
                document.ReplaceText("{OpportunityStage}", Convert.ToString(OpportunityStage), false, RegexOptions.IgnoreCase);
            }
            else
            {
                document.ReplaceText("{OpportunityStage}", "", false, RegexOptions.IgnoreCase);
            }

            #endregion

            #region Salesperson
            var salespersonText = document.FindAll("{Salesperson}");
            var salespersonFirstLastText = document.FindAll("{SalespersonFirstLast}");
            if ((salespersonText != null && salespersonText.Count > 0) || (salespersonFirstLastText != null && salespersonFirstLastText.Count > 0))
            {
                if (dr["SalespersonID"] == DBNull.Value || dr["SalespersonID"].ToString() == "0")
                {
                    document.ReplaceText("{Salesperson}", "", false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{SalespersonFirstLast}", "", false, RegexOptions.IgnoreCase);
                }
                else
                {
                    leadData.ConnConfig = Session["config"].ToString();
                    leadData.ID = Convert.ToInt32(dr["SalespersonID"]);
                    var dsSalesperson = objBL_Lead.GetSalespersonByID(leadData);
                    if (dsSalesperson.Tables[0] != null && dsSalesperson.Tables[0].Rows.Count > 0)
                    {
                        //String Salesperson = Convert.ToString(dsSalesperson.Tables[0].Rows[0]["fFirst"]) + " " + Convert.ToString(dsSalesperson.Tables[0].Rows[0]["lLast"]);
                        String Salesperson = Convert.ToString(dsSalesperson.Tables[0].Rows[0]["SDesc"]);
                        document.ReplaceText("{Salesperson}", Salesperson, false, RegexOptions.IgnoreCase);

                        String SalespersonFirstLast = Convert.ToString(dsSalesperson.Tables[0].Rows[0]["FirstLast"]);
                        document.ReplaceText("{SalespersonFirstLast}", SalespersonFirstLast, false, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        document.ReplaceText("{Salesperson}", "", false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{SalespersonFirstLast}", "", false, RegexOptions.IgnoreCase);
                    }
                }
            }
            #endregion

            #region Salesperson2
            var salesperson2 = dr["SalesPerson2"].ToString();
            document.ReplaceText("{Salesperson2}", salesperson2, false, RegexOptions.IgnoreCase);

            var salesperson2FirstLastText = document.FindAll("{Salesperson2FirstLast}");
            if (salesperson2FirstLastText != null && salesperson2FirstLastText.Count > 0)
            {
                if (dr["Salesperson2ID"] == DBNull.Value || dr["Salesperson2ID"].ToString() == "0")
                {
                    document.ReplaceText("{Salesperson2FirstLast}", "", false, RegexOptions.IgnoreCase);
                }
                else
                {
                    leadData.ConnConfig = Session["config"].ToString();
                    leadData.ID = Convert.ToInt32(dr["Salesperson2ID"]);
                    var dsSalesperson = objBL_Lead.GetSalespersonByID(leadData);
                    if (dsSalesperson.Tables[0] != null && dsSalesperson.Tables[0].Rows.Count > 0)
                    {
                        String Salesperson2FirstLast = Convert.ToString(dsSalesperson.Tables[0].Rows[0]["FirstLast"]);
                        document.ReplaceText("{Salesperson2FirstLast}", Salesperson2FirstLast, false, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        document.ReplaceText("{Salesperson2FirstLast}", "", false, RegexOptions.IgnoreCase);
                    }
                }
            }
            #endregion

            document.ReplaceText("{BillRate}", Convert.ToDecimal(dr["BillRate"] == DBNull.Value ? "0.00" : dr["BillRate"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{OTRate}", Convert.ToDecimal(dr["OT"] == DBNull.Value ? "0.00" : dr["OT"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{TravelRate}", Convert.ToDecimal(dr["RateTravel"] == DBNull.Value ? "0.00" : dr["RateTravel"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{17Rate}", Convert.ToDecimal(dr["RateNT"] == DBNull.Value ? "0.00" : dr["RateNT"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{DTRate}", Convert.ToDecimal(dr["DT"] == DBNull.Value ? "0.00" : dr["DT"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{MileageRate}", Convert.ToDecimal(dr["RateMileage"] == DBNull.Value ? "0.00" : dr["RateMileage"]).ToString("C"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{Amount}", Convert.ToDecimal(dr["Amount"] == DBNull.Value ? "0.00" : dr["Amount"]).ToString("C"), false, RegexOptions.IgnoreCase);

            document.ReplaceText("{YY}", DateTime.Now.ToString("yy"), false, RegexOptions.IgnoreCase);
            document.ReplaceText("{YYYY}", DateTime.Now.ToString("yyyy"), false, RegexOptions.IgnoreCase);

            #region Billing Type
            String billTypeName = "";
            var pType = Convert.ToString(dr["PType"]);
            if (pType == "0")
            {
                billTypeName = "None";
            }
            else if (pType == "1")
            {
                billTypeName = "Quoted";
            }
            else if (pType == "2")
            {
                billTypeName = "Maximum";
            }
            else
            {
                billTypeName = "None";
            }
            document.ReplaceText("{BilingType}", billTypeName, false, RegexOptions.IgnoreCase);

            #endregion

            #region Estimate Custom Field
            // Get all estimate custom field
            DataTable custFieldsDt = estimate.Tables[2];
            if (custFieldsDt != null)
            {
                foreach (DataRow item in custFieldsDt.Rows)
                {
                    var customTags = "{Custom_" + item["Label"].ToString() + "}";
                    var customValue = item["Value"] == DBNull.Value ? "" : item["Value"].ToString();
                    document.ReplaceText(customTags, customValue, false, RegexOptions.IgnoreCase);
                }
            }
            #endregion

            #region Estimate Equipment
            // Get all estimate custom field
            DataTable equip = estimate.Tables[6];
            if (equip != null && equip.Rows.Count > 0)
            {
                DataRow firstEquipment = equip.Rows[0];
                document.ReplaceText("{Equip.EquipmentID}", Convert.ToString(firstEquipment["EquipmentID"]), false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Manufacturer}", Convert.ToString(firstEquipment["Manufacturer"]), false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Description}", Convert.ToString(firstEquipment["Description"]), false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Type}", Convert.ToString(firstEquipment["Type"]), false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.ServiceType}", Convert.ToString(firstEquipment["ServiceType"]), false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Category}", Convert.ToString(firstEquipment["Category"]), false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Building}", Convert.ToString(firstEquipment["Building"]), false, RegexOptions.IgnoreCase);
                //document.ReplaceText("{Equip.Status}", Convert.ToString(firstEquipment["Status"]), false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Classification}", Convert.ToString(firstEquipment["Classification"]), false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.SerialNo}", Convert.ToString(firstEquipment["SerialNo"]), false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.UniqueNo}", Convert.ToString(firstEquipment["UniqueNo"]), false, RegexOptions.IgnoreCase);
                try
                {
                    var installedDate = Convert.ToDateTime(firstEquipment["Installed"]);
                    document.ReplaceText("{Equip.Installed}", String.Format("{0:MM/dd/yyyy}", installedDate), false, RegexOptions.IgnoreCase);
                }
                catch (Exception)
                {
                    document.ReplaceText("{Equip.Installed}", Convert.ToString(firstEquipment["Installed"]), false, RegexOptions.IgnoreCase);
                }

                try
                {
                    var serviceSinceDate = Convert.ToDateTime(firstEquipment["ServiceSince"]);
                    document.ReplaceText("{Equip.ServiceSince}", String.Format("{0:MM/dd/yyyy}", serviceSinceDate), false, RegexOptions.IgnoreCase);
                }
                catch (Exception)
                {
                    document.ReplaceText("{Equip.ServiceSince}", Convert.ToString(firstEquipment["ServiceSince"]), false, RegexOptions.IgnoreCase);
                }

                try
                {
                    var lastServiceDate = Convert.ToDateTime(firstEquipment["LastService"]);
                    document.ReplaceText("{Equip.LastService}", String.Format("{0:MM/dd/yyyy}", lastServiceDate), false, RegexOptions.IgnoreCase);
                }
                catch (Exception)
                {
                    document.ReplaceText("{Equip.LastService}", Convert.ToString(firstEquipment["LastService"]), false, RegexOptions.IgnoreCase);
                }
                document.ReplaceText("{Equip.Price}", Convert.ToString(firstEquipment["Price"]), false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Remarks}", Convert.ToString(firstEquipment["Remarks"]), false, RegexOptions.IgnoreCase);

                // Check case an estimate have many Equipment
                string[] equipTags = { "EquipmentID", "Manufacturer", "Description", "Type", "ServiceType", "Category"
                        ,"Category","Building","Classification","SerialNo", "UniqueNo"
                        ,"Installed","ServiceSince","LastService","Price","Remarks" };
                int i = 0;
                foreach (DataRow item in equip.Rows)
                {
                    // By Equipment ID
                    foreach (string tag in equipTags)
                    {
                        var equipTag = "{" + string.Format("Equip[{0}].{1}", item["EquipmentID"].ToString(), tag) + "}";
                        document.ReplaceText(equipTag, item[tag].ToString(), false, RegexOptions.IgnoreCase);
                    }

                    // By Equipment Index
                    foreach (string tag in equipTags)
                    {
                        var equipTag = "{" + string.Format("Equip[{0}].{1}", i, tag) + "}";
                        document.ReplaceText(equipTag, item[tag].ToString(), false, RegexOptions.IgnoreCase);
                    }

                    var templateId = item["Template"].ToString();
                    if(templateId != "" && templateId != "0")
                    {
                        // Get custom template and value
                        var equipmentId = Convert.ToInt32(item["ID"]);
                        var dsCust = objBL_Customer.GetCustomValuesOfEquip(Session["config"].ToString(), equipmentId);
                        var j = 0;
                        foreach (DataRow cust in dsCust.Tables[0].Rows)
                        {
                            var custEquipTag = "{" + string.Format("Equip[{0}].CustomLine[{1}]", i, j) + "}";
                            document.ReplaceText(custEquipTag, cust["Value"].ToString(), false, RegexOptions.IgnoreCase);
                            j++;
                        }
                    }
                    i++;
                }

                // Replace all equipment tags in case it not exist
                foreach (var tag in equipTags)
                {
                    var pattern = @"\{Equip\[[0-9]*\]." + tag + @"\}";
                    List<string> temp = document.FindUniqueByPattern(pattern, RegexOptions.Singleline);
                    if (temp.Count > 0)
                    {
                        foreach (var item in temp)
                        {
                            document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                        }
                    }
                }

                var patternCustomLine = @"\{Equip\[[0-9]*\].CustomLine\[[0-9]*\]\}";
                List<string> tempCustomline = document.FindUniqueByPattern(patternCustomLine, RegexOptions.Singleline);
                if (tempCustomline.Count > 0)
                {
                    foreach (var item in tempCustomline)
                    {
                        document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                    }
                }
            }
            else
            {
                document.ReplaceText("{Equip.EquipmentID}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Manufacturer}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Description}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Type}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.ServiceType}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Category}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Building}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Classification}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.SerialNo}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.UniqueNo}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Installed}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.ServiceSince}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.LastService}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Price}", "", false, RegexOptions.IgnoreCase);
                document.ReplaceText("{Equip.Remarks}", "", false, RegexOptions.IgnoreCase);

                string[] equipTags = { "EquipmentID", "Manufacturer", "Description", "Type", "ServiceType", "Category"
                        ,"Category","Building","Classification","SerialNo", "UniqueNo"
                        ,"Installed","ServiceSince","LastService","Price","Remarks" };

                foreach (var tag in equipTags)
                {
                    var pattern = @"\{Equip\[[0-9]*\]." + tag + @"\}";
                    List<string> temp = document.FindUniqueByPattern(pattern, RegexOptions.Singleline);
                    if (temp.Count > 0)
                    {
                        foreach (var item in temp)
                        {
                            document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                        }
                    }
                }

                var patternCustomLine = @"\{Equip\[[0-9]*\].CustomLine\[[0-9]*\]\}";
                List<string> tempCustomline = document.FindUniqueByPattern(patternCustomLine, RegexOptions.Singleline);
                if (tempCustomline.Count > 0)
                {
                    foreach (var item in tempCustomline)
                    {
                        document.ReplaceText(item, "", false, RegexOptions.IgnoreCase);
                    }
                }
            }

            #endregion

            #region Proposal Dates
            var proposalCreateDateText = document.FindAll("{ProposalCreateDate}");
            var proposalCreateDatetoWordsText = document.FindAll("{ProposalCreateDatetoWords}");
            var proposalRevisedDateText = document.FindAll("{ProposalRevisedDate}");
            var proposalRevisedDatetoWordsText = document.FindAll("{ProposalRevisedDatetoWords}");
            var proposalCreateDatewithRevisedDatetoWordsText = document.FindAll("{ProposalCreateDatewithRevisedDatetoWords}");

            if ((proposalCreateDateText != null && proposalCreateDateText.Count > 0) 
                || (proposalCreateDatetoWordsText != null && proposalCreateDatetoWordsText.Count > 0)
                || (proposalRevisedDateText != null && proposalRevisedDateText.Count > 0)
                || (proposalRevisedDatetoWordsText != null && proposalRevisedDatetoWordsText.Count > 0)
                || (proposalCreateDatewithRevisedDatetoWordsText != null && proposalCreateDatewithRevisedDatetoWordsText.Count > 0)
                )
            {
                if (Request.QueryString["uid"] != null)
                {
                    objEF.Estimate = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    objEF.ConnConfig = Session["config"].ToString();
                    DataSet dsForm = new DataSet();
                    dsForm = objBL_EF.GetEstimateFormsByEstimateId(objEF);

                    DataTable dtForm = new DataTable();
                    dtForm = dsForm.Tables[0];
                    int rowCount = dtForm.Rows.Count;

                    DateTime? proposalCreateDate = null;
                    DateTime? proposalRevisedDate = null;
                    // Set the first record as a latest
                    if (rowCount >= 1)
                    {
                        if (dtForm.Rows[rowCount - 1]["AddedOn"] != null && dtForm.Rows[rowCount - 1]["AddedOn"].ToString() != "")
                            proposalCreateDate = Convert.ToDateTime(dtForm.Rows[rowCount - 1]["AddedOn"].ToString());

                        proposalRevisedDate = DateTime.Now;
                    }
                    else
                    {
                        proposalCreateDate = DateTime.Now;
                        proposalRevisedDate = DateTime.Now;
                    }

                    //else if(rowCount > 1)
                    //{
                    //    if (dtForm.Rows[rowCount - 1]["AddedOn"] != null && dtForm.Rows[rowCount - 1]["AddedOn"].ToString() != "")
                    //        proposalCreateDate = Convert.ToDateTime(dtForm.Rows[rowCount - 1]["AddedOn"].ToString());

                    //    if (dtForm.Rows[0]["AddedOn"] != null && dtForm.Rows[0]["AddedOn"].ToString() != "")
                    //        proposalRevisedDate = Convert.ToDateTime(dtForm.Rows[0]["AddedOn"].ToString());
                    //}

                    if (proposalCreateDate.HasValue)
                    {
                        document.ReplaceText("{ProposalCreateDate}", String.Format("{0:MM/dd/yyyy}", proposalCreateDate.Value), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ProposalCreateDatetoWords}", String.Format("{0:MMMM dd, yyyy}", proposalCreateDate.Value), false, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        document.ReplaceText("{ProposalCreateDate}", "", false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ProposalCreateDatetoWords}", "", false, RegexOptions.IgnoreCase);
                    }

                    if (proposalRevisedDate.HasValue)
                    {
                        document.ReplaceText("{ProposalRevisedDate}", String.Format("{0:MM/dd/yyyy}", proposalRevisedDate.Value), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ProposalRevisedDatetoWords}", String.Format("{0:MMMM dd, yyyy}", proposalRevisedDate.Value), false, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        document.ReplaceText("{ProposalRevisedDate}", "", false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ProposalRevisedDatetoWords}", "", false, RegexOptions.IgnoreCase);
                    }

                    if (proposalRevisedDate.HasValue && proposalCreateDate.HasValue && proposalRevisedDate.Value != proposalCreateDate.Value)
                    {
                        document.ReplaceText("{ProposalCreateDatewithRevisedDatetoWords}", String.Format("{0:MMMM dd, yyyy}", proposalCreateDate.Value) +" - Revised "+ String.Format("{0:MMMM dd, yyyy}", proposalRevisedDate.Value), false, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        document.ReplaceText("{ProposalCreateDatewithRevisedDatetoWords}", "", false, RegexOptions.IgnoreCase);
                    }

                }
            }

                
            #endregion

            //document.ReplaceText("{field}", "This is value of Field 2", false, RegexOptions.IgnoreCase);
            document.Save();
        }
        #endregion

        #region Convert Docx file into PDF
        //Free version of Spire.Doc has limitations of first three pages more details at https://www.e-iceblue.com/Introduce/free-doc-component.html
        Document doc = new Document();
        doc.LoadFromFile(ef.FilePath);

        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
        ds = objBL_Customer.GetDescAndAmountOfEstimateByID(objProp_Customer);

        //Create word table for tag fullDescandAmount
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        TextSelection selection = doc.FindString("{fullDescandAmount}", true, true);
        if (dt.Rows.Count > 0 && selection != null)
        {
            //Create table contain Description and Amount
            Spire.Doc.Section s = doc.Sections[0];
            Spire.Doc.Table table = s.AddTable(true);

            String[] Header = { };
            List<string> header = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                header.Add(dc.ColumnName);
            }
            Header = header.ToArray();
            List<BillingData> data = new List<BillingData>();
            foreach (DataRow drow in dt.Rows)
            {
                data.Add(new BillingData { MilesName = drow.ItemArray[0].ToString(), Desc = drow.ItemArray[1].ToString(), Amount = Convert.ToDecimal(drow.ItemArray[2]).ToString("C") });
            }


            table.ResetCells(data.ToArray().Length + 1, Header.Length);
            Spire.Doc.TableRow FRow = table.Rows[0];
            FRow.IsHeader = true;
            FRow.Height = 23;

            for (int i = 0; i < Header.Length; i++)
            {
                Spire.Doc.Documents.Paragraph p = FRow.Cells[i].AddParagraph();
                FRow.Cells[i].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Bottom;
                p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                Spire.Doc.Fields.TextRange TR = p.AppendText(Header[i]);
                TR.CharacterFormat.Bold = true;
            }
            FRow.Cells[0].Width = table.Width / 4;
            FRow.Cells[1].Width = table.Width / 2;
            FRow.Cells[2].Width = table.Width / 4;
            for (int r = 0; r < data.ToArray().Length; r++)
            {
                Spire.Doc.TableRow DataRow = table.Rows[r + 1];
                DataRow.Height = 20;
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    DataRow.Cells[c].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
                    Spire.Doc.Documents.Paragraph p2 = DataRow.Cells[c].AddParagraph();
                    if (dt.Columns[c].ToString() == "Name")
                    {
                        Spire.Doc.Fields.TextRange TR1 = p2.AppendText(data[r].MilesName);
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                    else if (dt.Columns[c].ToString() == "Description")
                    {
                        Spire.Doc.Fields.TextRange TR2 = p2.AppendText(data[r].Desc);
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                    else if (dt.Columns[c].ToString() == "Amount")
                    {
                        Spire.Doc.Fields.TextRange TR3 = p2.AppendText(data[r].Amount);
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                }
            }

            Spire.Doc.Fields.TextRange range = selection.GetAsOneRange();
            Spire.Doc.Documents.Paragraph paragraph = range.OwnerParagraph;
            Body body = paragraph.OwnerTextBody;
            int index = body.ChildObjects.IndexOf(paragraph);

            body.ChildObjects.Remove(paragraph);
            body.ChildObjects.Insert(index, table);


        }
        else
        {
            doc.Replace("{fullDescandAmount}", "", false, true);
        }

        //Create word table for tag DescandAmount
        DataTable dtDesAmt = new DataTable();
        dt.Columns.Remove("Name");
        dtDesAmt = dt;
        TextSelection tSelection = doc.FindString("{DescandAmount}", true, true);
        if (dtDesAmt.Rows.Count > 0 && tSelection != null)
        {
            //Create table contain Description and Amount
            Spire.Doc.Section s = doc.Sections[0];
            Spire.Doc.Table table = s.AddTable(true);

            String[] Header = { };
            List<string> header = new List<string>();
            foreach (DataColumn dc in dtDesAmt.Columns)
            {
                header.Add(dc.ColumnName);
            }
            Header = header.ToArray();
            List<BillingDataDescAndAmount> data = new List<BillingDataDescAndAmount>();
            foreach (DataRow drow in dtDesAmt.Rows)
            {
                data.Add(new BillingDataDescAndAmount { Desc = drow.ItemArray[0].ToString(), Amount = Convert.ToDecimal(drow.ItemArray[1]).ToString("C") });
            }


            table.ResetCells(data.ToArray().Length + 1, Header.Length);
            Spire.Doc.TableRow FRow = table.Rows[0];
            FRow.IsHeader = true;
            FRow.Height = 23;

            for (int i = 0; i < Header.Length; i++)
            {
                Spire.Doc.Documents.Paragraph p = FRow.Cells[i].AddParagraph();
                FRow.Cells[i].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Bottom;
                p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                Spire.Doc.Fields.TextRange TR = p.AppendText(Header[i]);
                TR.CharacterFormat.Bold = true;
            }

            for (int r = 0; r < data.ToArray().Length; r++)
            {
                Spire.Doc.TableRow DataRow = table.Rows[r + 1];
                DataRow.Height = 20;
                for (int c = 0; c < dtDesAmt.Columns.Count; c++)
                {
                    DataRow.Cells[c].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
                    Spire.Doc.Documents.Paragraph p2 = DataRow.Cells[c].AddParagraph();
                    if (dtDesAmt.Columns[c].ToString() == "Description")
                    {
                        Spire.Doc.Fields.TextRange TR2 = p2.AppendText(data[r].Desc);
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                    else if (dtDesAmt.Columns[c].ToString() == "Amount")
                    {
                        Spire.Doc.Fields.TextRange TR3 = p2.AppendText(data[r].Amount);
                        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    }
                }
            }


            Spire.Doc.Fields.TextRange range = tSelection.GetAsOneRange();
            Spire.Doc.Documents.Paragraph paragraph = range.OwnerParagraph;
            Body body = paragraph.OwnerTextBody;
            int index = body.ChildObjects.IndexOf(paragraph);

            body.ChildObjects.Remove(paragraph);
            body.ChildObjects.Insert(index, table);
        }
        else
        {
            doc.Replace("{DescandAmount}", "", false, true);
        }


        doc.SaveToFile(ef.FilePath, FileFormat.Docx);
        doc.SaveToFile(ef.PdfFilePath, FileFormat.PDF);

        #endregion

        ef.Id = 0;
        ef.UpdatedBy = Session["Username"].ToString();
        ef.UpdatedOn = DateTime.Now;
        objBL_EF.AddEstimateForm(ef);
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

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = System.Text.Encoding.UTF8;

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

    protected void btnGenerateTemplete_Click(object sender, EventArgs e)
    {
        try
        {
            string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
            string savepath = savepathconfig + @"\" + Session["dbname"] + @"\EstimateForms\";
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }


            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
            DataSet estimate = objBL_Customer.GetEstimateOpportunityByEstimateID(objProp_Customer);

            objEF.ConnConfig = Session["config"].ToString();
            objET.ConnConfig = Session["config"].ToString();
            objET.JobTID = Convert.ToInt32("0" + ddlTemplate.SelectedValue.ToString());

            objEF.JobTID = objET.JobTID;
            objEF.Estimate = Convert.ToInt32(Request.QueryString["uid"].ToString());
            DataSet ds = objBL_ET.GetEstimateFormsByJobTId(objET);

            //foreach (GridViewRow row in gvEstimateTemplate.Rows)
            foreach (GridDataItem row in gvEstimateTemplate.Items)
            {
                //if (row.ItemType == DataControlRowType.DataRow)
                if (row is GridDataItem)
                {
                    CheckBox chkSelected = row.FindControl("chkSelect") as CheckBox;
                    if (chkSelected.Checked == true)
                    {
                        HiddenField hdID = row.FindControl("hdID") as HiddenField;
                        Int32 _ID = Convert.ToInt32(hdID.Value);
                        DataRow dr = ds.Tables[0].Rows.Cast<DataRow>().Where(x => x.Field<int>("ID") == _ID).FirstOrDefault();

                        #region Create File
                        objBL_ET.PopulateFields(objET, dr);
                        objEF.FileName = objET.FileName;
                        objEF.Name = objET.Name;
                        GenerateForms(estimate, objET, objEF, savepath);
                        #endregion
                    }
                }
            }

            //To ReBind the Form GridView.
            GetEstimateForms();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {

        #region Get Estimate Templates
        objEF.Estimate = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objEF.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_EF.GetEstimateFormsByEstimateId(objEF);
        #endregion

        List<BusinessEntity.MailSender> tableList = new List<BusinessEntity.MailSender>();

        foreach (GridDataItem item in RadGrid_Forms.SelectedItems)
        {
            HiddenField hdID = item.FindControl("hdID") as HiddenField;
            DataRow dr = ds.Tables[0].Rows.Cast<DataRow>().Where(x => x.Field<int>("ID") == Convert.ToInt32(hdID.Value)).FirstOrDefault();

            #region Data
            BusinessEntity.MailSender data = new MailSender();
            data.ID = Convert.ToInt32(dr["ID"]);
            data.Name = Convert.ToString(dr["Name"]);
            String fileName = Convert.ToString(dr["FileName"]);
            String addedOn = Convert.ToString(dr["AddedOn"]);
            String estimateNo = Convert.ToString(dr["Estimate"]);
            bool manualUpload = Convert.ToString(dr["JobTID"]) == "0";

            //String newFileName = fileName.Split('.').FirstOrDefault() + "-" + estimateNo + "-" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dr["AddedOn"])).Replace("/", "") + "." + fileName.Split('.').LastOrDefault();
            string strFileEx = fileName.Split('.').Length > 1 ? "." + fileName.Split('.').LastOrDefault() : "";
            string newFileName = fileName.Replace(strFileEx, "") + "-" + estimateNo + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(dr["AddedOn"])) + ".pdf";
            if (manualUpload)
            {
                data.FileName = fileName;
                data.PDFFilePath = Convert.ToString(dr["FilePath"]);
            }
            else
            {
                data.FileName = newFileName;
                data.PDFFilePath = Convert.ToString(dr["PdfFilePath"]);
            }
            tableList.Add(data);
            #endregion
        }

        if (tableList == null || tableList.Count <= 0)
        {
            //lblError.Text = "Please select a row";

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: 'Please select a proposal for sending email',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        else
        {
            Session["SelectedEstimateTemplate"] = null;
            Session["SelectedEstimateTemplate"] = tableList;
            Session["SelectedEstimate_Email"] = txtEmail.Text;
            
            if (!string.IsNullOrEmpty(txtContact.Text))
            {
                Session["SelectedEstimate_Contact"] = txtContact.Text + ":";
            }

            Response.Redirect("EmailSender.aspx?uid=" + Request.QueryString["uid"]);
        }


    }

    protected void lnkDeleteForms_Click(object sender, EventArgs e)
    {
        objEF.ConnConfig = Session["config"].ToString();
        

        foreach (GridDataItem item in RadGrid_Forms.SelectedItems)
        {
            HiddenField hdID = (HiddenField)item.FindControl("hdID");
            objEF.Id = Convert.ToInt32(hdID.Value);
            objBL_EF.DeleteEstimateForm(objEF);
        }

        GetEstimateForms();
    }

    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        //FillLocInfo();


        if (!string.IsNullOrEmpty(hdnLocID.Value) && hdnLocID.Value != "0")
        {
            _objUser.DBName = Session["dbname"].ToString();
            _objUser.ConnConfig = Session["config"].ToString();
            _objUser.LocID = Convert.ToInt32(hdnLocID.Value);
            _objUser.ConnConfig = Session["config"].ToString();
            DataSet dc = new DataSet();
            dc = objBL_User.GetLocInfoForEstimateByID(_objUser);
            if (dc.Tables[0].Rows.Count > 0)
            {
                txtCompany.Text = dc.Tables[0].Rows[0]["Company"].ToString();
                string str = dc.Tables[0].Rows[0]["terr"].ToString();

                if (!string.IsNullOrEmpty(str) && str != "0")
                {
                    SetSelectedValueForDDL(ddlEmployees, str);
                }

                txtPhone.Text = dc.Tables[0].Rows[0]["Phone"].ToString();
                txtEmail.Text = dc.Tables[0].Rows[0]["EMail"].ToString();
                txtFax.Text = dc.Tables[0].Rows[0]["Fax"].ToString();
                txtCellNew.Text = dc.Tables[0].Rows[0]["Cellular"].ToString();
                txtContact.Text = dc.Tables[0].Rows[0]["Contact"].ToString();
                //Set Hyperlink  For Loc / Customer
                if (hdnLocID.Value != "0")
                {

                    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;
                    GetDataEquip();
                }
                else
                    lnkLocationID.NavigateUrl = "";

                if (hdnCustId.Value != "0")
                    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustId.Value;
                else
                    lnkCustomerID.NavigateUrl = "";

            }

        }
        else
        {
            BL_User objBL_User = new BL_User();
            Rol objrol = new Rol();
            objrol.ID = string.IsNullOrEmpty(hdnROLId.Value) ? 0 : Convert.ToInt32(hdnROLId.Value);
            DataSet dc = objBL_User.GetEstimateRoleSpecificDetails(objrol);
            if (dc.Tables[0].Rows.Count > 0)
            {
                txtPhone.Text = dc.Tables[0].Rows[0]["Phone"].ToString();
                txtEmail.Text = dc.Tables[0].Rows[0]["EMail"].ToString();
                txtFax.Text = dc.Tables[0].Rows[0]["Fax"].ToString();
                txtCellNew.Text = dc.Tables[0].Rows[0]["Cellular"].ToString();
                txtContact.Text = dc.Tables[0].Rows[0]["Contact"].ToString();

            }

            if (dc.Tables[3].Rows.Count > 0)
            {
                string str = dc.Tables[3].Rows[0]["terr"].ToString();
                if (!string.IsNullOrEmpty(str) && str != "0")
                {
                    SetSelectedValueForDDL(ddlEmployees, str);
                }
                txtCompany.Text = dc.Tables[3].Rows[0]["Company"].ToString();
            }
        }
        if (!string.IsNullOrEmpty(hdnROLId.Value) && hdnROLId.Value != "0")
        {
            FillOpportunityDropdown(Convert.ToInt32(hdnROLId.Value));
            FillGroupNameDropdown(Convert.ToInt32(hdnROLId.Value));
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateGroupPanel", "ShowHideGroupEquipments(true);", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateGroupPanel", "ShowHideGroupEquipments(false);", true);
        }

        if (ddlEstimateGroup.SelectedValue != "0" && ddlEstimateGroup.SelectedValue != "")
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateEquimentPanel", "ShowHideEquipments(true);", true);
        else
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateEquimentPanel", "ShowHideEquipments(false);", true);
        // reset opportunity value
        hdnOpportunity.Value = ddlOpportunity.SelectedValue;
        if (string.IsNullOrEmpty(hdnOpportunity.Value) || hdnOpportunity.Value == "0")
        {
            txtOppName.Enabled = true;
        }
        else
        {
            txtOppName.Enabled = false;
        }
        if (!string.IsNullOrEmpty(hdnROLId.Value) && hdnROLId.Value != "0")
        {
            lnkAddnewContact.Visible = true;
            btnEditContact.Visible = true;
            btnDeleteContact.Visible = true;
        }
        else
        {
            lnkAddnewContact.Visible = false;
            btnEditContact.Visible = false;
            btnDeleteContact.Visible = false;
        }

        var template = ddlTemplate.SelectedValue;
        FillTemplateDropdown(0, 0, template);
        //FillTemplateDropdown(0, 0, hdnSelectedTemplate.Value);
        GetDataEquipmentForGrid();
        RadGrid_Contacts.Rebind();
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "rebindContactChange", "BindContactChange();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    private void GetDataEquip()
    {

    }
    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdProspect.Value) && hdProspect.Value != "0")// Prospect
        {
            BL_User objBL_User = new BL_User();
            Rol objrol = new Rol();
            objrol.ID = string.IsNullOrEmpty(hdnROLId.Value) ? 0 : Convert.ToInt32(hdnROLId.Value);
            DataSet dc = objBL_User.GetEstimateRoleSpecificDetails(objrol);
            if (dc.Tables[0].Rows.Count > 0)
            {
                txtPhone.Text = dc.Tables[0].Rows[0]["Phone"].ToString();
                txtEmail.Text = dc.Tables[0].Rows[0]["EMail"].ToString();
                txtFax.Text = dc.Tables[0].Rows[0]["Fax"].ToString();
                txtCellNew.Text = dc.Tables[0].Rows[0]["Cellular"].ToString();
                txtContact.Text = dc.Tables[0].Rows[0]["Contact"].ToString();

                if (hdnLocID.Value != "0")
                {
                    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;
                    GetDataEquip();
                }
                else
                    lnkLocationID.NavigateUrl = "";

                if (hdnCustId.Value != "0")
                    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustId.Value;
            }

            if (dc.Tables[3].Rows.Count > 0)
            {
                string str = dc.Tables[3].Rows[0]["terr"].ToString();

                if (!string.IsNullOrEmpty(str) && str != "0")
                {
                    SetSelectedValueForDDL(ddlEmployees, str);
                }
                txtCompany.Text = dc.Tables[3].Rows[0]["Company"].ToString();
            }
        }
        else // Location
        {
            User objPropUser = new User();
            BL_User objBL_User = new BL_User();
            objPropUser.SearchValue = "";
            objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objPropUser.CustomerID = Convert.ToInt32(hdnCustId.Value);

            DataSet ds = new DataSet();
            ds = objBL_User.getLocationAutojquery(objPropUser);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    //dictionary.Add("CheckUniqueRow", "1");
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        hdnLocID.Value = ds.Tables[0].Rows[0]["value"].ToString();
                        txtCont.Text = ds.Tables[0].Rows[0]["label"].ToString();
                        _objUser.DBName = Session["dbname"].ToString();
                        _objUser.ConnConfig = Session["config"].ToString();
                        _objUser.LocID = Convert.ToInt32(hdnLocID.Value);
                        _objUser.ConnConfig = Session["config"].ToString();
                        DataSet dc = new DataSet();
                        dc = objBL_User.GetLocInfoForEstimateByID(_objUser);
                        if (dc.Tables[0].Rows.Count > 0)
                        {
                            txtCompany.Text = dc.Tables[0].Rows[0]["Company"].ToString();
                            string str = dc.Tables[0].Rows[0]["terr"].ToString();

                            if (!string.IsNullOrEmpty(str) && str != "0")
                            {
                                SetSelectedValueForDDL(ddlEmployees, str);
                            }

                            txtPhone.Text = dc.Tables[0].Rows[0]["Phone"].ToString();
                            txtEmail.Text = dc.Tables[0].Rows[0]["EMail"].ToString();
                            txtFax.Text = dc.Tables[0].Rows[0]["Fax"].ToString();
                            txtCellNew.Text = dc.Tables[0].Rows[0]["Cellular"].ToString();
                            txtContact.Text = dc.Tables[0].Rows[0]["Contact"].ToString();
                        }
                        if (hdnLocID.Value != "0")
                        {
                            lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;
                            GetDataEquip();
                        }
                        else
                            lnkLocationID.NavigateUrl = "";

                        if (hdnCustId.Value != "0")
                            lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustId.Value;
                    }
                    else
                    {
                        hdnLocID.Value = string.Empty;
                        hdnROLId.Value = string.Empty;
                        txtCont.Text = string.Empty;
                        txtPhone.Text = string.Empty;
                        txtEmail.Text = string.Empty;
                        txtFax.Text = string.Empty;
                        txtCellNew.Text = string.Empty;
                        txtContact.Text = string.Empty;
                        //ddlContact.DataSource = null;
                        //ddlContact.DataBind();
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(hdnROLId.Value) && hdnROLId.Value != "0")
        {
            FillOpportunityDropdown(Convert.ToInt32(hdnROLId.Value));
            FillGroupNameDropdown(Convert.ToInt32(hdnROLId.Value));
            //divGroupEquipments.Visible = true;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateGroupPanel", "ShowHideGroupEquipments(true);", true);
        }
        else
        {
            //divGroupEquipments.Visible = false;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateGroupPanel", "ShowHideGroupEquipments(false);", true);
        }
        if (ddlEstimateGroup.SelectedValue != "0" && ddlEstimateGroup.SelectedValue != "")
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateEquimentPanel", "ShowHideEquipments(true);", true);
        else
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateEquimentPanel", "ShowHideEquipments(false);", true);

        // reset opportunity value
        hdnOpportunity.Value = ddlOpportunity.SelectedValue;
        if (string.IsNullOrEmpty(hdnOpportunity.Value) || hdnOpportunity.Value == "0")
        {
            txtOppName.Enabled = true;
        }
        else
        {
            txtOppName.Enabled = false;
        }
        if (!string.IsNullOrEmpty(hdnROLId.Value) && hdnROLId.Value != "0")
        {
            lnkAddnewContact.Visible = true;
            btnEditContact.Visible = true;
            btnDeleteContact.Visible = true;
        }
        else
        {
            lnkAddnewContact.Visible = false;
            btnEditContact.Visible = false;
            btnDeleteContact.Visible = false;
        }
        var template = ddlTemplate.SelectedValue;
        FillTemplateDropdown(0, 0, template);
        //FillTemplateDropdown(0, 0, hdnSelectedTemplate.Value);
        GetDataEquipmentForGrid();
        RadGrid_Contacts.Rebind();
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "rebindContactChange", "BindContactChange();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        txtContcName.Text = "";
        txtTitle.Text = "";
        txtContPhone.Text = "";
        txtContFax.Text = "";
        txtContCell.Text = "";
        txtContEmail.Text = "";

        string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void ibDeleteBom_Click(object sender, EventArgs e)
    {
        try
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "removeLine('" + gvBOM.ClientID + "')", true);\
            List<int> listItemDelete = new List<int>();
            foreach (GridDataItem gr in gvBOM.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked.Equals(true))
                {
                    Label lblLine = gr.FindControl("lblLine") as Label;
                    
                    listItemDelete.Add(Convert.ToInt32(lblLine.Text));
                    DeleteGridItem(listItemDelete, "Bom");
                }
            }

            DeleteGridItem(listItemDelete, "Bom");

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateSummaryTable", "CalculateEstimateBOM();UpdateProfit();", true);

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Estimate"];
            string url = "addestimate.aspx?uid=" + dt.Rows[0]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Estimate"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addestimate.aspx?uid=" + dt.Rows[index - 1]["ID"];
                Response.Redirect(url);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Estimate"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addestimate.aspx?uid=" + dt.Rows[index + 1]["ID"];
                Response.Redirect(url);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["Estimate"];
            string url = "addestimate.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
    }

    protected void RadGrid_Revision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
            DataSet ds = objBL_Customer.RevisionNotesByEstimate(objProp_Customer);
            RadGrid_Revision.DataSource = ds;
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow lastRow = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1];
                ViewState["RevisionVersion"] = Convert.ToString(lastRow["Version"]);
            }
        }
    }

    protected void lnkRevisionSave_Click(object sender, EventArgs e)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.RevisionNotes = txtRevisionNotes.Text;
        objProp_Customer.RevisionCreated = DateTime.Now;
        objProp_Customer.RevisionUser = Convert.ToString(Session["username"]);
        String Version = "";
        if (Convert.ToString(ViewState["RevisionVersion"]) == "")
        {
            Version = "1.0";
        }
        else
        {
            Version = Convert.ToString(ViewState["RevisionVersion"]);
            Version = Convert.ToString(Math.Round((Convert.ToDecimal(Version) + Convert.ToDecimal(".10")), 1));
        }
        objProp_Customer.RevisionVersion = Version;
        objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
        txtRevisionNotes.Text = "";
        objBL_Customer.AddRevisionNotes(objProp_Customer);
        RadGrid_Revision.Rebind();
    }

    public String BomItems(string MatItem, string Desc)
    {
        if (MatItem == "" || MatItem == "0")
        {
            return "";
        }
        else
        {
            return MatItem + ", " + Desc;
        }

    }

    #region Normal get/set object
    public class BillingData
    {
        public string Desc { get; set; }
        public string Amount { get; set; }
        public string MilesName { get; set; }
    }

    public class BillingDataDescAndAmount
    {
        public string Desc { get; set; }
        public string Amount { get; set; }
    }
    #endregion


    protected void RadMenuBomGrid_ItemClick(object sender, RadMenuEventArgs e)
    {
        DataTable dt = GetBOMGridItems();
        switch (e.Item.Text)
        {
            case "Add Row Above":
                AddNewRowGrid(dt, "above", "Bom");
                break;

            case "Add Row Below":
                AddNewRowGrid(dt, "below", "Bom");
                break;
        }
    }

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

    private void AddNewRowGrid(DataTable dt, string position, string gridName)
    {
        Int32 _orderNo = 0;
        DataRow dr = dt.NewRow();
        string maxvalue = dt.AsEnumerable().Max(row => row["Line"]).ToString();
        Int32 _line = Convert.ToInt32(maxvalue) + 1;
        if (gridName == "Bom")
        {
            _orderNo = Int32.Parse(radGridClickedRowIndex.Value);
            dr["OrderNo"] = _orderNo;
            dr["Line"] = _line;
            dr["MatMod"] = "0.00";
            dr["MatPrice"] = "0.00";
            dr["MatMarkup"] = "0.00";
            dr["LabHours"] = "0.00";
            dr["LabRate"] = "0.00";
            dr["LabMod"] = "0.00";
            dr["LabExt"] = "0.00";
            dr["LabPrice"] = "0.00";
            dr["LabMarkup"] = "0.00";
            dr["TotalExt"] = "0.00";
            dr["BudgetUnit"] = "0.00";
            dr["QtyReq"] = "0.00";
            dr["BudgetExt"] = "0.00";
        }
        else if (gridName == "Milestones")
        {
            _orderNo = Int32.Parse(radMilGridClickedRowIndex.Value);
            dr["OrderNo"] = _orderNo;
            dr["Line"] = _line;
            dr["Amount"] = "0.00";
        }

        if (position == "above")
        {
            dt.Rows.InsertAt(dr, _orderNo);
        }
        else if (position == "below")
        {
            dt.Rows.InsertAt(dr, _orderNo + 1);
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["OrderNo"] = i + 1;
        }

        if (gridName == "Bom")
        {
            
            BindgvBOM(dt);
        }
        else if (gridName == "Milestones")
        {
            //ViewState["TempMilestone"] = dt;
            gvMilestones.DataSource = dt;
            gvMilestones.DataBind();

        }
    }

    protected void lnkExportEstimateProfile_Click(object sender, EventArgs e)
    {
        var eid = TxtEstimateNo.Text;
        
        var url = "EstimateProfile.aspx?eid=" + eid;

        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        url += "&redirect=" + redirect;

        Response.Redirect(url);
    }

    
    public enum EstimatMode
    {
        Add = 0,
        Edit = 1,
        Copy = 2
    }

    private void GetDataEquipmentForGrid()
    {
        DataSet ds = new DataSet();
        _objUser.ConnConfig = Session["config"].ToString();

        if (!string.IsNullOrEmpty(hdnLocID.Value) && hdnLocID.Value != "0")
        {
            _objUser.SearchBy = string.Empty;
            _objUser.LocID = Convert.ToInt32(hdnLocID.Value);
            //HyperLinkAddEquip.NavigateUrl = "addequipment.aspx?lid=" + hdnLocID.Value + "&locname=" + txtCont.Text + "&addFrom=Ticket";
            //objPropUser.SearchBy = "e.loc";
            //objPropUser.SearchValue = hdnLocId.Value;
            _objUser.InstallDate = string.Empty;
            _objUser.ServiceDate = string.Empty;
            _objUser.Price = string.Empty;
            _objUser.Manufacturer = string.Empty;
            _objUser.Status = -1;
            #region Company Check
            _objUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            if (Convert.ToString(HttpContext.Current.Session["CmpChkDefault"]) == "1")
            {
                _objUser.EN = 1;
            }
            else
            {
                _objUser.EN = 0;
            }
            #endregion
            ds = objBL_User.getElev(_objUser);
            RadgvEquip.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadgvEquip.DataSource = ds.Tables[0];
            RadgvEquip.DataBind();
        }
        else
        {
            if (!string.IsNullOrEmpty(hdProspect.Value))
            {
                _objUser.ProspectID = Convert.ToInt32(hdProspect.Value);
                ds = objBL_User.getLeadEquip(_objUser);

                RadgvEquip.VirtualItemCount = ds.Tables[0].Rows.Count;
                RadgvEquip.DataSource = ds.Tables[0];
                RadgvEquip.DataBind();
            }
        }
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

    protected void ddlOpportunity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOpportunity.SelectedValue != "0")
        {
            GetOpportunity(Convert.ToInt32(ddlOpportunity.SelectedValue));
            //SetSelectedValueForDDL(ddlOpportunity, ddlOpportunity.SelectedValue);
            // Re-set value for hidden opportunity id
            hdnOpportunity.Value = ddlOpportunity.SelectedValue;
            // Disable opp info
            txtOppName.Enabled = false;
            //ddlOppStage.Enabled = false;
        }
        else
        {
            hdnOpportunity.Value = string.Empty;
            // Enable opp info
            txtOppName.Enabled = true;
            //ddlOppStage.Enabled = true;
            // Init value for txtOppName by copy from Estimate description
            txtOppName.Text = txtREPdesc.Text;
        }

        //var currentValueTemplate = ddlTemplate.SelectedValue;
        // Need to run after set value for hdnOpportunity
        var template = ddlTemplate.SelectedValue;
        if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            //FillTemplateDropdown(Convert.ToInt32(ddlOpportunity.SelectedValue), Convert.ToInt32(Request.QueryString["uid"].ToString()), hdnSelectedTemplate.Value);
            FillTemplateDropdown(Convert.ToInt32(ddlOpportunity.SelectedValue), Convert.ToInt32(Request.QueryString["uid"].ToString()), template);
        }
        else
        {
            //FillTemplateDropdown(Convert.ToInt32(ddlOpportunity.SelectedValue), 0, hdnSelectedTemplate.Value);
            FillTemplateDropdown(Convert.ToInt32(ddlOpportunity.SelectedValue), 0, template);
        }
        //SetSelectedValueForDDL(ddlTemplate, hdnSelectedTemplate.Value);
        // Reset selected Department
        //txtJobType.Text = string.Empty;

        GetDataEquipmentForGrid();
        UpdateSelectedEquipmentBySelectedGroup(Convert.ToInt32(ddlEstimateGroup.SelectedValue));
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    private void SetSelectedValueForDDL(DropDownList dropDownList, string strVal)
    {
        if (dropDownList.Items.FindByValue(strVal) != null)
        {
            dropDownList.SelectedValue = strVal;
        }
        else
        {
            dropDownList.SelectedValue = null;
        }
    }

    private void SetSelectedValueForddlOpportunity(string strVal)
    {
        SetSelectedValueForDDL(ddlOpportunity, strVal);

        if (!string.IsNullOrEmpty(ddlOpportunity.SelectedValue) && ddlOpportunity.SelectedValue != "0")
        {
            txtOppName.Enabled = false;
            //ddlOppStage.Enabled = false;
        }
        else
        {
            txtOppName.Enabled = true;
            //ddlOppStage.Enabled = true;
        }
    }

    private void SetSelectedValueForddlEstimateGroup(string strVal)
    {
        SetSelectedValueForDDL(ddlEstimateGroup, strVal);

        if (!string.IsNullOrEmpty(ddlEstimateGroup.SelectedValue) && ddlEstimateGroup.SelectedValue != "0")
        {
            lnkAddGroupName.ToolTip = "Edit Group Name";
        }
        else
        {
            lnkAddGroupName.ToolTip = "Add Group Name";
        }
    }

    private void FillGroupNameDropdown(int rolID)
    {
        //DropDownList ddlTemplate = gvTemplateItems.FooterRow.FindControl("ddlTemplate") as DropDownList;
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.RoleID = rolID;

        ds = objBL_Customer.GetEstimateGroupNames(objProp_Customer);
        ddlEstimateGroup.DataSource = ds.Tables[0];
        ddlEstimateGroup.DataTextField = "GroupName";
        ddlEstimateGroup.DataValueField = "Id";
        ddlEstimateGroup.DataBind();

        ddlEstimateGroup.Items.Insert(0, new ListItem("Select Group", "0"));
    }

    protected void ddlEstimateGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateSelectedEquipmentBySelectedGroup(Convert.ToInt32(ddlEstimateGroup.SelectedValue));
        if (!string.IsNullOrEmpty(ddlEstimateGroup.SelectedValue) && ddlEstimateGroup.SelectedValue != "0")
        {
            lnkAddGroupName.ToolTip = "Edit Group Name";
        }
        else
        {
            lnkAddGroupName.ToolTip = "Add Group Name";
        }
    }

    private void UpdateSelectedEquipmentBySelectedGroup(int groupID)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.GroupId = groupID;
        if (groupID != 0)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateEquimentPanel", "ShowHideEquipments(true);", true);
            ds = objBL_Customer.GetEquipmentsByGroupId(objProp_Customer);
            txtUnit.Text = string.Empty;
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

                        if (txtUnit.Text != string.Empty)
                        {
                            txtUnit.Text = txtUnit.Text + ", " + lblname.Text;
                        }
                        else
                        {
                            txtUnit.Text = lblname.Text;
                        }
                    }
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateEquimentPanel", "ShowHideEquipments(false);", true);
        }
    }

    protected void lnkPopupUpdateGroup_Click(object sender, EventArgs e)
    {
        try
        {
            //var currentSelectedVal = ddlEstimateGroup.SelectedValue;

            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.GroupName = txtGroupName.Text;
            objProp_Customer.GroupId = Convert.ToInt32(ddlEstimateGroup.SelectedValue);

            objProp_Customer.RoleID = Convert.ToInt32(hdnROLId.Value);

            DataSet ds = new DataSet();
            ds = objBL_Customer.AddUpdateEstimateGroup(objProp_Customer);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlEstimateGroup.DataSource = ds.Tables[0];
                ddlEstimateGroup.DataTextField = "GroupName";
                ddlEstimateGroup.DataValueField = "Id";
                ddlEstimateGroup.DataBind();

                ddlEstimateGroup.Items.Insert(0, new ListItem("Select Group", "0"));

                var currentSelectedVal = ds.Tables[1].Rows[0][0].ToString();
                //SetSelectedValueForDDL(ddlEstimateGroup, currentSelectedVal);
                SetSelectedValueForddlEstimateGroup(currentSelectedVal);
            }

            if (ddlEstimateGroup.SelectedValue != "0" && ddlEstimateGroup.SelectedValue != "")
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateEquimentPanel", "ShowHideEquipments(true);", true);
            else
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateEquimentPanel", "ShowHideEquipments(false);", true);

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseRadWindowGroup();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseRadWindowGroup();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkAddEditOpportunity_Click(object sender, EventArgs e)
    {
        var previousPageURL = HttpContext.Current.Request.Url.PathAndQuery;
        previousPageURL = WebUtility.UrlEncode(previousPageURL);
        if (!string.IsNullOrEmpty(ddlOpportunity.SelectedValue) && ddlOpportunity.SelectedValue != "0")
        {
            Response.Redirect("addopprt.aspx?uid=" + ddlOpportunity.SelectedValue + "&page=" + previousPageURL);
        }
        else
        {
            Response.Redirect("addopprt.aspx?page=" + previousPageURL);
        }

    }

    private double ConvertCurrentCurrencyFormatToDbl(string strCurrency)
    {
        if (!string.IsNullOrEmpty(strCurrency))
        {
            var dblReturn = double.Parse(strCurrency.Replace("$", ""), NumberStyles.AllowParentheses |
                                                        NumberStyles.AllowThousands |
                                                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingSign |
                                                        NumberStyles.Float);
            return dblReturn;
        }
        else
        {
            return 0;
        }
    }

    protected void gvBOM_PreRender(object sender, EventArgs e)
    {
        
        var focusControlId = string.Empty;
        if (!IsPostBack)
        {

            DataSet ds = new DataSet();
            _objUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _objUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            _objUser.PageName = "addestimate.aspx";
            _objUser.GridId = "gvBOM";
            ds = objBL_User.GetGridUserSettings(_objUser);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //string columnSettings = "[{Name: \"BType\", Display: true, Width: 300},{Name: \"MatItem\", Display: false, Width: 300}]";
                var columnSettings = ds.Tables[0].Rows[0][0].ToString();
                var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

                var colIndex = 0;

                foreach (GridColumn column in gvBOM.MasterTableView.OwnerGrid.Columns)
                {
                    colIndex++;
                    var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (clSetting != null)
                    {
                        column.Display = clSetting.Display;
                        if (clSetting.Width != 0)
                            column.ItemStyle.Width = clSetting.Width;
                        if (string.IsNullOrEmpty(focusControlId))
                        {
                            if (colIndex > 3 && clSetting.Display)
                            {
                                focusControlId = column.UniqueName;
                            }
                        }
                    }
                }
            }
        }

        try
        {
            if (gvBOM.Items.Count > 0 && !string.IsNullOrEmpty(hdnSelectedGrid.Value) && hdnSelectedGrid.Value == "gvBOM")
            {
                int lastRow = gvBOM.Items.Count;

                GridDataItem gr = (GridDataItem)gvBOM.Items[lastRow - 1];
                if (string.IsNullOrEmpty(focusControlId))
                {
                    TextBox txtCode = (TextBox)gr.FindControl("txtCode");
                    txtCode.Focus();
                }
                else if (focusControlId.IndexOf("txt") == 0)
                {
                    TextBox txtControl = (TextBox)gr.FindControl(focusControlId);
                    txtControl.Focus();
                }
                else if (focusControlId.IndexOf("chk") == 0)
                {
                    CheckBox chkControl = (CheckBox)gr.FindControl(focusControlId);
                    chkControl.Focus();
                }
                else if (focusControlId.IndexOf("ddl") == 0)
                {
                    DropDownList ddlControl = (DropDownList)gr.FindControl(focusControlId);
                    ddlControl.Focus();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnCopyPreviousBOM_Click(object sender, EventArgs e)
    {
        try
        {
            var selectIndex = 0;

            if (!string.IsNullOrEmpty(hdnBOMPrevSelectedRowIndex.Value))
            {
                selectIndex = Convert.ToInt32(hdnBOMPrevSelectedRowIndex.Value);
            }
            else
            {
                var selectItem = gvBOM.MasterTableView.GetSelectedItems();
                if (selectItem.Count() > 0)
                {
                    selectIndex = selectItem[0].ClientRowIndex;
                }
            }


            var dt = GetBOMGridItems();
            if (dt.Rows.Count > 0 && selectIndex > 0)
            {
                var copyRow = dt.Rows[selectIndex - 1];
                var dr = dt.Rows[selectIndex];
                //int i_ref = Convert.ToInt32(dr["Ref"]);
                //int i_line = Convert.ToInt32(dr["line"]);
                dr.ItemArray = copyRow.ItemArray.Clone() as object[];
                //dr["Ref"] = i_ref;
                //dr["line"] = i_line;
                dt.AcceptChanges();

                //ViewState["Transactions"] = dt;
                //gvBOM.DataSource = dt;
                //gvBOM.DataBind();
                BindgvBOM(dt);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "re-calculate", "CalculateEstimateBOM();CalculateEstimateMilestone();", true);
            }

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnCopyPreviousMilestones_Click(object sender, EventArgs e)
    {
        try
        {
            var selectIndex = 0;

            if (!string.IsNullOrEmpty(hdnBillingPrevSelectedRowIndex.Value))
            {
                selectIndex = Convert.ToInt32(hdnBillingPrevSelectedRowIndex.Value);
            }
            else
            {
                var selectItem = gvMilestones.MasterTableView.GetSelectedItems();
                if (selectItem.Count() > 0)
                {
                    selectIndex = selectItem[0].ClientRowIndex;
                }
            }


            var dt = GetMilestoneGridItems();
            if (dt.Rows.Count > 0 && selectIndex > 0)
            {
                var copyRow = dt.Rows[selectIndex - 1];
                var dr = dt.Rows[selectIndex];
                //int i_ref = Convert.ToInt32(dr["Ref"]);
                //int i_line = Convert.ToInt32(dr["line"]);
                dr.ItemArray = copyRow.ItemArray.Clone() as object[];
                //dr["Ref"] = i_ref;
                //dr["line"] = i_line;
                dt.AcceptChanges();

                //ViewState["Transactions"] = dt;
                gvMilestones.DataSource = dt;
                gvMilestones.DataBind();

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "re-calculate", "CalculateEstimateMilestone();", true);
            }

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    //public class ColumnSettings
    //{
    //    public string Name { get; set; }
    //    public bool Display { get; set; }
    //    public int Width { get; set; }
    //}

    protected void btnAddNewLinesBOM_Click(object sender, EventArgs e)
    {
        AddNewLinesBOM();
    }

    protected void btnAddNewLinesMilestones_Click(object sender, EventArgs e)
    {
        AddNewLinesMilestones();
    }


    private void AddNewLinesMilestones()
    {
        DataTable dt = GetMilestoneGridItems();

        string maxvalueLine = "";
        string maxValueOrderNo = "";

        if (dt.Rows.Count == 0)
        {
            maxvalueLine = "0";
            maxValueOrderNo = "0";
        }
        else
        {
            maxvalueLine = dt.AsEnumerable().Max(row => row["Line"]).ToString();
            if (string.IsNullOrEmpty(dt.Rows[0]["OrderNo"].ToString()))
            {
                maxValueOrderNo = "1";
            }
            else
            {
                maxValueOrderNo = dt.AsEnumerable().Max(row => row["OrderNo"]).ToString();
            }
        }
        Int32 _line = Convert.ToInt32(maxvalueLine) + 1;
        Int32 _orderNo = Convert.ToInt32(maxValueOrderNo) + 1;
        for (int j = 0; j < 1; j++)
        {
            DataRow dr = dt.NewRow();
            dr["OrderNo"] = _orderNo;
            dr["Line"] = _line;
            dr["Amount"] = "0.00";
            dt.Rows.Add(dr);
            _line = _line + 1;
            _orderNo = _orderNo + 1;
        }

        gvMilestones.DataSource = dt;
        gvMilestones.DataBind();
    }

    protected void gvMilestones_PreRender(object sender, EventArgs e)
    {
        try
        {
            if (gvMilestones.Items.Count > 0 && !string.IsNullOrEmpty(hdnSelectedGrid.Value) && hdnSelectedGrid.Value == "gvMilestones")
            {
                int lastRow = gvMilestones.Items.Count;

                GridDataItem gr = (GridDataItem)gvMilestones.Items[lastRow - 1];
                TextBox txtCode = (TextBox)gr.FindControl("txtCode");
                txtCode.Focus();
            }
        }
        catch (Exception ex)
        {

        }
    }

    #region logs

    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
            if (Request.QueryString["uid"] != null)
            {
                DataSet dsLog = new DataSet();
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"]);
                dsLog = objBL_Customer.GetEstimateLogs(objProp_Customer);
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
        catch { }
    }
    bool isGroupLog = false;

    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return RadGrid_gvLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            RadGrid_gvLogs.MasterTableView.SortExpressions.Count > 0;
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

    #endregion

    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        //string[] CommandArgument = btn.CommandArgument.Split(',');

        //string FileName = CommandArgument[0];

        //string FilePath = CommandArgument[1];
        string[] CommandArgument = btn.CommandArgument.Replace(btn.Text, " ").Split(',');

        string FileName = btn.Text;
        string FilePath = CommandArgument[1].Trim() + btn.Text.Trim();

        DownloadDocument(FilePath, FileName);
    }

    protected void lnkAddnewContact_Click(object sender, EventArgs e)
    {
        RadWindowContact.Title = "Add Contact";
        txtContcName.Text = "";
        txtTitle.Text = "";
        txtContPhone.Text = "";
        txtContFax.Text = "";
        txtContCell.Text = "";
        txtContEmail.Text = "";
        ViewState["ContactID"] = "0";
        string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}" +
            "$('[id *= txtContPhone]').mask('(999) 999 - 9999 ? Ext 99999');" +
            "$('[id *= txtContPhone]').bind('paste', function () { $(this).val(''); });" +
            "$('[id *= txtContCell]').mask('(999) 999 - 9999');" +
            "$('[id *= txtContFax]').mask('(999) 999 - 9999');" +
            "Sys.Application.add_load(f);" +
            "Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void btnEditContact_Click(object sender, EventArgs e)
    {
        if (RadGrid_Contacts.SelectedItems.Count > 0)
        {
            RadWindowContact.Title = "Edit Contact";

            foreach (GridDataItem item in RadGrid_Contacts.SelectedItems)
            {
                //DataTable dt = (DataTable)Session["contacttablelead"];
                Label lblContactName = (Label)item.FindControl("lblContactName");
                Label lblContactTitle = (Label)item.FindControl("lblContactTitle");
                Label lblContactPhone = (Label)item.FindControl("lblContactPhone");
                Label lblContactFax = (Label)item.FindControl("lblContactFax");
                Label lblContactCell = (Label)item.FindControl("lblContactCell");
                Label lblEmail = (Label)item.FindControl("lblEmail");
                HiddenField hdnContactID = (HiddenField)item.FindControl("hdnContactID");

                //DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];

                txtContcName.Text = lblContactName.Text;
                txtTitle.Text = lblContactTitle.Text;
                txtContPhone.Text = lblContactPhone.Text;
                txtContFax.Text = lblContactFax.Text;
                txtContCell.Text = lblContactCell.Text;
                txtContEmail.Text = lblEmail.Text;
                ViewState["ContactID"] = hdnContactID.Value;
                //ViewState["index"] = lblindex.Text;
            }

            //string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}" +
                "$('[id *= txtContPhone]').mask('(999) 999 - 9999 ? Ext 99999');" +
                "$('[id *= txtContPhone]').bind('paste', function () { $(this).val(''); });" +
                "$('[id *= txtContCell]').mask('(999) 999 - 9999');" +
                "$('[id *= txtContFax]').mask('(999) 999 - 9999');" +
                "Sys.Application.add_load(f);" +
                "Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: 'Select a contact for updating!',  type : 'warning', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnDeleteContact_Click(object sender, EventArgs e)
    {
        try
        {
            if (RadGrid_Contacts.SelectedItems.Count > 0)
            {
                foreach (GridDataItem item in RadGrid_Contacts.SelectedItems)
                {
                    HiddenField hdnContactID = (HiddenField)item.FindControl("hdnContactID");
                    PhoneModel objProp_Phone = new PhoneModel();
                    objProp_Phone.ConnConfig = Session["config"].ToString();
                    if (!string.IsNullOrEmpty(hdnContactID.Value))
                    {
                        objProp_Phone.ID = Convert.ToInt32(hdnContactID.Value);
                    }
                    else
                    {
                        objProp_Phone.ID = 0;
                    }
                    objBL_Customer.DeleteContact(objProp_Phone);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Contact deleted',  type : 'success', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                }
                RadGrid_Contacts.Rebind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: 'Select a contact for deleting!',  type : 'warning', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }


    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        try
        {
            //DataTable dt = (DataTable)Session["contacttablelead"];
            PhoneModel objProp_Phone = new PhoneModel();
            objProp_Phone.ConnConfig = Session["config"].ToString();
            objProp_Phone.Name = objGeneralFunctions.Truncate(txtContcName.Text, 50);
            objProp_Phone.Title = objGeneralFunctions.Truncate(txtTitle.Text, 50);
            objProp_Phone.Phone = objGeneralFunctions.Truncate(txtContPhone.Text, 50);
            objProp_Phone.Fax = objGeneralFunctions.Truncate(txtContFax.Text, 22);
            objProp_Phone.Cell = objGeneralFunctions.Truncate(txtContCell.Text, 22);
            objProp_Phone.Email = objGeneralFunctions.Truncate(txtContEmail.Text, 50);
            objProp_Phone.Rol = Convert.ToInt32(hdnROLId.Value);
            if (ViewState["ContactID"] != null && ViewState["ContactID"].ToString() != "")
            {
                objProp_Phone.ID = Convert.ToInt32(ViewState["ContactID"].ToString());
            }
            else
            {
                objProp_Phone.ID = 0;
            }
            objBL_Customer.AddUpdateContact(objProp_Phone);

            RadGrid_Contacts.Rebind();
            if (objProp_Phone.ID == 0)
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Contact added',  type : 'success', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            else
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Contact updated',  type : 'success', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, timeout: 3000, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_Contacts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //if (Request.QueryString["rol"] != null)
        if (Request.QueryString["rol"] != null || !string.IsNullOrEmpty(hdnROLId.Value))
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ROL = Convert.ToInt32(hdnROLId.Value);
            DataSet ds = new DataSet();
            ds = objBL_Customer.GetContactAllByRolID(objProp_Customer);
            RadGrid_Contacts.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Contacts.DataSource = ds.Tables[0];
            if (ds.Tables[1].Rows.Count > 0)
            {
                txtCompany.Text = ds.Tables[1].Rows[0]["Company"].ToString();
            }
        }
        else
        {
            RadGrid_Contacts.DataSource = string.Empty;
        }
    }

    protected void RadGrid_Project_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            if (Request.QueryString["uid"] != null)
            {
                DataSet ds = objBL_Customer.GetAllProjectByLoc(Session["config"].ToString(), Convert.ToInt32(hdnLocID.Value));

                RadGrid_Project.VirtualItemCount = ds.Tables[0].Rows.Count;
                RadGrid_Project.DataSource = ds;
            }
        }
        catch (Exception)
        {
            RadGrid_Project.VirtualItemCount = 0;
            RadGrid_Project.DataSource = String.Empty;
        }


    }


    protected void btnSaveProject_Click(object sender, EventArgs e)
    {
        try
        {
            if (RadGrid_Project.SelectedItems.Count == 1)
            {
                GridDataItem item = (GridDataItem)RadGrid_Project.SelectedItems[0];
                HiddenField hdnProjectID = (HiddenField)item.FindControl("hdnProjectID");
                DataSet ds = objBL_Customer.LinkEstimateToProjectNew(Session["config"].ToString(), Convert.ToInt32(Request.QueryString["uid"]), Convert.ToInt32(hdnProjectID.Value), Session["Username"].ToString());

                if (ds.Tables.Count > 0)
                {
                    // Refresh UI
                    RadGrid_Project.Rebind();
                    RadGrid_gvLogs.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccess", "noty({text: 'The estimate is successfully linked to the project', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    String url = "";
                    var projectId = ds.Tables[0].Rows[0]["job"].ToString();

                    url = "<span style='float :left'>Project #</span><a style='float :left' href='addproject?uid=" + projectId + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) + "'>" + projectId + "</a>";
                    trProj.InnerHtml = url.ToString();
                    liLinkProject.Visible = false;
                    lnkConvert.Visible = false;
                    lnkUndoConvert.Visible = Session["EstimateConvertPermission"] != null && Session["EstimateConvertPermission"].ToString() == "Y";
                    lnkSaveEstimate.Visible = false;
                    lnkUndoConvert.Text = "Unlink Project";
                    hdnLinkedProject.Value = projectId;
                    txtBidDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["BDate"].ToString()).ToShortDateString();
                    SetSelectedValueForDDL(ddlStatus, ds.Tables[0].Rows[0]["status"].ToString());
                }
            }


            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyCloseWindows", "CloseProjectWindow();", true);

        }

        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }

    }

    
    private void BindCustomGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            if (ViewState["CustomTable"] != null)
                dt = (DataTable)ViewState["CustomTable"];
            else
            {
                General objPropGeneral = new General();
                BL_General objBL_General = new BL_General();
                objPropGeneral.Screen = "Estimate";
                objPropGeneral.ConnConfig = Session["config"].ToString();
                if (Request.QueryString["uid"] != null)
                {
                    objPropGeneral.ScreenRefID = Convert.ToInt32(Request.QueryString["uid"]);
                }
                else
                {
                    objPropGeneral.ScreenRefID = 0;
                }
                var ds = objBL_General.GetScreenCustomFields(objPropGeneral);
                dt = ds.Tables[0];
                dt.Columns.Add("ControlID", typeof(string));
                dt.Columns.Add("IsValueChanged", typeof(bool));
                dt.Columns.Add("OldValue", typeof(string));


                ViewState["CustomTable"] = dt;
                ViewState["CustomValues"] = ds.Tables[1];
            }

            RadGrid_EstTags.DataSource = dt;
            RadGrid_EstTags.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void RadGrid_EstTags_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //var gridId = ((RadGrid)sender).ID;
            //var customViewStateName = gridId + "_dtValue";
            GridDataItem item = (GridDataItem)e.Item;
            CheckBox chkSelectAlert = (CheckBox)item.FindControl("chkSelectAlert");
            //CheckBox chkSelectTask = (CheckBox)item.FindControl("chkSelectTask");

            int format = Convert.ToInt32(DataBinder.Eval(item.DataItem, "Format"));
            int id = Convert.ToInt32(DataBinder.Eval(item.DataItem, "ID"));
            string textValue = DataBinder.Eval(item.DataItem, "Value").ToString();
            Label lblLine = (Label)item.FindControl("lblLine");

            Boolean isAlert = !string.IsNullOrEmpty(DataBinder.Eval(item.DataItem, "IsAlert").ToString()) ? Convert.ToBoolean(DataBinder.Eval(item.DataItem, "IsAlert")) : false;
            //Boolean isTask = !string.IsNullOrEmpty(DataBinder.Eval(item.DataItem, "IsTask").ToString()) ? Convert.ToBoolean(DataBinder.Eval(item.DataItem, "IsTask")) : false;

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
                    if (ViewState["CustomValues"] != null)
                    {
                        DataTable dtCustomval = (DataTable)ViewState["CustomValues"];
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

    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Emails.MasterTableView.FilterExpression != "" ||
            RadGrid_Emails.MasterTableView.GroupByExpressions.Count > 0 ||
            RadGrid_Emails.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Emails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Emails.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
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
        //FillDistributionList("", "");

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

    private void InitTeamMemberGridView()
    {
        DataSet ds = new DataSet();
        _objUser.ConnConfig = Session["config"].ToString();
        //objPropUser.Status = 0;
        //ds = objBL_User.GetUsersForTeamMemberList(objPropUser);
        ds = objBL_User.GetUsersAndRolesForTeamMemberList(_objUser);
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

        RadGrid_Emails.DataSource = teamMembers;
        if (teamMembers != null && teamMembers.Rows.Count > 0)
        {
            RadGrid_Emails.VirtualItemCount = teamMembers.Rows.Count;
        }
        else
        {
            RadGrid_Emails.VirtualItemCount = 0;
        }

    }

    protected void ReloadGrid_Click(object sender, EventArgs e)
    {
        InitTeamMemberGridView();
        RadGrid_Emails.Rebind();
    }

    private void UpdatingCustomDataTableValue(DataTable retDataTable)
    {
        foreach (GridDataItem row in RadGrid_EstTags.Items) // loops through each rows in RadGrid
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
                            break;
                        case "3":
                            TextBox txtbox = (TextBox)row.FindControl("txtFormatText");
                            if (!rowtb["Value"].Equals(txtbox.Text))
                            {
                                rowtb["OldValue"] = rowtb["Value"];
                                rowtb["Value"] = txtbox.Text;
                                rowtb["IsValueChanged"] = true;
                            }
                            break;
                        case "4":
                            DropDownList ddlCustomValue = (DropDownList)row.FindControl("drpdwnCustom");
                            if (!rowtb["Value"].Equals(ddlCustomValue.SelectedValue))
                            {
                                rowtb["OldValue"] = rowtb["Value"];
                                rowtb["Value"] = ddlCustomValue.SelectedValue;
                                rowtb["IsValueChanged"] = true;
                            }
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
                    HiddenField hdnMembers = (HiddenField)row.FindControl("hdnMembers");
                    rowtb["TeamMember"] = hdnMembers.Value;
                    TextBox txtMembers = (TextBox)row.FindControl("txtMembers");
                    rowtb["TeamMemberDisplay"] = txtMembers.Text;
                    rowtb["UpdatedDate"] = DateTime.Now;
                    rowtb["Username"] = Session["username"].ToString();
                }
            }
        }
    }

    private void CustomChangedAlert_Task(DataTable dtCustomValueChanged, int estimateId)
    {
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
            emailLog.Function = "Custom Fields Alert";
            emailLog.Screen = "Estimate";
            emailLog.Username = Session["Username"].ToString();
            emailLog.SessionNo = Guid.NewGuid().ToString();
            emailLog.Ref = estimateId;

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
                        var mailTitle = string.Format("{2} - estimate#: {0} - {1}", estimateId, txtREPdesc.Text, item["Label"]);


                        StringBuilder sbdTaskRemask = new StringBuilder();
                        sbdTaskRemask.AppendLine("This estimate has changes to alert you.");
                        sbdTaskRemask.AppendFormat("Estimate# {0} - {1}", estimateId, txtREPdesc.Text);
                        sbdTaskRemask.AppendLine();
                        sbdTaskRemask.AppendFormat("Estimate location: {0}", txtCont.Text);
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
                                CreateTaskOnWorkflowChange(mailTitle, sbdTaskRemask.ToString(), memberUserName, estimateId);
                            }
                            else if (Convert.ToBoolean(item["IsAlert"]))
                            {
                                if (changedItem != null && changedItem["email"] != null && changedItem["email"].ToString() != string.Empty)
                                {
                                    SendingAlertEmailOnWorkflowChanged(item, changedItem["email"].ToString().Trim(), emailSendError, sbdSentError, mimeErrorMessages
                                        , emailLog, mimeSentMessages, ref totalSendErr, estimateId);
                                }
                            }
                        }
                        else if (Convert.ToBoolean(item["IsAlert"]))
                        {
                            if (changedItem != null && changedItem["email"] != null && changedItem["email"].ToString() != string.Empty)
                            {
                                SendingAlertEmailOnWorkflowChanged(item, changedItem["email"].ToString().Trim(), emailSendError, sbdSentError, mimeErrorMessages
                                        , emailLog, mimeSentMessages, ref totalSendErr, estimateId);
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
                        var mailTitle = string.Format("{2} - estimate#: {0} - {1}", estimateId, txtREPdesc.Text, item["Label"]);

                        StringBuilder sbdTaskRemask = new StringBuilder();
                        sbdTaskRemask.AppendLine("This estimate has changes to alert you.");
                        sbdTaskRemask.AppendFormat("Estimate# {0} - {1}", estimateId, txtREPdesc.Text);
                        sbdTaskRemask.AppendLine();
                        sbdTaskRemask.AppendFormat("Estimate location: {0}", txtCont.Text);
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


                        var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "'").ToList();
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
                                                CreateTaskOnWorkflowChange(mailTitle, sbdTaskRemask.ToString(), memberUserName, estimateId);
                                            }
                                            else if (Convert.ToBoolean(item["IsAlert"]))
                                            {
                                                if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                {
                                                    SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                    , emailLog, mimeSentMessages, ref totalSendErr, estimateId);
                                                }
                                            }
                                        }
                                        else if (Convert.ToBoolean(item["IsAlert"]))
                                        {
                                            if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                            {
                                                SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                , emailLog, mimeSentMessages, ref totalSendErr, estimateId);
                                            }
                                        }
                                    }
                                    else if (Convert.ToBoolean(item["IsAlert"]))
                                    {
                                        if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                        {
                                            SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                            , emailLog, mimeSentMessages, ref totalSendErr, estimateId);
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
                                                    CreateTaskOnWorkflowChange(mailTitle, sbdTaskRemask.ToString(), memberUserName, estimateId);
                                                }
                                                else if (Convert.ToBoolean(item["IsAlert"]))
                                                {
                                                    if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                    {
                                                        SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                        , emailLog, mimeSentMessages, ref totalSendErr, estimateId);
                                                    }
                                                }
                                            }
                                            else if (Convert.ToBoolean(item["IsAlert"]))
                                            {
                                                if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                                {
                                                    SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                    , emailLog, mimeSentMessages, ref totalSendErr, estimateId);
                                                }
                                            }
                                        }
                                        else if (Convert.ToBoolean(item["IsAlert"]))
                                        {
                                            if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                            {
                                                SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                                , emailLog, mimeSentMessages, ref totalSendErr, estimateId);
                                            }
                                        }
                                    }
                                    else if (Convert.ToBoolean(item["IsAlert"]))
                                    {
                                        if (projUser != null && projUser["email"] != null && projUser["email"].ToString() != string.Empty)
                                        {
                                            SendingAlertEmailOnWorkflowChanged(item, projUser["email"].ToString(), emailSendError, sbdSentError, mimeErrorMessages
                                            , emailLog, mimeSentMessages, ref totalSendErr, estimateId);
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
    }

    private void CreateTaskOnWorkflowChange(string strSubject, string strRemarks, string assignedTo, int estimateId, string strMailTo = "")
    {
        var objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.ROL = Convert.ToInt32(hdnROLId.Value);
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
            objCustomer.Screen = "Estimate";

            objCustomer.Ref = estimateId;
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
                        mail.Title = txtCont.Text + ": " + objCustomer.Subject;
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendFormat("Dear {0}<br><br>", objCustomer.AssignedTo);
                        stringBuilder.Append("You are receiving an appointment task from MOM-->Sales-->Tasks<br><br>");
                        stringBuilder.AppendFormat("Customer Name: {0}<br>", txtCompanyName.Text);
                        stringBuilder.AppendFormat("Location Name: {0}<br>", txtCont.Text);
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
                        var apSubject = string.Format("Task name: {0}", txtCompanyName.Text);

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
                            , txtCont.Text
                            , apStart
                            , apEnd
                            , 60
                            );
                        var myByteArray = System.Text.Encoding.UTF8.GetBytes(icsAttachmentContentsStr);
                        mail.attachmentBytes = myByteArray;
                        mail.FileName = "TaskAppointment.ics";
                        mail.IsIncludeSignature = true;
                        mail.Send();
                    }
                    catch (Exception ex)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
            }
            #endregion

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }

        //  Response.Redirect("AddProspect.aspx?uid=" + ProspectID);
    }

    private void SendingAlertEmailOnWorkflowChanged(DataRow item, string mailTo, Tuple<int, string, string> emailSendError, StringBuilder sbdSentError
        , List<MimeKit.MimeMessage> mimeErrorMessages, EmailLog emailLog, List<MimeKit.MimeMessage> mimeSentMessages, ref int totalSendErr
        , int estimateId)
    {
        Mail mail = new Mail();
        mail.From = WebBaseUtility.GetFromEmailAddress();
        mail.Title = string.Format("{2} - estimate#: {0} - {1}", estimateId, txtREPdesc.Text, item["Label"]);
        StringBuilder sbdContent = new StringBuilder();
        sbdContent.Append("This estimate has changes to alert you.<br/><br/>");
        sbdContent.AppendFormat("Estimate# {0} - {1}<br/>", estimateId, txtREPdesc.Text);
        sbdContent.AppendFormat("Estimate location: {0}<br/><br/>", txtCont.Text);
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
        mail.IsIncludeSignature = true;

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

    private void FillTasks(string name, bool isShowAll)
    {
        DataSet ds = new DataSet();
        var _objCustomer = new Customer();
        _objCustomer.ConnConfig = Session["config"].ToString();

        _objCustomer.StartDate = string.Empty;
        _objCustomer.EndDate = string.Empty;
        if (isShowAll)
        {
            _objCustomer.SearchBy = "";
            _objCustomer.SearchValue = "";
            _objCustomer.Screen = "";
            _objCustomer.Ref = 0;
        }
        else
        {
            _objCustomer.SearchBy = "t.rol";
            _objCustomer.SearchValue = name;
            _objCustomer.Screen = "Estimate";
            _objCustomer.Ref = Convert.ToInt32(Request.QueryString["uid"].ToString());
        }
        _objCustomer.Mode = 5;

        ds = objBL_Customer.getTasks(_objCustomer);

        RadGrid_Tasks.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Tasks.DataSource = ds.Tables[0];
        RadGrid_Tasks.DataBind();
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

    protected void chkShowAllTasks_CheckedChanged(object sender, EventArgs e)
    {
        FillTasks(hdnROLId.Value, chkShowAllTasks.Checked);
    }

    protected void ddlEstimateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEstimateType.SelectedValue == "bid")
        {
            gvMilestones.MasterTableView.GetColumn("Quantity").Display = false;
            gvMilestones.MasterTableView.GetColumn("Price").Display = false;
            gvMilestones.MasterTableView.GetColumn("AmountPer").Display = true;

            txtOverride.Text = hdnFinalBid.Value;
            txtOverride.Style["display"] = "block";
            lblFinalBid.Style["display"] = "none";
        }
        else
        {
            gvMilestones.MasterTableView.GetColumn("Quantity").Display = true;
            gvMilestones.MasterTableView.GetColumn("Price").Display = true;
            gvMilestones.MasterTableView.GetColumn("AmountPer").Display = false;
            txtOverride.Style["display"] = "none";
            lblFinalBid.Style["display"] = "block";
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyEstTypeChanged", "UpdateBillingFinalBidPrice();", true);
    }

    private DataTable CreateLaborFromBillingItems(DataTable billingDt)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("NoOfMen", typeof(int));
        dt.Columns.Add("Classification", typeof(string));
        dt.Columns.Add("ST_Hour", typeof(double));
        dt.Columns.Add("PT_Hour", typeof(double));
        dt.Columns.Add("DT_Hour", typeof(double));
        dt.Columns.Add("ST_Rate", typeof(double));
        dt.Columns.Add("PT_Rate", typeof(double));
        dt.Columns.Add("DT_Rate", typeof(double));
        dt.Columns.Add("Total", typeof(double));


        foreach (DataRow dr in billingDt.Rows)
        {
            var desc = dr["fDesc"].ToString();
            var arr = desc.Split('-');
            if (arr.Length == 2)
            {
                var classification = arr[0].Trim();
                string expression = "Classification = '" + classification + "'";
                var csfDr = dt.Select(expression).FirstOrDefault();
                if (csfDr == null)
                {
                    csfDr = dt.NewRow();
                    csfDr["Classification"] = classification;
                    dt.Rows.Add(csfDr);
                }
                switch (arr[1].Trim().ToUpper())
                {
                    case "ST":
                        var stHour = csfDr["ST_Hour"] != null && csfDr["ST_Hour"].ToString() != string.Empty ? Convert.ToDouble(csfDr["ST_Hour"]) : 0;
                        var stQuan = dr["Quantity"] != null && dr["Quantity"].ToString() != string.Empty ? Convert.ToDouble(dr["Quantity"]) : 0;
                        csfDr["ST_Hour"] = stHour + stQuan;
                        var stRate = csfDr["ST_Rate"] != null && csfDr["ST_Rate"].ToString() != string.Empty ? Convert.ToDouble(csfDr["ST_Rate"]) : 0;
                        var stPrice = dr["Price"] != null && dr["Price"].ToString() != string.Empty ? Convert.ToDouble(dr["Price"]) : 0;
                        csfDr["ST_Rate"] = stRate + stPrice;
                        break;
                    case "PT":
                        //var ptHour = Convert.ToDouble(csfDr["PT_Hour"]) + Convert.ToDouble(dr["Quantity"]);
                        //csfDr["PT_Hour"] = ptHour;
                        //var ptRate = Convert.ToDouble(csfDr["PT_Rate"]) + Convert.ToDouble(dr["Price"]);
                        //csfDr["PT_Rate"] = ptRate;
                        var ptHour = csfDr["PT_Hour"] != null && csfDr["PT_Hour"].ToString() != string.Empty ? Convert.ToDouble(csfDr["PT_Hour"]) : 0;
                        var ptQuan = dr["Quantity"] != null && dr["Quantity"].ToString() != string.Empty ? Convert.ToDouble(dr["Quantity"]) : 0;
                        csfDr["PT_Hour"] = ptHour + ptQuan;
                        var ptRate = csfDr["PT_Rate"] != null && csfDr["PT_Rate"].ToString() != string.Empty ? Convert.ToDouble(csfDr["PT_Rate"]) : 0;
                        var ptPrice = dr["Price"] != null && dr["Price"].ToString() != string.Empty ? Convert.ToDouble(dr["Price"]) : 0;
                        csfDr["PT_Rate"] = ptRate + ptPrice;
                        break;
                    case "DT":
                        //var dtHour = Convert.ToDouble(csfDr["DT_Hour"]) + Convert.ToDouble(dr["Quantity"]);
                        //csfDr["DT_Hour"] = dtHour;
                        //var dtRate = Convert.ToDouble(csfDr["DT_Rate"]) + Convert.ToDouble(dr["Price"]);
                        //csfDr["DT_Rate"] = dtRate;
                        var dtHour = csfDr["DT_Hour"] != null && csfDr["DT_Hour"].ToString() != string.Empty ? Convert.ToDouble(csfDr["DT_Hour"]) : 0;
                        var dtQuan = dr["Quantity"] != null && dr["Quantity"].ToString() != string.Empty ? Convert.ToDouble(dr["Quantity"]) : 0;
                        csfDr["DT_Hour"] = dtHour + dtQuan;
                        var dtRate = csfDr["DT_Rate"] != null && csfDr["DT_Rate"].ToString() != string.Empty ? Convert.ToDouble(csfDr["DT_Rate"]) : 0;
                        var dtPrice = dr["Price"] != null && dr["Price"].ToString() != string.Empty ? Convert.ToDouble(dr["Price"]) : 0;
                        csfDr["DT_Rate"] = dtRate + dtPrice;
                        break;
                }
            }
            else
            {
                continue;
            }
        }

        return dt;
    }

    private DataTable CreateLaborFromBOMItems(DataTable bomDt)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("NoOfMen", typeof(int));
        dt.Columns.Add("Classification", typeof(string));
        dt.Columns.Add("ST_Hour", typeof(double));
        dt.Columns.Add("PT_Hour", typeof(double));
        dt.Columns.Add("DT_Hour", typeof(double));
        dt.Columns.Add("ST_Rate", typeof(double));
        dt.Columns.Add("PT_Rate", typeof(double));
        dt.Columns.Add("DT_Rate", typeof(double));
        dt.Columns.Add("Total", typeof(double));

        string laborFilter = "BType = '2'";
        var dtLabor = bomDt.Select(laborFilter).ToList();

        foreach (DataRow dr in dtLabor)
        {
            var desc = dr["fDesc"].ToString();
            var arr = desc.Split('-');
            if (arr.Length == 2)
            {
                var classification = arr[0].Trim();
                string expression = "Classification = '" + classification + "'";
                var csfDr = dt.Select(expression).FirstOrDefault();
                if (csfDr == null)
                {
                    csfDr = dt.NewRow();
                    csfDr["Classification"] = classification;
                    dt.Rows.Add(csfDr);
                }
                switch (arr[1].Trim().ToUpper())
                {
                    case "ST":
                        var stHour = csfDr["ST_Hour"] != null && csfDr["ST_Hour"].ToString() != string.Empty ? Convert.ToDouble(csfDr["ST_Hour"]) : 0;
                        var stQuan = dr["LabHours"] != null && dr["LabHours"].ToString() != string.Empty ? Convert.ToDouble(dr["LabHours"]) : 0;
                        csfDr["ST_Hour"] = stHour + stQuan;
                        var stRate = csfDr["ST_Rate"] != null && csfDr["ST_Rate"].ToString() != string.Empty ? Convert.ToDouble(csfDr["ST_Rate"]) : 0;
                        var stPrice = dr["LabRate"] != null && dr["LabRate"].ToString() != string.Empty ? Convert.ToDouble(dr["LabRate"]) : 0;
                        csfDr["ST_Rate"] = stRate + stPrice;
                        break;
                    case "PT":
                        var ptHour = csfDr["PT_Hour"] != null && csfDr["PT_Hour"].ToString() != string.Empty ? Convert.ToDouble(csfDr["PT_Hour"]) : 0;
                        var ptQuan = dr["LabHours"] != null && dr["LabHours"].ToString() != string.Empty ? Convert.ToDouble(dr["LabHours"]) : 0;
                        csfDr["PT_Hour"] = ptHour + ptQuan;
                        var ptRate = csfDr["PT_Rate"] != null && csfDr["PT_Rate"].ToString() != string.Empty ? Convert.ToDouble(csfDr["PT_Rate"]) : 0;
                        var ptPrice = dr["LabRate"] != null && dr["LabRate"].ToString() != string.Empty ? Convert.ToDouble(dr["LabRate"]) : 0;
                        csfDr["PT_Rate"] = ptRate + ptPrice;
                        break;
                    case "DT":
                        var dtHour = csfDr["DT_Hour"] != null && csfDr["DT_Hour"].ToString() != string.Empty ? Convert.ToDouble(csfDr["DT_Hour"]) : 0;
                        var dtQuan = dr["LabHours"] != null && dr["LabHours"].ToString() != string.Empty ? Convert.ToDouble(dr["LabHours"]) : 0;
                        csfDr["DT_Hour"] = dtHour + dtQuan;
                        var dtRate = csfDr["DT_Rate"] != null && csfDr["DT_Rate"].ToString() != string.Empty ? Convert.ToDouble(csfDr["DT_Rate"]) : 0;
                        var dtPrice = dr["LabRate"] != null && dr["LabRate"].ToString() != string.Empty ? Convert.ToDouble(dr["LabRate"]) : 0;
                        csfDr["DT_Rate"] = dtRate + dtPrice;
                        break;
                }
            }
            else
            {
                string expression = "Classification = '" + desc + "'";
                var csfDr = dt.Select(expression).FirstOrDefault();
                if (csfDr == null)
                {
                    csfDr = dt.NewRow();
                    csfDr["Classification"] = desc;
                    dt.Rows.Add(csfDr);
                }
            }
        }

        foreach (DataRow csfDr in dt.Rows)
        {
            var stHour = csfDr["ST_Hour"] != null && csfDr["ST_Hour"].ToString() != string.Empty ? Convert.ToDouble(csfDr["ST_Hour"]) : 0;
            var stRate = csfDr["ST_Rate"] != null && csfDr["ST_Rate"].ToString() != string.Empty ? Convert.ToDouble(csfDr["ST_Rate"]) : 0;
            var stAmount = stHour * stRate;

            var ptHour = csfDr["PT_Hour"] != null && csfDr["PT_Hour"].ToString() != string.Empty ? Convert.ToDouble(csfDr["PT_Hour"]) : 0;
            var ptRate = csfDr["PT_Rate"] != null && csfDr["PT_Rate"].ToString() != string.Empty ? Convert.ToDouble(csfDr["PT_Rate"]) : 0;
            var ptAmount = ptHour * ptRate;

            var dtHour = csfDr["DT_Hour"] != null && csfDr["DT_Hour"].ToString() != string.Empty ? Convert.ToDouble(csfDr["DT_Hour"]) : 0;
            var dtRate = csfDr["DT_Rate"] != null && csfDr["DT_Rate"].ToString() != string.Empty ? Convert.ToDouble(csfDr["DT_Rate"]) : 0;
            var dtAmount = dtHour * dtRate;

            csfDr["Total"] = stAmount + ptAmount + dtAmount;
        }

        return dt;
    }

    protected void lnkUndoConvert_Click(object sender, EventArgs e)
    {
        try
        {
            var projId = hdnLinkedProject.Value;
            if (!string.IsNullOrEmpty(projId))
            {
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objProp_Customer.job = Convert.ToInt32(projId);
                objProp_Customer.Username = Session["Username"].ToString();
                objBL_Customer.EstimateConversionUndo(objProp_Customer);

                liLinkProject.Visible = true;
                lnkConvert.Visible = Session["EstimateConvertPermission"] != null && Session["EstimateConvertPermission"].ToString() == "Y";
                lnkUndoConvert.Visible = false;
                lnkSaveEstimate.Visible = Session["EstimateEditPermission"] != null && Session["EstimateEditPermission"].ToString() == "Y";
                hdnLinkedProject.Value = "";
                trProj.InnerHtml = string.Empty;
                RadGrid_Project.Rebind();
                RadGrid_gvLogs.Rebind();
                SetSelectedValueForDDL(ddlStatus, "1");// Reset status to Open

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyUndoSucc", "noty({text: 'Unlinked successfully!',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void gvBOM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = GetBOMGridItems();
        BindgvBOM(dt, false);
    }

    private void BindgvBOM(DataTable dt, bool Rebind = true)
    {
        gvBOM.DataSource = dt;

        try
        {
            if (Rebind) gvBOM.Rebind();
        }
        catch { }

        ViewState["TempBOM"] = dt;
    }

    private DataTable GetBOMGridItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItemID", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("CodeDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("BType", typeof(int));
        dt.Columns.Add("QtyReq", typeof(double));
        dt.Columns.Add("UM", typeof(string));
        dt.Columns.Add("BudgetUnit", typeof(double));
        dt.Columns.Add("BudgetExt", typeof(double));    // JobTItem.Budget
        dt.Columns.Add("MatItem", typeof(int));         // BOM.MatItem
        dt.Columns.Add("MatMod", typeof(double));       // JobTItem.Modifier
        dt.Columns.Add("MatPrice", typeof(double));     //new column
        dt.Columns.Add("MatMarkup", typeof(double));
        dt.Columns.Add("STax", typeof(byte));
        dt.Columns.Add("Currency", typeof(string));
        dt.Columns.Add("LabItem", typeof(int));         // BOM.LabItem
        dt.Columns.Add("LabMod", typeof(double));       // JobTItem.ETCMod
        dt.Columns.Add("LabExt", typeof(double));       // JobTItem.ETC
        dt.Columns.Add("LabRate", typeof(double));      // BOM.LabRate
        dt.Columns.Add("LabHours", typeof(double));        //JobTItem.BHours
        dt.Columns.Add("SDate", typeof(DateTime));      // BOM.SDate
        dt.Columns.Add("VendorId", typeof(int));
        dt.Columns.Add("Vendor", typeof(string));
        dt.Columns.Add("TotalExt", typeof(double));
        dt.Columns.Add("LabPrice", typeof(double));     // new column
        dt.Columns.Add("LabMarkup", typeof(double));
        dt.Columns.Add("LStax", typeof(byte));
        dt.Columns.Add("EstimateItemID", typeof(int));
        dt.Columns.Add("OrderNo", typeof(int));

        dt.Columns.Add("MatName", typeof(string));         // BOM.MatName
        dt.Columns.Add("BTypeName", typeof(string));
        dt.Columns.Add("LabItemName", typeof(string));
        dt.Columns.Add("ChangeOrder", typeof(int));
        //dt.Columns.Add("Comment", typeof(string));

        DataTable dtemp = (DataTable)ViewState["TempBOM"];

        double budgetExt = 0;
        double _qtyReq = 0;
        double labExt = 0;
        try
        {
            foreach (GridDataItem item in gvBOM.Items)
            {
                //HiddenField hdnID = (HiddenField)item.FindControl("hdnID");
                HiddenField hdnLine = (HiddenField)item.FindControl("hdnLine");
                //bom items
                DropDownList ddlBType = (DropDownList)item.FindControl("ddlBType");

                TextBox txtScope = (TextBox)item.FindControl("txtScope");

                HiddenField hdnEstimateItemID = (HiddenField)item.FindControl("hdnEstimateItemID");

                //HiddenField hdntxtLabItem1 = (HiddenField)item.FindControl("hdntxtLabItem");

                DropDownList ddlLabItem = (DropDownList)item.FindControl("ddlLabItem");

                //TextBox txtLabItem = (TextBox)item.FindControl("txtLabItem");

                HiddenField hdnddlMatItemId = (HiddenField)item.FindControl("hdnMatItem");

                TextBox txtQtyReq = (TextBox)item.FindControl("txtQtyReq");

                TextBox txtUM = (TextBox)item.FindControl("txtUM");

                TextBox txtBudgetUnit = (TextBox)item.FindControl("txtBudgetUnit");

                TextBox txtMatMod = (TextBox)item.FindControl("txtMatMod");

                TextBox txtLabMod = (TextBox)item.FindControl("txtLabMod");

                TextBox txtVendor = (TextBox)item.FindControl("txtVendor");

                HiddenField hdnVendorId = (HiddenField)item.FindControl("hdnVendorId");

                TextBox txtCode = (TextBox)item.FindControl("txtCode");

                TextBox txtHours = (TextBox)item.FindControl("txtHours");

                TextBox txtLabRate = (TextBox)item.FindControl("txtLabRate");

                TextBox txtLabPrice = (TextBox)item.FindControl("txtLabPrice");
                TextBox txtLabMarkup = (TextBox)item.FindControl("txtLabMarkup");
                TextBox txtMatPrice = (TextBox)item.FindControl("txtMatPrice");
                TextBox txtMatMarkup = (TextBox)item.FindControl("txtMatMarkup");
                HiddenField hdnMatChk = (HiddenField)item.FindControl("hdnMatChk");
                HiddenField hdnLbChk = (HiddenField)item.FindControl("hdnLbChk");

                TextBox txtSDate = (TextBox)item.FindControl("txtSDate");

                TextBox txtddlMatItem = (TextBox)item.FindControl("txtMatItem");

                HiddenField hdnOrderNo = (HiddenField)item.FindControl("hdnOrderNo");

                TextBox lblCodeDesc = (TextBox)item.FindControl("lblCodeDesc");

                CheckBox chkChangeOrder = (CheckBox)item.FindControl("chkChangeOrder");

                if (txtScope.Text.Trim() != string.Empty && ddlBType.SelectedValue != "0" && ddlBType.SelectedValue != "Select Type")
                {
                    _qtyReq = 0; budgetExt = 0; labExt = 0;

                    DataRow dr = dt.NewRow();


                    //if (hdnID.Value.Trim() != string.Empty) dr["JobTItemID"] = Convert.ToInt32(hdnID.Value);


                    if (hdnLine.Value.Trim() == string.Empty) return dt;


                    if (hdnLine.Value.Trim() != string.Empty) dr["Line"] = Convert.ToInt32(hdnLine.Value);


                    dr["fDesc"] = txtScope.Text;

                    dr["Code"] = txtCode.Text;


                    if (ddlBType.SelectedValue != "0") dr["BType"] = Convert.ToInt32(ddlBType.SelectedValue);

                    //int hdntxtLabItem = 0; int.TryParse(hdntxtLabItem1.Value.ToString(), out hdntxtLabItem);

                    //if (!hdntxtLabItem.Equals(0)) dr["LabItem"] = hdntxtLabItem;

                    if (ddlLabItem.SelectedValue != "0")
                    {
                        dr["LabItem"] = Convert.ToInt32(ddlLabItem.SelectedValue);
                    }

                    //if ((txtLabItem.Text.ToString().Trim() != string.Empty)) dr["txtLabItem"] = txtLabItem.Text;

                    int _hdnddlMatItemId = 0;

                    int.TryParse(hdnddlMatItemId.Value, out _hdnddlMatItemId);

                    if (!_hdnddlMatItemId.Equals(0)) dr["MatItem"] = _hdnddlMatItemId;

                    dr["MatName"] = (txtddlMatItem.Text);
                    dr["BTypeName"] = (ddlBType.SelectedItem.Text);
                    dr["LabItemName"] = (ddlLabItem.SelectedItem.Text);

                    if (txtQtyReq.Text.Trim() != string.Empty && txtQtyReq.Text.Trim() != "0.00") dr["QtyReq"] = Convert.ToDouble(txtQtyReq.Text.Trim());
                    else dr["QtyReq"] = 0;

                    if (txtUM.Text != string.Empty) dr["UM"] = txtUM.Text;

                    if (!string.IsNullOrEmpty(txtBudgetUnit.Text) && txtBudgetUnit.Text.Trim() != "0.00") dr["BudgetUnit"] = Convert.ToDouble(txtBudgetUnit.Text);
                    else dr["BudgetUnit"] = 0;

                    if (txtBudgetUnit.Text != string.Empty && !string.IsNullOrEmpty(txtQtyReq.Text) && txtBudgetUnit.Text != "0.00" && txtQtyReq.Text != "0.00")
                    {
                        _qtyReq = Convert.ToDouble(txtQtyReq.Text);

                        if (_qtyReq.Equals(0))
                        {
                            _qtyReq = Convert.ToDouble(txtQtyReq.Text);
                        }
                        budgetExt = _qtyReq * Convert.ToDouble(txtBudgetUnit.Text);

                        dr["BudgetExt"] = budgetExt;
                    }
                    else
                    {
                        dr["BudgetExt"] = 0;
                    }



                    if (txtMatMod.Text.Trim() != string.Empty && txtMatMod.Text.Trim() != "0.00")
                    {
                        dr["MatMod"] = Convert.ToDouble(txtMatMod.Text);
                    }
                    else
                    {
                        dr["MatMod"] = 0;
                    }

                    if (txtLabMod.Text.Trim() != string.Empty && txtLabMod.Text.Trim() != "0.00") dr["LabMod"] = Convert.ToDouble(txtLabMod.Text);
                    else dr["LabMod"] = 0;

                    if (hdnVendorId.Value.Trim() != string.Empty && txtVendor.Text.Trim() != string.Empty) dr["VendorId"] = Convert.ToInt32(hdnVendorId.Value);

                    if (txtVendor.Text.Trim() != string.Empty) dr["Vendor"] = txtVendor.Text;

                    if (txtHours.Text != string.Empty && txtHours.Text != "0.00") dr["LabHours"] = Convert.ToDouble(txtHours.Text);
                    else dr["LabHours"] = 0;

                    if (!string.IsNullOrEmpty(txtLabRate.Text) && txtLabRate.Text != "0.00") dr["LabRate"] = txtLabRate.Text;
                    else dr["LabRate"] = 0;

                    if (!string.IsNullOrEmpty(txtLabRate.Text) && !string.IsNullOrEmpty(txtHours.Text)
                            && txtLabRate.Text.Trim() != "0.00" && txtHours.Text.Trim() != "0.00")
                    {
                        labExt = Convert.ToDouble(txtLabRate.Text);

                        if (txtHours.Text.Trim() != string.Empty) labExt = labExt * Convert.ToDouble(txtHours.Text.Trim());
                        dr["LabExt"] = labExt;
                    }

                    else dr["LabExt"] = 0;

                    dr["TotalExt"] = labExt + budgetExt;

                    if (txtSDate.Text != string.Empty) { dr["SDate"] = Convert.ToDateTime(txtSDate.Text); }

                    if (hdnOrderNo.Value != string.Empty) dr["OrderNo"] = Convert.ToInt32(hdnOrderNo.Value);

                    if (lblCodeDesc.Text != string.Empty) dr["CodeDesc"] = lblCodeDesc.Text;

                    if (!string.IsNullOrEmpty(hdnEstimateItemID.Value.ToString()))
                    {
                        dr["EstimateItemID"] = Convert.ToInt32(hdnEstimateItemID.Value.ToString());
                    }
                    //else
                    //{
                    //    dr["EstimateItemID"] = 0;
                    //}

                    if (txtLabPrice.Text.Trim() != string.Empty && txtLabPrice.Text.Trim() != "0.00")
                    {
                        dr["LabPrice"] = Convert.ToDouble(txtLabPrice.Text);
                    }
                    else
                    {
                        dr["LabPrice"] = 0;
                    }
                    if (txtLabMarkup.Text.Trim() != string.Empty && txtLabMarkup.Text.Trim() != "0.00")
                    {
                        dr["LabMarkup"] = Convert.ToDouble(txtLabMarkup.Text);
                    }
                    else
                    {
                        dr["LabMarkup"] = 0;
                    }
                    if (txtMatPrice.Text.Trim() != string.Empty && txtMatPrice.Text.Trim() != "0.00")
                    {
                        dr["MatPrice"] = Convert.ToDouble(txtMatPrice.Text);
                    }
                    else
                    {
                        dr["MatPrice"] = 0;
                    }
                    if (txtMatMarkup.Text.Trim() != string.Empty && txtMatMarkup.Text.Trim() != "0.00")
                    {
                        dr["MatMarkup"] = Convert.ToDouble(txtMatMarkup.Text);
                    }
                    else
                    {
                        dr["MatMarkup"] = 0;
                    }
                    if (hdnMatChk.Value.Trim() != string.Empty)
                    {
                        if (hdnMatChk.Value.ToLower() == "true")
                        {
                            dr["Stax"] = 1;
                        }
                        else
                        {
                            dr["Stax"] = 0;
                        }
                    }
                    if (hdnLbChk.Value.Trim() != string.Empty)
                    {
                        if (hdnLbChk.Value.ToLower() == "true")
                        {
                            dr["LStax"] = 1;
                        }
                        else
                        {
                            dr["LStax"] = 0;
                        }
                    }

                    dr["ChangeOrder"] = Convert.ToInt32(chkChangeOrder.Checked);

                    dt.Rows.Add(dr);
                }
            }

            dt.AcceptChanges();

            foreach (DataRow drtemp in dtemp.Rows)
            {
                bool IsNotExists = true;

                foreach (DataRow dtdr in dt.Rows)
                {
                    if (drtemp["EstimateItemID"].ToString() == dtdr["EstimateItemID"].ToString() && drtemp["Line"].ToString() == dtdr["Line"].ToString())
                    {
                        IsNotExists = false;
                    }

                }

                if (IsNotExists)
                {
                    DataRow dr = dt.NewRow();

                    //if (drtemp["JobTItemID"].ToString() != string.Empty) dr["JobTItemID"] = Convert.ToInt32(drtemp["JobTItemID"].ToString());

                    if (drtemp["Line"].ToString() != string.Empty) dr["Line"] = Convert.ToInt32(drtemp["Line"].ToString());

                    if (drtemp["Code"].ToString() != string.Empty) dr["Code"] = (drtemp["Code"].ToString());

                    if (drtemp["fDesc"].ToString() != string.Empty) dr["fDesc"] = (drtemp["fDesc"].ToString());

                    if (drtemp["BType"].ToString() != string.Empty) dr["BType"] = Convert.ToInt32(drtemp["BType"].ToString());

                    if (drtemp["LabItem"].ToString() != string.Empty) dr["LabItem"] = Convert.ToInt32(drtemp["LabItem"].ToString());

                    //if (drtemp["txtLabItem"].ToString() != string.Empty) dr["txtLabItem"] = (drtemp["txtLabItem"].ToString());

                    if (drtemp["MatItem"].ToString() != string.Empty) dr["MatItem"] = Convert.ToInt32(drtemp["MatItem"].ToString());

                    if (drtemp["MatName"].ToString() != string.Empty) dr["MatName"] = (drtemp["MatName"].ToString());
                    if (drtemp["BTypeName"].ToString() != string.Empty) dr["BTypeName"] = (drtemp["BTypeName"].ToString());
                    if (drtemp["LabItemName"].ToString() != string.Empty) dr["LabItemName"] = (drtemp["LabItemName"].ToString());

                    if (drtemp["QtyReq"].ToString() != string.Empty) dr["QtyReq"] = Convert.ToDouble(drtemp["QtyReq"].ToString());

                    if (drtemp["UM"].ToString() != string.Empty) dr["UM"] = (drtemp["UM"].ToString());

                    if (drtemp["BudgetUnit"].ToString() != string.Empty) dr["BudgetUnit"] = Convert.ToDouble(drtemp["BudgetUnit"].ToString());

                    if (drtemp["BudgetExt"].ToString() != string.Empty) dr["BudgetExt"] = Convert.ToDouble(drtemp["BudgetExt"].ToString());

                    if (drtemp["MatMod"].ToString() != string.Empty) dr["MatMod"] = Convert.ToDouble(drtemp["MatMod"].ToString());

                    if (drtemp["LabMod"].ToString() != string.Empty) dr["LabMod"] = Convert.ToDouble(drtemp["LabMod"].ToString());

                    if (drtemp["VendorId"].ToString() != string.Empty) dr["VendorId"] = Convert.ToInt32(drtemp["VendorId"].ToString());

                    if (drtemp["Vendor"].ToString() != string.Empty) dr["Vendor"] = (drtemp["Vendor"].ToString());

                    if (drtemp["LabHours"].ToString() != string.Empty) dr["LabHours"] = Convert.ToDouble(drtemp["LabHours"].ToString());

                    if (drtemp["LabRate"].ToString() != string.Empty) dr["LabRate"] = Convert.ToDouble(drtemp["LabRate"].ToString());

                    if (drtemp["LabExt"].ToString() != string.Empty) dr["LabExt"] = Convert.ToDouble(drtemp["LabExt"].ToString());

                    if (drtemp["TotalExt"].ToString() != string.Empty) dr["TotalExt"] = Convert.ToDouble(drtemp["TotalExt"].ToString());

                    if (drtemp["SDate"].ToString() != string.Empty) dr["SDate"] = Convert.ToDateTime(drtemp["SDate"].ToString());

                    if (drtemp["OrderNo"].ToString() != string.Empty) dr["OrderNo"] = (drtemp["OrderNo"].ToString());

                    if (drtemp["CodeDesc"].ToString() != string.Empty) dr["CodeDesc"] = (drtemp["CodeDesc"].ToString());

                    if (drtemp["EstimateItemID"].ToString() != string.Empty) { dr["EstimateItemID"] = Convert.ToInt32(drtemp["EstimateItemID"].ToString()); }
                    if (drtemp["LabPrice"].ToString() != string.Empty) { dr["LabPrice"] = Convert.ToDouble(drtemp["LabPrice"].ToString()); }
                    if (drtemp["LabMarkup"].ToString() != string.Empty) { dr["LabMarkup"] = Convert.ToDouble(drtemp["LabMarkup"].ToString()); }
                    if (drtemp["MatPrice"].ToString() != string.Empty) { dr["MatPrice"] = Convert.ToDouble(drtemp["MatPrice"].ToString()); }
                    if (drtemp["MatMarkup"].ToString() != string.Empty) { dr["MatMarkup"] = Convert.ToDouble(drtemp["MatMarkup"].ToString()); }
                    if (drtemp["Stax"].ToString() != string.Empty) { dr["Stax"] = Convert.ToByte(drtemp["Stax"].ToString()); }
                    if (drtemp["LStax"].ToString() != string.Empty) { dr["LStax"] = Convert.ToByte(drtemp["LStax"].ToString()); }
                    
                    if (drtemp["ChangeOrder"].ToString() != string.Empty) { dr["ChangeOrder"] = Convert.ToInt32(drtemp["ChangeOrder"].ToString()); }

                    dt.Rows.Add(dr);

                }
            }

            dt.AcceptChanges();

            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = "OrderNo asc";
                dt = dt.DefaultView.ToTable();
            }

            ViewState["TempBOM"] = dt;

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            dt = (DataTable)ViewState["TempBOM"];
        }

        return dt;
    }

    private void AddNewLinesBOM()
    {
        DataTable dt = GetBOMGridItems();
        string maxvalueLine = "";
        string maxValueOrderNo = "";

        if (dt.Rows.Count == 0)
        {
            maxvalueLine = "0";
            maxValueOrderNo = "0";
        }
        else
        {
            maxvalueLine = dt.AsEnumerable().Max(row => row["Line"]).ToString();
            if (string.IsNullOrEmpty(dt.Rows[0]["OrderNo"].ToString()))
            {
                maxValueOrderNo = "1";
            }
            else
            {
                maxValueOrderNo = dt.AsEnumerable().Max(row => row["OrderNo"]).ToString();
            }
        }
        Int32 _line = Convert.ToInt32(maxvalueLine) + 1;
        Int32 _orderNo = Convert.ToInt32(maxValueOrderNo) + 1;
        for (int j = 0; j < 1; j++)
        {
            DataRow dr = dt.NewRow();
            dr["OrderNo"] = _orderNo;
            dr["Line"] = _line;
            dr["BudgetUnit"] = "0.00";
            dr["MatMod"] = "0.00";
            dr["MatPrice"] = "0.00";
            dr["MatMarkup"] = "0.00";
            dr["LabHours"] = "0.00";
            dr["LabRate"] = "0.00";
            dr["LabMod"] = "0.00";
            dr["LabExt"] = "0.00";
            dr["LabPrice"] = "0.00";
            dr["LabMarkup"] = "0.00";
            dr["TotalExt"] = "0.00";
            dr["QtyReq"] = "0.00";
            dr["BudgetExt"] = "0.00";
            dt.Rows.Add(dr);
            _line = _line + 1;
            _orderNo = _orderNo + 1;
        }

        //gvBOM.DataSource = dt;
        //gvBOM.DataBind();
        ViewState["TempBOM"] = dt;
        BindgvBOM(dt);
    }

    //protected void ddlApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    var temp = Session["Username"].ToString() + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + "\n";
    //    if (ddlApprovalStatus.SelectedValue == "1")
    //    {
    //        txtApproveStatusComment.Text = "Approved by " + temp;
    //    }
    //    else if (ddlApprovalStatus.SelectedValue == "2")
    //    {
    //        txtApproveStatusComment.Text = "Changes required by " + temp;
    //    }
    //    else
    //    {
    //        txtApproveStatusComment.Text = "Pending by " + temp;
    //    }
    //}


    protected void RadGrid_Documents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GetDocuments();

    }
    protected void RadGrid_ApprovedStatusHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
            var ds = objBL_Customer.GetEstimateApprovedStatusHistory(objProp_Customer);
            if (ds.Tables.Count > 0)
                RadGrid_ApprovedStatusHistory.DataSource = ds.Tables[0];
        }
        else
        {
            RadGrid_ApprovedStatusHistory.DataSource = null;
        }
    }

    protected void lnkApplyApproveStatus_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            try
            {
                bool allowChange = (string)ViewState["EstimateApproveProposalPermission"] == "Y";

                if (!allowChange)
                {
                    // TODO: Get Approve Status Before change
                    var currApproveStatus = (string)ViewState["CurrApproveStatus"];
                    allowChange = currApproveStatus == "2" && ddlApprovalStatus.SelectedValue == "0";
                }
                
                if (allowChange)
                {
                    objProp_Customer.ConnConfig = Session["config"].ToString();
                    objProp_Customer.estimateno = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    objProp_Customer.Status = Convert.ToInt16(ddlApprovalStatus.SelectedValue);
                    objProp_Customer.date = DateTime.Now;
                    objProp_Customer.Username = Session["Username"].ToString();
                    objProp_Customer.Notes = txtApproveStatusComment.Text;
                    objProp_Customer.Comment = txtComment.Text;
                    

                    objBL_Customer.UpdateEstimateApprovalStatus(objProp_Customer);

                    BL_General objBL_General = new BL_General();
                    var isApprove = objBL_General.GetSalesApproveEstimate(Session["config"].ToString());
                    //if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["ApplyEstApproveProposal"]) && WebConfigurationManager.AppSettings["ApplyEstApproveProposal"] == "1")
                    if (isApprove)
                    {
                        if (ddlApprovalStatus.SelectedValue == "1")
                        {
                            btnSendEmail.Visible = true;
                        }
                        else
                        {
                            btnSendEmail.Visible = false;
                        }
                    }
                    else
                    {
                        btnSendEmail.Visible = true;
                    }

                    RadGrid_ApprovedStatusHistory.Rebind();
                    // Reset currApproveStatus
                    ViewState["CurrApproveStatus"] = ddlApprovalStatus.SelectedValue;
                    ViewState["CurrApproveStatusNote"] = txtApproveStatusComment.Text;

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Updated Approval Status successfully!',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                }
                else
                {
                    ddlApprovalStatus.SelectedValue = (string)ViewState["CurrApproveStatus"];
                    txtApproveStatusComment.Text = (string)ViewState["CurrApproveStatusNote"];

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: 'You do not have permission to do this function',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            catch(Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                if (str == "The approval status has not changed for applying")
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
    }

    protected void lnkLatestProposal_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            objEF.ConnConfig = Session["config"].ToString();
            objEF.Estimate = Convert.ToInt32(Request.QueryString["uid"]);
            var ds = objBL_EF.GetEstimateLastProposalByEstimateId(objEF);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                String fileName = ds.Tables[0].Rows[0]["FileName"].ToString();
                string strFileEx = fileName.Split('.').Length > 1 ? "." + fileName.Split('.').LastOrDefault() : "";
                bool manualUpload = Convert.ToString(ds.Tables[0].Rows[0]["JobTID"]) == "0" ? true : false;
                if (manualUpload)
                {
                    DownloadDocument(ds.Tables[0].Rows[0]["FilePath"].ToString(), fileName);
                }
                else
                {
                    String newFileName = fileName.Replace(strFileEx, "") + "-" + objEF.Estimate + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(ds.Tables[0].Rows[0]["AddedOn"].ToString())) + ".pdf";

                    DownloadDocument(ds.Tables[0].Rows[0]["PdfFilePath"].ToString(), newFileName);
                }
            }
        }
    }

    public string ShowHoverText(object estimateId, object fileName, object addedOn, object addedBy, object jobTID)
    {
        string result = string.Empty;
        //string strName = Convert.ToString(name);
        string strFileName = Convert.ToString(fileName);
        if (!string.IsNullOrEmpty(strFileName))
        {
            bool manualUpload = Convert.ToString(jobTID) == "0";
            string strFileEx = strFileName.Split('.').Length > 1 ? "." + strFileName.Split('.').LastOrDefault() : "";
            string newFileName = "";
            if (!manualUpload)
            {
                newFileName = strFileName.Replace(strFileEx, "") + "-" + estimateId.ToString() + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(addedOn)) + ".pdf";
            }
            else
            {
                newFileName = strFileName;
            }

            result = "<i>File Name:</i> " + newFileName + "<br/>";

            if (!string.IsNullOrEmpty(addedOn.ToString()) && addedOn.ToString().Length > 80)
            {
                result += "<i>Created Date</i>: " + Convert.ToString(addedOn).Replace("\n", "<br/>").Substring(0, 80) + " ...";
            }
            else
            {
                result += "<i>Created Date</i>: " + Convert.ToString(addedOn).Replace("\n", "<br/>");
            }

            if (!string.IsNullOrEmpty(Convert.ToString(addedBy)) && addedBy.ToString().Length > 80)
            {
                result += "<br/><i>Created By</i>: " + Convert.ToString(addedBy).Replace("\n", "<br/>").Substring(0, 80) + " ...";
            }
            else
            {
                result += "<br/><i>Created By</i>: " + Convert.ToString(addedBy).Replace("\n", "<br/>");
            }
        }

        return result;
    }

    protected void RadGrid_EmailLogs_ItemCreated(object sender, GridItemEventArgs e)
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

    protected void RadGrid_EmailLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            RadGrid_EmailLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupEmailLogs();
            DataSet dsLog = new DataSet();
            EmailLog emailLog = new EmailLog();
            emailLog.Screen = "Estimate";
            emailLog.ConnConfig = Session["config"].ToString();
            emailLog.Ref = Convert.ToInt32(Request.QueryString["uid"]);
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
                    RadGrid_EmailLogs.DataSource = string.Empty;
                }
                else
                {
                    RadGrid_EmailLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
                    RadGrid_EmailLogs.DataSource = dsLog.Tables[0];
                }
            }
            else
            {
                RadGrid_EmailLogs.DataSource = string.Empty;
            }
        }
        else
        {
            RadGrid_EmailLogs.DataSource = string.Empty;
        }
    }

    bool isGroupEmailLog = false;
    public bool ShouldApplySortFilterOrGroupEmailLogs()
    {
        return RadGrid_EmailLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_EmailLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupEmailLog) ||
            RadGrid_EmailLogs.MasterTableView.SortExpressions.Count > 0;
    }

    protected void lnkUploadProposal_Click(object sender, EventArgs e)
    {
        try
        {

            string filename = string.Empty;
            string fullpath = string.Empty;
            string mime = string.Empty;
            var savepath = string.Empty;
            var mainDirectory = "EstimateForms";

            if (Request.QueryString["uid"] != null)
            {
                mainDirectory += "\\EP_" + Request.QueryString["uid"];
            }

            savepath = GetProposalUploadDirectory(mainDirectory);

            if (Request.QueryString["uid"] != null)
            {
                if (!string.IsNullOrEmpty(FileUpload2.FileName))
                {
                    //foreach (HttpPostedFile postedFile in FileUpload2.PostedFiles)
                    //{
                    filename = FileUpload2.FileName;
                    fullpath = savepath + filename;
                    //mime = System.IO.Path.GetExtension(FileUpload2.PostedFile.FileName).Substring(1);

                    if (File.Exists(fullpath))
                    {
                        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                        filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                        fullpath = savepath + filename;
                    }

                    if (!Directory.Exists(savepath))
                    {
                        Directory.CreateDirectory(savepath);
                    }

                    FileUpload2.SaveAs(fullpath);

                    objEF.ConnConfig = Session["config"].ToString();
                    objEF.Estimate = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    objEF.JobTID = 0;
                    objEF.FileName = filename;
                    objEF.Name = "Manual Upload";
                    objEF.Id = 0;
                    objEF.UpdatedBy = Session["Username"].ToString();
                    objEF.UpdatedOn = DateTime.Now;
                    objEF.FilePath = fullpath;
                    objEF.PdfFilePath = fullpath;
                    objBL_EF.AddEstimateForm(objEF);
                    //}

                    //To ReBind the Form GridView.
                    GetEstimateForms();
                }
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void chkBilFrmBOM_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBilFrmBOM.Checked)
        {
            ddlEstimateType.SelectedValue = "quote";
            ddlEstimateType_SelectedIndexChanged(sender, e);

            var dtBOM = ViewState["TempBOM"] != null ? (DataTable)ViewState["TempBOM"] : null;
            //var dtBOM = GetBOMGridItems();
            if (dtBOM != null)
            {
                var dtBilling = CreateMilestoneDataTable();
                var i = 0;
                var j = 1;
                foreach (DataRow drBOM in dtBOM.Rows)
                {
                    DataRow drBil = dtBilling.NewRow();
                    drBil["Line"] = i;
                    drBil["OrderNo"] = j;
                    drBil["Amount"] = drBOM["TotalExt"];
                    drBil["fDesc"] = drBOM["fDesc"];
                    drBil["CodeDesc"] = drBOM["CodeDesc"];
                    drBil["jcode"] = drBOM["Code"];
                    drBil["Quantity"] = 1;
                    drBil["Price"] = drBOM["TotalExt"];

                    dtBilling.Rows.Add(drBil);
                    i++;
                    j++;
                }

                gvMilestones.DataSource = dtBilling;
                gvMilestones.DataBind();
            }

            //ddlEstimateType.SelectedValue = "quote";
        }
    }
}