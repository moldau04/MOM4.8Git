using BusinessEntity;
using BusinessLayer;
using BusinessLayer.Purchasing;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AddReceivePO : System.Web.UI.Page
{
    #region Variable
   // User _objPropUser = new User();

    

    MapData objMapData = new MapData();
    BL_MapData objBL_MapData = new BL_MapData();

    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();

    Vendor _objVendor = new Vendor();

    BL_Vendor _objBLVendor = new BL_Vendor();

    PO _objPO = new PO();
    //BL_PO _objBLPO = new BL_PO();

    BL_Bills _objBLBills = new BL_Bills();

    BL_Inventory _objInventory = new BL_Inventory();
    BusinessEntity.Inventory _objInv = new BusinessEntity.Inventory();
    string strErrorMessage = string.Empty;
    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();
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
                //txtDate_CalendarExtender.StartDate = DateTime.Now;
                lblTotal.Text = "$0.00";

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
                hdnIsInventoryTrackingIsOn.Value = TrackingInventory ? "1" : "0";

                if (Request.QueryString["id"] != null)
                {
                    Page.Title = "Edit PO Reception || MOM";
                    lblHeader.Text = "PO Reception";
                    liLogs.Style["display"] = "inline-block";
                    tbLogs.Style["display"] = "block";
                    _objPO.ConnConfig = Session["config"].ToString();
                    _objPO.RID = Convert.ToInt32(Request.QueryString["id"]);
                    hdnMode.Value = "1"; // Edit mode 
                    DataSet ds = _objBLBills.GetReceivePoById(_objPO);

                    //DataSet ds = _objBLPO.GetReceivePoById(_objPO);

                    DataRow dr = ds.Tables[0].Rows[0];
                    txtReception.Text = dr["ID"].ToString();
                    txtPO.Text = dr["PO"].ToString();
                    txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToString("MM/dd/yyyy");
                    if (!string.IsNullOrEmpty(dr["ReceiveDate"].ToString()))
                    {
                        txtDate.Text = Convert.ToDateTime(dr["ReceiveDate"]).ToString("MM/dd/yyyy");
                    }
                    txtRef.Text = dr["Ref"].ToString();
                    txtVendor.Text = dr["VendorName"].ToString();
                    txtVendorType.Text = dr["VendorType"].ToString();
                    hdnVendorID.Value = dr["Vendor"].ToString();
                    txtTrkWB.Text = dr["WB"].ToString();
                    txtShipTo.Text = dr["ShipTo"].ToString();
                    txtCreatedBy.Text = dr["fby"].ToString();
                    txtRcomments.Text = dr["Comments"].ToString();
                    txtComments.Text = dr["fDesc"].ToString();
                    txtAddress.Text = dr["Address"].ToString();

                    lblTotal.Text = "$" + string.Format("{0:c}", dr["ReceivedAmount"].ToString());

                    string status = "New";
                    switch (Convert.ToInt16(dr["Status"]))
                    {
                        case 0:
                            status = "New";
                            break;
                        case 1:
                            status = "Closed";
                            break;
                        case 2:
                            status = "Cancelled";
                            break;
                        case 3:
                            status = "Partial-Quantity";
                            break;
                        case 4:
                            status = "Partial-Amount";
                            break;
                    }
                    txtStatus.Text = status;

                    ///// If PO Closed User can't able to edite it.

                    if (status == "Closed")
                    {
                        btnSubmit.Visible = false;
                    }
                    else
                    {
                        btnSubmit.Visible = true;
                    }

                    //ViewState["PO"] = ds.Tables[0];

                    txtReception.Enabled = false;
                    txtPO.Enabled = false;
                    txtDate.Enabled = false;
                    txtRef.Enabled = false;
                    txtVendor.Enabled = false;
                    txtVendorType.Enabled = false;
                    txtTrkWB.Enabled = false;
                    txtShipTo.Enabled = false;
                    txtCreatedBy.Enabled = false;
                    txtComments.Enabled = false;
                    txtAddress.Enabled = false;
                    txtStatus.Enabled = false;
                    txtRcomments.Enabled = false;

                    RadGrid_POItems.DataSource = ds.Tables[1];
                    RadGrid_POItems.DataBind();


                    //General _objPropGeneral = new General();
                    //BL_General _objBLGeneral = new BL_General();
                    //_objPropGeneral.ConnConfig = Session["config"].ToString();
                    //DataSet _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");
                    //Boolean TrackingInventory = false;
                    //if (_dsCustom.Tables[0].Rows.Count > 0)
                    //{
                    //    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                    //    {
                    //        if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                    //        {
                    //            TrackingInventory = Convert.ToBoolean(_dr["Label"]);
                    //        }
                    //    }
                    //}
                    //hdnIsInventoryTrackingIsOn.Value = TrackingInventory ? "1" : "0";

                    GridHeaderItem headerItem = (GridHeaderItem)RadGrid_POItems.MasterTableView.GetItems(GridItemType.Header)[0];
                    CheckBox chkSelectAll = headerItem.FindControl("chkSelectAll") as CheckBox;
                    chkSelectAll.Enabled = false;
                    chkSelectAll.Visible = false;
                    if (TrackingInventory == false)
                    {
                        RadGrid_POItems.Columns[17].Visible = false;
                        RadGrid_POItems.Columns[18].Visible = false;
                    }
                    else
                    {
                        RadGrid_POItems.Columns[17].Visible = true;
                        RadGrid_POItems.Columns[18].Visible = true;
                    }

                    //foreach (GridViewRow gr in gvPOItems.Rows)
                    foreach (GridDataItem gr in RadGrid_POItems.Items)
                    {

                        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectItem");

                        TextBox txtReceiveQty = (TextBox)gr.FindControl("txtReceiveQty");

                        TextBox txtReceive = (TextBox)gr.FindControl("txtReceive");

                        Label lblJobID = (Label)gr.FindControl("lblJobID");

                        HiddenField hdnIsReceiveIssued = (HiddenField)gr.FindControl("hdnIsReceiveIssued");
                        //HiddenField hdnIsInventoryTrackingIsOn = (HiddenField)gr.FindControl("hdnIsInventoryTrackingIsOn");
                        HiddenField hdnIsProjectSelect = (HiddenField)gr.FindControl("hdnIsProjectSelect");

                        String jobID = lblJobID.Text;
                        HiddenField hdnInvID = (HiddenField)gr.FindControl("hdnInvID");
                        String InvID = hdnInvID.Value;

                        DropDownList drpRecievepoIssued = (DropDownList)gr.FindControl("drpRecievepoIssued");

                        try
                        {
                            drpRecievepoIssued.SelectedValue = hdnIsReceiveIssued.Value;

                            int hdnjobID = 0;

                            int.TryParse(lblJobID.Text, out hdnjobID);

                            if (hdnjobID > 0) { hdnIsProjectSelect.Value = "1"; }

                            else { hdnIsProjectSelect.Value = "0"; }
                        }
                        catch { }

                        //if (txtReceiveQty.Text != "0")
                        //{
                        //    chkSelect.Checked = true;
                        //}

                        chkSelect.Checked = true;
                        chkSelect.Enabled = false;
                        //chkSelect.Visible = false;

                        if (status.Equals("New", StringComparison.InvariantCultureIgnoreCase))
                        {
                            gr.Enabled = true;
                        }
                        else
                        {
                            gr.Enabled = false;
                        }
                    }
                    

                }
                else if (Request.QueryString["PO"] != null)
                {
                    Page.Title = "Add PO Reception || MOM";
                    hdnMode.Value = "0";// Add new mode
                    txtCreatedBy.Text = Session["Username"].ToString();
                    _objPO.ConnConfig = Session["config"].ToString();
                    txtDate.Text = DateTime.Now.ToShortDateString();
                    txtReception.Enabled = false;
                    txtCreatedBy.Enabled = false;
                    txtAddress.Enabled = false;
                    txtShipTo.Enabled = false;
                    txtStatus.Enabled = false;
                    txtComments.Enabled = false;
                    txtVendor.Enabled = true;
                    txtVendorType.Enabled = true;
                    txtPO.Text = Convert.ToString(Request.QueryString["PO"]);
                    GetPOByIdAjax(txtPO.Text);
                    GetAllPOByVendor();
                    RadGrid_POItems.DataSource = "";
                    RadGrid_POItems.DataBind();
                    btnSelectPO_Click(sender, e);
                }
                else
                {
                    Page.Title = "Add PO Reception || MOM";
                    hdnMode.Value = "0";// Add new mode
                    txtCreatedBy.Text = Session["Username"].ToString();

                    _objPO.ConnConfig = Session["config"].ToString();

                    // Creating id for the next receive
                    //int id = _objBLBills.GetMaxReceivePOId(_objPO);

                    txtDate.Text = DateTime.Now.ToShortDateString();

                    //txtReception.Text = id.ToString();
                    txtReception.Enabled = false;
                    //txtDueDate.Enabled = false;
                    txtCreatedBy.Enabled = false;
                    txtAddress.Enabled = false;
                    txtShipTo.Enabled = false;
                    txtStatus.Enabled = false;
                    txtComments.Enabled = false;
                    txtVendor.Enabled = true;
                    txtVendorType.Enabled = true;

                    //_objPO.ConnConfig = Session["config"].ToString();
                    //_objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
                    //if (Session["CmpChkDefault"].ToString() == "1")
                    //{
                    //    _objPO.EN = 1;
                    //}
                    //else
                    //{
                    //    _objPO.EN = 0;
                    //}
                    //DataTable _dtOpenPO = _objBLBills.GetAllPOByDue(_objPO);
                    ////DataTable _dtOpenPO = new DataTable();
                    ////ViewState["PO"] = ds.Tables[0];

                    //Session["dsPOData"] = _dtOpenPO;

                    //RadGrid_ReceivePO.DataSource = _dtOpenPO;
                    //RadGrid_ReceivePO.DataBind();
                    GetAllPOByVendor();

                    RadGrid_POItems.DataSource = "";
                    RadGrid_POItems.DataBind();

                }

                FillUserAddress();


                /// Successfully alert 
                if (Session["RPORefNO"] != null)
                //if (Request.QueryString["RefNO"] != null)
                {
                    string _strReceptionNo = Convert.ToString(Session["RPORefNO"]);
                    Session["RPORefNO"] = null;
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Saved successfully! </br><b> RPO# : " + _strReceptionNo + "</b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        pnlNext.Visible = false;
        if (Request.QueryString["id"] != null)
        {
            pnlNext.Visible = true;
            

        }

        userpermissions();

    }
    #endregion
    protected void btnSelectVendor_Click(object sender, EventArgs e)
    {
        GetAllPOByVendor();        
    }    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            bool _flag = false;
            GetPeriodDetails(Convert.ToDateTime(txtDate.Text));
            _flag = (bool)ViewState["FlagPeriodClose"];
            bool IsExist;

            if (_flag)
            {
                if (Request.QueryString["id"] == null)
                {
                    var selectedItems = GetUnReceivedSelectedItems("ADD");
                    
                    if (selectedItems.Count > 0)
                    {
                        
                        DataTable dtRPO = (DataTable)ViewState["dtRPOItem"];
                                                
                        double sum = Convert.ToInt32(dtRPO.Compute("SUM(Amount)", string.Empty));
                        //DataRow[] rw = dtRPO.Select("Quan = " + 0 + " AND Amount < " + qunatity + " AND MAX_AMOUNT > " + qunatity);

                        //DataView dataView = newsDataSet.Tables[0].DefaultView;
                        //dataView.RowFilter = "NewsDate2 Like '%" + yearID + "'";


                        //string errMessage = ValidateSelectedItems(selectedItems);
                        //if (string.IsNullOrEmpty(errMessage))
                        //{
                        ///
                        ///===> TO Avoid Violation primary Key Error , Check Reception No # Already not Exists in DB. 
                        ///
                        //_objPO.ConnConfig = Session["config"].ToString();

                        //int ID_NEW = _objBLBills.GetMaxReceivePOId(_objPO);

                        //if (txtReception.Text != ID_NEW.ToString())
                        //{
                        //    txtReception.Text = ID_NEW.ToString();
                        //}

                        // _objPO.Ref = txtRef.Text;
                        // _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);
                        // IsExist = _objBLBills.IsExistRPOForInsert(_objPO);

                        //if (IsExist.Equals(false))
                        //{
                        double amount = 0;
                        //double poAmount = 0;
                        //double poQty = 0;
                        double qty = 0;

                        ///////// ES-3793 Check Active/Inactive Item ///////
                        //foreach (GridDataItem gr in selectedItems)
                        //{
                        //HiddenField hdnInvID = (HiddenField)gr.FindControl("hdnInvID");
                        //HiddenField hdnfDesc = (HiddenField)gr.FindControl("hdnfDesc");
                        //DropDownList drpRecievepoIssued = (DropDownList)gr.FindControl("drpRecievepoIssued");
                        //int Inv = 0;
                        //if (Convert.ToString(hdnInvID.Value).ToString().Trim() != "" && hdnInvID.Value != string.Empty && hdnInvID.Value != null )
                        //{
                        //     Inv = Convert.ToInt32(hdnInvID.Value);
                        //}
                        //string sName = Convert.ToString(hdnfDesc.Value);

                        //if (Inv > 0 && Convert.ToInt32(drpRecievepoIssued.SelectedValue) == 2)
                        //{
                        //    DataSet _dsInv = new DataSet();
                        //    _objInv.ConnConfig = Session["config"].ToString();
                        //    _objInv.ID = Inv;
                        //    _objInv.UserID = Convert.ToInt32(Session["UserID"].ToString());
                        //    _dsInv = _objBLBills.GetInventoryItemStatus(_objInv);
                        //    int acctstatus = Convert.ToInt32(_dsInv.Tables[0].Rows[0]["Status"].ToString());
                        //    if (acctstatus == 1)
                        //    {
                        //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Item " + sName + " is Inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                        //        return;

                        //    }
                        //}






                        //}
                        ///////// ES-3793 Check Active/Inactive Item ///////




                        // Get poAmount and poQty: need to check all item in Grid
                        // TODO: this should get from database is better.
                        // Will do it later
                        //foreach (GridDataItem gv in RadGrid_POItems.Items)
                        //{
                        //    Label lblBOQty = (Label)gv.FindControl("lblBOQty");
                        //    Label lblOutstand = (Label)gv.FindControl("lblOutstand");
                        //    poQty = poQty + Convert.ToDouble(lblBOQty.Text);
                        //    poAmount = poAmount + ConvertCurrentCurrencyFormatToDbl(lblOutstand.Text);

                        //}

                        //bool IsAmount = false;
                        int countItem = 0;
                        int BatchID = 0;
                        double Inv_Total = 0;
                        int Inv_Qty = 0;

                        Boolean TrackingInventory = hdnIsInventoryTrackingIsOn.Value == "1" ? true : false;
                        //#region Update PO balance
                        General _objPropGeneral = new General();
                        BL_General _objBLGeneral = new BL_General();
                        _objPropGeneral.ConnConfig = Session["config"].ToString();
                        DataSet _dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);
                        int DefaultAcctID = 0;
                        if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
                        {
                            DefaultAcctID = Convert.ToInt32(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
                        }


                        #region Add Reception PO
                        _objPO.ConnConfig = Session["config"].ToString();
                        _objPO.RID = 0;
                        _objPO.POID = Convert.ToInt32(txtPO.Text);
                        _objPO.Ref = txtRef.Text;
                        _objPO.WB = txtTrkWB.Text;
                        _objPO.Comments = txtRcomments.Text;
                        _objPO.Amount = amount;
                        _objPO.fDate = Convert.ToDateTime(txtDate.Text);
                        _objPO.BatchID = BatchID;
                        _objPO.IsReceiveIssued = 1;
                        _objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                        _objPO.MOMUSer = Session["User"].ToString();
                        _objPO.Dt = dtRPO;
                        _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);
                        _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());

                        if (Request.QueryString["PO"] == null)
                        {
                            _objPO.IsAddReceivePO = false;
                        }
                        else
                        {
                            _objPO.IsAddReceivePO = true;
                        }

                            //int RecivePOId = _objBLBills.AddEditReceivePO(_objPO);
                            int RecivePOId = _objBLBills.AddRPO(_objPO); //DONE

                        UpdateTempDateWhenCreatingNewRPO(RecivePOId.ToString());
                        UpdateDocInfo();

                        txtReception.Text = Convert.ToString(RecivePOId);
                        #endregion


                        //foreach (GridDataItem gr in selectedItems)
                        //{
                        //    Label lblOutstand = (Label)gr.FindControl("lblOutstand");
                        //    Label lblPrice = (Label)gr.FindControl("lblPrice");
                        //    TextBox txtReceive = (TextBox)gr.FindControl("txtReceive");
                        //    TextBox txtReceiveQty = (TextBox)gr.FindControl("txtReceiveQty");
                        //    Label lblLine = (Label)gr.FindControl("lblLine");
                        //    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectItem");
                        //    HiddenField hdnInvID = (HiddenField)gr.FindControl("hdnInvID");
                        //    Label lblWarehouseID = (Label)gr.FindControl("lblWarehouseID");
                        //    Label lblLocationID = (Label)gr.FindControl("lblLocationID");
                        //    DropDownList drpRecievepoIssued = (DropDownList)gr.FindControl("drpRecievepoIssued");
                        //    Label lblJobID = (Label)gr.FindControl("lblJobID");
                        //    HiddenField hdnWarehouse = (HiddenField)gr.FindControl("hdnWarehouse");
                        //    HiddenField hdnWarehouseLocationID = (HiddenField)gr.FindControl("hdnWarehouseLocationID");
                        //    Label lblPrvIn = (Label)gr.FindControl("lblPrvIn");
                        //    Label lblPrvInQuan = (Label)gr.FindControl("lblPrvInQuan");

                        //    Label lblOrderedQuan = (Label)gr.FindControl("lblOrderedQuan");
                        //    Label lblOrdered = (Label)gr.FindControl("lblOrdered");
                        //    Label lblBOQty = (Label)gr.FindControl("lblBOQty");

                        //    _objPO.POID = Convert.ToInt32(txtPO.Text);
                        //    _objPO.Line = Convert.ToInt16(lblLine.Text);
                        //    _objPO.ConnConfig = Session["config"].ToString();

                        //    double dblOutstand = 0.00;
                        //    double dblReceive = 0.00;
                        //    double dblReceiveQty = Convert.ToDouble(txtReceiveQty.Text);
                        //    double dblPrvInQuan = Convert.ToDouble(lblPrvInQuan.Text);
                        //    double dblPrvIn = ConvertCurrentCurrencyFormatToDbl(lblPrvIn.Text);

                        //    if (!Convert.ToDouble(txtReceive.Text).Equals(0))
                        //    {
                        //        dblOutstand = ConvertCurrentCurrencyFormatToDbl(lblOutstand.Text);
                        //        dblReceive = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                        //        dblPrvIn = ConvertCurrentCurrencyFormatToDbl(lblPrvIn.Text);

                        //        //IsAmount = true;
                        //    }
                        //    if (dblReceive >= ConvertCurrentCurrencyFormatToDbl(lblOrdered.Text) - dblPrvIn)
                        //    {
                        //        dblReceive = Math.Round(ConvertCurrentCurrencyFormatToDbl(lblOrdered.Text) - dblPrvIn, 4);
                        //        txtReceive.Text = Convert.ToString(dblReceive);

                        //        dblReceiveQty = Math.Round(Convert.ToDouble(lblOrderedQuan.Text) - dblPrvInQuan, 4);
                        //        txtReceiveQty.Text = Convert.ToString(dblReceiveQty);

                        //        dblOutstand = dblReceive;
                        //        lblBOQty.Text = Convert.ToString(dblReceiveQty);
                        //    }
                        //    if (dblReceiveQty >= Convert.ToDouble(lblOrderedQuan.Text) - dblPrvInQuan)
                        //    {
                        //        dblReceiveQty = Math.Round(Convert.ToDouble(lblOrderedQuan.Text) - dblPrvInQuan, 4);
                        //        txtReceiveQty.Text = Convert.ToString(dblReceiveQty);

                        //        dblReceive = Math.Round(ConvertCurrentCurrencyFormatToDbl(lblOrdered.Text) - dblPrvIn, 4);
                        //        txtReceive.Text = Convert.ToString(dblReceive);

                        //        dblOutstand = dblReceive;
                        //        lblBOQty.Text = Convert.ToString(dblReceiveQty);
                        //    }

                        //    // Start-- ES-6411 QAE- Major issue on Received PO (As per Laxmi want)
                        //    if (dblOutstand == 0 && dblReceive == 0)
                        //    {
                        //        dblOutstand = ConvertCurrentCurrencyFormatToDbl(lblOutstand.Text);
                        //        dblPrvIn = ConvertCurrentCurrencyFormatToDbl(lblPrvIn.Text);
                        //    }
                        //    // End-- ES-6411 QAE- Major issue on Received PO (As per Laxmi want)

                        //    _objPO.Balance = dblOutstand - dblReceive;
                        //    _objPO.Selected = dblReceive + dblPrvIn;
                        //    _objBLBills.UpdatePOItemBalance(_objPO);
                        //    amount = amount + dblReceive;

                        //    //_objPO.BalanceQuan = Convert.ToDouble(lblBOQty.Text) - dblReceiveQty;
                        //    //_objPO.SelectedQuan = dblPrvInQuan + dblReceiveQty;


                        //    //Commented by Azhar for ES-6363 QAE - Received PO//_objPO.SelectedQuan = Math.Round(_objPO.Selected / ConvertCurrentCurrencyFormatToDbl(lblOrdered.Text), 4);
                        //    _objPO.SelectedQuan = Math.Round((dblPrvInQuan + dblReceiveQty), 4);
                        //    _objPO.BalanceQuan = ConvertCurrentCurrencyFormatToDbl(lblOrderedQuan.Text) - _objPO.SelectedQuan;


                        //    _objBLBills.UpdatePOItemQuan(_objPO);

                        //    qty = qty + dblReceiveQty;
                        //    _objPO.ConnConfig = Session["config"].ToString();
                        //    _objPO.Quan = dblReceiveQty;
                        //    _objPO.Amount = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                        //    _objPO.Line = Convert.ToInt16(lblLine.Text);
                        //    _objPO.ReceivePOId = Convert.ToInt32(txtReception.Text);
                        //    _objPO.IsReceiveIssued = Convert.ToInt32(drpRecievepoIssued.SelectedValue);

                        //    _objBLBills.AddReceivePOItem(_objPO);

                        //    //save in IwarehouseLocAdj

                        //    //DataSet _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");
                        //    //Boolean TrackingInventory = false;
                        //    //if (_dsCustom.Tables[0].Rows.Count > 0)
                        //    //{
                        //    //    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                        //    //    {
                        //    //        if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                        //    //        {
                        //    //            TrackingInventory = Convert.ToBoolean(_dr["Label"]);
                        //    //        }
                        //    //    }
                        //    //}

                        //    #region Inventory Case
                        //    if (TrackingInventory == true)
                        //    {
                        //        if (drpRecievepoIssued.SelectedItem.Text == "Inventory" && hdnInvID.Value != "")
                        //        {
                        //            IWarehouseLocAdj invWarehouseLoc = new IWarehouseLocAdj();
                        //            invWarehouseLoc.InvID = int.Parse(hdnInvID.Value);
                        //            if (string.IsNullOrEmpty(lblJobID.Text) || lblJobID.Text == "0")
                        //            {
                        //                invWarehouseLoc.WarehouseID = lblWarehouseID.Text;
                        //                invWarehouseLoc.locationID = int.Parse(lblLocationID.Text == "" ? "0" : lblLocationID.Text);
                        //            }
                        //            else
                        //            {
                        //                invWarehouseLoc.WarehouseID = hdnWarehouse.Value;
                        //                invWarehouseLoc.locationID = int.Parse(hdnWarehouseLocationID.Value);
                        //                _objPO.WarehouseID = hdnWarehouse.Value;
                        //                _objPO.LocationID = int.Parse(hdnWarehouseLocationID.Value);
                        //                _objBLBills.UpdatePOItemWarehouseLocation(_objPO);
                        //            }

                        //            invWarehouseLoc.Hand = Convert.ToDecimal(txtReceiveQty.Text);
                        //            invWarehouseLoc.Balance = (Decimal)ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);//Convert.ToDecimal(txtReceive.Text);
                        //            invWarehouseLoc.fOrder = Convert.ToDecimal(lblBOQty.Text) - Convert.ToDecimal(txtReceiveQty.Text);

                        //            invWarehouseLoc.Committed = Convert.ToDecimal(0);
                        //            //invWarehouseLoc.Committed = Convert.ToDecimal(txtReceiveQty.Text);
                        //            ///// Commented By Azhar on 08-Mar-2020 Due to ES-3718 Available quantity is not getting increased when user perform Receive PO
                        //            //invWarehouseLoc.Available = Convert.ToDecimal(0);
                        //            invWarehouseLoc.Available = Convert.ToDecimal(txtReceiveQty.Text);
                        //            ///// Commented By Azhar on 08-Mar-2020 Due to ES-3718 Available quantity is not getting increased when user perform Receive PO

                        //            //  Trnsaction Hitting for Inventory
                        //            Transaction trans = new Transaction();
                        //            trans.ID = 0;
                        //            //trans.BatchID = 0;
                        //            trans.BatchID = BatchID;
                        //            trans.TimeStamp = System.Text.Encoding.UTF8.GetBytes(txtDate.Text);


                        //            trans.AcctSub = int.Parse(hdnInvID.Value);
                        //            trans.Acct = DefaultAcctID;
                        //            trans.Type = 41;
                        //            trans.Line = Convert.ToInt16(lblLine.Text);
                        //            trans.strRef = txtRef.Text;
                        //            trans.Status = txtReceiveQty.Text == "" ? "0" : txtReceiveQty.Text;
                        //            trans.Amount = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);//Convert.ToDouble(txtReceive.Text == "" ? "0" : txtReceive.Text);
                        //            trans.fDate = Convert.ToDateTime(txtDate.Text);
                        //            trans.fDesc = "Inventory Recieve PO";

                        //            _objInventory.CreateReceivePOInvWarehouse(invWarehouseLoc, trans);

                        //            BatchID = trans.BatchID;
                        //            Inv_Total = Inv_Total + ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                        //            //Inv_Qty = Inv_Qty + Convert.ToInt32(trans.Status);

                        //            /* ** START REVERT ENTRY FOR PO QTY **  */
                        //            InventoryWHTrans _objInventoryWHTrans = new InventoryWHTrans();
                        //            _objInventoryWHTrans.ConnConfig = Session["config"].ToString();
                        //            _objInventoryWHTrans.InvID = Convert.ToInt32(hdnInvID.Value);
                        //            _objInventoryWHTrans.WarehouseID = invWarehouseLoc.WarehouseID;
                        //            _objInventoryWHTrans.LocationID = invWarehouseLoc.locationID;
                        //            _objInventoryWHTrans.Hand = 0;
                        //            _objInventoryWHTrans.Balance = 0;
                        //            _objInventoryWHTrans.fOrder = Convert.ToDecimal(txtReceiveQty.Text) * -1;
                        //            _objInventoryWHTrans.Committed = 0;
                        //            _objInventoryWHTrans.Available = 0;
                        //            _objInventoryWHTrans.Screen = "RPO";
                        //            _objInventoryWHTrans.ScreenID = Convert.ToInt32(txtReception.Text);
                        //            _objInventoryWHTrans.Mode = "Add";
                        //            _objInventoryWHTrans.TransType = "None";
                        //            _objInventoryWHTrans.Batch = BatchID;
                        //            _objBLBills.AddReceiveInventoryWHTrans(_objInventoryWHTrans);
                        //            /* ** CLOSE REVERT ENTRY FOR PO QTY **  */

                        //            /* ** START INVENTORY ENTRY FOR RPO **  */
                        //            InventoryWHTrans obj = new InventoryWHTrans();
                        //            obj.ConnConfig = Session["config"].ToString();
                        //            obj.InvID = Convert.ToInt32(hdnInvID.Value);
                        //            obj.WarehouseID = invWarehouseLoc.WarehouseID;
                        //            obj.LocationID = invWarehouseLoc.locationID;
                        //            obj.Hand = invWarehouseLoc.Hand;
                        //            obj.Balance = invWarehouseLoc.Balance;
                        //            obj.fOrder = 0;
                        //            obj.Committed = 0;
                        //            obj.Available = 0;
                        //            obj.Screen = "RPO";
                        //            obj.ScreenID = Convert.ToInt32(txtReception.Text);
                        //            obj.Mode = "Add";
                        //            obj.TransType = "In";
                        //            obj.Batch = BatchID;

                        //            _objBLBills.AddReceiveInventoryWHTrans(obj);
                        //            /* ** END INVENTORY ENTRY FOR RPO **  */
                        //            countItem++;
                        //        }
                        //    }
                        //    #endregion Inventory Case
                        //    // Commented on 03 March 2021 as Per NK Sir told we can comment this SP because they alredy handle it in another way
                        //    //#region Update job costing
                        //    //if (!string.IsNullOrEmpty(lblJobID.Text) && lblJobID.Text != "0")
                        //    //{
                        //    //    try
                        //    //    {
                        //    //        _objPO.jobID = Convert.ToInt32(lblJobID.Text);
                        //    //        _objBLBills.UpdateJobComm(_objPO);
                        //    //    }
                        //    //    catch (Exception)
                        //    //    {
                        //    //        // Do nothing
                        //    //    }

                        //    //}
                        //    //#endregion Update job costing
                        //}
                        //if (countItem > 0)
                        //{
                        //    Transaction objtrans = new Transaction();
                        //    objtrans.ID = 0;
                        //    objtrans.BatchID = BatchID;
                        //    objtrans.TimeStamp = System.Text.Encoding.UTF8.GetBytes(txtDate.Text);
                        //    objtrans.strRef = txtRef.Text;
                        //    objtrans.Status = "0";
                        //    objtrans.Amount = Inv_Total;//Convert.ToDouble(txtReceive.Text == "" ? "0" : txtReceive.Text);
                        //    objtrans.fDate = Convert.ToDateTime(txtDate.Text);
                        //    objtrans.fDesc = "Inventory Recieve PO";
                        //    _objInventory.CreateReceivePOInvWarehouseTrans(objtrans);
                        //}
                        //#endregion

                        ////#region Add Reception PO

                        ////_objPO.ConnConfig = Session["config"].ToString();
                        ////_objPO.RID = Convert.ToInt32(txtReception.Text);
                        ////_objPO.POID = Convert.ToInt32(txtPO.Text);
                        ////_objPO.Ref = txtRef.Text;
                        ////_objPO.WB = txtTrkWB.Text;
                        ////_objPO.Comments = txtRcomments.Text;
                        ////_objPO.Amount = amount;
                        ////_objPO.fDate = Convert.ToDateTime(txtDate.Text);
                        ////_objPO.BatchID = BatchID;
                        ////_objPO.IsReceiveIssued = 1;
                        ////_objBLBills.AddReceivePO(_objPO);
                        ////// _objPO.ConnConfig = Session["config"].ToString();
                        //////_objPO.POID = Convert.ToInt32(txtPO.Text);
                        //////_objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                        ////_objPO.MOMUSer = Session["User"].ToString();
                        ////int RecivePOId = _objBLBills.AddEditReceivePO(_objPO);
                        //////_objBLBills.UpdatePODue(_objPO);
                        ////#endregion

                        //#region Update Reception PO Amount & Batch
                        //_objPO.ConnConfig = Session["config"].ToString();
                        //_objPO.RID = Convert.ToInt32(txtReception.Text);
                        //_objPO.POID = Convert.ToInt32(txtPO.Text);
                        //_objPO.Ref = txtRef.Text;
                        //_objPO.WB = txtTrkWB.Text;
                        //_objPO.Comments = txtRcomments.Text;
                        //_objPO.Amount = amount;
                        //_objPO.fDate = Convert.ToDateTime(txtDate.Text);
                        //_objPO.BatchID = BatchID;
                        //_objPO.IsReceiveIssued = 1;
                        //_objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                        //_objPO.MOMUSer = Session["User"].ToString();
                        //_objBLBills.UpdtReceivePOAmnt(_objPO);                        
                        //#endregion

                        ////#region Update job costing
                        ////foreach (GridDataItem gr in selectedItems)
                        ////{
                        ////    Label lblJobID = (Label)gr.FindControl("lblJobID");
                        ////    if (!string.IsNullOrEmpty(lblJobID.Text) && lblJobID.Text != "0")
                        ////    {
                        ////        try
                        ////        {
                        ////            _objPO.jobID = Convert.ToInt32(lblJobID.Text);
                        ////         _objBLBills.UpdateJobComm(_objPO);
                        ////        }
                        ////        catch (Exception)
                        ////        {
                        ////            // Do nothing
                        ////        }

                        ////    }
                        ////}
                        ////#endregion

                        ////#region Update PO status
                        ////_objBLBills.AutoUpdatePOStatus(_objPO);
                        ////#endregion

                        Session["RPORefNO"] = txtReception.Text;
                        Response.Redirect(Request.RawUrl, false);
                        //if (!(Request.RawUrl).Contains("RefNO"))
                        //{
                        //    Response.Redirect(Request.RawUrl + "?RefNO="+ txtReception.Text + "", false);
                        //}
                        //else
                        //{
                        //    Response.Redirect(Request.RawUrl, false);
                        //}


                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: 'Ref number already exists with this vendor, Please use different Ref number!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        //}
                        //}
                        //else
                        //{
                        //    var dialog = "noty({text: \'" + errMessage + "\',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});";
                        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", dialog, true);
                        //}
                       
                    }
                    else // there are no item selected
                    {
                        if (strErrorMessage.Trim() != "")
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: '"+ strErrorMessage + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'You must fill out atleast one line.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }

                }
                else
                {
                    //_objBLBills.ReverseReceivePOInvetoryItem(Convert.ToInt32(Request.QueryString["id"]), Session["config"].ToString(), Session["UserID"].ToString());
                    var selectedItems = GetUnReceivedSelectedItems("EDIT");

                    if (selectedItems.Count > 0)
                    {
                        //string errMessage = ValidateSelectedItems(selectedItems);
                        //if (string.IsNullOrEmpty(errMessage))
                        //{
                            ///
                            ///===> TO Avoid Violation primary Key Error , Check Reception No # Already not Exists in DB. 
                            ///
                            //_objPO.ConnConfig = Session["config"].ToString();
                            //_objPO.Ref = txtRef.Text;
                            //IsExist = _objBLBills.IsExistRPOForInsert(_objPO);

                            //if (IsExist.Equals(false))
                            //{
                            double amount = 0;
                            //double poAmount = 0;
                            //double poQty = 0;
                            double qty = 0;

                            // Get poAmount and poQty: need to check all item in Grid
                            // TODO: this should get from database is better.
                            // Will do it later
                            //foreach (GridDataItem gv in RadGrid_POItems.Items)
                            //{
                            //    Label lblBOQty = (Label)gv.FindControl("lblBOQty");
                            //    Label lblOutstand = (Label)gv.FindControl("lblOutstand");
                            //    poQty = poQty + Convert.ToDouble(lblBOQty.Text);
                            //    poAmount = poAmount + ConvertCurrentCurrencyFormatToDbl(lblOutstand.Text);
                            //}

                            //bool IsAmount = false;
                            int countItem = 0;
                            int BatchID = 0;
                            double Inv_Total_Edit = 0;
                            Boolean TrackingInventory = hdnIsInventoryTrackingIsOn.Value == "1" ? true : false;

                        DataTable dtRPO = (DataTable)ViewState["dtRPOItem"];
                        #region Edit Reception PO
                        _objPO.ConnConfig = Session["config"].ToString();
                        _objPO.RID = Convert.ToInt32(Request.QueryString["id"].ToString());
                        _objPO.POID = Convert.ToInt32(txtPO.Text);
                        _objPO.Ref = txtRef.Text;
                        _objPO.WB = txtTrkWB.Text;
                        _objPO.Comments = txtRcomments.Text;
                        _objPO.Amount = amount;
                        _objPO.fDate = Convert.ToDateTime(txtDate.Text);
                        _objPO.BatchID = BatchID;
                        _objPO.IsReceiveIssued = 1;
                        _objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                        _objPO.MOMUSer = Session["User"].ToString();
                        _objPO.Dt = dtRPO;
                        _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
                        //int RecivePOId = _objBLBills.AddEditReceivePO(_objPO);
                        int RecivePOId = _objBLBills.EditRPO(_objPO);
                        UpdateTempDateWhenCreatingNewRPO(RecivePOId.ToString());
                        UpdateDocInfo();

                        txtReception.Text = Convert.ToString(RecivePOId);
                        #endregion

                        //#region Update PO balance
                        //foreach (GridDataItem gr in selectedItems)
                        //    {
                        //        Label lblOutstand = (Label)gr.FindControl("lblOutstand");
                        //        Label lblPrice = (Label)gr.FindControl("lblPrice");
                        //        TextBox txtReceive = (TextBox)gr.FindControl("txtReceive");
                        //        TextBox txtReceiveQty = (TextBox)gr.FindControl("txtReceiveQty");
                        //        HiddenField hdnReceive = (HiddenField)gr.FindControl("hdnReceive");
                        //        HiddenField hdnReceiveQty = (HiddenField)gr.FindControl("hdnReceiveQty");
                        //        Label lblLine = (Label)gr.FindControl("lblLine");
                        //        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectItem");
                        //        HiddenField hdnInvID = (HiddenField)gr.FindControl("hdnInvID");
                        //        Label lblWarehouseID = (Label)gr.FindControl("lblWarehouseID");
                        //        Label lblLocationID = (Label)gr.FindControl("lblLocationID");
                        //        DropDownList drpRecievepoIssued = (DropDownList)gr.FindControl("drpRecievepoIssued");
                        //        Label lblJobID = (Label)gr.FindControl("lblJobID");
                        //        HiddenField hdnWarehouse = (HiddenField)gr.FindControl("hdnWarehouse");
                        //        HiddenField hdnWarehouseLocationID = (HiddenField)gr.FindControl("hdnWarehouseLocationID");
                        //        Label lblPrvIn = (Label)gr.FindControl("lblPrvIn");
                        //        Label lblPrvInQuan = (Label)gr.FindControl("lblPrvInQuan");

                        //        Label lblOrderedQuan = (Label)gr.FindControl("lblOrderedQuan");
                        //        Label lblOrdered = (Label)gr.FindControl("lblOrdered");
                        //        //Label lblBOQty = (Label)gr.FindControl("lblBOQty");

                        //        _objPO.POID = Convert.ToInt32(txtPO.Text);
                        //        _objPO.Line = Convert.ToInt16(lblLine.Text);
                        //        //_objPO.ConnConfig = Session["config"].ToString();

                        //        if (!Convert.ToDouble(txtReceive.Text).Equals(0))
                        //        {
                        //            var dblOutstand = ConvertCurrentCurrencyFormatToDbl(lblOutstand.Text);
                        //            var dblReceive = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                        //            var dblPrvIn = ConvertCurrentCurrencyFormatToDbl(lblPrvIn.Text);
                        //            var dblReceiveOrg = Convert.ToDouble(hdnReceive.Value);
                        //            _objPO.Balance = dblOutstand + dblReceiveOrg - dblReceive;

                        //            _objPO.Selected = dblPrvIn - dblReceiveOrg + dblReceive;

                        //            _objBLBills.UpdatePOItemBalance(_objPO);

                        //            amount = amount + dblReceive;
                        //            //IsAmount = true;
                        //        }
                        //        Label lblBOQty = (Label)gr.FindControl("lblBOQty");
                        //        var dblReceiveQty = Convert.ToDouble(txtReceiveQty.Text);
                        //        var dblPrvInQuan = Convert.ToDouble(lblPrvInQuan.Text);
                        //        var dblReceiveQtyOrg = Convert.ToDouble(hdnReceiveQty.Value);

                        //        _objPO.BalanceQuan = Convert.ToDouble(lblBOQty.Text) + dblReceiveQtyOrg - dblReceiveQty;
                        //        _objPO.SelectedQuan = Math.Round((dblPrvInQuan - dblReceiveQtyOrg + dblReceiveQty),4);

                        //        //_objPO.SelectedQuan = Math.Round(_objPO.Selected / ConvertCurrentCurrencyFormatToDbl(lblOrdered.Text), 4);
                        //        //_objPO.BalanceQuan = ConvertCurrentCurrencyFormatToDbl(lblOrderedQuan.Text) - _objPO.SelectedQuan;
                        //        _objBLBills.UpdatePOItemQuan(_objPO);

                        //        qty = qty + dblReceiveQty;
                        //        //_objPO.ConnConfig = Session["config"].ToString();
                        //        _objPO.Quan = dblReceiveQty;
                        //        _objPO.Amount = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                        //        _objPO.Line = Convert.ToInt16(lblLine.Text);
                        //        _objPO.ReceivePOId = Convert.ToInt32(txtReception.Text);
                        //        _objPO.IsReceiveIssued = Convert.ToInt32(drpRecievepoIssued.SelectedValue);

                        //        _objBLBills.UpdateReceivePOItem(_objPO);

                        //        //save in IwarehouseLocAdj
                        //        General _objPropGeneral = new General();
                        //        BL_General _objBLGeneral = new BL_General();

                        //        _objPropGeneral.ConnConfig = Session["config"].ToString();
                        //        //DataSet _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");
                        //        DataSet _dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);
                        //        //Boolean TrackingInventory = false;
                        //        int DefaultAcctID = 0;
                        //        //if (_dsCustom.Tables[0].Rows.Count > 0)
                        //        //{
                        //        //    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                        //        //    {
                        //        //        if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                        //        //        {
                        //        //            TrackingInventory = Convert.ToBoolean(_dr["Label"]);
                        //        //        }
                        //        //    }
                        //        //}

                        //        if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
                        //        {
                        //            DefaultAcctID = Convert.ToInt32(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
                        //        }

                        //        // TODO: Thomas need to check and update in case of Inventory
                        //        #region Inventory Case
                        //        if (TrackingInventory == true)
                        //        {
                        //            if (drpRecievepoIssued.SelectedItem.Text == "Inventory" && hdnInvID.Value != "")
                        //            {
                        //                IWarehouseLocAdj invWarehouseLoc = new IWarehouseLocAdj();
                        //                invWarehouseLoc.InvID = int.Parse(hdnInvID.Value);
                        //                if (string.IsNullOrEmpty(lblJobID.Text) || lblJobID.Text == "0")
                        //                {
                        //                    invWarehouseLoc.WarehouseID = lblWarehouseID.Text;
                        //                    invWarehouseLoc.locationID = int.Parse(lblLocationID.Text == "" ? "0" : lblLocationID.Text);
                        //                }
                        //                else
                        //                {
                        //                    invWarehouseLoc.WarehouseID = hdnWarehouse.Value;
                        //                    invWarehouseLoc.locationID = int.Parse(hdnWarehouseLocationID.Value);
                        //                    _objPO.WarehouseID = hdnWarehouse.Value;
                        //                    _objPO.LocationID = int.Parse(hdnWarehouseLocationID.Value);
                        //                    _objBLBills.UpdatePOItemWarehouseLocation(_objPO);
                        //                }

                        //                invWarehouseLoc.Hand = Convert.ToDecimal(txtReceiveQty.Text);
                        //                invWarehouseLoc.Balance = (Decimal)ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);//Convert.ToDecimal(txtReceive.Text);
                        //                invWarehouseLoc.fOrder = Convert.ToDecimal(lblBOQty.Text) - Convert.ToDecimal(txtReceiveQty.Text);
                        //                invWarehouseLoc.Committed = Convert.ToDecimal(0);
                        //                //invWarehouseLoc.Committed = Convert.ToDecimal(txtReceiveQty.Text);
                        //                invWarehouseLoc.Available = Convert.ToDecimal(0);

                        //                //  Trnsaction Hitting for Inventory
                        //                Transaction trans = new Transaction();
                        //                trans.ID = 0;
                        //                //trans.BatchID = 0;
                        //                trans.BatchID = BatchID;
                        //                trans.TimeStamp = System.Text.Encoding.UTF8.GetBytes(txtDate.Text);
                        //                trans.AcctSub = int.Parse(hdnInvID.Value);
                        //                trans.Acct = DefaultAcctID;
                        //                trans.Type = 41;
                        //                trans.Line = Convert.ToInt16(lblLine.Text);
                        //                trans.strRef = txtRef.Text;
                        //                trans.Status = txtReceiveQty.Text == "" ? "0" : txtReceiveQty.Text;
                        //                trans.Amount = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);//Convert.ToDouble(txtReceive.Text == "" ? "0" : txtReceive.Text);
                        //                trans.fDate = Convert.ToDateTime(txtDate.Text);
                        //                trans.fDesc = "Inventory Recieve PO";

                        // _objInventory.CreateReceivePOInvWarehouse(invWarehouseLoc, trans);

                        //                BatchID = trans.BatchID;
                        //                Inv_Total_Edit = Inv_Total_Edit + ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                        //                //Inv_Qty = Inv_Qty + Convert.ToInt32(trans.Status);


                        //                ////  Trnsaction Hitting for Inventory
                        //                //Transaction trans = new Transaction();
                        //                //trans.ID = 0;
                        //                //trans.BatchID = 0;
                        //                //trans.TimeStamp = System.Text.Encoding.UTF8.GetBytes(txtDate.Text);
                        //                //trans.AcctSub = int.Parse(hdnInvID.Value);
                        //                //trans.Acct = DefaultAcctID;
                        //                //trans.Type = 41;
                        //                //trans.Line = Convert.ToInt16(lblLine.Text);
                        //                //trans.strRef = txtRef.Text;
                        //                //trans.Status = txtReceiveQty.Text == "" ? "0" : txtReceiveQty.Text;
                        //                //trans.Amount = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);//Convert.ToDouble(txtReceive.Text == "" ? "0" : txtReceive.Text);
                        //                //trans.fDate = Convert.ToDateTime(txtDate.Text);
                        //                //trans.fDesc = "Inventory Recieve PO";
                        //_objInventory.CreateReceivePOInvWarehouse(invWarehouseLoc, trans);
                        //                //BatchID = trans.BatchID;

                        //                /* ** START REVERT ENTRY FOR PO QTY **  */
                        //                InventoryWHTrans _objInventoryWHTrans = new InventoryWHTrans();
                        //                _objInventoryWHTrans.ConnConfig = Session["config"].ToString();
                        //                _objInventoryWHTrans.InvID = Convert.ToInt32(hdnInvID.Value);
                        //                _objInventoryWHTrans.WarehouseID = invWarehouseLoc.WarehouseID;
                        //                _objInventoryWHTrans.LocationID = invWarehouseLoc.locationID;
                        //                _objInventoryWHTrans.Hand = 0;
                        //                _objInventoryWHTrans.Balance = 0;
                        //                _objInventoryWHTrans.fOrder = Convert.ToDecimal(txtReceiveQty.Text) * -1;
                        //                _objInventoryWHTrans.Committed = 0;
                        //                _objInventoryWHTrans.Available = 0;
                        //                _objInventoryWHTrans.Screen = "RPO";
                        //                _objInventoryWHTrans.ScreenID = Convert.ToInt32(txtReception.Text);
                        //                _objInventoryWHTrans.Mode = "Add";
                        //                _objInventoryWHTrans.TransType = "None";
                        //                _objInventoryWHTrans.Batch = BatchID;
                        //                _objBLBills.AddReceiveInventoryWHTrans(_objInventoryWHTrans);
                        //                /* ** CLOSE REVERT ENTRY FOR PO QTY **  */

                        //                InventoryWHTrans obj = new InventoryWHTrans();
                        //                obj.ConnConfig = Session["config"].ToString();
                        //                obj.InvID = Convert.ToInt32(hdnInvID.Value);
                        //                obj.WarehouseID = invWarehouseLoc.WarehouseID;
                        //                obj.LocationID = invWarehouseLoc.locationID;
                        //                obj.fOrder = 0;
                        //                obj.Committed = 0;
                        //                obj.Available = 0;
                        //                obj.Screen = "RPO";
                        //                obj.ScreenID = Convert.ToInt32(txtReception.Text);
                        //                obj.Hand = invWarehouseLoc.Hand;
                        //                obj.Balance = invWarehouseLoc.Balance;
                        //                obj.Mode = "Edit";
                        //                obj.TransType = "In";
                        //                obj.Batch = trans.BatchID;
                        //                BatchID = trans.BatchID;
                        //                _objBLBills.AddReceiveInventoryWHTrans(obj);

                        //                countItem++;
                        //            }
                        //        }
                        //    #endregion Inventory Case
                        //    #region Update job costing

                        //    // Commented on 03 March 2021 as Per NK Sir told we can comment this SP because they alredy handle it in another way
                        //    //if (!string.IsNullOrEmpty(lblJobID.Text) && lblJobID.Text != "0")
                        //    //    {
                        //    //        try
                        //    //        {
                        //    //            _objPO.jobID = Convert.ToInt32(lblJobID.Text);
                        //    //            _objBLBills.UpdateJobComm(_objPO);
                        //    //        }
                        //    //        catch (Exception)
                        //    //        {
                        //    //            // Do nothing
                        //    //        }

                        //    //    }

                        //        #endregion
                        //        //countItem++;
                        //    }
                        //    #endregion
                        //    if (countItem > 0)
                        //    {
                        //        Transaction objtrans = new Transaction();
                        //        objtrans.ID = 0;
                        //        objtrans.BatchID = BatchID;
                        //        objtrans.TimeStamp = System.Text.Encoding.UTF8.GetBytes(txtDate.Text);
                        //        objtrans.strRef = txtRef.Text;
                        //        objtrans.Status = "0";
                        //        objtrans.Amount = Inv_Total_Edit;//Convert.ToDouble(txtReceive.Text == "" ? "0" : txtReceive.Text);
                        //        objtrans.fDate = Convert.ToDateTime(txtDate.Text);
                        //        objtrans.fDesc = "Inventory Recieve PO";
                        //        _objInventory.CreateReceivePOInvWarehouseTrans(objtrans);
                        //    }

                        //    #region Add Reception PO

                        //    _objPO.ConnConfig = Session["config"].ToString();
                        //    _objPO.RID = Convert.ToInt32(txtReception.Text);
                        //    _objPO.POID = Convert.ToInt32(txtPO.Text);
                        //    _objPO.Ref = txtRef.Text;
                        //    _objPO.WB = txtTrkWB.Text;
                        //    _objPO.Comments = txtRcomments.Text;
                        //    _objPO.Amount = amount;
                        //    _objPO.fDate = Convert.ToDateTime(txtDate.Text);
                        //    _objPO.BatchID = BatchID;
                        //    //_objBLBills.AddReceivePO(_objPO);
                        //    // _objPO.ConnConfig = Session["config"].ToString();
                        //    //_objPO.POID = Convert.ToInt32(txtPO.Text);
                        //    _objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                        //    _objPO.MOMUSer = Session["User"].ToString();
                        //    _objBLBills.UpdateReceivePO(_objPO);
                        //    //_objBLBills.UpdatePODue(_objPO);
                        //    #endregion

                        //    //#region Update job costing
                        //    //foreach (GridDataItem gr in selectedItems)
                        //    //{
                        //    //    Label lblJobID = (Label)gr.FindControl("lblJobID");
                        //    //    if (!string.IsNullOrEmpty(lblJobID.Text) && lblJobID.Text != "0")
                        //    //    {
                        //    //        try
                        //    //        {
                        //    //            _objPO.jobID = Convert.ToInt32(lblJobID.Text);
                        //    //            _objBLBills.UpdateJobComm(_objPO);
                        //    //        }
                        //    //        catch (Exception)
                        //    //        {
                        //    //            // Do nothing
                        //    //        }

                        //    //    }
                        //    //}
                        //    //#endregion

                        //    #region Update PO status
                        //    //_objBLBills.AutoUpdatePOStatus(_objPO);
                        //    #endregion

                        Session["RPORefNO"] = txtReception.Text;
                        Response.Redirect(Request.RawUrl, false);
                        //if (!(Request.RawUrl).Contains("RefNO"))
                        //{
                        //    Response.Redirect(Request.RawUrl + "&RefNO="+ txtReception.Text + "", false);
                        //}
                        //else
                        //{
                        //    Response.Redirect(Request.RawUrl, false);
                        //}

                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: 'Ref number already exists with this vendor, Please use different Ref number!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        //}
                        //}
                        //else
                        //{
                        //    var dialog = "noty({text: \'" + errMessage + "\',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});";
                        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", dialog, true);
                        //}
                        
                           
                    }
                        else // there are no item selected
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'You must fill out atleast one line.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }

                    //_objPO.ConnConfig = Session["config"].ToString();
                    //_objPO.RID = Convert.ToInt32(txtReception.Text);
                    //_objPO.POID = Convert.ToInt32(txtPO.Text);
                    //_objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                    //_objPO.MOMUSer = Session["User"].ToString();
                    //_objBLBills.UpdateReceivePODue(_objPO);

                    //if (!(Request.RawUrl).Contains("RefNO"))
                    //{
                    //    Response.Redirect(Request.RawUrl + "&RefNO=1", false);
                    //}
                    //else
                    //{
                    //    Response.Redirect(Request.RawUrl, false);
                    //}
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'This month/year period is closed out. You do not have permission to add/update this record.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);                
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private double ConvertCurrentCurrencyFormatToDbl(string strCurrency)
    {
        if (!string.IsNullOrEmpty(strCurrency))
        {
            var dblReturn = double.Parse(strCurrency.Replace('$', '0'), NumberStyles.AllowParentheses |
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

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["pid"] != null)
            {
                Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["pid"].ToString() + "&tab=budget");
            }
        }
        else
        {
            Response.Redirect("managereceivepo.aspx");
        }
    }

    //protected void btnSelectVendor_Click(object sender, EventArgs e)
    //{
    //    FillAddress();

    //    try
    //    {
    //        if (!string.IsNullOrEmpty(hdnVendorID.Value))
    //        {
    //            DataSet ds = new DataSet();
    //            _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
    //            _objPO.ConnConfig = Session["config"].ToString();
    //            _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);
    //            if (Session["CmpChkDefault"].ToString() == "1")
    //            {
    //                _objPO.EN = 1;
    //            }
    //            else
    //            {
    //                _objPO.EN = 0;
    //            }
    //            // ds = _objBLBills.GetPOByVendor(_objPO);
    //            ds = GetPOByVendor(_objPO);

    //            ViewState["PO"] = ds.Tables[0];
    //            //gvPO.DataSource = ds.Tables[0];
    //            //gvPO.DataBind();
    //            Session["dsPOData"] = ds.Tables[0];
    //            RadGrid_ReceivePO.Rebind();
    //            //RadGrid_ReceivePO.DataSource = ds.Tables[0];
    //            //RadGrid_ReceivePO.DataBind();

    //            //gvPOItems.DataSource = ds.Tables[1];
    //            //gvPOItems.DataBind();

    //            //RadGrid_POItems.DataSource = ds.Tables[1];
    //            //RadGrid_POItems.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

    //protected void txtVendor_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(hdnVendorID.Value))
    //        {
    //            DataSet ds = new DataSet();
    //            _objPO.ConnConfig = Session["config"].ToString();
    //            _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);

    //            // ds = _objBLBills.GetPOByVendor(_objPO);
    //            ds = GetPOByVendor(_objPO);

    //            ViewState["PO"] = ds.Tables[0];
    //            //gvPO.DataSource = ds.Tables[0];
    //            //gvPO.DataBind();
    //            Session["dsPOData"] = ds.Tables[0];
    //            RadGrid_ReceivePO.Rebind();
    //            //RadGrid_ReceivePO.DataSource = ds.Tables[0];
    //            //RadGrid_ReceivePO.DataBind();

    //            //gvPOItems.DataSource = ds.Tables[1];
    //            //gvPOItems.DataBind();

    //            //RadGrid_POItems.DataSource = ds.Tables[1];
    //            //RadGrid_POItems.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

    //protected void chkSelect_CheckedChanged1(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CheckBox chkSelect = (CheckBox)sender;
    //        GridViewRow row = (GridViewRow)chkSelect.NamingContainer;
    //        Label lblOrdered = (Label)row.FindControl("lblOrdered");
    //        Label lblPrvIn = (Label)row.FindControl("lblPrvIn");
    //        Label lblOutstand = (Label)row.FindControl("lblOutstand");
    //        TextBox txtReceive = (TextBox)row.FindControl("txtReceive");
    //        TextBox txtReceiveQty = (TextBox)row.FindControl("txtReceiveQty");
    //        bool IsAmount = false;

    //        if(!string.IsNullOrEmpty(txtReceiveQty.Text))
    //        {
    //            if(Convert.ToDouble(txtReceiveQty.Text).Equals(0))
    //            {
    //                IsAmount = true;
    //            }
    //        }
    //        else
    //            IsAmount = true;

    //        if(IsAmount)
    //        {
    //            double _dueBalance = double.Parse(lblOutstand.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                                               NumberStyles.AllowThousands |
    //                                               NumberStyles.AllowDecimalPoint);

    //            txtReceive.Text = _dueBalance.ToString("0.00", CultureInfo.InvariantCulture);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

    protected void btnSelectPO_Click(object sender, EventArgs e)
    {
        try
        {
            //if (!string.IsNullOrEmpty(hdnVendorID.Value))
            //{
            //    _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
            //    if (Session["CmpChkDefault"].ToString() == "1")
            //    {
            //        _objPO.EN = 1;
            //    }
            //    else
            //    {
            //        _objPO.EN = 0;
            //    }
            //    DataSet ds = new DataSet();
            //    _objPO.ConnConfig = Session["config"].ToString();
            //    _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);

            //    // ds = _objBLBills.GetPOByVendor(_objPO);
            //    //ds = GetPOByVendor(_objPO);
            //    ds = _objBLBills.GetAllPOByDue(_objPO);

            //    ViewState["PO"] = ds.Tables[0];
            //    //gvPO.DataSource = ds.Tables[0];
            //    //gvPO.DataBind();
            //    Session["dsPOData"] = ds.Tables[0];
            //    RadGrid_ReceivePO.Rebind();



            //    //RadGrid_ReceivePO.DataSource = ds.Tables[0];
            //    //RadGrid_ReceivePO.DataBind();

            //}

            //RadGrid_ReceivePO.Rebind();
            GetAllPOByVendor();
            _objPO.ConnConfig = Session["config"].ToString();

            if (!string.IsNullOrEmpty(txtPO.Text))
            {
                _objPO.POID = Convert.ToInt32(txtPO.Text);

                if (RadGrid_ReceivePO.SelectedItems.Count > 0)
                {
                    
                    _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
                    if (Session["CmpChkDefault"].ToString() == "1")
                    {
                        _objPO.EN = 1;
                    }
                    else
                    {
                        _objPO.EN = 0;
                    }
                    DataTable _dtPO = _objBLBills.spGetOpenPODetailforRPO(_objPO);
                    //DataTable _dtPO = new DataTable();
                    if (_dtPO.Rows.Count > 0)
                    {
                        txtVendor.Text = _dtPO.Rows[0]["VendorName"].ToString();
                        txtVendorType.Text = _dtPO.Rows[0]["VendorType"].ToString();
                        hdnVendorID.Value = _dtPO.Rows[0]["Vendor"].ToString(); 
                        txtAddress.Text = _dtPO.Rows[0]["Address"].ToString(); 
                        txtPO.Text = _dtPO.Rows[0]["PO"].ToString(); 
                        txtDueDate.Text = _dtPO.Rows[0]["Due"].ToString(); 
                        hdnAmount.Value = _dtPO.Rows[0]["Amount"].ToString(); 
                        txtComments.Text = _dtPO.Rows[0]["fDesc"].ToString();
                        txtVendor.Enabled = false;
                        string status = "New";
                        switch (Convert.ToInt16(_dtPO.Rows[0]["Status"].ToString()))
                        {
                            case 0:
                                status = "New";
                                break;
                            case 1:
                                status = "Closed";
                                break;
                            case 2:
                                status = "Cancelled";
                                break;
                            case 3:
                                status = "Partial-Quantity";
                                break;
                            case 4:
                                status = "Partial-Amount";
                                break;
                        }
                        txtStatus.Text = status;
                    }


                    DataSet dsItem = _objBLBills.GetPOItemByPO(_objPO);
                    BINDRadGrid_POItems(dsItem.Tables[0]);
                }
                else
                {
                    RadGrid_POItems.DataSource = string.Empty;
                    RadGrid_POItems.DataBind();
                    txtVendor.Text = "";
                    txtVendorType.Text = "";
                    txtAddress.Text = "";
                    txtPO.Text = "";
                    txtDueDate.Text = "";
                    txtComments.Text = "";
                    txtStatus.Text = "";
                }
            }
            else
            {
                RadGrid_POItems.DataSource = string.Empty;
                RadGrid_POItems.DataBind();
                txtVendor.Text = "";
                txtVendorType.Text = "";
                txtAddress.Text = "";
                txtPO.Text = "";
                txtDueDate.Text = "";
                txtComments.Text = "";
                txtStatus.Text = "";
            }

            //else
            //{
            //    var po = _objBLBills.GetPOByIdAjax(_objPO);
            //    if(po!= null && po.Tables.Count > 0)
            //    {
            //        var data = po.Tables[0];

            //        foreach (DataRow item in data.Rows)
            //        {
            //            txtVendor.Text = item["VendorName"].ToString();
            //            hdnVendorID.Value = item["Vendor"].ToString();
            //            txtAddress.Text = item["Address"].ToString();
            //            //txtPO.Text = item["PO"].ToString();
            //            txtDueDate.Text = item["Due"].ToString();
            //            hdnAmount.Value = item["Amount"].ToString();
            //            txtComments.Text = item["fDesc"].ToString();
            //            txtStatus.Text = item["StatusName"].ToString();
            //        }
            //    }
            //}


            ////  DataSet dsItem = _objBLBills.GetPOItemByPO(_objPO);

            //DataSet dsItem = GetPOItemByPO(_objPO);

            ////gvPOItems.DataSource = dsItem.Tables[0];
            ////gvPOItems.DataBind();

            //BINDRadGrid_POItems(dsItem.Tables[0]);



        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    private void BINDRadGrid_POItems(DataTable dt)
    {

        RadGrid_POItems.DataSource = dt;
        RadGrid_POItems.DataBind();

        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();

        _objPropGeneral.ConnConfig = Session["config"].ToString();
        Boolean TrackingInventory = hdnIsInventoryTrackingIsOn.Value == "1" ? true : false;
        //DataSet _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");
        //if (_dsCustom.Tables[0].Rows.Count > 0)
        //{
        //    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
        //    {
        //        if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
        //        {
        //            TrackingInventory = Convert.ToBoolean(_dr["Label"]);
        //        }
        //    }
        //}

        if (TrackingInventory == false)
        {
            RadGrid_POItems.Columns[17].Visible = false;
            RadGrid_POItems.Columns[18].Visible = false;
        }
        else
        {
            RadGrid_POItems.Columns[17].Visible = true;
            RadGrid_POItems.Columns[18].Visible = true;
        }

        //foreach (GridViewRow gr in gvPOItems.Rows)
        foreach (GridDataItem gr in RadGrid_POItems.Items)
        {

            Label lblJobID = (Label)gr.FindControl("lblJobID");
            HiddenField hdnInvID = (HiddenField)gr.FindControl("hdnInvID");
            String InvID = hdnInvID.Value;
            String jobID = lblJobID.Text;
            DropDownList drpRecievepoIssued = (DropDownList)gr.FindControl("drpRecievepoIssued");

            //  if ((string.IsNullOrEmpty(jobID) || jobID == "0") && (string.IsNullOrEmpty(InvID) || InvID == "0"))
            if (TrackingInventory)
            {
                Label lblWarehouseID = (Label)gr.FindControl("lblWarehouseID");
                if (string.IsNullOrEmpty(jobID) || jobID == "0")
                {
                    TextBox txtGvWarehouse = (TextBox)gr.FindControl("txtGvWarehouse");
                    TextBox txtGvWarehouseLocation = (TextBox)gr.FindControl("txtGvWarehouseLocation");
                    txtGvWarehouse.Visible = false;
                    txtGvWarehouseLocation.Visible = false;
                }
            }

            //////////// DDL ReceiveIssued Condition

            HiddenField hdnIsReceiveIssued = (HiddenField)gr.FindControl("hdnIsReceiveIssued");
            //HiddenField hdnIsInventoryTrackingIsOn = (HiddenField)gr.FindControl("hdnIsInventoryTrackingIsOn");
            HiddenField hdnIsProjectSelect = (HiddenField)gr.FindControl("hdnIsProjectSelect");
            //HiddenField hdnIsItemsExistsInInventory = (HiddenField)gr.FindControl("hdnIsItemsExistsInInventory");
            HiddenField hdnIsInventoryCode = (HiddenField)gr.FindControl("hdnIsInventoryCode");


            //if (TrackingInventory == false) { hdnIsInventoryTrackingIsOn.Value = "0"; } else { hdnIsInventoryTrackingIsOn.Value = "1"; }
            int hdnjobID = 0;
            int.TryParse(lblJobID.Text, out hdnjobID);

            if (hdnjobID > 0)
            {

                hdnIsProjectSelect.Value = "1";
                if (hdnIsInventoryCode.Value == "Inventory")
                {
                    hdnIsReceiveIssued.Value = drpRecievepoIssued.SelectedValue = "2";
                }
                else
                {
                    hdnIsReceiveIssued.Value = drpRecievepoIssued.SelectedValue = "1";
                }
            }
            else
            {
                hdnIsProjectSelect.Value = "0";

                if (TrackingInventory == true)
                {
                    if (hdnIsInventoryCode.Value == "Inventory")
                    {
                        hdnIsReceiveIssued.Value = drpRecievepoIssued.SelectedValue = "2";
                    }
                    else
                    {
                        hdnIsReceiveIssued.Value = drpRecievepoIssued.SelectedValue = "0";
                    }
                }
                else
                {
                    hdnIsReceiveIssued.Value = drpRecievepoIssued.SelectedValue = "0";
                }
            }
            /////////////////

        }
    }

    //protected void txtReceive_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        TextBox txtReceive = (TextBox)sender;
    //        GridViewRow row = (GridViewRow)txtReceive.NamingContainer;
    //        TextBox txtReceiveQty = (TextBox)row.FindControl("txtReceiveQty");
    //        Label lblOutstand = (Label)row.FindControl("lblOutstand");
    //        CheckBox chk = (CheckBox)row.FindControl("chkSelectItem");

    //        if (!string.IsNullOrEmpty(txtReceive.Text))
    //        {
    //            if (Convert.ToDouble(txtReceive.Text) > 0)
    //            {
    //                txtReceiveQty.Text = "0.00";
    //                double outstandVal = double.Parse(lblOutstand.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                              NumberStyles.AllowThousands |
    //                              NumberStyles.AllowDecimalPoint);
    //                if (Convert.ToDouble(txtReceive.Text) > outstandVal)
    //                {
    //                    txtReceive.Text = outstandVal.ToString("0.00", CultureInfo.InvariantCulture);
    //                }

    //                foreach(GridViewRow gr in gvPOItems.Rows)
    //                {
    //                    TextBox txtReceiveQty1 = (TextBox)gr.FindControl("txtReceiveQty");
    //                    TextBox txtReceive1 = (TextBox)gr.FindControl("txtReceive");
    //                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectItem");
    //                    txtReceiveQty1.Text = "0.00";
    //                    if (chkSelect.Checked.Equals(true))
    //                    {
    //                        if (Convert.ToDouble(txtReceive1.Text).Equals(0))
    //                        {
    //                            chkSelect.Checked = false;
    //                        }
    //                    }
    //                }
    //                chk.Checked = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //protected void txtReceiveQty_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        TextBox txtReceiveQty = (TextBox)sender;
    //        GridViewRow row = (GridViewRow)txtReceiveQty.NamingContainer;
    //        TextBox txtReceive = (TextBox)row.FindControl("txtReceive");
    //        Label lblBOQty = (Label)row.FindControl("lblBOQty");
    //        CheckBox chk = (CheckBox)row.FindControl("chkSelect");

    //        if (!(string.IsNullOrEmpty(txtReceiveQty.Text)))
    //        {
    //            if (Convert.ToDouble(txtReceiveQty.Text) > 0)
    //            {
    //                txtReceive.Text = "0.00";
    //                double outstandQty = double.Parse(lblBOQty.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                              NumberStyles.AllowThousands |
    //                              NumberStyles.AllowDecimalPoint);
    //                if (Convert.ToDouble(txtReceiveQty.Text) > outstandQty)
    //                {
    //                    txtReceiveQty.Text = outstandQty.ToString("0.00", CultureInfo.InvariantCulture);
    //                }

    //                foreach (GridViewRow gr in gvPOItems.Rows)
    //                {
    //                    TextBox txtReceive1 = (TextBox)gr.FindControl("txtReceive");
    //                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
    //                    TextBox txtReceiveQty1 = (TextBox)gr.FindControl("txtReceiveQty");
    //                    txtReceive1.Text = "0.00";
    //                    if(chkSelect.Checked.Equals(true))
    //                    {
    //                        if(Convert.ToDouble(txtReceiveQty1.Text).Equals(0))
    //                        {
    //                            chkSelect.Checked = false;
    //                        }
    //                    }
    //                }
    //                chk.Checked = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    #endregion

    #region Custom function
    private void FillAddress()
    {
        try
        {
            if (!string.IsNullOrEmpty(hdnVendorID.Value))
            {
                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = Convert.ToInt32(hdnVendorID.Value);
                DataSet ds = new DataSet();
                ds = _objBLVendor.GetVendorRolDetails(_objVendor);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["city"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString() + ", " + ds.Tables[0].Rows[0]["Zip"].ToString();
                }
                else
                {
                    txtAddress.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillUserAddress()
    {
        try
        {
            DataSet dsC = new DataSet();
            _objPropUser.ConnConfig = Session["config"].ToString();
            dsC = _objBLUser.getControl(_objPropUser);

            string address;
            address = dsC.Tables[0].Rows[0]["Address"].ToString() + ", " + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            txtShipTo.Text = address;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
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
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
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

                string POPermission = ds.Rows[0]["PO"] == DBNull.Value ? "YYYY" : ds.Rows[0]["PO"].ToString();
                string ADD = POPermission.Length < 1 ? "Y" : POPermission.Substring(0, 1);
                string Edit = POPermission.Length < 2 ? "Y" : POPermission.Substring(1, 1);
                string Delete = POPermission.Length < 2 ? "Y" : POPermission.Substring(2, 1);
                string View = POPermission.Length < 4 ? "Y" : POPermission.Substring(3, 1);

              
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
                        btnSubmit.Visible =  false;
                    }
                    else
                    {
                        Response.Redirect("Home.aspx?permission=no"); return;
                    }
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
    private bool ValidateGrid()
    {
        bool IsValid = false;
        int count = 0;
        //foreach (GridViewRow gr in gvPOItems.Rows)
        foreach (GridDataItem gr in RadGrid_POItems.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectItem");
            TextBox txtReceive = (TextBox)gr.FindControl("txtReceive");
            TextBox txtReceiveQty = (TextBox)gr.FindControl("txtReceiveQty");

            if (chkSelect.Checked.Equals(true))
            {
                if (!string.IsNullOrEmpty(txtReceive.Text))
                {
                    if (Convert.ToDouble(txtReceive.Text) != 0)
                    {
                        count++;
                    }
                }
                if (!string.IsNullOrEmpty(txtReceiveQty.Text))
                {
                    if (Convert.ToDouble(txtReceiveQty.Text) != 0)
                    {
                        count++;
                    }
                }
            }
        }
        if (count > 0)
        {
            IsValid = true;
        }
        return IsValid;
    }
    private DataTable GetRPOItem()
    {
        DataTable dt = new DataTable();
        
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));        
        dt.Columns.Add(new DataColumn("POLine", typeof(Int32)));      
        DataColumn dcq = new DataColumn("Quan", typeof(Decimal));
        dcq.AllowDBNull = true;
        DataColumn dPrice = new DataColumn("Price", typeof(string));
        dPrice.AllowDBNull = true;
        DataColumn dAmount = new DataColumn("Amount", typeof(Decimal));
        dAmount.AllowDBNull = true;
        dt.Columns.Add(dAmount);
        dt.Columns.Add(dcq);
        dt.Columns.Add(dPrice);
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("ItemID", typeof(Int32)));   
        dt.Columns.Add(new DataColumn("POItemId", typeof(Int32)));        
        dt.Columns.Add(new DataColumn("Selected", typeof(string)));
        dt.Columns.Add(new DataColumn("SelectedQuan", typeof(string)));
        dt.Columns.Add(new DataColumn("Balance", typeof(string)));
        dt.Columns.Add(new DataColumn("BalanceQuan", typeof(string)));
        dt.Columns.Add(new DataColumn("IsReceiveIssued", typeof(Int32)));
        dt.Columns.Add(new DataColumn("JobID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("Warehouse", typeof(string)));
        dt.Columns.Add(new DataColumn("LocationID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("BOQty", typeof(string)));
        return dt;
    }    
    private List<GridDataItem> GetUnReceivedSelectedItems(string addeditRPO)
    {
        strErrorMessage = string.Empty;
        var selectedItems = new List<GridDataItem>();
        DataTable dt = GetRPOItem();
        if (addeditRPO == "ADD")
        {
            bool IsExist;
            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.Ref = txtRef.Text;
            _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);
            IsExist = _objBLBills.IsExistRPOForInsert(_objPO);
            if (IsExist.Equals(true))
            {
                strErrorMessage = "Ref number already exists with this vendor, Please use different Ref number!";
                //break;
            }
        }
        else if (addeditRPO == "EDIT")
        {
            bool IsExist;
            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.Ref = txtRef.Text;            
            IsExist = _objBLBills.IsExistRPOForInsert(_objPO);
            if (IsExist.Equals(true))
            {
                strErrorMessage = "Ref number already exists with this vendor, Please use different Ref number!";
                //break;
            }

        }

        if (strErrorMessage.Trim() == "")
        {
            foreach (GridDataItem gr in RadGrid_POItems.Items)
            {
                

                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectItem");
                TextBox txtReceiveQty = (TextBox)gr.FindControl("txtReceiveQty");
                TextBox txtReceive = (TextBox)gr.FindControl("txtReceive");

                HiddenField hdnInvID = (HiddenField)gr.FindControl("hdnInvID");
                HiddenField hdnfDesc = (HiddenField)gr.FindControl("hdnfDesc");
                DropDownList drpRecievepoIssued = (DropDownList)gr.FindControl("drpRecievepoIssued");

                Label lblWarehouseID = (Label)gr.FindControl("lblWarehouseID");
                Label lblLocationID = (Label)gr.FindControl("lblLocationID");
                Label lblJobID = (Label)gr.FindControl("lblJobID");
                HiddenField hdnWarehouse = (HiddenField)gr.FindControl("hdnWarehouse");
                HiddenField hdnWarehouseLocationID = (HiddenField)gr.FindControl("hdnWarehouseLocationID");

                HiddenField hdnRPOItemId = (HiddenField)gr.FindControl("hdnRPOItemId");
                HiddenField hdnPOItemId = (HiddenField)gr.FindControl("hdnPOItemId");                

                Label lblOutstand = (Label)gr.FindControl("lblOutstand");
                Label lblPrice = (Label)gr.FindControl("lblPrice");
                Label lblLine = (Label)gr.FindControl("lblLine");
                Label lblPrvIn = (Label)gr.FindControl("lblPrvIn");
                Label lblPrvInQuan = (Label)gr.FindControl("lblPrvInQuan");
                Label lblOrderedQuan = (Label)gr.FindControl("lblOrderedQuan");
                Label lblOrdered = (Label)gr.FindControl("lblOrdered");
                Label lblBOQty = (Label)gr.FindControl("lblBOQty");

                HiddenField hdnReceive = (HiddenField)gr.FindControl("hdnReceive");
                HiddenField hdnReceiveQty = (HiddenField)gr.FindControl("hdnReceiveQty");


                if ((chkSelect.Checked.Equals(true)) || (Convert.ToDouble(txtReceiveQty.Text) != 0) || (Convert.ToDouble(txtReceive.Text) != 0))
                {


                    if (string.IsNullOrEmpty(txtReceive.Text))
                    {
                        strErrorMessage = "Receive amount cannot be empty.";
                        break;
                    }
                    if (string.IsNullOrEmpty(txtReceiveQty.Text))
                    {
                        strErrorMessage = "Receive quantity cannot be empty.";
                        break;
                    }
                    ///////// Check Job Closed in bill ///////
                    if (!string.IsNullOrEmpty(lblJobID.Text))
                    {
                        if (lblJobID.Text != "0")
                        {
                            DataSet _dsJobs = new DataSet();
                            objJob.ConnConfig = Session["config"].ToString();
                            objJob.ID = Convert.ToInt32(lblJobID.Text);
                            _dsJobs = objBL_Job.spGetJobStatus(objJob);

                            int jobstatus = Convert.ToInt32(_dsJobs.Tables[0].Rows[0]["STATUS"].ToString());
                            if (jobstatus == 1)
                            {
                                strErrorMessage = "Project# " + lblJobID.Text.ToString() + "  is closed. Please change the project status before proceeding.";
                                break;
                            }
                        }
                    }
                    else if (Convert.ToDouble(txtReceiveQty.Text) < 0)
                    {
                        // TODO: Validation with onHand value in case of inventory items
                        //DropDownList drpRecievepoIssued = (DropDownList)gr.FindControl("drpRecievepoIssued");
                        if (Convert.ToInt32(drpRecievepoIssued.SelectedValue) == 2)
                        { // Inventory item
                          //HiddenField hdnInvID = (HiddenField)gr.FindControl("hdnInvID");


                            var receiveQty = Convert.ToDouble(txtReceiveQty.Text);

                            IWarehouseLocAdj invWarehouseLoc = new IWarehouseLocAdj();
                            invWarehouseLoc.InvID = int.Parse(hdnInvID.Value);
                            if (string.IsNullOrEmpty(lblJobID.Text) || lblJobID.Text == "0")
                            {
                                invWarehouseLoc.WarehouseID = lblWarehouseID.Text;
                                invWarehouseLoc.locationID = int.Parse(lblLocationID.Text == "" ? "0" : lblLocationID.Text);
                            }
                            else
                            {
                                invWarehouseLoc.WarehouseID = hdnWarehouse.Value;
                                invWarehouseLoc.locationID = int.Parse(hdnWarehouseLocationID.Value);
                            }

                            // TODO: Get onHand value from database
                            DataSet ds = new DataSet();

                            ds = _objInventory.GetAutoFillOnHandBalance(invWarehouseLoc);
                            var table = ds.Tables[0];
                            if (table != null && table.Rows.Count > 0)
                            {
                                var onHandVal = Convert.ToDouble(table.Rows[0]["Hand"].ToString());
                                if (onHandVal + receiveQty < 0)
                                {
                                    strErrorMessage = "On hand value is not enough.";
                                    break;
                                }
                            }
                        }

                        int Inv = 0;
                        if (Convert.ToString(hdnInvID.Value).ToString().Trim() != "" && hdnInvID.Value != string.Empty && hdnInvID.Value != null)
                        {
                            Inv = Convert.ToInt32(hdnInvID.Value);
                        }
                        string sName = Convert.ToString(hdnfDesc.Value);
                        if (Inv > 0 && Convert.ToInt32(drpRecievepoIssued.SelectedValue) == 2)
                        {
                            DataSet _dsInv = new DataSet();
                            _objInv.ConnConfig = Session["config"].ToString();
                            _objInv.ID = Inv;
                            _objInv.UserID = Convert.ToInt32(Session["UserID"].ToString());
                            _dsInv = _objBLBills.GetInventoryItemStatus(_objInv);
                            int acctstatus = Convert.ToInt32(_dsInv.Tables[0].Rows[0]["Status"].ToString());
                            if (acctstatus == 1)
                            {
                                strErrorMessage = "Item " + sName + " is Inactive.";
                                break;
                            }
                        }


                    }

                }

                if (string.IsNullOrEmpty(strErrorMessage))
                {

                    if ((chkSelect.Checked.Equals(true)) || (Convert.ToDouble(txtReceiveQty.Text) != 0) || (Convert.ToDouble(txtReceive.Text) != 0))
                    {

                        selectedItems.Add(gr);

                        if (addeditRPO == "ADD")
                        {
                            double dblOutstand = 0.00;
                            double dblReceive = 0.00;
                            double dblReceiveQty = Convert.ToDouble(txtReceiveQty.Text);
                            double dblPrvInQuan = Convert.ToDouble(lblPrvInQuan.Text);
                            double dblPrvIn = ConvertCurrentCurrencyFormatToDbl(lblPrvIn.Text);

                            if (!Convert.ToDouble(txtReceive.Text).Equals(0))
                            {
                                dblOutstand = ConvertCurrentCurrencyFormatToDbl(lblOutstand.Text);
                                dblReceive = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                                dblPrvIn = ConvertCurrentCurrencyFormatToDbl(lblPrvIn.Text);

                                //IsAmount = true;
                            }
                            if (dblReceive >= ConvertCurrentCurrencyFormatToDbl(lblOrdered.Text) - dblPrvIn)
                            {
                                dblReceive = Math.Round(ConvertCurrentCurrencyFormatToDbl(lblOrdered.Text) - dblPrvIn, 4);
                                txtReceive.Text = Convert.ToString(dblReceive);

                                dblReceiveQty = Math.Round(Convert.ToDouble(lblOrderedQuan.Text) - dblPrvInQuan, 4);
                                txtReceiveQty.Text = Convert.ToString(dblReceiveQty);

                                dblOutstand = dblReceive;
                                lblBOQty.Text = Convert.ToString(dblReceiveQty);
                            }
                            if (dblReceiveQty >= Convert.ToDouble(lblOrderedQuan.Text) - dblPrvInQuan)
                            {
                                dblReceiveQty = Math.Round(Convert.ToDouble(lblOrderedQuan.Text) - dblPrvInQuan, 4);
                                txtReceiveQty.Text = Convert.ToString(dblReceiveQty);

                                dblReceive = Math.Round(ConvertCurrentCurrencyFormatToDbl(lblOrdered.Text) - dblPrvIn, 4);
                                txtReceive.Text = Convert.ToString(dblReceive);

                                dblOutstand = dblReceive;
                                lblBOQty.Text = Convert.ToString(dblReceiveQty);
                            }

                            // Start-- ES-6411 QAE- Major issue on Received PO (As per Laxmi want)
                            if (dblOutstand == 0 && dblReceive == 0)
                            {
                                dblOutstand = ConvertCurrentCurrencyFormatToDbl(lblOutstand.Text);
                                dblPrvIn = ConvertCurrentCurrencyFormatToDbl(lblPrvIn.Text);
                            }
                            // End-- ES-6411 QAE- Major issue on Received PO (As per Laxmi want)

                            //_objPO.Balance = dblOutstand - dblReceive;
                            //_objPO.Selected = dblReceive + dblPrvIn;
                            //_objBLBills.UpdatePOItemBalance(_objPO);
                            //amount = amount + dblReceive;
                            //_objPO.SelectedQuan = Math.Round((dblPrvInQuan + dblReceiveQty), 4);
                            //_objPO.BalanceQuan = ConvertCurrentCurrencyFormatToDbl(lblOrderedQuan.Text) - _objPO.SelectedQuan;
                            //_objBLBills.UpdatePOItemQuan(_objPO);

                            DataRow dr = dt.NewRow();
                            dr["ID"] = Convert.ToInt32(hdnRPOItemId.Value);
                            dr["POLine"] = Convert.ToInt32(lblLine.Text);
                            if (Convert.ToString(txtReceiveQty.Text) != "")
                            {
                                dr["Quan"] = Convert.ToDecimal(ConvertCurrentCurrencyFormatToDbl(txtReceiveQty.Text));
                            }
                            else
                            {
                                dr["Quan"] = 0;
                            }
                            dr["Price"] = Convert.ToString(ConvertCurrentCurrencyFormatToDbl(lblPrice.Text));
                            if (Convert.ToString(txtReceive.Text) != "")
                            {
                                dr["Amount"] = Convert.ToDecimal(ConvertCurrentCurrencyFormatToDbl(txtReceive.Text));
                            }
                            else
                            {
                                dr["Amount"] = 0;
                            }
                            dr["fDesc"] = Convert.ToString(hdnfDesc.Value);
                            if (Convert.ToString(hdnInvID.Value) != "")
                            {
                                dr["ItemID"] = Convert.ToInt32(hdnInvID.Value);
                            }
                            else
                            {
                                dr["ItemID"] = 0;
                            }
                            dr["POItemId"] = hdnPOItemId.Value != "" ? Convert.ToInt32(hdnPOItemId.Value) : 0;
                            dr["Selected"] = Convert.ToString(dblReceive + dblPrvIn);
                            dr["SelectedQuan"] = Convert.ToString(Math.Round((dblPrvInQuan + dblReceiveQty), 4));
                            dr["Balance"] = Convert.ToString(dblOutstand - dblReceive);
                            dr["BalanceQuan"] = Convert.ToString(Math.Round((Math.Round(ConvertCurrentCurrencyFormatToDbl(lblOrderedQuan.Text), 5) - Math.Round((dblPrvInQuan + dblReceiveQty), 5)), 5));
                            dr["IsReceiveIssued"] = Convert.ToInt32(drpRecievepoIssued.SelectedValue);
                            dr["JobID"] = lblJobID.Text != "" ? Convert.ToInt32(lblJobID.Text) : 0;
                            dr["Warehouse"] = Convert.ToString(hdnWarehouse.Value);
                            dr["LocationID"] = hdnWarehouseLocationID.Value != "" ? Convert.ToInt32(hdnWarehouseLocationID.Value) : 0;
                            dr["BOQty"] = Convert.ToString(lblBOQty.Text);

                            dt.Rows.Add(dr);
                        }
                        else if (addeditRPO == "EDIT")
                        {
                            var dblOutstand = 0.0000;
                            var dblReceive = 0.0000;
                            var dblPrvIn = 0.0000;
                            var dblReceiveOrg = 0.0000;
                            if (!Convert.ToDouble(txtReceive.Text).Equals(0))
                            {
                                dblOutstand = ConvertCurrentCurrencyFormatToDbl(lblOutstand.Text);
                                dblReceive = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                                dblPrvIn = ConvertCurrentCurrencyFormatToDbl(lblPrvIn.Text);
                                dblReceiveOrg = Convert.ToDouble(hdnReceive.Value);
                            }

                            var dblReceiveQty = Convert.ToDouble(txtReceiveQty.Text);
                            var dblPrvInQuan = Convert.ToDouble(lblPrvInQuan.Text);
                            var dblReceiveQtyOrg = Convert.ToDouble(hdnReceiveQty.Value);

                            DataRow dr = dt.NewRow();
                            dr["ID"] = Convert.ToInt32(hdnRPOItemId.Value);
                            dr["POLine"] = Convert.ToInt32(lblLine.Text);
                            //dr["Quan"] = Convert.ToString(ConvertCurrentCurrencyFormatToDbl(txtReceiveQty.Text));
                            if (Convert.ToString(txtReceiveQty.Text) != "")
                            {
                                dr["Quan"] = Convert.ToDecimal(ConvertCurrentCurrencyFormatToDbl(txtReceiveQty.Text));
                            }
                            else
                            {
                                dr["Quan"] = 0;
                            }
                            dr["Price"] = Convert.ToString(ConvertCurrentCurrencyFormatToDbl(lblPrice.Text));
                            //dr["Amount"] = Convert.ToString(ConvertCurrentCurrencyFormatToDbl(txtReceive.Text));
                            if (Convert.ToString(txtReceive.Text) != "")
                            {
                                dr["Amount"] = Convert.ToDecimal(ConvertCurrentCurrencyFormatToDbl(txtReceive.Text));
                            }
                            else
                            {
                                dr["Amount"] = 0;
                            }
                            dr["fDesc"] = Convert.ToString(hdnfDesc.Value);
                            //dr["ItemID"] = Convert.ToInt32(hdnInvID.Value);
                            if (Convert.ToString(hdnInvID.Value) != "")
                            {
                                dr["ItemID"] = Convert.ToInt32(hdnInvID.Value);
                            }
                            else
                            {
                                dr["ItemID"] = 0;
                            }
                            dr["POItemId"] = hdnPOItemId.Value != "" ? Convert.ToInt32(hdnPOItemId.Value) : 0;
                            dr["Selected"] = Convert.ToString(dblPrvIn - dblReceiveOrg + dblReceive);
                            dr["SelectedQuan"] = Convert.ToString(Math.Round((dblPrvInQuan - dblReceiveQtyOrg + dblReceiveQty), 4));
                            dr["Balance"] = Convert.ToString(dblOutstand + dblReceiveOrg - dblReceive);
                            dr["BalanceQuan"] = Convert.ToString(Math.Round((Convert.ToDouble(lblBOQty.Text) + dblReceiveQtyOrg - dblReceiveQty), 4));
                            dr["IsReceiveIssued"] = Convert.ToInt32(drpRecievepoIssued.SelectedValue);
                            dr["JobID"] = lblJobID.Text != "" ? Convert.ToInt32(lblJobID.Text) : 0;
                            dr["Warehouse"] = Convert.ToString(hdnWarehouse.Value);
                            dr["LocationID"] = hdnWarehouseLocationID.Value != "" ? Convert.ToInt32(hdnWarehouseLocationID.Value) : 0;
                            dr["BOQty"] = Convert.ToString(lblBOQty.Text);
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
        }
        ViewState["dtRPOItem"] = dt;
        return selectedItems;
    }

    private string ValidateSelectedItems(List<GridDataItem> lstSelectItems)
    {
        string str="";
        return str;
    }

    #endregion
    private void GetAllPOByVendor()
    {
        _objPO.ConnConfig = Session["config"].ToString();
        _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Session["CmpChkDefault"].ToString() == "1")
        {
            _objPO.EN = 1;
        }
        else
        {
            _objPO.EN = 0;
        }
        if (hdnVendorID.Value != null && hdnVendorID.Value.Trim() != "")
        {
            _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);
        }
        else
        {
            _objPO.Vendor = -1;
        }
        DataTable ds = _objBLBills.GetAllPOByDue(_objPO);

        RadGrid_ReceivePO.VirtualItemCount = ds.Rows.Count;
        RadGrid_ReceivePO.DataSource = ds;
        RadGrid_ReceivePO.DataBind();


    }
    public void GetPOByIdAjax(string strPOID)
    {
        PO objPO = new PO();
        BL_Bills objBLBills = new BL_Bills();

        DataSet ds = new DataSet();
        try
        {
            objPO.ConnConfig = Session["config"].ToString();
            objPO.POID = string.IsNullOrEmpty(strPOID) ? 0 : Convert.ToInt32(strPOID);
            //ds = objBLBills.GetPOItemByPO(objPO);
            #region Company Check
            objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objPO.EN = 1;
            }
            else
            {
                objPO.EN = 0;
            }
            #endregion
            ds = objBLBills.GetPOByIdAjax(objPO);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtDueDate.Text = ds.Tables[0].Rows[0]["Due"].ToString();
                txtVendor.Text = ds.Tables[0].Rows[0]["VendorName"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                hdnVendorID.Value = ds.Tables[0].Rows[0]["Vendor"].ToString();
                txtShipTo.Text = ds.Tables[0].Rows[0]["ShipTo"].ToString();
                txtCreatedBy.Text = ds.Tables[0].Rows[0]["fBy"].ToString();
                txtStatus.Text = ds.Tables[0].Rows[0]["StatusName"].ToString();
                txtComments.Text = ds.Tables[0].Rows[0]["fDesc"].ToString();
                hdnAmount.Value = ds.Tables[0].Rows[0]["Amount"].ToString();
                txtVendorType.Text = ds.Tables[0].Rows[0]["VendorType"].ToString();                
            }          
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
        
    }
    protected void RadGrid_ReceivePO_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_ReceivePO.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        //DataTable dt = (DataTable)Session["dsPOData"];ViewState["PO"]
        //_objPO.ConnConfig = Session["config"].ToString();
        //_objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
        //if (Session["CmpChkDefault"].ToString() == "1")
        //{
        //    _objPO.EN = 1;
        //}
        //else
        //{
        //    _objPO.EN = 0;
        //}
        //DataTable ds = _objBLBills.GetAllPOByDue(_objPO);        
        ////DataTable dt = (DataTable)ViewState["PO"];
        //RadGrid_ReceivePO.DataSource = ds;
        //GetAllPOByVendor();

    }
    bool isGrouping = false;

    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_ReceivePO.MasterTableView.FilterExpression != "" ||
            (RadGrid_ReceivePO.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_ReceivePO.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_ReceivePO_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            General _objPropGeneral = new General();
            BL_General _objBLGeneral = new BL_General();

            _objPropGeneral.ConnConfig = Session["config"].ToString();
            foreach (GridDataItem row in RadGrid_ReceivePO.SelectedItems)
            {
                Label lblID = (Label)row.FindControl("lblID");
                _objPO.ConnConfig = Session["config"].ToString();
                _objPO.POID = Convert.ToInt32(lblID.Text);
                _objPO.EN = 1;
                _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
                if (Session["CmpChkDefault"].ToString() == "1")
                {
                    _objPO.EN = 1;
                }
                else
                {
                    _objPO.EN = 0;
                }
                DataTable _dtPO = _objBLBills.spGetOpenPODetailforRPO(_objPO);
                //DataTable _dtPO = new DataTable();
                if (_dtPO.Rows.Count > 0)
                {
                    txtVendor.Text = _dtPO.Rows[0]["VendorName"].ToString();
                    txtVendorType.Text = _dtPO.Rows[0]["VendorType"].ToString();
                    hdnVendorID.Value = _dtPO.Rows[0]["Vendor"].ToString();
                    txtAddress.Text = _dtPO.Rows[0]["Address"].ToString();
                    txtPO.Text = _dtPO.Rows[0]["PO"].ToString();
                    txtDueDate.Text = _dtPO.Rows[0]["Due"].ToString();
                    hdnAmount.Value = _dtPO.Rows[0]["Amount"].ToString();
                    txtComments.Text = _dtPO.Rows[0]["fDesc"].ToString();
                    string status = "New";
                    txtVendor.Enabled = false;
                    switch (Convert.ToInt16(_dtPO.Rows[0]["Status"].ToString()))
                    {
                        case 0:
                            status = "New";
                            break;
                        case 1:
                            status = "Closed";
                            break;
                        case 2:
                            status = "Cancelled";
                            break;
                        case 3:
                            status = "Partial-Quantity";
                            break;
                        case 4:
                            status = "Partial-Amount";
                            break;
                    }
                    txtStatus.Text = status;
                }
                DataSet ds = _objBLBills.GetPOItemByPO(_objPO);
                BINDRadGrid_POItems(ds.Tables[0]);
            }


                //foreach (GridDataItem row in RadGrid_ReceivePO.SelectedItems)
                //{
                //    Label lblID = (Label)row.FindControl("lblID");
                //    Label lblDue = (Label)row.FindControl("lblDue");
                //    Label lblfDate = (Label)row.FindControl("lblfDate");
                //    Label lblAmount = (Label)row.FindControl("lblAmount");
                //    Label lblComment = (Label)row.FindControl("lblComment");
                //    Label lblVendorName = (Label)row.FindControl("lblVendorName");
                //    Label lblVendorType = (Label)row.FindControl("lblVendorType");
                //    Label lblVendor = (Label)row.FindControl("lblVendor");
                //    Label lblAddress = (Label)row.FindControl("lblAddress");
                //    Label lblStatus = (Label)row.FindControl("lblStatus");

                //    _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
                //    if (Session["CmpChkDefault"].ToString() == "1")
                //    {
                //        _objPO.EN = 1;
                //    }
                //    else
                //    {
                //        _objPO.EN = 0;
                //    }
                //    _objPO.ConnConfig = Session["config"].ToString();
                //    _objPO.POID = Convert.ToInt32(lblID.Text);
                //    DataSet ds = _objBLBills.GetPOItemByPO(_objPO);

                //    //DataSet ds = GetPOItemByPO(_objPO);

                //    //gvPOItems.DataSource = ds.Tables[0];
                //    //gvPOItems.DataBind();


                //    BINDRadGrid_POItems(ds.Tables[0]);

                //    //RadGrid_POItems.DataSource = ds.Tables[0];
                //    //RadGrid_POItems.DataBind();



                //    txtVendor.Text = lblVendorName.Text;
                //    txtVendorType.Text = lblVendorType.Text;
                //    hdnVendorID.Value = lblVendor.Text;
                //    txtAddress.Text = lblAddress.Text;
                //    txtPO.Text = lblID.Text;
                //    txtDueDate.Text = lblDue.Text;
                //    hdnAmount.Value = lblAmount.Text;
                //    txtComments.Text = lblComment.Text;
                //    string status = "New";
                //    switch (Convert.ToInt16(lblStatus.Text))
                //    {
                //        case 0:
                //            status = "New";
                //            break;
                //        case 1:
                //            status = "Closed";
                //            break;
                //        case 2:
                //            status = "Cancelled";
                //            break;
                //        case 3:
                //            status = "Partial-Quantity";
                //            break;
                //        case 4:
                //            status = "Partial-Amount";
                //            break;
                //    }
                //    txtStatus.Text = status;
                //}
                


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);


        }
    }

    protected void RadGrid_POItems_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_POItems.AllowCustomPaging = !ShouldApplySortFilterOrGroupPOItem();
    }

    bool isGroupingPOItem = false;

    public bool ShouldApplySortFilterOrGroupPOItem()
    {
        return RadGrid_POItems.MasterTableView.FilterExpression != "" ||
            (RadGrid_POItems.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_POItems.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_POItems_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {


            #region DrpIssueBInd Dynamic

            //General _objPropGeneral = new General();
            //BL_General _objBLGeneral = new BL_General();
            //_objPropGeneral.ConnConfig = Session["config"].ToString();
            //DataSet _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");
            //Boolean TrackingInventory = false;
            //if (_dsCustom.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            //    {
            //        if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
            //        {
            //            TrackingInventory = Convert.ToBoolean(_dr["Label"]);
            //        }
            //    }
            //}
            Boolean TrackingInventory = hdnIsInventoryTrackingIsOn.Value == "1" ? true : false;
            DropDownList drpRecievepoIssued = (DropDownList)e.Item.FindControl("drpRecievepoIssued");
            List<issueDrpValue> ListissueDrpValue = new List<issueDrpValue>();

            issueDrpValue first = new issueDrpValue();
            first.ID = 0;
            first.Name = "Select";
            ListissueDrpValue.Add(first);

            issueDrpValue second = new issueDrpValue();
            second.ID = 1;
            second.Name = "Project";
            ListissueDrpValue.Add(second);


            if (TrackingInventory == true)//Change it As per your requirement 
            {
                issueDrpValue third = new issueDrpValue();
                third.ID = 2;
                third.Name = "Inventory";
                ListissueDrpValue.Add(third);
            }

            drpRecievepoIssued.DataTextField = "Name";
            drpRecievepoIssued.DataValueField = "ID";
            drpRecievepoIssued.DataSource = ListissueDrpValue;
            drpRecievepoIssued.DataBind();


            //  if (string.IsNullOrEmpty(jobID) || jobID == "0")
            if (TrackingInventory == true)//Change it As per your requirement 
            {
                //drpRecievepoIssued.SelectedValue = "2";

            }
            else
            {
                drpRecievepoIssued.SelectedValue = "1";
            }
            #endregion
        }
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["ReceivePOList"];
            string url = "addreceivepo.aspx?id=" + dt.Rows[0]["ID"];
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
            dt = (DataTable)Session["ReceivePOList"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addreceivepo.aspx?id=" + dt.Rows[index - 1]["ID"];
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
            dt = (DataTable)Session["ReceivePOList"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addreceivepo.aspx?id=" + dt.Rows[index + 1]["ID"];
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
            dt = (DataTable)Session["ReceivePOList"];
            string url = "addreceivepo.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
    }

    protected void RadGrid_ReceivePO_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "RowClick")
        {
            if (RadGrid_ReceivePO.SelectedItems.Count <= 0 || txtPO.Text == "")
            {
                RadGrid_POItems.DataSource = string.Empty;
                RadGrid_POItems.DataBind();
                txtVendor.Text = "";
                txtVendorType.Text = "";
                txtAddress.Text = "";
                txtPO.Text = "";
                txtDueDate.Text = "";
                txtComments.Text = "";
                txtStatus.Text = "";
            }
        }
    }

    protected void RadGrid_ReceivePO_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            Label lblID = (Label)e.Item.FindControl("lblID");
            if (lblID.Text == txtPO.Text)
            {
                item.Selected = true;
            }
        }
    }

    protected void lnkReceivePOReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReceivePOReport.aspx?ID=" + Request.QueryString["id"]);
    }

    /// <summary>
    /// Get Method
    /// </summary>
    /// <param name="_objPO"></param>
    /// <returns></returns>

    public DataSet GetPOByVendor(PO _objPO)
    {
        try
        {
            StringBuilder varname = new StringBuilder();
            varname.Append("        SELECT p.*, r.Name as VendorName, r.Address +', '+ CHAR(13)+ r.City +', '+ r.State+', '+ r.Zip as Address                 \n");
            varname.Append("            FROM PO as p with (nolock) INNER JOIN Vendor as v   with (nolock)      \n");
            varname.Append("                ON p.Vendor = v.ID                      \n");
            varname.Append("                INNER JOIN Rol as r with (nolock) ON v.Rol = r.ID         \n");
            varname.Append("     left outer join tblUserCo UC with (nolock) on UC.CompanyID = r.EN  \n");
            varname.Append("     left outer join Branch B with (nolock) on B.ID = r.EN  \n");
            varname.Append("                WHERE p.Vendor = '" + _objPO.Vendor + "' AND (p.Status=0 OR p.Status=3 OR p.Status=4)  \n");
            if (_objPO.EN == 1)
            {
                varname.Append("      AND UC.IsSel = " + _objPO.EN + " and UC.UserID= " + _objPO.UserID + "  \n");
            }
            varname.Append("        SELECT  \n");
            varname.Append("            poi.PO, poi.Line, poi.Quan, poi.fDesc, poi.Price, poi.Amount, poi.Job, poi.Phase, poi.due,J.fDesc As JobName,Loc.tag As LocationName,   \n");
            varname.Append("            poi.Amount as Ordered,                      \n");
            varname.Append("            poi.Selected as PrvIn,                      \n");
            varname.Append("            poi.Balance as Outstanding,                 \n");
            varname.Append("            0.00 as Received,                           \n");
            varname.Append("            poi.Quan as OrderedQuan,            \n");
            varname.Append("            poi.SelectedQuan as PrvInQuan,      \n");
            varname.Append("            poi.BalanceQuan as OutstandQuan,    \n");
            varname.Append("            0.00 as ReceivedQuan, 0 As IsReceiveIssued,              \n");
            varname.Append("            poi.Inv, poi.GL, poi.Freight, poi.Rquan, poi.Billed, poi.Ticket,poi.WarehouseID,poi.LocationID  ,       \n");



            varname.Append("    isNULL((SELECT top 1  1 FROM INV with (nolock) WHERE ID = (poi.Inv)and type = 0),0) IsItemsExistsInInventory  ,                 \n");
            varname.Append("    ( SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW with (nolock) inner join Warehouse AS Wh with (nolock) on Wh.ID = INW.WarehouseID   where  INW.InvID=poi.Inv  and    INW.WareHouseID=poi.WarehouseID) As WarehouseName  ,             \n");
            varname.Append("    (Select top 1 Name from WHLoc WH with (nolock) where WH.WareHouseID = poi.WarehouseID and id = poi.LocationID) As WarehouseLoc   ,    \n");
            varname.Append("     (                 \n");
            varname.Append("   SELECT  top 1                 \n");
            varname.Append(" isnull(bt.Type, '') as Phase                 \n");
            varname.Append(" FROM POItem as ppp  with (nolock)               \n");
            varname.Append(" LEFT JOIN JobTItem as jt with (nolock) ON jt.Line = ppp.Phase and isnull(jt.Job,0) = isnull(j.ID, 0)                 \n");
            varname.Append(" INNER JOIN BOM as b with (nolock) ON b.JobTItemID = jt.ID                 \n");
            varname.Append(" LEFT JOIN Inv as i with (nolock) on i.ID = ppp.Inv and b.matitem = i.id                 \n");
            varname.Append(" inner join BOMT bt with (nolock) on bt.ID = b.Type                 \n");
            varname.Append(" WHERE ppp.PO = poi.PO and ppp.Line = poi.Line  ) as IsInventoryCode                 \n");



            varname.Append("            FROM POItem as poi with (nolock) LEFT JOIN PO as p with (nolock) ON poi.PO = p.PO                   \n");
            varname.Append("            left outer JOIN Job as J with (nolock) ON poi.Job = J.ID   \n");
            varname.Append("    left outer JOIN Loc as Loc with (nolock) ON Loc.Loc = J.Loc   \n");
            varname.Append("             INNER JOIN Vendor as v with (nolock)   ON p.Vendor = v.ID                      \n");
            varname.Append("                INNER JOIN Rol as r with (nolock) ON v.Rol = r.ID         \n");
            varname.Append("     left outer join tblUserCo UC with (nolock) on UC.CompanyID = r.EN  \n");
            varname.Append("     left outer join Branch B with (nolock) on B.ID = r.EN  \n");
            varname.Append("                WHERE poi.PO = (select top 1 po from PO with (nolock) WHERE Vendor = '" + _objPO.Vendor + "' order by PO)   \n");
            if (_objPO.EN == 1)
            {
                varname.Append("      AND UC.IsSel = " + _objPO.EN + " and UC.UserID= " + _objPO.UserID + "  \n");
            }
            varname.Append("                ORDER by poi.line       \n");
            return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //public DataSet GetPOItemByPO(PO _objPO)
    //{
    //    try
    //    {
    //        StringBuilder varname = new StringBuilder();
    //        varname.Append("    select poi.PO, poi.Line, poi.Quan, poi.fDesc, poi.Price, poi.Amount, poi.Job, poi.Phase, poi.due,poi.Inv,J.fDesc AS JobName,Loc.Tag As LocationName ,   \n");
    //        varname.Append("    poi.Amount as Ordered,         \n");
    //        varname.Append("    isnull(poi.Selected,0.00) as PrvIn,                      \n");
    //        varname.Append("    isnull(poi.Balance,poi.Amount) as Outstanding,                \n");
    //        varname.Append("    0.00 as Received,                   \n");
    //        varname.Append("    poi.Quan as OrderedQuan,            \n");
    //        varname.Append("    isnull(poi.SelectedQuan,0.00) as PrvInQuan,      \n");
    //        varname.Append("    isnull(poi.BalanceQuan,poi.Quan) as OutstandQuan,    \n");
    //        varname.Append("    0.00 as ReceivedQuan,0 As IsReceiveIssued,               \n");
    //        varname.Append("    poi.Inv, poi.GL, poi.Freight, poi.Rquan, poi.Billed, poi.Ticket,poi.WarehouseID,poi.LocationID ,   \n");



    //        varname.Append("    isNULL((SELECT top 1  1 FROM INV with (nolock) WHERE ID = (poi.Inv)and type = 0),0) IsItemsExistsInInventory  ,                 \n");

    //        varname.Append("    ( SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW with (nolock) inner join Warehouse AS Wh with (nolock) on Wh.ID = INW.WarehouseID   where  INW.InvID=poi.Inv  and    INW.WareHouseID=poi.WarehouseID) As WarehouseName  ,             \n");

    //        varname.Append("    (Select top 1 Name from WHLoc WH with (nolock) where WH.WareHouseID = poi.WarehouseID and id = poi.LocationID) As WarehouseLoc   ,    \n");
    //        varname.Append("     (                 \n");
    //        varname.Append("   SELECT  top 1                 \n");
    //        varname.Append(" isnull(bt.Type, '') as Phase                 \n");
    //        varname.Append(" FROM POItem as ppp   with (nolock)              \n");
    //        varname.Append(" LEFT JOIN JobTItem as jt with (nolock) ON jt.Line = ppp.Phase and isnull(jt.Job,0) = isnull(j.ID, 0)                 \n");
    //        varname.Append(" INNER JOIN BOM as b with (nolock) ON b.JobTItemID = jt.ID                 \n");
    //        varname.Append(" LEFT JOIN Inv as i with (nolock) on i.ID = ppp.Inv and b.matitem = i.id                 \n");
    //        varname.Append(" inner join BOMT bt with (nolock) on bt.ID = b.Type                 \n");
    //        varname.Append(" WHERE ppp.PO = poi.PO and ppp.Line = poi.Line  ) as IsInventoryCode                 \n");



    //        varname.Append("    FROM POItem as poi with (nolock) LEFT JOIN PO as p with (nolock) ON poi.PO = p.PO   \n");
    //        varname.Append("    left outer JOIN Job as J with (nolock) ON poi.Job = J.ID   \n");
    //        varname.Append("    left outer JOIN Loc as Loc with (nolock) ON Loc.Loc = J.Loc   \n");
    //        varname.Append("             INNER JOIN Vendor as v with (nolock)   ON p.Vendor = v.ID                      \n");
    //        varname.Append("                INNER JOIN Rol as r with (nolock) ON v.Rol = r.ID         \n");
    //        if (_objPO.EN == 1)
    //        {
    //            varname.Append("     left outer join tblUserCo UC with (nolock) on UC.CompanyID = r.EN  \n");
    //        }
    //        varname.Append("     left outer join Branch B with (nolock) on B.ID = r.EN  \n");
    //        varname.Append("        WHERE poi.PO = '" + _objPO.POID + "'    \n");
    //        if (_objPO.EN == 1)
    //        {
    //            varname.Append("      AND UC.IsSel = " + _objPO.EN + " and UC.UserID= " + _objPO.UserID + "  \n");
    //        }
    //        //varname.Append("        AND (poi.Balance <> '0' AND poi.BalanceQuan <> '0')   \n");  by Ravinder for price 0.00 check
    //        varname.Append("        ORDER by poi.line                       \n");
    //        return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public DataSet GetReceivePoById(PO _objPO)
    //{
    //    try
    //    {
    //        //StringBuilder varname = new StringBuilder();
    //        //varname.Append("    SELECT p.PO, p.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, isnull(r.Status,0) as Status, p.Due, p.ShipVia,           \n");
    //        //varname.Append("         p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,    \n");
    //        //varname.Append("         p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, r.Amount as ReceivedAmount, r.fDate as ReceiveDate, \n");
    //        //varname.Append("         ro.Address +', '+ CHAR(13)+ ro.City +', '+ ro.State+', '+ ro.Zip as Address    \n");
    //        //varname.Append("         FROM ReceivePO as r with (nolock)                            \n");
    //        //varname.Append("         INNER JOIN PO as p with (nolock) ON r.PO=p.PO                 \n");
    //        //varname.Append("         INNER JOIN Vendor as v with (nolock) ON p.Vendor = v.ID       \n");
    //        //varname.Append("         INNER JOIN Rol as ro with (nolock) ON v.Rol = ro.ID           \n");
    //        //varname.Append("         WHERE r.ID = '" + _objPO.RID + "'      \n");


    //        //varname.Append("    SELECT p.PO,p.Line, p.Quan, p.fDesc, p.Price, p.Job, p.Phase,    \n");
    //        //varname.Append("         p.Rquan, p.Billed, p.Ticket, p.Due, p.GL, p.Freight, p.Inv,J.fDesc As JobName,Loc.Tag As LocationName, \n");
    //        //varname.Append("         p.Amount as Ordered,                       \n");
    //        //varname.Append("         p.Selected as PrvIn,                       \n");
    //        //varname.Append("         p.Balance as Outstanding,                  \n");
    //        //varname.Append("         rp.Amount as Received,                     \n");
    //        //varname.Append("         p.Quan as OrderedQuan,                     \n");
    //        //varname.Append("         p.SelectedQuan as PrvInQuan,               \n");
    //        //varname.Append("         p.BalanceQuan as OutstandQuan,             \n");
    //        //varname.Append("         isnull(rp.Quan,0) as ReceivedQuan,p.WarehouseID,p.LocationID,                    \n");
    //        //varname.Append("         rp.POLine,                                 \n");
    //        //varname.Append("         rp.ReceivePO,rp.IsReceiveIssued ,                              \n");



    //        //varname.Append("    isNULL((SELECT top 1  1 FROM INV with (nolock) WHERE ID = (p.Inv)and type = 0),0) IsItemsExistsInInventory  ,                 \n");

    //        //varname.Append("    ( SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW with (nolock) inner join Warehouse AS Wh with (nolock) on Wh.ID = INW.WarehouseID   where  INW.InvID=p.Inv  and    INW.WareHouseID=p.WarehouseID) As WarehouseName  ,             \n");
    //        //varname.Append("    (Select top 1 Name from WHLoc WH with (nolock) where WH.WareHouseID = p.WarehouseID and id = p.LocationID) As WarehouseLoc   ,    \n");
    //        //varname.Append("     (                 \n");
    //        //varname.Append("   SELECT  top 1                 \n");
    //        //varname.Append(" isnull(bt.Type, '') as Phase                 \n");
    //        //varname.Append(" FROM POItem as ppp with (nolock)                \n");
    //        //varname.Append(" LEFT JOIN JobTItem as jt with (nolock) ON jt.Line = ppp.Phase and isnull(jt.Job,0) = isnull(j.ID, 0)                 \n");
    //        //varname.Append(" INNER JOIN BOM as b with (nolock) ON b.JobTItemID = jt.ID                 \n");
    //        //varname.Append(" LEFT JOIN Inv as i with (nolock) on i.ID = ppp.Inv and b.matitem = i.id                 \n");
    //        //varname.Append(" inner join BOMT bt with (nolock) on bt.ID = b.Type                 \n");
    //        //varname.Append(" WHERE ppp.PO = p.PO and ppp.Line = p.Line  ) as IsInventoryCode                 \n");




    //        //varname.Append("         FROM ReceivePO AS r   with (nolock)       \n");
    //        //varname.Append("        RIGHT JOIN RPOItem AS rp with (nolock) on rp.ReceivePO = r.ID                 \n");
    //        //varname.Append("        INNER JOIN POItem AS p with (nolock) ON p.Line = rp.POLine                    \n");
    //        //varname.Append("        left outer JOIN Job AS J with (nolock) ON p.Job = J.ID                  \n");
    //        //varname.Append("        left outer JOIN LOC AS LOC with (nolock) ON LOC.Loc = J.Loc                  \n");
    //        //varname.Append("        WHERE r.ID = '" + _objPO.RID + "' and p.PO = r.PO                   \n");
    //        //return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.Text, varname.ToString());

    //        return SqlHelper.ExecuteDataset(_objPO.ConnConfig, CommandType.StoredProcedure,"", varname.ToString());
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
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
                _objPO.RID = Convert.ToInt32(Request.QueryString["id"]);
                dsLog = _objBLBills.GetReceivePOLogs(_objPO);
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

    //---------------------------->
    
    
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
            //adGrid_gvLogs.Rebind();
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
    private string GetUploadDirectory(string mainDirectory)
    {
        var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        return savepathconfig + @"\" + Session["dbname"] + @"\" + mainDirectory + @"\";
    }
    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string MIME = string.Empty;
            var savepath = string.Empty;
            //HttpPostedFile filePosted = Request.Files["ctl00$ContentPlaceHolder1$FileUpload1"];

            var mainDirectory = "RPODocs";

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                mainDirectory += "\\RPO_" + Request.QueryString["id"];
            }
            else
            {

                if (ViewState["TempUploadDirectory"] == null)
                {
                    ViewState["TempUploadDirectory"] = Guid.NewGuid().ToString("N");
                }

                mainDirectory += "\\" + ViewState["TempUploadDirectory"] as string;
            }

            savepath = GetUploadDirectory(mainDirectory);



            //foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
            //{



            if (Request.QueryString["id"] != null)
            //if (FileUpload1.HasFile)
            {
                // objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objMapData.TempId = "0";

                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {
                    string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();

                    // string savepath = savepathconfig + @"\" + Session["dbname"] + @"\CustDocs\Id_" + Request.QueryString["uid"].ToString() + @"\";
                  //  string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["id"].ToString() + @"\";
                    // string fileExtensionApplication = System.IO.Path.GetExtension(savepathconfig);
                    //filename = System.IO.Path.GetFileName(FileUpload1.Value);
                    filename = postedFile.FileName;
                   // filename = filename.Replace(",", "");
                    fullpath = savepath + filename;
                    MIME = Path.GetExtension(postedFile.FileName).Substring(1);


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


                    objMapData.Screen = "RPO";
                    objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
                    objMapData.TempId = "0";
                    objMapData.FileName = filename;
                    objMapData.DocTypeMIME = MIME;
                    objMapData.FilePath = fullpath;

                    objMapData.DocID = 0;
                    objMapData.Mode = 0;
                    objMapData.ConnConfig = Session["config"].ToString();
                    objMapData.Worker = Session["User"].ToString();
                    objBL_MapData.AddFile(objMapData);
                }
                UpdateDocInfo();
                //GetDocuments();
                RadGrid_Documents.Rebind();
                ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadSuccess", "noty({text: 'File uploaded successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                //RadGrid_gvLogs.Rebind();
            }
            else
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();

                //string savepath = savepathconfig + @"\" + Session["dbname"] + @"\CustDocs\Id_" + Request.QueryString["id"].ToString() + @"\";
              // string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["id"].ToString() + @"\";
                var tempTable = new DataTable();
                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {
                    filename = postedFile.FileName;
                    fullpath = savepath + filename;
                    // MIME = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);
                    MIME = Path.GetExtension(postedFile.FileName).Substring(1);
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

                    tempTable = SaveAttachedFilesWhenAddingAPEditCheck(filename, fullpath, MIME);
                }
                //var tempTable = SaveAttachedFilesWhenAddingAPBill(filename, fullpath, mime);
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
    private DataTable SaveAttachedFilesWhenAddingAPEditCheck(string fileName, string fullPath, string doctype)
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

    private void UpdateDocInfo()
    {
        _objPropUser.ConnConfig = Session["config"].ToString();
        _objPropUser.dtDocs = SaveDocInfo();
        _objPropUser.Username = Session["User"].ToString();
        _objBLUser.UpdateDocInfo(_objPropUser);
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
            CheckBox chkMSVisible = (CheckBox)item.FindControl("chkMSVisible");
            DataRow dr = dt.NewRow();
            dr["ID"] = lblID.Text;
            dr["Portal"] = false;//chkPortal.Checked;
            dr["Remarks"] = txtRemarks.Text;
            dr["MSVisible"] = chkMSVisible.Checked;
            dt.Rows.Add(dr);
        }

        return dt;
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



    private void RowSelectDocuments()
    {
        if (hdnEditDocument.Value == "N")
        {
            foreach (GridDataItem item in RadGrid_Documents.Items)
            {
                //CheckBox chkSelected = (CheckBox)item.FindControl("chkSelect");
                //CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
                TextBox txtremarks = (TextBox)item.FindControl("txtremarks");
                //chkSelected.Enabled = 
                //chkPortal.Enabled = false;
                txtremarks.Enabled = false;
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
        GetDocuments();

    }
    private void GetDocuments()
    {
        if (Request.QueryString["id"] != null)
        {
            objMapData.Screen = "RPO";
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

    private void UpdateTempDateWhenCreatingNewRPO(string strRPOId)
    {
        var apRPOId = Convert.ToInt32(strRPOId);
        if (ViewState["TempUploadDirectory"] == null)
        {
            return;
        }
        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;
        var tempDirectory = "RPODocs\\" + ViewState["TempUploadDirectory"] as string;
        var newDirectory = "RPODocs\\" + "RPO_" + strRPOId;

        if (tempAttachedFiles == null)
        {
            return;
        }

        var sourceDirectory = GetUploadDirectory(tempDirectory);
        var destDirectory = GetUploadDirectory(newDirectory);
        Directory.Move(sourceDirectory, destDirectory);

        foreach (DataRow row in tempAttachedFiles.Rows)
        {
            objMapData.Screen = "RPO";
            objMapData.TicketID = apRPOId;
            objMapData.TempId = "0";
            objMapData.FileName = row.Field<string>("filename");
            objMapData.DocTypeMIME = row.Field<string>("doctype");
            objMapData.FilePath = row.Field<string>("Path").Replace(sourceDirectory, destDirectory);
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
        objMapData.Screen = "RPO";
        objMapData.TicketID = apRPOId;
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

    private void DocumentPermission()
    {

        if (Convert.ToString(Session["type"]) != "am" && Convert.ToString(Session["type"]) != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            //Document--------------------->

            string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
            //  hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
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
            else
            {
                lnkUploadDoc.Enabled = true;
            }
            // pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;
        }
    }

}