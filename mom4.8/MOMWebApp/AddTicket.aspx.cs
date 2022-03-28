using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using AjaxControlToolkit;
using System.ServiceModel.Channels;
using System.ServiceModel;
using MicrosoftTranslatorSdk.SoapSamples;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Threading;
using Telerik.Web.UI;
using BusinessLayer.Schedule;
using Microsoft.Reporting.WebForms;
using System.Web.Configuration;
using Stimulsoft.Report;
using System.Configuration;

//using PushNotification;

public partial class AddTicket : Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_Customer objBL_Customer = new BL_Customer();
    Customer objCustomer = new Customer();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    public GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    bool success;
    //AndroidPushNotification obj_PushNotification = new AndroidPushNotification(); 

    string defaultDate = "12/30/1899";

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        //if (Request.QueryString["popup"] != null)
        //{
        //    Page.MasterPageFile = "popup.master";
        //}
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["userid"] == null)
        {
            //Response.Redirect("timeout.htm");
            Response.Redirect("login.aspx");
            return;
        }

        //if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }

        GetControl();

        if (!IsPostBack)
        {
            Session["TicketListViewIsNotPageRefresh"] = "1";
            GetCustomReport();
            GetQBInt();
            userpermissions();
            Permission1();
            CompanyPermission();

            HighlightSideMenu("schMgr", "lnkListView", "schdMgrSub");
            SetInitialRow();
            #region FillControls
            ViewState["comp"] = "0";
            ViewState["convert"] = "0";
            hdnFormId.Value = objGeneralFunctions.generateRandomString(10);/****hdnformid is used for the randomid (tempid) for document upload feature.*********/
            FillCustomFields();
            getDiagnosticCategory();
            //lblReviewed.Visible = false;
            //chkReviewed.Visible = false;
            //FillLoc();
            FillDefaultRoute();
            FillWorker(string.Empty);
            FillCategory();
            FillLevels();
            FillElevUnit();
            FillDepartment();
            FillWage(string.Empty);
            FillZone();
            GetBillcodesforTimeSheet();
            GetPayrollforTimeSheet();
            ViewState["mode"] = 0;
            txtCallDt.Text = System.DateTime.Now.ToShortDateString();
            txtCallTime.Text = System.DateTime.Now.ToShortTimeString();
            ViewState["title"] = lblHeader.Text + " : Mobile Office Manager";
            FillProjectsTemplate();
            GetPOitem();
            txtFby.Text = Session["username"].ToString();
            #endregion

            /**********When start date passed from schedule board to add new ticket***********/
            if (Request.QueryString["timer"] != null)
            {
                txtSchDt.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortDateString();
                txtSchTime.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortTimeString();
                ddlRoute1.SelectedValue = ddlRoute.SelectedValue = Request.QueryString["r"].ToString().ToUpper();
                ddlRoute_SelectedIndexChanged(sender, e);
                ddlStatus1.SelectedValue = ddlStatus.SelectedValue = "1";
                ChangeStatus();
                TimeSpan ts = Convert.ToDateTime(Request.QueryString["end"]) - Convert.ToDateTime(Request.QueryString["start"]);
                txtEST.Text = string.Format("{0:00.00}", ts.TotalHours);
            }
            else
            {
                txtSchDt.Text = DateTime.Now.ToShortDateString();
                txtSchTime.Text = DateTime.Now.ToShortTimeString();
            }

            if (Request.QueryString["locid"] != null)
            {
                hdnLocId.Value = Request.QueryString["locid"].ToString();
                FillLocInfo();
                if (Request.QueryString["unitid"] != null)
                {
                    hdnUnitID.Value = Request.QueryString["unitid"].ToString();
                    txtUnit.Text = Request.QueryString["unit"].ToString();
                }
                GetARByLocation();
            }

            /******* If existing ticket is opened *********/
            if (Request.QueryString["id"] != null)
            {

                chkIsRecurring.Enabled = false;
                FillInventoryGrid();
                if (Request.QueryString["copy"] == null)
                {
                    lnkCopy.Visible = true;
                    lnkCopy.NavigateUrl = "addticket.aspx?copy=1&id=" + Request.QueryString["id"] + "&comp=0";
                    lnkCopy.Target = "_blank";
                }
                RadAjaxPanelHeader.Visible = liTicket.Visible = true;

                lblTicketHeader.Text = "Ticket # " + Request.QueryString["id"];
                liLogs.Style["display"] = "inline-block";
                tbLogs.Style["display"] = "block";
                /********* Check whether ticket is copy mode or edit mode***********/
                if (Request.QueryString["copy"] != null)
                {
                    lblHeader.Text = "Copy Ticket";
                    //tblTicketID.Visible = false;

                }
                else
                {
                    chkCreateMultipleTicket.Visible = false;
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Edit Ticket";
                    ddlStatus1.Items[5].Enabled = ddlStatus.Items[5].Enabled = true;
                    // tblTicketID.Visible = true;
                    //tblWO.Attributes.Add("style", "float:right; margin-right:0px;");
                }
                ViewState["title"] = lblHeader.Text + " : Mobile Office Manager";

                /**********check whether ticket is completed*************/
                if (Request.QueryString["comp"] != null)
                {
                    hdnComp.Value = Request.QueryString["comp"].ToString();
                    ViewState["comp"] = Request.QueryString["comp"].ToString();
                    //objMapData.ISTicketD = Convert.ToInt32(Request.QueryString["comp"]);
                    //if (objMapData.ISTicketD == 2 || objMapData.ISTicketD == 1)
                    if (Convert.ToInt32(Request.QueryString["comp"]) == 2 || Convert.ToInt32(Request.QueryString["comp"]) == 1)
                    {
                        ddlStatus1.Items[5].Enabled = ddlStatus.Items[5].Enabled = true;
                        ////lblReviewed.Visible = true;
                        //chkReviewed.Visible = true;

                    }


                }


                #region Fill data for edit ticket
                DataSet ds = new DataSet();

                ds = GetTicketDataByID();
                try
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        hdnIsJobChargable.Value = ds.Tables[0].Rows[0]["isJobChargeable"].ToString();
                        pnlNext.Visible = true;
                        GetOpportTicket(Convert.ToInt32(Request.QueryString["id"].ToString()));
                        fillREPHistory();
                        GetDocuments();
                        chkWorkComp.Checked = Convert.ToBoolean(Convert.ToInt32(ds.Tables[0].Rows[0]["workcmpl"].ToString()));
                        txtWO.Text = ds.Tables[0].Rows[0]["workorder"].ToString();
                        txtCustomer.Text = ds.Tables[0].Rows[0]["customername"].ToString();
                        hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                        if (ds.Tables[0].Rows[0]["IsTicketProspect"].ToString() == "1")
                        {
                            hdnProspect.Value = "1";
                        }

                        //FillLoc();
                        //ddlLoc.SelectedValue = ds.Tables[0].Rows[0]["lid"].ToString();
                        txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                        hdnLocId.Value = ds.Tables[0].Rows[0]["lid"].ToString();
                        FillLocInfo();
                        GetARByLocation();
                        ddlCategory.SelectedValue = ds.Tables[0].Rows[0]["cat"].ToString();
                        ddlLevel.SelectedValue = ds.Tables[0].Rows[0]["Level"].ToString();
                        //ddlUnit.SelectedValue = ds.Tables[0].Rows[0]["lelev"].ToString();
                        hdnUnitID.Value = ds.Tables[0].Rows[0]["lelev"].ToString();
                        FillRecentCalls(Convert.ToInt32(hdnLocId.Value));
                        txtUnit.Text = ds.Tables[0].Rows[0]["unitname"].ToString();
                        projectNo.Text = hdnProjectId.Value = ds.Tables[0].Rows[0]["job"].ToString();
                        if (hdnProjectId.Value != "0")
                            lnkProjectID.NavigateUrl = "addproject.aspx?uid=" + hdnProjectId.Value;
                        else
                            lnkProjectID.NavigateUrl = "";
                        GetJobCode();
                        SelectTaskCategory();
                        projectNo.Text = txtProject.Text = ds.Tables[0].Rows[0]["jobdesc"].ToString();
                        if (ds.Tables[0].Rows[0]["jobcode1"].ToString() == string.Empty && ds.Tables[0].Rows[0]["phase"].ToString() == "0")
                            txtJobCode.Text = string.Empty;
                        else
                            txtJobCode.Text = objGeneralFunctions.IsNull(ds.Tables[0].Rows[0]["jobcode1"].ToString(), "NA") + "/" + ds.Tables[0].Rows[0]["phase"].ToString() + "/" + ds.Tables[0].Rows[0]["jobitemdesc1"].ToString();
                        if (ds.Tables[0].Rows[0]["phase"].ToString() != "0" && ds.Tables[0].Rows[0]["phase"].ToString() != "")
                        {
                            hdnProjectCode.Value = ds.Tables[0].Rows[0]["phase"].ToString() + ":" + ds.Tables[0].Rows[0]["jobcode1"].ToString() + ":" + ds.Tables[0].Rows[0]["jobitemdesc1"].ToString();
                        }

                        txtReason.Text = ds.Tables[0].Rows[0]["fdesc"].ToString().Split('|')[0];
                        if (ds.Tables[0].Rows[0]["fdesc"].ToString().Split('|').Count() > 1)
                        {
                            txtTranslate.Text = ds.Tables[0].Rows[0]["fdesc"].ToString().Split('|')[1];
                            if (txtTranslate.Text.Trim() != string.Empty)
                            {
                                pnlTranslate.Attributes.Add("style", " display: block;");
                            }
                        }
                        txtWorkCompl.Text = ds.Tables[0].Rows[0]["descres"].ToString().Split('|')[0];
                        if (ds.Tables[0].Rows[0]["descres"].ToString().Split('|').Count() > 1)
                        {
                            txtTransDesc.Text = ds.Tables[0].Rows[0]["descres"].ToString().Split('|')[1];
                            //if (Request.QueryString["comp"].ToString() == "2" && txtTransDesc.Text.Trim() != string.Empty)
                            if (txtTransDesc.Text.Trim() != string.Empty)
                            {
                                //txtWorkCompl.Text = TranslateMethod(GetAccessToken(), txtTransDesc.Text, "es", "en");
                                pnlTransDesc.Attributes.Add("style", "display: block;");
                            }
                        }
                        if (ds.Tables[0].Rows[0]["phone"].ToString().Trim() != string.Empty)
                            txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                        if (ds.Tables[0].Rows[0]["cphone"].ToString().Trim() != string.Empty)
                            txtCell.Text = ds.Tables[0].Rows[0]["cphone"].ToString();
                        FillWorker(ds.Tables[0].Rows[0]["dworkup"].ToString());
                        ddlRoute1.SelectedValue = ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["dworkup"].ToString().Trim();
                        ddlRoute_SelectedIndexChanged(sender, e);
                        ViewState["workid"] = ds.Tables[0].Rows[0]["dworkup"].ToString();
                        txtCallDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["cdate"]).ToShortDateString();
                        string stc = Convert.ToDateTime(ds.Tables[0].Rows[0]["cdate"]).ToShortTimeString();
                        if (stc == "") { stc = "12:00 AM"; }
                        txtCallTime.Text = stc;
                        if (ds.Tables[0].Rows[0]["edate"] != DBNull.Value)
                        {
                            txtSchDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["edate"]).ToShortDateString();
                            string stc2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["edate"]).ToShortTimeString();
                            if (stc2 == "") { stc2 = "12:00 AM"; }
                            txtSchTime.Text = stc2;
                        }
                        ddlStatus1.SelectedValue = ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["assigned"].ToString();
                        ChangeStatus();
                        txtEST.Text = ds.Tables[0].Rows[0]["est"].ToString();
                        lblTicketnumber.Text = ds.Tables[0].Rows[0]["id"].ToString();
                        lblTicketnumber.Visible = true;
                        // lblTicketLabel.Visible = true;

                        HiddenFieldRT.Value = txtRT.Text = ds.Tables[0].Rows[0]["Reg"].ToString();
                        HiddenFieldOT.Value = txtOT.Text = ds.Tables[0].Rows[0]["ot"].ToString();
                        HiddenFieldNT.Value = txtNT.Text = ds.Tables[0].Rows[0]["nt"].ToString();
                        HiddenFieldDT.Value = txtDT.Text = ds.Tables[0].Rows[0]["dt"].ToString();
                        HiddenFieldBT.Value = txtBT.Text = ds.Tables[0].Rows[0]["BT"].ToString();
                        if (PermissionHiddenFieldTT.Value == "1")
                        {
                            HiddenFieldTT.Value = txtTT.Text = ds.Tables[0].Rows[0]["TT"].ToString();
                        }
                        else
                        {
                            HiddenFieldTT.Value = txtTT.Text = "0";
                        }
                        txtTotal.Value = lblTotal.Text = ds.Tables[0].Rows[0]["total"].ToString();

                        txtPartsUsed.Text = ds.Tables[0].Rows[0]["PartsUsed"].ToString();
                        txtComments.Text = ds.Tables[0].Rows[0]["Comments"].ToString();

                        txtExpMisc.Text = ds.Tables[0].Rows[0]["othere"].ToString();
                        txtExpToll.Text = ds.Tables[0].Rows[0]["toll"].ToString();
                        txtExpZone.Text = ds.Tables[0].Rows[0]["zone"].ToString();
                        txtMileStart.Text = ds.Tables[0].Rows[0]["Smile"].ToString();
                        txtMileEnd.Text = ds.Tables[0].Rows[0]["emile"].ToString();
                        //chkChargeable.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Chargen"]);
                        chkChargeable.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Charge"]);
                        chkJobChargeable.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Charge"]);

                        // imgInv.ImageUrl = chkChargeable.Checked == true ? "images/dollar.png" : "images/DollarRed.png";
                        chkReviewed.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ClearCheck1"]);
                        chkPayroll.Checked = false;
                        if (ds.Tables[0].Rows[0]["ClearPR"].ToString() != "")
                        {
                            chkPayroll.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ClearPR"]);
                        }

                        hdnReviewed.Value = ds.Tables[0].Rows[0]["ClearCheck1"].ToString();

                        chkTimeTrans.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["TimeTransfer"]);




                        int invoiceId = 0;

                        string Minvoice = ds.Tables[0].Rows[0]["manualinvoice"].ToString();

                        string Sinvoice = ds.Tables[0].Rows[0]["invoice"].ToString();

                        if (Int32.TryParse(Sinvoice, out invoiceId))
                        {
                            if (invoiceId > 0) txtInvoiceNo.Text = invoiceId.ToString();
                        }


                        txtManualInvoice.Text = Minvoice.ToString();





                        if (ds.Tables[0].Rows[0]["internet"] != DBNull.Value)
                        {
                            chkInternet.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["internet"]);
                        }

                        if (!IsPostBack)
                        {
                            if (ds.Tables[0].Rows[0]["tablename"].ToString() != "TicketD")
                            {


                                if (
                                    ViewState["internetdefault"].ToString() == "1"
                                    && chkReviewed.Checked == false
                                    && ddlStatus.SelectedValue == "4"
                                    )
                                    chkInternet.Checked = true;
                            }
                        }
                        txtNameWho.Text = ds.Tables[0].Rows[0]["who"].ToString();
                        //txtRemarks.Text = ds.Tables[0].Rows[0]["bremarks"].ToString();
                        txtRecommendation.Text = ds.Tables[0].Rows[0]["bremarks"].ToString();
                        ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();

                        txtCst1.Text = ds.Tables[0].Rows[0]["custom1"].ToString();
                        txtCst2.Text = ds.Tables[0].Rows[0]["custom2"].ToString();
                        txtCst3.Text = ds.Tables[0].Rows[0]["custom3"].ToString();
                        txtCst4.Text = ds.Tables[0].Rows[0]["custom4"].ToString();
                        txtCst5.Text = ds.Tables[0].Rows[0]["custom5"].ToString();

                        txtTickCustom1.Text = ds.Tables[0].Rows[0]["Customtick1"].ToString();
                        txtTickCustom2.Text = ds.Tables[0].Rows[0]["Customtick2"].ToString();
                        txtTickCustom3.Text = ds.Tables[0].Rows[0]["Customtick5"].ToString();

                        try
                        {
                            chkTickCustom1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Customticket3"]);
                            chkTickCustom2.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Customticket4"]);
                        }
                        catch { }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["highdecline"].ToString()))
                        {
                            imgHigh.Visible = Convert.ToBoolean(ds.Tables[0].Rows[0]["highdecline"]);
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["custom6"].ToString()))
                        {
                            chkCst1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["custom6"]);
                        }

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["custom7"].ToString()))
                        {
                            chkCst2.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["custom7"]);
                        }

                        if (ds.Tables[0].Rows[0]["timeroute"] != DBNull.Value)
                        {
                            txtEnrTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["timeroute"]).ToShortTimeString();
                        }
                        if (ds.Tables[0].Rows[0]["timesite"] != DBNull.Value)
                        {
                            txtOnsitetime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["timesite"]).ToShortTimeString();
                        }
                        if (ds.Tables[0].Rows[0]["timecomp"] != DBNull.Value)
                        {
                            txtComplTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["timecomp"]).ToShortTimeString();
                        }

                        ddlPayroll.SelectedValue = ds.Tables[0].Rows[0]["qbpayrollitem"].ToString();
                        ddlService.SelectedValue = ds.Tables[0].Rows[0]["qbserviceitem"].ToString();
                        ListItem item = ddlWage.Items.FindByValue(ds.Tables[0].Rows[0]["wagec"].ToString());
                        if (item != null)
                        {
                            ddlWage.SelectedValue = ds.Tables[0].Rows[0]["wagec"].ToString();
                        }
                        else {
                            try
                            {
                                if (ddlWage.Items.Count == 2 && ddlRoute1.SelectedIndex != 0)
                                {
                                    ddlWage.SelectedIndex = 1;
                                }
                                else if (hdnProjectwageID.Value != "" && ddlRoute1.SelectedIndex != 0 && ddlStatus.SelectedValue == "4" && ddlWage.SelectedIndex == 0)
                                {
                                    string str = hdnProjectwageID.Value.ToString();
                                    ListItem item1 = ddlWage.Items.FindByValue(str);
                                    if (item1 != null)
                                    {
                                        ddlWage.SelectedValue = str;
                                    }
                                }
                            }
                            catch { }
                        }
                        txtFby.Text = ds.Tables[0].Rows[0]["fby"].ToString();

                        imgAfterHours.Visible = Convert.ToBoolean(ds.Tables[0].Rows[0]["afterhours"]);
                        imgWeekend.Visible = Convert.ToBoolean(ds.Tables[0].Rows[0]["weekends"]);
                        imgEmail.Visible = (ds.Tables[0].Rows[0]["EmailNotified"].ToString() == "1") ? true : false;


                        selectEquip(Convert.ToInt32(Request.QueryString["id"].ToString()));


                        if (Request.QueryString["copy"] == null)
                        {
                            string signature = GetTicketSignature(ds.Tables[0].Rows[0]["id"].ToString(), ds.Tables[0].Rows[0]["fwork"].ToString()).Trim();
                            if (signature != string.Empty)
                            {
                                imgSign.ImageUrl = signature;
                                hdnImg.Value = signature;
                            }


                            Fill_InvoiceInfo(ds);

                            if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
                            {
                                if (ds.Tables[0].Rows[0]["dworkup"].ToString().Trim() != string.Empty)
                                {
                                    if (Session["username"].ToString().ToUpper() != ds.Tables[0].Rows[0]["superv"].ToString().ToUpper())
                                    {
                                        lnkSave.Visible = false;
                                        lnkPrint.Visible = false;
                                        pnlDocumentButtons.Visible = false;
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuccUp", "noty({text: 'Readonly Mode.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                                    }
                                }
                            }
                            ViewState["title"] = "Ticket# " + Request.QueryString["id"].ToString() + " : Mobile Office Manager";

                        }
                        try
                        {
                            chkIsRecurring.Checked = ds.Tables[0].Rows[0]["Recurring"] == DBNull.Value ? false : true;
                        }
                        catch { }

                        GetDocuments();

                        if (ds.Tables[0].Rows[0]["locStatus"].ToString() == "1")
                        {
                            lnkSave.Visible = false;
                            lnkCopy.Visible = false;

                            ClientScript.RegisterStartupScript(Page.GetType(), "keyLocationisinactive", "  noty({text: 'Location is inactive.',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false}); ", true);
                        }

                        if (ds.Tables[0].Rows[0]["assigned"].ToString() == "6")
                        {
                            lnkSave.Visible = false;
                            lnkCopy.Visible = false;

                            ClientScript.RegisterStartupScript(Page.GetType(), "keyVoided", "  noty({text: 'Ticket is Voided.',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false}); ", true);
                        }
                    }
                }
                catch (Exception ex)
                {

                    string ssss = ex.Message;
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyerrro", "  noty({text: '" + ssss + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false}); ", true);

                }
                #endregion


                if (Request.QueryString["comp"] != null)
                {
                    /******** check whether ticket is completed and is TS login ********* IntegrateIntegrate */
                    if (ViewState["tsint"].ToString() != "1")
                    {
                        //if (Session["MSM"].ToString() == "TS")//&& Convert.ToInt32(Request.QueryString["comp"]) != 1
                        //{
                        if (Request.QueryString["copy"] != null)
                        {
                            ddlStatus1.SelectedValue = ddlStatus.SelectedValue = "1";
                            ChangeStatus();
                            txtInvoiceNo.Text = "";
                            txtopp.Text = "";
                            lnkOpport.Visible = false;
                            lnkInvoice.Visible = false;

                            lnkProjectID.NavigateUrl = "";
                        }

                    }
                }

                if (Request.QueryString["st"] != null)
                {
                    ddlStatus1.SelectedValue = ddlStatus.SelectedValue = Request.QueryString["st"].ToString();
                    ChangeStatus();
                }

                /********Check for follow up tikcet ********/
                if (Request.QueryString["follow"] != null)
                {
                    lblHeader.Text = "Follow-up Ticket";
                    ViewState["title"] = lblHeader.Text + " : Mobile Office Manager";
                    txtReason.Text = "Follow-up on Ticket# " + Request.QueryString["id"].ToString() + "\n" + txtReason.Text;
                    if (Convert.ToInt16(Session["IsMultiLang"]) != 0)
                    {
                        txtTranslate.Text = TranslateMethod(GetAccessToken(), "Follow-up on Ticket#", "en", "es") + " " + Request.QueryString["id"].ToString() + "\n" + txtTranslate.Text;
                    }
                    chkWorkComp.Checked = true;
                    chkReviewed.Checked = false;
                    chkPayroll.Checked = false;
                    ddlStatus1.SelectedValue = ddlStatus.SelectedValue = "1";
                    ChangeStatus();
                    txtManualInvoice.Text = txtInvoiceNo.Text = string.Empty;
                    //txtWO.Text = string.Empty;

                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuucessFollowupMSg", "  noty({text: 'Ticket# " + Request.QueryString["id"].ToString() + " saved successfully!',dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); ", true);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyFollowUpMSg", "  noty({text: 'You can now create follow-up ticket below.',dismissQueue: true,  type : 'information', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false}); ", true);
                }

                /***********fill related tickets (tickets with same WO)************/
                ////FillRelatedTickets();
            }
            else
            {
                lnkPDF.Visible = false;
            }

            /******** Check for TS login**********IntegrateIntegrate*/
            if (ViewState["tsint"].ToString() != "1")
            {
                if (Session["MSM"].ToString() == "TS")
                {
                    chkCreateMultipleTicket.Visible = false;
                    ////ddlStatus.Enabled = false;
                    //btnEnroute.Enabled = false;
                    //btnOnsite.Enabled = false;
                    //btnComplete.Enabled = false;
                }
            }

            //GetQBInt();
            ShowQBSyncControls();

            /*******set focus on controls using hidden variable. it was needed because ajax toolkit tab control causes default focus to be set at the very first tab. so we need to set it this way.*********/
            hdnFocus.Value = txtLocation.ClientID;
            FillEquiptype();
            FillEquipCategory();
            FillBuilding();
            SetDefaultWorker();

            Statustooltipped.Style.Add("background-color", TicketStatusColor(ddlStatus.SelectedItem.Value));
            Statustooltipped.Attributes.Add("data-tooltip", ddlStatus.SelectedItem.Text);
            //if (ddlStatus.SelectedItem.Value == "4") { accrdcompleted.Visible = true; LI_Completed.Disabled = false; }
            //else { accrdcompleted.Visible = false; LI_Completed.Disabled = true; }
            bool zone = IsZoneEnabled();
            if (!zone)
            {
                ddlZone.Visible = false;
                lblZone.Visible = false;
            }
            Permission();
        }

        //////

        if (ddlStatus.SelectedValue == "4") /// Job Casting Condition...
        {
            // Project Template Validation for Job Casting...
            rfvjtempl.Visible = false;
            if ((hdnProjectId.Value == "0" || hdnProjectId.Value == ""))
            {
                // Check Quick Book Customer Condition 
                // We should not make required projects template for Quick Book Customer 
                if (ViewState["qbint"].ToString() != "1")
                {
                    rfvjtempl.Visible = true;
                }
                else
                {
                    rfvjtempl.Visible = false;
                    hdnIsCreateJob.Value = "0";
                }
            }

            //Wages category mandatory field if the Job Cost Labor = Burden Rate

            string wagesRage = ViewState["JobCostLabor"].ToString();

            if (wagesRage == "1")
            {
                if (ddlWage.SelectedIndex == 0)
                {
                    return;
                }
            }

        }
        if (!IsPostBack)
        {
            if (chkPayroll.Checked) { PayRollPermission(); }
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

    public bool CheckAddEditPermission()
    {
        bool result = true;
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            /// Ticket ///////////////////------->

            string ticketPermission = ds.Rows[0]["TicketPermission"] == DBNull.Value ? "YYNYYY" : ds.Rows[0]["TicketPermission"].ToString();
            string stAddeTicket = ticketPermission.Length < 1 ? "Y" : ticketPermission.Substring(0, 1);
            string stEditeTicket = ticketPermission.Length < 2 ? "Y" : ticketPermission.Substring(1, 1);
            string stViewTicket = ticketPermission.Length < 4 ? "Y" : ticketPermission.Substring(3, 1);



            //Document
            string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
            hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
            hdnEditeDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
            hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
            hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

            pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;

            if (Request.QueryString["id"] != null)
            {
                if (stViewTicket == "N")
                {
                    result = false;
                }
                else if (stViewTicket == "Y" && stEditeTicket == "N")
                {
                    lnkSave.Visible = false;
                }
            }
            else
            {
                if (stAddeTicket == "N")
                {
                    result = false;
                }
            }
        }
        return result;
    }

    private void Permission1()
    {


        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderSchd");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("schdMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }
        if (Session["MSM"].ToString() == "TS")
        {
            //lnkCopy.Visible = false;
            //lnkDelete.Visible = false;
            //lnkAddticket.Visible = false;
        }
    }

    private void Permission()
    {
        if (ViewState["PR"].ToString() == "False")
        {
            chkPayroll.Visible = false;
        }

        ViewState["MassPayrollTicket"] = "Y";

        if (Session["type"].ToString() == "c" || Session["MSM"].ToString() == "TS")

        {
            ViewState["MassPayrollTicket"] = "N";
        }

        #region Permission for location remarks
        string strLocRemarks = string.Empty;
        string strTCFix = string.Empty;
        if (Session["type"].ToString() != "am")
        {
            DataTable dtLocrem = new DataTable();
            dtLocrem = (DataTable)Session["userinfo"];

            strLocRemarks = dtLocrem.Rows[0]["Location"].ToString().Substring(3, 1);
            strTCFix = dtLocrem.Rows[0]["TC"].ToString().Substring(1, 1);
            if (strTCFix == "Y")
            {
                txtEnrTime.Enabled = false;
                txtOnsitetime.Enabled = false;
                txtComplTime.Enabled = false;
                MaskedEditValidator1.Enabled = false;
                MaskedEditValidator2.Enabled = false;
                MaskedEditValidator4.Enabled = false;
                //btnEnroute.Enabled = false;
                //btnOnsite.Enabled = false;
                //btnComplete.Enabled = false;
                //ddlStatus.Items[2].Attributes["disabled"] = "disabled";
                //ddlStatus.Items[3].Attributes["disabled"] = "disabled";
                //ddlStatus.Items[4].Attributes["disabled"] = "disabled";
                //ddlStatus.Items[2].Attributes["style"] = "background-color:silver";
                //ddlStatus.Items[3].Attributes["style"] = "background-color:silver";
                //ddlStatus.Items[4].Attributes["style"] = "background-color:silver";
            }
        }
        else
        {
            strLocRemarks = "Y";
        }
        if (strLocRemarks == "Y")
        {
            txtRemarks.Visible = true;
            lblRemarks.Visible = true;
        }
        else
        {
            txtRemarks.Visible = false;
            lblRemarks.Visible = false;
        }
        #endregion

        if (ViewState["title"] != null)
        {
            if (ViewState["title"].ToString() != string.Empty)
                Page.Title = ViewState["title"].ToString();
        }
        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {

            DataTable ds = new DataTable();
            ds = GetUserById();

            ViewState["MassPayrollTicket"] = ds.Rows[0]["MassPayrollTicket"].ToString();

            string CreditHoldPermission = ds.Rows[0]["CreditHold"] == DBNull.Value ? "YYYY" : ds.Rows[0]["CreditHold"].ToString();
            string CreditHold = CreditHoldPermission.Length < 1 ? "Y" : CreditHoldPermission.Substring(0, 1);
            if (CreditHold == "N")
            {
                chkCreditHold.Enabled = false;
            }
            string TimeStampPermission = ds.Rows[0]["TC"] == DBNull.Value ? "YYYY" : ds.Rows[0]["TC"].ToString();
            string TimeStamp = TimeStampPermission.Length < 1 ? "Y" : TimeStampPermission.Substring(1, 1);
            if (TimeStamp == "Y")
            {
                RadAjaxPanelTimestamps.Enabled = false;
            }
            string ResolvedTicketPermission = ds.Rows[0]["Resolve"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Resolve"].ToString();
            string ADDResolved = ResolvedTicketPermission.Length < 1 ? "Y" : ResolvedTicketPermission.Substring(0, 1);
            string EditResolved = ResolvedTicketPermission.Length < 2 ? "Y" : ResolvedTicketPermission.Substring(1, 1);
            string DeleteResolved = ResolvedTicketPermission.Length < 3 ? "Y" : ResolvedTicketPermission.Substring(2, 1);
            string ViewResolved = ResolvedTicketPermission.Length < 4 ? "Y" : ResolvedTicketPermission.Substring(3, 1);
            if (ViewResolved == "N")
            {
                DIVaccrdcompleted.Attributes.Add("style", "display:none");
                lblaccrdcompleted.Visible = true;
            }
            ViewState["ADDResolved"] = ADDResolved;
            ViewState["EditResolved"] = EditResolved;
            ViewState["ddlstatus"] = ddlStatus.SelectedValue;


            /// SchedulemodulePermission ///////////////////------->

            string SchedulemodulePermission = ds.Rows[0]["SchedulemodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["SchedulemodulePermission"].ToString();

            if (SchedulemodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// BillPay ///////////////////------->

            string ticketPermission = ds.Rows[0]["dispatch"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["dispatch"].ToString();
            string ADD = ticketPermission.Length < 1 ? "Y" : ticketPermission.Substring(0, 1);
            string Edit = ticketPermission.Length < 2 ? "Y" : ticketPermission.Substring(1, 1);
            string Delete = ticketPermission.Length < 3 ? "Y" : ticketPermission.Substring(2, 1);
            string View = ticketPermission.Length < 4 ? "Y" : ticketPermission.Substring(3, 1);
            string Report = ticketPermission.Length < 6 ? "Y" : ticketPermission.Substring(5, 1);
            string ticketDispatchPermission = ds.Rows[0]["dispatch"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["dispatch"].ToString();
            string dispatchView = ticketDispatchPermission.Length < 4 ? "Y" : ticketDispatchPermission.Substring(3, 1);
            if (Report != "Y")
            {
                lnkPrint.Visible = false;
                lnkPDF.Visible = false;
            }
            if (View != "Y" && dispatchView != "Y")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            else if (Request.QueryString["id"] == null)
            {
                if (ADD == "N")
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
                //else
                //{
                //    if (ADDResolved == "N")
                //    {
                //        ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Completed"));
                //    }
                //    lnkCopy.Visible = false;
                //}
            }
            else if (Edit == "N")
            {
                if (View == "Y")
                {
                    lnkSave.Visible = false;
                    lnkCopy.Visible = false;
                    //btnSubmitJob.Visible = false;

                }
                else
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }

            if (ViewState["MassPayrollTicket"].ToString() != "Y")
            {
                chkPayroll.Enabled = false;
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

    private void userpermissions()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
        if (intintegration == 1)
        { txtCustSageID.Visible = true; lblSageid.Visible = true; hdnSageInt.Value = "1"; }
        else
        { txtCustSageID.Visible = false; lblSageid.Visible = false; hdnSageInt.Value = "0"; }

        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Username = Session["username"].ToString();
                objPropUser.PageName = "addticket.aspx";
            }

        }
    }

    private void FillDefaultRoute()
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            ds = objBL_User.getRoute(objPropUser);
            ddlDefRoute.DataSource = ds.Tables[0];
            ddlDefRoute.DataTextField = "label";
            ddlDefRoute.DataValueField = "ID";
            ddlDefRoute.DataBind();
            ddlDefRoute.Items.Insert(0, new ListItem(" Select ", ""));
            ddlDefRoute.Items.Insert(1, new ListItem("Unassigned", "0"));
        }
        catch { }
    }

    private void GetOpportTicket(Int32 TicketID, bool Sendmail = false)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.TicketID = TicketID;
            string opportID = objBL_MapData.GetopportunityTicket(objMapData);

            if (!string.IsNullOrEmpty(opportID.Trim()))
            {
                lnkOpport.Visible = true;
                txtopp.Text = opportID;
                lnkOpport.NavigateUrl = "addopprt.aspx?uid=" + opportID;

                if (Session["MSM"].ToString() == "TS")
                {
                    //lnkOpport.Enabled = false;
                }

                if (Sendmail) SendMailToSalesPer(TicketID, opportID);
            }
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Fill data to custom fields tab
    /// </summary>
    private void FillCustomFields()
    {
        DataSet dscstm = new DataSet();

        dscstm = GetCustomFields("Ticket2");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom2.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket3");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom3.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket1");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom1.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket4");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom4.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket5");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom5.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket6");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom6.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket7");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom7.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("TicketCst1");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustomTick1.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("TicketCst2");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustomTick2.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("TicketCst3");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            chkTickCustom1.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("TicketCst4");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            chkTickCustom2.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("TicketCst5");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustomTick5.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
    }

    /// <summary>
    /// operation related to gridview row javascript.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridDataItem gr in RadgvEquip.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblname = (Label)gr.FindControl("lblUnit");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            TextBox txtPrice = (TextBox)gr.FindControl("txtPrice");
            TextBox txtHours = (TextBox)gr.FindControl("txtHours");

            chkSelect.Attributes["onclick"] = "SelectRowsEq();";
        }

        foreach (GridDataItem gr in RadgvProject.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblname = (Label)gr.FindControl("lblDescP");
            Label lblCharge = (Label)gr.FindControl("lblCharge");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["onclick"] = "SelectProjectRow('" + RadgvProject.ClientID + "','" + lblID.ClientID + "','" + lblname.ClientID + "','" + lblCharge.ClientID + "','"
                                                            + hdnProjectId.ClientID + "','" + txtProject.ClientID + "','" + projectNo.ClientID
                                                            + "','DivProject');  document.getElementById('" + btnGetCode.ClientID + "').click();";
        }

        foreach (GridDataItem gr in RadgvProjectCode.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblIDname = (Label)gr.FindControl("lblIDname");
            Label lblIDname1 = (Label)gr.FindControl("lblIDname1");
            Label lblname = (Label)gr.FindControl("lblDesc");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["onclick"] = "SelectProjectTypeRow('" + RadgvProjectCode.ClientID + "','" + lblIDname.ClientID + "','" + lblIDname1.ClientID + "','" + hdnProjectCode.ClientID + "','" + txtJobCode.ClientID + "','" + projectNo.ClientID + "','DivprojectType'); ";
        }

        foreach (GridDataItem gr in RadgvDocuments.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + RadgvDocuments.ClientID + "',event);";
        }

        foreach (GridDataItem gr in RadGrid_Inventory.Items)
        {

            HiddenField hdnAID = (HiddenField)gr.FindControl("hdnAID");
            ImageButton ibDelete = (ImageButton)gr.FindControl("ibDelete");
            TextBox txtGvQuan = (TextBox)gr.FindControl("txtGvQuan");
            TextBox txtGvWarehouse = (TextBox)gr.FindControl("txtGvWarehouse");
            TextBox txtGvItem = (TextBox)gr.FindControl("txtGvItem");

            TextBox txtGvPhase = (TextBox)gr.FindControl("txtGvPhase");
            TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
            TextBox txtGvWarehouseLocation = (TextBox)gr.FindControl("txtGvWarehouseLocation");
            CheckBox chkBill = (CheckBox)gr.FindControl("chkBill");

            if (hdnAID.Value != "")
            {

                txtGvPhase.ReadOnly =
                txtGvItem.ReadOnly =
                txtGvDesc.ReadOnly =
                txtGvQuan.ReadOnly =
                txtGvWarehouse.ReadOnly =
                txtGvWarehouseLocation.ReadOnly = true;
                chkBill.Enabled = false;
                ibDelete.Visible = false;
            };
        }

        // Permission();
    }

    /// <summary>
    /// Get data from Control table to show hide controls. Currently it handles the multilanguage option controls.
    /// </summary>
    private void GetControl()
    {
        ViewState["PR"] = "True";

        DataSet ds = new DataSet();

        User objProp_User = new User();

        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getControl(objProp_User);

        int Multilang = Convert.ToInt16(Session["IsMultiLang"]);

        hdnMultiLang.Value = Multilang.ToString();

        if (Multilang == 0)
        {
            divTransicon.Visible = false; divTransIconReason.Visible = false;
        }
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

        ddlCodeCat.Items.Insert(0, new ListItem("ALL", "ALL"));
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
    /// <summary>
    /// Fill custom fields data according to the field name paased.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objGeneral.CustomName = name;
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getCustomFields(objGeneral);
        return ds;
    }

    private void GetDataEquip()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.SearchBy = string.Empty;
        objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
        HyperLinkAddEquip.NavigateUrl = "addequipment.aspx?lid=" + hdnLocId.Value + "&locname=" + txtLocation.Text + "&addFrom=Ticket";
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

        //if (Request.QueryString["id"] != null)
        //{
        foreach (GridDataItem gr in RadgvEquip.Items)
        {
            LinkButton LinkButton = (LinkButton)gr.FindControl("lnkbtnEuipContractinfo");
            Label lblname = (Label)gr.FindControl("EquipContractinfo");
            DataSet ds11 = new DataSet();
            ds11 = GetContractInfo(0, Convert.ToInt32(LinkButton.CommandArgument), "Equipment");
            if (ds11.Tables[0].Rows.Count > 1)
            {
                LinkButton.Visible = true;
                lblname.Visible = false;
            }
            else if (ds11.Tables[0].Rows.Count == 1)
            {
                lblname.Text = "Contract type:-" + (ds11.Tables[0].Rows[0]["ContractType"]) + ", Schedule Frequency:-" + (ds11.Tables[0].Rows[0]["ScheduleFrequency"]);
                LinkButton.Visible = false;
            }
            else
            {
                LinkButton.Visible = false;
                lblname.Visible = false;
            }
        }
        //} 
    }

    private void selectEquip(int ticketID)
    {
        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = ticketID;
        ds = objBL_MapData.getElevByTicket(objMapData);
        txtUnit.Text = string.Empty;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            foreach (GridDataItem gr in RadgvEquip.Items)
            {
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblname = (Label)gr.FindControl("lblunit");
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                TextBox txtHours = (TextBox)gr.FindControl("txtHours");
                if (dr["elev_id"].ToString() == lblID.Text)
                {
                    chkSelect.Checked = true;
                    txtHours.Text = dr["labor_percentage"].ToString();

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

    private void GetDataProject()
    {
        if (hdnLocId.Value != "")
        {
            DataSet ds = new DataSet();
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.LocID = Convert.ToInt32(hdnLocId.Value);

            ds = objBL_Customer.getJobEstimate(objCustomer);
            RadgvProject.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadgvProject.DataSource = ds.Tables[0];
            //gvProject.DataBind();

            if (Request.QueryString["jobid"] != null)
            {


                hdnProjectId.Value = Request.QueryString["jobid"];
                txtProject.Text = hdnProjectId.Value + "-" + Request.QueryString["JobName"];
                projectNo.Text = hdnProjectId.Value + "-" + Request.QueryString["JobName"];
                GetJobCode();
                txtNameWho.Text = Session["username"].ToString();


            }
        }
    }

    private void FillWorker(string Worker)
    {
        ddlRoute1.Items.Clear(); ddlRoute.Items.Clear();
        ddlRoute1.SelectedIndex = -1; ddlRoute.SelectedIndex = -1;
        ddlRoute1.SelectedValue = null; ddlRoute.SelectedValue = null;
        ddlRoute1.ClearSelection(); ddlRoute.ClearSelection();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 0;
        objPropUser.Username = Worker;
        ds = objBL_User.getEMP(objPropUser);
        ddlRoute1.DataSource = ddlRoute.DataSource = ds.Tables[0];
        ddlRoute1.DataTextField = ddlRoute.DataTextField = "fDesc";
        ddlRoute1.DataValueField = ddlRoute.DataValueField = "fDesc";
        ddlRoute1.DataBind(); ddlRoute.DataBind();
        chkcatlist.DataSource = ds.Tables[0];
        chkcatlist.DataTextField = "fDesc";
        chkcatlist.DataValueField = "fDesc";
        chkcatlist.DataBind();

        ddlRoute.Items.Insert(0, new ListItem(" Select ", ""));
        ddlRoute1.Items.Insert(0, new ListItem(" Select ", ""));
    }

    private void FillCategory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        if (Request.QueryString["id"] != null)
        {
            ds = objBL_User.getCategory(objPropUser);
        }
        else
        {
            ds = objBL_User.getCategoryActive(objPropUser);
        }

        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "type";
        ddlCategory.DataValueField = "type";
        ddlCategory.DataBind();

        ddlCategory.Items.Insert(0, new ListItem(" Select ", ""));
        ddlCategory.Items.Insert(1, new ListItem("None", "None"));
    }
    private void FillLevels()
    {
        DataSet dsL = new DataSet();

        objPropUser.ConnConfig = Session["config"].ToString();

        dsL = objBL_User.getLevels(objPropUser);
        if (dsL.Tables[0].Rows.Count > 0)
        {
            ddlLevel.DataSource = dsL.Tables[0];
            ddlLevel.DataTextField = "Label";
            ddlLevel.DataValueField = "Name";
            ddlLevel.DataBind();
        }
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

        ddlDepartment.Items.Insert(0, new ListItem(" Select ", "-1"));

        if (Session["MSM"].ToString() != "TS")
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["isdefault"].ToString() == "1")
                {
                    ddlDepartment.SelectedValue = dr["id"].ToString();
                }
            }
        }
    }

    private void FillEquiptype()
    {
        Session["EquipTypeLabel"] = null;
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEquiptype(objPropUser);
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            Session["EquipTypeLabel"] = ds.Tables[0].Rows[0]["Label"].ToString();
            RadgvEquip.Columns[4].HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    private void FillEquipCategory()
    {
        Session["EquipCatLabel"] = null;
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEquipmentCategory(objPropUser);
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            Session["EquipCatLabel"] = ds.Tables[0].Rows[0]["Label"].ToString();
            RadgvEquip.Columns[5].HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    private void FillBuilding()
    {
        Session["EquipLabel"] = null;
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getBuildingElev(objPropUser);
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            Session["EquipLabel"] = ds.Tables[0].Rows[0]["Label"].ToString();
            RadgvEquip.Columns[7].HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    private void FillElevUnit()
    {
        //if (hdnLocId.Value != "")
        //{
        //    DataSet ds = new DataSet();
        //    objPropUser.ConnConfig = Session["config"].ToString();
        //    objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
        //    ds = objBL_User.getElevUnit(objPropUser);
        //    ddlUnit.DataSource = ds.Tables[0];
        //    ddlUnit.DataTextField = "unit";
        //    ddlUnit.DataValueField = "id";
        //    ddlUnit.DataBind();
        //}
        //ddlUnit.Items.Insert(0, new ListItem("None", "0"));
    }

    /// <summary>
    /// fills location accoding to the customer selected.
    /// </summary>
    public void FillLoc()
    {
        //if (hdnPatientId.Value != string.Empty)
        //{
        //    objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);
        //    objPropUser.DBName = Session["dbname"].ToString();
        //    DataSet ds = new DataSet();
        //    ds = objBL_User.getLocationByCustomerID(objPropUser);

        //    ddlLoc.DataSource = ds.Tables[0];
        //    ddlLoc.DataTextField = "Name";
        //    ddlLoc.DataValueField = "loc";
        //    ddlLoc.DataBind();
        //}
        //ddlLoc.Items.Insert(0,new ListItem(" Select ",""));

        DataSet ds = new DataSet();
        objPropUser.SearchValue = "";
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);
        ds = objBL_User.getLocationAutojquery(objPropUser, new GeneralFunctions().GetSalesAsigned());
        SetProspect();
        if (ds.Tables[0].Rows.Count == 1)
        {
            hdnLocId.Value = ds.Tables[0].Rows[0]["value"].ToString();
            txtLocation.Text = ds.Tables[0].Rows[0]["label"].ToString();
            FillLocInfo();
            FillRecentCalls(Convert.ToInt32(hdnLocId.Value));
        }
        else if (ds.Tables[0].Rows.Count > 1)
        { txtCustSageID.Text = ds.Tables[0].Rows[0]["custsageid"].ToString(); }
        else
        { txtCustSageID.Text = string.Empty; }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCustomers(string prefixText, int count, string contextKey)
    {
        Int32 IsSelesAsigned = 0;
        if (HttpContext.Current.Session["type"].ToString() != "am" && HttpContext.Current.Session["type"].ToString() != "c" && HttpContext.Current.Session["MSM"].ToString() != "TS")
        {
            DataTable dsSalesAssigned = new DataTable();
            dsSalesAssigned = (DataTable)HttpContext.Current.Session["userinfo"];
            string SalesAssigned = dsSalesAssigned.Rows[0]["SalesAssigned"] == DBNull.Value ? "0" : dsSalesAssigned.Rows[0]["SalesAssigned"].ToString();
            if (SalesAssigned == "1")
            {
                IsSelesAsigned = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            }
        }
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        DataSet ds = new DataSet();
        //objPropUser.DBName = HttpContext.Current.Session["dbname"].ToString();
        //objPropUser.SearchBy = "Name";
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_User.getCustomerAuto(objPropUser, IsSelesAsigned);
        //ds = objBL_User.getCustomerSearch(objPropUser);

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["Name"].ToString(), row["id"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Getlocation(string prefixText, int count, string contextKey)
    {
        Int32 IsSelesAsigned = 0;
        if (HttpContext.Current.Session["type"].ToString() != "am" && HttpContext.Current.Session["type"].ToString() != "c" && HttpContext.Current.Session["MSM"].ToString() != "TS")
        {
            DataTable dsSalesAssigned = new DataTable();
            dsSalesAssigned = (DataTable)HttpContext.Current.Session["userinfo"];
            string SalesAssigned = dsSalesAssigned.Rows[0]["SalesAssigned"] == DBNull.Value ? "0" : dsSalesAssigned.Rows[0]["SalesAssigned"].ToString();
            if (SalesAssigned == "1")
            {
                IsSelesAsigned = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            }
        }
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        BL_User objBL_User = new BL_User();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        //objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);           
        ds = objBL_User.getLocationByCustomerID(objPropUser, IsSelesAsigned);

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["Name"].ToString(), row["loc"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }

    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        lnkConvert.Visible = false;
        FillLoc();
        hdnFocus.Value = txtLocation.ClientID;
        btnSelectLoc_Click(sender, e);
    }

    //protected void ddlLoc_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //ResetFormControlValues(this);
    //    FillLocInfo();
    //}


    /// <summary>
    /// Fills location data for the location/customer selected.
    /// </summary>
    private void FillLocInfo()
    {
        if (hdnLocId.Value == "")
        {
            return;
        }
        if (hdnProspect.Value != "1")//hdnPatientId.Value != string.Empty
        {
            RequiredFieldValidator1.Enabled = true;
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
            objPropUser.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_User.getLocationByID(objPropUser);
            if (ds.Tables[0].Rows.Count > 0)
            {
                locContractinfo.InnerHtml = "";
                GetContractInfo(Convert.ToInt32(hdnLocId.Value), 0, "Location");
                lblLocHeader.Text = txtLocation.Text = ds.Tables[0].Rows[0]["tag"].ToString();
                HiddenFielLocOnCreditHold.Value = ds.Tables[0].Rows[0]["credit"] == null ? "0" : ds.Tables[0].Rows[0]["credit"].ToString();
                // if the location on credit hold it needs to give a warning and prevent the user
                if (HiddenFielLocOnCreditHold.Value == "1")
                {
                    imgCreditH.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "keyValid14", "noty({text: 'Location on credit hold!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);

                }
                lblAVCHeader.Text = txtAcctno.Text = ds.Tables[0].Rows[0]["id"].ToString();
                txtGoogleAutoc.Text = ds.Tables[0].Rows[0]["LocAddress"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["LocCity"].ToString();
                // ddlState.SelectedValue = ds.Tables[0].Rows[0]["locstate"].ToString();
                ddlState.Text = ds.Tables[0].Rows[0]["locstate"].ToString();
                txtZip.Text = ds.Tables[0].Rows[0]["locZip"].ToString();
                txtCountry.Text = ds.Tables[0].Rows[0]["Country"].ToString();
                //ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["Route"].ToString();             
                txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                txtMaincontact.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                txtCustSageID.Text = ds.Tables[0].Rows[0]["custsageid"].ToString();
                txtCustomer.Text = ds.Tables[0].Rows[0]["custname"].ToString();
                hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                //Set Hyperlink  For Loc / Customer
                if (hdnLocId.Value != "0")
                {
                    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocId.Value;
                    GetDataEquip();
                }
                else
                    lnkLocationID.NavigateUrl = "";
                if (hdnPatientId.Value != "0")
                    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnPatientId.Value;
                else
                    lnkCustomerID.NavigateUrl = "";
                lat.Value = ds.Tables[0].Rows[0]["lat"].ToString();
                lng.Value = ds.Tables[0].Rows[0]["lng"].ToString();
                hdnRolID.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
                //lblDefwork.Text = ds.Tables[0].Rows[0]["defwork"].ToString();
                foreach (ListItem li in ddlDefRoute.Items)
                {
                    if (li.Value == ds.Tables[0].Rows[0]["route"].ToString())
                        ddlDefRoute.SelectedValue = ds.Tables[0].Rows[0]["route"].ToString();
                }
                //zone
                foreach (ListItem li in ddlZone.Items)
                {
                    if (li.Value == ds.Tables[0].Rows[0]["zone"].ToString())
                        ddlZone.SelectedValue = ds.Tables[0].Rows[0]["zone"].ToString();
                }
                txtCreditReason.Text = ds.Tables[0].Rows[0]["creditreason"].ToString();
                chkDispAlert.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DispAlert"]);
                chkCreditHold.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Credit"]);
                //hdnUnitID.Value = string.Empty;
                //txtUnit.Text = string.Empty;
                txtProject.Text = string.Empty;
                projectNo.Text = hdnProjectId.Value = string.Empty;
                lnkProjectID.NavigateUrl = "";
                if (Session["MSM"].ToString() != "TS")
                    lnkConvert.Visible = false;
                // ddlDefRoute.Enabled = true;
                RequiredFieldValidator34.Enabled = true;
            }
            GetDataEquip();
            FillElevUnit();
            GetDataProject();
        }
        else
        {
            SetProspect();
            RequiredFieldValidator1.Enabled = false;
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.ProspectID = Convert.ToInt32(hdnLocId.Value);
            DataSet ds = new DataSet();
            ds = objBL_Customer.getProspectByID(objCustomer);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtAcctno.Text = "--";
                txtCustSageID.Text = "--";
                txtGoogleAutoc.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                // ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                ddlState.Text = ds.Tables[0].Rows[0]["state"].ToString();
                txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                txtCountry.Text = ds.Tables[0].Rows[0]["Country"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                txtMaincontact.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                txtCustomer.Text = ds.Tables[0].Rows[0]["name"].ToString();
                hdnRolID.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                txtLocation.Text = string.Empty;
                //hdnUnitID.Value = string.Empty;
                //txtUnit.Text = string.Empty;
                txtProject.Text = string.Empty;
                projectNo.Text = hdnProjectId.Value = string.Empty;
                lnkProjectID.NavigateUrl = "";
                if (Session["MSM"].ToString() != "TS")
                    lnkConvert.Visible = true;
                lat.Value = ds.Tables[0].Rows[0]["lat"].ToString();
                lng.Value = ds.Tables[0].Rows[0]["lng"].ToString();
                // ddlDefRoute.Enabled = false;
                ddlDefRoute.SelectedIndex = 0;
                RequiredFieldValidator34.Enabled = false;
            }
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
                    case "System.Web.UI.WebControls.HiddenField":
                        ((HiddenField)c).Value = "";
                        break;

                }
            }
        }
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {

        if (!Page.IsValid)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keyValid1", "noty({text: 'Please fill out all required fields marked with an asterisk.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return;
        }

        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            // ddlStatus.Items.Remove(ddlStatus.Items.FindByText("Completed"));
            if (Request.QueryString["id"] == null && ddlStatus.SelectedItem.Value == "4" && ViewState["ADDResolved"].ToString() == "N")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "321124", "noty({text: 'You do not have completed ticket permissions!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true,dismissQueue: true, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                return;
            }
            else if (Request.QueryString["id"] != null && ViewState["EditResolved"].ToString() == "N")
            {
                if (Request.QueryString["st"] == null && ViewState["ddlstatus"].ToString() != ddlStatus.SelectedItem.Value.ToString() && ddlStatus.SelectedItem.Value == "4")
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "321124", "noty({text: 'You do not have completed ticket permissions!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true,dismissQueue: true, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                    return;
                }
                else if (ddlStatus.SelectedItem.Value == "4")
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "321124", "noty({text: 'You do not have completed ticket permissions!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true,dismissQueue: true, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                    return;
                }
            }
        }
        if (hdnProspect.Value == string.Empty)
        {
            RequiredFieldValidator1.Enabled = true;
        }
        else
        {
            RequiredFieldValidator1.Enabled = false;
        }

        /*****************
         validation for prospect ticket completion. prospect need to be converted to customer before ticket completion. 
         If prospect ticket is completed from device (viewstate["comp"]=2) then it need to be redirect to convert process without saving the ticket data. 
         *****************/
        int deviceCompletedConvert = 0;
        if (ViewState["comp"].ToString() == "2" && ViewState["convert"].ToString() == "1")
        {
            deviceCompletedConvert = 1;
        }

        if (hdnProspect.Value != string.Empty && ddlStatus.SelectedValue == "4" && deviceCompletedConvert == 0)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keyProspectComplete", "noty({text: 'Ticket can be completed only for customers. Please convert the selected Lead to Customer first.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return;
        }


        if (!string.IsNullOrEmpty(txtInvoiceNo.Text))
        {
            int invoiceId = 0;

            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Int32.TryParse(txtInvoiceNo.Text, out invoiceId))
            {
                objProp_Contracts.InvoiceID = invoiceId;
            }



        }

        if (!string.IsNullOrEmpty(txtManualInvoice.Text))
        {

            objProp_Contracts.InvoiceIDCustom = txtManualInvoice.Text;

        }


        try
        {
            if (txtEnrTime.Text.Trim() == string.Empty)
            {
                objMapData.EnrouteTime = System.DateTime.MinValue;
                //objMapData.EnrouteTime =Convert.ToDateTime(defaultDate);
            }
            else
            {
                //objMapData.EnrouteTime = Convert.ToDateTime(txtSchDt.Text + " " + txtEnrTime.Text);
                objMapData.EnrouteTime = Convert.ToDateTime(defaultDate + " " + txtEnrTime.Text);
            }

            if (txtOnsitetime.Text.Trim() == string.Empty)
            {
                objMapData.OnsiteTime = System.DateTime.MinValue;
                //objMapData.OnsiteTime = Convert.ToDateTime(defaultDate);
            }
            else
            {
                //objMapData.OnsiteTime = Convert.ToDateTime(txtSchDt.Text + " " + txtOnsitetime.Text);
                objMapData.OnsiteTime = Convert.ToDateTime(defaultDate + " " + txtOnsitetime.Text);
            }

            if (txtComplTime.Text.Trim() == string.Empty)
            {
                objMapData.ComplTime = System.DateTime.MinValue;
                //objMapData.ComplTime = Convert.ToDateTime(defaultDate);
            }
            else
            {
                //objMapData.ComplTime = Convert.ToDateTime(txtSchDt.Text + " " + txtComplTime.Text);
                objMapData.ComplTime = Convert.ToDateTime(defaultDate + " " + txtComplTime.Text);
            }
            if (hdnLocId.Value == string.Empty)
            {
                objMapData.LocID = 0;
            }
            else
            {
                objMapData.LocID = Convert.ToInt32(hdnLocId.Value);
            }
            objMapData.CustomerName = txtCustomer.Text;
            if (hdnPatientId.Value == string.Empty)
            {
                objMapData.CustID = 0;
            }
            else
            {
                objMapData.CustID = Convert.ToInt32(hdnPatientId.Value);
            }
            objMapData.LocTag = txtLocation.Text;
            objMapData.LocAddress = txtGoogleAutoc.Text;
            objMapData.City = txtCity.Text;
            //objMapData.State = ddlState.SelectedValue;
            objMapData.State = ddlState.Text.Trim();
            objMapData.Zip = txtZip.Text;
            objMapData.Phone = txtPhoneCust.Text;
            objMapData.Cell = txtCell.Text;
            objMapData.Worker = ddlRoute.SelectedValue;
            objMapData.CallDate = Convert.ToDateTime(txtCallDt.Text + " " + txtCallTime.Text);

            if (txtSchDt.Text.Trim() != string.Empty)
            {
                objMapData.SchDate = Convert.ToDateTime(txtSchDt.Text + " " + txtSchTime.Text);
            }
            else
            {
                objMapData.SchDate = System.DateTime.MinValue;
            }

            objMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);
            objMapData.Category = ddlCategory.SelectedValue;
            if (hdnUnitID.Value != "")
            {
                objMapData.Unit = Convert.ToInt32(hdnUnitID.Value);
            }

            if (hdnProjectId.Value != "")
            {
                objMapData.jobid = Convert.ToInt32(hdnProjectId.Value);
            }



            if (txtTranslate.Text.Trim() != string.Empty)
            {
                objMapData.Reason = txtReason.Text.Replace('|', ' ') + "|" + txtTranslate.Text.Replace('|', ' ');
            }
            else
            {
                objMapData.Reason = txtReason.Text;
            }

            if (txtEST.Text.Trim() != string.Empty)
            {
                objMapData.EST = Convert.ToDouble(txtEST.Text);
            }
            else
            {
                objMapData.EST = 00.50;
            }
            objMapData.ConnConfig = Session["config"].ToString();

            if (hdnProjectCode.Value != "")
            {
                objMapData.jobcode = hdnProjectCode.Value;
            }


            //Separate the english and spanish text by '|'
            if (txtTransDesc.Text.Trim() != string.Empty)
            {
                objMapData.CompDescription = txtWorkCompl.Text.Replace('|', ' ') + "|" + txtTransDesc.Text.Replace('|', ' ');
            }
            else
            {
                objMapData.CompDescription = txtWorkCompl.Text;
            }

            double RT = Convert.ToDouble(objGeneralFunctions.IsNull(txtRT.Text, "0"));
            double OT = Convert.ToDouble(objGeneralFunctions.IsNull(txtOT.Text, "0"));
            double NT = Convert.ToDouble(objGeneralFunctions.IsNull(txtNT.Text, "0"));
            double TT = Convert.ToDouble(objGeneralFunctions.IsNull(txtTT.Text, "0"));


            double BT = Convert.ToDouble(objGeneralFunctions.IsNull(txtBT.Text, "0"));
            double DT = Convert.ToDouble(objGeneralFunctions.IsNull(txtDT.Text, "0"));



            double ZoneExp = Convert.ToDouble(objGeneralFunctions.IsNull(txtExpZone.Text, "0"));
            double TollExp = Convert.ToDouble(objGeneralFunctions.IsNull(txtExpToll.Text, "0"));
            double MiscExp = Convert.ToDouble(objGeneralFunctions.IsNull(txtExpMisc.Text, "0"));

            objMapData.ZoneExpense = ZoneExp;
            objMapData.TollExpense = TollExp;
            objMapData.MiscExpense = MiscExp;

            objMapData.RT = RT;
            objMapData.OT = OT;
            objMapData.NT = NT;
            objMapData.TT = TT;
            objMapData.BT = BT;
            objMapData.DT = DT;
            objMapData.Total = Convert.ToDouble(txtTotal.Value);
            //objMapData.Total = RT + OT + NT + TT + DT;//////////+BT
            if (!string.IsNullOrEmpty(txtInvoiceNo.Text.Trim()))
            {
                chkJobChargeable.Checked = chkChargeable.Checked = false;
                objMapData.Charge = 0;
            }
            else
            {
                objMapData.Charge = Convert.ToInt32(chkJobChargeable.Checked);
                chkChargeable.Checked = chkJobChargeable.Checked;
            }

            objMapData.Review = Convert.ToInt32(chkReviewed.Checked);
            objMapData.PayRoll = Convert.ToInt32(chkPayroll.Checked);
            objMapData.Who = txtNameWho.Text;
            objMapData.Remarks = txtRemarks.Text;
            if (Session["type"].ToString() == "c")
            {
                objMapData.Level = 99;
            }
            else
            {
                if (ddlLevel.SelectedValue != string.Empty)
                    objMapData.Level = Convert.ToInt32(ddlLevel.SelectedValue);
                else
                    objMapData.Level = 1;
            }
            objMapData.Department = Convert.ToInt32(ddlDepartment.SelectedValue == "" ? "-1" : ddlDepartment.SelectedValue);

            objMapData.PartsUsed = txtPartsUsed.Text;
            objMapData.Comments = txtComments.Text;

            objMapData.Custom1 = txtCst1.Text;
            objMapData.Custom2 = txtCst2.Text;
            objMapData.Custom3 = txtCst3.Text;
            objMapData.Custom4 = txtCst4.Text;
            objMapData.Custom5 = txtCst5.Text;
            objMapData.Custom6 = Convert.ToInt32(chkCst1.Checked);
            objMapData.Custom7 = Convert.ToInt32(chkCst2.Checked);
            objMapData.Workorder = txtWO.Text.Trim();
            objMapData.WorkComplete = Convert.ToInt16(chkWorkComp.Checked);
            objMapData.MileStart = Convert.ToDouble(objGeneralFunctions.IsNull(txtMileStart.Text.ToString(), "0"));
            objMapData.MileEnd = Convert.ToDouble(objGeneralFunctions.IsNull(txtMileEnd.Text.ToString(), "0"));
            objMapData.Internet = Convert.ToInt32(chkInternet.Checked);
            objMapData.ManualInvoiceID = txtManualInvoice.Text.Trim();

            int invoiceId = 0;

            if (Int32.TryParse(txtInvoiceNo.Text, out invoiceId))
            {
                objMapData.InvoiceID = invoiceId;
            }
            objMapData.TimeTransfer = Convert.ToInt32(chkTimeTrans.Checked);
            objMapData.DispAlert = Convert.ToInt16(chkDispAlert.Checked);
            objMapData.CreditHold = Convert.ToInt16(chkCreditHold.Checked);
            objMapData.CreditReason = txtCreditReason.Text.Trim();
            objMapData.IsRecurring = chkIsRecurring.Checked ? 1 : 0;
            //objMapData.InvoiceID = Convert.ToInt32(objGeneralFunctions.IsNull(txtInvoiceNo.Text.Trim(),"0"));
            objMapData.QBServiceID = ddlService.SelectedValue;
            objMapData.QBPayrollID = ddlPayroll.SelectedValue;
            objMapData.LastUpdatedBy = Session["username"].ToString();
            objMapData.MainContact = txtMaincontact.Text.Trim();
            objMapData.Recommendation = txtRecommendation.Text.Trim();
            objMapData.CustomTick1 = txtTickCustom1.Text.Trim();
            objMapData.CustomTick2 = txtTickCustom2.Text.Trim();
            objMapData.CustomTick3 = Convert.ToInt16(chkTickCustom1.Checked);
            objMapData.CustomTick4 = Convert.ToInt16(chkTickCustom2.Checked);
            objMapData.CustomTick5 = txtTickCustom3.Text.Trim();
            //objMapData.CustomTick5 = Convert.ToDouble(objGeneralFunctions.IsNull(txtTickCustom3.Text.Trim(), "0"));
            objMapData.Lat = lat.Value;
            objMapData.Lng = lng.Value;
            if (ddlDefRoute.SelectedValue == string.Empty || hdnPatientId.Value == string.Empty)
                objMapData.DefaultWorker = 0;
            else
                objMapData.DefaultWorker = Convert.ToInt32(ddlDefRoute.SelectedValue);
            if (ddlTemplate.SelectedValue != string.Empty)
                objMapData.JobTemplateID = Convert.ToInt32(ddlTemplate.SelectedValue);
            if (ddlWage.SelectedValue != string.Empty)
                objMapData.WageID = Convert.ToInt32(ddlWage.SelectedValue);
            //zone
            if (ddlZone.SelectedValue == string.Empty)
            {
                objMapData.Zone = 0;
            }
            else
            {
                objMapData.Zone = Convert.ToInt32(ddlZone.SelectedValue);
            }
            objMapData.fBy = txtFby.Text.Trim();
            objMapData.dtEquips = GetElevData();
            if (hdnImg.Value != "")
            {
                string str = hdnImg.Value;
                string last = str.Substring(str.LastIndexOf(',') + 1);
                objMapData.Signature = Convert.FromBase64String(last);
            }

            objMapData.dtTasks = TaskCodes();

            //   Is Create New Project 

            int isCreateJob = 0;

            if (hdnProjectId.Value == "0" || hdnProjectId.Value == "") { int.TryParse(hdnIsCreateJob.Value, out isCreateJob); }

            objMapData.IsCreateJob = isCreateJob;


            //// Inventory 

            DataTable dtTicketINV = GetInventoryForSaveItems();

            objMapData.dtTicketINV = dtTicketINV;

            if (ValidateINV_GRID(dtTicketINV))
            {

                /// End Inventory

                /****** if mode is to update existing record********/
                if (Convert.ToInt32(ViewState["mode"]) == 1)
                {
                    int projectID = 0;
                    objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"]);
                    if (deviceCompletedConvert == 0)
                    {
                        if (ViewState["tsint"].ToString() != "1")/*******For TS integration to work TS and MOM together with MOM while the TS database is converted to MOM*********/
                        {
                            if (Session["MSM"].ToString() == "TS")
                            {
                                projectID = objBL_MapData.UpdateTicketInfoTotalService(objMapData);
                            }
                            else
                            {
                                projectID = objBL_MapData.UpdateTicketInfo(objMapData);
                                SaveDocInfo();
                            }
                        }
                        //else
                        //    projectID = objBL_MapData.UpdateTicketInfoTS(objMapData);
                    }
                    if (hdnProjectId.Value == string.Empty || hdnProjectId.Value == "0")
                    {
                        hdnProjectId.Value = projectID.ToString();
                        if (hdnProjectId.Value != "0")
                            lnkProjectID.NavigateUrl = "addproject.aspx?uid=" + hdnProjectId.Value;
                        else
                            lnkProjectID.NavigateUrl = "";
                        txtProject.Text = projectID.ToString();
                        GetDataProject();
                        GetJobCode(false);
                    }
                    hdnReviewed.Value = Convert.ToInt16(chkReviewed.Checked).ToString();
                    ViewState["ticid"] = objMapData.TicketID;
                    ViewState["assign"] = ddlStatus.SelectedValue;
                    if (hdnloadlogtab.Value == "1") RadGrid_gvLogs.Rebind();
                    if (ViewState["convert"].ToString() == "1")
                    {
                        ConvertProspectWizard();
                    }
                    else
                    {
                        string comp = "0";
                        /***** if status is completed********/
                        if (ddlStatus.SelectedValue == "4")
                        {
                            comp = "1";
                            ddlStatus1.Enabled = ddlStatus.Enabled = false;
                            btnEnroute.Enabled = false;
                            btnOnsite.Enabled = false;
                            btnComplete.Enabled = false;

                        }
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWC", "CheckWorkComplete('" + ViewState["ticid"].ToString() + "'," + Convert.ToInt16(chkWorkComp.Checked) + ",'Ticket updated successfully!','" + comp + "'," + Convert.ToInt16(chkInvoice.Checked) + ");", true);


                        if (ddlStatus.SelectedValue != "4")
                        {
                            string nv = "none";

                            if (Request.QueryString["JobID"] != null && Request.QueryString["JobName"] != null)
                            {
                                nv = "job";
                            }
                            string WorkerName = ddlRoute1.Text.ToString();
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNotyConfirm", "notyConfirm('" + ViewState["ticid"].ToString() + "','" + nv + "', '" + WorkerName + "');", true);
                        }
                        if (txtWO.Text.Trim() == string.Empty)
                        {
                            txtWO.Text = lblTicketnumber.Text;
                        }

                        /******** Fill related tickets list**********/
                        FillRelatedTickets();
                        getProjectTasks();
                        //if (ViewState["workid"].ToString() != ddlRoute.SelectedValue)
                        //{
                        //SendPushNotification();
                        //}        
                        if (ddlStatus.SelectedValue == "4")
                        {
                            GetOpportTicket(objMapData.TicketID, true);
                        }
                    }

                }
                /******** when mode is add new record **********/
                else
                {
                    string TicketsID = string.Empty;
                    if (ViewState["tsint"].ToString() != "1")
                    {
                        if (chkCreateMultipleTicket.Checked)
                        {

                            objMapData.Status = Convert.ToInt32(ddlStatus.SelectedValue);

                            string selectedWorker = string.Empty;



                            foreach (RadComboBoxItem item in chkcatlist.CheckedItems)
                            {
                                if (item.Checked == true)
                                {
                                    selectedWorker += item.Value + ",";
                                }
                            }

                            objMapData.Worker = selectedWorker.Trim();

                            if (txtDays.Text.Trim() != string.Empty)
                                objMapData.days = Convert.ToInt16(txtDays.Text.Trim());
                            objBL_MapData.AddMultipleTicket(objMapData);
                            TicketsID = "Ticket added successfully!";
                        }
                        else
                        {
                            if (Session["MSM"].ToString() == "TS")
                            { objBL_MapData.AddTicketTotalService(objMapData); }
                            else
                            {
                                objBL_MapData.AddTicket(objMapData);
                                SaveDocInfo();
                            }
                            TicketsID = "Ticket added successfully! Ticket# : " + objMapData.TicketID.ToString();


                        }
                    }
                    //else
                    //    objBL_MapData.AddTicketTS(objMapData);


                    ViewState["ticid"] = objMapData.TicketID;
                    ViewState["assign"] = ddlStatus.SelectedValue;

                    /***********Update temp document in documents table with the ticket id created***********/
                    UpdateDoc();

                    if (ViewState["convert"].ToString() == "1")
                    {
                        ConvertProspectWizard();
                    }
                    else
                    {
                        string comp = "0";
                        if (ddlStatus.SelectedValue == "4")
                        {
                            comp = "1";
                        }
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWC", "CheckWorkComplete('" + ViewState["ticid"].ToString() + "'," + Convert.ToInt16(chkWorkComp.Checked) + ",'" + TicketsID + "','" + comp + "',0);", true);

                        if (ddlStatus.SelectedValue != "4" && chkCreateMultipleTicket.Checked == false)
                        {
                            string nv = "none";

                            if (Request.QueryString["JobID"] != null && Request.QueryString["JobName"] != null)
                            {
                                nv = "job";
                            }
                            string WorkerName = ddlRoute1.Text.ToString();
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNotyConfirm", "notyConfirm('" + ViewState["ticid"].ToString() + "','" + nv + "','" + WorkerName + "');", true);
                        }
                        else
                        {


                            if (Request.QueryString["JobID"] != null && Request.QueryString["JobName"] != null)
                            {
                                string nv = "job";
                                string WorkerName = ddlRoute1.Text.ToString();
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNotyConfirm", "notyConfirm('" + ViewState["ticid"].ToString() + "','" + nv + "','" + WorkerName + "');", true);
                            }


                        }

                        if (hdnIsAddPO.Value == "1")
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "NavigatetoAddPO", "RedirectToAddPoScreen(" + objMapData.TicketID.ToString() + "," + comp + ");", true);
                        }

                        ResetFormControlValues(this);
                        #region Multiple Ticket
                        lnkCustomerID.NavigateUrl = "";
                        lnkLocationID.NavigateUrl = "";
                        divForCreateMultipleTicket2.Visible = divForCreateMultipleTicket.Visible = false;
                        divForCreateMultipleTicket3.Visible = true;
                        ddlRoute1.Visible = ddlRoute.Visible = true;
                        foreach (RadComboBoxItem li in chkcatlist.Items)
                        { li.Checked = false; }
                        ddlStatus1.Enabled = ddlStatus.Enabled = true;
                        #endregion
                        txtCallDt.Text = System.DateTime.Now.ToShortDateString();
                        txtCallTime.Text = System.DateTime.Now.ToShortTimeString();
                        txtSchDt.Text = System.DateTime.UtcNow.ToShortDateString();
                        txtSchTime.Text = System.DateTime.UtcNow.ToShortTimeString();
                        lblHeader.Text = "Add Ticket";
                        RadgvEquip.DataSource = null;
                        RadgvEquip.Rebind();
                        RadGVContractInfo.DataSource = null;
                        RadGVContractInfo.Rebind();
                        locContractinfo.InnerHtml = "";
                        lstRecentCalls.DataSource = null;
                        lstRecentCalls.DataBind();
                        RadgvDocuments.DataSource = null;
                        RadgvDocuments.Rebind();
                        RadgvMCPDetails.DataSource = null;
                        RadgvMCPDetails.Rebind();
                        RadgvProject.DataSource = null;
                        RadgvProject.Rebind();
                        RadgvProjectCode.DataSource = null;
                        RadgvProjectCode.Rebind();
                        RadGvlstRelatedTickets.DataSource = null;
                        RadGvlstRelatedTickets.DataBind();
                        if (hdnloadlogtab.Value == "1") RadGrid_gvLogs.Rebind();
                        lat.Value = string.Empty;
                        lng.Value = string.Empty;
                        lnkbtnlocContractinfo.Visible = false;
                        chkWorkComp.Checked = true;
                        RadGVPO.DataSource = null;
                        RadGVPO.Rebind();
                        lnkOpport.NavigateUrl = "";
                        lnkOpport.Visible = false;
                        lnkInvoice.Visible = false;
                        hdnIsCreateJob.Value = "0";


                        //SendPushNotification();
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNW", "CallNearestWorker();", true);
                    }
                }
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh')) window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh').click();}", true);
                success = true;

                HiddenFieldRT.Value = txtRT.Text;
                HiddenFieldOT.Value = txtOT.Text;
                HiddenFieldNT.Value = txtNT.Text;
                HiddenFieldDT.Value = txtDT.Text;
                HiddenFieldTT.Value = txtTT.Text;
                HiddenFieldBT.Value = txtBT.Text;
                if (!string.IsNullOrEmpty(txtInvoiceNo.Text))
                {
                    lnkInvoice.Visible = true;
                    ChangeImage();
                    lnkInvoice.Attributes["onclick"] = "window.open('addinvoice.aspx?o=1&uid=" + txtInvoiceNo.Text + "', 'Invoice', 'height=768,width=1280,scrollbars=yes');";
                    chkChargeable.Checked = false;
                    chkJobChargeable.Checked = false;
                }
                else
                {
                    lnkInvoice.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message;
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string ErrorType = "error";
            if (str.Contains("No labor type item exist for the project") == true) { ErrorType = "warning"; }
            if (str == "RAISE_ERROR_1")
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr1", "RefressTicketPage();", true);
                success = false;
            }
            else
            {

                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : '" + ErrorType + "', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                success = false;
            }
        }
    }

    private DataTable GetElevData()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("ticket_id", typeof(int));
            dt.Columns.Add("elev_id", typeof(int));
            dt.Columns.Add("labor_percentage", typeof(double));

            foreach (GridDataItem gvr in RadgvEquip.Items)
            {
                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");

                if (chkSelect.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    Label lblUnit = (Label)gvr.FindControl("lblID");
                    TextBox txtHours = (TextBox)gvr.FindControl("txtHours");
                    dr["ticket_id"] = 0;
                    dr["elev_id"] = Convert.ToInt32(lblUnit.Text);
                    if (txtHours.Text.Trim() != string.Empty)
                    {
                        dr["labor_percentage"] = Convert.ToDouble(txtHours.Text);
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        catch { }
        return dt;
    }

    protected void gvEquip_DataBound(object sender, EventArgs e)
    {
        GridColumn bType = (GridColumn)RadgvEquip.Columns[4];
        bType.HeaderText = "Type";
        if (Session["EquipTypeLabel"] != null)
        {
            bType.HeaderText = Convert.ToString(Session["EquipTypeLabel"]);
        }
        GridColumn bCat = (GridColumn)RadgvEquip.Columns[5];
        bCat.HeaderText = "Category";
        if (Session["EquipCatLabel"] != null)
        {
            bCat.HeaderText = Convert.ToString(Session["EquipCatLabel"]);
        }
        GridColumn bf = (GridColumn)RadgvEquip.Columns[7];
        bf.HeaderText = "Building";
        if (Session["EquipLabel"] != null)
        {
            bf.HeaderText = Convert.ToString(Session["EquipLabel"]);
        }
    }

    /// <summary>
    /// update location coordinates for the seleted address for location
    /// </summary>
    private void updateCoordinates()
    {
        if (hdnRolID.Value != string.Empty)
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.RolId = Convert.ToInt32(hdnRolID.Value);
            objPropUser.Lat = lat.Value;
            objPropUser.Lng = lng.Value;

            objBL_User.UpdateRolCoordinates(objPropUser);
        }
    }

    private void SendPushNotification()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        objGeneral.EmpID = ddlRoute.SelectedValue;
        string str = objBL_General.GetRegID(objGeneral);
        if (str != string.Empty && str != null)
        {
            //obj_PushNotification.Android();
        }
    }

    private void ChangeStatus()
    {
        /*****unassigned **********/
        if (ddlStatus.SelectedValue == "0")
        {
            ddlRoute1.SelectedValue = ddlRoute.SelectedValue = "";
            ChangeRoute();

            txtEnrTime.Text = string.Empty;
            txtComplTime.Text = string.Empty;
            txtOnsitetime.Text = string.Empty;


            ddlRoute1.Enabled = ddlRoute.Enabled = true;
            txtEnrTime.Enabled = false;
            txtOnsitetime.Enabled = false;
            txtComplTime.Enabled = false;

            RequiredFieldValidator_txtSchTime.Enabled = false;
            // MaskedEditValidator3.Enabled = false;
            RequiredFieldValidator24.Enabled = false;
            RequiredFieldValidator25.Enabled = false;
            RequiredFieldValidator26.Enabled = false;
            RequiredFieldValidator29.Enabled = false;
            RequiredFieldValidator27.Enabled = false;
            RequiredFieldValidator28.Enabled = false;
            RequiredFieldValidator32.Visible = false;

            if (ViewState["JobCostLabor"].ToString() != string.Empty && ViewState["JobCostLabor"].ToString() != "0")
            {
                RequiredFieldVWage.Enabled = false;
                ValidatorCVWage.Enabled = false;
            }

            txtWorkCompl.Enabled = false;
            txtRecommendation.Enabled = false;
            txtWorkCompl.Text = string.Empty;
            lblWCD.Enabled = false;
            btnCodesCmpl.Visible = false;
            divTransicon.Visible = false;

            txtRT.Enabled = false;
            txtDT.Enabled = false;
            txtOT.Enabled = false;
            txtNT.Enabled = false;
            txtTT.Enabled = false;
            txtBT.Enabled = false;

            txtRT.Text = "0.00";
            txtDT.Text = "0.00";
            txtOT.Text = "0.00";
            txtNT.Text = "0.00";
            txtTT.Text = "0.00";
            txtBT.Text = "0.00";

            txtPartsUsed.Enabled = false;
            txtComments.Enabled = false;

            txtExpMisc.Enabled = false;
            txtExpZone.Enabled = false;
            txtExpToll.Enabled = false;

            txtMileEnd.Enabled = false;
            txtMileStart.Enabled = false;

            chkInternet.Enabled = false;
            chkWorkComp.Enabled = false;
            chkReviewed.Enabled = false;
            chkPayroll.Enabled = false;
            chkInvoice.Enabled = false;
            txtInvoiceNo.Enabled = false;
            chkChargeable.Enabled = false;            //chkJobChargeable.Enabled = false;
            chkTimeTrans.Enabled = false;
            //ddlService.Enabled = false;
            //ddlPayroll.Enabled = false; 
        }
        /***********assigned********/
        else if (ddlStatus.SelectedValue == "1")
        {
            txtEnrTime.Text = string.Empty;
            txtComplTime.Text = string.Empty;
            txtOnsitetime.Text = string.Empty;

            ddlRoute1.Enabled = ddlRoute.Enabled = true;
            txtEnrTime.Enabled = false;
            txtOnsitetime.Enabled = false;
            txtComplTime.Enabled = false;

            //MaskedEditValidator3.Enabled = true;
            RequiredFieldValidator_txtSchTime.Enabled = true;
            RequiredFieldValidator24.Enabled = true;
            RequiredFieldValidator25.Enabled = true;
            RequiredFieldValidator26.Enabled = false;
            RequiredFieldValidator29.Enabled = false;
            RequiredFieldValidator27.Enabled = false;
            RequiredFieldValidator28.Enabled = false;
            RequiredFieldValidator32.Visible = false;
            if (ViewState["JobCostLabor"].ToString() != string.Empty && ViewState["JobCostLabor"].ToString() != "0")
            {
                RequiredFieldVWage.Enabled = false;
                ValidatorCVWage.Enabled = false;
            }

            txtRecommendation.Enabled = false;
            txtWorkCompl.Enabled = false;
            txtWorkCompl.Text = string.Empty;
            lblWCD.Enabled = false;
            btnCodesCmpl.Visible = false;
            divTransicon.Visible = false;

            txtRT.Enabled = false;
            txtDT.Enabled = false;
            txtOT.Enabled = false;
            txtNT.Enabled = false;
            txtTT.Enabled = false;
            txtBT.Enabled = false;


            txtRT.Text = "0.00";
            txtDT.Text = "0.00";
            txtOT.Text = "0.00";
            txtNT.Text = "0.00";
            txtTT.Text = "0.00";
            txtBT.Text = "0.00";


            txtPartsUsed.Enabled = false;
            txtComments.Enabled = false;

            txtExpMisc.Enabled = false;
            txtExpZone.Enabled = false;
            txtExpToll.Enabled = false;

            txtMileEnd.Enabled = false;
            txtMileStart.Enabled = false;

            chkInternet.Enabled = false;
            chkWorkComp.Enabled = false;
            chkReviewed.Enabled = false;
            chkPayroll.Enabled = false;
            chkInvoice.Enabled = false;
            txtInvoiceNo.Enabled = false;
            chkChargeable.Enabled = false;  //chkJobChargeable.Enabled = false;
            chkTimeTrans.Enabled = false;
            //ddlService.Enabled = false;
            //ddlPayroll.Enabled = false; 
        }
        /*********enroute***********/
        else if (ddlStatus.SelectedValue == "2")
        {
            txtEnrTime.Text = System.DateTime.Now.ToShortTimeString();
            //txtEnrTime.Focus();

            txtComplTime.Text = string.Empty;
            txtOnsitetime.Text = string.Empty;

            ddlRoute1.Enabled = ddlRoute.Enabled = true;
            txtEnrTime.Enabled = true;
            txtOnsitetime.Enabled = false;
            txtComplTime.Enabled = false;

            //  MaskedEditValidator3.Enabled = true;
            RequiredFieldValidator_txtSchTime.Enabled = true;
            RequiredFieldValidator24.Enabled = true;
            RequiredFieldValidator25.Enabled = true;
            RequiredFieldValidator26.Enabled = true;
            RequiredFieldValidator29.Enabled = false;
            RequiredFieldValidator27.Enabled = false;
            RequiredFieldValidator28.Enabled = false;
            RequiredFieldValidator32.Visible = false;
            if (ViewState["JobCostLabor"].ToString() != string.Empty && ViewState["JobCostLabor"].ToString() != "0")
            {
                RequiredFieldVWage.Enabled = false;
                ValidatorCVWage.Enabled = false;
            }

            txtRecommendation.Enabled = false;
            txtWorkCompl.Enabled = false;
            txtWorkCompl.Text = string.Empty;
            lblWCD.Enabled = false;
            btnCodesCmpl.Visible = false;
            divTransicon.Visible = false;

            txtRT.Enabled = false;
            txtDT.Enabled = false;
            txtOT.Enabled = false;
            txtNT.Enabled = false;
            txtTT.Enabled = false;
            txtBT.Enabled = false;

            txtRT.Text = "0.00";
            txtDT.Text = "0.00";
            txtOT.Text = "0.00";
            txtNT.Text = "0.00";
            txtTT.Text = "0.00";
            txtBT.Text = "0.00";

            txtPartsUsed.Enabled = false;
            txtComments.Enabled = false;

            txtExpMisc.Enabled = false;
            txtExpZone.Enabled = false;
            txtExpToll.Enabled = false;

            txtMileEnd.Enabled = false;
            txtMileStart.Enabled = false;

            chkInternet.Enabled = false;
            chkWorkComp.Enabled = false;
            chkReviewed.Enabled = false;
            chkPayroll.Enabled = false;
            chkInvoice.Enabled = false;
            txtInvoiceNo.Enabled = false;
            chkChargeable.Enabled = false;  //chkJobChargeable.Enabled = false;
            chkTimeTrans.Enabled = false;
            //ddlService.Enabled = false;
            //ddlPayroll.Enabled = false; 
        }
        /***********onsite************/
        else if (ddlStatus.SelectedValue == "3")
        {

            txtOnsitetime.Text = System.DateTime.Now.ToShortTimeString();

            if (txtEnrTime.Text == string.Empty)
            {
                txtEnrTime.Text = System.DateTime.Now.ToShortTimeString();
            }
            txtComplTime.Text = string.Empty;

            ddlRoute1.Enabled = ddlRoute.Enabled = true;

            txtEnrTime.Enabled = true;
            txtOnsitetime.Enabled = true;
            txtComplTime.Enabled = false;

            // MaskedEditValidator3.Enabled = true;
            RequiredFieldValidator_txtSchTime.Enabled = true;
            RequiredFieldValidator24.Enabled = true;
            RequiredFieldValidator25.Enabled = true;
            RequiredFieldValidator26.Enabled = true;
            RequiredFieldValidator29.Enabled = true;
            RequiredFieldValidator27.Enabled = false;
            RequiredFieldValidator28.Enabled = false;
            RequiredFieldValidator32.Visible = false;
            if (ViewState["JobCostLabor"].ToString() != string.Empty && ViewState["JobCostLabor"].ToString() != "0")
            {
                RequiredFieldVWage.Enabled = false;
                ValidatorCVWage.Enabled = false;
            }
            txtRecommendation.Enabled = false;
            txtWorkCompl.Enabled = false;
            txtWorkCompl.Text = string.Empty;
            lblWCD.Enabled = false;
            btnCodesCmpl.Visible = false;
            divTransicon.Visible = false;

            txtRT.Enabled = false;
            txtDT.Enabled = false;
            txtOT.Enabled = false;
            txtNT.Enabled = false;
            txtTT.Enabled = false;
            txtBT.Enabled = false;

            txtRT.Text = "0.00";
            txtDT.Text = "0.00";
            txtOT.Text = "0.00";
            txtNT.Text = "0.00";
            txtTT.Text = "0.00";
            txtBT.Text = "0.00";


            txtPartsUsed.Enabled = false;
            txtComments.Enabled = false;

            txtExpMisc.Enabled = false;
            txtExpZone.Enabled = false;
            txtExpToll.Enabled = false;
            txtMileEnd.Enabled = false;
            txtMileStart.Enabled = false;

            chkInternet.Enabled = false;
            chkWorkComp.Enabled = false;
            chkReviewed.Enabled = false;
            chkPayroll.Enabled = false;
            chkInvoice.Enabled = false;
            txtInvoiceNo.Enabled = false;
            chkChargeable.Enabled = false; //chkJobChargeable.Enabled = false;
            chkTimeTrans.Enabled = false;

            //ddlService.Enabled = false;
            //ddlPayroll.Enabled = false;            
        }
        /********completed***********/
        else if (ddlStatus.SelectedValue == "4")
        {
            if (txtEnrTime.Text == string.Empty)
            {
                txtEnrTime.Text = System.DateTime.Now.ToShortTimeString();
            }

            if (txtOnsitetime.Text == string.Empty)
            {
                txtOnsitetime.Text = System.DateTime.Now.ToShortTimeString();
            }

            if (txtComplTime.Text == string.Empty)
            {
                txtComplTime.Text = System.DateTime.Now.ToShortTimeString();
                txtComplTime.Focus();
            }


            ddlRoute1.Enabled = ddlRoute.Enabled = true;
            txtEnrTime.Enabled = true;
            txtOnsitetime.Enabled = true;
            txtComplTime.Enabled = true;

            //  MaskedEditValidator3.Enabled = true;
            RequiredFieldValidator_txtSchTime.Enabled = true;
            RequiredFieldValidator24.Enabled = true;
            RequiredFieldValidator25.Enabled = true;
            RequiredFieldValidator26.Enabled = true;
            RequiredFieldValidator29.Enabled = true;
            RequiredFieldValidator27.Enabled = true;
            RequiredFieldValidator28.Enabled = true;
            RequiredFieldValidator32.Visible = true;
            if (ViewState["JobCostLabor"].ToString() != string.Empty && ViewState["JobCostLabor"].ToString() != "0")
            {
                RequiredFieldVWage.Enabled = true;
                ValidatorCVWage.Enabled = true;
            }
            txtRecommendation.Enabled = true;
            txtWorkCompl.Enabled = true;
            lblWCD.Enabled = true;
            btnCodesCmpl.Visible = true;
            int Multilang = Convert.ToInt16(Session["IsMultiLang"]);
            if (Multilang != 0)
            {
                divTransicon.Visible = true;
            }

            txtRT.Enabled = true;
            txtDT.Enabled = true;
            txtOT.Enabled = true;
            txtNT.Enabled = true;
            txtTT.Enabled = true;
            txtBT.Enabled = true;

            txtPartsUsed.Enabled = true;
            txtComments.Enabled = true;

            txtExpMisc.Enabled = true;
            txtExpZone.Enabled = true;
            txtExpToll.Enabled = true;

            txtMileEnd.Enabled = true;
            txtMileStart.Enabled = true;

            chkInternet.Enabled = true;
            chkWorkComp.Enabled = true;
            chkReviewed.Enabled = true;
            chkPayroll.Enabled = true;
            chkInvoice.Enabled = true;
            txtInvoiceNo.Enabled = true;
            chkChargeable.Enabled = true; //chkJobChargeable.Enabled = true;
            FillChargeableFromJob();
            chkTimeTrans.Enabled = true;
            ////ddlService.Enabled = true;
            ////ddlPayroll.Enabled = true;
            //FillChargeableFromCategory();
            //if (ViewState["internetdefault"].ToString() == "1" && chkReviewed.Checked == false && ddlStatus.SelectedValue == "4")
            //    chkInternet.Checked = true;


            // Control.MSTimeDataFieldVisibility (16) (Y/N) [YYYYYYYYYYYYYYYY]   [RT, OT, NT, DT, TT, TRT, TOT, TNT, TDT, Smile, EMile, Zone, Toll, Misc, Break, PartsUsed]

            string st = MSTimeDataFieldVisibility.Value;
            if (ddlStatus.SelectedValue == "4")
            {
                if (st.Length > 0)
                {
                    if (st.Substring(0, 1) == "N")
                    {
                        txtRT.Enabled = false;
                        txtRT.Text = "0";
                    }
                }

                if (st.Length > 1)
                {
                    if (st.Substring(1, 1) == "N")
                    {
                        txtOT.Enabled = false;
                        txtOT.Text = "0";
                    }
                }

                if (st.Length > 2)
                {
                    if (st.Substring(2, 1) == "N")
                    {
                        txtNT.Enabled = false;
                        txtNT.Text = "0";
                    }
                }

                if (st.Length > 3)
                {
                    if (st.Substring(3, 1) == "N")
                    {
                        txtDT.Enabled = false;
                        txtDT.Text = "0";
                    }
                }

                if (st.Length > 4)
                {
                    if (st.Substring(4, 1) == "N")
                    {
                        txtTT.Enabled = false;
                        HiddenFieldTT.Value = txtTT.Text = "0";

                    }
                }

                if (st.Length > 5)
                {
                    if (st.Substring(5, 1) == "N")
                    {
                        // TRT 
                    }
                }

                if (st.Length > 6)
                {
                    if (st.Substring(6, 1) == "N")
                    {
                        // TOT 
                    }
                }

                if (st.Length > 7)
                {
                    if (st.Substring(7, 1) == "N")
                    {
                        // TNT 
                    }
                }

                if (st.Length > 8)
                {
                    if (st.Substring(8, 1) == "N")
                    {
                        // TDT 

                    }
                }

                if (st.Length > 9)
                {
                    if (st.Substring(9, 1) == "N")
                    {
                        txtMileStart.Enabled = false;

                    }
                }

                if (st.Length > 10)
                {
                    if (st.Substring(10, 1) == "N")
                    {
                        txtMileEnd.Enabled = false;

                    }
                }

                if (st.Length > 11)
                {
                    if (st.Substring(11, 1) == "N")
                    {
                        // Zone 
                        txtExpZone.Enabled = false;

                    }
                }

                if (st.Length > 12)
                {
                    if (st.Substring(12, 1) == "N")
                    {
                        // Toll 
                        txtExpToll.Enabled = false;

                    }
                }

                if (st.Length > 13)
                {
                    if (st.Substring(13, 1) == "N")
                    {
                        // Misc 
                        txtExpMisc.Enabled = false;
                    }
                }

                if (st.Length > 14)
                {
                    if (st.Substring(14, 1) == "N")
                    {
                        txtBT.Enabled = false;
                        txtBT.Text = "0";
                    }
                }

                if (st.Length > 15)
                {
                    if (st.Substring(15, 1) == "N")
                    {
                        // PartsUsed 
                        txtPartsUsed.Enabled = false;

                    }
                }
            }



            try
            {
                if (ddlWage.Items.Count == 2 && ddlRoute1.SelectedIndex != 0 && ddlStatus.SelectedValue == "4" && ddlWage.SelectedIndex == 0)
                {
                    ddlWage.SelectedIndex = 1;
                }
                else if (hdnProjectwageID.Value != "" && ddlRoute1.SelectedIndex != 0 && ddlWage.SelectedIndex == 0)
                {
                    string str = hdnProjectwageID.Value.ToString();

                    ListItem item1 = ddlWage.Items.FindByValue(str);

                    if (item1 != null)
                    {
                        ddlWage.SelectedValue = str;
                    }
                }
            }
            catch { }



        }
    }

    private string TicketStatusColor(string Status)
    {
        string Color = "White";
        if (Status == "1") Color = "White";
        if (Status == "2") Color = "rgb(158, 247, 103)";
        if (Status == "3") Color = "Orange";
        if (Status == "4") Color = "DeepSkyBlue";
        if (Status == "5") Color = "Yellow";
        if (Status == "6") Color = "Yellow";
        return Color;
    }

    protected void ddlStatus1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlStatus.SelectedIndex = ddlStatus1.SelectedIndex;
        ddlStatus_SelectedIndexChanged(sender, e);
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlStatus1.SelectedIndex = ddlStatus.SelectedIndex;

        //if (ddlStatus.SelectedItem.Value == "4") { accrdcompleted.Visible = true; LI_Completed.Disabled = false; }
        //else { accrdcompleted.Visible = false; LI_Completed.Disabled = true; }

        /*******Integrate*****/
        Statustooltipped.Style.Add("background-color", TicketStatusColor(ddlStatus.SelectedItem.Value));
        Statustooltipped.Attributes.Add("data-tooltip", ddlStatus.SelectedItem.Text);
        if (ViewState["tsint"].ToString() != "1")
        {
            //if (ddlStatus.SelectedValue == "4" && Session["MSM"].ToString() == "TS")
            //{
            //    ScriptManager.RegisterStartupScript(this,this.GetType(),"keyCompleteDiable","noty({text: 'Ticket can be completed only from Total Service.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});",true);
            //    ddlStatus.SelectedValue = "0";
            //    ChangeStatus();
            //    return;
            //}
        }

        if (hdnProspect.Value != string.Empty && ddlStatus.SelectedValue == "4")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyCompleteDisable", "noty({text: 'Ticket can be completed only from Customers.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            ddlStatus1.SelectedValue = ddlStatus.SelectedValue = "1";
            ChangeStatus();
            return;
        }

        ChangeStatus();
        ShowQBSyncControls();
        FillChargeableFromCategory();
        /********set focus***********/
        hdnFocus.Value = ddlStatus.ClientID;

        if (txtJobCode.Text == string.Empty && ddlStatus.SelectedValue == "4")
        {
            foreach (GridDataItem gr in RadgvProjectCode.Items)
            {
                Label lblIDname = (Label)gr.FindControl("lblIDname");
                Label lblIDname1 = (Label)gr.FindControl("lblIDname1");
                hdnProjectCode.Value = lblIDname.Text;
                txtJobCode.Text = lblIDname1.Text;
                break;
            }
        }
        //For Multiple Ticket
        if (ddlStatus.SelectedValue == "0" & chkCreateMultipleTicket.Checked == true)
        {
            ValidatorCalloutExtender11.Enabled = false;
            RequiredFieldValidator3.Enabled = false;
        }
        else if (ddlStatus.SelectedValue != "0" & chkCreateMultipleTicket.Checked == true)
        {
            ValidatorCalloutExtender11.Enabled = true;
            RequiredFieldValidator3.Enabled = true;
        }
    }


    protected void lnkddlStatus_Click(object sender, EventArgs e)
    {
        ddlStatus_SelectedIndexChanged(sender, e);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ERPress", " calculate_Time();", true);
    }

    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        FillLocInfo();
        if (hdnLocId.Value != "")
        {
            FillRecentCalls(Convert.ToInt32(hdnLocId.Value));
        }
        hdnFocus.Value = txtGoogleAutoc.ClientID;
        selectEquip();
        // RadgvEquip.Rebind();
        RadgvProject.Rebind();
        RadgvProjectCode.Rebind();
        RadAjaxPanelHeader.Visible = true;
        GetARByLocation();
    }

    private void selectEquip()
    {
        List<string> EquipIDList = new List<string>();
        string EquipID = "";
        string ViewStateName = "VS" + hdnLocId.Value;
        if (Session["RefreshAddticketScreen"] != null)
        {
            if (Request.QueryString["id"] == null) { txtUnit.Text = string.Empty; }
            else { selectEquip(Convert.ToInt32(Request.QueryString["id"].ToString())); }

            EquipID = Session["RefreshAddticketScreen"].ToString();
            Session["RefreshAddticketScreen"] = null;
            if (ViewState[ViewStateName] == null)
            {

                EquipIDList.Add(EquipID);
                ViewState[ViewStateName] = EquipIDList;
            }
            else
            {
                EquipIDList = (List<string>)ViewState[ViewStateName];
                EquipIDList.Add(EquipID);
                ViewState[ViewStateName] = EquipIDList;
            }
        }
        foreach (GridDataItem gr in RadgvEquip.Items)
        {

            Label lblID = (Label)gr.FindControl("lblID");
            Label lblUnit = (Label)gr.FindControl("lblUnit");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (lblID.Text == hdnUnitID.Value)
            {
                chkSelect.Checked = true;
            }

            if (EquipIDList.Contains(lblID.Text))
            {
                chkSelect.Checked = true;
                if (txtUnit.Text != string.Empty) { txtUnit.Text += "," + lblUnit.Text; }
                else { txtUnit.Text = lblUnit.Text; }
            }

        }

    }

    //protected void lnkPrint_Click(object sender,EventArgs e)
    //{
    //    string stc = string.Empty;
    //    bool doRedirect = false;
    //    int TicketID = 0;

    //    if (ViewState["readonly"] != null)
    //    {
    //        if (Request.QueryString["comp"] != null)
    //        {
    //            stc = Request.QueryString["comp"].ToString();
    //        }
    //        doRedirect = true;
    //        TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
    //    }
    //    else
    //    {
    //        lnkSave_Click(sender,e);

    //        if (success == true)
    //        {
    //            if (Request.QueryString["comp"] != null)
    //            {
    //                stc = Request.QueryString["comp"].ToString();
    //            }

    //            if (ViewState["assign"].ToString() == "4")
    //            {
    //                stc = "1";
    //            }
    //            else
    //            {
    //                stc = "0";
    //            }
    //            doRedirect = true;
    //            TicketID = objMapData.TicketID;
    //        }
    //    }

    //    if (doRedirect)
    //    {
    //        //if (Request.QueryString["pop"] == null)
    //        Response.Redirect("Printticket.aspx?id=" + TicketID + "&c=" + stc);
    //        //else
    //        //    Response.Redirect("Printticket.aspx?id=" + TicketID + "&c=" + stc + "&pop=1");
    //    }
    //}

    protected void btnEnroute_Click(object sender, EventArgs e)
    {
        ddlStatus1.SelectedValue = ddlStatus.SelectedValue = "2";
        ChangeStatus();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ERPress", " calculate_Time();", true);
        hdnFocus.Value = txtEnrTime.ClientID;

    }

    protected void btnOnsite_Click(object sender, EventArgs e)
    {
        ddlStatus1.SelectedValue = ddlStatus.SelectedValue = "3";
        ChangeStatus();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "osPress", " calculate_Time();", true);
        hdnFocus.Value = txtOnsitetime.ClientID;
    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {
        if (hdnProspect.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyCompleteDisable", "noty({text: 'Ticket can be completed only from Customers.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            return;
        }

        ddlStatus1.SelectedValue = ddlStatus.SelectedValue = "4";
        ChangeStatus();
        FillChargeableFromCategory();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "compPress", " calculate_Time();", true);
        hdnFocus.Value = txtComplTime.ClientID;
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        lnkSave_Click(sender, e);
        string stc = "";
        stc = Request.QueryString["comp"].ToString();

        if (ddlStatus.SelectedValue == "4")
        {
            stc = "1";
        }
        if (Request.QueryString["fr"] != null && !string.IsNullOrEmpty(Request.QueryString["fr"]))
            Response.Redirect("PrintTicketEIR.aspx?id=" + objMapData.TicketID + "&c=" + stc + "&fr=" + Request.QueryString["fr"]);
        else
            Response.Redirect("PrintTicketEIR.aspx?id=" + objMapData.TicketID + "&c=" + stc);

    }

    protected void btnDone_Click(object sender, EventArgs e)
    {
        string str = string.Empty;
        if (ViewState["codes"].ToString() == "0")
        {
            str = txtReason.Text;
        }
        else
        {
            str = txtWorkCompl.Text;
        }

        for (int item = 0; item < chklstCodes.Items.Count; item++)
        {
            if (chklstCodes.Items[item].Selected == true)
            {
                //if (item == 0)
                //{
                if (str != string.Empty)
                {
                    str += ", ";
                }
                //}
                //if (item != chklstCodes.Items.Count - 1)
                //{
                //    str += chklstCodes.Items[item].Text ;
                //}
                //else
                //{
                str += chklstCodes.Items[item].Text;
                //}
            }
        }
        if (ViewState["codes"].ToString() == "0")
        {
            txtReason.Text = str;
            //txtReason.Focus();
            hdnFocus.Value = txtReason.ClientID;

        }
        else
        {
            txtWorkCompl.Text = str;
            //txtWorkCompl.Focus();
            hdnFocus.Value = txtWorkCompl.ClientID;
        }
        pnlCodes.Visible = false;
        chklstCodes.SelectedIndex = -1;

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlCodes.Visible = false;
        chklstCodes.SelectedIndex = -1;
        if (ViewState["codes"].ToString() == "0")
        {
            //txtReason.Focus();
            hdnFocus.Value = txtReason.ClientID;
        }
        else
        {
            //txtWorkCompl.Focus();
            hdnFocus.Value = txtWorkCompl.ClientID;
        }
    }

    protected void btnMail_Click(object sender, EventArgs e)
    {
        string stc = "";
        if (Request.QueryString["comp"] != null)
        {
            stc = Request.QueryString["comp"].ToString();
        }

        if (ViewState["assign"].ToString() == "4")
        {
            stc = "1";
        }
        else
        {
            stc = "0";
        }

        //if (Request.QueryString["pop"] == null)
        if (Request.QueryString["fr"] != null && !string.IsNullOrEmpty(Request.QueryString["fr"]))
            Response.Redirect("mailticket.aspx?id=" + ViewState["ticid"].ToString() + "&c=" + stc + "&fr=" + Request.QueryString["fr"]);
        else
            Response.Redirect("mailticket.aspx?id=" + ViewState["ticid"].ToString() + "&c=" + stc);

        //else
        //    Response.Redirect("mailticket.aspx?id=" + ViewState["ticid"].ToString() + "&c=" + stc + "pop=1");
    }

    /// <summary>
    /// Get access token from Bing translate API
    /// </summary>
    /// <returns></returns>
    public string GetAccessToken()
    {
        AdmAccessToken admToken;
        string headerValue;
        //Get Client Id and Client Secret from https://datamarket.azure.com/developer/applications/
        AdmAuthentication admAuth = new AdmAuthentication("MobileServiceManager", "fXzR98jYu/w2rXPvw8WTU3Zb2V7/WQkSiCS7Z1tSA6I=");

        admToken = admAuth.GetAccessToken();
        // Create a header with the access_token property of the returned token
        headerValue = "Bearer" + " " + HttpUtility.UrlEncode(admToken.access_token);

        return admToken.access_token;
    }

    /// <summary>
    /// Translation method to tranlate language from BING API
    /// </summary>
    /// <param name="authToken"></param>
    /// <param name="sourceText"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    private string TranslateMethod(string authToken, string sourceText, string from, string to)
    {
        // Add TranslatorService as a service reference, Address:http://api.microsofttranslator.com/V2/Soap.svc
        MOMWebApp.BingTranslatorService.LanguageServiceClient client = new MOMWebApp.BingTranslatorService.LanguageServiceClient();
        //Set Authorization header before sending the request
        HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
        httpRequestProperty.Method = "POST";
        httpRequestProperty.Headers.Add("Authorization", authToken);

        // Creates a block within which an OperationContext object is in scope.
        using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
            // string sourceText = "<UL><LI>Use generic class names. <LI>Use pixels to express measurements for padding and margins. <LI>Use percentages to specify font size and line height. <LI>Use either percentages or pixels to specify table and container width.   <LI>When selecting font families, choose browser-independent alternatives.   </LI></UL>";

            string translationResult;
            //Keep appId parameter blank as we are sending access token in authorization header.
            translationResult = client.Translate("Bearer" + " " + authToken, sourceText, from, to, "text/html", "general", "");
            return translationResult;
        }
    }

    /// <summary>
    /// Detects the language from BING API
    /// </summary>
    /// <param name="authToken"></param>
    /// <param name="sourceText"></param>
    /// <returns></returns>
    private string DetectLanguageMethod(string authToken, string sourceText)
    {
        // Add TranslatorService as a service reference, Address:http://api.microsofttranslator.com/V2/Soap.svc
        MOMWebApp.BingTranslatorService.LanguageServiceClient client = new MOMWebApp.BingTranslatorService.LanguageServiceClient();
        //Set Authorization header before sending the request
        HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
        httpRequestProperty.Method = "POST";
        httpRequestProperty.Headers.Add("Authorization", authToken);

        // Creates a block within which an OperationContext object is in scope.
        using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
            // string sourceText = "<UL><LI>Use generic class names. <LI>Use pixels to express measurements for padding and margins. <LI>Use percentages to specify font size and line height. <LI>Use either percentages or pixels to specify table and container width.   <LI>When selecting font families, choose browser-independent alternatives.   </LI></UL>";

            string translationResult;
            //Keep appId parameter blank as we are sending access token in authorization header.
            translationResult = client.Detect("Bearer" + " " + authToken, sourceText);
            return translationResult;
        }
    }

    protected void lnkTranslate_Click(object sender, EventArgs e)
    {
        if (txtReason.Text.Trim() == string.Empty)
        {
            pnlTranslate.Attributes.Add("style", " display: block;");
            return;
        }

        try
        {
            string sourceLang = DetectLanguageMethod(GetAccessToken(), txtReason.Text);
            string reason = txtReason.Text;

            if (sourceLang == "es")
            {
                txtReason.Text = TranslateMethod(GetAccessToken(), txtReason.Text, sourceLang, "en");
                txtTranslate.Text = reason;
            }
            else if (sourceLang == "en")
            {
                txtTranslate.Text = TranslateMethod(GetAccessToken(), txtReason.Text, sourceLang, "es");
            }

            hdnIsEdited.Value = "0";
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "keyTransErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        pnlTranslate.Attributes.Add("style", " display: block;");
    }

    protected void lnkTransEnglish_Click(object sender, EventArgs e)
    {
        if (txtWorkCompl.Text.Trim() == string.Empty)
        {
            pnlTransDesc.Attributes.Add("style", " display: block;");
            return;
        }

        try
        {
            string sourceLang = DetectLanguageMethod(GetAccessToken(), txtWorkCompl.Text);
            string workCompl = txtWorkCompl.Text;

            if (sourceLang == "es")
            {
                txtWorkCompl.Text = TranslateMethod(GetAccessToken(), txtWorkCompl.Text, sourceLang, "en");
                txtTransDesc.Text = workCompl;
            }
            else if (sourceLang == "en")
            {
                txtTransDesc.Text = TranslateMethod(GetAccessToken(), txtWorkCompl.Text, sourceLang, "es");
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "keyTransErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        //finally
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "keytxtscript", "$('#" + txtWorkCompl.ClientID + "').keyup(function(event) {replaceQuickCodes(event, '" + txtWorkCompl.ClientID + "', $('#" + hdnCon.ClientID + "').val());}); $('#" + txtWorkCompl.ClientID + "').focus(function() {$(this).animate({width: '520px', height: '75px'}, 500, function() {});});            $('#" + txtWorkCompl.ClientID + "').blur(function() {$(this).animate({width: '188px', height: '63px'}, 500, function() {});});", true);

        //}
        pnlTransDesc.Attributes.Add("style", " display: block;");
    }

    protected void lnkTransReasonToEnglish_Click(object sender, EventArgs e)
    {
        if (txtTranslate.Text.Trim() == string.Empty)
        {
            pnlTranslate.Attributes.Add("style", " display: block;");
            return;
        }

        try
        {
            txtReason.Text = TranslateMethod(GetAccessToken(), txtTranslate.Text, "es", "en");
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "keyTransErr2", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        pnlTranslate.Attributes.Add("style", " display: block;");
    }

    protected void lnkTransDescToEnglish_Click(object sender, EventArgs e)
    {
        if (txtTransDesc.Text.Trim() == string.Empty)
        {
            pnlTransDesc.Attributes.Add("style", "display: block;");
            return;
        }

        try
        {
            txtWorkCompl.Text = TranslateMethod(GetAccessToken(), txtTransDesc.Text, "es", "en");
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "keyTransErr1", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        pnlTransDesc.Attributes.Add("style", "display: block;");
    }

    private void ChangeRoute()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Username = ddlRoute.SelectedValue;

        hdnLang.Value = "english";
        //string Lang = objBL_User.getUserLangByID(objPropUser);
        //if (!string.IsNullOrEmpty(Lang))
        //{
        //    if (Lang != "none")
        //    {
        //        hdnLang.Value = Lang;
        //    }
        //    else
        //    {
        //        hdnLang.Value = "english";
        //    }
        //}
        //else
        //{
        //    hdnLang.Value = "english";
        //}
    }
    protected void ddlRoute1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRoute.SelectedIndex = ddlRoute1.SelectedIndex;
        ddlRoute_SelectedIndexChanged(sender, e);
    }
    protected void ddlRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRoute1.SelectedIndex = ddlRoute.SelectedIndex;
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Username = ddlRoute.SelectedValue;
        int status = objBL_User.getEMPStatus(objPropUser);
        if (status == 1)
            lblWorkStatus.Text = "Inactive";
        else
            lblWorkStatus.Text = "";
        ChangeRoute();
        hdnFocus.Value = ddlRoute.ClientID;
        string wagesRage = string.Empty;
        wagesRage = ViewState["JobCostLabor"].ToString();
        if (wagesRage == "1")
        {
            FillWage(ddlRoute.SelectedValue);

            if (hdnProjectwageID.Value != "")
            {
                string str = hdnProjectwageID.Value.ToString();
                ListItem item = ddlWage.Items.FindByValue(str);
                if (item != null)
                {
                    ddlWage.SelectedValue = str;
                }
            }
        }
        if (Page.IsPostBack)
        {
            if (ddlRoute.SelectedIndex == 0)
            {
                ddlStatus1.SelectedIndex = ddlStatus.SelectedIndex = 0;
                ddlStatus_SelectedIndexChanged(sender, e);
            }
            else if (ddlStatus.SelectedIndex == 0)
            {
                ddlStatus1.SelectedIndex = ddlStatus.SelectedIndex = 1;
                ddlStatus_SelectedIndexChanged(sender, e);
            }
        }
    }

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
            try
            {
                /****** Get path from web.config***********/
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string filename = "";
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\";
                string fullpath = "";
                string MIME = string.Empty;
                /****** When mode 0 (add new ticket) creates a temp id instead if the ticket id. creates directory with temp id name. when mode is 1 (update record) it creates directory with ticketID.
                 Ihe temp concept is just for uploading the doc before the ticket is saved as ticket id is not created yet.
                 If ticket is not created and documents are uploaded then they are automaticklly deleted next day on Tickelistview.aspx load event.
                 In documents table the screen field is set 'temp' in case of mode=0. In case of mode =1 screen is set to 'ticket'.**********/
                if (Convert.ToInt32(ViewState["mode"]) == 0)
                {
                    savepath += hdnFormId.Value + @"\";
                    //filename = hdnFormId.Value + "_" + FileUpload1.FileName;
                    filename = FileUpload1.FileName;
                    objMapData.Screen = "Temp";
                    objMapData.TicketID = 0;
                    objMapData.TempId = hdnFormId.Value;
                }
                else
                {
                    savepath += Request.QueryString["id"].ToString() + @"\";
                    //filename = Convert.ToInt32(Request.QueryString["id"].ToString()) + "_" + FileUpload1.FileName;
                    filename = FileUpload1.FileName;
                    objMapData.Screen = "Ticket";
                    objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
                    objMapData.TempId = "0";
                }

                filename = filename.Replace(",", "");
                fullpath = savepath + filename;
                MIME = Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);


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

                objMapData.FileName = filename;
                objMapData.DocTypeMIME = MIME;
                objMapData.FilePath = fullpath;
                objMapData.ConnConfig = Session["config"].ToString();

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                FileUpload1.SaveAs(fullpath);
                objMapData.Mode = 0;
                objMapData.DocID = 0;
                objBL_MapData.AddFile(objMapData);
                GetDocuments();
                ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadSuccess", "noty({text: 'File uploaded successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                RadgvDocuments.Rebind();
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

    private void GetDocuments()
    {
        if (Convert.ToInt32(ViewState["mode"]) == 0)
        {
            objMapData.Screen = "Temp";
            objMapData.TempId = hdnFormId.Value;
            objMapData.TicketID = 0;
        }
        else
        {
            objMapData.Screen = "Ticket";
            objMapData.TempId = "0";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
        }

        objMapData.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_MapData.GetDocuments(objMapData);
        RadgvDocuments.DataSource = ds.Tables[0];
        RadgvDocuments.VirtualItemCount = ds.Tables[0].Rows.Count;
    }

    /// <summary>
    /// Update document data when new ticket is saved and docs are already uploaded. hdnFormID stores the temp id. 
    /// it actually updates the screen and screenid fields. updated 'temp' with 'ticket' in screen document.screen field. updates document.screenid field with 'null' to ticketID created.
    /// </summary>
    /// 

    private void UpdateDoc()
    {
        objMapData.Screen = "Ticket";
        objMapData.TicketID = Convert.ToInt32(ViewState["ticid"]);
        objMapData.TempId = hdnFormId.Value;
        objMapData.ConnConfig = Session["config"].ToString();

        objBL_MapData.UpdateFile(objMapData);
    }

    private void SaveDocInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MSVisible", typeof(byte));


        foreach (GridDataItem item in RadgvDocuments.Items)
        {
            Label lblID = (Label)item.FindControl("lblID");
            CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
            Label lblScreen = (Label)item.FindControl("lblScreen");
            CheckBox chkMSVisible = (CheckBox)item.FindControl("chkMSVisible");

            DataRow dr = dt.NewRow();
            dr["ID"] = lblID.Text;
            dr["Portal"] = false;
            dr["Remarks"] = "";
            dr["MSVisible"] = chkMSVisible.Checked;
            dt.Rows.Add(dr);
        }
        MapData objMapDataloc = new MapData();

        objMapDataloc.Screen = "Ticket";
        objMapDataloc.TicketID = Convert.ToInt32(ViewState["ticid"]);
        objMapDataloc.TempId = hdnFormId.Value;
        objMapDataloc.ConnConfig = Session["config"].ToString();

        if (dt.Rows.Count > 0)
        {
            objBL_MapData.UpdateFileMSVisible(objMapData, dt);
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        HttpPostedFile file;
        Session["files"] = Request.Files;
        foreach (string key in Request.Files.Keys)
        {
            file = Request.Files[key];

            if (file != null && file.ContentLength > 0)
            {
                file.SaveAs("C:\\UploadedUserFiles\\" + file.FileName);
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
        foreach (GridDataItem di in RadgvDocuments.Items)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblId");
            Label lblPath = (Label)di.FindControl("lblPath");

            if (chkSelected.Checked == true)
            {
                DeleteFileFromFolder(lblPath.Text, Convert.ToInt32(lblID.Text));
            }
        }
        RadgvDocuments.Rebind();
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objBL_MapData.DeleteFile(objMapData);
            GetDocuments();
            ClientScript.RegisterStartupScript(Page.GetType(), "keyDeletedSuccess", "noty({text: 'File Deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillRelatedTickets()
    {
        if (txtWO.Text.Trim() != string.Empty)
        {
            if (!txtWO.Text.Trim().Equals(hdnWO.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                DataSet ds = new DataSet();
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.Workorder = txtWO.Text.Trim();

                if (Request.QueryString["id"] != null && Request.QueryString["copy"] == null)
                    objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
                else
                    objMapData.TicketID = 0;

                ds = objBL_MapData.GetTicketsByWorkorder(objMapData);

                RadGvlstRelatedTickets.VirtualItemCount = ds.Tables[0].Rows.Count;
                RadGvlstRelatedTickets.DataSource = ds.Tables[0];
                hdnWO.Value = txtWO.Text.Trim();
            }
        }

    }

    /// <summary>
    /// Get Signature image.
    /// </summary>
    /// <param name="ticketid"></param>
    /// <param name="workerid"></param>
    /// <returns></returns>
    /// 
    public string GetTicketSignature(string ticketid, string workerid)
    {
        DataSet ds = new DataSet();
        string image = string.Empty;
        if (workerid.Trim() != string.Empty)
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.TicketID = Convert.ToInt32(ticketid);
            objMapData.WorkID = Convert.ToInt32(workerid);
            ds = objBL_MapData.GetTicketSignature(objMapData);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["signature"] != DBNull.Value)
                {
                    image = "data:image/png;base64," + Convert.ToBase64String((byte[])ds.Tables[0].Rows[0]["signature"]);
                }
            }
        }
        return image;
    }


    private void fillREPHistory()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = 0;
        objPropUser.SearchBy = "rd.ticketID";
        objPropUser.SearchValue = Request.QueryString["id"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_User.getequipREPDetails(objPropUser);

        RadgvMCPDetails.DataSource = ds.Tables[0];
        RadgvMCPDetails.VirtualItemCount = ds.Tables[0].Rows.Count;
    }

    private void GetBillcodesforTimeSheet()
    {
        BL_Contracts objBL_Contracts = new BL_Contracts();
        Contracts objProp_Contracts = new Contracts();

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Contracts.GetBillcodesforTimeSheet(objProp_Contracts);

        ddlService.DataSource = ds.Tables[0];
        ddlService.DataTextField = "billcode";
        ddlService.DataValueField = "QBinvid";
        ddlService.DataBind();

        ddlService.Items.Insert(0, new ListItem(" Select ", ""));
    }

    private void GetPayrollforTimeSheet()
    {
        BL_Contracts objBL_Contracts = new BL_Contracts();
        Contracts objProp_Contracts = new Contracts();

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Contracts.GetPayrollforTimeSheet(objProp_Contracts);

        ddlPayroll.DataSource = ds.Tables[0];
        ddlPayroll.DataTextField = "fdesc";
        ddlPayroll.DataValueField = "QBwageid";
        ddlPayroll.DataBind();

        ddlPayroll.Items.Insert(0, new ListItem(" Select ", ""));
    }

    private void GetQBInt()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objPropUser);

        ViewState["qbint"] = "0";
        ViewState["tsint"] = "0";
        ViewState["internetdefault"] = "0";

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["QBIntegration"].ToString() == "1")

            {
                ViewState["qbint"] = "1";
                hdnQuickBooksIntegration.Value = "1";
            }
            if (ds.Tables[0].Rows[0]["TsIntegration"].ToString() == "1")

                ViewState["tsint"] = "1";

            if (ds.Tables[0].Rows[0]["tinternett"].ToString() == "1")
                ViewState["internetdefault"] = "1";

            //Wages category mandatory field if the Job Cost Labor = Burden Rate

            string wagesRage = ds.Tables[0].Rows[0]["JobCostLabor"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["JobCostLabor"].ToString();

            RequiredFieldVWage.Enabled = ValidatorCVWage.Enabled = wagesRage == "1" ? ((ddlStatus.SelectedValue == "4") ? true : false) : false;

            ViewState["JobCostLabor"] = wagesRage;

            MSTimeDataFieldVisibility.Value = ds.Tables[0].Rows[0]["MSTimeDataFieldVisibility"] == DBNull.Value ? "YYYYYYYYYYYYYYYY" : ds.Tables[0].Rows[0]["MSTimeDataFieldVisibility"].ToString();

            string st = MSTimeDataFieldVisibility.Value;

            if (st.Length > 4)
            {
                if (st.Substring(4, 1) == "N")
                {

                    PermissionHiddenFieldTT.Value = "0";
                }
            }

        }
    }

    private void ShowQBSyncControls()
    {
        if (ViewState["qbint"].ToString() == "1")//&& ddlStatus.SelectedValue=="4"
        {
            //chkTimeTrans.Enabled = true;
            ddlService.Enabled = true;
            ddlPayroll.Enabled = true;
            QbpayrollDiv.Visible = true;
        }
        else
        {
            //chkTimeTrans.Enabled = false;
            ddlService.Enabled = false;
            ddlPayroll.Enabled = false;
            QbpayrollDiv.Visible = false;
        }
    }

    protected void ddlService_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlService.SelectedValue != string.Empty)
        {
            BL_Contracts objBL_Contracts = new BL_Contracts();
            Contracts objProp_Contracts = new Contracts();

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.QBInvID = ddlService.SelectedValue;
            DataSet ds = new DataSet();
            ds = objBL_Contracts.GetPayrollByAccount(objProp_Contracts);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlPayroll.SelectedValue = ds.Tables[0].Rows[0]["QBPayrollItem"].ToString();
            }
            else
            {
                ddlPayroll.SelectedValue = string.Empty;
            }
        }
    }

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        //SetProspect();
    }

    private void SetProspect()
    {
        if (hdnProspect.Value != "1")
        {
            RequiredFieldValidator1.Enabled = true;
            txtLocation.Enabled = true;
            //txtLocation.Text = "";
            //hdnProspect.Value = "";
            //txtCustomer.ForeColor = System.Drawing.ColorTranslator.FromHtml("#686767");
        }
        else
        {
            RequiredFieldValidator1.Enabled = false;
            //hdnFocus.Value = txtAddress.ClientID;
            txtLocation.Enabled = false;
            //txtLocation.Text = "";
            //hdnProspect.Value = "1";
            //txtCustomer.ForeColor = System.Drawing.Color.Brown;
        }
    }

    protected void lnkConvert_Click(object sender, EventArgs e)
    {
        pnlCustomer.Visible = true;
        uc_CustomerSearch1._txtCustomer.Focus();
        lnkConvert.Visible = false;
        lnkPrint.Visible = false;
        ViewState["convert"] = "1";
    }

    private void ConvertProspectWizard()
    {
        string ProspectID = hdnLocId.Value;
        string ticketid = ViewState["ticid"].ToString();
        string comp = "0";
        if (Request.QueryString["comp"] == null)
        {
            if (ViewState["assign"] != null)
            {
                if (ViewState["assign"].ToString() == "4")
                    comp = "1";
            }
        }
        else
        {
            comp = Request.QueryString["comp"].ToString();
        }

        if (ViewState["convert"].ToString() == "1")
        {
            //if (uc_CustomerSearch1._hdnCustID.Value == string.Empty)
            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Continue to Convert Lead Wizard.'); window.location.href='addcustomer.aspx?cpw=1&prospectid=" + ProspectID + "&Ticketid=" + ticketid + "&comp=" + comp + "';", true);
            //else
            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + uc_CustomerSearch1._hdnCustID.Value + "&Ticketid=" + ticketid + "&comp=" + comp + "';", true);
        }
    }

    private void GetCustomReport()
    {
        if (Request.QueryString["id"] != null)
        {
            string TicketID = (Request.QueryString["id"].ToString());
            Customer objProp_Customer = new Customer();
            BL_Customer objBL_Customer = new BL_Customer();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Customer.GetCustomReport(objProp_Customer);
            StringBuilder sb = new StringBuilder();

            string URL = string.Empty;

            //Print
            if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["TicketReportFormat"]) && WebConfigurationManager.AppSettings["TicketReportFormat"].ToLower().Contains("mrt"))
            {
                if (Request.QueryString["fr"] != null)
                    URL = "TicketReport.aspx?id=" + TicketID + "&fr=" + Request.QueryString["fr"];
                else
                    URL = "TicketReport.aspx?id=" + TicketID;
            }
            else
            {
                if (Request.QueryString["fr"] != null)
                    URL = "Printticket.aspx?id=" + TicketID + "&c=0&RDLC=Custom&fr=" + Request.QueryString["fr"];
                else
                    URL = "Printticket.aspx?id=" + TicketID + "&c=0&RDLC=Custom";
            }
            sb.AppendLine(string.Format("<li><a href=\"{0}\">{1}</a></li>", ((URL)), ("Print Ticket")));
            sb.AppendLine("</a></li>");

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    if (item["ReportType"].ToString() == "Ticket")
                    {
                        //Print
                        //if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["TicketReportFormat"]) && WebConfigurationManager.AppSettings["TicketReportFormat"].ToLower().Contains("mrt"))
                        //{
                        //    URL = "TicketReport.aspx?id=" + TicketID;
                        //}
                        //else
                        //{
                        //    URL = "Printticket.aspx?id=" + TicketID + "&c=0&RDLC=" + item["ReportName"].ToString();
                        //}

                        if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["TicketReportFormat"]) && WebConfigurationManager.AppSettings["TicketReportFormat"].ToLower().Contains("mrt"))
                        {
                            if (Request.QueryString["fr"] != null)
                                URL = "TicketReport.aspx?id=" + TicketID + "&fr=" + Request.QueryString["fr"];
                            else
                                URL = "TicketReport.aspx?id=" + TicketID;
                        }
                        else
                        {
                            if (Request.QueryString["fr"] != null)
                                URL = "Printticket.aspx?id=" + TicketID + "&c=0&RDLC=Custom&fr=" + Request.QueryString["fr"];
                            else
                                URL = "Printticket.aspx?id=" + TicketID + "&c=0&RDLC=Custom";
                        }

                        sb.AppendLine(string.Format("<li><a href=\"{0}\">{1}</a></li>", ((URL)), (item["ReportDesc"].ToString())));
                        sb.AppendLine("</a></li>");
                    }

                }
            }


            dynamicUIPlaceholder.Text = sb.ToString();
        }
        else
        {
            dynamicUIPlaceholder.Text = "";
        }
    }

    private void GetJobCode(bool isnotsaveclick = true)
    {

        if (hdnProjectId.Value != string.Empty)
        {
            Customer objProp_Customer = new Customer();
            BL_Customer objBL_Customer = new BL_Customer();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProjectJobID = Convert.ToInt32(hdnProjectId.Value);
            objProp_Customer.Type = "1";
            if (Convert.ToInt32(hdnProjectId.Value) != 0)
            {
                DataSet ds = objBL_Customer.GetjobcodeInfo(objProp_Customer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hdnProjectTaskCategory.Value = ds.Tables[0].Rows[0]["taskcategory"].ToString();
                    projectNo.Text = txtProject.Text = ds.Tables[0].Rows[0]["ID"].ToString() + "-" + ds.Tables[0].Rows[0]["fdesc"].ToString();
                    ////hdnProjectId.Value = ds.Tables[0].Rows[0]["ID"].ToString();
                    //if (ds.Tables[1].Rows.Count > 0)
                    //{
                    RadgvProjectCode.VirtualItemCount = ds.Tables[1].Rows.Count;
                    RadgvProjectCode.DataSource = ds.Tables[1];
                    //gvProjectCode.DataBind();
                    txtJobCode.Text = string.Empty;
                    hdnProjectCode.Value = string.Empty;
                    //} 
                    ListItem item1 = ddlDepartment.Items.FindByValue(ds.Tables[0].Rows[0]["type"].ToString());
                    if (item1 != null)
                    {
                        ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    }


                    string str = ds.Tables[0].Rows[0]["wage"].ToString();
                    ListItem item = ddlWage.Items.FindByValue(str);
                    if (item != null)
                    {

                        hdnProjectwageID.Value = str;
                        ddlWage.SelectedValue = str;
                    }
                    //    }
                    //}
                    if (isnotsaveclick)
                    {
                        ///Add/Edit ticket screen if the selected project is marked chargeable then to mark 
                        ///the ticket chargeable automatically
                        if (ds.Tables[0].Rows[0]["Charge"].ToString().Equals("1"))
                        {
                            chkChargeable.Checked = true; chkJobChargeable.Checked = true;
                        }
                        else
                        {
                            ///check if the selected catetory is chargeable
                            int value;
                            if (int.TryParse(hdnIsJobChargable.Value, out value)
                            && Convert.ToInt16(hdnIsJobChargable.Value) > 0)
                            {
                                chkChargeable.Checked = true; chkJobChargeable.Checked = true;
                            }
                            //else
                            //{
                            //    chkChargeable.Checked = false; chkJobChargeable.Checked = false;
                            //}
                        }
                    }
                }



                GetJobRatebyID();
            }
        }
    }
    public void GetJobRatebyID()
    {
        txtnknt.Text      = "0.00";
        txtnkbt.Text      = "0.00";
        txtnkot.Text      = "0.00";
        txtnkdt.Text      = "0.00";
        txtnktr.Text      = "0.00";
        txtnkMileage.Text = "0.00";

        Customer objProp_Customer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.job = Convert.ToInt32(hdnProjectId.Value);
        objProp_Customer.Type = "1";

        if (Convert.ToInt32(hdnProjectId.Value) != 0)
        {
            DataSet ds = objBL_Customer.GetJobRatebyID(objProp_Customer);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtnknt.Text = ds.Tables[0].Rows[0]["NT"].ToString();
                txtnkbt.Text = ds.Tables[0].Rows[0]["BT"].ToString();
                txtnkot.Text = ds.Tables[0].Rows[0]["OT"].ToString();
                txtnkdt.Text = ds.Tables[0].Rows[0]["DT"].ToString();
                txtnktr.Text = ds.Tables[0].Rows[0]["TT"].ToString();
                txtnkMileage.Text = ds.Tables[0].Rows[0]["RM"].ToString();
            }
        }

    }

    protected void btnEquip_Click(object sender, EventArgs e)
    {
        FillRecentCalls(Convert.ToInt32(hdnLocId.Value));

    }

    private void FillProjectsTemplate()
    {
        DataSet ds = new DataSet();
        objCustomer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getJobProjectTemplate(objCustomer);
        ddlTemplate.DataSource = ds.Tables[0];
        ddlTemplate.DataTextField = "Fdesc";
        ddlTemplate.DataValueField = "id";
        ddlTemplate.DataBind();

        ddlTemplate.Items.Insert(0, new ListItem(" Select ", "0"));
    }

    private void FillWage(string fWorker)
    {
        ddlWage.Items.Clear();
        ddlWage.SelectedValue = null;
        DataSet ds = new DataSet();
        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.Fuser = fWorker;
        objCustomer.ticketID = Request.QueryString["id"] == null ? 0 : Convert.ToInt32(Request.QueryString["id"]);
        ds = objBL_Customer.getWage(objCustomer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlWage.DataSource = null;
            ddlWage.DataSource = ds.Tables[0];
            ddlWage.DataTextField = "Fdesc";
            ddlWage.DataValueField = "id";
            ddlWage.DataBind();
        }
        ddlWage.Items.Insert(0, new ListItem(" Select ", "0"));
        try
        {
            if (ds.Tables[0].Rows.Count == 1 && fWorker !="" && ddlStatus.SelectedValue=="4" )
            {
                ddlWage.SelectedIndex = 1;
            }
        }
        catch { }
    }

    private void FillRecentCalls(int loc)
    {
        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.LocID = loc;
        if (Request.QueryString["id"] != null)
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"]);
        ds = objBL_MapData.GetRecentCallsLoc(objMapData);
        lstRecentCalls.DataSource = ds.Tables[0];
        lstRecentCalls.DataBind();
    }

    public string RecentCallsDetails(string assigned, string worker, string cat, string elev)
    {
        string str = string.Empty;
        if (assigned.ToLower() == "assigned")
            str = assigned + " to <strong>" + worker + "</strong>";
        else
            str = assigned + " by <strong>" + worker + "</strong>";

        if (cat != string.Empty)
            str += "<BR/>Category - " + cat;
        if (elev != string.Empty)
            str += "<BR/>Equipment - " + elev;

        return str;
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillChargeableFromCategory();
    }

    private void FillChargeableFromCategory()
    {
        //if (hdnReviewed.Value != "")
        //{
        //    if (Convert.ToInt16(hdnReviewed.Value) == 0 && ddlStatus.SelectedValue == "4")
        //    {
        //        objPropUser.ConnConfig = Session["config"].ToString();
        //        objPropUser.Cat = ddlCategory.SelectedValue;
        //        DataSet ds = objBL_User.getcategoryAll(objPropUser);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            bool charge = Convert.ToBoolean(ds.Tables[0].Rows[0]["chargeable"]);
        //            if (charge) chkChargeable.Checked = charge;
        //        }
        //    }
        //}

        //check the JOB is chargeable the  check the chargeable 
        int value;
        if (int.TryParse(hdnIsJobChargable.Value, out value)
            && Convert.ToInt16(hdnIsJobChargable.Value) > 0)
        {
            chkChargeable.Checked = true; chkJobChargeable.Checked = true;
        }
        else
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Cat = ddlCategory.SelectedValue;
            DataSet ds = objBL_User.getcategoryAll(objPropUser);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bool charge = Convert.ToBoolean(ds.Tables[0].Rows[0]["chargeable"]);

                if (charge)
                {
                    hdnIsCatChargable.Value = "1";
                    chkJobChargeable.Checked = charge;
                    chkChargeable.Checked = charge;
                }
                else
                {
                    hdnIsCatChargable.Value = "0";
                }
            }
        }
    }

    private void FillChargeableFromJob()
    {
        if (hdnProjectId.Value != string.Empty)
        {
            Customer objProp_Customer = new Customer();
            BL_Customer objBL_Customer = new BL_Customer();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProjectJobID = Convert.ToInt32(hdnProjectId.Value);
            objProp_Customer.Type = "1";
            if (Convert.ToInt32(hdnProjectId.Value) != 0)
            {
                DataSet ds = objBL_Customer.GetjobcodeInfo(objProp_Customer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // ref-SECO - 285 
                    ///-Add/Edit ticket screen if the selected project is marked chargeable then to mark the ticket chargeable automatically
                    if (ds.Tables[0].Rows[0]["Charge"].ToString().Equals("1"))
                    {
                        chkChargeable.Checked = true; chkJobChargeable.Checked = true;
                    }
                }
            }
        }

    }

    #region Navigation

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        if (Session["ticketids"] != null)
        {
            DataTable dt = (DataTable)Session["ticketids"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                if (Request.QueryString["fr"] != null && !string.IsNullOrEmpty(Request.QueryString["fr"]))
                {
                    Response.Redirect("addticket.aspx?id=" + dt.Rows[index + 1]["id"] + "&comp=" + dt.Rows[index + 1]["comp"] + "&pop=1&fr=" + Request.QueryString["fr"]);
                }
                else
                {
                    Response.Redirect("addticket.aspx?id=" + dt.Rows[index + 1]["id"] + "&comp=" + dt.Rows[index + 1]["comp"] + "&pop=1");
                }
                //Response.Redirect("addticket.aspx?comp=" + dt.Rows[index + 1]["comp"] + "&id=" + dt.Rows[index + 1]["id"]);
            }
        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        if (Session["ticketids"] != null)
        {
            DataTable dt = (DataTable)Session["ticketids"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                if (Request.QueryString["fr"] != null && !string.IsNullOrEmpty(Request.QueryString["fr"]))
                {
                    Response.Redirect("addticket.aspx?id=" + dt.Rows[index - 1]["id"] + "&comp=" + dt.Rows[index - 1]["comp"] + "&pop=1&fr=" + Request.QueryString["fr"]);
                }
                else
                {
                    Response.Redirect("addticket.aspx?id=" + dt.Rows[index - 1]["id"] + "&comp=" + dt.Rows[index - 1]["comp"] + "&pop=1");
                }
                //Response.Redirect("addticket.aspx?comp=" + dt.Rows[index - 1]["comp"] + "&id=" + dt.Rows[index - 1]["id"]);
            }
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        if (Session["ticketids"] != null)
        {
            DataTable dt = (DataTable)Session["ticketids"];
            if (Request.QueryString["fr"] != null && !string.IsNullOrEmpty(Request.QueryString["fr"]))
            {
                //Response.Redirect("addticket.aspx?id=" + dt.Rows[0]["id"] + "&comp =" + dt.Rows[0]["comp"] + "&pop=1&fr=" + Request.QueryString["fr"]);
                Response.Redirect("addticket.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["id"] + "&comp=" + dt.Rows[dt.Rows.Count - 1]["comp"] + "&pop=1&fr=" + Request.QueryString["fr"]);
            }
            else
            {
                //Response.Redirect("addticket.aspx?comp=" + dt.Rows[dt.Rows.Count - 1]["comp"] + "&id=" + dt.Rows[dt.Rows.Count - 1]["id"]);
                Response.Redirect("addticket.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["id"] + "&comp=" + dt.Rows[dt.Rows.Count - 1]["comp"] + "&pop=1");
            }
        }
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        if (Session["ticketids"] != null)
        {
            DataTable dt = (DataTable)Session["ticketids"];
            //id = 1201546 & comp = 0 & pop = 1 & fr = tlv
            if (Request.QueryString["fr"] != null && !string.IsNullOrEmpty(Request.QueryString["fr"]))
            {
                Response.Redirect("addticket.aspx?id=" + dt.Rows[0]["id"] + "&comp=" + dt.Rows[0]["comp"] + "&pop=1&fr=" + Request.QueryString["fr"]);
            }
            else
            {
                Response.Redirect("addticket.aspx?id=" + dt.Rows[0]["id"] + "&comp=" + dt.Rows[0]["comp"] + "&pop=1");
            }

        }
    }

    #endregion
    private void GetBusinessHours()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objPropUser);
        if (ds.Tables[0].Rows.Count > 0)
        {
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

    private void SelectTaskCategory()
    {
        getDiagnosticCodes();
        getProjectTasks();
    }

    private void getDiagnosticCodes()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        objGeneral.CodeCategory = hdnProjectTaskCategory.Value;
        objGeneral.CodeType = 1;
        DataSet ds = new DataSet();
        ds = objBL_General.getDiagnostic(objGeneral);
        rptCodesList.DataSource = ds.Tables[0];
        rptCodesList.DataBind();
        if (ds.Tables[0].Rows.Count > 0) lblResolutionTasks.Visible = false;
    }

    private void getProjectTasks()
    {
        if (hdnProjectId.Value != string.Empty)
        {
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.job = Convert.ToInt32(hdnProjectId.Value);
            DataSet ds = objBL_Customer.getJobTasks(objCustomer);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (RepeaterItem rt in rptCodesList.Items)
                {
                    CheckBox chkCode = (CheckBox)rt.FindControl("chkCode");
                    HiddenField hdnCodeCat = (HiddenField)rt.FindControl("hdnCodeCat");
                    HiddenField hdnTicket = (HiddenField)rt.FindControl("hdnTicket");
                    HiddenField hdnUsername = (HiddenField)rt.FindControl("hdnUsername");
                    HiddenField hdnDate = (HiddenField)rt.FindControl("hdnDate");
                    Label lblDesc = (Label)rt.FindControl("lblDescRP");
                    HyperLink lnkTicket = (HyperLink)rt.FindControl("lnkTicket");

                    if (hdnCodeCat.Value.ToLower() == dr["category"].ToString().ToLower() && chkCode.Text.ToLower() == dr["task_code"].ToString().ToLower())
                    {
                        chkCode.Checked = true;
                        chkCode.Enabled = false;
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
                dr["task_code"] = chkCode.Text;
                dr["Category"] = hdnCodeCat.Value;
                if (hdnChecked.Value == "1")
                {
                    dr["username"] = Session["username"].ToString();
                    dr["dateupdated"] = System.DateTime.Now;
                    dr["ticket_id"] = 0;
                }
                else
                {
                    dr["username"] = hdnUsername.Value;
                    dr["dateupdated"] = Convert.ToDateTime(hdnDate.Value);
                    if (hdnTicket.Value != string.Empty)
                        dr["ticket_id"] = Convert.ToInt32(hdnTicket.Value);
                }
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }


    private void Fill_InvoiceInfo(DataSet ds)
    {
        var invoiceNo = txtInvoiceNo.Text;
        if (!string.IsNullOrEmpty(invoiceNo.ToString()))
        {
            lnkInvoice.Visible = true;
            ChangeImage();
            lnkInvoice.Attributes["onclick"] = "window.open('addinvoice.aspx?o=1&uid=" + invoiceNo + "', 'Invoice', 'height=768,width=1280,scrollbars=yes');";
        }

        /******** Prevent invoice listing on copy ticket *********/
        /////
        if (ds.Tables[0].Rows[0]["invoice"] != DBNull.Value)
        {
            if (ds.Tables[0].Rows[0]["invoice"].ToString().Trim() != string.Empty && ds.Tables[0].Rows[0]["invoice"].ToString().Trim() != "0")
            {
                txtInvoiceNo.Text = ds.Tables[0].Rows[0]["invoice"].ToString();
                txtInvoiceNo.Enabled = false;
                chkChargeable.Enabled = false;
                chkJobChargeable.Enabled = false;
                chkInvoice.Enabled = false;

                ChangeImage();
                if (Session["MSM"].ToString() != "TS")
                {
                    lnkInvoice.Visible = true;

                    lnkInvoice.Attributes["onclick"] = "window.open('addinvoice.aspx?o=1&uid=" + ds.Tables[0].Rows[0]["invoice"].ToString() + "', 'Invoice', 'height=768,width=1280,scrollbars=yes');";

                    if (ds.Tables[0].Rows[0]["qbinvoiceid"].ToString() != "")
                    {
                        imgInv.ImageUrl = "images/QB_invoice.png";
                        imgInv.ToolTip = "Invoice created in QuickBooks";
                    }
                }

            }
        }
    }

    private DataSet GetTicketDataByID()
    {

        DataSet ds = new DataSet();
        try
        {
            int TicketDId = 0;
            if (Request.QueryString["id"] != null)
            {
                TicketDId = Convert.ToInt32(Request.QueryString["id"]);
            }

            BL_Tickets _BL_Tickets = new BL_Tickets();

            string ConnConfig = Session["config"].ToString();

            ds = _BL_Tickets.GetTicketDataIByID(TicketDId, ConnConfig);
        }
        catch { }
        return ds;
    }

    protected void btnRefressTicketScreen_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds = GetTicketDataByID();
        Fill_InvoiceInfo(ds);//Fill Invoice Info after saving the invoice 
    }

    private DataSet GetContractInfo(Int32 LocID, Int32 EquipID, string Type)
    {
        DataSet ds = new DataSet();
        //if (Request.QueryString["id"] != null)
        //{
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.LocID = LocID;
        ds = objBL_MapData.GetContractInfo(objMapData, EquipID, Type);
        if (Type == "Location")
        {
            if (ds.Tables[0].Rows.Count > 1)
            {

                locContractinfo.InnerHtml = "";
                lnkbtnlocContractinfo.Visible = true;
                divlocContractinfo.Visible = true;
            }
            else if (ds.Tables[0].Rows.Count == 1)
            {
                locContractinfo.InnerHtml = "Contract type:-" + (ds.Tables[0].Rows[0]["ContractType"]) + "|| Schedule Frequency:-" + (ds.Tables[0].Rows[0]["ScheduleFrequency"]);
                lnkbtnlocContractinfo.Visible = false;
                divlocContractinfo.Visible = true;
            }
            else
            {
                locContractinfo.InnerHtml = "";
                divlocContractinfo.Visible = false;
            }
        }
        //}
        return ds;
    }

    protected void btnlocContractinfo_Click(object sender, EventArgs e)
    {
        if (hdnLocId.Value != "")
        {
            DataSet ds = new DataSet();
            ds = GetContractInfo(Convert.ToInt32(hdnLocId.Value), 0, "Location");
            RadGVContractInfo.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGVContractInfo.DataSource = ds.Tables[0];
            RadGVContractInfo.Rebind();
            string script = "function f(){$find(\"" + RadWindowlocContractinfo.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key1", script, true);
        }
    }

    protected void btnEquipContractinfo_Click(object sender, EventArgs e)
    {
        LinkButton button1 = sender as LinkButton;
        if (button1.CommandArgument != "")
        {
            if (hdnLocId.Value != "")
            {
                DataSet ds = new DataSet();
                ds = GetContractInfo(0, Convert.ToInt32(button1.CommandArgument), "Equipment");
                RadGVContractInfo.VirtualItemCount = ds.Tables[0].Rows.Count;
                RadGVContractInfo.DataSource = ds.Tables[0];
                RadGVContractInfo.Rebind();
                string script = "function f(){$find(\"" + RadWindowlocContractinfo.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key112", script, true);
            }
        }
    }

    private void SendMailToSalesPer(Int32 TicketID, string OpportID)
    {

        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.LocID = Convert.ToInt32(hdnLocId.Value);
        objMapData.TicketID = Convert.ToInt32(TicketID);
        ds = objBL_MapData.GetSalesPerInfo(objMapData);
        if (ds.Tables[0].Rows.Count == 1)
        {
            string To = ds.Tables[0].Rows[0]["SalesPerMailID"].ToString();
            if (To.Trim() != string.Empty && To.Contains('@'))
            {
                string Locname = txtLocation.Text;
                string OppDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
                string Recommendationinfo = txtRecommendation.Text;
                string Workername = Session["username"].ToString();

                string message = "You have an opportunity waiting. Opp #" + OpportID + " -" + Locname + "-" + OppDate + ".<BR/>";
                message += "Entered through Ticket #" + TicketID + " by " + Workername + ".<BR/>";
                message += Recommendationinfo + "<BR/>";
                try
                {
                    mail(message, To, Locname, OpportID);
                    objMapData.ConnConfig = Session["config"].ToString();
                    objMapData.TicketID = Convert.ToInt32(TicketID);
                    objBL_MapData.ResetisSendmailtosalesper(objMapData);
                }
                catch { }
            }

        }
    }

    private void mail(string message, string To, string Locname, string OppID)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        //Thread email = new Thread(delegate ()
        //{
        //string from = string.Empty;
        //DataSet dsC = new DataSet();
        //objPropUser.ConnConfig = Session["config"].ToString();
        //dsC = objBL_User.getControl(objPropUser);
        //if (dsC.Tables[0].Rows.Count > 0)
        //{
        //    from = dsC.Tables[0].Rows[0]["Email"].ToString();
        //}

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
        //});
        //email.IsBackground = true;
        //email.Start();
    }

    private void GetPOitem()
    {
        if (Request.QueryString["id"] != null)
        {
            PO _objPO = new PO();
            BL_Bills _objBLBills = new BL_Bills();
            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.POID = Convert.ToInt32(Request.QueryString["id"]);
            DataSet ds = _objBLBills.GetPOByTicketId(_objPO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                RadGVPO.DataSource = ds.Tables[0];
                RadGVPO.VirtualItemCount = ds.Tables[0].Rows.Count;
            }
            else
            {
                RadGVPO.DataSource = null;

            }
        }
    }

    protected void btnaddpo_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            if (Request.QueryString["fr"] != null && !string.IsNullOrEmpty(Request.QueryString["fr"]))
                Response.Redirect("addpo.aspx?TicketId=" + Request.QueryString["id"].ToString() + "&comp=" + Request.QueryString["comp"].ToString() + "&pop=1&fr=" + Request.QueryString["fr"]);
            else
                Response.Redirect("addpo.aspx?TicketId=" + Request.QueryString["id"].ToString() + "&comp=" + Request.QueryString["comp"].ToString() + "&pop=1");
        }
    }

    protected void btnCodes_Click(object sender, EventArgs e)
    {
        ViewState["codes"] = "0";
        pnlCodes.Visible = true;
        ddlCodeCat.SelectedIndex = 0;
        lblCodeHeader.Text = "Call Codes";
        ddlCodeCat_SelectedIndexChanged(sender, e);
        //ddlCodeCat.Focus();
        hdnFocus.Value = ddlCodeCat.ClientID;

    }

    protected void btnGetCode_Click(object sender, EventArgs e)
    {
        GetJobCode();
        SelectTaskCategory();
        lnkProjectID.NavigateUrl = "addproject.aspx?uid=" + hdnProjectId.Value;
        if (txtJobCode.Text == string.Empty && ddlStatus.SelectedValue == "4")
        {
            foreach (GridDataItem gr in RadgvProjectCode.Items)
            {
                Label lblIDname = (Label)gr.FindControl("lblIDname");
                Label lblIDname1 = (Label)gr.FindControl("lblIDname1");
                hdnProjectCode.Value = lblIDname.Text;
                txtJobCode.Text = lblIDname1.Text;
                break;
            }
        }

        RadgvProjectCode.Rebind();


    }

    protected void ddlCodeCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        objGeneral.CodeCategory = ddlCodeCat.SelectedValue;

        if (ViewState["codes"].ToString() == "0")
        {
            objGeneral.CodeType = 0;
        }
        else
        {
            objGeneral.CodeType = 1;
        }

        DataSet ds = new DataSet();
        ds = objBL_General.getDiagnostic(objGeneral);
        chklstCodes.DataSource = ds.Tables[0];
        chklstCodes.DataTextField = "fdesc";
        chklstCodes.DataValueField = "fdesc";
        chklstCodes.DataBind();

        //ddlCodeCat.Focus();

        string script = "function f(){$find(\"" + RadWindowReasonForServervice.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key11", script, true);
    }

    protected void btnCodesCmpl_Click(object sender, EventArgs e)
    {
        ViewState["codes"] = "1";
        pnlCodes.Visible = true;
        ddlCodeCat.SelectedIndex = 0;
        lblCodeHeader.Text = "Resolution Codes";
        ddlCodeCat_SelectedIndexChanged(sender, e);
        hdnFocus.Value = ddlCodeCat.ClientID;
    }

    protected void addnewproject_Click(object sender, EventArgs e)
    {

    }

    protected void lblRelatedTickets_Click(object sender, EventArgs e)
    {
        if (txtWO.Text != string.Empty)
        {
            FillRelatedTickets();
            RadGvlstRelatedTickets.Rebind();
            string script = "function f(){$find(\"" + RadWindow1RelatedTicket.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key323", script, true);
        }

    }

    protected void RadAjaxManager_AddTicket_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {

    }


    // For Create Multiple Ticket


    protected void btnchkCreateMultipleTicket_Click(object sender, EventArgs e)
    {

        ddlRoute1.SelectedIndex = ddlRoute.SelectedIndex = -1;
        divForCreateMultipleTicket2.Visible = divForCreateMultipleTicket.Visible = chkCreateMultipleTicket.Checked;
        divForCreateMultipleTicket3.Visible = !chkCreateMultipleTicket.Checked;
        ddlStatus1.SelectedValue = ddlStatus.SelectedValue = chkCreateMultipleTicket.Checked ? "1" : ddlStatus.SelectedValue;
        ddlStatus1.Enabled = ddlStatus.Enabled = chkCreateMultipleTicket.Checked ? false : true;
        ddlStatus_SelectedIndexChanged(sender, e);
        RequiredFieldValidator25.Enabled = ddlRoute1.Enabled = ddlRoute.Enabled = !chkCreateMultipleTicket.Checked;
    }

    protected void RadgvEquip_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        // RadgvEquip.AllowCustomPaging = !ShouldApplySortFilterOrGroup(RadgvEquip);
        // RadgvEquip.Rebind();
    }

    bool isGrouping = false;

    public bool ShouldApplySortFilterOrGroup(RadGrid RD)
    {
        return RD.MasterTableView.FilterExpression != "" ||
            (RD.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RD.MasterTableView.SortExpressions.Count > 0;
    }

    private void FillZone()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getZone(objPropUser);
        ddlZone.DataSource = ds.Tables[0];
        ddlZone.DataTextField = "Name";
        ddlZone.DataValueField = "ID";
        ddlZone.DataBind();
        ddlZone.Items.Insert(0, new ListItem("Select", "0"));
    }


    private bool IsZoneEnabled()
    {
        bool zone = false;
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = objBL_General.getCustomFieldsControl(objGeneral);
        if (_dsCustom.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            {
                if (_dr["Name"].ToString().Equals("Zone"))
                {
                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() == "1")
                    {
                        zone = true;
                    }
                }
            }
        }
        return zone;
    }

    private void ChangeImage()
    {
        try
        {
            var invoiceId = txtInvoiceNo.Text;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = Int32.Parse(invoiceId);
            DataSet ds = new DataSet();
            ds = objBL_Contracts.GetStatusNameByInvoiceId(objProp_Contracts);

            string img = "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                var statusName = ds.Tables[0].Rows[0]["StatusName"];
                if (invoiceId.Trim() != "" && statusName.ToString() == "Marked as Pending")
                {
                    img = "images/DollarOrange.png";
                }
                else if (invoiceId.Trim() != "" && statusName.ToString() != "Marked as Pending")
                {
                    img = "images/dollar.png";
                }
            }

            imgInv.ImageUrl = img;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    #region inventory
    private void SetInitialRow()    //Initialization of Datatable.
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("RowID", typeof(string)));
            dt.Columns.Add(new DataColumn("ID", typeof(Int32)));        // PO
            dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
            dt.Columns.Add(new DataColumn("fDesc", typeof(string)));  // fDesc
            dt.Columns.Add(new DataColumn("Quan", typeof(string)));
            dt.Columns.Add(new DataColumn("Price", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("Phase", typeof(string)));
            dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));   // Phase
            dt.Columns.Add(new DataColumn("Inv", typeof(Int32)));       // Inv
            dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));   //dt.Columns.Add(new DataColumn("Due", typeof(DateTime)));      //due date
            dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            dt.Columns.Add(new DataColumn("WarehouseID", typeof(string)));
            dt.Columns.Add(new DataColumn("LocationID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Warehousefdesc", typeof(string)));
            dt.Columns.Add(new DataColumn("Locationfdesc", typeof(string)));
            dt.Columns.Add(new DataColumn("OpSq", typeof(String)));
            dt.Columns.Add(new DataColumn("AID", typeof(String)));
            int rowIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                dr = dt.NewRow();
                dr["RowID"] = i + 1;
                dr["Line"] = i + 1;
                dt.Rows.Add(dr);
                rowIndex++;
            }

            ViewState["Transactions"] = dt;

            BIND_RadGrid_Inventory(dt);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable GetInventoryForSaveItems()
    {

        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("Ticket", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
        dt.Columns.Add(new DataColumn("Item", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Charge", typeof(double)));
        dt.Columns.Add(new DataColumn("Amount", typeof(string)));
        dt.Columns.Add(new DataColumn("Phase", typeof(Int32)));
        dt.Columns.Add(new DataColumn("AID", typeof(string)));
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("WarehouseID", typeof(string)));
        dt.Columns.Add(new DataColumn("LocationID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("PhaseName", typeof(string)));


        try
        {
            string strItems = hdnItemJSON.Value.Trim();
            if (strItems != string.Empty)
            {
                System.Web.Script.Serialization.JavaScriptSerializer sr = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {

                    if (dict["hdnItemID"].ToString().Trim() == string.Empty)
                    {
                        continue;
                    }
                    i++;

                    DataRow dr = dt.NewRow();

                    Int32 tINVID = 0;



                    dr["Ticket"] = tINVID;

                    if (!(dict["hdnLine"].ToString().Trim() == string.Empty))
                    {
                        dr["Line"] = Convert.ToInt16(dict["hdnLine"].ToString());
                    }

                    dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();

                    dr["Quan"] = !string.IsNullOrEmpty(dict["txtGvQuan"].ToString()) ? dict["txtGvQuan"].ToString() : "0";

                    dr["Amount"] = !string.IsNullOrEmpty(dict["txtGvAmount"].ToString()) ? dict["txtGvAmount"].ToString() : "0";

                    if (dict.ContainsKey("chkBill"))
                    {
                        dr["Charge"] = 1;
                    }
                    else
                    {
                        if (dict.ContainsKey("hdnBill"))
                        {
                            dr["Charge"] = !string.IsNullOrEmpty(dict["hdnBill"].ToString()) ? Convert.ToInt32(dict["hdnBill"].ToString()) : 0;
                        }
                        else
                        {
                            dr["Charge"] = 0;
                        }
                    }

                    dr["Phase"] = "1";

                    dr["PhaseName"] = "Materials";

                    dr["Item"] = Convert.ToInt32(dict["hdnItemID"]);

                    dr["WarehouseID"] = dict["hdnWarehouse"].ToString();

                    int WarehouseLocationID = 0;

                    int.TryParse(dict["hdnWarehouseLocationID"].ToString(), out WarehouseLocationID);

                    dr["LocationID"] = WarehouseLocationID;

                    dr["AID"] = dict["hdnAID"].ToString();

                    dt.Rows.Add(dr);

                    i++;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    private DataTable GetInventoryGridItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
        dt.Columns.Add(new DataColumn("hdnAID", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        dt.Columns.Add(new DataColumn("Price", typeof(string)));
        dt.Columns.Add(new DataColumn("Amount", typeof(string)));
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Inv", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("WarehouseID", typeof(string)));
        dt.Columns.Add(new DataColumn("LocationID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Warehousefdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Locationfdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("OpSq", typeof(String)));
        dt.Columns.Add(new DataColumn("AID", typeof(string)));
        try
        {
            string strItems = hdnItemJSON.Value.Trim();
            if (strItems != string.Empty)
            {
                System.Web.Script.Serialization.JavaScriptSerializer sr = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {

                    i++;
                    DataRow dr = dt.NewRow();

                    dr["AID"] = dict["hdnAID"].ToString();

                    if (!(dict["hdnLine"].ToString().Trim() == string.Empty))
                    {
                        dr["Line"] = Convert.ToInt16(dict["hdnLine"].ToString());
                    }

                    dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();
                    dr["Quan"] = dict["txtGvQuan"].ToString();
                    dr["Price"] = dict["txtGvPrice"].ToString().Trim();
                    dr["Amount"] = dict["txtGvAmount"].ToString().Trim();

                    //if (dict.ContainsKey("chkBill"))
                    //{
                    //    dr["Billed"] = 1;
                    //}
                    //else
                    //{
                    //    dr["Billed"] = 0;
                    //}
                    if (dict.ContainsKey("chkBill"))
                    {
                        dr["Billed"] = 1;
                    }
                    else
                    {
                        if (dict.ContainsKey("hdnBill"))
                        {
                            dr["Billed"] = !string.IsNullOrEmpty(dict["hdnBill"].ToString()) ? Convert.ToInt32(dict["hdnBill"].ToString()) : 0;
                        }
                        else
                        {
                            dr["Billed"] = 0;
                        }
                    }

                    if (!(dict["txtGvPhase"].ToString().Trim() == string.Empty))
                    {
                        dr["Phase"] = dict["txtGvPhase"].ToString().Trim();
                    }
                    if (!(dict["hdnPID"].ToString().Trim() == string.Empty))
                    {
                        dr["PhaseID"] = Convert.ToInt32(dict["hdnPID"].ToString());
                    }
                    if (!(dict["hdnTypeId"].ToString().Trim() == string.Empty))
                    {
                        dr["TypeID"] = Convert.ToInt32(dict["hdnTypeId"].ToString().Trim());
                    }

                    if (!(dict["hdnItemID"].ToString().Trim() == string.Empty))
                    {
                        dr["Inv"] = Convert.ToInt32(dict["hdnItemID"]);
                    }
                    if (!(dict["txtGvItem"].ToString().Trim() == string.Empty))
                    {
                        dr["ItemDesc"] = dict["txtGvItem"].ToString();
                    }

                    if (!(dict["hdnWarehouse"].ToString().Trim() == string.Empty))
                    {
                        dr["WarehouseID"] = dict["hdnWarehouse"].ToString();
                    }

                    int WarehouseLocationID = 0;

                    int.TryParse(dict["hdnWarehouseLocationID"].ToString(), out WarehouseLocationID);

                    dr["LocationID"] = WarehouseLocationID;

                    if (!(dict["txtGvWarehouse"].ToString().Trim() == string.Empty))
                        dr["Warehousefdesc"] = dict["txtGvWarehouse"].ToString();

                    if (!(dict["txtGvWarehouseLocation"].ToString().Trim() == string.Empty))
                        dr["Locationfdesc"] = dict["txtGvWarehouseLocation"].ToString();

                    dt.Rows.Add(dr);
                    i++;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    protected void RadGrid_Inventory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            int rowIndex = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "DeleteTransaction")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                //GridViewRow row = gvGLItems.Rows[index];
                GridDataItem row = RadGrid_Inventory.Items[index];


                TextBox txtGvDesc = (TextBox)row.FindControl("txtGvDesc");
                TextBox txtGvAmount = (TextBox)row.FindControl("txtGvAmount");
                TextBox txtGvQuan = (TextBox)row.FindControl("txtGvQuan");
                TextBox txtGvPrice = (TextBox)row.FindControl("txtGvPrice");
                TextBox txtGvPhase = (TextBox)row.FindControl("txtGvPhase");
                TextBox txtGvItem = (TextBox)row.FindControl("txtGvItem");
                CheckBox chkBill = (CheckBox)row.FindControl("chkBill");
                HiddenField hdnBill = (HiddenField)row.FindControl("hdnBill");
                HiddenField hdnPID = (HiddenField)row.FindControl("hdnPID");
                HiddenField hdnTypeId = (HiddenField)row.FindControl("hdnTypeId");
                HiddenField hdnItemID = (HiddenField)row.FindControl("hdnItemID");
                HiddenField hdnWarehouse = (HiddenField)row.FindControl("hdnWarehouse");
                HiddenField hdnWarehouseLocationID = (HiddenField)row.FindControl("hdnItemID");
                HiddenField hdntxtGvPhase = (HiddenField)row.FindControl("hdntxtGvPhase");

                txtGvDesc.Text = "";
                txtGvPrice.Text = "";
                txtGvQuan.Text = "";
                txtGvAmount.Text = "";
                txtGvPhase.Text = "";
                txtGvItem.Text = "";
                chkBill.Checked = false;
                hdnBill.Value = "0";
                hdnPID.Value = "";
                hdnTypeId.Value = "";
                hdnItemID.Value = "";
                hdnWarehouse.Value = "";
                hdnWarehouseLocationID.Value = "";
                hdntxtGvPhase.Value = "";

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnAddNewLines_Click(object sender, EventArgs e)
    {
        try
        {

            int rowIndex = RadGrid_Inventory.Items.Count - 1;

            DataTable dt = GetInventoryGridItems();
            Int32 Line = 1;//Default if dt is null
            if (dt != null && dt.Rows.Count > 0)
            {
                Line = Convert.ToInt32(Convert.ToString(dt.Rows[dt.Rows.Count - 1]["Line"]) == "" ? "0" : Convert.ToString(dt.Rows[dt.Rows.Count - 1]["Line"]));
            }
            DataRow dr = null;
            for (int i = 0; i < 1; i++)
            {
                Line = Line + 1;
                dr = dt.NewRow();
                dr["Line"] = Line;
                dr["AID"] = "";
                dt.Rows.Add(dr);

            }
            BIND_RadGrid_Inventory(dt);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillInventoryGrid()
    {
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
        Boolean TrackingInventory = false;
        if (_dsCustom.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            {
                if (_dr["Name"].ToString().Equals("InvGL"))
                {
                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                    {
                        TrackingInventory = Convert.ToBoolean(_dr["Label"]);
                    }
                }
            }
        }

        //RadGrid_Inventory.Visible = TrackingInventory;
        RadGrid_Inventory.Visible = true;


        TicketI ticket = new TicketI();

        BL_Tickets _BL_Tickets = new BL_Tickets();

        ticket.ConnConfig = Session["config"].ToString();

        ticket.TicketID = int.Parse(Request.QueryString["id"].ToString());

        DataSet ds = _BL_Tickets.GetTicketIByID(ticket);

        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));        // PO
        dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));  // fDesc
        dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        dt.Columns.Add(new DataColumn("Price", typeof(string)));
        dt.Columns.Add(new DataColumn("Amount", typeof(string)));
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));   // Phase
        dt.Columns.Add(new DataColumn("Inv", typeof(Int32)));       // Inv
        dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));    // Billed 
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("WarehouseID", typeof(string)));
        dt.Columns.Add(new DataColumn("LocationID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Warehousefdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Locationfdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("OpSq", typeof(String)));
        dt.Columns.Add(new DataColumn("AID", typeof(String)));

        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DataRow dr1 = dt.NewRow();
                dr1["Line"] = dr["Line"].ToString();
                dr1["fDesc"] = dr["fDesc"].ToString();
                dr1["Quan"] = dr["Quan"].ToString();
                dr1["Price"] = (Convert.ToDouble(dr["Amount"].ToString()) / Convert.ToDouble(dr["Quan"].ToString())).ToString("C").Replace("$", "");
                dr1["Amount"] = dr["Amount"].ToString();
                dr1["Phase"] = dr["PhaseName"].ToString();
                dr1["PhaseID"] = int.Parse(dr["Phase"].ToString());
                dr1["Inv"] = int.Parse(dr["Item"].ToString());
                dr1["Billed"] = int.Parse(dr["Charge"].ToString());
                dr1["TypeID"] = int.Parse(dr["TypeID"] == DBNull.Value ? "0" : dr["TypeID"].ToString());
                dr1["ItemDesc"] = (dr["Name"].ToString());
                dr1["WarehouseID"] = dr["WarehouseID"].ToString();
                dr1["LocationID"] = string.IsNullOrEmpty(dr["LocationID"].ToString()) ? 0 : int.Parse(dr["LocationID"].ToString());
                //dr1["Warehousefdesc"] = dr["WarehouseID"].ToString() + "," + dr["WarehouseName"].ToString();
                dr1["Warehousefdesc"] = dr["WarehouseName"].ToString();
                dr1["Locationfdesc"] = dr1["LocationID"].ToString() == "0" ? "" : dr1["LocationID"] + "," + dr["WHLoc"].ToString();
                dr1["AID"] = dr["AID"].ToString();
                dt.Rows.Add(dr1);
            }
            BIND_RadGrid_Inventory(dt);
        }
    }

    private void BIND_RadGrid_Inventory(DataTable dt)
    {

        RadGrid_Inventory.DataSource = dt;
        RadGrid_Inventory.DataBind();
    }
    #endregion

    #region logs
    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        if (hdnloadlogtab.Value == "1")
        {
            RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
            if (Request.QueryString["id"] != null)
            {
                DataSet dsLog = new DataSet();
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
                dsLog = objBL_MapData.GetTicketLogs(objMapData);
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
        //if (e.Item is GridPagerItem)
        //{
        //    var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
        //    var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

        //    if (totalCount == 0) totalCount = 1000;

        //    GeneralFunctions obj = new GeneralFunctions();

        //    var sizes = obj.TelerikPageSize(totalCount);

        //    dropDown.Items.Clear();

        //    foreach (var size in sizes)
        //    {
        //        var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
        //        cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
        //        if (e.Item.OwnerTableView.PageSize.ToString() == size.Value) cboItem.Selected = true;
        //        dropDown.Items.Add(cboItem);
        //    }
        //}
    }

    private bool ValidateINV_GRID(DataTable dt)
    {

        foreach (DataRow dr in dt.Rows)
        {
            if (!string.IsNullOrEmpty(dr["Item"].ToString().Trim()))
            {
                if (!string.IsNullOrEmpty(dr["Phase"].ToString().Trim()))
                {
                    double QunaINV = 0.00;
                    double.TryParse(dr["Quan"].ToString().Trim(), out QunaINV);

                    if (QunaINV == 0)
                    {
                        ShowMessage("Please enter a quantity for the inventory items." + dr["fDesc"].ToString(), 0);
                        return false;
                    }

                    if (QunaINV > 0)
                    {
                        if (string.IsNullOrEmpty(dr["WarehouseID"].ToString().Trim()))
                        {
                            ShowMessage("Please enter a warehouse for the inventory items." + dr["fDesc"].ToString(), 0);
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Show Message
    /// </summary>
    /// <param name="mesg"></param>
    /// <param name="type"> if (type == 0)   /// Warning message  if (type == 1)  /// Success message</param>
    /// 

    private void ShowMessage(string mesg, Int16 type)
    {
        if (type == 0)            /// Warning message
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keywarning1", "noty({text: '" + mesg + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        else if (type == 1)       /// Success message
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysucc1", "noty({text: '" + mesg + "',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

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

    protected void lnkProcessInvoicing_Click(object sender, EventArgs e)
    {
        string script = "function f(){$find(\"" + RadWindow1.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key32dfd3", script, true);
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
            ReportViewer ReportViewer1 = new ReportViewer();
            Detailreport(ReportViewer1);

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

    private void Detailreport(ReportViewer ReportViewer1)
    {
        string Signedby = string.Empty;

        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
        objMapData.ISTicketD = 0;
        ds = objBL_MapData.GetTicketByID(objMapData);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Signedby = ds.Tables[0].Rows[0]["custom1"].ToString();
            ds.Tables[0].Columns.Add("rtaddress");
            ds.Tables[0].Columns.Add("osaddress");
            ds.Tables[0].Columns.Add("ctaddress");
        }

        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Username = Session["username"].ToString();
        DataSet dsC = new DataSet();
        if (Session["MSM"].ToString() != "TS")
        {
            dsC = objBL_User.getControl(objPropUser);
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["lid"].ToString());
            dsC = objBL_User.getControlBranch(objPropUser);
        }

        string reportPath = "Reports/Ticket.rdlc";
        string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["TicketReport"].Trim();
        if (!string.IsNullOrEmpty(Report.Trim()))
        {
            reportPath = "Reports/" + Report.Trim();
        }
        DataSet dsEquip = objBL_MapData.getElevByTicket(objMapData);
        DataSet dsOtherWorker = new DataSet();

        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dtEquipDetails", dsEquip.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", ds.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtMCP", FillREPHistory()));
        if (ds.Tables.Count > 1)
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtPOItem", ds.Tables[1]));
        if (ds.Tables.Count > 2)
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicketI", ds.Tables[2]));
        ReportViewer1.LocalReport.ReportPath = reportPath;
        ReportViewer1.LocalReport.EnableExternalImages = true;
        List<ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        param1.Add(new ReportParameter("Path", "~/companylogo.ashx"));
        param1.Add(new ReportParameter("Custom", Signedby));
        param1.Add(new ReportParameter("TicketID", Request.QueryString["id"].ToString()));

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
        ReportViewer1.LocalReport.Refresh();
    }

    private void ReportMRTDownload()
    {
        try
        {
            byte[] buffer = null;

            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(DetailReportMRT(), stream, settings);
            buffer = stream.ToArray();

            HttpContext.Current.Response.Clear();
            MemoryStream ms = new MemoryStream(buffer);
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("Transfer-Encoding", "identity");
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=Ticket.pdf");
            HttpContext.Current.Response.Buffer = true;
            ms.WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.End();
        }
        catch (Exception ex)
        {

        }
    }

    private StiReport DetailReportMRT()
    {
        string reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketReport.mrt");
        if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["TicketReport"]) && WebConfigurationManager.AppSettings["TicketReport"].ToLower().Contains(".mrt"))
        {
            reportPathStimul = Server.MapPath($"StimulsoftReports/Tickets/{WebConfigurationManager.AppSettings["TicketReport"]}");
        }

        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        //report.Compile();

        report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();

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

        // Company details
        DataSet dsCompany = new DataSet();
        BL_Report bL_Report = new BL_Report();
        dsCompany = bL_Report.GetCompanyDetails(Session["config"].ToString());

        report.RegData("CompanyDetails", dsCompany.Tables[0]);

        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
        objMapData.ISTicketD = 0;
        DataSet ds = objBL_MapData.GetTicketByID(objMapData);
        DataSet dsEquip = objBL_MapData.getElevByTicket(objMapData);

        if (ds != null)
        {
            report.RegData("ReportData", ds.Tables[0]);

            if (ds.Tables.Count > 1)
            {
                report.RegData("dtPOItem", ds.Tables[1]);
            }

            if (ds.Tables.Count > 2)
            {
                report.RegData("dtTicketItem", ds.Tables[2]);
            }
        }

        if (dsEquip != null)
        {
            report.RegData("dtEquipment", dsEquip.Tables[0]);
        }

        report.RegData("dtMCP", FillREPHistory());
        report.CacheAllData = true;
        report.Render();

        return report;
    }

    private DataTable FillREPHistory()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = 0;
        objPropUser.SearchBy = "rd.ticketID";
        objPropUser.SearchValue = Request.QueryString["id"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_User.getequipREPDetails(objPropUser);

        return ds.Tables[0];
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {

    }


    protected void RadgvProject_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GetDataProject();
    }

    protected void RadgvProjectCode_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (hdnProjectId.Value != string.Empty)
        {
            Customer objProp_Customer = new Customer();
            BL_Customer objBL_Customer = new BL_Customer();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProjectJobID = Convert.ToInt32(hdnProjectId.Value);
            objProp_Customer.Type = "1";
            if (Convert.ToInt32(hdnProjectId.Value) != 0)
            {
                DataSet ds = objBL_Customer.GetjobcodeInfo(objProp_Customer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    RadgvProjectCode.VirtualItemCount = ds.Tables[1].Rows.Count;
                    RadgvProjectCode.DataSource = ds.Tables[1];
                }
            }
        }
    }

    protected void lnkloadlogtab_Click(object sender, EventArgs e)
    {
        RadGrid_gvLogs.Visible = true;
        hdnloadlogtab.Value = "1";
        RadGrid_gvLogs.Rebind();
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["fr"] != null && Request.QueryString["fr"] == "tlv")
        {
            Response.Redirect("TicketListView.aspx?fil=1");
        }
        else if (Request.QueryString["fr"] != null && Request.QueryString["fr"] == "proj")
        {
            if (!string.IsNullOrEmpty(hdnProjectId.Value))
                Response.Redirect("addProject?uid=" + hdnProjectId.Value);
        }
        else if (Request.QueryString["screen"] != null)
        {
            if (Request.QueryString["screen"] == "STList")
            {
                Response.Redirect("SafetyTest.aspx?fil=1");
            }
            else if (Request.QueryString["screen"] == "STEdit")
            {
                string elv = Request.QueryString["elv"];

                string STEditLID = Request.QueryString["LID"];

                Response.Redirect("AddTests.aspx?elv=" + elv + "&LID=" + STEditLID);
            }
        }
        else
        {
            Response.Redirect("TicketListView.aspx");
        }
    }

    protected void chkPayroll_CheckedChanged(object sender, EventArgs e)
    {
        if (ViewState["MassPayrollTicket"].ToString() == "Y")
        {
            PayRollPermission();
        }
        else
        {

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "325512124", "noty({text: 'You do not have permissions',  type : 'warning', layout:'topCenter',closeOnSelfClick:true,dismissQueue: true, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    public void PayRollPermission()
    {
        btnEnroute.Enabled =
             btnOnsite.Enabled =
              btnComplete.Enabled =
       ddlWage.Enabled=
        ddlRoute.Enabled =
        ddlRoute1.Enabled =
        ddlStatus.Enabled =
        ddlStatus1.Enabled =
        txtEnrTime.Enabled =
        txtOnsitetime.Enabled =
        txtComplTime.Enabled =
        txtRT.Enabled =
        txtDT.Enabled =
        txtOT.Enabled =
        txtNT.Enabled =
        txtTT.Enabled =
        txtBT.Enabled =
        txtExpMisc.Enabled =
        txtExpZone.Enabled =
        txtExpToll.Enabled =
        txtMileEnd.Enabled =
        txtMileStart.Enabled = !chkPayroll.Checked;

    }

    public void GetARByLocation()
    {
        StringBuilder str = new StringBuilder();

        str.AppendFormat("<table><tr><td>Balance</td><td>0</td></tr>");
        str.AppendFormat("<tr><td>0-30 Days</td><td>0</td></tr>");
        str.AppendFormat("<tr><td>31-60 Days</td><td>0</td></tr>");
        str.AppendFormat("<tr><td>61-90 Days</td><td>0</td></tr>");
        str.AppendFormat("<tr><td>91-120 Days</td><td>0</td></tr>");
        str.AppendFormat("<tr><td>121+ Days</td><td>0</td></tr></table>");
        //String str = "";
        Contracts objContract = new Contracts();
        BL_Contracts objBL_Contracts = new BL_Contracts();

        if (hdnLocId.Value == "")
        {
            LocBalance.Text = str.ToString();
            return;
        }

        objContract.ConnConfig = Session["config"].ToString();
        objContract.Date = DateTime.Now;
        objContract.isDBTotalService = false;
        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["isDBTotalService"]))
        {
            objContract.isDBTotalService = Convert.ToBoolean(ConfigurationManager.AppSettings["isDBTotalService"]);
        }
        objContract.Loc = Convert.ToInt32(hdnLocId.Value);
        DataSet ds = objBL_Contracts.GetARAgingByLocation(objContract);
        try
        {
            if (Convert.ToDouble(ds.Tables[0].Rows[0]["Balance"]) != 0)
            {
                imgDollar.Visible = true;
                str.Clear();
                str.AppendFormat("<table><tr><td>Balance</td><td>{0}</td></tr>", string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["Balance"])));
                str.AppendFormat("<tr><td>0-30 Days</td><td>{0}</td></tr>", string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["ThirtyDay"])));
                str.AppendFormat("<tr><td>31-60 Days</td><td>{0}</td></tr>", string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["SixtyDay"])));
                str.AppendFormat("<tr><td>61-90 Days</td><td>{0}</td></tr>", string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["NintyDay"])));
                str.AppendFormat("<tr><td>91-120 Days</td><td>{0}</td></tr>", string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["OverNintyDay"])));
                str.AppendFormat("<tr><td>121+ Days</td><td>{0}</td></tr></table>", string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["OneTwentyDay"])));

                LocBalance.Text = str.ToString();
            }
            else
            {
                imgDollar.Visible = false;
            }

        }
        catch (Exception ex)
        {

            LocBalance.Text = str.ToString();
        }
    }

}




