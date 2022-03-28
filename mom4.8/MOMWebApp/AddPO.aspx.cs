using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.IO;
using Stimulsoft.Report;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using Stimulsoft.Report.Web;
using iTextSharp.text.pdf;
using context = System.Web.HttpContext;

public partial class AddPO : System.Web.UI.Page
{
    #region Variable
    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();
    BL_Report bL_Report = new BL_Report();
    PO _objPO = new PO();
    ApprovePOStatus _objApprovePOStatus = new ApprovePOStatus();
    BL_Bills _objBLBills = new BL_Bills();
    BL_Vendor _objBLVendor = new BL_Vendor();
    Vendor _objVendor = new Vendor();
    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();
    BusinessEntity.Inventory _objInv = new BusinessEntity.Inventory();
    MapData objMapData = new MapData();
    BL_MapData objBL_MapData = new BL_MapData();
    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();
    private string _accountName;
    string reportPOID = string.Empty;
    private int _poApprovalStatus = 0;
    StringBuilder _InvIds = new StringBuilder();
    StringBuilder _ChartIds = new StringBuilder();
    
    
    #endregion

    #region Events
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
                divSuccess.Visible = false;
                txtCreatedBy.Text = Session["User"].ToString();
                FillUserAddress();
                GetDocuments();
                lnk_PO.Visible = true;
                txtPO.Enabled = false;
                txtPO.Visible = false;
                lablPO.Visible = false;
                
                //get the custom program value for Mitsu
                GetCustomProgramForMitsu();

                _objPO.UserID = Convert.ToInt32(Session["userid"]);
                _objPO.ConnConfig = Session["config"].ToString();
                DataSet ApprovePO = _objBLBills.GetPOApproveDetails(_objPO);
                if (ApprovePO.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ApprovePO.Tables[0].Rows[0];
                    hdnPOLimit.Value = Convert.ToString(row["POLimit"]);
                }

                if (Request.QueryString["id"] != null)
                {
                    emptyLinePOReason.Visible = false;
                    emptyLinePORevision.Visible = true;
                    if (Request.QueryString["t"] != null)
                    {
                        lblPO.Visible = false;
                        lblPOId.Visible = false;
                        txtPO.Visible = false;
                        lablPO.Visible = false;
                        //lnk_PO.Visible = false;
                        //lnk_CustomPOReport.Visible = false;
                        pnlNext.Visible = false;
                        lblHeader.Text = "Add New Purchase Order";
                        chkPOClose.Visible = false;
                    }
                    else
                    {
                        Page.Title = "Edit Purchase Order || MOM";
                        lblPO.Visible = true;
                        lblPOId.Visible = true;
                        txtPO.Visible = true;
                        lablPO.Visible = true;
                        //lnkPrint.Visible = true;
                        lnk_PO.Visible = true;
                        lnk_CustomPOReport.Visible = false;

                        if (ConfigurationManager.AppSettings["CustomPOReport"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["CustomPOReport"]))
                        {
                            lnk_CustomPOReport.Visible = true;
                        }

                        pnlNext.Visible = true;
                        lblHeader.Text = "Edit Purchase Order";
                        liLogs.Style["display"] = "inline-block";
                        tbLogs.Style["display"] = "block";
                        chkPOClose.Visible = true;
                        chkAddRPO.Visible = true;
                        
                    }

                    _objPO.ConnConfig = Session["config"].ToString();
                    _objPO.POID = Convert.ToInt32(Request.QueryString["id"]);
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

                 
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        hdnCustomProgram.Value = dr["CustomProgram"].ToString();
                        hdnEN.Value =  dr["EN"].ToString();
                        hdnHasInventory.Value =  dr["InventoryHasAvailable"].ToString();
                        _accountName = dr["Acct"].ToString();
                        _poApprovalStatus = (int)dr["POApprovalStatus"];
                        ddlApprovalStatus.SelectedValue = _poApprovalStatus.ToString();
                        DisableApprovalStatus( _poApprovalStatus);

                        lblPOId.Text = dr["PO"].ToString();
                        txtPO.Text = dr["PO"].ToString();
                        txtVendor.Text = dr["VendorName"].ToString();
                        txtVendorType.Text = dr["VendorType"].ToString();
                        hdnVendorID.Value = dr["Vendor"].ToString();
                        FillAddress(false);
                        FillUserAddress();
                        //txtAddress.Text = dr["Address"].ToString() + Environment.NewLine + dr["city"].ToString() + ", "+ dr["State"].ToString() + ", "+ dr["zip"].ToString();
                        txtAddress.Text = dr["Address"].ToString();
                        txtDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
                        txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToShortDateString();
                        ddlTerms.SelectedValue = dr["PaymentTerms"].ToString();
                        txtFOB.Text = dr["FOB"].ToString();
                        if (!string.IsNullOrEmpty(dr["StatusName"].ToString()))
                        {
                            if (Request.QueryString["t"] != null)
                            {
                                txtStatus.Enabled = true;
                                txtStatus.Text = "Open";
                                hdnStatus.Value = "0";
                            }
                            else
                            {
                                txtStatus.Text = dr["StatusName"].ToString();
                                txtStatus.Enabled = false;
                                hdnStatus.Value = dr["Status"].ToString();

                                if (dr["Status"].ToString() == "1")
                                {
                                    chkPOClose.Enabled = false;
                                    chkAddRPO.Enabled = false;
                                }
                            }

                        }

                        txtRequestedBy.Text = dr["RequestedBy"].ToString();
                        txtShipVia.Text = dr["ShipVia"].ToString();
                        //txtFreight.Text = dr["Freight"].ToString();
                        txtCreatedBy.Text = dr["fBy"].ToString();
                        txtPO1.Text = dr["Custom1"].ToString();
                        txtPO2.Text = dr["Custom2"].ToString();                        
                        lblTotalAmount.Text = dr["Amount"].ToString();
                        lblTotalOpenAmount.Text = dr["OpenAmount"].ToString();
                        hdnTotal.Value = dr["Amount"].ToString();
                        if (Convert.ToInt16(dr["Approved"]).Equals(1))
                        {
                            chkApproved.Checked = true;
                        }
                        txtDesc.Text = dr["fDesc"].ToString();
                        txtShipTo.Text = dr["ShipTo"].ToString();
                        txtPoRevision.Text = dr["PORevision"].ToString();
                        txtSalesOrderNo.Text = dr["SalesOrderNo"].ToString();
                        txtPOCode.Text = dr["POReasonCode"].ToString();
                        txtCourrierAcct.Text = dr["CourrierAcct"].ToString();
                        //gvGLItems.DataSource = ds.Tables[1];
                        //gvGLItems.DataBind();

                        if (txtStatus.Text != "Closed")
                        {
                            DataTable filterdt = new DataTable();
                            DataView dv = ds.Tables[1].DefaultView;
                            dv.RowFilter = "NOT(ForceClose = 1)";
                            filterdt = dv.ToTable();
                            RadGrid_AddPO.DataSource = filterdt;
                            RadGrid_AddPO.DataBind();

                        }
                        else
                        {

                            RadGrid_AddPO.DataSource = ds.Tables[1];
                            RadGrid_AddPO.DataBind();
                        }

                        MitsuChanges();
                        ///// If PO Closed then User can't able to changed info it.
                        if (txtStatus.Text == "Closed")
                        {
                            btnSubmit.Visible = false;
                            foreach (GridDataItem gr in RadGrid_AddPO.Items)
                            {
                                gr.Enabled = false;
                                btnAddNewLines.Visible = btnAddNewLines.Enabled = false;
                            }
                        }

                        //In the Case of Copy Feature
                        if (Request.QueryString["t"] != null)
                        {
                            SetPOForm();
                        }

                        ////only show approve buttons , When PO is not yet approved or its updated for reapproval
                        if (ApprovePO.Tables[0].Rows.Count > 0 && (_poApprovalStatus == 0 || _poApprovalStatus == 3))
                        {
                            DataRow row = ApprovePO.Tables[0].Rows[0];
                            if (Request.QueryString["id"] != null)
                            {
                                if (Convert.ToDecimal(row["POApprove"]) == 1)
                                {
                                    if (Convert.ToString(row["POApproveAmt"]) == "0")
                                    {
                                        if (Convert.ToDecimal(hdnTotal.Value) >= Convert.ToDecimal(row["MinAmount"]) && Convert.ToDecimal(hdnTotal.Value) <= Convert.ToDecimal(row["MaxAmount"]))
                                        {
                                            divStatusComments.Visible = true;
                                            divSignature.Visible = true;
                                            btnDecline.Visible = true;
                                            btnApprove.Visible = true;
                                        }
                                    }
                                    else if (Convert.ToString(row["POApproveAmt"]) == "1")
                                    {
                                        if (Convert.ToDecimal(row["MinAmount"]) >= Convert.ToDecimal(hdnTotal.Value))
                                        {
                                            divStatusComments.Visible = true;
                                            divSignature.Visible = true;
                                            btnDecline.Visible = true;
                                            btnApprove.Visible = true;
                                        }
                                    }
                                }
                                // for test only
                                //divStatusComments.Visible = true;
                                //divSignature.Visible = true;
                                //btnDecline.Visible = true;
                                //btnApprove.Visible = true;
                            }
                        }

                       
                    }
                    _objApprovePOStatus.ConnConfig = Session["config"].ToString();
                    _objApprovePOStatus.POID = Convert.ToInt32(Request.QueryString["id"]);
                    DataSet ApprovalStatus = _objBLBills.GetPOSign(_objApprovePOStatus);
                    if (ApprovalStatus.Tables[0].Rows.Count > 0)
                    {
                        //DataRow row = ApprovePOLimit.Tables[0].Rows[0];
                        string signature = null;
                        if (ApprovalStatus.Tables[0].Rows[0]["Signature"] != DBNull.Value)
                        {
                            signature = "data:image/png;base64," + Convert.ToBase64String((byte[])ApprovalStatus.Tables[0].Rows[0]["Signature"]);
                        }
                        if (!string.IsNullOrEmpty(signature))
                        {
                            imgSign.ImageUrl = signature;
                            hdnImg.Value = signature;
                        }

                    }
                }
                else
                {
                    emptyLinePOReason.Visible = true;
                    emptyLinePORevision.Visible = false;
                    Page.Title = "Add Purchase Order || MOM";
                    txtStatus.Enabled = true;
                    txtStatus.Text = "Open";
                    SetPOForm();
                }
                if (!string.IsNullOrEmpty(txtDate.Text))
                {
                    GetPeriodDetails(Convert.ToDateTime(txtDate.Text));
                }
                FillTerms();
                FillBomType();

                // Add Custom PO template menu
                listCustomPO.DataSource = GetListPOTemplate();
                listCustomPO.DataBind();

                DataSet dsTerm = _objBLBills.GetAddPOTerms(_objPO);
                if (dsTerm.Tables[0].Rows.Count > 0)
                {
                    lblTC.Text = dsTerm.Tables[0].Rows[0]["TermsConditions"].ToString().Replace("\n", "<br />");
                }
                txtQty.Text = "0.00";
                txtBudgetUnit.Text = "0.00";
                lblBudgetExt.Text = "0.00";
                GetTicketInfo();
                HighlightSideMenu("purchaseMgr", "lnkPO", "purchaseMgrSub");

                #region TrackingInventory

                ////TrackingInventory
                General _objPropGeneral = new General();
                BL_General _objBLGeneral = new BL_General();

                _objPropGeneral.ConnConfig = Session["config"].ToString();
                DataSet _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");
                Boolean TrackingInventory = false;
                if (_dsCustom.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                        {
                            TrackingInventory = Convert.ToBoolean(_dr["Label"]);
                        }
                    }
                }

                if (TrackingInventory == false)
                {
                  

                    RadGrid_AddPO.Columns.FindByUniqueName("WarehouseID").Visible = false;
                    RadGrid_AddPO.Columns.FindByUniqueName("LocationID").Visible = false;
                     
                }
                else
                {
                    RadGrid_AddPO.Columns.FindByUniqueName("WarehouseID").Visible = true;
                    RadGrid_AddPO.Columns.FindByUniqueName("LocationID").Visible = true;
                    GetInvDefaultAcct();
                }
                #endregion

                if (Session["POErrMess"] != null)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + Session["POErrMess"] + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    Session["POErrMess"] = null;
                }

                if (Session["POSuccMess"] != null)
                {
                    if (Session["POSuccMessWarn"] != null) 
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'PO will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                    }
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + Session["POSuccMess"] + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    Session["POSuccMess"] = null;
                    Session["POSuccMessWarn"] = null;
                }

                if (ConfigurationManager.AppSettings["CustomerName"].ToLower() == "brock")
                {
                    lnk_PO.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        ///
        userpermissions();
        DocumentPermission();
        fillPOCustom();
    }
    #endregion

    private void GetCustomProgramForMitsu()
    {
        hdnCustomProgram.Value  = _objBLBills.GetCustomProgramForMitsu(Session["config"].ToString());
      
    }
    private void MitsuChanges()
    {
        
        if (hdnCustomProgram.Value != string.Empty && hdnCustomProgram.Value.ToString().ToLower() == "mitsu")
        {

            foreach (GridHeaderItem itm in RadGrid_AddPO.MasterTableView.GetItems(GridItemType.Header))
            {
                itm["ProjectJob"].Text = "Job";
                itm["AcctGL"].Text = "GL Account No";
                itm["ItemDesc"].Text = "Inv ID";  
               
            }
            RadGrid_AddPO.MasterTableView.GetColumn("Ticket").Visible = false;
            RadGrid_AddPO.MasterTableView.GetColumn("Code").Visible = false;             
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
    protected void Page_PreRender(Object o, EventArgs e)
    {
        //foreach (GridViewRow gr in gvGLItems.Rows)
        foreach (GridDataItem gr in RadGrid_AddPO.Items)
        {
            TextBox txtGvAcctNo = (TextBox)gr.FindControl("txtGvAcctNo");

            //gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtGvAcctNo.ClientID + "','" + gvGLItems.ClientID + "',event);";
            gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtGvAcctNo.ClientID + "','" + RadGrid_AddPO.ClientID + "',event);";
        }

       


        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedRowStyle('" + gvJournal.ClientID + "');", true);
    }
    private void ShowMesg(string mesg, Int16 type)
    {
        if (type == 0)            /// Warning message
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keywarning", "noty({text: '" + mesg + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        else if (type == 1)       /// Success message
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysucc", "noty({text: '" + mesg + "',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private bool Validate(DataTable dt)
    {
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["JobName"].ToString() != "")
            {
                if (dr["Phase"].ToString() == "")
                {

                    ShowMesg("Please enter a code for the Project." + dr["JobName"].ToString(), 0);
                    return false;
                }

                if (dr["Phase"].ToString() == "Inventory")
                {
                    if (string.IsNullOrEmpty(dr["warehouseID"].ToString().Trim()))
                    {
                        ShowMesg("Please enter a warehouse for the Items." + dr["ItemDesc"].ToString(), 0);
                        return false;
                    }
                }

            }

            if ((dr["fDesc"].ToString() == ""))
            {

                ShowMesg("Please enter Item description", 0);
                return false;
            }

            ///////// Check Job Closed in bill ///////
            if (!string.IsNullOrEmpty(dr["JobID"].ToString()))
            {
                if (dr["JobID"].ToString() != "0")
                {
                    DataSet _dsJobs = new DataSet();
                    objJob.ConnConfig = Session["config"].ToString();
                    objJob.ID = Convert.ToInt32(dr["JobID"].ToString());
                    _dsJobs = objBL_Job.spGetJobStatus(objJob);

                    int jobstatus = Convert.ToInt32(_dsJobs.Tables[0].Rows[0]["STATUS"].ToString());
                    if (jobstatus == 1)
                    {                        
                        ShowMesg("Project# " + dr["JobID"].ToString() + " is closed.Please change the project status before proceeding.", 0);
                        return false;
                    }
                }
            }
        }


        //DataRow[] drDesc = dt.Select("fDesc = ''");
        //if (drDesc.Count() > 0)
        //{
        //    ShowMesg("Please enter Item description", 0);
        //    return false;
        //}

        return true;
    }
    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //Boolean chkValidation = false;

    //        //#region Validation
    //        //foreach (GridDataItem gr in RadGrid_AddPO.Items)
    //        //{
    //        //    HiddenField hdnJobID = (HiddenField)gr.FindControl("hdnJobID");
    //        //    TextBox txtGvJob = (TextBox)gr.FindControl("txtGvJob");

    //        //    HiddenField hdnTypeId = (HiddenField)gr.FindControl("hdnTypeId");
    //        //    TextBox txtGvPhase = (TextBox)gr.FindControl("txtGvPhase");

    //        //    if (hdnJobID.Value != "" || hdnTypeId.Value != "")
    //        //    {
    //        //        HiddenField hdnItemID = (HiddenField)gr.FindControl("hdnItemID");
    //        //        TextBox txtGvItem = (TextBox)gr.FindControl("txtGvItem");
    //        //        if (hdnItemID.Value == "" || hdnItemID.Value == "0")
    //        //        {
    //        //            chkValidation = true;
    //        //            break;
    //        //        }
    //        //    }
    //        //}
    //        //#endregion

    //        //if (chkValidation == true)
    //        //{
    //        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'You must have to select item on the purchase order.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //        //}
    //        //else
    //        //{
    //        bool _flag = false;
    //        GetPeriodDetails(Convert.ToDateTime(txtDate.Text));
    //        _flag = (bool)ViewState["FlagPeriodClose"];

    //        if (_flag)
    //        {
    //            DataTable dtPO = GetPOItem();

    //            if (dtPO.Rows.Count > 0)
    //            {
    //                if (Validate(dtPO))
    //                {
    //                    if (dtPO.Rows.Count > 0)
    //                    {
    //                        dtPO.Columns.Remove("RowID");
    //                        dtPO.Columns.Remove("AcctNo");
    //                        dtPO.Columns.Remove("Loc");
    //                        dtPO.Columns.Remove("JobName");
    //                        dtPO.Columns.Remove("Phase");
    //                        dtPO.Columns.Remove("Warehousefdesc");
    //                        dtPO.Columns.Remove("Locationfdesc");
    //                        //dtPO.Select("JobID = 0")
    //                        //  .AsEnumerable().ToList()
    //                        //  .ForEach(t => t["JobID"] = DBNull.Value);
    //                        //dtPO.Select("PhaseID = 0")
    //                        //     .AsEnumerable().ToList()
    //                        //     .ForEach(t => t["PhaseID"] = DBNull.Value);
    //                        dtPO.Select("Inv = 0")
    //                            .AsEnumerable().ToList()
    //                            .ForEach(t => t["Inv"] = DBNull.Value);

    //                        //dtPO.Select("ItemDesc is null ")
    //                        //    .AsEnumerable().ToList()
    //                        //    .ForEach(t => t["ItemDesc"] = t["fDesc"]);

    //                        //foreach (DataRow dr in dtPO.Rows)
    //                        //{
    //                        //    string _JobID = Convert.ToString(dr["JobID"]);
    //                        //    string _PhaseID = Convert.ToString(dr["PhaseID"]);
    //                        //    string _Inv = Convert.ToString(dr["Inv"]);
    //                        //    if (_Inv == "" && _JobID != "" && _JobID != "0" && _PhaseID != "")
    //                        //    {
    //                        //        string _ItemDesc = Convert.ToString(dr["ItemDesc"]);
    //                        //        var query = dtPO.Rows.Cast<DataRow>().Where(x => x.Field<string>("ItemDesc") == _ItemDesc).FirstOrDefault();
    //                        //        if (query != null)
    //                        //        {
    //                        //            String Line = Convert.ToString(query["Line"]);
    //                        //        }
    //                        //    }
    //                        //}

    //                        dtPO.AcceptChanges();

    //                        //dtPO.Columns.Remove("TypeID");
    //                    }
    //                    _objPO.PODt = dtPO;
    //                    //check the max id again before saving 


    //                    _objPO.ConnConfig = Session["config"].ToString();
    //                    _objPO.fDate = Convert.ToDateTime(txtDate.Text);
    //                    _objPO.Due = Convert.ToDateTime(txtDueDate.Text);
    //                    _objPO.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
    //                    _objPO.FOB = txtFOB.Text;

    //                    _objPO.ShipVia = txtShipVia.Text;
    //                    _objPO.ShipTo = txtAddress.Text;
    //                    _objPO.ReqBy = 0;
    //                    _objPO.fBy = txtCreatedBy.Text;
    //                    if (chkApproved.Checked.Equals(true))
    //                    {
    //                        _objPO.Approved = 1;
    //                    }
    //                    else
    //                    {
    //                        _objPO.Approved = 0;
    //                    }
    //                    _objPO.fDesc = txtDesc.Text;
    //                    _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);
    //                    _objPO.Status = 0;
    //                    _objPO.Due = Convert.ToDateTime(txtDueDate.Text);
    //                    _objPO.ShipVia = txtShipVia.Text;
    //                    _objPO.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
    //                    _objPO.FOB = txtFOB.Text;
    //                    _objPO.ShipTo = txtShipTo.Text;
    //                    //_objPO.Custom1 = txtCustom1.Text;
    //                    //_objPO.Custom2 = txtCustom2.Text;
    //                    _objPO.ApprovedBy = "";
    //                    _objPO.ReqBy = Convert.ToInt32(0);
    //                    _objPO.CourrierAcct = txtCourrierAcct.Text;
    //                    _objPO.POReasonCode = txtPOCode.Text;
    //                    _objPO.PORevision = txtPoRevision.Text;

    //                    double totalAmount = 0;
    //                    //foreach (GridViewRow gr in gvGLItems.Rows)
    //                    foreach (GridDataItem gr in RadGrid_AddPO.Items)
    //                    {
    //                        TextBox txtGvAmount = (TextBox)gr.FindControl("txtGvAmount");
    //                        if (!string.IsNullOrEmpty(txtGvAmount.Text))
    //                        {
    //                            totalAmount += Convert.ToDouble(txtGvAmount.Text);
    //                        }
    //                    }
    //                    _objPO.Amount = totalAmount;
    //                    if (hdnPOLimit.Value != "" && hdnPOLimit.Value != null && hdnPOLimit.Value != "0.00")
    //                    {
    //                        if (_objPO.Amount > Convert.ToDouble(hdnPOLimit.Value))
    //                        {
    //                            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'You only have " + hdnPOLimit.Value + " limit to create PO',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //                            return;
    //                        }
    //                    }

    //                    lblTotalAmount.Text = totalAmount.ToString("0.00");
    //                    if (Request.QueryString["id"] != null && Request.QueryString["t"] == null)
    //                    {
    //                        _objPO.POID = Convert.ToInt32(txtPO.Text);
    //                        _objBLBills.UpdatePO(_objPO);
    //                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'PO Updated Successfully! </br> <b> PO# : " + _objPO.POID + "</b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //                    }
    //                    else
    //                    {
    //                        _objPO.ConnConfig = Session["config"].ToString();
    //                        int PoId = _objBLBills.GetMaxPOId(_objPO);
    //                        if (PoId == Convert.ToInt32(txtPO.Text))
    //                        {
    //                            _objPO.POID = Convert.ToInt32(txtPO.Text);
    //                        }
    //                        else
    //                        {
    //                            _objPO.POID = PoId;
    //                        }
    //                        _objPO.Status = 0;
    //                        _objBLBills.AddPO(_objPO);

    //                        //Response.Redirect(Request.RawUrl, false);
    //                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccadd", "noty({text: 'PO Created Successfully! </br> <b> PO# : " + _objPO.POID + "</b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); ", true);
    //                        if (Request.QueryString["TicketId"] != null & Request.QueryString["comp"] != null)
    //                        {
    //                            string strTicketid = Request.QueryString["TicketId"].ToString();
    //                            string strcomp = Request.QueryString["comp"].ToString();
    //                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "NavigatetoAddPO", "RedirectTicketScreen(" + strTicketid + "," + strcomp + ");", true);
    //                        }

    //                        ResetFormControlValues(this);
    //                        SetPOForm();

    //                    }
    //                }
    //                else
    //                {
    //                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'You must have at least one item on the purchase order.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //                }
    //            }
    //            else
    //            {
    //                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr11", "noty({text: 'You must have at least one item on the purchase order.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //            }
    //        }
    //        //   }

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        submit();
    }
    private void submit(bool allowRedirect = true)
    {
        try
        {
            
            //Response.Write("Submit Start: "+DateTime.Now.ToString());
            ExceptionLogging.SendMsgToText("Submit Start: ");
            bool checktqty = false;
            bool _flag = false;
            GetPeriodDetails(Convert.ToDateTime(txtDate.Text));
            _flag = (bool)ViewState["FlagPeriodClose"];

            //Approval Status from drop down
            //_objPO.ApprovalStatus = Convert.ToInt16(ddlApprovalStatus.SelectedValue);

            if (_flag)
            {
                //Response.Write("GetDatafromClientside Start: " + DateTime.Now.ToString());
                ExceptionLogging.SendMsgToText("GetDatafromClientside Start: ");
                DataTable dtPO = GetPOItem();


                ExceptionLogging.SendMsgToText("GetDatafromClientside End: ");
                //Response.Write("GetDatafromClientside End: " + DateTime.Now.ToString());
                if (dtPO.Rows.Count > 0)
                {
                    if (Validate(dtPO))
                    {

                        DataRow[] result = dtPO.Select("AcctNo = ''");
                        if (result.Length == 0)
                        {
                            if (dtPO.Rows.Count > 0)
                            {
                                dtPO.Columns.Remove("RowID");
                                dtPO.Columns.Remove("AcctNo");
                                dtPO.Columns.Remove("Loc");
                                dtPO.Columns.Remove("JobName");
                                //dtPO.Columns.Remove("Phase");
                                dtPO.Columns.Remove("Warehousefdesc");
                                dtPO.Columns.Remove("Locationfdesc");
                                
                                dtPO.Select("Inv = 0")
                                    .AsEnumerable().ToList()
                                    .ForEach(t => t["Inv"] = DBNull.Value);

                                dtPO.AcceptChanges();
                                ExceptionLogging.SendMsgToText("Foreach loop Start: ");
                                //Response.Write("Foreach loop Start: " + DateTime.Now.ToString());
                                foreach (DataRow dtrow in dtPO.Rows)
                                {
                                    int AcctID = Convert.ToInt32(dtrow["AcctID"]);
                                    double tquant = 0;
                                    if (dtrow["Quan"] != DBNull.Value)
                                    {
                                        tquant = Convert.ToDouble(dtrow["Quan"]);
                                    }
                                    else
                                    {
                                        tquant = 1;
                                    }
                                    double tAmount = Convert.ToDouble(dtrow["Amount"]);
                                    if (tAmount == 0 && tquant == 0)
                                    {
                                        checktqty = true;

                                    }

                                    string Phase = Convert.ToString(dtrow["Phase"]);
                                    if (Phase == "Inventory")
                                    {
                                        int Itemcode = 0;
                                        if (Convert.ToString(dtrow["Inv"]) != "")
                                        {
                                            Itemcode = Convert.ToInt32(dtrow["Inv"]);
                                        }
                                        string sWarehouseID = "";
                                        if (Convert.ToString(dtrow["WarehouseID"]) != "")
                                        {
                                            sWarehouseID = Convert.ToString(dtrow["WarehouseID"]);
                                        }
                                        if (dtrow["JobID"] != DBNull.Value)
                                        {
                                            if (Convert.ToString(dtrow["JobID"]) != "" && Convert.ToString(dtrow["JobID"]) != "0")
                                            {                                                
                                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Inventory not allowed with        Project.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',             closable : true});", true);
                                                return;
                                            }
                                            
                                        }
                                        if (Itemcode == 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Please enter item.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});   $('#MOMloading').hide();", true);
                                            return;
                                        }
                                        if (sWarehouseID.Trim() == "")
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Please enter warehouse.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});  $('#MOMloading').hide();", true);
                                            return;
                                        }
                                    }

                                    if (Convert.ToString(dtrow["Inv"]) != "")
                                    {
                                        int Inv = Convert.ToInt32(dtrow["Inv"]);
                                        //int TypeID = Convert.ToInt32(dtrow["TypeID"]);
                                        string sName = Convert.ToString(dtrow["fDesc"]);


                                        ///////// ES-3793 Check Active/Inactive Item ///////
                                        if (Inv > 0 && Phase == "Inventory")
                                        {

                                            //DataSet _dsInv = new DataSet();
                                            //_objInv.ConnConfig = Session["config"].ToString();
                                            //_objInv.ID = Inv;
                                            //_objInv.UserID = Convert.ToInt32(Session["UserID"].ToString());
                                            //Response.Write("Check Active/Inactive Item Start: " + DateTime.Now.ToString());
                                            //ExceptionLogging.SendMsgToText("Check Active/Inactive Item Start: ");
                                            //_dsInv = _objBLBills.GetInventoryItemStatus(_objInv);

                                            //Response.Write("Check Active/Inactive Item End: " + DateTime.Now.ToString());
                                            //ExceptionLogging.SendMsgToText("Check Active/Inactive Item End: ");
                                            //int acctstatus = Convert.ToInt32(_dsInv.Tables[0].Rows[0]["Status"].ToString());
                                            //if (acctstatus == 1)
                                            //{
                                            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Item " + sName + " is Inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});  $('#MOMloading').hide();", true);
                                            //    return;

                                            //}
                                        }
                                        if (Phase == "Inventory")
                                        {
                                            if (AcctID != Convert.ToInt32(hdnInvDefaultAcctID.Value))
                                            {
                                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'Please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});  $('#MOMloading').hide();", true);
                                                return;
                                            }
                                        }
                                    }
                                    
                                }
                                //Response.Write("Foreach loop End: " + DateTime.Now.ToString());
                                ExceptionLogging.SendMsgToText("Foreach loop End: ");


                                DataTable _dtInvStatus = new DataTable();

                                string strInactiveProduct = "";

                                if (_InvIds.Length != 0)
                                {
                                    _dtInvStatus = _objBLBills.GetInventoryItemStatusbyIds(Session["config"].ToString(), _InvIds.ToString());

                                    if (_dtInvStatus.Rows.Count > 0)
                                    {
                                        foreach (DataRow inactiveItem in _dtInvStatus.Rows)
                                        {
                                            if (strInactiveProduct == "")
                                            {
                                                strInactiveProduct = inactiveItem["Name"].ToString();
                                            }
                                            else
                                            {
                                                strInactiveProduct += ", " + inactiveItem["Name"].ToString();
                                            }
                                        }
                                    }
                                }

                                if (strInactiveProduct != "")
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Item(s) " + strInactiveProduct + " is Inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});  $('#MOMloading').hide();", true);
                                    return;
                                }

                                DataTable _dtChartStatus = new DataTable();

                                string strInactiveChart = "";

                                if (_ChartIds.Length != 0)
                                {
                                    _dtChartStatus = _objBLBills.GetChartStatusbyIds(Session["config"].ToString(), _ChartIds.ToString());

                                    if (_dtChartStatus.Rows.Count > 0)
                                    {
                                        foreach (DataRow inactiveChart in _dtChartStatus.Rows)
                                        {
                                            if (strInactiveChart == "")
                                            {
                                                strInactiveChart = inactiveChart["fDesc"].ToString();
                                            }
                                            else
                                            {
                                                strInactiveChart += ", " + inactiveChart["fDesc"].ToString();
                                            }
                                        }
                                    }
                                }

                                if (strInactiveChart != "")
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Account(s) " + strInactiveChart + "   is inactive. Please change the account name before proceeding.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});  $('#MOMloading').hide();", true);
                                    return;
                                }

                                dtPO.Columns.Remove("Phase");
                                dtPO.AcceptChanges();
                                //dtPO.Columns.Remove("TypeID");
                            }
                            _objPO.PODt = dtPO;
                            //check the max id again before saving 

                            //Approval Status from drop down
                            _objPO.ApprovalStatus = Convert.ToInt16(ddlApprovalStatus.SelectedValue);

                            _objPO.ConnConfig = Session["config"].ToString();
                            _objPO.fDate = Convert.ToDateTime(txtDate.Text);
                            _objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                            _objPO.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
                            _objPO.FOB = txtFOB.Text;

                            _objPO.ShipVia = txtShipVia.Text;
                            _objPO.ShipTo = txtAddress.Text;
                            _objPO.ReqBy = 0;
                            _objPO.fBy = txtCreatedBy.Text;
                            _objPO.RequestedBy = txtRequestedBy.Text;
                            if (chkApproved.Checked.Equals(true))
                            {
                                _objPO.Approved = 1;
                            }
                            else
                            {
                                _objPO.Approved = 0;
                            }
                            _objPO.IsPOClose = chkPOClose.Checked;
                            _objPO.IsAddReceivePO = chkAddRPO.Checked;                            
                            _objPO.fDesc = txtDesc.Text;
                            _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);
                            //_objPO.Status = 0;
                            _objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                            _objPO.ShipVia = txtShipVia.Text;
                            _objPO.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
                            _objPO.FOB = txtFOB.Text;
                            _objPO.ShipTo = txtShipTo.Text;
                            _objPO.Custom1 = txtPO1.Text;
                            _objPO.Custom2 = txtPO2.Text;
                            _objPO.ApprovedBy = "";
                            _objPO.ReqBy = Convert.ToInt32(0);
                            _objPO.CourrierAcct = txtCourrierAcct.Text;
                            _objPO.SalesOrderNo = txtSalesOrderNo.Text;
                            _objPO.POReasonCode = txtPOCode.Text;
                            _objPO.PORevision = txtPoRevision.Text;
                            _objPO.MOMUSer = Session["User"].ToString();
                            _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
                            double totalAmount = 0;
                            //foreach (GridViewRow gr in gvGLItems.Rows)
                            foreach (GridDataItem gr in RadGrid_AddPO.Items)
                            {
                                TextBox txtGvAmount = (TextBox)gr.FindControl("txtGvAmount");
                                if (!string.IsNullOrEmpty(txtGvAmount.Text))
                                {
                                    totalAmount += Convert.ToDouble(txtGvAmount.Text);
                                }
                            }
                            _objPO.Amount = totalAmount;
                            if (hdnPOLimit.Value != "" && hdnPOLimit.Value != null && hdnPOLimit.Value != "0.00")
                            {
                                if (_objPO.Amount > Convert.ToDouble(hdnPOLimit.Value))
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'You only have $" + hdnPOLimit.Value + " limit to create PO',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});   $('#MOMloading').hide();", true);
                                    return;
                                }
                            }

                            lblTotalAmount.Text = totalAmount.ToString("0.00");
                            if (Request.QueryString["id"] != null && Request.QueryString["t"] == null)
                            {
                                _objPO.POID = Convert.ToInt32(txtPO.Text);

                                //var strPOStatus = txtStatus.Text;
                                DataTable dtPOfromDB = GetPOItemsByPOID(_objPO.POID);

                                _objPO.Status = Convert.ToInt16(hdnStatus.Value);

                                // Update Selected and SelectedQuan fields for updating POItems
                                var _dtPO = _objPO.PODt;
                                StringBuilder errorMessage = new StringBuilder();
                                if (_objPO.PODt.Rows.Count > 0)
                                {
                                    foreach (DataRow item in dtPOfromDB.Rows)
                                    {
                                        var _POline = _objPO.PODt.Select("ID = " + item["PO"].ToString() + " and Line=" + item["Line"]).FirstOrDefault();

                                        // Check line if receive or not
                                        // Case received: item["SelectedQuan"] != null && item["SelectedQuan"].ToString() != 0
                                        if (item["SelectedQuan"] != null
                                            && !string.IsNullOrEmpty(item["SelectedQuan"].ToString())
                                            && Convert.ToDouble(item["SelectedQuan"].ToString()) != 0)
                                        {
                                            if (_POline == null)
                                            {
                                                errorMessage.Append("Error deleting: PO Line " + item["Line"].ToString() + " was received.");
                                            }
                                            else
                                            {
                                                // case update quantity
                                                if (!string.IsNullOrEmpty(item["SelectedQuan"].ToString()) && Convert.ToDouble(item["SelectedQuan"].ToString()) > Convert.ToDouble(_POline["Quan"]))
                                                {
                                                    //errorMessage.AppendFormat("Error updating: PO Line {0} received {1} item(s).", item["Line"].ToString(), item["SelectedQuan"].ToString());
                                                    errorMessage.AppendFormat("Error updating: The quantity/amount cannot be less than the items already received.");
                                                }
                                                // case update price
                                                else if (!string.IsNullOrEmpty(item["Price"].ToString()) && Convert.ToDouble(item["Price"].ToString()) != Convert.ToDouble(_POline["Price"]))
                                                {
                                                    errorMessage.AppendFormat("Error updating: PO Line {0} was received. Cannot update price in this case.", item["Line"].ToString());
                                                }
                                                else
                                                {
                                                    _POline["Selected"] = item["Selected"];
                                                    _POline["SelectedQuan"] = item["SelectedQuan"];
                                                }
                                            }
                                        }
                                        else if (item["Selected"] != null
                                            && !string.IsNullOrEmpty(item["Selected"].ToString())
                                            && Convert.ToDouble(item["Selected"].ToString()) != 0)
                                        {
                                            if (_POline == null)
                                            {
                                                errorMessage.Append("Error deleting: PO Line " + item["Line"].ToString() + " was received.");
                                            }
                                            else
                                            {
                                                // case update receive
                                                if (!string.IsNullOrEmpty(item["Selected"].ToString()) && Convert.ToDouble(item["Selected"].ToString()) > Convert.ToDouble(_POline["Amount"]))
                                                {
                                                    errorMessage.AppendFormat("Error updating: PO Line {0} received {1}.", item["Line"].ToString(), item["Selected"].ToString());
                                                }
                                                // case update price
                                                else if (!string.IsNullOrEmpty(item["Price"].ToString()) && Convert.ToDouble(item["Price"].ToString()) != Convert.ToDouble(_POline["Price"]))
                                                {
                                                    errorMessage.AppendFormat("Error updating: PO Line {0} was received. Cannot update price in this case.", item["Line"].ToString());
                                                }
                                                else
                                                {
                                                    _POline["Selected"] = item["Selected"];
                                                    _POline["SelectedQuan"] = item["SelectedQuan"];
                                                }
                                            }
                                        }
                                    }

                                }

                                if (errorMessage.Length == 0)
                                {
                                    _objBLBills.UpdatePO(_objPO);
                                    #region Update PO status
                                    //_objBLBills.AutoUpdatePOStatus(_objPO);
                                    #endregion
                                    Session["POSuccMess"] = "PO Updated Successfully! </br> <b> PO# : " + _objPO.POID + "</b>";
                                    Session["POErrMess"] = null;

                                    if (checktqty == true)
                                    {
                                        Session["POSuccMessWarn"] = "PO will be created with zero quantity.";
                                    }
                                    else
                                    {
                                        Session["POSuccMessWarn"] = null;
                                    }

                                }
                                else
                                {
                                    Session["POErrMess"] = errorMessage.ToString();
                                    Session["POSuccMess"] = null;
                                    Session["POSuccMessWarn"] = null;                                    
                                }
                                UpdateDocInfo();
                                if (chkAddRPO.Checked == true)
                                {
                                    Session["POErrMess"] = null;
                                    Session["POSuccMess"] = null;
                                    Session["POSuccMessWarn"] = null;                                    
                                    Response.Redirect("AddReceivePO.aspx?PO=" + _objPO.POID);                                 
                                }
                                else
                                {
                                    if (allowRedirect) Response.Redirect("addpo.aspx?id=" + _objPO.POID);
                                }
                            }
                            else
                            {                                
                                //Response.Write("Check New ID Start: " + DateTime.Now.ToString());
                                ExceptionLogging.SendMsgToText("New POID Start: ");
                                int PoId = GetNewPOId();
                                //Response.Write("Check New ID End: " + DateTime.Now.ToString());
                                ExceptionLogging.SendMsgToText("New POID End: ");
                                _objPO.POID = PoId;
                                _objPO.Status = 0;

                                ExceptionLogging.SendMsgToText("Add PO Sp Start: ");
                                _objBLBills.AddPO(_objPO);
                                ExceptionLogging.SendMsgToText("Add PO Sp End: ");

                                //Response.Redirect(Request.RawUrl, false);
                                reportPOID = Convert.ToString(_objPO.POID);
                                Session["POErrMess"] = null;
                                Session["POSuccMess"] = "PO Created Successfully! </br> <b> PO# : " + _objPO.POID + "</b>";


                                if (checktqty == true)
                                {
                                    Session["POSuccMessWarn"] = "PO will be created with zero quantity.";
                                }
                                else
                                {
                                    Session["POSuccMessWarn"] = null;
                                }

                                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccadd", "noty({text: 'PO Created Successfully! </br> <b> PO# : " + _objPO.POID + "</b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); ", true);
                                if (Request.QueryString["TicketId"] != null & Request.QueryString["comp"] != null)
                                {
                                    allowRedirect = false;
                                    UpdateTempDateWhenCreatingNewPO(PoId);
                                    UpdateDocInfo();

                                    ResetFormControlValues(this);
                                    SetPOForm();

                                    string strTicketid = Request.QueryString["TicketId"].ToString();
                                    string strcomp = Request.QueryString["comp"].ToString();
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "NavigatetoAddPO", "RedirectTicketScreen(" + strTicketid + "," + strcomp + ");", true);

                                    if (Request.QueryString["fr"] != null && !string.IsNullOrEmpty(Request.QueryString["fr"]))
                                        Response.Redirect("addticket.aspx?id=" + Request.QueryString["TicketId"].ToString() + "&comp=" + Request.QueryString["comp"].ToString() + "&pop=1&fr=" + Request.QueryString["fr"]);
                                    else
                                        Response.Redirect("addticket.aspx?id=" + Request.QueryString["TicketId"].ToString() + "&comp=" + Request.QueryString["comp"].ToString() + "&pop=1");

                                }




                                //Update  Attachment Doc INFO                 
                                UpdateTempDateWhenCreatingNewPO(PoId);
                                UpdateDocInfo();

                                
                                if (chkAddRPO.Checked == true)
                                {
                                    Session["POErrMess"] = null;
                                    Session["POSuccMess"] = null;
                                    Session["POSuccMessWarn"] = null;
                                    Response.Redirect("AddReceivePO.aspx?PO=" + _objPO.POID);
                                }
                                else
                                {
                                    ResetFormControlValues(this);
                                    SetPOForm();
                                    if (allowRedirect) Response.Redirect("addpo.aspx");
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'Account no can not be blank line# "+result[0][2].ToString()+"',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr11", "noty({text: 'You must have at least one item on the purchase order.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr11", "noty({text: 'You must have at least one item on the purchase order.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            //   }
            //Response.Write("Submit End: " + DateTime.Now.ToString());
            ExceptionLogging.SendMsgToText("Submit End: ");

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["TicketId"] != null && Request.QueryString["comp"] != null)
        {
            if (Request.QueryString["fr"] != null && !string.IsNullOrEmpty(Request.QueryString["fr"]))
                Response.Redirect("addticket.aspx?id=" + Request.QueryString["TicketId"].ToString() + "&comp=" + Request.QueryString["comp"].ToString() + "&pop=1&fr=" + Request.QueryString["fr"]);
            else
                Response.Redirect("addticket.aspx?id=" + Request.QueryString["TicketId"].ToString() + "&comp=" + Request.QueryString["comp"].ToString() + "&pop=1");
        }
        else if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["pid"] != null)
            {
                Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["pid"].ToString() + "&tab=budget");
            }
        }
        else
        {
            Response.Redirect("ManagePO.aspx");
        }
    }
    protected void gvGLItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void btnAddNewLines_Click(object sender, EventArgs e)
    {
        try
        {
           
                DataTable dt = GetPOGridItems();
            
            Int32 Line = 0;//Default if dt is null
            if (dt != null && dt.Rows.Count > 0)
            {
                Line = Convert.ToInt32(Convert.ToString(dt.Rows[dt.Rows.Count - 1]["Line"]) == "" ? "0" : Convert.ToString(dt.Rows[dt.Rows.Count - 1]["Line"]));
            }

            DataRow dr = dt.NewRow();
            dr["RowID"] = Line + 1;
            dr["Line"] = Line + 1;
            if (!string.IsNullOrEmpty(txtProject.Text))
            {
                dr["JobName"] = txtProject.Text;
                dr["JobID"] = hdnmainjobID.Value;
                dr["Loc"] = hdMainGvLoc.Value;
                dr["AcctID"] = hdMainAcctID.Value;
                dr["AcctNo"] = hdMainGvAcctNo.Value;
                dr["Due"] = txtDueDate.Text;
            }

            if (!string.IsNullOrEmpty(txtCode.Text) && !string.IsNullOrEmpty(hdnMainCode.Value))
            {
                dr["Phase"] = txtCode.Text;
                dr["TypeID"] = Convert.ToInt32(hdnMainCode.Value);
            }

            dt.Rows.Add(dr);

            RadGrid_AddPO.DataSource = dt;
            RadGrid_AddPO.DataBind();

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalAmt", "CalculateTotalAmt();", true);

            //Focus last row
            GridDataItem lastRow = RadGrid_AddPO.Items[RadGrid_AddPO.Items.Count - 1];
            TextBox txtGvJob = (TextBox)lastRow.FindControl("txtGvJob");
            if(txtGvJob != null)
            {
                txtGvJob.Focus();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnCopyPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            var selectIndex = 0;
           
            if (!string.IsNullOrEmpty(hdnSelectPOIndex.Value))
            {
                selectIndex = Convert.ToInt32(hdnSelectPOIndex.Value);
            }            
            else
            {
                var selectItem = RadGrid_AddPO.MasterTableView.GetSelectedItems();
                if (selectItem.Count() > 0)
                {
                    selectIndex = selectItem[0].ClientRowIndex;
                }
            }

            DataTable dt = GetPOGridItems();
            if (dt.Rows.Count > 0 && selectIndex > 0)
            {
                var copyRow = dt.Rows[selectIndex - 1];
                var dr = dt.Rows[selectIndex];

                dr["RowID"] = dt.Rows.Count;
                dr["ID"] = copyRow["ID"];
                dr["AcctID"] = copyRow["AcctID"];
                dr["AcctNo"] = copyRow["AcctNo"];
                dr["fDesc"] = copyRow["fDesc"];
                dr["Loc"] = copyRow["Loc"];
                dr["JobName"] = copyRow["JobName"];
                dr["JobID"] = copyRow["JobID"];
                dr["Phase"] = copyRow["Phase"];
                dr["PhaseID"] = copyRow["PhaseID"];
                dr["TypeID"] = copyRow["TypeID"];
                dr["Quan"] = copyRow["Quan"];
                dr["Price"] = copyRow["Price"];
                dr["Amount"] = copyRow["Amount"];
                dr["Due"] = copyRow["Due"];
                dr["Ticket"] = copyRow["Ticket"];
                dr["ItemDesc"] = copyRow["ItemDesc"];
                dr["Inv"] = copyRow["Inv"];
                dr["OpSq"] = copyRow["OpSq"];
                dr["Warehousefdesc"] = copyRow["Warehousefdesc"];
                dr["WarehouseID"] = copyRow["WarehouseID"];
                dr["Locationfdesc"] = copyRow["Locationfdesc"];
                dr["LocationID"] = copyRow["LocationID"];

                dt.AcceptChanges();

                RadGrid_AddPO.DataSource = dt;
                RadGrid_AddPO.VirtualItemCount = dt.Rows.Count;
                RadGrid_AddPO.DataBind();

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalAmt", "CalculateTotalAmt();", true);

                //Focus row
                GridDataItem focusRow = RadGrid_AddPO.Items[selectIndex];
                TextBox txtGvJob = (TextBox)focusRow.FindControl("txtGvJob");
                if (txtGvJob != null)
                {
                    txtGvJob.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnSelectVendor_Click(object sender, EventArgs e)
    {
        FillAddress();
        //txtVendorType.Enabled = false;
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["PO"];
            Response.Redirect("addpo.aspx?id=" + dt.Rows[0]["PO"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["PO"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["PO"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("addpo.aspx?id=" + dt.Rows[index - 1]["PO"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["PO"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["PO"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;

            if (index < c)
            {
                Response.Redirect("addpo.aspx?id=" + dt.Rows[index + 1]["PO"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["PO"];
            Response.Redirect("addpo.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["PO"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnDownloadPOTemplate_Click(object sender, CommandEventArgs e)
    {
        try
        {
            var argument = e.CommandArgument.ToString();
            if (!string.IsNullOrEmpty(argument))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]) && Request.QueryString["t"] == null)
                {
                    Response.Redirect(string.Format("PrintCustomPOReport.aspx?id={0}&report={1}", Request.QueryString["id"].ToString(), HttpUtility.UrlEncode(argument)), true);
                }
                else
                {
                    submit(false);
                    if (!string.IsNullOrEmpty(reportPOID))
                    {
                        Response.Redirect(string.Format("PrintCustomPOReport.aspx?id={0}&report={1}", reportPOID, HttpUtility.UrlEncode(argument)), true);
                    }
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
    #endregion

    #region Custom Functions
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
    private void FillAddress(bool IsVenderAutoFill = true)
    {
        if (!string.IsNullOrEmpty(hdnVendorID.Value))
        {

            if (hdnVendorID.Value != "0")
                lnkVenderID.NavigateUrl = "AddVendor.aspx?id=" + hdnVendorID.Value;
            else
                lnkVenderID.NavigateUrl = "";

            _objVendor.ConnConfig = Session["config"].ToString();
            _objVendor.ID = Convert.ToInt32(hdnVendorID.Value);
            DataSet ds = new DataSet();

            if (IsVenderAutoFill)
            {
                ds = _objBLVendor.GetVendorRolDetails(_objVendor);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["city"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString() + ", " + ds.Tables[0].Rows[0]["Zip"].ToString();
                    txtShipVia.Text = ds.Tables[0].Rows[0]["ShipVia"].ToString();
                    txtCourrierAcct.Text = ds.Tables[0].Rows[0]["CourierAccount"].ToString();
                }
                else
                {
                    txtAddress.Text = "";
                }
            }
            DataSet Vendords = new DataSet();
            Vendords = _objBLVendor.GetVendorEdit(_objVendor);

            if (Vendords.Tables[0].Rows.Count > 0)
            {

                DataRow dr = Vendords.Tables[0].Rows[0];
                txtVenderContactName.Text = dr["Contact"].ToString();
                txtVenderCellular.Text = dr["Cellular"].ToString();
                txtVenderEmailid.Text = dr["EMail"].ToString();
                txtVenderWebsite.Text = dr["Website"].ToString();
                txttVenderPhone.Text = dr["Phone"].ToString();
                txtVenderFax.Text = dr["Fax"].ToString();
                try
                {
                    txtCourrierAcct.Text = Vendords.Tables[0].Rows[0]["CourierAccount"].ToString();
                }
                catch { txtCourrierAcct.Text = ""; }
            }
            else
            {

                txtVenderContactName.Text = txtVenderCellular.Text = txtVenderEmailid.Text = txtVenderWebsite.Text = txttVenderPhone.Text = txtVenderFax.Text =  txtCourrierAcct.Text= "";
            }


        }
        else
        {

            txtVenderContactName.Text = txtVenderCellular.Text = txtVenderEmailid.Text = txtVenderWebsite.Text = txttVenderPhone.Text = txtVenderFax.Text = "";
            lnkVenderID.NavigateUrl = "";
        }
    }
    private void FillUserAddress()
    {
        #region Company Check

        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            _objPO.EN = 1;
            if (!string.IsNullOrEmpty(hdnVendorID.Value))
            {
                var vendorId = Convert.ToInt32(hdnVendorID.Value);
                _objVendor.ID = vendorId;
                var dsAddress = _objBLVendor.GetVendorBasedPORemitAddress(_objVendor);
                if (dsAddress != null)
                {
                    txtShipTo.Text = dsAddress.Tables[0].Rows[0]["PORemit"].ToString();
                }
            }
            else
            {
                DataSet dsC = new DataSet();
                _objPropUser.ConnConfig = Session["config"].ToString();
                dsC = _objBLUser.getControl(_objPropUser);

                string address;
                address = dsC.Tables[0].Rows[0]["Name"].ToString() + ", " + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["Address"].ToString() + ", " + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
                txtShipTo.Text = address;
            }
        }
        else
        {
            DataSet dsC = new DataSet();
            _objPropUser.ConnConfig = Session["config"].ToString();
            dsC = _objBLUser.getControl(_objPropUser);

            string address;
            address = dsC.Tables[0].Rows[0]["Name"].ToString() + ", " + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + ", " + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            txtShipTo.Text = address;
        }
        #endregion

    }
    private void GetPeriodDetails(DateTime _transDate)
    {
        bool _flag = CommonHelper.GetPeriodDetails(_transDate);
        ViewState["FlagPeriodClose"] = _flag;
        if (!_flag)
        {
            divSuccess.Visible = true;
        }
    }
    private void userpermissions()
    {
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

            string POPermission = ds.Rows[0]["PO"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["PO"].ToString();
            string ADD = POPermission.Length < 1 ? "Y" : POPermission.Substring(0, 1);
            string Edit = POPermission.Length < 2 ? "Y" : POPermission.Substring(1, 1);
            string Delete = POPermission.Length < 2 ? "Y" : POPermission.Substring(2, 1);
            string View = POPermission.Length < 4 ? "Y" : POPermission.Substring(3, 1);
            string PONotification = ds.Rows[0]["PONotification"] == DBNull.Value ? "N" : ds.Rows[0]["PONotification"].ToString();
            ViewState["PONotification"] = PONotification;

            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            else if (Request.QueryString["id"] == null)
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
                    btnSubmit.Visible = false;
                }
                else
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
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
        ds = _objBLUser.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }
    private void SetInitialRow()    //Initialization of Datatable.
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("RowID", typeof(string)));
            dt.Columns.Add(new DataColumn("ID", typeof(Int32)));        // PO
            dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
            dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));    // GL
            dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
            dt.Columns.Add(new DataColumn("fDesc", typeof(string)));  // fDesc
            dt.Columns.Add(new DataColumn("Quan", typeof(string)));
            dt.Columns.Add(new DataColumn("Price", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("Loc", typeof(string)));
            dt.Columns.Add(new DataColumn("JobName", typeof(string)));
            dt.Columns.Add(new DataColumn("JobID", typeof(Int32)));    // Job
            dt.Columns.Add(new DataColumn("Phase", typeof(string)));
            dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));   // Phase
            dt.Columns.Add(new DataColumn("Inv", typeof(Int32)));       // Inv
            dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));    // Billed
            dt.Columns.Add(new DataColumn("Ticket", typeof(Int32)));    // Ticket
            dt.Columns.Add(new DataColumn("Due", typeof(DateTime)));      //due date
            dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            dt.Columns.Add(new DataColumn("WarehouseID", typeof(string)));
            dt.Columns.Add(new DataColumn("LocationID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Warehousefdesc", typeof(string)));
            dt.Columns.Add(new DataColumn("Locationfdesc", typeof(string)));
            dt.Columns.Add(new DataColumn("OpSq", typeof(string)));
            dt.Columns.Add(new DataColumn("Selected", typeof(string)));
            dt.Columns.Add(new DataColumn("SelectedQuan", typeof(string)));
            dt.Columns.Add(new DataColumn("ForceClose", typeof(Int32)));

            dr = dt.NewRow();
            dr["RowID"] = 1;
            dr["Line"] = 1;
            dr["ForceClose"] = 0;
            //dr["ID"] = 0;
            //dr["AcctID"] = 0;
            //dr["AcctNo"] = string.Empty;
            //dr["fDesc"] = string.Empty;
            //dr["Loc"] = string.Empty;
            //dr["JobName"] = string.Empty;
            //dr["JobID"] = DBNull.Value;
            //dr["Phase"] = string.Empty;
            //dr["PhaseID"] = DBNull.Value;
            //dr["TypeID"] = DBNull.Value;
            //dr["Quan"] = string.Empty;
            //dr["Price"] = string.Empty;
            //dr["Amount"] = string.Empty;
            //dr["Freight"] = 0.00;
            //dr["Rquan"] = 0.00;
            //dr["Due"] = DBNull.Value;
            //dr["ItemDesc"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["Transactions"] = dt;

            //gvGLItems.DataSource = dt;
            //gvGLItems.DataBind();
            RadGrid_AddPO.DataSource = dt;
            RadGrid_AddPO.DataBind();
            MitsuChanges();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void SetPOForm()
    {
        try
        {
            _objPO.ConnConfig = Session["config"].ToString();
            txtDesc.Text = "Purchase order";
            bool firstPo = _objBLBills.IsFirstPo(_objPO);
            if (firstPo.Equals(false))
            {
                txtPO.Enabled = true;
            }

            int PoId = GetNewPOId();//_objBLBills.GetMaxPOId(_objPO);
            txtPO.Text = PoId.ToString();

            if (Request.QueryString["t"] == null)
            {
                SetInitialRow();
                FillUserAddress();
            }


            lblTotalAmount.Text = "0.00";
            lblTotalOpenAmount.Text = "0.00";
            txtQty.Text = "0.00";
            txtBudgetUnit.Text = "0.00";
            lblBudgetExt.Text = "0.00";

            RadGrid_gvLogs.Rebind();

            //ViewState["AttachedFiles"] = null;
            RadGrid_Documents.Rebind();

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "GetClientUTC();", true);
            txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            txtDueDate.Text = DateTime.Now.AddDays(30).ToString("MM/dd/yyyy");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillTerms()
    {
        try
        {
            DataSet ds = new DataSet();
            _objPropUser.ConnConfig = Session["config"].ToString();

            ds = _objBLUser.getTerms(_objPropUser);

            ddlTerms.DataSource = ds.Tables[0];
            ddlTerms.DataTextField = "name";
            ddlTerms.DataValueField = "id";
            ddlTerms.DataBind();

            ddlTerms.Items.Insert(0, new ListItem("Select", ""));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataTable GetPOGridItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));        // PO
        dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));    // GL
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));  // fDesc
        dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        dt.Columns.Add(new DataColumn("Price", typeof(string)));
        dt.Columns.Add(new DataColumn("Amount", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(string)));    // Job
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));   // Phase
        dt.Columns.Add(new DataColumn("Inv", typeof(Int32)));       // Inv
        dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));    // Billed
        dt.Columns.Add(new DataColumn("Ticket", typeof(Int32)));    // Ticket
        dt.Columns.Add(new DataColumn("Due", typeof(DateTime)));      //due date
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("WarehouseID", typeof(string)));
        dt.Columns.Add(new DataColumn("LocationID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Warehousefdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Locationfdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("OpSq", typeof(String)));
        dt.Columns.Add(new DataColumn("Selected", typeof(string)));
        dt.Columns.Add(new DataColumn("SelectedQuan", typeof(string)));        
        dt.Columns.Add(new DataColumn("ForceClose", typeof(Int32)));

        try
        {
            string strItems = hdnItemJSON.Value.Trim();
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);

                int index = 1;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["hdnIndex"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    DataRow dr = dt.NewRow();
                    dr["RowID"] = index;

                    if (!(dict["hdnTID"].ToString().Trim() == string.Empty))
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnTID"].ToString());
                    }
                    if (!(dict["hdnLine"].ToString().Trim() == string.Empty))
                    {
                        dr["Line"] = Convert.ToInt16(dict["hdnLine"].ToString());
                    }
                    if (!(dict["hdnAcctID"].ToString().Trim() == string.Empty))
                    {
                        dr["AcctID"] = Convert.ToInt32(dict["hdnAcctID"].ToString().Trim());
                    }
                    dr["AcctNo"] = dict["txtGvAcctNo"].ToString().Trim();
                    dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();
                    dr["Quan"] = dict["txtGvQuan"].ToString();
                    dr["Price"] = dict["txtGvPrice"].ToString().Trim();
                    dr["Amount"] = dict["txtGvAmount"].ToString().Trim();
                    if (!(dict["txtGvLoc"].ToString().Trim() == string.Empty))
                    {
                        dr["Loc"] = dict["txtGvLoc"].ToString().Trim();
                    }
                    if (!(dict["txtGvJob"].ToString().Trim() == string.Empty))
                    {
                        dr["JobName"] = dict["txtGvJob"].ToString().Trim();
                    }
                                     

                    if (!(dict["hdnJobID"].ToString().Trim() == string.Empty))
                    {
                        dr["JobID"] = Convert.ToInt32(dict["hdnJobID"]);
                    }


                    if (dict.ContainsKey("hdnTypeId"))
                    {
                        if (!(dict["hdnTypeId"].ToString().Trim() == string.Empty))
                        {
                            dr["TypeID"] = Convert.ToInt32(dict["hdnTypeId"].ToString().Trim());
                        }
                    }
                    //dr["Freight"] = 0.00;
                    if (!(dict["txtGvDue"].ToString().Trim() == string.Empty))
                    {
                        dr["Due"] = Convert.ToDateTime(dict["txtGvDue"].ToString());
                    }
                    else
                    {
                        dr["Due"] = DBNull.Value;
                    }
                    if (!(dict["hdnItemID"].ToString().Trim() == string.Empty))
                    {
                        dr["Inv"] = Convert.ToInt32(dict["hdnItemID"]);
                    }
                    if (!(dict["txtGvItem"].ToString().Trim() == string.Empty))
                    {
                        dr["ItemDesc"] = dict["txtGvItem"].ToString();
                    }

                    dr["WarehouseID"] = DBNull.Value;

                    if (dict.ContainsKey("txtGvTicket"))
                    {
                        if (!(dict["txtGvTicket"].ToString().Trim() == string.Empty))
                        {
                            dr["Ticket"] = dict["txtGvTicket"].ToString().Trim();
                        }
                    }

                    if (dict.ContainsKey("hdnPID"))
                    {
                        if (!(dict["hdnPID"].ToString().Trim() == string.Empty))
                        {
                            dr["PhaseID"] = Convert.ToInt32(dict["hdnPID"].ToString());
                        }
                    }

                    if (dict.ContainsKey("txtGvPhase"))
                    {
                        if (!(dict["txtGvPhase"].ToString().Trim() == string.Empty))
                        {
                            dr["Phase"] = dict["txtGvPhase"].ToString().Trim();
                        }
                    }

                    if (dict.ContainsKey("hdnWarehouse"))
                    {
                        if (!(dict["hdnWarehouse"].ToString().Trim() == string.Empty))
                        {
                            dr["WarehouseID"] = dict["hdnWarehouse"].ToString();
                        }
                    }

                    dr["LocationID"] = DBNull.Value;

                    if (dict.ContainsKey("hdnWarehouseLocationID"))
                    {
                        if (!(dict["hdnWarehouseLocationID"].ToString().Trim() == string.Empty))
                        {
                            dr["LocationID"] = dict["hdnWarehouseLocationID"].ToString();
                        }
                    }

                    if (dict.ContainsKey("hdnWarehousefdesc"))
                    {
                        if (!(dict["hdnWarehousefdesc"].ToString().Trim() == string.Empty))
                        {
                            dr["Warehousefdesc"] = dict["hdnWarehousefdesc"].ToString();
                        }
                    }
                    if (dict.ContainsKey("hdnLocationfdesc"))
                    {
                        if (!(dict["hdnLocationfdesc"].ToString().Trim() == string.Empty))
                        {
                            dr["Locationfdesc"] = dict["hdnLocationfdesc"].ToString();
                        }
                    }

                    if (!(dict["hdOpSq"].ToString().Trim() == string.Empty))
                    {
                        dr["OpSq"] = dict["hdOpSq"].ToString();
                    }
                    else
                    {
                        dr["OpSq"] = "999";
                    }

                    if (!(dict["hdnForceClose"].ToString().Trim() == string.Empty))
                    {
                        dr["ForceClose"] = dict["hdnForceClose"].ToString();
                    }
                    else
                    {
                        dr["ForceClose"] = "0";
                    }
                    dt.Rows.Add(dr);
                    index++;
                }
                
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    private DataTable GetPOItem()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));        // PO
        dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));    // GL
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));  // fDesc

        //dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        //dt.Columns.Add(new DataColumn("Price", typeof(string)));
        //dt.Columns.Add(new DataColumn("Amount", typeof(string)));

        DataColumn dcq = new DataColumn("Quan", typeof(string));
        dcq.AllowDBNull = true;
        DataColumn dPrice = new DataColumn("Price", typeof(string));
        dPrice.AllowDBNull = true;
        DataColumn dAmount = new DataColumn("Amount", typeof(string));
        dAmount.AllowDBNull = true;

        dt.Columns.Add(dcq);
        dt.Columns.Add(dPrice);
        dt.Columns.Add(dAmount);

        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(string)));    // Job
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));   // Phase
        dt.Columns.Add(new DataColumn("Inv", typeof(Int32)));       // Inv
        dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));    // Billed
        dt.Columns.Add(new DataColumn("Ticket", typeof(Int32)));    // Ticket
        dt.Columns.Add(new DataColumn("Due", typeof(DateTime)));      //due date
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("WarehouseID", typeof(string)));
        dt.Columns.Add(new DataColumn("LocationID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Warehousefdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Locationfdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("OpSq", typeof(string)));
        dt.Columns.Add(new DataColumn("Selected", typeof(string)));
        dt.Columns.Add(new DataColumn("SelectedQuan", typeof(string)));


        try
        {
            string strItems = hdnItemJSON.Value.Trim();
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["txtGvAcctNo"].ToString().Trim() == string.Empty)
                    {
                        //return dt;
                        //continue;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    if (!(dict["hdnTID"].ToString().Trim() == string.Empty))
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnTID"].ToString());
                    }
                    if (!(dict["hdnLine"].ToString().Trim() == string.Empty))
                    {
                        dr["Line"] = Convert.ToInt16(dict["hdnLine"].ToString());
                    }
                    dr["AcctID"] = Convert.ToInt32(dict["hdnAcctID"].ToString().Trim());
                    //_ChartIds = Convert.ToString(dict["hdnAcctID"].ToString().Trim());

                    if (_ChartIds.Length == 0)
                    {
                        _ChartIds = _ChartIds.Append(Convert.ToString(dict["hdnAcctID"].ToString().Trim()));
                    }
                    else
                    {
                        _ChartIds = _ChartIds.Append(",");
                        _ChartIds = _ChartIds.Append(Convert.ToString(dict["hdnAcctID"].ToString().Trim()));
                    }

                    dr["AcctNo"] = dict["txtGvAcctNo"].ToString().Trim();
                    dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();
                    //dr["Quan"] = dict["txtGvQuan"].ToString();
                    //dr["Price"] = dict["txtGvPrice"].ToString().Trim();
                    //dr["Amount"] = dict["txtGvAmount"].ToString().Trim();
                    dr["Quan"] = !string.IsNullOrEmpty(dict["txtGvQuan"].ToString()) ? dict["txtGvQuan"].ToString() : "1";
                    dr["Price"] = !string.IsNullOrEmpty(dict["txtGvPrice"].ToString()) ? dict["txtGvPrice"].ToString() : "0";
                    dr["Amount"] = !string.IsNullOrEmpty(dict["txtGvAmount"].ToString()) ? dict["txtGvAmount"].ToString() : (Convert.ToDouble(dr["Quan"]) * Convert.ToDouble(dr["Price"])).ToString();
                   // dr["Ticket"] = !string.IsNullOrEmpty(dict["txtGvTicket"].ToString()) ? dict["txtGvTicket"].ToString() : "0";
                    if (!(dict["txtGvLoc"].ToString().Trim() == string.Empty))
                    {
                        dr["Loc"] = dict["txtGvLoc"].ToString().Trim();
                    }
                    if (!(dict["txtGvJob"].ToString().Trim() == string.Empty))
                    {
                        dr["JobName"] = dict["txtGvJob"].ToString().Trim();
                    }
                    if (!(dict["hdnJobID"].ToString().Trim() == string.Empty))
                    {
                        dr["JobID"] = Convert.ToInt32(dict["hdnJobID"]);
                    }
                    else
                    {
                        dr["JobID"] = 0;
                    }
                    if (dict.ContainsKey("txtGvPhase"))
                    {
                        if (!(dict["txtGvPhase"].ToString().Trim() == string.Empty))
                        {
                            dr["Phase"] = dict["txtGvPhase"].ToString().Trim();
                        }
                    }
                    else
                    {
                        dr["Phase"] = "";
                    }

                    if (dict.ContainsKey("hdnPID"))
                    {
                        if (!(dict["hdnPID"].ToString().Trim() == string.Empty))
                        {
                            dr["PhaseID"] = Convert.ToInt32(dict["hdnPID"].ToString());
                        }
                    }
                    else
                    {
                        dr["PhaseID"] = 0;
                    }

                    if (dict.ContainsKey("hdnTypeId"))
                    {
                        if (!(dict["hdnTypeId"].ToString().Trim() == string.Empty))
                        {
                            dr["TypeID"] = Convert.ToInt32(dict["hdnTypeId"].ToString().Trim());
                        }
                    }

                    if (dict.ContainsKey("txtGvDue"))
                    {
                        if (!(dict["txtGvDue"].ToString().Trim() == string.Empty))
                        {
                            dr["Due"] = Convert.ToDateTime(dict["txtGvDue"].ToString());
                        }
                    }
                    else
                    {
                        dr["Due"] = DBNull.Value;
                    }

                    if (dict.ContainsKey("hdnItemID"))
                    {

                        if (!(dict["hdnItemID"].ToString().Trim() == string.Empty))
                        {
                            dr["Inv"] = Convert.ToInt32(dict["hdnItemID"]);
                            
                            if (_InvIds.Length == 0)
                            {
                                _InvIds = _InvIds.Append(Convert.ToString(dict["hdnItemID"].ToString().Trim()));
                            }
                            else
                            {
                                _InvIds = _InvIds.Append(",");
                                _InvIds = _InvIds.Append(Convert.ToString(dict["hdnItemID"].ToString().Trim()));
                            }

                        }
                    }

                    if (dict.ContainsKey("txtGvItem"))
                    {
                        if (!(dict["txtGvItem"].ToString().Trim() == string.Empty))
                        {
                            dr["ItemDesc"] = dict["txtGvItem"].ToString();
                        }

                    }

                    dr["WarehouseID"] = " ";

                    if (dict.ContainsKey("txtGvTicket"))
                    {
                        if (!(dict["txtGvTicket"].ToString().Trim() == string.Empty))
                        {
                            dr["Ticket"] = dict["txtGvTicket"].ToString();
                        }
                    }
                    else
                    {
                        dr["Ticket"] = "0";
                    }

                   // if (dict.ContainsKey("hdnWarehouse"))
                   //if (dict.ContainsKey("hdnWarehouse") && dict.ContainsKey("txtGvWarehouse"))
                   //{
                   //     //if (!(dict["hdnWarehouse"].ToString().Trim() == string.Empty))
                   //     if (dict["hdnWarehouse"].ToString().Trim() != string.Empty && dict["txtGvWarehouse"].ToString().Trim() != string.Empty)
                   //     {
                   //         dr["WarehouseID"] = dict["hdnWarehouse"].ToString();
                   //     }
                   //     else
                   //     {
                   //         dr["WarehouseID"] = "";
                            
                   //     }
                   // }

                    dr["LocationID"] = 0;

                    //if (dict.ContainsKey("hdnWarehouseLocationID"))
                    //{
                    //    if (!(dict["hdnWarehouseLocationID"].ToString().Trim() == string.Empty))
                    //    {
                    //        dr["LocationID"] = dict["hdnWarehouseLocationID"].ToString();
                    //    }
                    //}



                    if (dict.ContainsKey("hdnWarehouse") && dict.ContainsKey("txtGvWarehouse"))
                    {
                        if (dict["hdnWarehouse"].ToString().Trim() != string.Empty && dict["txtGvWarehouse"].ToString().Trim() != string.Empty)
                        {
                            dr["WarehouseID"] = dict["hdnWarehouse"].ToString();
                        }
                        else
                        {
                            dr["WarehouseID"] = "";
                        }
                    }
                    else
                    {
                        dr["WarehouseID"] = "";
                    }


                    if (dict.ContainsKey("hdnWarehouseLocationID"))
                    {
                        if (dict["hdnWarehouseLocationID"].ToString().Trim() != string.Empty)
                        {
                            dr["LocationID"] = dict["hdnWarehouseLocationID"].ToString();
                        }
                        else
                        {
                            dr["LocationID"] = "0";
                        }
                    }
                    else
                    {
                        dr["LocationID"] = "0";
                    }






                    if (!(dict["hdOpSq"].ToString().Trim() == string.Empty))
                    {
                        dr["OpSq"] = dict["hdOpSq"].ToString();
                    }
                    else
                    {
                        dr["OpSq"] = "100";
                    }

                    dr["Selected"] = 0;
                    dr["SelectedQuan"] = 0;

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

    private DataTable GetPOItemsByPOID(int pPOId)
    {
        try
        {
            StringBuilder varname = new StringBuilder();
            varname.Append("SELECT * FROM POItem WHERE po= ");
            varname.Append(pPOId);
            varname.Append(" AND ISNULL(ForceClose,0) <> 1");

            var connConfig = Session["config"].ToString();

            var ds = SqlHelper.ExecuteDataset(connConfig, CommandType.Text, varname.ToString());
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return new DataTable();
    }

    private void FillBomType()
    {
        try
        {
            DataSet ds = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            ds = objBL_Job.GetBomType(objJob);

            DataRow dr = ds.Tables[0].NewRow();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dr["ID"] = 0;
                dr["Type"] = "Select Type";
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }
            else
            {
                dr["ID"] = 0;
                dr["Type"] = "No data found";
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }
            ddlBomType.DataSource = ds.Tables[0];
            ddlBomType.DataTextField = "Type";
            ddlBomType.DataValueField = "ID";
            ddlBomType.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion
   
    protected void lbtnItemSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //var item = txtItem.Text;
            //var type = ddlBomType.SelectedItem.Text;
            //objJob.ConnConfig = Session["config"].ToString();
            //objJob.Job = Convert.ToInt32(hdnJobId.Value);
            //objJob.Code = txtOpSeq.Text;
            //objJob.Type = Convert.ToInt16(ddlBomType.SelectedValue);
            //objJob.ItemID = Convert.ToInt32(hdnItemID.Value);
            //objJob.fDesc = txtJobDesc.Text;
            //objJob.QtyReq = Convert.ToDouble(txtQty.Text);
            //objJob.UM = txtUM.Text;
            //objJob.BudgetUnit = Convert.ToDouble(txtBudgetUnit.Text);
            //objJob.BudgetExt = Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtBudgetUnit.Text);
            //objJob.ScrapFact = 0;
            //objJob.Line = objBL_Job.AddBOMItem(objJob);
            ////addedItem(item, itemId, phaseId, typeId, type, fdesc)
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalAmt", "addedItem('" + item + "', '" + objJob.ItemID + "', '" + objJob.Line + "', '" + objJob.Type + "', '" + type + "', '" + objJob.fDesc + "');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnk_PO_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]) && Request.QueryString["t"] == null)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["POReport"]) && ConfigurationManager.AppSettings["POReport"].ToLower().Contains(".rdlc"))
            {
                Response.Redirect("PrintPO.aspx?id=" + Request.QueryString["id"].ToString(), true);
            }
            else
            {
                Response.Redirect("PrintPOReport.aspx?id=" + Request.QueryString["id"].ToString(), true);
            }
        }
        else
        {
            submit(false);
            if (!string.IsNullOrEmpty(reportPOID))
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["POReport"]) && ConfigurationManager.AppSettings["POReport"].ToLower().Contains(".rdlc"))
                {
                    Response.Redirect("PrintPO.aspx?id=" + reportPOID, true);
                }
                else
                {
                    Response.Redirect("PrintPOReport.aspx?id=" + reportPOID, true);
                }
            }
        }

    }

    protected void lnk_CustomPOReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]) && Request.QueryString["t"] == null)
        {
            if (ConfigurationManager.AppSettings["CustomPOReport"].ToLower().Contains(".mrt"))
            {
                Response.Redirect("CustomPOReport.aspx?id=" + Request.QueryString["id"].ToString(), true);
            }
            else
            {
                Response.Redirect("PrintCustomPO.aspx?id=" + Request.QueryString["id"].ToString(), true);
            }
        }
        else
        {
            submit(false);
            if (!string.IsNullOrEmpty(reportPOID))
            {
                if (ConfigurationManager.AppSettings["CustomPOReport"].ToLower().Contains(".mrt"))
                {
                    Response.Redirect("CustomPOReport.aspx?id=" + reportPOID, true);
                }
                else
                {
                    Response.Redirect("PrintCustomPO.aspx?id=" + reportPOID, true);
                }
            }
        }
    }

    protected void lnk_POReportForTS_Click(object sender, EventArgs e)
    {
        Response.Redirect("PrintPOReportForTS.aspx?id=" + Request.QueryString["id"].ToString(), true);
    }

    public void GetTicketInfo()
    {
        if (Request.QueryString["TicketId"] != null)
        {
            Customer objProp_Customer = new Customer();
            BL_Customer objBL_Customer = new BL_Customer();

            BL_MapData objBL_MapData = new BL_MapData();
            MapData objMapData = new MapData();
            DataSet ds = new DataSet();
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["TicketId"].ToString());
            ds = objBL_MapData.GetTicketByID(objMapData);
            if (ds.Tables[0].Rows.Count > 0)
                //foreach (GridViewRow gr in gvGLItems.Rows)
                foreach (GridDataItem gr in RadGrid_AddPO.Items)
                {
                    string jobid = ds.Tables[0].Rows[0]["Job"].ToString();

                    objProp_Customer.ConnConfig = Session["config"].ToString();
                    objProp_Customer.ProjectJobID = Convert.ToInt32(jobid);
                    objProp_Customer.Type = string.Empty;

                    DataSet dsjob = new DataSet();
                    dsjob = objBL_Customer.getJobProjectByJobID(objProp_Customer);

                    TextBox txtGvJob = (TextBox)gr.FindControl("txtGvJob");
                    HiddenField hdnJobID = (HiddenField)gr.FindControl("hdnJobID");
                    TextBox txtGvTicket = (TextBox)gr.FindControl("txtGvTicket");
                    TextBox txtGvQuan = (TextBox)gr.FindControl("txtGvQuan");

                    HiddenField hdnAcctID = (HiddenField)gr.FindControl("hdnAcctID");
                    TextBox txtGvAcctNo = (TextBox)gr.FindControl("txtGvAcctNo");
                    TextBox txtGvLoc = (TextBox)gr.FindControl("txtGvLoc");
                    if (jobid != "0")
                    {
                        hdnJobID.Value = jobid;
                        txtGvJob.Text = ds.Tables[0].Rows[0]["Jobdesc"].ToString();
                        txtGvLoc.Text = dsjob.Tables[0].Rows[0]["locname"].ToString();
                        txtGvAcctNo.Text = dsjob.Tables[0].Rows[0]["InvExpName"].ToString();
                        hdnAcctID.Value = dsjob.Tables[0].Rows[0]["InvExp"].ToString();
                    }
                    txtDesc.Text = "Added from ticket# " + Request.QueryString["TicketId"].ToString();
                    txtGvQuan.Text = "1";
                    txtGvTicket.Text = Request.QueryString["TicketId"].ToString();
                    break;
                }
        }
    }


    
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        LinkButton btnApproveDecline = (LinkButton)sender;
        string msg = "";
        string emailTitle = string.Empty;
        string attachedFilename = string.Empty;
        var poId = Convert.ToInt32(Request.QueryString["id"]);
        var isNotification = ViewState["PONotification"] != null && ViewState["PONotification"].ToString() == "Y" ? true : false;

        if (btnApproveDecline.Text == "Approve")
        {
            _objApprovePOStatus.Status = 1;
            msg = "PO approved successfully!";
            _objApprovePOStatus.Comments =   txtStatusComment.Text;
            emailTitle = string.Format("PO #{0} Approve - {1}", poId, Session["username"].ToString());
            attachedFilename = string.Format("PO{0} Approve.pdf", poId);
        }
        else if(btnApproveDecline.Text == "Decline")
        {
            _objApprovePOStatus.Status = 2;
            msg = "PO rejected successfully!";
            _objApprovePOStatus.Comments =  txtStatusComment.Text;
            emailTitle = string.Format("PO #{0} Declined - {1}", poId, Session["username"].ToString());
            attachedFilename = string.Format("PO{0} Declined.pdf", poId);
        }

        _objApprovePOStatus.ConnConfig = Session["config"].ToString();
        _objApprovePOStatus.POID = poId;
        _objApprovePOStatus.UserID = Convert.ToInt32(Session["userid"]);

        int _EN = 0 , _inventoryHasAvailable  = 0;

        if (hdnEN.Value != null && hdnEN.Value != string.Empty)
          {
            _EN =  Convert.ToInt32(hdnEN.Value);
        }

        if (hdnHasInventory.Value != null && hdnHasInventory.Value != string.Empty)
        {
            _inventoryHasAvailable = Convert.ToInt32(hdnHasInventory.Value);
        }

        string reportPath = string.Empty, filename = string.Empty, savepathconfig, filepath = string.Empty;
        if (hdnCustomProgram.Value != string.Empty && hdnCustomProgram.Value.ToLower() == "mitsu")
        {
            if (_accountName == "MELTEC-OO")
            {
                ////ES - 1674 If the Vendor.ID = 'MELTEC' then use template "Purchase Order EED -HQ MELTEC.rpt/mrt
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order/Purchase Order Eed -HQ MELTEC.mrt";
                filename = "PO_EedHQMeltec_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
            }
            else if (_EN != 0 && _inventoryHasAvailable == 0)
            {
                ////ES - 1674 If the Vendor.EN <> 00 and there are no inventory then use "Purchase Order EED Branch.rpt/.mrt            
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order Eed Branch.mrt";
                filename = "PO_EedBranch_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
            }
            else if (_EN != 0 && _inventoryHasAvailable > 0)
            {
                ////ES - 1674 If the Vendor.EN <> 00 and there are inventory items use "Purchase Order Inv- Branch.rpt/.mrt
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order/Purchase Order Inv- Branch.mrt";
                filename = "PO_InvBranch_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
            }
            else if (_EN == 0 && _inventoryHasAvailable == 0)
            {
                ////ES - 1674 If the Vendor.EN = 00 and no inventory items use "Purchase Order EED - HQ Only.rpt/.mrt
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order Eed - HQ Only.mrt";
                filename = "PO_EedHQ_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
            }
            else if (_EN == 0 && _inventoryHasAvailable > 0)
            {
                ////ES - 1674 If the Vendor.EN = 00 and there are inventory items use "Purchase Order Inv-HQ.rpt/.mrt
                reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order/Purchase Order Inv-HQ.mrt";
                filename = "PO_InvHQ_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
            }

            savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["POAttachmentPath"].TrimEnd('\\').Trim() + "\\ApprovedPO\\";
            filepath = savepathconfig + filename;

            using (new NetworkConnection())
            {
                if (!Directory.Exists(savepathconfig))
                {
                    Directory.CreateDirectory(savepathconfig);
                }
                FileUpload1.SaveAs(filepath);
            }
        }
        //else
        //{
        //    reportPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "/StimulsoftReports/Purchase Order/Purchase Order Eed -HQ MELTEC.mrt";
        //    filename = "PO_EedHQMeltec_" + _objApprovePOStatus.POID.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", string.Empty) + ".pdf";
        //    savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["POAttachmentPath"].TrimEnd('\\').Trim() + "\\ApprovedPO\\";
        //    filepath = savepathconfig + filename;

        //    using (new NetworkConnection())
        //    {
        //        if (!Directory.Exists(savepathconfig))
        //        {
        //            Directory.CreateDirectory(savepathconfig);
        //        }
        //        FileUpload1.SaveAs(filepath);
        //    }
        //}

        if (hdnImg.Value != "")
        {
            string str = hdnImg.Value;
            string last = str.Substring(str.LastIndexOf(',') + 1);
            _objApprovePOStatus.Signature = Convert.FromBase64String(last);
        }
        _objApprovePOStatus.FileName = filename;
        _objApprovePOStatus.FilePath = filepath;
        DataSet ApprovePO = _objBLBills.POApproveDetails(_objApprovePOStatus);

        if (ApprovePO.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(reportPath))
        {
            byte[] buffer = null;

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: '" + msg + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            StiReport report = new StiReport();
            report.Load(reportPath);
            report.Compile();

            DataSet POHead = new DataSet();
            DataTable hTable = ApprovePO.Tables[0].Copy();
            hTable.TableName = "POHead";
            POHead.Tables.Add(hTable);
            POHead.DataSetName = "POHead";

            DataSet POItem = new DataSet();
            DataTable dTable = ApprovePO.Tables[1].Copy();
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

            if (buffer != null)
            {
                using (new NetworkConnection())
                {
                    if (File.Exists(filepath))
                        File.Delete(filepath);
                    using (var fs = new FileStream(filepath, FileMode.Create))
                    {
                        fs.Write(buffer, 0, buffer.Length);
                        fs.Close();
                    }
                }
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (buffer.Length).ToString());
            Response.BinaryWrite(buffer);

            Mail mail = new Mail();
            mail.From = WebBaseUtility.GetFromEmailAddress();
            var userEmail = string.Empty;
            var isDBTotalService = false;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["isDBTotalService"]))
            {
                isDBTotalService = Convert.ToBoolean(ConfigurationManager.AppSettings["isDBTotalService"]);
            }

            if (hdnCustomProgram.Value != string.Empty && hdnCustomProgram.Value.ToLower() == "mitsu" || isDBTotalService)
            {
                _objPropUser.ConnConfig = Session["config"].ToString();
                _objPropUser.Username = Session["username"].ToString();
                userEmail = _objBLUser.getUserEmailFromTS(_objPropUser);
            }
            else
            {
                _objPropUser.ConnConfig = Session["config"].ToString();
                _objPropUser.Username = Session["username"].ToString();
                userEmail = _objBLUser.getUserEmail(_objPropUser);
            }

            if (isNotification)
            {
                var emailValidation = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
                if (emailValidation.IsValid(userEmail))
                {
                    mail.To.Add(userEmail);
                }
                if (mail.To.Count > 0)
                {
                    mail.Title = emailTitle;

                    mail.Text = "Please see the attached PO generated in Mobile Office Manager.";

                    mail.attachmentBytes = buffer;
                    mail.FileName = attachedFilename;

                    mail.DeleteFilesAfterSend = true;
                    mail.RequireAutentication = false;
                    try
                    {
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

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: '" + msg + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);

        Response.Redirect("addpo.aspx?id=" + poId);
    }

    protected void RadGrid_AddPO_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteTransaction")
            {
                int index = 1;
                int indexRow = Convert.ToInt32(e.CommandArgument);
                var dt = BuildPODetailsTable();

                for (var i = 0; i < RadGrid_AddPO.Items.Count; i++)
                {
                    if (i != indexRow)
                    {
                        GridDataItem row = RadGrid_AddPO.Items[i];

                        HiddenField hdnTID = (HiddenField)row.FindControl("hdnTID");
                        HiddenField hdnLine = (HiddenField)row.FindControl("hdnLine");
                        HiddenField hdnAcctID = (HiddenField)row.FindControl("hdnAcctID");
                        TextBox txtGvAcctNo = (TextBox)row.FindControl("txtGvAcctNo");
                        TextBox txtGvDesc = (TextBox)row.FindControl("txtGvDesc");
                        TextBox txtGvAmount = (TextBox)row.FindControl("txtGvAmount");
                        TextBox txtGvQuan = (TextBox)row.FindControl("txtGvQuan");
                        TextBox txtGvPrice = (TextBox)row.FindControl("txtGvPrice");
                        TextBox txtGvLoc = (TextBox)row.FindControl("txtGvLoc");
                        TextBox txtGvJob = (TextBox)row.FindControl("txtGvJob");
                        HiddenField hdnJobID = (HiddenField)row.FindControl("hdnJobID");
                        TextBox txtGvPhase = (TextBox)row.FindControl("txtGvPhase");
                        HiddenField hdnPID = (HiddenField)row.FindControl("hdnPID");
                        TextBox txtGvDue = (TextBox)row.FindControl("txtGvDue");
                        HiddenField hdnTypeId = (HiddenField)row.FindControl("hdnTypeId");
                        TextBox txtGvItem = (TextBox)row.FindControl("txtGvItem");
                        TextBox txtGvTicket = (TextBox)row.FindControl("txtGvTicket");
                        HiddenField hdnItemID = (HiddenField)row.FindControl("hdnItemID");
                        HiddenField hdOpSq = (HiddenField)row.FindControl("hdOpSq");
                        TextBox txtGvWarehouse = (TextBox)row.FindControl("txtGvWarehouse");
                        HiddenField hdnWarehouse = (HiddenField)row.FindControl("hdnWarehouse");
                        TextBox txtGvWarehouseLocation = (TextBox)row.FindControl("txtGvWarehouseLocation");
                        HiddenField hdnWarehouseLocationID = (HiddenField)row.FindControl("hdnWarehouseLocationID");
                        HiddenField hdnForceClose = (HiddenField)row.FindControl("hdnForceClose");
                        
                        var dr = dt.NewRow();
                        dr["RowID"] = index;
                        dr["Line"] = index;
                        dr["ID"] = hdnTID.Value;
                        dr["AcctID"] = hdnAcctID.Value;
                        dr["AcctNo"] = txtGvAcctNo.Text;
                        dr["fDesc"] = txtGvDesc.Text;
                        dr["Loc"] = txtGvLoc.Text;
                        dr["JobName"] = txtGvJob.Text;
                        dr["JobID"] = hdnJobID.Value;
                        dr["Phase"] = txtGvPhase.Text;
                        dr["PhaseID"] = hdnPID.Value;
                        dr["TypeID"] = hdnTypeId.Value;
                        dr["Quan"] = txtGvQuan.Text;
                        dr["Price"] = txtGvPrice.Text;
                        dr["Amount"] = txtGvAmount.Text;
                        dr["Due"] = txtGvDue.Text;
                        dr["Ticket"] = txtGvTicket.Text;
                        dr["ItemDesc"] = txtGvItem.Text;
                        dr["Inv"] = hdnItemID.Value;
                        dr["OpSq"] = hdOpSq.Value;
                        dr["Warehousefdesc"] = txtGvWarehouse.Text;
                        dr["WarehouseID"] = hdnWarehouse.Value;
                        dr["Locationfdesc"] = txtGvWarehouseLocation.Text;
                        dr["LocationID"] = hdnWarehouseLocationID.Value;

                        if (hdnForceClose.Value.ToString() == string.Empty)
                        {
                            dr["ForceClose"] = "0";
                        }
                        else
                        {
                            dr["ForceClose"] = hdnForceClose.Value;
                        }
                        

                        dt.Rows.Add(dr);

                        index++;
                    }
                }

                RadGrid_AddPO.DataSource = dt;
                RadGrid_AddPO.DataBind();

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalAmt", "CalculateTotalAmt();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
     

        private void GetInvDefaultAcct()
    {
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);

        if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
        {
            hdnInvDefaultAcctID.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
            hdnInvDefaultAcctName.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["Acct"]);
        }
    }

    #region logs

    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
            if (Request.QueryString["id"] != null)
            {
                DataSet dsLog = new DataSet();
                _objPO.ConnConfig = Session["config"].ToString();
                _objPO.POID = Convert.ToInt32(Request.QueryString["id"]);
                dsLog = _objBLBills.GetPOLogs(_objPO);
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

    protected void txtVendor_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            _objPO.EN = 1;
            if (!string.IsNullOrEmpty(hdnVendorID.Value))
            {
                var vendorId = Convert.ToInt32(hdnVendorID.Value);
                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = vendorId;
                var dsAddress = _objBLVendor.GetVendorBasedPORemitAddress(_objVendor);
                if (dsAddress != null)
                {
                    txtShipTo.Text = dsAddress.Tables[0].Rows[0]["PORemit"].ToString();
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

    private List<byte[]> PrintTemplate(StiWebViewer rvTemplate, string templateName)
    {
        // Export to PDF
        List<byte[]> templateAsBytes = new List<byte[]>();
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


            DataSet dsC = new DataSet();
            _objPropUser.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                dsC = _objBLUser.getControl(_objPropUser);
            }
            else
            {
                _objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = _objBLUser.getControlBranch(_objPropUser);
            }

            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath(string.Format("Templates/POTemplates/{0}", templateName));
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            report.Compile();


            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyLogo = new DataSet();
            companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            var imageString = companyLogo.Tables[0].Rows[0]["Logo"].ToString();
            if (!string.IsNullOrEmpty(imageString))
            {
                byte[] barrImg = (byte[])(companyLogo.Tables[0].Rows[0]["Logo"]);
                string strfn = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempImages/" + DateTime.Now.ToFileTime().ToString());
                FileStream fs = new FileStream(strfn,
                                  FileMode.CreateNew, FileAccess.Write);
                fs.Write(barrImg, 0, barrImg.Length);
                fs.Flush();
                fs.Close();

                System.Uri uri = new Uri(strfn);
                cRow["LogoURL"] = uri.AbsolutePath;
            }

            cRow["CompanyName"] = string.IsNullOrEmpty(companyLogo.Tables[0].Rows[0]["Name"].ToString()) ? "" : companyLogo.Tables[0].Rows[0]["Name"].ToString();
            cRow["CompanyAddress"] = string.IsNullOrEmpty(companyLogo.Tables[0].Rows[0]["Address"].ToString()) ? "" : companyLogo.Tables[0].Rows[0]["Address"].ToString();
            cRow["ContactNo"] = string.IsNullOrEmpty(companyLogo.Tables[0].Rows[0]["Contact"].ToString()) ? "" : companyLogo.Tables[0].Rows[0]["Contact"].ToString();
            cRow["Email"] = string.IsNullOrEmpty(companyLogo.Tables[0].Rows[0]["Email"].ToString()) ? "" : companyLogo.Tables[0].Rows[0]["Email"].ToString();

            cRow["City"] = string.IsNullOrEmpty(companyLogo.Tables[0].Rows[0]["City"].ToString()) ? "" : companyLogo.Tables[0].Rows[0]["City"].ToString();
            cRow["State"] = string.IsNullOrEmpty(companyLogo.Tables[0].Rows[0]["State"].ToString()) ? "" : companyLogo.Tables[0].Rows[0]["State"].ToString();
            cRow["Phone"] = string.IsNullOrEmpty(companyLogo.Tables[0].Rows[0]["Phone"].ToString()) ? "" : companyLogo.Tables[0].Rows[0]["Phone"].ToString();
            cRow["Fax"] = string.IsNullOrEmpty(companyLogo.Tables[0].Rows[0]["Fax"].ToString()) ? "" : companyLogo.Tables[0].Rows[0]["Fax"].ToString();
            cRow["Zip"] = string.IsNullOrEmpty(companyLogo.Tables[0].Rows[0]["Zip"].ToString()) ? "" : companyLogo.Tables[0].Rows[0]["Zip"].ToString();

            cTable.Rows.Add(cRow);

            DataSet CompanyDetails = new DataSet();
            cTable.TableName = "CompanyDetails";
            CompanyDetails.Tables.Add(cTable);
            CompanyDetails.DataSetName = "CompanyDetails";
            report.RegData("CompanyDetails", CompanyDetails);


            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.POID = Convert.ToInt32(Request.QueryString["id"]);
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
                DataSet poInfo = new DataSet();
                poInfo.DataSetName = "POInfo";
                DataTable dtPOInfo = new DataTable();
                dtPOInfo = dsPO.Tables[0].Copy();
                dtPOInfo.TableName = "POInfo";
                poInfo.Tables.Add(dtPOInfo);
                report.RegData("POInfo", poInfo);

                DataTable dtPOItems = new DataTable();
                dtPOItems = dsPO.Tables[1].Copy();
                dtPOItems.TableName = "POItems";

                report.RegData("POItems", dtPOItems);
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

    protected DataTable BuildPODetailsTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("ID", typeof(string)));
        dt.Columns.Add(new DataColumn("Line", typeof(string)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(string)));
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        dt.Columns.Add(new DataColumn("Price", typeof(string)));
        dt.Columns.Add(new DataColumn("Amount", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(string)));
        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(string)));
        dt.Columns.Add(new DataColumn("TypeID", typeof(string)));
        dt.Columns.Add(new DataColumn("Ticket", typeof(string)));
        dt.Columns.Add(new DataColumn("Due", typeof(string)));
        dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Inv", typeof(string)));
        dt.Columns.Add(new DataColumn("OpSq", typeof(string)));
        dt.Columns.Add(new DataColumn("Warehousefdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("WarehouseID", typeof(string)));
        dt.Columns.Add(new DataColumn("Locationfdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("LocationID", typeof(string)));
        dt.Columns.Add(new DataColumn("ForceClose", typeof(Int32)));

        return dt;
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

        //ScriptManager.RegisterStartupScript(this, GetType(), "DeleteDoc", "$('.dropify').dropify();", true);
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

            if (Request.QueryString["id"] != null)
            {
                mainDirectory = Request.QueryString["id"].ToString();
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
            //savepath = GetUploadDirectory();


            //if (FileUpload1.HasFile)
            if (!string.IsNullOrEmpty(FileUpload1.FileName))
            {
                filename = FileUpload1.FileName;
                fullpath = savepath + filename;
                mime = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                if (File.Exists(fullpath))
                {
                    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                    filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                    fullpath = savepath + filename;
                }
                //var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["POAttachmentPath"].Trim();
                //var credential = new System.Net.NetworkCredential();
                //credential.Password = "enclaveit@123";
                //credential.UserName = "Turlock";
                //var sss = new NetworkConnection(savepathconfig, credential);
                using (new NetworkConnection())
                {
                    if (!Directory.Exists(savepath))
                    {
                        Directory.CreateDirectory(savepath);
                    }

                    FileUpload1.SaveAs(fullpath);
                }
            }


            if (Request.QueryString["id"] != null)
            {

                objMapData.Screen = "PO";
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
                objMapData.TempId = "0";
                objMapData.FileName = filename;
                objMapData.DocTypeMIME = mime;
                objMapData.FilePath = fullpath;

                objMapData.DocID = 0;
                objMapData.Mode = 0;
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.Worker = Session["User"].ToString();
                objBL_MapData.AddFile(objMapData);
                UpdateDocInfo();
                //GetDocuments();
                RadGrid_Documents.Rebind();
                RadGrid_gvLogs.Rebind();
            }
            else
            {
                var tempTable = SaveAttachedFilesWhenAddingPO(filename, fullpath, mime);
                RadGrid_Documents.DataSource = tempTable;
                RadGrid_Documents.VirtualItemCount = tempTable.Rows.Count;
                RadGrid_Documents.DataBind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private string GetUploadDirectory(string mainDirectory)
    {
        var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["POAttachmentPath"].Trim();
        return savepathconfig + @"\" + Session["dbname"] + @"\ld_" + mainDirectory + @"\";
    }

    private string GetUploadDirectory()
    {
        var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["POAttachmentPath"].Trim();
        return savepathconfig;// + @"\" + Session["dbname"] + @"\PO" + @"\ld_" + mainDirectory + @"\";
    }

    private DataTable SaveAttachedFilesWhenAddingPO(string fileName, string fullPath, string doctype)
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

    private void UpdateDocInfo()
    {
        _objPropUser.ConnConfig = Session["config"].ToString();
        _objPropUser.dtDocs = SaveDocInfo();
        _objPropUser.Username = Session["User"].ToString();
        _objBLUser.UpdateDocInfo(_objPropUser);
    }

    private void GetDocuments()
    {
        if (Request.QueryString["id"] != null)
        {
            objMapData.Screen = "PO";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
            objMapData.TempId = "0";
            objMapData.Mode = 1;
            objMapData.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_MapData.GetDocuments(objMapData);
            //gvDocuments.DataSource = ds.Tables[0];
            //gvDocuments.DataBind();
            RadGrid_Documents.DataSource = ds.Tables[0];
            RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
            //RadGrid_Documents.DataBind();
        }
        else
        {
            var source = ViewState["AttachedFiles"] as DataTable;
            pnlDocumentButtons.Visible = true;
            RadGrid_Documents.DataSource = source;
            RadGrid_Documents.VirtualItemCount = source != null ? source.Rows.Count : 0;
            //RadGrid_Documents.DataBind();
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

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objMapData.Worker = Session["User"].ToString();
            objBL_MapData.DeleteFile(objMapData);
            UpdateDocInfo();
            //GetDocuments();
            RadGrid_Documents.Rebind();
            RadGrid_gvLogs.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
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

    private void UpdateTempDateWhenCreatingNewPO(int poId)
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
        //var sourceDirectory = GetUploadDirectory();
        var newDirectory = GetUploadDirectory(poId.ToString());
        Directory.Move(sourceDirectory, newDirectory);

        foreach (DataRow row in tempAttachedFiles.Rows)
        {
            objMapData.Screen = "PO";
            objMapData.TicketID = poId;
            objMapData.TempId = "0";
            objMapData.FileName = row.Field<string>("filename");
            objMapData.DocTypeMIME = row.Field<string>("doctype");
            objMapData.FilePath = row.Field<string>("Path").Replace(sourceDirectory, newDirectory);
            //objMapData.FilePath = row.Field<string>("Path");
            objMapData.DocID = 0;
            objMapData.Mode = 0;
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.Worker = Session["User"].ToString();
            objBL_MapData.AddFile(objMapData);
        }

        ViewState["TempUploadDirectory"] = null;
        ViewState["AttachedFiles"] = null;


        //get document     
        objMapData.Screen = "PO";
        objMapData.TicketID = poId;
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

    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Split(',');

        string FileName = CommandArgument[0];

        string FilePath = CommandArgument[1];

        DownloadDocument(FilePath, FileName);
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            using (new NetworkConnection())
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


    private void DocumentPermission()
    {

        if (Convert.ToString(Session["type"]) != "am" && Convert.ToString(Session["type"]) != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            //Document--------------------->

            string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
            hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
            hdnEditDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
            hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
            hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

            if (hdnDeleteDocument.Value == "N")
            {
                lnkDeleteDoc.Enabled = false;
            }
            else
            {
                lnkDeleteDoc.Enabled = true;
            }

            if (hdnAddeDocument.Value == "N")
            {
                lnkUploadDoc.Enabled = false;
            }
            else{
                lnkUploadDoc.Enabled = true;
            }
            pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;
        }

    }

    private void RowSelectDocuments()
    {
        if (hdnEditDocument.Value == "N")
        {
            foreach (GridDataItem item in RadGrid_Documents.Items)
            {
                //CheckBox chkSelected = (CheckBox)item.FindControl("chkSelect");
                CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
                TextBox txtremarks = (TextBox)item.FindControl("txtremarks");
                //chkSelected.Enabled = 
                chkPortal.Enabled = txtremarks.Enabled = false;
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";


            }
        }
    }

    protected void RadGrid_Documents_PreRender(object sender, EventArgs e)
    {
        RowSelectDocuments();
    }

    protected void RadGrid_Documents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        
            //RadGrid_Documents.AllowCustomPaging = !ShouldApplySortFilterOrGroupEquip();

            //if ((Request.QueryString["uid"]) != null)
            //{
            //    GetDataEquip();
            //}
        GetDocuments();
        
    }

    private int GetNewPOId()
    {
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "NextPO");
        int nextPONumber = 0;
        if (_dsCustom.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                {
                    nextPONumber = Convert.ToInt32(_dr["Label"]);
                }
            }
        }

        _objPO.ConnConfig = Session["config"].ToString();
        int PoId = _objBLBills.GetMaxPOId(_objPO);
        if(nextPONumber >= PoId)
        {
            return nextPONumber;
        }
        else
        {
            return PoId;
        }
    }

    private void DisableApprovalStatus(int approvalStatus)
    {
          if (approvalStatus == 1)
        {
            ddlApprovalStatus.Items[0].Attributes["disabled"] = "disabled";
            ddlApprovalStatus.Items[1].Attributes["disabled"] = "disabled"; 
            ddlApprovalStatus.Items[2].Attributes["enabled"] = "enabled";
            ddlApprovalStatus.Items[3].Attributes["disabled"] = "disabled";
            ddlApprovalStatus.Items[4].Attributes["enabled"] = "enabled";
        }
        else if (approvalStatus == 0 || approvalStatus == 2 || approvalStatus == 3)
        {
            ddlApprovalStatus.Items[0].Attributes["disabled"] = "disabled";
            ddlApprovalStatus.Items[1].Attributes["disabled"] = "disabled";
            ddlApprovalStatus.Items[2].Attributes["disabled"] = "disabled";
            ddlApprovalStatus.Items[3].Attributes["disabled"] = "disabled";
            ddlApprovalStatus.Items[4].Attributes["disabled"] = "disabled";
        }
       
    }
    protected void ddlApprovalStatus_SelectedIndexChanged (object sender, EventArgs e)
    {
        //if (ddlApprovalStatus.SelectedValue == "1")
        //{
        //    if(_poApprovalStatus == 1)
        //    {
        //        ShowMesg("Project has alread approved,Select reapprove only.", 0);
        //    } 
        //}
        //if (ddlApprovalStatus.SelectedValue == "2")
        //{
        //    if (_poApprovalStatus == 1)
        //    {
        //        ShowMesg("Project has alread declined.Not allowed to change the status", 0);
        //    }
        //}
    }

    private void fillPOCustom()
    {
        try
        {
            DataSet dscstm = new DataSet();

            dscstm = GetCustomFields("PO1");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                lb_txtPO1.InnerHtml = dscstm.Tables[0].Rows[0]["label"].ToString()==""?"Custom 1" : dscstm.Tables[0].Rows[0]["label"].ToString();
            }
            dscstm = GetCustomFields("PO2");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                lb_txtPO2.InnerHtml = dscstm.Tables[0].Rows[0]["label"].ToString() == "" ? "Custom 2" : dscstm.Tables[0].Rows[0]["label"].ToString();
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private DataSet GetCustomFields(string name)
    {
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        DataSet ds = new DataSet();
        objGeneral.CustomName = name;
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getCustomFields(objGeneral);
        return ds;
    }
}
public static class ExceptionLogging
{

    private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;

    public static void SendMsgToText(string Activity)
    {
        var line = Environment.NewLine + Environment.NewLine;

        //ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
        //Errormsg = ex.GetType().Name.ToString();
        //extype = ex.GetType().ToString();
        //exurl = context.Current.Request.Url.ToString();
        //ErrorLocation = ex.Message.ToString();

        try
        {
            string filepath = context.Current.Server.MapPath("~/ExceptionDetailsFile/");  //Text File Path

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);

            }
            filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
            if (!File.Exists(filepath))
            {


                File.Create(filepath).Dispose();

            }
            using (StreamWriter sw = File.AppendText(filepath))
            {
                //string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                string error = Activity + " " + DateTime.Now.ToString() +  line;
                //sw.WriteLine("-----------Exception Start on " + " " + DateTime.Now.ToString() + "-----------------");
                //sw.WriteLine("-------------------------------------------------------------------------------------");
                //sw.WriteLine(line);
                sw.WriteLine(error);
                //sw.WriteLine("--------------------------------*End*------------------------------------------");
                //sw.WriteLine(line);
                sw.Flush();
                sw.Close();

            }

        }
        catch (Exception e)
        {
            e.ToString();

        }
    }

}