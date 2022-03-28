using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessEntity;
using BusinessLayer;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
//using OpenPop.Mime;
//using OpenPop.Mime.Header;
//using OpenPop.Pop3;
//using OpenPop.Pop3.Exceptions;
//using OpenPop.Common.Logging;
//using Message = OpenPop.Mime.Message;

public partial class AddProspect : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    Vendor obj_vendor = new Vendor();
    BL_Vendor objBL_vendor = new BL_Vendor();

    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    BL_Lead obj_Lead = new BL_Lead();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    private string referral = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
            //Response.Redirect("timeout.htm");
            //return;
        }

        WebBaseUtility.UpdatePageTitle(this, "Lead", Request.QueryString["uid"], Request.QueryString["t"]);

        if (!IsPostBack)
        {
            ViewState["convert"] = "0";
            ViewState["edit"] = 0;
            ViewState["mode"] = 0;
            ViewState["editcon"] = 0;
            ViewState["notesmode"] = 0;
            Session["contacttablelead"] = null;
            CreateTable();
            GetProspectType();
            FillSalesPerson();
            getSource();
            getCustomer();
            getVendor();
            // FillStages();
            FillBT();
            CompanyPermission();
            //  FillService();
            //TODO
            //NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
            //masterSalesMaster.FillPendingLeads();

            if (Request.QueryString["uid"] == null)
            {
                ddlCompany.Visible = true;
                ddlCompanyLabel.Visible = true;
                txtCompany.Visible = false;
                btnCompanyPopUp.Visible = false;
            }
            if (Request.QueryString["uid"] != null)
            {
                if (Convert.ToString(Request.QueryString["t"]) != "c")
                    lblHeader.Text = "Edit Lead";
                else
                    lblHeader.Text = "Copy Lead";
                pnlNext.Visible = true;
                ddlCompanyLabel.Visible = false;
                ddlCompany.Visible = false;
                txtCompany.Visible = true;
                btnCompanyPopUp.Visible = true;
                if (Session["MSM"].ToString() != "TS")
                {
                    if (Convert.ToString(Request.QueryString["t"]) != "c")
                    {
                        lnkConvert.Visible = true;
                    }

                }
                FillProspectScreen(Request.QueryString["uid"].ToString());
                lnkNewEmail.Visible = true;
                lnkNewEmail.NavigateUrl = "email.aspx?to=" + txtEmail.Text.Trim() + "&rol=" + hdnRol.Value;
                HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + Convert.ToString(ViewState["rol"])
                    + "&name=" + txtProspectName.Text
                    + "&assignedTo=" + ddlSalesperson.SelectedItem.Text
                    + "&customer=" + txtCustomer.Text
                    + "&screen=lead&ref=" + Request.QueryString["uid"];
                    //+ "&leadId=" + Request.QueryString["uid"];
                HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + Convert.ToString(ViewState["rol"])
                    + "&name=" + txtProspectName.Text
                    + "&assignedTo=" + ddlSalesperson.SelectedItem.Text
                    + "&customer=" + txtCustomer.Text
                    + "&screen=lead&ref=" + Request.QueryString["uid"];
                    //+ "&leadId=" + Request.QueryString["uid"];
                lnkAddopp.NavigateUrl = "AddOpprt.aspx?rol=" + Convert.ToString(ViewState["rol"]) 
                    + "&owner=" + Convert.ToString(Request.QueryString["uid"]) 
                    + "&name=" + txtProspectName.Text 
                    + "&assignedTo=" + ddlSalesperson.SelectedItem.Value 
                    + "&BusinessType=" + ddlBusinessType.SelectedItem.Text 
                    + "&customer=" + txtCustomer.Text
                    + "&source=" + rcSource.Text
                    + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl);

            }
            if (ViewState["edit"].ToString() == "0")
            {
                HideControls();
            }

            if (Session["AddLeadSuccMess"] != null && Session["AddLeadSuccMess"].ToString() != "")
            {
                string strScript = string.Empty;

                strScript += "noty({text: '" + Session["AddLeadSuccMess"].ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddLeadSucc", strScript, true);
                Session["AddLeadSuccMess"] = null;
            }

            if (Session["AddEditTaskSuccMess"] != null && Session["AddEditTaskSuccMess"].ToString() != "")
            {
                string strScript = string.Empty;
                
                strScript += "noty({text: 'Task " + Session["AddEditTaskSuccMess"].ToString() + " Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);
                Session["AddEditTaskSuccMess"] = null;
            }
        }
        pnlNext.Visible = false;
        if (Request.QueryString["uid"] != null)
        {
            liLogs.Style["display"] = "inline-block";
            tbLogs.Style["display"] = "block";
            liEquipments.Style["display"] = "inline-block";
            adViewEquipments.Style["display"] = "block";
            pnlNext.Visible = true;
        }
        //Permission();
        UserPermission();
        HighlightSideMenu();
    }

    private void HighlightSideMenu()
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkProspect");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        div.Style.Add("display", "block");
    }

    protected void lnkConvert_Click(object sender, EventArgs e)
    {
        pnlCustomer.Visible = true;
        //uc_CustomerSearch1._txtCustomer.Focus();
        //uc_CustomerSearch1._CustomValidator2.Visible = false;
        uc_CustomerSearch1._txtCustomer.Height = 30;
        uc_CustomerSearch1._txtCustomer.Width = 500;
        lnkConvert.Visible = false;
        lnkSave.Text = "Next";
        ViewState["convert"] = "1";

        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", "Materialize.updateTextFields();", true);
    }

    private void ConvertProspectWizard()
    {
        if (ViewState["convert"].ToString() == "1")
        {
            string ProspectID = Request.QueryString["uid"].ToString();
            
            if(Request.QueryString["estimateid"]!=null)
            {
                string EstimateID = Request.QueryString["estimateid"].ToString();
                StringBuilder sdbScript = new StringBuilder();
                string alertMess = "Continue to Convert Lead Wizard.";
                
                if (uc_CustomerSearch1._hdnCustID.Value != string.Empty)
                {
                    sdbScript.AppendFormat("alert('{2}'); window.location.href='addlocation.aspx?cpw=1&prospectid={0}&estimatecid={1}", ProspectID, EstimateID, alertMess);
                    sdbScript.AppendFormat("&customerid={0}", uc_CustomerSearch1._hdnCustID.Value);
                }
                else
                {
                    sdbScript.AppendFormat("alert('{2}'); window.location.href='addcustomer.aspx?cpw=1&prospectid={0}&estimateid={1}", ProspectID, EstimateID, alertMess);
                }

                if (Request.QueryString["opid"] != null)
                {
                    sdbScript.AppendFormat("&opid={0}", Request.QueryString["opid"]);
                }

                sdbScript.Append("';");

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", sdbScript.ToString(), true);

                //if (uc_CustomerSearch1._hdnCustID.Value == string.Empty)
                //{                    
                //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Continue to Convert Lead Wizard.'); window.location.href='addcustomer.aspx?cpw=1&prospectid=" + ProspectID + "&estimateid=" + EstimateID + "';", true);
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + uc_CustomerSearch1._hdnCustID.Value + "&estimateid=" + EstimateID + "';", true);
                //}
            }
            else
            {
                string alertMess = "Continue to Convert Lead Wizard.";
                if (uc_CustomerSearch1._hdnCustID.Value == string.Empty)
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('"+ alertMess + "'); window.location.href='addcustomer.aspx?cpw=1&prospectid=" + ProspectID + "';", true);
                else
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('" + alertMess + "'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + uc_CustomerSearch1._hdnCustID.Value + "';", true);
            }
        }
    }

    private void Permission()
    {
   

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }
        //if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        //{
        //    Response.Redirect("home.aspx");
        //}

        if (Session["type"].ToString() != "am")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];
            string Sales = dt.Rows[0]["sales"].ToString().Substring(0, 1);

            if (Sales == "N")
            {
                Response.Redirect("home.aspx");
            }
        }

        /////
        if (Session["type"].ToString() != "am")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            //Contact.....................>
            string ContactPermission = ds.Rows[0]["ContactPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["ContactPermission"].ToString();
            hdnAddeContact.Value = ContactPermission.Length < 1 ? "Y" : ContactPermission.Substring(0, 1);
            hdnEditeContact.Value = ContactPermission.Length < 2 ? "Y" : ContactPermission.Substring(1, 1);
            hdnDeleteContact.Value = ContactPermission.Length < 3 ? "Y" : ContactPermission.Substring(2, 1);
            hdnViewContact.Value = ContactPermission.Length < 4 ? "Y" : ContactPermission.Substring(3, 1);

            //pnlContactButtons.Visible = gvContacts.Visible = hdnViewContact.Value == "N" ? false : true;
            pnlContactButtons.Visible = RadGrid_Contacts.Visible = hdnViewContact.Value == "N" ? false : true;

            //Document...................>
            string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
            hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
            hdnEditeDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
            hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
            hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

            //pnlDocumentButtons.Visible = gvDocuments.Visible = hdnViewDocument.Value == "N" ? false : true;
            pnlDocumentButtons.Visible = RadGrid_Documents.Visible = hdnViewDocument.Value == "N" ? false : true;



        }

        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable dtUserInfo = new DataTable();
            dtUserInfo = (DataTable)Session["userinfo"];
            // Equipment
            string ElevatorPermission = dtUserInfo.Rows[0]["Elevator"] == DBNull.Value ? "YYYYYY" : dtUserInfo.Rows[0]["Elevator"].ToString();

            hdnAddeEquipment.Value = ElevatorPermission.Length < 1 ? "Y" : ElevatorPermission.Substring(0, 1);
            hdnEditeEquipment.Value = ElevatorPermission.Length < 2 ? "Y" : ElevatorPermission.Substring(1, 1);
            hdnDeleteEquipment.Value = ElevatorPermission.Length < 3 ? "Y" : ElevatorPermission.Substring(2, 1);
            hdnViewEquipment.Value = ElevatorPermission.Length < 4 ? "Y" : ElevatorPermission.Substring(3, 1);
        }
        else
        {
            hdnAddeEquipment.Value = "Y";
            hdnEditeEquipment.Value = "Y";
            hdnDeleteEquipment.Value = "Y";
            hdnViewEquipment.Value = "Y";
        }
    }
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
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// SalesManager ///////////////////------->

            string RCmodulePermission = ds.Rows[0]["SalesManager"] == DBNull.Value ? "Y" : ds.Rows[0]["SalesManager"].ToString();

            if (RCmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// UserSales ///////////////////------->

            string ProcessRCPermission = ds.Rows[0]["UserSales"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["UserSales"].ToString();
            string ADD = ProcessRCPermission.Length < 1 ? "Y" : ProcessRCPermission.Substring(0, 1);
            string Edit = ProcessRCPermission.Length < 2 ? "Y" : ProcessRCPermission.Substring(1, 1);
            string Delete = ProcessRCPermission.Length < 3 ? "Y" : ProcessRCPermission.Substring(2, 1);
            string View = ProcessRCPermission.Length < 4 ? "Y" : ProcessRCPermission.Substring(3, 1);

            if (Request.QueryString["uid"] != null)
            {
                //aImport.Visible = false;
            }
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
                    lnkSave.Visible = false;
                    //btnSubmitJob.Visible = false;
                }
                else
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }

            // Opportunity Permission
            string ProposalPermission = ds.Rows[0]["Proposal"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Proposal"].ToString();
            string ADDOpp = ProposalPermission.Length < 1 ? "Y" : ProposalPermission.Substring(0, 1);
            string EditOpp = ProposalPermission.Length < 2 ? "Y" : ProposalPermission.Substring(1, 1);
            string DeleteOpp = ProposalPermission.Length < 2 ? "Y" : ProposalPermission.Substring(2, 1);
            string ViewOpp = ProposalPermission.Length < 4 ? "Y" : ProposalPermission.Substring(3, 1);
            string ReportOpp = ProposalPermission.Length < 6 ? "Y" : ProposalPermission.Substring(5, 1);

            // string ResolvedTicketPermission = ds.Rows[0]["Resolve"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Resolve"].ToString();

            // string DeleteResolved = hdnDeleteResolvedTicket.Value = ResolvedTicketPermission.Length < 3 ? "Y" : ResolvedTicketPermission.Substring(2, 1);

            if (ADDOpp == "N")
            {
                lnkAddopp.Visible = false;
                lnkCopyOpp.Visible = false;
            }
            if (EditOpp == "N")
            {
                lnkEditOpp.Visible = false;
            }
            if (DeleteOpp == "N")
            {
                lnkDeleteOpp.Visible = false;

            }
            if (ReportOpp == "N")
            {
                lnkExcelOpp.Visible = false;
            }
            if (ViewOpp == "N")
            {
                RadGrid_Opportunity.Visible = false;
            }
        }
        //else if (ADD == "N" && Edit == "N")
        //{                
        //        Response.Redirect("Home.aspx?permission=no"); return;
        //    }
        // else
        //    {
        //        Response.Redirect("Home.aspx?permission=no"); return;
        //    }
        // }

        
        
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
        if (Session["COPer"].ToString() == "1")
        {
            ViewState["CompPermission"] = 1;
            dvCompanyPermission.Visible = true;
            FillCompany();
            RadGrid_Opportunity.Columns.FindByDataField("Company").Visible = true;
        }
        else
        {
            ViewState["CompPermission"] = 0;
            dvCompanyPermission.Visible = false;
            RadGrid_Opportunity.Columns.FindByDataField("Company").Visible = false;
        }
    }
    private void HideControls()
    {
        //pnlOpenTasks.Visible = false;
        //pnlTaskH.Visible = false;
        //pnlOpp.Visible = false;
        //pnlEmail.Visible = false;
        //pnlnotes.Visible = false;
        //pnlSysInfo.Visible = false;
        //menuLeads.Visible = false;

        liOpenTask.Style["display"] = "none";
        adOpenTask.Style["display"] = "none";

        liTaskHistory.Style["display"] = "none";
        adTaskHistory.Style["display"] = "none";

        liOpportunities.Style["display"] = "none";
        adOpportunities.Style["display"] = "none";

        liEmails.Style["display"] = "none";
        adEmails.Style["display"] = "none";

        liNotes.Style["display"] = "none";
        adNotes.Style["display"] = "none";

        liSystemInfo.Style["display"] = "none";
        adSystemInfo.Style["display"] = "none";
    }

    private void CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ContactID", typeof(int));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Phone", typeof(string));
        dt.Columns.Add("Fax", typeof(string));
        dt.Columns.Add("Cell", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Title", typeof(string));

        Session["contacttablelead"] = dt;
    }
    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getCompanyByCustomer(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCompany.DataSource = ds.Tables[0];
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem(":: Select ::", "0"));

            ddlCompanyEdit.DataSource = ds.Tables[0];
            ddlCompanyEdit.DataTextField = "Name";
            ddlCompanyEdit.DataValueField = "CompanyID";
            ddlCompanyEdit.DataBind();
            ddlCompanyEdit.Items.Insert(0, new ListItem(":: Select ::", "0"));

        }
    }
    private void FillProspectScreen(string ID)
    {
        ViewState["edit"] = 1;
        // objGeneralFunctions.ResetFormControlValues(this);
        txtProspectName.Focus();

        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ProspectID = Convert.ToInt32(ID);
        DataSet ds = new DataSet();
        ds = objBL_Customer.getProspectByID(objProp_Customer);


        if (ds.Tables[0].Rows.Count > 0)
        {
            txtProspectName.Text = ds.Tables[0].Rows[0]["name"].ToString();
            lblHeaderLabel.Text = ds.Tables[0].Rows[0]["name"].ToString();
            txtCustomer.Text = ds.Tables[0].Rows[0]["customername"].ToString();
            ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
            ddlType.Text = ds.Tables[0].Rows[0]["type"].ToString();
            ddlSalesperson.SelectedValue = ds.Tables[0].Rows[0]["terr"].ToString();

            txtAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
            txtCity.Text = ds.Tables[0].Rows[0]["city"].ToString();
            ddlState.Text = ds.Tables[0].Rows[0]["state"].ToString();

            ddlCountry.SelectedItem.Text = ds.Tables[0].Rows[0]["Country"].ToString();


            txtZip.Text = ds.Tables[0].Rows[0]["zip"].ToString();
            // txtPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
            txtPhone.Text = ds.Tables[0].Rows[0]["billphone"].ToString();
            lat.Value = ds.Tables[0].Rows[0]["lat"].ToString();
            lng.Value = ds.Tables[0].Rows[0]["lng"].ToString();

            txtBillAddress.Text = ds.Tables[0].Rows[0]["billaddress"].ToString();
            txtBillCity.Text = ds.Tables[0].Rows[0]["billcity"].ToString();
            ddlBillState.Text = ds.Tables[0].Rows[0]["billstate"].ToString();
            txtBillZip.Text = ds.Tables[0].Rows[0]["billzip"].ToString();
            // txtBillPhone.Text = ds.Tables[0].Rows[0]["billphone"].ToString();
            ddlBillCountry.SelectedItem.Text = ds.Tables[0].Rows[0]["billCountry"].ToString();
            txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
            txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
            txtMaincontact.Text = ds.Tables[0].Rows[0]["contact"].ToString();
            txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
            txtWebsite.Text = ds.Tables[0].Rows[0]["website"].ToString();
            ViewState["rol"] = ds.Tables[0].Rows[0]["rol"].ToString();
            hdnRol.Value = ds.Tables[0].Rows[0]["rol"].ToString();
            txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
            ddlCompany.SelectedValue = ds.Tables[0].Rows[0]["EN"].ToString();
            txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
            ddlCompanyEdit.SelectedValue = ds.Tables[0].Rows[0]["EN"].ToString();
            getSource();

            //ddlSource.SelectedItem.Text = ds.Tables[0].Rows[0]["Source"].ToString();
            rcSource.SelectedValue = ds.Tables[0].Rows[0]["Source"].ToString();
            hdnReferral.Value = ds.Tables[0].Rows[0]["Referral"].ToString();
            string referralType = Convert.ToString(ds.Tables[0].Rows[0]["ReferralType"].ToString());
            string referral = Convert.ToString(ds.Tables[0].Rows[0]["Referral"].ToString());
            if (referralType == "")
            {
                chkReferral.Checked = false;
                ddlReferral.Style.Add("display", "none");
                //ddlReferral.Visible = false;
            }
            else
            {
                chkReferral.Checked = true;
                //ddlReferral.Visible = true;
                ddlReferral.Style.Add("display", "block");
                ddlReferral.SelectedItem.Text = referralType;
                if (referralType == "Customer")
                {
                    divDllCustomer.Style.Add("display", "block");
                    divDllVendor.Style.Add("display", "none");
                    divDlltxt.Style.Add("display", "none");
                    //ddlCustomer.SelectedItem.Text = referral;
                    rcCustomer.SelectedValue = referral;

                }
                else if (referralType == "Vendor")
                {
                    divDllCustomer.Style.Add("display", "none");
                    divDllVendor.Style.Add("display", "block");
                    divDlltxt.Style.Add("display", "none");
                    //ddlVendor.SelectedItem.Text = referral;
                    rcVendor.SelectedValue = referral;
                }
                else if (referralType == "Others")
                {
                    divDllCustomer.Style.Add("display", "none");
                    divDllVendor.Style.Add("display", "none");
                    divDlltxt.Style.Add("display", "block");
                    txtReferral.Text = referral;
                }
                else
                {
                    divDllCustomer.Style.Add("display", "none");
                    divDllVendor.Style.Add("display", "none");
                    divDlltxt.Style.Add("display", "none");
                }

            }
            //            ddlStage.SelectedItem.Text = ds.Tables[0].Rows[0]["Stage"].ToString();
            if (ddlBusinessType.Items.Count > 0)
            {
                ddlBusinessType.SelectedValue = ds.Tables[0].Rows[0]["BusinessType"].ToString();
            }
            //     ddlServices.SelectedItem.Text = ds.Tables[0].Rows[0]["Services"].ToString();
            //   txtSource.Text = ds.Tables[0].Rows[0]["source"].ToString();


            if (ds.Tables[0].Rows[0]["createdby"].ToString() != string.Empty)
                lblCreate.Text = ds.Tables[0].Rows[0]["createdby"].ToString() + ", " + ds.Tables[0].Rows[0]["createdate"].ToString();

            if (ds.Tables[0].Rows[0]["lastupdatedby"].ToString() != string.Empty)
                lblUpdate.Text = ds.Tables[0].Rows[0]["lastupdatedby"].ToString() + ", " + ds.Tables[0].Rows[0]["lastupdatedate"].ToString();

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    FillContacts(ds.Tables[1]);
                }
            }

            ViewState["FillTasks"] = ds.Tables[0].Rows[0]["rol"].ToString();
            FillTasks(ds.Tables[0].Rows[0]["rol"].ToString());
            FillOpportunity(ds.Tables[0].Rows[0]["rol"].ToString());
            //GetDocuments();
            //GetMailsfromdb(-1);
            //BindEmails(GetMailsfromdb(-1, string.Empty));
        }
    }

    //private void BindEmails(DataSet ds)
    //{
    //    if (ds != null)
    //    {
    //        gvmail.DataSource = ds.Tables[0];
    //        gvmail.DataBind();
    //        menuLeads.Items[4].Text = "Emails(" + ds.Tables[0].Rows.Count + ")";
    //        ViewState["newmail"] = ds.Tables[0].Rows.Count;
    //        ////lblNewEmail.Text = string.Empty;
    //        //lblEmailCount.Text = string.Empty;
    //        //panel9.Visible = false;
    //        hdnMailct.Value = ds.Tables[0].Rows.Count.ToString();
    //    }
    //    else
    //    {
    //        gvmail.DataBind();
    //        menuLeads.Items[4].Text = "Emails(0)";
    //    }
    //}

    private void GetProspectType()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getProspectType(objProp_Customer);
        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "type";
        ddlType.DataValueField = "type";
        ddlType.DataBind();

        ddlType.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void FillSalesPerson()
    {

        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        if(Request.QueryString["uid"] != null)
        {
            ds = objBL_User.GetSalesPerson(objPropUser, new GeneralFunctions().GetSalesAsigned(), Convert.ToInt32(Request.QueryString["uid"]), "LEAD", "t.SDesc");
        }
        else
        {
            ds = objBL_User.GetSalesPerson(objPropUser, new GeneralFunctions().GetSalesAsigned(), 0, "", "t.SDesc");
        }
        

        ddlSalesperson.DataSource = ds.Tables[0];
        ddlSalesperson.DataTextField = "SDesc";
        ddlSalesperson.DataValueField = "id";
        ddlSalesperson.DataBind();

        ddlSalesperson.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.Name = txtProspectName.Text.Trim();
        objProp_Customer.Address = txtAddress.Text.Trim();
        objProp_Customer.City = txtCity.Text.Trim();
        objProp_Customer.State = ddlState.Text.Trim();
        objProp_Customer.Country = ddlCountry.SelectedItem.Text == "Country" ? "" : ddlCountry.SelectedItem.Text;
        objProp_Customer.Zip = txtZip.Text.Trim();
        objProp_Customer.Phone = txtPhone.Text.Trim();
        objProp_Customer.Cellular = txtCell.Text.Trim();
        objProp_Customer.Contact = txtMaincontact.Text.Trim();
        objProp_Customer.Type = ddlType.SelectedValue;
        objProp_Customer.Status = Convert.ToInt32(ddlStatus.SelectedValue);
        objProp_Customer.Email = txtEmail.Text.Trim();
        objProp_Customer.CustomerName = txtCustomer.Text.Trim();
        if (ddlSalesperson.SelectedValue != string.Empty)
            objProp_Customer.Terr = Convert.ToInt32(ddlSalesperson.SelectedValue);
        objProp_Customer.Billaddress = txtBillAddress.Text.Trim();
        objProp_Customer.BillCity = txtBillCity.Text.Trim();
        objProp_Customer.BillState = ddlBillState.Text.Trim();
        objProp_Customer.BillCountry = ddlBillCountry.SelectedItem.Text == "Country" ? "" : ddlBillCountry.SelectedItem.Text;
        objProp_Customer.BillZip = txtBillZip.Text.Trim();
        // objProp_Customer.BillPhone = txtBillPhone.Text.Trim();
        objProp_Customer.BillPhone = txtPhone.Text.Trim();
        objProp_Customer.Fax = txtFax.Text.Trim();
        objProp_Customer.Website = txtWebsite.Text.Trim();
        objProp_Customer.Lat = lat.Value;
        objProp_Customer.Lng = lng.Value;
        objProp_Customer.Remarks = txtRemarks.Text.Trim();
        objProp_Customer.LastUpdateUser = Session["username"].ToString();

        //    objProp_Customer.Source = txtSource.Text.Trim();
        //if (ddlSource.Items.Count > 0)
        //{ objProp_Customer.Source = ddlSource.SelectedItem.Text; }
        //else
        //{ objProp_Customer.Source = "Source"; }

        if (rcSource.Items.Count > 0)
        { objProp_Customer.Source = rcSource.SelectedValue; }
        else
        { objProp_Customer.Source = "Source"; }

        objProp_Customer._Referral = getReferralValue();
        objProp_Customer.ReferralType = getReferralType();
        //        objProp_Customer._Stage = ddlStage.SelectedItem.Text;
        objProp_Customer._BT = ddlBusinessType.SelectedItem.Text;
        //  objProp_Customer._Service = ddlServices.SelectedItem.Text;

        if (Session["contacttablelead"] != null)
        {
            objProp_Customer.ContactData = (DataTable)Session["contacttablelead"];
        }
        int ProspectID = 0;

        try
        {
            
            if (ViewState["edit"].ToString() == "0" || Convert.ToString(Request.QueryString["t"]) == "c")
            {
                if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                    objProp_Customer.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                else
                    objProp_Customer.EN = 0;
                ProspectID = objBL_Customer.AddProspect(objProp_Customer);
                objGeneralFunctions.ResetFormControlValues(this);
                //gvContacts.DataSource = null;
                //gvContacts.DataBind();
                Session["contacttablelead"] = null;
                CreateTable();
                Session["AddLeadSuccMess"] = "Lead Added Successfully!";
                Response.Redirect("AddProspect.aspx?uid=" + ProspectID);
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                    objProp_Customer.EN = Convert.ToInt32(ddlCompanyEdit.SelectedValue);
                else
                    objProp_Customer.EN = 0;
                objProp_Customer.ProspectID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                ProspectID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objBL_Customer.UpdateProspect(objProp_Customer);
                RadGrid_gvLogs.Rebind();
                ConvertProspectWizard();
                FillContacts(FillContactByROL(Convert.ToInt32(ViewState["rol"])).Tables[0]);
                UpdatePanel1.Update();
                //string strMsg = "Added";
                //strMsg = "Updated";
                if (txtCustomer.Text == string.Empty)
                    txtCustomer.Text = txtProspectName.Text;

                string strScript = string.Empty;
                strScript += "noty({text: 'Lead Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);
                // Update sale person list: FillSalesPerson();
                if (Request.QueryString["uid"] != null)
                {
                    objPropUser.ConnConfig = Session["config"].ToString();
                    var ds = objBL_User.GetSalesPerson(objPropUser, new GeneralFunctions().GetSalesAsigned(), Convert.ToInt32(Request.QueryString["uid"]), "LEAD", "t.SDesc");
                    var selectedID = ddlSalesperson.SelectedValue;
                    ddlSalesperson.DataSource = ds.Tables[0];
                    ddlSalesperson.DataTextField = "SDesc";
                    ddlSalesperson.DataValueField = "id";
                    ddlSalesperson.DataBind();

                    ddlSalesperson.Items.Insert(0, new ListItem(":: Select ::", ""));

                    ddlSalesperson.SelectedValue = selectedID;
                    UpnSalespersonList.Update();
                }

                
                FillRecentProspect();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message;
            if (str.Equals("Prospect name already exists, please use different Prospect name !", StringComparison.InvariantCultureIgnoreCase))
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Location lead name is being used, please use different name.',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(str);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    private void FillRecentProspect()
    {
        //TODO
        //NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
        //masterSalesMaster.FillRecentProspect();
        //masterSalesMaster.FillPendingLeads();
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        RadWindowContact.Title = "Add Contact";
        txtContcName.Text = "";
        txtTitle.Text = "";
        txtContPhone.Text = "";
        txtContFax.Text = "";
        txtContCell.Text = "";
        txtContEmail.Text = "";
        ViewState["EditContactID"] = "";
        string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    //protected void lnkCancelContact_Click(object sender, EventArgs e)
    //{
    //    ModalPopup.Hide();
    //}

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        RadWindowContact.Title = "Edit Contact";

        foreach (GridDataItem item in RadGrid_Contacts.SelectedItems)
        {
            DataTable dt = (DataTable)Session["contacttablelead"];
            Label lblindex = (Label)item.FindControl("lblindex");

            DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];

            txtContcName.Text = dr["Name"].ToString();
            txtTitle.Text = dr["Title"].ToString();
            txtContPhone.Text = dr["Phone"].ToString();
            txtContFax.Text = dr["Fax"].ToString();
            txtContCell.Text = dr["Cell"].ToString();
            txtContEmail.Text = dr["Email"].ToString();
            ViewState["editcon"] = 1;
            ViewState["index"] = lblindex.Text;
        }

        string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lnkSave_Click(sender, e);
        if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["contacttablelead"];

        //foreach (GridViewRow di in gvContacts.Rows)
        //{
        //    CheckBox chkSelectCon = (CheckBox)di.FindControl("chkSelectCon");
        //    Label lblindex = (Label)di.FindControl("lblindex");

        //    if (chkSelectCon.Checked == true)
        //    {
        //        DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];
        //        String ID = dr["contactid"].ToString();
        //        objProp_Customer.ConnConfig = Session["config"].ToString();
        //        objProp_Customer.contact = ID;
        //        objBL_Customer.DeletePhone(objProp_Customer);
        //        dt.Rows.RemoveAt(Convert.ToInt32(lblindex.Text));
        //    }
        //}

        foreach (GridDataItem item in RadGrid_Contacts.SelectedItems)
        {
            Label lblindex = (Label)item.Cells[1].FindControl("lblindex");
            DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];
            String ID = dr["contactid"].ToString();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.contact = ID;
            objBL_Customer.DeletePhone(objProp_Customer);
            dt.Rows.RemoveAt(Convert.ToInt32(lblindex.Text));
        }

        dt.AcceptChanges();
        FillContacts(dt);

    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["contacttablelead"];

        DataRow dr = dt.NewRow();

        dr["Name"] = objGeneralFunctions.Truncate(txtContcName.Text, 50);
        dr["Title"] = objGeneralFunctions.Truncate(txtTitle.Text, 50);
        dr["Phone"] = objGeneralFunctions.Truncate(txtContPhone.Text, 50);
        dr["Fax"] = objGeneralFunctions.Truncate(txtContFax.Text, 22);
        dr["Cell"] = objGeneralFunctions.Truncate(txtContCell.Text, 22);
        dr["Email"] = objGeneralFunctions.Truncate(txtContEmail.Text, 50);

        if (ViewState["editcon"].ToString() == "1")
        {
            dt.Rows.RemoveAt(Convert.ToInt32(ViewState["index"]));
            dt.Rows.InsertAt(dr, Convert.ToInt32(ViewState["index"]));
            ViewState["editcon"] = 0;
        }
        else
        {
            dt.Rows.Add(dr);
        }

        dt.AcceptChanges();

        FillContacts(dt);
        if (Request.QueryString["uid"] != null && Convert.ToString(Request.QueryString["uid"]) != "")
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ContactData = dt;
            objProp_Customer.ProspectID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objBL_Customer.AddProspectContact(objProp_Customer);
        }

        //objGeneralFunctions.ResetFormControlValues(pnlContact);
        //ModalPopup.Hide();

    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["redirect"]))
        {
            Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
        }
        else if (Request.QueryString["opid"] != null)
        {
            Response.Redirect("addopprt.aspx?uid=" + Request.QueryString["opid"].ToString());
        }
        else
        {
            Response.Redirect("prospects.aspx");
        }
        
    }

    private void FillContacts(DataTable dt)
    {
        Session["contacttablelead"] = dt;

        RadGrid_Contacts.VirtualItemCount = dt.Rows.Count;
        RadGrid_Contacts.DataSource = dt;
        RadGrid_Contacts.Rebind();


        //gvContacts.DataSource = dt;
        //gvContacts.DataBind();
        //menuLeads.Items[0].Text = "Contacts(" + dt.Rows.Count + ")";

    }

    private DataSet FillContactByROL(int rol)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ROL = rol;
        DataSet ds = new DataSet();
        return ds = objBL_Customer.getContactByRolID(objProp_Customer);
    }

    private void FillTasks(string name)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = "t.rol";
        objProp_Customer.SearchValue = name;
        objProp_Customer.StartDate = string.Empty;
        objProp_Customer.EndDate = string.Empty;
        objProp_Customer.Mode = 1;
        objProp_Customer.Screen = "Sales";
        objProp_Customer.Ref = !string.IsNullOrEmpty(Request.QueryString["uid"]) ? Convert.ToInt32(Request.QueryString["uid"]) : 0;
        ds = objBL_Customer.getTasks(objProp_Customer);
        //DataView dv = ds.Tables[0].DefaultView;
        //dv.Sort = "ID desc";
        //DataTable sortedDT = dv.ToTable();

        //gvTasks.DataSource = sortedDT;
        //gvTasks.DataBind();
        RadGrid_Tasks.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Tasks.DataSource = ds.Tables[0];
        RadGrid_Tasks.Rebind();

        //menuLeads.Items[1].Text = "Open Tasks(" + ds.Tables[0].Rows.Count + ")";

        objProp_Customer.Mode = 0;
        objProp_Customer.Screen = "Sales";
        objProp_Customer.Ref = !string.IsNullOrEmpty(Request.QueryString["uid"]) ? Convert.ToInt32(Request.QueryString["uid"]) : 0;
        ds = objBL_Customer.getTasks(objProp_Customer);
        //gvTasksCompleted.DataSource = ds.Tables[0];
        //gvTasksCompleted.DataBind();
        RadGrid_TasksCompleted.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_TasksCompleted.DataSource = ds.Tables[0];
        RadGrid_TasksCompleted.Rebind();
        //menuLeads.Items[2].Text = "Task History(" + ds.Tables[0].Rows.Count + ")";
    }

    private void FillOpportunity(string name)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = "l.rol";
        objProp_Customer.SearchValue = name;
        objProp_Customer.StartDate = string.Empty;
        objProp_Customer.EndDate = string.Empty;

        ds = objBL_Customer.getOpportunityNew(objProp_Customer);
        //gvOpportunity.DataSource = ds.Tables[0];
        //gvOpportunity.DataBind();

        RadGrid_Opportunity.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Opportunity.DataSource = ds.Tables[0];
        //RadGrid_Opportunity.Rebind();

        //menuLeads.Items[3].Text = "Opportunities(" + ds.Tables[0].Rows.Count + ")";

    }

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {

        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string MIME = string.Empty;
            if (FileUpload1.HasFile)
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";
                filename = FileUpload1.FileName;
                fullpath = savepath + filename;
                MIME = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                if (File.Exists(fullpath))
                {
                    filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                    fullpath = savepath + filename;
                }

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                FileUpload1.SaveAs(fullpath);
            }

            objMapData.Screen = "SalesLead";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objMapData.TempId = "0";
            objMapData.FileName = filename;
            objMapData.DocTypeMIME = MIME;
            objMapData.FilePath = fullpath;
            objMapData.Subject = txtNoteSub.Text.Trim();
            objMapData.Body = txtNoteBody.Text.Trim();
            objMapData.Mode = Convert.ToInt16(ViewState["notesmode"]);
            if (ViewState["notesmode"].ToString() == "1")
                objMapData.DocID = Convert.ToInt32(hdnNoteID.Value);
            else
                objMapData.DocID = 0;
            objMapData.ConnConfig = Session["config"].ToString();
            objBL_MapData.AddFile(objMapData);
            GetDocuments();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void GetDocuments()
    {
        objMapData.Screen = "SalesLead";
        objMapData.TempId = "0";
        objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());

        objMapData.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_MapData.GetDocuments(objMapData);

        RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Documents.DataSource = ds.Tables[0];
        RadGrid_Documents.Rebind();

        //gvDocuments.DataSource = ds.Tables[0];
        //gvDocuments.DataBind();

        //menuLeads.Items[5].Text = "Notes and Attachments(" + ds.Tables[0].Rows.Count + ")";
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
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    protected void lblName_Click(object sender, EventArgs e)
    {
        if (hdnViewDocument.Value == "Y")
        {
            LinkButton btn = (LinkButton)sender;

            string[] CommandArgument = btn.CommandArgument.Split(',');

            string FileName = CommandArgument[0];

            string FilePath = CommandArgument[1];

            DownloadDocument(FilePath, FileName);
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
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteErrorWarning", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        //foreach (GridViewRow di in gvDocuments.Rows)
        //{
        //    CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
        //    Label lblID = (Label)di.FindControl("lblId");

        //    if (chkSelected.Checked == true)
        //    {
        //        DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
        //    }
        //}


        foreach (GridDataItem item in RadGrid_Documents.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
        }
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objBL_MapData.DeleteFile(objMapData);
            GetDocuments();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkAddNote_Click(object sender, EventArgs e)
    {
        if (hdnAddeDocument.Value == "Y")
        {
            txtNoteSub.Text = "";
            txtNoteBody.Text = "";
            string script = "function f(){$find(\"" + RadWindowNotes.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

            ViewState["notesmode"] = 0;
        }
    }

    protected void lnkEditNote_Click(object sender, EventArgs e)
    {
        //objGeneralFunctions.ResetFormControlValues(pnlAttach);
        //foreach (GridViewRow di in gvDocuments.Rows)
        //{
        //    CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
        //    Label lblID = (Label)di.FindControl("lblID");
        //    Label lblSub = (Label)di.FindControl("lblSub");
        //    Label lblBody = (Label)di.FindControl("lblBody");

        //    if (chkSelect.Checked == true)
        //    {
        //        ModalPopup.Show();
        //        pnlContact.Visible = false;
        //        pnlAttach.Visible = true;

        //        txtNoteSub.Text = lblSub.Text;
        //        txtNoteBody.Text = lblBody.Text;
        //        hdnNoteID.Value = lblID.Text;
        //        ViewState["notesmode"] = 1;
        //    }
        //}

        foreach (GridDataItem item in RadGrid_Documents.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblID");
            Label lblSub = (Label)item.FindControl("lblSub");
            Label lblBody = (Label)item.FindControl("lblBody");

            txtNoteSub.Text = lblSub.Text;
            txtNoteBody.Text = lblBody.Text;
            hdnNoteID.Value = lblID.Text;
            ViewState["notesmode"] = 1;
        }

        string script = "function f(){$find(\"" + RadWindowNotes.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void lnkRefreshMails_Click(object sender, EventArgs e)
    {
        //if (Application["pop3"] == null)
        //{
        //    Application["pop3"] = 0;
        //}

        //if ((int)Application["pop3"] == 0)
        //{
        //    Application["pop3"] = 1;
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        objGeneral.ConnConfig = Session["config"].ToString();
        //        ds = objBL_General.GetEmailAccounts(objGeneral);
        //        DataSet dsEmail = objBL_General.getCRMEmails(objGeneral);
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            ////Thread email = new Thread(delegate()
        //            ////   {
        //            ////try
        //            ////{

        //            string host = dr["inserver"].ToString();
        //            string user = dr["inusername"].ToString();
        //            string pass = dr["inpassword"].ToString();
        //            string port = dr["inport"].ToString();
        //            int Userid = Convert.ToInt32(dr["Userid"]);
        //            string LastFetch = dr["lastfetch"].ToString();
        //            objGeneral.AccountID = user;
        //            int MAXUID = objBL_General.GetMAXEmailUID(objGeneral);

        //            objGeneralFunctions.DownloadMailsIMAP(host, user, pass, port, Userid, Session["config"].ToString(), MAXUID, dsEmail);

        //            //objGeneralFunctions.DownloadMails(host, user, pass, port, Userid, Session["config"].ToString());
        //            ////}
        //            ////catch(Exception ex)
        //            ////{
        //            ////    log(ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace);
        //            ////}
        //            ////  });
        //            ////email.IsBackground = true;
        //            ////email.Start();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        //    }
        //    finally
        //    {
        //        Application["pop3"] = 0;
        //    }
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr1", "noty({text: 'Mail download in progress by another user. Please refresh to get downloaded mails.',  type : 'information', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout: 5000, theme : 'noty_theme_default',  closable : true});", true);
        //}

        //BindEmails(GetMailsfromdb(-1, string.Empty));
    }

    //private void DownloadMails(string host, string user, string pass, string port, int userid)
    //{
    //    Pop3Client pop3Client = new Pop3Client();

    //    try
    //    {
    //        if (pop3Client.Connected)
    //            pop3Client.Disconnect();

    //        pop3Client.Connect(host.Trim(), int.Parse(port.Trim()), true);
    //        pop3Client.Authenticate(user.Trim(), pass.Trim());

    //        int count = pop3Client.GetMessageCount();
    //        List<string> uids = pop3Client.GetMessageUids();

    //        objGeneral.ConnConfig = Session["config"].ToString();
    //        objGeneral.AccountID = user.Trim();
    //        DataSet ds = objBL_General.GetMsgUID(objGeneral);
    //        List<string> seenUids = ds.Tables[0].AsEnumerable()
    //                               .Select(r => r.Field<string>("UID"))
    //                               .ToList();

    //        for (int i = 0; i < uids.Count; i++)
    //        {
    //            string currentUidOnServer = uids[i];
    //            if (!seenUids.Contains(currentUidOnServer))
    //            {
    //                try
    //                {
    //                    Message unseenMessage = pop3Client.GetMessage(i + 1);

    //                    var AID = System.Guid.NewGuid();
    //                    objGeneral.From = Convert.ToString(unseenMessage.Headers.From.Address);
    //                    objGeneral.to = Convert.ToString(string.Join(",", objGeneralFunctions.toStringArray(unseenMessage.Headers.To)));
    //                    objGeneral.cc = Convert.ToString(string.Join(",", objGeneralFunctions.toStringArray(unseenMessage.Headers.Cc)));
    //                    objGeneral.bcc = Convert.ToString(string.Join(",", objGeneralFunctions.toStringArray(unseenMessage.Headers.Bcc)));
    //                    objGeneral.subject = Convert.ToString(unseenMessage.Headers.Subject);
    //                    objGeneral.sentdate = unseenMessage.Headers.DateSent;
    //                    //objGeneral.date = Convert.ToString(unseenMessage.Headers.Date);
    //                    objGeneral.Attachments = unseenMessage.FindAllAttachments().Count();
    //                    objGeneral.msgid = Convert.ToString(unseenMessage.Headers.MessageId);
    //                    objGeneral.uid = currentUidOnServer;
    //                    objGeneral.GUID = AID;
    //                    objGeneral.type = 0;
    //                    objGeneral.userid = userid;
    //                    objGeneral.AccountID = user.Trim();
    //                    objGeneral.ConnConfig = Session["config"].ToString();
    //                    int success = objBL_General.AddEmails(objGeneral);

    //                    if (success == 1)
    //                    {
    //                        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
    //                        string savepath = savepathconfig + @"\mails\";
    //                        if (!Directory.Exists(savepath))
    //                        {
    //                            Directory.CreateDirectory(savepath);
    //                        }
    //                        string filename = AID.ToString() + ".eml";
    //                        FileInfo file = new FileInfo(savepath + filename);
    //                        unseenMessage.Save(file);
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
    //                    throw ex;
    //                }
    //            }
    //        }
    //    }
    //    catch (InvalidLoginException)
    //    {
    //        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server did not accept the user credentials!", true);
    //        throw new Exception("The server did not accept the user credentials!");
    //    }
    //    catch (PopServerNotFoundException)
    //    {
    //        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server could not be found", true);
    //        throw new Exception("The server could not be found");
    //    }
    //    catch (PopServerLockedException)
    //    {
    //        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", true);
    //        throw new Exception("The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?");
    //    }
    //    catch (LoginDelayException)
    //    {
    //        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "Login not allowed. Server enforces delay between logins. Have you connected recently?", true);
    //        throw new Exception("Login not allowed. Server enforces delay between logins. Have you connected recently?");
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //        //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //int newmail = 0;
        //if (ViewState["newmail"] != null)
        //{
        //    newmail = Convert.ToInt32(ViewState["newmail"].ToString());
        //}
        //DataSet ds = GetMailsfromdb(-1, string.Empty);

        //if (newmail != ds.Tables[0].Rows.Count)
        //{
        //    if (ViewState["newmail"] != null)
        //    {
        //        //lblNewEmail.Text = Convert.ToString(ds.Tables[0].Rows.Count - newmail) + " New Email(s)";
        //        lblEmailCount.Text = Convert.ToString(ds.Tables[0].Rows.Count - newmail) + " New Email(s)";
        //        panel9.Visible = true;
        //    }
        //}
        ////lblEmailCount.Text = Convert.ToString(newmail) + " New Email(s)";
        ////panel9.Visible = true;
        ////System.Threading.Thread.Sleep(3000);
    }

    //protected void Page_PreRender(Object o, EventArgs e)
    //{
    //    UpdateProgress up = (UpdateProgress)Page.Master.Master.FindControl("UpdateProgress1");
    //    up.Visible = false;
    //}

    //[System.Web.Services.WebMethod(EnableSession = true)]
    //[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    //public static string CheckEmail(string rol, int type, int uid)
    //{
    //    int mails = 0;
    //    //DataSet ds = null;
    //    BL_General objBL_General = new BL_General();
    //    General objGeneral = new General();
    //    if (rol.Trim() != string.Empty)
    //    {
    //        objGeneral.OrderBy = "";

    //        objGeneral.ConnConfig = HttpContext.Current.Session["config"].ToString();
    //        objGeneral.type = type;
    //        objGeneral.rol = Convert.ToInt32(rol);
    //        objGeneral.userid = Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
    //        if (type == -2)
    //        {
    //            objGeneral.RegID = "[OP-" + uid.ToString() + "]";
    //            objGeneral.rol = 0;
    //        }
    //        mails = objBL_General.GetMailsCount(objGeneral);
    //    }
    //    return mails.ToString();
    //}
    private void getSource()
    {
        DataSet dsSources = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        dsSources = objBL_Customer.getSource(objProp_Customer);
        DataTable dt = dsSources.Tables[0];
        DataRow toInsert = dt.NewRow();
        toInsert["fdesc"] = ":: Select ::";
        dt.Rows.InsertAt(toInsert, 0);

        //ddlSource.DataSource = dt;
        //ddlSource.DataTextField = "fdesc";
        //ddlSource.DataValueField = "fdesc";
        //ddlSource.DataBind();

        rcSource.DataSource = dt;
        rcSource.DataTextField = "fdesc";
        rcSource.DataValueField = "fdesc";
        rcSource.DataBind();

        //if (Request.QueryString["uid"] != null) 
        //{ 
        //ddlSource.SelectedItem.Text=dsSources.Tables[0].Columns[]
        //}
    }
    private void getCustomer()
    {
        DataSet dsCustomers = new DataSet();
        objProp_Customer.DBName = Session["DBName"].ToString();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        Int32 UserID = Convert.ToInt32(Session["UserID"].ToString());
        dsCustomers = objBL_Customer.getUserAuthorization(objProp_Customer, UserID, new GeneralFunctions().GetSalesAsigned());
        dsCustomers.Tables[0].DefaultView.Sort = "Name ASC";
        //ddlCustomer.DataSource = dsCustomers.Tables[0];

        //ddlCustomer.DataTextField = "Name";
        //ddlCustomer.DataValueField = "Name";
        //ddlCustomer.DataBind();

        rcCustomer.DataSource = dsCustomers.Tables[0];
        rcCustomer.DataTextField = "Name";
        rcCustomer.DataValueField = "Name";
        rcCustomer.DataBind();


        //if (Request.QueryString["uid"] != null) 
        //{ 
        //ddlSource.SelectedItem.Text=dsSources.Tables[0].Columns[]
        //}
    }
    private void getVendor()
    {
        DataSet dsVendor = new DataSet();
        obj_vendor.ConnConfig = Session["config"].ToString();
        dsVendor = objBL_vendor.GetAll(obj_vendor);
        dsVendor.Tables[0].DefaultView.Sort = "Acct ASC";
        //ddlVendor.DataSource = dsVendor.Tables[0];
        //ddlVendor.DataTextField = "Acct";
        //ddlVendor.DataValueField = "Acct";
        //ddlVendor.DataBind();
        rcVendor.DataSource = dsVendor.Tables[0];
        rcVendor.DataTextField = "Acct";
        rcVendor.DataValueField = "Acct";
        rcVendor.DataBind();

        //if (Request.QueryString["uid"] != null) 
        //{ 
        //ddlSource.SelectedItem.Text=dsSources.Tables[0].Columns[]
        //}
    }
    protected void btnAddSource_Click(object sender, EventArgs e)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.Source = txtnewSource.Text;
        objBL_Customer.AddSource(objProp_Customer);
        getSource();
        rcSource.SelectedValue = txtnewSource.Text;
        string script = "function f(){$find(\"" + RadWindowSource.ClientID + "\").close(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    private string getReferralValue()
    {
        string refVal = string.Empty;
        if (chkReferral.Checked == true)
        {
            string val = ddlReferral.SelectedItem.Text;

            switch (val)
            {
                case "Customer":
                    //refVal = ddlCustomer.SelectedItem.Text;
                    refVal = rcCustomer.SelectedValue;
                    break;
                case "Vendor":
                    //refVal = ddlVendor.SelectedItem.Text;
                    refVal = rcVendor.SelectedValue;
                    break;
                case "Others":
                    refVal = txtReferral.Text;
                    break;
            }
        }
        return refVal;

    }

    private string getReferralType()
    {
        string refVal = string.Empty;
        if (chkReferral.Checked == true)
        {
            refVal = ddlReferral.SelectedItem.Text;

        }
        return refVal;

    }
    //private void FillStages()
    //{

    //    objProp_Customer.ConnConfig = Session["config"].ToString();
    //    DataSet ds = new DataSet();
    //    ds = objBL_Customer.getStages(objProp_Customer);
    //    ddlStage.DataSource = ds.Tables[0];
    //    ddlStage.DataTextField = "Description";
    //    ddlStage.DataValueField = "Description";
    //    ddlStage.DataBind();
    //    lblStage.Text = ds.Tables[0].Rows[0]["Label"].ToString();

    //}


    private void FillBT()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getBT(objProp_Customer);



        if (ds.Tables[0].Rows.Count > 0)
        {
            lblBusinessType.Text = ds.Tables[0].Rows[0]["Label"].ToString();
            ddlBusinessType.DataSource = ds.Tables[0];
            ddlBusinessType.DataTextField = "Description";
            ddlBusinessType.DataValueField = "Description";
            ddlBusinessType.DataBind();
            ddlBusinessType.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        else
        {
            lblBusinessType.Text = "Business Type";
            ddlBusinessType.Items.Insert(0, new ListItem(":: Select ::", ""));
        }

        
    }
    //private void FillService()
    //{
    //    objProp_Customer.ConnConfig = Session["config"].ToString();
    //    DataSet ds = new DataSet();
    //    ds = objBL_Customer.getService(objProp_Customer);
    //    ddlServices.DataSource = ds.Tables[0];
    //    ddlServices.DataTextField = "Description";
    //    ddlServices.DataValueField = "Description";
    //    ddlServices.DataBind();
    //    lblServices.Text = ds.Tables[0].Rows[0]["Label"].ToString();
    //}


    protected void lnkDeleteTask_Click(object sender, EventArgs e)
    {
        #region Validate Grid View Selection
        Boolean validate = false;
        //foreach (GridViewRow row in gvTasks.Rows)
        //{
        //    HiddenField hdID = (HiddenField)row.FindControl("hdID");
        //    CheckBox chkSelectCon = (CheckBox)row.FindControl("chkSelectCon");
        //    if (chkSelectCon.Checked == true)
        //    {
        //        if (hdID.Value != "")
        //        {
        //            validate = true;
        //        }
        //    }
        //}
        foreach (GridDataItem item in RadGrid_Tasks.SelectedItems)
        {
            HiddenField hdID = (HiddenField)item.FindControl("hdID");
            if (hdID.Value != "")
            {
                validate = true;
            }
        }

        if (validate == false)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Please select at least one row.',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            return;
        }

        #endregion

        #region Delete Task from TODO Table
        Lead data = new Lead();
        data.ConnConfig = Session["config"].ToString();
        //foreach (GridViewRow row in gvTasks.Rows)
        //{
        //    HiddenField hdID = (HiddenField)row.FindControl("hdID");
        //    CheckBox chkSelectCon = (CheckBox)row.FindControl("chkSelectCon");
        //    if (chkSelectCon.Checked == true)
        //    {
        //        if (hdID.Value != "")
        //        {
        //            data.ID = Convert.ToInt32(hdID.Value);
        //            obj_Lead.DeleteTaskByID(data);
        //        }
        //    }

        //}
        foreach (GridDataItem item in RadGrid_Tasks.SelectedItems)
        {
            HiddenField hdID = (HiddenField)item.FindControl("hdID");
            if (hdID.Value != "")
            {
                data.ID = Convert.ToInt32(hdID.Value);
                obj_Lead.DeleteTaskByID(data);
            }
        }
        #endregion

        FillTasks(Convert.ToString(ViewState["FillTasks"]));
    }

    protected void lnkCloseTask_Click(object sender, EventArgs e)
    {
        #region Validate Grid View Selection
        Boolean validate = false;
        //foreach (GridViewRow row in gvTasks.Rows)
        //{
        //    HiddenField hdID = (HiddenField)row.FindControl("hdID");
        //    CheckBox chkSelectCon = (CheckBox)row.FindControl("chkSelectCon");
        //    if (chkSelectCon.Checked == true)
        //    {
        //        if (hdID.Value != "")
        //        {
        //            validate = true;
        //        }
        //    }

        //}
        foreach (GridDataItem item in RadGrid_Tasks.SelectedItems)
        {
            HiddenField hdID = (HiddenField)item.FindControl("hdID");
            if (hdID.Value != "")
            {
                validate = true;
            }
        }

        if (validate == false)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Please select at least one row.',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            return;
        }

        #endregion


        #region Change the status of the Task And Shift it from TODO table to DONE Table

        objProp_Customer.ConnConfig = Session["config"].ToString();

        //foreach (GridViewRow row in gvTasks.Rows)
        //{
        //    HiddenField hdID = (HiddenField)row.FindControl("hdID");
        //    CheckBox chkSelectCon = (CheckBox)row.FindControl("chkSelectCon");
        //    DataSet ds = new DataSet();
        //    if (chkSelectCon.Checked == true)
        //    {
        //        if (hdID.Value != "")
        //        {
        //            objProp_Customer.TaskID = Convert.ToInt32(hdID.Value);
        //            objProp_Customer.DueDate = DateTime.Now;
        //            objProp_Customer.TimeDue = Convert.ToDateTime("01/01/1900 " + DateTime.Now.ToLongTimeString());
        //            String UserName = Convert.ToString(Session["Username"]);
        //            objProp_Customer.Username = UserName;
        //            objProp_Customer.Desc = UserName + " --- " + Convert.ToString(DateTime.Now);

        //            ds = objBL_Customer.UpdateTaskToClose(objProp_Customer);


        //        }
        //    }
        //}

        foreach (GridDataItem item in RadGrid_Tasks.SelectedItems)
        {
            HiddenField hdID = (HiddenField)item.FindControl("hdID");
            DataSet ds = new DataSet();
            objProp_Customer.TaskID = Convert.ToInt32(hdID.Value);
            objProp_Customer.DueDate = DateTime.Now;
            objProp_Customer.TimeDue = Convert.ToDateTime("01/01/1900 " + DateTime.Now.ToLongTimeString());
            String UserName = Convert.ToString(Session["Username"]);
            objProp_Customer.Username = UserName;
            objProp_Customer.Desc = UserName + " --- " + Convert.ToString(DateTime.Now);
            ds = objBL_Customer.UpdateTaskToClose(objProp_Customer);
        }
        #endregion

        FillTasks(Convert.ToString(ViewState["FillTasks"]));
    }

    protected void RadGrid_Contacts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Contacts.AllowCustomPaging = !ShouldApplySortFilterOrGroup();

        if (Request.QueryString["uid"] != null)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProspectID = Convert.ToInt32(Request.QueryString["uid"]);
            DataSet ds = new DataSet();
            ds = objBL_Customer.getProspectByID(objProp_Customer);

            RadGrid_Contacts.VirtualItemCount = ds.Tables[1].Rows.Count;
            RadGrid_Contacts.DataSource = ds.Tables[1];
        }
        else
        {
            RadGrid_Contacts.DataSource = string.Empty;
        }
    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Contacts.MasterTableView.FilterExpression != "" ||
            (RadGrid_Contacts.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Contacts.MasterTableView.SortExpressions.Count > 0;
    }


    protected void RadGrid_Tasks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Tasks.AllowCustomPaging = !ShouldApplySortFilterOrGroupTasks();
        if (Request.QueryString["uid"] != null)
        {
            DataSet ds = new DataSet();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.SearchBy = "t.rol";
            objProp_Customer.SearchValue = Convert.ToString(ViewState["FillTasks"]);
            objProp_Customer.StartDate = string.Empty;
            objProp_Customer.EndDate = string.Empty;
            objProp_Customer.Mode = 1;
            objProp_Customer.Screen = "Lead";
            objProp_Customer.Ref = Convert.ToInt32(Request.QueryString["uid"]);
            ds = objBL_Customer.getTasks(objProp_Customer);
            RadGrid_Tasks.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Tasks.DataSource = ds.Tables[0];
        }
    }

    bool isGroupingTasks = false;
    public bool ShouldApplySortFilterOrGroupTasks()
    {
        return RadGrid_Tasks.MasterTableView.FilterExpression != "" ||
            (RadGrid_Tasks.MasterTableView.GroupByExpressions.Count > 0 || isGroupingTasks) ||
            RadGrid_Tasks.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_TasksCompleted_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_TasksCompleted.AllowCustomPaging = !ShouldApplySortFilterOrGroupTasksCompleted();
        if (Request.QueryString["uid"] != null)
        {
            DataSet ds = new DataSet();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.SearchBy = "t.rol";
            objProp_Customer.SearchValue = Convert.ToString(ViewState["FillTasks"]);
            objProp_Customer.StartDate = string.Empty;
            objProp_Customer.EndDate = string.Empty;
            objProp_Customer.Mode = 0;
            objProp_Customer.Screen = "Lead";
            objProp_Customer.Ref = Convert.ToInt32(Request.QueryString["uid"]);
            ds = objBL_Customer.getTasks(objProp_Customer);
            RadGrid_TasksCompleted.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_TasksCompleted.DataSource = ds.Tables[0];
        }

    }

    bool isGroupingTasksCompleted = false;
    public bool ShouldApplySortFilterOrGroupTasksCompleted()
    {
        return RadGrid_TasksCompleted.MasterTableView.FilterExpression != "" ||
            (RadGrid_TasksCompleted.MasterTableView.GroupByExpressions.Count > 0 || isGroupingTasksCompleted) ||
            RadGrid_TasksCompleted.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Opportunity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Opportunity.AllowCustomPaging = !ShouldApplySortFilterOrGroupOpportunity();

        if (ViewState["rol"] != null && ViewState["rol"].ToString() != string.Empty)
            FillOpportunity(ViewState["rol"].ToString());
        //if (Request.QueryString["uid"] != null)
        //{
        //    FillProspectScreen(Request.QueryString["uid"].ToString());
        //}
    }

    bool isGroupingOpportunity = false;
    public bool ShouldApplySortFilterOrGroupOpportunity()
    {
        return RadGrid_Opportunity.MasterTableView.FilterExpression != "" ||
            (RadGrid_Opportunity.MasterTableView.GroupByExpressions.Count > 0 || isGroupingOpportunity) ||
            RadGrid_Opportunity.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Mail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        RadGrid_Mail.AllowCustomPaging = !ShouldApplySortFilterOrGroupMail();

        if (ViewState["rol"] != null)
        {
            if (ViewState["rol"].ToString().Trim() != string.Empty)
            {
                DataSet ds = null;
                objGeneral.OrderBy = "";
                objGeneral.ConnConfig = Session["config"].ToString();
                objGeneral.type = -1;
                objGeneral.rol = Convert.ToInt32(ViewState["rol"]);
                objGeneral.userid = Convert.ToInt32(Session["userid"].ToString());
                ds = objBL_General.GetMails(objGeneral);

                RadGrid_Mail.VirtualItemCount = ds.Tables[0].Rows.Count;
                RadGrid_Mail.DataSource = ds.Tables[0];
            }
        }
    }

    bool isGroupingMail = false;
    public bool ShouldApplySortFilterOrGroupMail()
    {
        return RadGrid_Mail.MasterTableView.FilterExpression != "" ||
            (RadGrid_Mail.MasterTableView.GroupByExpressions.Count > 0 || isGroupingMail) ||
            RadGrid_Mail.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Documents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Documents.AllowCustomPaging = !ShouldApplySortFilterOrGroupDocuments();
        if (Request.QueryString["uid"] != null)
        {
            objMapData.Screen = "SalesLead";
            objMapData.TempId = "0";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());

            objMapData.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_MapData.GetDocuments(objMapData);
            RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Documents.DataSource = ds.Tables[0];
        }
    }

    bool isGroupingDocuments = false;
    public bool ShouldApplySortFilterOrGroupDocuments()
    {
        return RadGrid_Documents.MasterTableView.FilterExpression != "" ||
            (RadGrid_Documents.MasterTableView.GroupByExpressions.Count > 0 || isGroupingDocuments) ||
            RadGrid_Documents.MasterTableView.SortExpressions.Count > 0;
    }

    protected void AddSource_Click(object sender, EventArgs e)
    {
        txtnewSource.Text = "";

        string script = "function f(){$find(\"" + RadWindowSource.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void btnCompanyPopUp_Click(object sender, EventArgs e)
    {
        string script = "function f(){$find(\"" + RadWindowCompany.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["prospects"];
            string url = "addprospect.aspx?uid=" + dt.Rows[0]["ID"];
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
            dt = (DataTable)Session["prospects"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addprospect.aspx?uid=" + dt.Rows[index - 1]["ID"];
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
            dt = (DataTable)Session["prospects"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addprospect.aspx?uid=" + dt.Rows[index + 1]["ID"];
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
            dt = (DataTable)Session["prospects"];
            string url = "addprospect.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
    }

    #region Equipment part
    protected void lnkAddEQ_Click(object sender, EventArgs e)
    {
        string url = txtProspectName.Text;
        Response.Redirect("addequipment.aspx?page=addprospect&lid=" + Request.QueryString["uid"].ToString() + "&locname=" + Server.UrlEncode(url));
    }
    protected void lnkDeleteEQ_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Equip.SelectedItems)
        {
            Label lblUserID = (Label)item.FindControl("lblId");
            DeleteEquipment(Convert.ToInt32(lblUserID.Text));
        }
    }
    private void DeleteEquipment(int EquipID)
    {
        objPropUser.EquipID = EquipID;
        objPropUser.ConnConfig = Session["config"].ToString();

        try
        {
            objBL_User.DeleteLeadEquipment(objPropUser);
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccEq", "noty({text: 'Equipment deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            RadGrid_Equip.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrEq", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    protected void btnCopyEQ_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Equip.SelectedItems)
        {
            Label lblUserID = (Label)item.FindControl("lblId");
            Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text + "&t=c&page=addprospect&lid=" + Request.QueryString["uid"].ToString());
        }
    }
    protected void lnkEditEq_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Equip.SelectedItems)
        {
            Label lblUserID = (Label)item.FindControl("lblId");
            Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text + "&page=addprospect&lid=" + Request.QueryString["uid"].ToString());
        }
    }

    protected void RadGrid_Equip_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Equip.AllowCustomPaging = !ShouldApplySortFilterOrGroupEquip();

            if (Request.QueryString["uid"] != null)
            {
                DataSet ds = new DataSet();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.SearchBy = string.Empty;

                objPropUser.ProspectID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objPropUser.InstallDate = string.Empty;
                objPropUser.ServiceDate = string.Empty;
                objPropUser.Price = string.Empty;
                objPropUser.Manufacturer = string.Empty;
                objPropUser.Status = -1;
                objPropUser.building = "All";
                ds = objBL_User.getLeadEquip(objPropUser);

                RadGrid_Equip.VirtualItemCount = ds.Tables[0].Rows.Count;
                RadGrid_Equip.DataSource = ds.Tables[0];
            }
        }
        catch { }
    }
    protected void RadGrid_Equip_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Equip.Items)
        {
            Label lblID = (Label)item.FindControl("lblId");

            if (hdnEditeEquipment.Value == "Y" || hdnViewEquipment.Value == "Y")
            {
                item.Attributes["ondblclick"] = "location.href='addequipment.aspx?uid=" + lblID.Text + "&page=addprospect&lid=" + Request.QueryString["uid"].ToString() + "'";
            }
            else
            {
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }
        }
    }

    bool isGroupingEquip = false;
    public bool ShouldApplySortFilterOrGroupEquip()
    {
        return RadGrid_Equip.MasterTableView.FilterExpression != "" ||
            (RadGrid_Equip.MasterTableView.GroupByExpressions.Count > 0 || isGroupingEquip) ||
            RadGrid_Equip.MasterTableView.SortExpressions.Count > 0;
    }
    #endregion
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
                objProp_Customer.ProspectID = Convert.ToInt32(Request.QueryString["uid"]);
                dsLog = objBL_Customer.GetProspectLogs(objProp_Customer);
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

    protected void lnkEditOpp_Click(object sender, EventArgs e)
    {
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        foreach (GridDataItem item in RadGrid_Opportunity.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            //Response.Redirect("addopprt.aspx?uid=" + lblID.Text + "&leadId=" + Request.QueryString["uid"]);
            Response.Redirect("addopprt.aspx?uid=" + lblID.Text + "&redirect=" + redirect);
        }
    }

    protected void lnkDeleteOpp_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem item in RadGrid_Opportunity.SelectedItems)
            {
                Label lblID = (Label)item.FindControl("lblId");
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.OpportunityID = Convert.ToInt32(lblID.Text);
                objBL_Customer.DeleteOpportunity(objProp_Customer);
                //OpportunityList();
                RadGrid_Opportunity.Rebind();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyS", "noty({text: 'Opportunity deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }

    protected void lnkExcelOpp_Click(object sender, EventArgs e)
    {
        RadGrid_Opportunity.ExportSettings.FileName = "Opportunity";
        RadGrid_Opportunity.ExportSettings.IgnorePaging = true;
        RadGrid_Opportunity.ExportSettings.ExportOnlyData = true;
        RadGrid_Opportunity.ExportSettings.OpenInNewWindow = true;
        RadGrid_Opportunity.ExportSettings.HideStructureColumns = true;
        RadGrid_Opportunity.MasterTableView.UseAllDataFields = true;
        RadGrid_Opportunity.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Opportunity.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_Opportunity_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 3;
        else
            currentItem = 4;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Opportunity.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Opportunity.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

    protected void RadGrid_Opportunity_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                //if (Convert.ToString(RadGrid_Opportunity.MasterTableView.FilterExpression) != "")
                //    lblRecordCount.Text = totalCount + " Record(s) found";
                //else
                //    lblRecordCount.Text = RadGrid_Opportunity.VirtualItemCount + " Record(s) found";
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

    protected void RadGrid_Opportunity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Clear();
        dt.Columns.Add("EstimateID");
        dt.Columns.Add("Last");

        DataTable dtProject = new DataTable();
        dtProject.Clear();
        dtProject.Columns.Add("ProjectID");
        dtProject.Columns.Add("Last");

        if (e.Item is GridDataItem)
        {
            var sss = e.Item.DataItem;
            Repeater InterestsRepeater = e.Item.FindControl("rptEstimates") as Repeater;
            HiddenField hdnEstimate = e.Item.FindControl("hdnGridEstimate") as HiddenField;
            if (hdnEstimate != null && !string.IsNullOrEmpty(hdnEstimate.Value))
            {
                var estArr = hdnEstimate.Value.Trim().Split(',');
                for (int i = 0; i < estArr.Length; i++)
                {
                    DataRow _temp = dt.NewRow();
                    _temp["EstimateID"] = estArr[i].Trim();
                    if (i == estArr.Length - 1)
                    {
                        _temp["Last"] = "true";
                    }
                    else
                    {
                        _temp["Last"] = "false";
                    }

                    dt.Rows.Add(_temp);
                }
            }
            //Get the instance of the right type
            if (InterestsRepeater != null)
            {
                InterestsRepeater.DataSource = dt;
                InterestsRepeater.DataBind();
            }

            Repeater projectRepeater = e.Item.FindControl("rptProjects") as Repeater;
            HiddenField hdnGridProject = e.Item.FindControl("hdnGridProject") as HiddenField;
            if (hdnGridProject != null && !string.IsNullOrEmpty(hdnGridProject.Value))
            {
                var projArr = hdnGridProject.Value.Trim().Split(',');
                for (int i = 0; i < projArr.Length; i++)
                {
                    DataRow _temp = dtProject.NewRow();
                    _temp["ProjectID"] = projArr[i].Trim();
                    if (i == projArr.Length - 1)
                    {
                        _temp["Last"] = "true";
                    }
                    else
                    {
                        _temp["Last"] = "false";
                    }

                    dtProject.Rows.Add(_temp);
                }
            }
            //Get the instance of the right type
            if (projectRepeater != null)
            {
                projectRepeater.DataSource = dtProject;
                projectRepeater.DataBind();
            }
        }
    }

    protected void LinkButton_Click(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Estimate #")
        {
            Response.Redirect("addestimate.aspx?uid=" + e.CommandArgument + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl));
        }
        else if (e.CommandName == "Project #")
        {
            Response.Redirect("addProject.aspx?uid=" + e.CommandArgument + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl));
        }
    }

    protected void lnkCopyOpp_Click(object sender, EventArgs e)
    {
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        foreach (GridDataItem di in RadGrid_Opportunity.SelectedItems)
        {
            TableCell cell = di["ClientSelectColumn"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblID = (Label)di.FindControl("lblId");

            if (chkSelect.Checked == true)
            {
                Response.Redirect("addopprt.aspx?uid=" + lblID.Text + "&t=c" + "&redirect=" + redirect);
            }
        }
    }
}

public class EditContactModelLead
{
    public String Name { get; set; }
    public String Phone { get; set; }
    public String Fax { get; set; }
    public String Cell { get; set; }
    public String Email { get; set; }
    public String lblIndex { get; set; }
    public Boolean EmailTicket { get; set; }

}
