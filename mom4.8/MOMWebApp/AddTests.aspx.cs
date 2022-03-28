using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Script.Serialization;
using BusinessEntity.Recurring;
using System.Globalization;
using Telerik.Web.UI;
using System.IO;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using Novacode;
using Table = Novacode.Table;
using System.Drawing;
using System.Threading;
using BusinessLayer.Utility;

public partial class AddTests : System.Web.UI.Page
{
    #region Properties

    User objPropUser = new User();

    BL_User objBL_User = new BL_User();

    BL_Customer objBL_Customer = new BL_Customer();

    Customer objPropCustomer = new Customer();

    private const string ASCENDING = " ASC";

    private const string DESCENDING = " DESC";
    General objGeneral = new General();
    BL_General objBL_General = new BL_General();
    #endregion

    //ES-417
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();
    bool isGrouping = false;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        PagePermission();
        if (Request.QueryString["LID"] == null)
        {
            divSuccess.Visible = false;
            Page.Title = "Add Test || MOM";
            lnkFirst.Visible = false;
            lnkPrevious.Visible = false;
            lnkNext.Visible = false;
            lnkLast.Visible = false;
            lblUnitTestName.Visible = false;
            lnkAssign.Visible = false;

            lnkGeneralProposal.Visible = false;

            lnkAddnew.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            lnkMail.Enabled = false;
            divContact.Style["display"] = "none";
            liContacts.Style["display"] = "none";
            liDocument.Style["display"] = "none";
            divDocument.Style["display"] = "none";
            liLogs.Style["display"] = "none";
            divLogs.Style["display"] = "none";
            divPriceHistory.Style["display"] = "none";
            liPriceHistory.Style["display"] = "none";
            liSchedule.Style["display"] = "none";
            divSchedule.Style["display"] = "none";

        }
        else
        {

            // Add New test Massage 
            if (!Page.IsPostBack & Request.QueryString["addnew"] != null)
            {
                if (Request.QueryString["addnew"] == "1")
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "addnewtestkeySucc", "noty({text: 'Test added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});  ", true);

            }
            Page.Title = "Edit Test || MOM";
            lblHeader.Text = "Edit Test";
            divContact.Style["display"] = "";
            liContacts.Style["display"] = "";
            liDocument.Style["display"] = "";
            divDocument.Style["display"] = "";
            divPriceHistory.Style["display"] = "";
            liPriceHistory.Style["display"] = "";
            lnkGeneralProposal.Visible = true;
            liSchedule.Style["display"] = "";
            divSchedule.Style["display"] = "";
            divSuccess.Visible = false;
        }
        //hdnCon.Value = Session["config"].ToString();

        if (!IsPostBack)
        {
            FillEquipClassification();
            BindDropdownlsts();
            if (Request.QueryString["elv"] != null)
            {

                if (Request.QueryString["LID"] == null)
                {
                    ViewState["mode"] = 0;
                    //lblHeader.Text = "Add New Test";
                    BindControls();
                }
                else
                {
                    ViewState["mode"] = 1;
                    //---ref MMO-106  Jira Card
                    if (Session["safetytest"] == null)
                    {
                        lnkFirst.Enabled = false;
                        lnkPrevious.Enabled = false;
                        lnkNext.Enabled = false;
                        lnkLast.Enabled = false;
                    }
                    else
                    {
                        if (isFirstItem())
                        {
                            lnkFirst.Enabled = false;
                            lnkPrevious.Enabled = false;
                        }
                        else if (isLastItem())
                        {
                            lnkNext.Enabled = false;
                            lnkLast.Enabled = false;
                        }
                    }
                    //lblHeader.Text = "Edit Test";
                    BindTest(Convert.ToInt32(Request.QueryString["elv"]), Convert.ToInt32(Request.QueryString["LID"]));

                    BindContact(Convert.ToInt32(Request.QueryString["elv"]), Convert.ToInt32(Request.QueryString["LID"]));
                    BindPriceHistory();
                }
            }
            else
            {
                txtTestNextDueDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                txtLastDueDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                drpdwnTestDueBy.SelectedValue = "1";
            }

            ViewState["editcon"] = 0;
            ////Custom
            CreateTestCustomTable();
            BindTestCustomGrid();
        }
        DataSet dscstm = new DataSet();
        Boolean displayCustom = false;
        dscstm = GetCustomFields("LoadTest1");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCusField1.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
            groupCusField1.Visible = lblCusField1.Text == "Custom 1" ? false : true;

        }
        else
        {
            groupCusField1.Visible = false;
        }
        dscstm = GetCustomFields("LoadTest2");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCusField2.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
            groupCusField2.Visible = lblCusField2.Text == "Custom 2" ? false : true;
        }
        else
        {
            groupCusField2.Visible = false;
        }
        dscstm = GetCustomFields("LoadTest3");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCusField3.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
            groupCusField3.Visible = lblCusField3.Text == "Custom 3" ? false : true;
        }
        else
        {
            groupCusField3.Visible = false;
        }
        dscstm = GetCustomFields("LoadTest4");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCusField4.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
            groupCusField4.Visible = lblCusField4.Text == "Custom 4" ? false : true;
        }
        else
        {
            groupCusField4.Visible = false;
        }

        if (groupCusField2.Visible && !groupCusField1.Visible)
        {
            groupCusField2Blank.Visible = true;
        }
        if (groupCusField3.Visible)
        {
            if ((groupCusField1.Visible && !groupCusField2.Visible) || (!groupCusField1.Visible && groupCusField2.Visible))
            {
                groupCusField3Blank.Visible = false;
            }
        }

        if (groupCusField1.Visible || groupCusField2.Visible || groupCusField3.Visible || groupCusField4.Visible)
        {
            displayCustom = true;
        }
        divCustomTitle.Visible = displayCustom;
        CompanyPermission();
        HighlightSideMenu("cntractsMgr", "lnk", "recurMgrSub");

        if (Request.QueryString["LID"] != null)
        {
            IsCreditHold(Convert.ToInt32(Request.QueryString["LID"]));
        }

        if (ddlStatus.SelectedValue == "1" && lnkTicket.InnerText.Length > 0)
        {
            lnkAssign.Visible = false;
        }
    }

    #region :: Methods ::

    


    private void IsCreditHold(int LID)
    {

        int _IsCreditHold =  Convert.ToInt16(objBL_General.GetLocCredit(LID, Session["config"].ToString())); 

        if (_IsCreditHold == 1)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyIsCreditHold", "noty({text: 'Location on credit hold!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
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

    private void BindDropdownlsts()
    {
        TestTypes objPropTestType = new TestTypes();
        BL_SafetyTest objTestTypes = new BL_SafetyTest();

        objPropTestType.ConnConfig = WebBaseUtility.ConnectionString;
        DataSet dsttypes = objTestTypes.GetAllTestTypes(objPropTestType);
        ddlTestTypes.Items.Add(new ListItem("Select a test", "0"));
        if (dsttypes.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsttypes.Tables[0].Rows.Count; i++)
            {
                if (dsttypes.Tables[0].Rows[i]["Status"].ToString() == "0")
                {
                    var item = new ListItem(dsttypes.Tables[0].Rows[i]["Name"].ToString(), dsttypes.Tables[0].Rows[i]["ID"].ToString());
                    item.Attributes.Add("data-frequency", dsttypes.Tables[0].Rows[i]["Frequency"].ToString());
                    item.Attributes.Add("data-DueBy", dsttypes.Tables[0].Rows[i]["NextDateCalcMode"].ToString());
                    item.Attributes.Add("data-IsTicketCoveredByTestType", dsttypes.Tables[0].Rows[i]["IsTicketCoveredByTestType"].ToString());
                    ddlTestTypes.Items.Add(item);
                }
            }
        }

        DataSet dststatus = objTestTypes.GetAllTestStatus(objPropTestType);

        if (dststatus.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dststatus.Tables[0].Rows.Count; i++)
            {
                ddlStatus.Items.Add(new ListItem(dststatus.Tables[0].Rows[i]["Status"].ToString(), dststatus.Tables[0].Rows[i]["ID"].ToString()));
            }
        }
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            //dvCompanyPermission.Visible = true;
        }
        else
        {
            //dvCompanyPermission.Visible = false;
        }
    }

    private void BindControls()
    {
        objPropUser.ConnConfig = WebBaseUtility.ConnectionString;
        objPropUser.EquipID = Convert.ToInt32(Request.QueryString["elv"]);
        DataSet ds = new DataSet();
        ds = objBL_User.getequipByID(objPropUser);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtUnit.Text = ds.Tables[0].Rows[0]["Unit"].ToString();
            hdnaccount.Value = ds.Tables[0].Rows[0]["Loc"].ToString();
            txtAccount.Text = ds.Tables[0].Rows[0]["location"].ToString();
            hdnEquipment.Value = ds.Tables[0].Rows[0]["unitid"].ToString();
            txtstate.Text = ds.Tables[0].Rows[0]["state"].ToString();
            txtTestNextDueDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            txtLastDueDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            //txtClassification.Text= ds.Tables[0].Rows[0]["Classification"].ToString();
            ddlClassification.SelectedValue = ds.Tables[0].Rows[0]["Classification"].ToString();
            txtEquipmentType.Text = ds.Tables[0].Rows[0]["Type"].ToString();
        }

    }

    private void BindTest(int elv, int lid)
    {
        int TestYear = 0;


        if (Request.QueryString["tyear"] != null)
        {
            TestYear = Convert.ToInt32(Request.QueryString["tyear"]);
        }

        SafetyTest objproptest = new SafetyTest();
        BL_SafetyTest objtestbl = new BL_SafetyTest();
        objproptest.ConnConfig = WebBaseUtility.ConnectionString;
        objproptest.Equipid = elv;
        objproptest.LID = lid;
        objproptest.PriceYear = TestYear;
        DataSet test = objtestbl.GetTestDetailsByYear(objproptest);

        if (test.Tables[0].Rows.Count > 0)
        {
            String txtticket = String.Empty;
            txtUnit.Text = test.Tables[0].Rows[0]["Unit"].ToString();
            hdnaccount.Value = test.Tables[0].Rows[0]["Loc"].ToString();
            hdnEquipment.Value = test.Tables[0].Rows[0]["NID"].ToString();
            hdnNumberOfTestNoTicketInLoc.Value = test.Tables[0].Rows[0]["NumberOfTestNoTicketInLoc"].ToString();
            txtAccount.Text = test.Tables[0].Rows[0]["Tag"].ToString();
            txtstate.Text = test.Tables[0].Rows[0]["state"].ToString();
            txtTestNextDueDate.Text = test.Tables[0].Rows[0]["Next"] != DBNull.Value ? Convert.ToDateTime(test.Tables[0].Rows[0]["Next"]).ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy");
            txtLastDueDate.Text = test.Tables[0].Rows[0]["LastDue"] != DBNull.Value ? Convert.ToDateTime(test.Tables[0].Rows[0]["LastDue"]).ToString("MM/dd/yyyy") : "";//DateTime.Now.ToString("MM/dd/yyyy");
            txtLasttestdon.Text = test.Tables[0].Rows[0]["Last"] != DBNull.Value ? Convert.ToDateTime(test.Tables[0].Rows[0]["Last"]).ToString("MM/dd/yyyy") : "";//DateTime.Now.ToString("MM/dd/yyyy");
            hdntestid.Value = test.Tables[0].Rows[0]["LID"].ToString();
            ddlStatus.SelectedValue = test.Tables[0].Rows[0]["StatusValue"].ToString();
            ddlTestTypes.SelectedValue = test.Tables[0].Rows[0]["LTID"].ToString();
            txtticket = test.Tables[0].Rows[0]["idTicket"] != DBNull.Value ? Convert.ToString(test.Tables[0].Rows[0]["idTicket"]) : string.Empty;
            txtTicketStatus.Text = test.Tables[0].Rows[0]["TicketStatusText"] != DBNull.Value ? Convert.ToString(test.Tables[0].Rows[0]["TicketStatusText"]) : string.Empty;

            txtWorker.Text = test.Tables[0].Rows[0]["CallSign"] != DBNull.Value ? Convert.ToString(test.Tables[0].Rows[0]["CallSign"]) : string.Empty;
            if (txtticket != "")
            {
                txtWorker.Enabled = false;
            }
            hdnWorker.Value = test.Tables[0].Rows[0]["idWorker"] != DBNull.Value ? Convert.ToString(test.Tables[0].Rows[0]["idWorker"]) : string.Empty;
            txtSchedule.Text = test.Tables[0].Rows[0]["EDate"] != DBNull.Value ? Convert.ToDateTime(test.Tables[0].Rows[0]["Edate"]).ToString("MM/dd/yyyy hh:mm tt") : string.Empty;

            string str1 = test.Tables[0].Rows[0]["JobId"] != DBNull.Value ? " - " : "";
            txtJob.Text = test.Tables[0].Rows[0]["JobId"] + str1 + test.Tables[0].Rows[0]["JobName"];
            hdnjob.Value = test.Tables[0].Rows[0]["JobId"] != DBNull.Value ? Convert.ToString(test.Tables[0].Rows[0]["JobId"]) : "";
            txtCat.Text = test.Tables[0].Rows[0]["Cat"].ToString();

            txtAmount.Text = test.Tables[0].Rows[0]["Amount"].ToString() == "" ? "0" : test.Tables[0].Rows[0]["Amount"].ToString();

            txtOverrideAmount.Text = test.Tables[0].Rows[0]["OverrideAmount"].ToString();
            txtThirdPartyName.Text = test.Tables[0].Rows[0]["ThirdPartyName"].ToString();
            hdnThirdPartyName.Value = test.Tables[0].Rows[0]["ThirdPartyName"].ToString();
            txtThirdPartyPhone.Text = test.Tables[0].Rows[0]["ThirdPartyPhone"].ToString();

            txtCusField1.Text = test.Tables[0].Rows[0]["Custom1"].ToString();
            txtCusField2.Text = test.Tables[0].Rows[0]["Custom2"].ToString();
            txtCusField3.Text = test.Tables[0].Rows[0]["Custom3"].ToString();
            txtCusField4.Text = test.Tables[0].Rows[0]["Custom4"].ToString();

            drpdwnTestDueBy.SelectedValue = test.Tables[0].Rows[0]["TestDueBy"].ToString();

            chkChargeableEdit.Checked = test.Tables[0].Rows[0]["Chargeable"] == DBNull.Value ? false : Convert.ToBoolean(test.Tables[0].Rows[0]["Chargeable"]);

            chkThirdParty.Checked = test.Tables[0].Rows[0]["ThirdPartyRequired"] == DBNull.Value ? false : Convert.ToBoolean(test.Tables[0].Rows[0]["ThirdPartyRequired"]);

            if (!chkThirdParty.Checked)
            {
                //Query EquipmentTestPricing to update Third Party Checkbox
                chkThirdParty.Checked = test.Tables[0].Rows[0]["ThirdPartyRequiredTestPricing"] == DBNull.Value ? false : Convert.ToBoolean(test.Tables[0].Rows[0]["ThirdPartyRequiredTestPricing"]);
            }

            hdnParentTestID.Value = test.Tables[0].Rows[0]["ParentTestID"].ToString();
            hdnIsParentTestType.Value = test.Tables[0].Rows[0]["IsParentTestType"].ToString();
            hdnTestTypeChildID.Value = test.Tables[0].Rows[0]["TestTypeChildID"].ToString();
            hdnTestTypeChildName.Value = test.Tables[0].Rows[0]["TestTypeChildName"].ToString();
            hdnHasChildTest.Value = test.Tables[0].Rows[0]["HasChildTest"].ToString();
            hdnTestTypeParentName.Value = test.Tables[0].Rows[0]["TestTypeParentName"].ToString();
            hdnTestTypeParentID.Value = test.Tables[0].Rows[0]["TestTypeParentID"].ToString();
            hdnHasParentTest.Value = test.Tables[0].Rows[0]["HasParentTest"].ToString();
            hdnParentTestID.Value = test.Tables[0].Rows[0]["ParentTestID"].ToString();
            hdnTestTypeParentChargable.Value = test.Tables[0].Rows[0]["TestTypeParentChargable"].ToString();
             

            hdnRolId.Value = test.Tables[0].Rows[0]["RolID"].ToString();

            //txtUnit.ReadOnly = true;
            if (txtticket == string.Empty)
            {
                lnkAssign.Visible = true; 
            }
            else
            {
                lnkAssign.Visible = false;
                lnkTicket.HRef = "addticket.aspx?id=" + test.Tables[0].Rows[0]["idTicket"].ToString() + "&comp=0&pop=1&screen=STEdit&elv=" + Request.QueryString["elv"] + "&LID=" + Request.QueryString["LID"];
                lnkTicket.InnerHtml = test.Tables[0].Rows[0]["idTicket"].ToString();
            }

            lblwho.Text = test.Tables[0].Rows[0]["Who"].ToString();

            //txtClassification.Text= test.Tables[0].Rows[0]["Classification"].ToString();

            ListItem item = ddlClassification.Items.FindByValue(test.Tables[0].Rows[0]["Classification"].ToString());

            if (item != null)
            {
                ddlClassification.SelectedValue = test.Tables[0].Rows[0]["Classification"].ToString();
            }
            else {
                ddlClassification.SelectedIndex = 0; 
            }

          

            txtEquipmentType.Text = test.Tables[0].Rows[0]["Type"].ToString();
            //Load Pricing
            //String equipmentClassification = txtClassification.Text;
            String equipmentClassification = ddlClassification.SelectedValue;
            int testtypeId = Convert.ToInt32(ddlTestTypes.SelectedValue);

            //Get Test Pricing by Classification and TestTypeId
            //Apply for old data  
            if (chkChargeableEdit.Checked)
            {
                if (txtOverrideAmount.Text.Trim() == "0" || txtOverrideAmount.Text.Trim() == "0.00")
                {
                    if (txtAmount.Text.Trim() == "0" || txtAmount.Text.Trim() == "0.00")
                    {
                        DataSet dsPricing = new DataSet();
                        BusinessEntity.User objProp_User = new BusinessEntity.User();
                        dsPricing = objBL_User.ValidateEquipmentTestPricing(Session["config"].ToString(), equipmentClassification, testtypeId, TestYear);

                        if (dsPricing.Tables[0].Rows.Count > 0)
                        {
                            Double price = 0;
                            Double overrideAmount = 0;
                            if (dsPricing.Tables[0].Rows.Count > 0)
                            {
                                price = Convert.ToDouble(dsPricing.Tables[0].Rows[0]["Amount"]);
                                overrideAmount = Convert.ToDouble(dsPricing.Tables[0].Rows[0]["Override"]);
                            }
                            txtAmount.Text = price.ToString();
                            txtOverrideAmount.Text = overrideAmount.ToString();
                        }
                    }
                }
                txtAmount.Enabled = true;
                txtOverrideAmount.Enabled = true;
            }
            else
            {
                txtAmount.Enabled = false;
                txtOverrideAmount.Enabled = false;
            }


            DataSet dsTestType = objtestbl.GetTestTypeById(objproptest.ConnConfig, Convert.ToInt32(ddlTestTypes.SelectedValue));

            if (dsTestType.Tables.Count > 0)
            {
                if (dsTestType.Tables[0].Rows.Count > 0)
                {
                    hdnTestTypeFrequency.Value = dsTestType.Tables[0].Rows[0]["Frequency"].ToString();
                }

            }

            if (Convert.ToInt32(test.Tables[0].Rows[0]["ExistData"]) == 0)
            {
                divSuccess.Visible = true;
                lnkAssign.Visible = false;
                lnkAddPrice.Visible = false;
                lnkEditPrice.Visible = false;
                lnkDeletePrice.Visible = false;
                lnkGeneralProposal.Visible = true;
                lnkAddSchedule.Visible = false;
                lnkEditSchedule.Visible = false;
                lnkDeleteSchedule.Visible = false;
                lblExistTestMsg.InnerText = "Please note this test is due next on " + txtTestNextDueDate.Text;

            }
            else
            {
                divSuccess.Visible = false;
                lnkAssign.Visible = true;
                lnkAddPrice.Visible = true;
                lnkEditPrice.Visible = true;
                lnkDeletePrice.Visible = true;
                lnkGeneralProposal.Visible = true;
                lnkAddSchedule.Visible = true;
                lnkEditSchedule.Visible = true;
                lnkDeleteSchedule.Visible = true;
            }

        }

        if (ddlStatus.SelectedValue == "1" && lnkTicket.InnerText.Length > 0)
        {
            lnkAssign.Visible = false;
        }

    }


 

    private bool DuplicateTest(SafetyTest objproptest)
    {
        bool testexists = false;
        BL_SafetyTest objtestbl = new BL_SafetyTest();

        DataSet ds = objtestbl.ValidateTest(objproptest);
        if (ds != null)
        {
            if (objproptest.LID == 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    testexists = true;
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (objproptest.LID != Convert.ToInt32(ds.Tables[0].Rows[0]["LID"]))
                    {
                        testexists = true;
                    }
                }
            }
        }

        return testexists;
    }

     

    private bool isFirstItem()
    {
        if (Session["safetytest"] != null)
        {
            DataTable dt = (DataTable)Session["safetytest"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["SafetyTestID"];
            dt.PrimaryKey = keyColumns;
            DataRow delv = dt.Rows.Find(Request.QueryString["LID"].ToString());
            int index = dt.Rows.IndexOf(delv);
            if (index == 0)
                return true;
            return false;
        }
        else
        { return true; }
    }

    private bool isLastItem()
    {
        if (Session["safetytest"] != null)
        {
            DataTable dt = (DataTable)Session["safetytest"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["SafetyTestID"];
            dt.PrimaryKey = keyColumns;
            DataRow delv = dt.Rows.Find(Request.QueryString["LID"].ToString());
            int index = dt.Rows.IndexOf(delv);
            if (index == dt.Rows.Count - 1)
                return true;
            return false;
        }
        else
        { return true; }
    }

    #endregion

    #region :: Event :: 
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["safetytest"];
        var redirect = string.Format("AddTests.aspx?elv={0}&LID={1}", dt.Rows[0]["NID"], dt.Rows[0]["LID"]);
        Response.Redirect(redirect);
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
      

        try
        {
            string STRLID = Request.QueryString["LID"].ToString();

            string STRELEV = Request.QueryString["elv"].ToString();
          

            DataTable dt = (DataTable)Session["safetytest"];

            for (int i = 1; i < dt.Rows.Count; i++)
            {

                if (dt.Rows[i]["LID"].ToString() == STRLID && i > 0)
                {
                    STRLID = dt.Rows[i - 1]["LID"].ToString();

                    STRELEV = dt.Rows[i - 1]["Elev"].ToString();

                    break;
                }
            }


            string redirect = "";
            redirect = string.Format("AddTests.aspx?elv={0}&LID={1}", STRELEV, STRLID);
            Response.Redirect(redirect);
        }
        catch { }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            string STRLID = Request.QueryString["LID"].ToString();

            string STRELEV = Request.QueryString["elv"].ToString();

            DataTable dt = (DataTable)Session["safetytest"];

            for (int i = 1; i < dt.Rows.Count; i++)
            {

                if (dt.Rows[i]["LID"].ToString() == STRLID && i < dt.Rows.Count)
                {
                    STRLID = dt.Rows[i + 1]["LID"].ToString();

                    STRELEV = dt.Rows[i + 1]["Elev"].ToString();

                    break;
                }
            }


            string redirect = "";
            redirect = string.Format("AddTests.aspx?elv={0}&LID={1}", STRELEV, STRLID);
            Response.Redirect(redirect);
        }
        catch { }
        
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["safetytest"];
        var redirect = string.Format("AddTests.aspx?elv={0}&LID={1}", dt.Rows[dt.Rows.Count - 1]["NID"], dt.Rows[dt.Rows.Count - 1]["LID"]);
        Response.Redirect(redirect);
    }

    protected void lnkAssignTicket_Click(object sender, EventArgs e)
    {
         

        if (Request.QueryString["LID"] != null & hdnEquipment.Value != string.Empty)
        {
            IsCreditHold(Convert.ToInt32(Request.QueryString["LID"]));

            if (ddlStatus.SelectedValue == "0" && lnkTicket.InnerText.Length == 0)
            {
                bool hasError = false;
                var msg = new List<string>();
                var success = 0;


                try
                {
                    SafetyTest objproptest = new SafetyTest();
                    BL_SafetyTest objtestbl = new BL_SafetyTest();
                    objproptest.ConnConfig = WebBaseUtility.ConnectionString;
                    objproptest.LID = Convert.ToInt32(Request.QueryString["LID"]);
                    objproptest.PriceYear = Convert.ToInt32(Request.QueryString["tyear"]);
                    objproptest.CreateTicketForAll = false;
                    if (hdnCreateTicketForAll.Value == "1")
                    {
                        objproptest.CreateTicketForAll = true;
                    }

                    DataSet test = objtestbl.GetTestDetailsByYear(objproptest);
                    //Validate test
                    if (test.Tables[0].Rows.Count > 0)
                    {

                        string strticket = test.Tables[0].Rows[0]["idTicket"] != DBNull.Value ? Convert.ToString(test.Tables[0].Rows[0]["idTicket"]) : string.Empty;
                        objproptest.OldStatus = Convert.ToInt32(test.Tables[0].Rows[0]["StatusValue"].ToString());

                        DateTime? Lastdate = null;

                        if (test.Tables[0].Rows[0]["Last"] != DBNull.Value)
                            Lastdate = Convert.ToDateTime(test.Tables[0].Rows[0]["Last"]);

                        //No tickets assigned
                        if (string.IsNullOrEmpty(strticket))
                        {
                            objproptest.Status = 1;
                            objproptest.Statusstr = "Assigned";
                            objproptest.Lastdate = Lastdate;
                            objproptest.UserName = Convert.ToString(HttpContext.Current.Session["username"]);
                            if (hdnCreateTicketForAll.Value == "1")
                            {
                                success = objtestbl.CreateTicketsByYearForAllTestInLocation(objproptest);
                            }
                            else
                            {
                                success = objtestbl.CreateTicketByYear(objproptest);
                            }

                        }
                        else
                        {
                            success = -2;
                        }
                    }

                    switch (success)
                    {
                        case 0:
                            hasError = true;
                            msg.Add("Ticket could not be created for test " + test.Tables[0].Rows[0]["Unit"] + " " + test.Tables[0].Rows[0]["Tag"]);

                            break;
                        case 1:
                            hasError = false;
                            msg.Add("Ticket created successfully for test " + test.Tables[0].Rows[0]["Unit"] + " " + test.Tables[0].Rows[0]["Tag"]);

                            break;
                        case -1:

                            hasError = true;
                            msg.Add("Ticket could not be created for test " + test.Tables[0].Rows[0]["Unit"] + " " + test.Tables[0].Rows[0]["Tag"]);
                            break;
                        case -2:
                            hasError = true;
                            msg.Add("Cannot create ticket for test " + test.Tables[0].Rows[0]["Unit"] + " " + test.Tables[0].Rows[0]["Tag"] + ". Already assigned.");

                            break;

                        default:
                            break;
                    }

                }
                catch
                {
                    //TODO: Log error
                    hasError = true;
                    msg.Add("Unexpected error has occurred");
                }


                if (msg.Any())
                {
                    var messageType = hasError ? "warning" : "success";

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "showMessage", "noty({text: '" + string.Join(",", msg) + "',  type : '" + messageType + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); setTimeout(lnkNavigationPage, 3000);", true);

                    RadGrid_gvLogs.Rebind();
                    gvSchedule.Rebind();
                }

                BindTest(Convert.ToInt32(Request.QueryString["elv"]), Convert.ToInt32(Request.QueryString["LID"]));



            }

            else {
                lnkAssign.Visible = false;
                var messageType =  "warning"  ;

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showMessage123", "noty({text: '" + string.Join(",", "Ticket#"+lnkTicket.InnerText  ) + "',  type : '" + messageType + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); ", true);

            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtTestNextDueDate.Text == txtLastDueDate.Text)
        {
            
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keyIsCreditHold", "noty({text: 'Last due date can not be equal to next due date!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        else
        { 

            if (Page.IsValid)
            {
                try
                {
                    SafetyTest objproptest = new SafetyTest();

                    BL_SafetyTest objtestbl = new BL_SafetyTest();

                    objproptest.ConnConfig = WebBaseUtility.ConnectionString;

                    objproptest.Typeid = Convert.ToInt32(ddlTestTypes.SelectedValue);

                    objproptest.Status = Convert.ToInt32(ddlStatus.SelectedItem.Value);

                    objproptest.Locid = Convert.ToInt32(hdnaccount.Value);

                    objproptest.Equipid = Request.QueryString["elv"] != null ? Convert.ToInt32(Request.QueryString["elv"]) : Convert.ToInt32(hdnEquipment.Value);

                    objproptest.Last = !string.IsNullOrEmpty(txtLasttestdon.Text) ? Convert.ToDateTime(txtLasttestdon.Text) : (Nullable<DateTime>)null;

                    objproptest.Next = !string.IsNullOrEmpty(txtTestNextDueDate.Text) ? Convert.ToDateTime(txtTestNextDueDate.Text) : (Nullable<DateTime>)null;

                    objproptest.Lastdate = !string.IsNullOrEmpty(txtLastDueDate.Text) ? Convert.ToDateTime(txtLastDueDate.Text) : (Nullable<DateTime>)null;

                    objproptest.UserName = Convert.ToString(Session["Username"]);

                    objproptest.Statusstr = ddlStatus.SelectedItem.Text;

                    objproptest.LID = string.IsNullOrEmpty(hdntestid.Value) ? 0 : Convert.ToInt32(hdntestid.Value);

                    objproptest.Job = !string.IsNullOrEmpty(hdnjob.Value) ? Convert.ToInt32(hdnjob.Value) : (Nullable<int>)null;

                    objproptest.Worker = txtWorker.Text;

                    objproptest.Workerid = !string.IsNullOrEmpty(Convert.ToString(hdnWorker.Value)) ? Convert.ToInt32(hdnWorker.Value) : (Nullable<int>)null;

                    objproptest.Classification = ddlClassification.SelectedValue;

                    objproptest.Custom1 = txtCusField1.Text;
                    objproptest.Custom2 = txtCusField2.Text;
                    objproptest.Custom3 = txtCusField3.Text;
                    objproptest.Custom4 = txtCusField4.Text;
                    objproptest.Charge = Convert.ToInt32(chkChargeableEdit.Checked);
                    objproptest.ThirdParty = Convert.ToInt32(chkThirdParty.Checked);
                    if (chkChargeableEdit.Checked == false)
                    {
                        objproptest.Amount = 0;
                        objproptest.OverrideAmount = 0;
                    }
                    else
                    {
                        if (txtAmount.Text.Trim() != string.Empty)
                            objproptest.Amount = double.Parse(txtAmount.Text.Trim(), NumberStyles.Currency);
                        if (txtOverrideAmount.Text.Trim() != string.Empty)
                            objproptest.OverrideAmount = double.Parse(txtOverrideAmount.Text.Trim(), NumberStyles.Currency);
                    }


                    objproptest.ThirdPartyName = txtThirdPartyName.Text;
                    objproptest.ThirdPartyPhone = txtThirdPartyPhone.Text;
                    objproptest.TestDueBy = Convert.ToInt32(drpdwnTestDueBy.SelectedValue);
                    objproptest.PriceYear = (Convert.ToDateTime(txtTestNextDueDate.Text)).Year;



                    if (Request.QueryString["tyear"] != null)
                    {
                        objproptest.PriceYear = Convert.ToInt32(Request.QueryString["tyear"]);
                    }

                    objproptest.Charge = Convert.ToInt32(chkChargeableEdit.Checked);

                    //Get CustomValue
                    processCustomTable(objproptest);
                    if (!DuplicateTest(objproptest))
                    {
                        if (hdntestid.Value == "")
                        {

                            /// Create New Test
                            /// 
                            objproptest = objtestbl.CreateTestByYear(objproptest);
                            processSendMail(objproptest);
                            processCreateTask(objproptest);
                            hdntestid.Value = Convert.ToString(objproptest.LID);


                            Response.Redirect("AddTests.aspx?elv=" + objproptest.Equipid + "&LID=" + objproptest.LID + "&tyear=" + Convert.ToDateTime(objproptest.Next).Year + "&addnew=1");

                        }
                        else
                        {
                            if (hdnUpdateThirdPartyForAll.Value == "1")
                            {
                                objproptest.UpdateThirdPartyForAll = true;
                            }

                            string Oldlast = "", OldNext = "", OldlastDue = "", Newlast = "", NewNext = "", NewlastDue = "";

                            objproptest.CreateTestHistory = false;

                            DataSet test = objtestbl.GetTestDetailsByYear(objproptest);

                            objproptest.OldTypeid = Convert.ToInt32(test.Tables[0].Rows[0]["LTID"].ToString());

                            objproptest.OldStatus = Convert.ToInt32(test.Tables[0].Rows[0]["StatusValue"].ToString());

                            objproptest.Locid = Convert.ToInt32(test.Tables[0].Rows[0]["Loc"].ToString());

                            objproptest.Equipid = Convert.ToInt32(test.Tables[0].Rows[0]["NID"].ToString());

                            objproptest.Ticket = test.Tables[0].Rows[0]["idTicket"] != DBNull.Value ? Convert.ToInt32(test.Tables[0].Rows[0]["idTicket"]) : (Nullable<int>)null;



                            //OLD Date

                            Oldlast = !string.IsNullOrEmpty(test.Tables[0].Rows[0]["last"].ToString()) ? Convert.ToDateTime(test.Tables[0].Rows[0]["last"]).ToString("MM/dd/yyyy") : "";

                            OldNext = !string.IsNullOrEmpty(test.Tables[0].Rows[0]["next"].ToString()) ? Convert.ToDateTime(test.Tables[0].Rows[0]["next"]).ToString("MM/dd/yyyy") : "";

                            OldlastDue = !string.IsNullOrEmpty(test.Tables[0].Rows[0]["lastdue"].ToString()) ? Convert.ToDateTime(test.Tables[0].Rows[0]["lastdue"]).ToString("MM/dd/yyyy") : "";


                            //NEW Date

                            Newlast = !string.IsNullOrEmpty(objproptest.Last.ToString()) ? Convert.ToDateTime(objproptest.Last).ToString("MM/dd/yyyy") : "";

                            NewNext = !string.IsNullOrEmpty(objproptest.Next.ToString()) ? Convert.ToDateTime(objproptest.Next).ToString("MM/dd/yyyy") : "";

                            NewlastDue = !string.IsNullOrEmpty(objproptest.Lastdate.ToString()) ? Convert.ToDateTime(objproptest.Lastdate).ToString("MM/dd/yyyy") : "";

                            if (Oldlast != Newlast)
                            { objproptest.CreateTestHistory = true; }

                            if (OldNext != NewNext)
                            { objproptest.CreateTestHistory = true; }

                            if (OldlastDue != NewlastDue)
                            { objproptest.CreateTestHistory = true; }

                            if (objproptest.Status != objproptest.OldStatus)
                            { objproptest.CreateTestHistory = true; }


                            ////Update Test 
                            objproptest = objtestbl.UpdateTestByYear(objproptest);
                            //Send mailOpenSelectFormWindow
                            processSendMail(objproptest);
                            processCreateTask(objproptest);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Test updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                            CreateTestCustomTable();
                            BindTestCustomGrid();
                            BindTest(Convert.ToInt32(Request.QueryString["elv"]), Convert.ToInt32(Request.QueryString["LID"]));
                            loadLog();
                            RadGrid_gvLogs.DataBind();
                            BindPriceHistory();
                            RadGrid_PriceHistory.Rebind();

                            string path = HttpContext.Current.Request.Url.ToString();

                            Response.Redirect(path);

                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'This test already exists!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                    }


                }
                catch (Exception ex)
                {
                    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
             
        }
        if (ddlStatus.SelectedValue == "1" && lnkTicket.InnerText.Length > 0)
        {
            lnkAssign.Visible = false;
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        //if (Session["PageType"] == "Equip")
        //{
        //    if (!string.IsNullOrEmpty(Request.QueryString["elv"]))
        //    {
        //        Response.Redirect("addequipment.aspx?uid=" + (Request.QueryString["elv"]));
        //    }
        //    else
        //        Response.Redirect("Equipments.aspx");

        //}
        //else
        //{
        //    Response.Redirect("SafetyTest.aspx");
        //}
        Response.Redirect("SafetyTest.aspx?fil=1");

    }
    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objGeneral.CustomName = name;
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getCustomFields(objGeneral);
        return ds;
    }
    #endregion



    #region "Contact"
    private DataSet getContactData()
    {
        DataSet ds = new DataSet();
        if (Request.QueryString["LID"] != null)
        {
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.LocID = Convert.ToInt32(hdnaccount.Value);

            ds = objBL_User.getLocationByID(objPropUser);


        }
        return ds;
    }
    private void BindContact(int elv, int lid)
    {
        try
        {
            RadGrid_Contacts.AllowCustomPaging = !ShouldApplySortFilterOrGroup();

            DataSet ds = new DataSet();
            ds = getContactData();
            if (ds.Tables.Count > 0)
            {
                RadGrid_Contacts.VirtualItemCount = ds.Tables[1].Rows.Count;

                RadGrid_Contacts.DataSource = ds.Tables[1];
                ViewState["contacttableloc"] = ds.Tables[1];
            }
        }
        catch { }
    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        String msg = "added";
        DataTable dt = (DataTable)ViewState["contacttableloc"];
        ViewState["index"] = hdnIndex.Value;
        DataRow dr = dt.NewRow();

        dr["Name"] = Truncate(txtContcName.Text, 50);
        dr["Phone"] = Truncate(txtContPhone.Text, 22);
        dr["Fax"] = Truncate(txtContFax.Text, 22);
        dr["Cell"] = Truncate(txtContCell.Text, 22);
        dr["Email"] = Truncate(txtContEmail.Text, 50);
        dr["Title"] = Truncate(txtTitle.Text, 50);
        dr["EmailTicket"] = chkEmailTicket.Checked;

        dr["EmailRecInvoice"] = chkEmailInvoice.Checked;
        dr["ShutdownAlert"] = chkShutdownAlert.Checked;
        dr["EmailRecTestProp"] = chkTestProposals.Checked;
        if (hdnAddEditContact.Value == "1")
        {
            dt.Rows.RemoveAt(Convert.ToInt32(ViewState["index"]));
            dt.Rows.InsertAt(dr, Convert.ToInt32(ViewState["index"]));

            msg = "updated";
        }
        else
        {
            dt.Rows.Add(dr);
        }

        dt.AcceptChanges();

        ViewState["contacttableloc"] = dt;
        //RadGrid_Contacts.VirtualItemCount = dt.Rows.Count;
        //RadGrid_Contacts.DataSource = dt;
        //RadGrid_Contacts.Rebind();

        ClearContact();

        if (ViewState["mode"].ToString() == "1")
        {
            SubmitContact();
        }

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseContactWindow();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Contact " + msg + " successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        BindContact(Convert.ToInt32(Request.QueryString["elv"]), Convert.ToInt32(Request.QueryString["LID"]));
        RadGrid_Contacts.Rebind();
    }

    private void SubmitContact()
    {
        try
        {
            if (ViewState["contacttableloc"] != null)
            {
                objPropUser.ContactData = (DataTable)ViewState["contacttableloc"];
            }

            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                objPropUser.RolId = Convert.ToInt32(hdnRolId.Value);
                objPropUser.ConnConfig = Session["config"].ToString();
                objBL_User.UpdateLocationContact(objPropUser);

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }


    private DataTable CreateTable()
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

        dt.Columns.Add("EmailRecInvoice", typeof(bool));
        dt.Columns.Add("ShutdownAlert", typeof(bool));
        dt.Columns.Add("EmailRecTestProp", typeof(bool));

        return dt;
    }

    private void ClearContact()
    {
        txtContcName.Text = string.Empty;
        txtContPhone.Text = string.Empty;
        txtContFax.Text = string.Empty;
        txtContCell.Text = string.Empty;
        txtContEmail.Text = string.Empty;
        txtTitle.Text = string.Empty;

        chkEmailTicket.Checked = false;
        chkEmailInvoice.Checked = false;
        chkShutdownAlert.Checked = false;
        chkTestProposals.Checked = false;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (hdnDeleteContact.Value == "Y")
        {
            DataTable dt = (DataTable)ViewState["contacttableloc"];

            foreach (GridDataItem item in RadGrid_Contacts.SelectedItems)
            {
                Label lblindex = (Label)item.Cells[1].FindControl("lblindex");
                dt.Rows.RemoveAt(Convert.ToInt32(lblindex.Text));
            }

            dt.AcceptChanges();
            ViewState["contacttableloc"] = dt;

            RadGrid_Contacts.VirtualItemCount = dt.Rows.Count;
            RadGrid_Contacts.DataSource = dt;
            RadGrid_Contacts.Rebind();

            if (ViewState["mode"].ToString() == "1")
            {
                SubmitContact();
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

    protected void RadGrid_Contacts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        RadGrid_Contacts.DataSource = CreateTable();
        BindContact(Convert.ToInt32(Request.QueryString["elv"]), Convert.ToInt32(Request.QueryString["LID"]));

    }

    protected void RadGrid_Contacts_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Contacts.Items)
        {
            Label lblMail = (Label)item.FindControl("lblEmail");
            String email = lblMail.Text;
            item.Attributes["onclick"] = "SelectRowmailPage('" + lblMail.ClientID + "','" + lnkMail.ClientID + "');";

        }
    }

    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Contacts.MasterTableView.FilterExpression != "" ||
            (RadGrid_Contacts.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Contacts.MasterTableView.SortExpressions.Count > 0;
    }

    public void reloadContactData(int locId)
    {
        try
        {
            RadGrid_Contacts.AllowCustomPaging = !ShouldApplySortFilterOrGroup();

            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.LocID = Convert.ToInt32(locId);
            DataSet ds = new DataSet();
            ds = objBL_User.getLocationByID(objPropUser);

            RadGrid_Contacts.VirtualItemCount = ds.Tables[1].Rows.Count;

            RadGrid_Contacts.DataSource = ds.Tables[1];
            RadGrid_Contacts.Rebind();
            ViewState["contacttableloc"] = ds.Tables[1];
        }
        catch { }
    }

    protected void btnReloadContact_Click(object sender, EventArgs e)
    {
        if (hdnaccount.Value != "")
        {
            lnkAddnew.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            lnkMail.Enabled = false;
            reloadContactData(Convert.ToInt32(hdnaccount.Value));
        }

    }
    #endregion


    private void PagePermission()
    {
        //if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        //{
        //    DataTable ds = new DataTable();
        //    ds = (DataTable)Session["userinfo"];

        //     //Contact
        //    string ContactPermission = ds.Rows[0]["ContactPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["ContactPermission"].ToString();
        //    hdnAddeContact.Value = ContactPermission.Length < 1 ? "Y" : ContactPermission.Substring(0, 1);
        //    hdnEditeContact.Value = ContactPermission.Length < 2 ? "Y" : ContactPermission.Substring(1, 1);
        //    hdnDeleteContact.Value = ContactPermission.Length < 3 ? "Y" : ContactPermission.Substring(2, 1);
        //    hdnViewContact.Value = ContactPermission.Length < 4 ? "Y" : ContactPermission.Substring(3, 1);

        //    if (hdnAddeContact.Value == "N")
        //    {
        //        lnkAddnew.Enabled = false;
        //    }

        //    if (hdnEditeContact.Value == "N")
        //    {
        //        btnEdit.Enabled = false;
        //    }

        //    if (hdnDeleteContact.Value == "N")
        //    {
        //        btnDelete.Enabled = false;
        //    }
        //    pnlgvConPermission.Visible = hdnViewContact.Value == "N" ? false : true;
        //}
        //else
        //{
        //    hdnAddeContact.Value = "Y";
        //    hdnEditeContact.Value = "Y";
        //    hdnDeleteContact.Value = "Y";
        //    hdnViewContact.Value = "Y";
        //}

        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// AccountPayablemodulePermission ///////////////////------->

            string RCmodulePermission = ds.Rows[0]["RCmodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["RCmodulePermission"].ToString();

            if (RCmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no");
                return;
            }

            /// SafetyTestsPermission ///////////////////------->

            string SafetyTestsPermission = ds.Rows[0]["SafetyTestsPermission"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["SafetyTestsPermission"].ToString();
            string ADD = SafetyTestsPermission.Length < 1 ? "Y" : SafetyTestsPermission.Substring(0, 1);
            string Edit = SafetyTestsPermission.Length < 2 ? "Y" : SafetyTestsPermission.Substring(1, 1);
            string Delete = SafetyTestsPermission.Length < 3 ? "Y" : SafetyTestsPermission.Substring(2, 1);
            string View = SafetyTestsPermission.Length < 4 ? "Y" : SafetyTestsPermission.Substring(3, 1);

            if (Request.QueryString["elv"] != null)
            {
                //aImport.Visible = false;
            }
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no");
                return;
            }
            else if (Request.QueryString["elv"] == null)
            {
                if (ADD == "N")
                {
                    Response.Redirect("Home.aspx?permission=no");
                    return;
                }
            }
            else if (Edit == "N")
            {
                if (View == "Y")
                {
                    btnSubmit.Visible = false;
                    //btnSubmitJob.Visible = false;
                }
                else
                {
                    Response.Redirect("Home.aspx?permission=no");
                    return;
                }
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


    #region Custom
    private void BindTestCustomGrid()
    {
        try
        {

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["TestCustomTable"];

            gvTestCustom.DataSource = dt;
            gvTestCustom.VirtualItemCount = dt.Rows.Count;
            gvTestCustom.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataTable FillMembers(string userName)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        try
        {
            BusinessEntity.User objProp_User = new BusinessEntity.User();
            ds = objBL_User.getTeamByMonUser(Session["config"].ToString(), userName);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    public DataTable getTestCustomValue(int testId, int equiId)
    {
        DataSet ds = new DataSet();
        BL_SafetyTest _objbltesttypes = new BL_SafetyTest();
        ds = _objbltesttypes.GetTestCustomValueByEquipTest(Session["config"].ToString(), testId, equiId);
        return ds.Tables[0];
    }


    protected void gvTestCustom_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            HiddenField hdnTestItemValueID = (HiddenField)item.FindControl("hdnTestItemValueID");
            HiddenField hdnMembers = (HiddenField)item.FindControl("hdnMembers");
            TextBox txtMembers = (TextBox)item.FindControl("txtMembers");
            DropDownList ddlFormat = (DropDownList)item.FindControl("ddlFormat");
            // RadComboBox drpMember = (RadComboBox)item.FindControl("ddlTeamMember");
            CheckBox chkSelectAlert = (CheckBox)item.FindControl("chkSelectAlert");
            Label lbUpdatedBy = (Label)item.FindControl("lbUpdatedBy");
            Label lbUpdatedDate = (Label)item.FindControl("lbUpdatedDate");
            //TextBox txtRoles = (TextBox)item.FindControl("txtRoles");
            //HiddenField hdnRoles = (HiddenField)item.FindControl("hdnRoles");

            int format = Convert.ToInt32(DataBinder.Eval(item.DataItem, "Format"));
            int id = Convert.ToInt32(DataBinder.Eval(item.DataItem, "ID"));



            //Process data get from TestItemCustomValues
            DataTable dtItemValue = (DataTable)ViewState["TestItemCustomValues"];
            String customValue = "";
            String teamMember = "";
            String RolesMember = "";
            Boolean isAlert = Convert.ToBoolean(DataBinder.Eval(item.DataItem, "IsAlert"));

            if (dtItemValue.Rows.Count > 0)
            {
                if (dtItemValue.Select("tblTestCustomFieldsID = " + id + "").Count() > 0)
                {
                    DataRow resultItemValue = dtItemValue.Select("tblTestCustomFieldsID = " + id + "").First();
                    lbUpdatedBy.Text = resultItemValue["UpdatedBy"].ToString();
                    lbUpdatedDate.Text = resultItemValue["UpdatedDate"].ToString() == "" ? "" : Convert.ToDateTime(resultItemValue["UpdatedDate"]).ToString("MM/dd/yyyy HH:mm");
                    hdnTestItemValueID.Value = resultItemValue["ID"].ToString();
                    customValue = resultItemValue["Value"].ToString();
                    teamMember = resultItemValue["TeamMember"].ToString();
                    hdnMembers.Value = resultItemValue["TeamMember"].ToString();
                    txtMembers.Text = resultItemValue["TeamMemberDisplay"].ToString();

                    RolesMember = resultItemValue["UserRoles"].ToString();
                    //hdnRoles.Value = resultItemValue["UserRoles"].ToString();
                    //txtRoles.Text = resultItemValue["UserRolesDisplay"].ToString();

                    isAlert = Convert.ToBoolean(resultItemValue["IsAlert"]);
                }

            }
            else
            {
                lbUpdatedBy.Text = "";
                lbUpdatedDate.Text = "";
                hdnTestItemValueID.Value = "0";
                teamMember = "";
            }

            chkSelectAlert.Checked = isAlert;

            switch (format)
            {
                case 1:
                    {
                        Panel divFormatCurrent = (Panel)item.FindControl("divFormatCurrent");
                        divFormatCurrent.Visible = true;
                        TextBox txtFormatCurrent = (TextBox)item.FindControl("txtFormatCurrent");
                        txtFormatCurrent.Text = customValue;
                        break;
                    }
                case 2:
                    {
                        Panel divFormatDate = (Panel)item.FindControl("divFormatDate");
                        divFormatDate.Visible = true;
                        TextBox txtFormatDate = (TextBox)item.FindControl("txtFormatDate");
                        txtFormatDate.Text = customValue;
                        break;
                    }
                case 3:
                    {
                        Panel divFormatText = (Panel)item.FindControl("divFormatText");
                        divFormatText.Visible = true;
                        TextBox txtFormatText = (TextBox)item.FindControl("txtFormatText");
                        txtFormatText.Text = customValue;
                        break;
                    }

                case 4:
                    {
                        Panel divFormatDrop = (Panel)item.FindControl("divFormatDrop");
                        divFormatDrop.Visible = true;
                        DropDownList drpdwnCustom = (DropDownList)item.FindControl("drpdwnCustom");
                        if (ViewState["TestCustomValues"] != null)
                        {
                            DataTable dtCustomval = (DataTable)ViewState["TestCustomValues"];
                            DataTable dataTemp = dtCustomval.Clone();


                            DataRow[] result = dtCustomval.Select("tblTestCustomFieldsID = " + id + "");
                            foreach (DataRow row in result)
                            {
                                dataTemp.ImportRow(row);
                            }

                            if (dataTemp.Rows.Count > 0)
                            {
                                dataTemp.DefaultView.Sort = "Value  ASC";
                                dataTemp = dataTemp.DefaultView.ToTable();
                            }

                            drpdwnCustom.DataSource = dataTemp;
                            drpdwnCustom.DataTextField = "Value";
                            drpdwnCustom.DataValueField = "Value";
                            drpdwnCustom.DataBind();
                            drpdwnCustom.Items.Insert(0, (new ListItem("--Select item--", "")));
                            drpdwnCustom.SelectedValue = customValue;

                        }
                        break;
                    }
                case 5:
                    {
                        Panel divFormatCheckbox = (Panel)item.FindControl("divFormatCheckbox");
                        divFormatCheckbox.Visible = true;
                        CheckBox chkCustomFormat = (CheckBox)item.FindControl("chkCustomFormat");
                        chkCustomFormat.Checked = customValue == "" ? false : Convert.ToBoolean(customValue);
                        break;
                    }
            }


        }

    }

    public DataTable RemoveDuplicateRows(DataTable table, string DistinctColumn)
    {
        try
        {
            ArrayList UniqueRecords = new ArrayList();
            ArrayList DuplicateRecords = new ArrayList();
            foreach (DataRow dRow in table.Rows)
            {
                if (UniqueRecords.Contains(dRow[DistinctColumn]))
                    DuplicateRecords.Add(dRow);
                else
                    UniqueRecords.Add(dRow[DistinctColumn]);
            }

            foreach (DataRow dRow in DuplicateRecords)
            {
                table.Rows.Remove(dRow);
            }

            return table;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    protected void RadComboBox1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {

        RadComboBox obj = sender as RadComboBox;
        obj.Items.Clear();

        obj.ClearCheckedItems();
        obj.ClearSelection();

        obj.DataSource = FillMembers(e.Text);
        obj.DataBind();
    }
    protected void RadComboBox1_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["MomUserID"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["ID"].ToString();
    }
    protected void gvCustom_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            GridDataItem item = (GridDataItem)e.Item;
            LinkButton lnkAddCustomValue = (LinkButton)item.FindControl("lnkAddTestCustomValue");
            LinkButton lnkDelCustomValue = (LinkButton)item.FindControl("lnkDelTestCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)item.FindControl("lnkUpdateTestCustomValue");


            if (e.CommandName.Equals("AddTestCustomValue"))
            {
                TextBox txtCustomValue = (TextBox)item.FindControl("txtTestCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlTestCustomValue");

                Boolean isExistItem = false;
                foreach (ListItem x in ddlCustomValue.Items)
                {
                    if (x.Text == txtCustomValue.Text.Trim())
                    {
                        isExistItem = true;
                        break;
                    }
                }

                if (txtCustomValue.Text.Trim() != string.Empty && !isExistItem)
                {
                    ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                    txtCustomValue.Text = string.Empty;
                    ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();
                }
            }
            else if (e.CommandName.Equals("UpdateTestCustomValue"))
            {

                TextBox txtCustomValue = (TextBox)item.FindControl("txtTestCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlTestCustomValue");
                if (txtCustomValue.Text.Trim() != string.Empty)
                {
                    ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                    ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                    ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();

                }
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
            else if (e.CommandName.Equals("DeleteTestCustomValue"))
            {

                TextBox txtCustomValue = (TextBox)item.FindControl("txtTestCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlTestCustomValue");

                ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                ddlCustomValue.SelectedIndex = 0;
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void CreateTestCustomTable()
    {

        DataSet dst = new DataSet();
        BL_SafetyTest _objbltesttypes = new BL_SafetyTest();
        dst = _objbltesttypes.GetAllTestCustom(Session["config"].ToString(), Session["dbname"].ToString());

        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("OrderNo", typeof(int));
        dt.Columns.Add("Label", typeof(string));
        dt.Columns.Add("IsAlert", typeof(Boolean));
        dt.Columns.Add("TeamMember", typeof(string));
        dt.Columns.Add("Format", typeof(int));
        dt.Columns.Add("TeamMemberDisplay", typeof(string));
        dt.Columns.Add("UserRoles", typeof(string));
        dt.Columns.Add("UserRolesDisplay", typeof(string));
        DataRow dr = dt.NewRow();

        dr["ID"] = 0;
        dr["Line"] = dt.Rows.Count + 1;
        dr["OrderNo"] = 0;
        dr["Label"] = "";
        dr["IsAlert"] = 0;
        dr["TeamMember"] = "";
        dr["Format"] = 0;
        dr["TeamMemberDisplay"] = "";
        dr["UserRoles"] = "";
        dr["UserRolesDisplay"] = "";
        dt.Rows.Add(dr);
        if (dst.Tables[0].Rows.Count == 0)
        {
            ViewState["TestCustomTable"] = dt;
        }
        else
        {
            ViewState["TestCustomTable"] = dst.Tables[0];
        }

        ViewState["TestCustomValues"] = dst.Tables[1];
        //get Test Custom Item value
        int equiId = hdnEquipment.Value == "" ? 0 : Convert.ToInt32(hdnEquipment.Value);
        int testID = 0;
        int testyear = 0;
        if (Request.QueryString["LID"] != null)
        {
            testID = Convert.ToInt32(Request.QueryString["LID"]);
        }
        if (Request.QueryString["tyear"] != null)
        {
            testyear = Convert.ToInt32(Request.QueryString["tyear"]);
        }
        DataSet dsTestItemValue = new DataSet();
        dsTestItemValue = _objbltesttypes.GetTestCustomValueByEquipTestByYear(Session["config"].ToString(), testID, equiId, testyear);
        ViewState["TestItemCustomValues"] = dsTestItemValue.Tables[0];


    }


    private void processCustomTable(SafetyTest obj)
    {
        obj.Cus_CreateTask = new List<NotificationCustomChange>();
        obj.Cus_EmailToTeamMember = new List<NotificationCustomChange>();
        int equiId = hdnEquipment.Value == "" ? 0 : Convert.ToInt32(hdnEquipment.Value);
        int testID = 0;
        if (Request.QueryString["LID"] != null)
        {
            testID = Convert.ToInt32(Request.QueryString["LID"]);
        }
        DataTable dtCustomValue = new DataTable();
        dtCustomValue.Columns.Add("ID", typeof(int));
        dtCustomValue.Columns.Add("tblTestCustomFieldsID", typeof(int));
        dtCustomValue.Columns.Add("Value", typeof(string));
        dtCustomValue.Columns.Add("UpdatedBy", typeof(string));
        dtCustomValue.Columns.Add("TestID", typeof(int));
        dtCustomValue.Columns.Add("EquipmentID", typeof(int));
        dtCustomValue.Columns.Add("IsAlert", typeof(Boolean));
        dtCustomValue.Columns.Add("TeamMember", typeof(string));
        dtCustomValue.Columns.Add("TeamMemberDisplay", typeof(string));
        dtCustomValue.Columns.Add("UserRoles", typeof(string));
        dtCustomValue.Columns.Add("UserRolesDisplay", typeof(string));

        NotificationCustomChange notification = new NotificationCustomChange();
        NotificationCustomChange createTask = new NotificationCustomChange();
        foreach (GridDataItem gr in gvTestCustom.Items)
        {

            HiddenField hdSelectTeam = (HiddenField)gr.FindControl("hdnMembers");
            TextBox txtMembers = (TextBox)gr.FindControl("txtMembers");
            CheckBox chkSelectAlert = (CheckBox)gr.FindControl("chkSelectAlert");
            Label lblFormat = (Label)gr.FindControl("lblFormat");
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblCustom = (Label)gr.FindControl("lblCustom");
            HiddenField hdnTestItemValueID = (HiddenField)gr.FindControl("hdnTestItemValueID");

            //HiddenField hdnRoles = (HiddenField)gr.FindControl("hdnRoles");
            //TextBox txtRoles = (TextBox)gr.FindControl("txtRoles");


            DataRow dr = dtCustomValue.NewRow();
            dr["ID"] = hdnTestItemValueID.Value == "" ? 0 : Convert.ToInt32(hdnTestItemValueID.Value);
            dr["tblTestCustomFieldsID"] = Convert.ToInt32(lblID.Text);
            int format = Convert.ToInt32(lblFormat.Text);

            switch (format)
            {
                case 1:
                    {
                        TextBox txtFormatCurrent = (TextBox)gr.FindControl("txtFormatCurrent");
                        dr["Value"] = txtFormatCurrent.Text;
                        break;
                    }
                case 2:
                    {
                        TextBox txtFormatDate = (TextBox)gr.FindControl("txtFormatDate");
                        dr["Value"] = txtFormatDate.Text;
                        break;
                    }

                case 3:
                    {
                        TextBox txtFormatText = (TextBox)gr.FindControl("txtFormatText");
                        dr["Value"] = txtFormatText.Text;
                        break;
                    }
                case 4:
                    {
                        DropDownList drpdwnCustom = (DropDownList)gr.FindControl("drpdwnCustom");
                        dr["Value"] = drpdwnCustom.SelectedValue;
                        break;
                    }
                case 5:
                    {
                        CheckBox chkCustomFormat = (CheckBox)gr.FindControl("chkCustomFormat");
                        dr["Value"] = chkCustomFormat.Checked;
                        break;
                    }
            }

            dr["UpdatedBy"] = Session["username"].ToString();
            dr["TestID"] = testID;
            dr["EquipmentID"] = equiId;
            dr["IsAlert"] = Convert.ToBoolean(chkSelectAlert.Checked);
            //dr["TeamMember"] = hdSelectTeam.Value.Replace("0_","").Replace("1_", "").Replace("3_", "").Replace("4_", "");
            dr["TeamMember"] = hdSelectTeam.Value;
            dr["TeamMemberDisplay"] = txtMembers.Text;

            //dr["UserRoles"] = hdnRoles.Value;
            //dr["UserRolesDisplay"] = txtRoles.Text;

            dtCustomValue.Rows.Add(dr);

            //Email

            if (Request.QueryString["LID"] != null)
            {
                //check data change
                DataTable oldData = (DataTable)ViewState["TestItemCustomValues"];

                String sql = "TestID = " + testID + " and EquipmentID = " + equiId + " and tblTestCustomFieldsID = " + lblID.Text + " and Value ='" + dr["Value"] + "'";
                int lsChange = oldData.Select(sql).Count();
                if (lsChange == 0)
                {
                    notification = new NotificationCustomChange();
                    notification.SubjectEmail = txtAccount.Text + " - Equip ID " + hdnEquipment.Value + "Test Type " + ddlTestTypes.SelectedItem.Text + " Alert";
                    notification.UserName = Session["username"].ToString();
                    notification.label = lblCustom.Text;
                    notification.EquipmentName = txtUnit.Text;
                    notification.EquipmentDesc = txtEquipmentDesc.Value;


                    createTask = new NotificationCustomChange();
                    createTask.SubjectEmail = txtAccount.Text + " - Equip ID " + hdnEquipment.Value + "Test Type " + ddlTestTypes.SelectedItem.Text + " Alert";
                    createTask.UserName = Session["username"].ToString();
                    createTask.label = lblCustom.Text;
                    createTask.EquipmentName = txtUnit.Text;
                    createTask.EquipmentDesc = txtEquipmentDesc.Value;

                    List<String> ls = hdSelectTeam.Value.ToString().Split(';').ToList();

                    foreach (string item in ls)
                    {
                        string[] arr = item.Split('_');
                        if (arr.Length > 2)
                        {
                            if (arr[2] == "1")
                            {

                                if (arr[0] == "6")
                                {
                                    createTask.lsRole = createTask.lsRole + ";" + arr[0] + "_" + arr[1];
                                }
                                else
                                {
                                    createTask.lsTeamMember = createTask.lsTeamMember + ";" + arr[0] + "_" + arr[1];

                                }

                            }
                            else
                            {
                                if (chkSelectAlert.Checked)
                                {
                                    if (arr[0] == "6")
                                    {
                                        notification.lsRole = notification.lsRole + ";" + arr[0] + "_" + arr[1];
                                    }
                                    else
                                    {
                                        notification.lsTeamMember = notification.lsTeamMember + ";" + arr[0] + "_" + arr[1];

                                    }

                                }
                            }
                        }
                        else
                        {
                            if (chkSelectAlert.Checked && arr.Length > 1)
                            {
                                if (arr[0] == "6")
                                {
                                    notification.lsRole = notification.lsRole + ";" + arr[0] + "_" + arr[1];
                                }
                                else
                                {
                                    notification.lsTeamMember = notification.lsTeamMember + ";" + arr[0] + "_" + arr[1];

                                }

                            }
                        }

                    }

                    obj.Cus_CreateTask.Add(createTask);
                    obj.Cus_EmailToTeamMember.Add(notification);
                }

            }
        }
        obj.Cus_TestItemValue = dtCustomValue;
    }

    private void processCreateTask(SafetyTest obj)
    {

        DataSet ds = new DataSet();
        BusinessEntity.User objProp_User = new BusinessEntity.User();
        DataTable lstProjectTeamMember = (DataTable)ViewState["AllProjectTeamMemberList"];

        foreach (NotificationCustomChange item in obj.Cus_CreateTask)
        {
            if (!String.IsNullOrEmpty(item.lsTeamMember))
            {
                List<String> ls = item.lsTeamMember.Substring(1).Split(';').ToList();
                foreach (string objUser in ls)
                {
                    var changedItem = lstProjectTeamMember.Select("memberkey='" + objUser + "'").FirstOrDefault();
                    if (changedItem != null)
                    {
                        CreateTaskOnWorkflowChange(item.SubjectEmail, item.EmailContent(), (string)changedItem["fUser"], (string)changedItem["email"]);
                    }

                }
            }



            if (!String.IsNullOrEmpty(item.lsRole))
            {
                List<String> ls = item.lsRole.Substring(1).Split(';').ToList();
                foreach (string objUser in ls)
                {
                    var changedItem = lstProjectTeamMember.Select("memberkey='" + objUser + "'").FirstOrDefault();
                    if (changedItem != null)
                    {
                        var role = changedItem["RoleName"].ToString();
                        var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "'").ToList();
                        foreach (var projUser in projTeamUsers)
                        {
                            CreateTaskOnWorkflowChange(item.SubjectEmail, item.EmailContent(), (string)projUser["fUser"], (string)projUser["email"]);
                        }
                    }

                }
            }

        }
    }

    private void processSendMail(SafetyTest obj)
    {
        List<String> lsEmail;
        List<String> ls;
        DataSet ds = new DataSet();
        BusinessEntity.User objProp_User = new BusinessEntity.User();
        DataTable lstProjectTeamMember = (DataTable)ViewState["AllProjectTeamMemberList"];

        foreach (NotificationCustomChange item in obj.Cus_EmailToTeamMember)
        {
            lsEmail = new List<string>();
            if (!String.IsNullOrEmpty(item.lsTeamMember))
            {
                ls = item.lsTeamMember.Substring(1).Split(';').ToList();
                foreach (string objUser in ls)
                {
                    var changedItem = lstProjectTeamMember.Select("memberkey='" + objUser + "'").FirstOrDefault();
                    lsEmail.Add((string)changedItem["email"]);
                }
            }



            if (!String.IsNullOrEmpty(item.lsRole))
            {
                ls = item.lsRole.Substring(1).Split(';').ToList();
                foreach (string objUser in ls)
                {
                    var changedItem = lstProjectTeamMember.Select("memberkey='" + objUser + "'").FirstOrDefault();
                    var role = changedItem["RoleName"].ToString();
                    var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "'").ToList();
                    foreach (var projUser in projTeamUsers)
                    {
                        lsEmail.Add((string)projUser["email"]);
                    }

                }
            }
            if (lsEmail.Count > 0)
            {
                Mail mail = new Mail();
                mail.From = WebBaseUtility.GetFromEmailAddress();
                mail.To = lsEmail;
                mail.Title = item.SubjectEmail;
                mail.Text = item.EmailContent();
                try
                {
                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                    mail.Send();
                }
                catch (Exception ex)
                {
                    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }


        }


    }
    #endregion


    private void FillTestPricing()
    {
        int equiId = hdnEquipment.Value == "" ? 0 : Convert.ToInt32(hdnEquipment.Value);
        //String equipmentClassification = txtClassification.Text;
        String equipmentClassification = ddlClassification.SelectedValue;
        int testtypeId = Convert.ToInt32(ddlTestTypes.SelectedValue);
        int testID = 0;
        if (Request.QueryString["LID"] != null)
        {
            testID = Convert.ToInt32(Request.QueryString["LID"]);
        }
        //Get Test Pricing by EquimentType and TestTypeId
        DataSet ds = new DataSet();
        ds = objBL_User.ValidateEquipmentTestPricing(Session["config"].ToString(), equipmentClassification, testtypeId);

        Double amount = 0;
        Double overrideAmount = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {

            amount = Convert.ToDouble(ds.Tables[0].Rows[0]["Amount"]);
            overrideAmount = Convert.ToDouble(ds.Tables[0].Rows[0]["Override"]);
        }

        txtAmount.Text = amount.ToString();
        txtOverrideAmount.Text = overrideAmount.ToString();

    }


    #region Proposal
    public void FillDocuments()
    {

        DataSet ds = new DataSet();
        BL_SafetyTest proposalTbl = new BL_SafetyTest();
        int TestId = hdntestid.Value == "" ? 0 : Convert.ToInt32(hdntestid.Value);
        ds = proposalTbl.GetProposalByTestID(Session["config"].ToString(), TestId);
        if (ds.Tables.Count > 0)
        {
            gvDocuments.VirtualItemCount = ds.Tables[0].Rows.Count;
            gvDocuments.DataSource = ds.Tables[0];
        }

    }
    protected void gvDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        FillDocuments();


    }
    protected void btnProcessDownload_Click(object sender, EventArgs e)
    {
        if (hdnDownloadID.Value.Trim() != "")
        {
            BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
            DataSet ds = bl_SafetyTest.GetProposalFormByID(Session["config"].ToString(), Convert.ToInt32(hdnDownloadID.Value));
            String type = ".docx";
            String path = ds.Tables[0].Rows[0]["FilePath"].ToString();


            if (ds.Tables.Count > 0)
            {
                if (hdnDownloadType.Value == "1")
                {
                    type = ".pdf";
                    path = ds.Tables[0].Rows[0]["PdfFilePath"].ToString();
                }
                DownloadDocument(path, ds.Tables[0].Rows[0]["FileName"].ToString() + type);
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

    #endregion

    #region GeneralProposal

    public void PopulateFields(TestSetupForm et, DataRow dr)
    {
        et.Id = Convert.ToInt32(dr["ID"]);
        et.Name = dr["Name"].ToString();
        et.FileName = dr["FileName"].ToString();
        et.FilePath = dr["FilePath"].ToString();
        et.MIME = dr["MIME"].ToString();
        et.Type = Convert.ToInt32(dr["Type"]);
        et.AddedBy = dr["AddedBy"].ToString();
        et.UpdatedBy = dr["UpdatedBy"].ToString();
        if (dr["UpdatedOn"].ToString() != "")
        {
            et.UpdatedOn = Convert.ToDateTime(dr["UpdatedOn"]);
        }
        if (dr["AddedOn"].ToString() != "")
        {
            et.AddedOn = Convert.ToDateTime(dr["AddedOn"]);
        }

    }


    public void doGenerateProposalTemplate(TestProposalDetail testProposal, TestSetupForm formPro)
    {

        DateTime fromDate;
        DateTime toDate;
        try
        {
            if (testProposal.ProposalEquipment.Count > 0)
            {

                if (drpdwnTestDueBy.SelectedValue == "1")
                {
                    fromDate = txtLastDueDate.Text == "" ? Convert.ToDateTime(txtLasttestdon.Text) : Convert.ToDateTime(txtLastDueDate.Text);
                }
                else
                {
                    fromDate = txtLasttestdon.Text == "" ? Convert.ToDateTime(txtLastDueDate.Text) : Convert.ToDateTime(txtLasttestdon.Text);
                }
                toDate = Convert.ToDateTime(txtTestNextDueDate.Text);

                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + "\\" + Session["dbname"] + "\\SafetyTest\\";
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                string guid = System.Guid.NewGuid().ToString();
                string pathDoc = String.Empty;
                string pathPDF = String.Empty;

                //get company information
                DataTable dtCompany = GetCompanyDetails();
                string CompanyName = dtCompany.Rows[0]["Name"].ToString();
                string CompanyAddress = dtCompany.Rows[0]["Address"].ToString();
                string CompanyPhoneNumber = dtCompany.Rows[0]["Phone"].ToString();
                string CompanyFax = dtCompany.Rows[0]["Fax"].ToString();
                string CompanyEmail = dtCompany.Rows[0]["Email"].ToString();
                string CompanyCity = dtCompany.Rows[0]["City"].ToString();
                string CompanyState = dtCompany.Rows[0]["State"].ToString();
                string CompanyZip = dtCompany.Rows[0]["Zip"].ToString();

                //get location information
                DataTable dtLoc = testProposal.LocationInfo;
                String LocName = dtLoc.Rows[0]["Tag"].ToString();
                String LocAddress = dtLoc.Rows[0]["locAddress"].ToString();
                String mainContact = dtLoc.Rows[0]["Contact"].ToString();
                String LocationTag = dtLoc.Rows[0]["tag"].ToString();

                //get customer information
                String customername = dtLoc.Rows[0]["CustomerName"].ToString();
                String customerAddress = dtLoc.Rows[0]["CustomerAddress"].ToString();
                String customerCity = dtLoc.Rows[0]["CustomerCity"].ToString();
                String customerState = dtLoc.Rows[0]["CustomerState"].ToString();
                String customerZip = dtLoc.Rows[0]["CustomerZip"].ToString();

                Boolean ThirdParty = false;
                String ThirdPartyName = "";

                String ClassificationName = "";
                String Remark = "";
                String TestType = "";


                ClassificationName = testProposal.Classification;
                Remark = testProposal.Remark == null ? "" : testProposal.Remark;
                TestType = testProposal.TestType == null ? "" : testProposal.TestType;
                //CreateFile
                String proposalFileName = LocName.Replace(" ", "").Replace("\\", "").Replace("/", "").Replace("*", "") + "_" + ClassificationName + DateTime.Now.ToString("MMddyyyyhhmmssfff");
                proposalFileName = proposalFileName.Replace(" ", "").Replace("\\", "").Replace("/", "").Replace("*", "").Replace(",", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace(":", "");
                pathDoc = savepath + proposalFileName + "." + formPro.MIME;
                pathPDF = savepath + proposalFileName + "." + "pdf";

                File.Copy(formPro.FilePath, pathDoc);

                using (DocX document = DocX.Load(pathDoc))
                {
                    document.ReplaceText("{LocationTag}", LocationTag, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{MainContract}", mainContact, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CustomerName}", customername, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CustomerName}", customername, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CustomerAddress}", customerAddress, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CustomerCity}", customerCity, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CustomerState}", customerState, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CustomerZip}", customerZip, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{Location}", LocName, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{LocationAddress}", LocAddress, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{Year}", testProposal.YearProposal.ToString(), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{FromDate}", fromDate.ToString("MM/dd/yyyy hh:mm"), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{ToDate}", toDate.ToString("MM/dd/yyyy hh:mm"), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{ProposalDate}", DateTime.Now.ToString("MMM dd, yyyy"), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{LocationCity}", dtLoc.Rows[0]["locCity"].ToString(), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{LocationState}", dtLoc.Rows[0]["locState"].ToString(), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{LocationZip}", dtLoc.Rows[0]["locZip"].ToString(), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{Phone}", dtLoc.Rows[0]["Phone"].ToString(), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{Mobile}", dtLoc.Rows[0]["Cellular"].ToString(), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{Classification}", ClassificationName, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{Remark}", Remark, false, RegexOptions.IgnoreCase);

                    //Company information
                    document.ReplaceText("{CompanyName}", CompanyName, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CompanyAddress}", CompanyAddress, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CompanyPhoneNumber}", CompanyPhoneNumber, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CompanyFax}", CompanyFax, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CompanyEmail}", CompanyEmail, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CompanyCity}", CompanyCity, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CompanyState}", CompanyState, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{CompanyZip}", CompanyZip, false, RegexOptions.IgnoreCase);

                    //Location Billing Information
                    document.ReplaceText("{LocationBillingAddress}", dtLoc.Rows[0]["Address"].ToString(), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{LocationBillingCity}", dtLoc.Rows[0]["City"].ToString(), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{LocationBillingState}", dtLoc.Rows[0]["State"].ToString(), false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{LocationBillingZip}", dtLoc.Rows[0]["Zip"].ToString(), false, RegexOptions.IgnoreCase);


                    if (hdnIsParentTestType.Value == "1")
                    {
                        if (hdnHasChildTest.Value == "1")
                        {
                            document.ReplaceText("{TestType}", ddlTestTypes.SelectedItem.ToString(), false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TestTypeCoverNote}", hdnTestTypeChildName.Value + " and " + ddlTestTypes.SelectedItem.ToString() + " inspections", false, RegexOptions.IgnoreCase);

                        }
                        else
                        {
                            document.ReplaceText("{TestType}", ddlTestTypes.SelectedItem.ToString(), false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TestTypeCoverNote}", ddlTestTypes.SelectedItem.ToString(), false, RegexOptions.IgnoreCase);
                        }
                    }
                    else
                    {
                        if (hdnHasParentTest.Value == "1")
                        {
                            document.ReplaceText("{TestType}", testProposal.CoveredByTestTypeName, false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TestTypeCoverNote}", ddlTestTypes.SelectedItem.ToString() + " and " + hdnTestTypeParentName.Value + " inspections", false, RegexOptions.IgnoreCase);
                        }
                        else
                        {
                            document.ReplaceText("{TestType}", ddlTestTypes.SelectedItem.ToString(), false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TestTypeCoverNote}", hdnTestTypeChildName.Value + " and " + ddlTestTypes.SelectedItem.ToString() + " inspections", false, RegexOptions.IgnoreCase);
                        }
                    }

                    List<String> lsEquipID = new List<string>();
                    StringBuilder strContent = new StringBuilder();

                    var rowCount = testProposal.ProposalEquipment.Count / 3;
                    if (testProposal.ProposalEquipment.Count % 3 > 0)
                    {
                        rowCount = rowCount + 1;
                    }
                    Table t = document.AddTable(rowCount, 3);
                    // Specify some properties for this Table.
                    t.Alignment = Alignment.center;
                    t.SetColumnWidth(0, 3000);
                    t.SetColumnWidth(1, 3000);
                    t.SetColumnWidth(2, 3000);
                    Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 0, Color.Transparent);
                    //t.Paragraphs[0].Font(new FontFamily("Times New Roman"));
                    int n = 0;
                    int m = 0;
                    //List<ProposalPrice> lsPrice = new List<ProposalPrice>();
                    //ProposalPrice objPrice;
                    List<Double> lsPrice = new List<double>();

                    foreach (ProposalEquipment itemEquip in testProposal.ProposalEquipment)
                    {
                        t.Rows[n].Cells[m].Paragraphs.First().Append(itemEquip.unit);
                        t.Rows[n].Cells[m].Paragraphs[0].Font(new FontFamily("Times New Roman"));
                        t.Rows[n].Cells[m].Paragraphs[0].FontSize(12);
                        m = m + 1;
                        if (m == 3)
                        {
                            m = 0;
                            n = n + 1;
                        }

                        lsEquipID.Add(itemEquip.ID.ToString());
                        lsPrice.Add(itemEquip.Amount);
                        if (itemEquip.ThirdPartyRequired == true)
                        {
                            ThirdParty = true;
                            ThirdPartyName = itemEquip.ThirdPartyName;

                        }


                    }
                    Border b = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 0, Color.Transparent);
                    t.SetBorder(TableBorderType.InsideH, b);
                    t.SetBorder(TableBorderType.InsideV, b);
                    t.SetBorder(TableBorderType.Bottom, b);
                    t.SetBorder(TableBorderType.Top, b);
                    t.SetBorder(TableBorderType.Left, b);
                    t.SetBorder(TableBorderType.Right, b);

                    // Insert the Table into the document.
                    try
                    {
                        foreach (var paragraph in document.Paragraphs)
                        {
                            paragraph.FindAll("{UnitNumber}").ForEach(index => paragraph.InsertTableAfterSelf((t)));
                        }
                        document.ReplaceText("{UnitNumber}", "", false, RegexOptions.IgnoreCase);
                    }
                    catch (Exception)
                    {
                        document.InsertTable(t);
                    }
                    var groupPrice = lsPrice
                     .GroupBy(i => i) //Group the words
                     .Select(i => new { Amount = i.Key, Count = i.Count() });

                    String ReguiredInformation = "";
                    String ThridPartyRequired = "";
                    String ThirdPartyContact = "";
                    String ThirdPartyWarning = "";
                    String ThirdPartyNameContent = "";
                    String ThirdPartyMsg = "Kindly sign and return to us immediately so we may schedule your inspection accordingly";
                    if (ThirdParty == true)
                    {
                        ThridPartyRequired = "This inspection requires your hiring a Private Elevator Inspection Agency to witness this inspection";
                        ThirdPartyNameContent = "Name of 3rd  Party Company:" + ThirdPartyName;
                        ThirdPartyContact = "Contact Person:_________________";
                        ThirdPartyWarning = "**WE CAN NOT ACCEPT PROPOSAL, IF THE THIRD PARTY INFORMATION IS NOT PROVIDED";
                        ThirdPartyMsg = "Kindly sign and provide your Third Party Witnessing Company information below and return to us immediately so we may schedule your inspection accordingly.";
                        ReguiredInformation = "** REQUIRED INFORMATION:";
                        document.ReplaceText("{ThirdpartyName}", ThirdPartyName, false, RegexOptions.IgnoreCase);
                    }
                    document.ReplaceText("{ThridPartyRequired}", ThridPartyRequired, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{ThirdPartyName}", ThirdPartyNameContent, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{ThirdPartyContact}", ThirdPartyContact, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{ThirdPartyWarning}", ThirdPartyWarning, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{ThirdPartyMsg}", ThirdPartyMsg, false, RegexOptions.IgnoreCase);
                    document.ReplaceText("{ReguiredInformation}", ReguiredInformation, false, RegexOptions.IgnoreCase);
                    if (chkChargeableEdit.Checked)
                    {
                        //Our proposal is based on providing a team to perform the test for a three (3) hour period, PER ELEVATOR.
                        //Should this inspection exceed the three(3) hour period per elevator, our standard billing rates will apply.

                        String message = "Our proposal is based on providing a team to perform the test for " + testProposal.DefaultHour.ToString() + " hour period, PER " + ClassificationName + ". Should this inspection exceed the ";
                        message += testProposal.DefaultHour.ToString() + " hour inspection period, PER " + ClassificationName + " our standard billing rates will apply.";

                        String strPrice = "PRICE: ";
                        Double totalAmount = 0;
                        foreach (var item in groupPrice)
                        {
                            if (groupPrice.Count() == 1)
                            {
                                strPrice = "PRICE: " + new BL_Utility().ConvertToWords(item.Amount.ToString()) + ", " + (item.Amount).ToString("C") + " PER " + ClassificationName + ", for a total of:" + (item.Amount * item.Count).ToString("C");
                                totalAmount = item.Amount;
                            }
                            else
                            {
                                strPrice += Environment.NewLine + "-" + new BL_Utility().ConvertToWords(item.Amount.ToString()) + ", " + (item.Amount).ToString("C") + " PER " + ClassificationName + ", for a total of:" + (item.Amount * item.Count).ToString("C");
                                totalAmount += item.Amount;
                            }
                        }
                        document.ReplaceText("{chargeableMessage}", message, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{TotalAmount}", totalAmount.ToString("C"), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{TestPricing}", strPrice.ToString(), false, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        if (hdnParentTestID.Value != "0")
                        {
                            String message = "Our proposal is based on providing a team to perform the test for " + testProposal.DefaultHour.ToString() + " hour period, PER " + ClassificationName + ". Should this inspection exceed the ";
                            message += testProposal.DefaultHour.ToString() + " hour inspection period, PER " + ClassificationName + " our standard billing rates will apply.";

                            String strPrice = "PRICE: ";
                            Double totalAmount = 0;
                            foreach (var item in groupPrice)
                            {
                                if (groupPrice.Count() == 1)
                                {
                                    strPrice = "PRICE: " + new BL_Utility().ConvertToWords(item.Amount.ToString()) + ", " + (item.Amount).ToString("C") + " PER " + ClassificationName + ", for a total of:" + (item.Amount * item.Count).ToString("C");
                                    totalAmount = item.Amount;
                                }
                                else
                                {
                                    strPrice += Environment.NewLine + "-" + new BL_Utility().ConvertToWords(item.Amount.ToString()) + ", " + (item.Amount).ToString("C") + " PER " + ClassificationName + ", for a total of:" + (item.Amount * item.Count).ToString("C");
                                    totalAmount += item.Amount;
                                }
                            }
                            //strPrice = "PRICE: " + new BL_Utility().ConvertToWords(testProposal.DefaultAmount.ToString()) + ", " + (testProposal.DefaultAmount.ToString("C")) + " PER " + ClassificationName + ", for a total of:" + (testProposal.DefaultAmount*proEquimentType.lsEquiment.Count).ToString("C");
                            document.ReplaceText("{chargeableMessage}", message, false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TestPricing}", strPrice.ToString(), false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TotalAmount}", totalAmount.ToString("C"), false, RegexOptions.IgnoreCase);
                        }
                        else
                        {
                            //This inspection is covered under the terms of your maintenance service agreement with TEI Group. However,
                            //should this inspection exceed the proposed({ hour }) hour inspection period, PER { Classification}, our standard billing rates will apply.

                            String message = "This inspection is covered under the terms of your maintenance service agreement with TEI Group. However, should this inspection exceed the proposed ";
                            message += testProposal.DefaultHour.ToString() + " hour inspection period, PER " + ClassificationName + " our standard billing rates will apply.";
                            document.ReplaceText("{chargeableMessage}", message, false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TestPricing}", "0.00", false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TotalAmount}", "0.00", false, RegexOptions.IgnoreCase);
                        }

                    }
                    #region "Save Proposal to DB"

                    ProposalForm objForm = new ProposalForm();
                    objForm.FileName = proposalFileName;
                    objForm.FilePath = pathDoc;
                    objForm.PdfFilePath = pathPDF;

                    objForm.FromDate = fromDate;
                    objForm.ToDate = toDate;
                    objForm.AddedBy = Session["username"].ToString();
                    objForm.LocID = Convert.ToInt32(dtLoc.Rows[0]["Loc"]);
                    objForm.Classification = ClassificationName;
                    objForm.Type = 2;
                    objForm.Status = "Pending";//Sold, Declined, Pending
                    objForm.AlertEmail = "";
                    objForm.ListEquipment = String.Join(",", lsEquipID);
                    objForm.ConnConfig = Session["config"].ToString();
                    objForm.Chargable = chkChargeableEdit.Checked;
                    objForm.YearProposal = testProposal.YearProposal;

                    if (hdnIsParentTestType.Value == "1")
                    {
                        objForm.TestTypeID = Convert.ToInt32(ddlTestTypes.SelectedValue);

                    }
                    else
                    {
                        if (hdnHasParentTest.Value == "1")
                        {
                            objForm.TestTypeID = Convert.ToInt32(hdnTestTypeParentID.Value);
                        }
                        else
                        {
                            objForm.TestTypeID = Convert.ToInt32(ddlTestTypes.SelectedValue);
                        }
                    }
                    BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
                    int id = bl_SafetyTest.CreateProposalForm(objForm);
                    #endregion

                    //Replace Estimate No with Proposal ID for now.
                    document.ReplaceText("{ProposalID}", id.ToString());
                    document.Save();

                    #region Convert Docx file into PDF
                    //Free version of Spire.Doc has limitations of first three pages more details at https://www.e-iceblue.com/Introduce/free-doc-component.html
                    Spire.Doc.Document doc = new Spire.Doc.Document();
                    doc.LoadFromFile(pathDoc);

                    doc.SaveToFile(pathDoc, Spire.Doc.FileFormat.Docx);
                    doc.SaveToFile(pathPDF, Spire.Doc.FileFormat.PDF);
                    #endregion

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Generated proposal successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'This equipment already create proposal', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddProposal", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private TestProposalDetail getDataToGeneralProposalTemplate(int proposalYear)
    {


        TestProposalDetail objTestProposal = new TestProposalDetail();
        List<ProposalEquipment> lsProposalEquipment = new List<ProposalEquipment>();
        try
        {

            //Double defaultHour = 0;
            //Load Pricing
            // String equipmentClassification = txtClassification.Text;
            String equipmentClassification = ddlClassification.SelectedValue;
            int testtypeId = Convert.ToInt32(ddlTestTypes.SelectedValue);

            //Get Test Pricing by Classification and TestTypeId
            //Apply for old data  
            objTestProposal.Remark = "";
            objTestProposal.DefaultHour = 0;
            objTestProposal.DefaultAmount = 0;
            objTestProposal.TestType = "";
            objTestProposal.TestTypeCoverName = "";
            objTestProposal.CoveredByTestTypeName = "";
            objTestProposal.TestTypeID = testtypeId;
            // objTestProposal.Classification= txtClassification.Text;
            objTestProposal.Classification = ddlClassification.SelectedValue;
            objTestProposal.YearProposal = Convert.ToInt32(ddlYear.SelectedValue);
            BL_User objBL = new BL_User();
            DataSet dsPricing = new DataSet();
            BusinessEntity.User objProp_User = new BusinessEntity.User();
            // dsPricing = objBL.ValidateEquipmentTestPricing(Session["config"].ToString(), equipmentClassification, testtypeId, proposalYear);
            dsPricing = objBL.GetDefaultTestPricingForEquipment(Session["config"].ToString(), Convert.ToInt32(Request.QueryString["elv"]), testtypeId, proposalYear);

            if (dsPricing != null)
            {
                if (dsPricing.Tables[0].Rows.Count > 0)
                {
                    objTestProposal.Remark = dsPricing.Tables[0].Rows[0]["Remarks"].ToString();
                    objTestProposal.DefaultHour = Double.Parse(dsPricing.Tables[0].Rows[0]["DefaultHour"].ToString());
                    objTestProposal.DefaultAmount = Double.Parse(dsPricing.Tables[0].Rows[0]["Amount"].ToString());
                    objTestProposal.TestType = dsPricing.Tables[0].Rows[0]["TestTypeName"].ToString();
                    objTestProposal.TestTypeCoverName = dsPricing.Tables[0].Rows[0]["TestTypeCoverName"].ToString();
                    objTestProposal.CoveredByTestTypeName = dsPricing.Tables[0].Rows[0]["CoveredByTestTypeName"].ToString();
                }
            }

            int loc = hdnaccount.Value.ToString() == "" ? 0 : Convert.ToInt32(hdnaccount.Value);
            //get Location info
            BL_User objBL_User = new BL_User();
            User objPropUser = new User();
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.LocID = loc;

            DataSet dsLoc = objBL_User.getLocationByID(objPropUser);
            if (dsLoc.Tables.Count > 0)
            {
                objTestProposal.LocationInfo = dsLoc.Tables[0];
            }
            //Classification

            ProposalEquipment proposalEquipment = new ProposalEquipment();
            //proposalEquipment.Classification= txtClassification.Text;
            proposalEquipment.Classification = ddlClassification.SelectedValue;
            DataSet dsEquipment = new DataSet();
            if (hdnIsParentTestType.Value == "1")
            {
                if (hdnHasChildTest.Value == "1")
                {
                    //dsEquipment = objBL_User.GetAllEquipmentHaveSameTestCover(Session["config"].ToString(), objPropUser.LocID, Convert.ToInt32(testtypeId),Convert.ToInt32( ddlYear.SelectedValue), Convert.ToBoolean(chkChargeableEdit.Checked), txtClassification.Text);
                    dsEquipment = objBL_User.GetExistTestCoverInLocByTestType(Session["config"].ToString(), objPropUser.LocID, Convert.ToInt32(testtypeId), Convert.ToInt32(ddlYear.SelectedValue), Convert.ToBoolean(chkChargeableEdit.Checked), ddlClassification.SelectedValue);
                }
                else
                {
                    dsEquipment = objBL_User.GetExistTestInLocByTestTypeAndChargable(Session["config"].ToString(), loc, Convert.ToInt32(ddlTestTypes.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue), chkChargeableEdit.Checked, ddlClassification.SelectedValue);
                }
            }
            else
            {
                if (hdnHasParentTest.Value == "1")
                {
                    //dsEquipment = objBL_User.GetAllEquipmentHaveSameTestCover(Session["config"].ToString(), objPropUser.LocID, Convert.ToInt32(hdnTestTypeParentID.Value), Convert.ToInt32(ddlYear.SelectedValue), Convert.ToBoolean(hdnTestTypeParentChargable.Value), txtClassification.Text);
                    dsEquipment = objBL_User.GetExistTestCoverInLocByTestType(Session["config"].ToString(), objPropUser.LocID, Convert.ToInt32(hdnTestTypeParentID.Value), Convert.ToInt32(ddlYear.SelectedValue), Convert.ToBoolean(hdnTestTypeParentChargable.Value), ddlClassification.SelectedValue);
                }
                else
                {
                    //dsEquipment = objBL_User.GetAllEquipmentHaveSameTestChargable(Session["config"].ToString(), objPropUser.LocID, Convert.ToInt32(testtypeId), Convert.ToInt32(ddlYear.SelectedValue), Convert.ToBoolean(chkChargeableEdit.Checked), txtClassification.Text);
                    dsEquipment = objBL_User.GetExistTestInLocByTestTypeAndChargable(Session["config"].ToString(), objPropUser.LocID, Convert.ToInt32(testtypeId), Convert.ToInt32(ddlYear.SelectedValue), Convert.ToBoolean(chkChargeableEdit.Checked), ddlClassification.SelectedValue);
                }
            }

            List<ProposalEquipment> lsProEquiments = new List<ProposalEquipment>();

            DataTable dtEquipment = new DataTable();
            if (dsEquipment.Tables.Count > 0)
            {
                dtEquipment = dsEquipment.Tables[0];

            }
            proposalEquipment = new ProposalEquipment();
            foreach (DataRow rowEqui in dtEquipment.Rows)
            {
                proposalEquipment = new ProposalEquipment();
                proposalEquipment.TestID = Convert.ToInt32(rowEqui["LID"].ToString());
                proposalEquipment.ID = Convert.ToInt32(rowEqui["ID"].ToString());
                proposalEquipment.unit = rowEqui["Unit"].ToString();
                proposalEquipment.Classification = rowEqui["Classification"].ToString();
                proposalEquipment.Amount = Convert.ToDouble(rowEqui["Amount"]);
                if (Convert.ToDouble(rowEqui["OverrideAmount"]) != 0)
                {
                    proposalEquipment.Amount = Convert.ToDouble(rowEqui["OverrideAmount"]);
                }
                proposalEquipment.OverrideAmount = Convert.ToDouble(rowEqui["OverrideAmount"]);
                proposalEquipment.Chargeable = Convert.ToBoolean(rowEqui["Chargeable"]);
                proposalEquipment.ThirdPartyName = rowEqui["ThirdPartyName"].ToString();
                proposalEquipment.ThirdPartyPhone = rowEqui["ThirdPartyPhone"].ToString();
                proposalEquipment.ThirdPartyRequired = Convert.ToBoolean(rowEqui["ThirdPartyRequired"]);
                lsProposalEquipment.Add(proposalEquipment);
            }
            objTestProposal.ProposalEquipment = lsProposalEquipment;

        }
        catch (Exception ex)
        {
            return null;
        }

        return objTestProposal;
    }

    #endregion

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {

        List<BusinessEntity.MailSender> tableList = new List<BusinessEntity.MailSender>();

        BL_SafetyTest proposalTbl = new BL_SafetyTest();
        foreach (GridDataItem item in gvDocuments.SelectedItems)
        {
            HiddenField hdID = item.FindControl("g_ProposalID") as HiddenField;
            DataSet ds = new DataSet();
            ds = proposalTbl.GetProposalFormByID(Session["config"].ToString(), Convert.ToInt32(hdID.Value));


            if (ds.Tables.Count > 0)
            {
                #region Data
                BusinessEntity.MailSender data = new MailSender();
                data.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                data.Name = Convert.ToString(ds.Tables[0].Rows[0]["FileName"]);
                String fileName = Convert.ToString(ds.Tables[0].Rows[0]["FileName"]);
                String addedOn = Convert.ToString(ds.Tables[0].Rows[0]["AddedOn"]);

                data.FileName = fileName;
                data.PDFFilePath = Convert.ToString(ds.Tables[0].Rows[0]["PdfFilePath"]);
                tableList.Add(data);
                #endregion
            }

        }

        if (tableList == null || tableList.Count <= 0)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddLoctype", "noty({text: 'Please select a row.', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
        else
        {
            DataSet dsContact = getContactData();
            List<String> lsEmail = new List<string>();
            if (dsContact.Tables.Count > 0)
            {
                DataRow[] result = dsContact.Tables[1].Select("EmailRecTestProp =true");

                foreach (DataRow row in result)
                {
                    if (!lsEmail.Contains(row["Email"].ToString()))
                    {
                        lsEmail.Add(row["Email"].ToString());
                    }

                }

            }
            Session["SelectedProposalTemplate"] = null;
            Session["SelectedProposalTemplate"] = tableList;

            Session["SelectedProposal_Contact"] = string.Join(",", lsEmail.ToArray());

            Response.Redirect("EmailSenderProposal.aspx?elv=" + Request.QueryString["elv"] + "&LID=" + Request.QueryString["LID"] + "&loc=" + hdnaccount.Value);
        }
    }

    protected void lnkDeleteForms_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in gvDocuments.SelectedItems)
        {
            HiddenField hdID = (HiddenField)item.FindControl("g_ProposalID");
            BL_SafetyTest proposalTbl = new BL_SafetyTest();
            proposalTbl.DeleteProposalForm(Session["config"].ToString(), Convert.ToInt32(hdID.Value));

        }
        FillDocuments();
        gvDocuments.Rebind();
    }

    protected void btnGenerateTemplete_Click(object sender, EventArgs e)
    {
        //GET all Test 

        TestSetupForm formPro = new TestSetupForm();
        foreach (GridDataItem item in gvFormTemplate.SelectedItems)
        {
            HiddenField hdID = (HiddenField)item.FindControl("hdnFormID");
            BL_SafetyTest proposalTbl = new BL_SafetyTest();
            DataSet ds = proposalTbl.GetTestSetupFormsById(Session["config"].ToString(), Convert.ToInt32(hdID.Value));
            PopulateFields(formPro, ds.Tables[0].Rows[0]);
        }

        if (formPro != null)
        {
            TestProposalDetail testProposal = getDataToGeneralProposalTemplate(Convert.ToInt32(ddlYear.SelectedValue));
            doGenerateProposalTemplate(testProposal, formPro);
            FillDocuments();
            gvDocuments.Rebind();
        }
        else
        {

            string str = "You do not have Form. Please add Form in Setup page";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddLoctype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
    }

    protected void gvFormTemplate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        FillTestSetupForms();
    }
    private void FillTestSetupForms()
    {

        DataSet ds = new DataSet();
        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
        ds = bl_SafetyTest.GetAllTestSetupForms(Session["config"].ToString());

        gvFormTemplate.VirtualItemCount = ds.Tables[0].Rows.Count;
        gvFormTemplate.DataSource = ds.Tables[0];


    }
    private void FillProposalYear()
    {
        ddlYear.Items.Clear();
        //Bind year
        if (ViewState["ProposalYear"] != null)
        {
            DataTable dtYear = (DataTable)ViewState["ProposalYear"];
            foreach (DataRow row in dtYear.Rows)
            {
                var item = new ListItem(row["PriceYear"].ToString(), row["PriceYear"].ToString());
                ddlYear.Items.Add(item);

            }
        }
    }
    protected void gvDocuments_ItemDataBound(object sender, GridItemEventArgs e)
    {
        foreach (GridDataItem gvRow in gvDocuments.Items)
        {
            DropDownList ddlStatus = gvRow.FindControl("ddlStatusDocument") as DropDownList;
            HiddenField hdnID = gvRow.FindControl("g_ProposalID") as HiddenField;

            Label lblStatus = (Label)gvRow.FindControl("lblStatus");
            if (ddlStatus != null)
            {
                ddlStatus.SelectedValue = lblStatus.Text;
                ddlStatus.Attributes.Add("ProposalID", hdnID.Value);

            }
        }
    }

    protected void ddlStatusDocument_SelectedIndexChanged(object sender, EventArgs e)
    {

        String selectValue = ((DropDownList)sender).SelectedValue;
        int id = Convert.ToInt32(((DropDownList)sender).Attributes["ProposalID"]);
        BL_SafetyTest obj = new BL_SafetyTest();
        try
        {
            obj.UpdateStatusProposalForm(Session["config"].ToString(), id,
                      selectValue, Session["username"].ToString());


            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Document updated status successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillDocuments();
            gvDocuments.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
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
    private void InitTeamMemberGridView()
    {
        User objPropUser = new User();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        //objPropUser.Status = 0;
        // ds = objBL_User.GetUsersForTeamMemberList(objPropUser);
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
                _dr["roleName"] = "";
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


    #region logs

    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        loadLog();
    }
    private void loadLog()
    {
        try
        {
            RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
            if (Request.QueryString["LID"] != null)
            {

                BL_SafetyTest objtestbl = new BL_SafetyTest();
                SafetyTest objproptest = new SafetyTest();
                objproptest.ConnConfig = Session["config"].ToString();
                objproptest.LID = Convert.ToInt32(Request.QueryString["LID"]);


                DataSet dsLog = new DataSet();

                int TestYear = 0;


                if (Request.QueryString["tyear"] != null)
                {
                    TestYear = Convert.ToInt32(Request.QueryString["tyear"]);
                }
                // dsLog = objtestbl.GetTestLogs(objproptest);

                dsLog = objtestbl.GetTestLogsByYear(objproptest, TestYear);

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

            if (totalCount == 0)
                totalCount = 1000;

            GeneralFunctions obj = new GeneralFunctions();

            var sizes = obj.TelerikPageSize(totalCount);

            dropDown.Items.Clear();

            foreach (var size in sizes)
            {
                var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                if (e.Item.OwnerTableView.PageSize.ToString() == size.Value)
                    cboItem.Selected = true;
                dropDown.Items.Add(cboItem);
            }
        }
    }
    #endregion

    private void CreateTaskOnWorkflowChange(string strSubject, string strRemarks, string assignedTo, string strMailTo = "")
    {
        var objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.ROL = Convert.ToInt32(hdnRolId.Value);
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
            objCustomer.Screen = "Equipment";

            objCustomer.Ref = Convert.ToInt32(hdnEquipment.Value);
            objBL_Customer.AddTask(objCustomer);

            #region Thomas: Send email with a appointment to login user 
            if (objCustomer.IsAlert)
            {
                // Create
                BusinessEntity.User objPropUser = new BusinessEntity.User();
                BL_User objBL_User = new BL_User();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Username = assignedTo;

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
                        mail.Title = txtAccount.Text + ": " + objCustomer.Subject;

                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendFormat("Dear {0}<br><br>", objCustomer.AssignedTo);
                        stringBuilder.Append("You are receiving an appointment task from MOM-->Sales-->Tasks<br><br>");
                        // stringBuilder.AppendFormat("Customer Name: {0}<br>", txtCustomer.Text);
                        stringBuilder.AppendFormat("Location Name: {0}<br>", txtAccount.Text);

                        stringBuilder.AppendFormat("Subject: {0}<br>", objCustomer.Subject);
                        stringBuilder.AppendFormat("Description: {0}<br>", objCustomer.Remarks);
                        stringBuilder.AppendFormat("Due on: {0} {1}<br><br>", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        stringBuilder.Append("Attached files is a task appointment assigned to you.<br>");
                        stringBuilder.Append("To add this appointment to your calendar, please open and save it<br><br>");
                        stringBuilder.AppendFormat("<a href={0}>{0}</a><br><br>", uri);
                        stringBuilder.Append("Thanks");

                        mail.Text = stringBuilder.ToString();

                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                        var apSubject = string.Format("Task name: {0}", txtAccount.Text);

                        StringBuilder apBody = new StringBuilder();
                        var _strRemarks = objCustomer.Remarks.Replace("\r\n", "=0D=0A").Replace("\n", "=0D=0A");
                        apBody.AppendFormat("{0}.=0D=0A", _strRemarks);
                        apBody.AppendFormat("Due on: {0} {1}. =0D=0A ", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        apBody.Append("Attached files is a task appointment assigned to you.  =0D=0A");
                        apBody.Append("To add this appointment to your calendar, please open and save it.=0D=0A");
                        apBody.Append("Thanks");


                        var strStartDate = string.Format("{0} {1}", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        var apStart = Convert.ToDateTime(strStartDate);
                        var apEnd = apStart.AddHours(objCustomer.Duration);

                        var icsAttachmentContentsStr = WebBaseUtility.CreateICSAttachmentCalendarStr(apSubject
                            , apBody.ToString()
                            , txtAccount.Text
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

    private void BindPriceHistory()
    {
        try
        {
            if (Request.QueryString["LID"] != null)
            {
                SafetyTest objproptest = new SafetyTest();
                BL_SafetyTest objtestbl = new BL_SafetyTest();
                objproptest.ConnConfig = WebBaseUtility.ConnectionString;
                objproptest.LID = Convert.ToInt32(Request.QueryString["LID"]);
                DataSet ds = objtestbl.GetPriceHistory(objproptest);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        RadGrid_PriceHistory.DataSource = ds.Tables[0];
                        RadGrid_PriceHistory.VirtualItemCount = ds.Tables[0].Rows.Count;
                        ViewState["ProposalYear"] = ds.Tables[0];
                        FillProposalYear();
                    }
                }
            }


        }
        catch { }
    }
    private void BindScheduleHistory()
    {
        try
        {
            Int32 TestYear = 0;


            if (Request.QueryString["tyear"] != null)
            {
                TestYear = Convert.ToInt32(Request.QueryString["tyear"]);
            }

            if (Request.QueryString["LID"] != null)
            {
                SafetyTest objproptest = new SafetyTest();
                BL_SafetyTest objtestbl = new BL_SafetyTest();
                objproptest.ConnConfig = WebBaseUtility.ConnectionString;
                objproptest.LID = Convert.ToInt32(Request.QueryString["LID"]);
                DataSet ds = objtestbl.GetAllLoadTestItemSchedule(objproptest);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        gvSchedule.DataSource = ds.Tables[0];
                        gvSchedule.VirtualItemCount = ds.Tables[0].Rows.Count;

                        if (ds.Tables[0].Select("ScheduledYear=" + TestYear).Count() > 0)
                        {
                            DataRow dataRows = ds.Tables[0].Select("ScheduledYear=" + TestYear).OrderByDescending(u => u["ID"]).FirstOrDefault();
                            txtSchedule.Text = dataRows["ScheduledDate"].ToString();
                            txtWorker.Text = dataRows["Worker"].ToString();
                        }
                        else
                        {
                            txtSchedule.Text = "";
                            txtWorker.Text = "";
                        }
                        //var defaultSelectedRow = ds.Tables[0].AsEnumerable()
                        //    .Where(x => x.Field<Int32>("ScheduledYear").Equals(TestYear))
                        //             .OrderBy(y => y.Field<Int32>("ID")).FirstOrDefault();
                        //if (defaultSelectedRow != null)
                        //{
                        //    txtSchedule.Text = defaultSelectedRow.Field<string>("ScheduledDate");
                        //    txtWorker.Text = defaultSelectedRow.Field<string>("Worker");
                        //}
                        //else
                        //{
                        //    txtSchedule.Text = "";
                        //    txtWorker.Text = "";
                        //}
                    }
                }
                else
                {
                    txtSchedule.Text = "";
                    txtWorker.Text = "";
                }

            }


        }
        catch (Exception ex)
        {


        }
    }

    protected void RadGrid_PriceHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPriceHistory();
    }

    protected void gvSchedule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindScheduleHistory();
    }

    protected void lnkDeleteSchedule_Click(object sender, EventArgs e)
    {
        try
        {


            foreach (GridDataItem item in gvSchedule.SelectedItems)
            {
                HiddenField hdnScheduleID = (HiddenField)item.FindControl("hdnScheduleID");
                BL_SafetyTest objtestbl = new BL_SafetyTest();
                SafetyTest obj = new SafetyTest();
                obj.ScheduleID = Convert.ToInt32(hdnScheduleID.Value);
                obj.ConnConfig = Session["config"].ToString();
                objtestbl.DeleteTestScheduledDetail(obj);

            }


            BindScheduleHistory();
            gvSchedule.Rebind();

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseContactWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Schedule is delete successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'We were unable to delete the schedule', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
    }



    protected void lnkScheduleSave_Click(object sender, EventArgs e)
    {
        String msg = "added";

        try
        {
            BL_SafetyTest objtestbl = new BL_SafetyTest();
            SafetyTest obj = new SafetyTest();

            obj.LID = Convert.ToInt32(Request.QueryString["LID"]);
            obj.ScheduleDate = txtScheduleDate.Text;
            obj.PriceYear = Convert.ToInt32(txtScheduleYear.Text);
            obj.ScheduleStatusID = Convert.ToInt32(ddlScheduleStatus.SelectedValue);
            obj.ScheduleWorker = auto_hdnScheduleWorker.Value;
            obj.UserName = Convert.ToString(HttpContext.Current.Session["Username"].ToString());
            obj.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());

            if (hdnAddEditSchedule.Value == "1")
            {
                msg = "Updated";
                obj.ScheduleID = Convert.ToInt32(hdnID.Value);
            }
            objtestbl.UpdateTestScheduledDetail(obj);
            BindScheduleHistory();
            gvSchedule.Rebind();
            txtSchedule.Text = txtScheduleDate.Text;
            txtWorker.Text = auto_hdnScheduleWorker.Value;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseScheduleWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddSchedule", "noty({text: 'Schedule is  " + msg + " successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrorAddSchedule", "noty({text: 'We were unable to update the schedule', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }




    }

    protected void lnkDeletePrice_Click(object sender, EventArgs e)
    {
        try
        {


            foreach (GridDataItem item in RadGrid_PriceHistory.SelectedItems)
            {
                Label lblPriceYear = (Label)item.FindControl("lblPriceYear");
                BL_SafetyTest objtestbl = new BL_SafetyTest();
                SafetyTest obj = new SafetyTest();
                obj.LID = Convert.ToInt32(Request.QueryString["LID"]);
                obj.PriceYear = Convert.ToInt32(lblPriceYear.Text);
                obj.ConnConfig = Session["config"].ToString();
                objtestbl.DeleteTestPriceByYear(obj);

            }


            BindPriceHistory();
            RadGrid_PriceHistory.Rebind();


            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccDeletePrice", "noty({text: 'Price is delete successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDeletePrice", "noty({text: 'We were unable to delete the Price', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
    }

    protected void lnkPriceSave_Click(object sender, EventArgs e)
    {
        String msg = "added";
        int isNew = 1;
        try
        {
            BL_SafetyTest objtestbl = new BL_SafetyTest();
            SafetyTest obj = new SafetyTest();

            obj.LID = Convert.ToInt32(Request.QueryString["LID"]);
            obj.PriceYear = Convert.ToInt32(txtPriceYear.Text);
            obj.Charge = chkChargeablePriceYear.Checked ? 1 : 0;
            if (chkChargeablePriceYear.Checked == false)
            {
                obj.Amount = 0;
                obj.OverrideAmount = 0;
            }
            else
            {
                obj.Amount = txtDefaultAmountYear.Text.Trim() == "" ? 0 : double.Parse(txtDefaultAmountYear.Text.Trim(), NumberStyles.Currency);
                obj.OverrideAmount = txtOverrideAmountYear.Text.Trim() == "" ? 0 : double.Parse(txtOverrideAmountYear.Text.Trim(), NumberStyles.Currency);
            }

            obj.ThirdParty = chkThirdPartyYear.Checked ? 1 : 0;
            obj.ThirdPartyName = txtThirdPartyNameYear.Text;
            obj.ThirdPartyPhone = txtThirdPartyPhoneYear.Text;
            obj.UserName = Convert.ToString(HttpContext.Current.Session["Username"].ToString());
            obj.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());

            if (hdnAddEditPrice.Value == "1")
            {
                msg = "updated";
                isNew = 0;
            }
            int result = objtestbl.UpdatePriceDetailByYear(obj, Convert.ToInt32(hdnUpdatePriceForAll.Value), isNew);
            if (result == 1)
            {
                BindPriceHistory();
                RadGrid_PriceHistory.Rebind();
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "ClosePriceWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddPrice", "noty({text: 'Price is  " + msg + " successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                if (result == 2)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrAddPrice", "noty({text: 'Price for this year already exist', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrAddPrice", "noty({text: 'We were unable to update the price', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }


            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrAddPrice", "noty({text: 'We were unable to update the price', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
    }



    //private string crateTaskTemplate(String equi,string strOldValue, string strNewValue)
    //{
    //    StringBuilder sbdTaskRemask = new StringBuilder();
    //    sbdTaskRemask.AppendLine("This test has changes to alert you.");
    //    sbdTaskRemask.AppendFormat("Equip ID # {0} - {1}", hdnEquipment.Value, ddlTestTypes.SelectedItem.Text);
    //    sbdTaskRemask.AppendLine();
    //    sbdTaskRemask.AppendFormat("Location: {0}", txtAccount.Text);
    //    sbdTaskRemask.AppendLine();

    //    int format = 1;     


    //    sbdTaskRemask.AppendFormat("{0} - {1} value changed from {4} to {5} by {2} - {3}"
    //        , item["Label"], strFormat, item["Username"], System Datetime.now.ToString("MM/dd/yyyy HH:mm tt"), strOldValue, strNewValue);
    //    return sbdTaskRemask.ToString();
    //}
    private void RowSelect()
    {
        foreach (GridDataItem gr in gvSchedule.Items)
        {

            gr.Attributes["ondblclick"] = "OpenEditScheduleWindow()";
        }
    }

    protected void gvSchedule_PreRender(object sender, EventArgs e)
    {
        RowSelect();

    }

    private void FillEquipClassification()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEquipClassificationActive(objPropUser);
        ddlClassification.DataSource = ds.Tables[0];
        ddlClassification.DataTextField = "edesc";
        ddlClassification.DataValueField = "edesc";
        ddlClassification.DataBind();
        ddlClassification.Items.Insert(0, new ListItem("None", "None"));

    }
    public DataTable GetCompanyDetails()
    {
        //Company details
        var connString = Session["config"].ToString();
        BL_Report objBL_Report = new BL_Report();
        BL_User objBL_User = new BL_User();

        User objPropUser = new User();
        objPropUser.ConnConfig = connString;

        DataSet companyInfo = companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());

        return companyInfo.Tables[0];
    }


}
public class ProposalPrice
{
    Double amount { get; set; }
    int count { get; set; }

}
