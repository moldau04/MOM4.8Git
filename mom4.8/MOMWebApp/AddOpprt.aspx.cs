using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Collections.Generic;

public partial class AddOpprt : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
    BL_General objBL_General = new BL_General();
    General objGeneral = new General();
    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            getSource();
        }


        if (!IsPostBack)
        {
            FillJobType();
            FillOpportunityStatus();
            lnkSpecific.Visible = false;
            lnkAllMail.Visible = false;
            //lnkNewEmail.Visible = false;

            ViewState["edit"] = 0;
            FillUsers();
            FillStages();
            FillBT();
            FillProduct();

            

            ViewState["rolapp"] = 0;
            //SalesMaster masterSalesMaster = (SalesMaster)Page.Master;
            //masterSalesMaster.FillPendingRec();

            if (Request.QueryString["uid"] != null)
            {
                // Edit
                if (Request.QueryString["t"] != "c")
                {
                    hdnUID.Value = Request.QueryString["uid"].ToString();
                    lnkSpecific.Visible = true;
                    lnkAllMail.Visible = true;
                    lnkNewEmail.Visible = true;
                    Page.Title = "Edit Opportunity || MOM";

                    ViewState["edit"] = 1;
                    lblHeader.Text = "Edit Opportunity";
                    GetOpportunity();
                    //FillEstimatesLinked(Convert.ToInt32(hdnUID.Value));
                    //HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text + "&assignedTo=" + ddlAssigned.SelectedItem.Text + "&customer=" + txtCompanyName.Text;
                    //HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text + "&assignedTo=" + ddlAssigned.SelectedItem.Text + "&customer=" + txtCompanyName.Text;
                    HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text + "&assignedTo=" + ddlAssigned.SelectedValue + "&customer=" + txtCompanyName.Text + "&screen=opportunity&ref=" + hdnUID.Value;
                    HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text + "&assignedTo=" + ddlAssigned.SelectedValue + "&customer=" + txtCompanyName.Text + "&screen=opportunity&ref=" + hdnUID.Value; ;
                    lnkNewEmail.NavigateUrl = "email.aspx?op=" + Request.QueryString["uid"].ToString() + "&rol=" + hdnId.Value;
                    lnkAddNewEstimate.NavigateUrl = "addestimate.aspx?opp=" + Request.QueryString["uid"].ToString()
                        + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl);
                    lblOpportunityText.Visible = true;
                }
                else// Copy
                {
                    ViewState["edit"] = 2;
                    Page.Title = "Copy Opportunity || MOM";
                    lblHeader.Text = "Copy Opportunity";
                    lblOpportunityText.Visible = false;
                    GetOpportunity();
                }
            }
            else
            {
                //lnkEstimate.Visible = false;
                lblOpportunityText.Visible = false;
                //divEstimate.Style.Add("display", "none");
                Page.Title = "Add Opportunity || MOM";
                lblHeader.Text = "Add Opportunity";
            }

            if (ViewState["edit"].ToString() != "1")
            {
                HideControls();
            }

            #region add opportunity from Lead or Location
            if (Request.QueryString["rol"] != null)
            {
                ViewState["rolapp"] = Request.QueryString["rol"];
                hdnId.Value = Request.QueryString["rol"].ToString();
                txtName.Text = string.IsNullOrEmpty(Request.QueryString["name"]) ? "" : Request.QueryString["name"];
                FillTasks(hdnId.Value);

                //GetDocuments(Convert.ToInt32(Request.QueryString["owner"].ToString()));
                HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
                HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
                lnkNewEmail.NavigateUrl = "email.aspx?rol=" + hdnId.Value;
            }
            if (Request.QueryString["customer"] != null)
            {
                txtCompanyName.Text = Request.QueryString["customer"].ToString();
            }
            if (Request.QueryString["assignedTo"] != null)
            {
                ListItem LI = ddlAssigned.Items.FindByValue(Request.QueryString["assignedTo"].ToString());
                if (LI != null)
                {
                    //ddlAssigned.SelectedItem.Text = Request.QueryString["assignedTo"].ToString();
                    String assignTo = Convert.ToString(Request.QueryString["assignedTo"]);
                    if (assignTo != "")
                    {
                        ddlAssigned.SelectedValue = assignTo;//ddlAssigned.Items.FindByText(assignTo).Value;
                        RequiredFieldValidator44.Enabled = false;
                    }
                }
            }
            if (Request.QueryString["BusinessType"] != null)
            {
                ListItem LI = ddlBusinessType.Items.FindByText(Request.QueryString["BusinessType"].ToString());
                if (LI != null)
                {
                    ddlBusinessType.SelectedItem.Text = Request.QueryString["BusinessType"].ToString();
                }
            }
            if (Request.QueryString["source"] != null)
            {
                ddlSource.SelectedItem.Text = Request.QueryString["source"].ToString();
            }
            #endregion

            #region add opportunity from Customer
            if (Request.QueryString["custId"] != null)
            {
                hdnCustId.Value = hdnOwnerID.Value = Request.QueryString["custId"].ToString();
                hdnProsID.Value = "0";
                lblType.Text = "Existing";
                txtCompanyName.Text = string.IsNullOrEmpty(Request.QueryString["custName"]) ? "" : Request.QueryString["custName"].ToString();
                // Get location name by customer name
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.CustomerID = Convert.ToInt32(hdnCustId.Value);
                var dsloc = objBL_Customer.GetLocationOfCustomerInCaseUnique(objProp_Customer);
                if (dsloc.Tables.Count > 0 && dsloc.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = dsloc.Tables[0].Rows[0]["Tag"].ToString();
                    hdnId.Value = dsloc.Tables[0].Rows[0]["locRol"].ToString();
                }
            }
            #endregion

            //NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
            //masterSalesMaster.FillPendingRec(Convert.ToInt32(ViewState["rolapp"]));
            if (Session["AddEditTaskSuccMess"] != null && Session["AddEditTaskSuccMess"].ToString() != "")
            {
                string strScript = string.Empty;

                strScript += "noty({text: 'Task " + Session["AddEditTaskSuccMess"].ToString() + " Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);
                Session["AddEditTaskSuccMess"] = null;
            }
        }
     

        //Permission();
        UserPermission();
        CompanyPermission();
        //if(hdnId.Value!="" && hdnId.Value!="0")
        //{
        //    GetOpportunity1();
        //}
     
        HighlightSideMenu("SalesMgr", "lnkOpportunities", "SalesMgrSub");

        pnlNext.Visible = false;
        if (Request.QueryString["uid"] != null)
        {
            // Edit
            if (Request.QueryString["t"] != "c")
            {
                pnlNext.Visible = true;
                liNotes.Style["display"] = "";
                adNotes.Style["display"] = "";

                liContacts.Style["display"] = "";
                adContacts.Style["display"] = "";
                liLogs.Style["display"] = "inline-block";
                tbLogs.Style["display"] = "block";
            }
            else
            {
                liNotes.Style["display"] = "none";
                adNotes.Style["display"] = "none";

                liContacts.Style["display"] = "none";
                adContacts.Style["display"] = "none";
            }
        }
        else
        {
            liNotes.Style["display"] = "none";
            adNotes.Style["display"] = "none";

            liContacts.Style["display"] = "none";
            adContacts.Style["display"] = "none";
           
        }

        if (Convert.ToString(Session["strMsg"]) != "")
        {
            String strMsg = Convert.ToString(Session["strMsg"]);

            string strScript = string.Empty;
            strScript += "noty({text: 'Opportunity " + strMsg + " Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);

            Session["strMsg"] = null;
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
    private void getSource()
    {
        DataSet dsSources = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        dsSources = objBL_Customer.getSource(objProp_Customer);
        DataTable dt = dsSources.Tables[0];

        ddlSource.DataSource = dt;
        ddlSource.DataTextField = "fdesc";
        ddlSource.DataValueField = "fdesc";
        ddlSource.DataBind();

        ddlSource.Items.Insert(0, new ListItem("Select", ""));
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

    protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*
        if (ddlStage.SelectedValue == "r.name" || ddlStage.SelectedValue == "l.fdesc")
        {
            txtSearch.Visible = true;
            ddlProbab.Visible = false;
            ddlStatus.Visible = false;
            ddlAssigned.Visible = false;
        }
         */
    }

    private void HideControls()
    {
        //pnlNotes.Visible = false;
        liSystemInfo.Style["display"] = "none";
        adSystemInfo.Style["display"] = "none";
        //menuLeads.Visible = false;
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.Master.FindControl("lnkOpportunities");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl ul = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgrSub");
        //ul.Attributes.Remove("class");
        //ul.Style.Add("display", "block");

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

        if (Session["type"].ToString() != "am")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            /// Ticket ///////////////////------->

            string ticketPermission = ds.Rows[0]["TicketPermission"] == DBNull.Value ? "YYNYYY" : ds.Rows[0]["TicketPermission"].ToString();
            hdnEditeTicket.Value = ticketPermission.Length < 2 ? "Y" : ticketPermission.Substring(1, 1);
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

            string SalesmodulePermission = ds.Rows[0]["SalesManager"] == DBNull.Value ? "Y" : ds.Rows[0]["SalesManager"].ToString();

            if (SalesmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// UserSales ///////////////////------->

            string ProposalPermission = ds.Rows[0]["Proposal"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Proposal"].ToString();
            string ADD = ProposalPermission.Length < 1 ? "Y" : ProposalPermission.Substring(0, 1);
            string Edit = ProposalPermission.Length < 2 ? "Y" : ProposalPermission.Substring(1, 1);
            string Delete = ProposalPermission.Length < 3 ? "Y" : ProposalPermission.Substring(2, 1);
            string View = ProposalPermission.Length < 4 ? "Y" : ProposalPermission.Substring(3, 1);

            if (Request.QueryString["uid"] != null)
            {
                //aImport.Visible = false;
            }
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            else if (Request.QueryString["uid"] == null || (Request.QueryString["t"] != null && Request.QueryString["t"].ToString() == "c"))
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

            string EstimatesPermission = ds.Rows[0]["Estimates"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Estimates"].ToString();
            string ADDEst = EstimatesPermission.Length < 1 ? "Y" : EstimatesPermission.Substring(0, 1);
            string EditEst = EstimatesPermission.Length < 2 ? "Y" : EstimatesPermission.Substring(1, 1);
            string DeleteEst = EstimatesPermission.Length < 2 ? "Y" : EstimatesPermission.Substring(2, 1);
            string ViewEst = EstimatesPermission.Length < 4 ? "Y" : EstimatesPermission.Substring(3, 1);
            string ReportEst = EstimatesPermission.Length < 6 ? "Y" : EstimatesPermission.Substring(5, 1);

            string AwardEstimatesPermission = ds.Rows[0]["AwardEstimates"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["AwardEstimates"].ToString();

            string AwardEstimates = AwardEstimatesPermission.Length < 3 ? "Y" : AwardEstimatesPermission.Substring(2, 1);

            if (AwardEstimates == "N")
            {
                lnkProjectEstimate.Visible = false;
            }
            if (ADDEst == "N")
            {
                lnkAddNewEstimate.Visible = false;
            }
            if (EditEst == "N")
            {
                lnkEditEstimate.Visible = false;
                lnkCopyEstimate.Visible = false;
            }
            if (DeleteEst == "N")
            {
                lnkDeleteEstimate.Visible = false;

            }
            if (ReportEst == "N")
            {
                lnkReportEstimate.Visible = false;
                lnkExcelEstimate.Visible = false;
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
    private void FillProduct()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getService(objProp_Customer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlProduct.DataSource = ds.Tables[0];
            ddlProduct.DataTextField = "Description";
            ddlProduct.DataValueField = "ID";
            ddlProduct.DataBind();
        }
        ddlProduct.Items.Insert(0, new ListItem("Select", ""));
        lblProduct.Text = "Product";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblProduct.Text = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    private void FillOpportunityStatus()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();

        ds = objBL_Customer.getOpportunityStatus(objProp_Customer);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //lblProduct.Text = ds.Tables[0].Rows[0]["Label"].ToString();
            ddlStatus.DataSource = ds.Tables[0];
            ddlStatus.DataTextField = "Name";
            ddlStatus.DataValueField = "ID";
            ddlStatus.DataBind();
        }
        else
        {
            //lblProduct.Text = "Products;
            //ddlProduct.Items.Insert(0, new ListItem("Select", ""));
        }
        ddlStatus.Items.Insert(0, new ListItem("Select", ""));
        ddlStatus.SelectedValue = "";

      
    }

    private void FillBT()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getBT(objProp_Customer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBusinessType.DataSource = ds.Tables[0];
            ddlBusinessType.DataTextField = "Description";
            ddlBusinessType.DataValueField = "Description";
            ddlBusinessType.DataBind();
        }

        ddlBusinessType.Items.Insert(0, new ListItem("Select", ""));
        lblBusinessType.Text = "Business Type";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblBusinessType.Text = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    private void FillStages()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getStages(objProp_Customer);

        ddlStage.DataSource = ds.Tables[0];
        ddlStage.DataTextField = "DescWithProbability";
        ddlStage.DataValueField = "ID";
        ddlStage.DataBind();
        ddlStage.Items.Insert(0, new ListItem("Select", ""));
        lblStage.InnerText = "Stage";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblStage.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    private void GetOpportunity()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.OpportunityID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        DataSet ds = new DataSet();
        ds = objBL_Customer.getOpportunityByID(objProp_Customer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["edit"] != null && ViewState["edit"].ToString() == "1")//Case edit
            {
                ViewState["rolapp"] = ds.Tables[0].Rows[0]["rol"].ToString();
                lblOpportunityID.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                hdnId.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtCompanyName.Text = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                txtOppName.Text = ds.Tables[0].Rows[0]["fDesc"].ToString();
                if (Convert.ToString(ds.Tables[0].Rows[0]["fDesc"]) != "")
                {
                    lblOppNameLabel.Text = " | " + Convert.ToString(ds.Tables[0].Rows[0]["fDesc"]);
                }
                txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                if (ds.Tables[0].Rows[0]["closedate"] != DBNull.Value)
                    txtCloseDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["closedate"]).ToShortDateString();
                ddlProbab.SelectedValue = ds.Tables[0].Rows[0]["Probability"].ToString();
                ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                lblType.Text = ds.Tables[0].Rows[0]["contacttype"].ToString().ToUpper() == "ACCOUNT" ? "Existing" : ds.Tables[0].Rows[0]["contacttype"].ToString();
                hdnOwnerID.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                txtNextStep.Text = ds.Tables[0].Rows[0]["nextstep"].ToString();
                txtDesc.Text = ds.Tables[0].Rows[0]["desc"].ToString();
                //ddlAssigned.SelectedValue = ds.Tables[0].Rows[0]["AssignedToID"].ToString();
                string Terr = ds.Tables[0].Rows[0]["AssignedToID"].ToString();
                ListItem liTerr = ddlAssigned.Items.FindByValue(Terr);
                if (liTerr != null) ddlAssigned.SelectedValue = Terr;
                chkClosed.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["closed"]);
                ddlStage.SelectedValue = ds.Tables[0].Rows[0]["OpportunityStageID"].ToString();

                if (ddlStatus.SelectedItem.Text == "Withdrawn" || ddlStatus.SelectedItem.Text == "Disqualified" || ddlStatus.SelectedItem.Text == "Sold")
                {
                    CloseDateDiv.Style.Add("display", "block");
                }
                else
                {
                    CloseDateDiv.Style.Add("display", "none");
                }
                if (ds.Tables[0].Rows[0]["createdby"].ToString() != string.Empty)
                    lblCreate.Text = ds.Tables[0].Rows[0]["createdby"].ToString() + ", " + ds.Tables[0].Rows[0]["createdate"].ToString();

                if (ds.Tables[0].Rows[0]["lastupdatedby"].ToString() != string.Empty)
                    lblUpdate.Text = ds.Tables[0].Rows[0]["lastupdatedby"].ToString() + ", " + ds.Tables[0].Rows[0]["lastupdatedate"].ToString();

                if (ds.Tables[0].Rows[0]["revenue"] != DBNull.Value)
                {
                    txtAmount.Text = String.Format("{0:C}", Convert.ToDouble(ds.Tables[0].Rows[0]["revenue"]));
                }
                ddlSource.SelectedValue = ds.Tables[0].Rows[0]["source"].ToString();
                FillTasks(ds.Tables[0].Rows[0]["rol"].ToString());
                FillContact(Convert.ToInt32(ds.Tables[0].Rows[0]["rol"].ToString()));
                GetDocuments(Convert.ToInt32(ds.Tables[0].Rows[0]["ownerid"].ToString()));
                FillLocInfo(Convert.ToInt32(ds.Tables[0].Rows[0]["ownerid"].ToString()));
                string ticketid = ds.Tables[0].Rows[0]["ticketid"].ToString();
                string DocumentCount = ds.Tables[0].Rows[0]["DocumentCount"].ToString();
                imgDoc.Visible = DocumentCount == "0" ? false : true;
                if (!string.IsNullOrEmpty(ticketid))
                {
                    ticketurl.InnerText = ticketid;
                    ticketspan.Visible = ticketurl.Visible = true;
                    ticketurl.HRef = "addticket.aspx?id=" + ticketid + "&comp=0&pop=1";
                }
                else { ticketspan.Visible = ticketurl.Visible = false; }
                pnlTicket.Visible = (ticketid != "0");
                ddlBusinessType.SelectedValue = ds.Tables[0].Rows[0]["BusinessType"].ToString();
                ddlProduct.SelectedValue = ds.Tables[0].Rows[0]["Product"].ToString();
                //lnkEstimate.Text = ds.Tables[0].Rows[0]["Estimate"].ToString();
                //if (lnkEstimate.Text == "")
                //{
                //    lnkEstimate.Text = "Add New Estimate";
                //}
                //else
                //{
                //    lnkEstimate.Text = "Estimate #" + ds.Tables[0].Rows[0]["Estimate"].ToString();
                //}
                ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["Department"] != null ? ds.Tables[0].Rows[0]["Department"].ToString() : "";

                var isAllowDeptChange = ds.Tables[0].Rows[0]["EstimateDepartment"] == null || ds.Tables[0].Rows[0]["EstimateDepartment"].ToString() == "";
                ddlDepartment.Enabled = isAllowDeptChange;
                hdnCustId.Value = ds.Tables[0].Rows[0]["CustID"].ToString();
                string roltype= ds.Tables[0].Rows[0]["RolType"].ToString();
                if (hdnCustId.Value != "0" && hdnCustId.Value != "")
                {
                    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustId.Value;
                }
                else if(hdnCustId.Value == "0" && roltype=="0")
                {
                    hdnCustId.Value = ds.Tables[0].Rows[0]["ownerid"].ToString();
                    lnkCustomerID.NavigateUrl ="addprospect.aspx?uid=" + hdnCustId.Value;
                    lnkLocationID.NavigateUrl ="addprospect.aspx?uid=" + hdnCustId.Value;
                }

                else
                    lnkCustomerID.NavigateUrl = "";

                if (hdnCustId.Value != "0" && hdnCustId.Value != "" && roltype != "0")
                {
                    hdnLocId.Value = hdnOwnerID.Value;
                    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocId.Value;
                   
                }
                else
                    lnkCustomerID.NavigateUrl = "";

            }
            else if (ViewState["edit"] != null && ViewState["edit"].ToString() == "2")//Case copy
            {
                lblOpportunityID.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                hdnId.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtCompanyName.Text = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                txtOppName.Text = ds.Tables[0].Rows[0]["fDesc"].ToString();
                if (Convert.ToString(ds.Tables[0].Rows[0]["fDesc"]) != "")
                {
                    lblOppNameLabel.Text = " | " + Convert.ToString(ds.Tables[0].Rows[0]["fDesc"]);
                }
                txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                if (ds.Tables[0].Rows[0]["closedate"] != DBNull.Value)
                    txtCloseDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["closedate"]).ToShortDateString();
                ddlProbab.SelectedValue = ds.Tables[0].Rows[0]["Probability"].ToString();
                ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                lblType.Text = ds.Tables[0].Rows[0]["contacttype"].ToString().ToUpper() == "ACCOUNT" ? "Existing" : ds.Tables[0].Rows[0]["contacttype"].ToString();
                hdnOwnerID.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                txtNextStep.Text = ds.Tables[0].Rows[0]["nextstep"].ToString();
                txtDesc.Text = ds.Tables[0].Rows[0]["desc"].ToString();
                //ddlAssigned.SelectedValue = ds.Tables[0].Rows[0]["AssignedToID"].ToString();
                string Terr = ds.Tables[0].Rows[0]["AssignedToID"].ToString();
                ListItem liTerr = ddlAssigned.Items.FindByValue(Terr);
                if (liTerr != null) ddlAssigned.SelectedValue = Terr;
                chkClosed.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["closed"]);
                ddlStage.SelectedValue = ds.Tables[0].Rows[0]["OpportunityStageID"].ToString();

                if (ddlStatus.SelectedItem.Text == "Withdrawn" || ddlStatus.SelectedItem.Text == "Disqualified" || ddlStatus.SelectedItem.Text == "Sold")
                {

                    CloseDateDiv.Style.Add("display", "block");

                }
                else
                {
                    CloseDateDiv.Style.Add("display", "none");
                }
                if (ds.Tables[0].Rows[0]["createdby"].ToString() != string.Empty)
                    lblCreate.Text = ds.Tables[0].Rows[0]["createdby"].ToString() + ", " + ds.Tables[0].Rows[0]["createdate"].ToString();

                if (ds.Tables[0].Rows[0]["lastupdatedby"].ToString() != string.Empty)
                    lblUpdate.Text = ds.Tables[0].Rows[0]["lastupdatedby"].ToString() + ", " + ds.Tables[0].Rows[0]["lastupdatedate"].ToString();

                if (ds.Tables[0].Rows[0]["revenue"] != DBNull.Value)
                {
                    txtAmount.Text = String.Format("{0:C}", Convert.ToDouble(ds.Tables[0].Rows[0]["revenue"]));
                }
                ddlSource.SelectedValue = ds.Tables[0].Rows[0]["source"].ToString();
                ddlBusinessType.SelectedValue = ds.Tables[0].Rows[0]["BusinessType"].ToString();
                ddlProduct.SelectedValue = ds.Tables[0].Rows[0]["Product"].ToString();
                ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["Department"] != null ? ds.Tables[0].Rows[0]["Department"].ToString() : "";

                var isAllowDeptChange = ds.Tables[0].Rows[0]["EstimateDepartment"] == null || ds.Tables[0].Rows[0]["EstimateDepartment"].ToString() == "";
                ddlDepartment.Enabled = isAllowDeptChange;
                hdnCustId.Value = ds.Tables[0].Rows[0]["CustID"].ToString();


                //if (hdnLocID.Value != "0" && hdnLocID.Value != "")
                //{

                //    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;
                //    GetDataEquip();
                //}
             
                //else
                //    lnkLocationID.NavigateUrl = "";



                if (hdnCustId.Value != "0" && hdnCustId.Value != "")
                {
                    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustId.Value;
                }

             
                else
                    lnkCustomerID.NavigateUrl = "";

                if (hdnCustId.Value != "0" && hdnCustId.Value != "")
                {
                    hdnLocId.Value = hdnOwnerID.Value;
                    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocId.Value;

                }
                else
                    lnkCustomerID.NavigateUrl = "";

            }
        }
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        if (ViewState["edit"].ToString() != "0" && (hdnCustId.Value == "" || hdnCustId.Value == "0"))
        {
            if (chkClosed.Checked == true)
            {
                if (pnlCustomer.Visible == false)
                {
                    pnlCustomer.Visible = true;
                    uc_CustomerSearch1._txtCustomer.Focus();
                    return;
                }
            }
            else
            {
                pnlCustomer.Visible = false;
                uc_CustomerSearch1._hdnCustID.Value = string.Empty;
            }
        }
        else
        {
            pnlCustomer.Visible = false;
            uc_CustomerSearch1._hdnCustID.Value = string.Empty;
        }

        objProp_Customer.CompanyName = txtCompanyName.Text.Trim();
        objProp_Customer.Name = txtOppName.Text.Trim();
        objProp_Customer.ROL = Convert.ToInt32(hdnId.Value);
        objProp_Customer.Probability = Convert.ToInt32(ddlProbab.SelectedValue);
        objProp_Customer.Status = Convert.ToInt32(ddlStatus.SelectedValue);
        objProp_Customer.Remarks = txtRemarks.Text.Trim();
        if (ddlStatus.SelectedItem.Text == "Withdrawn" || ddlStatus.SelectedItem.Text == "Disqualified" || ddlStatus.SelectedItem.Text == "Sold")
        {
            if (txtCloseDate.Text != "")
            {
                objProp_Customer.EndDate = txtCloseDate.Text.Trim();
            }
            else
            {
                objProp_Customer.EndDate = null;
            }

        }
        else
        {
            objProp_Customer.EndDate = null;
        }

        objProp_Customer.ConnConfig = Session["config"].ToString();
        if (uc_CustomerSearch1._hdnCustID.Value.Trim() != string.Empty)
            objProp_Customer.ProspectID = Convert.ToInt32(uc_CustomerSearch1._hdnCustID.Value);//0;//Convert.ToInt32(hdnOwnerID.Value);
        else
            objProp_Customer.ProspectID = 0;
        objProp_Customer.NextStep = string.Empty;
        objProp_Customer.Fuser = ddlAssigned.SelectedItem.Text;
        objProp_Customer.AssignedToID = ddlAssigned.SelectedValue;
        objProp_Customer.LastUpdateUser = Session["username"].ToString();
        objProp_Customer.Close = Convert.ToInt16(chkClosed.Checked);
        objProp_Customer.ticketID = 0;

        objProp_Customer._BT = ddlBusinessType.SelectedItem.Value;
        objProp_Customer._Service = ddlProduct.SelectedItem.Value;
        objProp_Customer.OpportunityStageID = ddlStage.SelectedValue;

        string strDesc = string.Empty;
        if (txtNextStep.Text.Trim() != string.Empty)
        {
            strDesc = System.DateTime.Now.ToString() + "      " + Session["username"].ToString() + Environment.NewLine + "Next Step : " + txtNextStep.Text.Trim() + Environment.NewLine + txtDesc.Text.Trim();
        }
        else
        {
            strDesc = txtDesc.Text.Trim();
        }
        objProp_Customer.Description = strDesc;
        objProp_Customer.Source = ddlSource.SelectedValue;
        if (txtAmount.Text.Trim() != string.Empty)
            objProp_Customer.Amount = double.Parse(txtAmount.Text.Trim(), NumberStyles.Currency);

        int OpprtID = 0;
        try
        {

            string strMsg = "Added";
            if (ViewState["edit"].ToString() != "1")
            {
                if (ViewState["edit"].ToString() == "2") strMsg = "Copied";
                objProp_Customer.OpportunityID = 0;
                objProp_Customer.Mode = 0;
                int? department = null;
                if (!string.IsNullOrEmpty(ddlDepartment.SelectedValue)) department = Convert.ToInt32(ddlDepartment.SelectedValue);
                OpprtID = objBL_Customer.AddEditOpportunity(objProp_Customer, department);
                if ((hdnCustId.Value == "" || hdnCustId.Value == "0") && chkClosed.Checked == true)
                {
                    Session["strMsg"] = strMsg;
                    ConvertProspectWizard(OpprtID);
                }
                else
                {
                    
                    Session["strMsg"] = strMsg;

                    SendMailToSalesPer(OpprtID, txtOppName.Text, txtName.Text);

                    if (!string.IsNullOrEmpty(Request.QueryString["redirect"]))
                        Response.Redirect("addopprt.aspx?uid=" + OpprtID + "&redirect=" + HttpUtility.UrlEncode(Request.QueryString["redirect"]));
                    else
                        Response.Redirect("addopprt.aspx?uid=" + OpprtID);
                }

                pnlCustomer.Visible = false;
                uc_CustomerSearch1._hdnCustID.Value = string.Empty;
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                objProp_Customer.OpportunityID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objProp_Customer.Mode = 1;
                int? department = null;
                if (!string.IsNullOrEmpty(ddlDepartment.SelectedValue)) department = Convert.ToInt32(ddlDepartment.SelectedValue);
                OpprtID = objBL_Customer.AddEditOpportunity(objProp_Customer, department);
                if (hdnCustId.Value == "" || hdnCustId.Value == "0")
                {
                    Session["strMsg"] = strMsg;
                    ConvertProspectWizard(OpprtID);
                }
                else
                {
                    strMsg = "Updated";
                    Session["strMsg"] = strMsg;                     

                    Response.Redirect(Page.Request.RawUrl, false);
                }

                GetOpportunity();
                
                pnlCustomer.Visible = false;

                RadGrid_gvLogs.Rebind();
                string strScript = string.Empty;
                strScript += "noty({text: 'Opportunity " + strMsg + " Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);
                Response.Redirect(Page.Request.RawUrl, false);
            }
         

            //FillRecentProspect();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_lnkSearch')) window.opener.document.getElementById('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_lnkSearch').click();}", true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    private void SendMailToSalesPer(int OpprtID, string OppName,string LocName)
    {
   

        if (Request.QueryString["uid"] == null)
        {
              
            string To = "";  

            To = objBL_MapData.GetSalesPerInfo2(OpprtID.ToString(), Session["config"].ToString());
             
            {
                

                if (To.Trim() != string.Empty && To.Contains('@'))
                {
                     
                    string OppDate = DateTime.Now.Date.ToString("MM/dd/yyyy");

                    string Workername = Session["username"].ToString();

                    string message = "You have an opportunity waiting. Opp #" + OppName + " -" + LocName + "-" + OppDate + ".<BR/>";


                    try
                    {
                        mail(message, To, LocName, OpprtID.ToString());  

                        objBL_MapData.ResetisSendmailtosalesper2(OpprtID ,Session["config"].ToString());
                    }
                    catch { }
                }

            }
        }
    }

    private void mail(string message, string To, string Locname, string OppID)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();



        string from = WebBaseUtility.GetFromEmailAddress();

        if (To.Trim() != string.Empty && from.Trim() != string.Empty)
        {
            try
            {
                Mail mail = new Mail();
                mail.From = from;
                mail.To = To.Split(';', ',').OfType<string>().ToList();
                mail.Bcc.Add(from);
                mail.Title = "New opportunity - Opp#" + OppID + " -" + Locname;
                mail.Text = message;
                mail.RequireAutentication = true;
                mail.Send();
            }
            catch (Exception ex)
            {

            }
        }

    }


    private void ConvertProspectWizard(int OpprtID)
    {
        string ProspectID = hdnOwnerID.Value.Trim();
        if ((hdnCustId.Value == "" || hdnCustId.Value == "0") && chkClosed.Checked == true && uc_CustomerSearch1._hdnCustID.Value == string.Empty)
        //if (lblType.Text.Trim() == "Lead" && chkClosed.Checked == true && uc_CustomerSearch1._hdnCustID.Value == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Opportunity Saved Successfully. Continue to Convert Lead Wizard.'); window.location.href='addcustomer.aspx?cpw=1&prospectid=" + ProspectID + "&opid=" + OpprtID + "';", true);
        }
        //else if (lblType.Text.Trim() == "Lead" && chkClosed.Checked == true && uc_CustomerSearch1._hdnCustID.Value != string.Empty)
        else if ((hdnCustId.Value == "" || hdnCustId.Value == "0") && chkClosed.Checked == true && uc_CustomerSearch1._hdnCustID.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Opportunity Saved Successfully. Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + uc_CustomerSearch1._hdnCustID.Value + "&opid=" + OpprtID + "';", true);
        }
    }

 
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            var previousPage = Request.QueryString["page"].ToString();
            previousPage = System.Net.WebUtility.UrlDecode(previousPage);
            Response.Redirect(previousPage);
        }


        if (!string.IsNullOrEmpty(Request.QueryString["redirect"]))
        {
            Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
        }
        else
        {
           
            Response.Redirect("opportunity.aspx?fil=1");
        }
    }
    protected void btnFillTasks_Click(object sender, EventArgs e)
    {
        int n;
        bool isNumeric = int.TryParse(hdnOwnerID.Value, out n);

        FillTasks(hdnId.Value);
        FillContact(Convert.ToInt32(hdnId.Value));
        RadGrid_Contacts.Rebind();
        if (isNumeric)
        {
            GetDocuments(Convert.ToInt32(hdnOwnerID.Value));
            RadGrid_Documents.Rebind();
            FillLocInfo(Convert.ToInt32(hdnOwnerID.Value));
        }
        else
        {
            FillProInfo(Convert.ToInt32(hdnProsID.Value));
        }
        HyperLink1.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
        HyperLink2.NavigateUrl = "AddTask.aspx?rol=" + hdnId.Value + "&name=" + txtName.Text;
        lnkNewEmail.NavigateUrl = "email.aspx?rol=" + hdnId.Value;

        ViewState["rolapp"] = hdnId.Value;


        //New 26-1-22
        DataSet ds1 = new DataSet();
     
        string rol = hdnId.Value;
        objProp_Customer.ROL = Convert.ToInt32(rol);

        ds1 = objBL_Customer.GetRolLocID(objProp_Customer);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            hdnCustId.Value = ds1.Tables[0].Rows[0]["Owner"].ToString();
            hdnLocId.Value = ds1.Tables[0].Rows[0]["Loc"].ToString();
        }
     
     
        if (hdnCustId.Value != "0" && hdnCustId.Value!="")
        {
            lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustId.Value;
        }
            
        else if (hdnCustId.Value == "0"|| hdnCustId.Value=="")
        {
            //hdnCustId.Value = ds.Tables[0].Rows[0]["ownerid"].ToString();
            lnkCustomerID.NavigateUrl = "addprospect.aspx?uid=" + hdnProsID.Value;
           // lnkLocationID.NavigateUrl = "addprospect.aspx?uid=" + hdnCustId.Value;
        }

        else
            lnkCustomerID.NavigateUrl = "";
        if (hdnLocId.Value != "0" && hdnLocId.Value != "")
        {

            lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocId.Value;
            //    //GetDataEquip();
        }

        else if (hdnLocId.Value == "0" || hdnLocId.Value=="")
        {
            //hdnCustId.Value = ds.Tables[0].Rows[0]["ownerid"].ToString();
           // lnkCustomerID.NavigateUrl = "addprospect.aspx?uid=" + hdnProsID.Value;
            lnkLocationID.NavigateUrl = "addprospect.aspx?uid=" + hdnProsID.Value;
        }

        else
            lnkLocationID.NavigateUrl = "";


        //NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
        //masterSalesMaster.FillPendingRec(Convert.ToInt32(ViewState["rolapp"]));
    }
    private void FillRecentProspect()
    {
        //NewSalesMaster masterSalesMaster = (NewSalesMaster)Page.Master;
        //masterSalesMaster.FillRecentProspect();
        //masterSalesMaster.FillPendingRec(Convert.ToInt32(ViewState["rolapp"]));
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
        objProp_Customer.Screen = "Opportunity";
        objProp_Customer.Ref = !string.IsNullOrEmpty(hdnUID.Value) ? Convert.ToInt32(hdnUID.Value) : 0;
        ds = objBL_Customer.getTasks(objProp_Customer);
        RadGrid_OpenTasks.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_OpenTasks.DataSource = ds.Tables[0];
        RadGrid_OpenTasks.DataBind();
        //gvTasks.DataSource = ds.Tables[0];
        //gvTasks.DataBind();


        //menuLeads.Items[1].Text = "Open Tasks(" + ds.Tables[0].Rows.Count + ")";

        objProp_Customer.Mode = 0;
        objProp_Customer.Screen = "Opportunity";
        objProp_Customer.Ref = !string.IsNullOrEmpty(hdnUID.Value) ? Convert.ToInt32(hdnUID.Value) : 0;
        ds = objBL_Customer.getTasks(objProp_Customer);
        RadGrid_TaskHistory.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_TaskHistory.DataSource = ds.Tables[0];
        RadGrid_TaskHistory.DataBind();

        //gvTasksCompleted.DataSource = ds.Tables[0];
        //gvTasksCompleted.DataBind();




        //menuLeads.Items[2].Text = "Task History(" + ds.Tables[0].Rows.Count + ")";

        if (ViewState["edit"].ToString() == "0")
        {
            //BindEmails(GetMailsfromdb(-1, string.Empty));
            hdnMailType.Value = "-1";
        }
        else
        {
            //BindEmails(GetMailsfromdb(-2, string.Empty));
            hdnMailType.Value = "-2";
        }
    }

    private void FillContact(int rol)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ROL = rol;
        DataSet ds = new DataSet();
  
        ds = objBL_Customer.getContactByRolID(objProp_Customer);

       
        //gvContacts.DataSource = ds.Tables[0];
        //gvContacts.DataBind();

      

        RadGrid_Contacts.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Contacts.DataSource = ds.Tables[0];


        Session["OppContactData"] = ds.Tables[0];
        pnlContactButtons.Visible = true;
     
        //menuLeads.Items[3].Text = "Contacts(" + ds.Tables[0].Rows.Count + ")";
    }

    private void FillUsers()
    {
        int OpportunityID = Request.QueryString["uid"] != null ? Convert.ToInt32(Request.QueryString["uid"].ToString()) : 0;
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        //ds = objBL_User.getTerritory(objPropUser, new GeneralFunctions().GetSalesAsigned(), 0, OpportunityID, "t.SDesc");

        ds = objBL_User.GetSalesPerson(objPropUser, new GeneralFunctions().GetSalesAsigned(), OpportunityID, "OPPORTUNITY", "t.SDesc");

        ddlAssigned.DataSource = ds.Tables[0];
        ddlAssigned.DataTextField = "SDesc";
        ddlAssigned.DataValueField = "id";
        ddlAssigned.DataBind();

        //ddlAssigned.Items.Insert(0, new ListItem("Select", ""));
        ddlAssigned.Items.Insert(0, new ListItem("None", ""));
    }
    private void GetDocuments(int Leadid)
    {
        if (lblType.Text == "Lead")
            objMapData.Screen = "SalesLead";
        else
            objMapData.Screen = "Location";

        objMapData.TempId = "0";
        objMapData.TicketID = Leadid;
        ViewState["OwnerID"] = Leadid;
        objMapData.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_MapData.GetDocuments(objMapData);
        RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Documents.DataSource = ds.Tables[0];

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
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Split(',');

        string FileName = CommandArgument[0];

        string FilePath = CommandArgument[1];

        DownloadDocument(FilePath, FileName);
    }

    
    protected void lnkRefreshMails_Click(object sender, EventArgs e)
    {
         

        if (hdnMailType.Value == "-1")
        {
            //BindEmails(GetMailsfromdb(-1, string.Empty));
            BindMail(-1, string.Empty);
            RadGrid_Mail.Rebind();
        }
        else if (hdnMailType.Value == "-2")
        {
            //BindEmails(GetMailsfromdb(-2, string.Empty));
            BindMail(-1, string.Empty);
            RadGrid_Mail.Rebind();
        }

    }
    protected void lnkAllMail_Click(object sender, EventArgs e)
    {
        //BindEmails(GetMailsfromdb(-1, string.Empty));
        hdnMailType.Value = "-1";
    }
    protected void lnkSpecific_Click(object sender, EventArgs e)
    {
        //BindEmails(GetMailsfromdb(-2, string.Empty));
        hdnMailType.Value = "-2";
    }

   
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        
    }
   
    protected void chkClosed_CheckedChanged(object sender, EventArgs e)
    {
        pnlCustomer.Visible = chkClosed.Checked;
    }

    private void FillLocInfo(int intOwnerID)
    {
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = intOwnerID;
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet dc = new DataSet();
        dc = objBL_User.getCustomerByID(objPropUser);

        if (dc.Tables[0].Rows.Count > 0)
        {
            txtCompany.Text = dc.Tables[0].Rows[0]["Company"].ToString();
        }

    }

    private void FillProInfo(int LeadID)
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ProspectID = Convert.ToInt32(LeadID);
        DataSet ds = new DataSet();
        ds = objBL_Customer.getProspectByID(objProp_Customer);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
        }

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

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToString(ViewState["EditContactID"]) != "")
        {
            PhoneModel _objData = new PhoneModel();
            _objData.ID = Convert.ToInt32(ViewState["EditContactID"]);
            _objData.Cell = txtContCell.Text;
            _objData.ConnConfig = Session["config"].ToString();
            _objData.Email = txtContEmail.Text;
            _objData.Fax = txtContFax.Text;
            _objData.Name = txtContcName.Text;
            _objData.Phone = txtContPhone.Text;
            _objData.Rol = Convert.ToInt32(hdnId.Value);
            _objData.Title = txtTitle.Text;
            objBL_Customer.UpdateContactByID(_objData);
        }
        else
        {
            PhoneModel _objData = new PhoneModel();
            _objData.Cell = txtContCell.Text;
            _objData.ConnConfig = Session["config"].ToString();
            _objData.Email = txtContEmail.Text;
            _objData.Fax = txtContFax.Text;
            _objData.Name = txtContcName.Text;
            _objData.Phone = txtContPhone.Text;
            _objData.Rol = Convert.ToInt32(hdnId.Value);
            _objData.Title = txtTitle.Text;
            objBL_Customer.AddContactByRol(_objData);
        }

    

        FillContact(Convert.ToInt32(hdnId.Value));
        RadGrid_Contacts.Rebind();

        string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").hide(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        //foreach (GridViewRow di in gvContacts.Rows)
        //{

        //    CheckBox chkSelectCon = (CheckBox)di.FindControl("chkSelectCon");
        //    if (chkSelectCon.Checked == true)
        //    {
        //        DataTable dt = (DataTable)Session["OppContactData"];
        //        Label lblID = (Label)di.FindControl("lblId0");

        //        ModalPopup.Show();
        //        pnlContact.Visible = true;

        //        DataRow dr = dt.Select("ContactID = " + lblID.Text).First();
        //        txtContcName.Text = dr["Name"].ToString();
        //        txtTitle.Text = dr["Title"].ToString();
        //        txtContPhone.Text = dr["Phone"].ToString();
        //        txtContFax.Text = dr["Fax"].ToString();
        //        txtContCell.Text = dr["Cell"].ToString();
        //        txtContEmail.Text = dr["Email"].ToString();
        //        ViewState["EditContactID"] = dr["ContactID"].ToString();
        //    }
        //}

        RadWindowContact.Title = "Edit Contact";
        foreach (GridDataItem item in RadGrid_Contacts.SelectedItems)
        {
            DataTable dt = (DataTable)Session["OppContactData"];
            Label lblID = (Label)item.FindControl("lblId0");

            DataRow dr = dt.Select("ContactID = " + lblID.Text).First();
            txtContcName.Text = dr["Name"].ToString();
            txtTitle.Text = dr["Title"].ToString();
            txtContPhone.Text = dr["Phone"].ToString();
            txtContFax.Text = dr["Fax"].ToString();
            txtContCell.Text = dr["Cell"].ToString();
            txtContEmail.Text = dr["Email"].ToString();
            ViewState["EditContactID"] = dr["ContactID"].ToString();

            //New
            //if (hdnLocID.Value != "0")
            //{

            //    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocID.Value;
            //    //GetDataEquip();
            //}
            //else
            //    lnkLocationID.NavigateUrl = "";

            //if (hdnCustId.Value != "0")
            //    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnCustId.Value;
            //else
            //    lnkCustomerID.NavigateUrl = "";
        }

        string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        //foreach (GridViewRow di in gvContacts.Rows)
        //{

        //    CheckBox chkSelectCon = (CheckBox)di.FindControl("chkSelectCon");
        //    if (chkSelectCon.Checked == true)
        //    {
        //        Label lblID = (Label)di.FindControl("lblId0");
        //        objProp_Customer.ConnConfig = Session["config"].ToString();
        //        objProp_Customer.contact = lblID.Text;
        //        objBL_Customer.DeletePhone(objProp_Customer);
        //    }
        //}

        foreach (GridDataItem item in RadGrid_Contacts.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId0");
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.contact = lblID.Text;
            objBL_Customer.DeletePhone(objProp_Customer);
        }

        FillContact(Convert.ToInt32(hdnId.Value));
        RadGrid_Contacts.Rebind();
    }

    protected void RadGrid_Contacts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Contacts.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        if (hdnId.Value != "")
        {
            FillContact(Convert.ToInt32(hdnId.Value));
            

        }
    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Contacts.MasterTableView.FilterExpression != "" ||
            (RadGrid_Contacts.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Contacts.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_OpenTasks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

    }

    protected void RadGrid_TaskHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

    }

    protected void RadGrid_Mail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Mail.AllowCustomPaging = !ShouldApplySortFilterOrGroupMail();
        BindMail(-1, "");
    }
    bool isGroupingMail = false;
    public bool ShouldApplySortFilterOrGroupMail()
    {
        return RadGrid_Mail.MasterTableView.FilterExpression != "" ||
            (RadGrid_Mail.MasterTableView.GroupByExpressions.Count > 0 || isGroupingMail) ||
            RadGrid_Mail.MasterTableView.SortExpressions.Count > 0;
    }
    private void BindMail(Int32 type, String OrderBy)
    {
        DataSet ds = null;
        lnkAllMail.BackColor = System.Drawing.Color.DarkGray;
        lnkSpecific.BackColor = System.Drawing.Color.Transparent;
        if (hdnId.Value.Trim() != string.Empty)
        {
            objGeneral.OrderBy = OrderBy;
            objGeneral.ConnConfig = Session["config"].ToString();
            objGeneral.type = type;
            objGeneral.rol = Convert.ToInt32(hdnId.Value);
            objGeneral.userid = Convert.ToInt32(Session["userid"].ToString());
            if (type == -2)
            {
                objGeneral.RegID = "[OP-" + Request.QueryString["uid"].ToString() + "]";
                objGeneral.rol = 0;
                lnkAllMail.BackColor = System.Drawing.Color.Transparent;
                lnkSpecific.BackColor = System.Drawing.Color.DarkGray;
            }

            ds = objBL_General.GetMails(objGeneral);

            RadGrid_Mail.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Mail.DataSource = ds.Tables[0];
        }
    }

    protected void RadGrid_Documents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Documents.AllowCustomPaging = !ShouldApplySortFilterOrGroupDocuments();
        if (Request.QueryString["rol"] != null)
        {
            GetDocuments(Convert.ToInt32(Request.QueryString["owner"].ToString()));
        }
    }
    bool isGroupingDocuments = false;
    public bool ShouldApplySortFilterOrGroupDocuments()
    {
        return RadGrid_Documents.MasterTableView.FilterExpression != "" ||
            (RadGrid_Documents.MasterTableView.GroupByExpressions.Count > 0 || isGroupingDocuments) ||
            RadGrid_Documents.MasterTableView.SortExpressions.Count > 0;
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["opps"];
            string url = "addopprt.aspx?uid=" + dt.Rows[0]["ID"];
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
            dt = (DataTable)Session["opps"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addopprt.aspx?uid=" + dt.Rows[index - 1]["ID"];
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
            dt = (DataTable)Session["opps"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addopprt.aspx?uid=" + dt.Rows[index + 1]["ID"];
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
            dt = (DataTable)Session["opps"];
            string url = "addopprt.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
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
                filename = filename.Replace(",", "");
                fullpath = savepath + filename;
                MIME = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

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

                FileUpload1.SaveAs(fullpath);
            }

            if (lblType.Text == "Lead")
                objMapData.Screen = "SalesLead";
            else
                objMapData.Screen = "Location";

            objMapData.TicketID = Convert.ToInt32(ViewState["OwnerID"]);
            objMapData.TempId = "0";
            objMapData.FileName = filename;
            objMapData.DocTypeMIME = MIME;
            objMapData.FilePath = fullpath;

            objMapData.DocID = 0;
            objMapData.Mode = 0;
            objMapData.ConnConfig = Session["config"].ToString();
            objBL_MapData.AddFile(objMapData);

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.dtDocs = SaveDocInfo();
            objBL_User.UpdateDocInfo(objPropUser);


            if (lblType.Text == "Lead")
                objMapData.Screen = "SalesLead";
            else
                objMapData.Screen = "Location";

            objMapData.TempId = "0";
            objMapData.TicketID = Convert.ToInt32(ViewState["OwnerID"]); ;
            objMapData.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_MapData.GetDocuments(objMapData);
            RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Documents.DataSource = ds.Tables[0];
            RadGrid_Documents.DataBind();

            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadSuccess", "noty({text: 'File uploaded successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string extension = Path.GetExtension(FileUpload1.FileName);
            if (extension == "")
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadextension", "noty({text: 'Invalid File!',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }
    private DataTable SaveDocInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MSVisible", typeof(int));


        foreach (GridDataItem item in RadGrid_Documents.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblID");
            //  CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
            // TextBox txtRemarks = (TextBox)item.FindControl("txtRemarks");

            DataRow dr = dt.NewRow();
            dr["ID"] = lblID.Text;
            // dr["Portal"] = chkPortal.Checked;
            // dr["Remarks"] = txtRemarks.Text;
            dr["MSVisible"] = false;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Documents.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblID");
            DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr1", "noty({text: 'File deleted successfully!',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout: 5000, theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    public void DeleteFileFromFolder(string StrFilename, int DocumentID)
    {
        try
        {
            DeleteFile(DocumentID);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

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

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.dtDocs = SaveDocInfo();
            objBL_User.UpdateDocInfo(objPropUser);

            int n;
            bool isNumeric = int.TryParse(hdnOwnerID.Value, out n);
            if (isNumeric)
            {
                GetDocuments(Convert.ToInt32(hdnOwnerID.Value));
                RadGrid_Documents.Rebind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
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
                objProp_Customer.LogRefId = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objProp_Customer.LogScreen = "Opportunity";
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

    private void FillEstimatesLinked(int oppId)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.OpportunityID = oppId;

        ds = objBL_Customer.GetEstimatesByOpportunityID(objProp_Customer);
        RadGrid_EstimatesLinked.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_EstimatesLinked.DataSource = ds.Tables[0];
        //RadGrid_EstimatesLinked.DataBind();

        if (ds.Tables[0].Rows.Count == 0)
        {
            ddlDepartment.Enabled = true;
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            txtAmount.Text = ds.Tables[1].Rows[0]["Revenue"].ToString();
            ddlDepartment.SelectedValue = ds.Tables[1].Rows[0]["Department"] != null ? ds.Tables[1].Rows[0]["Department"].ToString() : "";
        }
    }

    protected void lnkEditEstimate_Click(object sender, EventArgs e)
    {
        if (RadGrid_EstimatesLinked.Items.Count > 0 && RadGrid_EstimatesLinked.SelectedItems.Count == 1)
        {
            foreach (GridDataItem item in RadGrid_EstimatesLinked.SelectedItems)
            {
                Label lblID = (Label)item.FindControl("lblId");
                //Response.Redirect("addestimate.aspx?uid=" + lblID.Text + "&opp=" + Request.QueryString["uid"].ToString());
                Response.Redirect("addestimate.aspx?uid=" + lblID.Text
                    + "&opp=" + Request.QueryString["uid"].ToString()
                    + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl));
            }
        }
        else if (RadGrid_EstimatesLinked.Items.Count > 0 && RadGrid_EstimatesLinked.SelectedItems.Count > 1)
        {
            // TODO: Show messages: Please select only one estimate for edit
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnEdit", "noty({text: 'Please select only one estimate to edit',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnEdit", "noty({text: 'There is no selected estimate',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkDeleteEstimate_Click(object sender, EventArgs e)
    {
        try
        {
            if (RadGrid_EstimatesLinked.Items.Count > 0 && RadGrid_EstimatesLinked.SelectedItems.Count > 0)
            {
                var errorMessage = new StringBuilder();
                var successMessage = new StringBuilder();
                foreach (GridDataItem di in RadGrid_EstimatesLinked.SelectedItems)
                {
                    try
                    {
                        TableCell cell = di["chkSelect"];
                        CheckBox chkSelect = (CheckBox)cell.Controls[0];
                        Label lblID = (Label)di.FindControl("lblID");

                        if (chkSelect.Checked == true)
                        {
                            objProp_Customer.ConnConfig = Session["config"].ToString();
                            objProp_Customer.dtLaborItems = null;
                            objProp_Customer.dtItems = null;
                            objProp_Customer.Mode = 2;
                            objProp_Customer.estimateno = Convert.ToInt32(lblID.Text);
                            objProp_Customer.Username = Session["Username"].ToString();
                            objBL_Customer.DeleteEstimate(objProp_Customer);

                            successMessage.AppendFormat("Estimate {0} deleted successfully<br/>", objProp_Customer.estimateno);
                            //successMessage.AppendLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                        errorMessage.AppendFormat("{0} <br/>", str);
                        //errorMessage.AppendLine();
                    }

                }

                if (successMessage.Length > 0)
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDel", "noty({text: '" + successMessage.ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                if (errorMessage.Length > 0)
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDel", "noty({text: '" + errorMessage.ToString() + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

                if (!string.IsNullOrEmpty(hdnUID.Value))
                {
                    try
                    {
                        objProp_Customer.ConnConfig = Session["config"].ToString();
                        var opprID = Convert.ToInt32(hdnUID.Value);
                        objProp_Customer.OpportunityID = opprID;
                        var oppDS = objBL_Customer.GetOppStatus(objProp_Customer);

                        if (oppDS.Tables.Count > 0 && oppDS.Tables[0].Rows.Count > 0)
                        {
                            ddlStatus.SelectedValue = oppDS.Tables[0].Rows[0]["Status"].ToString();
                        }

                    }
                    catch (Exception)
                    {
                    }
                }



                RadGrid_EstimatesLinked.Rebind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnDel", "noty({text: 'There is no estimate selected',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDel", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCopyEstimate_Click(object sender, EventArgs e)
    {
        if (RadGrid_EstimatesLinked.Items.Count > 0 && RadGrid_EstimatesLinked.SelectedItems.Count == 1)
        {
            foreach (GridDataItem item in RadGrid_EstimatesLinked.SelectedItems)
            {
                Label lblID = (Label)item.FindControl("lblId");
                Response.Redirect("addestimate.aspx?uid=" + lblID.Text + "&t=c&opp=" + Request.QueryString["uid"].ToString() + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl));
            }
        }
        else if (RadGrid_EstimatesLinked.Items.Count > 0 && RadGrid_EstimatesLinked.SelectedItems.Count > 1)
        {
            // TODO: Show messages: Please select only one estimate for edit
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnCopy", "noty({text: 'Please select only one estimate to for copying',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnCopy", "noty({text: 'There is no estimate selected',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_EstimatesLinked_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_EstimatesLinked.AllowCustomPaging = !ShouldApplySortFilterOrGroupForEstimates();
        if (!string.IsNullOrEmpty(hdnUID.Value))
        {
            try
            {
                var opprID = Convert.ToInt32(hdnUID.Value);
                FillEstimatesLinked(opprID);
            }
            catch (Exception)
            {
            }
        }

    }

    protected void RadGrid_EstimatesLinked_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                //if (Convert.ToString(RadGrid_EstimatesLinked.MasterTableView.FilterExpression) != "")
                //    lblRecordCount.Text = totalCount + " Record(s) found";
                //else
                //    lblRecordCount.Text = RadGrid_EstimatesLinked.VirtualItemCount + " Record(s) found";
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

    protected void RadGrid_EstimatesLinked_PreRender(object sender, EventArgs e)
    {
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_EstimatesLinked);
    }

    public bool ShouldApplySortFilterOrGroupForEstimates()
    {
        return RadGrid_EstimatesLinked.MasterTableView.FilterExpression != "" ||
            RadGrid_EstimatesLinked.MasterTableView.GroupByExpressions.Count > 0 ||
            RadGrid_EstimatesLinked.MasterTableView.SortExpressions.Count > 0;
    }

    private void FillJobType()
    {
        try
        {
            DataSet _dsJob = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            // is recurring job template exist or not
            //if (Request.QueryString["uid"] != null)
            //{
            //    objJob.ID = Convert.ToInt32(Request.QueryString["uid"]);
            //}

            //objJob.IsExistRecurr = objBL_Job.IsExistRecurrJobT(objJob);

            _dsJob = objBL_Job.GetAllJobType(objJob);
            DataTable jobDt = new DataTable();

            //if (objJob.IsExistRecurr.Equals(true))
            //{
            //    jobDt = _dsJob.Tables[0].Select("ID <> 0").CopyToDataTable();
            //}
            //else
            //{
            //    jobDt = _dsJob.Tables[0];
            //}
            jobDt = _dsJob.Tables[0];
            if (_dsJob.Tables[0].Rows.Count > 0)
            {
                ddlDepartment.Items.Clear();
                ddlDepartment.Items.Add(new ListItem("Select Department", ""));
                ddlDepartment.AppendDataBoundItems = true;
                ddlDepartment.DataSource = jobDt;
                ddlDepartment.DataValueField = "ID";
                ddlDepartment.DataTextField = "Type";
                ddlDepartment.DataBind();
            }
            else
            {
                ddlDepartment.Items.Add(new ListItem("No data found", ""));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkExcelEstimate_Click(object sender, EventArgs e)
    {
        RadGrid_EstimatesLinked.ExportSettings.FileName = "Estimate";
        RadGrid_EstimatesLinked.ExportSettings.IgnorePaging = true;
        RadGrid_EstimatesLinked.ExportSettings.ExportOnlyData = true;
        RadGrid_EstimatesLinked.ExportSettings.OpenInNewWindow = true;
        RadGrid_EstimatesLinked.ExportSettings.HideStructureColumns = true;
        RadGrid_EstimatesLinked.MasterTableView.UseAllDataFields = true;
        RadGrid_EstimatesLinked.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_EstimatesLinked.MasterTableView.ExportToExcel();
    }

    protected void lnkProjectEstimate_Click(object sender, EventArgs e)
    {
        string prospectID = "";
        DataSet ds = new DataSet();

        if (RadGrid_EstimatesLinked.Items.Count > 0 && RadGrid_EstimatesLinked.SelectedItems.Count > 0)
        {
            if (RadGrid_EstimatesLinked.SelectedItems.Count > 1)
            {
                if (lblType.Text.Equals("Existing", StringComparison.InvariantCultureIgnoreCase))
                {
                    var contractMessage = new StringBuilder();
                    var errorMessage = new StringBuilder();
                    var lstEstIds = new List<int>();
                    foreach (GridDataItem di in RadGrid_EstimatesLinked.SelectedItems)
                    {
                        try
                        {
                            Label lblID = (Label)di.FindControl("lblId");
                            string lblIDVal = lblID.Text;
                            lstEstIds.Add(Convert.ToInt32(lblIDVal));
                        }
                        catch (Exception)
                        { }
                    }

                    lstEstIds.Sort();

                    foreach (var item in lstEstIds)
                    {
                        try
                        {
                            objProp_Customer.ConnConfig = Session["config"].ToString();
                            objProp_Customer.estimateno = item;
                            ds = objBL_Customer.GetEstimateByID(objProp_Customer);

                            string jobTypeID = ds.Tables[0].Rows[0]["JobTypeID"].ToString();
                            if (!string.IsNullOrEmpty(jobTypeID) && jobTypeID != "0")
                            {
                                ConvertToProject(item.ToString(), true);
                            }
                            else if (jobTypeID == "0")
                            {

                                var projectId = ds.Tables[0].Rows[0]["job"].ToString();
                                if (string.IsNullOrEmpty(projectId) || projectId == "0")
                                {
                                    contractMessage.Append("This estimate will create contract on converting. So please do it individually!");
                                    break;
                                }
                                else
                                {
                                    errorMessage.AppendFormat("Estimate {0} converted error: Estimate already converted to project! <br/>", objProp_Customer.estimateno);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                            errorMessage.AppendFormat("Estimate {0} converted error: {1} <br/>", objProp_Customer.estimateno, str);
                        }
                    }

                    //foreach (GridDataItem di in RadGrid_EstimatesLinked.SelectedItems)
                    //{
                    //    try
                    //    {
                    //        TableCell cell = di["chkSelect"];
                    //        CheckBox chkSelect = (CheckBox)cell.Controls[0];
                    //        Label lblID = (Label)di.FindControl("lblId");
                    //        string lblIDVal = lblID.Text;
                    //        if (chkSelect.Checked == true)
                    //        {

                    //            objProp_Customer.ConnConfig = Session["config"].ToString();
                    //            objProp_Customer.estimateno = Convert.ToInt32(lblIDVal);
                    //            ds = objBL_Customer.GetEstimateByID(objProp_Customer);

                    //            string jobTypeID = ds.Tables[0].Rows[0]["JobTypeID"].ToString();
                    //            if (!string.IsNullOrEmpty(jobTypeID) && jobTypeID != "0")
                    //            {
                    //                ConvertToProject(lblIDVal, true);
                    //            }
                    //            else if (jobTypeID == "0")
                    //            {
                    //                // TODO: Need to convert manually because of convert contract for default department
                    //                //bool isExistProj = ds.Tables[0].Rows[0]["IsExistProj"].ToString() == "1";
                    //                //if (!isExistProj)
                    //                //{
                    //                //    contractMessage.Append("There is no contract linked to the opportunity. To create contract on converting, select only an estimate convert first then select mutiple later");
                    //                //    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Convert Recurring Contract Wizard.'); window.location.href='addreccontract.aspx?eid=" + Request.QueryString["uid"].ToString() + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) + "';", true);
                    //                //    break;
                    //                //}
                    //                //else
                    //                //{
                    //                //    ConvertToProject(lblIDVal, true);
                    //                //}
                    //                var projectId = ds.Tables[0].Rows[0]["job"].ToString();
                    //                if (string.IsNullOrEmpty(projectId) || projectId == "0")
                    //                {
                    //                    contractMessage.Append("This estimate will create contract on converting. So please do it individually!");
                    //                    break;
                    //                }
                    //                else
                    //                {
                    //                    errorMessage.AppendFormat("Estimate {0} converted error: Estimate already converted to project! <br/>", objProp_Customer.estimateno);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                    //        errorMessage.AppendFormat("Estimate {0} converted error: {1} <br/>", objProp_Customer.estimateno, str);
                    //    }
                    //}
                    if (!string.IsNullOrEmpty(hdnUID.Value))
                    {
                        try
                        {
                            objProp_Customer.ConnConfig = Session["config"].ToString();
                            var opprID = Convert.ToInt32(hdnUID.Value);
                            objProp_Customer.OpportunityID = opprID;
                            var oppDS = objBL_Customer.GetOppStatus(objProp_Customer);

                            if (oppDS.Tables.Count > 0 && oppDS.Tables[0].Rows.Count > 0)
                            {
                                ddlStatus.SelectedValue = oppDS.Tables[0].Rows[0]["Status"].ToString();
                            }

                        }
                        catch (Exception)
                        {
                        }
                    }
                    //FillEstimate();
                    RadGrid_EstimatesLinked.Rebind();


                    if (contractMessage.Length > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnContract", "noty({text: '" + contractMessage.ToString() + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }

                    if (errorMessage.Length > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrorConvert", "noty({text: '" + errorMessage.ToString() + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    //Alert to user: "Please convert the Lead to a Customer/Location before awarding any Estimates."
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyConvertWarning", "noty({text: 'Please convert the Lead to a Customer/Location before awarding any Estimates.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Continue to Convert Lead Wizard.'); window.location.href='addprospect.aspx?uid=" + prospectID + "&estimateid=" + lblIDVal + "';", true);
                }
            }
            else
            {
                foreach (GridDataItem di in RadGrid_EstimatesLinked.SelectedItems)
                {
                    TableCell cell = di["chkSelect"];
                    CheckBox chkSelect = (CheckBox)cell.Controls[0];
                    Label lblID = (Label)di.FindControl("lblId");
                    string lblIDVal = lblID.Text;
                    if (chkSelect.Checked == true)
                    {
                        objProp_Customer.ConnConfig = Session["config"].ToString();
                        objProp_Customer.estimateno = Convert.ToInt32(lblIDVal);
                        ds = objBL_Customer.GetEstimateByID(objProp_Customer);
                        string ffor = ds.Tables[0].Rows[0]["ffor"].ToString();

                        if (!ffor.Equals("ACCOUNT"))
                        {
                            objProp_Customer.ConnConfig = Session["config"].ToString();
                            objProp_Customer.estimateno = Convert.ToInt32(lblIDVal);
                            DataSet dsProspect = objBL_Customer.getProspectIDbyEstimateID(objProp_Customer);
                            if (dsProspect.Tables[0].Rows.Count > 0)
                            {
                                prospectID = dsProspect.Tables[0].Rows[0]["ID"].ToString();
                                Session["Oppr_prosID"] = prospectID;
                            }
                            else
                            {
                                Session["Oppr_prosID"] = null;
                            }
                        }

                        if (ffor.Equals("ACCOUNT"))
                        {
                            //ConvertToProject(lblIDVal);
                            string jobTypeID = ds.Tables[0].Rows[0]["JobTypeID"].ToString();
                            if (!string.IsNullOrEmpty(jobTypeID) && jobTypeID != "0")
                            {
                                ConvertToProject(lblIDVal);
                            }
                            else if (jobTypeID == "0")
                            {
                                //bool isExistProj = ds.Tables[0].Rows[0]["IsExistProj"].ToString() == "1";
                                //if (!isExistProj)
                                //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Convert Recurring Contract Wizard.'); window.location.href='addreccontract.aspx?eid=" + lblIDVal + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) + "';", true);
                                //else
                                //    ConvertToProject(lblIDVal);
                                var projectId = ds.Tables[0].Rows[0]["job"].ToString();
                                if (string.IsNullOrEmpty(projectId) || projectId == "0")
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Convert Recurring Contract Wizard.'); window.location.href='addreccontract.aspx?eid=" + lblIDVal + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) + "';", true);
                                else
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyConvertValidation", "noty({text: 'Estimate already converted to project!',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

                            }
                        }
                        else
                        {
                            if (Session["Oppr_prosID"] != null)
                            {
                                prospectID = Session["Oppr_prosID"].ToString();
                                Session["Oppr_prosID"] = null;
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Continue to Convert Lead Wizard.'); window.location.href='addprospect.aspx?uid=" + prospectID + "&estimateid=" + lblIDVal + "&opid=" + Request.QueryString["uid"] + "';", true);
                                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Continue to Convert Lead Wizard.'); window.location.href='addprospect.aspx?uid=" + prospectID + "&estimateid=" + lblIDVal + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) + "';", true);

                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(hdnUID.Value))
                {
                    try
                    {
                        objProp_Customer.ConnConfig = Session["config"].ToString();
                        var opprID = Convert.ToInt32(hdnUID.Value);
                        objProp_Customer.OpportunityID = opprID;
                        var oppDS = objBL_Customer.GetOppStatus(objProp_Customer);

                        if (oppDS.Tables.Count > 0 && oppDS.Tables[0].Rows.Count > 0)
                        {
                            ddlStatus.SelectedValue = oppDS.Tables[0].Rows[0]["Status"].ToString();
                        }

                    }
                    catch (Exception)
                    {
                    }
                }
                //FillEstimate();
                RadGrid_EstimatesLinked.Rebind();
            }
        }
    }

    private void ConvertToProject(string lblID, bool isMultiple = false)
    {
        try
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.TemplateID = Convert.ToInt32(lblID);
            objProp_Customer.estimateno = Convert.ToInt32(lblID);
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
                    //if (Convert.ToString(dr["BType"]) == "2")
                    //{
                    //    LabPrice = LabPrice + Convert.ToDecimal(dr["LabPrice"] == DBNull.Value ? "0" : dr["LabPrice"]);
                    //}
                    //else if (Convert.ToString(dr["BType"]) == "1")
                    //{
                    //    MatPrice = MatPrice + Convert.ToDecimal(dr["MatPrice"] == DBNull.Value ? "0" : dr["MatPrice"]);
                    //}
                    //else
                    //{
                    //    OtherPrice = OtherPrice + Convert.ToDecimal(dr["MatPrice"] == DBNull.Value ? "0" : dr["MatPrice"]);
                    //}

                    //Hours = Hours + Convert.ToDecimal(dr["LabHours"] == DBNull.Value ? "0" : dr["LabHours"]);

                    decimal LabPriceNK = 0; decimal LabHoursNK = 0;
                    if (Convert.ToString(dr["BType"]) == "1" || Convert.ToString(dr["BType"]) == "8")// Materials or Inventory
                    {
                        MatPrice = MatPrice + Convert.ToDecimal(dr["BudgetExt"] == DBNull.Value ? "0" : dr["BudgetExt"]);
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
                if (isMultiple)
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj" + lblID, "noty({text: 'Estimate " + lblID + " converted to project successfully. <BR/>Project# " + retVal + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                else
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj" + lblID, "noty({text: 'Estimate Converted to Project Successfully! <BR/>Project# " + retVal + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                if (isMultiple)
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnProj" + lblID, "noty({text: 'Estimate " + lblID + " converted failed. " + retVal + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                else
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnProj" + lblID, "noty({text: '" + retVal + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            if (isMultiple)
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj" + lblID, "noty({text: 'Estimate " + lblID + " converted error: " + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            else
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj" + lblID, "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkExportEstimateProfile_Click(object sender, EventArgs e)
    {
        //bool IsChecked = false;
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);

        if (RadGrid_EstimatesLinked.Items.Count > 0 && RadGrid_EstimatesLinked.SelectedItems.Count == 1)
        {
            foreach (GridDataItem di in RadGrid_EstimatesLinked.SelectedItems)
            {
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblID = (Label)di.FindControl("lblId");

                if (chkSelect.Checked == true)
                {
                    var url = "EstimateProfile.aspx?eid=" + lblID.Text + "&redirect=" + redirect;
                    Response.Redirect(url);

                    //Response.Redirect("EstimateProfile.aspx?eid=" + lblID.Text);
                    //IsChecked = true;
                }
            }
        }
        else if (RadGrid_EstimatesLinked.Items.Count > 0 && RadGrid_EstimatesLinked.SelectedItems.Count > 1)
        {
            //if (IsChecked == false)
            //{
            string strEstimateIDs = string.Empty;
            foreach (GridDataItem gr in RadGrid_EstimatesLinked.SelectedItems)
            {
                Label lblID = (Label)gr.FindControl("lblId");
                if (lblID != null)
                {
                    strEstimateIDs += lblID.Text + ",";
                }
            }
            if (strEstimateIDs.Length > 0)
            {
                strEstimateIDs = strEstimateIDs.TrimEnd(',');
            }
            Response.Redirect("EstimateProfile.aspx?eids=" + strEstimateIDs + "&redirect=" + redirect);
            //}
        }
        else
        {
            // TODO: Please select asleat an estimate to export
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnExportPdf", "noty({text: 'There is no estimate selected',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_EstimatesLinked_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 3;
        else
            currentItem = 4;
        if (e.Worksheet.Table.Rows.Count == RadGrid_EstimatesLinked.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_EstimatesLinked.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

   
}
